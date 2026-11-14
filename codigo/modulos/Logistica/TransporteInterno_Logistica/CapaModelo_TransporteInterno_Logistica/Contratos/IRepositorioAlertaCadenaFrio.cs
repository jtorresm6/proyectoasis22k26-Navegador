// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: IRepositorioAlertaCadenaFrio.cs
// DESCRIPCIÓN: Contrato de consulta para tblAlertaCadenaFrio
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using CapaModelo_TransporteInterno_Logistica.Entidades;
using System.Collections.Generic;

namespace CapaModelo_TransporteInterno_Logistica.Contratos
{
    public interface IRepositorioAlertaCadenaFrio : IRepositorioGenerico<ClsAlertaCadenaFrio>
    {
        // Registrar una alerta.
        int LogisticaMetAgregar(
            ClsAlertaCadenaFrio Entidad);

        // Modificar una alerta.
        int LogisticaMetEditar(
            ClsAlertaCadenaFrio Entidad);

        // Eliminar una alerta.
        int LogisticaMetEliminar(
            long IdAlertaCadenaFrio);

        // Obtener registros de telemetría.
        IEnumerable<ClsCatalogoDetalleViajeLote>
            LogisticaMetObtenerTelemetrias();
    }
}
