// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System;

namespace VentasPresentacion
{
    internal static class ClsHerramientasVentas
    {
        // FormatoVentas.
        internal static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-GT");
        internal static void VentasMetCargarOpciones(ComboBox Control, DataTable Datos, int IdSeleccionado = 0)
        {
            Control.DataSource = null;
            Control.Items.Clear();
            if (!Datos.Columns.Contains("Id"))
                return;
            Control.DisplayMember = "Nombre";
            Control.ValueMember = "Id";
            Control.DataSource = Datos;
            if (IdSeleccionado > 0)
                Control.SelectedValue = IdSeleccionado;
            else
                Control.SelectedIndex = -1;
        }

        internal static int VentasFuncObtenerId(ComboBox Control)
        {
            return Control.SelectedValue == null || Control.SelectedValue is DataRowView ? 0 : Convert.ToInt32(Control.SelectedValue);
        }

        internal static DataRow VentasFuncObtenerFila(ComboBox Control)
        {
            var FilaSeleccionada = Control.SelectedItem as DataRowView;
            return FilaSeleccionada == null ? null : FilaSeleccionada.Row;
        }

        internal static void VentasMetFormatearFechas(DataGridView Grid)
        {
            var Estilo = Grid.DefaultCellStyle;
            Estilo.FormatProvider = Cultura;
            Grid.DefaultCellStyle = Estilo;
            Grid.CellFormatting += (Origen, Evento) =>
            {
                if (Evento.Value is DateTime)
                {
                    Evento.Value = ((DateTime)Evento.Value).ToString("dd/MM/yyyy");
                    Evento.FormattingApplied = true;
                }
            };
        }

        internal static string VentasFuncDinero(decimal Importe, string Moneda)
        {
            return Moneda + " " + Importe.ToString("N2", Cultura);
        }

        // MensajesVentas.
        internal static void VentasMetEjecutar(Form Ventana, Action Accion)
        {
            Ventana.UseWaitCursor = true;
            try
            {
                Accion();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Ventana, Error.Message, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Ventana.UseWaitCursor = false;
            }
        }

