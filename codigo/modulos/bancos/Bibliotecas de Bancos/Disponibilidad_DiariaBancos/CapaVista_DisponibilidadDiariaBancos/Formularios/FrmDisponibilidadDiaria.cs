using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_DisponibilidadDiariaBancos;

namespace CapaVista_DisponibilidadDiaria.Formularios
{
    public partial class FrmDisponibilidadDiaria : Form
    {
        private readonly ClsControladorDisponibilidad _controlador;

        public FrmDisponibilidadDiaria()
        {
            InitializeComponent();
            _controlador = new ClsControladorDisponibilidad();

            try
            {
                if (Properties.Resources.Bancario != null)
                {
                    this.Icon = Icon.FromHandle(Properties.Resources.Bancario.GetHicon());
                }
            }
            catch { }

            BancosMetCargarCatalogos();
            BancosBtnConsultar_Click(this, EventArgs.Empty);
        }

        // ==========================================
        // CARGA INICIAL Y EVENTOS DE INTERFAZ
        // ==========================================

        private void BancosMetCargarCatalogos()
        {
            BancosCboBanco.Items.Clear();
            foreach (var b in _controlador.BancosMetObtenerBancos())
            {
                BancosCboBanco.Items.Add(b);
            }
            if (BancosCboBanco.Items.Count > 0)
            {
                BancosCboBanco.SelectedIndex = 0; // "Todos"
            }

            BancosCboMoneda.Items.Clear();
            BancosCboMoneda.Items.Add("Todas");
            BancosCboMoneda.Items.Add("GTQ");
            BancosCboMoneda.Items.Add("USD");
            BancosCboMoneda.Items.Add("EUR");
            BancosCboMoneda.SelectedIndex = 1; // "GTQ"

            BancosDtpFechaCorte.Value = new DateTime(2026, 10, 13);
        }

        private void BancosCboBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            string banco = BancosCboBanco.SelectedItem?.ToString() ?? "Todos";
            BancosCboCuenta.Items.Clear();
            foreach (var c in _controlador.BancosMetObtenerCuentas(banco))
            {
                BancosCboCuenta.Items.Add(c);
            }
            if (BancosCboCuenta.Items.Count > 0)
            {
                BancosCboCuenta.SelectedIndex = 0;
            }
        }

        private void BancosBtnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fecha = BancosDtpFechaCorte.Value.Date;
                string banco   = BancosCboBanco.SelectedItem?.ToString() ?? "Todos";
                string cuenta  = BancosCboCuenta.SelectedItem?.ToString() ?? "Todas";
                string moneda  = BancosCboMoneda.SelectedItem?.ToString() ?? "GTQ";

                // Detalle por cuenta
                DataTable dt = _controlador.BancosMetConsultarDisponibilidad(fecha, banco, cuenta, moneda);
                BancosDgvDisponibilidad.DataSource = dt;
                BancosMetFormatearGrillaDisponibilidad();

                // Resumen KPI
                string monedaRes = moneda == "Todas" ? "GTQ" : moneda;
                var res = _controlador.BancosMetCalcularResumen(fecha, banco, cuenta, monedaRes);

                string pref = monedaRes == "GTQ" ? "Q" : (monedaRes == "USD" ? "$" : "€");
                BancosLblValorSaldoInicial.Text = $"{pref}{res.SaldoInicial:N2}";
                BancosLblValorIngresos.Text     = $"{pref}{res.Ingresos:N2}";
                BancosLblValorEgresos.Text      = $"{pref}{res.Egresos:N2}";
                BancosLblValorComprometido.Text = $"{pref}{res.Comprometido:N2}";
                BancosLblValorRestringido.Text  = $"{pref}{res.Restringido:N2}";
                BancosLblValorDisponible.Text   = $"{pref}{res.Disponible:N2}";

                BancosLblMonedaResumen.Text = $"Resumen en {monedaRes}";
                BancosLblStatusLine.Text    = $"{dt.Rows.Count} cuentas   •   Importes en {monedaRes}";

                // Cargar compromisos y resumen para la cuenta seleccionada
                BancosMetActualizarPanelesInferiores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar disponibilidad:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BancosDgvDisponibilidad_SelectionChanged(object sender, EventArgs e)
        {
            BancosMetActualizarPanelesInferiores();
        }

        private void BancosMetActualizarPanelesInferiores()
        {
            string cuentaSel = "001-123456-7";
            if (BancosDgvDisponibilidad.SelectedRows.Count > 0)
            {
                var row = BancosDgvDisponibilidad.SelectedRows[0];
                if (row.Cells["Cuenta"] != null && row.Cells["Cuenta"].Value != null)
                {
                    cuentaSel = row.Cells["Cuenta"].Value.ToString();
                }
            }

            // Cargar compromisos
            DataTable dtComp = _controlador.BancosMetConsultarCompromisos(cuentaSel);
            BancosDgvCompromisos.DataSource = dtComp;
            BancosMetFormatearGrillaCompromisos();

            // Cargar resumen de cuenta
            DataTable dtRes = _controlador.BancosMetConsultarResumenCuenta(cuentaSel);
            BancosDgvResumenCuenta.DataSource = dtRes;
            BancosMetFormatearGrillaResumenCuenta();
        }

