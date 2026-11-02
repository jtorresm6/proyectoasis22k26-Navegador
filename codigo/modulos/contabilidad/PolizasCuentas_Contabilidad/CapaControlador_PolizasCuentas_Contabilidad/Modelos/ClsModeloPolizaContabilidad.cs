/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Modelo de la póliza con sus validaciones.
 * Reglas especificas:
 *  Una póliza es válida si tiene descripción, al menos dos líneas,
 *  cada línea tiene importe solo en el debe o solo en el haber, y el
 *  total del debe es igual al del haber y mayor que cero.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaControlador_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsModeloPolizaContabilidad : IValidatableObject
    {
        public int IdPoliza { get; set; }

        [MaxLength(40, ErrorMessage = "El código de la póliza admite hasta 40 caracteres.")]
        public string CodigoPoliza { get; set; }

        public string Periodo { get; set; }

        [Required(ErrorMessage = "Escriba la descripción de la póliza.")]
        [MaxLength(255, ErrorMessage = "La descripción admite hasta 255 caracteres.")]
        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public string Estado { get; set; } = "BORRADOR";

        public int IdModulo { get; set; } = 1;

        public string Modulo { get; set; }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public IList<ClsModeloLineaPolizaContabilidad> Lineas { get; set; } = new List<ClsModeloLineaPolizaContabilidad>();
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

        public IEnumerable<ValidationResult> Validate(ValidationContext Contexto)
        {
            if (Lineas.Count < 2)
            {
                yield return new ValidationResult("La póliza necesita al menos dos cuentas.", new[] { "Lineas" });
                yield break;
            }
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            foreach (ClsModeloLineaPolizaContabilidad Linea in Lineas)
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            {
                if (Linea.IdCuenta <= 0)
                {
                    yield return new ValidationResult("Todas las líneas deben tener una cuenta contable válida (" + Linea.Cuenta + ").", new[] { "Lineas" });
                    yield break;
                }
                if (Linea.Debe < 0 || Linea.Haber < 0)
                {
                    yield return new ValidationResult("Los importes no pueden ser negativos (" + Linea.Cuenta + ").", new[] { "Lineas" });
                    yield break;
                }
                if ((Linea.Debe > 0) == (Linea.Haber > 0))
                {
                    yield return new ValidationResult("Cada cuenta debe tener importe solo en el debe o solo en el haber (" + Linea.Cuenta + ").", new[] { "Lineas" });
                    yield break;
                }
            }
            decimal Debe = Lineas.Sum(Linea => Linea.Debe);
            if (Debe <= 0 || Debe != Lineas.Sum(Linea => Linea.Haber))
                yield return new ValidationResult("La póliza no cuadra: el total del debe debe ser igual al del haber.", new[] { "Lineas" });
        }
    }
}
