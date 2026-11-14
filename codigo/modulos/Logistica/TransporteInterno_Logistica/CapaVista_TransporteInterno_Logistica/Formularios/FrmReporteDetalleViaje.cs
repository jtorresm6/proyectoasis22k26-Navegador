using CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores;
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
    public partial class FrmReporteDetalleViaje : Form
    {
        // =====================================================
        // DATOS DEL REPORTE
        // =====================================================

        private readonly DataTable _Detalles;

        public FrmReporteDetalleViaje()
            : this(new DataTable())
        {
        }

        public FrmReporteDetalleViaje(DataTable Detalles)
        {
            if (Detalles == null)
            {
                throw new ArgumentNullException("Detalles");
            }

            InitializeComponent();

            _Detalles = Detalles;
        }



        private DataTable LogisticaMetObtenerDatosReporte()
        {
            DataTable Tabla = new DataTable();

            Tabla.Columns.Add("IdDetalleViaje", typeof(long));
            Tabla.Columns.Add("FkEncabezadoViajeCodigo", typeof(long));
            Tabla.Columns.Add("OrdenDetalleViaje", typeof(int));
            Tabla.Columns.Add("DescripcionDetalleViaje", typeof(string));
            Tabla.Columns.Add("CantidadDetalleViaje", typeof(decimal));
            Tabla.Columns.Add("PesoDetalleViaje", typeof(decimal));

            var Controlador = new ClsControladorDetalleViaje();
            var Registros = Controlador.LogisticaFuncObtenerTodos();

            foreach (var Detalle in Registros)
            {
                DataRow Fila = Tabla.NewRow();

                Fila["IdDetalleViaje"] = Detalle.IdDetalleViaje;
                Fila["FkEncabezadoViajeCodigo"] =
                    Detalle.FkEncabezadoViajeCodigo;
                Fila["OrdenDetalleViaje"] = Detalle.OrdenDetalleViaje;
                Fila["DescripcionDetalleViaje"] =
                    Detalle.DescripcionDetalleViaje ?? "";
                Fila["CantidadDetalleViaje"] =
                    Detalle.CantidadDetalleViaje;
                Fila["PesoDetalleViaje"] = Detalle.PesoDetalleViaje;

                Tabla.Rows.Add(Fila);
            }

            return Tabla;
        }


        // =====================================================
        // CARGAR EL REPORTVIEWER
        // =====================================================

        private void FrmReporteDetalleViaje_Load(
            object Sender, EventArgs E)
        {
            var Fuente = new ReportDataSource(
                "DsDetalleViaje",
                _Detalles);

            LogisticaRvReporteDetalleViaje.ProcessingMode =
                ProcessingMode.Local;

            LogisticaRvReporteDetalleViaje
                .LocalReport.ReportEmbeddedResource =
                "CapaVista_TransporteInterno_Logistica." +
                "Reportes.RpReporteDetalleViaje.rdlc";

            LogisticaRvReporteDetalleViaje
                .LocalReport.DataSources.Clear();

            LogisticaRvReporteDetalleViaje
                .LocalReport.DataSources.Add(Fuente);

            LogisticaRvReporteDetalleViaje.RefreshReport();
        }
    }
}
