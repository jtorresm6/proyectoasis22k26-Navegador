// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioDetalleViajeLoteProducto.cs
// DESCRIPCIÓN: Obtiene los registros de tblDetalleViajeLoteProducto
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_TransporteInterno_Logistica.Contratos;
using CapaModelo_TransporteInterno_Logistica.Entidades;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public class ClsRepositorioDetalleViajeLoteProducto : ClsRepositorioMaestro, IRepositorioDetalleViajeLoteProducto
    {
        public IEnumerable<ClsDetalleViajeLoteProducto> LogisticaMetObtenerTodos()
        {
            var lista = new List<ClsDetalleViajeLoteProducto>();
            DataTable tabla = LogisticaMetEjecucionConsulta("SELECT * FROM tblDetalleViajeLoteProducto");

            foreach (DataRow fila in tabla.Rows)
            {
                var entidad = new ClsDetalleViajeLoteProducto
                {
                    Pk_Detalle_Viaje_Lote_Producto = Convert.ToInt64(fila["Pk_Detalle_Viaje_Lote_Producto"]),
                    Fk_Detalle_Viaje_Codigo = Convert.ToInt64(fila["Fk_Detalle_Viaje_Codigo"]),
                    Fk_Lote_Producto_Codigo = Convert.ToInt64(fila["Fk_Lote_Producto_Codigo"]),
                    CantidadCajasDetalleViajeLoteProducto = Convert.ToInt32(fila["cantidadCajasDetalleViajeLoteProducto"]),
                    PesoKgDetalleViajeLoteProducto = Convert.ToDecimal(fila["pesoKgDetalleViajeLoteProducto"]),
                    CreatedAt = Convert.ToDateTime(fila["created_at"]),
                    UpdatedAt = Convert.ToDateTime(fila["updated_at"])
                };
                lista.Add(entidad);
            }
            return lista;
        }
    }
}
