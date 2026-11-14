/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Consulta el total de comisiones aprobadas de un empleado
 *  dentro del período solicitado. Relaciona al empleado con
 *  el vendedor, los encabezados y los detalles de comisiones
 *  para obtener el monto que se incorporará a la nómina.
 * ==================================================================
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Repositorios
{
    public class ClsRepositorioComisiones : ClsSentencias
    {
        // Poner en false para ignorar las comisiones 
        private static readonly bool COMISIONES_ACTIVAS = true;

        
        private const string SQL_TOTAL_COMISIONES =
            "SELECT COALESCE(SUM(cd.montoComisionDetalle), 0) " +
            "FROM tblComisionEncabezado ce " +
            "INNER JOIN tblVendedor v ON v.Pk_Vendedor = ce.Fk_Vendedor_Codigo " +
            "INNER JOIN tblComisionDetalle cd ON cd.Fk_ComisionEncabezado_Codigo = ce.Pk_ComisionEncabezado " +
            "WHERE v.Fk_Empleado_Codigo = ? AND ce.estadoComisionEncabezado = 'APROBADA' " +
            "AND ce.fechaHastaComisionEncabezado BETWEEN ? AND ?";

        public decimal RrhhMetObtenerTotalComisiones(int IdEmpleado, DateTime FechaInicio, DateTime FechaFinal)
        {
            if (!COMISIONES_ACTIVAS)
                return 0;

            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idEmpleado", OdbcType.Int) { Value = IdEmpleado });
            Parametros.Add(new OdbcParameter("P_fechaInicio", OdbcType.VarChar) { Value = FechaInicio.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
            Parametros.Add(new OdbcParameter("P_fechaFinal", OdbcType.VarChar) { Value = FechaFinal.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });

            var Tabla = RrhhMetEjecucionConsulta(SQL_TOTAL_COMISIONES, CommandType.Text, Parametros);
            return Convert.ToDecimal(Tabla.Rows[0][0]);
        }
    }
}
