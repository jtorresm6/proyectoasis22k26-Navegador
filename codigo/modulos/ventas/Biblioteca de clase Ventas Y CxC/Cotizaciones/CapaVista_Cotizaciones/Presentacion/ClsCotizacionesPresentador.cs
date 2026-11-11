// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using CapaControlador_Cotizaciones;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_Cotizaciones
{
    internal sealed class ClsCotizacionesPresentador
    {
        internal ClsCotizacionesPresentador(FrmCotizacionVentas Vista)
        {
            this.Vista = Vista;
            RegistroVistaCotizaciones = new ClsRegistroVistaCotizaciones(this);
            new ClsEventosCotizaciones(this).VentasMetConectarEventos();
        }

        internal FrmCotizacionVentas Vista { get; set; }

        internal ClsCotizacionesControlador Controlador { get; set; } = new ClsCotizacionesControlador();

        internal DataTable Lineas { get; set; } = ClsCotizacionesPresentador.VentasFuncCrearDetalle();

        internal bool Cargando { get; set; } = true;

        internal int IdCotizacion { get; set; }

        internal int UsuarioInicial { get; set; }

        internal string EstadoGuardado { get; set; } = "";

        internal string NumeroGuardado { get; set; } = "";

        internal bool EditandoCotizacion { get; set; } = true;

        internal int IndiceLinea { get; set; } = -1;

        public int VentasIdUsuario
        {
            get
            {
                return UsuarioInicial > 0 ? UsuarioInicial : ClsHerramientasVentas.VentasFuncObtenerId(Vista.VentasCboUsuarioRegistro);
            }

            set
            {
                UsuarioInicial = value;
                Vista.VentasCboUsuarioRegistro.Enabled = value <= 0;
                if (Vista.VentasCboUsuarioRegistro.DataSource != null)
                    Vista.VentasCboUsuarioRegistro.SelectedValue = value;
            }
        }

        internal ClsRegistroVistaCotizaciones RegistroVistaCotizaciones { get; private set; }

        internal void VentasMetCargar()
        {
            this.Cargando = true;
            try
            {
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboUsuarioRegistro, this.Controlador.VentasFuncCatalogo("Usuarios"), this.UsuarioInicial);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotCliente, this.Controlador.VentasFuncCatalogo("Clientes"));
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotMoneda, this.Controlador.VentasFuncCatalogo("Monedas"));
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotVendedor, this.Controlador.VentasFuncCatalogo("Vendedores"));
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotEstablecimiento, this.Controlador.VentasFuncCatalogo("Establecimientos"));
            }
            finally
            {
                this.Cargando = false;
            }

            VentasMetNuevo();
        }

        internal void VentasMetCliente()
        {
            DataRow ClienteSeleccionado = ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotCliente);
            this.Vista.VentasTxtCotNit.Text = ClienteSeleccionado == null ? "" : Convert.ToString(ClienteSeleccionado["numeroIdentificacion"]);
            this.Vista.VentasTxtCotDireccion.Text = ClienteSeleccionado == null ? "" : Convert.ToString(ClienteSeleccionado["direccionCliente"]);
            this.Vista.VentasTxtCotContacto.Text = ClienteSeleccionado == null ? "" : Convert.ToString(ClienteSeleccionado["contactoCliente"]);
            if (ClienteSeleccionado != null)
                this.Vista.VentasCboCotMoneda.SelectedValue = Convert.ToInt32(ClienteSeleccionado["idMonedaCredito"]);
            VentasMetPrecios();
        }

        internal void VentasMetPrecios()
        {
            if (this.Cargando || !this.RegistroVistaCotizaciones.VentasFuncEditable())
                return;
            int Cliente = ClsHerramientasVentas.VentasFuncObtenerId(this.Vista.VentasCboCotCliente), Moneda = ClsHerramientasVentas.VentasFuncObtenerId(this.Vista.VentasCboCotMoneda);
            if (Cliente <= 0 || Moneda <= 0)
            {
                this.Vista.VentasTxtCotListaPrecios.Clear();
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotProducto, new DataTable());
                return;
            }

            DataTable Lista = this.Controlador.VentasFuncListaCliente(Cliente, Moneda, this.Vista.VentasDtpCotFecha.Value);
            this.Vista.VentasTxtCotListaPrecios.Text = Lista.Rows.Count == 1 ? Convert.ToString(Lista.Rows[0]["nombreListaPrecio"]) : "Sin lista vigente asignada para esta moneda";
            ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotProducto, this.Controlador.VentasFuncProductos(Cliente, Moneda, this.Vista.VentasDtpCotFecha.Value));
        }

        internal void VentasMetNuevo()
        {
            this.Cargando = true;
            try
            {
                this.EditandoCotizacion = true;
                VentasMetRestablecerLinea();
                this.IdCotizacion = 0;
                this.EstadoGuardado = "";
                this.NumeroGuardado = "";
                this.Lineas.Rows.Clear();
                this.Vista.VentasTxtCotNumero.Clear();
                this.Vista.VentasTxtCotNit.Clear();
                this.Vista.VentasTxtCotDireccion.Clear();
                this.Vista.VentasTxtCotContacto.Clear();
                this.Vista.VentasTxtCotListaPrecios.Clear();
                this.Vista.VentasTxtCotPrecio.Clear();
                this.Vista.VentasCboCotCliente.SelectedIndex = -1;
                this.Vista.VentasCboCotMoneda.SelectedIndex = -1;
                this.Vista.VentasCboCotVendedor.SelectedIndex = -1;
                this.Vista.VentasDtpCotFecha.Value = DateTime.Today;
                this.Vista.VentasDtpCotVigencia.Value = DateTime.Today.AddDays(30);
                this.Vista.VentasCboCotEstado.SelectedIndex = 0;
                this.Vista.VentasNudCotCantidad.Value = 1;
                this.Vista.VentasNudCotDescuento.Value = 0;
                this.Vista.VentasNudCotTipoCambio.Value = 1;
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotProducto, new DataTable());
            }
            finally
            {
                this.Cargando = false;
            }

            VentasMetMostrarLineas();
            VentasMetMostrarVigencia();
            this.Vista.VentasCboCotCliente.Focus();
            if (this.Vista.VentasCboCotCliente.Items.Count == 0)
                this.Vista.VentasLblCotVigencia.Text = "Configure clientes, vendedores, productos y la lista asignada al cliente para registrar cotizaciones.";
        }

        internal void VentasMetMostrarVigencia()
        {
            // Las reglas definitivas se validan en el Controlador al guardar o convertir.
            bool FechaValida = this.Vista.VentasDtpCotVigencia.Value.Date >= DateTime.Today && this.Vista.VentasDtpCotVigencia.Value.Date >= this.Vista.VentasDtpCotFecha.Value.Date;
            bool Vigente = FechaValida && this.Vista.VentasCboCotEstado.Text == "Vigente";
            this.Vista.VentasBtnCotPedido.Enabled = Vigente;
            this.Vista.VentasBtnCotFactura.Enabled = Vigente;
            this.Vista.VentasLblCotVigencia.Text = !FechaValida || this.Vista.VentasCboCotEstado.Text == "Vencida" ? "Cotización vencida o fechas no válidas: genere una nueva con los precios actuales." : "Una cotización vigente conserva sus precios. Si vence, se genera una nueva con los precios actuales.";
            this.RegistroVistaCotizaciones.VentasMetActualizarEdicion();
        }

        internal static DataTable VentasFuncCrearDetalle()
        {
            var TablaDetalle = new DataTable();
            TablaDetalle.Columns.Add("idProducto", typeof(int));
            TablaDetalle.Columns.Add("Descripcion", typeof(string));
            TablaDetalle.Columns.Add("Tipo", typeof(string));
            foreach (string NombreColumna in new[]
            {
                "Cantidad",
                "Precio",
                "Descuento",
                "Tasa",
                "Base",
                "Impuesto",
                "Total"
            })
                TablaDetalle.Columns.Add(NombreColumna, typeof(decimal));
            return TablaDetalle;
        }

        internal void VentasMetAgregar()
        {
            this.Controlador.VentasMetValidarCaptura(this.RegistroVistaCotizaciones.VentasFuncEditable(), "La cotizacion emitida conserva su detalle.");
            object[] Valores = this.Controlador.VentasFuncCalcularLinea(ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotProducto), this.Vista.VentasNudCotCantidad.Value, this.Vista.VentasNudCotDescuento.Value);
            if (this.IndiceLinea >= 0)
                this.Lineas.Rows[this.IndiceLinea].ItemArray = Valores;
            else
                this.Lineas.Rows.Add(Valores);
            VentasMetRestablecerLinea();
            VentasMetMostrarLineas();
            this.Vista.VentasNudCotDescuento.Value = 0;
        }

        internal void VentasMetMostrarLineas()
        {
            this.Vista.VentasDgvCotProductos.Rows.Clear();
            foreach (DataRow Fila in this.Lineas.Rows)
                this.Vista.VentasDgvCotProductos.Rows.Add(Fila["Descripcion"], Fila["Cantidad"], Fila["Precio"], Fila["Descuento"], Fila["Base"], Fila["Impuesto"], Fila["Total"]);
            string Moneda = ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotMoneda) == null ? "" : Convert.ToString(ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotMoneda)["codigoMoneda"]);
            this.Vista.VentasLblCotSubtotal.Text = ClsHerramientasVentas.VentasFuncDinero(this.Lineas.AsEnumerable().Sum(Registro => (decimal)Registro["Base"]), Moneda);
            this.Vista.VentasLblCotDescuento.Text = ClsHerramientasVentas.VentasFuncDinero(this.Lineas.AsEnumerable().Sum(Registro => (decimal)Registro["Descuento"]), Moneda);
            this.Vista.VentasLblCotImpuesto.Text = ClsHerramientasVentas.VentasFuncDinero(this.Lineas.AsEnumerable().Sum(Registro => (decimal)Registro["Impuesto"]), Moneda);
            this.Vista.VentasLblCotTotal.Text = ClsHerramientasVentas.VentasFuncDinero(this.Lineas.AsEnumerable().Sum(Registro => (decimal)Registro["Total"]), Moneda);
            this.RegistroVistaCotizaciones.VentasMetActualizarEdicion();
        }

        internal void VentasMetEditarLinea()
        {
            this.Controlador.VentasMetValidarEdicion(this.IdCotizacion, this.EstadoGuardado, this.Vista.VentasDtpCotVigencia.Value);
            if (!this.EditandoCotizacion)
                this.RegistroVistaCotizaciones.VentasMetModificar();
            if (this.Vista.VentasDgvCotProductos.CurrentRow == null)
                return;
            this.IndiceLinea = this.Vista.VentasDgvCotProductos.CurrentRow.Index;
            DataRow Fila = this.Lineas.Rows[this.IndiceLinea];
            this.Vista.VentasCboCotProducto.SelectedValue = Fila["idProducto"];
            this.Vista.VentasNudCotCantidad.Value = Convert.ToDecimal(Fila["Cantidad"]);
            this.Vista.VentasNudCotDescuento.Value = Convert.ToDecimal(Fila["Descuento"]);
            this.Vista.VentasBtnCotAgregar.BackgroundImage = Properties.Resources.BotonOriginalGuardarVentas;
            this.Vista.VentasTtpAcciones.SetToolTip(this.Vista.VentasBtnCotAgregar, "Guardar cambios del producto seleccionado");
        }

        internal void VentasMetRestablecerLinea()
        {
            this.IndiceLinea = -1;
            this.Vista.VentasBtnCotAgregar.BackgroundImage = Properties.Resources.BotonOriginalIngresarVentas;
            this.Vista.VentasTtpAcciones.SetToolTip(this.Vista.VentasBtnCotAgregar, "Agregar producto con precio de la lista del cliente");
        }
    }
}
