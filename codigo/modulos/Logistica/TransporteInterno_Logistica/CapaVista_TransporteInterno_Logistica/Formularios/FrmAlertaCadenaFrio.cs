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
    public partial class FrmAlertaCadenaFrio : Form
    {
        // =====================================================
        // CONTROLADOR Y REGISTROS
        // =====================================================

        private readonly ClsControladorAlertaCadenaFrio
            _Controlador = new ClsControladorAlertaCadenaFrio();

        private List<ClsDatosAlertaCadenaFrio> _Alertas =
            new List<ClsDatosAlertaCadenaFrio>();

        private List<ClsDatosAlertaCadenaFrio> _Filtrados =
            new List<ClsDatosAlertaCadenaFrio>();

        // Estado del formulario.
        private bool _ModoEdicion = false;
        private bool _EsNuevo = false;
        private long _IdSeleccionado = 0;

        // Paginación y ordenamiento.
        private int _PaginaActual = 1;
        private const int _TamanoPagina = 10;

        private string _ColumnaOrden = "IdAlertaCadenaFrio";
        private bool _OrdenAscendente = false;

        // Controles adicionales de consulta.
        private Label LogisticaLblBuscarAlerta;
        private TextBox LogisticaTxtBuscarAlerta;

        private Button LogisticaBtnAnteriorAlerta;
        private Button LogisticaBtnSiguienteAlerta;

        private Label LogisticaLblPaginacionAlerta;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public FrmAlertaCadenaFrio()
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
            // Código autoincremental.
            LogisticaTxtCodigoAlerta.ReadOnly = true;
            LogisticaTxtCodigoAlerta.TabStop = false;

            // Solo permitir telemetrías existentes.
            LogisticaCboTelemetria.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // Tipos de alerta.
            LogisticaCboTipoAlerta.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboTipoAlerta.Items.Clear();

            LogisticaCboTipoAlerta.Items.Add(
                "Temperatura alta");

            LogisticaCboTipoAlerta.Items.Add(
                "Temperatura baja");

            LogisticaCboTipoAlerta.Items.Add(
                "Puerta abierta");

            LogisticaCboTipoAlerta.Items.Add(
                "Humedad fuera de rango");

            // Observaciones.
            LogisticaTxtObservacion.MaxLength = 500;
            LogisticaTxtObservacion.Multiline = true;
            LogisticaTxtObservacion.ScrollBars =
                ScrollBars.Vertical;

            // Estado inicial.
            LogisticaChkAtendida.Checked = false;

            // =================================================
            // DATAGRIDVIEW
            // =================================================

            LogisticaDgvAlertas.AutoGenerateColumns = false;

            LogisticaDgvAlertas.ReadOnly = true;
            LogisticaDgvAlertas.MultiSelect = false;

            LogisticaDgvAlertas.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            LogisticaDgvAlertas.AllowUserToAddRows = false;
            LogisticaDgvAlertas.AllowUserToDeleteRows = false;

            LogisticaDgvAlertas.RowHeadersVisible = false;

            LogisticaDgvAlertas.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LogisticaDgvAlertas.EnableHeadersVisualStyles =
                false;

            // Colores de Logística.
            LogisticaDgvAlertas
                .ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(52, 143, 141);

            LogisticaDgvAlertas
                .ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            LogisticaDgvAlertas
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 99, 46);

            LogisticaDgvAlertas
                .DefaultCellStyle.SelectionForeColor =
                Color.White;

            LogisticaDgvAlertas
                .AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(251, 246, 242);

            // Vincular columnas del diseñador.
            ColCodigoAlerta.DataPropertyName =
                "IdAlertaCadenaFrio";

            ColTelemetria.DataPropertyName =
                "NombreTelemetria";

            ColTipoAlerta.DataPropertyName =
                "TipoAlertaCadenaFrio";

            ColAtendida.DataPropertyName =
                "AtendidaAlertaCadenaFrio";

            ColObservacion.DataPropertyName =
                "ObservacionCalidadAlertaCadenaFrio";

            foreach (DataGridViewColumn Columna
                in LogisticaDgvAlertas.Columns)
            {
                Columna.SortMode =
                    DataGridViewColumnSortMode.Programmatic;
            }
        }

        // =====================================================
        // CREAR BÚSQUEDA Y PAGINACIÓN
        // =====================================================

        private void LogisticaProcCrearConsultaVisual()
        {
            Color Verde = Color.FromArgb(52, 143, 141);
            Color Botones = Color.FromArgb(242, 216, 197);

            int X = LogisticaDgvAlertas.Left;
            int Y = LogisticaDgvAlertas.Top;

            // Etiqueta Buscar.
            LogisticaLblBuscarAlerta = new Label
            {
                Name = "LogisticaLblBuscarAlerta",
                Text = "Buscar:",
                Location = new Point(X, Y + 3),
                Size = new Size(75, 25),
                ForeColor = Verde,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f)
            };

            // Campo de búsqueda.
            LogisticaTxtBuscarAlerta = new TextBox
            {
                Name = "LogisticaTxtBuscarAlerta",
                Location = new Point(X + 80, Y),
                Size = new Size(350, 28),
                Font = new Font("Segoe UI", 10f)
            };

            // Espacio para búsqueda.
            LogisticaDgvAlertas.Top = Y + 40;

            // Botón Anterior.
            LogisticaBtnAnteriorAlerta = new Button
            {
                Name = "LogisticaBtnAnteriorAlerta",
                Text = "Anterior",
                Location = new Point(
                    X, LogisticaDgvAlertas.Bottom + 12),
                Size = new Size(95, 30),
                BackColor = Botones
            };

            // Botón Siguiente.
            LogisticaBtnSiguienteAlerta = new Button
            {
                Name = "LogisticaBtnSiguienteAlerta",
                Text = "Siguiente",
                Location = new Point(
                    X + 105,
                    LogisticaDgvAlertas.Bottom + 12),
                Size = new Size(95, 30),
                BackColor = Botones
            };

            // Etiqueta de paginación.
            LogisticaLblPaginacionAlerta = new Label
            {
                Name = "LogisticaLblPaginacionAlerta",
                Location = new Point(
                    X + 215,
                    LogisticaDgvAlertas.Bottom + 16),
                Size = new Size(430, 25),
                ForeColor = Verde,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f)
            };

            // Agregar al panel existente.
            LogisticaPnlHistorialAlertas.Controls.Add(
                LogisticaLblBuscarAlerta);

            LogisticaPnlHistorialAlertas.Controls.Add(
                LogisticaTxtBuscarAlerta);

            LogisticaPnlHistorialAlertas.Controls.Add(
                LogisticaBtnAnteriorAlerta);

            LogisticaPnlHistorialAlertas.Controls.Add(
                LogisticaBtnSiguienteAlerta);

            LogisticaPnlHistorialAlertas.Controls.Add(
                LogisticaLblPaginacionAlerta);

            // Ampliar el panel.
            LogisticaPnlHistorialAlertas.Height = Math.Max(
                LogisticaPnlHistorialAlertas.Height,
                LogisticaBtnAnteriorAlerta.Bottom + 15);

            // Búsqueda automática.
            LogisticaTxtBuscarAlerta.TextChanged +=
                LogisticaTxtBuscarAlerta_TextChanged;

            // Paginación anterior.
            LogisticaBtnAnteriorAlerta.Click += (sender, e) =>
            {
                if (_PaginaActual > 1)
                {
                    _PaginaActual--;
                    LogisticaProcMostrarPagina();
                }
            };

            // Paginación siguiente.
            LogisticaBtnSiguienteAlerta.Click += (sender, e) =>
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
            Load -= FrmAlertaCadenaFrio_Load;
            Load += FrmAlertaCadenaFrio_Load;

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

            LogisticaDgvAlertas.CellDoubleClick -=
                LogisticaDgvAlertas_CellDoubleClick;

            LogisticaDgvAlertas.CellDoubleClick +=
                LogisticaDgvAlertas_CellDoubleClick;

            LogisticaDgvAlertas.ColumnHeaderMouseClick -=
                LogisticaDgvAlertas_ColumnHeaderMouseClick;

            LogisticaDgvAlertas.ColumnHeaderMouseClick +=
                LogisticaDgvAlertas_ColumnHeaderMouseClick;
        }

        // =====================================================
        // EVENTO LOAD
        // =====================================================

        private void FrmAlertaCadenaFrio_Load(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcCargarTelemetrias();
                LogisticaProcRecargarAlertas();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(
                    "No se pudieron cargar los datos: " +
                    ex.Message);
            }
        }

        // =====================================================
        // CARGAR TELEMETRÍAS
        // =====================================================

        private void LogisticaProcCargarTelemetrias()
        {
            LogisticaCboTelemetria.DataSource = null;

            LogisticaCboTelemetria.DisplayMember = "Nombre";
            LogisticaCboTelemetria.ValueMember = "Id";

            LogisticaCboTelemetria.DataSource =
                _Controlador.LogisticaFuncTelemetrias();

            LogisticaCboTelemetria.SelectedIndex = -1;
        }

        // =====================================================
        // CONSULTAR ALERTAS
        // =====================================================

        private void LogisticaProcRecargarAlertas()
        {
            _Alertas = _Controlador.LogisticaFuncObtenerTodos();

            LogisticaProcFiltrarAlertas();
        }

        // =====================================================
        // FILTRAR ALERTAS
        // =====================================================

        private void LogisticaProcFiltrarAlertas()
        {
            string Busqueda =
                LogisticaTxtBuscarAlerta.Text.Trim();

            _Filtrados = _Alertas.Where(A =>
                A.IdAlertaCadenaFrio.ToString()
                    .Contains(Busqueda) ||

                (A.NombreTelemetria ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (A.TipoAlertaCadenaFrio ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (A.ObservacionCalidadAlertaCadenaFrio ?? "")
                    .IndexOf(
                        Busqueda,
                        StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            LogisticaProcOrdenarAlertas();

            _PaginaActual = 1;
            LogisticaProcMostrarPagina();
        }

        private void LogisticaTxtBuscarAlerta_TextChanged(
            object sender, EventArgs e)
        {
            LogisticaProcFiltrarAlertas();
        }

        // =====================================================
        // ORDENAR ALERTAS
        // =====================================================

        private void LogisticaProcOrdenarAlertas()
        {
            PropertyInfo Propiedad =
                typeof(ClsDatosAlertaCadenaFrio)
                    .GetProperty(_ColumnaOrden);

            if (Propiedad == null)
                return;

            _Filtrados = _OrdenAscendente
                ? _Filtrados.OrderBy(
                    A => Propiedad.GetValue(A, null)).ToList()
                : _Filtrados.OrderByDescending(
                    A => Propiedad.GetValue(A, null)).ToList();
        }

        private void LogisticaDgvAlertas_ColumnHeaderMouseClick(
            object sender,
            DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            string Nombre =
                LogisticaDgvAlertas.Columns[e.ColumnIndex]
                    .DataPropertyName;

            if (string.IsNullOrWhiteSpace(Nombre))
                return;

            _OrdenAscendente =
                _ColumnaOrden == Nombre
                    ? !_OrdenAscendente
                    : true;

            _ColumnaOrden = Nombre;

            LogisticaProcOrdenarAlertas();

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
            LogisticaDgvAlertas.DataSource = null;

            LogisticaDgvAlertas.DataSource =
                _Filtrados
                    .Skip((_PaginaActual - 1) * _TamanoPagina)
                    .Take(_TamanoPagina)
                    .ToList();

            LogisticaDgvAlertas.ClearSelection();

            LogisticaLblPaginacionAlerta.Text =
                string.Format(
                    "Página {0} de {1} | {2} alertas",
                    _PaginaActual,
                    LogisticaFuncTotalPaginas(),
                    _Filtrados.Count);

            LogisticaBtnAnteriorAlerta.Enabled =
                _PaginaActual > 1;

            LogisticaBtnSiguienteAlerta.Enabled =
                _PaginaActual < LogisticaFuncTotalPaginas();
        }

        // =====================================================
        // OBTENER ALERTA SELECCIONADA
        // =====================================================

        private ClsDatosAlertaCadenaFrio
            LogisticaFuncSeleccionado()
        {
            if (LogisticaDgvAlertas.SelectedRows.Count == 0)
            {
                throw new Exception(
                    "Seleccione una alerta del historial.");
            }

            var Alerta =
                LogisticaDgvAlertas.SelectedRows[0]
                    .DataBoundItem as ClsDatosAlertaCadenaFrio;

            if (Alerta == null)
            {
                throw new Exception(
                    "No se pudo identificar la alerta.");
            }

            return Alerta;
        }

        // =====================================================
        // CARGAR ALERTA PARA MODIFICAR
        // =====================================================

        private void LogisticaProcCargarSeleccionado()
        {
            var Alerta = LogisticaFuncSeleccionado();

            _IdSeleccionado = Alerta.IdAlertaCadenaFrio;
            _EsNuevo = false;
            _ModoEdicion = true;

            LogisticaPnlDatosAlerta.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaTxtCodigoAlerta.Text =
                Alerta.IdAlertaCadenaFrio.ToString();

            LogisticaCboTelemetria.SelectedValue =
                Alerta.FkTelemetriaTemperaturaCodigo;

            // Conservar el valor de registros anteriores.
            if (!LogisticaCboTipoAlerta.Items.Contains(
                Alerta.TipoAlertaCadenaFrio))
            {
                LogisticaCboTipoAlerta.Items.Add(
                    Alerta.TipoAlertaCadenaFrio);
            }

            LogisticaCboTipoAlerta.SelectedItem =
                Alerta.TipoAlertaCadenaFrio;

            LogisticaChkAtendida.Checked =
                Alerta.AtendidaAlertaCadenaFrio;

            LogisticaTxtObservacion.Text =
                Alerta.ObservacionCalidadAlertaCadenaFrio;
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

            LogisticaPnlDatosAlerta.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaChkAtendida.Checked = false;
            LogisticaCboTelemetria.Focus();
        }

        // =====================================================
        // BOTÓN CONSULTAR
        // =====================================================

        private void LogisticaBtnConsultar_Click(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcRecargarAlertas();
                LogisticaTxtBuscarAlerta.Focus();
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

        private void LogisticaDgvAlertas_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                LogisticaDgvAlertas.ClearSelection();
                LogisticaDgvAlertas.Rows[e.RowIndex].Selected =
                    true;

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

        private ClsDatosAlertaCadenaFrio
            LogisticaFuncLeerFormulario()
        {
            if (LogisticaCboTelemetria.SelectedIndex < 0)
            {
                throw new Exception(
                    "Seleccione un registro de telemetría.");
            }

            if (LogisticaCboTipoAlerta.SelectedIndex < 0)
            {
                throw new Exception(
                    "Seleccione un tipo de alerta.");
            }

            if (string.IsNullOrWhiteSpace(
                LogisticaTxtObservacion.Text))
            {
                throw new Exception(
                    "Ingrese una observación.");
            }

            return new ClsDatosAlertaCadenaFrio
            {
                IdAlertaCadenaFrio =
                    _IdSeleccionado,

                FkTelemetriaTemperaturaCodigo =
                    Convert.ToInt64(
                        LogisticaCboTelemetria.SelectedValue),

                TipoAlertaCadenaFrio =
                    LogisticaCboTipoAlerta.SelectedItem.ToString(),

                AtendidaAlertaCadenaFrio =
                    LogisticaChkAtendida.Checked,

                ObservacionCalidadAlertaCadenaFrio =
                    LogisticaTxtObservacion.Text.Trim()
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
                LogisticaProcRecargarAlertas();
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
                var Alerta = LogisticaFuncSeleccionado();

                DialogResult Confirmacion = MessageBox.Show(
                    "¿Desea eliminar la alerta número " +
                    Alerta.IdAlertaCadenaFrio + "?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (Confirmacion != DialogResult.Yes)
                    return;

                string Mensaje =
                    _Controlador.LogisticaFuncEliminar(
                        Alerta.IdAlertaCadenaFrio);

                MessageBox.Show(
                    Mensaje,
                    "Logística",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();
                LogisticaProcRecargarAlertas();
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
                LogisticaProcCargarTelemetrias();

                LogisticaTxtBuscarAlerta.Clear();

                LogisticaProcRecargarAlertas();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        // =====================================================
        // LIMPIAR Y BLOQUEAR
        // =====================================================

        private void LogisticaProcBloquearFormulario()
        {
            _ModoEdicion = false;
            _EsNuevo = false;
            _IdSeleccionado = 0;

            LogisticaTxtCodigoAlerta.Clear();

            LogisticaCboTelemetria.SelectedIndex = -1;
            LogisticaCboTipoAlerta.SelectedIndex = -1;

            LogisticaChkAtendida.Checked = false;
            LogisticaTxtObservacion.Clear();

            LogisticaPnlDatosAlerta.Enabled = false;
            LogisticaBtnGuardar.Enabled = false;
        }

        // =====================================================
        // BOTÓN IMPRIMIR
        // =====================================================

        private void LogisticaBtnImprimir_Click(
            object sender, EventArgs e)
        {
            
        }

        // =====================================================
        // BOTÓN AYUDA
        // =====================================================

        private void LogisticaBtnAyuda_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "El manual de ayuda de Alertas de Cadena de Frío " +
                "está pendiente de integrar.",
                "Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =====================================================
        // OBTENER DATOS PARA REPORTE DE ALERTAS
        // =====================================================

        private DataTable LogisticaMetObtenerDatosReporte()
        {
            DataTable Tabla = new DataTable();

            Tabla.Columns.Add(
                "IdAlertaCadenaFrio", typeof(long));

            Tabla.Columns.Add(
                "FkTelemetriaTemperaturaCodigo",
                typeof(long));

            Tabla.Columns.Add(
                "TipoAlertaCadenaFrio", typeof(string));

            Tabla.Columns.Add(
                "Atendida", typeof(string));

            Tabla.Columns.Add(
                "ObservacionCalidadAlertaCadenaFrio",
                typeof(string));

            // Consultar datos desde el controlador existente.
            var Controlador =
                new ClsControladorAlertaCadenaFrio();

            var Registros =
                Controlador.LogisticaFuncObtenerTodos();

            foreach (var Alerta in Registros)
            {
                DataRow Fila = Tabla.NewRow();

                Fila["IdAlertaCadenaFrio"] =
                    Alerta.IdAlertaCadenaFrio;

                Fila["FkTelemetriaTemperaturaCodigo"] =
                    Alerta.FkTelemetriaTemperaturaCodigo;

                Fila["TipoAlertaCadenaFrio"] =
                    Alerta.TipoAlertaCadenaFrio ?? "";

                Fila["Atendida"] =
                    Alerta.AtendidaAlertaCadenaFrio
                    ? "Sí"
                    : "No";

                Fila["ObservacionCalidadAlertaCadenaFrio"] =
                    Alerta.ObservacionCalidadAlertaCadenaFrio
                    ?? "";

                Tabla.Rows.Add(Fila);
            }

            return Tabla;
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

        private void LogisticaBtnImprimir_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Obtener todos los registros de alertas.
                DataTable TablaDatos =
                    LogisticaMetObtenerDatosReporte();

                // Abrir el formulario del reporte.
                using (var Formulario =
                    new FrmReporteAlertaCadenaFrio(TablaDatos))
                {
                    Formulario.ShowDialog(this);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(
                    "No se pudo generar el reporte de alertas:\n"
                    + Ex.Message,
                    "Reporte de Alertas de Cadena de Frío",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
