/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Mario Taracena
 * Carné : 0901-23-9335
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Repositorio de presupuestos sobre tblPresupuestoEncabezado y
 *  tblPresupuestoDetalle: lista, agrega, modifica y da de baja un
 *  presupuesto con sus partidas en una sola transacción.
 * Reglas especificas:
 *  El restante lo calcula la base (columna generada). Las partidas se
 *  ligan al módulo por su nombre en tblModuloContable. Eliminar es
 *  baja lógica: nunca se usa DELETE.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
using System.Data;
using System.Data.Odbc;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;

namespace CapaModelo_Contabilidad.Repositorios
{
    // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
    public class ClsRepositorioPresupuestosContabilidad : ClsRepositorioMaestroContabilidad, IRepositorioPresupuestosContabilidad
    // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
    {
        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        private const string _SelectTodos =
            "SELECT e.idPresupuestoEncabezado, e.nombrePresupuestoEncabezado, e.totalPresupuestoEncabezado, e.gastadoPresupuestoEncabezado, " +
            "e.fechaInicioPresupuestoEncabezado, e.fechaCierrePresupuestoEncabezado, d.nombrePresupuestoDetalle, m.nombreModuloContable, " +
            "d.cantidadPresupuestoDetalle FROM tblPresupuestoEncabezado e " +
            "LEFT JOIN tblPresupuestoDetalle d ON d.idPresupuestoEncabezado = e.idPresupuestoEncabezado AND d.is_active = TRUE " +
            "LEFT JOIN tblModuloContable m ON m.idModuloContable = d.idModuloContable " +
            "WHERE e.is_active = TRUE ORDER BY e.fechaInicioPresupuestoEncabezado DESC, e.idPresupuestoEncabezado DESC, d.idPresupuestoDetalle";
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        private const string _InsertEncabezado =
            "INSERT INTO tblPresupuestoEncabezado (nombrePresupuestoEncabezado, totalPresupuestoEncabezado, gastadoPresupuestoEncabezado, " +
            "fechaInicioPresupuestoEncabezado, fechaCierrePresupuestoEncabezado) VALUES (?, ?, ?, ?, ?)";

        private const string _UpdateEncabezado =
            "UPDATE tblPresupuestoEncabezado SET nombrePresupuestoEncabezado = ?, totalPresupuestoEncabezado = ?, gastadoPresupuestoEncabezado = ?, " +
            "fechaInicioPresupuestoEncabezado = ?, fechaCierrePresupuestoEncabezado = ? WHERE idPresupuestoEncabezado = ? AND is_active = TRUE";

        private const string _InsertDetalle =
            "INSERT INTO tblPresupuestoDetalle (idPresupuestoEncabezado, idModuloContable, nombrePresupuestoDetalle, cantidadPresupuestoDetalle) " +
            "SELECT ?, idModuloContable, ?, ? FROM tblModuloContable WHERE nombreModuloContable = ? AND is_active = TRUE LIMIT 1";

        private const string _DesactivarDetalle =
            "UPDATE tblPresupuestoDetalle SET is_active = FALSE WHERE idPresupuestoEncabezado = ? AND is_active = TRUE";

        private const string _DesactivarEncabezado =
            "UPDATE tblPresupuestoEncabezado SET is_active = FALSE WHERE idPresupuestoEncabezado = ? AND is_active = TRUE";

        // Cuenta si el presupuesto existe y esta activo
        private const string _ExisteEncabezado =
            "SELECT COUNT(*) FROM tblPresupuestoEncabezado WHERE idPresupuestoEncabezado = ? AND is_active = TRUE";
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026

        public int ContabilidadFuncAgregar(ClsPresupuestoContabilidad Entidad)
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            return ContabilidadFuncGuardar(Entidad, true);
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        }

