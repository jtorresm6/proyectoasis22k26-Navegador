using System;
using System.Collections.Generic;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Entidades del cierre de movimientos: período, saldo por proveedor,
 *  rango y antigüedad de saldos, validaciones y datos del cierre.
 *  Los montos están en moneda local (quetzales) mientras el cierre en varias
 *  monedas queda pendiente para la entrega del 100%.
 * ===================================================================
*/

namespace CapaModelo_CierreMovimientos.Entidades
{
    public enum ComprasEnumEstadoValidacion
    {
        Correcta,
        Advertencia,
        Bloqueante
    }

    public class ClsPeriodoCompras
    {
        public int IdPeriodo { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public bool EstaCerrado => string.Equals(Estado, "Cerrado", StringComparison.OrdinalIgnoreCase);

        public string Descripcion
        {
            get
            {
                string Nombre = new System.Globalization.CultureInfo("es-ES").DateTimeFormat.GetMonthName(Mes);
                return char.ToUpper(Nombre[0]) + Nombre.Substring(1) + " " + Anio;
            }
        }
    }

    public class ClsSaldoProveedorCierre
    {
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal Cargos { get; set; }
        public decimal Abonos { get; set; }
        public decimal SaldoFinal => SaldoInicial + Cargos - Abonos;
        public bool TieneMovimiento => SaldoInicial != 0 || Cargos != 0 || Abonos != 0;
    }

    public class ClsRangoAntiguedad
    {
        public int IdRango { get; set; }
        public string Nombre { get; set; }
        public int? DiasDesde { get; set; }
        public int? DiasHasta { get; set; }

        public bool ComprasFuncIncluye(int DiasVencidos)
        {
            return (!DiasDesde.HasValue || DiasVencidos >= DiasDesde.Value)
                && (!DiasHasta.HasValue || DiasVencidos <= DiasHasta.Value);
        }
    }

    public class ClsDocumentoPendiente
    {
        public int IdProveedor { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal SaldoLocal { get; set; }
    }

    public class ClsAntiguedadCierre
    {
        public int IdProveedor { get; set; }
        public int IdRango { get; set; }
        public decimal Monto { get; set; }
        public int CantidadDocumentos { get; set; }
    }

    public class ClsValidacionCierre
    {
        public ComprasEnumEstadoValidacion Estado { get; set; }
        public string Validacion { get; set; }
        public string Origen { get; set; }
    }

    public class ClsIndicadoresCierre
    {
        public int PeriodosAbiertosAnteriores { get; set; }
        public int PagosProgramados { get; set; }
        public decimal MontoPagosProgramados { get; set; }
        public int OrdenesPendientes { get; set; }
        public int ComprasSinCuentaPorPagar { get; set; }
        public int DocumentosMonedaExtranjera { get; set; }
        public int RangosAntiguedadActivos { get; set; }
    }

    public class ClsResultadoCierre
    {
        public ClsPeriodoCompras Periodo { get; set; }
        public int Proveedores { get; set; }
        public decimal TotalCargos { get; set; }
        public decimal TotalAbonos { get; set; }
        public decimal SaldoFinal { get; set; }
        public int DocumentosBloqueados { get; set; }
        public string SiguientePeriodo { get; set; }
    }

    public class ClsDatosCierre
    {
        public ClsPeriodoCompras Periodo { get; set; }
        public long IdMonedaLocal { get; set; }
        public List<ClsSaldoProveedorCierre> Saldos { get; set; } = new List<ClsSaldoProveedorCierre>();
        public List<ClsAntiguedadCierre> Antiguedad { get; set; } = new List<ClsAntiguedadCierre>();
        public int? IdUsuario { get; set; }
        public string Ip { get; set; }
    }
}
