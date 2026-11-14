using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_ComisionVendedores.Contratos;
using CapaControlador_ComisionVendedores.DTO;

namespace ComisionVendedoresVista
{
    /// <summary>Registra el recibo del cliente y su aplicación a una factura.</summary>
    internal sealed class FrmPagoFacturaTemporal : Form
    {
        private readonly IControladorComision _Controlador;
        private readonly int _IdUsuario;
        private readonly int? _IdFacturaInicial;
        private readonly ComboBox VentasCboFactura;
        private readonly ComboBox VentasCboFormaPago;
        private readonly NumericUpDown VentasNudMonto;
        private readonly DateTimePicker VentasDtpFecha;
        private readonly TextBox VentasTxtReferencia;
        private readonly Label VentasLblTotal;
        private readonly Label VentasLblPagado;
        private readonly Label VentasLblSaldo;
        private readonly Label VentasLblReferencia;
        private readonly Label VentasLblObligatorioReferencia;
        private readonly ErrorProvider _ErrorProveedor = new ErrorProvider();
        private readonly Dictionary<Control, Panel> _ContenedoresValidacion = new Dictionary<Control, Panel>();
        private readonly Label VentasLblCalculo;
        private readonly Button VentasBtnRegistrarAbono;
        private List<ClsFacturaPagoVista> _Facturas;
        private List<ClsFormaPagoVentaVista> _FormasPago;

        public FrmPagoFacturaTemporal(IControladorComision Controlador, int IdUsuario, int? IdFacturaInicial = null)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException("Controlador");
            _IdUsuario = IdUsuario;
            _IdFacturaInicial = IdFacturaInicial;
            Name = "FrmPagoFacturaTemporal";
            Text = "7009 – RegistrarAbonoFactura";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = true;
            Icon = Properties.ClsRecursosComision.IconoVentas;
            ClientSize = new Size(700, 446);
            Font = new Font("Segoe UI", 9.5F);
            BackColor = Color.FromArgb(248, 245, 249);
            _ErrorProveedor.ContainerControl = this;
            _ErrorProveedor.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            Controls.Add(VentasMetCrearEtiqueta("VentasLblFacturaAbono", "Factura", 22, 18, 180, 24));
            VentasMetAgregarAsteriscoObligatorio("VentasLblObligatorioFacturaAbono", 68, 18);
            VentasCboFactura = new ComboBox { Name = "VentasCboFactura", DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(22, 44), Size = new Size(656, 31), Font = Font };
            VentasCboFactura.SelectedIndexChanged += VentasMetActualizarResumen;
            Controls.Add(VentasCboFactura);

            VentasLblTotal = VentasMetCrearEtiqueta("VentasLblTotalFactura", "Total factura: —", 22, 88, 205, 25);
            VentasLblPagado = VentasMetCrearEtiqueta("VentasLblTotalPagado", "Pagado: —", 248, 88, 185, 25);
            VentasLblSaldo = VentasMetCrearEtiqueta("VentasLblSaldoFactura", "Saldo: —", 460, 88, 205, 25);
            Controls.Add(VentasLblTotal);
            Controls.Add(VentasLblPagado);
            Controls.Add(VentasLblSaldo);

            Controls.Add(VentasMetCrearEtiqueta("VentasLblMontoAbono", "Monto de este abono", 22, 132, 205, 24));
            VentasMetAgregarAsteriscoObligatorio("VentasLblObligatorioMontoAbono", 157, 132);
            VentasNudMonto = new NumericUpDown
            {
                Name = "VentasNudMonto",
                Location = new Point(22, 158), Size = new Size(205, 31),
                DecimalPlaces = 2, Minimum = 0.01m, Maximum = 99999999.99m,
                Increment = 0.01m, ThousandsSeparator = true, Font = Font
            };
            VentasNudMonto.ValueChanged += VentasMetActualizarResumen;
            Controls.Add(VentasNudMonto);

            Controls.Add(VentasMetCrearEtiqueta("VentasLblFormaPago", "Método de pago", 248, 132, 200, 24));
            VentasMetAgregarAsteriscoObligatorio("VentasLblObligatorioFormaPago", 347, 132);
            VentasCboFormaPago = new ComboBox { Name = "VentasCboFormaPago", DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(248, 158), Size = new Size(205, 31), Font = Font };
            VentasCboFormaPago.SelectedIndexChanged += VentasMetActualizarReferencia;
            Controls.Add(VentasCboFormaPago);

