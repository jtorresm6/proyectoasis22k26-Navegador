/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Define el contrato genérico de acceso a datos para agregar,
 *  editar, eliminar y consultar entidades. Permite que los
 *  repositorios compartan las mismas operaciones básicas
 *  independientemente del tipo de entidad que administran.
 * ==================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Contratos
{
    public interface IRepositorioGenerico <Entidad> where Entidad : class
    {
        int RrhhMetAgregar(Entidad Entidad);
        int RrhhMetEditar(Entidad Entidad);
        int RrhhMetRemover(Entidad Entidad);
        IEnumerable<Entidad> RrhhMetObtenerTodos();
    }
}
