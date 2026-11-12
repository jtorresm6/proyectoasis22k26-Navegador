using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Seguridad;
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
 * Área : Seguridad
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 22/09/2026
 * ==================================================================
 * Propósito :
 *  El formulario del módulo de Mantenimiento de Empleados nos 
 *  permite registrar, consultar, modificar y eliminar los 
 *  datos ingresados al sistema, tambien cuenta con Reportes y Ayudas.
 * ===================================================================
*/

namespace CapaVista_Seguridad
{
    public partial class FrmMantenimientoEmpleado : Form
    {
        private ClsModeloEmpleado _Empleado = new ClsModeloEmpleado();
        private ClsPermisoAplicacion _MisPermisos;

        private const int ID_MODULO = 4;       
        private const int ID_APLICACION = 4;   

        public FrmMantenimientoEmpleado()
        {
            InitializeComponent();
        }

        private const string PrefijoCodigoEmpleado = "EMP-";

        private void FrmMantenimientoEmpleado_Load(object sender, EventArgs e)
        {
            var MapaBotones = new Dictionary<Control, TipoPermiso>
    {
        { SeguridadBtnGuardar,   TipoPermiso.Insertar },
        { SeguridadBtnModificar, TipoPermiso.Editar },
        { SeguridadBtnEliminar,  TipoPermiso.Eliminar },
        { SeguridadBtnImprimir,  TipoPermiso.Imprimir }
    };

            _MisPermisos = ClsSeguridadFormHelper.SeguridadMetInicializarSeguridad(
                this, ID_MODULO, ID_APLICACION, MapaBotones);

            if (!_MisPermisos.TieneAcceso)
               return;

            SeguridadMetListarEmpleados();
            SeguridadTxtCodigo.Text = PrefijoCodigoEmpleado;
            SeguridadTxtCodigo.SelectionStart = SeguridadTxtCodigo.Text.Length;
            SeguridadMetHabilitarCampos(false);
            SeguridadMetActualizarContador();
        }