        public int ContabilidadFuncEditar(ClsPresupuestoContabilidad Entidad)
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            return ContabilidadFuncGuardar(Entidad, false);
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        }

        public int ContabilidadFuncEliminar(ClsPresupuestoContabilidad Entidad)
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            using (OdbcConnection Conexion = ContabilidadFuncObtenerConexion())
            {
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        ContabilidadFuncEjecutar(Conexion, Transaccion, _DesactivarDetalle, Entidad.IdPresupuesto);
                        int Filas = ContabilidadFuncEjecutar(Conexion, Transaccion, _DesactivarEncabezado, Entidad.IdPresupuesto);
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
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        }

        public IEnumerable<ClsPresupuestoContabilidad> ContabilidadFuncObtenerTodos()
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            List<ClsPresupuestoContabilidad> Presupuestos = new List<ClsPresupuestoContabilidad>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectTodos, CommandType.Text))
            {
                ClsPresupuestoContabilidad Actual = null;
                foreach (DataRow Fila in Tabla.Rows)
                {
                    int Id = Convert.ToInt32(Fila["idPresupuestoEncabezado"]);
                    if (Actual == null || Actual.IdPresupuesto != Id)
                    {
                        Actual = new ClsPresupuestoContabilidad
                        {
                            IdPresupuesto = Id,
                            Nombre = Convert.ToString(Fila["nombrePresupuestoEncabezado"]),
                            Total = Convert.ToDecimal(Fila["totalPresupuestoEncabezado"]),
                            Gastado = Convert.ToDecimal(Fila["gastadoPresupuestoEncabezado"]),
                            FechaInicio = Convert.ToDateTime(Fila["fechaInicioPresupuestoEncabezado"]),
                            FechaCierre = Fila["fechaCierrePresupuestoEncabezado"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Fila["fechaCierrePresupuestoEncabezado"])
                        };
                        Presupuestos.Add(Actual);
                    }
                    if (Fila["nombrePresupuestoDetalle"] == DBNull.Value) continue;
                    Actual.Detalle.Add(new ClsPresupuestoDetalleContabilidad
                    {
                        Nombre = Convert.ToString(Fila["nombrePresupuestoDetalle"]),
                        Modulo = Convert.ToString(Fila["nombreModuloContable"]),
                        Cantidad = Convert.ToDecimal(Fila["cantidadPresupuestoDetalle"])
                    });
                }
            }
            return Presupuestos;
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        public Dictionary<string, decimal> ContabilidadFuncObtenerEjecutado(DateTime Desde, DateTime Hasta)
        {
            Dictionary<string, decimal> Ejecutado = new Dictionary<string, decimal>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(
                "SELECT m.nombreModuloContable, SUM(pd.debePolizaDetalle - pd.haberPolizaDetalle) FROM tblPolizaDetalle pd " +
                "INNER JOIN tblPolizaEncabezado pe ON pe.idPolizaEncabezado = pd.idPolizaEncabezado " +
                "INNER JOIN tblCuenta c ON c.idCuenta = pd.idCuenta INNER JOIN tblModuloContable m ON m.idModuloContable = pe.idModuloContable " +
                "WHERE pe.estadoPolizaEncabezado = 'APLICADA' AND pe.is_active = TRUE AND pd.is_active = TRUE AND c.codigoCuenta LIKE '5%' " +
                "AND pe.fechaPolizaEncabezado BETWEEN ? AND ? GROUP BY m.nombreModuloContable", CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Desde", OdbcType.Date) { Value = Desde.Date }, new OdbcParameter("Hasta", OdbcType.Date) { Value = Hasta.Date } }))
                foreach (DataRow Fila in Tabla.Rows) Ejecutado[Convert.ToString(Fila[0])] = Convert.ToDecimal(Fila[1]);
            return Ejecutado;
        }
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026

        public string[] ContabilidadFuncObtenerModulos()
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            List<string> Modulos = new List<string>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(
                "SELECT nombreModuloContable FROM tblModuloContable WHERE is_active = TRUE ORDER BY idModuloContable", CommandType.Text))
                foreach (DataRow Fila in Tabla.Rows) Modulos.Add(Convert.ToString(Fila[0]));
            return Modulos.ToArray();
        }

        private int ContabilidadFuncGuardar(ClsPresupuestoContabilidad Entidad, bool Nuevo)
        {
            using (OdbcConnection Conexion = ContabilidadFuncObtenerConexion())
            {
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        // Sin presupuesto activo no hay nada que modificar
                        if (!Nuevo)
                            using (OdbcCommand Existe = new OdbcCommand(_ExisteEncabezado, Conexion, Transaccion))
                            {
                                Existe.Parameters.AddWithValue("Id", Entidad.IdPresupuesto);
                                if (Convert.ToInt32(Existe.ExecuteScalar()) == 0)
                                {
                                    Transaccion.Rollback();
                                    return 0;
                                }
                            }
                        using (OdbcCommand Comando = new OdbcCommand(Nuevo ? _InsertEncabezado : _UpdateEncabezado, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Nombre", Entidad.Nombre);
                            Comando.Parameters.Add(new OdbcParameter("Total", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Entidad.Total });
                            Comando.Parameters.Add(new OdbcParameter("Gastado", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Entidad.Gastado });
                            Comando.Parameters.Add(new OdbcParameter("Inicio", OdbcType.Date) { Value = Entidad.FechaInicio.Date });
                            Comando.Parameters.Add(new OdbcParameter("Cierre", OdbcType.Date) { Value = Entidad.FechaCierre.HasValue ? (object)Entidad.FechaCierre.Value.Date : DBNull.Value });
                            if (!Nuevo) Comando.Parameters.AddWithValue("Presupuesto", Entidad.IdPresupuesto);
                            if (Comando.ExecuteNonQuery() == 0)
                            {
                                Transaccion.Rollback();
                                return 0;
                            }
                        }
                        if (Nuevo)
                            using (OdbcCommand Comando = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                                Entidad.IdPresupuesto = Convert.ToInt32(Comando.ExecuteScalar());
                        else
                            ContabilidadFuncEjecutar(Conexion, Transaccion, _DesactivarDetalle, Entidad.IdPresupuesto);

                        foreach (ClsPresupuestoDetalleContabilidad Partida in Entidad.Detalle)
                            using (OdbcCommand Comando = new OdbcCommand(_InsertDetalle, Conexion, Transaccion))
                            {
                                Comando.Parameters.AddWithValue("Presupuesto", Entidad.IdPresupuesto);
                                Comando.Parameters.AddWithValue("Nombre", Partida.Nombre);
                                Comando.Parameters.Add(new OdbcParameter("Cantidad", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Partida.Cantidad });
                                Comando.Parameters.AddWithValue("Modulo", Partida.Modulo);
                                if (Comando.ExecuteNonQuery() == 0)
                                    throw new InvalidOperationException("El módulo '" + Partida.Modulo + "' no existe o está inactivo.");
                            }
                        Transaccion.Commit();
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

        private static int ContabilidadFuncEjecutar(OdbcConnection Conexion, OdbcTransaction Transaccion, string Sentencia, int Id)
        {
            using (OdbcCommand Comando = new OdbcCommand(Sentencia, Conexion, Transaccion))
            {
                Comando.Parameters.AddWithValue("Id", Id);
                return Comando.ExecuteNonQuery();
            }
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        }
    }
}
