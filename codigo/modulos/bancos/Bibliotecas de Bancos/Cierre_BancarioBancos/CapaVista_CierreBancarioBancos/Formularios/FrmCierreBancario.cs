/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Autor  : Andre De Jesus Gonzalez Camey
 * Carné  : 9959-23-1224 --- 9959-23-3117
 * Fecha  : 08/11/2026
 * ================================================================
 * Propósito :
 *  Presenta el cierre bancario, permite consultar y validar períodos,
 *  cerrar conciliaciones y recorrer o exportar el historial.
 * ================================================================
 */
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CapaControlador_CierreBancario;
using CapaVista_CierreBancario.frmReportes;

namespace CapaVista_CierreBancario.Formularios
{
    public partial class FrmCierreBancario : Form
    {
        private static readonly CultureInfo CulturaFechas = CultureInfo.InvariantCulture;
        private readonly ClsControladorCierreBancario _Controlador;
        private DataTable _DatosPeriodo;
        private DataTable _DatosHistorico;
        public FrmCierreBancario()
        {
            InitializeComponent();
            BancosDgvHistorico.Dock = DockStyle.None;
            BancosLblHistorico.Dock = DockStyle.None;
            BancosLblHistorico.AutoSize = true;
            BancosLblHistorico.Location = new System.Drawing.Point(12, 19);
            BancosBtnActualizarHistorico.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            BancosBtnConsultar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            _Controlador = new ClsControladorCierreBancario();
            BancosCboTipoCierre.SelectedIndexChanged += BancosCboTipoCierre_SelectedIndexChanged;
            BancosPnlHistorico.Resize += BancosPnlHistorico_Resize;
            BancosPnlPeriodoCierre.Resize += BancosPnlPeriodoCierre_Resize;
            BancosBtnInicio.Click += BancosBtnInicio_Click;
            BancosBtnAnterior.Click += BancosBtnAnterior_Click;
            BancosBtnSiguiente.Click += BancosBtnSiguiente_Click;
            BancosBtnFin.Click += BancosBtnFin_Click;
            BancosDgvHistorico.SelectionChanged += BancosDgvHistorico_SelectionChanged;
            BancosMetAjustarVistaHistorico();
            BancosMetAjustarBotonConsulta();
            BancosMetActualizarNavegacionHistorico();
        }

        private void BancosPnlPeriodoCierre_Resize(object sender, EventArgs e)
        {
            BancosMetAjustarBotonConsulta();
        }

        private void BancosMetAjustarBotonConsulta()
        {
            int X = Math.Max(12, BancosPnlPeriodoCierre.ClientSize.Width - BancosBtnConsultar.Width - 12);
            BancosBtnConsultar.Location = new System.Drawing.Point(X, 12);
        }

        private void BancosPnlHistorico_Resize(object sender, EventArgs e)
        {
            BancosMetAjustarVistaHistorico();
        }

        private void BancosMetAjustarVistaHistorico()
        {
            int Ancho = Math.Max(0, BancosPnlHistorico.ClientSize.Width - 24);
            int Alto = Math.Max(0, BancosPnlHistorico.ClientSize.Height - 82);
            const int AnchoBoton = 60;
            const int AltoBoton = 52;
            const int Espacio = 6;
            int X = BancosPnlHistorico.ClientSize.Width - 12 - AnchoBoton;
            BancosBtnActualizarHistorico.SetBounds(X, 6, AnchoBoton, AltoBoton);
            X -= AnchoBoton + Espacio;
            BancosBtnFin.SetBounds(X, 6, AnchoBoton, AltoBoton);
            X -= AnchoBoton + Espacio;
            BancosBtnSiguiente.SetBounds(X, 6, AnchoBoton, AltoBoton);
            X -= AnchoBoton + Espacio;
            BancosBtnAnterior.SetBounds(X, 6, AnchoBoton, AltoBoton);
            X -= AnchoBoton + Espacio;
            BancosBtnInicio.SetBounds(X, 6, AnchoBoton, AltoBoton);
            BancosDgvHistorico.SetBounds(12, 70, Ancho, Alto);
        }

        private void FrmCierreBancario_Load(object sender, EventArgs e)
        {
            BancosCboTipoCierre.Items.Clear();
            BancosCboTipoCierre.Items.AddRange(new object[] { "Mensual", "Trimestral", "Semestral", "Anual" });
            BancosCboTipoCierre.SelectedIndex = 0;
            BancosMetLimpiarValidaciones();
            BancosMetHabilitarCierre(false);
        }

