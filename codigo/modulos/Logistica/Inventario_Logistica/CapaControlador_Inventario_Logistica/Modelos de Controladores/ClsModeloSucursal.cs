
/*
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsModeloSucursal.Informe.cs
 * RESPONSABLE: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Obtiene todos los datos de sucursales y
 * los adapta para el diseñador RDLC.
 */

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Inventario_Logistica.Repositorios;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public partial class ClsModeloSucursal
    {
        public int IdSucursalInforme { get; set; }

        public string NombreSucursalInforme { get; set; }

        public string DescripcionSucursalInforme { get; set; }

        public string UbicacionSucursalInforme { get; set; }

        public string PaisSucursalInforme { get; set; }

        public DateTime FechaCreacionInforme { get; set; }

        public DateTime FechaActualizacionInforme { get; set; }

        public List<ClsModeloSucursal> LogisticaMetObtenerTodosInforme()
        {
            var Repositorio = new ClsRepositorioSucursal();

            DataTable TablaDatos =
                Repositorio.LogisticaMetObtenerDatosInforme();

            var ListaSucursales = new List<ClsModeloSucursal>();

            foreach (DataRow Fila in TablaDatos.Rows)
            {
                ListaSucursales.Add(new ClsModeloSucursal
                {
                    IdSucursalInforme =
                        Convert.ToInt32(Fila["idSucursal"]),

                    NombreSucursalInforme =
                        Convert.ToString(Fila["nombreSucursal"]),

                    DescripcionSucursalInforme =
                        Convert.ToString(Fila["descripcionSucursal"]),

                    UbicacionSucursalInforme =
                        Convert.ToString(Fila["ubicacionSucursal"]),

                    PaisSucursalInforme =
                        Convert.ToString(Fila["paisSucursal"]),

                    FechaCreacionInforme =
                        Convert.ToDateTime(Fila["created_at"]),

                    FechaActualizacionInforme =
                        Convert.ToDateTime(Fila["updated_at"])
                });
            }

            return ListaSucursales;
        }
    }
}
