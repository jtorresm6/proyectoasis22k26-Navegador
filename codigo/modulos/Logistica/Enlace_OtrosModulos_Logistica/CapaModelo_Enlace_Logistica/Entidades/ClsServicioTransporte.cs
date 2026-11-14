using System;
namespace CapaModelo_Enlace_Logistica.Entidades
{
    public class ClsServicioTransporte
    {
        public int IdServicio { get; set; }
        public string NumeroGuia { get; set; }
        public string NombreTransportista { get; set; }
        public string TipoServicio { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal MontoTotal { get; set; }
        public string Moneda { get; set; }
        public string TipoPago { get; set; }
        public DateTime? FechaServicio { get; set; }
        public string NitTransportista { get; set; }
        public string DireccionTransportista { get; set; }
    }
}