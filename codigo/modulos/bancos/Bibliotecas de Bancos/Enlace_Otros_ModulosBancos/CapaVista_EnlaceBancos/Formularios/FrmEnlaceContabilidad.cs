using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceContabilidad : Form
    {
        private readonly ClsControladorEnlaceB _Controlador;
        private readonly ComboBox _BancarioCboEmpresa = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 255 };
        private readonly ComboBox _BancarioCboPais = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 255 };
        private readonly ComboBox _BancarioCboModulo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 255 };
        private readonly Label _BancarioLblResultado = new Label { Width = 255, Height = 65, ForeColor = ClsEstiloEnlace.Verde };
        private long _Partida;
        private bool _Ocupado, _Rellenando, _PuedeEnviar;

        public FrmEnlaceContabilidad() : this(new ClsControladorEnlaceB()) { }
        public FrmEnlaceContabilidad(ClsControladorEnlaceB Controlador)
        {
            this._Controlador = Controlador; InitializeComponent();
            Text = "8008 – EnlaceContabilidad"; _BancarioLblTitulo.Text = "Enlace Contabilidad";
            _BancarioLblTitulo.Font = new Font("Lucida Bright", 24, FontStyle.Bold | FontStyle.Italic);
            _BancarioLblTitulo.Location = new Point(26, 12);
            _BancarioLblTituloPanel.Font = new Font("Lucida Bright", 16, FontStyle.Bold | FontStyle.Italic);
            _BancarioLblMovimientos.Font = new Font("Lucida Bright", 18, FontStyle.Bold | FontStyle.Italic);
            _BancarioLblPartidaContable.Font = new Font("Lucida Bright", 18, FontStyle.Bold | FontStyle.Italic);
            StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(1230, 680);
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImage = Properties.Resources.FondoEnlaceotrosModulo; BackgroundImageLayout = ImageLayout.Stretch;
            _BancarioPbFondo.Visible = false; _BancarioPbFondoMovimientos.Visible = false;
            _BancarioPbFondoPartida.Visible = false; _BancarioPbFondoEnlace.Visible = false;
            _BancarioPbDiferencia.Visible = false; _BancarioPbDebe.Visible = false; _BancarioPbHaber.Visible = false;
            _BancarioPnlFiltros.Location = new Point(26, 70); _BancarioPnlFiltros.Size = new Size(830, 100);
            _BancarioDtpFechaInicial.Format = DateTimePickerFormat.Custom; _BancarioDtpFechaFinal.Format = DateTimePickerFormat.Custom; _BancarioDtpFechaInicial.CustomFormat = _BancarioDtpFechaFinal.CustomFormat = "dd/MM/yyyy";
            _BancarioLblMovimientos.Location = new Point(26, 174);
            _BancarioDgvMovimientos.Location = new Point(26, 215); _BancarioDgvMovimientos.Size = new Size(680, 120);
            _BancarioBtnGenerarPartida.Location = new Point(744, 236);
            _BancarioLblPartidaContable.Location = new Point(26, 349);
            _BancarioDgvPartidaContable.Location = new Point(26, 390); _BancarioDgvPartidaContable.Size = new Size(680, 130);
            _BancarioBtnEnviar.Location = new Point(730, 415); _BancarioBtnLimpiar.Location = new Point(830, 415);
            _BancarioPbMoneda.Location = new Point(35, 550); _BancarioLblTotalDebe.Location = new Point(99, 565);
            _BancarioPbBolsa.Location = new Point(260, 550); _BancarioLblTotalHaber.Location = new Point(324, 565);
            _BancarioPbBalanza.Location = new Point(485, 550); _BancarioLblDiferencia.Location = new Point(549, 565);
            _BancarioBtnEnviar.Size = new Size(88, 88); _BancarioBtnLimpiar.Size = new Size(88, 88);
            _BancarioPbMascota.Location = new Point(735, 530); _BancarioPbMascota.Size = new Size(160, 115);
            _BancarioPbMascota.SizeMode = PictureBoxSizeMode.Zoom;
            _BancarioLblAvisoTraslado.Location = new Point(50, 622); _BancarioLblAvisoTraslado.AutoSize = false; _BancarioLblAvisoTraslado.Size = new Size(650, 40);
            _BancarioBtnAdvertencia.Location = new Point(26, 625);
            _BancarioDgvMovimientos.Columns.Clear(); _BancarioDgvPartidaContable.Columns.Clear();
            ClsEstiloEnlace.BancarioMetTabla(_BancarioDgvMovimientos); ClsEstiloEnlace.BancarioMetTabla(_BancarioDgvPartidaContable);
            _BancarioCboPeriodo.Items.Clear(); _BancarioCboPeriodo.Items.Add("Mensual"); _BancarioCboPeriodo.SelectedIndex = 0;
            _BancarioDtpFechaInicial.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            _BancarioDtpFechaFinal.Value = _BancarioDtpFechaInicial.Value.AddMonths(1).AddDays(-1);
            _BancarioLblAvisoTraslado.Text = "GTQ. El envío registra una póliza BORRADOR, sin aplicar saldos.";
            _BancarioBtnEnviar.Enabled = false;
            _BancarioBtnGenerarPartida.Enabled = false;
            var BancarioFlpPanel = new FlowLayoutPanel { Location = new Point(935, 72), Size = new Size(278, 600),
                FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(8), BackColor = ClsEstiloEnlace.Fondo };
            BancarioFlpPanel.Controls.Add(new Label { Text = "Datos del traslado", Width = 255, Height = 28, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = ClsEstiloEnlace.Verde });
            foreach (var Item in new[] { Tuple.Create("Empresa", _BancarioCboEmpresa), Tuple.Create("País contable (GTQ)", _BancarioCboPais), Tuple.Create("Módulo contable de Bancos", _BancarioCboModulo) })
            {
                Item.Item2.Width = 250; Item.Item2.Margin = Padding.Empty;
                BancarioFlpPanel.Controls.Add(ClsEstiloEnlace.BancarioMetEtiquetaObligatoria(Item.Item1)); BancarioFlpPanel.Controls.Add(ClsEstiloEnlace.BancarioMetCampo(Item.Item2));
                Item.Item2.SelectedIndexChanged += (Remitente, Argumentos) => { ClsEstiloEnlace.BancarioMetValidacion(Item.Item2, ""); BancarioMetInvalidar(); };
            }
            var BancarioFlpAcciones = new FlowLayoutPanel { Width = 255, Height = 100, WrapContents = false };
            BancarioFlpAcciones.Controls.Add(ClsEstiloEnlace.BancarioMetBoton("Cargar catálogos", async (Remitente, Argumentos) => await BancarioMetCargarCatalogos()));
            BancarioFlpAcciones.Controls.Add(ClsEstiloEnlace.BancarioMetBoton("Ver historial", async (Remitente, Argumentos) => await BancarioMetHistorial()));
            BancarioFlpPanel.Controls.Add(BancarioFlpAcciones);
            BancarioFlpPanel.Controls.Add(new Label { Text = "Usuario: " + (ClsSesionSeguridad.HaySesionActiva ? ClsSesionSeguridad.NombreUsuario : "sin sesión"), Width = 255, Height = 32 });
            _BancarioLblResultado.Text = "Seleccione un mes. Consulte las pólizas y cargue los catálogos antes de generar."; BancarioFlpPanel.Controls.Add(_BancarioLblResultado); Controls.Add(BancarioFlpPanel); BancarioFlpPanel.BringToFront();
            _BancarioBtnConsultarMovimientos.Click += async (Remitente, Argumentos) => await BancarioMetEjecutar(async () => {
                if (!BancarioMetValidarFormulario(false)) return;
                DateTime Inicio = _BancarioDtpFechaInicial.Value.Date, Fin = _BancarioDtpFechaFinal.Value.Date;
                var Datos = await Task.Run(() => Controlador.BancarioMetConsultarPolizas(Inicio, Fin));
                _BancarioDgvMovimientos.DataSource = Datos; _BancarioLblResultado.Text = Datos.Rows.Count + " pólizas consultadas. Revise la columna Resultado."; });
            _BancarioBtnGenerarPartida.Click += async (Remitente, Argumentos) => await BancarioMetEjecutar(async () => {
                if (!BancarioMetValidarFormulario(true)) return;
                var Contexto = BancarioMetContexto();
                if (MessageBox.Show(this, "Se guardará la partida GTQ del mes con todas las pólizas validadas disponibles. ¿Continuar?", "Generar partida", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                BancarioMetMostrar(await Task.Run(() => Controlador.BancarioMetGenerar(Contexto))); ClsEstiloEnlace.BancarioMetExito(this, "La partida se generó correctamente."); });
            _BancarioBtnEnviar.Click += async (Remitente, Argumentos) => await BancarioMetEjecutar(async () => {
                if (!BancarioMetValidarFormulario(true)) return;
                var Contexto = BancarioMetContexto(); long Id = _Partida;
                if (MessageBox.Show(this, "¿Confirma el envío de la partida " + Id + " a Contabilidad como BORRADOR? No se aplicarán saldos.", "Confirmar traslado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                string Referencia = await Task.Run(() => Controlador.BancarioMetEnviar(Id, Contexto));
                BancarioMetMostrar(await Task.Run(() => Controlador.BancarioMetObtener(Id))); _BancarioLblResultado.Text = "Recibida en Contabilidad: " + Referencia + ". Sin aplicar saldos."; ClsEstiloEnlace.BancarioMetExito(this, "Partida recibida en Contabilidad: " + Referencia); });
            _BancarioBtnLimpiar.Click += (Remitente, Argumentos) => { BancarioMetInvalidar(); _BancarioDgvMovimientos.DataSource = null; _BancarioLblResultado.Text = "Sin partida seleccionada."; };
            _BancarioBtnCerrar.Click += (Remitente, Argumentos) => Close();
            _BancarioDtpFechaInicial.ValueChanged += (Remitente, Argumentos) => BancarioMetInvalidar(); _BancarioDtpFechaFinal.ValueChanged += (Remitente, Argumentos) => BancarioMetInvalidar();
            foreach (var BancarioDtpFecha in new[] { _BancarioDtpFechaInicial, _BancarioDtpFechaFinal })
            {
                Control Contenedor = BancarioDtpFecha.Parent; Point Posicion = BancarioDtpFecha.Location;
                Control BancarioPnlCampo = ClsEstiloEnlace.BancarioMetCampo(BancarioDtpFecha);
                BancarioPnlCampo.Location = Posicion; Contenedor.Controls.Add(BancarioPnlCampo); BancarioPnlCampo.BringToFront();
                BancarioDtpFecha.ValueChanged += (Remitente, Argumentos) => ClsEstiloEnlace.BancarioMetValidacion(BancarioDtpFecha, "");
            }
            foreach (var BancarioLblFecha in new[] { _BancarioLblFechaInicial, _BancarioLblFechaFinal })
            {
                var BancarioLblObligatorio = new Label { Text = "*", AutoSize = true, ForeColor = Color.Red,
                    Location = new Point(BancarioLblFecha.Right, BancarioLblFecha.Top) };
                BancarioLblFecha.Parent.Controls.Add(BancarioLblObligatorio); BancarioLblObligatorio.BringToFront();
            }
            foreach (var BancarioDgvTabla in new[] { _BancarioDgvMovimientos, _BancarioDgvPartidaContable })
            {
                Point Posicion = BancarioDgvTabla.Location; Size Medida = BancarioDgvTabla.Size;
                Control Paginador = ClsEstiloEnlace.BancarioMetPaginar(BancarioDgvTabla);
                Paginador.Location = Posicion; Paginador.Size = Medida; Paginador.Dock = DockStyle.None;
                Controls.Add(Paginador); Paginador.BringToFront();
            }
            ClsEstiloEnlace.BancarioMetVentana(this);
            Shown += async (Remitente, Argumentos) => await BancarioMetCargarCatalogos();
        }
        private ClsContextoPartida BancarioMetContexto()
        {
            if (!ClsSesionSeguridad.HaySesionActiva) throw new InvalidOperationException("Inicie sesión en el sistema para generar o enviar partidas.");
            return new ClsContextoPartida { Empresa = _BancarioCboEmpresa.SelectedValue == null ? 0 : Convert.ToInt32(_BancarioCboEmpresa.SelectedValue),
                Pais = _BancarioCboPais.SelectedValue == null ? 0 : Convert.ToInt32(_BancarioCboPais.SelectedValue),
                ModuloContable = _BancarioCboModulo.SelectedValue == null ? 0 : Convert.ToInt32(_BancarioCboModulo.SelectedValue),
                Responsable = ClsSesionSeguridad.NombreUsuario, UsuarioId = ClsSesionSeguridad.IdUsuario,
                Ip = ClsSesionSeguridad.SeguridadMetObtenerIPLocal(), Inicio = _BancarioDtpFechaInicial.Value.Date, Fin = _BancarioDtpFechaFinal.Value.Date };
        }
        private bool BancarioMetValidarFormulario(bool Mensual)
        {
            DateTime Inicio = _BancarioDtpFechaInicial.Value.Date, Fin = _BancarioDtpFechaFinal.Value.Date;
            bool FechasValidas = Inicio <= Fin && Fin.Year < 9999 && (!Mensual || (Inicio.Day == 1 && Fin == Inicio.AddMonths(1).AddDays(-1)));
            ClsEstiloEnlace.BancarioMetValidacion(_BancarioDtpFechaFinal, FechasValidas ? "" : Mensual ? "Seleccione un mes completo." : "Revise el rango de fechas.");
            if (!FechasValidas) { _BancarioLblResultado.Text = "Corrija las fechas antes de continuar."; _BancarioDtpFechaFinal.Focus(); return false; }
            if (!Mensual) return true;
            foreach (var BancarioCboCampo in new[] { _BancarioCboEmpresa, _BancarioCboPais, _BancarioCboModulo })
                ClsEstiloEnlace.BancarioMetValidacion(BancarioCboCampo, BancarioCboCampo.SelectedIndex >= 0 ? "" : "Seleccione una opción.");
            return BancarioMetCatalogosSeleccionados();
        }
        private async Task BancarioMetCargarCatalogos()
        {
            await BancarioMetEjecutar(async () => {
                var Datos = await Task.Run(() => new[] { _Controlador.BancarioMetCatalogo("empresa"), _Controlador.BancarioMetCatalogo("pais"), _Controlador.BancarioMetCatalogo("modulo") });
                ComboBox[] Controles = { _BancarioCboEmpresa, _BancarioCboPais, _BancarioCboModulo };
                for (int Indice = 0; Indice < Controles.Length; Indice++) { Controles[Indice].DisplayMember = "Nombre"; Controles[Indice].ValueMember = "Id"; Controles[Indice].DataSource = Datos[Indice]; Controles[Indice].SelectedIndex = -1; }
                _BancarioLblResultado.Text = Datos.Any(ImporteDebe => ImporteDebe.Rows.Count == 0)
                    ? "Catálogos disponibles: empresas " + Datos[0].Rows.Count + ", países " + Datos[1].Rows.Count + ", módulos " + Datos[2].Rows.Count + ". Faltan datos activos en la base conectada."
                    : "Seleccione empresa, país y módulo contable para habilitar Generar.";
            });
        }
        private void BancarioMetInvalidar()
        {
            if (_Rellenando) return;
            _BancarioBtnGenerarPartida.Enabled = BancarioMetCatalogosSeleccionados();
            _Partida = 0; _PuedeEnviar = false; _BancarioBtnEnviar.Enabled = false; _BancarioDgvPartidaContable.DataSource = null;
            _BancarioLblTotalDebe.Text = "Total Debe"; _BancarioLblTotalHaber.Text = "Total Haber"; _BancarioLblDiferencia.Text = "Diferencia";
        }
        private void BancarioMetMostrar(ClsPartidaEnlace Parametro)
        {
            _Partida = Parametro.Id; _BancarioDgvPartidaContable.DataSource = Parametro.Detalle;
            decimal Debe = Parametro.Detalle.Rows.Cast<DataRow>().Sum(Registro => Convert.ToDecimal(Registro["DebeDetallePartida"]));
            decimal Haber = Parametro.Detalle.Rows.Cast<DataRow>().Sum(Registro => Convert.ToDecimal(Registro["HaberDetallePartida"]));
            _BancarioLblTotalDebe.Text = "Debe: Q " + Debe.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("es-GT")); _BancarioLblTotalHaber.Text = "Haber: Q " + Haber.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("es-GT")); _BancarioLblDiferencia.Text = "Dif.: Q " + (Debe - Haber).ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("es-GT"));
            _BancarioLblResultado.Text = "Partida " + Parametro.Id + " — " + Parametro.Estado + (string.IsNullOrEmpty(Parametro.Referencia) ? "" : "\nReferencia: " + Parametro.Referencia);
            _PuedeEnviar = Parametro.Estado == "VALIDADA" || Parametro.Estado == "ERROR";
            _BancarioBtnEnviar.Enabled = _PuedeEnviar;
        }
        private bool BancarioMetCatalogosSeleccionados()
        {
            return _BancarioCboEmpresa.SelectedIndex >= 0 && _BancarioCboPais.SelectedIndex >= 0 && _BancarioCboModulo.SelectedIndex >= 0;
        }
        private async Task BancarioMetHistorial()
        {
            await BancarioMetEjecutar(async () => {
                DataTable Datos = await Task.Run(() => _Controlador.BancarioMetHistorico());
                using (var BancarioFrmDialogo = new Form { Text = "8008 – HistorialPartidas", ClientSize = new Size(1050, 530), StartPosition = FormStartPosition.CenterParent, BackColor = ClsEstiloEnlace.Fondo })
                {
                    var BancarioDgvTabla = new DataGridView { Dock = DockStyle.Fill }; ClsEstiloEnlace.BancarioMetTabla(BancarioDgvTabla);
                    var Paginador = ClsEstiloEnlace.BancarioMetPaginar(BancarioDgvTabla); BancarioDgvTabla.DataSource = Datos;
                    BancarioFrmDialogo.Icon = Icon; ClsEstiloEnlace.BancarioMetVentana(BancarioFrmDialogo);
                    var BancarioBtnSeleccionar = ClsEstiloEnlace.BancarioMetBoton("Seleccionar", (Remitente, Argumentos) => { if (BancarioDgvTabla.CurrentRow != null) BancarioFrmDialogo.DialogResult = DialogResult.OK; });
                    var BancarioFlpPie = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 100, FlowDirection = FlowDirection.RightToLeft };
                    BancarioFlpPie.Controls.Add(BancarioBtnSeleccionar); BancarioFrmDialogo.Controls.Add(Paginador); BancarioFrmDialogo.Controls.Add(BancarioFlpPie);
                    if (BancarioFrmDialogo.ShowDialog(this) != DialogResult.OK) return;
                    var Row = ((DataRowView)BancarioDgvTabla.CurrentRow.DataBoundItem).Row;
                    var Parametro = await Task.Run(() => _Controlador.BancarioMetObtener(Convert.ToInt64(Row["Id"])));
                    _Rellenando = true;
                    try { _BancarioDtpFechaInicial.Value = Convert.ToDateTime(Row["Inicio"]); _BancarioDtpFechaFinal.Value = Convert.ToDateTime(Row["Fin"]); if (_BancarioCboEmpresa.DataSource != null) _BancarioCboEmpresa.SelectedValue = Row["EmpresaId"]; }
                    finally { _Rellenando = false; }
                    BancarioMetMostrar(Parametro);
                }
            });
        }
        private async Task BancarioMetEjecutar(Func<Task> Accion)
        {
            if (_Ocupado) return;
            _Ocupado = true; UseWaitCursor = true;
            foreach (Control Elemento in Controls) Elemento.Enabled = false;
            try { await Accion(); }
            catch (Exception Ex) { _BancarioLblResultado.Text = "La operación no se completó."; ClsEstiloEnlace.BancarioMetError(this, Ex); }
            finally
            {
                foreach (Control Elemento in Controls) Elemento.Enabled = true;
                _Ocupado = false; UseWaitCursor = false;
                _BancarioBtnGenerarPartida.Enabled = BancarioMetCatalogosSeleccionados();
                _BancarioBtnEnviar.Enabled = _Partida > 0 && _PuedeEnviar;
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs Argumentos) { if (_Ocupado) Argumentos.Cancel = true; base.OnFormClosing(Argumentos); }
        private void BancarioMetBancDgvPartidaContable_CellContentClick(object Sender, DataGridViewCellEventArgs Argumentos) { }
    }
}
