// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioAlertaCadenaFrio.cs
// DESCRIPCIÓN: Obtiene los registros de tblAlertaCadenaFrio
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
    public class ClsRepositorioAlertaCadenaFrio : ClsRepositorioMaestro, IRepositorioAlertaCadenaFrio
    {
        // =====================================================
        // AGREGAR ALERTA
        // =====================================================

        public int LogisticaMetAgregar(
            ClsAlertaCadenaFrio Entidad)
        {
            string Consulta =
                "INSERT INTO tblAlertaCadenaFrio (" +
                "Fk_Telemetria_Temperatura_Codigo, " +
                "tipoAlertaCadenaFrio, " +
                "atendidaAlertaCadenaFrio, " +
                "observacionCalidadAlertaCadenaFrio) " +
                "VALUES (?, ?, ?, ?)";

            return LogisticaMetEjecutarOperacion(
                Consulta,
                LogisticaFuncParametros(Entidad));
        }

        // =====================================================
        // MODIFICAR ALERTA
        // =====================================================

        public int LogisticaMetEditar(
            ClsAlertaCadenaFrio Entidad)
        {
            string Consulta =
                "UPDATE tblAlertaCadenaFrio SET " +
                "Fk_Telemetria_Temperatura_Codigo = ?, " +
                "tipoAlertaCadenaFrio = ?, " +
                "atendidaAlertaCadenaFrio = ?, " +
                "observacionCalidadAlertaCadenaFrio = ? " +
                "WHERE idAlertaCadenaFrio = ?";

            var Parametros =
                LogisticaFuncParametros(Entidad);

            Parametros.Add(
                new OdbcParameter(
                    "idAlertaCadenaFrio",
                    OdbcType.BigInt)
                {
                    Value = Entidad.IdAlertaCadenaFrio
                });

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // =====================================================
        // ELIMINAR ALERTA
        // =====================================================

        public int LogisticaMetEliminar(
            long IdAlertaCadenaFrio)
        {
            string Consulta =
                "DELETE FROM tblAlertaCadenaFrio " +
                "WHERE idAlertaCadenaFrio = ?";

            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter(
                    "idAlertaCadenaFrio",
                    OdbcType.BigInt)
                {
                    Value = IdAlertaCadenaFrio
                }
            };

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // =====================================================
        // CONSULTAR TODAS LAS ALERTAS
        // =====================================================

        public IEnumerable<ClsAlertaCadenaFrio>
            LogisticaMetObtenerTodos()
        {
            string Consulta =
                "SELECT * FROM tblAlertaCadenaFrio " +
                "ORDER BY idAlertaCadenaFrio DESC";

            DataTable Tabla =
                LogisticaMetEjecucionConsulta(Consulta);

            var Lista = new List<ClsAlertaCadenaFrio>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Lista.Add(new ClsAlertaCadenaFrio
                {
                    IdAlertaCadenaFrio =
                        Convert.ToInt64(
                            Fila["idAlertaCadenaFrio"]),

                    Fk_Telemetria_Temperatura_Codigo =
                        Convert.ToInt64(
                            Fila["Fk_Telemetria_Temperatura_Codigo"]),

                    TipoAlertaCadenaFrio =
                        Convert.ToString(
                            Fila["tipoAlertaCadenaFrio"]),

                    AtendidaAlertaCadenaFrio =
                        Convert.ToBoolean(
                            Fila["atendidaAlertaCadenaFrio"]),

                    ObservacionCalidadAlertaCadenaFrio =
                        Convert.ToString(
                            Fila["observacionCalidadAlertaCadenaFrio"]),

                    CreatedAt =
                        Convert.ToDateTime(Fila["created_at"]),

                    UpdatedAt =
                        Convert.ToDateTime(Fila["updated_at"])
                });
            }

            return Lista;
        }

        // =====================================================
        // OBTENER TELEMETRÍAS PARA EL COMBOBOX
        // =====================================================

        public IEnumerable<ClsCatalogoDetalleViajeLote>
            LogisticaMetObtenerTelemetrias()
        {
            string Consulta =
                "SELECT " +
                "idTelemetriaTemperatura AS Id, " +
                "CONCAT('Telemetría #', idTelemetriaTemperatura, " +
                "' - Viaje #', Fk_Encabezado_Viaje_Codigo) " +
                "AS Nombre " +
                "FROM tblTelemetriaTemperatura " +
                "ORDER BY idTelemetriaTemperatura DESC";

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
        // PARÁMETROS PARA INSERT Y UPDATE
        // =====================================================

        private List<OdbcParameter> LogisticaFuncParametros(
            ClsAlertaCadenaFrio Entidad)
        {
            return new List<OdbcParameter>
            {
                new OdbcParameter(
                    "Fk_Telemetria_Temperatura_Codigo",
                    OdbcType.BigInt)
                {
                    Value =
                        Entidad.Fk_Telemetria_Temperatura_Codigo
                },

                new OdbcParameter(
                    "tipoAlertaCadenaFrio",
                    OdbcType.VarChar)
                {
                    Size = 30,
                    Value = Entidad.TipoAlertaCadenaFrio
                },

                new OdbcParameter(
                    "atendidaAlertaCadenaFrio",
                    OdbcType.Bit)
                {
                    Value = Entidad.AtendidaAlertaCadenaFrio
                },

                new OdbcParameter(
                    "observacionCalidadAlertaCadenaFrio",
                    OdbcType.VarChar)
                {
                    Size = 500,
                    Value =
                        Entidad.ObservacionCalidadAlertaCadenaFrio
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
