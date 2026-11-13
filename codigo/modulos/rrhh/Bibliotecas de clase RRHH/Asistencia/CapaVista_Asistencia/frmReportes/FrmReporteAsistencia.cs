/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de asistencias (aplicación 9101) con los registros
 *  que la ventana tiene filtrados.
 * ===================================================================
*/

using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CapaVista_Asistencia.frmReportes
{
    public partial class FrmReporteAsistencia : Form
    {
        private readonly DataTable _Datos;

        public FrmReporteAsistencia(DataTable Datos)
        {
            InitializeComponent();
            _Datos = Datos;
        }

        private void FrmReporteAsistencia_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsAsistencia", _Datos);
                RrhhRvAsistencia.LocalReport.ReportEmbeddedResource = "CapaVista_Asistencia.Reportes.RpAsistencia.rdlc";
                RrhhRvAsistencia.LocalReport.DataSources.Clear();
                RrhhRvAsistencia.LocalReport.DataSources.Add(Fuente);
                RrhhRvAsistencia.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
