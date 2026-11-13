using System;
using CapaControlador_MovimientosProveedores;
using CapaVista_Componentes.Componentes;
using Microsoft.Reporting.WinForms;

namespace CapaVista_MovimientosProveedores
{
    public partial class FrmMovimientosProveedores
    {
        private void ComprasMetPrepararReporteCuentaPorPagar(object Remitente, ComprasReporteEventArgs Evento)
        {
            int IdProveedor = ComprasFuncObtenerIdSeleccionado(ComprasCboProveedor);
            var Controlador = new ClsControladorCuentaPorPagar();
            // El encabezado compartido abre el formulario del visor y muestra los errores de consulta.
            var Datos = Controlador.ComprasMetConsultar(IdProveedor == 0 ? (int?)null : IdProveedor);
            Evento.Reporte.DataSources.Clear();
            Evento.Reporte.DataSources.Add(new ReportDataSource("CuentaPorPagar", Datos));
            Evento.Reporte.SetParameters(new ReportParameter("FiltroProveedor",
                IdProveedor == 0 ? "Todos los proveedores" : "Proveedor: " + ComprasCboProveedor.Text));
        }
    }
}
