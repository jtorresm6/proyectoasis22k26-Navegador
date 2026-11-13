/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Cuenta los registros que muestran los seis indicadores del MDI:
 *  pólizas, presupuestos vigentes, activos fijos, períodos cerrados,
 *  pólizas aplicadas y cuentas contables.
 * Reglas especificas:
 *  Solo cuenta registros activos. Las pólizas anuladas no cuentan; las
 *  aplicadas son las que entran en los estados financieros.
 *  Un presupuesto es vigente si ya empezó y no ha cerrado. Un período
 *  está cerrado cuando sus saldos quedaron en CERRADO.
 * ===================================================================
*/

using System;
using System.Data;

namespace CapaModelo_Contabilidad.Repositorios
{
    public class ClsRepositorioIndicadoresContabilidad : ClsRepositorioMaestroContabilidad
    {
        // pólizas
        public int ContabilidadFuncContarPolizas()
        {
            return ContabilidadFuncContar("SELECT COUNT(*) FROM tblPolizaEncabezado " +
                "WHERE is_active = TRUE AND estadoPolizaEncabezado <> 'ANULADA'");
        }

        // presupuestos vigentes
        public int ContabilidadFuncContarPresupuestosVigentes()
        {
            return ContabilidadFuncContar("SELECT COUNT(*) FROM tblPresupuestoEncabezado " +
                "WHERE is_active = TRUE AND fechaInicioPresupuestoEncabezado <= CURDATE() " +
                "AND (fechaCierrePresupuestoEncabezado IS NULL OR fechaCierrePresupuestoEncabezado >= CURDATE())");
        }

        // activos fijos
        public int ContabilidadFuncContarActivosFijos()
        {
            return ContabilidadFuncContar("SELECT COUNT(*) FROM tblActivoFijo WHERE is_active = TRUE");
        }

        // períodos cerrados
        public int ContabilidadFuncContarPeriodosCerrados()
        {
            return ContabilidadFuncContar("SELECT COUNT(DISTINCT periodoContableSaldoCuentaPeriodo) FROM tblSaldoCuentaPeriodo " +
                "WHERE is_active = TRUE AND estadoSaldoCuentaPeriodo = 'CERRADO'");
        }

        // pólizas aplicadas
        public int ContabilidadFuncContarPolizasAplicadas()
        {
            return ContabilidadFuncContar("SELECT COUNT(*) FROM tblPolizaEncabezado " +
                "WHERE is_active = TRUE AND estadoPolizaEncabezado = 'APLICADA'");
        }

        // cuentas contables
        public int ContabilidadFuncContarCuentas()
        {
            return ContabilidadFuncContar("SELECT COUNT(*) FROM tblCuenta WHERE is_active = TRUE");
        }

        // primer valor de la consulta
        private int ContabilidadFuncContar(string Consulta)
        {
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(Consulta, CommandType.Text))
                return Tabla.Rows.Count == 0 || Tabla.Rows[0][0] == DBNull.Value ? 0 : Convert.ToInt32(Tabla.Rows[0][0]);
        }
    }
}
