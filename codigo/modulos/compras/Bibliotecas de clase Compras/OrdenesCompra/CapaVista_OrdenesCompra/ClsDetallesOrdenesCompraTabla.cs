using System;
using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;
namespace CapaVista_OrdenesCompra.Presentacion
{
    internal static class ClsDetallesOrdenesCompraTabla
    {
        internal static void ComprasProcConfigurarDetalles(ComprasUsrDataGridView Tabla)
        {
            Tabla.AutoGenerateColumns = false;
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "ordenDetalleOrdenCompra", "L?nea", 10, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "idProducto", "ID producto", 12, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "cantidadDetalleOrdenCompra", "Cantidad", 20, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "costoUnitarioDetalleOrdenCompra", "Costo unitario", 22, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "descuentoPorcentajeDetalleOrdenCompra", "Descuento %", 20, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "descuentoImporteDetalleOrdenCompra", "Desc. por unidad", 20, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "subtotalDetalleOrdenCompra", "Subtotal", 22, true);
            foreach (DataGridViewColumn Columna in Tabla.Columns)
                Columna.DefaultCellStyle.Format = (Columna.DataPropertyName == "idProducto" || Columna.DataPropertyName == "ordenDetalleOrdenCompra") ? "0" : "N4";
            Tabla.Columns["ComprasColdescuentoPorcentajeDetalleOrdenCompra"].DefaultCellStyle.Format = "N2";
            Tabla.Tabla.DefaultCellStyle.Alignment =
                System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            Tabla.Tabla.ColumnHeadersDefaultCellStyle.Alignment =
                System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            foreach (System.Windows.Forms.DataGridViewColumn Columna in Tabla.Columns)
            {
                Columna.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            }
            Tabla.ReadOnly = true;
            Tabla.AllowUserToAddRows = false;
            Tabla.AllowUserToDeleteRows = false;

            Tabla.ClearSelection();
        }
    }
}
