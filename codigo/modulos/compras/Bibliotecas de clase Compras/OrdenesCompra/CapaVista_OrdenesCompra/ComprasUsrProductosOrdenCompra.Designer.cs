

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
    partial class ComprasUsrProductosOrdenCompra
    {
        private System.ComponentModel.IContainer components;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxProductos;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvProductos;
        private System.Windows.Forms.TableLayoutPanel ComprasTlpDetalles;
        private System.Windows.Forms.TableLayoutPanel ComprasTlpNavegacion;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpAccionesProductos;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnBodegaAnterior;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnBodegaSiguiente;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColsku;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColproducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColpeso;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColcantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColprecio;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColdescuento;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColporcentaje;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComprasColtotal;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ComprasDgvProductos.DataSource = null;
                _ProductosBodega?.Dispose();
                _Productos.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComprasUsrProductosOrdenCompra));
            this.ComprasDgvProductos = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasColsku = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColproducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColpeso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColcantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColprecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColdescuento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColporcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasColtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComprasTlpDetalles = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasTlpNavegacion = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasBtnBodegaAnterior = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasFlpAccionesProductos = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnBodegaSiguiente = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasGbxProductos = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpDetalles.SuspendLayout();
            this.ComprasTlpNavegacion.SuspendLayout();
            this.ComprasGbxProductos.SuspendLayout();
            this.SuspendLayout();
            //
            // ComprasDgvProductos
            //
            this.ComprasDgvProductos.AutoGenerateColumns = false;
            this.ComprasDgvProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvProductos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ComprasColsku,
            this.ComprasColproducto,
            this.ComprasColpeso,
            this.ComprasColcantidad,
            this.ComprasColprecio,
            this.ComprasColdescuento,
            this.ComprasColporcentaje,
            this.ComprasColtotal});
            this.ComprasDgvProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvProductos.Location = new System.Drawing.Point(5, 90);
            this.ComprasDgvProductos.Margin = new System.Windows.Forms.Padding(0, 5, 0, 10);
            this.ComprasDgvProductos.Name = "ComprasDgvProductos";
            this.ComprasDgvProductos.ResaltarFilasAlPasar = true;
            this.ComprasDgvProductos.Size = new System.Drawing.Size(751, 359);
            this.ComprasDgvProductos.TabIndex = 1;
            //
            // ComprasColsku
            //
            this.ComprasColsku.DataPropertyName = "sku";
            this.ComprasColsku.HeaderText = "SKU";
            this.ComprasColsku.MinimumWidth = 45;
            this.ComprasColsku.Name = "ComprasColsku";
            this.ComprasColsku.ReadOnly = true;
            this.ComprasColsku.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // ComprasColproducto
            //
            this.ComprasColproducto.DataPropertyName = "producto";
            this.ComprasColproducto.FillWeight = 180F;
            this.ComprasColproducto.HeaderText = "Producto";
            this.ComprasColproducto.MinimumWidth = 45;
            this.ComprasColproducto.Name = "ComprasColproducto";
            this.ComprasColproducto.ReadOnly = true;
            this.ComprasColproducto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // ComprasColpeso
            //
            this.ComprasColpeso.DataPropertyName = "peso";
            dataGridViewCellStyle1.Format = "N2";
            this.ComprasColpeso.DefaultCellStyle = dataGridViewCellStyle1;
            this.ComprasColpeso.HeaderText = "Peso";
            this.ComprasColpeso.MinimumWidth = 45;
            this.ComprasColpeso.Name = "ComprasColpeso";
            this.ComprasColpeso.ReadOnly = true;
            this.ComprasColpeso.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // ComprasColcantidad
            //
            this.ComprasColcantidad.DataPropertyName = "cantidad";
            this.ComprasColcantidad.HeaderText = "Cantidad";
            this.ComprasColcantidad.MinimumWidth = 45;
            this.ComprasColcantidad.Name = "ComprasColcantidad";
            this.ComprasColcantidad.ReadOnly = true;
            this.ComprasColcantidad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // ComprasColprecio
            //
            this.ComprasColprecio.DataPropertyName = "precio";
            dataGridViewCellStyle2.Format = "N2";
            this.ComprasColprecio.DefaultCellStyle = dataGridViewCellStyle2;
            this.ComprasColprecio.HeaderText = "Precio unitario";
            this.ComprasColprecio.MinimumWidth = 45;
            this.ComprasColprecio.Name = "ComprasColprecio";
            this.ComprasColprecio.ReadOnly = true;
            this.ComprasColprecio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // ComprasColdescuento
            //
            this.ComprasColdescuento.DataPropertyName = "descuento";
            dataGridViewCellStyle3.Format = "N2";
            this.ComprasColdescuento.DefaultCellStyle = dataGridViewCellStyle3;
            this.ComprasColdescuento.HeaderText = "Desc. por unidad";
            this.ComprasColdescuento.MinimumWidth = 45;
            this.ComprasColdescuento.Name = "ComprasColdescuento";
            this.ComprasColdescuento.ReadOnly = true;
            this.ComprasColdescuento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // ComprasColporcentaje
            //
            this.ComprasColporcentaje.DataPropertyName = "porcentaje";
            dataGridViewCellStyle4.Format = "N2";
            this.ComprasColporcentaje.DefaultCellStyle = dataGridViewCellStyle4;
            this.ComprasColporcentaje.HeaderText = "Desc. %";
            this.ComprasColporcentaje.MinimumWidth = 45;
            this.ComprasColporcentaje.Name = "ComprasColporcentaje";
            this.ComprasColporcentaje.ReadOnly = true;
            this.ComprasColporcentaje.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // ComprasColtotal
            //
            this.ComprasColtotal.DataPropertyName = "total";
            dataGridViewCellStyle5.Format = "N2";
            this.ComprasColtotal.DefaultCellStyle = dataGridViewCellStyle5;
            this.ComprasColtotal.HeaderText = "Total";
            this.ComprasColtotal.MinimumWidth = 45;
            this.ComprasColtotal.Name = "ComprasColtotal";
            this.ComprasColtotal.ReadOnly = true;
            this.ComprasColtotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // ComprasTlpDetalles
            //
            this.ComprasTlpDetalles.ColumnCount = 1;
            this.ComprasTlpDetalles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Controls.Add(this.ComprasTlpNavegacion, 0, 0);
            this.ComprasTlpDetalles.Controls.Add(this.ComprasDgvProductos, 0, 1);
            this.ComprasTlpDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpDetalles.Location = new System.Drawing.Point(6, 27);
            this.ComprasTlpDetalles.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpDetalles.Name = "ComprasTlpDetalles";
            this.ComprasTlpDetalles.Padding = new System.Windows.Forms.Padding(5);
            this.ComprasTlpDetalles.RowCount = 2;
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Size = new System.Drawing.Size(761, 464);
            this.ComprasTlpDetalles.TabIndex = 0;
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
            this.ComprasTlpNavegacion.Location = new System.Drawing.Point(5, 5);
            this.ComprasTlpNavegacion.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpNavegacion.Name = "ComprasTlpNavegacion";
            this.ComprasTlpNavegacion.RowCount = 1;
            this.ComprasTlpNavegacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpNavegacion.Size = new System.Drawing.Size(751, 80);
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
            this.ComprasFlpAccionesProductos.AutoScroll = true;
            this.ComprasFlpAccionesProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasFlpAccionesProductos.Location = new System.Drawing.Point(80, 0);
            this.ComprasFlpAccionesProductos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpAccionesProductos.Name = "ComprasFlpAccionesProductos";
            this.ComprasFlpAccionesProductos.Size = new System.Drawing.Size(591, 80);
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
            this.ComprasBtnBodegaSiguiente.Location = new System.Drawing.Point(671, 0);
            this.ComprasBtnBodegaSiguiente.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnBodegaSiguiente.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaSiguiente.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaSiguiente.Name = "ComprasBtnBodegaSiguiente";
            this.ComprasBtnBodegaSiguiente.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnBodegaSiguiente.TabIndex = 2;
            this.ComprasBtnBodegaSiguiente.UseVisualStyleBackColor = false;
            //
            // ComprasGbxProductos
            //
            this.ComprasGbxProductos.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxProductos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasGbxProductos.Controls.Add(this.ComprasTlpDetalles);
            this.ComprasGbxProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxProductos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxProductos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxProductos.Location = new System.Drawing.Point(0, 0);
            this.ComprasGbxProductos.MostrarBorde = false;
            this.ComprasGbxProductos.Name = "ComprasGbxProductos";
            this.ComprasGbxProductos.Padding = new System.Windows.Forms.Padding(6);
            this.ComprasGbxProductos.Size = new System.Drawing.Size(773, 497);
            this.ComprasGbxProductos.TabIndex = 0;
            this.ComprasGbxProductos.TabStop = false;
            //
            // ComprasUsrProductosOrdenCompra
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.ComprasGbxProductos);
            this.Name = "ComprasUsrProductosOrdenCompra";
            this.Size = new System.Drawing.Size(773, 497);
            this.ComprasTlpDetalles.ResumeLayout(false);
            this.ComprasTlpNavegacion.ResumeLayout(false);
            this.ComprasGbxProductos.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
