using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_DisponibilidadDiariaBancos;
using CapaControlador_TipoCambio;
using CapaVista_Navegador;
using CapaModelo_DisponibilidadDiariaBancos.Entidades;

namespace CapaVista_DisponibilidadDiaria.Formularios
{
    public partial class FrmDisponibilidadDiaria : Form
    {
        private readonly ClsControladorDisponibilidad _controlador;
        private readonly CancellationTokenSource _cierre = new CancellationTokenSource();
        private readonly ToolTip _informacion = new ToolTip();
        private readonly Button _registrar = new Button();
        private readonly Label _mensajeCompromisos = new Label();
        private CancellationTokenSource _consulta;
        private int _versionConsulta;
        private bool _inicializando = true, _cargandoCatalogos, _actualizandoAutomatico;
        private ClsCatalogosDisponibilidad _catalogos;
        private ClsConsultaDisponibilidad _resultado;
        private ClsResultadoActualizacionDisponibilidad _ultimoResultadoAutomatico;
        private DataTable _detalle;

        public FrmDisponibilidadDiaria() : this(new ClsControladorDisponibilidad()) { }
        public FrmDisponibilidadDiaria(ClsControladorDisponibilidad controlador)
        {
            _controlador = controlador ?? throw new ArgumentNullException(nameof(controlador));
            if (ClsNavegadorSesion.NavegadorFuncHaySesion())
                _controlador.BancosMetEstablecerUsuarioRegistro(ClsNavegadorSesion.NavegadorFuncIdUsuario(),
                    ClsNavegadorSesion.NavegadorFuncNombreUsuario());
            InitializeComponent();
            BancosMetInicializarFormulario();
            ClsControladorMonedaBanguat.BancosCatalogoMonedaActualizado += BancosCatalogoMoneda_Actualizado;
            Shown += async (s, e) =>
            {
                if (await BancosMetCargarCatalogosAsync())
                {
                    await BancosMetActualizarCalculosAsync(false);
                    await BancosMetConsultarAsync();
                }
            };
            FormClosing += (s, e) => { _cierre.Cancel(); _consulta?.Cancel(); };
            FormClosed += (s, e) =>
            {
                ClsControladorMonedaBanguat.BancosCatalogoMonedaActualizado -= BancosCatalogoMoneda_Actualizado;
                _informacion.Dispose();
            };
        }

        private void BancosCatalogoMoneda_Actualizado(object sender, EventArgs e)
        {
            if (BancosMetCerrando() || !IsHandleCreated) return;
            if (InvokeRequired)
            {
                try { BeginInvoke(new Action(() => BancosCatalogoMoneda_Actualizado(sender, e))); }
                catch (InvalidOperationException) { }
                return;
            }
            _catalogos = null;
            _registrar.Enabled = false;
            BancosMetInvalidarConsulta();
            BancosLblStatusLine.Text = "Catálogo de monedas actualizado. Pulsa Consultar para recargar las monedas y cuentas habilitadas.";
        }

