
/*
 * Responsable: Nills Berducido Gómez
 * Carné: 0901 23 14275
 *
 * Descripción:
 * Formulario de mantenimiento de líneas de productos.
 * Permite ingresar, guardar, modificar y eliminar
 * líneas en el sistema mediante el navegador,
 * incluyendo la búsqueda y consulta de registros.
 *
 * Fecha: 26/10/2026
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoLinea : Form
    {
        // Constructor del formulario.
        public FrmMantenimientoLinea()
        {
            InitializeComponent();

            // Configuración del navegador para líneas.
            LogisticaNavLinea.NavegadorMetConfigurar(
                "tblLinea", 4, 6);
        }
    }
}
