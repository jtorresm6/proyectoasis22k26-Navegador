
/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 *
 * Descripción:
 * Formulario de mantenimiento para los tipos
 * de movimiento de inventario.
 *
 * Permite ingresar, guardar, modificar y eliminar
 * tipos de movimiento mediante el navegador,
 * incluyendo la búsqueda y consulta de registros.
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoTipoMovimiento : Form
    {
        // Constructor del formulario.
        public FrmMantenimientoTipoMovimiento()
        {
            InitializeComponent();

            // Configuración del navegador para
            // tipos de movimiento de inventario.
            LogisticaNavTipoMovimiento.NavegadorMetConfigurar(
                "tblTipoMovimiento", 4, 6);
        }
    }
}
