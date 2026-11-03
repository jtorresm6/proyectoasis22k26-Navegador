using System;

namespace CapaControlador_ListadoPrecios
{
    public class ClsPrecioClienteDTO
    {
        public string TipoCliente { get; set; }
        public string Marca { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public string Proveedor { get; set; }
        public string Producto { get; set; }
        public string TipoModificacion { get; set; }
        public decimal Valor { get; set; }
        public string AlcanceModificacion { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioFinal { get; set; }
    }
}
