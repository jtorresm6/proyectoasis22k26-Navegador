using System;
using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;
namespace CapaVista_OrdenesCompra.Presentacion
{
    internal static class ClsEncabezadosOrdenesCompraTabla
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
    }
}
