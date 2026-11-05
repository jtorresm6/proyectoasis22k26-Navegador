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
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CapaControlador_TipoCambio;

namespace CapaVista_TipoCambio.Formularios
{
    public partial class FrmTipoCambio : Form
    {
        private readonly ClsControladorTipoCambio _controlador;
        private bool _inicializando = true;

        public FrmTipoCambio()
        {
            InitializeComponent();
            _controlador = new ClsControladorTipoCambio();
            BancosMetInicializarFormulario();
        }

        // ==========================================
        // INICIALIZACIÓN GENERAL DEL FORMULARIO
        // ==========================================

        private void BancosMetInicializarFormulario()
        {
            _inicializando = true;

            // Configurar grillas
            BancosDgvTasasActuales.CellFormatting += BancosDgvTasasActuales_CellFormatting;
            BancosDgvHistorico.CellFormatting += BancosDgvHistorico_CellFormatting;

            // 1. Pestaña: Tasas actuales
            BancosDtpFechaActuales.Value = new DateTime(2026, 10, 13);
            BancosCboMonedaBaseActuales.Items.Clear();
            BancosCboMonedaBaseActuales.Items.Add("GTQ - Quetzal");
            BancosCboMonedaBaseActuales.SelectedIndex = 0;

            BancosCboMonedaActuales.Items.Clear();
            BancosCboMonedaActuales.Items.Add("Todas las monedas");
            BancosCboMonedaActuales.Items.Add("USD - Dólar estadounidense");
            BancosCboMonedaActuales.Items.Add("EUR - Euro");
            BancosCboMonedaActuales.Items.Add("MXN - Peso mexicano");
            BancosCboMonedaActuales.Items.Add("GBP - Libra esterlina");
            BancosCboMonedaActuales.Items.Add("CAD - Dólar canadiense");
            BancosCboMonedaActuales.Items.Add("JPY - Yen japonés");
            BancosCboMonedaActuales.Items.Add("CHF - Franco suizo");
            BancosCboMonedaActuales.Items.Add("CRC - Colón costarricense");
            BancosCboMonedaActuales.Items.Add("HNL - Lempira hondureña");
            BancosCboMonedaActuales.SelectedIndex = 0;

            BancosChkMostrarSinTasa.Checked = true;
            BancosMetCargarTasasActuales();

            // 2. Pestaña: Histórico
            BancosDtpDesde.Value = new DateTime(2026, 10, 9);
            BancosDtpHasta.Value = new DateTime(2026, 10, 13);

            BancosCboMonedaOrigenHist.Items.Clear();
            BancosCboMonedaOrigenHist.Items.AddRange(new object[] { "USD", "EUR", "MXN", "GBP", "CAD", "JPY", "CHF" });
            BancosCboMonedaOrigenHist.SelectedIndex = 0; // USD

            BancosCboMonedaDestinoHist.Items.Clear();
            BancosCboMonedaDestinoHist.Items.AddRange(new object[] { "GTQ", "USD", "EUR" });
            BancosCboMonedaDestinoHist.SelectedIndex = 0; // GTQ

            BancosMetCargarHistorico();

            // 3. Pestaña: Conversor
            BancosCboMonedaOrigenConv.Items.Clear();
            BancosCboMonedaOrigenConv.Items.AddRange(new object[] { "EUR", "USD", "GTQ", "MXN", "GBP", "CAD", "JPY", "CHF" });
            BancosCboMonedaOrigenConv.SelectedIndex = 0; // EUR

            BancosCboMonedaDestinoConv.Items.Clear();
            BancosCboMonedaDestinoConv.Items.AddRange(new object[] { "GTQ", "USD", "EUR", "MXN", "GBP", "CAD" });
            BancosCboMonedaDestinoConv.SelectedIndex = 0; // GTQ

            BancosNumImporteConv.Value = 1000.00m;
            BancosDtpFechaConv.Value = new DateTime(2026, 10, 13);

            _inicializando = false;

            BancosMetActualizarBandera(BancosPicFlagOrigen, "EUR");
            BancosMetActualizarBandera(BancosPicFlagDestino, "GTQ");
            BancosMetRecalcularConversor();
        }

        // ==========================================
        // PESTAÑA: TASAS ACTUALES
        // ==========================================

