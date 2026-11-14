using System;
using System.Data;
using System.Globalization;
namespace CapaVista_OrdenesCompra
{
    internal sealed class ClsOrdenesCompraFlujo
    {
        internal bool CargandoCatalogos { get; set; }
        internal bool CatalogosCargados { get; set; }
        internal string FechaEmisionTexto { get; set; }
        internal DateTime FechaEmision { get; set; }
        internal long? IdMonedaSeleccionada { get; set; }
        internal int? IdProveedorSeleccionado { get; set; }
        internal int? IdPlazoSeleccionado { get; set; }
        internal decimal? TipoCambioSeleccionado { get; set; }
        internal long? IdTipoCambioSeleccionado { get; set; }
        internal DateTime? FechaVencimiento { get; set; }

        internal bool EncabezadoValido(DataRow Moneda, DataRow Proveedor, DataRow Plazo)
        {
            bool EmisionValida = DateTime.TryParseExact(FechaEmisionTexto, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime Emision) && Emision.Year >= 1000;
            return !CargandoCatalogos && CatalogosCargados && Moneda != null
                && Proveedor != null && Plazo != null && EmisionValida && Emision.Date == FechaEmision.Date
                && IdMonedaSeleccionada == Convert.ToInt64(Moneda["idMoneda"])
                && IdProveedorSeleccionado == Convert.ToInt32(Proveedor["idProveedor"])
                && Convert.ToBoolean(Proveedor["estadoProveedor"])
                && IdPlazoSeleccionado == Convert.ToInt32(Plazo["idPlazo"])
                && TipoCambioSeleccionado.HasValue && TipoCambioSeleccionado.Value > 0M
                && TipoCambioSeleccionado.Value <= 999999.999999M
                && decimal.Round(TipoCambioSeleccionado.Value, 6) == TipoCambioSeleccionado.Value
                && (IdTipoCambioSeleccionado.HasValue && IdTipoCambioSeleccionado.Value > 0
                    || !IdTipoCambioSeleccionado.HasValue && TipoCambioSeleccionado == 1M)
                && FechaVencimiento.HasValue && FechaVencimiento.Value.Date >= Emision.Date;
        }
    }
}
