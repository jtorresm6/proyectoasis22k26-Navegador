using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    public partial class Frm_Crud : Form
    {
        // CAMBIAR AQUÍ MANUALMENTE LA TABLA A LA QUE SE DESEA HACER CRUD
        private string nombreTabla = "tbl_permisos";

        private ctrlTabla ctrlTabla = new ctrlTabla();
        private crudGrid grid;
        private crudFormulario formulario;
        private crudAcciones acciones = new crudAcciones();
        private selectorLlave selectorLlave;
        private crudSeguridad seguridad;

        private List<columnaInfo> esquemaActual;
        private Dictionary<string, string> pkModificar;

        public string NombreTabla
        {
            get { return nombreTabla; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    nombreTabla = value.Trim();
                    ConsultarTabla();
                }
            }
        }

        public Frm_Crud() : this("USUARIO_PRUEBA", "EMPLEADOS", null) { }

        public Frm_Crud(string usuarioActual, string codigoModulo) : this(usuarioActual, codigoModulo, null) { }

        public Frm_Crud(string usuarioActual, string codigoModulo, string tabla)
        {
            InitializeComponent();

            grid = new crudGrid(this);
            formulario = new crudFormulario(this);
            selectorLlave = new selectorLlave(this);
            seguridad = new crudSeguridad(usuarioActual, codigoModulo);

            if (!string.IsNullOrWhiteSpace(tabla))
                nombreTabla = tabla.Trim();

            CablearBotones();

            Load += (s, e) => grid.Ocultar();
            Resize += (s, e) => Posicionar();
        }

        private void CablearBotones()
        {
            btnIngresar.Click += btnIngresar_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnConsultar.Click += btnConsultar_Click;
            btnRefrescar.Click += btnRefrescar_Click;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnSalir.Click += (s, e) => Close();

            btnInicio.Click += (s, e) => grid.Inicio();
            btnAnterior.Click += (s, e) => grid.Anterior();
            btnSiguiente.Click += (s, e) => grid.Siguiente();
            btnFin.Click += (s, e) => grid.Fin();
        }

        private void ConsultarTabla()
        {
            try
            {
                DataTable datos = ctrlTabla.llenarDgv(nombreTabla);
                esquemaActual = selectorLlave.ObtenerEsquemaConLlaves(nombreTabla);

                grid.Mostrar(datos);
                Posicionar();

                Text = "CRUD - " + nombreTabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show(acciones.MensajeAmigable(ex), "Error al consultar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Posicionar()
        {
            int inicio = ObtenerInicioContenido();
            int posicionY = formulario.Visible ? formulario.Bottom + 10 : inicio;
            grid.Posicionar(posicionY);
        }

        private int ObtenerInicioContenido()
        {
            int maxBottom = 0;

            foreach (Control c in Controls)
            {
                if (c is Button && c.Bottom > maxBottom)
                {
                    maxBottom = c.Bottom;
                }
            }

            return maxBottom + 15;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!seguridad.TieneAcceso()) return;

            ConsultarTabla();
            pkModificar = null;

            formulario.Abrir(nombreTabla, esquemaActual, false, null, grid, ObtenerInicioContenido());
            Posicionar();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (!seguridad.TieneAcceso()) return;

            formulario.Cerrar();
            ConsultarTabla();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            if (!seguridad.TieneAcceso()) return;

            formulario.Cerrar();
            grid.Ocultar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!seguridad.TieneAcceso()) return;

            DataGridViewRow fila = grid.Grid != null ? grid.Grid.CurrentRow : null;

            if (fila == null || fila.IsNewRow)
            {
                MessageBox.Show("Seleccione un registro en la tabla para modificar.", "Modificar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            esquemaActual = selectorLlave.ObtenerEsquemaConLlaves(nombreTabla);
            pkModificar = grid.ObtenerClavesPrimarias(esquemaActual, fila);

            if (pkModificar.Count == 0)
            {
                MessageBox.Show("No se pudo obtener la llave primaria del registro seleccionado.", "Modificar registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            formulario.Abrir(nombreTabla, esquemaActual, true, fila, grid, ObtenerInicioContenido());
            Posicionar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!seguridad.TieneAcceso()) return;

            DataGridViewRow fila = grid.Grid != null ? grid.Grid.CurrentRow : null;

            if (fila == null || fila.IsNewRow)
            {
                MessageBox.Show("Seleccione un registro en la tabla para eliminar.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Dictionary<string, string> pk = grid.ObtenerClavesPrimarias(esquemaActual, fila);
            string mensaje;

            try
            {
                if (acciones.Eliminar(nombreTabla, pk, out mensaje))
                {
                    MessageBox.Show("Registro eliminado correctamente.", "Eliminación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ConsultarTabla();
                }
                else if (!string.IsNullOrEmpty(mensaje))
                {
                    MessageBox.Show(mensaje, "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(acciones.MensajeAmigable(ex), "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!formulario.Visible)
            {
                MessageBox.Show("Abra un registro con Ingresar o Modificar antes de guardar.", "Guardar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Dictionary<string, string> datos = formulario.ObtenerDatos();
            string mensaje;

            try
            {
                if (acciones.Guardar(nombreTabla, esquemaActual, datos, formulario.ModoModificar, pkModificar, out mensaje))
                {
                    MessageBox.Show("Registro guardado correctamente.", "Guardado exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formulario.Cerrar();
                    pkModificar = null;
                    ConsultarTabla();
                }
                else if (!string.IsNullOrEmpty(mensaje))
                {
                    MessageBox.Show(mensaje, "Guardar registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(acciones.MensajeAmigable(ex), "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            formulario.Cerrar();
            pkModificar = null;
            Posicionar();
        }
    }
}