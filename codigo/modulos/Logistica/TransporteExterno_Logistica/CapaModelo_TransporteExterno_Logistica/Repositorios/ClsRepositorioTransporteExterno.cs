
// ------------------------------------------------------
// Autor: Gabriela Pinto García
// Carné: 9959-23-1087
// Fecha: 21/10/2026
// Módulo: Logística - Transporte Externo
// ------------------------------------------------------

using System;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_TransporteExterno_Logistica.Repositorio
{
    public class ClsRepositorioTransporteExterno : ClsRepositorio
    {
        // Obtener todos los transportes.
        public DataTable LogisticaFuncObtenerTransportes()
        {
            return LogisticaFuncConsultar(
                "SELECT * FROM tblServicioTransporteExterno");
        }

        // Obtener empresas transportistas.
        public DataTable LogisticaFuncObtenerEmpresas()
        {
            return LogisticaFuncConsultar(
                @"SELECT
                    Pk_EmpresaTransportista AS idEmpresaTransportista,
                    nombreEmpresaTransportista,
                    contactoEmpresaTransportista
                  FROM tblEmpresaTransportista
                  WHERE is_active = 1
                  ORDER BY nombreEmpresaTransportista");
        }

        // Obtener tipos de servicio.
        public DataTable LogisticaFuncObtenerTiposServicio()
        {
            return LogisticaFuncConsultar(
                @"SELECT
                    idTipoServicio,
                    nombreTipoServicio
                  FROM tblTipoServicio
                  ORDER BY nombreTipoServicio");
        }

        // Obtener registros con sus datos relacionados.
        public DataTable LogisticaFuncObtenerRegistroTransportes()
        {
            return LogisticaFuncConsultar(
                @"SELECT
                    s.idServicioTransporteExterno,
                    s.numeroGuiaServicioTransporteExterno,
                    s.Fk_Empresa_Transportista_Codigo,
                    s.Fk_Tipo_Servicio_Codigo,
                    s.Fk_Traslado_Codigo,
                    s.estadoServicioTransporteExterno,
                    s.origenServicioTransporteExterno,
                    s.direccionOrigenServicioTransporteExterno,
                    s.destinoServicioTransporteExterno,
                    s.direccionDestinoServicioTransporteExterno,
                    s.fechaContratacionServicioTransporteExterno,
                    s.fechaEstimadaEntregaServicioTransporteExterno,
                    s.fechaEntregaRealServicioTransporteExterno,
                    e.contactoEmpresaTransportista,
                    v.placaVehiculoTransporte,
                    v.tipoVehiculoTransporte,
                    v.capacidadVehiculoTransporte,
                    c.nombreConductorTransporte,
                    c.telefonoConductorTransporte,
                    p.codigoSolicitudPagoTransporte
                  FROM tblServicioTransporteExterno s
                  INNER JOIN tblEmpresaTransportista e
                    ON e.Pk_EmpresaTransportista =
                       s.Fk_Empresa_Transportista_Codigo
                  LEFT JOIN tblVehiculoTransporte v
                    ON v.Fk_Servicio_Transporte_Externo_Codigo =
                       s.idServicioTransporteExterno
                  LEFT JOIN tblConductorTransporte c
                    ON c.Fk_Servicio_Transporte_Externo_Codigo =
                       s.idServicioTransporteExterno
                  LEFT JOIN tblSolicitudPagoTransporte p
                    ON p.Fk_Servicio_Transporte_Externo_Codigo =
                       s.idServicioTransporteExterno
                  ORDER BY s.idServicioTransporteExterno DESC");
        }

        // Ejecutar consultas y devolver un DataTable.
        private DataTable LogisticaFuncConsultar(string sql)
        {
            DataTable tabla = new DataTable();

            using (OdbcConnection conexion =
                LogisticaMetObtenerConexion())
            using (OdbcCommand comando =
                new OdbcCommand(sql, conexion))
            using (OdbcDataAdapter adaptador =
                new OdbcDataAdapter(comando))
            {
                conexion.Open();
                adaptador.Fill(tabla);
            }

            return tabla;
        }

        // Obtener el último traslado que todavía no tenga transporte.
        private int LogisticaFuncObtenerUltimoTraslado(
            OdbcConnection conexion,
            OdbcTransaction transaccion)
        {
            using (OdbcCommand comando = new OdbcCommand(
                @"SELECT t.idTraslado
                  FROM tbltraslado t
                  WHERE t.estadoTraslado <> 'CANCELADO'
                    AND NOT EXISTS
                    (
                        SELECT 1
                        FROM tblServicioTransporteExterno s
                        WHERE s.Fk_Traslado_Codigo = t.idTraslado
                    )
                  ORDER BY t.idTraslado DESC
                  LIMIT 1",
                conexion, transaccion))
            {
                object resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    throw new Exception(
                        "No existen traslados disponibles sin transporte asignado.");
                }

                return Convert.ToInt32(resultado);
            }
        }

        // Obtener origen, destino y direcciones del traslado.
        private void LogisticaMetObtenerDatosTraslado(
            OdbcConnection conexion,
            OdbcTransaction transaccion,
            int idTraslado,
            out string origen,
            out string direccionOrigen,
            out string destino,
            out string direccionDestino)
        {
            using (OdbcCommand comando = new OdbcCommand(
                @"SELECT
                    bo.nombreBodega AS origen,
                    CONCAT_WS(', ',
                        so.ubicacionSucursal,
                        so.paisSucursal) AS direccionOrigen,
                    bd.nombreBodega AS destino,
                    CONCAT_WS(', ',
                        sd.ubicacionSucursal,
                        sd.paisSucursal) AS direccionDestino
                  FROM tbltraslado t
                  INNER JOIN tblbodega bo
                    ON bo.idBodega = t.Fk_Bodega_Origen_Codigo
                  INNER JOIN tblsucursal so
                    ON so.idSucursal = bo.Fk_Sucursal_Codigo
                  INNER JOIN tblbodega bd
                    ON bd.idBodega = t.Fk_Bodega_Destino_Codigo
                  INNER JOIN tblsucursal sd
                    ON sd.idSucursal = bd.Fk_Sucursal_Codigo
                  WHERE t.idTraslado = ?",
                conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@idTraslado", idTraslado);

                using (OdbcDataReader lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                    {
                        throw new Exception(
                            "No se encontraron las bodegas o sucursales del traslado.");
                    }

                    origen = Convert.ToString(lector["origen"]);
                    direccionOrigen =
                        Convert.ToString(lector["direccionOrigen"]);
                    destino = Convert.ToString(lector["destino"]);
                    direccionDestino =
                        Convert.ToString(lector["direccionDestino"]);
                }
            }
        }

        // Obtener vista previa de los próximos códigos.
        public DataTable LogisticaFuncObtenerProximosCodigos()
        {
            return LogisticaFuncConsultar(
                @"SELECT
                    (SELECT COALESCE(
                        MAX(idServicioTransporteExterno), 0) + 1
                     FROM tblServicioTransporteExterno)
                     AS siguienteTransporte,

                    (SELECT COALESCE(
                        MAX(idSolicitudPagoTransporte), 0) + 1
                     FROM tblSolicitudPagoTransporte)
                     AS siguienteSolicitud");
        }

        // Registrar transporte, vehículo, conductor y solicitud de pago.
        public DataTable LogisticaFuncAgregarTransporte(
            int empresa,
            int tipo,
            string estado,
            DateTime contratacion,
            DateTime estimada,
            DateTime? real,
            string placa,
            string tipoVehiculo,
            string capacidad,
            string conductor,
            string telefono)
        {
            DataTable resultado = new DataTable();

            resultado.Columns.Add("idServicio", typeof(int));
            resultado.Columns.Add("numeroGuia", typeof(string));
            resultado.Columns.Add("idSolicitud", typeof(int));
            resultado.Columns.Add("codigoSolicitudPago", typeof(string));

            using (OdbcConnection conexion =
                LogisticaMetObtenerConexion())
            {
                conexion.Open();

                using (OdbcTransaction transaccion =
                    conexion.BeginTransaction())
                {
                    try
                    {
                        int traslado =
                            LogisticaFuncObtenerUltimoTraslado(
                                conexion, transaccion);

                        string origen;
                        string direccionOrigen;
                        string destino;
                        string direccionDestino;

                        LogisticaMetObtenerDatosTraslado(
                            conexion,
                            transaccion,
                            traslado,
                            out origen,
                            out direccionOrigen,
                            out destino,
                            out direccionDestino);

                        string guiaTemporal =
                            "TMP-" + Guid.NewGuid().ToString("N").Substring(0, 20);

                        // Insertar servicio de transporte.
                        using (OdbcCommand comando = new OdbcCommand(
                            @"INSERT INTO tblServicioTransporteExterno
                              (
                                Fk_Empresa_Transportista_Codigo,
                                Fk_Tipo_Servicio_Codigo,
                                Fk_Traslado_Codigo,
                                numeroGuiaServicioTransporteExterno,
                                estadoServicioTransporteExterno,
                                origenServicioTransporteExterno,
                                direccionOrigenServicioTransporteExterno,
                                destinoServicioTransporteExterno,
                                direccionDestinoServicioTransporteExterno,
                                fechaContratacionServicioTransporteExterno,
                                fechaEstimadaEntregaServicioTransporteExterno,
                                fechaEntregaRealServicioTransporteExterno
                              )
                              VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@empresa", empresa);
                            comando.Parameters.AddWithValue("@tipo", tipo);
                            comando.Parameters.AddWithValue("@traslado", traslado);
                            comando.Parameters.AddWithValue("@guia", guiaTemporal);
                            comando.Parameters.AddWithValue("@estado", estado);
                            comando.Parameters.AddWithValue("@origen", origen);
                            comando.Parameters.AddWithValue("@direccionOrigen", direccionOrigen);
                            comando.Parameters.AddWithValue("@destino", destino);
                            comando.Parameters.AddWithValue("@direccionDestino", direccionDestino);
                            comando.Parameters.AddWithValue("@contratacion", contratacion.Date);
                            comando.Parameters.AddWithValue("@estimada", estimada.Date);
                            comando.Parameters.AddWithValue(
                                "@real",
                                real.HasValue
                                    ? (object)real.Value.Date
                                    : DBNull.Value);

                            comando.ExecuteNonQuery();
                        }

                        int idServicio;

                        using (OdbcCommand comando = new OdbcCommand(
                            "SELECT LAST_INSERT_ID()",
                            conexion, transaccion))
                        {
                            idServicio = Convert.ToInt32(
                                comando.ExecuteScalar());
                        }

                        string guia =
                            "GUIA-" + idServicio.ToString("D6");

                        // Actualizar número de guía.
                        using (OdbcCommand comando = new OdbcCommand(
                            @"UPDATE tblServicioTransporteExterno
                              SET numeroGuiaServicioTransporteExterno = ?
                              WHERE idServicioTransporteExterno = ?",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@guia", guia);
                            comando.Parameters.AddWithValue("@id", idServicio);
                            comando.ExecuteNonQuery();
                        }

                        // Registrar vehículo.
                        using (OdbcCommand comando = new OdbcCommand(
                            @"INSERT INTO tblVehiculoTransporte
                              (
                                Fk_Servicio_Transporte_Externo_Codigo,
                                placaVehiculoTransporte,
                                tipoVehiculoTransporte,
                                capacidadVehiculoTransporte
                              )
                              VALUES (?, ?, ?, ?)",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", idServicio);
                            comando.Parameters.AddWithValue("@placa", placa);
                            comando.Parameters.AddWithValue("@tipo", tipoVehiculo);
                            comando.Parameters.AddWithValue("@capacidad", capacidad);
                            comando.ExecuteNonQuery();
                        }

                        // Registrar conductor.
                        using (OdbcCommand comando = new OdbcCommand(
                            @"INSERT INTO tblConductorTransporte
                              (
                                Fk_Servicio_Transporte_Externo_Codigo,
                                nombreConductorTransporte,
                                telefonoConductorTransporte
                              )
                              VALUES (?, ?, ?)",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", idServicio);
                            comando.Parameters.AddWithValue("@nombre", conductor);
                            comando.Parameters.AddWithValue("@telefono", telefono);
                            comando.ExecuteNonQuery();
                        }

                        // Registrar solicitud de pago.
                        string codigoTemporal =
                            "TMP-" + Guid.NewGuid().ToString("N").Substring(0, 20);

                        using (OdbcCommand comando = new OdbcCommand(
                            @"INSERT INTO tblSolicitudPagoTransporte
                              (
                                Fk_Servicio_Transporte_Externo_Codigo,
                                codigoSolicitudPagoTransporte
                              )
                              VALUES (?, ?)",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", idServicio);
                            comando.Parameters.AddWithValue("@codigo", codigoTemporal);
                            comando.ExecuteNonQuery();
                        }

                        int idSolicitud;

                        using (OdbcCommand comando = new OdbcCommand(
                            "SELECT LAST_INSERT_ID()",
                            conexion, transaccion))
                        {
                            idSolicitud = Convert.ToInt32(
                                comando.ExecuteScalar());
                        }

                        string codigo =
                            "SP-" + idSolicitud.ToString("D6");

                        // Actualizar código de solicitud.
                        using (OdbcCommand comando = new OdbcCommand(
                            @"UPDATE tblSolicitudPagoTransporte
                              SET codigoSolicitudPagoTransporte = ?
                              WHERE idSolicitudPagoTransporte = ?",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@codigo", codigo);
                            comando.Parameters.AddWithValue("@id", idSolicitud);
                            comando.ExecuteNonQuery();
                        }

                        transaccion.Commit();

                        resultado.Rows.Add(
                            idServicio,
                            guia,
                            idSolicitud,
                            codigo);

                        return resultado;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        // Modificar transporte, vehículo y conductor.
        public void LogisticaMetModificarTransporte(
            int id,
            int empresa,
            int tipo,
            string estado,
            DateTime contratacion,
            DateTime estimada,
            DateTime? real,
            string placa,
            string tipoVehiculo,
            string capacidad,
            string conductor,
            string telefono)
        {
            using (OdbcConnection conexion =
                LogisticaMetObtenerConexion())
            {
                conexion.Open();

                using (OdbcTransaction transaccion =
                    conexion.BeginTransaction())
                {
                    try
                    {
                        using (OdbcCommand comando = new OdbcCommand(
                            @"UPDATE tblServicioTransporteExterno
                              SET Fk_Empresa_Transportista_Codigo = ?,
                                  Fk_Tipo_Servicio_Codigo = ?,
                                  estadoServicioTransporteExterno = ?,
                                  fechaContratacionServicioTransporteExterno = ?,
                                  fechaEstimadaEntregaServicioTransporteExterno = ?,
                                  fechaEntregaRealServicioTransporteExterno = ?
                              WHERE idServicioTransporteExterno = ?",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@empresa", empresa);
                            comando.Parameters.AddWithValue("@tipo", tipo);
                            comando.Parameters.AddWithValue("@estado", estado);
                            comando.Parameters.AddWithValue("@contratacion", contratacion.Date);
                            comando.Parameters.AddWithValue("@estimada", estimada.Date);
                            comando.Parameters.AddWithValue(
                                "@real",
                                real.HasValue
                                    ? (object)real.Value.Date
                                    : DBNull.Value);
                            comando.Parameters.AddWithValue("@id", id);

                            comando.ExecuteNonQuery();
                        }

                        // Modificar o registrar vehículo.
                        using (OdbcCommand comando = new OdbcCommand(
                            @"INSERT INTO tblVehiculoTransporte
                              (
                                Fk_Servicio_Transporte_Externo_Codigo,
                                placaVehiculoTransporte,
                                tipoVehiculoTransporte,
                                capacidadVehiculoTransporte
                              )
                              VALUES (?, ?, ?, ?)
                              ON DUPLICATE KEY UPDATE
                                placaVehiculoTransporte =
                                    VALUES(placaVehiculoTransporte),
                                tipoVehiculoTransporte =
                                    VALUES(tipoVehiculoTransporte),
                                capacidadVehiculoTransporte =
                                    VALUES(capacidadVehiculoTransporte)",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", id);
                            comando.Parameters.AddWithValue("@placa", placa);
                            comando.Parameters.AddWithValue("@tipo", tipoVehiculo);
                            comando.Parameters.AddWithValue("@capacidad", capacidad);
                            comando.ExecuteNonQuery();
                        }

                        // Modificar o registrar conductor.
                        using (OdbcCommand comando = new OdbcCommand(
                            @"INSERT INTO tblConductorTransporte
                              (
                                Fk_Servicio_Transporte_Externo_Codigo,
                                nombreConductorTransporte,
                                telefonoConductorTransporte
                              )
                              VALUES (?, ?, ?)
                              ON DUPLICATE KEY UPDATE
                                nombreConductorTransporte =
                                    VALUES(nombreConductorTransporte),
                                telefonoConductorTransporte =
                                    VALUES(telefonoConductorTransporte)",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", id);
                            comando.Parameters.AddWithValue("@nombre", conductor);
                            comando.Parameters.AddWithValue("@telefono", telefono);
                            comando.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        // Eliminar transporte.
        public void LogisticaMetEliminarTransporte(int id)
        {
            using (OdbcConnection conexion =
                LogisticaMetObtenerConexion())
            {
                conexion.Open();

                using (OdbcTransaction transaccion =
                    conexion.BeginTransaction())
                {
                    try
                    {
                        using (OdbcCommand comando = new OdbcCommand(
                            @"SELECT idServicioTransporteExterno
                              FROM tblServicioTransporteExterno
                              WHERE idServicioTransporteExterno = ?
                              FOR UPDATE",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", id);

                            object valor = comando.ExecuteScalar();

                            if (valor == null || valor == DBNull.Value)
                            {
                                throw new Exception(
                                    "El transporte no existe.");
                            }
                        }

                        using (OdbcCommand comando = new OdbcCommand(
                            @"DELETE FROM tblSolicitudPagoTransporte
                              WHERE Fk_Servicio_Transporte_Externo_Codigo = ?",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", id);
                            comando.ExecuteNonQuery();
                        }

                        using (OdbcCommand comando = new OdbcCommand(
                            @"DELETE FROM tblConductorTransporte
                              WHERE Fk_Servicio_Transporte_Externo_Codigo = ?",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", id);
                            comando.ExecuteNonQuery();
                        }

                        using (OdbcCommand comando = new OdbcCommand(
                            @"DELETE FROM tblVehiculoTransporte
                              WHERE Fk_Servicio_Transporte_Externo_Codigo = ?",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", id);
                            comando.ExecuteNonQuery();
                        }

                        using (OdbcCommand comando = new OdbcCommand(
                            @"DELETE FROM tblServicioTransporteExterno
                              WHERE idServicioTransporteExterno = ?",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@id", id);

                            int filas = comando.ExecuteNonQuery();

                            if (filas == 0)
                            {
                                throw new Exception(
                                    "No se pudo eliminar el transporte.");
                            }
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
