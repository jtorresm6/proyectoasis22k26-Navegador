/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Formulario 9004 - Puestos. Mantenimiento de tblPuesto con el Navegador.
 *  Puestos de trabajo, cada uno ligado a su tipo de puesto.
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using CapaVista_Empleado.frmReportes;

namespace CapaVista_Empleado.Formularios
{
    public partial class FrmPuestosNavegador : Form
    {
        public FrmPuestosNavegador()
        {
            InitializeComponent();
            RrhhNavPuestos.NavegadorMetConfigurar("tblPuesto", 9000, 9004);
        }
    }
}
