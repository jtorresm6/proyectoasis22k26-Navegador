/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de tblPuestoEmpleado (aplicación 9006).
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Empleado.Modelos_de_Controladores;

namespace CapaVista_Empleado.frmReportes
{
    public partial class FrmReporteAsigPuestos : Form
    {
        private ClsModeloPuestoEmpleado _PuestoEmpleado = new ClsModeloPuestoEmpleado();

        public FrmReporteAsigPuestos()
        {
            InitializeComponent();
        }

        private void FrmReporteAsigPuestos_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsPuestoEmpleado", _PuestoEmpleado.RrhhMetObtenerParaReporte());
                RrhhRvAsigPuestos.LocalReport.ReportEmbeddedResource = "CapaVista_Empleado.Reportes.RpPuestoEmpleado.rdlc";
                RrhhRvAsigPuestos.LocalReport.DataSources.Clear();
                RrhhRvAsigPuestos.LocalReport.DataSources.Add(Fuente);
                RrhhRvAsigPuestos.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
