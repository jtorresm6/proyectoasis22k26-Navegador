/*
 * Responsable: Velveth Saraí Chavez Mejía
 * Carné: 0901 23 6269
 *
 * Descripción:
 * Formulario de mantenimiento de bodegas.
 * Permite ingresar, guardar, modificar y eliminar bodegas
 * mediante el navegador, incluyendo la búsqueda y
 * filtrado de registros.
 *
 * Fecha: 25/10/2026
 */

using System;
using System.Windows.Forms;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoBodegas : Form
    {
        public FrmMantenimientoBodegas()
        {
            InitializeComponent();

            LogisticaNavBodega.NavegadorMetConfigurar("tblBodega", 4, 6);
        }
    }
}