// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: IRepositorioDetalleViaje.cs
// DESCRIPCIÓN: Contrato de consulta para tblDetalleViaje
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using CapaModelo_TransporteInterno_Logistica.Entidades;
using System.Collections.Generic;

namespace CapaModelo_TransporteInterno_Logistica.Contratos
{
    public interface IRepositorioDetalleViaje : IRepositorioGenerico<ClsDetalleViaje>
    {
        int LogisticaMetAgregar(ClsDetalleViaje Entidad);

        int LogisticaMetEditar(ClsDetalleViaje Entidad);

        int LogisticaMetEliminar(long IdDetalleViaje);

        IEnumerable<ClsCatalogoTransporteInterno>
            LogisticaMetObtenerViajes();
    }
}
