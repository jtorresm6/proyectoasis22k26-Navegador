
/*
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsDatosStock.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA: 07/11/2026
 * DESCRIPCIÓN: Datos y validaciones del stock
 * utilizados entre la Vista y el Controlador.
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public class ClsDatosStock
    {
        public int IdStock { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Seleccione una sucursal.")]
        public int FkSucursalCodigo { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Seleccione una bodega.")]
        public int FkBodegaCodigo { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Seleccione un producto.")]
        public int FkProductoCodigo { get; set; }

        [Range(0, int.MaxValue)]
        public int StockFisicoStock { get; set; }

        [Range(0, int.MaxValue)]
        public int StockVirtualStock { get; set; }

        [Range(0, int.MaxValue)]
        public int StockAnteriorStock { get; set; }

        [Range(0, int.MaxValue)]
        public int StockActualStock { get; set; }

        [Range(0, int.MaxValue)]
        public int CargosMesStock { get; set; }

        [Range(0, int.MaxValue)]
        public int AbonosMesStock { get; set; }

        [Range(typeof(long), "0",
            "9223372036854775807")]
        public long CargosAcumuladosStock { get; set; }

        [Range(typeof(long), "0",
            "9223372036854775807")]
        public long AbonosAcumuladosStock { get; set; }

        [Range(0, int.MaxValue)]
        public int ApartadosStock { get; set; }

        public bool EstadoVentaStock { get; set; }
        public bool EstadoCompraStock { get; set; }

        // Indicador para el CheckBox Estado Stock.
        // No es una columna de la base de datos.
        public bool TieneExistencias
        {
            get
            {
                return StockActualStock > 0;
            }
        }

        public string NombreSucursal { get; set; }
        public string NombreBodega { get; set; }
        public string NombreProducto { get; set; }
    }
}
