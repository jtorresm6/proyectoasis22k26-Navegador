/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Presenta la conciliación bancaria: permite elegir la cuenta, el
 *  estado de cuenta y la fecha de corte, hacer el corte automático,
 *  marcar o desmarcar partidas a mano y abrir el reporte.
 * ================================================================
 */
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using CapaControlador_ConciliacionBancaria;
using CapaVista_ConciliacionBancaria.frmReportes;

namespace CapaVista_ConciliacionBancaria.Formularios
{
    public partial class FrmConciliacion : Form
    {
        private static readonly CultureInfo CulturaFechas = CultureInfo.InvariantCulture;
        private readonly ClsControladorConciliacion _Controlador;
        private long _IdConciliacion;
        private bool _Cargando;
        private long _IdMovimientoElegido;
        private long _IdLineaElegida;
        private long _IdMovimientoManual;
        private long _IdLineaManual;
        private string _AvisoSeleccion = string.Empty;
        private readonly Font _FuenteNegrita;
        private readonly ToolTip _Sugerencias = new ToolTip();

        public FrmConciliacion()
        {
            InitializeComponent();
            _Controlador = new ClsControladorConciliacion();
            _FuenteNegrita = new Font(BancosDgvMovimientos.DefaultCellStyle.Font, FontStyle.Bold);
            BancosMetConfigurarColumnas();
            BancosCboCuenta.SelectedIndexChanged += BancosCboCuenta_SelectedIndexChanged;
            BancosCboExtracto.SelectedIndexChanged += BancosCboExtracto_SelectedIndexChanged;
            BancosDtpFechaCorte.ValueChanged += BancosDtpFechaCorte_ValueChanged;
            BancosDgvMovimientos.DataBindingComplete += BancosDgvMovimientos_DataBindingComplete;
            BancosDgvMovimientos.CellClick += BancosDgvMovimientos_CellClick;
            BancosDgvMovimientos.KeyDown += BancosDgvMovimientos_KeyDown;
            _Sugerencias.SetToolTip(BancosBtnCorte, "Compara el libro contra el estado de cuenta a la fecha de corte");
            _Sugerencias.SetToolTip(BancosBtnMarcar, "Concilia (o desmarca) las partidas que eligió con un clic");
            BancosBtnCorte.Click += BancosBtnCorte_Click;
            BancosBtnMarcar.Click += BancosBtnMarcar_Click;
            BancosBtnReporte.Click += BancosBtnReporte_Click;
            BancosBtnAyuda.Click += BancosBtnAyuda_Click;
            BancosBtnSalir.Click += BancosBtnSalir_Click;
        }

        private void FrmConciliacion_Load(object sender, EventArgs e)
        {
            BancosMetLimpiarResultado();
            BancosMetCargarCuentas();
        }

        private void BancosMetConfigurarColumnas()
        {
            BancosDgvMovimientos.AutoGenerateColumns = false;
            BancosDgvMovimientos.Columns.Clear();
            DataGridViewTextBoxColumn ColumnaMarca = new DataGridViewTextBoxColumn();
            ColumnaMarca.Name = "Marca";
            ColumnaMarca.HeaderText = "Elegida";
            ColumnaMarca.Width = 85;
            ColumnaMarca.MinimumWidth = 70;
            ColumnaMarca.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ColumnaMarca.SortMode = DataGridViewColumnSortMode.NotSortable;
            BancosDgvMovimientos.Columns.Add(ColumnaMarca);
            BancosMetAgregarColumna("IdMovimiento", "IdMovimiento", 0, false, false);
            BancosMetAgregarColumna("IdLinea", "IdLinea", 0, false, false);
            BancosMetAgregarColumna("Estado", "Estado", 115, true, false);
            BancosMetAgregarColumna("Origen", "Origen", 105, true, false);
            BancosMetAgregarColumna("Fecha", "Fecha", 90, true, false);
            BancosMetAgregarColumna("ReferenciaLibro", "Ref. libro", 115, true, false);
            BancosMetAgregarColumna("ConceptoLibro", "Concepto libro", 240, true, false);
            BancosMetAgregarColumna("ImporteLibro", "Importe libro", 115, true, true);
            BancosMetAgregarColumna("ReferenciaBanco", "Ref. banco", 115, true, false);
            BancosMetAgregarColumna("ConceptoBanco", "Concepto banco", 240, true, false);
            BancosMetAgregarColumna("ImporteBanco", "Importe banco", 115, true, true);
            BancosMetAgregarColumna("Diferencia", "Diferencia", 105, true, true);
            BancosMetAgregarColumna("Metodo", "Método", 100, true, false);
            BancosMetAgregarColumna("Motivo", "Observación", 360, true, false);
            BancosMetAjustarTabla();
        }

