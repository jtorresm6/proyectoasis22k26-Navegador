/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 * Descripción: Define el contrato del repositorio para consultar tipos de movimiento.
 */
using CapaModelo_Inventario_Logistica.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Inventario_Logistica.Contratos
{
    public interface IRepositorioTipoMovimiento : IRepositorioGenerico<ClsTipoMovimiento>
    {
    }
}
