
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsSucursal.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Representa los datos de las sucursales registradas
 * en la base de datos dbLogistica.
 *
 * Contiene las propiedades correspondientes a los
 * campos de la tabla tblSucursal.
 * ============================================================================
 */

using System;

namespace CapaModelo_Inventario_Logistica.Entidades
{
    public class ClsSucursal
    {
        // =====================================================
        // IDENTIFICADOR DE LA SUCURSAL
        // =====================================================

        public int Pk_Sucursal { get; set; }

        // =====================================================
        // INFORMACIÓN GENERAL
        // =====================================================

        public string NombreSucursal { get; set; }

        public string DescripcionSucursal { get; set; }

        public string UbicacionSucursal { get; set; }

        public string PaisSucursal { get; set; }

        // =====================================================
        // FECHAS DE REGISTRO Y ACTUALIZACIÓN
        // =====================================================

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
