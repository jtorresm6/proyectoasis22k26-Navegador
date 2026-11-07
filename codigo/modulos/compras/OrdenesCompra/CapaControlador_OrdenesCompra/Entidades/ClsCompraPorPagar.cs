using System;

namespace CapaControlador_OrdenesCompra.Entidades
{
    public sealed class ClsCompraPorPagar
    {
        public int IdCuentaPorPagar { get; }
        public int IdProveedor { get; }
        public string NumeroFactura { get; }
        public DateTime FechaEmision { get; }
        public DateTime FechaVencimiento { get; }
        public decimal MontoTotal { get; }
        public decimal Saldo { get; }
        public string Estado { get; }
        public string NombreProveedor { get; }
        public string NitProveedor { get; }
        public string TelefonoProveedor { get; }
        public string CorreoProveedor { get; }
        public string DireccionProveedor { get; }
        public bool ProveedorActivo { get; }

        internal ClsCompraPorPagar(CapaModelo_OrdenesCompra.Entidades.ClsCompraPorPagar Compra)
        {
            if (Compra == null) throw new ArgumentNullException(nameof(Compra));
            IdCuentaPorPagar = Compra.IdCuentaPorPagar;
            IdProveedor = Compra.IdProveedor;
            NumeroFactura = Compra.NumeroFactura;
            FechaEmision = Compra.FechaEmision;
            FechaVencimiento = Compra.FechaVencimiento;
            MontoTotal = Compra.MontoTotal;
            Saldo = Compra.Saldo;
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
