/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 30/10/2026
 * ==================================================================
 * Propósito :
 *  Impuesto que Contabilidad entrega a los demás módulos: su valor en
 *  fracción, las cuentas donde se registra y sus tramos si los tiene.
 * ===================================================================
*/

using System.Collections.Generic;

namespace CapaControlador_Contabilidad
{
    public class ClsModeloImpuestoContabilidad
    {
        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 02/11/2026
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Operacion { get; set; }
        public decimal Valor { get; set; }
        public string Periodicidad { get; set; }
        public string CuentaAcreditable { get; set; }
        public string CuentaPorPagar { get; set; }
        public List<ClsModeloTramoImpuestoContabilidad> Tramos { get; set; } = new List<ClsModeloTramoImpuestoContabilidad>();
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 02/11/2026
    }
}
