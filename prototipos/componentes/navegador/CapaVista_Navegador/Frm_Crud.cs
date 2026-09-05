using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_Navegador;

namespace CapaVista_Navegador
{
    public partial class Frm_Crud : Form
    {
        string nombreTabla = "tbl_empleados";
        Controlador controlador = new Controlador();
        string modo = ""; // "INSERT" o "UPDATE"

        public Frm_Crud()
        {
            InitializeComponent();
            Btn_ingresar.Click += Btn_ingresar_Click;
            Btn_modificar.Click += Btn_modificar_Click;
            Btn_cancelar.Click += Btn_cancelar_Click;
            Dgv_datos.ReadOnly = true;
        }

        public void actualizarDataGridView()
        {
            DataTable dtVista = controlador.llenarDgv(nombreTabla);
            Dgv_datos.DataSource = dtVista;
            Dgv_datos.ReadOnly = true;
            modo = "";
        }

        private void Btn_Consultar_Click(object sender, EventArgs e)
        {
            actualizarDataGridView();
        }

        private void Btn_ingresar_Click(object sender, EventArgs e)
        {
            modo = "INSERT";
            Dgv_datos.ReadOnly = false;
            MessageBox.Show("Modo Ingresar activado. Ingrese los datos en la nueva fila del grid y presione Guardar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Btn_modificar_Click(object sender, EventArgs e)
        {
            if (Dgv_datos.CurrentRow == null || Dgv_datos.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Seleccione un registro válido para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            modo = "UPDATE";
            Dgv_datos.ReadOnly = false;
            MessageBox.Show("Modo Modificar activado. Edite el registro en el grid y presione Guardar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Btn_cancelar_Click(object sender, EventArgs e)
        {
            actualizarDataGridView();
            MessageBox.Show("Operación cancelada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Btn_guardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(modo))
                {
                    MessageBox.Show("No hay ningún modo activo (Ingresar o Modificar).", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (Dgv_datos.CurrentRow == null)
                {
                    MessageBox.Show("No hay ningún registro seleccionado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow row = Dgv_datos.CurrentRow;
                if (row.IsNewRow && modo == "UPDATE")
                {
                    MessageBox.Show("Seleccione una fila existente para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validación antes de enviar
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        if (!row.IsNewRow)
                        {
                            MessageBox.Show("Existen campos vacíos. Por favor complete todos los datos antes de guardar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                DataTable dt = (DataTable)Dgv_datos.DataSource;
                if (dt == null)
                {
                    MessageBox.Show("El origen de datos no está disponible.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string sql = "";

                if (modo == "INSERT")
                {
                    List<string> columnas = new List<string>();
                    List<string> valores = new List<string>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        int colIndex = dt.Columns.IndexOf(col);
                        if (colIndex < row.Cells.Count && row.Cells[colIndex].Value != null)
                        {
                            columnas.Add(col.ColumnName);
                            valores.Add("'" + row.Cells[colIndex].Value.ToString().Replace("'", "''") + "'");
                        }
                    }

                    if (columnas.Count == 0)
                    {
                        MessageBox.Show("No hay datos para insertar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sql = $"INSERT INTO {nombreTabla} ({string.Join(", ", columnas)}) VALUES ({string.Join(", ", valores)});";
                }
                else if (modo == "UPDATE")
                {
                    string primaryKeyCol = dt.Columns[0].ColumnName;
                    string primaryKeyValue = row.Cells[0].Value?.ToString() ?? "";

                    if (string.IsNullOrEmpty(primaryKeyValue))
                    {
                        MessageBox.Show("El identificador del registro (clave primaria) no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    List<string> setClauses = new List<string>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        int colIndex = dt.Columns.IndexOf(col);
                        if (colIndex < row.Cells.Count && row.Cells[colIndex].Value != null)
                        {
                            string val = row.Cells[colIndex].Value.ToString().Replace("'", "''");
                            setClauses.Add($"{col.ColumnName} = '{val}'");
                        }
                    }

                    sql = $"UPDATE {nombreTabla} SET {string.Join(", ", setClauses)} WHERE {primaryKeyCol} = '{primaryKeyValue}';";
                }

                // Enviar al controlador
                controlador.guardarDatos(sql);
                MessageBox.Show("¡Registro guardado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refrescar el grid al terminar
                actualizarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el registro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
