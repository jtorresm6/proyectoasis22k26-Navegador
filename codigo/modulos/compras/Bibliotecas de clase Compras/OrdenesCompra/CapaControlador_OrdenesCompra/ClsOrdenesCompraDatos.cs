using System;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Expone los datos de una orden de compra a la capa de presentación.
 * ===================================================================
*/

namespace CapaControlador_OrdenesCompra
{
    public sealed class ClsOrdenesCompraDatos
    {
        public int IdOrdenCompra { get; }
        public int IdProveedor { get; }
        public string CodigoOrdenCompra { get; }
        public string NumeroOrden => CodigoOrdenCompra;
        public DateTime FechaEmision { get; }
        public DateTime FechaVencimiento { get; }
        public decimal MontoTotal { get; }
        public decimal Subtotal { get; }
        public decimal Descuento { get; }
        public decimal Impuesto { get; }
        public bool Bloqueado { get; }
        public int IdPlazo { get; }
        public string NombrePlazo { get; }
        public int DiasPlazo { get; }
        public int IdMoneda { get; }
        public decimal TipoCambio { get; }
        public string Comentario { get; }
        public string Estado { get; }
        public string NombreProveedor { get; }
        public string NitProveedor { get; }
        public string TelefonoProveedor { get; }
        public string CorreoProveedor { get; }
        public string DireccionProveedor { get; }
        public bool ProveedorActivo { get; }

        internal ClsOrdenesCompraDatos(CapaModelo_OrdenesCompra.Entidades.ClsOrdenesCompraEntidad Compra)
        {
            if (Compra == null) throw new ArgumentNullException(nameof(Compra));
            IdOrdenCompra = Compra.IdOrdenCompra;
            IdProveedor = Compra.IdProveedor;
            CodigoOrdenCompra = Compra.CodigoOrdenCompra;
            FechaEmision = Compra.FechaEmision;
            FechaVencimiento = Compra.FechaVencimiento;
            MontoTotal = Compra.MontoTotal;
            Subtotal = Compra.Subtotal;
            Descuento = Compra.Descuento;
            Impuesto = Compra.Impuesto;
            Bloqueado = Compra.Bloqueado;
            IdPlazo = Compra.IdPlazo;
            NombrePlazo = Compra.NombrePlazo;
            DiasPlazo = Compra.DiasPlazo;
            IdMoneda = Compra.IdMoneda;
            TipoCambio = Compra.TipoCambio;
            Comentario = Compra.Comentario;
            Estado = Compra.Estado;
            NombreProveedor = Compra.NombreProveedor;
            NitProveedor = Compra.NitProveedor;
            TelefonoProveedor = Compra.TelefonoProveedor;
            CorreoProveedor = Compra.CorreoProveedor;
            DireccionProveedor = Compra.DireccionProveedor;
            ProveedorActivo = Compra.ProveedorActivo;
        }
    }
}
