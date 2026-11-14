using System;
namespace CapaControlador_OrdenesCompra.Servicios
{
    public static class ClsOrdenesCompraCalcular
    {
        public static decimal ComprasFuncCalcularDescuento(decimal Precio, decimal Porcentaje)
        {
            if (Precio < 0M || Precio > 99999999.9999M || decimal.Round(Precio, 4) != Precio
                || Porcentaje < 0M || Porcentaje > 99.99M || decimal.Round(Porcentaje, 2) != Porcentaje)
                throw new ArgumentException("Ingresa un porcentaje entre 0 y 99.99 con hasta 2 decimales y un precio unitario válido.");
            return decimal.Round(Precio * Porcentaje / 100M, 4, MidpointRounding.AwayFromZero);
        }

        public static decimal ComprasFuncCalcularTotalProducto(decimal Cantidad, decimal Precio, decimal Porcentaje)
        {
            if (Cantidad <= 0M || Cantidad > 99999999.9999M || decimal.Round(Cantidad, 4) != Cantidad)
                throw new ArgumentException("Ingresa una cantidad válida con hasta 4 decimales.");
            ComprasFuncCalcularDescuento(Precio, Porcentaje);
            return decimal.Round(Cantidad * Precio * (1M - Porcentaje / 100M), 4, MidpointRounding.AwayFromZero);
        }

        public static decimal ComprasFuncCalcularImpuesto(decimal SubtotalNeto, decimal Porcentaje)
        {
            if (SubtotalNeto < 0M || SubtotalNeto > 99999999.9999M || decimal.Round(SubtotalNeto, 4) != SubtotalNeto)
                throw new ArgumentException("El subtotal neto excede el rango o los 4 decimales permitidos.");
            if (Porcentaje < 0M || Porcentaje > 999.99M || decimal.Round(Porcentaje, 2) != Porcentaje)
                throw new ArgumentException("Ingresa un impuesto entre 0 y 999.99 % con hasta 2 decimales.");
            decimal Importe = decimal.Round(SubtotalNeto * Porcentaje / 100M, 4, MidpointRounding.AwayFromZero);
            if (Importe > 99999999.9999M)
                throw new ArgumentException("El importe del impuesto supera el máximo de 99,999,999.9999 permitido por la base de datos.");
            if (SubtotalNeto + Importe > 99999999.9999M)
                throw new ArgumentException("El total con impuesto supera el máximo de 99,999,999.9999. Reduce el impuesto o los importes de la orden.");
            return Importe;
        }
    }
}
