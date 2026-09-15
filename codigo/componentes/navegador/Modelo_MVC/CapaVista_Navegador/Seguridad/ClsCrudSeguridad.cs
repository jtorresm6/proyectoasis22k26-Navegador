using System;
using System.Windows.Forms;
using CapaControlador_Navegador; // <- esta es la unica capa que crudSeguridad debe conocer


//-----------------------------------------------------
// - Hecho por: Natali Sofía Montenegro Portillo 
// - Ultima modificación: 14/09/2026
// - Carne: 0901-23-10017 
namespace CapaVista_Navegador
{
    public class ClsCrudSeguridad
    {
        private string _Usuario;
        private string _Modulo;   //recibe datos

        // IMPORTANTE: aqui va ctrlPermiso (Controlador), NUNCA "permisos" (Modelo).
        // La Vista no debe conocer clases de CapaModelo_Navegador.
        private ClsCtrlPermiso _Permisos = new ClsCtrlPermiso();

        public ClsCrudSeguridad(string Usuario, string Modulo)
        {
            this._Usuario = Usuario;
            this._Modulo = Modulo; //guardar info de controlado
        }

        public bool NavegadorFuncTieneAcceso()
        {
            if (string.IsNullOrEmpty(_Usuario) || string.IsNullOrEmpty(_Modulo))
            {
                return true;  //verifica si no están vacios los campos
            }

            //validaciones por try y catch
            try
            {
                return _Permisos.NavegadorFuncValidarAcceso(_Usuario, _Modulo);   //llama a la funcion de validar acceso
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(
                    "Error al validar los permisos: " + Excepcion.Message,
                    "Error de seguridad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);  //si no encuentra permisos, muestra error

                return false;
            }
        }
    }
}

//--------------------------------------------------
// - Final: Natali Sofía Montenegro Portillo 
// - Carne: 0901-23-10017 
// Clase que permite la validacion de permisos de un usuario sobre un módulo específico. 