namespace CapaVista_AprobacionFinanciera
{
    partial class FrmRechazarOrden
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRechazarOrden));
            this.ComprasTlpContenidoVisible = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasTxtComentario = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTlpAcciones = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasBtnSiRechazar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnNoRechazar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpFormulario = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasUsrEncabezadoFormulario = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasLblEtiquetaComentario = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpContenidoVisible.SuspendLayout();
            this.ComprasTlpAcciones.SuspendLayout();
            this.ComprasTlpFormulario.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasPnlDesplazamiento.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpContenidoVisible
            // 
            this.ComprasTlpContenidoVisible.ColumnCount = 1;
            this.ComprasTlpContenidoVisible.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpContenidoVisible.Controls.Add(this.ComprasTxtComentario, 0, 0);
            this.ComprasTlpContenidoVisible.Controls.Add(this.ComprasTlpAcciones, 0, 1);
            this.ComprasTlpContenidoVisible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpContenidoVisible.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpContenidoVisible.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpContenidoVisible.Name = "ComprasTlpContenidoVisible";
            this.ComprasTlpContenidoVisible.Padding = new System.Windows.Forms.Padding(10);
            this.ComprasTlpContenidoVisible.RowCount = 2;
            this.ComprasTlpContenidoVisible.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpContenidoVisible.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpContenidoVisible.Size = new System.Drawing.Size(1300, 550);
            this.ComprasTlpContenidoVisible.TabIndex = 0;
            // 
            // ComprasTxtComentario
            // 
            this.ComprasTxtComentario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtComentario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtComentario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtComentario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtComentario.Location = new System.Drawing.Point(10, 10);
            this.ComprasTxtComentario.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtComentario.MaxLength = 2147483647;
            this.ComprasTxtComentario.Name = "ComprasTxtComentario";
            this.ComprasTxtComentario.SelectedText = "";
            this.ComprasTxtComentario.SelectionLength = 0;
            this.ComprasTxtComentario.SelectionStart = 0;
            this.ComprasTxtComentario.Size = new System.Drawing.Size(1280, 430);
            this.ComprasTxtComentario.TabIndex = 1;
            // 
            // ComprasTlpAcciones
            // 
            this.ComprasTlpAcciones.ColumnCount = 2;
            this.ComprasTlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpAcciones.Controls.Add(this.ComprasBtnSiRechazar, 0, 0);
            this.ComprasTlpAcciones.Controls.Add(this.ComprasBtnNoRechazar, 1, 0);
            this.ComprasTlpAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpAcciones.Location = new System.Drawing.Point(10, 440);
            this.ComprasTlpAcciones.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpAcciones.Name = "ComprasTlpAcciones";
            this.ComprasTlpAcciones.RowCount = 1;
            this.ComprasTlpAcciones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAcciones.Size = new System.Drawing.Size(1280, 100);
            this.ComprasTlpAcciones.TabIndex = 2;
            // 
            // ComprasBtnSiRechazar
            // 
            this.ComprasBtnSiRechazar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnSiRechazar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnSiRechazar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnSiRechazar.BackgroundImage")));
            this.ComprasBtnSiRechazar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnSiRechazar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnSiRechazar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnSiRechazar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnSiRechazar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnSiRechazar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnSiRechazar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnSiRechazar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnSiRechazar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnSiRechazar.Location = new System.Drawing.Point(280, 10);
            this.ComprasBtnSiRechazar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnSiRechazar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnSiRechazar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnSiRechazar.Name = "ComprasBtnSiRechazar";
            this.ComprasBtnSiRechazar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnSiRechazar.TabIndex = 5;
            this.ComprasBtnSiRechazar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnNoRechazar
            // 
            this.ComprasBtnNoRechazar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnNoRechazar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnNoRechazar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnNoRechazar.BackgroundImage")));
            this.ComprasBtnNoRechazar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnNoRechazar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnNoRechazar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNoRechazar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnNoRechazar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnNoRechazar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnNoRechazar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnNoRechazar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnNoRechazar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNoRechazar.Location = new System.Drawing.Point(920, 10);
            this.ComprasBtnNoRechazar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnNoRechazar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnNoRechazar.MinimumSize = new System.Drawing.Size(80, 20);
            this.ComprasBtnNoRechazar.Name = "ComprasBtnNoRechazar";
            this.ComprasBtnNoRechazar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnNoRechazar.TabIndex = 6;
            this.ComprasBtnNoRechazar.UseVisualStyleBackColor = false;
            // 
            // ComprasTlpFormulario
            // 
            this.ComprasTlpFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFormulario.ColumnCount = 1;
            this.ComprasTlpFormulario.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFormulario.Controls.Add(this.ComprasUsrEncabezadoFormulario, 0, 0);
            this.ComprasTlpFormulario.Controls.Add(this.ComprasPnlContenido, 0, 1);
            this.ComprasTlpFormulario.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTlpFormulario.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpFormulario.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFormulario.Name = "ComprasTlpFormulario";
            this.ComprasTlpFormulario.RowCount = 2;
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFormulario.Size = new System.Drawing.Size(1300, 700);
            this.ComprasTlpFormulario.TabIndex = 0;
            // 
            // ComprasUsrEncabezadoFormulario
            // 
            this.ComprasUsrEncabezadoFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezadoFormulario.Codigo = "6008";
            this.ComprasUsrEncabezadoFormulario.Descripcion = "Indica el motivo y confirma el rechazo de la orden de compra seleccionada.";
            this.ComprasUsrEncabezadoFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezadoFormulario.Location = new System.Drawing.Point(10, 10);
            this.ComprasUsrEncabezadoFormulario.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasUsrEncabezadoFormulario.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezadoFormulario.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezadoFormulario.Name = "ComprasUsrEncabezadoFormulario";
            this.ComprasUsrEncabezadoFormulario.Size = new System.Drawing.Size(1280, 130);
            this.ComprasUsrEncabezadoFormulario.TabIndex = 0;
            this.ComprasUsrEncabezadoFormulario.TabStop = false;
            this.ComprasUsrEncabezadoFormulario.Titulo = "Rechazar orden de compra";
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Controls.Add(this.ComprasTlpContenidoVisible);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 150);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1300, 550);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasPnlDesplazamiento
            // 
            this.ComprasPnlDesplazamiento.AutoScroll = true;
            this.ComprasPnlDesplazamiento.Controls.Add(this.ComprasTlpFormulario);
            this.ComprasPnlDesplazamiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDesplazamiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDesplazamiento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlDesplazamiento.Name = "ComprasPnlDesplazamiento";
            this.ComprasPnlDesplazamiento.Size = new System.Drawing.Size(1300, 700);
            this.ComprasPnlDesplazamiento.TabIndex = 0;
            // 
            // ComprasLblEtiquetaComentario
            // 
            this.ComprasLblEtiquetaComentario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaComentario.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaComentario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaComentario.Location = new System.Drawing.Point(11, 11);
            this.ComprasLblEtiquetaComentario.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasLblEtiquetaComentario.MaximumSize = new System.Drawing.Size(875, 17);
            this.ComprasLblEtiquetaComentario.Name = "ComprasLblEtiquetaComentario";
            this.ComprasLblEtiquetaComentario.Size = new System.Drawing.Size(87, 17);
            this.ComprasLblEtiquetaComentario.TabIndex = 0;
            this.ComprasLblEtiquetaComentario.TabStop = false;
            this.ComprasLblEtiquetaComentario.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaComentario.Text = "Comentario";
            this.ComprasLblEtiquetaComentario.Texto = "Comentario";
            // 
            // FrmRechazarOrden
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmRechazarOrden";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "6008 – RechazarOrden";
            this.ComprasTlpContenidoVisible.ResumeLayout(false);
            this.ComprasTlpAcciones.ResumeLayout(false);
            this.ComprasTlpFormulario.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ComprasTlpContenidoVisible;
        private System.Windows.Forms.TableLayoutPanel ComprasTlpAcciones;
        private System.Windows.Forms.TableLayoutPanel ComprasTlpFormulario;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezadoFormulario;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaComentario;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtComentario;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnSiRechazar;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnNoRechazar;
    }
}
