
/*
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsIRepositorioStock.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA: 07/11/2026
 * DESCRIPCIÓN: Contrato para operaciones y consultas de Stock.
 */

using System.Collections.Generic;
using CapaModelo_Inventario_Logistica.Entidades;

namespace CapaModelo_Inventario_Logistica.Contratos
{
    public interface ClsIRepositorioStock :
        IRepositorioGenerico<ClsStock>
    {
        int LogisticaMetAgregar(ClsStock Entidad);
        int LogisticaMetEditar(ClsStock Entidad);
        int LogisticaMetEliminar(int IdStock);
        IEnumerable<ClsCatalogo>
            LogisticaMetObtenerSucursales();

        IEnumerable<ClsCatalogo>
            LogisticaMetObtenerBodegas(int IdSucursal);

        IEnumerable<ClsCatalogo>
            LogisticaMetObtenerProductos();
    }
}
