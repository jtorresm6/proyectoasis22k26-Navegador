// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioTelemetriaTemperatura.cs
// DESCRIPCIÓN: Obtiene los registros de tblTelemetriaTemperatura
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
    public class ClsRepositorioTelemetriaTemperatura : ClsRepositorioMaestro, IRepositorioTelemetriaTemperatura
    {
        // =====================================================
        // AGREGAR REGISTRO
        // =====================================================

        public int LogisticaMetAgregar(
            ClsTelemetriaTemperatura Entidad)
        {
            string Consulta =
                "INSERT INTO tblTelemetriaTemperatura (" +
                "Fk_Encabezado_Viaje_Codigo, " +
                "fechaHoraTelemetriaTemperatura, " +
                "temperaturaCelsiusTelemetriaTemperatura, " +
                "humedadRelativaTelemetriaTemperatura, " +
                "puertaAbiertaTelemetriaTemperatura, " +
                "ubicacionGpsTelemetriaTemperatura) " +
                "VALUES (?, ?, ?, ?, ?, ?)";

            return LogisticaMetEjecutarOperacion(
                Consulta,
                LogisticaFuncParametros(Entidad));
        }

        // =====================================================
        // MODIFICAR REGISTRO
        // =====================================================

        public int LogisticaMetEditar(
            ClsTelemetriaTemperatura Entidad)
        {
            string Consulta =
                "UPDATE tblTelemetriaTemperatura SET " +
                "Fk_Encabezado_Viaje_Codigo = ?, " +
                "fechaHoraTelemetriaTemperatura = ?, " +
                "temperaturaCelsiusTelemetriaTemperatura = ?, " +
                "humedadRelativaTelemetriaTemperatura = ?, " +
                "puertaAbiertaTelemetriaTemperatura = ?, " +
                "ubicacionGpsTelemetriaTemperatura = ? " +
                "WHERE idTelemetriaTemperatura = ?";

            var Parametros =
                LogisticaFuncParametros(Entidad);

            Parametros.Add(
                new OdbcParameter(
                    "idTelemetriaTemperatura",
                    OdbcType.BigInt)
                {
                    Value = Entidad.IdTelemetriaTemperatura
                });

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // =====================================================
        // ELIMINAR REGISTRO
        // =====================================================

        public int LogisticaMetEliminar(
            long IdTelemetriaTemperatura)
        {
            string Consulta =
                "DELETE FROM tblTelemetriaTemperatura " +
                "WHERE idTelemetriaTemperatura = ?";

            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter(
                    "idTelemetriaTemperatura",
                    OdbcType.BigInt)
                {
                    Value = IdTelemetriaTemperatura
                }
            };

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // =====================================================
        // CONSULTAR TODOS LOS REGISTROS
        // =====================================================

        public IEnumerable<ClsTelemetriaTemperatura>
            LogisticaMetObtenerTodos()
        {
            string Consulta =
                "SELECT * FROM tblTelemetriaTemperatura " +
                "ORDER BY idTelemetriaTemperatura DESC";

            DataTable Tabla =
                LogisticaMetEjecucionConsulta(Consulta);

            var Lista =
                new List<ClsTelemetriaTemperatura>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Lista.Add(new ClsTelemetriaTemperatura
                {
                    IdTelemetriaTemperatura =
                        Convert.ToInt64(
                            Fila["idTelemetriaTemperatura"]),

                    Fk_Encabezado_Viaje_Codigo =
                        Convert.ToInt64(
                            Fila["Fk_Encabezado_Viaje_Codigo"]),

                    FechaHoraTelemetriaTemperatura =
                        Convert.ToDateTime(
                            Fila["fechaHoraTelemetriaTemperatura"]),

                    TemperaturaCelsiusTelemetriaTemperatura =
                        Convert.ToDecimal(
                            Fila["temperaturaCelsiusTelemetriaTemperatura"]),

                    HumedadRelativaTelemetriaTemperatura =
                        Convert.ToDecimal(
                            Fila["humedadRelativaTelemetriaTemperatura"]),

                    PuertaAbiertaTelemetriaTemperatura =
                        Convert.ToBoolean(
                            Fila["puertaAbiertaTelemetriaTemperatura"]),

                    UbicacionGpsTelemetriaTemperatura =
                        Convert.ToString(
                            Fila["ubicacionGpsTelemetriaTemperatura"]),

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

        public IEnumerable<ClsCatalogoDetalleViajeLote>
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
            ClsTelemetriaTemperatura Entidad)
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
                    "fechaHoraTelemetriaTemperatura",
                    OdbcType.DateTime)
                {
                    Value =
                        Entidad.FechaHoraTelemetriaTemperatura
                },

                new OdbcParameter(
                    "temperaturaCelsiusTelemetriaTemperatura",
                    OdbcType.Decimal)
                {
                    Precision = 5,
                    Scale = 2,
                    Value =
                        Entidad.TemperaturaCelsiusTelemetriaTemperatura
                },

                new OdbcParameter(
                    "humedadRelativaTelemetriaTemperatura",
                    OdbcType.Decimal)
                {
                    Precision = 5,
                    Scale = 4,
                    Value =
                        Entidad.HumedadRelativaTelemetriaTemperatura
                },

                new OdbcParameter(
                    "puertaAbiertaTelemetriaTemperatura",
                    OdbcType.Bit)
                {
                    Value =
                        Entidad.PuertaAbiertaTelemetriaTemperatura
                },

                new OdbcParameter(
                    "ubicacionGpsTelemetriaTemperatura",
                    OdbcType.VarChar)
                {
                    Value =
                        Entidad.UbicacionGpsTelemetriaTemperatura
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