        private void BancosCboTipoCierre_SelectedIndexChanged(object sender, EventArgs e)
        {
            BancosMetActualizarFechasPeriodo();
            BancosMetLimpiarValidaciones();
        }

        private void BancosMetActualizarFechasPeriodo()
        {
            DateTime Hoy = DateTime.Today;
            DateTime Inicio;
            DateTime Fin;
            switch (BancosCboTipoCierre.SelectedItem == null ? string.Empty : BancosCboTipoCierre.SelectedItem.ToString())
            {
                case "Trimestral":
                    DateTime InicioTrimestreActual = new DateTime(Hoy.Year, ((Hoy.Month - 1) / 3) * 3 + 1, 1);
                    Fin = InicioTrimestreActual.AddDays(-1);
                    Inicio = Fin.AddMonths(-2);
                    Inicio = new DateTime(Inicio.Year, Inicio.Month, 1);
                    break;
                case "Semestral":
                    DateTime InicioSemestreActual = new DateTime(Hoy.Year, Hoy.Month <= 6 ? 1 : 7, 1);
                    Fin = InicioSemestreActual.AddDays(-1);
                    Inicio = Fin.AddMonths(-5);
                    Inicio = new DateTime(Inicio.Year, Inicio.Month, 1);
                    break;
                case "Anual":
                    Inicio = new DateTime(Hoy.Year - 1, 1, 1);
                    Fin = new DateTime(Hoy.Year - 1, 12, 31);
                    break;
                default:
                    Inicio = new DateTime(Hoy.Year, Hoy.Month, 1).AddMonths(-1);
                    Fin = Inicio.AddMonths(1).AddDays(-1);
                    break;
            }

            BancosTxtFechaInicial.Text = Inicio.ToString("dd/MM/yyyy", CulturaFechas);
            BancosTxFechaFinal.Text = Fin.ToString("dd/MM/yyyy", CulturaFechas);
        }

        private void BancosMetLimpiarValidaciones()
        {
            _DatosPeriodo = null;
            BancosDgvResumenCuentas.DataSource = null;
            BancosChkPeriodoValido.Checked = false;
            BancosChkConciliacionesVerificadas.Checked = false;
            BancosChkSaldosVerificados.Checked = false;
            BancosTxtEstadoPeriodo.Text = "Sin consultar";
            BancosTxtAlcance.Text = string.Empty;
            BancosTxtResultadoValidacionesCierre.Text = "Pendiente de consulta y validación";
            BancosMetHabilitarCierre(false);
        }

        private void BancosMetHabilitarCierre(bool Habilitado)
        {
            BancosBtnCerrarPeriodo.Enabled = Habilitado;
        }

