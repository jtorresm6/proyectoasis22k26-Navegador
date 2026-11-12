using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Text;
using System.Threading;
using CapaModelo_DisponibilidadDiariaBancos.Entidades;

namespace CapaModelo_DisponibilidadDiariaBancos.Repositorio
{
    public interface IRepositorioDisponibilidad
    {
        ClsCatalogosDisponibilidad BancosMetObtenerCatalogos(CancellationToken token);
        ClsConsultaDisponibilidad BancosMetConsultar(DateTime fecha, long? bancoId, long? cuentaId, string moneda, CancellationToken token);
        void BancosMetGuardar(ClsDisponibilidad registro, bool reemplazar, CancellationToken token);
    }

    // Sólo consulta y guarda cortes reales. No genera aperturas ni reservas ficticias.
    public class ClsRepositorioDisponibilidad : IRepositorioDisponibilidad
    {
        private const decimal ImporteMaximo = 99999999.9999m;
        private readonly string _cadenaConexion;

        public ClsRepositorioDisponibilidad() : this("Dsn=EmbutidosS.A") { }
        public ClsRepositorioDisponibilidad(string cadenaConexion)
        {
            if (string.IsNullOrWhiteSpace(cadenaConexion)) throw new ArgumentException("Indica la conexión a MySQL.");
            _cadenaConexion = cadenaConexion;
        }

        public ClsCatalogosDisponibilidad BancosMetObtenerCatalogos(CancellationToken token)
        {
            using (OdbcConnection conexion = AbrirConexion(token))
            {
                var resultado = new ClsCatalogosDisponibilidad
                {
                    Bancos = new List<ClsOpcionCatalogo>(), Cuentas = new List<ClsCuentaCatalogo>(),
                    Monedas = new List<ClsOpcionCatalogo>()
                };
                Leer(conexion, null, "SELECT Pk_Banco, codigoBanco, nombreBanco FROM tblBanco " +
                    "WHERE is_active = 1 ORDER BY nombreBanco, Pk_Banco", null, token,
                    lector => resultado.Bancos.Add(new ClsOpcionCatalogo
                    {
                        Id = Convert.ToInt64(lector[0]), Codigo = Convert.ToString(lector[1]),
                        Nombre = Convert.ToString(lector[2])
                    }));
                Leer(conexion, null, "SELECT Pk_Moneda, codigoMoneda, nombreMoneda FROM tblMoneda " +
                    "WHERE is_active = 1 ORDER BY codigoMoneda", null, token,
                    lector => resultado.Monedas.Add(new ClsOpcionCatalogo
                    {
                        Id = Convert.ToInt64(lector[0]), Codigo = Convert.ToString(lector[1]).Trim().ToUpperInvariant(),
                        Nombre = Convert.ToString(lector[1]).Trim() + " - " + Convert.ToString(lector[2])
                    }));
                Leer(conexion, null,
                    "SELECT c.Pk_CuentaBancaria, c.Fk_Banco_Codigo, c.numeroCuentaBancaria, " +
                    "c.nombreCuentaBancaria, m.codigoMoneda, b.nombreBanco " +
                    "FROM tblCuentaBancaria c JOIN tblBanco b ON b.Pk_Banco = c.Fk_Banco_Codigo " +
                    "JOIN tblMoneda m ON m.Pk_Moneda = c.Fk_Moneda_Codigo " +
                    "JOIN tblTipoCuentaBancaria t ON t.Pk_TipoCuentaBancaria = c.Fk_TipoCuentaBancaria_Codigo " +
                    "WHERE c.is_active = 1 AND b.is_active = 1 AND m.is_active = 1 AND t.is_active = 1 " +
                    "ORDER BY b.nombreBanco, c.numeroCuentaBancaria, c.Pk_CuentaBancaria",
                    null, token, lector => resultado.Cuentas.Add(new ClsCuentaCatalogo
                    {
                        Id = Convert.ToInt64(lector[0]), BancoId = Convert.ToInt64(lector[1]),
                        NumeroCuenta = Convert.ToString(lector[2]), Codigo = Convert.ToString(lector[2]),
                        Nombre = Convert.ToString(lector[2]) + " - " + Convert.ToString(lector[3]) + " (" + Convert.ToString(lector[5]) + ")",
                        Moneda = Convert.ToString(lector[4]).Trim().ToUpperInvariant()
                    }));
                resultado.TieneDisponibilidades = ExisteTabla(conexion, "tblDisponibilidadDiaria", token);
                resultado.Mensaje = resultado.TieneDisponibilidades ? "Catálogos recibidos de MySQL." :
                    "Los catálogos están disponibles. Falta preparar la tabla de disponibilidades diarias.";
                return resultado;
            }
        }

