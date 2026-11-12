using System;
using System.Data;
using System.Windows.Forms;
// Si mantienes el controlador en AprobacionFinanciera, déjalo. Si lo moviste a Compras, ajusta el namespace.
using CapaControlador_AprobacionFinanciera;

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmVerCompra : Form
    {
        // Instancia a tu controlador (ajusta el nombre de la clase si lo cambiaste)
        private ClsControladorDevoluciones _Controlador = new ClsControladorDevoluciones();
        private int _IdCompraBase;
        private DataTable _DetallesCompraBase;

        public FrmVerCompra(int IdCompra)
        {
            InitializeComponent();
            _IdCompraBase = IdCompra;

            this.Text = "6007 - Visor de Compra";
            this.MaximizeBox = false;

            // 1. Bloqueamos la ventana para que sea solo de lectura
            ComprasMetBloquearModificaciones();

            // 2. Extraemos y pintamos los datos
            ComprasMetCargarDatosBase();
        }

        private void ComprasMetBloquearModificaciones()
        {
            // Bloqueo de Encabezado
            ComprasTxtNumFactura.Enabled = false;
            ComprasTxtSerieFactura.Enabled = false;
            ComprasTxtMontoFisica.Enabled = false;
            ComprasTxtNumOrden.Enabled = false;
            
            ComprasDtpFechaemision.Enabled = false;
            ComprasDtpRecibido.Enabled = false;

          
            // Bloqueo de la Tabla Visual
            ComprasDgvComprasrecibido.ReadOnly = true;
            ComprasDgvComprasrecibido.AllowUserToAddRows = false;
            ComprasDgvComprasrecibido.AllowUserToDeleteRows = false;

            // Ocultar componentes innecesarios en un visor (si tienes estos controles)
            // ComprasBtnGuardar.Visible = false;
            // ComprasBtnModificarProducto.Visible = false;
        }

        private void ComprasMetCargarDatosBase()
        {
            try
            {
                // A) LLENAR LOS DATOS GENERALES (Factura)
                DataTable Encabezado = _Controlador.ComprasFuncObtenerEncabezadoCompra(_IdCompraBase);
                if (Encabezado != null && Encabezado.Rows.Count > 0)
                {
                    DataRow Fila = Encabezado.Rows[0];

                    // Asignación de textos
                    ComprasLblEstadoValor.Text = Fila["estadoCompra"].ToString();
                    ComprasTxtSerieFactura.Text = Fila["serieFacturaCompra"].ToString();
                    ComprasTxtNumFactura.Text = Fila["numeroFacturaCompra"].ToString();
                    ComprasTxtMontoFisica.Text = Convert.ToDecimal(Fila["totalCompra"]).ToString("N2");

                    // Referencia a la orden original (si existe)
                    if (Fila["idOrdenCompra"] != DBNull.Value)
                    {
                        ComprasTxtNumOrden.Text = Fila["idOrdenCompra"].ToString();
                    }

                    // Fechas
                    if (DateTime.TryParse(Fila["fechaCompra"].ToString(), out DateTime fecha))
                    {
                        ComprasDtpRecibido.Value = fecha;
                        ComprasDtpFechaemision.Value = fecha;
                    }
                }

                // B) LLENAR LOS PRODUCTOS (Tabla)
                _DetallesCompraBase = _Controlador.ComprasFuncObtenerDetallesCompra(_IdCompraBase);

                if (_DetallesCompraBase != null && _DetallesCompraBase.Rows.Count > 0)
                {
                    // Llenar el ComboBox de Bodegas 
                    DataTable bodegasDistintas = _DetallesCompraBase.DefaultView.ToTable(true, "idBodega");
                    ComprasCboBodega.DataSource = bodegasDistintas;
                    ComprasCboBodega.DisplayMember = "idBodega";
                    ComprasCboBodega.ValueMember = "idBodega";

                    // Configurar el DataGridView
                    ComprasDgvComprasrecibido.AutoGenerateColumns = true;
                    ComprasDgvComprasrecibido.DataSource = _DetallesCompraBase;
                    ComprasDgvComprasrecibido.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Dar formato visual a las columnas (Si existen)
                    if (ComprasDgvComprasrecibido.Columns.Contains("Precio"))
                        ComprasDgvComprasrecibido.Columns["Precio"].DefaultCellStyle.Format = "N2";
                    if (ComprasDgvComprasrecibido.Columns.Contains("Subtotal"))
                        ComprasDgvComprasrecibido.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
                    if (ComprasDgvComprasrecibido.Columns.Contains("CantidadRecibida"))
                        ComprasDgvComprasrecibido.Columns["CantidadRecibida"].DefaultCellStyle.Format = "0.##";

                    // Ocultar columnas que el usuario no necesita ver
                    if (ComprasDgvComprasrecibido.Columns.Contains("Linea"))
                        ComprasDgvComprasrecibido.Columns["Linea"].Visible = false;
                    if (ComprasDgvComprasrecibido.Columns.Contains("IdProducto"))
                        ComprasDgvComprasrecibido.Columns["IdProducto"].Visible = false;
                    if (ComprasDgvComprasrecibido.Columns.Contains("IdBodega"))
                        ComprasDgvComprasrecibido.Columns["IdBodega"].Visible = false;
                    if (ComprasDgvComprasrecibido.Columns.Contains("Descuento"))
                        ComprasDgvComprasrecibido.Columns["Descuento"].Visible = false;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al cargar los datos del visor: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // Eventos Visuales
        // =========================================================

        

        // Evento del botón de salir de la imagen
        private void ComprasBtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComprasCboBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComprasCboBodega.SelectedValue != null && _DetallesCompraBase != null)
            {
                string valorSeleccionado = ComprasCboBodega.SelectedValue.ToString();

                // ESCUDO: Solo intentamos filtrar si el valor es un número entero real
                if (int.TryParse(valorSeleccionado, out int idBodegaSeleccionada))
                {
                    _DetallesCompraBase.DefaultView.RowFilter = $"IdBodega = {idBodegaSeleccionada}";
                }
            }
        }
    }
}