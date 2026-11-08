using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using Microsoft.Reporting.WinForms;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta el código, título y descripción de los formularios de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    [DefaultProperty("Titulo")]
    public partial class ComprasUsrEncabezado : UserControl
    {
        private static readonly Random _Aleatorio = new Random();
        private static readonly ComponentResourceManager _Recursos =
            new ComponentResourceManager(typeof(ComprasUsrEncabezado));
        private bool _FondoSeleccionado;
        private bool _MostrarAyudas = true;
        private bool _MostrarReportes = true;
        private string _Titulo = "Titulo";
        private string _Codigo = "6000";
        private Image _Icono;
        private readonly Image _IconoPredeterminado;

        public ComprasUsrEncabezado()
        {
            InitializeComponent();
            _IconoPredeterminado = ComprasPbxIcono.BackgroundImage;
            ComprasProcActualizarTitulo();
            ComprasProcActualizarAcciones();
            DoubleBuffered = true;
            TabStop = false;
            ComprasProcSeleccionarFondo();
        }

        [Category("Compras")]
        [Description("Imagen del extremo izquierdo. Si no se selecciona una imagen valida, muestra el icono predeterminado.")]
        [Browsable(true)]
        [Localizable(true)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Icono
        {
            get => _Icono;
            set
            {
                try
                {
                    if (value != null && (value.Width <= 0 || value.Height <= 0))
                        value = null;
                    ComprasPbxIcono.BackgroundImage = value ?? _IconoPredeterminado;
                    _Icono = value;
                }
                catch (Exception)
                {
                    _Icono = null;
                    ComprasPbxIcono.BackgroundImage = _IconoPredeterminado;
                }
            }
        }

        [Category("Compras")]
        [Description("T?tulo principal del encabezado.")]
        [Browsable(true)]
        [Bindable(true)]
        [Localizable(true)]
        [DefaultValue("Titulo")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Titulo
        {
            get => _Titulo;
            set
            {
                _Titulo = value ?? string.Empty;
                ComprasProcActualizarTitulo();
            }
        }

        [Category("Compras")]
        [Description("Descripci?n del formulario o proceso.")]
        [Browsable(true)]
        [Bindable(true)]
        [Localizable(true)]
        [DefaultValue("Descripcion")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Descripcion
        {
            get => ComprasLblDescripcion.Texto;
            set => ComprasLblDescripcion.Texto = value ?? string.Empty;
        }

        [Category("Compras")]
        [Description("C?digo del formulario de Compras, en el rango 6000?6999.")]
        [Browsable(true)]
        [Bindable(true)]
        [Localizable(true)]
        [DefaultValue("6000")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Codigo
        {
            get => _Codigo;
            set
            {
                _Codigo = value ?? string.Empty;
                ComprasProcActualizarTitulo();
            }
        }

        [Category("Compras")]
        [DefaultValue(true)]
        [Description("Muestra el boton de Ayudas en el encabezado.")]
        public bool MostrarAyudas
        {
            get => _MostrarAyudas;
            set
            {
                _MostrarAyudas = value;
                ComprasProcActualizarAcciones();
            }
        }

        [Category("Compras")]
        [DefaultValue(true)]
        [Description("Muestra el boton de Reportes en el encabezado.")]
        public bool MostrarReportes
        {
            get => _MostrarReportes;
            set
            {
                _MostrarReportes = value;
                ComprasProcActualizarAcciones();
            }
        }

        private void ComprasProcActualizarAcciones()
        {
            ComprasTlpPrincipal.SuspendLayout();
            ComprasTlpAcciones.SuspendLayout();
            try
            {
                ComprasBtnAyudas.Visible = _MostrarAyudas;
                ComprasBtnReportes.Visible = _MostrarReportes;
                int Cantidad = (_MostrarAyudas ? 1 : 0) + (_MostrarReportes ? 1 : 0);
                ComprasTlpAcciones.ColumnStyles[0].SizeType = SizeType.Absolute;
                ComprasTlpAcciones.ColumnStyles[0].Width = _MostrarAyudas ? 86F : 0F;
                ComprasTlpAcciones.ColumnStyles[1].SizeType = SizeType.Absolute;
                ComprasTlpAcciones.ColumnStyles[1].Width = _MostrarReportes ? 86F : 0F;
                ComprasTlpAcciones.Width = Cantidad * 86;
                ComprasTlpAcciones.Visible = Cantidad > 0;
                ComprasTlpPrincipal.ColumnStyles[2].SizeType = SizeType.Absolute;
                ComprasTlpPrincipal.ColumnStyles[2].Width = Cantidad > 0 ? Cantidad * 86 + 28 : 0;
            }
            finally
            {
                ComprasTlpAcciones.ResumeLayout(true);
                ComprasTlpPrincipal.ResumeLayout(true);
            }
        }

        private string _RutaReporte = string.Empty;
        private string _PaginaInicialAyuda = "index.html";

        [Category("Compras")]
        [DefaultValue("")]
        [Description("Seleccione un RDLC. Guarda solo el nombre y lo busca en Reportes desde la carpeta del ensamblado del formulario y sus directorios padres; luego desde el ejecutable y en todas las carpetas de Bibliotecas de clase Compras.")]
        [Editor(typeof(ClsEditorReporteCompras), typeof(UITypeEditor))]
        public string RutaReporte
        {
            get => _RutaReporte;
            set => _RutaReporte = ClsArchivosEncabezadoCompras.NormalizarNombre(value, ".rdlc");
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RutaAyuda
        {
            get => "ComprasAyudas.chm";
            set { }
        }

        [Category("Compras")]
        [DefaultValue("index.html")]
        [Description("Seleccione un archivo .html. Guarda solo el nombre de la pagina dentro de ComprasAyudas.chm. Si queda vacio o la pagina no existe, abre index.html.")]
        [Editor(typeof(ClsEditorPaginaAyudaCompras), typeof(UITypeEditor))]
        public string PaginaInicialAyuda
        {
            get => _PaginaInicialAyuda;
            set
            {
                string Nombre = ClsArchivosEncabezadoCompras.NormalizarNombre(value, ".html");
                _PaginaInicialAyuda = Nombre.Length == 0 ? "index.html" : Nombre;
            }
        }

        [Category("Compras")]
        [Description("Permite agregar fuentes de datos y parametros al reporte antes de mostrarlo.")]
        public event EventHandler<ComprasReporteEventArgs> PrepararReporte;

        public void AbrirAyuda()
        {
            ClsAyudasCompras.Abrir(this, PaginaInicialAyuda);
        }

        public void AbrirReporte()
        {
            string Archivo = ClsReportesCompras.ObtenerRuta(this, RutaReporte);
            using (var Ventana = new Form())
            using (var Visor = new ReportViewer())
            {
                Ventana.Text = "Reportes - " + Titulo;
                var Propietario = FindForm();
                if (Propietario != null) Ventana.Icon = Propietario.Icon;
                Ventana.StartPosition = FormStartPosition.CenterParent;
                Ventana.Size = new Size(1000, 700);
                Ventana.MinimizeBox = false;
                Visor.Dock = DockStyle.Fill;
                Visor.ProcessingMode = ProcessingMode.Local;
                Visor.ZoomMode = ZoomMode.PageWidth;
                Visor.LocalReport.ReportPath = Archivo;
                PrepararReporte?.Invoke(this, new ComprasReporteEventArgs(Visor.LocalReport));
                Ventana.Controls.Add(Visor);
                Ventana.Load += (Remitente, Evento) => Visor.RefreshReport();
                Ventana.ShowDialog(FindForm());
            }
        }

        private void ComprasBtnAyudas_Click(object Remitente, EventArgs Evento)
        {
            ComprasProcAbrirArchivo(AbrirAyuda);
        }

        private void ComprasBtnReportes_Click(object Remitente, EventArgs Evento)
        {
            ComprasProcAbrirArchivo(AbrirReporte);
        }

        private void ComprasProcAbrirArchivo(Action Abrir)
        {
            try
            {
                Abrir();
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, Error.Message, "Compras", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnLoad(EventArgs Evento)
        {
            ComprasProcSeleccionarFondo();
            base.OnLoad(Evento);
        }

        private void ComprasProcSeleccionarFondo()
        {
            if (!_FondoSeleccionado && !DesignMode &&
                LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                int Indice;
                lock (_Aleatorio)
                {
                    Indice = _Aleatorio.Next(1, 9);
                }
                BackgroundImage = (Image)_Recursos.GetObject("Encabezado" + Indice);
                BackgroundImageLayout = ImageLayout.Stretch;
                _FondoSeleccionado = true;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs Evento)
        {
            if (BackgroundImage == null)
            {
                base.OnPaintBackground(Evento);
                return;
            }
            using (var Fondo = new SolidBrush(BackColor))
                Evento.Graphics.FillRectangle(Fondo, ClientRectangle);
            using (var Atributos = new ImageAttributes())
            {
                var Matriz = new ColorMatrix { Matrix33 = 0.7F };
                Atributos.SetColorMatrix(Matriz);
                Evento.Graphics.DrawImage(BackgroundImage, ClientRectangle,
                    0, 0, BackgroundImage.Width, BackgroundImage.Height,
                    GraphicsUnit.Pixel, Atributos);
            }
        }

        private void ComprasProcActualizarTitulo()
        {
            ComprasLblTitulo.Texto = _Titulo.ToUpperInvariant();
        }
    }
    public sealed class ComprasReporteEventArgs : EventArgs
    {
        public LocalReport Reporte { get; }

        public ComprasReporteEventArgs(LocalReport Reporte)
        {
            this.Reporte = Reporte;
        }
    }
}
