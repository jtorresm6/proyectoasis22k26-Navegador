using CapaVista_OrdenesCompra.Presentacion;
using CapaVista_Componentes.Componentes;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using Microsoft.Reporting.WinForms;
using System.Data;
using System.Globalization;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Presenta los detalles de una orden y permite generar su reporte o anularla.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    public partial class FrmDetalleOrdenCompra : System.Windows.Forms.Form
    {
        private readonly ClsDetallesOrdenesCompraDiseno _DisenoDetalles;
        private readonly ClsOrdenesCompraComunEdicion _Edicion;
        private readonly ClsOrdenesCompraControlador _Controlador;
        private ClsOrdenesCompraDatos _Compra;
        private bool _Anulando;
        private bool _DetallesCargados;
        private string _MonedaReporte;
        public bool OrdenGuardada { get; private set; }

        public FrmDetalleOrdenCompra() : this(null, new ClsOrdenesCompraControlador()) { }

        public FrmDetalleOrdenCompra(ClsOrdenesCompraDatos Compra, ClsOrdenesCompraControlador Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            _Compra = Compra;
            InitializeComponent();
            _Edicion = new ClsOrdenesCompraComunEdicion(this);
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasProcSoloLectura(this);
            ComprasDgvProductos.Tabla.ReadOnly = true;
            ComprasDgvProductos.Tabla.AllowUserToAddRows = false;
            ComprasDgvProductos.Tabla.AllowUserToDeleteRows = false;
            ComprasCboNombreProveedor.SelectedIndexChanged += ComprasProcActualizarProveedor;
            ComprasDtpFechaEmision.ValueChanged += ComprasProcActualizarVencimiento;
            ClsDetallesOrdenesCompraTabla.ComprasProcConfigurarDetalles(ComprasDgvProductos);
            _DisenoDetalles = new ClsDetallesOrdenesCompraDiseno(this, ComprasDgvProductos,
                ComprasPnlContenido, ComprasTlpPrincipal, ComprasTlpDetalles, ComprasGbxDetalles);
            _Edicion.Aplicar(ComprasTlpMantenimientoProductos, false);
            ComprasBtnGuardar.Enabled = false;
            ComprasBtnGuardar.AccessibleDescription = "Se requieren productos para guardar la orden.";
            ComprasBtnDescartar.AccessibleName = "Anular orden";
            ComprasBtnDescartar.Enabled = ComprasFuncPuedeAnular();
            ComprasBtnDescartar.Click += ComprasProcAnular;
            ComprasBtnRegresar.Click += (Remitente, Evento) => Close();
            ComprasUsrEncabezado.PrepararReporte += (Remitente, Evento) => ComprasProcPrepararReporte(Evento.Reporte);
            FormClosing += (Remitente, Evento) => { if (_Anulando) Evento.Cancel = true; };
            ComprasProcMostrarEncabezado();
        }

        private void ComprasProcSoloLectura(Control Contenedor)
        {
            foreach (Control Campo in Contenedor.Controls)
            {
                if (Campo is ComprasUsrTextBox || Campo is ComprasUsrComboBox || Campo is DateTimePicker)
                {
                    _Edicion.Establecer(Campo, false);
                    var Texto = Campo as ComprasUsrTextBox;
                    if (Texto != null) Texto.MostrarEtiquetaError = false;
                }
                else ComprasProcSoloLectura(Campo);
            }
        }

        private void ComprasProcMostrarEncabezado()
        {
            bool Nueva = _Compra == null;
            ComprasLblCodigoOrden.MostrarAsterisco = false;
            ComprasLblFechaDeEmision.MostrarAsterisco = false;
            ComprasLblFechaDeVencimiento.MostrarAsterisco = false;
            ComprasTxtCodigoOrden.Enabled = true;
            ComprasTxtCodigoOrden.MaxLength = 30;
            ComprasDtpFechaEmision.Enabled = true;
            ComprasDtpFechaVencimiento.Enabled = true;
            ComprasCboNombreProveedor.Enabled = true;
            ComprasTxtComentario.Enabled = true;
            ComprasTxtComentario.Text = Nueva ? string.Empty : _Compra.Comentario;
            ComprasTxtEstadoProveedor.Text = string.Empty;
            ComprasTxtEstadoProveedor.Enabled = true;
            ComprasTxtDireccionProveedor.Enabled = true;
            ComprasTxtCodigoOrden.Text = Nueva ? string.Empty : _Compra.CodigoOrdenCompra;
            ComprasDtpFechaEmision.Value = Nueva ? DateTime.Today : _Compra.FechaEmision;
            ComprasDtpFechaVencimiento.Value = Nueva ? ComprasDtpFechaEmision.Value.Date : _Compra.FechaVencimiento;
            ComprasTxtPlazoValor.Text = Nueva ? string.Empty : _Compra.NombrePlazo;
            ComprasLblEstadoValor.Texto = Nueva ? "Nueva" : _Compra.Estado;
            ComprasTxtMoneda.Text = Nueva ? string.Empty : _Compra.IdMoneda.ToString();
            ComprasTxtTipoCambio.Text = Nueva ? string.Empty : _Compra.TipoCambio.ToString("N6");
            ComprasTxtDiasPlazo.Text = Nueva ? string.Empty : _Compra.DiasPlazo.ToString();
            ComprasTxtSubtotal.Text = (Nueva ? 0M : _Compra.Subtotal).ToString("N4");
            ComprasTxtImpuesto.Text = (Nueva ? 0M : _Compra.Impuesto).ToString("N4");
            ComprasTxtDescuentoTotal.Text = (Nueva ? 0M : _Compra.Descuento).ToString("N4");
            ComprasTxtTotalDeLaOrden.Text = (Nueva ? 0M : _Compra.MontoTotal).ToString("N4");
            foreach (var Campo in new[] { ComprasTxtMoneda, ComprasTxtTipoCambio, ComprasTxtDiasPlazo,
                ComprasTxtSubtotal, ComprasTxtDescuentoTotal, ComprasTxtPlazoValor })
                Campo.Enabled = true;
            ComprasTxtTotalDeLaOrden.Enabled = true;
            if (!Nueva)
            {
                ComprasCboNombreProveedor.TipoDatos = EnmTipoDatosCompras.Ninguno;
                ComprasCboNombreProveedor.DataSource = null;
                ComprasCboNombreProveedor.Items.Clear();
                ComprasCboNombreProveedor.Items.Add(_Compra.NombreProveedor);
                ComprasCboNombreProveedor.SelectedIndex = 0;
                ComprasTxtNitProveedor.Text = _Compra.NitProveedor;
                ComprasTxtTelefonoProveedor.Text = _Compra.TelefonoProveedor;
                ComprasTxtCorreoProveedor.Text = _Compra.CorreoProveedor;
                ComprasTxtDireccionProveedor.Text = _Compra.DireccionProveedor;
                ComprasTxtEstadoProveedor.Text = _Compra.ProveedorActivo ? "Activo" : "Inactivo";
            }
            ComprasProcSoloLectura(this);
        }

        private void ComprasProcActualizarVencimiento(object Remitente, EventArgs Evento)
        {
            if (_Compra != null) return;
            ComprasDtpFechaVencimiento.Value = ComprasDtpFechaEmision.Value.Date;
        }

        protected override async void OnShown(EventArgs Evento)
        {
            base.OnShown(Evento);
            if (_Compra == null)
            {
                ComprasCboNombreProveedor.SelectedIndex = -1;
                ComprasProcActualizarProveedor(this, EventArgs.Empty);
            }
            else
            {
                ComprasProcMostrarEncabezado();
                await ComprasProcCargarDetallesAsync();
            }
        }

        internal Task ComprasProcPrepararAsync()
        {
            return ComprasProcCargarDetallesAsync(true);
        }

        private async Task ComprasProcCargarDetallesAsync(bool PropagarError = false)
        {
            if (_DetallesCargados || _Compra == null) return;
            try
            {
                int Id = _Compra.IdOrdenCompra;
                var Detalles = await Task.Run(() =>
                {
                    using (var Monedas = _Controlador.ComprasFuncListarMonedas())
                    {
                        foreach (DataRow Moneda in Monedas.Rows)
                            if (Convert.ToInt32(Moneda["idMoneda"]) == _Compra.IdMoneda)
                                _MonedaReporte = Convert.ToString(Moneda["codigoMoneda"]) + " - " + Convert.ToString(Moneda["nombreMoneda"]);
                    }
                    return _Controlador.ComprasFuncListarDetalles(Id);
                });
                if (IsDisposed || Disposing)
                {
                    Detalles.Dispose();
                    return;
                }
                ComprasDgvProductos.DataSource = Detalles;
                ComprasDgvProductos.ClearSelection();
                _DetallesCargados = true;
            }
            catch (Exception Ex)
            {
                if (PropagarError) throw;
                if (!IsDisposed && !Disposing)
                    MessageBox.Show(this, "No se pudieron cargar los detalles. " + Ex.Message,
                        "Órdenes de compra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComprasProcPrepararReporte(LocalReport Reporte)
        {
            var Detalles = ComprasDgvProductos.DataSource as DataTable;
            if (_Anulando || !_DetallesCargados || _Compra == null || Detalles == null)
                throw new InvalidOperationException("Espera a que termine la carga de la orden antes de abrir el reporte.");
            if (string.IsNullOrWhiteSpace(_MonedaReporte))
                throw new InvalidOperationException("No se pudo identificar la moneda de la orden. Actualiza la consulta antes de generar el reporte.");
            ClsDetallesOrdenesCompraReporte.Preparar(Reporte, _Compra, Detalles, _MonedaReporte);
        }

        private void ComprasProcLiberarDetalles()
        {
            _Edicion?.Dispose();
            _DisenoDetalles?.Dispose();
            var Detalles = ComprasDgvProductos.DataSource as System.Data.DataTable;
            ComprasDgvProductos.DataSource = null;
            Detalles?.Dispose();
        }

        private async void ComprasProcAnular(object Remitente, EventArgs Evento)
        {
            if (_Anulando || !ComprasFuncPuedeAnular()) return;
            if (MessageBox.Show(this, "¿Anular la orden " + _Compra.CodigoOrdenCompra + "?",
                "Anular orden", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            _Anulando = true;
            ComprasBtnDescartar.Enabled = false;
            ComprasBtnRegresar.Enabled = false;
            UseWaitCursor = true;
            try
            {
                int Id = _Compra.IdOrdenCompra;
                _Compra = await Task.Run(() => _Controlador.ComprasFuncAnular(Id));
                OrdenGuardada = true;
                ComprasProcMostrarEncabezado();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "No se pudo anular la orden. " + Ex.Message,
                    "Órdenes de compra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _Anulando = false;
                ComprasBtnDescartar.Enabled = ComprasFuncPuedeAnular();
                ComprasBtnRegresar.Enabled = true;
                UseWaitCursor = false;
            }
        }

        private bool ComprasFuncPuedeAnular()
        {
            return _Compra != null && !_Compra.Bloqueado
                && (_Compra.Estado == "Pendiente" || _Compra.Estado == "Autorizada" || _Compra.Estado == "Rechazada");
        }

        private void ComprasProcActualizarProveedor(object Remitente, EventArgs Evento)
        {
            if (_Compra != null) return;
            ComprasTxtTelefonoProveedor.Text = string.Empty;
            ComprasTxtCorreoProveedor.Text = string.Empty;
            ComprasTxtNitProveedor.Text = string.Empty;
            ComprasTxtDireccionProveedor.Text = string.Empty;
            ComprasTxtEstadoProveedor.Text = string.Empty;
            try
            {
                var Proveedor = ComprasCboNombreProveedor.ComprasFuncObtenerProveedorSeleccionado();
                if (Proveedor == null) return;
                ComprasTxtTelefonoProveedor.Text = Proveedor.TelefonoProveedor;
                ComprasTxtCorreoProveedor.Text = Proveedor.CorreoProveedor;
                ComprasTxtNitProveedor.Text = Proveedor.NitProveedor;
                ComprasTxtDireccionProveedor.Text = Proveedor.DireccionProveedor;
                ComprasTxtEstadoProveedor.Text = Proveedor.EstadoProveedor ? "Activo" : "Inactivo";
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine(Ex);
                ComprasCboNombreProveedor.ComprasMetMostrarError("No se pudieron obtener los datos del proveedor.");
            }
        }

    }
}
