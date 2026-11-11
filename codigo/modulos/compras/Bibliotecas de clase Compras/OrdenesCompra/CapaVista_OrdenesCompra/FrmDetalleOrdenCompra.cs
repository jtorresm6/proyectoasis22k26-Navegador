using CapaVista_OrdenesCompra.Presentacion;
using CapaVista_Componentes.Componentes;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaControlador_OrdenesCompra.Entidades;
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
        private readonly ClsDisenoDetallesOrdenCompra _DisenoDetalles;
        private readonly ClsOrdenesCompra _Controlador;
        private ClsOrdenCompra _Compra;
        private bool _Anulando;
        private bool _DetallesCargados;
        private string _MonedaReporte;
        public bool OrdenGuardada { get; private set; }

        public FrmDetalleOrdenCompra() : this(null, new ClsOrdenesCompra()) { }

        public FrmDetalleOrdenCompra(ClsOrdenCompra Compra, ClsOrdenesCompra Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            _Compra = Compra;
            InitializeComponent();
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasProcSoloLectura(this);
            ComprasDgvProductos.Tabla.ReadOnly = true;
            ComprasDgvProductos.Tabla.AllowUserToAddRows = false;
            ComprasDgvProductos.Tabla.AllowUserToDeleteRows = false;
            ComprasCboNombreProveedor.SelectedIndexChanged += ComprasProcActualizarProveedor;
            ComprasDtpFechaEmision.ValueChanged += ComprasProcActualizarVencimiento;
            ClsTablasOrdenesCompra.ComprasProcConfigurarDetalles(ComprasDgvProductos);
            _DisenoDetalles = new ClsDisenoDetallesOrdenCompra(this, ComprasDgvProductos,
                ComprasPnlContenido, ComprasTlpPrincipal, ComprasTlpDetalles, ComprasGbxDetalles);
            ComprasTlpMantenimientoProductos.Enabled = false;
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

        private static void ComprasProcSoloLectura(Control Contenedor)
        {
            foreach (Control Campo in Contenedor.Controls)
            {
                var Texto = Campo as ComprasUsrTextBox;
                if (Texto != null)
                {
                    Texto.Enabled = true;
                    Texto.ReadOnly = true;
                    Texto.MostrarEtiquetaError = false;
                    Texto.TabStop = false;
                }
                if (Campo is ComprasUsrComboBox || Campo is DateTimePicker) Campo.Enabled = false;
                ComprasProcSoloLectura(Campo);
            }
        }

        private void ComprasProcMostrarEncabezado()
        {
            bool Nueva = _Compra == null;
            ComprasLblCodigoOrden.MostrarAsterisco = false;
            ComprasLblFechaDeEmision.MostrarAsterisco = false;
            ComprasLblFechaDeVencimiento.MostrarAsterisco = false;
            ComprasTxtCodigoOrden.Enabled = false;
            ComprasTxtCodigoOrden.MaxLength = 30;
            ComprasDtpFechaEmision.Enabled = false;
            ComprasDtpFechaVencimiento.Enabled = false;
            ComprasCboNombreProveedor.Enabled = false;
            ComprasTxtComentario.Enabled = false;
            ComprasTxtComentario.Text = Nueva ? string.Empty : _Compra.Comentario;
            ComprasTxtEstadoProveedor.Text = string.Empty;
            ComprasTxtEstadoProveedor.Enabled = false;
            ComprasTxtDireccionProveedor.Enabled = false;
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
                Campo.Enabled = false;
            ComprasTxtTotalDeLaOrden.Enabled = false;
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
            Reporte.DataSources.Clear();
            Reporte.DataSources.Add(new ReportDataSource("DetalleOrdenCompra", Detalles));
            Reporte.DisplayName = "Orden de compra " + _Compra.CodigoOrdenCompra;
            var Formato = CultureInfo.GetCultureInfo("es-GT");
            Reporte.SetParameters(new[]
            {
                new ReportParameter("Orden", "Orden No. " + _Compra.CodigoOrdenCompra + " | Estado: " + _Compra.Estado),
                new ReportParameter("Proveedor", "PROVEEDOR: " + _Compra.NombreProveedor + "\nNIT: " + _Compra.NitProveedor
                    + "\nDirección: " + _Compra.DireccionProveedor + "\nTel.: " + _Compra.TelefonoProveedor + " | Correo: " + _Compra.CorreoProveedor),
                new ReportParameter("Condiciones", "Emisión: " + _Compra.FechaEmision.ToString("dd/MM/yyyy")
                    + " | Vencimiento: " + _Compra.FechaVencimiento.ToString("dd/MM/yyyy")
                    + "\nPlazo: " + _Compra.NombrePlazo + " (" + _Compra.DiasPlazo + " días) | Moneda: " + _MonedaReporte),
                new ReportParameter("Totales", "Subtotal: " + _Compra.Subtotal.ToString("N4", Formato)
                    + "\nDescuento: " + _Compra.Descuento.ToString("N4", Formato)
                    + "\nImpuesto: " + _Compra.Impuesto.ToString("N4", Formato)
                    + "\nTOTAL (" + _MonedaReporte + "): " + _Compra.MontoTotal.ToString("N4", Formato)),
                new ReportParameter("Comentario", string.IsNullOrWhiteSpace(_Compra.Comentario) ? "Sin observaciones." : _Compra.Comentario)
            });
        }

        private void ComprasProcLiberarDetalles()
        {
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