        private void BancosMetCargarTasasActuales()
        {
            bool mostrarSinTasa = BancosChkMostrarSinTasa.Checked;
            DataTable dt = _controlador.BancosMetObtenerTasasActuales(mostrarSinTasa);

            // Filtrar por moneda si no es 'Todas'
            string filtroMoneda = BancosCboMonedaActuales.SelectedItem?.ToString() ?? "";
            if (!string.IsNullOrEmpty(filtroMoneda) && !filtroMoneda.StartsWith("Todas"))
            {
                string codigo = filtroMoneda.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)[0];
                DataView dv = dt.DefaultView;
                dv.RowFilter = $"Código = '{codigo}'";
                BancosDgvTasasActuales.DataSource = dv.ToTable();
            }
            else
            {
                BancosDgvTasasActuales.DataSource = dt;
            }

            int totalFilas = BancosDgvTasasActuales.Rows.Count;
            BancosLblTasasRefStatusIzq.Text = $"Mostrando {totalFilas} registro{(totalFilas != 1 ? "s" : "")}";
            BancosLblTasasRefStatusDer.Text = $"Fecha de consulta: {BancosDtpFechaActuales.Value:dd/MM/yyyy}";

            if (totalFilas > 0)
            {
                BancosDgvTasasActuales.Rows[0].Selected = true;
                BancosMetActualizarDetalleMoneda();
            }
            else
            {
                BancosLblDetalleCodVal.Text = "—";
                BancosLblDetalleNomVal.Text = "—";
                BancosLblDetalleTasaVal.Text = "—";
                BancosLblDetalleFechaVal.Text = "—";
            }
        }

        private void BancosDgvTasasActuales_SelectionChanged(object sender, EventArgs e)
        {
            BancosMetActualizarDetalleMoneda();
        }

        private void BancosMetActualizarDetalleMoneda()
        {
            if (BancosDgvTasasActuales.SelectedRows.Count > 0)
            {
                var row = BancosDgvTasasActuales.SelectedRows[0];
                string cod = row.Cells["Código"]?.Value?.ToString() ?? "—";
                string nom = row.Cells["Moneda"]?.Value?.ToString() ?? "—";
                string tasa = row.Cells["Tasa en GTQ"]?.Value?.ToString() ?? "—";
                string fecha = row.Cells["Fecha"]?.Value?.ToString() ?? "—";

                BancosLblDetalleCodVal.Text = cod;
                BancosLblDetalleNomVal.Text = nom;
                BancosLblDetalleTasaVal.Text = tasa == "—" ? "Sin cotización" : $"1 {cod} = {tasa} GTQ";
                BancosLblDetalleFechaVal.Text = fecha == "—" ? "—" : $"{fecha}  ·  Fuente: Demostración";
            }
        }

        private void BancosBtnConsultarActuales_Click(object sender, EventArgs e)
        {
            BancosMetCargarTasasActuales();
        }

