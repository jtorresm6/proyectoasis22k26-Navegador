/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Formulario 9002 - Departamentos. Mantenimiento de tblDepartamento con el Navegador.
 *  Departamentos de la empresa.
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using CapaVista_Empleado.frmReportes;

namespace CapaVista_Empleado.Formularios
{
    public partial class FrmDepartamentosNavegador : Form
    {
        public FrmDepartamentosNavegador()
        {
            InitializeComponent();
            RrhhNavDepartamentos.NavegadorMetConfigurar("tblDepartamento", 9000, 9002);
        }

     
    }
}
