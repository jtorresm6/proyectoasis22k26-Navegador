
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsRepositorioBodega.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Consulta todas las bodegas registradas en la base
 * de datos y obtiene la sucursal correspondiente.
 *
 * Convierte los resultados de la consulta en
 * entidades del modelo para generar reportes.
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Inventario_Logistica.Contratos;
using CapaModelo_Inventario_Logistica.Entidades;

namespace CapaModelo_Inventario_Logistica.Repositorios
{
    public class ClsRepositorioBodega
        : ClsRepositorioMaestro, ClsIRepositorioBodega
    {
        private readonly string _SelectAll;

        public ClsRepositorioBodega()
        {
            _SelectAll =
                "SELECT " +
                "b.idBodega AS Pk_Bodega, " +
                "b.nombreBodega AS NombreBodega, " +
                "b.Fk_Sucursal_Codigo AS Fk_Sucursal_Codigo, " +
                "s.nombreSucursal AS NombreSucursal, " +
                "b.paisBodega AS PaisBodega, " +
                "b.created_at AS CreatedAt, " +
                "b.updated_at AS UpdatedAt " +
                "FROM tblBodega b " +
                "LEFT JOIN tblSucursal s " +
                "ON b.Fk_Sucursal_Codigo = s.idSucursal " +
                "ORDER BY b.idBodega";
        }

        public IEnumerable<ClsBodega> LogisticaMetObtenerTodos()
        {
            var ListaBodegas = new List<ClsBodega>();

            var TablaDatos = LogisticaMetEjecucionConsulta(
                _SelectAll,
                CommandType.Text
            );

            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Bodega = new ClsBodega
                {
                    Pk_Bodega =
                        Convert.ToInt32(Fila["Pk_Bodega"]),

                    NombreBodega =
                        Convert.ToString(Fila["NombreBodega"]),

                    Fk_Sucursal_Codigo =
                        Convert.ToInt32(Fila["Fk_Sucursal_Codigo"]),

                    NombreSucursal =
                        Convert.ToString(Fila["NombreSucursal"]),

                    PaisBodega =
                        Convert.ToString(Fila["PaisBodega"]),

                    CreatedAt =
                        Convert.ToDateTime(Fila["CreatedAt"]),

                    UpdatedAt =
                        Convert.ToDateTime(Fila["UpdatedAt"])
                };

                ListaBodegas.Add(Bodega);
            }

            return ListaBodegas;
        }
    }
}
