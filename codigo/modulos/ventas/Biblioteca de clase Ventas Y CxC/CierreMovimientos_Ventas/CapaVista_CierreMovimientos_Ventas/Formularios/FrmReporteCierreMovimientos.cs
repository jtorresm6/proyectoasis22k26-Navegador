/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 10/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Mostrar el reporte estándar del cierre.
 */
using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CapaVista_CierreMovimientos_Ventas
{
    internal sealed partial class FrmReporteCierreMovimientos : Form
    {
        internal FrmReporteCierreMovimientos(DataTable Datos, string Titulo, DateTime Desde,
            DateTime Hasta, string Moneda, bool EsPartida)
        {
            InitializeComponent();
            VentasMetPreparar(VentasUsrReporteCierre.LocalReport, Datos,
                Titulo, Desde, Hasta, Moneda, EsPartida);
        }

        private void VentasMetReporteMostrado(object Origen, EventArgs Evento)
        {
            VentasUsrReporteCierre.SetDisplayMode(DisplayMode.PrintLayout);
            VentasUsrReporteCierre.ZoomMode = ZoomMode.PageWidth;
            VentasUsrReporteCierre.RefreshReport();
        }
    }
}
