// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using CapaControlador_MovimientosClientes;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_MovimientosClientes
{
    internal sealed class ClsConsultasMovimientos
    {
        private readonly FrmMovimientosClientesVentas _Vista;

        internal ClsConsultasMovimientos(FrmMovimientosClientesVentas Vista)
        {
            _Vista = Vista;
        }

        internal void VentasMetMostrarEstado()
        {
            _Vista.VentasDgvMovimientos.Rows.Clear();
            DataRow Resumen = _Vista.EstadoCuenta.Tables[0].Rows[0];
            decimal Inicial = Convert.ToDecimal(Resumen["Inicial"]), Cargos = Convert.ToDecimal(Resumen["Cargos"]), Abonos = Convert.ToDecimal(Resumen["Abonos"]);
            _Vista.VentasLblValorInicial.Text = ClsHerramientasVentas.VentasFuncDinero(Inicial, _Vista.CodigoMoneda);
            _Vista.VentasLblValorCargos.Text = ClsHerramientasVentas.VentasFuncDinero(Cargos, _Vista.CodigoMoneda);
            _Vista.VentasLblValorAbonos.Text = ClsHerramientasVentas.VentasFuncDinero(Abonos, _Vista.CodigoMoneda);
            _Vista.VentasLblValorFinal.Text = ClsHerramientasVentas.VentasFuncDinero(Inicial + Cargos - Abonos, _Vista.CodigoMoneda);
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
            string Tipo = Tipos[Math.Max(0, _Vista.VentasCboTipoConsulta.SelectedIndex)];
            foreach (DataRow Fila in _Vista.EstadoCuenta.Tables[1].Rows)
            {
                if (Tipo.Length > 0 && (string)Fila["Tipo"] != Tipo)
                    continue;
                if (_Vista.VentasCboEstadoConsulta.SelectedIndex > 0 && !string.Equals((string)Fila["Estado"], _Vista.VentasCboEstadoConsulta.Text, StringComparison.OrdinalIgnoreCase))
                    continue;
                int IndiceFila = _Vista.VentasDgvMovimientos.Rows.Add(Fila["Fecha"], Fila["Documento"], Fila["Tipo"], Fila["Descripcion"], Fila["Cargo"], Fila["Abono"], Fila["Saldo"], Fila["Estado"]);
                _Vista.VentasDgvMovimientos.Rows[IndiceFila].Tag = Fila;
            }

            _Vista.VentasLblRegistros.Text = _Vista.VentasDgvMovimientos.Rows.Count + " movimientos mostrados";
            _Vista.VentasLblEstadoCierre.Text = "Los totales representan todo el periodo; los filtros solo cambian las filas mostradas.";
        }

        internal void VentasMetConsultarCaptura()
        {
            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpDocumento)
            {
                DataTable Datos = _Vista.Controlador.VentasFuncListarDocumentos(_Vista.Cliente, _Vista.Moneda);
                string Numero = _Vista.VentasTxtNumeroDocumento.Text.Trim();
                DataRow Fila = Datos.AsEnumerable().FirstOrDefault(Registro => Convert.ToString(Registro["Documento"]) == Numero || Convert.ToString(Registro["Documento"]) == "CXC-" + Numero);
                if (Fila == null || Numero.Length == 0)
                    Fila = ClsHerramientasVentas.VentasFuncSeleccionar(_Vista, "Consultar documentos de CxC", Datos);
                if (Fila != null)
                    VentasMetCargarDocumento(Convert.ToInt32(Fila["Id"]), Convert.ToString(Fila["Clase"]));
            }
            else if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpRecibos)
            {
                DataTable Datos = _Vista.Controlador.VentasFuncListarRecibos(_Vista.Cliente, _Vista.Moneda);
                string Numero = _Vista.VentasTxtRecibo.Text.Trim();
                DataRow Fila2 = Datos.AsEnumerable().FirstOrDefault(Registro => Convert.ToString(Registro["Recibo"]) == Numero);
                if (Fila2 == null || Numero.Length == 0)
                    Fila2 = ClsHerramientasVentas.VentasFuncSeleccionar(_Vista, "Consultar recibos", Datos);
                if (Fila2 != null)
                    VentasMetCargarRecibo(Convert.ToInt32(Fila2["Id"]));
            }
            else
                _Vista.VentasMetConsultarActual();
        }

        internal void VentasMetAbrirSeleccionActual()
        {
            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpCuenta)
            {
                DataRow Fila = _Vista.VentasDgvMovimientos.CurrentRow == null ? null : _Vista.VentasDgvMovimientos.CurrentRow.Tag as DataRow;
                _Vista.Controlador.VentasMetValidarCaptura(Fila != null, "Seleccione un movimiento de la cuenta.");
                DataRow Registro = _Vista.Controlador.VentasFuncLeerMovimiento(Convert.ToInt32(Fila["idMovimiento"]));
                if (Convert.ToString(Registro["Clase"]) == "RECIBO")
                    VentasMetCargarRecibo(Convert.ToInt32(Registro["Id"]));
                else
                    VentasMetCargarDocumento(Convert.ToInt32(Registro["Id"]), Convert.ToString(Registro["Clase"]));
            }
            else if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpConsultaRecibos)
            {
                DataRow Fila2 = _Vista.VentasDgvConsultaRecibos.CurrentRow == null ? null : _Vista.VentasDgvConsultaRecibos.CurrentRow.Tag as DataRow;
                _Vista.Controlador.VentasMetValidarCaptura(Fila2 != null, "Seleccione un recibo.");
                VentasMetCargarRecibo(Convert.ToInt32(Fila2["idPago"]));
            }
            else if ((_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpDocumento && _Vista.DocumentoActual == null) || (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpRecibos && _Vista.ReciboActual == null))
                VentasMetConsultarCaptura();
        }

        internal void VentasMetImprimir()
        {
            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpCuenta)
            {
                ClsHerramientasVentas.VentasMetImprimir(_Vista, "Estado de cuenta", _Vista.VentasCboCliente.Text + " / " + _Vista.CodigoMoneda, _Vista.EstadoCuenta == null ? null : _Vista.EstadoCuenta.Tables[1]);
                return;
            }

            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpDocumento)
            {
                ClsHerramientasVentas.VentasMetImprimir(_Vista, "Documento interno de CxC", _Vista.CodigoMoneda, _Vista.Controlador.VentasFuncDocumento(_Vista.Cliente, _Vista.Moneda, ClsHerramientasVentas.VentasFuncObtenerId(_Vista.VentasCboEstablecimientoDocumento), _Vista.VentasTxtNumeroDocumento.Text.Trim(), _Vista.VentasCboTipoDocumento.SelectedIndex == 0));
                return;
            }

            if (_Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpDepositos)
            {
                DataSet ConjuntoDatos = _Vista.Controlador.VentasFuncDeposito(_Vista.VentasTxtCodigoDeposito.Text.Trim());
                DataRow Encabezado = ConjuntoDatos.Tables[0].Rows[0];
                ClsHerramientasVentas.VentasMetImprimir(_Vista, "Preparacion local " + Encabezado["codigoDeposito"], "Estado: " + Encabezado["estadoDeposito"] + " / Referencia de cuenta: " + Encabezado["cuentaReferencia"] + " / Total: " + Encabezado["total"], ConjuntoDatos.Tables[1]);
                return;
            }

            bool Recepcion = _Vista.VentasTbcMovimientos.SelectedTab == _Vista.VentasTbpRecibos;
            DataTable Recibos = _Vista.Controlador.VentasFuncConsultarRecibos(Recepcion ? _Vista.VentasTxtRecibo.Text.Trim() : _Vista.VentasTxtConsultaRecibo.Text.Trim(), Recepcion ? "" : _Vista.VentasTxtConsultaDeposito.Text.Trim());
            _Vista.Controlador.VentasMetValidarCaptura(Recibos.Rows.Count == 1, "Consulte un solo recibo para imprimir su comprobante.");
            DataRow Registro = Recibos.Rows[0];
            ClsHerramientasVentas.VentasMetImprimir(_Vista, "Recibo " + Registro["Recibo"], Registro["Cliente"] + " / " + Registro["Moneda"] + " / Total: " + Convert.ToDecimal(Registro["Monto"]).ToString("N2", ClsHerramientasVentas.Cultura) + " / " + Registro["Estado"], _Vista.Controlador.VentasFuncAplicaciones(Convert.ToInt32(Registro["idPago"])));
        }

        internal void VentasMetCargarDocumento(int Id, string Clase)
        {
            DataRow Fila = _Vista.Controlador.VentasFuncLeerDocumento(Id, Clase);
            _Vista.Cargando = true;
            try
            {
                _Vista.VentasTbcMovimientos.SelectedTab = _Vista.VentasTbpDocumento;
                _Vista.VentasCboCliente.SelectedValue = Fila["idCliente"];
                _Vista.VentasCboMoneda.SelectedValue = Fila["idMoneda"];
                _Vista.VentasCboEstablecimientoDocumento.SelectedValue = Fila["idEstablecimiento"];
                _Vista.VentasCboTipoDocumento.SelectedIndex = Convert.ToInt32(Fila["Tipo"]);
                _Vista.VentasTxtNumeroDocumento.Text = Convert.ToString(Fila["Numero"]);
                _Vista.VentasDtpFechaDocumento.Value = Convert.ToDateTime(Fila["Fecha"]);
                _Vista.VentasDtpVencimientoDocumento.Value = Convert.ToDateTime(Fila["Vencimiento"]);
                _Vista.VentasNudImporteDocumento.Value = Convert.ToDecimal(Fila["Importe"]);
                _Vista.VentasTxtFacturaReferencia.Text = Convert.ToString(Fila["Referencia"]);
                _Vista.VentasTxtDescripcionDocumento.Text = Convert.ToString(Fila["Descripcion"]);
                _Vista.VentasCboEstadoDocumento.Items.Clear();
                _Vista.VentasCboEstadoDocumento.Items.Add(Convert.ToString(Fila["Estado"]));
                _Vista.VentasCboEstadoDocumento.SelectedIndex = 0;
                DataRow ClienteActual = ClsHerramientasVentas.VentasFuncObtenerFila(_Vista.VentasCboCliente);
                _Vista.VentasTxtCondicion.Text = ClienteActual == null ? "" : ClienteActual["diasCredito"] + " dias; limite " + Convert.ToDecimal(ClienteActual["limiteCredito"]).ToString("N2", ClsHerramientasVentas.Cultura);
                _Vista.DocumentoActual = Fila;
                _Vista.EditandoDocumento = false;
                _Vista.Guardados[1] = true;
            }
            finally
            {
                _Vista.Cargando = false;
            }

            _Vista.CapturaMovimientos.VentasMetTipoDocumentoCambio();
            _Vista.CapturaMovimientos.VentasMetRefrescarBotones();
            _Vista.VentasLblEtapaBancos.Text = "Documento consultado. Modificar permite cambiar la descripcion y el vencimiento de una factura.";
        }

        internal void VentasMetCargarRecibo(int Id)
        {
            DataSet Datos = _Vista.Controlador.VentasFuncLeerRecibo(Id);
            DataRow Fila = Datos.Tables[0].Rows[0];
            _Vista.Cargando = true;
            try
            {
                _Vista.VentasTbcMovimientos.SelectedTab = _Vista.VentasTbpRecibos;
                _Vista.VentasCboCliente.SelectedValue = Fila["idCliente"];
                _Vista.VentasCboMoneda.SelectedValue = Fila["idMoneda"];
                _Vista.VentasTxtRecibo.Text = Convert.ToString(Fila["numeroRecibo"]);
                _Vista.VentasDtpFechaPago.Value = Convert.ToDateTime(Fila["fechaPago"]);
                _Vista.VentasNudMontoPago.Value = Convert.ToDecimal(Fila["montoPago"]);
                _Vista.VentasNudTipoCambioPago.Value = Convert.ToDecimal(Fila["tipoCambio"]);
                string[] Formas =
                {
                    "EFECTIVO",
                    "TRANSFERENCIA",
                    "DEPOSITO",
                    "CHEQUE",
                    "TARJETA"
                };
                _Vista.VentasCboFormaPago.SelectedIndex = Array.IndexOf(Formas, Convert.ToString(Fila["formaPago"]));
                _Vista.VentasTxtReferenciaPago.Text = Convert.ToString(Fila["referenciaPago"]);
                _Vista.VentasTxtObservacionesPago.Text = Convert.ToString(Fila["observaciones"]);
                _Vista.VentasDgvDocumentos.Rows.Clear();
                foreach (DataRow Aplicacion in Datos.Tables[1].Rows)
                {
                    int IndiceFila = _Vista.VentasDgvDocumentos.Rows.Add(true, Aplicacion["Documento"], Aplicacion["Fecha"], Aplicacion["Vencimiento"], Aplicacion["Moneda"], Aplicacion["Saldo"], Aplicacion["Importe"]);
                    _Vista.VentasDgvDocumentos.Rows[IndiceFila].Tag = Aplicacion;
                }

                DataRow ClienteActual = ClsHerramientasVentas.VentasFuncObtenerFila(_Vista.VentasCboCliente);
                _Vista.VentasTxtCondicion.Text = ClienteActual == null ? "" : ClienteActual["diasCredito"] + " dias; limite " + Convert.ToDecimal(ClienteActual["limiteCredito"]).ToString("N2", ClsHerramientasVentas.Cultura);
                _Vista.ReciboActual = Fila;
                _Vista.EditandoRecibo = false;
                _Vista.Guardados[2] = true;
            }
            finally
            {
                _Vista.Cargando = false;
            }

            _Vista.CapturaMovimientos.VentasMetTotalesPago();
            _Vista.CapturaMovimientos.VentasMetRefrescarBotones();
            _Vista.VentasLblEtapaBancos.Text = "Recibo " + Fila["numeroRecibo"] + " / " + Fila["estadoPago"] + ". Modificar permite actualizar sus observaciones.";
        }

        internal void VentasMetConfigurarCatalogosBancos(DataTable Empresas, DataTable Bancos, DataTable Cuentas, DataTable Conceptos, string Modo)
        {
            // Tablas con Id y Nombre suministradas por el modulo dueno de esos catalogos.
            foreach (var Combo in new[]
            {
                _Vista.VentasCboEmpresaDeposito,
                _Vista.VentasCboBancoDestino,
                _Vista.VentasCboCuentaDestino,
                _Vista.VentasCboConceptoBancario
            })
                Combo.DropDownStyle = ComboBoxStyle.DropDownList;
            ClsHerramientasVentas.VentasMetCargarOpciones(_Vista.VentasCboEmpresaDeposito, Empresas);
            ClsHerramientasVentas.VentasMetCargarOpciones(_Vista.VentasCboBancoDestino, Bancos);
            ClsHerramientasVentas.VentasMetCargarOpciones(_Vista.VentasCboCuentaDestino, Cuentas);
            ClsHerramientasVentas.VentasMetCargarOpciones(_Vista.VentasCboConceptoBancario, Conceptos);
            _Vista.VentasTxtModoActualizacion.Text = Modo;
        }

        internal static string VentasFuncReferencia(ComboBox Combo)
        {
            return Combo.SelectedValue == null ? Combo.Text.Trim() : Convert.ToString(Combo.SelectedValue);
        }

        internal void VentasMetDisponibles()
        {
            _Vista.Cargando = true;
            try
            {
                _Vista.VentasDgvRecibosDeposito.Rows.Clear();
                foreach (DataRow Fila in _Vista.Controlador.VentasFuncRecibosDisponibles(ClsHerramientasVentas.VentasFuncObtenerId(_Vista.VentasCboMonedaDeposito)).Rows)
                {
                    int IndiceFila = _Vista.VentasDgvRecibosDeposito.Rows.Add(false, Fila["Recibo"], Fila["Cliente"], Fila["Moneda"], Fila["Monto"], Fila["Depositado"], Fila["Disponible"], 0m);
                    _Vista.VentasDgvRecibosDeposito.Rows[IndiceFila].Tag = Fila;
                }
            }
            finally
            {
                _Vista.Cargando = false;
            }

            _Vista.VentasTxtTotalDeposito.Text = "0.00";
        }

        internal void VentasMetDepositoCambio(object Origen, DataGridViewCellEventArgs Evento)
        {
            if (_Vista.Cargando || Evento.RowIndex < 0)
                return;
            ClsHerramientasVentas.VentasMetEjecutar(_Vista, () =>
            {
                var FilaTabla = _Vista.VentasDgvRecibosDeposito.Rows[Evento.RowIndex];
                if (Evento.ColumnIndex == 0 && Convert.ToBoolean(FilaTabla.Cells[0].Value) && _Vista.Controlador.VentasFuncImporteCelda(FilaTabla.Cells[7].Value) == 0)
                    FilaTabla.Cells[7].Value = FilaTabla.Cells[6].Value;
                _Vista.VentasTxtTotalDeposito.Text = _Vista.VentasFuncSumar(_Vista.VentasDgvRecibosDeposito, 7).ToString("N2", ClsHerramientasVentas.Cultura);
            });
        }

        internal void VentasMetConsultarDeposito()
        {
            DataSet Datos = _Vista.Controlador.VentasFuncDeposito(_Vista.VentasTxtCodigoDeposito.Text.Trim());
            DataRow Fila = Datos.Tables[0].Rows[0];
            _Vista.Cargando = true;
            try
            {
                _Vista.VentasCboMonedaDeposito.SelectedValue = Fila["idMoneda"];
                _Vista.VentasDtpFechaDeposito.Value = Convert.ToDateTime(Fila["fechaDeposito"]);
                _Vista.VentasCboEmpresaDeposito.Text = Convert.ToString(Fila["empresaReferencia"]);
                _Vista.VentasCboBancoDestino.Text = Convert.ToString(Fila["bancoReferencia"]);
                _Vista.VentasCboCuentaDestino.Text = Convert.ToString(Fila["cuentaReferencia"]);
                _Vista.VentasCboConceptoBancario.Text = Convert.ToString(Fila["conceptoReferencia"]);
                _Vista.VentasTxtReferenciaDeposito.Text = Convert.ToString(Fila["referenciaDeposito"]);
                _Vista.VentasDgvRecibosDeposito.Rows.Clear();
                foreach (DataRow DetalleDeposito in Datos.Tables[1].Rows)
                    _Vista.VentasDgvRecibosDeposito.Rows.Add(true, DetalleDeposito["Recibo"], DetalleDeposito["Cliente"], _Vista.VentasCboMonedaDeposito.Text, DetalleDeposito["Importe"], DetalleDeposito["Importe"], 0m, DetalleDeposito["Importe"]);
                _Vista.VentasTxtTotalDeposito.Text = Convert.ToDecimal(Fila["total"]).ToString("N2", ClsHerramientasVentas.Cultura);
                _Vista.VentasTxtEstadoEnvio.Text = Convert.ToString(Fila["estadoDeposito"]);
                _Vista.Guardados[3] = true;
            }
            finally
            {
                _Vista.Cargando = false;
            }

            _Vista.CapturaMovimientos.VentasMetRefrescarBotones();
            _Vista.VentasLblEtapaBancos.Text = "Deposito preparado localmente. Su confirmacion bancaria corresponde al modulo de Bancos.";
        }

        internal void VentasMetConsultarRecibos()
        {
            _Vista.Cargando = true;
            try
            {
                _Vista.VentasDgvConsultaRecibos.Rows.Clear();
                _Vista.VentasDgvConsultaAplicaciones.Rows.Clear();
                _Vista.VentasTxtMovimientoBancario.Clear();
                foreach (DataRow Fila in _Vista.Controlador.VentasFuncConsultarRecibos(_Vista.VentasTxtConsultaRecibo.Text.Trim(), _Vista.VentasTxtConsultaDeposito.Text.Trim()).Rows)
                {
                    int IndiceFila = _Vista.VentasDgvConsultaRecibos.Rows.Add(Fila["Recibo"], Fila["Fecha"], Fila["Cliente"], Fila["Moneda"], Fila["Monto"], Fila["Estado"], Fila["Deposito"], Fila["Depositado"]);
                    _Vista.VentasDgvConsultaRecibos.Rows[IndiceFila].Tag = Fila;
                }
            }
            finally
            {
                _Vista.Cargando = false;
            }

            VentasMetAplicacionesConsulta();
        }

        internal void VentasMetAplicacionesConsulta()
        {
            DataRow Fila = _Vista.VentasDgvConsultaRecibos.CurrentRow == null ? null : _Vista.VentasDgvConsultaRecibos.CurrentRow.Tag as DataRow;
            _Vista.VentasDgvConsultaAplicaciones.Rows.Clear();
            if (Fila == null)
                return;
            _Vista.VentasTxtMovimientoBancario.Text = Convert.ToString(Fila["MovimientoBancario"]);
            foreach (DataRow Aplicacion in _Vista.Controlador.VentasFuncAplicaciones(Convert.ToInt32(Fila["idPago"])).Rows)
                _Vista.VentasDgvConsultaAplicaciones.Rows.Add(Aplicacion["Factura"], Aplicacion["Fecha"], Aplicacion["Importe"], Aplicacion["Estado"]);
        }
    }
}
