using CapaModelo_AutorizacionOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==========================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==========================================================================
 * Propósito :
 *  La interfaz IRepositorioAutorizacionOrdenCompra es el contrato de la capa
 *  modelo para las operaciones sobre la autorización de órdenes de compra.
 * ==========================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Contratos
{
    public interface IRepositorioAutorizacionOrdenCompra : IRepositorioGenerico<ClsAutorizacionOrdenCompra>
    {
        int BancosMetObtenerIdEmpresa();
    }
}
