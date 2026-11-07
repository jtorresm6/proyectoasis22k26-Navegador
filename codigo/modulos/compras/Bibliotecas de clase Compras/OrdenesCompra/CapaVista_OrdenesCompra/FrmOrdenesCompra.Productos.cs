using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

namespace CapaVista_OrdenesCompra
{
    public partial class FrmOrdenesCompra
    {
        private readonly DataTable _DetallesProductos = new DataTable();
        private DataView _ProductosBodega;
        private int? _BodegaVisible;
        private DataRow _ProductoEditado;
        private bool _ActualizandoDetalle;
        private ClsButtonCompras ComprasBtnBodegaAnterior;
        private ClsButtonCompras ComprasBtnBodegaSiguiente;

        private void ComprasProcConfigurarProductos()
        {
            var Clave = _DetallesProductos.Columns.Add("idLinea", typeof(int));
            Clave.AutoIncrement = true;
            Clave.AutoIncrementSeed = 1;
            _DetallesProductos.PrimaryKey = new[] { Clave };
            foreach (string Nombre in new[] { "idProducto", "idBodega", "idSucursal", "cantidad" })
                _DetallesProductos.Columns.Add(Nombre, typeof(int));
            foreach (string Nombre in new[] { "sku", "producto", "descripcion", "bodega" })
                _DetallesProductos.Columns.Add(Nombre, typeof(string));
            foreach (string Nombre in new[] { "peso", "costoBase", "descuentoBase", "porcentaje", "precio", "descuento", "total" })
                _DetallesProductos.Columns.Add(Nombre, typeof(decimal));
            _ProductosBodega = new DataView(_DetallesProductos) { RowFilter = "1 = 0" };
            ComprasDgvProductos.AutoGenerateColumns = false;
            ComprasDgvProductos.ReadOnly = true;
            ComprasDgvProductos.AllowUserToAddRows = false;
            ComprasDgvProductos.AllowUserToDeleteRows = false;
            ComprasDgvProductos.MultiSelect = false;
            ComprasDgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ComprasDgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ComprasDgvProductos.Columns.Clear();
            string[] Campos = { "sku", "producto", "peso", "cantidad", "precio", "descuento", "porcentaje", "total" };
            string[] Titulos = { "SKU", "Producto", "Peso", "Cantidad", "Precio unitario", "Desc. por unidad", "Desc. %", "Total" };
            for (int Indice = 0; Indice < Campos.Length; Indice++)
            {
                var Columna = new DataGridViewTextBoxColumn
                {
                    Name = "ComprasCol" + Campos[Indice], DataPropertyName = Campos[Indice],
                    HeaderText = Titulos[Indice], FillWeight = Indice == 1 ? 180 : 100,
                    MinimumWidth = 45, SortMode = DataGridViewColumnSortMode.NotSortable
                };
                if (Indice >= 4) Columna.DefaultCellStyle.Format = "N2";
                if (Indice == 2) Columna.DefaultCellStyle.Format = "N2";
                ComprasDgvProductos.Columns.Add(Columna);
            }
            ComprasDgvProductos.DataSource = _ProductosBodega;
            var Navegacion = new TableLayoutPanel
            {
                Name = "ComprasTlpNavegacionBodegas", Dock = DockStyle.Fill,
                Margin = Padding.Empty, ColumnCount = 3, RowCount = 1
            };
            Navegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            Navegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Navegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            Navegacion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            ComprasBtnBodegaAnterior = new ClsButtonCompras
            {
                Name = "ComprasBtnBodegaAnterior", TipoBoton = ComprasTipoBoton.Tabla,
                Text = "Anterior", Dock = DockStyle.Fill, Margin = new Padding(2), Enabled = false
            };
            ComprasBtnBodegaSiguiente = new ClsButtonCompras
            {
                Name = "ComprasBtnBodegaSiguiente", TipoBoton = ComprasTipoBoton.Tabla,
                Text = "Siguiente", Dock = DockStyle.Fill, Margin = new Padding(2), Enabled = false
            };
            ComprasTlpDetalles.Controls.Remove(ComprasFlpAccionesProductos);
            ComprasTlpDetalles.Controls.Add(Navegacion, 0, 0);
            Navegacion.Controls.Add(ComprasBtnBodegaAnterior, 0, 0);
            Navegacion.Controls.Add(ComprasFlpAccionesProductos, 1, 0);
            Navegacion.Controls.Add(ComprasBtnBodegaSiguiente, 2, 0);
            ComprasBtnBodegaAnterior.Click += (Remitente, Evento) => ComprasProcCambiarPestanaBodega(-1);
            ComprasBtnBodegaSiguiente.Click += (Remitente, Evento) => ComprasProcCambiarPestanaBodega(1);
            ComprasFlpAccionesProductos.Dock = DockStyle.Fill;
            ComprasFlpAccionesProductos.WrapContents = false;
            ComprasFlpAccionesProductos.AutoScroll = true;
            ComprasBtnInsertarProducto.Click += (Remitente, Evento) => ComprasProcGuardarProducto(false);
            ComprasBtnModificarProducto.Click += (Remitente, Evento) => ComprasProcGuardarProducto(true);
            ComprasBtnEliminarProducto.Click += (Remitente, Evento) => ComprasProcEliminarProducto();
            ComprasDgvProductos.SelectionChanged += (Remitente, Evento) => ComprasProcSeleccionarProductoAgregado();
            ComprasProcEstadoBotonesProducto();
        }

