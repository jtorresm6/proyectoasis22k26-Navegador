/*
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * Propósito : Diseño del formulario de inicio de sesión de Contabilidad.
 */

namespace CapaVista_Contabilidad
{
    partial class FrmLoginContabilidad
    {
        /// <summary>
        /// Variable requerida por el diseñador.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberar los recursos utilizados.
        /// </summary>
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(Disposing);
        }

        #region Código generado por el diseñador de Windows Forms

        /// <summary>
        /// Método requerido por el diseñador; no modificar
        /// su contenido desde el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager Recursos = new System.ComponentModel.ComponentResourceManager(typeof(FrmLoginContabilidad));
            this.ContabilidadPnlInterfazLogin = new System.Windows.Forms.Panel();
            this.ContabilidadPbMascota = new System.Windows.Forms.PictureBox();
            this.ContabilidadBtnLogin = new System.Windows.Forms.Button();
            this.ContabilidadLblContrasena = new System.Windows.Forms.Label();
            this.ContabilidadTxtContrasena = new System.Windows.Forms.TextBox();
            this.ContabilidadPnlLineaContrasena = new System.Windows.Forms.Panel();
            this.ContabilidadLblUsuario = new System.Windows.Forms.Label();
            this.ContabilidadTxtUsuario = new System.Windows.Forms.TextBox();
            this.ContabilidadLblTituloLogin = new System.Windows.Forms.Label();
            this.ContabilidadPnlLineaUsuario = new System.Windows.Forms.Panel();
            this.ContabilidadPbFondo = new System.Windows.Forms.PictureBox();
            this.ContabilidadPbLogo = new System.Windows.Forms.PictureBox();
            this.ContabilidadChkMostrarContra = new System.Windows.Forms.CheckBox();
            this.ContabilidadPbFondo.SuspendLayout();
            this.ContabilidadPnlInterfazLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbMascota)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbFondo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // ContabilidadPnlInterfazLogin
            // 
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadChkMostrarContra);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadPbMascota);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadBtnLogin);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadLblContrasena);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadTxtContrasena);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadPnlLineaContrasena);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadLblUsuario);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadTxtUsuario);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadLblTituloLogin);
            this.ContabilidadPnlInterfazLogin.Controls.Add(this.ContabilidadPnlLineaUsuario);
            this.ContabilidadPnlInterfazLogin.BackColor = System.Drawing.Color.FromArgb(246, 247, 248);
            this.ContabilidadPnlInterfazLogin.Location = new System.Drawing.Point(529, 106);
            this.ContabilidadPnlInterfazLogin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadPnlInterfazLogin.Name = "ContabilidadPnlInterfazLogin";
            this.ContabilidadPnlInterfazLogin.Size = new System.Drawing.Size(351, 455);
            this.ContabilidadPnlInterfazLogin.TabIndex = 5;
            this.ContabilidadPnlInterfazLogin.Paint += new System.Windows.Forms.PaintEventHandler(this.ContabilidadMetPintarPanel);
            // 
            // ContabilidadPbMascota
            // 
            this.ContabilidadPbMascota.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPbMascota.Image = global::CapaVista_Contabilidad.Properties.Resources.DonButiLogin;
            this.ContabilidadPbMascota.Location = new System.Drawing.Point(121, 34);
            this.ContabilidadPbMascota.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadPbMascota.Name = "ContabilidadPbMascota";
            this.ContabilidadPbMascota.Size = new System.Drawing.Size(116, 129);
            this.ContabilidadPbMascota.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ContabilidadPbMascota.TabIndex = 8;
            this.ContabilidadPbMascota.TabStop = false;
            // 
            // ContabilidadBtnLogin
            // 
            this.ContabilidadBtnLogin.BackColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadBtnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ContabilidadBtnLogin.FlatAppearance.BorderSize = 0;
            this.ContabilidadBtnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ContabilidadBtnLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ContabilidadBtnLogin.ForeColor = System.Drawing.Color.White;
            this.ContabilidadBtnLogin.Location = new System.Drawing.Point(120, 390);
            this.ContabilidadBtnLogin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadBtnLogin.Name = "ContabilidadBtnLogin";
            this.ContabilidadBtnLogin.Size = new System.Drawing.Size(117, 36);
            this.ContabilidadBtnLogin.TabIndex = 0;
            this.ContabilidadBtnLogin.Text = "Iniciar sesión";
            this.ContabilidadBtnLogin.UseVisualStyleBackColor = false;
            this.ContabilidadBtnLogin.Click += new System.EventHandler(this.ContabilidadMetIniciarSesion);
            // 
            // ContabilidadLblContrasena
            // 
            this.ContabilidadLblContrasena.AutoSize = true;
            this.ContabilidadLblContrasena.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContabilidadLblContrasena.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblContrasena.Location = new System.Drawing.Point(48, 276);
            this.ContabilidadLblContrasena.Name = "ContabilidadLblContrasena";
            this.ContabilidadLblContrasena.Size = new System.Drawing.Size(96, 17);
            this.ContabilidadLblContrasena.TabIndex = 5;
            this.ContabilidadLblContrasena.Text = "Contraseña";
            // 
            // ContabilidadTxtContrasena
            // 
            this.ContabilidadTxtContrasena.Cursor = System.Windows.Forms.Cursors.Default;
            this.ContabilidadTxtContrasena.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadTxtContrasena.Location = new System.Drawing.Point(48, 302);
            this.ContabilidadTxtContrasena.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadTxtContrasena.Name = "ContabilidadTxtContrasena";
            this.ContabilidadTxtContrasena.PasswordChar = '*';
            this.ContabilidadTxtContrasena.Size = new System.Drawing.Size(264, 22);
            this.ContabilidadTxtContrasena.TabIndex = 4;
            this.ContabilidadTxtContrasena.Tag = "";
            // 
            // ContabilidadPnlLineaContrasena
            // 
            this.ContabilidadPnlLineaContrasena.BackColor = System.Drawing.Color.FromArgb(212, 222, 228);
            this.ContabilidadPnlLineaContrasena.Visible = false;
            this.ContabilidadPnlLineaContrasena.Location = new System.Drawing.Point(43, 297);
            this.ContabilidadPnlLineaContrasena.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadPnlLineaContrasena.Name = "ContabilidadPnlLineaContrasena";
            this.ContabilidadPnlLineaContrasena.Size = new System.Drawing.Size(275, 32);
            this.ContabilidadPnlLineaContrasena.TabIndex = 6;
            // 
            // ContabilidadLblUsuario
            // 
            this.ContabilidadLblUsuario.AutoSize = true;
            this.ContabilidadLblUsuario.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContabilidadLblUsuario.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblUsuario.Location = new System.Drawing.Point(48, 206);
            this.ContabilidadLblUsuario.Name = "ContabilidadLblUsuario";
            this.ContabilidadLblUsuario.Size = new System.Drawing.Size(75, 17);
            this.ContabilidadLblUsuario.TabIndex = 2;
            this.ContabilidadLblUsuario.Text = "Usuario";
            // 
            // ContabilidadTxtUsuario
            // 
            this.ContabilidadTxtUsuario.Cursor = System.Windows.Forms.Cursors.Default;
            this.ContabilidadTxtUsuario.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadTxtUsuario.Location = new System.Drawing.Point(48, 232);
            this.ContabilidadTxtUsuario.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadTxtUsuario.Name = "ContabilidadTxtUsuario";
            this.ContabilidadTxtUsuario.Size = new System.Drawing.Size(264, 22);
            this.ContabilidadTxtUsuario.TabIndex = 1;
            this.ContabilidadTxtUsuario.Tag = "";
            // 
            // ContabilidadLblTituloLogin
            // 
            this.ContabilidadLblTituloLogin.AutoSize = true;
            this.ContabilidadLblTituloLogin.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContabilidadLblTituloLogin.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblTituloLogin.Location = new System.Drawing.Point(72, 166);
            this.ContabilidadLblTituloLogin.Name = "ContabilidadLblTituloLogin";
            this.ContabilidadLblTituloLogin.Size = new System.Drawing.Size(122, 20);
            this.ContabilidadLblTituloLogin.TabIndex = 1;
            this.ContabilidadLblTituloLogin.Text = "Iniciar sesión";
            // 
            // ContabilidadPnlLineaUsuario
            // 
            this.ContabilidadPnlLineaUsuario.BackColor = System.Drawing.Color.FromArgb(212, 222, 228);
            this.ContabilidadPnlLineaUsuario.Visible = false;
            this.ContabilidadPnlLineaUsuario.Location = new System.Drawing.Point(43, 227);
            this.ContabilidadPnlLineaUsuario.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadPnlLineaUsuario.Name = "ContabilidadPnlLineaUsuario";
            this.ContabilidadPnlLineaUsuario.Size = new System.Drawing.Size(275, 32);
            this.ContabilidadPnlLineaUsuario.TabIndex = 3;
            // 
            // ContabilidadPbFondo
            // 
            this.ContabilidadPbFondo.Image = global::CapaVista_Contabilidad.Properties.Resources.fondoLogin;
            this.ContabilidadPbFondo.Location = new System.Drawing.Point(-4, -10);
            this.ContabilidadPbFondo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadPbFondo.Name = "ContabilidadPbFondo";
            this.ContabilidadPbFondo.Size = new System.Drawing.Size(947, 619);
            this.ContabilidadPbFondo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ContabilidadPbFondo.TabIndex = 6;
            this.ContabilidadPbFondo.TabStop = false;
            // 
            // ContabilidadPbLogo
            // 
            this.ContabilidadPbLogo.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPbLogo.Image = global::CapaVista_Contabilidad.Properties.Resources.logo;
            this.ContabilidadPbLogo.Location = new System.Drawing.Point(81, 92);
            this.ContabilidadPbLogo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContabilidadPbLogo.Name = "ContabilidadPbLogo";
            this.ContabilidadPbLogo.Size = new System.Drawing.Size(225, 222);
            this.ContabilidadPbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ContabilidadPbLogo.TabIndex = 7;
            this.ContabilidadPbLogo.TabStop = false;
            // 
            // ContabilidadChkMostrarContra
            // 
            this.ContabilidadChkMostrarContra.AutoSize = true;
            this.ContabilidadChkMostrarContra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContabilidadChkMostrarContra.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadChkMostrarContra.Location = new System.Drawing.Point(235, 350);
            this.ContabilidadChkMostrarContra.Name = "ContabilidadChkMostrarContra";
            this.ContabilidadChkMostrarContra.Size = new System.Drawing.Size(52, 24);
            this.ContabilidadChkMostrarContra.TabIndex = 16;
            this.ContabilidadChkMostrarContra.Text = "Mostrar";
            this.ContabilidadChkMostrarContra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ContabilidadChkMostrarContra.UseVisualStyleBackColor = true;
            this.ContabilidadChkMostrarContra.CheckedChanged += new System.EventHandler(this.ContabilidadMetMostrarContrasena);
            // 
            // FrmLoginContabilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AcceptButton = this.ContabilidadBtnLogin;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(931, 599);
            this.ContabilidadPbFondo.Controls.Add(this.ContabilidadPbLogo);
            this.ContabilidadPbFondo.Controls.Add(this.ContabilidadPnlInterfazLogin);
            this.Controls.Add(this.ContabilidadPbFondo);
            this.Icon = ((System.Drawing.Icon)(Recursos.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Name = "FrmLoginContabilidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "10006 - LoginContabilidad";
            this.ContabilidadPbFondo.ResumeLayout(false);
            this.ContabilidadPnlInterfazLogin.ResumeLayout(false);
            this.ContabilidadPnlInterfazLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbMascota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbFondo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ContabilidadPnlInterfazLogin;
        private System.Windows.Forms.Button ContabilidadBtnLogin;
        private System.Windows.Forms.Label ContabilidadLblContrasena;
        private System.Windows.Forms.TextBox ContabilidadTxtContrasena;
        private System.Windows.Forms.Panel ContabilidadPnlLineaContrasena;
        private System.Windows.Forms.Label ContabilidadLblUsuario;
        private System.Windows.Forms.TextBox ContabilidadTxtUsuario;
        private System.Windows.Forms.Label ContabilidadLblTituloLogin;
        private System.Windows.Forms.Panel ContabilidadPnlLineaUsuario;
        private System.Windows.Forms.PictureBox ContabilidadPbFondo;
        private System.Windows.Forms.PictureBox ContabilidadPbMascota;
        private System.Windows.Forms.PictureBox ContabilidadPbLogo;
        private System.Windows.Forms.CheckBox ContabilidadChkMostrarContra;
    }
}
