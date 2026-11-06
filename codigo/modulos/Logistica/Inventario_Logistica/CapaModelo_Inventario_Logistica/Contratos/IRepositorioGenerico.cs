/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 * Descripción: Define el contrato genérico para obtener todas las entidades de un repositorio.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Inventario_Logistica.Contratos
{
    public interface IRepositorioGenerico<Entidad> where Entidad : class
    {
        IEnumerable<Entidad> LogisticaMetObtenerTodos();
    }
}
