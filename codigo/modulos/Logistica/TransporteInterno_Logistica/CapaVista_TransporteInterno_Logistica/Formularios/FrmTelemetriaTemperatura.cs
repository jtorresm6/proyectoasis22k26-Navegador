using CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    public partial class FrmTelemetriaTransporte : Form
    {
        // =====================================================
        // CONTROLADOR Y REGISTROS
        // =====================================================

        private readonly ClsControladorTelemetriaTemperatura
            _Controlador =
                new ClsControladorTelemetriaTemperatura();

        private List<ClsDatosTelemetriaTemperatura> _Telemetrias =
            new List<ClsDatosTelemetriaTemperatura>();

        private List<ClsDatosTelemetriaTemperatura> _Filtrados =
            new List<ClsDatosTelemetriaTemperatura>();

        // Estado del formulario.
        private bool _ModoEdicion = false;
        private bool _EsNuevo = false;
        private long _IdSeleccionado = 0;

        // Paginación y ordenamiento.
        private int _PaginaActual = 1;
        private const int _TamanoPagina = 10;

        private string _ColumnaOrden =
            "IdTelemetriaTemperatura";

        private bool _OrdenAscendente = false;

        // Controles adicionales para consultas.
        private Label LogisticaLblBuscarTelemetria;
        private TextBox LogisticaTxtBuscarTelemetria;

        private Button LogisticaBtnAnteriorTelemetria;
        private Button LogisticaBtnSiguienteTelemetria;

        private Label LogisticaLblPaginacionTelemetria;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public FrmTelemetriaTransporte()
        {
            InitializeComponent();

            LogisticaProcConfigurarControles();
            LogisticaProcCrearConsultaVisual();
            LogisticaProcVincularEventos();
            LogisticaProcBloquearFormulario();

            AutoScroll = true;
        }

        // =====================================================
        // CONFIGURAR CONTROLES
        // =====================================================

        private void LogisticaProcConfigurarControles()
        {
            // Código automático.
            LogisticaTxtCodigoTelemetria.ReadOnly = true;
            LogisticaTxtCodigoTelemetria.TabStop = false;

            // Viajes existentes.
            LogisticaCboViaje.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // Fecha y hora.
            LogisticaDtpFechaHora.Format =
                DateTimePickerFormat.Custom;

            LogisticaDtpFechaHora.CustomFormat =
                "dd/MM/yyyy HH:mm";

            LogisticaDtpFechaHora.ShowUpDown = true;

            // Temperatura Celsius.
            LogisticaNudTemperatura.DecimalPlaces = 2;
            LogisticaNudTemperatura.Minimum = -999.99m;
            LogisticaNudTemperatura.Maximum = 999.99m;
            LogisticaNudTemperatura.Increment = 0.10m;

            // Humedad relativa: 0.0000 a 1.0000.
            LogisticaNudHumedad.DecimalPlaces = 4;
            LogisticaNudHumedad.Minimum = 0;
            LogisticaNudHumedad.Maximum = 1;
            LogisticaNudHumedad.Increment = 0.0001m;

            // Ubicación GPS.
            LogisticaTxtUbicacionGPS.MaxLength = 100;

            // Configuración del DataGridView.
            LogisticaDvgHistorialTelemetria.AutoGenerateColumns =
                false;

            LogisticaDvgHistorialTelemetria.ReadOnly = true;

            LogisticaDvgHistorialTelemetria.MultiSelect = false;

            LogisticaDvgHistorialTelemetria.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            LogisticaDvgHistorialTelemetria.AllowUserToAddRows =
                false;

            LogisticaDvgHistorialTelemetria.AllowUserToDeleteRows =
                false;

            LogisticaDvgHistorialTelemetria.RowHeadersVisible =
                false;

            LogisticaDvgHistorialTelemetria.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LogisticaDvgHistorialTelemetria.EnableHeadersVisualStyles =
                false;

            // Colores estandarizados de Logística.
            LogisticaDvgHistorialTelemetria
                .ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(52, 143, 141);

            LogisticaDvgHistorialTelemetria
                .ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            LogisticaDvgHistorialTelemetria
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 99, 46);

            LogisticaDvgHistorialTelemetria
                .DefaultCellStyle.SelectionForeColor =
                Color.White;

            LogisticaDvgHistorialTelemetria
                .AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(251, 246, 242);

            // Relacionar columnas con el controlador.
            ColCodigoTelemetria.DataPropertyName =
                "IdTelemetriaTemperatura";

            ColViaje.DataPropertyName =
                "NombreViaje";

            ColFechaHora.DataPropertyName =
                "FechaHoraTelemetriaTemperatura";

            ColTemperatura.DataPropertyName =
                "TemperaturaCelsiusTelemetriaTemperatura";

            ColHumedad.DataPropertyName =
                "HumedadRelativaTelemetriaTemperatura";

            ColPuertaAbierta.DataPropertyName =
                "PuertaAbiertaTelemetriaTemperatura";

            ColUbicacionGPS.DataPropertyName =
                "UbicacionGpsTelemetriaTemperatura";

            // Formatos del historial.
            ColFechaHora.DefaultCellStyle.Format =
                "dd/MM/yyyy HH:mm";

            ColTemperatura.DefaultCellStyle.Format = "N2";
            ColHumedad.DefaultCellStyle.Format = "N4";

            foreach (DataGridViewColumn Columna
                in LogisticaDvgHistorialTelemetria.Columns)
            {
                Columna.SortMode =
                    DataGridViewColumnSortMode.Programmatic;
            }
        }

        // =====================================================
        // CREAR CONTROLES DE BÚSQUEDA Y PAGINACIÓN
        // =====================================================

        private void LogisticaProcCrearConsultaVisual()
        {
            Color Verde = Color.FromArgb(52, 143, 141);
            Color Botones = Color.FromArgb(242, 216, 197);

            int X = LogisticaDvgHistorialTelemetria.Left;
            int Y = LogisticaDvgHistorialTelemetria.Top;

            // Label Buscar.
            LogisticaLblBuscarTelemetria = new Label
            {
                Name = "LogisticaLblBuscarTelemetria",
                Text = "Buscar:",
                Location = new Point(X, Y + 3),
                Size = new Size(75, 25),
                ForeColor = Verde,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f)
            };

            // TextBox Buscar.
            LogisticaTxtBuscarTelemetria = new TextBox
            {
                Name = "LogisticaTxtBuscarTelemetria",
                Location = new Point(X + 80, Y),
                Size = new Size(350, 28),
                Font = new Font("Segoe UI", 10f)
            };

            // Desplazar la tabla.
            LogisticaDvgHistorialTelemetria.Top = Y + 40;

            // Botón Anterior.
            LogisticaBtnAnteriorTelemetria = new Button
            {
                Name = "LogisticaBtnAnteriorTelemetria",
                Text = "Anterior",
                Location = new Point(
                    X,
                    LogisticaDvgHistorialTelemetria.Bottom + 12),
                Size = new Size(95, 30),
                BackColor = Botones
            };

            // Botón Siguiente.
            LogisticaBtnSiguienteTelemetria = new Button
            {
                Name = "LogisticaBtnSiguienteTelemetria",
                Text = "Siguiente",
                Location = new Point(
                    X + 105,
                    LogisticaDvgHistorialTelemetria.Bottom + 12),
                Size = new Size(95, 30),
                BackColor = Botones
            };

            // Información de paginación.
            LogisticaLblPaginacionTelemetria = new Label
            {
                Name = "LogisticaLblPaginacionTelemetria",
                Location = new Point(
                    X + 215,
                    LogisticaDvgHistorialTelemetria.Bottom + 16),
                Size = new Size(430, 25),
                ForeColor = Verde,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f)
            };

            // Agregar controles al panel existente.
            LogisticaPnlHistorialTelemetria.Controls.Add(
                LogisticaLblBuscarTelemetria);

            LogisticaPnlHistorialTelemetria.Controls.Add(
                LogisticaTxtBuscarTelemetria);

            LogisticaPnlHistorialTelemetria.Controls.Add(
                LogisticaBtnAnteriorTelemetria);

            LogisticaPnlHistorialTelemetria.Controls.Add(
                LogisticaBtnSiguienteTelemetria);

            LogisticaPnlHistorialTelemetria.Controls.Add(
                LogisticaLblPaginacionTelemetria);

            // Ampliar panel para incluir la paginación.
            LogisticaPnlHistorialTelemetria.Height = Math.Max(
                LogisticaPnlHistorialTelemetria.Height,
                LogisticaBtnAnteriorTelemetria.Bottom + 15);

            // Eventos.
            LogisticaTxtBuscarTelemetria.TextChanged +=
                LogisticaTxtBuscarTelemetria_TextChanged;

            LogisticaBtnAnteriorTelemetria.Click += (sender, e) =>
            {
                if (_PaginaActual > 1)
                {
                    _PaginaActual--;
                    LogisticaProcMostrarPagina();
                }
            };

            LogisticaBtnSiguienteTelemetria.Click += (sender, e) =>
            {
                if (_PaginaActual < LogisticaFuncTotalPaginas())
                {
                    _PaginaActual++;
                    LogisticaProcMostrarPagina();
                }
            };
        }

        // =====================================================
        // VINCULAR EVENTOS
        // =====================================================

        private void LogisticaProcVincularEventos()
        {
            Load -= FrmTelemetriaTemperatura_Load;
            Load += FrmTelemetriaTemperatura_Load;

            LogisticaBtnNuevo.Click -= LogisticaBtnNuevo_Click;
            LogisticaBtnNuevo.Click += LogisticaBtnNuevo_Click;

            LogisticaBtnConsultar.Click -= LogisticaBtnConsultar_Click;
            LogisticaBtnConsultar.Click += LogisticaBtnConsultar_Click;

            LogisticaBtnModificar.Click -= LogisticaBtnModificar_Click;
            LogisticaBtnModificar.Click += LogisticaBtnModificar_Click;

            LogisticaBtnGuardar.Click -= LogisticaBtnGuardar_Click;
            LogisticaBtnGuardar.Click += LogisticaBtnGuardar_Click;

            LogisticaBtnEliminar.Click -= LogisticaBtnEliminar_Click;
            LogisticaBtnEliminar.Click += LogisticaBtnEliminar_Click;

            LogisticaBtnCancelar.Click -= LogisticaBtnCancelar_Click;
            LogisticaBtnCancelar.Click += LogisticaBtnCancelar_Click;

            LogisticaBtnRefrescar.Click -= LogisticaBtnRefrescar_Click;
            LogisticaBtnRefrescar.Click += LogisticaBtnRefrescar_Click;

            LogisticaBtnImprimir.Click -= LogisticaBtnImprimir_Click;
            LogisticaBtnImprimir.Click += LogisticaBtnImprimir_Click;

            LogisticaBtnAyuda.Click -= LogisticaBtnAyuda_Click;
            LogisticaBtnAyuda.Click += LogisticaBtnAyuda_Click;

            LogisticaDvgHistorialTelemetria.CellDoubleClick -=
                LogisticaDgvTelemetria_CellDoubleClick;

            LogisticaDvgHistorialTelemetria.CellDoubleClick +=
                LogisticaDgvTelemetria_CellDoubleClick;

            LogisticaDvgHistorialTelemetria.ColumnHeaderMouseClick -=
                LogisticaDgvTelemetria_ColumnHeaderMouseClick;

            LogisticaDvgHistorialTelemetria.ColumnHeaderMouseClick +=
                LogisticaDgvTelemetria_ColumnHeaderMouseClick;
        }

        // =====================================================
        // EVENTO LOAD
        // =====================================================

        private void FrmTelemetriaTemperatura_Load(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcCargarViajes();
                LogisticaProcRecargarTelemetrias();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(
                    "No se pudieron cargar los datos: " +
                    ex.Message);
            }
        }

        // =====================================================
        // CARGAR VIAJES
        // =====================================================

        private void LogisticaProcCargarViajes()
        {
            LogisticaCboViaje.DataSource = null;

            LogisticaCboViaje.DisplayMember = "Nombre";
            LogisticaCboViaje.ValueMember = "Id";

            LogisticaCboViaje.DataSource =
                _Controlador.LogisticaFuncViajes();

            LogisticaCboViaje.SelectedIndex = -1;
        }

        // =====================================================
        // CONSULTAR REGISTROS
        // =====================================================

        private void LogisticaProcRecargarTelemetrias()
        {
            _Telemetrias =
                _Controlador.LogisticaFuncObtenerTodos();

            LogisticaProcFiltrarTelemetrias();
        }

        // =====================================================
        // FILTRAR REGISTROS
        // =====================================================

        private void LogisticaProcFiltrarTelemetrias()
        {
            string Busqueda =
                LogisticaTxtBuscarTelemetria.Text.Trim();

            _Filtrados = _Telemetrias.Where(T =>
                T.IdTelemetriaTemperatura.ToString()
                    .Contains(Busqueda) ||

                (T.NombreViaje ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (T.UbicacionGpsTelemetriaTemperatura ?? "")
                    .IndexOf(
                        Busqueda,
                        StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            LogisticaProcOrdenarTelemetrias();

            _PaginaActual = 1;
            LogisticaProcMostrarPagina();
        }

        private void LogisticaTxtBuscarTelemetria_TextChanged(
            object sender, EventArgs e)
        {
            LogisticaProcFiltrarTelemetrias();
        }

        // =====================================================
        // ORDENAR REGISTROS
        // =====================================================

        private void LogisticaProcOrdenarTelemetrias()
        {
            PropertyInfo Propiedad =
                typeof(ClsDatosTelemetriaTemperatura)
                    .GetProperty(_ColumnaOrden);

            if (Propiedad == null)
                return;

            _Filtrados = _OrdenAscendente
                ? _Filtrados.OrderBy(
                    T => Propiedad.GetValue(T, null)).ToList()
                : _Filtrados.OrderByDescending(
                    T => Propiedad.GetValue(T, null)).ToList();
        }

        private void LogisticaDgvTelemetria_ColumnHeaderMouseClick(
            object sender,
            DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            string Nombre =
                LogisticaDvgHistorialTelemetria
                    .Columns[e.ColumnIndex].DataPropertyName;

            if (string.IsNullOrEmpty(Nombre))
                return;

            _OrdenAscendente =
                _ColumnaOrden == Nombre
                    ? !_OrdenAscendente
                    : true;

            _ColumnaOrden = Nombre;

            LogisticaProcOrdenarTelemetrias();

            _PaginaActual = 1;
            LogisticaProcMostrarPagina();
        }

        // =====================================================
        // PAGINACIÓN
        // =====================================================

        private int LogisticaFuncTotalPaginas()
        {
            return Math.Max(
                1,
                (_Filtrados.Count + _TamanoPagina - 1)
                    / _TamanoPagina);
        }

        private void LogisticaProcMostrarPagina()
        {
            LogisticaDvgHistorialTelemetria.DataSource = null;

            LogisticaDvgHistorialTelemetria.DataSource =
                _Filtrados
                    .Skip((_PaginaActual - 1) * _TamanoPagina)
                    .Take(_TamanoPagina)
                    .ToList();

            LogisticaDvgHistorialTelemetria.ClearSelection();

            LogisticaLblPaginacionTelemetria.Text =
                string.Format(
                    "Página {0} de {1} | {2} registros",
                    _PaginaActual,
                    LogisticaFuncTotalPaginas(),
                    _Filtrados.Count);

            LogisticaBtnAnteriorTelemetria.Enabled =
                _PaginaActual > 1;

            LogisticaBtnSiguienteTelemetria.Enabled =
                _PaginaActual < LogisticaFuncTotalPaginas();
        }

        // =====================================================
        // OBTENER REGISTRO SELECCIONADO
        // =====================================================

        private ClsDatosTelemetriaTemperatura
            LogisticaFuncSeleccionado()
        {
            if (LogisticaDvgHistorialTelemetria
                .SelectedRows.Count == 0)
            {
                throw new Exception(
                    "Seleccione un registro de telemetría.");
            }

            var Registro =
                LogisticaDvgHistorialTelemetria
                    .SelectedRows[0].DataBoundItem
                    as ClsDatosTelemetriaTemperatura;

            if (Registro == null)
            {
                throw new Exception(
                    "No se pudo identificar el registro seleccionado.");
            }

            return Registro;
        }

        // =====================================================
        // CARGAR REGISTRO PARA MODIFICAR
        // =====================================================

        private void LogisticaProcCargarSeleccionado()
        {
            var Registro = LogisticaFuncSeleccionado();

            _IdSeleccionado =
                Registro.IdTelemetriaTemperatura;

            _EsNuevo = false;
            _ModoEdicion = true;

            LogisticaPnlDatosTelemetria.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaTxtCodigoTelemetria.Text =
                Registro.IdTelemetriaTemperatura.ToString();

            LogisticaCboViaje.SelectedValue =
                Registro.FkEncabezadoViajeCodigo;

            LogisticaDtpFechaHora.Value =
                Registro.FechaHoraTelemetriaTemperatura;

            LogisticaNudTemperatura.Value = Math.Max(
                LogisticaNudTemperatura.Minimum,
                Math.Min(
                    LogisticaNudTemperatura.Maximum,
                    Registro.TemperaturaCelsiusTelemetriaTemperatura));

            LogisticaNudHumedad.Value = Math.Max(
                LogisticaNudHumedad.Minimum,
                Math.Min(
                    LogisticaNudHumedad.Maximum,
                    Registro.HumedadRelativaTelemetriaTemperatura));

            LogisticaChkPuertaAbierta.Checked =
                Registro.PuertaAbiertaTelemetriaTemperatura;

            LogisticaTxtUbicacionGPS.Text =
                Registro.UbicacionGpsTelemetriaTemperatura;
        }

        // =====================================================
        // BOTÓN INGRESAR
        // =====================================================

        private void LogisticaBtnNuevo_Click(
            object sender, EventArgs e)
        {
            LogisticaProcBloquearFormulario();

            _ModoEdicion = true;
            _EsNuevo = true;
            _IdSeleccionado = 0;

            LogisticaPnlDatosTelemetria.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaDtpFechaHora.Value = DateTime.Now;

            LogisticaCboViaje.Focus();
        }

        // =====================================================
        // BOTÓN CONSULTAR
        // =====================================================

        private void LogisticaBtnConsultar_Click(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcRecargarTelemetrias();
                LogisticaTxtBuscarTelemetria.Focus();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        // =====================================================
        // BOTÓN MODIFICAR
        // =====================================================

        private void LogisticaBtnModificar_Click(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcCargarSeleccionado();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        // =====================================================
        // DOBLE CLIC EN EL HISTORIAL
        // =====================================================

        private void LogisticaDgvTelemetria_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                LogisticaDvgHistorialTelemetria.ClearSelection();

                LogisticaDvgHistorialTelemetria
                    .Rows[e.RowIndex].Selected = true;

                LogisticaProcCargarSeleccionado();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        // =====================================================
        // LEER DATOS DEL FORMULARIO
        // =====================================================

        private ClsDatosTelemetriaTemperatura
            LogisticaFuncLeerFormulario()
        {
            if (LogisticaCboViaje.SelectedIndex < 0)
            {
                throw new Exception(
                    "Seleccione un viaje válido.");
            }

            if (string.IsNullOrWhiteSpace(
                LogisticaTxtUbicacionGPS.Text))
            {
                throw new Exception(
                    "Ingrese la ubicación GPS.");
            }

            return new ClsDatosTelemetriaTemperatura
            {
                IdTelemetriaTemperatura =
                    _IdSeleccionado,

                FkEncabezadoViajeCodigo =
                    Convert.ToInt64(
                        LogisticaCboViaje.SelectedValue),

                FechaHoraTelemetriaTemperatura =
                    LogisticaDtpFechaHora.Value,

                TemperaturaCelsiusTelemetriaTemperatura =
                    LogisticaNudTemperatura.Value,

                HumedadRelativaTelemetriaTemperatura =
                    LogisticaNudHumedad.Value,

                PuertaAbiertaTelemetriaTemperatura =
                    LogisticaChkPuertaAbierta.Checked,

                UbicacionGpsTelemetriaTemperatura =
                    LogisticaTxtUbicacionGPS.Text.Trim()
            };
        }

        // =====================================================
        // BOTÓN GUARDAR
        // =====================================================

        private void LogisticaBtnGuardar_Click(
            object sender, EventArgs e)
        {
            if (!_ModoEdicion)
                return;

            try
            {
                var Datos = LogisticaFuncLeerFormulario();

                string Mensaje;

                if (_EsNuevo)
                {
                    Mensaje =
                        _Controlador.LogisticaFuncAgregar(Datos);
                }
                else
                {
                    Mensaje =
                        _Controlador.LogisticaFuncEditar(Datos);
                }

                MessageBox.Show(
                    Mensaje,
                    "Logística",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();
                LogisticaProcRecargarTelemetrias();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        // =====================================================
        // BOTÓN ELIMINAR
        // =====================================================

        private void LogisticaBtnEliminar_Click(
            object sender, EventArgs e)
        {
            try
            {
                var Registro = LogisticaFuncSeleccionado();

                DialogResult Confirmacion = MessageBox.Show(
                    "¿Desea eliminar el registro de telemetría " +
                    Registro.IdTelemetriaTemperatura + "?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (Confirmacion != DialogResult.Yes)
                    return;

                string Mensaje =
                    _Controlador.LogisticaFuncEliminar(
                        Registro.IdTelemetriaTemperatura);

                MessageBox.Show(
                    Mensaje,
                    "Logística",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();
                LogisticaProcRecargarTelemetrias();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        // =====================================================
        // BOTÓN CANCELAR
        // =====================================================

        private void LogisticaBtnCancelar_Click(
            object sender, EventArgs e)
        {
            LogisticaProcBloquearFormulario();
        }

        // =====================================================
        // BOTÓN REFRESCAR
        // =====================================================

        private void LogisticaBtnRefrescar_Click(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcBloquearFormulario();
                LogisticaProcCargarViajes();

                LogisticaTxtBuscarTelemetria.Clear();

                LogisticaProcRecargarTelemetrias();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        // =====================================================
        // LIMPIAR Y BLOQUEAR FORMULARIO
        // =====================================================

        private void LogisticaProcBloquearFormulario()
        {
            _ModoEdicion = false;
            _EsNuevo = false;
            _IdSeleccionado = 0;

            LogisticaTxtCodigoTelemetria.Clear();

            LogisticaCboViaje.SelectedIndex = -1;

            LogisticaDtpFechaHora.Value = DateTime.Now;

            LogisticaNudTemperatura.Value = 0;
            LogisticaNudHumedad.Value = 0;

            LogisticaChkPuertaAbierta.Checked = false;
            LogisticaTxtUbicacionGPS.Clear();

            LogisticaPnlDatosTelemetria.Enabled = false;
            LogisticaBtnGuardar.Enabled = false;
        }

        // =====================================================
        // BOTÓN IMPRIMIR
        // =====================================================

        private void LogisticaBtnImprimir_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "El reporte de telemetría está pendiente de integrar.",
                "Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =====================================================
        // BOTÓN AYUDA
        // =====================================================

        private void LogisticaBtnAyuda_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "El manual de ayuda de Telemetría " +
                "está pendiente de integrar.",
                "Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =====================================================
        // MOSTRAR ERRORES
        // =====================================================

        private void LogisticaProcMostrarError(string Mensaje)
        {
            MessageBox.Show(
                Mensaje,
                "Error - Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
