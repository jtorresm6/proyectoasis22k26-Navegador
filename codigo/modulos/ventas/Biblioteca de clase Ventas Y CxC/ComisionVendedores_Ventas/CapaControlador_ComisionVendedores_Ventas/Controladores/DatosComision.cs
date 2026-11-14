// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Transporta facturas, movimientos, porcentajes y resultados hacia los formularios.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CapaControlador_ComisionVendedores_Ventas.DTO
{
    // Contiene el método, la base y el resultado por línea de factura.
    public class ClsComisionFacturaDetalle
    {
        public int IdFacturaDetalle { get; set; }
        public string MetodoComision { get; set; }
        public string SimboloMoneda { get; set; }
        public bool ComisionGuardada { get; set; }
        public string NombreProducto { get; set; }
        public string ElementoComision { get; set; }
        public double Cantidad { get; set; }
        public decimal BaseVenta { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PorcentajeProducto { get; set; }
        public decimal PorcentajeMarca { get; set; }
        public decimal PorcentajeLinea { get; set; }
        public decimal PorcentajeCategoria { get; set; }
        public string EtiquetaComisionPor { get { return MetodoComision + " · " + Porcentaje.ToString("0.00") + "%"; } }
        public decimal ComisionTotal { get; set; }
        public decimal VentaCobrado { get; set; }
        public decimal ComisionUltimoAbono { get; set; }
        public decimal ComisionDisponible { get; set; }
        public decimal ComisionPendiente { get; set; }
    }

    // Contiene un abono de comisión para la pestaña de comisiones emitidas.
    public class ClsComisionNominaVista
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

    // Agrupa los importes y movimientos del período por vendedor.
    public class ClsComisionVendedorResultado
    {
        public int IdVendedor { get; set; }
        public int IdMoneda { get; set; }
        public string SimboloMoneda { get; set; }
        public string NombreVendedor { get; set; }
        public string ElementoComision { get; set; }
        public string MetodoComision { get; set; }
        public string EtiquetaMetodoPorcentaje { get { return MetodoComision + " / " + Porcentaje.ToString("0.00") + "%"; } }
        public decimal Porcentaje { get; set; }
        public decimal BaseVentas { get; set; }
        public decimal MontoComision { get; set; }

        [Browsable(false)]
        public List<ClsMovimientoComisionVista> Movimientos { get; set; }
    }

    // Identifica una factura disponible para asignación o cálculo.
    public class ClsFacturaComisionVista
    {
        public int IdFactura { get; set; }
        public int IdVendedor { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal UltimoAbono { get; set; }
        public string SimboloMoneda { get; set; }
        public string Etiqueta { get { return "FAC-" + IdFactura + "  |  " + Fecha.ToString("dd/MM/yyyy") + "  |  " + Cliente + "  |  " + SimboloMoneda + " " + Total.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("es-GT")); } }
    }

    // Expone el saldo de la factura y el estado de su comisión en el formulario de abonos.
    public class ClsFacturaPagoVista
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

    // Identifica la forma de pago e indica si requiere referencia.
    public class ClsFormaPagoVentaVista
    {
        public int IdFormaPago { get; set; }
        public string Nombre { get; set; }
        public bool RequiereReferencia { get; set; }
    }

    /// <summary>Datos de comisión que la Vista necesita mostrar o enviar para guardar.</summary>
    // Representa un movimiento devengado o pendiente de liquidación.
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

    // Contiene los porcentajes que se muestran en el mantenimiento de clasificaciones.
    public class ClsProductoPorcentaje
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

    // Expone el vendedor en el selector de asignación.
    public class ClsVendedorComisionVista
    {
        public int IdVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
