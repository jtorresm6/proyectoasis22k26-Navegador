/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Fecha de actualización: 17/10/2026
 * Propósito: Operar CxC, registrar documentos y recibos y preparar depósitos con persistencia transaccional.
 */
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_MovimientosClientes;
using VentasPresentacion;

namespace CapaVista_MovimientosClientes
{
    public partial class FrmMovimientosClientesVentas : Form
    {
        public FrmMovimientosClientesVentas()
        {
            InitializeComponent();
            CapturaMovimientos = new ClsCapturaMovimientos(this);
            ConsultasMovimientos = new ClsConsultasMovimientos(this);
            VentasMetConectarEventos();
        }

        internal ClsConsultasMovimientos ConsultasMovimientos { get; }



        internal ClsMovimientosControlador Controlador { get; set; } = new ClsMovimientosControlador();

        internal Button VentasBtnSeleccionarFactura { get; set; } = new Button();

        internal bool Cargando { get; set; } = true;

        internal bool Inicializado { get; set; }

        internal bool[] Guardados { get; set; } = new bool[5];

        internal DataSet EstadoCuenta { get; set; }

        internal int UsuarioInicial { get; set; }

        internal DataRow DocumentoActual { get; set; }

        internal DataRow ReciboActual { get; set; }

        internal bool EditandoDocumento { get; set; }

        internal bool EditandoRecibo { get; set; }

        public int VentasIdUsuario
        {
            get
            {
                return UsuarioInicial > 0 ? UsuarioInicial : ClsHerramientasVentas.VentasFuncObtenerId(this.VentasCboUsuarioRegistro);
            }

            set
            {
                UsuarioInicial = value;
                this.VentasCboUsuarioRegistro.Enabled = value <= 0;
                if (this.VentasCboUsuarioRegistro.DataSource != null)
                    this.VentasCboUsuarioRegistro.SelectedValue = value;
            }
        }

        internal int Cliente
        {
            get
            {
                return ClsHerramientasVentas.VentasFuncObtenerId(this.VentasCboCliente);
            }
        }

        internal int Moneda
        {
            get
            {
                return ClsHerramientasVentas.VentasFuncObtenerId(this.VentasCboMoneda);
            }
        }

        internal string CodigoMoneda
        {
            get
            {
                var Fila = ClsHerramientasVentas.VentasFuncObtenerFila(this.VentasCboMoneda);
                return Fila == null ? "" : Convert.ToString(Fila["codigoMoneda"]);
            }
        }

        internal ClsCapturaMovimientos CapturaMovimientos { get; private set; }

        public void VentasMetConfigurarCatalogosBancos(DataTable Empresas, DataTable Bancos, DataTable Cuentas, DataTable Conceptos, string Modo)
        {
            ConsultasMovimientos.VentasMetConfigurarCatalogosBancos(Empresas, Bancos, Cuentas, Conceptos, Modo);
        }

        internal void VentasMetTipoDocumentoCambio()
        {
            CapturaMovimientos.VentasMetTipoDocumentoCambio();
        }

        internal void VentasMetCargar()
        {
            this.Cargando = true;
            try
            {
                ClsHerramientasVentas.VentasMetCargarOpciones(this.VentasCboUsuarioRegistro, this.Controlador.VentasFuncCatalogo("Usuarios"), this.UsuarioInicial);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.VentasCboCliente, this.Controlador.VentasFuncCatalogo("Clientes"));
                DataTable Monedas = this.Controlador.VentasFuncCatalogo("Monedas");
                ClsHerramientasVentas.VentasMetCargarOpciones(this.VentasCboMoneda, Monedas);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.VentasCboMonedaDeposito, Monedas.Copy());
                ClsHerramientasVentas.VentasMetCargarOpciones(this.VentasCboEstablecimientoDocumento, this.Controlador.VentasFuncCatalogo("Establecimientos"));
                this.VentasCboTipoConsulta.SelectedIndex = 0;
                this.VentasCboEstadoConsulta.SelectedIndex = 0;
                this.Inicializado = true;
                this.VentasLblEtapaBancos.Text = this.VentasCboCliente.Items.Count == 0 ? "Registre clientes, monedas y emisor en los catalogos de Ventas para comenzar." : "Seleccione cliente y moneda. Los recibos se generan al guardar Recepcion de pagos.";
            }
            finally
            {
                this.Cargando = false;
                CapturaMovimientos.VentasMetRefrescarBotones();
            }

