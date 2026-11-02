using System;
using System.Data;
using System.Data.Odbc;
using CapaModelo_MovimientosProveedores.Contratos;
using CapaModelo_MovimientosProveedores.Entidades;

namespace CapaModelo_MovimientosProveedores.Repositorios
{
    public sealed class ClsRepositorioNotasProveedor : ClsSentenciasCompras, IRepositorioNotasProveedor
    {
        public DataTable ComprasMetObtenerProveedores()
        {
            return ComprasMetEjecucionConsulta("SELECT idProveedor, nombreProveedor, nitProveedor FROM tblProveedor WHERE estadoProveedor=1 ORDER BY nombreProveedor");
        }

        public DataTable ComprasMetObtenerConceptos()
        {
            return ComprasMetEjecucionConsulta(@"SELECT idConceptoProveedor, nombreConceptoProveedor, naturalezaConceptoProveedor
                FROM tblConceptoProveedor
                WHERE (LOWER(nombreConceptoProveedor) IN ('nota de debito','nota de débito') AND naturalezaConceptoProveedor='Cargo')
                   OR (LOWER(nombreConceptoProveedor) IN ('nota de credito','nota de crédito') AND naturalezaConceptoProveedor='Abono')
                ORDER BY naturalezaConceptoProveedor DESC");
        }

        public DataTable ComprasMetObtenerMonedas()
        {
            // Catálogo compartido utilizado también por Ventas.
            return ComprasMetEjecucionConsulta("SELECT Pk_Moneda AS idMoneda, CONCAT(codigoMoneda, ' - ', nombreMoneda) AS nombreMoneda FROM tblMoneda WHERE is_active=1 ORDER BY codigoMoneda");
        }

        public DataTable ComprasMetConsultarNotas(int IdProveedor)
        {
            return ComprasMetEjecucionConsulta(@"SELECT c.nombreConceptoProveedor AS Tipo, c.naturalezaConceptoProveedor AS Naturaleza,
                    p.serieDocumentoCuentaPorPagar AS Serie, p.numeroDocumentoCuentaPorPagar AS Numero,
                    p.fechaEmisionCuentaPorPagar AS Emision, p.fechaVencimientoCuentaPorPagar AS Vencimiento,
                    p.idMoneda AS Moneda, p.tipoCambioCuentaPorPagar AS TipoCambio,
                    p.montoTotalCuentaPorPagar AS Importe, p.saldoActualCuentaPorPagar AS Saldo,
                    p.estadoCuentaPorPagar AS Estado
                FROM tblCuentaPorPagar p JOIN tblConceptoProveedor c ON c.idConceptoProveedor=p.idConceptoProveedor
                WHERE p.idProveedor=? AND LOWER(c.nombreConceptoProveedor) IN
                    ('nota de debito','nota de débito','nota de credito','nota de crédito')
                ORDER BY p.fechaEmisionCuentaPorPagar DESC, p.created_at DESC",
                new OdbcParameter("proveedor", OdbcType.Int) { Value = IdProveedor });
        }

        public void ComprasMetAgregar(ClsNotaProveedor Nota)
        {
            Nota.ComprasMetValidar();
            using (var Conexion = ComprasMetObtenerConexion())
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        // Bloquear al proveedor serializa registros de notas para el mismo proveedor.
                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion,
                            "SELECT idProveedor FROM tblProveedor WHERE idProveedor=? AND estadoProveedor=1 FOR UPDATE", Nota.IdProveedor))
                            if (Comando.ExecuteScalar() == null) throw new InvalidOperationException("El proveedor ya no está activo.");

                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, @"SELECT COUNT(*) FROM tblConceptoProveedor
                            WHERE idConceptoProveedor=? AND
                            ((LOWER(nombreConceptoProveedor) IN ('nota de debito','nota de débito') AND naturalezaConceptoProveedor='Cargo')
                            OR (LOWER(nombreConceptoProveedor) IN ('nota de credito','nota de crédito') AND naturalezaConceptoProveedor='Abono'))", Nota.IdConceptoProveedor))
                            if (Convert.ToInt32(Comando.ExecuteScalar()) != 1) throw new InvalidOperationException("El concepto no corresponde a una nota de débito o crédito.");

                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion,
                            "SELECT COUNT(*) FROM tblMoneda WHERE Pk_Moneda=? AND is_active=1", Nota.IdMoneda))
                            if (Convert.ToInt32(Comando.ExecuteScalar()) != 1) throw new InvalidOperationException("La moneda ya no está activa.");

                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, @"SELECT COUNT(*) FROM tblCuentaPorPagar
                            WHERE idProveedor=? AND idConceptoProveedor=? AND serieDocumentoCuentaPorPagar=? AND numeroDocumentoCuentaPorPagar=?",
                            Nota.IdProveedor, Nota.IdConceptoProveedor, Nota.Serie, Nota.Numero))
                            if (Convert.ToInt32(Comando.ExecuteScalar()) > 0) throw new InvalidOperationException("Ya existe una nota con ese proveedor, tipo, serie y número.");

                        // El signo lo determina la naturaleza del concepto. El saldo es el importe pendiente
                        // del documento, incluso para un abono; no se aplica automáticamente a una factura.
                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, @"INSERT INTO tblCuentaPorPagar
                            (idProveedor,idConceptoProveedor,serieDocumentoCuentaPorPagar,numeroDocumentoCuentaPorPagar,
                             fechaEmisionCuentaPorPagar,fechaVencimientoCuentaPorPagar,idMoneda,tipoCambioCuentaPorPagar,
                             montoTotalCuentaPorPagar,saldoActualCuentaPorPagar,estadoCuentaPorPagar)
                            VALUES (?,?,?,?,?,?,?,?,?,?,'Pendiente')", Nota.IdProveedor, Nota.IdConceptoProveedor,
                            Nota.Serie, Nota.Numero, Nota.FechaEmision.Date, Nota.FechaVencimiento.Date,
                            Nota.IdMoneda, Nota.TipoCambio, Nota.Monto, Nota.Monto))
                            Comando.ExecuteNonQuery();
                        Transaccion.Commit();
                    }
                    catch
                    {
                        try { Transaccion.Rollback(); } catch (OdbcException) { }
                        throw;
                    }
                }
            }
        }

        private static OdbcCommand ComprasFuncCrearComando(OdbcConnection Conexion, OdbcTransaction Transaccion, string Sql, params object[] Valores)
        {
            var Comando = new OdbcCommand(Sql, Conexion, Transaccion) { CommandTimeout = 20 };
            for (int Indice = 0; Indice < Valores.Length; Indice++)
            {
                object Valor = Valores[Indice];
                var Parametro = new OdbcParameter { ParameterName = "p" + Indice, Value = Valor };
                if (Valor is int) Parametro.OdbcType = OdbcType.Int;
                else if (Valor is DateTime) Parametro.OdbcType = OdbcType.Date;
                else if (Valor is decimal)
                {
                    Parametro.OdbcType = OdbcType.Decimal;
                    Parametro.Precision = 12;
                    Parametro.Scale = (byte)(Indice == 7 ? 6 : 4);
                }
                else { Parametro.OdbcType = OdbcType.VarChar; Parametro.Size = 30; }
                Comando.Parameters.Add(Parametro);
            }
            return Comando;
        }
    }
}


