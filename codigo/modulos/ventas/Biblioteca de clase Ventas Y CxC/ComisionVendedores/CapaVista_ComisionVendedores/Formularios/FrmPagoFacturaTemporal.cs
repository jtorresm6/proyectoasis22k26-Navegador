// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Valida la captura del abono y solicita su registro al controlador.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_ComisionVendedores.Contratos;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Validaciones;

namespace CapaVista_ComisionVendedores
{
    /// <summary>Registra el recibo del cliente y su aplicación a una factura.</summary>
    internal sealed partial class FrmPagoFacturaTemporal : Form
    {
        private readonly IControladorComision _Controlador;
        private readonly int _IdUsuario;
        private readonly int? _IdFacturaInicial;
        private readonly ErrorProvider VentasErpValidacion = new ErrorProvider();
        private List<ClsFacturaPagoVista> _Facturas;
        private List<ClsFormaPagoVentaVista> _FormasPago;

        public FrmPagoFacturaTemporal() : this(new CapaControlador_ComisionVendedores.Controladores.ClsControladorComision(), 0)
        {
        }

        public FrmPagoFacturaTemporal(IControladorComision Controlador, int IdUsuario, int? IdFacturaInicial = null)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException("Controlador");
            _IdUsuario = IdUsuario;
            _IdFacturaInicial = IdFacturaInicial;
            InitializeComponent();
            VentasErpValidacion.ContainerControl = this;
            VentasErpValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }



