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
            string Numero = _Estado.Controlador.VentasFuncGuardar(_Estado.IdCotizacion, _Estado.Vista.VentasTxtCotNumero.Text, ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboCotCliente), ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboCotVendedor), ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboCotMoneda), _Estado.VentasIdUsuario, _Estado.Vista.VentasDtpCotFecha.Value, _Estado.Vista.VentasDtpCotVigencia.Value, _Estado.Vista.VentasNudCotTipoCambio.Value, Estado, _Estado.Lineas.Copy());
            VentasMetConsultar(Numero);
            ClsHerramientasVentas.VentasMetInformar(_Estado.Vista, "Cotizacion guardada: " + Numero);
        }

        internal void VentasMetConsultar(string Numero)
        {
            _Estado.Controlador.VentasMetValidarCaptura(!(string.IsNullOrWhiteSpace(Numero)), "Ingrese el numero de cotizacion a consultar.");
            DataSet ConjuntoDatos = _Estado.Controlador.VentasFuncConsultar(Numero);
            DataRow CabeceraCotizacion = ConjuntoDatos.Tables[0].Rows[0];
            _Estado.Cargando = true;
            try
            {
                _Estado.IdCotizacion = Convert.ToInt32(CabeceraCotizacion["idCotizacion"]);
                _Estado.EstadoGuardado = Convert.ToString(CabeceraCotizacion["estadoCotizacion"]);
                _Estado.NumeroGuardado = Convert.ToString(CabeceraCotizacion["numeroCotizacion"]);
                _Estado.Vista.VentasTxtCotNumero.Text = _Estado.NumeroGuardado;
                _Estado.Vista.VentasDtpCotFecha.Value = Convert.ToDateTime(CabeceraCotizacion["fechaEmision"]);
                _Estado.Vista.VentasDtpCotVigencia.Value = Convert.ToDateTime(CabeceraCotizacion["vigenteHasta"]);
                _Estado.Vista.VentasCboCotCliente.SelectedValue = Convert.ToInt32(CabeceraCotizacion["idCliente"]);
                _Estado.Vista.VentasCboCotMoneda.SelectedValue = Convert.ToInt32(CabeceraCotizacion["idMoneda"]);
                _Estado.Vista.VentasCboCotVendedor.SelectedValue = Convert.ToInt32(CabeceraCotizacion["idVendedor"]);
                DataRow Cliente = ClsHerramientasVentas.VentasFuncObtenerFila(_Estado.Vista.VentasCboCotCliente);
                _Estado.Vista.VentasTxtCotNit.Text = Cliente == null ? "" : Convert.ToString(Cliente["numeroIdentificacion"]);
                _Estado.Vista.VentasTxtCotDireccion.Text = Cliente == null ? "" : Convert.ToString(Cliente["direccionCliente"]);
                _Estado.Vista.VentasTxtCotContacto.Text = Cliente == null ? "" : Convert.ToString(Cliente["contactoCliente"]);
                _Estado.Vista.VentasNudCotTipoCambio.Value = Convert.ToDecimal(CabeceraCotizacion["tipoCambio"]);
                _Estado.Vista.VentasTxtCotListaPrecios.Text = Convert.ToString(CabeceraCotizacion["nombreListaPrecio"]);
                string Visual = Convert.ToString(CabeceraCotizacion["EstadoVisual"]);
                _Estado.Vista.VentasCboCotEstado.SelectedIndex = Visual == "BORRADOR" ? 0 : Visual == "EMITIDA" || Visual == "ACEPTADA" ? 1 : Visual == "VENCIDA" ? 2 : Visual == "CONVERTIDA" ? 3 : 4;
                _Estado.Lineas.Rows.Clear();
                foreach (DataRow Fila in ConjuntoDatos.Tables[1].Rows)
                    _Estado.Lineas.Rows.Add(Fila["idProducto"], Fila["descripcionItem"], Fila["tipoItem"], Fila["cantidad"], Fila["precioUnitario"], Fila["descuentoMonto"], Fila["porcentajeImpuesto"], Math.Round((decimal)Fila["cantidad"] * (decimal)Fila["precioUnitario"], 2, MidpointRounding.AwayFromZero), Fila["impuestoMonto"], Fila["totalLinea"]);
            }
            finally
            {
                _Estado.Cargando = false;
            }

            _Estado.EditandoCotizacion = false;
            _Estado.VentasMetRestablecerLinea();
            _Estado.VentasMetMostrarLineas();
            _Estado.VentasMetMostrarVigencia();
            if (VentasFuncEditable())
                _Estado.VentasMetPrecios();
        }

        internal void VentasMetBuscar()
        {
            DataTable Datos = _Estado.Controlador.VentasFuncListar(_Estado.Vista.VentasTxtCotNumero.Text.Trim());
            DataRow Fila = Datos.Rows.Count == 1 ? Datos.Rows[0] : ClsHerramientasVentas.VentasFuncSeleccionar(_Estado.Vista, "Consultar cotizaciones", Datos);
            if (Fila != null)
                VentasMetConsultar(Convert.ToString(Fila["Numero"]));
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
            if (MessageBox.Show(_Estado.Vista, "¿Desea anular la cotizacion " + _Estado.NumeroGuardado + "?", "Anular cotizacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            _Estado.Controlador.VentasMetAnular(_Estado.IdCotizacion, _Estado.VentasIdUsuario);
            VentasMetConsultar(_Estado.NumeroGuardado);
            ClsHerramientasVentas.VentasMetInformar(_Estado.Vista, "Cotizacion anulada.");
        }

        internal bool VentasFuncEditable()
        {
            return _Estado.EditandoCotizacion && _Estado.Controlador.VentasFuncPuedeEditar(_Estado.IdCotizacion, _Estado.EstadoGuardado, _Estado.Vista.VentasDtpCotVigencia.Value);
        }

        internal void VentasMetActualizarEdicion()
        {
            bool Editable = VentasFuncEditable();
            _Estado.Vista.VentasBtnCotModificar.Enabled = _Estado.IdCotizacion > 0 && _Estado.Controlador.VentasFuncPuedeEditar(_Estado.IdCotizacion, _Estado.EstadoGuardado, _Estado.Vista.VentasDtpCotVigencia.Value);
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
            string Numero = _Estado.Controlador.VentasFuncConvertir(_Estado.IdCotizacion, AFactura, ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboCotEstablecimiento), _Estado.VentasIdUsuario);
            VentasMetConsultar(_Estado.NumeroGuardado);
            ClsHerramientasVentas.VentasMetInformar(_Estado.Vista, AFactura ? "Factura borrador preparada con los precios originales: " + Numero + ". Su emision e inventarios corresponden a Facturas." : "Pedido creado con los precios originales: " + Numero);
        }
    }
}
