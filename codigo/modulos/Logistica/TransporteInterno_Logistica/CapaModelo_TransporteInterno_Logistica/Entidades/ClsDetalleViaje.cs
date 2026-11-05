// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsDetalleViaje.cs
// DESCRIPCIÓN: Entidad correspondiente a tblDetalleViaje
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;

namespace CapaModelo_TransporteInterno_Logistica.Entidades
{
    public class ClsDetalleViaje
    {
        public long Pk_Detalle_Viaje { get; set; }
        public long Fk_Encabezado_Viaje_Codigo { get; set; }
        public int OrdenDetalleViaje { get; set; }
        public string DescripcionDetalleViaje { get; set; }
        public decimal CantidadDetalleViaje { get; set; }
        public decimal PesoDetalleViaje { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
