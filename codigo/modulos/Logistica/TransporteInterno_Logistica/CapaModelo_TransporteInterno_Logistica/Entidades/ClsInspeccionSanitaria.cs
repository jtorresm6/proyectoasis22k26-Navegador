// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsInspeccionSanitaria.cs
// DESCRIPCIÓN: Entidad correspondiente a tblInspeccionSanitaria
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;

namespace CapaModelo_TransporteInterno_Logistica.Entidades
{
    public class ClsInspeccionSanitaria
    {
        public long Pk_Inspeccion_Sanitaria { get; set; }
        public int Fk_Transporte_Codigo { get; set; }
        public DateTime FechaInspeccionSanitaria { get; set; }
        public bool SanitizadoCorrectoInspeccionSanitaria { get; set; }
        public bool LibreOloresContamInspeccionSanitaria { get; set; }
        public bool AprobadoParaCargaInspeccionSanitaria { get; set; }
        public string ObservacionesInspeccionSanitaria { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
