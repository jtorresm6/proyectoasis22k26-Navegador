// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioAlertaCadenaFrio.cs
// DESCRIPCIÓN: Obtiene los registros de tblAlertaCadenaFrio
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_TransporteInterno_Logistica.Contratos;
using CapaModelo_TransporteInterno_Logistica.Entidades;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public class ClsRepositorioAlertaCadenaFrio : ClsRepositorioMaestro, IRepositorioAlertaCadenaFrio
    {
        public IEnumerable<ClsAlertaCadenaFrio> LogisticaMetObtenerTodos()
        {
            var lista = new List<ClsAlertaCadenaFrio>();
            DataTable tabla = LogisticaMetEjecucionConsulta("SELECT * FROM tblAlertaCadenaFrio");

            foreach (DataRow fila in tabla.Rows)
            {
                var entidad = new ClsAlertaCadenaFrio
                {
                    Pk_Alerta_Cadena_Frio = Convert.ToInt64(fila["Pk_Alerta_Cadena_Frio"]),
                    Fk_Encabezado_Viaje_Codigo = Convert.ToInt64(fila["Fk_Encabezado_Viaje_Codigo"]),
                    Fk_Telemetria_Temperatura_Codigo = Convert.ToInt64(fila["Fk_Telemetria_Temperatura_Codigo"]),
                    TipoAlertaCadenaFrio = Convert.ToString(fila["tipoAlertaCadenaFrio"]),
                    TemperaturaRegistradaAlertaCadenaFrio = Convert.ToDecimal(fila["temperaturaRegistradaAlertaCadenaFrio"]),
                    AtendidaAlertaCadenaFrio = Convert.ToBoolean(fila["atendidaAlertaCadenaFrio"]),
                    ObservacionCalidadAlertaCadenaFrio = Convert.ToString(fila["observacionCalidadAlertaCadenaFrio"]),
                    CreatedAt = Convert.ToDateTime(fila["created_at"]),
                    UpdatedAt = Convert.ToDateTime(fila["updated_at"])
                };
                lista.Add(entidad);
            }
            return lista;
        }
    }
}
