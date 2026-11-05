/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Modelo del activo fijo con sus validaciones. Los atributos
 *  validan cada campo y Validate valida las reglas entre campos.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaControlador_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsModeloActivoFijoContabilidad : IValidatableObject
    {
        public int IdActivo { get; set; }

        [Required(ErrorMessage = "Ingrese una descripción.")]
        [MaxLength(255, ErrorMessage = "La descripción admite hasta 255 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Ingrese el centro de costo.")]
        [MaxLength(20, ErrorMessage = "El centro de costo admite hasta 20 caracteres.")]
        public string Centro { get; set; }

        public string Modulo { get; set; }

        public string Tipo { get; set; }

        public DateTime Adquisicion { get; set; }

        public DateTime Inicio { get; set; }

        public decimal Compra { get; set; }

        public decimal Residual { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La vida útil debe ser mayor que cero.")]
        public int Meses { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext Contexto)
        {
            if (Inicio.Date < Adquisicion.Date)
                yield return new ValidationResult("No puede ser anterior a la adquisición.", new[] { "Inicio" });
            if (Compra <= 0m || Compra > ClsLimitesContabilidad.MaximoMonetario)
                yield return new ValidationResult("Ingrese un valor de compra válido mayor que cero.", new[] { "Compra" });
            if (Residual < 0m || Residual > Compra)
                yield return new ValidationResult("Debe estar entre cero y el valor de compra.", new[] { "Residual" });
        }
    }
}
