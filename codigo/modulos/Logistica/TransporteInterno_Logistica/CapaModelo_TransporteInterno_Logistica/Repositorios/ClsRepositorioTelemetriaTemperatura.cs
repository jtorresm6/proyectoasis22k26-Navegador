// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioTelemetriaTemperatura.cs
// DESCRIPCIÓN: Obtiene los registros de tblTelemetriaTemperatura
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_TransporteInterno_Logistica.Contratos;
using CapaModelo_TransporteInterno_Logistica.Entidades;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public class ClsRepositorioTelemetriaTemperatura : ClsRepositorioMaestro, IRepositorioTelemetriaTemperatura
    {
        public IEnumerable<ClsTelemetriaTemperatura> LogisticaMetObtenerTodos()
        {
            var lista = new List<ClsTelemetriaTemperatura>();
            DataTable tabla = LogisticaMetEjecucionConsulta("SELECT * FROM tblTelemetriaTemperatura");

            foreach (DataRow fila in tabla.Rows)
            {
                var entidad = new ClsTelemetriaTemperatura
                {
                    Pk_Telemetria_Temperatura = Convert.ToInt64(fila["Pk_Telemetria_Temperatura"]),
                    Fk_Encabezado_Viaje_Codigo = Convert.ToInt64(fila["Fk_Encabezado_Viaje_Codigo"]),
                    FechaHoraTelemetriaTemperatura = Convert.ToDateTime(fila["fechaHoraTelemetriaTemperatura"]),
                    TemperaturaCelsiusTelemetriaTemperatura = Convert.ToDecimal(fila["temperaturaCelsiusTelemetriaTemperatura"]),
                    HumedadRelativaTelemetriaTemperatura = Convert.ToDecimal(fila["humedadRelativaTelemetriaTemperatura"]),
                    PuertaAbiertaTelemetriaTemperatura = Convert.ToBoolean(fila["puertaAbiertaTelemetriaTemperatura"]),
                    UbicacionGpsTelemetriaTemperatura = Convert.ToString(fila["ubicacionGpsTelemetriaTemperatura"]),
                    CreatedAt = Convert.ToDateTime(fila["created_at"]),
                    UpdatedAt = Convert.ToDateTime(fila["updated_at"])
                };
                lista.Add(entidad);
            }
            return lista;
        }
    }
}
