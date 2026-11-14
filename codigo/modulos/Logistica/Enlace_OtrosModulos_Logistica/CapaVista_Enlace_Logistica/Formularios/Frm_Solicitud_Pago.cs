
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
using System.IO;
using System.Globalization;
using System.Reflection;
using CapaVista_Reporteador.Formas;

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

            LogisticaPbAyuda.Click += (sender, e) =>
                 LogisticaMetMostrarAyuda();

            // El botón Limpiar se conecta desde el diseñador.
            // No se suscribe nuevamente para evitar dos ejecuciones.
        }

        private void LimpiarFormulario()
        {
            // 1. Deseleccionar el servicio de transporte.
            // Las guías cargadas permanecen disponibles.
            LogisticaCboServicioTransporte.SelectedIndex = -1;

            // 2. Limpiar información del servicio.
            LogisticaTxtNumeroServicio.Clear();
            LogisticaTxtNumeroGuia.Clear();
            LogisticaTxtFechaServicio.Clear();

            // 3. Limpiar información del transportista.
            LogisticaTxtNombreTransportista.Clear();
            LogisticaTxtNitTransportista.Clear();
            LogisticaTxtDireccionTransportista.Clear();

            // 4. Limpiar información bancaria.
            LogisticaCboBancoBeneficiario.SelectedIndex = -1;
            LogisticaCboTipoCuenta.SelectedIndex = -1;
            LogisticaTxtNumeroCuenta.Clear();

            // 5. Limpiar detalle del servicio.
            if (LogisticaDgvDetalleServicio.DataSource != null)
            {
                LogisticaDgvDetalleServicio.DataSource = null;
            }

            LogisticaDgvDetalleServicio.Rows.Clear();

            // 6. Limpiar información del pago.
            LogisticaTxtMontoPagar.Clear();
            LogisticaTxtConceptoPago.Clear();

            // 7. Restablecer moneda predeterminada.
            LogisticaRdoQuetzales.Checked = true;
            LogisticaRdoDolares.Checked = false;
            LogisticaLblMonedaMonto.Text = "GTQ";

            // 8. Actualizar el resumen.
            ActualizarResumenSolicitud();

            // 9. Regresar el cursor a la selección del servicio.
            LogisticaCboServicioTransporte.Focus();
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
        // MOSTRAR VISTA PREVIA
        // ==========================================

        private void MostrarVistaPrevia()
        {
            ClsServicioTransporte servicio =
                LogisticaCboServicioTransporte.SelectedItem
                    as ClsServicioTransporte;

            if (servicio == null)
            {
                MessageBox.Show(
                    "Seleccione un servicio de transporte.",
                    "Vista Previa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                LogisticaCboServicioTransporte.Focus();
                return;
            }

            decimal montoTotal;

            if (!ObtenerDecimalVistaPrevia(
                LogisticaTxtMontoPagar.Text, out montoTotal))
            {
                MessageBox.Show(
                    "El monto a pagar no tiene un formato válido.",
                    "Vista Previa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                LogisticaTxtMontoPagar.Focus();
                return;
            }

            try
            {
                string rutaReporte = ObtenerRutaReporteSolicitudPago();

                if (!File.Exists(rutaReporte))
                {
                    MessageBox.Show(
                        "No se encontró el reporte:\n" + rutaReporte +
                        "\n\nVerifique que el RDLC tenga la propiedad " +
                        "'Copiar si es posterior'.",
                        "Vista Previa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                ClsSolicitudPagoVistaPrevia solicitud =
                    PrepararDatosVistaPrevia(servicio, montoTotal);

                List<ClsSolicitudPagoVistaPrevia> datosReporte =
                    new List<ClsSolicitudPagoVistaPrevia>();

                datosReporte.Add(solicitud);

                Dictionary<string, object> fuentesDeDatos =
                    new Dictionary<string, object>();

                fuentesDeDatos.Add(
                    "DsSolicitudPago",
                    datosReporte
                );

                using (FrmVistaReportes formularioReporte =
                    new FrmVistaReportes(
                        rutaReporte,
                        fuentesDeDatos))
                {
                    formularioReporte.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No fue posible mostrar la vista previa:\n" +
                    ex.Message,
                    "Vista Previa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string ObtenerRutaReporteSolicitudPago()
        {
            Assembly ensamblado = typeof(Frm_Solicitud_Pago).Assembly;

            string nombreRecurso =
                "CapaVista_Enlace_Logistica.Reportes.SolicitudPagoTransporte.rdlc";

            using (Stream recurso =
                ensamblado.GetManifestResourceStream(nombreRecurso))
            {
                if (recurso == null)
                {
                    throw new FileNotFoundException(
                        "No se encontró el recurso RDLC: " + nombreRecurso);
                }

                string rutaTemporal = Path.Combine(
                    Path.GetTempPath(),
                    "SolicitudPagoTransporte_" +
                    Guid.NewGuid().ToString("N") + ".rdlc"
                );

                using (FileStream archivo = File.Create(rutaTemporal))
                {
                    recurso.CopyTo(archivo);
                }

                return rutaTemporal;
            }
        }

        // ==========================================
        // PREPARAR DATOS DEL REPORTE
        // ==========================================

        private ClsSolicitudPagoVistaPrevia PrepararDatosVistaPrevia(
            ClsServicioTransporte servicio,
            decimal montoTotal)
        {
            ClsSolicitudPagoVistaPrevia solicitud =
                new ClsSolicitudPagoVistaPrevia();

            solicitud.IdServicio = servicio.IdServicio;

            solicitud.NumeroGuia =
                LogisticaTxtNumeroGuia.Text.Trim();

            solicitud.FechaServicio =
                LogisticaTxtFechaServicio.Text.Trim();

            solicitud.NombreTransportista =
                LogisticaTxtNombreTransportista.Text.Trim();

            solicitud.NitTransportista =
                LogisticaTxtNitTransportista.Text.Trim();

            solicitud.DireccionTransportista =
                LogisticaTxtDireccionTransportista.Text.Trim();

            solicitud.BancoBeneficiario =
                LogisticaCboBancoBeneficiario.SelectedIndex >= 0
                    ? LogisticaCboBancoBeneficiario.Text
                    : "Pendiente";

            solicitud.TipoCuenta =
                LogisticaCboTipoCuenta.SelectedIndex >= 0
                    ? LogisticaCboTipoCuenta.Text
                    : "Pendiente";

            solicitud.NumeroCuenta =
                string.IsNullOrWhiteSpace(
                    LogisticaTxtNumeroCuenta.Text)
                    ? "Pendiente"
                    : LogisticaTxtNumeroCuenta.Text.Trim();

            solicitud.Moneda =
                LogisticaRdoDolares.Checked
                    ? "USD"
                    : "GTQ";

            solicitud.MontoTotal = montoTotal;

            solicitud.ConceptoPago =
                LogisticaTxtConceptoPago.Text.Trim();

            solicitud.DescripcionServicio =
                servicio.TipoServicio ?? string.Empty;

            solicitud.Cantidad = servicio.Cantidad;

            solicitud.CostoUnitario = servicio.CostoUnitario;

            
            foreach (DataGridViewRow fila in
                LogisticaDgvDetalleServicio.Rows)
            {
                if (fila.IsNewRow)
                {
                    continue;
                }

                solicitud.DescripcionServicio =
                    Convert.ToString(fila.Cells[1].Value)
                    ?? string.Empty;

                decimal cantidad;
                decimal costoUnitario;

                if (ObtenerDecimalVistaPrevia(
                    Convert.ToString(fila.Cells[2].Value),
                    out cantidad))
                {
                    solicitud.Cantidad = cantidad;
                }

                if (ObtenerDecimalVistaPrevia(
                    Convert.ToString(fila.Cells[3].Value),
                    out costoUnitario))
                {
                    solicitud.CostoUnitario = costoUnitario;
                }

                // Actualmente el formulario genera una sola fila.
                break;
            }

            return solicitud;
        }

        // ==========================================
        // CONVERTIR VALORES DECIMALES
        // ==========================================

        private bool ObtenerDecimalVistaPrevia(
            string texto,
            out decimal valor)
        {
            valor = 0m;

            if (string.IsNullOrWhiteSpace(texto))
            {
                return false;
            }

            NumberStyles estilo =
                NumberStyles.Number;

            if (decimal.TryParse(
                texto.Trim(),
                estilo,
                CultureInfo.CurrentCulture,
                out valor))
            {
                return true;
            }

            return decimal.TryParse(
                texto.Trim(),
                estilo,
                CultureInfo.InvariantCulture,
                out valor
            );
        }

        // Mostrar Ayudas//



        private void LogisticaMetMostrarAyuda()
        {
            DirectoryInfo directorioActual =
                new DirectoryInfo(
                    AppDomain.CurrentDomain.BaseDirectory);

            while (directorioActual != null)
            {
                string rutaAyuda = Path.Combine(
                    directorioActual.FullName,
                    "ayuda",
                    "modulos",
                    "logistica",
                    "SolicitudPagoAyuda_Logistica",
                    "AyudaLogistica.chm"
                );

                if (File.Exists(rutaAyuda))
                {
                    Help.ShowHelp(
                        this,
                        rutaAyuda,
                        "Solicitud_Pago.html"
                    );

                    return;
                }

                directorioActual = directorioActual.Parent;
            }

            MessageBox.Show(
                "No se encontró el archivo AyudaLogistica.chm.",
                "Ayuda de Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
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
            LimpiarFormulario();
        }

        private void LogisticaBtnVistaPrevia_Click(object sender, EventArgs e)
        {
            MostrarVistaPrevia();
        }

        private void LogisticaPbAyuda_Click(object sender, EventArgs e)
        {
            LogisticaMetMostrarAyuda();
        }
    }
}
