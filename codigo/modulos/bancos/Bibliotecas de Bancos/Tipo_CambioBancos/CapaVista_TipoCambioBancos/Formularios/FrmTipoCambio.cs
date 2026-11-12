/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Victor Josue Ortiz Samayoa
 * Carné  : 0901-23-3424
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Formulario principal del módulo Tipo de Cambio. Gestiona tres
 *  pestañas: Tasas Actuales (consulta y filtrado de tipos de cambio
 *  de referencia del Banguat), Histórico (seguimiento de variaciones
 *  por par de monedas en un rango de fechas) y Conversor (cálculo
 *  de equivalencias multimoneda con visualización de banderas).
 *  Permite exportar datos, actualizar tasas desde el Banguat,
 *  mostrar detalle por moneda seleccionada e intercambiar el par
 *  de conversión. Incluye acceso a ayuda, reportes y cierre del
 *  formulario desde la barra inferior.
 * ================================================================
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_TipoCambio;
using CapaModelo_TipoCambio.Entidades;

namespace CapaVista_TipoCambio.Formularios
{
    public partial class FrmTipoCambio : Form
    {
        private readonly ClsControladorTipoCambio _controlador;
        private readonly ToolTip _informacion = new ToolTip();
        private readonly CancellationTokenSource _cierre = new CancellationTokenSource();
        private readonly Dictionary<string, Image> _banderas = new Dictionary<string, Image>();
        private CancellationTokenSource _consultaActuales;
        private CancellationTokenSource _consultaHistorico;
        private CancellationTokenSource _consultaConversion;
        private int _versionActuales;
        private int _versionHistorico;
        private int _versionConversion;
        private bool _inicializando = true;
        private bool _cargandoCatalogo;
        private List<ClsMoneda> _monedas = new List<ClsMoneda>();
        private DataTable _tasasConsultadas;
        private DataTable _tasasVista;
        private DataTable _historicoVista;
        private DateTime? _fechaConsultada;
        private ClsResultadoConversion _ultimaConversion;
        private decimal _importeConvertido;
        private readonly TextBox _buscarMoneda = new TextBox();
        private readonly ComboBox _claseMoneda = new ComboBox();
        private readonly Label _detalleMetodo = new Label();
        private bool _actualizandoBanguat;
        private const string AvisoConversion = "USD/GTQ: referencia publicada.\r\nOtras monedas: cruces con promedio de compra/venta.\r\nCotizaciones de la misma fecha · Cuatro decimales.";

        public FrmTipoCambio() : this(new ClsControladorTipoCambio()) { }

        public FrmTipoCambio(ClsControladorTipoCambio controlador)
        {
            _controlador = controlador ?? throw new ArgumentNullException(nameof(controlador));
            InitializeComponent();
            BancosMetInicializarFormulario();
            Shown += BancosFrmTipoCambio_Shown;
            FormClosing += BancosFrmTipoCambio_FormClosing;
            FormClosed += BancosFrmTipoCambio_FormClosed;
        }

        private void BancosMetInicializarFormulario()
        {
            DateTime hoy = DateTime.Today;
            BancosDtpFechaActuales.MaxDate = hoy;
            BancosDtpFechaConv.MaxDate = hoy;
            BancosDtpDesde.MaxDate = hoy;
            BancosDtpHasta.MaxDate = hoy;
            BancosDtpFechaActuales.Value = hoy;
            BancosDtpFechaConv.Value = hoy;
            BancosDtpHasta.Value = hoy;
            BancosDtpDesde.Value = hoy.AddDays(-30);
            BancosNumImporteConv.Minimum = 0;
            BancosNumImporteConv.DecimalPlaces = 4;
            BancosNumImporteConv.Value = 0;
            BancosChkMostrarSinTasa.Checked = false;
            BancosPicFlagOrigen.Image = null;
            BancosPicFlagDestino.Image = null;

            foreach (ComboBox combo in new[]
            {
                BancosCboMonedaBaseActuales, BancosCboMonedaActuales,
                BancosCboMonedaOrigenHist, BancosCboMonedaDestinoHist,
                BancosCboMonedaOrigenConv, BancosCboMonedaDestinoConv
            })
            {
                combo.Items.Clear();
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                combo.AutoCompleteSource = AutoCompleteSource.ListItems;
                combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                if (combo != BancosCboMonedaBaseActuales)
                {
                    combo.DropDownWidth = 350;
                    combo.KeyDown += BancosMoneda_KeyDown;
                    var menu = new ContextMenuStrip();
                    menu.Items.Add("Buscar por código o nombre…", null, (s, e) => BancosMetBuscarMoneda(combo));
                    combo.ContextMenuStrip = menu;
                    _informacion.SetToolTip(combo, "Escribe el código o pulsa Ctrl+F para buscar por código o nombre.");
                }
            }

            foreach (DataGridView grilla in new[] { BancosDgvTasasActuales, BancosDgvHistorico })
            {
                grilla.ReadOnly = true;
                grilla.AllowUserToAddRows = false;
                grilla.AllowUserToDeleteRows = false;
                grilla.AutoGenerateColumns = true;
                grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grilla.MultiSelect = false;
                grilla.CellFormatting += BancosDgvTasasActuales_CellFormatting;
            }

            // Los eventos Click y los de los combos existentes los conecta el diseñador.
            BancosDtpFechaActuales.ValueChanged += BancosDtpFechaActuales_ValueChanged;
            BancosDtpDesde.ValueChanged += BancosFiltrosHistorico_Changed;
            BancosDtpHasta.ValueChanged += BancosFiltrosHistorico_Changed;
            BancosCboMonedaOrigenHist.SelectedIndexChanged += BancosFiltrosHistorico_Changed;
            BancosCboMonedaDestinoHist.SelectedIndexChanged += BancosFiltrosHistorico_Changed;
            BancosDtpFechaConv.ValueChanged += BancosFechaConversor_Changed;

            BancosLblTituloTasasRef.Text = "Tasas publicadas y calculadas";
            BancosLblTasasRefSub.Text = "GTQ por unidad · Método en tabla";
            BancosLblKpi1Titulo.Text = "Tasas disponibles";
            BancosLblKpi1Sub.Text = "Para la fecha consultada";
            BancosLblKpi2Titulo.Text = "Moneda base";
            BancosLblKpi2Valor.Text = "GTQ";
            BancosLblKpi2Sub.Text = "Quetzal guatemalteco";
            BancosLblKpi3Titulo.Text = "Fecha consultada";
            BancosLblFuente.Text = "Fuente: Banco de Guatemala";
            BancosLblUltimaCarga.Text = "Última consulta: —";
            BancosLblEstadoConexion.Text = "●  Sin consulta";
            BancosLblEstadoConexion.ForeColor = Color.FromArgb(90, 105, 95);
            BancosLblSubMonedaActuales.Text = "Monedas admitidas por el servicio";
            BancosLblSubFiltrosHist.Text = "Elige el par y consulta su rango de fechas";
            BancosLblSubFechaConv.AutoSize = false;
            BancosLblSubFechaConv.Location = new Point(14, 445);
            BancosLblSubFechaConv.Size = new Size(320, 15);
            BancosLblSubFechaConv.Text = "Cotizaciones para la fecha seleccionada";
            BancosLblNoticeHist.Text = "Cruces: promedio compra/venta y referencia USD/GTQ, con cotizaciones de la misma fecha.";
            BancosLblNoticeConv.Text = AvisoConversion;
            BancosLblDispCotizacionDesc.Text = "Sin cotización para la fecha: Sin tasa.";
            BancosLblConvFechaTit.Text = "Fecha de la tasa:";
            BancosLblConvTasaDetalle.AutoSize = false;
            BancosLblConvTasaDetalle.AutoEllipsis = true;
            BancosLblConvTasaDetalle.Size = new Size(318, 22);
            BancosLblConvDispVal.AutoSize = false;
            BancosLblConvDispVal.Size = new Size(187, 30);
            BancosLblKpiVarSub.Text = "Última tasa menos primera tasa";
            BancosMetConfigurarBusquedaYDetalle();
            BancosMetLimpiarActuales("Consulta pendiente");
            BancosMetLimpiarHistorico("Consulta pendiente");
            BancosMetLimpiarConversion("Selecciona las monedas");
            _inicializando = false;
        }

