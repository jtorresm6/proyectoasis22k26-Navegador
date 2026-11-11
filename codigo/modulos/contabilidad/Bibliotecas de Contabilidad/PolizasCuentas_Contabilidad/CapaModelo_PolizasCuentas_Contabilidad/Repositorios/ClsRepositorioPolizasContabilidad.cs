/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Repositorio de pólizas sobre tblPolizaEncabezado y tblPolizaDetalle:
 *  lista las pólizas con sus líneas, agrega una póliza completa en una
 *  transacción, cambia su estado o descripción y la anula.
 * Reglas especificas:
 *  Insert y listado los creó Oskar Cermeño; se reescribieron para las
 *  tablas de EST-01. El código POL-AAAA-MM-#### se genera aquí con el
 *  correlativo del período, dentro de la transacción. Solo una póliza
 *  en BORRADOR se modifica y una APLICADA no se anula. Eliminar es baja
 *  lógica: nunca se usa DELETE. El país de la póliza se toma de las
 *  cuentas asignadas a su módulo; si no tiene, queda Guatemala.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Repositorios
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsRepositorioPolizasContabilidad : CapaModelo_Contabilidad.Repositorios.ClsRepositorioMaestroContabilidad, IRepositorioPolizasContabilidad
    {
        private const string _SelectTodos =
            "SELECT pe.idPolizaEncabezado, pe.codigoPolizaEncabezado, pe.fechaPolizaEncabezado, pe.periodoContablePolizaEncabezado, " +
            "pe.estadoPolizaEncabezado, pe.idModuloContable, m.nombreModuloContable, pe.documentoOrigenPolizaEncabezado, pe.descripcionPolizaEncabezado, " +
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            "pe.idPais, " +
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            "pd.idPolizaDetalle, pd.idCuenta, pd.lineaPolizaDetalle, pd.debePolizaDetalle, pd.haberPolizaDetalle, pd.descripcionPolizaDetalle, " +
            "c.codigoCuenta, c.nombreCuenta " +
            "FROM tblPolizaEncabezado pe " +
            "INNER JOIN tblModuloContable m ON m.idModuloContable = pe.idModuloContable " +
            "LEFT JOIN tblPolizaDetalle pd ON pd.idPolizaEncabezado = pe.idPolizaEncabezado AND pd.is_active = TRUE " +
            "LEFT JOIN tblCuenta c ON c.idCuenta = pd.idCuenta " +
            "WHERE pe.is_active = TRUE " +
            "ORDER BY pe.fechaPolizaEncabezado DESC, pe.idPolizaEncabezado DESC, pd.lineaPolizaDetalle";

        private const string _SiguienteCorrelativo =
            "SELECT COALESCE(MAX(CAST(SUBSTRING(codigoPolizaEncabezado, 13) AS UNSIGNED)), 0) + 1 FROM tblPolizaEncabezado " +
            "WHERE periodoContablePolizaEncabezado = ? AND codigoPolizaEncabezado LIKE 'POL-%' FOR UPDATE";

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        private const string _InsertEncabezado =
            "INSERT INTO tblPolizaEncabezado (idModuloContable, idPais, codigoPolizaEncabezado, fechaPolizaEncabezado, periodoContablePolizaEncabezado, " +
            "estadoPolizaEncabezado, documentoOrigenPolizaEncabezado, descripcionPolizaEncabezado) " +
            "VALUES (?, COALESCE(NULLIF(?, 0), (SELECT MIN(mcp.idPais) FROM tblModuloCuentaPais mcp WHERE mcp.idModuloContable = ?), 1), ?, ?, ?, ?, ?, ?)";
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026

        private const string _InsertDetalle =
            "INSERT INTO tblPolizaDetalle (idPolizaEncabezado, idCuenta, lineaPolizaDetalle, debePolizaDetalle, haberPolizaDetalle, descripcionPolizaDetalle) " +
            "VALUES (?, ?, ?, ?, ?, ?)";

        private const string _UpdateEncabezado =
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            "UPDATE tblPolizaEncabezado SET fechaPolizaEncabezado = ?, estadoPolizaEncabezado = ?, descripcionPolizaEncabezado = ? " +
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ", idPais = COALESCE(NULLIF(?, 0), idPais) " +
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            "WHERE idPolizaEncabezado = ? AND estadoPolizaEncabezado = 'BORRADOR' AND periodoContablePolizaEncabezado = ?";

        private const string _DesactivarDetalle =
            "UPDATE tblPolizaDetalle SET is_active = FALSE WHERE idPolizaEncabezado = ? AND is_active = TRUE";

        private const string _UltimaLinea =
            "SELECT COALESCE(MAX(lineaPolizaDetalle), 0) FROM tblPolizaDetalle WHERE idPolizaEncabezado = ?";

        private const string _PolizasPorRango =
            "SELECT idPolizaEncabezado, periodoContablePolizaEncabezado FROM tblPolizaEncabezado " +
            "WHERE estadoPolizaEncabezado = 'VALIDADA' AND is_active = TRUE AND fechaPolizaEncabezado BETWEEN ? AND ? FOR UPDATE";

        private const string _PolizaPorId =
            "SELECT idPolizaEncabezado, periodoContablePolizaEncabezado FROM tblPolizaEncabezado " +
            "WHERE estadoPolizaEncabezado = 'VALIDADA' AND is_active = TRUE AND idPolizaEncabezado = ? FOR UPDATE";

        private const string _InsertActualizacion =
            "INSERT INTO tblActualizacionSaldos (modoActualizacionSaldos, tipoPeriodoActualizacionSaldos, fechaDesdeActualizacionSaldos, " +
            "fechaHastaActualizacionSaldos, polizasProcesadasActualizacionSaldos, usuarioActualizacionSaldos) VALUES (?, ?, ?, ?, 0, ?)";

        private const string _MarcarPoliza =
            "UPDATE tblPolizaEncabezado SET idActualizacionSaldos = ? WHERE idPolizaEncabezado = ?";

        private const string _AcumularSaldos =
            "INSERT INTO tblSaldoCuentaPeriodo (idCuenta, idActualizacionSaldos, periodoContableSaldoCuentaPeriodo, saldoInicialSaldoCuentaPeriodo, " +
            "cargoSaldoCuentaPeriodo, abonoSaldoCuentaPeriodo, saldoFinalSaldoCuentaPeriodo, estadoSaldoCuentaPeriodo) " +
            "SELECT m.idCuenta, m.idActualizacion, m.periodo, m.inicial, m.debe, m.haber, 0, 'ABIERTO' FROM (" +
            "WITH RECURSIVE movimiento AS (" +
            "SELECT pd.idCuenta, pe.periodoContablePolizaEncabezado AS periodo, SUM(pd.debePolizaDetalle) AS debe, SUM(pd.haberPolizaDetalle) AS haber " +
            "FROM tblPolizaDetalle pd INNER JOIN tblPolizaEncabezado pe ON pe.idPolizaEncabezado = pd.idPolizaEncabezado " +
            "WHERE pe.idActualizacionSaldos = ? AND pd.is_active = TRUE GROUP BY pd.idCuenta, pe.periodoContablePolizaEncabezado" +
            "), ascenso AS (" +
            "SELECT idCuenta, periodo, debe, haber FROM movimiento UNION ALL " +
            "SELECT c.idCuentaPadre, a.periodo, a.debe, a.haber FROM ascenso a INNER JOIN tblCuenta c ON c.idCuenta = a.idCuenta WHERE c.idCuentaPadre IS NOT NULL" +
            ") SELECT a.idCuenta, ? AS idActualizacion, a.periodo, SUM(a.debe) AS debe, SUM(a.haber) AS haber, " +
            "COALESCE((SELECT s.saldoFinalSaldoCuentaPeriodo FROM tblSaldoCuentaPeriodo s WHERE s.idCuenta = a.idCuenta " +
            "AND s.periodoContableSaldoCuentaPeriodo < a.periodo ORDER BY s.periodoContableSaldoCuentaPeriodo DESC LIMIT 1), 0) AS inicial " +
            "FROM ascenso a INNER JOIN tblCuenta c ON c.idCuenta = a.idCuenta WHERE c.codigoCuenta <> '0' GROUP BY a.idCuenta, a.periodo" +
            ") AS m ON DUPLICATE KEY UPDATE cargoSaldoCuentaPeriodo = cargoSaldoCuentaPeriodo + m.debe, " +
            "abonoSaldoCuentaPeriodo = abonoSaldoCuentaPeriodo + m.haber, idActualizacionSaldos = m.idActualizacion";

        private const string _CalcularSaldoFinal =
            "UPDATE tblSaldoCuentaPeriodo s INNER JOIN tblCuenta c ON c.idCuenta = s.idCuenta SET s.saldoFinalSaldoCuentaPeriodo = " +
            "CASE WHEN c.tipoCuenta = 'DEUDORA' THEN s.saldoInicialSaldoCuentaPeriodo + s.cargoSaldoCuentaPeriodo - s.abonoSaldoCuentaPeriodo " +
            "ELSE s.saldoInicialSaldoCuentaPeriodo + s.abonoSaldoCuentaPeriodo - s.cargoSaldoCuentaPeriodo END " +
            "WHERE s.idActualizacionSaldos = ?";

        private const string _ActualizarCuentas =
            "UPDATE tblCuenta c INNER JOIN tblSaldoCuentaPeriodo s ON s.idCuenta = c.idCuenta " +
            "SET c.saldoInicialCuenta = s.saldoInicialSaldoCuentaPeriodo, c.cargoPeriodoCuenta = s.cargoSaldoCuentaPeriodo, " +
            "c.abonoPeriodoCuenta = s.abonoSaldoCuentaPeriodo, c.saldoActualCuenta = s.saldoFinalSaldoCuentaPeriodo, " +
            "c.cargoAcumuladoCuenta = (SELECT SUM(a.cargoSaldoCuentaPeriodo) FROM (SELECT idCuenta, periodoContableSaldoCuentaPeriodo, cargoSaldoCuentaPeriodo FROM tblSaldoCuentaPeriodo) a " +
            "WHERE a.idCuenta = c.idCuenta AND a.periodoContableSaldoCuentaPeriodo BETWEEN CONCAT(LEFT(s.periodoContableSaldoCuentaPeriodo, 4), '-01') AND s.periodoContableSaldoCuentaPeriodo), " +
            "c.abonoAcumuladoCuenta = (SELECT SUM(a.abonoSaldoCuentaPeriodo) FROM (SELECT idCuenta, periodoContableSaldoCuentaPeriodo, abonoSaldoCuentaPeriodo FROM tblSaldoCuentaPeriodo) a " +
            "WHERE a.idCuenta = c.idCuenta AND a.periodoContableSaldoCuentaPeriodo BETWEEN CONCAT(LEFT(s.periodoContableSaldoCuentaPeriodo, 4), '-01') AND s.periodoContableSaldoCuentaPeriodo), " +
            "c.periodoSaldoCuenta = s.periodoContableSaldoCuentaPeriodo " +
            "WHERE s.periodoContableSaldoCuentaPeriodo = " +
            "(SELECT MAX(u.periodoContableSaldoCuentaPeriodo) FROM (SELECT idCuenta, periodoContableSaldoCuentaPeriodo FROM tblSaldoCuentaPeriodo) u WHERE u.idCuenta = c.idCuenta) " +
            "AND EXISTS (SELECT 1 FROM (SELECT idCuenta, idActualizacionSaldos FROM tblSaldoCuentaPeriodo) t WHERE t.idCuenta = c.idCuenta AND t.idActualizacionSaldos = ?)";

        private const string _PeriodosPosteriores =
            "SELECT DISTINCT periodoContableSaldoCuentaPeriodo FROM tblSaldoCuentaPeriodo WHERE periodoContableSaldoCuentaPeriodo > ? " +
            "ORDER BY periodoContableSaldoCuentaPeriodo";

        private const string _ArrastrarSaldos =
            "UPDATE tblSaldoCuentaPeriodo s INNER JOIN tblCuenta c ON c.idCuenta = s.idCuenta INNER JOIN (" +
            "SELECT DISTINCT a.idCuenta, a.saldoFinalSaldoCuentaPeriodo AS anterior FROM tblSaldoCuentaPeriodo a " +
            "WHERE a.periodoContableSaldoCuentaPeriodo = (SELECT MAX(b.periodoContableSaldoCuentaPeriodo) FROM tblSaldoCuentaPeriodo b " +
            "WHERE b.idCuenta = a.idCuenta AND b.periodoContableSaldoCuentaPeriodo < ?)) p ON p.idCuenta = s.idCuenta " +
            "SET s.saldoInicialSaldoCuentaPeriodo = p.anterior, s.saldoFinalSaldoCuentaPeriodo = CASE WHEN c.tipoCuenta = 'DEUDORA' " +
            "THEN p.anterior + s.cargoSaldoCuentaPeriodo - s.abonoSaldoCuentaPeriodo ELSE p.anterior + s.abonoSaldoCuentaPeriodo - s.cargoSaldoCuentaPeriodo END " +
            "WHERE s.periodoContableSaldoCuentaPeriodo = ?";

        private const string _AplicarPolizas =
            "UPDATE tblPolizaEncabezado SET estadoPolizaEncabezado = 'APLICADA' WHERE idActualizacionSaldos = ? AND estadoPolizaEncabezado = 'VALIDADA'";

        private const string _CerrarActualizacion =
            "UPDATE tblActualizacionSaldos SET polizasProcesadasActualizacionSaldos = ? WHERE idActualizacionSaldos = ?";
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026

        private const string _Anular =
            "UPDATE tblPolizaEncabezado SET estadoPolizaEncabezado = 'ANULADA', is_active = FALSE " +
            "WHERE idPolizaEncabezado = ? AND estadoPolizaEncabezado IN ('BORRADOR','VALIDADA')";

        public int ContabilidadFuncAgregar(ClsPolizaContabilidad Entidad)
        {
            if (Entidad.Detalle == null || Entidad.Detalle.Count < 2)
                throw new InvalidOperationException("Una póliza contable debe contener al menos dos líneas de detalle.");
            decimal TotalDebe = Entidad.Detalle.Sum(Linea => Linea.Debe);
            if (TotalDebe <= 0m || TotalDebe != Entidad.Detalle.Sum(Linea => Linea.Haber))
                throw new InvalidOperationException("La póliza no cuadra: el total del debe debe ser igual al del haber y mayor que cero.");

            string Periodo = Entidad.Fecha.ToString("yyyy-MM");
            using (OdbcConnection Conexion = ContabilidadFuncObtenerConexion())
            {
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                        ContabilidadMetValidarPeriodoAbierto(Conexion, Transaccion, Periodo);
                        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                        string Codigo;
                        using (OdbcCommand Comando = new OdbcCommand(_SiguienteCorrelativo, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Periodo", Periodo);
                            Codigo = "POL-" + Periodo + "-" + Convert.ToInt32(Comando.ExecuteScalar()).ToString("0000");
                        }

                        using (OdbcCommand Comando = new OdbcCommand(_InsertEncabezado, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Modulo", Entidad.IdModulo);
                            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                            Comando.Parameters.AddWithValue("Pais", Entidad.IdPais);
                            Comando.Parameters.AddWithValue("ModuloPais", Entidad.IdModulo);
                            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                            Comando.Parameters.AddWithValue("Codigo", Codigo);
                            Comando.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Entidad.Fecha.Date });
                            Comando.Parameters.AddWithValue("Periodo", Periodo);
                            Comando.Parameters.AddWithValue("Estado", string.IsNullOrWhiteSpace(Entidad.Estado) ? "BORRADOR" : Entidad.Estado);
                            Comando.Parameters.AddWithValue("Documento", string.IsNullOrWhiteSpace(Entidad.DocumentoOrigen) ? (object)DBNull.Value : Entidad.DocumentoOrigen);
                            Comando.Parameters.AddWithValue("Descripcion", string.IsNullOrWhiteSpace(Entidad.Descripcion) ? (object)DBNull.Value : Entidad.Descripcion);
                            Comando.ExecuteNonQuery();
                        }

                        int IdPoliza;
                        using (OdbcCommand Comando = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                            IdPoliza = Convert.ToInt32(Comando.ExecuteScalar());

                        int Linea = 1;
                        foreach (ClsPolizaDetalleContabilidad Detalle in Entidad.Detalle)
                        {
                            using (OdbcCommand Comando = new OdbcCommand(_InsertDetalle, Conexion, Transaccion))
                            {
                                Comando.Parameters.AddWithValue("Poliza", IdPoliza);
                                Comando.Parameters.AddWithValue("Cuenta", Detalle.IdCuenta);
                                Comando.Parameters.AddWithValue("Linea", Linea++);
                                Comando.Parameters.Add(new OdbcParameter("Debe", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Detalle.Debe });
                                Comando.Parameters.Add(new OdbcParameter("Haber", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Detalle.Haber });
                                Comando.Parameters.AddWithValue("DescripcionDetalle", string.IsNullOrWhiteSpace(Detalle.DescripcionDetalle) ? (object)DBNull.Value : Detalle.DescripcionDetalle);
                                Comando.ExecuteNonQuery();
                            }
                        }

                        Transaccion.Commit();
                        Entidad.IdPoliza = IdPoliza;
                        Entidad.CodigoPoliza = Codigo;
                        Entidad.PeriodoContable = Periodo;
                        return 1;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public int ContabilidadFuncEditar(ClsPolizaContabilidad Entidad)
        {
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            using (OdbcConnection Conexion = ContabilidadFuncObtenerConexion())
            {
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        ContabilidadMetValidarPeriodoAbierto(Conexion, Transaccion, Entidad.PeriodoContable);
                        int Filas;
                        using (OdbcCommand Comando = new OdbcCommand(_UpdateEncabezado, Conexion, Transaccion))
                        {
                            Comando.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Entidad.Fecha.Date });
                            Comando.Parameters.AddWithValue("Estado", Entidad.Estado);
                            Comando.Parameters.AddWithValue("Descripcion", string.IsNullOrWhiteSpace(Entidad.Descripcion) ? (object)DBNull.Value : Entidad.Descripcion);
                            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                            Comando.Parameters.AddWithValue("Pais", Entidad.IdPais);
                            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                            Comando.Parameters.AddWithValue("Poliza", Entidad.IdPoliza);
                            Comando.Parameters.AddWithValue("Periodo", Entidad.PeriodoContable);
                            Filas = Comando.ExecuteNonQuery();
                        }
                        if (Filas == 0)
                        {
                            Transaccion.Rollback();
                            return 0;
                        }
                        if (Entidad.Detalle != null && Entidad.Detalle.Count > 0)
                        {
                            using (OdbcCommand Comando = new OdbcCommand(_DesactivarDetalle, Conexion, Transaccion))
                            {
                                Comando.Parameters.AddWithValue("Poliza", Entidad.IdPoliza);
                                Comando.ExecuteNonQuery();
                            }
                            int Linea;
                            using (OdbcCommand Comando = new OdbcCommand(_UltimaLinea, Conexion, Transaccion))
                            {
                                Comando.Parameters.AddWithValue("Poliza", Entidad.IdPoliza);
                                Linea = Convert.ToInt32(Comando.ExecuteScalar()) + 1;
                            }
                            ContabilidadMetInsertarDetalle(Conexion, Transaccion, Entidad.IdPoliza, Entidad.Detalle, Linea);
                        }
                        Transaccion.Commit();
                        return Filas;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public int ContabilidadFuncActualizarSaldos(string Modo, string TipoPeriodo, DateTime Desde, DateTime Hasta, int IdPoliza, string Usuario)
        {
            using (OdbcConnection Conexion = ContabilidadFuncObtenerConexion())
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            {
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        List<int> Polizas = new List<int>();
                        HashSet<string> Periodos = new HashSet<string>();
                        using (OdbcCommand Comando = new OdbcCommand(IdPoliza > 0 ? _PolizaPorId : _PolizasPorRango, Conexion, Transaccion))
                        {
                            if (IdPoliza > 0) Comando.Parameters.AddWithValue("Poliza", IdPoliza);
                            else
                            {
                                Comando.Parameters.Add(new OdbcParameter("Desde", OdbcType.Date) { Value = Desde.Date });
                                Comando.Parameters.Add(new OdbcParameter("Hasta", OdbcType.Date) { Value = Hasta.Date });
                            }
                            using (OdbcDataReader Lector = Comando.ExecuteReader())
                                while (Lector.Read())
                                {
                                    Polizas.Add(Convert.ToInt32(Lector[0]));
                                    Periodos.Add(Convert.ToString(Lector[1]));
                                }
                        }
                        if (Polizas.Count == 0)
                        {
                            Transaccion.Rollback();
                            return 0;
                        }

                        using (OdbcCommand Comando = new OdbcCommand("SELECT COUNT(*) FROM tblSaldoCuentaPeriodo WHERE estadoSaldoCuentaPeriodo = 'CERRADO' AND periodoContableSaldoCuentaPeriodo IN (" +
                            string.Join(", ", Periodos.Select(Periodo => "?")) + ")", Conexion, Transaccion))
                        {
                            foreach (string Periodo in Periodos) Comando.Parameters.AddWithValue("Periodo", Periodo);
                            if (Convert.ToInt32(Comando.ExecuteScalar()) > 0)
                                throw new InvalidOperationException("Hay pólizas de un período cerrado (" + string.Join(", ", Periodos) + ").");
                        }

                        int IdActualizacion;
                        using (OdbcCommand Comando = new OdbcCommand(_InsertActualizacion, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Modo", Modo);
                            Comando.Parameters.AddWithValue("Tipo", TipoPeriodo);
                            Comando.Parameters.Add(new OdbcParameter("Desde", OdbcType.Date) { Value = Desde.Date });
                            Comando.Parameters.Add(new OdbcParameter("Hasta", OdbcType.Date) { Value = Hasta.Date });
                            Comando.Parameters.AddWithValue("Usuario", string.IsNullOrWhiteSpace(Usuario) ? (object)DBNull.Value : Usuario);
                            Comando.ExecuteNonQuery();
                        }
                        using (OdbcCommand Comando = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                            IdActualizacion = Convert.ToInt32(Comando.ExecuteScalar());

                        foreach (int Poliza in Polizas)
                            using (OdbcCommand Comando = new OdbcCommand(_MarcarPoliza, Conexion, Transaccion))
                            {
                                Comando.Parameters.AddWithValue("Actualizacion", IdActualizacion);
                                Comando.Parameters.AddWithValue("Poliza", Poliza);
                                Comando.ExecuteNonQuery();
                            }

                        using (OdbcCommand Comando = new OdbcCommand(_AcumularSaldos, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Actualizacion", IdActualizacion);
                            Comando.Parameters.AddWithValue("ActualizacionSaldo", IdActualizacion);
                            Comando.ExecuteNonQuery();
                        }
                        using (OdbcCommand Comando = new OdbcCommand(_CalcularSaldoFinal, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Actualizacion", IdActualizacion);
                            Comando.ExecuteNonQuery();
                        }
                        List<string> Posteriores = new List<string>();
                        using (OdbcCommand Comando = new OdbcCommand(_PeriodosPosteriores, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Periodo", Periodos.Min());
                            using (OdbcDataReader Lector = Comando.ExecuteReader())
                                while (Lector.Read()) Posteriores.Add(Convert.ToString(Lector[0]));
                        }
                        foreach (string Periodo in Posteriores)
                            using (OdbcCommand Comando = new OdbcCommand(_ArrastrarSaldos, Conexion, Transaccion))
                            {
                                Comando.Parameters.AddWithValue("Anterior", Periodo);
                                Comando.Parameters.AddWithValue("Periodo", Periodo);
                                Comando.ExecuteNonQuery();
                            }
                        foreach (string Sentencia in new[] { _ActualizarCuentas, _AplicarPolizas })
                            using (OdbcCommand Comando = new OdbcCommand(Sentencia, Conexion, Transaccion))
                            {
                                Comando.Parameters.AddWithValue("Actualizacion", IdActualizacion);
                                Comando.ExecuteNonQuery();
                            }
                        using (OdbcCommand Comando = new OdbcCommand(_CerrarActualizacion, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Cantidad", Polizas.Count);
                            Comando.Parameters.AddWithValue("Actualizacion", IdActualizacion);
                            Comando.ExecuteNonQuery();
                        }
                        Transaccion.Commit();
                        return Polizas.Count;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        private static void ContabilidadMetValidarPeriodoAbierto(OdbcConnection Conexion, OdbcTransaction Transaccion, string Periodo)
        {
            using (OdbcCommand Comando = new OdbcCommand("SELECT COUNT(*) FROM tblSaldoCuentaPeriodo WHERE estadoSaldoCuentaPeriodo = 'CERRADO' " +
                "AND periodoContableSaldoCuentaPeriodo >= ?", Conexion, Transaccion))
            {
                Comando.Parameters.AddWithValue("Periodo", Periodo);
                if (Convert.ToInt32(Comando.ExecuteScalar()) > 0)
                    throw new InvalidOperationException("El período " + Periodo + " ya está cerrado. Registre la póliza en un período abierto.");
            }
        }

        private static void ContabilidadMetInsertarDetalle(OdbcConnection Conexion, OdbcTransaction Transaccion, int IdPoliza,
            IEnumerable<ClsPolizaDetalleContabilidad> Detalle, int LineaInicial)
        {
            int Linea = LineaInicial;
            foreach (ClsPolizaDetalleContabilidad Elemento in Detalle)
                using (OdbcCommand Comando = new OdbcCommand(_InsertDetalle, Conexion, Transaccion))
                {
                    Comando.Parameters.AddWithValue("Poliza", IdPoliza);
                    Comando.Parameters.AddWithValue("Cuenta", Elemento.IdCuenta);
                    Comando.Parameters.AddWithValue("Linea", Linea++);
                    Comando.Parameters.Add(new OdbcParameter("Debe", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Elemento.Debe });
                    Comando.Parameters.Add(new OdbcParameter("Haber", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Elemento.Haber });
                    Comando.Parameters.AddWithValue("DescripcionDetalle", string.IsNullOrWhiteSpace(Elemento.DescripcionDetalle) ? (object)DBNull.Value : Elemento.DescripcionDetalle);
                    Comando.ExecuteNonQuery();
                }
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        }

        public int ContabilidadFuncEliminar(ClsPolizaContabilidad Entidad)
        {
            return ContabilidadFuncEjecutarNonQuery(_Anular, new List<OdbcParameter> { new OdbcParameter("Poliza", Entidad.IdPoliza) }, CommandType.Text);
        }

        public IEnumerable<ClsPolizaContabilidad> ContabilidadFuncObtenerTodos()
        {
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectTodos, CommandType.Text))
            {
                Dictionary<int, ClsPolizaContabilidad> Polizas = new Dictionary<int, ClsPolizaContabilidad>();
                List<ClsPolizaContabilidad> Orden = new List<ClsPolizaContabilidad>();
                foreach (DataRow Fila in Tabla.Rows)
                {
                    int IdPoliza = Convert.ToInt32(Fila["idPolizaEncabezado"]);
                    ClsPolizaContabilidad Poliza;
                    if (!Polizas.TryGetValue(IdPoliza, out Poliza))
                    {
                        Poliza = new ClsPolizaContabilidad
                        {
                            IdPoliza = IdPoliza,
                            CodigoPoliza = Convert.ToString(Fila["codigoPolizaEncabezado"]),
                            Fecha = Convert.ToDateTime(Fila["fechaPolizaEncabezado"]),
                            PeriodoContable = Convert.ToString(Fila["periodoContablePolizaEncabezado"]),
                            Estado = Convert.ToString(Fila["estadoPolizaEncabezado"]),
                            IdModulo = Convert.ToInt32(Fila["idModuloContable"]),
                            Modulo = Convert.ToString(Fila["nombreModuloContable"]),
                            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                            IdPais = Convert.ToInt32(Fila["idPais"]),
                            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                            DocumentoOrigen = Fila["documentoOrigenPolizaEncabezado"] == DBNull.Value ? null : Convert.ToString(Fila["documentoOrigenPolizaEncabezado"]),
                            Descripcion = Fila["descripcionPolizaEncabezado"] == DBNull.Value ? null : Convert.ToString(Fila["descripcionPolizaEncabezado"])
                        };
                        Polizas.Add(IdPoliza, Poliza);
                        Orden.Add(Poliza);
                    }
                    if (Fila["idPolizaDetalle"] == DBNull.Value) continue;
                    Poliza.Detalle.Add(new ClsPolizaDetalleContabilidad
                    {
                        IdPolizaDetalle = Convert.ToInt32(Fila["idPolizaDetalle"]),
                        IdPoliza = IdPoliza,
                        IdCuenta = Convert.ToInt32(Fila["idCuenta"]),
                        Linea = Convert.ToInt32(Fila["lineaPolizaDetalle"]),
                        Debe = Convert.ToDecimal(Fila["debePolizaDetalle"]),
                        Haber = Convert.ToDecimal(Fila["haberPolizaDetalle"]),
                        DescripcionDetalle = Fila["descripcionPolizaDetalle"] == DBNull.Value ? null : Convert.ToString(Fila["descripcionPolizaDetalle"]),
                        Codigo = Convert.ToString(Fila["codigoCuenta"]),
                        Cuenta = Convert.ToString(Fila["nombreCuenta"])
                    });
                }
                return Orden;
            }
        }

        public List<ClsCuentaContabilidad> ContabilidadFuncObtenerCuentas()
        {
            return new ClsRepositorioCuentasContabilidad().ContabilidadFuncObtenerTodos().Where(Cuenta => Cuenta.Activo).ToList();
        }

        public string[] ContabilidadFuncObtenerEstados()
        {
            return new[] { "BORRADOR", "VALIDADA", "ANULADA" };
        }

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        public Dictionary<int, string> ContabilidadFuncObtenerPaises()
        {
            Dictionary<int, string> Paises = new Dictionary<int, string>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta("SELECT idPais, nombrePais FROM tblPais WHERE is_active = TRUE ORDER BY idPais", CommandType.Text))
                foreach (DataRow Fila in Tabla.Rows) Paises[Convert.ToInt32(Fila[0])] = Convert.ToString(Fila[1]);
            return Paises;
        }

        public Dictionary<int, string> ContabilidadFuncObtenerMonedasPais()
        {
            Dictionary<int, string> Monedas = new Dictionary<int, string>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta("SELECT idPais, codigoMonedaPais FROM tblPais", CommandType.Text))
                foreach (DataRow Fila in Tabla.Rows) Monedas[Convert.ToInt32(Fila[0])] = Convert.ToString(Fila[1]);
            return Monedas;
        }
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026

        public string ContabilidadFuncObtenerNombreModulo(int IdModulo)
        {
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta("SELECT nombreModuloContable FROM tblModuloContable WHERE idModuloContable = ? AND is_active = TRUE",
                CommandType.Text, new List<OdbcParameter> { new OdbcParameter("Modulo", IdModulo) }))
                return Tabla.Rows.Count == 0 ? string.Empty : Convert.ToString(Tabla.Rows[0][0]);
        }

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        public string ContabilidadFuncObtenerMetodoActualizacion(int IdModulo)
        {
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta("SELECT metodoActualizacionModuloContable FROM tblModuloContable WHERE idModuloContable = ? AND is_active = TRUE",
                CommandType.Text, new List<OdbcParameter> { new OdbcParameter("Modulo", IdModulo) }))
                return Tabla.Rows.Count == 0 ? string.Empty : Convert.ToString(Tabla.Rows[0][0]);
        }
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
    }
}
