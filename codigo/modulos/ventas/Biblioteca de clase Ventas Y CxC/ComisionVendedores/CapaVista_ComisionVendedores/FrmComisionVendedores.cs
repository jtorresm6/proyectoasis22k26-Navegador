using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using CapaControlador_ComisionVendedores.Contratos;
using CapaControlador_ComisionVendedores.Controladores;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Enumeraciones;

namespace ComisionVendedoresVista
{
    public partial class FrmComisionVendedores : Form
    {
        private readonly IControladorComision _ControladorComision;
        private List<ClsComisionVendedorResultado> _Resultados;
        private List<ClsComisionFacturaDetalle> _DetallesFactura;
        private List<ClsProductoPorcentaje> _ProductosPorcentaje;
        private const int _FilasPorPagina = 9;
        private const int _FilasResultadosPorPagina = 9;
        private int _PaginaResultados;
        private int _PaginaFactura;
        private int _PaginaPorcentajes;
        private bool _CargandoCatalogosFactura;
        private int _IdFacturaVistaPrevia;
        private ComboBox ComisionVendedoresCboElementoPorcentaje;
        private bool _CargandoPorcentajes;
        private TabPage ComisionVendedoresTabNomina;
        private DataGridView ComisionVendedoresDgvNomina;
        private Label ComisionVendedoresLblNominaEstado;
        private List<ClsComisionNominaVista> _ComisionesNomina;
        private Button ComisionVendedoresBtnAnteriorNomina;
        private Button ComisionVendedoresBtnSiguienteNomina;
        private Button ComisionVendedoresBtnActualizarNomina;
        private Label ComisionVendedoresLblPaginaNomina;
        private const int _FilasNominaPorPagina = 9;
        private int _PaginaNomina;
        private readonly ErrorProvider _ErrorProveedor = new ErrorProvider();
        private readonly Dictionary<DataGridView, string> _ColumnasOrdenadas = new Dictionary<DataGridView, string>();
        private readonly Dictionary<DataGridView, bool> _OrdenAscendente = new Dictionary<DataGridView, bool>();
        private readonly Dictionary<Control, Panel> _ContenedoresValidacion = new Dictionary<Control, Panel>();
        private static readonly Dictionary<string, System.Drawing.Image> _IconosBotonesEstandarizados = new Dictionary<string, System.Drawing.Image>();
        private static readonly Dictionary<string, System.Drawing.Image> _IconosBotonesDimensionados = new Dictionary<string, System.Drawing.Image>();

        private sealed class ClsOpcionPorcentaje
        {
            public int IdClasificacion { get; set; }
            public string Etiqueta { get; set; }
        }

        public FrmComisionVendedores()
            : this(new ClsControladorComision())
        {
        }

        public FrmComisionVendedores(IControladorComision ControladorComision)
        {
            InitializeComponent();
            ComisionVendedoresMetEnvolverControl(ComisionVendedoresDtpFechaInicio, "ComisionVendedoresPnlFechaInicio");
            ComisionVendedoresMetEnvolverControl(ComisionVendedoresDtpFechaFin, "ComisionVendedoresPnlFechaFin");
            ComisionVendedoresMetEnvolverControl(ComisionVendedoresCboFactura, "ComisionVendedoresPnlFactura");
            ComisionVendedoresMetEnvolverControl(ComisionVendedoresCboVendedorFactura, "ComisionVendedoresPnlVendedor");
            ComisionVendedoresMetEnvolverControl(ComisionVendedoresCboAlcance, "ComisionVendedoresPnlAlcance");
            System.Drawing.Font FuenteCampo = new System.Drawing.Font("Segoe UI", 10F);
            ComisionVendedoresCboFactura.Font = FuenteCampo;
            ComisionVendedoresCboVendedorFactura.Font = FuenteCampo;
            ComisionVendedoresCboMetodoFactura.Font = FuenteCampo;
            ComisionVendedoresCboAlcance.Font = FuenteCampo;
            ComisionVendedoresTxtPorcentaje.Font = FuenteCampo;
            ComisionVendedoresDgvDetalleFactura.DefaultCellStyle.Font = FuenteCampo;
            ComisionVendedoresClmMetodoProducto.DefaultCellStyle.Font = FuenteCampo;
            _ErrorProveedor.ContainerControl = this;
            _ErrorProveedor.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            ComisionVendedoresMetCrearPestanaNomina();
            ComisionVendedoresBtnAyuda.Click += ComisionVendedoresMetAbrirAyuda;
            ComisionVendedoresCboMetodoFactura.Visible = false;
            ComisionVendedoresLblMetodoFactura.Visible = false;
            _ControladorComision = ControladorComision ?? throw new ArgumentNullException("ControladorComision");
            _Resultados = new List<ClsComisionVendedorResultado>();
            _DetallesFactura = new List<ClsComisionFacturaDetalle>();
            _ProductosPorcentaje = new List<ClsProductoPorcentaje>();
            ComisionVendedoresTbcPestanas.SelectedIndexChanged += ComisionVendedoresMetCambiarPestana;
            ComisionVendedoresTabAsignarFactura.Enter += ComisionVendedoresMetCambiarPestana;
            Load += (Sender, Argumentos) =>
            {
                if (ComisionVendedoresTbcPestanas.SelectedTab == ComisionVendedoresTabAsignarFactura)
                    ComisionVendedoresMetCargarCatalogosClick(Sender, Argumentos);
            };
            ComisionVendedoresCboFactura.SelectedValueChanged += ComisionVendedoresMetCambioSeleccionFactura;
            ComisionVendedoresCboVendedorFactura.SelectedValueChanged += ComisionVendedoresMetCambioSeleccionFactura;
            ComisionVendedoresDgvResultados.CellFormatting += ComisionVendedoresMetFormatearMoneda;
            ComisionVendedoresDgvDetalleFactura.CellFormatting += ComisionVendedoresMetFormatearMoneda;
            ComisionVendedoresLblEstado.AutoSize = false;
            ComisionVendedoresLblEstado.Size = new System.Drawing.Size(1210, 28);
            ComisionVendedoresLblEstado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ComisionVendedoresLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ComisionVendedoresLblEstadoFactura.AutoSize = false;
            ComisionVendedoresLblEstadoFactura.Size = new System.Drawing.Size(1210, 40);
            ComisionVendedoresLblEstadoFactura.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ComisionVendedoresLblEstadoFactura.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ComisionVendedoresDgvResultados.ColumnHeaderMouseClick += ComisionVendedoresMetOrdenarTabla;
            ComisionVendedoresDgvDetalleFactura.ColumnHeaderMouseClick += ComisionVendedoresMetOrdenarTabla;
            ComisionVendedoresDgvDetalleFactura.CellPainting += ComisionVendedoresMetMarcarColumnaObligatoria;
            ComisionVendedoresDgvPorcentajes.ColumnHeaderMouseClick += ComisionVendedoresMetOrdenarTabla;
            ComisionVendedoresDgvDetalleFactura.CurrentCellDirtyStateChanged += (Sender, Argumentos) =>
            {
                if (ComisionVendedoresDgvDetalleFactura.IsCurrentCellDirty)
                    ComisionVendedoresDgvDetalleFactura.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            ComisionVendedoresDgvDetalleFactura.CellValueChanged += ComisionVendedoresMetCambiarMetodoProducto;
            ComisionVendedoresCboAlcance.SelectedIndexChanged += ComisionVendedoresMetCambiarAlcancePorcentaje;
            ComisionVendedoresLblAlcance.Text = "Tipo de porcentaje";
            ComisionVendedoresLblAlcance.Location = new System.Drawing.Point(24, 7);
            ComisionVendedoresCboAlcance.Location = new System.Drawing.Point(24, 25);
            ComisionVendedoresCboAlcance.Size = new System.Drawing.Size(150, 31);
            ComisionVendedoresLblPorcentajeNuevo.AutoSize = false;
            ComisionVendedoresLblPorcentajeNuevo.Text = "Porcentaje nuevo";
            ComisionVendedoresLblPorcentajeNuevo.Location = new System.Drawing.Point(420, 7);
            ComisionVendedoresLblPorcentajeNuevo.Size = new System.Drawing.Size(150, 23);
            ComisionVendedoresTxtPorcentaje.Location = new System.Drawing.Point(420, 25);
            ComisionVendedoresTxtPorcentaje.Size = new System.Drawing.Size(110, 30);
            ComisionVendedoresMetEnvolverControl(ComisionVendedoresTxtPorcentaje, "ComisionVendedoresPnlPorcentaje");
            ComisionVendedoresBtnBuscar.Location = new System.Drawing.Point(568, 23);
            ComisionVendedoresBtnBuscar.Size = new System.Drawing.Size(100, 30);
            ComisionVendedoresBtnActualizar.Location = new System.Drawing.Point(670, 23);
            ComisionVendedoresBtnActualizar.Size = new System.Drawing.Size(190, 30);
            ComisionVendedoresLblMantenimientoEstado.AutoSize = false;
            ComisionVendedoresLblMantenimientoEstado.Location = new System.Drawing.Point(24, 68);
            ComisionVendedoresLblMantenimientoEstado.Size = new System.Drawing.Size(1210, 28);
            Label EtiquetaElementoPorcentaje = new Label
            {
                Name = "ComisionVendedoresLblElementoPorcentaje",
                Text = "Elemento específico", Location = new System.Drawing.Point(190, 7),
                Size = new System.Drawing.Size(220, 23), ForeColor = System.Drawing.Color.FromArgb(59, 29, 74)
            };
            ComisionVendedoresTabMantenimiento.Controls.Add(EtiquetaElementoPorcentaje);
            Label AsteriscoElementoPorcentaje = new Label
            {
                Name = "ComisionVendedoresLblObligatorioElementoPorcentaje", Text = "*",
                Location = new System.Drawing.Point(EtiquetaElementoPorcentaje.Left + System.Windows.Forms.TextRenderer.MeasureText(EtiquetaElementoPorcentaje.Text, EtiquetaElementoPorcentaje.Font).Width + 2, 7), Size = new System.Drawing.Size(12, 18),
                Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Firebrick
            };
            ComisionVendedoresTabMantenimiento.Controls.Add(AsteriscoElementoPorcentaje);
            ComisionVendedoresCboElementoPorcentaje = new ComboBox
            {
                Name = "ComisionVendedoresCboElementoPorcentaje",
                DropDownStyle = ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(190, 25),
                Size = new System.Drawing.Size(220, 31), Font = ComisionVendedoresCboAlcance.Font
            };
            ComisionVendedoresTabMantenimiento.Controls.Add(ComisionVendedoresCboElementoPorcentaje);
            ComisionVendedoresMetEnvolverControl(ComisionVendedoresCboElementoPorcentaje, "ComisionVendedoresPnlElementoPorcentaje");
            ComisionVendedoresLblObligatorioAlcance.Text = "*";
            ComisionVendedoresLblObligatorioAlcance.Visible = true;
            ComisionVendedoresLblObligatorioAlcance.Location = new System.Drawing.Point(ComisionVendedoresLblAlcance.Left + System.Windows.Forms.TextRenderer.MeasureText(ComisionVendedoresLblAlcance.Text, ComisionVendedoresLblAlcance.Font).Width + 2, 7);
            ComisionVendedoresLblObligatorioPorcentaje.Text = "*";
            ComisionVendedoresLblObligatorioPorcentaje.Visible = true;
            ComisionVendedoresLblObligatorioPorcentaje.Location = new System.Drawing.Point(ComisionVendedoresLblPorcentajeNuevo.Left + System.Windows.Forms.TextRenderer.MeasureText(ComisionVendedoresLblPorcentajeNuevo.Text, ComisionVendedoresLblPorcentajeNuevo.Font).Width + 2, 7);
            ComisionVendedoresLblObligatorioAlcance.BringToFront();
            AsteriscoElementoPorcentaje.BringToFront();
            ComisionVendedoresLblObligatorioPorcentaje.BringToFront();
            ComisionVendedoresCboElementoPorcentaje.AccessibleDescription = "Campo obligatorio";
            ComisionVendedoresLblObligatorioFactura.Text = "*";
            ComisionVendedoresLblObligatorioFactura.Visible = true;
            ComisionVendedoresLblObligatorioVendedor.Text = "*";
            ComisionVendedoresLblObligatorioVendedor.Visible = true;
            ComisionVendedoresCboAlcance.AccessibleDescription = "Campo obligatorio";
            ComisionVendedoresTxtPorcentaje.AccessibleDescription = "Campo obligatorio";
            ComisionVendedoresCboFactura.AccessibleDescription = "Campo obligatorio";
            ComisionVendedoresCboVendedorFactura.AccessibleDescription = "Campo obligatorio";
            ComisionVendedoresDtpFechaInicio.AccessibleDescription = "Campo obligatorio";
            ComisionVendedoresDtpFechaFin.AccessibleDescription = "Campo obligatorio";
            ComisionVendedoresMetEstandarizarBotones();
        }

        private void ComisionVendedoresMetEstandarizarBotones()
        {
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnAyuda, "btn_ayuda", "Ayuda", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnCalcular, "btn_aplicarFiltro", "Calcular", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnCalcularFactura, "btn_consultar", "Calcular comisión", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnAsignarVendedorFactura, "btn_enlace", "Asignar vendedor", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnRegistrarAbonoFactura, "btn_ingresar", "Realizar pago", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnBuscar, "btn_buscar", "Buscar", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnActualizar, "btn_modificar", "Actualizar", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnAnteriorResultados, "btn_anterior", "Anterior", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnSiguienteResultados, "btn_siguiente", "Siguiente", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnAnteriorFactura, "btn_anterior", "Anterior", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnSiguienteFactura, "btn_siguiente", "Siguiente", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnAnteriorPorcentajes, "btn_anterior", "Anterior", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnSiguientePorcentajes, "btn_siguiente", "Siguiente", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnActualizarNomina, "btn_refrescar", "Actualizar", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnAnteriorNomina, "btn_anterior", "Anterior", new System.Drawing.Size(76, 78));
            ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnSiguienteNomina, "btn_siguiente", "Siguiente", new System.Drawing.Size(76, 78));

