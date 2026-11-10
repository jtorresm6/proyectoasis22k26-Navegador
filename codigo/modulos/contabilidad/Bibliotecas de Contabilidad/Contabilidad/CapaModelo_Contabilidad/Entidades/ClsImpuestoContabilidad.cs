/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 30/10/2026
 * ==================================================================
 * Propósito :
 *  Un impuesto de tblImpuesto con su valor, sus cuentas contables y
 *  los tramos de tblImpuestoTramo cuando los tiene.
 * ===================================================================
*/

using System.Collections.Generic;

namespace CapaModelo_Contabilidad.Entidades
{
    public class ClsImpuestoContabilidad
    {
        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 02/11/2026
        public int IdImpuesto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Operacion { get; set; }
        public decimal Valor { get; set; }
        public string Periodicidad { get; set; }
        public string CuentaAcreditable { get; set; }
        public string CuentaPorPagar { get; set; }
        public List<ClsImpuestoTramoContabilidad> Tramos { get; set; } = new List<ClsImpuestoTramoContabilidad>();
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 02/11/2026
    }
}
