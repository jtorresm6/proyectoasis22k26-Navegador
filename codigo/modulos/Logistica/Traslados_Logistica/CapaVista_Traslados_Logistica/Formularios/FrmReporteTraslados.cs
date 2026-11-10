using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

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
            _Traslados = Traslados;
        }

        private void FrmReporteTraslados_Load(object Sender, EventArgs E)
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
