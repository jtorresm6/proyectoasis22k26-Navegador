

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Define la distribución visual de los productos y la navegación por bodegas.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    partial class UsrProductosOrdenesCompra
    {
        private System.ComponentModel.IContainer components;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ScrollTabla?.Dispose();
                ComprasDgvProductos.DataSource = null;
                _ProductosBodega?.Dispose();
                _Productos.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsrProductosOrdenesCompra));
            this.ComprasTlpDetalles = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasTlpNavegacion = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasBtnBodegaAnterior = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasFlpAccionesProductos = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnBodegaSiguiente = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasDgvProductos = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasTlpDetalles.SuspendLayout();
            this.ComprasTlpNavegacion.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpDetalles
            // 
            this.ComprasTlpDetalles.ColumnCount = 1;
            this.ComprasTlpDetalles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Controls.Add(this.ComprasTlpNavegacion, 0, 0);
            this.ComprasTlpDetalles.Controls.Add(this.ComprasDgvProductos, 0, 1);
            this.ComprasTlpDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpDetalles.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpDetalles.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpDetalles.Name = "ComprasTlpDetalles";
            this.ComprasTlpDetalles.RowCount = 2;
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Size = new System.Drawing.Size(773, 430);
            this.ComprasTlpDetalles.TabIndex = 1;
            // 
            // ComprasTlpNavegacion
            // 
            this.ComprasTlpNavegacion.ColumnCount = 3;
            this.ComprasTlpNavegacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpNavegacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpNavegacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpNavegacion.Controls.Add(this.ComprasBtnBodegaAnterior, 0, 0);
            this.ComprasTlpNavegacion.Controls.Add(this.ComprasFlpAccionesProductos, 1, 0);
            this.ComprasTlpNavegacion.Controls.Add(this.ComprasBtnBodegaSiguiente, 2, 0);
            this.ComprasTlpNavegacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpNavegacion.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpNavegacion.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpNavegacion.Name = "ComprasTlpNavegacion";
            this.ComprasTlpNavegacion.RowCount = 1;
            this.ComprasTlpNavegacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpNavegacion.Size = new System.Drawing.Size(773, 80);
            this.ComprasTlpNavegacion.TabIndex = 0;
            // 
            // ComprasBtnBodegaAnterior
            // 
            this.ComprasBtnBodegaAnterior.AccessibleName = "Anterior";
            this.ComprasBtnBodegaAnterior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnBodegaAnterior.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnBodegaAnterior.BackgroundImage")));
            this.ComprasBtnBodegaAnterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnBodegaAnterior.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnBodegaAnterior.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasBtnBodegaAnterior.Enabled = false;
            this.ComprasBtnBodegaAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnBodegaAnterior.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnBodegaAnterior.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnBodegaAnterior.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnBodegaAnterior.ImagenDeshabilitado")));
            this.ComprasBtnBodegaAnterior.Location = new System.Drawing.Point(0, 0);
            this.ComprasBtnBodegaAnterior.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnBodegaAnterior.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaAnterior.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaAnterior.Name = "ComprasBtnBodegaAnterior";
            this.ComprasBtnBodegaAnterior.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaAnterior.TabIndex = 0;
            this.ComprasBtnBodegaAnterior.UseVisualStyleBackColor = false;
            // 
            // ComprasFlpAccionesProductos
            // 
            this.ComprasFlpAccionesProductos.AutoScroll = false;
            this.ComprasFlpAccionesProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasFlpAccionesProductos.Location = new System.Drawing.Point(80, 0);
            this.ComprasFlpAccionesProductos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpAccionesProductos.Name = "ComprasFlpAccionesProductos";
            this.ComprasFlpAccionesProductos.Size = new System.Drawing.Size(613, 80);
            this.ComprasFlpAccionesProductos.TabIndex = 1;
            this.ComprasFlpAccionesProductos.WrapContents = false;
            // 
            // ComprasBtnBodegaSiguiente
            // 
            this.ComprasBtnBodegaSiguiente.AccessibleName = "Siguiente";
            this.ComprasBtnBodegaSiguiente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnBodegaSiguiente.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnBodegaSiguiente.BackgroundImage")));
            this.ComprasBtnBodegaSiguiente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnBodegaSiguiente.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnBodegaSiguiente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasBtnBodegaSiguiente.Enabled = false;
            this.ComprasBtnBodegaSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnBodegaSiguiente.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnBodegaSiguiente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnBodegaSiguiente.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnBodegaSiguiente.ImagenDeshabilitado")));
            this.ComprasBtnBodegaSiguiente.Location = new System.Drawing.Point(693, 0);
            this.ComprasBtnBodegaSiguiente.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnBodegaSiguiente.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaSiguiente.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaSiguiente.Name = "ComprasBtnBodegaSiguiente";
            this.ComprasBtnBodegaSiguiente.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaSiguiente.TabIndex = 2;
            this.ComprasBtnBodegaSiguiente.UseVisualStyleBackColor = false;
            // 
            // ComprasDgvProductos
            // 
            this.ComprasDgvProductos.AutoGenerateColumns = false;
            this.ComprasDgvProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvProductos.Location = new System.Drawing.Point(0, 85);
            this.ComprasDgvProductos.Margin = new System.Windows.Forms.Padding(0, 5, 0, 10);
            this.ComprasDgvProductos.Name = "ComprasDgvProductos";
            this.ComprasDgvProductos.ResaltarFilasAlPasar = true;
            this.ComprasDgvProductos.Size = new System.Drawing.Size(773, 335);
            this.ComprasDgvProductos.TabIndex = 1;
            // 
            // UsrProductosOrdenesCompra
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.ComprasTlpDetalles);
            this.Name = "UsrProductosOrdenesCompra";
            this.Size = new System.Drawing.Size(773, 430);
            this.ComprasTlpDetalles.ResumeLayout(false);
            this.ComprasTlpNavegacion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel ComprasTlpDetalles;
        private System.Windows.Forms.TableLayoutPanel ComprasTlpNavegacion;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnBodegaAnterior;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpAccionesProductos;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnBodegaSiguiente;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvProductos;
    }
}
