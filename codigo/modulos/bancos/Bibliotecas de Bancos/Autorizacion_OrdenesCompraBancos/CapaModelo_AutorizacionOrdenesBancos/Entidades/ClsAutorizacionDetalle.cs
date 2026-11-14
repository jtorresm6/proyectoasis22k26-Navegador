using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * =============================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * =============================================================================
 * Propósito :
 *  La entidad ClsAutorizacionDetalle representa el monto asignado a una cuenta
 *  bancaria para el pago de una orden autorizada.
 * =============================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Entidades
{
    public class ClsAutorizacionDetalle
    {
        public long IdAutorizacionPagoOrden { get; set; }
        public long IdAutorizacionOrden { get; set; }
        public long IdCuenta { get; set; }
        public long IdDisponibilidadFondos { get; set; }
        public decimal MontoAsignadoPagoOrden { get; set; }
        public string EstadoPagoOrden { get; set; }
        public DateTime? FechaReservaPagoOrden { get; set; }
        public long? IdOrdenPago { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
