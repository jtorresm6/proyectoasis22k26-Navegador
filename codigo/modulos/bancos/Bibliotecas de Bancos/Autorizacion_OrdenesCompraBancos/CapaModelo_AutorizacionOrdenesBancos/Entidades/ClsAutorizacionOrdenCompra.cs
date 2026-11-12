using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  La entidad ClsAutorizacionOrdenCompra representa el encabezado de la
 *  autorización de una orden de compra (tabla tblAutOrdenCompra) junto con su
 *  evidencia de fondos y el detalle por cuenta.
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Entidades
{
    public class ClsAutorizacionOrdenCompra
    {
        public long IdAutorizacionOrden { get; set; }
        public long IdOrdenCompra { get; set; }
        public int IdEmpresaAutorizacionOrden { get; set; }
        public string EstadoAutorizacionOrden { get; set; }
        public int? IdUsuarioAutoriza { get; set; }
        public DateTime? FechaDecisionAutorizacionOrden { get; set; }
        public string ComentarioAutorizacionOrden { get; set; }
        public string MotivoRechazoAutorizacionOrden { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ClsEvidenciaFondos> Evidencias { get; set; } = new List<ClsEvidenciaFondos>();
        public List<ClsAutorizacionDetalle> Detalles { get; set; } = new List<ClsAutorizacionDetalle>();
        public string Ip { get; set; }
    }
}
