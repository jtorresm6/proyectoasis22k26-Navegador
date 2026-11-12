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
    public partial class FrmReporteDetalleViajeProducto : Form
    {
        // =====================================================
        // DATOS DEL REPORTE
        // =====================================================

        private readonly DataTable _DetallesProductos;

        public FrmReporteDetalleViajeProducto()
            : this(LogisticaMetCrearTablaVacia())
        {
        }

        public FrmReporteDetalleViajeProducto(
            DataTable DetallesProductos)
        {
            if (DetallesProductos == null)
            {
                throw new ArgumentNullException(
                    "DetallesProductos");
            }

            InitializeComponent();

            _DetallesProductos = DetallesProductos;
        }

        // =====================================================
        // CARGAR EL REPORTVIEWER
        // =====================================================

        private void FrmReporteDetalleViajeProducto_Load(
            object Sender, EventArgs E)
        {
            // Configurar ReportViewer para reportes locales.
            LogisticaRvReporteDetalleViajeProducto.ProcessingMode =
                ProcessingMode.Local;

            // Asignar el archivo RDLC.
            LogisticaRvReporteDetalleViajeProducto
                .LocalReport.ReportEmbeddedResource =
                "CapaVista_TransporteInterno_Logistica." +
                "Reportes.RpReporteDetalleViajeProducto.rdlc";

            // Limpiar orígenes de datos anteriores.
            LogisticaRvReporteDetalleViajeProducto
                .LocalReport.DataSources.Clear();

            // Crear el origen de datos.
            ReportDataSource Fuente = new ReportDataSource(
                "DsDetalleViajeProducto",
                _DetallesProductos);

            // Asignar el origen de datos.
            LogisticaRvReporteDetalleViajeProducto
                .LocalReport.DataSources.Add(Fuente);

            // Actualizar el reporte.
            LogisticaRvReporteDetalleViajeProducto.RefreshReport();
        }

        // =====================================================
        // ESTRUCTURA DEL DATATABLE
        // =====================================================

        private static DataTable LogisticaMetCrearTablaVacia()
        {
            DataTable Tabla = new DataTable();

            Tabla.Columns.Add(
                "IdDetalleViajeLoteProducto",
                typeof(long));

            Tabla.Columns.Add(
                "FkDetalleViajeCodigo",
                typeof(long));

            Tabla.Columns.Add(
                "FkLoteProductoCodigo",
                typeof(long));

            Tabla.Columns.Add(
                "CantidadCajasDetalleViajeLoteProducto",
                typeof(int));

            Tabla.Columns.Add(
                "PesoKgDetalleViajeLoteProducto",
                typeof(decimal));

            return Tabla;
        }

        private void FrmReporteDetalleViajeProducto_Load_1(object sender, EventArgs e)
        {


            // Configurar ReportViewer para reportes locales.
            LogisticaRvReporteDetalleViajeProducto.ProcessingMode =
                ProcessingMode.Local;

            // Asignar el archivo RDLC.
            LogisticaRvReporteDetalleViajeProducto
                .LocalReport.ReportEmbeddedResource =
                "CapaVista_TransporteInterno_Logistica." +
                "Reportes.RpReporteDetalleViajeProducto.rdlc";

            // Limpiar orígenes de datos anteriores.
            LogisticaRvReporteDetalleViajeProducto
                .LocalReport.DataSources.Clear();

            // Crear el origen de datos.
            ReportDataSource Fuente = new ReportDataSource(
                "DsDetalleViajeProducto",
                _DetallesProductos);

            // Asignar el origen de datos.
            LogisticaRvReporteDetalleViajeProducto
                .LocalReport.DataSources.Add(Fuente);

            // Actualizar el reporte.
            LogisticaRvReporteDetalleViajeProducto.RefreshReport();

        }
    }
}
