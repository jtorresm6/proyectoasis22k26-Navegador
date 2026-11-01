using System;

namespace CapaModelo_ListadoPrecios.Entidades
{
    /// <summary>Entidad que representa el precio asignado a un tipo de cliente y producto.</summary>
    public class ClsPrecioCliente
    {
        public string TipoCliente { get; set; }
        public string TipoProducto { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioFinal { get; set; }
    }
}
