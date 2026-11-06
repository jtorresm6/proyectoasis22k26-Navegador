
/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 *
 * Descripción:
 * Formulario de mantenimiento de detalles de movimientos
 * de inventario. Permite gestionar los registros mediante
 * el navegador, incluyendo las operaciones de ingreso,
 * modificación, eliminación, búsqueda y consulta.
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoDetalleMovimiento : Form
    {
        // Constructor del formulario.
        public FrmMantenimientoDetalleMovimiento()
        {
            InitializeComponent();

            // Configuración del navegador para detalles
            // de movimientos de inventario.
            LogisticaNavDetalleMovimiento.NavegadorMetConfigurar(
                "tblDetalleMovimiento", 4, 6);
        }
    }
}
