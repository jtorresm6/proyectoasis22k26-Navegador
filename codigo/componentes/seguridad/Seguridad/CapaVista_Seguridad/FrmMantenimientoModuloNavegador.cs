using System;
using System.Collections.Generic;
using CapaControlador_Seguridad;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace CapaVista_Seguridad
{
    public partial class FrmMantenimientoModuloNavegador : Form
    {
        private const int ID_MODULO = 4;
        private const int ID_APLICACION = 6;
        private readonly ClsModeloModulo _ControladorModulo = new ClsModeloModulo();
        private readonly Dictionary<string, object> _FuentesReporte = new Dictionary<string, object>();

        public FrmMantenimientoModuloNavegador()
        {
            InitializeComponent();

            // El diseñador puede crear el formulario sin una conexión ODBC.
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            ActualizarFuentesReporte();
            navegador1.NavegadorMetConfigurar("tblmodulo", ID_MODULO, ID_APLICACION, _FuentesReporte);
            navegador1.NavegadorAccionSolicitada += NavegadorAccionSolicitada;
        }

        private void NavegadorAccionSolicitada(string accion)
        {
            if (accion == "IMPRIMIR")
                ActualizarFuentesReporte();
        }

        private void ActualizarFuentesReporte()
        {
            // Si la consulta falla, nunca se conserva una copia antigua del reporte.
            var datos = new DataTable();
            datos.Columns.Add("IdModulo", typeof(int));
            datos.Columns.Add("NombreModulo", typeof(string));
            datos.Columns.Add("DescripcionModulo", typeof(string));
            datos.Columns.Add("IsActive", typeof(bool));
            AsignarFuentesReporte(datos);

            try
            {
                AsignarFuentesReporte(_ControladorModulo.SeguridadMetObtenerModulosReporte());
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los datos del reporte de módulos: " + ex.Message,
                    "Mantenimiento de módulos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AsignarFuentesReporte(DataTable datos)
        {
            _FuentesReporte["DsMantenimientoModulo"] = datos;
            _FuentesReporte["RpReporteMantenimientoModulo"] = datos;
        }
    }
}
