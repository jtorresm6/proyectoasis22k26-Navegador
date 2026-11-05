using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
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
 *  de facturas, y coordina las acciones del usuario con el controlador.
 * ===================================================================
*/

namespace CapaVista_PagosProveedores
{
    public partial class FrmPagosProveedores : Form
    {
        private const string _TituloMensajes = "Pagos a proveedores";
        private const string _FormatoFecha = "dd/MM/yyyy";

        private readonly ClsModeloPagoProveedor _Modelo;
        private DataTable _TablaCuentas;
        private int _IdPagoEditando;
        private string _EstadoEditando;
        private bool _Cargando;

        public FrmPagosProveedores()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            _Modelo = new ClsModeloPagoProveedor();
            _IdPagoEditando = 0;
            _EstadoEditando = "Programado";
            _Cargando = false;

            Load += ComprasMetFormularioLoad;
            ComprasBtnNuevoPago.Click += ComprasMetNuevoPagoClick;
            ComprasBtnGuardar.Click += ComprasMetGuardarClick;
            ComprasBtnCancelar.Click += ComprasMetCancelarClick;
            ComprasBtnEditar.Click += ComprasMetEditarClick;
            ComprasBtnEliminar.Click += ComprasMetEliminarClick;
            ComprasMetSuscribirEventos();
        }

