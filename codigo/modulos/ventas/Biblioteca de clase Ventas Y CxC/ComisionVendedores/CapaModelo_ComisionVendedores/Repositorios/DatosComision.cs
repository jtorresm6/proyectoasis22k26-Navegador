// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Representa las facturas, los abonos y las clasificaciones que devuelve el repositorio.

using System;

namespace CapaModelo_ComisionVendedores.Entidades
{
    /// <summary>Un abono de comisión generado por un pago individual del cliente.</summary>
    // Representa el devengo de comisión asociado a una aplicación de pago.
    public class ClsComisionNomina
    {
        public int IdAbono { get; set; }
        public int IdFactura { get; set; }
        public DateTime FechaAbono { get; set; }
        public string Vendedor { get; set; }
        public string SimboloMoneda { get; set; }
        public decimal TotalComision { get; set; }
        public decimal MontoPagoCliente { get; set; }
        public decimal ComisionDevengadaAbono { get; set; }
        public string EstadoPago { get; set; }
    }

    // Contiene el método y la base comisionable de una línea de factura.
    public class ClsDetalleProductoComision
    {
        public int IdFacturaDetalle { get; set; }
        public int IdProducto { get; set; }
        public int IdProductoLogistica { get; set; }
        public int IdClasificacionLogistica { get; set; }
        public decimal MontoComisionLineaGuardado { get; set; }
        public string MetodoGuardado { get; set; }
        public string MetodoSeleccionado { get; set; }
        public int IdVendedorGuardado { get; set; }
        public bool ComisionGuardada { get; set; }
        public decimal MontoComisionTotalGuardado { get; set; }
        public decimal ComisionDevengada { get; set; }
        public decimal UltimaComisionDevengada { get; set; }
        public string NombreProducto { get; set; }
        public string Marca { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public string ElementoComision { get; set; }
        public double Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal BaseComisionable { get; set; }
        public int IdReglaComision { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PorcentajeProducto { get; set; }
        public decimal PorcentajeMarca { get; set; }
        public decimal PorcentajeLinea { get; set; }
        public decimal PorcentajeCategoria { get; set; }
    }

    // Representa una factura y sus importes de cobro para calcular comisiones.
    public class ClsFacturaComision
    {
        public int IdFactura { get; set; }
        public int IdVendedor { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal UltimoAbono { get; set; }
        public string SimboloMoneda { get; set; }
    }

    // Representa el saldo de una factura y sus importes de comisión.
    public class ClsFacturaPago
    {
        public int IdFactura { get; set; }
        public int IdCliente { get; set; }
        public long IdMoneda { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string Etiqueta { get; set; }
        public string SimboloMoneda { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Saldo { get; set; }
        public decimal ComisionTotal { get; set; }
        public decimal ComisionDevengada { get; set; }
        public decimal ComisionPagada { get; set; }
        public decimal ComisionPendientePago { get; set; }
    }

    // Representa una forma de pago y su requisito de referencia.
    public class ClsFormaPagoVenta
    {
        public int IdFormaPago { get; set; }
        public string Nombre { get; set; }
        public bool RequiereReferencia { get; set; }
    }

    // Contiene los porcentajes registrados por producto y sus clasificaciones.
    public class ClsProductoComision
    {
        public int IdProducto { get; set; }
        public int IdMarca { get; set; }
        public int IdLinea { get; set; }
        public int IdCategoria { get; set; }
        public string NombreProducto { get; set; }
        public string Marca { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PorcentajeMarca { get; set; }
        public decimal PorcentajeLinea { get; set; }
        public decimal PorcentajeCategoria { get; set; }
        public decimal PorcentajeProducto { get; set; }
    }

    // Identifica un vendedor activo para asignar facturas.
    public class ClsVendedorComision
    {
        public int IdVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public decimal Porcentaje { get; set; }
    }

    // Representa una venta o un abono con importes comisionables.
    public class ClsVentaVendedorBase
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
