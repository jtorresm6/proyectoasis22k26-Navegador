using System.Windows.Forms;
using CapaVista_Componentes.Componentes;
namespace CapaVista_OrdenesCompra
{
    public sealed partial class ClsProductosOrdenesCompraTabla
    {
        internal static void ConfigurarColumnas(ComprasUsrDataGridView Tabla)
        {
            Tabla.AutoGenerateColumns = false;
            Tabla.ReadOnly = true;
            Tabla.AllowUserToAddRows = false;
            Tabla.AllowUserToDeleteRows = false;
            Tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Tabla.Columns.Clear();
            string[] Campos = { "sku", "producto", "cantidad", "precio", "descuento", "porcentaje", "descuentoTotal", "total" };
            string[] Titulos = { "SKU", "Producto", "Cantidad", "Precio unitario", "Desc. por unidad", "Desc. %", "Descuento total", "Total" };
            for (int Indice = 0; Indice < Campos.Length; Indice++)
            {
                var Columna = new DataGridViewTextBoxColumn
                {
                    Name = "ComprasCol" + Campos[Indice], DataPropertyName = Campos[Indice], HeaderText = Titulos[Indice],
                    ReadOnly = true, MinimumWidth = 45, FillWeight = Indice == 1 ? 180F : 100F,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                if (Indice >= 3) Columna.DefaultCellStyle.Format = "N2";
                Tabla.Columns.Add(Columna);
            }
        }
    }
}
