using System;
using System.Data;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Representa la información consultada de una compra por pagar,
 *  incluyendo factura, fechas, saldo y datos del proveedor.
 * ===================================================================
*/

namespace CapaModelo_OrdenesCompra.Entidades
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

        public ClsCompraPorPagar(DataRow Fila)
        {
            if (Fila == null) throw new ArgumentNullException(nameof(Fila));
            IdCuentaPorPagar = Convert.ToInt32(Fila["idCuentaPorPagar"]);
            IdProveedor = Convert.ToInt32(Fila["idProveedor"]);
            NumeroFactura = Convert.ToString(Fila["numeroFacturaCuentaPorPagar"]);
            FechaEmision = Convert.ToDateTime(Fila["fechaEmisionCuentaPorPagar"]).Date;
            FechaVencimiento = Convert.ToDateTime(Fila["fechaVencimientoCuentaPorPagar"]).Date;
            MontoTotal = Convert.ToDecimal(Fila["montoTotalCuentaPorPagar"]);
            Saldo = Convert.ToDecimal(Fila["saldoCuentaPorPagar"]);
            Estado = Convert.ToString(Fila["estadoCuentaPorPagar"]);
            NombreProveedor = Convert.ToString(Fila["nombreProveedor"]);
            NitProveedor = Convert.ToString(Fila["nitProveedor"]);
            TelefonoProveedor = Convert.ToString(Fila["telefonoProveedor"]);
            CorreoProveedor = Convert.ToString(Fila["correoProveedor"]);
            DireccionProveedor = Convert.ToString(Fila["direccionProveedor"]);
            ProveedorActivo = !Fila.IsNull("estadoProveedor") && Convert.ToBoolean(Fila["estadoProveedor"]);
        }
    }
}
