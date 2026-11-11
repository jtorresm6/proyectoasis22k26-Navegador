/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de cuentas contables. Es de solo lectura:
 *  el mantenimiento de cuentas lo hace el control Navegador.
 * Reglas especificas:
 *  Contrato creado por Oskar Cermeño; se quitaron las operaciones de
 *  escritura y se agregó la consulta de cuentas imputables.
 * ===================================================================
*/

using System.Collections.Generic;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Entidades;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Contratos
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public interface IRepositorioCuentasContabilidad
    {
        IEnumerable<ClsCuentaContabilidad> ContabilidadFuncObtenerTodos();

        IEnumerable<ClsCuentaContabilidad> ContabilidadFuncObtenerTodos(int IdModulo);

        IEnumerable<ClsCuentaContabilidad> ContabilidadFuncObtenerImputables(int IdModulo);

        IEnumerable<ClsCuentaContabilidad> ContabilidadFuncObtenerPadres(ClsCuentaContabilidad Entidad);
    }
}
