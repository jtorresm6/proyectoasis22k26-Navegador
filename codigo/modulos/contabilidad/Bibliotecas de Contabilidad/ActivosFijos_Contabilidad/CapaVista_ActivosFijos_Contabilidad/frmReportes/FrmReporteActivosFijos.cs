/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Ventana del reporte de activos fijos y depreciación a una fecha de
 *  corte, con filtros arriba y el ReportViewer abajo.
 * Reglas especificas:
 *  La ventana solo habla con ClsControladorReporteActivosFijosContabilidad.
 *  El reporte RpReporteActivosFijos.rdlc va embebido en este ensamblado.
 *  Imprimir y exportar solo se muestran si el permiso lo permite.
 * ===================================================================
*/

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Contabilidad;
using CapaVista_Contabilidad.Componentes;

namespace CapaVista_Contabilidad
{
    // Ventana del reporte de activos fijos
    public partial class FrmReporteActivosFijos : Form
    {
        // Recurso del reporte
        public const string RecursoReporte = "CapaVista_Contabilidad.Reportes.RpReporteActivosFijos.rdlc";

        // Conjunto de datos del reporte
        public const string ConjuntoDatos = "DsActivosFijos";

        // Controlador del reporte
        private readonly ClsControladorReporteActivosFijosContabilidad _Controlador = new ClsControladorReporteActivosFijosContabilidad();

        // Filas mostradas
        private DataTable _Datos;

        // Abre con todos los permisos
        public FrmReporteActivosFijos() : this(ClsModeloPermisosPantallaContabilidad.Total)
        {
        }

        // Abre con los permisos dados
        public FrmReporteActivosFijos(ClsModeloPermisosPantallaContabilidad Permisos)
        {
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Reporte de activos fijos", new Size(1180, 720));
            bool PuedeImprimir = Permisos != null && Permisos.PuedeImprimir;
            ContabilidadRpvReporte.ShowPrintButton = PuedeImprimir;
            ContabilidadRpvReporte.ShowExportButton = PuedeImprimir;
            ContabilidadDtpCorte.MaxDate = DateTime.Today;
            ContabilidadDtpCorte.Value = DateTime.Today;
            ContabilidadCboAgrupar.DataSource = _Controlador.ContabilidadFuncAgrupaciones();
            try
            {
                ContabilidadCboModulo.DataSource = _Controlador.ContabilidadFuncModulos();
                ContabilidadCboTipo.DisplayMember = "Nombre";
                ContabilidadCboTipo.ValueMember = "Tipo";
                ContabilidadCboTipo.DataSource = _Controlador.ContabilidadFuncTipos();
            }
            catch (Exception Excepcion)
            {
                ContabilidadCboModulo.DataSource = new[] { ClsControladorReporteActivosFijosContabilidad.Todos };
                Load += (Remitente, Argumentos) =>
                    ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron cargar los filtros: " + Excepcion.Message);
            }
            ContabilidadBtnGenerar.Click += (Remitente, Argumentos) => ContabilidadMetGenerar();
            Load += (Remitente, Argumentos) => ContabilidadMetGenerar();
            Disposed += (Remitente, Argumentos) => { if (_Datos != null) _Datos.Dispose(); };
        }

        // Parámetros del reporte
        public static ReportParameter[] ContabilidadFuncParametros(DateTime Corte, string Modulo, string Tipo, string AgruparPor)
        {
            string Detalle = "Agrupado por " + (AgruparPor ?? string.Empty).ToLower() + "   |   Módulo: " + (Modulo ?? string.Empty) +
                "   |   Tipo de activo: " + (Tipo ?? string.Empty) + "   |   Valores en quetzales";
            return new[]
            {
                new ReportParameter("Titulo", "Reporte de activos fijos y depreciación"),
                new ReportParameter("Corte", "Fecha de corte: " + Corte.ToString("dd/MM/yyyy")),
                new ReportParameter("Detalle", Detalle)
            };
        }

        // Carga el reporte con sus datos
        public static void ContabilidadMetPrepararReporte(LocalReport Reporte, DataTable Filas, ReportParameter[] Parametros)
        {
            Reporte.ReportEmbeddedResource = RecursoReporte;
            Reporte.DisplayName = "Reporte de activos fijos";
            Reporte.DataSources.Clear();
            Reporte.DataSources.Add(new ReportDataSource(ConjuntoDatos, Filas));
            Reporte.SetParameters(Parametros);
        }

        // Genera el reporte
        private void ContabilidadMetGenerar()
        {
            string Mensaje;
            DataTable Datos = _Controlador.ContabilidadFuncDatosReporte(ContabilidadDtpCorte.Value, ContabilidadCboModulo.Text,
                Convert.ToString(ContabilidadCboTipo.SelectedValue), ContabilidadCboAgrupar.Text, out Mensaje);
            if (Datos == null)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, Mensaje);
                return;
            }
            try
            {
                ContabilidadMetPrepararReporte(ContabilidadRpvReporte.LocalReport, Datos, ContabilidadFuncParametros(
                    ContabilidadDtpCorte.Value, ContabilidadCboModulo.Text, ContabilidadCboTipo.Text, ContabilidadCboAgrupar.Text));
                ContabilidadRpvReporte.SetDisplayMode(DisplayMode.PrintLayout);
                ContabilidadRpvReporte.ZoomMode = ZoomMode.PageWidth;
                ContabilidadRpvReporte.RefreshReport();
                if (_Datos != null) _Datos.Dispose();
                _Datos = Datos;
            }
            catch (Exception Excepcion)
            {
                Datos.Dispose();
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudo cargar el reporte: " + Excepcion.Message);
            }
        }
    }
}