        private bool BancosMetObtenerFechas(out DateTime FechaInicial, out DateTime FechaFinal)
        {
            bool InicioValido = DateTime.TryParseExact(BancosTxtFechaInicial.Text.Trim(), "dd/MM/yyyy", CulturaFechas,
                DateTimeStyles.None, out FechaInicial);
            bool FinalValido = DateTime.TryParseExact(BancosTxFechaFinal.Text.Trim(), "dd/MM/yyyy", CulturaFechas,
                DateTimeStyles.None, out FechaFinal);
            if (!InicioValido || !FinalValido)
            {
                MessageBox.Show("Ingrese ambas fechas con el formato dd/MM/aaaa.", "Fechas no válidas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (FechaInicial.Date > FechaFinal.Date)
            {
                MessageBox.Show("La fecha inicial debe ser anterior o igual a la fecha final.", "Rango no válido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void BancosBtnConsultar_Click(object sender, EventArgs e)
        {
            BancosMetLimpiarValidaciones();
            if (!BancosMetObtenerFechas(out DateTime FechaInicial, out DateTime FechaFinal)) return;

            string Error = _Controlador.BancosMetConsultar(
                FechaInicial.ToString("dd/MM/yyyy", CulturaFechas),
                FechaFinal.ToString("dd/MM/yyyy", CulturaFechas),
                out DataTable Datos);
            if (!string.IsNullOrEmpty(Error))
            {
                BancosTxtEstadoPeriodo.Text = "Error de consulta";
                MessageBox.Show(Error, "No fue posible consultar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Datos == null || Datos.Rows.Count == 0)
            {
                BancosTxtEstadoPeriodo.Text = "Sin conciliaciones";
                BancosTxtAlcance.Text = string.Format("{0:dd/MM/yyyy} al {1:dd/MM/yyyy}", FechaInicial, FechaFinal);
                MessageBox.Show("No hay conciliaciones registradas en el período seleccionado.", "Sin datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _DatosPeriodo = Datos;
            BancosDgvResumenCuentas.DataSource = Datos;
            BancosMetFormatearGrilla(BancosDgvResumenCuentas);
            BancosTxtEstadoPeriodo.Text = BancosMetDescribirEstado(Datos);
            BancosTxtAlcance.Text = string.Format("{0:dd/MM/yyyy} al {1:dd/MM/yyyy}", FechaInicial, FechaFinal);
        }

        private string BancosMetDescribirEstado(DataTable Datos)
        {
            bool Cerrada = false;
            bool Pendiente = false;
            bool ConDiferencias = false;
            foreach (DataRow Fila in Datos.Rows)
            {
                string Estado = Convert.ToString(Fila["Estado"]);
                if (Estado == "CERRADA") Cerrada = true;
                else if (Estado == "CON_DIFERENCIAS") ConDiferencias = true;
                else if (Estado != "CONCILIADA") Pendiente = true;
            }
            if (Cerrada) return "Cierre previo detectado";
            if (ConDiferencias) return "Hay diferencias por resolver";
            if (Pendiente) return "Hay conciliaciones pendientes";
            return "Conciliaciones listas para validar";
        }

        private void BancosMetFormatearGrilla(DataGridView Grilla)
        {
            if (Grilla.Columns.Contains("IdConciliacion")) Grilla.Columns["IdConciliacion"].Visible = false;
            BancosMetConfigurarColumna(Grilla, "FechaCorte", "Fecha de corte", 80);
            BancosMetConfigurarColumna(Grilla, "FechaInicial", "Fecha inicial", 80);
            BancosMetConfigurarColumna(Grilla, "FechaFinal", "Fecha final", 80);
            BancosMetConfigurarColumna(Grilla, "Cuenta", "Cuenta bancaria", 160);
            BancosMetConfigurarColumna(Grilla, "NumeroCuenta", "Número de cuenta", 120);
            BancosMetConfigurarColumna(Grilla, "Moneda", "Moneda", 70);
            BancosMetConfigurarColumna(Grilla, "Estado", "Estado", 90);
            BancosMetConfigurarColumna(Grilla, "SaldoLibro", "Saldo en libros", 105);
            BancosMetConfigurarColumna(Grilla, "SaldoBanco", "Saldo bancario", 105);
            BancosMetConfigurarColumna(Grilla, "Diferencia", "Diferencia", 95);
            foreach (string Columna in new[] { "SaldoLibro", "SaldoBanco", "Diferencia" })
            {
                if (!Grilla.Columns.Contains(Columna)) continue;
                Grilla.Columns[Columna].DefaultCellStyle.Format = "N4";
                Grilla.Columns[Columna].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (Grilla.Columns.Contains("FechaCorte")) Grilla.Columns["FechaCorte"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (Grilla.Columns.Contains("FechaInicial")) Grilla.Columns["FechaInicial"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (Grilla.Columns.Contains("FechaFinal")) Grilla.Columns["FechaFinal"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (Grilla.Columns.Contains("FechaCierre")) Grilla.Columns["FechaCierre"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        private void BancosMetConfigurarColumna(DataGridView Grilla, string Nombre, string Encabezado, float Ancho)
        {
            if (!Grilla.Columns.Contains(Nombre)) return;
            Grilla.Columns[Nombre].HeaderText = Encabezado;
            Grilla.Columns[Nombre].FillWeight = Ancho;
        }

        private void BancosBtnValidarCierre_Click(object sender, EventArgs e)
        {
            if (_DatosPeriodo == null || _DatosPeriodo.Rows.Count == 0)
            {
                MessageBox.Show("Consulte un período con conciliaciones antes de validar.", "Falta la consulta",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!BancosMetObtenerFechas(out DateTime FechaInicial, out DateTime FechaFinal)) return;

            string Error = _Controlador.BancosMetValidarCierre(
                FechaInicial.ToString("dd/MM/yyyy", CulturaFechas),
                FechaFinal.ToString("dd/MM/yyyy", CulturaFechas),
                out bool PeriodoValido,
                out bool ConciliacionesVerificadas,
                out bool SaldosVerificados);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BancosMetHabilitarCierre(false);
                return;
            }

            BancosChkPeriodoValido.Checked = PeriodoValido;
            BancosChkConciliacionesVerificadas.Checked = ConciliacionesVerificadas;
            BancosChkSaldosVerificados.Checked = SaldosVerificados;
            bool Listo = PeriodoValido && ConciliacionesVerificadas && SaldosVerificados;
            BancosTxtResultadoValidacionesCierre.Text = Listo
                ? "Validación aprobada. El período puede cerrarse."
                : "No se puede cerrar: revise los requisitos pendientes.";
            BancosTxtResultadoValidacionesCierre.ForeColor = Listo
                ? System.Drawing.Color.FromArgb(25, 78, 53)
                : System.Drawing.Color.FromArgb(150, 45, 35);
            BancosTxtResultadoValidacionesCierre.BackColor = Listo
                ? System.Drawing.Color.FromArgb(224, 241, 226)
                : System.Drawing.Color.FromArgb(255, 239, 220);
            BancosMetHabilitarCierre(Listo);
        }

        private void BancosBtnCerrarPeriodo_Click(object sender, EventArgs e)
        {
            if (!BancosBtnCerrarPeriodo.Enabled || _DatosPeriodo == null) return;
            if (!BancosMetObtenerFechas(out DateTime FechaInicial, out DateTime FechaFinal)) return;
            string Mensaje = string.Format(
                "¿Confirma el cierre de las conciliaciones entre {0:dd/MM/yyyy} y {1:dd/MM/yyyy}?\n\n" +
                "Las conciliaciones pasarán a estado CERRADA y no podrán volver a cerrarse.", FechaInicial, FechaFinal);
            if (MessageBox.Show(Mensaje, "Confirmar cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            string Error = _Controlador.BancosMetCerrarPeriodo(
                FechaInicial.ToString("dd/MM/yyyy", CulturaFechas),
                FechaFinal.ToString("dd/MM/yyyy", CulturaFechas));
            if (!string.IsNullOrEmpty(Error))
            {
                BancosMetHabilitarCierre(false);
                MessageBox.Show(Error, "No se completó el cierre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BancosBtnConsultar_Click(sender, e);
                return;
            }

            MessageBox.Show("El período se cerró correctamente.", "Cierre completado",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            BancosMetLimpiarValidaciones();
            BancosBtnVerHistorico_Click(sender, e);
        }

        private void BancosBtnVerHistorico_Click(object sender, EventArgs e)
        {
            string Error = _Controlador.BancosMetConsultarHistorico(out DataTable Historico);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible cargar el historial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DatosHistorico = Historico ?? new DataTable();
            BancosDgvHistorico.DataSource = _DatosHistorico;
            BancosMetFormatearGrilla(BancosDgvHistorico);
            BancosTabCierre.SelectedTab = BancosTabHistorico;
            BancosMetSeleccionarFilaHistorico(ClsNavegacionHistorialCierre.BancosMetInicio(BancosDgvHistorico.Rows.Count));
            if (_DatosHistorico.Rows.Count == 0)
                MessageBox.Show("Todavía no hay cierres registrados.", "Historial vacío",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BancosBtnInicio_Click(object sender, EventArgs e)
        {
            BancosMetSeleccionarFilaHistorico(ClsNavegacionHistorialCierre.BancosMetInicio(BancosDgvHistorico.Rows.Count));
        }

        private void BancosBtnAnterior_Click(object sender, EventArgs e)
        {
            BancosMetSeleccionarFilaHistorico(ClsNavegacionHistorialCierre.BancosMetAnterior(
                BancosDgvHistorico.CurrentRow == null ? -1 : BancosDgvHistorico.CurrentRow.Index,
                BancosDgvHistorico.Rows.Count));
        }

        private void BancosBtnSiguiente_Click(object sender, EventArgs e)
        {
            BancosMetSeleccionarFilaHistorico(ClsNavegacionHistorialCierre.BancosMetSiguiente(
                BancosDgvHistorico.CurrentRow == null ? -1 : BancosDgvHistorico.CurrentRow.Index,
                BancosDgvHistorico.Rows.Count));
        }

        private void BancosBtnFin_Click(object sender, EventArgs e)
        {
            BancosMetSeleccionarFilaHistorico(ClsNavegacionHistorialCierre.BancosMetFin(BancosDgvHistorico.Rows.Count));
        }

        private void BancosDgvHistorico_SelectionChanged(object sender, EventArgs e)
        {
            BancosMetActualizarNavegacionHistorico();
        }

        private void BancosMetSeleccionarFilaHistorico(int Indice)
        {
            if (Indice < 0 || Indice >= BancosDgvHistorico.Rows.Count)
            {
                BancosMetActualizarNavegacionHistorico();
                return;
            }

            BancosDgvHistorico.ClearSelection();
            DataGridViewRow Fila = BancosDgvHistorico.Rows[Indice];
            Fila.Selected = true;
            foreach (DataGridViewColumn Columna in BancosDgvHistorico.Columns)
            {
                if (Columna.Visible)
                {
                    BancosDgvHistorico.CurrentCell = Fila.Cells[Columna.Index];
                    break;
                }
            }
            int PrimeraFilaVisible = BancosDgvHistorico.FirstDisplayedScrollingRowIndex;
            int FilasVisibles = Math.Max(1, BancosDgvHistorico.DisplayedRowCount(false));
            if (PrimeraFilaVisible < 0 || Indice < PrimeraFilaVisible || Indice >= PrimeraFilaVisible + FilasVisibles)
            {
                BancosDgvHistorico.FirstDisplayedScrollingRowIndex = Indice;
            }
            BancosMetActualizarNavegacionHistorico();
        }

        private void BancosMetActualizarNavegacionHistorico()
        {
            int TotalFilas = BancosDgvHistorico.Rows.Count;
            int IndiceActual = BancosDgvHistorico.CurrentRow == null ? -1 : BancosDgvHistorico.CurrentRow.Index;
            bool HayFilas = TotalFilas > 0;
            BancosBtnInicio.Enabled = HayFilas && IndiceActual > 0;
            BancosBtnAnterior.Enabled = HayFilas && IndiceActual > 0;
            BancosBtnSiguiente.Enabled = HayFilas && IndiceActual >= 0 && IndiceActual < TotalFilas - 1;
            BancosBtnFin.Enabled = HayFilas && IndiceActual < TotalFilas - 1;
        }

        private void BancosBtnReporte_Click(object sender, EventArgs e)
        {
            using (FrmReporteCierreBancario Reporte = new FrmReporteCierreBancario())
            {
                Reporte.ShowDialog(this);
            }
        }

        private void BancosBtnExportar_Click(object sender, EventArgs e)
        {
            DataGridView Grilla = BancosTabCierre.SelectedTab == BancosTabHistorico
                ? BancosDgvHistorico
                : BancosDgvResumenCuentas;
            if (Grilla.DataSource == null || Grilla.Rows.Count == 0)
            {
                MessageBox.Show("No hay información para exportar.", "Sin datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog Dialogo = new SaveFileDialog())
            {
                Dialogo.Filter = "Archivo CSV (*.csv)|*.csv";
                Dialogo.FileName = BancosTabCierre.SelectedTab == BancosTabHistorico
                    ? "HistorialCierresBancarios.csv"
                    : "ResumenCierreBancario.csv";
                if (Dialogo.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    using (StreamWriter Escritor = new StreamWriter(Dialogo.FileName, false, new UTF8Encoding(true)))
                    {
                        for (int Col = 0; Col < Grilla.Columns.Count; Col++)
                        {
                            DataGridViewColumn Columna = Grilla.Columns[Col];
                            if (!Columna.Visible) continue;
                            if (Escritor.BaseStream.Position > 0) Escritor.Write(';');
                            Escritor.Write(BancosMetCsv(Columna.HeaderText));
                        }
                        Escritor.WriteLine();
                        foreach (DataGridViewRow Fila in Grilla.Rows)
                        {
                            bool Primera = true;
                            foreach (DataGridViewColumn Columna in Grilla.Columns)
                            {
                                if (!Columna.Visible) continue;
                                if (!Primera) Escritor.Write(';');
                                Escritor.Write(BancosMetCsv(Convert.ToString(Fila.Cells[Columna.Index].FormattedValue)));
                                Primera = false;
                            }
                            Escritor.WriteLine();
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("No se pudo guardar el archivo seleccionado.", "Error de exportación",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string BancosMetCsv(string Valor)
        {
            return "\"" + (Valor ?? string.Empty).Replace("\"", "\"\"") + "\"";
        }

        private void BancosBtnLimpiar_Click(object sender, EventArgs e)
        {
            if (BancosCboTipoCierre.SelectedIndex < 0) BancosCboTipoCierre.SelectedIndex = 0;
            else BancosMetActualizarFechasPeriodo();
            BancosMetLimpiarValidaciones();
        }

        private void BancosBtnAyuda_Click(object sender, EventArgs e)
        {
            DirectoryInfo Directorio = new DirectoryInfo(Application.StartupPath);
            while (Directorio != null)
            {
                string Ruta = Path.Combine(Directorio.FullName, "ayuda", "modulos", "bancos", "BancosAyudas", "BancosAyudas.chm");
                if (File.Exists(Ruta))
                {
                    Help.ShowHelp(this, Ruta, HelpNavigator.Topic, "CierreBancario_Bancos.html");
                    return;
                }
                Directorio = Directorio.Parent;
            }
            MessageBox.Show("No se encontró el archivo de ayuda del módulo de Bancos.", "Ayuda de cierre bancario",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BancosBtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
