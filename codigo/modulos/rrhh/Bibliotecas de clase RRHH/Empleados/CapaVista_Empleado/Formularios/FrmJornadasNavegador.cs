/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Formulario 9005 - Jornadas. Mantenimiento de tblJornada con el Navegador.
 *  Jornadas laborales con su horario y minutos de tolerancia.
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using CapaVista_Empleado.frmReportes;

namespace CapaVista_Empleado.Formularios
{
    public partial class FrmJornadasNavegador : Form
    {
        public FrmJornadasNavegador()
        {
            InitializeComponent();
            RrhhNavJornadas.NavegadorMetConfigurar("tblJornada", 9000, 9005);
        }

    }
}
