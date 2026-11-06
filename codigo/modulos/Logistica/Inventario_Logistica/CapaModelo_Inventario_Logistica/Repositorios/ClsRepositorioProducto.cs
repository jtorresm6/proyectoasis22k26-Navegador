/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsRepositorioProducto.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * DESCRIPCIÓN: Realiza el mantenimiento de tblProducto, consultas con JOIN y carga de catálogos, usando consultas parametrizadas.
 * FECHA DE REALIZACIÓN: 05/11/2026
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

using CapaModelo_Inventario_Logistica.Contratos;
using CapaModelo_Inventario_Logistica.Entidades;

namespace CapaModelo_Inventario_Logistica.Repositorios
{
    public class ClsRepositorioProducto :
       ClsRepositorioMaestro,
       ClsIRepositorioProducto
    {
        private const string _Columnas =
            "skuProducto, clasificacionProducto, nombreProducto, " +
            "idProveedorProductos, Fk_Marca_Codigo, " +
            "Fk_Categoria_Codigo, Fk_Linea_Codigo, " +
            "descripcionProducto, imagenProducto, paisProducto, " +
            "precioCostoProducto, precioVentaProducto, " +
            "porcentajeComisionProducto, stockMinimoProducto, " +
            "stockMaximoProducto, pesoProducto";

        private const string _Valores =
            "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?";

        // INSERT: registra un producto nuevo.
        public int LogisticaMetAgregar(ClsProducto Entidad)
        {
            string Consulta =
                "INSERT INTO tblProducto (" + _Columnas + ") " +
                "VALUES (" + _Valores + ")";

            return LogisticaMetEjecutarOperacion(
                Consulta,
                LogisticaFuncParametros(Entidad));
        }

        // UPDATE: modifica un producto existente.
        public int LogisticaMetEditar(ClsProducto Entidad)
        {
            string Consulta =
                "UPDATE tblProducto SET " +
                "skuProducto = ?, " +
                "clasificacionProducto = ?, " +
                "nombreProducto = ?, " +
                "idProveedorProductos = ?, " +
                "Fk_Marca_Codigo = ?, " +
                "Fk_Categoria_Codigo = ?, " +
                "Fk_Linea_Codigo = ?, " +
                "descripcionProducto = ?, " +
                "imagenProducto = ?, " +
                "paisProducto = ?, " +
                "precioCostoProducto = ?, " +
                "precioVentaProducto = ?, " +
                "porcentajeComisionProducto = ?, " +
                "stockMinimoProducto = ?, " +
                "stockMaximoProducto = ?, " +
                "pesoProducto = ? " +
                "WHERE idProducto = ?";

            var Parametros = LogisticaFuncParametros(Entidad);

            Parametros.Add(
                new OdbcParameter("idProducto", OdbcType.Int)
                {
                    Value = Entidad.IdProducto
                });

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // DELETE: elimina el producto por ID.
        public int LogisticaMetEliminar(int IdProducto)
        {
            string Consulta =
                "DELETE FROM tblProducto WHERE idProducto = ?";

            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("idProducto", OdbcType.Int)
                {
                    Value = IdProducto
                }
            };

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // SELECT: consulta todos los productos.
        public IEnumerable<ClsProducto> LogisticaMetObtenerTodos()
        {
            string Consulta = @"
                SELECT
                    p.*,
                    c.nombreCategoria AS NombreCategoria,
                    m.nombreMarca AS NombreMarca,
                    l.nombreLinea AS NombreLinea,
                    v.nombreProveedor AS NombreProveedor
                FROM tblProducto p
                INNER JOIN tblCategoria c
                    ON p.Fk_Categoria_Codigo = c.idCategoria
                INNER JOIN tblMarca m
                    ON p.Fk_Marca_Codigo = m.idMarca
                INNER JOIN tblLinea l
                    ON p.Fk_Linea_Codigo = l.idLinea
                INNER JOIN tblProveedor v
                    ON p.idProveedorProductos = v.idProveedor
                ORDER BY p.idProducto DESC";

            DataTable Tabla = LogisticaMetEjecucionConsulta(
                Consulta,
                CommandType.Text);

            var Lista = new List<ClsProducto>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                var Producto = new ClsProducto
                {
                    IdProducto =
                        Convert.ToInt32(Fila["idProducto"]),

                    SkuProducto =
                        Convert.ToString(Fila["skuProducto"]),

                    ClasificacionProducto =
                        Convert.ToString(Fila["clasificacionProducto"]),

                    NombreProducto =
                        Convert.ToString(Fila["nombreProducto"]),

                    IdProveedorProductos =
                        Convert.ToInt32(Fila["idProveedorProductos"]),

                    FkMarcaCodigo =
                        Convert.ToInt32(Fila["Fk_Marca_Codigo"]),

                    FkCategoriaCodigo =
                        Convert.ToInt32(Fila["Fk_Categoria_Codigo"]),

                    FkLineaCodigo =
                        Convert.ToInt32(Fila["Fk_Linea_Codigo"]),

                    DescripcionProducto =
                        Convert.ToString(Fila["descripcionProducto"]),

                    ImagenProducto =
                        (byte[])Fila["imagenProducto"],

                    PaisProducto =
                        Convert.ToString(Fila["paisProducto"]),

                    PrecioCostoProducto =
                        Convert.ToDecimal(Fila["precioCostoProducto"]),

                    PrecioVentaProducto =
                        Convert.ToDecimal(Fila["precioVentaProducto"]),

                    PorcentajeComisionProducto =
                        Convert.ToDecimal(
                            Fila["porcentajeComisionProducto"]),

                    StockMinimoProducto =
                        Convert.ToInt32(Fila["stockMinimoProducto"]),

                    StockMaximoProducto =
                        Convert.ToInt32(Fila["stockMaximoProducto"]),

                    PesoProducto =
                        Convert.ToDecimal(Fila["pesoProducto"]),

                    NombreCategoria =
                        Convert.ToString(Fila["NombreCategoria"]),

                    NombreMarca =
                        Convert.ToString(Fila["NombreMarca"]),

                    NombreLinea =
                        Convert.ToString(Fila["NombreLinea"]),

                    NombreProveedor =
                        Convert.ToString(Fila["NombreProveedor"])
                };

                Lista.Add(Producto);
            }

            return Lista;
        }

        // Catálogo de categorías.
        public IEnumerable<ClsCatalogo> LogisticaMetObtenerCategorias()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idCategoria AS Id, " +
                "nombreCategoria AS Nombre " +
                "FROM tblCategoria ORDER BY nombreCategoria");
        }

        // Catálogo de marcas.
        public IEnumerable<ClsCatalogo> LogisticaMetObtenerMarcas()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idMarca AS Id, " +
                "nombreMarca AS Nombre " +
                "FROM tblMarca ORDER BY nombreMarca");
        }

        // Catálogo de líneas.
        public IEnumerable<ClsCatalogo> LogisticaMetObtenerLineas()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idLinea AS Id, " +
                "nombreLinea AS Nombre " +
                "FROM tblLinea ORDER BY nombreLinea");
        }

        // Catálogo de proveedores.
        public IEnumerable<ClsCatalogo> LogisticaMetObtenerProveedores()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idProveedor AS Id, " +
                "nombreProveedor AS Nombre " +
                "FROM tblProveedor ORDER BY nombreProveedor");
        }

        // Convierte resultados de catálogos en objetos.
        private IEnumerable<ClsCatalogo>
            LogisticaFuncConsultarCatalogo(string Consulta)
        {
            DataTable Tabla = LogisticaMetEjecucionConsulta(
                Consulta,
                CommandType.Text);

            var Lista = new List<ClsCatalogo>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Lista.Add(new ClsCatalogo
                {
                    Id = Convert.ToInt32(Fila["Id"]),
                    Nombre = Convert.ToString(Fila["Nombre"])
                });
            }

            return Lista;
        }

        // Construye los parámetros para INSERT y UPDATE.
        // ODBC utiliza el orden de los signos ?.
        private List<OdbcParameter> LogisticaFuncParametros(
            ClsProducto Entidad)
        {
            return new List<OdbcParameter>
            {
                new OdbcParameter("skuProducto", OdbcType.VarChar)
                {
                    Value = Entidad.SkuProducto
                },

                new OdbcParameter("clasificacionProducto", OdbcType.Char)
                {
                    Value = Entidad.ClasificacionProducto
                },

                new OdbcParameter("nombreProducto", OdbcType.VarChar)
                {
                    Value = Entidad.NombreProducto
                },

                new OdbcParameter("idProveedorProductos", OdbcType.Int)
                {
                    Value = Entidad.IdProveedorProductos
                },

                new OdbcParameter("Fk_Marca_Codigo", OdbcType.Int)
                {
                    Value = Entidad.FkMarcaCodigo
                },

                new OdbcParameter("Fk_Categoria_Codigo", OdbcType.Int)
                {
                    Value = Entidad.FkCategoriaCodigo
                },

                new OdbcParameter("Fk_Linea_Codigo", OdbcType.Int)
                {
                    Value = Entidad.FkLineaCodigo
                },

                new OdbcParameter("descripcionProducto", OdbcType.VarChar)
                {
                    Value = Entidad.DescripcionProducto
                },

                new OdbcParameter("imagenProducto", OdbcType.Image)
                {
                    Value = Entidad.ImagenProducto
                },

                new OdbcParameter("paisProducto", OdbcType.VarChar)
                {
                    Value = Entidad.PaisProducto
                },

                new OdbcParameter("precioCostoProducto", OdbcType.Decimal)
                {
                    Precision = 12,
                    Scale = 4,
                    Value = Entidad.PrecioCostoProducto
                },

                new OdbcParameter("precioVentaProducto", OdbcType.Decimal)
                {
                    Precision = 12,
                    Scale = 4,
                    Value = Entidad.PrecioVentaProducto
                },

                new OdbcParameter(
                    "porcentajeComisionProducto", OdbcType.Decimal)
                {
                    Precision = 3,
                    Scale = 2,
                    Value = Entidad.PorcentajeComisionProducto
                },

                new OdbcParameter("stockMinimoProducto", OdbcType.Int)
                {
                    Value = Entidad.StockMinimoProducto
                },

                new OdbcParameter("stockMaximoProducto", OdbcType.Int)
                {
                    Value = Entidad.StockMaximoProducto
                },

                new OdbcParameter("pesoProducto", OdbcType.Decimal)
                {
                    Precision = 4,
                    Scale = 2,
                    Value = Entidad.PesoProducto
                }
            };
        }

        // Ejecuta INSERT, UPDATE y DELETE sin modificar ClsRepositorioMaestro.
        private int LogisticaMetEjecutarOperacion(
            string ComandoTexto,
            List<OdbcParameter> Parametros)
        {
            using (var Conexion = LogisticaMetObtenerConexion())
            {
                Conexion.Open();
                using (var Comando = new OdbcCommand())
                {
                    Comando.Connection = Conexion;
                    Comando.CommandText = ComandoTexto;
                    Comando.CommandType = CommandType.Text;
                    Comando.Parameters.AddRange(Parametros.ToArray());
                    return Comando.ExecuteNonQuery();
                }
            }
        }
    }
}
