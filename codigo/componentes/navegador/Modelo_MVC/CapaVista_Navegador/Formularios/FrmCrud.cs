using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    public partial class FrmCrud : Form
    {
        // CAMBIAR AQUÍ MANUALMENTE LA TABLA A LA QUE SE DESEA HACER CRUD
        private string _NombreTabla = "tblempleado";

        private ClsCtrlTabla _CtrlTabla = new ClsCtrlTabla();
        private ClsCrudGrid _Grid;
        private ClsCrudFormulario _Formulario;
        private ClsCrudAcciones _Acciones = new ClsCrudAcciones();
        private ClsSelectorLlave _SelectorLlave;
        private ClsCrudSeguridad _Seguridad;

        private List<ClsColumnaInfo> _EsquemaActual;
        private Dictionary<string, string> _PkModificar;

        public string NombreTabla
        {
            get { return _NombreTabla; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _NombreTabla = value.Trim();
                    NavegadorFuncConsultarTabla();
                }
            }
        }

        public FrmCrud() : this("USUARIO_PRUEBA", "EMPLEADOS", null) { }

        public FrmCrud(string UsuarioActual, string CodigoModulo) : this(UsuarioActual, CodigoModulo, null) { }

        public FrmCrud(string UsuarioActual, string CodigoModulo, string Tabla)
        {
            InitializeComponent();

            _Grid = new ClsCrudGrid(this);
            _Formulario = new ClsCrudFormulario(this);
            _SelectorLlave = new ClsSelectorLlave(this);
            _Seguridad = new ClsCrudSeguridad(UsuarioActual, CodigoModulo);

            if (!string.IsNullOrWhiteSpace(Tabla))
                _NombreTabla = Tabla.Trim();

            NavegadorMetCablearBotones();

            Load += (Origen, Evento) => _Grid.NavegadorMetOcultar();
            Resize += (Origen, Evento) => NavegadorMetPosicionar();
        }

        private void NavegadorMetCablearBotones()
        {
            NavegadorBtnIngresar.Click += NavegadorMetIngresarClick;
            NavegadorBtnCancelar.Click += NavegadorMetCancelarClick;
            NavegadorBtnConsultar.Click += NavegadorMetConsultarClick;
            NavegadorBtnRefrescar.Click += NavegadorMetRefrescarClick;
            NavegadorBtnModificar.Click += NavegadorMetModificarClick;
            NavegadorBtnEliminar.Click += NavegadorMetEliminarClick;
            NavegadorBtnGuardar.Click += NavegadorMetGuardarClick;
            NavegadorBtnSalir.Click += (Origen, Evento) => Close();

            NavegadorBtnInicio.Click += (Origen, Evento) => _Grid.NavegadorMetInicio();
            NavegadorBtnAnterior.Click += (Origen, Evento) => _Grid.NavegadorMetAnterior();
            NavegadorBtnSiguiente.Click += (Origen, Evento) => _Grid.NavegadorMetSiguiente();
            NavegadorBtnFin.Click += (Origen, Evento) => _Grid.NavegadorMetFin();
        }

        private bool NavegadorFuncConsultarTabla()
        {
            try
            {
                DataTable Datos = _CtrlTabla.NavegadorFuncLlenarDgv(_NombreTabla);
                _EsquemaActual = _SelectorLlave.NavegadorFuncObtenerEsquemaConLlaves(_NombreTabla);

                _Grid.NavegadorMetMostrar(Datos);
                NavegadorMetPosicionar();

                Text = "1001 – Crud " + _NombreTabla;
                return true;
            }
            catch (Exception Excepcion)
            {
                _EsquemaActual = null;
                _Formulario.NavegadorMetCerrar();
                _Grid.NavegadorMetOcultar();
                MessageBox.Show(
                    _Acciones.NavegadorFuncMensajeAmigable(Excepcion),
                    "Error al consultar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        private void NavegadorMetPosicionar()
        {
            int Inicio = NavegadorFuncObtenerInicioContenido();
            int PosicionY = _Formulario.Visible ? _Formulario.Bottom + 10 : Inicio;
            _Grid.NavegadorMetPosicionar(PosicionY);
        }

        private int NavegadorFuncObtenerInicioContenido()
        {
            int MaxBottom = 0;

            foreach (Control ControlActual in Controls)
            {
                if (ControlActual is Button && ControlActual.Bottom > MaxBottom)
                {
                    MaxBottom = ControlActual.Bottom;
                }
            }

            return MaxBottom + 15;
        }

        private void NavegadorMetIngresarClick(object Sender, EventArgs Evento)
        {
            if (!_Seguridad.NavegadorFuncTieneAcceso()) return;

            if (!NavegadorFuncConsultarTabla())
                return;

            _PkModificar = null;

            if (_EsquemaActual == null || _EsquemaActual.Count == 0)
            {
                MessageBox.Show(
                    "No se pudo obtener la estructura de la tabla indicada.",
                    "Ingresar registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _Formulario.NavegadorMetAbrir(
                _NombreTabla,
                _EsquemaActual,
                false,
                null,
                _Grid,
                NavegadorFuncObtenerInicioContenido());

            NavegadorMetPosicionar();
        }

        private void NavegadorMetConsultarClick(object Sender, EventArgs Evento)
        {
            if (!_Seguridad.NavegadorFuncTieneAcceso()) return;

            _Formulario.NavegadorMetCerrar();
            NavegadorFuncConsultarTabla();
        }

        private void NavegadorMetRefrescarClick(object Sender, EventArgs Evento)
        {
            if (!_Seguridad.NavegadorFuncTieneAcceso()) return;

            _Formulario.NavegadorMetCerrar();
            _Grid.NavegadorMetOcultar();
        }

        private void NavegadorMetModificarClick(object Sender, EventArgs Evento)
        {
            if (!_Seguridad.NavegadorFuncTieneAcceso()) return;

            DataGridViewRow Fila = _Grid.NavegadorDgvDatos != null
                ? _Grid.NavegadorDgvDatos.CurrentRow
                : null;

            if (Fila == null || Fila.IsNewRow)
            {
                MessageBox.Show(
                    "Seleccione un registro en la tabla para Modificar.",
                    "Modificar registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            _EsquemaActual = _SelectorLlave.NavegadorFuncObtenerEsquemaConLlaves(_NombreTabla);
            _PkModificar = _Grid.NavegadorFuncObtenerClavesPrimarias(_EsquemaActual, Fila);

            if (_PkModificar.Count == 0)
            {
                MessageBox.Show(
                    "No se pudo obtener la llave primaria del registro seleccionado.",
                    "Modificar registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _Formulario.NavegadorMetAbrir(
                _NombreTabla,
                _EsquemaActual,
                true,
                Fila,
                _Grid,
                NavegadorFuncObtenerInicioContenido());

            NavegadorMetPosicionar();
        }

        private void NavegadorMetEliminarClick(object Sender, EventArgs Evento)
        {
            if (!_Seguridad.NavegadorFuncTieneAcceso()) return;

            DataGridViewRow Fila = _Grid.NavegadorDgvDatos != null
                ? _Grid.NavegadorDgvDatos.CurrentRow
                : null;

            if (Fila == null || Fila.IsNewRow)
            {
                MessageBox.Show(
                    "Seleccione un registro en la tabla para eliminar.",
                    "Eliminar registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            Dictionary<string, string> ClavesPrimarias =
                _Grid.NavegadorFuncObtenerClavesPrimarias(_EsquemaActual, Fila);

            string Mensaje;

            try
            {
                if (_Acciones.NavegadorFuncEliminar(
                    _NombreTabla,
                    ClavesPrimarias,
                    out Mensaje))
                {
                    MessageBox.Show(
                        "Registro eliminado correctamente.",
                        "Eliminación exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    NavegadorFuncConsultarTabla();
                }
                else if (!string.IsNullOrEmpty(Mensaje))
                {
                    MessageBox.Show(
                        Mensaje,
                        "Eliminar registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(
                    _Acciones.NavegadorFuncMensajeAmigable(Excepcion),
                    "Error al eliminar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void NavegadorMetGuardarClick(object Sender, EventArgs Evento)
        {
            if (!_Formulario.Visible)
            {
                MessageBox.Show(
                    "Abra un registro con Ingresar o Modificar antes de guardar.",
                    "Guardar registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            Dictionary<string, string> Datos =
                _Formulario.NavegadorFuncObtenerDatos();

            string Mensaje;

            try
            {
                if (_Acciones.NavegadorFuncGuardar(
                    _NombreTabla,
                    _EsquemaActual,
                    Datos,
                    _Formulario.ModoModificar,
                    _PkModificar,
                    out Mensaje))
                {
                    MessageBox.Show(
                        "Registro guardado correctamente.",
                        "Guardado exitoso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _Formulario.NavegadorMetCerrar();
                    _PkModificar = null;
                    NavegadorFuncConsultarTabla();
                }
                else if (!string.IsNullOrEmpty(Mensaje))
                {
                    MessageBox.Show(
                        Mensaje,
                        "Guardar registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(
                    _Acciones.NavegadorFuncMensajeAmigable(Excepcion),
                    "Error al guardar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void NavegadorMetCancelarClick(object Sender, EventArgs Evento)
        {
            _Formulario.NavegadorMetCerrar();
            _PkModificar = null;
            NavegadorMetPosicionar();
        }
    }
}