        public ClsConsultaDisponibilidad BancosMetConsultar(
            DateTime fecha, long? bancoId, long? cuentaId, string moneda, CancellationToken token)
        {
            ValidarFiltros(fecha, bancoId, cuentaId, moneda);
            moneda = NormalizarMoneda(moneda);
            var resultado = new ClsConsultaDisponibilidad
            {
                Fecha = fecha.Date, Detalle = new List<ClsDisponibilidad>(),
                Compromisos = new List<ClsCompromiso>(), MonedaResumen = moneda ?? "GTQ"
            };
            using (OdbcConnection conexion = AbrirConexion(token))
            {
                resultado.TieneTabla = ExisteTabla(conexion, "tblDisponibilidadDiaria", token);
                if (!resultado.TieneTabla)
                {
                    resultado.Mensaje = "Todavía no existe la tabla de disponibilidades diarias. No se utilizan saldos de ejemplo.";
                    return resultado;
                }
                bool conservaMoneda = ExisteColumna(conexion, "tblDisponibilidadDiaria", "codigoMonedaDisponibilidadDiaria", token);
                string columnaMoneda = conservaMoneda ? "d.codigoMonedaDisponibilidadDiaria" : "m.codigoMoneda";
                bool tieneAuditoria = TieneEsquemaAuditoria(conexion, token);
                string columnasAuditoria = tieneAuditoria
                    ? "d.versionDisponibilidadDiaria, d.usuarioRegistroIdDisponibilidadDiaria, " +
                        "d.usuarioRegistroDisponibilidadDiaria, d.fechaActualizacionDisponibilidadDiaria"
                    : "0, NULL, NULL, NULL";
                string sql =
                    "SELECT c.Pk_CuentaBancaria, b.Pk_Banco, b.nombreBanco, c.numeroCuentaBancaria, " +
                    "t.nombreTipoCuentaBancaria, " + columnaMoneda + ", d.fechaDisponibilidadDiaria, " +
                    "d.saldoInicialDisponibilidadDiaria, d.ingresosDisponibilidadDiaria, d.egresosDisponibilidadDiaria, " +
                    "d.comprometidoDisponibilidadDiaria, d.restringidoDisponibilidadDiaria, d.saldoDisponibleDisponibilidadDiaria, " +
                    "d.tipoCambioAplicadoDisponibilidadDiaria, d.saldoDisponibleGTQDisponibilidadDiaria, " +
                    "d.fuenteDisponibilidadDiaria, d.fechaGeneracionDisponibilidadDiaria, " + columnasAuditoria + " " +
                    "FROM tblDisponibilidadDiaria d JOIN tblCuentaBancaria c ON c.Pk_CuentaBancaria = d.Fk_CuentaBancaria_Codigo " +
                    "JOIN tblBanco b ON b.Pk_Banco = c.Fk_Banco_Codigo " +
                    "JOIN tblMoneda m ON m.Pk_Moneda = c.Fk_Moneda_Codigo " +
                    "JOIN tblTipoCuentaBancaria t ON t.Pk_TipoCuentaBancaria = c.Fk_TipoCuentaBancaria_Codigo " +
                    "WHERE d.fechaDisponibilidadDiaria = ?";
                var parametros = new List<OdbcParameter> { Parametro(OdbcType.Date, fecha.Date) };
                if (bancoId.HasValue) { sql += " AND b.Pk_Banco = ?"; parametros.Add(Parametro(OdbcType.BigInt, bancoId.Value)); }
                if (cuentaId.HasValue) { sql += " AND c.Pk_CuentaBancaria = ?"; parametros.Add(Parametro(OdbcType.BigInt, cuentaId.Value)); }
                if (moneda != null) { sql += " AND " + columnaMoneda + " = ?"; parametros.Add(Parametro(OdbcType.VarChar, moneda)); }
                sql += " ORDER BY b.nombreBanco, c.numeroCuentaBancaria, c.Pk_CuentaBancaria";
                // Los cortes históricos conservan filas aunque después se desactive una cuenta.
                Leer(conexion, null, sql, parametros, token, lector =>
                {
                    var registro = new ClsDisponibilidad
                    {
                        CuentaId = Convert.ToInt64(lector[0]), BancoId = Convert.ToInt64(lector[1]),
                        Banco = Convert.ToString(lector[2]), NumeroCuenta = Convert.ToString(lector[3]),
                        TipoCuenta = Convert.ToString(lector[4]), Moneda = Convert.ToString(lector[5]).Trim().ToUpperInvariant(),
                        FechaCorte = Convert.ToDateTime(lector[6]).Date,
                        SaldoInicial = Convert.ToDecimal(lector[7]), Ingresos = Convert.ToDecimal(lector[8]),
                        Egresos = Convert.ToDecimal(lector[9]), Comprometido = Convert.ToDecimal(lector[10]),
                        Restringido = Convert.ToDecimal(lector[11]),
                        TipoCambioAplicado = lector.IsDBNull(13) ? (decimal?)null : Convert.ToDecimal(lector[13]),
                        DisponibleGtq = lector.IsDBNull(14) ? (decimal?)null : Convert.ToDecimal(lector[14]),
                        Fuente = Convert.ToString(lector[15]), FechaGeneracion = Convert.ToDateTime(lector[16]),
                        VersionRegistro = Convert.ToInt64(lector[17]),
                        UsuarioRegistroId = lector.IsDBNull(18) ? (int?)null : Convert.ToInt32(lector[18]),
                        UsuarioRegistro = lector.IsDBNull(19) ? null : Convert.ToString(lector[19]),
                        FechaActualizacion = lector.IsDBNull(20) ? (DateTime?)null : Convert.ToDateTime(lector[20])
                    };
                    if (registro.SaldoInicial < 0m || registro.Ingresos < 0m || registro.Egresos < 0m ||
                        registro.Comprometido < 0m || registro.Restringido < 0m ||
                        registro.Disponible != Convert.ToDecimal(lector[12]))
                        throw new InvalidOperationException("El corte de la cuenta " + registro.NumeroCuenta +
                            " tiene importes inconsistentes. Revisa sus componentes.");
                    if (registro.Moneda == "GTQ" && registro.TipoCambioAplicado.HasValue &&
                        registro.TipoCambioAplicado.Value != 1m)
                        throw new InvalidOperationException("La cuenta en GTQ tiene una tasa registrada distinta de 1.");
                    if (registro.TipoCambioAplicado.HasValue && registro.TipoCambioAplicado.Value > 0m &&
                        registro.DisponibleGtq.HasValue && registro.DisponibleGtq.Value !=
                        Math.Round(registro.Disponible * registro.TipoCambioAplicado.Value, 4, MidpointRounding.AwayFromZero))
                        throw new InvalidOperationException("El equivalente en GTQ no coincide con la tasa guardada para la cuenta " +
                            registro.NumeroCuenta + ".");
                    resultado.Detalle.Add(registro);
                });
                resultado.Mensaje = resultado.Detalle.Count == 0
                    ? "No hay registros para la fecha y los filtros seleccionados."
                    : "No hay detalle por beneficiario; el monto comprometido corresponde al registro diario.";
                if (!conservaMoneda) resultado.Mensaje += " La estructura antigua no conserva la moneda histórica; " +
                    "se muestra la moneda actual de la cuenta. Actualiza el esquema antes de guardar.";
                if (!tieneAuditoria) resultado.Mensaje += " Falta aplicar la actualización de auditoría y versiones antes de guardar.";
                return resultado;
            }
        }

