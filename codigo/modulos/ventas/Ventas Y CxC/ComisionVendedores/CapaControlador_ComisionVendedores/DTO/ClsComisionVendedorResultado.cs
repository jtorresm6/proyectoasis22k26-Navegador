using System.Collections.Generic;
using System.ComponentModel;

namespace CapaControlador_ComisionVendedores.DTO
{
    public class ClsComisionVendedorResultado
    {
        public int IdVendedor { get; set; }
        public int IdMoneda { get; set; }
        public string SimboloMoneda { get; set; }
        public string NombreVendedor { get; set; }
        public string ElementoComision { get; set; }
        public string MetodoComision { get; set; }
        public string EtiquetaMetodoPorcentaje { get { return MetodoComision + " / " + Porcentaje.ToString("0.00") + "%"; } }
        public decimal Porcentaje { get; set; }
        public decimal BaseVentas { get; set; }
        public decimal MontoComision { get; set; }

        [Browsable(false)]
        public List<ClsMovimientoComisionVista> Movimientos { get; set; }
    }
}
