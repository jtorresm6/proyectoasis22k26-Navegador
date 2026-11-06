
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaContralador_Enlace_Logistica;
using CapaModelo_Enlace_Logistica.Entidades;

namespace CapaVista_Enlace_Logistica.Formas
{
    public partial class Frm_Solicitud_Pago : Form
    {
        // ==========================================
        // ATRIBUTOS
        // ==========================================

        private readonly ClsSolicitudPagoControlador
            controladorSolicitudPago;

        private List<ClsServicioTransporte>
            serviciosDisponibles;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public Frm_Solicitud_Pago()
        {
            InitializeComponent();

            controladorSolicitudPago =
                new ClsSolicitudPagoControlador();

            serviciosDisponibles =
                new List<ClsServicioTransporte>();

            ConfigurarEventos();
            InicializarFormulario();
        }

        // ==========================================
        // CONFIGURACION DE EVENTOS
        // ==========================================

        private void ConfigurarEventos()
        {
            LogisticaCboServicioTransporte.SelectionChangeCommitted
                += LogisticaCboServicioTransporte_SelectionChangeCommitted;

            LogisticaCboBancoBeneficiario.SelectedIndexChanged
                += ActualizarResumen_Evento;

            LogisticaCboTipoCuenta.SelectedIndexChanged
                += ActualizarResumen_Evento;

            LogisticaTxtNumeroCuenta.TextChanged
                += ActualizarResumen_Evento;

            LogisticaTxtConceptoPago.TextChanged
                += ActualizarResumen_Evento;

            LogisticaTxtMontoPagar.TextChanged
                += ActualizarResumen_Evento;

            LogisticaRdoQuetzales.CheckedChanged
                += Moneda_CheckedChanged;

            LogisticaRdoDolares.CheckedChanged
                += Moneda_CheckedChanged;

            LogisticaBtnLimpiar.Click
                += LogisticaBtnLimpiar_Click;
        }

        // ==========================================
        // INICIALIZAR FORMULARIO
        // ==========================================

        private void InicializarFormulario()
        {
            LimpiarFormulario();
            CargarServiciosTransporte();
        }

        // ==========================================
        // CARGAR SERVICIOS DE TRANSPORTE
        // ==========================================

