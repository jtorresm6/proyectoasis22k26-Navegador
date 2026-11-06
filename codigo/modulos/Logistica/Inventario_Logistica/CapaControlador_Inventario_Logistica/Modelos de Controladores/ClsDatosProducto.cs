/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsDatosProducto.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * DESCRIPCIÓN: Transporta los datos del producto entre la vista y el controlador y define validaciones de entrada.
 * FECHA DE REALIZACIÓN: 05/11/2026
 * ============================================================================
 */

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public class ClsDatosProducto
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El SKU es obligatorio.")]
        [StringLength(30, ErrorMessage = "El SKU admite hasta 30 caracteres.")]
        public string SkuProducto { get; set; }

        [Required(ErrorMessage = "La clasificación es obligatoria.")]
        [StringLength(5, MinimumLength = 1,
            ErrorMessage = "La clasificación admite entre 1 y 5 caracteres.")]
        public string ClasificacionProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre admite hasta 100 caracteres.")]
        public string NombreProducto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un proveedor válido.")]
        public int IdProveedorProductos { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una marca válida.")]
        public int FkMarcaCodigo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        public int FkCategoriaCodigo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una línea válida.")]
        public int FkLineaCodigo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(255, ErrorMessage = "La descripción admite hasta 255 caracteres.")]
        public string DescripcionProducto { get; set; }

        [Required(ErrorMessage = "Debe agregar una imagen.")]
        [Browsable(false)]
        public byte[] ImagenProducto { get; set; }

        [Required(ErrorMessage = "El país es obligatorio.")]
        [StringLength(100, ErrorMessage = "El país admite hasta 100 caracteres.")]
        public string PaisProducto { get; set; }

        public decimal PrecioCostoProducto { get; set; }
        public decimal PrecioVentaProducto { get; set; }
        public decimal PorcentajeComisionProducto { get; set; }
        public int StockMinimoProducto { get; set; }
        public int StockMaximoProducto { get; set; }
        public decimal PesoProducto { get; set; }

        // Nombres legibles para la tabla de consultas del formulario.
        public string NombreCategoria { get; set; }
        public string NombreMarca { get; set; }
        public string NombreLinea { get; set; }
        public string NombreProveedor { get; set; }
    }
}
