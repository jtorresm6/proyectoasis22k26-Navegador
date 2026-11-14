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
    public partial class FrmReporteAlertaCadenaFrio : Form
    {
        // =====================================================
        // DATOS DEL REPORTE
        // =====================================================

        private readonly DataTable _Alertas;

        public FrmReporteAlertaCadenaFrio()
            : this(LogisticaMetCrearTablaVacia())
        {
        }

        public FrmReporteAlertaCadenaFrio(DataTable Alertas)
        {
            if (Alertas == null)
            {
                throw new ArgumentNullException("Alertas");
            }

            InitializeComponent();
            _Alertas = Alertas;
        }

        // =====================================================
        // CARGAR REPORTVIEWER
        // =====================================================

        private void FrmReporteAlertaCadenaFrio_Load(
            object sender, EventArgs e)
        {
            // Configurar ReportViewer.
            LogisticaRvReporteAlertaCadenaFrio.ProcessingMode =
                ProcessingMode.Local;

            // Asignar el RDLC.
            LogisticaRvReporteAlertaCadenaFrio
                .LocalReport.ReportEmbeddedResource =
                "CapaVista_TransporteInterno_Logistica." +
                "Reportes.RpReporteAlertaCadenaFrio.rdlc";

            // Limpiar los datos anteriores.
            LogisticaRvReporteAlertaCadenaFrio
                .LocalReport.DataSources.Clear();

            // Crear origen de datos.
            ReportDataSource Fuente = new ReportDataSource(
                "DsAlertaCadenaFrio",
                _Alertas);

            // Asignar los registros.
            LogisticaRvReporteAlertaCadenaFrio
                .LocalReport.DataSources.Add(Fuente);

            // Mostrar reporte.
            LogisticaRvReporteAlertaCadenaFrio.RefreshReport();
        }

        // =====================================================
        // CREAR DATATABLE VACÍO
        // =====================================================

        private static DataTable LogisticaMetCrearTablaVacia()
        {
            DataTable Tabla = new DataTable();

            Tabla.Columns.Add(
                "IdAlertaCadenaFrio", typeof(long));

            Tabla.Columns.Add(
                "FkTelemetriaTemperaturaCodigo",
                typeof(long));

            Tabla.Columns.Add(
                "TipoAlertaCadenaFrio", typeof(string));

            Tabla.Columns.Add(
                "Atendida", typeof(string));

            Tabla.Columns.Add(
                "ObservacionCalidadAlertaCadenaFrio",
                typeof(string));

            return Tabla;
        }
    }
}
