using CapaControlador_EvaluacionDesempeño.Controladores;
using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_EvaluacionDesempeño.Utilidades;
using CapaVista_Seguridad.Ayudas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)
 * ==================================================================
 * Propósito :
 *  Ventana 9403 - Plan de Mejora.
 *  Registra, modifica y elimina planes de mejora y los asigna a la
 *  evaluación de desempeño de un empleado. Es la única ventana donde
 *  se asigna un plan a una evaluación.
 * Reglas especificas:
 *  Nombre y Descripción son obligatorios. Empleado y Evaluación son
 *  opcionales, pero si se elige un empleado hay que elegir cuál de
 *  sus evaluaciones recibe el plan. Una evaluación tiene como máximo
 *  un plan: solo se listan las que no tienen plan o tienen este.
 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    public partial class FrmPlanMejora : Form
    {
        private const int ID_MODULO = 9000;
        private const int ID_APLICACION = 9403;

        private readonly ClsModeloPlanMejora _PlanMejora = new ClsModeloPlanMejora();
        private ClsPermisoAplicacion _MisPermisos;

        // Plan seleccionado en la tabla (0 = ninguno).
        private int _IdPlanSeleccionado;

        // null = solo consulta; Added o Modified mientras se edita.
        private EstadoEntidad? _Edicion;

        // Último empleado ya aplicado (filtro o carga de evaluaciones), para no repetir la acción.
        private int _IdEmpleadoAplicado;

        public FrmPlanMejora()
        {
            InitializeComponent();

            RrhhColId.DataPropertyName = "IdPlanMejora";
            RrhhColNombre.DataPropertyName = "NombrePlanMejora";
            RrhhColEmpleado.DataPropertyName = "NombreEmpleado";
            RrhhColCiclo.DataPropertyName = "FechaEvaluacion";
            RrhhDgvPlanes.AutoGenerateColumns = false;

            Load += FrmPlanMejora_Load;
            RrhhDgvPlanes.CellClick += RrhhDgvPlanes_CellClick;
            RrhhCboEmpleado.SelectionChangeCommitted += (Remitente, Evento) => RrhhMetAplicarEmpleado();
            RrhhCboEmpleado.Leave += (Remitente, Evento) => RrhhMetAplicarEmpleado();
            RrhhCboEmpleado.KeyDown += RrhhCboEmpleado_KeyDown;
            RrhhTxtBuscar.KeyDown += RrhhTxtBuscar_KeyDown;
            RrhhBtnBuscar.Click += RrhhBtnBuscar_Click;
            RrhhBtnNuevo.Click += RrhhBtnNuevo_Click;
            RrhhBtnGuardar.Click += RrhhBtnGuardar_Click;
            RrhhBtnModificar.Click += RrhhBtnModificar_Click;
            RrhhBtnEliminar.Click += RrhhBtnEliminar_Click;
            RrhhBtnCancelar.Click += RrhhBtnCancelar_Click;
            RrhhBtnRefrescar.Click += RrhhBtnRefrescar_Click;
            RrhhBtnImprimir.Click += (Remitente, Evento) => ClsAccionesVista.RrhhMetImprimir(this);
            RrhhBtnInicio.Click += (Remitente, Evento) => RrhhMetIrAFila(0);
            RrhhBtnAnterior.Click += (Remitente, Evento) => RrhhMetIrAFila(RrhhMetFilaActual() - 1);
            RrhhBtnSiguiente.Click += (Remitente, Evento) => RrhhMetIrAFila(RrhhMetFilaActual() + 1);
            RrhhBtnFin.Click += (Remitente, Evento) => RrhhMetIrAFila(RrhhDgvPlanes.Rows.Count - 1);
            RrhhBtnAyuda.Click += (Remitente, Evento) => ClsAccionesVista.RrhhMetMostrarAyuda(this);
        }

        private void FrmPlanMejora_Load(object sender, EventArgs e)
        {
            var MapaBotones = new Dictionary<Control, TipoPermiso>
            {
                { RrhhBtnNuevo, TipoPermiso.Insertar },
                { RrhhBtnModificar, TipoPermiso.Editar },
                { RrhhBtnEliminar, TipoPermiso.Eliminar },
                { RrhhBtnImprimir, TipoPermiso.Imprimir }
            };

            _MisPermisos = ClsSeguridadFormHelper.SeguridadMetInicializarSeguridad(
                this, ID_MODULO, ID_APLICACION, MapaBotones);

            if (!_MisPermisos.TieneAcceso)
            {
                MessageBox.Show("No tiene acceso a Plan de Mejora.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            try
            {
                RrhhMetCargarEmpleados();
                RrhhMetListarPlanes(null);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudieron cargar los planes de mejora: " + Ex.Message, "Plan de Mejora",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            RrhhMetModoConsulta();
        }

        // ------------------------------------------------------------ Carga de datos

        private void RrhhMetCargarEmpleados()
        {
            DataTable Empleados = _PlanMejora.RrhhMetObtenerEmpleados();
            DataRow SinEmpleado = Empleados.NewRow();
            SinEmpleado["idEmpleado"] = 0;
            SinEmpleado["nombreEmpleado"] = "(Sin empleado)";
            Empleados.Rows.InsertAt(SinEmpleado, 0);

            RrhhCboEmpleado.DataSource = Empleados;
            RrhhCboEmpleado.DisplayMember = "nombreEmpleado";
            RrhhCboEmpleado.ValueMember = "idEmpleado";
            RrhhCboEmpleado.SelectedIndex = 0;
            RrhhMetCargarEvaluaciones();
        }

        private void RrhhMetCargarEvaluaciones()
        {
            int IdEmpleado = RrhhMetIdEmpleadoSeleccionado() ?? 0;
            _IdEmpleadoAplicado = IdEmpleado;
            int IdPlan = _Edicion == EstadoEntidad.Added ? 0 : _IdPlanSeleccionado;

            RrhhCboEvaluacion.DataSource = IdEmpleado == 0 ? null : _PlanMejora.RrhhMetObtenerEvaluaciones(IdEmpleado, IdPlan);
            RrhhCboEvaluacion.DisplayMember = "descripcionEvaluacion";
            RrhhCboEvaluacion.ValueMember = "idEvaluacionDesempeno";
            RrhhCboEvaluacion.SelectedIndex = RrhhCboEvaluacion.Items.Count == 1 ? 0 : -1;
        }

        private void RrhhMetListarPlanes(string Filtro)
        {
            RrhhDgvPlanes.DataSource = _PlanMejora.RrhhMetBuscar(Filtro);
            RrhhDgvPlanes.ClearSelection();
            RrhhDgvPlanes.CurrentCell = null;
            _IdPlanSeleccionado = 0;
            RrhhMetLimpiarCampos();
        }

        private void RrhhMetCargarFilaEnFormulario(int Indice)
        {
            var Plan = (ClsModeloPlanMejora)RrhhDgvPlanes.Rows[Indice].DataBoundItem;

            _IdPlanSeleccionado = Plan.IdPlanMejora;

            RrhhTxtNombrePlan.Text = Plan.NombrePlanMejora;
            RrhhTxtDescripcion.Text = Plan.DescripcionPlanMejora;
            RrhhCboEmpleado.SelectedValue = Plan.IdEmpleado ?? 0;
            if (RrhhCboEmpleado.SelectedIndex < 0) RrhhCboEmpleado.SelectedIndex = 0;
            RrhhMetCargarEvaluaciones();
            if (Plan.IdEvaluacionDesempeno.HasValue)
                RrhhCboEvaluacion.SelectedValue = Plan.IdEvaluacionDesempeno.Value;
        }

        private void RrhhMetLimpiarCampos()
        {
            RrhhTxtNombrePlan.Clear();
            RrhhTxtDescripcion.Clear();
            if (RrhhCboEmpleado.Items.Count > 0) RrhhCboEmpleado.SelectedIndex = 0;
            RrhhMetCargarEvaluaciones();
        }

        // ------------------------------------------------------------ Modos de la ventana

        // Consulta: los campos son de solo lectura y se navega por la tabla.
        private void RrhhMetModoConsulta()
        {
            _Edicion = null;
            RrhhMetHabilitar(false);
        }

        private void RrhhMetModoEdicion(EstadoEntidad Edicion)
        {
            _Edicion = Edicion;
            RrhhMetHabilitar(true);
            RrhhTxtNombrePlan.Focus();
        }

        private void RrhhMetHabilitar(bool Editando)
        {
            RrhhTxtNombrePlan.ReadOnly = !Editando;
            RrhhTxtDescripcion.ReadOnly = !Editando;
            // El empleado siempre se puede buscar: en consulta filtra la tabla y al editar recibe el plan.
            RrhhCboEvaluacion.Enabled = Editando;

            RrhhBtnGuardar.Enabled = Editando;
            RrhhBtnCancelar.Enabled = Editando;
            RrhhBtnNuevo.Enabled = !Editando && _MisPermisos.PuedeInsertar;
            RrhhBtnModificar.Enabled = !Editando && _MisPermisos.PuedeEditar;
            RrhhBtnEliminar.Enabled = !Editando && _MisPermisos.PuedeEliminar;

            RrhhBtnRefrescar.Enabled = !Editando;
            RrhhBtnBuscar.Enabled = !Editando;
            RrhhTxtBuscar.Enabled = !Editando;
            RrhhDgvPlanes.Enabled = !Editando;
            RrhhBtnInicio.Enabled = RrhhBtnAnterior.Enabled = RrhhBtnSiguiente.Enabled = RrhhBtnFin.Enabled = !Editando;
        }

        // ------------------------------------------------------------ Eventos

        private void RrhhDgvPlanes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) RrhhMetCargarFilaEnFormulario(e.RowIndex);
        }

        private void RrhhCboEmpleado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            RrhhMetAplicarEmpleado();
        }

        // Se llama al elegir de la lista, al presionar Enter o al salir del combo después de escribir.
        private void RrhhMetAplicarEmpleado()
        {
            // Si lo escrito no coincide con ningún empleado, se vuelve a "(Sin empleado)".
            if (RrhhCboEmpleado.SelectedIndex < 0 && RrhhCboEmpleado.Items.Count > 0)
                RrhhCboEmpleado.SelectedIndex = 0;

            int IdEmpleado = RrhhMetIdEmpleadoSeleccionado() ?? 0;
            if (IdEmpleado == _IdEmpleadoAplicado) return;

            if (_Edicion != null)
            {
                RrhhMetCargarEvaluaciones();
                if (IdEmpleado > 0 && RrhhCboEvaluacion.Items.Count == 0)
                    MessageBox.Show("El empleado no tiene evaluaciones disponibles. Regístrela primero en Realizar Evaluación.",
                        "Plan de Mejora", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // En consulta: muestra solo los planes del empleado elegido (o todos con "(Sin empleado)").
            _IdEmpleadoAplicado = IdEmpleado;
            RrhhTxtBuscar.Clear();
            RrhhDgvPlanes.DataSource = _PlanMejora.RrhhMetBuscarPorEmpleado(IdEmpleado);
            RrhhDgvPlanes.ClearSelection();
            RrhhDgvPlanes.CurrentCell = null;
            _IdPlanSeleccionado = 0;
            RrhhTxtNombrePlan.Clear();
            RrhhTxtDescripcion.Clear();
            RrhhMetCargarEvaluaciones();
        }

        private void RrhhTxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            RrhhBtnBuscar_Click(sender, e);
        }

        private void RrhhBtnBuscar_Click(object sender, EventArgs e)
        {
            RrhhMetListarPlanes(RrhhTxtBuscar.Text);
            if (RrhhDgvPlanes.Rows.Count == 0)
                MessageBox.Show("No se encontraron planes de mejora con ese criterio.", "Buscar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RrhhBtnNuevo_Click(object sender, EventArgs e)
        {
            RrhhDgvPlanes.ClearSelection();
            RrhhDgvPlanes.CurrentCell = null;
            _IdPlanSeleccionado = 0;
            _Edicion = EstadoEntidad.Added;
            RrhhMetLimpiarCampos();
            RrhhMetModoEdicion(EstadoEntidad.Added);
        }

        private void RrhhBtnModificar_Click(object sender, EventArgs e)
        {
            if (_IdPlanSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un plan de mejora de la tabla para modificarlo.", "Modificar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            RrhhMetModoEdicion(EstadoEntidad.Modified);
        }

        private void RrhhBtnGuardar_Click(object sender, EventArgs e)
        {
            if (_Edicion == null) return;

            _PlanMejora.IdPlanMejora = _Edicion == EstadoEntidad.Added ? 0 : _IdPlanSeleccionado;
            _PlanMejora.NombrePlanMejora = RrhhTxtNombrePlan.Text;
            _PlanMejora.DescripcionPlanMejora = RrhhTxtDescripcion.Text;
            _PlanMejora.IdEmpleado = RrhhMetIdEmpleadoSeleccionado();
            _PlanMejora.IdEvaluacionDesempeno = _PlanMejora.IdEmpleado.HasValue && RrhhCboEvaluacion.SelectedValue != null
                ? Convert.ToInt32(RrhhCboEvaluacion.SelectedValue)
                : (int?)null;
            _PlanMejora.Estado = _Edicion.Value;

            string Resultado = _PlanMejora.RrhhMetGrabarCambios();
            bool Exito = Resultado.Contains("exitosa");
            MessageBox.Show(Resultado, "Guardar", MessageBoxButtons.OK,
                Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (!Exito) return;

            int IdGuardado = _PlanMejora.IdPlanMejora;
            RrhhMetModoConsulta();
            RrhhMetRecargar();
            RrhhMetSeleccionarPlan(IdGuardado);
        }

        private void RrhhBtnEliminar_Click(object sender, EventArgs e)
        {
            if (_IdPlanSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un plan de mejora de la tabla para eliminarlo.", "Eliminar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var Respuesta = MessageBox.Show(
                "¿Desea eliminar el plan de mejora seleccionado?\nLa evaluación a la que pertenece quedará sin plan.",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Respuesta != DialogResult.Yes) return;

            _PlanMejora.IdPlanMejora = _IdPlanSeleccionado;
            _PlanMejora.NombrePlanMejora = RrhhTxtNombrePlan.Text;
            _PlanMejora.Estado = EstadoEntidad.Deleted;

            string Resultado = _PlanMejora.RrhhMetGrabarCambios();
            MessageBox.Show(Resultado, "Eliminar");
            if (Resultado.Contains("exitosa")) RrhhMetRecargar();
        }

        private void RrhhBtnCancelar_Click(object sender, EventArgs e)
        {
            RrhhMetModoConsulta();
            int IdPlan = _IdPlanSeleccionado;
            RrhhMetLimpiarCampos();
            RrhhMetSeleccionarPlan(IdPlan);
        }

        private void RrhhBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhTxtBuscar.Clear();
            RrhhMetRecargar();
        }

        private void RrhhBtnSalir_Click(object sender, EventArgs e)
        {
            if (_Edicion != null &&
                MessageBox.Show("Hay cambios sin guardar. ¿Desea salir de todas formas?", "Salir",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Close();
        }

        // ------------------------------------------------------------ Apoyo

        // Los empleados pueden cambiar (nuevas evaluaciones), por eso se recargan junto con la tabla.
        private void RrhhMetRecargar()
        {
            try
            {
                RrhhMetCargarEmpleados();
                RrhhMetListarPlanes(RrhhTxtBuscar.Text);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudieron cargar los planes de mejora: " + Ex.Message, "Plan de Mejora",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RrhhMetSeleccionarPlan(int IdPlan)
        {
            foreach (DataGridViewRow Fila in RrhhDgvPlanes.Rows)
            {
                if (((ClsModeloPlanMejora)Fila.DataBoundItem).IdPlanMejora == IdPlan)
                {
                    RrhhMetIrAFila(Fila.Index);
                    return;
                }
            }
        }

        private int RrhhMetFilaActual()
        {
            return RrhhDgvPlanes.CurrentCell == null ? -1 : RrhhDgvPlanes.CurrentCell.RowIndex;
        }

        private void RrhhMetIrAFila(int Indice)
        {
            if (RrhhDgvPlanes.Rows.Count == 0) return;
            Indice = Math.Max(0, Math.Min(Indice, RrhhDgvPlanes.Rows.Count - 1));
            RrhhDgvPlanes.ClearSelection();
            RrhhDgvPlanes.CurrentCell = RrhhDgvPlanes.Rows[Indice].Cells[RrhhColNombre.Index];
            RrhhDgvPlanes.Rows[Indice].Selected = true;
            RrhhMetCargarFilaEnFormulario(Indice);
        }

        private int? RrhhMetIdEmpleadoSeleccionado()
        {
            if (RrhhCboEmpleado.SelectedValue == null) return null;
            int IdEmpleado = Convert.ToInt32(RrhhCboEmpleado.SelectedValue);
            return IdEmpleado > 0 ? IdEmpleado : (int?)null;
        }
    }
}
