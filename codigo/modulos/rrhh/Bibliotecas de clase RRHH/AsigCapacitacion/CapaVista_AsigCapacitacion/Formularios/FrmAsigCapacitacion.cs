using CapaControlador_AsigCapacitacion.Modelos_de_Controladores;
using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_AsigCapacitacion.FrmReporte;
using CapaVista_Seguridad.Ayudas;
using CapaVista_Seguridad.frmReportes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : RRHH
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 31/10/2026
 * ==================================================================
 * Propósito :
 *  El formulario del módulo de Asignacion Capacitacion nos 
 *  permite registrar, consultar, modificar y eliminar los 
 *  datos ingresados al sistema, tambien cuenta con Reportes y Ayudas.
 * ===================================================================
*/


namespace CapaVista_AsigCapacitacion.Formularios
{
    public partial class FrmAsigCapacitacion : Form
    {
        private ClsModeloAsigCapacitacion _Asignacion = new ClsModeloAsigCapacitacion();
        private int _IdAsignacionSeleccionada = 0;
        private ClsPermisoAplicacion _MisPermisos;
        private FrmReporteAsigCapacitacion reporteAsigCapacitacion;

        private const int ID_MODULO = 9000;
        private const int ID_APLICACION = 9305;

        public FrmAsigCapacitacion()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmAsigCapacitacion_Load);
        }
       
        private void FrmAsigCapacitacion_Load(object sender, EventArgs e)
        {
            var MapaBotones = new Dictionary<Control, TipoPermiso>
    {
        { RrhhBtnGuardarAsigCap,   TipoPermiso.Insertar },
        { RrhhBtnModificarAsigCap, TipoPermiso.Editar },
        { RrhhBtnEliminarAsigCap,  TipoPermiso.Eliminar },
        { RrhhBtnReporteAsigCap,  TipoPermiso.Imprimir }
    };

            _MisPermisos = ClsSeguridadFormHelper.SeguridadMetInicializarSeguridad(
                this, ID_MODULO, ID_APLICACION, MapaBotones);

            if (!_MisPermisos.TieneAcceso)
                return;

            CargarCombos();
            RrhhMetListarAsignaciones();
            RrhhMetReinicio();

        }

        private void CargarCombos()
        {
            RrhhCboEmpleado.DataSource = _Asignacion.RrhhMetObtenerEmpleados();
            RrhhCboEmpleado.DisplayMember = "nombreEmpleado";
            RrhhCboEmpleado.ValueMember = "idEmpleado";
            RrhhCboEmpleado.SelectedIndex = -1;

            RrhhCboCapacitacion.DataSource = _Asignacion.RrhhMetObtenerCapacitaciones();
            RrhhCboCapacitacion.DisplayMember = "nombreCapacitacion";
            RrhhCboCapacitacion.ValueMember = "idCapacitacion";
            RrhhCboCapacitacion.SelectedIndex = -1;
        }

    
        private void RrhhMetListarAsignaciones()
        {
            try
            {
                RrhhDgvAsigCapacitacion.DataSource = _Asignacion.RrhhMetObtenerTodos();
                RrhhMetConfigurarEncabezadosGrid();
                RrhhDgvAsigCapacitacion.ClearSelection();
                RrhhDgvAsigCapacitacion.CurrentCell = null;
                _IdAsignacionSeleccionada = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RrhhMetConfigurarEncabezadosGrid()
        {
            
            RrhhDgvAsigCapacitacion.ColumnHeadersDefaultCellStyle.Font = new Font(RrhhDgvAsigCapacitacion.Font, FontStyle.Bold);
            RrhhDgvAsigCapacitacion.ReadOnly = true;
            RrhhDgvAsigCapacitacion.AllowUserToAddRows = false;
            RrhhDgvAsigCapacitacion.AllowUserToDeleteRows = false;
            RrhhDgvAsigCapacitacion.MultiSelect = false;
            RrhhDgvAsigCapacitacion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RrhhDgvAsigCapacitacion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (RrhhDgvAsigCapacitacion.Columns["IdAsignacionCapacitacion"] != null)
                RrhhDgvAsigCapacitacion.Columns["IdAsignacionCapacitacion"].Visible = false;

            if (RrhhDgvAsigCapacitacion.Columns["IdEmpleado"] != null)
                RrhhDgvAsigCapacitacion.Columns["IdEmpleado"].Visible = false;

            if (RrhhDgvAsigCapacitacion.Columns["IdCapacitacion"] != null)
                RrhhDgvAsigCapacitacion.Columns["IdCapacitacion"].Visible = false;

            if (RrhhDgvAsigCapacitacion.Columns["NombreEmpleado"] != null)
                RrhhDgvAsigCapacitacion.Columns["NombreEmpleado"].HeaderText = "Empleado";

            if (RrhhDgvAsigCapacitacion.Columns["NombreCapacitacion"] != null)
                RrhhDgvAsigCapacitacion.Columns["NombreCapacitacion"].HeaderText = "Capacitación";

            if (RrhhDgvAsigCapacitacion.Columns["NombreInstructor"] != null)
                RrhhDgvAsigCapacitacion.Columns["NombreInstructor"].HeaderText = "Instructor";

            if (RrhhDgvAsigCapacitacion.Columns["FechaAsignacionCapacitacion"] != null)
            {
                RrhhDgvAsigCapacitacion.Columns["FechaAsignacionCapacitacion"].HeaderText = "Fecha de Asignación";
                RrhhDgvAsigCapacitacion.Columns["FechaAsignacionCapacitacion"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }

        private void RrhhDgvAsignacion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                RrhhMetCargarFilaEnFormulario(e.RowIndex);
            }
        }

        private void RrhhMetCargarFilaEnFormulario(int rowIndex)
        {
            var fila = RrhhDgvAsigCapacitacion.Rows[rowIndex];

            _IdAsignacionSeleccionada = Convert.ToInt32(fila.Cells["IdAsignacionCapacitacion"].Value);
            RrhhCboEmpleado.SelectedValue = Convert.ToInt32(fila.Cells["IdEmpleado"].Value);
            RrhhCboCapacitacion.SelectedValue = Convert.ToInt32(fila.Cells["IdCapacitacion"].Value);
            RrhhDtpFechaAsig.Value = Convert.ToDateTime(fila.Cells["FechaAsignacionCapacitacion"].Value);
        }

        private void RrhhMetReinicio()
        {
            _IdAsignacionSeleccionada = 0;
            RrhhCboEmpleado.SelectedIndex = -1;
            RrhhCboCapacitacion.SelectedIndex = -1;
            RrhhDtpFechaAsig.Value = DateTime.Today;
        }

        private bool ValidarCampos()
        {
            if (RrhhCboEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un Empleado");
                return false;
            }

            if (RrhhCboCapacitacion.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar una Capacitación");
                return false;
            }

            return true;
        }

        private void RrhhMetAsignarDatosAlControlador()
        {
            _Asignacion.IdAsignacionCapacitacion = _IdAsignacionSeleccionada;
            _Asignacion.IdEmpleado = Convert.ToInt32(RrhhCboEmpleado.SelectedValue);
            _Asignacion.IdCapacitacion = Convert.ToInt32(RrhhCboCapacitacion.SelectedValue);
            _Asignacion.FechaAsignacionCapacitacion = RrhhDtpFechaAsig.Value.Date;
        }

        private void RrhhBtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) return;

                _IdAsignacionSeleccionada = 0;
                RrhhMetAsignarDatosAlControlador();
                _Asignacion.Estado = EstadoEntidad.Added;

                string Resultado = _Asignacion.RrhhMetGrabarCambios();
                MessageBox.Show(Resultado);

                if (Resultado.Contains("exitosa"))
                {
                    RrhhMetListarAsignaciones();
                    RrhhMetReinicio();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RrhhBtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_IdAsignacionSeleccionada == 0)
                {
                    MessageBox.Show("Seleccione una fila del listado para modificar");
                    return;
                }

                if (!ValidarCampos()) return;

                RrhhMetAsignarDatosAlControlador();
                _Asignacion.Estado = EstadoEntidad.Modified;

                string Resultado = _Asignacion.RrhhMetGrabarCambios();
                MessageBox.Show(Resultado);

                if (Resultado.Contains("exitosa"))
                {
                    RrhhMetListarAsignaciones();
                    RrhhMetReinicio();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RrhhBtnEliminar_Click(object sender, EventArgs e)
        {
            if (_IdAsignacionSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una fila");
                return;
            }

            var Respuesta = MessageBox.Show("¿Desea eliminar la asignación seleccionada?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Respuesta != DialogResult.Yes) return;

            _Asignacion.IdAsignacionCapacitacion = _IdAsignacionSeleccionada;
            _Asignacion.Estado = EstadoEntidad.Deleted;

            string Resultado = _Asignacion.RrhhMetGrabarCambios();
            MessageBox.Show(Resultado);

            if (Resultado.Contains("exitosa"))
            {
                RrhhMetListarAsignaciones();
                RrhhMetReinicio();
            }
        }

        private void RrhhBtnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                int IdEmp = RrhhCboEmpleado.SelectedValue == null ? 0 : Convert.ToInt32(RrhhCboEmpleado.SelectedValue);
                int IdCap = RrhhCboCapacitacion.SelectedValue == null ? 0 : Convert.ToInt32(RrhhCboCapacitacion.SelectedValue);

                if (IdEmp == 0 && IdCap == 0)
                {
                    RrhhMetListarAsignaciones();
                    return;
                }

                RrhhDgvAsigCapacitacion.DataSource = _Asignacion.RrhhMetBuscarPorFiltro(IdEmp, IdCap);
                RrhhMetConfigurarEncabezadosGrid();
                RrhhDgvAsigCapacitacion.ClearSelection();
                RrhhDgvAsigCapacitacion.CurrentCell = null;
                _IdAsignacionSeleccionada = 0;

                if (RrhhDgvAsigCapacitacion.Rows.Count == 0)
                    MessageBox.Show("No se encontraron asignaciones con ese criterio");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void RrhhMetIrAFila(int Indice)
        {
            RrhhDgvAsigCapacitacion.ClearSelection();
            RrhhDgvAsigCapacitacion.Rows[Indice].Selected = true;
            RrhhDgvAsigCapacitacion.CurrentCell = RrhhDgvAsigCapacitacion.Rows[Indice].Cells["NombreEmpleado"];
            RrhhMetCargarFilaEnFormulario(Indice);
        }

        private void RrhhBtnInicio_Click(object sender, EventArgs e)
        {
            if (RrhhDgvAsigCapacitacion.Rows.Count > 0)
            {
                RrhhMetIrAFila(0);
            }
        }

        private void RrhhBtnAnterior_Click(object sender, EventArgs e)
        {
            if (RrhhDgvAsigCapacitacion.Rows.Count > 0 && RrhhDgvAsigCapacitacion.CurrentCell != null)
            {
                int FilaActual = RrhhDgvAsigCapacitacion.CurrentCell.RowIndex;
                if (FilaActual > 0)
                {
                    RrhhMetIrAFila(FilaActual - 1);
                }
            }
        }

        private void RrhhBtnSiguiente_Click(object sender, EventArgs e)
        {
            if (RrhhDgvAsigCapacitacion.Rows.Count > 0 && RrhhDgvAsigCapacitacion.CurrentCell != null)
            {
                int FilaActual = RrhhDgvAsigCapacitacion.CurrentCell.RowIndex;
                if (FilaActual < RrhhDgvAsigCapacitacion.Rows.Count - 1)
                {
                    RrhhMetIrAFila(FilaActual + 1);
                }
            }
        }

        private void RrhhBtnFin_Click(object sender, EventArgs e)
        {
            if (RrhhDgvAsigCapacitacion.Rows.Count > 0)
            {
                RrhhMetIrAFila(RrhhDgvAsigCapacitacion.Rows.Count - 1);
            }
        }

        private void RrhhBtnLimpiar_Click(object sender, EventArgs e)
        {
            RrhhMetLimpiarCampos();
        }

        private void RrhhBtnAyuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "C:/RrhhAyudas/RecursosHumanosAyudas.chm", "AsigCapacitacion_RecursosHumanos.html");
        }
        private void RrhhMetLimpiarCampos()
        {
            RrhhMetReinicio();
            RrhhMetListarAsignaciones();
            RrhhCboEmpleado.Focus();
        }
        private void RrhhBtnReporte_Click(object sender, EventArgs e)
        {
            if (reporteAsigCapacitacion == null ||
               reporteAsigCapacitacion.IsDisposed)
            {
                reporteAsigCapacitacion =
                    new FrmReporteAsigCapacitacion();

                reporteAsigCapacitacion.FormClosed +=
                    (s, args) => reporteAsigCapacitacion = null;

                reporteAsigCapacitacion.Show();
            }
            else
            {
                reporteAsigCapacitacion.BringToFront();
                reporteAsigCapacitacion.Activate();
            }
        }
    }
}