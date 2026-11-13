/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Muestra en un visor el reporte de una conciliación bancaria con
 *  sus partidas conciliadas, con diferencia y pendientes.
 * ================================================================
 */
using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_ConciliacionBancaria;
using Microsoft.Reporting.WinForms;

namespace CapaVista_ConciliacionBancaria.frmReportes
{
    public partial class FrmReporteConciliacionBancaria : Form
    {
        private readonly ClsControladorConciliacion _Controlador = new ClsControladorConciliacion();
        private readonly long _IdConciliacion;

        public FrmReporteConciliacionBancaria(long IdConciliacion)
        {
            InitializeComponent();
            _IdConciliacion = IdConciliacion;
        }

        private void FrmReporteConciliacionBancaria_Load(object sender, EventArgs e)
        {
            string Error = _Controlador.BancosMetConsultarReporte(_IdConciliacion, out DataTable Partidas);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible generar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            ReportDataSource DatosReporte = new ReportDataSource("DsConciliacionBancaria", Partidas);
            BancosRvReporte.LocalReport.ReportEmbeddedResource = "CapaVista_ConciliacionBancaria.Reportes.RpReporteConciliacionBancaria.rdlc";
            BancosRvReporte.LocalReport.DataSources.Clear();
            BancosRvReporte.LocalReport.DataSources.Add(DatosReporte);
            BancosRvReporte.RefreshReport();
        }
    }
}
