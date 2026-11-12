/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Datos del activo fijo que Contabilidad envía a Logística.
 * ===================================================================
*/

using System;

namespace CapaControlador_Contabilidad
{
    public class ClsModeloActivoLogisticaContabilidad
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Modulo { get; set; }
        public string CodigoTipo { get; set; }
        public string NombreTipo { get; set; }
        public DateTime FechaAdquisicion { get; set; }
        public DateTime FechaInicioUso { get; set; }
        public decimal ValorCompra { get; set; }
        public decimal ValorResidual { get; set; }
        public int VidaUtilMeses { get; set; }
    }
}
