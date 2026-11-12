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
 *  La entidad ClsTipoCuentaBancaria representa el catálogo de tipos
 *  de cuenta bancaria (tabla tblTipoCuentaBancaria).
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Entidades
{
    public class ClsTipoCuentaBancaria
    {
        public int IdTipoCuentaBancaria { get; set; }
        public string CodigoTipoCuentaBancaria { get; set; }
        public string NombreTipoCuentaBancaria { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}