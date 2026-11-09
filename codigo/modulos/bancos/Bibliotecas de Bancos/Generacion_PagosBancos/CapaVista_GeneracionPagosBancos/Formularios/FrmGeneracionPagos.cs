using CapaControlador_GeneracionPagos;
using CapaControlador_Seguridad.Objetos_de_valor;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 03/11/2026
 * ================================================================
 * Propósito :
 *  Formulario 8012 Generación de Pagos. Lista las obligaciones
 *  pendientes y las órdenes de pago, y guía cada orden por las
 *  etapas Preparar, Autorizar, Emitir y Registrar. La vista solo
 *  habla con el controlador.
 * ================================================================
 */

namespace CapaVista_GeneracionPagos.Formularios
{
    public partial class FrmGeneracionPagos : Form
    {
        private const string Titulo = "Generación de Pagos";
        private const string TextoBusqueda = "Documento o beneficiario";
        private const string TextoNuevaOrden = "Nueva orden";
        private const string TextoPorAsignar = "Por asignar";
        private const string TextoTodasMonedas = "Todas";
        private const string TextoSinTipoCambio = "Sin tipo de cambio";
        private const string MonedaBase = "GTQ";
        private const string FormatoFecha = "dd/MM/yyyy";
        private const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        private const string FormatoTipoCambio = "0.0000";
        private const int DiasAvisoVencimiento = ClsControladorGeneracionPagos.DiasAvisoVencimiento;
        private const int MaximoAvisosMostrados = 10;

        private static readonly Color _VerdeOscuro = Color.FromArgb(28, 74, 48);
        private static readonly Color _VerdeMedio = Color.FromArgb(46, 138, 85);
        private static readonly Color _TextoPrincipal = Color.FromArgb(40, 40, 40);
        private static readonly Color _TextoMarcador = Color.FromArgb(120, 130, 122);
        private static readonly Color _Rojo = Color.FromArgb(192, 57, 43);
        private static readonly Color _Ambar = Color.FromArgb(156, 101, 0);
        private static readonly Color _Azul = Color.FromArgb(31, 95, 139);
        private static readonly Color _FondoVencida = Color.FromArgb(251, 228, 224);
        private static readonly Color _FondoPorVencer = Color.FromArgb(255, 243, 205);
        private static readonly Color _AvisoVencidas = Color.FromArgb(255, 190, 120);

        private readonly ClsControladorGeneracionPagos _Controlador = new ClsControladorGeneracionPagos();

        // Obligación elegida para preparar la orden
        private string _TipoOrigen = "";
        private long _IdOrigen;
        private long _IdMoneda;
        private decimal _TipoCambio;

        // Orden elegida en el listado
        private long _IdOrdenActual;
        private string _MonedaOrdenActual = MonedaBase;

        private ClsResumenVencimientos _Vencimientos;

        private bool _Cargando;
        private Color _ColorRegistros;

        public FrmGeneracionPagos()
        {
            InitializeComponent();
            BancosMetAjustarTablas();
            BancosLblAviso.Visible = false;
            _ColorRegistros = BancosLblRegistros.ForeColor;

            Load += FrmGeneracionPagos_Load;
            Shown += FrmGeneracionPagos_Shown;
            BancosLblAviso.Click += BancosLblAviso_Click;

            BancosBtnBuscar.Click += BancosBtnBuscar_Click;
            BancosTxtBuscar.KeyDown += BancosTxtBuscar_KeyDown;
            BancosTxtBuscar.Enter += BancosTxtBuscar_Enter;
            BancosTxtBuscar.Leave += BancosTxtBuscar_Leave;
            BancosCboOrigen.SelectedIndexChanged += BancosCboFiltro_SelectedIndexChanged;
            BancosCboMoneda.SelectedIndexChanged += BancosCboFiltro_SelectedIndexChanged;
            BancosDgvObligaciones.CellClick += BancosDgvObligaciones_CellClick;
            BancosDgvOrdenes.CellClick += BancosDgvOrdenes_CellClick;

            BancosCboPrepCuenta.SelectedIndexChanged += BancosCboPrepCuenta_SelectedIndexChanged;
            BancosTxtPrepMonto.Enter += BancosTxtPrepMonto_Enter;
            BancosTxtPrepMonto.Leave += BancosTxtPrepMonto_Leave;
            BancosTxtPrepMonto.KeyPress += BancosTxtPrepMonto_KeyPress;
            BancosBtnPrepLimpiar.Click += BancosBtnPrepLimpiar_Click;
            BancosBtnPreparar.Click += BancosBtnPreparar_Click;

            BancosBtnAutAutorizar.Click += BancosBtnAutAutorizar_Click;
            BancosBtnAutRechazar.Click += BancosBtnAutRechazar_Click;

            BancosBtnEmiCheque.Click += BancosBtnEmiCheque_Click;
            BancosBtnEnviar.Click += BancosBtnEnviar_Click;

            BancosBtnRegMovimiento.Click += BancosBtnRegMovimiento_Click;
            BancosBtnSalir.Click += BancosBtnSalir_Click;
        }

        private void FrmGeneracionPagos_Load(object sender, EventArgs e)
        {
            _Cargando = true;
            BancosMetCargarMonedas();
            BancosCboOrigen.SelectedIndex = 0;
            BancosCboMoneda.SelectedIndex = 0;
            BancosDtpPrepFechaPago.MinDate = DateTime.Today;
            BancosDtpPrepFechaPago.Value = DateTime.Today;
            _Cargando = false;

            BancosMetCargarAutorizadores();
            BancosMetLimpiarPreparar();
            BancosMetLimpiarOrden();
            BancosMetCargarListados();
        }

        private void FrmGeneracionPagos_Shown(object sender, EventArgs e)
        {
            BancosMetMostrarDetalleVencimientos();
        }