        private void BancosMetAjustarTabla()
        {
            BancosDgvMovimientos.ScrollBars = ScrollBars.Both;
            BancosDgvMovimientos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            BancosDgvMovimientos.RowTemplate.Height = 28;
            BancosDgvMovimientos.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            BancosDgvMovimientos.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            BancosDgvMovimientos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            BancosDgvMovimientos.GridColor = Color.FromArgb(214, 226, 218);
            BancosDgvMovimientos.Columns["Estado"].Frozen = true;
        }

        private void BancosMetAgregarColumna(string Campo, string Encabezado, float Ancho, bool Visible, bool Importe)
        {
            DataGridViewTextBoxColumn Columna = new DataGridViewTextBoxColumn();
            Columna.Name = Campo;
            Columna.DataPropertyName = Campo;
            Columna.HeaderText = Encabezado;
            Columna.MinimumWidth = Ancho > 0 ? 60 : 2;
            Columna.Width = Ancho > 0 ? (int)Ancho : 2;
            Columna.Visible = Visible;
            Columna.SortMode = DataGridViewColumnSortMode.NotSortable;
            if (Importe)
            {
                Columna.DefaultCellStyle.Format = "N2";
                Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Columna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            BancosDgvMovimientos.Columns.Add(Columna);
        }

        private void BancosMetCargarCuentas()
        {
            string Error = _Controlador.BancosMetConsultarCuentas(out DataTable Cuentas);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible cargar las cuentas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Cargando = true;
            BancosCboCuenta.DataSource = Cuentas;
            BancosCboCuenta.DisplayMember = "Descripcion";
            BancosCboCuenta.ValueMember = "IdCuenta";
            BancosCboCuenta.SelectedIndex = -1;
            _Cargando = false;
        }

        private long BancosFuncIdSeleccionado(ComboBox Combo)
        {
            if (Combo.SelectedIndex < 0 || Combo.SelectedValue == null) return 0;
            long.TryParse(Convert.ToString(Combo.SelectedValue, CulturaFechas), out long Id);
            return Id;
        }

        private void BancosCboCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Cargando) return;
            BancosMetLimpiarResultado();
            BancosMetLimpiarExtractos();

            long IdCuenta = BancosFuncIdSeleccionado(BancosCboCuenta);
            if (IdCuenta <= 0) return;

            string Error = _Controlador.BancosMetConsultarExtractos(IdCuenta, out DataTable Extractos);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible cargar los extractos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Cargando = true;
            BancosCboExtracto.DataSource = Extractos;
            BancosCboExtracto.DisplayMember = "Descripcion";
            BancosCboExtracto.ValueMember = "IdExtracto";
            BancosCboExtracto.SelectedIndex = -1;
            _Cargando = false;

