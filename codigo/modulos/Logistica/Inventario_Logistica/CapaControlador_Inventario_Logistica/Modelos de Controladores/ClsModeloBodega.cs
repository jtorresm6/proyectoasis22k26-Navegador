
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsModeloBodega.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Obtiene los registros de bodegas desde el repositorio
 * y adapta los datos para generar los reportes RDLC.
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using CapaModelo_Inventario_Logistica.Entidades;
using CapaModelo_Inventario_Logistica.Repositorios;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public class ClsModeloBodega
    {
        private readonly ClsRepositorioBodega _RepositorioBodega;

        // =====================================================
        // PROPIEDADES DEL MODELO
        // =====================================================

        public int PkBodega { get; set; }

        public string NombreBodega { get; set; }

        public int FkSucursalCodigo { get; set; }

        public string NombreSucursal { get; set; }

        public string PaisBodega { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public ClsModeloBodega()
        {
            _RepositorioBodega = new ClsRepositorioBodega();
        }

        // =====================================================
        // OBTENER TODAS LAS BODEGAS
        // =====================================================

        public List<ClsModeloBodega> LogisticaMetObtenerTodos()
        {
            var ResultadoConsulta =
                _RepositorioBodega.LogisticaMetObtenerTodos();

            var ListaBodegas = new List<ClsModeloBodega>();

            foreach (ClsBodega Item in ResultadoConsulta)
            {
                ListaBodegas.Add(new ClsModeloBodega
                {
                    PkBodega = Item.Pk_Bodega,

                    NombreBodega = Item.NombreBodega,

                    FkSucursalCodigo = Item.Fk_Sucursal_Codigo,

                    NombreSucursal = Item.NombreSucursal,

                    PaisBodega = Item.PaisBodega,

                    CreatedAt = Item.CreatedAt,

                    UpdatedAt = Item.UpdatedAt
                });
            }

            return ListaBodegas;
        }
    }
}
