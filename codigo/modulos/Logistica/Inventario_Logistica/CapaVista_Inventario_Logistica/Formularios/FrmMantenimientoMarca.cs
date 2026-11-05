
/*
 * Responsable: Carlos Eduardo Chin Caal
 * Carné: 0901-23-5686
 *
 * Descripción:
 * Formulario de mantenimiento de marcas de productos.
 * Permite ingresar, guardar, modificar y eliminar
 * marcas en el sistema mediante el navegador,
 * incluyendo la búsqueda y filtrado de registros.
 *
 * Fecha: 26/10/2026
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoMarca : Form
    {
        // Constructor del formulario.
        public FrmMantenimientoMarca()
        {
            InitializeComponent();

            // Configuración del navegador para marcas.
            LogisticaNavMarca.NavegadorMetConfigurar(
                "tblmarca", 4, 6);
        }
    }
}