        private void ComprasProcEstadoBotonesProducto()
        {
            bool Seleccionado = _ProductoEditado != null && _ProductoEditado.RowState != DataRowState.Detached;
            ComprasBtnModificarProducto.Enabled = Seleccionado;
            ComprasBtnEliminarProducto.Enabled = Seleccionado;
        }

        private void ComprasProcSeleccionarProductoAgregado()
        {
            if (_ActualizandoDetalle || _Liberado) return;
            var Seleccion = ComprasDgvProductos.SelectedRows.Count == 0 ? null
                : ComprasDgvProductos.SelectedRows[0].DataBoundItem as DataRowView;
            _ProductoEditado = Seleccion == null ? null : _DetallesProductos.Rows.Find(Seleccion["idLinea"]);
            ComprasProcEstadoBotonesProducto();
            if (_ProductoEditado == null) return;
            _ActualizandoDetalle = true;
            try
            {
                DataRow Fila = _ProductoEditado;
                ComprasCboSucursal.SelectedValue = (int)Fila["idSucursal"];
                ComprasCboBodega.SelectedValue = (int)Fila["idBodega"];
                ComprasCboSkuProducto.SelectedValue = (int)Fila["idProducto"];
                _CostoProductoBase = (decimal)Fila["costoBase"];
                ComprasTxtDescripcionProducto.Text = Convert.ToString(Fila["descripcion"]);
                ComprasTxtPesoProducto.Text = ((decimal)Fila["peso"]).ToString("N2");
                ComprasTxtCantidadProducto.Text = Convert.ToString(Fila["cantidad"], CultureInfo.CurrentCulture);
                _DescuentoDesdePorcentaje = false;
                _DescuentoImporteBase = (decimal)Fila["descuentoBase"];
                _DescuentoValido = true;
                ComprasProcActualizarImportesConvertidos();
            }
            finally { _ActualizandoDetalle = false; }
        }

