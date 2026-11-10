/*
 * Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: composición visual común de los prototipos.
 */
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad.Componentes
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    internal static class ClsDisenoContabilidad
    {
        internal static Label ContabilidadFuncEtiqueta(Control Padre, string Nombre, string Texto, int X, int Y, int Ancho, int Alto = 24)
        {
            Label Etiqueta = new Label { Name = Nombre, Text = Texto, Location = new Point(X, Y), Size = new Size(Ancho, Alto),
                ForeColor = Color.FromArgb(35, 50, 61), BackColor = Color.Transparent, Font = new Font("Tahoma", 10F) };
            Padre.Controls.Add(Etiqueta);
            return Etiqueta;
        }

        internal static void ContabilidadMetTitulo(Form Ventana, string Titulo)
        {
            Ventana.AutoScaleMode = AutoScaleMode.None;
            Ventana.ClientSize = new Size(1180, 688);
            Label Etiqueta = ContabilidadFuncEtiqueta(Ventana, "ContabilidadLblTitulo", Titulo, 24, 22, 1132, 38);
            Etiqueta.Font = new Font("Lucida Fax", 20F, FontStyle.Bold | FontStyle.Italic);
        }

        internal static ClsPanelRedondeadoContabilidad ContabilidadFuncTarjeta(Control Padre, string Nombre, int Y, int Alto)
        {
            ClsPanelRedondeadoContabilidad Tarjeta = new ClsPanelRedondeadoContabilidad { Name = Nombre,
                Location = new Point(24, Y), Size = new Size(1132, Alto), BackColor = Color.Transparent,
                ColorRelleno = Color.White, ColorBorde = Color.FromArgb(212, 222, 228), Radio = 18 };
            Padre.Controls.Add(Tarjeta);
            return Tarjeta;
        }

        internal static T ContabilidadFuncCampo<T>(Control Padre, T Campo, string Nombre, string Titulo, int X, int Y, int Ancho) where T : Control
        {
            ContabilidadFuncEtiqueta(Padre, "ContabilidadLbl" + Nombre.Substring(15), Titulo, X, Y, Ancho);
            Campo.Name = Nombre;
            Campo.Location = new Point(X, Y + 26);
            Campo.Size = new Size(Ancho, 30);
            Campo.Font = new Font("Segoe UI", 10F);
            Padre.Controls.Add(Campo);
            return Campo;
        }

        internal static NumericUpDown ContabilidadFuncImporte()
        {
            return new NumericUpDown { DecimalPlaces = 2, Maximum = 99999999.99m, ThousandsSeparator = true, TextAlign = HorizontalAlignment.Right };
        }

        internal static DateTimePicker ContabilidadFuncFecha()
        {
            return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
        }

        internal static ComboBox ContabilidadFuncLista()
        {
            return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        }

        internal static ClsBotonContabilidad ContabilidadFuncBoton(Control Padre, string Nombre, string Texto, int X, int Y, int Ancho,
            EstiloBotonContabilidad Estilo = EstiloBotonContabilidad.Principal)
        {
            ClsBotonContabilidad Boton = new ClsBotonContabilidad { Name = Nombre, Text = Texto, Location = new Point(X, Y),
                Size = new Size(Ancho, 42), Estilo = Estilo };
            Padre.Controls.Add(Boton);
            return Boton;
        }

        internal static ClsTablaContabilidad ContabilidadFuncTabla(Control Padre, string Nombre, int Y, int Alto, string[] Campos, string[] Titulos)
        {
            ClsTablaContabilidad Tabla = new ClsTablaContabilidad { Name = Nombre, Location = new Point(20, Y), Size = new Size(1092, Alto),
                AutoGenerateColumns = false, ReadOnly = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            for (int Indice = 0; Indice < Campos.Length; Indice++)
            {
                DataGridViewTextBoxColumn Columna = new DataGridViewTextBoxColumn { Name = "ContabilidadCol" + Campos[Indice],
                    DataPropertyName = Campos[Indice], HeaderText = Titulos[Indice], MinimumWidth = 90 };
                if (Array.IndexOf(new[] { "Cantidad", "Debe", "Haber", "Diferencia", "Importe" }, Campos[Indice]) >= 0)
                {
                    Columna.DefaultCellStyle.Format = "N2";
                    Columna.DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("es-GT");
                    Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (Campos[Indice] == "Fecha") Columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                Tabla.Columns.Add(Columna);
            }
            Padre.Controls.Add(Tabla);
            return Tabla;
        }

        internal static string ContabilidadFuncMoneda(decimal Importe)
        {
            return "Q " + Importe.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
        }

        internal static void ContabilidadMetAviso(Form Ventana, string Error, string Accion)
        {
            if (string.IsNullOrEmpty(Error)) ClsDialogosContabilidad.ContabilidadMetExito(Ventana, Accion);
            else ClsDialogosContabilidad.ContabilidadMetError(Ventana, Error);
        }
    }
}