        private void CargarServiciosTransporte()
        {
            try
            {
                serviciosDisponibles =
                    controladorSolicitudPago
                        .ObtenerServiciosDisponibles()
                        .ToList();

                LogisticaCboServicioTransporte.DataSource = null;

                LogisticaCboServicioTransporte.DisplayMember =
                    "NumeroGuia";

                LogisticaCboServicioTransporte.ValueMember =
                    "IdServicio";

                LogisticaCboServicioTransporte.DataSource =
                    serviciosDisponibles;

                LogisticaCboServicioTransporte.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar los servicios de transporte: "
                    + ex.Message,
                    "Solicitud de Pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ==========================================
        // EVENTO SELECCIONAR SERVICIO
        // ==========================================

        private void LogisticaCboServicioTransporte_SelectionChangeCommitted(
            object sender,
            EventArgs e)
        {
            ClsServicioTransporte servicio =
                LogisticaCboServicioTransporte.SelectedItem
                    as ClsServicioTransporte;

            if (servicio == null)
            {
                LimpiarDatosServicio();
                return;
            }

            MostrarDatosServicio(servicio);
        }

        // ==========================================
        // MOSTRAR DATOS DEL SERVICIO
        // ==========================================

        private void MostrarDatosServicio(
            ClsServicioTransporte servicio)
        {
            // Datos del servicio.
            LogisticaTxtNumeroServicio.Text =
                servicio.IdServicio.ToString();

            LogisticaTxtNumeroGuia.Text =
                servicio.NumeroGuia ?? string.Empty;

            LogisticaTxtFechaServicio.Text =
                servicio.FechaServicio.HasValue
                    ? servicio.FechaServicio.Value.ToString("dd/MM/yyyy")
                    : string.Empty;

            // Datos del transportista.
            LogisticaTxtNombreTransportista.Text =
                servicio.NombreTransportista ?? string.Empty;

            LogisticaTxtNitTransportista.Text =
                servicio.NitTransportista ?? string.Empty;

            LogisticaTxtDireccionTransportista.Text =
                servicio.DireccionTransportista ?? string.Empty;

            // Mostrar detalle.
            MostrarDetalleServicio(servicio);

            // Datos del pago.
            LogisticaTxtMontoPagar.Text =
                servicio.MontoTotal.ToString("N2");

            LogisticaTxtConceptoPago.Text =
                "Pago por servicio de transporte: "
                + (servicio.TipoServicio ?? string.Empty);

            // Moneda del servicio.
            bool esDolar = string.Equals(
                servicio.Moneda,
                "USD",
                StringComparison.OrdinalIgnoreCase
            );

            LogisticaRdoDolares.Checked = esDolar;
            LogisticaRdoQuetzales.Checked = !esDolar;

            ActualizarMoneda();
            ActualizarResumenSolicitud();
        }

        // ==========================================
        // MOSTRAR DETALLE DEL SERVICIO
        // ==========================================

        private void MostrarDetalleServicio(
            ClsServicioTransporte servicio)
        {
            LimpiarDetalleServicio();

            LogisticaDgvDetalleServicio.Rows.Add(
                "1",
                servicio.TipoServicio ?? string.Empty,
                servicio.Cantidad.ToString("N4"),
                servicio.CostoUnitario.ToString("N2"),
                servicio.MontoTotal.ToString("N2")
            );
        }

        // ==========================================
        // LIMPIAR DETALLE
        // ==========================================

        private void LimpiarDetalleServicio()
        {
            if (LogisticaDgvDetalleServicio.DataSource != null)
            {
                LogisticaDgvDetalleServicio.DataSource = null;
            }

            LogisticaDgvDetalleServicio.Rows.Clear();
        }

        // ==========================================
        // LIMPIAR DATOS DEL SERVICIO
        // ==========================================

        private void LimpiarDatosServicio()
        {
            LogisticaTxtNumeroServicio.Clear();
            LogisticaTxtNumeroGuia.Clear();
            LogisticaTxtFechaServicio.Clear();

            LogisticaTxtNombreTransportista.Clear();
            LogisticaTxtNitTransportista.Clear();
            LogisticaTxtDireccionTransportista.Clear();

            LimpiarDetalleServicio();

            LogisticaTxtMontoPagar.Clear();
            LogisticaTxtConceptoPago.Clear();

            ActualizarResumenSolicitud();
        }

        // ==========================================
        // LIMPIAR FORMULARIO
        // ==========================================

        private void LimpiarFormulario()
        {
            // Deseleccionar guia sin borrar los servicios.
            LogisticaCboServicioTransporte.SelectedIndex = -1;

            // Limpiar informacion del servicio.
            LimpiarDatosServicio();

            // Limpiar datos bancarios.
            LogisticaCboBancoBeneficiario.SelectedIndex = -1;
            LogisticaCboTipoCuenta.SelectedIndex = -1;

            LogisticaTxtNumeroCuenta.Clear();

            // Restablecer moneda.
            LogisticaRdoQuetzales.Checked = true;
            LogisticaRdoDolares.Checked = false;

            ActualizarMoneda();
            ActualizarResumenSolicitud();
        }

        // ==========================================
        // BOTON LIMPIAR
        // ==========================================

        private void LogisticaBtnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarFormulario();

            LogisticaCboServicioTransporte.Focus();
        }

        // ==========================================
        // MONEDA
        // ==========================================

        private void Moneda_CheckedChanged(
            object sender,
            EventArgs e)
        {
            ActualizarMoneda();
            ActualizarResumenSolicitud();
        }

        private void ActualizarMoneda()
        {
            LogisticaLblMonedaMonto.Text =
                LogisticaRdoDolares.Checked ? "USD" : "GTQ";
        }

        // ==========================================
        // ACTUALIZAR RESUMEN
        // ==========================================

        private void ActualizarResumen_Evento(
            object sender,
            EventArgs e)
        {
            ActualizarResumenSolicitud();
        }

        private void ActualizarResumenSolicitud()
        {
            LogisticaLblTextoResumenNumeroServicio.Text =
                ObtenerTextoResumen(
                    LogisticaTxtNumeroServicio.Text
                );

            LogisticaLblTextoResumenFechaServicio.Text =
                ObtenerTextoResumen(
                    LogisticaTxtFechaServicio.Text
                );

            LogisticaLblTextoResumenTransportista.Text =
                ObtenerTextoResumen(
                    LogisticaTxtNombreTransportista.Text
                );

            LogisticaLblTextoResumenNit.Text =
                ObtenerTextoResumen(
                    LogisticaTxtNitTransportista.Text
                );

            LogisticaLblTextoResumenBanco.Text =
                ObtenerTextoResumen(
                    LogisticaCboBancoBeneficiario.SelectedIndex >= 0
                        ? LogisticaCboBancoBeneficiario.Text
                        : string.Empty
                );

            LogisticaLblTextoResumenTipoCuenta.Text =
                ObtenerTextoResumen(
                    LogisticaCboTipoCuenta.SelectedIndex >= 0
                        ? LogisticaCboTipoCuenta.Text
                        : string.Empty
                );

            LogisticaLblTextoResumenNumeroCuenta.Text =
                ObtenerTextoResumen(
                    LogisticaTxtNumeroCuenta.Text
                );

            LogisticaLblTextoResumenMoneda.Text =
                LogisticaLblMonedaMonto.Text;

            LogisticaLblTextoResumenMonto.Text =
                ObtenerTextoResumen(
                    LogisticaTxtMontoPagar.Text
                );

            LogisticaLblTextoResumenConcepto.Text =
                ObtenerTextoResumen(
                    LogisticaTxtConceptoPago.Text
                );
        }

        // ==========================================
        // OBTENER TEXTO DEL RESUMEN
        // ==========================================

        private string ObtenerTextoResumen(string texto)
        {
            return string.IsNullOrWhiteSpace(texto)
                ? "-"
                : texto.Trim();
        }

        // ==========================================
        // EVENTOS EXISTENTES DEL DESIGNER
        // ==========================================

        private void pictureBox1_Click(
            object sender, EventArgs e)
        {
        }

        private void pnlFondoSolicitudPago_Paint(
            object sender, PaintEventArgs e)
        {
        }

        private void label3_Click(
            object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(
            object sender, EventArgs e)
        {
        }

        private void lblMoneda_Click(
            object sender, EventArgs e)
        {
        }

        private void label1_Click(
            object sender, EventArgs e)
        {
        }

        private void pnlInformacionPago_Paint(
            object sender, PaintEventArgs e)
        {
        }

        private void btnSeleccionarDocumentos_Click(
            object sender, EventArgs e)
        {
            // Pendiente de implementar documentos opcionales.
        }

        private void lblDocumentosRespaldo_Click(
            object sender, EventArgs e)
        {
        }

        private void pnlResumenSolicitud_Paint(
            object sender, PaintEventArgs e)
        {
        }

        private void lblResumenNit_Click(
            object sender, EventArgs e)
        {
        }

        private void lblResumenBanco_Click(
            object sender, EventArgs e)
        {
        }

        private void lblResumenTipoCuenta_Click(
            object sender, EventArgs e)
        {
        }

        private void lblResumenNumeroCuenta_Click(
            object sender, EventArgs e)
        {
        }

        private void lblResumenTransportista_Click(
            object sender, EventArgs e)
        {
        }

        private void lblResumenMoneda_Click(
            object sender, EventArgs e)
        {
        }

        private void lblResumenMonto_Click(
            object sender, EventArgs e)
        {
        }

        private void lblResumenConcepto_Click(
            object sender, EventArgs e)
        {
        }

        private void LogisticaBtnLimpiar_Click_1(object sender, EventArgs e)
        {

        }
    }
}
