using System.Data;
using CapaModelo_OrdenesCompra.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Define las operaciones de consulta, creación, anulación y actualización
 *  de vencimiento que debe implementar el repositorio de compras.
 * ===================================================================
*/

namespace CapaModelo_OrdenesCompra.Contratos
{
    public interface IRepositorioComprasPorPagar
    {
        DataTable ComprasFuncListar();
        ClsCompraPorPagar ComprasFuncObtener(int Id);
        DataTable ComprasFuncListarProveedores();
        void ComprasProcAnular(int Id);
        void ComprasProcActualizarVencimiento(int Id, System.DateTime Fecha);
        int ComprasFuncCrear(int IdProveedor, string Factura, System.DateTime Emision, System.DateTime Vencimiento, decimal Monto);
    }
}
