
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsRepositorioStock.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA: 07/11/2026
 *
 * DESCRIPCIÓN:
 * Ejecuta operaciones CRUD sobre tblStock mediante ODBC.
 * Consulta los catálogos y sincroniza tblSaldos dentro de
 * la misma transacción para preservar la integridad.
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
    public class ClsRepositorioStock :
        ClsRepositorioMaestro,
        ClsIRepositorioStock
    {
        // ========================================================
        // SQL PARA INSERTAR
        // ========================================================

        private const string LogisticaSqlInsertar = @"
            INSERT INTO tblStock
            (
                Fk_Sucursal_Codigo,
                Fk_Bodega_Codigo,
                Fk_Producto_Codigo,
                stockFisicoStock,
                stockVirtualStock,
                stockAnteriorStock,
                stockActualStock,
                cargosMesStock,
                abonosMesStock,
                cargosAcumuladosStock,
                abonosAcumuladosStock,
                apartadosStock,
                estadoVentaStock,
                estadoCompraStock
            )
            VALUES
            (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

        // ========================================================
        // SQL PARA MODIFICAR
        // ========================================================

        private const string LogisticaSqlModificar = @"
            UPDATE tblStock SET
                Fk_Sucursal_Codigo = ?,
                Fk_Bodega_Codigo = ?,
                Fk_Producto_Codigo = ?,
                stockFisicoStock = ?,
                stockVirtualStock = ?,
                stockAnteriorStock = ?,
                stockActualStock = ?,
                cargosMesStock = ?,
                abonosMesStock = ?,
                cargosAcumuladosStock = ?,
                abonosAcumuladosStock = ?,
                apartadosStock = ?,
                estadoVentaStock = ?,
                estadoCompraStock = ?
            WHERE idStock = ?";

        // ========================================================
        // INGRESAR
        // ========================================================

        public int LogisticaMetAgregar(ClsStock Entidad)
        {
            using (OdbcConnection Conexion =
                LogisticaMetObtenerConexion())
            {
                Conexion.Open();

                using (OdbcTransaction Transaccion =
                    Conexion.BeginTransaction())
                {
                    try
                    {
                        int Filas = LogisticaMetEjecutar(
                            Conexion,
                            Transaccion,
                            LogisticaSqlInsertar,
                            LogisticaFuncParametros(Entidad));

                        if (Filas > 0)
                        {
                            LogisticaMetActualizarSaldo(
                                Conexion,
                                Transaccion,
                                Entidad);
                        }

                        Transaccion.Commit();

                        return Filas;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        // ========================================================
        // MODIFICAR
        // ========================================================

        public int LogisticaMetEditar(ClsStock Entidad)
        {
            using (OdbcConnection Conexion =
                LogisticaMetObtenerConexion())
            {
                Conexion.Open();

                using (OdbcTransaction Transaccion =
                    Conexion.BeginTransaction())
                {
                    try
                    {
                        int BodegaAnterior = 0;
                        int ProductoAnterior = 0;

                        // Consultar la relación anterior.
                        using (OdbcCommand Consulta =
                            new OdbcCommand(
                                "SELECT Fk_Sucursal_Codigo, " +
                                "Fk_Bodega_Codigo, " +
                                "Fk_Producto_Codigo " +
                                "FROM tblStock WHERE idStock = ?",
                                Conexion,
                                Transaccion))
                        {
                            Consulta.Parameters.Add(
                                "idStock", OdbcType.Int)
                                .Value = Entidad.IdStock;

                            using (OdbcDataReader Lector =
                                Consulta.ExecuteReader())
                            {
                                if (!Lector.Read())
                                {
                                    return 0;
                                }

                                BodegaAnterior =
                                    Convert.ToInt32(Lector[1]);

                                ProductoAnterior =
                                    Convert.ToInt32(Lector[2]);
                            }
                        }

                        List<OdbcParameter> Parametros =
                            LogisticaFuncParametros(Entidad);

                        Parametros.Add(
                            new OdbcParameter(
                                "idStock", OdbcType.Int)
                            {
                                Value = Entidad.IdStock
                            });

                        int Filas = LogisticaMetEjecutar(
                            Conexion,
                            Transaccion,
                            LogisticaSqlModificar,
                            Parametros);

                        if (Filas > 0)
                        {
                            // Eliminar saldo anterior cuando
                            // cambie el producto o la bodega.
                            if (BodegaAnterior !=
                                    Entidad.FkBodegaCodigo ||
                                ProductoAnterior !=
                                    Entidad.FkProductoCodigo)
                            {
                                LogisticaMetEliminarSaldo(
                                    Conexion,
                                    Transaccion,
                                    ProductoAnterior,
                                    BodegaAnterior);
                            }

                            LogisticaMetActualizarSaldo(
                                Conexion,
                                Transaccion,
                                Entidad);
                        }

                        Transaccion.Commit();

                        return Filas;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        // ========================================================
        // ELIMINAR
        // ========================================================

        public int LogisticaMetEliminar(int IdStock)
        {
            using (OdbcConnection Conexion =
                LogisticaMetObtenerConexion())
            {
                Conexion.Open();

                using (OdbcTransaction Transaccion =
                    Conexion.BeginTransaction())
                {
                    try
                    {
                        int Bodega = 0;
                        int Producto = 0;

                        // Consultar relación del stock.
                        using (OdbcCommand Consulta =
                            new OdbcCommand(
                                "SELECT Fk_Bodega_Codigo, " +
                                "Fk_Producto_Codigo " +
                                "FROM tblStock WHERE idStock = ?",
                                Conexion,
                                Transaccion))
                        {
                            Consulta.Parameters.Add(
                                "idStock", OdbcType.Int)
                                .Value = IdStock;

                            using (OdbcDataReader Lector =
                                Consulta.ExecuteReader())
                            {
                                if (!Lector.Read())
                                {
                                    return 0;
                                }

                                Bodega =
                                    Convert.ToInt32(Lector[0]);

                                Producto =
                                    Convert.ToInt32(Lector[1]);
                            }
                        }

                        int Filas = LogisticaMetEjecutar(
                            Conexion,
                            Transaccion,
                            "DELETE FROM tblStock " +
                            "WHERE idStock = ?",
                            new List<OdbcParameter>
                            {
                                new OdbcParameter(
                                    "idStock", OdbcType.Int)
                                {
                                    Value = IdStock
                                }
                            });

                        if (Filas > 0)
                        {
                            LogisticaMetEliminarSaldo(
                                Conexion,
                                Transaccion,
                                Producto,
                                Bodega);
                        }

                        Transaccion.Commit();

                        return Filas;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        // ========================================================
        // SINCRONIZAR tblSaldos
        // ========================================================

        private void LogisticaMetActualizarSaldo(
            OdbcConnection Conexion,
            OdbcTransaction Transaccion,
            ClsStock Entidad)
        {
            string Sql = @"
                INSERT INTO tblSaldos
                (
                    Fk_Producto_Codigo,
                    Fk_Bodega_Codigo,
                    saldoActualSaldo
                )
                VALUES (?, ?, ?)
                ON DUPLICATE KEY UPDATE
                    saldoActualSaldo =
                    VALUES(saldoActualSaldo)";

            LogisticaMetEjecutar(
                Conexion,
                Transaccion,
                Sql,
                new List<OdbcParameter>
                {
                    new OdbcParameter(
                        "producto", OdbcType.Int)
                    {
                        Value = Entidad.FkProductoCodigo
                    },

                    new OdbcParameter(
                        "bodega", OdbcType.Int)
                    {
                        Value = Entidad.FkBodegaCodigo
                    },

                    new OdbcParameter(
                        "saldo", OdbcType.Int)
                    {
                        Value = Entidad.StockActualStock
                    }
                });
        }

        // ========================================================
        // ELIMINAR SALDO ASOCIADO
        // ========================================================

        private void LogisticaMetEliminarSaldo(
            OdbcConnection Conexion,
            OdbcTransaction Transaccion,
            int IdProducto,
            int IdBodega)
        {
            LogisticaMetEjecutar(
                Conexion,
                Transaccion,
                "DELETE FROM tblSaldos " +
                "WHERE Fk_Producto_Codigo = ? " +
                "AND Fk_Bodega_Codigo = ?",
                new List<OdbcParameter>
                {
                    new OdbcParameter(
                        "producto", OdbcType.Int)
                    {
                        Value = IdProducto
                    },

                    new OdbcParameter(
                        "bodega", OdbcType.Int)
                    {
                        Value = IdBodega
                    }
                });
        }

        // ========================================================
        // EJECUTAR OPERACIÓN ODBC
        // ========================================================

        private int LogisticaMetEjecutar(
            OdbcConnection Conexion,
            OdbcTransaction Transaccion,
            string Sql,
            List<OdbcParameter> Parametros)
        {
            using (OdbcCommand Comando =
                new OdbcCommand(
                    Sql,
                    Conexion,
                    Transaccion))
            {
                Comando.CommandType = CommandType.Text;

                Comando.Parameters.AddRange(
                    Parametros.ToArray());

                return Comando.ExecuteNonQuery();
            }
        }

        // ========================================================
        // PARÁMETROS DE STOCK
        // ========================================================

        private List<OdbcParameter> LogisticaFuncParametros(
            ClsStock D)
        {
            return new List<OdbcParameter>
            {
                new OdbcParameter("sucursal", OdbcType.Int)
                {
                    Value = D.FkSucursalCodigo
                },

                new OdbcParameter("bodega", OdbcType.Int)
                {
                    Value = D.FkBodegaCodigo
                },

                new OdbcParameter("producto", OdbcType.Int)
                {
                    Value = D.FkProductoCodigo
                },

                new OdbcParameter("fisico", OdbcType.Int)
                {
                    Value = D.StockFisicoStock
                },

                new OdbcParameter("virtual", OdbcType.Int)
                {
                    Value = D.StockVirtualStock
                },

                new OdbcParameter("anterior", OdbcType.Int)
                {
                    Value = D.StockAnteriorStock
                },

                new OdbcParameter("actual", OdbcType.Int)
                {
                    Value = D.StockActualStock
                },

                new OdbcParameter("cargosMes", OdbcType.Int)
                {
                    Value = D.CargosMesStock
                },

                new OdbcParameter("abonosMes", OdbcType.Int)
                {
                    Value = D.AbonosMesStock
                },

                new OdbcParameter("cargosAcum", OdbcType.BigInt)
                {
                    Value = D.CargosAcumuladosStock
                },

                new OdbcParameter("abonosAcum", OdbcType.BigInt)
                {
                    Value = D.AbonosAcumuladosStock
                },

                new OdbcParameter("apartados", OdbcType.Int)
                {
                    Value = D.ApartadosStock
                },

                new OdbcParameter("estadoVenta", OdbcType.Int)
                {
                    Value = D.EstadoVentaStock ? 1 : 0
                },

                new OdbcParameter("estadoCompra", OdbcType.Int)
                {
                    Value = D.EstadoCompraStock ? 1 : 0
                }
            };
        }

        // ========================================================
        // CONSULTAR STOCK
        // ========================================================

        public IEnumerable<ClsStock> LogisticaMetObtenerTodos()
        {
            string Sql = @"
                SELECT
                    s.idStock,
                    s.Fk_Sucursal_Codigo,
                    s.Fk_Bodega_Codigo,
                    s.Fk_Producto_Codigo,
                    s.stockFisicoStock,
                    s.stockVirtualStock,
                    s.stockAnteriorStock,
                    s.stockActualStock,
                    s.cargosMesStock,
                    s.abonosMesStock,
                    s.cargosAcumuladosStock,
                    s.abonosAcumuladosStock,
                    s.apartadosStock,
                    s.estadoVentaStock,
                    s.estadoCompraStock,
                    u.nombreSucursal AS NombreSucursal,
                    b.nombreBodega AS NombreBodega,
                    p.nombreProducto AS NombreProducto
                FROM tblStock s
                INNER JOIN tblSucursal u
                    ON u.idSucursal = s.Fk_Sucursal_Codigo
                INNER JOIN tblBodega b
                    ON b.idBodega = s.Fk_Bodega_Codigo
                    AND b.Fk_Sucursal_Codigo =
                        s.Fk_Sucursal_Codigo
                INNER JOIN tblProducto p
                    ON p.idProducto = s.Fk_Producto_Codigo
                ORDER BY s.idStock DESC";

            DataTable Tabla =
                LogisticaMetEjecucionConsulta(
                    Sql,
                    CommandType.Text);

            List<ClsStock> Lista =
                new List<ClsStock>();

            foreach (DataRow F in Tabla.Rows)
            {
                Lista.Add(new ClsStock
                {
                    IdStock =
                        Convert.ToInt32(F["idStock"]),

                    FkSucursalCodigo =
                        Convert.ToInt32(
                            F["Fk_Sucursal_Codigo"]),

                    FkBodegaCodigo =
                        Convert.ToInt32(
                            F["Fk_Bodega_Codigo"]),

                    FkProductoCodigo =
                        Convert.ToInt32(
                            F["Fk_Producto_Codigo"]),

                    StockFisicoStock =
                        Convert.ToInt32(
                            F["stockFisicoStock"]),

                    StockVirtualStock =
                        Convert.ToInt32(
                            F["stockVirtualStock"]),

                    StockAnteriorStock =
                        Convert.ToInt32(
                            F["stockAnteriorStock"]),

                    StockActualStock =
                        Convert.ToInt32(
                            F["stockActualStock"]),

                    CargosMesStock =
                        Convert.ToInt32(
                            F["cargosMesStock"]),

                    AbonosMesStock =
                        Convert.ToInt32(
                            F["abonosMesStock"]),

                    CargosAcumuladosStock =
                        Convert.ToInt64(
                            F["cargosAcumuladosStock"]),

                    AbonosAcumuladosStock =
                        Convert.ToInt64(
                            F["abonosAcumuladosStock"]),

                    ApartadosStock =
                        Convert.ToInt32(
                            F["apartadosStock"]),

                    EstadoVentaStock =
                        Convert.ToBoolean(
                            F["estadoVentaStock"]),

                    EstadoCompraStock =
                        Convert.ToBoolean(
                            F["estadoCompraStock"]),

                    NombreSucursal =
                        Convert.ToString(F["NombreSucursal"]),

                    NombreBodega =
                        Convert.ToString(F["NombreBodega"]),

                    NombreProducto =
                        Convert.ToString(F["NombreProducto"])
                });
            }

            return Lista;
        }

        // ========================================================
        // OBTENER SUCURSALES
        // ========================================================

        public IEnumerable<ClsCatalogo>
            LogisticaMetObtenerSucursales()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idSucursal AS Id, " +
                "nombreSucursal AS Nombre " +
                "FROM tblSucursal " +
                "ORDER BY nombreSucursal",
                null);
        }

        // ========================================================
        // OBTENER BODEGAS DE UNA SUCURSAL
        // ========================================================

        public IEnumerable<ClsCatalogo>
            LogisticaMetObtenerBodegas(int IdSucursal)
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idBodega AS Id, " +
                "nombreBodega AS Nombre " +
                "FROM tblBodega " +
                "WHERE Fk_Sucursal_Codigo = ? " +
                "ORDER BY nombreBodega",
                new List<OdbcParameter>
                {
                    new OdbcParameter(
                        "sucursal", OdbcType.Int)
                    {
                        Value = IdSucursal
                    }
                });
        }

        // ========================================================
        // OBTENER PRODUCTOS
        // ========================================================

        public IEnumerable<ClsCatalogo>
            LogisticaMetObtenerProductos()
        {
            return LogisticaFuncConsultarCatalogo(
                "SELECT idProducto AS Id, " +
                "CONCAT(skuProducto, ' - ', nombreProducto) " +
                "AS Nombre " +
                "FROM tblProducto " +
                "ORDER BY nombreProducto",
                null);
        }

        // ========================================================
        // CONSULTA GENERAL DE CATÁLOGOS
        // ========================================================

        private IEnumerable<ClsCatalogo>
            LogisticaFuncConsultarCatalogo(
                string Sql,
                List<OdbcParameter> Parametros)
        {
            DataTable Tabla;

            if (Parametros == null)
            {
                Tabla = LogisticaMetEjecucionConsulta(
                    Sql,
                    CommandType.Text);
            }
            else
            {
                Tabla = LogisticaMetEjecucionConsulta(
                    Sql,
                    CommandType.Text,
                    Parametros);
            }

            List<ClsCatalogo> Lista =
                new List<ClsCatalogo>();

            foreach (DataRow F in Tabla.Rows)
            {
                Lista.Add(new ClsCatalogo
                {
                    Id = Convert.ToInt32(F["Id"]),

                    Nombre =
                        Convert.ToString(F["Nombre"])
                });
            }

            return Lista;
        }
    }
}
