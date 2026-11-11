/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Entidad que coincide con la tabla tblCuenta: código, nombre,
 *  naturaleza, cuenta padre y estado.
 * Reglas especificas:
 *  Los campos de tipo, padre y estado los agregó Oskar Cermeño; aquí
 *  se ajustaron a los nombres de EST-01 (is_active como bool).
 * ===================================================================
*/

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Entidades
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsCuentaContabilidad
    {
        public int IdCuenta { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int IdPadre { get; set; }
        public bool Activo { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
