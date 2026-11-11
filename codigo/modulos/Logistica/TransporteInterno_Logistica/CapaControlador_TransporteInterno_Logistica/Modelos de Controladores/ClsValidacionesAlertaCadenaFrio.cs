using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsValidacionesAlertaCadenaFrio
    {
        // =====================================================
        // VALIDAR LOS DATOS DE ALERTA DE CADENA DE FRÍO
        // =====================================================

        public static void LogisticaProcValidar(
            ClsDatosAlertaCadenaFrio Datos)
        {
            // Verificar que existan los datos.
            if (Datos == null)
            {
                throw new ValidationException(
                    "Complete los datos de la alerta.");
            }

            // Ejecutar validaciones DataAnnotations.
            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos,
                Contexto,
                true);

            // Validar telemetría.
            if (Datos.FkTelemetriaTemperaturaCodigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione una telemetría válida.");
            }

            // Validar tipo de alerta.
            if (string.IsNullOrWhiteSpace(
                Datos.TipoAlertaCadenaFrio))
            {
                throw new ValidationException(
                    "Seleccione un tipo de alerta.");
            }

            if (Datos.TipoAlertaCadenaFrio.Length > 30)
            {
                throw new ValidationException(
                    "El tipo de alerta admite hasta 30 caracteres.");
            }

            // Validar observación.
            if (string.IsNullOrWhiteSpace(
                Datos.ObservacionCalidadAlertaCadenaFrio))
            {
                throw new ValidationException(
                    "Ingrese una observación de calidad.");
            }

            if (Datos.ObservacionCalidadAlertaCadenaFrio.Length > 500)
            {
                throw new ValidationException(
                    "La observación admite hasta 500 caracteres.");
            }
        }
    }
}
