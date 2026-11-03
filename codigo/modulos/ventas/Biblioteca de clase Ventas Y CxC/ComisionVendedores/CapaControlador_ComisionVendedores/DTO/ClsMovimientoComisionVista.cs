using System;

namespace CapaControlador_ComisionVendedores.DTO
{
    /// <summary>Datos de comisión que la Vista necesita mostrar o enviar para guardar.</summary>
    public class ClsMovimientoComisionVista
    {
        public int IdVendedor { get; set; }
        public int IdMoneda { get; set; }
        public string SimboloMoneda { get; set; }
        public int IdFactura { get; set; }
        public int IdFacturaDetalle { get; set; }
        public string MetodoComision { get; set; }
        public int IdAplicacionPago { get; set; }
        public int IdComisionAbono { get; set; }
        public int IdReglaComision { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public string NombreVendedor { get; set; }
        public string ElementoComision { get; set; }
        public decimal PorcentajeVendedor { get; set; }
        public decimal BaseVentas { get; set; }
        public decimal MontoComision { get; set; }
    }
}
