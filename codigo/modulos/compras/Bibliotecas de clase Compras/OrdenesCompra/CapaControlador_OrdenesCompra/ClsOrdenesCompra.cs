using System;
using System.Data;
using CapaControlador_OrdenesCompra.Servicios;
using CapaModelo_OrdenesCompra.Contratos;
using CapaControlador_OrdenesCompra.Entidades;
using CapaModelo_OrdenesCompra.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Coordina la validación, consulta y registro de las órdenes de compra.
 * ===================================================================
*/

namespace CapaControlador_OrdenesCompra
{
    public sealed class ClsOrdenesCompra
    {
        private readonly IRepositorioOrdenesCompra _Repositorio;

        public ClsOrdenesCompra() : this(new ClsRepositorioOrdenesCompra()) { }

        public ClsOrdenesCompra(IRepositorioOrdenesCompra Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public DataTable ComprasFuncListar() => _Repositorio.ComprasFuncListar();

        public DataTable ComprasFuncListar(string Estado)
        {
            if (ClsEstadosOrdenesCompra.ComprasFuncEsTodos(Estado))
                return ComprasFuncListar();
            return _Repositorio.ComprasFuncListar(ClsEstadosOrdenesCompra.ComprasFuncNormalizarEstado(Estado));
        }

        public void ComprasProcCambiarEstado(int Id, string Estado)
        {
            ClsValidacionOrdenesCompra.ComprasProcValidarId(Id);
            _Repositorio.ComprasProcCambiarEstado(Id, ClsEstadosOrdenesCompra.ComprasFuncNormalizarEstado(Estado));
        }

        public DataTable ComprasFuncListarSucursales() => _Repositorio.ComprasFuncListarSucursales();
        public DataTable ComprasFuncListarBodegas() => _Repositorio.ComprasFuncListarBodegas();
        public DataTable ComprasFuncListarProductos() => _Repositorio.ComprasFuncListarProductos();

        public DataTable ComprasFuncListarMonedas() => _Repositorio.ComprasFuncListarMonedas();
        public DataTable ComprasFuncListarPlazos() => _Repositorio.ComprasFuncListarPlazos();

        public DataTable ComprasFuncObtenerTipoCambio(long IdMoneda, DateTime Fecha)
        {
            if (IdMoneda <= 0) throw new ArgumentOutOfRangeException(nameof(IdMoneda));
            return _Repositorio.ComprasFuncObtenerTipoCambio(IdMoneda, Fecha.Date);
        }

        public DateTime ComprasFuncCalcularVencimiento(DateTime Emision, int DiasPlazo)
        {
            if (DiasPlazo < 0) throw new ArgumentOutOfRangeException(nameof(DiasPlazo));
            return Emision.Date.AddDays(DiasPlazo);
        }

        public DataTable ComprasFuncListarProveedores() => _Repositorio.ComprasFuncListarProveedores();

        public DataTable ComprasFuncListarDetalles(int Id)
        {
            ClsValidacionOrdenesCompra.ComprasProcValidarId(Id);
            return _Repositorio.ComprasFuncListarDetalles(Id);
        }

        public ClsOrdenCompra ComprasFuncAnular(int Id)
        {
            ClsValidacionOrdenesCompra.ComprasProcValidarId(Id);
            _Repositorio.ComprasProcAnular(Id);
            var Orden = ComprasFuncObtener(Id);
            if (Orden == null || Orden.Estado != "Anulada")
                throw new InvalidOperationException("No se pudo verificar la anulación. Actualiza la lista.");
            return Orden;
        }

        public void ComprasProcActualizarVencimiento(int Id, DateTime Fecha)
        {
            var Orden = ComprasFuncObtener(Id);
            ClsValidacionOrdenesCompra.ComprasProcValidarVencimiento(Orden, Fecha);
            _Repositorio.ComprasProcActualizarVencimiento(Id, Fecha.Date);
        }

        public int ComprasFuncCrear(string CodigoOrdenCompra, int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario)
        {
            ClsValidacionOrdenesCompra.ComprasFuncNormalizarCodigo(CodigoOrdenCompra);
            return ComprasFuncCrear(IdProveedor, IdPlazo, IdMoneda, Emision, Vencimiento,
                TipoCambio, Subtotal, Descuento, Comentario);
        }

        public int ComprasFuncCrear(int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario,
            decimal Impuesto = 0M)
        {
            ClsValidacionOrdenesCompra.ComprasProcValidarCreacion(IdProveedor, IdPlazo, IdMoneda,
                Emision, Vencimiento, TipoCambio, Subtotal, Descuento, Impuesto);
            Comentario = ClsValidacionOrdenesCompra.ComprasFuncNormalizarComentario(Comentario);
            return _Repositorio.ComprasFuncCrear(IdProveedor, IdPlazo, IdMoneda, Emision.Date,
                Vencimiento.Date, TipoCambio, Subtotal, Descuento, Comentario, Impuesto);
        }

        public int ComprasFuncCrearConDetalles(int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, long? IdTipoCambio, string Comentario, DataTable Detalles)
        {
            if (Detalles == null || Detalles.Rows.Count == 0)
                throw new ArgumentException("Agrega al menos un producto antes de guardar la orden.");
            if (IdTipoCambio.HasValue && IdTipoCambio.Value <= 0 || !IdTipoCambio.HasValue && TipoCambio != 1M)
                throw new ArgumentException("Selecciona un tipo de cambio válido para la moneda y fecha de emisión.");
            decimal Subtotal = 0M;
            decimal Descuento = 0M;
            foreach (DataRow Fila in Detalles.Rows)
            {
                int Producto = Convert.ToInt32(Fila["idProducto"]);
                int Bodega = Convert.ToInt32(Fila["idBodega"]);
                decimal Cantidad = Convert.ToDecimal(Fila["cantidad"]);
                decimal Precio = Convert.ToDecimal(Fila["precio"]);
                decimal Porcentaje = Convert.ToDecimal(Fila["porcentaje"]);
                if (Producto <= 0 || Bodega <= 0 || Cantidad <= 0 || Cantidad > 99999999.9999M
                    || decimal.Round(Cantidad, 4) != Cantidad || Precio < 0 || Precio > 99999999.9999M
                    || decimal.Round(Precio, 4) != Precio || Porcentaje < 0 || Porcentaje > 100
                    || decimal.Round(Porcentaje, 2) != Porcentaje)
                    throw new ArgumentException("Revisa el producto, bodega, cantidad, precio y descuento de cada línea.");
                decimal Bruto = Cantidad * Precio;
                Subtotal += Bruto;
                Descuento += Bruto - decimal.Round(Bruto * (1M - Porcentaje / 100M), 4, MidpointRounding.AwayFromZero);
            }
            ClsValidacionOrdenesCompra.ComprasProcValidarCreacion(IdProveedor, IdPlazo, IdMoneda,
                Emision, Vencimiento, TipoCambio, Subtotal, Descuento, 0M);
            Comentario = ClsValidacionOrdenesCompra.ComprasFuncNormalizarComentario(Comentario);
            return _Repositorio.ComprasFuncCrear(IdProveedor, IdPlazo, IdMoneda, Emision.Date,
                Vencimiento.Date, TipoCambio, Subtotal, Descuento, Comentario, 0M, Detalles, IdTipoCambio);
        }

        public ClsOrdenCompra ComprasFuncObtener(int Id)
        {
            ClsValidacionOrdenesCompra.ComprasProcValidarId(Id);
            var Orden = _Repositorio.ComprasFuncObtener(Id);
            return Orden == null ? null : new ClsOrdenCompra(Orden);
        }

        public DataTable ComprasFuncFiltrar(DataTable Datos, string Palabra, string Estado, DateTime? Inicio, DateTime? Fin)
        {
            return ClsFiltroOrdenesCompra.ComprasFuncFiltrar(Datos, Palabra, Estado, Inicio, Fin);
        }

    }
}