            ComisionVendedoresBtnCalcular.Location = new System.Drawing.Point(572, 5);
            ComisionVendedoresBtnAyuda.Location = new System.Drawing.Point(664, 5);
            ComisionVendedoresLblEstado.Location = new System.Drawing.Point(24, 100);
            ComisionVendedoresDgvResultados.Location = new System.Drawing.Point(24, 130);
            ComisionVendedoresDgvResultados.Size = new System.Drawing.Size(1210, 368);

            ComisionVendedoresBtnCalcularFactura.Location = new System.Drawing.Point(24, 80);
            ComisionVendedoresBtnAsignarVendedorFactura.Location = new System.Drawing.Point(116, 80);
            ComisionVendedoresBtnRegistrarAbonoFactura.Location = new System.Drawing.Point(208, 80);
            ComisionVendedoresLblEstadoFactura.Location = new System.Drawing.Point(24, 174);
            ComisionVendedoresDgvDetalleFactura.Location = new System.Drawing.Point(24, 208);
            ComisionVendedoresDgvDetalleFactura.Size = new System.Drawing.Size(1210, 290);

            ComisionVendedoresBtnBuscar.Location = new System.Drawing.Point(570, 4);
            ComisionVendedoresBtnActualizar.Location = new System.Drawing.Point(664, 4);
            ComisionVendedoresLblMantenimientoEstado.Location = new System.Drawing.Point(24, 100);
            ComisionVendedoresDgvPorcentajes.Location = new System.Drawing.Point(24, 130);
            ComisionVendedoresDgvPorcentajes.Size = new System.Drawing.Size(1210, 368);

            Button[] BotonesAnteriores = { ComisionVendedoresBtnAnteriorResultados, ComisionVendedoresBtnAnteriorFactura,
                ComisionVendedoresBtnAnteriorPorcentajes, ComisionVendedoresBtnAnteriorNomina };
            Button[] BotonesSiguientes = { ComisionVendedoresBtnSiguienteResultados, ComisionVendedoresBtnSiguienteFactura,
                ComisionVendedoresBtnSiguientePorcentajes, ComisionVendedoresBtnSiguienteNomina };
            foreach (Button Boton in BotonesAnteriores) Boton.Location = new System.Drawing.Point(24, 496);
            foreach (Button Boton in BotonesSiguientes) Boton.Location = new System.Drawing.Point(1184, 496);
            ComisionVendedoresLblPaginaResultados.Location = new System.Drawing.Point(486, 532);
            ComisionVendedoresLblPaginaFactura.Location = new System.Drawing.Point(486, 532);
            ComisionVendedoresLblPaginaPorcentajes.Location = new System.Drawing.Point(486, 532);
            ComisionVendedoresDgvNomina.Location = new System.Drawing.Point(24, 104);
            ComisionVendedoresDgvNomina.Size = new System.Drawing.Size(1210, 394);
            ComisionVendedoresLblPaginaNomina.Location = new System.Drawing.Point(486, 532);
            ComisionVendedoresBtnActualizarNomina.Location = new System.Drawing.Point(1166, 8);
        }
        internal static void ComisionVendedoresMetAplicarBotonEstandar(Button Boton, string Icono, string Etiqueta, System.Drawing.Size Tamano)
        {
            System.Drawing.Image ImagenNormal;
            if (!_IconosBotonesEstandarizados.TryGetValue(Icono, out ImagenNormal))
            {
                string NombreRecurso = "ComisionVendedoresVista.IconosEstandarizados." + Icono + ".png";
                using (Stream Recurso = Assembly.GetExecutingAssembly().GetManifestResourceStream(NombreRecurso))
                {
                    if (Recurso == null) throw new InvalidOperationException("No se encontró el botón estandarizado " + Icono + ".");
                    using (System.Drawing.Image Imagen = System.Drawing.Image.FromStream(Recurso))
                        ImagenNormal = new System.Drawing.Bitmap(Imagen);
                }
                _IconosBotonesEstandarizados.Add(Icono, ImagenNormal);
            }

            int TamanoIcono = Math.Max(24, Math.Min(52, Math.Min(Tamano.Width - 12, Tamano.Height - 38)));
            string ClaveImagenDimensionada = Icono + "_" + TamanoIcono.ToString(CultureInfo.InvariantCulture);
            System.Drawing.Image ImagenDimensionada;
            if (!_IconosBotonesDimensionados.TryGetValue(ClaveImagenDimensionada, out ImagenDimensionada))
            {
                ImagenDimensionada = new System.Drawing.Bitmap(ImagenNormal, new System.Drawing.Size(TamanoIcono, TamanoIcono));
                _IconosBotonesDimensionados.Add(ClaveImagenDimensionada, ImagenDimensionada);
            }

            System.Drawing.Image ImagenDeshabilitada = new System.Drawing.Bitmap(ImagenDimensionada);
            System.Drawing.Bitmap IconoDeshabilitado = (System.Drawing.Bitmap)ImagenDeshabilitada;
            for (int Y = 0; Y < IconoDeshabilitado.Height; Y++)
                for (int X = 0; X < IconoDeshabilitado.Width; X++)
                {
                    System.Drawing.Color Pixel = IconoDeshabilitado.GetPixel(X, Y);
                    if (Pixel.A > 0)
                        IconoDeshabilitado.SetPixel(X, Y, System.Drawing.Color.FromArgb(Pixel.A, 128, 128, 128));
                }

            Boton.Size = Tamano;
            Boton.Text = Etiqueta;
            Boton.AccessibleName = Etiqueta;
            Boton.AccessibleDescription = Etiqueta;
            Boton.Image = Boton.Enabled ? ImagenDimensionada : ImagenDeshabilitada;
            Boton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            Boton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            Boton.TextImageRelation = TextImageRelation.ImageAboveText;
            Boton.Padding = new Padding(2);
            Boton.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            Boton.FlatStyle = FlatStyle.Flat;
            Boton.FlatAppearance.BorderSize = 1;
            Boton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(228, 212, 234);
            Boton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            Boton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(201, 162, 74);
            Boton.UseVisualStyleBackColor = false;
            Boton.BackColor = Boton.Enabled ? System.Drawing.Color.FromArgb(59, 29, 74) : System.Drawing.Color.LightGray;
            Boton.ForeColor = Boton.Enabled ? System.Drawing.Color.FromArgb(248, 245, 249) : System.Drawing.SystemColors.GrayText;
            Boton.Cursor = Cursors.Hand;
            Boton.Tag = Icono;
            Boton.EnabledChanged += (Sender, Argumentos) =>
            {
                Boton.BackColor = Boton.Enabled ? System.Drawing.Color.FromArgb(59, 29, 74) : System.Drawing.Color.LightGray;
                Boton.ForeColor = Boton.Enabled ? System.Drawing.Color.FromArgb(248, 245, 249) : System.Drawing.SystemColors.GrayText;
                Boton.Image = Boton.Enabled ? ImagenDimensionada : ImagenDeshabilitada;
            };
        }

        private void ComisionVendedoresMetCalcularClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblEstado,
                    ComisionVendedoresDtpFechaInicio, ComisionVendedoresDtpFechaFin);
                if (ComisionVendedoresDtpFechaInicio.Value.Date > ComisionVendedoresDtpFechaFin.Value.Date)
                    throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");
                _Resultados = _ControladorComision.ComisionVendedoresMetCalcular(EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado,
                    ComisionVendedoresDtpFechaInicio.Value.Date, ComisionVendedoresDtpFechaFin.Value.Date);
                _PaginaResultados = 0;
                ComisionVendedoresMetMostrarPaginaResultados();
                ComisionVendedoresLblEstado.Text = _Resultados.Count == 0
                    ? "No hay comisiones asignadas ni pagos de comisión en el rango seleccionado."
                    : "Cálculo completado. Se muestran los movimientos de comisión devengados en el período.";
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstado, Excepcion.Message,
                    ComisionVendedoresDtpFechaInicio, ComisionVendedoresDtpFechaFin);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetRegistrarAbonoFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                int IdUsuario = ComisionVendedoresMetObtenerIdUsuarioSesion();
                ClsFacturaComisionVista FacturaSeleccionada = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                using (FrmPagoFacturaTemporal Formulario = new FrmPagoFacturaTemporal(_ControladorComision, IdUsuario,
                    FacturaSeleccionada == null ? (int?)null : FacturaSeleccionada.IdFactura))
                {
                    if (Formulario.ShowDialog(this) == DialogResult.OK)
                    {
                        ComisionVendedoresLblEstadoFactura.Text =
                            "Abono de cliente registrado. La comisión proporcional queda devengada y aparecerá al calcular comisiones.";
                        ComisionVendedoresMetCargarCatalogosClick(Sender, Argumentos);
                    }
                }
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresLblEstadoFactura.Text = Excepcion.Message;
                MessageBox.Show(this, Excepcion.Message, "Registrar abono de factura",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int ComisionVendedoresMetObtenerIdUsuarioSesion()
        {
            const string NombreTipo = "CapaControlador_Seguridad.Objetos_de_valor.ClsSesionSeguridad";
            Type TipoSesion = AppDomain.CurrentDomain.GetAssemblies()
                .Select(Ensamblado => Ensamblado.GetType(NombreTipo, false))
                .FirstOrDefault(Tipo => Tipo != null);
            if (TipoSesion == null)
            {
                try
                {
                    TipoSesion = Assembly.Load("CapaControlador_Seguridad").GetType(NombreTipo, false);
                }
                catch { }
            }
            PropertyInfo Propiedad = TipoSesion == null ? null : TipoSesion.GetProperty(
                "IdUsuario", BindingFlags.Public | BindingFlags.Static);
            if (Propiedad == null)
                throw new InvalidOperationException("No se encontró el usuario de la sesión. Inicie sesión nuevamente para registrar el abono.");
            int IdUsuario = Convert.ToInt32(Propiedad.GetValue(null, null));
            if (IdUsuario <= 0)
                throw new InvalidOperationException("La sesión no tiene un usuario válido. Inicie sesión nuevamente para registrar el abono.");
            return IdUsuario;
        }

        private void ComisionVendedoresMetBuscarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblMantenimientoEstado,
                    ComisionVendedoresCboAlcance, ComisionVendedoresCboElementoPorcentaje, ComisionVendedoresTxtPorcentaje);
                if (ComisionVendedoresCboAlcance.SelectedItem == null)
                    throw new ArgumentException("Seleccione el tipo de porcentaje.");
                List<ClsProductoPorcentaje> Productos = ComisionVendedoresMetCargarPorcentajes();
                ComisionVendedoresLblMantenimientoEstado.Text = Productos.Count + " producto(s) cargados. Elija el elemento específico que desea modificar.";
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado, Excepcion.Message, ComisionVendedoresCboAlcance);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetCargarCatalogosClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                _CargandoCatalogosFactura = true;
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblEstadoFactura,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                ComisionVendedoresCboFactura.DataSource = null;
                ComisionVendedoresCboVendedorFactura.DataSource = null;
                List<ClsFacturaComisionVista> Facturas = _ControladorComision.ComisionVendedoresMetObtenerFacturas();
                List<ClsVendedorComisionVista> Vendedores = _ControladorComision.ComisionVendedoresMetObtenerVendedores();
                // Usa un campo simple del DTO para asegurar que WinForms muestre el elemento
                // aunque falle el enlace de la propiedad calculada Etiqueta.
                ComisionVendedoresCboFactura.DisplayMember = "IdFactura";
                ComisionVendedoresCboFactura.ValueMember = "IdFactura";
                ComisionVendedoresCboFactura.DataSource = Facturas;
                if (Facturas.Count > 0 && ComisionVendedoresCboFactura.SelectedIndex < 0)
                    ComisionVendedoresCboFactura.SelectedIndex = 0;
                ComisionVendedoresCboVendedorFactura.DisplayMember = "NombreVendedor";
                ComisionVendedoresCboVendedorFactura.ValueMember = "IdVendedor";
                ComisionVendedoresCboVendedorFactura.DataSource = Vendedores;
                ClsFacturaComisionVista FacturaSeleccionada = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (FacturaSeleccionada != null && FacturaSeleccionada.IdVendedor > 0 &&
                    ComisionVendedoresCboVendedorFactura.Items.Cast<ClsVendedorComisionVista>().Any(Vendedor => Vendedor.IdVendedor == FacturaSeleccionada.IdVendedor))
                    ComisionVendedoresCboVendedorFactura.SelectedValue = FacturaSeleccionada.IdVendedor;
                else
                    ComisionVendedoresCboVendedorFactura.SelectedIndex = -1;
                ComisionVendedoresLblEstadoFactura.Text = Facturas.Count == 0
                    ? "No hay facturas pendientes de asignación (puede que no existan facturas emitidas nuevas o que ya tengan comisión registrada)."
                    : Facturas.Count + " factura(s) y " + Vendedores.Count + " vendedor(es) cargados. Seleccione una factura.";
                _CargandoCatalogosFactura = false;
                if (Facturas.Count == 0)
                {
                    ComisionVendedoresDgvDetalleFactura.DataSource = null;
                    _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                    return;
                }
                ComisionVendedoresMetCalcularFacturaSeleccionada();
            }
            catch (Exception Excepcion)
            {
                _CargandoCatalogosFactura = false;
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstadoFactura, Excepcion.Message,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetCalcularFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblEstadoFactura,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                if (ComisionVendedoresCboFactura.SelectedValue == null || ComisionVendedoresCboVendedorFactura.SelectedValue == null)
                {
                    ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstadoFactura,
                        "Seleccione una factura y un vendedor para continuar.", ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                    ComisionVendedoresLblEstadoFactura.ForeColor = System.Drawing.Color.Firebrick;
                    ComisionVendedoresLblEstadoFactura.Text = "No se puede calcular: seleccione una factura y un vendedor. Si el combo de factura está vacío, revise el error de carga del catálogo.";
                    return;
                }

                int IdFactura = Convert.ToInt32(ComisionVendedoresCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(ComisionVendedoresCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                EnumMetodoComision Metodo = EnumMetodoComision.Producto;
                List<ClsComisionFacturaDetalle> DetalleFacturaCalculado;
                if (_DetallesFactura.Count > 0 && _IdFacturaVistaPrevia == IdFactura)
                {
                    List<ClsComisionFacturaDetalle> Selecciones = ComisionVendedoresMetObtenerSeleccionesMetodo();
                    DetalleFacturaCalculado = _ControladorComision.ComisionVendedoresMetCalcularFactura(IdFactura, IdVendedor, Selecciones);
                }
                else
                    DetalleFacturaCalculado = _ControladorComision.ComisionVendedoresMetCalcularFactura(IdFactura, IdVendedor, Metodo);
                ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                string SimboloMoneda = Factura == null ? String.Empty : Factura.SimboloMoneda;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    // La columna ComboBox compara valores exactos. Las tasas históricas
                    // pueden venir como PRODUCTO/LINEA en mayúscula desde MySQL.
                    Detalle.MetodoComision = ComisionVendedoresMetNormalizarMetodo(Detalle.MetodoComision);
                    Detalle.SimboloMoneda = SimboloMoneda;
                }
                ComisionVendedoresDgvDetalleFactura.DataSource = null;
                _DetallesFactura = DetalleFacturaCalculado;
                _IdFacturaVistaPrevia = IdFactura;
                _PaginaFactura = 0;
                ComisionVendedoresMetMostrarPaginaFactura();

                decimal TotalComision = 0m, Disponible = 0m, Pendiente = 0m, ComisionUltimoAbono = 0m, TotalBase = 0m;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    TotalBase += Detalle.BaseVenta;
                    TotalComision += Detalle.ComisionTotal;
                    Disponible += Detalle.ComisionDisponible;
                    Pendiente += Detalle.ComisionPendiente;
                    ComisionUltimoAbono += Detalle.ComisionUltimoAbono;
                }
                ClsVendedorComisionVista Vendedor = ComisionVendedoresCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal PagadoFactura = Factura == null ? 0m : Factura.TotalPagado;
                decimal PorcentajeTotal = TotalBase <= 0m ? 0m : TotalComision / TotalBase * 100m;
                ComisionVendedoresLblEstadoFactura.Text = "Factura " + IdFactura + " · " + (Vendedor == null ? "" : Vendedor.NombreVendedor) +
                    " · Cobrado " + SimboloMoneda + " " + PagadoFactura.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + Environment.NewLine +
                    "Total comisión: " + PorcentajeTotal.ToString("0.00") + "% · " + SimboloMoneda + " " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Disponible " + SimboloMoneda + " " + Disponible.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Comisión del último abono " + SimboloMoneda + " " + ComisionUltimoAbono.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Pendiente " + SimboloMoneda + " " + Pendiente.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            }
            catch (Exception Excepcion)
            {
                string MensajeError = ComisionVendedoresMetObtenerDetalleError(Excepcion);
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstadoFactura, MensajeError,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                MessageBox.Show(MensajeError, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetAsignarVendedorFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblEstadoFactura,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                if (ComisionVendedoresCboFactura.SelectedValue == null || ComisionVendedoresCboVendedorFactura.SelectedValue == null)
                {
                    ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstadoFactura,
                        "Seleccione una factura y un vendedor.", ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                    return;
                }
                int IdFactura = Convert.ToInt32(ComisionVendedoresCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(ComisionVendedoresCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                List<ClsComisionFacturaDetalle> Selecciones = ComisionVendedoresMetObtenerSeleccionesMetodo();
                ClsVendedorComisionVista Vendedor = ComisionVendedoresCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal TotalComisionGuardada = Selecciones.Sum(Detalle => Detalle.ComisionTotal);
                ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                string SimboloMoneda = Factura == null ? String.Empty : Factura.SimboloMoneda;
                string TextoConfirmacion = "Se asignará la factura " + IdFactura + " a " +
                    (Vendedor == null ? "el vendedor seleccionado" : Vendedor.NombreVendedor) + ". Se guardarán " +
                    Selecciones.Count + " líneas de comisión por " + SimboloMoneda + " " +
                    TotalComisionGuardada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ". ¿Desea continuar?";
                if (ComisionVendedoresMetConfirmar(this, "7008 – ComisionVendedores", TextoConfirmacion, "Sí, asignar") != DialogResult.Yes)
                    return;
                int IdComision = _ControladorComision.ComisionVendedoresMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Selecciones);
                string Confirmacion = "Factura " + IdFactura + " asignada a " +
                    (Vendedor == null ? "vendedor" : Vendedor.NombreVendedor) + ". Se guardaron " +
                    Selecciones.Count + " líneas de comisión por un total de " + SimboloMoneda + " " +
                    TotalComisionGuardada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " (registro " + IdComision + "). La factura se quitó de la lista para evitar asignarla otra vez.";
                ComisionVendedoresMetActualizarFacturasDisponiblesDespuesAsignar(IdFactura, Confirmacion);
                ComisionVendedoresMetMostrarExito(this, "7008 – ComisionVendedores", Confirmacion);
            }
            catch (Exception Excepcion)
            {
                string MensajeError = ComisionVendedoresMetObtenerDetalleError(Excepcion);
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstadoFactura, MensajeError,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                MessageBox.Show(MensajeError, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static DialogResult ComisionVendedoresMetConfirmar(IWin32Window VentanaPropietaria, string Titulo, string Mensaje, string TextoAccion)
        {
            return ComisionVendedoresMetMostrarDialogoOperacion(VentanaPropietaria, Titulo, Mensaje, true, TextoAccion);
        }

        internal static void ComisionVendedoresMetMostrarExito(IWin32Window VentanaPropietaria, string Titulo, string Mensaje)
        {
            ComisionVendedoresMetMostrarDialogoOperacion(VentanaPropietaria, Titulo, Mensaje, false, "Aceptar");
        }

        private static DialogResult ComisionVendedoresMetMostrarDialogoOperacion(
            IWin32Window VentanaPropietaria, string Titulo, string Mensaje, bool EsConfirmacion, string TextoAccion)
        {
            using (Form Dialogo = new Form())
            {
                Dialogo.Name = "ComisionVendedoresFrmDialogoOperacion";
                Dialogo.Text = Titulo;
                Dialogo.StartPosition = FormStartPosition.CenterParent;
                Dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                Dialogo.MaximizeBox = false;
                Dialogo.MinimizeBox = true;
                Dialogo.ShowInTaskbar = false;
                Dialogo.ClientSize = new System.Drawing.Size(580, 220);
                Dialogo.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
                Dialogo.Font = new System.Drawing.Font("Segoe UI", 10F);
                Dialogo.Icon = Properties.ClsRecursosComision.IconoVentas;

                Panel ComisionVendedoresPnlEncabezadoDialogo = new Panel
                {
                    Name = "ComisionVendedoresPnlEncabezadoDialogo",
                    BackColor = System.Drawing.Color.FromArgb(59, 29, 74),
                    Location = new System.Drawing.Point(0, 0), Size = new System.Drawing.Size(580, 58)
                };
                Label ComisionVendedoresLblEstadoDialogo = new Label
                {
                    Name = "ComisionVendedoresLblEstadoDialogo",
                    Text = EsConfirmacion ? "!" : "✓",
                    Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold),
                    ForeColor = EsConfirmacion ? System.Drawing.Color.FromArgb(201, 162, 74) : System.Drawing.Color.ForestGreen,
                    Location = new System.Drawing.Point(18, 7), Size = new System.Drawing.Size(40, 44),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };
                Label ComisionVendedoresLblTituloDialogo = new Label
                {
                    Name = "ComisionVendedoresLblTituloDialogo",
                    Text = EsConfirmacion ? "Confirmar operación" : "Operación completada",
                    Font = new System.Drawing.Font("Lucida Sans Unicode", 14F,
                        System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic),
                    ForeColor = System.Drawing.Color.FromArgb(248, 245, 249),
                    Location = new System.Drawing.Point(68, 12), Size = new System.Drawing.Size(490, 34),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                };
                ComisionVendedoresPnlEncabezadoDialogo.Controls.Add(ComisionVendedoresLblEstadoDialogo);
                ComisionVendedoresPnlEncabezadoDialogo.Controls.Add(ComisionVendedoresLblTituloDialogo);
                Dialogo.Controls.Add(ComisionVendedoresPnlEncabezadoDialogo);

                Label ComisionVendedoresLblMensajeDialogo = new Label
                {
                    Name = "ComisionVendedoresLblMensajeDialogo",
                    Text = Mensaje, Font = new System.Drawing.Font("Tahoma", 9F),
                    ForeColor = System.Drawing.Color.FromArgb(59, 29, 74),
                    Location = new System.Drawing.Point(20, 70), Size = new System.Drawing.Size(540, 58),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft, AutoEllipsis = true
                };
                Dialogo.Controls.Add(ComisionVendedoresLblMensajeDialogo);

                Button ComisionVendedoresBtnAceptarDialogo = new Button
                {
                    Name = "ComisionVendedoresBtnAceptarDialogo",
                    Text = TextoAccion, DialogResult = EsConfirmacion ? DialogResult.Yes : DialogResult.OK,
                    Location = new System.Drawing.Point(EsConfirmacion ? 450 : 490, 136), Size = new System.Drawing.Size(76, 78),
                    BackColor = System.Drawing.Color.FromArgb(59, 29, 74),
                    ForeColor = System.Drawing.Color.FromArgb(248, 245, 249), FlatStyle = FlatStyle.Flat
                };
                ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnAceptarDialogo,
                    EsConfirmacion ? "btn_guardar" : "btn_salir", EsConfirmacion ? "Confirmar" : "Cerrar", new System.Drawing.Size(76, 78));
                ComisionVendedoresBtnAceptarDialogo.AccessibleDescription = TextoAccion;
                
                Dialogo.Controls.Add(ComisionVendedoresBtnAceptarDialogo);
                Dialogo.AcceptButton = ComisionVendedoresBtnAceptarDialogo;

                if (EsConfirmacion)
                {
                    Button ComisionVendedoresBtnCancelarDialogo = new Button
                    {
                        Name = "ComisionVendedoresBtnCancelarDialogo", Text = "No", DialogResult = DialogResult.No,
                        Location = new System.Drawing.Point(370, 136), Size = new System.Drawing.Size(76, 78),
                        BackColor = System.Drawing.Color.FromArgb(59, 29, 74),
                        ForeColor = System.Drawing.Color.FromArgb(248, 245, 249), FlatStyle = FlatStyle.Flat
                    };
                    ComisionVendedoresMetAplicarBotonEstandar(ComisionVendedoresBtnCancelarDialogo,
                        "btn_cancelar", "No", new System.Drawing.Size(76, 78));
                    
                    Dialogo.Controls.Add(ComisionVendedoresBtnCancelarDialogo);
                    Dialogo.CancelButton = ComisionVendedoresBtnCancelarDialogo;
                }

                return VentanaPropietaria == null ? Dialogo.ShowDialog() : Dialogo.ShowDialog(VentanaPropietaria);
            }
        }
        private void ComisionVendedoresMetActualizarFacturasDisponiblesDespuesAsignar(int IdFactura, string Confirmacion)
        {
            try
            {
                List<ClsFacturaComisionVista> FacturasDisponibles = _ControladorComision.ComisionVendedoresMetObtenerFacturas();
                _CargandoCatalogosFactura = true;
                ComisionVendedoresCboFactura.DataSource = null;
                ComisionVendedoresCboFactura.DisplayMember = "IdFactura";
                ComisionVendedoresCboFactura.ValueMember = "IdFactura";
                ComisionVendedoresCboFactura.DataSource = FacturasDisponibles;
                ComisionVendedoresCboFactura.SelectedIndex = -1;
                ComisionVendedoresCboVendedorFactura.SelectedIndex = -1;
                _CargandoCatalogosFactura = false;
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                _IdFacturaVistaPrevia = 0;
                _PaginaFactura = 0;
                ComisionVendedoresDgvDetalleFactura.DataSource = null;
                ComisionVendedoresMetMostrarPaginaFactura();
                ComisionVendedoresLblEstadoFactura.Text = Confirmacion + Environment.NewLine +
                    (FacturasDisponibles.Count == 0
                        ? "No quedan facturas pendientes de asignación."
                        : "Quedan " + FacturasDisponibles.Count + " factura(s) pendientes de asignación.");
            }
            catch (Exception ErrorActualizacion)
            {
                _CargandoCatalogosFactura = false;
                ComisionVendedoresLblEstadoFactura.Text = Confirmacion + Environment.NewLine +
                    "La asignación se guardó, pero no se pudo actualizar la lista de facturas: " +
                    ComisionVendedoresMetObtenerDetalleError(ErrorActualizacion);
            }
        }

        private void ComisionVendedoresMetCambiarPestana(object Sender, EventArgs Argumentos)
        {
            if (ComisionVendedoresTbcPestanas.SelectedTab == ComisionVendedoresTabAsignarFactura)
                ComisionVendedoresMetCargarCatalogosClick(Sender, Argumentos);
            else if (ComisionVendedoresTbcPestanas.SelectedTab == ComisionVendedoresTabMantenimiento)
                ComisionVendedoresMetBuscarClick(Sender, Argumentos);
            else if (ComisionVendedoresTbcPestanas.SelectedTab == ComisionVendedoresTabNomina)
                ComisionVendedoresMetCargarNominaClick(Sender, Argumentos);
        }

        private void ComisionVendedoresMetCrearPestanaNomina()
        {
            ComisionVendedoresTabNomina = new TabPage("Comisiones emitidas")
            {
                Name = "ComisionVendedoresTabNomina",
                BackColor = System.Drawing.Color.FromArgb(248, 245, 249),
                Padding = new Padding(16),
                Size = new System.Drawing.Size(1272, 582)
            };
            ComisionVendedoresBtnActualizarNomina = new Button
            {
                Name = "ComisionVendedoresBtnActualizarNomina", Text = "Actualizar",
                Location = new System.Drawing.Point(24, 18), Size = new System.Drawing.Size(120, 30),
                BackColor = System.Drawing.Color.FromArgb(123, 63, 147), ForeColor = System.Drawing.Color.FromArgb(248, 245, 249),
                FlatStyle = FlatStyle.Flat
            };
            ComisionVendedoresBtnActualizarNomina.FlatAppearance.BorderSize = 0;
            ComisionVendedoresBtnActualizarNomina.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ComisionVendedoresBtnActualizarNomina.Location = new System.Drawing.Point(1128, 18);
            ComisionVendedoresBtnActualizarNomina.Click += ComisionVendedoresMetCargarNominaClick;
            ComisionVendedoresLblNominaEstado = new Label
            {
                Name = "ComisionVendedoresLblNominaEstado", AutoSize = false,
                Location = new System.Drawing.Point(160, 18), Size = new System.Drawing.Size(930, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ForeColor = System.Drawing.Color.FromArgb(59, 29, 74), TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            ComisionVendedoresDgvNomina = new DataGridView
            {
                Name = "ComisionVendedoresDgvNomina", Location = new System.Drawing.Point(24, 60),
                Size = new System.Drawing.Size(1210, 480), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AutoGenerateColumns = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.FromArgb(248, 245, 249),
                AllowUserToResizeRows = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                RowTemplate = { Height = 22 },
                RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(228, 212, 234), ForeColor = System.Drawing.Color.FromArgb(59, 29, 74) },
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(248, 245, 249),
                    ForeColor = System.Drawing.Color.FromArgb(59, 29, 74),
                    SelectionBackColor = System.Drawing.Color.FromArgb(123, 63, 147),
                    SelectionForeColor = System.Drawing.Color.FromArgb(248, 245, 249) }
            };
            ComisionVendedoresDgvNomina.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            ComisionVendedoresDgvNomina.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            ComisionVendedoresDgvNomina.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            ComisionVendedoresDgvNomina.ColumnHeadersHeight = 54;
            ComisionVendedoresDgvNomina.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ComisionVendedoresDgvNomina.EnableHeadersVisualStyles = false;
            ComisionVendedoresDgvNomina.ColumnHeaderMouseClick += ComisionVendedoresMetOrdenarTabla;
            ComisionVendedoresDgvNomina.CellFormatting += ComisionVendedoresMetFormatearImporteNomina;
            ComisionVendedoresDgvNomina.DataBindingComplete += (Sender, Argumentos) =>
            {
                string[] Encabezados = { "IdAbono|ID abono", "IdFactura|Factura", "FechaAbono|Fecha del abono",
                    "Vendedor|Vendedor", "SimboloMoneda|Moneda", "TotalComision|Comisión total factura",
                    "MontoPagoCliente|Abono del cliente", "ComisionDevengadaAbono|Comisión de este abono",
                    "EstadoPago|Estado del pago al vendedor" };
                foreach (string Par in Encabezados)
                {
                    string[] Partes = Par.Split('|');
                    if (ComisionVendedoresDgvNomina.Columns.Contains(Partes[0]))
                        ComisionVendedoresDgvNomina.Columns[Partes[0]].HeaderText = Partes[1];
                }
                if (ComisionVendedoresDgvNomina.Columns.Contains("FechaAbono"))
                    ComisionVendedoresDgvNomina.Columns["FechaAbono"].DefaultCellStyle.Format = "dd/MM/yyyy";
            };
            ComisionVendedoresBtnAnteriorNomina = new Button
            {
                Name = "ComisionVendedoresBtnAnteriorNomina", Text = "Anterior",
                Location = new System.Drawing.Point(24, 550), Size = new System.Drawing.Size(100, 28),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left, Enabled = false,
                BackColor = System.Drawing.Color.LightGray, ForeColor = System.Drawing.SystemColors.GrayText,
                FlatStyle = FlatStyle.Flat
            };
            ComisionVendedoresBtnSiguienteNomina = new Button
            {
                Name = "ComisionVendedoresBtnSiguienteNomina", Text = "Siguiente",
                Location = new System.Drawing.Point(1134, 550), Size = new System.Drawing.Size(100, 28),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right, Enabled = false,
                BackColor = System.Drawing.Color.LightGray, ForeColor = System.Drawing.SystemColors.GrayText,
                FlatStyle = FlatStyle.Flat
            };
            ComisionVendedoresBtnAnteriorNomina.FlatAppearance.BorderSize = 0;
            ComisionVendedoresBtnSiguienteNomina.FlatAppearance.BorderSize = 0;
            ComisionVendedoresBtnAnteriorNomina.Click += ComisionVendedoresMetCambiarPaginaNomina;
            ComisionVendedoresBtnSiguienteNomina.Click += ComisionVendedoresMetCambiarPaginaNomina;
            ComisionVendedoresLblPaginaNomina = new Label
            {
                Name = "ComisionVendedoresLblPaginaNomina", AutoSize = false,
                Location = new System.Drawing.Point(486, 554), Size = new System.Drawing.Size(300, 20),
                Anchor = AnchorStyles.Bottom, Text = "Página 0 de 0 · 0 registros",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            ComisionVendedoresTabNomina.Controls.Add(ComisionVendedoresBtnAnteriorNomina);
            ComisionVendedoresTabNomina.Controls.Add(ComisionVendedoresBtnSiguienteNomina);
            ComisionVendedoresTabNomina.Controls.Add(ComisionVendedoresLblPaginaNomina);
            ComisionVendedoresTabNomina.Controls.Add(ComisionVendedoresBtnActualizarNomina);
            ComisionVendedoresTabNomina.Controls.Add(ComisionVendedoresLblNominaEstado);
            ComisionVendedoresTabNomina.Controls.Add(ComisionVendedoresDgvNomina);
            ComisionVendedoresTbcPestanas.TabPages.Add(ComisionVendedoresTabNomina);
        }

        private void ComisionVendedoresMetCargarNominaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresLblNominaEstado.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
                _ComisionesNomina = _ControladorComision.ComisionVendedoresMetObtenerComisionesNomina();
                _PaginaNomina = 0;
                ComisionVendedoresDgvNomina.DataSource = null;
                ComisionVendedoresMetMostrarPaginaNomina();
                ComisionVendedoresLblNominaEstado.Text = _ComisionesNomina.Count == 0
                    ? "Aún no hay abonos de comisión. Asigne un vendedor y registre pagos confirmados del cliente."
                    : _ComisionesNomina.Count + " abono(s) de comisión. Cada fila corresponde a un pago confirmado del cliente.";
            }
            catch (Exception Error)
            {
                ComisionVendedoresLblNominaEstado.ForeColor = System.Drawing.Color.Firebrick;
                ComisionVendedoresLblNominaEstado.Text = "No se pudieron cargar las comisiones: " + ComisionVendedoresMetObtenerDetalleError(Error);
            }
        }

        private void ComisionVendedoresMetCambioSeleccionFactura(object Sender, EventArgs Argumentos)
        {
            if (Sender == ComisionVendedoresCboFactura)
            {
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                _IdFacturaVistaPrevia = 0;
            }
            if (!_CargandoCatalogosFactura && Sender == ComisionVendedoresCboFactura)
            {
                ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (Factura != null && Factura.IdVendedor > 0 &&
                    ComisionVendedoresCboVendedorFactura.Items.Cast<ClsVendedorComisionVista>().Any(Vendedor => Vendedor.IdVendedor == Factura.IdVendedor))
                    ComisionVendedoresCboVendedorFactura.SelectedValue = Factura.IdVendedor;
                else
                    ComisionVendedoresCboVendedorFactura.SelectedIndex = -1;
            }
            ComisionVendedoresMetCalcularFacturaSeleccionada();
        }

        private void ComisionVendedoresMetCalcularFacturaSeleccionada()
        {
            if (_CargandoCatalogosFactura) return;
            if (ComisionVendedoresCboFactura.SelectedValue == null ||
                ComisionVendedoresCboVendedorFactura.SelectedValue == null)
            {
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                ComisionVendedoresDgvDetalleFactura.DataSource = null;
                ComisionVendedoresMetMostrarPaginaFactura();
                ComisionVendedoresLblEstadoFactura.Text = "Seleccione una factura con vendedor asignado o seleccione un vendedor.";
                return;
            }
            ClsFacturaComisionVista FacturaSeleccionada = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
            int IdVendedorSeleccionado = Convert.ToInt32(ComisionVendedoresCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
            if (FacturaSeleccionada != null && FacturaSeleccionada.IdVendedor != IdVendedorSeleccionado)
            {
                // Cambiar el vendedor no debe borrar los renglones ni las tasas
                // seleccionadas para la factura actual.
                if (_IdFacturaVistaPrevia != FacturaSeleccionada.IdFactura || _DetallesFactura.Count == 0)
                    ComisionVendedoresMetCalcularFacturaClick(ComisionVendedoresBtnCalcularFactura, EventArgs.Empty);
                else
                    ComisionVendedoresLblEstadoFactura.Text = "Vendedor seleccionado: " +
                        ComisionVendedoresCboVendedorFactura.Text +
                        ". Revise los métodos por producto y pulse «Asignar vendedor» para guardar.";
                return;
            }
            ComisionVendedoresMetCalcularFacturaClick(ComisionVendedoresBtnCalcularFactura, EventArgs.Empty);
        }

        private List<ClsComisionFacturaDetalle> ComisionVendedoresMetObtenerSeleccionesMetodo()
        {
            ComisionVendedoresDgvDetalleFactura.EndEdit();
            foreach (DataGridViewRow Fila in ComisionVendedoresDgvDetalleFactura.Rows)
            {
                ClsComisionFacturaDetalle Detalle = Fila.DataBoundItem as ClsComisionFacturaDetalle;
                if (Detalle == null) continue;
                object Valor = Fila.Cells["ComisionVendedoresClmMetodoProducto"].Value;
                if (Detalle.ComisionGuardada && String.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                    Valor = Detalle.MetodoComision;
                if (Valor == null || String.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                    throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto en cada renglón de la factura.");
                Detalle.MetodoComision = ComisionVendedoresMetNormalizarMetodo(Convert.ToString(Valor));
                if (String.IsNullOrWhiteSpace(Detalle.MetodoComision))
                    throw new InvalidOperationException("Método no válido en la línea " + Detalle.IdFacturaDetalle + ": «" + Convert.ToString(Valor) + "».");
            }
            List<ClsComisionFacturaDetalle> Selecciones = _DetallesFactura;
            if (Selecciones.Any(Detalle => String.IsNullOrWhiteSpace(Detalle.MetodoComision)))
                throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto para cada producto. Use la paginación para revisar todos.");
            if (Selecciones.Count == 0) throw new InvalidOperationException("La factura no tiene productos cargados.");
            return Selecciones;
        }

        private void ComisionVendedoresMetActualizarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblMantenimientoEstado,
                    ComisionVendedoresCboAlcance, ComisionVendedoresCboElementoPorcentaje, ComisionVendedoresTxtPorcentaje);
                if (ComisionVendedoresCboAlcance.SelectedItem == null)
                {
                    ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado,
                        "Seleccione el tipo de porcentaje.", ComisionVendedoresCboAlcance);
                    throw new ArgumentException("Seleccione el tipo de porcentaje.");
                }
                decimal Porcentaje;
                if (!decimal.TryParse(ComisionVendedoresTxtPorcentaje.Text, NumberStyles.Number,
                    CultureInfo.GetCultureInfo("es-GT"), out Porcentaje))
                {
                    ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado,
                        "Ingrese un porcentaje válido entre 0.00 y 99.00.", ComisionVendedoresTxtPorcentaje);
                    throw new ArgumentException("Ingrese un porcentaje válido entre 0.00 y 99.00.");
                }
                if (Porcentaje < 0m || Porcentaje > 99m)
                {
                    ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado,
                        "El porcentaje debe estar entre 0.00 y 99.00.", ComisionVendedoresTxtPorcentaje);
                    throw new ArgumentOutOfRangeException("Porcentaje", "El esquema permite porcentajes entre 0.00 y 99.00.");
                }
                ClsOpcionPorcentaje Opcion = ComisionVendedoresCboElementoPorcentaje.SelectedItem as ClsOpcionPorcentaje;
                if (Opcion == null)
                {
                    ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado,
                        "Seleccione un elemento específico.", ComisionVendedoresCboElementoPorcentaje);
                    throw new ArgumentException("Seleccione la línea, producto, categoría o marca que desea modificar.");
                }
                EnumMetodoComision Metodo = ComisionVendedoresMetObtenerMetodo(ComisionVendedoresCboAlcance.SelectedItem.ToString());
                string TextoConfirmacion = "Se actualizará el porcentaje de " + Opcion.Etiqueta + " a " +
                    Porcentaje.ToString("0.00", CultureInfo.GetCultureInfo("es-GT")) + "%. ¿Desea continuar?";
                if (ComisionVendedoresMetConfirmar(this, "7008 – ComisionVendedores", TextoConfirmacion, "Sí, actualizar") != DialogResult.Yes)
                    return;
                int Actualizados = _ControladorComision.ComisionVendedoresMetActualizarPorcentaje(Metodo,
                    Opcion.IdClasificacion, Porcentaje);

                _ProductosPorcentaje = _ControladorComision.ComisionVendedoresMetConsultarPorcentajes(Metodo);
                _PaginaPorcentajes = Math.Min(_PaginaPorcentajes, ComisionVendedoresMetObtenerUltimaPagina(_ProductosPorcentaje.Count));
                ComisionVendedoresMetMostrarPaginaPorcentajes();
                ComisionVendedoresLblMantenimientoEstado.Text = Actualizados + " registro(s) de " + Opcion.Etiqueta +
                    " guardado(s) con " + Porcentaje.ToString("0.00") + "%.";
                ComisionVendedoresMetCargarElementosPorcentaje();
                ComisionVendedoresMetMostrarExito(this, "7008 – ComisionVendedores", ComisionVendedoresLblMantenimientoEstado.Text);
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado, Excepcion.Message);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ClsProductoPorcentaje> ComisionVendedoresMetCargarPorcentajes()
        {
            _ProductosPorcentaje = _ControladorComision.ComisionVendedoresMetConsultarPorcentajes(
                ComisionVendedoresMetObtenerMetodo(ComisionVendedoresCboAlcance.SelectedItem.ToString()));
            _PaginaPorcentajes = 0;
            ComisionVendedoresMetMostrarPaginaPorcentajes();
            ComisionVendedoresMetCargarElementosPorcentaje();
            return _ProductosPorcentaje;
        }

        private void ComisionVendedoresMetCambiarAlcancePorcentaje(object Sender, EventArgs Argumentos)
        {
            if (_CargandoPorcentajes || _ProductosPorcentaje == null) return;
            ComisionVendedoresCboElementoPorcentaje.DataSource = null;
            ComisionVendedoresMetCargarElementosPorcentaje();
        }

        private void ComisionVendedoresMetCargarElementosPorcentaje()
        {
            if (ComisionVendedoresCboElementoPorcentaje == null || ComisionVendedoresCboAlcance.SelectedItem == null) return;
            EnumMetodoComision Metodo = ComisionVendedoresMetObtenerMetodo(ComisionVendedoresCboAlcance.SelectedItem.ToString());
            IEnumerable<ClsOpcionPorcentaje> Opciones;
            switch (Metodo)
            {
                case EnumMetodoComision.Producto:
                    Opciones = _ProductosPorcentaje.Select(Producto => new ClsOpcionPorcentaje
                    { IdClasificacion = Producto.IdProducto, Etiqueta = Producto.NombreProducto });
                    break;
                case EnumMetodoComision.Marca:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdMarca > 0).GroupBy(Producto => Producto.IdMarca)
                        .Select(Grupo => new ClsOpcionPorcentaje { IdClasificacion = Grupo.Key,
                            Etiqueta = Grupo.First().Marca + " · " + Grupo.Count() + " producto(s)" });
                    break;
                case EnumMetodoComision.Linea:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdLinea > 0).GroupBy(Producto => Producto.IdLinea)
                        .Select(Grupo => new ClsOpcionPorcentaje { IdClasificacion = Grupo.Key,
                            Etiqueta = Grupo.First().Linea + " · " + Grupo.Count() + " producto(s)" });
                    break;
                case EnumMetodoComision.Categoria:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdCategoria > 0).GroupBy(Producto => Producto.IdCategoria)
                        .Select(Grupo => new ClsOpcionPorcentaje { IdClasificacion = Grupo.Key,
                            Etiqueta = Grupo.First().Categoria + " · " + Grupo.Count() + " producto(s)" });
                    break;
                default: Opciones = Enumerable.Empty<ClsOpcionPorcentaje>(); break;
            }
            _CargandoPorcentajes = true;
            try
            {
                int IdSeleccionado = (ComisionVendedoresCboElementoPorcentaje.SelectedItem as ClsOpcionPorcentaje)?.IdClasificacion ?? 0;
                List<ClsOpcionPorcentaje> Lista = Opciones.OrderBy(Opcion => Opcion.Etiqueta).ToList();
                ComisionVendedoresCboElementoPorcentaje.DisplayMember = "Etiqueta";
                ComisionVendedoresCboElementoPorcentaje.ValueMember = "IdClasificacion";
                ComisionVendedoresCboElementoPorcentaje.DataSource = Lista;
                ComisionVendedoresCboElementoPorcentaje.SelectedIndex = Lista.FindIndex(Opcion => Opcion.IdClasificacion == IdSeleccionado);
            }
            finally { _CargandoPorcentajes = false; }
        }

        private void ComisionVendedoresMetMostrarPaginaResultados()
        {
            int UltimaPagina = ComisionVendedoresMetObtenerUltimaPagina(_Resultados.Count, _FilasResultadosPorPagina);
            _PaginaResultados = Math.Max(0, Math.Min(_PaginaResultados, UltimaPagina));
            ComisionVendedoresDgvResultados.DataSource = _Resultados.Skip(_PaginaResultados * _FilasResultadosPorPagina).Take(_FilasResultadosPorPagina).ToList();
            ComisionVendedoresLblPaginaResultados.Text = ComisionVendedoresMetObtenerTextoPagina(_PaginaResultados, _Resultados.Count, _FilasResultadosPorPagina);
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnAnteriorResultados, _PaginaResultados > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnSiguienteResultados,
                _PaginaResultados < UltimaPagina, System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetAplicarIndicadorOrden(ComisionVendedoresDgvResultados);
        }

        private void ComisionVendedoresMetMarcarColumnaObligatoria(object Sender, DataGridViewCellPaintingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex != -1 || Argumentos.ColumnIndex < 0 ||
                ComisionVendedoresDgvDetalleFactura.Columns[Argumentos.ColumnIndex].Name != "ComisionVendedoresClmMetodoProducto")
                return;

            Argumentos.Paint(Argumentos.ClipBounds, DataGridViewPaintParts.All);
            string TextoEncabezado = Convert.ToString(Argumentos.FormattedValue);
            System.Drawing.Size TamanoTexto = TextRenderer.MeasureText(TextoEncabezado, Argumentos.CellStyle.Font);
            int X = Argumentos.CellBounds.Left + Argumentos.CellStyle.Padding.Left + TamanoTexto.Width;
            int Y = Argumentos.CellBounds.Top + (Argumentos.CellBounds.Height - TamanoTexto.Height) / 2;
            using (System.Drawing.Font FuenteAsterisco = new System.Drawing.Font(Argumentos.CellStyle.Font, System.Drawing.FontStyle.Bold))
            {
                TextRenderer.DrawText(Argumentos.Graphics, "*", FuenteAsterisco,
                    new System.Drawing.Point(X, Y), System.Drawing.Color.FromArgb(255, 124, 124), TextFormatFlags.NoPadding);
            }
            Argumentos.Handled = true;
        }
        private void ComisionVendedoresMetFormatearMoneda(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            DataGridView Tabla = Sender as DataGridView;
            if (Tabla == null || Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 || Argumentos.Value == null)
                return;

            string Propiedad = Tabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (Propiedad != "BaseVentas" && Propiedad != "MontoComision" &&
                Propiedad != "BaseVenta" && Propiedad != "ComisionTotal" &&
                Propiedad != "VentaCobrado" && Propiedad != "ComisionUltimoAbono" &&
                Propiedad != "ComisionDisponible" && Propiedad != "ComisionPendiente")
                return;

            string SimboloMoneda = String.Empty;
            if (Tabla == ComisionVendedoresDgvResultados)
            {
                ClsComisionVendedorResultado Resultado = Tabla.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionVendedorResultado;
                if (Resultado != null) SimboloMoneda = Resultado.SimboloMoneda;
            }
            else
            {
                ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (Factura != null) SimboloMoneda = Factura.SimboloMoneda;
            }

            Argumentos.Value = (SimboloMoneda ?? String.Empty) + " " +
                Convert.ToDecimal(Argumentos.Value).ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            Argumentos.FormattingApplied = true;
        }

        private void ComisionVendedoresMetFormatearImporteNomina(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 || Argumentos.Value == null) return;
            string Propiedad = ComisionVendedoresDgvNomina.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (Propiedad != "TotalComision" && Propiedad != "MontoPagoCliente" && Propiedad != "ComisionDevengadaAbono") return;
            ClsComisionNominaVista Comision = ComisionVendedoresDgvNomina.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionNominaVista;
            if (Comision == null) return;
            Argumentos.Value = (Comision.SimboloMoneda ?? String.Empty) + " " +
                Convert.ToDecimal(Argumentos.Value).ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            Argumentos.FormattingApplied = true;
        }

        private void ComisionVendedoresMetOrdenarTabla(object Sender, DataGridViewCellMouseEventArgs Argumentos)
        {
            DataGridView Tabla = Sender as DataGridView;
            if (Tabla == null || Argumentos.ColumnIndex < 0 || Tabla.Columns[Argumentos.ColumnIndex] is DataGridViewComboBoxColumn) return;
            string Propiedad = Tabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (String.IsNullOrWhiteSpace(Propiedad)) return;

            bool Ascendente = !_ColumnasOrdenadas.ContainsKey(Tabla) || _ColumnasOrdenadas[Tabla] != Propiedad ||
                !_OrdenAscendente.ContainsKey(Tabla) || !_OrdenAscendente[Tabla];
            _ColumnasOrdenadas[Tabla] = Propiedad;
            _OrdenAscendente[Tabla] = Ascendente;
            foreach (DataGridViewColumn Columna in Tabla.Columns)
            {
                if (Columna is DataGridViewComboBoxColumn) continue;
                Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
                Columna.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            Tabla.Columns[Argumentos.ColumnIndex].HeaderCell.SortGlyphDirection = Ascendente ? SortOrder.Ascending : SortOrder.Descending;

            if (Tabla == ComisionVendedoresDgvResultados)
                ComisionVendedoresMetOrdenarLista(_Resultados, Propiedad, Ascendente);
            else if (Tabla == ComisionVendedoresDgvDetalleFactura)
                ComisionVendedoresMetOrdenarLista(_DetallesFactura, Propiedad, Ascendente);
            else if (Tabla == ComisionVendedoresDgvPorcentajes)
                ComisionVendedoresMetOrdenarLista(_ProductosPorcentaje, Propiedad, Ascendente);
            else if (Tabla == ComisionVendedoresDgvNomina && _ComisionesNomina != null)
                ComisionVendedoresMetOrdenarLista(_ComisionesNomina, Propiedad, Ascendente);

            if (Tabla == ComisionVendedoresDgvResultados) ComisionVendedoresMetMostrarPaginaResultados();
            else if (Tabla == ComisionVendedoresDgvDetalleFactura) ComisionVendedoresMetMostrarPaginaFactura();
            else if (Tabla == ComisionVendedoresDgvPorcentajes) ComisionVendedoresMetMostrarPaginaPorcentajes();
            else if (Tabla == ComisionVendedoresDgvNomina)
            {
                ComisionVendedoresMetMostrarPaginaNomina();
                ComisionVendedoresMetAplicarIndicadorOrden(ComisionVendedoresDgvNomina);
            }
        }

        private static void ComisionVendedoresMetOrdenarLista<T>(List<T> Registros, string NombrePropiedad, bool Ascendente)
        {
            if (Registros == null || Registros.Count < 2) return;
            System.Reflection.PropertyInfo Propiedad = typeof(T).GetProperty(NombrePropiedad);
            if (Propiedad == null) return;
            Registros.Sort((Primero, Segundo) =>
            {
                object ValorPrimero = Propiedad.GetValue(Primero, null);
                object ValorSegundo = Propiedad.GetValue(Segundo, null);
                int Comparacion;
                if (ValorPrimero == null) Comparacion = ValorSegundo == null ? 0 : -1;
                else if (ValorSegundo == null) Comparacion = 1;
                else if (ValorPrimero is IComparable) Comparacion = ((IComparable)ValorPrimero).CompareTo(ValorSegundo);
                else Comparacion = String.Compare(Convert.ToString(ValorPrimero), Convert.ToString(ValorSegundo), StringComparison.CurrentCultureIgnoreCase);
                return Ascendente ? Comparacion : -Comparacion;
            });
        }

        private void ComisionVendedoresMetAplicarIndicadorOrden(DataGridView Tabla)
        {
            string Propiedad;
            bool Ascendente;
            if (!_ColumnasOrdenadas.TryGetValue(Tabla, out Propiedad) || !_OrdenAscendente.TryGetValue(Tabla, out Ascendente)) return;
            foreach (DataGridViewColumn Columna in Tabla.Columns)
            {
                if (Columna is DataGridViewComboBoxColumn) continue;
                Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
                Columna.HeaderCell.SortGlyphDirection = Columna.DataPropertyName == Propiedad
                    ? (Ascendente ? SortOrder.Ascending : SortOrder.Descending) : SortOrder.None;
            }
        }

        private void ComisionVendedoresMetMostrarPaginaFactura()
        {
            ComisionVendedoresDgvDetalleFactura.DataSource = _DetallesFactura.Skip(_PaginaFactura * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            if (ComisionVendedoresDgvDetalleFactura.Columns.Contains("ComisionVendedoresClmMetodoProducto"))
                ComisionVendedoresDgvDetalleFactura.Columns["ComisionVendedoresClmMetodoProducto"].DataPropertyName = "MetodoComision";
            ComisionVendedoresLblPaginaFactura.Text = ComisionVendedoresMetObtenerTextoPagina(_PaginaFactura, _DetallesFactura.Count);
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnAnteriorFactura, _PaginaFactura > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnSiguienteFactura,
                _PaginaFactura < ComisionVendedoresMetObtenerUltimaPagina(_DetallesFactura.Count), System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetAplicarIndicadorOrden(ComisionVendedoresDgvDetalleFactura);
        }

        private void ComisionVendedoresMetCambiarMetodoProducto(object Sender, DataGridViewCellEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 ||
                ComisionVendedoresDgvDetalleFactura.Columns[Argumentos.ColumnIndex].Name != "ComisionVendedoresClmMetodoProducto") return;
            ClsComisionFacturaDetalle Detalle = ComisionVendedoresDgvDetalleFactura.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionFacturaDetalle;
            if (Detalle == null || Detalle.ComisionGuardada) return;
            string Metodo = ComisionVendedoresMetNormalizarMetodo(Convert.ToString(
                ComisionVendedoresDgvDetalleFactura.Rows[Argumentos.RowIndex].Cells[Argumentos.ColumnIndex].Value));
            if (String.IsNullOrWhiteSpace(Metodo)) return;
            decimal Porcentaje = Metodo == "Producto" ? Detalle.PorcentajeProducto : Metodo == "Marca" ? Detalle.PorcentajeMarca : Metodo == "Linea" ? Detalle.PorcentajeLinea : Detalle.PorcentajeCategoria;
            Detalle.MetodoComision = Metodo;
            Detalle.Porcentaje = Porcentaje;
            Detalle.ComisionTotal = Math.Round(Detalle.BaseVenta * Porcentaje / 100m, 4, MidpointRounding.AwayFromZero);
            Detalle.ComisionPendiente = Math.Max(0m, Detalle.ComisionTotal - Detalle.ComisionDisponible);
            decimal TotalComision = _DetallesFactura.Sum(Item => Item.ComisionTotal);
            decimal PorcentajePonderado = _DetallesFactura.Sum(Item => Item.BaseVenta) <= 0m ? 0m : TotalComision / _DetallesFactura.Sum(Item => Item.BaseVenta) * 100m;
            ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
            ComisionVendedoresLblEstadoFactura.Text = "Total comisión: " + PorcentajePonderado.ToString("0.00") + "% · " + (Factura == null ? String.Empty : Factura.SimboloMoneda) + " " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            ComisionVendedoresDgvDetalleFactura.Refresh();
        }

        private void ComisionVendedoresMetAbrirAyuda(object Sender, EventArgs Argumentos)
        {
            string RutaAyuda = null;
            DirectoryInfo CarpetaActual = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (CarpetaActual != null)
            {
                string RutaCentralAyuda = Path.Combine(CarpetaActual.FullName, "ayuda", "modulos", "ventas", "ComisionVendedoresAyuda", "comisionvendedores.chm");
                if (File.Exists(RutaCentralAyuda))
                {
                    RutaAyuda = RutaCentralAyuda;
                    break;
                }
                CarpetaActual = CarpetaActual.Parent;
            }

            if (File.Exists(RutaAyuda))
            {
                Help.ShowHelp(this, RutaAyuda);
                return;
            }
            MessageBox.Show(this, "No se encontró comisionvendedores.chm en ayuda\\modulos\\ventas\\ComisionVendedoresAyuda.",
                "Ayuda ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComisionVendedoresMetFormatearMetodoProducto(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 ||
                ComisionVendedoresDgvDetalleFactura.Columns[Argumentos.ColumnIndex].Name != "ComisionVendedoresClmDetallePorcentaje") return;
            ClsComisionFacturaDetalle Detalle = ComisionVendedoresDgvDetalleFactura.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionFacturaDetalle;
            if (Detalle != null)
            {
                Argumentos.Value = Detalle.Porcentaje.ToString("0.00") + "%";
                Argumentos.FormattingApplied = true;
            }
        }

        private void ComisionVendedoresMetMostrarPaginaPorcentajes()
        {
            ComisionVendedoresDgvPorcentajes.DataSource = _ProductosPorcentaje.Skip(_PaginaPorcentajes * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            ComisionVendedoresLblPaginaPorcentajes.Text = ComisionVendedoresMetObtenerTextoPagina(_PaginaPorcentajes, _ProductosPorcentaje.Count);
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnAnteriorPorcentajes, _PaginaPorcentajes > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnSiguientePorcentajes,
                _PaginaPorcentajes < ComisionVendedoresMetObtenerUltimaPagina(_ProductosPorcentaje.Count), System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetAplicarIndicadorOrden(ComisionVendedoresDgvPorcentajes);
        }

        private int ComisionVendedoresMetObtenerUltimaPagina(int TotalRegistros)
        {
            return Math.Max(0, (int)Math.Ceiling(TotalRegistros / (double)_FilasPorPagina) - 1);
        }

        private int ComisionVendedoresMetObtenerUltimaPagina(int TotalRegistros, int FilasPorPagina)
        {
            return Math.Max(0, (int)Math.Ceiling(TotalRegistros / (double)FilasPorPagina) - 1);
        }

        private string ComisionVendedoresMetObtenerTextoPagina(int Pagina, int TotalRegistros, int FilasPorPagina)
        {
            int TotalPaginas = TotalRegistros == 0 ? 0 : ComisionVendedoresMetObtenerUltimaPagina(TotalRegistros, FilasPorPagina) + 1;
            return "Página " + (TotalPaginas == 0 ? 0 : Pagina + 1) + " de " + TotalPaginas + " · " + TotalRegistros + " registros";
        }

        private void ComisionVendedoresMetEnvolverControl(Control Campo, string NombreContenedor)
        {
            Control Padre = Campo.Parent;
            if (Padre == null || _ContenedoresValidacion.ContainsKey(Campo)) return;

            int Indice = Padre.Controls.GetChildIndex(Campo);
            System.Drawing.Color ColorOriginal = Padre.BackColor;
            Panel Contenedor = new Panel
            {
                Name = NombreContenedor, Location = Campo.Location, Size = Campo.Size,
                Anchor = Campo.Anchor, BackColor = ColorOriginal, Padding = new Padding(2),
                TabStop = false, TabIndex = Campo.TabIndex, Tag = ColorOriginal
            };
            Padre.Controls.Remove(Campo);
            Contenedor.Controls.Add(Campo);
            Campo.Dock = DockStyle.Fill;
            Campo.Margin = Padding.Empty;
            Campo.TabIndex = 0;
            Padre.Controls.Add(Contenedor);
            Padre.Controls.SetChildIndex(Contenedor, Indice);
            _ContenedoresValidacion.Add(Campo, Contenedor);
        }
        private void ComisionVendedoresMetMostrarError(Label EtiquetaEstado, string Mensaje, params Control[] Controles)
        {
            EtiquetaEstado.ForeColor = System.Drawing.Color.Firebrick;
            EtiquetaEstado.Text = Mensaje;
            foreach (Control ControlValidado in Controles)
            {
                ControlValidado.BackColor = System.Drawing.Color.MistyRose;
                Panel Contenedor;
                if (_ContenedoresValidacion.TryGetValue(ControlValidado, out Contenedor))
                    Contenedor.BackColor = System.Drawing.Color.Firebrick;
                _ErrorProveedor.SetError(ControlValidado, Mensaje);
            }
        }

        private string ComisionVendedoresMetObtenerDetalleError(Exception Excepcion)
        {
            List<string> Detalles = new List<string>();
            for (Exception Actual = Excepcion; Actual != null; Actual = Actual.InnerException)
            {
                string Mensaje = Actual.Message;
                Detalles.Add(String.IsNullOrWhiteSpace(Mensaje)
                    ? Actual.GetType().FullName
                    : Actual.GetType().FullName + ": " + Mensaje);
            }
            return String.Join(Environment.NewLine, Detalles);
        }

        private static string ComisionVendedoresMetNormalizarMetodo(string Metodo)
        {
            if (String.IsNullOrWhiteSpace(Metodo)) return String.Empty;
            string Valor = Metodo.Trim().Replace('í', 'i').Replace('Í', 'I')
                .Replace('á', 'a').Replace('Á', 'A').Replace('é', 'e').Replace('É', 'E');
            switch (Valor.ToUpperInvariant())
            {
                case "PRODUCTO": return "Producto";
                case "MARCA": return "Marca";
                case "LINEA": return "Linea";
                case "CATEGORIA": return "Categoria";
                default: return String.Empty;
            }
        }

        private void ComisionVendedoresMetActualizarBoton(Button Boton, bool Habilitado, System.Drawing.Color ColorActivo)
        {
            Boton.Enabled = Habilitado;
            Boton.BackColor = Habilitado ? System.Drawing.Color.FromArgb(59, 29, 74) : System.Drawing.Color.LightGray;
            Boton.ForeColor = Habilitado ? System.Drawing.Color.FromArgb(248, 245, 249) : System.Drawing.SystemColors.GrayText;
        }

        private void ComisionVendedoresMetRestablecerEstado(Label EtiquetaEstado, params Control[] Controles)
        {
            EtiquetaEstado.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            foreach (Control ControlValidado in Controles)
            {
                ControlValidado.BackColor = System.Drawing.SystemColors.Window;
                Panel Contenedor;
                if (_ContenedoresValidacion.TryGetValue(ControlValidado, out Contenedor))
                    Contenedor.BackColor = Contenedor.Tag is System.Drawing.Color ? (System.Drawing.Color)Contenedor.Tag : BackColor;
                _ErrorProveedor.SetError(ControlValidado, String.Empty);
            }
        }

        private string ComisionVendedoresMetObtenerTextoPagina(int Pagina, int TotalRegistros)
        {
            int TotalPaginas = TotalRegistros == 0 ? 0 : ComisionVendedoresMetObtenerUltimaPagina(TotalRegistros) + 1;
            return "Página " + (TotalPaginas == 0 ? 0 : Pagina + 1) + " de " + TotalPaginas + " · " + TotalRegistros + " registros";
        }

        private void ComisionVendedoresMetCambiarPaginaResultados(object Sender, EventArgs Argumentos)
        {
            _PaginaResultados += Sender == ComisionVendedoresBtnAnteriorResultados ? -1 : 1;
            ComisionVendedoresMetMostrarPaginaResultados();
        }

        private void ComisionVendedoresMetCambiarPaginaFactura(object Sender, EventArgs Argumentos)
        {
            _PaginaFactura += Sender == ComisionVendedoresBtnAnteriorFactura ? -1 : 1;
            ComisionVendedoresMetMostrarPaginaFactura();
        }

        private void ComisionVendedoresMetCambiarPaginaPorcentajes(object Sender, EventArgs Argumentos)
        {
            _PaginaPorcentajes += Sender == ComisionVendedoresBtnAnteriorPorcentajes ? -1 : 1;
            ComisionVendedoresMetMostrarPaginaPorcentajes();
        }

        private void ComisionVendedoresMetMostrarPaginaNomina()
        {
            if (_ComisionesNomina == null) _ComisionesNomina = new List<ClsComisionNominaVista>();
            int UltimaPagina = ComisionVendedoresMetObtenerUltimaPagina(_ComisionesNomina.Count, _FilasNominaPorPagina);
            _PaginaNomina = Math.Max(0, Math.Min(_PaginaNomina, UltimaPagina));
            ComisionVendedoresDgvNomina.DataSource = _ComisionesNomina
                .Skip(_PaginaNomina * _FilasNominaPorPagina).Take(_FilasNominaPorPagina).ToList();
            ComisionVendedoresLblPaginaNomina.Text = ComisionVendedoresMetObtenerTextoPagina(
                _PaginaNomina, _ComisionesNomina.Count, _FilasNominaPorPagina);
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnAnteriorNomina, _PaginaNomina > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnSiguienteNomina,
                _PaginaNomina < UltimaPagina, System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetAplicarIndicadorOrden(ComisionVendedoresDgvNomina);
        }

        private void ComisionVendedoresMetCambiarPaginaNomina(object Sender, EventArgs Argumentos)
        {
            _PaginaNomina += Sender == ComisionVendedoresBtnAnteriorNomina ? -1 : 1;
            ComisionVendedoresMetMostrarPaginaNomina();
        }

        private EnumMetodoComision ComisionVendedoresMetObtenerMetodo(string Texto)
        {
            switch (Texto)
            {
                case "Vendedor": return EnumMetodoComision.Vendedor;
                case "Marca": return EnumMetodoComision.Marca;
                case "Línea": return EnumMetodoComision.Linea;
                case "Producto": return EnumMetodoComision.Producto;
                case "Categoría": return EnumMetodoComision.Categoria;
                default: throw new ArgumentException("Seleccione un método de comisión.");
            }
        }
    }
}
