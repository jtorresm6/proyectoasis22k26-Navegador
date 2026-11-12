using System;
using System.Collections.Generic;

namespace CapaModelo_DisponibilidadDiariaBancos.Entidades
{
    public class ClsCotizacionDisponibilidad
    {
        public string Moneda { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TasaOriginal { get; set; }
        public decimal TasaAplicada { get; set; }
        public string Fuente { get; set; }
        public string Metodo { get; set; }
    }

    public class ClsOpcionCatalogo
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public override string ToString() { return Nombre; }
    }

    public class ClsCuentaCatalogo : ClsOpcionCatalogo
    {
        public long BancoId { get; set; }
        public string NumeroCuenta { get; set; }
        public string Moneda { get; set; }
    }

    public class ClsCatalogosDisponibilidad
    {
        public List<ClsOpcionCatalogo> Bancos { get; set; } = new List<ClsOpcionCatalogo>();
        public List<ClsCuentaCatalogo> Cuentas { get; set; } = new List<ClsCuentaCatalogo>();
        public List<ClsOpcionCatalogo> Monedas { get; set; } = new List<ClsOpcionCatalogo>();
        public bool TieneDisponibilidades { get; set; }
        public string Mensaje { get; set; }
    }

    public class ClsResumenDisponibilidad
    {
        public bool EsConsolidado { get; set; }
        public bool ConversionCompleta { get; set; }
        public string Moneda { get; set; }
        public decimal? SaldoInicial { get; set; }
        public decimal? Ingresos { get; set; }
        public decimal? Egresos { get; set; }
        public decimal? Comprometido { get; set; }
        public decimal? Restringido { get; set; }
        public decimal? Disponible { get; set; }
        public int CuentasSinTasa { get; set; }
    }

    public class ClsConsultaDisponibilidad
    {
        public List<ClsDisponibilidad> Detalle { get; set; } = new List<ClsDisponibilidad>();
        public List<ClsCompromiso> Compromisos { get; set; } = new List<ClsCompromiso>();
        public string Mensaje { get; set; }
        public bool TieneTabla { get; set; }
        public DateTime Fecha { get; set; }
        public string MonedaResumen { get; set; }
        public ClsResumenDisponibilidad Resumen { get; set; }
    }
}
