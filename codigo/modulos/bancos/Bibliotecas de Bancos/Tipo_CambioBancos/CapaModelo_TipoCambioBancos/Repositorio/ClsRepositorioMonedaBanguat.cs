using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CapaModelo_TipoCambio.Entidades;
using CapaModelo_TipoCambio.Servicios;

namespace CapaModelo_TipoCambio.Repositorio
{
    public interface IRepositorioMonedaBanguat
    {
        List<ClsMonedaCatalogoLocal> BancosMetConsultar(CancellationToken token);
        Task<ClsResultadoSincronizacionMonedas> BancosMetSincronizarAsync(CancellationToken token);
        void BancosMetEstablecerActiva(long id, bool activa, CancellationToken token);
    }

    public class ClsRepositorioMonedaBanguat : IRepositorioMonedaBanguat
    {
        private readonly string _conexion;
        private readonly ClsServicioBanguat _servicio;
        private const string Tabla = "tblMoneda";

        public ClsRepositorioMonedaBanguat() : this("Dsn=EmbutidosS.A", new ClsServicioBanguat()) { }
        public ClsRepositorioMonedaBanguat(string conexion, ClsServicioBanguat servicio)
        {
            if (string.IsNullOrWhiteSpace(conexion)) throw new ArgumentException("Indica la conexión a MySQL.");
            _conexion = conexion;
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
        }

        public List<ClsMonedaCatalogoLocal> BancosMetConsultar(CancellationToken token)
        {
            using (var conexion = Abrir(token)) return LeerMonedas(conexion, null, false, token);
        }

        public async Task<ClsResultadoSincronizacionMonedas> BancosMetSincronizarAsync(CancellationToken token)
        {
            // La consulta remota termina antes de abrir la transacción y bloquear las cuentas.
            var catalogo = await _servicio.BancosMetObtenerMonedasAsync(token).ConfigureAwait(false);
            ValidarCatalogo(catalogo);
            return await Task.Run(() => Sincronizar(catalogo, token), token).ConfigureAwait(false);
        }

        public void BancosMetEstablecerActiva(long id, bool activa, CancellationToken token)
        {
            if (id <= 0) throw new ArgumentException("Selecciona una moneda válida.");
            using (var conexion = Abrir(token))
            using (var transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    string codigo = null;
                    Leer(conexion, transaccion, "SELECT codigoMoneda FROM tblMoneda WHERE Pk_Moneda = ? FOR UPDATE",
                        new[] { Numero(id) }, token, r => codigo = Convert.ToString(r[0]).Trim().ToUpperInvariant());
                    if (codigo == null) throw new InvalidOperationException("La moneda ya no existe. Vuelve a consultar.");
                    if (ClsCatalogoMonedasBanguat.BancosMetIdDeCodigo(codigo) != id)
                        throw new InvalidOperationException("Primero sincroniza el catálogo para utilizar los identificadores de Banguat.");
                    Ejecutar(conexion, transaccion, "UPDATE tblMoneda SET is_active = ? WHERE Pk_Moneda = ?",
                        new[] { Numero(activa ? 1 : 0), Numero(id) }, token);
                    token.ThrowIfCancellationRequested();
                    transaccion.Commit();
                }
                catch { IntentarRevertir(transaccion); throw; }
            }
        }

