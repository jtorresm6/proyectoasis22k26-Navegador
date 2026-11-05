using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Componentes.Repositorios;
using CapaModelo_PagosProveedores.Contratos;
using CapaModelo_PagosProveedores.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Propósito :
 *  Repositorio de pagos a proveedores (encabezado + detalle) sobre ODBC.
 *  Cada operación de escritura corre en una sola transacción.
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores.Repositorios
{
    public class ClsRepositorioPagoProveedor : ClsConexionCompras, IRepositorioPagoProveedor
    {
        private const string MonedaBase = "GTQ";

        // ==================== CONEXIÓN / UTILIDADES ====================
        private OdbcConnection ComprasFuncAbrirConexion()
        {
            // El repositorio hereda de ClsConexionCompras y usa su método protegido.
            OdbcConnection cn = (OdbcConnection)ComprasMetObtenerConexion();
            if (cn.State != ConnectionState.Open) cn.Open();
            return cn;
        }

        // Crea un comando con parámetros posicionales, en el mismo orden de los "?"
        private static OdbcCommand ComprasFuncComando(OdbcConnection cn, OdbcTransaction tx, string sql, params object[] valores)
        {
            var cmd = new OdbcCommand(sql, cn, tx);
            foreach (var v in valores)
                cmd.Parameters.AddWithValue("p" + cmd.Parameters.Count, v ?? DBNull.Value);
            return cmd;
        }

        private static bool ComprasFuncEsEstadoEfectivo(string estado)
        {
            return estado == "Pagado" || estado == "Programado";
        }

        private static object[] ComprasFuncValoresEncabezado(ClsEncabezadoPagoProveedor p)
        {
            return new object[]
            {
                p.IdProveedor,
                p.FechaPagoProveedor.Date,
                p.MontoPagoProveedor,
                p.IdMetodoPago,
                p.IdMoneda,
                p.IdTipoCambio.HasValue ? (object)p.IdTipoCambio.Value : DBNull.Value,
                p.TipoCambioPagoProveedor,
                string.IsNullOrWhiteSpace(p.ReferenciaPagoProveedor) ? (object)DBNull.Value : p.ReferenciaPagoProveedor,
                p.EstadoPagoProveedor
            };
        }

        // ==================== AGREGAR ====================
        public int ComprasMetAgregar(ClsEncabezadoPagoProveedor Pago)
        {
            using (var cn = ComprasFuncAbrirConexion())
            using (var tx = cn.BeginTransaction())
            {
                try
                {
                    int idPago;
                    using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoInsertarEncabezado, ComprasFuncValoresEncabezado(Pago)))
                        cmd.ExecuteNonQuery();
                    using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.UltimoIdInsertado))
                        idPago = Convert.ToInt32(cmd.ExecuteScalar());

                    bool efectivo = ComprasFuncEsEstadoEfectivo(Pago.EstadoPagoProveedor);
                    foreach (var d in Pago.Detalles)
                    {
                        ComprasFuncInsertarDetalle(cn, tx, idPago, d);
                        if (efectivo)
                            ComprasFuncAplicarSaldo(cn, tx, Pago.IdProveedor, d, Pago.IdMoneda, Pago.TipoCambioPagoProveedor, true);
                    }

                    string beneficiario = ComprasFuncEscalarTexto(cn, tx, ClsSentenciasCompras.PagoNombreProveedor, Pago.IdProveedor);
                    string metodo = ComprasFuncEscalarTexto(cn, tx, ClsSentenciasCompras.PagoNombreMetodo, Pago.IdMetodoPago);

                    using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoInsertarSolicitud,
                        idPago, beneficiario, Pago.MontoPagoProveedor, metodo))
                        cmd.ExecuteNonQuery();

                    tx.Commit();
                    return idPago;
                }
                catch (ClsExcepcionDatosCompras) { tx.Rollback(); throw; }
                catch (OdbcException) { tx.Rollback(); throw new ClsExcepcionDatosCompras("No se pudo registrar el pago. Verifique los datos e intente nuevamente."); }
                catch { tx.Rollback(); throw; }
            }
        }

        // ==================== EDITAR ====================
        public void ComprasMetEditar(ClsEncabezadoPagoProveedor Pago)
        {
            using (var cn = ComprasFuncAbrirConexion())
            using (var tx = cn.BeginTransaction())
            {
                try
                {
                    int provAnt; string estadoAnt; long monedaAnt; decimal tcAnt;
                    ComprasFuncLeerEncabezadoBloqueado(cn, tx, Pago.IdPagoProveedor, out provAnt, out estadoAnt, out monedaAnt, out tcAnt);

                    ComprasFuncValidarSolicitudNoProcesada(cn, tx, Pago.IdPagoProveedor, "modificar");

                    // 1. Revertir el efecto anterior
                    if (ComprasFuncEsEstadoEfectivo(estadoAnt))
                    {
                        foreach (var d in ComprasFuncObtenerDetalles(cn, tx, Pago.IdPagoProveedor))
                            ComprasFuncAplicarSaldo(cn, tx, provAnt, d, monedaAnt, tcAnt, false);
                    }

                    // 2. Reemplazar detalles y encabezado
                    ComprasFuncEjecutarPorIdPago(cn, tx, ClsSentenciasCompras.PagoEliminarDetalles, Pago.IdPagoProveedor);

                    var valores = new List<object>(ComprasFuncValoresEncabezado(Pago));
                    valores.Add(Pago.IdPagoProveedor);
                    using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoActualizarEncabezado, valores.ToArray()))
                        cmd.ExecuteNonQuery();

                    // 3. Aplicar el nuevo efecto
                    bool efectivo = ComprasFuncEsEstadoEfectivo(Pago.EstadoPagoProveedor);
                    foreach (var d in Pago.Detalles)
                    {
                        ComprasFuncInsertarDetalle(cn, tx, Pago.IdPagoProveedor, d);
                        if (efectivo)
                            ComprasFuncAplicarSaldo(cn, tx, Pago.IdProveedor, d, Pago.IdMoneda, Pago.TipoCambioPagoProveedor, true);
                    }

                    // 4. Solicitud de pago
                    string metodo = ComprasFuncEscalarTexto(cn, tx, ClsSentenciasCompras.PagoNombreMetodo, Pago.IdMetodoPago);
                    string estadoSolicitud = (Pago.EstadoPagoProveedor == "Rechazado" || Pago.EstadoPagoProveedor == "Anulado") ? "Rechazada" : "Pendiente";
                    using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoActualizarSolicitud,
                        Pago.MontoPagoProveedor, metodo, estadoSolicitud, Pago.IdPagoProveedor))
                        cmd.ExecuteNonQuery();

                    tx.Commit();
                }
                catch (ClsExcepcionDatosCompras) { tx.Rollback(); throw; }
                catch (OdbcException) { tx.Rollback(); throw new ClsExcepcionDatosCompras("No se pudo actualizar el pago. Verifique los datos e intente nuevamente."); }
                catch { tx.Rollback(); throw; }
            }
        }

        // ==================== REMOVER ====================
        public void ComprasMetRemover(ClsEncabezadoPagoProveedor Pago)
        {
            using (var cn = ComprasFuncAbrirConexion())
            using (var tx = cn.BeginTransaction())
            {
                try
                {
                    int prov; string estado; long moneda; decimal tc;
                    ComprasFuncLeerEncabezadoBloqueado(cn, tx, Pago.IdPagoProveedor, out prov, out estado, out moneda, out tc);

                    ComprasFuncValidarSolicitudNoProcesada(cn, tx, Pago.IdPagoProveedor, "eliminar");

                    if (ComprasFuncEsEstadoEfectivo(estado))
                    {
                        foreach (var d in ComprasFuncObtenerDetalles(cn, tx, Pago.IdPagoProveedor))
                            ComprasFuncAplicarSaldo(cn, tx, prov, d, moneda, tc, false);
                    }

                    ComprasFuncEjecutarPorIdPago(cn, tx, ClsSentenciasCompras.PagoEliminarSolicitud, Pago.IdPagoProveedor);
                    ComprasFuncEjecutarPorIdPago(cn, tx, ClsSentenciasCompras.PagoEliminarDetalles, Pago.IdPagoProveedor);
                    ComprasFuncEjecutarPorIdPago(cn, tx, ClsSentenciasCompras.PagoEliminarEncabezado, Pago.IdPagoProveedor);

                    tx.Commit();
                }
                catch (ClsExcepcionDatosCompras) { tx.Rollback(); throw; }
                catch (OdbcException) { tx.Rollback(); throw new ClsExcepcionDatosCompras("No se pudo eliminar el pago."); }
                catch { tx.Rollback(); throw; }
            }
        }

        // ==================== CONSULTAS ====================
        public List<ClsEncabezadoPagoProveedor> ComprasMetConsultar(int? IdProveedor, string Estado, DateTime? Desde, DateTime? Hasta)
        {
            var sql = ClsSentenciasCompras.PagoConsultarBase;
            var valores = new List<object>();
            if (IdProveedor.HasValue) { sql += ClsSentenciasCompras.PagoConsultarFiltroProveedor; valores.Add(IdProveedor.Value); }
            if (!string.IsNullOrEmpty(Estado)) { sql += ClsSentenciasCompras.PagoConsultarFiltroEstado; valores.Add(Estado); }
            if (Desde.HasValue) { sql += ClsSentenciasCompras.PagoConsultarFiltroDesde; valores.Add(Desde.Value.Date); }
            if (Hasta.HasValue) { sql += ClsSentenciasCompras.PagoConsultarFiltroHasta; valores.Add(Hasta.Value.Date); }
            sql += ClsSentenciasCompras.PagoConsultarOrden;

            var lista = new List<ClsEncabezadoPagoProveedor>();
            using (var cn = ComprasFuncAbrirConexion())
            using (var cmd = ComprasFuncComando(cn, null, sql, valores.ToArray()))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read()) lista.Add(ComprasFuncLeerEncabezado(rd));
            }
            return lista;
        }

        public ClsEncabezadoPagoProveedor ComprasMetObtenerPorId(int IdPago)
        {
            ClsEncabezadoPagoProveedor pago = null;
            using (var cn = ComprasFuncAbrirConexion())
            {
                using (var cmd = ComprasFuncComando(cn, null, ClsSentenciasCompras.PagoObtenerPorId, IdPago))
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read()) pago = ComprasFuncLeerEncabezado(rd);
                }
                if (pago != null)
                    pago.Detalles = ComprasFuncObtenerDetalles(cn, null, IdPago);
            }
            return pago;
        }

        public DataTable ComprasMetObtenerProveedores()
        {
            return ComprasFuncTabla(ClsSentenciasCompras.PagoProveedores);
        }

        public DataTable ComprasMetObtenerCuentasPendientes(int IdProveedor)
        {
            return ComprasFuncTabla(ClsSentenciasCompras.PagoCuentasPendientes, IdProveedor);
        }

        public DataTable ComprasMetObtenerMetodosPago()
        {
            return ComprasFuncTabla(ClsSentenciasCompras.PagoMetodos);
        }

        public DataTable ComprasMetObtenerMonedas()
        {
            return ComprasFuncTabla(ClsSentenciasCompras.PagoMonedas);
        }

        // ==================== TIPO DE CAMBIO ====================
        public void ComprasMetResolverTipoCambio(long IdMoneda, DateTime Fecha, out long? IdTipoCambio, out decimal Valor)
        {
            IdTipoCambio = null;
            Valor = 1;

            using (var cn = ComprasFuncAbrirConexion())
            {
                long idBase;
                using (var cmd = ComprasFuncComando(cn, null, ClsSentenciasCompras.PagoIdMonedaPorCodigo, MonedaBase))
                {
                    object r = cmd.ExecuteScalar();
                    if (r == null) throw new ClsExcepcionDatosCompras("No existe la moneda base (" + MonedaBase + ") en el catálogo de monedas.");
                    idBase = Convert.ToInt64(r);
                }

                if (IdMoneda == idBase) return; // moneda base: sin referencia, valor 1

                // Directo: moneda -> base
                long? idDirecto; decimal valorDirecto;
                if (ComprasFuncBuscarTipoCambio(cn, IdMoneda, idBase, Fecha, out idDirecto, out valorDirecto))
                {
                    IdTipoCambio = idDirecto;
                    Valor = valorDirecto;
                    return;
                }

                // Inverso: base -> moneda (se usa 1/valor)
                long? idInverso; decimal valorInverso;
                if (ComprasFuncBuscarTipoCambio(cn, idBase, IdMoneda, Fecha, out idInverso, out valorInverso) && valorInverso > 0)
                {
                    IdTipoCambio = idInverso;
                    Valor = Math.Round(1m / valorInverso, 6);
                    return;
                }
            }
            throw new ClsExcepcionDatosCompras("No existe un tipo de cambio registrado para la moneda y la fecha del pago.");
        }

        private bool ComprasFuncBuscarTipoCambio(OdbcConnection cn, long origen, long destino, DateTime fecha, out long? id, out decimal valor)
        {
            id = null; valor = 0;
            using (var cmd = ComprasFuncComando(cn, null, ClsSentenciasCompras.PagoTipoCambioVigente, origen, destino, fecha.Date))
            using (var rd = cmd.ExecuteReader())
            {
                if (!rd.Read()) return false;
                id = Convert.ToInt64(rd[0]);
                valor = Convert.ToDecimal(rd[1]);
                return true;
            }
        }

        // ==================== SALDO DE LA CUENTA ====================
        // descontar = true  -> aplica el pago (resta saldo)
        // descontar = false -> revierte el pago (suma saldo)
        private void ComprasFuncAplicarSaldo(OdbcConnection cn, OdbcTransaction tx, int idProveedor,
            ClsDetallePagoProveedor d, long idMonedaPago, decimal tcPago, bool descontar)
        {
            decimal saldo, total, tcDoc;
            long idMonedaDoc;
            string estadoCuenta;

            using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoBloquearCuenta,
                d.SerieDocumento, d.NumeroDocumento, d.IdConcepto, idProveedor))
            using (var rd = cmd.ExecuteReader())
            {
                if (!rd.Read())
                    throw new ClsExcepcionDatosCompras("El documento " + d.SerieDocumento + "-" + d.NumeroDocumento + " no existe o no pertenece al proveedor.");
                saldo = Convert.ToDecimal(rd[0]);
                total = Convert.ToDecimal(rd[1]);
                estadoCuenta = Convert.ToString(rd[2]);
                idMonedaDoc = Convert.ToInt64(rd[3]);
                tcDoc = Convert.ToDecimal(rd[4]);
            }

            if (descontar && estadoCuenta == "Anulada")
                throw new ClsExcepcionDatosCompras("El documento " + d.SerieDocumento + "-" + d.NumeroDocumento + " está anulado.");

            // Monto expresado en la moneda del documento
            decimal montoDoc = (idMonedaDoc == idMonedaPago || tcDoc <= 0)
                ? d.MontoAplicado
                : Math.Round(d.MontoAplicado * tcPago / tcDoc, 4);

            decimal nuevoSaldo = descontar ? saldo - montoDoc : saldo + montoDoc;

            if (nuevoSaldo < 0 && nuevoSaldo > -0.0001m) nuevoSaldo = 0;   // tolerancia de redondeo
            if (nuevoSaldo < 0)
                throw new ClsExcepcionDatosCompras("El monto excede el saldo pendiente del documento " + d.SerieDocumento + "-" + d.NumeroDocumento + ".");
            if (nuevoSaldo > total) nuevoSaldo = total;

            string nuevoEstado = estadoCuenta == "Anulada" ? "Anulada"
                               : nuevoSaldo <= 0 ? "Pagada"
                               : nuevoSaldo >= total ? "Pendiente"
                               : "Parcial";

            using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoActualizarSaldoCuenta,
                nuevoSaldo, nuevoEstado, d.SerieDocumento, d.NumeroDocumento, d.IdConcepto))
                cmd.ExecuteNonQuery();
        }

        // ==================== AUXILIARES ====================
        private static void ComprasFuncInsertarDetalle(OdbcConnection cn, OdbcTransaction tx, int idPago, ClsDetallePagoProveedor d)
        {
            using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoInsertarDetalle,
                idPago, d.SerieDocumento, d.NumeroDocumento, d.IdConcepto, d.MontoAplicado))
                cmd.ExecuteNonQuery();
        }

        private static List<ClsDetallePagoProveedor> ComprasFuncObtenerDetalles(OdbcConnection cn, OdbcTransaction tx, int idPago)
        {
            var lista = new List<ClsDetallePagoProveedor>();
            using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoObtenerDetalles, idPago))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    lista.Add(new ClsDetallePagoProveedor
                    {
                        SerieDocumento = Convert.ToString(rd[0]),
                        NumeroDocumento = Convert.ToString(rd[1]),
                        IdConcepto = Convert.ToInt32(rd[2]),
                        NombreConcepto = Convert.ToString(rd[3]),
                        MontoAplicado = Convert.ToDecimal(rd[4])
                    });
                }
            }
            return lista;
        }

        private static void ComprasFuncLeerEncabezadoBloqueado(OdbcConnection cn, OdbcTransaction tx, int idPago,
            out int idProveedor, out string estado, out long idMoneda, out decimal tipoCambio)
        {
            using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoBloquearEncabezado, idPago))
            using (var rd = cmd.ExecuteReader())
            {
                if (!rd.Read()) throw new ClsExcepcionDatosCompras("El pago no existe.");
                idProveedor = Convert.ToInt32(rd[0]);
                estado = Convert.ToString(rd[1]);
                idMoneda = Convert.ToInt64(rd[2]);
                tipoCambio = Convert.ToDecimal(rd[3]);
            }
        }

        private static void ComprasFuncValidarSolicitudNoProcesada(OdbcConnection cn, OdbcTransaction tx, int idPago, string accion)
        {
            using (var cmd = ComprasFuncComando(cn, tx, ClsSentenciasCompras.PagoEstadoSolicitud, idPago))
            {
                object r = cmd.ExecuteScalar();
                if (r != null && Convert.ToString(r) == "Procesada")
                    throw new ClsExcepcionDatosCompras("Bancos ya procesó este pago, no se puede " + accion + ".");
            }
        }

        private static void ComprasFuncEjecutarPorIdPago(OdbcConnection cn, OdbcTransaction tx, string sql, int idPago)
        {
            using (var cmd = ComprasFuncComando(cn, tx, sql, idPago))
                cmd.ExecuteNonQuery();
        }

        private static string ComprasFuncEscalarTexto(OdbcConnection cn, OdbcTransaction tx, string sql, int id)
        {
            using (var cmd = ComprasFuncComando(cn, tx, sql, id))
            {
                object r = cmd.ExecuteScalar();
                if (r == null) throw new ClsExcepcionDatosCompras("No se encontró el registro relacionado (proveedor o método de pago).");
                return Convert.ToString(r);
            }
        }

        private static ClsEncabezadoPagoProveedor ComprasFuncLeerEncabezado(IDataRecord rd)
        {
            int oTc = rd.GetOrdinal("idTipoCambio");
            int oRef = rd.GetOrdinal("referenciaPagoProveedor");
            return new ClsEncabezadoPagoProveedor
            {
                IdPagoProveedor = Convert.ToInt32(rd["idPagoProveedor"]),
                IdProveedor = Convert.ToInt32(rd["idProveedor"]),
                NombreProveedor = Convert.ToString(rd["nombreProveedor"]),
                FechaPagoProveedor = Convert.ToDateTime(rd["fechaPagoProveedor"]),
                MontoPagoProveedor = Convert.ToDecimal(rd["montoPagoProveedor"]),
                IdMetodoPago = Convert.ToInt32(rd["idMetodoPago"]),
                NombreMetodoPago = Convert.ToString(rd["nombreMetodoPago"]),
                IdMoneda = Convert.ToInt64(rd["idMoneda"]),
                IdTipoCambio = rd.IsDBNull(oTc) ? (long?)null : Convert.ToInt64(rd.GetValue(oTc)),
                TipoCambioPagoProveedor = Convert.ToDecimal(rd["tipoCambioPagoProveedor"]),
                ReferenciaPagoProveedor = rd.IsDBNull(oRef) ? null : Convert.ToString(rd.GetValue(oRef)),
                EstadoPagoProveedor = Convert.ToString(rd["estadoPagoProveedor"])
            };
        }

        private DataTable ComprasFuncTabla(string sql, params object[] valores)
        {
            var tabla = new DataTable();
            using (var cn = ComprasFuncAbrirConexion())
            using (var cmd = ComprasFuncComando(cn, null, sql, valores))
            using (var da = new OdbcDataAdapter(cmd))
                da.Fill(tabla);
            return tabla;
        }
    }
}