/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Contrato que Logística implementa para recibir en su inventario los
 *  activos fijos que Contabilidad registra.
 * Reglas especificas:
 *  La clase que lo implemente debe ser pública, tener constructor sin
 *  parámetros y vivir en un DLL cargado o cuyo nombre contenga
 *  "Logistica". Devuelve true con la referencia de Logística cuando el
 *  activo queda registrado; con false el activo queda pendiente.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public interface IReceptorActivoFijoContabilidad
    {
        bool ContabilidadFuncRecibirActivoFijo(ClsModeloActivoLogisticaContabilidad Activo, out string Referencia, out string Mensaje);
    }
}
