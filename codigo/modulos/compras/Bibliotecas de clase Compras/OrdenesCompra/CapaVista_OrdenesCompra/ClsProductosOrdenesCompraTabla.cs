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
    public sealed partial class ClsProductosOrdenesCompraTabla : IDisposable
    {
        public DataTable Detalles { get; } = new DataTable();
        public decimal Bruto { get; private set; }
        public decimal Descuento { get; private set; }

        public ClsProductosOrdenesCompraTabla()
        {
            var Clave = Detalles.Columns.Add("idLinea", typeof(int));
            Clave.AutoIncrement = true;
            Clave.AutoIncrementSeed = 1;
            Detalles.PrimaryKey = new[] { Clave };
            foreach (string Nombre in new[] { "idProducto", "idBodega", "idSucursal", "cantidad" })
                Detalles.Columns.Add(Nombre, typeof(int));
            foreach (string Nombre in new[] { "sku", "producto", "descripcion", "bodega", "sucursal" })
                Detalles.Columns.Add(Nombre, typeof(string));
            foreach (string Nombre in new[] { "peso", "costoBase", "porcentaje", "precio", "descuento", "descuentoTotal", "total" })
                Detalles.Columns.Add(Nombre, typeof(decimal));
        }

        public void Guardar(DataRow Editado, DataRow Producto, DataRow Bodega, int Cantidad,
            decimal CostoBase, decimal TipoCambio, decimal Costo, decimal Porcentaje, string NombreSucursal = null)
        {
            if (TipoCambio <= 0M || TipoCambio > 999999.999999M || decimal.Round(TipoCambio, 6) != TipoCambio)
                throw new ArgumentException("El tipo de cambio debe ser positivo, no superar 999,999.999999 y tener hasta 6 decimales.");
            CapaControlador_OrdenesCompra.ClsOrdenesCompraControlador.ComprasFuncCalcularDescuento(Costo, Porcentaje);
            decimal SubtotalAnterior = 0M;
            foreach (DataRow Fila in Detalles.Rows)
            {
                if (ReferenceEquals(Fila, Editado)) continue;
                decimal Precio = decimal.Round((decimal)Fila["costoBase"] / TipoCambio, 2, MidpointRounding.AwayFromZero);
                CapaControlador_OrdenesCompra.ClsOrdenesCompraControlador.ComprasFuncCalcularDescuento(Precio, (decimal)Fila["porcentaje"]);
                CapaControlador_OrdenesCompra.ClsOrdenesCompraControlador.ComprasProcValidarLimitesProducto(
                    (int)Fila["cantidad"], Precio, 0M, SubtotalAnterior, 0M);
                SubtotalAnterior += (int)Fila["cantidad"] * Precio;

            }
            CapaControlador_OrdenesCompra.ClsOrdenesCompraControlador.ComprasProcValidarLimitesProducto(
                Cantidad, Costo, 0M, SubtotalAnterior, 0M);
            DataRow Detalle = Editado ?? Detalles.NewRow();
            Detalle.BeginEdit();
            Detalle["idProducto"] = Producto["idProducto"];
            Detalle["idBodega"] = Bodega["idBodega"];
            Detalle["idSucursal"] = Bodega["Fk_Sucursal_Codigo"];
            Detalle["bodega"] = Bodega["nombreBodega"];
            Detalle["sucursal"] = !string.IsNullOrWhiteSpace(NombreSucursal) ? NombreSucursal
                : Bodega.Table.Columns.Contains("nombreSucursal") && !Bodega.IsNull("nombreSucursal")
                    ? Convert.ToString(Bodega["nombreSucursal"]) : string.Empty;
            Detalle["sku"] = Producto["skuProducto"];
            Detalle["producto"] = Producto["nombreProducto"];
            Detalle["descripcion"] = Producto["descripcionProducto"];
            Detalle["peso"] = Producto["pesoProducto"];
            Detalle["cantidad"] = Cantidad;
            Detalle["costoBase"] = CostoBase;
            Detalle["porcentaje"] = Porcentaje;
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
                    foreach (string Campo in new[] { "precio", "descuento", "descuentoTotal", "total" }) Fila[Campo] = DBNull.Value;
                    continue;
                }
                decimal Precio = decimal.Round((decimal)Fila["costoBase"] / TipoCambio.Value, 2, MidpointRounding.AwayFromZero);
                decimal Porcentaje = (decimal)Fila["porcentaje"];
                decimal Rebaja = CapaControlador_OrdenesCompra.ClsOrdenesCompraControlador.ComprasFuncCalcularDescuento(Precio, Porcentaje);
                int Cantidad = (int)Fila["cantidad"];
                Fila["precio"] = Precio;
                Fila["descuento"] = Rebaja;
                decimal Total = CapaControlador_OrdenesCompra.ClsOrdenesCompraControlador.ComprasFuncCalcularTotalProducto(Cantidad, Precio, Porcentaje);
                Fila["total"] = Total;
                Bruto += Cantidad * Precio;
                Fila["descuentoTotal"] = Cantidad * Precio - Total;
                Descuento += (decimal)Fila["descuentoTotal"];
            }
        }

        public void Dispose()
        {
            Detalles.Dispose();
        }

    }
}
