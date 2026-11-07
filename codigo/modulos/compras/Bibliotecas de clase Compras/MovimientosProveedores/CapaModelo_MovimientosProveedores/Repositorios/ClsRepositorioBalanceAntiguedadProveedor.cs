using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_MovimientosProveedores.Entidades;
using CapaModelo_MovimientosProveedores.Contratos;

namespace CapaModelo_MovimientosProveedores.Repositorios
{
    public sealed class ClsRepositorioBalanceAntiguedadProveedor : RepositorioMaestro, IRepositorioBalanceAntiguedadProveedor
    {
        /// <summary>
        /// Clasifica los saldos actuales por vencimiento respecto a FechaCorte.
        /// No reconstruye saldos historicos: los pagos o anulaciones posteriores
        /// ya estan reflejados en saldoActualCuentaPorPagar y en el estado actual.
        /// Los importes conservan la moneda del documento; no deben sumarse monedas distintas.
        /// IdProveedor permite filtrar un proveedor; null incluye todos.
        /// </summary>
        public List<ClsBalanceAntiguedadProveedor> ComprasMetConsultarBalanceAntiguedad(
            DateTime FechaCorte, int? IdProveedor = null)
        {
            if (IdProveedor.HasValue && IdProveedor.Value <= 0)
                throw new ArgumentOutOfRangeException("IdProveedor", "El proveedor debe ser mayor que cero.");

            const string Sql = @"SELECT s.IdProveedor, s.NombreProveedor, s.IdMoneda,
                    SUM(CASE WHEN s.DiasVencidos <= 0 THEN s.Saldo ELSE 0 END) AS Vigente,
                    SUM(CASE WHEN s.DiasVencidos BETWEEN 1 AND 30 THEN s.Saldo ELSE 0 END) AS De1A30,
                    SUM(CASE WHEN s.DiasVencidos BETWEEN 31 AND 60 THEN s.Saldo ELSE 0 END) AS De31A60,
                    SUM(CASE WHEN s.DiasVencidos BETWEEN 61 AND 90 THEN s.Saldo ELSE 0 END) AS De61A90,
                    SUM(CASE WHEN s.DiasVencidos > 90 THEN s.Saldo ELSE 0 END) AS MasDe90,
                    SUM(s.Saldo) AS Total
                FROM (
                    SELECT p.idProveedor AS IdProveedor, p.nombreProveedor AS NombreProveedor,
                        c.idMoneda AS IdMoneda,
                        DATEDIFF(?, c.fechaVencimientoCuentaPorPagar) AS DiasVencidos,
                        CASE WHEN t.naturalezaConceptoProveedor = 'Abono'
                            THEN -c.saldoActualCuentaPorPagar
                            ELSE c.saldoActualCuentaPorPagar END AS Saldo
                    FROM tblCuentaPorPagar c
                    INNER JOIN tblProveedor p ON p.idProveedor = c.idProveedor
                    INNER JOIN tblConceptoProveedor t ON t.idConceptoProveedor = c.idConceptoProveedor
                    WHERE c.estadoCuentaPorPagar IN ('Pendiente', 'Parcial')
                        AND c.saldoActualCuentaPorPagar > 0
                        AND c.fechaEmisionCuentaPorPagar <= ?";

            var Parametros = new List<OdbcParameter> {
                new OdbcParameter("FechaAntiguedad", OdbcType.Date) { Value = FechaCorte.Date },
                new OdbcParameter("FechaEmisionLimite", OdbcType.Date) { Value = FechaCorte.Date }
            };
            string Consulta = Sql;
            if (IdProveedor.HasValue)
            {
                Consulta += " AND c.idProveedor = ?";
                Parametros.Add(new OdbcParameter("IdProveedor", OdbcType.Int) { Value = IdProveedor.Value });
            }
            Consulta += @") s GROUP BY s.IdProveedor, s.NombreProveedor, s.IdMoneda
                ORDER BY s.NombreProveedor, s.IdProveedor, s.IdMoneda";

            var Resultado = new List<ClsBalanceAntiguedadProveedor>();
            using (DataTable Tabla = EjecucionConsulta(Consulta, CommandType.Text, Parametros.ToArray()))
            {
                foreach (DataRow Fila in Tabla.Rows)
                {
                    Resultado.Add(new ClsBalanceAntiguedadProveedor {
                        FechaCorte = FechaCorte.Date,
                        IdProveedor = Convert.ToInt32(Fila["IdProveedor"]),
                        NombreProveedor = Convert.ToString(Fila["NombreProveedor"]),
                        IdMoneda = Convert.ToInt32(Fila["IdMoneda"]),
                        Vigente = Convert.ToDecimal(Fila["Vigente"]),
                        De1A30 = Convert.ToDecimal(Fila["De1A30"]),
                        De31A60 = Convert.ToDecimal(Fila["De31A60"]),
                        De61A90 = Convert.ToDecimal(Fila["De61A90"]),
                        MasDe90 = Convert.ToDecimal(Fila["MasDe90"]),
                        Total = Convert.ToDecimal(Fila["Total"])
                    });
                }
            }
            return Resultado;
        }
    }
}
