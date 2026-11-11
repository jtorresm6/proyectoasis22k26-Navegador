// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using CapaControlador_Cotizaciones;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_Cotizaciones
{
    internal sealed class ClsEventosCotizaciones
    {
        private readonly ClsCotizacionesPresentador _Estado;

        internal ClsEventosCotizaciones(ClsCotizacionesPresentador Estado)
        {
            _Estado = Estado;
        }

        internal void VentasMetConectarEventos()
        {
            ClsHerramientasVentas.VentasMetFormatearFechas(_Estado.Vista.VentasDgvCotProductos);
            _Estado.Vista.Load += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.VentasMetCargar);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnCotIngresar, _Estado.VentasMetNuevo);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnCotModificar, _Estado.RegistroVistaCotizaciones.VentasMetModificar);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnCotAnular, _Estado.RegistroVistaCotizaciones.VentasMetAnular);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnCotRefrescar, _Estado.RegistroVistaCotizaciones.VentasMetRefrescar);
            _Estado.Vista.VentasDgvCotProductos.CellDoubleClick += (Origen, Evento) =>
            {
                if (Evento.RowIndex >= 0)
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.VentasMetEditarLinea);
            };
            VentasMetConectarBoton(_Estado.Vista.VentasBtnCotAgregar, _Estado.VentasMetAgregar);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnCotGuardar, _Estado.RegistroVistaCotizaciones.VentasMetGuardar);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnCotLimpiar, _Estado.VentasMetNuevo);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnCotConsultar, _Estado.RegistroVistaCotizaciones.VentasMetBuscar);
            _Estado.Vista.VentasTxtCotNumero.KeyDown += (Origen, Evento) =>
            {
                if (Evento.KeyCode == Keys.Enter)
                {
                    Evento.SuppressKeyPress = true;
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.RegistroVistaCotizaciones.VentasMetBuscar);
                }
            };
            _Estado.Vista.VentasBtnCotPedido.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, () => _Estado.RegistroVistaCotizaciones.VentasMetConvertir(false));
            _Estado.Vista.VentasBtnCotFactura.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, () => _Estado.RegistroVistaCotizaciones.VentasMetConvertir(true));
            _Estado.Vista.VentasBtnCotEliminar.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, () =>
            {
                if (_Estado.Vista.VentasDgvCotProductos.CurrentRow != null && _Estado.RegistroVistaCotizaciones.VentasFuncEditable())
                {
                    _Estado.Lineas.Rows.RemoveAt(_Estado.Vista.VentasDgvCotProductos.CurrentRow.Index);
                    _Estado.VentasMetRestablecerLinea();
                    _Estado.VentasMetMostrarLineas();
                }
            });
            _Estado.Vista.VentasBtnCotCerrar.Click += (Origen, Evento) => _Estado.Vista.Close();
            _Estado.Vista.VentasBtnCotImprimir.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, () =>
            {
                _Estado.Controlador.VentasMetValidarCaptura(!(_Estado.IdCotizacion <= 0), "Guarde la cotizacion antes de imprimir.");
                ClsHerramientasVentas.VentasMetImprimir(_Estado.Vista, "Cotizacion " + _Estado.NumeroGuardado, _Estado.Vista.VentasCboCotCliente.Text + " / " + _Estado.Vista.VentasCboCotMoneda.Text + " / " + _Estado.Vista.VentasCboCotEstado.Text + " / Vigencia " + _Estado.Vista.VentasDtpCotVigencia.Value.ToString("dd/MM/yyyy"), _Estado.Lineas);
            });
            _Estado.Vista.VentasCboCotCliente.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!_Estado.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.VentasMetCliente);
            };
            _Estado.Vista.VentasCboCotMoneda.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!_Estado.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.VentasMetPrecios);
            };
            _Estado.Vista.VentasDtpCotFecha.ValueChanged += (Origen, Evento) =>
            {
                if (!_Estado.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.VentasMetPrecios);
            };
            _Estado.Vista.VentasCboCotProducto.SelectedIndexChanged += (Origen, Evento) =>
            {
                DataRow ProductoSeleccionado = ClsHerramientasVentas.VentasFuncObtenerFila(_Estado.Vista.VentasCboCotProducto);
                _Estado.Vista.VentasTxtCotPrecio.Text = ProductoSeleccionado == null ? "" : Convert.ToDecimal(ProductoSeleccionado["Precio"]).ToString("N4", ClsHerramientasVentas.Cultura);
            };
            _Estado.Vista.VentasTxtCotNit.ReadOnly = true;
            _Estado.Vista.VentasTxtCotContacto.ReadOnly = true;
            _Estado.Vista.VentasTxtCotDireccion.ReadOnly = true;
            _Estado.Vista.VentasNudCotCantidad.DecimalPlaces = 4;
            _Estado.Vista.VentasNudCotCantidad.Minimum = 0.0001m;
            _Estado.Vista.VentasNudCotCantidad.Value = 1;
            _Estado.Vista.VentasTtpAcciones.SetToolTip(_Estado.Vista.VentasNudCotDescuento, "Descuento como importe de la linea, en la moneda de la cotizacion.");
            _Estado.Vista.VentasTtpAcciones.SetToolTip(_Estado.Vista.VentasTxtCotNumero, "Numero o vacio para generar correlativo. Enter para consultar una cotizacion guardada.");
        }
        private void VentasMetConectarBoton(Button Boton, Action Accion)
        {
            Boton.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, Accion);
        }
    }
}
