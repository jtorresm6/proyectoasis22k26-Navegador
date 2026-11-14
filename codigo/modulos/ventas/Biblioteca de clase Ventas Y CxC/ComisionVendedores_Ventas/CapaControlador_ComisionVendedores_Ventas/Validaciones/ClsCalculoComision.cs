// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Valida las entradas y concentra las reglas de cálculo que pueden probarse sin conexión.

using System;
using CapaControlador_ComisionVendedores_Ventas.DTO;

namespace CapaControlador_ComisionVendedores_Ventas.Validaciones
{
    public static class ClsCalculoComision
    {
        public static void VentasMetActualizarVistaPrevia(ClsComisionFacturaDetalle Detalle, string Metodo)
        {
            decimal Porcentaje = Metodo == "Producto" ? Detalle.PorcentajeProducto
                : Metodo == "Marca" ? Detalle.PorcentajeMarca
                : Metodo == "Linea" ? Detalle.PorcentajeLinea : Detalle.PorcentajeCategoria;
            Detalle.MetodoComision = Metodo;
            Detalle.Porcentaje = Porcentaje;
            Detalle.ComisionTotal = Math.Round(Detalle.BaseVenta * Porcentaje / 100m, 4, MidpointRounding.AwayFromZero);
            Detalle.ComisionPendiente = Math.Max(0m, Detalle.ComisionTotal - Detalle.ComisionDisponible);
        }

        public static decimal VentasMetEstimarAbono(ClsFacturaPagoVista Factura, decimal Monto)
        {
            decimal Proporcion = Math.Min(1m, (Factura.TotalPagado + Monto) / Factura.Total);
            decimal Objetivo = Math.Round(Factura.ComisionTotal * Proporcion, 4, MidpointRounding.AwayFromZero);
            return Math.Max(0m, Objetivo - Factura.ComisionDevengada);
        }
    }
}
