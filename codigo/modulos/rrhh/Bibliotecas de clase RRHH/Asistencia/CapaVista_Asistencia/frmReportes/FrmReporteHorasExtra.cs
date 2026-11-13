/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de horas extra (aplicación 9102) con los registros
 *  que la ventana tiene filtrados.
 * ===================================================================
*/

using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CapaVista_Asistencia.frmReportes
{
    public partial class FrmReporteHorasExtra : Form
    {
        private readonly DataTable _Datos;

        public FrmReporteHorasExtra(DataTable Datos)
        {
            InitializeComponent();
            _Datos = Datos;
        }

        private void FrmReporteHorasExtra_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsHorasExtra", _Datos);
                RrhhRvHorasExtra.LocalReport.ReportEmbeddedResource = "CapaVista_Asistencia.Reportes.RpHorasExtra.rdlc";
                RrhhRvHorasExtra.LocalReport.DataSources.Clear();
                RrhhRvHorasExtra.LocalReport.DataSources.Add(Fuente);
                RrhhRvHorasExtra.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
