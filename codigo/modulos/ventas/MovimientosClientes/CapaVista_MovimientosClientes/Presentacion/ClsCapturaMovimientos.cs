// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
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
        private readonly FrmMovimientosClientesVentas _Vista;

        internal ClsCapturaMovimientos(FrmMovimientosClientesVentas Vista)
        {
            _Vista = Vista;
        }

        internal void VentasMetNuevo()
        {
            if (_Vista.VentasTbcMovimientos.SelectedIndex == 0)
                _Vista.VentasTbcMovimientos.SelectedTab = _Vista.VentasTbpDocumento;
            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpConsultaRecibos)
                _Vista.VentasTbcMovimientos.SelectedTab = _Vista.VentasTbpRecibos;
            int Tab = _Vista.VentasTbcMovimientos.SelectedIndex;
            _Vista.Guardados[Tab] = false;
            if (Tab == 1)
            {
                _Vista.DocumentoActual = null;
                _Vista.EditandoDocumento = false;
            }

            if (Tab == 2)
            {
                _Vista.ReciboActual = null;
                _Vista.EditandoRecibo = false;
            }

            if (Tab == 1)
            {
                VentasMetEstadoNuevoDocumento();
                _Vista.VentasTxtNumeroDocumento.Clear();
                _Vista.VentasTxtFacturaReferencia.Clear();
                _Vista.VentasTxtDescripcionDocumento.Clear();
                _Vista.VentasNudImporteDocumento.Value = 0;
                _Vista.VentasCboTipoDocumento.SelectedIndex = 0;
                _Vista.VentasDtpFechaDocumento.Value = DateTime.Today;
                DataRow ClienteSeleccionado = ClsHerramientasVentas.VentasFuncObtenerFila(_Vista.VentasCboCliente);
                _Vista.VentasDtpVencimientoDocumento.Value = DateTime.Today.AddDays(ClienteSeleccionado == null ? 0 : Convert.ToInt32(ClienteSeleccionado["diasCredito"]));
            }

            if (Tab == 2)
            {
                _Vista.VentasTxtRecibo.Clear();
                _Vista.VentasTxtReferenciaPago.Clear();
                _Vista.VentasTxtObservacionesPago.Clear();
                _Vista.VentasNudMontoPago.Value = 0;
                _Vista.VentasNudTipoCambioPago.Value = 1;
                _Vista.VentasDtpFechaPago.Value = DateTime.Today;
                _Vista.VentasCboFormaPago.SelectedIndex = 0;
                VentasMetPendientes();
            }

            if (Tab == 3)
            {
                _Vista.VentasTxtCodigoDeposito.Clear();
                _Vista.VentasTxtReferenciaDeposito.Clear();
                _Vista.VentasDtpFechaDeposito.Value = DateTime.Today;
                _Vista.ConsultasMovimientos.VentasMetDisponibles();
            }

            VentasMetRefrescarBotones();
            if (Tab == 1)
                _Vista.VentasLblEtapaBancos.Text = "Nuevo cargo: seleccione cliente, moneda y emisor; ingrese importe y motivo, y presione Guardar.";
            if (Tab < 3 && _Vista.Cliente <= 0)
                _Vista.VentasCboCliente.Focus();
            else if (Tab == 1)
                _Vista.VentasNudImporteDocumento.Focus();
            else if (Tab == 2)
                _Vista.VentasNudMontoPago.Focus();
        }

        internal void VentasMetRefrescarBotones()
        {
            int Tab = _Vista.VentasTbcMovimientos.SelectedIndex;
            _Vista.VentasBtnGuardar.Enabled = _Vista.Inicializado && Tab > 0 && Tab < 4 && !_Vista.Guardados[Tab];
            _Vista.VentasBtnIngresar.Enabled = _Vista.Inicializado;
            _Vista.VentasBtnCancelar.Enabled = _Vista.VentasBtnIngresar.Enabled;
            VentasMetActualizarAcciones();
        }

        internal void VentasMetActualizarAcciones()
        {
            int Tab = _Vista.VentasTbcMovimientos.SelectedIndex;
            bool Documento = Tab == 1 && _Vista.DocumentoActual != null, Recibo = Tab == 2 && _Vista.ReciboActual != null;
            _Vista.VentasPnlCliente.Enabled = Tab < 3 && !Documento && !Recibo;
            foreach (Control Control2 in new Control[]
            {
                _Vista.VentasCboTipoDocumento,
                _Vista.VentasTxtNumeroDocumento,
                _Vista.VentasDtpFechaDocumento,
                _Vista.VentasNudImporteDocumento
            })
                Control2.Enabled = !Documento;
            _Vista.VentasCboEstablecimientoDocumento.Enabled = !Documento && _Vista.VentasCboTipoDocumento.SelectedIndex == 0;
            _Vista.VentasTxtFacturaReferencia.Enabled = !Documento && _Vista.VentasCboTipoDocumento.SelectedIndex > 0;
            _Vista.VentasBtnSeleccionarFactura.Enabled = _Vista.VentasTxtFacturaReferencia.Enabled;
            _Vista.VentasTxtDescripcionDocumento.ReadOnly = Documento && !_Vista.EditandoDocumento;
            _Vista.VentasDtpVencimientoDocumento.Enabled = !Documento && _Vista.VentasCboTipoDocumento.SelectedIndex == 0 || _Vista.EditandoDocumento && Convert.ToString(_Vista.DocumentoActual["Clase"]) == "FACTURA";
            foreach (Control Control3 in new Control[]
            {
                _Vista.VentasTxtRecibo,
                _Vista.VentasDtpFechaPago,
                _Vista.VentasNudMontoPago,
                _Vista.VentasNudTipoCambioPago,
                _Vista.VentasCboFormaPago,
                _Vista.VentasTxtReferenciaPago
            })
                Control3.Enabled = !Recibo;
            _Vista.VentasDgvRecibosDeposito.ReadOnly = _Vista.Guardados[3];
            _Vista.VentasDgvDocumentos.ReadOnly = Recibo;
            _Vista.VentasTxtObservacionesPago.ReadOnly = Recibo && !_Vista.EditandoRecibo;
            _Vista.VentasBtnGuardar.Enabled = _Vista.Inicializado && Tab > 0 && Tab < 4 && (!_Vista.Guardados[Tab] || Tab == 1 && _Vista.EditandoDocumento || Tab == 2 && _Vista.EditandoRecibo);
            _Vista.VentasBtnModificar.Enabled = _Vista.VentasBtnEliminar.Enabled = _Vista.Inicializado && Tab != 3;
        }

        internal void VentasMetTipoDocumentoCambio()
        {
            // Presenta el efecto y los campos requeridos del documento.
            int Tipo = _Vista.VentasCboTipoDocumento.SelectedIndex;
            _Vista.VentasTxtNaturalezaDocumento.Text = Tipo < 0 ? string.Empty : (Tipo == 1 || Tipo == 4 ? "Abono - disminuye la deuda" : "Cargo - aumenta la deuda");
            if (_Vista.DocumentoActual == null && !_Vista.Cargando)
            {
                _Vista.VentasTxtFacturaReferencia.Clear();
                if (Tipo == 0)
                    VentasMetVencimientoCargo();
            }

            VentasMetActualizarAcciones();
        }

        internal void VentasMetEstadoNuevoDocumento()
        {
            _Vista.VentasCboEstadoDocumento.Items.Clear();
            _Vista.VentasCboEstadoDocumento.Items.Add("Pendiente de guardar");
            _Vista.VentasCboEstadoDocumento.SelectedIndex = 0;
            _Vista.VentasCboEstadoDocumento.Enabled = false;
        }

        internal void VentasMetVencimientoCargo()
        {
            if (_Vista.DocumentoActual != null || _Vista.VentasCboTipoDocumento.SelectedIndex != 0)
                return;
            DataRow ClienteSeleccionado = ClsHerramientasVentas.VentasFuncObtenerFila(_Vista.VentasCboCliente);
            _Vista.VentasDtpVencimientoDocumento.Value = _Vista.VentasDtpFechaDocumento.Value.Date.AddDays(ClienteSeleccionado == null ? 0 : Convert.ToInt32(ClienteSeleccionado["diasCredito"]));
        }

        internal void VentasMetSeleccionarFactura()
        {
            _Vista.Controlador.VentasMetValidarCaptura(_Vista.Cliente > 0 && _Vista.Moneda > 0, "Seleccione primero el cliente y la moneda.");
            DataTable Datos = _Vista.Controlador.VentasFuncListarDocumentos(_Vista.Cliente, _Vista.Moneda);
            DataTable Facturas = Datos.Clone();
            foreach (DataRow Fila in Datos.Rows)
                if (Convert.ToString(Fila["Clase"]) == "FACTURA" && Convert.ToString(Fila["Estado"]) == "EMITIDA")
                    Facturas.ImportRow(Fila);
            _Vista.Controlador.VentasMetValidarCaptura(Facturas.Rows.Count > 0, "El cliente no tiene documentos emitidos en esta moneda. Registre primero un Cargo inicial.");
            DataRow Seleccion = ClsHerramientasVentas.VentasFuncSeleccionar(_Vista, "Factura a la que se aplica la nota o ajuste", Facturas);
            if (Seleccion != null)
                _Vista.VentasTxtFacturaReferencia.Text = Convert.ToString(Seleccion["Documento"]);
        }

        internal void VentasMetModificar()
        {
            _Vista.ConsultasMovimientos.VentasMetAbrirSeleccionActual();
            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpDocumento)
            {
                _Vista.Controlador.VentasMetValidarCaptura(_Vista.DocumentoActual != null && Convert.ToString(_Vista.DocumentoActual["Estado"]) != "ANULADA", "Consulte un documento aplicado antes de modificarlo.");
                _Vista.EditandoDocumento = true;
                VentasMetRefrescarBotones();
                _Vista.VentasTxtDescripcionDocumento.Focus();
            }
            else if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpRecibos)
            {
                _Vista.Controlador.VentasMetValidarCaptura(_Vista.ReciboActual != null && Convert.ToString(_Vista.ReciboActual["estadoPago"]) == "CONFIRMADO", "Consulte un recibo confirmado antes de modificarlo.");
                _Vista.EditandoRecibo = true;
                VentasMetRefrescarBotones();
                _Vista.VentasTxtObservacionesPago.Focus();
            }
        }

        internal void VentasMetAnular()
        {
            _Vista.ConsultasMovimientos.VentasMetAbrirSeleccionActual();
            bool Recibo = _Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpRecibos;
            _Vista.Controlador.VentasMetValidarCaptura(Recibo ? _Vista.ReciboActual != null : _Vista.DocumentoActual != null, "Consulte el registro a anular.");
            string Numero = Recibo ? Convert.ToString(_Vista.ReciboActual["numeroRecibo"]) : Convert.ToString(_Vista.DocumentoActual["Numero"]);
            string Motivo = ClsHerramientasVentas.VentasFuncMotivo(_Vista, "Anular " + Numero);
            if (Motivo == null)
                return;
            if (Recibo)
            {
                int Id = Convert.ToInt32(_Vista.ReciboActual["idPago"]);
                _Vista.Controlador.VentasMetAnularRecibo(Id, DateTime.Today, _Vista.VentasIdUsuario, Motivo);
                _Vista.ConsultasMovimientos.VentasMetCargarRecibo(Id);
            }
            else
            {
                int Id = Convert.ToInt32(_Vista.DocumentoActual["Id"]);
                string Clase = Convert.ToString(_Vista.DocumentoActual["Clase"]);
                _Vista.Controlador.VentasMetAnularDocumento(Id, Clase, _Vista.VentasIdUsuario, Motivo);
                _Vista.ConsultasMovimientos.VentasMetCargarDocumento(Id, Clase);
            }

            ClsHerramientasVentas.VentasMetInformar(_Vista, "Registro anulado. Se conservaron los movimientos originales y su reverso.");
        }

        internal bool VentasFuncGuardarModificacion()
        {
            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpDocumento && _Vista.EditandoDocumento)
            {
                int Id = Convert.ToInt32(_Vista.DocumentoActual["Id"]);
                string Clase = Convert.ToString(_Vista.DocumentoActual["Clase"]);
                _Vista.Controlador.VentasMetModificarDocumento(Id, Clase, _Vista.VentasIdUsuario, _Vista.VentasTxtDescripcionDocumento.Text, _Vista.VentasDtpVencimientoDocumento.Value);
                _Vista.ConsultasMovimientos.VentasMetCargarDocumento(Id, Clase);
                ClsHerramientasVentas.VentasMetInformar(_Vista, "Documento actualizado.");
                return true;
            }

            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpRecibos && _Vista.EditandoRecibo)
            {
                int Id = Convert.ToInt32(_Vista.ReciboActual["idPago"]);
                _Vista.Controlador.VentasMetModificarRecibo(Id, _Vista.VentasIdUsuario, _Vista.VentasTxtObservacionesPago.Text);
                _Vista.ConsultasMovimientos.VentasMetCargarRecibo(Id);
                ClsHerramientasVentas.VentasMetInformar(_Vista, "Observaciones del recibo actualizadas.");
                return true;
            }

            return false;
        }

        internal void VentasMetGuardar()
        {
            if (VentasFuncGuardarModificacion())
                return;
            int Tab = _Vista.VentasTbcMovimientos.SelectedIndex;
            _Vista.Controlador.VentasMetValidarCaptura(!(_Vista.Guardados[Tab]), "El registro ya se guardo. Use Ingresar para comenzar otro.");
            string Numero;
            if (Tab == 1)
            {
                Numero = _Vista.Controlador.VentasFuncRegistrarDocumento(_Vista.Cliente, _Vista.Moneda, _Vista.VentasIdUsuario, ClsHerramientasVentas.VentasFuncObtenerId(_Vista.VentasCboEstablecimientoDocumento), _Vista.VentasTxtNumeroDocumento.Text, _Vista.VentasCboTipoDocumento.SelectedIndex, _Vista.VentasDtpFechaDocumento.Value, _Vista.VentasDtpVencimientoDocumento.Value, _Vista.VentasNudImporteDocumento.Value, _Vista.VentasTxtFacturaReferencia.Text, _Vista.VentasTxtDescripcionDocumento.Text);
                _Vista.VentasTxtNumeroDocumento.Text = Numero;
                bool Abono = _Vista.VentasCboTipoDocumento.SelectedIndex == 1 || _Vista.VentasCboTipoDocumento.SelectedIndex == 4;
                ClsHerramientasVentas.VentasMetInformar(_Vista, (Abono ? "Abono registrado; disminuye la deuda: " : "Cargo registrado; aumenta la deuda: ") + Numero);
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
                _Vista.Controlador.VentasMetValidarCaptura(_Vista.VentasCboFormaPago.SelectedIndex >= 0, "Seleccione la forma de pago.");
                Numero = _Vista.Controlador.VentasFuncRegistrarRecibo(_Vista.Cliente, _Vista.Moneda, _Vista.VentasIdUsuario, _Vista.VentasTxtRecibo.Text, _Vista.VentasDtpFechaPago.Value, _Vista.VentasNudMontoPago.Value, _Vista.VentasNudTipoCambioPago.Value, Formas[_Vista.VentasCboFormaPago.SelectedIndex], _Vista.VentasTxtReferenciaPago.Text, _Vista.VentasTxtObservacionesPago.Text, _Vista.VentasFuncSeleccionados(_Vista.VentasDgvDocumentos, "idFactura", 6));
                _Vista.VentasTxtRecibo.Text = Numero;
                VentasMetPendientes();
                ClsHerramientasVentas.VentasMetInformar(_Vista, "Recibo generado y aplicado correctamente: " + Numero);
            }
            else if (Tab == 3)
            {
                Numero = _Vista.Controlador.VentasFuncPrepararDeposito(ClsHerramientasVentas.VentasFuncObtenerId(_Vista.VentasCboMonedaDeposito), _Vista.VentasIdUsuario, _Vista.VentasDtpFechaDeposito.Value, _Vista.VentasTxtCodigoDeposito.Text, ClsConsultasMovimientos.VentasFuncReferencia(_Vista.VentasCboEmpresaDeposito), ClsConsultasMovimientos.VentasFuncReferencia(_Vista.VentasCboBancoDestino), ClsConsultasMovimientos.VentasFuncReferencia(_Vista.VentasCboCuentaDestino), ClsConsultasMovimientos.VentasFuncReferencia(_Vista.VentasCboConceptoBancario), _Vista.VentasTxtReferenciaDeposito.Text, _Vista.VentasFuncSeleccionados(_Vista.VentasDgvRecibosDeposito, "idPago", 7));
                _Vista.VentasTxtCodigoDeposito.Text = Numero;
                _Vista.ConsultasMovimientos.VentasMetDisponibles();
                ClsHerramientasVentas.VentasMetInformar(_Vista, "Deposito preparado localmente: " + Numero + ". El registro en Bancos requiere su enlace.");
            }
            else
            {
                _Vista.Controlador.VentasMetValidarCaptura(false, "Seleccione una pestana de captura.");
                return;
            }

            _Vista.Guardados[Tab] = true;
            if (Tab == 1)
            {
                string Documento = _Vista.VentasCboTipoDocumento.SelectedIndex == 0 ? "CXC-" + Numero : Numero;
                DataRow Fila = _Vista.Controlador.VentasFuncListarDocumentos(_Vista.Cliente, _Vista.Moneda).AsEnumerable().Single(Registro => Convert.ToString(Registro["Documento"]) == Documento);
                _Vista.ConsultasMovimientos.VentasMetCargarDocumento(Convert.ToInt32(Fila["Id"]), Convert.ToString(Fila["Clase"]));
            }
            else if (Tab == 2)
                _Vista.ConsultasMovimientos.VentasMetCargarRecibo(Convert.ToInt32(_Vista.Controlador.VentasFuncConsultarRecibos(Numero, "").Rows[0]["idPago"]));
            VentasMetRefrescarBotones();
        }

        internal void VentasMetPendientes()
        {
            _Vista.Cargando = true;
            try
            {
                _Vista.VentasDgvDocumentos.Rows.Clear();
                foreach (DataRow Fila in _Vista.Controlador.VentasFuncPendientes(_Vista.Cliente, _Vista.Moneda).Rows)
                {
                    int IndiceFila = _Vista.VentasDgvDocumentos.Rows.Add(false, Fila["Documento"], Fila["Fecha"], Fila["Vencimiento"], Fila["Moneda"], Fila["Saldo"], 0m);
                    _Vista.VentasDgvDocumentos.Rows[IndiceFila].Tag = Fila;
                }
            }
            finally
            {
                _Vista.Cargando = false;
            }

            VentasMetTotalesPago();
        }

        internal void VentasMetAplicacionCambio(object Origen, DataGridViewCellEventArgs Evento)
        {
            if (_Vista.Cargando || Evento.RowIndex < 0)
                return;
            ClsHerramientasVentas.VentasMetEjecutar(_Vista, () =>
            {
                if (Evento.ColumnIndex == 0 && Convert.ToBoolean(_Vista.VentasDgvDocumentos.Rows[Evento.RowIndex].Cells[0].Value))
                {
                    var FilaTabla = _Vista.VentasDgvDocumentos.Rows[Evento.RowIndex];
                    if (_Vista.Controlador.VentasFuncImporteCelda(FilaTabla.Cells[6].Value) == 0)
                        FilaTabla.Cells[6].Value = _Vista.VentasNudMontoPago.Value == 0 ? _Vista.Controlador.VentasFuncImporteCelda(FilaTabla.Cells[5].Value) : Math.Min(_Vista.Controlador.VentasFuncImporteCelda(FilaTabla.Cells[5].Value), Math.Max(0, _Vista.VentasNudMontoPago.Value - _Vista.VentasFuncSumar(_Vista.VentasDgvDocumentos, 6)));
                }

                VentasMetTotalesPago();
            });
        }

        internal void VentasMetTotalesPago()
        {
            if (_Vista.Cargando)
                return;
            decimal Aplicado = _Vista.VentasFuncSumar(_Vista.VentasDgvDocumentos, 6);
            _Vista.VentasLblValorRecibido.Text = ClsHerramientasVentas.VentasFuncDinero(_Vista.VentasNudMontoPago.Value, _Vista.CodigoMoneda);
            _Vista.VentasLblValorAplicado.Text = ClsHerramientasVentas.VentasFuncDinero(Aplicado, _Vista.CodigoMoneda);
            _Vista.VentasLblValorDisponible.Text = ClsHerramientasVentas.VentasFuncDinero(_Vista.VentasNudMontoPago.Value - Aplicado, _Vista.CodigoMoneda);
        }
    }
}
