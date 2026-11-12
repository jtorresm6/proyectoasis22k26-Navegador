using CapaModelo_AutorizacionOrdenes.Entidades;
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
 * Fecha : 07/11/2026
 * ==================================================================
 * Propósito :
 *  La interfaz IRepositorioTipoCuentaBancaria define el contrato de
 *  las operaciones CRUD del repositorio de Tipo de Cuenta Bancaria
 *  heredando de IRepositorioGenerico.
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Contratos
{
    public interface IRepositorioTipoCuentaBancaria : IRepositorioGenerico<ClsTipoCuentaBancaria>
    {
    }
}