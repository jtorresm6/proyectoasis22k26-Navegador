using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores;

namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    public partial class FrmDetalleViajeProducto : Form
    {
        // =====================================================
        // CONTROLADOR Y REGISTROS
        // =====================================================

        private readonly ClsControladorDetalleViajeLoteProducto
            _Controlador =
                new ClsControladorDetalleViajeLoteProducto();

        private List<ClsDatosDetalleViajeLoteProducto> _Asignaciones =
            new List<ClsDatosDetalleViajeLoteProducto>();

        private List<ClsDatosDetalleViajeLoteProducto> _Filtrados =
            new List<ClsDatosDetalleViajeLoteProducto>();

        private bool _ModoEdicion = false;
        private bool _EsNuevo = false;
        private long _IdSeleccionado = 0;

        // Paginación.
        private int _PaginaActual = 1;
        private const int _TamanoPagina = 10;

        // Controles de consulta.
        private Label LogisticaLblBuscarLote;
        private TextBox LogisticaTxtBuscarLote;
        private Label LogisticaLblPaginacionLote;
        private Button LogisticaBtnAnteriorLote;
        private Button LogisticaBtnSiguienteLote;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public FrmDetalleViajeProducto()
        {
            InitializeComponent();

            LogisticaProcConfigurarControles();
            LogisticaProcCrearConsultaVisual();
            LogisticaProcVincularEventos();
            LogisticaProcBloquearFormulario();

            AutoScroll = true;
        }

        // =====================================================
        // CONFIGURACIÓN DE CONTROLES
        // =====================================================

        private void LogisticaProcConfigurarControles()
        {
            LogisticaCboDetalleViaje.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboLoteProducto.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaNudCantidadCajas.Minimum = 1;
            LogisticaNudCantidadCajas.Maximum = 1000000;
            LogisticaNudCantidadCajas.DecimalPlaces = 0;

            LogisticaNudPesoTotal.Minimum = 0;
            LogisticaNudPesoTotal.Maximum = 999999999;
            LogisticaNudPesoTotal.DecimalPlaces = 3;

            // Configurar DataGridView del diseñador.
            LogisticaDgvLotesAsignados.AutoGenerateColumns = false;
            LogisticaDgvLotesAsignados.ReadOnly = true;
            LogisticaDgvLotesAsignados.MultiSelect = false;

            LogisticaDgvLotesAsignados.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            LogisticaDgvLotesAsignados.AllowUserToAddRows = false;
            LogisticaDgvLotesAsignados.AllowUserToDeleteRows = false;
            LogisticaDgvLotesAsignados.RowHeadersVisible = false;

            LogisticaDgvLotesAsignados.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LogisticaDgvLotesAsignados.EnableHeadersVisualStyles = false;

            // Colores de Logística.
            LogisticaDgvLotesAsignados
                .ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(52, 143, 141);

            LogisticaDgvLotesAsignados
                .ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            LogisticaDgvLotesAsignados
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 99, 46);

            LogisticaDgvLotesAsignados
                .AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(251, 246, 242);

            // Vinculación de columnas.
            ColDetalleViaje.DataPropertyName =
                "NombreDetalleViaje";

            ColLoteProducto.DataPropertyName =
                "NombreLoteProducto";

            ColCantidadCajas.DataPropertyName =
                "CantidadCajasDetalleViajeLoteProducto";

            ColPesoTotal.DataPropertyName =
                "PesoKgDetalleViajeLoteProducto";

            ColPesoTotal.DefaultCellStyle.Format = "N3";
        }

        // =====================================================
        // CREAR BÚSQUEDA Y PAGINACIÓN
        // =====================================================

        private void LogisticaProcCrearConsultaVisual()
        {
            Color Verde = Color.FromArgb(52, 143, 141);
            Color Botones = Color.FromArgb(242, 216, 197);

            int X = LogisticaDgvLotesAsignados.Left;
            int Y = LogisticaDgvLotesAsignados.Top;

            LogisticaLblBuscarLote = new Label
            {
                Name = "LogisticaLblBuscarLote",
                Text = "Buscar:",
                Location = new Point(X, Y + 3),
                Size = new Size(75, 25),
                ForeColor = Verde,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f)
            };

            LogisticaTxtBuscarLote = new TextBox
            {
                Name = "LogisticaTxtBuscarLote",
                Location = new Point(X + 80, Y),
                Size = new Size(350, 28),
                Font = new Font("Segoe UI", 10f)
            };

            // Desplazar tabla para dejar espacio al buscador.
            LogisticaDgvLotesAsignados.Top = Y + 40;

            LogisticaBtnAnteriorLote = new Button
            {
                Name = "LogisticaBtnAnteriorLote",
                Text = "Anterior",
                Location = new Point(
                    X,
                    LogisticaDgvLotesAsignados.Bottom + 12),
                Size = new Size(95, 30),
                BackColor = Botones
            };

            LogisticaBtnSiguienteLote = new Button
            {
                Name = "LogisticaBtnSiguienteLote",
                Text = "Siguiente",
                Location = new Point(
                    X + 105,
                    LogisticaDgvLotesAsignados.Bottom + 12),
                Size = new Size(95, 30),
                BackColor = Botones
            };

            LogisticaLblPaginacionLote = new Label
            {
                Name = "LogisticaLblPaginacionLote",
                Location = new Point(
                    X + 215,
                    LogisticaDgvLotesAsignados.Bottom + 16),
                Size = new Size(400, 24),
                ForeColor = Verde,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f)
            };

            LogisticaPnlHistorialLotes.Controls.Add(
                LogisticaLblBuscarLote);

            LogisticaPnlHistorialLotes.Controls.Add(
                LogisticaTxtBuscarLote);

            LogisticaPnlHistorialLotes.Controls.Add(
                LogisticaBtnAnteriorLote);

            LogisticaPnlHistorialLotes.Controls.Add(
                LogisticaBtnSiguienteLote);

            LogisticaPnlHistorialLotes.Controls.Add(
                LogisticaLblPaginacionLote);

            LogisticaPnlHistorialLotes.Height = Math.Max(
                LogisticaPnlHistorialLotes.Height,
                LogisticaBtnAnteriorLote.Bottom + 15);

            // Eventos de búsqueda y paginación.
            LogisticaTxtBuscarLote.TextChanged +=
                LogisticaTxtBuscarLote_TextChanged;

            LogisticaBtnAnteriorLote.Click += (sender, e) =>
            {
                if (_PaginaActual > 1)
                {
                    _PaginaActual--;
                    LogisticaProcMostrarPagina();
                }
            };

            LogisticaBtnSiguienteLote.Click += (sender, e) =>
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
            Load -= FrmDetalleViajeProducto_Load;
            Load += FrmDetalleViajeProducto_Load;

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

            LogisticaDgvLotesAsignados.CellDoubleClick -=
                LogisticaDgvLotes_CellDoubleClick;

            LogisticaDgvLotesAsignados.CellDoubleClick +=
                LogisticaDgvLotes_CellDoubleClick;
        }

        // =====================================================
        // CARGA INICIAL
        // =====================================================

        private void FrmDetalleViajeProducto_Load(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcCargarCatalogos();
                LogisticaProcRecargarAsignaciones();
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
            LogisticaCboDetalleViaje.DataSource = null;
            LogisticaCboDetalleViaje.DisplayMember = "Nombre";
            LogisticaCboDetalleViaje.ValueMember = "Id";

            LogisticaCboDetalleViaje.DataSource =
                _Controlador.LogisticaFuncDetallesViaje();

            LogisticaCboDetalleViaje.SelectedIndex = -1;

            LogisticaCboLoteProducto.DataSource = null;
            LogisticaCboLoteProducto.DisplayMember = "Nombre";
            LogisticaCboLoteProducto.ValueMember = "Id";

            LogisticaCboLoteProducto.DataSource =
                _Controlador.LogisticaFuncLotesProducto();

            LogisticaCboLoteProducto.SelectedIndex = -1;
        }

        // =====================================================
        // CONSULTAR ASIGNACIONES
        // =====================================================

        private void LogisticaProcRecargarAsignaciones()
        {
            _Asignaciones =
                _Controlador.LogisticaFuncObtenerTodos();

            LogisticaProcFiltrarAsignaciones();
        }

        // =====================================================
        // BÚSQUEDA
        // =====================================================

        private void LogisticaProcFiltrarAsignaciones()
        {
            string Buscar = LogisticaTxtBuscarLote.Text.Trim();

            _Filtrados = _Asignaciones.Where(A =>
                A.IdDetalleViajeLoteProducto.ToString()
                    .Contains(Buscar) ||

                (A.NombreDetalleViaje ?? "").IndexOf(
                    Buscar,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (A.NombreLoteProducto ?? "").IndexOf(
                    Buscar,
                    StringComparison.OrdinalIgnoreCase) >= 0
            )
            .OrderByDescending(A => A.IdDetalleViajeLoteProducto)
            .ToList();

            _PaginaActual = 1;
            LogisticaProcMostrarPagina();
        }

        private void LogisticaTxtBuscarLote_TextChanged(
            object sender, EventArgs e)
        {
            LogisticaProcFiltrarAsignaciones();
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
            LogisticaDgvLotesAsignados.DataSource = null;

            LogisticaDgvLotesAsignados.DataSource =
                _Filtrados
                    .Skip((_PaginaActual - 1) * _TamanoPagina)
                    .Take(_TamanoPagina)
                    .ToList();

            LogisticaDgvLotesAsignados.ClearSelection();

            LogisticaLblPaginacionLote.Text = string.Format(
                "Página {0} de {1} | {2} asignaciones",
                _PaginaActual,
                LogisticaFuncTotalPaginas(),
                _Filtrados.Count);

            LogisticaBtnAnteriorLote.Enabled =
                _PaginaActual > 1;

            LogisticaBtnSiguienteLote.Enabled =
                _PaginaActual < LogisticaFuncTotalPaginas();
        }

        // =====================================================
        // OBTENER REGISTRO SELECCIONADO
        // =====================================================

        private ClsDatosDetalleViajeLoteProducto
            LogisticaFuncSeleccionado()
        {
            if (LogisticaDgvLotesAsignados.SelectedRows.Count == 0)
            {
                throw new Exception(
                    "Seleccione una asignación del historial.");
            }

            var Asignacion =
                LogisticaDgvLotesAsignados.SelectedRows[0]
                    .DataBoundItem
                    as ClsDatosDetalleViajeLoteProducto;

            if (Asignacion == null)
            {
                throw new Exception(
                    "No se pudo identificar la asignación.");
            }

            return Asignacion;
        }

        // =====================================================
        // CARGAR REGISTRO PARA MODIFICAR
        // =====================================================

        private void LogisticaProcCargarSeleccionado()
        {
            var Asignacion = LogisticaFuncSeleccionado();

            _IdSeleccionado =
                Asignacion.IdDetalleViajeLoteProducto;

            _EsNuevo = false;
            _ModoEdicion = true;

            LogisticaPnlDatosDetalle.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaCboDetalleViaje.SelectedValue =
                Asignacion.FkDetalleViajeCodigo;

            LogisticaCboLoteProducto.SelectedValue =
                Asignacion.FkLoteProductoCodigo;

            LogisticaNudCantidadCajas.Value = Math.Max(
                LogisticaNudCantidadCajas.Minimum,
                Math.Min(
                    LogisticaNudCantidadCajas.Maximum,
                    Asignacion.CantidadCajasDetalleViajeLoteProducto));

            LogisticaNudPesoTotal.Value = Math.Max(
                LogisticaNudPesoTotal.Minimum,
                Math.Min(
                    LogisticaNudPesoTotal.Maximum,
                    Asignacion.PesoKgDetalleViajeLoteProducto));
        }

        // =====================================================
        // BOTÓN NUEVO
        // =====================================================

        private void LogisticaBtnNuevo_Click(
            object sender, EventArgs e)
        {
            LogisticaProcBloquearFormulario();

            _ModoEdicion = true;
            _EsNuevo = true;
            _IdSeleccionado = 0;

            LogisticaPnlDatosDetalle.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaCboDetalleViaje.Focus();
        }

        // =====================================================
        // BOTÓN CONSULTAR
        // =====================================================

        private void LogisticaBtnConsultar_Click(
            object sender, EventArgs e)
        {
            try
            {
                LogisticaProcRecargarAsignaciones();
                LogisticaTxtBuscarLote.Focus();
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

        private void LogisticaDgvLotes_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                LogisticaDgvLotesAsignados.ClearSelection();

                LogisticaDgvLotesAsignados
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

        private ClsDatosDetalleViajeLoteProducto
            LogisticaFuncLeerFormulario()
        {
            if (LogisticaCboDetalleViaje.SelectedIndex < 0 ||
                LogisticaCboLoteProducto.SelectedIndex < 0)
            {
                throw new Exception(
                    "Seleccione un detalle de viaje y un lote.");
            }

            return new ClsDatosDetalleViajeLoteProducto
            {
                IdDetalleViajeLoteProducto =
                    _IdSeleccionado,

                FkDetalleViajeCodigo =
                    Convert.ToInt64(
                        LogisticaCboDetalleViaje.SelectedValue),

                FkLoteProductoCodigo =
                    Convert.ToInt64(
                        LogisticaCboLoteProducto.SelectedValue),

                CantidadCajasDetalleViajeLoteProducto =
                    Convert.ToInt32(
                        LogisticaNudCantidadCajas.Value),

                PesoKgDetalleViajeLoteProducto =
                    LogisticaNudPesoTotal.Value
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
                LogisticaProcRecargarAsignaciones();
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
                var Asignacion = LogisticaFuncSeleccionado();

                DialogResult Confirmacion = MessageBox.Show(
                    "¿Desea eliminar la asignación número " +
                    Asignacion.IdDetalleViajeLoteProducto + "?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (Confirmacion != DialogResult.Yes)
                    return;

                string Mensaje =
                    _Controlador.LogisticaFuncEliminar(
                        Asignacion.IdDetalleViajeLoteProducto);

                MessageBox.Show(
                    Mensaje,
                    "Logística",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();
                LogisticaProcRecargarAsignaciones();
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

                LogisticaTxtBuscarLote.Clear();

                LogisticaProcRecargarAsignaciones();
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

            LogisticaCboDetalleViaje.SelectedIndex = -1;
            LogisticaCboLoteProducto.SelectedIndex = -1;

            LogisticaNudCantidadCajas.Value =
                LogisticaNudCantidadCajas.Minimum;

            LogisticaNudPesoTotal.Value = 0;

            LogisticaPnlDatosDetalle.Enabled = false;
            LogisticaBtnGuardar.Enabled = false;
        }

        // =====================================================
        // BOTÓN IMPRIMIR
        // =====================================================

        private void LogisticaBtnImprimir_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "El reporte de lotes asignados está pendiente.",
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
                "El manual de ayuda está pendiente de integrar.",
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
