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
        //cambiar nombre de la tabala a la que se desea hacer el CRUD
        string nombreTabla = "tbl_empleados";
        Controlador controlador = new Controlador();
<<<<<<< HEAD
        string modo = ""; // "INSERT" o "UPDATE"
=======
        string modo = ""; // "INSERT", "UPDATE" o "CONSULTA"

        private DataTable esquemaCampos;
        private List<string> columnasPK = new List<string>();
        private readonly Dictionary<string, Control> controlesCampos = new Dictionary<string, Control>();
        private readonly Dictionary<string, string> respaldo = new Dictionary<string, string>();
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)

        public Frm_Crud()
        {
            InitializeComponent();
            Btn_ingresar.Click += Btn_ingresar_Click;
            Btn_refrescar.Click += Btn_modificar_Click;
            Btn_cancelar.Click += Btn_cancelar_Click;
            Dgv_datos.ReadOnly = true;
<<<<<<< HEAD
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
            try
            {
                DataTable dt = controlador.llenarDgv("tbl_empleados");
                Dgv_datos.DataSource = dt;
=======
            Btn_eliminar.Click += Btn_eliminar_Click;
            Dgv_datos.CellClick += Dgv_datos_CellClick;
            Load += Frm_Crud_Load;
        }

        private void Frm_Crud_Load(object sender, EventArgs e)
        {
            try
            {
                ConstruirCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo leer el esquema de la tabla:\n\n" + ex.Message,
                    "Esquema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            TomarRespaldo();
            AplicarModo("");
        }

        private void ConstruirCampos()
        {
            Pnl_campos.Controls.Clear();
            controlesCampos.Clear();

            esquemaCampos = controlador.ObtenerEsquemaCampos(nombreTabla);
            columnasPK = controlador.ObtenerLlavePrimaria(nombreTabla);

            foreach (DataRow columna in esquemaCampos.Rows)
            {
                string nombreColumna = columna["COLUMN_NAME"].ToString();

                Label etiqueta = new Label();
                etiqueta.Text = nombreColumna + (EsLlave(nombreColumna) ? " (PK)" : "");
                etiqueta.AutoSize = false;
                etiqueta.Height = 20;
                etiqueta.Dock = DockStyle.Top;

                Control control = CrearControl(nombreColumna);
                control.Name = "Campo_" + nombreColumna;
                control.Dock = DockStyle.Top;
                control.Enabled = false;

                Panel celda = new Panel();
                celda.Width = 180;
                celda.Height = 52;
                celda.Margin = new Padding(4);
                celda.Controls.Add(control);
                celda.Controls.Add(etiqueta);

                Pnl_campos.Controls.Add(celda);
                controlesCampos.Add(nombreColumna, control);
            }
        }

        private DataRow FilaEsquema(string nombreColumna)
        {
            if (esquemaCampos == null) return null;

            foreach (DataRow fila in esquemaCampos.Rows)
            {
                if (string.Equals(fila["COLUMN_NAME"].ToString(), nombreColumna,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return fila;
                }
            }

            return null;
        }

        private bool EsBooleano(string nombreColumna)
        {
            DataRow fila = FilaEsquema(nombreColumna);
            if (fila == null) return false;

            string tipo = fila["DATA_TYPE"].ToString().ToLower();
            string tipoCompleto = fila["COLUMN_TYPE"].ToString().ToLower();

            if (tipo == "bit" || tipo == "bool" || tipo == "boolean") return true;

            return tipoCompleto.StartsWith("tinyint(1)");
        }

        private bool EsNumerico(string nombreColumna)
        {
            DataRow fila = FilaEsquema(nombreColumna);
            if (fila == null) return false;

            switch (fila["DATA_TYPE"].ToString().ToLower())
            {
                case "int":
                case "integer":
                case "bigint":
                case "smallint":
                case "mediumint":
                case "tinyint":
                case "decimal":
                case "numeric":
                case "float":
                case "double":
                    return true;
                default:
                    return false;
            }
        }

        private bool EsLlave(string nombreColumna)
        {
            foreach (string pk in columnasPK)
            {
                if (string.Equals(pk, nombreColumna, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private Control CrearControl(string nombreColumna)
        {
            if (!EsBooleano(nombreColumna))
            {
                return new TextBox();
            }

            CheckBox casilla = new CheckBox();
            casilla.AutoSize = false;
            casilla.Height = 26;
            casilla.ThreeState = true;
            casilla.CheckState = CheckState.Indeterminate;
            casilla.TextAlign = ContentAlignment.MiddleCenter;

            if (EsLlave(nombreColumna))
            {
                casilla.Appearance = Appearance.Normal;
            }
            else
            {
                casilla.Appearance = Appearance.Button;
                casilla.FlatStyle = FlatStyle.Standard;
            }

            casilla.CheckStateChanged += CampoBooleano_CheckStateChanged;
            ActualizarTextoCasilla(casilla);

            return casilla;
        }

        private void CampoBooleano_CheckStateChanged(object sender, EventArgs e)
        {
            ActualizarTextoCasilla(sender as CheckBox);
        }

        private void ActualizarTextoCasilla(CheckBox casilla)
        {
            if (casilla == null) return;

            if (casilla.CheckState == CheckState.Indeterminate) casilla.Text = "(sin valor)";
            else if (casilla.CheckState == CheckState.Checked) casilla.Text = "Si";
            else casilla.Text = "No";
        }

        private string LeerCampo(Control control)
        {
            CheckBox casilla = control as CheckBox;

            if (casilla != null)
            {
                if (casilla.CheckState == CheckState.Indeterminate) return "";
                return casilla.CheckState == CheckState.Checked ? "1" : "0";
            }

            return control.Text.Trim();
        }

        private void EscribirCampo(Control control, string texto)
        {
            CheckBox casilla = control as CheckBox;

            if (casilla != null)
            {
                if (texto == "1") casilla.CheckState = CheckState.Checked;
                else if (texto == "0") casilla.CheckState = CheckState.Unchecked;
                else casilla.CheckState = CheckState.Indeterminate;
                return;
            }

            control.Text = texto;
        }

        private string TextoDesdeValor(string nombreColumna, object valor)
        {
            if (valor == null || valor == DBNull.Value) return "";

            if (EsBooleano(nombreColumna))
            {
                try
                {
                    return Convert.ToBoolean(valor) ? "1" : "0";
                }
                catch (Exception)
                {
                    return "";
                }
            }

            if (valor is DateTime)
            {
                return ((DateTime)valor).ToString("yyyy-MM-dd");
            }

            return valor.ToString();
        }

        private void AplicarModo(string nuevoModo)
        {
            modo = nuevoModo;

            bool consultando = modo == "CONSULTA";

            foreach (KeyValuePair<string, Control> par in controlesCampos)
            {
                par.Value.Enabled = consultando;

                TextBox caja = par.Value as TextBox;
                if (caja != null)
                {
                    caja.BackColor = consultando ? SystemColors.Window : SystemColors.Control;
                }
            }

            Btn_ingresar.Enabled = modo == "";
            Btn_modificar.Enabled = modo == "";
            Btn_cancelar.Enabled = modo != "";
            Btn_eliminar.Enabled = modo == "" || modo == "CONSULTA";

            Lbl_modo.Text = "Modo: " + (modo == "" ? "Inicial" : modo);
        }

        private void LimpiarCampos()
        {
            foreach (KeyValuePair<string, Control> par in controlesCampos)
            {
                EscribirCampo(par.Value, "");
            }
        }

        private void TomarRespaldo()
        {
            respaldo.Clear();

            foreach (KeyValuePair<string, Control> par in controlesCampos)
            {
                respaldo[par.Key] = LeerCampo(par.Value);
            }
        }

        private void RestaurarRespaldo()
        {
            foreach (KeyValuePair<string, Control> par in controlesCampos)
            {
                string valor;
                if (!respaldo.TryGetValue(par.Key, out valor)) valor = "";
                EscribirCampo(par.Value, valor);
            }
        }

        private bool HayCambios()
        {
            foreach (KeyValuePair<string, Control> par in controlesCampos)
            {
                string valor;
                if (!respaldo.TryGetValue(par.Key, out valor)) valor = "";
                if (LeerCampo(par.Value) != valor) return true;
            }

            return false;
        }

        private bool ConvertirValor(string nombreColumna, string texto, out object valor)
        {
            valor = texto;

            try
            {
                if (EsBooleano(nombreColumna))
                {
                    if (texto == "1") { valor = true; return true; }
                    if (texto == "0") { valor = false; return true; }
                    return false;
                }

                if (EsNumerico(nombreColumna))
                {
                    valor = Convert.ToDecimal(texto, System.Globalization.CultureInfo.CurrentCulture);
                    return true;
                }

                return true;
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EjecutarConsulta()
        {
<<<<<<< HEAD
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
        private void Btn_refrescar_Click(object sender, EventArgs e)
        {

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
=======
            List<Criterio> criterios = new List<Criterio>();

            foreach (KeyValuePair<string, Control> par in controlesCampos)
            {
                string texto = LeerCampo(par.Value);
                if (texto.Length == 0) continue;

                Criterio criterio = new Criterio();
                criterio.Columna = par.Key;

                if (!EsBooleano(par.Key) && !EsNumerico(par.Key))
                {
                    criterio.UsarLike = true;
                    criterio.Valor = "%" + texto + "%";
                }
                else
                {
                    object valor;
                    if (!ConvertirValor(par.Key, texto, out valor))
                    {
                        MessageBox.Show("El valor de '" + par.Key + "' no es valido.",
                            "Consultar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        par.Value.Focus();
                        return;
                    }

                    criterio.UsarLike = false;
                    criterio.Valor = valor;
                }

                criterios.Add(criterio);
            }

            try
            {
                DataTable dt = controlador.Consultar(nombreTabla, criterios);
                Dgv_datos.DataSource = dt;
                Dgv_datos.ReadOnly = true;
                Lbl_estado.Text = dt.Rows.Count + " registro(s) con " + criterios.Count + " criterio(s)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Consulta",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_eliminar_Click(object sender, EventArgs e)
        {
            if (modo == "INSERT" || modo == "UPDATE")
            {
                MessageBox.Show("Termine o cancele la edicion antes de eliminar.",
                    "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (columnasPK.Count == 0)
            {
                MessageBox.Show("No se pudo determinar la llave primaria de la tabla.",
                    "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<Criterio> llave = new List<Criterio>();
            string detalle = "";

            foreach (string pk in columnasPK)
            {
                Control control;
                if (!controlesCampos.TryGetValue(pk, out control))
                {
                    MessageBox.Show("La llave primaria '" + pk + "' no aparece entre los campos.",
                        "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string texto = LeerCampo(control);

                if (texto.Length == 0)
                {
                    MessageBox.Show("Seleccione un registro en la tabla o indique la llave primaria '" +
                        pk + "' antes de eliminar.",
                        "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                object valor;
                if (!ConvertirValor(pk, texto, out valor))
                {
                    MessageBox.Show("El valor de '" + pk + "' no es valido.",
                        "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    control.Focus();
                    return;
                }

                Criterio criterio = new Criterio();
                criterio.Columna = pk;
                criterio.Valor = valor;
                criterio.UsarLike = false;
                llave.Add(criterio);

                detalle += pk + " = " + texto + Environment.NewLine;
            }

            DialogResult confirmacion = MessageBox.Show(
                "Se eliminara el registro con:" + Environment.NewLine +
                detalle + Environment.NewLine +
                "Esta accion no se puede deshacer. Confirma la eliminacion?",
                "Confirmar eliminacion", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirmacion != DialogResult.Yes)
            {
                Lbl_estado.Text = "Eliminacion cancelada";
                return;
            }

            try
            {
                int filas = controlador.EliminarRegistro(nombreTabla, llave);

                if (filas > 0)
                {
                    MessageBox.Show(filas + " registro(s) eliminado(s).",
                        "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarCampos();
                    TomarRespaldo();
                    actualizarDataGridView();
                    AplicarModo("");
                }
                else
                {
                    MessageBox.Show("No se elimino ningun registro. Verifique la llave primaria.",
                        "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dgv_datos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (modo == "INSERT" || modo == "UPDATE") return;

            DataGridViewRow fila = Dgv_datos.Rows[e.RowIndex];

            foreach (KeyValuePair<string, Control> par in controlesCampos)
            {
                if (!Dgv_datos.Columns.Contains(par.Key)) continue;

                object valor = fila.Cells[par.Key].Value;
                EscribirCampo(par.Value, TextoDesdeValor(par.Key, valor));
            }

            TomarRespaldo();
        }

        public void actualizarDataGridView()
        {
            DataTable dtVista = controlador.llenarDgv(nombreTabla);
            Dgv_datos.DataSource = dtVista;
            Dgv_datos.ReadOnly = true;
            Lbl_estado.Text = dtVista.Rows.Count + " registro(s)";
            AplicarModo("");
        }

        private void Btn_Consultar_Click(object sender, EventArgs e)
        {
            if (modo == "INSERT" || modo == "UPDATE")
            {
                MessageBox.Show("Termine o cancele la edicion antes de consultar.",
                    "Consultar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (modo == "")
            {
                LimpiarCampos();
                TomarRespaldo();
                AplicarModo("CONSULTA");
            }

            EjecutarConsulta();
        }

        private void Btn_ingresar_Click(object sender, EventArgs e)
        {
            AplicarModo("INSERT");
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
            AplicarModo("UPDATE");
            Dgv_datos.ReadOnly = false;
            MessageBox.Show("Modo Modificar activado. Edite el registro en el grid y presione Guardar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void Btn_refrescar_Click(object sender, EventArgs e)
        {

        }
        private void Btn_cancelar_Click(object sender, EventArgs e)
        {
            if (modo == "") return;

            if (HayCambios())
            {
                DialogResult respuesta = MessageBox.Show(
                    "Se descartaran los cambios no guardados. Desea continuar?",
                    "Cancelar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (respuesta != DialogResult.Yes) return;
            }

            RestaurarRespaldo();
            actualizarDataGridView();
            Lbl_estado.Text = "Cambios descartados";
        }

        private void Btn_guardar_Click(object sender, EventArgs e)
        {
            try
            {
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)
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

                        // Omitir el id_empleado (columna 0) para que MySQL lo genere automáticamente
                        if (colIndex == 0) continue;

                        if (colIndex < row.Cells.Count && row.Cells[colIndex].Value != null)
                        {
                            columnas.Add(col.ColumnName);
                            valores.Add("'" + FormatearValorParaSql(row.Cells[colIndex].Value, col.DataType) + "'");
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
                            string val = FormatearValorParaSql(row.Cells[colIndex].Value, col.DataType);
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
        private string FormatearValorParaSql(object cellValue, Type columnType)
        {
            if (cellValue == null || cellValue == DBNull.Value) return "";

            if (columnType == typeof(DateTime) || DateTime.TryParse(cellValue.ToString(), out _))
            {
                if (DateTime.TryParse(cellValue.ToString(), out DateTime fecha))
                {
                    return fecha.ToString("yyyy-MM-dd");
                }
            }

            return cellValue.ToString().Replace("'", "''");
        }

      




        public List<string> ValidarRegistroConEsquema(Dictionary<string, string> datos)
        {
            try
            {
                return controlador.ValidarRegistro(datos, nombreTabla);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al validar los datos:\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return new List<string>();
            }
        }

        public DataTable ObtenerEsquemaTabla()
        {
            try
            {
                return controlador.ObtenerEsquemaTabla(nombreTabla);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al obtener el esquema de la tabla:\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return null;
            }
        }
    }
}
