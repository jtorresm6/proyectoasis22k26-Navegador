
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsRepositorioProducto.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA DE REALIZACIÓN: 06/11/2026
 *
 * DESCRIPCIÓN:
 * Realiza las operaciones CRUD de tblProducto mediante ODBC.
 * También consulta los catálogos relacionados y administra
 * el almacenamiento binario de las imágenes.
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Text;

using CapaModelo_Inventario_Logistica.Contratos;
using CapaModelo_Inventario_Logistica.Entidades;

namespace CapaModelo_Inventario_Logistica.Repositorios
{
    public class ClsRepositorioProducto :
        ClsRepositorioMaestro,
        ClsIRepositorioProducto
    {
        // ========================================================
        // AGREGAR PRODUCTO
        // ========================================================

        public int LogisticaMetAgregar(ClsProducto Entidad)
        {
            string Consulta = @"
                INSERT INTO tblProducto
                (
                    skuProducto,
                    clasificacionProducto,
                    nombreProducto,
                    idProveedorProductos,
                    Fk_Marca_Codigo,
                    Fk_Categoria_Codigo,
                    Fk_Linea_Codigo,
                    descripcionProducto,
                    imagenProducto,
                    paisProducto,
                    precioCostoProducto,
                    precioVentaProducto,
                    porcentajeComisionProducto,
                    stockMinimoProducto,
                    stockMaximoProducto,
                    pesoProducto
                )
                VALUES
                (
                    ?, ?, ?, ?, ?, ?, ?, ?,
                    ?, ?, ?, ?, ?, ?, ?, ?
                )";

            return LogisticaMetEjecutarOperacion(
                Consulta,
                LogisticaFuncParametros(Entidad));
        }

        // ========================================================
        // MODIFICAR PRODUCTO
        // ========================================================

        public int LogisticaMetEditar(ClsProducto Entidad)
        {
            string Consulta = @"
                UPDATE tblProducto SET
                    skuProducto = ?,
                    clasificacionProducto = ?,
                    nombreProducto = ?,
                    idProveedorProductos = ?,
                    Fk_Marca_Codigo = ?,
                    Fk_Categoria_Codigo = ?,
                    Fk_Linea_Codigo = ?,
                    descripcionProducto = ?,
                    imagenProducto = ?,
                    paisProducto = ?,
                    precioCostoProducto = ?,
                    precioVentaProducto = ?,
                    porcentajeComisionProducto = ?,
                    stockMinimoProducto = ?,
                    stockMaximoProducto = ?,
                    pesoProducto = ?
                WHERE idProducto = ?";

            List<OdbcParameter> Parametros =
                LogisticaFuncParametros(Entidad);

            Parametros.Add(
                new OdbcParameter(
                    "idProducto", OdbcType.Int)
                {
                    Value = Entidad.IdProducto
                });

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // ========================================================
        // ELIMINAR PRODUCTO
        // ========================================================

        public int LogisticaMetEliminar(int IdProducto)
        {
            string Consulta =
                "DELETE FROM tblProducto WHERE idProducto = ?";

            List<OdbcParameter> Parametros =
                new List<OdbcParameter>
                {
                    new OdbcParameter(
                        "idProducto", OdbcType.Int)
                    {
                        Value = IdProducto
                    }
                };

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // ========================================================
        // CONSULTAR TODOS LOS PRODUCTOS
        // ========================================================

        public IEnumerable<ClsProducto> LogisticaMetObtenerTodos()
        {
            string Consulta = @"
                SELECT
                    p.idProducto,
                    p.skuProducto,
                    p.clasificacionProducto,
                    p.nombreProducto,
                    p.idProveedorProductos,
                    p.Fk_Marca_Codigo,
                    p.Fk_Categoria_Codigo,
                    p.Fk_Linea_Codigo,
                    p.descripcionProducto,
                    p.imagenProducto,
                    p.paisProducto,
                    p.precioCostoProducto,
                    p.precioVentaProducto,
                    p.porcentajeComisionProducto,
                    p.stockMinimoProducto,
                    p.stockMaximoProducto,
                    p.pesoProducto,
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

            DataTable Tabla =
                LogisticaMetEjecucionConsulta(
                    Consulta,
                    CommandType.Text);

            List<ClsProducto> Lista =
                new List<ClsProducto>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                ClsProducto Producto = new ClsProducto
                {
                    IdProducto =
                        Convert.ToInt32(Fila["idProducto"]),

                    SkuProducto =
                        Convert.ToString(Fila["skuProducto"]),

                    ClasificacionProducto =
                        Convert.ToString(
                            Fila["clasificacionProducto"]),

                    NombreProducto =
                        Convert.ToString(Fila["nombreProducto"]),

                    IdProveedorProductos =
                        Convert.ToInt32(
                            Fila["idProveedorProductos"]),

                    FkMarcaCodigo =
                        Convert.ToInt32(
                            Fila["Fk_Marca_Codigo"]),

                    FkCategoriaCodigo =
                        Convert.ToInt32(
                            Fila["Fk_Categoria_Codigo"]),

                    FkLineaCodigo =
                        Convert.ToInt32(
                            Fila["Fk_Linea_Codigo"]),

                    DescripcionProducto =
                        Convert.ToString(
                            Fila["descripcionProducto"]),

                    ImagenProducto =
                        Fila["imagenProducto"] as byte[],

                    PaisProducto =
                        Convert.ToString(
                            Fila["paisProducto"]),

                    PrecioCostoProducto =
                        Convert.ToDecimal(
                            Fila["precioCostoProducto"]),

                    PrecioVentaProducto =
                        Convert.ToDecimal(
                            Fila["precioVentaProducto"]),

                    PorcentajeComisionProducto =
                        Convert.ToDecimal(
                            Fila["porcentajeComisionProducto"]),

                    StockMinimoProducto =
                        Convert.ToInt32(
                            Fila["stockMinimoProducto"]),

                    StockMaximoProducto =
                        Convert.ToInt32(
                            Fila["stockMaximoProducto"]),

                    PesoProducto =
                        Convert.ToDecimal(
                            Fila["pesoProducto"]),

                    NombreCategoria =
                        Convert.ToString(
                            Fila["NombreCategoria"]),

                    NombreMarca =
                        Convert.ToString(
                            Fila["NombreMarca"]),

                    NombreLinea =
                        Convert.ToString(
                            Fila["NombreLinea"]),

                    NombreProveedor =
                        Convert.ToString(
                            Fila["NombreProveedor"])
                };

                Lista.Add(Producto);
            }

            return Lista;
        }

        // ========================================================
        // CONSULTAR CATÁLOGOS
        // ========================================================

        public IEnumerable<ClsCatalogo>
            LogisticaMetObtenerCategorias()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idCategoria AS Id, " +
                "nombreCategoria AS Nombre " +
                "FROM tblCategoria ORDER BY nombreCategoria");
        }

