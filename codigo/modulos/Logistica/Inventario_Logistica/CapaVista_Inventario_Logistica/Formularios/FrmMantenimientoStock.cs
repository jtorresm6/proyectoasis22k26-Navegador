
/*
 * MÓDULO: LOGÍSTICA
 * FORMULARIO: FrmMantenimientoStock
 *
 * DESCRIPCIÓN:
 * Formulario de mantenimiento de stock.
 * Permite consultar y administrar las existencias
 * físicas y virtuales de los productos por sucursal
 * y bodega.
 *
 * También permite acceder a los mantenimientos de
 * movimientos de inventario, detalle de movimientos
 * y tipos de movimiento.
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoStock : Form
    {
        // Constructor del formulario.
        public FrmMantenimientoStock()
        {
            InitializeComponent();
        }

        // Procedimiento para abrir formularios secundarios.
        private void LogisticaMetAbrirFormulario(Form Formulario)
        {
            using (Formulario)
            {
                Formulario.StartPosition =
                    FormStartPosition.CenterParent;

                Formulario.ShowDialog(this);
            }
        }

        // Evento para abrir Movimiento de Inventario.
        private void LogisticaBtnMovimientoInventario_Click(
            object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(
                new FrmMantenimientoMovimientoInventario());
        }

        // Evento para abrir Detalle de Movimiento.
        private void LogisticaBtnDetalleMovimiento_Click(
            object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(
                new FrmMantenimientoDetalleMovimiento());
        }

        // Evento para abrir Tipo de Movimiento.
        private void LogisticaBtnTipoMovimiento_Click(
            object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(
                new global::CapaVista_Inventario_Logistica
                    .Formularios.FrmMantenimientoTipoMovimiento());
        }
    }
}
