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


using CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores;


namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    public partial class FrmEncabezadoViaje : Form
    {
        // =====================================================
        // CONTROLADOR Y REGISTROS
        // =====================================================

        private readonly ClsControladorEncabezadoViaje _Controlador =
            new ClsControladorEncabezadoViaje();

        private List<ClsDatosEncabezadoViaje> _Viajes =
            new List<ClsDatosEncabezadoViaje>();

        private List<ClsDatosEncabezadoViaje> _ViajesFiltrados =
            new List<ClsDatosEncabezadoViaje>();
        // Estado de edición.
        private bool _ModoEdicion = false;
        private bool _EsNuevo = false;
        private long _IdSeleccionado = 0;

        // Paginación y ordenamiento.
        private int _PaginaActual = 1;
        private const int _TamanoPagina = 10;

        private string _ColumnaOrden =
            "IdEncabezadoViaje";

        private bool _OrdenAscendente = false;

        // Controles de búsqueda.
        private Label LogisticaLblBuscarViaje;
        private TextBox LogisticaTxtBuscarViaje;
        private Button LogisticaBtnAnteriorViaje;
        private Button LogisticaBtnSiguienteViaje;
        private Label LogisticaLblPaginacionViaje;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public FrmEncabezadoViaje()
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
            LogisticaTxtCodigoViaje.ReadOnly = true;
            LogisticaTxtCodigoViaje.TabStop = false;

            LogisticaCboTransporte.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboConductor.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboEstadoViaje.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboEstadoViaje.Items.Clear();
            LogisticaCboEstadoViaje.Items.Add("EN_CURSO");
            LogisticaCboEstadoViaje.Items.Add("FINALIZADO");

            LogisticaDtpFechaDocumento.Format =
                DateTimePickerFormat.Short;

            LogisticaDtpHoraSalida.Format =
                DateTimePickerFormat.Custom;

            LogisticaDtpHoraSalida.CustomFormat =
                "dd/MM/yyyy HH:mm";

            LogisticaDtpHoraSalida.ShowUpDown = true;

            LogisticaDtpHoraLlegada.Format =
                DateTimePickerFormat.Custom;

            LogisticaDtpHoraLlegada.CustomFormat =
                "dd/MM/yyyy HH:mm";

            LogisticaDtpHoraLlegada.ShowUpDown = true;
            LogisticaDtpHoraLlegada.ShowCheckBox = true;

            LogisticaNudPesoTotal.DecimalPlaces = 3;
            LogisticaNudPesoTotal.Minimum = 0;
            LogisticaNudPesoTotal.Maximum = 999999999;

            LogisticaDgvViajes.AutoGenerateColumns = false;
            LogisticaDgvViajes.ReadOnly = true;
            LogisticaDgvViajes.MultiSelect = false;

            LogisticaDgvViajes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            LogisticaDgvViajes.AllowUserToAddRows = false;
            LogisticaDgvViajes.AllowUserToDeleteRows = false;
            LogisticaDgvViajes.RowHeadersVisible = false;

            LogisticaDgvViajes.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LogisticaDgvViajes.EnableHeadersVisualStyles = false;

            LogisticaDgvViajes
                .ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(52, 143, 141);

            LogisticaDgvViajes
                .ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            LogisticaDgvViajes
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 99, 46);

            LogisticaDgvViajes
                .AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(251, 246, 242);

            // Relacionar columnas con las propiedades.
            ColCodigoViaje.DataPropertyName =
                "IdEncabezadoViaje";

            ColFechaDocumento.DataPropertyName =
                "FechaDocumentoEncabezadoViaje";

            ColHoraSalida.DataPropertyName =
                "HoraSalidaEncabezadoViaje";

            ColHoraLlegada.DataPropertyName =
                "HoraLlegadaEncabezadoViaje";

            ColCadenaFrio.DataPropertyName =
                "RequiereCadenaFrioEncabezadoViaje";

            ColEstadoViaje.DataPropertyName =
                "EstadoViajeEncabezadoViaje";

            ColPesoTotal.DataPropertyName =
                "PesoTotalEncabezadoViaje";

            ColTransporte.DataPropertyName =
                "PlacaTransporte";

            ColConductor.DataPropertyName =
                "NombreConductor";

            ColFechaDocumento.DefaultCellStyle.Format =
                "dd/MM/yyyy";

            ColHoraSalida.DefaultCellStyle.Format =
                "dd/MM/yyyy HH:mm";

            ColHoraLlegada.DefaultCellStyle.Format =
                "dd/MM/yyyy HH:mm";

            ColPesoTotal.DefaultCellStyle.Format = "N3";

            foreach (DataGridViewColumn Columna
                in LogisticaDgvViajes.Columns)
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

            int X = LogisticaDgvViajes.Left;
            int Y = LogisticaDgvViajes.Top;

            LogisticaLblBuscarViaje = new Label
            {
                Name = "LogisticaLblBuscarViaje",
                Text = "Buscar:",
                Location = new Point(X, Y),
                Size = new Size(75, 28),
                ForeColor = Verde,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f)
            };

            LogisticaTxtBuscarViaje = new TextBox
            {
                Name = "LogisticaTxtBuscarViaje",
                Location = new Point(X + 80, Y),
                Size = new Size(350, 28),
                Font = new Font("Segoe UI", 10f)
            };

            LogisticaTxtBuscarViaje.TextChanged +=
                LogisticaTxtBuscarViaje_TextChanged;

            // Dejar espacio para el buscador.
            LogisticaDgvViajes.Top = Y + 42;

            LogisticaBtnAnteriorViaje = new Button
            {
                Name = "LogisticaBtnAnteriorViaje",
                Text = "Anterior",
                Location = new Point(
                    X, LogisticaDgvViajes.Bottom + 12),
                Size = new Size(95, 30),
                BackColor = Botones
            };

            LogisticaBtnSiguienteViaje = new Button
            {
                Name = "LogisticaBtnSiguienteViaje",
                Text = "Siguiente",
                Location = new Point(
                    X + 105, LogisticaDgvViajes.Bottom + 12),
                Size = new Size(95, 30),
                BackColor = Botones
            };

            LogisticaLblPaginacionViaje = new Label
            {
                Name = "LogisticaLblPaginacionViaje",
                Location = new Point(
                    X + 215, LogisticaDgvViajes.Bottom + 16),
                Size = new Size(500, 24),
                ForeColor = Verde,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f)
            };

            LogisticaPnlHistorial.Controls.Add(
                LogisticaLblBuscarViaje);

            LogisticaPnlHistorial.Controls.Add(
                LogisticaTxtBuscarViaje);

            LogisticaPnlHistorial.Controls.Add(
                LogisticaBtnAnteriorViaje);

            LogisticaPnlHistorial.Controls.Add(
                LogisticaBtnSiguienteViaje);

            LogisticaPnlHistorial.Controls.Add(
                LogisticaLblPaginacionViaje);

            LogisticaPnlHistorial.Height = Math.Max(
                LogisticaPnlHistorial.Height,
                LogisticaBtnAnteriorViaje.Bottom + 15);

            LogisticaBtnAnteriorViaje.Click += (sender, e) =>
            {
                if (_PaginaActual > 1)
                {
                    _PaginaActual--;
                    LogisticaProcMostrarPagina();
                }
            };

            LogisticaBtnSiguienteViaje.Click += (sender, e) =>
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
            Load -= FrmEncabezadoViaje_Load;
            Load += FrmEncabezadoViaje_Load;

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

            LogisticaDgvViajes.CellDoubleClick -=
                LogisticaDgvViajes_CellDoubleClick;

            LogisticaDgvViajes.CellDoubleClick +=
                LogisticaDgvViajes_CellDoubleClick;

            LogisticaDgvViajes.ColumnHeaderMouseClick -=
                LogisticaDgvViajes_ColumnHeaderMouseClick;

            LogisticaDgvViajes.ColumnHeaderMouseClick +=
                LogisticaDgvViajes_ColumnHeaderMouseClick;
        }

        // =====================================================
        // EVENTO LOAD
        // =====================================================

        private void FrmEncabezadoViaje_Load(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcCargarCatalogos();
                LogisticaProcRecargarViajes();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(
                    "No se pudieron cargar los datos: " +
                    ex.Message);
            }
        }

        // =====================================================
        // CARGAR CATÁLOGOS
        // =====================================================

        private void LogisticaProcCargarCatalogos()
        {
            var Transportes =
                _Controlador.LogisticaFuncTransportes();

            LogisticaCboTransporte.DataSource = null;
            LogisticaCboTransporte.DisplayMember = "Nombre";
            LogisticaCboTransporte.ValueMember = "Id";
            LogisticaCboTransporte.DataSource = Transportes;
            LogisticaCboTransporte.SelectedIndex = -1;

            var Conductores =
                _Controlador.LogisticaFuncConductores();

            LogisticaCboConductor.DataSource = null;
            LogisticaCboConductor.DisplayMember = "Nombre";
            LogisticaCboConductor.ValueMember = "Id";
            LogisticaCboConductor.DataSource = Conductores;
            LogisticaCboConductor.SelectedIndex = -1;
        }

        // =====================================================
        // CONSULTAR Y FILTRAR
        // =====================================================

        private void LogisticaProcRecargarViajes()
        {
            _Viajes = _Controlador.LogisticaFuncObtenerTodos();

            LogisticaProcFiltrarViajes();
        }

        private void LogisticaProcFiltrarViajes()
        {
            string Busqueda = LogisticaTxtBuscarViaje.Text.Trim();

            _ViajesFiltrados = _Viajes.Where(V =>
                V.IdEncabezadoViaje.ToString().Contains(Busqueda) ||

                (V.PlacaTransporte ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (V.NombreConductor ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (V.EstadoViajeEncabezadoViaje ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            LogisticaProcOrdenarViajes();

            _PaginaActual = 1;
            LogisticaProcMostrarPagina();
        }

        // =====================================================
        // ORDENAMIENTO
        // =====================================================

        private void LogisticaProcOrdenarViajes()
        {
            PropertyInfo Propiedad =
                typeof(ClsDatosEncabezadoViaje).GetProperty(
                    _ColumnaOrden);

            if (Propiedad == null)
                return;

            _ViajesFiltrados = _OrdenAscendente
                ? _ViajesFiltrados.OrderBy(
                    V => Propiedad.GetValue(V, null)).ToList()
                : _ViajesFiltrados.OrderByDescending(
                    V => Propiedad.GetValue(V, null)).ToList();
        }

        private void LogisticaDgvViajes_ColumnHeaderMouseClick(
            object sender,
            DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            string Nombre =
                LogisticaDgvViajes.Columns[e.ColumnIndex]
                    .DataPropertyName;

            if (string.IsNullOrEmpty(Nombre))
                return;

            _OrdenAscendente =
                _ColumnaOrden == Nombre
                    ? !_OrdenAscendente
                    : true;

            _ColumnaOrden = Nombre;

            LogisticaProcOrdenarViajes();

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
                (_ViajesFiltrados.Count + _TamanoPagina - 1)
                    / _TamanoPagina);
        }

        private void LogisticaProcMostrarPagina()
        {
            LogisticaDgvViajes.DataSource = null;

            LogisticaDgvViajes.DataSource =
                _ViajesFiltrados
                    .Skip((_PaginaActual - 1) * _TamanoPagina)
                    .Take(_TamanoPagina)
                    .ToList();

            LogisticaDgvViajes.ClearSelection();

            LogisticaLblPaginacionViaje.Text = string.Format(
                "Página {0} de {1} | {2} viajes",
                _PaginaActual,
                LogisticaFuncTotalPaginas(),
                _ViajesFiltrados.Count);

            LogisticaBtnAnteriorViaje.Enabled =
                _PaginaActual > 1;

            LogisticaBtnSiguienteViaje.Enabled =
                _PaginaActual < LogisticaFuncTotalPaginas();
        }

        private void LogisticaTxtBuscarViaje_TextChanged(
            object sender, EventArgs e)
        {
            LogisticaProcFiltrarViajes();
        }

        // =====================================================
        // OBTENER REGISTRO SELECCIONADO
        // =====================================================

        private ClsDatosEncabezadoViaje LogisticaFuncSeleccionado()
        {
            if (LogisticaDgvViajes.SelectedRows.Count == 0)
            {
                throw new Exception(
                    "Seleccione un viaje del historial.");
            }

            ClsDatosEncabezadoViaje Viaje =
                LogisticaDgvViajes.SelectedRows[0].DataBoundItem
                as ClsDatosEncabezadoViaje;

            if (Viaje == null)
            {
                throw new Exception(
                    "No se pudo identificar el viaje seleccionado.");
            }

            return Viaje;
        }

        // =====================================================
        // CARGAR VIAJE PARA MODIFICAR
        // =====================================================

        private void LogisticaProcCargarSeleccionado()
        {
            ClsDatosEncabezadoViaje Viaje =
                LogisticaFuncSeleccionado();

            _IdSeleccionado = Viaje.IdEncabezadoViaje;
            _EsNuevo = false;
            _ModoEdicion = true;

            LogisticaPnlDatosViaje.Enabled = true;
            LogisticaPnlAsignacion.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaTxtCodigoViaje.Text =
                Viaje.IdEncabezadoViaje.ToString();

            LogisticaDtpFechaDocumento.Value =
                Viaje.FechaDocumentoEncabezadoViaje;

            LogisticaDtpHoraSalida.Value =
                Viaje.HoraSalidaEncabezadoViaje;

            if (Viaje.HoraLlegadaEncabezadoViaje.HasValue)
            {
                LogisticaDtpHoraLlegada.Value =
                    Viaje.HoraLlegadaEncabezadoViaje.Value;

                LogisticaDtpHoraLlegada.Checked = true;
            }
            else
            {
                LogisticaDtpHoraLlegada.Value = DateTime.Now;
                LogisticaDtpHoraLlegada.Checked = false;
            }

            LogisticaChkCadenaFrio.Checked =
                Viaje.RequiereCadenaFrioEncabezadoViaje;

            if (!LogisticaCboEstadoViaje.Items.Contains(
                Viaje.EstadoViajeEncabezadoViaje))
            {
                LogisticaCboEstadoViaje.Items.Add(
                    Viaje.EstadoViajeEncabezadoViaje);
            }

            LogisticaCboEstadoViaje.SelectedItem =
                Viaje.EstadoViajeEncabezadoViaje;

            LogisticaNudPesoTotal.Value =
                Math.Max(
                    LogisticaNudPesoTotal.Minimum,
                    Math.Min(
                        LogisticaNudPesoTotal.Maximum,
                        Viaje.PesoTotalEncabezadoViaje));

            LogisticaCboTransporte.SelectedValue =
                Viaje.Fk_Transporte_Codigo;

            LogisticaCboConductor.SelectedValue =
                Viaje.Fk_Conductor_Codigo;
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

            LogisticaPnlDatosViaje.Enabled = true;
            LogisticaPnlAsignacion.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaCboEstadoViaje.SelectedItem = "EN_CURSO";
            LogisticaDtpFechaDocumento.Value = DateTime.Now;
            LogisticaDtpHoraSalida.Value = DateTime.Now;

            LogisticaDtpHoraLlegada.Value = DateTime.Now;
            LogisticaDtpHoraLlegada.Checked = false;

            LogisticaCboTransporte.Focus();
        }

        // =====================================================
        // BOTÓN CONSULTAR
        // =====================================================

        private void LogisticaBtnConsultar_Click(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcRecargarViajes();
                LogisticaTxtBuscarViaje.Focus();
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

        private void LogisticaDgvViajes_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                LogisticaDgvViajes.ClearSelection();
                LogisticaDgvViajes.Rows[e.RowIndex].Selected = true;
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

        private ClsDatosEncabezadoViaje LogisticaFuncLeerFormulario()
        {
            if (LogisticaCboTransporte.SelectedIndex < 0 ||
                LogisticaCboConductor.SelectedIndex < 0 ||
                LogisticaCboEstadoViaje.SelectedIndex < 0)
            {
                throw new Exception(
                    "Complete todos los campos obligatorios.");
            }

            return new ClsDatosEncabezadoViaje
            {
                IdEncabezadoViaje = _IdSeleccionado,

                FechaDocumentoEncabezadoViaje =
                    LogisticaDtpFechaDocumento.Value.Date,

                HoraSalidaEncabezadoViaje =
                    LogisticaDtpHoraSalida.Value,

                HoraLlegadaEncabezadoViaje =
                    LogisticaDtpHoraLlegada.Checked
                        ? (DateTime?)LogisticaDtpHoraLlegada.Value
                        : null,

                RequiereCadenaFrioEncabezadoViaje =
                    LogisticaChkCadenaFrio.Checked,

                EstadoViajeEncabezadoViaje =
                    LogisticaCboEstadoViaje.SelectedItem.ToString(),

                PesoTotalEncabezadoViaje =
                    LogisticaNudPesoTotal.Value,

                Fk_Transporte_Codigo =
                    Convert.ToInt32(
                        LogisticaCboTransporte.SelectedValue),

                Fk_Conductor_Codigo =
                    Convert.ToInt32(
                        LogisticaCboConductor.SelectedValue)
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
                ClsDatosEncabezadoViaje Viaje =
                    LogisticaFuncLeerFormulario();

                string Mensaje;

                if (_EsNuevo)
                {
                    Mensaje =
                        _Controlador.LogisticaFuncAgregar(Viaje);
                }
                else
                {
                    Mensaje =
                        _Controlador.LogisticaFuncEditar(Viaje);
                }

                MessageBox.Show(
                    Mensaje,
                    "Logística",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();
                LogisticaProcRecargarViajes();
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
                ClsDatosEncabezadoViaje Viaje =
                    LogisticaFuncSeleccionado();

                DialogResult Confirmacion = MessageBox.Show(
                    "¿Desea eliminar el viaje " +
                    Viaje.IdEncabezadoViaje + "?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (Confirmacion != DialogResult.Yes)
                    return;

                string Mensaje =
                    _Controlador.LogisticaFuncEliminar(
                        Viaje.IdEncabezadoViaje);

                MessageBox.Show(
                    Mensaje,
                    "Logística",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();
                LogisticaProcRecargarViajes();
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
                LogisticaProcCargarCatalogos();

                LogisticaTxtBuscarViaje.Clear();
                LogisticaProcRecargarViajes();
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

            LogisticaTxtCodigoViaje.Clear();

            LogisticaDtpFechaDocumento.Value = DateTime.Now;
            LogisticaDtpHoraSalida.Value = DateTime.Now;

            LogisticaDtpHoraLlegada.Value = DateTime.Now;
            LogisticaDtpHoraLlegada.Checked = false;

            LogisticaChkCadenaFrio.Checked = false;

            LogisticaCboEstadoViaje.SelectedIndex = -1;
            LogisticaCboTransporte.SelectedIndex = -1;
            LogisticaCboConductor.SelectedIndex = -1;

            LogisticaNudPesoTotal.Value = 0;

            LogisticaPnlDatosViaje.Enabled = false;
            LogisticaPnlAsignacion.Enabled = false;
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
                "El manual de ayuda de Encabezado de Viaje " +
                "está pendiente de integrar.",
                "Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }


        private DataTable LogisticaMetObtenerDatosReporte()
        {
            // Crear tabla para el ReportViewer.
            DataTable Tabla = new DataTable();

            // Definir columnas del reporte.
            Tabla.Columns.Add("CodigoViaje", typeof(string));
            Tabla.Columns.Add("FechaDocumento", typeof(DateTime));
            Tabla.Columns.Add("FechaSalida", typeof(DateTime));
            Tabla.Columns.Add("FechaLlegada", typeof(DateTime));
            Tabla.Columns.Add("Transporte", typeof(string));
            Tabla.Columns.Add("Conductor", typeof(string));
            Tabla.Columns.Add("EstadoViaje", typeof(string));
            Tabla.Columns.Add("PesoTotal", typeof(decimal));

            Tabla.Columns.Add("CadenaFrio", typeof(string));
            Tabla.Columns.Add("IdTransporte", typeof(int));
            Tabla.Columns.Add("IdConductor", typeof(int));
            Tabla.Columns.Add("FechaCreacion", typeof(DateTime));
            Tabla.Columns.Add("FechaActualizacion", typeof(DateTime));

            // Utilizar el controlador existente.
            ClsControladorEncabezadoViaje Controlador =
                new ClsControladorEncabezadoViaje();

            // Obtener todos los registros de MySQL.
            var Registros = Controlador.LogisticaFuncObtenerTodos();

            // Recorrer los registros.
            foreach (var Viaje in Registros)
            {
                DataRow Fila = Tabla.NewRow();

                Fila["CodigoViaje"] =
                    Viaje.IdEncabezadoViaje.ToString();

                Fila["FechaDocumento"] =
                    Viaje.FechaDocumentoEncabezadoViaje;

                Fila["FechaSalida"] =
                    Viaje.HoraSalidaEncabezadoViaje;

                // La fecha de llegada puede estar vacía.
                Fila["FechaLlegada"] =
                    Viaje.HoraLlegadaEncabezadoViaje.HasValue
                    ? (object)Viaje.HoraLlegadaEncabezadoViaje.Value
                    : DBNull.Value;

                Fila["Transporte"] =
                    Viaje.PlacaTransporte ?? "";

                Fila["Conductor"] =
                    Viaje.NombreConductor ?? "";

                Fila["EstadoViaje"] =
                    Viaje.EstadoViajeEncabezadoViaje ?? "";

                Fila["PesoTotal"] =
                    Viaje.PesoTotalEncabezadoViaje;

                Fila["CadenaFrio"] =
                    Viaje.RequiereCadenaFrioEncabezadoViaje
                    ? "Sí"
                    : "No";

                Fila["IdTransporte"] =
                    Viaje.Fk_Transporte_Codigo;

                Fila["IdConductor"] =
                    Viaje.Fk_Conductor_Codigo;

                Fila["FechaCreacion"] =
                    Viaje.CreatedAt;

                Fila["FechaActualizacion"] =
                    Viaje.UpdatedAt;

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
                // Obtener los registros desde el controlador.
                DataTable TablaDatos =
                    LogisticaMetObtenerDatosReporte();

                // Abrir la ventana del reporte.
                using (FrmReporteEncabezadoViaje Formulario =
                    new FrmReporteEncabezadoViaje(TablaDatos))
                {
                    Formulario.ShowDialog(this);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(
                    "No se pudo generar el reporte:\n" + Ex.Message,
                    "Reporte de Encabezado de Viaje",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
