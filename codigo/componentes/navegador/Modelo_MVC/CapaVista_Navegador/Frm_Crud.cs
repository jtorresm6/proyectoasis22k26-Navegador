using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CapaControlador_Navegador;

namespace CapaVista_Navegador
{
    public partial class Frm_Crud : Form
    {
        // CAMBIAR AQUÍ MANUALMENTE LA TABLA A LA QUE SE DESEA HACER

        private string nombreTabla = "tbl_seguros";
        private Controlador controlador = new Controlador();

        private DataGridView dgvDatos;
        private DataTable esquemaActual;

        private bool modoModificar = false;

        private Panel panelRegistro;
        private Dictionary<string, Control> controlesRegistro;

        //Modificación realizada por: Natali Sofía Montenegro Portillo validaciones de permisos del MVC
        private string _UsuarioActual = "gerente1";
        private string _CodigoModulo = "123";

        private ClsPermisoControlador _PermisoControlador =
            new ClsPermisoControlador();

        private Dictionary<string, string> clavesPrimariasModificar =
            new Dictionary<string, string>();

        // =========================================================
        // MEJORA: LLAVES PRIMARIAS DEFINIDAS MANUALMENTE
        // =========================================================
        // Cuando ODBC no logra detectar la llave primaria de una tabla
        // (algunos motores/drivers no exponen esa metadata), se le
        // pregunta al usuario una sola vez por tabla y se recuerda
        // aquí durante el resto de la sesión.
        private Dictionary<string, List<string>> clavesManualesPorTabla =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public string NombreTabla
        {
            get
            {
                return nombreTabla;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    nombreTabla = value.Trim();
                    modoModificar = false;

                    if (dgvDatos != null)
                    {
                        ConsultarTabla();
                    }
                }
            }
        }

        // =========================================================
        // FIX: antes estos dos constructores "de comodidad" pasaban
        // siempre "tbl_puestos" como tabla, y como el constructor
        // principal hacía "NombreTabla = tabla;" sin condición, ese
        // "tbl_puestos" SIEMPRE sobrescribía el valor que se hubiera
        // puesto manualmente en el campo "nombreTabla" de arriba (por
        // ejemplo "tbl_seguros"). Por eso al cambiar la tabla arriba
        // no se notaba ningún efecto.
        //
        // Ahora se pasa "null" (sin tabla explícita) y el constructor
        // principal solo sobrescribe "nombreTabla" cuando SÍ recibe un
        // valor. Así, el valor que definas manualmente en el campo
        // "nombreTabla" (arriba, al inicio de la clase) es el que
        // realmente se usa.
        public Frm_Crud()
            : this(
                "USUARIO_PRUEBA",
                "EMPLEADOS",
                null)
        {
        }

        public Frm_Crud(
            string UsuarioActual,
            string CodigoModulo)
            : this(
                UsuarioActual,
                CodigoModulo,
                null)
        {
        }

        public Frm_Crud(
            string UsuarioActual,
            string CodigoModulo,
            string tabla)
        {
            InitializeComponent();



            // FIX: antes no se asignaban los parámetros a los campos, así que
            // siempre se usaban los valores por defecto ("gerente1"/"123")
            // sin importar qué usuario/módulo se pasara al constructor.

            _UsuarioActual = UsuarioActual;
            _CodigoModulo = CodigoModulo;

            // =====================================================
            // CAMBIAR AQUÍ MANUALMENTE LA TABLA: si quieres fijar la
            // tabla por código, edita el campo "nombreTabla" declarado
            // al inicio de la clase (private string nombreTabla = "...").
            // Ese es el ÚNICO lugar que necesitas tocar.
            //
            // Este bloque solo sobrescribe esa tabla si alguien llama
            // al constructor pasando explícitamente un nombre de tabla
            // (por ejemplo: new Frm_Crud("gerente1", "123", "tbl_x")).
            // Si "tabla" viene vacío o null, se respeta el valor
            // manual definido arriba.
            // =====================================================
            if (!string.IsNullOrWhiteSpace(tabla))
            {
                NombreTabla = tabla;
            }

            // FIX: se usa -= antes de += para garantizar una sola suscripción
            // por evento, sin importar si el Designer ya lo enganchó.


            btnIngresar.Click -= btnIngresar_Click;
            btnIngresar.Click += btnIngresar_Click;

            btnCancelar.Click -= btnCancelar_Click;
            btnCancelar.Click += btnCancelar_Click;

            btnConsultar.Click -= btnConsultar_Click;
            btnConsultar.Click += btnConsultar_Click;

            btnRefrescar.Click -= btnRefrescar_Click;
            btnRefrescar.Click += btnRefrescar_Click;

            btnModificar.Click -= btnModificar_Click;
            btnModificar.Click += btnModificar_Click;

            btnEliminar.Click -= btnEliminar_Click;
            btnEliminar.Click += btnEliminar_Click;

            btnGuardar.Click -= btnGuardar_Click;
            btnGuardar.Click += btnGuardar_Click;

            btnSalir.Click -= btnSalir_Click;
            btnSalir.Click += btnSalir_Click;

            // MEJORA: los botones de navegación (Anterior, Inicio, Fin,
            // Siguiente) ahora mueven la selección dentro del DataGridView.
            // Así el usuario puede "pasearse" por los registros y, una vez
            // posicionado en el que le interesa, usar Modificar o Eliminar
            // sobre esa fila.
            btnAnterior.Click -= btnAnterior_Click;
            btnAnterior.Click += btnAnterior_Click;

            btnInicio.Click -= btnInicio_Click;
            btnInicio.Click += btnInicio_Click;

            btnFin.Click -= btnFin_Click;
            btnFin.Click += btnFin_Click;

            btnSiguiente.Click -= btnSiguiente_Click;
            btnSiguiente.Click += btnSiguiente_Click;

            Load += Frm_Crud_Load;
            Resize += Frm_Crud_Resize;
        }

        private void Frm_Crud_Load(
            object sender,
            EventArgs e)
        {
            if (dgvDatos != null)
            {
                dgvDatos.Visible = false;
            }
        }

        private void Frm_Crud_Resize(
            object sender,
            EventArgs e)
        {
            PosicionarControles();
        }

