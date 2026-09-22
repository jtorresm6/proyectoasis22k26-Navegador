using System;
using System.Windows.Forms;

namespace CapaVista_Navegador.formularios
{
    public class ClsCrudEventos
    {
        private readonly ClsCrudCoordinador _Coordinador;

        public ClsCrudEventos(
            Control Formulario,
            string Tabla,
            string UsuarioActual,
            string CodigoModulo)
        {
            _Coordinador = new ClsCrudCoordinador(
                Formulario,
                Tabla,
                UsuarioActual,
                CodigoModulo);
        }

        // Inicio cambio - Gabriel André Guillén Pocón - 0901-23-1998
        // Nombre de la tabla sobre la que trabaja el CRUD; lo cambia quien usa el navegador.
        public string NombreTabla
        {
            get { return _Coordinador.NombreTabla; }
            set { _Coordinador.NombreTabla = value; }
        }
        // Fin cambio - Gabriel André Guillén Pocón - 0901-23-1998

        public void NavegadorMetCargar()
        {
            _Coordinador.NavegadorMetOcultarGrid();
        }

        public void NavegadorMetIngresar()
        {
            _Coordinador.NavegadorMetIngresar();
        }

        public void NavegadorMetConsultar()
        {
            _Coordinador.NavegadorMetConsultar();
        }

        public void NavegadorMetRefrescar()
        {
            _Coordinador.NavegadorMetRefrescar();
        }

        public void NavegadorMetModificar()
        {
            _Coordinador.NavegadorMetModificar();
        }

        public void NavegadorMetEliminar()
        {
            _Coordinador.NavegadorMetEliminar();
        }

        public void NavegadorMetGuardar()
        {
            _Coordinador.NavegadorMetGuardar();
        }

        public void NavegadorMetCancelar()
        {
            _Coordinador.NavegadorMetCancelar();
        }

        public void NavegadorMetInicio()
        {
            _Coordinador.NavegadorMetInicio();
        }

        public void NavegadorMetAnterior()
        {
            _Coordinador.NavegadorMetAnterior();
        }

        public void NavegadorMetSiguiente()
        {
            _Coordinador.NavegadorMetSiguiente();
        }

        public void NavegadorMetFin()
        {
            _Coordinador.NavegadorMetFin();
        }

        public void NavegadorMetImprimir()
        {
            _Coordinador.NavegadorMetImprimir();
        }

        public void NavegadorMetPosicionar()
        {
            _Coordinador.NavegadorMetPosicionar();
        }
    }
}