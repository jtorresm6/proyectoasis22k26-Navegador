/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Consulta cuentas, extractos, movimientos y detalles, y guarda de
 *  forma transaccional el resultado de la conciliación bancaria.
 * ================================================================
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using CapaModelo_ConciliacionBancaria.Entidades;

namespace CapaModelo_ConciliacionBancaria.Repositorio
{
    public class ClsRepositorioConciliacion : ClsConexion, IClsRepositorioConciliacion
    {
        private const string SqlCuentas =
            "SELECT cb.idCuentaBancaria AS IdCuenta, " +
            "CONCAT(b.nombreBanco, ' - ', cb.numeroCuentaBancaria, ' - ', cb.nombreCuentaBancaria, ' (', m.codigoMoneda, ')') AS Descripcion " +
            "FROM tblCuentaBancaria cb " +
            "INNER JOIN tblBanco b ON b.idBanco = cb.Fk_Banco_Codigo " +
            "INNER JOIN tblMoneda m ON m.idMoneda = cb.Fk_Moneda_Codigo " +
            "WHERE cb.is_active = 1 " +
            "ORDER BY b.nombreBanco, cb.numeroCuentaBancaria";

        private const string SqlExtractos =
            "SELECT e.Pk_ExtractoBancario AS IdExtracto, " +
            "CONCAT(e.referenciaExtractoBancario, ' (', DATE_FORMAT(e.fechaInicioExtractoBancario, '%d/%m/%Y'), " +
            "' al ', DATE_FORMAT(e.fechaFinExtractoBancario, '%d/%m/%Y'), ')') AS Descripcion " +
            "FROM tblExtractoBancario e " +
            "WHERE e.Fk_CuentaBancaria_Codigo = ? " +
            "ORDER BY e.fechaFinExtractoBancario DESC, e.Pk_ExtractoBancario DESC";

        private const string SqlExtracto =
            "SELECT e.Pk_ExtractoBancario, e.Fk_CuentaBancaria_Codigo, e.referenciaExtractoBancario, " +
            "e.fechaInicioExtractoBancario, e.fechaFinExtractoBancario, " +
            "e.saldoInicialExtractoBancario, e.saldoFinalExtractoBancario " +
            "FROM tblExtractoBancario e " +
            "WHERE e.Pk_ExtractoBancario = ?";

        private const string SqlSaldoLibro =
            "SELECT cb.saldoInicialCuentaBancaria + COALESCE(SUM(m.importeMovimientoBancario * m.sentidoMovimientoBancario), 0) " +
            "FROM tblCuentaBancaria cb " +
            "LEFT JOIN tblMovimientoBancario m ON m.Fk_CuentaBancaria_Codigo = cb.idCuentaBancaria " +
            "AND m.fechaValorMovimientoBancario >= DATE(cb.created_at) " +
            "AND m.fechaValorMovimientoBancario <= ? " +
            "WHERE cb.idCuentaBancaria = ? " +
            "GROUP BY cb.idCuentaBancaria, cb.saldoInicialCuentaBancaria";

        private const string SqlNetoExtracto =
            "SELECT COALESCE(SUM(d.importeDetalleExtracto * d.sentidoDetalleExtracto), 0) " +
            "FROM tblDetalleExtracto d " +
            "WHERE d.Fk_ExtractoBancario_Codigo = ? AND d.fechaValorDetalleExtracto <= ?";

        private const string SqlMovimientosLibro =
            "SELECT m.idMovimientoBancario, m.referenciaMovimientoBancario, m.conceptoMovimientoBancario, " +
            "m.importeMovimientoBancario, m.sentidoMovimientoBancario, " +
            "m.fechaOperacionMovimientoBancario, m.fechaValorMovimientoBancario " +
            "FROM tblMovimientoBancario m " +
            "INNER JOIN tblCuentaBancaria cb ON cb.idCuentaBancaria = m.Fk_CuentaBancaria_Codigo " +
            "WHERE m.Fk_CuentaBancaria_Codigo = ? " +
            "AND m.fechaValorMovimientoBancario >= DATE(cb.created_at) " +
            "AND m.fechaValorMovimientoBancario <= ? " +
            "AND NOT EXISTS (SELECT 1 FROM tblDetalleConciliacion d " +
            "INNER JOIN tblConciliacion c ON c.idConciliacion = d.Fk_Conciliacion_Codigo " +
            "WHERE d.Fk_MovimientoBancario_Codigo = m.idMovimientoBancario " +
            "AND d.resultadoDetalleConciliacion = 'COINCIDE' AND c.idConciliacion <> ?) " +
            "ORDER BY m.fechaValorMovimientoBancario, m.idMovimientoBancario";

