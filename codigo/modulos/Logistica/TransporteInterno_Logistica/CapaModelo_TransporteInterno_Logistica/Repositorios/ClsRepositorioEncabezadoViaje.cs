// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioEncabezadoViaje.cs
// DESCRIPCIÓN: Obtiene los registros de tblEncabezadoViaje
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_TransporteInterno_Logistica.Contratos;
using CapaModelo_TransporteInterno_Logistica.Entidades;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public class ClsRepositorioEncabezadoViaje : ClsRepositorioMaestro, IRepositorioEncabezadoViaje
    {
        public IEnumerable<ClsEncabezadoViaje> LogisticaMetObtenerTodos()
        {
            var lista = new List<ClsEncabezadoViaje>();
            DataTable tabla = LogisticaMetEjecucionConsulta("SELECT * FROM tblEncabezadoViaje");

            foreach (DataRow fila in tabla.Rows)
            {
                var entidad = new ClsEncabezadoViaje
                {
                    Pk_Encabezado_Viaje = Convert.ToInt64(fila["Pk_Encabezado_Viaje"]),
                    FechaDocumentoEncabezadoViaje = Convert.ToDateTime(fila["fechaDocumentoEncabezadoViaje"]),
                    HoraSalidaEncabezadoViaje = Convert.ToDateTime(fila["horaSalidaEncabezadoViaje"]),
                    HoraLlegadaEncabezadoViaje = fila["horaLlegadaEncabezadoViaje"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(fila["horaLlegadaEncabezadoViaje"]),
                    RequiereCadenaFrioEncabezadoViaje = Convert.ToBoolean(fila["requiereCadenaFrioEncabezadoViaje"]),
                    EstadoViajeEncabezadoViaje = Convert.ToString(fila["estadoViajeEncabezadoViaje"]),
                    PesoTotalEncabezadoViaje = Convert.ToDecimal(fila["pesoTotalEncabezadoViaje"]),
                    Fk_Transporte_Codigo = Convert.ToInt32(fila["Fk_Transporte_Codigo"]),
                    Fk_Conductor_Codigo = Convert.ToInt32(fila["Fk_Conductor_Codigo"]),
                    CreatedAt = Convert.ToDateTime(fila["created_at"]),
                    UpdatedAt = Convert.ToDateTime(fila["updated_at"])
                };
                lista.Add(entidad);
            }
            return lista;
        }
    }
}
