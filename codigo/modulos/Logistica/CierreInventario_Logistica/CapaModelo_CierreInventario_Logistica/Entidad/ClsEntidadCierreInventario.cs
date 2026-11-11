using System.Data;

namespace CapaModelo_CierreInventario_Logistica.Entidad
{
    public class ResultadoCierre
    {
        public DataTable Detalle { get; set; }
        public string Mensaje { get; set; }
        public bool PuedeCerrar { get { return string.IsNullOrEmpty(Mensaje); } }
        public int Productos { get; set; }
        public int Bodegas { get; set; }
        public int Movimientos { get; set; }
        public int Diferencias { get; set; }
    }
}