        private void BancosLblAviso_Click(object sender, EventArgs e)
        {
            BancosMetMostrarDetalleVencimientos();
        }

        private void BancosMetCargarListados()
        {
            try
            {
                BancosMetCargarObligaciones();
                BancosMetCargarOrdenes();
            }
            catch (Exception Ex)
            {
                BancosMetMostrarMensaje("No se pudieron cargar los datos: " + Ex.Message, MessageBoxIcon.Error);
            }
            BancosMetActualizarAviso();
        }

        // El aviso toma todas las obligaciones pendientes, no solo las que deja ver el filtro de la lista
        private void BancosMetActualizarAviso()
        {
            try
            {
                _Vencimientos = _Controlador.BancosMetConsultarVencimientos();
            }
            catch (Exception)
            {
                _Vencimientos = null;
            }

            bool Hay = _Vencimientos != null && _Vencimientos.HayAvisos;
            BancosLblAviso.Visible = Hay;
            if (!Hay)
                return;

            bool Urgente = _Vencimientos.HayUrgentes;
            BancosLblAviso.BackColor = Urgente ? _FondoVencida : _FondoPorVencer;
            BancosLblAviso.ForeColor = Urgente ? _Rojo : _Ambar;
            BancosLblAviso.Text = BancosFuncTextoAviso(_Vencimientos);
        }

        private string BancosFuncTextoAviso(ClsResumenVencimientos Resumen)
        {
            var Partes = new System.Collections.Generic.List<string>();
            if (Resumen.Vencidas > 0)
                Partes.Add(Resumen.Vencidas + (Resumen.Vencidas == 1 ? " vencida" : " vencidas"));
            if (Resumen.VencenHoy > 0)
                Partes.Add(Resumen.VencenHoy + (Resumen.VencenHoy == 1 ? " vence hoy" : " vencen hoy"));
            if (Resumen.PorVencer > 0)
                Partes.Add(Resumen.PorVencer + (Resumen.PorVencer == 1 ? " vence" : " vencen") + " en los próximos " + DiasAvisoVencimiento + " días");
            return "Atención: " + string.Join(" · ", Partes) + "   (clic para ver el detalle)";
        }

