/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Representa una fila de la tabla cuenta (id_cuenta, nombre) del
 *  esquema presupuesto_db.
 * ===================================================================
*/

namespace CapaModelo_Contabilidad
{
    public class ClsCuentaContabilidad
    {
        public int IdCuenta { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
