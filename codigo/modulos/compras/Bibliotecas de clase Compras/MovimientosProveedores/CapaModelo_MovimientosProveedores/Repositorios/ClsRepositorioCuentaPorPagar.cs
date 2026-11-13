using System;
using System.Data;
using System.Data.Odbc;
using CapaModelo_MovimientosProveedores.Contratos;

namespace CapaModelo_MovimientosProveedores.Repositorios
{
    /// <summary>Saldos pendientes actuales, en la moneda original de cada documento.</summary>
    public sealed class ClsRepositorioCuentaPorPagar : RepositorioMaestro, IRepositorioCuentaPorPagar
    {
        public DataTable ComprasMetConsultar(int? IdProveedor)
        {
            if (IdProveedor.HasValue && IdProveedor.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(IdProveedor));

            const string Consulta = @"SELECT p.idProveedor AS IdProveedor,
                    p.nombreProveedor AS Proveedor, p.nitProveedor AS Nit,
                    c.idMoneda AS IdMoneda, m.codigoMoneda AS Moneda,
                    t.nombreConceptoProveedor AS Concepto,
                    t.naturalezaConceptoProveedor AS Naturaleza,
                    CONCAT(c.serieDocumentoCuentaPorPagar, '-', c.numeroDocumentoCuentaPorPagar) AS Documento,
                    c.fechaEmisionCuentaPorPagar AS Emision,
                    c.fechaVencimientoCuentaPorPagar AS Vencimiento,
                    c.montoTotalCuentaPorPagar AS Importe,
                    CASE WHEN t.naturalezaConceptoProveedor = 'Abono'
                        THEN -c.saldoActualCuentaPorPagar ELSE c.saldoActualCuentaPorPagar END AS Saldo,
                    c.estadoCuentaPorPagar AS Estado
                FROM tblCuentaPorPagar c
                INNER JOIN tblProveedor p ON p.idProveedor = c.idProveedor
                INNER JOIN tblConceptoProveedor t ON t.idConceptoProveedor = c.idConceptoProveedor
                INNER JOIN tblMoneda m ON m.idMoneda = c.idMoneda
                WHERE c.estadoCuentaPorPagar IN ('Pendiente', 'Parcial')
                    AND c.saldoActualCuentaPorPagar > 0";
            const string Orden = " ORDER BY m.codigoMoneda, c.idMoneda, p.nombreProveedor, p.idProveedor, c.fechaVencimientoCuentaPorPagar, c.fechaEmisionCuentaPorPagar, Documento";
            // No se excluyen proveedores ni monedas inactivos: pueden conservar deuda pendiente.
            return IdProveedor.HasValue
                ? EjecucionConsulta(Consulta + " AND c.idProveedor = ?" + Orden, CommandType.Text,
                    new OdbcParameter("IdProveedor", OdbcType.Int) { Value = IdProveedor.Value })
                : EjecucionConsulta(Consulta + Orden, CommandType.Text);
        }
    }
}
