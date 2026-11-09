using CapaVista_Componentes.Componentes;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaControlador_OrdenesCompra.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta la factura y los datos del proveedor de una orden de compra,
 *  gestiona las fechas, muestra sus importes y permite su anulación.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    public partial class FrmOrdenesCompra : System.Windows.Forms.Form
    {
        private bool _AjustandoAltura;
        private const int _AlturaMinimaArticulo = 40;
        private readonly ClsComprasPorPagar _Controlador;
        private ClsCompraPorPagar _Compra;
        private bool _Anulando;
        public bool OrdenGuardada { get; private set; }

        public FrmOrdenesCompra() : this(null, new ClsComprasPorPagar()) { }

        public FrmOrdenesCompra(ClsCompraPorPagar Compra, ClsComprasPorPagar Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            _Compra = Compra;
            InitializeComponent();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasCboNombreProveedor.SelectedIndexChanged += ComprasProcActualizarProveedor;
            if (Compra == null) ComprasDtpFechaEmision.MaxDate = ComprasDtpFechaVencimiento.MaxDate.AddMonths(-3);
            ComprasDtpFechaEmision.ValueChanged += ComprasProcActualizarVencimiento;
            ComprasProcConfigurarArticulos();
            ComprasProcConfigurarAlturaArticulos();
            ComprasTlpMantenimientoProductos.Enabled = false;
            ComprasBtnGuardar.Enabled = false;
            ComprasBtnGuardar.AccessibleDescription = "Se requieren productos para guardar la orden.";
            ComprasBtnDescartar.AccessibleName = Compra == null ? "Descartar" : "Anular orden";
            ComprasBtnDescartar.Enabled = true;
            ComprasBtnDescartar.Click += ComprasProcAnular;
            ComprasBtnRegresar.Click += (Remitente, Evento) => Close();
            FormClosing += (Remitente, Evento) => { if (_Anulando) Evento.Cancel = true; };
            ComprasProcMostrarEncabezado();
        }

        private void ComprasProcMostrarEncabezado()
        {
            bool Nueva = _Compra == null;
            ComprasLblNumeroDeFactura.MostrarAsterisco = Nueva;
            ComprasLblFechaDeEmision.MostrarAsterisco = Nueva;
            ComprasLblFechaDeVencimiento.MostrarAsterisco = Nueva;
            ComprasTxtNumeroDeFacturaValor.Enabled = Nueva;
            ComprasTxtNumeroDeFacturaValor.MaxLength = 30;
            ComprasDtpFechaEmision.Enabled = Nueva;
            ComprasDtpFechaVencimiento.Enabled = Nueva;
            ComprasCboNombreProveedor.Enabled = Nueva;
            ComprasTxtComentario.Enabled = false;
            ComprasTxtEstadoProveedor.Text = string.Empty;
            ComprasTxtEstadoProveedor.Enabled = false;
            ComprasTxtDireccionProveedor.Enabled = false;
            ComprasTxtNumeroDeFacturaValor.Text = Nueva ? string.Empty : _Compra.NumeroFactura;
            ComprasDtpFechaEmision.Value = Nueva ? DateTime.Today : _Compra.FechaEmision;
            ComprasDtpFechaVencimiento.Value = Nueva ? ComprasDtpFechaEmision.Value.Date.AddMonths(3) : _Compra.FechaVencimiento;
            ComprasTxtSaldoPendienteValor.Text = ClsPresentacionCompras.ComprasFuncFormatearMonto(Nueva ? 0M : _Compra.Saldo,
                ComprasTxtSaldoPendienteValor.TipoMoneda);
            ComprasLblEstadoValor.Texto = Nueva ? "Nueva" : _Compra.Estado;
            ComprasTxtTotalDeLaOrden.Text = ClsPresentacionCompras.ComprasFuncFormatearMonto(Nueva ? 0M : _Compra.MontoTotal,
                ComprasTxtTotalDeLaOrden.TipoMoneda);
            foreach (var Campo in new[] { ComprasTxtImporteBruto, ComprasTxtSubtotal,
                ComprasTxtDescuentoTotal, ComprasTxtImpuestoTotal, ComprasTxtTotalPorcentaje })
            {
                Campo.Text = Nueva ? (Campo == ComprasTxtTotalPorcentaje ? "0.00" :
                    ClsPresentacionCompras.ComprasFuncFormatearMonto(0M, Campo.TipoMoneda)) : string.Empty;
                Campo.Enabled = false;
            }
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
        }

        private void ComprasProcActualizarVencimiento(object Remitente, EventArgs Evento)
        {
            if (_Compra != null) return;
            ComprasDtpFechaVencimiento.Value = ComprasDtpFechaEmision.Value.Date.AddMonths(3);
        }

        protected override void OnShown(EventArgs Evento)
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
            }
        }

        private async void ComprasProcAnular(object Remitente, EventArgs Evento)
        {
            if (_Anulando) return;
            if (_Compra == null) { Close(); return; }
            if (MessageBox.Show(this, "¿Anular la factura " + _Compra.NumeroFactura + "?",
                "Anular orden", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            _Anulando = true;
            ComprasBtnDescartar.Enabled = false;
            ComprasBtnRegresar.Enabled = false;
            UseWaitCursor = true;
            try
            {
                int Id = _Compra.IdCuentaPorPagar;
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
                ComprasBtnDescartar.Enabled = true;
                ComprasBtnRegresar.Enabled = true;
                UseWaitCursor = false;
            }
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

        private void ComprasProcConfigurarAlturaArticulos()
        {
            var Tabla = ComprasDgvProductos.Tabla;
            Tabla.RowTemplate.MinimumHeight = _AlturaMinimaArticulo;
            Tabla.RowTemplate.Height = Math.Max(Tabla.RowTemplate.Height, _AlturaMinimaArticulo);
            Tabla.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            ComprasPnlContenido.MinimumSize = System.Drawing.Size.Empty;
            ComprasTlpPrincipal.RowStyles[2].SizeType = System.Windows.Forms.SizeType.Absolute;
            ComprasTlpDetalles.RowStyles[1].SizeType = System.Windows.Forms.SizeType.Absolute;
            Tabla.RowsAdded += (Remitente, Evento) => ComprasProcAjustarAlturaArticulos();
            Tabla.RowsRemoved += (Remitente, Evento) => ComprasProcAjustarAlturaArticulos();
            Tabla.RowHeightChanged += (Remitente, Evento) => ComprasProcAjustarAlturaArticulos();
            Tabla.ColumnHeadersHeightChanged += (Remitente, Evento) => ComprasProcAjustarAlturaArticulos();
            Tabla.DataBindingComplete += (Remitente, Evento) => ComprasProcAjustarAlturaArticulos();
            Tabla.SizeChanged += (Remitente, Evento) => ComprasProcAjustarAlturaArticulos();
            Shown += (Remitente, Evento) => ComprasProcAjustarAlturaArticulos();
            ComprasProcAjustarAlturaArticulos();
        }

        private void ComprasProcAjustarAlturaArticulos()
        {
            if (_AjustandoAltura || IsDisposed) return;
            _AjustandoAltura = true;
            try
            {
                var Tabla = ComprasDgvProductos.Tabla;
                foreach (System.Windows.Forms.DataGridViewRow Fila in Tabla.Rows)
                {
                    if (Fila.MinimumHeight != _AlturaMinimaArticulo)
                        Fila.MinimumHeight = _AlturaMinimaArticulo;
                }
                int Altura = Tabla.ColumnHeadersVisible ? Tabla.ColumnHeadersHeight : 0;
                Altura += Tabla.Rows.GetRowsHeight(System.Windows.Forms.DataGridViewElementStates.Visible);
                Altura += ComprasDgvProductos.Height - Tabla.ClientSize.Height;
                foreach (System.Windows.Forms.Control Control in Tabla.Controls)
                {
                    if (Control is System.Windows.Forms.HScrollBar && Control.Visible)
                        Altura += Control.Height;
                }
                ComprasTlpDetalles.RowStyles[1].Height = Math.Max(1, Altura);
                int AlturaDetalles = (int)ComprasTlpDetalles.RowStyles[0].Height + Math.Max(1, Altura)
                    + ComprasTlpDetalles.Margin.Vertical + ComprasGbxDetalles.Margin.Vertical
                    + ComprasGbxDetalles.Height - ComprasGbxDetalles.DisplayRectangle.Height;
                ComprasTlpPrincipal.RowStyles[2].Height = AlturaDetalles;
                ComprasPnlContenido.Height = (int)ComprasTlpPrincipal.RowStyles[0].Height
                    + (int)ComprasTlpPrincipal.RowStyles[1].Height
                    + AlturaDetalles + (int)ComprasTlpPrincipal.RowStyles[3].Height
                    + ComprasTlpPrincipal.Padding.Vertical + ComprasPnlContenido.Padding.Vertical;
            }
            finally
            {
                _AjustandoAltura = false;
            }
        }

        private void ComprasProcConfigurarArticulos()
        {
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "IDarticulo", "ID artículo", 8, false);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "nombre", "Nombre", 22, false);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "unidad", "Unidad", 7, false);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "cantidad", "Cantidad", 8, true);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "precioUnitario", "Precio unitario", 11, true);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "subtotal", "Subtotal", 11, true);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "descuento", "Descuento", 10, true);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "porcentajeImpuesto", "Impuesto %", 8, false);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "impuesto", "Impuesto", 10, true);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvProductos, "total", "Total", 12, true);
            ComprasDgvProductos.Columns["ComprasColporcentajeImpuesto"].DefaultCellStyle.Format = "P0";
            ComprasDgvProductos.Tabla.DefaultCellStyle.Alignment =
                System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            ComprasDgvProductos.Tabla.ColumnHeadersDefaultCellStyle.Alignment =
                System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            foreach (System.Windows.Forms.DataGridViewColumn Columna in ComprasDgvProductos.Columns)
            {
                Columna.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            }
            ComprasDgvProductos.ReadOnly = true;
            ComprasDgvProductos.AllowUserToAddRows = false;
            ComprasDgvProductos.AllowUserToDeleteRows = false;

            ComprasDgvProductos.ClearSelection();
        }
    }
}
