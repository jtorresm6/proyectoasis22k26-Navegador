using System;
using System.Data;
using System.Data.Odbc;
using CapaModelo_MovimientosProveedores.Contratos;
using CapaModelo_MovimientosProveedores.Entidades;

namespace CapaModelo_MovimientosProveedores.Repositorios
{
    public sealed class ClsRepositorioNotasProveedor : RepositorioMaestro, IRepositorioNotasProveedor
    {
        private const string ConsultaMonedas = @"SELECT m.idMoneda, CONCAT(m.codigoMoneda, ' - ', m.nombreMoneda) AS nombreMoneda
            FROM tblMoneda m WHERE {0}=1 ORDER BY m.codigoMoneda";

        private const string ConsultaTipoCambio = @"SELECT t.idTipoCambio,
                CASE WHEN m.codigoMoneda='GTQ' THEN 1 ELSE t.valorTipoCambio END AS valorTipoCambio
            FROM tblMoneda m
            LEFT JOIN tblTipoCambio t ON t.idTipoCambio = (
                SELECT c.idTipoCambio
                FROM tblTipoCambio c JOIN tblMoneda d ON d.idMoneda=c.Fk_Moneda_Destino_Codigo
                WHERE c.Fk_Moneda_Origen_Codigo=m.idMoneda AND d.codigoMoneda='GTQ' AND {1}=1
                  AND c.fechaTipoCambio<=?
                ORDER BY c.fechaTipoCambio DESC, c.versionTipoCambio DESC, c.idTipoCambio DESC
                LIMIT 1)
            WHERE m.idMoneda=? AND {0}=1";

        public DataTable ComprasMetObtenerProveedores()
        {
            return EjecucionConsulta("SELECT idProveedor, nombreProveedor, nitProveedor FROM tblProveedor WHERE estadoProveedor=1 ORDER BY nombreProveedor", CommandType.Text);
        }

        public DataTable ComprasMetObtenerConceptos()
        {
            return EjecucionConsulta(@"SELECT idConceptoProveedor, nombreConceptoProveedor, naturalezaConceptoProveedor
                FROM tblConceptoProveedor
                ORDER BY nombreConceptoProveedor", CommandType.Text);
        }

        public DataTable ComprasMetObtenerMonedas()
        {
            using (var Conexion = ObtenerConexion())
            {
                Conexion.Open();
                using (var Comando = ComprasFuncCrearComandoMonedas(Conexion, null, ConsultaMonedas))
                using (var Lector = Comando.ExecuteReader())
                {
                    var Monedas = new DataTable();
                    Monedas.Load(Lector);
                    return Monedas;
                }
            }
        }

        public decimal? ComprasMetObtenerTipoCambio(long IdMoneda, DateTime FechaEmision)
        {
            using (var Conexion = ObtenerConexion())
            {
                Conexion.Open();
                using (var Comando = ComprasFuncCrearComandoMonedas(Conexion, null, ConsultaTipoCambio, FechaEmision.Date, IdMoneda))
                using (var Lector = Comando.ExecuteReader())
                {
                    if (!Lector.Read() || Lector.IsDBNull(1)) return null;
                    return Convert.ToDecimal(Lector.GetValue(1));
                }
            }
        }

        public DataTable ComprasMetConsultarNotas(int IdProveedor)
        {
            return EjecucionConsulta(@"SELECT c.nombreConceptoProveedor AS Tipo, c.naturalezaConceptoProveedor AS Naturaleza,
                    p.serieDocumentoCuentaPorPagar AS Serie, p.numeroDocumentoCuentaPorPagar AS Numero,
                    p.fechaEmisionCuentaPorPagar AS Emision, p.fechaVencimientoCuentaPorPagar AS Vencimiento,
                    p.idMoneda AS Moneda, p.tipoCambioCuentaPorPagar AS TipoCambio,
                    p.montoTotalCuentaPorPagar AS Importe, p.saldoActualCuentaPorPagar AS Saldo,
                    p.estadoCuentaPorPagar AS Estado
                FROM tblCuentaPorPagar p JOIN tblConceptoProveedor c ON c.idConceptoProveedor=p.idConceptoProveedor
                WHERE p.idProveedor=?
                ORDER BY p.fechaEmisionCuentaPorPagar DESC, p.created_at DESC", CommandType.Text,
                new OdbcParameter("proveedor", OdbcType.Int) { Value = IdProveedor });
        }

        public void ComprasMetAgregar(ClsNotaProveedor Nota)
        {
            if (Nota == null) throw new ArgumentNullException("Nota");
            using (var Conexion = ObtenerConexion())
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        // Bloquear al proveedor serializa registros de documentos para el mismo proveedor.
                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion,
                            "SELECT idProveedor FROM tblProveedor WHERE idProveedor=? AND estadoProveedor=1 FOR UPDATE", Nota.IdProveedor))
                            if (Comando.ExecuteScalar() == null) throw new InvalidOperationException("El proveedor ya no está activo.");

                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, @"SELECT COUNT(*) FROM tblConceptoProveedor
                            WHERE idConceptoProveedor=?", Nota.IdConceptoProveedor))
                            if (Convert.ToInt32(Comando.ExecuteScalar()) != 1) throw new InvalidOperationException("El concepto de proveedor seleccionado ya no existe.");

