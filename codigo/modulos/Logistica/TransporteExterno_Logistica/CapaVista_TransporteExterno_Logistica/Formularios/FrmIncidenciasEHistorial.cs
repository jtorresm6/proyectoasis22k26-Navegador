
using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_TransporteExterno_Logistica;

namespace CapaVista_TransporteExterno_Logistica.Formularios
{
    public partial class FrmIncidenciasEHistorial : Form
    {
        private ClsControladorIncidenciasEHistorial controlador;
        private int idIncidenciaSeleccionada = 0;

        public FrmIncidenciasEHistorial()
        {
            InitializeComponent();

            controlador = new ClsControladorIncidenciasEHistorial();

            this.Load += FrmIncidenciasEHistorial_Load;

            LogisticaCboNumeroGuia.SelectedIndexChanged +=
                LogisticaCboNumeroGuia_SelectedIndexChanged;

            LogisticaBtnGuardarIncidencia.Click +=
                LogisticaBtnGuardarIncidencia_Click;

            LogisticaBtnModificarIncidencia.Click +=
                LogisticaBtnModificarIncidencia_Click;

            LogisticaBtnEliminarIncidencia.Click +=
                LogisticaBtnEliminarIncidencia_Click;

            LogisticaBtnConsultarHistorial.Click +=
                LogisticaBtnConsultarHistorial_Click;

            LogisticaDgvIncidencias.CellClick +=
                LogisticaDgvIncidencias_CellClick;
        }

