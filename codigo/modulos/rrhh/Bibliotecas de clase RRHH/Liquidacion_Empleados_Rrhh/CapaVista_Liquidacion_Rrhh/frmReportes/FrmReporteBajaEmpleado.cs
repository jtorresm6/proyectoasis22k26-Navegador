using CapaControlador_Liquidacion_Rrhh;
using CapaControlador_Liquidacion_Rrhh.Modelos_de_Controladores;
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


namespace CapaVista_Liquidacion_Rrhh.frmReportes
{
    public partial class FrmReporteBajaEmpleado : Form
    {
        private ClsModeloBajaEmpleado BajaEmpleado = new ClsModeloBajaEmpleado();
        public FrmReporteBajaEmpleado()
        {
            InitializeComponent();
        }

        private void FrmReporteBajaEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsBajaEmpleado", BajaEmpleado.LiquidacionMetObtenerTodos());
                RrhhRdvBajaEmpleado.LocalReport.ReportEmbeddedResource = "CapaVista_Liquidacion_Rrhh.Reportes.RpBajaEmpleado.rdlc";
                RrhhRdvBajaEmpleado.LocalReport.DataSources.Clear();
                RrhhRdvBajaEmpleado.LocalReport.DataSources.Add(Fuente);
                RrhhRdvBajaEmpleado.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.RrhhRdvBajaEmpleado.RefreshReport();
        }
    }
}
