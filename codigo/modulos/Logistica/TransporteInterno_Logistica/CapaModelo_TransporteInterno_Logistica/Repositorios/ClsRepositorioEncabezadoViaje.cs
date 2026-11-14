// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioEncabezadoViaje.cs
// DESCRIPCIÓN: Obtiene los registros de tblEncabezadoViaje
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
    public class ClsRepositorioEncabezadoViaje :ClsRepositorioMaestro,IRepositorioEncabezadoViaje
    {
        // INSERT: registra un nuevo viaje.
        public int LogisticaMetAgregar(
            ClsEncabezadoViaje Entidad)
        {
            string Consulta =
                "INSERT INTO tblEncabezadoViaje (" +
                "fechaDocumentoEncabezadoViaje, " +
                "horaSalidaEncabezadoViaje, " +
                "horaLlegadaEncabezadoViaje, " +
                "requiereCadenaFrioEncabezadoViaje, " +
                "estadoViajeEncabezadoViaje, " +
                "pesoTotalEncabezadoViaje, " +
                "Fk_Transporte_Codigo, " +
                "Fk_Conductor_Codigo) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

            return LogisticaMetEjecutarOperacion(
                Consulta,
                LogisticaFuncParametros(Entidad));
        }

        // UPDATE: modifica un viaje existente.
        public int LogisticaMetEditar(
            ClsEncabezadoViaje Entidad)
        {
            string Consulta =
                "UPDATE tblEncabezadoViaje SET " +
                "fechaDocumentoEncabezadoViaje = ?, " +
                "horaSalidaEncabezadoViaje = ?, " +
                "horaLlegadaEncabezadoViaje = ?, " +
                "requiereCadenaFrioEncabezadoViaje = ?, " +
                "estadoViajeEncabezadoViaje = ?, " +
                "pesoTotalEncabezadoViaje = ?, " +
                "Fk_Transporte_Codigo = ?, " +
                "Fk_Conductor_Codigo = ? " +
                "WHERE idEncabezadoViaje = ?";

            var Parametros = LogisticaFuncParametros(Entidad);

            Parametros.Add(
                new OdbcParameter(
                    "idEncabezadoViaje",
                    OdbcType.BigInt)
                {
                    Value = Entidad.IdEncabezadoViaje
                });

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // DELETE: elimina un viaje por su ID.
        public int LogisticaMetEliminar(
            long IdEncabezadoViaje)
        {
            string Consulta =
                "DELETE FROM tblEncabezadoViaje " +
                "WHERE idEncabezadoViaje = ?";

            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter(
                    "idEncabezadoViaje",
                    OdbcType.BigInt)
                {
                    Value = IdEncabezadoViaje
                }
            };

            return LogisticaMetEjecutarOperacion(
                Consulta,
                Parametros);
        }

        // SELECT: consulta los viajes con sus datos relacionados.
        public IEnumerable<ClsEncabezadoViaje>
            LogisticaMetObtenerTodos()
        {
            string Consulta = @"
                SELECT
                    e.*,
                    t.nroPlacaTransporte AS PlacaTransporte,
                    c.nombreCompletoConductor AS NombreConductor
                FROM tblEncabezadoViaje e
                INNER JOIN tblTransporte t
                    ON e.Fk_Transporte_Codigo = t.idTransporte
                INNER JOIN tblConductor c
                    ON e.Fk_Conductor_Codigo = c.idConductor
                ORDER BY e.idEncabezadoViaje DESC";

            DataTable Tabla =
                LogisticaMetEjecucionConsulta(Consulta);

            var Lista = new List<ClsEncabezadoViaje>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                var Viaje = new ClsEncabezadoViaje
                {
                    IdEncabezadoViaje =
                        Convert.ToInt64(
                            Fila["idEncabezadoViaje"]),

                    FechaDocumentoEncabezadoViaje =
                        Convert.ToDateTime(
                            Fila["fechaDocumentoEncabezadoViaje"]),

                    HoraSalidaEncabezadoViaje =
                        Convert.ToDateTime(
                            Fila["horaSalidaEncabezadoViaje"]),

                    HoraLlegadaEncabezadoViaje =
                        Fila["horaLlegadaEncabezadoViaje"]
                            == DBNull.Value
                        ? (DateTime?)null
                        : Convert.ToDateTime(
                            Fila["horaLlegadaEncabezadoViaje"]),

                    RequiereCadenaFrioEncabezadoViaje =
                        Convert.ToBoolean(
                            Fila["requiereCadenaFrioEncabezadoViaje"]),

                    EstadoViajeEncabezadoViaje =
                        Convert.ToString(
                            Fila["estadoViajeEncabezadoViaje"]),

                    PesoTotalEncabezadoViaje =
                        Convert.ToDecimal(
                            Fila["pesoTotalEncabezadoViaje"]),

                    Fk_Transporte_Codigo =
                        Convert.ToInt32(
                            Fila["Fk_Transporte_Codigo"]),

                    Fk_Conductor_Codigo =
                        Convert.ToInt32(
                            Fila["Fk_Conductor_Codigo"]),

                    CreatedAt =
                        Convert.ToDateTime(
                            Fila["created_at"]),

                    UpdatedAt =
                        Convert.ToDateTime(
                            Fila["updated_at"]),

                    PlacaTransporte =
                        Convert.ToString(
                            Fila["PlacaTransporte"]),

                    NombreConductor =
                        Convert.ToString(
                            Fila["NombreConductor"])
                };

                Lista.Add(Viaje);
            }

            return Lista;
        }

        // Obtiene los vehículos disponibles para selección.
        public IEnumerable<ClsCatalogoTransporteInterno>
            LogisticaMetObtenerTransportes()
        {
            string Consulta =
                "SELECT idTransporte AS Id, " +
                "nroPlacaTransporte AS Nombre " +
                "FROM tblTransporte " +
                "ORDER BY nroPlacaTransporte";

            return LogisticaFuncConsultarCatalogo(Consulta);
        }

        // Obtiene los conductores para selección.
        public IEnumerable<ClsCatalogoTransporteInterno>
            LogisticaMetObtenerConductores()
        {
            string Consulta =
                "SELECT idConductor AS Id, " +
                "nombreCompletoConductor AS Nombre " +
                "FROM tblConductor " +
                "ORDER BY nombreCompletoConductor";

            return LogisticaFuncConsultarCatalogo(Consulta);
        }

        // Convierte los resultados en objetos de catálogo.
        private IEnumerable<ClsCatalogoTransporteInterno>
            LogisticaFuncConsultarCatalogo(string Consulta)
        {
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

        // Construye los parámetros para INSERT y UPDATE.
        // ODBC utiliza el orden de los signos ?.
        private List<OdbcParameter> LogisticaFuncParametros(
            ClsEncabezadoViaje Entidad)
        {
            return new List<OdbcParameter>
            {
                new OdbcParameter(
                    "fechaDocumentoEncabezadoViaje",
                    OdbcType.DateTime)
                {
                    Value = Entidad.FechaDocumentoEncabezadoViaje
                },

                new OdbcParameter(
                    "horaSalidaEncabezadoViaje",
                    OdbcType.DateTime)
                {
                    Value = Entidad.HoraSalidaEncabezadoViaje
                },

                new OdbcParameter(
                    "horaLlegadaEncabezadoViaje",
                    OdbcType.DateTime)
                {
                    Value = Entidad.HoraLlegadaEncabezadoViaje.HasValue
                        ? (object)Entidad.HoraLlegadaEncabezadoViaje.Value
                        : DBNull.Value
                },

                new OdbcParameter(
                    "requiereCadenaFrioEncabezadoViaje",
                    OdbcType.Bit)
                {
                    Value = Entidad.RequiereCadenaFrioEncabezadoViaje
                },

                new OdbcParameter(
                    "estadoViajeEncabezadoViaje",
                    OdbcType.VarChar)
                {
                    Value = Entidad.EstadoViajeEncabezadoViaje
                },

                new OdbcParameter(
                    "pesoTotalEncabezadoViaje",
                    OdbcType.Decimal)
                {
                    Precision = 18,
                    Scale = 3,
                    Value = Entidad.PesoTotalEncabezadoViaje
                },

                new OdbcParameter(
                    "Fk_Transporte_Codigo",
                    OdbcType.Int)
                {
                    Value = Entidad.Fk_Transporte_Codigo
                },

                new OdbcParameter(
                    "Fk_Conductor_Codigo",
                    OdbcType.Int)
                {
                    Value = Entidad.Fk_Conductor_Codigo
                }
            };
        }

        // Ejecuta INSERT, UPDATE y DELETE.
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

                    Comando.Parameters.AddRange(
                        Parametros.ToArray());

                    return Comando.ExecuteNonQuery();
                }
            }
        }
    }
}
