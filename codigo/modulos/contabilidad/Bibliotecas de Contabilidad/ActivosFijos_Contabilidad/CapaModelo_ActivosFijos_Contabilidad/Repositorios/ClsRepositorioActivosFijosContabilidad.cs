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
 *  baja lógica: nunca se usa DELETE. La depreciación del mes se graba
 *  en una sola transacción: póliza VALIDADA y baja del valor en libros.
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

        // Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
        // Depreciación acumulada del activo
        private const string _Acumulada =
            "SELECT valorCompraActivoFijo - valorEnLibrosActivoFijo FROM tblActivoFijo WHERE idActivoFijo = ? AND is_active = TRUE FOR UPDATE";

        // Póliza de depreciación del mes
        private const string _PeriodoDepreciado =
            "SELECT COUNT(*) FROM tblPolizaEncabezado WHERE documentoOrigenPolizaEncabezado = ?";

        // Períodos cerrados
        private const string _PeriodoCerrado =
            "SELECT COUNT(*) FROM tblSaldoCuentaPeriodo WHERE estadoSaldoCuentaPeriodo = 'CERRADO' AND periodoContableSaldoCuentaPeriodo >= ?";

        // Correlativo de la póliza
        private const string _SiguientePoliza =
            "SELECT COALESCE(MAX(CAST(SUBSTRING(codigoPolizaEncabezado, 13) AS UNSIGNED)), 0) + 1 FROM tblPolizaEncabezado " +
            "WHERE periodoContablePolizaEncabezado = ? AND codigoPolizaEncabezado LIKE 'POL-%' FOR UPDATE";

        // Encabezado de la póliza
        private const string _InsertPoliza =
            "INSERT INTO tblPolizaEncabezado (idModuloContable, idPais, codigoPolizaEncabezado, fechaPolizaEncabezado, " +
            "periodoContablePolizaEncabezado, estadoPolizaEncabezado, documentoOrigenPolizaEncabezado, descripcionPolizaEncabezado) " +
            "SELECT m.idModuloContable, COALESCE((SELECT MIN(mcp.idPais) FROM tblModuloCuentaPais mcp WHERE mcp.idModuloContable = m.idModuloContable), 1), " +
            "?, ?, ?, 'VALIDADA', ?, ? FROM tblModuloContable m WHERE m.nombreModuloContable = 'Contabilidad' AND m.is_active = TRUE LIMIT 1";

        // Línea de la póliza
        private const string _InsertDetalle =
            "INSERT INTO tblPolizaDetalle (idPolizaEncabezado, idCuenta, lineaPolizaDetalle, debePolizaDetalle, haberPolizaDetalle, " +
            "descripcionPolizaDetalle) SELECT ?, c.idCuenta, ?, ?, ?, ? FROM tblCuenta c WHERE c.codigoCuenta = ? AND c.is_active = TRUE " +
            "AND c.clasificacionCuenta = 'DETALLE' LIMIT 1";

        // Baja del valor en libros
        private const string _AplicarCuota =
            "UPDATE tblActivoFijo SET valorEnLibrosActivoFijo = valorEnLibrosActivoFijo - ? WHERE idActivoFijo = ? AND is_active = TRUE " +
            "AND valorEnLibrosActivoFijo - ? >= valorResidualActivoFijo";
        // Fin de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026

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
                // Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        object Acumulada;
                        using (OdbcCommand Comando = new OdbcCommand(_Acumulada, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Activo", Entidad.IdActivo);
                            Acumulada = Comando.ExecuteScalar();
                        }
                        if (Acumulada == null || Acumulada == DBNull.Value)
                        {
                            Transaccion.Rollback();
                            return 0;
                        }
                        Entidad.ValorEnLibros = Math.Max(Entidad.Compra - Convert.ToDecimal(Acumulada), Entidad.Residual);
                        int Filas;
                        using (OdbcCommand Comando = new OdbcCommand(_Update, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Modulo", Entidad.Modulo);
                            Comando.Parameters.AddWithValue("Tipo", Entidad.Tipo);
                            ContabilidadMetParametros(Comando, Entidad);
                            Comando.Parameters.AddWithValue("Activo", Entidad.IdActivo);
                            Filas = Comando.ExecuteNonQuery();
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
                // Fin de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
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

        // Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
        // Indica si el mes ya tiene póliza de depreciación
        public bool ContabilidadFuncPeriodoDepreciado(string Periodo)
        {
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_PeriodoDepreciado, CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Documento", "DEP-" + Periodo) }))
                return Convert.ToInt32(Tabla.Rows[0][0]) > 0;
        }

        // Graba la póliza del mes y baja el valor en libros
        public string ContabilidadFuncRegistrarDepreciacion(string Periodo, DateTime Fecha, string Descripcion,
            List<ClsCuotaDepreciacionContabilidad> Cuotas, List<ClsLineaDepreciacionContabilidad> Lineas)
        {
            using (OdbcConnection Conexion = ContabilidadFuncObtenerConexion())
            {
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        using (OdbcCommand Comando = new OdbcCommand(_PeriodoCerrado, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Periodo", Periodo);
                            if (Convert.ToInt32(Comando.ExecuteScalar()) > 0)
                                throw new InvalidOperationException("El período " + Periodo + " ya está cerrado.");
                        }
                        using (OdbcCommand Comando = new OdbcCommand(_PeriodoDepreciado, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Documento", "DEP-" + Periodo);
                            if (Convert.ToInt32(Comando.ExecuteScalar()) > 0)
                                throw new InvalidOperationException("El mes " + Periodo + " ya tiene póliza de depreciación.");
                        }
                        string Codigo;
                        using (OdbcCommand Comando = new OdbcCommand(_SiguientePoliza, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Periodo", Periodo);
                            Codigo = "POL-" + Periodo + "-" + Convert.ToInt32(Comando.ExecuteScalar()).ToString("0000");
                        }
                        using (OdbcCommand Comando = new OdbcCommand(_InsertPoliza, Conexion, Transaccion))
                        {
                            Comando.Parameters.AddWithValue("Codigo", Codigo);
                            Comando.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                            Comando.Parameters.AddWithValue("Periodo", Periodo);
                            Comando.Parameters.AddWithValue("Documento", "DEP-" + Periodo);
                            Comando.Parameters.AddWithValue("Descripcion", Descripcion);
                            if (Comando.ExecuteNonQuery() == 0)
                                throw new InvalidOperationException("No existe el módulo contable Contabilidad.");
                        }
                        int IdPoliza;
                        using (OdbcCommand Comando = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                            IdPoliza = Convert.ToInt32(Comando.ExecuteScalar());
                        for (int Indice = 0; Indice < Lineas.Count; Indice++)
                            using (OdbcCommand Comando = new OdbcCommand(_InsertDetalle, Conexion, Transaccion))
                            {
                                Comando.Parameters.AddWithValue("Poliza", IdPoliza);
                                Comando.Parameters.AddWithValue("Linea", Indice + 1);
                                Comando.Parameters.Add(new OdbcParameter("Debe", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Lineas[Indice].Debe });
                                Comando.Parameters.Add(new OdbcParameter("Haber", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Lineas[Indice].Haber });
                                Comando.Parameters.AddWithValue("Descripcion", Lineas[Indice].Descripcion);
                                Comando.Parameters.AddWithValue("Cuenta", Lineas[Indice].CodigoCuenta);
                                if (Comando.ExecuteNonQuery() == 0)
                                    throw new InvalidOperationException("La cuenta " + Lineas[Indice].CodigoCuenta + " no existe o no es de detalle.");
                            }
                        foreach (ClsCuotaDepreciacionContabilidad Cuota in Cuotas)
                            using (OdbcCommand Comando = new OdbcCommand(_AplicarCuota, Conexion, Transaccion))
                            {
                                Comando.Parameters.Add(new OdbcParameter("Monto", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Cuota.Monto });
                                Comando.Parameters.AddWithValue("Activo", Cuota.IdActivo);
                                Comando.Parameters.Add(new OdbcParameter("Tope", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Cuota.Monto });
                                if (Comando.ExecuteNonQuery() == 0)
                                    throw new InvalidOperationException("Un activo cambió mientras se depreciaba. Vuelva a intentarlo.");
                            }
                        Transaccion.Commit();
                        return Codigo;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
        // Fin de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026

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