            if (Extractos.Rows.Count == 0)
            {
                MessageBox.Show("La cuenta seleccionada no tiene estados de cuenta cargados.", "Sin extractos",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BancosMetLimpiarExtractos()
        {
            _Cargando = true;
            BancosCboExtracto.DataSource = null;
            BancosCboExtracto.Items.Clear();
            BancosCboExtracto.SelectedIndex = -1;
            BancosDtpFechaCorte.MinDate = new DateTime(1753, 1, 1);
            BancosDtpFechaCorte.MaxDate = new DateTime(9998, 12, 31);
            BancosDtpFechaCorte.Value = DateTime.Today;
            _Cargando = false;
        }

        private void BancosCboExtracto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Cargando) return;
            BancosMetLimpiarResultado();

            long IdExtracto = BancosFuncIdSeleccionado(BancosCboExtracto);
            if (IdExtracto <= 0) return;

            string Error = _Controlador.BancosMetObtenerPeriodoExtracto(IdExtracto,
                out DateTime FechaInicio, out DateTime FechaFin, out decimal SaldoInicial);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible leer el extracto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Cargando = true;
            BancosDtpFechaCorte.MinDate = new DateTime(1753, 1, 1);
            BancosDtpFechaCorte.MaxDate = new DateTime(9998, 12, 31);
            BancosDtpFechaCorte.Value = FechaFin;
            BancosDtpFechaCorte.MinDate = FechaInicio;
            BancosDtpFechaCorte.MaxDate = FechaFin;
            _Cargando = false;
            BancosTxtSaldoInicialBanco.Text = SaldoInicial.ToString("N2");
        }

        private void BancosDtpFechaCorte_ValueChanged(object sender, EventArgs e)
        {
            if (_Cargando) return;
            long IdExtracto = BancosFuncIdSeleccionado(BancosCboExtracto);
            string Saldo = BancosTxtSaldoInicialBanco.Text;
            BancosMetLimpiarResultado();
            if (IdExtracto > 0) BancosTxtSaldoInicialBanco.Text = Saldo;
        }

        private void BancosMetLimpiarResultado()
        {
            _IdConciliacion = 0;
            BancosMetLimpiarEleccion();
            BancosDgvMovimientos.DataSource = null;
            foreach (TextBox Caja in new[]
            {
                BancosTxtSaldoInicialBanco, BancosTxtSaldoBanco, BancosTxtSaldoLibro, BancosTxtDiferencia,
                BancosTxtCargoCon, BancosTxtAbonoCon, BancosTxtSaldoCon,
                BancosTxtCargoPorCon, BancosTxtAbonoPorCon, BancosTxtSaldoPorCon
            })
            {
                Caja.Text = string.Empty;
            }
            BancosLblContadores.Text = string.Empty;
            BancosTxtEstado.Text = "Sin corte";
            BancosTxtEstado.ForeColor = SystemColors.ControlText;
            BancosTxtEstado.BackColor = Color.FromArgb(242, 247, 240);
            BancosMetActualizarSeleccion();
        }

        private void BancosMetMostrarResultado(ClsResultadoConciliacion Resultado)
        {
            _IdConciliacion = Resultado.IdConciliacion;
            BancosMetLimpiarEleccion();
            BancosTxtSaldoInicialBanco.Text = Resultado.SaldoInicialBanco.ToString("N2");
            BancosTxtSaldoBanco.Text = Resultado.SaldoBanco.ToString("N2");
            BancosTxtSaldoLibro.Text = Resultado.SaldoLibro.ToString("N2");
            BancosTxtDiferencia.Text = Resultado.Diferencia.ToString("N2");
            BancosTxtCargoCon.Text = Resultado.CargosConciliados.ToString("N2");
            BancosTxtAbonoCon.Text = Resultado.AbonosConciliados.ToString("N2");
            BancosTxtSaldoCon.Text = (Resultado.AbonosConciliados - Resultado.CargosConciliados).ToString("N2");
            BancosTxtCargoPorCon.Text = Resultado.CargosPorConciliar.ToString("N2");
            BancosTxtAbonoPorCon.Text = Resultado.AbonosPorConciliar.ToString("N2");
            BancosTxtSaldoPorCon.Text = (Resultado.AbonosPorConciliar - Resultado.CargosPorConciliar).ToString("N2");

            bool Cuadrada = Resultado.Pendientes == 0 && Resultado.Diferencias == 0 && Resultado.Diferencia == 0m;
            BancosTxtEstado.Text = Resultado.EstadoTexto;
            BancosTxtEstado.ForeColor = Cuadrada ? Color.FromArgb(25, 78, 53) : Color.FromArgb(150, 45, 35);
            BancosTxtEstado.BackColor = Cuadrada ? Color.FromArgb(224, 241, 226) : Color.FromArgb(255, 239, 220);

            BancosLblContadores.Text = string.Format(CulturaFechas,
                "Conciliadas: {0}\r\nCon diferencia: {1}\r\nPendientes: {2}",
                Resultado.Coincidencias, Resultado.Diferencias, Resultado.Pendientes);

            BancosDgvMovimientos.DataSource = Resultado.Tabla;
            BancosDgvMovimientos.ClearSelection();
            BancosMetActualizarSeleccion();
        }

