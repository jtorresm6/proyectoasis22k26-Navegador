/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Lee de tblActivoFijo, tblModuloContable y tblMaestroVidaUtil los
 *  activos vigentes adquiridos hasta la fecha de corte.
 * Reglas especificas:
 *  Solo lectura. Los activos dados de baja (is_active = FALSE) no
 *  salen. Parámetros ODBC posicionales con ?.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;

namespace CapaModelo_Contabilidad.Repositorios
{
    // Repositorio del reporte
    public class ClsRepositorioReporteActivosFijosContabilidad : ClsRepositorioMaestroContabilidad, IRepositorioReporteActivosFijosContabilidad
    {
        // Consulta de activos
        private const string _SelectAlCorte =
            "SELECT a.idActivoFijo, a.codigoActivoFijo, a.descripcionActivoFijo, m.nombreModuloContable, v.codigoMaestroVidaUtil, " +
            "v.nombreMaestroVidaUtil, a.fechaAdquisicionActivoFijo, a.fechaInicioUsoActivoFijo, a.valorCompraActivoFijo, " +
            "a.valorResidualActivoFijo, a.valorEnLibrosActivoFijo, a.mesesVidaUtilActivoFijo FROM tblActivoFijo a " +
            "INNER JOIN tblModuloContable m ON m.idModuloContable = a.idModuloContable " +
            "INNER JOIN tblMaestroVidaUtil v ON v.idMaestroVidaUtil = a.idMaestroVidaUtil " +
            "WHERE a.is_active = TRUE AND a.fechaAdquisicionActivoFijo <= ? " +
            "AND (? = '' OR m.nombreModuloContable = ?) AND (? = '' OR v.codigoMaestroVidaUtil = ?) " +
            "ORDER BY a.codigoActivoFijo";

        // Lee los activos
        public List<ClsActivoReporteContabilidad> ContabilidadFuncObtenerActivosAlCorte(DateTime Corte, string Modulo, string Tipo)
        {
            string FiltroModulo = (Modulo ?? string.Empty).Trim();
            string FiltroTipo = (Tipo ?? string.Empty).Trim();
            List<OdbcParameter> Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("Corte", OdbcType.Date) { Value = Corte.Date },
                new OdbcParameter("Modulo", FiltroModulo), new OdbcParameter("ModuloIgual", FiltroModulo),
                new OdbcParameter("Tipo", FiltroTipo), new OdbcParameter("TipoIgual", FiltroTipo)
            };
            List<ClsActivoReporteContabilidad> Activos = new List<ClsActivoReporteContabilidad>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectAlCorte, CommandType.Text, Parametros))
                foreach (DataRow Fila in Tabla.Rows)
                    Activos.Add(new ClsActivoReporteContabilidad
                    {
                        IdActivo = Convert.ToInt32(Fila["idActivoFijo"]),
                        Codigo = Convert.ToString(Fila["codigoActivoFijo"]),
                        Descripcion = Convert.ToString(Fila["descripcionActivoFijo"]),
                        Modulo = Convert.ToString(Fila["nombreModuloContable"]),
                        Tipo = Convert.ToString(Fila["codigoMaestroVidaUtil"]),
                        NombreTipo = Convert.ToString(Fila["nombreMaestroVidaUtil"]),
                        Adquisicion = Convert.ToDateTime(Fila["fechaAdquisicionActivoFijo"]),
                        Inicio = Convert.ToDateTime(Fila["fechaInicioUsoActivoFijo"]),
                        Compra = Convert.ToDecimal(Fila["valorCompraActivoFijo"]),
                        Residual = Convert.ToDecimal(Fila["valorResidualActivoFijo"]),
                        ValorEnLibros = Convert.ToDecimal(Fila["valorEnLibrosActivoFijo"]),
                        Meses = Convert.ToInt32(Fila["mesesVidaUtilActivoFijo"])
                    });
            return Activos;
        }
    }
}
