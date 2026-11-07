/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Formulario 9006 - AsignacionPuestos. Mantenimiento de tblPuestoEmpleado con el Navegador.
 *  Asigna un puesto a cada empleado y guarda su historial.
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using CapaVista_Empleado.frmReportes;

namespace CapaVista_Empleado.Formularios
{
    public partial class FrmAsigPuestosNavegador : Form
    {
        public FrmAsigPuestosNavegador()
        {
            InitializeComponent();
            RrhhNavAsigPuestos.NavegadorMetConfigurar("tblPuestoEmpleado", 9000, 9006);
        }

    }
}