        private void BancosMetInicializarFormulario()
        {
            BancosDtpFechaCorte.MaxDate = DateTime.Today;
            BancosDtpFechaCorte.Value = DateTime.Today;
            foreach (ComboBox combo in new[] { BancosCboBanco, BancosCboCuenta, BancosCboMoneda })
            {
                combo.Items.Clear();
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                combo.DisplayMember = "Nombre";
            }
            BancosCboBanco.DropDownWidth = 300;
            BancosCboCuenta.DropDownWidth = 360;
            BancosCboMoneda.DisplayMember = "Codigo";
            foreach (DataGridView grilla in new[] { BancosDgvDisponibilidad, BancosDgvCompromisos, BancosDgvResumenCuenta })
            {
                grilla.ReadOnly = true;
                grilla.AllowUserToAddRows = false;
                grilla.AllowUserToDeleteRows = false;
                grilla.AutoGenerateColumns = true;
                grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                grilla.MultiSelect = false;
                grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grilla.CellFormatting += (s, e) => { if (e.Value is decimal && (decimal)e.Value < 0) e.CellStyle.ForeColor = Color.Firebrick; };
            }
            BancosDtpFechaCorte.ValueChanged += BancosFiltros_Changed;
            BancosCboCuenta.SelectedIndexChanged += BancosFiltros_Changed;
            BancosCboMoneda.SelectedIndexChanged += BancosCboMoneda_SelectedIndexChanged;
            BancosLblEstadoConexion.Text = "Sin consulta";
            _mensajeCompromisos.Name = "BancosLblSinDesgloseCompromisos";
            _mensajeCompromisos.Location = new Point(12, 36);
            _mensajeCompromisos.Size = new Size(BancosPnlCompromisos.ClientSize.Width - 24, BancosPnlCompromisos.ClientSize.Height - 48);
            _mensajeCompromisos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _mensajeCompromisos.Font = new Font("Segoe UI", 10F);
            _mensajeCompromisos.ForeColor = Color.FromArgb(65, 83, 73);
            BancosPnlCompromisos.Controls.Add(_mensajeCompromisos);
            BancosLblMonedaResumen.AutoSize = false;
            BancosLblMonedaResumen.Location = new Point(610, 180);
            BancosLblMonedaResumen.Size = new Size(402, 16);
            BancosLblMonedaResumen.AutoEllipsis = true;
            BancosLblSubtituloFiltros.Text = "Consulta por fecha, banco, cuenta y moneda.";
            BancosLblStatusLine.AutoSize = false;
            BancosLblStatusLine.AutoEllipsis = true;
            BancosLblStatusLine.Size = new Size(820, 18);
            BancosLblCompromisosTitulo.AutoSize = false;
            BancosLblCompromisosTitulo.AutoEllipsis = true;
            BancosLblCompromisosTitulo.Size = new Size(485, 18);
            foreach (Label valor in BancosMetEtiquetasValores())
            {
                valor.AutoSize = false;
                valor.AutoEllipsis = true;
                valor.Width = Math.Max(80, valor.Parent.Width - valor.Left - 6);
                valor.Height = 23;
            }
            _registrar.Name = "BancosBtnRegistrarDisponibilidad";
            _registrar.Text = string.Empty;
            _registrar.Size = new Size(58, 58);
            _registrar.Location = new Point(857, 15);
            _registrar.BackColor = Color.Transparent;
            _registrar.BackgroundImage = global::CapaVista_DisponibilidadDiaria.Properties.Resources.btn_actualizarDisponibilidad;
            _registrar.BackgroundImageLayout = ImageLayout.Zoom;
            _registrar.Cursor = Cursors.Hand;
            _registrar.FlatAppearance.BorderSize = 0;
            _registrar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            _registrar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            _registrar.FlatStyle = FlatStyle.Flat;
            _registrar.Enabled = false;
            _registrar.Click += BancosBtnRegistrar_Click;
            _informacion.SetToolTip(_registrar, "Actualizar Disponibilidad");
            BancosPnlFiltros.Controls.Add(_registrar);
            BancosMetLimpiarConsulta("Consulta pendiente.");
            _inicializando = false;
        }

        private Label[] BancosMetEtiquetasValores()
        {
            return new[] { BancosLblValorSaldoInicial, BancosLblValorIngresos, BancosLblValorEgresos,
                BancosLblValorComprometido, BancosLblValorRestringido, BancosLblValorDisponible };
        }
        private bool BancosMetCerrando() { return IsDisposed || Disposing || _cierre.IsCancellationRequested; }
        private static long? BancosMetIdSeleccionado(ComboBox combo)
        {
            long id = (combo.SelectedItem as ClsOpcionCatalogo)?.Id ?? 0;
            return id > 0 ? (long?)id : null;
        }
        private string BancosMetMonedaSeleccionada()
        {
            var moneda = BancosCboMoneda.SelectedItem as ClsOpcionCatalogo;
            return moneda == null || moneda.Id == 0 ? null : moneda.Codigo;
        }

