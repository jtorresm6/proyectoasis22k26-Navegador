
namespace CapaVista_MuestreoAuditoria_Logistica.Formularios
{
    partial class FrmAuditoriaInventario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAuditoriaInventario));
            this.LogisticaLblTitulo = new System.Windows.Forms.Label();
            this.LogisticaLblDescripcion = new System.Windows.Forms.Label();
            this.LogisticaPnlOpciones = new System.Windows.Forms.Panel();
            this.LogisticaGpbConsulta = new System.Windows.Forms.GroupBox();
            this.LogisticaBtnBuscar = new System.Windows.Forms.Button();
            this.LogisticaTxtBuscar = new System.Windows.Forms.TextBox();
            this.LogisticaCboEstado = new System.Windows.Forms.ComboBox();
            this.LogisticaCboBodega = new System.Windows.Forms.ComboBox();
            this.LogisticaLblBuscar = new System.Windows.Forms.Label();
            this.LogisticaLblEstado = new System.Windows.Forms.Label();
            this.LogisticaLblBodega = new System.Windows.Forms.Label();
            this.LogisticaGpbNuevaAuditoria = new System.Windows.Forms.GroupBox();
            this.LogisticaBtnMultiple = new System.Windows.Forms.Button();
            this.LogisticaBtnIndividual = new System.Windows.Forms.Button();
            this.LogisticaPnlHistorial = new System.Windows.Forms.Panel();
            this.LogisticaLblPagina = new System.Windows.Forms.Label();
            this.LogisticaBtnActualizar = new System.Windows.Forms.Button();
            this.LogisticaBtnEditar = new System.Windows.Forms.Button();
            this.LogisticaBtnConsultar = new System.Windows.Forms.Button();
            this.LogisticaBtnSiguiente = new System.Windows.Forms.Button();
            this.LogisticaBtnAnterior = new System.Windows.Forms.Button();
            this.LogisticaDgvAuditorias = new System.Windows.Forms.DataGridView();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBodega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObservaciones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogisticaLblHistorial = new System.Windows.Forms.Label();
            this.LogisticaPnlOpciones.SuspendLayout();
            this.LogisticaGpbConsulta.SuspendLayout();
            this.LogisticaGpbNuevaAuditoria.SuspendLayout();
            this.LogisticaPnlHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogisticaDgvAuditorias)).BeginInit();
            this.SuspendLayout();
            // 
            // LogisticaLblTitulo
            // 
            this.LogisticaLblTitulo.AutoSize = true;
            this.LogisticaLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaLblTitulo.Font = new System.Drawing.Font("Lucida Bright", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaLblTitulo.Location = new System.Drawing.Point(145, 25);
            this.LogisticaLblTitulo.Name = "LogisticaLblTitulo";
            this.LogisticaLblTitulo.Size = new System.Drawing.Size(607, 37);
            this.LogisticaLblTitulo.TabIndex = 0;
            this.LogisticaLblTitulo.Text = "Muestreo y Auditoría de Inventario";
            this.LogisticaLblTitulo.UseWaitCursor = true;
            // 
            // LogisticaLblDescripcion
            // 
            this.LogisticaLblDescripcion.AutoSize = true;
            this.LogisticaLblDescripcion.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaLblDescripcion.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaLblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(114)))), ((int)(((byte)(56)))));
            this.LogisticaLblDescripcion.Location = new System.Drawing.Point(148, 71);
            this.LogisticaLblDescripcion.Name = "LogisticaLblDescripcion";
            this.LogisticaLblDescripcion.Size = new System.Drawing.Size(399, 22);
            this.LogisticaLblDescripcion.TabIndex = 1;
            this.LogisticaLblDescripcion.Text = "Control y verificación de existencias de productos";
            this.LogisticaLblDescripcion.UseWaitCursor = true;
            // 
            // LogisticaPnlOpciones
            // 
            this.LogisticaPnlOpciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogisticaPnlOpciones.BackColor = System.Drawing.Color.White;
            this.LogisticaPnlOpciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LogisticaPnlOpciones.Controls.Add(this.LogisticaGpbConsulta);
            this.LogisticaPnlOpciones.Controls.Add(this.LogisticaGpbNuevaAuditoria);
            this.LogisticaPnlOpciones.Location = new System.Drawing.Point(69, 124);
            this.LogisticaPnlOpciones.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaPnlOpciones.Name = "LogisticaPnlOpciones";
            this.LogisticaPnlOpciones.Size = new System.Drawing.Size(1155, 195);
            this.LogisticaPnlOpciones.TabIndex = 2;
            this.LogisticaPnlOpciones.UseWaitCursor = true;
            // 
            // LogisticaGpbConsulta
            // 
            this.LogisticaGpbConsulta.Controls.Add(this.LogisticaBtnBuscar);
            this.LogisticaGpbConsulta.Controls.Add(this.LogisticaTxtBuscar);
            this.LogisticaGpbConsulta.Controls.Add(this.LogisticaCboEstado);
            this.LogisticaGpbConsulta.Controls.Add(this.LogisticaCboBodega);
            this.LogisticaGpbConsulta.Controls.Add(this.LogisticaLblBuscar);
            this.LogisticaGpbConsulta.Controls.Add(this.LogisticaLblEstado);
            this.LogisticaGpbConsulta.Controls.Add(this.LogisticaLblBodega);
            this.LogisticaGpbConsulta.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaGpbConsulta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaGpbConsulta.Location = new System.Drawing.Point(575, 10);
            this.LogisticaGpbConsulta.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaGpbConsulta.Name = "LogisticaGpbConsulta";
            this.LogisticaGpbConsulta.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaGpbConsulta.Size = new System.Drawing.Size(560, 170);
            this.LogisticaGpbConsulta.TabIndex = 4;
            this.LogisticaGpbConsulta.TabStop = false;
            this.LogisticaGpbConsulta.Text = "Consulta rápida";
            this.LogisticaGpbConsulta.UseWaitCursor = true;
            // 
            // LogisticaBtnBuscar
            // 
            this.LogisticaBtnBuscar.Location = new System.Drawing.Point(371, 114);
            this.LogisticaBtnBuscar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaBtnBuscar.Name = "LogisticaBtnBuscar";
            this.LogisticaBtnBuscar.Size = new System.Drawing.Size(160, 38);
            this.LogisticaBtnBuscar.TabIndex = 6;
            this.LogisticaBtnBuscar.Text = "Buscar";
            this.LogisticaBtnBuscar.UseVisualStyleBackColor = true;
            this.LogisticaBtnBuscar.UseWaitCursor = true;
            // 
            // LogisticaTxtBuscar
            // 
            this.LogisticaTxtBuscar.Location = new System.Drawing.Point(15, 121);
            this.LogisticaTxtBuscar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaTxtBuscar.Name = "LogisticaTxtBuscar";
            this.LogisticaTxtBuscar.Size = new System.Drawing.Size(335, 29);
            this.LogisticaTxtBuscar.TabIndex = 5;
            this.LogisticaTxtBuscar.UseWaitCursor = true;
            // 
            // LogisticaCboEstado
            // 
            this.LogisticaCboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LogisticaCboEstado.FormattingEnabled = true;
            this.LogisticaCboEstado.Location = new System.Drawing.Point(200, 57);
            this.LogisticaCboEstado.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaCboEstado.Name = "LogisticaCboEstado";
            this.LogisticaCboEstado.Size = new System.Drawing.Size(151, 30);
            this.LogisticaCboEstado.TabIndex = 4;
            this.LogisticaCboEstado.UseWaitCursor = true;
            // 
            // LogisticaCboBodega
            // 
            this.LogisticaCboBodega.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LogisticaCboBodega.FormattingEnabled = true;
            this.LogisticaCboBodega.Location = new System.Drawing.Point(15, 57);
            this.LogisticaCboBodega.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaCboBodega.Name = "LogisticaCboBodega";
            this.LogisticaCboBodega.Size = new System.Drawing.Size(169, 30);
            this.LogisticaCboBodega.TabIndex = 3;
            this.LogisticaCboBodega.UseWaitCursor = true;
            // 
            // LogisticaLblBuscar
            // 
            this.LogisticaLblBuscar.AutoSize = true;
            this.LogisticaLblBuscar.Location = new System.Drawing.Point(15, 196);
            this.LogisticaLblBuscar.Name = "LogisticaLblBuscar";
            this.LogisticaLblBuscar.Size = new System.Drawing.Size(171, 22);
            this.LogisticaLblBuscar.TabIndex = 2;
            this.LogisticaLblBuscar.Text = "Buscar por código";
            this.LogisticaLblBuscar.UseWaitCursor = true;
            // 
            // LogisticaLblEstado
            // 
            this.LogisticaLblEstado.AutoSize = true;
            this.LogisticaLblEstado.Location = new System.Drawing.Point(200, 33);
            this.LogisticaLblEstado.Name = "LogisticaLblEstado";
            this.LogisticaLblEstado.Size = new System.Drawing.Size(70, 22);
            this.LogisticaLblEstado.TabIndex = 1;
            this.LogisticaLblEstado.Text = "Estado";
            this.LogisticaLblEstado.UseWaitCursor = true;
            // 
            // LogisticaLblBodega
            // 
            this.LogisticaLblBodega.AutoSize = true;
            this.LogisticaLblBodega.Location = new System.Drawing.Point(15, 33);
            this.LogisticaLblBodega.Name = "LogisticaLblBodega";
            this.LogisticaLblBodega.Size = new System.Drawing.Size(77, 22);
            this.LogisticaLblBodega.TabIndex = 0;
            this.LogisticaLblBodega.Text = "Bodega";
            this.LogisticaLblBodega.UseWaitCursor = true;
            // 
            // LogisticaGpbNuevaAuditoria
            // 
            this.LogisticaGpbNuevaAuditoria.Controls.Add(this.LogisticaBtnMultiple);
            this.LogisticaGpbNuevaAuditoria.Controls.Add(this.LogisticaBtnIndividual);
            this.LogisticaGpbNuevaAuditoria.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaGpbNuevaAuditoria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaGpbNuevaAuditoria.Location = new System.Drawing.Point(15, 10);
            this.LogisticaGpbNuevaAuditoria.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaGpbNuevaAuditoria.Name = "LogisticaGpbNuevaAuditoria";
            this.LogisticaGpbNuevaAuditoria.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaGpbNuevaAuditoria.Size = new System.Drawing.Size(540, 170);
            this.LogisticaGpbNuevaAuditoria.TabIndex = 3;
            this.LogisticaGpbNuevaAuditoria.TabStop = false;
            this.LogisticaGpbNuevaAuditoria.Text = "Nueva auditoría";
            this.LogisticaGpbNuevaAuditoria.UseWaitCursor = true;
            // 
            // LogisticaBtnMultiple
            // 
            this.LogisticaBtnMultiple.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(216)))), ((int)(((byte)(197)))));
            this.LogisticaBtnMultiple.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.LogisticaBtnMultiple.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogisticaBtnMultiple.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaBtnMultiple.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaBtnMultiple.Location = new System.Drawing.Point(275, 46);
            this.LogisticaBtnMultiple.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaBtnMultiple.Name = "LogisticaBtnMultiple";
            this.LogisticaBtnMultiple.Size = new System.Drawing.Size(240, 75);
            this.LogisticaBtnMultiple.TabIndex = 1;
            this.LogisticaBtnMultiple.Text = "Iniciar auditoría múltiple";
            this.LogisticaBtnMultiple.UseVisualStyleBackColor = false;
            this.LogisticaBtnMultiple.UseWaitCursor = true;
            // 
            // LogisticaBtnIndividual
            // 
            this.LogisticaBtnIndividual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(216)))), ((int)(((byte)(197)))));
            this.LogisticaBtnIndividual.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.LogisticaBtnIndividual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogisticaBtnIndividual.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaBtnIndividual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaBtnIndividual.Location = new System.Drawing.Point(20, 46);
            this.LogisticaBtnIndividual.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaBtnIndividual.Name = "LogisticaBtnIndividual";
            this.LogisticaBtnIndividual.Size = new System.Drawing.Size(240, 75);
            this.LogisticaBtnIndividual.TabIndex = 0;
            this.LogisticaBtnIndividual.Text = "Iniciar auditoría individual";
            this.LogisticaBtnIndividual.UseVisualStyleBackColor = false;
            this.LogisticaBtnIndividual.UseWaitCursor = true;
            // 
            // LogisticaPnlHistorial
            // 
            this.LogisticaPnlHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogisticaPnlHistorial.BackColor = System.Drawing.Color.White;
            this.LogisticaPnlHistorial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LogisticaPnlHistorial.Controls.Add(this.LogisticaLblPagina);
            this.LogisticaPnlHistorial.Controls.Add(this.LogisticaBtnActualizar);
            this.LogisticaPnlHistorial.Controls.Add(this.LogisticaBtnEditar);
            this.LogisticaPnlHistorial.Controls.Add(this.LogisticaBtnConsultar);
            this.LogisticaPnlHistorial.Controls.Add(this.LogisticaBtnSiguiente);
            this.LogisticaPnlHistorial.Controls.Add(this.LogisticaBtnAnterior);
            this.LogisticaPnlHistorial.Controls.Add(this.LogisticaDgvAuditorias);
            this.LogisticaPnlHistorial.Controls.Add(this.LogisticaLblHistorial);
            this.LogisticaPnlHistorial.Location = new System.Drawing.Point(69, 341);
            this.LogisticaPnlHistorial.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaPnlHistorial.Name = "LogisticaPnlHistorial";
            this.LogisticaPnlHistorial.Size = new System.Drawing.Size(1155, 329);
            this.LogisticaPnlHistorial.TabIndex = 3;
            this.LogisticaPnlHistorial.UseWaitCursor = true;
            // 
            // LogisticaLblPagina
            // 
            this.LogisticaLblPagina.AutoSize = true;
            this.LogisticaLblPagina.Location = new System.Drawing.Point(145, 284);
            this.LogisticaLblPagina.Name = "LogisticaLblPagina";
            this.LogisticaLblPagina.Size = new System.Drawing.Size(89, 16);
            this.LogisticaLblPagina.TabIndex = 7;
            this.LogisticaLblPagina.Text = "Página 1 de 1";
            this.LogisticaLblPagina.UseWaitCursor = true;
            // 
            // LogisticaBtnActualizar
            // 
            this.LogisticaBtnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(216)))), ((int)(((byte)(197)))));
            this.LogisticaBtnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogisticaBtnActualizar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaBtnActualizar.Location = new System.Drawing.Point(955, 274);
            this.LogisticaBtnActualizar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaBtnActualizar.Name = "LogisticaBtnActualizar";
            this.LogisticaBtnActualizar.Size = new System.Drawing.Size(145, 34);
            this.LogisticaBtnActualizar.TabIndex = 6;
            this.LogisticaBtnActualizar.Text = "Actualizar";
            this.LogisticaBtnActualizar.UseVisualStyleBackColor = false;
            this.LogisticaBtnActualizar.UseWaitCursor = true;
            // 
            // LogisticaBtnEditar
            // 
            this.LogisticaBtnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(216)))), ((int)(((byte)(197)))));
            this.LogisticaBtnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogisticaBtnEditar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaBtnEditar.Location = new System.Drawing.Point(800, 274);
            this.LogisticaBtnEditar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaBtnEditar.Name = "LogisticaBtnEditar";
            this.LogisticaBtnEditar.Size = new System.Drawing.Size(145, 34);
            this.LogisticaBtnEditar.TabIndex = 5;
            this.LogisticaBtnEditar.Text = "Editar";
            this.LogisticaBtnEditar.UseVisualStyleBackColor = false;
            this.LogisticaBtnEditar.UseWaitCursor = true;
            // 
            // LogisticaBtnConsultar
            // 
            this.LogisticaBtnConsultar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(216)))), ((int)(((byte)(197)))));
            this.LogisticaBtnConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogisticaBtnConsultar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaBtnConsultar.Location = new System.Drawing.Point(645, 274);
            this.LogisticaBtnConsultar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaBtnConsultar.Name = "LogisticaBtnConsultar";
            this.LogisticaBtnConsultar.Size = new System.Drawing.Size(145, 34);
            this.LogisticaBtnConsultar.TabIndex = 4;
            this.LogisticaBtnConsultar.Text = "Consultar";
            this.LogisticaBtnConsultar.UseVisualStyleBackColor = false;
            this.LogisticaBtnConsultar.UseWaitCursor = true;
            // 
            // LogisticaBtnSiguiente
            // 
            this.LogisticaBtnSiguiente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(216)))), ((int)(((byte)(197)))));
            this.LogisticaBtnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogisticaBtnSiguiente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaBtnSiguiente.Location = new System.Drawing.Point(245, 274);
            this.LogisticaBtnSiguiente.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaBtnSiguiente.Name = "LogisticaBtnSiguiente";
            this.LogisticaBtnSiguiente.Size = new System.Drawing.Size(105, 34);
            this.LogisticaBtnSiguiente.TabIndex = 3;
            this.LogisticaBtnSiguiente.Text = "Siguiente";
            this.LogisticaBtnSiguiente.UseVisualStyleBackColor = false;
            this.LogisticaBtnSiguiente.UseWaitCursor = true;
            // 
            // LogisticaBtnAnterior
            // 
            this.LogisticaBtnAnterior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(216)))), ((int)(((byte)(197)))));
            this.LogisticaBtnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogisticaBtnAnterior.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaBtnAnterior.Location = new System.Drawing.Point(20, 274);
            this.LogisticaBtnAnterior.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaBtnAnterior.Name = "LogisticaBtnAnterior";
            this.LogisticaBtnAnterior.Size = new System.Drawing.Size(105, 34);
            this.LogisticaBtnAnterior.TabIndex = 2;
            this.LogisticaBtnAnterior.Text = "Anterior";
            this.LogisticaBtnAnterior.UseVisualStyleBackColor = false;
            this.LogisticaBtnAnterior.UseWaitCursor = true;
            // 
            // LogisticaDgvAuditorias
            // 
            this.LogisticaDgvAuditorias.AllowUserToAddRows = false;
            this.LogisticaDgvAuditorias.AllowUserToDeleteRows = false;
            this.LogisticaDgvAuditorias.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(246)))), ((int)(((byte)(242)))));
            this.LogisticaDgvAuditorias.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.LogisticaDgvAuditorias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogisticaDgvAuditorias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.LogisticaDgvAuditorias.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(138)))), ((int)(((byte)(132)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LogisticaDgvAuditorias.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.LogisticaDgvAuditorias.ColumnHeadersHeight = 38;
            this.LogisticaDgvAuditorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.LogisticaDgvAuditorias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCodigo,
            this.colFecha,
            this.colBodega,
            this.colTipo,
            this.colProductos,
            this.colResultado,
            this.colEstado,
            this.colObservaciones});
            this.LogisticaDgvAuditorias.EnableHeadersVisualStyles = false;
            this.LogisticaDgvAuditorias.Location = new System.Drawing.Point(20, 50);
            this.LogisticaDgvAuditorias.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogisticaDgvAuditorias.MultiSelect = false;
            this.LogisticaDgvAuditorias.Name = "LogisticaDgvAuditorias";
            this.LogisticaDgvAuditorias.ReadOnly = true;
            this.LogisticaDgvAuditorias.RowHeadersVisible = false;
            this.LogisticaDgvAuditorias.RowHeadersWidth = 51;
            this.LogisticaDgvAuditorias.RowTemplate.Height = 30;
            this.LogisticaDgvAuditorias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LogisticaDgvAuditorias.Size = new System.Drawing.Size(1109, 206);
            this.LogisticaDgvAuditorias.TabIndex = 1;
            this.LogisticaDgvAuditorias.UseWaitCursor = true;
            // 
            // colCodigo
            // 
            this.colCodigo.HeaderText = "Código";
            this.colCodigo.MinimumWidth = 6;
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.ReadOnly = true;
            // 
            // colFecha
            // 
            this.colFecha.HeaderText = "Fecha";
            this.colFecha.MinimumWidth = 6;
            this.colFecha.Name = "colFecha";
            this.colFecha.ReadOnly = true;
            // 
            // colBodega
            // 
            this.colBodega.HeaderText = "Bodega";
            this.colBodega.MinimumWidth = 6;
            this.colBodega.Name = "colBodega";
            this.colBodega.ReadOnly = true;
            // 
            // colTipo
            // 
            this.colTipo.HeaderText = "Tipo de auditoría";
            this.colTipo.MinimumWidth = 6;
            this.colTipo.Name = "colTipo";
            this.colTipo.ReadOnly = true;
            // 
            // colProductos
            // 
            this.colProductos.HeaderText = "Productos";
            this.colProductos.MinimumWidth = 6;
            this.colProductos.Name = "colProductos";
            this.colProductos.ReadOnly = true;
            // 
            // colResultado
            // 
            this.colResultado.HeaderText = "Resultado";
            this.colResultado.MinimumWidth = 6;
            this.colResultado.Name = "colResultado";
            this.colResultado.ReadOnly = true;
            // 
            // colEstado
            // 
            this.colEstado.HeaderText = "Estado";
            this.colEstado.MinimumWidth = 6;
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            // 
            // colObservaciones
            // 
            this.colObservaciones.HeaderText = "Observaciones";
            this.colObservaciones.MinimumWidth = 6;
            this.colObservaciones.Name = "colObservaciones";
            this.colObservaciones.ReadOnly = true;
            // 
            // LogisticaLblHistorial
            // 
            this.LogisticaLblHistorial.AutoSize = true;
            this.LogisticaLblHistorial.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaLblHistorial.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaLblHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(44)))), ((int)(((byte)(18)))));
            this.LogisticaLblHistorial.Location = new System.Drawing.Point(20, 15);
            this.LogisticaLblHistorial.Name = "LogisticaLblHistorial";
            this.LogisticaLblHistorial.Size = new System.Drawing.Size(232, 24);
            this.LogisticaLblHistorial.TabIndex = 0;
            this.LogisticaLblHistorial.Text = "Historial de auditorías";
            this.LogisticaLblHistorial.UseWaitCursor = true;
            // 
            // FrmAuditoriaInventario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CapaVista_MuestreoAuditoria_Logistica.Properties.Resources.FondoLogistica;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1265, 750);
            this.Controls.Add(this.LogisticaPnlHistorial);
            this.Controls.Add(this.LogisticaPnlOpciones);
            this.Controls.Add(this.LogisticaLblDescripcion);
            this.Controls.Add(this.LogisticaLblTitulo);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "FrmAuditoriaInventario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "5011 - Auditoría y muestreo de inventario";
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.FrmAuditoriaInventario_Load);
            this.LogisticaPnlOpciones.ResumeLayout(false);
            this.LogisticaGpbConsulta.ResumeLayout(false);
            this.LogisticaGpbConsulta.PerformLayout();
            this.LogisticaGpbNuevaAuditoria.ResumeLayout(false);
            this.LogisticaPnlHistorial.ResumeLayout(false);
            this.LogisticaPnlHistorial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogisticaDgvAuditorias)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LogisticaLblTitulo;
        private System.Windows.Forms.Label LogisticaLblDescripcion;
        private System.Windows.Forms.Panel LogisticaPnlOpciones;
        private System.Windows.Forms.GroupBox LogisticaGpbNuevaAuditoria;
        private System.Windows.Forms.Button LogisticaBtnMultiple;
        private System.Windows.Forms.Button LogisticaBtnIndividual;
        private System.Windows.Forms.GroupBox LogisticaGpbConsulta;
        private System.Windows.Forms.ComboBox LogisticaCboEstado;
        private System.Windows.Forms.ComboBox LogisticaCboBodega;
        private System.Windows.Forms.Label LogisticaLblBuscar;
        private System.Windows.Forms.Label LogisticaLblEstado;
        private System.Windows.Forms.Label LogisticaLblBodega;
        private System.Windows.Forms.Button LogisticaBtnBuscar;
        private System.Windows.Forms.TextBox LogisticaTxtBuscar;
        private System.Windows.Forms.Panel LogisticaPnlHistorial;
        private System.Windows.Forms.DataGridView LogisticaDgvAuditorias;
        private System.Windows.Forms.Label LogisticaLblHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBodega;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObservaciones;
        private System.Windows.Forms.Button LogisticaBtnActualizar;
        private System.Windows.Forms.Button LogisticaBtnEditar;
        private System.Windows.Forms.Button LogisticaBtnConsultar;
        private System.Windows.Forms.Button LogisticaBtnSiguiente;
        private System.Windows.Forms.Button LogisticaBtnAnterior;
        private System.Windows.Forms.Label LogisticaLblPagina;
    }
}
