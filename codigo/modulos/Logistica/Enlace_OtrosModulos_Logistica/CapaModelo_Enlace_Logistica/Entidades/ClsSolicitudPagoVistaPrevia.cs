using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Enlace_Logistica.Entidades
{
    public class ClsSolicitudPagoVistaPrevia
    {
        public int IdServicio { get; set; }

        public string NumeroGuia { get; set; }

        public string FechaServicio { get; set; }

        public string NombreTransportista { get; set; }

        public string NitTransportista { get; set; }

        public string DireccionTransportista { get; set; }

        public string BancoBeneficiario { get; set; }

        public string TipoCuenta { get; set; }

        public string NumeroCuenta { get; set; }

        public string Moneda { get; set; }

        public decimal MontoTotal { get; set; }

        public string ConceptoPago { get; set; }

        public string DescripcionServicio { get; set; }

        public decimal Cantidad { get; set; }

        public decimal CostoUnitario { get; set; }
    }
}