        internal static void VentasMetInformar(Form Ventana, string Mensaje)
        {
            MessageBox.Show(Ventana, Mensaje, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ImpresionVentas.
        internal static void VentasMetImprimir(Form Ventana, string Titulo, string Encabezado, DataTable Datos)
        {
            if (Datos == null || Datos.Rows.Count == 0)
                throw new InvalidOperationException("Consulte o guarde datos antes de imprimir.");
            var Lineas = new System.Collections.Generic.List<string>
            {
                Titulo,
                Encabezado,
                ""
            };
            foreach (DataRow Fila in Datos.Rows)
            {
                foreach (DataColumn Columna in Datos.Columns)
                {
                    object Valor = Fila[Columna];
                    string Texto = Valor is DateTime ? ((DateTime)Valor).ToString("dd/MM/yyyy") : Valor is decimal ? ((decimal)Valor).ToString("N2", ClsHerramientasVentas.Cultura) : Convert.ToString(Valor);
                    Lineas.Add(Columna.ColumnName + ": " + Texto);
                }

                Lineas.Add("");
            }

            int Posicion = 0;
            using (var Documento = new PrintDocument())
            using (var Fuente = new Font("Tahoma", 10))
            using (var Vista = new PrintPreviewDialog
            {
                Document = Documento,
                Width = 1000,
                Height = 750
            }

            )
            {
                Documento.DocumentName = Titulo;
                Documento.BeginPrint += (Origen, Evento) => Posicion = 0;
                Documento.PrintPage += (Origen, Evento) =>
                {
                    float PosicionVertical = Evento.MarginBounds.Top;
                    while (Posicion < Lineas.Count)
                    {
                        string Texto = Lineas[Posicion];
                        SizeF Medida = Evento.Graphics.MeasureString(Texto.Length == 0 ? " " : Texto, Fuente, Evento.MarginBounds.Width);
                        if (PosicionVertical + Medida.Height > Evento.MarginBounds.Bottom)
                            break;
                        Evento.Graphics.DrawString(Texto, Fuente, Brushes.Black, new RectangleF(Evento.MarginBounds.Left, PosicionVertical, Evento.MarginBounds.Width, Medida.Height));
                        PosicionVertical += Medida.Height + 3;
                        Posicion++;
                    }

                    Evento.HasMorePages = Posicion < Lineas.Count;
                };
                Vista.ShowDialog(Ventana);
            }
        }

        // SeleccionVentas.
        internal static DataRow VentasFuncSeleccionar(Form Padre, string Titulo, DataTable Datos)
        {
            using (var Ventana = new Form
            {
                Text = Titulo,
                Width = 1000,
                Height = 480,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                BackColor = Color.FromArgb(248, 245, 249)
            }

            )
            using (var VentasTxtBuscar = new TextBox
            {
                Name = "VentasTxtBuscar",
                Dock = DockStyle.Top,
                Font = new Font("Tahoma", 11)
            }

            )
            using (var VentasDgvSeleccion = new DataGridView
            {
                Name = "VentasDgvSeleccion",
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                EnableHeadersVisualStyles = false
            }

            )
            using (var VentasFlpPie = new FlowLayoutPanel
            {
                Name = "VentasFlpPie",
                Dock = DockStyle.Bottom,
                Height = 44,
                FlowDirection = FlowDirection.RightToLeft
            }

            )
            using (var VentasBtnSeleccionar = new Button
            {
                Name = "VentasBtnSeleccionar",
                Text = "Seleccionar",
                Width = 110,
                Height = 32,
                BackColor = Color.FromArgb(123, 63, 147),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            }

            )
            using (var VentasBtnCancelar = new Button
            {
                Name = "VentasBtnCancelar",
                Text = "Cancelar",
                Width = 100,
                Height = 32,
                DialogResult = DialogResult.Cancel
            }

            )
            {
                VentasDgvSeleccion.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(59, 29, 74);
                VentasDgvSeleccion.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                VentasDgvSeleccion.DefaultCellStyle.SelectionBackColor = Color.FromArgb(228, 212, 234);
                VentasDgvSeleccion.DefaultCellStyle.SelectionForeColor = Color.FromArgb(59, 29, 74);
                VentasDgvSeleccion.DataSource = Datos;
                foreach (DataGridViewColumn Columna in VentasDgvSeleccion.Columns)
                    if (Columna.Name == "Id" || Columna.Name.StartsWith("id"))
                        Columna.Visible = false;
                DataRow Elegida = null;
                Action Elegir = () =>
                {
                    var FilaSeleccionada = VentasDgvSeleccion.CurrentRow == null ? null : VentasDgvSeleccion.CurrentRow.DataBoundItem as DataRowView;
                    if (FilaSeleccionada != null)
                    {
                        Elegida = FilaSeleccionada.Row;
                        Ventana.DialogResult = DialogResult.OK;
                    }
                };
                VentasBtnSeleccionar.Click += (Origen, Evento) => Elegir();
                VentasDgvSeleccion.CellDoubleClick += (Origen, Evento) =>
                {
                    if (Evento.RowIndex >= 0)
                        Elegir();
                };
                VentasTxtBuscar.TextChanged += (Origen, Evento) =>
                {
                    var Filtro = Datos.Clone();
                    string Texto = VentasTxtBuscar.Text.Trim();
                    foreach (DataRow Fila in Datos.Rows)
                        if (Texto.Length == 0 || Array.Exists(Fila.ItemArray, Valor => Convert.ToString(Valor).IndexOf(Texto, StringComparison.OrdinalIgnoreCase) >= 0))
                            Filtro.ImportRow(Fila);
                    VentasDgvSeleccion.DataSource = Filtro;
                };
                VentasFlpPie.Controls.Add(VentasBtnSeleccionar);
                VentasFlpPie.Controls.Add(VentasBtnCancelar);
                Ventana.Controls.Add(VentasDgvSeleccion);
                Ventana.Controls.Add(VentasTxtBuscar);
                Ventana.Controls.Add(VentasFlpPie);
                Ventana.AcceptButton = VentasBtnSeleccionar;
                Ventana.CancelButton = VentasBtnCancelar;
                Ventana.ShowDialog(Padre);
                return Elegida;
            }
        }

        internal static string VentasFuncMotivo(Form Padre, string Titulo)
        {
            using (var Ventana = new Form
            {
                Text = Titulo,
                Width = 520,
                Height = 205,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                BackColor = Color.FromArgb(248, 245, 249)
            }

            )
            using (var VentasLblMotivo = new Label
            {
                Name = "VentasLblMotivo",
                Text = "Motivo de anulacion",
                Left = 16,
                Top = 16,
                Width = 460
            }

            )
            using (var VentasTxtMotivo = new TextBox
            {
                Name = "VentasTxtMotivo",
                Left = 16,
                Top = 44,
                Width = 470,
                Height = 55,
                Multiline = true,
                MaxLength = 180
            }

            )
            using (var VentasBtnAceptarMotivo = new Button
            {
                Name = "VentasBtnAceptarMotivo",
                Text = "Anular",
                Left = 260,
                Top = 112,
                Width = 110,
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(123, 63, 147),
                ForeColor = Color.White
            }

            )
            using (var VentasBtnCancelar = new Button
            {
                Name = "VentasBtnCancelar",
                Text = "Cancelar",
                Left = 380,
                Top = 112,
                Width = 105,
                DialogResult = DialogResult.Cancel
            }

            )
            {
                Ventana.Controls.AddRange(new Control[] { VentasLblMotivo, VentasTxtMotivo, VentasBtnAceptarMotivo, VentasBtnCancelar });
                Ventana.AcceptButton = VentasBtnAceptarMotivo;
                Ventana.CancelButton = VentasBtnCancelar;
                return Ventana.ShowDialog(Padre) == DialogResult.OK ? VentasTxtMotivo.Text.Trim() : null;
            }
        }
    }
}
