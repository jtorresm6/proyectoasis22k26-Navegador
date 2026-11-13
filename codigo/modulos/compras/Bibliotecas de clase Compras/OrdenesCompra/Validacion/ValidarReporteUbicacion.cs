using System;
using System.Data;
using System.IO;
using Microsoft.Reporting.WinForms;
using CapaModelo_OrdenesCompra.Repositorios;

class ValidarReporteUbicacion
{
    static int Main()
    {
        try
        {
            var Repositorio = new ClsOrdenesCompraRepositorio();
            using (var Ordenes = Repositorio.ComprasFuncListar())
            {
                if (Ordenes.Rows.Count == 0) throw new Exception("No hay órdenes para verificar");
                int Indice = Math.Min(1, Ordenes.Rows.Count - 1);
                int Id = Convert.ToInt32(Ordenes.Rows[Indice]["idOrdenCompra"]);
                using (var Detalles = Repositorio.ComprasFuncListarDetalles(Id))
                using (var Reporte = new LocalReport())
                {
                    foreach (DataRow Fila in Detalles.Rows)
                        if (string.IsNullOrWhiteSpace(Convert.ToString(Fila["nombreSucursal"])) || string.IsNullOrWhiteSpace(Convert.ToString(Fila["nombreBodega"])))
                            throw new Exception("Falta el nombre de sucursal o bodega");
                    Reporte.ReportPath = "CapaVista_OrdenesCompra/Reportes/RpReporteDetalleOrdenCompra.rdlc";
                    Reporte.DataSources.Add(new ReportDataSource("DetalleOrdenCompra", Detalles));
                    Reporte.SetParameters(new[] {
                        new ReportParameter("Orden", "Orden No. " + Id),
                        new ReportParameter("Proveedor", "PROVEEDOR: Validación del reporte de compras"),
                        new ReportParameter("Condiciones", "Verificación de sucursal y bodega por producto"),
                        new ReportParameter("Totales", "Subtotal: " + Ordenes.Rows[Indice]["subtotalOrdenCompra"] + "\nDescuento: " + Ordenes.Rows[Indice]["descuentoOrdenCompra"]
                            + "\nImpuesto: " + Ordenes.Rows[Indice]["impuestoOrdenCompra"] + "\nTOTAL: " + Ordenes.Rows[Indice]["totalOrdenCompra"]),
                        new ReportParameter("Comentario", "Validación de lectura. No se modificaron registros.")
                    });
                    string Mime, Codificacion, Extension; string[] Flujos; Warning[] Avisos;
                    byte[] Imagen = Reporte.Render("IMAGE", "<DeviceInfo><OutputFormat>PNG</OutputFormat><DpiX>120</DpiX><DpiY>120</DpiY><StartPage>1</StartPage><EndPage>1</EndPage></DeviceInfo>",
                        out Mime, out Codificacion, out Extension, out Flujos, out Avisos);
                    File.WriteAllBytes("Validacion/ReporteUbicacion.png", Imagen);
                    foreach (Warning Aviso in Avisos) if (Aviso.Severity == Severity.Error) throw new Exception(Aviso.Message);
                    Console.WriteLine("Reporte renderizado correctamente con sucursal y bodega. Productos: " + Detalles.Rows.Count + ". Avisos: " + Avisos.Length);
                }
            }
            return 0;
        }
        catch (Exception Error) { Console.WriteLine(Error); return 1; }
    }
}
