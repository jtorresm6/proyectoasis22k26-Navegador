using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
   public class ClsValidacionesEncabezadoViaje
    {
        // =====================================================
        // VALIDACIONES DE ENCABEZADO DE VIAJE
        // =====================================================

        public static void LogisticaProcValidar(
            ClsDatosEncabezadoViaje Datos)
        {
            // Verificar que existan datos.
            if (Datos == null)
            {
                throw new ValidationException(
                    "Complete los datos del viaje.");
            }

            // Ejecutar las validaciones DataAnnotations.
            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos,
                Contexto,
                true);

            // =================================================
            // FECHA DEL DOCUMENTO
            // =================================================

            if (Datos.FechaDocumentoEncabezadoViaje ==
                DateTime.MinValue)
            {
                throw new ValidationException(
                    "Ingrese una fecha de documento válida.");
            }

            // =================================================
            // FECHA Y HORA DE SALIDA
            // =================================================

            if (Datos.HoraSalidaEncabezadoViaje ==
                DateTime.MinValue)
            {
                throw new ValidationException(
                    "Ingrese una fecha y hora de salida válida.");
            }

            // =================================================
            // FECHA Y HORA DE LLEGADA
            // =================================================

            // La llegada puede ser NULL mientras
            // el viaje continúa en curso.
            if (Datos.HoraLlegadaEncabezadoViaje.HasValue)
            {
                if (Datos.HoraLlegadaEncabezadoViaje.Value <
                    Datos.HoraSalidaEncabezadoViaje)
                {
                    throw new ValidationException(
                        "La fecha de llegada no puede ser " +
                        "anterior a la salida.");
                }
            }

            // =================================================
            // VALIDAR SALIDA RESPECTO AL DOCUMENTO
            // =================================================

            if (Datos.HoraSalidaEncabezadoViaje <
                Datos.FechaDocumentoEncabezadoViaje.Date)
            {
                throw new ValidationException(
                    "La salida no puede ser anterior " +
                    "a la fecha del documento.");
            }

            // =================================================
            // VALIDAR TRANSPORTE
            // =================================================

            if (Datos.Fk_Transporte_Codigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione un transporte válido.");
            }

            // =================================================
            // VALIDAR CONDUCTOR
            // =================================================

            if (Datos.Fk_Conductor_Codigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione un conductor válido.");
            }

            // =================================================
            // VALIDAR ESTADO DEL VIAJE
            // =================================================

            if (string.IsNullOrWhiteSpace(
                Datos.EstadoViajeEncabezadoViaje))
            {
                throw new ValidationException(
                    "Seleccione un estado de viaje.");
            }
        }
    }
}
