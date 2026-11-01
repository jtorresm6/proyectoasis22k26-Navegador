using System.Collections.Generic;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Define las operaciones comunes para agregar, editar, eliminar y
 *  consultar las entidades administradas por los repositorios.
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores.Contratos
{
   
    public interface IRepositorioGenerico<Entidad> where Entidad : class
    {
        int ComprasMetAgregar(Entidad Entidad);
        int ComprasMetEditar(Entidad Entidad);
        int ComprasMetRemover(Entidad Entidad);
        IEnumerable<Entidad> ComprasMetObtenerTodos();
    }
}