        private void BancosMetMostrarDetalleVencimientos()
        {
            if (_Vencimientos == null || !_Vencimientos.HayAvisos)
                return;

            var Detalle = new StringBuilder();
            Detalle.AppendLine("Obligaciones que necesitan atención:");
            Detalle.AppendLine();
            foreach (ClsAvisoVencimiento Aviso in _Vencimientos.Avisos.Take(MaximoAvisosMostrados))
                Detalle.AppendLine(BancosFuncLineaAviso(Aviso));
            if (_Vencimientos.Avisos.Count > MaximoAvisosMostrados)
                Detalle.AppendLine("... y " + (_Vencimientos.Avisos.Count - MaximoAvisosMostrados) + " más en la lista de obligaciones.");

            MessageBox.Show(Detalle.ToString(), "Pagos por vencer", MessageBoxButtons.OK,
                _Vencimientos.HayUrgentes ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private string BancosFuncLineaAviso(ClsAvisoVencimiento Aviso)
        {
            string Estado;
            if (Aviso.Dias < 0)
                Estado = "Vencida hace " + (-Aviso.Dias) + (Aviso.Dias == -1 ? " día" : " días");
            else if (Aviso.Dias == 0)
                Estado = "Vence hoy";
            else
                Estado = "Vence en " + Aviso.Dias + (Aviso.Dias == 1 ? " día" : " días");

            return Estado + ": " + Aviso.Documento + " - " + Aviso.Beneficiario + " - " + Aviso.Importe;
        }

        private void BancosMetCargarObligaciones()
        {
            DataTable Tabla = _Controlador.BancosMetConsultarObligaciones(
                Convert.ToString(BancosCboOrigen.SelectedItem),
                Convert.ToString(BancosCboMoneda.SelectedItem),
                BancosFuncTextoBusqueda());

            int Vencidas = 0;
            BancosDgvObligaciones.Rows.Clear();
            foreach (DataRow Fila in Tabla.Rows)
            {
                string Moneda = Convert.ToString(Fila["Moneda"]);
                bool TieneFecha = Fila["Vencimiento"] != DBNull.Value;
                DateTime Vencimiento = TieneFecha ? Convert.ToDateTime(Fila["Vencimiento"]).Date : DateTime.Today;
                int Indice = BancosDgvObligaciones.Rows.Add(
                    BancosFuncOrigenCorto(Convert.ToString(Fila["Origen"])),
                    Convert.ToString(Fila["Documento"]),
                    Convert.ToString(Fila["Beneficiario"]),
                    TieneFecha ? Vencimiento.ToString(FormatoFecha) : "",
                    Moneda,
                    _Controlador.BancosFuncFormatearImporte(Convert.ToDecimal(Fila["Pendiente"]), Moneda));
                BancosDgvObligaciones.Rows[Indice].Tag = Fila;

                if (TieneFecha && BancosMetMarcarVencimiento(BancosDgvObligaciones.Rows[Indice], (DateTime.Today - Vencimiento).Days))
                    Vencidas++;
            }

            BancosLblRegistros.Text = "Mostrando " + Tabla.Rows.Count + " de " + Tabla.Rows.Count + " registros";
            if (Vencidas > 0)
                BancosLblRegistros.Text += " - " + Vencidas + (Vencidas == 1 ? " vencida" : " vencidas");
            BancosLblRegistros.ForeColor = Vencidas > 0 ? _AvisoVencidas : _ColorRegistros;
        }

        // Pinta la celda de vencimiento según los días que faltan o que han pasado. Devuelve true si ya venció.
        private bool BancosMetMarcarVencimiento(DataGridViewRow Fila, int DiasVencida)
        {
            DataGridViewCell Celda = Fila.Cells["BancosDgvObligacionesColVencimiento"];
            if (DiasVencida > 0)
            {
                Celda.Style.ForeColor = _Rojo;
                Celda.Style.BackColor = _FondoVencida;
                Celda.Style.SelectionForeColor = _Rojo;
                Celda.Style.SelectionBackColor = _FondoVencida;
                Celda.ToolTipText = "Vencida hace " + DiasVencida + (DiasVencida == 1 ? " día" : " días");
                return true;
            }

            int Faltan = -DiasVencida;
            if (Faltan <= DiasAvisoVencimiento)
            {
                Celda.Style.ForeColor = _Ambar;
                Celda.Style.BackColor = _FondoPorVencer;
                Celda.Style.SelectionForeColor = _Ambar;
                Celda.Style.SelectionBackColor = _FondoPorVencer;
            }
            Celda.ToolTipText = Faltan == 0 ? "Vence hoy" : "Vence en " + Faltan + (Faltan == 1 ? " día" : " días");
            return false;
        }

        private string BancosFuncOrigenCorto(string Origen)
        {
            switch (Origen)
            {
                case "Compras y CxP": return "Compras";
                case "Recursos Humanos": return "RRHH";
                default: return Origen;
            }
        }

        // Las columnas se reparten el ancho de cada tabla pero nunca bajan de su ancho mínimo; si no caben
        // aparece la barra horizontal. Filas de alto fijo y cada encabezado alineado igual que los datos de su columna.
        private void BancosMetAjustarTablas()
        {
            BancosDgvObligacionesColMoneda.Visible = false;
            BancosMetAjustarTabla(BancosDgvObligaciones,
                BancosFuncColumna(BancosDgvObligacionesColOrigen, 15, 75, DataGridViewContentAlignment.MiddleLeft),
                BancosFuncColumna(BancosDgvObligacionesColDocumento, 24, 120, DataGridViewContentAlignment.MiddleLeft),
                BancosFuncColumna(BancosDgvObligacionesColBeneficiario, 26, 175, DataGridViewContentAlignment.MiddleLeft),
                BancosFuncColumna(BancosDgvObligacionesColVencimiento, 17, 95, DataGridViewContentAlignment.MiddleCenter),
                BancosFuncColumna(BancosDgvObligacionesColMonto, 18, 115, DataGridViewContentAlignment.MiddleRight));

            BancosDgvOrdenesColMoneda.Visible = false;
            BancosDgvOrdenesColNumeroOrden.HeaderText = "Orden";
            BancosMetAjustarTabla(BancosDgvOrdenes,
                BancosFuncColumna(BancosDgvOrdenesColNumeroOrden, 17, 105, DataGridViewContentAlignment.MiddleLeft),
                BancosFuncColumna(BancosDgvOrdenesColBeneficiario, 38, 170, DataGridViewContentAlignment.MiddleLeft),
                BancosFuncColumna(BancosDgvOrdenesColMonto, 24, 115, DataGridViewContentAlignment.MiddleRight),
                BancosFuncColumna(BancosDgvOrdenesColEstado, 21, 90, DataGridViewContentAlignment.MiddleCenter));

            BancosMetAjustarTabla(BancosDgvAutAprobaciones,
                BancosFuncColumna(BancosDgvAutAprobacionesColAutorizador, 40, 150, DataGridViewContentAlignment.MiddleLeft),
                BancosFuncColumna(BancosDgvAutAprobacionesColDecision, 24, 100, DataGridViewContentAlignment.MiddleCenter),
                BancosFuncColumna(BancosDgvAutAprobacionesColFechaHora, 36, 150, DataGridViewContentAlignment.MiddleCenter));

            BancosMetAjustarTabla(BancosDgvRegPoliza,
                BancosFuncColumna(BancosDgvRegPolizaColCodigo, 20, 120, DataGridViewContentAlignment.MiddleLeft),
                BancosFuncColumna(BancosDgvRegPolizaColNombre, 44, 220, DataGridViewContentAlignment.MiddleLeft),
                BancosFuncColumna(BancosDgvRegPolizaColDebe, 18, 110, DataGridViewContentAlignment.MiddleRight),
                BancosFuncColumna(BancosDgvRegPolizaColHaber, 18, 110, DataGridViewContentAlignment.MiddleRight));

            BancosLblRegEstado.Location = new Point(BancosLblRegEstado.Right - 200, BancosLblRegEstado.Top);
            BancosLblRegEstado.Size = new Size(200, BancosLblRegEstado.Height);
        }

        private static Tuple<DataGridViewColumn, float, int, DataGridViewContentAlignment> BancosFuncColumna(
            DataGridViewColumn Columna, float Peso, int AnchoMinimo, DataGridViewContentAlignment Alineacion)
        {
            return Tuple.Create(Columna, Peso, AnchoMinimo, Alineacion);
        }

        private static void BancosMetAjustarTabla(DataGridView Tabla, params Tuple<DataGridViewColumn, float, int, DataGridViewContentAlignment>[] Columnas)
        {
            Tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Tabla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            Tabla.ScrollBars = ScrollBars.Both;
            Tabla.AllowUserToResizeColumns = false;
            Tabla.AllowUserToResizeRows = false;
            Tabla.AllowUserToOrderColumns = false;

            var Relleno = new Padding(6, 0, 6, 0);
            Tabla.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            Tabla.DefaultCellStyle.Padding = Relleno;
            Tabla.AlternatingRowsDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            // Las filas alternas heredan la alineación de su columna; si no, Monto, Debe y Haber se desalinean
            Tabla.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.NotSet;
            Tabla.AlternatingRowsDefaultCellStyle.Padding = Relleno;
            Tabla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            Tabla.ColumnHeadersDefaultCellStyle.Padding = Relleno;

            foreach (var Datos in Columnas)
            {
                DataGridViewColumn Columna = Datos.Item1;
                Columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Columna.FillWeight = Datos.Item2;
                Columna.MinimumWidth = Datos.Item3;
                Columna.SortMode = DataGridViewColumnSortMode.NotSortable;
                Columna.DefaultCellStyle.Alignment = Datos.Item4;
                Columna.HeaderCell.Style.Alignment = Datos.Item4;
            }
        }

        private void BancosMetCargarOrdenes()
        {
            DataTable Tabla = _Controlador.BancosMetConsultarOrdenes();

            BancosDgvOrdenes.Rows.Clear();
            foreach (DataRow Fila in Tabla.Rows)
            {
                string Moneda = Convert.ToString(Fila["codigoMoneda"]);
                string Estado = Convert.ToString(Fila["estadoOrdenPago"]);
                int Indice = BancosDgvOrdenes.Rows.Add(
                    Convert.ToString(Fila["referenciaOrdenPago"]),
                    Convert.ToString(Fila["beneficiarioOrdenPago"]),
                    Moneda,
                    _Controlador.BancosFuncFormatearImporte(Convert.ToDecimal(Fila["importeOrdenPago"]), Moneda),
                    BancosFuncTitulo(Estado));
                BancosDgvOrdenes.Rows[Indice].Tag = Convert.ToInt64(Fila["idOrdenPago"]);
                BancosDgvOrdenes.Rows[Indice].Cells["BancosDgvOrdenesColEstado"].Style.ForeColor = BancosFuncColorEstado(Estado);
            }
        }

        private void BancosMetCargarMonedas()
        {
            BancosCboMoneda.Items.Clear();
            BancosCboMoneda.Items.Add(TextoTodasMonedas);
            try
            {
                foreach (DataRow Moneda in _Controlador.BancosMetConsultarMonedas().Rows)
                    BancosCboMoneda.Items.Add(Convert.ToString(Moneda["codigoMoneda"]));
            }
            catch (Exception Ex)
            {
                BancosMetMostrarMensaje("No se pudo cargar la lista de monedas: " + Ex.Message, MessageBoxIcon.Warning);
            }
        }

        private void BancosMetCargarTiposPago()
        {
            _Cargando = true;
            BancosCboPrepTipoPago.DataSource = null;
            if (!string.IsNullOrEmpty(_TipoOrigen))
            {
                BancosCboPrepTipoPago.DisplayMember = "nombreMetodoPago";
                BancosCboPrepTipoPago.ValueMember = "idMetodoPago";
                BancosCboPrepTipoPago.DataSource = _Controlador.BancosMetObtenerTiposPago(_TipoOrigen);
                BancosCboPrepTipoPago.SelectedIndex = -1;
            }
            _Cargando = false;
        }

        private void BancosMetCargarAutorizadores()
        {
            try
            {
                BancosCboAutAutorizador.DataSource = null;
                BancosCboAutAutorizador.DisplayMember = "nombreUsuario";
                BancosCboAutAutorizador.ValueMember = "nombreUsuario";
                BancosCboAutAutorizador.DataSource = _Controlador.BancosMetObtenerAutorizadores();
                BancosCboAutAutorizador.SelectedIndex = -1;
            }
            catch (Exception Ex)
            {
                BancosMetMostrarMensaje("No se pudo cargar la lista de autorizadores: " + Ex.Message, MessageBoxIcon.Warning);
            }
        }

        private void BancosBtnBuscar_Click(object sender, EventArgs e)
        {
            BancosMetCargarListados();
        }

        private void BancosTxtBuscar_Enter(object sender, EventArgs e)
        {
            if (BancosTxtBuscar.Text == TextoBusqueda)
            {
                BancosTxtBuscar.Text = "";
                BancosTxtBuscar.ForeColor = _TextoPrincipal;
            }
        }

        private void BancosTxtBuscar_Leave(object sender, EventArgs e)
        {
            if (BancosTxtBuscar.Text.Trim().Length == 0)
            {
                BancosTxtBuscar.Text = TextoBusqueda;
                BancosTxtBuscar.ForeColor = _TextoMarcador;
            }
        }

        private void BancosTxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BancosMetCargarListados();
            }
        }

