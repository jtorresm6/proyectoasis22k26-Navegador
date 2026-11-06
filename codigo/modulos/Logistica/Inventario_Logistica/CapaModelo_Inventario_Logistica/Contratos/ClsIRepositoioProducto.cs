/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsIRepositorioProducto.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * DESCRIPCIÓN: Define el contrato de operaciones CRUD y consulta de catálogos para tblProducto.
 * FECHA DE REALIZACIÓN: 05/11/2026
 * ============================================================================
 */

using System.Collections.Generic;
using CapaModelo_Inventario_Logistica.Entidades;

namespace CapaModelo_Inventario_Logistica.Contratos
{
    public interface ClsIRepositorioProducto : IRepositorioGenerico<ClsProducto>
    {
        int LogisticaMetAgregar(ClsProducto Entidad);
        int LogisticaMetEditar(ClsProducto Entidad);
        int LogisticaMetEliminar(int IdProducto);
        IEnumerable<ClsCatalogo> LogisticaMetObtenerCategorias();
        IEnumerable<ClsCatalogo> LogisticaMetObtenerMarcas();
        IEnumerable<ClsCatalogo> LogisticaMetObtenerLineas();
        IEnumerable<ClsCatalogo> LogisticaMetObtenerProveedores();
    }
}