            Controls.Add(VentasMetCrearEtiqueta("VentasLblFechaAbono", "Fecha de pago", 474, 132, 190, 24));
            VentasMetAgregarAsteriscoObligatorio("VentasLblObligatorioFechaAbono", 568, 132);
            VentasDtpFecha = new DateTimePicker { Name = "VentasDtpFecha", Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Location = new Point(474, 158), Size = new Size(204, 31), Font = Font, Value = DateTime.Today };
            Controls.Add(VentasDtpFecha);

            VentasLblReferencia = VentasMetCrearEtiqueta("VentasLblReferencia", "Referencia (opcional)", 22, 208, 220, 24);
            VentasLblObligatorioReferencia = new Label
            {
                Name = "VentasLblObligatorioReferencia", Text = String.Empty,
                Location = new Point(22, 208), Size = new Size(12, 24),
                Font = new Font("Tahoma", 9F, FontStyle.Bold), ForeColor = Color.Firebrick,
                TextAlign = ContentAlignment.MiddleLeft
            };
            VentasTxtReferencia = new TextBox { Name = "VentasTxtReferencia", Location = new Point(22, 234), Size = new Size(431, 31), Font = Font, MaxLength = 255 };
            Controls.Add(VentasLblReferencia);
            Controls.Add(VentasLblObligatorioReferencia);
            Controls.Add(VentasTxtReferencia);

            VentasLblCalculo = new Label
            {
                Name = "VentasLblCalculoAbono",
                AutoSize = false, Location = new Point(22, 278), Size = new Size(656, 58),
                ForeColor = Color.FromArgb(59, 29, 74), TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(VentasLblCalculo);

            VentasBtnRegistrarAbono = new Button
            {
                Name = "VentasBtnRegistrarAbono",
                Text = "Realizar pago", Location = new Point(498, 343), Size = new Size(76, 78),
                BackColor = Color.FromArgb(59, 29, 74), ForeColor = Color.FromArgb(248, 245, 249), FlatStyle = FlatStyle.Flat
            };
            FrmComisionVendedores.VentasMetAplicarBotonEstandar(VentasBtnRegistrarAbono,
                "btn_ingresar", "Realizar pago", new Size(76, 78));
            VentasBtnRegistrarAbono.Click += VentasMetGuardarClick;
            Controls.Add(VentasBtnRegistrarAbono);
            Button VentasBtnCancelarAbono = new Button { Name = "VentasBtnCancelarAbono", Text = "Cancelar", Location = new Point(594, 343), Size = new Size(76, 78), DialogResult = DialogResult.Cancel };
            FrmComisionVendedores.VentasMetAplicarBotonEstandar(VentasBtnCancelarAbono,
                "btn_cancelar", "Cancelar", new Size(76, 78));
            Controls.Add(VentasBtnCancelarAbono);
            AcceptButton = VentasBtnRegistrarAbono;
            CancelButton = VentasBtnCancelarAbono;
            Load += VentasMetCargarCatalogos;
        }