        public void BancosMetGuardar(ClsDisponibilidad registro, bool reemplazar, CancellationToken token)
        {
            BancosMetValidarRegistro(registro);
            token.ThrowIfCancellationRequested();
            using (OdbcConnection conexion = AbrirConexion(token))
            {
                if (!ExisteTabla(conexion, "tblDisponibilidadDiaria", token) ||
                    !ExisteColumna(conexion, "tblDisponibilidadDiaria", "codigoMonedaDisponibilidadDiaria", token) ||
                    !TieneEsquemaAuditoria(conexion, token))
                    throw new InvalidOperationException("Falta preparar el esquema de disponibilidad diaria con moneda, auditoría y versiones (script 002).");
                using (OdbcTransaction transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string monedaActual = null;
                        Leer(conexion, transaccion,
                            "SELECT m.codigoMoneda FROM tblCuentaBancaria c " +
                            "JOIN tblBanco b ON b.Pk_Banco = c.Fk_Banco_Codigo " +
                            "JOIN tblMoneda m ON m.Pk_Moneda = c.Fk_Moneda_Codigo " +
                            "JOIN tblTipoCuentaBancaria t ON t.Pk_TipoCuentaBancaria = c.Fk_TipoCuentaBancaria_Codigo " +
                            "WHERE c.Pk_CuentaBancaria = ? AND c.is_active = 1 AND b.is_active = 1 " +
                            "AND m.is_active = 1 AND t.is_active = 1 FOR UPDATE",
                            new[] { Parametro(OdbcType.BigInt, registro.CuentaId) }, token,
                            lector => monedaActual = Convert.ToString(lector[0]).Trim().ToUpperInvariant());
                        if (monedaActual == null) throw new InvalidOperationException("La cuenta no existe o no está activa.");
                        if (!string.Equals(monedaActual, registro.Moneda.Trim(), StringComparison.OrdinalIgnoreCase))
                            throw new InvalidOperationException("La moneda de la cuenta cambió. Vuelve a cargar el catálogo.");
                        DateTime? generacionAnterior = null;
                        string monedaAnterior = null;
                        long idRegistro = 0, versionAnterior = 0;
                        Leer(conexion, transaccion,
                            "SELECT fechaGeneracionDisponibilidadDiaria, codigoMonedaDisponibilidadDiaria, " +
                            "Pk_DisponibilidadDiaria, versionDisponibilidadDiaria " +
                            "FROM tblDisponibilidadDiaria " +
                            "WHERE Fk_CuentaBancaria_Codigo = ? AND fechaDisponibilidadDiaria = ? FOR UPDATE",
                            new[] { Parametro(OdbcType.BigInt, registro.CuentaId), Parametro(OdbcType.Date, registro.FechaCorte.Date) },
                            token, lector =>
                            {
                                generacionAnterior = Convert.ToDateTime(lector[0]);
                                monedaAnterior = Convert.ToString(lector[1]).Trim().ToUpperInvariant();
                                idRegistro = Convert.ToInt64(lector[2]);
                                versionAnterior = Convert.ToInt64(lector[3]);
                            });
                        if (generacionAnterior.HasValue && !reemplazar)
                            throw new ClsConflictoDisponibilidadException("Ya existe un corte para esta cuenta y fecha. Recarga antes de actualizar.");
                        if (!generacionAnterior.HasValue && reemplazar)
                            throw new ClsConflictoDisponibilidadException("El corte que intentabas actualizar ya no existe. Recarga antes de continuar.");
                        if (reemplazar && monedaAnterior != monedaActual)
                            throw new InvalidOperationException("La moneda histórica del corte es distinta de la moneda actual de la cuenta. No se puede reinterpretar ese registro.");
                        if (reemplazar && (registro.VersionRegistro <= 0 || registro.VersionRegistro != versionAnterior))
                            throw new ClsConflictoDisponibilidadException("El corte cambió después de consultarlo. Recarga antes de actualizar.");
                        if (!reemplazar && registro.VersionRegistro != 0)
                            throw new InvalidOperationException("La captura nueva no debe contener la versión de otro corte.");
                        long versionNueva = reemplazar ? checked(versionAnterior + 1) : 1;
                        // El antes se toma de la fila bloqueada, antes de modificarla.
                        // El historial no es visible hasta confirmar toda la transaccion.
                        if (reemplazar) RegistrarHistorial(conexion, transaccion, idRegistro, false, token);
                        decimal tasa = monedaActual == "GTQ" ? 1m : registro.TipoCambioAplicado.Value;
                        decimal disponibleGtq = Math.Round(registro.Disponible * tasa, 4, MidpointRounding.AwayFromZero);
                        var parametros = new List<OdbcParameter>
                        {
                            ParametroDecimal(registro.SaldoInicial), ParametroDecimal(registro.Ingresos),
                            ParametroDecimal(registro.Egresos), ParametroDecimal(registro.Comprometido),
                            ParametroDecimal(registro.Restringido), ParametroDecimal(registro.Disponible),
                            ParametroDecimal(disponibleGtq), ParametroDecimal(tasa),
                            Parametro(OdbcType.VarChar, registro.Fuente.Trim()), Parametro(OdbcType.Char, monedaActual),
                            Parametro(OdbcType.Int, registro.UsuarioRegistroId), Parametro(OdbcType.VarChar, registro.UsuarioRegistro)
                        };
                        string sql;
                        if (reemplazar)
                            sql = "UPDATE tblDisponibilidadDiaria SET saldoInicialDisponibilidadDiaria = ?, " +
                                "ingresosDisponibilidadDiaria = ?, egresosDisponibilidadDiaria = ?, comprometidoDisponibilidadDiaria = ?, " +
                                "restringidoDisponibilidadDiaria = ?, saldoDisponibleDisponibilidadDiaria = ?, " +
                                "saldoDisponibleGTQDisponibilidadDiaria = ?, tipoCambioAplicadoDisponibilidadDiaria = ?, " +
                                "fuenteDisponibilidadDiaria = ?, codigoMonedaDisponibilidadDiaria = ?, " +
                                "usuarioRegistroIdDisponibilidadDiaria = ?, usuarioRegistroDisponibilidadDiaria = ?, " +
                                "fechaActualizacionDisponibilidadDiaria = CURRENT_TIMESTAMP(6), " +
                                "versionDisponibilidadDiaria = versionDisponibilidadDiaria + 1 " +
                                "WHERE Fk_CuentaBancaria_Codigo = ? AND fechaDisponibilidadDiaria = ? AND versionDisponibilidadDiaria = ?";
                        else
                            sql = "INSERT INTO tblDisponibilidadDiaria (saldoInicialDisponibilidadDiaria, ingresosDisponibilidadDiaria, " +
                                "egresosDisponibilidadDiaria, comprometidoDisponibilidadDiaria, restringidoDisponibilidadDiaria, " +
                                "saldoDisponibleDisponibilidadDiaria, saldoDisponibleGTQDisponibilidadDiaria, tipoCambioAplicadoDisponibilidadDiaria, " +
                                "fuenteDisponibilidadDiaria, codigoMonedaDisponibilidadDiaria, " +
                                "usuarioRegistroIdDisponibilidadDiaria, usuarioRegistroDisponibilidadDiaria, " +
                                "Fk_CuentaBancaria_Codigo, fechaDisponibilidadDiaria, fechaActualizacionDisponibilidadDiaria) " +
                                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, CURRENT_TIMESTAMP(6))";
                        parametros.Add(Parametro(OdbcType.BigInt, registro.CuentaId));
                        parametros.Add(Parametro(OdbcType.Date, registro.FechaCorte.Date));
                        if (reemplazar) parametros.Add(Parametro(OdbcType.BigInt, versionAnterior));
                        using (OdbcCommand comando = CrearComando(conexion, transaccion, sql, parametros))
                        using (token.Register(() => Cancelar(comando)))
                        {
                            token.ThrowIfCancellationRequested();
                            int afectadas = comando.ExecuteNonQuery();
                            // La version aumenta siempre, aun cuando los importes sean iguales.
                            if (afectadas != 1)
                                throw new InvalidOperationException("No se pudo guardar un único corte diario.");
                        }
                        if (!reemplazar)
                            Leer(conexion, transaccion, "SELECT LAST_INSERT_ID()", null, token,
                                lector => idRegistro = Convert.ToInt64(lector[0]));
                        if (reemplazar) CompletarHistorial(conexion, transaccion, idRegistro, token);
                        else RegistrarHistorial(conexion, transaccion, idRegistro, true, token);
                        token.ThrowIfCancellationRequested();
                        transaccion.Commit();
                        registro.VersionRegistro = versionNueva;
                    }
                    catch
                    {
                        try { transaccion.Rollback(); }
                        catch (OdbcException) { }
                        catch (InvalidOperationException) { }
                        if (token.IsCancellationRequested) throw new OperationCanceledException(token);
                        throw;
                    }
                }
            }
        }

        public static void BancosMetValidarRegistro(ClsDisponibilidad registro)
        {
            if (registro == null) throw new ArgumentNullException(nameof(registro));
            if (registro.VersionRegistro < 0) throw new ArgumentException("La versión del corte no es válida.");
            if (registro.UsuarioRegistroId.HasValue && registro.UsuarioRegistroId.Value <= 0)
                throw new ArgumentException("El identificador de usuario no es válido.");
            if (registro.UsuarioRegistro != null && registro.UsuarioRegistro.Length > 100)
                throw new ArgumentException("El nombre del usuario no puede superar 100 caracteres.");
            ValidarFiltros(registro.FechaCorte, null, registro.CuentaId, registro.Moneda);
            if (NormalizarMoneda(registro.Moneda) == null) throw new ArgumentException("Selecciona la moneda de la cuenta.");
            if (string.IsNullOrWhiteSpace(registro.Fuente) || registro.Fuente.Trim().Length > 100)
                throw new ArgumentException("Indica la fuente o respaldo, con un máximo de 100 caracteres.");
            ValidarImporte(registro.SaldoInicial, "Saldo inicial", true);
            ValidarImporte(registro.Ingresos, "Ingresos", true);
            ValidarImporte(registro.Egresos, "Egresos", true);
            ValidarImporte(registro.Comprometido, "Comprometido", true);
            ValidarImporte(registro.Restringido, "Restringido", true);
            ValidarImporte(registro.Disponible, "Disponible", false);
            bool esGtq = string.Equals(registro.Moneda.Trim(), "GTQ", StringComparison.OrdinalIgnoreCase);
            decimal tasa = esGtq ? 1m : registro.TipoCambioAplicado ?? 0m;
            if (tasa <= 0m || (esGtq && registro.TipoCambioAplicado.HasValue && registro.TipoCambioAplicado.Value != 1m))
                throw new ArgumentException("Indica una tasa GTQ positiva de la fecha elegida. Para GTQ la tasa es 1.");
            ValidarImporte(tasa, "Tipo de cambio", true);
            ValidarImporte(Math.Round(checked(registro.Disponible * tasa), 4,
                MidpointRounding.AwayFromZero), "Disponible en GTQ", false);
        }

        private static void ValidarImporte(decimal valor, string nombre, bool noNegativo)
        {
            if ((noNegativo && valor < 0m) || valor < -ImporteMaximo || valor > ImporteMaximo)
                throw new ArgumentOutOfRangeException(nombre, nombre + " está fuera del rango DECIMAL(12,4).");
            if (valor != Math.Round(valor, 4, MidpointRounding.AwayFromZero))
                throw new ArgumentException(nombre + " admite como máximo cuatro decimales.");
        }

        private static void ValidarFiltros(DateTime fecha, long? bancoId, long? cuentaId, string moneda)
        {
            if (fecha.Date > DateTime.Today) throw new ArgumentException("No se admiten cortes futuros.");
            if (fecha.Date < new DateTime(1000, 1, 1)) throw new ArgumentException("La fecha está fuera del rango de MySQL.");
            if (bancoId.HasValue && bancoId.Value <= 0) throw new ArgumentException("Selecciona un banco válido.");
            if (cuentaId.HasValue && cuentaId.Value <= 0) throw new ArgumentException("Selecciona una cuenta válida.");
            NormalizarMoneda(moneda);
        }

        private static string NormalizarMoneda(string moneda)
        {
            if (string.IsNullOrWhiteSpace(moneda) || string.Equals(moneda.Trim(), "Todas", StringComparison.OrdinalIgnoreCase)) return null;
            moneda = moneda.Trim().ToUpperInvariant();
            if (moneda.Length != 3 || moneda[0] < 'A' || moneda[0] > 'Z' ||
                moneda[1] < 'A' || moneda[1] > 'Z' || moneda[2] < 'A' || moneda[2] > 'Z')
                throw new ArgumentException("Selecciona un código de moneda válido.");
            return moneda;
        }

        private OdbcConnection AbrirConexion(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var conexion = new OdbcConnection(_cadenaConexion);
            try { conexion.Open(); token.ThrowIfCancellationRequested(); return conexion; }
            catch { conexion.Dispose(); throw; }
        }

        private static bool TieneEsquemaAuditoria(OdbcConnection conexion, CancellationToken token)
        {
            return ExisteColumna(conexion, "tblDisponibilidadDiaria", "versionDisponibilidadDiaria", token)
                && ExisteColumna(conexion, "tblDisponibilidadDiaria", "usuarioRegistroIdDisponibilidadDiaria", token)
                && ExisteColumna(conexion, "tblDisponibilidadDiaria", "usuarioRegistroDisponibilidadDiaria", token)
                && ExisteColumna(conexion, "tblDisponibilidadDiaria", "fechaActualizacionDisponibilidadDiaria", token)
                && ExisteTabla(conexion, "tblDisponibilidadDiariaHistorial", token);
        }

        private static void RegistrarHistorial(OdbcConnection conexion, OdbcTransaction transaccion,
            long idRegistro, bool esNuevo, CancellationToken token)
        {
            var parametros = new[] { Parametro(OdbcType.BigInt, idRegistro) };
            // CREAR se ejecuta despues de insertar el corte; ACTUALIZAR toma
            // el antes de la fila bloqueada y completa el despues tras el UPDATE.
            string sql = "INSERT INTO tblDisponibilidadDiariaHistorial (Fk_DisponibilidadDiaria, " +
                "Fk_CuentaBancaria, fechaCorte, accion, versionAnterior, versionNueva, usuarioId, usuarioNombre, " +
                "monedaAnterior, monedaNueva, saldoInicialAnterior, ingresosAnterior, egresosAnterior, " +
                "comprometidoAnterior, restringidoAnterior, tasaAnterior, fuenteAnterior, saldoInicialNuevo, " +
                "ingresosNuevo, egresosNuevo, comprometidoNuevo, restringidoNuevo, tasaNueva, fuenteNueva) " +
                "SELECT d.Pk_DisponibilidadDiaria, d.Fk_CuentaBancaria_Codigo, d.fechaDisponibilidadDiaria, " +
                (esNuevo ? "'CREAR', NULL, d.versionDisponibilidadDiaria, " :
                    "'ACTUALIZAR', d.versionDisponibilidadDiaria, d.versionDisponibilidadDiaria + 1, ") +
                "d.usuarioRegistroIdDisponibilidadDiaria, d.usuarioRegistroDisponibilidadDiaria, " +
                (esNuevo ? "NULL, " : "d.codigoMonedaDisponibilidadDiaria, ") + "d.codigoMonedaDisponibilidadDiaria, " +
                (esNuevo ? "NULL, NULL, NULL, NULL, NULL, NULL, NULL, " :
                    "d.saldoInicialDisponibilidadDiaria, d.ingresosDisponibilidadDiaria, d.egresosDisponibilidadDiaria, " +
                    "d.comprometidoDisponibilidadDiaria, d.restringidoDisponibilidadDiaria, " +
                    "d.tipoCambioAplicadoDisponibilidadDiaria, d.fuenteDisponibilidadDiaria, ") +
                "d.saldoInicialDisponibilidadDiaria, d.ingresosDisponibilidadDiaria, d.egresosDisponibilidadDiaria, " +
                "d.comprometidoDisponibilidadDiaria, d.restringidoDisponibilidadDiaria, " +
                "d.tipoCambioAplicadoDisponibilidadDiaria, d.fuenteDisponibilidadDiaria " +
                "FROM tblDisponibilidadDiaria d WHERE d.Pk_DisponibilidadDiaria = ?";
            using (OdbcCommand comando = CrearComando(conexion, transaccion, sql, parametros))
            using (token.Register(() => Cancelar(comando)))
            {
                token.ThrowIfCancellationRequested();
                if (comando.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("No se pudo conservar el historial del corte. No se guardaron los cambios.");
            }
        }

        private static void CompletarHistorial(OdbcConnection conexion, OdbcTransaction transaccion,
            long idRegistro, CancellationToken token)
        {
            const string sql = "UPDATE tblDisponibilidadDiariaHistorial h " +
                "JOIN tblDisponibilidadDiaria d ON d.Pk_DisponibilidadDiaria = h.Fk_DisponibilidadDiaria " +
                "AND d.versionDisponibilidadDiaria = h.versionNueva " +
                "SET h.usuarioId = d.usuarioRegistroIdDisponibilidadDiaria, h.usuarioNombre = d.usuarioRegistroDisponibilidadDiaria, " +
                "h.monedaNueva = d.codigoMonedaDisponibilidadDiaria, h.saldoInicialNuevo = d.saldoInicialDisponibilidadDiaria, " +
                "h.ingresosNuevo = d.ingresosDisponibilidadDiaria, h.egresosNuevo = d.egresosDisponibilidadDiaria, " +
                "h.comprometidoNuevo = d.comprometidoDisponibilidadDiaria, h.restringidoNuevo = d.restringidoDisponibilidadDiaria, " +
                "h.tasaNueva = d.tipoCambioAplicadoDisponibilidadDiaria, h.fuenteNueva = d.fuenteDisponibilidadDiaria " +
                "WHERE d.Pk_DisponibilidadDiaria = ?";
            using (OdbcCommand comando = CrearComando(conexion, transaccion, sql,
                new[] { Parametro(OdbcType.BigInt, idRegistro) }))
            using (token.Register(() => Cancelar(comando)))
            {
                token.ThrowIfCancellationRequested();
                int afectadas = comando.ExecuteNonQuery();
                if (afectadas > 1)
                    throw new InvalidOperationException("No se pudo completar un único historial del corte.");
                if (afectadas == 0)
                {
                    // Un no-op es valido solamente si el snapshot ya coincide
                    // con la fila persistida de esta version, no si falta el JOIN.
                    bool coincide = false;
                    Leer(conexion, transaccion,
                        "SELECT 1 FROM tblDisponibilidadDiariaHistorial h " +
                        "JOIN tblDisponibilidadDiaria d ON d.Pk_DisponibilidadDiaria = h.Fk_DisponibilidadDiaria " +
                        "AND d.versionDisponibilidadDiaria = h.versionNueva " +
                        "WHERE d.Pk_DisponibilidadDiaria = ? " +
                        "AND h.usuarioId <=> d.usuarioRegistroIdDisponibilidadDiaria " +
                        "AND h.usuarioNombre <=> d.usuarioRegistroDisponibilidadDiaria " +
                        "AND h.monedaNueva = d.codigoMonedaDisponibilidadDiaria " +
                        "AND h.saldoInicialNuevo = d.saldoInicialDisponibilidadDiaria " +
                        "AND h.ingresosNuevo = d.ingresosDisponibilidadDiaria " +
                        "AND h.egresosNuevo = d.egresosDisponibilidadDiaria " +
                        "AND h.comprometidoNuevo = d.comprometidoDisponibilidadDiaria " +
                        "AND h.restringidoNuevo = d.restringidoDisponibilidadDiaria " +
                        "AND h.tasaNueva = d.tipoCambioAplicadoDisponibilidadDiaria " +
                        "AND h.fuenteNueva = d.fuenteDisponibilidadDiaria",
                        new[] { Parametro(OdbcType.BigInt, idRegistro) }, token, lector => coincide = true);
                    if (!coincide) throw new InvalidOperationException("El historial no coincide con la versión guardada. No se guardaron los cambios.");
                }
            }
        }

        private static bool ExisteTabla(OdbcConnection conexion, string tabla, CancellationToken token)
        {
            bool existe = false;
            Leer(conexion, null, "SELECT 1 FROM information_schema.TABLES " +
                "WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = ? LIMIT 1",
                new[] { Parametro(OdbcType.VarChar, tabla.ToLowerInvariant()) }, token, lector => existe = true);
            return existe;
        }

        private static bool ExisteColumna(OdbcConnection conexion, string tabla, string columna, CancellationToken token)
        {
            bool existe = false;
            Leer(conexion, null, "SELECT 1 FROM information_schema.COLUMNS " +
                "WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = ? AND LOWER(COLUMN_NAME) = ? LIMIT 1",
                new[] { Parametro(OdbcType.VarChar, tabla.ToLowerInvariant()), Parametro(OdbcType.VarChar, columna.ToLowerInvariant()) },
                token, lector => existe = true);
            return existe;
        }

        private static void Leer(OdbcConnection conexion, OdbcTransaction transaccion, string sql,
            IEnumerable<OdbcParameter> parametros, CancellationToken token, Action<OdbcDataReader> leerFila)
        {
            using (OdbcCommand comando = CrearComando(conexion, transaccion, sql, parametros))
            using (token.Register(() => Cancelar(comando)))
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    using (OdbcDataReader lector = comando.ExecuteReader())
                        while (lector.Read()) { token.ThrowIfCancellationRequested(); leerFila(lector); }
                    token.ThrowIfCancellationRequested();
                }
                catch (OdbcException) when (token.IsCancellationRequested) { throw new OperationCanceledException(token); }
            }
        }

        private static OdbcCommand CrearComando(OdbcConnection conexion, OdbcTransaction transaccion, string sql, IEnumerable<OdbcParameter> parametros)
        {
            var valores = parametros == null ? new List<OdbcParameter>() : new List<OdbcParameter>(parametros);
            string[] partes = sql.Split('?');
            if (partes.Length != valores.Count + 1)
                throw new InvalidOperationException("La consulta no contiene los parámetros esperados.");
            var texto = new StringBuilder(partes[0]);
            var comando = new OdbcCommand { Connection = conexion, Transaction = transaccion, CommandTimeout = 20 };
            for (int i = 0; i < valores.Count; i++)
            {
                // Estos SQL son constantes internos con marcadores posicionales.
                // Los opcionales vacios se expresan con SQL NULL, sin atribuirles
                // ceros ni texto. Todo valor recibido sigue parametrizado.
                if (valores[i].Value == null || valores[i].Value == DBNull.Value) texto.Append("NULL");
                else { texto.Append('?'); comando.Parameters.Add(valores[i]); }
                texto.Append(partes[i + 1]);
            }
            comando.CommandText = texto.ToString();
            return comando;
        }

        private static OdbcParameter Parametro(OdbcType tipo, object valor)
        {
            return new OdbcParameter
            {
                OdbcType = tipo, Value = valor ?? DBNull.Value,
                // Mantiene una longitud declarada tambien para los textos opcionales.
                Size = tipo == OdbcType.Char ? 3 : tipo == OdbcType.VarChar ? 100 : 0
            };
        }

        private static OdbcParameter ParametroDecimal(decimal valor)
        { return new OdbcParameter { OdbcType = OdbcType.Decimal, Precision = 12, Scale = 4, Value = valor }; }

        private static void Cancelar(OdbcCommand comando)
        {
            try { comando.Cancel(); }
            catch (OdbcException) { }
            catch (InvalidOperationException) { }
        }
    }
}
