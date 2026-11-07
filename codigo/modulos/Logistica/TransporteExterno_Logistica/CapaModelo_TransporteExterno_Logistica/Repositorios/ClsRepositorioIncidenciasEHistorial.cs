
using System;
using System.Data;
using System.Data.Odbc;
using CapaModelo_TransporteExterno_Logistica.Repositorio;

namespace CapaModelo_TransporteExterno_Logistica
{
    public class ClsRepositorioIncidenciasEHistorial : ClsRepositorio
    {
        // ==========================================
        // OBTENER GUIAS
        // ==========================================
        public DataTable LogisticaFuncObtenerGuias()
        {
            DataTable tabla = new DataTable();

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        idServicioTransporteExterno AS idServicio,
                        numeroGuiaServicioTransporteExterno AS numeroGuia
                    FROM tblServicioTransporteExterno
                    WHERE numeroGuiaServicioTransporteExterno IS NOT NULL
                    ORDER BY numeroGuiaServicioTransporteExterno";

                using (OdbcDataAdapter adaptador =
                    new OdbcDataAdapter(consulta, conexion))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        // ==========================================
        // OBTENER INCIDENCIAS
        // ==========================================
        public DataTable LogisticaFuncObtenerIncidencias(int idServicio)
        {
            DataTable tabla = new DataTable();

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        idIncidenciaTransporte,
                        codigoIncidenciaTransporte,
                        fechaIncidenciaTransporte,
                        tipoIncidenciaTransporte,
                        descripcionIncidenciaTransporte,
                        responsableIncidenciaTransporte,
                        estadoIncidenciaTransporte
                    FROM tblIncidenciaTransporte
                    WHERE Fk_Servicio_Transporte_Externo_Codigo = ?
                    ORDER BY fechaIncidenciaTransporte DESC";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@servicio", idServicio);

                    using (OdbcDataAdapter adaptador =
                        new OdbcDataAdapter(comando))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }

        // ==========================================
        // GUARDAR INCIDENCIA
        // ==========================================
        public void LogisticaProcGuardarIncidencia(
            int idServicio,
            string codigo,
            DateTime fecha,
            string tipo,
            string descripcion,
            string responsable,
            string estado)
        {
            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                conexion.Open();

                string consulta = @"
                    INSERT INTO tblIncidenciaTransporte
                    (
                        Fk_Servicio_Transporte_Externo_Codigo,
                        codigoIncidenciaTransporte,
                        fechaIncidenciaTransporte,
                        tipoIncidenciaTransporte,
                        descripcionIncidenciaTransporte,
                        responsableIncidenciaTransporte,
                        estadoIncidenciaTransporte
                    )
                    VALUES (?, ?, ?, ?, ?, ?, ?)";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@servicio", idServicio);
                    comando.Parameters.AddWithValue("@codigo", codigo);
                    comando.Parameters.AddWithValue("@fecha", fecha);
                    comando.Parameters.AddWithValue("@tipo", tipo);
                    comando.Parameters.AddWithValue("@descripcion", descripcion);
                    comando.Parameters.AddWithValue("@responsable", responsable);
                    comando.Parameters.AddWithValue("@estado", estado);

