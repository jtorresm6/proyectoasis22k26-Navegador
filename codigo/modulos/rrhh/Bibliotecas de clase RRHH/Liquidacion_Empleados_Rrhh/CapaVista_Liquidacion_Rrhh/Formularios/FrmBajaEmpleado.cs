using CapaControlador_Liquidacion_Rrhh;
using CapaControlador_Liquidacion_Rrhh.Objetos_de_valor;
using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Liquidacion_Rrhh.frmReportes;
using CapaVista_Seguridad.Ayudas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CapaVista_Liquidacion_Rrhh.Formularios
{
    public partial class FrmBajaEmpleado : Form
    {
        private ClsModeloBajaEmpleado _BajaEmpleado  = new ClsModeloBajaEmpleado();
        private ClsPermisoAplicacion _MisPermisos;
        private BindingSource _BindingSource = new BindingSource();
        private List<ClsModeloBajaEmpleado> _ListaBajaEmpleados = new List<ClsModeloBajaEmpleado>();
        
        private const int ID_MODULO = 9000;
        private const int ID_APLICACION = 9704;
        public FrmBajaEmpleado()
        {
            InitializeComponent();
        }

        private void FrmBajaEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                var MapaBotones = new Dictionary<Control, TipoPermiso>
                {
                    { RrhhBtnGuardarBajaEmp,   TipoPermiso.Insertar },
                    { RrhhBtnModificarBajaEmp, TipoPermiso.Editar },
                    { RrhhBtnImprimirBajaEmp,   TipoPermiso.Imprimir },
                    { RrhhBtnEliminarBajaEmp,   TipoPermiso.Eliminar }
                };

                _MisPermisos = ClsSeguridadFormHelper.SeguridadMetInicializarSeguridad(
                    this, ID_MODULO, ID_APLICACION, MapaBotones);

                if (!_MisPermisos.TieneAcceso)
                    return;

                RrhhMetConfigurarColumnasBajaEmpleado();
                RrhhDgvBajaEmp.DataSource = _BindingSource;
                RrhhdMetListarBajaEmp();
                RrhhMetCargarCombos();

                _BindingSource.CurrentChanged += RrhhBsCurrentChanged;
               
                RrhhBtnInicioBajaEmp.Click += RrhhBtnInicioBajaEmp_Click;
                RrhhBtnAnteriorBajaEmp.Click += RrhhBtnAnteriorBajaEmp_Click;
                RrhhBtnSiguienteBajaEmp.Click += RrhhBtnSiguienteBajaEmp_Click;
                RrhhBtnFinBajaEmp.Click += RrhhBtnFinBajaEmp_Click;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + Ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RrhhBsCurrentChanged(object sender, EventArgs e)
        {
            if (_BindingSource.Current is ClsModeloBajaEmpleado BajaEmpleado)
            {
                RrhhTxtIdBajaEmpleado.Text = BajaEmpleado.IdBajaEmpleado.ToString();
                RrhhTxtIdEmpleado.Text = BajaEmpleado.IdEmpleado.ToString();
                RrhhTxtIdMotivoBaja.Text = BajaEmpleado.IdMotivoBaja.ToString();
                RrhhDtpFechaBajaEmpleado.Value = BajaEmpleado.FechaBajaEmpleado;
                RrhhChkEstadoBajaEmp.Checked = Convert.ToBoolean(BajaEmpleado.IsActive);
                

                if (RrhhCboEmpleado.DataSource != null)
                    RrhhCboEmpleado.SelectedValue = BajaEmpleado.IdEmpleado;

                if (RrhhCboMotivoBaja.DataSource != null)
                    RrhhCboMotivoBaja.SelectedValue = BajaEmpleado.IdMotivoBaja;

            }
        }
        private void RrhhMetCargarCombos()
        {
            try
            {
                // 1. Cargar Empleados
                RrhhCboEmpleado.SelectedIndexChanged -= RrhhCboEmpleado_SelectedIndexChanged;

                RrhhCboEmpleado.DisplayMember = "nombreEmpleado"; // Coincide con AS nombreEmpleado en tu SQL
                RrhhCboEmpleado.ValueMember = "idEmpleado";       // Coincide con idEmpleado en tu SQL
                RrhhCboEmpleado.DataSource = _BajaEmpleado.LiquidacionMetObtenerEmpleados();
                RrhhCboEmpleado.SelectedIndex = -1;

                RrhhCboEmpleado.SelectedIndexChanged += RrhhCboEmpleado_SelectedIndexChanged;

                // 2. Cargar Motivos de Baja
                RrhhCboMotivoBaja.SelectedIndexChanged -= RrhhCboMotivoBaja_SelectedIndexChanged;

                RrhhCboMotivoBaja.DisplayMember = "nombreMotivoBaja"; // Coincide con nombreMotivoBaja en tu SQL
                RrhhCboMotivoBaja.ValueMember = "idMotivoBaja";       // Coincide con idMotivoBaja en tu SQL
                RrhhCboMotivoBaja.DataSource = _BajaEmpleado.LiquidacionMetObtenerMotivosBaja();
                RrhhCboMotivoBaja.SelectedIndex = -1;

                RrhhCboMotivoBaja.SelectedIndexChanged += RrhhCboMotivoBaja_SelectedIndexChanged;

                // 3. Cargar Usuario en Sesión

                RrhhTxtIdUsuRegistra.Text = ClsSesionSeguridad.IdUsuario.ToString();

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar combos: {ex.Message}");
            }
        }
        private void RrhhMetConfigurarColumnasBajaEmpleado()
        {
            RrhhDgvBajaEmp.AutoGenerateColumns = false;
            RrhhDgvBajaEmp.Columns.Clear();

            RrhhDgvBajaEmp.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdEmpleado",
                DataPropertyName = "IdEmpleado",
                HeaderText = "ID EMP",
                Width = 50
            });
            RrhhDgvBajaEmp.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdMotivoBaja",
                DataPropertyName = "IdMotivoBaja",
                HeaderText = "ID MBaja",
                Width = 90
            });
            RrhhDgvBajaEmp.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFechaBajaEmpleado",
                DataPropertyName = "FechaBajaEmpleado",
                HeaderText = "Fecha_Baja",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });
            RrhhDgvBajaEmp.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colObservacionBajaEmpleado",
                DataPropertyName = "ObservacionBajaEmpleado",
                HeaderText = "Observaciones",
                Width = 150,
                
            });
            RrhhDgvBajaEmp.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdUsuarioRegistra",
                DataPropertyName = "IdUsuarioRegistra",
                HeaderText = "ID_Usu_Regristra",
                Width = 150,
             
            });
            RrhhDgvBajaEmp.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colIsActive",
                DataPropertyName = "IsActive",
                HeaderText = "Estado",
                Width = 60
            });
        }

        private void RrhhdMetListarBajaEmp()
        {
            try
            {
                _ListaBajaEmpleados = _BajaEmpleado.LiquidacionMetObtenerTodos();
                _BindingSource.DataSource = _ListaBajaEmpleados;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void RrhhCboEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RrhhCboEmpleado.SelectedValue != null)
                RrhhTxtIdEmpleado.Text = RrhhCboEmpleado.SelectedValue.ToString();
        }

        private void RrhhCboMotivoBaja_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RrhhCboMotivoBaja.SelectedValue != null)
                RrhhTxtIdMotivoBaja.Text = RrhhCboMotivoBaja.SelectedValue.ToString();

        }

        private void RrhhBtnInicioBajaEmp_Click(object sender, EventArgs e)
        {
            if (_BindingSource.Count > 0)
                _BindingSource.MoveFirst();
        }

        private void RrhhBtnAnteriorBajaEmp_Click(object sender, EventArgs e)
        {
            if (_BindingSource.Position > 0)
                _BindingSource.MovePrevious();
        }

        private void RrhhBtnSiguienteBajaEmp_Click(object sender, EventArgs e)
        {
            if (_BindingSource.Position < _BindingSource.Count - 1)
                _BindingSource.MoveNext();
        }

        private void RrhhBtnFinBajaEmp_Click(object sender, EventArgs e)
        {
            if (_BindingSource.Count > 0)
                _BindingSource.MoveLast();
        }

        private void RrhhBtnGuardarBajaEmp_Click(object sender, EventArgs e)
        {
            if (!_MisPermisos.PuedeInsertar)
            {
                MessageBox.Show("No tienes permiso para agregar usuarios.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(RrhhTxtIdEmpleado.Text))
                {
                    MessageBox.Show("Seleccione un empleado del listado.", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(RrhhTxtIdMotivoBaja.Text))
                {
                    MessageBox.Show("Seleccione un Motivo de baja del listado.", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _BajaEmpleado.IdEmpleado = Convert.ToInt32(RrhhTxtIdEmpleado.Text);
                _BajaEmpleado.IdMotivoBaja = Convert.ToInt32(RrhhTxtIdMotivoBaja.Text);
                _BajaEmpleado.FechaBajaEmpleado = RrhhDtpFechaBajaEmpleado.Value.Date;
                _BajaEmpleado.ObservacionBajaEmpleado = RrhhTxtObservacionesBaja.Text;
                _BajaEmpleado.IdUsuarioRegistra = Convert.ToInt32(RrhhTxtIdUsuRegistra.Text);
                _BajaEmpleado.IsActive = RrhhChkEstadoBajaEmp.Checked ? 1 : 0;
                _BajaEmpleado.Estado = EstadoEntidadLiquidacion.Added;

                bool Valido = new ClsValidacionDatos(_BajaEmpleado).SeguridadMetValidar();
                if (Valido)
                {
                    string Resultado = _BajaEmpleado.LiquidacionMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    RrhhdMetListarBajaEmp();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void RrhhBtnModificarBajaEmp_Click(object sender, EventArgs e)
        {
            if (!_MisPermisos.PuedeEditar)
            {
                MessageBox.Show("No tienes permiso para modificar Baja Empleado.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                // Validar que exista un ID cargado en la caja de texto visual
                if (string.IsNullOrWhiteSpace(RrhhTxtIdBajaEmpleado.Text))
                {
                    MessageBox.Show("Seleccione o haga clic sobre una fila del listado para cargar los datos a modificar.",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(RrhhTxtIdEmpleado.Text))
                {
                    MessageBox.Show("Debe seleccionar un empleado válido.", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Captura del ID desde el TextBox visual en lugar de las celdas del DataGridView
                _BajaEmpleado.IdBajaEmpleado = Convert.ToInt32(RrhhTxtIdBajaEmpleado.Text);
                _BajaEmpleado.IdEmpleado = Convert.ToInt32(RrhhTxtIdEmpleado.Text);
                _BajaEmpleado.IdMotivoBaja = Convert.ToInt32(RrhhTxtIdMotivoBaja.Text);
                _BajaEmpleado.FechaBajaEmpleado = RrhhDtpFechaBajaEmpleado.Value.Date;
                _BajaEmpleado.ObservacionBajaEmpleado = RrhhTxtObservacionesBaja.Text;
                _BajaEmpleado.IdUsuarioRegistra = Convert.ToInt32(RrhhTxtIdUsuRegistra.Text);
                _BajaEmpleado.IsActive = RrhhChkEstadoBajaEmp.Checked ? 1 : 0;
                _BajaEmpleado.Estado = EstadoEntidadLiquidacion.Modified;

                bool Valido = new ClsValidacionDatos(_BajaEmpleado).SeguridadMetValidar();
                if (Valido)
                {
                    string Resultado = _BajaEmpleado.LiquidacionMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    RrhhdMetListarBajaEmp();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        
        }

        private void RrhhBtnEliminarBajaEmp_Click(object sender, EventArgs e)
        {
            if (!_MisPermisos.PuedeEliminar)
            {
                MessageBox.Show("No tienes permiso para eliminar Baja Empleado",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                // Validar que exista un ID cargado en la caja de texto visual
                if (string.IsNullOrWhiteSpace(RrhhTxtIdBajaEmpleado.Text))
                {
                    MessageBox.Show("Seleccione una fila del listado para eliminar.",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult Confirmacion = MessageBox.Show(
                    "¿Está seguro de que desea eliminar la Baja Empleado seleccionada?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (Confirmacion == DialogResult.Yes)
                {
                    // Captura del ID desde el TextBox visual
                    _BajaEmpleado.IdBajaEmpleado = Convert.ToInt32(RrhhTxtIdBajaEmpleado.Text);
                    _BajaEmpleado.Estado = EstadoEntidadLiquidacion.Deleted;

                    string Resultado = _BajaEmpleado.LiquidacionMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    RrhhdMetListarBajaEmp();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void RrhhDgvBajaEmp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que se haya hecho clic sobre una fila válida (no en los encabezados)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = RrhhDgvBajaEmp.Rows[e.RowIndex];

                // Obtiene el valor de la celda de la llave primaria (ajusta el nombre de la columna si varía)
                RrhhTxtIdBajaEmpleado.Text = fila.Cells["IdBajaEmpleado"].Value?.ToString();
            }
        }

        private void RrhhBtnImprimirBajaEmp_Click(object sender, EventArgs e)
        {
            FrmReporteBajaEmpleado ReporteBajaEmpleado = new FrmReporteBajaEmpleado();
            ReporteBajaEmpleado.ShowDialog();
        }
    }

}
