using System;
using System.Data;
using System.Data.Odbc;
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
 *  Crear nuevas notas de débito y crédito para proveedores, registrando los movimientos en la cuenta por pagar.
 * ===================================================================
*/
namespace CapaVista_MovimientosProveedores
{
    public partial class FrmMovimientosProveedores : Form
    {

        private readonly ClsModeloNotaProveedor _ModeloNotaProveedor = new ClsModeloNotaProveedor();
        private bool _Guardando;
        private bool _CatalogosListos;
        private int _IdConceptoDebito;
        private int _IdConceptoCredito;

        public FrmMovimientosProveedores()
        {
            InitializeComponent();
            Load += (Remitente, Evento) => ComprasProcCargarCatalogos();
            ComprasRdbDebito.CheckedChanged += (Remitente, Evento) => ComprasProcActualizarResumen();
            ComprasRdbCredito.CheckedChanged += (Remitente, Evento) => ComprasProcActualizarResumen();
            ComprasTxtMonto.TextChanged += (Remitente, Evento) => ComprasProcActualizarResumen();
            ComprasTxtTipoCambio.TextChanged += (Remitente, Evento) => ComprasProcActualizarResumen();
            ComprasDtpFechaEmision.ValueChanged += (Remitente, Evento) => {
                if (ComprasDtpFechaVencimiento.Value.Date < ComprasDtpFechaEmision.Value.Date)
                    ComprasDtpFechaVencimiento.Value = ComprasDtpFechaEmision.Value.Date;
            };
            ComprasBtnGuardar.Click += (Remitente, Evento) => ComprasProcGuardarNota();
            ComprasBtnNueva.Click += (Remitente, Evento) => ComprasProcLimpiarFormulario();
            ComprasBtnActualizar.Click += (Remitente, Evento) => ComprasProcCargarCatalogos();
            ComprasProcLimpiarFormulario();
            ComprasBtnGuardar.Enabled = false;
        }

        private static void ComprasProcLlenarSelector(CapaVista_Componentes.Componentes.ComprasUsrComboBox Selector, DataTable Tabla, string TextoDescriptivo, string CampoValor)
        {
            Selector.DisplayMember = TextoDescriptivo;
            Selector.ValueMember = CampoValor;
            Selector.DataSource = Tabla;
            Selector.SelectedIndex = -1;
        }

        private void ComprasProcCargarCatalogos()
        {
            _CatalogosListos = false;
            ComprasBtnGuardar.Enabled = false;
            ComprasProcCargarTiposNota(null);
            try
            {
                ComprasProcLlenarSelector(ComprasCboProveedor, _ModeloNotaProveedor.ComprasMetObtenerProveedores(), "nombreProveedor", "idProveedor");
                ComprasProcCargarTiposNota(_ModeloNotaProveedor.ComprasMetObtenerConceptos());
                ComprasProcLlenarSelector(ComprasCboMoneda, _ModeloNotaProveedor.ComprasMetObtenerMonedas(), "nombreMoneda", "idMoneda");
                _CatalogosListos = ComprasCboProveedor.Items.Count > 0 && (_IdConceptoDebito > 0 || _IdConceptoCredito > 0) && ComprasCboMoneda.Items.Count > 0;
                ComprasBtnGuardar.Enabled = _CatalogosListos;
                ComprasLblEstado.Text = _CatalogosListos ? "Seleccione un proveedor y complete los datos de la nota." : "Faltan proveedores, conceptos de nota o monedas activas. Complete los catálogos y pulse Actualizar.";
            }
            catch (Exception Error) { ComprasProcMostrarError("No se pudieron cargar los catálogos. Revise la conexión y pulse Actualizar.", Error); }
            finally { ComprasProcActualizarResumen(); }
        }

        private int ComprasFuncObtenerIdSeleccionado(CapaVista_Componentes.Componentes.ComprasUsrComboBox Selector)
        {
            return Selector.SelectedItem is DataRowView ? Convert.ToInt32(Selector.SelectedValue) : 0;
        }

