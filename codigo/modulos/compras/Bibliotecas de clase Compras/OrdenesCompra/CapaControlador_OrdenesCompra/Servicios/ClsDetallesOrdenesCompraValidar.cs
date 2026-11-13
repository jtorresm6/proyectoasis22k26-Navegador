using System;
namespace CapaControlador_OrdenesCompra.Servicios
{
    public static class ClsDetallesOrdenesCompraValidar
    {
        public static void ComprasProcValidarLimitesProducto(decimal Cantidad, decimal Precio,
            decimal DescuentoUnitario, decimal SubtotalAnterior = 0M, decimal DescuentoAnterior = 0M)
        {
            const decimal Maximo = 99999999.9999M;
            if (Cantidad <= 0M || Cantidad > Maximo || decimal.Round(Cantidad, 4) != Cantidad)
                throw new ArgumentException("La cantidad debe ser mayor que cero, no superar 99,999,999.9999 y tener hasta 4 decimales.");
            if (Precio < 0M || Precio > Maximo || decimal.Round(Precio, 4) != Precio)
                throw new ArgumentException("El precio unitario no puede superar 99,999,999.9999 ni tener más de 4 decimales.");
            if (DescuentoUnitario < 0M || DescuentoUnitario > Precio || decimal.Round(DescuentoUnitario, 4) != DescuentoUnitario)
                throw new ArgumentException("El descuento por unidad debe estar entre cero y el precio unitario, con hasta 4 decimales.");
            decimal Bruto = Cantidad * Precio;
            decimal Rebaja = Cantidad * DescuentoUnitario;
            if (Bruto > Maximo || Bruto - Rebaja > Maximo)
                throw new ArgumentException("El importe del producto supera el máximo de 99,999,999.9999. Reduce la cantidad o el precio unitario.");
            if (SubtotalAnterior < 0M || DescuentoAnterior < 0M || DescuentoAnterior > SubtotalAnterior
                || SubtotalAnterior + Bruto > Maximo || DescuentoAnterior + Rebaja > Maximo
                || SubtotalAnterior + Bruto - DescuentoAnterior - Rebaja > Maximo)
                throw new ArgumentException("Al agregar o modificar este producto, el acumulado de la orden supera el máximo de 99,999,999.9999.");
            if (decimal.Round(SubtotalAnterior + Bruto, 4) != SubtotalAnterior + Bruto
                || decimal.Round(DescuentoAnterior + Rebaja, 4) != DescuentoAnterior + Rebaja)
                throw new ArgumentException("Los importes acumulados de la orden admiten hasta 4 decimales.");
        }
    }
}
