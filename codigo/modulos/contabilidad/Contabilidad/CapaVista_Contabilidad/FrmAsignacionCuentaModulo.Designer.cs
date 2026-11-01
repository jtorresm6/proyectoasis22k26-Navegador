/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Mario Taracena
 * Carné : 0901-23-9335
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Diseño de la ventana FrmAsignacionCuentaModulo: solo contiene el control Navegador.
 * ===================================================================
*/

namespace CapaVista_Contabilidad
{
    partial class FrmAsignacionCuentaModulo
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
            this.ContabilidadNavAsignacionCuentaModulo = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            this.ContabilidadNavAsignacionCuentaModulo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContabilidadNavAsignacionCuentaModulo.Location = new System.Drawing.Point(11, 11);
            this.ContabilidadNavAsignacionCuentaModulo.Margin = new System.Windows.Forms.Padding(2);
            this.ContabilidadNavAsignacionCuentaModulo.Name = "ContabilidadNavAsignacionCuentaModulo";
            this.ContabilidadNavAsignacionCuentaModulo.Size = new System.Drawing.Size(1158, 90);
            this.ContabilidadNavAsignacionCuentaModulo.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 688);
            this.Controls.Add(this.ContabilidadNavAsignacionCuentaModulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Contabilidad.Properties.Resources.Icono_Contabilidad;
            this.MaximizeBox = false;
            this.Name = "FrmAsignacionCuentaModulo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "10015 - AsignacionCuentaModulo";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador ContabilidadNavAsignacionCuentaModulo;
    }
}
