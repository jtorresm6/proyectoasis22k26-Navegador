using System;

namespace CapaModelo_DisponibilidadDiariaBancos.Entidades
{
    public class ClsCompromiso
    {
        public long CuentaId { get; set; }
        public string Moneda { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public string Beneficiario { get; set; }
        public decimal Monto { get; set; }
    }
}
