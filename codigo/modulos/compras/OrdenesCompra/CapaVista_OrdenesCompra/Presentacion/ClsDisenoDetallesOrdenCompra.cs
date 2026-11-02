using System;
using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Ajusta la altura del bloque de detalles al contenido de su DGV.
 * ===================================================================
*/










namespace CapaVista_OrdenesCompra.Presentacion
{
    internal sealed class ClsDisenoDetallesOrdenCompra : IDisposable
    {
        private const int _AlturaMinimaArticulo = 40;
        private readonly Form _Formulario;
        private readonly ComprasUsrDataGridView _Productos;
        private readonly Panel _Contenido;
        private readonly TableLayoutPanel _Principal;
        private readonly TableLayoutPanel _Detalles;
        private readonly Control _GrupoDetalles;
        private bool _AjustandoAltura;
        private bool _Liberado;

        
        
        
        
        internal ClsDisenoDetallesOrdenCompra(Form Formulario, ComprasUsrDataGridView Productos,
            Panel Contenido, TableLayoutPanel Principal, TableLayoutPanel Detalles, Control GrupoDetalles)
        {
            _Formulario = Formulario;
            _Productos = Productos;
            _Contenido = Contenido;
            _Principal = Principal;
            _Detalles = Detalles;
            _GrupoDetalles = GrupoDetalles;
            var Tabla = _Productos.Tabla;
            Tabla.RowTemplate.MinimumHeight = _AlturaMinimaArticulo;
            Tabla.RowTemplate.Height = Math.Max(Tabla.RowTemplate.Height, _AlturaMinimaArticulo);
            Tabla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _Contenido.MinimumSize = Size.Empty;
            _Principal.RowStyles[2].SizeType = SizeType.Absolute;
            _Detalles.RowStyles[1].SizeType = SizeType.Absolute;
            Tabla.RowsAdded += ComprasProcActualizar;
            Tabla.RowsRemoved += ComprasProcActualizar;
            Tabla.RowHeightChanged += ComprasProcActualizar;
            Tabla.ColumnHeadersHeightChanged += ComprasProcActualizar;
            Tabla.DataBindingComplete += ComprasProcActualizar;
            Tabla.SizeChanged += ComprasProcActualizar;
            _Formulario.Shown += ComprasProcActualizar;
            _Formulario.Disposed += ComprasProcLiberar;
            ComprasProcAjustarAlturaArticulos();
        }

        
        
        
        private void ComprasProcActualizar(object Remitente, EventArgs Evento)
        {
            ComprasProcAjustarAlturaArticulos();
        }

        
        
        
        private void ComprasProcLiberar(object Remitente, EventArgs Evento)
        {
            Dispose();
        }

        
        
        
        
        public void Dispose()
        {
            if (_Liberado)
                return;
            _Liberado = true;
            var Tabla = _Productos.Tabla;
            Tabla.RowsAdded -= ComprasProcActualizar;
            Tabla.RowsRemoved -= ComprasProcActualizar;
            Tabla.RowHeightChanged -= ComprasProcActualizar;
            Tabla.ColumnHeadersHeightChanged -= ComprasProcActualizar;
            Tabla.DataBindingComplete -= ComprasProcActualizar;
            Tabla.SizeChanged -= ComprasProcActualizar;
            _Formulario.Shown -= ComprasProcActualizar;
            _Formulario.Disposed -= ComprasProcLiberar;
        }

        
        
        
        
        private void ComprasProcAjustarAlturaArticulos()
        {
            if (_AjustandoAltura || _Liberado || _Formulario.IsDisposed) return;
            _AjustandoAltura = true;
            try
            {
                var Tabla = _Productos.Tabla;
                foreach (System.Windows.Forms.DataGridViewRow Fila in Tabla.Rows)
                {
                    if (Fila.MinimumHeight != _AlturaMinimaArticulo)
                        Fila.MinimumHeight = _AlturaMinimaArticulo;
                }
                int Altura = Tabla.ColumnHeadersVisible ? Tabla.ColumnHeadersHeight : 0;
                Altura += Tabla.Rows.GetRowsHeight(System.Windows.Forms.DataGridViewElementStates.Visible);
                Altura += _Productos.Height - Tabla.ClientSize.Height;
                foreach (System.Windows.Forms.Control Control in Tabla.Controls)
                {
                    if (Control is System.Windows.Forms.HScrollBar && Control.Visible)
                        Altura += Control.Height;
                }
                _Detalles.RowStyles[1].Height = Math.Max(1, Altura);
                int AlturaDetalles = (int)_Detalles.RowStyles[0].Height + Math.Max(1, Altura)
                    + _Detalles.Margin.Vertical + _GrupoDetalles.Margin.Vertical
                    + _GrupoDetalles.Height - _GrupoDetalles.DisplayRectangle.Height;
                _Principal.RowStyles[2].Height = AlturaDetalles;
                _Contenido.Height = (int)_Principal.RowStyles[0].Height
                    + (int)_Principal.RowStyles[1].Height
                    + AlturaDetalles + (int)_Principal.RowStyles[3].Height
                    + _Principal.Padding.Vertical + _Contenido.Padding.Vertical;
            }
            finally
            {
                _AjustandoAltura = false;
            }
        }
    }
}
