// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioDetalleViaje.cs
// DESCRIPCIÓN: Obtiene los registros de tblDetalleViaje
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
    public class ClsRepositorioDetalleViaje : ClsRepositorioMaestro, IRepositorioDetalleViaje
    {
        // =====================================================
        // AGREGAR DETALLE DE VIAJE
        // =====================================================

        public int LogisticaMetAgregar(
            ClsDetalleViaje Entidad)
        {
            string Consulta =
                "INSERT INTO tblDetalleViaje (" +
                "Fk_Encabezado_Viaje_Codigo, " +
                "ordenDetalleViaje, " +
                "descripcionDetalleViaje, " +
                "cantidadDetalleViaje, " +
                "pesoDetalleViaje) " +
                "VALUES (?, ?, ?, ?, ?)";

            return LogisticaMetEjecutarOperacion(
                Consulta,
                LogisticaFuncParametros(Entidad));
        }

        // =====================================================
        // MODIFICAR DETALLE DE VIAJE
        // =====================================================

        public int LogisticaMetEditar(
            ClsDetalleViaje Entidad)
        {
            string Consulta =
                "UPDATE tblDetalleViaje SET " +
                "Fk_Encabezado_Viaje_Codigo = ?, " +
                "ordenDetalleViaje = ?, " +
                "descripcionDetalleViaje = ?, " +
                "cantidadDetalleViaje = ?, " +
                "pesoDetalleViaje = ? " +
                "WHERE idDetalleViaje = ?";

            var Parametros =
                LogisticaFuncParametros(Entidad);

            Parametros.Add(
                new OdbcParameter(
                    "idDetalleViaje",
                    OdbcType.BigInt)
                {
                    Value = Entidad.IdDetalleViaje
                });

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // =====================================================
        // ELIMINAR DETALLE DE VIAJE
        // =====================================================

        public int LogisticaMetEliminar(long IdDetalleViaje)
        {
            string Consulta =
                "DELETE FROM tblDetalleViaje " +
                "WHERE idDetalleViaje = ?";

            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter(
                    "idDetalleViaje",
                    OdbcType.BigInt)
                {
                    Value = IdDetalleViaje
                }
            };

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // =====================================================
        // CONSULTAR TODOS LOS DETALLES
        // =====================================================

        public IEnumerable<ClsDetalleViaje>
            LogisticaMetObtenerTodos()
        {
            string Consulta =
                "SELECT * FROM tblDetalleViaje " +
                "ORDER BY idDetalleViaje DESC";

            DataTable Tabla =
                LogisticaMetEjecucionConsulta(Consulta);

            var Lista = new List<ClsDetalleViaje>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Lista.Add(new ClsDetalleViaje
                {
                    IdDetalleViaje =
                        Convert.ToInt64(
                            Fila["idDetalleViaje"]),

                    Fk_Encabezado_Viaje_Codigo =
                        Convert.ToInt64(
                            Fila["Fk_Encabezado_Viaje_Codigo"]),

                    OrdenDetalleViaje =
                        Convert.ToInt32(
                            Fila["ordenDetalleViaje"]),

                    DescripcionDetalleViaje =
                        Convert.ToString(
                            Fila["descripcionDetalleViaje"]),

                    CantidadDetalleViaje =
                        Convert.ToDecimal(
                            Fila["cantidadDetalleViaje"]),

                    PesoDetalleViaje =
                        Convert.ToDecimal(
                            Fila["pesoDetalleViaje"]),

                    CreatedAt =
                        Convert.ToDateTime(
                            Fila["created_at"]),

                    UpdatedAt =
                        Convert.ToDateTime(
                            Fila["updated_at"])
                });
            }

            return Lista;
        }

        // =====================================================
        // CONSULTAR VIAJES PARA EL COMBOBOX
        // =====================================================

        public IEnumerable<ClsCatalogoTransporteInterno>
            LogisticaMetObtenerViajes()
        {
            string Consulta =
                "SELECT idEncabezadoViaje AS Id, " +
                "CONCAT('Viaje #', idEncabezadoViaje) " +
                "AS Nombre " +
                "FROM tblEncabezadoViaje " +
                "ORDER BY idEncabezadoViaje DESC";

            DataTable Tabla =
                LogisticaMetEjecucionConsulta(Consulta);

            var Lista =
                new List<ClsCatalogoTransporteInterno>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Lista.Add(
                    new ClsCatalogoTransporteInterno
                    {
                        Id = Convert.ToInt32(Fila["Id"]),
                        Nombre = Convert.ToString(Fila["Nombre"])
                    });
            }

            return Lista;
        }

        // =====================================================
        // PARÁMETROS PARA INSERT Y UPDATE
        // =====================================================

        private List<OdbcParameter> LogisticaFuncParametros(
            ClsDetalleViaje Entidad)
        {
            return new List<OdbcParameter>
            {
                new OdbcParameter(
                    "Fk_Encabezado_Viaje_Codigo",
                    OdbcType.BigInt)
                {
                    Value = Entidad.Fk_Encabezado_Viaje_Codigo
                },

                new OdbcParameter(
                    "ordenDetalleViaje",
                    OdbcType.Int)
                {
                    Value = Entidad.OrdenDetalleViaje
                },

                new OdbcParameter(
                    "descripcionDetalleViaje",
                    OdbcType.VarChar)
                {
                    Value = Entidad.DescripcionDetalleViaje
                },

                new OdbcParameter(
                    "cantidadDetalleViaje",
                    OdbcType.Decimal)
                {
                    Precision = 18,
                    Scale = 3,
                    Value = Entidad.CantidadDetalleViaje
                },

                new OdbcParameter(
                    "pesoDetalleViaje",
                    OdbcType.Decimal)
                {
                    Precision = 18,
                    Scale = 3,
                    Value = Entidad.PesoDetalleViaje
                }
            };
        }

        // =====================================================
        // EJECUTAR INSERT, UPDATE Y DELETE
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
