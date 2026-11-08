using System;
using System.Data;
using System.Data.Odbc;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_MovimientosProveedores;
using CapaVista_Componentes.Componentes;


/*
 * ==================================================================
 * Área : Compras
 * Autor : Carlos Andres Arriaza Lara
 * Carné : 0901-23-13862
 * Fecha : 30/10/2026
 * ==================================================================
 * Propósito :
 *  Registrar documentos de proveedor en cuentas por pagar según el concepto seleccionado.
 * ===================================================================
*/
namespace CapaVista_MovimientosProveedores
{
    public partial class FrmMovimientosProveedores : Form
    {
        private const string TextoReintento = "Reintentar carga";
        private const string InstruccionReintento = "\n\nUse el enlace «" + TextoReintento + "» en el formulario.";

        private readonly ClsModeloNotaProveedor _ModeloNotaProveedor = new ClsModeloNotaProveedor();
        private bool _Guardando;
        private bool _CatalogosListos;
        private int _VersionTipoCambio;
        private decimal? _TipoCambio;

        public FrmMovimientosProveedores()
        {
            InitializeComponent();
            ComprasTxtTipoCambio.Entrada.TabStop = false;
            Load += async (Remitente, Evento) => await ComprasProcCargarCatalogos();
            ComprasLblEstado.LinkClicked += async (Remitente, Evento) => await ComprasProcCargarCatalogos();
            ComprasCboConcepto.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarResumen();
            ComprasTxtMonto.TextChanged += (Remitente, Evento) => ComprasProcActualizarResumen();
            ComprasCboMoneda.SelectedIndexChanged += async (Remitente, Evento) => await ComprasProcCargarTipoCambio();
            ComprasDtpFechaEmision.ValueChanged += async (Remitente, Evento) => {
                if (ComprasDtpFechaVencimiento.Value.Date < ComprasDtpFechaEmision.Value.Date)
                    ComprasDtpFechaVencimiento.Value = ComprasDtpFechaEmision.Value.Date;
                await ComprasProcCargarTipoCambio();
            };
            ComprasBtnGuardar.Click += (Remitente, Evento) => ComprasProcGuardarDocumento();
            ComprasBtnNueva.Click += (Remitente, Evento) => ComprasProcLimpiarFormulario();
            ComprasProcLimpiarFormulario();
            ComprasBtnGuardar.Enabled = false;
        }

        private static void ComprasProcLlenarSelector(ComprasUsrComboBox Selector, DataTable Tabla, string TextoDescriptivo, string CampoValor)
        {
            Selector.DisplayMember = TextoDescriptivo;
            Selector.ValueMember = CampoValor;
            Selector.DataSource = Tabla;
            Selector.SelectedIndex = -1;
        }

        private async Task ComprasProcCargarCatalogos()
        {
            _CatalogosListos = false;
            ComprasBtnGuardar.Enabled = false;
            try
            {
                object IdProveedor = ComprasCboProveedor.SelectedValue;
                object IdConcepto = ComprasCboConcepto.SelectedValue;
                object IdMoneda = ComprasCboMoneda.SelectedValue;
                DataTable Proveedores = _ModeloNotaProveedor.ComprasMetObtenerProveedores();
                DataTable Conceptos = _ModeloNotaProveedor.ComprasMetObtenerConceptos();
                DataTable Monedas = _ModeloNotaProveedor.ComprasMetObtenerMonedas();

                ComprasProcLlenarSelector(ComprasCboProveedor, Proveedores, "nombreProveedor", "idProveedor");
                ComprasProcLlenarSelector(ComprasCboConcepto, Conceptos, "nombreConceptoProveedor", "idConceptoProveedor");
                ComprasProcLlenarSelector(ComprasCboMoneda, Monedas, "nombreMoneda", "idMoneda");
                if (IdProveedor != null) ComprasCboProveedor.SelectedValue = IdProveedor;
                if (IdConcepto != null) ComprasCboConcepto.SelectedValue = IdConcepto;
                if (IdMoneda != null) ComprasCboMoneda.SelectedValue = IdMoneda;

                _CatalogosListos = ComprasCboProveedor.Items.Count > 0 && ComprasCboConcepto.Items.Count > 0 && ComprasCboMoneda.Items.Count > 0;
                ComprasBtnGuardar.Enabled = _CatalogosListos;
                if (!_CatalogosListos)
                    ComprasProcMostrarEstado("Faltan proveedores, conceptos de proveedor o monedas disponibles. Complete los catálogos.", PermitirReintento: true);
                else
                    ComprasProcMostrarEstado("Seleccione un proveedor y complete los datos del documento.");
            }
            catch (Exception Error) { ComprasProcMostrarError("No se pudieron cargar los catálogos. Revise la conexión.", Error, PermitirReintento: true); }
            await ComprasProcCargarTipoCambio();
        }

