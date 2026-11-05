/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Contrato base de todo repositorio con mantenimiento: agregar,
 *  editar, eliminar y obtener todos. Solo define las firmas.
 * ===================================================================
*/

using System.Collections.Generic;

namespace CapaModelo_Contabilidad.Contratos
{
    public interface IRepositorioGenericoContabilidad<TEntidad> where TEntidad : class
    {
        int ContabilidadFuncAgregar(TEntidad Entidad);

        int ContabilidadFuncEditar(TEntidad Entidad);

        int ContabilidadFuncEliminar(TEntidad Entidad);

        IEnumerable<TEntidad> ContabilidadFuncObtenerTodos();
    }
}
