/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Donald Osorio
 * Carné : 0901-23-17982
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Diseño de la ventana FrmMantenimientoCuenta: solo contiene el control Navegador.
 * ===================================================================
*/

namespace CapaVista_Contabilidad
{
    partial class FrmMantenimientoCuenta
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
            this.ContabilidadNavCuenta = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            this.ContabilidadNavCuenta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContabilidadNavCuenta.Location = new System.Drawing.Point(11, 11);
            this.ContabilidadNavCuenta.Margin = new System.Windows.Forms.Padding(2);
            this.ContabilidadNavCuenta.Name = "ContabilidadNavCuenta";
            this.ContabilidadNavCuenta.Size = new System.Drawing.Size(1158, 90);
            this.ContabilidadNavCuenta.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 688);
            this.Controls.Add(this.ContabilidadNavCuenta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Contabilidad.Properties.Resources.Icono_Contabilidad;
            this.MaximizeBox = false;
            this.Name = "FrmMantenimientoCuenta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "10014 - MantenimientoCuenta";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador ContabilidadNavCuenta;
    }
}
