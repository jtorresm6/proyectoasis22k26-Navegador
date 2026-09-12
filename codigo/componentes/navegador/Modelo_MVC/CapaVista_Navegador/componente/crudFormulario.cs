using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    // Arma el panel dinamico de un registro: labels, textbox, combo, fecha, checkbox
    public class crudFormulario
    {
        private Form formulario;
        private Panel panelRegistro;
        private Dictionary<string, Control> controles;
        private List<columnaInfo> esquema;
        private string tabla;
        private bool modoModificar;

        private ctrlTabla ctrlTabla = new ctrlTabla();
        private ctrlEsquema ctrlEsquema = new ctrlEsquema();

        public bool Visible
        {
            get { return panelRegistro != null && panelRegistro.Visible; }
        }

        public bool ModoModificar
        {
            get { return modoModificar; }
        }

        public int Bottom
        {
            get { return panelRegistro != null ? panelRegistro.Bottom : 0; }
        }

        public crudFormulario(Form formulario)
        {
            this.formulario = formulario;
        }

        public void Abrir(string tabla, List<columnaInfo> esquema, bool modificar, DataGridViewRow fila, crudGrid grid, int posicionY)
        {
            Cerrar();

            this.tabla = tabla;
            this.esquema = esquema;
            this.modoModificar = modificar;

            panelRegistro = new Panel();
            panelRegistro.Name = "panelRegistro";
            panelRegistro.Location = new Point(10, posicionY);
            panelRegistro.Width = formulario.ClientSize.Width - 20;

            int altura = 50 + esquema.Count * 42;
            if (altura < 150) altura = 150;
            if (altura > 400) altura = 400;
            panelRegistro.Height = altura;

            panelRegistro.BackColor = Color.Beige;
            panelRegistro.BorderStyle = BorderStyle.FixedSingle;
            panelRegistro.AutoScroll = true;

            formulario.Controls.Add(panelRegistro);

            controles = new Dictionary<string, Control>();

            Label titulo = new Label();
            titulo.Text = (modificar ? "Modificar registro - " : "Nuevo registro - ") + tabla;
            titulo.Font = new Font(formulario.Font.FontFamily, 10, FontStyle.Bold);
            titulo.AutoSize = true;
            titulo.Location = new Point(10, 8);
            panelRegistro.Controls.Add(titulo);

            int posicionYCampo = 34;

            foreach (columnaInfo col in esquema)
            {
                Control control = CrearControlColumna(col, modificar, fila, grid, posicionYCampo);

                Label etiqueta = new Label();
                etiqueta.Text = col.Nombre + (col.EsPK ? " [PK]" : "") + (col.EsFK ? " [FK]" : "");
                etiqueta.Location = new Point(15, posicionYCampo + 4);
                etiqueta.AutoSize = true;

                if (col.EsPK)
                {
                    etiqueta.Font = new Font(etiqueta.Font, FontStyle.Bold);
                    etiqueta.ForeColor = Color.DarkRed;
                }
                else if (col.EsFK)
                {
                    etiqueta.Font = new Font(etiqueta.Font, FontStyle.Bold);
                    etiqueta.ForeColor = Color.DarkBlue;
                }

                panelRegistro.Controls.Add(etiqueta);
                panelRegistro.Controls.Add(control);

                controles[col.Nombre] = control;
                posicionYCampo += 42;
            }

            panelRegistro.Visible = true;
            panelRegistro.BringToFront();
        }

        // Decide que control dibujar segun el tipo de columna
        private Control CrearControlColumna(columnaInfo col, bool modificar, DataGridViewRow fila, crudGrid grid, int posicionY)
        {
            if (col.EsFK && !string.IsNullOrWhiteSpace(col.TablaFK) && !string.IsNullOrWhiteSpace(col.ColumnaFK))
            {
                ComboBox combo = CrearComboFk(col, fila, grid);
                combo.Location = new Point(190, posicionY);
                combo.Width = 250;
                combo.Enabled = !(col.EsPK && modificar);
                return combo;
            }

            if (tipoColumna.EsFecha(col))
            {
                DateTimePicker fecha = new DateTimePicker();
                fecha.Location = new Point(190, posicionY);
                fecha.Width = 250;
                fecha.Format = DateTimePickerFormat.Short;
                fecha.Value = ObtenerFechaInicial(fila, col.Nombre, grid);
                fecha.Enabled = !(col.EsPK && modificar);
                return fecha;
            }

            if (tipoColumna.EsBooleano(col))
            {
                CheckBox chk = new CheckBox();
                chk.Text = "Sí (marcado) / No (desmarcado)";
                chk.AutoSize = true;
                chk.Location = new Point(190, posicionY + 3);
                chk.Checked = ObtenerBooleanoInicial(fila, col.Nombre, grid);
                chk.Enabled = !(col.EsPK && modificar);
                return chk;
            }

            TextBox caja = new TextBox();
            caja.Location = new Point(190, posicionY);
            caja.Width = 250;
            caja.Text = fila != null ? grid.ObtenerValor(fila, col.Nombre) : "";

            // Autogeneracion de la llave primaria: MAX + 1, para no depender del motor de BD
            if (!modificar && col.EsAutoincremento)
            {
                caja.Text = "(automático)";
                caja.ReadOnly = true;
                caja.BackColor = Color.LightGray;
            }
            else if (!modificar && col.EsPK && tipoColumna.EsNumerico(col))
            {
                object siguiente = null;

                try { siguiente = ctrlEsquema.ObtenerSiguienteValorLlave(tabla, col.Nombre); }
                catch { }

                caja.Text = siguiente != null ? Convert.ToString(siguiente) : "";

                if (siguiente != null)
                {
                    caja.ReadOnly = true;
                    caja.BackColor = Color.LightGray;
                }
            }

            if (col.EsPK && modificar)
            {
                caja.ReadOnly = true;
                caja.BackColor = Color.LightGray;
            }

            return caja;
        }

        private ComboBox CrearComboFk(columnaInfo col, DataGridViewRow fila, crudGrid grid)
        {
            ComboBox combo = new ComboBox();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                DataTable opciones = ctrlTabla.llenarDgv(col.TablaFK);
                List<columnaInfo> esquemaFk = ctrlEsquema.ObtenerEsquemaTabla(col.TablaFK);

                string columnaMostrar = col.ColumnaFK;

                foreach (columnaInfo c in esquemaFk)
                {
                    if (string.Equals(c.Nombre, col.ColumnaFK, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string tipo = (c.TipoDato ?? "").ToLowerInvariant();

                    if (tipo.Contains("char") || tipo.Contains("text"))
                    {
                        columnaMostrar = c.Nombre;
                        break;
                    }
                }

                if (!opciones.Columns.Contains(col.ColumnaFK))
                    return combo;

                if (!opciones.Columns.Contains(columnaMostrar))
                    columnaMostrar = col.ColumnaFK;

                combo.DataSource = opciones;
                combo.ValueMember = col.ColumnaFK;
                combo.DisplayMember = columnaMostrar;
                combo.SelectedIndex = -1;

                if (fila != null)
                {
                    string valor = grid.ObtenerValor(fila, col.Nombre);

                    for (int i = 0; i < combo.Items.Count; i++)
                    {
                        DataRowView item = combo.Items[i] as DataRowView;
                        if (item == null) continue;

                        if (string.Equals(Convert.ToString(item.Row[col.ColumnaFK]), valor, StringComparison.OrdinalIgnoreCase))
                        {
                            combo.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las opciones de '" + col.Nombre + "'.\n\n" + ex.Message,
                    "Error al cargar opciones",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return combo;
        }

        private DateTime ObtenerFechaInicial(DataGridViewRow fila, string campo, crudGrid grid)
        {
            if (fila == null) return DateTime.Today;

            DateTime fecha;
            return DateTime.TryParse(grid.ObtenerValor(fila, campo), out fecha) ? fecha : DateTime.Today;
        }

        private bool ObtenerBooleanoInicial(DataGridViewRow fila, string campo, crudGrid grid)
        {
            if (fila == null) return false;

            string valor = grid.ObtenerValor(fila, campo).ToLowerInvariant();
            return valor == "1" || valor == "true" || valor == "yes" || valor == "si";
        }

        public Dictionary<string, string> ObtenerDatos()
        {
            Dictionary<string, string> datos = new Dictionary<string, string>();

            if (controles == null)
                return datos;

            foreach (KeyValuePair<string, Control> par in controles)
                datos[par.Key] = ObtenerValorControl(par.Value);

            return datos;
        }

        private string ObtenerValorControl(Control control)
        {
            DateTimePicker fecha = control as DateTimePicker;
            if (fecha != null) return fecha.Value.ToString("yyyy-MM-dd");

            ComboBox combo = control as ComboBox;
            if (combo != null) return combo.SelectedValue == null ? "" : Convert.ToString(combo.SelectedValue);

            CheckBox check = control as CheckBox;
            if (check != null) return check.Checked ? "1" : "0";

            return control.Text.Trim();
        }

        public void Cerrar()
        {
            if (panelRegistro != null)
            {
                formulario.Controls.Remove(panelRegistro);
                panelRegistro.Dispose();
                panelRegistro = null;
            }

            controles = null;
        }
    }
}