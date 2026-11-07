using System;
using CapaControlador_OrdenesCompra.Entidades;

namespace CapaControlador_OrdenesCompra.Servicios
{
    internal static class ClsValidacionOrdenesCompra
    {

        internal static void ComprasProcValidarId(int Id)
        {
            if (Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(Id));
        }

        private static void ComprasProcValidarDecimal(decimal Valor, decimal Maximo, int Decimales, string Campo)
        {
            if (Valor < 0 || Valor > Maximo || decimal.Round(Valor, Decimales) != Valor)
                throw new ArgumentOutOfRangeException(Campo, "El valor excede el rango o la precisión permitidos.");
        }

        internal static string ComprasFuncNormalizarCodigo(string CodigoOrdenCompra)
        {
            string Codigo = (CodigoOrdenCompra ?? string.Empty).Trim();
            if (Codigo.Length == 0 || Codigo.Length > 30)
                throw new ArgumentException("El codigo de la orden debe tener entre 1 y 30 caracteres.", nameof(CodigoOrdenCompra));
            return Codigo;
        }

        internal static string ComprasFuncNormalizarComentario(string Comentario)
        {
            string Texto = (Comentario ?? string.Empty).Trim();
            if (Texto.Length > 500)
                throw new ArgumentException("El comentario admite hasta 500 caracteres.", nameof(Comentario));
            return Texto;
        }

        internal static void ComprasProcValidarCreacion(int IdProveedor, int IdPlazo, int IdMoneda,
            DateTime Emision, DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, decimal Impuesto)
        {
            if (IdProveedor <= 0 || IdPlazo <= 0 || IdMoneda <= 0)
                throw new ArgumentException("Selecciona proveedor, plazo y moneda válidos.");
            if (Emision.Date > Vencimiento.Date)
                throw new ArgumentException("El vencimiento no puede ser anterior a la emisión.");
            ComprasProcValidarDecimal(TipoCambio, 999999.999999M, 6, nameof(TipoCambio));
            if (TipoCambio == 0)
                throw new ArgumentException("El tipo de cambio debe ser positivo.", nameof(TipoCambio));
            ComprasProcValidarDecimal(Subtotal, 99999999.9999M, 4, nameof(Subtotal));
            ComprasProcValidarDecimal(Descuento, Subtotal, 4, nameof(Descuento));
            ComprasProcValidarDecimal(Impuesto, 99999999.9999M, 4, nameof(Impuesto));
            ComprasProcValidarDecimal(Subtotal - Descuento + Impuesto, 99999999.9999M, 4, "Total");
        }

        internal static void ComprasProcValidarVencimiento(ClsOrdenCompra Orden, DateTime Fecha)
        {
            if (Orden == null || Orden.Bloqueado || Orden.Estado != "Pendiente")
                throw new InvalidOperationException("Solo se puede modificar una orden pendiente.");
            if (Fecha.Date < Orden.FechaEmision)
                throw new ArgumentException("El vencimiento no puede ser anterior a la emisión.", nameof(Fecha));
        }
    }
}
