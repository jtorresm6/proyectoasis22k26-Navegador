using System;
using System.Collections.Generic;
using System.Data.Odbc;
using CapaModelo_Enlace_Logistica.Contratos;
using CapaModelo_Enlace_Logistica.Entidades;
namespace CapaModelo_Enlace_Logistica.Repositorios
{
    public class ClsRepositorioSolicitudPago :
    ClsRepositorioBase, ClsIRepositorioSolicitudPago
    {
        public IEnumerable<ClsServicioTransporte> ObtenerServiciosDisponibles()
        {
            const string sql = @"
                SELECT s.Pk_Servicio_Transporte_Externo,
                       s.numeroGuiaServicioTransporteExterno,
                       e.nombreEmpresaTransportista,
                       t.nombreTipoServicio,
                       c.cantidadCostoTransporte,
                       c.costoUnitarioCostoTransporte,
                       c.costoTotalCostoTransporte,
                       c.monedaCostoTransporte,
                       c.tipoPagoCostoTransporte,
                       s.fechaContratacionServicioTransporteExterno
                FROM dbLogistica.tblServicioTransporteExterno s
                INNER JOIN dbLogistica.tblEmpresaTransportista e
                  ON e.Pk_Empresa_Transportista = s.Fk_Empresa_Transportista_Codigo
                INNER JOIN dbLogistica.tblTipoServicio t
                  ON t.Pk_Tipo_Servicio = s.Fk_Tipo_Servicio_Codigo
                INNER JOIN dbLogistica.tblCostoTransporte c
                  ON c.Fk_Servicio_Transporte_Externo_Codigo = s.Pk_Servicio_Transporte_Externo
                WHERE s.estadoServicioTransporteExterno <> 'Cancelado'
                ORDER BY s.Pk_Servicio_Transporte_Externo DESC";
            var servicios = new List<ClsServicioTransporte>();
            using (OdbcConnection conexion = ObtenerConexion())
            using (OdbcCommand comando = new OdbcCommand(sql, conexion))
            {
                conexion.Open();
                using (OdbcDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        servicios.Add(new ClsServicioTransporte
                        {
                            IdServicio = Convert.ToInt32(lector.GetValue(0)),
                            NumeroGuia = Convert.ToString(lector.GetValue(1)),
                            NombreTransportista = Convert.ToString(lector.GetValue(2)),
                            TipoServicio = Convert.ToString(lector.GetValue(3)),
                            Cantidad = Convert.ToDecimal(lector.GetValue(4)),
                            CostoUnitario = Convert.ToDecimal(lector.GetValue(5)),
                            MontoTotal = Convert.ToDecimal(lector.GetValue(6)),
                            Moneda = Convert.ToString(lector.GetValue(7)),
                            TipoPago = Convert.ToString(lector.GetValue(8)),
                            FechaServicio = lector.IsDBNull(9) ? (DateTime?)null : Convert.ToDateTime(lector.GetValue(9)),
                            NitTransportista = null,
                            DireccionTransportista = null
                        });
                    }
                }
            }
            return servicios;
        }
    }
}
