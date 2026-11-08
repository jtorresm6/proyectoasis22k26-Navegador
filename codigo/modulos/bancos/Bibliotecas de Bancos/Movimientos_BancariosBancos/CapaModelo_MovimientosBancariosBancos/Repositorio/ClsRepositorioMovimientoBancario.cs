using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Entidades;

namespace CapaModelo_MovimientosBancarios.Repositorio
{
    public class ClsRepositorioMovimientoBancario : ClsSentencias, IRepositorioMovimientoBancario
    {

        public long BancosMetRegistrarMovimiento(
            ClsRegistroMovimientoBancario Movimiento)
        {
            if (Movimiento == null)
                throw new ArgumentNullException(nameof(Movimiento));

            if (string.IsNullOrWhiteSpace(Movimiento.ReferenciaMovimiento) || Movimiento.ReferenciaMovimiento.Length > 100)
            {
                throw new InvalidOperationException(
                    "La referencia del movimiento no es válida.");
            }

            string Naturaleza = (Movimiento.Naturaleza ?? "")
                .Trim().ToUpperInvariant();

            string Tipo = (Movimiento.TipoBeneficiario ?? "")
                .Trim().ToUpperInvariant();

            if (Naturaleza != "INGRESO" && Naturaleza != "EGRESO")
                throw new InvalidOperationException(
                    "La naturaleza del movimiento no es válida.");

            if (Tipo != "NINGUNO" && Tipo != "EMPLEADO" &&
                Tipo != "PROVEEDOR" && Tipo != "CLIENTE")
                throw new InvalidOperationException(
                    "El tipo de beneficiario no es válido.");

            if (Movimiento.IdConceptoBancario <= 0 ||
                Movimiento.IdMetodoPago <= 0 ||
                Movimiento.IdTipoTransaccion <= 0 ||
                Movimiento.IdCuentaEmpresa <= 0)
                throw new InvalidOperationException(
                    "Faltan identificadores obligatorios.");

            if (Movimiento.Importe <= 0 ||
                Movimiento.Importe >= 100000000m ||
                decimal.Round(Movimiento.Importe, 4) != Movimiento.Importe)
                throw new InvalidOperationException(
                    "El importe no es válido para DECIMAL(12,4).");

            if (Movimiento.ImporteGTQ != Movimiento.Importe ||
                Movimiento.TipoCambioAplicado != 1m)
                throw new InvalidOperationException(
                    "Esta versión únicamente permite operaciones GTQ.");

            if (Movimiento.FechaOperacion.Date > DateTime.Today ||
                Movimiento.FechaAplicacion.Date > DateTime.Today ||
                Movimiento.FechaAplicacion.Date <
                    Movimiento.FechaOperacion.Date)
                throw new InvalidOperationException(
                    "Las fechas no cumplen las reglas de esta versión.");

            bool TieneTercero = Tipo != "NINGUNO";

            if (TieneTercero &&
                (!Movimiento.IdBeneficiario.HasValue ||
                 Movimiento.IdBeneficiario.Value <= 0 ||
                 !Movimiento.IdCuentaTercero.HasValue ||
                 Movimiento.IdCuentaTercero.Value <= 0))
                throw new InvalidOperationException(
                    "Debe seleccionar un beneficiario y su cuenta.");

            if (!TieneTercero &&
                (Movimiento.IdBeneficiario.HasValue ||
                 Movimiento.IdCuentaTercero.HasValue))
                throw new InvalidOperationException(
                    "Este concepto no debe asociarse a un tercero.");

            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            {
                Conexion.Open();

                using (OdbcTransaction Transaccion =
                    Conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    // Ayuda a identificar qué consulta falla, incluso si
                    // el controlador ODBC devuelve un mensaje vacío.
                    string EtapaRegistro = "inicio de transacción";

                    try
                    {
                        // 1. Bloquear la cuenta empresarial y verificar GTQ.
                        // Este bloqueo serializa registros realizados
                        // mediante este mismo procedimiento.
                        EtapaRegistro = "1. Verificar y bloquear cuenta empresarial";
                        decimal SaldoInicial;

                        using (OdbcCommand Comando = new OdbcCommand(
                            @"SELECT c.saldoInicialCuentaBancaria
                      FROM tblCuentaBancaria c
                      INNER JOIN tblMoneda mo
                          ON mo.idMoneda = c.Fk_Moneda_Codigo
                      WHERE c.idCuentaBancaria = ?
                        AND c.is_active = 1
                        AND mo.codigoMoneda = 'GTQ'
                      FOR UPDATE",
                            Conexion, Transaccion))
                        {
                            Comando.Parameters.Add("?", OdbcType.BigInt)
                                .Value = Movimiento.IdCuentaEmpresa;

                            object Resultado = Comando.ExecuteScalar();

                            if (Resultado == null || Resultado == DBNull.Value)
                                throw new InvalidOperationException(
                                    "La cuenta empresarial no existe, " +
                                    "está inactiva o no utiliza GTQ.");

                            SaldoInicial = Convert.ToDecimal(Resultado);
                        }

                        // 2. Validar concepto activo y su clasificación.
                        EtapaRegistro = "2. Validar concepto bancario";
                        string NombreConcepto;

                        using (OdbcCommand Comando = new OdbcCommand(
                            @"SELECT nombreConceptoBancario,
                             naturalezaConceptoBancario,
                             tipoTerceroConceptoBancario,
                             claseOperacionConceptoBancario
                      FROM tblConceptoBancario
                      WHERE idConceptoBancario = ?
                        AND is_active = 1",
                            Conexion, Transaccion))
                        {
                            Comando.Parameters.Add("?", OdbcType.BigInt)
                                .Value = Movimiento.IdConceptoBancario;

                            using (OdbcDataReader Lector =
                                Comando.ExecuteReader())
                            {
                                if (!Lector.Read())
                                    throw new InvalidOperationException(
                                        "El concepto no está disponible.");

                                NombreConcepto = Convert.ToString(
                                    Lector["nombreConceptoBancario"]);

                                string NaturalezaConcepto =
                                    Convert.ToString(
                                        Lector["naturalezaConceptoBancario"]);

                                string TerceroConcepto =
                                    Convert.ToString(
                                        Lector["tipoTerceroConceptoBancario"]);

                                string Clase =
                                    Convert.ToString(
                                        Lector["claseOperacionConceptoBancario"]);

                                if (NaturalezaConcepto != Naturaleza &&
                                    NaturalezaConcepto != "AMBOS")
                                    throw new InvalidOperationException(
                                        "La naturaleza no coincide con el concepto.");

                                if (TerceroConcepto != Tipo)
                                    throw new InvalidOperationException(
                                        "El beneficiario no corresponde al concepto.");

                                if (Clase != "NORMAL")
                                    throw new InvalidOperationException(
                                        "Este registro solo admite operaciones normales.");
                            }
                        }

                        // 3. Verificar el método de pago.
                        EtapaRegistro = "3. Verificar método de pago";
                        using (OdbcCommand Comando = new OdbcCommand(
                            @"SELECT COUNT(*)
                      FROM tblMetodoPago
                      WHERE idMetodoPago = ?
                        AND is_active = 1",
                            Conexion, Transaccion))
                        {
                            Comando.Parameters.Add("?", OdbcType.Int)
                                .Value = Convert.ToInt32(Movimiento.IdMetodoPago);

                            if (Convert.ToInt32(Comando.ExecuteScalar()) != 1)
                                throw new InvalidOperationException(
                                    "El método de pago ya no está activo.");
                        }

                        // 4. Verificar tipo de transacción y naturaleza.
                        EtapaRegistro = "4. Verificar tipo de transacción";
                        using (OdbcCommand Comando = new OdbcCommand(
                            @"SELECT COUNT(*)
                      FROM tblTipoTransaccion
                      WHERE idTipoTransaccion = ?
                        AND is_active = 1
                        AND naturalezaTipoTransaccion IN (?, 'AMBOS')",
                            Conexion, Transaccion))
                        {
                            Comando.Parameters.Add("?", OdbcType.BigInt)
                                .Value = Movimiento.IdTipoTransaccion;

                            Comando.Parameters.Add("?", OdbcType.VarChar)
                                .Value = Naturaleza;

                            if (Convert.ToInt32(Comando.ExecuteScalar()) != 1)
                                throw new InvalidOperationException(
                                    "El tipo de transacción no corresponde " +
                                    "a la naturaleza del movimiento.");
                        }

                        EtapaRegistro = "5. Verificar cuenta del beneficiario";
                        // 5. Comprobar que la cuenta externa pertenece
                        // al beneficiario, cuando corresponda.
                        if (TieneTercero)
                        {
                            string ColumnaTercero =
                                Tipo == "EMPLEADO" ? "idEmpleado" :
                                Tipo == "PROVEEDOR" ? "idProveedor" :
                                "idCliente";

                            // El nombre de columna procede exclusivamente
                            // de la lista fija anterior, nunca del usuario.
                            string SqlTercero =
                                @"SELECT COUNT(*)
                          FROM tblCuentaBancariaTercero ct
                          INNER JOIN tblMoneda mo
                              ON mo.idMoneda = ct.idMoneda
                          WHERE ct.idCuentaBancariaTercero = ?
                            AND ct." + ColumnaTercero + @" = ?
                            AND ct.is_active = 1
                            AND mo.codigoMoneda = 'GTQ'";

                            using (OdbcCommand Comando = new OdbcCommand(
                                SqlTercero, Conexion, Transaccion))
                            {
                                Comando.Parameters.Add("?", OdbcType.BigInt)
                                    .Value = Movimiento.IdCuentaTercero.Value;

                                Comando.Parameters.Add("?", OdbcType.BigInt)
                                    .Value = Movimiento.IdBeneficiario.Value;

                                if (Convert.ToInt32(Comando.ExecuteScalar()) != 1)
                                    throw new InvalidOperationException(
                                        "La cuenta del tercero no corresponde " +
                                        "al beneficiario o no utiliza GTQ.");
                            }
                        }

                        // 6. Recalcular el saldo desde MySQL.
                        EtapaRegistro = "6. Recalcular saldo";
                        decimal TotalMovimientos;

                        using (OdbcCommand Comando = new OdbcCommand(
                            @"SELECT COALESCE(
                          SUM(importeMovimientoBancario *
                              sentidoMovimientoBancario), 0)
                      FROM tblMovimientoBancario
                      WHERE Fk_CuentaBancaria_Codigo = ?",
                            Conexion, Transaccion))
                        {
                            Comando.Parameters.Add("?", OdbcType.BigInt)
                                .Value = Movimiento.IdCuentaEmpresa;

                            TotalMovimientos =
                                Convert.ToDecimal(Comando.ExecuteScalar());
                        }

                        decimal Saldo = SaldoInicial + TotalMovimientos;

                        if (Naturaleza == "EGRESO" &&
                            Movimiento.Importe > Saldo)
                            throw new InvalidOperationException(
                                "Fondos insuficientes. Saldo actual: " +
                                Saldo.ToString("N2") + " GTQ.");

                        // 7. Insertar movimiento principal.
                        EtapaRegistro = "7. INSERT tblMovimientoBancario";
                        int Sentido = Naturaleza == "INGRESO" ? 1 : -1;


                        using (OdbcCommand Comando = new OdbcCommand(
                            @"INSERT INTO tblMovimientoBancario
                            (
                                referenciaMovimientoBancario,
                                conceptoMovimientoBancario,
                                importeMovimientoBancario,
                                importeGTQMovimientoBancario,
                                tipoCambioAplicadoMovimientoBancario,
                                sentidoMovimientoBancario,
                                fechaOperacionMovimientoBancario,
                                fechaValorMovimientoBancario,
                                origenMovimientoBancario,
                                Fk_CuentaBancaria_Codigo,
                                Fk_TipoTransaccion_Codigo,
                                idConceptoBancario,
                                idMetodoPago
                            )
                            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                            Conexion, Transaccion))
                        {
                            Comando.Parameters.Add("?", OdbcType.VarChar, 100)
                                .Value = Movimiento.ReferenciaMovimiento;

                            Comando.Parameters.Add("?", OdbcType.VarChar)
                                .Value = NombreConcepto;

                            // Compatibilidad ODBC: enviamos el decimal exacto
                            // como texto parametrizado con punto decimal.
                            // MySQL lo convierte a DECIMAL(12,4) al guardarlo.
                            // Evita problemas de SQLBindParameter con
                            // OdbcType.Decimal / Precision / Scale.
                            Comando.Parameters.Add("?", OdbcType.VarChar, 20)
                                .Value = Movimiento.Importe.ToString(
                                    "0.0000", CultureInfo.InvariantCulture);

                            Comando.Parameters.Add("?", OdbcType.VarChar, 20)
                                .Value = Movimiento.ImporteGTQ.ToString(
                                    "0.0000", CultureInfo.InvariantCulture);

                            Comando.Parameters.Add("?", OdbcType.VarChar, 20)
                                .Value = Movimiento.TipoCambioAplicado.ToString(
                                    "0.0000", CultureInfo.InvariantCulture);

                            Comando.Parameters.Add("?", OdbcType.Int)
                                .Value = Sentido;

                            Comando.Parameters.Add("?", OdbcType.Date)
                                .Value = Movimiento.FechaOperacion.Date;

                            Comando.Parameters.Add("?", OdbcType.Date)
                                .Value = Movimiento.FechaAplicacion.Date;

                            Comando.Parameters.Add("?", OdbcType.VarChar)
                                .Value = "MANUAL";

                            Comando.Parameters.Add("?", OdbcType.BigInt)
                                .Value = Movimiento.IdCuentaEmpresa;

                            Comando.Parameters.Add("?", OdbcType.BigInt)
                                .Value = Movimiento.IdTipoTransaccion;

                            Comando.Parameters.Add("?", OdbcType.BigInt)
                                .Value = Movimiento.IdConceptoBancario;

                            Comando.Parameters.Add("?", OdbcType.Int)
                                .Value = Convert.ToInt32(Movimiento.IdMetodoPago);

                            if (Comando.ExecuteNonQuery() != 1)
                            {
                                throw new InvalidOperationException(
                                    "No se pudo insertar el movimiento.");
                            }
                        }


                        // 8. Obtener el ID desde la misma conexión.
                        EtapaRegistro = "8. Recuperar LAST_INSERT_ID";
                        long IdMovimiento;

                        using (OdbcCommand Comando = new OdbcCommand(
                            "SELECT LAST_INSERT_ID()",
                            Conexion, Transaccion))
                        {
                            IdMovimiento =
                                Convert.ToInt64(Comando.ExecuteScalar());
                        }

                        if (IdMovimiento <= 0)
                            throw new InvalidOperationException(
                                "No se obtuvo el identificador del movimiento.");

                        // 9. Registrar tercero, si existe.
                        EtapaRegistro = "9. INSERT tblMovimientoBancarioTercero";
                        if (TieneTercero)
                        {
                            object IdEmpleado = DBNull.Value;
                            object IdProveedor = DBNull.Value;
                            object IdCliente = DBNull.Value;

                            // Estas columnas son INT en MySQL. La entidad
                            // usa long, por eso convertimos explícitamente.
                            int IdTercero = checked((int)
                                Movimiento.IdBeneficiario.Value);

                            if (Tipo == "EMPLEADO")
                                IdEmpleado = IdTercero;
                            else if (Tipo == "PROVEEDOR")
                                IdProveedor = IdTercero;
                            else
                                IdCliente = IdTercero;

                            using (OdbcCommand Comando = new OdbcCommand(
                                @"INSERT INTO tblMovimientoBancarioTercero
                          (
                              idMovimientoBancario,
                              idEmpleado,
                              idProveedor,
                              idCliente,
                              idCuentaBancariaTercero
                          )
                          VALUES (?, ?, ?, ?, ?)",
                                Conexion, Transaccion))
                            {
                                Comando.Parameters.Add("?", OdbcType.BigInt)
                                    .Value = IdMovimiento;

                                Comando.Parameters.Add("?", OdbcType.Int)
                                    .Value = IdEmpleado;

                                Comando.Parameters.Add("?", OdbcType.Int)
                                    .Value = IdProveedor;

                                Comando.Parameters.Add("?", OdbcType.Int)
                                    .Value = IdCliente;

                                Comando.Parameters.Add("?", OdbcType.BigInt)
                                    .Value = Movimiento.IdCuentaTercero.Value;

                                if (Comando.ExecuteNonQuery() != 1)
                                    throw new InvalidOperationException(
                                        "No se pudo asociar el beneficiario.");
                            }
                        }

                        // 10. Confirmar toda la operación.
                        EtapaRegistro = "10. COMMIT de la transacción";
                        Transaccion.Commit();

                        return IdMovimiento;
                    }
                    catch (Exception Ex)
                    {
                        try
                        {
                            Transaccion.Rollback();
                        }
                        catch (Exception)
                        {
                            // Conservar la excepción original incluso si
                            // el rollback no puede ejecutarse.
                            // Si falló COMMIT, comprobar por referencia
                            // antes de reintentar.
                        }

                        if (Ex is OdbcException ErrorOdbc)
                        {
                            StringBuilder DetalleOdbc = new StringBuilder();
                            foreach (OdbcError Error in ErrorOdbc.Errors)
                            {
                                DetalleOdbc.Append(" SQLState=")
                                    .Append(Error.SQLState)
                                    .Append(", NativeError=")
                                    .Append(Error.NativeError)
                                    .Append(", Mensaje=")
                                    .Append(Error.Message).Append(";");
                            }

                            throw new InvalidOperationException(
                                "Error ODBC en etapa: " + EtapaRegistro + "." +
                                (DetalleOdbc.Length > 0
                                    ? DetalleOdbc.ToString()
                                    : " El controlador no proporcionó SQLState.") +
                                " Detalle: " + ErrorOdbc.Message,
                                ErrorOdbc);
                        }

                        throw;
                    }
                }
            }
        }


