using System;
using System.Collections.Generic;
using System.IO;
using CapaControlador_Seguridad;
using Microsoft.Reporting.WinForms;

internal static class ValidarReporteBitacora
{
    private static void Main()
    {
        using (var reporte = new LocalReport())
        {
            reporte.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "RpReporteBitacora.rdlc");
            reporte.DataSources.Add(new ReportDataSource("RpReporteBitacora", new List<ClsModeloBitacora> {
                new ClsModeloBitacora { IdBitacora = 1, NombreUsuario = "Prueba", AccionBitacora = "Insertar",
                    TablaBitacora = "tblOrdenCompra", DetallesBitacora = "Validacion de reporte",
                    IpBitacora = "127.0.0.1", FechaHoraBitacora = DateTime.Now }
            }));
            var pdf = reporte.Render("PDF");
            if (pdf.Length < 100) throw new Exception("Reporte vacio");
            Console.WriteLine("OK: reporte PDF generado ({0} bytes).", pdf.Length);
        }
    }
}
