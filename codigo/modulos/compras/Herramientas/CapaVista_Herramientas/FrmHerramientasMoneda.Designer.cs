namespace CapaVista_Herramientas
{
    partial class FrmHerramientasMoneda
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.ComprasTlpFormulario = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasUsrEncabezadoFormulario = new CapaVista_Componentes.Componentes.UsrEncabezadoCompras();
            this.ComprasTlpPestanas = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnTipoCambio = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnConversor = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnVisualizacion = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnHistorial = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasUsrTipoCambio = new CapaVista_Herramientas.Controles.UsrTipoCambioVigente();
            this.ComprasUsrConversor = new CapaVista_Herramientas.Controles.UsrConversorMoneda();
            this.ComprasUsrVisualizacion = new CapaVista_Herramientas.Controles.UsrMonedaVisualizacion();
            this.ComprasUsrHistorial = new CapaVista_Herramientas.Controles.UsrHistorialTipoCambio();
            this.ComprasTlpPie = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnSalir = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpFormulario.SuspendLayout();
            this.ComprasTlpPestanas.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasTlpPie.SuspendLayout();
            this.SuspendLayout();
            //
            // ComprasTlpFormulario
            //
            this.ComprasTlpFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFormulario.ColumnCount = 1;
            this.ComprasTlpFormulario.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFormulario.Controls.Add(this.ComprasUsrEncabezadoFormulario, 0, 0);
            this.ComprasTlpFormulario.Controls.Add(this.ComprasTlpPestanas, 0, 1);
            this.ComprasTlpFormulario.Controls.Add(this.ComprasPnlContenido, 0, 2);
            this.ComprasTlpFormulario.Controls.Add(this.ComprasTlpPie, 0, 3);
            this.ComprasTlpFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFormulario.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpFormulario.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFormulario.Name = "ComprasTlpFormulario";
            this.ComprasTlpFormulario.RowCount = 4;
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpFormulario.Size = new System.Drawing.Size(1300, 700);
            this.ComprasTlpFormulario.TabIndex = 0;
            //
            // ComprasUsrEncabezadoFormulario
            //
            this.ComprasUsrEncabezadoFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezadoFormulario.Codigo = "6010";
            this.ComprasUsrEncabezadoFormulario.Descripcion = "Consulta tipos de cambio, convierte montos y elige la moneda en la que se muestran los montos de Compras.";
            this.ComprasUsrEncabezadoFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezadoFormulario.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasUsrEncabezadoFormulario.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezadoFormulario.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezadoFormulario.Name = "ComprasUsrEncabezadoFormulario";
            this.ComprasUsrEncabezadoFormulario.Size = new System.Drawing.Size(1280, 130);
            this.ComprasUsrEncabezadoFormulario.TabIndex = 0;
            this.ComprasUsrEncabezadoFormulario.TabStop = false;
            this.ComprasUsrEncabezadoFormulario.Titulo = "Herramientas de moneda";
            //
            // ComprasTlpPestanas
            //
            this.ComprasTlpPestanas.ColumnCount = 5;
            this.ComprasTlpPestanas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.ComprasTlpPestanas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.ComprasTlpPestanas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.ComprasTlpPestanas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.ComprasTlpPestanas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPestanas.Controls.Add(this.ComprasBtnTipoCambio, 0, 0);
            this.ComprasTlpPestanas.Controls.Add(this.ComprasBtnConversor, 1, 0);
            this.ComprasTlpPestanas.Controls.Add(this.ComprasBtnVisualizacion, 2, 0);
            this.ComprasTlpPestanas.Controls.Add(this.ComprasBtnHistorial, 3, 0);
            this.ComprasTlpPestanas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPestanas.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPestanas.Name = "ComprasTlpPestanas";
            this.ComprasTlpPestanas.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.ComprasTlpPestanas.RowCount = 1;
            this.ComprasTlpPestanas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPestanas.TabIndex = 1;
            //
            // ComprasBtnTipoCambio
            //
            this.ComprasBtnTipoCambio.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnTipoCambio.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnTipoCambio.Name = "ComprasBtnTipoCambio";
            this.ComprasBtnTipoCambio.Size = new System.Drawing.Size(220, 44);
            this.ComprasBtnTipoCambio.TabIndex = 0;
            this.ComprasBtnTipoCambio.Text = "Tipo de cambio vigente";
            //
            // ComprasBtnConversor
            //
            this.ComprasBtnConversor.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnConversor.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnConversor.Name = "ComprasBtnConversor";
            this.ComprasBtnConversor.Size = new System.Drawing.Size(220, 44);
            this.ComprasBtnConversor.TabIndex = 1;
            this.ComprasBtnConversor.Text = "Conversor de montos";
            //
            // ComprasBtnVisualizacion
            //
            this.ComprasBtnVisualizacion.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnVisualizacion.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnVisualizacion.Name = "ComprasBtnVisualizacion";
            this.ComprasBtnVisualizacion.Size = new System.Drawing.Size(220, 44);
            this.ComprasBtnVisualizacion.TabIndex = 2;
            this.ComprasBtnVisualizacion.Text = "Moneda de visualización";
            //
            // ComprasBtnHistorial
            //
            this.ComprasBtnHistorial.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnHistorial.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnHistorial.Name = "ComprasBtnHistorial";
            this.ComprasBtnHistorial.Size = new System.Drawing.Size(220, 44);
            this.ComprasBtnHistorial.TabIndex = 3;
            this.ComprasBtnHistorial.Text = "Historial del tipo de cambio";
            //
            // ComprasPnlContenido
            //
            this.ComprasPnlContenido.Controls.Add(this.ComprasUsrTipoCambio);
            this.ComprasPnlContenido.Controls.Add(this.ComprasUsrConversor);
            this.ComprasPnlContenido.Controls.Add(this.ComprasUsrVisualizacion);
            this.ComprasPnlContenido.Controls.Add(this.ComprasUsrHistorial);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.TabIndex = 2;
            //
            // ComprasUsrTipoCambio
            //
            this.ComprasUsrTipoCambio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrTipoCambio.Name = "ComprasUsrTipoCambio";
            this.ComprasUsrTipoCambio.TabIndex = 0;
            //
            // ComprasUsrConversor
            //
            this.ComprasUsrConversor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrConversor.Name = "ComprasUsrConversor";
            this.ComprasUsrConversor.TabIndex = 1;
            this.ComprasUsrConversor.Visible = false;
            //
            // ComprasUsrVisualizacion
            //
            this.ComprasUsrVisualizacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrVisualizacion.Name = "ComprasUsrVisualizacion";
            this.ComprasUsrVisualizacion.TabIndex = 2;
            this.ComprasUsrVisualizacion.Visible = false;
            //
            // ComprasUsrHistorial
            //
            this.ComprasUsrHistorial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrHistorial.Name = "ComprasUsrHistorial";
            this.ComprasUsrHistorial.TabIndex = 3;
            this.ComprasUsrHistorial.Visible = false;
            //
            // ComprasTlpPie
            //
            this.ComprasTlpPie.ColumnCount = 2;
            this.ComprasTlpPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.ComprasTlpPie.Controls.Add(this.ComprasBtnSalir, 1, 0);
            this.ComprasTlpPie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPie.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPie.Name = "ComprasTlpPie";
            this.ComprasTlpPie.Padding = new System.Windows.Forms.Padding(16, 6, 16, 10);
            this.ComprasTlpPie.RowCount = 1;
            this.ComprasTlpPie.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPie.TabIndex = 3;
            //
            // ComprasBtnSalir
            //
            this.ComprasBtnSalir.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnSalir.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnSalir.Name = "ComprasBtnSalir";
            this.ComprasBtnSalir.Size = new System.Drawing.Size(150, 40);
            this.ComprasBtnSalir.TabIndex = 0;
            this.ComprasBtnSalir.Text = "Salir";
            //
            // FrmHerramientasMoneda
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.ComprasTlpFormulario);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmHerramientasMoneda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "6010 – Herramientas de moneda";
            this.ComprasTlpPie.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasTlpPestanas.ResumeLayout(false);
            this.ComprasTlpFormulario.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFormulario;
        private CapaVista_Componentes.Componentes.UsrEncabezadoCompras ComprasUsrEncabezadoFormulario;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPestanas;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnTipoCambio;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnConversor;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnVisualizacion;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnHistorial;
        private System.Windows.Forms.Panel ComprasPnlContenido;
        private CapaVista_Herramientas.Controles.UsrTipoCambioVigente ComprasUsrTipoCambio;
        private CapaVista_Herramientas.Controles.UsrConversorMoneda ComprasUsrConversor;
        private CapaVista_Herramientas.Controles.UsrMonedaVisualizacion ComprasUsrVisualizacion;
        private CapaVista_Herramientas.Controles.UsrHistorialTipoCambio ComprasUsrHistorial;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPie;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnSalir;
    }
}
