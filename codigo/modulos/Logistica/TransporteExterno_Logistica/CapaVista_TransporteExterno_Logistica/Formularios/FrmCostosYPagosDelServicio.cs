
// ------------------------------------------------------
// Autor: Gabriela Pinto García
// Módulo: Logística - Transporte Externo
// Formulario: Costos y Pagos del Servicio
// ------------------------------------------------------

using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using CapaControlador_TransporteExterno_Logistica;

namespace CapaVista_TransporteExterno_Logistica.Formularios
{
    public partial class FrmCostosYPagosDelServicio : Form
    {
        private ClsControladorCostosTransporte
            LogisticaObjControlador =
            new ClsControladorCostosTransporte();

        private decimal LogisticaDblCostoUnitario = 0m;
        private decimal LogisticaDblCostoTotal = 0m;

        private decimal LogisticaDblTipoCambio = 7.80m;

        private bool LogisticaBlnCargandoDatos = false;

        // ------------------------------------------------------
        // CONSTRUCTOR
        // ------------------------------------------------------
        public FrmCostosYPagosDelServicio()
        {
            InitializeComponent();

            LogisticaMetConfigurarFormulario();
            LogisticaMetAsignarEventos();

            LogisticaMetCargarGuias();
            LogisticaMetCargarTarifas();
        }

        // ------------------------------------------------------
        // CONFIGURAR FORMULARIO
        // ------------------------------------------------------
        private void LogisticaMetConfigurarFormulario()
        {
            LogisticaCboNumeroGuia.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboTarifa.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboTipoPago.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboMoneda.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaTxtCantidad.Clear();

            LogisticaLblCantidad.Text = "Kilómetros recorridos:";

            LogisticaLblCostoUnitario1.Text = "Q 0.00";
            LogisticaLblCostoTotal1.Text = "Q 0.00";

            LogisticaCboNumeroGuia.SelectedIndex = -1;
            LogisticaCboTarifa.SelectedIndex = -1;
            LogisticaCboTipoPago.SelectedIndex = -1;

            if (LogisticaCboMoneda.Items.Count > 0)
                LogisticaCboMoneda.SelectedIndex = 0;

            LogisticaMetHabilitarCampos(false);
        }

        // ------------------------------------------------------
        // ASIGNAR EVENTOS
        // ------------------------------------------------------
        private void LogisticaMetAsignarEventos()
        {
            LogisticaBtnNuevoCosto.Click -=
                LogisticaBtnNuevoCosto_Click;
            LogisticaBtnNuevoCosto.Click +=
                LogisticaBtnNuevoCosto_Click;

            LogisticaBtnGuardarCosto.Click -=
                LogisticaBtnGuardarCosto_Click;
            LogisticaBtnGuardarCosto.Click +=
                LogisticaBtnGuardarCosto_Click;

            LogisticaBtnModificarCosto.Click -=
                LogisticaBtnModificarCosto_Click;
            LogisticaBtnModificarCosto.Click +=
                LogisticaBtnModificarCosto_Click;

            LogisticaBtnEliminarCosto.Click -=
                LogisticaBtnEliminarCosto_Click;
            LogisticaBtnEliminarCosto.Click +=
                LogisticaBtnEliminarCosto_Click;

            LogisticaCboNumeroGuia.SelectedIndexChanged -=
                LogisticaCboNumeroGuia_SelectedIndexChanged;
            LogisticaCboNumeroGuia.SelectedIndexChanged +=
                LogisticaCboNumeroGuia_SelectedIndexChanged;

            LogisticaCboTarifa.SelectedIndexChanged -=
                LogisticaCboTarifa_SelectedIndexChanged;
            LogisticaCboTarifa.SelectedIndexChanged +=
                LogisticaCboTarifa_SelectedIndexChanged;

            LogisticaTxtCantidad.TextChanged -=
                LogisticaTxtCantidad_TextChanged;
            LogisticaTxtCantidad.TextChanged +=
                LogisticaTxtCantidad_TextChanged;

            LogisticaCboMoneda.SelectedIndexChanged -=
                LogisticaCboMoneda_SelectedIndexChanged;
            LogisticaCboMoneda.SelectedIndexChanged +=
                LogisticaCboMoneda_SelectedIndexChanged;
        }

        // ------------------------------------------------------
        // HABILITAR CAMPOS
        // ------------------------------------------------------
        private void LogisticaMetHabilitarCampos(bool habilitar)
        {
            LogisticaCboTarifa.Enabled = habilitar;
            LogisticaCboTipoPago.Enabled = habilitar;
            LogisticaCboMoneda.Enabled = habilitar;
            LogisticaTxtCantidad.Enabled = habilitar;
        }

