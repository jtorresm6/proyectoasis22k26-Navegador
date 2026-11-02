
namespace CapaVista_Componentes.Componentes
{
    partial class ComprasUsrTextBox
    {
        private System.ComponentModel.IContainer _Componentes = null;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            this.ComprasTlpMain = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPnlBorde = new System.Windows.Forms.Panel();
            this.ComprasPnlInterior = new System.Windows.Forms.Panel();
            this.ComprasTxtTexto = new System.Windows.Forms.RichTextBox();
            this.ComprasLblError = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpMain.SuspendLayout();
            this.ComprasPnlBorde.SuspendLayout();
            this.ComprasPnlInterior.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpMain
            // 
            this.ComprasTlpMain.BackColor = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.ComprasTlpMain.ColumnCount = 1;
            this.ComprasTlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMain.Controls.Add(this.ComprasPnlBorde, 0, 0);
            this.ComprasTlpMain.Controls.Add(this.ComprasLblError, 0, 1);
            this.ComprasTlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpMain.ForeColor = CapaVista_Componentes.Componentes.ClsTemaCompras.PrincipalNavegador;
            this.ComprasTlpMain.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpMain.Name = "ComprasTlpMain";
            this.ComprasTlpMain.Padding = new System.Windows.Forms.Padding(3);
            this.ComprasTlpMain.RowCount = 2;
            this.ComprasTlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpMain.Size = new System.Drawing.Size(174, 66);
            this.ComprasTlpMain.TabIndex = 0;
            // 
            // ComprasPnlBorde
            // 
            this.ComprasPnlBorde.BackColor = CapaVista_Componentes.Componentes.ClsTemaCompras.PrincipalNavegador;
            this.ComprasPnlBorde.Controls.Add(this.ComprasPnlInterior);
            this.ComprasPnlBorde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlBorde.Location = new System.Drawing.Point(3, 3);
            this.ComprasPnlBorde.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlBorde.Name = "ComprasPnlBorde";
            this.ComprasPnlBorde.Padding = new System.Windows.Forms.Padding(1);
            this.ComprasPnlBorde.Size = new System.Drawing.Size(168, 30);
            this.ComprasPnlBorde.TabIndex = 2;
            // 
            // ComprasPnlInterior
            // 
            this.ComprasPnlInterior.BackColor = this.BackColor;
            this.ComprasPnlInterior.Controls.Add(this.ComprasTxtTexto);
            this.ComprasPnlInterior.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlInterior.Location = new System.Drawing.Point(1, 1);
            this.ComprasPnlInterior.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlInterior.Name = "ComprasPnlInterior";
            this.ComprasPnlInterior.Padding = new System.Windows.Forms.Padding(6, 1, 6, 1);
            this.ComprasPnlInterior.Size = new System.Drawing.Size(166, 28);
            this.ComprasPnlInterior.TabIndex = 0;
            // 
            // ComprasTxtTexto
            // 
            this.ComprasTxtTexto.BackColor = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.ComprasTxtTexto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ComprasTxtTexto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtTexto.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.ComprasTxtTexto.ForeColor = CapaVista_Componentes.Componentes.ClsTemaCompras.PrincipalNavegador;
            this.ComprasTxtTexto.Location = new System.Drawing.Point(6, 1);
            this.ComprasTxtTexto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtTexto.Multiline = false;
            this.ComprasTxtTexto.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.ComprasTxtTexto.Name = "ComprasTxtTexto";
            this.ComprasTxtTexto.Size = new System.Drawing.Size(154, 26);
            this.ComprasTxtTexto.TabIndex = 0;
            this.ComprasTxtTexto.Text = "";
            // 
            // ComprasUsrError
            // 
            this.ComprasLblError.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblError.BackColor = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.ComprasLblError.ColorTexto = System.Drawing.Color.Red;
            this.ComprasLblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblError.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblError.ForeColor = System.Drawing.Color.Red;
            this.ComprasLblError.Location = new System.Drawing.Point(6, 37);
            this.ComprasLblError.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblError.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblError.MostrarAsterisco = false;
            this.ComprasLblError.Name = "ComprasLblError";
            this.ComprasLblError.Size = new System.Drawing.Size(162, 22);
            this.ComprasLblError.TabIndex = 0;
            this.ComprasLblError.TabStop = false;
            this.ComprasLblError.TamanoTipografia = 9F;
            this.ComprasLblError.Text = "";
            this.ComprasLblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblError.Texto = "";
            // 
            // ComprasUsrTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.Controls.Add(this.ComprasTlpMain);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = CapaVista_Componentes.Componentes.ClsTemaCompras.PrincipalNavegador;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ComprasUsrTextBox";
            this.Size = new System.Drawing.Size(174, 66);
            this.ComprasTlpMain.ResumeLayout(false);
            this.ComprasPnlBorde.ResumeLayout(false);
            this.ComprasPnlInterior.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpMain;
        private System.Windows.Forms.Panel ComprasPnlBorde;
        private System.Windows.Forms.Panel ComprasPnlInterior;
        private ComprasUsrLabel ComprasLblError;
        private System.Windows.Forms.RichTextBox ComprasTxtTexto;
    }
}
