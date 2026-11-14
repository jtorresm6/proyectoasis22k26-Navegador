using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ======================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ======================================================================
 * Propósito :
 *  La entidad ClsOrdenCompra representa una orden de compra recibida del
 *  módulo de Compras (tabla tblOrdenCompraRecibida).
 * =======================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Entidades
{
    public class ClsOrdenCompra
    {
        public long IdOrdenCompra { get; set; }
        public string ReferenciaOrdenCompra { get; set; }
        public string ProveedorOrdenCompra { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public DateTime FechaVencimientoOrdenCompra { get; set; }
        public decimal ImporteOrdenCompra { get; set; }
        public string EstadoOrdenCompra { get; set; }
        public long IdMoneda { get; set; }
        public string CodigoMoneda { get; set; }
    }
}
