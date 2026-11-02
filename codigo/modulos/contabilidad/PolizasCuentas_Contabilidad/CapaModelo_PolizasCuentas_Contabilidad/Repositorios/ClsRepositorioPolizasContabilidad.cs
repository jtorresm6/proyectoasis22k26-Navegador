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
 *  lógica: nunca se usa DELETE.
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

        private const string _InsertEncabezado =
            "INSERT INTO tblPolizaEncabezado (idModuloContable, codigoPolizaEncabezado, fechaPolizaEncabezado, periodoContablePolizaEncabezado, " +
            "estadoPolizaEncabezado, documentoOrigenPolizaEncabezado, descripcionPolizaEncabezado) VALUES (?, ?, ?, ?, ?, ?, ?)";

        private const string _InsertDetalle =
            "INSERT INTO tblPolizaDetalle (idPolizaEncabezado, idCuenta, lineaPolizaDetalle, debePolizaDetalle, haberPolizaDetalle, descripcionPolizaDetalle) " +
            "VALUES (?, ?, ?, ?, ?, ?)";

        private const string _UpdateEncabezado =
            "UPDATE tblPolizaEncabezado SET estadoPolizaEncabezado = ?, descripcionPolizaEncabezado = ? " +
            "WHERE idPolizaEncabezado = ? AND estadoPolizaEncabezado = 'BORRADOR'";

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
                        string Codigo;
                        using (OdbcCommand Comando = new OdbcCommand(_SiguienteCorrelativo, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Periodo", Periodo);
                            Codigo = "POL-" + Periodo + "-" + Convert.ToInt32(Comando.ExecuteScalar()).ToString("0000");
                        }

                        using (OdbcCommand Comando = new OdbcCommand(_InsertEncabezado, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Modulo", Entidad.IdModulo);
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
            return ContabilidadFuncEjecutarNonQuery(_UpdateEncabezado, new List<OdbcParameter>
            {
                new OdbcParameter("Estado", Entidad.Estado),
                new OdbcParameter("Descripcion", string.IsNullOrWhiteSpace(Entidad.Descripcion) ? (object)DBNull.Value : Entidad.Descripcion),
                new OdbcParameter("Poliza", Entidad.IdPoliza)
            }, CommandType.Text);
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

        public string ContabilidadFuncObtenerNombreModulo(int IdModulo)
        {
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta("SELECT nombreModuloContable FROM tblModuloContable WHERE idModuloContable = ? AND is_active = TRUE",
                CommandType.Text, new List<OdbcParameter> { new OdbcParameter("Modulo", IdModulo) }))
                return Tabla.Rows.Count == 0 ? string.Empty : Convert.ToString(Tabla.Rows[0][0]);
        }
    }
}
