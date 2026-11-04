/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Jose, Natali
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Natali Sofía Montenegro Portillo | Carné: 0901-23-10017
 * Encargado: Matthew Andreé Juárez Xoy
 * Fecha de actualización: 03/11/2026
 * Propósito: Emisión de facturas DTE con exportación a PDF nativo mediante el motor RDLC ReportViewer y recursos incrustados.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CapaVista_Facturas
{
    public partial class FrmFacturas : Form
    {
        private DataTable dtDetalleFactura;
        private bool sincronizandoCombos = false;
        private bool facturaEmitidaConExito = false;
        private bool origenExternoBloqueado = false;
        private bool bloqueandoEventos = false;

        private class ProductoPrueba
        {
            public int Codigo { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
        }

        private List<ProductoPrueba> listaProductosMock;

        public FrmFacturas()
        {
            InitializeComponent();

            // --- CONEXIÓN DE EVENTOS DE INTERFAZ ---
            this.FormClosing += FrmFacturas_FormClosing;

            VentasCboTipoOrigen.SelectedIndexChanged += VentasCboTipoOrigen_SelectedIndexChanged;
            VentasBtnCargarDocumento.Click += VentasBtnCargarDocumento_Click;

            VentasTxtNitReceptor.Leave += VentasTxtNitReceptor_Leave;
            VentasTxtNitReceptor.KeyDown += VentasTxtNitReceptor_KeyDown;
            VentasTxtNitReceptor.KeyPress += VentasTxtNitReceptor_KeyPress;

            VentasCboCodigoProducto.SelectedIndexChanged += VentasCboCodigoProducto_SelectedIndexChanged;
            VentasCboNombreProducto.SelectedIndexChanged += VentasCboNombreProducto_SelectedIndexChanged;
            VentasTxtCantidad.KeyPress += VentasTxtCantidad_KeyPress;
            VentasTxtCantidad.TextChanged += VentasTxtCantidad_TextChanged;
            VentasTxtDescuentos.TextChanged += VentasTxtDescuentos_TextChanged;

            VentasBtnAgregarItem.Click -= VentasBtnAgregarItem_Click;
            VentasBtnAgregarItem.Click += VentasBtnAgregarItem_Click;

            VentasDgvItems.KeyDown += VentasDgvItems_KeyDown;
            VentasDgvItems.SelectionChanged += VentasDgvItems_SelectionChanged;

            VentasBtnEmitirFactura.Click -= VentasBtnEmitirFactura_Click;
            VentasBtnEmitirFactura.Click += VentasBtnEmitirFactura_Click;

            // --- INICIALIZACIÓN DE DATOS ---
            InicializarDatosPrueba();
            ConfigurarOrigenDocumento();
            ConfigurarParametrosEmision();
            ConfigurarGrillaYParametrosIniciales();
            RecalcularTotalesGenerales();
        }

        private void InicializarDatosPrueba()
        {
            listaProductosMock = new List<ProductoPrueba>
            {
                new ProductoPrueba { Codigo = 101, Nombre = "Salchicha Jamonada 1lb", Precio = 20.00m },
                new ProductoPrueba { Codigo = 102, Nombre = "Jamón de Pavo Premium 250g", Precio = 35.50m },
                new ProductoPrueba { Codigo = 103, Nombre = "Chorizo Tradicional 1lb", Precio = 28.00m },
                new ProductoPrueba { Codigo = 104, Nombre = "Salami Italiano 200g", Precio = 42.00m }
            };

            bloqueandoEventos = true;
            VentasCboCodigoProducto.DataSource = new BindingSource(listaProductosMock, null);
            VentasCboCodigoProducto.DisplayMember = "Codigo";
            VentasCboCodigoProducto.ValueMember = "Codigo";

            VentasCboNombreProducto.DataSource = new BindingSource(listaProductosMock, null);
            VentasCboNombreProducto.DisplayMember = "Nombre";
            VentasCboNombreProducto.ValueMember = "Codigo";

            VentasCboCodigoProducto.SelectedIndex = -1;
            VentasCboNombreProducto.SelectedIndex = -1;
            bloqueandoEventos = false;
        }

        private void ConfigurarOrigenDocumento()
        {
            if (VentasCboTipoOrigen.SelectedIndex == -1)
            {
                VentasCboTipoOrigen.SelectedIndex = 0; // Manual por defecto
            }
        }

        private void ConfigurarParametrosEmision()
        {
            if (VentasCboTipoDte.Items.Count > 0)
            {
                VentasCboTipoDte.SelectedIndex = 0;
            }
            VentasCboTipoDte.Enabled = false;

            VentasCboMoneda.SelectedIndex = -1;
            VentasCboCondicionPago.SelectedIndex = -1;

            VentasTxtPrecioUnitario.Enabled = false;
            VentasTxtPrecioUnitario.ReadOnly = true;

            VentasTxtSubtotalLinea.Enabled = false;
            VentasTxtSubtotalLinea.ReadOnly = true;

            VentasTxtIvaLinea.Enabled = false;
            VentasTxtIvaLinea.ReadOnly = true;

            VentasTxtTotal.Enabled = false;
            VentasTxtTotal.ReadOnly = true;

            VentasTxtCantidad.Clear();
            VentasTxtDescuentos.Clear();
        }

        private void ConfigurarGrillaYParametrosIniciales()
        {
            VentasDtpFechaEmision.Value = DateTime.Now;

            dtDetalleFactura = new DataTable();
            dtDetalleFactura.Columns.Add("Codigo", typeof(int));
            dtDetalleFactura.Columns.Add("Producto", typeof(string));
            dtDetalleFactura.Columns.Add("Cantidad", typeof(decimal));
            dtDetalleFactura.Columns.Add("PrecioUnitario", typeof(decimal));
            dtDetalleFactura.Columns.Add("Descuento", typeof(decimal));
            dtDetalleFactura.Columns.Add("Subtotal", typeof(decimal));
            dtDetalleFactura.Columns.Add("Impuesto", typeof(decimal));
            dtDetalleFactura.Columns.Add("Total", typeof(decimal));

            VentasDgvItems.DataSource = dtDetalleFactura;
            VentasDgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            VentasDgvItems.AllowUserToAddRows = false;
            VentasDgvItems.ReadOnly = false;

            if (VentasDgvItems.Columns["Codigo"] != null) VentasDgvItems.Columns["Codigo"].HeaderText = "Código";
            if (VentasDgvItems.Columns["Producto"] != null) VentasDgvItems.Columns["Producto"].HeaderText = "Producto";
            if (VentasDgvItems.Columns["Cantidad"] != null) VentasDgvItems.Columns["Cantidad"].HeaderText = "Cantidad";
            if (VentasDgvItems.Columns["PrecioUnitario"] != null) VentasDgvItems.Columns["PrecioUnitario"].HeaderText = "Precio Unit.";
            if (VentasDgvItems.Columns["Descuento"] != null) VentasDgvItems.Columns["Descuento"].HeaderText = "Descuento";
            if (VentasDgvItems.Columns["Subtotal"] != null) VentasDgvItems.Columns["Subtotal"].HeaderText = "Subtotal";
            if (VentasDgvItems.Columns["Impuesto"] != null) VentasDgvItems.Columns["Impuesto"].HeaderText = "IVA";
            if (VentasDgvItems.Columns["Total"] != null) VentasDgvItems.Columns["Total"].HeaderText = "Total";
        }

        #region --- Control de Salida / Cerrar Ventana (X) ---

        private void FrmFacturas_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (facturaEmitidaConExito) return;

            bool hayDatosIngresados = (dtDetalleFactura != null && dtDetalleFactura.Rows.Count > 0) ||
                                      !string.IsNullOrWhiteSpace(VentasTxtNitReceptor.Text) ||
                                      !string.IsNullOrWhiteSpace(VentasTxtNombreComprador.Text);

            if (hayDatosIngresados)
            {
                DialogResult dr = MessageBox.Show(
                    "Si sale del formulario, se perderán todos los cambios no guardados.\n\n¿Desea salir de todos modos?",
                    "Confirmación de Salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region --- Lógica de Origen de Documentos (Cotizaciones / Pedidos) ---

        private void VentasCboTipoOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bloqueandoEventos) return;
            string tipoSeleccionado = VentasCboTipoOrigen.SelectedItem != null ? VentasCboTipoOrigen.SelectedItem.ToString() : "Manual";

            LimpiarFormularioCompleto(false);

            VentasCboDocumentoOrigen.Items.Clear();
            VentasCboDocumentoOrigen.Text = string.Empty;

            if (tipoSeleccionado == "Cotización")
            {
                VentasCboDocumentoOrigen.Enabled = true;
                VentasBtnCargarDocumento.Enabled = true;
                VentasCboDocumentoOrigen.Items.Add("COT-2026-001 (Distribuidora El Sol)");
                VentasCboDocumentoOrigen.Items.Add("COT-2026-002 (Supermercado La Bendición)");
                VentasCboDocumentoOrigen.SelectedIndex = 0;
            }
            else if (tipoSeleccionado == "Pedido")
            {
                VentasCboDocumentoOrigen.Enabled = true;
                VentasBtnCargarDocumento.Enabled = true;
                VentasCboDocumentoOrigen.Items.Add("PED-2026-501 (Distribuidora El Sol)");
                VentasCboDocumentoOrigen.Items.Add("PED-2026-502 (Supermercado La Bendición)");
                VentasCboDocumentoOrigen.SelectedIndex = 0;
            }
            else // Manual
            {
                VentasCboDocumentoOrigen.Enabled = false;
                VentasBtnCargarDocumento.Enabled = false;
                HabilitarEdicionManualProductos(true);
                VentasTxtNitReceptor.Enabled = true;
                origenExternoBloqueado = false;
                VentasDgvItems.ReadOnly = false;
            }
        }

        private void VentasBtnCargarDocumento_Click(object sender, EventArgs e)
        {
            if (VentasCboDocumentoOrigen.SelectedIndex == -1 || string.IsNullOrWhiteSpace(VentasCboDocumentoOrigen.Text))
            {
                MessageBox.Show("Seleccione un documento activo para cargar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string docSeleccionado = VentasCboDocumentoOrigen.SelectedItem.ToString();
            dtDetalleFactura.Clear();

            if (docSeleccionado.Contains("El Sol"))
            {
                VentasTxtNitReceptor.Text = "12345678";
                BuscarYAutocompletarCliente();

                dtDetalleFactura.Rows.Add(101, "Salchicha Jamonada 1lb", 10.00m, 20.00m, 0.00m, 200.00m, 24.00m, 200.00m);
                dtDetalleFactura.Rows.Add(103, "Chorizo Tradicional 1lb", 5.00m, 28.00m, 0.00m, 140.00m, 16.80m, 140.00m);
            }
            else
            {
                VentasTxtNitReceptor.Text = "87654321";
                BuscarYAutocompletarCliente();

                dtDetalleFactura.Rows.Add(102, "Jamón de Pavo Premium 250g", 8.00m, 35.50m, 0.00m, 284.00m, 34.08m, 284.00m);
            }

            HabilitarEdicionManualProductos(false);

            VentasTxtNitReceptor.Enabled = false;
            origenExternoBloqueado = true;
            VentasDgvItems.ReadOnly = true;
            VentasDgvItems.ClearSelection();

            RecalcularTotalesGenerales();
            MessageBox.Show($"Datos del documento '{docSeleccionado}' cargados con éxito.", "Carga Completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HabilitarEdicionManualProductos(bool habilitar)
        {
            VentasCboCodigoProducto.Enabled = habilitar;
            VentasCboNombreProducto.Enabled = habilitar;
            VentasTxtCantidad.Enabled = habilitar;
            VentasTxtDescuentos.Enabled = habilitar;
            VentasBtnAgregarItem.Enabled = habilitar;

            if (!habilitar)
            {
                bloqueandoEventos = true;
                VentasCboCodigoProducto.SelectedIndex = -1;
                VentasCboNombreProducto.SelectedIndex = -1;
                bloqueandoEventos = false;

                VentasTxtCantidad.Clear();
                VentasTxtPrecioUnitario.Clear();
                VentasTxtDescuentos.Clear();
                VentasTxtSubtotalLinea.Clear();
                VentasTxtIvaLinea.Clear();
                VentasTxtTotal.Clear();
            }
        }

        private void LimpiarFormularioCompleto(bool reiniciarOrigen)
        {
            bloqueandoEventos = true;
            if (reiniciarOrigen)
            {
                VentasCboTipoOrigen.SelectedIndex = 0; // Manual
            }

            if (dtDetalleFactura != null)
            {
                dtDetalleFactura.Clear();
            }

            VentasTxtNitReceptor.Clear();
            VentasTxtNitReceptor.Enabled = true;

            VentasTxtNombreComprador.Clear();
            VentasTxtDireccionComprador.Clear();

            VentasCboMoneda.SelectedIndex = -1;
            VentasCboCondicionPago.SelectedIndex = -1;

            origenExternoBloqueado = false;
            HabilitarEdicionManualProductos(true);
            VentasDgvItems.ReadOnly = false;
            RecalcularTotalesGenerales();
            bloqueandoEventos = false;
        }

        #endregion

        #region --- Control de Bloqueo de Grilla (Cotización / Pedido) ---

        private void VentasDgvItems_SelectionChanged(object sender, EventArgs e)
        {
            if (origenExternoBloqueado && VentasDgvItems != null)
            {
                VentasDgvItems.ClearSelection();
            }
        }

        private void VentasDgvItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (origenExternoBloqueado)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                if (VentasDgvItems.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in VentasDgvItems.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            VentasDgvItems.Rows.Remove(row);
                        }
                    }
                    RecalcularTotalesGenerales();
                }
            }
        }

        #endregion

        #region --- Autocompletado de Receptor por NIT / Código ---

        private void VentasTxtNitReceptor_Leave(object sender, EventArgs e)
        {
            BuscarYAutocompletarCliente();
        }

        private void VentasTxtNitReceptor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarYAutocompletarCliente();
                e.SuppressKeyPress = true;
            }
        }

        private void BuscarYAutocompletarCliente()
        {
            string nitIngresado = VentasTxtNitReceptor.Text.Trim();

            if (nitIngresado == "12345678")
            {
                VentasTxtNombreComprador.Text = "Distribuidora El Sol, S.A.";
                VentasTxtDireccionComprador.Text = "6ta Avenida 12-45 Zona 9, Guatemala";
            }
            else if (nitIngresado == "87654321")
            {
                VentasTxtNombreComprador.Text = "Supermercado La Bendición";
                VentasTxtDireccionComprador.Text = "Calzada Roosevelt 34-12 Zona 11, Guatemala";
            }
            else if (nitIngresado.ToUpper() == "CF")
            {
                VentasTxtNombreComprador.Text = "CONSUMIDOR FINAL";
                VentasTxtDireccionComprador.Text = "CIUDAD";
            }
        }

        #endregion

        #region --- Sincronización Recíproca de Combos de Producto ---

        private void VentasCboCodigoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sincronizandoCombos || bloqueandoEventos) return;

            if (VentasCboCodigoProducto.SelectedIndex != -1)
            {
                sincronizandoCombos = true;
                VentasCboNombreProducto.SelectedValue = VentasCboCodigoProducto.SelectedValue;
                CargarPrecioProductoSeleccionado();
                sincronizandoCombos = false;
            }
        }

        private void VentasCboNombreProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sincronizandoCombos || bloqueandoEventos) return;

            if (VentasCboNombreProducto.SelectedIndex != -1)
            {
                sincronizandoCombos = true;
                VentasCboCodigoProducto.SelectedValue = VentasCboNombreProducto.SelectedValue;
                CargarPrecioProductoSeleccionado();
                sincronizandoCombos = false;
            }
        }

        private void CargarPrecioProductoSeleccionado()
        {
            if (VentasCboCodigoProducto.SelectedItem is ProductoPrueba prod)
            {
                VentasTxtPrecioUnitario.Text = prod.Precio.ToString("F2");
                VentasTxtDescuentos.Text = "0.00";
                if (string.IsNullOrWhiteSpace(VentasTxtCantidad.Text))
                {
                    VentasTxtCantidad.Text = "1";
                }
                CalcularSubtotalRenglon();
            }
        }

        private void CalcularSubtotalRenglon()
        {
            if (bloqueandoEventos) return;

            decimal.TryParse(VentasTxtCantidad.Text, out decimal cant);
            decimal.TryParse(VentasTxtPrecioUnitario.Text, out decimal precio);
            decimal.TryParse(VentasTxtDescuentos.Text, out decimal desc);

            decimal subtotal = (cant * precio) - desc;
            decimal iva = subtotal * 0.12m;
            decimal total = subtotal;

            VentasTxtSubtotalLinea.Text = subtotal.ToString("F2");
            VentasTxtIvaLinea.Text = iva.ToString("F2");
            VentasTxtTotal.Text = total.ToString("F2");
        }

        #endregion

        #region --- Métodos de Validación y Seguridad de Formulario ---

        private bool EsNitValido(string nit)
        {
            if (string.IsNullOrWhiteSpace(nit)) return false;
            string nitLimpio = nit.Trim().ToUpper();
            if (nitLimpio.Length < 1 || nitLimpio.Length > 15) return false;
            if (nitLimpio == "CF") return true;
            return Regex.IsMatch(nitLimpio, @"^[0-9]+(-?[0-9K])?$");
        }

        private bool ValidarFormularioCompleto()
        {
            if (VentasCboMoneda.SelectedIndex == -1) return false;
            if (VentasCboCondicionPago.SelectedIndex == -1) return false;

            if (!EsNitValido(VentasTxtNitReceptor.Text)) return false;
            if (string.IsNullOrWhiteSpace(VentasTxtNombreComprador.Text)) return false;
            if (string.IsNullOrWhiteSpace(VentasTxtDireccionComprador.Text)) return false;

            if (dtDetalleFactura == null || dtDetalleFactura.Rows.Count == 0) return false;

            return true;
        }

        private void RecalcularTotalesGenerales()
        {
            decimal subtotalGeneral = 0;
            decimal ivaGeneral = 0;
            decimal totalGeneral = 0;

            if (dtDetalleFactura != null)
            {
                foreach (DataRow row in dtDetalleFactura.Rows)
                {
                    subtotalGeneral += Convert.ToDecimal(row["Subtotal"]);
                    ivaGeneral += Convert.ToDecimal(row["Impuesto"]);
                    totalGeneral += Convert.ToDecimal(row["Total"]);
                }
            }

            Control[] foundSub = this.Controls.Find("VentasLblSubtotalResumen", true);
            if (foundSub.Length > 0 && foundSub[0] is Label lblSub)
                lblSub.Text = string.Format("Subtotal:                              Q {0:N2}", subtotalGeneral);

            Control[] foundIva = this.Controls.Find("VentasLblImpuestosResumen", true);
            if (foundIva.Length > 0 && foundIva[0] is Label lblIva)
                lblIva.Text = string.Format("Impuestos (IVA):                Q {0:N2}", ivaGeneral);

            Control[] foundTot = this.Controls.Find("VentasLblTotalGeneralResumen", true);
            if (foundTot.Length > 0 && foundTot[0] is Label lblTot)
                lblTot.Text = string.Format("TOTAL DE LA FACTURA:       Q {0:N2}", totalGeneral);
        }

        #endregion

        #region --- Eventos KeyPress & TextChanged ---

        private void VentasTxtNitReceptor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '-' && e.KeyChar != 'k' && e.KeyChar != 'K')
            {
                e.Handled = true;
            }
        }

        private void VentasTxtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && txt != null && txt.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void VentasTxtCantidad_TextChanged(object sender, EventArgs e) => CalcularSubtotalRenglon();
        private void VentasTxtDescuentos_TextChanged(object sender, EventArgs e) => CalcularSubtotalRenglon();

        #endregion

        #region --- Botones de Acción ---

        private void VentasBtnAgregarItem_Click(object sender, EventArgs e)
        {
            if (VentasCboCodigoProducto.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un producto del inventario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(VentasTxtCantidad.Text, out decimal cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida mayor a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int codigo = Convert.ToInt32(VentasCboCodigoProducto.SelectedValue);
            string producto = VentasCboNombreProducto.Text;
            decimal precio = Convert.ToDecimal(VentasTxtPrecioUnitario.Text);
            decimal descuento = string.IsNullOrWhiteSpace(VentasTxtDescuentos.Text) ? 0.00m : Convert.ToDecimal(VentasTxtDescuentos.Text);
            decimal subtotal = Convert.ToDecimal(VentasTxtSubtotalLinea.Text);
            decimal iva = Convert.ToDecimal(VentasTxtIvaLinea.Text);
            decimal total = Convert.ToDecimal(VentasTxtTotal.Text);

            dtDetalleFactura.Rows.Add(codigo, producto, cantidad, precio, descuento, subtotal, iva, total);

            bloqueandoEventos = true;
            sincronizandoCombos = true;

            VentasCboCodigoProducto.SelectedIndex = -1;
            VentasCboNombreProducto.SelectedIndex = -1;

            sincronizandoCombos = false;
            bloqueandoEventos = false;

            VentasTxtCantidad.Clear();
            VentasTxtPrecioUnitario.Clear();
            VentasTxtDescuentos.Clear();
            VentasTxtSubtotalLinea.Clear();
            VentasTxtIvaLinea.Clear();
            VentasTxtTotal.Clear();

            RecalcularTotalesGenerales();

            MessageBox.Show("Producto agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void VentasBtnEmitirFactura_Click(object sender, EventArgs e)
        {
            if (!ValidarFormularioCompleto())
            {
                MessageBox.Show("Debe de llenar todos los campos para emitir la factura.",
                                "Validación de Formulario - DTE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idFacturaGenerada = 7001;
                facturaEmitidaConExito = true;

                MostrarOpcionesSalidaRdlc(idFacturaGenerada);

                LimpiarFormularioCompleto(true);
                facturaEmitidaConExito = false;
            }
            catch (Exception ex)
            {
                facturaEmitidaConExito = false;
                MessageBox.Show("Error crítico al procesar el guardado de la factura en la BD: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarOpcionesSalidaRdlc(int idFactura)
        {
            Form dlgOpciones = new Form();
            dlgOpciones.Text = "Emisión Exitosa - DTE";
            dlgOpciones.Size = new Size(430, 210);
            dlgOpciones.StartPosition = FormStartPosition.CenterParent;
            dlgOpciones.FormBorderStyle = FormBorderStyle.FixedDialog;
            dlgOpciones.MaximizeBox = false;
            dlgOpciones.MinimizeBox = false;
            dlgOpciones.BackColor = Color.FromArgb(248, 245, 249);

            Label lblMensaje = new Label();
            lblMensaje.Text = "¡Factura emitida de forma exitosa!";
            lblMensaje.Location = new Point(20, 30);
            lblMensaje.Size = new Size(380, 35);
            lblMensaje.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblMensaje.ForeColor = Color.FromArgb(59, 29, 74);
            lblMensaje.TextAlign = ContentAlignment.MiddleCenter;

            Button btnImprimir = new Button();
            btnImprimir.Text = "🖨️ Imprimir";
            btnImprimir.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnImprimir.BackColor = Color.FromArgb(59, 29, 74);
            btnImprimir.ForeColor = Color.White;
            btnImprimir.FlatStyle = FlatStyle.Flat;
            btnImprimir.Location = new Point(45, 95);
            btnImprimir.Size = new Size(150, 45);
            btnImprimir.Click += (s, args) =>
            {
                dlgOpciones.DialogResult = DialogResult.Yes;
                dlgOpciones.Close();
            };

            Button btnPdf = new Button();
            btnPdf.Text = "📄 PDF";
            btnPdf.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnPdf.BackColor = Color.FromArgb(123, 63, 147);
            btnPdf.ForeColor = Color.White;
            btnPdf.FlatStyle = FlatStyle.Flat;
            btnPdf.Location = new Point(215, 95);
            btnPdf.Size = new Size(150, 45);
            btnPdf.Click += (s, args) =>
            {
                dlgOpciones.DialogResult = DialogResult.No;
                dlgOpciones.Close();
            };

            dlgOpciones.Controls.Add(lblMensaje);
            dlgOpciones.Controls.Add(btnImprimir);
            dlgOpciones.Controls.Add(btnPdf);

            DialogResult resultado = dlgOpciones.ShowDialog(this);

            if (resultado == DialogResult.Yes)
            {
                using (PrintDialog printDialog = new PrintDialog())
                {
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Enviando diseño de factura RDLC a la impresora seleccionada...", "Impresión DTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (resultado == DialogResult.No)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
                    saveFileDialog.Title = "Guardar Factura - DTE";
                    saveFileDialog.FileName = $"Factura_{idFactura}.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            LocalReport report = new LocalReport();

                            // Carga segura del informe mediante Recurso Incrustado (Embedded Resource)
                            Assembly ensamblado = Assembly.GetExecutingAssembly();
                            string nombreRecurso = ensamblado.GetManifestResourceNames()
                                .FirstOrDefault(n => n.EndsWith("rptFactura.rdlc", StringComparison.OrdinalIgnoreCase));

                            if (string.IsNullOrEmpty(nombreRecurso))
                            {
                                throw new FileNotFoundException("No se encontró el archivo 'rptFactura.rdlc' configurado como Recurso Incrustado en el proyecto.");
                            }

                            using (Stream streamReporte = ensamblado.GetManifestResourceStream(nombreRecurso))
                            {
                                report.LoadReportDefinition(streamReporte);
                            }

                            report.DataSources.Clear();
                            report.DataSources.Add(new ReportDataSource("dsDetalle", dtDetalleFactura));

                            string mimeType, encoding, extension;
                            string[] streamids;
                            Warning[] warnings;

                            // Renderizado binario PDF mediante el motor RDLC
                            byte[] pdfBytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                            File.WriteAllBytes(saveFileDialog.FileName, pdfBytes);
                            MessageBox.Show("La factura en PDF se ha generado y guardado correctamente con su diseño RDLC.", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            Exception inner = ex.InnerException != null ? ex.InnerException : ex;
                            MessageBox.Show("Detalle técnico del error RDLC: " + inner.Message, "Error de Procesamiento Local", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 03/11/2026