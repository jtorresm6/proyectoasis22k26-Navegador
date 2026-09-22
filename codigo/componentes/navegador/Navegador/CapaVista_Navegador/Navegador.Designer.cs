namespace CapaVista_Navegador
{
    partial class Navegador
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Navegador));
            this.NavegadorBtnIngresar = new System.Windows.Forms.Button();
            this.NavegadorBtnConsultar = new System.Windows.Forms.Button();
            this.NavegadorBtnModificar = new System.Windows.Forms.Button();
            this.NavegadorBtnEliminar = new System.Windows.Forms.Button();
            this.NavegadorIlImagenes = new System.Windows.Forms.ImageList(this.components);
            this.NavegadorBtnCancelar = new System.Windows.Forms.Button();
            this.NavegadorBtnGuardar = new System.Windows.Forms.Button();
            this.NavegadorBtnAnterior = new System.Windows.Forms.Button();
            this.NavegadorBtnSiguiente = new System.Windows.Forms.Button();
            this.NavegadorBtnFin = new System.Windows.Forms.Button();
            this.NavegadorBtnInicio = new System.Windows.Forms.Button();
            this.NavegadorBtnImprimir = new System.Windows.Forms.Button();
            this.NavegadorBtnAyuda = new System.Windows.Forms.Button();
            this.NavegadorBtnSalir = new System.Windows.Forms.Button();
            this.NavegadorBtnRefrescar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NavegadorBtnIngresar
            // 
            this.NavegadorBtnIngresar.ImageIndex = 11;
            this.NavegadorBtnIngresar.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnIngresar.Location = new System.Drawing.Point(12, 15);
            // Botones "pegados" (sin espacio entre ellos): cada uno inicia justo donde termina el
            // anterior (ancho 101) para que la cinta ocupe menos pantalla, según lo pedido por el
            // catedrático. No se redujo el tamaño de los botones, solo el espacio entre ellos.
            this.NavegadorBtnIngresar.Name = "NavegadorBtnIngresar";
            this.NavegadorBtnIngresar.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnIngresar.TabIndex = 1;
            this.NavegadorBtnIngresar.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnConsultar
            // 
            this.NavegadorBtnConsultar.ImageIndex = 2;
            this.NavegadorBtnConsultar.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnConsultar.Location = new System.Drawing.Point(113, 15);
            this.NavegadorBtnConsultar.Name = "NavegadorBtnConsultar";
            this.NavegadorBtnConsultar.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnConsultar.TabIndex = 2;
            this.NavegadorBtnConsultar.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnModificar
            // 
            this.NavegadorBtnModificar.ImageIndex = 15;
            this.NavegadorBtnModificar.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnModificar.Location = new System.Drawing.Point(214, 15);
            this.NavegadorBtnModificar.Name = "NavegadorBtnModificar";
            this.NavegadorBtnModificar.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnModificar.TabIndex = 3;
            this.NavegadorBtnModificar.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnEliminar
            // 
            this.NavegadorBtnEliminar.ImageIndex = 3;
            this.NavegadorBtnEliminar.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnEliminar.Location = new System.Drawing.Point(315, 15);
            this.NavegadorBtnEliminar.Name = "NavegadorBtnEliminar";
            this.NavegadorBtnEliminar.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnEliminar.TabIndex = 4;
            this.NavegadorBtnEliminar.UseVisualStyleBackColor = true;
            // 
            // NavegadorIlImagenes
            // 
            this.NavegadorIlImagenes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("NavegadorIlImagenes.ImageStream")));
            this.NavegadorIlImagenes.TransparentColor = System.Drawing.Color.Transparent;
            this.NavegadorIlImagenes.Images.SetKeyName(0, "ayuda.png");
            this.NavegadorIlImagenes.Images.SetKeyName(1, "cancelar.png");
            this.NavegadorIlImagenes.Images.SetKeyName(2, "consultar.png");
            this.NavegadorIlImagenes.Images.SetKeyName(3, "eliminar.png");
            this.NavegadorIlImagenes.Images.SetKeyName(4, "Fin.png");
            this.NavegadorIlImagenes.Images.SetKeyName(5, "Guardar.png");
            this.NavegadorIlImagenes.Images.SetKeyName(6, "icono aterior.png");
            this.NavegadorIlImagenes.Images.SetKeyName(7, "Icono Navegador.ico");
            this.NavegadorIlImagenes.Images.SetKeyName(8, "Icono Navegador.png");
            this.NavegadorIlImagenes.Images.SetKeyName(9, "iconos navegador - copia - copia.png");
            this.NavegadorIlImagenes.Images.SetKeyName(10, "imprimir.png");
            this.NavegadorIlImagenes.Images.SetKeyName(11, "ingresar.png");
            this.NavegadorIlImagenes.Images.SetKeyName(12, "inicio.png");
            this.NavegadorIlImagenes.Images.SetKeyName(13, "logo  con fondo.jpg");
            this.NavegadorIlImagenes.Images.SetKeyName(14, "logo sin fondo embutidos.png");
            this.NavegadorIlImagenes.Images.SetKeyName(15, "modificar.png");
            this.NavegadorIlImagenes.Images.SetKeyName(16, "refrescar.png");
            this.NavegadorIlImagenes.Images.SetKeyName(17, "salir.png");
            this.NavegadorIlImagenes.Images.SetKeyName(18, "siguiente.png");
            // 
            // NavegadorBtnCancelar
            // 
            this.NavegadorBtnCancelar.ImageIndex = 1;
            this.NavegadorBtnCancelar.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnCancelar.Location = new System.Drawing.Point(517, 15);
            this.NavegadorBtnCancelar.Name = "NavegadorBtnCancelar";
            this.NavegadorBtnCancelar.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnCancelar.TabIndex = 6;
            this.NavegadorBtnCancelar.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnGuardar
            // 
            this.NavegadorBtnGuardar.ImageIndex = 5;
            this.NavegadorBtnGuardar.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnGuardar.Location = new System.Drawing.Point(416, 15);
            this.NavegadorBtnGuardar.Name = "NavegadorBtnGuardar";
            this.NavegadorBtnGuardar.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnGuardar.TabIndex = 5;
            this.NavegadorBtnGuardar.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnAnterior
            // 
            this.NavegadorBtnAnterior.ImageIndex = 6;
            this.NavegadorBtnAnterior.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnAnterior.Location = new System.Drawing.Point(1022, 15);
            this.NavegadorBtnAnterior.Name = "NavegadorBtnAnterior";
            this.NavegadorBtnAnterior.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnAnterior.TabIndex = 11;
            this.NavegadorBtnAnterior.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnSiguiente
            // 
            this.NavegadorBtnSiguiente.ImageIndex = 18;
            this.NavegadorBtnSiguiente.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnSiguiente.Location = new System.Drawing.Point(1123, 15);
            this.NavegadorBtnSiguiente.Name = "NavegadorBtnSiguiente";
            this.NavegadorBtnSiguiente.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnSiguiente.TabIndex = 12;
            this.NavegadorBtnSiguiente.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnFin
            // 
            this.NavegadorBtnFin.ImageIndex = 4;
            this.NavegadorBtnFin.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnFin.Location = new System.Drawing.Point(1224, 15);
            this.NavegadorBtnFin.Name = "NavegadorBtnFin";
            this.NavegadorBtnFin.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnFin.TabIndex = 13;
            this.NavegadorBtnFin.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnInicio
            // 
            this.NavegadorBtnInicio.ImageIndex = 12;
            this.NavegadorBtnInicio.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnInicio.Location = new System.Drawing.Point(921, 15);
            this.NavegadorBtnInicio.Name = "NavegadorBtnInicio";
            this.NavegadorBtnInicio.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnInicio.TabIndex = 10;
            this.NavegadorBtnInicio.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnImprimir
            // 
            this.NavegadorBtnImprimir.ImageIndex = 10;
            this.NavegadorBtnImprimir.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnImprimir.Location = new System.Drawing.Point(719, 15);
            this.NavegadorBtnImprimir.Name = "NavegadorBtnImprimir";
            this.NavegadorBtnImprimir.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnImprimir.TabIndex = 8;
            this.NavegadorBtnImprimir.UseVisualStyleBackColor = true;
            // 
            // NavegadorBtnAyuda
            // 
            this.NavegadorBtnAyuda.ImageIndex = 0;
            this.NavegadorBtnAyuda.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnAyuda.Location = new System.Drawing.Point(820, 15);
            this.NavegadorBtnAyuda.Name = "NavegadorBtnAyuda";
            this.NavegadorBtnAyuda.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnAyuda.TabIndex = 9;
            this.NavegadorBtnAyuda.UseVisualStyleBackColor = true;
            //
            // NavegadorBtnSalir
            //
            this.NavegadorBtnSalir.ImageIndex = 17;
            this.NavegadorBtnSalir.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnSalir.Location = new System.Drawing.Point(1325, 15);
            this.NavegadorBtnSalir.Name = "NavegadorBtnSalir";
            this.NavegadorBtnSalir.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnSalir.TabIndex = 14;
            this.NavegadorBtnSalir.UseVisualStyleBackColor = true;
            //
            // NavegadorBtnRefrescar
            //
            this.NavegadorBtnRefrescar.ImageIndex = 16;
            this.NavegadorBtnRefrescar.ImageList = this.NavegadorIlImagenes;
            this.NavegadorBtnRefrescar.Location = new System.Drawing.Point(618, 15);
            this.NavegadorBtnRefrescar.Name = "NavegadorBtnRefrescar";
            this.NavegadorBtnRefrescar.Size = new System.Drawing.Size(101, 81);
            this.NavegadorBtnRefrescar.TabIndex = 7;
            this.NavegadorBtnRefrescar.UseVisualStyleBackColor = true;
            // 
            // Navegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NavegadorBtnRefrescar);
            this.Controls.Add(this.NavegadorBtnSalir);
            this.Controls.Add(this.NavegadorBtnAyuda);
            this.Controls.Add(this.NavegadorBtnImprimir);
            this.Controls.Add(this.NavegadorBtnInicio);
            this.Controls.Add(this.NavegadorBtnFin);
            this.Controls.Add(this.NavegadorBtnSiguiente);
            this.Controls.Add(this.NavegadorBtnAnterior);
            this.Controls.Add(this.NavegadorBtnGuardar);
            this.Controls.Add(this.NavegadorBtnCancelar);
            this.Controls.Add(this.NavegadorBtnEliminar);
            this.Controls.Add(this.NavegadorBtnModificar);
            this.Controls.Add(this.NavegadorBtnConsultar);
            this.Controls.Add(this.NavegadorBtnIngresar);
            this.Name = "Navegador";
            this.Size = new System.Drawing.Size(1438, 111);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NavegadorBtnIngresar;
        private System.Windows.Forms.Button NavegadorBtnConsultar;
        private System.Windows.Forms.Button NavegadorBtnModificar;
        private System.Windows.Forms.Button NavegadorBtnEliminar;
        private System.Windows.Forms.ImageList NavegadorIlImagenes;
        private System.Windows.Forms.Button NavegadorBtnCancelar;
        private System.Windows.Forms.Button NavegadorBtnGuardar;
        private System.Windows.Forms.Button NavegadorBtnAnterior;
        private System.Windows.Forms.Button NavegadorBtnSiguiente;
        private System.Windows.Forms.Button NavegadorBtnFin;
        private System.Windows.Forms.Button NavegadorBtnInicio;
        private System.Windows.Forms.Button NavegadorBtnImprimir;
        private System.Windows.Forms.Button NavegadorBtnAyuda;
        private System.Windows.Forms.Button NavegadorBtnSalir;
        private System.Windows.Forms.Button NavegadorBtnRefrescar;
    }
}