        private void ComprasProcCargarTiposNota(DataTable Conceptos)
        {
            // Los radios comparten un contenedor, por lo que solo se puede elegir uno.
            // Débito usa el concepto Cargo y crédito el concepto Abono del catálogo;
            // sus identificadores reales se envían al guardar, sin depender de un combo
            // ni de IDs fijos. La elección siempre está habilitada, aunque falle
            // la carga del catálogo; la existencia del concepto se valida al guardar.
            // Al recargar se limpia la elección para no reutilizar IDs anteriores.
            ComprasRdbDebito.Checked = false;
            ComprasRdbCredito.Checked = false;
            _IdConceptoDebito = 0;
            _IdConceptoCredito = 0;
            if (Conceptos != null)
            {
                foreach (DataRow Concepto in Conceptos.Rows)
                {
                    string Naturaleza = Convert.ToString(Concepto["naturalezaConceptoProveedor"]).Trim();
                    int IdConcepto = Convert.ToInt32(Concepto["idConceptoProveedor"]);
                    if (string.Equals(Naturaleza, "Cargo", StringComparison.OrdinalIgnoreCase) && _IdConceptoDebito == 0) _IdConceptoDebito = IdConcepto;
                    if (string.Equals(Naturaleza, "Abono", StringComparison.OrdinalIgnoreCase) && _IdConceptoCredito == 0) _IdConceptoCredito = IdConcepto;
                }
            }
        }

        private int ComprasFuncObtenerIdConceptoSeleccionado()
        {
            if (ComprasRdbDebito.Checked) return _IdConceptoDebito;
            if (ComprasRdbCredito.Checked) return _IdConceptoCredito;
            return 0;
        }

        private void ComprasProcActualizarResumen()
        {
            string Naturaleza = ComprasRdbDebito.Checked ? "Cargo" : ComprasRdbCredito.Checked ? "Abono" : "";
            ComprasLblEfecto.Text = Naturaleza == "Cargo" ? "DÉBITO · Cargo que aumenta la cuenta por pagar" :
                Naturaleza == "Abono" ? "CRÉDITO · Abono a favor de la cuenta del proveedor" : "Seleccione nota de débito o nota de crédito";
            decimal Monto, TipoCambio;
            ComprasLblEquivalente.Text = ComprasFuncLeerDecimal(ComprasTxtMonto, 99999999.9999m, 4, out Monto)
                && ComprasFuncLeerDecimal(ComprasTxtTipoCambio, 999999.999999m, 6, out TipoCambio)
                ? "Equivalente en moneda base: " + (Monto * TipoCambio).ToString("N4")
                : "Ingrese un importe y un tipo de cambio válidos para calcular el equivalente.";
        }

        private void ComprasProcLimpiarFormulario()
        {
            ComprasRdbDebito.Checked = false;
            ComprasRdbCredito.Checked = false;
            ComprasCboMoneda.SelectedIndex = -1;
            ComprasTxtSerie.Clear();
            ComprasTxtNumero.Clear();
            ComprasDtpFechaEmision.Value = DateTime.Today;
            ComprasDtpFechaVencimiento.Value = DateTime.Today;
            ComprasTxtMonto.Text = "0";
            ComprasTxtTipoCambio.Text = "1";
            ComprasLblEstado.Text = "Nueva nota. Complete los campos obligatorios.";
            ComprasProcActualizarResumen();
        }

        private static bool ComprasFuncLeerDecimal(TextBox Entrada, decimal Maximo, int Decimales, out decimal Valor)
        {
            var Estilo = System.Globalization.NumberStyles.AllowDecimalPoint
                | System.Globalization.NumberStyles.AllowLeadingSign
                | System.Globalization.NumberStyles.AllowLeadingWhite
                | System.Globalization.NumberStyles.AllowTrailingWhite;
            return decimal.TryParse(Entrada.Text, Estilo, System.Globalization.CultureInfo.CurrentCulture, out Valor)
                && Valor > 0 && Valor <= Maximo && decimal.Round(Valor, Decimales) == Valor;
        }