        private bool TieneAcceso()
        {
            // Garantizar que la instancia exista usando el nombre correcto con guion bajo (_)

            if (_PermisoControlador == null)
            {
                _PermisoControlador =
                    new ClsPermisoControlador();
            }

            // Si no hay usuario o módulo asignado, permitir acceso de prueba

            if (string.IsNullOrEmpty(_UsuarioActual) ||
                string.IsNullOrEmpty(_CodigoModulo))
            {
                return true;
            }

            try
            {
                return _PermisoControlador.ValidarAcceso(
                    _UsuarioActual,
                    _CodigoModulo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al validar los permisos: " +
                    ex.Message,
                    "Error de seguridad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }
        }


        // =========================================================
        // MEJORA: SELECTOR GENÉRICO DE LISTA
        // =========================================================
        // Se usa ÚNICAMENTE para definir manualmente las columnas de
        // la llave primaria cuando el driver ODBC no puede detectarla.
        // Ya NO se usa para elegir la tabla del CRUD: eso ahora se
        // fija exclusivamente por código (ver "nombreTabla" / propiedad
        // NombreTabla / parámetro "tabla" del constructor).
        private List<string> MostrarSelectorLista(
            string titulo,
            string mensaje,
            List<string> opciones,
            bool multiSeleccion)
        {
            List<string> seleccion = new List<string>();

            using (Form dialogo = new Form())
            {
                dialogo.Text = titulo;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.Width = 380;
                dialogo.Height = 420;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.ShowInTaskbar = false;

                Label lbl = new Label();
                lbl.Text = mensaje;
                lbl.Location = new Point(10, 10);
                lbl.Size = new Size(340, 40);
                dialogo.Controls.Add(lbl);

                Button btnOk = new Button();
                btnOk.Text = "Aceptar";
                btnOk.Location = new Point(190, 325);
                btnOk.DialogResult = DialogResult.OK;

                Button btnCancel = new Button();
                btnCancel.Text = "Cancelar";
                btnCancel.Location = new Point(275, 325);
                btnCancel.DialogResult = DialogResult.Cancel;

                dialogo.AcceptButton = btnOk;
                dialogo.CancelButton = btnCancel;

                if (multiSeleccion)
                {
                    CheckedListBox clb = new CheckedListBox();
                    clb.Location = new Point(10, 55);
                    clb.Size = new Size(340, 260);

                    foreach (string opcion in opciones)
                    {
                        clb.Items.Add(opcion);
                    }

                    dialogo.Controls.Add(clb);
                    dialogo.Controls.Add(btnOk);
                    dialogo.Controls.Add(btnCancel);

                    if (dialogo.ShowDialog(this) == DialogResult.OK)
                    {
                        foreach (object item in clb.CheckedItems)
                        {
                            seleccion.Add(item.ToString());
                        }
                    }
                }
                else
                {
                    ListBox lb = new ListBox();
                    lb.Location = new Point(10, 55);
                    lb.Size = new Size(340, 260);

                    foreach (string opcion in opciones)
                    {
                        lb.Items.Add(opcion);
                    }

                    lb.DoubleClick += (s, e) =>
                    {
                        dialogo.DialogResult = DialogResult.OK;
                    };

                    dialogo.Controls.Add(lb);
                    dialogo.Controls.Add(btnOk);
                    dialogo.Controls.Add(btnCancel);

                    if (dialogo.ShowDialog(this) == DialogResult.OK &&
                        lb.SelectedItem != null)
                    {
                        seleccion.Add(lb.SelectedItem.ToString());
                    }
                }

                return seleccion;
            }
        }

        // =========================================================
        // MEJORA: OBTENER ESQUEMA GARANTIZANDO UNA LLAVE PRIMARIA
        // =========================================================
        // Envuelve controlador.ObtenerEsquemaTabla: si ninguna columna
        // quedó marcada como llave primaria (porque el driver ODBC no
        // expone esa metadata para esta base de datos), se le pregunta
        // al usuario una sola vez por tabla y se recuerda la elección
        // durante la sesión, en vez de bloquear Modificar/Eliminar.
        private DataTable ObtenerEsquemaConLlaves(string tabla)
        {
            DataTable esquema = controlador.ObtenerEsquemaTabla(tabla);

            bool tieneLlave = false;

            foreach (DataRow fila in esquema.Rows)
            {
                if (ObtenerBooleanoEsquema(fila, "IS_PRIMARY_KEY"))
                {
                    tieneLlave = true;
                    break;
                }
            }

            if (tieneLlave)
            {
                return esquema;
            }

            List<string> columnasElegidas;

            if (!clavesManualesPorTabla.TryGetValue(tabla, out columnasElegidas))
            {
                List<string> nombresColumnas = new List<string>();

                foreach (DataRow fila in esquema.Rows)
                {
                    nombresColumnas.Add(Convert.ToString(fila["COLUMN_NAME"]));
                }

                columnasElegidas = MostrarSelectorLista(
                    "Definir llave primaria",
                    "No se pudo detectar automáticamente la llave primaria de '" +
                    tabla +
                    "'.\nSeleccione la o las columnas que la conforman:",
                    nombresColumnas,
                    true);

                clavesManualesPorTabla[tabla] = columnasElegidas;
            }

            if (columnasElegidas != null && columnasElegidas.Count > 0)
            {
                foreach (DataRow fila in esquema.Rows)
                {
                    string nombreCol = Convert.ToString(fila["COLUMN_NAME"]);

                    if (columnasElegidas.Contains(nombreCol, StringComparer.OrdinalIgnoreCase))
                    {
                        fila["IS_PRIMARY_KEY"] = true;
                    }
                }
            }

            return esquema;
        }


        // Ingresar


        // =========================================================
        // CAMBIO SOLICITADO: SE ELIMINÓ LA VENTANA "Seleccionar tabla"
        // =========================================================
        // Antes este botón mostraba una ventana con TODAS las tablas
        // de la base de datos para elegir en cuál insertar. Ahora ya
        // no se muestra ninguna ventana de selección: siempre se
        // trabaja sobre "nombreTabla", la tabla fijada manualmente en
        // el código (ver arriba: campo "nombreTabla", la propiedad
        // "NombreTabla" o el parámetro "tabla" del constructor).
        private void btnIngresar_Click(
            object sender,
            EventArgs e)
        {
            if (!TieneAcceso())
            {
                return;
            }

            modoModificar = false;

            try
            {
                ConsultarTabla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ObtenerMensajeAmigable(ex),
                    "Error al consultar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            clavesPrimariasModificar =
                new Dictionary<string, string>();

            CrearFormularioRegistro(
                false,
                null);
        }

        // Consulta

        private void btnConsultar_Click(
            object sender,
            EventArgs e)
        {
            if (!TieneAcceso())
            {
                return;
            }

            CerrarFormularioRegistro();

            ConsultarTabla();
        }

        // Refrescar

        private void btnRefrescar_Click(
            object sender,
            EventArgs e)
        {
            if (!TieneAcceso())
            {
                return;
            }

            CerrarFormularioRegistro();

            if (dgvDatos != null)
            {
                dgvDatos.Visible = false;
            }
        }


        // =========================================================
        // MEJORA: NAVEGACIÓN DEL DATAGRIDVIEW
        // =========================================================
        // Estos 4 botones permiten recorrer los registros cargados y
        // dejar seleccionada la fila que luego se puede Modificar o
        // Eliminar, sin depender únicamente del clic manual sobre la
        // grilla.

        private void btnInicio_Click(
            object sender,
            EventArgs e)
        {
            SeleccionarFila(0);
        }

        private void btnAnterior_Click(
            object sender,
            EventArgs e)
        {
            if (dgvDatos == null ||
                dgvDatos.Rows.Count == 0)
            {
                return;
            }

            int filaActual =
                ObtenerIndiceFilaActual();

            SeleccionarFila(
                Math.Max(
                    0,
                    filaActual - 1));
        }

        private void btnSiguiente_Click(
            object sender,
            EventArgs e)
        {
            if (dgvDatos == null ||
                dgvDatos.Rows.Count == 0)
            {
                return;
            }

            int filaActual =
                ObtenerIndiceFilaActual();

            SeleccionarFila(
                Math.Min(
                    dgvDatos.Rows.Count - 1,
                    filaActual + 1));
        }

        private void btnFin_Click(
            object sender,
            EventArgs e)
        {
            if (dgvDatos == null ||
                dgvDatos.Rows.Count == 0)
            {
                return;
            }

            SeleccionarFila(
                dgvDatos.Rows.Count - 1);
        }

        private int ObtenerIndiceFilaActual()
        {
            if (dgvDatos == null ||
                dgvDatos.CurrentRow == null)
            {
                return 0;
            }

            return dgvDatos.CurrentRow.Index;
        }

        private void SeleccionarFila(
            int indice)
        {
            if (dgvDatos == null ||
                !dgvDatos.Visible ||
                dgvDatos.Rows.Count == 0 ||
                indice < 0 ||
                indice >= dgvDatos.Rows.Count)
            {
                return;
            }

            dgvDatos.ClearSelection();

            dgvDatos.Rows[indice].Selected =
                true;

            dgvDatos.CurrentCell =
                dgvDatos.Rows[indice].Cells[0];

            // Asegurar que la fila seleccionada quede visible en pantalla.
            if (dgvDatos.FirstDisplayedScrollingRowIndex > indice ||
                dgvDatos.FirstDisplayedScrollingRowIndex +
                    dgvDatos.DisplayedRowCount(false) <= indice)
            {
                dgvDatos.FirstDisplayedScrollingRowIndex =
                    indice;
            }
        }

        // Modificar

        private void btnModificar_Click(
            object sender,
            EventArgs e)
        {
            if (!TieneAcceso())
            {
                return;
            }

            if (dgvDatos == null ||
                dgvDatos.CurrentRow == null ||
                dgvDatos.CurrentRow.IsNewRow)
            {
                MessageBox.Show(
                    "Seleccione un registro en la tabla para modificar.",
                    "Modificar registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            try
            {
                clavesPrimariasModificar =
                    ObtenerClavesPrimarias(
                        dgvDatos.CurrentRow);

                if (clavesPrimariasModificar.Count == 0)
                {
                    MessageBox.Show(
                        "No se pudo obtener la llave primaria del registro seleccionado.",
                        "Modificar registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                foreach (
                    KeyValuePair<string, string> clave
                    in clavesPrimariasModificar)
                {
                    if (string.IsNullOrWhiteSpace(clave.Value))
                    {
                        MessageBox.Show(
                            "La llave primaria del registro seleccionado no tiene un valor válido.",
                            "Modificar registro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }

                modoModificar = true;

                CrearFormularioRegistro(
                    true,
                    dgvDatos.CurrentRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ObtenerMensajeAmigable(ex),
                    "Error al modificar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Eliminar


        private void btnEliminar_Click(
            object sender,
            EventArgs e)
        {
            if (!TieneAcceso())
            {
                return;
            }

            if (dgvDatos == null ||
                dgvDatos.CurrentRow == null ||
                dgvDatos.CurrentRow.IsNewRow)
            {
                MessageBox.Show(
                    "Seleccione un registro en la tabla para eliminar.",
                    "Eliminar registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            try
            {
                Dictionary<string, string> pk =
                    ObtenerClavesPrimarias(
                        dgvDatos.CurrentRow);

                if (pk.Count == 0)
                {
                    MessageBox.Show(
                        "La tabla no tiene una llave primaria detectable.",
                        "Eliminar registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                foreach (
                    KeyValuePair<string, string> clave
                    in pk)
                {
                    if (string.IsNullOrWhiteSpace(clave.Value))
                    {
                        MessageBox.Show(
                            "No se pudo obtener el valor de la llave primaria del registro seleccionado.",
                            "Eliminar registro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }

                if (!ConfirmarAccion(
                    "Confirmar eliminación",
                    "¿Desea eliminar el registro seleccionado de la tabla '" +
                    nombreTabla +
                    "'?"))
                {
                    return;
                }

                if (controlador.EliminarRegistro(
                    nombreTabla,
                    pk))
                {
                    MessageBox.Show(
                        "Registro eliminado correctamente.",
                        "Eliminación exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    clavesPrimariasModificar =
                        new Dictionary<string, string>();

                    ConsultarTabla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ObtenerMensajeAmigable(ex),
                    "Error al eliminar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // GUARDAR
        // =========================================================
        // MEJORA: se eliminaron los botones "Guardar"/"Cancelar" que
        // se generaban dentro del panel de registro; el toolbar ya
        // tiene Btn_guardar/Btn_cancelar cumpliendo la misma función,
        // así que aquí solo delegamos a GuardarFormularioRegistro().

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            if (panelRegistro != null &&
                panelRegistro.Visible)
            {
                GuardarFormularioRegistro();
                return;
            }

            MessageBox.Show(
                "Abra un registro con Ingresar o Modificar antes de guardar.",
                "Guardar registro",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =========================================================
        // SALIR
        // =========================================================

        private void btnSalir_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        // =========================================================
        // CONSULTAR TABLA
        // =========================================================

        private void ConsultarTabla()
        {
            try
            {
                DataTable datos =
                    controlador.llenarDgv(
                        nombreTabla);

                esquemaActual =
                    ObtenerEsquemaConLlaves(
                        nombreTabla);

                if (dgvDatos == null)
                {
                    dgvDatos =
                        new DataGridView();

                    dgvDatos.Name =
                        "dgvDatos";

                    dgvDatos.AllowUserToAddRows =
                        false;

                    dgvDatos.AllowUserToDeleteRows =
                        false;

                    dgvDatos.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode
                            .DisplayedCells;

                    dgvDatos.SelectionMode =
                        DataGridViewSelectionMode
                            .FullRowSelect;

                    dgvDatos.MultiSelect =
                        false;

                    dgvDatos.ReadOnly =
                        true;

                    dgvDatos.BackgroundColor =
                        Color.White;

                    dgvDatos.Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right;

                    Controls.Add(dgvDatos);
                }

                dgvDatos.DataSource =
                    datos;

                dgvDatos.Visible =
                    true;

                dgvDatos.ReadOnly =
                    true;

                foreach (
                    DataGridViewColumn columna
                    in dgvDatos.Columns)
                {
                    columna.ReadOnly = true;
                }

                PosicionarControles();

                Text =
                    "CRUD - " +
                    nombreTabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ObtenerMensajeAmigable(ex),
                    "Error al consultar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Posicion control

        private void PosicionarControles()
        {
            if (dgvDatos == null ||
                !dgvDatos.Visible)
            {
                return;
            }

            int margen = 10;

            int posicionY = 450;

            if (panelRegistro != null &&
                panelRegistro.Visible)
            {
                posicionY =
                    panelRegistro.Bottom + 10;
            }

            dgvDatos.Location =
                new Point(
                    margen,
                    posicionY);

            dgvDatos.Size =
                new Size(
                    Math.Max(
                        100,
                        ClientSize.Width - (margen * 2)),
                    Math.Max(
                        100,
                        ClientSize.Height -
                        posicionY -
                        margen));

            dgvDatos.BringToFront();

            if (panelRegistro != null &&
                panelRegistro.Visible)
            {
                panelRegistro.BringToFront();
            }

            btnIngresar.BringToFront();
            btnCancelar.BringToFront();
            btnConsultar.BringToFront();
            btnEliminar.BringToFront();
            btnRefrescar.BringToFront();
            btnModificar.BringToFront();
            btnAnterior.BringToFront();
            btnInicio.BringToFront();
            btnFin.BringToFront();
            btnSiguiente.BringToFront();
            btnImprimir.BringToFront();
            btnGuardar.BringToFront();
            btnAyuda.BringToFront();
            btnSalir.BringToFront();
        }

        // =========================================================
        // CREAR FORMULARIO DE REGISTRO
        // =========================================================

        private void CrearFormularioRegistro(
            bool modificar,
            DataGridViewRow filaSeleccionada)
        {
            try
            {
                DataTable esquema =
                    ObtenerEsquemaConLlaves(
                        nombreTabla);

                esquemaActual =
                    esquema;

                CerrarFormularioRegistro();

                // Si acabamos de cerrar el formulario,
                // restauramos el modo que necesitamos.
                modoModificar = modificar;

                panelRegistro =
                    new Panel();

                panelRegistro.Name =
                    "panelRegistro";

                panelRegistro.Location =
                    new Point(
                        10,
                        450);

                panelRegistro.Width =
                    ClientSize.Width - 20;

                int altura =
                    50 +
                    esquema.Rows.Count * 42;

                if (altura < 150)
                {
                    altura = 150;
                }

                if (altura > 400)
                {
                    altura = 400;
                }

                panelRegistro.Height =
                    altura;

                panelRegistro.BackColor =
                    Color.Beige;

                panelRegistro.BorderStyle =
                    BorderStyle.FixedSingle;

                panelRegistro.AutoScroll =
                    true;

                panelRegistro.Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right;

                Controls.Add(
                    panelRegistro);

                controlesRegistro =
                    new Dictionary<string, Control>();

                // MEJORA: título claro del formulario según el modo,
                // para que quede visualmente organizado.
                Label titulo =
                    new Label();

                titulo.Text =
                    (modificar
                        ? "Modificar registro - "
                        : "Nuevo registro - ") +
                    nombreTabla;

                titulo.Font =
                    new Font(
                        Font.FontFamily,
                        10,
                        FontStyle.Bold);

                titulo.AutoSize =
                    true;

                titulo.Location =
                    new Point(
                        10,
                        8);

                panelRegistro.Controls.Add(
                    titulo);

                int posicionY = 34;

                foreach (DataRow columna
                    in esquema.Rows)
                {
                    string campo =
                        Convert.ToString(
                            columna["COLUMN_NAME"]);

                    string tipo =
                        Convert.ToString(
                            columna["DATA_TYPE"]);

                    bool esPk =
                        ObtenerBooleanoEsquema(
                            columna,
                            "IS_PRIMARY_KEY");

                    bool esFk =
                        ObtenerBooleanoEsquema(
                            columna,
                            "IS_FOREIGN_KEY");

                    bool esAuto =
                        ObtenerBooleanoEsquema(
                            columna,
                            "IS_AUTOINCREMENT");

                    string tablaFk =
                        ObtenerTextoEsquema(
                            columna,
                            "FK_TABLE_NAME");

                    string columnaFk =
                        ObtenerTextoEsquema(
                            columna,
                            "FK_COLUMN_NAME");

                    Label etiqueta =
                        new Label();

                    etiqueta.Text =
                        campo +
                        (esPk
                            ? " [PK]"
                            : "") +
                        (esFk
                            ? " [FK]"
                            : "");

                    etiqueta.Location =
                        new Point(
                            15,
                            posicionY + 4);

                    etiqueta.AutoSize =
                        true;

                    // MEJORA: distinguir visualmente PK/FK del resto de
                    // los campos para que el formulario se lea mejor.
                    if (esPk)
                    {
                        etiqueta.Font =
                            new Font(
                                Font.FontFamily,
                                Font.Size,
                                FontStyle.Bold);

                        etiqueta.ForeColor =
                            Color.DarkRed;
                    }
                    else if (esFk)
                    {
                        etiqueta.Font =
                            new Font(
                                Font.FontFamily,
                                Font.Size,
                                FontStyle.Bold);

                        etiqueta.ForeColor =
                            Color.DarkBlue;
                    }

                    Control control;

                    // =================================================
                    // FK = COMBOBOX
                    // =================================================

                    if (esFk &&
                        !string.IsNullOrWhiteSpace(
                            tablaFk) &&
                        !string.IsNullOrWhiteSpace(
                            columnaFk))
                    {
                        ComboBox combo =
                            CrearComboLlaveForanea(
                                tablaFk,
                                columnaFk,
                                filaSeleccionada,
                                campo);

                        combo.Location =
                            new Point(
                                190,
                                posicionY);

                        combo.Width =
                            250;

                        // FIX: antes se deshabilitaba el combo con solo
                        // "!esPk", así que una FK que también es PK (caso
                        // muy común en llaves compuestas, ej. tbl_asistencias)
                        // quedaba SIEMPRE bloqueada, incluso al Ingresar un
                        // registro nuevo, donde sí se necesita elegir el
                        // valor. Ahora solo se bloquea si además se está
                        // Modificando (donde la PK no debe cambiar).
                        combo.Enabled =
                            !(esPk && modificar);

                        control =
                            combo;
                    }

                    // =================================================
                    // FECHA
                    // =================================================

                    else if (EsFecha(columna))
                    {
                        DateTimePicker fecha =
                            new DateTimePicker();

                        fecha.Name =
                            "dtp_" +
                            campo;

                        fecha.Location =
                            new Point(
                                190,
                                posicionY);

                        fecha.Width =
                            250;

                        fecha.Format =
                            DateTimePickerFormat.Short;

                        fecha.Value =
                            ObtenerFechaInicial(
                                filaSeleccionada,
                                campo);

                        // FIX: mismo problema que con el combo de FK:
                        // antes "!esPk" bloqueaba también al Ingresar.
                        fecha.Enabled =
                            !(esPk && modificar);

                        control =
                            fecha;
                    }

                    // =================================================
                    // BOOLEANO = CHECKBOX (marcado = 1, sin marcar = 0)
                    // =================================================
                    // CAMBIO SOLICITADO: se reemplazó la pareja de
                    // RadioButton "Sí/No" por un único CheckBox. Se
                    // genera automáticamente para CUALQUIER columna que
                    // EsBooleano() detecte (por ejemplo un campo
                    // "estado_seguro BOOLEAN DEFAULT TRUE"), sin importar
                    // la tabla: marcado = true/1, sin marcar = false/0.

                    else if (EsBooleano(columna))
                    {
                        CheckBox chk =
                            new CheckBox();

                        chk.Name =
                            "chk_" +
                            campo;

                        // El propio nombre del campo ya se muestra en la
                        // etiqueta de la izquierda; el texto del checkbox
                        // solo aclara el significado de marcado/desmarcado.
                        chk.Text =
                            "Sí (marcado) / No (desmarcado)";

                        chk.AutoSize =
                            true;

                        chk.Location =
                            new Point(
                                190,
                                posicionY + 3);

                        bool valorInicial =
                            ObtenerBooleanoInicial(
                                filaSeleccionada,
                                campo);

                        chk.Checked =
                            valorInicial;

                        // FIX: mismo criterio que combo/fecha; solo se
                        // bloquea si además se está Modificando (nunca al
                        // Ingresar un registro nuevo).
                        chk.Enabled =
                            !(esPk && modificar);

                        control =
                            chk;
                    }

                    // =================================================
                    // TEXTO / NUMERO
                    // =================================================

                    else
                    {
                        TextBox caja =
                            new TextBox();

                        caja.Name =
                            "txt_" +
                            campo;

                        caja.Location =
                            new Point(
                                190,
                                posicionY);

                        caja.Width =
                            250;

                        caja.Text =
                            ObtenerTextoInicial(
                                filaSeleccionada,
                                campo);

                        // =============================================
                        // MEJORA: AUTOGENERACIÓN DE LLAVE PRIMARIA
                        // =============================================
                        // Al insertar un registro nuevo:
                        //  - Si el motor de BD maneja autoincremento
                        //    real (esAuto), lo dejamos en manos del
                        //    motor: no mostramos ni enviamos un valor.
                        //  - Si es PK pero NO es autoincremento nativo
                        //    (la mayoría de tablas vía ODBC genérico),
                        //    calculamos automáticamente el siguiente
                        //    valor (MAX + 1) y lo mostramos ya listo,
                        //    de forma que si hay 14 registros, el nuevo
                        //    campo ID aparezca con 15 sin que el usuario
                        //    tenga que escribirlo. Esto es lo que hacen
                        //    la mayoría de sistemas: el ID nunca lo
                        //    escribe el usuario, se calcula solo.
                        if (!modificar && esAuto)
                        {
                            caja.Text =
                                "(automático)";

                            caja.ReadOnly =
                                true;

                            caja.BackColor =
                                Color.LightGray;
                        }
                        else if (!modificar && esPk)
                        {
                            if (EsTipoNumerico(columna))
                            {
                                object siguiente = null;

                                try
                                {
                                    siguiente =
                                        controlador
                                            .ObtenerSiguienteValorLlave(
                                                nombreTabla,
                                                campo);
                                }
                                catch (Exception exPk)
                                {
                                    MessageBox.Show(
                                        "No se pudo calcular automáticamente el " +
                                        "siguiente valor de '" +
                                        campo +
                                        "'. Ingréselo manualmente.\n\n" +
                                        exPk.Message,
                                        "Llave primaria",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                }

                                caja.Text =
                                    siguiente != null
                                        ? Convert.ToString(siguiente)
                                        : "";

                                // Se pudo calcular: se muestra de solo
                                // lectura para que el usuario vea el ID
                                // que se va a registrar (no lo puede
                                // cambiar, igual que en un sistema real
                                // donde el correlativo no se edita).
                                if (siguiente != null)
                                {
                                    caja.ReadOnly =
                                        true;

                                    caja.BackColor =
                                        Color.LightGray;
                                }
                            }

                            // Si la PK no es numérica (códigos, etc.), o
                            // forma parte de una llave compuesta con un
                            // valor de negocio (ej. fecha_asistencia),
                            // no se puede autogenerar: queda editable
                            // para que el usuario la escriba/elija.
                        }

                        // Al modificar:
                        // PK no se puede cambiar, pero sí se muestra.
                        if (modificar &&
                            esPk)
                        {
                            caja.ReadOnly =
                                true;

                            caja.BackColor =
                                Color.LightGray;
                        }

                        control =
                            caja;
                    }

                    // =================================================
                    // BLOQUEAR PK SOLO AL MODIFICAR
                    // =================================================
                    // FIX: antes este bloque se ejecutaba siempre que
                    // "esPk" era true, sin importar si se estaba
                    // Ingresando o Modificando. Eso volvía a bloquear
                    // (Enabled = false) cualquier control de PK que no
                    // fuera TextBox (combo de FK+PK, fecha PK, checkbox
                    // PK) incluso al Ingresar un registro nuevo, que es
                    // exactamente el bug reportado con
                    // tbl_asistencias (id_empleado [PK][FK] y
                    // fecha_asistencia [PK] bloqueados al Ingresar).
                    //
                    // Ahora solo se fuerza solo-lectura/deshabilitado
                    // cuando corresponde: al Modificar siempre, y al
                    // Ingresar solo si el propio control de texto ya
                    // quedó en solo lectura porque su valor se
                    // autogeneró arriba.

                    if (esPk)
                    {
                        TextBox cajaPk =
                            control as TextBox;

                        if (cajaPk != null)
                        {
                            if (modificar || cajaPk.ReadOnly)
                            {
                                cajaPk.ReadOnly = true;
                                cajaPk.BackColor = Color.LightGray;
                            }
                        }
                        else if (modificar)
                        {
                            control.Enabled = false;
                        }
                    }

                    panelRegistro.Controls.Add(
                        etiqueta);

                    panelRegistro.Controls.Add(
                        control);

                    controlesRegistro[campo] =
                        control;

                    posicionY += 42;
                }

                panelRegistro.Visible =
                    true;

                PosicionarControles();

                panelRegistro.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ObtenerMensajeAmigable(ex),
                    "Error al abrir el formulario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // MEJORA: CONFIRMACIÓN ANTES DE EJECUTAR UN CAMBIO
        // =========================================================
        // Se usa para Ingresar, Modificar y Eliminar: primero se
        // muestran las labels/valores del registro en el panel (ya
        // visibles) y solo cuando el usuario vuelve a dar clic en el
        // botón de acción y confirma en este cuadro, se ejecuta el
        // cambio contra la base de datos. Es genérico: funciona para
        // cualquier tabla/campo, sin importar el esquema conectado.

        private bool ConfirmarAccion(
            string titulo,
            string mensaje)
        {
            DialogResult respuesta =
                MessageBox.Show(
                    mensaje,
                    titulo,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            return respuesta == DialogResult.Yes;
        }

        // Arma un resumen "campo: valor" con los datos que se van a
        // guardar, para mostrarlo dentro del cuadro de confirmación.

        private string ConstruirResumenDatos(
            Dictionary<string, string> datos)
        {
            string resumen = "";

            foreach (KeyValuePair<string, string> dato in datos)
            {
                resumen +=
                    dato.Key +
                    ": " +
                    dato.Value +
                    "\n";
            }

            return resumen;
        }

        // =========================================================
        // GUARDAR FORMULARIO
        // =========================================================

        private void GuardarFormularioRegistro()
        {
            if (controlesRegistro == null ||
                esquemaActual == null)
            {
                return;
            }

            try
            {
                Dictionary<string, string> datos =
                    new Dictionary<string, string>();

                // =====================================================
                // OBTENER DATOS DEL FORMULARIO
                // =====================================================

                foreach (DataRow columna
                    in esquemaActual.Rows)
                {
                    string campo =
                        Convert.ToString(
                            columna["COLUMN_NAME"]);

                    string tipo =
                        Convert.ToString(
                            columna["DATA_TYPE"]);

                    bool esPk =
                        ObtenerBooleanoEsquema(
                            columna,
                            "IS_PRIMARY_KEY");

                    bool esAuto =
                        ObtenerBooleanoEsquema(
                            columna,
                            "IS_AUTOINCREMENT");

                    Control control;

                    if (!controlesRegistro.TryGetValue(
                        campo,
                        out control))
                    {
                        continue;
                    }

                    // En INSERTAR:
                    // MEJORA: solo se omite el campo si el motor de BD
                    // maneja autoincremento nativo (esAuto). Si es una
                    // PK "manual" (la mayoría de tablas por ODBC), su
                    // valor ya fue calculado automáticamente (MAX + 1)
                    // al construir el formulario, o fue elegido/escrito
                    // por el usuario (llave compuesta), y SÍ debe
                    // enviarse.
                    if (!modoModificar &&
                        esAuto)
                    {
                        continue;
                    }

                    // En MODIFICAR:
                    // La PK jamás se actualiza.
                    if (modoModificar &&
                        esPk)
                    {
                        continue;
                    }

                    string valor =
                        ObtenerValorControl(
                            control,
                            tipo);

                    bool nullable =
                        string.Equals(
                            ObtenerTextoEsquema(
                                columna,
                                "IS_NULLABLE"),
                            "YES",
                            StringComparison
                                .OrdinalIgnoreCase);

                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        if (!nullable)
                        {
                            MessageBox.Show(
                                "El campo '" +
                                campo +
                                "' es obligatorio.",
                                "Validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            control.Focus();

                            return;
                        }

                        continue;
                    }

                    datos[campo] =
                        valor;
                }

                // =====================================================
                // VALIDAR
                // =====================================================

                List<string> errores =
                    controlador.ValidarRegistro(
                        datos,
                        nombreTabla);

                if (errores.Count > 0)
                {
                    MessageBox.Show(
                        string.Join(
                            "\n",
                            errores),
                        "Errores de validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // =====================================================
                // INSERTAR
                // =====================================================

                if (!modoModificar)
                {
                    // =================================================
                    // MEJORA: VALIDAR LLAVE PRIMARIA DUPLICADA ANTES DE
                    // INSERTAR (mensaje claro, sin esperar al error del
                    // motor de base de datos)
                    // =================================================
                    // Antes, si la llave primaria ya existía, el INSERT
                    // fallaba en la BD y el usuario solo veía el mensaje
                    // genérico traducido por ObtenerMensajeAmigable (o el
                    // mensaje crudo del driver si no calzaba con ningún
                    // patrón). Ahora se verifica primero con
                    // ExisteLlavePrimaria, indicando exactamente qué
                    // campo(s) y valor(es) ya están en uso, sin importar
                    // si la llave es simple o compuesta, ni de qué motor
                    // de base de datos se trate.
                    List<string> pkCampos =
                        new List<string>();

                    List<string> pkValores =
                        new List<string>();

                    foreach (DataRow columnaPk
                        in esquemaActual.Rows)
                    {
                        bool esPkCampo =
                            ObtenerBooleanoEsquema(
                                columnaPk,
                                "IS_PRIMARY_KEY");

                        if (!esPkCampo)
                        {
                            continue;
                        }

                        string nombreCampoPk =
                            Convert.ToString(
                                columnaPk["COLUMN_NAME"]);

                        string valorPk;

                        if (datos.TryGetValue(
                            nombreCampoPk,
                            out valorPk))
                        {
                            pkCampos.Add(nombreCampoPk);
                            pkValores.Add(valorPk);
                        }
                    }

                    if (pkCampos.Count > 0)
                    {
                        bool yaExiste =
                            controlador.ExisteLlavePrimaria(
                                nombreTabla,
                                pkCampos.ToArray(),
                                pkValores.ToArray());

                        if (yaExiste)
                        {
                            MessageBox.Show(
                                "Ya existe un registro con esta llave primaria (" +
                                string.Join(", ", pkCampos) +
                                " = " +
                                string.Join(", ", pkValores) +
                                ").\nCambie el valor e intente nuevamente.",
                                "Llave primaria duplicada",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }

                    // =================================================
                    // MEJORA: CONFIRMAR ANTES DE INGRESAR
                    // =================================================
                    if (!ConfirmarAccion(
                        "Confirmar ingreso",
                        "¿Desea ingresar el siguiente registro en la tabla '" +
                        nombreTabla +
                        "'?\n\n" +
                        ConstruirResumenDatos(datos)))
                    {
                        return;
                    }

                    if (controlador.InsertarRegistro(
                        nombreTabla,
                        datos))
                    {
                        MessageBox.Show(
                            "Registro guardado correctamente.",
                            "Guardado exitoso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        CerrarFormularioRegistro();

                        ConsultarTabla();
                    }

                    return;
                }

                // =====================================================
                // MODIFICAR
                // =====================================================

                Dictionary<string, string> pk =
                    new Dictionary<string, string>(
                        clavesPrimariasModificar);

                if (pk.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontró la llave primaria del registro seleccionado.",
                        "Modificar registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                foreach (
                    KeyValuePair<string, string> clave
                    in pk)
                {
                    if (string.IsNullOrWhiteSpace(clave.Value))
                    {
                        MessageBox.Show(
                            "La llave primaria original no tiene un valor válido.",
                            "Modificar registro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }

                // Los datos contienen únicamente campos
                // que sí pueden modificarse.
                Dictionary<string, string> valores =
                    new Dictionary<string, string>();

                foreach (
                    KeyValuePair<string, string> dato
                    in datos)
                {
                    if (!EsLlavePrimaria(
                        dato.Key))
                    {
                        valores[dato.Key] =
                            dato.Value;
                    }
                }

                if (valores.Count == 0)
                {
                    MessageBox.Show(
                        "No hay campos disponibles para modificar.",
                        "Modificar registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                // =====================================================
                // MEJORA: CONFIRMAR ANTES DE MODIFICAR
                // =====================================================
                if (!ConfirmarAccion(
                    "Confirmar modificación",
                    "¿Desea guardar los siguientes cambios en el registro " +
                    "seleccionado de la tabla '" +
                    nombreTabla +
                    "'?\n\n" +
                    ConstruirResumenDatos(valores)))
                {
                    return;
                }

                // ActualizarRegistro utiliza la PK original
                // para modificar exactamente el registro seleccionado.
                if (controlador.ActualizarRegistro(
                    nombreTabla,
                    valores,
                    pk))
                {
                    MessageBox.Show(
                        "Registro modificado correctamente.",
                        "Modificación exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    CerrarFormularioRegistro();

                    clavesPrimariasModificar =
                        new Dictionary<string, string>();

                    ConsultarTabla();
                }
                else
                {
                    MessageBox.Show(
                        "No se pudo modificar el registro seleccionado.",
                        "Modificar registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ObtenerMensajeAmigable(ex),
                    "Error al guardar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // CERRAR FORMULARIO
        // =========================================================

        private void CerrarFormularioRegistro()
        {
            if (panelRegistro != null)
            {
                Controls.Remove(
                    panelRegistro);

                panelRegistro.Dispose();

                panelRegistro =
                    null;
            }

            controlesRegistro =
                null;

            if (dgvDatos != null)
            {
                dgvDatos.ReadOnly =
                    true;

                dgvDatos.ClearSelection();
            }

            modoModificar = false;

            PosicionarControles();
        }

        // =========================================================
        // COMBOBOX PARA FK
        // =========================================================

        private ComboBox CrearComboLlaveForanea(
            string tablaFk,
            string columnaFk,
            DataGridViewRow fila,
            string campo)
        {
            ComboBox combo =
                new ComboBox();

            combo.Name =
                "cbo_" +
                campo;

            combo.DropDownStyle =
                ComboBoxStyle.DropDownList;

            try
            {
                DataTable opciones =
                    controlador.llenarDgv(
                        tablaFk);

                DataTable esquemaFk =
                    controlador.ObtenerEsquemaTabla(
                        tablaFk);

                string columnaMostrar =
                    columnaFk;

                // =====================================================
                // BUSCAR UNA COLUMNA DESCRIPTIVA
                // =====================================================

                foreach (DataRow c
                    in esquemaFk.Rows)
                {
                    string nombre =
                        Convert.ToString(
                            c["COLUMN_NAME"]);

                    string tipo =
                        Convert.ToString(
                            c["DATA_TYPE"])
                        .ToLowerInvariant();

                    if (nombre.Equals(
                        columnaFk,
                        StringComparison
                            .OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (tipo.Contains("char") ||
                        tipo.Contains("text"))
                    {
                        columnaMostrar =
                            nombre;

                        break;
                    }
                }

                if (!opciones.Columns.Contains(
                    columnaFk))
                {
                    return combo;
                }

                if (!opciones.Columns.Contains(
                    columnaMostrar))
                {
                    columnaMostrar =
                        columnaFk;
                }

                combo.DataSource =
                    opciones;

                combo.ValueMember =
                    columnaFk;

                combo.DisplayMember =
                    columnaMostrar;

                combo.SelectedIndex =
                    -1;

                // =====================================================
                // SELECCIONAR EL VALOR ACTUAL
                // =====================================================

                if (fila != null &&
                    fila.DataGridView != null)
                {
                    int indice =
                        ObtenerIndiceColumna(
                            fila.DataGridView,
                            campo);

                    if (indice >= 0)
                    {
                        object valorCelda =
                            fila.Cells[indice].Value;

                        if (valorCelda != null &&
                            valorCelda != DBNull.Value)
                        {
                            string valor =
                                Convert.ToString(
                                    valorCelda);

                            for (int i = 0;
                                i < combo.Items.Count;
                                i++)
                            {
                                DataRowView item =
                                    combo.Items[i]
                                    as DataRowView;

                                if (item == null)
                                {
                                    continue;
                                }

                                string valorItem =
                                    Convert.ToString(
                                        item.Row[
                                            columnaFk]);

                                if (string.Equals(
                                    valorItem,
                                    valor,
                                    StringComparison
                                        .OrdinalIgnoreCase))
                                {
                                    combo.SelectedIndex =
                                        i;

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las opciones de la llave foránea '" +
                    campo +
                    "'.\n\n" +
                    ex.Message,
                    "Error al cargar opciones",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return combo;
        }

        // =========================================================
        // OBTENER PK DEL REGISTRO SELECCIONADO
        // =========================================================

        private Dictionary<string, string>
            ObtenerClavesPrimarias(
                DataGridViewRow fila)
        {
            Dictionary<string, string> resultado =
                new Dictionary<string, string>();

            if (esquemaActual == null ||
                fila == null ||
                fila.DataGridView == null)
            {
                return resultado;
            }

            foreach (DataRow columna
                in esquemaActual.Rows)
            {
                bool esPk =
                    ObtenerBooleanoEsquema(
                        columna,
                        "IS_PRIMARY_KEY");

                if (!esPk)
                {
                    continue;
                }

                string nombre =
                    Convert.ToString(
                        columna["COLUMN_NAME"]);

                int indice =
                    ObtenerIndiceColumna(
                        fila.DataGridView,
                        nombre);

                if (indice < 0)
                {
                    continue;
                }

                object valor =
                    fila.Cells[indice].Value;

                resultado[nombre] =
                    valor == null ||
                    valor == DBNull.Value
                        ? ""
                        : Convert.ToString(
                            valor);
            }

            return resultado;
        }

        // =========================================================
        // OBTENER INDICE DE COLUMNA
        // =========================================================

        private int ObtenerIndiceColumna(
            DataGridView grid,
            string nombreCampo)
        {
            if (grid == null)
            {
                return -1;
            }

            foreach (DataGridViewColumn columna
                in grid.Columns)
            {
                if (string.Equals(
                    columna.DataPropertyName,
                    nombreCampo,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return columna.Index;
                }

                if (string.Equals(
                    columna.Name,
                    nombreCampo,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return columna.Index;
                }
            }

            return -1;
        }

        // =========================================================
        // COMPROBAR PK
        // =========================================================

        private bool EsLlavePrimaria(
            string nombreCampo)
        {
            if (esquemaActual == null)
            {
                return false;
            }

            foreach (DataRow fila
                in esquemaActual.Rows)
            {
                bool esPk =
                    ObtenerBooleanoEsquema(
                        fila,
                        "IS_PRIMARY_KEY");

                if (esPk &&
                    string.Equals(
                        Convert.ToString(
                            fila["COLUMN_NAME"]),
                        nombreCampo,
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // =========================================================
        // OBTENER VALOR DEL CONTROL
        // =========================================================
        // CAMBIO SOLICITADO: se retiró el manejo del panel de
        // RadioButton (ya no se crea); el booleano ahora llega como
        // CheckBox y se traduce a "1"/"0" según su propiedad Checked.

        private string ObtenerValorControl(
            Control control,
            string tipo)
        {
            DateTimePicker fecha =
                control as DateTimePicker;

            if (fecha != null)
            {
                return fecha.Value.ToString(
                    "yyyy-MM-dd");
            }

            ComboBox combo =
                control as ComboBox;

            if (combo != null)
            {
                if (combo.SelectedIndex < 0 ||
                    combo.SelectedValue == null)
                {
                    return "";
                }

                return Convert.ToString(
                    combo.SelectedValue);
            }

            CheckBox check =
                control as CheckBox;

            if (check != null)
            {
                // marcado (Checked = true)  -> "1"
                // sin marcar (Checked = false) -> "0"
                return check.Checked
                    ? "1"
                    : "0";
            }

            return control.Text.Trim();
        }

        // =========================================================
        // TEXTO INICIAL
        // =========================================================

        private string ObtenerTextoInicial(
            DataGridViewRow fila,
            string campo)
        {
            if (fila == null ||
                fila.DataGridView == null)
            {
                return "";
            }

            int indice =
                ObtenerIndiceColumna(
                    fila.DataGridView,
                    campo);

            if (indice < 0)
            {
                return "";
            }

            object valor =
                fila.Cells[indice].Value;

            return valor == null ||
                   valor == DBNull.Value
                ? ""
                : Convert.ToString(
                    valor);
        }

        // =========================================================
        // FECHA INICIAL
        // =========================================================

        private DateTime ObtenerFechaInicial(
            DataGridViewRow fila,
            string campo)
        {
            DateTime fecha;

            string texto =
                ObtenerTextoInicial(
                    fila,
                    campo);

            if (DateTime.TryParse(
                texto,
                out fecha))
            {
                return fecha;
            }

            return DateTime.Today;
        }

        // =========================================================
        // BOOLEANO INICIAL
        // =========================================================

        private bool ObtenerBooleanoInicial(
            DataGridViewRow fila,
            string campo)
        {
            string valor =
                ObtenerTextoInicial(
                    fila,
                    campo)
                .ToLowerInvariant();

            return valor == "1" ||
                   valor == "true" ||
                   valor == "yes" ||
                   valor == "si";
        }

        // =========================================================
        // DETECTAR FECHA
        // =========================================================
        // FIX: antes solo miraba el texto crudo de DATA_TYPE, que
        // varía mucho según el driver ODBC y puede no contener
        // "date"/"time" aunque la columna sí sea una fecha. Ahora se
        // usa primero NET_TYPE (el tipo .NET real leído directamente
        // del driver, ver Sentencias.ObtenerTiposNet), y DATA_TYPE
        // queda como respaldo.
        private bool EsFecha(DataRow columna)
        {
            string net =
                ObtenerTextoEsquema(columna, "NET_TYPE")
                .ToLowerInvariant();

            if (net == "datetime" ||
                net == "date" ||
                net == "timespan")
            {
                return true;
            }

            string t =
                ObtenerTextoEsquema(columna, "DATA_TYPE")
                .ToLowerInvariant();

            return t.Contains("date") ||
                   t.Contains("time") ||
                   t.Contains("timestamp");
        }

        // =========================================================
        // DETECTAR BOOLEANO
        // =========================================================
        // FIX: mismo criterio que EsFecha, usando NET_TYPE primero.
        // Esta detección es la que decide si el campo se dibuja como
        // CheckBox en el formulario (ver CrearFormularioRegistro).
        //
        // MEJORA: se agregó reconocer "tinyint(1)" como booleano.
        // MySQL NO tiene un tipo BOOLEAN real: cuando en la BD se
        // declara una columna como "BOOLEAN" o "BOOL" (ej.
        // "estado_seguro BOOLEAN DEFAULT TRUE"), el motor la guarda
        // internamente como TINYINT(1), y el driver ODBC la reporta
        // como "tinyint" a secas, sin ninguna palabra "bool" en
        // DATA_TYPE ni en NET_TYPE. Por eso antes ese campo caía
        // siempre en el "else" (TextBox) en vez de mostrarse como
        // CheckBox.
        //
        // FIX (nuevo): la convención universal para distinguir un
        // tinyint booleano de un tinyint numérico normal es la
        // longitud/precisión declarada de la columna: tinyint(1) =
        // booleano, tinyint(3) o más = numérico real. Esa longitud
        // debería venir en "COLUMN_SIZE" (ver
        // Sentencias.ObtenerEsquemaTabla), PERO la mayoría de drivers
        // ODBC de MySQL reportan COLUMN_SIZE = 3 (la precisión del
        // TIPO tinyint) para CUALQUIER tinyint, sin importar si se
        // declaró como (1) o no — es decir, no conservan el "display
        // width" original. Por eso ese chequeo solo, fallaba siempre
        // para columnas BOOLEAN reales.
        //
        // Ahora se usa primero "COLUMN_TYPE_TEXT", que sí trae el
        // texto real de MySQL (ej. "tinyint(1)") leído directamente de
        // INFORMATION_SCHEMA.COLUMNS (ver
        // Sentencias.ObtenerTiposColumnaTexto), y se deja COLUMN_SIZE
        // como respaldo para motores distintos a MySQL donde esa
        // información sí sea confiable.
        private bool EsBooleano(DataRow columna)
        {
            string net =
                ObtenerTextoEsquema(columna, "NET_TYPE")
                .ToLowerInvariant();

            if (net == "boolean")
            {
                return true;
            }

            // FIX: texto real de COLUMN_TYPE (MySQL/MariaDB), conserva
            // el display width que ODBC pierde (ej. "tinyint(1)").
            string columnTypeTexto =
                ObtenerTextoEsquema(columna, "COLUMN_TYPE_TEXT")
                .ToLowerInvariant()
                .Replace(" ", "");

            if (columnTypeTexto.Contains("tinyint(1)") ||
                columnTypeTexto == "bool" ||
                columnTypeTexto == "boolean")
            {
                return true;
            }

            string t =
                ObtenerTextoEsquema(columna, "DATA_TYPE")
                .ToLowerInvariant();

            if (t == "bit" ||
                t == "boolean" ||
                t == "bool")
            {
                return true;
            }

            // Respaldo: motores donde COLUMN_SIZE sí refleja el
            // display width real (no MySQL, donde ese chequeo por sí
            // solo no es confiable, ver comentario arriba).
            if (t == "tinyint" ||
                t.Contains("tinyint"))
            {
                string tamanoTexto =
                    ObtenerTextoEsquema(
                        columna,
                        "COLUMN_SIZE");

                int tamano;

                if (int.TryParse(
                    tamanoTexto,
                    out tamano) &&
                    tamano == 1)
                {
                    return true;
                }
            }

            return false;
        }

        // =========================================================
        // MEJORA: DETECTAR TIPO NUMÉRICO (para autogenerar PK)
        // =========================================================
        // FIX: mismo criterio, ahora recibe la fila completa del
        // esquema para poder usar NET_TYPE como primera fuente de
        // verdad y DATA_TYPE como respaldo.
        private bool EsTipoNumerico(DataRow columna)
        {
            string net =
                ObtenerTextoEsquema(columna, "NET_TYPE")
                .ToLowerInvariant();

            string[] tiposNetNumericos =
            {
                "int16", "int32", "int64",
                "byte", "sbyte",
                "decimal", "double", "single"
            };

            if (tiposNetNumericos.Contains(net))
            {
                return true;
            }

            string t =
                ObtenerTextoEsquema(columna, "DATA_TYPE")
                .ToLowerInvariant();

            switch (t)
            {
                case "int":
                case "integer":
                case "smallint":
                case "bigint":
                case "tinyint":
                case "decimal":
                case "numeric":
                case "float":
                case "double":
                case "real":
                case "counter":
                case "number":
                    return true;

                default:
                    return false;
            }
        }

        // =========================================================
        // VALIDAR CORREO
        // =========================================================

        private bool ValidarCorreo(
            string correo)
        {
            return Regex.IsMatch(
                correo,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // =========================================================
        // CANCELAR
        // =========================================================

        private void btnCancelar_Click(
            object sender,
            EventArgs e)
        {
            CerrarFormularioRegistro();

            clavesPrimariasModificar =
                new Dictionary<string, string>();
        }

        // =========================================================
        // OBTENER ESQUEMA
        // =========================================================

        public DataTable ObtenerEsquemaTabla()
        {
            try
            {
                return controlador
                    .ObtenerEsquemaTabla(
                        nombreTabla);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al obtener el esquema de la tabla:\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return null;
            }
        }

        // =========================================================
        // OBTENER TEXTO DE ESQUEMA DE FORMA SEGURA
        // =========================================================

        private string ObtenerTextoEsquema(
            DataRow fila,
            string columna)
        {
            if (fila == null ||
                fila.Table == null ||
                !fila.Table.Columns.Contains(
                    columna) ||
                fila[columna] == DBNull.Value)
            {
                return "";
            }

            return Convert.ToString(
                fila[columna]);
        }

        // =========================================================
        // OBTENER BOOLEANO DE ESQUEMA DE FORMA SEGURA
        // =========================================================
        /// <sum

      //hola
        private bool ObtenerBooleanoEsquema(
            DataRow fila,
            string columna)
        {
            string valor =
                ObtenerTextoEsquema(
                    fila,
                    columna);

            if (string.IsNullOrWhiteSpace(valor))
            {
                return false;
            }

            bool resultado;

            if (bool.TryParse(
                valor,
                out resultado))
            {
                return resultado;
            }

            return valor == "1" ||
                   valor.Equals(
                       "YES",
                       StringComparison
                           .OrdinalIgnoreCase) ||
                   valor.Equals(
                       "SI",
                       StringComparison
                           .OrdinalIgnoreCase);
        }

        // =========================================================
        // MENSAJES AMIGABLES holaaaaaaaaaaa
        // =========================================================

        private string ObtenerMensajeAmigable(
            Exception ex)
        {
            string msg =
                ex.Message ?? "";

            string lower =
                msg.ToLowerInvariant();

            if (lower.Contains("foreign key") ||
                lower.Contains("fk_") ||
                lower.Contains("reference constraint"))
            {
                return
                    "El registro no puede guardarse o eliminarse " +
                    "porque existe una relación de llave foránea. " +
                    "Verifique los registros relacionados.";
            }

            if (lower.Contains("duplicate entry") ||
                lower.Contains("duplicate key") ||
                lower.Contains("unique constraint") ||
                lower.Contains("violation of unique") ||
                lower.Contains("violation of primary key"))
            {
                return
                    "Ya existe un registro con el mismo " +
                    "valor en un campo único.";
            }

            if (lower.Contains("cannot be null") ||
                lower.Contains("null value") ||
                lower.Contains("not-null constraint") ||
                lower.Contains("insert the value null"))
            {
                return
                    "Hay un campo obligatorio que no puede " +
                    "quedar vacío.";
            }

            if (lower.Contains("data too long") ||
                lower.Contains("truncat") ||
                lower.Contains(
                    "string or binary data would be truncated"))
            {
                return
                    "Uno de los valores ingresados es demasiado " +
                    "largo para el campo correspondiente.";
            }

            if ((lower.Contains("incorrect") &&
                 lower.Contains("value")) ||
                lower.Contains("conversion failed") ||
                lower.Contains("invalid input syntax"))
            {
                return
                    "Uno de los valores ingresados tiene un " +
                    "formato incorrecto para su campo.";
            }

            return msg;
        }
    }
}