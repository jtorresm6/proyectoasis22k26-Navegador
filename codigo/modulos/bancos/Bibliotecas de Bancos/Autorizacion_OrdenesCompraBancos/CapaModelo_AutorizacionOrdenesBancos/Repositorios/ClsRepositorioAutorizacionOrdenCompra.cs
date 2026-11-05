using CapaModelo_AutorizacionOrdenes.Contratos;
using CapaModelo_AutorizacionOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 4/11/2026
 * ==================================================================
 * Propósito :
 *  La clase Repositorio de Autorización de Orden de Compra se encarga de
 *  registrar, dentro de una sola transacción ODBC, el encabezado de la
 *  autorización, la evidencia de fondos, el detalle por cuenta, el cambio de
 *  estado de la orden y la bitácora, heredando de ClsSentencias e
 *  implementando la interfaz IRepositorioAutorizacionOrdenCompra.
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Repositorios
{
    public class ClsRepositorioAutorizacionOrdenCompra : ClsSentencias, IRepositorioAutorizacionOrdenCompra
    {
        private string _SelectAll;
        private string _InsertEncabezado, _InsertEvidencia, _InsertDetalle, _UpdateOrden, _InsertBitacora, _UltimoId, _UpdateDisponibilidad;
        public ClsRepositorioAutorizacionOrdenCompra()
        {
         _SelectAll = "SELECT Pk_AutOrdenCompra, Fk_OrdenCompraRecibida_Codigo, identificadorEmpresaAutOrdenCompra, " +
                      "estadoAutOrdenCompra, identificadorUsuarioAutoriza, fechaDecisionAutOrdenCompra, " +
                      "comentarioAutOrdenCompra, motivoRechazoAutOrdenCompra, created_at, updated_at " +
                      "FROM tblAutOrdenCompra";

         _InsertEncabezado = "INSERT INTO tblAutOrdenCompra (Fk_OrdenCompraRecibida_Codigo, identificadorEmpresaAutOrdenCompra, estadoAutOrdenCompra, " +
                             "identificadorUsuarioAutoriza, fechaDecisionAutOrdenCompra, comentarioAutOrdenCompra, motivoRechazoAutOrdenCompra) " +
                             "VALUES (?, ?, ?, ?, NOW(), ?, ?)";

         _InsertEvidencia = "INSERT INTO tblAutDisponibilidadFondos (Fk_AutOrdenCompra_Codigo, Fk_CuentaBancaria_Codigo, fechaCorteAutDisponibilidadFondos, " +
                             "saldoDisponibleAutDisponibilidadFondos, Fk_Moneda_Origen, Fk_Moneda_Destino, tipoCambioAutDisponibilidadFondos, " +
                             "Fk_TipoCambio_Codigo, fuenteAutDisponibilidadFondos) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

         _InsertDetalle = "INSERT INTO tblAutPagoOrden (Fk_AutOrdenCompra_Codigo, Fk_CuentaBancaria_Codigo, Fk_AutDisponibilidadFondos_Codigo, " +                             "montoAsignadoAutPagoOrden, estadoAutPagoOrden, fechaReservaAutPagoOrden) VALUES (?, ?, ?, ?, 'RESERVADA', NOW())";

         _UpdateOrden = "UPDATE tblEncabezadoOrdenCompra SET estadoOrdenCompra = ? WHERE idOrdenCompra = ? AND estadoOrdenCompra = 'Pendiente'";

         _UpdateDisponibilidad = "UPDATE tblDisponibilidadDiaria " +
                                 "SET comprometidoDisponibilidadDiaria = comprometidoDisponibilidadDiaria + ?, " +
                                 "saldoDisponibleDisponibilidadDiaria = saldoDisponibleDisponibilidadDiaria - ?, " +
                                 "saldoDisponibleGTQDisponibilidadDiaria = saldoDisponibleGTQDisponibilidadDiaria - (? * tipoCambioAplicadoDisponibilidadDiaria) " +
                                 "WHERE Fk_CuentaBancaria_Codigo = ? AND fechaDisponibilidadDiaria = ? " +
                                 "AND saldoDisponibleDisponibilidadDiaria >= ?";

         _InsertBitacora = "INSERT INTO tblBitacora (idUsuario, accionBitacora, tablaBitacora, idRegistroBitacora, detallesBitacora, ipBitacora, fechaHoraBitacora) " +
                            "VALUES (?, ?, ?, ?, ?, ?, ?)";

         _UltimoId = "SELECT LAST_INSERT_ID()";
        }

        public int BancosMetAgregar(ClsAutorizacionOrdenCompra Entidad)
        {
            bool Autoriza = Entidad.EstadoAutorizacionOrden == "AUTORIZADA";
            using (var Conexion = BancosMetObtenerConexion())
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        //encabezado 
                        var ParametroEncabezado = new List<OdbcParameter>();
                        ParametroEncabezado.Add(new OdbcParameter("p_IdOrdenCompra", Entidad.IdOrdenCompra));
                        ParametroEncabezado.Add(new OdbcParameter("p_IdEmpresa", Entidad.IdEmpresaAutorizacionOrden));
                        ParametroEncabezado.Add(new OdbcParameter("p_Estado", Entidad.EstadoAutorizacionOrden));
                        ParametroEncabezado.Add(new OdbcParameter("p_IdUsuarioAutoriza", Entidad.IdUsuarioAutoriza));
                        ParametroEncabezado.Add(new OdbcParameter("p_Comentario", (object)Entidad.ComentarioAutorizacionOrden ?? DBNull.Value));
                        ParametroEncabezado.Add(new OdbcParameter("p_MotivoRechazo", (object)Entidad.MotivoRechazoAutorizacionOrden ?? DBNull.Value));
                        BancosMetEjecucionNonQuery(_InsertEncabezado, ParametroEncabezado, CommandType.Text, Conexion, Transaccion);

                        long IdAutorizacion = Convert.ToInt64(BancosMetEjecucionEscalar(_UltimoId, new List<OdbcParameter>(), CommandType.Text, Conexion, Transaccion));

                        //Evidencia
                        if (Autoriza)
                        {
                            foreach (var Evidencia in Entidad.Evidencias)
                            {
                                var ParametroEvidencia = new List<OdbcParameter>();
                                ParametroEvidencia.Add(new OdbcParameter("p_IdAutorizacion", IdAutorizacion));
                                ParametroEvidencia.Add(new OdbcParameter("p_IdCuenta", Evidencia.IdCuenta));
                                ParametroEvidencia.Add(new OdbcParameter("p_FechaCorte", Evidencia.FechaCorteDisponibilidadFondos));
                                ParametroEvidencia.Add(new OdbcParameter("p_Saldo", Evidencia.SaldoDisponibleDisponibilidadFondos));
                                ParametroEvidencia.Add(new OdbcParameter("p_IdMonedaOrigen", Evidencia.IdMonedaOrigen));
                                ParametroEvidencia.Add(new OdbcParameter("p_IdMonedaDestino", Evidencia.IdMonedaDestino));
                                ParametroEvidencia.Add(new OdbcParameter("p_TipoCambio", Evidencia.TipoCambioDisponibilidadFondos));
                                ParametroEvidencia.Add(new OdbcParameter("p_IdTipoCambio", Evidencia.IdTipoCambio.HasValue ? (object)Evidencia.IdTipoCambio.Value : DBNull.Value));
                                ParametroEvidencia.Add(new OdbcParameter("p_Fuente", Evidencia.FuenteDisponibilidadFondos));
                                BancosMetEjecucionNonQuery(_InsertEvidencia, ParametroEvidencia, CommandType.Text, Conexion, Transaccion);

                                long IdEvidencia = Convert.ToInt64(BancosMetEjecucionEscalar(_UltimoId, new List<OdbcParameter>(), CommandType.Text, Conexion, Transaccion));

                                var Detalle = Entidad.Detalles.First(d => d.IdCuenta == Evidencia.IdCuenta);
                                var ParametroDetalle = new List<OdbcParameter>();
                                ParametroDetalle.Add(new OdbcParameter("p_IdAutorizacion", IdAutorizacion));
                                ParametroDetalle.Add(new OdbcParameter("p_IdCuenta", Detalle.IdCuenta));
                                ParametroDetalle.Add(new OdbcParameter("p_IdEvidencia", IdEvidencia));
                                ParametroDetalle.Add(new OdbcParameter("p_Monto", Detalle.MontoAsignadoPagoOrden));
                                BancosMetEjecucionNonQuery(_InsertDetalle, ParametroDetalle, CommandType.Text, Conexion, Transaccion);

                                decimal Monto = Detalle.MontoAsignadoPagoOrden;
                                var ParametroReserva = new List<OdbcParameter>();
                                ParametroReserva.Add(new OdbcParameter("p_MontoComprometido", Monto));
                                ParametroReserva.Add(new OdbcParameter("p_MontoSaldo", Monto));
                                ParametroReserva.Add(new OdbcParameter("p_MontoGTQ", Monto));
                                ParametroReserva.Add(new OdbcParameter("p_IdCuenta", Detalle.IdCuenta));
                                ParametroReserva.Add(new OdbcParameter("p_Fecha", Evidencia.FechaCorteDisponibilidadFondos));
                                ParametroReserva.Add(new OdbcParameter("p_MontoMinimo", Monto));

                                int FilasReserva = BancosMetEjecucionNonQuery(_UpdateDisponibilidad, ParametroReserva, CommandType.Text, Conexion, Transaccion);
                                if (FilasReserva != 1)
                                    throw new InvalidOperationException("La cuenta ya no tiene saldo disponible suficiente.");
                            }
                        }


                        //ACTUALIZAR ORDEN
                        var ParametroOrden = new List<OdbcParameter>();
                        ParametroOrden.Add(new OdbcParameter("p_Estado", Autoriza ? "Autorizada" : "Rechazada"));
                        ParametroOrden.Add(new OdbcParameter("p_IdOrdenCompra", Entidad.IdOrdenCompra));
                        int Filas = BancosMetEjecucionNonQuery(_UpdateOrden, ParametroOrden, CommandType.Text, Conexion, Transaccion);
                        if (Filas != 1)
                            throw new InvalidOperationException("La orden ya no está pendiente.");

                        //BITACORA
                        var ParametroBitacora = new List<OdbcParameter>();
                        ParametroBitacora.Add(new OdbcParameter("p_idUsuario", Entidad.IdUsuarioAutoriza));
                        ParametroBitacora.Add(new OdbcParameter("p_accionBitacora", Autoriza ? "AUTORIZAR_OC" : "RECHAZAR_OC"));
                        ParametroBitacora.Add(new OdbcParameter("p_tablaBitacora", "tblAutOrdenCompra"));
                        ParametroBitacora.Add(new OdbcParameter("p_idRegistroBitacora", (int)IdAutorizacion));
                        ParametroBitacora.Add(new OdbcParameter("p_detallesBitacora", "Orden " + Entidad.IdOrdenCompra));
                        ParametroBitacora.Add(new OdbcParameter("p_ipBitacora", Entidad.Ip));
                        ParametroBitacora.Add(new OdbcParameter("p_fechaHoraBitacora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        BancosMetEjecucionNonQuery(_InsertBitacora, ParametroBitacora, CommandType.Text, Conexion, Transaccion);

                        Transaccion.Commit();
                        return (int)IdAutorizacion;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public int BancosMetEditar(ClsAutorizacionOrdenCompra Entidad)
        {
            throw new NotSupportedException("Una autorización no se edita.");
        }

        public int BancosMetRemover(ClsAutorizacionOrdenCompra Entidad)
        {
            throw new NotSupportedException("Una autorización no se elimina.");
        }
        public IEnumerable<ClsAutorizacionOrdenCompra> BancosMetObtenerTodos()
        {
            var Lista = new List<ClsAutorizacionOrdenCompra>();
            var TablaDatos = BancosMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Row in TablaDatos.Rows)
            {
                var Tipo = new ClsAutorizacionOrdenCompra();
                Tipo.IdAutorizacionOrden = Convert.ToInt64(Row[0]);
                Tipo.IdOrdenCompra = Convert.ToInt64(Row[1]);
                Tipo.IdEmpresaAutorizacionOrden = Convert.ToInt32(Row[2]);
                Tipo.EstadoAutorizacionOrden = Convert.ToString(Row[3]);
                Tipo.IdUsuarioAutoriza = Row[4] == DBNull.Value ? (int?)null : Convert.ToInt32(Row[4]);
                Tipo.FechaDecisionAutorizacionOrden = Row[5] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Row[5]);
                Tipo.ComentarioAutorizacionOrden = Convert.ToString(Row[6]);
                Tipo.MotivoRechazoAutorizacionOrden = Convert.ToString(Row[7]);
                Tipo.CreatedAt = Convert.ToDateTime(Row[8]);
                Tipo.UpdatedAt = Convert.ToDateTime(Row[9]);
                Lista.Add(Tipo);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }
    }
}
