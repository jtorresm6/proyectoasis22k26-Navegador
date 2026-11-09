/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Repositorio de activos fijos sobre tblActivoFijo y del maestro de
 *  vida útil sobre tblMaestroVidaUtil.
 * Reglas especificas:
 *  El código AF-#### se asigna al grabar dentro de la transacción. El
 *  valor depreciable lo calcula la base (columna generada). Eliminar es
 *  baja lógica: nunca se usa DELETE.
 * ===================================================================
*/

// Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
using System;
// Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
using System.Collections.Generic;
// Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
using System.Data;
using System.Data.Odbc;
// Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
using CapaModelo_Contabilidad.Contratos;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Entidades;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Repositorios
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
    public class ClsRepositorioActivosFijosContabilidad : ClsRepositorioMaestroContabilidad, IRepositorioActivosFijosContabilidad
    // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
    {
        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        private const string _SelectTodos =
            "SELECT a.idActivoFijo, a.codigoActivoFijo, a.descripcionActivoFijo, m.nombreModuloContable, " +
            "v.codigoMaestroVidaUtil, a.fechaAdquisicionActivoFijo, a.fechaInicioUsoActivoFijo, a.valorCompraActivoFijo, " +
            "a.valorResidualActivoFijo, a.valorEnLibrosActivoFijo, a.mesesVidaUtilActivoFijo FROM tblActivoFijo a " +
            "INNER JOIN tblModuloContable m ON m.idModuloContable = a.idModuloContable " +
            "INNER JOIN tblMaestroVidaUtil v ON v.idMaestroVidaUtil = a.idMaestroVidaUtil " +
            "WHERE a.is_active = TRUE ORDER BY a.codigoActivoFijo";

        private const string _SiguienteCodigo =
            "SELECT COALESCE(MAX(CAST(SUBSTRING(codigoActivoFijo, 4) AS UNSIGNED)), 0) + 1 FROM tblActivoFijo " +
            "WHERE codigoActivoFijo LIKE 'AF-%' FOR UPDATE";

        private const string _Insert =
            "INSERT INTO tblActivoFijo (idModuloContable, idMaestroVidaUtil, codigoActivoFijo, descripcionActivoFijo, " +
            "valorCompraActivoFijo, valorResidualActivoFijo, valorEnLibrosActivoFijo, mesesVidaUtilActivoFijo, fechaAdquisicionActivoFijo, " +
            "fechaInicioUsoActivoFijo) SELECT m.idModuloContable, v.idMaestroVidaUtil, ?, ?, ?, ?, ?, ?, ?, ? " +
            "FROM tblModuloContable m, tblMaestroVidaUtil v WHERE m.nombreModuloContable = ? AND m.is_active = TRUE " +
            "AND v.codigoMaestroVidaUtil = ? AND v.is_active = TRUE LIMIT 1";

        private const string _Update =
            "UPDATE tblActivoFijo a INNER JOIN tblModuloContable m ON m.nombreModuloContable = ? AND m.is_active = TRUE " +
            "INNER JOIN tblMaestroVidaUtil v ON v.codigoMaestroVidaUtil = ? AND v.is_active = TRUE " +
            "SET a.idModuloContable = m.idModuloContable, a.idMaestroVidaUtil = v.idMaestroVidaUtil, a.descripcionActivoFijo = ?, " +
            "a.valorCompraActivoFijo = ?, a.valorResidualActivoFijo = ?, a.valorEnLibrosActivoFijo = ?, " +
            "a.mesesVidaUtilActivoFijo = ?, a.fechaAdquisicionActivoFijo = ?, a.fechaInicioUsoActivoFijo = ? " +
            "WHERE a.idActivoFijo = ? AND a.is_active = TRUE";

        private const string _Desactivar =
            "UPDATE tblActivoFijo SET is_active = FALSE WHERE idActivoFijo = ? AND is_active = TRUE";
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026

        public int ContabilidadFuncAgregar(ClsActivoFijoContabilidad Entidad)
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            using (OdbcConnection Conexion = ContabilidadFuncObtenerConexion())
            {
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        using (OdbcCommand Comando = new OdbcCommand(_SiguienteCodigo, Conexion, Transaccion))
                            Entidad.Codigo = "AF-" + Convert.ToInt32(Comando.ExecuteScalar()).ToString("0000");
                        using (OdbcCommand Comando = new OdbcCommand(_Insert, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Codigo", Entidad.Codigo);
                            ContabilidadMetParametros(Comando, Entidad);
                            Comando.Parameters.AddWithValue("Modulo", Entidad.Modulo);
                            Comando.Parameters.AddWithValue("Tipo", Entidad.Tipo);
                            if (Comando.ExecuteNonQuery() == 0)
                                throw new InvalidOperationException("El módulo o el tipo de activo no existe o está inactivo.");
                        }
                        using (OdbcCommand Comando = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                            Entidad.IdActivo = Convert.ToInt32(Comando.ExecuteScalar());
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
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        public int ContabilidadFuncEditar(ClsActivoFijoContabilidad Entidad)
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            using (OdbcConnection Conexion = ContabilidadFuncObtenerConexion())
            {
                Conexion.Open();
                using (OdbcCommand Comando = new OdbcCommand(_Update, Conexion))
                {
                    Comando.Parameters.AddWithValue("Modulo", Entidad.Modulo);
                    Comando.Parameters.AddWithValue("Tipo", Entidad.Tipo);
                    ContabilidadMetParametros(Comando, Entidad);
                    Comando.Parameters.AddWithValue("Activo", Entidad.IdActivo);
                    return Comando.ExecuteNonQuery();
                }
            }
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        public int ContabilidadFuncEliminar(ClsActivoFijoContabilidad Entidad)
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            return ContabilidadFuncEjecutarNonQuery(_Desactivar,
                new List<OdbcParameter> { new OdbcParameter("Activo", Entidad.IdActivo) }, CommandType.Text);
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        public IEnumerable<ClsActivoFijoContabilidad> ContabilidadFuncObtenerTodos()
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            List<ClsActivoFijoContabilidad> Activos = new List<ClsActivoFijoContabilidad>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectTodos, CommandType.Text))
                foreach (DataRow Fila in Tabla.Rows)
                    Activos.Add(new ClsActivoFijoContabilidad
                    {
                        IdActivo = Convert.ToInt32(Fila["idActivoFijo"]),
                        Codigo = Convert.ToString(Fila["codigoActivoFijo"]),
                        Descripcion = Convert.ToString(Fila["descripcionActivoFijo"]),
                        Modulo = Convert.ToString(Fila["nombreModuloContable"]),
                        Tipo = Convert.ToString(Fila["codigoMaestroVidaUtil"]),
                        Adquisicion = Convert.ToDateTime(Fila["fechaAdquisicionActivoFijo"]),
                        Inicio = Convert.ToDateTime(Fila["fechaInicioUsoActivoFijo"]),
                        Compra = Convert.ToDecimal(Fila["valorCompraActivoFijo"]),
                        Residual = Convert.ToDecimal(Fila["valorResidualActivoFijo"]),
                        ValorEnLibros = Convert.ToDecimal(Fila["valorEnLibrosActivoFijo"]),
                        Meses = Convert.ToInt32(Fila["mesesVidaUtilActivoFijo"])
                    });
            return Activos;
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        public string[] ContabilidadFuncObtenerModulos()
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            List<string> Modulos = new List<string>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(
                "SELECT nombreModuloContable FROM tblModuloContable WHERE is_active = TRUE ORDER BY idModuloContable", CommandType.Text))
                foreach (DataRow Fila in Tabla.Rows) Modulos.Add(Convert.ToString(Fila[0]));
            return Modulos.ToArray();
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        public List<ClsVidaUtilContabilidad> ContabilidadFuncObtenerVidaUtil()
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            List<ClsVidaUtilContabilidad> Vidas = new List<ClsVidaUtilContabilidad>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(
                "SELECT codigoMaestroVidaUtil, nombreMaestroVidaUtil, cantidadMesesMaestroVidaUtil FROM tblMaestroVidaUtil " +
                "WHERE is_active = TRUE ORDER BY nombreMaestroVidaUtil", CommandType.Text))
                foreach (DataRow Fila in Tabla.Rows)
                    Vidas.Add(new ClsVidaUtilContabilidad
                        { Tipo = Convert.ToString(Fila[0]), Nombre = Convert.ToString(Fila[1]), Meses = Convert.ToInt32(Fila[2]) });
            return Vidas;
        }

        private static void ContabilidadMetParametros(OdbcCommand Comando, ClsActivoFijoContabilidad Entidad)
        {
            Comando.Parameters.AddWithValue("Descripcion", Entidad.Descripcion);
            Comando.Parameters.Add(new OdbcParameter("Compra", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Entidad.Compra });
            Comando.Parameters.Add(new OdbcParameter("Residual", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Entidad.Residual });
            Comando.Parameters.Add(new OdbcParameter("Libros", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Entidad.ValorEnLibros });
            Comando.Parameters.AddWithValue("Meses", Entidad.Meses);
            Comando.Parameters.Add(new OdbcParameter("Adquisicion", OdbcType.Date) { Value = Entidad.Adquisicion.Date });
            Comando.Parameters.Add(new OdbcParameter("Inicio", OdbcType.Date) { Value = Entidad.Inicio.Date });
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }
    }
}