        private void VentasMetCargarCatalogos(object Sender, EventArgs Argumentos)
        {
            if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            try
            {
                _Facturas = _Controlador.VentasMetObtenerFacturasConSaldo();
                _FormasPago = _Controlador.VentasMetObtenerFormasPagoVenta();
                VentasCboFactura.DisplayMember = "Etiqueta";
                VentasCboFactura.ValueMember = "IdFactura";
                VentasCboFactura.DataSource = _Facturas;
                if (_IdFacturaInicial.HasValue && _Facturas.Any(Factura => Factura.IdFactura == _IdFacturaInicial.Value))
                    VentasCboFactura.SelectedValue = _IdFacturaInicial.Value;
                VentasCboFormaPago.DisplayMember = "Nombre";
                VentasCboFormaPago.ValueMember = "IdFormaPago";
                VentasCboFormaPago.DataSource = _FormasPago;
                if (_Facturas.Count == 0)
                    VentasLblCalculoAbono.Text = "No se encontraron facturas. Revise la conexión a la base de datos.";
                else if (_FormasPago.Count == 0)
                    VentasLblCalculoAbono.Text = "No hay métodos de pago configurados en tblFormaPagoVenta.";
                VentasMetActualizarResumen(null, EventArgs.Empty);
                VentasMetActualizarReferencia(null, EventArgs.Empty);
            }
            catch (Exception Error)
            {
                VentasBtnRegistrarAbono.Enabled = false;
                MessageBox.Show(this, Error.Message, "7009 – RegistrarAbonoFactura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetActualizarResumen(object Sender, EventArgs Argumentos)
        {
            ClsFacturaPagoVista Factura = VentasCboFactura.SelectedItem as ClsFacturaPagoVista;
            if (Factura == null)
            {
                VentasLblTotal.Text = "Total factura: —";
                VentasLblPagado.Text = "Pagado: —";
                VentasLblSaldo.Text = "Saldo: —";
                VentasBtnRegistrarAbono.Enabled = false;
                return;
            }
            string Simbolo = Factura.SimboloMoneda ?? "";
            VentasLblTotal.Text = "Total: " + Simbolo + " " + Factura.Total.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            VentasLblPagado.Text = "Pagado: " + Simbolo + " " + Factura.TotalPagado.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            VentasLblSaldo.Text = "Saldo: " + Simbolo + " " + Factura.Saldo.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            if (VentasNudMonto == null || VentasLblCalculoAbono == null) return;
            decimal Maximo = Math.Min(Factura.Saldo, 99999999.9999m);
            VentasNudMonto.Maximum = Math.Max(VentasNudMonto.Minimum, Maximo);
            if (VentasNudMonto.Value > VentasNudMonto.Maximum) VentasNudMonto.Value = VentasNudMonto.Maximum;
            if (VentasNudMonto.Value < VentasNudMonto.Minimum && VentasNudMonto.Maximum >= VentasNudMonto.Minimum)
                VentasNudMonto.Value = VentasNudMonto.Maximum;

            bool TieneFormaPago = _FormasPago != null && _FormasPago.Count > 0;
            bool FacturaPagable = String.Equals(Factura.Estado, "EMITIDA", StringComparison.OrdinalIgnoreCase) &&
                Factura.Saldo >= VentasNudMonto.Minimum && Factura.Total > 0m &&
                Factura.IdCliente > 0 && Factura.IdMoneda > 0;
            VentasBtnRegistrarAbono.Enabled = TieneFormaPago && FacturaPagable;
            if (!TieneFormaPago)
            {
                VentasLblCalculoAbono.Text = "No hay métodos de pago configurados en tblFormaPagoVenta.";
                return;
            }
            if (!String.Equals(Factura.Estado, "EMITIDA", StringComparison.OrdinalIgnoreCase))
            {
                VentasLblCalculoAbono.Text = "Factura " + Factura.IdFactura + " en estado " + Factura.Estado + ". Solo se aceptan abonos para facturas EMITIDAS.";
                return;
            }
            if (Factura.IdCliente <= 0 || Factura.IdMoneda <= 0)
            {
                VentasLblCalculoAbono.Text = "La factura " + Factura.IdFactura + " no tiene cliente o moneda vinculados; no se puede registrar el recibo.";
                return;
            }
            if (Factura.Total <= 0m || Factura.Saldo < VentasNudMonto.Minimum)
            {
                VentasLblCalculoAbono.Text = "La factura " + Factura.IdFactura + " no tiene saldo pendiente para recibir otro abono.";
                return;
            }

            if (Factura.Total <= 0m || VentasNudMonto.Value <= 0m) return;
            decimal PorcentajeAbono = VentasNudMonto.Value / Factura.Total * 100m;
            decimal ComisionAbono = ClsCalculoComision.VentasMetEstimarAbono(Factura, VentasNudMonto.Value);
            VentasLblCalculoAbono.Text = Factura.ComisionTotal <= 0m
                ? "Este abono representa " + PorcentajeAbono.ToString("0.00") + "% de la factura. No hay comisión asignada; el pago se guardará y podrá asignarse después."
                : "Este abono representa " + PorcentajeAbono.ToString("0.00") + "% de la factura. Comisión que se devengará para el vendedor con este abono: " +
                  Simbolo + " " + ComisionAbono.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ".";
        }

        private void VentasMetActualizarReferencia(object Sender, EventArgs Argumentos)
        {
            ClsFormaPagoVentaVista Forma = VentasCboFormaPago.SelectedItem as ClsFormaPagoVentaVista;
            bool Requiere = Forma != null && Forma.RequiereReferencia;
            VentasLblReferencia.Text = Requiere ? "Referencia (obligatoria)" : "Referencia (opcional)";
            if (!Requiere) VentasTxtReferencia.BackColor = SystemColors.Window;
        }

        private void VentasMetGuardarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                Control[] Campos = { VentasCboFactura, VentasCboFormaPago,
                    VentasNudMonto, VentasTxtReferencia };
                foreach (Control Campo in Campos)
                {
                    VentasErpValidacion.SetError(Campo, String.Empty);
                    Campo.BackColor = SystemColors.Window;
                }
                VentasLblCalculoAbono.ForeColor = Color.FromArgb(59, 29, 74);

                ClsFacturaPagoVista Factura = VentasCboFactura.SelectedItem as ClsFacturaPagoVista;
                ClsFormaPagoVentaVista Forma = VentasCboFormaPago.SelectedItem as ClsFormaPagoVentaVista;
                if (Factura == null)
                {
                    VentasMetMostrarErrorCampo(VentasCboFactura, "Seleccione la factura a la que se aplicará el abono.");
                    return;
                }
                if (!String.Equals(Factura.Estado, "EMITIDA", StringComparison.OrdinalIgnoreCase))
                {
                    VentasMetMostrarErrorCampo(VentasCboFactura, "Solo se pueden registrar abonos en facturas EMITIDAS.");
                    return;
                }
                if (Factura.Saldo < 0.01m)
                {
                    VentasMetMostrarErrorCampo(VentasCboFactura, "La factura seleccionada no tiene saldo pendiente.");
                    return;
                }
                if (Forma == null)
                {
                    VentasMetMostrarErrorCampo(VentasCboFormaPago, "Seleccione un método de pago.");
                    return;
                }
                if (VentasNudMonto.Value <= 0m || VentasNudMonto.Value > Factura.Saldo)
                {
                    VentasMetMostrarErrorCampo(VentasNudMonto, "Ingrese un monto mayor que cero y no superior al saldo de la factura.");
                    return;
                }
                string Referencia = VentasTxtReferencia.Text.Trim();
                if (Forma.RequiereReferencia && String.IsNullOrWhiteSpace(Referencia))
                {
                    VentasMetMostrarErrorCampo(VentasTxtReferencia, "El método de pago seleccionado requiere número de referencia.");
                    return;
                }

                string Simbolo = Factura.SimboloMoneda ?? String.Empty;
                string TextoConfirmacion = "Factura " + Factura.IdFactura + " · " + Simbolo + " " +
                    VentasNudMonto.Value.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + " · " + Forma.Nombre +
                    " · " + VentasDtpFecha.Value.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("es-GT")) +
                    ". ¿Desea registrar este abono?";
                if (FrmComisionesVendedoresVentas.VentasMetConfirmar(this,
                    "7009 – RegistrarAbonoFactura", TextoConfirmacion, "Sí, registrar") != DialogResult.Yes)
                    return;

                decimal ComisionDevengada = _Controlador.VentasMetRegistrarAbonoFactura(
                    Factura.IdFactura, VentasNudMonto.Value, Forma.IdFormaPago, VentasDtpFecha.Value.Date, Referencia, _IdUsuario);
                decimal PorcentajeAbono = VentasNudMonto.Value / Factura.Total * 100m;
                string Mensaje = "Abono registrado para la factura " + Factura.IdFactura + ": " + Simbolo + " " +
                    VentasNudMonto.Value.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + " (" + PorcentajeAbono.ToString("0.00") + "% del total)." +
                    Environment.NewLine + "Comisión devengada en este abono: " + Simbolo + " " +
                    ComisionDevengada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ".";
                FrmComisionesVendedoresVentas.VentasMetMostrarExito(this, "7009 – RegistrarAbonoFactura", Mensaje);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception Error)
            {
                VentasLblCalculoAbono.ForeColor = Color.Firebrick;
                VentasLblCalculoAbono.Text = Error.Message;
                MessageBox.Show(this, Error.Message, "7009 – RegistrarAbonoFactura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetMostrarErrorCampo(Control Campo, string Mensaje)
        {
            Campo.BackColor = Color.MistyRose;
            VentasErpValidacion.SetError(Campo, Mensaje);
            VentasLblCalculoAbono.ForeColor = Color.Firebrick;
            VentasLblCalculoAbono.Text = Mensaje;
            Campo.Focus();
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing) VentasErpValidacion.Dispose();
            base.Dispose(Disposing);
        }
    }
}
