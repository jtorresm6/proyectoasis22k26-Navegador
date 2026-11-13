using CapaControlador_Liquidacion_Rrhh.Modelos_de_Controladores;
using System;
using Microsoft.Reporting.WinForms;
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
    public partial class FrmReporteParametroLey : Form
    {
        private ClsModeloParametroLey ParametroLey = new ClsModeloParametroLey();
        public FrmReporteParametroLey()
        {
            InitializeComponent();
        }

        private void RrhhRdvParametroLey_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsParametroLey", ParametroLey.ParametroLeyMetObtenerParaReporte());
                RrhhRdvParametroLey.LocalReport.ReportEmbeddedResource = "CapaVista_Liquidacion_Rrhh.Reportes.RpParametroLey.rdlc";
                RrhhRdvParametroLey.LocalReport.DataSources.Clear();
                RrhhRdvParametroLey.LocalReport.DataSources.Add(Fuente);
                RrhhRdvParametroLey.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.RrhhRdvParametroLey.RefreshReport();
        }
    }
}
