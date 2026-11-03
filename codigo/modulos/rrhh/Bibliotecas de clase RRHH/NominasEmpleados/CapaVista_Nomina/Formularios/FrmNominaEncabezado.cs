using CapaControlador_Nomina.Modelos_de_controladores;
using CapaControlador_Nomina.ObjetosdeValor;
using CapaVista_Nomina.Ayudas;
using CapaVista_Nomina.Reportes;
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
    public partial class FrmNominaEncabezado : Form
    {
        private ClsModeloNominaEncabezado _RrhhNomina = new ClsModeloNominaEncabezado();
        private ClsModeloCatalogos _RrhhCatalogos = new ClsModeloCatalogos();

        private List<KeyValuePair<int, string>> _EmpleadosActivos;   // para generar una nómina nueva
        private List<KeyValuePair<int, string>> _EmpleadosTodos;     // para mostrar nóminas ya guardadas

        private bool _Cargando = false;   // evita recalcular cuando el código llena los controles
        private const string ESTADO_INICIAL = "Aprobada";

        public FrmNominaEncabezado()
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

        

        private void NominaBtnEliminar_Click(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
            {
                MessageBox.Show("Seleccione una fila");
                return;
            }

            var Confirmacion = MessageBox.Show("¿Desea eliminar la nómina seleccionada?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Confirmacion != DialogResult.Yes)
                return;

            try
            {
                _RrhhNomina.IdNominaEncabezado = Seleccionado.IdNominaEncabezado;
                _RrhhNomina.Estado = EstadoEntidad.Deleted;
                string Resultado = _RrhhNomina.RrhhMetGrabarCambios();
                MessageBox.Show(Resultado);

                if (Resultado.EndsWith("exitosa"))
                {
                    RrhhTxtBuscar.Clear();
                    RrhhMetListarNominas();
                    RrhhMetReinicio();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }


        private void RrhhBtnAsignacionConceptos_Click(object sender, EventArgs e)
        {
            FrmAsignacionConceptos Conceptos = new FrmAsignacionConceptos();
            Conceptos.ShowDialog(this);
        }

       

        private void FrmNominaEncabezado_Load(object sender, EventArgs e)
        {
            RrhhMetConfigurarControles();
            RrhhMetConfigurarGrid();
            RrhhMetCargarEmpleados();
            RrhhMetListarNominas();
            RrhhMetReinicio();

        }

        private void RrhhMetConfigurarGrid()
        {
            RrhhDgvListaNominaEncabezado.AutoGenerateColumns = false;
            RrhhDgvListaNominaEncabezado.Columns.Clear();
            RrhhDgvListaNominaEncabezado.AllowUserToAddRows = false;
            RrhhDgvListaNominaEncabezado.AllowUserToDeleteRows = false;
            RrhhDgvListaNominaEncabezado.ReadOnly = true;
            RrhhDgvListaNominaEncabezado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RrhhDgvListaNominaEncabezado.MultiSelect = false;

            var Dinero = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight };
            var Fecha = new DataGridViewCellStyle { Format = "dd/MM/yyyy" };

            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdNominaEncabezado", DataPropertyName = "IdNominaEncabezado", HeaderText = "Código" });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "NombreEmpleado", DataPropertyName = "NombreEmpleado", HeaderText = "Empleado", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "FechaInicioNominaEncabezado", DataPropertyName = "FechaInicioNominaEncabezado", HeaderText = "Fecha Inicio", DefaultCellStyle = Fecha });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "FechaFinalNominaEncabezado", DataPropertyName = "FechaFinalNominaEncabezado", HeaderText = "Fecha Final", DefaultCellStyle = Fecha });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "SalarioBaseNominaEncabezado", DataPropertyName = "SalarioBaseNominaEncabezado", HeaderText = "Salario Base", DefaultCellStyle = Dinero });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "DuracionHoraExtraNominaEncabezado", DataPropertyName = "DuracionHoraExtraNominaEncabezado", HeaderText = "Min. Horas Extra" });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalHorasExtraNominaEncabezado", DataPropertyName = "TotalHorasExtraNominaEncabezado", HeaderText = "Pago Horas Extra", DefaultCellStyle = Dinero });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalPercepcionNominaEncabezado", DataPropertyName = "TotalPercepcionNominaEncabezado", HeaderText = "Percepciones", DefaultCellStyle = Dinero });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalDeduccionNominaEncabezado", DataPropertyName = "TotalDeduccionNominaEncabezado", HeaderText = "Deducciones", DefaultCellStyle = Dinero });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalNetoNominaEncabezado", DataPropertyName = "TotalNetoNominaEncabezado", HeaderText = "Neto a Pagar", DefaultCellStyle = Dinero });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstadoNminaEncabezado", DataPropertyName = "EstadoNminaEncabezado", HeaderText = "Estado" });
            RrhhDgvListaNominaEncabezado.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Fecha Creación" });
        }

        // 3. Carga de datos y metodos auxiliares
        private void RrhhMetConfigurarControles()
        {
            RrhhCboPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;

            RrhhCboEstadoNomina.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhCboEstadoNomina.Items.Clear();
            RrhhCboEstadoNomina.Items.AddRange(new object[] { "Aprobada", "Rechazada", "Pagada" });

            RrhhDtpFechaInicio.Format = DateTimePickerFormat.Short;
            RrhhDtpFechaFin.Format = DateTimePickerFormat.Short;

            // Solo muestran datos: ReadOnly mantiene el texto legible
            RrhhTxtDepartamento.ReadOnly = true;
            RrhhTxtSalarioBase.ReadOnly = true;
            RrhhTxtHorasExtra.ReadOnly = true;
            RrhhTxtTotalPercepciones.ReadOnly = true;
            RrhhTxtTotalDeducciones.ReadOnly = true;
            RrhhTxtTotalNeto.ReadOnly = true;
        }

        private void RrhhMetCargarEmpleados()
        {
            try
            {
                _EmpleadosActivos = _RrhhCatalogos.RrhhMetObtenerEmpleadosActivos();
                _EmpleadosTodos = _RrhhCatalogos.RrhhMetObtenerEmpleados();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudo cargar la lista de empleados: " + Ex.Message);
                _EmpleadosActivos = new List<KeyValuePair<int, string>>();
                _EmpleadosTodos = new List<KeyValuePair<int, string>>();
            }
        }

        private void RrhhMetEnlazarEmpleados(List<KeyValuePair<int, string>> Lista)
        {
            RrhhCboPeriodo.DisplayMember = "Value";
            RrhhCboPeriodo.ValueMember = "Key";
            RrhhCboPeriodo.DataSource = Lista;
            RrhhCboPeriodo.SelectedIndex = -1;
        }

        private void RrhhMetListarNominas()
        {
            try
            {
                RrhhDgvListaNominaEncabezado.DataSource = _RrhhNomina.RrhhMetObtenerTodos();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudo cargar la lista: " + Ex.Message);
            }
        }

        private void RrhhMetLimpiarMontos()
        {
            RrhhTxtSalarioBase.Text = "0.00";
            RrhhTxtHorasExtra.Text = "0.00";
            RrhhTxtTotalPercepciones.Text = "0.00";
            RrhhTxtTotalDeducciones.Text = "0.00";
            RrhhTxtTotalNeto.Text = "0.00";
        }

        private void RrhhMetReinicio()
        {
            _Cargando = true;
            try
            {
                RrhhMetEnlazarEmpleados(_EmpleadosActivos);
                RrhhTxtDepartamento.Clear();
                RrhhCboEstadoNomina.SelectedItem = ESTADO_INICIAL;
                RrhhDtpFechaInicio.Value = DateTime.Today;
                RrhhDtpFechaFin.Value = DateTime.Today;
                RrhhMetLimpiarMontos();
                RrhhDgvListaNominaEncabezado.ClearSelection();
            }
            finally
            {
                _Cargando = false;
            }
        }

        private ClsModeloNominaEncabezado RrhhMetObtenerSeleccionado()
        {
            if (RrhhDgvListaNominaEncabezado.SelectedRows.Count == 0)
                return null;
            return RrhhDgvListaNominaEncabezado.SelectedRows[0].DataBoundItem as ClsModeloNominaEncabezado;
        }

        // Calcula la nomina con empleado y fechas elegidos y muestra el resultado
        private void RrhhMetRecalcularNomina()
        {
            if (_Cargando || RrhhCboPeriodo.SelectedIndex < 0)
                return;

            // Cambiar empleado o fechas significa que se esta preparando una nomina nueva
            _Cargando = true;
            RrhhDgvListaNominaEncabezado.ClearSelection();
            RrhhCboEstadoNomina.SelectedItem = ESTADO_INICIAL;
            _Cargando = false;

            // Fechas en orden incorrecto: se limpian los montos sin molestar al usuario;
            // el mensaje aparece al guardar
            if (RrhhDtpFechaFin.Value.Date < RrhhDtpFechaInicio.Value.Date)
            {
                RrhhMetLimpiarMontos();
                return;
            }

            _RrhhNomina.IdEmpleado = Convert.ToInt32(RrhhCboPeriodo.SelectedValue);
            _RrhhNomina.FechaInicioNominaEncabezado = RrhhDtpFechaInicio.Value.Date;
            _RrhhNomina.FechaFinalNominaEncabezado = RrhhDtpFechaFin.Value.Date;

            string Error = _RrhhNomina.RrhhMetCalcularNomina();
            if (Error != null)
            {
                RrhhMetLimpiarMontos();
                MessageBox.Show(Error);
                return;
            }

            RrhhTxtSalarioBase.Text = _RrhhNomina.SalarioBaseNominaEncabezado.ToString("N2");
            RrhhTxtHorasExtra.Text = _RrhhNomina.TotalHorasExtraNominaEncabezado.ToString("N2");
            RrhhTxtTotalPercepciones.Text = _RrhhNomina.TotalPercepcionNominaEncabezado.ToString("N2");
            RrhhTxtTotalDeducciones.Text = _RrhhNomina.TotalDeduccionNominaEncabezado.ToString("N2");
            RrhhTxtTotalNeto.Text = _RrhhNomina.TotalNetoNominaEncabezado.ToString("N2");
        }

        // Eventos que disparan el calculo (solo por accion del usuario)
        

        private void RrhhDtpFechaInicio_ValueChanged(object sender, EventArgs e)
        {
            RrhhMetRecalcularNomina();
        }

        private void RrhhDtpFechaFin_ValueChanged(object sender, EventArgs e)
        {
            RrhhMetRecalcularNomina();
        }

        private void RrhhDedBtnGuardar_Click(object sender, EventArgs e)
        {
            if (RrhhCboPeriodo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un empleado");
                return;
            }

            try
            {
                // Flujo de guardado: copiar controles, asignar Estado, validar, grabar, mensaje y refrescar
                _RrhhNomina.IdNominaEncabezado = 0;
                _RrhhNomina.IdEmpleado = Convert.ToInt32(RrhhCboPeriodo.SelectedValue);
                _RrhhNomina.FechaInicioNominaEncabezado = RrhhDtpFechaInicio.Value.Date;
                _RrhhNomina.FechaFinalNominaEncabezado = RrhhDtpFechaFin.Value.Date;
                _RrhhNomina.EstadoNminaEncabezado = ESTADO_INICIAL;   // toda nómina nueva nace Aprobada
                _RrhhNomina.Estado = EstadoEntidad.Added;

                bool Valido = new ClsValidacionDatos(_RrhhNomina).RrhhMetValidar();
                if (Valido)
                {
                    string Resultado = _RrhhNomina.RrhhMetGrabarCambios();
                    MessageBox.Show(Resultado);

                    // Si hubo un problema se conservan los datos para que el usuario los corrija
                    if (Resultado.EndsWith("exitosa"))
                    {
                        RrhhTxtBuscar.Clear();
                        RrhhMetListarNominas();
                        RrhhMetReinicio();
                    }
                }
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

            string NuevoEstado = RrhhCboEstadoNomina.SelectedItem?.ToString();
            if (NuevoEstado == null)
            {
                MessageBox.Show("Seleccione el estado de la nómina");
                return;
            }

            if (NuevoEstado == "Rechazada" && Seleccionado.EstadoNminaEncabezado != "Rechazada")
            {
                var Confirmacion = MessageBox.Show(
                    "Al rechazar la nómina se liberan sus horas extra y ya no podrá reactivarse. ¿Desea continuar?",
                    "Confirmar rechazo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Confirmacion != DialogResult.Yes)
                    return;
            }

            try
            {
                // Por ahora solo se modifica el estado; empleado y fechas quedan como estaban
                _RrhhNomina.IdNominaEncabezado = Seleccionado.IdNominaEncabezado;
                _RrhhNomina.IdEmpleado = Seleccionado.IdEmpleado;
                _RrhhNomina.FechaInicioNominaEncabezado = Seleccionado.FechaInicioNominaEncabezado;
                _RrhhNomina.FechaFinalNominaEncabezado = Seleccionado.FechaFinalNominaEncabezado;
                _RrhhNomina.EstadoNminaEncabezado = NuevoEstado;
                _RrhhNomina.Estado = EstadoEntidad.Modified;

                bool Valido = new ClsValidacionDatos(_RrhhNomina).RrhhMetValidar();
                if (Valido)
                {
                    string Resultado = _RrhhNomina.RrhhMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    RrhhTxtBuscar.Clear();
                    RrhhMetListarNominas();
                    RrhhMetReinicio();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private void RrhhDedBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhTxtBuscar.Clear();
            RrhhMetCargarEmpleados();
            RrhhMetListarNominas();
            RrhhMetReinicio();
        }

        private void RrhhDedBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RrhhTxtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RrhhDgvListaNominaEncabezado.DataSource = _RrhhNomina
                    .RrhhMetBuscarPorNombre(RrhhTxtBuscar.Text.Trim())
                    .ToList();
                RrhhMetReinicio();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RrhhDgvListaNominaEncabezado_SelectionChanged(object sender, EventArgs e)
        {
            if (_Cargando)
                return;

            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
                return;

            _Cargando = true;
            try
            {
                // La nómina pudo ser de un empleado que ya no esta activo: se usa la lista completa
                RrhhMetEnlazarEmpleados(_EmpleadosTodos);
                RrhhCboPeriodo.SelectedValue = Seleccionado.IdEmpleado;
                RrhhTxtDepartamento.Text = _RrhhCatalogos.RrhhMetObtenerDepartamentoEmpleado(Seleccionado.IdEmpleado);
                RrhhCboEstadoNomina.SelectedItem = Seleccionado.EstadoNminaEncabezado;
                RrhhDtpFechaInicio.Value = Seleccionado.FechaInicioNominaEncabezado;
                RrhhDtpFechaFin.Value = Seleccionado.FechaFinalNominaEncabezado;
                RrhhTxtSalarioBase.Text = Seleccionado.SalarioBaseNominaEncabezado.ToString("N2");
                RrhhTxtHorasExtra.Text = Seleccionado.TotalHorasExtraNominaEncabezado.ToString("N2");
                RrhhTxtTotalPercepciones.Text = Seleccionado.TotalPercepcionNominaEncabezado.ToString("N2");
                RrhhTxtTotalDeducciones.Text = Seleccionado.TotalDeduccionNominaEncabezado.ToString("N2");
                RrhhTxtTotalNeto.Text = Seleccionado.TotalNetoNominaEncabezado.ToString("N2");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                _Cargando = false;
            }
        }

        private void RrhhMetIrAFila(int Indice)
        {
            if (RrhhDgvListaNominaEncabezado.Rows.Count == 0)
                return;

            Indice = Math.Max(0, Math.Min(Indice, RrhhDgvListaNominaEncabezado.Rows.Count - 1));
            RrhhDgvListaNominaEncabezado.ClearSelection();
            RrhhDgvListaNominaEncabezado.Rows[Indice].Selected = true;
            RrhhDgvListaNominaEncabezado.CurrentCell = RrhhDgvListaNominaEncabezado.Rows[Indice].Cells[0];
        }

        private int RrhhMetFilaActual()
        {
            return RrhhDgvListaNominaEncabezado.SelectedRows.Count > 0
                ? RrhhDgvListaNominaEncabezado.SelectedRows[0].Index
                : -1;
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
            RrhhMetIrAFila(RrhhDgvListaNominaEncabezado.Rows.Count - 1);
        }

        private void RrhhCboPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RrhhCboPeriodo.SelectedIndex >= 0)
            {
                try
                {
                    RrhhTxtDepartamento.Text = _RrhhCatalogos.RrhhMetObtenerDepartamentoEmpleado(Convert.ToInt32(RrhhCboPeriodo.SelectedValue));
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
            RrhhMetRecalcularNomina();
        }

        private void RrhhBtnAbrir_Click(object sender, EventArgs e)
        {
            FrmNominaDetalle NominaDetalle = new FrmNominaDetalle();
            NominaDetalle.ShowDialog(this);
        }

        private void RrhhDedBtnImprimir_Click(object sender, EventArgs e)
        {
            FrmReporteNominaEncabezado reporte = new FrmReporteNominaEncabezado();
            reporte.Show();
        }
    }
}
