/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Jose, Natali
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Natali Sofía Montenegro Portillo | Carné: 0901-23-10017
 * Encargado: Matthew Andreé Juárez Xoy
 * Fecha de actualización: 03/11/2026
 * Propósito: Emisión de facturas DTE con descarga automática de PDF e impresión directa integrada.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Facturas;

namespace CapaVista_Facturas
{
    public partial class FrmFacturas : Form
    {
        private readonly ClsFacturaOrigenControlador _FacturasOrigen = new ClsFacturaOrigenControlador();
        public int VentasIdUsuario { get; set; }
        private int _IdOrigenCargado;

        private DataTable dtDetalleFactura;
        private bool sincronizandoCombos = false;
        private bool facturaEmitidaConExito = false;
        private bool origenExternoBloqueado = false;
        private bool bloqueandoEventos = false;

        // Variables para soporte de impresión directa de páginas RDLC
        private int m_currentPageIndex;
        private IList<Stream> m_streams;

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

            VentasCboTipoOrigen.DropDownStyle = ComboBoxStyle.DropDownList;
            VentasCboDocumentoOrigen.DropDownStyle = ComboBoxStyle.DropDownList;
            VentasCboTipoOrigen.SelectedIndexChanged += VentasCboTipoOrigen_SelectedIndexChanged;
            VentasCboDocumentoOrigen.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!bloqueandoEventos && _IdOrigenCargado > 0)
                    LimpiarFormularioCompleto(false);
            };
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
            dtDetalleFactura.Columns.Add("NitCliente", typeof(string));
            dtDetalleFactura.Columns.Add("NombreCliente", typeof(string));
            dtDetalleFactura.Columns.Add("DireccionCliente", typeof(string));

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

            if (VentasDgvItems.Columns["NitCliente"] != null) VentasDgvItems.Columns["NitCliente"].Visible = false;
            if (VentasDgvItems.Columns["NombreCliente"] != null) VentasDgvItems.Columns["NombreCliente"].Visible = false;
            if (VentasDgvItems.Columns["DireccionCliente"] != null) VentasDgvItems.Columns["DireccionCliente"].Visible = false;
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
            LimpiarFormularioCompleto(false);
            VentasCboDocumentoOrigen.DataSource = null;
            VentasBtnCargarDocumento.Enabled = false;
            VentasCboDocumentoOrigen.Enabled = false;
            VentasTtpAccionesOriginales.SetToolTip(VentasBtnEmitirFactura,
                VentasCboTipoOrigen.SelectedIndex > 0 ? "Registrar factura interna" : "Generar factura");
            if (VentasCboTipoOrigen.SelectedIndex <= 0) return;

            try
            {
                DataTable Documentos = _FacturasOrigen.VentasFuncOrigenes(VentasCboTipoOrigen.SelectedIndex == 2);
                VentasCboDocumentoOrigen.DisplayMember = "Nombre";
                VentasCboDocumentoOrigen.ValueMember = "Id";
                VentasCboDocumentoOrigen.DataSource = Documentos;
                VentasCboDocumentoOrigen.SelectedIndex = -1;
                VentasCboDocumentoOrigen.Enabled = Documentos.Rows.Count > 0;
                VentasBtnCargarDocumento.Enabled = Documentos.Rows.Count > 0;
                if (Documentos.Rows.Count == 0)
                    MessageBox.Show("No hay documentos disponibles para facturar.", "Facturas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, "Consulta de documentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasBtnCargarDocumento_Click(object sender, EventArgs e)
        {
            if (VentasCboDocumentoOrigen.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione una cotización o pedido para cargar.", "Facturas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int Id = Convert.ToInt32(VentasCboDocumentoOrigen.SelectedValue);
                DataSet Documento = _FacturasOrigen.VentasFuncCargar(VentasCboTipoOrigen.SelectedIndex == 2, Id);
                VentasMetMostrarOrigen(Documento);
                _IdOrigenCargado = Id;
            }
            catch (Exception Error)
            {
                LimpiarFormularioCompleto(false);
                MessageBox.Show(Error.Message, "Carga de documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetMostrarOrigen(DataSet Documento)
        {
            DataRow Cabecera = Documento.Tables[0].Rows[0];
            origenExternoBloqueado = true;
            VentasTxtNitReceptor.Enabled = false;
            VentasTxtNitReceptor.Text = Convert.ToString(Cabecera["Identificacion"]);
            VentasTxtNombreComprador.Text = Convert.ToString(Cabecera["Cliente"]);
            VentasTxtDireccionComprador.Text = Convert.ToString(Cabecera["Direccion"]);
            VentasCboMoneda.Items.Clear();
            VentasCboMoneda.Items.Add(Convert.ToString(Cabecera["Moneda"]));
            VentasCboMoneda.SelectedIndex = 0;
            VentasCboMoneda.Enabled = false;
            if (VentasCboCondicionPago.SelectedIndex < 0)
                VentasCboCondicionPago.SelectedItem = Convert.ToInt32(Cabecera["DiasCredito"]) > 0 ? "CREDITO" : "CONTADO";

            dtDetalleFactura.Clear();
            foreach (DataRow Linea in Documento.Tables[1].Rows)
                dtDetalleFactura.Rows.Add(Linea["ProductoId"], Linea["Producto"], Linea["Cantidad"], Linea["Precio"],
                    Linea["Descuento"], Linea["Subtotal"], Linea["Impuesto"], Linea["Total"],
                    DBNull.Value, DBNull.Value, DBNull.Value);
            HabilitarEdicionManualProductos(false);
            VentasDgvItems.ReadOnly = true;
            VentasDgvItems.ClearSelection();
            RecalcularTotalesGenerales();
        }

        private int VentasFuncElegirSucursal()
        {
            DataTable Sucursales = _FacturasOrigen.VentasFuncSucursales();
            if (Sucursales.Rows.Count == 0) throw new InvalidOperationException("Configure una sucursal antes de registrar la factura.");
            if (Sucursales.Rows.Count == 1) return Convert.ToInt32(Sucursales.Rows[0]["Id"]);

            using (var Dialogo = new Form { Text = "Sucursal emisora", ClientSize = new Size(380, 110),
                StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false, MaximizeBox = false })
            {
                var Selector = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 12, Top = 16, Width = 356,
                    DisplayMember = "Nombre", ValueMember = "Id", DataSource = Sucursales };
                var Aceptar = new Button { Text = "Registrar", DialogResult = DialogResult.OK, Left = 176, Top = 64, Width = 92 };
                var Cancelar = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Left = 276, Top = 64, Width = 92 };
                Dialogo.Controls.AddRange(new Control[] { Selector, Aceptar, Cancelar });
                Dialogo.AcceptButton = Aceptar;
                Dialogo.CancelButton = Cancelar;
                return Dialogo.ShowDialog(this) == DialogResult.OK ? Convert.ToInt32(Selector.SelectedValue) : 0;
            }
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
                VentasCboDocumentoOrigen.DataSource = null;
                VentasCboDocumentoOrigen.Enabled = false;
                VentasBtnCargarDocumento.Enabled = false;
            }

            if (dtDetalleFactura != null)
            {
                dtDetalleFactura.Clear();
            }

            VentasTxtNitReceptor.Clear();
            VentasTxtNitReceptor.Enabled = true;

            VentasTxtNombreComprador.Clear();
            VentasTxtDireccionComprador.Clear();

            VentasCboMoneda.Items.Clear();
            VentasCboMoneda.Items.AddRange(new object[] { "GTQ-Quetzal", "USD-Dólar estadounidense" });
            VentasCboMoneda.SelectedIndex = -1;
            VentasCboMoneda.Enabled = true;
            VentasCboCondicionPago.SelectedIndex = -1;

            _IdOrigenCargado = 0;
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
            if (origenExternoBloqueado) return;
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

            if (!origenExternoBloqueado && !EsNitValido(VentasTxtNitReceptor.Text)) return false;
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

            string Moneda = origenExternoBloqueado ? VentasCboMoneda.Text : "Q";
            Control[] foundSub = this.Controls.Find("VentasLblSubtotalResumen", true);
            if (foundSub.Length > 0 && foundSub[0] is Label lblSub)
                lblSub.Text = string.Format("Subtotal:                              {0} {1:N2}", Moneda, subtotalGeneral);

            Control[] foundIva = this.Controls.Find("VentasLblImpuestosResumen", true);
            if (foundIva.Length > 0 && foundIva[0] is Label lblIva)
                lblIva.Text = string.Format("Impuestos (IVA):                {0} {1:N2}", Moneda, ivaGeneral);

            Control[] foundTot = this.Controls.Find("VentasLblTotalGeneralResumen", true);
            if (foundTot.Length > 0 && foundTot[0] is Label lblTot)
                lblTot.Text = string.Format("TOTAL DE LA FACTURA:       {0} {1:N2}", Moneda, totalGeneral);
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

            if (!decimal.TryParse(VentasTxtCantidad.Text, out decimal cantidadAgregada) || cantidadAgregada <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida mayor a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int codigo = Convert.ToInt32(VentasCboCodigoProducto.SelectedValue);
            string producto = VentasCboNombreProducto.Text;
            decimal precio = Convert.ToDecimal(VentasTxtPrecioUnitario.Text);
            decimal descuentoAgregado = string.IsNullOrWhiteSpace(VentasTxtDescuentos.Text) ? 0.00m : Convert.ToDecimal(VentasTxtDescuentos.Text);

            DataRow filaExistente = dtDetalleFactura.AsEnumerable()
                .FirstOrDefault(r => r.Field<int>("Codigo") == codigo);

            if (filaExistente != null)
            {
                decimal cantidadActual = filaExistente.Field<decimal>("Cantidad");
                decimal descuentoActual = filaExistente.Field<decimal>("Descuento");

                decimal nuevaCantidad = cantidadActual + cantidadAgregada;
                decimal nuevoDescuento = descuentoActual + descuentoAgregado;

                decimal nuevoSubtotal = (nuevaCantidad * precio) - nuevoDescuento;
                decimal nuevoIva = nuevoSubtotal * 0.12m;
                decimal nuevoTotal = nuevoSubtotal;

                filaExistente["Cantidad"] = nuevaCantidad;
                filaExistente["Descuento"] = nuevoDescuento;
                filaExistente["Subtotal"] = nuevoSubtotal;
                filaExistente["Impuesto"] = nuevoIva;
                filaExistente["Total"] = nuevoTotal;
            }
            else
            {
                decimal subtotal = (cantidadAgregada * precio) - descuentoAgregado;
                decimal iva = subtotal * 0.12m;
                decimal total = subtotal;

                dtDetalleFactura.Rows.Add(codigo, producto, cantidadAgregada, precio, descuentoAgregado, subtotal, iva, total, DBNull.Value, DBNull.Value, DBNull.Value);
            }

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
            bool DesdeDocumento = VentasCboTipoOrigen.SelectedIndex > 0;
            if (DesdeDocumento && _IdOrigenCargado == 0)
            {
                MessageBox.Show("Cargue primero el documento seleccionado.", "Facturas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidarFormularioCompleto())
            {
                MessageBox.Show("Debe de llenar todos los campos para emitir la factura.",
                                "Validación de Formulario - DTE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idFacturaGenerada = 7001;
                if (DesdeDocumento)
                {
                    int Sucursal = VentasFuncElegirSucursal();
                    if (Sucursal == 0) return;
                    DataSet Registrado;
                    idFacturaGenerada = _FacturasOrigen.VentasFuncRegistrar(VentasCboTipoOrigen.SelectedIndex == 2,
                        _IdOrigenCargado, Sucursal, VentasIdUsuario, Convert.ToString(VentasCboCondicionPago.SelectedItem),
                        out Registrado);
                    facturaEmitidaConExito = true;
                    VentasMetMostrarOrigen(Registrado);
                }
                facturaEmitidaConExito = true;
                MostrarOpcionesSalidaRdlc(idFacturaGenerada, DesdeDocumento);

                LimpiarFormularioCompleto(true);
                facturaEmitidaConExito = false;
            }
            catch (Exception ex)
            {
                if (DesdeDocumento && facturaEmitidaConExito)
                {
                    LimpiarFormularioCompleto(true);
                    MessageBox.Show("La factura quedó registrada. No se pudo generar la salida: " + ex.Message,
                        "Factura registrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show(ex.Message, "Facturas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                facturaEmitidaConExito = false;
            }
        }

        private void MostrarOpcionesSalidaRdlc(int idFactura, bool DocumentoInterno)
        {
            Form dlgOpciones = new Form();
            dlgOpciones.Text = DocumentoInterno ? "Factura interna registrada" : "Emisión Exitosa - DTE";
            dlgOpciones.Size = new Size(460, 210);
            dlgOpciones.StartPosition = FormStartPosition.CenterParent;
            dlgOpciones.FormBorderStyle = FormBorderStyle.FixedDialog;
            dlgOpciones.MaximizeBox = false;
            dlgOpciones.MinimizeBox = false;
            dlgOpciones.BackColor = Color.FromArgb(248, 245, 249);

            Label lblMensaje = new Label();
            lblMensaje.Text = DocumentoInterno ? "Factura registrada en el ERP" : "¡Factura emitida de forma exitosa!";
            lblMensaje.Location = new Point(20, 25);
            lblMensaje.Size = new Size(410, 30);
            lblMensaje.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblMensaje.ForeColor = Color.FromArgb(59, 29, 74);
            lblMensaje.TextAlign = ContentAlignment.MiddleCenter;

            // Botón 1: Solo PDF (Guarda el PDF)
            Button btnPdf = new Button();
            btnPdf.Text = "📄 PDF";
            btnPdf.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPdf.BackColor = Color.FromArgb(123, 63, 147);
            btnPdf.ForeColor = Color.White;
            btnPdf.FlatStyle = FlatStyle.Flat;
            btnPdf.Location = new Point(50, 85);
            btnPdf.Size = new Size(160, 45);
            btnPdf.Click += (s, args) =>
            {
                dlgOpciones.DialogResult = DialogResult.No; // Indicador de solo PDF
                dlgOpciones.Close();
            };

            // Botón 2: PDF e Imprimir (Guarda el PDF y manda directo a la impresora)
            Button btnPdfImprimir = new Button();
            btnPdfImprimir.Text = "🖨️ PDF e Imprimir";
            btnPdfImprimir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPdfImprimir.BackColor = Color.FromArgb(59, 29, 74);
            btnPdfImprimir.ForeColor = Color.White;
            btnPdfImprimir.FlatStyle = FlatStyle.Flat;
            btnPdfImprimir.Location = new Point(235, 85);
            btnPdfImprimir.Size = new Size(160, 45);
            btnPdfImprimir.Click += (s, args) =>
            {
                dlgOpciones.DialogResult = DialogResult.Yes; // Indicador de PDF + Impresión
                dlgOpciones.Close();
            };

            dlgOpciones.Controls.Add(lblMensaje);
            dlgOpciones.Controls.Add(btnPdf);
            dlgOpciones.Controls.Add(btnPdfImprimir);

            DialogResult resultado = dlgOpciones.ShowDialog(this);

            // Preparación común del Reporte RDLC e Inyección de Datos del Receptor
            LocalReport report = new LocalReport();
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

            string nitActual = string.IsNullOrWhiteSpace(VentasTxtNitReceptor.Text) ? "CF" : VentasTxtNitReceptor.Text.Trim();
            string nombreActual = string.IsNullOrWhiteSpace(VentasTxtNombreComprador.Text) ? "CONSUMIDOR FINAL" : VentasTxtNombreComprador.Text.Trim();
            string direccionActual = string.IsNullOrWhiteSpace(VentasTxtDireccionComprador.Text) ? "CIUDAD" : VentasTxtDireccionComprador.Text.Trim();

            foreach (DataRow row in dtDetalleFactura.Rows)
            {
                row["NitCliente"] = nitActual;
                row["NombreCliente"] = nombreActual;
                row["DireccionCliente"] = direccionActual;
            }
            dtDetalleFactura.AcceptChanges();

            report.DataSources.Clear();
            report.DataSources.Add(new ReportDataSource("dsDetalle", dtDetalleFactura));

            // ACCIÓN 1: SIEMPRE DESCARGAR / GUARDAR EL PDF
            string rutaPdfGuardado = string.Empty;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
                saveFileDialog.Title = DocumentoInterno ? "Guardar factura interna" : "Guardar Factura - DTE";
                saveFileDialog.FileName = $"Factura_{idFactura}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string mimeType, encoding, extension;
                        string[] streamids;
                        Warning[] warnings;

                        byte[] pdfBytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                        File.WriteAllBytes(saveFileDialog.FileName, pdfBytes);
                        rutaPdfGuardado = saveFileDialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        Exception inner = ex.InnerException != null ? ex.InnerException : ex;
                        MessageBox.Show("Detalle técnico del error al generar PDF: " + inner.Message, "Error de Procesamiento Local", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    // Si el usuario cancela el diálogo de guardado, detenemos el proceso
                    return;
                }
            }

            // Mensaje de éxito de descarga del PDF
            MessageBox.Show("El archivo PDF de la factura se ha guardado correctamente.", "Descarga Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // ACCIÓN 2: SI ELIGIÓ EL BOTÓN COMBINADO (PDF e Imprimir), ADEMÁS MANDAMOS A IMPRIMIR
            if (resultado == DialogResult.Yes && !string.IsNullOrEmpty(rutaPdfGuardado))
            {
                try
                {
                    ImprimirReporteDirecto(report);
                    MessageBox.Show("La factura también se ha enviado a la impresora correctamente.", "Impresión Directa Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("El PDF se guardó con éxito, pero ocurrió un error al intentar imprimir directamente: " + ex.Message, "Error de Impresión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #endregion

        #region --- Métodos de Impresión Directa RDLC ---

        private void ImprimirReporteDirecto(LocalReport report)
        {
            ExportarAStream(report);

            m_currentPageIndex = 0;
            if (m_streams == null || m_streams.Count == 0)
                return;

            using (PrintDocument printDoc = new PrintDocument())
            {
                if (!printDoc.PrinterSettings.IsValid)
                {
                    throw new Exception("No se encontró ninguna impresora válida instalada o predeterminada en el sistema.");
                }

                printDoc.PrintPage += new PrintPageEventHandler(PrintPageHandler);
                printDoc.Print();
            }
        }

        private void ExportarAStream(LocalReport report)
        {
            string deviceInfo =
                @"<DeviceInfo>" +
                @"  <OutputFormat>EMF</OutputFormat>" +
                @"  <PageWidth>8.5in</PageWidth>" +
                @"  <PageHeight>11in</PageHeight>" +
                @"  <MarginTop>0.5in</MarginTop>" +
                @"  <MarginLeft>0.5in</MarginLeft>" +
                @"  <MarginRight>0.5in</MarginRight>" +
                @"  <MarginBottom>0.5in</MarginBottom>" +
                @"</DeviceInfo>";

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStreamCallback, out warnings);

            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        private Stream CreateStreamCallback(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs ev)
        {
            Metafile pageMetafile = new Metafile(m_streams[m_currentPageIndex]);

            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);
            ev.Graphics.DrawImage(pageMetafile, adjustedRect);

            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        #endregion
    }
}
// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 03/11/2026
