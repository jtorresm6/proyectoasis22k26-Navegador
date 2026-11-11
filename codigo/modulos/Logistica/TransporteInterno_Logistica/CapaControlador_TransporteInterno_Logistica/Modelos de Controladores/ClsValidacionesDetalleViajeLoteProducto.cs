using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsValidacionesDetalleViajeLoteProducto
    {
        // =====================================================
        // VALIDACIONES DE DETALLE DE VIAJE LOTE PRODUCTO
        // =====================================================

        public static void LogisticaProcValidar(
            ClsDatosDetalleViajeLoteProducto Datos)
        {
            // Verificar que existan datos.
            if (Datos == null)
            {
                throw new ValidationException(
                    "Complete los datos de la asignación.");
            }

            // Ejecutar validaciones DataAnnotations.
            var Contexto = new ValidationContext(Datos);

            Validator.ValidateObject(
                Datos,
                Contexto,
                true);

            // Validar detalle de viaje.
            if (Datos.FkDetalleViajeCodigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione un detalle de viaje válido.");
            }

            // Validar lote de producto.
            if (Datos.FkLoteProductoCodigo <= 0)
            {
                throw new ValidationException(
                    "Seleccione un lote de producto válido.");
            }

            // Validar cantidad de cajas.
            if (Datos.CantidadCajasDetalleViajeLoteProducto <= 0)
            {
                throw new ValidationException(
                    "La cantidad de cajas debe ser mayor que cero.");
            }

            // Validar peso total.
            if (Datos.PesoKgDetalleViajeLoteProducto < 0)
            {
                throw new ValidationException(
                    "El peso total no puede ser negativo.");
            }
        }
    }
}
