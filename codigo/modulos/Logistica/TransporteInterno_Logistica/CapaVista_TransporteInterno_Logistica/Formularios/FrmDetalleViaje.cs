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
    public partial class FrmDetalleViaje : Form
    {

        private readonly ClsControladorDetalleViaje _Controlador =
            new ClsControladorDetalleViaje();

        private List<ClsDatosDetalleViaje> _Detalles =
            new List<ClsDatosDetalleViaje>();

        private List<ClsDatosDetalleViaje> _Filtrados =
            new List<ClsDatosDetalleViaje>();

        private bool _ModoEdicion = false;
        private bool _EsNuevo = false;
        private long _IdSeleccionado = 0;

        private int _PaginaActual = 1;
        private const int _TamanoPagina = 10;

        private TextBox LogisticaTxtBuscarDetalle;
        private Label LogisticaLblPaginacionDetalle;
        private Button LogisticaBtnAnteriorDetalle;
        private Button LogisticaBtnSiguienteDetalle;

        public FrmDetalleViaje()
        {
            InitializeComponent();

            LogisticaProcConfigurarControles();
            LogisticaProcCrearConsultaVisual();
            LogisticaProcVincularEventos();
            LogisticaProcBloquearFormulario();
        }

        // =====================================================
        // CONFIGURAR CONTROLES
        // =====================================================

        private void LogisticaProcConfigurarControles()
        {
            LogisticaTxtCodigoDetalle.ReadOnly = true;

            LogisticaCboViaje.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaNudOrdenDetalle.Minimum = 1;
            LogisticaNudOrdenDetalle.Maximum = 1000000;

            LogisticaNudCantidad.DecimalPlaces = 3;
            LogisticaNudCantidad.Minimum = 0;
            LogisticaNudCantidad.Maximum = 999999999;

            LogisticaNudPeso.DecimalPlaces = 3;
            LogisticaNudPeso.Minimum = 0;
            LogisticaNudPeso.Maximum = 999999999;

            LogisticaTxtDescripcionDetalle.MaxLength = 255;

            LogisticaDgvDetalleViaje.AutoGenerateColumns = false;
            LogisticaDgvDetalleViaje.ReadOnly = true;
            LogisticaDgvDetalleViaje.MultiSelect = false;

            LogisticaDgvDetalleViaje.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            LogisticaDgvDetalleViaje.AllowUserToAddRows = false;
            LogisticaDgvDetalleViaje.AllowUserToDeleteRows = false;
            LogisticaDgvDetalleViaje.RowHeadersVisible = false;

            LogisticaDgvDetalleViaje.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LogisticaDgvDetalleViaje.EnableHeadersVisualStyles = false;

            LogisticaDgvDetalleViaje
                .ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(52, 143, 141);

            LogisticaDgvDetalleViaje
                .ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            ColCodigoDetalle.DataPropertyName = "IdDetalleViaje";
            ColViaje.DataPropertyName = "DescripcionViaje";
            ColOrdenDetalle.DataPropertyName = "OrdenDetalleViaje";
            ColDescripcion.DataPropertyName = "DescripcionDetalleViaje";
            ColCantidad.DataPropertyName = "CantidadDetalleViaje";
            ColPeso.DataPropertyName = "PesoDetalleViaje";

            ColCantidad.DefaultCellStyle.Format = "N3";
            ColPeso.DefaultCellStyle.Format = "N3";
        }

        // =====================================================
        // BÚSQUEDA Y PAGINACIÓN
        // =====================================================

        private void LogisticaProcCrearConsultaVisual()
        {
            int X = LogisticaDgvDetalleViaje.Left;
            int Y = LogisticaDgvDetalleViaje.Top;

            LogisticaTxtBuscarDetalle = new TextBox
            {
                Name = "LogisticaTxtBuscarDetalle",
                Location = new Point(X + 80, Y),
                Size = new Size(300, 28)
            };

            Label EtiquetaBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(X, Y + 3),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 143, 141)
            };

            LogisticaDgvDetalleViaje.Top = Y + 40;

            LogisticaBtnAnteriorDetalle = new Button
            {
                Text = "Anterior",
                Location = new Point(
                    X, LogisticaDgvDetalleViaje.Bottom + 10),
                Size = new Size(95, 30),
                BackColor = Color.FromArgb(242, 216, 197)
            };

            LogisticaBtnSiguienteDetalle = new Button
            {
                Text = "Siguiente",
                Location = new Point(
                    X + 105, LogisticaDgvDetalleViaje.Bottom + 10),
                Size = new Size(95, 30),
                BackColor = Color.FromArgb(242, 216, 197)
            };

            LogisticaLblPaginacionDetalle = new Label
            {
                Location = new Point(
                    X + 215, LogisticaDgvDetalleViaje.Bottom + 15),
                Size = new Size(380, 25),
                ForeColor = Color.FromArgb(52, 143, 141)
            };

            LogisticaPnlHistorialDetalle.Controls.Add(EtiquetaBuscar);
            LogisticaPnlHistorialDetalle.Controls.Add(LogisticaTxtBuscarDetalle);
            LogisticaPnlHistorialDetalle.Controls.Add(LogisticaBtnAnteriorDetalle);
            LogisticaPnlHistorialDetalle.Controls.Add(LogisticaBtnSiguienteDetalle);
            LogisticaPnlHistorialDetalle.Controls.Add(LogisticaLblPaginacionDetalle);

            LogisticaPnlHistorialDetalle.Height = Math.Max(
                LogisticaPnlHistorialDetalle.Height,
                LogisticaBtnAnteriorDetalle.Bottom + 15);

            AutoScroll = true;

            LogisticaTxtBuscarDetalle.TextChanged += (s, e) =>
                LogisticaProcFiltrar();

            LogisticaBtnAnteriorDetalle.Click += (s, e) =>
            {
                if (_PaginaActual > 1)
                {
                    _PaginaActual--;
                    LogisticaProcMostrarPagina();
                }
            };

            LogisticaBtnSiguienteDetalle.Click += (s, e) =>
            {
                if (_PaginaActual < LogisticaFuncTotalPaginas())
                {
                    _PaginaActual++;
                    LogisticaProcMostrarPagina();
                }
            };
        }

        // =====================================================
        // EVENTOS
        // =====================================================

        private void LogisticaProcVincularEventos()
        {
            Load += FrmDetalleViaje_Load;

            LogisticaBtnNuevo.Click += LogisticaBtnNuevo_Click;
            LogisticaBtnConsultar.Click += LogisticaBtnConsultar_Click;
            LogisticaBtnModificar.Click += LogisticaBtnModificar_Click;
            LogisticaBtnGuardar.Click += LogisticaBtnGuardar_Click;
            LogisticaBtnEliminar.Click += LogisticaBtnEliminar_Click;
            LogisticaBtnCancelar.Click += LogisticaBtnCancelar_Click;
            LogisticaBtnRefrescar.Click += LogisticaBtnRefrescar_Click;
            LogisticaBtnImprimir.Click += LogisticaBtnImprimir_Click;
            LogisticaBtnAyuda.Click += LogisticaBtnAyuda_Click;

            LogisticaDgvDetalleViaje.CellDoubleClick +=
                LogisticaDgvDetalleViaje_CellDoubleClick;
        }

        private void FrmDetalleViaje_Load(object sender, EventArgs e)
        {
            try
            {
                LogisticaProcCargarViajes();
                LogisticaProcRecargarDetalles();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        // =====================================================
        // CATÁLOGOS Y CONSULTAS
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

        private void LogisticaProcRecargarDetalles()
        {
            _Detalles = _Controlador.LogisticaFuncObtenerTodos();
            LogisticaProcFiltrar();
        }

        private void LogisticaProcFiltrar()
        {
            string Buscar = LogisticaTxtBuscarDetalle.Text.Trim();

            _Filtrados = _Detalles.Where(D =>
                D.IdDetalleViaje.ToString().Contains(Buscar) ||
                D.FkEncabezadoViajeCodigo.ToString().Contains(Buscar) ||
                (D.DescripcionDetalleViaje ?? "").IndexOf(
                    Buscar, StringComparison.OrdinalIgnoreCase) >= 0
            ).OrderByDescending(D => D.IdDetalleViaje).ToList();

            _PaginaActual = 1;
            LogisticaProcMostrarPagina();
        }

        private int LogisticaFuncTotalPaginas()
        {
            return Math.Max(1,
                (_Filtrados.Count + _TamanoPagina - 1) / _TamanoPagina);
        }

        private void LogisticaProcMostrarPagina()
        {
            LogisticaDgvDetalleViaje.DataSource = null;

            LogisticaDgvDetalleViaje.DataSource =
                _Filtrados.Skip(
                    (_PaginaActual - 1) * _TamanoPagina)
                    .Take(_TamanoPagina)
                    .ToList();

            LogisticaDgvDetalleViaje.ClearSelection();

            LogisticaLblPaginacionDetalle.Text = string.Format(
                "Página {0} de {1} | {2} detalles",
                _PaginaActual,
                LogisticaFuncTotalPaginas(),
                _Filtrados.Count);

            LogisticaBtnAnteriorDetalle.Enabled = _PaginaActual > 1;
            LogisticaBtnSiguienteDetalle.Enabled =
                _PaginaActual < LogisticaFuncTotalPaginas();
        }

        // =====================================================
        // SELECCIÓN DE DETALLES
        // =====================================================

        private ClsDatosDetalleViaje LogisticaFuncSeleccionado()
        {
            if (LogisticaDgvDetalleViaje.SelectedRows.Count == 0)
                throw new Exception("Seleccione un detalle del historial.");

            var Detalle =
                LogisticaDgvDetalleViaje.SelectedRows[0]
                    .DataBoundItem as ClsDatosDetalleViaje;

            if (Detalle == null)
                throw new Exception("No se pudo identificar el detalle.");

            return Detalle;
        }

        private void LogisticaProcCargarSeleccionado()
        {
            var Detalle = LogisticaFuncSeleccionado();

            _ModoEdicion = true;
            _EsNuevo = false;
            _IdSeleccionado = Detalle.IdDetalleViaje;

            LogisticaPnlDatosDetalle.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaTxtCodigoDetalle.Text =
                Detalle.IdDetalleViaje.ToString();

            LogisticaCboViaje.SelectedValue =
                Convert.ToInt32(Detalle.FkEncabezadoViajeCodigo);

            LogisticaNudOrdenDetalle.Value = Detalle.OrdenDetalleViaje;
            LogisticaTxtDescripcionDetalle.Text =
                Detalle.DescripcionDetalleViaje;

            LogisticaNudCantidad.Value = Math.Min(
                LogisticaNudCantidad.Maximum,
                Math.Max(LogisticaNudCantidad.Minimum,
                    Detalle.CantidadDetalleViaje));

            LogisticaNudPeso.Value = Math.Min(
                LogisticaNudPeso.Maximum,
                Math.Max(LogisticaNudPeso.Minimum,
                    Detalle.PesoDetalleViaje));
        }

        // =====================================================
        // BOTONES DEL FORMULARIO
        // =====================================================

        private void LogisticaBtnNuevo_Click(object sender, EventArgs e)
        {
            LogisticaProcBloquearFormulario();

            _ModoEdicion = true;
            _EsNuevo = true;

            LogisticaPnlDatosDetalle.Enabled = true;
            LogisticaBtnGuardar.Enabled = true;

            LogisticaNudOrdenDetalle.Value = 1;
            LogisticaCboViaje.Focus();
        }

        private void LogisticaBtnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                LogisticaProcRecargarDetalles();
                LogisticaTxtBuscarDetalle.Focus();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        private void LogisticaBtnModificar_Click(object sender, EventArgs e)
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

        private void LogisticaDgvDetalleViaje_CellDoubleClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                LogisticaDgvDetalleViaje.ClearSelection();
                LogisticaDgvDetalleViaje.Rows[e.RowIndex].Selected = true;
                LogisticaProcCargarSeleccionado();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        private ClsDatosDetalleViaje LogisticaFuncLeerFormulario()
        {
            if (LogisticaCboViaje.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(
                    LogisticaTxtDescripcionDetalle.Text))
            {
                throw new Exception(
                    "Complete todos los campos obligatorios.");
            }

            return new ClsDatosDetalleViaje
            {
                IdDetalleViaje = _IdSeleccionado,

                FkEncabezadoViajeCodigo =
                    Convert.ToInt64(LogisticaCboViaje.SelectedValue),

                OrdenDetalleViaje =
                    Convert.ToInt32(LogisticaNudOrdenDetalle.Value),

                DescripcionDetalleViaje =
                    LogisticaTxtDescripcionDetalle.Text.Trim(),

                CantidadDetalleViaje = LogisticaNudCantidad.Value,
                PesoDetalleViaje = LogisticaNudPeso.Value
            };
        }

        private void LogisticaBtnGuardar_Click(object sender, EventArgs e)
        {
            if (!_ModoEdicion)
                return;

            try
            {
                var Datos = LogisticaFuncLeerFormulario();

                string Mensaje = _EsNuevo
                    ? _Controlador.LogisticaFuncAgregar(Datos)
                    : _Controlador.LogisticaFuncEditar(Datos);

                MessageBox.Show(
                    Mensaje, "Logística",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();
                LogisticaProcRecargarDetalles();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        private void LogisticaBtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var Detalle = LogisticaFuncSeleccionado();

                DialogResult Confirmacion = MessageBox.Show(
                    "¿Desea eliminar el detalle número " +
                    Detalle.IdDetalleViaje + "?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (Confirmacion != DialogResult.Yes)
                    return;

                string Mensaje = _Controlador.LogisticaFuncEliminar(
                    Detalle.IdDetalleViaje);

                MessageBox.Show(
                    Mensaje, "Logística",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();
                LogisticaProcRecargarDetalles();
            }
            catch (Exception ex)
            {
                LogisticaProcMostrarError(ex.Message);
            }
        }

        private void LogisticaBtnCancelar_Click(object sender, EventArgs e)
        {
            LogisticaProcBloquearFormulario();
        }

        private void LogisticaBtnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                LogisticaProcBloquearFormulario();
                LogisticaProcCargarViajes();
                LogisticaTxtBuscarDetalle.Clear();
                LogisticaProcRecargarDetalles();
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

            LogisticaTxtCodigoDetalle.Clear();
            LogisticaCboViaje.SelectedIndex = -1;

            LogisticaNudOrdenDetalle.Value = 1;
            LogisticaTxtDescripcionDetalle.Clear();

            LogisticaNudCantidad.Value = 0;
            LogisticaNudPeso.Value = 0;

            LogisticaPnlDatosDetalle.Enabled = false;
            LogisticaBtnGuardar.Enabled = false;
        }

        // =====================================================
        // OBTENER DATOS PARA EL REPORTE DE DETALLE DE VIAJE
        // =====================================================

        private DataTable LogisticaMetObtenerDatosReporte()
        {
            DataTable Tabla = new DataTable();

            var Controlador =
                new ClsControladorDetalleViaje();

            var Registros =
                Controlador.LogisticaFuncObtenerTodos();

            // Crear columnas utilizando los nombres
            // reales de las propiedades del controlador.
            PropertyInfo[] Propiedades =
                typeof(ClsDatosDetalleViaje).GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance);

            foreach (PropertyInfo Propiedad in Propiedades)
            {
                if (!Propiedad.CanRead)
                {
                    continue;
                }

                Type TipoColumna =
                    Nullable.GetUnderlyingType(
                        Propiedad.PropertyType)
                    ?? Propiedad.PropertyType;

                Tabla.Columns.Add(
                    Propiedad.Name,
                    TipoColumna);
            }

            // Agregar todos los registros.
            foreach (var Detalle in Registros)
            {
                DataRow Fila = Tabla.NewRow();

                foreach (PropertyInfo Propiedad in Propiedades)
                {
                    if (!Propiedad.CanRead)
                    {
                        continue;
                    }

                    object Valor =
                        Propiedad.GetValue(Detalle, null);

                    Fila[Propiedad.Name] =
                        Valor ?? DBNull.Value;
                }

                Tabla.Rows.Add(Fila);
            }

            return Tabla;
        }

        private void LogisticaBtnImprimir_Click(object sender, EventArgs e)
        {
           
        }

        private void LogisticaBtnAyuda_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "El manual de ayuda está pendiente de integrar.",
                "Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

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
                DataTable TablaDatos =
                    LogisticaMetObtenerDatosReporte();

                using (FrmReporteDetalleViaje Formulario =
                    new FrmReporteDetalleViaje(TablaDatos))
                {
                    Formulario.ShowDialog(this);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(
                    "No se pudo generar el reporte:\n" + Ex.Message,
                    "Reporte de Detalle de Viaje",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
