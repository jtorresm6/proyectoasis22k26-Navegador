namespace CapaVista_Compras
{
    partial class FrmBitacoraCompras
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBitacoraCompras));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ComprasTlpPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasEncabezado = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasTlpAcciones = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasTxtBuscar = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasBtnBuscar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnSalir = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasDgvBitacora = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasColIdBitacora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColNombreUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColAccionBitacora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColTablaBitacora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColIdRegistroBitacora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColDetallesBitacora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColIpBitacora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColFechaHoraBitacora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasTlpAcciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpPrincipal
            // 
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPrincipal.ColumnCount = 1;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasEncabezado, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpAcciones, 0, 1);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasDgvBitacora, 0, 2);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasLblEstado, 0, 3);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.RowCount = 4;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1681, 832);
            this.ComprasTlpPrincipal.TabIndex = 0;
            // 
            // ComprasEncabezado
            // 
            this.ComprasEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasEncabezado.Codigo = "";
            this.ComprasEncabezado.Descripcion = "Consulta el historial de acciones del sistema y busca por usuario, acción, tabla " +
    "o fecha.";
            this.ComprasEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasEncabezado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasEncabezado.Location = new System.Drawing.Point(0, 0);
            this.ComprasEncabezado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasEncabezado.MaximumSize = new System.Drawing.Size(2000, 160);
            this.ComprasEncabezado.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasEncabezado.Name = "ComprasEncabezado";
            this.ComprasEncabezado.RutaReporte = "RpReporteBitacora.rdlc";
            this.ComprasEncabezado.Size = new System.Drawing.Size(1681, 130);
            this.ComprasEncabezado.TabIndex = 0;
            this.ComprasEncabezado.TabStop = false;
            this.ComprasEncabezado.Titulo = "BITÁCORA";
            this.ComprasEncabezado.PrepararReporte += new System.EventHandler<CapaVista_Componentes.Componentes.ComprasReporteEventArgs>(this.ComprasMetPrepararReporte);
            // 
            // ComprasTlpAcciones
            // 
            this.ComprasTlpAcciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(247)))), ((int)(((byte)(242)))));
            this.ComprasTlpAcciones.ColumnCount = 3;
            this.ComprasTlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpAcciones.Controls.Add(this.ComprasTxtBuscar, 0, 0);
            this.ComprasTlpAcciones.Controls.Add(this.ComprasBtnBuscar, 1, 0);
            this.ComprasTlpAcciones.Controls.Add(this.ComprasBtnSalir, 2, 0);
            this.ComprasTlpAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpAcciones.Location = new System.Drawing.Point(0, 130);
            this.ComprasTlpAcciones.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpAcciones.Name = "ComprasTlpAcciones";
            this.ComprasTlpAcciones.Padding = new System.Windows.Forms.Padding(24, 17, 24, 12);
            this.ComprasTlpAcciones.RowCount = 1;
            this.ComprasTlpAcciones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAcciones.Size = new System.Drawing.Size(1681, 109);
            this.ComprasTlpAcciones.TabIndex = 1;
            // 
            // ComprasTxtBuscar
            // 
            this.ComprasTxtBuscar.AccessibleName = "Buscar en la bitácora";
            this.ComprasTxtBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtBuscar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtBuscar.Location = new System.Drawing.Point(24, 17);
            this.ComprasTxtBuscar.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.ComprasTxtBuscar.MaxLength = 2147483647;
            this.ComprasTxtBuscar.Name = "ComprasTxtBuscar";
            this.ComprasTxtBuscar.SelectedText = "";
            this.ComprasTxtBuscar.SelectionLength = 0;
            this.ComprasTxtBuscar.SelectionStart = 0;
            this.ComprasTxtBuscar.Size = new System.Drawing.Size(1437, 80);
            this.ComprasTxtBuscar.TabIndex = 0;
            this.ComprasTxtBuscar.TextoKeyDown += new System.Windows.Forms.KeyEventHandler(this.ComprasMetBuscarTecla);
            this.ComprasBtnBuscar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnBuscar
            // 
            this.ComprasBtnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnBuscar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnBuscar.BackgroundImage")));
            this.ComprasBtnBuscar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasBtnBuscar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnBuscar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnBuscar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnBuscar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnBuscar.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasBtnBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnBuscar.Location = new System.Drawing.Point(1482, 17);
            this.ComprasBtnBuscar.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.ComprasBtnBuscar.Name = "ComprasBtnBuscar";
            this.ComprasBtnBuscar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnBuscar.TabIndex = 1;
            this.ComprasBtnBuscar.UseVisualStyleBackColor = false;
            this.ComprasBtnBuscar.Click += new System.EventHandler(this.ComprasMetBuscar);
            this.ComprasBtnSalir.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnSalir
            // 
            this.ComprasBtnSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnSalir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnSalir.BackgroundImage")));
            this.ComprasBtnSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnSalir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasBtnSalir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnSalir.FlatAppearance.BorderSize = 0;
            this.ComprasBtnSalir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnSalir.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasBtnSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnSalir.Location = new System.Drawing.Point(1572, 17);
            this.ComprasBtnSalir.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.ComprasBtnSalir.Name = "ComprasBtnSalir";
            this.ComprasBtnSalir.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnSalir.TabIndex = 4;
            this.ComprasBtnSalir.UseVisualStyleBackColor = false;
            this.ComprasBtnSalir.Click += new System.EventHandler(this.ComprasMetSalir);
            // 
            // ComprasDgvBitacora
            // 
            this.ComprasDgvBitacora.AutoGenerateColumns = false;
            this.ComprasDgvBitacora.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.ComprasDgvBitacora.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvBitacora.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ComprasColIdBitacora,
            this.ComprasColNombreUsuario,
            this.ComprasColAccionBitacora,
            this.ComprasColTablaBitacora,
            this.ComprasColIdRegistroBitacora,
            this.ComprasColDetallesBitacora,
            this.ComprasColIpBitacora,
            this.ComprasColFechaHoraBitacora});
            this.ComprasDgvBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvBitacora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvBitacora.Location = new System.Drawing.Point(24, 239);
            this.ComprasDgvBitacora.Margin = new System.Windows.Forms.Padding(24, 0, 24, 0);
            this.ComprasDgvBitacora.Name = "ComprasDgvBitacora";
            this.ComprasDgvBitacora.Size = new System.Drawing.Size(1633, 536);
            this.ComprasDgvBitacora.TabIndex = 2;
            // 
            // ComprasColIdBitacora
            // 
            this.ComprasColIdBitacora.DataPropertyName = "IdBitacora";
            this.ComprasColIdBitacora.HeaderText = "ID";
            this.ComprasColIdBitacora.MinimumWidth = 6;
            this.ComprasColIdBitacora.Name = "ComprasColIdBitacora";
            this.ComprasColIdBitacora.ReadOnly = true;
            this.ComprasColIdBitacora.Width = 55;
            // 
            // ComprasColNombreUsuario
            // 
            this.ComprasColNombreUsuario.DataPropertyName = "NombreUsuario";
            this.ComprasColNombreUsuario.HeaderText = "Usuario";
            this.ComprasColNombreUsuario.MinimumWidth = 6;
            this.ComprasColNombreUsuario.Name = "ComprasColNombreUsuario";
            this.ComprasColNombreUsuario.ReadOnly = true;
            this.ComprasColNombreUsuario.Width = 120;
            // 
            // ComprasColAccionBitacora
            // 
            this.ComprasColAccionBitacora.DataPropertyName = "AccionBitacora";
            this.ComprasColAccionBitacora.HeaderText = "Acción";
            this.ComprasColAccionBitacora.MinimumWidth = 6;
            this.ComprasColAccionBitacora.Name = "ComprasColAccionBitacora";
            this.ComprasColAccionBitacora.ReadOnly = true;
            this.ComprasColAccionBitacora.Width = 110;
            // 
            // ComprasColTablaBitacora
            // 
            this.ComprasColTablaBitacora.DataPropertyName = "TablaBitacora";
            this.ComprasColTablaBitacora.HeaderText = "Tabla afectada";
            this.ComprasColTablaBitacora.MinimumWidth = 6;
            this.ComprasColTablaBitacora.Name = "ComprasColTablaBitacora";
            this.ComprasColTablaBitacora.ReadOnly = true;
            this.ComprasColTablaBitacora.Width = 140;
            // 
            // ComprasColIdRegistroBitacora
            // 
            this.ComprasColIdRegistroBitacora.DataPropertyName = "IdRegistroBitacora";
            this.ComprasColIdRegistroBitacora.HeaderText = "ID Registro";
            this.ComprasColIdRegistroBitacora.MinimumWidth = 6;
            this.ComprasColIdRegistroBitacora.Name = "ComprasColIdRegistroBitacora";
            this.ComprasColIdRegistroBitacora.ReadOnly = true;
            this.ComprasColIdRegistroBitacora.Width = 95;
            // 
            // ComprasColDetallesBitacora
            // 
            this.ComprasColDetallesBitacora.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ComprasColDetallesBitacora.DataPropertyName = "DetallesBitacora";
            this.ComprasColDetallesBitacora.HeaderText = "Detalles";
            this.ComprasColDetallesBitacora.MinimumWidth = 180;
            this.ComprasColDetallesBitacora.Name = "ComprasColDetallesBitacora";
            this.ComprasColDetallesBitacora.ReadOnly = true;
            // 
            // ComprasColIpBitacora
            // 
            this.ComprasColIpBitacora.DataPropertyName = "IpBitacora";
            this.ComprasColIpBitacora.HeaderText = "Dirección IP";
            this.ComprasColIpBitacora.MinimumWidth = 6;
            this.ComprasColIpBitacora.Name = "ComprasColIpBitacora";
            this.ComprasColIpBitacora.ReadOnly = true;
            this.ComprasColIpBitacora.Width = 120;
            // 
            // ComprasColFechaHoraBitacora
            // 
            this.ComprasColFechaHoraBitacora.DataPropertyName = "FechaHoraBitacora";
            dataGridViewCellStyle1.Format = "yyyy-MM-dd HH:mm:ss";
            this.ComprasColFechaHoraBitacora.DefaultCellStyle = dataGridViewCellStyle1;
            this.ComprasColFechaHoraBitacora.HeaderText = "Fecha y hora";
            this.ComprasColFechaHoraBitacora.MinimumWidth = 6;
            this.ComprasColFechaHoraBitacora.Name = "ComprasColFechaHoraBitacora";
            this.ComprasColFechaHoraBitacora.ReadOnly = true;
            this.ComprasColFechaHoraBitacora.Width = 160;
            // 
            // ComprasLblEstado
            // 
            this.ComprasLblEstado.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEstado.Font = new System.Drawing.Font("Tahoma", 13F);
            this.ComprasLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstado.Location = new System.Drawing.Point(24, 780);
            this.ComprasLblEstado.Margin = new System.Windows.Forms.Padding(24, 5, 24, 5);
            this.ComprasLblEstado.MaximumSize = new System.Drawing.Size(2000, 0);
            this.ComprasLblEstado.MostrarAsterisco = false;
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(1633, 47);
            this.ComprasLblEstado.TabIndex = 3;
            this.ComprasLblEstado.TabStop = false;
            this.ComprasLblEstado.Text = "Cargando bitácora...";
            this.ComprasLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.Texto = "Cargando bitácora...";
            // 
            // FrmBitacoraCompras
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1681, 832);
            this.Controls.Add(this.ComprasTlpPrincipal);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmBitacoraCompras";
            this.Text = "Bitácora - Compras";
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ComprasTlpAcciones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ComprasTlpPrincipal;
        private System.Windows.Forms.TableLayoutPanel ComprasTlpAcciones;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasEncabezado;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtBuscar;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvBitacora;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstado;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnBuscar;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnSalir;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColIdBitacora;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColNombreUsuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColAccionBitacora;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColTablaBitacora;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColIdRegistroBitacora;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColDetallesBitacora;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColIpBitacora;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColFechaHoraBitacora;
    }
}