        public IEnumerable<ClsCatalogo>
            LogisticaMetObtenerMarcas()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idMarca AS Id, " +
                "nombreMarca AS Nombre " +
                "FROM tblMarca ORDER BY nombreMarca");
        }

        public IEnumerable<ClsCatalogo>
            LogisticaMetObtenerLineas()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idLinea AS Id, " +
                "nombreLinea AS Nombre " +
                "FROM tblLinea ORDER BY nombreLinea");
        }

        public IEnumerable<ClsCatalogo>
            LogisticaMetObtenerProveedores()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idProveedor AS Id, " +
                "nombreProveedor AS Nombre " +
                "FROM tblProveedor ORDER BY nombreProveedor");
        }

        private IEnumerable<ClsCatalogo>
            LogisticaFuncConsultarCatalogo(string Consulta)
        {
            DataTable Tabla =
                LogisticaMetEjecucionConsulta(
                    Consulta,
                    CommandType.Text);

            List<ClsCatalogo> Catalogos =
                new List<ClsCatalogo>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Catalogos.Add(new ClsCatalogo
                {
                    Id = Convert.ToInt32(Fila["Id"]),
                    Nombre = Convert.ToString(Fila["Nombre"])
                });
            }

            return Catalogos;
        }

        // ========================================================
        // PARÁMETROS DEL PRODUCTO
        // El orden coincide con los signos ? del SQL.
        // ========================================================

        private List<OdbcParameter> LogisticaFuncParametros(
            ClsProducto Entidad)
        {
            if (Entidad == null ||
                Entidad.ImagenProducto == null ||
                Entidad.ImagenProducto.Length == 0)
            {
                throw new ArgumentException(
                    "Debe agregar una imagen válida del producto.");
            }

            return new List<OdbcParameter>
            {
                new OdbcParameter(
                    "skuProducto", OdbcType.VarChar, 30)
                {
                    Value = Entidad.SkuProducto
                },

                new OdbcParameter(
                    "clasificacionProducto", OdbcType.Char, 5)
                {
                    Value = Entidad.ClasificacionProducto
                },

                new OdbcParameter(
                    "nombreProducto", OdbcType.VarChar, 100)
                {
                    Value = Entidad.NombreProducto
                },

                new OdbcParameter(
                    "idProveedorProductos", OdbcType.Int)
                {
                    Value = Entidad.IdProveedorProductos
                },

                new OdbcParameter(
                    "Fk_Marca_Codigo", OdbcType.Int)
                {
                    Value = Entidad.FkMarcaCodigo
                },

                new OdbcParameter(
                    "Fk_Categoria_Codigo", OdbcType.Int)
                {
                    Value = Entidad.FkCategoriaCodigo
                },

                new OdbcParameter(
                    "Fk_Linea_Codigo", OdbcType.Int)
                {
                    Value = Entidad.FkLineaCodigo
                },

                new OdbcParameter(
                    "descripcionProducto", OdbcType.VarChar, 255)
                {
                    Value = Entidad.DescripcionProducto
                },

                // Imagen binaria para LONGBLOB.
                new OdbcParameter(
                    "imagenProducto", OdbcType.Image)
                {
                    Size = Entidad.ImagenProducto.Length,
                    Value = Entidad.ImagenProducto
                },

                new OdbcParameter(
                    "paisProducto", OdbcType.VarChar, 100)
                {
                    Value = Entidad.PaisProducto
                },

                // Alternativa para evitar el enlace
                // de parámetros OdbcType.Decimal.
                new OdbcParameter(
                    "precioCostoProducto", OdbcType.VarChar, 32)
                {
                    Value = Entidad.PrecioCostoProducto.ToString(
                        "0.0000",
                        CultureInfo.InvariantCulture)
                },

                new OdbcParameter(
                    "precioVentaProducto", OdbcType.VarChar, 32)
                {
                    Value = Entidad.PrecioVentaProducto.ToString(
                        "0.0000",
                        CultureInfo.InvariantCulture)
                },

                new OdbcParameter(
                    "porcentajeComisionProducto",
                    OdbcType.VarChar, 16)
                {
                    Value =
                        Entidad.PorcentajeComisionProducto.ToString(
                            "0.00",
                            CultureInfo.InvariantCulture)
                },

                new OdbcParameter(
                    "stockMinimoProducto", OdbcType.Int)
                {
                    Value = Entidad.StockMinimoProducto
                },

                new OdbcParameter(
                    "stockMaximoProducto", OdbcType.Int)
                {
                    Value = Entidad.StockMaximoProducto
                },

                new OdbcParameter(
                    "pesoProducto", OdbcType.VarChar, 32)
                {
                    Value = Entidad.PesoProducto.ToString(
                        "0.00",
                        CultureInfo.InvariantCulture)
                }
            };
        }

        // ========================================================
        // EJECUTAR INSERT, UPDATE O DELETE
        // No modifica ClsRepositorioMaestro.
        // ========================================================

        private int LogisticaMetEjecutarOperacion(
            string ComandoTexto,
            List<OdbcParameter> Parametros)
        {
            using (OdbcConnection Conexion =
                LogisticaMetObtenerConexion())
            {
                Conexion.Open();

                using (OdbcCommand Comando = new OdbcCommand())
                {
                    Comando.Connection = Conexion;
                    Comando.CommandText = ComandoTexto;
                    Comando.CommandType = CommandType.Text;

                    Comando.Parameters.AddRange(
                        Parametros.ToArray());

                    try
                    {
                        return Comando.ExecuteNonQuery();
                    }
                    catch (OdbcException Ex)
                    {
                        StringBuilder Mensaje =
                            new StringBuilder();

                        Mensaje.AppendLine(
                            "Error ODBC al ejecutar la operación.");
                        Mensaje.AppendLine();

                        foreach (OdbcError Error in Ex.Errors)
                        {
                            Mensaje.AppendLine(
                                "SQLSTATE: " + Error.SQLState);

                            Mensaje.AppendLine(
                                "Código: " + Error.NativeError);

                            Mensaje.AppendLine(
                                "Detalle: " + Error.Message);
                        }

                        if (Ex.Errors.Count == 0)
                        {
                            Mensaje.AppendLine(
                                "El controlador no devolvió SQLSTATE.");
                        }

                        Mensaje.AppendLine();
                        Mensaje.AppendLine("Parámetros enviados:");

                        for (int I = 0;
                             I < Comando.Parameters.Count;
                             I++)
                        {
                            OdbcParameter Parametro =
                                Comando.Parameters[I];

                            Mensaje.AppendLine(
                                (I + 1) + ". " +
                                Parametro.ParameterName +
                                " | " + Parametro.OdbcType +
                                " | Tamaño: " + Parametro.Size);
                        }

                        throw new InvalidOperationException(
                            Mensaje.ToString(),
                            Ex);
                    }
                }
            }
        }
    }
}
