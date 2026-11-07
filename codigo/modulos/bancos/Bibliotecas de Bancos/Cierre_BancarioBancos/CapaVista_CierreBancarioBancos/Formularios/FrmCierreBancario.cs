using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_CierreBancario;

namespace CapaVista_CierreBancario.Formularios
{
    public partial class FrmCierreBancario : Form
    {
        private ClsControladorCierreBancario _Controlador;

        public FrmCierreBancario()
        {
            InitializeComponent();
            _Controlador = new ClsControladorCierreBancario();
        }

        private void FrmCierreBancario_Load(object sender, EventArgs e)
        {
            BancosMetInicializarFormulario();
        }

        private void BancosMetInicializarFormulario()
        {
            BancosCboTipoCierre.Items.Clear();
            BancosCboTipoCierre.Items.Add("Mensual");
            BancosCboTipoCierre.Items.Add("Trimestral");
            BancosCboTipoCierre.Items.Add("Semestral");
            BancosCboTipoCierre.Items.Add("Anual");
            BancosCboTipoCierre.SelectedIndex = 0;

            BancosTxtFechaInicial.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd/MM/yyyy");
            BancosTxFechaFinal.Text = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");

            BancosMetLimpiarValidaciones();
            BancosMetHabilitarBotonesCierre(false);
        }

        private void BancosMetLimpiarValidaciones()
        {
            BancosChkPeriodoValido.Checked = false;
            BancosChkConciliacionesVerificadas.Checked = false;
            BancosChkSaldosVerificados.Checked = false;
            BancosTxtResultadoValidacionesCierre.Text = "Resultado: pendiente de validación";
            BancosTxtEstadoPeriodo.Text = string.Empty;
            BancosDgvResumenCuentas.DataSource = null;
        }

        private void BancosMetHabilitarBotonesCierre(bool Habilitado)
        {
            BancosBtnCerrarPeriodo.Enabled = Habilitado;
        }

