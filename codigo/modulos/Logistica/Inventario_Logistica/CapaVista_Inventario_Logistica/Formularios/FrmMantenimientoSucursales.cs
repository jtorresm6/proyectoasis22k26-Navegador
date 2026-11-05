
/*
 * Responsable: Velveth Saraí Chavez Mejía
 * Carné: 0901 23 6269
 *
 * Descripción:
 * Formulario de mantenimiento de sucursales.
 * Permite ingresar, guardar, modificar y eliminar
 * sucursales en el sistema mediante el navegador,
 * incluyendo la búsqueda y filtrado de registros.
 *
 * También permite acceder al mantenimiento de bodegas.
 *
 * Fecha: 25/10/2026
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoSucursales : Form
    {
        // Constructor del formulario.
        public FrmMantenimientoSucursales()
        {
            InitializeComponent();

            // Configuración del navegador para sucursales.
            LogisticaNavSucursales.NavegadorMetConfigurar(
                "tblSucursal", 4, 6);
        }

        // Procedimiento para abrir el mantenimiento de bodegas.
        private void LogisticaProcAbrirMantenimientoBodegas()
        {
            using (FrmMantenimientoBodegas FrmBodega =
                new FrmMantenimientoBodegas())
            {
                FrmBodega.ShowDialog(this);
            }
        }

        // Evento del botón de mantenimiento de bodegas.
        private void LogisticaBtnBodega_Click(
            object sender, EventArgs e)
        {
            LogisticaProcAbrirMantenimientoBodegas();
        }
    }
}
