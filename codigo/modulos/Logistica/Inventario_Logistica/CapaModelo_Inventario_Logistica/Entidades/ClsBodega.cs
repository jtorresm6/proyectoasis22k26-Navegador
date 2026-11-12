
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsBodega.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Representa los datos de una bodega registrados
 * en la base de datos, incluyendo la información
 * de la sucursal a la que pertenece.
 * ============================================================================
 */

using System;

namespace CapaModelo_Inventario_Logistica.Entidades
{
    public class ClsBodega
    {
        public int Pk_Bodega { get; set; }

        public string NombreBodega { get; set; }

        public int Fk_Sucursal_Codigo { get; set; }

        public string NombreSucursal { get; set; }

        public string PaisBodega { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
