/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Mario Taracena
 * Carné : 0901-23-9335
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Modelos del presupuesto y de su partida con sus validaciones.
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
    public class ClsModeloPartidaPresupuestoContabilidad : IValidatableObject
    {
        [Required(ErrorMessage = "Ingrese el nombre de la partida.")]
        [MaxLength(150, ErrorMessage = "El nombre admite hasta 150 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Seleccione un módulo válido.")]
        public string Modulo { get; set; }

        public decimal Cantidad { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext Contexto)
        {
            if (Cantidad <= 0m || Cantidad > ClsLimitesContabilidad.MaximoMonetario)
                yield return new ValidationResult("La asignación debe ser mayor que cero.", new[] { "Cantidad" });
        }
    }

    public class ClsModeloPresupuestoContabilidad : IValidatableObject
    {
        public int IdPresupuesto { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaCierre { get; set; }

        public decimal Gastado { get; set; }

        public List<ClsModeloPartidaPresupuestoContabilidad> Partidas { get; set; } = new List<ClsModeloPartidaPresupuestoContabilidad>();

        public IEnumerable<ValidationResult> Validate(ValidationContext Contexto)
        {
            if (FechaCierre.HasValue && FechaCierre.Value.Date < FechaInicio.Date)
                yield return new ValidationResult("La fecha de cierre debe ser igual o posterior al inicio.", new[] { "FechaCierre" });
            if (Gastado < 0m || Gastado > ClsLimitesContabilidad.MaximoMonetario)
                yield return new ValidationResult("Lo gastado debe estar dentro del límite permitido.", new[] { "Gastado" });
            if (Partidas.Count == 0)
            {
                yield return new ValidationResult("Agregue al menos una partida al presupuesto.", new[] { "Partidas" });
                yield break;
            }
            foreach (ClsModeloPartidaPresupuestoContabilidad Partida in Partidas)
            {
                if (string.IsNullOrWhiteSpace(Partida.Nombre))
                    yield return new ValidationResult("Cada partida necesita un nombre.", new[] { "Partidas" });
                else if (Partida.Cantidad <= 0m || Partida.Cantidad > ClsLimitesContabilidad.MaximoMonetario)
                    yield return new ValidationResult("Las partidas deben tener importes positivos dentro del límite permitido.", new[] { "Partidas" });
            }
            if (Partidas.Sum(Partida => Partida.Cantidad) > ClsLimitesContabilidad.MaximoMonetario)
                yield return new ValidationResult("El total excede el límite permitido.", new[] { "Partidas" });
        }
    }
}
