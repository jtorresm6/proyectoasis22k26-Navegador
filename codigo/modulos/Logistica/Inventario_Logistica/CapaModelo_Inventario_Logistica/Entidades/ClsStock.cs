
/*
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsStock.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA: 07/11/2026
 * DESCRIPCIÓN: Representa los campos de tblStock.
 */

namespace CapaModelo_Inventario_Logistica.Entidades
{
    public class ClsStock
    {
        public int IdStock { get; set; }

        public int FkSucursalCodigo { get; set; }
        public int FkBodegaCodigo { get; set; }
        public int FkProductoCodigo { get; set; }

        public int StockFisicoStock { get; set; }
        public int StockVirtualStock { get; set; }
        public int StockAnteriorStock { get; set; }
        public int StockActualStock { get; set; }
        public int CargosMesStock { get; set; }
        public int AbonosMesStock { get; set; }
        public long CargosAcumuladosStock { get; set; }
        public long AbonosAcumuladosStock { get; set; }
        public int ApartadosStock { get; set; }
        public bool EstadoVentaStock { get; set; }
        public bool EstadoCompraStock { get; set; }
        // Datos para las consultas.
        public string NombreSucursal { get; set; }
        public string NombreBodega { get; set; }
        public string NombreProducto { get; set; }
    }
}
