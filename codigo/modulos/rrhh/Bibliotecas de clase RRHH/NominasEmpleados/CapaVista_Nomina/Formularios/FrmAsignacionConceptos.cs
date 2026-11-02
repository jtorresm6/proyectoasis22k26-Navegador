using CapaControlador_Nomina.Modelos_de_controladores;
using CapaControlador_Nomina.ObjetosdeValor;
using CapaVista_Nomina.Ayudas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Nomina.Formularios
{
    public partial class FrmAsignacionConceptos : Form
    {
        private ClsModeloPercepcionDeduccionEmpleado _RrhhAsignacion = new ClsModeloPercepcionDeduccionEmpleado();

        private const string TIPO_EMPLEADO = "Empleado";
        private const string TIPO_DEPARTAMENTO = "Departamento";
        private const string APLICACION_ALGUNOS = "Algunos";
        private bool _AjustandoTipoAsignacion = false;

        // Lista actual de asignaciones (para busquedas en el formulario)
        private List<ClsModeloPercepcionDeduccionEmpleado> _ListaAsignaciones = new List<ClsModeloPercepcionDeduccionEmpleado>();

        // Diccionarios Id -> Nombre para mostrar nombres en el grid en lugar de Ids
        private Dictionary<int, string> _NombresConceptos = new Dictionary<int, string>();
        private Dictionary<int, string> _NombresEmpleados = new Dictionary<int, string>();
        private Dictionary<int, string> _NombresDepartamentos = new Dictionary<int, string>();


        public FrmAsignacionConceptos()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RrhhMetAjustarAPantalla();
        }

        // en pantallas de 1366x768 la ventana se reduce para que quepa completa
        private void RrhhMetAjustarAPantalla()
        {
            Rectangle Area = Screen.FromControl(this).WorkingArea;
            double Factor = Math.Min(1d, Math.Min((double)Area.Width / Width, (double)Area.Height / Height));
            if (Factor >= 1d)
                return;

            List<KeyValuePair<Control, Font>> Fuentes = new List<KeyValuePair<Control, Font>>();
            RrhhMetGuardarFuentes(this, Fuentes);

            AutoScaleMode = AutoScaleMode.None;
            Scale(new SizeF((float)Factor, (float)Factor));
            foreach (KeyValuePair<Control, Font> Par in Fuentes)
                Par.Key.Font = RrhhMetEscalarFuente(Par.Value, Factor);

            foreach (DataGridView Tabla in Fuentes.Select(Par => Par.Key).OfType<DataGridView>())
            {
                Tabla.ColumnHeadersDefaultCellStyle.Font = RrhhMetEscalarFuente(Tabla.ColumnHeadersDefaultCellStyle.Font, Factor);
                Tabla.DefaultCellStyle.Font = RrhhMetEscalarFuente(Tabla.DefaultCellStyle.Font, Factor);
            }

            if (Owner != null)
                CenterToParent();
            else
                CenterToScreen();
        }

        private void RrhhMetGuardarFuentes(Control Padre, List<KeyValuePair<Control, Font>> Fuentes)
        {
            Fuentes.Add(new KeyValuePair<Control, Font>(Padre, Padre.Font));
            foreach (Control Hijo in Padre.Controls)
                RrhhMetGuardarFuentes(Hijo, Fuentes);
        }

        private Font RrhhMetEscalarFuente(Font Original, double Factor)
        {
            if (Original == null)
                return null;
            return new Font(Original.FontFamily, (float)(Original.Size * Factor), Original.Style, Original.Unit);
        }

        private void RrhhMetConfigurarGrid()
        {
            RrhhDgvListaAsignaciones.AutoGenerateColumns = false;
            RrhhDgvListaAsignaciones.Columns.Clear();
            RrhhDgvListaAsignaciones.AllowUserToAddRows = false;
            RrhhDgvListaAsignaciones.AllowUserToDeleteRows = false;
            RrhhDgvListaAsignaciones.ReadOnly = true;
            RrhhDgvListaAsignaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RrhhDgvListaAsignaciones.MultiSelect = false;

            RrhhDgvListaAsignaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdPercepcionDeduccionEmpleado", DataPropertyName = "IdPercepcionDeduccionEmpleado", HeaderText = "Código" });
            // Las columnas de Id se muestran como nombre (ver CellFormatting)
            RrhhDgvListaAsignaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdPercepcionDeduccion", DataPropertyName = "IdPercepcionDeduccion", HeaderText = "Concepto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            RrhhDgvListaAsignaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdEmpleado", DataPropertyName = "IdEmpleado", HeaderText = "Empleado", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            RrhhDgvListaAsignaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdDepartamento", DataPropertyName = "IdDepartamento", HeaderText = "Departamento", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            RrhhDgvListaAsignaciones.Columns.Add(new DataGridViewCheckBoxColumn { Name = "IsActive", DataPropertyName = "IsActive", HeaderText = "Activo" });
            RrhhDgvListaAsignaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Fecha Creación" });
            RrhhDgvListaAsignaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Última Actualización" });

            RrhhDgvListaAsignaciones.CellFormatting += RrhhDgvListaAsignaciones_CellFormatting;
        }

        //private void FrmAsignacionConceptos_Load(object sender, EventArgs e)
        //{
        //    RrhhMetCargarCombos();
        //    RrhhMetConfigurarGrid();
        //    RrhhMetListarAsignaciones();
        //    RrhhMetReinicio();
        //}

        private void RrhhMetCargarCombos()
        {

            // Conceptos: todos para los nombres del grid, pero el combo solo muestra los de aplicacion "Algunos"
            var TodosConceptos = new ClsModeloPercepcionDeduccion().RrhhMetObtenerTodos();
            _NombresConceptos = TodosConceptos.ToDictionary(c => c.IdPercepcionDeduccion, c => c.NombrePercepcionDeduccion);

            RrhhCboConcepto.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhCboConcepto.DisplayMember = "NombrePercepcionDeduccion";
            RrhhCboConcepto.ValueMember = "IdPercepcionDeduccion";
            RrhhCboConcepto.DataSource = TodosConceptos
                .Where(c => c.AplicacionPercepcionDeduccion == APLICACION_ALGUNOS)
                .ToList();

            // Empleados y departamentos: solo Id y nombre para los combos
            var Catalogos = new ClsModeloCatalogos();
            var Empleados = Catalogos.RrhhMetObtenerEmpleados();
            _NombresEmpleados = Empleados.ToDictionary(x => x.Key, x => x.Value);

            RrhhCboEmpleado.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhCboEmpleado.DisplayMember = "Value";
            RrhhCboEmpleado.ValueMember = "Key";
            RrhhCboEmpleado.DataSource = Empleados;

            var Departamentos = Catalogos.RrhhMetObtenerDepartamentos();
            _NombresDepartamentos = Departamentos.ToDictionary(x => x.Key, x => x.Value);

            RrhhCboDepartamento.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhCboDepartamento.DisplayMember = "Value";
            RrhhCboDepartamento.ValueMember = "Key";
            RrhhCboDepartamento.DataSource = Departamentos;

            // CheckedListBox con selección única: decide cual combo se habilita (no se guarda en la BD)
            RrhhClbTipoAsignacion.CheckOnClick = true;
            RrhhClbTipoAsignacion.Items.Clear();
            RrhhClbTipoAsignacion.Items.AddRange(new object[] { TIPO_EMPLEADO, TIPO_DEPARTAMENTO });
            RrhhClbTipoAsignacion.ItemCheck += RrhhClbTipoAsignacion_ItemCheck;



        }

        private void RrhhMetListarAsignaciones()
        {
            try
            {
                _ListaAsignaciones = _RrhhAsignacion.RrhhMetObtenerTodos();
                RrhhDgvListaAsignaciones.DataSource = _ListaAsignaciones;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudo cargar la lista: " + Ex.Message);
            }
        }

        private void RrhhMetReinicio()
        {
            RrhhCboConcepto.SelectedIndex = -1;
            RrhhMetMarcarTipoAsignacion(null);
            RrhhCboActivo.Checked = true;
            RrhhDgvListaAsignaciones.ClearSelection();
        }

        private string RrhhMetObtenerTipoAsignacion()
        {
            return RrhhClbTipoAsignacion.CheckedItems.Count > 0 ? RrhhClbTipoAsignacion.CheckedItems[0].ToString() : null;
        }

        private void RrhhMetMarcarTipoAsignacion(string Tipo)
        {
            _AjustandoTipoAsignacion = true;
            for (int i = 0; i < RrhhClbTipoAsignacion.Items.Count; i++)
                RrhhClbTipoAsignacion.SetItemChecked(i, RrhhClbTipoAsignacion.Items[i].ToString() == Tipo);
            _AjustandoTipoAsignacion = false;

            RrhhMetHabilitarCombosSegunTipo(Tipo);
        }

        private void RrhhMetHabilitarCombosSegunTipo(string Tipo)
        {
            bool EsEmpleado = Tipo == TIPO_EMPLEADO;
            bool EsDepartamento = Tipo == TIPO_DEPARTAMENTO;

            RrhhCboEmpleado.Enabled = EsEmpleado;
            RrhhCboDepartamento.Enabled = EsDepartamento;

            if (!EsEmpleado)
                RrhhCboEmpleado.SelectedIndex = -1;
            if (!EsDepartamento)
                RrhhCboDepartamento.SelectedIndex = -1;
        }

        private string RrhhMetObtenerNombre(Dictionary<int, string> Nombres, int? Id)
        {
            if (!Id.HasValue)
                return string.Empty;
            string Nombre;
            return Nombres.TryGetValue(Id.Value, out Nombre) ? Nombre : Id.Value.ToString();
        }

        private ClsModeloPercepcionDeduccionEmpleado RrhhMetObtenerSeleccionado()
        {
            if (RrhhDgvListaAsignaciones.SelectedRows.Count == 0)
                return null;
            return RrhhDgvListaAsignaciones.SelectedRows[0].DataBoundItem as ClsModeloPercepcionDeduccionEmpleado;
        }

        private bool RrhhMetCopiarControlesAlModelo()
        {
            string Tipo = RrhhMetObtenerTipoAsignacion();

            if (!(RrhhCboConcepto.SelectedValue is int IdConcepto))
            {
                MessageBox.Show("Seleccione la percepción/deducción");
                return false;
            }
            if (Tipo == null)
            {
                MessageBox.Show("Seleccione el tipo de asignación (Empleado o Departamento)");
                return false;
            }

            int? IdEmpleado = null;
            int? IdDepartamento = null;

            if (Tipo == TIPO_EMPLEADO)
            {
                if (!(RrhhCboEmpleado.SelectedValue is int IdEmp))
                {
                    MessageBox.Show("Seleccione un empleado");
                    return false;
                }
                IdEmpleado = IdEmp;
            }
            else
            {
                if (!(RrhhCboDepartamento.SelectedValue is int IdDep))
                {
                    MessageBox.Show("Seleccione un departamento");
                    return false;
                }
                IdDepartamento = IdDep;
            }

            _RrhhAsignacion.IdPercepcionDeduccion = IdConcepto;
            _RrhhAsignacion.IdEmpleado = IdEmpleado;
            _RrhhAsignacion.IdDepartamento = IdDepartamento;
            _RrhhAsignacion.IsActive = RrhhCboActivo.Checked;
            return true;
        }

        private void RrhhMetEjecutarOperacion(EstadoEntidad Estado, int IdAsignacion)
        {
            try
            {
                if (!RrhhMetCopiarControlesAlModelo())
                    return;

                _RrhhAsignacion.IdPercepcionDeduccionEmpleado = IdAsignacion;
                _RrhhAsignacion.Estado = Estado;

                bool Valido = new ClsValidacionDatos(_RrhhAsignacion).RrhhMetValidar();
                if (Valido)
                {
                    string Resultado = _RrhhAsignacion.RrhhMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    RrhhTxtBuscar.Clear();
                    RrhhMetListarAsignaciones();
                    RrhhMetReinicio();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }



        private void RrhhDgvListaAsignaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var Asignacion = RrhhDgvListaAsignaciones.Rows[e.RowIndex].DataBoundItem as ClsModeloPercepcionDeduccionEmpleado;
            if (Asignacion == null)
                return;

            string Columna = RrhhDgvListaAsignaciones.Columns[e.ColumnIndex].Name;

            if (Columna == "IdPercepcionDeduccion")
            {
                e.Value = RrhhMetObtenerNombre(_NombresConceptos, Asignacion.IdPercepcionDeduccion);
                e.FormattingApplied = true;
            }
            else if (Columna == "IdEmpleado")
            {
                e.Value = RrhhMetObtenerNombre(_NombresEmpleados, Asignacion.IdEmpleado);
                e.FormattingApplied = true;
            }
            else if (Columna == "IdDepartamento")
            {
                e.Value = RrhhMetObtenerNombre(_NombresDepartamentos, Asignacion.IdDepartamento);
                e.FormattingApplied = true;
            }
        }

        private void RrhhDedBtnGuardar_Click(object sender, EventArgs e)
        {
            RrhhMetEjecutarOperacion(EstadoEntidad.Added, 0);
        }

        private void RrhhDedBtnEliminar_Click(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
            {
                MessageBox.Show("Seleccione una fila");
                return;
            }

            var Confirmacion = MessageBox.Show("¿Desea eliminar la asignación seleccionada?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Confirmacion != DialogResult.Yes)
                return;

            try
            {
                _RrhhAsignacion.IdPercepcionDeduccionEmpleado = Seleccionado.IdPercepcionDeduccionEmpleado;
                _RrhhAsignacion.Estado = EstadoEntidad.Deleted;
                string Resultado = _RrhhAsignacion.RrhhMetGrabarCambios();
                MessageBox.Show(Resultado);
                RrhhTxtBuscar.Clear();
                RrhhMetListarAsignaciones();
                RrhhMetReinicio();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RrhhDedBtnModificar_Click(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
            {
                MessageBox.Show("Seleccione una fila");
                return;
            }
            RrhhMetEjecutarOperacion(EstadoEntidad.Modified, Seleccionado.IdPercepcionDeduccionEmpleado);

        }

        private void RrhhDedBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhTxtBuscar.Clear();
            RrhhMetListarAsignaciones();
            RrhhMetReinicio();
        }

        private void RrhhTxtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string Texto = RrhhTxtBuscar.Text.Trim().ToUpper();
                RrhhDgvListaAsignaciones.DataSource = _ListaAsignaciones.FindAll(a =>
                    RrhhMetObtenerNombre(_NombresConceptos, a.IdPercepcionDeduccion).ToUpper().Contains(Texto) ||
                    RrhhMetObtenerNombre(_NombresEmpleados, a.IdEmpleado).ToUpper().Contains(Texto) ||
                    RrhhMetObtenerNombre(_NombresDepartamentos, a.IdDepartamento).ToUpper().Contains(Texto));
                RrhhMetReinicio();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RrhhDgvListaAsignaciones_SelectionChanged(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
                return;

            // Si el concepto ya no es "Algunos" no esta en el combo y queda sin seleccion
            RrhhCboConcepto.SelectedValue = Seleccionado.IdPercepcionDeduccion;

            string Tipo = null;
            if (Seleccionado.IdEmpleado.HasValue)
                Tipo = TIPO_EMPLEADO;
            else if (Seleccionado.IdDepartamento.HasValue)
                Tipo = TIPO_DEPARTAMENTO;

            RrhhMetMarcarTipoAsignacion(Tipo);

            if (Tipo == TIPO_EMPLEADO)
                RrhhCboEmpleado.SelectedValue = Seleccionado.IdEmpleado.Value;
            else if (Tipo == TIPO_DEPARTAMENTO)
                RrhhCboDepartamento.SelectedValue = Seleccionado.IdDepartamento.Value;

            RrhhCboActivo.Checked = Seleccionado.IsActive;
        }

        private void RrhhClbTipoAsignacion_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_AjustandoTipoAsignacion)
                return;

            _AjustandoTipoAsignacion = true;
            string Tipo = null;
            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < RrhhClbTipoAsignacion.Items.Count; i++)
                    if (i != e.Index)
                        RrhhClbTipoAsignacion.SetItemChecked(i, false);
                Tipo = RrhhClbTipoAsignacion.Items[e.Index].ToString();
            }
            _AjustandoTipoAsignacion = false;

            RrhhMetHabilitarCombosSegunTipo(Tipo);
        }

        private void RrhhMetIrAFila(int Indice)
        {
            if (RrhhDgvListaAsignaciones.Rows.Count == 0)
                return;

            Indice = Math.Max(0, Math.Min(Indice, RrhhDgvListaAsignaciones.Rows.Count - 1));
            RrhhDgvListaAsignaciones.ClearSelection();
            RrhhDgvListaAsignaciones.Rows[Indice].Selected = true;
            RrhhDgvListaAsignaciones.CurrentCell = RrhhDgvListaAsignaciones.Rows[Indice].Cells[0];
        }

        private int RrhhMetFilaActual()
        {
            return RrhhDgvListaAsignaciones.SelectedRows.Count > 0 ? RrhhDgvListaAsignaciones.SelectedRows[0].Index : -1;
        }

        private void RrhhDedBtnInicio_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(0);
        }

        private void RrhhDedBtnAnterior_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(RrhhMetFilaActual() - 1);
        }

        private void RrhhDedBtnSiguiente_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(RrhhMetFilaActual() + 1);
        }

        private void RrhhDedBtnFin_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(RrhhDgvListaAsignaciones.Rows.Count - 1);
        }

        private void FrmAsignacionConceptos_Load(object sender, EventArgs e)
        {
            RrhhMetCargarCombos();
            RrhhMetConfigurarGrid();
            RrhhMetListarAsignaciones();
            RrhhMetReinicio();
        }
    }
}
