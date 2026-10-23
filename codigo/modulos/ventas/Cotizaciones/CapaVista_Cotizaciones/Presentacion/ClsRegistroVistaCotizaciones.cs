// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using CapaControlador_Cotizaciones;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_Cotizaciones
{
    internal sealed class ClsRegistroVistaCotizaciones
    {
        private readonly ClsCotizacionesPresentador _Estado;
        internal ClsRegistroVistaCotizaciones(ClsCotizacionesPresentador Estado)
        {
            _Estado = Estado;
        }

        // RegistroVistaCotizaciones.
        internal void VentasMetGuardar()
        {
            string Estado = _Estado.Vista.VentasCboCotEstado.Text == "Borrador" ? "BORRADOR" : _Estado.Vista.VentasCboCotEstado.Text == "Vigente" ? "EMITIDA" : "";
            string Numero = _Estado.Controlador.VentasMetGuardar(_Estado.IdCotizacion, _Estado.Vista.VentasTxtCotNumero.Text, ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboCotCliente), ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboCotVendedor), ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboCotMoneda), _Estado.VentasIdUsuario, _Estado.Vista.VentasDtpCotFecha.Value, _Estado.Vista.VentasDtpCotVigencia.Value, _Estado.Vista.VentasNudCotTipoCambio.Value, Estado, _Estado.Lineas.Copy());
            VentasMetConsultar(Numero);
            Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(_Estado.Vista, "Cotizacion guardada: " + Numero);
        }

        internal void VentasMetConsultar(string Numero)
        {
            _Estado.Controlador.VentasMetValidarCaptura(!(string.IsNullOrWhiteSpace(Numero)), "Ingrese el numero de cotizacion a consultar.");
            DataSet D = _Estado.Controlador.VentasMetConsultar(Numero);
            DataRow C = D.Tables[0].Rows[0];
            _Estado.Cargando = true;
            try
            {
                _Estado.IdCotizacion = Convert.ToInt32(C["idCotizacion"]);
                _Estado.EstadoGuardado = Convert.ToString(C["estadoCotizacion"]);
                _Estado.NumeroGuardado = Convert.ToString(C["numeroCotizacion"]);
                _Estado.Vista.VentasTxtCotNumero.Text = _Estado.NumeroGuardado;
                _Estado.Vista.VentasDtpCotFecha.Value = Convert.ToDateTime(C["fechaEmision"]);
                _Estado.Vista.VentasDtpCotVigencia.Value = Convert.ToDateTime(C["vigenteHasta"]);
                _Estado.Vista.VentasCboCotCliente.SelectedValue = Convert.ToInt32(C["idCliente"]);
                _Estado.Vista.VentasCboCotMoneda.SelectedValue = Convert.ToInt32(C["idMoneda"]);
                _Estado.Vista.VentasCboCotVendedor.SelectedValue = Convert.ToInt32(C["idVendedor"]);
                DataRow Cliente = ClsHerramientasVentas.VentasFuncObtenerFila(_Estado.Vista.VentasCboCotCliente);
                _Estado.Vista.VentasTxtCotNit.Text = Cliente == null ? "" : Convert.ToString(Cliente["numeroIdentificacion"]);
                _Estado.Vista.VentasTxtCotDireccion.Text = Cliente == null ? "" : Convert.ToString(Cliente["direccionCliente"]);
                _Estado.Vista.VentasTxtCotContacto.Text = Cliente == null ? "" : Convert.ToString(Cliente["contactoCliente"]);
                _Estado.Vista.VentasNudCotTipoCambio.Value = Convert.ToDecimal(C["tipoCambio"]);
                _Estado.Vista.VentasTxtCotListaPrecios.Text = Convert.ToString(C["nombreListaPrecio"]);
                string Visual = Convert.ToString(C["EstadoVisual"]);
                _Estado.Vista.VentasCboCotEstado.SelectedIndex = Visual == "BORRADOR" ? 0 : Visual == "EMITIDA" || Visual == "ACEPTADA" ? 1 : Visual == "VENCIDA" ? 2 : Visual == "CONVERTIDA" ? 3 : 4;
                _Estado.Lineas.Rows.Clear();
                foreach (DataRow F in D.Tables[1].Rows)
                    _Estado.Lineas.Rows.Add(F["idProducto"], F["descripcionItem"], F["tipoItem"], F["cantidad"], F["precioUnitario"], F["descuentoMonto"], F["porcentajeImpuesto"], Math.Round((decimal)F["cantidad"] * (decimal)F["precioUnitario"], 2, MidpointRounding.AwayFromZero), F["impuestoMonto"], F["totalLinea"]);
            }
            finally
            {
                _Estado.Cargando = false;
            }

            _Estado.EditandoCotizacion = false;
            _Estado.VentasMetRestablecerLinea();
            _Estado.VentasMetMostrarLineas();
            _Estado.VentasMetMostrarVigencia();
            if (VentasMetEditable())
                _Estado.VentasMetPrecios();
        }

        internal void VentasMetBuscar()
        {
            DataTable Datos = _Estado.Controlador.VentasMetListar(_Estado.Vista.VentasTxtCotNumero.Text.Trim());
            DataRow F = Datos.Rows.Count == 1 ? Datos.Rows[0] : ClsHerramientasVentas.VentasMetSeleccionar(_Estado.Vista, "Consultar cotizaciones", Datos);
            if (F != null)
                VentasMetConsultar(Convert.ToString(F["Numero"]));
        }

        internal void VentasMetModificar()
        {
            if (_Estado.IdCotizacion == 0)
                VentasMetBuscar();
            _Estado.Controlador.VentasMetValidarCaptura(_Estado.IdCotizacion > 0, "Seleccione una cotizacion guardada.");
            _Estado.Controlador.VentasMetValidarEdicion(_Estado.IdCotizacion, _Estado.EstadoGuardado, _Estado.Vista.VentasDtpCotVigencia.Value);
            _Estado.EditandoCotizacion = true;
            _Estado.VentasMetPrecios();
            VentasMetActualizarEdicion();
            _Estado.Vista.VentasCboCotProducto.Focus();
        }

        internal void VentasMetAnular()
        {
            if (_Estado.IdCotizacion == 0)
                VentasMetBuscar();
            _Estado.Controlador.VentasMetValidarCaptura(_Estado.IdCotizacion > 0, "Consulte la cotizacion a anular.");
            if (Ventas.Estandarizacion.ClsDialogosVentas.VentasMetConfirmar(_Estado.Vista, "¿Desea anular la cotizacion " + _Estado.NumeroGuardado + "?", "Anular cotizacion") != DialogResult.Yes)
                return;
            _Estado.Controlador.VentasMetAnular(_Estado.IdCotizacion, _Estado.VentasIdUsuario);
            VentasMetConsultar(_Estado.NumeroGuardado);
            Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(_Estado.Vista, "Cotizacion anulada.");
        }

        internal bool VentasMetEditable()
        {
            return _Estado.EditandoCotizacion && _Estado.Controlador.VentasMetPuedeEditar(_Estado.IdCotizacion, _Estado.EstadoGuardado, _Estado.Vista.VentasDtpCotVigencia.Value);
        }

        internal void VentasMetActualizarEdicion()
        {
            bool Editable = VentasMetEditable();
            _Estado.Vista.VentasBtnCotModificar.Enabled = _Estado.IdCotizacion > 0 && _Estado.Controlador.VentasMetPuedeEditar(_Estado.IdCotizacion, _Estado.EstadoGuardado, _Estado.Vista.VentasDtpCotVigencia.Value);
            _Estado.Vista.VentasBtnCotAnular.Enabled = _Estado.IdCotizacion > 0 && (_Estado.EstadoGuardado == "BORRADOR" || _Estado.EstadoGuardado == "EMITIDA" || _Estado.EstadoGuardado == "ACEPTADA");
            _Estado.Vista.VentasBtnCotGuardar.Enabled = Editable;
            _Estado.Vista.VentasBtnCotAgregar.Enabled = Editable;
            _Estado.Vista.VentasBtnCotEliminar.Enabled = Editable;
            _Estado.Vista.VentasPnlCotProducto.Enabled = Editable;
            _Estado.Vista.VentasCboCotCliente.Enabled = Editable && _Estado.Lineas.Rows.Count == 0;
            _Estado.Vista.VentasCboCotMoneda.Enabled = Editable && _Estado.Lineas.Rows.Count == 0;
            _Estado.Vista.VentasCboCotVendedor.Enabled = Editable;
            _Estado.Vista.VentasCboCotEstado.Enabled = Editable;
            _Estado.Vista.VentasDtpCotFecha.Enabled = Editable && _Estado.Lineas.Rows.Count == 0;
            _Estado.Vista.VentasDtpCotVigencia.Enabled = Editable;
            _Estado.Vista.VentasNudCotTipoCambio.Enabled = Editable;
            bool Vigente = _Estado.IdCotizacion > 0 && (_Estado.EstadoGuardado == "EMITIDA" || _Estado.EstadoGuardado == "ACEPTADA") && _Estado.Vista.VentasDtpCotFecha.Value.Date <= DateTime.Today && _Estado.Vista.VentasDtpCotVigencia.Value.Date >= DateTime.Today;
            _Estado.Vista.VentasBtnCotPedido.Enabled = Vigente;
            _Estado.Vista.VentasBtnCotFactura.Enabled = Vigente;
        }

        internal void VentasMetRefrescar()
        {
            if (_Estado.IdCotizacion > 0)
                VentasMetConsultar(_Estado.NumeroGuardado);
            else
                _Estado.VentasMetCargar();
        }

        // ConversionVistaCotizaciones.
        internal void VentasMetConvertir(bool AFactura)
        {
            string Numero = _Estado.Controlador.VentasMetConvertir(_Estado.IdCotizacion, AFactura, ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboCotEstablecimiento), _Estado.VentasIdUsuario);
            VentasMetConsultar(_Estado.NumeroGuardado);
            Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(_Estado.Vista, AFactura ? "Factura borrador preparada con los precios originales: " + Numero + ". Su emision e inventarios corresponden a Facturas." : "Pedido creado con los precios originales: " + Numero);
        }
    }
}