        private async Task ComprasProcCargarTipoCambio()
        {
            int Version = ++_VersionTipoCambio;
            _TipoCambio = null;
            ComprasTxtTipoCambio.Clear();
            ComprasProcActualizarResumen();

            var Moneda = ComprasCboMoneda.SelectedItem as DataRowView;
            if (Moneda == null)
            {
                ComprasTxtMonto.TipoMoneda = string.Empty;
            }
            else
            {
                string Codigo = Convert.ToString(Moneda["codigoMoneda"]);
                string Simbolo = Convert.ToString(Moneda["simboloMoneda"]);
                ComprasTxtMonto.ComprasMetConfigurarMoneda(Codigo, string.IsNullOrWhiteSpace(Simbolo) ? Codigo : Simbolo);
            }

            long IdMoneda = ComprasFuncObtenerIdMoneda();
            if (!_CatalogosListos || IdMoneda == 0)
            {
                ComprasBtnGuardar.Enabled = _CatalogosListos && !_Guardando;
                return;
            }

            DateTime FechaEmision = ComprasDtpFechaEmision.Value.Date;
            ComprasBtnGuardar.Enabled = false;
            try
            {
                decimal? TipoCambio = await Task.Run(() => _ModeloNotaProveedor.ComprasMetObtenerTipoCambio(IdMoneda, FechaEmision));
                if (Version != _VersionTipoCambio || IsDisposed || Disposing) return;

                _TipoCambio = TipoCambio;
                ComprasTxtTipoCambio.Text = TipoCambio.HasValue ? $"Q {TipoCambio.Value:N6}" : string.Empty;
                ComprasProcActualizarResumen();
            }
            catch (Exception Error)
            {
                if (Version == _VersionTipoCambio && !IsDisposed && !Disposing)
                    ComprasProcMostrarError("No se pudo consultar el tipo de cambio.", Error, PermitirReintento: true);
            }
            finally
            {
                if (Version == _VersionTipoCambio && !IsDisposed && !Disposing)
                    ComprasBtnGuardar.Enabled = _CatalogosListos && !_Guardando;
            }
        }

        private int ComprasFuncObtenerIdSeleccionado(ComprasUsrComboBox Selector)
        {
            return Selector.SelectedItem is DataRowView ? Convert.ToInt32(Selector.SelectedValue) : 0;
        }

        private long ComprasFuncObtenerIdMoneda()
        {
            var Moneda = ComprasCboMoneda.SelectedItem as DataRowView;
            return Moneda == null ? 0L : Convert.ToInt64(Moneda["idMoneda"]);
        }

        private void ComprasProcActualizarResumen()
        {
            var Concepto = ComprasCboConcepto.SelectedItem as DataRowView;
            string Naturaleza = Convert.ToString(Concepto?["naturalezaConceptoProveedor"]).Trim();
            if (string.Equals(Naturaleza, "Cargo", StringComparison.OrdinalIgnoreCase))
                ComprasLblEfecto.Text = "Cargo que aumenta la cuenta por pagar";
            else if (string.Equals(Naturaleza, "Abono", StringComparison.OrdinalIgnoreCase))
                ComprasLblEfecto.Text = "Abono a favor de la cuenta del proveedor";
            else
                ComprasLblEfecto.Text = "Seleccione el concepto del documento.";

            decimal Monto;
            ComprasLblEquivalente.Text = _TipoCambio.HasValue && ComprasFuncLeerDecimal(ComprasTxtMonto, 99999999.9999m, 4, out Monto)
                ? "Equivalente en GTQ: " + (Monto * _TipoCambio.Value).ToString("N4")
                : "El equivalente en GTQ requiere un importe válido y una cotización para la fecha de emisión.";
        }

        private void ComprasProcLimpiarFormulario(bool ConservarProveedor = false)
        {
            if (!ConservarProveedor)
            {
                ComprasCboProveedor.SelectedIndex = -1;
                ComprasCboProveedor.Text = string.Empty;
            }
            ComprasCboConcepto.SelectedIndex = -1;
            ComprasCboMoneda.SelectedIndex = -1;
            ComprasTxtSerie.Clear();
            ComprasTxtNumero.Clear();
            ComprasDtpFechaEmision.Value = DateTime.Today;
            ComprasDtpFechaVencimiento.Value = DateTime.Today;
            ComprasTxtMonto.Text = $"Q {0m:N2}";
            if (_CatalogosListos)
                ComprasProcMostrarEstado("Nuevo documento. Complete los campos obligatorios.");
            ComprasProcActualizarResumen();
        }

        private static bool ComprasFuncLeerDecimal(ComprasUsrTextBox Entrada, decimal Maximo, int Decimales, out decimal Valor)
        {
            return Entrada.ComprasFuncLeerMonto(out Valor)
                && Valor > 0 && Valor <= Maximo && decimal.Round(Valor, Decimales) == Valor;
        }

