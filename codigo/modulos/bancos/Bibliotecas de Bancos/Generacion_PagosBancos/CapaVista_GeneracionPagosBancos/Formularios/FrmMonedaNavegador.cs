/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Formulario navegador del módulo Moneda. Inicializa el control
 *  de navegación apuntando a la tabla tblMoneda, dentro del módulo
 *  Bancos (8000), aplicación 8001.
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

namespace CapaVista_GeneracionPagos.Formularios
{
    public partial class FrmMonedaNavegador : Form
    {
        public FrmMonedaNavegador()
        {
            InitializeComponent();
            BancosNavMoneda.NavegadorMetConfigurar("tblMoneda", 8000, 8001);

        }
    }
}