/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Andre De Jesus Gonzalez Camey
 * Carné  : 9959-23-3117
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Formulario navegador del módulo Bancos. Inicializa el control
 *  de navegación apuntando a la tabla tblBanco, dentro del módulo
 *  Bancos (8000), aplicación 8002. Aplica el color de fondo del
 *  componente a todos los controles internos del navegador.
 * ================================================================
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmBancoNavegador : Form
    {
        public FrmBancoNavegador()
        {
            InitializeComponent();
            BancosNavBanco.NavegadorMetConfigurar("tblBanco", 8000, 8002);
            foreach (Control ctrl in BancosNavBanco.Controls)
            {
                ctrl.BackColor = Color.FromArgb(46, 138, 85);
                foreach (Control inner in ctrl.Controls)
                {
                    inner.BackColor = Color.FromArgb(46, 138, 85);
                }
            }


        }
    }
}
