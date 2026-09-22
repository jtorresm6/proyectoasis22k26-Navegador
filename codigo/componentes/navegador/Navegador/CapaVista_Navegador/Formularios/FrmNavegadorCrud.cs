using System;
using System.ComponentModel;
using System.Windows.Forms;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Navegador.formularios;

// Inicio cambio - Gabriel André Guillén Pocón - 0901-23-1998
namespace CapaVista_Navegador
{
    // Formulario base del CRUD. Ya trae insertado el control Navegador (la barra de botones, arrastrado
    // desde la caja de herramientas como cualquier botón) y arma la tabla (grid), el panel de
    // ingreso/modificación, los permisos por rol y la bitácora.
    //
    // El programador que consume el navegador solo llama a NavegadorMetConfigurar con 3 datos:
    //   1) Tabla ............ sobre la que se hace el CRUD.
    //   2) CodigoAplicacion . con el que Seguridad valida al usuario y le da/quita permisos.
    //   3) RutaAyuda ........ archivo .chm que abre el botón Ayuda para esa tabla.
    // El usuario NO se parametriza: se obtiene automáticamente de la sesión activa
    // (ClsSesionSeguridad, del componente Seguridad), que es quien realmente sabe quién inició sesión.
    public partial class FrmNavegadorCrud : Form
    {
        private ClsCrudEventos _Eventos;

        private string _NombreTabla = "tblaplicacion";
        private int _CodigoAplicacion = 4;
        private string _RutaAyuda = "";

        public FrmNavegadorCrud()
        {
            InitializeComponent();

            navegador1.NavegadorAccionSolicitada += NavegadorMetEjecutarAccion;
        }

        // Único método de configuración: tabla, código de aplicación (Seguridad) y ruta de ayuda.
        public void NavegadorMetConfigurar(string Tabla, int CodigoAplicacion, string RutaAyuda)
        {
            _CodigoAplicacion = CodigoAplicacion;
            this.RutaAyuda = RutaAyuda;
            NombreTabla = Tabla;
        }

        [Category("Navegador")]
        [Description("Ruta del archivo de ayuda (.chm) que abre el botón Ayuda para esta tabla.")]
        public string RutaAyuda
        {
            get { return _RutaAyuda; }
            set { _RutaAyuda = value; }
        }

        [Category("Navegador")]
        [Description("Nombre de la tabla sobre la que se hace el CRUD.")]
        public string NombreTabla
        {
            get { return _NombreTabla; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                _NombreTabla = value.Trim();
                navegador1.NavegadorMetCambiarTabla(_NombreTabla);

                if (_Eventos != null)
                {
                    _Eventos.NombreTabla = _NombreTabla;
                    _Eventos.NavegadorMetConsultar();
                }
            }
        }

        // Cuando el formulario se abre en ejecución se arma el CRUD.
        // En el diseñador de Visual Studio no se hace nada (no debe tocar la base de datos).
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (NavegadorFuncEsDiseno())
            {
                return;
            }

            // El usuario no lo parametriza quien usa el navegador: se lee de la sesión activa que dejó
            // Seguridad al iniciar sesión (ver ClsSesionSeguridad / ClsSesionPrueba mientras no hay login real).
            string UsuarioActual = ClsSesionSeguridad.NombreUsuario;

            _Eventos = new ClsCrudEventos(this, _NombreTabla, UsuarioActual, _NombreTabla);

            // CodigoAplicacion se usa como Módulo e IdAplicacion de Seguridad: un único código por
            // tabla es suficiente para validar al usuario y aplicar sus permisos (ver ClsCrudSeguridad).
            navegador1.NavegadorMetConfigurar(_NombreTabla, UsuarioActual, _NombreTabla, _CodigoAplicacion, _CodigoAplicacion);

            _Eventos.NavegadorMetCargar();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_Eventos != null)
            {
                _Eventos.NavegadorMetPosicionar();
            }
        }

        private bool NavegadorFuncEsDiseno()
        {
            return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        // Lo que hace cada botón del navegador.
        private void NavegadorMetEjecutarAccion(string Accion)
        {
            if (_Eventos == null)
            {
                return;
            }

            switch (Accion)
            {
                case "INGRESAR":
                    _Eventos.NavegadorMetIngresar();
                    break;
                case "CONSULTAR":
                    _Eventos.NavegadorMetConsultar();
                    break;
                case "MODIFICAR":
                    _Eventos.NavegadorMetModificar();
                    break;
                case "ELIMINAR":
                    _Eventos.NavegadorMetEliminar();
                    break;
                case "REFRESCAR":
                    _Eventos.NavegadorMetRefrescar();
                    break;
                case "GUARDAR":
                    _Eventos.NavegadorMetGuardar();
                    break;
                case "CANCELAR":
                    _Eventos.NavegadorMetCancelar();
                    break;
                case "INICIO":
                    _Eventos.NavegadorMetInicio();
                    break;
                case "ANTERIOR":
                    _Eventos.NavegadorMetAnterior();
                    break;
                case "SIGUIENTE":
                    _Eventos.NavegadorMetSiguiente();
                    break;
                case "FIN":
                    _Eventos.NavegadorMetFin();
                    break;
                case "IMPRIMIR":
                    _Eventos.NavegadorMetImprimir();
                    break;
                case "AYUDA":
                    NavegadorMetMostrarAyuda();
                    break;
                case "SALIR":
                    Form Padre = FindForm();

                    if (Padre != null)
                    {
                        Padre.Close();
                    }
                    break;
            }
        }

        // AYUDA: abre el archivo .chm (HTML Help) parametrizado en RutaAyuda. Cada programador que use
        // el navegador coloca aquí la ruta de su propio archivo de ayuda (ver capacitación de Ayudas).
        private void NavegadorMetMostrarAyuda()
        {
            if (string.IsNullOrWhiteSpace(RutaAyuda))
            {
                MessageBox.Show(
                    "No se configuró la ruta del archivo de ayuda (propiedad RutaAyuda) para este formulario.",
                    "Ayuda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            try
            {
                Help.ShowHelp(this, RutaAyuda);
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(
                    "No se pudo abrir el archivo de ayuda '" + RutaAyuda + "'.\n\n" + Excepcion.Message,
                    "Ayuda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
// Fin cambio - Gabriel André Guillén Pocón - 0901-23-1998
