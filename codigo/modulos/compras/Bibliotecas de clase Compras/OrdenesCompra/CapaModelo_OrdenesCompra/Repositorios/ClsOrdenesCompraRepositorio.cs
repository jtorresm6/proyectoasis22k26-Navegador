using System;
using System.Data;
using CapaModelo_OrdenesCompra.Contratos;
using CapaModelo_OrdenesCompra.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Integra las consultas y los comandos de persistencia de las órdenes de compra.
 * ===================================================================
*/

namespace CapaModelo_OrdenesCompra.Repositorios
{
    public sealed class ClsOrdenesCompraRepositorio : IOrdenesCompraRepositorio
    {
        private readonly ClsEncabezadosOrdenesCompraConsultar _Encabezados = new ClsEncabezadosOrdenesCompraConsultar();
        private readonly ClsDetallesOrdenesCompraConsultar _Detalles = new ClsDetallesOrdenesCompraConsultar();
        private readonly ClsOrdenesCompraConsultarCatalogos _Catalogos = new ClsOrdenesCompraConsultarCatalogos();
        private readonly ClsOrdenesCompraComandos _Comandos = new ClsOrdenesCompraComandos();

        public DataTable ComprasFuncListar() => _Encabezados.ComprasFuncListar();

        public DataTable ComprasFuncListar(string Estado) => _Encabezados.ComprasFuncListar(Estado);

        public ClsOrdenesCompraEntidad ComprasFuncObtener(int Id) => _Encabezados.ComprasFuncObtener(Id);

        public DataTable ComprasFuncListarDetalles(int Id) => _Detalles.ComprasFuncListarDetalles(Id);

        public DataTable ComprasFuncListarSucursales() => _Catalogos.ComprasFuncListarSucursales();
        public DataTable ComprasFuncListarBodegas() => _Catalogos.ComprasFuncListarBodegas();
        public DataTable ComprasFuncListarProductos() => _Catalogos.ComprasFuncListarProductos();

        public DataTable ComprasFuncListarMonedas() => _Catalogos.ComprasFuncListarMonedas();
        public DataTable ComprasFuncListarPlazos() => _Catalogos.ComprasFuncListarPlazos();
        public DataTable ComprasFuncObtenerTipoCambio(long IdMoneda, DateTime Fecha) => _Catalogos.ComprasFuncObtenerTipoCambio(IdMoneda, Fecha);

        public DataTable ComprasFuncListarProveedores() => _Catalogos.ComprasFuncListarProveedores();

        public void ComprasProcAnular(int Id) => _Comandos.ComprasProcAnular(Id);

        public void ComprasProcCambiarEstado(int Id, string Estado) => _Comandos.ComprasProcCambiarEstado(Id, Estado);

        public void ComprasProcActualizarVencimiento(int Id, DateTime Fecha) => _Comandos.ComprasProcActualizarVencimiento(Id, Fecha);

        public int ComprasFuncCrear(int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario, decimal Impuesto = 0M,
            DataTable Detalles = null, long? IdTipoCambio = null)
        {
            return _Comandos.ComprasFuncCrear(IdProveedor, IdPlazo, IdMoneda,
                Emision, Vencimiento, TipoCambio, Subtotal, Descuento, Comentario, Impuesto, Detalles, IdTipoCambio);
        }
    }
}
