using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsValidacionesTelemetria
    {
        public static void LogisticaProcValidar(
            ClsDatosTelemetriaTemperatura Datos)
        {
            if (Datos == null)
            {
                throw new ValidationException(
                    "Los datos son obligatorios.");
            }

            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos, Contexto, true);

            if (Datos.FkEncabezadoViajeCodigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione un viaje válido.");
            }

            if (Datos.FechaHoraTelemetriaTemperatura ==
                DateTime.MinValue)
            {
                throw new ValidationException(
                    "Ingrese una fecha válida.");
            }

            if (Datos.TemperaturaCelsiusTelemetriaTemperatura
                    < -999.99m ||
                Datos.TemperaturaCelsiusTelemetriaTemperatura
                    > 999.99m)
            {
                throw new ValidationException(
                    "Temperatura fuera del rango permitido.");
            }

            if (Datos.HumedadRelativaTelemetriaTemperatura < 0 ||
                Datos.HumedadRelativaTelemetriaTemperatura > 1)
            {
                throw new ValidationException(
                    "Humedad fuera del rango permitido.");
            }

            if (string.IsNullOrWhiteSpace(
                Datos.UbicacionGpsTelemetriaTemperatura))
            {
                throw new ValidationException(
                    "La ubicación GPS es obligatoria.");
            }
        }
    }
}
