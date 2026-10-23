// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
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
        internal FrmCotizacionVentas Vista { get; set; }
        internal ClsCotizacionesControlador Controlador { get; set; } = new ClsCotizacionesControlador();
        internal DataTable Lineas { get; set; } = ClsCotizacionesPresentador.VentasMetCrearDetalle();
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

        internal ClsCotizacionesPresentador(FrmCotizacionVentas Vista)
        {
            this.Vista = Vista;
            RegistroVistaCotizaciones = new ClsRegistroVistaCotizaciones(this);
            VentasMetConectarEventos();
        }

        // EventosCotizaciones.
        internal void VentasMetConectarEventos()
        {
            this.Vista.Load += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetCargar);
            this.Vista.VentasBtnCotIngresar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetNuevo);
            this.Vista.VentasBtnCotModificar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.RegistroVistaCotizaciones.VentasMetModificar);
            this.Vista.VentasBtnCotAnular.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.RegistroVistaCotizaciones.VentasMetAnular);
            this.Vista.VentasBtnCotRefrescar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.RegistroVistaCotizaciones.VentasMetRefrescar);
            this.Vista.VentasDgvCotProductos.CellDoubleClick += (s, ArgumentosEvento) =>
            {
                if (ArgumentosEvento.RowIndex >= 0)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetEditarLinea);
            };
            this.Vista.VentasBtnCotAgregar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetAgregar);
            this.Vista.VentasBtnCotGuardar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.RegistroVistaCotizaciones.VentasMetGuardar);
            this.Vista.VentasBtnCotLimpiar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetNuevo);
            this.Vista.VentasBtnCotConsultar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.RegistroVistaCotizaciones.VentasMetBuscar);
            this.Vista.VentasTxtCotNumero.KeyDown += (s, ArgumentosEvento) =>
            {
                if (ArgumentosEvento.KeyCode == Keys.Enter)
                {
                    ArgumentosEvento.SuppressKeyPress = true;
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.RegistroVistaCotizaciones.VentasMetBuscar);
                }
            };
            this.Vista.VentasBtnCotPedido.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, () => this.RegistroVistaCotizaciones.VentasMetConvertir(false));
            this.Vista.VentasBtnCotFactura.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, () => this.RegistroVistaCotizaciones.VentasMetConvertir(true));
            this.Vista.VentasBtnCotEliminar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, () =>
            {
                if (this.Vista.VentasDgvCotProductos.CurrentRow != null && this.RegistroVistaCotizaciones.VentasMetEditable())
                {
                    this.Lineas.Rows.RemoveAt(this.Vista.VentasDgvCotProductos.CurrentRow.Index);
                    VentasMetRestablecerLinea();
                    VentasMetMostrarLineas();
                }
            });
            this.Vista.VentasBtnCotCerrar.Click += (s, ArgumentosEvento) => this.Vista.Close();
            this.Vista.VentasBtnCotImprimir.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, () =>
            {
                this.Controlador.VentasMetValidarCaptura(!(this.IdCotizacion <= 0), "Guarde la cotizacion antes de imprimir.");
                ClsHerramientasVentas.VentasMetImprimir(this.Vista, "Cotizacion " + this.NumeroGuardado, this.Vista.VentasCboCotCliente.Text + " / " + this.Vista.VentasCboCotMoneda.Text + " / " + this.Vista.VentasCboCotEstado.Text + " / Vigencia " + this.Vista.VentasDtpCotVigencia.Value.ToString("dd/MM/yyyy"), this.Lineas);
            });
            this.Vista.VentasCboCotCliente.SelectedIndexChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetCliente);
            };
            this.Vista.VentasCboCotMoneda.SelectedIndexChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetPrecios);
            };
            this.Vista.VentasDtpCotFecha.ValueChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetPrecios);
            };
            this.Vista.VentasCboCotProducto.SelectedIndexChanged += (s, ArgumentosEvento) =>
            {
                DataRow P = ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotProducto);
                this.Vista.VentasTxtCotPrecio.Text = P == null ? "" : Convert.ToDecimal(P["Precio"]).ToString("N4", ClsHerramientasVentas.Cultura);
            };
            this.Vista.VentasTxtCotNit.ReadOnly = true;
            this.Vista.VentasTxtCotContacto.ReadOnly = true;
            this.Vista.VentasTxtCotDireccion.ReadOnly = true;
            this.Vista.VentasNudCotCantidad.DecimalPlaces = 4;
            this.Vista.VentasNudCotCantidad.Minimum = 0.0001m;
            this.Vista.VentasNudCotCantidad.Value = 1;
            this.Vista.VentasTtpAcciones.SetToolTip(this.Vista.VentasNudCotDescuento, "Descuento como importe de la linea, en la moneda de la cotizacion.");
            this.Vista.VentasTtpAcciones.SetToolTip(this.Vista.VentasTxtCotNumero, "Numero o vacio para generar correlativo. Enter para consultar una cotizacion guardada.");
        }

        // CapturaCotizaciones.
        internal void VentasMetCargar()
        {
            this.Cargando = true;
            try
            {
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboUsuarioRegistro, this.Controlador.VentasMetCatalogo("Usuarios"), this.UsuarioInicial);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotCliente, this.Controlador.VentasMetCatalogo("Clientes"));
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotMoneda, this.Controlador.VentasMetCatalogo("Monedas"));
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotVendedor, this.Controlador.VentasMetCatalogo("Vendedores"));
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotEstablecimiento, this.Controlador.VentasMetCatalogo("Establecimientos"));
            }
            finally
            {
                this.Cargando = false;
            }

            VentasMetNuevo();
        }

        internal void VentasMetCliente()
        {
            DataRow C = ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotCliente);
            this.Vista.VentasTxtCotNit.Text = C == null ? "" : Convert.ToString(C["numeroIdentificacion"]);
            this.Vista.VentasTxtCotDireccion.Text = C == null ? "" : Convert.ToString(C["direccionCliente"]);
            this.Vista.VentasTxtCotContacto.Text = C == null ? "" : Convert.ToString(C["contactoCliente"]);
            if (C != null)
                this.Vista.VentasCboCotMoneda.SelectedValue = Convert.ToInt32(C["idMonedaCredito"]);
            VentasMetPrecios();
        }

        internal void VentasMetPrecios()
        {
            if (this.Cargando || !this.RegistroVistaCotizaciones.VentasMetEditable())
                return;
            int Cliente = ClsHerramientasVentas.VentasFuncObtenerId(this.Vista.VentasCboCotCliente), Moneda = ClsHerramientasVentas.VentasFuncObtenerId(this.Vista.VentasCboCotMoneda);
            if (Cliente <= 0 || Moneda <= 0)
            {
                this.Vista.VentasTxtCotListaPrecios.Clear();
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotProducto, new DataTable());
                return;
            }

            DataTable Lista = this.Controlador.VentasMetListaCliente(Cliente, Moneda, this.Vista.VentasDtpCotFecha.Value);
            this.Vista.VentasTxtCotListaPrecios.Text = Lista.Rows.Count == 1 ? Convert.ToString(Lista.Rows[0]["nombreListaPrecio"]) : "Sin lista vigente asignada para esta moneda";
            ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCotProducto, this.Controlador.VentasMetProductos(Cliente, Moneda, this.Vista.VentasDtpCotFecha.Value));
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

        // DetalleCotizaciones.
        internal static DataTable VentasMetCrearDetalle()
        {
            var T = new DataTable();
            T.Columns.Add("idProducto", typeof(int));
            T.Columns.Add("Descripcion", typeof(string));
            T.Columns.Add("Tipo", typeof(string));
            foreach (string C in new[]
            {
                "Cantidad",
                "Precio",
                "Descuento",
                "Tasa",
                "Base",
                "Impuesto",
                "Total"
            }

            )
                T.Columns.Add(C, typeof(decimal));
            return T;
        }

        internal void VentasMetAgregar()
        {
            this.Controlador.VentasMetValidarCaptura(!(!this.RegistroVistaCotizaciones.VentasMetEditable()), "La cotizacion emitida conserva su detalle.");
            object[] Valores = this.Controlador.VentasMetCalcularLinea(ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotProducto), this.Vista.VentasNudCotCantidad.Value, this.Vista.VentasNudCotDescuento.Value);
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
            foreach (DataRow F in this.Lineas.Rows)
                this.Vista.VentasDgvCotProductos.Rows.Add(F["Descripcion"], F["Cantidad"], F["Precio"], F["Descuento"], F["Base"], F["Impuesto"], F["Total"]);
            string Moneda = ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotMoneda) == null ? "" : Convert.ToString(ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCotMoneda)["codigoMoneda"]);
            this.Vista.VentasLblCotSubtotal.Text = ClsHerramientasVentas.VentasFuncDinero(this.Lineas.AsEnumerable().Sum(F => (decimal)F["Base"]), Moneda);
            this.Vista.VentasLblCotDescuento.Text = ClsHerramientasVentas.VentasFuncDinero(this.Lineas.AsEnumerable().Sum(F => (decimal)F["Descuento"]), Moneda);
            this.Vista.VentasLblCotImpuesto.Text = ClsHerramientasVentas.VentasFuncDinero(this.Lineas.AsEnumerable().Sum(F => (decimal)F["Impuesto"]), Moneda);
            this.Vista.VentasLblCotTotal.Text = ClsHerramientasVentas.VentasFuncDinero(this.Lineas.AsEnumerable().Sum(F => (decimal)F["Total"]), Moneda);
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
            DataRow F = this.Lineas.Rows[this.IndiceLinea];
            this.Vista.VentasCboCotProducto.SelectedValue = F["idProducto"];
            this.Vista.VentasNudCotCantidad.Value = Convert.ToDecimal(F["Cantidad"]);
            this.Vista.VentasNudCotDescuento.Value = Convert.ToDecimal(F["Descuento"]);
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
