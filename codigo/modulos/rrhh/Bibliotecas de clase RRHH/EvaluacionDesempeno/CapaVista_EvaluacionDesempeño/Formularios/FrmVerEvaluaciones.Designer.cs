/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * ==================================================================
 * Propósito :
 *  Diseño (Designer) de la ventana 9404 - VerEvaluaciones (consulta
 *  del colaborador: resultados, puntajes y retroalimentación).
 *  Prototipo NO funcional: solo controles visuales, sin datos de
 *  ejemplo, eventos, validaciones ni acceso a datos.
 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    partial class FrmVerEvaluaciones
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVerEvaluaciones));
            this.RrhhPnlTituloResultados = new System.Windows.Forms.Panel();
            this.RrhhLblTituloResultados = new System.Windows.Forms.Label();
            this.RrhhPnlResumen = new System.Windows.Forms.Panel();
            this.RrhhPnlEncabezadoResumen = new System.Windows.Forms.Panel();
            this.RrhhLblTituloResumen = new System.Windows.Forms.Label();
            this.RrhhLblResumenCiclo = new System.Windows.Forms.Label();
            this.RrhhCboResumenCiclo = new System.Windows.Forms.ComboBox();
            this.RrhhLblResumenColaborador = new System.Windows.Forms.Label();
            this.RrhhTxtResumenColaborador = new System.Windows.Forms.TextBox();
            this.RrhhLblResumenPuesto = new System.Windows.Forms.Label();
            this.RrhhTxtResumenPuesto = new System.Windows.Forms.TextBox();
            this.RrhhLblResumenEvaluador = new System.Windows.Forms.Label();
            this.RrhhTxtResumenEvaluador = new System.Windows.Forms.TextBox();
            this.RrhhLblResumenFecha = new System.Windows.Forms.Label();
            this.RrhhTxtResumenFecha = new System.Windows.Forms.TextBox();
            this.RrhhLblPuntajeFinal = new System.Windows.Forms.Label();
            this.RrhhLblPuntajeFinalValor = new System.Windows.Forms.Label();
            this.RrhhPnlPuntajePista = new System.Windows.Forms.Panel();
            this.RrhhPnlPuntajeRelleno = new System.Windows.Forms.Panel();
            this.RrhhLblNivelResumen = new System.Windows.Forms.Label();
            this.RrhhPnlCriterios = new System.Windows.Forms.Panel();
            this.RrhhPnlEncabezadoCriterios = new System.Windows.Forms.Panel();
            this.RrhhLblTituloCriterios = new System.Windows.Forms.Label();
            this.RrhhPnlEncabezadoTablaCriterios = new System.Windows.Forms.Panel();
            this.RrhhLblColumnaCriterio = new System.Windows.Forms.Label();
            this.RrhhLblColumnaPeso = new System.Windows.Forms.Label();
            this.RrhhLblColumnaNota = new System.Windows.Forms.Label();
            this.RrhhLblColumnaPonderado = new System.Windows.Forms.Label();
            this.RrhhPnlFilaCalidadTrabajo = new System.Windows.Forms.Panel();
            this.RrhhLblCriterioCalidadTrabajo = new System.Windows.Forms.Label();
            this.RrhhLblPesoCalidadTrabajo = new System.Windows.Forms.Label();
            this.RrhhLblNotaCalidadTrabajo = new System.Windows.Forms.Label();
            this.RrhhLblPonderadoCalidadTrabajo = new System.Windows.Forms.Label();
            this.RrhhPnlFilaProductividad = new System.Windows.Forms.Panel();
            this.RrhhLblCriterioProductividad = new System.Windows.Forms.Label();
            this.RrhhLblPesoProductividad = new System.Windows.Forms.Label();
            this.RrhhLblNotaProductividad = new System.Windows.Forms.Label();
            this.RrhhLblPonderadoProductividad = new System.Windows.Forms.Label();
            this.RrhhPnlFilaTrabajoEquipo = new System.Windows.Forms.Panel();
            this.RrhhLblCriterioTrabajoEquipo = new System.Windows.Forms.Label();
            this.RrhhLblPesoTrabajoEquipo = new System.Windows.Forms.Label();
            this.RrhhLblNotaTrabajoEquipo = new System.Windows.Forms.Label();
            this.RrhhLblPonderadoTrabajoEquipo = new System.Windows.Forms.Label();
            this.RrhhPnlFilaPuntualidad = new System.Windows.Forms.Panel();
            this.RrhhLblCriterioPuntualidad = new System.Windows.Forms.Label();
            this.RrhhLblPesoPuntualidad = new System.Windows.Forms.Label();
            this.RrhhLblNotaPuntualidad = new System.Windows.Forms.Label();
            this.RrhhLblPonderadoPuntualidad = new System.Windows.Forms.Label();
            this.RrhhPnlFilaIniciativa = new System.Windows.Forms.Panel();
            this.RrhhLblCriterioIniciativa = new System.Windows.Forms.Label();
            this.RrhhLblPesoIniciativa = new System.Windows.Forms.Label();
            this.RrhhLblNotaIniciativa = new System.Windows.Forms.Label();
            this.RrhhLblPonderadoIniciativa = new System.Windows.Forms.Label();
            this.RrhhPnlTotalCriterios = new System.Windows.Forms.Panel();
            this.RrhhLblTotalCriteriosTexto = new System.Windows.Forms.Label();
            this.RrhhLblTotalCriteriosValor = new System.Windows.Forms.Label();
            this.RrhhLblLeyendaCriterios = new System.Windows.Forms.Label();
            this.RrhhPnlRetroalimentacion = new System.Windows.Forms.Panel();
            this.RrhhPnlEncabezadoRetroalimentacion = new System.Windows.Forms.Panel();
            this.RrhhLblTituloRetroalimentacion = new System.Windows.Forms.Label();
            this.RrhhLblPlanMejora = new System.Windows.Forms.Label();
            this.RrhhTxtPlanMejora = new System.Windows.Forms.TextBox();
            this.RrhhPnlHistorial = new System.Windows.Forms.Panel();
            this.RrhhPnlEncabezadoHistorial = new System.Windows.Forms.Panel();
            this.RrhhLblTituloHistorial = new System.Windows.Forms.Label();
            this.RrhhDgvHistorial = new System.Windows.Forms.DataGridView();
            this.RrhhColHistorialEvaluador = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RrhhColHistorialFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RrhhColHistorialPuntaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RrhhColHistorialNivel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RrhhColHistorialPorcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RrhhPnlBotones = new System.Windows.Forms.Panel();
            this.RrhhBtnRefrescar = new System.Windows.Forms.Button();
            this.RrhhBtnImprimir = new System.Windows.Forms.Button();
            this.RrhhBtnInicio = new System.Windows.Forms.Button();
            this.RrhhBtnAnterior = new System.Windows.Forms.Button();
            this.RrhhBtnSiguiente = new System.Windows.Forms.Button();
            this.RrhhBtnFin = new System.Windows.Forms.Button();
            this.RrhhBtnAyuda = new System.Windows.Forms.Button();
            this.RrhhBtnSalir = new System.Windows.Forms.Button();
            this.RrhhPnlEncabezado = new System.Windows.Forms.Panel();
            this.RrhhLblTitulo = new System.Windows.Forms.Label();
            this.RrhhLblSubtitulo = new System.Windows.Forms.Label();
            this.RrhhPbIconoVerEvaluaciones = new System.Windows.Forms.PictureBox();
            this.RrhhPbMascota = new System.Windows.Forms.PictureBox();
            this.RrhhPnlTituloResultados.SuspendLayout();
            this.RrhhPnlResumen.SuspendLayout();
            this.RrhhPnlEncabezadoResumen.SuspendLayout();
            this.RrhhPnlPuntajePista.SuspendLayout();
            this.RrhhPnlCriterios.SuspendLayout();
            this.RrhhPnlEncabezadoCriterios.SuspendLayout();
            this.RrhhPnlEncabezadoTablaCriterios.SuspendLayout();
            this.RrhhPnlFilaCalidadTrabajo.SuspendLayout();
            this.RrhhPnlFilaProductividad.SuspendLayout();
            this.RrhhPnlFilaTrabajoEquipo.SuspendLayout();
            this.RrhhPnlFilaPuntualidad.SuspendLayout();
            this.RrhhPnlFilaIniciativa.SuspendLayout();
            this.RrhhPnlTotalCriterios.SuspendLayout();
            this.RrhhPnlRetroalimentacion.SuspendLayout();
            this.RrhhPnlEncabezadoRetroalimentacion.SuspendLayout();
            this.RrhhPnlHistorial.SuspendLayout();
            this.RrhhPnlEncabezadoHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RrhhDgvHistorial)).BeginInit();
            this.RrhhPnlBotones.SuspendLayout();
            this.RrhhPnlEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RrhhPbIconoVerEvaluaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RrhhPbMascota)).BeginInit();
            this.SuspendLayout();
            // 
            // RrhhPnlTituloResultados
            // 
            this.RrhhPnlTituloResultados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(247)))), ((int)(((byte)(246)))));
            this.RrhhPnlTituloResultados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhPnlTituloResultados.Controls.Add(this.RrhhLblTituloResultados);
            this.RrhhPnlTituloResultados.Location = new System.Drawing.Point(10, 218);
            this.RrhhPnlTituloResultados.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlTituloResultados.Name = "RrhhPnlTituloResultados";
            this.RrhhPnlTituloResultados.Size = new System.Drawing.Size(1384, 44);
            this.RrhhPnlTituloResultados.TabIndex = 1;
            // 
            // RrhhLblTituloResultados
            // 
            this.RrhhLblTituloResultados.AutoSize = true;
            this.RrhhLblTituloResultados.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblTituloResultados.Font = new System.Drawing.Font("Tahoma", 10.21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblTituloResultados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblTituloResultados.Location = new System.Drawing.Point(17, 9);
            this.RrhhLblTituloResultados.Name = "RrhhLblTituloResultados";
            this.RrhhLblTituloResultados.Size = new System.Drawing.Size(278, 25);
            this.RrhhLblTituloResultados.TabIndex = 0;
            this.RrhhLblTituloResultados.Text = "Resultados de mi Evaluación";
            // 
            // RrhhPnlResumen
            // 
            this.RrhhPnlResumen.BackColor = System.Drawing.Color.White;
            this.RrhhPnlResumen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhPnlResumen.Controls.Add(this.RrhhPnlEncabezadoResumen);
            this.RrhhPnlResumen.Controls.Add(this.RrhhLblResumenCiclo);
            this.RrhhPnlResumen.Controls.Add(this.RrhhCboResumenCiclo);
            this.RrhhPnlResumen.Controls.Add(this.RrhhLblResumenColaborador);
            this.RrhhPnlResumen.Controls.Add(this.RrhhTxtResumenColaborador);
            this.RrhhPnlResumen.Controls.Add(this.RrhhLblResumenPuesto);
            this.RrhhPnlResumen.Controls.Add(this.RrhhTxtResumenPuesto);
            this.RrhhPnlResumen.Controls.Add(this.RrhhLblResumenEvaluador);
            this.RrhhPnlResumen.Controls.Add(this.RrhhTxtResumenEvaluador);
            this.RrhhPnlResumen.Controls.Add(this.RrhhLblResumenFecha);
            this.RrhhPnlResumen.Controls.Add(this.RrhhTxtResumenFecha);
            this.RrhhPnlResumen.Controls.Add(this.RrhhLblPuntajeFinal);
            this.RrhhPnlResumen.Controls.Add(this.RrhhLblPuntajeFinalValor);
            this.RrhhPnlResumen.Controls.Add(this.RrhhPnlPuntajePista);
            this.RrhhPnlResumen.Controls.Add(this.RrhhLblNivelResumen);
            this.RrhhPnlResumen.Location = new System.Drawing.Point(10, 295);
            this.RrhhPnlResumen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlResumen.Name = "RrhhPnlResumen";
            this.RrhhPnlResumen.Size = new System.Drawing.Size(448, 362);
            this.RrhhPnlResumen.TabIndex = 2;
            // 
            // RrhhPnlEncabezadoResumen
            // 
            this.RrhhPnlEncabezadoResumen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(238)))), ((int)(((byte)(240)))));
            this.RrhhPnlEncabezadoResumen.Controls.Add(this.RrhhLblTituloResumen);
            this.RrhhPnlEncabezadoResumen.Location = new System.Drawing.Point(0, 0);
            this.RrhhPnlEncabezadoResumen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlEncabezadoResumen.Name = "RrhhPnlEncabezadoResumen";
            this.RrhhPnlEncabezadoResumen.Size = new System.Drawing.Size(446, 28);
            this.RrhhPnlEncabezadoResumen.TabIndex = 0;
            // 
            // RrhhLblTituloResumen
            // 
            this.RrhhLblTituloResumen.AutoSize = true;
            this.RrhhLblTituloResumen.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblTituloResumen.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblTituloResumen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblTituloResumen.Location = new System.Drawing.Point(14, 5);
            this.RrhhLblTituloResumen.Name = "RrhhLblTituloResumen";
            this.RrhhLblTituloResumen.Size = new System.Drawing.Size(201, 18);
            this.RrhhLblTituloResumen.TabIndex = 0;
            this.RrhhLblTituloResumen.Text = "Resumen de la Evaluación";
            // 
            // RrhhLblResumenCiclo
            // 
            this.RrhhLblResumenCiclo.AutoSize = true;
            this.RrhhLblResumenCiclo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblResumenCiclo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblResumenCiclo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblResumenCiclo.Location = new System.Drawing.Point(16, 48);
            this.RrhhLblResumenCiclo.Name = "RrhhLblResumenCiclo";
            this.RrhhLblResumenCiclo.Size = new System.Drawing.Size(80, 18);
            this.RrhhLblResumenCiclo.TabIndex = 1;
            this.RrhhLblResumenCiclo.Text = "Evaluación:";
            // 
            // RrhhCboResumenCiclo
            // 
            this.RrhhCboResumenCiclo.BackColor = System.Drawing.Color.White;
            this.RrhhCboResumenCiclo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RrhhCboResumenCiclo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhCboResumenCiclo.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhCboResumenCiclo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhCboResumenCiclo.FormattingEnabled = true;
            this.RrhhCboResumenCiclo.Location = new System.Drawing.Point(128, 45);
            this.RrhhCboResumenCiclo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhCboResumenCiclo.Name = "RrhhCboResumenCiclo";
            this.RrhhCboResumenCiclo.Size = new System.Drawing.Size(258, 28);
            this.RrhhCboResumenCiclo.TabIndex = 2;
            // 
            // RrhhLblResumenColaborador
            // 
            this.RrhhLblResumenColaborador.AutoSize = true;
            this.RrhhLblResumenColaborador.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblResumenColaborador.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblResumenColaborador.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblResumenColaborador.Location = new System.Drawing.Point(16, 88);
            this.RrhhLblResumenColaborador.Name = "RrhhLblResumenColaborador";
            this.RrhhLblResumenColaborador.Size = new System.Drawing.Size(90, 18);
            this.RrhhLblResumenColaborador.TabIndex = 3;
            this.RrhhLblResumenColaborador.Text = "Colaborador:";
            // 
            // RrhhTxtResumenColaborador
            // 
            this.RrhhTxtResumenColaborador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.RrhhTxtResumenColaborador.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhTxtResumenColaborador.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhTxtResumenColaborador.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhTxtResumenColaborador.Location = new System.Drawing.Point(128, 82);
            this.RrhhTxtResumenColaborador.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhTxtResumenColaborador.Name = "RrhhTxtResumenColaborador";
            this.RrhhTxtResumenColaborador.ReadOnly = true;
            this.RrhhTxtResumenColaborador.Size = new System.Drawing.Size(260, 27);
            this.RrhhTxtResumenColaborador.TabIndex = 4;
            this.RrhhTxtResumenColaborador.TabStop = false;
            // 
            // RrhhLblResumenPuesto
            // 
            this.RrhhLblResumenPuesto.AutoSize = true;
            this.RrhhLblResumenPuesto.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblResumenPuesto.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblResumenPuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblResumenPuesto.Location = new System.Drawing.Point(16, 125);
            this.RrhhLblResumenPuesto.Name = "RrhhLblResumenPuesto";
            this.RrhhLblResumenPuesto.Size = new System.Drawing.Size(57, 18);
            this.RrhhLblResumenPuesto.TabIndex = 5;
            this.RrhhLblResumenPuesto.Text = "Puesto:";
            // 
            // RrhhTxtResumenPuesto
            // 
            this.RrhhTxtResumenPuesto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.RrhhTxtResumenPuesto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhTxtResumenPuesto.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhTxtResumenPuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhTxtResumenPuesto.Location = new System.Drawing.Point(128, 122);
            this.RrhhTxtResumenPuesto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhTxtResumenPuesto.Name = "RrhhTxtResumenPuesto";
            this.RrhhTxtResumenPuesto.ReadOnly = true;
            this.RrhhTxtResumenPuesto.Size = new System.Drawing.Size(260, 27);
            this.RrhhTxtResumenPuesto.TabIndex = 6;
            this.RrhhTxtResumenPuesto.TabStop = false;
            // 
            // RrhhLblResumenEvaluador
            // 
            this.RrhhLblResumenEvaluador.AutoSize = true;
            this.RrhhLblResumenEvaluador.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblResumenEvaluador.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblResumenEvaluador.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblResumenEvaluador.Location = new System.Drawing.Point(16, 165);
            this.RrhhLblResumenEvaluador.Name = "RrhhLblResumenEvaluador";
            this.RrhhLblResumenEvaluador.Size = new System.Drawing.Size(76, 18);
            this.RrhhLblResumenEvaluador.TabIndex = 7;
            this.RrhhLblResumenEvaluador.Text = "Evaluador:";
            // 
            // RrhhTxtResumenEvaluador
            // 
            this.RrhhTxtResumenEvaluador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.RrhhTxtResumenEvaluador.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhTxtResumenEvaluador.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhTxtResumenEvaluador.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhTxtResumenEvaluador.Location = new System.Drawing.Point(128, 161);
            this.RrhhTxtResumenEvaluador.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhTxtResumenEvaluador.Name = "RrhhTxtResumenEvaluador";
            this.RrhhTxtResumenEvaluador.ReadOnly = true;
            this.RrhhTxtResumenEvaluador.Size = new System.Drawing.Size(260, 27);
            this.RrhhTxtResumenEvaluador.TabIndex = 8;
            this.RrhhTxtResumenEvaluador.TabStop = false;
            // 
            // RrhhLblResumenFecha
            // 
            this.RrhhLblResumenFecha.AutoSize = true;
            this.RrhhLblResumenFecha.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblResumenFecha.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblResumenFecha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblResumenFecha.Location = new System.Drawing.Point(16, 205);
            this.RrhhLblResumenFecha.Name = "RrhhLblResumenFecha";
            this.RrhhLblResumenFecha.Size = new System.Drawing.Size(52, 18);
            this.RrhhLblResumenFecha.TabIndex = 9;
            this.RrhhLblResumenFecha.Text = "Fecha:";
            // 
            // RrhhTxtResumenFecha
            // 
            this.RrhhTxtResumenFecha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.RrhhTxtResumenFecha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhTxtResumenFecha.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhTxtResumenFecha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhTxtResumenFecha.Location = new System.Drawing.Point(128, 200);
            this.RrhhTxtResumenFecha.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhTxtResumenFecha.Name = "RrhhTxtResumenFecha";
            this.RrhhTxtResumenFecha.ReadOnly = true;
            this.RrhhTxtResumenFecha.Size = new System.Drawing.Size(260, 27);
            this.RrhhTxtResumenFecha.TabIndex = 10;
            this.RrhhTxtResumenFecha.TabStop = false;
            // 
            // RrhhLblPuntajeFinal
            // 
            this.RrhhLblPuntajeFinal.AutoSize = true;
            this.RrhhLblPuntajeFinal.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPuntajeFinal.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPuntajeFinal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPuntajeFinal.Location = new System.Drawing.Point(16, 258);
            this.RrhhLblPuntajeFinal.Name = "RrhhLblPuntajeFinal";
            this.RrhhLblPuntajeFinal.Size = new System.Drawing.Size(92, 18);
            this.RrhhLblPuntajeFinal.TabIndex = 11;
            this.RrhhLblPuntajeFinal.Text = "Puntaje final:";
            // 
            // RrhhLblPuntajeFinalValor
            // 
            this.RrhhLblPuntajeFinalValor.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPuntajeFinalValor.Font = new System.Drawing.Font("Lucida Bright", 14.74F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPuntajeFinalValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblPuntajeFinalValor.Location = new System.Drawing.Point(117, 238);
            this.RrhhLblPuntajeFinalValor.Name = "RrhhLblPuntajeFinalValor";
            this.RrhhLblPuntajeFinalValor.Size = new System.Drawing.Size(91, 49);
            this.RrhhLblPuntajeFinalValor.TabIndex = 12;
            this.RrhhLblPuntajeFinalValor.Text = "0%";
            this.RrhhLblPuntajeFinalValor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhPnlPuntajePista
            // 
            this.RrhhPnlPuntajePista.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(232)))), ((int)(((byte)(229)))));
            this.RrhhPnlPuntajePista.Controls.Add(this.RrhhPnlPuntajeRelleno);
            this.RrhhPnlPuntajePista.Location = new System.Drawing.Point(215, 251);
            this.RrhhPnlPuntajePista.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlPuntajePista.Name = "RrhhPnlPuntajePista";
            this.RrhhPnlPuntajePista.Size = new System.Drawing.Size(174, 11);
            this.RrhhPnlPuntajePista.TabIndex = 13;
            // 
            // RrhhPnlPuntajeRelleno
            // 
            this.RrhhPnlPuntajeRelleno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(139)))), ((int)(((byte)(145)))));
            this.RrhhPnlPuntajeRelleno.Location = new System.Drawing.Point(0, 0);
            this.RrhhPnlPuntajeRelleno.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlPuntajeRelleno.Name = "RrhhPnlPuntajeRelleno";
            this.RrhhPnlPuntajeRelleno.Size = new System.Drawing.Size(9, 11);
            this.RrhhPnlPuntajeRelleno.TabIndex = 0;
            // 
            // RrhhLblNivelResumen
            // 
            this.RrhhLblNivelResumen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(232)))), ((int)(((byte)(233)))));
            this.RrhhLblNivelResumen.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblNivelResumen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(96)))), ((int)(((byte)(100)))));
            this.RrhhLblNivelResumen.Location = new System.Drawing.Point(215, 270);
            this.RrhhLblNivelResumen.Name = "RrhhLblNivelResumen";
            this.RrhhLblNivelResumen.Size = new System.Drawing.Size(174, 22);
            this.RrhhLblNivelResumen.TabIndex = 14;
            this.RrhhLblNivelResumen.Text = "—";
            this.RrhhLblNivelResumen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhPnlCriterios
            // 
            this.RrhhPnlCriterios.BackColor = System.Drawing.Color.White;
            this.RrhhPnlCriterios.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhPnlCriterios.Controls.Add(this.RrhhPnlEncabezadoCriterios);
            this.RrhhPnlCriterios.Controls.Add(this.RrhhPnlEncabezadoTablaCriterios);
            this.RrhhPnlCriterios.Controls.Add(this.RrhhPnlFilaCalidadTrabajo);
            this.RrhhPnlCriterios.Controls.Add(this.RrhhPnlFilaProductividad);
            this.RrhhPnlCriterios.Controls.Add(this.RrhhPnlFilaTrabajoEquipo);
            this.RrhhPnlCriterios.Controls.Add(this.RrhhPnlFilaPuntualidad);
            this.RrhhPnlCriterios.Controls.Add(this.RrhhPnlFilaIniciativa);
            this.RrhhPnlCriterios.Controls.Add(this.RrhhPnlTotalCriterios);
            this.RrhhPnlCriterios.Controls.Add(this.RrhhLblLeyendaCriterios);
            this.RrhhPnlCriterios.Location = new System.Drawing.Point(471, 295);
            this.RrhhPnlCriterios.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlCriterios.Name = "RrhhPnlCriterios";
            this.RrhhPnlCriterios.Size = new System.Drawing.Size(456, 362);
            this.RrhhPnlCriterios.TabIndex = 3;
            // 
            // RrhhPnlEncabezadoCriterios
            // 
            this.RrhhPnlEncabezadoCriterios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(238)))), ((int)(((byte)(240)))));
            this.RrhhPnlEncabezadoCriterios.Controls.Add(this.RrhhLblTituloCriterios);
            this.RrhhPnlEncabezadoCriterios.Location = new System.Drawing.Point(0, 0);
            this.RrhhPnlEncabezadoCriterios.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlEncabezadoCriterios.Name = "RrhhPnlEncabezadoCriterios";
            this.RrhhPnlEncabezadoCriterios.Size = new System.Drawing.Size(454, 28);
            this.RrhhPnlEncabezadoCriterios.TabIndex = 0;
            // 
            // RrhhLblTituloCriterios
            // 
            this.RrhhLblTituloCriterios.AutoSize = true;
            this.RrhhLblTituloCriterios.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblTituloCriterios.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblTituloCriterios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblTituloCriterios.Location = new System.Drawing.Point(14, 5);
            this.RrhhLblTituloCriterios.Name = "RrhhLblTituloCriterios";
            this.RrhhLblTituloCriterios.Size = new System.Drawing.Size(187, 18);
            this.RrhhLblTituloCriterios.TabIndex = 0;
            this.RrhhLblTituloCriterios.Text = "Calificación por Criterio";
            // 
            // RrhhPnlEncabezadoTablaCriterios
            // 
            this.RrhhPnlEncabezadoTablaCriterios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhPnlEncabezadoTablaCriterios.Controls.Add(this.RrhhLblColumnaCriterio);
            this.RrhhPnlEncabezadoTablaCriterios.Controls.Add(this.RrhhLblColumnaPeso);
            this.RrhhPnlEncabezadoTablaCriterios.Controls.Add(this.RrhhLblColumnaNota);
            this.RrhhPnlEncabezadoTablaCriterios.Controls.Add(this.RrhhLblColumnaPonderado);
            this.RrhhPnlEncabezadoTablaCriterios.Location = new System.Drawing.Point(14, 36);
            this.RrhhPnlEncabezadoTablaCriterios.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlEncabezadoTablaCriterios.Name = "RrhhPnlEncabezadoTablaCriterios";
            this.RrhhPnlEncabezadoTablaCriterios.Size = new System.Drawing.Size(385, 30);
            this.RrhhPnlEncabezadoTablaCriterios.TabIndex = 1;
            // 
            // RrhhLblColumnaCriterio
            // 
            this.RrhhLblColumnaCriterio.AutoSize = true;
            this.RrhhLblColumnaCriterio.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblColumnaCriterio.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblColumnaCriterio.ForeColor = System.Drawing.Color.White;
            this.RrhhLblColumnaCriterio.Location = new System.Drawing.Point(10, 8);
            this.RrhhLblColumnaCriterio.Name = "RrhhLblColumnaCriterio";
            this.RrhhLblColumnaCriterio.Size = new System.Drawing.Size(66, 18);
            this.RrhhLblColumnaCriterio.TabIndex = 0;
            this.RrhhLblColumnaCriterio.Text = "Criterio";
            // 
            // RrhhLblColumnaPeso
            // 
            this.RrhhLblColumnaPeso.AutoSize = true;
            this.RrhhLblColumnaPeso.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblColumnaPeso.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblColumnaPeso.ForeColor = System.Drawing.Color.White;
            this.RrhhLblColumnaPeso.Location = new System.Drawing.Point(196, 8);
            this.RrhhLblColumnaPeso.Name = "RrhhLblColumnaPeso";
            this.RrhhLblColumnaPeso.Size = new System.Drawing.Size(44, 18);
            this.RrhhLblColumnaPeso.TabIndex = 1;
            this.RrhhLblColumnaPeso.Text = "Peso";
            // 
            // RrhhLblColumnaNota
            // 
            this.RrhhLblColumnaNota.AutoSize = true;
            this.RrhhLblColumnaNota.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblColumnaNota.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblColumnaNota.ForeColor = System.Drawing.Color.White;
            this.RrhhLblColumnaNota.Location = new System.Drawing.Point(248, 8);
            this.RrhhLblColumnaNota.Name = "RrhhLblColumnaNota";
            this.RrhhLblColumnaNota.Size = new System.Drawing.Size(44, 18);
            this.RrhhLblColumnaNota.TabIndex = 2;
            this.RrhhLblColumnaNota.Text = "Nota";
            // 
            // RrhhLblColumnaPonderado
            // 
            this.RrhhLblColumnaPonderado.AutoSize = true;
            this.RrhhLblColumnaPonderado.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblColumnaPonderado.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblColumnaPonderado.ForeColor = System.Drawing.Color.White;
            this.RrhhLblColumnaPonderado.Location = new System.Drawing.Point(292, 8);
            this.RrhhLblColumnaPonderado.Name = "RrhhLblColumnaPonderado";
            this.RrhhLblColumnaPonderado.Size = new System.Drawing.Size(88, 18);
            this.RrhhLblColumnaPonderado.TabIndex = 3;
            this.RrhhLblColumnaPonderado.Text = "Ponderado";
            // 
            // RrhhPnlFilaCalidadTrabajo
            // 
            this.RrhhPnlFilaCalidadTrabajo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(244)))), ((int)(((byte)(243)))));
            this.RrhhPnlFilaCalidadTrabajo.Controls.Add(this.RrhhLblCriterioCalidadTrabajo);
            this.RrhhPnlFilaCalidadTrabajo.Controls.Add(this.RrhhLblPesoCalidadTrabajo);
            this.RrhhPnlFilaCalidadTrabajo.Controls.Add(this.RrhhLblNotaCalidadTrabajo);
            this.RrhhPnlFilaCalidadTrabajo.Controls.Add(this.RrhhLblPonderadoCalidadTrabajo);
            this.RrhhPnlFilaCalidadTrabajo.Location = new System.Drawing.Point(14, 68);
            this.RrhhPnlFilaCalidadTrabajo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlFilaCalidadTrabajo.Name = "RrhhPnlFilaCalidadTrabajo";
            this.RrhhPnlFilaCalidadTrabajo.Size = new System.Drawing.Size(385, 34);
            this.RrhhPnlFilaCalidadTrabajo.TabIndex = 2;
            // 
            // RrhhLblCriterioCalidadTrabajo
            // 
            this.RrhhLblCriterioCalidadTrabajo.AutoSize = true;
            this.RrhhLblCriterioCalidadTrabajo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblCriterioCalidadTrabajo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblCriterioCalidadTrabajo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblCriterioCalidadTrabajo.Location = new System.Drawing.Point(10, 9);
            this.RrhhLblCriterioCalidadTrabajo.Name = "RrhhLblCriterioCalidadTrabajo";
            this.RrhhLblCriterioCalidadTrabajo.Size = new System.Drawing.Size(127, 18);
            this.RrhhLblCriterioCalidadTrabajo.TabIndex = 0;
            this.RrhhLblCriterioCalidadTrabajo.Text = "Calidad del trabajo";
            // 
            // RrhhLblPesoCalidadTrabajo
            // 
            this.RrhhLblPesoCalidadTrabajo.AutoSize = true;
            this.RrhhLblPesoCalidadTrabajo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPesoCalidadTrabajo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPesoCalidadTrabajo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPesoCalidadTrabajo.Location = new System.Drawing.Point(196, 9);
            this.RrhhLblPesoCalidadTrabajo.Name = "RrhhLblPesoCalidadTrabajo";
            this.RrhhLblPesoCalidadTrabajo.Size = new System.Drawing.Size(39, 18);
            this.RrhhLblPesoCalidadTrabajo.TabIndex = 1;
            this.RrhhLblPesoCalidadTrabajo.Text = "25%";
            // 
            // RrhhLblNotaCalidadTrabajo
            // 
            this.RrhhLblNotaCalidadTrabajo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblNotaCalidadTrabajo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblNotaCalidadTrabajo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblNotaCalidadTrabajo.Location = new System.Drawing.Point(243, 9);
            this.RrhhLblNotaCalidadTrabajo.Name = "RrhhLblNotaCalidadTrabajo";
            this.RrhhLblNotaCalidadTrabajo.Size = new System.Drawing.Size(33, 19);
            this.RrhhLblNotaCalidadTrabajo.TabIndex = 2;
            this.RrhhLblNotaCalidadTrabajo.Text = "—";
            this.RrhhLblNotaCalidadTrabajo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhLblPonderadoCalidadTrabajo
            // 
            this.RrhhLblPonderadoCalidadTrabajo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPonderadoCalidadTrabajo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPonderadoCalidadTrabajo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPonderadoCalidadTrabajo.Location = new System.Drawing.Point(292, 9);
            this.RrhhLblPonderadoCalidadTrabajo.Name = "RrhhLblPonderadoCalidadTrabajo";
            this.RrhhLblPonderadoCalidadTrabajo.Size = new System.Drawing.Size(79, 19);
            this.RrhhLblPonderadoCalidadTrabajo.TabIndex = 3;
            this.RrhhLblPonderadoCalidadTrabajo.Text = "0.00";
            this.RrhhLblPonderadoCalidadTrabajo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhPnlFilaProductividad
            // 
            this.RrhhPnlFilaProductividad.BackColor = System.Drawing.Color.White;
            this.RrhhPnlFilaProductividad.Controls.Add(this.RrhhLblCriterioProductividad);
            this.RrhhPnlFilaProductividad.Controls.Add(this.RrhhLblPesoProductividad);
            this.RrhhPnlFilaProductividad.Controls.Add(this.RrhhLblNotaProductividad);
            this.RrhhPnlFilaProductividad.Controls.Add(this.RrhhLblPonderadoProductividad);
            this.RrhhPnlFilaProductividad.Location = new System.Drawing.Point(14, 101);
            this.RrhhPnlFilaProductividad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlFilaProductividad.Name = "RrhhPnlFilaProductividad";
            this.RrhhPnlFilaProductividad.Size = new System.Drawing.Size(385, 34);
            this.RrhhPnlFilaProductividad.TabIndex = 3;
            // 
            // RrhhLblCriterioProductividad
            // 
            this.RrhhLblCriterioProductividad.AutoSize = true;
            this.RrhhLblCriterioProductividad.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblCriterioProductividad.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblCriterioProductividad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblCriterioProductividad.Location = new System.Drawing.Point(10, 9);
            this.RrhhLblCriterioProductividad.Name = "RrhhLblCriterioProductividad";
            this.RrhhLblCriterioProductividad.Size = new System.Drawing.Size(93, 18);
            this.RrhhLblCriterioProductividad.TabIndex = 0;
            this.RrhhLblCriterioProductividad.Text = "Productividad";
            // 
            // RrhhLblPesoProductividad
            // 
            this.RrhhLblPesoProductividad.AutoSize = true;
            this.RrhhLblPesoProductividad.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPesoProductividad.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPesoProductividad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPesoProductividad.Location = new System.Drawing.Point(196, 9);
            this.RrhhLblPesoProductividad.Name = "RrhhLblPesoProductividad";
            this.RrhhLblPesoProductividad.Size = new System.Drawing.Size(39, 18);
            this.RrhhLblPesoProductividad.TabIndex = 1;
            this.RrhhLblPesoProductividad.Text = "25%";
            // 
            // RrhhLblNotaProductividad
            // 
            this.RrhhLblNotaProductividad.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblNotaProductividad.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblNotaProductividad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblNotaProductividad.Location = new System.Drawing.Point(243, 9);
            this.RrhhLblNotaProductividad.Name = "RrhhLblNotaProductividad";
            this.RrhhLblNotaProductividad.Size = new System.Drawing.Size(33, 19);
            this.RrhhLblNotaProductividad.TabIndex = 2;
            this.RrhhLblNotaProductividad.Text = "—";
            this.RrhhLblNotaProductividad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhLblPonderadoProductividad
            // 
            this.RrhhLblPonderadoProductividad.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPonderadoProductividad.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPonderadoProductividad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPonderadoProductividad.Location = new System.Drawing.Point(292, 9);
            this.RrhhLblPonderadoProductividad.Name = "RrhhLblPonderadoProductividad";
            this.RrhhLblPonderadoProductividad.Size = new System.Drawing.Size(79, 19);
            this.RrhhLblPonderadoProductividad.TabIndex = 3;
            this.RrhhLblPonderadoProductividad.Text = "0.00";
            this.RrhhLblPonderadoProductividad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhPnlFilaTrabajoEquipo
            // 
            this.RrhhPnlFilaTrabajoEquipo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(244)))), ((int)(((byte)(243)))));
            this.RrhhPnlFilaTrabajoEquipo.Controls.Add(this.RrhhLblCriterioTrabajoEquipo);
            this.RrhhPnlFilaTrabajoEquipo.Controls.Add(this.RrhhLblPesoTrabajoEquipo);
            this.RrhhPnlFilaTrabajoEquipo.Controls.Add(this.RrhhLblNotaTrabajoEquipo);
            this.RrhhPnlFilaTrabajoEquipo.Controls.Add(this.RrhhLblPonderadoTrabajoEquipo);
            this.RrhhPnlFilaTrabajoEquipo.Location = new System.Drawing.Point(14, 135);
            this.RrhhPnlFilaTrabajoEquipo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlFilaTrabajoEquipo.Name = "RrhhPnlFilaTrabajoEquipo";
            this.RrhhPnlFilaTrabajoEquipo.Size = new System.Drawing.Size(385, 34);
            this.RrhhPnlFilaTrabajoEquipo.TabIndex = 4;
            // 
            // RrhhLblCriterioTrabajoEquipo
            // 
            this.RrhhLblCriterioTrabajoEquipo.AutoSize = true;
            this.RrhhLblCriterioTrabajoEquipo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblCriterioTrabajoEquipo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblCriterioTrabajoEquipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblCriterioTrabajoEquipo.Location = new System.Drawing.Point(10, 9);
            this.RrhhLblCriterioTrabajoEquipo.Name = "RrhhLblCriterioTrabajoEquipo";
            this.RrhhLblCriterioTrabajoEquipo.Size = new System.Drawing.Size(127, 18);
            this.RrhhLblCriterioTrabajoEquipo.TabIndex = 0;
            this.RrhhLblCriterioTrabajoEquipo.Text = "Trabajo en equipo";
            // 
            // RrhhLblPesoTrabajoEquipo
            // 
            this.RrhhLblPesoTrabajoEquipo.AutoSize = true;
            this.RrhhLblPesoTrabajoEquipo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPesoTrabajoEquipo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPesoTrabajoEquipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPesoTrabajoEquipo.Location = new System.Drawing.Point(196, 9);
            this.RrhhLblPesoTrabajoEquipo.Name = "RrhhLblPesoTrabajoEquipo";
            this.RrhhLblPesoTrabajoEquipo.Size = new System.Drawing.Size(39, 18);
            this.RrhhLblPesoTrabajoEquipo.TabIndex = 1;
            this.RrhhLblPesoTrabajoEquipo.Text = "20%";
            // 
            // RrhhLblNotaTrabajoEquipo
            // 
            this.RrhhLblNotaTrabajoEquipo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblNotaTrabajoEquipo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblNotaTrabajoEquipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblNotaTrabajoEquipo.Location = new System.Drawing.Point(243, 9);
            this.RrhhLblNotaTrabajoEquipo.Name = "RrhhLblNotaTrabajoEquipo";
            this.RrhhLblNotaTrabajoEquipo.Size = new System.Drawing.Size(33, 19);
            this.RrhhLblNotaTrabajoEquipo.TabIndex = 2;
            this.RrhhLblNotaTrabajoEquipo.Text = "—";
            this.RrhhLblNotaTrabajoEquipo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhLblPonderadoTrabajoEquipo
            // 
            this.RrhhLblPonderadoTrabajoEquipo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPonderadoTrabajoEquipo.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPonderadoTrabajoEquipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPonderadoTrabajoEquipo.Location = new System.Drawing.Point(292, 9);
            this.RrhhLblPonderadoTrabajoEquipo.Name = "RrhhLblPonderadoTrabajoEquipo";
            this.RrhhLblPonderadoTrabajoEquipo.Size = new System.Drawing.Size(79, 19);
            this.RrhhLblPonderadoTrabajoEquipo.TabIndex = 3;
            this.RrhhLblPonderadoTrabajoEquipo.Text = "0.00";
            this.RrhhLblPonderadoTrabajoEquipo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhPnlFilaPuntualidad
            // 
            this.RrhhPnlFilaPuntualidad.BackColor = System.Drawing.Color.White;
            this.RrhhPnlFilaPuntualidad.Controls.Add(this.RrhhLblCriterioPuntualidad);
            this.RrhhPnlFilaPuntualidad.Controls.Add(this.RrhhLblPesoPuntualidad);
            this.RrhhPnlFilaPuntualidad.Controls.Add(this.RrhhLblNotaPuntualidad);
            this.RrhhPnlFilaPuntualidad.Controls.Add(this.RrhhLblPonderadoPuntualidad);
            this.RrhhPnlFilaPuntualidad.Location = new System.Drawing.Point(14, 169);
            this.RrhhPnlFilaPuntualidad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlFilaPuntualidad.Name = "RrhhPnlFilaPuntualidad";
            this.RrhhPnlFilaPuntualidad.Size = new System.Drawing.Size(385, 34);
            this.RrhhPnlFilaPuntualidad.TabIndex = 5;
            // 
            // RrhhLblCriterioPuntualidad
            // 
            this.RrhhLblCriterioPuntualidad.AutoSize = true;
            this.RrhhLblCriterioPuntualidad.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblCriterioPuntualidad.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblCriterioPuntualidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblCriterioPuntualidad.Location = new System.Drawing.Point(10, 9);
            this.RrhhLblCriterioPuntualidad.Name = "RrhhLblCriterioPuntualidad";
            this.RrhhLblCriterioPuntualidad.Size = new System.Drawing.Size(161, 18);
            this.RrhhLblCriterioPuntualidad.TabIndex = 0;
            this.RrhhLblCriterioPuntualidad.Text = "Puntualidad y asistencia";
            // 
            // RrhhLblPesoPuntualidad
            // 
            this.RrhhLblPesoPuntualidad.AutoSize = true;
            this.RrhhLblPesoPuntualidad.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPesoPuntualidad.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPesoPuntualidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPesoPuntualidad.Location = new System.Drawing.Point(196, 9);
            this.RrhhLblPesoPuntualidad.Name = "RrhhLblPesoPuntualidad";
            this.RrhhLblPesoPuntualidad.Size = new System.Drawing.Size(39, 18);
            this.RrhhLblPesoPuntualidad.TabIndex = 1;
            this.RrhhLblPesoPuntualidad.Text = "15%";
            // 
            // RrhhLblNotaPuntualidad
            // 
            this.RrhhLblNotaPuntualidad.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblNotaPuntualidad.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblNotaPuntualidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblNotaPuntualidad.Location = new System.Drawing.Point(243, 9);
            this.RrhhLblNotaPuntualidad.Name = "RrhhLblNotaPuntualidad";
            this.RrhhLblNotaPuntualidad.Size = new System.Drawing.Size(33, 19);
            this.RrhhLblNotaPuntualidad.TabIndex = 2;
            this.RrhhLblNotaPuntualidad.Text = "—";
            this.RrhhLblNotaPuntualidad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhLblPonderadoPuntualidad
            // 
            this.RrhhLblPonderadoPuntualidad.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPonderadoPuntualidad.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPonderadoPuntualidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPonderadoPuntualidad.Location = new System.Drawing.Point(292, 9);
            this.RrhhLblPonderadoPuntualidad.Name = "RrhhLblPonderadoPuntualidad";
            this.RrhhLblPonderadoPuntualidad.Size = new System.Drawing.Size(79, 19);
            this.RrhhLblPonderadoPuntualidad.TabIndex = 3;
            this.RrhhLblPonderadoPuntualidad.Text = "0.00";
            this.RrhhLblPonderadoPuntualidad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhPnlFilaIniciativa
            // 
            this.RrhhPnlFilaIniciativa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(244)))), ((int)(((byte)(243)))));
            this.RrhhPnlFilaIniciativa.Controls.Add(this.RrhhLblCriterioIniciativa);
            this.RrhhPnlFilaIniciativa.Controls.Add(this.RrhhLblPesoIniciativa);
            this.RrhhPnlFilaIniciativa.Controls.Add(this.RrhhLblNotaIniciativa);
            this.RrhhPnlFilaIniciativa.Controls.Add(this.RrhhLblPonderadoIniciativa);
            this.RrhhPnlFilaIniciativa.Location = new System.Drawing.Point(14, 202);
            this.RrhhPnlFilaIniciativa.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlFilaIniciativa.Name = "RrhhPnlFilaIniciativa";
            this.RrhhPnlFilaIniciativa.Size = new System.Drawing.Size(385, 34);
            this.RrhhPnlFilaIniciativa.TabIndex = 6;
            // 
            // RrhhLblCriterioIniciativa
            // 
            this.RrhhLblCriterioIniciativa.AutoSize = true;
            this.RrhhLblCriterioIniciativa.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblCriterioIniciativa.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblCriterioIniciativa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblCriterioIniciativa.Location = new System.Drawing.Point(10, 9);
            this.RrhhLblCriterioIniciativa.Name = "RrhhLblCriterioIniciativa";
            this.RrhhLblCriterioIniciativa.Size = new System.Drawing.Size(64, 18);
            this.RrhhLblCriterioIniciativa.TabIndex = 0;
            this.RrhhLblCriterioIniciativa.Text = "Iniciativa";
            // 
            // RrhhLblPesoIniciativa
            // 
            this.RrhhLblPesoIniciativa.AutoSize = true;
            this.RrhhLblPesoIniciativa.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPesoIniciativa.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPesoIniciativa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPesoIniciativa.Location = new System.Drawing.Point(196, 9);
            this.RrhhLblPesoIniciativa.Name = "RrhhLblPesoIniciativa";
            this.RrhhLblPesoIniciativa.Size = new System.Drawing.Size(39, 18);
            this.RrhhLblPesoIniciativa.TabIndex = 1;
            this.RrhhLblPesoIniciativa.Text = "15%";
            // 
            // RrhhLblNotaIniciativa
            // 
            this.RrhhLblNotaIniciativa.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblNotaIniciativa.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblNotaIniciativa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblNotaIniciativa.Location = new System.Drawing.Point(243, 9);
            this.RrhhLblNotaIniciativa.Name = "RrhhLblNotaIniciativa";
            this.RrhhLblNotaIniciativa.Size = new System.Drawing.Size(33, 19);
            this.RrhhLblNotaIniciativa.TabIndex = 2;
            this.RrhhLblNotaIniciativa.Text = "—";
            this.RrhhLblNotaIniciativa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhLblPonderadoIniciativa
            // 
            this.RrhhLblPonderadoIniciativa.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPonderadoIniciativa.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPonderadoIniciativa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPonderadoIniciativa.Location = new System.Drawing.Point(292, 9);
            this.RrhhLblPonderadoIniciativa.Name = "RrhhLblPonderadoIniciativa";
            this.RrhhLblPonderadoIniciativa.Size = new System.Drawing.Size(79, 19);
            this.RrhhLblPonderadoIniciativa.TabIndex = 3;
            this.RrhhLblPonderadoIniciativa.Text = "0.00";
            this.RrhhLblPonderadoIniciativa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RrhhPnlTotalCriterios
            // 
            this.RrhhPnlTotalCriterios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(244)))));
            this.RrhhPnlTotalCriterios.Controls.Add(this.RrhhLblTotalCriteriosTexto);
            this.RrhhPnlTotalCriterios.Controls.Add(this.RrhhLblTotalCriteriosValor);
            this.RrhhPnlTotalCriterios.Location = new System.Drawing.Point(14, 239);
            this.RrhhPnlTotalCriterios.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlTotalCriterios.Name = "RrhhPnlTotalCriterios";
            this.RrhhPnlTotalCriterios.Size = new System.Drawing.Size(385, 35);
            this.RrhhPnlTotalCriterios.TabIndex = 7;
            // 
            // RrhhLblTotalCriteriosTexto
            // 
            this.RrhhLblTotalCriteriosTexto.AutoSize = true;
            this.RrhhLblTotalCriteriosTexto.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblTotalCriteriosTexto.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblTotalCriteriosTexto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblTotalCriteriosTexto.Location = new System.Drawing.Point(10, 9);
            this.RrhhLblTotalCriteriosTexto.Name = "RrhhLblTotalCriteriosTexto";
            this.RrhhLblTotalCriteriosTexto.Size = new System.Drawing.Size(112, 18);
            this.RrhhLblTotalCriteriosTexto.TabIndex = 0;
            this.RrhhLblTotalCriteriosTexto.Text = "Total (100%)";
            // 
            // RrhhLblTotalCriteriosValor
            // 
            this.RrhhLblTotalCriteriosValor.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblTotalCriteriosValor.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblTotalCriteriosValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblTotalCriteriosValor.Location = new System.Drawing.Point(250, 8);
            this.RrhhLblTotalCriteriosValor.Name = "RrhhLblTotalCriteriosValor";
            this.RrhhLblTotalCriteriosValor.Size = new System.Drawing.Size(125, 20);
            this.RrhhLblTotalCriteriosValor.TabIndex = 1;
            this.RrhhLblTotalCriteriosValor.Text = "0.00 / 5.00";
            this.RrhhLblTotalCriteriosValor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RrhhLblLeyendaCriterios
            // 
            this.RrhhLblLeyendaCriterios.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblLeyendaCriterios.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblLeyendaCriterios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(131)))), ((int)(((byte)(133)))));
            this.RrhhLblLeyendaCriterios.Location = new System.Drawing.Point(14, 278);
            this.RrhhLblLeyendaCriterios.Name = "RrhhLblLeyendaCriterios";
            this.RrhhLblLeyendaCriterios.Size = new System.Drawing.Size(403, 52);
            this.RrhhLblLeyendaCriterios.TabIndex = 8;
            this.RrhhLblLeyendaCriterios.Text = "1 = Deficiente · 2 = Regular · 3 = Cumple\r\n4 = Bueno · 5 = Excelente";
            // 
            // RrhhPnlRetroalimentacion
            // 
            this.RrhhPnlRetroalimentacion.BackColor = System.Drawing.Color.White;
            this.RrhhPnlRetroalimentacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhPnlRetroalimentacion.Controls.Add(this.RrhhPnlEncabezadoRetroalimentacion);
            this.RrhhPnlRetroalimentacion.Controls.Add(this.RrhhLblPlanMejora);
            this.RrhhPnlRetroalimentacion.Controls.Add(this.RrhhTxtPlanMejora);
            this.RrhhPnlRetroalimentacion.Location = new System.Drawing.Point(939, 295);
            this.RrhhPnlRetroalimentacion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlRetroalimentacion.Name = "RrhhPnlRetroalimentacion";
            this.RrhhPnlRetroalimentacion.Size = new System.Drawing.Size(454, 362);
            this.RrhhPnlRetroalimentacion.TabIndex = 4;
            // 
            // RrhhPnlEncabezadoRetroalimentacion
            // 
            this.RrhhPnlEncabezadoRetroalimentacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(238)))), ((int)(((byte)(240)))));
            this.RrhhPnlEncabezadoRetroalimentacion.Controls.Add(this.RrhhLblTituloRetroalimentacion);
            this.RrhhPnlEncabezadoRetroalimentacion.Location = new System.Drawing.Point(0, 0);
            this.RrhhPnlEncabezadoRetroalimentacion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlEncabezadoRetroalimentacion.Name = "RrhhPnlEncabezadoRetroalimentacion";
            this.RrhhPnlEncabezadoRetroalimentacion.Size = new System.Drawing.Size(452, 28);
            this.RrhhPnlEncabezadoRetroalimentacion.TabIndex = 0;
            // 
            // RrhhLblTituloRetroalimentacion
            // 
            this.RrhhLblTituloRetroalimentacion.AutoSize = true;
            this.RrhhLblTituloRetroalimentacion.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblTituloRetroalimentacion.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblTituloRetroalimentacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblTituloRetroalimentacion.Location = new System.Drawing.Point(14, 5);
            this.RrhhLblTituloRetroalimentacion.Name = "RrhhLblTituloRetroalimentacion";
            this.RrhhLblTituloRetroalimentacion.Size = new System.Drawing.Size(119, 18);
            this.RrhhLblTituloRetroalimentacion.TabIndex = 0;
            this.RrhhLblTituloRetroalimentacion.Text = "Plan de Mejora";
            // 
            // RrhhLblPlanMejora
            // 
            this.RrhhLblPlanMejora.AutoSize = true;
            this.RrhhLblPlanMejora.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblPlanMejora.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblPlanMejora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhLblPlanMejora.Location = new System.Drawing.Point(16, 42);
            this.RrhhLblPlanMejora.Name = "RrhhLblPlanMejora";
            this.RrhhLblPlanMejora.Size = new System.Drawing.Size(173, 18);
            this.RrhhLblPlanMejora.TabIndex = 3;
            this.RrhhLblPlanMejora.Text = "Plan de mejora asignado:";
            // 
            // RrhhTxtPlanMejora
            // 
            this.RrhhTxtPlanMejora.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.RrhhTxtPlanMejora.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhTxtPlanMejora.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhTxtPlanMejora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.RrhhTxtPlanMejora.Location = new System.Drawing.Point(16, 68);
            this.RrhhTxtPlanMejora.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhTxtPlanMejora.Multiline = true;
            this.RrhhTxtPlanMejora.Name = "RrhhTxtPlanMejora";
            this.RrhhTxtPlanMejora.ReadOnly = true;
            this.RrhhTxtPlanMejora.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RrhhTxtPlanMejora.Size = new System.Drawing.Size(416, 150);
            this.RrhhTxtPlanMejora.TabIndex = 4;
            this.RrhhTxtPlanMejora.TabStop = false;
            // 
            // RrhhPnlHistorial
            // 
            this.RrhhPnlHistorial.BackColor = System.Drawing.Color.White;
            this.RrhhPnlHistorial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RrhhPnlHistorial.Controls.Add(this.RrhhPnlEncabezadoHistorial);
            this.RrhhPnlHistorial.Controls.Add(this.RrhhDgvHistorial);
            this.RrhhPnlHistorial.Location = new System.Drawing.Point(10, 665);
            this.RrhhPnlHistorial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlHistorial.Name = "RrhhPnlHistorial";
            this.RrhhPnlHistorial.Size = new System.Drawing.Size(1384, 254);
            this.RrhhPnlHistorial.TabIndex = 5;
            // 
            // RrhhPnlEncabezadoHistorial
            // 
            this.RrhhPnlEncabezadoHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(238)))), ((int)(((byte)(240)))));
            this.RrhhPnlEncabezadoHistorial.Controls.Add(this.RrhhLblTituloHistorial);
            this.RrhhPnlEncabezadoHistorial.Location = new System.Drawing.Point(0, 0);
            this.RrhhPnlEncabezadoHistorial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlEncabezadoHistorial.Name = "RrhhPnlEncabezadoHistorial";
            this.RrhhPnlEncabezadoHistorial.Size = new System.Drawing.Size(1382, 28);
            this.RrhhPnlEncabezadoHistorial.TabIndex = 0;
            // 
            // RrhhLblTituloHistorial
            // 
            this.RrhhLblTituloHistorial.AutoSize = true;
            this.RrhhLblTituloHistorial.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblTituloHistorial.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblTituloHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblTituloHistorial.Location = new System.Drawing.Point(14, 5);
            this.RrhhLblTituloHistorial.Name = "RrhhLblTituloHistorial";
            this.RrhhLblTituloHistorial.Size = new System.Drawing.Size(227, 18);
            this.RrhhLblTituloHistorial.TabIndex = 0;
            this.RrhhLblTituloHistorial.Text = "Historial de Mis Evaluaciones";
            // 
            // RrhhDgvHistorial
            // 
            this.RrhhDgvHistorial.AllowUserToAddRows = false;
            this.RrhhDgvHistorial.AllowUserToDeleteRows = false;
            this.RrhhDgvHistorial.AllowUserToResizeColumns = false;
            this.RrhhDgvHistorial.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.RrhhDgvHistorial.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.RrhhDgvHistorial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.RrhhDgvHistorial.BackgroundColor = System.Drawing.Color.White;
            this.RrhhDgvHistorial.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RrhhDgvHistorial.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.RrhhDgvHistorial.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.RrhhDgvHistorial.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.RrhhDgvHistorial.ColumnHeadersHeight = 28;
            this.RrhhDgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.RrhhDgvHistorial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RrhhColHistorialEvaluador,
            this.RrhhColHistorialFecha,
            this.RrhhColHistorialPuntaje,
            this.RrhhColHistorialNivel,
            this.RrhhColHistorialPorcentaje});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(244)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.RrhhDgvHistorial.DefaultCellStyle = dataGridViewCellStyle6;
            this.RrhhDgvHistorial.EnableHeadersVisualStyles = false;
            this.RrhhDgvHistorial.Location = new System.Drawing.Point(10, 35);
            this.RrhhDgvHistorial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhDgvHistorial.MultiSelect = false;
            this.RrhhDgvHistorial.Name = "RrhhDgvHistorial";
            this.RrhhDgvHistorial.ReadOnly = true;
            this.RrhhDgvHistorial.RowHeadersVisible = false;
            this.RrhhDgvHistorial.RowHeadersWidth = 26;
            this.RrhhDgvHistorial.RowTemplate.Height = 12;
            this.RrhhDgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.RrhhDgvHistorial.ShowCellToolTips = false;
            this.RrhhDgvHistorial.Size = new System.Drawing.Size(1361, 210);
            this.RrhhDgvHistorial.TabIndex = 1;
            // 
            // RrhhColHistorialEvaluador
            // 
            this.RrhhColHistorialEvaluador.FillWeight = 163F;
            this.RrhhColHistorialEvaluador.HeaderText = "Evaluador";
            this.RrhhColHistorialEvaluador.MinimumWidth = 3;
            this.RrhhColHistorialEvaluador.Name = "RrhhColHistorialEvaluador";
            this.RrhhColHistorialEvaluador.ReadOnly = true;
            // 
            // RrhhColHistorialFecha
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RrhhColHistorialFecha.DefaultCellStyle = dataGridViewCellStyle3;
            this.RrhhColHistorialFecha.FillWeight = 67F;
            this.RrhhColHistorialFecha.HeaderText = "Fecha";
            this.RrhhColHistorialFecha.MinimumWidth = 3;
            this.RrhhColHistorialFecha.Name = "RrhhColHistorialFecha";
            this.RrhhColHistorialFecha.ReadOnly = true;
            // 
            // RrhhColHistorialPuntaje
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RrhhColHistorialPuntaje.DefaultCellStyle = dataGridViewCellStyle4;
            this.RrhhColHistorialPuntaje.FillWeight = 64F;
            this.RrhhColHistorialPuntaje.HeaderText = "Puntaje";
            this.RrhhColHistorialPuntaje.MinimumWidth = 3;
            this.RrhhColHistorialPuntaje.Name = "RrhhColHistorialPuntaje";
            this.RrhhColHistorialPuntaje.ReadOnly = true;
            // 
            // RrhhColHistorialNivel
            // 
            this.RrhhColHistorialNivel.FillWeight = 178F;
            this.RrhhColHistorialNivel.HeaderText = "Nivel";
            this.RrhhColHistorialNivel.MinimumWidth = 3;
            this.RrhhColHistorialNivel.Name = "RrhhColHistorialNivel";
            this.RrhhColHistorialNivel.ReadOnly = true;
            // 
            // RrhhColHistorialPorcentaje
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RrhhColHistorialPorcentaje.DefaultCellStyle = dataGridViewCellStyle5;
            this.RrhhColHistorialPorcentaje.FillWeight = 249F;
            this.RrhhColHistorialPorcentaje.HeaderText = "Porcentaje";
            this.RrhhColHistorialPorcentaje.MinimumWidth = 3;
            this.RrhhColHistorialPorcentaje.Name = "RrhhColHistorialPorcentaje";
            this.RrhhColHistorialPorcentaje.ReadOnly = true;
            // 
            // RrhhPnlBotones
            // 
            this.RrhhPnlBotones.BackColor = System.Drawing.Color.Transparent;
            this.RrhhPnlBotones.Controls.Add(this.RrhhBtnRefrescar);
            this.RrhhPnlBotones.Controls.Add(this.RrhhBtnImprimir);
            this.RrhhPnlBotones.Controls.Add(this.RrhhBtnInicio);
            this.RrhhPnlBotones.Controls.Add(this.RrhhBtnAnterior);
            this.RrhhPnlBotones.Controls.Add(this.RrhhBtnSiguiente);
            this.RrhhPnlBotones.Controls.Add(this.RrhhBtnFin);
            this.RrhhPnlBotones.Controls.Add(this.RrhhBtnAyuda);
            this.RrhhPnlBotones.Controls.Add(this.RrhhBtnSalir);
            this.RrhhPnlBotones.Location = new System.Drawing.Point(10, 928);
            this.RrhhPnlBotones.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlBotones.Name = "RrhhPnlBotones";
            this.RrhhPnlBotones.Size = new System.Drawing.Size(1384, 85);
            this.RrhhPnlBotones.TabIndex = 20;
            // 
            // RrhhBtnRefrescar
            // 
            this.RrhhBtnRefrescar.BackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnRefrescar.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_refrescar;
            this.RrhhBtnRefrescar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RrhhBtnRefrescar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RrhhBtnRefrescar.FlatAppearance.BorderSize = 0;
            this.RrhhBtnRefrescar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnRefrescar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhBtnRefrescar.Location = new System.Drawing.Point(3, 1);
            this.RrhhBtnRefrescar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhBtnRefrescar.Name = "RrhhBtnRefrescar";
            this.RrhhBtnRefrescar.Size = new System.Drawing.Size(74, 82);
            this.RrhhBtnRefrescar.TabIndex = 0;
            this.RrhhBtnRefrescar.UseVisualStyleBackColor = false;
            // 
            // RrhhBtnImprimir
            // 
            this.RrhhBtnImprimir.BackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnImprimir.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_imprimir;
            this.RrhhBtnImprimir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RrhhBtnImprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RrhhBtnImprimir.FlatAppearance.BorderSize = 0;
            this.RrhhBtnImprimir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnImprimir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhBtnImprimir.Location = new System.Drawing.Point(87, 1);
            this.RrhhBtnImprimir.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhBtnImprimir.Name = "RrhhBtnImprimir";
            this.RrhhBtnImprimir.Size = new System.Drawing.Size(74, 82);
            this.RrhhBtnImprimir.TabIndex = 1;
            this.RrhhBtnImprimir.UseVisualStyleBackColor = false;
            // 
            // RrhhBtnInicio
            // 
            this.RrhhBtnInicio.BackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnInicio.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_inicio;
            this.RrhhBtnInicio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RrhhBtnInicio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RrhhBtnInicio.FlatAppearance.BorderSize = 0;
            this.RrhhBtnInicio.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnInicio.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnInicio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhBtnInicio.Location = new System.Drawing.Point(170, 1);
            this.RrhhBtnInicio.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhBtnInicio.Name = "RrhhBtnInicio";
            this.RrhhBtnInicio.Size = new System.Drawing.Size(74, 82);
            this.RrhhBtnInicio.TabIndex = 3;
            this.RrhhBtnInicio.UseVisualStyleBackColor = false;
            // 
            // RrhhBtnAnterior
            // 
            this.RrhhBtnAnterior.BackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnAnterior.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_anterior;
            this.RrhhBtnAnterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RrhhBtnAnterior.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RrhhBtnAnterior.FlatAppearance.BorderSize = 0;
            this.RrhhBtnAnterior.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnAnterior.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhBtnAnterior.Location = new System.Drawing.Point(253, 1);
            this.RrhhBtnAnterior.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhBtnAnterior.Name = "RrhhBtnAnterior";
            this.RrhhBtnAnterior.Size = new System.Drawing.Size(74, 82);
            this.RrhhBtnAnterior.TabIndex = 4;
            this.RrhhBtnAnterior.UseVisualStyleBackColor = false;
            // 
            // RrhhBtnSiguiente
            // 
            this.RrhhBtnSiguiente.BackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnSiguiente.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_siguiente;
            this.RrhhBtnSiguiente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RrhhBtnSiguiente.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RrhhBtnSiguiente.FlatAppearance.BorderSize = 0;
            this.RrhhBtnSiguiente.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnSiguiente.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhBtnSiguiente.Location = new System.Drawing.Point(336, 1);
            this.RrhhBtnSiguiente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhBtnSiguiente.Name = "RrhhBtnSiguiente";
            this.RrhhBtnSiguiente.Size = new System.Drawing.Size(74, 82);
            this.RrhhBtnSiguiente.TabIndex = 5;
            this.RrhhBtnSiguiente.UseVisualStyleBackColor = false;
            // 
            // RrhhBtnFin
            // 
            this.RrhhBtnFin.BackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnFin.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_fin;
            this.RrhhBtnFin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RrhhBtnFin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RrhhBtnFin.FlatAppearance.BorderSize = 0;
            this.RrhhBtnFin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnFin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnFin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhBtnFin.Location = new System.Drawing.Point(420, 1);
            this.RrhhBtnFin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhBtnFin.Name = "RrhhBtnFin";
            this.RrhhBtnFin.Size = new System.Drawing.Size(74, 82);
            this.RrhhBtnFin.TabIndex = 6;
            this.RrhhBtnFin.UseVisualStyleBackColor = false;
            // 
            // RrhhBtnAyuda
            // 
            this.RrhhBtnAyuda.BackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnAyuda.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_ayuda;
            this.RrhhBtnAyuda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RrhhBtnAyuda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RrhhBtnAyuda.FlatAppearance.BorderSize = 0;
            this.RrhhBtnAyuda.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnAyuda.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhBtnAyuda.Location = new System.Drawing.Point(503, 1);
            this.RrhhBtnAyuda.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhBtnAyuda.Name = "RrhhBtnAyuda";
            this.RrhhBtnAyuda.Size = new System.Drawing.Size(74, 82);
            this.RrhhBtnAyuda.TabIndex = 7;
            this.RrhhBtnAyuda.UseVisualStyleBackColor = false;
            // 
            // RrhhBtnSalir
            // 
            this.RrhhBtnSalir.BackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnSalir.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_salir;
            this.RrhhBtnSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RrhhBtnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RrhhBtnSalir.FlatAppearance.BorderSize = 0;
            this.RrhhBtnSalir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RrhhBtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RrhhBtnSalir.Location = new System.Drawing.Point(586, 1);
            this.RrhhBtnSalir.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhBtnSalir.Name = "RrhhBtnSalir";
            this.RrhhBtnSalir.Size = new System.Drawing.Size(74, 82);
            this.RrhhBtnSalir.TabIndex = 8;
            this.RrhhBtnSalir.UseVisualStyleBackColor = false;
            this.RrhhBtnSalir.Click += new System.EventHandler(this.RrhhBtnSalir_Click);
            // 
            // RrhhPnlEncabezado
            // 
            this.RrhhPnlEncabezado.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.Pnl_fondo;
            this.RrhhPnlEncabezado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RrhhPnlEncabezado.Controls.Add(this.RrhhLblTitulo);
            this.RrhhPnlEncabezado.Controls.Add(this.RrhhLblSubtitulo);
            this.RrhhPnlEncabezado.Controls.Add(this.RrhhPbIconoVerEvaluaciones);
            this.RrhhPnlEncabezado.Controls.Add(this.RrhhPbMascota);
            this.RrhhPnlEncabezado.Location = new System.Drawing.Point(10, 11);
            this.RrhhPnlEncabezado.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPnlEncabezado.Name = "RrhhPnlEncabezado";
            this.RrhhPnlEncabezado.Size = new System.Drawing.Size(1384, 199);
            this.RrhhPnlEncabezado.TabIndex = 0;
            // 
            // RrhhLblTitulo
            // 
            this.RrhhLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblTitulo.Font = new System.Drawing.Font("Lucida Bright", 21.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblTitulo.Location = new System.Drawing.Point(158, 20);
            this.RrhhLblTitulo.Name = "RrhhLblTitulo";
            this.RrhhLblTitulo.Size = new System.Drawing.Size(739, 86);
            this.RrhhLblTitulo.TabIndex = 4;
            this.RrhhLblTitulo.Text = "Ver Evaluaciones";
            // 
            // RrhhLblSubtitulo
            // 
            this.RrhhLblSubtitulo.BackColor = System.Drawing.Color.Transparent;
            this.RrhhLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 11.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrhhLblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.RrhhLblSubtitulo.Location = new System.Drawing.Point(159, 108);
            this.RrhhLblSubtitulo.Name = "RrhhLblSubtitulo";
            this.RrhhLblSubtitulo.Size = new System.Drawing.Size(720, 45);
            this.RrhhLblSubtitulo.TabIndex = 5;
            this.RrhhLblSubtitulo.Text = "Consulta tus resultados, puntajes y retroalimentación de desempeño.";
            // 
            // RrhhPbIconoVerEvaluaciones
            // 
            this.RrhhPbIconoVerEvaluaciones.BackColor = System.Drawing.Color.Transparent;
            this.RrhhPbIconoVerEvaluaciones.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.btn_resultados;
            this.RrhhPbIconoVerEvaluaciones.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RrhhPbIconoVerEvaluaciones.Location = new System.Drawing.Point(14, 14);
            this.RrhhPbIconoVerEvaluaciones.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPbIconoVerEvaluaciones.Name = "RrhhPbIconoVerEvaluaciones";
            this.RrhhPbIconoVerEvaluaciones.Size = new System.Drawing.Size(130, 138);
            this.RrhhPbIconoVerEvaluaciones.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RrhhPbIconoVerEvaluaciones.TabIndex = 0;
            this.RrhhPbIconoVerEvaluaciones.TabStop = false;
            // 
            // RrhhPbMascota
            // 
            this.RrhhPbMascota.BackColor = System.Drawing.Color.Transparent;
            this.RrhhPbMascota.BackgroundImage = global::CapaVista_EvaluacionDesempeño.Properties.Resources.mascota;
            this.RrhhPbMascota.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RrhhPbMascota.Location = new System.Drawing.Point(1078, 20);
            this.RrhhPbMascota.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RrhhPbMascota.Name = "RrhhPbMascota";
            this.RrhhPbMascota.Size = new System.Drawing.Size(125, 161);
            this.RrhhPbMascota.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RrhhPbMascota.TabIndex = 3;
            this.RrhhPbMascota.TabStop = false;
            // 
            // FrmVerEvaluaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1404, 1020);
            this.Controls.Add(this.RrhhPnlBotones);
            this.Controls.Add(this.RrhhPnlEncabezado);
            this.Controls.Add(this.RrhhPnlTituloResultados);
            this.Controls.Add(this.RrhhPnlResumen);
            this.Controls.Add(this.RrhhPnlCriterios);
            this.Controls.Add(this.RrhhPnlRetroalimentacion);
            this.Controls.Add(this.RrhhPnlHistorial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FrmVerEvaluaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9404 – VerEvaluaciones";
            this.RrhhPnlTituloResultados.ResumeLayout(false);
            this.RrhhPnlTituloResultados.PerformLayout();
            this.RrhhPnlResumen.ResumeLayout(false);
            this.RrhhPnlResumen.PerformLayout();
            this.RrhhPnlEncabezadoResumen.ResumeLayout(false);
            this.RrhhPnlEncabezadoResumen.PerformLayout();
            this.RrhhPnlPuntajePista.ResumeLayout(false);
            this.RrhhPnlCriterios.ResumeLayout(false);
            this.RrhhPnlEncabezadoCriterios.ResumeLayout(false);
            this.RrhhPnlEncabezadoCriterios.PerformLayout();
            this.RrhhPnlEncabezadoTablaCriterios.ResumeLayout(false);
            this.RrhhPnlEncabezadoTablaCriterios.PerformLayout();
            this.RrhhPnlFilaCalidadTrabajo.ResumeLayout(false);
            this.RrhhPnlFilaCalidadTrabajo.PerformLayout();
            this.RrhhPnlFilaProductividad.ResumeLayout(false);
            this.RrhhPnlFilaProductividad.PerformLayout();
            this.RrhhPnlFilaTrabajoEquipo.ResumeLayout(false);
            this.RrhhPnlFilaTrabajoEquipo.PerformLayout();
            this.RrhhPnlFilaPuntualidad.ResumeLayout(false);
            this.RrhhPnlFilaPuntualidad.PerformLayout();
            this.RrhhPnlFilaIniciativa.ResumeLayout(false);
            this.RrhhPnlFilaIniciativa.PerformLayout();
            this.RrhhPnlTotalCriterios.ResumeLayout(false);
            this.RrhhPnlTotalCriterios.PerformLayout();
            this.RrhhPnlRetroalimentacion.ResumeLayout(false);
            this.RrhhPnlRetroalimentacion.PerformLayout();
            this.RrhhPnlEncabezadoRetroalimentacion.ResumeLayout(false);
            this.RrhhPnlEncabezadoRetroalimentacion.PerformLayout();
            this.RrhhPnlHistorial.ResumeLayout(false);
            this.RrhhPnlEncabezadoHistorial.ResumeLayout(false);
            this.RrhhPnlEncabezadoHistorial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RrhhDgvHistorial)).EndInit();
            this.RrhhPnlBotones.ResumeLayout(false);
            this.RrhhPnlEncabezado.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RrhhPbIconoVerEvaluaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RrhhPbMascota)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel RrhhPnlEncabezado;
        private System.Windows.Forms.PictureBox RrhhPbIconoVerEvaluaciones;
        private System.Windows.Forms.PictureBox RrhhPbMascota;
        private System.Windows.Forms.Panel RrhhPnlTituloResultados;
        private System.Windows.Forms.Label RrhhLblTituloResultados;
        private System.Windows.Forms.Panel RrhhPnlResumen;
        private System.Windows.Forms.Panel RrhhPnlEncabezadoResumen;
        private System.Windows.Forms.Label RrhhLblTituloResumen;
        private System.Windows.Forms.Label RrhhLblResumenCiclo;
        private System.Windows.Forms.ComboBox RrhhCboResumenCiclo;
        private System.Windows.Forms.Label RrhhLblResumenColaborador;
        private System.Windows.Forms.TextBox RrhhTxtResumenColaborador;
        private System.Windows.Forms.Label RrhhLblResumenPuesto;
        private System.Windows.Forms.TextBox RrhhTxtResumenPuesto;
        private System.Windows.Forms.Label RrhhLblResumenEvaluador;
        private System.Windows.Forms.TextBox RrhhTxtResumenEvaluador;
        private System.Windows.Forms.Label RrhhLblResumenFecha;
        private System.Windows.Forms.TextBox RrhhTxtResumenFecha;
        private System.Windows.Forms.Label RrhhLblPuntajeFinal;
        private System.Windows.Forms.Label RrhhLblPuntajeFinalValor;
        private System.Windows.Forms.Panel RrhhPnlPuntajePista;
        private System.Windows.Forms.Panel RrhhPnlPuntajeRelleno;
        private System.Windows.Forms.Label RrhhLblNivelResumen;
        private System.Windows.Forms.Panel RrhhPnlCriterios;
        private System.Windows.Forms.Panel RrhhPnlEncabezadoCriterios;
        private System.Windows.Forms.Label RrhhLblTituloCriterios;
        private System.Windows.Forms.Panel RrhhPnlEncabezadoTablaCriterios;
        private System.Windows.Forms.Label RrhhLblColumnaCriterio;
        private System.Windows.Forms.Label RrhhLblColumnaPeso;
        private System.Windows.Forms.Label RrhhLblColumnaNota;
        private System.Windows.Forms.Label RrhhLblColumnaPonderado;
        private System.Windows.Forms.Panel RrhhPnlFilaCalidadTrabajo;
        private System.Windows.Forms.Label RrhhLblCriterioCalidadTrabajo;
        private System.Windows.Forms.Label RrhhLblPesoCalidadTrabajo;
        private System.Windows.Forms.Label RrhhLblNotaCalidadTrabajo;
        private System.Windows.Forms.Label RrhhLblPonderadoCalidadTrabajo;
        private System.Windows.Forms.Panel RrhhPnlFilaProductividad;
        private System.Windows.Forms.Label RrhhLblCriterioProductividad;
        private System.Windows.Forms.Label RrhhLblPesoProductividad;
        private System.Windows.Forms.Label RrhhLblNotaProductividad;
        private System.Windows.Forms.Label RrhhLblPonderadoProductividad;
        private System.Windows.Forms.Panel RrhhPnlFilaTrabajoEquipo;
        private System.Windows.Forms.Label RrhhLblCriterioTrabajoEquipo;
        private System.Windows.Forms.Label RrhhLblPesoTrabajoEquipo;
        private System.Windows.Forms.Label RrhhLblNotaTrabajoEquipo;
        private System.Windows.Forms.Label RrhhLblPonderadoTrabajoEquipo;
        private System.Windows.Forms.Panel RrhhPnlFilaPuntualidad;
        private System.Windows.Forms.Label RrhhLblCriterioPuntualidad;
        private System.Windows.Forms.Label RrhhLblPesoPuntualidad;
        private System.Windows.Forms.Label RrhhLblNotaPuntualidad;
        private System.Windows.Forms.Label RrhhLblPonderadoPuntualidad;
        private System.Windows.Forms.Panel RrhhPnlFilaIniciativa;
        private System.Windows.Forms.Label RrhhLblCriterioIniciativa;
        private System.Windows.Forms.Label RrhhLblPesoIniciativa;
        private System.Windows.Forms.Label RrhhLblNotaIniciativa;
        private System.Windows.Forms.Label RrhhLblPonderadoIniciativa;
        private System.Windows.Forms.Panel RrhhPnlTotalCriterios;
        private System.Windows.Forms.Label RrhhLblTotalCriteriosTexto;
        private System.Windows.Forms.Label RrhhLblTotalCriteriosValor;
        private System.Windows.Forms.Label RrhhLblLeyendaCriterios;
        private System.Windows.Forms.Panel RrhhPnlRetroalimentacion;
        private System.Windows.Forms.Panel RrhhPnlEncabezadoRetroalimentacion;
        private System.Windows.Forms.Label RrhhLblTituloRetroalimentacion;
        private System.Windows.Forms.Label RrhhLblPlanMejora;
        private System.Windows.Forms.TextBox RrhhTxtPlanMejora;
        private System.Windows.Forms.Panel RrhhPnlHistorial;
        private System.Windows.Forms.Panel RrhhPnlEncabezadoHistorial;
        private System.Windows.Forms.Label RrhhLblTituloHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn RrhhColHistorialEvaluador;
        private System.Windows.Forms.DataGridViewTextBoxColumn RrhhColHistorialFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn RrhhColHistorialPuntaje;
        private System.Windows.Forms.DataGridViewTextBoxColumn RrhhColHistorialNivel;
        private System.Windows.Forms.DataGridViewTextBoxColumn RrhhColHistorialPorcentaje;
        private System.Windows.Forms.DataGridView RrhhDgvHistorial;
        private System.Windows.Forms.Label RrhhLblTitulo;
        private System.Windows.Forms.Label RrhhLblSubtitulo;
        private System.Windows.Forms.Panel RrhhPnlBotones;
        private System.Windows.Forms.Button RrhhBtnRefrescar;
        private System.Windows.Forms.Button RrhhBtnImprimir;
        private System.Windows.Forms.Button RrhhBtnInicio;
        private System.Windows.Forms.Button RrhhBtnAnterior;
        private System.Windows.Forms.Button RrhhBtnSiguiente;
        private System.Windows.Forms.Button RrhhBtnFin;
        private System.Windows.Forms.Button RrhhBtnAyuda;
        private System.Windows.Forms.Button RrhhBtnSalir;
    }
}
