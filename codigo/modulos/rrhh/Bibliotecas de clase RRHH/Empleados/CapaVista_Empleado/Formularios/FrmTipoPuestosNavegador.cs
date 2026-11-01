/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Formulario 9003 - TipoPuestos. Mantenimiento de tblTipoPuesto con el Navegador.
 *  Tipos de puesto (operativo, administrativo, etc.).
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using CapaVista_Empleado.frmReportes;

namespace CapaVista_Empleado.Formularios
{
    public partial class FrmTipoPuestosNavegador : Form
    {
        public FrmTipoPuestosNavegador()
        {
            InitializeComponent();
            RrhhNavTipoPuestos.NavegadorMetConfigurar("tblTipoPuesto", 9000, 9003);
        }

    }
}
