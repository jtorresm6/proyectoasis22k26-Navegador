using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Seguridad.Objetos_de_valor;

namespace CapaVista_Navegador
{
    public partial class Navegador : UserControl
    {
        public event Action<string> NavegadorAccionSolicitada;
        private string _Tabla;
        private string _Usuario;
        private string _Modulo;
        // Módulo y Aplicación de Seguridad con los que se buscan los permisos. 0 = sin seguridad.
        private int _IdModulo;
        private int _IdAplicacion;
        private bool _SeguridadAplicada;
        public Navegador()
        {
            InitializeComponent();

            NavegadorMetCablearBotones();
        }
        // CONFIGURAR NAVEGADOR (sin seguridad: no habilita ni deshabilita botones)
        public void NavegadorMetConfigurar(
            string Tabla,
            string Usuario,
            string Modulo)
        {
            NavegadorMetConfigurar(Tabla, Usuario, Modulo, 0, 0);
        }
        // CONFIGURAR NAVEGADOR CON SEGURIDAD
        // IdModulo e IdAplicacion son los de Seguridad; con ellos se leen los permisos del usuario en sesión.
        public void NavegadorMetConfigurar(
            string Tabla,
            string Usuario,
            string Modulo,
            int IdModulo,
            int IdAplicacion)
        {
            _Tabla = Tabla;
            _Usuario = Usuario;
            _Modulo = Modulo;
            _IdModulo = IdModulo;
            _IdAplicacion = IdAplicacion;
            _SeguridadAplicada = false;

            // Si el control ya está dentro de un formulario se aplican de una vez;
            // si no, se aplican cuando cargue (OnLoad).
            NavegadorMetAplicarSeguridad();
        }
        // Aplica los permisos cuando el control ya tiene un formulario padre.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            NavegadorMetAplicarSeguridad();
        }
        // Deshabilita o habilita los botones según el rol del usuario en sesión.
        // ClsSeguridadFormHelper necesita un Form, y FindForm() devuelve null mientras el control
        // no esté agregado a uno (por ejemplo en el constructor), por eso se espera a OnLoad.
        private void NavegadorMetAplicarSeguridad()
        {
            if (_SeguridadAplicada || _IdModulo <= 0 || _IdAplicacion <= 0)
                return;
            Form Formulario = FindForm();
            if (Formulario == null)
                return;
            _SeguridadAplicada = true;

            // "Este botón necesita este permiso para poder usarse".
            // Guardar no se mapea porque se usa tanto para insertar como para modificar; el filtro
            // real ya pasó al poder abrir el panel con Ingresar o con Modificar.
            var MapaBotones = new Dictionary<Control, TipoPermiso>
            {
                { NavegadorBtnIngresar, TipoPermiso.Insertar },
                { NavegadorBtnModificar, TipoPermiso.Editar },
                { NavegadorBtnEliminar, TipoPermiso.Eliminar },
                { NavegadorBtnImprimir, TipoPermiso.Imprimir },
            };
            new ClsCrudSeguridad(_IdModulo, _IdAplicacion)
                .NavegadorMetAplicarPermisos(Formulario, MapaBotones);
        }
        // CAMBIAR TABLA
        public void NavegadorMetCambiarTabla(string Tabla)
        {
            if (!string.IsNullOrWhiteSpace(Tabla))
            {
                _Tabla = Tabla.Trim();
            }
        }
        //OBTENER TABLA
        public string NavegadorFuncObtenerTabla()
        {
            return _Tabla;
        }
        // CABLEAR BOTONES
        private void NavegadorMetCablearBotones()
        {
            NavegadorBtnIngresar.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("INGRESAR");

            NavegadorBtnConsultar.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("CONSULTAR");

            NavegadorBtnModificar.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("MODIFICAR");

            NavegadorBtnEliminar.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("ELIMINAR");

            NavegadorBtnRefrescar.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("REFRESCAR");

            NavegadorBtnGuardar.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("GUARDAR");

            NavegadorBtnCancelar.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("CANCELAR");

            NavegadorBtnInicio.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("INICIO");

            NavegadorBtnAnterior.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("ANTERIOR");

            NavegadorBtnSiguiente.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("SIGUIENTE");

            NavegadorBtnFin.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("FIN");

            NavegadorBtnImprimir.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("IMPRIMIR");

            NavegadorBtnAyuda.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("AYUDA");

            NavegadorBtnSalir.Click +=
                (Sender, Evento) =>
                NavegadorMetSolicitarAccion("SALIR");
        }
        // SOLICITAR ACCIÓN AL FORMULARIO
        private void NavegadorMetSolicitarAccion(string Accion)
        {
            NavegadorAccionSolicitada?.Invoke(Accion);
        }
    }
}