        private void BancosBtnExportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "La exportación a formatos externos (Excel / PDF) estará disponible al integrar la base de datos MySQL.\n" +
                "Funcionalidad prototipo.",
                "Exportar – Información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BancosBtnFooterAyuda_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Módulo: 8002 – Disponibilidad Diaria\n" +
                "Empresa: Embutidos de Calidad S.A.\n\n" +
                "Permite consultar los saldos consolidados por banco y cuenta bancaria, " +
                "así como los cobros acreditados, pagos programados y fondos en reserva.",
                "Ayuda del Sistema",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BancosBtnFooterReporte_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Generación de reportes de Disponibilidad Diaria.\n" +
                "Los reportes oficiales se emiten al cierre del día.",
                "Reportes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BancosBtnFooterSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ==========================================
        // FORMATEO DE GRIDVIEWS
        // ==========================================

        private void BancosMetFormatearGrillaDisponibilidad()
        {
            if (BancosDgvDisponibilidad.Columns.Count == 0) return;

            string[] colsNumericas = { "Saldo inicial", "Ingresos", "Egresos", "Comprometido", "Restringido", "Disponible" };
            foreach (string col in colsNumericas)
            {
                if (BancosDgvDisponibilidad.Columns.Contains(col))
                {
                    BancosDgvDisponibilidad.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    BancosDgvDisponibilidad.Columns[col].DefaultCellStyle.Format = "N2";
                }
            }

            if (BancosDgvDisponibilidad.Columns.Contains("Banco"))
                BancosDgvDisponibilidad.Columns["Banco"].Width = 110;
            if (BancosDgvDisponibilidad.Columns.Contains("Cuenta"))
                BancosDgvDisponibilidad.Columns["Cuenta"].Width = 110;
            if (BancosDgvDisponibilidad.Columns.Contains("Tipo de cuenta"))
            {
                BancosDgvDisponibilidad.Columns["Tipo de cuenta"].Width = 110;
                BancosDgvDisponibilidad.Columns["Tipo de cuenta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
            if (BancosDgvDisponibilidad.Columns.Contains("Moneda"))
            {
                BancosDgvDisponibilidad.Columns["Moneda"].Width = 70;
                BancosDgvDisponibilidad.Columns["Moneda"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (BancosDgvDisponibilidad.Columns.Contains("Disponible"))
            {
                BancosDgvDisponibilidad.Columns["Disponible"].DefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                BancosDgvDisponibilidad.Columns["Disponible"].DefaultCellStyle.ForeColor = Color.FromArgb(28, 74, 48);
            }
        }

        private void BancosMetFormatearGrillaCompromisos()
        {
            if (BancosDgvCompromisos.Columns.Count == 0) return;

            if (BancosDgvCompromisos.Columns.Contains("Monto"))
            {
                BancosDgvCompromisos.Columns["Monto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                BancosDgvCompromisos.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                BancosDgvCompromisos.Columns["Monto"].DefaultCellStyle.Format = "N2";
                BancosDgvCompromisos.Columns["Monto"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            }
            if (BancosDgvCompromisos.Columns.Contains("Fecha"))
            {
                BancosDgvCompromisos.Columns["Fecha"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                BancosDgvCompromisos.Columns["Fecha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                BancosDgvCompromisos.Columns["Fecha"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            }
            if (BancosDgvCompromisos.Columns.Contains("Concepto"))
                BancosDgvCompromisos.Columns["Concepto"].FillWeight = 45;
            if (BancosDgvCompromisos.Columns.Contains("Beneficiario"))
                BancosDgvCompromisos.Columns["Beneficiario"].FillWeight = 55;
        }

        private void BancosMetFormatearGrillaResumenCuenta()
        {
            if (BancosDgvResumenCuenta.Columns.Count == 0) return;

            if (BancosDgvResumenCuenta.Columns.Contains("Monto"))
            {
                BancosDgvResumenCuenta.Columns["Monto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                BancosDgvResumenCuenta.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                BancosDgvResumenCuenta.Columns["Monto"].DefaultCellStyle.Format = "N2";
                BancosDgvResumenCuenta.Columns["Monto"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            }

            foreach (DataGridViewRow row in BancosDgvResumenCuenta.Rows)
            {
                if (row.Cells["Concepto"]?.Value?.ToString() == "Disponible")
                {
                    row.DefaultCellStyle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(28, 74, 48);
                }
            }
        }
    }
}