        private void ComprasProcGuardarNota()
        {
            if (_Guardando || !_CatalogosListos) return;
            if (ComprasFuncObtenerIdSeleccionado(ComprasCboProveedor) == 0) { ComprasProcMostrarAviso("Seleccione un proveedor."); ComprasCboProveedor.Focus(); return; }
            int IdConcepto = ComprasFuncObtenerIdConceptoSeleccionado();
            if (!ComprasRdbDebito.Checked && !ComprasRdbCredito.Checked)
            {
                ComprasProcMostrarAviso("Seleccione nota de débito o nota de crédito.");
                ComprasRdbDebito.Focus();
                return;
            }
            if (IdConcepto == 0)
            {
                ComprasProcMostrarAviso("El tipo de nota seleccionado no tiene un concepto disponible. Revise el catálogo de conceptos de proveedor y pulse Actualizar.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ComprasTxtSerie.Text)) { ComprasProcMostrarAviso("Ingrese la serie."); ComprasTxtSerie.Focus(); return; }
            if (string.IsNullOrWhiteSpace(ComprasTxtNumero.Text)) { ComprasProcMostrarAviso("Ingrese el número del documento."); ComprasTxtNumero.Focus(); return; }
            if (ComprasFuncObtenerIdSeleccionado(ComprasCboMoneda) == 0) { ComprasProcMostrarAviso("Seleccione una moneda."); ComprasCboMoneda.Focus(); return; }
            decimal Monto, TipoCambio;
            if (!ComprasFuncLeerDecimal(ComprasTxtMonto, 99999999.9999m, 4, out Monto))
            { ComprasProcMostrarAviso("Ingrese un importe positivo de hasta 99,999,999.9999 y cuatro decimales, sin separadores de miles."); ComprasTxtMonto.Focus(); return; }
            if (!ComprasFuncLeerDecimal(ComprasTxtTipoCambio, 999999.999999m, 6, out TipoCambio))
            { ComprasProcMostrarAviso("Ingrese un tipo de cambio positivo de hasta 999,999.999999 y seis decimales, sin separadores de miles."); ComprasTxtTipoCambio.Focus(); return; }
            if (ComprasDtpFechaVencimiento.Value.Date < ComprasDtpFechaEmision.Value.Date) { ComprasProcMostrarAviso("El vencimiento no puede ser anterior a la emisión."); ComprasDtpFechaVencimiento.Focus(); return; }
            try
            {
                _Guardando = true;
                ComprasBtnGuardar.Enabled = false;
                _ModeloNotaProveedor.ComprasMetAgregar(ComprasFuncObtenerIdSeleccionado(ComprasCboProveedor), IdConcepto, ComprasTxtSerie.Text,
                    ComprasTxtNumero.Text, ComprasDtpFechaEmision.Value, ComprasDtpFechaVencimiento.Value,
                    ComprasFuncObtenerIdSeleccionado(ComprasCboMoneda), TipoCambio, Monto);
            }
            catch (Exception Error) { ComprasProcMostrarError("No se pudo guardar la nota.", Error); return; }
            finally { _Guardando = false; ComprasBtnGuardar.Enabled = _CatalogosListos; }
            ComprasProcLimpiarFormulario();
            MessageBox.Show(this, "La nota se registró en la cuenta por pagar con estado Pendiente.", "Movimientos de proveedores", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComprasProcMostrarAviso(string TextoDescriptivo)
        {
            ComprasLblEstado.Text = TextoDescriptivo;
            MessageBox.Show(this, TextoDescriptivo, "Movimientos de proveedores", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ComprasProcMostrarError(string Contexto, Exception Error)
        {
            string Detalle = Error.Message;
            var ErrorOdbc = Error as OdbcException;
            if (ErrorOdbc != null)
                foreach (OdbcError Fallo in ErrorOdbc.Errors)
                    if (Fallo.NativeError == 1062) Detalle = "Ya existe una nota con ese proveedor, tipo, serie y número.";
            if (string.IsNullOrWhiteSpace(Detalle))
                Detalle = ErrorOdbc != null ? "El controlador ODBC no devolvió detalles del error al procesar la operación."
                    : "La operación falló sin devolver un detalle adicional.";
            ComprasLblEstado.Text = Contexto;
            MessageBox.Show(this, Contexto + "\n\n" + Detalle, "Movimientos de proveedores", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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



