using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsValidacionesDetalleViaje
    {
        // =====================================================
        // VALIDACIONES DE DETALLE DE VIAJE
        // =====================================================

        public static void LogisticaProcValidar(
            ClsDatosDetalleViaje Datos)
        {
            // Validar que existan los datos.
            if (Datos == null)
            {
                throw new ValidationException(
                    "Complete los datos del detalle de viaje.");
            }

            // Ejecutar validaciones DataAnnotations.
            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos,
                Contexto,
                true);

            // Validar encabezado de viaje.
            if (Datos.FkEncabezadoViajeCodigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione un viaje válido.");
            }

            // Validar orden del detalle.
            if (Datos.OrdenDetalleViaje <= 0)
            {
                throw new ValidationException(
                    "El orden debe ser mayor que cero.");
            }

            // Validar descripción.
            if (string.IsNullOrWhiteSpace(
                Datos.DescripcionDetalleViaje))
            {
                throw new ValidationException(
                    "La descripción es obligatoria.");
            }

            if (Datos.DescripcionDetalleViaje.Length > 255)
            {
                throw new ValidationException(
                    "La descripción admite hasta 255 caracteres.");
            }

            // Validar cantidad.
            if (Datos.CantidadDetalleViaje <= 0)
            {
                throw new ValidationException(
                    "La cantidad debe ser mayor que cero.");
            }

            // Validar peso.
            if (Datos.PesoDetalleViaje < 0)
            {
                throw new ValidationException(
                    "El peso no puede ser negativo.");
            }
        }
    }
}
