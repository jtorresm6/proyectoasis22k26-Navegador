// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: IRepositorioEncabezadoViaje.cs
// DESCRIPCIÓN: Contrato de consulta para tblEncabezadoViaje
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using CapaModelo_TransporteInterno_Logistica.Entidades;
using System.Collections.Generic;

namespace CapaModelo_TransporteInterno_Logistica.Contratos
{
    public interface IRepositorioEncabezadoViaje : IRepositorioGenerico<ClsEncabezadoViaje>
    {
        int LogisticaMetAgregar(ClsEncabezadoViaje Entidad);

        int LogisticaMetEditar(ClsEncabezadoViaje Entidad);

        int LogisticaMetEliminar(long IdEncabezadoViaje);

        IEnumerable<ClsCatalogoTransporteInterno>
            LogisticaMetObtenerTransportes();

        IEnumerable<ClsCatalogoTransporteInterno>
            LogisticaMetObtenerConductores();
    }
}