        private async void BancosFrmTipoCambio_Shown(object sender, EventArgs e)
        {
            if (await BancosMetCargarCatalogoAsync())
            {
                await BancosMetConsultarActualesAsync(false);
                if (!BancosMetCerrando()) await BancosMetConsultarHistoricoAsync();
            }
        }

        private async Task<bool> BancosMetCargarCatalogoAsync()
        {
            if (_cargandoCatalogo || BancosMetCerrando()) return false;
            _cargandoCatalogo = true;
            BancosBtnActualizarBanguat.Enabled = false;
            BancosLblEstadoConexion.Text = "●  Cargando catálogo";
            try
            {
                List<ClsMoneda> monedas = await _controlador.BancosMetObtenerMonedasAsync(_cierre.Token);
                if (BancosMetCerrando()) return false;
                if (!monedas.Any(m => m.Codigo == "GTQ") || !monedas.Any(m => m.Codigo == "USD"))
                    throw new InvalidOperationException("El catálogo recibido no contiene GTQ y USD.");
                _monedas = monedas.OrderBy(m => BancosMetClaseMoneda(m) == "Moneda" ? 0 : 1)
                    .ThenBy(m => m.Codigo).ToList();
                _inicializando = true;
                try
                {
                    BancosCboMonedaBaseActuales.Items.Clear();
                    BancosCboMonedaBaseActuales.Items.Add(_monedas.First(m => m.Codigo == "GTQ"));
                    BancosCboMonedaBaseActuales.SelectedIndex = 0;
                    BancosCboMonedaActuales.Items.Clear();
                    BancosCboMonedaActuales.Items.Add("Todas las monedas");
                    BancosCboMonedaActuales.Items.AddRange(_monedas.Cast<object>().ToArray());
                    BancosCboMonedaActuales.SelectedIndex = 0;
                    BancosMetLlenarMonedas(BancosCboMonedaOrigenHist, "USD");
                    BancosMetLlenarMonedas(BancosCboMonedaDestinoHist, "GTQ");
                    BancosMetLlenarMonedas(BancosCboMonedaOrigenConv, "USD");
                    BancosMetLlenarMonedas(BancosCboMonedaDestinoConv, "GTQ");
                }
                finally { _inicializando = false; }
                BancosLblEstadoConexion.Text = "●  Catálogo recibido";
                BancosMetActualizarBanderas();
                BancosMetLimpiarConversion("Pulsa Calcular");
                return true;
            }
            catch (OperationCanceledException) { return false; }
            catch (Exception ex)
            {
                if (!BancosMetCerrando())
                {
                    BancosLblEstadoConexion.Text = "●  Sin conexión";
                    BancosMetMostrarError("No se pudo cargar el catálogo. Puedes reintentar con Actualizar Banguat.", ex);
                }
                return false;
            }
            finally
            {
                _cargandoCatalogo = false;
                if (!BancosMetCerrando()) BancosBtnActualizarBanguat.Enabled = true;
            }
        }

        private void BancosMetLlenarMonedas(ComboBox combo, string codigo)
        {
            combo.Items.Clear();
            combo.Items.AddRange(_monedas.Cast<object>().ToArray());
            combo.SelectedItem = _monedas.First(m => m.Codigo == codigo);
        }

        private void BancosMetConfigurarBusquedaYDetalle()
        {
            BancosPnlDetalleMoneda.Top = BancosPnlDispCotizacion.Top = 416;
            BancosLblTasasRefStatusIzq.Top = BancosLblTasasRefStatusDer.Top = 396;
            BancosDgvTasasActuales.Height = 208;
            BancosLblDispCotizacionTitulo.Text = "Buscar moneda / categoría";
            _buscarMoneda.Name = "BancosTxtBuscarMoneda";
            _buscarMoneda.SetBounds(10, 28, 155, 22);
            _buscarMoneda.Font = new Font("Segoe UI", 8F);
            _buscarMoneda.MaxLength = 80;
            _informacion.SetToolTip(_buscarMoneda, "Filtra la tabla por código o nombre, por ejemplo USD, euro o peso.");
            _buscarMoneda.TextChanged += (s, e) => { if (!_inicializando) BancosMetFiltrarActuales(); };
            _claseMoneda.Name = "BancosCboClaseMoneda";
            _claseMoneda.SetBounds(172, 28, 150, 22);
            _claseMoneda.Font = new Font("Segoe UI", 8F);
            _claseMoneda.DropDownStyle = ComboBoxStyle.DropDownList;
            _claseMoneda.Items.AddRange(new object[] { "Todas", "Monedas", "Históricas", "Unidades de cuenta" });
            _claseMoneda.SelectedIndex = 0;
            _claseMoneda.SelectedIndexChanged += (s, e) => { if (!_inicializando) BancosMetFiltrarActuales(); };
            BancosLblDispCotizacionDesc.AutoSize = false;
            BancosLblDispCotizacionDesc.SetBounds(10, 52, 312, 16);
            BancosLblDispCotizacionDesc.Font = new Font("Segoe UI", 7F);
            BancosLblDispCotizacionDesc.Text = "Todas conserva monedas históricas y unidades de cuenta.";
            BancosChkMostrarSinTasa.Top = 73;
            BancosPnlDispCotizacion.Controls.Add(_buscarMoneda);
            BancosPnlDispCotizacion.Controls.Add(_claseMoneda);

            BancosLblDetalleTasaTit.Top = BancosLblDetalleTasaVal.Top = 53;
            BancosLblDetalleFechaTit.Top = BancosLblDetalleFechaVal.Top = 68;
            BancosLblDetalleFechaVal.AutoSize = false;
            BancosLblDetalleFechaVal.SetBounds(92, 68, 260, 14);
            BancosLblDetalleFechaVal.AutoEllipsis = true;
            _detalleMetodo.Name = "BancosLblDetalleMetodo";
            _detalleMetodo.SetBounds(10, 82, 342, 14);
            _detalleMetodo.Font = new Font("Segoe UI", 7F);
            _detalleMetodo.ForeColor = Color.FromArgb(80, 90, 85);
            _detalleMetodo.AutoEllipsis = true;
            _detalleMetodo.Text = "Método: —";
            BancosPnlDetalleMoneda.Controls.Add(_detalleMetodo);

            BancosLblConvResultadoGrande.AutoSize = false;
            BancosLblConvResultadoGrande.SetBounds(14, 45, 288, 52);
            BancosLblConvResultadoGrande.AutoEllipsis = true;
            BancosLblConvResultadoGrande.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            BancosLblConvCalculoDetalle.AutoSize = false;
            BancosLblConvCalculoDetalle.SetBounds(18, 99, 282, 33);
            BancosLblConvCalculoDetalle.AutoEllipsis = true;
            BancosLblConvFuenteVal.AutoSize = false;
            BancosLblConvFuenteVal.Size = new Size(187, 18);
            BancosLblConvFuenteVal.AutoEllipsis = true;
            _informacion.SetToolTip(BancosBtnCalcular, "Calcular o reintentar la conversión seleccionada.");
            _informacion.SetToolTip(BancosBtnActualizarBanguat, "Consulta de nuevo a Banguat los datos de la pestaña abierta.");
            BancosLblSubMonedaDestinoConv.Text = "Código o nombre: Ctrl+F para buscar.";
        }

