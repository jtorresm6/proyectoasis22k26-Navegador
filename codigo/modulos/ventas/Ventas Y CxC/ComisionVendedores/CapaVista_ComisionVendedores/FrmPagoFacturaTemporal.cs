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
        private readonly ComboBox ComisionVendedoresCboFactura;
        private readonly ComboBox ComisionVendedoresCboFormaPago;
        private readonly NumericUpDown ComisionVendedoresNudMonto;
        private readonly DateTimePicker ComisionVendedoresDtpFecha;
        private readonly TextBox ComisionVendedoresTxtReferencia;
        private readonly Label ComisionVendedoresLblTotal;
        private readonly Label ComisionVendedoresLblPagado;
        private readonly Label ComisionVendedoresLblSaldo;
        private readonly Label ComisionVendedoresLblReferencia;
        private readonly Label ComisionVendedoresLblCalculo;
        private readonly Button ComisionVendedoresBtnRegistrarAbono;
        private List<ClsFacturaPagoVista> _Facturas;
        private List<ClsFormaPagoVentaVista> _FormasPago;

        public FrmPagoFacturaTemporal(IControladorComision Controlador, int IdUsuario)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException("Controlador");
            _IdUsuario = IdUsuario;
            Name = "FrmPagoFacturaTemporal";
            Text = "7009 – RegistrarAbonoFactura";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = true;
            Icon = Properties.ClsRecursosComision.IconoVentas;
            ClientSize = new Size(700, 410);
            Font = new Font("Segoe UI", 9.5F);
            BackColor = Color.FromArgb(248, 245, 249);

            Controls.Add(ComisionVendedoresMetCrearEtiqueta("ComisionVendedoresLblFacturaAbono", "Factura", 22, 18, 180, 24));
            ComisionVendedoresCboFactura = new ComboBox { Name = "ComisionVendedoresCboFactura", DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(22, 44), Size = new Size(656, 31), Font = Font };
            ComisionVendedoresCboFactura.SelectedIndexChanged += ComisionVendedoresMetActualizarResumen;
            Controls.Add(ComisionVendedoresCboFactura);

            ComisionVendedoresLblTotal = ComisionVendedoresMetCrearEtiqueta("ComisionVendedoresLblTotalFactura", "Total factura: —", 22, 88, 205, 25);
            ComisionVendedoresLblPagado = ComisionVendedoresMetCrearEtiqueta("ComisionVendedoresLblTotalPagado", "Pagado: —", 248, 88, 185, 25);
            ComisionVendedoresLblSaldo = ComisionVendedoresMetCrearEtiqueta("ComisionVendedoresLblSaldoFactura", "Saldo: —", 460, 88, 205, 25);
            Controls.Add(ComisionVendedoresLblTotal);
            Controls.Add(ComisionVendedoresLblPagado);
            Controls.Add(ComisionVendedoresLblSaldo);

            Controls.Add(ComisionVendedoresMetCrearEtiqueta("ComisionVendedoresLblMontoAbono", "Monto de este abono", 22, 132, 205, 24));
            ComisionVendedoresNudMonto = new NumericUpDown
            {
                Name = "ComisionVendedoresNudMonto",
                Location = new Point(22, 158), Size = new Size(205, 31),
                DecimalPlaces = 2, Minimum = 0.01m, Maximum = 99999999.99m,
                Increment = 0.01m, ThousandsSeparator = true, Font = Font
            };
            ComisionVendedoresNudMonto.ValueChanged += ComisionVendedoresMetActualizarResumen;
            Controls.Add(ComisionVendedoresNudMonto);

            Controls.Add(ComisionVendedoresMetCrearEtiqueta("ComisionVendedoresLblFormaPago", "Método de pago", 248, 132, 200, 24));
            ComisionVendedoresCboFormaPago = new ComboBox { Name = "ComisionVendedoresCboFormaPago", DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(248, 158), Size = new Size(205, 31), Font = Font };
            ComisionVendedoresCboFormaPago.SelectedIndexChanged += ComisionVendedoresMetActualizarReferencia;
            Controls.Add(ComisionVendedoresCboFormaPago);

            Controls.Add(ComisionVendedoresMetCrearEtiqueta("ComisionVendedoresLblFechaAbono", "Fecha de pago", 474, 132, 190, 24));
            ComisionVendedoresDtpFecha = new DateTimePicker { Name = "ComisionVendedoresDtpFecha", Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Location = new Point(474, 158), Size = new Size(204, 31), Font = Font, Value = DateTime.Today };
            Controls.Add(ComisionVendedoresDtpFecha);

            ComisionVendedoresLblReferencia = ComisionVendedoresMetCrearEtiqueta("ComisionVendedoresLblReferencia", "Referencia (si aplica)", 22, 208, 220, 24);
            ComisionVendedoresTxtReferencia = new TextBox { Name = "ComisionVendedoresTxtReferencia", Location = new Point(22, 234), Size = new Size(431, 31), Font = Font, MaxLength = 255 };
            Controls.Add(ComisionVendedoresLblReferencia);
            Controls.Add(ComisionVendedoresTxtReferencia);

            ComisionVendedoresLblCalculo = new Label
            {
                Name = "ComisionVendedoresLblCalculoAbono",
                AutoSize = false, Location = new Point(22, 278), Size = new Size(656, 58),
                ForeColor = Color.FromArgb(59, 29, 74), TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(ComisionVendedoresLblCalculo);

            ComisionVendedoresBtnRegistrarAbono = new Button
            {
                Name = "ComisionVendedoresBtnRegistrarAbono",
                Text = "Registrar abono", Location = new Point(464, 354), Size = new Size(145, 36),
                BackColor = Color.FromArgb(59, 29, 74), ForeColor = Color.White, FlatStyle = FlatStyle.Flat
            };
            ComisionVendedoresBtnRegistrarAbono.FlatAppearance.BorderSize = 0;
            ComisionVendedoresBtnRegistrarAbono.Click += ComisionVendedoresMetGuardarClick;
            Controls.Add(ComisionVendedoresBtnRegistrarAbono);
            Button ComisionVendedoresBtnCancelarAbono = new Button { Name = "ComisionVendedoresBtnCancelarAbono", Text = "Cancelar", Location = new Point(615, 354), Size = new Size(63, 36), DialogResult = DialogResult.Cancel };
            Controls.Add(ComisionVendedoresBtnCancelarAbono);
            AcceptButton = ComisionVendedoresBtnRegistrarAbono;
            CancelButton = ComisionVendedoresBtnCancelarAbono;
            Load += ComisionVendedoresMetCargarCatalogos;
        }

        private static Label ComisionVendedoresMetCrearEtiqueta(string NombreControl, string Texto, int X, int Y, int Ancho, int Alto)
        {
            return new Label
            {
                Name = NombreControl,
                Text = Texto, Location = new Point(X, Y), Size = new Size(Ancho, Alto),
                ForeColor = Color.FromArgb(59, 29, 74), TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private void ComisionVendedoresMetCargarCatalogos(object Sender, EventArgs Argumentos)
        {
            try
            {
                _Facturas = _Controlador.ComisionVendedoresMetObtenerFacturasConSaldo();
                _FormasPago = _Controlador.ComisionVendedoresMetObtenerFormasPagoVenta();
                ComisionVendedoresCboFactura.DisplayMember = "Etiqueta";
                ComisionVendedoresCboFactura.ValueMember = "IdFactura";
                ComisionVendedoresCboFactura.DataSource = _Facturas;
                ComisionVendedoresCboFormaPago.DisplayMember = "Nombre";
                ComisionVendedoresCboFormaPago.ValueMember = "IdFormaPago";
                ComisionVendedoresCboFormaPago.DataSource = _FormasPago;
                bool Habilitado = _Facturas.Count > 0 && _FormasPago.Count > 0;
                ComisionVendedoresBtnRegistrarAbono.Enabled = Habilitado;
                if (!Habilitado)
                    ComisionVendedoresLblCalculo.Text = _Facturas.Count == 0
                        ? "No hay facturas emitidas con saldo pendiente en CxC."
                        : "No hay métodos de pago configurados en tblFormaPagoVenta.";
                ComisionVendedoresMetActualizarResumen(null, EventArgs.Empty);
                ComisionVendedoresMetActualizarReferencia(null, EventArgs.Empty);
            }
            catch (Exception Error)
            {
                ComisionVendedoresBtnRegistrarAbono.Enabled = false;
                MessageBox.Show(this, Error.Message, "7009 – RegistrarAbonoFactura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetActualizarResumen(object Sender, EventArgs Argumentos)
        {
            ClsFacturaPagoVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaPagoVista;
            if (Factura == null)
            {
                ComisionVendedoresLblTotal.Text = "Total factura: —";
                ComisionVendedoresLblPagado.Text = "Pagado: —";
                ComisionVendedoresLblSaldo.Text = "Saldo: —";
                return;
            }
            string Simbolo = Factura.SimboloMoneda ?? "";
            ComisionVendedoresLblTotal.Text = "Total: " + Simbolo + " " + Factura.Total.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            ComisionVendedoresLblPagado.Text = "Pagado: " + Simbolo + " " + Factura.TotalPagado.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            ComisionVendedoresLblSaldo.Text = "Saldo: " + Simbolo + " " + Factura.Saldo.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            if (ComisionVendedoresNudMonto == null || ComisionVendedoresLblCalculo == null) return;
            decimal Maximo = Math.Min(Factura.Saldo, 99999999.9999m);
            ComisionVendedoresNudMonto.Maximum = Math.Max(ComisionVendedoresNudMonto.Minimum, Maximo);
            if (ComisionVendedoresNudMonto.Value > ComisionVendedoresNudMonto.Maximum) ComisionVendedoresNudMonto.Value = ComisionVendedoresNudMonto.Maximum;
            if (ComisionVendedoresNudMonto.Value < ComisionVendedoresNudMonto.Minimum && ComisionVendedoresNudMonto.Maximum >= ComisionVendedoresNudMonto.Minimum)
                ComisionVendedoresNudMonto.Value = ComisionVendedoresNudMonto.Maximum;

            if (Factura.Total <= 0m || ComisionVendedoresNudMonto.Value <= 0m) return;
            decimal PorcentajeAbono = ComisionVendedoresNudMonto.Value / Factura.Total * 100m;
            decimal ProporcionPagadaFinal = Math.Min(1m, (Factura.TotalPagado + ComisionVendedoresNudMonto.Value) / Factura.Total);
            decimal ComisionObjetivo = Math.Round(Factura.ComisionTotal * ProporcionPagadaFinal, 4, MidpointRounding.AwayFromZero);
            decimal ComisionAbono = Math.Max(0m, ComisionObjetivo - Factura.ComisionDevengada);
            ComisionVendedoresLblCalculo.Text = Factura.ComisionTotal <= 0m
                ? "Este abono representa " + PorcentajeAbono.ToString("0.00") + "% de la factura. No hay comisión asignada; el pago se guardará y podrá asignarse después."
                : "Este abono representa " + PorcentajeAbono.ToString("0.00") + "% de la factura. Comisión que se devengará para el vendedor con este abono: " +
                  Simbolo + " " + ComisionAbono.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ".";
        }

        private void ComisionVendedoresMetActualizarReferencia(object Sender, EventArgs Argumentos)
        {
            ClsFormaPagoVentaVista Forma = ComisionVendedoresCboFormaPago.SelectedItem as ClsFormaPagoVentaVista;
            bool Requiere = Forma != null && Forma.RequiereReferencia;
            ComisionVendedoresLblReferencia.Text = Requiere ? "Referencia (obligatoria)" : "Referencia (opcional)";
            if (!Requiere) ComisionVendedoresTxtReferencia.BackColor = SystemColors.Window;
        }

        private void ComisionVendedoresMetGuardarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ClsFacturaPagoVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaPagoVista;
                ClsFormaPagoVentaVista Forma = ComisionVendedoresCboFormaPago.SelectedItem as ClsFormaPagoVentaVista;
                if (Factura == null) throw new InvalidOperationException("Seleccione la factura a la que se aplicará el abono.");
                if (Forma == null) throw new InvalidOperationException("Seleccione un método de pago.");
                if (ComisionVendedoresNudMonto.Value <= 0m || ComisionVendedoresNudMonto.Value > Factura.Saldo)
                    throw new InvalidOperationException("Ingrese un monto mayor que cero y no superior al saldo de la factura.");
                string Referencia = ComisionVendedoresTxtReferencia.Text.Trim();
                if (Forma.RequiereReferencia && String.IsNullOrWhiteSpace(Referencia))
                {
                    ComisionVendedoresTxtReferencia.BackColor = Color.MistyRose;
                    throw new InvalidOperationException("El método de pago seleccionado requiere número de referencia.");
                }
                decimal ComisionDevengada = _Controlador.ComisionVendedoresMetRegistrarAbonoFactura(
                    Factura.IdFactura, ComisionVendedoresNudMonto.Value, Forma.IdFormaPago, ComisionVendedoresDtpFecha.Value.Date, Referencia, _IdUsuario);
                decimal PorcentajeAbono = ComisionVendedoresNudMonto.Value / Factura.Total * 100m;
                string Simbolo = Factura.SimboloMoneda ?? "";
                string Mensaje = "Abono registrado para la factura " + Factura.IdFactura + ": " + Simbolo + " " +
                    ComisionVendedoresNudMonto.Value.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + " (" + PorcentajeAbono.ToString("0.00") + "% del total)." +
                    Environment.NewLine + "Comisión devengada en este abono: " + Simbolo + " " +
                    ComisionDevengada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ".";
                MessageBox.Show(this, Mensaje, "7009 – RegistrarAbonoFactura", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, Error.Message, "7009 – RegistrarAbonoFactura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
