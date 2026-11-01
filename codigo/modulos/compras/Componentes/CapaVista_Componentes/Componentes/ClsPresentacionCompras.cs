using System.Windows.Forms;
using System.Drawing;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Centraliza los tamaños de formularios, la presentación monetaria y la configuración visual compartida.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public static class ClsPresentacionCompras
    {
        public static Size TamanoFormulario => new Size(1300, 700);
        public static int AlturaContenido => 723;

        public static string ComprasFuncFormatearMonto(decimal Monto, ComprasEnumTipoMoneda Moneda = ComprasEnumTipoMoneda.Quetzales)
        {
            string Simbolo = Moneda == ComprasEnumTipoMoneda.Dolares ? "$" :
                Moneda == ComprasEnumTipoMoneda.Euros ? "€" : "Q";
            return Simbolo + " " + Monto.ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static void ComprasMetFijarTamanoFormulario(Form Formulario)
        {
            Formulario.AutoSize = false;
            Formulario.FormBorderStyle = FormBorderStyle.FixedSingle;
            Formulario.MaximizeBox = false;
            Formulario.MinimizeBox = true;
            Formulario.StartPosition = FormStartPosition.CenterScreen;
            var Recursos = new System.Resources.ResourceManager("CapaVista_Componentes.Componentes.RecursosCompras", typeof(ClsPresentacionCompras).Assembly);
            Formulario.Icon = (Icon)Recursos.GetObject("IconoCompras");
            Formulario.ClientSize = TamanoFormulario;
            Formulario.MinimumSize = Formulario.Size;
            Formulario.MaximumSize = Formulario.Size;
            Formulario.Load += (Remitente, Evento) =>
            {
                Rectangle Area = Screen.FromControl(Formulario.Owner ?? Formulario).WorkingArea;
                ComprasMetAjustarAreaFormulario(Formulario, Area);
                Formulario.Location = new Point(Area.Left + (Area.Width - Formulario.Width) / 2,
                    Area.Top + (Area.Height - Formulario.Height) / 2);
            };
            ComprasMetAplicarScrollVertical(Formulario);
        }

        private static void ComprasMetAplicarScrollVertical(Control Contenedor)
        {
            DateTimePicker Fecha = Contenedor as DateTimePicker;
            if (Fecha != null)
            {
                Fecha.Format = DateTimePickerFormat.Custom;
                Fecha.CustomFormat = "dd/MM/yyyy";
            }
            DataGridView Tabla = Contenedor as DataGridView;
            if (Tabla != null) Tabla.ScrollBars = ScrollBars.Vertical;
            foreach (Control Hijo in Contenedor.Controls) ComprasMetAplicarScrollVertical(Hijo);
        }

        public static void ComprasMetAjustarAreaFormulario(Form Formulario, Rectangle Area)
        {
            Size Tamano = new Size(System.Math.Min(Formulario.Width, Area.Width), System.Math.Min(Formulario.Height, Area.Height));
            Formulario.MinimumSize = Size.Empty;
            Formulario.MaximumSize = Size.Empty;
            Formulario.Size = Tamano;
            Formulario.MinimumSize = Tamano;
            Formulario.MaximumSize = Tamano;
        }

        public static void ComprasProcAgregarColumna(ComprasUsrDataGridView Tabla, string Campo,
            string Encabezado, int Peso, bool EsMonto)
        {
            Tabla.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn Columna = new DataGridViewTextBoxColumn
            {
                Name = "ComprasCol" + Campo,
                DataPropertyName = Campo,
                HeaderText = Encabezado,
                FillWeight = Peso
            };

            if (EsMonto)
            {
                Columna.DefaultCellStyle.Format = "'Q '#,##0.00";
                Columna.DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;
                Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            Tabla.Columns.Add(Columna);
        }

        public static void ComprasProcMostrarMonto(ComprasUsrTextBox Entrada, decimal Monto)
        {
            Entrada.Text = ComprasFuncFormatearMonto(Monto, Entrada.TipoMoneda);
            Entrada.Entrada.SelectAll();
            Entrada.Entrada.SelectionAlignment = HorizontalAlignment.Right;
        }

    }
}