        private void BancosBtnExportarActuales_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Exportación de tasas de referencia preparada.\nFormato: Excel (.xlsx) y CSV.\nLos datos han sido preparados para su descarga.",
                "Exportación de Tasas Actuales",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BancosCboMonedaActuales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_inicializando)
            {
                BancosMetCargarTasasActuales();
            }
        }

        private void BancosChkMostrarSinTasa_CheckedChanged(object sender, EventArgs e)
        {
            if (!_inicializando)
            {
                BancosMetCargarTasasActuales();
            }
        }

        private void BancosDgvTasasActuales_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= BancosDgvTasasActuales.Rows.Count) return;

            if (BancosDgvTasasActuales.Columns[e.ColumnIndex].Name == "Estado")
            {
                string estado = e.Value?.ToString();
                if (estado == "Vigente")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(28, 74, 48); // Verde
                    e.CellStyle.Font = new Font(BancosDgvTasasActuales.Font, FontStyle.Bold);
                }
                else if (estado == "Sin tasa")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(180, 83, 9); // Ámbar
                    e.CellStyle.Font = new Font(BancosDgvTasasActuales.Font, FontStyle.Bold);
                }
            }
            else if (BancosDgvTasasActuales.Columns[e.ColumnIndex].Name == "Tasa en GTQ")
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                e.CellStyle.Font = new Font("Consolas", 9.5F, FontStyle.Bold);
            }
            else if (BancosDgvTasasActuales.Columns[e.ColumnIndex].Name == "Código")
            {
                e.CellStyle.Font = new Font(BancosDgvTasasActuales.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(28, 74, 48);
            }
        }

        // ==========================================
        // PESTAÑA: HISTÓRICO
        // ==========================================

        private void BancosMetCargarHistorico()
        {
            DateTime desde = BancosDtpDesde.Value.Date;
            DateTime hasta = BancosDtpHasta.Value.Date;
            string orig = BancosCboMonedaOrigenHist.SelectedItem?.ToString() ?? "USD";
            string dest = BancosCboMonedaDestinoHist.SelectedItem?.ToString() ?? "GTQ";

            BancosLblHistParDer.Text = $"Par: {orig} → {dest}";

            DataTable dt = _controlador.BancosMetObtenerHistorico(desde, hasta, orig, dest);
            BancosDgvHistorico.DataSource = dt;

            int count = BancosDgvHistorico.Rows.Count;
            BancosLblHistStatusIzq.Text = $"Mostrando {count} registros históricos";
            BancosLblHistStatusDer.Text = $"Rango: {desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}";

            var resumen = _controlador.BancosMetObtenerResumenHistorico();
            BancosLblKpiMinValor.Text = resumen.Minima.ToString("F6");
            BancosLblKpiMaxValor.Text = resumen.Maxima.ToString("F6");
            BancosLblKpiVarValor.Text = $"+{resumen.Variacion:F6}";
        }

        private void BancosBtnBuscarHist_Click(object sender, EventArgs e)
        {
            BancosMetCargarHistorico();
        }

        private void BancosBtnExportarHist_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Exportación de tasas históricas preparada.\nFormato: Excel (.xlsx) y CSV.\nLos datos seleccionados han sido enviados a cola de exportación.",
                "Exportación de Tasas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BancosDgvHistorico_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= BancosDgvHistorico.Rows.Count) return;

            if (BancosDgvHistorico.Columns[e.ColumnIndex].Name == "Estado")
            {
                string estado = e.Value?.ToString();
                if (estado == "Vigente")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(28, 74, 48);
                    e.CellStyle.Font = new Font(BancosDgvHistorico.Font, FontStyle.Bold);
                }
            }
            else if (BancosDgvHistorico.Columns[e.ColumnIndex].Name == "Tasa")
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                e.CellStyle.Font = new Font("Consolas", 9.5F, FontStyle.Bold);
            }
            else if (BancosDgvHistorico.Columns[e.ColumnIndex].Name == "Moneda origen" ||
                     BancosDgvHistorico.Columns[e.ColumnIndex].Name == "Moneda destino")
            {
                e.CellStyle.Font = new Font(BancosDgvHistorico.Font, FontStyle.Bold);
            }
        }

        // ==========================================
        // PESTAÑA: CONVERSOR
        // ==========================================

        private void BancosCboMonedaOrigenConv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_inicializando) return;
            string orig = BancosCboMonedaOrigenConv.SelectedItem?.ToString() ?? "EUR";
            BancosMetActualizarBandera(BancosPicFlagOrigen, orig);
            BancosMetRecalcularConversor();
        }

        private void BancosCboMonedaDestinoConv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_inicializando) return;
            string dest = BancosCboMonedaDestinoConv.SelectedItem?.ToString() ?? "GTQ";
            BancosMetActualizarBandera(BancosPicFlagDestino, dest);
            BancosMetRecalcularConversor();
        }

        private void BancosBtnIntercambiar_Click(object sender, EventArgs e)
        {
            _inicializando = true;
            int idxOrig = BancosCboMonedaOrigenConv.SelectedIndex;
            string txtOrig = BancosCboMonedaOrigenConv.SelectedItem?.ToString() ?? "EUR";
            string txtDest = BancosCboMonedaDestinoConv.SelectedItem?.ToString() ?? "GTQ";

            // Intercambiar selección
            if (!BancosCboMonedaOrigenConv.Items.Contains(txtDest))
                BancosCboMonedaOrigenConv.Items.Add(txtDest);
            if (!BancosCboMonedaDestinoConv.Items.Contains(txtOrig))
                BancosCboMonedaDestinoConv.Items.Add(txtOrig);

            BancosCboMonedaOrigenConv.SelectedItem = txtDest;
            BancosCboMonedaDestinoConv.SelectedItem = txtOrig;

            _inicializando = false;

            BancosMetActualizarBandera(BancosPicFlagOrigen, txtDest);
            BancosMetActualizarBandera(BancosPicFlagDestino, txtOrig);
            BancosMetRecalcularConversor();
        }

        private void BancosNumImporteConv_ValueChanged(object sender, EventArgs e)
        {
            if (!_inicializando)
            {
                BancosMetRecalcularConversor();
            }
        }

        private void BancosBtnCalcular_Click(object sender, EventArgs e)
        {
            BancosMetRecalcularConversor();
        }

        private void BancosBtnVerHistorico_Click(object sender, EventArgs e)
        {
            string orig = BancosCboMonedaOrigenConv.SelectedItem?.ToString() ?? "USD";
            string dest = BancosCboMonedaDestinoConv.SelectedItem?.ToString() ?? "GTQ";

            if (BancosCboMonedaOrigenHist.Items.Contains(orig))
                BancosCboMonedaOrigenHist.SelectedItem = orig;

            if (BancosCboMonedaDestinoHist.Items.Contains(dest))
                BancosCboMonedaDestinoHist.SelectedItem = dest;

            BancosTabControl.SelectedTab = BancosTabHistorico;
            BancosMetCargarHistorico();
        }

        private void BancosMetRecalcularConversor()
        {
            decimal importe = BancosNumImporteConv.Value;
            string orig = BancosCboMonedaOrigenConv.SelectedItem?.ToString() ?? "EUR";
            string dest = BancosCboMonedaDestinoConv.SelectedItem?.ToString() ?? "GTQ";

            var (resultado, tasa, _) = _controlador.BancosMetConvertir(importe, orig, dest);

            string prefDest = dest == "GTQ" ? "Q " : (dest == "USD" ? "$ " : (dest == "EUR" ? "€ " : $"{dest} "));
            BancosLblConvResultadoGrande.Text = $"{prefDest}{resultado:N2}";
            BancosLblConvCalculoDetalle.Text = $"{importe:N2} {orig} × {tasa:F6}";
            BancosLblConvTasaDetalle.Text = $"Tasa aplicada: 1 {orig} = {tasa:F6} {dest}";

            BancosLblConvParVal.Text = $"{orig} → {dest}";
            BancosLblConvFechaVal.Text = $"{BancosDtpFechaConv.Value:dd/MM/yyyy}";
            BancosLblConvFuenteVal.Text = "Demostración";
            BancosLblConvDispVal.Text = "●  Con tasa";
            BancosLblConvDispVal.ForeColor = Color.FromArgb(28, 74, 48);
        }

        private void BancosMetActualizarBandera(PictureBox pic, string codigoMoneda)
        {
            if (pic == null || string.IsNullOrWhiteSpace(codigoMoneda)) return;
            Image flag = BancosMetObtenerBandera(codigoMoneda);
            if (flag != null)
            {
                pic.Image = flag;
            }
        }

        private Image BancosMetObtenerBandera(string codigoMoneda)
        {
            if (string.IsNullOrWhiteSpace(codigoMoneda)) return null;
            codigoMoneda = codigoMoneda.Trim().ToUpperInvariant();

            // 1. Desde Properties.Resources
            try
            {
                object obj = Properties.Resources.ResourceManager.GetObject(codigoMoneda);
                if (obj is Image imgRes) return imgRes;
            }
            catch { }

            // 2. Desde archivos en disco (Resouces/Banderas)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] rutas = new[]
            {
                Path.Combine(baseDir, "Resouces", "Banderas", $"{codigoMoneda}.png"),
                Path.Combine(baseDir, "..", "..", "Resouces", "Banderas", $"{codigoMoneda}.png"),
                Path.Combine(baseDir, "..", "..", "TipoCambio", "CapaVista_TipoCambio", "Resouces", "Banderas", $"{codigoMoneda}.png")
            };

            foreach (var r in rutas)
            {
                try
                {
                    if (File.Exists(r))
                    {
                        using (var fs = new FileStream(r, FileMode.Open, FileAccess.Read))
                        {
                            return Image.FromStream(fs);
                        }
                    }
                }
                catch { }
            }
            return null;
        }

        // ==========================================
        // PANEL DERECHO: BANGUAT
        // ==========================================

        private void BancosBtnActualizarBanguat_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_controlador.BancosMetActualizarDesdeBanguat(),
                "Actualización – Banguat",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private Image BancosMetRedimensionar(Image img, int width, int height)
        {
            if (img == null) return null;
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.DrawImage(img, 0, 0, width, height);
            }
            return bmp;
        }

        // ==========================================
        // BARRA INFERIOR (FOOTER)
        // ==========================================

        private void BancosBtnFooterAyuda_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Módulo de Tipo de Cambio\nERP Tesorería y Bancos — Embutidos de Calidad S.A.\n\n" +
                "Funcionalidades disponibles:\n" +
                "• Tasas actuales: Consulta de tipos de cambio de referencia del Banco de Guatemala.\n" +
                "• Histórico: Registro y seguimiento de variaciones de cotización en el tiempo.\n" +
                "• Conversor: Simulación precisa de operaciones multimoneda.",
                "Ayuda del Sistema",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BancosBtnFooterReporte_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Generador de Reportes de Tipo de Cambio.\n" +
                "Puede exportar el consolidado histórico a Excel/PDF desde la pestaña 'Histórico'.",
                "Reportes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BancosBtnFooterSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
