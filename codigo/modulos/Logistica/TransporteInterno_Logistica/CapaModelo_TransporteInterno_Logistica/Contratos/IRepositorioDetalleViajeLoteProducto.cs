// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: IRepositorioDetalleViajeLoteProducto.cs
// DESCRIPCIÓN: Contrato de consulta para tblDetalleViajeLoteProducto
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using CapaModelo_TransporteInterno_Logistica.Entidades;
using System.Collections.Generic;

namespace CapaModelo_TransporteInterno_Logistica.Contratos
{
    public interface IRepositorioDetalleViajeLoteProducto : IRepositorioGenerico<ClsDetalleViajeLoteProducto>
    {
        int LogisticaMetAgregar(
            ClsDetalleViajeLoteProducto Entidad);

        int LogisticaMetEditar(
            ClsDetalleViajeLoteProducto Entidad);

        int LogisticaMetEliminar(
            long IdDetalleViajeLoteProducto);

        IEnumerable<ClsCatalogoDetalleViajeLote>
            LogisticaMetObtenerDetallesViaje();

        IEnumerable<ClsCatalogoDetalleViajeLote>
            LogisticaMetObtenerLotesProducto();
    }
}
