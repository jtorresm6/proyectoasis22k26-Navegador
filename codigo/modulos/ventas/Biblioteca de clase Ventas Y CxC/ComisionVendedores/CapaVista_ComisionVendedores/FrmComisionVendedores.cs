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
        private ComboBox VentasCboElementoPorcentaje;
        private bool _CargandoPorcentajes;
        private TabPage VentasTbpNomina;
        private DataGridView VentasDgvNomina;
        private Label VentasLblNominaEstado;
        private List<ClsComisionNominaVista> _ComisionesNomina;
        private Button VentasBtnAnteriorNomina;
        private Button VentasBtnSiguienteNomina;
        private Button VentasBtnActualizarNomina;
        private Label VentasLblPaginaNomina;
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
            VentasMetEnvolverControl(VentasDtpFechaInicio, "VentasPnlFechaInicio");
            VentasMetEnvolverControl(VentasDtpFechaFin, "VentasPnlFechaFin");
            VentasMetEnvolverControl(VentasCboFactura, "VentasPnlFactura");
            VentasMetEnvolverControl(VentasCboVendedorFactura, "VentasPnlVendedor");
            VentasMetEnvolverControl(VentasCboAlcance, "VentasPnlAlcance");
            System.Drawing.Font FuenteCampo = new System.Drawing.Font("Segoe UI", 10F);
            VentasCboFactura.Font = FuenteCampo;
            VentasCboVendedorFactura.Font = FuenteCampo;
            VentasCboMetodoFactura.Font = FuenteCampo;
            VentasCboAlcance.Font = FuenteCampo;
            VentasTxtPorcentaje.Font = FuenteCampo;
            VentasDgvDetalleFactura.DefaultCellStyle.Font = FuenteCampo;
            VentasClmMetodoProducto.DefaultCellStyle.Font = FuenteCampo;
            _ErrorProveedor.ContainerControl = this;
            _ErrorProveedor.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            VentasMetCrearPestanaNomina();
            VentasBtnAyuda.Click += VentasMetAbrirAyuda;
            VentasCboMetodoFactura.Visible = false;
            VentasLblMetodoFactura.Visible = false;
            _ControladorComision = ControladorComision ?? throw new ArgumentNullException("ControladorComision");
            _Resultados = new List<ClsComisionVendedorResultado>();
            _DetallesFactura = new List<ClsComisionFacturaDetalle>();
            _ProductosPorcentaje = new List<ClsProductoPorcentaje>();
            VentasTbcPestanas.SelectedIndexChanged += VentasMetCambiarPestana;
            VentasTbpAsignarFactura.Enter += VentasMetCambiarPestana;
            Load += (Sender, Argumentos) =>
            {
                if (VentasTbcPestanas.SelectedTab == VentasTbpAsignarFactura)
                    VentasMetCargarCatalogosClick(Sender, Argumentos);
            };
            VentasCboFactura.SelectedValueChanged += VentasMetCambioSeleccionFactura;
            VentasCboVendedorFactura.SelectedValueChanged += VentasMetCambioSeleccionFactura;
            VentasDgvResultados.CellFormatting += VentasMetFormatearMoneda;
            VentasDgvDetalleFactura.CellFormatting += VentasMetFormatearMoneda;
            VentasLblEstado.AutoSize = false;
            VentasLblEstado.Size = new System.Drawing.Size(1210, 28);
            VentasLblEstado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            VentasLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            VentasLblEstadoFactura.AutoSize = false;
            VentasLblEstadoFactura.Size = new System.Drawing.Size(1210, 40);
            VentasLblEstadoFactura.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            VentasLblEstadoFactura.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            VentasDgvResultados.ColumnHeaderMouseClick += VentasMetOrdenarTabla;
            VentasDgvDetalleFactura.ColumnHeaderMouseClick += VentasMetOrdenarTabla;
            VentasDgvDetalleFactura.CellPainting += VentasMetMarcarColumnaObligatoria;
            VentasDgvPorcentajes.ColumnHeaderMouseClick += VentasMetOrdenarTabla;
            VentasDgvDetalleFactura.CurrentCellDirtyStateChanged += (Sender, Argumentos) =>
            {
                if (VentasDgvDetalleFactura.IsCurrentCellDirty)
                    VentasDgvDetalleFactura.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            VentasDgvDetalleFactura.CellValueChanged += VentasMetCambiarMetodoProducto;
            VentasCboAlcance.SelectedIndexChanged += VentasMetCambiarAlcancePorcentaje;
            VentasLblAlcance.Text = "Tipo de porcentaje";
            VentasLblAlcance.Location = new System.Drawing.Point(24, 7);
            VentasCboAlcance.Location = new System.Drawing.Point(24, 25);
            VentasCboAlcance.Size = new System.Drawing.Size(150, 31);
            VentasLblPorcentajeNuevo.AutoSize = false;
            VentasLblPorcentajeNuevo.Text = "Porcentaje nuevo";
            VentasLblPorcentajeNuevo.Location = new System.Drawing.Point(420, 7);
            VentasLblPorcentajeNuevo.Size = new System.Drawing.Size(150, 23);
            VentasTxtPorcentaje.Location = new System.Drawing.Point(420, 25);
            VentasTxtPorcentaje.Size = new System.Drawing.Size(110, 30);
            VentasMetEnvolverControl(VentasTxtPorcentaje, "VentasPnlPorcentaje");
            VentasBtnBuscar.Location = new System.Drawing.Point(568, 23);
            VentasBtnBuscar.Size = new System.Drawing.Size(100, 30);
            VentasBtnActualizar.Location = new System.Drawing.Point(670, 23);
            VentasBtnActualizar.Size = new System.Drawing.Size(190, 30);
            VentasLblMantenimientoEstado.AutoSize = false;
            VentasLblMantenimientoEstado.Location = new System.Drawing.Point(24, 68);
            VentasLblMantenimientoEstado.Size = new System.Drawing.Size(1210, 28);
            Label EtiquetaElementoPorcentaje = new Label
            {
                Name = "VentasLblElementoPorcentaje",
                Text = "Elemento específico", Location = new System.Drawing.Point(190, 7),
                Size = new System.Drawing.Size(220, 23), ForeColor = System.Drawing.Color.FromArgb(59, 29, 74)
            };
            VentasTbpMantenimiento.Controls.Add(EtiquetaElementoPorcentaje);
            Label AsteriscoElementoPorcentaje = new Label
            {
                Name = "VentasLblObligatorioElementoPorcentaje", Text = "*",
                Location = new System.Drawing.Point(EtiquetaElementoPorcentaje.Left + System.Windows.Forms.TextRenderer.MeasureText(EtiquetaElementoPorcentaje.Text, EtiquetaElementoPorcentaje.Font).Width + 2, 7), Size = new System.Drawing.Size(12, 18),
                Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Firebrick
            };
            VentasTbpMantenimiento.Controls.Add(AsteriscoElementoPorcentaje);
            VentasCboElementoPorcentaje = new ComboBox
            {
                Name = "VentasCboElementoPorcentaje",
                DropDownStyle = ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(190, 25),
                Size = new System.Drawing.Size(220, 31), Font = VentasCboAlcance.Font
            };
            VentasTbpMantenimiento.Controls.Add(VentasCboElementoPorcentaje);
            VentasMetEnvolverControl(VentasCboElementoPorcentaje, "VentasPnlElementoPorcentaje");
            VentasLblObligatorioAlcance.Text = "*";
            VentasLblObligatorioAlcance.Visible = true;
            VentasLblObligatorioAlcance.Location = new System.Drawing.Point(VentasLblAlcance.Left + System.Windows.Forms.TextRenderer.MeasureText(VentasLblAlcance.Text, VentasLblAlcance.Font).Width + 2, 7);
            VentasLblObligatorioPorcentaje.Text = "*";
            VentasLblObligatorioPorcentaje.Visible = true;
            VentasLblObligatorioPorcentaje.Location = new System.Drawing.Point(VentasLblPorcentajeNuevo.Left + System.Windows.Forms.TextRenderer.MeasureText(VentasLblPorcentajeNuevo.Text, VentasLblPorcentajeNuevo.Font).Width + 2, 7);
            VentasLblObligatorioAlcance.BringToFront();
            AsteriscoElementoPorcentaje.BringToFront();
            VentasLblObligatorioPorcentaje.BringToFront();
            VentasCboElementoPorcentaje.AccessibleDescription = "Campo obligatorio";
            VentasLblObligatorioFactura.Text = "*";
            VentasLblObligatorioFactura.Visible = true;
            VentasLblObligatorioVendedor.Text = "*";
            VentasLblObligatorioVendedor.Visible = true;
            VentasCboAlcance.AccessibleDescription = "Campo obligatorio";
            VentasTxtPorcentaje.AccessibleDescription = "Campo obligatorio";
            VentasCboFactura.AccessibleDescription = "Campo obligatorio";
            VentasCboVendedorFactura.AccessibleDescription = "Campo obligatorio";
            VentasDtpFechaInicio.AccessibleDescription = "Campo obligatorio";
            VentasDtpFechaFin.AccessibleDescription = "Campo obligatorio";
            VentasMetEstandarizarBotones();
        }

        private void VentasMetEstandarizarBotones()
        {
            VentasMetAplicarBotonEstandar(VentasBtnAyuda, "btn_ayuda", "Ayuda", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnCalcular, "btn_aplicarFiltro", "Calcular", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnCalcularFactura, "btn_consultar", "Calcular comisión", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnAsignarVendedorFactura, "btn_enlace", "Asignar vendedor", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnRegistrarAbonoFactura, "btn_ingresar", "Realizar pago", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnBuscar, "btn_buscar", "Buscar", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnActualizar, "btn_modificar", "Actualizar", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnAnteriorResultados, "btn_anterior", "Anterior", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnSiguienteResultados, "btn_siguiente", "Siguiente", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnAnteriorFactura, "btn_anterior", "Anterior", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnSiguienteFactura, "btn_siguiente", "Siguiente", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnAnteriorPorcentajes, "btn_anterior", "Anterior", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnSiguientePorcentajes, "btn_siguiente", "Siguiente", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnActualizarNomina, "btn_refrescar", "Actualizar", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnAnteriorNomina, "btn_anterior", "Anterior", new System.Drawing.Size(76, 78));
            VentasMetAplicarBotonEstandar(VentasBtnSiguienteNomina, "btn_siguiente", "Siguiente", new System.Drawing.Size(76, 78));

            VentasBtnCalcular.Location = new System.Drawing.Point(572, 5);
            VentasBtnAyuda.Location = new System.Drawing.Point(664, 5);
            VentasLblEstado.Location = new System.Drawing.Point(24, 100);
            VentasDgvResultados.Location = new System.Drawing.Point(24, 130);
            VentasDgvResultados.Size = new System.Drawing.Size(1210, 368);

            VentasBtnCalcularFactura.Location = new System.Drawing.Point(24, 80);
            VentasBtnAsignarVendedorFactura.Location = new System.Drawing.Point(116, 80);
            VentasBtnRegistrarAbonoFactura.Location = new System.Drawing.Point(208, 80);
            VentasLblEstadoFactura.Location = new System.Drawing.Point(24, 174);
            VentasDgvDetalleFactura.Location = new System.Drawing.Point(24, 208);
            VentasDgvDetalleFactura.Size = new System.Drawing.Size(1210, 290);

            VentasBtnBuscar.Location = new System.Drawing.Point(570, 4);
            VentasBtnActualizar.Location = new System.Drawing.Point(664, 4);
            VentasLblMantenimientoEstado.Location = new System.Drawing.Point(24, 100);
            VentasDgvPorcentajes.Location = new System.Drawing.Point(24, 130);
            VentasDgvPorcentajes.Size = new System.Drawing.Size(1210, 368);

            Button[] BotonesAnteriores = { VentasBtnAnteriorResultados, VentasBtnAnteriorFactura,
                VentasBtnAnteriorPorcentajes, VentasBtnAnteriorNomina };
            Button[] BotonesSiguientes = { VentasBtnSiguienteResultados, VentasBtnSiguienteFactura,
                VentasBtnSiguientePorcentajes, VentasBtnSiguienteNomina };
            foreach (Button Boton in BotonesAnteriores) Boton.Location = new System.Drawing.Point(24, 496);
            foreach (Button Boton in BotonesSiguientes) Boton.Location = new System.Drawing.Point(1184, 496);
            VentasLblPaginaResultados.Location = new System.Drawing.Point(486, 532);
            VentasLblPaginaFactura.Location = new System.Drawing.Point(486, 532);
            VentasLblPaginaPorcentajes.Location = new System.Drawing.Point(486, 532);
            VentasDgvNomina.Location = new System.Drawing.Point(24, 104);
            VentasDgvNomina.Size = new System.Drawing.Size(1210, 394);
            VentasLblPaginaNomina.Location = new System.Drawing.Point(486, 532);
            VentasBtnActualizarNomina.Location = new System.Drawing.Point(1166, 8);
        }
        internal static void VentasMetAplicarBotonEstandar(Button Boton, string Icono, string Etiqueta, System.Drawing.Size Tamano)
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

        private void VentasMetCalcularClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstado,
                    VentasDtpFechaInicio, VentasDtpFechaFin);
                if (VentasDtpFechaInicio.Value.Date > VentasDtpFechaFin.Value.Date)
                    throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");
                _Resultados = _ControladorComision.VentasMetCalcular(EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado,
                    VentasDtpFechaInicio.Value.Date, VentasDtpFechaFin.Value.Date);
                _PaginaResultados = 0;
                VentasMetMostrarPaginaResultados();
                VentasLblEstado.Text = _Resultados.Count == 0
                    ? "No hay comisiones asignadas ni pagos de comisión en el rango seleccionado."
                    : "Cálculo completado. Se muestran los movimientos de comisión devengados en el período.";
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblEstado, Excepcion.Message,
                    VentasDtpFechaInicio, VentasDtpFechaFin);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetRegistrarAbonoFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                int IdUsuario = VentasMetObtenerIdUsuarioSesion();
                ClsFacturaComisionVista FacturaSeleccionada = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                using (FrmPagoFacturaTemporal Formulario = new FrmPagoFacturaTemporal(_ControladorComision, IdUsuario,
                    FacturaSeleccionada == null ? (int?)null : FacturaSeleccionada.IdFactura))
                {
                    if (Formulario.ShowDialog(this) == DialogResult.OK)
                    {
                        VentasLblEstadoFactura.Text =
                            "Abono de cliente registrado. La comisión proporcional queda devengada y aparecerá al calcular comisiones.";
                        VentasMetCargarCatalogosClick(Sender, Argumentos);
                    }
                }
            }
            catch (Exception Excepcion)
            {
                VentasLblEstadoFactura.Text = Excepcion.Message;
                MessageBox.Show(this, Excepcion.Message, "Registrar abono de factura",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int VentasMetObtenerIdUsuarioSesion()
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

        private void VentasMetBuscarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblMantenimientoEstado,
                    VentasCboAlcance, VentasCboElementoPorcentaje, VentasTxtPorcentaje);
                if (VentasCboAlcance.SelectedItem == null)
                    throw new ArgumentException("Seleccione el tipo de porcentaje.");
                List<ClsProductoPorcentaje> Productos = VentasMetCargarPorcentajes();
                VentasLblMantenimientoEstado.Text = Productos.Count + " producto(s) cargados. Elija el elemento específico que desea modificar.";
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblMantenimientoEstado, Excepcion.Message, VentasCboAlcance);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetCargarCatalogosClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                _CargandoCatalogosFactura = true;
                VentasMetRestablecerEstado(VentasLblEstadoFactura,
                    VentasCboFactura, VentasCboVendedorFactura);
                VentasCboFactura.DataSource = null;
                VentasCboVendedorFactura.DataSource = null;
                List<ClsFacturaComisionVista> Facturas = _ControladorComision.VentasMetObtenerFacturas();
                List<ClsVendedorComisionVista> Vendedores = _ControladorComision.VentasMetObtenerVendedores();
                // Usa un campo simple del DTO para asegurar que WinForms muestre el elemento
                // aunque falle el enlace de la propiedad calculada Etiqueta.
                VentasCboFactura.DisplayMember = "IdFactura";
                VentasCboFactura.ValueMember = "IdFactura";
                VentasCboFactura.DataSource = Facturas;
                if (Facturas.Count > 0 && VentasCboFactura.SelectedIndex < 0)
                    VentasCboFactura.SelectedIndex = 0;
                VentasCboVendedorFactura.DisplayMember = "NombreVendedor";
                VentasCboVendedorFactura.ValueMember = "IdVendedor";
                VentasCboVendedorFactura.DataSource = Vendedores;
                ClsFacturaComisionVista FacturaSeleccionada = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (FacturaSeleccionada != null && FacturaSeleccionada.IdVendedor > 0 &&
                    VentasCboVendedorFactura.Items.Cast<ClsVendedorComisionVista>().Any(Vendedor => Vendedor.IdVendedor == FacturaSeleccionada.IdVendedor))
                    VentasCboVendedorFactura.SelectedValue = FacturaSeleccionada.IdVendedor;
                else
                    VentasCboVendedorFactura.SelectedIndex = -1;
                VentasLblEstadoFactura.Text = Facturas.Count == 0
                    ? "No hay facturas pendientes de asignación (puede que no existan facturas emitidas nuevas o que ya tengan comisión registrada)."
                    : Facturas.Count + " factura(s) y " + Vendedores.Count + " vendedor(es) cargados. Seleccione una factura.";
                _CargandoCatalogosFactura = false;
                if (Facturas.Count == 0)
                {
                    VentasDgvDetalleFactura.DataSource = null;
                    _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                    return;
                }
                VentasMetCalcularFacturaSeleccionada();
            }
            catch (Exception Excepcion)
            {
                _CargandoCatalogosFactura = false;
                VentasMetMostrarError(VentasLblEstadoFactura, Excepcion.Message,
                    VentasCboFactura, VentasCboVendedorFactura);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetCalcularFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstadoFactura,
                    VentasCboFactura, VentasCboVendedorFactura);
                if (VentasCboFactura.SelectedValue == null || VentasCboVendedorFactura.SelectedValue == null)
                {
                    VentasMetMostrarError(VentasLblEstadoFactura,
                        "Seleccione una factura y un vendedor para continuar.", VentasCboFactura, VentasCboVendedorFactura);
                    VentasLblEstadoFactura.ForeColor = System.Drawing.Color.Firebrick;
                    VentasLblEstadoFactura.Text = "No se puede calcular: seleccione una factura y un vendedor. Si el combo de factura está vacío, revise el error de carga del catálogo.";
                    return;
                }

                int IdFactura = Convert.ToInt32(VentasCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(VentasCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                EnumMetodoComision Metodo = EnumMetodoComision.Producto;
                List<ClsComisionFacturaDetalle> DetalleFacturaCalculado;
                if (_DetallesFactura.Count > 0 && _IdFacturaVistaPrevia == IdFactura)
                {
                    List<ClsComisionFacturaDetalle> Selecciones = VentasMetObtenerSeleccionesMetodo();
                    DetalleFacturaCalculado = _ControladorComision.VentasMetCalcularFactura(IdFactura, IdVendedor, Selecciones);
                }
                else
                    DetalleFacturaCalculado = _ControladorComision.VentasMetCalcularFactura(IdFactura, IdVendedor, Metodo);
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                string SimboloMoneda = Factura == null ? String.Empty : Factura.SimboloMoneda;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    // La columna ComboBox compara valores exactos. Las tasas históricas
                    // pueden venir como PRODUCTO/LINEA en mayúscula desde MySQL.
                    Detalle.MetodoComision = VentasMetNormalizarMetodo(Detalle.MetodoComision);
                    Detalle.SimboloMoneda = SimboloMoneda;
                }
                VentasDgvDetalleFactura.DataSource = null;
                _DetallesFactura = DetalleFacturaCalculado;
                _IdFacturaVistaPrevia = IdFactura;
                _PaginaFactura = 0;
                VentasMetMostrarPaginaFactura();

                decimal TotalComision = 0m, Disponible = 0m, Pendiente = 0m, ComisionUltimoAbono = 0m, TotalBase = 0m;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    TotalBase += Detalle.BaseVenta;
                    TotalComision += Detalle.ComisionTotal;
                    Disponible += Detalle.ComisionDisponible;
                    Pendiente += Detalle.ComisionPendiente;
                    ComisionUltimoAbono += Detalle.ComisionUltimoAbono;
                }
                ClsVendedorComisionVista Vendedor = VentasCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal PagadoFactura = Factura == null ? 0m : Factura.TotalPagado;
                decimal PorcentajeTotal = TotalBase <= 0m ? 0m : TotalComision / TotalBase * 100m;
                VentasLblEstadoFactura.Text = "Factura " + IdFactura + " · " + (Vendedor == null ? "" : Vendedor.NombreVendedor) +
                    " · Cobrado " + SimboloMoneda + " " + PagadoFactura.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + Environment.NewLine +
                    "Total comisión: " + PorcentajeTotal.ToString("0.00") + "% · " + SimboloMoneda + " " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Disponible " + SimboloMoneda + " " + Disponible.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Comisión del último abono " + SimboloMoneda + " " + ComisionUltimoAbono.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Pendiente " + SimboloMoneda + " " + Pendiente.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            }
            catch (Exception Excepcion)
            {
                string MensajeError = VentasMetObtenerDetalleError(Excepcion);
                VentasMetMostrarError(VentasLblEstadoFactura, MensajeError,
                    VentasCboFactura, VentasCboVendedorFactura);
                MessageBox.Show(MensajeError, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetAsignarVendedorFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstadoFactura,
                    VentasCboFactura, VentasCboVendedorFactura);
                if (VentasCboFactura.SelectedValue == null || VentasCboVendedorFactura.SelectedValue == null)
                {
                    VentasMetMostrarError(VentasLblEstadoFactura,
                        "Seleccione una factura y un vendedor.", VentasCboFactura, VentasCboVendedorFactura);
                    return;
                }
                int IdFactura = Convert.ToInt32(VentasCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(VentasCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                List<ClsComisionFacturaDetalle> Selecciones = VentasMetObtenerSeleccionesMetodo();
                ClsVendedorComisionVista Vendedor = VentasCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal TotalComisionGuardada = Selecciones.Sum(Detalle => Detalle.ComisionTotal);
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                string SimboloMoneda = Factura == null ? String.Empty : Factura.SimboloMoneda;
                string TextoConfirmacion = "Se asignará la factura " + IdFactura + " a " +
                    (Vendedor == null ? "el vendedor seleccionado" : Vendedor.NombreVendedor) + ". Se guardarán " +
                    Selecciones.Count + " líneas de comisión por " + SimboloMoneda + " " +
                    TotalComisionGuardada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ". ¿Desea continuar?";
                if (VentasMetConfirmar(this, "7008 – ComisionVendedores", TextoConfirmacion, "Sí, asignar") != DialogResult.Yes)
                    return;
                int IdComision = _ControladorComision.VentasMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Selecciones);
                string Confirmacion = "Factura " + IdFactura + " asignada a " +
                    (Vendedor == null ? "vendedor" : Vendedor.NombreVendedor) + ". Se guardaron " +
                    Selecciones.Count + " líneas de comisión por un total de " + SimboloMoneda + " " +
                    TotalComisionGuardada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " (registro " + IdComision + "). La factura se quitó de la lista para evitar asignarla otra vez.";
                VentasMetActualizarFacturasDisponiblesDespuesAsignar(IdFactura, Confirmacion);
                VentasMetMostrarExito(this, "7008 – ComisionVendedores", Confirmacion);
            }
            catch (Exception Excepcion)
            {
                string MensajeError = VentasMetObtenerDetalleError(Excepcion);
                VentasMetMostrarError(VentasLblEstadoFactura, MensajeError,
                    VentasCboFactura, VentasCboVendedorFactura);
                MessageBox.Show(MensajeError, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static DialogResult VentasMetConfirmar(IWin32Window VentanaPropietaria, string Titulo, string Mensaje, string TextoAccion)
        {
            return VentasMetMostrarDialogoOperacion(VentanaPropietaria, Titulo, Mensaje, true, TextoAccion);
        }

        internal static void VentasMetMostrarExito(IWin32Window VentanaPropietaria, string Titulo, string Mensaje)
        {
            VentasMetMostrarDialogoOperacion(VentanaPropietaria, Titulo, Mensaje, false, "Aceptar");
        }

        private static DialogResult VentasMetMostrarDialogoOperacion(
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

                Panel VentasPnlEncabezadoDialogo = new Panel
                {
                    Name = "VentasPnlEncabezadoDialogo",
                    BackColor = System.Drawing.Color.FromArgb(59, 29, 74),
                    Location = new System.Drawing.Point(0, 0), Size = new System.Drawing.Size(580, 58)
                };
                Label VentasLblEstadoDialogo = new Label
                {
                    Name = "VentasLblEstadoDialogo",
                    Text = EsConfirmacion ? "!" : "✓",
                    Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold),
                    ForeColor = EsConfirmacion ? System.Drawing.Color.FromArgb(201, 162, 74) : System.Drawing.Color.ForestGreen,
                    Location = new System.Drawing.Point(18, 7), Size = new System.Drawing.Size(40, 44),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };
                Label VentasLblTituloDialogo = new Label
                {
                    Name = "VentasLblTituloDialogo",
                    Text = EsConfirmacion ? "Confirmar operación" : "Operación completada",
                    Font = new System.Drawing.Font("Lucida Sans Unicode", 14F,
                        System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic),
                    ForeColor = System.Drawing.Color.FromArgb(248, 245, 249),
                    Location = new System.Drawing.Point(68, 12), Size = new System.Drawing.Size(490, 34),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                };
                VentasPnlEncabezadoDialogo.Controls.Add(VentasLblEstadoDialogo);
                VentasPnlEncabezadoDialogo.Controls.Add(VentasLblTituloDialogo);
                Dialogo.Controls.Add(VentasPnlEncabezadoDialogo);

                Label VentasLblMensajeDialogo = new Label
                {
                    Name = "VentasLblMensajeDialogo",
                    Text = Mensaje, Font = new System.Drawing.Font("Tahoma", 9F),
                    ForeColor = System.Drawing.Color.FromArgb(59, 29, 74),
                    Location = new System.Drawing.Point(20, 70), Size = new System.Drawing.Size(540, 58),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft, AutoEllipsis = true
                };
                Dialogo.Controls.Add(VentasLblMensajeDialogo);

                Button VentasBtnAceptarDialogo = new Button
                {
                    Name = "VentasBtnAceptarDialogo",
                    Text = TextoAccion, DialogResult = EsConfirmacion ? DialogResult.Yes : DialogResult.OK,
                    Location = new System.Drawing.Point(EsConfirmacion ? 450 : 490, 136), Size = new System.Drawing.Size(76, 78),
                    BackColor = System.Drawing.Color.FromArgb(59, 29, 74),
                    ForeColor = System.Drawing.Color.FromArgb(248, 245, 249), FlatStyle = FlatStyle.Flat
                };
                VentasMetAplicarBotonEstandar(VentasBtnAceptarDialogo,
                    EsConfirmacion ? "btn_guardar" : "btn_salir", EsConfirmacion ? "Confirmar" : "Cerrar", new System.Drawing.Size(76, 78));
                VentasBtnAceptarDialogo.AccessibleDescription = TextoAccion;
                
                Dialogo.Controls.Add(VentasBtnAceptarDialogo);
                Dialogo.AcceptButton = VentasBtnAceptarDialogo;

                if (EsConfirmacion)
                {
                    Button VentasBtnCancelarDialogo = new Button
                    {
                        Name = "VentasBtnCancelarDialogo", Text = "No", DialogResult = DialogResult.No,
                        Location = new System.Drawing.Point(370, 136), Size = new System.Drawing.Size(76, 78),
                        BackColor = System.Drawing.Color.FromArgb(59, 29, 74),
                        ForeColor = System.Drawing.Color.FromArgb(248, 245, 249), FlatStyle = FlatStyle.Flat
                    };
                    VentasMetAplicarBotonEstandar(VentasBtnCancelarDialogo,
                        "btn_cancelar", "No", new System.Drawing.Size(76, 78));
                    
                    Dialogo.Controls.Add(VentasBtnCancelarDialogo);
                    Dialogo.CancelButton = VentasBtnCancelarDialogo;
                }

                return VentanaPropietaria == null ? Dialogo.ShowDialog() : Dialogo.ShowDialog(VentanaPropietaria);
            }
        }
        private void VentasMetActualizarFacturasDisponiblesDespuesAsignar(int IdFactura, string Confirmacion)
        {
            try
            {
                List<ClsFacturaComisionVista> FacturasDisponibles = _ControladorComision.VentasMetObtenerFacturas();
                _CargandoCatalogosFactura = true;
                VentasCboFactura.DataSource = null;
                VentasCboFactura.DisplayMember = "IdFactura";
                VentasCboFactura.ValueMember = "IdFactura";
                VentasCboFactura.DataSource = FacturasDisponibles;
                VentasCboFactura.SelectedIndex = -1;
                VentasCboVendedorFactura.SelectedIndex = -1;
                _CargandoCatalogosFactura = false;
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                _IdFacturaVistaPrevia = 0;
                _PaginaFactura = 0;
                VentasDgvDetalleFactura.DataSource = null;
                VentasMetMostrarPaginaFactura();
                VentasLblEstadoFactura.Text = Confirmacion + Environment.NewLine +
                    (FacturasDisponibles.Count == 0
                        ? "No quedan facturas pendientes de asignación."
                        : "Quedan " + FacturasDisponibles.Count + " factura(s) pendientes de asignación.");
            }
            catch (Exception ErrorActualizacion)
            {
                _CargandoCatalogosFactura = false;
                VentasLblEstadoFactura.Text = Confirmacion + Environment.NewLine +
                    "La asignación se guardó, pero no se pudo actualizar la lista de facturas: " +
                    VentasMetObtenerDetalleError(ErrorActualizacion);
            }
        }

        private void VentasMetCambiarPestana(object Sender, EventArgs Argumentos)
        {
            if (VentasTbcPestanas.SelectedTab == VentasTbpAsignarFactura)
                VentasMetCargarCatalogosClick(Sender, Argumentos);
            else if (VentasTbcPestanas.SelectedTab == VentasTbpMantenimiento)
                VentasMetBuscarClick(Sender, Argumentos);
            else if (VentasTbcPestanas.SelectedTab == VentasTbpNomina)
                VentasMetCargarNominaClick(Sender, Argumentos);
        }

        private void VentasMetCrearPestanaNomina()
        {
            VentasTbpNomina = new TabPage("Comisiones emitidas")
            {
                Name = "VentasTbpNomina",
                BackColor = System.Drawing.Color.FromArgb(248, 245, 249),
                Padding = new Padding(16),
                Size = new System.Drawing.Size(1272, 582)
            };
            VentasBtnActualizarNomina = new Button
            {
                Name = "VentasBtnActualizarNomina", Text = "Actualizar",
                Location = new System.Drawing.Point(24, 18), Size = new System.Drawing.Size(120, 30),
                BackColor = System.Drawing.Color.FromArgb(123, 63, 147), ForeColor = System.Drawing.Color.FromArgb(248, 245, 249),
                FlatStyle = FlatStyle.Flat
            };
            VentasBtnActualizarNomina.FlatAppearance.BorderSize = 0;
            VentasBtnActualizarNomina.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            VentasBtnActualizarNomina.Location = new System.Drawing.Point(1128, 18);
            VentasBtnActualizarNomina.Click += VentasMetCargarNominaClick;
            VentasLblNominaEstado = new Label
            {
                Name = "VentasLblNominaEstado", AutoSize = false,
                Location = new System.Drawing.Point(160, 18), Size = new System.Drawing.Size(930, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ForeColor = System.Drawing.Color.FromArgb(59, 29, 74), TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            VentasDgvNomina = new DataGridView
            {
                Name = "VentasDgvNomina", Location = new System.Drawing.Point(24, 60),
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
            VentasDgvNomina.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            VentasDgvNomina.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            VentasDgvNomina.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            VentasDgvNomina.ColumnHeadersHeight = 54;
            VentasDgvNomina.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            VentasDgvNomina.EnableHeadersVisualStyles = false;
            VentasDgvNomina.ColumnHeaderMouseClick += VentasMetOrdenarTabla;
            VentasDgvNomina.CellFormatting += VentasMetFormatearImporteNomina;
            VentasDgvNomina.DataBindingComplete += (Sender, Argumentos) =>
            {
                string[] Encabezados = { "IdAbono|ID abono", "IdFactura|Factura", "FechaAbono|Fecha del abono",
                    "Vendedor|Vendedor", "SimboloMoneda|Moneda", "TotalComision|Comisión total factura",
                    "MontoPagoCliente|Abono del cliente", "ComisionDevengadaAbono|Comisión de este abono",
                    "EstadoPago|Estado del pago al vendedor" };
                foreach (string Par in Encabezados)
                {
                    string[] Partes = Par.Split('|');
                    if (VentasDgvNomina.Columns.Contains(Partes[0]))
                        VentasDgvNomina.Columns[Partes[0]].HeaderText = Partes[1];
                }
                if (VentasDgvNomina.Columns.Contains("FechaAbono"))
                    VentasDgvNomina.Columns["FechaAbono"].DefaultCellStyle.Format = "dd/MM/yyyy";
            };
            VentasBtnAnteriorNomina = new Button
            {
                Name = "VentasBtnAnteriorNomina", Text = "Anterior",
                Location = new System.Drawing.Point(24, 550), Size = new System.Drawing.Size(100, 28),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left, Enabled = false,
                BackColor = System.Drawing.Color.LightGray, ForeColor = System.Drawing.SystemColors.GrayText,
                FlatStyle = FlatStyle.Flat
            };
            VentasBtnSiguienteNomina = new Button
            {
                Name = "VentasBtnSiguienteNomina", Text = "Siguiente",
                Location = new System.Drawing.Point(1134, 550), Size = new System.Drawing.Size(100, 28),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right, Enabled = false,
                BackColor = System.Drawing.Color.LightGray, ForeColor = System.Drawing.SystemColors.GrayText,
                FlatStyle = FlatStyle.Flat
            };
            VentasBtnAnteriorNomina.FlatAppearance.BorderSize = 0;
            VentasBtnSiguienteNomina.FlatAppearance.BorderSize = 0;
            VentasBtnAnteriorNomina.Click += VentasMetCambiarPaginaNomina;
            VentasBtnSiguienteNomina.Click += VentasMetCambiarPaginaNomina;
            VentasLblPaginaNomina = new Label
            {
                Name = "VentasLblPaginaNomina", AutoSize = false,
                Location = new System.Drawing.Point(486, 554), Size = new System.Drawing.Size(300, 20),
                Anchor = AnchorStyles.Bottom, Text = "Página 0 de 0 · 0 registros",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            VentasTbpNomina.Controls.Add(VentasBtnAnteriorNomina);
            VentasTbpNomina.Controls.Add(VentasBtnSiguienteNomina);
            VentasTbpNomina.Controls.Add(VentasLblPaginaNomina);
            VentasTbpNomina.Controls.Add(VentasBtnActualizarNomina);
            VentasTbpNomina.Controls.Add(VentasLblNominaEstado);
            VentasTbpNomina.Controls.Add(VentasDgvNomina);
            VentasTbcPestanas.TabPages.Add(VentasTbpNomina);
        }

        private void VentasMetCargarNominaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasLblNominaEstado.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
                _ComisionesNomina = _ControladorComision.VentasMetObtenerComisionesNomina();
                _PaginaNomina = 0;
                VentasDgvNomina.DataSource = null;
                VentasMetMostrarPaginaNomina();
                VentasLblNominaEstado.Text = _ComisionesNomina.Count == 0
                    ? "Aún no hay abonos de comisión. Asigne un vendedor y registre pagos confirmados del cliente."
                    : _ComisionesNomina.Count + " abono(s) de comisión. Cada fila corresponde a un pago confirmado del cliente.";
            }
            catch (Exception Error)
            {
                VentasLblNominaEstado.ForeColor = System.Drawing.Color.Firebrick;
                VentasLblNominaEstado.Text = "No se pudieron cargar las comisiones: " + VentasMetObtenerDetalleError(Error);
            }
        }

        private void VentasMetCambioSeleccionFactura(object Sender, EventArgs Argumentos)
        {
            if (Sender == VentasCboFactura)
            {
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                _IdFacturaVistaPrevia = 0;
            }
            if (!_CargandoCatalogosFactura && Sender == VentasCboFactura)
            {
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (Factura != null && Factura.IdVendedor > 0 &&
                    VentasCboVendedorFactura.Items.Cast<ClsVendedorComisionVista>().Any(Vendedor => Vendedor.IdVendedor == Factura.IdVendedor))
                    VentasCboVendedorFactura.SelectedValue = Factura.IdVendedor;
                else
                    VentasCboVendedorFactura.SelectedIndex = -1;
            }
            VentasMetCalcularFacturaSeleccionada();
        }

        private void VentasMetCalcularFacturaSeleccionada()
        {
            if (_CargandoCatalogosFactura) return;
            if (VentasCboFactura.SelectedValue == null ||
                VentasCboVendedorFactura.SelectedValue == null)
            {
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                VentasDgvDetalleFactura.DataSource = null;
                VentasMetMostrarPaginaFactura();
                VentasLblEstadoFactura.Text = "Seleccione una factura con vendedor asignado o seleccione un vendedor.";
                return;
            }
            ClsFacturaComisionVista FacturaSeleccionada = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
            int IdVendedorSeleccionado = Convert.ToInt32(VentasCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
            if (FacturaSeleccionada != null && FacturaSeleccionada.IdVendedor != IdVendedorSeleccionado)
            {
                // Cambiar el vendedor no debe borrar los renglones ni las tasas
                // seleccionadas para la factura actual.
                if (_IdFacturaVistaPrevia != FacturaSeleccionada.IdFactura || _DetallesFactura.Count == 0)
                    VentasMetCalcularFacturaClick(VentasBtnCalcularFactura, EventArgs.Empty);
                else
                    VentasLblEstadoFactura.Text = "Vendedor seleccionado: " +
                        VentasCboVendedorFactura.Text +
                        ". Revise los métodos por producto y pulse «Asignar vendedor» para guardar.";
                return;
            }
            VentasMetCalcularFacturaClick(VentasBtnCalcularFactura, EventArgs.Empty);
        }

        private List<ClsComisionFacturaDetalle> VentasMetObtenerSeleccionesMetodo()
        {
            VentasDgvDetalleFactura.EndEdit();
            foreach (DataGridViewRow Fila in VentasDgvDetalleFactura.Rows)
            {
                ClsComisionFacturaDetalle Detalle = Fila.DataBoundItem as ClsComisionFacturaDetalle;
                if (Detalle == null) continue;
                object Valor = Fila.Cells["VentasClmMetodoProducto"].Value;
                if (Detalle.ComisionGuardada && String.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                    Valor = Detalle.MetodoComision;
                if (Valor == null || String.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                    throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto en cada renglón de la factura.");
                Detalle.MetodoComision = VentasMetNormalizarMetodo(Convert.ToString(Valor));
                if (String.IsNullOrWhiteSpace(Detalle.MetodoComision))
                    throw new InvalidOperationException("Método no válido en la línea " + Detalle.IdFacturaDetalle + ": «" + Convert.ToString(Valor) + "».");
            }
            List<ClsComisionFacturaDetalle> Selecciones = _DetallesFactura;
            if (Selecciones.Any(Detalle => String.IsNullOrWhiteSpace(Detalle.MetodoComision)))
                throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto para cada producto. Use la paginación para revisar todos.");
            if (Selecciones.Count == 0) throw new InvalidOperationException("La factura no tiene productos cargados.");
            return Selecciones;
        }

        private void VentasMetActualizarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblMantenimientoEstado,
                    VentasCboAlcance, VentasCboElementoPorcentaje, VentasTxtPorcentaje);
                if (VentasCboAlcance.SelectedItem == null)
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "Seleccione el tipo de porcentaje.", VentasCboAlcance);
                    throw new ArgumentException("Seleccione el tipo de porcentaje.");
                }
                decimal Porcentaje;
                if (!decimal.TryParse(VentasTxtPorcentaje.Text, NumberStyles.Number,
                    CultureInfo.GetCultureInfo("es-GT"), out Porcentaje))
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "Ingrese un porcentaje válido entre 0.00 y 99.00.", VentasTxtPorcentaje);
                    throw new ArgumentException("Ingrese un porcentaje válido entre 0.00 y 99.00.");
                }
                if (Porcentaje < 0m || Porcentaje > 99m)
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "El porcentaje debe estar entre 0.00 y 99.00.", VentasTxtPorcentaje);
                    throw new ArgumentOutOfRangeException("Porcentaje", "El esquema permite porcentajes entre 0.00 y 99.00.");
                }
                ClsOpcionPorcentaje Opcion = VentasCboElementoPorcentaje.SelectedItem as ClsOpcionPorcentaje;
                if (Opcion == null)
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "Seleccione un elemento específico.", VentasCboElementoPorcentaje);
                    throw new ArgumentException("Seleccione la línea, producto, categoría o marca que desea modificar.");
                }
                EnumMetodoComision Metodo = VentasMetObtenerMetodo(VentasCboAlcance.SelectedItem.ToString());
                string TextoConfirmacion = "Se actualizará el porcentaje de " + Opcion.Etiqueta + " a " +
                    Porcentaje.ToString("0.00", CultureInfo.GetCultureInfo("es-GT")) + "%. ¿Desea continuar?";
                if (VentasMetConfirmar(this, "7008 – ComisionVendedores", TextoConfirmacion, "Sí, actualizar") != DialogResult.Yes)
                    return;
                int Actualizados = _ControladorComision.VentasMetActualizarPorcentaje(Metodo,
                    Opcion.IdClasificacion, Porcentaje);

                _ProductosPorcentaje = _ControladorComision.VentasMetConsultarPorcentajes(Metodo);
                _PaginaPorcentajes = Math.Min(_PaginaPorcentajes, VentasMetObtenerUltimaPagina(_ProductosPorcentaje.Count));
                VentasMetMostrarPaginaPorcentajes();
                VentasLblMantenimientoEstado.Text = Actualizados + " registro(s) de " + Opcion.Etiqueta +
                    " guardado(s) con " + Porcentaje.ToString("0.00") + "%.";
                VentasMetCargarElementosPorcentaje();
                VentasMetMostrarExito(this, "7008 – ComisionVendedores", VentasLblMantenimientoEstado.Text);
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblMantenimientoEstado, Excepcion.Message);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ClsProductoPorcentaje> VentasMetCargarPorcentajes()
        {
            _ProductosPorcentaje = _ControladorComision.VentasMetConsultarPorcentajes(
                VentasMetObtenerMetodo(VentasCboAlcance.SelectedItem.ToString()));
            _PaginaPorcentajes = 0;
            VentasMetMostrarPaginaPorcentajes();
            VentasMetCargarElementosPorcentaje();
            return _ProductosPorcentaje;
        }

        private void VentasMetCambiarAlcancePorcentaje(object Sender, EventArgs Argumentos)
        {
            if (_CargandoPorcentajes || _ProductosPorcentaje == null) return;
            VentasCboElementoPorcentaje.DataSource = null;
            VentasMetCargarElementosPorcentaje();
        }

        private void VentasMetCargarElementosPorcentaje()
        {
            if (VentasCboElementoPorcentaje == null || VentasCboAlcance.SelectedItem == null) return;
            EnumMetodoComision Metodo = VentasMetObtenerMetodo(VentasCboAlcance.SelectedItem.ToString());
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
                int IdSeleccionado = (VentasCboElementoPorcentaje.SelectedItem as ClsOpcionPorcentaje)?.IdClasificacion ?? 0;
                List<ClsOpcionPorcentaje> Lista = Opciones.OrderBy(Opcion => Opcion.Etiqueta).ToList();
                VentasCboElementoPorcentaje.DisplayMember = "Etiqueta";
                VentasCboElementoPorcentaje.ValueMember = "IdClasificacion";
                VentasCboElementoPorcentaje.DataSource = Lista;
                VentasCboElementoPorcentaje.SelectedIndex = Lista.FindIndex(Opcion => Opcion.IdClasificacion == IdSeleccionado);
            }
            finally { _CargandoPorcentajes = false; }
        }

        private void VentasMetMostrarPaginaResultados()
        {
            int UltimaPagina = VentasMetObtenerUltimaPagina(_Resultados.Count, _FilasResultadosPorPagina);
            _PaginaResultados = Math.Max(0, Math.Min(_PaginaResultados, UltimaPagina));
            VentasDgvResultados.DataSource = _Resultados.Skip(_PaginaResultados * _FilasResultadosPorPagina).Take(_FilasResultadosPorPagina).ToList();
            VentasLblPaginaResultados.Text = VentasMetObtenerTextoPagina(_PaginaResultados, _Resultados.Count, _FilasResultadosPorPagina);
            VentasMetActualizarBoton(VentasBtnAnteriorResultados, _PaginaResultados > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetActualizarBoton(VentasBtnSiguienteResultados,
                _PaginaResultados < UltimaPagina, System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetAplicarIndicadorOrden(VentasDgvResultados);
        }

        private void VentasMetMarcarColumnaObligatoria(object Sender, DataGridViewCellPaintingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex != -1 || Argumentos.ColumnIndex < 0 ||
                VentasDgvDetalleFactura.Columns[Argumentos.ColumnIndex].Name != "VentasClmMetodoProducto")
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
        private void VentasMetFormatearMoneda(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
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
            if (Tabla == VentasDgvResultados)
            {
                ClsComisionVendedorResultado Resultado = Tabla.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionVendedorResultado;
                if (Resultado != null) SimboloMoneda = Resultado.SimboloMoneda;
            }
            else
            {
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (Factura != null) SimboloMoneda = Factura.SimboloMoneda;
            }

            Argumentos.Value = (SimboloMoneda ?? String.Empty) + " " +
                Convert.ToDecimal(Argumentos.Value).ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            Argumentos.FormattingApplied = true;
        }

        private void VentasMetFormatearImporteNomina(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 || Argumentos.Value == null) return;
            string Propiedad = VentasDgvNomina.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (Propiedad != "TotalComision" && Propiedad != "MontoPagoCliente" && Propiedad != "ComisionDevengadaAbono") return;
            ClsComisionNominaVista Comision = VentasDgvNomina.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionNominaVista;
            if (Comision == null) return;
            Argumentos.Value = (Comision.SimboloMoneda ?? String.Empty) + " " +
                Convert.ToDecimal(Argumentos.Value).ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            Argumentos.FormattingApplied = true;
        }

        private void VentasMetOrdenarTabla(object Sender, DataGridViewCellMouseEventArgs Argumentos)
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

            if (Tabla == VentasDgvResultados)
                VentasMetOrdenarLista(_Resultados, Propiedad, Ascendente);
            else if (Tabla == VentasDgvDetalleFactura)
                VentasMetOrdenarLista(_DetallesFactura, Propiedad, Ascendente);
            else if (Tabla == VentasDgvPorcentajes)
                VentasMetOrdenarLista(_ProductosPorcentaje, Propiedad, Ascendente);
            else if (Tabla == VentasDgvNomina && _ComisionesNomina != null)
                VentasMetOrdenarLista(_ComisionesNomina, Propiedad, Ascendente);

            if (Tabla == VentasDgvResultados) VentasMetMostrarPaginaResultados();
            else if (Tabla == VentasDgvDetalleFactura) VentasMetMostrarPaginaFactura();
            else if (Tabla == VentasDgvPorcentajes) VentasMetMostrarPaginaPorcentajes();
            else if (Tabla == VentasDgvNomina)
            {
                VentasMetMostrarPaginaNomina();
                VentasMetAplicarIndicadorOrden(VentasDgvNomina);
            }
        }

        private static void VentasMetOrdenarLista<T>(List<T> Registros, string NombrePropiedad, bool Ascendente)
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

        private void VentasMetAplicarIndicadorOrden(DataGridView Tabla)
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

        private void VentasMetMostrarPaginaFactura()
        {
            VentasDgvDetalleFactura.DataSource = _DetallesFactura.Skip(_PaginaFactura * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            if (VentasDgvDetalleFactura.Columns.Contains("VentasClmMetodoProducto"))
                VentasDgvDetalleFactura.Columns["VentasClmMetodoProducto"].DataPropertyName = "MetodoComision";
            VentasLblPaginaFactura.Text = VentasMetObtenerTextoPagina(_PaginaFactura, _DetallesFactura.Count);
            VentasMetActualizarBoton(VentasBtnAnteriorFactura, _PaginaFactura > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetActualizarBoton(VentasBtnSiguienteFactura,
                _PaginaFactura < VentasMetObtenerUltimaPagina(_DetallesFactura.Count), System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetAplicarIndicadorOrden(VentasDgvDetalleFactura);
        }

        private void VentasMetCambiarMetodoProducto(object Sender, DataGridViewCellEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 ||
                VentasDgvDetalleFactura.Columns[Argumentos.ColumnIndex].Name != "VentasClmMetodoProducto") return;
            ClsComisionFacturaDetalle Detalle = VentasDgvDetalleFactura.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionFacturaDetalle;
            if (Detalle == null || Detalle.ComisionGuardada) return;
            string Metodo = VentasMetNormalizarMetodo(Convert.ToString(
                VentasDgvDetalleFactura.Rows[Argumentos.RowIndex].Cells[Argumentos.ColumnIndex].Value));
            if (String.IsNullOrWhiteSpace(Metodo)) return;
            decimal Porcentaje = Metodo == "Producto" ? Detalle.PorcentajeProducto : Metodo == "Marca" ? Detalle.PorcentajeMarca : Metodo == "Linea" ? Detalle.PorcentajeLinea : Detalle.PorcentajeCategoria;
            Detalle.MetodoComision = Metodo;
            Detalle.Porcentaje = Porcentaje;
            Detalle.ComisionTotal = Math.Round(Detalle.BaseVenta * Porcentaje / 100m, 4, MidpointRounding.AwayFromZero);
            Detalle.ComisionPendiente = Math.Max(0m, Detalle.ComisionTotal - Detalle.ComisionDisponible);
            decimal TotalComision = _DetallesFactura.Sum(Item => Item.ComisionTotal);
            decimal PorcentajePonderado = _DetallesFactura.Sum(Item => Item.BaseVenta) <= 0m ? 0m : TotalComision / _DetallesFactura.Sum(Item => Item.BaseVenta) * 100m;
            ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
            VentasLblEstadoFactura.Text = "Total comisión: " + PorcentajePonderado.ToString("0.00") + "% · " + (Factura == null ? String.Empty : Factura.SimboloMoneda) + " " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            VentasDgvDetalleFactura.Refresh();
        }

        private void VentasMetAbrirAyuda(object Sender, EventArgs Argumentos)
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

        private void VentasMetFormatearMetodoProducto(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 ||
                VentasDgvDetalleFactura.Columns[Argumentos.ColumnIndex].Name != "VentasClmDetallePorcentaje") return;
            ClsComisionFacturaDetalle Detalle = VentasDgvDetalleFactura.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionFacturaDetalle;
            if (Detalle != null)
            {
                Argumentos.Value = Detalle.Porcentaje.ToString("0.00") + "%";
                Argumentos.FormattingApplied = true;
            }
        }

        private void VentasMetMostrarPaginaPorcentajes()
        {
            VentasDgvPorcentajes.DataSource = _ProductosPorcentaje.Skip(_PaginaPorcentajes * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            VentasLblPaginaPorcentajes.Text = VentasMetObtenerTextoPagina(_PaginaPorcentajes, _ProductosPorcentaje.Count);
            VentasMetActualizarBoton(VentasBtnAnteriorPorcentajes, _PaginaPorcentajes > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetActualizarBoton(VentasBtnSiguientePorcentajes,
                _PaginaPorcentajes < VentasMetObtenerUltimaPagina(_ProductosPorcentaje.Count), System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetAplicarIndicadorOrden(VentasDgvPorcentajes);
        }

        private int VentasMetObtenerUltimaPagina(int TotalRegistros)
        {
            return Math.Max(0, (int)Math.Ceiling(TotalRegistros / (double)_FilasPorPagina) - 1);
        }

        private int VentasMetObtenerUltimaPagina(int TotalRegistros, int FilasPorPagina)
        {
            return Math.Max(0, (int)Math.Ceiling(TotalRegistros / (double)FilasPorPagina) - 1);
        }

        private string VentasMetObtenerTextoPagina(int Pagina, int TotalRegistros, int FilasPorPagina)
        {
            int TotalPaginas = TotalRegistros == 0 ? 0 : VentasMetObtenerUltimaPagina(TotalRegistros, FilasPorPagina) + 1;
            return "Página " + (TotalPaginas == 0 ? 0 : Pagina + 1) + " de " + TotalPaginas + " · " + TotalRegistros + " registros";
        }

        private void VentasMetEnvolverControl(Control Campo, string NombreContenedor)
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
        private void VentasMetMostrarError(Label EtiquetaEstado, string Mensaje, params Control[] Controles)
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

        private string VentasMetObtenerDetalleError(Exception Excepcion)
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

        private static string VentasMetNormalizarMetodo(string Metodo)
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

        private void VentasMetActualizarBoton(Button Boton, bool Habilitado, System.Drawing.Color ColorActivo)
        {
            Boton.Enabled = Habilitado;
            Boton.BackColor = Habilitado ? System.Drawing.Color.FromArgb(59, 29, 74) : System.Drawing.Color.LightGray;
            Boton.ForeColor = Habilitado ? System.Drawing.Color.FromArgb(248, 245, 249) : System.Drawing.SystemColors.GrayText;
        }

        private void VentasMetRestablecerEstado(Label EtiquetaEstado, params Control[] Controles)
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

        private string VentasMetObtenerTextoPagina(int Pagina, int TotalRegistros)
        {
            int TotalPaginas = TotalRegistros == 0 ? 0 : VentasMetObtenerUltimaPagina(TotalRegistros) + 1;
            return "Página " + (TotalPaginas == 0 ? 0 : Pagina + 1) + " de " + TotalPaginas + " · " + TotalRegistros + " registros";
        }

        private void VentasMetCambiarPaginaResultados(object Sender, EventArgs Argumentos)
        {
            _PaginaResultados += Sender == VentasBtnAnteriorResultados ? -1 : 1;
            VentasMetMostrarPaginaResultados();
        }

        private void VentasMetCambiarPaginaFactura(object Sender, EventArgs Argumentos)
        {
            _PaginaFactura += Sender == VentasBtnAnteriorFactura ? -1 : 1;
            VentasMetMostrarPaginaFactura();
        }

        private void VentasMetCambiarPaginaPorcentajes(object Sender, EventArgs Argumentos)
        {
            _PaginaPorcentajes += Sender == VentasBtnAnteriorPorcentajes ? -1 : 1;
            VentasMetMostrarPaginaPorcentajes();
        }

        private void VentasMetMostrarPaginaNomina()
        {
            if (_ComisionesNomina == null) _ComisionesNomina = new List<ClsComisionNominaVista>();
            int UltimaPagina = VentasMetObtenerUltimaPagina(_ComisionesNomina.Count, _FilasNominaPorPagina);
            _PaginaNomina = Math.Max(0, Math.Min(_PaginaNomina, UltimaPagina));
            VentasDgvNomina.DataSource = _ComisionesNomina
                .Skip(_PaginaNomina * _FilasNominaPorPagina).Take(_FilasNominaPorPagina).ToList();
            VentasLblPaginaNomina.Text = VentasMetObtenerTextoPagina(
                _PaginaNomina, _ComisionesNomina.Count, _FilasNominaPorPagina);
            VentasMetActualizarBoton(VentasBtnAnteriorNomina, _PaginaNomina > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetActualizarBoton(VentasBtnSiguienteNomina,
                _PaginaNomina < UltimaPagina, System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetAplicarIndicadorOrden(VentasDgvNomina);
        }

        private void VentasMetCambiarPaginaNomina(object Sender, EventArgs Argumentos)
        {
            _PaginaNomina += Sender == VentasBtnAnteriorNomina ? -1 : 1;
            VentasMetMostrarPaginaNomina();
        }

        private EnumMetodoComision VentasMetObtenerMetodo(string Texto)
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
