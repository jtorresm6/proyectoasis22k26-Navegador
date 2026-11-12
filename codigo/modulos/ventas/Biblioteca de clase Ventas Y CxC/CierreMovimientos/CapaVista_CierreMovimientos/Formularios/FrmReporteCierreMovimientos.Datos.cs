/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 10/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Preparar los datos del reporte de cierre.
 */
using System;
using System.Data;
using Microsoft.Reporting.WinForms;

namespace CapaVista_CierreMovimientos
{
    internal sealed partial class FrmReporteCierreMovimientos
    {

        internal static void VentasMetPreparar(LocalReport Reporte, DataTable Datos, string Titulo,
            DateTime Desde, DateTime Hasta, string Moneda, bool EsPartida)
        {
            var Tabla = new DataTable();
            foreach (string Campo in new[] { "Referencia", "Detalle", "Anterior", "Cargos", "Abonos", "Actual" })
                Tabla.Columns.Add(Campo, typeof(string));

            foreach (DataRow Fila in Datos.Rows)
            {
                if (EsPartida)
                    Tabla.Rows.Add(VentasFuncTexto(Fila, "Cuenta"), VentasFuncTexto(Fila, "Descripcion"), "",
                        VentasFuncImporte(Fila, "Debe"), VentasFuncImporte(Fila, "Haber"), "");
                else
                    Tabla.Rows.Add(VentasFuncTexto(Fila, "Codigo"), VentasFuncTexto(Fila, "Nombre"),
                        VentasFuncImporte(Fila, "Anterior"), VentasFuncImporte(Fila, "Cargos"),
                        VentasFuncImporte(Fila, "Abonos"), VentasFuncImporte(Fila, "Actual"));
            }

            Reporte.ReportEmbeddedResource = "CapaVista_CierreMovimientos.Reportes.RpReporteCierreMovimientos.rdlc";
            Reporte.DataSources.Clear();
            Reporte.DataSources.Add(new ReportDataSource("DsCierre", Tabla));
            Reporte.SetParameters(new[]
            {
                new ReportParameter("FechaDesde", Desde.ToString("dd/MM/yyyy")),
                new ReportParameter("FechaHasta", Hasta.ToString("dd/MM/yyyy")),
                new ReportParameter("Estado", Titulo + " | " + Moneda)
            });
            Reporte.DisplayName = "Ventas_Cierre_" + Desde.ToString("yyyyMMdd") + "_" + Hasta.ToString("yyyyMMdd");
        }

        private static string VentasFuncTexto(DataRow Fila, string Campo)
        {
            return Fila.Table.Columns.Contains(Campo) && Fila[Campo] != DBNull.Value ? Convert.ToString(Fila[Campo]) : "";
        }

        private static string VentasFuncImporte(DataRow Fila, string Campo)
        {
            return Fila.Table.Columns.Contains(Campo) && Fila[Campo] != DBNull.Value
                ? Convert.ToDecimal(Fila[Campo]).ToString("N2") : "";
        }
    }
}
