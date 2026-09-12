using System.Windows.Forms;
using System.Drawing;


namespace CapaVista_Navegador
{
    partial class Frm_Crud
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Crud));
            this.btnIngresar = new System.Windows.Forms.Button();
            this.Imgn_list1 = new System.Windows.Forms.ImageList(this.components);
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnInicio = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnFin = new System.Windows.Forms.Button();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnAyuda = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnIngresar
            // 
            this.btnIngresar.ImageIndex = 0;
            this.btnIngresar.ImageList = this.Imgn_list1;
            this.btnIngresar.Location = new System.Drawing.Point(12, 24);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(101, 81);
            this.btnIngresar.TabIndex = 0;
            this.btnIngresar.UseVisualStyleBackColor = true;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // Imgn_list1
            // 
            this.Imgn_list1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Imgn_list1.ImageStream")));
            this.Imgn_list1.TransparentColor = System.Drawing.Color.Transparent;
            this.Imgn_list1.Images.SetKeyName(0, "ingresar.png");
            this.Imgn_list1.Images.SetKeyName(1, "cancelar.png");
            this.Imgn_list1.Images.SetKeyName(2, "consultar.png");
            this.Imgn_list1.Images.SetKeyName(3, "eliminar.png");
            this.Imgn_list1.Images.SetKeyName(4, "Fin.png");
            this.Imgn_list1.Images.SetKeyName(5, "Guardar.png");
            this.Imgn_list1.Images.SetKeyName(6, "icono aterior.png");
            this.Imgn_list1.Images.SetKeyName(7, "imprimir.png");
            this.Imgn_list1.Images.SetKeyName(8, "inicio.png");
            this.Imgn_list1.Images.SetKeyName(9, "modificar.png");
            this.Imgn_list1.Images.SetKeyName(10, "refrescar.png");
            this.Imgn_list1.Images.SetKeyName(11, "salir.png");
            this.Imgn_list1.Images.SetKeyName(12, "siguiente.png");
            this.Imgn_list1.Images.SetKeyName(13, "Fin.png");
            this.Imgn_list1.Images.SetKeyName(14, "ayuda.png");
            // 
            // btnCancelar
            // 
            this.btnCancelar.ImageIndex = 1;
            this.btnCancelar.ImageList = this.Imgn_list1;
            this.btnCancelar.Location = new System.Drawing.Point(119, 24);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(101, 81);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnModificar
            // 
            this.btnModificar.ImageIndex = 9;
            this.btnModificar.ImageList = this.Imgn_list1;
            this.btnModificar.Location = new System.Drawing.Point(547, 24);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(101, 81);
            this.btnModificar.TabIndex = 2;
            this.btnModificar.UseVisualStyleBackColor = true;
            // 
            // btnImprimir
            // 
            this.btnImprimir.ImageIndex = 7;
            this.btnImprimir.ImageList = this.Imgn_list1;
            this.btnImprimir.Location = new System.Drawing.Point(333, 111);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(101, 81);
            this.btnImprimir.TabIndex = 3;
            this.btnImprimir.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.ImageIndex = 5;
            this.btnGuardar.ImageList = this.Imgn_list1;
            this.btnGuardar.Location = new System.Drawing.Point(440, 111);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(101, 81);
            this.btnGuardar.TabIndex = 4;
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.ImageIndex = 13;
            this.btnSiguiente.ImageList = this.Imgn_list1;
            this.btnSiguiente.Location = new System.Drawing.Point(868, 24);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(101, 81);
            this.btnSiguiente.TabIndex = 5;
            this.btnSiguiente.UseVisualStyleBackColor = true;
            // 
            // btnAnterior
            // 
            this.btnAnterior.ImageIndex = 8;
            this.btnAnterior.ImageList = this.Imgn_list1;
            this.btnAnterior.Location = new System.Drawing.Point(761, 24);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(101, 81);
            this.btnAnterior.TabIndex = 6;
            this.btnAnterior.UseVisualStyleBackColor = true;
            // 
            // btnInicio
            // 
            this.btnInicio.ImageIndex = 6;
            this.btnInicio.ImageList = this.Imgn_list1;
            this.btnInicio.Location = new System.Drawing.Point(654, 24);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Size = new System.Drawing.Size(101, 81);
            this.btnInicio.TabIndex = 7;
            this.btnInicio.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.ImageIndex = 3;
            this.btnEliminar.ImageList = this.Imgn_list1;
            this.btnEliminar.Location = new System.Drawing.Point(333, 24);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(101, 81);
            this.btnEliminar.TabIndex = 8;
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnConsultar
            // 
            this.btnConsultar.ImageIndex = 2;
            this.btnConsultar.ImageList = this.Imgn_list1;
            this.btnConsultar.Location = new System.Drawing.Point(226, 24);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(101, 81);
            this.btnConsultar.TabIndex = 9;
            this.btnConsultar.UseVisualStyleBackColor = true;
            // 
            // btnSalir
            // 
            this.btnSalir.ImageIndex = 11;
            this.btnSalir.ImageList = this.Imgn_list1;
            this.btnSalir.Location = new System.Drawing.Point(761, 111);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(101, 81);
            this.btnSalir.TabIndex = 10;
            this.btnSalir.UseVisualStyleBackColor = true;
            // 
            // btnFin
            // 
            this.btnFin.ImageIndex = 12;
            this.btnFin.ImageList = this.Imgn_list1;
            this.btnFin.Location = new System.Drawing.Point(975, 24);
            this.btnFin.Name = "btnFin";
            this.btnFin.Size = new System.Drawing.Size(101, 81);
            this.btnFin.TabIndex = 11;
            this.btnFin.UseVisualStyleBackColor = true;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.ImageIndex = 10;
            this.btnRefrescar.ImageList = this.Imgn_list1;
            this.btnRefrescar.Location = new System.Drawing.Point(440, 24);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(101, 81);
            this.btnRefrescar.TabIndex = 13;
            this.btnRefrescar.UseVisualStyleBackColor = true;
            // 
            // btnAyuda
            // 
            this.btnAyuda.ImageIndex = 14;
            this.btnAyuda.ImageList = this.Imgn_list1;
            this.btnAyuda.Location = new System.Drawing.Point(654, 111);
            this.btnAyuda.Name = "btnAyuda";
            this.btnAyuda.Size = new System.Drawing.Size(101, 81);
            this.btnAyuda.TabIndex = 14;
            this.btnAyuda.UseVisualStyleBackColor = true;
            // 
            // Frm_Crud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            this.ClientSize = new System.Drawing.Size(1089, 653);
            this.Controls.Add(this.btnAyuda);
            this.Controls.Add(this.btnRefrescar);
            this.Controls.Add(this.btnFin);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnConsultar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnInicio);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnIngresar);
            this.Name = "Frm_Crud";
            this.Text = "Frm_Crud";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.ImageList Imgn_list1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnInicio;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnFin;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Button btnAyuda;
    }
}