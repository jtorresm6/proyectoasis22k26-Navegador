using System;
using System.Data;

namespace CapaModelo_OrdenesCompra.Entidades
{
    public sealed class ClsOrdenCompra
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

        public ClsOrdenCompra(DataRow Fila)
        {
            if (Fila == null) throw new ArgumentNullException(nameof(Fila));
            IdOrdenCompra = Convert.ToInt32(Fila["idOrdenCompra"]);
            IdProveedor = Convert.ToInt32(Fila["idProveedor"]);
            CodigoOrdenCompra = IdOrdenCompra.ToString(System.Globalization.CultureInfo.InvariantCulture);
            FechaEmision = Convert.ToDateTime(Fila["fechaEmisionOrdenCompra"]).Date;
            FechaVencimiento = Convert.ToDateTime(Fila["fechaVencimientoOrdenCompra"]).Date;
            MontoTotal = Convert.ToDecimal(Fila["totalOrdenCompra"]);
            Subtotal = Convert.ToDecimal(Fila["subtotalOrdenCompra"]);
            Descuento = Convert.ToDecimal(Fila["descuentoOrdenCompra"]);
            Impuesto = Convert.ToDecimal(Fila["impuestoOrdenCompra"]);
            Bloqueado = Convert.ToBoolean(Fila["bloqueadoOrdenCompra"]);
            IdPlazo = Convert.ToInt32(Fila["idPlazo"]);
            NombrePlazo = Convert.ToString(Fila["nombrePlazo"]);
            DiasPlazo = Convert.ToInt32(Fila["diasPlazo"]);
            IdMoneda = Convert.ToInt32(Fila["idMoneda"]);
            TipoCambio = Convert.ToDecimal(Fila["tipoCambioOrdenCompra"]);
            Comentario = Convert.ToString(Fila["comentarioOrdenCompra"]);
            Estado = Convert.ToString(Fila["estadoOrdenCompra"]);
            NombreProveedor = Convert.ToString(Fila["nombreProveedor"]);
            NitProveedor = Convert.ToString(Fila["nitProveedor"]);
            TelefonoProveedor = Convert.ToString(Fila["telefonoProveedor"]);
            CorreoProveedor = Convert.ToString(Fila["correoProveedor"]);
            DireccionProveedor = Convert.ToString(Fila["direccionProveedor"]);
            ProveedorActivo = !Fila.IsNull("estadoProveedor") && Convert.ToBoolean(Fila["estadoProveedor"]);
        }
    }
}
