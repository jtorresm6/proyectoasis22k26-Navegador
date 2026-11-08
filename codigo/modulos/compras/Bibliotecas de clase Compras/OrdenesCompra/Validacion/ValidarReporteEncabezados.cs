using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using Microsoft.Reporting.WinForms;

internal static class ValidarReporteEncabezados
{
    [STAThread]
    private static void Main()
    {
        using (var Vista = new FrmEncabezados())
        using (var Reporte = new LocalReport())
        using (var Datos = new DataTable())
        {
            foreach (string Campo in new[] { "codigoOrdenCompra", "nombreProveedor", "nitProveedor", "estadoOrdenCompra" })
                Datos.Columns.Add(Campo, typeof(string));
            foreach (string Campo in new[] { "fechaEmisionOrdenCompra", "fechaVencimientoOrdenCompra" })
                Datos.Columns.Add(Campo, typeof(DateTime));
            Datos.Columns.Add("totalOrdenCompra", typeof(decimal));
            Datos.Columns.Add("idMoneda", typeof(int));
            Datos.Rows.Add("OC-2026-001", "Proveedor de ejemplo con nombre extendido", "1234567-8", "Pendiente",
                new DateTime(2026, 11, 7), new DateTime(2026, 12, 7), 12345.6789M, 1);
            var Opciones = BindingFlags.Instance | BindingFlags.NonPublic;
            var Tabla = (ComprasUsrDataGridView)typeof(FrmEncabezados).GetField("ComprasDgvEncabezados", Opciones).GetValue(Vista);
            Tabla.DataSource = Datos;
            var Encabezado = (ComprasUsrEncabezado)typeof(FrmEncabezados).GetField("ComprasUsrEncabezado", Opciones).GetValue(Vista);
            var Preparar = (EventHandler<ComprasReporteEventArgs>)typeof(ComprasUsrEncabezado).GetField("PrepararReporte", Opciones).GetValue(Encabezado);
            Reporte.ReportPath = ClsReportesCompras.ObtenerRuta(Vista, Encabezado.RutaReporte);
            Preparar(Encabezado, new ComprasReporteEventArgs(Reporte));
            if (Reporte.DataSources.Count != 1 || !ReferenceEquals(Reporte.DataSources[0].Value, Datos))
                throw new Exception("El reporte no utiliza los datos del listado.");
            byte[] Imagen = Reporte.Render("IMAGE", "<DeviceInfo><OutputFormat>PNG</OutputFormat><DpiX>110</DpiX><DpiY>110</DpiY></DeviceInfo>");
            File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReporteEncabezados.png"), Imagen);
            Datos.Clear();
            if (Reporte.Render("IMAGE", "<DeviceInfo><OutputFormat>PNG</OutputFormat></DeviceInfo>").Length == 0)
                throw new Exception("El reporte vacío no se pudo renderizar.");
            Tabla.DataSource = null;
            Console.WriteLine("Enlace de datos, renderizado con datos y reporte vacío correctos.");
        }
    }
}
