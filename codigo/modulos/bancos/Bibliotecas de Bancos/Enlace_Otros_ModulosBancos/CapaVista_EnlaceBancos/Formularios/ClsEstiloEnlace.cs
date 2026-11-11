using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using System.Globalization;

namespace CapaVista_EnlaceB.Formularios
{
    internal static class ClsEstiloEnlace
    {
        internal static readonly Color Verde = Color.FromArgb(28, 74, 48);
        internal static readonly Color Fondo = Color.FromArgb(245, 248, 243);
        internal static readonly Color Oro = Color.FromArgb(210, 169, 59);
        internal static readonly Color Acento = Color.FromArgb(46, 138, 85);
        internal static readonly Color Botones = Color.FromArgb(212, 231, 211);
        internal static Button BancarioMetBoton(string Texto, EventHandler Accion)
        {
            var BancarioBtnBoton = new ClsBotonEstandarizado(Texto) { Size = new Size(90, 90),
                Margin = new Padding(4), AccessibleName = Texto, Cursor = Cursors.Hand };
            BancarioBtnBoton.Click += Accion; return BancarioBtnBoton;
        }
        internal static void BancarioMetTabla(DataGridView BancarioDgvTabla)
        {
            BancarioDgvTabla.ReadOnly = true; BancarioDgvTabla.AllowUserToAddRows = false; BancarioDgvTabla.AllowUserToDeleteRows = false;
            BancarioDgvTabla.AutoGenerateColumns = true; BancarioDgvTabla.BackgroundColor = Color.White; BancarioDgvTabla.RowHeadersVisible = false;
            BancarioDgvTabla.BorderStyle = BorderStyle.FixedSingle; BancarioDgvTabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            BancarioDgvTabla.MultiSelect = false; BancarioDgvTabla.EnableHeadersVisualStyles = false;
            BancarioDgvTabla.ColumnHeadersDefaultCellStyle.BackColor = Verde; BancarioDgvTabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            BancarioDgvTabla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            BancarioDgvTabla.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            BancarioDgvTabla.DefaultCellStyle.SelectionBackColor = Botones;
            BancarioDgvTabla.DefaultCellStyle.SelectionForeColor = Verde; BancarioDgvTabla.AlternatingRowsDefaultCellStyle.BackColor = Fondo;
            BancarioDgvTabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            BancarioDgvTabla.ColumnHeadersHeight = 36; BancarioDgvTabla.RowTemplate.Height = 28;
            BancarioDgvTabla.DataBindingComplete += (Remitente, Argumentos) => BancarioMetFormatearTabla(BancarioDgvTabla);
            BancarioDgvTabla.CellFormatting += (Remitente, Argumentos) => {
                if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 || !(Argumentos.Value is decimal)) return;
                string Columna = BancarioDgvTabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
                if (!System.Text.RegularExpressions.Regex.IsMatch(Columna, "Importe|Total|Debe|Haber|Saldo|Neto|Costo|Monto", System.Text.RegularExpressions.RegexOptions.IgnoreCase)) return;
                string Moneda = BancarioDgvTabla.FindForm() is FrmEnlaceContabilidad ? "GTQ" : "";
                if (BancarioDgvTabla.Columns.Contains("Moneda")) Moneda = Convert.ToString(BancarioDgvTabla.Rows[Argumentos.RowIndex].Cells["Moneda"].Value);
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
                if (Columna.ValueType == typeof(DateTime)) Columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                if (Columna.ValueType == typeof(decimal))
                {
                    Columna.DefaultCellStyle.Format = "N2";
                    Columna.DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("es-GT");
                    Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
        internal static Control BancarioMetPaginar(DataGridView BancarioDgvTabla)
        { return new ClsPaginadorEnlace(BancarioDgvTabla); }
        internal static DataTable BancarioMetDatosCompletos(DataGridView BancarioDgvTabla)
        {
            var Paginador = BancarioDgvTabla.Tag as ClsPaginadorEnlace;
            return Paginador == null ? BancarioDgvTabla.DataSource as DataTable : Paginador.Datos;
        }
        internal static void BancarioMetVentana(Form BancarioFrmVentana)
        {
            BancarioFrmVentana.MaximizeBox = false; BancarioFrmVentana.MinimizeBox = true;
            BancarioFrmVentana.FormBorderStyle = FormBorderStyle.FixedDialog;
            BancarioFrmVentana.SizeGripStyle = SizeGripStyle.Hide;
            BancarioFrmVentana.AutoSize = false;
            BancarioFrmVentana.WindowState = FormWindowState.Normal;
            BancarioFrmVentana.StartPosition = FormStartPosition.CenterParent;
            if (BancarioFrmVentana.Icon == null && BancarioFrmVentana.Owner != null) BancarioFrmVentana.Icon = BancarioFrmVentana.Owner.Icon;
            // Reserva espacio para barra de tareas y bordes en 1366 x 768.
            BancarioFrmVentana.MaximumSize = new Size(1554, 1061);
            BancarioFrmVentana.Shown += (Remitente, Argumentos) => {
                Rectangle Area = Screen.FromControl(BancarioFrmVentana).WorkingArea;
                if (BancarioFrmVentana.Height > Area.Height || BancarioFrmVentana.Width > Area.Width)
                {
                    BancarioFrmVentana.AutoScroll = true;
                    BancarioFrmVentana.MinimumSize = Size.Empty;
                    BancarioFrmVentana.Size = new Size(Math.Min(BancarioFrmVentana.Width, Area.Width), Math.Min(BancarioFrmVentana.Height, Area.Height));
                    BancarioFrmVentana.Location = new Point(Area.Left + (Area.Width - BancarioFrmVentana.Width) / 2, Area.Top + (Area.Height - BancarioFrmVentana.Height) / 2);
                }
            };
            foreach (var Campo in BancarioFrmVentana.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                var BancarioUsrControl = Campo.GetValue(BancarioFrmVentana) as Control;
                if (BancarioUsrControl != null) BancarioUsrControl.Name = Campo.Name.TrimStart('_');
            }
            BancarioMetTipografia(BancarioFrmVentana);
        }
        private static void BancarioMetTipografia(Control Control)
        {
            foreach (Control Hijo in Control.Controls)
            {
                if (Hijo is Label)
                    Hijo.Font = Hijo.Font.Name.StartsWith("Lucida", StringComparison.OrdinalIgnoreCase)
                        ? new Font("Lucida Bright", Hijo.Font.Size, FontStyle.Bold | FontStyle.Italic)
                        : new Font("Tahoma", Hijo.Font.Size, Hijo.Font.Style);
                else if (Hijo is ComboBox || Hijo is DateTimePicker || Hijo is TextBox)
                    Hijo.Font = new Font("Segoe UI", 10);
                var BancarioDtpFecha = Hijo as DateTimePicker;
                if (BancarioDtpFecha != null) { BancarioDtpFecha.Format = DateTimePickerFormat.Custom; BancarioDtpFecha.CustomFormat = "dd/MM/yyyy"; }
                BancarioMetTipografia(Hijo);
            }
        }
        internal static Control BancarioMetEtiquetaObligatoria(string Texto)
        {
            var BancarioFlpGrupo = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, 6, 0, 0) };
            BancarioFlpGrupo.Controls.Add(new Label { Text = Texto, AutoSize = true, Font = new Font("Tahoma", 9), Margin = Padding.Empty });
            BancarioFlpGrupo.Controls.Add(new Label { Text = "*", ForeColor = Color.Red, AutoSize = true, Margin = Padding.Empty });
            return BancarioFlpGrupo;
        }
        internal static Control BancarioMetCampo(Control Control)
        {
            var BancarioPnlGrupo = new Panel { Width = Control.Width + 4, Height = Control.Height + 34, Margin = Control.Margin };
            var BancarioPnlBorde = new Panel { Location = Point.Empty, Size = new Size(Control.Width + 4, Control.Height + 4), Padding = new Padding(2) };
            var BancarioLblError = new Label { Location = new Point(0, Control.Height + 5), Width = BancarioPnlGrupo.Width, Height = 28,
                ForeColor = Color.Red, Font = new Font("Tahoma", 8), Visible = false };
            Control.Dock = DockStyle.Fill; Control.Margin = Padding.Empty;
            BancarioPnlBorde.Controls.Add(Control); BancarioPnlGrupo.Controls.Add(BancarioPnlBorde); BancarioPnlGrupo.Controls.Add(BancarioLblError);
            Control.Tag = Tuple.Create(BancarioPnlBorde, BancarioLblError);
            return BancarioPnlGrupo;
        }
        internal static void BancarioMetValidacion(Control Control, string Mensaje)
        {
            var Campo = Control.Tag as Tuple<Panel, Label>;
            if (Campo == null) return;
            Campo.Item1.BackColor = string.IsNullOrEmpty(Mensaje) ? Fondo : Color.Red;
            Campo.Item2.Text = Mensaje; Campo.Item2.Visible = !string.IsNullOrEmpty(Mensaje);
        }
        internal static void BancarioMetExito(IWin32Window Owner, string Mensaje)
        {
            using (var BancarioFrmDialogo = new Form { Text = "Operación completada", ClientSize = new Size(430, 155),
                BackColor = Fondo, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false,
                MinimizeBox = true, StartPosition = FormStartPosition.CenterParent, ShowInTaskbar = false })
            {
                var BancarioPnlMarca = new Panel { Location = new Point(12, 16), Size = new Size(55, 65), AccessibleName = "Éxito" };
                BancarioPnlMarca.Paint += (Remitente, Argumentos) => {
                    Argumentos.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var Trazo = new Pen(Acento, 5)) Argumentos.Graphics.DrawLines(Trazo,
                        new[] { new Point(8, 30), new Point(22, 44), new Point(46, 16) });
                };
                var BancarioLblTexto = new Label { Text = Mensaje, Font = new Font("Tahoma", 10), Location = new Point(78, 18), Size = new Size(335, 75) };
                var BancarioBtnAceptar = new Button { Text = "Aceptar", DialogResult = DialogResult.OK, BackColor = Botones,
                    Location = new Point(310, 110), Size = new Size(100, 30) };
                BancarioFrmDialogo.Controls.AddRange(new Control[] { BancarioPnlMarca, BancarioLblTexto, BancarioBtnAceptar }); BancarioFrmDialogo.AcceptButton = BancarioBtnAceptar;
                BancarioFrmDialogo.ShowDialog(Owner);
            }
        }
        internal static void BancarioMetError(IWin32Window Owner, Exception Ex)
        {
            string Mensaje = "No se completó la operación.\n\n" + Ex.Message;
            MessageBox.Show(Owner, Mensaje, "Error — Enlace con otros módulos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Mantiene todos los registros para exportar y ordenar; muestra 25 por página.
    internal sealed class ClsPaginadorEnlace : TableLayoutPanel
    {
        private readonly DataGridView _BancarioDgvTabla;
        private readonly Button _BancarioBtnAnterior = new Button { Text = "Anterior", Width = 85 };
        private readonly Button _BancarioBtnSiguiente = new Button { Text = "Siguiente", Width = 85 };
        private readonly Label _BancarioLblResumen = new Label { AutoSize = true, Margin = new Padding(8) };
        private int _Pagina;
        private bool _Actualizando;
        private string _Orden = "";
        private bool _Ascendente = true;
        internal DataTable Datos { get; private set; }
        internal ClsPaginadorEnlace(DataGridView BancarioDgvTabla)
        {
            _BancarioDgvTabla = BancarioDgvTabla; BancarioDgvTabla.Tag = this; Dock = DockStyle.Fill; RowCount = 2; ColumnCount = 1;
            RowStyles.Add(new RowStyle(SizeType.Percent, 100)); RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            Controls.Add(BancarioDgvTabla, 0, 0); BancarioDgvTabla.Dock = DockStyle.Fill;
            var BancarioFlpPie = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, BackColor = ClsEstiloEnlace.Fondo };
            _BancarioBtnAnterior.BackColor = _BancarioBtnSiguiente.BackColor = ClsEstiloEnlace.Botones;
            BancarioFlpPie.Controls.AddRange(new Control[] { _BancarioBtnAnterior, _BancarioBtnSiguiente, _BancarioLblResumen }); Controls.Add(BancarioFlpPie, 0, 1);
            _BancarioBtnAnterior.Click += (Remitente, Argumentos) => { if (_Pagina > 0) { _Pagina--; BancarioMetMostrarPagina(); } };
            _BancarioBtnSiguiente.Click += (Remitente, Argumentos) => { if (Datos != null && (_Pagina + 1) * 25 < Datos.Rows.Count) { _Pagina++; BancarioMetMostrarPagina(); } };
            BancarioDgvTabla.DataSourceChanged += (Remitente, Argumentos) => {
                if (_Actualizando) return;
                Datos = BancarioDgvTabla.DataSource as DataTable; _Pagina = 0; _Orden = "";
                BancarioMetMostrarPagina();
            };
            BancarioDgvTabla.ColumnHeaderMouseClick += (Remitente, Argumentos) => {
                if (Datos == null || Argumentos.ColumnIndex < 0) return;
                string Columna = BancarioDgvTabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
                _Ascendente = _Orden == Columna ? !_Ascendente : true; _Orden = Columna; _Pagina = 0;
                BancarioMetMostrarPagina();
            };
            Disposed += (Remitente, Argumentos) => { if (BancarioDgvTabla.Tag == this) BancarioDgvTabla.Tag = null; };
            BancarioMetMostrarPagina();
        }
        private void BancarioMetMostrarPagina()
        {
            _Actualizando = true;
            try
            {
                int Total = Datos == null ? 0 : Datos.Rows.Count;
                if (Datos != null)
                {
                    var Vista = new DataView(Datos);
                    if (!string.IsNullOrEmpty(_Orden)) Vista.Sort = "[" + _Orden.Replace("\\", "\\\\").Replace("]", "\\]") + "] " + (_Ascendente ? "ASC" : "DESC");
                    var Pagina = Datos.Clone();
                    for (int Indice = _Pagina * 25; Indice < Math.Min((_Pagina + 1) * 25, Vista.Count); Indice++) Pagina.ImportRow(Vista[Indice].Row);
                    _BancarioDgvTabla.DataSource = Pagina;
                    foreach (DataGridViewColumn Columna in _BancarioDgvTabla.Columns)
                        Columna.HeaderCell.SortGlyphDirection = Columna.DataPropertyName == _Orden
                            ? (_Ascendente ? SortOrder.Ascending : SortOrder.Descending) : SortOrder.None;
                }
                _BancarioBtnAnterior.Enabled = _Pagina > 0; _BancarioBtnSiguiente.Enabled = (_Pagina + 1) * 25 < Total;
                _BancarioLblResumen.Text = Total + " registros · Página " + (Total == 0 ? 0 : _Pagina + 1) + " de " + ((Total + 24) / 25);
            }
            finally { _Actualizando = false; }
        }
    }

    internal sealed class ClsBotonEstandarizado : Button
    {
        private static readonly Dictionary<string, Bitmap> _Iconos = new Dictionary<string, Bitmap>();
        private readonly Bitmap _Icono;
        private bool _Encima, _Presionado;

        internal ClsBotonEstandarizado(string Texto)
        {
            Text = Texto; Font = new Font("Segoe UI", 9, FontStyle.Regular);
            BackColor = ClsEstiloEnlace.Verde; ForeColor = Color.FromArgb(245, 248, 243);
            FlatStyle = FlatStyle.Flat; FlatAppearance.BorderSize = 0;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            _Icono = BancarioMetObtenerIcono(Texto);
        }
        private static Bitmap BancarioMetObtenerIcono(string Texto)
        {
            Bitmap ImagenAlmacenada;
            if (_Iconos.TryGetValue(Texto, out ImagenAlmacenada)) return ImagenAlmacenada;
            Image Imagen; Rectangle Recorte; bool Completa = false;
            string Externo = null;
            switch (Texto)
            {
                case "Logística": Externo = "Logistica.png"; break;
                case "Compras": Externo = "Compras.png"; break;
                case "Recursos Humanos": Externo = "Personal.png"; break;
                case "Ver historial": Externo = "Historial.png"; break;
                case "Cargar catálogos": Externo = "Actualizar.png"; break;
                case "Seleccionar": Externo = "Seleccionar.png"; break;
            }
            if (Externo != null)
            {
                using (var Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enlace.Iconos." + Externo))
                {
                    if (Stream == null) throw new InvalidOperationException("No se encontró la imagen estandarizada " + Externo);
                    using (var Original = Image.FromStream(Stream)) Imagen = new Bitmap(Original);
                }
                Completa = true;
            }
            else
            {
                switch (Texto)
                {
                    case "Consultar": Imagen = Properties.Resources.btn_consultarV; break;
                    case "Limpiar": Imagen = Properties.Resources.btn_limpiarV; break;
                    case "Exportar CSV": case "Ventas": Imagen = Properties.Resources.btn_reporteB1; break;
                    case "Contabilidad": Imagen = Properties.Resources.btn_generarpartidaV; break;
                    case "Cerrar": case "Volver": Imagen = Properties.Resources.btn_salirV; break;
                    default: throw new ArgumentException("Falta asignar una imagen a la acción " + Texto);
                }
            }
            if (Texto == "Logística") Recorte = new Rectangle(13, 12, 29, 20);
            else if (Texto == "Compras") Recorte = new Rectangle(11, 6, 35, 30);
            else if (Texto == "Recursos Humanos") Recorte = new Rectangle(28, 24, 76, 80);
            else Recorte = new Rectangle((int)(Imagen.Width * .23), (int)(Imagen.Height * .10), (int)(Imagen.Width * .54), (int)(Imagen.Height * .48));
            using (var Original = new Bitmap(Imagen))
            using (var Region = Original.Clone(Recorte, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                for (int PosicionY = 0; PosicionY < Region.Height; PosicionY++)
                    for (int PosicionX = 0; PosicionX < Region.Width; PosicionX++)
                    {
                        Color Elemento = Region.GetPixel(PosicionX, PosicionY);
                        int Minimo = Math.Min(Elemento.R, Math.Min(Elemento.G, Elemento.B));
                        int Alpha = Math.Min(Elemento.A, Math.Max(0, Math.Min(255, (Minimo - 150) * 3)));
                        Region.SetPixel(PosicionX, PosicionY, Color.FromArgb(Alpha, 245, 248, 243));
                    }
                ImagenAlmacenada = new Bitmap(52, 52);
                using (var Graficos = Graphics.FromImage(ImagenAlmacenada))
                {
                    Graficos.Clear(Color.Transparent); Graficos.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    float Escala = Math.Min(48f / Region.Width, 48f / Region.Height);
                    float Ancho = Region.Width * Escala, ImporteHaber = Region.Height * Escala;
                    Graficos.DrawImage(Region, (52 - Ancho) / 2, (52 - ImporteHaber) / 2, Ancho, ImporteHaber);
                }
            }
            if (Completa) Imagen.Dispose();
            _Iconos.Add(Texto, ImagenAlmacenada); return ImagenAlmacenada;
        }
        protected override void OnPaint(PaintEventArgs Argumentos)
        {
            var Graficos = Argumentos.Graphics; Graficos.Clear(Parent == null || Parent.BackColor.A < 255 ? ClsEstiloEnlace.Fondo : Parent.BackColor);
            Rectangle Cuadro = new Rectangle(1, 1, Width - 3, Height - 3);
            using (var Borde = new Pen(Color.FromArgb(175, 188, 175))) Graficos.DrawRectangle(Borde, Cuadro);
            Cuadro.Inflate(-2, -2);
            using (var Borde = new Pen(Color.FromArgb(245, 248, 243), 2)) Graficos.DrawRectangle(Borde, Cuadro);
            Cuadro.Inflate(-2, -2);
            Color Fondo = !Enabled ? Color.LightGray : _Presionado ? ClsEstiloEnlace.Acento : _Encima ? ClsEstiloEnlace.Acento : ClsEstiloEnlace.Verde;
            using (var Pincel = new SolidBrush(Fondo)) Graficos.FillRectangle(Pincel, Cuadro);
            using (var Borde = new Pen(ClsEstiloEnlace.Oro, 2)) Graficos.DrawRectangle(Borde, Cuadro);
            int Lado = Math.Min(44, Height - 47);
            double Escala = Math.Min((double)Lado / _Icono.Width, (double)Lado / _Icono.Height);
            int Ancho = Math.Max(1, (int)(_Icono.Width * Escala)), Alto = Math.Max(1, (int)(_Icono.Height * Escala));
            var Destino = new Rectangle((Width - Ancho) / 2, 9 + (_Presionado ? 1 : 0), Ancho, Alto);
            if (Enabled) Graficos.DrawImage(_Icono, Destino);
            else using (var Atributos = new System.Drawing.Imaging.ImageAttributes())
            {
                Atributos.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(new[] {
                    new[] { .3f, .3f, .3f, 0f, 0f }, new[] { .59f, .59f, .59f, 0f, 0f },
                    new[] { .11f, .11f, .11f, 0f, 0f }, new[] { 0f, 0f, 0f, .45f, 0f }, new[] { 0f, 0f, 0f, 0f, 1f }
                }));
                Graficos.DrawImage(_Icono, Destino, 0, 0, _Icono.Width, _Icono.Height, GraphicsUnit.Pixel, Atributos);
            }
            string Caption = Text == "Cargar catálogos" ? "Cargar\ncatálogos" : Text == "Exportar CSV" ? "Exportar\nCSV" : Text == "Ver historial" ? "Ver\nhistorial" : Text == "Recursos Humanos" ? "Recursos\nHumanos" : Text;
            TextRenderer.DrawText(Graficos, Caption, Font, new Rectangle(7, Height - 36, Width - 14, 30), Enabled ? ForeColor : ClsEstiloEnlace.Verde,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            if (Focused && ShowFocusCues) ControlPaint.DrawFocusRectangle(Graficos, new Rectangle(8, 8, Width - 16, Height - 16), ForeColor, Fondo);
        }
        protected override void OnMouseEnter(EventArgs Argumentos) { _Encima = true; Invalidate(); base.OnMouseEnter(Argumentos); }
        protected override void OnMouseLeave(EventArgs Argumentos) { _Encima = false; _Presionado = false; Invalidate(); base.OnMouseLeave(Argumentos); }
        protected override void OnMouseDown(MouseEventArgs Argumentos) { _Presionado = Argumentos.Button == MouseButtons.Left; Invalidate(); base.OnMouseDown(Argumentos); }
        protected override void OnMouseUp(MouseEventArgs Argumentos) { _Presionado = false; Invalidate(); base.OnMouseUp(Argumentos); }
        protected override void OnEnabledChanged(EventArgs Argumentos) { Invalidate(); base.OnEnabledChanged(Argumentos); }
        protected override void OnGotFocus(EventArgs Argumentos) { Invalidate(); base.OnGotFocus(Argumentos); }
        protected override void OnLostFocus(EventArgs Argumentos) { Invalidate(); base.OnLostFocus(Argumentos); }
    }
}
