// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using CapaControlador_MovimientosClientes;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_MovimientosClientes
{
    internal sealed class ClsMovimientosPresentador
    {
        internal FrmMovimientosClientesVentas Vista { get; set; }
        internal ClsMovimientosControlador Controlador { get; set; } = new ClsMovimientosControlador();
        internal Button SeleccionarFactura { get; set; } = new Button();
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

        internal int Cliente
        {
            get
            {
                return ClsHerramientasVentas.VentasFuncObtenerId(Vista.VentasCboCliente);
            }
        }

        internal int Moneda
        {
            get
            {
                return ClsHerramientasVentas.VentasFuncObtenerId(Vista.VentasCboMoneda);
            }
        }

        internal string CodigoMoneda
        {
            get
            {
                var F = ClsHerramientasVentas.VentasFuncObtenerFila(Vista.VentasCboMoneda);
                return F == null ? "" : Convert.ToString(F["codigoMoneda"]);
            }
        }

        internal ClsCapturaMovimientos CapturaMovimientos { get; private set; }

        internal ClsMovimientosPresentador(FrmMovimientosClientesVentas Vista)
        {
            this.Vista = Vista;
            CapturaMovimientos = new ClsCapturaMovimientos(this);
            VentasMetConectarEventos();
        }

        public void VentasMetConfigurarCatalogosBancos(DataTable Empresas, DataTable Bancos, DataTable Cuentas, DataTable Conceptos, string Modo)
        {
            this.CapturaMovimientos.VentasMetConfigurarCatalogosBancos(Empresas, Bancos, Cuentas, Conceptos, Modo);
        }

        internal void VentasMetTipoDocumentoCambio()
        {
            this.CapturaMovimientos.VentasMetTipoDocumentoCambio();
        }

        // EventosMovimientos.
        internal void VentasMetConectarEventos()
        {
            int AnchoSelector = Math.Max(120, TextRenderer.MeasureText("Elegir factura", this.Vista.VentasTxtFacturaReferencia.Font).Width + 24);
            this.Vista.VentasTxtFacturaReferencia.Width -= AnchoSelector + 8;
            this.SeleccionarFactura.Name = "VentasBtnSeleccionarFactura";
            this.SeleccionarFactura.Text = "Elegir factura";
            this.SeleccionarFactura.SetBounds(this.Vista.VentasTxtFacturaReferencia.Right + 8, this.Vista.VentasTxtFacturaReferencia.Top, AnchoSelector, this.Vista.VentasTxtFacturaReferencia.Height);
            this.SeleccionarFactura.Font = this.Vista.VentasTxtFacturaReferencia.Font;
            this.SeleccionarFactura.Anchor = this.Vista.VentasTxtFacturaReferencia.Anchor;
            this.SeleccionarFactura.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.SeleccionarFactura.ForeColor = System.Drawing.Color.White;
            this.SeleccionarFactura.FlatStyle = FlatStyle.Flat;
            this.Vista.VentasTxtFacturaReferencia.Parent.Controls.Add(this.SeleccionarFactura);
            this.SeleccionarFactura.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.CapturaMovimientos.VentasMetSeleccionarFactura);
            this.Vista.VentasDtpFechaDocumento.ValueChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando && this.DocumentoActual == null && this.Vista.VentasCboTipoDocumento.SelectedIndex == 0)
                    this.CapturaMovimientos.VentasMetVencimientoCargo();
            };
            foreach (var Grid in new[]
            {
                this.Vista.VentasDgvMovimientos,
                this.Vista.VentasDgvDocumentos,
                this.Vista.VentasDgvConsultaRecibos,
                this.Vista.VentasDgvConsultaAplicaciones
            }

            )
                ClsHerramientasVentas.VentasMetFormatearFechas(Grid);
            this.Vista.Load += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetCargar);
            this.Vista.VentasBtnConsultar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, () =>
            {
                if (!this.Inicializado)
                    VentasMetCargar();
                else
                    VentasMetConsultarCaptura();
            });
            this.Vista.VentasBtnModificar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.CapturaMovimientos.VentasMetModificar);
            this.Vista.VentasBtnEliminar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.CapturaMovimientos.VentasMetAnular);
            this.Vista.VentasBtnRefrescar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, () =>
            {
                if (this.Vista.VentasTbcMovimientos.SelectedIndex == 1 && this.DocumentoActual != null)
                    this.CapturaMovimientos.VentasMetCargarDocumento(Convert.ToInt32(this.DocumentoActual["Id"]), Convert.ToString(this.DocumentoActual["Clase"]));
                else if (this.Vista.VentasTbcMovimientos.SelectedIndex == 2 && this.ReciboActual != null)
                    this.CapturaMovimientos.VentasMetCargarRecibo(Convert.ToInt32(this.ReciboActual["idPago"]));
                else
                    VentasMetConsultarActual();
            });
            this.Vista.VentasDgvMovimientos.CellDoubleClick += (s, ArgumentosEvento) =>
            {
                if (ArgumentosEvento.RowIndex >= 0)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetAbrirSeleccionActual);
            };
            this.Vista.VentasDgvConsultaRecibos.CellDoubleClick += (s, ArgumentosEvento) =>
            {
                if (ArgumentosEvento.RowIndex >= 0)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetAbrirSeleccionActual);
            };
            foreach (var Combo in new[]
            {
                this.Vista.VentasCboEmpresaDeposito,
                this.Vista.VentasCboBancoDestino,
                this.Vista.VentasCboCuentaDestino,
                this.Vista.VentasCboConceptoBancario
            }

            )
                Combo.DropDownStyle = ComboBoxStyle.DropDown;
            this.Vista.VentasBtnGuardar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.CapturaMovimientos.VentasMetGuardar);
            this.Vista.VentasBtnIngresar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.CapturaMovimientos.VentasMetNuevo);
            this.Vista.VentasBtnCancelar.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.CapturaMovimientos.VentasMetNuevo);
            this.Vista.VentasBtnCerrar.Click += (s, ArgumentosEvento) => this.Vista.Close();
            this.Vista.VentasBtnImprimir.Click += (s, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetImprimir);
            this.Vista.VentasCboCliente.SelectedIndexChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetClienteCambio);
            };
            this.Vista.VentasCboMoneda.SelectedIndexChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetConsultarActual);
            };
            this.Vista.VentasCboMonedaDeposito.SelectedIndexChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.CapturaMovimientos.VentasMetDisponibles);
            };
            this.Vista.VentasCboTipoConsulta.SelectedIndexChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando && this.EstadoCuenta != null)
                    VentasMetMostrarEstado();
            };
            this.Vista.VentasCboEstadoConsulta.SelectedIndexChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando && this.EstadoCuenta != null)
                    VentasMetMostrarEstado();
            };
            this.Vista.VentasNudMontoPago.ValueChanged += (s, ArgumentosEvento) => this.CapturaMovimientos.VentasMetTotalesPago();
            foreach (var Grid in new[]
            {
                this.Vista.VentasDgvDocumentos,
                this.Vista.VentasDgvRecibosDeposito
            }

            )
            {
                Grid.CurrentCellDirtyStateChanged += (s, ArgumentosEvento) =>
                {
                    if (Grid.IsCurrentCellDirty && Grid.CurrentCell is DataGridViewCheckBoxCell)
                        Grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                };
                Grid.DataError += (s, ArgumentosEvento) =>
                {
                    ArgumentosEvento.ThrowException = false;
                    Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarAdvertencia(this.Vista, "Ingrese un importe numerico valido.", "Validación");
                };
            }

            this.Vista.VentasDgvDocumentos.CellValueChanged += this.CapturaMovimientos.VentasMetAplicacionCambio;
            this.Vista.VentasDgvRecibosDeposito.CellValueChanged += this.CapturaMovimientos.VentasMetDepositoCambio;
            this.Vista.VentasDgvConsultaRecibos.SelectionChanged += (s, ArgumentosEvento) =>
            {
                if (!this.Cargando)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.CapturaMovimientos.VentasMetAplicacionesConsulta);
            };
            this.CapturaMovimientos.VentasMetEstadoNuevoDocumento();
            this.Vista.VentasCboEstadoDeposito.Items.Clear();
            this.Vista.VentasCboEstadoDeposito.Items.Add("Preparacion local");
            this.Vista.VentasCboEstadoDeposito.SelectedIndex = 0;
            this.Vista.VentasCboEstadoDeposito.Enabled = false;
            this.Vista.VentasTxtEstadoEnvio.Text = "Pendiente de enlace con Bancos";
            this.Vista.VentasTxtModoActualizacion.Text = "Lo define Bancos; sin afectar saldos bancarios";
            this.Vista.VentasDtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            this.Vista.VentasDtpHasta.Value = DateTime.Today;
            this.Vista.VentasNudTipoCambioPago.Value = 1;
            this.Vista.VentasTtpAcciones.SetToolTip(this.Vista.VentasTxtNumeroDocumento, "Numero interno. Si se deja vacio, se genera un correlativo. No emite DTE.");
            this.Vista.VentasTtpAcciones.SetToolTip(this.Vista.VentasTxtRecibo, "Dejar vacio para generar el numero del recibo al guardar.");
            this.Vista.VentasLblDocumentoRecibo.Text = "Para registrar dinero recibido y reducir la deuda use Recepcion de pagos; al guardar se genera el recibo.";
            this.Vista.VentasTtpAcciones.SetToolTip(this.Vista.VentasTxtFacturaReferencia, "Seleccione una factura del cliente y moneda actuales con Elegir factura.");
            this.Vista.VentasCboTipoDocumento.SelectedIndex = 0;
            this.CapturaMovimientos.VentasMetTipoDocumentoCambio();
        }

        // CatalogosMovimientos.
        internal void VentasMetCargar()
        {
            this.Cargando = true;
            try
            {
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboUsuarioRegistro, this.Controlador.VentasMetCatalogo("Usuarios"), this.UsuarioInicial);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboCliente, this.Controlador.VentasMetCatalogo("Clientes"));
                DataTable Monedas = this.Controlador.VentasMetCatalogo("Monedas");
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboMoneda, Monedas);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboMonedaDeposito, Monedas.Copy());
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboEstablecimientoDocumento, this.Controlador.VentasMetCatalogo("Establecimientos"));
                this.Vista.VentasCboTipoConsulta.SelectedIndex = 0;
                this.Vista.VentasCboEstadoConsulta.SelectedIndex = 0;
                this.Inicializado = true;
                this.Vista.VentasLblEtapaBancos.Text = this.Vista.VentasCboCliente.Items.Count == 0 ? "Registre clientes, monedas y emisor en los catalogos de Ventas para comenzar." : "Seleccione cliente y moneda. Los recibos se generan al guardar Recepcion de pagos.";
            }
            finally
            {
                this.Cargando = false;
                this.CapturaMovimientos.VentasMetRefrescarBotones();
            }

            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpConsultaRecibos)
                this.CapturaMovimientos.VentasMetConsultarRecibos();
        }

        internal void VentasMetClienteCambio()
        {
            this.DocumentoActual = null;
            this.ReciboActual = null;
            this.EditandoDocumento = this.EditandoRecibo = false;
            this.Guardados[1] = this.Guardados[2] = false;
            this.Vista.VentasTxtNumeroDocumento.Clear();
            this.Vista.VentasTxtRecibo.Clear();
            this.Vista.VentasTxtDescripcionDocumento.Clear();
            this.Vista.VentasTxtObservacionesPago.Clear();
            this.CapturaMovimientos.VentasMetRefrescarBotones();
            DataRow C = ClsHerramientasVentas.VentasFuncObtenerFila(this.Vista.VentasCboCliente);
            this.Vista.VentasTxtCondicion.Text = C == null ? "" : C["diasCredito"] + " dias; limite " + Convert.ToDecimal(C["limiteCredito"]).ToString("N2", ClsHerramientasVentas.Cultura);
            if (C != null)
                this.Vista.VentasCboMoneda.SelectedValue = Convert.ToInt32(C["idMonedaCredito"]);
            this.Vista.VentasTxtFacturaReferencia.Clear();
            this.CapturaMovimientos.VentasMetEstadoNuevoDocumento();
            this.CapturaMovimientos.VentasMetVencimientoCargo();
            VentasMetLimpiarResultados();
            if (this.Cliente > 0 && this.Moneda > 0)
                VentasMetConsultarActual();
        }

        internal void VentasMetLimpiarResultados()
        {
            this.EstadoCuenta = null;
            this.Vista.VentasDgvMovimientos.Rows.Clear();
            this.Vista.VentasDgvDocumentos.Rows.Clear();
            this.Vista.VentasLblValorInicial.Text = this.Vista.VentasLblValorCargos.Text = this.Vista.VentasLblValorAbonos.Text = this.Vista.VentasLblValorFinal.Text = ClsHerramientasVentas.VentasFuncDinero(0, this.CodigoMoneda);
        }

        internal void VentasMetConsultarActual()
        {
            if (this.Cargando)
                return;
            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpConsultaRecibos)
            {
                this.CapturaMovimientos.VentasMetConsultarRecibos();
                return;
            }

            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpDepositos)
            {
                if (!string.IsNullOrWhiteSpace(this.Vista.VentasTxtCodigoDeposito.Text))
                    this.CapturaMovimientos.VentasMetConsultarDeposito();
                else
                    this.CapturaMovimientos.VentasMetDisponibles();
                return;
            }

            if (this.Cliente <= 0 || this.Moneda <= 0)
            {
                VentasMetLimpiarResultados();
                return;
            }

            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpCuenta)
            {
                this.EstadoCuenta = this.Controlador.VentasMetEstadoCuenta(this.Cliente, this.Moneda, this.Vista.VentasDtpDesde.Value, this.Vista.VentasDtpHasta.Value);
                VentasMetMostrarEstado();
            }

            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpRecibos && this.ReciboActual == null)
                this.CapturaMovimientos.VentasMetPendientes();
        }

        internal void VentasMetCambiarPestana()
        {
            VentasMetActualizarContexto();
            if (!this.Cargando && this.Inicializado)
                ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetConsultarActual);
        }

        internal void VentasMetActualizarContexto()
        {
            // Un depósito agrupa recibos de distintos clientes; no hereda el filtro individual.
            this.Vista.VentasPnlCliente.Enabled = this.Vista.VentasTbcMovimientos.SelectedIndex < 3;
            this.CapturaMovimientos.VentasMetRefrescarBotones();
        }

        internal void VentasMetMostrarRecepcionPagos()
        {
            this.Vista.VentasTbcMovimientos.SelectedTab = this.Vista.VentasTbpRecibos;
            VentasMetActualizarContexto();
        }

        internal void VentasMetMostrarConsultaRecibos(string Recibo, string Deposito)
        {
            this.Vista.VentasTxtConsultaRecibo.Text = Recibo ?? "";
            this.Vista.VentasTxtConsultaDeposito.Text = Deposito ?? "";
            this.Vista.VentasTbcMovimientos.SelectedTab = this.Vista.VentasTbpConsultaRecibos;
            VentasMetActualizarContexto();
        }

        // ConsultaMovimientos.
        internal void VentasMetMostrarEstado()
        {
            this.Vista.VentasDgvMovimientos.Rows.Clear();
            DataRow R = this.EstadoCuenta.Tables[0].Rows[0];
            decimal Inicial = Convert.ToDecimal(R["Inicial"]), Cargos = Convert.ToDecimal(R["Cargos"]), Abonos = Convert.ToDecimal(R["Abonos"]);
            this.Vista.VentasLblValorInicial.Text = ClsHerramientasVentas.VentasFuncDinero(Inicial, this.CodigoMoneda);
            this.Vista.VentasLblValorCargos.Text = ClsHerramientasVentas.VentasFuncDinero(Cargos, this.CodigoMoneda);
            this.Vista.VentasLblValorAbonos.Text = ClsHerramientasVentas.VentasFuncDinero(Abonos, this.CodigoMoneda);
            this.Vista.VentasLblValorFinal.Text = ClsHerramientasVentas.VentasFuncDinero(Inicial + Cargos - Abonos, this.CodigoMoneda);
            string[] Tipos =
            {
                "",
                "FACTURA",
                "PAGO",
                "NOTA_CREDITO",
                "NOTA_DEBITO",
                "AJUSTE_POSITIVO",
                "AJUSTE_NEGATIVO"
            };
            string Tipo = Tipos[Math.Max(0, this.Vista.VentasCboTipoConsulta.SelectedIndex)];
            foreach (DataRow F in this.EstadoCuenta.Tables[1].Rows)
            {
                if (Tipo.Length > 0 && (string)F["Tipo"] != Tipo)
                    continue;
                if (this.Vista.VentasCboEstadoConsulta.SelectedIndex > 0 && !string.Equals((string)F["Estado"], this.Vista.VentasCboEstadoConsulta.Text, StringComparison.OrdinalIgnoreCase))
                    continue;
                int I = this.Vista.VentasDgvMovimientos.Rows.Add(F["Fecha"], F["Documento"], F["Tipo"], F["Descripcion"], F["Cargo"], F["Abono"], F["Saldo"], F["Estado"]);
                this.Vista.VentasDgvMovimientos.Rows[I].Tag = F;
            }

            this.Vista.VentasLblRegistros.Text = this.Vista.VentasDgvMovimientos.Rows.Count + " movimientos mostrados";
            this.Vista.VentasLblEstadoCierre.Text = "Los totales representan todo el periodo; los filtros solo cambian las filas mostradas.";
        }

        internal void VentasMetConsultarCaptura()
        {
            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpDocumento)
            {
                DataTable Datos = this.Controlador.VentasMetListarDocumentos(this.Cliente, this.Moneda);
                string Numero = this.Vista.VentasTxtNumeroDocumento.Text.Trim();
                DataRow F = Datos.AsEnumerable().FirstOrDefault(R => Convert.ToString(R["Documento"]) == Numero || Convert.ToString(R["Documento"]) == "CXC-" + Numero);
                if (F == null || Numero.Length == 0)
                    F = ClsHerramientasVentas.VentasMetSeleccionar(this.Vista, "Consultar documentos de CxC", Datos);
                if (F != null)
                    this.CapturaMovimientos.VentasMetCargarDocumento(Convert.ToInt32(F["Id"]), Convert.ToString(F["Clase"]));
            }
            else if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpRecibos)
            {
                DataTable Datos = this.Controlador.VentasMetListarRecibos(this.Cliente, this.Moneda);
                string Numero = this.Vista.VentasTxtRecibo.Text.Trim();
                DataRow F = Datos.AsEnumerable().FirstOrDefault(R => Convert.ToString(R["Recibo"]) == Numero);
                if (F == null || Numero.Length == 0)
                    F = ClsHerramientasVentas.VentasMetSeleccionar(this.Vista, "Consultar recibos", Datos);
                if (F != null)
                    this.CapturaMovimientos.VentasMetCargarRecibo(Convert.ToInt32(F["Id"]));
            }
            else
                VentasMetConsultarActual();
        }

        internal void VentasMetAbrirSeleccionActual()
        {
            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpCuenta)
            {
                DataRow F = this.Vista.VentasDgvMovimientos.CurrentRow == null ? null : this.Vista.VentasDgvMovimientos.CurrentRow.Tag as DataRow;
                this.Controlador.VentasMetValidarCaptura(F != null, "Seleccione un movimiento de la cuenta.");
                DataRow R = this.Controlador.VentasMetLeerMovimiento(Convert.ToInt32(F["idMovimiento"]));
                if (Convert.ToString(R["Clase"]) == "RECIBO")
                    this.CapturaMovimientos.VentasMetCargarRecibo(Convert.ToInt32(R["Id"]));
                else
                    this.CapturaMovimientos.VentasMetCargarDocumento(Convert.ToInt32(R["Id"]), Convert.ToString(R["Clase"]));
            }
            else if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpConsultaRecibos)
            {
                DataRow F = this.Vista.VentasDgvConsultaRecibos.CurrentRow == null ? null : this.Vista.VentasDgvConsultaRecibos.CurrentRow.Tag as DataRow;
                this.Controlador.VentasMetValidarCaptura(F != null, "Seleccione un recibo.");
                this.CapturaMovimientos.VentasMetCargarRecibo(Convert.ToInt32(F["idPago"]));
            }
            else if ((this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpDocumento && this.DocumentoActual == null) || (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpRecibos && this.ReciboActual == null))
                VentasMetConsultarCaptura();
        }

        // DetalleMovimientos.
        internal decimal VentasMetSumar(DataGridView Grid, int Columna)
        {
            return Grid.Rows.Cast<DataGridViewRow>().Where(F => F.Cells[0].Value is bool && (bool)F.Cells[0].Value).Sum(F => this.Controlador.VentasFuncImporteCelda(F.Cells[Columna].Value));
        }

        internal DataTable VentasMetSeleccionados(DataGridView Grid, string Id, int Importe)
        {
            this.Controlador.VentasMetValidarCaptura(!(!Grid.EndEdit()), "Corrija la celda en edicion.");
            var Tabla = new DataTable();
            Tabla.Columns.Add(Id, typeof(int));
            Tabla.Columns.Add("Importe", typeof(decimal));
            foreach (DataGridViewRow F in Grid.Rows)
                if (F.Cells[0].Value is bool && (bool)F.Cells[0].Value && F.Tag is DataRow)
                    Tabla.Rows.Add(((DataRow)F.Tag)[Id], this.Controlador.VentasFuncImporteCelda(F.Cells[Importe].Value));
            return Tabla;
        }

        // ImpresionMovimientos.
        internal void VentasMetImprimir()
        {
            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpCuenta)
            {
                ClsHerramientasVentas.VentasMetImprimir(this.Vista, "Estado de cuenta", this.Vista.VentasCboCliente.Text + " / " + this.CodigoMoneda, this.EstadoCuenta == null ? null : this.EstadoCuenta.Tables[1]);
                return;
            }

            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpDocumento)
            {
                ClsHerramientasVentas.VentasMetImprimir(this.Vista, "Documento interno de CxC", this.CodigoMoneda, this.Controlador.VentasMetDocumento(this.Cliente, this.Moneda, ClsHerramientasVentas.VentasFuncObtenerId(this.Vista.VentasCboEstablecimientoDocumento), this.Vista.VentasTxtNumeroDocumento.Text.Trim(), this.Vista.VentasCboTipoDocumento.SelectedIndex == 0));
                return;
            }

            if (this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpDepositos)
            {
                DataSet D = this.Controlador.VentasMetDeposito(this.Vista.VentasTxtCodigoDeposito.Text.Trim());
                DataRow H = D.Tables[0].Rows[0];
                ClsHerramientasVentas.VentasMetImprimir(this.Vista, "Preparacion local " + H["codigoDeposito"], "Estado: " + H["estadoDeposito"] + " / Referencia de cuenta: " + H["cuentaReferencia"] + " / Total: " + H["total"], D.Tables[1]);
                return;
            }

            bool Recepcion = this.Vista.VentasTbcMovimientos.SelectedTab == this.Vista.VentasTbpRecibos;
            DataTable Recibos = this.Controlador.VentasMetConsultarRecibos(Recepcion ? this.Vista.VentasTxtRecibo.Text.Trim() : this.Vista.VentasTxtConsultaRecibo.Text.Trim(), Recepcion ? "" : this.Vista.VentasTxtConsultaDeposito.Text.Trim());
            this.Controlador.VentasMetValidarCaptura(!(Recibos.Rows.Count != 1), "Consulte un solo recibo para imprimir su comprobante.");
            DataRow R = Recibos.Rows[0];
            ClsHerramientasVentas.VentasMetImprimir(this.Vista, "Recibo " + R["Recibo"], R["Cliente"] + " / " + R["Moneda"] + " / Total: " + Convert.ToDecimal(R["Monto"]).ToString("N2", ClsHerramientasVentas.Cultura) + " / " + R["Estado"], this.Controlador.VentasMetAplicaciones(Convert.ToInt32(R["idPago"])));
        }
    }
}
