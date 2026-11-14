using System;
using System.Data;
using CapaControlador_OrdenesCompra.Servicios;
using CapaModelo_OrdenesCompra.Contratos;
using CapaControlador_OrdenesCompra;
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
    public sealed class ClsOrdenesCompraControlador
    {
        private readonly ClsEncabezadosOrdenesCompraControlador _Encabezados;
        private readonly ClsDetallesOrdenesCompraControlador _Detalles;
        private readonly IOrdenesCompraRepositorio _Repositorio;

        public ClsOrdenesCompraControlador() : this(new ClsOrdenesCompraRepositorio()) { }

        public ClsOrdenesCompraControlador(IOrdenesCompraRepositorio Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
            _Detalles = new ClsDetallesOrdenesCompraControlador(_Repositorio);
            _Encabezados = new ClsEncabezadosOrdenesCompraControlador(_Repositorio);
        }

        public DataTable ComprasFuncListar()
        {
            return _Encabezados.ComprasFuncListar();
        }

        public DataTable ComprasFuncListar(string Estado)
        {
            return _Encabezados.ComprasFuncListar(Estado);
        }

        public void ComprasProcCambiarEstado(int Id, string Estado)
        {
            _Encabezados.ComprasProcCambiarEstado(Id, Estado);
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
            return _Detalles.ComprasFuncListarDetalles(Id);
        }

        public ClsOrdenesCompraDatos ComprasFuncAnular(int Id)
        {
            return _Detalles.ComprasFuncAnular(Id);
        }

        public void ComprasProcActualizarVencimiento(int Id, DateTime Fecha)
        {
            _Detalles.ComprasProcActualizarVencimiento(Id, Fecha);
        }

        public int ComprasFuncCrear(string CodigoOrdenCompra, int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario)
        {
            ClsOrdenesCompraValidar.ComprasFuncNormalizarCodigo(CodigoOrdenCompra);
            return ComprasFuncCrear(IdProveedor, IdPlazo, IdMoneda, Emision, Vencimiento,
                TipoCambio, Subtotal, Descuento, Comentario);
        }

        public int ComprasFuncCrear(int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario,
            decimal Impuesto = 0M)
        {
            ClsOrdenesCompraValidar.ComprasProcValidarCreacion(IdProveedor, IdPlazo, IdMoneda,
                Emision, Vencimiento, TipoCambio, Subtotal, Descuento, Impuesto);
            Comentario = ClsOrdenesCompraValidar.ComprasFuncNormalizarComentario(Comentario);
            return _Repositorio.ComprasFuncCrear(IdProveedor, IdPlazo, IdMoneda, Emision.Date,
                Vencimiento.Date, TipoCambio, Subtotal, Descuento, Comentario, Impuesto);
        }

        public static void ComprasProcValidarLimitesProducto(decimal Cantidad, decimal Precio,
            decimal DescuentoUnitario, decimal SubtotalAnterior = 0M, decimal DescuentoAnterior = 0M)
        {
            ClsDetallesOrdenesCompraValidar.ComprasProcValidarLimitesProducto(Cantidad, Precio, DescuentoUnitario, SubtotalAnterior, DescuentoAnterior);
        }

        public static decimal ComprasFuncCalcularDescuento(decimal Precio, decimal Porcentaje)
        {
            return ClsOrdenesCompraCalcular.ComprasFuncCalcularDescuento(Precio, Porcentaje);
        }

        public static decimal ComprasFuncCalcularTotalProducto(decimal Cantidad, decimal Precio, decimal Porcentaje)
        {
            return ClsOrdenesCompraCalcular.ComprasFuncCalcularTotalProducto(Cantidad, Precio, Porcentaje);
        }

        public static decimal ComprasFuncCalcularImpuesto(decimal SubtotalNeto, decimal Porcentaje)
        {
            return ClsOrdenesCompraCalcular.ComprasFuncCalcularImpuesto(SubtotalNeto, Porcentaje);
        }

        public int ComprasFuncCrearConDetalles(int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, long? IdTipoCambio, string Comentario, DataTable Detalles, decimal ImpuestoPorcentaje = 0M)
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
                    || decimal.Round(Precio, 4) != Precio || Porcentaje < 0 || Porcentaje > 99.99M
                    || decimal.Round(Porcentaje, 2) != Porcentaje)
                    throw new ArgumentException("Revisa el producto, bodega, cantidad, precio y descuento de cada línea.");
                ComprasProcValidarLimitesProducto(Cantidad, Precio, 0M, Subtotal, 0M);
                decimal Bruto = Cantidad * Precio;
                Subtotal += Bruto;
                Descuento += Bruto - ComprasFuncCalcularTotalProducto(Cantidad, Precio, Porcentaje);
            }
            decimal Impuesto = ComprasFuncCalcularImpuesto(Subtotal - Descuento, ImpuestoPorcentaje);
            ClsOrdenesCompraValidar.ComprasProcValidarCreacion(IdProveedor, IdPlazo, IdMoneda,
                Emision, Vencimiento, TipoCambio, Subtotal, Descuento, Impuesto);
            Comentario = ClsOrdenesCompraValidar.ComprasFuncNormalizarComentario(Comentario);
            return _Repositorio.ComprasFuncCrear(IdProveedor, IdPlazo, IdMoneda, Emision.Date,
                Vencimiento.Date, TipoCambio, Subtotal, Descuento, Comentario, Impuesto, Detalles, IdTipoCambio);
        }

        public ClsOrdenesCompraDatos ComprasFuncObtener(int Id)
        {
            return _Detalles.ComprasFuncObtener(Id);
        }

        public DataTable ComprasFuncFiltrar(DataTable Datos, string Palabra, string Estado, DateTime? Inicio, DateTime? Fin)
        {
            return _Encabezados.ComprasFuncFiltrar(Datos, Palabra, Estado, Inicio, Fin);
        }

    }
}