        private void ComprasMetFormularioLoad(object Sender, EventArgs Evento)
        {
            try
            {
                _Cargando = true;

                DataTable Proveedores = _Modelo.ComprasMetObtenerProveedores();

                ComprasMetLlenarCombo(ComprasCboProveedorPago, Proveedores, "nombre", "idProveedor");
                ComprasMetLlenarCombo(ComprasCboEstado, new List<string> { "Todos", "Pagado", "Programado", "Rechazado", "Anulado" }, null, null);
                ComprasMetLlenarCombo(ComprasCboMetodo, new List<string> { "Transferencia", "Cheque", "Efectivo", "Deposito" }, null, null);

                DateTime Hoy = DateTime.Today;
                DateTime PrimerDia = new DateTime(Hoy.Year, Hoy.Month, 1);
                ComprasMetFijarTexto(ComprasTxtDesde, PrimerDia.ToString(_FormatoFecha));
                ComprasMetFijarTexto(ComprasTxtHasta, PrimerDia.AddMonths(1).AddDays(-1).ToString(_FormatoFecha));

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

        private void ComprasMetCargarGrid()
        {
            try
            {
                DateTime? Desde = null;
                DateTime? Hasta = null;
                DateTime FechaLeida;

                string TextoDesde = ComprasFuncTexto(ComprasTxtDesde).Trim();
                string TextoHasta = ComprasFuncTexto(ComprasTxtHasta).Trim();

                if (TextoDesde.Length > 0)
                {
                    if (!ComprasFuncFecha(TextoDesde, out FechaLeida))
                    {
                        ComprasMetMostrarAviso("La fecha Desde debe tener el formato dd/MM/aaaa.");
                        return;
                    }
                    Desde = FechaLeida;
                }

                if (TextoHasta.Length > 0)
                {
                    if (!ComprasFuncFecha(TextoHasta, out FechaLeida))
                    {
                        ComprasMetMostrarAviso("La fecha Hasta debe tener el formato dd/MM/aaaa.");
                        return;
                    }
                    Hasta = FechaLeida;
                }

                int? IdProveedor = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboProveedor));
                string EstadoFiltro = ComprasFuncTextoCombo(ComprasCboEstado);

                List<ClsModeloPagoProveedor> Lista = _Modelo.ComprasMetObtenerTodos(IdProveedor, EstadoFiltro, Desde, Hasta);

                DataTable Tabla = new DataTable();
                Tabla.Columns.Add("idPago", typeof(int));
                Tabla.Columns.Add("noPago", typeof(string));
                Tabla.Columns.Add("proveedor", typeof(string));
                Tabla.Columns.Add("factura", typeof(string));
                Tabla.Columns.Add("fecha", typeof(string));
                Tabla.Columns.Add("monto", typeof(string));
                Tabla.Columns.Add("estado", typeof(string));

                foreach (ClsModeloPagoProveedor Pago in Lista)
                {
                    Tabla.Rows.Add(
                        Pago.IdPagoProveedor,
                        Pago.NoPagoProveedor,
                        Pago.NombreProveedor,
                        Pago.FacturaCuentaPorPagar,
                        Pago.FechaPagoProveedor.ToString(_FormatoFecha),
                        "Q " + Pago.MontoPagoProveedor.ToString("N2"),
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
            if (ComprasDgvPagos.Columns.Contains("factura"))
                ComprasDgvPagos.Columns["factura"].HeaderText = "Factura";
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

        private void ComprasMetLimpiarFormulario()
        {
            _IdPagoEditando = 0;
            _EstadoEditando = "Programado";

            ComprasCboProveedorPago.Enabled = true;
            ComprasCboFactura.Enabled = true;

            _Cargando = true;
            ComprasMetFijarIndiceCombo(ComprasCboProveedorPago, 0);
            ComprasMetFijarIndiceCombo(ComprasCboMetodo, 0);
            _Cargando = false;

            ComprasMetFijarTexto(ComprasTxtFechaPago, DateTime.Today.ToString(_FormatoFecha));
            ComprasMetFijarTexto(ComprasTxtMonto, "");
            ComprasMetFijarTexto(ComprasTxtReferencia, "");

            ComprasMetCargarFacturas();
        }

        private void ComprasMetCargarFacturas()
        {
            try
            {
                int? IdProveedor = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboProveedorPago));
                if (!IdProveedor.HasValue)
                {
                    _TablaCuentas = null;
                    ComprasMetLlenarCombo(ComprasCboFactura, null, null, null);
                    ComprasMetFijarTexto(ComprasTxtMonto, "");
                    return;
                }

                _Cargando = true;
                _TablaCuentas = _Modelo.ComprasMetObtenerCuentasPendientes(IdProveedor.Value);
                ComprasMetLlenarCombo(ComprasCboFactura, _TablaCuentas, "descripcion", "idCuentaPorPagar");
                if (_TablaCuentas.Rows.Count > 0)
                    ComprasMetFijarIndiceCombo(ComprasCboFactura, 0);
                _Cargando = false;

                ComprasMetAutocompletarMonto();
            }
            catch (Exception Ex)
            {
                _Cargando = false;
                ComprasMetMostrarAviso("No se pudieron cargar las facturas por pagar. " + Ex.Message);
            }
        }

        private void ComprasMetAutocompletarMonto()
        {
            if (_IdPagoEditando != 0)
                return;

            int? IdCuenta = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboFactura));
            if (!IdCuenta.HasValue || _TablaCuentas == null)
            {
                ComprasMetFijarTexto(ComprasTxtMonto, "");
                return;
            }

            DataRow[] Filas = _TablaCuentas.Select("idCuentaPorPagar = " + IdCuenta.Value);
            if (Filas.Length > 0)
                ComprasMetFijarTexto(ComprasTxtMonto, Convert.ToDecimal(Filas[0]["saldo"]).ToString("F2"));
            else
                ComprasMetFijarTexto(ComprasTxtMonto, "");
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

                _TablaCuentas = _Modelo.ComprasMetObtenerCuentasPendientes(Pago.IdProveedor);
                if (_TablaCuentas.Select("idCuentaPorPagar = " + Pago.IdCuentaPorPagar).Length == 0)
                {
                    DataRow FilaCuenta = _TablaCuentas.NewRow();
                    FilaCuenta["idCuentaPorPagar"] = Pago.IdCuentaPorPagar;
                    FilaCuenta["descripcion"] = Pago.FacturaCuentaPorPagar;
                    FilaCuenta["saldo"] = 0m;
                    _TablaCuentas.Rows.Add(FilaCuenta);
                }
                ComprasMetLlenarCombo(ComprasCboFactura, _TablaCuentas, "descripcion", "idCuentaPorPagar");
                ComprasMetFijarValorCombo(ComprasCboFactura, Pago.IdCuentaPorPagar);

                ComprasMetFijarTexto(ComprasTxtFechaPago, Pago.FechaPagoProveedor.ToString(_FormatoFecha));
                ComprasMetFijarTexto(ComprasTxtMonto, Pago.MontoPagoProveedor.ToString("F2"));
                ComprasMetFijarTextoCombo(ComprasCboMetodo, Pago.MetodoPagoProveedor);
                ComprasMetFijarTexto(ComprasTxtReferencia, Pago.ReferenciaPagoProveedor ?? "");

                ComprasCboProveedorPago.Enabled = false;
                ComprasCboFactura.Enabled = false;
            }
            finally
            {
                _Cargando = false;
            }
        }

        private void ComprasMetNuevoPagoClick(object Sender, EventArgs Evento)
        {
            if (ComprasFuncHayOperacionEnCurso())
            {
                DialogResult Respuesta = MessageBox.Show(
                    "Hay datos sin guardar. ¿Desea descartarlos y registrar un nuevo pago?",
                    _TituloMensajes,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (Respuesta != DialogResult.Yes)
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

                DialogResult Respuesta = MessageBox.Show(
                    Pregunta,
                    _TituloMensajes,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (Respuesta != DialogResult.Yes)
                    return;
            }

            // Restablece los filtros a su estado inicial
            _Cargando = true;
            DateTime Hoy = DateTime.Today;
            DateTime PrimerDia = new DateTime(Hoy.Year, Hoy.Month, 1);
            ComprasMetFijarTexto(ComprasTxtDesde, PrimerDia.ToString(_FormatoFecha));
            ComprasMetFijarTexto(ComprasTxtHasta, PrimerDia.AddMonths(1).AddDays(-1).ToString(_FormatoFecha));
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

            if (ComprasFuncTexto(ComprasTxtReferencia).Trim().Length > 0)
                return true;

            if (ComprasFuncTexto(ComprasTxtFechaPago).Trim() != DateTime.Today.ToString(_FormatoFecha))
                return true;

            return false;
        }

        private void ComprasMetGuardarClick(object Sender, EventArgs Evento)
        {
            DateTime Fecha;
            if (!ComprasFuncFecha(ComprasFuncTexto(ComprasTxtFechaPago).Trim(), out Fecha))
            {
                ComprasMetMostrarAviso("La fecha de pago debe tener el formato dd/MM/aaaa.");
                return;
            }

            decimal Monto;
            if (!ComprasFuncMonto(ComprasFuncTexto(ComprasTxtMonto), out Monto))
            {
                ComprasMetMostrarAviso("El monto no es válido.");
                return;
            }

            string Referencia = ComprasFuncTexto(ComprasTxtReferencia).Trim();

            ClsModeloPagoProveedor Pago = new ClsModeloPagoProveedor();
            Pago.IdPagoProveedor = _IdPagoEditando;
            Pago.IdProveedor = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboProveedorPago)) ?? 0;
            Pago.IdCuentaPorPagar = ComprasFuncEntero(ComprasFuncValorCombo(ComprasCboFactura)) ?? 0;
            Pago.FechaPagoProveedor = Fecha;
            Pago.MontoPagoProveedor = Monto;
            Pago.MetodoPagoProveedor = ComprasFuncTextoCombo(ComprasCboMetodo);
            Pago.ReferenciaPagoProveedor = Referencia.Length == 0 ? null : Referencia;
            Pago.EstadoPagoProveedor = _EstadoEditando;
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
            DialogResult Respuesta = MessageBox.Show(
                "¿Desea eliminar el pago " + NoPago + "?",
                _TituloMensajes,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (Respuesta != DialogResult.Yes)
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
                else
                    ComprasMetCargarFacturas();
                ComprasMetCargarGrid();
            }
            else
            {
                ComprasMetMostrarAviso(Mensaje);
            }
        }

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
            ComprasMetCargarFacturas();
        }

        private void ComprasMetFacturaCambio(object Sender, EventArgs Evento)
        {
            if (_Cargando)
                return;
            ComprasMetAutocompletarMonto();
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
            ComprasTxtDesde.Leave += ComprasMetFiltroCambio;
            ComprasTxtHasta.Leave += ComprasMetFiltroCambio;
            ComprasCboProveedorPago.SelectedIndexChanged += ComprasMetProveedorPagoCambio;
            ComprasCboFactura.SelectedIndexChanged += ComprasMetFacturaCambio;
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

        private void ComprasMetFijarTextoCombo(ComprasUsrComboBox Combo, string Texto)
        {
            Combo.Text = Texto;
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
    }
}
