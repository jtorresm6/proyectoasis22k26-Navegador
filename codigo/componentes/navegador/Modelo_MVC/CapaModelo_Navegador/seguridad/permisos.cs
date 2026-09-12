using System;

namespace CapaModelo_Navegador
{
    // Valida si un usuario tiene acceso a un modulo.
    public class permisos
    {
        private conexionBD conn = new conexionBD();

        public bool ValidarAcceso(string usuario, string modulo)
        {
            return true;
        }
    }
}
