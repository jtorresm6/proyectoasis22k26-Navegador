
/*
 * Responsable: Nills Berducido Gómez
 * Carné: 0901 23 14275
 *
 * Descripción:
 * Formulario de mantenimiento de categorías.
 * Permite ingresar, guardar, modificar y eliminar
 * categorías de productos en el sistema mediante
 * el navegador, incluyendo la búsqueda y el
 * filtrado de registros.
 *
 * Fecha: 26/10/2026
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoCategoria : Form
    {
        // Constructor del formulario.
        public FrmMantenimientoCategoria()
        {
            InitializeComponent();

            // Configuración del navegador para categorías.
            LogisticaNavCategoria.NavegadorMetConfigurar(
                "tblCategoria", 4, 6);
        }
    }
}
