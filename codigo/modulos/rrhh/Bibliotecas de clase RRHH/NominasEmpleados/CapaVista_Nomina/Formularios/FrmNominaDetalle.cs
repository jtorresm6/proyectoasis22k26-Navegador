using CapaControlador_Nomina.Modelos_de_controladores;
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
    public partial class FrmNominaDetalle : Form
    {
        private ClsModeloNominaEncabezado _RrhhNomina = new ClsModeloNominaEncabezado();
        private ClsModeloNominaDetalle _RrhhDetalle = new ClsModeloNominaDetalle();

        private List<ClsModeloNominaEncabezado> _Nominas = new List<ClsModeloNominaEncabezado>();
        private bool _Cargando = false;
        public FrmNominaDetalle()
        {
            InitializeComponent();
        }

        private void FrmNominaDetalle_Load(object sender, EventArgs e)
        {
            RrhhMetConfigurarControles();
            RrhhMetConfigurarGrid();
            RrhhMetCargarNominas(0);

        }

        private void RrhhMetConfigurarGrid()
        {
            RrhhDgvListaDetalle.AutoGenerateColumns = false;
            RrhhDgvListaDetalle.Columns.Clear();
            RrhhDgvListaDetalle.AllowUserToAddRows = false;
            RrhhDgvListaDetalle.AllowUserToDeleteRows = false;
            RrhhDgvListaDetalle.ReadOnly = true;
            RrhhDgvListaDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RrhhDgvListaDetalle.MultiSelect = false;

            RrhhDgvListaDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "NombreConcepto", DataPropertyName = "NombreConcepto", HeaderText = "Concepto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            RrhhDgvListaDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "TipoConcepto", DataPropertyName = "TipoConcepto", HeaderText = "Tipo" });
            RrhhDgvListaDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MontoNominaDetalle",
                DataPropertyName = "MontoNominaDetalle",
                HeaderText = "Monto",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        // 3. Carga de datos y metodos auxiliares
        private void RrhhMetConfigurarControles()
        {
            RrhhCboNombreEmpleado.DropDownStyle = ComboBoxStyle.DropDownList;

            // Solo muestran datos: ReadOnly mantiene el texto legible
            RrhhTxtSalarioBase.ReadOnly = true;
            RrhhTxtEstadoNomina.ReadOnly = true;
            RrhhTxtTotalPercepciones.ReadOnly = true;
            RrhhTxtTotalDeducciones.ReadOnly = true;
            RrhhTxtTotalNeto.ReadOnly = true;
        }

        // Llena el combo con "Empleado - periodo" de cada nomina (menos las rechazadas)
        // e intenta dejar seleccionada la nomina indicada.
        private void RrhhMetCargarNominas(int IdSeleccionar)
        {
            try
            {
                _Nominas = _RrhhNomina.RrhhMetObtenerTodos()
                    .Where(n => n.EstadoNminaEncabezado != "Rechazada")
                    .OrderBy(n => n.NombreEmpleado)
                    .ThenByDescending(n => n.FechaInicioNominaEncabezado)
                    .ToList();

                List<int> ConDetalle = _RrhhDetalle.RrhhMetObtenerNominasConDetalle();

                var Opciones = new List<KeyValuePair<int, string>>();
                foreach (ClsModeloNominaEncabezado Nomina in _Nominas)
                {
                    string Texto = Nomina.NombreEmpleado + " - " +
                                   Nomina.FechaInicioNominaEncabezado.ToString("dd/MM/yyyy") + " al " +
                                   Nomina.FechaFinalNominaEncabezado.ToString("dd/MM/yyyy");
                    if (!ConDetalle.Contains(Nomina.IdNominaEncabezado))
                        Texto += " (sin detalle)";
                    Opciones.Add(new KeyValuePair<int, string>(Nomina.IdNominaEncabezado, Texto));
                }

                RrhhCboNombreEmpleado.DisplayMember = "Value";
                RrhhCboNombreEmpleado.ValueMember = "Key";
                RrhhCboNombreEmpleado.DataSource = Opciones;
                RrhhCboNombreEmpleado.SelectedIndex = -1;

                int Indice = Opciones.FindIndex(o => o.Key == IdSeleccionar);
                if (Indice >= 0)
                {
                    RrhhCboNombreEmpleado.SelectedIndex = Indice;
                    RrhhMetCargarNomina(IdSeleccionar);
                }
                else
                {
                    RrhhMetLimpiar();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudo cargar la lista de nóminas: " + Ex.Message);
            }
        }

        // Se lee desde SelectedItem: dentro de SelectionChangeCommitted, SelectedValue puede venir desactualizado
        private int RrhhMetObtenerIdNominaSeleccionada()
        {
            object Item = RrhhCboNombreEmpleado.SelectedItem;
            if (Item == null)
                return 0;
            return ((KeyValuePair<int, string>)Item).Key;
        }

        private void RrhhMetLimpiar()
        {
            RrhhTxtSalarioBase.Clear();
            RrhhTxtEstadoNomina.Clear();
            RrhhTxtTotalPercepciones.Clear();
            RrhhTxtTotalDeducciones.Clear();
            RrhhTxtTotalNeto.Clear();
            RrhhDgvListaDetalle.DataSource = null;

            _Cargando = true;
            RrhhTxtBuscar.Clear();
            _Cargando = false;
        }

        // Muestra los valores GUARDADOS en el encabezado y las lineas de su detalle
        private void RrhhMetCargarNomina(int IdNominaEncabezado)
        {
            ClsModeloNominaEncabezado Nomina = _Nominas.FirstOrDefault(n => n.IdNominaEncabezado == IdNominaEncabezado);
            if (Nomina == null)
            {
                RrhhMetLimpiar();
                return;
            }

            try
            {
                RrhhTxtSalarioBase.Text = Nomina.SalarioBaseNominaEncabezado.ToString("N2");
                RrhhTxtEstadoNomina.Text = Nomina.EstadoNminaEncabezado;
                RrhhTxtTotalPercepciones.Text = Nomina.TotalPercepcionNominaEncabezado.ToString("N2");
                RrhhTxtTotalDeducciones.Text = Nomina.TotalDeduccionNominaEncabezado.ToString("N2");
                RrhhTxtTotalNeto.Text = Nomina.TotalNetoNominaEncabezado.ToString("N2");

                _Cargando = true;
                RrhhTxtBuscar.Clear();
                _Cargando = false;

                RrhhDgvListaDetalle.DataSource = _RrhhDetalle.RrhhMetObtenerPorNomina(IdNominaEncabezado);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudo cargar el detalle: " + Ex.Message);
            }
        }

        private void RrhhDedBtnGuardar_Click(object sender, EventArgs e)
        {
            int IdNomina = RrhhMetObtenerIdNominaSeleccionada();
            if (IdNomina <= 0)
            {
                MessageBox.Show("Seleccione una nómina");
                return;
            }

            var Confirmacion = MessageBox.Show("¿Desea generar el detalle de la nómina seleccionada?", "Confirmar generación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Confirmacion != DialogResult.Yes)
                return;

            try
            {
                string Resultado = _RrhhDetalle.RrhhMetGenerarDetalle(IdNomina);
                MessageBox.Show(Resultado);
                RrhhMetCargarNominas(IdNomina);   // actualiza el combo (marca "sin detalle") y el grid
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RrhhDedBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhMetCargarNominas(RrhhMetObtenerIdNominaSeleccionada());
        }

        private void RrhhDedBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RrhhTxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (_Cargando || RrhhMetObtenerIdNominaSeleccionada() <= 0)
                return;

            try
            {
                RrhhDgvListaDetalle.DataSource = _RrhhDetalle
                    .RrhhMetBuscarPorNombre(RrhhTxtBuscar.Text.Trim())
                    .ToList();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RrhhCboNombreEmpleado_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RrhhMetCargarNomina(RrhhMetObtenerIdNominaSeleccionada());
        }
        private void RrhhMetIrANomina(int Indice)
        {
            int Total = RrhhCboNombreEmpleado.Items.Count;
            if (Total == 0)
                return;

            Indice = Math.Max(0, Math.Min(Indice, Total - 1));
            RrhhCboNombreEmpleado.SelectedIndex = Indice;
            RrhhMetCargarNomina(RrhhMetObtenerIdNominaSeleccionada());
        }

        private void RrhhDedBtnInicio_Click(object sender, EventArgs e)
        {
            RrhhMetIrANomina(0);
        }

        private void RrhhDedBtnAnterior_Click(object sender, EventArgs e)
        {
            RrhhMetIrANomina(RrhhCboNombreEmpleado.SelectedIndex - 1);
        }

        private void RrhhDedBtnSiguiente_Click(object sender, EventArgs e)
        {
            RrhhMetIrANomina(RrhhCboNombreEmpleado.SelectedIndex + 1);
        }

        private void RrhhDedBtnFin_Click(object sender, EventArgs e)
        {
            RrhhMetIrANomina(RrhhCboNombreEmpleado.Items.Count - 1);
        }
    }
}
