// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioDetalleViaje.cs
// DESCRIPCIÓN: Obtiene los registros de tblDetalleViaje
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_TransporteInterno_Logistica.Contratos;
using CapaModelo_TransporteInterno_Logistica.Entidades;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public class ClsRepositorioDetalleViaje : ClsRepositorioMaestro, IRepositorioDetalleViaje
    {
        public IEnumerable<ClsDetalleViaje> LogisticaMetObtenerTodos()
        {
            var lista = new List<ClsDetalleViaje>();
            DataTable tabla = LogisticaMetEjecucionConsulta("SELECT * FROM tblDetalleViaje");

            foreach (DataRow fila in tabla.Rows)
            {
                var entidad = new ClsDetalleViaje
                {
                    Pk_Detalle_Viaje = Convert.ToInt64(fila["Pk_Detalle_Viaje"]),
                    Fk_Encabezado_Viaje_Codigo = Convert.ToInt64(fila["Fk_Encabezado_Viaje_Codigo"]),
                    OrdenDetalleViaje = Convert.ToInt32(fila["ordenDetalleViaje"]),
                    DescripcionDetalleViaje = Convert.ToString(fila["descripcionDetalleViaje"]),
                    CantidadDetalleViaje = Convert.ToDecimal(fila["cantidadDetalleViaje"]),
                    PesoDetalleViaje = Convert.ToDecimal(fila["pesoDetalleViaje"]),
                    CreatedAt = Convert.ToDateTime(fila["created_at"]),
                    UpdatedAt = Convert.ToDateTime(fila["updated_at"])
                };
                lista.Add(entidad);
            }
            return lista;
        }
    }
}
