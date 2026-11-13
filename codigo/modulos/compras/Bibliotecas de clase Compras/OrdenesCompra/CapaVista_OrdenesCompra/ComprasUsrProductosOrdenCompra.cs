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
    public partial class ComprasUsrProductosOrdenCompra : UserControl
    {
        private readonly ClsProductosOrdenCompra _Productos = new ClsProductosOrdenCompra();
        private DataView _ProductosBodega;
        private int? _BodegaVisible;
        private bool _ActualizandoDetalle;
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

        public ComprasUsrProductosOrdenCompra()
        {
            InitializeComponent();
            _ProductosBodega = new DataView(Detalles) { RowFilter = "1 = 0" };
            ComprasDgvProductos.DataSource = _ProductosBodega;
            ComprasDgvProductos.SelectionChanged += (Remitente, Evento) =>
            {
                if (!_ActualizandoDetalle) ProductoSeleccionado?.Invoke(this, EventArgs.Empty);
            };
            ComprasBtnBodegaAnterior.Click += (Remitente, Evento) => ComprasProcCambiarPestanaBodega(-1);
            ComprasBtnBodegaSiguiente.Click += (Remitente, Evento) => ComprasProcCambiarPestanaBodega(1);
        }

        internal void Guardar(DataRow Editado, DataRow Producto, DataRow Bodega, int Cantidad,
            decimal CostoBase, decimal Descuento, decimal TipoCambio, decimal Costo)
        {
            _Productos.Guardar(Editado, Producto, Bodega, Cantidad, CostoBase, Descuento, TipoCambio, Costo);
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
                    if (Columna.DataPropertyName == "precio" || Columna.DataPropertyName == "descuento" || Columna.DataPropertyName == "total")
                        Columna.DefaultCellStyle.Format = "'" + Simbolo.Replace("'", "''") + "' #,##0.00";
            }
            finally { _ActualizandoDetalle = false; }
        }

        internal void ComprasProcActualizarPestanasBodega()
        {
            bool ActualizandoAntes = _ActualizandoDetalle;
            _ActualizandoDetalle = true;
            while (ComprasFlpAccionesProductos.Controls.Count > 0)
            {
                Control Anterior = ComprasFlpAccionesProductos.Controls[0];
                ComprasFlpAccionesProductos.Controls.Remove(Anterior);
                Anterior.Dispose();
            }
            var Bodegas = new System.Collections.Generic.Dictionary<int, string>();
            foreach (DataRow Fila in Detalles.Rows)
                Bodegas[Convert.ToInt32(Fila["idBodega"])] = Convert.ToString(Fila["bodega"]);
            if (!_BodegaVisible.HasValue || !Bodegas.ContainsKey(_BodegaVisible.Value))
            {
                _BodegaVisible = null;
                foreach (int Id in Bodegas.Keys) { _BodegaVisible = Id; break; }
            }
            foreach (var Bodega in Bodegas)
            {
                int Id = Bodega.Key;
                var Boton = new ClsButtonCompras
                {
                    TipoBoton = ComprasTipoBoton.Libre, EsActivo = _BodegaVisible == Id,
                    Name = "ComprasTabBodega" + Id, Text = Bodega.Value, Tag = Id, AutoSize = true,
                    Height = 80, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    MinimumSize = new Size(0, 80), MaximumSize = new Size(0, 80),
                    FlatStyle = FlatStyle.Flat, Margin = new Padding(2, 0, 2, 0),
                    TabIndex = ComprasFlpAccionesProductos.Controls.Count,
                    UseVisualStyleBackColor = false,
                    BackColor = _BodegaVisible == Id ? ClsTemaCompras.PrincipalNavegador : ClsTemaCompras.FondoBotones,
                    ForeColor = _BodegaVisible == Id ? Color.White : ClsTemaCompras.PrincipalNavegador
                };
                Boton.FlatAppearance.MouseOverBackColor = Boton.BackColor;
                Boton.FlatAppearance.MouseDownBackColor = Boton.BackColor;
                Boton.Click += (Remitente, Evento) =>
                {
                    _BodegaVisible = Id;
                    ComprasProcActualizarPestanasBodega();
                };
                ComprasFlpAccionesProductos.Controls.Add(Boton);
            }
            _ProductosBodega.RowFilter = _BodegaVisible.HasValue ? "idBodega = " + _BodegaVisible.Value : "1 = 0";
            ComprasDgvProductos.ClearSelection();
            int IndiceActivo = -1;
            for (int Indice = 0; Indice < ComprasFlpAccionesProductos.Controls.Count; Indice++)
                if ((int)ComprasFlpAccionesProductos.Controls[Indice].Tag == _BodegaVisible)
                    IndiceActivo = Indice;
            ComprasBtnBodegaAnterior.Enabled = IndiceActivo > 0;
            ComprasBtnBodegaSiguiente.Enabled = IndiceActivo >= 0 && IndiceActivo < ComprasFlpAccionesProductos.Controls.Count - 1;
            if (IndiceActivo >= 0)
                ComprasFlpAccionesProductos.ScrollControlIntoView(ComprasFlpAccionesProductos.Controls[IndiceActivo]);
            _ActualizandoDetalle = ActualizandoAntes;
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
