/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Entrega al MDI los seis indicadores del resumen del sistema.
 * Reglas especificas:
 *  Cada indicador se lee aparte: si uno falla queda nulo y los demás
 *  se muestran igual.
 * ===================================================================
*/

using System;
using CapaModelo_Contabilidad.Repositorios;

namespace CapaControlador_Contabilidad
{
    public class ClsControladorIndicadoresContabilidad
    {
        private readonly ClsRepositorioIndicadoresContabilidad _Repositorio = new ClsRepositorioIndicadoresContabilidad();

        // indicadores del MDI
        public ClsModeloIndicadoresContabilidad ContabilidadFuncObtener()
        {
            return new ClsModeloIndicadoresContabilidad
            {
                Polizas = ContabilidadFuncLeer(_Repositorio.ContabilidadFuncContarPolizas),
                PresupuestosVigentes = ContabilidadFuncLeer(_Repositorio.ContabilidadFuncContarPresupuestosVigentes),
                ActivosFijos = ContabilidadFuncLeer(_Repositorio.ContabilidadFuncContarActivosFijos),
                PeriodosCerrados = ContabilidadFuncLeer(_Repositorio.ContabilidadFuncContarPeriodosCerrados),
                PolizasAplicadas = ContabilidadFuncLeer(_Repositorio.ContabilidadFuncContarPolizasAplicadas),
                Cuentas = ContabilidadFuncLeer(_Repositorio.ContabilidadFuncContarCuentas)
            };
        }

        // un indicador o nulo
        private static int? ContabilidadFuncLeer(Func<int> Contar)
        {
            try
            {
                return Contar();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