            if (this.VentasTbcMovimientos.SelectedTab == this.VentasTbpConsultaRecibos)
                ConsultasMovimientos.VentasMetConsultarRecibos();
        }

        internal void VentasMetClienteCambio()
        {
            this.DocumentoActual = null;
            this.ReciboActual = null;
            this.EditandoDocumento = this.EditandoRecibo = false;
            this.Guardados[1] = this.Guardados[2] = false;
            this.VentasTxtNumeroDocumento.Clear();
            this.VentasTxtRecibo.Clear();
            this.VentasTxtDescripcionDocumento.Clear();
            this.VentasTxtObservacionesPago.Clear();
            CapturaMovimientos.VentasMetRefrescarBotones();
            DataRow ClienteSeleccionado = ClsHerramientasVentas.VentasFuncObtenerFila(this.VentasCboCliente);
            this.VentasTxtCondicion.Text = ClienteSeleccionado == null ? "" : ClienteSeleccionado["diasCredito"] + " dias; limite " + Convert.ToDecimal(ClienteSeleccionado["limiteCredito"]).ToString("N2", ClsHerramientasVentas.Cultura);
            if (ClienteSeleccionado != null)
                this.VentasCboMoneda.SelectedValue = Convert.ToInt32(ClienteSeleccionado["idMonedaCredito"]);
            this.VentasTxtFacturaReferencia.Clear();
            CapturaMovimientos.VentasMetEstadoNuevoDocumento();
            CapturaMovimientos.VentasMetVencimientoCargo();
            VentasMetLimpiarResultados();
            if (this.Cliente > 0 && this.Moneda > 0)
                VentasMetConsultarActual();
        }

        internal void VentasMetLimpiarResultados()
        {
            this.EstadoCuenta = null;
            this.VentasDgvMovimientos.Rows.Clear();
            this.VentasDgvDocumentos.Rows.Clear();
            this.VentasLblValorInicial.Text = this.VentasLblValorCargos.Text = this.VentasLblValorAbonos.Text = this.VentasLblValorFinal.Text = ClsHerramientasVentas.VentasFuncDinero(0, this.CodigoMoneda);
        }

        internal void VentasMetConsultarActual()
        {
            if (this.Cargando)
                return;
            if (this.VentasTbcMovimientos.SelectedTab == this.VentasTbpConsultaRecibos)
            {
                ConsultasMovimientos.VentasMetConsultarRecibos();
                return;
            }

            if (this.VentasTbcMovimientos.SelectedTab == this.VentasTbpDepositos)
            {
                if (!string.IsNullOrWhiteSpace(this.VentasTxtCodigoDeposito.Text))
                    ConsultasMovimientos.VentasMetConsultarDeposito();
                else
                    ConsultasMovimientos.VentasMetDisponibles();
                return;
            }

            if (this.Cliente <= 0 || this.Moneda <= 0)
            {
                VentasMetLimpiarResultados();
                return;
            }

            if (this.VentasTbcMovimientos.SelectedTab == this.VentasTbpCuenta)
            {
                this.EstadoCuenta = this.Controlador.VentasFuncEstadoCuenta(this.Cliente, this.Moneda, this.VentasDtpDesde.Value, this.VentasDtpHasta.Value);
                ConsultasMovimientos.VentasMetMostrarEstado();
            }

            if (this.VentasTbcMovimientos.SelectedTab == this.VentasTbpRecibos && this.ReciboActual == null)
                CapturaMovimientos.VentasMetPendientes();
        }

        internal void VentasMetCambiarPestana()
        {
            VentasMetActualizarContexto();
            if (!this.Cargando && this.Inicializado)
                ClsHerramientasVentas.VentasMetEjecutar(this, VentasMetConsultarActual);
        }

        internal void VentasMetActualizarContexto()
        {
            // Un depósito agrupa recibos de distintos clientes; no hereda el filtro individual.
            this.VentasPnlCliente.Enabled = this.VentasTbcMovimientos.SelectedIndex < 3;
            CapturaMovimientos.VentasMetRefrescarBotones();
        }

        public void VentasMetMostrarRecepcionPagos()
        {
            this.Text = "7005 – RecepciónDePagos";
            this.VentasLblCodigo.Text = "7005";
            this.VentasTbcMovimientos.SelectedTab = this.VentasTbpRecibos;
            VentasMetActualizarContexto();
        }

        public void VentasMetMostrarConsultaRecibos(string Recibo, string Deposito)
        {
            this.VentasTxtConsultaRecibo.Text = Recibo ?? "";
            this.VentasTxtConsultaDeposito.Text = Deposito ?? "";
            this.VentasTbcMovimientos.SelectedTab = this.VentasTbpConsultaRecibos;
            VentasMetActualizarContexto();
        }

        internal decimal VentasFuncSumar(DataGridView Grid, int Columna)
        {
            return Grid.Rows.Cast<DataGridViewRow>().Where(FilaTabla => FilaTabla.Cells[0].Value is bool && (bool)FilaTabla.Cells[0].Value).Sum(FilaTabla => this.Controlador.VentasFuncImporteCelda(FilaTabla.Cells[Columna].Value));
        }

        internal DataTable VentasFuncSeleccionados(DataGridView Grid, string Id, int Importe)
        {
            this.Controlador.VentasMetValidarCaptura(Grid.EndEdit(), "Corrija la celda en edicion.");
            var Tabla = new DataTable();
            Tabla.Columns.Add(Id, typeof(int));
            Tabla.Columns.Add("Importe", typeof(decimal));
            foreach (DataGridViewRow FilaTabla in Grid.Rows)
                if (FilaTabla.Cells[0].Value is bool && (bool)FilaTabla.Cells[0].Value && FilaTabla.Tag is DataRow)
                    Tabla.Rows.Add(((DataRow)FilaTabla.Tag)[Id], this.Controlador.VentasFuncImporteCelda(FilaTabla.Cells[Importe].Value));
            return Tabla;
        }

        private void VentasMetTbcMovimientosSelectedIndexChanged(object Origen, EventArgs Evento)
        {
            if (CapturaMovimientos != null)
                VentasMetCambiarPestana();
        }

        private void VentasMetCboTipoDocumentoSelectedIndexChanged(object Origen, EventArgs Evento)
        {
            if (CapturaMovimientos != null)
                VentasMetTipoDocumentoCambio();
        }

        internal void VentasMetConectarEventos()
        {
            VentasMetCrearSelectorFactura();
            this.VentasDtpFechaDocumento.ValueChanged += (Origen, Evento) =>
            {
                if (!this.Cargando && this.DocumentoActual == null && this.VentasCboTipoDocumento.SelectedIndex == 0)
                    this.CapturaMovimientos.VentasMetVencimientoCargo();
            };
            foreach (var Grid in new[]
            {
                this.VentasDgvMovimientos,
                this.VentasDgvDocumentos,
                this.VentasDgvRecibosDeposito,
                this.VentasDgvConsultaRecibos,
                this.VentasDgvConsultaAplicaciones
            })
                ClsHerramientasVentas.VentasMetFormatearFechas(Grid);
            this.Load += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(this, this.VentasMetCargar);
            this.VentasBtnConsultar.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(this, () =>
            {
                if (!this.Inicializado)
                    this.VentasMetCargar();
                else
                    this.ConsultasMovimientos.VentasMetConsultarCaptura();
            });
            VentasMetConectarBoton(this.VentasBtnModificar, this.CapturaMovimientos.VentasMetModificar);
            VentasMetConectarBoton(this.VentasBtnEliminar, this.CapturaMovimientos.VentasMetAnular);
            this.VentasBtnRefrescar.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(this, () =>
            {
                if (this.VentasTbcMovimientos.SelectedIndex == 1 && this.DocumentoActual != null)
                    this.ConsultasMovimientos.VentasMetCargarDocumento(Convert.ToInt32(this.DocumentoActual["Id"]), Convert.ToString(this.DocumentoActual["Clase"]));
                else if (this.VentasTbcMovimientos.SelectedIndex == 2 && this.ReciboActual != null)
                    this.ConsultasMovimientos.VentasMetCargarRecibo(Convert.ToInt32(this.ReciboActual["idPago"]));
                else
                    this.VentasMetConsultarActual();
            });
            this.VentasDgvMovimientos.CellDoubleClick += (Origen, Evento) =>
            {
                if (Evento.RowIndex >= 0)
                    ClsHerramientasVentas.VentasMetEjecutar(this, this.ConsultasMovimientos.VentasMetAbrirSeleccionActual);
            };
            this.VentasDgvConsultaRecibos.CellDoubleClick += (Origen, Evento) =>
            {
                if (Evento.RowIndex >= 0)
                    ClsHerramientasVentas.VentasMetEjecutar(this, this.ConsultasMovimientos.VentasMetAbrirSeleccionActual);
            };
            foreach (var Combo in new[]
            {
                this.VentasCboEmpresaDeposito,
                this.VentasCboBancoDestino,
                this.VentasCboCuentaDestino,
                this.VentasCboConceptoBancario
            })
                Combo.DropDownStyle = ComboBoxStyle.DropDown;
            VentasMetConectarBoton(this.VentasBtnGuardar, this.CapturaMovimientos.VentasMetGuardar);
            VentasMetConectarBoton(this.VentasBtnIngresar, this.CapturaMovimientos.VentasMetNuevo);
            VentasMetConectarBoton(this.VentasBtnCancelar, this.CapturaMovimientos.VentasMetNuevo);
            this.VentasBtnCerrar.Click += (Origen, Evento) => this.Close();
            VentasMetConectarBoton(this.VentasBtnImprimir, this.ConsultasMovimientos.VentasMetImprimir);
            VentasMetConectarLista(this.VentasCboCliente, this.VentasMetClienteCambio);
            VentasMetConectarLista(this.VentasCboMoneda, this.VentasMetConsultarActual);
            VentasMetConectarLista(this.VentasCboMonedaDeposito, this.ConsultasMovimientos.VentasMetDisponibles);
            this.VentasCboTipoConsulta.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!this.Cargando && this.EstadoCuenta != null)
                    this.ConsultasMovimientos.VentasMetMostrarEstado();
            };
            this.VentasCboEstadoConsulta.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!this.Cargando && this.EstadoCuenta != null)
                    this.ConsultasMovimientos.VentasMetMostrarEstado();
            };
            this.VentasNudMontoPago.ValueChanged += (Origen, Evento) => this.CapturaMovimientos.VentasMetTotalesPago();
            foreach (var Grid in new[]
            {
                this.VentasDgvDocumentos,
                this.VentasDgvRecibosDeposito
            })
            {
                Grid.CurrentCellDirtyStateChanged += (Origen, Evento) =>
                {
                    if (Grid.IsCurrentCellDirty && Grid.CurrentCell is DataGridViewCheckBoxCell)
                        Grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                };
                Grid.DataError += (Origen, Evento) =>
                {
                    Evento.ThrowException = false;
                    ClsHerramientasVentas.VentasMetInformar(this, "Ingrese un importe numerico valido.");
                };
            }

            this.VentasDgvDocumentos.CellValueChanged += this.CapturaMovimientos.VentasMetAplicacionCambio;
            this.VentasDgvRecibosDeposito.CellValueChanged += this.ConsultasMovimientos.VentasMetDepositoCambio;
            this.VentasDgvConsultaRecibos.SelectionChanged += (Origen, Evento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this, this.ConsultasMovimientos.VentasMetAplicacionesConsulta);
            };
            VentasMetConfigurarCampos();
        }

        private void VentasMetCrearSelectorFactura()
        {
            int AnchoSelector = Math.Max(120, TextRenderer.MeasureText("Elegir factura", this.VentasTxtFacturaReferencia.Font).Width + 24);
            this.VentasTxtFacturaReferencia.Width -= AnchoSelector + 8;
            this.VentasBtnSeleccionarFactura.Name = "VentasBtnSeleccionarFactura";
            this.VentasBtnSeleccionarFactura.Text = "Elegir factura";
            this.VentasBtnSeleccionarFactura.SetBounds(this.VentasTxtFacturaReferencia.Right + 8, this.VentasTxtFacturaReferencia.Top, AnchoSelector, this.VentasTxtFacturaReferencia.Height);
            this.VentasBtnSeleccionarFactura.Font = this.VentasTxtFacturaReferencia.Font;
            this.VentasBtnSeleccionarFactura.Anchor = this.VentasTxtFacturaReferencia.Anchor;
            this.VentasBtnSeleccionarFactura.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnSeleccionarFactura.ForeColor = System.Drawing.Color.White;
            this.VentasBtnSeleccionarFactura.FlatStyle = FlatStyle.Flat;
            this.VentasTxtFacturaReferencia.Parent.Controls.Add(this.VentasBtnSeleccionarFactura);
            VentasMetConectarBoton(this.VentasBtnSeleccionarFactura, this.CapturaMovimientos.VentasMetSeleccionarFactura);
        }

        private void VentasMetConfigurarCampos()
        {
            this.CapturaMovimientos.VentasMetEstadoNuevoDocumento();
            this.VentasCboEstadoDeposito.Items.Clear();
            this.VentasCboEstadoDeposito.Items.Add("Preparacion local");
            this.VentasCboEstadoDeposito.SelectedIndex = 0;
            this.VentasCboEstadoDeposito.Enabled = false;
            this.VentasTxtEstadoEnvio.Text = "Pendiente de enlace con Bancos";
            this.VentasTxtModoActualizacion.Text = "Lo define Bancos; sin afectar saldos bancarios";
            this.VentasDtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            this.VentasDtpHasta.Value = DateTime.Today;
            this.VentasNudTipoCambioPago.Value = 1;
            this.VentasTtpAcciones.SetToolTip(this.VentasTxtNumeroDocumento, "Numero interno. Si se deja vacio, se genera un correlativo. No emite DTE.");
            this.VentasTtpAcciones.SetToolTip(this.VentasTxtRecibo, "Dejar vacio para generar el numero del recibo al guardar.");
            this.VentasLblDocumentoRecibo.Text = "Para registrar dinero recibido y reducir la deuda use Recepcion de pagos; al guardar se genera el recibo.";
            this.VentasTtpAcciones.SetToolTip(this.VentasTxtFacturaReferencia, "Seleccione una factura del cliente y moneda actuales con Elegir factura.");
            this.VentasCboTipoDocumento.SelectedIndex = 0;
            this.CapturaMovimientos.VentasMetTipoDocumentoCambio();
        }

        private void VentasMetConectarBoton(Button Boton, Action Accion)
        {
            Boton.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(this, Accion);
        }

        private void VentasMetConectarLista(ComboBox Lista, Action Accion)
        {
            Lista.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this, Accion);
            };
        }
    }
}
