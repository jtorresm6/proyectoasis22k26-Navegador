/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Ejecuta las validaciones de un modelo con DataAnnotations y
 *  devuelve los errores por campo para que la Vista los muestre.
 * Reglas especificas:
 *  Validator solo ejecuta IValidatableObject si no hay errores en los
 *  atributos; aquí se ejecuta siempre para reportar todo junto.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CapaControlador_Contabilidad
{
    public static class ClsValidacionDatosContabilidad
    {
        public static Dictionary<string, string> ContabilidadFuncErrores(object Modelo)
        {
            List<ValidationResult> Resultados = new List<ValidationResult>();
            ValidationContext Contexto = new ValidationContext(Modelo);
            bool TodoValido = Validator.TryValidateObject(Modelo, Contexto, Resultados, true);
            IValidatableObject Reglas = Modelo as IValidatableObject;
            if (!TodoValido && Reglas != null) Resultados.AddRange(Reglas.Validate(Contexto));

            Dictionary<string, string> Errores = new Dictionary<string, string>();
            foreach (ValidationResult Resultado in Resultados)
            {
                string Campo = Resultado.MemberNames.FirstOrDefault() ?? string.Empty;
                if (!Errores.ContainsKey(Campo)) Errores[Campo] = Resultado.ErrorMessage;
            }
            return Errores;
        }

        public static string ContabilidadFuncPrimerError(object Modelo)
        {
            return ContabilidadFuncErrores(Modelo).Values.FirstOrDefault() ?? string.Empty;
        }

        public static string ContabilidadFuncErrorRangoFechas(DateTime Inicio, DateTime Fin)
        {
            return Fin.Date < Inicio.Date ? "La fecha final no puede ser anterior a la inicial." : string.Empty;
        }
    }
}