        private void ComprasProcGuardarDocumento()
        {
            if (_Guardando || !_CatalogosListos) return;
            if (ComprasFuncObtenerIdSeleccionado(ComprasCboProveedor) == 0) { ComprasProcMostrarAviso("Seleccione un proveedor."); ComprasCboProveedor.Focus(); return; }
            int IdConcepto = ComprasFuncObtenerIdSeleccionado(ComprasCboConcepto);
            if (IdConcepto == 0)
            {
                ComprasProcMostrarAviso("Seleccione un concepto de proveedor.");
                ComprasCboConcepto.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(ComprasTxtSerie.Text)) { ComprasProcMostrarAviso("Ingrese la serie."); ComprasTxtSerie.Focus(); return; }
            if (string.IsNullOrWhiteSpace(ComprasTxtNumero.Text)) { ComprasProcMostrarAviso("Ingrese el número del documento."); ComprasTxtNumero.Focus(); return; }
            long IdMoneda = ComprasFuncObtenerIdMoneda();
            if (IdMoneda == 0) { ComprasProcMostrarAviso("Seleccione una moneda."); ComprasCboMoneda.Focus(); return; }
            decimal Monto;
            if (!ComprasFuncLeerDecimal(ComprasTxtMonto, 99999999.9999m, 4, out Monto))
            { ComprasProcMostrarAviso("Ingrese un importe positivo de hasta 99,999,999.9999 y cuatro decimales."); ComprasTxtMonto.Focus(); return; }
            if (!_TipoCambio.HasValue || _TipoCambio.Value <= 0 || _TipoCambio.Value > 999999.999999m)
            { ComprasProcMostrarAviso("No hay un tipo de cambio válido hacia GTQ para la moneda y la fecha de emisión.", PermitirReintento: true); return; }
            if (ComprasDtpFechaVencimiento.Value.Date < ComprasDtpFechaEmision.Value.Date) { ComprasProcMostrarAviso("El vencimiento no puede ser anterior a la emisión."); ComprasDtpFechaVencimiento.Focus(); return; }
            try
            {
                _Guardando = true;
                ComprasBtnGuardar.Enabled = false;
                _ModeloNotaProveedor.ComprasMetAgregar(ComprasFuncObtenerIdSeleccionado(ComprasCboProveedor), IdConcepto, ComprasTxtSerie.Text,
                    ComprasTxtNumero.Text, ComprasDtpFechaEmision.Value, ComprasDtpFechaVencimiento.Value,
                    IdMoneda, _TipoCambio.Value, Monto);
            }
            catch (Exception Error) { ComprasProcMostrarError("No se pudo guardar el documento.", Error, PermitirReintento: true); return; }
            finally { _Guardando = false; ComprasBtnGuardar.Enabled = _CatalogosListos; }
            ComprasProcLimpiarFormulario(ConservarProveedor: true);
            MessageBox.Show(this, "El documento se registró en la cuenta por pagar con estado Pendiente.", "Movimientos de proveedores", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComprasProcMostrarEstado(string TextoDescriptivo, bool PermitirReintento = false)
        {
            ComprasLblEstado.Links.Clear();
            ComprasLblEstado.Text = TextoDescriptivo;
            if (PermitirReintento)
            {
                ComprasLblEstado.Text += " " + TextoReintento;
                ComprasLblEstado.Links.Add(TextoDescriptivo.Length + 1, TextoReintento.Length);
            }
        }

        private void ComprasProcMostrarAviso(string TextoDescriptivo, bool PermitirReintento = false)
        {
            ComprasProcMostrarEstado(TextoDescriptivo, PermitirReintento);
            if (PermitirReintento) TextoDescriptivo += InstruccionReintento;
            MessageBox.Show(this, TextoDescriptivo, "Movimientos de proveedores", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ComprasProcMostrarError(string Contexto, Exception Error, bool PermitirReintento = false)
        {
            string Detalle = Error.Message;
            var ErrorOdbc = Error as OdbcException;
            if (ErrorOdbc != null)
                foreach (OdbcError Fallo in ErrorOdbc.Errors)
                    if (Fallo.NativeError == 1062) Detalle = "Ya existe un documento con ese concepto, serie y número.";
            if (string.IsNullOrWhiteSpace(Detalle))
                Detalle = ErrorOdbc != null ? "El controlador ODBC no devolvió detalles del error al procesar la operación."
                    : "La operación falló sin devolver un detalle adicional.";
            ComprasProcMostrarEstado(Contexto, PermitirReintento);
            string Mensaje = Contexto + "\n\n" + Detalle;
            if (PermitirReintento) Mensaje += InstruccionReintento;
            MessageBox.Show(this, Mensaje, "Movimientos de proveedores", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ComprasMetAbrirHistorial(object Remitente, EventArgs Evento)
        {
            using (var Historial = new FrmHistorialMovimientosProveedores())
            {
                Historial.ShowDialog(this);
            }
        }
    }
}



