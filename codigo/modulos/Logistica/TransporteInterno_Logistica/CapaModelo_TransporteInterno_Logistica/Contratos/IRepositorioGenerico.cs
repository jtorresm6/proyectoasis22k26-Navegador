// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: IRepositorioGenerico.cs
// DESCRIPCIÓN: Contrato para consultar entidades
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System.Collections.Generic;

namespace CapaModelo_TransporteInterno_Logistica.Contratos
{
    public interface IRepositorioGenerico<T> where T : class
    {
        IEnumerable<T> LogisticaMetObtenerTodos();
    }
}
