using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_Seguridad.Objetos_de_valor;

// Inicio cambio - Gabriel André Guillén Pocón - 0901-23-1998
namespace CapaVista_Navegador
{
    // Control único del componente Navegador: se arrastra desde la caja de herramientas a CUALQUIER
    // formulario (sin heredar de nada) y se configura con una sola línea:
    //
    //     navegador1.NavegadorMetConfigurar("tblempleado", 4, 5);   // tabla, IdModulo, IdAplicacion
    //
    // Al cargar el formulario arma la grilla, el panel de ingreso/modificación, los permisos del
    // usuario en sesión (Seguridad) y la bitácora. El usuario y sus roles NO se parametrizan: salen de
    // la sesión que dejó el login de Seguridad (ver ClsNavegadorSesion).
    // Inicio cambio - Evelyn Sofía Andrade Luna - 9959-23-1224 - 26/10/2026
    // La ayuda sale de la aplicación configurada (IdAplicacion): el botón Ayuda abre la página de esa
    // aplicación dentro del .chm de su módulo (ver ClsNavegadorAyuda).
    // Fin cambio - Evelyn Sofía Andrade Luna - 9959-23-1224 - 26/10/2026
    // Lo que hace cada botón está en ClsNavegadorAcciones.
    public partial class Navegador : UserControl
    {
        public event Action<string> NavegadorAccionSolicitada;
        private string _Tabla;
        private int _IdModulo;
        private int _IdAplicacion;
        private Dictionary<string, object> _FuentesDeDatosReporteador = new Dictionary<string, object>();
        private bool _Cargado;
        private ClsNavegadorAcciones _Acciones;
        private int _AltoBase;
        private readonly Panel _PnlDetalle = new Panel();

        public Navegador()
        {
            InitializeComponent();
            NavegadorMetCablearBotones();
            NavegadorMetCrearDetalle();
        }

        // Área desplegable debajo de la barra de botones: ahí se muestran la tabla (DataGridView) y el
        // formulario de Ingresar/Modificar. Empieza oculta y el control mide solo la barra.
        internal Panel NavegadorPnlDetalle
        {
            get { return _PnlDetalle; }
        }

        internal int NavegadorIdAplicacion
        {
            get { return _IdAplicacion; }
        }

        internal Dictionary<string, object> NavegadorFuentesDeDatosReporteador
        {
            get { return _FuentesDeDatosReporteador; }
        }

        private void NavegadorMetCrearDetalle()
        {
            _AltoBase = Height;

            _PnlDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _PnlDetalle.BackColor = Color.FromArgb(242, 233, 217);
            _PnlDetalle.BorderStyle = BorderStyle.FixedSingle;
            _PnlDetalle.Visible = false;

            Controls.Add(_PnlDetalle);
        }

        // Expande el Navegador hacia abajo con Alto de espacio para el área desplegable y lo pone al frente
        // para que cubra lo que haya debajo en el formulario o MDI que lo contiene.
        internal void NavegadorMetMostrarDetalle(int Alto)
        {
            _PnlDetalle.SetBounds(0, _AltoBase, Width, Alto);
            _PnlDetalle.Visible = true;
            Height = _AltoBase + Alto;
            BringToFront();
        }

        // Contrae el Navegador: vuelve a medir solo la barra de botones.
        internal void NavegadorMetOcultarDetalle()
        {
            _PnlDetalle.Visible = false;
            Height = _AltoBase;
        }

        // CONFIGURAR NAVEGADOR (con seguridad)
        // Único método de configuración: la tabla del CRUD y el Módulo y la Aplicación de Seguridad
        // con los que se buscan los permisos del usuario (tblrolmoduloaplicacion).
        public void NavegadorMetConfigurar(string Tabla, int IdModulo, int IdAplicacion, Dictionary<string, object> FuentesDeDatosReporteador = null)
        {
            _Tabla = Tabla;
            _IdModulo = IdModulo;
            _IdAplicacion = IdAplicacion;
            _FuentesDeDatosReporteador = FuentesDeDatosReporteador ?? new Dictionary<string, object>();

            // Si el formulario ya está abierto se aplica de una vez; si no, en OnLoad.
            if (_Cargado)
            {
                NavegadorMetArmar();
            }
        }

        // CAMBIAR TABLA
        public void NavegadorMetCambiarTabla(string Tabla)
        {
            if (!string.IsNullOrWhiteSpace(Tabla))
            {
                _Tabla = Tabla.Trim();

                // Inicio cambio - Gabriel André Guillén Pocón - 0901-23-1998
                // Si el CRUD ya está armado, lo actualiza con la nueva tabla.
                if (_Acciones != null)
                {
                    _Acciones.NavegadorMetCambiarTabla(_Tabla);
                }
                // Fin cambio - Gabriel André Guillén Pocón - 0901-23-1998
            }
        }

        //OBTENER TABLA
        public string NavegadorFuncObtenerTabla()
        {
            return _Tabla;
        }

        // FindForm() devuelve null en el constructor del formulario, por eso se espera a OnLoad.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _Cargado = !NavegadorFuncEsDiseno();
            NavegadorMetArmar();
        }

        // Aplica los permisos y arma (o actualiza) el CRUD en el formulario que contiene al control.
        private void NavegadorMetArmar()
        {
            Form Padre = FindForm();

            if (!_Cargado || Padre == null || string.IsNullOrWhiteSpace(_Tabla))
            {
                return;
            }

            new ClsCrudSeguridad(_IdModulo, _IdAplicacion)
                .NavegadorMetAplicarPermisos(Padre, NavegadorFuncMapaPermisos());

            if (_Acciones == null)
            {
                _Acciones = new ClsNavegadorAcciones(this, _Tabla);
            }
            else
            {
                _Acciones.NavegadorMetCambiarTabla(_Tabla);
            }
        }

        // "Este botón necesita este permiso para poder usarse". Guardar no se mapea porque sirve para
        // insertar y para modificar; el filtro real ya pasó al abrir el panel con Ingresar o Modificar.
        private Dictionary<Control, TipoPermiso> NavegadorFuncMapaPermisos()
        {
            return new Dictionary<Control, TipoPermiso>
            {
                { NavegadorBtnIngresar, TipoPermiso.Insertar },
                { NavegadorBtnModificar, TipoPermiso.Editar },
                { NavegadorBtnEliminar, TipoPermiso.Eliminar },
                { NavegadorBtnImprimir, TipoPermiso.Imprimir },
            };
        }

        // En el diseñador de Visual Studio no se debe tocar la base de datos.
        private bool NavegadorFuncEsDiseno()
        {
            return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        // Inicio cambio - Gabriel André Guillén Pocón - 0901-23-1998
        // Se conserva el evento original NavegadorAccionSolicitada (lo puede escuchar quien use el
        // control) y ahora, además, la acción se ejecuta con ClsNavegadorAcciones si el CRUD ya está armado.
        // Fin cambio - Gabriel André Guillén Pocón - 0901-23-1998
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

            if (_Acciones != null)
            {
                _Acciones.NavegadorMetEjecutar(Accion);
            }
        }
    }
}
// Fin cambio - Gabriel André Guillén Pocón - 0901-23-1998