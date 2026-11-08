using System;
using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Configura las columnas y los formatos de las tablas de órdenes de compra.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra.Presentacion
{
    internal static class ClsTablasOrdenesCompra
    {

        internal static void ComprasProcConfigurarEncabezados(ComprasUsrDataGridView Tabla)
        {
            Tabla.Tabla.RowTemplate.MinimumHeight = 40;
            Tabla.Tabla.RowTemplate.Height = 40;
            Tabla.AutoGenerateColumns = false;
            foreach (var Campo in new[] {
                new[] { "codigoOrdenCompra", "Código", "16" },
                new[] { "nombreProveedor", "Proveedor", "24" },
                new[] { "nitProveedor", "NIT", "11" },
                new[] { "fechaEmisionOrdenCompra", "Emisión", "12" },
                new[] { "fechaVencimientoOrdenCompra", "Vencimiento", "12" },
                new[] { "totalOrdenCompra", "Monto total", "12" },
                new[] { "idMoneda", "Moneda (ID)", "12" },
                new[] { "estadoOrdenCompra", "Estado", "11" } })
            {
                var Columna = new DataGridViewTextBoxColumn { Name = "ComprasCol" + Campo[0],
                    DataPropertyName = Campo[0], HeaderText = Campo[1], FillWeight = int.Parse(Campo[2]), ReadOnly = true };
                if (Campo[0].StartsWith("fecha")) Columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                if (Campo[0] == "totalOrdenCompra")
                {
                    Columna.DefaultCellStyle.Format = "#,##0.0000";
                    Columna.DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;
                    Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                Tabla.Columns.Add(Columna);
            }
            Tabla.ReadOnly = true;
            Tabla.AllowUserToAddRows = false;
            Tabla.AllowUserToDeleteRows = false;
        }

        internal static void ComprasProcConfigurarDetalles(ComprasUsrDataGridView Tabla)
        {
            Tabla.AutoGenerateColumns = false;
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "ordenDetalleOrdenCompra", "L?nea", 10, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "idProducto", "ID producto", 12, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "cantidadDetalleOrdenCompra", "Cantidad", 20, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "costoUnitarioDetalleOrdenCompra", "Costo unitario", 22, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, "descuentoPorcentajeDetalleOrdenCompra", "Descuento %", 20, true);
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
