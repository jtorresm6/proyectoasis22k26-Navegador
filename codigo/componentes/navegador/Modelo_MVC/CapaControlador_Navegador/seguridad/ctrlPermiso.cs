using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a validar el acceso de un usuario a un modulo
    public class ctrlPermiso
    {
        private permisos permisos = new permisos();

        public bool ValidarAcceso(string usuario, string modulo)
        {
            try
            {
                return permisos.ValidarAcceso(usuario, modulo);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error al validar los permisos del usuario '" +
                    usuario +
                    "' sobre el módulo '" +
                    modulo +
                    "': " +
                    ex.Message,
                    ex);
            }
        }
    }
}