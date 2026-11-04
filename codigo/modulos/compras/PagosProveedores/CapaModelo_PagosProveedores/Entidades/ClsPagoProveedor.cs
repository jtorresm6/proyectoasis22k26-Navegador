using System;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Representa los datos de un pago a proveedor, su factura asociada,
 *  importe, fecha, método, referencia y estado.
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores.Entidades
{
    public class ClsPagoProveedor
    {
        public int IdPagoProveedor { get; set; }
        public string NoPagoProveedor { get; set; }        
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }        
        public int IdCuentaPorPagar { get; set; }
        public string FacturaCuentaPorPagar { get; set; }  
        public DateTime FechaPagoProveedor { get; set; }
        public decimal MontoPagoProveedor { get; set; }
        public string MetodoPagoProveedor { get; set; }
        public string ReferenciaPagoProveedor { get; set; }
        public string EstadoPagoProveedor { get; set; }
    }
}
