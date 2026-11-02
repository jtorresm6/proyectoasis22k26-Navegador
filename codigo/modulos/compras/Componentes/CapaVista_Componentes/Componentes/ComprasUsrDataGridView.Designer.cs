namespace CapaVista_Componentes.Componentes
{
    partial class ComprasUsrDataGridView
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
            System.Windows.Forms.DataGridViewCellStyle EstiloFilasAlternas = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloEncabezados = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCeldas = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloFilas = new System.Windows.Forms.DataGridViewCellStyle();
            this.ComprasPnlBorde = new System.Windows.Forms.Panel();
            this.ComprasDgvMain = new System.Windows.Forms.DataGridView();
            this.ComprasPnlBorde.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasDgvMain)).BeginInit();
            this.SuspendLayout();
            this.ComprasPnlBorde.BackColor = ClsTemaCompras.PrincipalNavegador;
            this.ComprasPnlBorde.Controls.Add(this.ComprasDgvMain);
            this.ComprasPnlBorde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlBorde.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlBorde.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlBorde.Name = "ComprasPnlBorde";
            this.ComprasPnlBorde.Padding = new System.Windows.Forms.Padding(1);
            this.ComprasPnlBorde.Size = new System.Drawing.Size(150, 150);
            this.ComprasPnlBorde.TabIndex = 0;
            this.ComprasDgvMain.AllowUserToAddRows = false;
            this.ComprasDgvMain.AllowUserToDeleteRows = false;
            this.ComprasDgvMain.AllowUserToResizeRows = false;
            EstiloFilasAlternas.BackColor = ClsTemaCompras.FondoBotones;
            EstiloFilasAlternas.ForeColor = ClsTemaCompras.PrincipalNavegador;
            EstiloFilasAlternas.SelectionBackColor = ClsTemaCompras.AcentoNavegador;
            EstiloFilasAlternas.SelectionForeColor = System.Drawing.Color.White;
            this.ComprasDgvMain.AlternatingRowsDefaultCellStyle = EstiloFilasAlternas;
            this.ComprasDgvMain.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ComprasDgvMain.BackgroundColor = ClsTemaCompras.FondoGeneral;
            this.ComprasDgvMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ComprasDgvMain.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.ComprasDgvMain.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            EstiloEncabezados.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloEncabezados.BackColor = ClsTemaCompras.PrincipalNavegador;
            EstiloEncabezados.Font = new System.Drawing.Font("Segoe UI", 9F);
            EstiloEncabezados.ForeColor = System.Drawing.Color.White;
            EstiloEncabezados.SelectionBackColor = ClsTemaCompras.AcentoNavegador;
            EstiloEncabezados.SelectionForeColor = System.Drawing.Color.White;
            this.ComprasDgvMain.ColumnHeadersDefaultCellStyle = EstiloEncabezados;
            this.ComprasDgvMain.ColumnHeadersHeight = 32;
            this.ComprasDgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            EstiloCeldas.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCeldas.BackColor = ClsTemaCompras.FondoGeneral;
            EstiloCeldas.Font = new System.Drawing.Font("Segoe UI", 9F);
            EstiloCeldas.ForeColor = ClsTemaCompras.PrincipalNavegador;
            EstiloCeldas.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            EstiloCeldas.SelectionBackColor = ClsTemaCompras.AcentoNavegador;
            EstiloCeldas.SelectionForeColor = System.Drawing.Color.White;
            EstiloCeldas.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ComprasDgvMain.DefaultCellStyle = EstiloCeldas;
            this.ComprasDgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ComprasDgvMain.EnableHeadersVisualStyles = false;
            this.ComprasDgvMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ComprasDgvMain.GridColor = ClsTemaCompras.FondoBotones;
            this.ComprasDgvMain.Location = new System.Drawing.Point(1, 1);
            this.ComprasDgvMain.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvMain.MultiSelect = false;
            this.ComprasDgvMain.Name = "ComprasDgvMain";
            this.ComprasDgvMain.ReadOnly = true;
            this.ComprasDgvMain.RowHeadersVisible = false;
            this.ComprasDgvMain.RowHeadersWidth = 51;
            EstiloFilas.BackColor = ClsTemaCompras.FondoGeneral;
            EstiloFilas.ForeColor = ClsTemaCompras.PrincipalNavegador;
            EstiloFilas.SelectionBackColor = ClsTemaCompras.AcentoNavegador;
            EstiloFilas.SelectionForeColor = System.Drawing.Color.White;
            this.ComprasDgvMain.RowsDefaultCellStyle = EstiloFilas;
            this.ComprasDgvMain.RowTemplate.Height = 28;
            this.ComprasDgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ComprasDgvMain.Size = new System.Drawing.Size(148, 148);
            this.ComprasDgvMain.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = ClsTemaCompras.FondoGeneral;
            this.ForeColor = ClsTemaCompras.PrincipalNavegador;
            this.Controls.Add(this.ComprasPnlBorde);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ComprasUsrDataGridView";
            this.ComprasPnlBorde.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasDgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ComprasPnlBorde;
        private System.Windows.Forms.DataGridView ComprasDgvMain;
    }
}
