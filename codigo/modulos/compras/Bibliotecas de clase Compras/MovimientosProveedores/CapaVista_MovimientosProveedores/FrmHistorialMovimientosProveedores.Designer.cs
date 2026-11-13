namespace CapaVista_MovimientosProveedores
{
    partial class FrmHistorialMovimientosProveedores
    {
        private System.Windows.Forms.TableLayoutPanel ComprasTlpFormulario;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezadoFormulario;
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHistorialMovimientosProveedores));
            this.ComprasTlpFormulario = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasUsrEncabezadoFormulario = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasColFecha = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColTipoMovimiento = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColDocumento = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColDescripcion = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColDebe = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColHaber = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColSaldo = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColEstado = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasTlpFormulario.SuspendLayout();
            this.ComprasPnlDesplazamiento.SuspendLayout();
            this.SuspendLayout();
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
            this.ComprasTlpFormulario.Size = new System.Drawing.Size(1279, 873);
            this.ComprasTlpFormulario.TabIndex = 0;
            // 
            // ComprasUsrEncabezadoFormulario
            // 
            this.ComprasUsrEncabezadoFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezadoFormulario.Codigo = "6001";
            this.ComprasUsrEncabezadoFormulario.MostrarReportes = true;
            this.ComprasUsrEncabezadoFormulario.PaginaInicialAyuda = "MovimientosProveedores.html";
            this.ComprasUsrEncabezadoFormulario.RutaReporte = "RpReporteBalanceAntiguedadSaldos.rdlc";
            this.ComprasUsrEncabezadoFormulario.Descripcion = "Consulta la antigüedad de saldos por proveedor a una fecha de corte.";
            this.ComprasUsrEncabezadoFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezadoFormulario.Location = new System.Drawing.Point(10, 10);
            this.ComprasUsrEncabezadoFormulario.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasUsrEncabezadoFormulario.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezadoFormulario.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezadoFormulario.Name = "ComprasUsrEncabezadoFormulario";
            this.ComprasUsrEncabezadoFormulario.Size = new System.Drawing.Size(1259, 130);
            this.ComprasUsrEncabezadoFormulario.TabIndex = 0;
            this.ComprasUsrEncabezadoFormulario.TabStop = false;
            this.ComprasUsrEncabezadoFormulario.Titulo = "Historial de Movimientos de Proveedores";
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 150);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1279, 723);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasColFecha
            // 
            this.ComprasColFecha.MinimumWidth = 6;
            this.ComprasColFecha.Name = "ComprasColFecha";
            this.ComprasColFecha.ReadOnly = true;
            this.ComprasColFecha.Width = 125;
            // 
            // ComprasColTipoMovimiento
            // 
            this.ComprasColTipoMovimiento.MinimumWidth = 6;
            this.ComprasColTipoMovimiento.Name = "ComprasColTipoMovimiento";
            this.ComprasColTipoMovimiento.ReadOnly = true;
            this.ComprasColTipoMovimiento.Width = 125;
            // 
            // ComprasColDocumento
            // 
            this.ComprasColDocumento.MinimumWidth = 6;
            this.ComprasColDocumento.Name = "ComprasColDocumento";
            this.ComprasColDocumento.ReadOnly = true;
            this.ComprasColDocumento.Width = 125;
            // 
            // ComprasColDescripcion
            // 
            this.ComprasColDescripcion.MinimumWidth = 6;
            this.ComprasColDescripcion.Name = "ComprasColDescripcion";
            this.ComprasColDescripcion.ReadOnly = true;
            this.ComprasColDescripcion.Width = 125;
            // 
            // ComprasColDebe
            // 
            this.ComprasColDebe.MinimumWidth = 6;
            this.ComprasColDebe.Name = "ComprasColDebe";
            this.ComprasColDebe.ReadOnly = true;
            this.ComprasColDebe.Width = 125;
            // 
            // ComprasColHaber
            // 
            this.ComprasColHaber.MinimumWidth = 6;
            this.ComprasColHaber.Name = "ComprasColHaber";
            this.ComprasColHaber.ReadOnly = true;
            this.ComprasColHaber.Width = 125;
            // 
            // ComprasColSaldo
            // 
            this.ComprasColSaldo.MinimumWidth = 6;
            this.ComprasColSaldo.Name = "ComprasColSaldo";
            this.ComprasColSaldo.ReadOnly = true;
            this.ComprasColSaldo.Width = 125;
            // 
            // ComprasColEstado
            // 
            this.ComprasColEstado.MinimumWidth = 6;
            this.ComprasColEstado.Name = "ComprasColEstado";
            this.ComprasColEstado.ReadOnly = true;
            this.ComprasColEstado.Width = 125;
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
            // FrmHistorialMovimientosProveedores
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "FrmHistorialMovimientosProveedores";
            this.Text = "6001 – MovimientosProveedores";
            this.ComprasTlpFormulario.ResumeLayout(false);
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridViewColumn ComprasColFecha;
        private System.Windows.Forms.DataGridViewColumn ComprasColTipoMovimiento;
        private System.Windows.Forms.DataGridViewColumn ComprasColDocumento;
        private System.Windows.Forms.DataGridViewColumn ComprasColDescripcion;
        private System.Windows.Forms.DataGridViewColumn ComprasColDebe;
        private System.Windows.Forms.DataGridViewColumn ComprasColHaber;
        private System.Windows.Forms.DataGridViewColumn ComprasColSaldo;
        private System.Windows.Forms.DataGridViewColumn ComprasColEstado;
    }
}
