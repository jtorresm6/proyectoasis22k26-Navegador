//Aca comienza mi codigo
//Donald Estuardo Osorio Pérez 
//Carnet: 0901-23-17982
//18/09/2026
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaModelo_Navegador;

namespace CapaVista_Navegador
{
    // Se encarga del DataGridView: crearlo, mostrarlo, moverse entre filas y sincronizar los controles
    public class ClsCrudGrid
    {
        // Guardamos el formulario donde se va a dibujar la tabla
        private Control _Formulario;

        // Lista de controles del formulario mapeados con sus nombres de campo/columna
        private Dictionary<string, Control> _MapaControles;

        // Evento opcional para notificar la selección de fila hacia afuera si se requiere
        public event EventHandler<DataGridViewRow> AlSeleccionarFila;

        // Propiedad para acceder a la tabla desde fuera si hace falta
        public DataGridView NavegadorDgvDatos { get; private set; }

        public ClsCrudGrid(Control Formulario)
        {
            this._Formulario = Formulario;
            this._MapaControles = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);
        }

        // Permite vincular un control (TextBox, Label, ComboBox, etc.) con el nombre del campo en la BD
        public void NavegadorMetRegistrarControl(string NombreCampo, Control ControlFormulario)
        {
            if (!string.IsNullOrEmpty(NombreCampo) && ControlFormulario != null)
            {
                _MapaControles[NombreCampo] = ControlFormulario;
            }
        }

        // Llena la tabla con los datos que vienen del DataTable y la hace visible
        public void NavegadorMetMostrar(DataTable Datos)
        {
            // Si la tabla no existe en el form, la creamos
            if (NavegadorDgvDatos == null)
                NavegadorMetCrearGrid();

            NavegadorDgvDatos.DataSource = Datos;
            NavegadorDgvDatos.Visible = true;
            NavegadorDgvDatos.ReadOnly = true;

            // Bloqueamos las columnas para que el usuario no edite nada directo
            foreach (DataGridViewColumn Columna in NavegadorDgvDatos.Columns)
                Columna.ReadOnly = true;

            // Si hay datos, seleccionamos la primera fila y poblamos los controles
            if (NavegadorDgvDatos.Rows.Count > 0)
            {
                NavegadorMetSeleccionar(0);
            }
        }

        // Oculta la tabla si está creada
        public void NavegadorMetOcultar()
        {
            if (NavegadorDgvDatos != null)
                NavegadorDgvDatos.Visible = false;
        }

        // Instancia el DataGridView y le da las propiedades iniciales del diseño
        private void NavegadorMetCrearGrid()
        {
            NavegadorDgvDatos = new DataGridView();
            NavegadorDgvDatos.Name = "NavegadorDgvDatos";
            NavegadorDgvDatos.AllowUserToAddRows = false;
            NavegadorDgvDatos.AllowUserToDeleteRows = false;
            NavegadorDgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            NavegadorDgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            NavegadorDgvDatos.MultiSelect = false;
            NavegadorDgvDatos.ReadOnly = true;
            NavegadorDgvDatos.BackgroundColor = Color.White;
            NavegadorDgvDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Suscripción al evento CellClick para actualizar datos al hacer clic en una fila
            NavegadorDgvDatos.CellClick += NavegadorDgvDatos_CellClick;

            // Lo pegamos al formulario que recibimos en el constructor
            _Formulario.Controls.Add(NavegadorDgvDatos);
        }

