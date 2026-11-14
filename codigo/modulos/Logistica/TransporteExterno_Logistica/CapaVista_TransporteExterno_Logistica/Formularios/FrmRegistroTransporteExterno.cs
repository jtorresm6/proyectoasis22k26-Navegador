// ------------------------------------------------------
// Autor: Gabriela Pinto García
// Carné: 9959-23-1087
// Fecha: 21/10/2026
// Módulo: Logística - Transporte Externo
// ------------------------------------------------------

using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_TransporteExterno_Logistica;

namespace CapaVista_TransporteExterno_Logistica.Formularios
{
    public partial class FrmRegistroTransporteExterno : Form
    {
        private readonly ClsControladorTransporteExterno _Controlador;
        private DataTable _Transportes;
        private int _IdSeleccionado = 0;
        private bool _Cargando = false;

        public FrmRegistroTransporteExterno()
        {
            InitializeComponent();

            _Controlador = new ClsControladorTransporteExterno();

            LogisticaBtnGuardarRegistro.Click += LogisticaBtnGuardarRegistro_Click;
            LogisticaBtnModificarRegistro.Click += LogisticaBtnModificarRegistro_Click;
            LogisticaBtnEliminarRegistro.Click += LogisticaBtnEliminarRegistro_Click;

            LogisticaCboNumeroGuia.SelectedIndexChanged +=
                LogisticaCboNumeroGuia_SelectedIndexChanged;

            LogisticaCboTransportista.SelectedIndexChanged +=
                LogisticaCboTransportista_SelectedIndexChanged;

            LogisticaCboEstado.SelectedIndexChanged +=
                LogisticaCboEstado_SelectedIndexChanged;

            LogisticaTxtCodigo.ReadOnly = true;
            LogisticaTxtNumeroGuia.ReadOnly = true;

            LogisticaDtpFechaEntregaReal.ShowCheckBox = true;
            LogisticaDtpFechaEntregaReal.Checked = false;
            LogisticaDtpFechaEntregaReal.Enabled = false;

            LogisticaMetCargarCatalogos();
            LogisticaMetCargarGuias();
            LogisticaMetMostrarVistaPrevia();
        }

        // Cargar empresas y tipos de servicio.
        private void LogisticaMetCargarCatalogos()
        {
            try
            {
                _Cargando = true;

                DataTable empresas =
                    _Controlador.LogisticaFuncObtenerEmpresas();

                LogisticaCboTransportista.DataSource = empresas;
                LogisticaCboTransportista.DisplayMember =
                    "nombreEmpresaTransportista";
                LogisticaCboTransportista.ValueMember =
                    "idEmpresaTransportista";
                LogisticaCboTransportista.SelectedIndex = -1;

                DataTable tipos =
                    _Controlador.LogisticaFuncObtenerTiposServicio();

                LogisticaCboTipoServicio.DataSource = tipos;
                LogisticaCboTipoServicio.DisplayMember =
                    "nombreTipoServicio";
                LogisticaCboTipoServicio.ValueMember =
                    "idTipoServicio";
                LogisticaCboTipoServicio.SelectedIndex = -1;

                LogisticaCboEstado.SelectedItem = "Programado";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar catálogos: " + ex.Message);
            }
            finally
            {
                _Cargando = false;
            }
        }

        // Cargar las guías existentes.
        private void LogisticaMetCargarGuias()
        {
            try
            {
                _Cargando = true;

                _Transportes =
                    _Controlador.LogisticaFuncObtenerRegistroTransportes();

                LogisticaCboNumeroGuia.DataSource = null;
                LogisticaCboNumeroGuia.DisplayMember =
                    "numeroGuiaServicioTransporteExterno";
                LogisticaCboNumeroGuia.ValueMember =
                    "idServicioTransporteExterno";
                LogisticaCboNumeroGuia.DataSource = _Transportes;
                LogisticaCboNumeroGuia.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar guías: " + ex.Message);
            }
            finally
            {
                _Cargando = false;
            }
        }

        // Mostrar vista previa de los códigos.
        private void LogisticaMetMostrarVistaPrevia()
        {
            if (_Cargando || _IdSeleccionado > 0)
                return;

            try
            {
                DataTable datos =
                    _Controlador.LogisticaFuncObtenerProximosCodigos();

                if (datos.Rows.Count == 0)
                    throw new Exception(
                        "No se encontraron los próximos correlativos.");

                int idTransporte =
                    Convert.ToInt32(datos.Rows[0]["siguienteTransporte"]);

                int idSolicitud =
                    Convert.ToInt32(datos.Rows[0]["siguienteSolicitud"]);

                LogisticaTxtNumeroGuia.Text =
                    "GUIA-" + idTransporte.ToString("D6");

                LogisticaTxtCodigo.Text =
                    "SP-" + idSolicitud.ToString("D6");
            }
            catch (Exception ex)
            {
                LogisticaTxtNumeroGuia.Text =
                    "Pendiente de generar";

                LogisticaTxtCodigo.Text =
                    "Pendiente de generar";

                MessageBox.Show(
                    "No se pudieron consultar los correlativos: " +
                    ex.Message);
            }
        }

