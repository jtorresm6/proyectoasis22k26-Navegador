using System;
using System.Data;
using CapaModelo_Traslados_Logistica.Entidades;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Contrato de acceso a los datos de traslados entre bodegas.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Contratos
{
    public interface IRepositorioTraslado
    {
        DataTable LogisticaMetObtenerTraslados();
        DataTable LogisticaMetObtenerBodegas();
        DataTable LogisticaMetObtenerTiposTransporte();
        DataTable LogisticaMetObtenerConductores();
        DataTable LogisticaMetObtenerTransportes();
        DataTable LogisticaMetObtenerStockProductos(int CodigoBodega, string CodigoTrasladoReserva);
        DataTable LogisticaMetObtenerDetalleTraslado(string CodigoTraslado);
        void LogisticaMetEjecutarUnidadTrabajo(Action<IRepositorioUnidadTrabajoTraslado> Operacion);
    }
}
