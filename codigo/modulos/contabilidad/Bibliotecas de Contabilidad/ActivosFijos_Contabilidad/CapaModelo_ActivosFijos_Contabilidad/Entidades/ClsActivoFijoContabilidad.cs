/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Entidades de activos fijos: la ficha del activo y el maestro de
 *  vida útil por tipo de activo.
 * ===================================================================
*/

using System;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Entidades
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsActivoFijoContabilidad
    {
        public int IdActivo { get; set; }
        public string Descripcion { get; set; }
        public string Centro { get; set; }
        public string Modulo { get; set; }
        public string Tipo { get; set; }
        public DateTime Adquisicion { get; set; }
        public DateTime Inicio { get; set; }
        public decimal Compra { get; set; }
        public decimal Residual { get; set; }
        public int Meses { get; set; }
    }

    public class ClsVidaUtilContabilidad
    {
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public int Meses { get; set; }
    }
}
