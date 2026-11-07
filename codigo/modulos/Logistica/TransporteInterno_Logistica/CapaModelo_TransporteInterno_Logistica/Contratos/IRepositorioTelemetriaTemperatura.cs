 // ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: IRepositorioTelemetriaTemperatura.cs
// DESCRIPCIÓN: Contrato de consulta para tblTelemetriaTemperatura
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using CapaModelo_TransporteInterno_Logistica.Entidades;
using System.Collections.Generic;

namespace CapaModelo_TransporteInterno_Logistica.Contratos
{
    public interface IRepositorioTelemetriaTemperatura : IRepositorioGenerico<ClsTelemetriaTemperatura>
    {
        // Registrar telemetría.
        int LogisticaMetAgregar(
            ClsTelemetriaTemperatura Entidad);

        // Modificar telemetría.
        int LogisticaMetEditar(
            ClsTelemetriaTemperatura Entidad);

        // Eliminar por identificador.
        int LogisticaMetEliminar(
            long IdTelemetriaTemperatura);

        // Obtener viajes para el ComboBox.
        IEnumerable<ClsCatalogoDetalleViajeLote>
            LogisticaMetObtenerViajes();
    }
}
