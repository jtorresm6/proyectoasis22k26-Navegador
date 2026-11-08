// ------------------------------------------------------
// Autor: Gabriela Pinto García
// Carné: 9959-23-1087
// Módulo: Logística - Transporte Externo (Repositorio)
// ------------------------------------------------------
using System;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_TransporteExterno_Logistica.Repositorio
{
    public class ClsRepositorioRutaSeguimiento : ClsRepositorio
    {
        private DataTable LogisticaFuncConsultar(string consulta, params object[] parametros)
        {
            DataTable tabla = new DataTable();
            try
            {
                using (OdbcConnection conexion = LogisticaMetObtenerConexion())
                {
                    using (OdbcCommand comando = new OdbcCommand(consulta, conexion))
                    {
                        if (parametros != null)
                        {
                            foreach (object p in parametros)
                            {
                                comando.Parameters.Add(new OdbcParameter("?", p ?? DBNull.Value));
                            }
                        }
                        using (OdbcDataAdapter adaptador = new OdbcDataAdapter(comando))
                        {
                            adaptador.Fill(tabla);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR REPOSITORIO SELECT]: " + ex.Message);
            }
            return tabla;
        }

        private int LogisticaFuncEjecutar(string consulta, params object[] parametros)
        {
            try
            {
                using (OdbcConnection conexion = LogisticaMetObtenerConexion())
                {
                    using (OdbcCommand comando = new OdbcCommand(consulta, conexion))
                    {
                        if (parametros != null)
                        {
                            foreach (object p in parametros)
                            {
                                comando.Parameters.Add(new OdbcParameter("?", p ?? DBNull.Value));
                            }
                        }
                        if (conexion.State != ConnectionState.Open) conexion.Open();
                        return comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR REPOSITORIO EXECUTE]: " + ex.Message);
                return 0;
            }
        }

        public DataTable LogisticaFuncObtenerGuias()
        {
            return LogisticaFuncConsultar(
                @"SELECT idServicioTransporteExterno AS Id, numeroGuiaServicioTransporteExterno AS Guia 
                  FROM tblServicioTransporteExterno 
                  WHERE numeroGuiaServicioTransporteExterno IS NOT NULL 
                  ORDER BY idServicioTransporteExterno DESC;");
        }

        public DataTable LogisticaFuncObtenerBodegas()
        {
            return LogisticaFuncConsultar(
                @"SELECT b.idBodega AS Id, b.nombreBodega AS Nombre, IFNULL(s.ubicacionSucursal, '') AS Direccion 
                  FROM tblBodega b 
                  LEFT JOIN tblSucursal s ON b.Fk_Sucursal_Codigo = s.idSucursal 
                  ORDER BY b.nombreBodega;");
        }

        public DataTable LogisticaFuncObtenerRuta(int idServicio)
        {
            return LogisticaFuncConsultar(
                @"SELECT IFNULL(origenServicioTransporteExterno, '') AS Origen, 
                         IFNULL(direccionOrigenServicioTransporteExterno, '') AS DireccionOrigen, 
                         IFNULL(destinoServicioTransporteExterno, '') AS Destino, 
                         IFNULL(direccionDestinoServicioTransporteExterno, '') AS DireccionDestino, 
                         IFNULL(estadoServicioTransporteExterno, 'Programado') AS Estado,
                         Fk_Traslado_Codigo AS IdTraslado
                  FROM tblServicioTransporteExterno 
                  WHERE idServicioTransporteExterno = ?;", idServicio);
        }

        public DataTable LogisticaFuncObtenerBodegasTraslado(int idTraslado)
        {
            return LogisticaFuncConsultar(
                @"SELECT Fk_Bodega_Origen_Codigo AS IdOrigen, Fk_Bodega_Destino_Codigo AS IdDestino
                  FROM tblTraslado
                  WHERE idTraslado = ?;", idTraslado);
        }

        public DataTable LogisticaFuncObtenerUbicacion(int idServicio)
        {
            return LogisticaFuncConsultar(
                @"SELECT IFNULL(estadoServicioTransporteExterno, 'Programado') AS Ubicacion 
                  FROM tblServicioTransporteExterno 
                  WHERE idServicioTransporteExterno = ?;", idServicio);
        }

        public DataTable LogisticaFuncObtenerHistorialUbicaciones(int idServicio)
        {
            return LogisticaFuncConsultar(
                @"SELECT estadoServicioTransporteExterno AS Ubicacion 
                  FROM tblServicioTransporteExterno 
                  WHERE idServicioTransporteExterno = ? AND estadoServicioTransporteExterno LIKE '%,%';", idServicio);
        }

        public int LogisticaMetGuardarRuta(int idServicio, string origen, string direccionOrigen, string destino, string direccionDestino)
        {
            return LogisticaFuncEjecutar(
                @"UPDATE tblServicioTransporteExterno 
                  SET origenServicioTransporteExterno = ?, 
                      direccionOrigenServicioTransporteExterno = ?, 
                      destinoServicioTransporteExterno = ?, 
                      direccionDestinoServicioTransporteExterno = ? 
                  WHERE idServicioTransporteExterno = ?;", origen, direccionOrigen, destino, direccionDestino, idServicio);
        }

        public int LogisticaMetEliminarRuta(int idServicio)
        {
            return LogisticaFuncEjecutar(
                @"UPDATE tblServicioTransporteExterno 
                  SET origenServicioTransporteExterno = NULL, 
                      direccionOrigenServicioTransporteExterno = NULL, 
                      destinoServicioTransporteExterno = NULL, 
                      direccionDestinoServicioTransporteExterno = NULL 
                  WHERE idServicioTransporteExterno = ?;", idServicio);
        }

        public int LogisticaMetGuardarUbicacion(int idServicio, string ubicacion)
        {
            return LogisticaFuncEjecutar(
                @"UPDATE tblServicioTransporteExterno 
                  SET estadoServicioTransporteExterno = ? 
                  WHERE idServicioTransporteExterno = ?;", ubicacion, idServicio);
        }
    }
}