        private async Task<bool> BancosMetCargarCatalogosAsync()
        {
            if (_cargandoCatalogos || BancosMetCerrando()) return false;
            _cargandoCatalogos = true;
            BancosBtnConsultar.Enabled = false;
            BancosMetEstado("Cargando catálogos…", "Consultando bancos, cuentas y monedas.");
            try
            {
                var catalogos = await _controlador.BancosMetObtenerCatalogosAsync(_cierre.Token);
                if (BancosMetCerrando()) return false;
                _catalogos = catalogos;
                _inicializando = true;
                try
                {
                    BancosCboBanco.Items.Clear();
                    BancosCboBanco.Items.Add(new ClsOpcionCatalogo { Id = 0, Nombre = "Todos los bancos" });
                    BancosCboBanco.Items.AddRange(catalogos.Bancos.Cast<object>().ToArray());
                    BancosCboBanco.SelectedIndex = 0;
                    BancosCboMoneda.Items.Clear();
                    BancosCboMoneda.Items.Add(new ClsOpcionCatalogo { Id = 0, Codigo = "Todas", Nombre = "Todas" });
                    BancosCboMoneda.Items.AddRange(catalogos.Monedas.Cast<object>().ToArray());
                    BancosCboMoneda.SelectedItem = catalogos.Monedas.FirstOrDefault(m => m.Codigo == "GTQ")
                        ?? (ClsOpcionCatalogo)BancosCboMoneda.Items[0];
                    BancosMetCargarCuentas();
                }
                finally { _inicializando = false; }
                BancosMetActualizarEstadoBotonCalculo();
                BancosMetEstado("Catálogos conectados", catalogos.Mensaje);
                return true;
            }
            catch (OperationCanceledException) when (_cierre.IsCancellationRequested) { return false; }
            catch (Exception ex)
            {
                if (!BancosMetCerrando())
                {
                    BancosMetLimpiarConsulta("No se pudo conectar. Pulsa Consultar para reintentar.");
                    BancosMetEstado("Sin conexión", ex.Message);
                }
                return false;
            }
            finally { _cargandoCatalogos = false; if (!BancosMetCerrando()) BancosBtnConsultar.Enabled = true; }
        }

        private void BancosMetCargarCuentas()
        {
            long? bancoId = BancosMetIdSeleccionado(BancosCboBanco);
            long? anterior = BancosMetIdSeleccionado(BancosCboCuenta);
            string moneda = BancosMetMonedaSeleccionada();
            BancosCboCuenta.Items.Clear();
            BancosCboCuenta.Items.Add(new ClsCuentaCatalogo { Id = 0, Nombre = "Todas las cuentas" });
            if (_catalogos != null)
            {
                var cuentas = _catalogos.Cuentas.Where(c => (!bancoId.HasValue || c.BancoId == bancoId.Value)
                    && (string.IsNullOrEmpty(moneda) || c.Moneda == moneda)).ToList();
                BancosCboCuenta.Items.AddRange(cuentas.Cast<object>().ToArray());
                BancosCboCuenta.SelectedItem = cuentas.FirstOrDefault(c => c.Id == anterior);
            }
            if (BancosCboCuenta.SelectedIndex < 0) BancosCboCuenta.SelectedIndex = 0;
        }

