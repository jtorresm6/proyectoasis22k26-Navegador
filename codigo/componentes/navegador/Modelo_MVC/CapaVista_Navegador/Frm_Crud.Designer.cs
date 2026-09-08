using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Crud));
            Btn_ingresar = new Button();
            Imgn_list1 = new ImageList(components);
            Btn_cancelar = new Button();
            Btn_modificar = new Button();
            Btn_imprimir = new Button();
            Btn_guardar = new Button();
            Btn_siguiente = new Button();
            Btn_anterior = new Button();
            Btn_inicio = new Button();
            Btn_eliminar = new Button();
            Btn_Consultar = new Button();
            Btn_salir = new Button();
            Btn_fin = new Button();
            Dgv_datos = new DataGridView();
            Btn_refrescar = new Button();
            Btn_ayuda = new Button();
<<<<<<< HEAD
=======
            Pnl_campos = new FlowLayoutPanel();
            Lbl_modo = new Label();
            Lbl_estado = new Label();
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)
            ((System.ComponentModel.ISupportInitialize)Dgv_datos).BeginInit();
            SuspendLayout();
            // 
            // Btn_ingresar
            // 
            Btn_ingresar.ImageIndex = 0;
            Btn_ingresar.ImageList = Imgn_list1;
            Btn_ingresar.Location = new Point(12, 30);
            Btn_ingresar.Margin = new Padding(3, 4, 3, 4);
            Btn_ingresar.Name = "Btn_ingresar";
            Btn_ingresar.Size = new Size(101, 101);
            Btn_ingresar.TabIndex = 0;
            Btn_ingresar.UseVisualStyleBackColor = true;
            // 
            // Imgn_list1
            // 
            Imgn_list1.ColorDepth = ColorDepth.Depth4Bit;
            Imgn_list1.ImageStream = (ImageListStreamer)resources.GetObject("Imgn_list1.ImageStream");
            Imgn_list1.TransparentColor = Color.Transparent;
            Imgn_list1.Images.SetKeyName(0, "ingresar.png");
            Imgn_list1.Images.SetKeyName(1, "cancelar.png");
            Imgn_list1.Images.SetKeyName(2, "consultar.png");
            Imgn_list1.Images.SetKeyName(3, "eliminar.png");
            Imgn_list1.Images.SetKeyName(4, "Fin.png");
            Imgn_list1.Images.SetKeyName(5, "Guardar.png");
            Imgn_list1.Images.SetKeyName(6, "icono aterior.png");
            Imgn_list1.Images.SetKeyName(7, "imprimir.png");
            Imgn_list1.Images.SetKeyName(8, "inicio.png");
            Imgn_list1.Images.SetKeyName(9, "modificar.png");
            Imgn_list1.Images.SetKeyName(10, "refrescar.png");
            Imgn_list1.Images.SetKeyName(11, "salir.png");
            Imgn_list1.Images.SetKeyName(12, "siguiente.png");
            Imgn_list1.Images.SetKeyName(13, "Fin.png");
            Imgn_list1.Images.SetKeyName(14, "ayuda.png");
            // 
            // Btn_cancelar
            // 
            Btn_cancelar.ImageIndex = 1;
            Btn_cancelar.ImageList = Imgn_list1;
            Btn_cancelar.Location = new Point(119, 30);
            Btn_cancelar.Margin = new Padding(3, 4, 3, 4);
            Btn_cancelar.Name = "Btn_cancelar";
            Btn_cancelar.Size = new Size(101, 101);
            Btn_cancelar.TabIndex = 1;
            Btn_cancelar.UseVisualStyleBackColor = true;
            // 
            // Btn_modificar
            // 
            Btn_modificar.ImageIndex = 9;
            Btn_modificar.ImageList = Imgn_list1;
            Btn_modificar.Location = new Point(547, 30);
            Btn_modificar.Margin = new Padding(3, 4, 3, 4);
            Btn_modificar.Name = "Btn_modificar";
            Btn_modificar.Size = new Size(101, 101);
            Btn_modificar.TabIndex = 2;
            Btn_modificar.UseVisualStyleBackColor = true;
            Btn_modificar.Click += Btn_modificar_Click;
            // 
            // Btn_imprimir
            // 
            Btn_imprimir.ImageIndex = 7;
            Btn_imprimir.ImageList = Imgn_list1;
            Btn_imprimir.Location = new Point(333, 139);
            Btn_imprimir.Margin = new Padding(3, 4, 3, 4);
            Btn_imprimir.Name = "Btn_imprimir";
            Btn_imprimir.Size = new Size(101, 101);
            Btn_imprimir.TabIndex = 3;
            Btn_imprimir.UseVisualStyleBackColor = true;
            // 
            // Btn_guardar
            // 
            Btn_guardar.ImageIndex = 5;
            Btn_guardar.ImageList = Imgn_list1;
            Btn_guardar.Location = new Point(440, 139);
            Btn_guardar.Margin = new Padding(3, 4, 3, 4);
            Btn_guardar.Name = "Btn_guardar";
            Btn_guardar.Size = new Size(101, 101);
            Btn_guardar.TabIndex = 4;
            Btn_guardar.UseVisualStyleBackColor = true;
            Btn_guardar.Click += Btn_guardar_Click;
            // 
            // Btn_siguiente
            // 
            Btn_siguiente.ImageIndex = 13;
            Btn_siguiente.ImageList = Imgn_list1;
            Btn_siguiente.Location = new Point(868, 30);
            Btn_siguiente.Margin = new Padding(3, 4, 3, 4);
            Btn_siguiente.Name = "Btn_siguiente";
            Btn_siguiente.Size = new Size(101, 101);
            Btn_siguiente.TabIndex = 5;
            Btn_siguiente.UseVisualStyleBackColor = true;
            // 
            // Btn_anterior
            // 
            Btn_anterior.ImageIndex = 8;
            Btn_anterior.ImageList = Imgn_list1;
            Btn_anterior.Location = new Point(761, 30);
            Btn_anterior.Margin = new Padding(3, 4, 3, 4);
            Btn_anterior.Name = "Btn_anterior";
            Btn_anterior.Size = new Size(101, 101);
            Btn_anterior.TabIndex = 6;
            Btn_anterior.UseVisualStyleBackColor = true;
            // 
            // Btn_inicio
            // 
            Btn_inicio.ImageIndex = 6;
            Btn_inicio.ImageList = Imgn_list1;
            Btn_inicio.Location = new Point(654, 30);
            Btn_inicio.Margin = new Padding(3, 4, 3, 4);
            Btn_inicio.Name = "Btn_inicio";
            Btn_inicio.Size = new Size(101, 101);
            Btn_inicio.TabIndex = 7;
            Btn_inicio.UseVisualStyleBackColor = true;
            // 
            // Btn_eliminar
            // 
            Btn_eliminar.ImageIndex = 3;
            Btn_eliminar.ImageList = Imgn_list1;
            Btn_eliminar.Location = new Point(333, 30);
            Btn_eliminar.Margin = new Padding(3, 4, 3, 4);
            Btn_eliminar.Name = "Btn_eliminar";
            Btn_eliminar.Size = new Size(101, 101);
            Btn_eliminar.TabIndex = 8;
            Btn_eliminar.UseVisualStyleBackColor = true;
            // 
            // Btn_Consultar
            // 
            Btn_Consultar.ImageIndex = 2;
            Btn_Consultar.ImageList = Imgn_list1;
            Btn_Consultar.Location = new Point(226, 30);
            Btn_Consultar.Margin = new Padding(3, 4, 3, 4);
            Btn_Consultar.Name = "Btn_Consultar";
            Btn_Consultar.Size = new Size(101, 101);
            Btn_Consultar.TabIndex = 9;
            Btn_Consultar.UseVisualStyleBackColor = true;
            Btn_Consultar.Click += Btn_Consultar_Click;
            // 
            // Btn_salir
            // 
            Btn_salir.ImageIndex = 11;
            Btn_salir.ImageList = Imgn_list1;
            Btn_salir.Location = new Point(761, 139);
            Btn_salir.Margin = new Padding(3, 4, 3, 4);
            Btn_salir.Name = "Btn_salir";
            Btn_salir.Size = new Size(101, 101);
            Btn_salir.TabIndex = 10;
            Btn_salir.UseVisualStyleBackColor = true;
            // 
            // Btn_fin
            // 
            Btn_fin.ImageIndex = 12;
            Btn_fin.ImageList = Imgn_list1;
            Btn_fin.Location = new Point(975, 30);
            Btn_fin.Margin = new Padding(3, 4, 3, 4);
            Btn_fin.Name = "Btn_fin";
            Btn_fin.Size = new Size(101, 101);
            Btn_fin.TabIndex = 11;
            Btn_fin.UseVisualStyleBackColor = true;
            // 
            // Dgv_datos
            // 
            Dgv_datos.BackgroundColor = Color.Maroon;
            Dgv_datos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Dgv_datos.Location = new Point(105, 361);
            Dgv_datos.Margin = new Padding(3, 4, 3, 4);
            Dgv_datos.Name = "Dgv_datos";
            Dgv_datos.RowHeadersWidth = 51;
            Dgv_datos.RowTemplate.Height = 24;
            Dgv_datos.Size = new Size(837, 330);
            Dgv_datos.TabIndex = 12;
            // 
            // Btn_refrescar
            // 
            Btn_refrescar.ImageIndex = 10;
            Btn_refrescar.ImageList = Imgn_list1;
            Btn_refrescar.Location = new Point(440, 30);
            Btn_refrescar.Margin = new Padding(3, 4, 3, 4);
            Btn_refrescar.Name = "Btn_refrescar";
            Btn_refrescar.Size = new Size(101, 101);
            Btn_refrescar.TabIndex = 13;
            Btn_refrescar.UseVisualStyleBackColor = true;
            Btn_refrescar.Click += Btn_refrescar_Click;
            // 
            // Btn_ayuda
            // 
            Btn_ayuda.ImageIndex = 14;
            Btn_ayuda.ImageList = Imgn_list1;
            Btn_ayuda.Location = new Point(654, 139);
            Btn_ayuda.Margin = new Padding(3, 4, 3, 4);
            Btn_ayuda.Name = "Btn_ayuda";
            Btn_ayuda.Size = new Size(101, 101);
            Btn_ayuda.TabIndex = 14;
            Btn_ayuda.UseVisualStyleBackColor = true;
