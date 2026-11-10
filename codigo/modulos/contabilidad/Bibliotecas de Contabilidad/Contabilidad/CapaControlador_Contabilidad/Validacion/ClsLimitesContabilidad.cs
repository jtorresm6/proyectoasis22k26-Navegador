/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Límites comunes de los importes del módulo.
 * Reglas especificas:
 *  EST-01 define los importes como DECIMAL(12,4): hasta 99,999,999.9999.
 *  La interfaz trabaja con dos decimales (Q 1,250.00), por eso el
 *  máximo que se acepta es 99,999,999.99.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public static class ClsLimitesContabilidad
    {
        public const decimal MaximoMonetario = 99999999.99m;
    }
}
