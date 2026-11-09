using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_PagosProveedores;

using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta los pagos a proveedores, administra los filtros y la selección
 *  de documentos por pagar, y coordina las acciones del usuario con el
 *  controlador. Un pago puede aplicarse a varios documentos (detalle).
 * ===================================================================
*/

namespace CapaVista_PagosProveedores
{
    public partial class FrmPagosProveedores : Form
    {
        private const string _TituloMensajes = "Pagos a proveedores";
        private const string _FormatoFecha = "dd/MM/yyyy";
        private const string _MonedaBase = "GTQ";

        private readonly ClsModeloPagoProveedor _Modelo;
        private DataTable _TablaCuentas;
        private DataTable _TablaMonedas;
        private List<ClsModeloDetallePagoProveedor> _Detalles;
        private int _IdPagoEditando;
        private string _EstadoEditando;
        private bool _Cargando;
        private int? _ProveedorAnterior;
        private int? _MonedaAnterior;

        public FrmPagosProveedores()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            _Modelo = new ClsModeloPagoProveedor();
            _Detalles = new List<ClsModeloDetallePagoProveedor>();
            _IdPagoEditando = 0;
            _EstadoEditando = "Programado";
            _Cargando = false;

            Load += ComprasMetFormularioLoad;
            ComprasBtnNuevoPago.Click += ComprasMetNuevoPagoClick;
            ComprasBtnGuardar.Click += ComprasMetGuardarClick;
            ComprasBtnCancelar.Click += ComprasMetCancelarClick;
            ComprasBtnEditar.Click += ComprasMetEditarClick;
            ComprasBtnEliminar.Click += ComprasMetEliminarClick;
            ComprasBtnAgregarDocumento.Click += ComprasMetAgregarDocumentoClick;
            ComprasBtnQuitarDocumento.Click += ComprasMetQuitarDocumentoClick;
            ComprasMetSuscribirEventos();
        }

        private void ComprasMetFormularioLoad(object Sender, EventArgs Evento)
        {
            try
            {
                _Cargando = true;

                DataTable Proveedores = _Modelo.ComprasMetObtenerProveedores();
                DataTable Metodos = _Modelo.ComprasMetObtenerMetodosPago();
                _TablaMonedas = _Modelo.ComprasMetObtenerMonedas();

                ComprasMetLlenarCombo(ComprasCboProveedorPago, Proveedores, "nombre", "idProveedor");
                ComprasMetLlenarCombo(ComprasCboEstado, new List<string> { "Todos", "Pagado", "Programado", "Rechazado", "Anulado" }, null, null);
                ComprasMetLlenarCombo(ComprasCboMetodo, Metodos, "nombre", "idMetodoPago");
                ComprasMetLlenarCombo(ComprasCboMoneda, _TablaMonedas, "codigo", "idMoneda");

                ComprasMetRestablecerRangoFechas();

                ComprasMetFijarIndiceCombo(ComprasCboEstado, 0);

                _Cargando = false;

                ComprasMetLimpiarFormulario();
                ComprasMetCargarGrid();
            }
            catch (Exception Ex)
            {
                _Cargando = false;
                ComprasMetMostrarAviso("No se pudieron cargar los datos iniciales. " + Ex.Message);
            }
        }

        // LISTA DE PAGOS 
        private void ComprasMetCargarGrid()
        {
            try
            {
                DateTime? Desde = comprasUsrRangoFechas1.FechaInicio;
                DateTime? Hasta = comprasUsrRangoFechas1.FechaFin;

                if (Desde.HasValue && Hasta.HasValue && Desde.Value.Date > Hasta.Value.Date)
                {
                    ComprasMetMostrarAviso("La fecha Desde no puede ser mayor que la fecha Hasta.");
                    return;
                }

                int? IdProveedor = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboProveedor));
                string EstadoFiltro = ComprasFuncTextoCombo(ComprasCboEstado);

                List<ClsModeloPagoProveedor> Lista = _Modelo.ComprasMetObtenerTodos(IdProveedor, EstadoFiltro, Desde, Hasta);