<<<<<<< HEAD
=======
            // 
            // Pnl_campos
            // 
            Pnl_campos.AutoScroll = true;
            Pnl_campos.BorderStyle = BorderStyle.FixedSingle;
            Pnl_campos.Location = new Point(12, 248);
            Pnl_campos.Name = "Pnl_campos";
            Pnl_campos.Size = new Size(1064, 105);
            Pnl_campos.TabIndex = 16;
            // 
            // Lbl_modo
            // 
            Lbl_modo.AutoSize = true;
            Lbl_modo.Location = new Point(105, 700);
            Lbl_modo.Name = "Lbl_modo";
            Lbl_modo.TabIndex = 17;
            Lbl_modo.Text = "Modo:";
            // 
            // Lbl_estado
            // 
            Lbl_estado.AutoSize = true;
            Lbl_estado.Location = new Point(400, 700);
            Lbl_estado.Name = "Lbl_estado";
            Lbl_estado.TabIndex = 18;
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)
            // 
            // Frm_Crud
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Beige;
            ClientSize = new Size(1089, 816);
<<<<<<< HEAD
=======
            Controls.Add(Lbl_estado);
            Controls.Add(Lbl_modo);
            Controls.Add(Pnl_campos);
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)
            Controls.Add(Btn_ayuda);
            Controls.Add(Btn_refrescar);
            Controls.Add(Dgv_datos);
            Controls.Add(Btn_fin);
            Controls.Add(Btn_salir);
            Controls.Add(Btn_Consultar);
            Controls.Add(Btn_eliminar);
            Controls.Add(Btn_inicio);
            Controls.Add(Btn_anterior);
            Controls.Add(Btn_siguiente);
            Controls.Add(Btn_guardar);
            Controls.Add(Btn_imprimir);
            Controls.Add(Btn_modificar);
            Controls.Add(Btn_cancelar);
            Controls.Add(Btn_ingresar);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Frm_Crud";
            Text = "Frm_Crud";
            ((System.ComponentModel.ISupportInitialize)Dgv_datos).EndInit();
            ResumeLayout(false);
<<<<<<< HEAD
=======
            PerformLayout();
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)

        }

        #endregion

        private System.Windows.Forms.Button Btn_ingresar;
        private System.Windows.Forms.ImageList Imgn_list1;
        private System.Windows.Forms.Button Btn_cancelar;
        private System.Windows.Forms.Button Btn_modificar;
        private System.Windows.Forms.Button Btn_imprimir;
        private System.Windows.Forms.Button Btn_guardar;
        private System.Windows.Forms.Button Btn_siguiente;
        private System.Windows.Forms.Button Btn_anterior;
        private System.Windows.Forms.Button Btn_inicio;
        private System.Windows.Forms.Button Btn_eliminar;
        private System.Windows.Forms.Button Btn_Consultar;
        private System.Windows.Forms.Button Btn_salir;
        private System.Windows.Forms.Button Btn_fin;
        private System.Windows.Forms.DataGridView Dgv_datos;
        private System.Windows.Forms.Button Btn_refrescar;
        private System.Windows.Forms.Button Btn_ayuda;
    }
}