        private void ComprasProcGuardarProducto(bool Modificar)
        {
            if (_CargandoCatalogos || _Liberado) return;
            var Producto = ComprasFuncFila(ComprasCboNombreProducto);
            var Bodega = ComprasFuncFila(ComprasCboBodega);
            _Errores.SetError(ComprasCboNombreProducto, Producto == null ? "Selecciona un producto." : string.Empty);
            _Errores.SetError(ComprasCboBodega, Bodega == null ? "Selecciona una bodega." : string.Empty);
            ComprasProcActualizarDescuentos();
            if (Producto == null || Bodega == null || string.IsNullOrWhiteSpace(ComprasTxtTotalProducto.Text)) return;
            if (Modificar && (_ProductoEditado == null || _ProductoEditado.RowState == DataRowState.Detached))
            {
                _Errores.SetError(ComprasDgvProductos, "Selecciona el producto que deseas modificar.");
                return;
            }
            int Cantidad = int.Parse(ComprasTxtCantidadProducto.Text, CultureInfo.CurrentCulture);
            decimal Costo = decimal.Round(_CostoProductoBase.Value / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
            decimal Descuento = _DescuentoDesdePorcentaje
                ? decimal.Round(Costo * _DescuentoPorcentaje / 100M, 2, MidpointRounding.AwayFromZero)
                : decimal.Round(_DescuentoImporteBase / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
            _ActualizandoDetalle = true;
            try
            {
            DataRow Detalle = Modificar ? _ProductoEditado : _DetallesProductos.NewRow();
            Detalle["idProducto"] = Producto["idProducto"];
            Detalle["idBodega"] = Bodega["idBodega"];
            Detalle["idSucursal"] = Bodega["Fk_Sucursal_Codigo"];
            Detalle["bodega"] = Bodega["nombreBodega"];
            Detalle["sku"] = Producto["skuProducto"];
            Detalle["producto"] = Producto["nombreProducto"];
            Detalle["descripcion"] = Producto["descripcionProducto"];
            Detalle["peso"] = Producto["pesoProducto"];
            Detalle["cantidad"] = Cantidad;
            Detalle["costoBase"] = _CostoProductoBase.Value;
            Detalle["descuentoBase"] = Descuento * TipoCambioSeleccionado.Value;
            Detalle["porcentaje"] = Costo == 0 ? 0M : decimal.Round(Descuento * 100M / Costo, 2, MidpointRounding.AwayFromZero);
            if (!Modificar) _DetallesProductos.Rows.Add(Detalle);
            _ProductoEditado = null;
            _Errores.SetError(ComprasDgvProductos, string.Empty);
            _BodegaVisible = Convert.ToInt32(Bodega["idBodega"]);
            ComprasProcActualizarProductosAgregados();
            ComprasProcActualizarPestanasBodega();
            }
            finally { _ActualizandoDetalle = false; ComprasProcEstadoBotonesProducto(); }
        }

        private void ComprasProcEliminarProducto()
        {
            if (_ProductoEditado == null) return;
            _ActualizandoDetalle = true;
            try
            {
                _DetallesProductos.Rows.Remove(_ProductoEditado);
                _ProductoEditado = null;
                ComprasProcActualizarProductosAgregados();
                ComprasProcActualizarPestanasBodega();
            }
            finally { _ActualizandoDetalle = false; ComprasProcEstadoBotonesProducto(); }
        }

        private void ComprasProcActualizarPestanasBodega()
        {
            bool ActualizandoAntes = _ActualizandoDetalle;
            _ActualizandoDetalle = true;
            Point Scroll = ComprasPnlDesplazamiento.AutoScrollPosition;
            while (ComprasFlpAccionesProductos.Controls.Count > 0)
            {
                Control Anterior = ComprasFlpAccionesProductos.Controls[0];
                ComprasFlpAccionesProductos.Controls.Remove(Anterior);
                Anterior.Dispose();
            }
            var Bodegas = new System.Collections.Generic.Dictionary<int, string>();
            foreach (DataRow Fila in _DetallesProductos.Rows)
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
                    Height = 32, FlatStyle = FlatStyle.Flat, Margin = new Padding(2),
                    UseVisualStyleBackColor = false,
                    BackColor = _BodegaVisible == Id ? ClsTemaCompras.PrincipalNavegador : ClsTemaCompras.FondoBotones,
                    ForeColor = _BodegaVisible == Id ? Color.White : ClsTemaCompras.PrincipalNavegador
                };
                Boton.FlatAppearance.MouseOverBackColor = Boton.BackColor;
                Boton.FlatAppearance.MouseDownBackColor = Boton.BackColor;
                Boton.Click += (Remitente, Evento) =>
                {
                    _BodegaVisible = Id;
                    _ProductoEditado = null;
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
            _ProductoEditado = null;
            _ActualizandoDetalle = ActualizandoAntes;
            ComprasProcEstadoBotonesProducto();
            ComprasPnlDesplazamiento.AutoScrollPosition = new Point(-Scroll.X, -Scroll.Y);
        }

        private void ComprasProcCambiarPestanaBodega(int Direccion)
        {
            for (int Indice = 0; Indice < ComprasFlpAccionesProductos.Controls.Count; Indice++)
            {
                if ((int)ComprasFlpAccionesProductos.Controls[Indice].Tag != _BodegaVisible) continue;
                int Destino = Indice + Direccion;
                if (Destino < 0 || Destino >= ComprasFlpAccionesProductos.Controls.Count) return;
                _BodegaVisible = (int)ComprasFlpAccionesProductos.Controls[Destino].Tag;
                _ProductoEditado = null;
                ComprasProcActualizarPestanasBodega();
                return;
            }
        }

        private void ComprasProcActualizarProductosAgregados()
        {
            if (_ProductosBodega == null || _ActualizandoDetalle && _ProductoEditado != null) return;
            bool ActualizandoAntes = _ActualizandoDetalle;
            _ActualizandoDetalle = true;
            try
            {
            if (!TipoCambioSeleccionado.HasValue || TipoCambioSeleccionado.Value <= 0)
            {
                foreach (DataRow Fila in _DetallesProductos.Rows)
                    foreach (string Campo in new[] { "precio", "descuento", "total" }) Fila[Campo] = DBNull.Value;
                foreach (var Campo in new[] { ComprasTxtSubtotalOrden, ComprasTxtDescuentoTotal, ComprasTxtDescuentoPorcentajeTotal, ComprasTxtSubtotalNeto, ComprasTxtImpuestoTotal, ComprasTxtTotalOrden }) Campo.Text = string.Empty;
                return;
            }
            decimal Bruto = 0M, Descuento = 0M;
            foreach (DataRow Fila in _DetallesProductos.Rows)
            {
                decimal Precio = decimal.Round((decimal)Fila["costoBase"] / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
                decimal Rebaja = decimal.Round((decimal)Fila["descuentoBase"] / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
                int Cantidad = (int)Fila["cantidad"];
                Fila["precio"] = Precio;
                Fila["descuento"] = Rebaja;
                Fila["total"] = Cantidad * (Precio - Rebaja);
                Bruto += Cantidad * Precio;
                Descuento += Cantidad * Rebaja;
            }
            var Moneda = ComprasFuncFila(ComprasCboMoneda);
            string Simbolo = Moneda == null ? string.Empty : Convert.ToString(Moneda["simboloMoneda"]);
            foreach (DataGridViewColumn Columna in ComprasDgvProductos.Columns)
                if (Columna.DataPropertyName == "precio" || Columna.DataPropertyName == "descuento" || Columna.DataPropertyName == "total")
                    Columna.DefaultCellStyle.Format = "'" + Simbolo.Replace("'", "''") + "' #,##0.00";
            _BrutoOrden = Bruto;
            _DescuentoLineasOrden = Descuento;
            ComprasProcActualizarDescuentoTotal();
            }
            finally
            {
                if (!ActualizandoAntes)
                {
                    ComprasDgvProductos.ClearSelection();
                    _ProductoEditado = null;
                    ComprasProcEstadoBotonesProducto();
                }
                _ActualizandoDetalle = ActualizandoAntes;
            }
        }
    }
}
