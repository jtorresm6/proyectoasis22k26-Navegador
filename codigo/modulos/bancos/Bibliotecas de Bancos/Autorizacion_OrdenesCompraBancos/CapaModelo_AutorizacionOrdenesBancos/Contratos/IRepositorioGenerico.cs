using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  La interfaz IRepositorioGenerico define el contrato de las operaciones CRUD
 *  (agregar, editar, remover y obtener) que deben implementar los repositorios
 *  de la capa modelo.
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Contratos
{
    public interface IRepositorioGenerico<Entidad> where Entidad : class
    {
        int BancosMetAgregar(Entidad Entidad);
        int BancosMetEditar(Entidad Entidad);
        int BancosMetRemover(Entidad Entidad);
        IEnumerable<Entidad> BancosMetObtenerTodos();
    }
}