        private void BancosDgvMovimientos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            BancosMetPintarFilas();
        }

        private void BancosDgvMovimientos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            BancosMetElegirFila(BancosDgvMovimientos.Rows[e.RowIndex]);
        }

        private void BancosDgvMovimientos_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode != Keys.Space && e.KeyCode != Keys.Enter) || BancosDgvMovimientos.CurrentRow == null) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            BancosMetElegirFila(BancosDgvMovimientos.CurrentRow);
        }

        private void BancosMetLimpiarEleccion()
        {
            _IdMovimientoElegido = 0;
            _IdLineaElegida = 0;
            _IdMovimientoManual = 0;
            _IdLineaManual = 0;
            _AvisoSeleccion = string.Empty;
        }

        private void BancosMetElegirFila(DataGridViewRow Fila)
        {
            if (_IdConciliacion <= 0) return;

            string Origen = Convert.ToString(Fila.Cells["Origen"].Value);
            long IdMovimiento = BancosFuncIdFila(Fila, "IdMovimiento");
            long IdLinea = BancosFuncIdFila(Fila, "IdLinea");
            _AvisoSeleccion = string.Empty;

            if (Origen == "Solo libro")
            {
                _IdMovimientoManual = 0;
                _IdLineaManual = 0;
                _IdMovimientoElegido = _IdMovimientoElegido == IdMovimiento ? 0 : IdMovimiento;
                if (_IdMovimientoElegido > 0 && _IdLineaElegida > 0 && !BancosFuncSentidosCompatibles())
                {
                    _IdLineaElegida = 0;
                    _AvisoSeleccion = "La línea del banco que había elegido es de otro tipo (cargo o abono). Elija otra.";
                }
            }
            else if (Origen == "Solo banco")
            {
                _IdMovimientoManual = 0;
                _IdLineaManual = 0;
                _IdLineaElegida = _IdLineaElegida == IdLinea ? 0 : IdLinea;
                if (_IdLineaElegida > 0 && _IdMovimientoElegido > 0 && !BancosFuncSentidosCompatibles())
                {
                    _IdMovimientoElegido = 0;
                    _AvisoSeleccion = "La partida del libro que había elegido es de otro tipo (cargo o abono). Elija otra.";
                }
            }
            else if (BancosFuncEsParejaManual(Fila))
            {
                _IdMovimientoElegido = 0;
                _IdLineaElegida = 0;
                bool YaElegida = _IdMovimientoManual == IdMovimiento && _IdLineaManual == IdLinea;
                _IdMovimientoManual = YaElegida ? 0 : IdMovimiento;
                _IdLineaManual = YaElegida ? 0 : IdLinea;
            }
            else
            {
                _AvisoSeleccion = "Esa partida ya quedó conciliada. Elija una partida pendiente.";
            }

            BancosMetActualizarSeleccion();
        }

        private bool BancosFuncSentidosCompatibles()
        {
            DataGridViewRow FilaLibro = BancosFuncBuscarFila("IdMovimiento", _IdMovimientoElegido, "Solo libro");
            DataGridViewRow FilaBanco = BancosFuncBuscarFila("IdLinea", _IdLineaElegida, "Solo banco");
            if (FilaLibro == null || FilaBanco == null) return true;
            return _Controlador.BancosFuncMismoSentido(
                BancosFuncImporteFila(FilaLibro, "ImporteLibro"), BancosFuncImporteFila(FilaBanco, "ImporteBanco"));
        }

        private DataGridViewRow BancosFuncBuscarFila(string ColumnaId, long Id, string Origen)
        {
            if (Id <= 0) return null;
            foreach (DataGridViewRow Fila in BancosDgvMovimientos.Rows)
            {
                if (Convert.ToString(Fila.Cells["Origen"].Value) == Origen && BancosFuncIdFila(Fila, ColumnaId) == Id)
                    return Fila;
            }
            return null;
        }

        private string BancosFuncDescribirPartida(DataGridViewRow Fila, string ColumnaReferencia, string ColumnaConcepto, string ColumnaImporte)
        {
            string Texto = Convert.ToString(Fila.Cells[ColumnaReferencia].Value);
            if (string.IsNullOrWhiteSpace(Texto)) Texto = Convert.ToString(Fila.Cells[ColumnaConcepto].Value);
            return Texto + " (" + Math.Abs(BancosFuncImporteFila(Fila, ColumnaImporte)).ToString("N2", CulturaFechas) + ")";
        }

        private void BancosMetPintarFilas()
        {
            DataGridViewRow FilaLibro = BancosFuncBuscarFila("IdMovimiento", _IdMovimientoElegido, "Solo libro");
            DataGridViewRow FilaBanco = BancosFuncBuscarFila("IdLinea", _IdLineaElegida, "Solo banco");
            decimal ImporteLibro = FilaLibro == null ? 0m : BancosFuncImporteFila(FilaLibro, "ImporteLibro");
            decimal ImporteBanco = FilaBanco == null ? 0m : BancosFuncImporteFila(FilaBanco, "ImporteBanco");
            Color ColorElegida = Color.FromArgb(198, 224, 245);
            Color ColorSugerida = Color.FromArgb(226, 240, 215);

            foreach (DataGridViewRow Fila in BancosDgvMovimientos.Rows)
            {
                string Estado = Convert.ToString(Fila.Cells["Estado"].Value);
                string Origen = Convert.ToString(Fila.Cells["Origen"].Value);
                long IdMovimiento = BancosFuncIdFila(Fila, "IdMovimiento");
                long IdLinea = BancosFuncIdFila(Fila, "IdLinea");
                string Marca = string.Empty;
                Color Fondo = Color.Empty;

                if (Estado == "Pendiente") Fondo = Color.FromArgb(255, 243, 224);
                else if (Estado == "Con diferencia") Fondo = Color.FromArgb(253, 232, 229);

                bool Elegida = (Origen == "Solo libro" && IdMovimiento == _IdMovimientoElegido && IdMovimiento > 0)
                    || (Origen == "Solo banco" && IdLinea == _IdLineaElegida && IdLinea > 0)
                    || (_IdMovimientoManual > 0 && IdMovimiento == _IdMovimientoManual && IdLinea == _IdLineaManual);
                bool Sugerida = !Elegida
                    && ((Origen == "Solo banco" && FilaLibro != null && FilaBanco == null
                            && _Controlador.BancosFuncEsSugerida(ImporteLibro, BancosFuncImporteFila(Fila, "ImporteBanco")))
                        || (Origen == "Solo libro" && FilaBanco != null && FilaLibro == null
                            && _Controlador.BancosFuncEsSugerida(BancosFuncImporteFila(Fila, "ImporteLibro"), ImporteBanco)));

                if (Elegida) { Fondo = ColorElegida; Marca = "Elegida"; }
                else if (Sugerida) { Fondo = ColorSugerida; Marca = "Sugerida"; }

                Fila.DefaultCellStyle.BackColor = Fondo;
                Fila.DefaultCellStyle.SelectionBackColor = Fondo.IsEmpty ? Color.Empty : Fondo;
                Fila.DefaultCellStyle.SelectionForeColor = Fondo.IsEmpty ? Color.Empty : SystemColors.ControlText;
                Fila.DefaultCellStyle.Font = Elegida ? _FuenteNegrita : null;
                Fila.Cells["Marca"].Value = Marca;
            }
        }

        private void BancosMetActualizarSeleccion()
        {
            BancosMetPintarFilas();

            DataGridViewRow FilaLibro = BancosFuncBuscarFila("IdMovimiento", _IdMovimientoElegido, "Solo libro");
            DataGridViewRow FilaBanco = BancosFuncBuscarFila("IdLinea", _IdLineaElegida, "Solo banco");
            string Texto;

            if (_IdConciliacion <= 0)
            {
                Texto = "Elija la cuenta, el estado de cuenta y la fecha, y presione Corte de conciliación.";
            }
            else if (_IdMovimientoManual > 0)
            {
                Texto = "Pareja marcada a mano.\r\nPara deshacerla presione Marcar Conciliación.";
            }
            else if (FilaLibro != null && FilaBanco != null)
            {
                decimal Diferencia = _Controlador.BancosFuncDiferenciaMarca(
                    BancosFuncImporteFila(FilaLibro, "ImporteLibro"), BancosFuncImporteFila(FilaBanco, "ImporteBanco"));
                Texto = string.Format(CulturaFechas,
                    "Libro: {0}\r\nBanco: {1}\r\nDiferencia: {2:N2}. Presione Marcar Conciliación.",
                    BancosFuncDescribirPartida(FilaLibro, "ReferenciaLibro", "ConceptoLibro", "ImporteLibro"),
                    BancosFuncDescribirPartida(FilaBanco, "ReferenciaBanco", "ConceptoBanco", "ImporteBanco"),
                    Diferencia);
            }
            else if (FilaLibro != null)
            {
                Texto = "Libro elegido: " + BancosFuncDescribirPartida(FilaLibro, "ReferenciaLibro", "ConceptoLibro", "ImporteLibro")
                    + ".\r\nAhora haga clic en la línea del banco. Las posibles coincidencias están en verde.";
            }
            else if (FilaBanco != null)
            {
                Texto = "Banco elegido: " + BancosFuncDescribirPartida(FilaBanco, "ReferenciaBanco", "ConceptoBanco", "ImporteBanco")
                    + ".\r\nAhora haga clic en la partida del libro. Las posibles coincidencias están en verde.";
            }
            else if (BancosFuncHayPendientes())
            {
                Texto = "Para conciliar a mano:\r\n1) Clic en una partida pendiente del libro.\r\n2) Clic en la línea del banco que le corresponde.\r\n3) Presione Marcar Conciliación.";
            }
            else
            {
                Texto = "No hay partidas pendientes por conciliar.";
            }

            bool Aviso = _AvisoSeleccion.Length > 0;
            BancosLblIndicacion.ForeColor = Aviso ? Color.FromArgb(150, 45, 35) : Color.DimGray;
            BancosLblIndicacion.Text = Aviso ? _AvisoSeleccion + "\r\n" + Texto : Texto;
        }

        private bool BancosFuncHayPendientes()
        {
            foreach (DataGridViewRow Fila in BancosDgvMovimientos.Rows)
            {
                if (Convert.ToString(Fila.Cells["Estado"].Value) == "Pendiente") return true;
            }
            return false;
        }

        private void BancosBtnCorte_Click(object sender, EventArgs e)
        {
            long IdCuenta = BancosFuncIdSeleccionado(BancosCboCuenta);
            long IdExtracto = BancosFuncIdSeleccionado(BancosCboExtracto);
            string FechaCorte = BancosDtpFechaCorte.Value.ToString("dd/MM/yyyy", CulturaFechas);

            string Error = _Controlador.BancosMetEjecutarCorte(IdCuenta, IdExtracto, FechaCorte,
                Environment.UserName, out ClsResultadoConciliacion Resultado);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No se realizó el corte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BancosMetMostrarResultado(Resultado);
        }

        private void BancosBtnMarcar_Click(object sender, EventArgs e)
        {
            if (_IdConciliacion <= 0)
            {
                MessageBox.Show("Realice primero el corte de conciliación.", "Falta el corte",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_IdMovimientoManual > 0 && _IdLineaManual > 0)
            {
                BancosMetDesmarcarPareja(_IdMovimientoManual, _IdLineaManual);
                return;
            }

            DataGridViewRow FilaLibro = BancosFuncBuscarFila("IdMovimiento", _IdMovimientoElegido, "Solo libro");
            DataGridViewRow FilaBanco = BancosFuncBuscarFila("IdLinea", _IdLineaElegida, "Solo banco");
            if (FilaLibro == null || FilaBanco == null)
            {
                MessageBox.Show("Haga clic en una partida pendiente del libro y luego en la línea del banco que le corresponde.\n\n" +
                    "Para desmarcar, haga clic en una pareja marcada a mano.", "Falta elegir las partidas",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal ImporteLibro = BancosFuncImporteFila(FilaLibro, "ImporteLibro");
            decimal ImporteBanco = BancosFuncImporteFila(FilaBanco, "ImporteBanco");
            decimal Diferencia = _Controlador.BancosFuncDiferenciaMarca(ImporteLibro, ImporteBanco);
            bool AceptarDiferencia = false;
            if (Diferencia != 0m)
            {
                string Mensaje = string.Format("Los importes no coinciden. La diferencia es de {0:N2}.\n\n¿Desea marcar las partidas de todos modos?", Diferencia);
                if (MessageBox.Show(Mensaje, "Confirmar diferencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                AceptarDiferencia = true;
            }

            string Error = _Controlador.BancosMetMarcar(_IdConciliacion,
                BancosFuncIdFila(FilaLibro, "IdMovimiento"), BancosFuncIdFila(FilaBanco, "IdLinea"),
                AceptarDiferencia, Environment.UserName, out ClsResultadoConciliacion Resultado);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No se marcaron las partidas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BancosMetMostrarResultado(Resultado);
        }

        private void BancosMetDesmarcarPareja(long IdMovimiento, long IdLinea)
        {
            if (MessageBox.Show("¿Desea desmarcar la pareja seleccionada?\n\nLas dos partidas volverán a quedar pendientes.",
                "Confirmar desmarcado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string Error = _Controlador.BancosMetDesmarcar(_IdConciliacion, IdMovimiento, IdLinea,
                Environment.UserName, out ClsResultadoConciliacion Resultado);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No se desmarcaron las partidas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BancosMetMostrarResultado(Resultado);
        }

        private bool BancosFuncEsParejaManual(DataGridViewRow Fila)
        {
            return Convert.ToString(Fila.Cells["Origen"].Value) == "Libro y banco"
                && Convert.ToString(Fila.Cells["Metodo"].Value) == "Manual";
        }

        private long BancosFuncIdFila(DataGridViewRow Fila, string Columna)
        {
            object Valor = Fila.Cells[Columna].Value;
            if (Valor == null || Valor == DBNull.Value) return 0;
            return Convert.ToInt64(Valor, CulturaFechas);
        }

        private decimal BancosFuncImporteFila(DataGridViewRow Fila, string Columna)
        {
            object Valor = Fila.Cells[Columna].Value;
            if (Valor == null || Valor == DBNull.Value) return 0m;
            return Convert.ToDecimal(Valor, CulturaFechas);
        }

        private void BancosBtnReporte_Click(object sender, EventArgs e)
        {
            if (_IdConciliacion <= 0)
            {
                MessageBox.Show("Realice primero el corte de conciliación para generar el reporte.", "Falta el corte",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (FrmReporteConciliacionBancaria Reporte = new FrmReporteConciliacionBancaria(_IdConciliacion))
            {
                Reporte.ShowDialog(this);
            }
        }

        private void BancosBtnAyuda_Click(object sender, EventArgs e)
        {
            DirectoryInfo Directorio = new DirectoryInfo(Application.StartupPath);
            while (Directorio != null)
            {
                string Ruta = Path.Combine(Directorio.FullName, "ayuda", "modulos", "bancos", "BancosAyudas", "BancosAyudas.chm");
                if (File.Exists(Ruta))
                {
                    Help.ShowHelp(this, Ruta, HelpNavigator.Topic, "Conciliacion_Bancos.html");
                    return;
                }
                Directorio = Directorio.Parent;
            }
            MessageBox.Show("No se encontró el archivo de ayuda del módulo de Bancos.", "Ayuda de conciliación bancaria",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BancosBtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
