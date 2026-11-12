
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: FrmMantenimientoSucursales.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Formulario de mantenimiento de sucursales.
 * Integra el Navegador y la generación de reportes.
 *
 * Incluye una verificación temporal para identificar
 * la ubicación del archivo RDLC.
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using CapaControlador_Inventario_Logistica;
using CapaControlador_Inventario_Logistica.Modelos_de_Controladores;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoSucursales : Form
    {
        private readonly ClsModeloSucursal _DatosSucursal =
            new ClsModeloSucursal();

        private readonly Dictionary<string, object> _FuentesReporteSucursal =
            new Dictionary<string, object>();

        public FrmMantenimientoSucursales()
        {
            InitializeComponent();

            // =====================================================
            // OBTENER LOS REGISTROS DE SUCURSALES
            // =====================================================

            List<ClsModeloSucursal> ListaSucursales =
                _DatosSucursal.LogisticaMetObtenerTodosInforme();

            // =====================================================
            // COMPROBAR LA UBICACIÓN DEL REPORTE
            // =====================================================

            string CarpetaEjecucion =
                AppDomain.CurrentDomain.BaseDirectory;

            string RutaReportes = Path.Combine(
                CarpetaEjecucion,
                "Reportes",
                "RpReporteSucursal.rdlc"
            );

            string RutaPrincipal = Path.Combine(
                CarpetaEjecucion,
                "RpReporteSucursal.rdlc"
            );

            string EstadoReporte =
                File.Exists(RutaReportes)
                    ? "Encontrado en carpeta Reportes"
                    : File.Exists(RutaPrincipal)
                        ? "Encontrado junto al ejecutable"
                        : "No encontrado en las rutas comprobadas";

            MessageBox.Show(
                "Sucursales obtenidas: " + ListaSucursales.Count +
                "\n\nCarpeta de ejecución:\n" +
                CarpetaEjecucion +
                "\n\nEstado del archivo RDLC:\n" +
                EstadoReporte +
                "\n\nRuta comprobada:\n" +
                RutaReportes,
                "Diagnóstico del reporte de Sucursales",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // =====================================================
            // ENTREGAR DATOS AL NAVEGADOR
            // =====================================================

            _FuentesReporteSucursal.Add(
                "DsReporteSucursal",
                ListaSucursales
            );

            // =====================================================
            // CONFIGURAR NAVEGADOR
            // =====================================================

            LogisticaNavSucursales.NavegadorMetConfigurar(
                "tblSucursal",
                4,
                15,
                _FuentesReporteSucursal
            );
        }
    }
}
