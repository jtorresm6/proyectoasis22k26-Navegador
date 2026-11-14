// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsEncabezadoViaje.cs
// DESCRIPCIÓN: Entidad correspondiente a tblEncabezadoViaje
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;

namespace CapaModelo_TransporteInterno_Logistica.Entidades
{
    public class ClsEncabezadoViaje
    {
        public long IdEncabezadoViaje { get; set; }
        public DateTime FechaDocumentoEncabezadoViaje { get; set; }
        public DateTime HoraSalidaEncabezadoViaje { get; set; }
        public DateTime? HoraLlegadaEncabezadoViaje { get; set; }
        public bool RequiereCadenaFrioEncabezadoViaje { get; set; }
        public string EstadoViajeEncabezadoViaje { get; set; }
        public decimal PesoTotalEncabezadoViaje { get; set; }
        public int Fk_Transporte_Codigo { get; set; }
        public int Fk_Conductor_Codigo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Datos adicionales obtenidos mediante JOIN.
        public string PlacaTransporte { get; set; }

        public string NombreConductor { get; set; }
    }
}