        private ClsResultadoSincronizacionMonedas Sincronizar(List<ClsMoneda> catalogo, CancellationToken token)
        {
            using (var conexion = Abrir(token))
            {
                string esquema = Convert.ToString(Escalar(conexion, null, "SELECT DATABASE()", null, token));
                if (string.IsNullOrWhiteSpace(esquema)) throw new InvalidOperationException("La conexión no tiene una base de datos seleccionada.");
                string bloqueo;
                using (var hash = SHA256.Create())
                    bloqueo = "banguat.moneda." + BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(esquema))).Replace("-", "").Substring(0, 32);
                bool adquirido = Convert.ToInt32(Escalar(conexion, null, "SELECT GET_LOCK(?, 10)", new[] { Texto(bloqueo, 64) }, token)) == 1;
                if (!adquirido) throw new InvalidOperationException("Otra sincronización está en curso. Reintenta cuando termine.");
                try
                {
                    using (var transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            ValidarMotor(conexion, transaccion, esquema, Tabla, token);
                            var originales = LeerMonedas(conexion, transaccion, true, token);
                            var apiPorCodigo = catalogo.ToDictionary(m => m.Codigo, StringComparer.Ordinal);
                            foreach (var moneda in originales)
                                if (!apiPorCodigo.ContainsKey(moneda.Codigo))
                                    throw new InvalidOperationException("La moneda local " + moneda.Codigo + " no tiene correspondencia en Banguat. No se modificó el catálogo.");
                            if (originales.Select(m => m.Codigo).Distinct(StringComparer.Ordinal).Count() != originales.Count)
                                throw new InvalidOperationException("El catálogo local contiene códigos de moneda duplicados.");
                            var porCodigo = originales.ToDictionary(m => m.Codigo, StringComparer.Ordinal);
                            var cambios = originales.Where(m => m.Id != apiPorCodigo[m.Codigo].IdBanguat).ToList();
                            var referencias = ObtenerReferencias(conexion, transaccion, esquema, cambios.Select(m => m.Id).ToList(), token);
                            RechazarTriggers(conexion, transaccion, esquema, Tabla, null, token);
                            var resultado = new ClsResultadoSincronizacionMonedas { TotalApi = catalogo.Count, IdsMigrados = cambios.Count };
                            long temporal = Math.Max(originales.Count == 0 ? 0 : originales.Max(m => m.Id), catalogo.Max(m => (long)m.IdBanguat));
                            var temporales = new Dictionary<string, long>(StringComparer.Ordinal);
                            int indice = 0;
                            foreach (var moneda in cambios)
                            {
                                temporal = checked(temporal + 1);
                                temporales.Add(moneda.Codigo, temporal);
                                // El alias y el ID temporal se generan aquí, no provienen de un campo de usuario.
                                string codigoTemporal = "0" + (indice++).ToString("D2", CultureInfo.InvariantCulture);
                                string sqlClonar = "INSERT INTO tblMoneda (Pk_Moneda,codigoMoneda,nombreMoneda,simboloMoneda,is_active,created_at,updated_at) " +
                                    "SELECT " + temporal.ToString(CultureInfo.InvariantCulture) + ", '" + codigoTemporal + "', " +
                                    "nombreMoneda,simboloMoneda,is_active,created_at,updated_at FROM tblMoneda WHERE Pk_Moneda = ?";
                                if (Ejecutar(conexion, transaccion, sqlClonar, new[] { Numero(moneda.Id) }, token) != 1)
                                    throw new InvalidOperationException("No se pudo preparar la migración de la moneda " + moneda.Codigo + ".");
                                foreach (var referencia in referencias.Where(r => r.Filas > 0))
                                    resultado.ReferenciasMigradas += MoverReferencias(conexion, transaccion, referencia, moneda.Id, temporal, token);
                            }
                            foreach (var moneda in cambios)
                                if (Ejecutar(conexion, transaccion, "DELETE FROM tblMoneda WHERE Pk_Moneda = ?", new[] { Numero(moneda.Id) }, token) != 1)
                                    throw new InvalidOperationException("No se pudo sustituir el identificador de " + moneda.Codigo + ".");

                            foreach (var moneda in catalogo.OrderBy(m => m.IdBanguat))
                            {
                                ClsMonedaCatalogoLocal anterior;
                                bool existia = porCodigo.TryGetValue(moneda.Codigo, out anterior);
                                if (!existia || temporales.ContainsKey(moneda.Codigo))
                                {
                                    bool activa = existia ? anterior.Activa : ClsCatalogoMonedasBanguat.BancosMetClase(moneda.Codigo) == "Moneda";
                                    string simbolo = existia ? anterior.Simbolo : moneda.Codigo;
                                    if (Ejecutar(conexion, transaccion,
                                        "INSERT INTO tblMoneda (Pk_Moneda,codigoMoneda,nombreMoneda,simboloMoneda,is_active) VALUES (?,?,?,?,?)",
                                        new[] { Numero(moneda.IdBanguat), Texto(moneda.Codigo, 3), Texto(moneda.Nombre, 255), Texto(simbolo, 8), Numero(activa ? 1 : 0) }, token) != 1)
                                        throw new InvalidOperationException("No se pudo incorporar " + moneda.Codigo + ".");
                                    if (!existia) resultado.Agregadas++;
                                    else
                                    {
                                        Ejecutar(conexion, transaccion,
                                            "UPDATE tblMoneda c JOIN tblMoneda t ON t.Pk_Moneda = ? " +
                                            "SET c.created_at = t.created_at, c.updated_at = t.updated_at WHERE c.Pk_Moneda = ?",
                                            new[] { Numero(temporales[moneda.Codigo]), Numero(moneda.IdBanguat) }, token);
                                    }
                                }
                                else if (!string.Equals(anterior.Nombre, moneda.Nombre, StringComparison.Ordinal))
                                {
                                    Ejecutar(conexion, transaccion, "UPDATE tblMoneda SET nombreMoneda = ?, updated_at = updated_at WHERE Pk_Moneda = ?",
                                        new[] { Texto(moneda.Nombre, 255), Numero(moneda.IdBanguat) }, token);
                                }
                                if (existia && !string.Equals(anterior.Nombre, moneda.Nombre, StringComparison.Ordinal)) resultado.Actualizadas++;
                            }
                            foreach (var moneda in cambios)
                            {
                                long idTemporal = temporales[moneda.Codigo];
                                foreach (var referencia in referencias.Where(r => r.Filas > 0))
                                    MoverReferencias(conexion, transaccion, referencia, idTemporal, apiPorCodigo[moneda.Codigo].IdBanguat, token);
                                if (Ejecutar(conexion, transaccion, "DELETE FROM tblMoneda WHERE Pk_Moneda = ?", new[] { Numero(idTemporal) }, token) != 1)
                                    throw new InvalidOperationException("No se pudo terminar la migración de " + moneda.Codigo + ".");
                            }
                            var finales = LeerMonedas(conexion, transaccion, false, token);
                            if (finales.Count != catalogo.Count || finales.Any(m => !apiPorCodigo.ContainsKey(m.Codigo) || m.Id != apiPorCodigo[m.Codigo].IdBanguat))
                                throw new InvalidOperationException("La verificación final del catálogo no coincide con Banguat. Se revirtió la operación.");
                            token.ThrowIfCancellationRequested();
                            transaccion.Commit();
                            resultado.Mensaje = "Catálogo sincronizado: " + resultado.TotalApi + " monedas y unidades de cuenta; " +
                                resultado.Agregadas + " nuevas y " + resultado.IdsMigrados + " identificadores migrados. " +
                                "Las cuentas y sus importes se conservaron.";
                            return resultado;
                        }
                        catch { IntentarRevertir(transaccion); throw; }
                    }
                }
                finally
                {
                    try { Escalar(conexion, null, "SELECT RELEASE_LOCK(?)", new[] { Texto(bloqueo, 64) }, CancellationToken.None); }
                    catch (OdbcException) { /* Cerrar la conexión también libera el bloqueo. */ }
                }
            }
        }

        private sealed class Referencia
        {
            public string Esquema, Tabla, Columna;
            public long Filas;
            public List<string> Automaticas = new List<string>();
        }

        private static List<Referencia> ObtenerReferencias(OdbcConnection conexion, OdbcTransaction tx, string esquema,
            List<long> ids, CancellationToken token)
        {
            var grupos = new Dictionary<string, List<Referencia>>(StringComparer.OrdinalIgnoreCase);
            Leer(conexion, tx, "SELECT TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME,CONSTRAINT_NAME,REFERENCED_COLUMN_NAME " +
                "FROM information_schema.KEY_COLUMN_USAGE WHERE REFERENCED_TABLE_SCHEMA = ? " +
                "AND LOWER(REFERENCED_TABLE_NAME) = 'tblmoneda' ORDER BY TABLE_SCHEMA,TABLE_NAME,CONSTRAINT_NAME,ORDINAL_POSITION",
                new[] { Texto(esquema, 64) }, token, r =>
                {
                    if (!string.Equals(Convert.ToString(r[4]), "Pk_Moneda", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("Hay una relación de moneda que requiere una migración específica.");
                    string clave = Convert.ToString(r[0]) + "." + Convert.ToString(r[1]) + "." + Convert.ToString(r[3]);
                    List<Referencia> lista;
                    if (!grupos.TryGetValue(clave, out lista)) grupos.Add(clave, lista = new List<Referencia>());
                    lista.Add(new Referencia { Esquema = Convert.ToString(r[0]), Tabla = Convert.ToString(r[1]), Columna = Convert.ToString(r[2]) });
                });
            if (grupos.Values.Any(g => g.Count != 1))
                throw new InvalidOperationException("Hay una relación compuesta de moneda que requiere una migración específica.");
            var referencias = grupos.Values.Select(g => g[0]).GroupBy(r => r.Esquema + "." + r.Tabla + "." + r.Columna,
                StringComparer.OrdinalIgnoreCase).Select(g => g.First()).ToList();
            if (ids.Count == 0) return referencias;
            // Una referencia antigua sin FK no debe quedar apuntando a otra moneda
            // después de reasignar los IDs. Se exige revisarla antes de migrar.
            var sinRelacion = new List<Referencia>();
            Leer(conexion, tx, "SELECT TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME FROM information_schema.COLUMNS " +
                "WHERE TABLE_SCHEMA = ? AND LOWER(TABLE_NAME) <> 'tblmoneda' " +
                "AND LOWER(COLUMN_NAME) LIKE '%moneda%' AND DATA_TYPE IN ('tinyint','smallint','mediumint','int','bigint')",
                new[] { Texto(esquema, 64) }, token, r =>
                {
                    var candidata = new Referencia { Esquema = Convert.ToString(r[0]), Tabla = Convert.ToString(r[1]), Columna = Convert.ToString(r[2]) };
                    if (!referencias.Any(f => string.Equals(f.Esquema, candidata.Esquema, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(f.Tabla, candidata.Tabla, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(f.Columna, candidata.Columna, StringComparison.OrdinalIgnoreCase))) sinRelacion.Add(candidata);
                });
            foreach (var candidata in sinRelacion)
            {
                long coincidencias = Convert.ToInt64(Escalar(conexion, tx, "SELECT COUNT(*) FROM " +
                    Identificador(candidata.Esquema) + "." + Identificador(candidata.Tabla) + " WHERE " +
                    Identificador(candidata.Columna) + " IN (" + string.Join(",", ids.Select(id => "?")) + ")",
                    ids.Select(Numero).ToArray(), token));
                if (coincidencias > 0) throw new InvalidOperationException("La columna " + candidata.Tabla + "." + candidata.Columna +
                    " contiene referencias de moneda sin una clave foránea. Revisa esa relación antes de sincronizar.");
            }
            foreach (var referencia in referencias)
            {
                ValidarMotor(conexion, tx, referencia.Esquema, referencia.Tabla, token);
                string campo = Identificador(referencia.Columna);
                string tabla = Identificador(referencia.Esquema) + "." + Identificador(referencia.Tabla);
                Leer(conexion, tx, "SELECT " + campo + " FROM " + tabla + " WHERE " + campo + " IN (" +
                    string.Join(",", ids.Select(id => "?")) + ") FOR UPDATE", ids.Select(Numero).ToArray(), token, r => referencia.Filas++);
                if (referencia.Filas == 0) continue;
                RechazarTriggers(conexion, tx, referencia.Esquema, referencia.Tabla, "UPDATE", token);
                long dependientes = Convert.ToInt64(Escalar(conexion, tx,
                    "SELECT COUNT(*) FROM information_schema.KEY_COLUMN_USAGE WHERE REFERENCED_TABLE_SCHEMA = ? AND REFERENCED_TABLE_NAME = ? AND REFERENCED_COLUMN_NAME = ?",
                    new[] { Texto(referencia.Esquema, 64), Texto(referencia.Tabla, 64), Texto(referencia.Columna, 64) }, token));
                if (dependientes != 0)
                    throw new InvalidOperationException("La relación " + referencia.Tabla + "." + referencia.Columna + " tiene dependencias adicionales y requiere una migración específica.");
                Leer(conexion, tx, "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = ? AND TABLE_NAME = ? AND LOWER(EXTRA) LIKE '%on update%'",
                    new[] { Texto(referencia.Esquema, 64), Texto(referencia.Tabla, 64) }, token,
                    r => referencia.Automaticas.Add(Convert.ToString(r[0])));
            }
            return referencias;
        }

        private static long MoverReferencias(OdbcConnection conexion, OdbcTransaction tx, Referencia referencia,
            long anterior, long nuevo, CancellationToken token)
        {
            string campo = Identificador(referencia.Columna);
            string asignaciones = campo + " = ?";
            foreach (string automatica in referencia.Automaticas)
                asignaciones += ", " + Identificador(automatica) + " = " + Identificador(automatica);
            return Ejecutar(conexion, tx, "UPDATE " + Identificador(referencia.Esquema) + "." + Identificador(referencia.Tabla) +
                " SET " + asignaciones + " WHERE " + campo + " = ?", new[] { Numero(nuevo), Numero(anterior) }, token);
        }

        private static void ValidarMotor(OdbcConnection conexion, OdbcTransaction tx, string esquema, string tabla, CancellationToken token)
        {
            string motor = Convert.ToString(Escalar(conexion, tx, "SELECT ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA = ? AND LOWER(TABLE_NAME) = ?",
                new[] { Texto(esquema, 64), Texto(tabla.ToLowerInvariant(), 64) }, token));
            if (!string.Equals(motor, "InnoDB", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("La tabla " + esquema + "." + tabla + " debe usar InnoDB antes de sincronizar sus identificadores.");
        }

        private static void RechazarTriggers(OdbcConnection conexion, OdbcTransaction tx, string esquema,
            string tabla, string evento, CancellationToken token)
        {
            string trigger = null;
            var parametros = new List<OdbcParameter> { Texto(esquema, 64), Texto(tabla.ToLowerInvariant(), 64) };
            string sql = "SELECT TRIGGER_NAME FROM information_schema.TRIGGERS WHERE EVENT_OBJECT_SCHEMA = ? AND LOWER(EVENT_OBJECT_TABLE) = ?";
            if (evento != null) { sql += " AND EVENT_MANIPULATION = ?"; parametros.Add(Texto(evento, 16)); }
            Leer(conexion, tx, sql, parametros, token, r => trigger = Convert.ToString(r[0]));
            if (trigger != null) throw new InvalidOperationException("El disparador " + trigger + " de " + tabla + " requiere revisión antes de cambiar los identificadores de moneda. No se modificaron datos.");
        }

        private static List<ClsMonedaCatalogoLocal> LeerMonedas(OdbcConnection conexion, OdbcTransaction tx, bool bloquear, CancellationToken token)
        {
            var monedas = new List<ClsMonedaCatalogoLocal>();
            Leer(conexion, tx, "SELECT Pk_Moneda,codigoMoneda,nombreMoneda,simboloMoneda,is_active,created_at,updated_at " +
                "FROM tblMoneda ORDER BY Pk_Moneda" + (bloquear ? " FOR UPDATE" : ""), null, token,
                r => monedas.Add(new ClsMonedaCatalogoLocal
                {
                    Id = Convert.ToInt64(r[0]), Codigo = Convert.ToString(r[1]).Trim().ToUpperInvariant(),
                    Nombre = Convert.ToString(r[2]), Simbolo = Convert.ToString(r[3]), Activa = Convert.ToBoolean(r[4]),
                    FechaCreacion = Convert.ToDateTime(r[5]), FechaActualizacion = Convert.ToDateTime(r[6])
                }));
            return monedas;
        }

        private static void ValidarCatalogo(List<ClsMoneda> catalogo)
        {
            if (catalogo == null || catalogo.Count != ClsCatalogoMonedasBanguat.BancosCodigosPorId.Count ||
                catalogo.Any(m => m == null || string.IsNullOrWhiteSpace(m.Codigo) || string.IsNullOrWhiteSpace(m.Nombre) || m.Nombre.Length > 255 ||
                    ClsCatalogoMonedasBanguat.BancosMetIdDeCodigo(m.Codigo) != m.IdBanguat) ||
                catalogo.Select(m => m.Codigo).Distinct(StringComparer.Ordinal).Count() != catalogo.Count ||
                catalogo.Select(m => m.IdBanguat).Distinct().Count() != catalogo.Count)
                throw new InvalidOperationException("Banguat no devolvió el catálogo completo con identificadores válidos. No se modificó la base de datos.");
        }

        private OdbcConnection Abrir(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var conexion = new OdbcConnection(_conexion);
            try { conexion.Open(); token.ThrowIfCancellationRequested(); return conexion; }
            catch { conexion.Dispose(); throw; }
        }
        private static string Identificador(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto) || texto.Any(c => !(char.IsLetterOrDigit(c) || c == '_')))
                throw new InvalidOperationException("La relación contiene un identificador SQL no admitido.");
            return "`" + texto + "`";
        }
        private static OdbcParameter Numero(long valor) { return new OdbcParameter { OdbcType = OdbcType.BigInt, Value = valor }; }
        private static OdbcParameter Texto(string valor, int longitud) { return new OdbcParameter { OdbcType = OdbcType.VarChar, Size = longitud, Value = valor }; }
        private static OdbcCommand Comando(OdbcConnection conexion, OdbcTransaction tx, string sql, IEnumerable<OdbcParameter> parametros)
        {
            var comando = new OdbcCommand(sql, conexion, tx) { CommandTimeout = 30 };
            if (parametros != null) foreach (var parametro in parametros) comando.Parameters.Add(parametro);
            return comando;
        }
        private static object Escalar(OdbcConnection conexion, OdbcTransaction tx, string sql, IEnumerable<OdbcParameter> parametros, CancellationToken token)
        {
            using (var comando = Comando(conexion, tx, sql, parametros))
            using (token.Register(() => Cancelar(comando))) { token.ThrowIfCancellationRequested(); return comando.ExecuteScalar(); }
        }
        private static int Ejecutar(OdbcConnection conexion, OdbcTransaction tx, string sql, IEnumerable<OdbcParameter> parametros, CancellationToken token)
        {
            using (var comando = Comando(conexion, tx, sql, parametros))
            using (token.Register(() => Cancelar(comando))) { token.ThrowIfCancellationRequested(); return comando.ExecuteNonQuery(); }
        }
        private static void Leer(OdbcConnection conexion, OdbcTransaction tx, string sql, IEnumerable<OdbcParameter> parametros,
            CancellationToken token, Action<OdbcDataReader> fila)
        {
            using (var comando = Comando(conexion, tx, sql, parametros))
            using (token.Register(() => Cancelar(comando)))
            {
                token.ThrowIfCancellationRequested();
                using (var lector = comando.ExecuteReader())
                    while (lector.Read()) { token.ThrowIfCancellationRequested(); fila(lector); }
            }
        }
        private static void Cancelar(OdbcCommand comando) { try { comando.Cancel(); } catch (OdbcException) { } catch (InvalidOperationException) { } }
        private static void IntentarRevertir(OdbcTransaction tx) { try { tx.Rollback(); } catch (OdbcException) { } catch (InvalidOperationException) { } }
    }
}
