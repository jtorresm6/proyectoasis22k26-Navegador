using System.Data;
using System.Globalization;
using Microsoft.Reporting.WinForms;
using CapaControlador_OrdenesCompra;
namespace CapaVista_OrdenesCompra
{
    internal static class ClsDetallesOrdenesCompraReporte
    {
        internal static void Preparar(LocalReport Reporte, ClsOrdenesCompraDatos Compra, DataTable Detalles, string Moneda)
        {
            Reporte.DataSources.Clear();
            Reporte.DataSources.Add(new ReportDataSource("DetalleOrdenCompra", Detalles));
            Reporte.DisplayName = "Orden de compra " + Compra.CodigoOrdenCompra;
            var Formato = CultureInfo.GetCultureInfo("es-GT");
            Reporte.SetParameters(new[]
            {
                new ReportParameter("Orden", "Orden No. " + Compra.CodigoOrdenCompra + " | Estado: " + Compra.Estado),
                new ReportParameter("Proveedor", "PROVEEDOR: " + Compra.NombreProveedor + "\nNIT: " + Compra.NitProveedor
                    + "\nDirección: " + Compra.DireccionProveedor + "\nTel.: " + Compra.TelefonoProveedor + " | Correo: " + Compra.CorreoProveedor),
                new ReportParameter("Condiciones", "Emisión: " + Compra.FechaEmision.ToString("dd/MM/yyyy")
                    + " | Vencimiento: " + Compra.FechaVencimiento.ToString("dd/MM/yyyy")
                    + "\nPlazo: " + Compra.NombrePlazo + " (" + Compra.DiasPlazo + " días) | Moneda: " + Moneda),
                new ReportParameter("Totales", "Subtotal: " + Compra.Subtotal.ToString("N4", Formato)
                    + "\nDescuento: " + Compra.Descuento.ToString("N4", Formato)
                    + "\nImpuesto: " + Compra.Impuesto.ToString("N4", Formato)
                    + "\nTOTAL (" + Moneda + "): " + Compra.MontoTotal.ToString("N4", Formato)),
                new ReportParameter("Comentario", string.IsNullOrWhiteSpace(Compra.Comentario) ? "Sin observaciones." : Compra.Comentario)
            });
        }
    }
}
