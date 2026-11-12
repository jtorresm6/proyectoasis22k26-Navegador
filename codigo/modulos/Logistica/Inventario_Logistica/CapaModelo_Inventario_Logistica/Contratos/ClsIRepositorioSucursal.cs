
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 *
 * REALIZADO POR: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Define el contrato del repositorio para consultar
 * las sucursales utilizadas en los reportes.
 * ============================================================================
 */

using CapaModelo_Inventario_Logistica.Entidades;

namespace CapaModelo_Inventario_Logistica.Contratos
{
    public interface ClsIRepositorioSucursal
        : IRepositorioGenerico<ClsSucursal>
    {
    }
}