        public IEnumerable<ClsRegistroMovimientoBancario>
            BancosMetObtenerTodos()
        {
            throw new NotSupportedException("La consulta de movimientos aún no está implementada.");
        }

        public int BancosMetAgregar(ClsRegistroMovimientoBancario Movimiento)
        {
            long IdGenerado = BancosMetRegistrarMovimiento(Movimiento);
            return checked((int)IdGenerado);
        }

        public int BancosMetEditar(ClsRegistroMovimientoBancario Movimiento)
        {
            throw new NotSupportedException(
                "Los movimientos bancarios confirmados no pueden " +
                "editarse directamente. Debe utilizarse una reversión.");
        }

        public int BancosMetRemover(ClsRegistroMovimientoBancario Movimiento)
        {
            throw new NotSupportedException(
                "Los movimientos bancarios no pueden eliminarse " +
                "directamente. Debe utilizarse una reversión.");
        }


        public DataTable BancosMetObtenerMovimientosTabla()
        {
            string Consulta = @"
        SELECT
            m.idMovimientoBancario AS ID,
            m.referenciaMovimientoBancario AS Referencia,
            m.conceptoMovimientoBancario AS Concepto,

            CASE
                WHEN m.sentidoMovimientoBancario = 1
                    THEN 'INGRESO'
                WHEN m.sentidoMovimientoBancario = -1
                    THEN 'EGRESO'
                ELSE 'DESCONOCIDO'
            END AS Naturaleza,

            m.importeMovimientoBancario AS Monto,
            m.fechaOperacionMovimientoBancario AS Fecha,

            c.numeroCuentaBancaria AS CuentaEmpresa,
            tt.nombreTipoTransaccion AS TipoTransaccion,

            CASE
                WHEN t.idEmpleado IS NOT NULL
                    THEN CONCAT('Empleado #', t.idEmpleado)
                WHEN t.idProveedor IS NOT NULL
                    THEN CONCAT('Proveedor #', t.idProveedor)
                WHEN t.idCliente IS NOT NULL
                    THEN CONCAT('Cliente #', t.idCliente)
                ELSE 'Sin tercero'
            END AS Beneficiario

                FROM tblMovimientoBancario m

                LEFT JOIN tblCuentaBancaria c
                    ON c.idCuentaBancaria =
                       m.Fk_CuentaBancaria_Codigo

                LEFT JOIN tblTipoTransaccion tt
                    ON tt.idTipoTransaccion =
                       m.Fk_TipoTransaccion_Codigo

                LEFT JOIN tblMovimientoBancarioTercero t
                    ON t.idMovimientoBancario =
                       m.idMovimientoBancario

                ORDER BY m.idMovimientoBancario DESC
                LIMIT 100;";

            return BancosMetEjecucionConsulta(
                Consulta,
                CommandType.Text
            );
        }


    }
}
