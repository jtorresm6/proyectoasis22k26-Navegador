/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 * Descripción: Representa los datos de un detalle asociado a un movimiento de inventario.
 */
using System;

namespace CapaModelo_Inventario_Logistica.Entidades
{
    public class ClsDetalleMovimiento
    {
        public int Pk_DetalleMovimiento { get; set; }
        public int Fk_MovimientoInventario_Codigo { get; set; }
        public string LoteMovimientoInventario { get; set; }
        public int Fk_Producto_Codigo { get; set; }
        public int CantidadDetalleMovimiento { get; set; }
        public decimal PrecioCompraDetalleMovimiento { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