        private static Label VentasMetCrearEtiqueta(string NombreControl, string Texto, int X, int Y, int Ancho, int Alto)
        {
            return new Label
            {
                Name = NombreControl,
                Text = Texto, Location = new Point(X, Y), Size = new Size(Ancho, Alto),
                Font = new Font("Tahoma", 9F), ForeColor = Color.FromArgb(59, 29, 74), TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private void VentasMetAgregarAsteriscoObligatorio(string NombreControl, int X, int Y)
        {
            Controls.Add(new Label
            {
                Name = NombreControl, Text = "*", Location = new Point(X, Y), Size = new Size(12, 24),
                Font = new Font("Tahoma", 9F, FontStyle.Bold), ForeColor = Color.Firebrick,
                TextAlign = ContentAlignment.MiddleLeft
            });
        }

        private void VentasMetCargarCatalogos(object Sender, EventArgs Argumentos)
        {
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
                    VentasLblCalculo.Text = "No se encontraron facturas. Revise la conexión a la base de datos.";
                else if (_FormasPago.Count == 0)
                    VentasLblCalculo.Text = "No hay métodos de pago configurados en tblFormaPagoVenta.";
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
            if (VentasNudMonto == null || VentasLblCalculo == null) return;
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
                VentasLblCalculo.Text = "No hay métodos de pago configurados en tblFormaPagoVenta.";
                return;
            }
            if (!String.Equals(Factura.Estado, "EMITIDA", StringComparison.OrdinalIgnoreCase))
            {
                VentasLblCalculo.Text = "Factura " + Factura.IdFactura + " en estado " + Factura.Estado + ". Solo se aceptan abonos para facturas EMITIDAS.";
                return;
            }
            if (Factura.IdCliente <= 0 || Factura.IdMoneda <= 0)
            {
                VentasLblCalculo.Text = "La factura " + Factura.IdFactura + " no tiene cliente o moneda vinculados; no se puede registrar el recibo.";
                return;
            }
            if (Factura.Total <= 0m || Factura.Saldo < VentasNudMonto.Minimum)
            {
                VentasLblCalculo.Text = "La factura " + Factura.IdFactura + " no tiene saldo pendiente para recibir otro abono.";
                return;
            }

            if (Factura.Total <= 0m || VentasNudMonto.Value <= 0m) return;
            decimal PorcentajeAbono = VentasNudMonto.Value / Factura.Total * 100m;
            decimal ProporcionPagadaFinal = Math.Min(1m, (Factura.TotalPagado + VentasNudMonto.Value) / Factura.Total);
            decimal ComisionObjetivo = Math.Round(Factura.ComisionTotal * ProporcionPagadaFinal, 4, MidpointRounding.AwayFromZero);
            decimal ComisionAbono = Math.Max(0m, ComisionObjetivo - Factura.ComisionDevengada);
            VentasLblCalculo.Text = Factura.ComisionTotal <= 0m
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
                    _ErrorProveedor.SetError(Campo, String.Empty);
                    Campo.BackColor = SystemColors.Window;
                    Panel Contenedor;
                    if (_ContenedoresValidacion.TryGetValue(Campo, out Contenedor))
                        Contenedor.BackColor = (Color)Contenedor.Tag;
                }
                VentasLblCalculo.ForeColor = Color.FromArgb(59, 29, 74);

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
                if (FrmComisionVendedores.VentasMetConfirmar(this,
                    "7009 – RegistrarAbonoFactura", TextoConfirmacion, "Sí, registrar") != DialogResult.Yes)
                    return;

                decimal ComisionDevengada = _Controlador.VentasMetRegistrarAbonoFactura(
                    Factura.IdFactura, VentasNudMonto.Value, Forma.IdFormaPago, VentasDtpFecha.Value.Date, Referencia, _IdUsuario);
                decimal PorcentajeAbono = VentasNudMonto.Value / Factura.Total * 100m;
                string Mensaje = "Abono registrado para la factura " + Factura.IdFactura + ": " + Simbolo + " " +
                    VentasNudMonto.Value.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + " (" + PorcentajeAbono.ToString("0.00") + "% del total)." +
                    Environment.NewLine + "Comisión devengada en este abono: " + Simbolo + " " +
                    ComisionDevengada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ".";
                FrmComisionVendedores.VentasMetMostrarExito(this, "7009 – RegistrarAbonoFactura", Mensaje);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception Error)
            {
                VentasLblCalculo.ForeColor = Color.Firebrick;
                VentasLblCalculo.Text = Error.Message;
                MessageBox.Show(this, Error.Message, "7009 – RegistrarAbonoFactura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetEnvolverControl(Control Campo, string NombreContenedor)
        {
            Control Padre = Campo.Parent;
            if (Padre == null || _ContenedoresValidacion.ContainsKey(Campo)) return;

            int Indice = Padre.Controls.GetChildIndex(Campo);
            Color ColorOriginal = Padre.BackColor;
            Panel Contenedor = new Panel
            {
                Name = NombreContenedor, Location = Campo.Location, Size = Campo.Size,
                Anchor = Campo.Anchor, BackColor = ColorOriginal, Padding = new Padding(2),
                TabStop = false, TabIndex = Campo.TabIndex, Tag = ColorOriginal
            };
            Padre.Controls.Remove(Campo);
            Contenedor.Controls.Add(Campo);
            Campo.Dock = DockStyle.Fill;
            Campo.Margin = Padding.Empty;
            Campo.TabIndex = 0;
            Padre.Controls.Add(Contenedor);
            Padre.Controls.SetChildIndex(Contenedor, Indice);
            _ContenedoresValidacion.Add(Campo, Contenedor);
        }
        private void VentasMetMostrarErrorCampo(Control Campo, string Mensaje)
        {
            Campo.BackColor = Color.MistyRose;
            Panel Contenedor;
            if (_ContenedoresValidacion.TryGetValue(Campo, out Contenedor))
                Contenedor.BackColor = Color.Firebrick;
            _ErrorProveedor.SetError(Campo, Mensaje);
            VentasLblCalculo.ForeColor = Color.Firebrick;
            VentasLblCalculo.Text = Mensaje;
            Campo.Focus();
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing) _ErrorProveedor.Dispose();
            base.Dispose(Disposing);
        }
    }
}
