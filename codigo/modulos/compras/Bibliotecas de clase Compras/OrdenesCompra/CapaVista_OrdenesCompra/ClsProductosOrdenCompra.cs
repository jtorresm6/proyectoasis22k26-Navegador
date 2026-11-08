using System;
using System.Data;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Mantiene las líneas de productos y calcula sus importes y descuentos.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    public sealed class ClsProductosOrdenCompra : IDisposable
    {
        public DataTable Detalles { get; } = new DataTable();
        public decimal Bruto { get; private set; }
        public decimal Descuento { get; private set; }

        public ClsProductosOrdenCompra()
        {
            var Clave = Detalles.Columns.Add("idLinea", typeof(int));
            Clave.AutoIncrement = true;
            Clave.AutoIncrementSeed = 1;
            Detalles.PrimaryKey = new[] { Clave };
            foreach (string Nombre in new[] { "idProducto", "idBodega", "idSucursal", "cantidad" })
                Detalles.Columns.Add(Nombre, typeof(int));
            foreach (string Nombre in new[] { "sku", "producto", "descripcion", "bodega" })
                Detalles.Columns.Add(Nombre, typeof(string));
            foreach (string Nombre in new[] { "peso", "costoBase", "descuentoBase", "porcentaje", "precio", "descuento", "total" })
                Detalles.Columns.Add(Nombre, typeof(decimal));
        }

        public void Guardar(DataRow Editado, DataRow Producto, DataRow Bodega, int Cantidad,
            decimal CostoBase, decimal Descuento, decimal TipoCambio, decimal Costo)
        {
            DataRow Detalle = Editado ?? Detalles.NewRow();
            Detalle.BeginEdit();
            Detalle["idProducto"] = Producto["idProducto"];
            Detalle["idBodega"] = Bodega["idBodega"];
            Detalle["idSucursal"] = Bodega["Fk_Sucursal_Codigo"];
            Detalle["bodega"] = Bodega["nombreBodega"];
            Detalle["sku"] = Producto["skuProducto"];
            Detalle["producto"] = Producto["nombreProducto"];
            Detalle["descripcion"] = Producto["descripcionProducto"];
            Detalle["peso"] = Producto["pesoProducto"];
            Detalle["cantidad"] = Cantidad;
            Detalle["costoBase"] = CostoBase;
            Detalle["descuentoBase"] = Descuento * TipoCambio;
            Detalle["porcentaje"] = Costo == 0 ? 0M : decimal.Round(Descuento * 100M / Costo, 2, MidpointRounding.AwayFromZero);
            Detalle.EndEdit();
            if (Editado == null) Detalles.Rows.Add(Detalle);
        }

        public void Eliminar(DataRow Detalle)
        {
            if (Detalle != null && Detalle.Table == Detalles && Detalle.RowState != DataRowState.Detached)
                Detalles.Rows.Remove(Detalle);
        }

        public void Recalcular(decimal? TipoCambio)
        {
            Bruto = 0M;
            Descuento = 0M;
            foreach (DataRow Fila in Detalles.Rows)
            {
                if (!TipoCambio.HasValue || TipoCambio.Value <= 0M)
                {
                    foreach (string Campo in new[] { "precio", "descuento", "total" }) Fila[Campo] = DBNull.Value;
                    continue;
                }
                decimal Precio = decimal.Round((decimal)Fila["costoBase"] / TipoCambio.Value, 2, MidpointRounding.AwayFromZero);
                decimal Rebaja = decimal.Round((decimal)Fila["descuentoBase"] / TipoCambio.Value, 2, MidpointRounding.AwayFromZero);
                int Cantidad = (int)Fila["cantidad"];
                Fila["precio"] = Precio;
                Fila["descuento"] = Rebaja;
                Fila["total"] = Cantidad * (Precio - Rebaja);
                Bruto += Cantidad * Precio;
                Descuento += Cantidad * Rebaja;
            }
        }

        public void Dispose()
        {
            Detalles.Dispose();
        }
    }
}