                    comando.ExecuteNonQuery();
                }
            }
        }

        // ==========================================
        // MODIFICAR INCIDENCIA
        // ==========================================
        public void LogisticaProcModificarIncidencia(
            int idIncidencia,
            string codigo,
            DateTime fecha,
            string tipo,
            string descripcion,
            string responsable,
            string estado)
        {
            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                conexion.Open();

                string consulta = @"
                    UPDATE tblIncidenciaTransporte
                    SET
                        codigoIncidenciaTransporte = ?,
                        fechaIncidenciaTransporte = ?,
                        tipoIncidenciaTransporte = ?,
                        descripcionIncidenciaTransporte = ?,
                        responsableIncidenciaTransporte = ?,
                        estadoIncidenciaTransporte = ?
                    WHERE idIncidenciaTransporte = ?";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@codigo", codigo);
                    comando.Parameters.AddWithValue("@fecha", fecha);
                    comando.Parameters.AddWithValue("@tipo", tipo);
                    comando.Parameters.AddWithValue("@descripcion", descripcion);
                    comando.Parameters.AddWithValue("@responsable", responsable);
                    comando.Parameters.AddWithValue("@estado", estado);
                    comando.Parameters.AddWithValue("@id", idIncidencia);

                    comando.ExecuteNonQuery();
                }
            }
        }

        // ==========================================
        // ELIMINAR INCIDENCIA
        // ==========================================
        public void LogisticaProcEliminarIncidencia(int idIncidencia)
        {
            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                conexion.Open();

                string consulta = @"
                    DELETE FROM tblIncidenciaTransporte
                    WHERE idIncidenciaTransporte = ?";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", idIncidencia);
                    comando.ExecuteNonQuery();
                }
            }
        }

        // ==========================================
        // CONSULTAR HISTORIAL
        // ==========================================
        public DataTable LogisticaFuncObtenerHistorial(
            int idServicio,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            string estado)
        {
            DataTable tabla = new DataTable();

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        s.numeroGuiaServicioTransporteExterno AS numeroGuia,
                        h.fechaMovimientoHistorialTransporte AS fechaMovimiento,
                        h.tipoMovimientoHistorialTransporte AS tipoMovimiento,
                        h.descripcionMovimientoHistorialTransporte AS descripcion,
                        h.estadoTransporteHistorialTransporte AS estado
                    FROM tblHistorialTransporte h
                    INNER JOIN tblServicioTransporteExterno s
                        ON h.Fk_Servicio_Transporte_Externo_Codigo =
                           s.idServicioTransporteExterno
                    WHERE h.Fk_Servicio_Transporte_Externo_Codigo = ?";

                using (OdbcCommand comando = new OdbcCommand())
                {
                    comando.Connection = conexion;
                    comando.Parameters.AddWithValue("@servicio", idServicio);

                    if (fechaDesde.HasValue)
                    {
                        consulta +=
                            " AND h.fechaMovimientoHistorialTransporte >= ?";

                        comando.Parameters.AddWithValue(
                            "@desde", fechaDesde.Value.Date);
                    }

                    if (fechaHasta.HasValue)
                    {
                        consulta +=
                            " AND h.fechaMovimientoHistorialTransporte < ?";

                        comando.Parameters.AddWithValue(
                            "@hasta", fechaHasta.Value.Date.AddDays(1));
                    }

                    if (!string.IsNullOrWhiteSpace(estado) &&
                        estado != "Todos")
                    {
                        consulta +=
                            " AND h.estadoTransporteHistorialTransporte = ?";

                        comando.Parameters.AddWithValue("@estado", estado);
                    }

                    consulta +=
                        " ORDER BY h.fechaMovimientoHistorialTransporte DESC";

                    comando.CommandText = consulta;

                    using (OdbcDataAdapter adaptador =
                        new OdbcDataAdapter(comando))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }

        // ==========================================
        // REGISTRAR MOVIMIENTO EN HISTORIAL
        // ==========================================
        public void LogisticaProcRegistrarMovimiento(
            int idServicio,
            string tipoMovimiento,
            string descripcion,
            string estado)
        {
            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                conexion.Open();

                string consulta = @"
                    INSERT INTO tblHistorialTransporte
                    (
                        Fk_Servicio_Transporte_Externo_Codigo,
                        fechaMovimientoHistorialTransporte,
                        tipoMovimientoHistorialTransporte,
                        descripcionMovimientoHistorialTransporte,
                        estadoTransporteHistorialTransporte
                    )
                    VALUES (?, NOW(), ?, ?, ?)";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@servicio", idServicio);
                    comando.Parameters.AddWithValue("@tipo", tipoMovimiento);
                    comando.Parameters.AddWithValue("@descripcion", descripcion);
                    comando.Parameters.AddWithValue("@estado", estado);

                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
