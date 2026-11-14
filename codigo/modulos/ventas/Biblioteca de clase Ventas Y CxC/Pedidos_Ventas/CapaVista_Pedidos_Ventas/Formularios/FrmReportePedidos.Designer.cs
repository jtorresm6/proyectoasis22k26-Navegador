/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Seleccionar los filtros del reporte de pedidos.
 */
namespace CapaVista_Pedidos_Ventas
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Define los controles del filtro del reporte de pedidos.
    partial class FrmReportePedidos
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel VentasPnlEncabezado;
        private System.Windows.Forms.Label VentasLblTitulo;
        private System.Windows.Forms.Label VentasLblSubtitulo;
        private System.Windows.Forms.Label VentasLblDesde;
        private System.Windows.Forms.Label VentasLblHasta;
        private System.Windows.Forms.Label VentasLblEstado;
        private System.Windows.Forms.DateTimePicker VentasDtpDesde;
        private System.Windows.Forms.DateTimePicker VentasDtpHasta;
        private System.Windows.Forms.ComboBox VentasCboEstadoReporte;
        private System.Windows.Forms.Button VentasBtnGenerarReporte;
        private System.Windows.Forms.Button VentasBtnCancelarReporte;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasLblDesde = new System.Windows.Forms.Label();
            this.VentasLblHasta = new System.Windows.Forms.Label();
            this.VentasLblEstado = new System.Windows.Forms.Label();
            this.VentasDtpDesde = new System.Windows.Forms.DateTimePicker();
            this.VentasDtpHasta = new System.Windows.Forms.DateTimePicker();
            this.VentasCboEstadoReporte = new System.Windows.Forms.ComboBox();
            this.VentasBtnGenerarReporte = new System.Windows.Forms.Button();
            this.VentasBtnCancelarReporte = new System.Windows.Forms.Button();
            this.VentasPnlEncabezado.SuspendLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReportePedidos));
            this.SuspendLayout();
            this.VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.VentasPnlEncabezado.Height = 94;
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasLblTitulo.AutoSize = true;
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Sans", 19F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.White;
            this.VentasLblTitulo.Location = new System.Drawing.Point(24, 16);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Text = "Reporte de pedidos";
            this.VentasLblSubtitulo.AutoSize = true;
            this.VentasLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(201, 162, 74);
            this.VentasLblSubtitulo.Location = new System.Drawing.Point(28, 59);
            this.VentasLblSubtitulo.Name = "VentasLblSubtitulo";
            this.VentasLblSubtitulo.Text = "VENTAS Y CUENTAS POR COBRAR  |  7004";
            this.VentasLblDesde.AutoSize = true;
            this.VentasLblDesde.Location = new System.Drawing.Point(28, 125);
            this.VentasLblDesde.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDesde.Name = "VentasLblDesde";
            this.VentasLblDesde.Text = "Fecha desde";
            this.VentasDtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpDesde.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpDesde.Location = new System.Drawing.Point(28, 150);
            this.VentasDtpDesde.Name = "VentasDtpDesde";
            this.VentasDtpDesde.Width = 240;
            this.VentasLblHasta.AutoSize = true;
            this.VentasLblHasta.Location = new System.Drawing.Point(292, 125);
            this.VentasLblHasta.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblHasta.Name = "VentasLblHasta";
            this.VentasLblHasta.Text = "Fecha hasta";
            this.VentasDtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpHasta.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpHasta.Location = new System.Drawing.Point(292, 150);
            this.VentasDtpHasta.Name = "VentasDtpHasta";
            this.VentasDtpHasta.Width = 240;
            this.VentasLblEstado.AutoSize = true;
            this.VentasLblEstado.Location = new System.Drawing.Point(28, 193);
            this.VentasLblEstado.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblEstado.Name = "VentasLblEstado";
            this.VentasLblEstado.Text = "Estado";
            this.VentasCboEstadoReporte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboEstadoReporte.Items.AddRange(new object[] { "Todos los estados", "PENDIENTE", "CONFIRMADO", "COMPLETADO", "ANULADO" });
            this.VentasCboEstadoReporte.Location = new System.Drawing.Point(28, 218);
            this.VentasCboEstadoReporte.Name = "VentasCboEstadoReporte";
            this.VentasCboEstadoReporte.Size = new System.Drawing.Size(504, 31);
            this.VentasCboEstadoReporte.SelectedIndex = 0;
            this.VentasBtnGenerarReporte.BackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnGenerarReporte.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalImprimirVentas")));
            this.VentasBtnGenerarReporte.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnGenerarReporte.FlatAppearance.BorderSize = 0;
            this.VentasBtnGenerarReporte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnGenerarReporte.ForeColor = System.Drawing.Color.White;
            this.VentasBtnGenerarReporte.Location = new System.Drawing.Point(468, 263);
            this.VentasBtnGenerarReporte.Name = "VentasBtnGenerarReporte";
            this.VentasBtnGenerarReporte.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnGenerarReporte.Text = string.Empty;
            this.VentasBtnGenerarReporte.AccessibleName = "Generar reporte";
            this.VentasBtnGenerarReporte.UseVisualStyleBackColor = false;
            this.VentasBtnGenerarReporte.Click += new System.EventHandler(this.VentasMetMostrarReporte);
            this.VentasBtnCancelarReporte.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.VentasBtnCancelarReporte.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalCancelarVentas")));
            this.VentasBtnCancelarReporte.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnCancelarReporte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCancelarReporte.FlatAppearance.BorderSize = 0;
            this.VentasBtnCancelarReporte.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnCancelarReporte.Location = new System.Drawing.Point(392, 263);
            this.VentasBtnCancelarReporte.Name = "VentasBtnCancelarReporte";
            this.VentasBtnCancelarReporte.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnCancelarReporte.Text = string.Empty;
            this.VentasBtnCancelarReporte.AccessibleName = "Cancelar";
            this.AcceptButton = this.VentasBtnGenerarReporte;
            this.CancelButton = this.VentasBtnCancelarReporte;
            this.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.ClientSize = new System.Drawing.Size(560, 347);
            this.Controls.Add(this.VentasBtnCancelarReporte);
            this.Controls.Add(this.VentasBtnGenerarReporte);
            this.Controls.Add(this.VentasCboEstadoReporte);
            this.Controls.Add(this.VentasLblEstado);
            this.Controls.Add(this.VentasDtpHasta);
            this.Controls.Add(this.VentasLblHasta);
            this.Controls.Add(this.VentasDtpDesde);
            this.Controls.Add(this.VentasLblDesde);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("IconoVentas")));
            this.Name = "FrmReportePedidos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7004 – FiltroReportePedidos";
            this.VentasPnlEncabezado.ResumeLayout(false);
            this.VentasPnlEncabezado.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