        // Actualizar contacto al seleccionar empresa.
        private void LogisticaCboTransportista_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (_Cargando)
                return;

            DataRowView empresa =
                LogisticaCboTransportista.SelectedItem as DataRowView;

            if (empresa != null)
            {
                LogisticaTxtContacto.Text =
                    Convert.ToString(
                        empresa["contactoEmpresaTransportista"]);
            }

            LogisticaMetMostrarVistaPrevia();
        }

        // Habilitar fecha real cuando el transporte esté entregado.
        private void LogisticaCboEstado_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (_Cargando)
                return;

            bool entregado =
                LogisticaCboEstado.Text.Trim().Equals(
                    "Entregado",
                    StringComparison.OrdinalIgnoreCase);

            LogisticaDtpFechaEntregaReal.Enabled = entregado;

            if (!entregado)
                LogisticaDtpFechaEntregaReal.Checked = false;
        }

        // Cargar los datos de una guía seleccionada.
        private void LogisticaCboNumeroGuia_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (_Cargando || LogisticaCboNumeroGuia.SelectedIndex < 0)
                return;

            DataRowView seleccionado =
                LogisticaCboNumeroGuia.SelectedItem as DataRowView;

            if (seleccionado == null)
                return;

            _Cargando = true;

            try
            {
                DataRow fila = seleccionado.Row;

                _IdSeleccionado =
                    Convert.ToInt32(
                        fila["idServicioTransporteExterno"]);

                LogisticaTxtNumeroGuia.Text =
                    Convert.ToString(
                        fila["numeroGuiaServicioTransporteExterno"]);

                LogisticaTxtCodigo.Text =
                    fila["codigoSolicitudPagoTransporte"] == DBNull.Value
                        ? ""
                        : Convert.ToString(
                            fila["codigoSolicitudPagoTransporte"]);

                LogisticaCboTransportista.SelectedValue =
                    fila["Fk_Empresa_Transportista_Codigo"];

                LogisticaCboTipoServicio.SelectedValue =
                    fila["Fk_Tipo_Servicio_Codigo"];

                LogisticaCboEstado.SelectedItem =
                    Convert.ToString(
                        fila["estadoServicioTransporteExterno"]);

                LogisticaTxtContacto.Text =
                    Convert.ToString(
                        fila["contactoEmpresaTransportista"]);

                LogisticaTxtPlaca.Text =
                    Convert.ToString(
                        fila["placaVehiculoTransporte"]);

                LogisticaTxtTipoVehiculo.Text =
                    Convert.ToString(
                        fila["tipoVehiculoTransporte"]);

                LogisticaTxtCapacidad.Text =
                    Convert.ToString(
                        fila["capacidadVehiculoTransporte"]);

                LogisticaTxtConductor.Text =
                    Convert.ToString(
                        fila["nombreConductorTransporte"]);

                LogisticaTxtTelefonoConductor.Text =
                    Convert.ToString(
                        fila["telefonoConductorTransporte"]);

                LogisticaDtpFechaContratacion.Value =
                    Convert.ToDateTime(
                        fila["fechaContratacionServicioTransporteExterno"]);

                LogisticaDtpFechaEstimada.Value =
                    Convert.ToDateTime(
                        fila["fechaEstimadaEntregaServicioTransporteExterno"]);

                LogisticaDtpFechaEntregaReal.Checked =
                    fila["fechaEntregaRealServicioTransporteExterno"]
                    != DBNull.Value;

                if (LogisticaDtpFechaEntregaReal.Checked)
                {
                    LogisticaDtpFechaEntregaReal.Value =
                        Convert.ToDateTime(
                            fila["fechaEntregaRealServicioTransporteExterno"]);
                }

                LogisticaDtpFechaEntregaReal.Enabled =
                    LogisticaCboEstado.Text.Trim().Equals(
                        "Entregado",
                        StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar los datos de la guía: " +
                    ex.Message);
            }
            finally
            {
                _Cargando = false;
            }
        }

        // Validar información.
        private bool LogisticaFuncValidar()
        {
            if (LogisticaCboTransportista.SelectedIndex < 0 ||
                LogisticaCboTipoServicio.SelectedIndex < 0 ||
                LogisticaCboEstado.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Selecciona empresa, tipo de servicio y estado.");

                return false;
            }

            if (LogisticaDtpFechaEstimada.Value.Date <
                LogisticaDtpFechaContratacion.Value.Date)
            {
                MessageBox.Show(
                    "La fecha estimada no puede ser anterior a la contratación.");

                return false;
            }

            if (LogisticaCboEstado.Text.Trim().Equals(
                "Entregado",
                StringComparison.OrdinalIgnoreCase) &&
                !LogisticaDtpFechaEntregaReal.Checked)
            {
                MessageBox.Show(
                    "Selecciona la fecha real de entrega.");

                return false;
            }

            return true;
        }

        // Obtener fecha real de entrega.
        private DateTime? LogisticaFuncFechaReal()
        {
            return LogisticaDtpFechaEntregaReal.Checked
                ? (DateTime?)LogisticaDtpFechaEntregaReal.Value.Date
                : null;
        }

        // Guardar transporte.
        private void LogisticaBtnGuardarRegistro_Click(
            object sender, EventArgs e)
        {
            if (!LogisticaFuncValidar())
                return;

            try
            {
                DataTable resultado =
                    _Controlador.LogisticaFuncAgregarTransporte(
                        Convert.ToInt32(
                            LogisticaCboTransportista.SelectedValue),
                        Convert.ToInt32(
                            LogisticaCboTipoServicio.SelectedValue),
                        LogisticaCboEstado.Text,
                        LogisticaDtpFechaContratacion.Value.Date,
                        LogisticaDtpFechaEstimada.Value.Date,
                        LogisticaFuncFechaReal(),
                        LogisticaTxtPlaca.Text.Trim(),
                        LogisticaTxtTipoVehiculo.Text.Trim(),
                        LogisticaTxtCapacidad.Text.Trim(),
                        LogisticaTxtConductor.Text.Trim(),
                        LogisticaTxtTelefonoConductor.Text.Trim());

                int idServicio =
                    Convert.ToInt32(
                        resultado.Rows[0]["idServicio"]);

                MessageBox.Show(
                    "Transporte registrado correctamente.\n" +
                    "Número de guía: " +
                    resultado.Rows[0]["numeroGuia"] + "\n" +
                    "Solicitud de pago: " +
                    resultado.Rows[0]["codigoSolicitudPago"]);

                _IdSeleccionado = 0;

                LogisticaMetCargarGuias();

                LogisticaCboNumeroGuia.SelectedValue = idServicio;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al guardar transporte: " +
                    ex.Message);
            }
        }

        // Modificar transporte.
        private void LogisticaBtnModificarRegistro_Click(
            object sender, EventArgs e)
        {
            if (_IdSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Primero selecciona un número de guía.");
                return;
            }

            if (!LogisticaFuncValidar())
                return;

            try
            {
                _Controlador.LogisticaMetModificarTransporte(
                    _IdSeleccionado,
                    Convert.ToInt32(
                        LogisticaCboTransportista.SelectedValue),
                    Convert.ToInt32(
                        LogisticaCboTipoServicio.SelectedValue),
                    LogisticaCboEstado.Text,
                    LogisticaDtpFechaContratacion.Value.Date,
                    LogisticaDtpFechaEstimada.Value.Date,
                    LogisticaFuncFechaReal(),
                    LogisticaTxtPlaca.Text.Trim(),
                    LogisticaTxtTipoVehiculo.Text.Trim(),
                    LogisticaTxtCapacidad.Text.Trim(),
                    LogisticaTxtConductor.Text.Trim(),
                    LogisticaTxtTelefonoConductor.Text.Trim());

                MessageBox.Show(
                    "Transporte modificado correctamente.");

                int id = _IdSeleccionado;

                LogisticaMetCargarGuias();
                LogisticaCboNumeroGuia.SelectedValue = id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al modificar transporte: " +
                    ex.Message);
            }
        }

        // Eliminar transporte.
        private void LogisticaBtnEliminarRegistro_Click(
            object sender, EventArgs e)
        {
            if (_IdSeleccionado <= 0)
            {
                MessageBox.Show(
                    "Primero selecciona un número de guía.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Deseas eliminar el transporte " +
                LogisticaTxtNumeroGuia.Text + "?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                _Controlador.LogisticaMetEliminarTransporte(
                    _IdSeleccionado);

                _IdSeleccionado = 0;

                LogisticaMetCargarGuias();
                LogisticaMetLimpiarFormulario();
                LogisticaMetMostrarVistaPrevia();

                MessageBox.Show(
                    "El transporte se eliminó correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo eliminar el transporte: " +
                    ex.Message);
            }
        }

        // Limpiar formulario.
        private void LogisticaMetLimpiarFormulario()
        {
            _Cargando = true;

            try
            {
                LogisticaCboNumeroGuia.SelectedIndex = -1;
                LogisticaCboTransportista.SelectedIndex = -1;
                LogisticaCboTipoServicio.SelectedIndex = -1;

                LogisticaTxtContacto.Clear();
                LogisticaTxtPlaca.Clear();
                LogisticaTxtTipoVehiculo.Clear();
                LogisticaTxtCapacidad.Clear();
                LogisticaTxtConductor.Clear();
                LogisticaTxtTelefonoConductor.Clear();

                LogisticaDtpFechaEntregaReal.Checked = false;
                LogisticaDtpFechaEntregaReal.Enabled = false;

                LogisticaCboEstado.SelectedItem = "Programado";
            }
            finally
            {
                _Cargando = false;
            }
        }
    }
}
