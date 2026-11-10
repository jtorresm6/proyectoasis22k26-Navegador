using System.Data;
using CapaModelo_Componentes.Repositorios;

namespace CapaModelo_Polizas
{
    public sealed class ClsRepositorioPolizasCompras : ClsRepositorioMaestroCompras
    {
        public DataTable ComprasFuncConsultarCuentasPorPagar()
        {
            const string Sql = @"SELECT cp.numeroFacturaCuentaPorPagar AS DocumentoOrigen,
       cp.fechaEmisionCuentaPorPagar AS Fecha,
       cp.montoTotalCuentaPorPagar AS MontoTotal,
       cp.saldoCuentaPorPagar AS Saldo,
       cp.estadoCuentaPorPagar AS Estado,
       p.nombreProveedor AS Proveedor,
       CONCAT(cp.numeroFacturaCuentaPorPagar, ' - ', p.nombreProveedor,
              ' - Q ', FORMAT(cp.saldoCuentaPorPagar, 2)) AS TextoOrigen
FROM dbsistemaembutidos.tblCuentaPorPagar cp
INNER JOIN dbsistemaembutidos.tblProveedor p ON p.idProveedor = cp.idProveedor
WHERE cp.estadoCuentaPorPagar IN ('Pendiente', 'Parcial')
ORDER BY cp.fechaEmisionCuentaPorPagar DESC, cp.idCuentaPorPagar DESC";
            return ComprasFuncEjecucionConsulta(Sql, CommandType.Text);
        }
    }
}

