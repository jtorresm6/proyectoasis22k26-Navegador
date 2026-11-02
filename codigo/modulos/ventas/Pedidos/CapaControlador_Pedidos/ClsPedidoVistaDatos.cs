using System;
using System.Collections.Generic;

namespace CapaControlador_Pedidos
{
    public class ClsCatalogoPedido
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public sealed class ClsCotizacionPedido : ClsCatalogoPedido
    {
        public string Numero { get; set; }
        public int ClienteId { get; set; }
        public int VendedorId { get; set; }
        public int MonedaId { get; set; }
        public decimal TipoCambio { get; set; } = 1m;
        public DateTime Vence { get; set; }
        public List<ClsLineaPedido> Lineas { get; set; } = new List<ClsLineaPedido>();
    }

    public sealed class ClsLineaPedido
    {
        public int ProductoId { get; set; }
        public int? CotizacionDetalleId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }

    public sealed class ClsPedido
    {
        public string Numero { get; set; }
        public int? CotizacionId { get; set; }
        public string Referencia { get; set; }
        public int ClienteId { get; set; }
        public int VendedorId { get; set; }
        public int MonedaId { get; set; }
        public decimal TipoCambio { get; set; } = 1m;
        public DateTime Fecha { get; set; }
        public DateTime Entrega { get; set; }
        public DateTime Vencimiento { get; set; }
        public List<ClsLineaPedido> Lineas { get; set; } = new List<ClsLineaPedido>();
    }
}