        // ------------------------------------------------------
        // CARGAR GUÍAS
        // ------------------------------------------------------
        private void LogisticaMetCargarGuias()
        {
            try
            {
                LogisticaBlnCargandoDatos = true;

                DataTable tabla =
                    LogisticaObjControlador.LogisticaMetObtenerGuias();

                LogisticaCboNumeroGuia.DataSource = null;
                LogisticaCboNumeroGuia.DisplayMember =
                    "numeroGuiaServicioTransporteExterno";
                LogisticaCboNumeroGuia.ValueMember =
                    "idServicioTransporteExterno";
                LogisticaCboNumeroGuia.DataSource = tabla;
                LogisticaCboNumeroGuia.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar las guías: " + ex.Message,
                    "Costos de transporte",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                LogisticaBlnCargandoDatos = false;
            }
        }

        // ------------------------------------------------------
        // CARGAR TARIFAS
        // ------------------------------------------------------
        private void LogisticaMetCargarTarifas()
        {
            try
            {
                LogisticaBlnCargandoDatos = true;

                DataTable tabla =
                    LogisticaObjControlador.LogisticaMetObtenerTarifas();

                LogisticaCboTarifa.DataSource = null;
                LogisticaCboTarifa.DisplayMember =
                    "nombreTarifaTransporte";
                LogisticaCboTarifa.ValueMember =
                    "idTarifaTransporte";
                LogisticaCboTarifa.DataSource = tabla;
                LogisticaCboTarifa.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar las tarifas: " + ex.Message,
                    "Costos de transporte",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                LogisticaBlnCargandoDatos = false;
            }
        }

        // ------------------------------------------------------
        // OBTENER COSTO POR KILÓMETRO
        // ------------------------------------------------------
        private decimal LogisticaMetObtenerCostoPorKilometro(int idTarifa)
        {
            switch (idTarifa)
            {
                case 1: return 12.00m;
                case 2: return 18.00m;
                case 3: return 10.00m;
                case 4: return 13.00m;
                case 5: return 20.00m;
                case 6: return 11.00m;
                case 7: return 15.00m;
                case 8: return 22.00m;
                case 9: return 12.00m;
                case 10: return 16.00m;
                default: return 0m;
            }
        }

        // ------------------------------------------------------
        // OBTENER ID DE TARIFA
        // ------------------------------------------------------
        private int LogisticaMetObtenerIdTarifa()
        {
            if (LogisticaCboTarifa.SelectedValue == null ||
                LogisticaCboTarifa.SelectedValue is DataRowView)
                return 0;

            int idTarifa;

            if (int.TryParse(
                Convert.ToString(LogisticaCboTarifa.SelectedValue),
                out idTarifa))
                return idTarifa;

            return 0;
        }

        // ------------------------------------------------------
        // EVENTO: CAMBIO DE TARIFA
        // ------------------------------------------------------
        private void LogisticaCboTarifa_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (LogisticaBlnCargandoDatos)
                return;

            LogisticaMetCalcularCosto();
        }

        // ------------------------------------------------------
        // EVENTO: CAMBIO DE KILÓMETROS
        // ------------------------------------------------------
        private void LogisticaTxtCantidad_TextChanged(
            object sender, EventArgs e)
        {
            if (LogisticaBlnCargandoDatos)
                return;

            LogisticaMetCalcularCosto();
        }

        // ------------------------------------------------------
        // EVENTO: CAMBIO DE MONEDA
        // ------------------------------------------------------
        private void LogisticaCboMoneda_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (LogisticaBlnCargandoDatos)
                return;

