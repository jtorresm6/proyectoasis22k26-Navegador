/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de tblJornada (aplicación 9005).
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Empleado.Modelos_de_Controladores;

namespace CapaVista_Empleado.frmReportes
{
    public partial class FrmReporteJornadas : Form
    {
        private ClsModeloJornada _Jornada = new ClsModeloJornada();

        public FrmReporteJornadas()
        {
            InitializeComponent();
        }

        private void FrmReporteJornadas_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsJornada", _Jornada.RrhhMetObtenerParaReporte());
                RrhhRvJornadas.LocalReport.ReportEmbeddedResource = "CapaVista_Empleado.Reportes.RpJornada.rdlc";
                RrhhRvJornadas.LocalReport.DataSources.Clear();
                RrhhRvJornadas.LocalReport.DataSources.Add(Fuente);
                RrhhRvJornadas.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
