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
    public partial class FrmReporteEncabezadoViaje : Form
    {
        // =====================================================
        // DATOS QUE RECIBIRÁ EL REPORTE
        // =====================================================

        private readonly DataTable _Encabezados;

        public FrmReporteEncabezadoViaje()
            : this(LogisticaMetCrearTablaVacia())
        {
        }

        public FrmReporteEncabezadoViaje(DataTable Encabezados)
        {
            if (Encabezados == null)
            {
                throw new ArgumentNullException("Encabezados");
            }

            InitializeComponent();

            _Encabezados = Encabezados;
        }

        // =====================================================
        // CARGAR REPORTE EN REPORTVIEWER
        // =====================================================

        private void FrmReporteEncabezadoViaje_Load(
            object Sender, EventArgs E)
        {
            var Fuente = new ReportDataSource(
                "DsEncabezadoViaje",
                _Encabezados);

            LogisticaRvReporteEncabezadoViaje.ProcessingMode =
                ProcessingMode.Local;

            LogisticaRvReporteEncabezadoViaje
                .LocalReport.ReportEmbeddedResource =
                "CapaVista_TransporteInterno_Logistica." +
                "Reportes.RpReporteEncabezadoViaje.rdlc";

            LogisticaRvReporteEncabezadoViaje
                .LocalReport.DataSources.Clear();

            LogisticaRvReporteEncabezadoViaje
                .LocalReport.DataSources.Add(Fuente);

            LogisticaRvReporteEncabezadoViaje.RefreshReport();
        }

        // =====================================================
        // ESTRUCTURA DEL DATATABLE
        // =====================================================

        private static DataTable LogisticaMetCrearTablaVacia()
        {
            var Tabla = new DataTable();

            // Columnas principales del RDLC.
            Tabla.Columns.Add("CodigoViaje", typeof(string));
            Tabla.Columns.Add("FechaDocumento", typeof(DateTime));
            Tabla.Columns.Add("FechaSalida", typeof(DateTime));
            Tabla.Columns.Add("FechaLlegada", typeof(DateTime));
            Tabla.Columns.Add("Transporte", typeof(string));
            Tabla.Columns.Add("Conductor", typeof(string));
            Tabla.Columns.Add("EstadoViaje", typeof(string));
            Tabla.Columns.Add("PesoTotal", typeof(decimal));

            // Columnas adicionales disponibles.
            Tabla.Columns.Add("CadenaFrio", typeof(string));
            Tabla.Columns.Add("IdTransporte", typeof(int));
            Tabla.Columns.Add("IdConductor", typeof(int));
            Tabla.Columns.Add("FechaCreacion", typeof(DateTime));
            Tabla.Columns.Add("FechaActualizacion", typeof(DateTime));

            return Tabla;
        }
    }
}
