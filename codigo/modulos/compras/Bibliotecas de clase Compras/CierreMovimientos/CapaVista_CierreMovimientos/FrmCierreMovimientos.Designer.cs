namespace CapaVista_CierreMovimientos
{
    partial class FrmCierreMovimientos
    {
        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCierreMovimientos));
            this.ComprasTlpFormulario = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasUsrEncabezadoFormulario = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasTlpPrincipal = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasGbxPeriodo = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasCboPeriodo = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblEtiquetaPeriodo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpContenido = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasGbxValidaciones = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasDgvValidaciones = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasGbxSaldos = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasDgvSaldos = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasTlpTotales = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTxtTotalCargos = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaTotalAbonos = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtTotalAbonos = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaSaldoFinal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtSaldoFinal = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaTotalCargos = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpBotones = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnEjecutarCierre = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnValidar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnImprimir = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnAyuda = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnSalir = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasLblTitulo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpFormulario.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasGbxPeriodo.SuspendLayout();
            this.ComprasTlpContenido.SuspendLayout();
            this.ComprasGbxValidaciones.SuspendLayout();
            this.ComprasGbxSaldos.SuspendLayout();
            this.ComprasTlpTotales.SuspendLayout();
            this.ComprasTlpBotones.SuspendLayout();
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
            this.ComprasUsrEncabezadoFormulario.Codigo = "6006";
            this.ComprasUsrEncabezadoFormulario.Descripcion = "Revisa los movimientos y totales del período antes de realizar el cierre de compr" +
    "as.";
            this.ComprasUsrEncabezadoFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezadoFormulario.Location = new System.Drawing.Point(10, 10);
            this.ComprasUsrEncabezadoFormulario.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasUsrEncabezadoFormulario.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezadoFormulario.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezadoFormulario.Name = "ComprasUsrEncabezadoFormulario";
            this.ComprasUsrEncabezadoFormulario.Size = new System.Drawing.Size(1259, 130);
            this.ComprasUsrEncabezadoFormulario.TabIndex = 0;
            this.ComprasUsrEncabezadoFormulario.TabStop = false;
            this.ComprasUsrEncabezadoFormulario.Titulo = "Cierre de movimientos";
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Controls.Add(this.ComprasTlpPrincipal);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 150);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1279, 723);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasTlpPrincipal
            // 
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPrincipal.ColumnCount = 1;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxPeriodo, 0, 1);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpContenido, 0, 2);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpTotales, 0, 3);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpBotones, 0, 4);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.Padding = new System.Windows.Forms.Padding(16);
            this.ComprasTlpPrincipal.RowCount = 5;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1279, 723);
            this.ComprasTlpPrincipal.TabIndex = 0;
            // 
            // ComprasGbxPeriodo
            // 
            this.ComprasGbxPeriodo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxPeriodo.Controls.Add(this.ComprasCboPeriodo);
            this.ComprasGbxPeriodo.Controls.Add(this.ComprasLblEtiquetaPeriodo);
            this.ComprasGbxPeriodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxPeriodo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxPeriodo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxPeriodo.Location = new System.Drawing.Point(19, 19);
            this.ComprasGbxPeriodo.Name = "ComprasGbxPeriodo";
            this.ComprasGbxPeriodo.Size = new System.Drawing.Size(1241, 74);
            this.ComprasGbxPeriodo.TabIndex = 1;
            this.ComprasGbxPeriodo.TabStop = false;
            this.ComprasGbxPeriodo.Text = "Período a cerrar";
            // 
            // ComprasCboPeriodo
            // 
            this.ComprasCboPeriodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboPeriodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboPeriodo.Location = new System.Drawing.Point(214, 24);
            this.ComprasCboPeriodo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboPeriodo.MaxLength = 0;
            this.ComprasCboPeriodo.MostrarEtiquetaError = false;
            this.ComprasCboPeriodo.Name = "ComprasCboPeriodo";
            this.ComprasCboPeriodo.Size = new System.Drawing.Size(174, 36);
            this.ComprasCboPeriodo.TabIndex = 1;
            // 
            // ComprasLblEtiquetaPeriodo
            // 
            this.ComprasLblEtiquetaPeriodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaPeriodo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaPeriodo.Location = new System.Drawing.Point(3, 27);
            this.ComprasLblEtiquetaPeriodo.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaPeriodo.Name = "ComprasLblEtiquetaPeriodo";
            this.ComprasLblEtiquetaPeriodo.Size = new System.Drawing.Size(208, 22);
            this.ComprasLblEtiquetaPeriodo.TabIndex = 0;
            this.ComprasLblEtiquetaPeriodo.TabStop = false;
            this.ComprasLblEtiquetaPeriodo.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaPeriodo.Text = "Período";
            this.ComprasLblEtiquetaPeriodo.Texto = "Período";
            // 
            // ComprasTlpContenido
            // 
            this.ComprasTlpContenido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpContenido.ColumnCount = 2;
            this.ComprasTlpContenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.ComprasTlpContenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.ComprasTlpContenido.Controls.Add(this.ComprasGbxValidaciones, 0, 0);
            this.ComprasTlpContenido.Controls.Add(this.ComprasGbxSaldos, 1, 0);
            this.ComprasTlpContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpContenido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpContenido.Location = new System.Drawing.Point(19, 99);
            this.ComprasTlpContenido.Name = "ComprasTlpContenido";
            this.ComprasTlpContenido.RowCount = 1;
            this.ComprasTlpContenido.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpContenido.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 375F));
            this.ComprasTlpContenido.Size = new System.Drawing.Size(1241, 425);
            this.ComprasTlpContenido.TabIndex = 2;
            // 
            // ComprasGbxValidaciones
            // 
            this.ComprasGbxValidaciones.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxValidaciones.Controls.Add(this.ComprasDgvValidaciones);
            this.ComprasGbxValidaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxValidaciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxValidaciones.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxValidaciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxValidaciones.Location = new System.Drawing.Point(3, 3);
            this.ComprasGbxValidaciones.Name = "ComprasGbxValidaciones";
            this.ComprasGbxValidaciones.Size = new System.Drawing.Size(490, 419);
            this.ComprasGbxValidaciones.TabIndex = 0;
            this.ComprasGbxValidaciones.TabStop = false;
            this.ComprasGbxValidaciones.Text = "Validaciones previas";
            // 
            // ComprasDgvValidaciones
            // 
            this.ComprasDgvValidaciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvValidaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvValidaciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvValidaciones.Location = new System.Drawing.Point(3, 24);
            this.ComprasDgvValidaciones.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvValidaciones.Name = "ComprasDgvValidaciones";
            this.ComprasDgvValidaciones.Size = new System.Drawing.Size(484, 392);
            this.ComprasDgvValidaciones.TabIndex = 0;
            // 
            // ComprasGbxSaldos
            // 
            this.ComprasGbxSaldos.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxSaldos.Controls.Add(this.ComprasDgvSaldos);
            this.ComprasGbxSaldos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxSaldos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxSaldos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxSaldos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxSaldos.Location = new System.Drawing.Point(499, 3);
            this.ComprasGbxSaldos.Name = "ComprasGbxSaldos";
            this.ComprasGbxSaldos.Size = new System.Drawing.Size(739, 419);
            this.ComprasGbxSaldos.TabIndex = 1;
            this.ComprasGbxSaldos.TabStop = false;
            this.ComprasGbxSaldos.Text = "Resumen por proveedor";
            // 
            // ComprasDgvSaldos
            // 
            this.ComprasDgvSaldos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvSaldos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvSaldos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvSaldos.Location = new System.Drawing.Point(3, 24);
            this.ComprasDgvSaldos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvSaldos.Name = "ComprasDgvSaldos";
            this.ComprasDgvSaldos.Size = new System.Drawing.Size(733, 392);
            this.ComprasDgvSaldos.TabIndex = 0;
            // 
            // ComprasTlpTotales
            // 
            this.ComprasTlpTotales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotales.ColumnCount = 3;
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtTotalCargos, 0, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaTotalAbonos, 1, 0);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtTotalAbonos, 1, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaSaldoFinal, 2, 0);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtSaldoFinal, 2, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaTotalCargos, 0, 0);
            this.ComprasTlpTotales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpTotales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpTotales.Location = new System.Drawing.Point(19, 530);
            this.ComprasTlpTotales.Name = "ComprasTlpTotales";
            this.ComprasTlpTotales.RowCount = 2;
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTotales.Size = new System.Drawing.Size(1241, 74);
            this.ComprasTlpTotales.TabIndex = 3;
            // 
            // ComprasTxtTotalCargos
            // 
            this.ComprasTxtTotalCargos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTotalCargos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtTotalCargos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTotalCargos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTotalCargos.Location = new System.Drawing.Point(0, 26);
            this.ComprasTxtTotalCargos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtTotalCargos.MaxLength = 2147483647;
            this.ComprasTxtTotalCargos.MostrarEtiquetaError = false;
            this.ComprasTxtTotalCargos.Name = "ComprasTxtTotalCargos";
            this.ComprasTxtTotalCargos.ReadOnly = true;
            this.ComprasTxtTotalCargos.SelectedText = "";
            this.ComprasTxtTotalCargos.SelectionLength = 0;
            this.ComprasTxtTotalCargos.SelectionStart = 0;
            this.ComprasTxtTotalCargos.Size = new System.Drawing.Size(413, 48);
            this.ComprasTxtTotalCargos.TabIndex = 1;
            // 
            // ComprasLblEtiquetaTotalAbonos
            // 
            this.ComprasLblEtiquetaTotalAbonos.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEtiquetaTotalAbonos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaTotalAbonos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaTotalAbonos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaTotalAbonos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaTotalAbonos.Location = new System.Drawing.Point(416, 3);
            this.ComprasLblEtiquetaTotalAbonos.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaTotalAbonos.MostrarAsterisco = false;
            this.ComprasLblEtiquetaTotalAbonos.Name = "ComprasLblEtiquetaTotalAbonos";
            this.ComprasLblEtiquetaTotalAbonos.Size = new System.Drawing.Size(407, 20);
            this.ComprasLblEtiquetaTotalAbonos.TabIndex = 2;
            this.ComprasLblEtiquetaTotalAbonos.TabStop = false;
            this.ComprasLblEtiquetaTotalAbonos.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaTotalAbonos.Text = "Total Abonos (Q):";
            this.ComprasLblEtiquetaTotalAbonos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEtiquetaTotalAbonos.Texto = "Total Abonos (Q):";
            // 
            // ComprasTxtTotalAbonos
            // 
            this.ComprasTxtTotalAbonos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTotalAbonos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtTotalAbonos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTotalAbonos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTotalAbonos.Location = new System.Drawing.Point(413, 26);
            this.ComprasTxtTotalAbonos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtTotalAbonos.MaxLength = 2147483647;
            this.ComprasTxtTotalAbonos.MostrarEtiquetaError = false;
            this.ComprasTxtTotalAbonos.Name = "ComprasTxtTotalAbonos";
            this.ComprasTxtTotalAbonos.ReadOnly = true;
            this.ComprasTxtTotalAbonos.SelectedText = "";
            this.ComprasTxtTotalAbonos.SelectionLength = 0;
            this.ComprasTxtTotalAbonos.SelectionStart = 0;
            this.ComprasTxtTotalAbonos.Size = new System.Drawing.Size(413, 48);
            this.ComprasTxtTotalAbonos.TabIndex = 3;
            // 
            // ComprasLblEtiquetaSaldoFinal
            // 
            this.ComprasLblEtiquetaSaldoFinal.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEtiquetaSaldoFinal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaSaldoFinal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaSaldoFinal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaSaldoFinal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaSaldoFinal.Location = new System.Drawing.Point(829, 3);
            this.ComprasLblEtiquetaSaldoFinal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaSaldoFinal.MostrarAsterisco = false;
            this.ComprasLblEtiquetaSaldoFinal.Name = "ComprasLblEtiquetaSaldoFinal";
            this.ComprasLblEtiquetaSaldoFinal.Size = new System.Drawing.Size(409, 20);
            this.ComprasLblEtiquetaSaldoFinal.TabIndex = 4;
            this.ComprasLblEtiquetaSaldoFinal.TabStop = false;
            this.ComprasLblEtiquetaSaldoFinal.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaSaldoFinal.Text = "Saldo Final CxP (Q):";
            this.ComprasLblEtiquetaSaldoFinal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEtiquetaSaldoFinal.Texto = "Saldo Final CxP (Q):";
            // 
            // ComprasTxtSaldoFinal
            // 
            this.ComprasTxtSaldoFinal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtSaldoFinal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtSaldoFinal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtSaldoFinal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtSaldoFinal.Location = new System.Drawing.Point(826, 26);
            this.ComprasTxtSaldoFinal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtSaldoFinal.MaxLength = 2147483647;
            this.ComprasTxtSaldoFinal.MostrarEtiquetaError = false;
            this.ComprasTxtSaldoFinal.Name = "ComprasTxtSaldoFinal";
            this.ComprasTxtSaldoFinal.ReadOnly = true;
            this.ComprasTxtSaldoFinal.SelectedText = "";
            this.ComprasTxtSaldoFinal.SelectionLength = 0;
            this.ComprasTxtSaldoFinal.SelectionStart = 0;
            this.ComprasTxtSaldoFinal.Size = new System.Drawing.Size(415, 48);
            this.ComprasTxtSaldoFinal.TabIndex = 5;
            // 
            // ComprasLblEtiquetaTotalCargos
            // 
            this.ComprasLblEtiquetaTotalCargos.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEtiquetaTotalCargos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaTotalCargos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaTotalCargos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaTotalCargos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaTotalCargos.Location = new System.Drawing.Point(3, 3);
            this.ComprasLblEtiquetaTotalCargos.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaTotalCargos.MostrarAsterisco = false;
            this.ComprasLblEtiquetaTotalCargos.Name = "ComprasLblEtiquetaTotalCargos";
            this.ComprasLblEtiquetaTotalCargos.Size = new System.Drawing.Size(407, 20);
            this.ComprasLblEtiquetaTotalCargos.TabIndex = 0;
            this.ComprasLblEtiquetaTotalCargos.TabStop = false;
            this.ComprasLblEtiquetaTotalCargos.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaTotalCargos.Text = "Total Cargos (Q):";
            this.ComprasLblEtiquetaTotalCargos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEtiquetaTotalCargos.Texto = "Total Cargos (Q):";
            // 
            // ComprasTlpBotones
            // 
            this.ComprasTlpBotones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpBotones.ColumnCount = 5;
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnEjecutarCierre, 1, 0);
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnValidar, 0, 0);
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnImprimir, 2, 0);
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnAyuda, 3, 0);
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnSalir, 4, 0);
            this.ComprasTlpBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpBotones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpBotones.Location = new System.Drawing.Point(19, 610);
            this.ComprasTlpBotones.Name = "ComprasTlpBotones";
            this.ComprasTlpBotones.RowCount = 1;
            this.ComprasTlpBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpBotones.Size = new System.Drawing.Size(1241, 94);
            this.ComprasTlpBotones.TabIndex = 4;
            // 
            // ComprasBtnEjecutarCierre
            // 
            this.ComprasBtnEjecutarCierre.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnEjecutarCierre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnEjecutarCierre.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnEjecutarCierre.BackgroundImage")));
            this.ComprasBtnEjecutarCierre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnEjecutarCierre.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnEjecutarCierre.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEjecutarCierre.FlatAppearance.BorderSize = 0;
            this.ComprasBtnEjecutarCierre.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnEjecutarCierre.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnEjecutarCierre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnEjecutarCierre.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnEjecutarCierre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEjecutarCierre.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnEjecutarCierre.ImagenDeshabilitado")));
            this.ComprasBtnEjecutarCierre.Location = new System.Drawing.Point(332, 7);
            this.ComprasBtnEjecutarCierre.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnEjecutarCierre.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEjecutarCierre.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEjecutarCierre.Name = "ComprasBtnEjecutarCierre";
            this.ComprasBtnEjecutarCierre.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnEjecutarCierre.TabIndex = 1;
            this.ComprasBtnEjecutarCierre.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnValidar
            // 
            this.ComprasBtnValidar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnValidar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnValidar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnValidar.BackgroundImage")));
            this.ComprasBtnValidar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnValidar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnValidar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnValidar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnValidar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnValidar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnValidar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnValidar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnValidar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnValidar.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnValidar.ImagenDeshabilitado")));
            this.ComprasBtnValidar.Location = new System.Drawing.Point(84, 7);
            this.ComprasBtnValidar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnValidar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnValidar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnValidar.Name = "ComprasBtnValidar";
            this.ComprasBtnValidar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnValidar.TabIndex = 0;
            this.ComprasBtnValidar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnImprimir
            // 
            this.ComprasBtnImprimir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnImprimir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnImprimir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnImprimir.BackgroundImage")));
            this.ComprasBtnImprimir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnImprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnImprimir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnImprimir.FlatAppearance.BorderSize = 0;
            this.ComprasBtnImprimir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnImprimir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnImprimir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnImprimir.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnImprimir.ImagenDeshabilitado")));
            this.ComprasBtnImprimir.Location = new System.Drawing.Point(580, 7);
            this.ComprasBtnImprimir.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnImprimir.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnImprimir.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnImprimir.Name = "ComprasBtnImprimir";
            this.ComprasBtnImprimir.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnImprimir.TabIndex = 2;
            this.ComprasBtnImprimir.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnAyuda
            // 
            this.ComprasBtnAyuda.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnAyuda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnAyuda.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnAyuda.BackgroundImage")));
            this.ComprasBtnAyuda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnAyuda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnAyuda.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnAyuda.FlatAppearance.BorderSize = 0;
            this.ComprasBtnAyuda.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnAyuda.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnAyuda.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnAyuda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnAyuda.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnAyuda.ImagenDeshabilitado")));
            this.ComprasBtnAyuda.Location = new System.Drawing.Point(828, 7);
            this.ComprasBtnAyuda.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnAyuda.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnAyuda.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnAyuda.Name = "ComprasBtnAyuda";
            this.ComprasBtnAyuda.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnAyuda.TabIndex = 3;
            this.ComprasBtnAyuda.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnSalir
            // 
            this.ComprasBtnSalir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnSalir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnSalir.BackgroundImage")));
            this.ComprasBtnSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnSalir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnSalir.FlatAppearance.BorderSize = 0;
            this.ComprasBtnSalir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnSalir.Location = new System.Drawing.Point(1076, 7);
            this.ComprasBtnSalir.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnSalir.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnSalir.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnSalir.Name = "ComprasBtnSalir";
            this.ComprasBtnSalir.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnSalir.TabIndex = 4;
            this.ComprasBtnSalir.UseVisualStyleBackColor = false;
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
            // ComprasLblTitulo
            // 
            this.ComprasLblTitulo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTitulo.Font = new System.Drawing.Font("Tahoma", 16.2F);
            this.ComprasLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTitulo.Location = new System.Drawing.Point(21, 22);
            this.ComprasLblTitulo.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ComprasLblTitulo.MaximumSize = new System.Drawing.Size(1875, 40);
            this.ComprasLblTitulo.MostrarAsterisco = false;
            this.ComprasLblTitulo.Name = "ComprasLblTitulo";
            this.ComprasLblTitulo.Size = new System.Drawing.Size(390, 38);
            this.ComprasLblTitulo.TabIndex = 0;
            this.ComprasLblTitulo.TabStop = false;
            this.ComprasLblTitulo.TamanoTipografia = 16.2F;
            this.ComprasLblTitulo.Text = "Cierre de Movimientos";
            this.ComprasLblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTitulo.Texto = "Cierre de Movimientos";
            // 
            // FrmCierreMovimientos
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
            this.Name = "FrmCierreMovimientos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "6006 – CierreMovimientos";
            this.ComprasTlpFormulario.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ComprasGbxPeriodo.ResumeLayout(false);
            this.ComprasTlpContenido.ResumeLayout(false);
            this.ComprasGbxValidaciones.ResumeLayout(false);
            this.ComprasGbxSaldos.ResumeLayout(false);
            this.ComprasTlpTotales.ResumeLayout(false);
            this.ComprasTlpBotones.ResumeLayout(false);
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ComprasTlpFormulario;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezadoFormulario;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTitulo;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxPeriodo;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboPeriodo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaPeriodo;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpContenido;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxValidaciones;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvValidaciones;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxSaldos;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvSaldos;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpTotales;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaTotalCargos;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTotalCargos;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaTotalAbonos;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTotalAbonos;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaSaldoFinal;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtSaldoFinal;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpBotones;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnValidar;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnEjecutarCierre;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnImprimir;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnAyuda;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnSalir;
    }
}
