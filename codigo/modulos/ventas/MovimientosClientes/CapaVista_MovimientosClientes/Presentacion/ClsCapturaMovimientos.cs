// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using CapaControlador_MovimientosClientes;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_MovimientosClientes
{
    internal sealed class ClsCapturaMovimientos
    {
        private readonly ClsMovimientosPresentador _Estado;
        internal ClsCapturaMovimientos(ClsMovimientosPresentador Estado)
        {
            _Estado = Estado;
        }

        // CapturaMovimientos.
        internal void VentasMetNuevo()
        {
            if (_Estado.Vista.VentasTbcMovimientos.SelectedIndex == 0)
                _Estado.Vista.VentasTbcMovimientos.SelectedTab = _Estado.Vista.VentasTbpDocumento;
            if (_Estado.Vista.VentasTbcMovimientos.SelectedTab == _Estado.Vista.VentasTbpConsultaRecibos)
                _Estado.Vista.VentasTbcMovimientos.SelectedTab = _Estado.Vista.VentasTbpRecibos;
            int Tab = _Estado.Vista.VentasTbcMovimientos.SelectedIndex;
            _Estado.Guardados[Tab] = false;
            if (Tab == 1)
            {
                _Estado.DocumentoActual = null;
                _Estado.EditandoDocumento = false;
            }

            if (Tab == 2)
            {
                _Estado.ReciboActual = null;
                _Estado.EditandoRecibo = false;
            }

            if (Tab == 1)
            {
                VentasMetEstadoNuevoDocumento();
                _Estado.Vista.VentasTxtNumeroDocumento.Clear();
                _Estado.Vista.VentasTxtFacturaReferencia.Clear();
                _Estado.Vista.VentasTxtDescripcionDocumento.Clear();
                _Estado.Vista.VentasNudImporteDocumento.Value = 0;
                _Estado.Vista.VentasCboTipoDocumento.SelectedIndex = 0;
                _Estado.Vista.VentasDtpFechaDocumento.Value = DateTime.Today;
                DataRow C = ClsHerramientasVentas.VentasFuncObtenerFila(_Estado.Vista.VentasCboCliente);
                _Estado.Vista.VentasDtpVencimientoDocumento.Value = DateTime.Today.AddDays(C == null ? 0 : Convert.ToInt32(C["diasCredito"]));
            }

            if (Tab == 2)
            {
                _Estado.Vista.VentasTxtRecibo.Clear();
                _Estado.Vista.VentasTxtReferenciaPago.Clear();
                _Estado.Vista.VentasTxtObservacionesPago.Clear();
                _Estado.Vista.VentasNudMontoPago.Value = 0;
                _Estado.Vista.VentasNudTipoCambioPago.Value = 1;
                _Estado.Vista.VentasDtpFechaPago.Value = DateTime.Today;
                _Estado.Vista.VentasCboFormaPago.SelectedIndex = 0;
                VentasMetPendientes();
            }

            if (Tab == 3)
            {
                _Estado.Vista.VentasTxtCodigoDeposito.Clear();
                _Estado.Vista.VentasTxtReferenciaDeposito.Clear();
                _Estado.Vista.VentasDtpFechaDeposito.Value = DateTime.Today;
                VentasMetDisponibles();
            }

            VentasMetRefrescarBotones();
            if (Tab == 1)
                _Estado.Vista.VentasLblEtapaBancos.Text = "Nuevo cargo: seleccione cliente, moneda y emisor; ingrese importe y motivo, y presione Guardar.";
            if (Tab < 3 && _Estado.Cliente <= 0)
                _Estado.Vista.VentasCboCliente.Focus();
            else if (Tab == 1)
                _Estado.Vista.VentasNudImporteDocumento.Focus();
            else if (Tab == 2)
                _Estado.Vista.VentasNudMontoPago.Focus();
        }

        internal void VentasMetRefrescarBotones()
        {
            int Tab = _Estado.Vista.VentasTbcMovimientos.SelectedIndex;
            _Estado.Vista.VentasBtnGuardar.Enabled = _Estado.Inicializado && Tab > 0 && Tab < 4 && !_Estado.Guardados[Tab];
            _Estado.Vista.VentasBtnIngresar.Enabled = _Estado.Inicializado;
            _Estado.Vista.VentasBtnCancelar.Enabled = _Estado.Vista.VentasBtnIngresar.Enabled;
            VentasMetActualizarAcciones();
        }

        internal void VentasMetActualizarAcciones()
        {
            int Tab = _Estado.Vista.VentasTbcMovimientos.SelectedIndex;
            bool Documento = Tab == 1 && _Estado.DocumentoActual != null, Recibo = Tab == 2 && _Estado.ReciboActual != null;
            _Estado.Vista.VentasPnlCliente.Enabled = Tab < 3 && !Documento && !Recibo;
            foreach (Control C in new Control[]
            {
                _Estado.Vista.VentasCboTipoDocumento,
                _Estado.Vista.VentasTxtNumeroDocumento,
                _Estado.Vista.VentasDtpFechaDocumento,
                _Estado.Vista.VentasNudImporteDocumento
            }

            )
                C.Enabled = !Documento;
            _Estado.Vista.VentasCboEstablecimientoDocumento.Enabled = !Documento && _Estado.Vista.VentasCboTipoDocumento.SelectedIndex == 0;
            _Estado.Vista.VentasTxtFacturaReferencia.Enabled = !Documento && _Estado.Vista.VentasCboTipoDocumento.SelectedIndex > 0;
            _Estado.SeleccionarFactura.Enabled = _Estado.Vista.VentasTxtFacturaReferencia.Enabled;
            _Estado.Vista.VentasTxtDescripcionDocumento.ReadOnly = Documento && !_Estado.EditandoDocumento;
            _Estado.Vista.VentasDtpVencimientoDocumento.Enabled = !Documento && _Estado.Vista.VentasCboTipoDocumento.SelectedIndex == 0 || _Estado.EditandoDocumento && Convert.ToString(_Estado.DocumentoActual["Clase"]) == "FACTURA";
            foreach (Control C in new Control[]
            {
                _Estado.Vista.VentasTxtRecibo,
                _Estado.Vista.VentasDtpFechaPago,
                _Estado.Vista.VentasNudMontoPago,
                _Estado.Vista.VentasNudTipoCambioPago,
                _Estado.Vista.VentasCboFormaPago,
                _Estado.Vista.VentasTxtReferenciaPago
            }

            )
                C.Enabled = !Recibo;
            _Estado.Vista.VentasDgvRecibosDeposito.ReadOnly = _Estado.Guardados[3];
            _Estado.Vista.VentasDgvDocumentos.ReadOnly = Recibo;
            _Estado.Vista.VentasTxtObservacionesPago.ReadOnly = Recibo && !_Estado.EditandoRecibo;
            _Estado.Vista.VentasBtnGuardar.Enabled = _Estado.Inicializado && Tab > 0 && Tab < 4 && (!_Estado.Guardados[Tab] || Tab == 1 && _Estado.EditandoDocumento || Tab == 2 && _Estado.EditandoRecibo);
            _Estado.Vista.VentasBtnModificar.Enabled = _Estado.Vista.VentasBtnEliminar.Enabled = _Estado.Inicializado && Tab != 3;
        }

        internal void VentasMetTipoDocumentoCambio()
        {
            // Presenta el efecto y los campos requeridos del documento.
            int Tipo = _Estado.Vista.VentasCboTipoDocumento.SelectedIndex;
            _Estado.Vista.VentasTxtNaturalezaDocumento.Text = Tipo < 0 ? string.Empty : (Tipo == 1 || Tipo == 4 ? "Abono - disminuye la deuda" : "Cargo - aumenta la deuda");
            if (_Estado.DocumentoActual == null && !_Estado.Cargando)
            {
                _Estado.Vista.VentasTxtFacturaReferencia.Clear();
                if (Tipo == 0)
                    VentasMetVencimientoCargo();
            }

            VentasMetActualizarAcciones();
        }

        internal void VentasMetEstadoNuevoDocumento()
        {
            _Estado.Vista.VentasCboEstadoDocumento.Items.Clear();
            _Estado.Vista.VentasCboEstadoDocumento.Items.Add("Pendiente de guardar");
            _Estado.Vista.VentasCboEstadoDocumento.SelectedIndex = 0;
            _Estado.Vista.VentasCboEstadoDocumento.Enabled = false;
        }

        internal void VentasMetVencimientoCargo()
        {
            if (_Estado.DocumentoActual != null || _Estado.Vista.VentasCboTipoDocumento.SelectedIndex != 0)
                return;
            DataRow C = ClsHerramientasVentas.VentasFuncObtenerFila(_Estado.Vista.VentasCboCliente);
            _Estado.Vista.VentasDtpVencimientoDocumento.Value = _Estado.Vista.VentasDtpFechaDocumento.Value.Date.AddDays(C == null ? 0 : Convert.ToInt32(C["diasCredito"]));
        }

        internal void VentasMetSeleccionarFactura()
        {
            _Estado.Controlador.VentasMetValidarCaptura(_Estado.Cliente > 0 && _Estado.Moneda > 0, "Seleccione primero el cliente y la moneda.");
            DataTable Datos = _Estado.Controlador.VentasMetListarDocumentos(_Estado.Cliente, _Estado.Moneda);
            DataTable Facturas = Datos.Clone();
            foreach (DataRow F in Datos.Rows)
                if (Convert.ToString(F["Clase"]) == "FACTURA" && Convert.ToString(F["Estado"]) == "EMITIDA")
                    Facturas.ImportRow(F);
            _Estado.Controlador.VentasMetValidarCaptura(Facturas.Rows.Count > 0, "El cliente no tiene documentos emitidos en esta moneda. Registre primero un Cargo inicial.");
            DataRow Seleccion = ClsHerramientasVentas.VentasMetSeleccionar(_Estado.Vista, "Factura a la que se aplica la nota o ajuste", Facturas);
            if (Seleccion != null)
                _Estado.Vista.VentasTxtFacturaReferencia.Text = Convert.ToString(Seleccion["Documento"]);
        }

        // EdicionMovimientos.
        internal void VentasMetCargarDocumento(int Id, string Clase)
        {
            DataRow F = _Estado.Controlador.VentasMetLeerDocumento(Id, Clase);
            _Estado.Cargando = true;
            try
            {
                _Estado.Vista.VentasTbcMovimientos.SelectedTab = _Estado.Vista.VentasTbpDocumento;
                _Estado.Vista.VentasCboCliente.SelectedValue = F["idCliente"];
                _Estado.Vista.VentasCboMoneda.SelectedValue = F["idMoneda"];
                _Estado.Vista.VentasCboEstablecimientoDocumento.SelectedValue = F["idEstablecimiento"];
                _Estado.Vista.VentasCboTipoDocumento.SelectedIndex = Convert.ToInt32(F["Tipo"]);
                _Estado.Vista.VentasTxtNumeroDocumento.Text = Convert.ToString(F["Numero"]);
                _Estado.Vista.VentasDtpFechaDocumento.Value = Convert.ToDateTime(F["Fecha"]);
                _Estado.Vista.VentasDtpVencimientoDocumento.Value = Convert.ToDateTime(F["Vencimiento"]);
                _Estado.Vista.VentasNudImporteDocumento.Value = Convert.ToDecimal(F["Importe"]);
                _Estado.Vista.VentasTxtFacturaReferencia.Text = Convert.ToString(F["Referencia"]);
                _Estado.Vista.VentasTxtDescripcionDocumento.Text = Convert.ToString(F["Descripcion"]);
                _Estado.Vista.VentasCboEstadoDocumento.Items.Clear();
                _Estado.Vista.VentasCboEstadoDocumento.Items.Add(Convert.ToString(F["Estado"]));
                _Estado.Vista.VentasCboEstadoDocumento.SelectedIndex = 0;
                DataRow ClienteActual = ClsHerramientasVentas.VentasFuncObtenerFila(_Estado.Vista.VentasCboCliente);
                _Estado.Vista.VentasTxtCondicion.Text = ClienteActual == null ? "" : ClienteActual["diasCredito"] + " dias; limite " + Convert.ToDecimal(ClienteActual["limiteCredito"]).ToString("N2", ClsHerramientasVentas.Cultura);
                _Estado.DocumentoActual = F;
                _Estado.EditandoDocumento = false;
                _Estado.Guardados[1] = true;
            }
            finally
            {
                _Estado.Cargando = false;
            }

            VentasMetTipoDocumentoCambio();
            VentasMetRefrescarBotones();
            _Estado.Vista.VentasLblEtapaBancos.Text = "Documento consultado. Modificar permite cambiar la descripcion y el vencimiento de una factura.";
        }

        internal void VentasMetCargarRecibo(int Id)
        {
            DataSet Datos = _Estado.Controlador.VentasMetLeerRecibo(Id);
            DataRow F = Datos.Tables[0].Rows[0];
            _Estado.Cargando = true;
            try
            {
                _Estado.Vista.VentasTbcMovimientos.SelectedTab = _Estado.Vista.VentasTbpRecibos;
                _Estado.Vista.VentasCboCliente.SelectedValue = F["idCliente"];
                _Estado.Vista.VentasCboMoneda.SelectedValue = F["idMoneda"];
                _Estado.Vista.VentasTxtRecibo.Text = Convert.ToString(F["numeroRecibo"]);
                _Estado.Vista.VentasDtpFechaPago.Value = Convert.ToDateTime(F["fechaPago"]);
                _Estado.Vista.VentasNudMontoPago.Value = Convert.ToDecimal(F["montoPago"]);
                _Estado.Vista.VentasNudTipoCambioPago.Value = Convert.ToDecimal(F["tipoCambio"]);
                string[] Formas =
                {
                    "EFECTIVO",
                    "TRANSFERENCIA",
                    "DEPOSITO",
                    "CHEQUE",
                    "TARJETA"
                };
                _Estado.Vista.VentasCboFormaPago.SelectedIndex = Array.IndexOf(Formas, Convert.ToString(F["formaPago"]));
                _Estado.Vista.VentasTxtReferenciaPago.Text = Convert.ToString(F["referenciaPago"]);
                _Estado.Vista.VentasTxtObservacionesPago.Text = Convert.ToString(F["observaciones"]);
                _Estado.Vista.VentasDgvDocumentos.Rows.Clear();
                foreach (DataRow A in Datos.Tables[1].Rows)
                {
                    int I = _Estado.Vista.VentasDgvDocumentos.Rows.Add(true, A["Documento"], A["Fecha"], A["Vencimiento"], A["Moneda"], A["Saldo"], A["Importe"]);
                    _Estado.Vista.VentasDgvDocumentos.Rows[I].Tag = A;
                }

                DataRow ClienteActual = ClsHerramientasVentas.VentasFuncObtenerFila(_Estado.Vista.VentasCboCliente);
                _Estado.Vista.VentasTxtCondicion.Text = ClienteActual == null ? "" : ClienteActual["diasCredito"] + " dias; limite " + Convert.ToDecimal(ClienteActual["limiteCredito"]).ToString("N2", ClsHerramientasVentas.Cultura);
                _Estado.ReciboActual = F;
                _Estado.EditandoRecibo = false;
                _Estado.Guardados[2] = true;
            }
            finally
            {
                _Estado.Cargando = false;
            }

            VentasMetTotalesPago();
            VentasMetRefrescarBotones();
            _Estado.Vista.VentasLblEtapaBancos.Text = "Recibo " + F["numeroRecibo"] + " / " + F["estadoPago"] + ". Modificar permite actualizar sus observaciones.";
        }

        internal void VentasMetModificar()
        {
            _Estado.VentasMetAbrirSeleccionActual();
            if (_Estado.Vista.VentasTbcMovimientos.SelectedTab == _Estado.Vista.VentasTbpDocumento)
            {
                _Estado.Controlador.VentasMetValidarCaptura(_Estado.DocumentoActual != null && Convert.ToString(_Estado.DocumentoActual["Estado"]) != "ANULADA", "Consulte un documento aplicado antes de modificarlo.");
                _Estado.EditandoDocumento = true;
                VentasMetRefrescarBotones();
                _Estado.Vista.VentasTxtDescripcionDocumento.Focus();
            }
            else if (_Estado.Vista.VentasTbcMovimientos.SelectedTab == _Estado.Vista.VentasTbpRecibos)
            {
                _Estado.Controlador.VentasMetValidarCaptura(_Estado.ReciboActual != null && Convert.ToString(_Estado.ReciboActual["estadoPago"]) == "CONFIRMADO", "Consulte un recibo confirmado antes de modificarlo.");
                _Estado.EditandoRecibo = true;
                VentasMetRefrescarBotones();
                _Estado.Vista.VentasTxtObservacionesPago.Focus();
            }
        }

        internal void VentasMetAnular()
        {
            _Estado.VentasMetAbrirSeleccionActual();
            bool Recibo = _Estado.Vista.VentasTbcMovimientos.SelectedTab == _Estado.Vista.VentasTbpRecibos;
            _Estado.Controlador.VentasMetValidarCaptura(Recibo ? _Estado.ReciboActual != null : _Estado.DocumentoActual != null, "Consulte el registro a anular.");
            string Numero = Recibo ? Convert.ToString(_Estado.ReciboActual["numeroRecibo"]) : Convert.ToString(_Estado.DocumentoActual["Numero"]);
            string Motivo = ClsHerramientasVentas.VentasMetMotivo(_Estado.Vista, "Anular " + Numero);
            if (Motivo == null)
                return;
            if (Recibo)
            {
                int Id = Convert.ToInt32(_Estado.ReciboActual["idPago"]);
                _Estado.Controlador.VentasMetAnularRecibo(Id, DateTime.Today, _Estado.VentasIdUsuario, Motivo);
                VentasMetCargarRecibo(Id);
            }
            else
            {
                int Id = Convert.ToInt32(_Estado.DocumentoActual["Id"]);
                string Clase = Convert.ToString(_Estado.DocumentoActual["Clase"]);
                _Estado.Controlador.VentasMetAnularDocumento(Id, Clase, _Estado.VentasIdUsuario, Motivo);
                VentasMetCargarDocumento(Id, Clase);
            }

            Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(_Estado.Vista, "Registro anulado. Se conservaron los movimientos originales y su reverso.");
        }

        internal bool VentasMetGuardarModificacion()
        {
            if (_Estado.Vista.VentasTbcMovimientos.SelectedTab == _Estado.Vista.VentasTbpDocumento && _Estado.EditandoDocumento)
            {
                int Id = Convert.ToInt32(_Estado.DocumentoActual["Id"]);
                string Clase = Convert.ToString(_Estado.DocumentoActual["Clase"]);
                _Estado.Controlador.VentasMetModificarDocumento(Id, Clase, _Estado.VentasIdUsuario, _Estado.Vista.VentasTxtDescripcionDocumento.Text, _Estado.Vista.VentasDtpVencimientoDocumento.Value);
                VentasMetCargarDocumento(Id, Clase);
                Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(_Estado.Vista, "Documento actualizado.");
                return true;
            }

            if (_Estado.Vista.VentasTbcMovimientos.SelectedTab == _Estado.Vista.VentasTbpRecibos && _Estado.EditandoRecibo)
            {
                int Id = Convert.ToInt32(_Estado.ReciboActual["idPago"]);
                _Estado.Controlador.VentasMetModificarRecibo(Id, _Estado.VentasIdUsuario, _Estado.Vista.VentasTxtObservacionesPago.Text);
                VentasMetCargarRecibo(Id);
                Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(_Estado.Vista, "Observaciones del recibo actualizadas.");
                return true;
            }

            return false;
        }

        // GuardadoMovimientos.
        internal void VentasMetGuardar()
        {
            if (VentasMetGuardarModificacion())
                return;
            int Tab = _Estado.Vista.VentasTbcMovimientos.SelectedIndex;
            _Estado.Controlador.VentasMetValidarCaptura(!(_Estado.Guardados[Tab]), "El registro ya se guardo. Use Ingresar para comenzar otro.");
            string Numero;
            if (Tab == 1)
            {
                Numero = _Estado.Controlador.VentasMetRegistrarDocumento(_Estado.Cliente, _Estado.Moneda, _Estado.VentasIdUsuario, ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboEstablecimientoDocumento), _Estado.Vista.VentasTxtNumeroDocumento.Text, _Estado.Vista.VentasCboTipoDocumento.SelectedIndex, _Estado.Vista.VentasDtpFechaDocumento.Value, _Estado.Vista.VentasDtpVencimientoDocumento.Value, _Estado.Vista.VentasNudImporteDocumento.Value, _Estado.Vista.VentasTxtFacturaReferencia.Text, _Estado.Vista.VentasTxtDescripcionDocumento.Text);
                _Estado.Vista.VentasTxtNumeroDocumento.Text = Numero;
                bool Abono = _Estado.Vista.VentasCboTipoDocumento.SelectedIndex == 1 || _Estado.Vista.VentasCboTipoDocumento.SelectedIndex == 4;
                Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(_Estado.Vista, (Abono ? "Abono registrado; disminuye la deuda: " : "Cargo registrado; aumenta la deuda: ") + Numero);
            }
            else if (Tab == 2)
            {
                string[] Formas =
                {
                    "EFECTIVO",
                    "TRANSFERENCIA",
                    "DEPOSITO",
                    "CHEQUE",
                    "TARJETA"
                };
                _Estado.Controlador.VentasMetValidarCaptura(!(_Estado.Vista.VentasCboFormaPago.SelectedIndex < 0), "Seleccione la forma de pago.");
                Numero = _Estado.Controlador.VentasMetRegistrarRecibo(_Estado.Cliente, _Estado.Moneda, _Estado.VentasIdUsuario, _Estado.Vista.VentasTxtRecibo.Text, _Estado.Vista.VentasDtpFechaPago.Value, _Estado.Vista.VentasNudMontoPago.Value, _Estado.Vista.VentasNudTipoCambioPago.Value, Formas[_Estado.Vista.VentasCboFormaPago.SelectedIndex], _Estado.Vista.VentasTxtReferenciaPago.Text, _Estado.Vista.VentasTxtObservacionesPago.Text, _Estado.VentasMetSeleccionados(_Estado.Vista.VentasDgvDocumentos, "idFactura", 6));
                _Estado.Vista.VentasTxtRecibo.Text = Numero;
                VentasMetPendientes();
                Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(_Estado.Vista, "Recibo generado y aplicado correctamente: " + Numero);
            }
            else if (Tab == 3)
            {
                Numero = _Estado.Controlador.VentasMetPrepararDeposito(ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboMonedaDeposito), _Estado.VentasIdUsuario, _Estado.Vista.VentasDtpFechaDeposito.Value, _Estado.Vista.VentasTxtCodigoDeposito.Text, ClsCapturaMovimientos.VentasMetReferencia(_Estado.Vista.VentasCboEmpresaDeposito), ClsCapturaMovimientos.VentasMetReferencia(_Estado.Vista.VentasCboBancoDestino), ClsCapturaMovimientos.VentasMetReferencia(_Estado.Vista.VentasCboCuentaDestino), ClsCapturaMovimientos.VentasMetReferencia(_Estado.Vista.VentasCboConceptoBancario), _Estado.Vista.VentasTxtReferenciaDeposito.Text, _Estado.VentasMetSeleccionados(_Estado.Vista.VentasDgvRecibosDeposito, "idPago", 7));
                _Estado.Vista.VentasTxtCodigoDeposito.Text = Numero;
                VentasMetDisponibles();
                Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarAdvertencia(_Estado.Vista, "Deposito preparado localmente: " + Numero + ". El registro en Bancos requiere su enlace.", "Depósito pendiente");
            }
            else
            {
                _Estado.Controlador.VentasMetValidarCaptura(false, "Seleccione una pestana de captura.");
                return;
            }

            _Estado.Guardados[Tab] = true;
            if (Tab == 1)
            {
                string Documento = _Estado.Vista.VentasCboTipoDocumento.SelectedIndex == 0 ? "CXC-" + Numero : Numero;
                DataRow F = _Estado.Controlador.VentasMetListarDocumentos(_Estado.Cliente, _Estado.Moneda).AsEnumerable().Single(R => Convert.ToString(R["Documento"]) == Documento);
                VentasMetCargarDocumento(Convert.ToInt32(F["Id"]), Convert.ToString(F["Clase"]));
            }
            else if (Tab == 2)
                VentasMetCargarRecibo(Convert.ToInt32(_Estado.Controlador.VentasMetConsultarRecibos(Numero, "").Rows[0]["idPago"]));
            VentasMetRefrescarBotones();
        }

        // PagoMovimientos.
        internal void VentasMetPendientes()
        {
            _Estado.Cargando = true;
            try
            {
                _Estado.Vista.VentasDgvDocumentos.Rows.Clear();
                foreach (DataRow F in _Estado.Controlador.VentasMetPendientes(_Estado.Cliente, _Estado.Moneda).Rows)
                {
                    int I = _Estado.Vista.VentasDgvDocumentos.Rows.Add(false, F["Documento"], F["Fecha"], F["Vencimiento"], F["Moneda"], F["Saldo"], 0m);
                    _Estado.Vista.VentasDgvDocumentos.Rows[I].Tag = F;
                }
            }
            finally
            {
                _Estado.Cargando = false;
            }

            VentasMetTotalesPago();
        }

        internal void VentasMetAplicacionCambio(object Sender, DataGridViewCellEventArgs E)
        {
            if (_Estado.Cargando || E.RowIndex < 0)
                return;
            ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, () =>
            {
                if (E.ColumnIndex == 0 && Convert.ToBoolean(_Estado.Vista.VentasDgvDocumentos.Rows[E.RowIndex].Cells[0].Value))
                {
                    var F = _Estado.Vista.VentasDgvDocumentos.Rows[E.RowIndex];
                    if (_Estado.Controlador.VentasFuncImporteCelda(F.Cells[6].Value) == 0)
                        F.Cells[6].Value = _Estado.Vista.VentasNudMontoPago.Value == 0 ? _Estado.Controlador.VentasFuncImporteCelda(F.Cells[5].Value) : Math.Min(_Estado.Controlador.VentasFuncImporteCelda(F.Cells[5].Value), Math.Max(0, _Estado.Vista.VentasNudMontoPago.Value - _Estado.VentasMetSumar(_Estado.Vista.VentasDgvDocumentos, 6)));
                }

                VentasMetTotalesPago();
            });
        }

        internal void VentasMetTotalesPago()
        {
            if (_Estado.Cargando)
                return;
            decimal Aplicado = _Estado.VentasMetSumar(_Estado.Vista.VentasDgvDocumentos, 6);
            _Estado.Vista.VentasLblValorRecibido.Text = ClsHerramientasVentas.VentasFuncDinero(_Estado.Vista.VentasNudMontoPago.Value, _Estado.CodigoMoneda);
            _Estado.Vista.VentasLblValorAplicado.Text = ClsHerramientasVentas.VentasFuncDinero(Aplicado, _Estado.CodigoMoneda);
            _Estado.Vista.VentasLblValorDisponible.Text = ClsHerramientasVentas.VentasFuncDinero(_Estado.Vista.VentasNudMontoPago.Value - Aplicado, _Estado.CodigoMoneda);
        }

        // DepositoMovimientos.
        internal void VentasMetConfigurarCatalogosBancos(DataTable Empresas, DataTable Bancos, DataTable Cuentas, DataTable Conceptos, string Modo)
        {
            // Tablas con Id y Nombre suministradas por el modulo dueno de esos catalogos.
            foreach (var Combo in new[]
            {
                _Estado.Vista.VentasCboEmpresaDeposito,
                _Estado.Vista.VentasCboBancoDestino,
                _Estado.Vista.VentasCboCuentaDestino,
                _Estado.Vista.VentasCboConceptoBancario
            }

            )
                Combo.DropDownStyle = ComboBoxStyle.DropDownList;
            ClsHerramientasVentas.VentasMetCargarOpciones(_Estado.Vista.VentasCboEmpresaDeposito, Empresas);
            ClsHerramientasVentas.VentasMetCargarOpciones(_Estado.Vista.VentasCboBancoDestino, Bancos);
            ClsHerramientasVentas.VentasMetCargarOpciones(_Estado.Vista.VentasCboCuentaDestino, Cuentas);
            ClsHerramientasVentas.VentasMetCargarOpciones(_Estado.Vista.VentasCboConceptoBancario, Conceptos);
            _Estado.Vista.VentasTxtModoActualizacion.Text = Modo;
        }

        internal static string VentasMetReferencia(ComboBox Combo)
        {
            return Combo.SelectedValue == null ? Combo.Text.Trim() : Convert.ToString(Combo.SelectedValue);
        }

        internal void VentasMetDisponibles()
        {
            _Estado.Cargando = true;
            try
            {
                _Estado.Vista.VentasDgvRecibosDeposito.Rows.Clear();
                foreach (DataRow F in _Estado.Controlador.VentasMetRecibosDisponibles(ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboMonedaDeposito)).Rows)
                {
                    int I = _Estado.Vista.VentasDgvRecibosDeposito.Rows.Add(false, F["Recibo"], F["Cliente"], F["Moneda"], F["Monto"], F["Depositado"], F["Disponible"], 0m);
                    _Estado.Vista.VentasDgvRecibosDeposito.Rows[I].Tag = F;
                }
            }
            finally
            {
                _Estado.Cargando = false;
            }

            _Estado.Vista.VentasTxtTotalDeposito.Text = "0.00";
        }

        internal void VentasMetDepositoCambio(object Sender, DataGridViewCellEventArgs E)
        {
            if (_Estado.Cargando || E.RowIndex < 0)
                return;
            ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, () =>
            {
                var F = _Estado.Vista.VentasDgvRecibosDeposito.Rows[E.RowIndex];
                if (E.ColumnIndex == 0 && Convert.ToBoolean(F.Cells[0].Value) && _Estado.Controlador.VentasFuncImporteCelda(F.Cells[7].Value) == 0)
                    F.Cells[7].Value = F.Cells[6].Value;
                _Estado.Vista.VentasTxtTotalDeposito.Text = _Estado.VentasMetSumar(_Estado.Vista.VentasDgvRecibosDeposito, 7).ToString("N2", ClsHerramientasVentas.Cultura);
            });
        }

        internal void VentasMetConsultarDeposito()
        {
            DataSet Datos = _Estado.Controlador.VentasMetDeposito(_Estado.Vista.VentasTxtCodigoDeposito.Text.Trim());
            DataRow F = Datos.Tables[0].Rows[0];
            _Estado.Cargando = true;
            try
            {
                _Estado.Vista.VentasCboMonedaDeposito.SelectedValue = F["idMoneda"];
                _Estado.Vista.VentasDtpFechaDeposito.Value = Convert.ToDateTime(F["fechaDeposito"]);
                _Estado.Vista.VentasCboEmpresaDeposito.Text = Convert.ToString(F["empresaReferencia"]);
                _Estado.Vista.VentasCboBancoDestino.Text = Convert.ToString(F["bancoReferencia"]);
                _Estado.Vista.VentasCboCuentaDestino.Text = Convert.ToString(F["cuentaReferencia"]);
                _Estado.Vista.VentasCboConceptoBancario.Text = Convert.ToString(F["conceptoReferencia"]);
                _Estado.Vista.VentasTxtReferenciaDeposito.Text = Convert.ToString(F["referenciaDeposito"]);
                _Estado.Vista.VentasDgvRecibosDeposito.Rows.Clear();
                foreach (DataRow D in Datos.Tables[1].Rows)
                    _Estado.Vista.VentasDgvRecibosDeposito.Rows.Add(true, D["Recibo"], D["Cliente"], _Estado.Vista.VentasCboMonedaDeposito.Text, D["Importe"], D["Importe"], 0m, D["Importe"]);
                _Estado.Vista.VentasTxtTotalDeposito.Text = Convert.ToDecimal(F["total"]).ToString("N2", ClsHerramientasVentas.Cultura);
                _Estado.Vista.VentasTxtEstadoEnvio.Text = Convert.ToString(F["estadoDeposito"]);
                _Estado.Guardados[3] = true;
            }
            finally
            {
                _Estado.Cargando = false;
            }

            VentasMetRefrescarBotones();
            _Estado.Vista.VentasLblEtapaBancos.Text = "Deposito preparado localmente. Su confirmacion bancaria corresponde al modulo de Bancos.";
        }

        // ReciboMovimientos.
        internal void VentasMetConsultarRecibos()
        {
            _Estado.Cargando = true;
            try
            {
                _Estado.Vista.VentasDgvConsultaRecibos.Rows.Clear();
                _Estado.Vista.VentasDgvConsultaAplicaciones.Rows.Clear();
                _Estado.Vista.VentasTxtMovimientoBancario.Clear();
                foreach (DataRow F in _Estado.Controlador.VentasMetConsultarRecibos(_Estado.Vista.VentasTxtConsultaRecibo.Text.Trim(), _Estado.Vista.VentasTxtConsultaDeposito.Text.Trim()).Rows)
                {
                    int I = _Estado.Vista.VentasDgvConsultaRecibos.Rows.Add(F["Recibo"], F["Fecha"], F["Cliente"], F["Moneda"], F["Monto"], F["Estado"], F["Deposito"], F["Depositado"]);
                    _Estado.Vista.VentasDgvConsultaRecibos.Rows[I].Tag = F;
                }
            }
            finally
            {
                _Estado.Cargando = false;
            }

            VentasMetAplicacionesConsulta();
        }

        internal void VentasMetAplicacionesConsulta()
        {
            DataRow F = _Estado.Vista.VentasDgvConsultaRecibos.CurrentRow == null ? null : _Estado.Vista.VentasDgvConsultaRecibos.CurrentRow.Tag as DataRow;
            _Estado.Vista.VentasDgvConsultaAplicaciones.Rows.Clear();
            if (F == null)
                return;
            _Estado.Vista.VentasTxtMovimientoBancario.Text = Convert.ToString(F["MovimientoBancario"]);
            foreach (DataRow A in _Estado.Controlador.VentasMetAplicaciones(Convert.ToInt32(F["idPago"])).Rows)
                _Estado.Vista.VentasDgvConsultaAplicaciones.Rows.Add(A["Factura"], A["Fecha"], A["Importe"], A["Estado"]);
        }
    }
}