        // Manejador del evento CellClick al hacer clic directamente en la tabla
        private void NavegadorDgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < NavegadorDgvDatos.Rows.Count)
            {
                NavegadorMetProcesarSeleccionFila(NavegadorDgvDatos.Rows[e.RowIndex]);
            }
        }

        // Procesa la fila activa: llena controles (TextBox, Label, ComboBox, CheckBox) y dispara eventos
        private void NavegadorMetProcesarSeleccionFila(DataGridViewRow Fila)
        {
            if (Fila == null) return;

            // Actualizamos dinámicamente cada control registrado en el diccionario
            foreach (KeyValuePair<string, Control> Par in _MapaControles)
            {
                string NombreCampo = Par.Key;
                Control ControlForm = Par.Value;
                string Valor = NavegadorFuncObtenerValor(Fila, NombreCampo);

                NavegadorMetAsignarValorAControl(ControlForm, Valor);
            }

            // Notificamos si existe algún suscriptor externo
            AlSeleccionarFila?.Invoke(this, Fila);
        }

        // Asigna el valor leído de la celda al tipo de control correspondiente
        private void NavegadorMetAsignarValorAControl(Control ControlForm, string Valor)
        {
            if (ControlForm == null) return;

            if (ControlForm is TextBox txt)
            {
                txt.Text = Valor;
            }
            else if (ControlForm is Label lbl)
            {
                lbl.Text = Valor;
            }
            else if (ControlForm is ComboBox cbo)
            {
                cbo.Text = Valor;
                if (cbo.SelectedIndex == -1 && cbo.Items.Count > 0)
                {
                    cbo.SelectedValue = Valor;
                }
            }
            else if (ControlForm is CheckBox chk)
            {
                chk.Checked = Valor == "1" || Valor.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            else if (ControlForm is DateTimePicker dtp)
            {
                if (DateTime.TryParse(Valor, out DateTime Fecha))
                    dtp.Value = Fecha;
            }
        }

        // Acomoda la posición y el tamaño de la tabla según el espacio disponible en pantalla
        public void NavegadorMetPosicionar(int PosicionY)
        {
            if (NavegadorDgvDatos == null || !NavegadorDgvDatos.Visible)
                return;

            int Margen = 10;

            NavegadorDgvDatos.Location = new Point(Margen, PosicionY);

            // Ajustamos el ancho y alto dinámicamente según la ventana
            NavegadorDgvDatos.Size = new Size(
                Math.Max(100, _Formulario.ClientSize.Width - (Margen * 2)),
                Math.Max(100, _Formulario.ClientSize.Height - PosicionY - Margen));

            NavegadorDgvDatos.BringToFront();
        }

        // Busca en qué posición de la tabla está una columna por su nombre
        public int NavegadorFuncObtenerIndiceColumna(string NombreCampo)
        {
            if (NavegadorDgvDatos == null)
                return -1;

            foreach (DataGridViewColumn Columna in NavegadorDgvDatos.Columns)
            {
                // Revisamos si coincide con el nombre del mapeo
                if (string.Equals(Columna.DataPropertyName, NombreCampo, StringComparison.OrdinalIgnoreCase))
                    return Columna.Index;

                // O si coincide con el nombre directo del control
                if (string.Equals(Columna.Name, NombreCampo, StringComparison.OrdinalIgnoreCase))
                    return Columna.Index;
            }

            return -1; // Si no la encuentra devuelve -1
        }

        // Saca el texto de una celda en específico recibiendo la fila y el nombre de la columna
        public string NavegadorFuncObtenerValor(DataGridViewRow Fila, string Campo)
        {
            int Indice = NavegadorFuncObtenerIndiceColumna(Campo);

            if (Indice < 0 || Fila == null)
                return "";

            object Valor = Fila.Cells[Indice].Value;
            // Validamos que no venga nulo ni con vacíos de BD
            return Valor == null || Valor == DBNull.Value ? "" : Convert.ToString(Valor);
        }

        // Lee de la fila seleccionada solo las columnas marcadas como PK en el esquema
        public Dictionary<string, string> NavegadorFuncObtenerClavesPrimarias(List<ClsColumnaInfo> Esquema, DataGridViewRow Fila)
        {
            Dictionary<string, string> Resultado = new Dictionary<string, string>();

            if (Esquema == null || Fila == null)
                return Resultado;

            // Recorremos las columnas del esquema y armamos un mapa con las que son Llave Primaria
            foreach (ClsColumnaInfo Columna in Esquema)
            {
                if (Columna.EsPK)
                    Resultado[Columna.Nombre] = NavegadorFuncObtenerValor(Fila, Columna.Nombre);
            }

            return Resultado;
        }

        // Selecciona la primera fila de la tabla
        public void NavegadorMetInicio()
        {
            NavegadorMetSeleccionar(0);
        }

        // Sube una fila en la selección si no estamos al principio
        public void NavegadorMetAnterior()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0)
                return;

            NavegadorMetSeleccionar(Math.Max(0, NavegadorFuncIndiceActual() - 1));
        }

        // Baja una fila en la selección si no llegamos al final
        public void NavegadorMetSiguiente()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0)
                return;

            NavegadorMetSeleccionar(
                Math.Min(NavegadorDgvDatos.Rows.Count - 1, NavegadorFuncIndiceActual() + 1));
        }

        // Selecciona la última fila disponible
        public void NavegadorMetFin()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0)
                return;

            NavegadorMetSeleccionar(NavegadorDgvDatos.Rows.Count - 1);
        }

        // Devuelve el número de fila donde está parado el usuario actualmente
        private int NavegadorFuncIndiceActual()
        {
            return NavegadorDgvDatos != null && NavegadorDgvDatos.CurrentRow != null
                ? NavegadorDgvDatos.CurrentRow.Index
                : 0;
        }

        // Marca la fila seleccionada en pantalla y mueve el scroll para que se vea si está muy abajo
        private void NavegadorMetSeleccionar(int Indice)
        {
            if (NavegadorDgvDatos == null ||
                !NavegadorDgvDatos.Visible ||
                NavegadorDgvDatos.Rows.Count == 0 ||
                Indice < 0 ||
                Indice >= NavegadorDgvDatos.Rows.Count)
                return;

            // Limpiamos selección anterior y marcamos la nueva
            NavegadorDgvDatos.ClearSelection();
            NavegadorDgvDatos.Rows[Indice].Selected = true;
            NavegadorDgvDatos.CurrentCell = NavegadorDgvDatos.Rows[Indice].Cells[0];

            // Si la fila quedó fuera de la vista actual, movemos el scroll automático
            if (NavegadorDgvDatos.FirstDisplayedScrollingRowIndex > Indice ||
                NavegadorDgvDatos.FirstDisplayedScrollingRowIndex +
                NavegadorDgvDatos.DisplayedRowCount(false) <= Indice)
            {
                NavegadorDgvDatos.FirstDisplayedScrollingRowIndex = Indice;
            }

            // Carga de datos inmediata a los controles registrados al moverse por los botones
            NavegadorMetProcesarSeleccionFila(NavegadorDgvDatos.Rows[Indice]);
        }
    }
}

//aca termina mi codigo
//Donald Estuardo Osorio Pérez 
//Carnet: 0901-23-17982
//18/09/2026