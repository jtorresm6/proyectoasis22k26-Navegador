using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    public partial class FrmReporteTelemetriaTemperatura : Form
    {
        private readonly DataTable _Telemetrias;

        // Constructor sin parámetros.
        public FrmReporteTelemetriaTemperatura()
            : this(new DataTable())
        {
        }

        // Constructor que recibe los registros reales.
        public FrmReporteTelemetriaTemperatura(DataTable Telemetrias)
        {
            if (Telemetrias == null)
            {
                throw new ArgumentNullException("Telemetrias");
            }

            InitializeComponent();

            _Telemetrias = Telemetrias;
        }



        private void FrmReporteTelemetriaTemperatura_Load(object sender, EventArgs e)
        {
            // Configurar ReportViewer.
            LogisticaRvReporteTelemetriaTemperatura.ProcessingMode =
                ProcessingMode.Local;

            // Asignar el archivo RDLC.
            LogisticaRvReporteTelemetriaTemperatura
                .LocalReport.ReportEmbeddedResource =
                "CapaVista_TransporteInterno_Logistica." +
                "Reportes.RpReporteTelemetriaTemperatura.rdlc";

            // Limpiar los datos anteriores.
            LogisticaRvReporteTelemetriaTemperatura
                .LocalReport.DataSources.Clear();

            // Crear el origen de datos.
            ReportDataSource Fuente = new ReportDataSource(
                "DsTelemetriaTemperatura",
                _Telemetrias);

            // Asignar los registros.
            LogisticaRvReporteTelemetriaTemperatura
                .LocalReport.DataSources.Add(Fuente);

            // Mostrar el reporte.
            LogisticaRvReporteTelemetriaTemperatura.RefreshReport();
        }
    }
}