        private static string BancosMetClaseMoneda(ClsMoneda moneda)
        {
            if (moneda == null) return "";
            if (moneda.IdBanguat == 26 || moneda.IdBanguat == 28) return "Unidad de cuenta";
            return new[] { 4, 6, 8, 11, 12, 13, 14, 22, 27, 37 }.Contains(moneda.IdBanguat)
                ? "Histórica" : "Moneda";
        }

        private void BancosMoneda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                e.SuppressKeyPress = true;
                BancosMetBuscarMoneda((ComboBox)sender);
            }
        }

        private void BancosMetBuscarMoneda(ComboBox destino)
        {
            if (_monedas.Count == 0 || BancosMetCerrando()) return;
            using (var buscador = new Form
            {
                Text = "Buscar moneda", ClientSize = new Size(460, 380),
                StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false, MaximizeBox = false, ShowInTaskbar = false,
                BackColor = Color.FromArgb(237, 245, 238), Font = new Font("Segoe UI", 9F)
            })
            using (var entrada = new TextBox { Name = "BancosTxtBuscarSeleccionMoneda", Dock = DockStyle.Fill, MaxLength = 80 })
            using (var lista = new ListBox { Name = "BancosLstMonedasBusqueda", Dock = DockStyle.Fill, IntegralHeight = false })
            {
                var marco = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 1, RowCount = 4 };
                marco.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                marco.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
                marco.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
                marco.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                marco.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
                marco.Controls.Add(new Label { Text = "Código o nombre · doble clic para seleccionar", Dock = DockStyle.Fill }, 0, 0);
                marco.Controls.Add(entrada, 0, 1);
                marco.Controls.Add(lista, 0, 2);
                var aceptar = new Button { Text = "Seleccionar", Dock = DockStyle.Right, Width = 120, BackColor = Color.FromArgb(28, 74, 48), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
                marco.Controls.Add(aceptar, 0, 3);
                buscador.Controls.Add(marco);
                Action filtrar = () =>
                {
                    string texto = entrada.Text.Trim();
                    lista.Items.Clear();
                    foreach (var moneda in _monedas.Where(m => m.Codigo.IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0 || m.Nombre.IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0))
                        lista.Items.Add(new OpcionBusquedaMoneda { Moneda = moneda, Clase = BancosMetClaseMoneda(moneda) });
                    if (lista.Items.Count > 0) lista.SelectedIndex = 0;
                    aceptar.Enabled = lista.Items.Count > 0;
                };
                Action seleccionar = () =>
                {
                    var opcion = lista.SelectedItem as OpcionBusquedaMoneda;
                    if (opcion == null) return;
                    destino.SelectedItem = opcion.Moneda;
                    buscador.DialogResult = DialogResult.OK;
                    buscador.Close();
                };
                entrada.TextChanged += (s, e) => filtrar();
                aceptar.Click += (s, e) => seleccionar();
                lista.DoubleClick += (s, e) => seleccionar();
                buscador.AcceptButton = aceptar;
                buscador.KeyPreview = true;
                buscador.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) buscador.Close(); };
                filtrar();
                buscador.ShowDialog(this);
            }
        }

        private sealed class OpcionBusquedaMoneda
        {
            public ClsMoneda Moneda { get; set; }
            public string Clase { get; set; }
            public override string ToString() { return Moneda + " · " + Clase; }
        }

        private static string BancosMetCodigo(ComboBox combo)
        {
            return (combo.SelectedItem as ClsMoneda)?.Codigo ?? "";
        }

        private bool BancosMetCerrando()
        {
            return IsDisposed || Disposing || _cierre.IsCancellationRequested;
        }

        private CancellationTokenSource BancosMetNuevaConsulta(ref CancellationTokenSource anterior)
        {
            if (anterior != null) anterior.Cancel();
            anterior = CancellationTokenSource.CreateLinkedTokenSource(_cierre.Token);
            return anterior;
        }

        private async Task BancosMetConsultarActualesAsync(bool forzar)
        {
            if (_inicializando || _monedas.Count == 0 || BancosMetCerrando()) return;
            int version = ++_versionActuales;
            CancellationTokenSource consulta = BancosMetNuevaConsulta(ref _consultaActuales);
            DateTime fecha = BancosDtpFechaActuales.Value.Date;
            BancosMetLimpiarActuales("Consultando…");
            BancosBtnConsultarActuales.Enabled = false;
            BancosBtnActualizarBanguat.Enabled = false;
            BancosLblEstadoConexion.Text = "●  Consultando Banguat";
            try
            {
                DataTable tabla = await _controlador.BancosMetObtenerTasasActualesAsync(fecha, true, forzar, consulta.Token);
                if (BancosMetCerrando() || version != _versionActuales || consulta.IsCancellationRequested) return;
                _tasasConsultadas = tabla;
                _fechaConsultada = fecha;
                BancosMetFiltrarActuales();
                BancosLblKpi1Valor.Text = tabla.AsEnumerable().Count(f => !f.IsNull("Tasa en GTQ")).ToString();
                BancosLblKpi3Valor.Text = fecha.ToString("dd/MM/yyyy");
                BancosLblUltimaCarga.Text = "Última consulta: " + DateTime.Now.ToString("dd/MM HH:mm:ss");
                BancosLblEstadoConexion.Text = "●  Consulta completada";
                BancosLblEstadoConexion.ForeColor = Color.FromArgb(28, 74, 48);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                if (!BancosMetCerrando() && version == _versionActuales)
                {
                    BancosMetLimpiarActuales("Consulta fallida");
                    BancosLblEstadoConexion.Text = "●  Consulta fallida";
                    BancosLblEstadoConexion.ForeColor = Color.FromArgb(180, 83, 9);
                    BancosMetMostrarError("No se pudieron consultar las tasas.", ex);
                }
            }
            finally
            {
                if (!BancosMetCerrando() && version == _versionActuales)
                {
                    BancosBtnConsultarActuales.Enabled = true;
                    BancosBtnActualizarBanguat.Enabled = true;
                }
                if (ReferenceEquals(_consultaActuales, consulta)) _consultaActuales = null;
                consulta.Dispose();
            }
        }

        private void BancosMetFiltrarActuales()
        {
            if (_tasasConsultadas == null) return;
            string codigo = BancosMetCodigo(BancosCboMonedaActuales);
            string texto = _buscarMoneda.Text.Trim();
            string clase = _claseMoneda.SelectedIndex == 1 ? "Moneda" :
                _claseMoneda.SelectedIndex == 2 ? "Histórica" :
                _claseMoneda.SelectedIndex == 3 ? "Unidad de cuenta" : "";
            IEnumerable<DataRow> filas = _tasasConsultadas.AsEnumerable().Where(f =>
                (codigo.Length == 0 || Convert.ToString(f["Código"]) == codigo) &&
                (BancosChkMostrarSinTasa.Checked || Convert.ToString(f["Estado"]) != "Sin tasa") &&
                (texto.Length == 0 || Convert.ToString(f["Código"]).IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    Convert.ToString(f["Moneda"]).IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0) &&
                (clase.Length == 0 || BancosMetClaseMoneda(_monedas.FirstOrDefault(m => m.Codigo == Convert.ToString(f["Código"]))) == clase));
            _tasasVista = filas.Any() ? filas.CopyToDataTable() : _tasasConsultadas.Clone();
            BancosDgvTasasActuales.DataSource = _tasasVista;
            BancosMetConfigurarColumnas(BancosDgvTasasActuales);
            BancosLblTasasRefStatusIzq.Text = _tasasVista.Rows.Count == 0
                ? "0 registros · Revisa filtros y Mostrar sin tasa"
                : "Mostrando " + _tasasVista.Rows.Count + " registros";
            BancosLblTasasRefStatusDer.Text = "Fecha solicitada: " + _fechaConsultada.Value.ToString("dd/MM/yyyy");
            BancosMetActualizarDetalleMoneda();
        }

        private void BancosMetLimpiarActuales(string estado)
        {
            _tasasConsultadas = null;
            _tasasVista = null;
            _fechaConsultada = null;
            BancosDgvTasasActuales.DataSource = null;
            BancosLblKpi1Valor.Text = "—";
            BancosLblKpi3Valor.Text = "—";
            BancosLblTasasRefStatusIzq.Text = estado;
            BancosLblTasasRefStatusDer.Text = "Sin datos consultados";
            BancosLblDetalleCodVal.Text = "—";
            BancosLblDetalleNomVal.Text = "—";
            BancosLblDetalleTasaVal.Text = "—";
            BancosLblDetalleFechaVal.Text = "—";
            _detalleMetodo.Text = "Método: —";
        }

        private void BancosMetActualizarDetalleMoneda()
        {
            if (BancosDgvTasasActuales.SelectedRows.Count == 0 || _tasasVista == null)
            {
                BancosLblDetalleCodVal.Text = "—";
                BancosLblDetalleNomVal.Text = "—";
                BancosLblDetalleTasaVal.Text = "—";
                BancosLblDetalleFechaVal.Text = "—";
                _detalleMetodo.Text = "Método: —";
                return;
            }
            var fila = BancosDgvTasasActuales.SelectedRows[0].DataBoundItem as DataRowView;
            if (fila == null) return;
            string codigo = Convert.ToString(fila["Código"]);
            BancosLblDetalleCodVal.Text = codigo;
            BancosLblDetalleNomVal.Text = Convert.ToString(fila["Moneda"]);
            BancosLblDetalleTasaVal.Text = fila.Row.IsNull("Tasa en GTQ") ? "Sin cotización" :
                "1 " + codigo + " = " + Convert.ToDecimal(fila["Tasa en GTQ"]).ToString("F4") + " GTQ";
            BancosLblDetalleFechaVal.Text = fila.Row.IsNull("Tasa en GTQ") ? "Sin tasa para la fecha elegida" :
                Convert.ToDateTime(fila["Fecha"]).ToString("dd/MM/yyyy") + " · " + Convert.ToString(fila["Fuente"]);
            BancosLblDetalleTasaVal.AccessibleDescription = Convert.ToString(fila["Metodo"]);
            string metodo = Convert.ToString(fila["Metodo"]);
            string descripcion = metodo == "IDENTIDAD" ? "Identidad (1 a 1)" : metodo == "DIRECTA" ? "Referencia directa" :
                metodo.StartsWith("CRUZADA", StringComparison.OrdinalIgnoreCase) ? "Cruce calculado con promedio" :
                metodo == "PROMEDIO HISTORICO" ? "Promedio de compra/venta" : metodo;
            string clase = BancosMetClaseMoneda(_monedas.FirstOrDefault(m => m.Codigo == codigo));
            _detalleMetodo.Text = clase + " · Método: " + descripcion;
            _informacion.SetToolTip(_detalleMetodo, _detalleMetodo.Text);
            _informacion.SetToolTip(BancosLblDetalleFechaVal, BancosLblDetalleFechaVal.Text);
        }

        private void BancosDtpFechaActuales_ValueChanged(object sender, EventArgs e)
        {
            if (_inicializando) return;
            ++_versionActuales;
            _consultaActuales?.Cancel();
            BancosMetLimpiarActuales("Fecha cambiada: pulsa Consultar");
            BancosLblEstadoConexion.Text = "●  Consulta pendiente";
            BancosBtnConsultarActuales.Enabled = true;
            BancosBtnActualizarBanguat.Enabled = !_cargandoCatalogo;
        }

        private async void BancosBtnConsultarActuales_Click(object sender, EventArgs e)
        {
            if (_monedas.Count == 0 && !await BancosMetCargarCatalogoAsync()) return;
            await BancosMetConsultarActualesAsync(false);
        }

        private async void BancosBtnActualizarBanguat_Click(object sender, EventArgs e)
        {
            await BancosMetActualizarPestanaAsync();
        }

        private async Task BancosMetActualizarPestanaAsync()
        {
            if (_actualizandoBanguat || BancosMetCerrando()) return;
            _actualizandoBanguat = true;
            TabPage pestana = BancosTabControl.SelectedTab;
            BancosBtnActualizarBanguat.Enabled = false;
            try
            {
                if (_monedas.Count == 0 && !await BancosMetCargarCatalogoAsync()) return;
                if (pestana == BancosTabHistorico) { ++_versionHistorico; _consultaHistorico?.Cancel(); }
                else if (pestana == BancosTabConversor) { ++_versionConversion; _consultaConversion?.Cancel(); }
                else { ++_versionActuales; _consultaActuales?.Cancel(); }
                await _controlador.BancosMetInvalidarCacheAsync(_cierre.Token);
                if (BancosMetCerrando() || BancosTabControl.SelectedTab != pestana) return;
                BancosLblEstadoConexion.Text = "●  Actualizando " + pestana.Text.Trim();
                if (pestana == BancosTabHistorico) await BancosMetConsultarHistoricoAsync();
                else if (pestana == BancosTabConversor) await BancosMetRecalcularConversorAsync();
                else await BancosMetConsultarActualesAsync(true);
                if (!BancosMetCerrando() && BancosTabControl.SelectedTab == pestana)
                {
                    bool completada = pestana == BancosTabHistorico ? _historicoVista != null :
                        pestana == BancosTabConversor ? _ultimaConversion != null : _tasasConsultadas != null;
                    BancosLblEstadoConexion.Text = completada ? "●  Consulta completada" :
                        pestana == BancosTabConversor ? "●  " + BancosLblConvDispVal.Text : "●  Consulta fallida";
                    if (completada) BancosLblUltimaCarga.Text = "Última consulta: " + DateTime.Now.ToString("dd/MM HH:mm:ss");
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                if (!BancosMetCerrando())
                {
                    BancosLblEstadoConexion.Text = "●  No se pudo actualizar";
                    BancosMetMostrarError("No se pudo actualizar la pestaña seleccionada. Puedes reintentar.", ex);
                }
            }
            finally
            {
                _actualizandoBanguat = false;
                if (!BancosMetCerrando())
                {
                    BancosBtnActualizarBanguat.Enabled = !_cargandoCatalogo;
                    if (_consultaActuales == null || _consultaActuales.IsCancellationRequested) BancosBtnConsultarActuales.Enabled = true;
                    if (_consultaHistorico == null || _consultaHistorico.IsCancellationRequested) BancosBtnBuscarHist.Enabled = true;
                    if (_consultaConversion == null || _consultaConversion.IsCancellationRequested) BancosBtnCalcular.Enabled = true;
                }
            }
        }

        private void BancosCboMonedaActuales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_inicializando) BancosMetFiltrarActuales();
        }

        private void BancosChkMostrarSinTasa_CheckedChanged(object sender, EventArgs e)
        {
            if (!_inicializando) BancosMetFiltrarActuales();
        }

        private void BancosDgvTasasActuales_SelectionChanged(object sender, EventArgs e)
        {
            if (!_inicializando) BancosMetActualizarDetalleMoneda();
        }

        private async Task BancosMetConsultarHistoricoAsync()
        {
            if (_inicializando || _monedas.Count == 0 || BancosMetCerrando()) return;
            int version = ++_versionHistorico;
            CancellationTokenSource consulta = BancosMetNuevaConsulta(ref _consultaHistorico);
            DateTime desde = BancosDtpDesde.Value.Date;
            DateTime hasta = BancosDtpHasta.Value.Date;
            string origen = BancosMetCodigo(BancosCboMonedaOrigenHist);
            string destino = BancosMetCodigo(BancosCboMonedaDestinoHist);
            BancosMetLimpiarHistorico("Consultando…");
            BancosLblHistParDer.Text = origen + " → " + destino;
            BancosBtnBuscarHist.Enabled = false;
            try
            {
                DataTable tabla = await _controlador.BancosMetObtenerHistoricoAsync(desde, hasta, origen, destino, consulta.Token);
                if (BancosMetCerrando() || version != _versionHistorico || consulta.IsCancellationRequested) return;
                _historicoVista = tabla;
                BancosDgvHistorico.DataSource = tabla;
                BancosMetConfigurarColumnas(BancosDgvHistorico);
                BancosLblHistStatusIzq.Text = tabla.Rows.Count + " registros encontrados";
                BancosLblHistStatusDer.Text = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy");
                if (tabla.Rows.Count > 0)
                {
                    DataRow[] ordenadas = tabla.Select("", "Fecha ASC");
                    decimal minima = ordenadas.Min(f => Convert.ToDecimal(f["Tasa"]));
                    decimal maxima = ordenadas.Max(f => Convert.ToDecimal(f["Tasa"]));
                    decimal variacion = Convert.ToDecimal(ordenadas.Last()["Tasa"]) - Convert.ToDecimal(ordenadas.First()["Tasa"]);
                    BancosLblKpiMinValor.Text = minima.ToString("F4");
                    BancosLblKpiMaxValor.Text = maxima.ToString("F4");
                    BancosLblKpiVarValor.Text = variacion.ToString("+0.0000;-0.0000;0.0000");
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                if (!BancosMetCerrando() && version == _versionHistorico)
                {
                    BancosMetLimpiarHistorico("Consulta fallida");
                    BancosMetMostrarError("No se pudo consultar el histórico.", ex);
                }
            }
            finally
            {
                if (!BancosMetCerrando() && version == _versionHistorico) BancosBtnBuscarHist.Enabled = true;
                if (ReferenceEquals(_consultaHistorico, consulta)) _consultaHistorico = null;
                consulta.Dispose();
            }
        }

        private void BancosMetLimpiarHistorico(string estado)
        {
            _historicoVista = null;
            BancosDgvHistorico.DataSource = null;
            BancosLblKpiMinValor.Text = "—";
            BancosLblKpiMaxValor.Text = "—";
            BancosLblKpiVarValor.Text = "—";
            BancosLblHistStatusIzq.Text = estado;
            BancosLblHistStatusDer.Text = "Sin datos consultados";
            BancosLblHistParDer.Text = "Par: —";
        }

        private void BancosFiltrosHistorico_Changed(object sender, EventArgs e)
        {
            if (_inicializando) return;
            ++_versionHistorico;
            _consultaHistorico?.Cancel();
            BancosMetLimpiarHistorico("Filtros cambiados: pulsa Buscar");
            BancosBtnBuscarHist.Enabled = true;
        }

        private async void BancosBtnBuscarHist_Click(object sender, EventArgs e)
        {
            if (_monedas.Count == 0 && !await BancosMetCargarCatalogoAsync()) return;
            await BancosMetConsultarHistoricoAsync();
        }

        private async Task BancosMetRecalcularConversorAsync()
        {
            if (_inicializando || _monedas.Count == 0 || BancosMetCerrando()) return;
            int version = ++_versionConversion;
            CancellationTokenSource consulta = BancosMetNuevaConsulta(ref _consultaConversion);
            decimal importe = BancosNumImporteConv.Value;
            DateTime fecha = BancosDtpFechaConv.Value.Date;
            string origen = BancosMetCodigo(BancosCboMonedaOrigenConv);
            string destino = BancosMetCodigo(BancosCboMonedaDestinoConv);
            BancosMetLimpiarConversion("Consultando…");
            BancosBtnCalcular.Enabled = false;
            BancosLblConvParVal.Text = origen + " → " + destino;
            try
            {
                ClsResultadoConversion resultado = await _controlador.BancosMetConvertirAsync(importe, fecha, origen, destino, consulta.Token);
                if (BancosMetCerrando() || version != _versionConversion || consulta.IsCancellationRequested) return;
                _ultimaConversion = resultado;
                _importeConvertido = importe;
                BancosLblConvResultadoGrande.Text = resultado.Resultado.ToString("N4") + " " + destino;
                BancosLblConvCalculoDetalle.Text = importe.ToString("N4") + " " + origen + " × " + resultado.Tasa.ToString("F4");
                BancosLblConvTasaDetalle.Text = "Tasa aplicada: 1 " + origen + " = " + resultado.Tasa.ToString("F4") + " " + destino;
                _informacion.SetToolTip(BancosLblConvTasaDetalle, BancosLblConvTasaDetalle.Text);
                _informacion.SetToolTip(BancosLblConvResultadoGrande, BancosLblConvResultadoGrande.Text);
                _informacion.SetToolTip(BancosLblConvCalculoDetalle, BancosLblConvCalculoDetalle.Text);
                BancosLblConvFechaVal.Text = resultado.Fecha.ToString("dd/MM/yyyy");
                BancosLblConvFuenteVal.Text = resultado.Fuente;
                _informacion.SetToolTip(BancosLblConvFuenteVal, resultado.Fuente);
                BancosLblConvDispVal.Text = "●  " + resultado.Metodo;
                BancosLblConvDispVal.ForeColor = Color.FromArgb(28, 74, 48);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                if (!BancosMetCerrando() && version == _versionConversion)
                {
                    string estado = BancosMetDescripcionError(ex);
                    BancosMetLimpiarConversion(estado);
                    BancosLblConvTasaDetalle.Text = "Tasa aplicada: —";
                    BancosLblNoticeConv.Text = estado + ".\r\n" + ex.Message + "\r\nPulsa Calcular para reintentar o revisa la fecha y las monedas.";
                    _informacion.SetToolTip(BancosLblNoticeConv, BancosLblNoticeConv.Text);
                    _informacion.SetToolTip(BancosLblConvTasaDetalle, ex.Message);
                    BancosLblConvDispVal.ForeColor = Color.FromArgb(180, 83, 9);
                }
            }
            finally
            {
                if (!BancosMetCerrando() && version == _versionConversion) BancosBtnCalcular.Enabled = true;
                if (ReferenceEquals(_consultaConversion, consulta)) _consultaConversion = null;
                consulta.Dispose();
            }
        }

        private void BancosMetLimpiarConversion(string estado)
        {
            _ultimaConversion = null;
            BancosLblNoticeConv.Text = AvisoConversion;
            _informacion.SetToolTip(BancosLblNoticeConv, AvisoConversion);
            BancosLblConvResultadoGrande.Text = "—";
            BancosLblConvCalculoDetalle.Text = "—";
            BancosLblConvTasaDetalle.Text = "—";
            _informacion.SetToolTip(BancosLblConvTasaDetalle, "");
            BancosLblConvFechaVal.Text = "—";
            BancosLblConvFuenteVal.Text = "—";
            BancosLblConvParVal.Text = "—";
            BancosLblConvDispVal.Text = estado;
            BancosLblConvDispVal.ForeColor = Color.FromArgb(90, 105, 95);
        }

        private static string BancosMetDescripcionError(Exception error)
        {
            for (Exception actual = error; actual != null; actual = actual.InnerException)
                if (actual is HttpRequestException || actual is WebException || actual is TimeoutException || actual is IOException)
                    return actual is TimeoutException ? "Banguat tardó en responder" : "Sin conexión con Banguat";
            string mensaje = error.Message;
            if (mensaje.IndexOf("no hay cotizaciones", StringComparison.OrdinalIgnoreCase) >= 0 ||
                mensaje.IndexOf("no publicó", StringComparison.OrdinalIgnoreCase) >= 0 ||
                mensaje.IndexOf("sin tasa", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Sin publicación para esta fecha";
            if (error is ArgumentException) return "Revisa las monedas, fecha e importe";
            return "No se pudo obtener una cotización válida";
        }

        private async void BancosCboMonedaOrigenConv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_inicializando) return;
            BancosMetActualizarBanderas();
            await BancosMetRecalcularConversorAsync();
        }

        private async void BancosCboMonedaDestinoConv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_inicializando) return;
            BancosMetActualizarBanderas();
            await BancosMetRecalcularConversorAsync();
        }

        private async void BancosFechaConversor_Changed(object sender, EventArgs e)
        {
            if (!_inicializando) await BancosMetRecalcularConversorAsync();
        }

        private async void BancosNumImporteConv_ValueChanged(object sender, EventArgs e)
        {
            if (!_inicializando) await BancosMetRecalcularConversorAsync();
        }

        private async void BancosBtnCalcular_Click(object sender, EventArgs e)
        {
            if (_monedas.Count == 0 && !await BancosMetCargarCatalogoAsync()) return;
            await BancosMetRecalcularConversorAsync();
        }

        private async void BancosBtnIntercambiar_Click(object sender, EventArgs e)
        {
            if (_monedas.Count == 0 || _inicializando) return;
            object origen = BancosCboMonedaOrigenConv.SelectedItem;
            object destino = BancosCboMonedaDestinoConv.SelectedItem;
            _inicializando = true;
            try
            {
                BancosCboMonedaOrigenConv.SelectedItem = destino;
                BancosCboMonedaDestinoConv.SelectedItem = origen;
            }
            finally { _inicializando = false; }
            BancosMetActualizarBanderas();
            await BancosMetRecalcularConversorAsync();
        }

        private async void BancosBtnVerHistorico_Click(object sender, EventArgs e)
        {
            if (_monedas.Count == 0) return;
            _inicializando = true;
            try
            {
                BancosCboMonedaOrigenHist.SelectedItem = BancosCboMonedaOrigenConv.SelectedItem;
                BancosCboMonedaDestinoHist.SelectedItem = BancosCboMonedaDestinoConv.SelectedItem;
                BancosDtpHasta.Value = BancosDtpFechaConv.Value.Date;
                BancosDtpDesde.Value = BancosDtpFechaConv.Value.Date.AddDays(-30);
            }
            finally { _inicializando = false; }
            BancosTabControl.SelectedTab = BancosTabHistorico;
            await BancosMetConsultarHistoricoAsync();
        }

        private static void BancosMetConfigurarColumnas(DataGridView grilla)
        {
            foreach (DataGridViewColumn columna in grilla.Columns)
            {
                if (columna.ValueType == typeof(decimal))
                {
                    columna.DefaultCellStyle.Format = "F4";
                    columna.DefaultCellStyle.NullValue = "—";
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (columna.ValueType == typeof(DateTime)) columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                if (columna.Name == "Metodo") columna.HeaderText = "Método";
            }
        }

        private void BancosDgvTasasActuales_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grilla = sender as DataGridView;
            if (grilla == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (grilla.Columns[e.ColumnIndex].Name == "Estado")
                e.CellStyle.ForeColor = Convert.ToString(e.Value) == "Sin tasa" ?
                    Color.FromArgb(180, 83, 9) : Color.FromArgb(28, 74, 48);
        }

        private void BancosDgvHistorico_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            BancosDgvTasasActuales_CellFormatting(sender, e);
        }

        private void BancosMetActualizarBanderas()
        {
            BancosPicFlagOrigen.Image = BancosMetObtenerBandera(BancosMetCodigo(BancosCboMonedaOrigenConv));
            BancosPicFlagDestino.Image = BancosMetObtenerBandera(BancosMetCodigo(BancosCboMonedaDestinoConv));
        }

        private Image BancosMetObtenerBandera(string codigo)
        {
            if (string.IsNullOrEmpty(codigo)) return null;
            Image bandera = null;
            if (_banderas.TryGetValue(codigo, out bandera)) return bandera;
            try
            {
                var recurso = Properties.Resources.ResourceManager.GetObject(codigo) as Image;
                if (recurso != null) bandera = new Bitmap(recurso);
                else
                {
                    string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resouces", "Banderas", codigo + ".png");
                    if (!File.Exists(ruta))
                        ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resouces", "Banderas", codigo + ".png");
                    if (File.Exists(ruta))
                        using (Image archivo = Image.FromFile(ruta)) bandera = new Bitmap(archivo);
                }
            }
            catch (Exception) { bandera = null; }
            _banderas[codigo] = bandera;
            return bandera;
        }

        private void BancosBtnExportarActuales_Click(object sender, EventArgs e)
        {
            BancosMetExportarCsv(_tasasVista, "Tasas_" + (_fechaConsultada ?? DateTime.Today).ToString("yyyyMMdd"));
        }

        private void BancosBtnExportarHist_Click(object sender, EventArgs e)
        {
            BancosMetExportarCsv(_historicoVista, "Historico_" + BancosMetCodigo(BancosCboMonedaOrigenHist) + "_" + BancosMetCodigo(BancosCboMonedaDestinoHist));
        }

        private void BancosMetExportarCsv(DataTable tabla, string nombre)
        {
            if (tabla == null || tabla.Rows.Count == 0)
            {
                MessageBox.Show(this, "Consulta datos antes de exportar.", "Exportar CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var dialogo = new SaveFileDialog { Filter = "Archivo CSV (*.csv)|*.csv", FileName = nombre + ".csv", AddExtension = true, DefaultExt = "csv" })
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    using (var escritor = new StreamWriter(dialogo.FileName, false, new UTF8Encoding(true)))
                    {
                        escritor.WriteLine(string.Join(",", tabla.Columns.Cast<DataColumn>().Select(c => BancosMetCeldaCsv(c.ColumnName))));
                        foreach (DataRow fila in tabla.Rows)
                            escritor.WriteLine(string.Join(",", fila.ItemArray.Select(BancosMetCeldaCsv)));
                    }
                    MessageBox.Show(this, "Archivo guardado correctamente.", "Exportar CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { BancosMetMostrarError("No se pudo guardar el archivo CSV.", ex); }
            }
        }

        private static string BancosMetCeldaCsv(object valor)
        {
            string texto;
            if (valor == null || valor == DBNull.Value) texto = "";
            else if (valor is DateTime) texto = ((DateTime)valor).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            else if (valor is decimal) texto = ((decimal)valor).ToString("F4", CultureInfo.InvariantCulture);
            else if (valor is string)
            {
                texto = (string)valor;
                string inicio = texto.TrimStart();
                if (inicio.Length > 0 && "=+-@".IndexOf(inicio[0]) >= 0) texto = "'" + texto;
            }
            else if (valor is IFormattable) texto = ((IFormattable)valor).ToString(null, CultureInfo.InvariantCulture);
            else texto = valor.ToString();
            return "\"" + texto.Replace("\"", "\"\"") + "\"";
        }

        private void BancosBtnFooterReporte_Click(object sender, EventArgs e)
        {
            DataTable tabla;
            string titulo;
            if (BancosTabControl.SelectedTab == BancosTabHistorico)
            {
                tabla = _historicoVista;
                titulo = "Histórico de tipos de cambio";
            }
            else if (BancosTabControl.SelectedTab == BancosTabConversor)
            {
                tabla = BancosMetTablaConversion();
                titulo = "Conversión de moneda";
            }
            else
            {
                tabla = _tasasVista;
                titulo = "Tasas de cambio en GTQ";
            }
            if (tabla == null || tabla.Rows.Count == 0)
            {
                MessageBox.Show(this, "Consulta datos en la pestaña antes de generar el reporte.", "Reporte", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            BancosMetVistaPrevia(tabla.Copy(), titulo);
        }

        private DataTable BancosMetTablaConversion()
        {
            if (_ultimaConversion == null) return null;
            var tabla = new DataTable();
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Origen", typeof(string));
            tabla.Columns.Add("Destino", typeof(string));
            tabla.Columns.Add("Importe", typeof(decimal));
            tabla.Columns.Add("Tasa", typeof(decimal));
            tabla.Columns.Add("Resultado", typeof(decimal));
            tabla.Columns.Add("Fuente", typeof(string));
            tabla.Columns.Add("Metodo", typeof(string));
            tabla.Rows.Add(_ultimaConversion.Fecha, _ultimaConversion.MonedaOrigen, _ultimaConversion.MonedaDestino,
                _importeConvertido, _ultimaConversion.Tasa, _ultimaConversion.Resultado, _ultimaConversion.Fuente, _ultimaConversion.Metodo);
            return tabla;
        }

        private void BancosMetVistaPrevia(DataTable tabla, string titulo)
        {
            int fila = 0;
            int pagina = 0;
            using (var documento = new PrintDocument())
            using (var vista = new PrintPreviewDialog())
            {
                documento.DocumentName = titulo;
                documento.DefaultPageSettings.Landscape = true;
                documento.DefaultPageSettings.Margins = new Margins(35, 35, 40, 40);
                documento.BeginPrint += (s, e) => { fila = 0; pagina = 0; };
                documento.PrintPage += (s, e) =>
                {
                    pagina++;
                    using (var fuente = new Font("Arial", 8))
                    using (var negrita = new Font("Arial", 8, FontStyle.Bold))
                    using (var tituloFuente = new Font("Arial", 12, FontStyle.Bold))
                    using (var formato = new StringFormat { Trimming = StringTrimming.EllipsisCharacter })
                    {
                        Rectangle margen = e.MarginBounds;
                        float y = margen.Top;
                        e.Graphics.DrawString(titulo, tituloFuente, Brushes.Black, margen.Left, y);
                        y += 25;
                        e.Graphics.DrawString("Fuente de cotizaciones: Banco de Guatemala · Tasas calculadas según columna Método", fuente, Brushes.Black, margen.Left, y);
                        y += 25;
                        float[] pesos = tabla.Columns.Cast<DataColumn>().Select(c =>
                            c.ColumnName == "Fuente" ? 1.8f : c.ColumnName == "Moneda" ? 1.6f :
                            c.ColumnName == "Metodo" ? 1.3f : c.DataType == typeof(DateTime) ? 1.1f : 1f).ToArray();
                        float suma = pesos.Sum();
                        float[] anchos = pesos.Select(p => p / suma * margen.Width).ToArray();
                        float altoCabecera = 30;
                        for (int col = 0; col < tabla.Columns.Count; col++)
                            altoCabecera = Math.Max(altoCabecera, e.Graphics.MeasureString(tabla.Columns[col].ColumnName, negrita, (int)anchos[col] - 8).Height + 8);
                        float x = margen.Left;
                        for (int col = 0; col < tabla.Columns.Count; col++)
                        {
                            e.Graphics.FillRectangle(Brushes.LightGray, x, y, anchos[col], altoCabecera);
                            e.Graphics.DrawRectangle(Pens.Gray, x, y, anchos[col], altoCabecera);
                            e.Graphics.DrawString(tabla.Columns[col].ColumnName == "Metodo" ? "Método" : tabla.Columns[col].ColumnName,
                                negrita, Brushes.Black, new RectangleF(x + 4, y + 4, anchos[col] - 8, altoCabecera - 6), formato);
                            x += anchos[col];
                        }
                        y += altoCabecera;
                        float altoMaximoFila = margen.Bottom - 25 - y;
                        while (fila < tabla.Rows.Count)
                        {
                            string[] valores = tabla.Rows[fila].ItemArray.Select(BancosMetTextoReporte).ToArray();
                            float alto = 24;
                            for (int col = 0; col < valores.Length; col++)
                                alto = Math.Max(alto, e.Graphics.MeasureString(valores[col], fuente, (int)anchos[col] - 8).Height + 8);
                            // Evitar un ciclo infinito de páginas si una descripción
                            // excepcionalmente larga excede una página completa.
                            alto = Math.Min(alto, altoMaximoFila);
                            if (y + alto > margen.Bottom - 25) break;
                            x = margen.Left;
                            for (int col = 0; col < valores.Length; col++)
                            {
                                e.Graphics.DrawRectangle(Pens.LightGray, x, y, anchos[col], alto);
                                e.Graphics.DrawString(valores[col], fuente, Brushes.Black,
                                    new RectangleF(x + 4, y + 4, anchos[col] - 8, alto - 6), formato);
                                x += anchos[col];
                            }
                            y += alto;
                            fila++;
                        }
                        e.Graphics.DrawString("Página " + pagina, fuente, Brushes.Black, margen.Left, margen.Bottom - 15);
                        e.HasMorePages = fila < tabla.Rows.Count;
                    }
                };
                vista.Document = documento;
                vista.UseAntiAlias = true;
                vista.Width = 1100;
                vista.Height = 750;
                try { vista.ShowDialog(this); }
                catch (Exception ex) { BancosMetMostrarError("No se pudo abrir la vista previa. Comprueba que haya una impresora instalada.", ex); }
            }
        }

        private static string BancosMetTextoReporte(object valor)
        {
            if (valor == null || valor == DBNull.Value) return "—";
            if (valor is DateTime) return ((DateTime)valor).ToString("dd/MM/yyyy");
            if (valor is decimal) return ((decimal)valor).ToString("F4", CultureInfo.InvariantCulture);
            return Convert.ToString(valor);
        }

        private void BancosBtnFooterAyuda_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,
                "TIPO DE CAMBIO\n\n" +
                "Tasas: elige una fecha y pulsa Consultar. Actualizar Banguat fuerza una nueva consulta.\n\n" +
                "La búsqueda de Tasas filtra por código o nombre. La categoría distingue monedas, históricas y unidades de cuenta; Mostrar sin tasa incluye las entradas sin publicación para esa fecha. En los selectores, Ctrl+F abre la búsqueda completa.\n\n" +
                "Histórico: selecciona el par y el rango de fechas. Los indicadores se calculan con esos registros.\n\n" +
                "Conversor: selecciona monedas, importe y fecha. Se usan tasas de la misma fecha; si faltan cotizaciones no se calcula.\n\n" +
                "USD/GTQ usa la referencia publicada. Los cruces de otras monedas se calculan con el promedio de compra/venta y la referencia USD/GTQ, siempre de la misma fecha. La columna Método los identifica. Para una misma moneda se aplica la identidad 1 a 1.\n\n" +
                "Actualizar Banguat renueva la pestaña abierta. Montos y tasas se presentan con cuatro decimales; el conversor utiliza la tasa aplicada que muestra. Exportar guarda CSV y Reporte abre la vista previa de impresión.\n\n" +
                "Fuente de las cotizaciones: Banco de Guatemala. Se requiere conexión a internet para nuevas consultas.",
                "Ayuda de Tipo de Cambio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BancosMetMostrarError(string mensaje, Exception ex)
        {
            if (!BancosMetCerrando()) MessageBox.Show(this, mensaje + "\n\n" + ex.Message,
                "Tipo de Cambio", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BancosBtnFooterSalir_Click(object sender, EventArgs e) { Close(); }

        private void BancosFrmTipoCambio_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cierre.Cancel();
            _consultaActuales?.Cancel();
            _consultaHistorico?.Cancel();
            _consultaConversion?.Cancel();
        }

        private void BancosFrmTipoCambio_FormClosed(object sender, FormClosedEventArgs e)
        {
            BancosPicFlagOrigen.Image = null;
            BancosPicFlagDestino.Image = null;
            foreach (Image bandera in _banderas.Values) bandera?.Dispose();
            _banderas.Clear();
            foreach (ComboBox combo in new[] { BancosCboMonedaActuales, BancosCboMonedaOrigenHist,
                BancosCboMonedaDestinoHist, BancosCboMonedaOrigenConv, BancosCboMonedaDestinoConv })
                combo.ContextMenuStrip?.Dispose();
            _informacion.Dispose();
        }
    }
}
