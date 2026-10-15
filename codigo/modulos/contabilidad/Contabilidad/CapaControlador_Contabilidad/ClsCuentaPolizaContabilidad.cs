/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
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
    public class ClsCuentaPolizaContabilidad
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
