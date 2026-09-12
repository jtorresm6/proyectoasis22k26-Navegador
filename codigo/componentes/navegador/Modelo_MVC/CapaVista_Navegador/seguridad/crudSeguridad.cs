using System;
using System.Windows.Forms;
using CapaControlador_Navegador; // <- esta es la unica capa que crudSeguridad debe conocer

namespace CapaVista_Navegador
{
    // Envuelve la validacion de permisos para no repetir el try/catch en cada boton
    public class crudSeguridad
    {
        private string usuario;
        private string modulo;

        // IMPORTANTE: aqui va ctrlPermiso (Controlador), NUNCA "permisos" (Modelo).
        // La Vista no debe conocer clases de CapaModelo_Navegador.
        private ctrlPermiso permisos = new ctrlPermiso();

        public crudSeguridad(string usuario, string modulo)
        {
            this.usuario = usuario;
            this.modulo = modulo;
        }

        public bool TieneAcceso()
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(modulo))
            {
                return true;
            }

            try
            {
                return permisos.ValidarAcceso(usuario, modulo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al validar los permisos: " + ex.Message,
                    "Error de seguridad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }
        }
    }
}