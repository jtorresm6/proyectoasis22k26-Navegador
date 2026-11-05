// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Cuenta tal como la necesita la Vista para elegirla en una póliza.
 *  Es el tipo que expone el Controlador, de modo que la Vista no
 *  depende de las clases del Modelo.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public class ClsModeloCuentaPolizaContabilidad
    {
        public int IdCuenta { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get { return Codigo + " - " + Nombre; } }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
