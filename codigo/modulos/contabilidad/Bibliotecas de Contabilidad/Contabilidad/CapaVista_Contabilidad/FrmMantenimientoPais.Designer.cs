/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Diseño de la ventana FrmMantenimientoPais: solo contiene el control Navegador.
 * ===================================================================
*/

namespace CapaVista_Contabilidad
{
    partial class FrmMantenimientoPais
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(Disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.ContabilidadNavPais = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            this.ContabilidadNavPais.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContabilidadNavPais.Location = new System.Drawing.Point(11, 11);
            this.ContabilidadNavPais.Margin = new System.Windows.Forms.Padding(2);
            this.ContabilidadNavPais.Name = "ContabilidadNavPais";
            this.ContabilidadNavPais.Size = new System.Drawing.Size(1158, 90);
            this.ContabilidadNavPais.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 688);
            this.Controls.Add(this.ContabilidadNavPais);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Contabilidad.Properties.Resources.Icono_Contabilidad;
            this.MaximizeBox = false;
            this.Name = "FrmMantenimientoPais";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "10011 - MantenimientoPais";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador ContabilidadNavPais;
    }
}
