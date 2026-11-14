// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con Contabilidad
// Descripción: Formulario que carga y muestra el reporte RDLC del enlace con
//              Contabilidad en el visor de reportes.
// Fecha:       12/11/2026
// =====================================================================
/*
 * ================================================================
 * Área   : Bancos
 * Autor  : André De Jesús Gonzalez Camey
 * Carné  : 9959-23-3117
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Muestra en un visor el reporte con el historial de partidas
 *  enviadas a Contabilidad desde el enlace de Bancos.
 * ================================================================
 */
using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using Microsoft.Reporting.WinForms;

namespace CapaVista_EnlaceB.frmReportes
{
    public partial class FrmReporteEnlaceContabilidad : Form
    {
        private readonly ClsControladorEnlaceB _Controlador = new ClsControladorEnlaceB();

        public FrmReporteEnlaceContabilidad()
        {
            InitializeComponent();
        }

        private void FrmReporteEnlaceContabilidad_Load(object sender, EventArgs e)
        {
            string Error = _Controlador.BancarioMetReporteHistorico(out DataTable Historico);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible generar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            ReportDataSource DatosReporte = new ReportDataSource("DsEnlaceContabilidad", Historico);
            BancosRvReporte.LocalReport.ReportEmbeddedResource = "CapaVista_EnlaceB.Reportes.RpReporteEnlaceContabilidad.rdlc";
            BancosRvReporte.LocalReport.DataSources.Clear();
            BancosRvReporte.LocalReport.DataSources.Add(DatosReporte);
            BancosRvReporte.RefreshReport();
        }
    }
}
