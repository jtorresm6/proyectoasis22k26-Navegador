using CapaModelo_AutorizacionOrdenes.Contratos;
using CapaModelo_AutorizacionOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;

/*
 * ==============================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ===============================================================================
 * Propósito :
 *  La clase Repositorio de Autorización de Orden de Compra se encarga de
 *  registrar, mediante ODBC y una transacción, la decisión de Tesorería sobre
 *  una orden de compra (autorizar o rechazar). Al autorizar guarda la evidencia
 *  de fondos y crea las reservas por cuenta que Disponibilidad Diaria toma como
 *  monto comprometido; al rechazar actualiza el estado de la orden. También
 *  consulta las autorizaciones registradas y la empresa activa de tblEmpresa.
 *  Hereda de ClsSentencias e implementa IRepositorioAutorizacionOrdenCompra.
 * ================================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Repositorios
{
    /// <summary>
    /// Registra la decisión de Tesorería y crea las reservas que
    /// Disponibilidad Diaria toma como monto comprometido.
    /// </summary>
    public class ClsRepositorioAutorizacionOrdenCompra : ClsSentencias, IRepositorioAutorizacionOrdenCompra
    {
        private const string SelectAll =
            "SELECT a.Pk_AutOrdenCompra, a.Fk_OrdenCompraRecibida_Codigo, a.identificadorEmpresaAutOrdenCompra, " +
            "a.estadoAutOrdenCompra, a.identificadorUsuarioAutorizaAutOrdenCompra, a.fechaDecisionAutOrdenCompra, " +
            "a.comentarioAutOrdenCompra, a.motivoRechazoAutOrdenCompra, a.created_at, a.updated_at, " +
            "u.nombreUsuario, COALESCE(e.nombreComercialEmpresa, e.razonSocialEmpresa) " +
            "FROM tblAutOrdenCompra a " +
            "LEFT JOIN tblUsuario u ON u.idUsuario = a.identificadorUsuarioAutorizaAutOrdenCompra " +
            "LEFT JOIN tblEmpresa e ON e.idEmpresa = a.identificadorEmpresaAutOrdenCompra";

        private const string InsertEncabezado =
            "INSERT INTO tblAutOrdenCompra (Fk_OrdenCompraRecibida_Codigo, identificadorEmpresaAutOrdenCompra, " +
            "estadoAutOrdenCompra, identificadorUsuarioAutorizaAutOrdenCompra, fechaDecisionAutOrdenCompra, " +
            "comentarioAutOrdenCompra, motivoRechazoAutOrdenCompra) VALUES (?, ?, ?, ?, NOW(), ?, ?)";

        private const string InsertEvidencia =
            "INSERT INTO tblAutDisponibilidadFondos (Fk_AutOrdenCompra_Codigo, Fk_CuentaBancaria_Codigo, " +
            "fechaCorteAutDisponibilidadFondos, fechaEvaluacionAutDisponibilidadFondos, " +
            "saldoDisponibleAutDisponibilidadFondos, Fk_Moneda_Origen_Codigo, Fk_Moneda_Destino_Codigo, " +
            "tipoCambioAutDisponibilidadFondos, Fk_TipoCambio_Codigo, fuenteAutDisponibilidadFondos) " +
            "VALUES (?, ?, ?, NOW(), ?, ?, ?, ?, ?, ?)";

        private const string InsertReserva =
            "INSERT INTO tblAutPagoOrden (Fk_AutDisponibilidadFondos_Codigo, montoAsignadoAutPagoOrden, " +
            "estadoAutPagoOrden, fechaReservaAutPagoOrden) VALUES (?, ?, 'RESERVADA', NOW())";

        private const string RechazarOrden =
            "UPDATE tblOrdenCompraRecibida SET estadoOrdenCompraRecibida = 'RECHAZADA' " +
            "WHERE Pk_OrdenCompraRecibida = ? AND UPPER(estadoOrdenCompraRecibida) = 'APROBADA'";

        private const string SelectEmpresa =
            "SELECT idEmpresa FROM tblEmpresa WHERE is_active = 1 ORDER BY idEmpresa LIMIT 1";

        private const string SelectSaldoActual =
            "SELECT (d.saldoInicialDisponibilidadDiaria + d.ingresosDisponibilidadDiaria - " +
            "d.egresosDisponibilidadDiaria - d.restringidoDisponibilidadDiaria - " +
            "COALESCE((SELECT SUM(ap.montoAsignadoAutPagoOrden / af.tipoCambioAutDisponibilidadFondos) FROM tblAutPagoOrden ap " +
            "JOIN tblAutDisponibilidadFondos af ON af.idAutDisponibilidadFondos = ap.Fk_AutDisponibilidadFondos_Codigo " +
            "WHERE af.Fk_CuentaBancaria_Codigo = d.Fk_CuentaBancaria_Codigo " +
            "AND UPPER(ap.estadoAutPagoOrden) IN ('RESERVADA','RESERVADO') " +
            "AND NOT EXISTS (SELECT 1 FROM tblMovimientoBancario mb " +
            "WHERE mb.Fk_OrdenPago_Codigo = ap.Fk_OrdenPago_Codigo)), 0) - " +
            "COALESCE((SELECT SUM(op.importeOrdenPago) FROM tblOrdenPago op " +
            "WHERE op.Fk_CuentaBancaria_Codigo = d.Fk_CuentaBancaria_Codigo " +
            "AND UPPER(op.estadoOrdenPago) IN ('AUTORIZADA','EMITIDA') " +
            "AND NOT EXISTS (SELECT 1 FROM tblMovimientoBancario mb2 WHERE mb2.Fk_OrdenPago_Codigo = op.idOrdenPago) " +
            "AND NOT EXISTS (SELECT 1 FROM tblAutPagoOrden ap2 WHERE ap2.Fk_OrdenPago_Codigo = op.idOrdenPago " +
            "AND UPPER(ap2.estadoAutPagoOrden) IN ('RESERVADA','RESERVADO'))), 0)) " +
            "FROM tblDisponibilidadDiaria d WHERE d.Fk_CuentaBancaria_Codigo = ? " +
            "AND d.fechaDisponibilidadDiaria = CURDATE() FOR UPDATE";

        public int BancosMetAgregar(ClsAutorizacionOrdenCompra entidad)
        {
            bool autoriza = string.Equals(entidad.EstadoAutorizacionOrden, "AUTORIZADA", StringComparison.OrdinalIgnoreCase);
            if (!entidad.IdUsuarioAutoriza.HasValue || entidad.IdUsuarioAutoriza.Value <= 0)
                throw new InvalidOperationException("Debe existir una sesión de usuario válida para registrar la decisión.");
            if (autoriza && (entidad.Evidencias == null || entidad.Evidencias.Count == 0))
                throw new InvalidOperationException("Debe asignar al menos una cuenta bancaria.");

            using (OdbcConnection conexion = BancosMetObtenerConexion())
            {
                conexion.Open();
                using (OdbcTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        Ejecutar(conexion, transaccion, InsertEncabezado,
                            entidad.IdOrdenCompra, entidad.IdEmpresaAutorizacionOrden,
                            autoriza ? "AUTORIZADA" : "RECHAZADA", entidad.IdUsuarioAutoriza.Value,
                            Db(entidad.ComentarioAutorizacionOrden), Db(entidad.MotivoRechazoAutorizacionOrden));
                        long idAutorizacion = UltimoId(conexion, transaccion);

                        if (autoriza)
                        {
                            foreach (ClsEvidenciaFondos evidencia in entidad.Evidencias)
                            {
                                ClsAutorizacionDetalle detalle = entidad.Detalles.FirstOrDefault(d => d.IdCuenta == evidencia.IdCuenta);
                                if (detalle == null || detalle.MontoAsignadoPagoOrden <= 0)
                                    throw new InvalidOperationException("La asignación de la cuenta no es válida.");

                                decimal saldoActual = SaldoActual(conexion, transaccion, evidencia.IdCuenta);
                                if (detalle.MontoAsignadoPagoOrden > saldoActual * evidencia.TipoCambioDisponibilidadFondos)
                                    throw new InvalidOperationException("La cuenta " + evidencia.IdCuenta +
                                    " ya no tiene saldo suficiente. Disponible: " + saldoActual.ToString("N2") + ".");

                                Ejecutar(conexion, transaccion, InsertEvidencia,
                                    idAutorizacion, evidencia.IdCuenta, DateTime.Today, saldoActual,
                                    evidencia.IdMonedaOrigen, evidencia.IdMonedaDestino,
                                    evidencia.TipoCambioDisponibilidadFondos, Db(evidencia.IdTipoCambio),
                                    "DISPONIBILIDAD_DIARIA_AUTOMATICA");
                                    long idEvidencia = UltimoId(conexion, transaccion);
                                    Ejecutar(conexion, transaccion, InsertReserva,
                                        idEvidencia, detalle.MontoAsignadoPagoOrden);
                            }
                        }
                        else if (Ejecutar(conexion, transaccion, RechazarOrden, entidad.IdOrdenCompra) != 1)
                        {
                            throw new InvalidOperationException("La orden ya no se encuentra disponible para rechazo.");
                        }

                        transaccion.Commit();
                        return Convert.ToInt32(idAutorizacion);
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public int BancosMetObtenerIdEmpresa()
        {
            DataTable tabla = BancosMetEjecucionConsulta(SelectEmpresa, CommandType.Text);
            if (tabla.Rows.Count == 0 || tabla.Rows[0][0] == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToInt32(tabla.Rows[0][0]);
        }

        public int BancosMetEditar(ClsAutorizacionOrdenCompra entidad)
        {
            throw new NotSupportedException("Una autorización no se edita.");
        }

        public int BancosMetRemover(ClsAutorizacionOrdenCompra entidad)
        {
            throw new NotSupportedException("Una autorización no se elimina.");
        }

        public IEnumerable<ClsAutorizacionOrdenCompra> BancosMetObtenerTodos()
        {
            var lista = new List<ClsAutorizacionOrdenCompra>();
            DataTable tabla = BancosMetEjecucionConsulta(SelectAll, CommandType.Text);
            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new ClsAutorizacionOrdenCompra
                {
                    IdAutorizacionOrden = Convert.ToInt64(row[0]),
                    IdOrdenCompra = Convert.ToInt64(row[1]),
                    IdEmpresaAutorizacionOrden = Convert.ToInt32(row[2]),
                    EstadoAutorizacionOrden = Convert.ToString(row[3]),
                    IdUsuarioAutoriza = row[4] == DBNull.Value ? (int?)null : Convert.ToInt32(row[4]),
                    FechaDecisionAutorizacionOrden = row[5] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row[5]),
                    ComentarioAutorizacionOrden = Convert.ToString(row[6]),
                    MotivoRechazoAutorizacionOrden = Convert.ToString(row[7]),
                    CreatedAt = Convert.ToDateTime(row[8]),
                    UpdatedAt = Convert.ToDateTime(row[9]),
                    NombreUsuarioAutoriza = Convert.ToString(row[10]),
                    NombreEmpresaAutorizacionOrden = Convert.ToString(row[11])
                });
            }
            return lista;
        }

        private static decimal SaldoActual(OdbcConnection conexion, OdbcTransaction transaccion, long idCuenta)
        {
            using (OdbcCommand comando = new OdbcCommand(SelectSaldoActual, conexion, transaccion))
            {
                comando.Parameters.Add(new OdbcParameter { OdbcType = OdbcType.BigInt, Value = idCuenta });
                object valor = comando.ExecuteScalar();
                if (valor == null || valor == DBNull.Value)
                    throw new InvalidOperationException("Primero consulte Disponibilidad Diaria para generar el corte de hoy.");
                return Convert.ToDecimal(valor);
            }
        }

        private static int Ejecutar(OdbcConnection conexion, OdbcTransaction transaccion, string sql, params object[] valores)
        {
            using (OdbcCommand comando = new OdbcCommand(sql, conexion, transaccion))
            {
                foreach (object valor in valores)
                    comando.Parameters.Add(new OdbcParameter { Value = valor ?? DBNull.Value });
                return comando.ExecuteNonQuery();
            }
        }

        private static long UltimoId(OdbcConnection conexion, OdbcTransaction transaccion)
        {
            using (OdbcCommand comando = new OdbcCommand("SELECT LAST_INSERT_ID()", conexion, transaccion))
                return Convert.ToInt64(comando.ExecuteScalar());
        }

        private static object Db(object valor)
        {
            if (valor == null) return DBNull.Value;
            string texto = valor as string;
            return texto != null && string.IsNullOrWhiteSpace(texto) ? (object)DBNull.Value : valor;
        }
    }
}