        private const string SqlLineasExtracto =
            "SELECT x.idDetalleExtracto, x.identificadorDetalleExtracto, x.referenciaDetalleExtracto, " +
            "x.conceptoDetalleExtracto, x.importeDetalleExtracto, x.sentidoDetalleExtracto, " +
            "x.fechaOperacionDetalleExtracto, x.fechaValorDetalleExtracto " +
            "FROM tblDetalleExtracto x " +
            "WHERE x.Fk_ExtractoBancario_Codigo = ? AND x.fechaValorDetalleExtracto <= ? " +
            "AND NOT EXISTS (SELECT 1 FROM tblDetalleConciliacion d " +
            "INNER JOIN tblConciliacion c ON c.idConciliacion = d.Fk_Conciliacion_Codigo " +
            "WHERE d.Fk_DetalleExtracto_Codigo = x.idDetalleExtracto " +
            "AND d.resultadoDetalleConciliacion = 'COINCIDE' AND c.idConciliacion <> ?) " +
            "ORDER BY x.fechaValorDetalleExtracto, x.idDetalleExtracto";

        private const string SqlEncabezado =
            "SELECT c.idConciliacion, c.Fk_CuentaBancaria_Codigo, c.Fk_ExtractoBancario_Codigo, " +
            "c.fechaCorteConciliacion, c.estadoConciliacion, c.metodoConciliacion, " +
            "c.saldoLibroConciliacion, c.saldoBancoConciliacion, c.diferenciaConciliacion, " +
            "c.identificadorResponsableConciliacion, b.nombreBanco, cb.numeroCuentaBancaria, " +
            "cb.nombreCuentaBancaria, m.codigoMoneda, e.referenciaExtractoBancario " +
            "FROM tblConciliacion c " +
            "INNER JOIN tblCuentaBancaria cb ON cb.idCuentaBancaria = c.Fk_CuentaBancaria_Codigo " +
            "INNER JOIN tblBanco b ON b.idBanco = cb.Fk_Banco_Codigo " +
            "INNER JOIN tblMoneda m ON m.idMoneda = cb.Fk_Moneda_Codigo " +
            "INNER JOIN tblExtractoBancario e ON e.Pk_ExtractoBancario = c.Fk_ExtractoBancario_Codigo ";

        private const string SqlBuscarConciliacion =
            SqlEncabezado + "WHERE c.Fk_CuentaBancaria_Codigo = ? AND c.fechaCorteConciliacion = ?";

        private const string SqlObtenerConciliacion =
            SqlEncabezado + "WHERE c.idConciliacion = ?";

        private const string SqlDetalles =
            "SELECT d.idDetalleConciliacion, d.resultadoDetalleConciliacion, d.metodoDetalleConciliacion, " +
            "d.diferenciaDetalleConciliacion, d.motivoDetalleConciliacion, " +
            "m.idMovimientoBancario, m.referenciaMovimientoBancario, m.conceptoMovimientoBancario, " +
            "m.importeMovimientoBancario, m.sentidoMovimientoBancario, " +
            "m.fechaOperacionMovimientoBancario, m.fechaValorMovimientoBancario, " +
            "x.idDetalleExtracto, x.identificadorDetalleExtracto, x.referenciaDetalleExtracto, " +
            "x.conceptoDetalleExtracto, x.importeDetalleExtracto, x.sentidoDetalleExtracto, " +
            "x.fechaOperacionDetalleExtracto, x.fechaValorDetalleExtracto " +
            "FROM tblDetalleConciliacion d " +
            "LEFT JOIN tblMovimientoBancario m ON m.idMovimientoBancario = d.Fk_MovimientoBancario_Codigo " +
            "LEFT JOIN tblDetalleExtracto x ON x.idDetalleExtracto = d.Fk_DetalleExtracto_Codigo " +
            "WHERE d.Fk_Conciliacion_Codigo = ? " +
            "ORDER BY d.idDetalleConciliacion";

        private const string SqlBloquearConciliacion =
            "SELECT estadoConciliacion FROM tblConciliacion WHERE idConciliacion = ? FOR UPDATE";