        private void BancosCboBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_inicializando) return;
            _inicializando = true;
            try { BancosMetCargarCuentas(); } finally { _inicializando = false; }
            BancosMetInvalidarConsulta();
        }
        private void BancosCboMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            BancosCboBanco_SelectedIndexChanged(sender, e);
        }
        private void BancosFiltros_Changed(object sender, EventArgs e)
        {
            if (!_inicializando)
            {
                BancosMetActualizarEstadoBotonCalculo();
                BancosMetInvalidarConsulta();
            }
        }
        private void BancosMetActualizarEstadoBotonCalculo()
        {
            _registrar.Enabled = !_cargandoCatalogos && !_actualizandoAutomatico &&
                _catalogos != null && _catalogos.Cuentas.Count > 0 &&
                BancosDtpFechaCorte.Value.Date == DateTime.Today;
        }
        private void BancosMetInvalidarConsulta()
        {
            ++_versionConsulta;
            _consulta?.Cancel();
            BancosMetLimpiarConsulta("Filtros cambiados: pulsa Consultar.");
            BancosMetEstado("Consulta pendiente", "Consulta los filtros seleccionados para actualizar los resultados.");
            BancosBtnConsultar.Enabled = !_cargandoCatalogos;
        }
        private async void BancosBtnConsultar_Click(object sender, EventArgs e)
        {
            if (_catalogos == null && !await BancosMetCargarCatalogosAsync()) return;
            if (BancosDtpFechaCorte.Value.Date == DateTime.Today)
                await BancosMetActualizarCalculosAsync(false);
            await BancosMetConsultarAsync();
        }

        private async Task BancosMetConsultarAsync()
        {
            if (_inicializando || _catalogos == null || BancosMetCerrando()) return;
            int version = ++_versionConsulta;
            _consulta?.Cancel();
            var consulta = CancellationTokenSource.CreateLinkedTokenSource(_cierre.Token);
            _consulta = consulta;
            DateTime fecha = BancosDtpFechaCorte.Value.Date;
            long? bancoId = BancosMetIdSeleccionado(BancosCboBanco), cuentaId = BancosMetIdSeleccionado(BancosCboCuenta);
            string moneda = BancosMetMonedaSeleccionada();
            BancosMetLimpiarConsulta("Consultando disponibilidad…");
            BancosBtnConsultar.Enabled = false;
            BancosMetEstado("Consultando…", "Consultando los saldos registrados para la fecha elegida.");
            try
            {
                var resultado = await _controlador.BancosMetConsultarAsync(fecha, bancoId, cuentaId, moneda, consulta.Token);
                if (BancosMetCerrando() || version != _versionConsulta || consulta.IsCancellationRequested) return;
                _resultado = resultado;
                _detalle = BancosMetCrearTablaDetalle(resultado.Detalle);
                BancosDgvDisponibilidad.DataSource = _detalle;
                BancosMetFormatearGrilla(BancosDgvDisponibilidad);
                BancosMetActualizarResumen(resultado.Resumen);
                BancosBtnExportar.Enabled = BancosBtnFooterReporte.Enabled = _detalle.Rows.Count > 0;
                string informacion = string.IsNullOrWhiteSpace(resultado.Mensaje)
                    ? "No hay disponibilidades registradas para los filtros seleccionados." : resultado.Mensaje;
                if (fecha == DateTime.Today && _ultimoResultadoAutomatico != null)
                    informacion += " " + _ultimoResultadoAutomatico.Mensaje;
                BancosMetEstado(resultado.TieneTabla ? (_detalle.Rows.Count > 0 ? "Consulta completada" : "Sin registros")
                    : "Sin disponibilidades", informacion);
                BancosLblStatusLine.Text = _detalle.Rows.Count > 0
                    ? _detalle.Rows.Count + " cuentas · Corte " + fecha.ToString("dd/MM/yyyy") + " · " +
                        (string.IsNullOrEmpty(moneda) ? "Detalle en moneda original" : "Importes en " + moneda)
                    : informacion;
                BancosMetActualizarPanelesInferiores();
            }
            catch (OperationCanceledException) when (consulta.IsCancellationRequested) { }
            catch (Exception ex)
            {
                if (!BancosMetCerrando() && version == _versionConsulta)
                {
                    BancosMetLimpiarConsulta("Consulta fallida. Pulsa Consultar para reintentar.");
                    BancosMetEstado("Consulta fallida", ex.Message);
                }
            }
            finally
            {
                if (!BancosMetCerrando() && version == _versionConsulta) BancosBtnConsultar.Enabled = true;
                BancosMetActualizarEstadoBotonCalculo();
                if (ReferenceEquals(_consulta, consulta)) _consulta = null;
                consulta.Dispose();
            }
        }

        private static DataTable BancosMetCrearTablaDetalle(IEnumerable<ClsDisponibilidad> datos)
        {
            var tabla = new DataTable("Disponibilidad diaria");
            tabla.Columns.Add("CuentaId", typeof(long));
            tabla.Columns.Add("Fecha", typeof(DateTime));
            foreach (string nombre in new[] { "Banco", "Cuenta", "Tipo de cuenta", "Moneda" }) tabla.Columns.Add(nombre, typeof(string));
            foreach (string nombre in new[] { "Saldo inicial", "Ingresos", "Egresos", "Comprometido",
                "Restringido", "Disponible", "Disponible GTQ", "Tipo de cambio" }) tabla.Columns.Add(nombre, typeof(decimal));
            tabla.Columns.Add("Fuente", typeof(string));
            foreach (ClsDisponibilidad cuenta in datos)
                tabla.Rows.Add(cuenta.CuentaId, cuenta.FechaCorte.Date, cuenta.Banco, cuenta.NumeroCuenta,
                    cuenta.TipoCuenta, cuenta.Moneda, cuenta.SaldoInicial, cuenta.Ingresos, cuenta.Egresos,
                    cuenta.Comprometido, cuenta.Restringido, cuenta.Disponible,
                    cuenta.DisponibleGtq.HasValue ? (object)cuenta.DisponibleGtq.Value : DBNull.Value,
                    cuenta.TipoCambioAplicado.HasValue ? (object)cuenta.TipoCambioAplicado.Value : DBNull.Value,
                    BancosMetFormatearFuente(cuenta.Fuente));
            return tabla;
        }

        private static string BancosMetFormatearFuente(string fuente)
        {
            if (string.IsNullOrWhiteSpace(fuente)) return "Cálculo automático";
            string limpia = fuente.Trim();
            if (limpia.StartsWith("AUTO", StringComparison.OrdinalIgnoreCase))
                return "Cálculo automático";
            if (limpia.StartsWith("MANUAL", StringComparison.OrdinalIgnoreCase))
                return "Registro manual";
            return limpia;
        }

        private void BancosMetActualizarResumen(ClsResumenDisponibilidad resumen)
        {
            if (_resultado == null || _resultado.Detalle.Count == 0 || resumen == null)
            {
                foreach (Label valor in BancosMetEtiquetasValores()) valor.Text = "—";
                BancosLblMonedaResumen.Text = "Sin disponibilidades para la selección";
                return;
            }
            decimal?[] valores = { resumen.SaldoInicial, resumen.Ingresos, resumen.Egresos,
                resumen.Comprometido, resumen.Restringido, resumen.Disponible };
            Label[] etiquetas = BancosMetEtiquetasValores();
            for (int i = 0; i < etiquetas.Length; i++)
            {
                etiquetas[i].Text = valores[i].HasValue ? valores[i].Value.ToString("N4") : "—";
                _informacion.SetToolTip(etiquetas[i], valores[i].HasValue
                    ? valores[i].Value.ToString("N4") + " " + resumen.Moneda : "No hay equivalencia completa.");
            }
            BancosLblMonedaResumen.Text = (resumen.EsConsolidado ? "Consolidado en " : "Resumen en ") + resumen.Moneda;
            if (!resumen.ConversionCompleta)
            {
                BancosLblMonedaResumen.Text = "Consolidado no disponible: faltan tipos de cambio";
                _informacion.SetToolTip(BancosLblMonedaResumen, "Cuentas sin tasa: " + resumen.CuentasSinTasa);
            }
            BancosLblValorDisponible.ForeColor = resumen.Disponible.GetValueOrDefault() < 0
                ? Color.FromArgb(255, 210, 180) : Color.White;
        }

        private void BancosMetLimpiarConsulta(string mensaje)
        {
            _resultado = null; _detalle = null;
            BancosDgvDisponibilidad.DataSource = BancosDgvCompromisos.DataSource = BancosDgvResumenCuenta.DataSource = null;
            foreach (Label etiqueta in BancosMetEtiquetasValores()) { etiqueta.Text = "—"; _informacion.SetToolTip(etiqueta, ""); }
            BancosLblValorDisponible.ForeColor = Color.White;
            BancosLblMonedaResumen.Text = "Sin datos consultados";
            BancosLblCompromisosTitulo.Text = "Compromisos de la cuenta seleccionada";
            BancosMetMostrarCompromisos(false, "Selecciona una cuenta del detalle para revisar sus compromisos.");
            BancosLblResumenCuentaTitulo.Text = "Resumen de la cuenta";
            BancosLblStatusLine.Text = mensaje;
            BancosBtnExportar.Enabled = BancosBtnFooterReporte.Enabled = false;
        }
        private void BancosMetEstado(string estado, string detalle)
        {
            BancosLblEstadoConexion.Text = estado;
            BancosLblEstadoConexion.ForeColor = estado.IndexOf("fallida", StringComparison.OrdinalIgnoreCase) >= 0 ||
                estado == "Sin conexión" ? Color.Firebrick : Color.FromArgb(28, 74, 48);
            _informacion.SetToolTip(BancosLblEstadoConexion, detalle ?? "");
            _informacion.SetToolTip(BancosLblStatusLine, detalle ?? "");
        }
        private void BancosDgvDisponibilidad_SelectionChanged(object sender, EventArgs e)
        { if (!_inicializando) BancosMetActualizarPanelesInferiores(); }

        private void BancosMetActualizarPanelesInferiores()
        {
            BancosDgvCompromisos.DataSource = BancosDgvResumenCuenta.DataSource = null;
            BancosLblCompromisosTitulo.Text = "Compromisos de la cuenta seleccionada";
            BancosMetMostrarCompromisos(false, "Selecciona una cuenta del detalle para revisar sus compromisos.");
            BancosLblResumenCuentaTitulo.Text = "Resumen de la cuenta";
            if (_resultado == null || BancosDgvDisponibilidad.SelectedRows.Count == 0) return;
            var fila = BancosDgvDisponibilidad.SelectedRows[0].DataBoundItem as DataRowView;
            if (fila == null) return;
            long cuentaId = Convert.ToInt64(fila["CuentaId"]);
            var cuenta = _resultado.Detalle.SingleOrDefault(d => d.CuentaId == cuentaId);
            if (cuenta == null) return;
            var compromisos = new DataTable();
            compromisos.Columns.Add("Fecha", typeof(DateTime));
            foreach (string nombre in new[] { "Concepto", "Beneficiario", "Moneda" }) compromisos.Columns.Add(nombre, typeof(string));
            compromisos.Columns.Add("Monto", typeof(decimal));
            foreach (var compromiso in _resultado.Compromisos.Where(c => c.CuentaId == cuentaId).OrderBy(c => c.Fecha))
                compromisos.Rows.Add(compromiso.Fecha.Date, compromiso.Concepto, compromiso.Beneficiario, compromiso.Moneda, compromiso.Monto);
            BancosDgvCompromisos.DataSource = compromisos;
            BancosMetFormatearGrilla(BancosDgvCompromisos);
            BancosLblCompromisosTitulo.Text = compromisos.Rows.Count > 0
                ? "Compromisos · " + cuenta.NumeroCuenta : "Comprometido registrado · " + cuenta.NumeroCuenta;
            BancosMetMostrarCompromisos(compromisos.Rows.Count > 0,
                "Importe comprometido: " + cuenta.Comprometido.ToString("N4") + " " + cuenta.Moneda +
                ".\r\nNo hay un desglose registrado por beneficiario.\r\nRespaldo: " + (cuenta.Fuente ?? "Sin respaldo indicado."));
            _informacion.SetToolTip(BancosLblCompromisosTitulo,
                "El monto comprometido registrado está en el resumen. Un desglose sólo aparece si existen registros de pagos de esa cuenta.");
            var resumen = new DataTable();
            resumen.Columns.Add("Concepto", typeof(string)); resumen.Columns.Add("Monto", typeof(decimal));
            resumen.Rows.Add("Saldo inicial", cuenta.SaldoInicial); resumen.Rows.Add("Ingresos", cuenta.Ingresos);
            resumen.Rows.Add("Egresos", cuenta.Egresos); resumen.Rows.Add("Comprometido", cuenta.Comprometido);
            resumen.Rows.Add("Restringido", cuenta.Restringido); resumen.Rows.Add("Disponible", cuenta.Disponible);
            BancosDgvResumenCuenta.DataSource = resumen;
            BancosMetFormatearGrilla(BancosDgvResumenCuenta);
            BancosDgvResumenCuenta.Columns["Concepto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BancosLblResumenCuentaTitulo.Text = "Resumen de la cuenta · " + cuenta.Moneda;
            _informacion.SetToolTip(BancosLblResumenCuentaTitulo, cuenta.Banco + " · " + cuenta.NumeroCuenta + " · " + cuenta.Fuente +
                (cuenta.FechaActualizacion.HasValue ? " · Actualizado: " + cuenta.FechaActualizacion.Value.ToString("dd/MM/yyyy HH:mm") : "") +
                (string.IsNullOrWhiteSpace(cuenta.UsuarioRegistro) ? "" : " · Usuario: " + cuenta.UsuarioRegistro));
        }

        private void BancosMetMostrarCompromisos(bool tieneDesglose, string mensaje)
        {
            BancosDgvCompromisos.Visible = tieneDesglose;
            _mensajeCompromisos.Text = mensaje;
            _mensajeCompromisos.Visible = !tieneDesglose;
        }

        private static void BancosMetFormatearGrilla(DataGridView grilla)
        {
            foreach (DataGridViewColumn columna in grilla.Columns)
            {
                if (columna.Name == "CuentaId") columna.Visible = false;
                if (columna.ValueType == typeof(decimal))
                {
                    columna.DefaultCellStyle.Format = columna.Name == "Tipo de cambio" ? "F4" : "N4";
                    columna.DefaultCellStyle.NullValue = "—";
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (columna.ValueType == typeof(DateTime)) columna.DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
        private DataTable BancosMetTablaExportable()
        {
            if (_detalle == null) return null;
            var tabla = _detalle.Copy(); tabla.Columns.Remove("CuentaId"); return tabla;
        }
        private void BancosBtnExportar_Click(object sender, EventArgs e)
        {
            DataTable tabla = BancosMetTablaExportable();
            if (tabla == null || tabla.Rows.Count == 0) return;
            using (var dialogo = new SaveFileDialog { Filter = "Archivo CSV (*.csv)|*.csv",
                FileName = "Disponibilidad_" + _resultado.Fecha.ToString("yyyyMMdd") + ".csv", AddExtension = true, DefaultExt = "csv" })
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    using (var escritor = new StreamWriter(dialogo.FileName, false, new UTF8Encoding(true)))
                    {
                        escritor.WriteLine(string.Join(",", tabla.Columns.Cast<DataColumn>().Select(c => BancosMetCeldaCsv(c.ColumnName))));
                        foreach (DataRow fila in tabla.Rows) escritor.WriteLine(string.Join(",", fila.ItemArray.Select(BancosMetCeldaCsv)));
                    }
                    BancosLblStatusLine.Text = "CSV guardado: " + dialogo.FileName;
                    _informacion.SetToolTip(BancosLblStatusLine, dialogo.FileName);
                }
                catch (Exception ex) { BancosMetMostrarError("No se pudo guardar el CSV.", ex); }
            }
        }
        private void BancosBtnFooterReporte_Click(object sender, EventArgs e)
        {
            DataTable tabla = BancosMetTablaExportable();
            if (tabla != null && tabla.Rows.Count > 0)
                BancosMetVistaPrevia(tabla, "Disponibilidad diaria · " + _resultado.Fecha.ToString("dd/MM/yyyy"));
        }
        private void BancosBtnFooterAyuda_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "DISPONIBILIDAD DIARIA\n\n" +
                "Selecciona fecha, banco, cuenta y moneda; pulsa Consultar para ver el corte. La consulta de hoy recalcula primero los datos operativos.\n\n" +
                "Disponible = saldo inicial + ingresos - egresos - comprometido - restringido.\n\n" +
                "Actualizar cálculos reconstruye el día actual: apertura desde el último corte, conciliación, extracto o saldo inicial de la cuenta; ingresos y egresos desde movimientos; comprometido desde reservas y órdenes aún no ejecutadas.\n\n" +
                "El saldo inicial se registra en Mantenimiento de Cuenta Bancaria y sólo se usa antes del primer corte. Los fondos restringidos muestran cero mientras no exista un proceso operativo que los suministre.\n\n" +
                "Banguat proporciona la tasa de la fecha para moneda extranjera; GTQ utiliza 1. Los cortes anteriores son históricos y no se recalculan.\n\n" +
                "El detalle mantiene su moneda original. El consolidado en GTQ requiere tipos de cambio para todas las cuentas.\n\n" +
                "Selecciona una fila para ver su resumen y compromisos detallados disponibles. Exportar guarda CSV y Reporte abre la vista previa para imprimir.",
                "Ayuda · 8009 Disponibilidad Diaria", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BancosBtnFooterSalir_Click(object sender, EventArgs e) { Close(); }
        private void BancosMetMostrarError(string mensaje, Exception ex)
        { BancosMetEstado("Operación fallida", ex.Message); BancosLblStatusLine.Text = mensaje + " " + ex.Message; }

        private async void BancosBtnRegistrar_Click(object sender, EventArgs e)
        {
            if (await BancosMetActualizarCalculosAsync(true))
                await BancosMetConsultarAsync();
        }

        private async Task<bool> BancosMetActualizarCalculosAsync(bool informar)
        {
            if (_actualizandoAutomatico || BancosMetCerrando() || _catalogos == null ||
                BancosDtpFechaCorte.Value.Date != DateTime.Today) return false;
            _actualizandoAutomatico = true;
            BancosBtnConsultar.Enabled = false;
            BancosMetActualizarEstadoBotonCalculo();
            BancosMetEstado("Calculando…", "Reconstruyendo el corte desde movimientos, reservas, conciliaciones y extractos.");
            try
            {
                _ultimoResultadoAutomatico = null;
                if (ClsNavegadorSesion.NavegadorFuncHaySesion())
                    _controlador.BancosMetEstablecerUsuarioRegistro(ClsNavegadorSesion.NavegadorFuncIdUsuario(),
                        ClsNavegadorSesion.NavegadorFuncNombreUsuario());
                _ultimoResultadoAutomatico = await _controlador.BancosMetActualizarAutomaticamenteAsync(
                    DateTime.Today, _cierre.Token);
                BancosMetEstado("Cálculo actualizado", _ultimoResultadoAutomatico.Mensaje);
                BancosLblStatusLine.Text = _ultimoResultadoAutomatico.Mensaje;
                if (informar)
                {
                    string mensaje = _ultimoResultadoAutomatico.Mensaje;
                    if (_ultimoResultadoAutomatico.CuentasPendientes.Count > 0)
                        mensaje += "\n\nCuentas pendientes:\n• " +
                            string.Join("\n• ", _ultimoResultadoAutomatico.CuentasPendientes.ToArray());
                    MessageBox.Show(this, mensaje, "Disponibilidad diaria",
                        MessageBoxButtons.OK, _ultimoResultadoAutomatico.CuentasPendientes.Count > 0
                            ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                }
                return true;
            }
            catch (OperationCanceledException) when (_cierre.IsCancellationRequested) { return false; }
            catch (Exception ex)
            {
                if (!BancosMetCerrando()) BancosMetMostrarError("No se pudo actualizar el cálculo automático.", ex);
                return false;
            }
            finally
            {
                _actualizandoAutomatico = false;
                if (!BancosMetCerrando()) BancosBtnConsultar.Enabled = true;
                BancosMetActualizarEstadoBotonCalculo();
            }
        }

        private static string BancosMetCeldaCsv(object valor)
        {
            string texto;
            if (valor == null || valor == DBNull.Value) texto = "";
            else if (valor is DateTime) texto = ((DateTime)valor).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            else if (valor is string)
            {
                texto = (string)valor;
                string inicio = texto.TrimStart();
                if (inicio.Length > 0 && "=+-@".IndexOf(inicio[0]) >= 0) texto = "'" + texto;
            }
            else if (valor is decimal) texto = ((decimal)valor).ToString("F4", CultureInfo.InvariantCulture);
            else if (valor is IFormattable) texto = ((IFormattable)valor).ToString(null, CultureInfo.InvariantCulture);
            else texto = valor.ToString();
            return "\"" + texto.Replace("\"", "\"\"") + "\"";
        }

        private void BancosMetVistaPrevia(DataTable tabla, string titulo)
        {
            int fila = 0;
            int pagina = 0;
            DateTime fechaEmision = DateTime.Now;
            string filtros = "Banco: " + Convert.ToString(BancosCboBanco.Text) + " · Cuenta: " + Convert.ToString(BancosCboCuenta.Text) +
                " · Moneda: " + Convert.ToString(BancosCboMoneda.Text);
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
                        e.Graphics.DrawString(BancosLblMonedaResumen.Text + " · Detalle en moneda original de la cuenta", fuente, Brushes.Black, margen.Left, y);
                        y += 25;
                        e.Graphics.DrawString("Emitido: " + fechaEmision.ToString("dd/MM/yyyy HH:mm") + " · Importes y tasas con cuatro decimales", fuente, Brushes.Black, margen.Left, y);
                        y += 20;
                        float altoFiltros = e.Graphics.MeasureString(filtros, fuente, margen.Width).Height + 8;
                        e.Graphics.DrawString(filtros, fuente, Brushes.Black, new RectangleF(margen.Left, y, margen.Width, altoFiltros));
                        y += altoFiltros + 8;
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
            if (valor is decimal) return ((decimal)valor).ToString("N4");
            return Convert.ToString(valor);
        }

    }
}