        // ==========================================
        // INICIAR FORMULARIO
        // ==========================================
        private void FrmIncidenciasEHistorial_Load(
            object sender,
            EventArgs e)
        {
            try
            {
                LogisticaCboTipoIncidencia.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                LogisticaCboTipoIncidencia.Items.Clear();

                LogisticaCboTipoIncidencia.Items.AddRange(
                    new object[]
                    {
                        "Retraso en entrega",
                        "Producto dañado",
                        "Producto extraviado",
                        "Problema con vehículo",
                        "Accidente",
                        "Otro"
                    });

                LogisticaCboEstadoIncidencia.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                LogisticaCboEstadoIncidencia.Items.Clear();

                LogisticaCboEstadoIncidencia.Items.AddRange(
                    new object[]
                    {
                        "Pendiente",
                        "En proceso",
                        "Resuelta"
                    });

                LogisticaCboResponsableIncidencia.DropDownStyle =
                    ComboBoxStyle.DropDown;

                LogisticaCboFiltroEstadoHistorial.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                LogisticaCboFiltroEstadoHistorial.Items.Clear();

                LogisticaCboFiltroEstadoHistorial.Items.AddRange(
                    new object[]
                    {
                        "Todos",
                        "SOLICITADO",
                        "EN_TRANSITO",
                        "RECIBIDO",
                        "CANCELADO"
                    });

                LogisticaCboFiltroEstadoHistorial.SelectedIndex = 0;

                LogisticaDgvIncidencias.ReadOnly = true;
                LogisticaDgvIncidencias.AllowUserToAddRows = false;
                LogisticaDgvIncidencias.AllowUserToDeleteRows = false;
                LogisticaDgvIncidencias.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;
                LogisticaDgvIncidencias.MultiSelect = false;
                LogisticaDgvIncidencias.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                LogisticaDgvHistorial.ReadOnly = true;
                LogisticaDgvHistorial.AllowUserToAddRows = false;
                LogisticaDgvHistorial.AllowUserToDeleteRows = false;
                LogisticaDgvHistorial.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;
                LogisticaDgvHistorial.MultiSelect = false;
                LogisticaDgvHistorial.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                // Permitir dejar las fechas sin filtro.
                LogisticaDtpHistorialDesde.ShowCheckBox = true;
                LogisticaDtpHistorialHasta.ShowCheckBox = true;

                LogisticaDtpHistorialDesde.Checked = false;
                LogisticaDtpHistorialHasta.Checked = false;

                DataTable guias = controlador.LogisticaFuncObtenerGuias();

                LogisticaCboNumeroGuia.DataSource = guias;
                LogisticaCboNumeroGuia.DisplayMember = "numeroGuia";
                LogisticaCboNumeroGuia.ValueMember = "idServicio";
                LogisticaCboNumeroGuia.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                LogisticaCboNumeroGuia.SelectedIndex = -1;

                LogisticaMetLimpiarIncidencia();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al iniciar el formulario:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // OBTENER ID DEL SERVICIO
        // ==========================================
        private int LogisticaFuncObtenerIdServicio()
        {
            if (LogisticaCboNumeroGuia.SelectedValue == null ||
                LogisticaCboNumeroGuia.SelectedValue is DataRowView)
            {
                return 0;
            }

            return Convert.ToInt32(
                LogisticaCboNumeroGuia.SelectedValue);
        }

        // ==========================================
        // CAMBIAR GUIA
        // ==========================================
        private void LogisticaCboNumeroGuia_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                int idServicio = LogisticaFuncObtenerIdServicio();

                if (idServicio <= 0)
                    return;

                idIncidenciaSeleccionada = 0;

                LogisticaMetLimpiarIncidencia();
                LogisticaMetCargarIncidencias();

                LogisticaDgvHistorial.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar las incidencias:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // CARGAR INCIDENCIAS
        // ==========================================
        private void LogisticaMetCargarIncidencias()
        {
            int idServicio = LogisticaFuncObtenerIdServicio();

            if (idServicio <= 0)
                return;

            DataTable tabla =
                controlador.LogisticaFuncObtenerIncidencias(idServicio);

            LogisticaDgvIncidencias.DataSource = tabla;

            if (LogisticaDgvIncidencias.Columns.Contains(
                "idIncidenciaTransporte"))
            {
                LogisticaDgvIncidencias.Columns[
                    "idIncidenciaTransporte"].Visible = false;
            }

            if (LogisticaDgvIncidencias.Columns.Contains(
                "codigoIncidenciaTransporte"))
            {
                LogisticaDgvIncidencias.Columns[
                    "codigoIncidenciaTransporte"].HeaderText = "Código";
            }

            if (LogisticaDgvIncidencias.Columns.Contains(
                "fechaIncidenciaTransporte"))
            {
                LogisticaDgvIncidencias.Columns[
                    "fechaIncidenciaTransporte"].HeaderText = "Fecha";
            }

            if (LogisticaDgvIncidencias.Columns.Contains(
                "tipoIncidenciaTransporte"))
            {
                LogisticaDgvIncidencias.Columns[
                    "tipoIncidenciaTransporte"].HeaderText = "Tipo";
            }

            if (LogisticaDgvIncidencias.Columns.Contains(
                "descripcionIncidenciaTransporte"))
            {
                LogisticaDgvIncidencias.Columns[
                    "descripcionIncidenciaTransporte"].HeaderText =
                    "Descripción";
            }

            if (LogisticaDgvIncidencias.Columns.Contains(
                "responsableIncidenciaTransporte"))
            {
                LogisticaDgvIncidencias.Columns[
                    "responsableIncidenciaTransporte"].HeaderText =
                    "Responsable";
            }

            if (LogisticaDgvIncidencias.Columns.Contains(
                "estadoIncidenciaTransporte"))
            {
                LogisticaDgvIncidencias.Columns[
                    "estadoIncidenciaTransporte"].HeaderText = "Estado";
            }
        }

        // ==========================================
        // GUARDAR INCIDENCIA
        // ==========================================
        private void LogisticaBtnGuardarIncidencia_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                int idServicio = LogisticaFuncObtenerIdServicio();

                string codigo =
                    LogisticaTxtCodigoIncidencia.Text.Trim();

                string tipo =
                    Convert.ToString(
                        LogisticaCboTipoIncidencia.SelectedItem);

                string descripcion =
                    LogisticaTxtDescripcionIncidencia.Text.Trim();

                string responsable =
                    LogisticaCboResponsableIncidencia.Text.Trim();

                string estado =
                    Convert.ToString(
                        LogisticaCboEstadoIncidencia.SelectedItem);

                if (idIncidenciaSeleccionada > 0)
                {
                    controlador.LogisticaProcModificarIncidencia(
                        idIncidenciaSeleccionada,
                        codigo,
                        LogisticaDtpFechaIncidencia.Value,
                        tipo,
                        descripcion,
                        responsable,
                        estado);

                    MessageBox.Show(
                        "La incidencia se modificó correctamente.",
                        "Modificación exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    controlador.LogisticaProcGuardarIncidencia(
                        idServicio,
                        codigo,
                        LogisticaDtpFechaIncidencia.Value,
                        tipo,
                        descripcion,
                        responsable,
                        estado);

                    MessageBox.Show(
                        "La incidencia se registró correctamente.",
                        "Registro exitoso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                LogisticaMetCargarIncidencias();

                idIncidenciaSeleccionada = 0;

                LogisticaMetLimpiarIncidencia();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo guardar la incidencia:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // SELECCIONAR INCIDENCIA PARA MODIFICAR
        // ==========================================
        private void LogisticaBtnModificarIncidencia_Click(
            object sender,
            EventArgs e)
        {
            if (LogisticaDgvIncidencias.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione una incidencia.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                DataGridViewRow fila =
                    LogisticaDgvIncidencias.CurrentRow;

                idIncidenciaSeleccionada = Convert.ToInt32(
                    fila.Cells["idIncidenciaTransporte"].Value);

                LogisticaTxtCodigoIncidencia.Text =
                    Convert.ToString(
                        fila.Cells["codigoIncidenciaTransporte"].Value);

                LogisticaDtpFechaIncidencia.Value =
                    Convert.ToDateTime(
                        fila.Cells["fechaIncidenciaTransporte"].Value);

                LogisticaCboTipoIncidencia.Text =
                    Convert.ToString(
                        fila.Cells["tipoIncidenciaTransporte"].Value);

                LogisticaTxtDescripcionIncidencia.Text =
                    Convert.ToString(
                        fila.Cells["descripcionIncidenciaTransporte"].Value);

                LogisticaCboResponsableIncidencia.Text =
                    Convert.ToString(
                        fila.Cells["responsableIncidenciaTransporte"].Value);

                LogisticaCboEstadoIncidencia.Text =
                    Convert.ToString(
                        fila.Cells["estadoIncidenciaTransporte"].Value);

                LogisticaLblTituloRegistroIncidencia.Text =
                    "Modificar incidencia";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo seleccionar la incidencia:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // ELIMINAR INCIDENCIA
        // ==========================================
        private void LogisticaBtnEliminarIncidencia_Click(
            object sender,
            EventArgs e)
        {
            if (LogisticaDgvIncidencias.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione una incidencia.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                int idIncidencia = Convert.ToInt32(
                    LogisticaDgvIncidencias.CurrentRow.Cells[
                        "idIncidenciaTransporte"].Value);

                DialogResult respuesta = MessageBox.Show(
                    "¿Está seguro de eliminar esta incidencia?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes)
                    return;

                controlador.LogisticaProcEliminarIncidencia(
                    idIncidencia);

                MessageBox.Show(
                    "La incidencia se eliminó correctamente.",
                    "Eliminación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                idIncidenciaSeleccionada = 0;

                LogisticaMetCargarIncidencias();
                LogisticaMetLimpiarIncidencia();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo eliminar la incidencia:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // SELECCIONAR FILA DE INCIDENCIA
        // ==========================================
        private void LogisticaDgvIncidencias_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            idIncidenciaSeleccionada = Convert.ToInt32(
                LogisticaDgvIncidencias.Rows[e.RowIndex].Cells[
                    "idIncidenciaTransporte"].Value);
        }

        // ==========================================
        // CONSULTAR HISTORIAL
        // ==========================================
        private void LogisticaBtnConsultarHistorial_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                int idServicio = LogisticaFuncObtenerIdServicio();

                string estado =
                    LogisticaCboFiltroEstadoHistorial.Text;

                if (string.IsNullOrWhiteSpace(estado))
                    estado = "Todos";

                DateTime? fechaDesde = null;
                DateTime? fechaHasta = null;

                if (LogisticaDtpHistorialDesde.Checked)
                    fechaDesde = LogisticaDtpHistorialDesde.Value.Date;

                if (LogisticaDtpHistorialHasta.Checked)
                    fechaHasta = LogisticaDtpHistorialHasta.Value.Date;

                DataTable tabla =
                    controlador.LogisticaFuncObtenerHistorial(
                        idServicio,
                        fechaDesde,
                        fechaHasta,
                        estado);

                LogisticaDgvHistorial.DataSource = tabla;

                LogisticaDgvHistorial.ReadOnly = true;
                LogisticaDgvHistorial.AllowUserToAddRows = false;
                LogisticaDgvHistorial.AllowUserToDeleteRows = false;

                LogisticaDgvHistorial.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                LogisticaDgvHistorial.MultiSelect = false;

                LogisticaDgvHistorial.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                if (LogisticaDgvHistorial.Columns.Contains("numeroGuia"))
                {
                    LogisticaDgvHistorial.Columns[
                        "numeroGuia"].HeaderText = "Número de guía";
                }

                if (LogisticaDgvHistorial.Columns.Contains("fechaMovimiento"))
                {
                    LogisticaDgvHistorial.Columns[
                        "fechaMovimiento"].HeaderText =
                        "Fecha del movimiento";
                }

                if (LogisticaDgvHistorial.Columns.Contains("tipoMovimiento"))
                {
                    LogisticaDgvHistorial.Columns[
                        "tipoMovimiento"].HeaderText =
                        "Tipo de movimiento";
                }

                if (LogisticaDgvHistorial.Columns.Contains("descripcion"))
                {
                    LogisticaDgvHistorial.Columns[
                        "descripcion"].HeaderText = "Descripción";
                }

                if (LogisticaDgvHistorial.Columns.Contains("estado"))
                {
                    LogisticaDgvHistorial.Columns[
                        "estado"].HeaderText = "Estado";
                }

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontraron movimientos para los filtros seleccionados.",
                        "Historial de transporte",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo consultar el historial:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // LIMPIAR CAMPOS DE INCIDENCIA
        // ==========================================
        private void LogisticaMetLimpiarIncidencia()
        {
            LogisticaTxtCodigoIncidencia.Clear();

            LogisticaTxtDescripcionIncidencia.Clear();

            LogisticaDtpFechaIncidencia.Value = DateTime.Today;

            LogisticaCboTipoIncidencia.SelectedIndex = -1;

            LogisticaCboEstadoIncidencia.SelectedIndex = -1;

            LogisticaCboResponsableIncidencia.Text = "";

            LogisticaLblTituloRegistroIncidencia.Text =
                "Registro de incidencias";
        }
    }
}
