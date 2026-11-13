using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Presenta y administra los productos de la orden agrupados por bodega.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    public partial class UsrProductosOrdenesCompra : UserControl
    {
        private readonly ClsProductosOrdenesCompraTabla _Productos = new ClsProductosOrdenesCompraTabla();
        private DataView _ProductosBodega;
        private int? _BodegaVisible;
        private bool _ActualizandoDetalle;
        private readonly Presentacion.ClsProductosOrdenesCompraPaginacion _ScrollTabla;
        public event EventHandler ProductoSeleccionado;
        internal ComprasUsrDataGridView Tabla => ComprasDgvProductos;
        internal DataTable Detalles => _Productos.Detalles;
        internal int? BodegaVisible { get => _BodegaVisible; set => _BodegaVisible = value; }
        internal decimal Bruto => _Productos.Bruto;
        internal decimal Descuento => _Productos.Descuento;
        internal DataRow Seleccionado
        {
            get
            {
                var Fila = ComprasDgvProductos.SelectedRows.Count == 0 ? null
                    : ComprasDgvProductos.SelectedRows[0].DataBoundItem as DataRowView;
                return Fila == null ? null : Detalles.Rows.Find(Fila["idLinea"]);
            }
        }

        public UsrProductosOrdenesCompra()
        {
            InitializeComponent();
            ComprasProcDesactivarScrollInterno(this);
            ComprasFlpAccionesProductos.SizeChanged += (Remitente, Evento) => ComprasProcAjustarPestanasVisibles();
            ComprasProcConfigurarColumnas();
            _ScrollTabla = new Presentacion.ClsProductosOrdenesCompraPaginacion(ComprasDgvProductos);
            _ProductosBodega = new DataView(Detalles) { RowFilter = "1 = 0" };
            ComprasDgvProductos.DataSource = _ProductosBodega;
            ComprasDgvProductos.SelectionChanged += (Remitente, Evento) =>
            {
                if (!_ActualizandoDetalle) ProductoSeleccionado?.Invoke(this, EventArgs.Empty);
            };
            ComprasBtnBodegaAnterior.Click += (Remitente, Evento) => ComprasProcCambiarPestanaBodega(-1);
            ComprasBtnBodegaSiguiente.Click += (Remitente, Evento) => ComprasProcCambiarPestanaBodega(1);
        }

        private static void ComprasProcDesactivarScrollInterno(Control Contenedor)
        {
            var Panel = Contenedor as ScrollableControl;
            if (Panel != null) Panel.AutoScroll = false;
            foreach (Control Hijo in Contenedor.Controls) ComprasProcDesactivarScrollInterno(Hijo);
        }

        private void ComprasProcAjustarPestanasVisibles()
        {
            ClsProductosOrdenesCompraPestanas.Ajustar(ComprasFlpAccionesProductos, _BodegaVisible);
        }

        private void ComprasProcConfigurarColumnas()
        {
            ClsProductosOrdenesCompraTabla.ConfigurarColumnas(ComprasDgvProductos);
        }

        internal void Guardar(DataRow Editado, DataRow Producto, DataRow Bodega, int Cantidad,
            decimal CostoBase, decimal TipoCambio, decimal Costo, decimal Porcentaje, string NombreSucursal = null)
        {
            _Productos.Guardar(Editado, Producto, Bodega, Cantidad, CostoBase, TipoCambio, Costo, Porcentaje, NombreSucursal);
        }

        internal void Eliminar(DataRow Detalle)
        {
            _Productos.Eliminar(Detalle);
        }

        internal void Recalcular(decimal? TipoCambio, string Simbolo)
        {
            _ActualizandoDetalle = true;
            try
            {
                _Productos.Recalcular(TipoCambio);
                foreach (DataGridViewColumn Columna in ComprasDgvProductos.Columns)
                    if (Columna.DataPropertyName == "precio" || Columna.DataPropertyName == "descuento" || Columna.DataPropertyName == "descuentoTotal" || Columna.DataPropertyName == "total")
                        Columna.DefaultCellStyle.Format = "'" + Simbolo.Replace("'", "''") + "' #,##0.00";
            }
            finally { _ActualizandoDetalle = false; }
        }

        internal void ComprasProcActualizarPestanasBodega()
        {
            bool ActualizandoAntes = _ActualizandoDetalle;
            _ActualizandoDetalle = true;
            try
            {
                _BodegaVisible = ClsProductosOrdenesCompraPestanas.Actualizar(ComprasFlpAccionesProductos,
                    ComprasBtnBodegaAnterior, ComprasBtnBodegaSiguiente, Detalles, _BodegaVisible, Id =>
                    {
                        _BodegaVisible = Id;
                        ComprasProcActualizarPestanasBodega();
                    });
                _ProductosBodega.RowFilter = _BodegaVisible.HasValue ? "idBodega = " + _BodegaVisible.Value : "1 = 0";
                ComprasDgvProductos.ClearSelection();
            }
            finally { _ActualizandoDetalle = ActualizandoAntes; }
            if (!ActualizandoAntes) ProductoSeleccionado?.Invoke(this, EventArgs.Empty);
        }

        private void ComprasProcCambiarPestanaBodega(int Direccion)
        {
            for (int Indice = 0; Indice < ComprasFlpAccionesProductos.Controls.Count; Indice++)
            {
                if ((int)ComprasFlpAccionesProductos.Controls[Indice].Tag != _BodegaVisible) continue;
                int Destino = Indice + Direccion;
                if (Destino < 0 || Destino >= ComprasFlpAccionesProductos.Controls.Count) return;
                _BodegaVisible = (int)ComprasFlpAccionesProductos.Controls[Destino].Tag;
                ComprasProcActualizarPestanasBodega();
                return;
            }
        }
    }
}
