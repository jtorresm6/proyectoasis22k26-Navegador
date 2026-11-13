using System.Data;
using CapaModelo_OrdenesCompra.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Establece las operaciones de consulta y persistencia de las órdenes de compra.
 * ===================================================================
*/

namespace CapaModelo_OrdenesCompra.Contratos
{
    public interface IOrdenesCompraRepositorio
    {

        DataTable ComprasFuncListar();

        DataTable ComprasFuncListar(string Estado);

        void ComprasProcCambiarEstado(int Id, string Estado);

        DataTable ComprasFuncListarDetalles(int Id);

        ClsOrdenesCompraEntidad ComprasFuncObtener(int Id);

        DataTable ComprasFuncListarSucursales();
        DataTable ComprasFuncListarBodegas();
        DataTable ComprasFuncListarProductos();
        DataTable ComprasFuncListarMonedas();
        DataTable ComprasFuncListarPlazos();
        DataTable ComprasFuncObtenerTipoCambio(long IdMoneda, System.DateTime Fecha);

        DataTable ComprasFuncListarProveedores();

        void ComprasProcAnular(int Id);

        void ComprasProcActualizarVencimiento(int Id, System.DateTime Fecha);

        int ComprasFuncCrear(int IdProveedor, int IdPlazo, int IdMoneda, System.DateTime Emision,
            System.DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario, decimal Impuesto = 0M,
            DataTable Detalles = null, long? IdTipoCambio = null);
    }
}
