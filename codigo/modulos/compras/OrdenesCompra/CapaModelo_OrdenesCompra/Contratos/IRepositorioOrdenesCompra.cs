using System.Data;
using CapaModelo_OrdenesCompra.Entidades;

namespace CapaModelo_OrdenesCompra.Contratos
{
    public interface IRepositorioOrdenesCompra
    {

        DataTable ComprasFuncListar();

        DataTable ComprasFuncListar(string Estado);

        void ComprasProcCambiarEstado(int Id, string Estado);

        DataTable ComprasFuncListarDetalles(int Id);

        ClsOrdenCompra ComprasFuncObtener(int Id);

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
            System.DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario, decimal Impuesto = 0M);
    }
}
