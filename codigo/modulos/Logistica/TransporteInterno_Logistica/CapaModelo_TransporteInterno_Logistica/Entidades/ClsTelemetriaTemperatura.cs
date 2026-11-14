// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsTelemetriaTemperatura.cs
// DESCRIPCIÓN: Entidad correspondiente a tblTelemetriaTemperatura
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;

namespace CapaModelo_TransporteInterno_Logistica.Entidades
{
    public class ClsTelemetriaTemperatura
    {
        public long IdTelemetriaTemperatura { get; set; }
        public long Fk_Encabezado_Viaje_Codigo { get; set; }
        public DateTime FechaHoraTelemetriaTemperatura { get; set; }
        public decimal TemperaturaCelsiusTelemetriaTemperatura { get; set; }
        public decimal HumedadRelativaTelemetriaTemperatura { get; set; }
        public bool PuertaAbiertaTelemetriaTemperatura { get; set; }
        public string UbicacionGpsTelemetriaTemperatura { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
