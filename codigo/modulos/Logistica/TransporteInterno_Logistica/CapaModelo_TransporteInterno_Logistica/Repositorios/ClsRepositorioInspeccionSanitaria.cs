// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioInspeccionSanitaria.cs
// DESCRIPCIÓN: Obtiene los registros de tblInspeccionSanitaria
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_TransporteInterno_Logistica.Contratos;
using CapaModelo_TransporteInterno_Logistica.Entidades;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public class ClsRepositorioInspeccionSanitaria : ClsRepositorioMaestro, IRepositorioInspeccionSanitaria
    {
        public IEnumerable<ClsInspeccionSanitaria> LogisticaMetObtenerTodos()
        {
            var lista = new List<ClsInspeccionSanitaria>();
            DataTable tabla = LogisticaMetEjecucionConsulta("SELECT * FROM tblInspeccionSanitaria");

            foreach (DataRow fila in tabla.Rows)
            {
                var entidad = new ClsInspeccionSanitaria
                {
                    Pk_Inspeccion_Sanitaria = Convert.ToInt64(fila["Pk_Inspeccion_Sanitaria"]),
                    Fk_Transporte_Codigo = Convert.ToInt32(fila["Fk_Transporte_Codigo"]),
                    FechaInspeccionSanitaria = Convert.ToDateTime(fila["fechaInspeccionSanitaria"]),
                    SanitizadoCorrectoInspeccionSanitaria = Convert.ToBoolean(fila["sanitizadoCorrectoInspeccionSanitaria"]),
                    LibreOloresContamInspeccionSanitaria = Convert.ToBoolean(fila["libreOloresContamInspeccionSanitaria"]),
                    AprobadoParaCargaInspeccionSanitaria = Convert.ToBoolean(fila["aprobadoParaCargaInspeccionSanitaria"]),
                    ObservacionesInspeccionSanitaria = Convert.ToString(fila["observacionesInspeccionSanitaria"]),
                    CreatedAt = Convert.ToDateTime(fila["created_at"]),
                    UpdatedAt = Convert.ToDateTime(fila["updated_at"])
                };
                lista.Add(entidad);
            }
            return lista;
        }
    }
}