        private void BancosCboFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Cargando)
                return;
            BancosMetCargarListados();
        }

        private void BancosDgvObligaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            DataRow Fila = BancosDgvObligaciones.Rows[e.RowIndex].Tag as DataRow;
            if (Fila != null)
                BancosMetSeleccionarObligacion(Fila);
        }

        private void BancosDgvOrdenes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !(BancosDgvOrdenes.Rows[e.RowIndex].Tag is long))
                return;
            BancosMetSeleccionarOrden((long)BancosDgvOrdenes.Rows[e.RowIndex].Tag);
        }

        private void BancosMetSeleccionarObligacion(DataRow Fila)
        {
            _TipoOrigen = Convert.ToString(Fila["TipoOrigen"]);
            _IdOrigen = Convert.ToInt64(Fila["IdOrigen"]);
            _IdMoneda = Convert.ToInt64(Fila["IdMoneda"]);
            string Moneda = Convert.ToString(Fila["Moneda"]);
            decimal Pendiente = Convert.ToDecimal(Fila["Pendiente"]);
            string Documento = Convert.ToString(Fila["Documento"]);
            string Beneficiario = Convert.ToString(Fila["Beneficiario"]);

            BancosTbcProceso.SelectedTab = BancosTbpPreparar;
            BancosTxtPrepNumeroOrden.Text = TextoNuevaOrden;
            BancosTxtPrepObligacion.Text = Documento;
            BancosTxtPrepBeneficiario.Text = Beneficiario;
            BancosTxtPrepMoneda.Text = Moneda;
            BancosTxtPrepMonto.Text = _Controlador.BancosFuncFormatearImporte(Pendiente, Moneda);
            BancosTxtPrepConcepto.Text = "Pago " + Documento + " - " + Beneficiario;
            BancosDtpPrepFechaPago.Value = DateTime.Today;

            try
            {
                BancosMetCargarTiposPago();
                BancosMetCargarCuentas();
                _TipoCambio = _Controlador.BancosFuncObtenerTipoCambio(Moneda);
            }
            catch (Exception Ex)
            {
                _TipoCambio = 0m;
                BancosMetMostrarMensaje("No se pudieron consultar las cuentas o el tipo de cambio: " + Ex.Message, MessageBoxIcon.Warning);
            }
            BancosMetMostrarEquivalente();
        }

        private void BancosMetCargarCuentas()
        {
            _Cargando = true;
            BancosCboPrepCuenta.DataSource = null;
            BancosCboPrepCuenta.DisplayMember = "Cuenta";
            BancosCboPrepCuenta.ValueMember = "idCuentaBancaria";
            BancosCboPrepCuenta.DataSource = _Controlador.BancosMetConsultarCuentas(_IdMoneda);
            BancosCboPrepCuenta.SelectedIndex = -1;
            _Cargando = false;

            BancosTxtPrepSaldo.Text = "";
            if (BancosCboPrepCuenta.Items.Count == 0)
                BancosMetMostrarMensaje("No hay cuentas bancarias activas en " + BancosTxtPrepMoneda.Text + " para pagar esta obligación.", MessageBoxIcon.Information);
        }

        private void BancosCboPrepCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Cargando)
                return;
            DataRowView Cuenta = BancosCboPrepCuenta.SelectedItem as DataRowView;
            if (Cuenta == null)
            {
                BancosTxtPrepSaldo.Text = "";
                return;
            }
            BancosTxtPrepSaldo.Text = _Controlador.BancosFuncFormatearImporte(Convert.ToDecimal(Cuenta["Disponible"]), BancosTxtPrepMoneda.Text);
        }

        private void BancosTxtPrepMonto_Enter(object sender, EventArgs e)
        {
            decimal Monto = BancosFuncLeerMonto(BancosTxtPrepMonto.Text);
            BancosTxtPrepMonto.Text = Monto > 0 ? Monto.ToString("0.00", CultureInfo.InvariantCulture) : "";
            BancosTxtPrepMonto.SelectAll();
        }

        private void BancosTxtPrepMonto_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_TipoOrigen))
                return;
            decimal Monto = BancosFuncLeerMonto(BancosTxtPrepMonto.Text);
            BancosTxtPrepMonto.Text = _Controlador.BancosFuncFormatearImporte(Monto, BancosTxtPrepMoneda.Text);
            BancosMetMostrarEquivalente();
        }

        private void BancosTxtPrepMonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                return;
            if (e.KeyChar == '.' && !BancosTxtPrepMonto.Text.Contains("."))
                return;
            e.Handled = true;
        }

        private void BancosMetMostrarEquivalente()
        {
            if (_TipoCambio <= 0m)
            {
                BancosTxtPrepTipoCambio.Text = string.IsNullOrEmpty(_TipoOrigen) ? "" : TextoSinTipoCambio;
                BancosTxtPrepEquivalente.Text = "";
                return;
            }
            decimal Monto = BancosFuncLeerMonto(BancosTxtPrepMonto.Text);
            BancosTxtPrepTipoCambio.Text = _TipoCambio.ToString(FormatoTipoCambio, CultureInfo.InvariantCulture);
            BancosTxtPrepEquivalente.Text = _Controlador.BancosFuncFormatearImporte(
                _Controlador.BancosFuncCalcularEquivalente(Monto, _TipoCambio), MonedaBase);
        }

        private void BancosBtnPrepLimpiar_Click(object sender, EventArgs e)
        {
            BancosMetLimpiarPreparar();
        }

        private void BancosMetLimpiarPreparar()
        {
            _TipoOrigen = "";
            _IdOrigen = 0;
            _IdMoneda = 0;
            _TipoCambio = 0m;

            _Cargando = true;
            BancosCboPrepCuenta.DataSource = null;
            BancosCboPrepCuenta.Items.Clear();
            _Cargando = false;

            BancosTxtPrepNumeroOrden.Text = TextoNuevaOrden;
            BancosTxtPrepObligacion.Text = "";
            BancosTxtPrepBeneficiario.Text = "";
            BancosTxtPrepSaldo.Text = "";
            BancosTxtPrepMoneda.Text = "";
            BancosTxtPrepMonto.Text = "";
            BancosTxtPrepTipoCambio.Text = "";
            BancosTxtPrepEquivalente.Text = "";
            BancosTxtPrepConcepto.Text = "";
            BancosDtpPrepFechaPago.Value = DateTime.Today;
            BancosMetCargarTiposPago();
        }

        private void BancosBtnPreparar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_TipoOrigen))
            {
                BancosMetMostrarMensaje("Seleccione una obligación de la lista.", MessageBoxIcon.Information);
                return;
            }
            DataRowView Cuenta = BancosCboPrepCuenta.SelectedItem as DataRowView;
            if (Cuenta == null)
            {
                BancosMetMostrarMensaje("Seleccione la cuenta de origen.", MessageBoxIcon.Information);
                BancosCboPrepCuenta.Focus();
                return;
            }
            decimal Monto = BancosFuncLeerMonto(BancosTxtPrepMonto.Text);
            if (Monto <= 0m)
            {
                BancosMetMostrarMensaje("Ingrese un monto mayor a cero.", MessageBoxIcon.Information);
                BancosTxtPrepMonto.Focus();
                return;
            }

            DataRowView TipoPago = BancosCboPrepTipoPago.SelectedItem as DataRowView;
            if (TipoPago == null)
            {
                BancosMetMostrarMensaje("Seleccione el tipo de pago.", MessageBoxIcon.Information);
                BancosCboPrepTipoPago.Focus();
                return;
            }

            var Resultado = _Controlador.BancosMetPrepararOrden(_TipoOrigen, _IdOrigen,
                Convert.ToInt64(Cuenta["idCuentaBancaria"]), _IdMoneda, Monto,
                BancosDtpPrepFechaPago.Value.Date, Convert.ToInt64(TipoPago["idMetodoPago"]), BancosFuncUsuario(), BancosFuncIdUsuario());

            if (!Resultado.Exito)
            {
                BancosMetMostrarMensaje(Resultado.Mensaje, MessageBoxIcon.Warning);
                return;
            }

            BancosMetMostrarMensaje(Resultado.Mensaje, MessageBoxIcon.Information);
            BancosMetLimpiarPreparar();
            BancosMetCargarListados();
            BancosMetSeleccionarOrden(Convert.ToInt64(Resultado.Valor));
        }

        private void BancosMetSeleccionarOrden(long IdOrden)
        {
            DataTable Datos;
            try
            {
                Datos = _Controlador.BancosMetObtenerDatosOrden(IdOrden);
            }
            catch (Exception Ex)
            {
                BancosMetMostrarMensaje("No se pudo cargar la orden: " + Ex.Message, MessageBoxIcon.Error);
                return;
            }
            if (Datos.Rows.Count == 0)
            {
                BancosMetMostrarMensaje("La orden seleccionada ya no existe.", MessageBoxIcon.Information);
                return;
            }

            DataRow Orden = Datos.Rows[0];
            string Estado = Convert.ToString(Orden["estadoOrdenPago"]);
            _IdOrdenActual = IdOrden;
            _MonedaOrdenActual = Convert.ToString(Orden["codigoMoneda"]);

            BancosMetMostrarCabecera(Orden);
            try
            {
                BancosMetMostrarAprobaciones(Convert.ToDecimal(Orden["importeOrdenPago"]), Convert.ToInt64(Orden["Fk_Moneda_Codigo"]), Estado);
                BancosMetMostrarEmision(Convert.ToInt64(Orden["Fk_CuentaBancaria_Codigo"]), Convert.ToString(Orden["medioOrdenPago"]), Estado);
                BancosMetMostrarRegistro(Estado);
            }
            catch (Exception Ex)
            {
                BancosMetMostrarMensaje("No se pudo cargar el detalle de la orden: " + Ex.Message, MessageBoxIcon.Error);
            }

            BancosTbcProceso.SelectedTab = BancosFuncPestanaDeEstado(Estado);
        }

        private void BancosMetMostrarCabecera(DataRow Orden)
        {
            string Referencia = Convert.ToString(Orden["referenciaOrdenPago"]);
            string Beneficiario = Convert.ToString(Orden["beneficiarioOrdenPago"]);
            string Importe = _Controlador.BancosFuncFormatearImporte(Convert.ToDecimal(Orden["importeOrdenPago"]), _MonedaOrdenActual);

            BancosTxtAutNumeroOrden.Text = Referencia;
            BancosTxtAutBeneficiario.Text = Beneficiario;
            BancosTxtAutMonto.Text = Importe;

            BancosTxtEmiNumeroOrden.Text = Referencia;
            BancosTxtEmiBeneficiario.Text = Beneficiario;
            BancosTxtEmiMedio.Text = BancosFuncTitulo(Convert.ToString(Orden["medioOrdenPago"]));
            BancosTxtEmiImporte.Text = Importe;
            BancosTxtEmiCuenta.Text = Convert.ToString(Orden["Cuenta"]);

            BancosTxtRegNumeroOrden.Text = Referencia;
        }

        private TabPage BancosFuncPestanaDeEstado(string Estado)
        {
            switch (Estado)
            {
                case ClsValoresPagoVista.EstadoPendiente:
                case ClsValoresPagoVista.EstadoRechazada:
                    return BancosTbpAutorizar;
                case ClsValoresPagoVista.EstadoAutorizada:
                    return BancosTbpEmitir;
                default:
                    return BancosTbpRegistrar;
            }
        }

        private void BancosMetMostrarAprobaciones(decimal Importe, long IdMoneda, string Estado)
        {
            DataTable Aprobaciones = _Controlador.BancosMetConsultarAprobaciones(_IdOrdenActual);
            BancosDgvAutAprobaciones.Rows.Clear();
            int Aprobadas = 0;
            foreach (DataRow Fila in Aprobaciones.Rows)
            {
                string Decision = Convert.ToString(Fila["decisionAprobacion"]);
                if (Decision == ClsValoresPagoVista.DecisionAprobada)
                    Aprobadas++;
                BancosDgvAutAprobaciones.Rows.Add(
                    Convert.ToString(Fila["identificadorAprobadorAprobacion"]),
                    BancosFuncTitulo(Decision),
                    Convert.ToDateTime(Fila["created_at"]).ToString(FormatoFechaHora));
            }

            int Requeridas = _Controlador.BancosFuncObtenerAprobacionesRequeridas(Importe, IdMoneda);
            BancosTxtAutAprobaciones.Text = Requeridas == 0
                ? "Sin política vigente"
                : Aprobadas + " de " + Requeridas;

            bool Pendiente = Estado == ClsValoresPagoVista.EstadoPendiente && Requeridas > 0;
            BancosCboAutAutorizador.Enabled = Pendiente;
            BancosBtnAutAutorizar.Enabled = Pendiente;
            BancosBtnAutRechazar.Enabled = Pendiente;
            BancosCboAutAutorizador.SelectedIndex = -1;
        }

        private void BancosMetMostrarEmision(long IdCuenta, string Medio, string Estado)
        {
            bool Autorizada = Estado == ClsValoresPagoVista.EstadoAutorizada;
            bool EsCheque = Medio == ClsValoresPagoVista.MedioCheque;

            BancosTxtEmiSerie.Text = "";
            BancosTxtEmiSiguiente.Text = "";
            if (EsCheque && (Autorizada || Estado == ClsValoresPagoVista.EstadoEmitida))
            {
                DataTable Chequera = _Controlador.BancosMetObtenerChequera(IdCuenta);
                if (Chequera.Rows.Count == 0)
                {
                    BancosTxtEmiSerie.Text = "Sin chequera activa";
                }
                else
                {
                    BancosTxtEmiSerie.Text = Convert.ToString(Chequera.Rows[0]["serieChequera"]);
                    BancosTxtEmiSiguiente.Text = Convert.ToString(Chequera.Rows[0]["siguienteNumeroChequera"]);
                }
            }

            BancosBtnEmiCheque.Enabled = Autorizada && EsCheque;
            BancosBtnEnviar.Enabled = Autorizada && !EsCheque;
        }

        private void BancosMetMostrarRegistro(string Estado)
        {
            BancosMetLimpiarRegistro();
            BancosBtnRegMovimiento.Enabled = false;
            if (Estado != ClsValoresPagoVista.EstadoEmitida && Estado != ClsValoresPagoVista.EstadoPagada)
                return;

            ClsDatosRegistro Datos = _Controlador.BancosMetObtenerRegistro(_IdOrdenActual);
            BancosMetMostrarMovimiento(Datos);
            BancosMetMostrarPoliza(Datos.Poliza);

            bool Registrable = Estado == ClsValoresPagoVista.EstadoEmitida && !Datos.Registrado && string.IsNullOrEmpty(Datos.Advertencia);
            BancosBtnRegMovimiento.Enabled = Registrable;
            if (!string.IsNullOrEmpty(Datos.Advertencia))
                BancosMetMostrarMensaje(Datos.Advertencia, MessageBoxIcon.Warning);
        }

        private void BancosMetMostrarMovimiento(ClsDatosRegistro Datos)
        {
            bool HayTipoCambio = Datos.TipoCambio > 0m;

            BancosTxtRegReferencia.Text = Datos.Registrado ? Datos.Referencia : TextoPorAsignar;
            BancosTxtRegImporte.Text = _Controlador.BancosFuncFormatearImporte(Datos.Importe, _MonedaOrdenActual);
            BancosTxtRegTipoCambio.Text = HayTipoCambio ? Datos.TipoCambio.ToString(FormatoTipoCambio, CultureInfo.InvariantCulture) : TextoSinTipoCambio;
            BancosTxtRegImporteGTQ.Text = HayTipoCambio ? _Controlador.BancosFuncFormatearImporte(Datos.ImporteGTQ, MonedaBase) : "";
            BancosLblRegEstado.Text = "Estado: " + BancosFuncTitulo(Datos.EstadoPoliza);
        }

        private void BancosMetMostrarPoliza(DataTable Poliza)
        {
            decimal TotalDebe = 0m;
            decimal TotalHaber = 0m;
            foreach (DataRow Linea in Poliza.Rows)
            {
                decimal Debe = Convert.ToDecimal(Linea["Debe"]);
                decimal Haber = Convert.ToDecimal(Linea["Haber"]);
                TotalDebe += Debe;
                TotalHaber += Haber;
                BancosDgvRegPoliza.Rows.Add(
                    Convert.ToString(Linea["Codigo"]),
                    Convert.ToString(Linea["Nombre"]),
                    Debe.ToString("N2", CultureInfo.InvariantCulture),
                    Haber.ToString("N2", CultureInfo.InvariantCulture));
            }

            BancosTxtRegTotalDebe.Text = _Controlador.BancosFuncFormatearImporte(TotalDebe, MonedaBase);
            BancosTxtRegTotalHaber.Text = _Controlador.BancosFuncFormatearImporte(TotalHaber, MonedaBase);
            BancosTxtRegDiferencia.Text = _Controlador.BancosFuncFormatearImporte(TotalDebe - TotalHaber, MonedaBase);
        }

        private void BancosMetLimpiarRegistro()
        {
            BancosTxtRegReferencia.Text = "";
            BancosTxtRegImporte.Text = "";
            BancosTxtRegTipoCambio.Text = "";
            BancosTxtRegImporteGTQ.Text = "";
            BancosLblRegEstado.Text = "Estado: Sin registrar";
            BancosDgvRegPoliza.Rows.Clear();
            BancosTxtRegTotalDebe.Text = "";
            BancosTxtRegTotalHaber.Text = "";
            BancosTxtRegDiferencia.Text = "";
        }

        private void BancosMetLimpiarOrden()
        {
            _IdOrdenActual = 0;

            BancosTxtAutNumeroOrden.Text = "";
            BancosTxtAutBeneficiario.Text = "";
            BancosTxtAutMonto.Text = "";
            BancosTxtAutAprobaciones.Text = "";
            BancosDgvAutAprobaciones.Rows.Clear();
            BancosCboAutAutorizador.SelectedIndex = -1;
            BancosCboAutAutorizador.Enabled = false;
            BancosBtnAutAutorizar.Enabled = false;
            BancosBtnAutRechazar.Enabled = false;

            BancosTxtEmiNumeroOrden.Text = "";
            BancosTxtEmiBeneficiario.Text = "";
            BancosTxtEmiMedio.Text = "";
            BancosTxtEmiImporte.Text = "";
            BancosTxtEmiCuenta.Text = "";
            BancosTxtEmiSerie.Text = "";
            BancosTxtEmiSiguiente.Text = "";
            BancosBtnEmiCheque.Enabled = false;
            BancosBtnEnviar.Enabled = false;

            BancosTxtRegNumeroOrden.Text = "";
            BancosMetLimpiarRegistro();
            BancosBtnRegMovimiento.Enabled = false;
        }

        private void BancosBtnAutAutorizar_Click(object sender, EventArgs e)
        {
            BancosMetRegistrarDecision(true);
        }

        private void BancosBtnAutRechazar_Click(object sender, EventArgs e)
        {
            BancosMetRegistrarDecision(false);
        }

        private void BancosMetRegistrarDecision(bool Aprobar)
        {
            if (!BancosFuncHayOrden())
                return;

            DataRowView Autorizador = BancosCboAutAutorizador.SelectedItem as DataRowView;
            if (Autorizador == null)
            {
                BancosMetMostrarMensaje("Seleccione el autorizador.", MessageBoxIcon.Information);
                BancosCboAutAutorizador.Focus();
                return;
            }
            if (!Aprobar && !BancosFuncConfirmar("¿Seguro que desea rechazar la orden " + BancosTxtAutNumeroOrden.Text + "?"))
                return;

            string Nombre = Convert.ToString(Autorizador["nombreUsuario"]);
            var Resultado = Aprobar
                ? _Controlador.BancosMetAutorizarOrden(_IdOrdenActual, Nombre, BancosFuncIdUsuario())
                : _Controlador.BancosMetRechazarOrden(_IdOrdenActual, Nombre, BancosFuncIdUsuario());

            BancosMetTerminarOperacion(Resultado.Exito, Resultado.Mensaje);
        }

        private void BancosBtnEmiCheque_Click(object sender, EventArgs e)
        {
            if (!BancosFuncHayOrden())
                return;

            var Resultado = _Controlador.BancosMetEmitirCheque(_IdOrdenActual, BancosFuncUsuario(), BancosFuncIdUsuario());
            string Mensaje = Resultado.Exito ? Resultado.Mensaje + " Cheque " + Resultado.Valor + "." : Resultado.Mensaje;
            BancosMetTerminarOperacion(Resultado.Exito, Mensaje);
        }

        private void BancosBtnEnviar_Click(object sender, EventArgs e)
        {
            if (!BancosFuncHayOrden())
                return;

            var Resultado = _Controlador.BancosMetEnviarTransferencia(_IdOrdenActual, BancosFuncUsuario(), BancosFuncIdUsuario());
            BancosMetTerminarOperacion(Resultado.Exito, Resultado.Mensaje);
        }

        private bool BancosFuncHayOrden()
        {
            if (_IdOrdenActual != 0)
                return true;
            BancosMetMostrarMensaje("Seleccione una orden del listado.", MessageBoxIcon.Information);
            return false;
        }

        private void BancosBtnRegMovimiento_Click(object sender, EventArgs e)
        {
            if (_IdOrdenActual == 0)
                return;

            string Pregunta = "Se va a registrar el movimiento bancario de la orden " + BancosTxtRegNumeroOrden.Text
                + " y se generará la póliza en borrador. La orden pasará a Pagada. ¿Desea continuar?";
            if (!BancosFuncConfirmar(Pregunta))
                return;

            var Resultado = _Controlador.BancosMetRegistrarMovimiento(_IdOrdenActual, BancosFuncIdUsuario());
            BancosMetTerminarOperacion(Resultado.Exito, Resultado.Mensaje);
        }

        private void BancosMetTerminarOperacion(bool Exito, string Mensaje)
        {
            BancosMetMostrarMensaje(Mensaje, Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            long IdOrden = _IdOrdenActual;
            BancosMetCargarListados();
            if (Exito)
                BancosMetSeleccionarOrden(IdOrden);
        }

        private void BancosBtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string BancosFuncTextoBusqueda()
        {
            return BancosTxtBuscar.Text == TextoBusqueda ? "" : BancosTxtBuscar.Text;
        }

        private void BancosMetMostrarMensaje(string Mensaje, MessageBoxIcon Icono)
        {
            MessageBox.Show(Mensaje, Titulo, MessageBoxButtons.OK, Icono);
        }

        private bool BancosFuncConfirmar(string Pregunta)
        {
            return MessageBox.Show(Pregunta, Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private string BancosFuncUsuario()
        {
            return ClsSesionSeguridad.HaySesionActiva ? ClsSesionSeguridad.NombreUsuario : "Sin sesión";
        }

        private int? BancosFuncIdUsuario()
        {
            if (ClsSesionSeguridad.HaySesionActiva && ClsSesionSeguridad.IdUsuario > 0)
                return ClsSesionSeguridad.IdUsuario;
            return null;
        }

        private decimal BancosFuncLeerMonto(string Texto)
        {
            string Numero = new string((Texto ?? "").Where(Caracter => char.IsDigit(Caracter) || Caracter == '.').ToArray());
            decimal Valor;
            return decimal.TryParse(Numero, NumberStyles.Number, CultureInfo.InvariantCulture, out Valor) ? Valor : 0m;
        }

        private string BancosFuncTitulo(string Texto)
        {
            if (string.IsNullOrEmpty(Texto))
                return "";
            return char.ToUpper(Texto[0]) + Texto.Substring(1).ToLower();
        }

        private Color BancosFuncColorEstado(string Estado)
        {
            switch (Estado)
            {
                case ClsValoresPagoVista.EstadoPendiente: return _Ambar;
                case ClsValoresPagoVista.EstadoAutorizada: return _Azul;
                case ClsValoresPagoVista.EstadoEmitida: return _VerdeMedio;
                case ClsValoresPagoVista.EstadoPagada: return _VerdeOscuro;
                case ClsValoresPagoVista.EstadoRechazada: return _Rojo;
                default: return _TextoPrincipal;
            }
        }
    }
}