                        object IdTipoCambio;
                        decimal TipoCambio;
                        using (var Comando = ComprasFuncCrearComandoMonedas(Conexion, Transaccion, ConsultaTipoCambio,
                            Nota.FechaEmision.Date, Nota.IdMoneda))
                        using (var Lector = Comando.ExecuteReader())
                        {
                            if (!Lector.Read()) throw new InvalidOperationException("La moneda seleccionada no existe o está inactiva.");
                            if (Lector.IsDBNull(1)) throw new InvalidOperationException("No hay un tipo de cambio hacia GTQ para la fecha de emisión.");
                            IdTipoCambio = Lector.IsDBNull(0) ? (object)DBNull.Value : Convert.ToInt64(Lector.GetValue(0));
                            TipoCambio = Convert.ToDecimal(Lector.GetValue(1));
                        }
                        if (TipoCambio != Nota.TipoCambio)
                            throw new InvalidOperationException("El tipo de cambio cambió. Recargue la cotización y revise el equivalente antes de guardar.");

                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, @"SELECT COUNT(*) FROM tblCuentaPorPagar
                            WHERE idProveedor=? AND idConceptoProveedor=? AND serieDocumentoCuentaPorPagar=? AND numeroDocumentoCuentaPorPagar=?",
                            Nota.IdProveedor, Nota.IdConceptoProveedor, Nota.Serie, Nota.Numero))
                            if (Convert.ToInt32(Comando.ExecuteScalar()) > 0) throw new InvalidOperationException("Ya existe un documento con ese proveedor, concepto, serie y número.");

                        // El signo lo determina la naturaleza del concepto. El saldo es el importe pendiente
                        // del documento, incluso para un abono; no se aplica automáticamente a una factura.
                        using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, @"INSERT INTO tblCuentaPorPagar
                            (idProveedor,idConceptoProveedor,serieDocumentoCuentaPorPagar,numeroDocumentoCuentaPorPagar,
                             fechaEmisionCuentaPorPagar,fechaVencimientoCuentaPorPagar,idMoneda,idTipoCambio,tipoCambioCuentaPorPagar,
                             montoTotalCuentaPorPagar,saldoActualCuentaPorPagar,estadoCuentaPorPagar)
                            VALUES (?,?,?,?,?,?,?,?,CAST(? AS DECIMAL(12,6)),CAST(? AS DECIMAL(12,4)),CAST(? AS DECIMAL(12,4)),'Pendiente')", Nota.IdProveedor, Nota.IdConceptoProveedor,
                            Nota.Serie, Nota.Numero, Nota.FechaEmision.Date, Nota.FechaVencimiento.Date,
                            Nota.IdMoneda, IdTipoCambio, TipoCambio, Nota.Monto, Nota.Monto))
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

        private static OdbcCommand ComprasFuncCrearComandoMonedas(OdbcConnection Conexion, OdbcTransaction Transaccion,
            string Consulta, params object[] Valores)
        {
            // Algunas instalaciones comparten el catálogo con Ventas y usan is_active.
            // Si el catálogo no define un estado, todas sus monedas están disponibles.
            string CampoActivo = null;
            using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion,
                "SHOW COLUMNS FROM tblMoneda WHERE Field IN ('activoMoneda','is_active')"))
            using (var Lector = Comando.ExecuteReader())
            {
                while (Lector.Read())
                {
                    if (string.Equals(Lector.GetString(0), "activoMoneda", StringComparison.OrdinalIgnoreCase))
                    {
                        CampoActivo = "activoMoneda";
                        break;
                    }
                    CampoActivo = "is_active";
                }
            }

            string Sql = string.Format(Consulta,
                CampoActivo == null ? "1" : "m." + CampoActivo,
                CampoActivo == null ? "1" : "d." + CampoActivo);
            return ComprasFuncCrearComando(Conexion, Transaccion, Sql, Valores);
        }

        private static OdbcCommand ComprasFuncCrearComando(OdbcConnection Conexion, OdbcTransaction Transaccion, string Sql, params object[] Valores)
        {
            var Comando = new OdbcCommand(Sql, Conexion, Transaccion) { CommandTimeout = 20 };
            for (int Indice = 0; Indice < Valores.Length; Indice++)
            {
                object Valor = Valores[Indice];
                var Parametro = new OdbcParameter { ParameterName = "p" + Indice, Value = Valor };
                if (Valor is int) Parametro.OdbcType = OdbcType.Int;
                else if (Valor is long || Valor == DBNull.Value) Parametro.OdbcType = OdbcType.BigInt;
                else if (Valor is DateTime) Parametro.OdbcType = OdbcType.Date;
                else if (Valor is decimal)
                {
                    // Algunos controladores ODBC fallan al enlazar el decimal de seis
                    // posiciones del tipo de cambio y no devuelven detalle del error.
                    // Se envia como parametro de texto con punto decimal invariable;
                    // los CAST del INSERT recuperan el DECIMAL exacto, sin usar double
                    // ni concatenar el valor con SQL ni depender de la configuracion regional.
                    Parametro.OdbcType = OdbcType.VarChar;
                    Parametro.Size = 32;
                    Parametro.Value = ((decimal)Valor).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else { Parametro.OdbcType = OdbcType.VarChar; Parametro.Size = 30; }
                Comando.Parameters.Add(Parametro);
            }
            return Comando;
        }
    }
}


