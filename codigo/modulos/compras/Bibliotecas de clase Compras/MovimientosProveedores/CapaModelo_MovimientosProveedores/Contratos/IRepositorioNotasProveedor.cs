using System.Data;
using CapaModelo_MovimientosProveedores.Entidades;

namespace CapaModelo_MovimientosProveedores.Contratos
{
    /// <summary>Operaciones necesarias para ingresar y consultar notas de proveedor.</summary>
    public interface IRepositorioNotasProveedor
    {
        DataTable ComprasMetObtenerProveedores();
        DataTable ComprasMetObtenerConceptos();
        DataTable ComprasMetObtenerMonedas();
        DataTable ComprasMetConsultarNotas(int IdProveedor);
        void ComprasMetAgregar(ClsNotaProveedor Nota);
    }
}
