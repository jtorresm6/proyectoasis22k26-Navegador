using System.Data;

namespace CapaModelo_MovimientosProveedores.Contratos
{
    public interface IRepositorioCuentaPorPagar
    {
        DataTable ComprasMetConsultar(int? IdProveedor);
    }
}
