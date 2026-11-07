using System;
using System.Collections.Generic;

/*
 * ==================================================================
 * Área : Compras
 * Propósito :
 *  Entidades de datos del pago a proveedor (encabezado y detalle).
 *  Reemplazan a ClsPagoProveedor.
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores.Entidades
{
    public class ClsDetallePagoProveedor
    {
        public string SerieDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public int IdConcepto { get; set; }
        public string NombreConcepto { get; set; }
        public decimal MontoAplicado { get; set; }
    }

    public class ClsEncabezadoPagoProveedor
    {
        public int IdPagoProveedor { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public DateTime FechaPagoProveedor { get; set; }
        public decimal MontoPagoProveedor { get; set; }
        public int IdMetodoPago { get; set; }
        public string NombreMetodoPago { get; set; }
        public long IdMoneda { get; set; }
        public long? IdTipoCambio { get; set; }
        public decimal TipoCambioPagoProveedor { get; set; } = 1;
        public string ReferenciaPagoProveedor { get; set; }
        public string EstadoPagoProveedor { get; set; }
        public List<ClsDetallePagoProveedor> Detalles { get; set; } = new List<ClsDetallePagoProveedor>();
    }
}