                DataTable Tabla = new DataTable();
                Tabla.Columns.Add("idPago", typeof(int));
                Tabla.Columns.Add("noPago", typeof(string));
                Tabla.Columns.Add("proveedor", typeof(string));
                Tabla.Columns.Add("metodo", typeof(string));
                Tabla.Columns.Add("fecha", typeof(string));
                Tabla.Columns.Add("monto", typeof(string));
                Tabla.Columns.Add("estado", typeof(string));

                foreach (ClsModeloPagoProveedor Pago in Lista)
                {
                    Tabla.Rows.Add(
                        Pago.IdPagoProveedor,
                        Pago.NoPagoProveedor,
                        Pago.NombreProveedor,
                        Pago.NombreMetodoPago,
                        Pago.FechaPagoProveedor.ToString(_FormatoFecha),
                        ComprasFuncFormatoMonto(Pago.IdMoneda, Pago.MontoPagoProveedor),
                        Pago.EstadoPagoProveedor);
                }

                ComprasDgvPagos.AutoGenerateColumns = true;
                ComprasDgvPagos.DataSource = Tabla;
                ComprasMetConfigurarColumnas();
            }
            catch (Exception Ex)
            {
                ComprasMetMostrarAviso("No se pudo cargar la lista de pagos. " + Ex.Message);
            }
        }

        private void ComprasMetConfigurarColumnas()
        {
            if (ComprasDgvPagos.Columns.Contains("idPago"))
                ComprasDgvPagos.Columns["idPago"].Visible = false;
            if (ComprasDgvPagos.Columns.Contains("noPago"))
                ComprasDgvPagos.Columns["noPago"].HeaderText = "No. pago";
            if (ComprasDgvPagos.Columns.Contains("proveedor"))
                ComprasDgvPagos.Columns["proveedor"].HeaderText = "Proveedor";
            if (ComprasDgvPagos.Columns.Contains("metodo"))
                ComprasDgvPagos.Columns["metodo"].HeaderText = "Método";
            if (ComprasDgvPagos.Columns.Contains("fecha"))
                ComprasDgvPagos.Columns["fecha"].HeaderText = "Fecha";
            if (ComprasDgvPagos.Columns.Contains("monto"))
                ComprasDgvPagos.Columns["monto"].HeaderText = "Monto";
            if (ComprasDgvPagos.Columns.Contains("estado"))
                ComprasDgvPagos.Columns["estado"].HeaderText = "Estado";
        }

        private bool ComprasFuncObtenerSeleccion(out int IdPago, out string NoPago)
        {
            IdPago = 0;
            NoPago = "";

            DataGridViewRow Fila = ComprasDgvPagos.CurrentRow;
            if (Fila == null || Fila.IsNewRow || !ComprasDgvPagos.Columns.Contains("idPago"))
            {
                ComprasMetMostrarAviso("Seleccione un pago de la lista.");
                return false;
            }

            IdPago = Convert.ToInt32(Fila.Cells["idPago"].Value);
            NoPago = Convert.ToString(Fila.Cells["noPago"].Value);
            return true;
        }

        private void ComprasMetEditarClick(object Sender, EventArgs Evento)
        {
            int IdPago;
            string NoPago;
            if (!ComprasFuncObtenerSeleccion(out IdPago, out NoPago))
                return;

            try
            {
                ComprasMetCargarParaEditar(IdPago);
            }
            catch (Exception Ex)
            {
                ComprasMetMostrarAviso("No se pudo cargar el pago. " + Ex.Message);
            }
        }

        private void ComprasMetEliminarClick(object Sender, EventArgs Evento)
        {
            int IdPago;
            string NoPago;
            if (!ComprasFuncObtenerSeleccion(out IdPago, out NoPago))
                return;

            ComprasMetEliminarPago(IdPago, NoPago);
        }

        //  FORMULARIO DE REGISTRO 
        private void ComprasMetLimpiarFormulario()
        {
            _IdPagoEditando = 0;
            _EstadoEditando = "Programado";
            _Detalles = new List<ClsModeloDetallePagoProveedor>();

            ComprasCboProveedorPago.Enabled = true;
            ComprasCboFactura.Enabled = true;

            _Cargando = true;
            ComprasMetFijarIndiceCombo(ComprasCboProveedorPago, 0);
            ComprasMetFijarIndiceCombo(ComprasCboMetodo, 0);
            ComprasMetSeleccionarMonedaBase();
            _ProveedorAnterior = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboProveedorPago));
            _MonedaAnterior = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboMoneda));
            _Cargando = false;

            ComprasMetFijarTexto(ComprasTxtFechaPago, DateTime.Today.ToString(_FormatoFecha));
            ComprasMetFijarTexto(ComprasTxtMonto, "");
            ComprasMetFijarTexto(ComprasTxtReferencia, "");

            ComprasMetCargarFacturas();
            ComprasMetRefrescarDetalle();
        }

        private void ComprasMetSeleccionarMonedaBase()
        {
            if (_TablaMonedas != null)
            {
                foreach (DataRow Fila in _TablaMonedas.Rows)
                {
                    if (Convert.ToString(Fila["codigo"]) == _MonedaBase)
                    {
                        ComprasMetFijarValorCombo(ComprasCboMoneda, Fila["idMoneda"]);
                        return;
                    }
                }
            }
            ComprasMetFijarIndiceCombo(ComprasCboMoneda, 0);
        }

        private void ComprasMetPrepararTablaCuentas(DataTable Tabla)
        {
            if (Tabla != null && !Tabla.Columns.Contains("clave"))
                Tabla.Columns.Add("clave", typeof(string), "serie + '|' + numero + '|' + Convert(idConcepto, 'System.String')");
        }

        private DataRow ComprasFuncFilaCuenta(string Clave)
        {
            if (_TablaCuentas == null || string.IsNullOrEmpty(Clave))
                return null;

            foreach (DataRow Fila in _TablaCuentas.Rows)
            {
                if (Convert.ToString(Fila["clave"]) == Clave)
                    return Fila;
            }
            return null;
        }

        private void ComprasMetCargarFacturas()
        {
            try
            {
                int? IdProveedor = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboProveedorPago));
                _ProveedorAnterior = IdProveedor;

                if (!IdProveedor.HasValue)
                {
                    _TablaCuentas = null;
                    ComprasMetLlenarCombo(ComprasCboFactura, null, null, null);
                    ComprasMetFijarTexto(ComprasTxtMonto, "");
                    return;
                }

                _Cargando = true;
                _TablaCuentas = _Modelo.ComprasMetObtenerCuentasPendientes(IdProveedor.Value);
                ComprasMetPrepararTablaCuentas(_TablaCuentas);
                ComprasMetLlenarCombo(ComprasCboFactura, _TablaCuentas, "descripcion", "clave");
                if (_TablaCuentas.Rows.Count > 0)
                    ComprasMetFijarIndiceCombo(ComprasCboFactura, 0);
                _Cargando = false;

                ComprasMetAutocompletarMonto();
            }
            catch (Exception Ex)
            {
                _Cargando = false;
                ComprasMetMostrarAviso("No se pudieron cargar los documentos por pagar. " + Ex.Message);
            }
        }

        // Sugiere el saldo del documento cuando está en la misma moneda del pago
        private void ComprasMetAutocompletarMonto()
        {
            string Clave = Convert.ToString(ComprasFuncValorCombo(ComprasCboFactura));
            DataRow Fila = ComprasFuncFilaCuenta(Clave);
            int? MonedaPago = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboMoneda));

            if (Fila == null || !MonedaPago.HasValue || Convert.ToInt64(Fila["idMoneda"]) != MonedaPago.Value)
            {
                ComprasMetFijarTexto(ComprasTxtMonto, "");
                return;
            }

            ComprasMetFijarTexto(ComprasTxtMonto, Convert.ToDecimal(Fila["saldo"]).ToString("0.00##", CultureInfo.CurrentCulture));
        }

        private void ComprasMetCargarParaEditar(int IdPago)
        {
            ClsModeloPagoProveedor Pago = _Modelo.ComprasMetObtenerPorId(IdPago);
            if (Pago == null)
            {
                ComprasMetMostrarAviso("El pago seleccionado ya no existe.");
                ComprasMetCargarGrid();
                return;
            }

            _Cargando = true;
            try
            {
                _IdPagoEditando = Pago.IdPagoProveedor;
                _EstadoEditando = Pago.EstadoPagoProveedor;

                ComprasMetFijarValorCombo(ComprasCboProveedorPago, Pago.IdProveedor);
                ComprasMetFijarValorCombo(ComprasCboMoneda, (int)Pago.IdMoneda);
                ComprasMetFijarValorCombo(ComprasCboMetodo, Pago.IdMetodoPago);

                _TablaCuentas = _Modelo.ComprasMetObtenerCuentasPendientes(Pago.IdProveedor);
                ComprasMetPrepararTablaCuentas(_TablaCuentas);

                // Los documentos ya pagados por este pago pueden no estar pendientes: se agregan para poder reemplazarlos
                foreach (ClsModeloDetallePagoProveedor Detalle in Pago.Detalles)
                {
                    if (ComprasFuncFilaCuenta(Detalle.DocumentoClave) != null)
                        continue;

                    DataRow FilaNueva = _TablaCuentas.NewRow();
                    FilaNueva["serie"] = Detalle.SerieDocumento;
                    FilaNueva["numero"] = Detalle.NumeroDocumento;
                    FilaNueva["idConcepto"] = Detalle.IdConcepto;
                    FilaNueva["concepto"] = Detalle.NombreConcepto;
                    FilaNueva["idMoneda"] = Pago.IdMoneda;
                    FilaNueva["codigoMoneda"] = ComprasFuncCodigoMoneda(Pago.IdMoneda);
                    FilaNueva["saldo"] = Detalle.MontoAplicado;
                    FilaNueva["descripcion"] = Detalle.SerieDocumento + "-" + Detalle.NumeroDocumento + " (" + Detalle.NombreConcepto + ")";
                    _TablaCuentas.Rows.Add(FilaNueva);
                }
                ComprasMetLlenarCombo(ComprasCboFactura, _TablaCuentas, "descripcion", "clave");

                _Detalles = new List<ClsModeloDetallePagoProveedor>();
                foreach (ClsModeloDetallePagoProveedor Detalle in Pago.Detalles)
                {
                    _Detalles.Add(new ClsModeloDetallePagoProveedor
                    {
                        SerieDocumento = Detalle.SerieDocumento,
                        NumeroDocumento = Detalle.NumeroDocumento,
                        IdConcepto = Detalle.IdConcepto,
                        NombreConcepto = Detalle.NombreConcepto,
                        MontoAplicado = Detalle.MontoAplicado
                    });
                }

                _ProveedorAnterior = Pago.IdProveedor;
                _MonedaAnterior = (int)Pago.IdMoneda;

                ComprasMetFijarTexto(ComprasTxtFechaPago, Pago.FechaPagoProveedor.ToString(_FormatoFecha));
                ComprasMetFijarTexto(ComprasTxtMonto, "");
                ComprasMetFijarTexto(ComprasTxtReferencia, Pago.ReferenciaPagoProveedor ?? "");

                ComprasCboProveedorPago.Enabled = false;
            }
            finally
            {
                _Cargando = false;
            }

            ComprasMetRefrescarDetalle();
        }

        private void ComprasMetNuevoPagoClick(object Sender, EventArgs Evento)
        {
            if (ComprasFuncHayOperacionEnCurso())
            {
                if (!ComprasFuncConfirmar("Hay datos sin guardar. ¿Desea descartarlos y registrar un nuevo pago?"))
                    return;
            }

            ComprasMetLimpiarFormulario();
            ComprasDgvPagos.ClearSelection();

            // Baja hasta el formulario "Registrar pago" y deja el cursor en Proveedor
            ComprasMetDesplazarHacia(ComprasGbxRegistrarPago);
            ComprasCboProveedorPago.Focus();
        }

        private void ComprasMetCancelarClick(object Sender, EventArgs Evento)
        {
            if (ComprasFuncHayOperacionEnCurso())
            {
                string Pregunta = _IdPagoEditando != 0
                    ? "¿Desea cancelar la edición del pago? Se perderán los cambios no guardados."
                    : "¿Desea cancelar el registro? Se perderán los datos ingresados.";

                if (!ComprasFuncConfirmar(Pregunta))
                    return;
            }

            // Restablece los filtros a su estado inicial
            _Cargando = true;
            ComprasMetRestablecerRangoFechas();
            ComprasMetFijarIndiceCombo(ComprasCboEstado, 0);
            ComprasMetFijarIndiceCombo(ComprasCboProveedor, 0);
            _Cargando = false;

            // Restablece el formulario y la lista
            ComprasMetLimpiarFormulario();
            ComprasMetCargarGrid();
            ComprasDgvPagos.ClearSelection();

            // Vuelve al inicio de la pantalla
            ComprasPnlDesplazamiento.AutoScrollPosition = new System.Drawing.Point(0, 0);

            ComprasMetMostrarInformacion("Operación cancelada. ");
        }

        // Mueve la barra de desplazamiento hasta que el control quede visible arriba
        private void ComprasMetDesplazarHacia(Control Destino)
        {
            System.Drawing.Point PosicionPantalla = Destino.PointToScreen(System.Drawing.Point.Empty);
            System.Drawing.Point PosicionPanel = ComprasPnlDesplazamiento.PointToClient(PosicionPantalla);

            // AutoScrollPosition devuelve valores negativos cuando ya se bajó
            int PosicionAbsoluta = PosicionPanel.Y - ComprasPnlDesplazamiento.AutoScrollPosition.Y;

            ComprasPnlDesplazamiento.AutoScrollPosition = new System.Drawing.Point(0, Math.Max(0, PosicionAbsoluta - 10));
        }

        // Devuelve true si se está editando un pago o el usuario ya escribió algo
        private bool ComprasFuncHayOperacionEnCurso()
        {
            if (_IdPagoEditando != 0)
                return true;

            if (_Detalles.Count > 0)
                return true;

            if (ComprasFuncTexto(ComprasTxtReferencia).Trim().Length > 0)
                return true;

            if (ComprasFuncTexto(ComprasTxtFechaPago).Trim() != DateTime.Today.ToString(_FormatoFecha))
                return true;

            return false;
        }

        //  DETALLE (DOCUMENTOS A PAGAR) 
        private void ComprasMetAgregarDocumentoClick(object Sender, EventArgs Evento)
        {
            string Clave = Convert.ToString(ComprasFuncValorCombo(ComprasCboFactura));
            DataRow Fila = ComprasFuncFilaCuenta(Clave);
            if (Fila == null)
            {
                ComprasMetMostrarAviso("Seleccione un documento por pagar.");
                return;
            }

            decimal Monto;
            if (!ComprasFuncMonto(ComprasFuncTexto(ComprasTxtMonto), out Monto) || Monto <= 0)
            {
                ComprasMetMostrarAviso("El monto a aplicar no es válido.");
                return;
            }
            Monto = Math.Round(Monto, 4);

            if (_Detalles.Any(Detalle => Detalle.DocumentoClave == Clave))
            {
                ComprasMetMostrarAviso("El documento ya fue agregado al pago.");
                return;
            }

            decimal Saldo = Convert.ToDecimal(Fila["saldo"]);
            int? MonedaPago = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboMoneda));
            bool MismaMoneda = MonedaPago.HasValue && Convert.ToInt64(Fila["idMoneda"]) == MonedaPago.Value;
            if (MismaMoneda && Monto > Saldo)
            {
                ComprasMetMostrarAviso("El monto aplicado excede el saldo pendiente del documento.");
                return;
            }

            _Detalles.Add(new ClsModeloDetallePagoProveedor
            {
                SerieDocumento = Convert.ToString(Fila["serie"]),
                NumeroDocumento = Convert.ToString(Fila["numero"]),
                IdConcepto = Convert.ToInt32(Fila["idConcepto"]),
                NombreConcepto = Convert.ToString(Fila["concepto"]),
                MontoAplicado = Monto,
                SaldoDocumento = Saldo
            });

            ComprasMetFijarTexto(ComprasTxtMonto, "");
            ComprasMetRefrescarDetalle();
        }

        private void ComprasMetQuitarDocumentoClick(object Sender, EventArgs Evento)
        {
            DataGridViewRow Fila = ComprasDgvDetalle.CurrentRow;
            if (Fila == null || Fila.IsNewRow || Fila.Index < 0 || Fila.Index >= _Detalles.Count)
            {
                ComprasMetMostrarAviso("Seleccione un documento de la lista del pago.");
                return;
            }

            _Detalles.RemoveAt(Fila.Index);
            ComprasMetRefrescarDetalle();
        }

        private void ComprasMetRefrescarDetalle()
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("documento", typeof(string));
            Tabla.Columns.Add("concepto", typeof(string));
            Tabla.Columns.Add("monto", typeof(string));

            long IdMoneda = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboMoneda)) ?? 0;
            foreach (ClsModeloDetallePagoProveedor Detalle in _Detalles)
            {
                Tabla.Rows.Add(
                    Detalle.SerieDocumento + "-" + Detalle.NumeroDocumento,
                    Detalle.NombreConcepto,
                    ComprasFuncFormatoMonto(IdMoneda, Detalle.MontoAplicado));
            }

            ComprasDgvDetalle.AutoGenerateColumns = true;
            ComprasDgvDetalle.DataSource = Tabla;

            if (ComprasDgvDetalle.Columns.Contains("documento"))
                ComprasDgvDetalle.Columns["documento"].HeaderText = "Documento";
            if (ComprasDgvDetalle.Columns.Contains("concepto"))
                ComprasDgvDetalle.Columns["concepto"].HeaderText = "Concepto";
            if (ComprasDgvDetalle.Columns.Contains("monto"))
                ComprasDgvDetalle.Columns["monto"].HeaderText = "Monto aplicado";

            decimal Total = _Detalles.Sum(Detalle => Detalle.MontoAplicado);
            string TextoTotal = ComprasFuncFormatoMonto(IdMoneda, Total);
            ComprasLblTotalAplicado.Texto = TextoTotal;
            ComprasLblTotalAplicado.Text = TextoTotal;
        }

        //  GUARDAR / ELIMINAR
        private void ComprasMetGuardarClick(object Sender, EventArgs Evento)
        {
            DateTime Fecha;
            if (!ComprasFuncFecha(ComprasFuncTexto(ComprasTxtFechaPago).Trim(), out Fecha))
            {
                ComprasMetMostrarAviso("La fecha de pago debe tener el formato dd/MM/aaaa.");
                return;
            }

            if (_Detalles.Count == 0)
            {
                ComprasMetMostrarAviso("Agregue al menos un documento por pagar al pago.");
                return;
            }

            string Referencia = ComprasFuncTexto(ComprasTxtReferencia).Trim();

            ClsModeloPagoProveedor Pago = new ClsModeloPagoProveedor();
            Pago.IdPagoProveedor = _IdPagoEditando;
            Pago.IdProveedor = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboProveedorPago)) ?? 0;
            Pago.FechaPagoProveedor = Fecha;
            Pago.MontoPagoProveedor = _Detalles.Sum(Detalle => Detalle.MontoAplicado);
            Pago.IdMetodoPago = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboMetodo)) ?? 0;
            Pago.IdMoneda = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboMoneda)) ?? 0;
            Pago.ReferenciaPagoProveedor = Referencia.Length == 0 ? null : Referencia;
            Pago.EstadoPagoProveedor = _EstadoEditando;
            Pago.Detalles = new List<ClsModeloDetallePagoProveedor>(_Detalles);
            Pago.Estado = _IdPagoEditando == 0 ? EnmEstadoEntidadCompras.Added : EnmEstadoEntidadCompras.Modified;

            List<ValidationResult> Errores = new List<ValidationResult>();
            if (!Validator.TryValidateObject(Pago, new ValidationContext(Pago), Errores, true))
            {
                List<string> Textos = new List<string>();
                foreach (ValidationResult Error in Errores)
                    Textos.Add(Error.ErrorMessage);
                ComprasMetMostrarAviso(string.Join(Environment.NewLine, Textos));
                return;
            }

            string Mensaje = Pago.ComprasMetGrabarCambios();
            if (ComprasFuncEsExito(Mensaje))
            {
                ComprasMetMostrarInformacion(Mensaje);
                ComprasMetLimpiarFormulario();
                ComprasMetCargarGrid();
            }
            else
            {
                ComprasMetMostrarAviso(Mensaje);
            }
        }

        private void ComprasMetEliminarPago(int IdPago, string NoPago)
        {
            if (!ComprasFuncConfirmar("¿Desea eliminar el pago " + NoPago + "?"))
                return;

            ClsModeloPagoProveedor Pago = new ClsModeloPagoProveedor();
            Pago.IdPagoProveedor = IdPago;
            Pago.Estado = EnmEstadoEntidadCompras.Deleted;

            string Mensaje = Pago.ComprasMetGrabarCambios();
            if (ComprasFuncEsExito(Mensaje))
            {
                ComprasMetMostrarInformacion(Mensaje);
                if (_IdPagoEditando == IdPago)
                    ComprasMetLimpiarFormulario();
                else if (_IdPagoEditando == 0)
                    ComprasMetCargarFacturas();
                ComprasMetCargarGrid();
            }
            else
            {
                ComprasMetMostrarAviso(Mensaje);
            }
        }

        // EVENTOS DE CONTROLES 
        private void ComprasMetFiltroCambio(object Sender, EventArgs Evento)
        {
            if (_Cargando)
                return;
            ComprasMetCargarGrid();
        }

        private void ComprasMetProveedorPagoCambio(object Sender, EventArgs Evento)
        {
            if (_Cargando || _IdPagoEditando != 0)
                return;

            if (_Detalles.Count > 0)
            {
                if (!ComprasFuncConfirmar("Al cambiar de proveedor se quitarán los documentos agregados al pago. ¿Desea continuar?"))
                {
                    if (_ProveedorAnterior.HasValue)
                    {
                        _Cargando = true;
                        ComprasMetFijarValorCombo(ComprasCboProveedorPago, _ProveedorAnterior.Value);
                        _Cargando = false;
                    }
                    return;
                }
                _Detalles.Clear();
                ComprasMetRefrescarDetalle();
            }

            ComprasMetCargarFacturas();
        }

        private void ComprasMetFacturaCambio(object Sender, EventArgs Evento)
        {
            if (_Cargando)
                return;
            ComprasMetAutocompletarMonto();
        }

        private void ComprasMetMonedaCambio(object Sender, EventArgs Evento)
        {
            if (_Cargando)
                return;

            if (_Detalles.Count > 0)
            {
                if (!ComprasFuncConfirmar("Al cambiar de moneda se quitarán los documentos agregados al pago. ¿Desea continuar?"))
                {
                    if (_MonedaAnterior.HasValue)
                    {
                        _Cargando = true;
                        ComprasMetFijarValorCombo(ComprasCboMoneda, _MonedaAnterior.Value);
                        _Cargando = false;
                    }
                    return;
                }
                _Detalles.Clear();
            }

            _MonedaAnterior = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboMoneda));
            ComprasMetRefrescarDetalle();
            ComprasMetAutocompletarMonto();
        }

        // ==================== AUXILIARES ====================

        // Deja el rango de fechas en el mes actual
        private void ComprasMetRestablecerRangoFechas()
        {
            DateTime Hoy = DateTime.Today;
            DateTime PrimerDia = new DateTime(Hoy.Year, Hoy.Month, 1);

            comprasUsrRangoFechas1.FechaInicio = PrimerDia;
            comprasUsrRangoFechas1.FechaFin = PrimerDia.AddMonths(1).AddDays(-1);
        }

        private string ComprasFuncCodigoMoneda(long IdMoneda)
        {
            if (_TablaMonedas != null)
            {
                foreach (DataRow Fila in _TablaMonedas.Rows)
                {
                    if (Convert.ToInt64(Fila["idMoneda"]) == IdMoneda)
                        return Convert.ToString(Fila["codigo"]);
                }
            }
            return "";
        }

        private string ComprasFuncFormatoMonto(long IdMoneda, decimal Monto)
        {
            string Codigo = ComprasFuncCodigoMoneda(IdMoneda);
            return (Codigo.Length > 0 ? Codigo + " " : "") + Monto.ToString("N2");
        }

        private bool ComprasFuncConfirmar(string Pregunta)
        {
            return MessageBox.Show(Pregunta, _TituloMensajes, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private static int? ComprasFuncEntero(object Valor)
        {
            if (Valor == null || Valor == DBNull.Value)
                return null;
            int Resultado;
            return int.TryParse(Convert.ToString(Valor), out Resultado) ? (int?)Resultado : null;
        }

        private static bool ComprasFuncFecha(string Texto, out DateTime Fecha)
        {
            return DateTime.TryParseExact(Texto, _FormatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out Fecha);
        }

        private static bool ComprasFuncMonto(string Texto, out decimal Monto)
        {
            string Limpio = (Texto ?? "").Replace("Q", "").Replace("q", "").Trim();
            return decimal.TryParse(Limpio, NumberStyles.Number, CultureInfo.CurrentCulture, out Monto);
        }

        private static bool ComprasFuncEsExito(string Mensaje)
        {
            return Mensaje != null && Mensaje.EndsWith("exitosa", StringComparison.OrdinalIgnoreCase);
        }

        private void ComprasMetMostrarAviso(string Texto)
        {
            MessageBox.Show(Texto, _TituloMensajes, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ComprasMetMostrarInformacion(string Texto)
        {
            MessageBox.Show(Texto, _TituloMensajes, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComprasMetSuscribirEventos()
        {
            ComprasCboProveedor.SelectedIndexChanged += ComprasMetFiltroCambio;
            ComprasCboEstado.SelectedIndexChanged += ComprasMetFiltroCambio;
            // El control avisa con RangoCambiado cada vez que cambia Desde o Hasta
            comprasUsrRangoFechas1.RangoCambiado += ComprasMetFiltroCambio;
            ComprasCboProveedorPago.SelectedIndexChanged += ComprasMetProveedorPagoCambio;
            ComprasCboFactura.SelectedIndexChanged += ComprasMetFacturaCambio;
            ComprasCboMoneda.SelectedIndexChanged += ComprasMetMonedaCambio;
        }

        private void ComprasMetLlenarCombo(ComprasUsrComboBox Combo, object Origen, string MiembroTexto, string MiembroValor)
        {
            if (MiembroTexto != null)
                Combo.DisplayMember = MiembroTexto;
            if (MiembroValor != null)
                Combo.ValueMember = MiembroValor;
            Combo.DataSource = Origen;
        }

        private object ComprasFuncValorCombo(ComprasUsrComboBox Combo)
        {
            return Combo.SelectedValue;
        }

        private string ComprasFuncTextoCombo(ComprasUsrComboBox Combo)
        {
            return Combo.Text;
        }

        private void ComprasMetFijarValorCombo(ComprasUsrComboBox Combo, object Valor)
        {
            Combo.SelectedValue = Valor;
        }

        private void ComprasMetFijarIndiceCombo(ComprasUsrComboBox Combo, int Indice)
        {
            try
            {
                Combo.SelectedIndex = Indice;
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private string ComprasFuncTexto(ComprasUsrTextBox Caja)
        {
            return Caja.Text ?? "";
        }

        private void ComprasMetFijarTexto(ComprasUsrTextBox Caja, string Texto)
        {
            Caja.Text = Texto;
        }


        private void ComprasCboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComprasBtnAgregarDocumento_Click(object sender, EventArgs e)
        {

        }

        private void ComprasUsrEncabezadoFormulario_Load(object sender, EventArgs e)
        {

        }
    }
}