// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsAlertaCadenaFrio.cs
// DESCRIPCIÓN: Entidad correspondiente a tblAlertaCadenaFrio
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;

namespace CapaModelo_TransporteInterno_Logistica.Entidades
{
    public class ClsAlertaCadenaFrio
    {
        public long Pk_Alerta_Cadena_Frio { get; set; }
        public long Fk_Encabezado_Viaje_Codigo { get; set; }
        public long Fk_Telemetria_Temperatura_Codigo { get; set; }
        public string TipoAlertaCadenaFrio { get; set; }
        public decimal TemperaturaRegistradaAlertaCadenaFrio { get; set; }
        public bool AtendidaAlertaCadenaFrio { get; set; }
        public string ObservacionCalidadAlertaCadenaFrio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