            LogisticaMetMostrarCostos();
        }

        // ------------------------------------------------------
        // CALCULAR COSTO
        // ------------------------------------------------------
        private void LogisticaMetCalcularCosto()
        {
            int idTarifa = LogisticaMetObtenerIdTarifa();

            decimal kilometros;

            LogisticaDblCostoUnitario =
                LogisticaMetObtenerCostoPorKilometro(idTarifa);

            if (LogisticaDblCostoUnitario <= 0 ||
                !decimal.TryParse(
                    LogisticaTxtCantidad.Text,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out kilometros) ||
                kilometros <= 0)
            {
                LogisticaDblCostoTotal = 0m;
                LogisticaMetMostrarCostos();
                return;
            }

            LogisticaDblCostoTotal =
                kilometros * LogisticaDblCostoUnitario;

            LogisticaMetMostrarCostos();
        }

        // ------------------------------------------------------
        // MOSTRAR COSTOS
        // ------------------------------------------------------
        private void LogisticaMetMostrarCostos()
        {
            decimal costoUnitario = LogisticaDblCostoUnitario;
            decimal costoTotal = LogisticaDblCostoTotal;

            string simbolo = "Q";

            if (LogisticaCboMoneda.SelectedIndex == 1)
            {
                simbolo = "$";

                costoUnitario /= LogisticaDblTipoCambio;
                costoTotal /= LogisticaDblTipoCambio;
            }

            LogisticaLblCostoUnitario1.Text =
                simbolo + " " + costoUnitario.ToString("N2");

            LogisticaLblCostoTotal1.Text =
                simbolo + " " + costoTotal.ToString("N2");
        }

        // ------------------------------------------------------
        // OBTENER COSTO UNITARIO PARA BASE DE DATOS
        // ------------------------------------------------------
        private decimal LogisticaMetObtenerCostoUnitarioBD()
        {
            decimal costo = LogisticaDblCostoUnitario;

            if (LogisticaCboMoneda.SelectedIndex == 1)
                costo /= LogisticaDblTipoCambio;

            return Math.Round(costo, 4);
        }

        // ------------------------------------------------------
        // OBTENER MONEDA
        // ------------------------------------------------------
        private string LogisticaMetObtenerMoneda()
        {
            if (LogisticaCboMoneda.SelectedIndex == 0)
                return "GTQ";

            if (LogisticaCboMoneda.SelectedIndex == 1)
                return "USD";

            return "";
        }

        // ------------------------------------------------------
        // OBTENER NÚMERO DE GUÍA
        // ------------------------------------------------------
        private string LogisticaMetObtenerNumeroGuia()
        {
            if (LogisticaCboNumeroGuia.SelectedIndex < 0)
                return "";

            return LogisticaCboNumeroGuia.Text.Trim();
        }

        // ------------------------------------------------------
        // VALIDAR CAMPOS
        // ------------------------------------------------------
        private bool LogisticaMetValidarCampos()
        {
            if (LogisticaCboNumeroGuia.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Seleccione una guía de transporte.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (LogisticaMetObtenerIdTarifa() <= 0)
            {
                MessageBox.Show(
                    "Seleccione una tarifa.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            decimal kilometros;

            if (!decimal.TryParse(
                LogisticaTxtCantidad.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out kilometros) || kilometros <= 0)
            {
                MessageBox.Show(
                    "Ingrese una cantidad válida de kilómetros.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                LogisticaTxtCantidad.Focus();
                return false;
            }

            if (LogisticaCboMoneda.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Seleccione una moneda.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (LogisticaCboTipoPago.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Seleccione el tipo de pago.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (LogisticaDblCostoUnitario <= 0)
            {
                MessageBox.Show(
                    "No se pudo obtener el costo por kilómetro.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ------------------------------------------------------
        // CONSULTAR COSTO
        // ------------------------------------------------------
        private void LogisticaMetConsultarCosto()
        {
            string guia = LogisticaMetObtenerNumeroGuia();

            if (string.IsNullOrWhiteSpace(guia))
                return;

            try
            {
                DataTable tabla =
                    LogisticaObjControlador
                    .LogisticaMetObtenerCostoPorGuia(guia);

                if (tabla.Rows.Count > 0)
                {
                    LogisticaMetMostrarCostoRegistrado(tabla.Rows[0]);
                }
                else
                {
                    LogisticaMetLimpiarDatosCosto();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar el costo: " + ex.Message,
                    "Costos de transporte",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ------------------------------------------------------
        // MOSTRAR COSTO REGISTRADO
        // ------------------------------------------------------
        private void LogisticaMetMostrarCostoRegistrado(DataRow fila)
        {
            LogisticaBlnCargandoDatos = true;

            try
            {
                LogisticaCboTarifa.SelectedValue =
                    Convert.ToInt32(
                        fila["Fk_Tarifa_Transporte_Codigo"]);

                string moneda =
                    Convert.ToString(fila["monedaCostoTransporte"]);

                LogisticaCboMoneda.SelectedIndex =
                    moneda == "USD" ? 1 : 0;

                string tipoPago =
                    Convert.ToString(fila["tipoPagoCostoTransporte"]);

                LogisticaCboTipoPago.SelectedItem = tipoPago;

                LogisticaTxtCantidad.Text =
                    Convert.ToDecimal(
                        fila["cantidadCostoTransporte"])
                    .ToString(CultureInfo.CurrentCulture);

                LogisticaDblCostoUnitario =
                    Convert.ToDecimal(
                        fila["costoUnitarioCostoTransporte"]);

                LogisticaDblCostoTotal =
                    Convert.ToDecimal(
                        fila["costoTotalCostoTransporte"]);

                if (moneda == "USD")
                {
                    LogisticaDblCostoUnitario *=
                        LogisticaDblTipoCambio;

                    LogisticaDblCostoTotal *=
                        LogisticaDblTipoCambio;
                }

                LogisticaMetHabilitarCampos(true);
            }
            finally
            {
                LogisticaBlnCargandoDatos = false;
            }

            LogisticaMetMostrarCostos();
        }

        // ------------------------------------------------------
        // LIMPIAR COSTO
        // ------------------------------------------------------
        private void LogisticaMetLimpiarDatosCosto()
        {
            LogisticaBlnCargandoDatos = true;

            try
            {
                LogisticaCboTarifa.SelectedIndex = -1;
                LogisticaCboTipoPago.SelectedIndex = -1;
                LogisticaTxtCantidad.Clear();

                LogisticaDblCostoUnitario = 0m;
                LogisticaDblCostoTotal = 0m;
            }
            finally
            {
                LogisticaBlnCargandoDatos = false;
            }

            LogisticaMetMostrarCostos();
        }

        // ------------------------------------------------------
        // EVENTO: SELECCIONAR GUÍA
        // ------------------------------------------------------
        private void LogisticaCboNumeroGuia_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (LogisticaBlnCargandoDatos)
                return;

            if (LogisticaCboNumeroGuia.SelectedIndex < 0)
                return;

            LogisticaMetHabilitarCampos(true);
            LogisticaMetConsultarCosto();
        }

        // ------------------------------------------------------
        // BOTÓN NUEVO
        // ------------------------------------------------------
        private void LogisticaBtnNuevoCosto_Click(
            object sender, EventArgs e)
        {
            LogisticaMetLimpiarDatosCosto();

            LogisticaCboNumeroGuia.SelectedIndex = -1;

            LogisticaMetHabilitarCampos(true);

            LogisticaCboNumeroGuia.Focus();
        }

        // ------------------------------------------------------
        // BOTÓN GUARDAR
        // ------------------------------------------------------
        private void LogisticaBtnGuardarCosto_Click(
            object sender, EventArgs e)
        {
            if (!LogisticaMetValidarCampos())
                return;

            try
            {
                string guia = LogisticaMetObtenerNumeroGuia();

                decimal kilometros;

                if (!decimal.TryParse(
                    LogisticaTxtCantidad.Text,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out kilometros))
                    return;

                bool guardado =
                    LogisticaObjControlador.LogisticaMetGuardarCosto(
                        guia,
                        LogisticaMetObtenerIdTarifa(),
                        kilometros,
                        LogisticaMetObtenerCostoUnitarioBD(),
                        LogisticaMetObtenerMoneda(),
                        LogisticaCboTipoPago.Text);

                if (guardado)
                {
                    MessageBox.Show(
                        "El costo del servicio se guardó correctamente.",
                        "Registro exitoso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LogisticaMetConsultarCosto();
                }
                else
                {
                    MessageBox.Show(
                        "No se pudo guardar. Verifique si la guía ya tiene un costo registrado.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al guardar el costo: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ------------------------------------------------------
        // BOTÓN MODIFICAR
        // ------------------------------------------------------
        private void LogisticaBtnModificarCosto_Click(
            object sender, EventArgs e)
        {
            if (!LogisticaMetValidarCampos())
                return;

            try
            {
                string guia = LogisticaMetObtenerNumeroGuia();

                decimal kilometros;

                if (!decimal.TryParse(
                    LogisticaTxtCantidad.Text,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out kilometros))
                    return;

                bool modificado =
                    LogisticaObjControlador.LogisticaMetModificarCosto(
                        guia,
                        LogisticaMetObtenerIdTarifa(),
                        kilometros,
                        LogisticaMetObtenerCostoUnitarioBD(),
                        LogisticaMetObtenerMoneda(),
                        LogisticaCboTipoPago.Text);

                if (modificado)
                {
                    MessageBox.Show(
                        "El costo del servicio se modificó correctamente.",
                        "Modificación exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LogisticaMetConsultarCosto();
                }
                else
                {
                    MessageBox.Show(
                        "No se pudo modificar el costo.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al modificar el costo: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ------------------------------------------------------
        // BOTÓN ELIMINAR
        // ------------------------------------------------------
        private void LogisticaBtnEliminarCosto_Click(
            object sender, EventArgs e)
        {
            string guia = LogisticaMetObtenerNumeroGuia();

            if (string.IsNullOrWhiteSpace(guia))
            {
                MessageBox.Show(
                    "Seleccione la guía que desea eliminar.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar el costo de la guía " +
                guia + "?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                bool eliminado =
                    LogisticaObjControlador
                    .LogisticaMetEliminarCosto(guia);

                if (eliminado)
                {
                    MessageBox.Show(
                        "El costo se eliminó correctamente.",
                        "Eliminación exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LogisticaMetLimpiarDatosCosto();
                }
                else
                {
                    MessageBox.Show(
                        "No se encontró un costo para eliminar.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar el costo: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
