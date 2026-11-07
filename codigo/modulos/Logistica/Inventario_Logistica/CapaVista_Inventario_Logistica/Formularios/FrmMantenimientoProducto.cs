
/*
 * Responsable: Gabriela Pinto García
 * Carné: 9959 23 1087
 *
 * Descripción:
 * Formulario de mantenimiento de productos.
 * Permite ingresar, guardar, modificar y eliminar
 * productos en el sistema mediante el navegador.
 *
 * También permite acceder a los mantenimientos
 * de categorías, marcas y líneas de productos.
 *
 * Fecha: 25/10/2024
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoProducto : Form
    {
        // Constructor del formulario.
        public FrmMantenimientoProducto()
        {
            InitializeComponent();

            // Configuración del navegador para productos.
            LogisticaNavProducto.NavegadorMetConfigurar(
                "tblProducto", 4, 6);
        }

        // Procedimiento para abrir mantenimiento de categorías.
        private void LogisticaProcAbrirMantenimientoCategoria()
        {
            using (FrmMantenimientoCategoria FrmCategoria =
                new FrmMantenimientoCategoria())
            {
                FrmCategoria.ShowDialog(this);
            }
        }

        // Evento del botón Categoría.
        private void LogisticaBtnCategoria_Click(
            object sender, EventArgs e)
        {
            LogisticaProcAbrirMantenimientoCategoria();
        }

        // Procedimiento para abrir mantenimiento de marcas.
        private void LogisticaProcAbrirMantenimientoMarca()
        {
            using (FrmMantenimientoMarca FrmMarca =
                new FrmMantenimientoMarca())
            {
                FrmMarca.ShowDialog(this);
            }
        }

        // Evento del botón Marca.
        private void LogisticaBtnMarca_Click(
            object sender, EventArgs e)
        {
            LogisticaProcAbrirMantenimientoMarca();
        }

        // Procedimiento para abrir mantenimiento de líneas.
        private void LogisticaProcAbrirMantenimientoLinea()
        {
            using (FrmMantenimientoLinea FrmLinea =
                new FrmMantenimientoLinea())
            {
                FrmLinea.ShowDialog(this);
            }
        }

        // Evento del botón Línea.
        private void LogisticaBtnLinea_Click(
            object sender, EventArgs e)
        {
            LogisticaProcAbrirMantenimientoLinea();
        }
    }
}
