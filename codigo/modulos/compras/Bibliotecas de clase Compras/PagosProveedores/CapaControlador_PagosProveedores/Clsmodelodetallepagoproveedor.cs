using System;
using System.ComponentModel.DataAnnotations;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Representa un documento por pagar al que se aplica un pago a proveedor
 *  (una línea del detalle del pago).
 * ===================================================================
*/

namespace CapaControlador_PagosProveedores
{
    public class ClsModeloDetallePagoProveedor
    {
        [Required(ErrorMessage = "La serie del documento es requerida")]
        [StringLength(20, ErrorMessage = "La Serie no puede exceder 20 caracteres")]
        public string SerieDocumento { get; set; }

        [Required(ErrorMessage = "El número del documento es requerido")]
        [StringLength(30, ErrorMessage = "El Número no puede exceder 30 caracteres")]
        public string NumeroDocumento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el concepto del documento")]
        public int IdConcepto { get; set; }

        // Monto expresado en la moneda del pago
        public decimal MontoAplicado { get; set; }

        // Solo informativos (para mostrar en la grilla, no se guardan)
        public string NombreConcepto { get; set; }
        public decimal SaldoDocumento { get; set; }

        public string DocumentoClave => SerieDocumento + "|" + NumeroDocumento + "|" + IdConcepto;
    }
}
