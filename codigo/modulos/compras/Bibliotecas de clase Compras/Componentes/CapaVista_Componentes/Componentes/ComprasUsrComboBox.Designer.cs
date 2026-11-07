
namespace CapaVista_Componentes.Componentes
{
    partial class ComprasUsrComboBox
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
            this.ComprasCboEntrada = new CapaVista_Componentes.Componentes.ClsSelectorCompras();
            this.ComprasLblError = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpMain.SuspendLayout();
            this.ComprasPnlBorde.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpMain
            // 
            this.ComprasTlpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpMain.ColumnCount = 1;
            this.ComprasTlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMain.Controls.Add(this.ComprasPnlBorde, 0, 0);
            this.ComprasTlpMain.Controls.Add(this.ComprasLblError, 0, 1);
            this.ComprasTlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
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
            this.ComprasPnlBorde.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlBorde.Controls.Add(this.ComprasCboEntrada);
            this.ComprasPnlBorde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlBorde.Location = new System.Drawing.Point(3, 3);
            this.ComprasPnlBorde.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlBorde.Name = "ComprasPnlBorde";
            this.ComprasPnlBorde.Padding = new System.Windows.Forms.Padding(1);
            this.ComprasPnlBorde.Size = new System.Drawing.Size(168, 30);
            this.ComprasPnlBorde.TabIndex = 2;
            // 
            // ComprasCboEntrada
            // 
            this.ComprasCboEntrada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboEntrada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboEntrada.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComprasCboEntrada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasCboEntrada.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboEntrada.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboEntrada.Location = new System.Drawing.Point(1, 1);
            this.ComprasCboEntrada.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboEntrada.Name = "ComprasCboEntrada";
            this.ComprasCboEntrada.Size = new System.Drawing.Size(166, 31);
            this.ComprasCboEntrada.TabIndex = 0;
            // 
            // ComprasLblError
            // 
            this.ComprasLblError.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
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
            // ComprasUsrComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.ComprasTlpMain);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ComprasUsrComboBox";
            this.Size = new System.Drawing.Size(174, 66);
            this.ComprasTlpMain.ResumeLayout(false);
            this.ComprasPnlBorde.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpMain;
        private System.Windows.Forms.Panel ComprasPnlBorde;
        private ComprasUsrLabel ComprasLblError;
        private CapaVista_Componentes.Componentes.ClsSelectorCompras ComprasCboEntrada;
    }
}
