// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsDetalleViajeLoteProducto.cs
// DESCRIPCIÓN: Entidad correspondiente a tblDetalleViajeLoteProducto
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;

namespace CapaModelo_TransporteInterno_Logistica.Entidades
{
    public class ClsDetalleViajeLoteProducto
    {
        public long Pk_Detalle_Viaje_Lote_Producto { get; set; }
        public long Fk_Detalle_Viaje_Codigo { get; set; }
        public long Fk_Lote_Producto_Codigo { get; set; }
        public int CantidadCajasDetalleViajeLoteProducto { get; set; }
        public decimal PesoKgDetalleViajeLoteProducto { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
