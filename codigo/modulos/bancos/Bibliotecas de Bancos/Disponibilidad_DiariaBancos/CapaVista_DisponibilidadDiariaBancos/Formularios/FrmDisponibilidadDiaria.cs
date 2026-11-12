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
using CapaVista_DisponibilidadDiaria.FrmReportes;
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
        private bool _inicializando = true, _cargandoCatalogos;
        private ClsCatalogosDisponibilidad _catalogos;
        private ClsConsultaDisponibilidad _resultado;
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
            Shown += async (s, e) => { if (await BancosMetCargarCatalogosAsync()) await BancosMetConsultarAsync(); };
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
            _registrar.Text = "Registrar / editar";
            _registrar.Size = new Size(140, 33);
            _registrar.Location = new Point(848, 25);
            _registrar.BackColor = Color.FromArgb(28, 74, 48);
            _registrar.ForeColor = Color.White;
            _registrar.FlatStyle = FlatStyle.Flat;
            _registrar.Enabled = false;
            _registrar.Click += BancosBtnRegistrar_Click;
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
                _registrar.Enabled = catalogos.Cuentas.Count > 0;
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
        private void BancosFiltros_Changed(object sender, EventArgs e) { if (!_inicializando) BancosMetInvalidarConsulta(); }
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
                    cuenta.TipoCambioAplicado.HasValue ? (object)cuenta.TipoCambioAplicado.Value : DBNull.Value, cuenta.Fuente);
            return tabla;
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
            {
                using (var reporte = new FrmReporteDisponibilidadDiaria(tabla, "Disponibilidad diaria · " + _resultado.Fecha.ToString("dd/MM/yyyy")))
                    reporte.ShowDialog(this);
            }
        }
        private void BancosBtnFooterAyuda_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "DISPONIBILIDAD DIARIA\n\n" +
                "Selecciona fecha, banco, cuenta y moneda; pulsa Consultar para ver los saldos registrados.\n\n" +
                "Disponible = saldo inicial + ingresos - egresos - comprometido - restringido.\n\n" +
                "Registrar / editar permite guardar importes por cuenta y fecha, indicando su respaldo. El registro queda identificado como manual.\n\n" +
                "Los importes y tasas se presentan con cuatro decimales. Para nuevos cortes en moneda extranjera, Banguat proporciona la tasa de la fecha seleccionada; GTQ utiliza 1. Los cortes guardados conservan su tasa aplicada.\n\n" +
                "Si modificas una captura, el formulario solicita confirmación antes de descartarla al cambiar cuenta, fecha o cerrar.\n\n" +
                "El detalle mantiene su moneda original. El consolidado en GTQ requiere tipos de cambio para todas las cuentas.\n\n" +
                "Selecciona una fila para ver su resumen y compromisos detallados disponibles. Exportar guarda CSV y Reporte abre la vista previa para imprimir.",
                "Ayuda · 8009 Disponibilidad Diaria", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BancosBtnFooterSalir_Click(object sender, EventArgs e) { Close(); }
        private void BancosMetMostrarError(string mensaje, Exception ex)
        { BancosMetEstado("Operación fallida", ex.Message); BancosLblStatusLine.Text = mensaje + " " + ex.Message; }

        private async void BancosBtnRegistrar_Click(object sender, EventArgs e)
        {
            if (_catalogos == null || _catalogos.Cuentas.Count == 0 || BancosMetCerrando()) return;
            if (ClsNavegadorSesion.NavegadorFuncHaySesion())
                _controlador.BancosMetEstablecerUsuarioRegistro(ClsNavegadorSesion.NavegadorFuncIdUsuario(),
                    ClsNavegadorSesion.NavegadorFuncNombreUsuario());
            long? cuentaId = BancosMetIdSeleccionado(BancosCboCuenta);
            var seleccionada = BancosDgvDisponibilidad.SelectedRows.Count > 0
                ? BancosDgvDisponibilidad.SelectedRows[0].DataBoundItem as DataRowView : null;
            if (seleccionada != null) cuentaId = Convert.ToInt64(seleccionada["CuentaId"]);
            var previo = cuentaId.HasValue && _resultado != null ? _resultado.Detalle.FirstOrDefault(d => d.CuentaId == cuentaId.Value) : null;
            using (var captura = new BancosFrmCapturaDisponibilidad(_controlador, _catalogos.Cuentas, BancosDtpFechaCorte.Value.Date, cuentaId, previo))
            {
                if (captura.ShowDialog(this) != DialogResult.OK || BancosMetCerrando()) return;
                long guardadaId = captura.CuentaGuardadaId; DateTime guardadaFecha = captura.FechaGuardada;
                if (!await BancosMetCargarCatalogosAsync()) return;
                _inicializando = true;
                try
                {
                    var guardada = _catalogos.Cuentas.First(c => c.Id == guardadaId);
                    BancosCboBanco.SelectedItem = _catalogos.Bancos.FirstOrDefault(b => b.Id == guardada.BancoId);
                    BancosCboMoneda.SelectedItem = _catalogos.Monedas.FirstOrDefault(m => m.Codigo == guardada.Moneda);
                    BancosMetCargarCuentas();
                    BancosCboCuenta.SelectedItem = _catalogos.Cuentas.FirstOrDefault(c => c.Id == guardadaId);
                    BancosDtpFechaCorte.Value = guardadaFecha;
                }
                finally { _inicializando = false; }
                await BancosMetConsultarAsync();
            }
        }

        private sealed class BancosFrmCapturaDisponibilidad : Form
        {
            private const string PrefijoFuente = "Registro manual - ";
            private readonly ClsControladorDisponibilidad _controlador;
            private readonly CancellationTokenSource _cierre = new CancellationTokenSource();
            private readonly ComboBox _cuenta = new ComboBox();
            private readonly DateTimePicker _fecha = new DateTimePicker();
            private readonly NumericUpDown[] _importes = new NumericUpDown[5];
            private readonly NumericUpDown _tasa = new NumericUpDown();
            private readonly TextBox _fuente = new TextBox();
            private readonly Label _moneda = new Label(), _unidad = new Label(), _preview = new Label(), _estado = new Label();
            private readonly Button _guardar = new Button(), _reintentar = new Button();
            private readonly Label _resultadoGrande = new Label(), _resumenMoneda = new Label(), _resumenTasa = new Label(), _resumenOrigen = new Label();
            private readonly ToolTip _informacion = new ToolTip();
            private CancellationTokenSource _carga;
            private int _versionCarga;
            private bool _actualizando = true, _guardando;
            private bool _tasaLista, _tasaGuardada, _reintentarSoloTasa;
            private long _cuentaDeTasa;
            private string _monedaDeTasa, _detalleTasa;
            private DateTime _fechaDeTasa;
            private decimal _tasaAprobada;
            private ClsCuentaCatalogo _cuentaAceptada;
            private DateTime _fechaAceptada, _fechaGeneracionOriginal;
            private readonly decimal[] _importesOriginales = new decimal[5];
            private string _fuenteOriginal = "";
            private bool _registroExistente, _guardado, _registroEnConflicto;
            private long _versionRegistroOriginal;
            public long CuentaGuardadaId { get; private set; }
            public DateTime FechaGuardada { get; private set; }

            public BancosFrmCapturaDisponibilidad(ClsControladorDisponibilidad controlador,
                List<ClsCuentaCatalogo> cuentas, DateTime fecha, long? cuentaId, ClsDisponibilidad previo)
            {
                _controlador = controlador;
                Text = "8009 - Registro de disponibilidad";
                Name = "BancosFrmCapturaDisponibilidad";
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.Sizable;
                MaximizeBox = true;
                MinimizeBox = false;
                ShowInTaskbar = false;
                ClientSize = new Size(1000, 790);
                MinimumSize = new Size(840, 650);
                Font = new Font("Segoe UI", 9F);
                BackColor = Color.FromArgb(236, 247, 239);
                BackgroundImage = Properties.Resources.FondoDiponibilidadDiaria;
                BackgroundImageLayout = ImageLayout.Stretch;
                DoubleBuffered = true;

                var raiz = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill, BackColor = Color.Transparent, ColumnCount = 1,
                    RowCount = 3, Margin = Padding.Empty, Padding = Padding.Empty
                };
                raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
                raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 74));
                Controls.Add(raiz);

                var cabecera = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill, BackColor = Color.Transparent, ColumnCount = 1,
                    RowCount = 1, Padding = new Padding(18, 13, 18, 11), Margin = Padding.Empty
                };
                cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                cabecera.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                var titulos = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, BackColor = Color.Transparent, Margin = Padding.Empty };
                titulos.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
                titulos.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
                var titulo = new Label
                {
                    Text = "Registro de disponibilidad", Dock = DockStyle.Fill, Margin = Padding.Empty,
                    ForeColor = Color.FromArgb(20, 65, 45), Font = new Font("Georgia", 26F, FontStyle.Bold),
                    AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft
                };
                titulos.Controls.Add(titulo, 0, 0);
                titulos.Controls.Add(new Label
                {
                    Text = "ERP Tesorería y Bancos — Embutidos de Calidad S.A.", Dock = DockStyle.Fill,
                    ForeColor = Color.FromArgb(83, 104, 92), AutoEllipsis = true, Margin = Padding.Empty,
                    Font = new Font("Segoe UI", 9.5F), TextAlign = ContentAlignment.MiddleLeft
                }, 0, 1);
                cabecera.Controls.Add(titulos, 0, 0);
                cabecera.Paint += (s, e) => e.Graphics.DrawLine(Pens.DarkGreen, 0, cabecera.Height - 1, cabecera.Width, cabecera.Height - 1);
                raiz.Controls.Add(cabecera, 0, 0);

                var cuerpo = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Color.Transparent,
                    Padding = new Padding(12, 12, 12, 0), Margin = Padding.Empty
                };
                cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67));
                cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                raiz.Controls.Add(cuerpo, 0, 1);
                var panelDatos = new Panel
                {
                    Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(15, 10, 15, 8), Margin = new Padding(0, 0, 8, 0), AutoScroll = true
                };
                cuerpo.Controls.Add(panelDatos, 0, 0);
                var datos = new TableLayoutPanel
                {
                    Dock = DockStyle.Top, Height = 551, ColumnCount = 1, RowCount = 15,
                    BackColor = Color.White, Margin = Padding.Empty, Padding = Padding.Empty
                };
                datos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                int[] alturas = { 29, 17, 30, 17, 29, 8, 37, 37, 37, 37, 37, 46, 25, 29, 36 };
                foreach (int alto in alturas) datos.RowStyles.Add(new RowStyle(SizeType.Absolute, alto));
                panelDatos.Controls.Add(datos);
                datos.Controls.Add(new Label
                {
                    Text = "Datos del registro", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(23, 75, 53), Margin = Padding.Empty
                }, 0, 0);
                datos.Controls.Add(BancosMetCrearEtiqueta("CUENTA BANCARIA"), 0, 1);
                _cuenta.Name = "BancosCboCuentaCaptura";
                _cuenta.Dock = DockStyle.Fill;
                _cuenta.DropDownStyle = ComboBoxStyle.DropDownList;
                _cuenta.DropDownWidth = 580;
                _cuenta.DisplayMember = "Nombre";
                _cuenta.Items.AddRange(cuentas.Cast<object>().ToArray());
                _cuenta.SelectedItem = cuentas.FirstOrDefault(c => c.Id == cuentaId) ?? cuentas.FirstOrDefault();
                _cuenta.Margin = new Padding(0, 2, 0, 4);
                datos.Controls.Add(_cuenta, 0, 2);
                var etiquetasFecha = BancosMetDosColumnas();
                etiquetasFecha.Controls.Add(BancosMetCrearEtiqueta("FECHA DE CORTE"), 0, 0);
                etiquetasFecha.Controls.Add(BancosMetCrearEtiqueta("MONEDA"), 1, 0);
                datos.Controls.Add(etiquetasFecha, 0, 3);
                var valoresFecha = BancosMetDosColumnas();
                _fecha.Name = "BancosDtpFechaCaptura";
                _fecha.Format = DateTimePickerFormat.Custom;
                _fecha.CustomFormat = "dd/MM/yyyy";
                _fecha.MaxDate = DateTime.Today;
                _fecha.Value = fecha;
                _fecha.Dock = DockStyle.Fill;
                _fecha.Margin = new Padding(0, 1, 12, 3);
                valoresFecha.Controls.Add(_fecha, 0, 0);
                _moneda.Dock = DockStyle.Fill;
                _moneda.BackColor = Color.FromArgb(244, 246, 244);
                _moneda.BorderStyle = BorderStyle.FixedSingle;
                _moneda.TextAlign = ContentAlignment.MiddleLeft;
                _moneda.Padding = new Padding(8, 0, 0, 0);
                _moneda.Margin = new Padding(0, 1, 0, 3);
                valoresFecha.Controls.Add(_moneda, 1, 0);
                datos.Controls.Add(valoresFecha, 0, 4);

                string[] conceptos = { "Saldo inicial", "Ingresos", "Egresos", "Comprometido", "Restringido" };
                string[] nombres = { "SaldoInicial", "Ingresos", "Egresos", "Comprometido", "Restringido" };
                Image[] iconos = { Properties.Resources.kpiSaldoInicial, Properties.Resources.kpiIngresos,
                    Properties.Resources.kpiEgresos, Properties.Resources.kpiComprometido, Properties.Resources.kpiRestringido };
                for (int i = 0; i < conceptos.Length; i++)
                {
                    var fila = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Margin = Padding.Empty };
                    fila.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34));
                    fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
                    fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
                    fila.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                    fila.Controls.Add(new PictureBox { Image = iconos[i], SizeMode = PictureBoxSizeMode.Zoom,
                        Size = new Size(25, 25), Anchor = AnchorStyles.None, Margin = new Padding(0, 0, 7, 0) }, 0, 0);
                    fila.Controls.Add(new Label { Text = conceptos[i], Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Margin = Padding.Empty }, 1, 0);
                    _importes[i] = BancosMetNumero("BancosNum" + nombres[i] + "Captura", 0, 0);
                    _importes[i].Dock = DockStyle.None;
                    _importes[i].Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    _importes[i].Margin = new Padding(4, 0, 0, 0);
                    _importes[i].ValueChanged += (s, e) => BancosMetPrevisualizar();
                    fila.Controls.Add(_importes[i], 2, 0);
                    datos.Controls.Add(fila, 0, 6 + i);
                }
                _informacion.SetToolTip(_importes[2], "Salidas ya realizadas. No incluyas aquí los pagos aún comprometidos.");
                _informacion.SetToolTip(_importes[3], "Fondos para pagos pendientes que todavía no se han descontado como egresos.");
                _informacion.SetToolTip(_importes[4], "Reservas o fondos bloqueados que no estén descontados en los otros importes.");
                var filaTasa = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Margin = Padding.Empty };
                filaTasa.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43));
                filaTasa.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
                filaTasa.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27));
                filaTasa.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                filaTasa.Controls.Add(BancosMetCrearEtiqueta("TIPO DE CAMBIO"), 0, 0);
                BancosMetConfigurarNumero(_tasa, "BancosNumTipoCambioCaptura", 0, 0);
                _tasa.ReadOnly = true;
                _tasa.Enabled = false;
                _tasa.Dock = DockStyle.None;
                _tasa.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                _tasa.Margin = new Padding(0, 0, 7, 0);
                _tasa.ValueChanged += (s, e) => BancosMetPrevisualizar();
                filaTasa.Controls.Add(_tasa, 1, 0);
                _unidad.Dock = DockStyle.Fill;
                _unidad.TextAlign = ContentAlignment.MiddleLeft;
                _unidad.AutoEllipsis = true;
                _unidad.Margin = Padding.Empty;
                filaTasa.Controls.Add(_unidad, 2, 0);
                datos.Controls.Add(filaTasa, 0, 11);
                datos.Controls.Add(BancosMetCrearEtiqueta("FUENTE / RESPALDO DEL SALDO"), 0, 12);
                _fuente.Name = "BancosTxtFuenteCaptura";
                _fuente.Dock = DockStyle.Fill;
                _fuente.Margin = new Padding(0, 1, 0, 2);
                _fuente.MaxLength = 100 - PrefijoFuente.Length;
                datos.Controls.Add(_fuente, 0, 13);
                datos.Controls.Add(new Label
                {
                    Text = "Ej.: extracto bancario o cuadre de contabilidad. Este campo documenta el origen.",
                    Dock = DockStyle.Fill, ForeColor = Color.DimGray, Margin = new Padding(0, 2, 0, 0),
                    Font = new Font("Segoe UI", 8F)
                }, 0, 14);

                var lateral = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Margin = new Padding(4, 0, 0, 0), BackColor = Color.Transparent };
                lateral.RowStyles.Add(new RowStyle(SizeType.Absolute, 188));
                lateral.RowStyles.Add(new RowStyle(SizeType.Absolute, 122));
                lateral.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                cuerpo.Controls.Add(lateral, 1, 0);
                var tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(23, 75, 53), Padding = new Padding(14), Margin = new Padding(0, 0, 0, 10) };
                var tarjetaContenido = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = Color.Transparent, Margin = Padding.Empty };
                tarjetaContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 33));
                tarjetaContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 49));
                tarjetaContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                tarjetaContenido.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                var disponibleTitulo = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1,
                    BackColor = Color.Transparent, Margin = Padding.Empty
                };
                disponibleTitulo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 32));
                disponibleTitulo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                disponibleTitulo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                disponibleTitulo.Controls.Add(new PictureBox
                {
                    Image = Properties.Resources.kpiDisponible, SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(20, 20), Anchor = AnchorStyles.None, Margin = Padding.Empty
                }, 0, 0);
                disponibleTitulo.Controls.Add(new Label
                {
                    Text = "DISPONIBLE", Dock = DockStyle.Fill, ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    Margin = Padding.Empty
                }, 1, 0);
                tarjetaContenido.Controls.Add(disponibleTitulo, 0, 0);
                _resultadoGrande.Name = "BancosLblDisponibleGrandeCaptura";
                _resultadoGrande.Dock = DockStyle.Fill;
                _resultadoGrande.Font = new Font("Segoe UI", 21F, FontStyle.Bold);
                _resultadoGrande.ForeColor = Color.White;
                _resultadoGrande.TextAlign = ContentAlignment.MiddleLeft;
                _resultadoGrande.AutoEllipsis = true;
                _resultadoGrande.Margin = Padding.Empty;
                tarjetaContenido.Controls.Add(_resultadoGrande, 0, 1);
                tarjetaContenido.Controls.Add(new Label { Text = "Saldo final operativo", Dock = DockStyle.Fill, ForeColor = Color.FromArgb(185, 220, 199), Margin = Padding.Empty }, 0, 2);
                _preview.Name = "BancosLblPrevisualizacionCaptura";
                _preview.Dock = DockStyle.Fill;
                _preview.Font = new Font("Segoe UI", 8F);
                _preview.ForeColor = Color.FromArgb(203, 231, 213);
                _preview.Margin = new Padding(0, 2, 0, 0);
                tarjetaContenido.Controls.Add(_preview, 0, 3);
                tarjeta.Controls.Add(tarjetaContenido);
                tarjeta.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 4, BackColor = Color.FromArgb(197, 165, 42) });
                lateral.Controls.Add(tarjeta, 0, 0);
                var resumen = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(232, 246, 236), BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(12, 10, 12, 8), Margin = new Padding(0, 0, 0, 10) };
                var resumenFilas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Margin = Padding.Empty };
                foreach (Label etiqueta in new[] { _resumenMoneda, _resumenTasa, _resumenOrigen })
                {
                    etiqueta.Dock = DockStyle.Fill;
                    etiqueta.TextAlign = ContentAlignment.MiddleLeft;
                    etiqueta.AutoEllipsis = true;
                    etiqueta.Margin = Padding.Empty;
                    etiqueta.ForeColor = Color.FromArgb(23, 75, 53);
                    etiqueta.Font = new Font("Segoe UI", 10F);
                    resumenFilas.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333f));
                    resumenFilas.Controls.Add(etiqueta);
                }
                _resumenOrigen.Text = "Origen: Registro manual";
                resumen.Controls.Add(resumenFilas);
                lateral.Controls.Add(resumen, 0, 1);
                var mascota = new PictureBox { Name = "BancosPicMascotaRegistro", Dock = DockStyle.Fill, Image = Properties.Resources._11, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Margin = Padding.Empty };
                lateral.Controls.Add(mascota, 0, 2);

                var pie = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill, BackColor = Color.Transparent, ColumnCount = 3, RowCount = 1,
                    Padding = new Padding(14, 12, 14, 12), Margin = Padding.Empty
                };
                pie.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                pie.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
                pie.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 225));
                pie.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                var estadoPanel = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 8, 0), BackColor = Color.Transparent };
                _estado.Name = "BancosLblEstadoCaptura";
                _estado.Dock = DockStyle.Fill;
                _estado.TextAlign = ContentAlignment.MiddleLeft;
                _estado.ForeColor = Color.FromArgb(111, 74, 17);
                _estado.AutoEllipsis = false;
                _reintentar.Name = "BancosBtnReintentarCaptura";
                _reintentar.Text = "Reintentar";
                _reintentar.Dock = DockStyle.Right;
                _reintentar.Width = 96;
                _reintentar.Visible = false;
                _reintentar.Click += async (s, e) =>
                {
                    if (_reintentarSoloTasa) await BancosMetReintentarTasaAsync();
                    else if (BancosMetConfirmarDescartarCambios()) await BancosMetCargarRegistroAsync();
                };
                estadoPanel.Controls.Add(_estado);
                estadoPanel.Controls.Add(_reintentar);
                pie.Controls.Add(estadoPanel, 0, 0);
                var cancelar = new Button { Name = "BancosBtnCancelarCaptura", Text = "Cancelar", DialogResult = DialogResult.Cancel };
                BancosMetEstiloAccion(cancelar, false);
                _guardar.Name = "BancosBtnGuardarCaptura";
                _guardar.Text = "Guardar disponibilidad";
                _guardar.Enabled = false;
                BancosMetEstiloAccion(_guardar, true);
                _guardar.Click += BancosBtnGuardar_Click;
                pie.Controls.Add(cancelar, 1, 0);
                pie.Controls.Add(_guardar, 2, 0);
                raiz.Controls.Add(pie, 0, 2);
                AcceptButton = _guardar;
                CancelButton = cancelar;
                _cuenta.SelectedIndexChanged += BancosCapturaFiltros_Changed;
                _fecha.ValueChanged += BancosCapturaFiltros_Changed;
                _actualizando = false;
                BancosMetLimpiarImportes();
                if (previo != null && previo.CuentaId == (_cuenta.SelectedItem as ClsCuentaCatalogo)?.Id
                    && previo.FechaCorte.Date == fecha.Date
                    && previo.Moneda == (_cuenta.SelectedItem as ClsCuentaCatalogo)?.Moneda)
                    BancosMetAplicarRegistro(previo);
                BancosMetMarcarCapturaSinCambios();
                Load += (s, e) =>
                {
                    if (Owner != null) Icon = Owner.Icon;
                    Rectangle area = Screen.FromControl(this).WorkingArea;
                    MinimumSize = new Size(Math.Min(840, area.Width - 24), Math.Min(650, area.Height - 24));
                    Size = new Size(Math.Min(Width, area.Width - 24), Math.Min(Height, area.Height - 24));
                };
                Shown += async (s, e) => await BancosMetCargarRegistroAsync();
                FormClosing += BancosCaptura_FormClosing;
                FormClosed += (s, e) => _informacion.Dispose();
            }

            private static Label BancosMetCrearEtiqueta(string texto)
            {
                return new Label { Text = texto, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.FromArgb(65, 83, 73), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Margin = Padding.Empty };
            }

            private static TableLayoutPanel BancosMetDosColumnas()
            {
                var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = Padding.Empty };
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                return panel;
            }

            private static void BancosMetEstiloAccion(Button boton, bool guardar)
            {
                boton.Dock = DockStyle.Fill;
                boton.Margin = new Padding(7, 2, 0, 2);
                boton.FlatStyle = FlatStyle.Flat;
                boton.FlatAppearance.BorderColor = Color.FromArgb(197, 165, 42);
                boton.FlatAppearance.BorderSize = 2;
                boton.BackColor = Color.FromArgb(23, 75, 53);
                boton.ForeColor = Color.White;
                boton.Font = new Font("Segoe UI", 9F);
                boton.Padding = new Padding(26, 0, 4, 0);
                boton.Paint += (s, e) =>
                {
                    using (var linea = new Pen(Color.White, 1.7f))
                    {
                        int x = 12, y = (boton.Height - 18) / 2;
                        if (guardar)
                        {
                            e.Graphics.DrawRectangle(linea, x, y, 17, 18);
                            e.Graphics.DrawRectangle(linea, x + 4, y + 1, 8, 5);
                            e.Graphics.DrawRectangle(linea, x + 4, y + 11, 9, 6);
                        }
                        else
                        {
                            e.Graphics.DrawRectangle(linea, x + 4, y, 13, 18);
                            e.Graphics.DrawLine(linea, x - 2, y + 9, x + 11, y + 9);
                            e.Graphics.DrawLine(linea, x - 2, y + 9, x + 3, y + 4);
                            e.Graphics.DrawLine(linea, x - 2, y + 9, x + 3, y + 14);
                        }
                    }
                };
            }
            private void BancosMetEtiqueta(string texto, int x, int y)
            {
                Controls.Add(new Label { Text = texto, Location = new Point(x, y), Size = new Size(180, 23) });
            }
            private NumericUpDown BancosMetNumero(string nombre, int x, int y)
            {
                var numero = new NumericUpDown();
                BancosMetConfigurarNumero(numero, nombre, x, y);
                return numero;
            }
            private void BancosMetConfigurarNumero(NumericUpDown numero, string nombre, int x, int y)
            {
                numero.Name = nombre;
                numero.DecimalPlaces = 4;
                numero.Minimum = 0m;
                numero.Maximum = 99999999.9999m;
                numero.Increment = 0.01m;
                numero.ThousandsSeparator = true;
                numero.TextAlign = HorizontalAlignment.Right;
                numero.Location = new Point(x, y);
                numero.Size = new Size(180, 25);
                Controls.Add(numero);
            }
            private async void BancosCapturaFiltros_Changed(object sender, EventArgs e)
            {
                if (_actualizando || _guardando) return;
                if (!BancosMetConfirmarDescartarCambios())
                {
                    _actualizando = true;
                    try { _cuenta.SelectedItem = _cuentaAceptada; _fecha.Value = _fechaAceptada; }
                    finally { _actualizando = false; }
                    if (!_tasaLista && _reintentarSoloTasa && _carga == null)
                        await BancosMetReintentarTasaAsync();
                    return;
                }
                await BancosMetCargarRegistroAsync();
            }
            private bool BancosMetTieneCambiosSinGuardar()
            {
                return !_guardado && (_fuente.Text != _fuenteOriginal ||
                    _importes.Where((numero, indice) => numero.Value != _importesOriginales[indice]).Any());
            }
            private bool BancosMetConfirmarDescartarCambios()
            {
                return !BancosMetTieneCambiosSinGuardar() || MessageBox.Show(this,
                    "Tienes cambios sin guardar en esta captura. ¿Deseas descartarlos?",
                    "Cambios sin guardar", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            }
            private void BancosMetMarcarCapturaSinCambios()
            {
                for (int i = 0; i < _importes.Length; i++) _importesOriginales[i] = _importes[i].Value;
                _fuenteOriginal = _fuente.Text;
                _cuentaAceptada = _cuenta.SelectedItem as ClsCuentaCatalogo;
                _fechaAceptada = _fecha.Value.Date;
            }
            private void BancosCaptura_FormClosing(object sender, FormClosingEventArgs e)
            {
                if (_guardando && !_guardado && e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    DialogResult = DialogResult.None;
                    _estado.Text = "Espera a que termine el guardado antes de cerrar.";
                    return;
                }
                if (!_guardado && !BancosMetConfirmarDescartarCambios())
                {
                    e.Cancel = true;
                    DialogResult = DialogResult.None;
                    return;
                }
                _cierre.Cancel();
                _carga?.Cancel();
            }
            private void BancosMetLimpiarImportes()
            {
                _actualizando = true;
                try
                {
                    foreach (NumericUpDown numero in _importes) numero.Value = 0m;
                    _fuente.Text = "";
                    _registroExistente = false;
                    _registroEnConflicto = false;
                    _versionRegistroOriginal = 0;
                    _fechaGeneracionOriginal = default(DateTime);
                    _informacion.SetToolTip(_resumenOrigen, "Origen: Registro manual de importes respaldados por documentos.");
                    var cuenta = _cuenta.SelectedItem as ClsCuentaCatalogo;
                    BancosMetInvalidarTasa();
                    _moneda.Text = cuenta == null ? "Selecciona una cuenta." : "Moneda de la cuenta: " + cuenta.Moneda;
                    _unidad.Text = cuenta == null ? "" : "GTQ por 1 " + cuenta.Moneda;
                }
                finally { _actualizando = false; }
                BancosMetMarcarCapturaSinCambios();
                BancosMetPrevisualizar();
            }
            private void BancosMetAplicarRegistro(ClsDisponibilidad registro)
            {
                _actualizando = true;
                try
                {
                    _registroExistente = true;
                    _registroEnConflicto = false;
                    _versionRegistroOriginal = registro.VersionRegistro;
                    _fechaGeneracionOriginal = registro.FechaGeneracion;
                    string ultimaEdicion = registro.FechaActualizacion.HasValue
                        ? registro.FechaActualizacion.Value.ToString("dd/MM/yyyy HH:mm") : registro.FechaGeneracion.ToString("dd/MM/yyyy HH:mm");
                    _informacion.SetToolTip(_resumenOrigen, "Origen: Registro manual. Último registro: " + ultimaEdicion +
                        (string.IsNullOrWhiteSpace(registro.UsuarioRegistro) ? ". Usuario no informado en el registro." : ". Usuario: " + registro.UsuarioRegistro + "."));
                    decimal[] valores = { registro.SaldoInicial, registro.Ingresos, registro.Egresos,
                        registro.Comprometido, registro.Restringido };
                    for (int i = 0; i < valores.Length; i++) _importes[i].Value = valores[i];
                    string fuente = registro.Fuente ?? "";
                    _fuente.Text = fuente.StartsWith(PrefijoFuente, StringComparison.OrdinalIgnoreCase)
                        ? fuente.Substring(PrefijoFuente.Length) : fuente;
                    BancosMetInvalidarTasa();
                    decimal tasa = registro.TipoCambioAplicado.GetValueOrDefault();
                    if (BancosMetCodigoMoneda(registro.Moneda) == "GTQ" && !registro.TipoCambioAplicado.HasValue) tasa = 1m;
                    if (tasa > 0m && tasa <= _tasa.Maximum && decimal.Round(tasa, 4) == tasa)
                        BancosMetAceptarTasa(registro.CuentaId, BancosMetCodigoMoneda(registro.Moneda),
                            registro.FechaCorte.Date, tasa, true,
                            "Tasa guardada con el corte del " + registro.FechaCorte.ToString("dd/MM/yyyy") +
                            ". Se conserva para no recalcular el registro histórico.");
                }
                finally { _actualizando = false; }
                BancosMetMarcarCapturaSinCambios();
                BancosMetPrevisualizar();
            }
            private static string BancosMetCodigoMoneda(string moneda)
            {
                return (moneda ?? "").Trim().ToUpperInvariant();
            }
            private void BancosMetInvalidarTasa()
            {
                _tasaLista = _tasaGuardada = false;
                _cuentaDeTasa = 0;
                _monedaDeTasa = _detalleTasa = null;
                _fechaDeTasa = default(DateTime);
                _tasaAprobada = 0m;
                _tasa.Value = 0m;
                _tasa.ReadOnly = true;
                _tasa.Enabled = false;
                _guardar.Enabled = false;
                _informacion.SetToolTip(_tasa, "La tasa se obtiene automáticamente para la fecha de corte.");
                _informacion.SetToolTip(_resumenTasa, "Cotización pendiente para la cuenta y fecha seleccionadas.");
                BancosMetPrevisualizar();
            }
            private void BancosMetAceptarTasa(long cuentaId, string moneda, DateTime fecha, decimal tasa,
                bool guardada, string detalle)
            {
                _cuentaDeTasa = cuentaId;
                _monedaDeTasa = moneda;
                _fechaDeTasa = fecha.Date;
                _tasaAprobada = tasa;
                _tasaGuardada = guardada;
                _detalleTasa = detalle;
                _tasaLista = true;
                _tasa.Value = tasa;
                _tasa.ReadOnly = true;
                _tasa.Enabled = false;
                _informacion.SetToolTip(_tasa, detalle);
                _informacion.SetToolTip(_resumenTasa, detalle);
                BancosMetPrevisualizar();
            }
            private bool BancosMetTasaVigente()
            {
                var cuenta = _cuenta.SelectedItem as ClsCuentaCatalogo;
                return _tasaLista && cuenta != null && _cuentaDeTasa == cuenta.Id
                    && _monedaDeTasa == BancosMetCodigoMoneda(cuenta.Moneda)
                    && _fechaDeTasa == _fecha.Value.Date && _tasaAprobada > 0m
                    && _tasa.Value == _tasaAprobada;
            }
            private bool BancosMetCargaActual(long cuentaId, string moneda, DateTime fecha, int version, CancellationToken token)
            {
                var cuenta = _cuenta.SelectedItem as ClsCuentaCatalogo;
                return !IsDisposed && !_cierre.IsCancellationRequested && !token.IsCancellationRequested
                    && version == _versionCarga && cuenta != null && cuenta.Id == cuentaId
                    && BancosMetCodigoMoneda(cuenta.Moneda) == moneda && _fecha.Value.Date == fecha.Date;
            }
            private void BancosMetPrevisualizar()
            {
                if (_actualizando) return;
                string moneda = (_cuenta.SelectedItem as ClsCuentaCatalogo)?.Moneda ?? "";
                bool tasaVigente = BancosMetTasaVigente();
                decimal disponible = _importes[0].Value + _importes[1].Value - _importes[2].Value
                    - _importes[3].Value - _importes[4].Value;
                _preview.Text = "Disponible: " + disponible.ToString("N4") + " " + moneda +
                    (tasaVigente ? "    ·    GTQ: " + (disponible * _tasa.Value).ToString("N4") : "    ·    Cotización pendiente.");
                _preview.ForeColor = disponible < 0 ? Color.FromArgb(255, 210, 180) : Color.FromArgb(203, 231, 213);
                string prefijo = moneda == "GTQ" ? "Q " : moneda + " ";
                _resultadoGrande.Text = prefijo + disponible.ToString("N4");
                _resultadoGrande.ForeColor = disponible < 0 ? Color.FromArgb(255, 210, 180) : Color.White;
                _resumenMoneda.Text = "Moneda: " + (moneda.Length == 0 ? "—" : moneda);
                string origenTasa = _tasaGuardada ? "Tasa guardada: " : (BancosMetCodigoMoneda(moneda) == "GTQ" ? "Tasa aplicada: " : "Tasa Banguat: ");
                _resumenTasa.Text = tasaVigente ? origenTasa + _tasa.Value.ToString("N4") : "Tasa aplicada: Pendiente";
                _informacion.SetToolTip(_resultadoGrande, disponible.ToString("N4") + " " + moneda);
            }
            private async Task BancosMetConsultarTasaAsync(long cuentaId, string moneda, DateTime fecha, int version,
                CancellationToken token)
            {
                if (!BancosMetCargaActual(cuentaId, moneda, fecha, version, token)) return;
                BancosMetInvalidarTasa();
                _reintentarSoloTasa = true;
                _reintentar.Visible = false;
                _reintentar.Text = "Reintentar";
                _estado.Text = moneda == "GTQ" ? "Aplicando tasa GTQ…" : "Consultando Banguat para " + fecha.ToString("dd/MM/yyyy") + "…";
                _estado.ForeColor = Color.FromArgb(23, 75, 53);
                _informacion.SetToolTip(_estado, _estado.Text);
                try
                {
                    if (moneda == "GTQ")
                    {
                        BancosMetAceptarTasa(cuentaId, moneda, fecha, 1m, false, "1 GTQ = 1 GTQ. No requiere consulta a Banguat.");
                    }
                    else
                    {
                        var cotizacion = await _controlador.BancosMetObtenerTipoCambioAsync(moneda, fecha, token);
                        if (!BancosMetCargaActual(cuentaId, moneda, fecha, version, token)) return;
                        if (cotizacion == null || BancosMetCodigoMoneda(cotizacion.Moneda) != moneda || cotizacion.Fecha.Date != fecha.Date)
                            throw new InvalidOperationException("Banguat no devolvió una cotización de " + moneda + " para la fecha de corte seleccionada.");
                        if (cotizacion.TasaAplicada <= 0m || cotizacion.TasaAplicada > _tasa.Maximum
                            || decimal.Round(cotizacion.TasaAplicada, 4) != cotizacion.TasaAplicada)
                            throw new InvalidOperationException("La cotización recibida no se puede aplicar con los cuatro decimales del registro.");
                        string detalle = "Fuente de la tasa: " + cotizacion.Fuente + ". Fecha: " + cotizacion.Fecha.ToString("dd/MM/yyyy") +
                            ". " + cotizacion.Metodo + ". Tasa aplicada: " + cotizacion.TasaAplicada.ToString("N4") +
                            " GTQ por 1 " + moneda + ". Redondeada a cuatro decimales según el estándar del registro.";
                        BancosMetAceptarTasa(cuentaId, moneda, fecha, cotizacion.TasaAplicada, false, detalle);
                    }
                    _estado.Text = moneda == "GTQ" ? "Nuevo registro. Tasa GTQ: 1,0000." : "Tasa Banguat lista para el " + fecha.ToString("dd/MM/yyyy") + ".";
                    _informacion.SetToolTip(_estado, _detalleTasa);
                    _guardar.Enabled = BancosMetTasaVigente() && !_guardando;
                    _reintentarSoloTasa = false;
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested) { }
                catch (Exception ex)
                {
                    if (!BancosMetCargaActual(cuentaId, moneda, fecha, version, token)) return;
                    BancosMetInvalidarTasa();
                    _estado.Text = BancosMetExplicarErrorTasa(ex, moneda, fecha);
                    _estado.ForeColor = Color.Firebrick;
                    _informacion.SetToolTip(_estado, "No se puede guardar sin una tasa de " + moneda + " del " + fecha.ToString("dd/MM/yyyy") + ". " + ex.Message);
                    _reintentarSoloTasa = true;
                    _reintentar.Visible = true;
                }
            }
            private static string BancosMetExplicarErrorTasa(Exception error, string moneda, DateTime fecha)
            {
                string mensaje = error.Message ?? "";
                if (error is TimeoutException)
                    return "Banguat tardó demasiado en responder. Pulsa Reintentar.";
                if (error is System.Net.Http.HttpRequestException || error is System.Net.WebException)
                    return "No se pudo conectar con Banguat. Revisa la conexión y reintenta.";
                if (mensaje.IndexOf("no publicó", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    mensaje.IndexOf("no hay cotizaciones", StringComparison.OrdinalIgnoreCase) >= 0)
                    return "Sin publicación de " + moneda + " para el " + fecha.ToString("dd/MM/yyyy") + ". Elige una fecha con cotización.";
                if (mensaje.IndexOf("no está disponible en el servicio", StringComparison.OrdinalIgnoreCase) >= 0)
                    return "La moneda " + moneda + " no está disponible en Banguat. Revisa la moneda de la cuenta.";
                if (mensaje.IndexOf("precisión", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    mensaje.IndexOf("cuatro decimales", StringComparison.OrdinalIgnoreCase) >= 0)
                    return "La cotización recibida no se puede aplicar con cuatro decimales. No se puede guardar.";
                return "No se pudo obtener una cotización válida de " + moneda + ". Pulsa Reintentar.";
            }
            private async Task BancosMetReintentarTasaAsync()
            {
                if (_guardando || _cierre.IsCancellationRequested || IsDisposed) return;
                var cuenta = _cuenta.SelectedItem as ClsCuentaCatalogo;
                if (cuenta == null) return;
                int version = ++_versionCarga;
                _carga?.Cancel();
                var carga = CancellationTokenSource.CreateLinkedTokenSource(_cierre.Token);
                _carga = carga;
                try
                {
                    await BancosMetConsultarTasaAsync(cuenta.Id, BancosMetCodigoMoneda(cuenta.Moneda), _fecha.Value.Date, version, carga.Token);
                }
                finally
                {
                    if (ReferenceEquals(_carga, carga)) _carga = null;
                    carga.Dispose();
                }
            }
            private async Task BancosMetCargarRegistroAsync()
            {
                if (_cierre.IsCancellationRequested || IsDisposed) return;
                int version = ++_versionCarga;
                _carga?.Cancel();
                var carga = CancellationTokenSource.CreateLinkedTokenSource(_cierre.Token);
                _carga = carga;
                var cuenta = _cuenta.SelectedItem as ClsCuentaCatalogo;
                DateTime fecha = _fecha.Value.Date;
                string moneda = BancosMetCodigoMoneda(cuenta?.Moneda);
                BancosMetLimpiarImportes();
                _guardar.Enabled = false;
                _reintentarSoloTasa = false;
                _reintentar.Visible = false;
                _reintentar.Text = "Reintentar";
                _estado.Text = "Consultando registro de la cuenta y fecha…";
                _estado.ForeColor = Color.FromArgb(23, 75, 53);
                _informacion.SetToolTip(_estado, _estado.Text);
                _fuente.Enabled = false;
                foreach (NumericUpDown numero in _importes) numero.Enabled = false;
                try
                {
                    if (cuenta == null) { _estado.Text = "Selecciona una cuenta bancaria."; return; }
                    var consulta = await _controlador.BancosMetConsultarAsync(
                        fecha, null, cuenta.Id, null, carga.Token);
                    if (!BancosMetCargaActual(cuenta.Id, moneda, fecha, version, carga.Token)) return;
                    _fuente.Enabled = true;
                    foreach (NumericUpDown numero in _importes) numero.Enabled = true;
                    if (!consulta.TieneTabla)
                    {
                        _estado.Text = "Falta preparar la tabla de disponibilidad diaria.";
                        _reintentar.Visible = true;
                        return;
                    }
                    var anterior = consulta.Detalle.SingleOrDefault(d => d.CuentaId == cuenta.Id);
                    if (anterior != null && BancosMetCodigoMoneda(anterior.Moneda) != moneda)
                    {
                        _estado.Text = "Este corte conserva otra moneda histórica. No se puede reinterpretar.";
                        return;
                    }
                    if (anterior != null)
                    {
                        BancosMetAplicarRegistro(anterior);
                        _estado.Text = BancosMetTasaVigente() ? "Registro existente: se conserva la tasa guardada." : "El registro no contiene una tasa guardada válida.";
                        _informacion.SetToolTip(_estado, _detalleTasa ?? _estado.Text);
                        _guardar.Enabled = BancosMetTasaVigente();
                    }
                    else await BancosMetConsultarTasaAsync(cuenta.Id, moneda, fecha, version, carga.Token);
                }
                catch (OperationCanceledException) when (carga.IsCancellationRequested) { }
                catch (Exception ex)
                {
                    if (cuenta != null && BancosMetCargaActual(cuenta.Id, moneda, fecha, version, carga.Token))
                    { _estado.Text = "No se pudo consultar el registro de la cuenta. Revisa la conexión y reintenta."; _estado.ForeColor = Color.Firebrick; _informacion.SetToolTip(_estado, ex.Message); _reintentar.Visible = true; }
                }
                finally
                {
                    if (BancosMetCargaActual(cuenta?.Id ?? 0, moneda, fecha, version, carga.Token) && !_guardando)
                    {
                        _fuente.Enabled = true;
                        foreach (NumericUpDown numero in _importes) numero.Enabled = true;
                    }
                    if (ReferenceEquals(_carga, carga)) _carga = null;
                    carga.Dispose();
                }
            }
            private async void BancosBtnGuardar_Click(object sender, EventArgs e)
            {
                if (_guardando || _cierre.IsCancellationRequested) return;
                var cuenta = _cuenta.SelectedItem as ClsCuentaCatalogo;
                if (cuenta == null) { _estado.Text = "Selecciona una cuenta bancaria."; return; }
                string fuente = _fuente.Text.Trim();
                if (fuente.Length == 0 || fuente.Length > 100 - PrefijoFuente.Length)
                { _estado.Text = "Indica el respaldo del registro (máximo " + (100 - PrefijoFuente.Length) + " caracteres)."; return; }
                if (_registroEnConflicto) { _estado.Text = "El corte cambió. Pulsa Recargar antes de actualizar."; return; }
                if (!BancosMetTasaVigente()) { _estado.Text = "Espera una tasa válida para esta cuenta y fecha antes de guardar."; return; }
                var registro = new ClsDisponibilidad
                {
                    CuentaId = cuenta.Id, BancoId = cuenta.BancoId, NumeroCuenta = cuenta.NumeroCuenta,
                    Moneda = cuenta.Moneda, FechaCorte = _fecha.Value.Date,
                    SaldoInicial = _importes[0].Value, Ingresos = _importes[1].Value, Egresos = _importes[2].Value,
                    Comprometido = _importes[3].Value, Restringido = _importes[4].Value,
                    TipoCambioAplicado = _tasa.Value, Fuente = PrefijoFuente + fuente,
                    VersionRegistro = _versionRegistroOriginal, FechaGeneracion = _fechaGeneracionOriginal
                };
                _guardando = true;
                _guardar.Enabled = false;
                _cuenta.Enabled = _fecha.Enabled = _fuente.Enabled = false;
                foreach (NumericUpDown numero in _importes) numero.Enabled = false;
                _tasa.Enabled = false;
                _estado.Text = "Validando y guardando…";
                try
                {
                    // La versión pertenece al corte que el usuario abrió; una
                    // relectura aquí ocultaría una modificación de otra persona.
                    bool reemplazar = _registroExistente;
                    if (reemplazar && MessageBox.Show(this,
                        "Ya existe disponibilidad para esta cuenta y fecha. ¿Deseas reemplazarla con los importes revisados?",
                        "Confirmar actualización", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    { _estado.Text = "Se conservó el registro anterior."; return; }
                    await _controlador.BancosMetGuardarAsync(registro, reemplazar, _cierre.Token);
                    if (_cierre.IsCancellationRequested || IsDisposed) return;
                    CuentaGuardadaId = registro.CuentaId;
                    FechaGuardada = registro.FechaCorte;
                    _guardado = true;
                    BancosMetMarcarCapturaSinCambios();
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (ClsConflictoDisponibilidadException ex)
                {
                    if (!IsDisposed)
                    {
                        _registroEnConflicto = true;
                        _estado.Text = ex.Message;
                        _estado.ForeColor = Color.Firebrick;
                        _informacion.SetToolTip(_estado, ex.Message);
                        _reintentarSoloTasa = false;
                        _reintentar.Text = "Recargar";
                        _reintentar.Visible = true;
                    }
                }
                catch (OperationCanceledException) when (_cierre.IsCancellationRequested) { }
                catch (Exception ex)
                {
                    if (!IsDisposed) { _estado.Text = ex.Message; _informacion.SetToolTip(_estado, ex.Message); }
                }
                finally
                {
                    _guardando = false;
                    if (!IsDisposed)
                    {
                        _guardar.Enabled = BancosMetTasaVigente() && !_registroEnConflicto;
                        _cuenta.Enabled = _fecha.Enabled = _fuente.Enabled = true;
                        foreach (NumericUpDown numero in _importes) numero.Enabled = true;
                        _tasa.ReadOnly = true;
                        _tasa.Enabled = false;
                    }
                }
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
