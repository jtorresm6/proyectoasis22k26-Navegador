/*
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * Propósito : Diseño de la presentación inicial de Contabilidad.
 */

namespace CapaVista_Contabilidad
{
    partial class FrmSplashContabilidad
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
            this.components = new System.ComponentModel.Container();
            this.ContabilidadPgbCarga = new System.Windows.Forms.ProgressBar();
            this._TimerCarga = new System.Windows.Forms.Timer(this.components);
            this.ContabilidadLblTitulo = new System.Windows.Forms.Label();
            this.ContabilidadLblMensaje = new System.Windows.Forms.Label();
            this.ContabilidadLblPorcentaje = new System.Windows.Forms.Label();
            this.ContabilidadPbLogo = new System.Windows.Forms.PictureBox();
            this.ContabilidadPbMascota = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbMascota)).BeginInit();
            this.SuspendLayout();
            // 
            // ContabilidadPgbCarga
            // 
            this.ContabilidadPgbCarga.ForeColor = System.Drawing.Color.FromArgb(197, 143, 62);
            this.ContabilidadPgbCarga.Location = new System.Drawing.Point(163, 296);
            this.ContabilidadPgbCarga.Name = "ContabilidadPgbCarga";
            this.ContabilidadPgbCarga.Size = new System.Drawing.Size(476, 25);
            this.ContabilidadPgbCarga.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ContabilidadPgbCarga.TabIndex = 2;
            // 
            // _TimerCarga
            // 
            this._TimerCarga.Interval = 35;
            this._TimerCarga.Tick += new System.EventHandler(this.ContabilidadMetActualizarCarga);
            // 
            // ContabilidadLblTitulo
            // 
            this.ContabilidadLblTitulo.AutoSize = true;
            this.ContabilidadLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContabilidadLblTitulo.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblTitulo.Location = new System.Drawing.Point(327, 205);
            this.ContabilidadLblTitulo.Name = "ContabilidadLblTitulo";
            this.ContabilidadLblTitulo.Size = new System.Drawing.Size(162, 27);
            this.ContabilidadLblTitulo.TabIndex = 3;
            this.ContabilidadLblTitulo.Text = "CARGANDO";
            // 
            // ContabilidadLblMensaje
            // 
            this.ContabilidadLblMensaje.AutoSize = true;
            this.ContabilidadLblMensaje.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblMensaje.ForeColor = System.Drawing.Color.FromArgb(70, 104, 125);
            this.ContabilidadLblMensaje.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContabilidadLblMensaje.Location = new System.Drawing.Point(148, 240);
            this.ContabilidadLblMensaje.Name = "ContabilidadLblMensaje";
            this.ContabilidadLblMensaje.Size = new System.Drawing.Size(512, 19);
            this.ContabilidadLblMensaje.TabIndex = 5;
            this.ContabilidadLblMensaje.Text = "Por favor espere mientras preparamos la mejor experiencia para usted.";
            // 
            // ContabilidadLblPorcentaje
            // 
            this.ContabilidadLblPorcentaje.AutoSize = true;
            this.ContabilidadLblPorcentaje.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblPorcentaje.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblPorcentaje.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContabilidadLblPorcentaje.Location = new System.Drawing.Point(389, 326);
            this.ContabilidadLblPorcentaje.Name = "ContabilidadLblPorcentaje";
            this.ContabilidadLblPorcentaje.Size = new System.Drawing.Size(28, 19);
            this.ContabilidadLblPorcentaje.TabIndex = 6;
            this.ContabilidadLblPorcentaje.Text = "0%";
            this.ContabilidadLblPorcentaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ContabilidadPbLogo
            // 
            this.ContabilidadPbLogo.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPbLogo.BackgroundImage = global::CapaVista_Contabilidad.Properties.Resources.logo;
            this.ContabilidadPbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ContabilidadPbLogo.Location = new System.Drawing.Point(273, 73);
            this.ContabilidadPbLogo.Name = "ContabilidadPbLogo";
            this.ContabilidadPbLogo.Size = new System.Drawing.Size(130, 126);
            this.ContabilidadPbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ContabilidadPbLogo.TabIndex = 4;
            this.ContabilidadPbLogo.TabStop = false;
            // 
            // ContabilidadPbMascota
            // 
            this.ContabilidadPbMascota.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPbMascota.BackgroundImage = global::CapaVista_Contabilidad.Properties.Resources.DonButi;
            this.ContabilidadPbMascota.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ContabilidadPbMascota.Location = new System.Drawing.Point(406, 52);
            this.ContabilidadPbMascota.Name = "ContabilidadPbMascota";
            this.ContabilidadPbMascota.Size = new System.Drawing.Size(117, 147);
            this.ContabilidadPbMascota.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ContabilidadPbMascota.TabIndex = 0;
            this.ContabilidadPbMascota.TabStop = false;
            // 
            // FrmSplashContabilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::CapaVista_Contabilidad.Properties.Resources.fondoSplash;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ContabilidadLblPorcentaje);
            this.Controls.Add(this.ContabilidadLblMensaje);
            this.Controls.Add(this.ContabilidadPbLogo);
            this.Controls.Add(this.ContabilidadLblTitulo);
            this.Controls.Add(this.ContabilidadPgbCarga);
            this.Controls.Add(this.ContabilidadPbMascota);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSplashContabilidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "10007 - SplashContabilidad";
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadPbMascota)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ContabilidadPbMascota;
        private System.Windows.Forms.ProgressBar ContabilidadPgbCarga;
        private System.Windows.Forms.Timer _TimerCarga;
        private System.Windows.Forms.Label ContabilidadLblTitulo;
        private System.Windows.Forms.PictureBox ContabilidadPbLogo;
        private System.Windows.Forms.Label ContabilidadLblMensaje;
        private System.Windows.Forms.Label ContabilidadLblPorcentaje;
    }
}