        private void BancosBtnConsultar_Click(object sender, EventArgs e)
        {
            BancosMetLimpiarValidaciones();

            string Error = _Controlador.BancosMetConsultar(
                BancosTxtFechaInicial.Text,
                BancosTxFechaFinal.Text,
                out DataTable Datos);

            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Datos == null || Datos.Rows.Count == 0)
            {
                BancosTxtEstadoPeriodo.Text = "Sin registros";
                MessageBox.Show("No se encontraron conciliaciones en el período indicado.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BancosDgvResumenCuentas.DataSource = Datos;
            BancosMetFormatearGrilla();

            string PrimerEstado = Datos.Rows[0]["Estado"].ToString();
            BancosTxtEstadoPeriodo.Text = BancosMetDescribirEstado(Datos);
            BancosTxtAlcance.Text = $"Período: {BancosTxtFechaInicial.Text} al {BancosTxFechaFinal.Text}";
        }

        private string BancosMetDescribirEstado(DataTable Datos)
        {
            bool TieneCerradas = false;
            bool TieneAbiertas = false;
            bool TienePendientes = false;

            foreach (DataRow Fila in Datos.Rows)
            {
                string Estado = Fila["Estado"].ToString();
                if (Estado == "CERRADA") TieneCerradas = true;
                if (Estado == "ABIERTA" || Estado == "PROCESANDO") TieneAbiertas = true;
                if (Estado == "CON_DIFERENCIAS") TienePendientes = true;
            }

            if (TieneCerradas) return "CERRADO";
            if (TieneAbiertas) return "CON PENDIENTES";
            if (TienePendientes) return "CON DIFERENCIAS";
            return "CONCILIADO";
        }

        private void BancosMetFormatearGrilla()
        {
            if (BancosDgvResumenCuentas.Columns.Count == 0) return;

            BancosDgvResumenCuentas.Columns["Pk_Conciliacion"].Visible = false;

            if (BancosDgvResumenCuentas.Columns.Contains("SaldoLibro"))
                BancosDgvResumenCuentas.Columns["SaldoLibro"].DefaultCellStyle.Format = "N2";
            if (BancosDgvResumenCuentas.Columns.Contains("SaldoBanco"))
                BancosDgvResumenCuentas.Columns["SaldoBanco"].DefaultCellStyle.Format = "N2";
            if (BancosDgvResumenCuentas.Columns.Contains("Diferencia"))
                BancosDgvResumenCuentas.Columns["Diferencia"].DefaultCellStyle.Format = "N2";
        }

        private void BancosBtnValidarCierre_Click(object sender, EventArgs e)
        {
            if (BancosDgvResumenCuentas.DataSource == null)
            {
                MessageBox.Show("Primero debe consultar el período a cerrar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string Error = _Controlador.BancosMetValidarCierre(
                BancosTxtFechaInicial.Text,
                BancosTxFechaFinal.Text,
                out bool PeriodoValido,
                out bool ConciliacionesVerificadas,
                out bool SaldosVerificados);

            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BancosChkPeriodoValido.Checked = PeriodoValido;
            BancosChkConciliacionesVerificadas.Checked = ConciliacionesVerificadas;
            BancosChkSaldosVerificados.Checked = SaldosVerificados;

            bool TodoListo = PeriodoValido && ConciliacionesVerificadas && SaldosVerificados;
            BancosTxtResultadoValidacionesCierre.Text = TodoListo
                ? "Resultado: Listo para cerrar"
                : "Resultado: No cumple todas las condiciones";

            BancosMetHabilitarBotonesCierre(TodoListo);

            if (!TodoListo)
            {
                string Detalle = string.Empty;
                if (!PeriodoValido) Detalle += "· El período ya tiene un cierre previo.\n";
                if (!ConciliacionesVerificadas) Detalle += "· Existen conciliaciones aún abiertas o en proceso.\n";
                if (!SaldosVerificados) Detalle += "· Existen cuentas con diferencias pendientes.\n";
                MessageBox.Show(Detalle.TrimEnd(), "Validación incompleta",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BancosBtnCerrarPeriodo_Click(object sender, EventArgs e)
        {
            if (!BancosChkPeriodoValido.Checked ||
                !BancosChkConciliacionesVerificadas.Checked ||
                !BancosChkSaldosVerificados.Checked)
            {
                MessageBox.Show("Debe completar todas las validaciones antes de cerrar el período.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string Responsable = System.Environment.UserName;

            DialogResult Confirmacion = MessageBox.Show(
                $"¿Confirma el cierre del período {BancosTxtFechaInicial.Text} al {BancosTxFechaFinal.Text}?\n\n" +
                "Los documentos del período quedarán bloqueados.\n" +
                "Esta acción no se puede deshacer.",
                "Confirmar cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (Confirmacion != DialogResult.Yes) return;

            string Error = _Controlador.BancosMetCerrarPeriodo(
                BancosTxtFechaInicial.Text,
                BancosTxFechaFinal.Text,
                Responsable);

            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "Error al cerrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Período cerrado correctamente.", "Cierre exitoso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            BancosMetLimpiarValidaciones();
            BancosMetHabilitarBotonesCierre(false);
        }

        private void BancosBtnVerHistorico_Click(object sender, EventArgs e)
        {
            string Error = _Controlador.BancosMetConsultarHistorico(out DataTable Historico);

            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Historico == null || Historico.Rows.Count == 0)
            {
                MessageBox.Show("No existen períodos cerrados en el historial.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BancosDgvResumenCuentas.DataSource = Historico;
        }

        private void BancosBtnReporte_Click(object sender, EventArgs e)
        {
            if (BancosDgvResumenCuentas.DataSource == null)
            {
                MessageBox.Show("No hay datos para generar el reporte. Consulte un período primero.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Módulo de reporte pendiente de implementación.",
                "Reporte", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BancosBtnLimpiar_Click(object sender, EventArgs e)
        {
            BancosMetInicializarFormulario();
        }

        private void BancosBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BancosBtnAyuda_Click(object sender, EventArgs e)
        {
        }
    }
}
