/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Muestra en un ReportViewer el estado financiero que se armó en la
 *  vista previa, con el mismo nivel de detalle, para imprimirlo o
 *  exportarlo a PDF.
 * Reglas especificas:
 *  El reporte RpEstadoFinanciero.rdlc va embebido en este ensamblado y
 *  recibe las filas tal como se ven en la vista previa.
 * ===================================================================
*/

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaVista_Contabilidad.Componentes;

namespace CapaVista_Contabilidad
{
    public partial class FrmReporteEstadoFinanciero : Form
    {
        public const string RecursoReporte = "CapaVista_Contabilidad.Reportes.RpEstadoFinanciero.rdlc";

        private readonly DataTable _Filas;
        private readonly ReportParameter[] _Parametros;

        public FrmReporteEstadoFinanciero(DataTable Filas, string Titulo, string Periodo, string Detalle)
        {
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Reporte de estados financieros", new Size(1100, 720));
            _Filas = Filas.Copy();
            _Parametros = ContabilidadFuncParametros(Titulo, Periodo, Detalle);
            Disposed += (Remitente, Argumentos) => _Filas.Dispose();
            Load += ContabilidadMetCargar;
        }

        public static ReportParameter[] ContabilidadFuncParametros(string Titulo, string Periodo, string Detalle)
        {
            return new[]
            {
                new ReportParameter("Titulo", Titulo ?? string.Empty), new ReportParameter("Periodo", Periodo ?? string.Empty),
                new ReportParameter("Detalle", Detalle ?? string.Empty)
            };
        }

        public static void ContabilidadMetPrepararReporte(LocalReport Reporte, DataTable Filas, ReportParameter[] Parametros)
        {
            Reporte.ReportEmbeddedResource = RecursoReporte;
            Reporte.DataSources.Clear();
            Reporte.DataSources.Add(new ReportDataSource("DsEstadoFinanciero", Filas));
            Reporte.SetParameters(Parametros);
        }

        private void ContabilidadMetCargar(object Remitente, EventArgs Argumentos)
        {
            try
            {
                ContabilidadMetPrepararReporte(ContabilidadRpvReporte.LocalReport, _Filas, _Parametros);
                ContabilidadRpvReporte.SetDisplayMode(DisplayMode.PrintLayout);
                ContabilidadRpvReporte.ZoomMode = ZoomMode.PageWidth;
                ContabilidadRpvReporte.RefreshReport();
            }
            catch (Exception Excepcion)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudo cargar el reporte: " + Excepcion.Message);
            }
        }
    }
}
