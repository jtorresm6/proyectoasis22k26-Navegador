/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Victor Josue Ortiz Samayoa
 * Carné  : 0901-23-3424
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Formulario navegador del módulo Cuenta Bancaria. Inicializa
 *  el control de navegación apuntando a la tabla tblCuentaBancaria,
 *  dentro del módulo Bancos (8000), aplicación 8005.
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
using System.Diagnostics;
using System.IO;

namespace CapaVista_DisponibilidadDiaria.Formularios
{
    public partial class FrmCuentaBancariaNavegador : Form
    {
        public FrmCuentaBancariaNavegador()
        {
            InitializeComponent();
            BancosNavCuentaBancaria.NavegadorMetConfigurar("tblCuentaBancaria", 8000, 8005);
        }
    }
}
