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

        public FrmConciliacion()
        {
            InitializeComponent();
            _Controlador = new ClsControladorConciliacion();
            BancosMetConfigurarColumnas();
            BancosCboCuenta.SelectedIndexChanged += BancosCboCuenta_SelectedIndexChanged;
            BancosCboExtracto.SelectedIndexChanged += BancosCboExtracto_SelectedIndexChanged;
            BancosDtpFechaCorte.ValueChanged += BancosDtpFechaCorte_ValueChanged;
            BancosDgvMovimientos.DataBindingComplete += BancosDgvMovimientos_DataBindingComplete;
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
            BancosMetAgregarColumna("IdMovimiento", "IdMovimiento", 0, false, false);
            BancosMetAgregarColumna("IdLinea", "IdLinea", 0, false, false);
            BancosMetAgregarColumna("Estado", "Estado", 78, true, false);
            BancosMetAgregarColumna("Origen", "Origen", 78, true, false);
            BancosMetAgregarColumna("Fecha", "Fecha", 70, true, false);
            BancosMetAgregarColumna("ReferenciaLibro", "Ref. libro", 75, true, false);
            BancosMetAgregarColumna("ConceptoLibro", "Concepto libro", 125, true, false);
            BancosMetAgregarColumna("ImporteLibro", "Importe libro", 85, true, true);
            BancosMetAgregarColumna("ReferenciaBanco", "Ref. banco", 75, true, false);
            BancosMetAgregarColumna("ConceptoBanco", "Concepto banco", 125, true, false);
            BancosMetAgregarColumna("ImporteBanco", "Importe banco", 85, true, true);
            BancosMetAgregarColumna("Diferencia", "Diferencia", 75, true, true);
            BancosMetAgregarColumna("Metodo", "Método", 75, true, false);
            BancosMetAgregarColumna("Motivo", "Observación", 170, true, false);
        }

        private void BancosMetAgregarColumna(string Campo, string Encabezado, float Ancho, bool Visible, bool Importe)
        {
            DataGridViewTextBoxColumn Columna = new DataGridViewTextBoxColumn();
            Columna.Name = Campo;
            Columna.DataPropertyName = Campo;
            Columna.HeaderText = Encabezado;
            Columna.FillWeight = Ancho > 0 ? Ancho : 1;
            Columna.Visible = Visible;
            Columna.SortMode = DataGridViewColumnSortMode.NotSortable;
            if (Importe)
            {
                Columna.DefaultCellStyle.Format = "N2";
                Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
        }

        private void BancosMetMostrarResultado(ClsResultadoConciliacion Resultado)
        {
            _IdConciliacion = Resultado.IdConciliacion;
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
        }

        private void BancosDgvMovimientos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow Fila in BancosDgvMovimientos.Rows)
            {
                string Estado = Convert.ToString(Fila.Cells["Estado"].Value);
                if (Estado == "Pendiente")
                    Fila.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 224);
                else if (Estado == "Con diferencia")
                    Fila.DefaultCellStyle.BackColor = Color.FromArgb(253, 232, 229);
            }
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

            DataGridViewSelectedRowCollection Seleccion = BancosDgvMovimientos.SelectedRows;
            if (Seleccion.Count == 1 && BancosFuncEsParejaManual(Seleccion[0]))
            {
                BancosMetDesmarcarPareja(Seleccion[0]);
                return;
            }

            DataGridViewRow FilaLibro = null;
            DataGridViewRow FilaBanco = null;
            foreach (DataGridViewRow Fila in Seleccion)
            {
                string Origen = Convert.ToString(Fila.Cells["Origen"].Value);
                if (Origen == "Solo libro") FilaLibro = Fila;
                else if (Origen == "Solo banco") FilaBanco = Fila;
            }

            if (Seleccion.Count != 2 || FilaLibro == null || FilaBanco == null)
            {
                MessageBox.Show("Seleccione con Ctrl una partida del libro y una línea del banco que estén pendientes.\n\n" +
                    "Para desmarcar, seleccione una pareja marcada a mano.", "Selección incompleta",
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

        private void BancosMetDesmarcarPareja(DataGridViewRow Fila)
        {
            if (MessageBox.Show("¿Desea desmarcar la pareja seleccionada?\n\nLas dos partidas volverán a quedar pendientes.",
                "Confirmar desmarcado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string Error = _Controlador.BancosMetDesmarcar(_IdConciliacion,
                BancosFuncIdFila(Fila, "IdMovimiento"), BancosFuncIdFila(Fila, "IdLinea"),
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