        private void SeguridadMetListarEmpleados()
        {
            try
            {
                SeguridadDgvEmpleados.DataSource = _Empleado.SeguridadMetObtenerTodos();
                SeguridadMetConfigurarEncabezadosGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SeguridadMetConfigurarEncabezadosGrid()
        {
            if (SeguridadDgvEmpleados.Columns["IdEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["IdEmpleado"].HeaderText = "No.";

            if (SeguridadDgvEmpleados.Columns["CodigoEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["CodigoEmpleado"].HeaderText = "Codigo";

            if (SeguridadDgvEmpleados.Columns["DpiEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["DpiEmpleado"].HeaderText = "Dpi";

            if (SeguridadDgvEmpleados.Columns["NitEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["NitEmpleado"].HeaderText = "Nit";

            if (SeguridadDgvEmpleados.Columns["NombresEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["NombresEmpleado"].HeaderText = "Nombre";

            if (SeguridadDgvEmpleados.Columns["ApellidosEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["ApellidosEmpleado"].HeaderText = "Apellido";

            if (SeguridadDgvEmpleados.Columns["PuestoEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["PuestoEmpleado"].HeaderText = "Puesto";

            if (SeguridadDgvEmpleados.Columns["GeneroEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["GeneroEmpleado"].HeaderText = "Genero";

            if (SeguridadDgvEmpleados.Columns["FechaNacimientoEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["FechaNacimientoEmpleado"].HeaderText = "Nacimiento";

            if (SeguridadDgvEmpleados.Columns["FechaContratacionEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["FechaContratacionEmpleado"].HeaderText = "Contratacion";

            if (SeguridadDgvEmpleados.Columns["TelefonoEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["TelefonoEmpleado"].HeaderText = "No.Telefono";

            if (SeguridadDgvEmpleados.Columns["CorreoEmpleado"] != null)
                SeguridadDgvEmpleados.Columns["CorreoEmpleado"].HeaderText = "Correo";

            if (SeguridadDgvEmpleados.Columns["IsActive"] != null)
                SeguridadDgvEmpleados.Columns["IsActive"].HeaderText = "Estado";

            if (SeguridadDgvEmpleados.Columns["CreatedAt"] != null)
                SeguridadDgvEmpleados.Columns["CreatedAt"].HeaderText = "Registrado";

            if (SeguridadDgvEmpleados.Columns["UpdatedAt"] != null)
                SeguridadDgvEmpleados.Columns["UpdatedAt"].HeaderText = "Última Modificación";
        }

        private void SeguridadMetActualizarContador()
        {
            int Total = SeguridadDgvEmpleados.Rows.Count;

            if (Total == 0)
            {
                LblSeguridadContadorEmpleados.Text = "Mostrando 0 de 0 registros";
                return;
            }

            int FilaActual = (SeguridadDgvEmpleados.CurrentCell != null)
                ? SeguridadDgvEmpleados.CurrentCell.RowIndex + 1
                : 1;

            LblSeguridadContadorEmpleados.Text = $"Mostrando {FilaActual} de {Total} registros";
        }


        private void SeguridadBtnAyuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "C:/SeguridadAyudas/SeguridadAyudas.chm", "Empleados_Seguridad.html");
        }

        private void SeguridadBtnAgregar_Click(object sender, EventArgs e)
        {
            SeguridadTxtCodigo.Text = "";
            SeguridadTxtDpi.Text = "";
            SeguridadTxtNit.Text = "";
            SeguridadTxtNombres.Text = "";
            SeguridadTxtApellidos.Text = "";
            SeguridadTxtPuesto.Text = "";
            SeguridadCboGenero.SelectedIndex = -1;
            SeguridadTxtTelefono.Text = "";
            SeguridadTxtCorreo.Text = "";

            _Empleado.Estado = EstadoEntidad.Added;

            SeguridadCboGenero.Enabled = true;
            SeguridadMetHabilitarCampos(true);
        }




        private void SeguridadMetHabilitarCampos(bool habilitar)
        {
            SeguridadTxtDpi.Enabled = habilitar;
            SeguridadTxtNit.Enabled = habilitar;
            SeguridadTxtNombres.Enabled = habilitar;
            SeguridadTxtApellidos.Enabled = habilitar;
            SeguridadTxtPuesto.Enabled = habilitar;
            SeguridadCboGenero.Enabled = habilitar;
            SeguridadDtpFechaNacimiento.Enabled = habilitar;
            SeguridadDtpFechaContratacion.Enabled = habilitar;
            SeguridadTxtTelefono.Enabled = habilitar;
            SeguridadTxtCorreo.Enabled = habilitar;
            SeguridadChkActivo.Enabled = habilitar;
        }
        private void SeguridadBtnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SeguridadTxtCodigo.Text)
                || SeguridadTxtCodigo.Text.Trim() == PrefijoCodigoEmpleado)
                {
                    MessageBox.Show("Ingrese un código de Empleado");
                    return;
                }


                string CodigoEmpleado = SeguridadTxtCodigo.Text;
                SeguridadDgvEmpleados.DataSource = _Empleado.SeguridadMetBuscarPorId(CodigoEmpleado);
                SeguridadMetConfigurarEncabezadosGrid();
            }
            catch (FormatException)
            {
                MessageBox.Show("El Id de Empleado debe ser un número");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SeguridadBtnEliminar_Click(object sender, EventArgs e)
        {
            if (SeguridadDgvEmpleados.SelectedRows.Count > 0)
            {
                _Empleado.Estado = EstadoEntidad.Deleted;
                _Empleado.IdEmpleado = Convert.ToInt32(SeguridadDgvEmpleados.CurrentRow.Cells[0].Value);

                string Resultado = _Empleado.SeguridadMetGrabarCambios();
                MessageBox.Show(Resultado);
                SeguridadMetListarEmpleados();
            }
            else MessageBox.Show("Seleccione una fila");
        }


        private void SeguridadBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SeguridadDgvEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SeguridadDgvEmpleados.SelectedRows.Count > 0)
            {
                SeguridadMetCargarFilaEnFormulario(SeguridadDgvEmpleados.CurrentRow.Index);
            }
        }

        private void SeguridadMetReinicio()
        {
            SeguridadTxtCodigo.Text = PrefijoCodigoEmpleado; 
            SeguridadTxtDpi.Text = "";
            SeguridadTxtNit.Text = "";
            SeguridadTxtNombres.Text = "";
            SeguridadTxtApellidos.Text = "";
            SeguridadTxtPuesto.Text = "";
            SeguridadCboGenero.SelectedIndex = -1;
            SeguridadTxtTelefono.Text = "";
            SeguridadTxtCorreo.Text = "";
        }

        private void SeguridadBtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (SeguridadDgvEmpleados.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una fila del listado para modificar");
                    return;

                }
               
                if (!ValidarFechas()) return;
                _Empleado.IdEmpleado = Convert.ToInt32(SeguridadTxtIdEmpleado.Text);
                _Empleado.CodigoEmpleado = SeguridadTxtCodigo.Text;
                _Empleado.DpiEmpleado = SeguridadTxtDpi.Text;
                _Empleado.NitEmpleado = SeguridadTxtNit.Text;
                _Empleado.NombresEmpleado = SeguridadTxtNombres.Text;
                _Empleado.ApellidosEmpleado = SeguridadTxtApellidos.Text;
                _Empleado.PuestoEmpleado = SeguridadTxtPuesto.Text;
                _Empleado.GeneroEmpleado = SeguridadCboGenero.SelectedItem?.ToString();
                _Empleado.FechaNacimientoEmpleado = SeguridadDtpFechaNacimiento.Value;
                _Empleado.FechaContratacionEmpleado = SeguridadDtpFechaContratacion.Value;
                _Empleado.TelefonoEmpleado = SeguridadTxtTelefono.Text;
                _Empleado.CorreoEmpleado = SeguridadTxtCorreo.Text;
                _Empleado.Estado = EstadoEntidad.Modified;

                bool Valido = new ClsValidacionDatos(_Empleado).SeguridadMetValidar();
                if (Valido)
                {
                    string Resultado = _Empleado.SeguridadMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    SeguridadMetListarEmpleados();
                    SeguridadMetReinicio();
                    SeguridadMetHabilitarCampos(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SeguridadBtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarFechas()) return;
                _Empleado.CodigoEmpleado = SeguridadTxtCodigo.Text;
                _Empleado.DpiEmpleado = SeguridadTxtDpi.Text;
                _Empleado.NitEmpleado = SeguridadTxtNit.Text;
                _Empleado.NombresEmpleado = SeguridadTxtNombres.Text;
                _Empleado.ApellidosEmpleado = SeguridadTxtApellidos.Text;
                _Empleado.PuestoEmpleado = SeguridadTxtPuesto.Text;
                _Empleado.GeneroEmpleado = SeguridadCboGenero.SelectedItem?.ToString();
                _Empleado.FechaNacimientoEmpleado = SeguridadDtpFechaNacimiento.Value;
                _Empleado.FechaContratacionEmpleado = SeguridadDtpFechaContratacion.Value;
                _Empleado.TelefonoEmpleado = SeguridadTxtTelefono.Text;
                _Empleado.CorreoEmpleado = SeguridadTxtCorreo.Text;
                _Empleado.Estado = EstadoEntidad.Added;

                bool Valido = new ClsValidacionDatos(_Empleado).SeguridadMetValidar();
                if (Valido)
                {
                    string Resultado = _Empleado.SeguridadMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    SeguridadMetListarEmpleados();
                    SeguridadMetReinicio();
                    SeguridadMetHabilitarCampos(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SeguridadBtnLimpiar_Click(object sender, EventArgs e)
        {
            SeguridadTxtIdEmpleado.Clear();
            SeguridadMetReinicio();
            SeguridadMetListarEmpleados();
            SeguridadMetHabilitarCampos(false);
        }

        private void SeguridadBtnRefrescar_Click(object sender, EventArgs e)
        {
            SeguridadTxtIdEmpleado.Clear();
            SeguridadMetListarEmpleados();
        }

        private void SeguridadBtnInicio_Click(object sender, EventArgs e)
        {
            if (SeguridadDgvEmpleados.Rows.Count > 0)
            {
                SeguridadDgvEmpleados.ClearSelection();
                SeguridadDgvEmpleados.Rows[0].Selected = true;
                SeguridadDgvEmpleados.CurrentCell = SeguridadDgvEmpleados.Rows[0].Cells[0];
                SeguridadMetCargarFilaEnFormulario(0);
                SeguridadMetActualizarContador();
            }
        }

        private void SeguridadBtnAnterior_Click(object sender, EventArgs e)
        {
            if (SeguridadDgvEmpleados.Rows.Count > 0 && SeguridadDgvEmpleados.CurrentCell != null)
            {
                int FilaActual = SeguridadDgvEmpleados.CurrentCell.RowIndex;
                if (FilaActual > 0)
                {
                    SeguridadDgvEmpleados.ClearSelection();
                    SeguridadDgvEmpleados.Rows[FilaActual - 1].Selected = true;
                    SeguridadDgvEmpleados.CurrentCell = SeguridadDgvEmpleados.Rows[FilaActual - 1].Cells[0];
                    SeguridadMetCargarFilaEnFormulario(FilaActual - 1);
                    SeguridadMetActualizarContador();
                }
            }
        }

        private void SeguridadBtnSiguiente_Click(object sender, EventArgs e)
        {
            if (SeguridadDgvEmpleados.Rows.Count > 0 && SeguridadDgvEmpleados.CurrentCell != null)
            {
                int FilaActual = SeguridadDgvEmpleados.CurrentCell.RowIndex;
                if (FilaActual < SeguridadDgvEmpleados.Rows.Count - 1)
                {
                    SeguridadDgvEmpleados.ClearSelection();
                    SeguridadDgvEmpleados.Rows[FilaActual + 1].Selected = true;
                    SeguridadDgvEmpleados.CurrentCell = SeguridadDgvEmpleados.Rows[FilaActual + 1].Cells[0];
                    SeguridadMetCargarFilaEnFormulario(FilaActual + 1);
                    SeguridadMetActualizarContador();
                }
            }

        }

        private void SeguridadBtnFin_Click(object sender, EventArgs e)
        {
            if (SeguridadDgvEmpleados.Rows.Count > 0)
            {
                int UltimaFila = SeguridadDgvEmpleados.Rows.Count - 1;
                SeguridadDgvEmpleados.ClearSelection();
                SeguridadDgvEmpleados.Rows[UltimaFila].Selected = true;
                SeguridadDgvEmpleados.CurrentCell = SeguridadDgvEmpleados.Rows[UltimaFila].Cells[0];
                SeguridadMetCargarFilaEnFormulario(UltimaFila);
                SeguridadMetActualizarContador();
            }
        }

        private void SeguridadMetCargarFilaEnFormulario(int rowIndex)
        {
            var fila = SeguridadDgvEmpleados.Rows[rowIndex];

            _Empleado.Estado = EstadoEntidad.Modified;
            SeguridadTxtIdEmpleado.Text = fila.Cells[0].Value.ToString();
            SeguridadTxtCodigo.Text = fila.Cells[1].Value.ToString();
            SeguridadTxtDpi.Text = fila.Cells[2].Value.ToString();
            SeguridadTxtNit.Text = fila.Cells[3].Value?.ToString();
            SeguridadTxtNombres.Text = fila.Cells[4].Value.ToString();
            SeguridadTxtApellidos.Text = fila.Cells[5].Value.ToString();
            SeguridadTxtPuesto.Text = fila.Cells[6].Value.ToString();
            SeguridadCboGenero.SelectedItem = fila.Cells[7].Value.ToString();
            SeguridadDtpFechaNacimiento.Value = Convert.ToDateTime(fila.Cells[8].Value);
            SeguridadDtpFechaContratacion.Value = Convert.ToDateTime(fila.Cells[9].Value);
            SeguridadTxtTelefono.Text = fila.Cells[10].Value?.ToString();
            SeguridadTxtCorreo.Text = fila.Cells[11].Value?.ToString();
            SeguridadChkActivo.Checked = Convert.ToBoolean(fila.Cells[12].Value);

            SeguridadMetHabilitarCampos(true);
        }

        private void SeguridadTxtDpi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void SeguridadTxtNit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private bool _formateandoNit = false;

        private void SeguridadTxtNit_TextChanged(object sender, EventArgs e)
        {
            if (_formateandoNit) return;

            _formateandoNit = true;

            string soloDigitos = SeguridadTxtNit.Text.Replace("-", "");

            if (soloDigitos.Length > 8)
                soloDigitos = soloDigitos.Substring(0, 8);

            string textoFormateado = soloDigitos;
            if (soloDigitos.Length == 8)
            {
                textoFormateado = soloDigitos.Substring(0, 7) + "-" + soloDigitos.Substring(7, 1);
            }

            SeguridadTxtNit.Text = textoFormateado;
            SeguridadTxtNit.SelectionStart = SeguridadTxtNit.Text.Length;

            _formateandoNit = false;
        }

        private void SeguridadTxtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (!SeguridadTxtCodigo.Text.StartsWith(PrefijoCodigoEmpleado))
            {
                SeguridadTxtCodigo.Text = PrefijoCodigoEmpleado;
                SeguridadTxtCodigo.SelectionStart = SeguridadTxtCodigo.Text.Length;
            }
        }

        private void SeguridadTxtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                && SeguridadTxtCodigo.SelectionStart <= PrefijoCodigoEmpleado.Length)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private bool ValidarFechas()
        {
            DateTime fechaNacimiento = SeguridadDtpFechaNacimiento.Value;
            DateTime fechaContratacion = SeguridadDtpFechaContratacion.Value;

            int edadEnContratacion = fechaContratacion.Year - fechaNacimiento.Year;

            if (fechaContratacion < fechaNacimiento.AddYears(edadEnContratacion))
            {
                edadEnContratacion--;
            }

            if (edadEnContratacion < 18)
            {
                MessageBox.Show("El empleado debe tener al menos 18 años cumplidos a la fecha de contratación");
                return false;
            }

            return true;
        }

        private void SeguridadBtnCancelar_Click(object sender, EventArgs e)
        {
            SeguridadTxtIdEmpleado.Clear();
            SeguridadMetReinicio();
            SeguridadMetListarEmpleados();
            SeguridadMetHabilitarCampos(false);
        }

        private void SeguridadTxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
                return;

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SeguridadTxtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void SeguridadBtnImprimir_Click(object sender, EventArgs e)
        {
            FrmReporteMantenimientoEmpleado reporte = new FrmReporteMantenimientoEmpleado();
            reporte.Show();
        }
    }
}