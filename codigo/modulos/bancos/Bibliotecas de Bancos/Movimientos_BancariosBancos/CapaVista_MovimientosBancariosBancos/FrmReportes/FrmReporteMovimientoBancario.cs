using CapaControlador_MovimientosBancarios.Modelo_de_Controladores;
using CapaControlador_Seguridad;
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

namespace CapaVista_MovimientosBancarios.FrmReportes
{
    public partial class FrmReporteMovimientoBancario : Form
    {

        private readonly ClsModeloMovimientoBancario _ControladorMovimiento =
            new ClsModeloMovimientoBancario();

        private Microsoft.Reporting.WinForms.ReportViewer visor;

        public FrmReporteMovimientoBancario()
        {
            InitializeComponent();

            visor = new Microsoft.Reporting.WinForms.ReportViewer
            {
                Dock = DockStyle.Fill,
                ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
            };

            this.Controls.Add(visor);
            visor.BringToFront();

            this.Load += FrmReporteMovimientoBancario_Load;
        }

        private void FrmReporteMovimientoBancario_Load(object sender, EventArgs e)
        {
            try
            {
                var Movimientos =
                    _ControladorMovimiento.BancosMetObtenerMovimientosTabla();

                visor.LocalReport.ReportEmbeddedResource =
                    "CapaVista_MovimientosBancarios.Reportes.RpReporteMovimientoBancario.rdlc";

                visor.LocalReport.DataSources.Clear();

                visor.LocalReport.DataSources.Add(
                    new Microsoft.Reporting.WinForms.ReportDataSource(
                        "ReporteMovimientoBancario",
                        Movimientos
                    )
                );

                visor.RefreshReport();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(
                    "Error al generar reporte:\n" + Ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

    }
}