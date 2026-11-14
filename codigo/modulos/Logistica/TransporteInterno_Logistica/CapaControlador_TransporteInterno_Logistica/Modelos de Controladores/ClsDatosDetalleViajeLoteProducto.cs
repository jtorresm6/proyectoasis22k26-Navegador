using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores
{
    public class ClsDatosDetalleViajeLoteProducto
    {
        public long IdDetalleViajeLoteProducto { get; set; }

        [Range(
            1,
            long.MaxValue,
            ErrorMessage = "Seleccione un detalle de viaje válido.")]
        public long FkDetalleViajeCodigo { get; set; }

        [Range(
            1,
            long.MaxValue,
            ErrorMessage = "Seleccione un lote de producto válido.")]
        public long FkLoteProductoCodigo { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "La cantidad de cajas debe ser mayor que cero.")]
        public int CantidadCajasDetalleViajeLoteProducto
        {
            get;
            set;
        }

        [Range(
            typeof(decimal),
            "0",
            "999999999999999.999",
            ErrorMessage = "El peso no puede ser negativo.")]
        public decimal PesoKgDetalleViajeLoteProducto
        {
            get;
            set;
        }

        // Datos legibles para el historial.
        public string NombreDetalleViaje { get; set; }

        public string NombreLoteProducto { get; set; }

        [Browsable(false)]
        public DateTime CreatedAt { get; set; }

        [Browsable(false)]
        public DateTime UpdatedAt { get; set; }
    }
}
