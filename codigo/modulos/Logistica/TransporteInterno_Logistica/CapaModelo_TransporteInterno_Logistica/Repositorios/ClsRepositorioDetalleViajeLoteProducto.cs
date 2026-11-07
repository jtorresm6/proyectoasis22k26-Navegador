// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioDetalleViajeLoteProducto.cs
// DESCRIPCIÓN: Obtiene los registros de tblDetalleViajeLoteProducto
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using CapaModelo_TransporteInterno_Logistica.Contratos;
using CapaModelo_TransporteInterno_Logistica.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public class ClsRepositorioDetalleViajeLoteProducto : ClsRepositorioMaestro, IRepositorioDetalleViajeLoteProducto
    {
        // =====================================================
        // AGREGAR ASIGNACIÓN
        // =====================================================

        public int LogisticaMetAgregar(
            ClsDetalleViajeLoteProducto Entidad)
        {
            string Consulta =
                "INSERT INTO tblDetalleViajeLoteProducto (" +
                "Fk_Detalle_Viaje_Codigo, " +
                "Fk_Lote_Producto_Codigo, " +
                "cantidadCajasDetalleViajeLoteProducto, " +
                "pesoKgDetalleViajeLoteProducto) " +
                "VALUES (?, ?, ?, ?)";

            return LogisticaMetEjecutarOperacion(
                Consulta,
                LogisticaFuncParametros(Entidad));
        }

        // =====================================================
        // MODIFICAR ASIGNACIÓN
        // =====================================================

        public int LogisticaMetEditar(
            ClsDetalleViajeLoteProducto Entidad)
        {
            string Consulta =
                "UPDATE tblDetalleViajeLoteProducto SET " +
                "Fk_Detalle_Viaje_Codigo = ?, " +
                "Fk_Lote_Producto_Codigo = ?, " +
                "cantidadCajasDetalleViajeLoteProducto = ?, " +
                "pesoKgDetalleViajeLoteProducto = ? " +
                "WHERE idDetalleViajeLoteProducto = ?";

            var Parametros =
                LogisticaFuncParametros(Entidad);

            Parametros.Add(
                new OdbcParameter(
                    "idDetalleViajeLoteProducto",
                    OdbcType.BigInt)
                {
                    Value = Entidad.IdDetalleViajeLoteProducto
                });

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // =====================================================
        // ELIMINAR ASIGNACIÓN
        // =====================================================

        public int LogisticaMetEliminar(
            long IdDetalleViajeLoteProducto)
        {
            string Consulta =
                "DELETE FROM tblDetalleViajeLoteProducto " +
                "WHERE idDetalleViajeLoteProducto = ?";

            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter(
                    "idDetalleViajeLoteProducto",
                    OdbcType.BigInt)
                {
                    Value = IdDetalleViajeLoteProducto
                }
            };

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // =====================================================
        // CONSULTAR ASIGNACIONES
        // =====================================================

        public IEnumerable<ClsDetalleViajeLoteProducto>
            LogisticaMetObtenerTodos()
        {
            string Consulta =
                "SELECT * FROM tblDetalleViajeLoteProducto " +
                "ORDER BY idDetalleViajeLoteProducto DESC";

            DataTable Tabla =
                LogisticaMetEjecucionConsulta(Consulta);

            var Lista =
                new List<ClsDetalleViajeLoteProducto>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Lista.Add(new ClsDetalleViajeLoteProducto
                {
                    IdDetalleViajeLoteProducto =
                        Convert.ToInt64(
                            Fila["idDetalleViajeLoteProducto"]),

                    Fk_Detalle_Viaje_Codigo =
                        Convert.ToInt64(
                            Fila["Fk_Detalle_Viaje_Codigo"]),

                    Fk_Lote_Producto_Codigo =
                        Convert.ToInt64(
                            Fila["Fk_Lote_Producto_Codigo"]),

                    CantidadCajasDetalleViajeLoteProducto =
                        Convert.ToInt32(
                            Fila["cantidadCajasDetalleViajeLoteProducto"]),

                    PesoKgDetalleViajeLoteProducto =
                        Convert.ToDecimal(
                            Fila["pesoKgDetalleViajeLoteProducto"]),

                    CreatedAt =
                        Convert.ToDateTime(Fila["created_at"]),

                    UpdatedAt =
                        Convert.ToDateTime(Fila["updated_at"])
                });
            }

            return Lista;
        }

        // =====================================================
        // CATÁLOGO DE DETALLES DE VIAJE
        // =====================================================

        public IEnumerable<ClsCatalogoDetalleViajeLote>
            LogisticaMetObtenerDetallesViaje()
        {
            string Consulta =
                "SELECT idDetalleViaje AS Id, " +
                "CONCAT('Detalle #', idDetalleViaje) AS Nombre " +
                "FROM tblDetalleViaje " +
                "ORDER BY idDetalleViaje DESC";

            return LogisticaFuncConsultarCatalogo(Consulta);
        }

        // =====================================================
        // CATÁLOGO DE LOTES DE PRODUCTO
        // =====================================================

        public IEnumerable<ClsCatalogoDetalleViajeLote>
            LogisticaMetObtenerLotesProducto()
        {
            string Consulta =
                "SELECT idLoteProducto AS Id, " +
                "CONCAT('Lote #', idLoteProducto) AS Nombre " +
                "FROM tblLoteProducto " +
                "ORDER BY idLoteProducto DESC";

            return LogisticaFuncConsultarCatalogo(Consulta);
        }

        // =====================================================
        // CONVERTIR CATÁLOGOS
        // =====================================================

        private IEnumerable<ClsCatalogoDetalleViajeLote>
            LogisticaFuncConsultarCatalogo(string Consulta)
        {
            DataTable Tabla =
                LogisticaMetEjecucionConsulta(Consulta);

            var Lista =
                new List<ClsCatalogoDetalleViajeLote>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Lista.Add(new ClsCatalogoDetalleViajeLote
                {
                    Id = Convert.ToInt64(Fila["Id"]),
                    Nombre = Convert.ToString(Fila["Nombre"])
                });
            }

            return Lista;
        }

        // =====================================================
        // PARÁMETROS DE INSERT Y UPDATE
        // =====================================================

        private List<OdbcParameter> LogisticaFuncParametros(
            ClsDetalleViajeLoteProducto Entidad)
        {
            return new List<OdbcParameter>
            {
                new OdbcParameter(
                    "Fk_Detalle_Viaje_Codigo",
                    OdbcType.BigInt)
                {
                    Value = Entidad.Fk_Detalle_Viaje_Codigo
                },

                new OdbcParameter(
                    "Fk_Lote_Producto_Codigo",
                    OdbcType.BigInt)
                {
                    Value = Entidad.Fk_Lote_Producto_Codigo
                },

                new OdbcParameter(
                    "cantidadCajasDetalleViajeLoteProducto",
                    OdbcType.Int)
                {
                    Value =
                        Entidad.CantidadCajasDetalleViajeLoteProducto
                },

                new OdbcParameter(
                    "pesoKgDetalleViajeLoteProducto",
                    OdbcType.Decimal)
                {
                    Precision = 18,
                    Scale = 3,
                    Value =
                        Entidad.PesoKgDetalleViajeLoteProducto
                }
            };
        }

        // =====================================================
        // EJECUTAR OPERACIONES SQL
        // =====================================================

        private int LogisticaMetEjecutarOperacion(
            string Consulta,
            List<OdbcParameter> Parametros)
        {
            using (var Conexion = LogisticaMetObtenerConexion())
            {
                Conexion.Open();

                using (var Comando = new OdbcCommand())
                {
                    Comando.Connection = Conexion;
                    Comando.CommandText = Consulta;
                    Comando.CommandType = CommandType.Text;

                    Comando.Parameters.AddRange(
                        Parametros.ToArray());

                    return Comando.ExecuteNonQuery();
                }
            }
        }
    }
}
