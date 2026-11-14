/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsModeloProducto.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * DESCRIPCIÓN: Valida productos, ejecuta altas, cambios y bajas a través del contrato de repositorio, y consulta catálogos.
 * FECHA DE REALIZACIÓN: 05/11/2026
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CapaModelo_Inventario_Logistica.Contratos;
using CapaModelo_Inventario_Logistica.Entidades;
using CapaModelo_Inventario_Logistica.Repositorios;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public class ClsModeloProducto
    {
        private readonly ClsIRepositorioProducto _RepositorioProducto;

        public ClsEstadoEntidad Estado { get; set; }

        public ClsDatosProducto Producto { get; set; }

        public ClsModeloProducto()
        {
            _RepositorioProducto = new ClsRepositorioProducto();

            Producto = new ClsDatosProducto();
        }

        // Permite inyectar un repositorio falso en las pruebas unitarias.
        // El constructor original sigue funcionando con ODBC.
        public ClsModeloProducto(ClsIRepositorioProducto Repositorio)
        {
            _RepositorioProducto = Repositorio
                ?? throw new ArgumentNullException(nameof(Repositorio));

            Producto = new ClsDatosProducto();
        }

        // Ejecuta la operación seleccionada.
        public string LogisticaFuncGrabarCambios()
        {
            if (Producto == null)
            {
                throw new ValidationException(
                    "No hay datos del producto.");
            }

            int FilasAfectadas;
            string Mensaje;

            switch (Estado)
            {
                case ClsEstadoEntidad.Added:

                    LogisticaMetValidarProducto(Producto);

                    FilasAfectadas =
                        _RepositorioProducto.LogisticaMetAgregar(
                            LogisticaFuncCrearEntidad(Producto));

                    Mensaje = "Producto guardado correctamente.";
                    break;

                case ClsEstadoEntidad.Modified:

                    if (Producto.IdProducto <= 0)
                    {
                        throw new ValidationException(
                            "Seleccione el producto a modificar.");
                    }

                    LogisticaMetValidarProducto(Producto);

                    FilasAfectadas =
                        _RepositorioProducto.LogisticaMetEditar(
                            LogisticaFuncCrearEntidad(Producto));

                    Mensaje = "Producto modificado correctamente.";
                    break;

                case ClsEstadoEntidad.Deleted:

                    if (Producto.IdProducto <= 0)
                    {
                        throw new ValidationException(
                            "Seleccione el producto a eliminar.");
                    }

                    FilasAfectadas =
                        _RepositorioProducto.LogisticaMetEliminar(
                            Producto.IdProducto);

                    Mensaje = "Producto eliminado correctamente.";
                    break;

                default:
                    throw new InvalidOperationException(
                        "La operación no es válida.");
            }

            if (FilasAfectadas <= 0)
            {
                throw new InvalidOperationException(
                    "No se modificó ningún registro.");
            }

            return Mensaje;
        }

        // Valida los campos antes de guardar.
        private void LogisticaMetValidarProducto(
            ClsDatosProducto Datos)
        {
            var Contexto = new ValidationContext(Datos);

            var Resultados = new List<ValidationResult>();

            bool EsValido = Validator.TryValidateObject(
                Datos,
                Contexto,
                Resultados,
                true);

            if (!EsValido)
            {
                throw new ValidationException(
                    string.Join("\n",
                        Resultados.Select(R => R.ErrorMessage)));
            }

            if (string.IsNullOrWhiteSpace(Datos.SkuProducto) ||
                string.IsNullOrWhiteSpace(Datos.NombreProducto) ||
                string.IsNullOrWhiteSpace(Datos.ClasificacionProducto) ||
                string.IsNullOrWhiteSpace(Datos.DescripcionProducto) ||
                string.IsNullOrWhiteSpace(Datos.PaisProducto))
            {
                throw new ValidationException(
                    "Complete los campos obligatorios.");
            }

            if (Datos.ImagenProducto == null ||
                Datos.ImagenProducto.Length == 0)
            {
                throw new ValidationException(
                    "Seleccione una imagen del producto.");
            }

            if (Datos.PrecioCostoProducto < 0 ||
                Datos.PrecioCostoProducto > 99999999.9999m ||
                Datos.PrecioVentaProducto < 0 ||
                Datos.PrecioVentaProducto > 99999999.9999m)
            {
                throw new ValidationException(
                    "Los precios están fuera del rango permitido.");
            }

            if (Datos.PorcentajeComisionProducto < 0 ||
                Datos.PorcentajeComisionProducto > 0.99m)
            {
                throw new ValidationException(
                    "La comisión debe estar entre 0.00 y 0.99.");
            }

            if (Datos.StockMinimoProducto < 0 ||
                Datos.StockMaximoProducto < Datos.StockMinimoProducto)
            {
                throw new ValidationException(
                    "El stock máximo debe ser mayor o igual " +
                    "al mínimo, y ambos deben ser no negativos.");
            }

            if (Datos.PesoProducto < 0 ||
                Datos.PesoProducto > 99.99m)
            {
                throw new ValidationException(
                    "El peso debe estar entre 0 y 99.99.");
            }

            if (decimal.Round(Datos.PesoProducto, 2) !=
                Datos.PesoProducto ||
                decimal.Round(Datos.PorcentajeComisionProducto, 2) !=
                Datos.PorcentajeComisionProducto)
            {
                throw new ValidationException(
                    "El peso y la comisión permiten " +
                    "máximo dos decimales.");
            }

            if (decimal.Round(Datos.PrecioCostoProducto, 4) !=
                Datos.PrecioCostoProducto ||
                decimal.Round(Datos.PrecioVentaProducto, 4) !=
                Datos.PrecioVentaProducto)
            {
                throw new ValidationException(
                    "Los precios permiten máximo cuatro decimales.");
            }
        }

        // Convierte los datos a la entidad de base de datos.
        private ClsProducto LogisticaFuncCrearEntidad(
            ClsDatosProducto Datos)
        {
            return new ClsProducto
            {
                IdProducto = Datos.IdProducto,
                SkuProducto = Datos.SkuProducto,
                ClasificacionProducto = Datos.ClasificacionProducto,
                NombreProducto = Datos.NombreProducto,
                IdProveedorProductos = Datos.IdProveedorProductos,
                FkMarcaCodigo = Datos.FkMarcaCodigo,
                FkCategoriaCodigo = Datos.FkCategoriaCodigo,
                FkLineaCodigo = Datos.FkLineaCodigo,
                DescripcionProducto = Datos.DescripcionProducto,
                ImagenProducto = Datos.ImagenProducto,
                PaisProducto = Datos.PaisProducto,
                PrecioCostoProducto = Datos.PrecioCostoProducto,
                PrecioVentaProducto = Datos.PrecioVentaProducto,
                PorcentajeComisionProducto =
                    Datos.PorcentajeComisionProducto,
                StockMinimoProducto = Datos.StockMinimoProducto,
                StockMaximoProducto = Datos.StockMaximoProducto,
                PesoProducto = Datos.PesoProducto
            };
        }

        // Obtiene los productos registrados.
        public List<ClsDatosProducto> LogisticaFuncObtenerTodos()
        {
            return _RepositorioProducto
                .LogisticaMetObtenerTodos()
                .Select(P => new ClsDatosProducto
                {
                    IdProducto = P.IdProducto,
                    SkuProducto = P.SkuProducto,
                    ClasificacionProducto = P.ClasificacionProducto,
                    NombreProducto = P.NombreProducto,
                    IdProveedorProductos = P.IdProveedorProductos,
                    FkMarcaCodigo = P.FkMarcaCodigo,
                    FkCategoriaCodigo = P.FkCategoriaCodigo,
                    FkLineaCodigo = P.FkLineaCodigo,
                    DescripcionProducto = P.DescripcionProducto,
                    ImagenProducto = P.ImagenProducto,
                    PaisProducto = P.PaisProducto,
                    PrecioCostoProducto = P.PrecioCostoProducto,
                    PrecioVentaProducto = P.PrecioVentaProducto,
                    PorcentajeComisionProducto =
                        P.PorcentajeComisionProducto,
                    StockMinimoProducto = P.StockMinimoProducto,
                    StockMaximoProducto = P.StockMaximoProducto,
                    PesoProducto = P.PesoProducto,
                    NombreCategoria = P.NombreCategoria,
                    NombreMarca = P.NombreMarca,
                    NombreLinea = P.NombreLinea,
                    NombreProveedor = P.NombreProveedor
                })
                .ToList();
        }

        // Busca productos por ID, SKU o nombre.
        public List<ClsDatosProducto> LogisticaFuncBuscar(
            string Filtro)
        {
            var Lista = LogisticaFuncObtenerTodos();

            if (string.IsNullOrWhiteSpace(Filtro))
            {
                return Lista;
            }

            return Lista.Where(P =>
                P.IdProducto.ToString().Contains(Filtro) ||
                (P.SkuProducto ?? "").IndexOf(
                    Filtro,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||
                (P.NombreProducto ?? "").IndexOf(
                    Filtro,
                    StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();
        }

        private List<ClsOpcionCatalogo> LogisticaFuncOpciones(
            IEnumerable<ClsCatalogo> Catalogo)
        {
            return Catalogo.Select(C => new ClsOpcionCatalogo
            {
                Id = C.Id,
                Nombre = C.Nombre
            }).ToList();
        }

        public List<ClsOpcionCatalogo> LogisticaFuncCategorias()
        {
            return LogisticaFuncOpciones(
                _RepositorioProducto.LogisticaMetObtenerCategorias());
        }

        public List<ClsOpcionCatalogo> LogisticaFuncMarcas()
        {
            return LogisticaFuncOpciones(
                _RepositorioProducto.LogisticaMetObtenerMarcas());
        }

        public List<ClsOpcionCatalogo> LogisticaFuncLineas()
        {
            return LogisticaFuncOpciones(
                _RepositorioProducto.LogisticaMetObtenerLineas());
        }

        public List<ClsOpcionCatalogo> LogisticaFuncProveedores()
        {
            return LogisticaFuncOpciones(
                _RepositorioProducto.LogisticaMetObtenerProveedores());
        }

        // Busca el identificador de la categoría
        // escrita en el TextBox del formulario.
        public int LogisticaFuncObtenerIdCategoria(string Nombre)
        {
            var Categoria = LogisticaFuncCategorias()
                .FirstOrDefault(C =>
                    string.Equals(
                        C.Nombre,
                        Nombre == null ? "" : Nombre.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            return Categoria == null ? 0 : Categoria.Id;
        }
    }
}
