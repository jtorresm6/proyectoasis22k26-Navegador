namespace CapaVista_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Define el selector visual reutilizable de cliente.
    partial class VentasUsrCliente
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label VentasLblCliente;
        private System.Windows.Forms.Label VentasLblTipo;
        private System.Windows.Forms.ComboBox VentasCboSeleccionCliente;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.VentasLblCliente = new System.Windows.Forms.Label();
            this.VentasLblTipo = new System.Windows.Forms.Label();
            this.VentasCboSeleccionCliente = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            this.VentasLblCliente.AutoSize = true;
            this.VentasLblCliente.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblCliente.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasLblCliente.Location = new System.Drawing.Point(0, 0);
            this.VentasLblCliente.Name = "VentasLblCliente";
            this.VentasLblCliente.Text = "Cliente";
            this.VentasLblTipo.ForeColor = System.Drawing.Color.FromArgb(105, 96, 111);
            this.VentasLblTipo.Location = new System.Drawing.Point(100, 0);
            this.VentasLblTipo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblTipo.Name = "VentasLblTipo";
            this.VentasLblTipo.Size = new System.Drawing.Size(395, 20);
            this.VentasLblTipo.Text = "Tipo: seleccione un cliente";
            this.VentasLblTipo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.VentasCboSeleccionCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboSeleccionCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboSeleccionCliente.Location = new System.Drawing.Point(0, 22);
            this.VentasCboSeleccionCliente.Name = "VentasCboSeleccionCliente";
            this.VentasCboSeleccionCliente.Size = new System.Drawing.Size(500, 31);
            this.VentasCboSeleccionCliente.TabIndex = 0;
            this.VentasCboSeleccionCliente.SelectedIndexChanged += new System.EventHandler(this.VentasMetClienteCambiado);
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.VentasLblCliente);
            this.Controls.Add(this.VentasLblTipo);
            this.Controls.Add(this.VentasCboSeleccionCliente);
            this.Name = "VentasUsrCliente";
            this.Size = new System.Drawing.Size(500, 56);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
