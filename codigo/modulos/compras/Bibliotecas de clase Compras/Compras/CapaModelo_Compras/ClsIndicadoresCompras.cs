using System.Data;
using CapaModelo_Componentes.Repositorios;

namespace CapaModelo_Compras
{
    public sealed class ClsIndicadoresCompras : ClsRepositorioMaestroCompras
    {
        public DataTable ComprasFuncConsultar()
        {
            return ComprasFuncEjecucionConsulta(@"
SELECT
    COALESCE(AVG(GREATEST(DATEDIFF(CURRENT_DATE(), c.fechaEmisionCuentaPorPagar), 0)), 0) AS AntiguedadPromedio,
    (SELECT COALESCE(100.0 * SUM(CASE WHEN o.estadoOrdenCompra IN ('Autorizada', 'Recibida') THEN 1 ELSE 0 END)
        / NULLIF(COUNT(*), 0), 0)
     FROM tblEncabezadoOrdenCompra o
     WHERE o.estadoOrdenCompra <> 'Anulada' AND o.fechaEmisionOrdenCompra <= CURRENT_DATE()) AS PorcentajeAutorizadas,
    (SELECT COUNT(*) FROM tblEncabezadoOrdenCompra o
     WHERE o.estadoOrdenCompra = 'Autorizada' AND o.fechaEmisionOrdenCompra <= CURRENT_DATE()) AS PendientesRecepcion,
    COALESCE(SUM(c.saldoActualCuentaPorPagar * c.tipoCambioCuentaPorPagar), 0) AS SaldoPorPagar,
    COALESCE(SUM(CASE WHEN c.fechaVencimientoCuentaPorPagar < CURRENT_DATE()
        THEN c.saldoActualCuentaPorPagar * c.tipoCambioCuentaPorPagar ELSE 0 END), 0) AS SaldoVencido,
    COALESCE(SUM(CASE WHEN c.fechaVencimientoCuentaPorPagar BETWEEN CURRENT_DATE() AND DATE_ADD(CURRENT_DATE(), INTERVAL 7 DAY)
        THEN c.saldoActualCuentaPorPagar * c.tipoCambioCuentaPorPagar ELSE 0 END), 0) AS ProximosVencimientos
FROM tblCuentaPorPagar c
INNER JOIN tblConceptoProveedor concepto ON concepto.idConceptoProveedor = c.idConceptoProveedor
WHERE c.estadoCuentaPorPagar IN ('Pendiente', 'Parcial')
    AND concepto.naturalezaConceptoProveedor = 'Cargo'
    AND c.saldoActualCuentaPorPagar > 0
    AND c.fechaEmisionCuentaPorPagar <= CURRENT_DATE()", CommandType.Text);
        }
    }
}