        private const string SqlInsertarEncabezado =
            "INSERT INTO tblConciliacion (fechaCorteConciliacion, estadoConciliacion, metodoConciliacion, " +
            "saldoLibroConciliacion, saldoBancoConciliacion, diferenciaConciliacion, " +
            "identificadorResponsableConciliacion, Fk_CuentaBancaria_Codigo, Fk_ExtractoBancario_Codigo) " +
            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

        private const string SqlActualizarEncabezado =
            "UPDATE tblConciliacion SET estadoConciliacion = ?, metodoConciliacion = ?, " +
            "saldoLibroConciliacion = ?, saldoBancoConciliacion = ?, diferenciaConciliacion = ?, " +
            "identificadorResponsableConciliacion = ?, fechaCierreConciliacion = NULL " +
            "WHERE idConciliacion = ?";

        private const string SqlEliminarDetalles =
            "DELETE FROM tblDetalleConciliacion WHERE Fk_Conciliacion_Codigo = ?";

        private const string SqlInsertarDetalle =
            "INSERT INTO tblDetalleConciliacion (resultadoDetalleConciliacion, metodoDetalleConciliacion, " +
            "diferenciaDetalleConciliacion, motivoDetalleConciliacion, Fk_Conciliacion_Codigo, " +
            "Fk_MovimientoBancario_Codigo, Fk_DetalleExtracto_Codigo) " +
            "VALUES (?, ?, ?, ?, ?, ?, ?)";

        public DataTable BancosMetConsultarCuentas()
        {
            return BancosFuncConsultarTabla(SqlCuentas);
        }

        public DataTable BancosMetConsultarExtractos(long IdCuenta)
        {
            return BancosFuncConsultarTabla(SqlExtractos, BancosFuncParametro("IdCuenta", OdbcType.BigInt, IdCuenta));
        }

