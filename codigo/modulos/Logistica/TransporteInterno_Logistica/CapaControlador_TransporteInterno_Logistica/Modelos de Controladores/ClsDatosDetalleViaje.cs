using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsDatosDetalleViaje
    {
        // Identificador del detalle.
        public long IdDetalleViaje { get; set; }

        // Encabezado de viaje relacionado.
        [Range(
            1,
            long.MaxValue,
            ErrorMessage = "Seleccione un viaje válido.")]
        public long FkEncabezadoViajeCodigo { get; set; }

        // Orden del detalle.
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "El orden debe ser mayor que cero.")]
        public int OrdenDetalleViaje { get; set; }

        // Descripción del detalle.
        [Required(
            ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(
            255,
            ErrorMessage = "La descripción admite hasta 255 caracteres.")]
        public string DescripcionDetalleViaje { get; set; }

        // Cantidad.
        [Range(
            typeof(decimal),
            "0",
            "999999999999999.999",
            ErrorMessage = "La cantidad no puede ser negativa.")]
        public decimal CantidadDetalleViaje { get; set; }

        // Peso.
        [Range(
            typeof(decimal),
            "0",
            "999999999999999.999",
            ErrorMessage = "El peso no puede ser negativo.")]
        public decimal PesoDetalleViaje { get; set; }

        // Campos adicionales para consultas.
        public string DescripcionViaje { get; set; }

        [Browsable(false)]
        public DateTime CreatedAt { get; set; }

        [Browsable(false)]
        public DateTime UpdatedAt { get; set; }
    }
}
