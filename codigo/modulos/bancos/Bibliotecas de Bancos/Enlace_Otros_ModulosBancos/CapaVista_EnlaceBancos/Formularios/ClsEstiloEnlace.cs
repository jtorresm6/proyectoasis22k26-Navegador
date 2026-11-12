using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Globalization;

namespace CapaVista_EnlaceB.Formularios
{
    internal static class ClsEstiloEnlace
    {
        internal static readonly Color Verde = Color.FromArgb(28, 74, 48);
        internal static readonly Color Fondo = Color.FromArgb(245, 248, 243);
        internal static readonly Color Botones = Color.FromArgb(212, 231, 211);
        internal static void BancarioMetTabla(DataGridView BancarioDgvTabla)
        {
            BancarioDgvTabla.ReadOnly = true;
            BancarioDgvTabla.AllowUserToAddRows = false;
            BancarioDgvTabla.AllowUserToDeleteRows = false;
            BancarioDgvTabla.AutoGenerateColumns = true;
            BancarioDgvTabla.BackgroundColor = Color.White;
            BancarioDgvTabla.RowHeadersVisible = false;
            BancarioDgvTabla.BorderStyle = BorderStyle.FixedSingle;
            BancarioDgvTabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            BancarioDgvTabla.MultiSelect = false;
            BancarioDgvTabla.EnableHeadersVisualStyles = false;
            BancarioDgvTabla.ColumnHeadersDefaultCellStyle.BackColor = Verde;
            BancarioDgvTabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            BancarioDgvTabla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            BancarioDgvTabla.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            BancarioDgvTabla.DefaultCellStyle.SelectionBackColor = Botones;
            BancarioDgvTabla.DefaultCellStyle.SelectionForeColor = Verde;
            BancarioDgvTabla.AlternatingRowsDefaultCellStyle.BackColor = Fondo;
            BancarioDgvTabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            BancarioDgvTabla.ColumnHeadersHeight = 36;
            BancarioDgvTabla.RowTemplate.Height = 28;
            BancarioDgvTabla.DataBindingComplete += (Remitente, Argumentos) => BancarioMetFormatearTabla(BancarioDgvTabla);
            BancarioDgvTabla.CellFormatting += (Remitente, Argumentos) =>
            {
                if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 || !(Argumentos.Value is decimal))
                    return;
                string Columna = BancarioDgvTabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
                if (!System.Text.RegularExpressions.Regex.IsMatch(Columna, "Importe|Total|Debe|Haber|Saldo|Neto|Costo|Monto", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    return;
                string Moneda = BancarioDgvTabla.FindForm() is FrmEnlaceContabilidad ? "GTQ" : "";
                if (BancarioDgvTabla.Columns.Contains("Moneda"))
                    Moneda = Convert.ToString(BancarioDgvTabla.Rows[Argumentos.RowIndex].Cells["Moneda"].Value);
                string Simbolo = Moneda == "GTQ" ? "Q" : Moneda == "USD" ? "$" : Moneda;
                Argumentos.Value = (string.IsNullOrEmpty(Simbolo) ? "" : Simbolo + " ") + ((decimal)Argumentos.Value).ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
                Argumentos.FormattingApplied = true;
            };
        }

        private static void BancarioMetFormatearTabla(DataGridView BancarioDgvTabla)
        {
            foreach (DataGridViewColumn Columna in BancarioDgvTabla.Columns)
            {
                Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
                if (Columna.ValueType == typeof(DateTime))
                    Columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                if (Columna.ValueType == typeof(decimal))
                {
                    Columna.DefaultCellStyle.Format = "N2";
                    Columna.DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("es-GT");
                    Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        internal static void BancarioMetValidacion(Panel Borde, Label Etiqueta, string Mensaje)
        {
            Etiqueta.Text = Mensaje;
            Etiqueta.Visible = Borde.Visible = !string.IsNullOrEmpty(Mensaje);
        }

        internal static void BancarioMetExito(IWin32Window Ventana, string Mensaje)
        {
            using (var Dialogo = new FrmOperacionCompletada(Mensaje))
                Dialogo.ShowDialog(Ventana);
        }

        internal static void BancarioMetError(IWin32Window Owner, Exception Ex)
        {
            string Mensaje = "No se completó la operación.\n\n" + Ex.Message;
            MessageBox.Show(Owner, Mensaje, "Error — Enlace con otros módulos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