        public ClsDatosExtracto BancosMetObtenerExtracto(long IdExtracto)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlExtracto, Conexion))
            {
                Comando.Parameters.Add(BancosFuncParametro("IdExtracto", OdbcType.BigInt, IdExtracto));
                Conexion.Open();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                {
                    if (!Lector.Read()) return null;
                    return new ClsDatosExtracto
                    {
                        IdExtracto = Convert.ToInt64(Lector[0]),
                        IdCuenta = Convert.ToInt64(Lector[1]),
                        Referencia = Convert.ToString(Lector[2]),
                        FechaInicio = Convert.ToDateTime(Lector[3]).Date,
                        FechaFin = Convert.ToDateTime(Lector[4]).Date,
                        SaldoInicial = Convert.ToDecimal(Lector[5]),
                        SaldoFinal = Convert.ToDecimal(Lector[6])
                    };
                }
            }
        }

        public decimal BancosMetObtenerSaldoLibro(long IdCuenta, DateTime FechaCorte)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlSaldoLibro, Conexion))
            {
                Comando.Parameters.Add(BancosFuncParametro("FechaCorte", OdbcType.Date, FechaCorte.Date));
                Comando.Parameters.Add(BancosFuncParametro("IdCuenta", OdbcType.BigInt, IdCuenta));
                Conexion.Open();
                object Valor = Comando.ExecuteScalar();
                return Valor == null || Valor == DBNull.Value ? 0m : Convert.ToDecimal(Valor);
            }
        }

        public decimal BancosMetObtenerNetoExtracto(long IdExtracto, DateTime FechaCorte)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlNetoExtracto, Conexion))
            {
                Comando.Parameters.Add(BancosFuncParametro("IdExtracto", OdbcType.BigInt, IdExtracto));
                Comando.Parameters.Add(BancosFuncParametro("FechaCorte", OdbcType.Date, FechaCorte.Date));
                Conexion.Open();
                object Valor = Comando.ExecuteScalar();
                return Valor == null || Valor == DBNull.Value ? 0m : Convert.ToDecimal(Valor);
            }
        }

        public List<ClsMovimientoLibro> BancosMetConsultarMovimientosLibro(long IdCuenta, DateTime FechaCorte, long IdConciliacion)
        {
            List<ClsMovimientoLibro> Resultado = new List<ClsMovimientoLibro>();
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlMovimientosLibro, Conexion))
            {
                Comando.Parameters.Add(BancosFuncParametro("IdCuenta", OdbcType.BigInt, IdCuenta));
                Comando.Parameters.Add(BancosFuncParametro("FechaCorte", OdbcType.Date, FechaCorte.Date));
                Comando.Parameters.Add(BancosFuncParametro("IdConciliacion", OdbcType.BigInt, IdConciliacion));
                Conexion.Open();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                    while (Lector.Read())
                        Resultado.Add(BancosFuncLeerMovimiento(Lector, 0));
            }
            return Resultado;
        }

        public List<ClsLineaExtracto> BancosMetConsultarLineasExtracto(long IdExtracto, DateTime FechaCorte, long IdConciliacion)
        {
            List<ClsLineaExtracto> Resultado = new List<ClsLineaExtracto>();
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlLineasExtracto, Conexion))
            {
                Comando.Parameters.Add(BancosFuncParametro("IdExtracto", OdbcType.BigInt, IdExtracto));
                Comando.Parameters.Add(BancosFuncParametro("FechaCorte", OdbcType.Date, FechaCorte.Date));
                Comando.Parameters.Add(BancosFuncParametro("IdConciliacion", OdbcType.BigInt, IdConciliacion));
                Conexion.Open();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                    while (Lector.Read())
                        Resultado.Add(BancosFuncLeerLinea(Lector, 0));
            }
            return Resultado;
        }

        public ClsConciliacion BancosMetBuscarConciliacion(long IdCuenta, DateTime FechaCorte)
        {
            return BancosFuncConsultarEncabezado(SqlBuscarConciliacion,
                BancosFuncParametro("IdCuenta", OdbcType.BigInt, IdCuenta),
                BancosFuncParametro("FechaCorte", OdbcType.Date, FechaCorte.Date));
        }

        public ClsConciliacion BancosMetObtenerConciliacion(long IdConciliacion)
        {
            return BancosFuncConsultarEncabezado(SqlObtenerConciliacion,
                BancosFuncParametro("IdConciliacion", OdbcType.BigInt, IdConciliacion));
        }

        public List<ClsDetalleConciliacion> BancosMetConsultarDetalles(long IdConciliacion)
        {
            List<ClsDetalleConciliacion> Resultado = new List<ClsDetalleConciliacion>();
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlDetalles, Conexion))
            {
                Comando.Parameters.Add(BancosFuncParametro("IdConciliacion", OdbcType.BigInt, IdConciliacion));
                Conexion.Open();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        ClsDetalleConciliacion Detalle = new ClsDetalleConciliacion
                        {
                            IdDetalle = Convert.ToInt64(Lector[0]),
                            Resultado = Convert.ToString(Lector[1]),
                            Metodo = Convert.ToString(Lector[2]),
                            Diferencia = Convert.ToDecimal(Lector[3]),
                            Motivo = Lector.IsDBNull(4) ? null : Convert.ToString(Lector[4])
                        };
                        if (!Lector.IsDBNull(5)) Detalle.Movimiento = BancosFuncLeerMovimiento(Lector, 5);
                        if (!Lector.IsDBNull(12)) Detalle.Linea = BancosFuncLeerLinea(Lector, 12);
                        Resultado.Add(Detalle);
                    }
                }
            }
            return Resultado;
        }

        public string BancosMetGuardarConciliacion(ClsConciliacion Encabezado, List<ClsDetalleConciliacion> Detalles, out long IdConciliacion)
        {
            IdConciliacion = Encabezado.IdConciliacion;
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            {
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (Encabezado.IdConciliacion > 0)
                        {
                            string EstadoActual;
                            using (OdbcCommand Bloqueo = new OdbcCommand(SqlBloquearConciliacion, Conexion, Transaccion))
                            {
                                Bloqueo.Parameters.Add(BancosFuncParametro("IdConciliacion", OdbcType.BigInt, Encabezado.IdConciliacion));
                                object Valor = Bloqueo.ExecuteScalar();
                                EstadoActual = Valor == null || Valor == DBNull.Value ? null : Convert.ToString(Valor);
                            }

                            if (EstadoActual == null)
                                return BancosFuncRevertir(Transaccion, "La conciliación ya no existe.");
                            if (EstadoActual == ClsValoresConciliacion.EstadoCerrada)
                                return BancosFuncRevertir(Transaccion, "La conciliación de este corte ya está cerrada y no se puede modificar.");

                            using (OdbcCommand Actualizar = new OdbcCommand(SqlActualizarEncabezado, Conexion, Transaccion))
                            {
                                Actualizar.Parameters.Add(BancosFuncTexto("Estado", Encabezado.Estado, 30));
                                Actualizar.Parameters.Add(BancosFuncTexto("Metodo", Encabezado.Metodo, 30));
                                Actualizar.Parameters.Add(BancosFuncDecimal("SaldoLibro", Encabezado.SaldoLibro));
                                Actualizar.Parameters.Add(BancosFuncDecimal("SaldoBanco", Encabezado.SaldoBanco));
                                Actualizar.Parameters.Add(BancosFuncDecimal("Diferencia", Encabezado.Diferencia));
                                Actualizar.Parameters.Add(BancosFuncTexto("Responsable", Encabezado.Responsable, 100));
                                Actualizar.Parameters.Add(BancosFuncParametro("IdConciliacion", OdbcType.BigInt, Encabezado.IdConciliacion));
                                Actualizar.ExecuteNonQuery();
                            }

                            using (OdbcCommand Eliminar = new OdbcCommand(SqlEliminarDetalles, Conexion, Transaccion))
                            {
                                Eliminar.Parameters.Add(BancosFuncParametro("IdConciliacion", OdbcType.BigInt, Encabezado.IdConciliacion));
                                Eliminar.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (OdbcCommand Insertar = new OdbcCommand(SqlInsertarEncabezado, Conexion, Transaccion))
                            {
                                Insertar.Parameters.Add(BancosFuncParametro("FechaCorte", OdbcType.Date, Encabezado.FechaCorte.Date));
                                Insertar.Parameters.Add(BancosFuncTexto("Estado", Encabezado.Estado, 30));
                                Insertar.Parameters.Add(BancosFuncTexto("Metodo", Encabezado.Metodo, 30));
                                Insertar.Parameters.Add(BancosFuncDecimal("SaldoLibro", Encabezado.SaldoLibro));
                                Insertar.Parameters.Add(BancosFuncDecimal("SaldoBanco", Encabezado.SaldoBanco));
                                Insertar.Parameters.Add(BancosFuncDecimal("Diferencia", Encabezado.Diferencia));
                                Insertar.Parameters.Add(BancosFuncTexto("Responsable", Encabezado.Responsable, 100));
                                Insertar.Parameters.Add(BancosFuncParametro("IdCuenta", OdbcType.BigInt, Encabezado.IdCuenta));
                                Insertar.Parameters.Add(BancosFuncParametro("IdExtracto", OdbcType.BigInt, Encabezado.IdExtracto));
                                Insertar.ExecuteNonQuery();
                            }

                            using (OdbcCommand UltimoId = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                                IdConciliacion = Convert.ToInt64(UltimoId.ExecuteScalar());
                        }

                        foreach (ClsDetalleConciliacion Detalle in Detalles)
                        {
                            using (OdbcCommand Insertar = new OdbcCommand(SqlInsertarDetalle, Conexion, Transaccion))
                            {
                                Insertar.Parameters.Add(BancosFuncTexto("Resultado", Detalle.Resultado, 30));
                                Insertar.Parameters.Add(BancosFuncTexto("Metodo", Detalle.Metodo, 30));
                                Insertar.Parameters.Add(BancosFuncDecimal("Diferencia", Detalle.Diferencia));
                                Insertar.Parameters.Add(BancosFuncTexto("Motivo", Detalle.Motivo, 255));
                                Insertar.Parameters.Add(BancosFuncParametro("IdConciliacion", OdbcType.BigInt, IdConciliacion));
                                Insertar.Parameters.Add(BancosFuncParametro("IdMovimiento", OdbcType.BigInt,
                                    Detalle.Movimiento == null ? (object)DBNull.Value : Detalle.Movimiento.IdMovimiento));
                                Insertar.Parameters.Add(BancosFuncParametro("IdLinea", OdbcType.BigInt,
                                    Detalle.Linea == null ? (object)DBNull.Value : Detalle.Linea.IdLinea));
                                Insertar.ExecuteNonQuery();
                            }
                        }

                        Transaccion.Commit();
                        return string.Empty;
                    }
                    catch (OdbcException)
                    {
                        if (Transaccion.Connection != null) Transaccion.Rollback();
                        return "Error de base de datos al guardar la conciliación.";
                    }
                    catch (Exception)
                    {
                        if (Transaccion.Connection != null) Transaccion.Rollback();
                        return "Error inesperado al guardar la conciliación.";
                    }
                }
            }
        }

        private string BancosFuncRevertir(OdbcTransaction Transaccion, string Mensaje)
        {
            Transaccion.Rollback();
            return Mensaje;
        }

        private DataTable BancosFuncConsultarTabla(string Sql, params OdbcParameter[] Parametros)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion))
            {
                Comando.Parameters.AddRange(Parametros);
                Conexion.Open();
                DataTable Resultado = new DataTable();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                    Resultado.Load(Lector);
                return Resultado;
            }
        }

        private ClsConciliacion BancosFuncConsultarEncabezado(string Sql, params OdbcParameter[] Parametros)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion))
            {
                Comando.Parameters.AddRange(Parametros);
                Conexion.Open();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                {
                    if (!Lector.Read()) return null;
                    return new ClsConciliacion
                    {
                        IdConciliacion = Convert.ToInt64(Lector[0]),
                        IdCuenta = Convert.ToInt64(Lector[1]),
                        IdExtracto = Convert.ToInt64(Lector[2]),
                        FechaCorte = Convert.ToDateTime(Lector[3]).Date,
                        Estado = Convert.ToString(Lector[4]),
                        Metodo = Convert.ToString(Lector[5]),
                        SaldoLibro = Lector.IsDBNull(6) ? 0m : Convert.ToDecimal(Lector[6]),
                        SaldoBanco = Lector.IsDBNull(7) ? 0m : Convert.ToDecimal(Lector[7]),
                        Diferencia = Lector.IsDBNull(8) ? 0m : Convert.ToDecimal(Lector[8]),
                        Responsable = Lector.IsDBNull(9) ? string.Empty : Convert.ToString(Lector[9]),
                        Banco = Convert.ToString(Lector[10]),
                        NumeroCuenta = Convert.ToString(Lector[11]),
                        NombreCuenta = Convert.ToString(Lector[12]),
                        Moneda = Convert.ToString(Lector[13]),
                        ReferenciaExtracto = Convert.ToString(Lector[14])
                    };
                }
            }
        }

        private static ClsMovimientoLibro BancosFuncLeerMovimiento(OdbcDataReader Lector, int Inicio)
        {
            return new ClsMovimientoLibro
            {
                IdMovimiento = Convert.ToInt64(Lector[Inicio]),
                Referencia = Lector.IsDBNull(Inicio + 1) ? null : Convert.ToString(Lector[Inicio + 1]),
                Concepto = Convert.ToString(Lector[Inicio + 2]),
                Importe = Convert.ToDecimal(Lector[Inicio + 3]),
                Sentido = Convert.ToInt32(Lector[Inicio + 4]),
                FechaOperacion = Convert.ToDateTime(Lector[Inicio + 5]).Date,
                FechaValor = Convert.ToDateTime(Lector[Inicio + 6]).Date
            };
        }

        private static ClsLineaExtracto BancosFuncLeerLinea(OdbcDataReader Lector, int Inicio)
        {
            return new ClsLineaExtracto
            {
                IdLinea = Convert.ToInt64(Lector[Inicio]),
                Identificador = Convert.ToString(Lector[Inicio + 1]),
                Referencia = Lector.IsDBNull(Inicio + 2) ? null : Convert.ToString(Lector[Inicio + 2]),
                Concepto = Convert.ToString(Lector[Inicio + 3]),
                Importe = Convert.ToDecimal(Lector[Inicio + 4]),
                Sentido = Convert.ToInt32(Lector[Inicio + 5]),
                FechaOperacion = Convert.ToDateTime(Lector[Inicio + 6]).Date,
                FechaValor = Convert.ToDateTime(Lector[Inicio + 7]).Date
            };
        }

        private static OdbcParameter BancosFuncParametro(string Nombre, OdbcType Tipo, object Valor)
        {
            OdbcParameter Parametro = new OdbcParameter(Nombre, Tipo);
            Parametro.Value = Valor;
            return Parametro;
        }

        private static OdbcParameter BancosFuncTexto(string Nombre, string Valor, int Tamano)
        {
            OdbcParameter Parametro = new OdbcParameter(Nombre, OdbcType.VarChar, Tamano);
            Parametro.Value = string.IsNullOrEmpty(Valor) ? (object)DBNull.Value : Valor;
            return Parametro;
        }

        private static OdbcParameter BancosFuncDecimal(string Nombre, decimal Valor)
        {
            OdbcParameter Parametro = new OdbcParameter(Nombre, OdbcType.VarChar, 20);
            Parametro.Value = Valor.ToString("0.0000", CultureInfo.InvariantCulture);
            return Parametro;
        }
    }
}
