using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Presenta el reporte de traslados entre bodegas.
 * ===================================================================
 */

namespace CapaVista_Traslados_Logistica.Formularios
{
    public partial class FrmReporteTraslados : Form
    {
        private readonly DataTable _Traslados;

        public FrmReporteTraslados()
            : this(LogisticaMetCrearTablaVacia())
        {
        }

        public FrmReporteTraslados(DataTable Traslados)
        {
            if (Traslados == null)
            {
                throw new ArgumentNullException("Traslados");
            }

            InitializeComponent();
            Icon = (System.Drawing.Icon)new System.ComponentModel.ComponentResourceManager(
                typeof(FrmTraslados)).GetObject("$this.Icon");
            _Traslados = Traslados;
        }

        private void LogisticaProcCargarReporteTraslados(object Sender, EventArgs E)
        {
            var Fuente = new ReportDataSource("DsTraslados", _Traslados);
            LogisticaRvReporteTraslados.LocalReport.ReportEmbeddedResource =
                "CapaVista_Traslados_Logistica.Reportes.RpReporteTraslados.rdlc";
            LogisticaRvReporteTraslados.LocalReport.DataSources.Clear();
            LogisticaRvReporteTraslados.LocalReport.DataSources.Add(Fuente);
            LogisticaRvReporteTraslados.RefreshReport();
        }

        private static DataTable LogisticaMetCrearTablaVacia()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("CodigoTraslado", typeof(string));
            Tabla.Columns.Add("FechaSolicitud", typeof(DateTime));
            Tabla.Columns.Add("BodegaOrigen", typeof(string));
            Tabla.Columns.Add("FechaSalida", typeof(DateTime));
            Tabla.Columns.Add("BodegaDestino", typeof(string));
            Tabla.Columns.Add("FechaLlegada", typeof(DateTime));
            Tabla.Columns.Add("Observaciones", typeof(string));
            Tabla.Columns.Add("Estado", typeof(string));
            return Tabla;
        }
    }
}