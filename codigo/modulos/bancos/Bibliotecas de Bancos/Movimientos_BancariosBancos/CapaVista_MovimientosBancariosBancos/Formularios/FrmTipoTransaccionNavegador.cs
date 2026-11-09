/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Oscar Emilio Morales Lemus
 * Carné  : 9959-23-3070
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Formulario navegador del módulo Tipo de Transacción. Inicializa
 *  el control de navegación apuntando a la tabla tblTipoTransaccion,
 *  dentro del módulo Bancos (8000), aplicación 8004.
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

namespace CapaVista_MovimientosBancarios.Formularios
{
    public partial class FrmTipoTransaccionNavegador : Form
    {
        public FrmTipoTransaccionNavegador()
        {
            InitializeComponent();
            BancosNavTipoTransaccion.NavegadorMetConfigurar("tblTipoTransaccion", 8000, 8004);

        }
    }
}
