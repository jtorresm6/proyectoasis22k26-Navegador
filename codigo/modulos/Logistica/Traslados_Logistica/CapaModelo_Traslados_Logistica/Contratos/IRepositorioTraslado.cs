using System.Data;
using CapaModelo_Traslados_Logistica.Entidades;

namespace CapaModelo_Traslados_Logistica.Contratos
{
    public interface IRepositorioTraslado
    {
        DataTable LogisticaMetObtenerTraslados();
        DataTable LogisticaMetObtenerBodegas();
        DataTable LogisticaMetObtenerTiposTransporte();
        DataTable LogisticaMetObtenerConductoresDisponibles(string CodigoTrasladoExcluir);
        DataTable LogisticaMetObtenerTransportesDisponibles(int CodigoTipoTransporte);
        DataTable LogisticaMetObtenerTransportesDisponibles(int CodigoTipoTransporte, string CodigoTrasladoExcluir);
        DataTable LogisticaMetObtenerProductosDisponibles(int CodigoBodega);
        DataTable LogisticaMetObtenerProductosDisponibles(int CodigoBodega, string CodigoTrasladoExcluir);
        DataTable LogisticaMetObtenerDetalleTraslado(string CodigoTraslado);
        void LogisticaMetGuardarTraslado(ClsTrasladoSolicitud Solicitud);
        void LogisticaMetActualizarTraslado(ClsTrasladoSolicitud Solicitud);
    }
}
