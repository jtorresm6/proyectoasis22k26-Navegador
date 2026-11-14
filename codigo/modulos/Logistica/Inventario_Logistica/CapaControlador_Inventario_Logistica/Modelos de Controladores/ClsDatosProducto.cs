
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsDatosProducto.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA DE REALIZACIÓN: 06/11/2026
 *
 * DESCRIPCIÓN:
 * Representa los datos del producto y sus validaciones.
 * Permite trasladar información entre la Vista y el Controlador,
 * incluyendo un indicador booleano de imagen para la consulta.
 * ============================================================================
 */

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public class ClsDatosProducto
    {
        // Identificador.
        public int IdProducto { get; set; }

        // Datos generales.
        [Required(ErrorMessage = "Completar todos los campos.")]
        [StringLength(30)]
        public string SkuProducto { get; set; }

        [Required(ErrorMessage = "Completar todos los campos.")]
        [StringLength(5, MinimumLength = 1)]
        public string ClasificacionProducto { get; set; }

        [Required(ErrorMessage = "Completar todos los campos.")]
        [StringLength(100)]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "Completar todos los campos.")]
        [StringLength(255)]
        public string DescripcionProducto { get; set; }

        [Required(ErrorMessage = "Completar todos los campos.")]
        [StringLength(100)]
        public string PaisProducto { get; set; }

        // Llaves foráneas.
        [Range(1, int.MaxValue,
            ErrorMessage = "Completar todos los campos.")]
        public int IdProveedorProductos { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Completar todos los campos.")]
        public int FkMarcaCodigo { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Completar todos los campos.")]
        public int FkCategoriaCodigo { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Completar todos los campos.")]
        public int FkLineaCodigo { get; set; }

        // Precios y existencias.
        public decimal PrecioCostoProducto { get; set; }
        public decimal PrecioVentaProducto { get; set; }
        public decimal PorcentajeComisionProducto { get; set; }
        public int StockMinimoProducto { get; set; }
        public int StockMaximoProducto { get; set; }
        public decimal PesoProducto { get; set; }

        // Imagen almacenada en la base de datos.
        [Required(ErrorMessage = "Completar todos los campos.")]
        [Browsable(false)]
        public byte[] ImagenProducto { get; set; }

        // Indicador booleano para el DataGridView.
        public bool TieneImagen
        {
            get
            {
                return ImagenProducto != null &&
                       ImagenProducto.Length > 0;
            }
        }

        // Nombres de catálogos para consultas.
        public string NombreCategoria { get; set; }
        public string NombreMarca { get; set; }
        public string NombreLinea { get; set; }
        public string NombreProveedor { get; set; }
    }
}
