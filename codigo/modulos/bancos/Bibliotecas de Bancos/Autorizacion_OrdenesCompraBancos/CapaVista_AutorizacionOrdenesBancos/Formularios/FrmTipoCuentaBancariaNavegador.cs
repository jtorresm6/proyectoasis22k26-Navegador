using CapaControlador_AutorizacionOrdenes.Modelos_de_controladores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * ================================================================
 * Área   : Autorización de Órdenes
 * Autor  : Andy Alfonso Garcia Lopez
 * Carné  : 9959-23-1494
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Formulario navegador del módulo Tipo de Cuenta Bancaria.
 *  Inicializa el control de navegación apuntando a la tabla
 *  tblTipoCuentaBancaria, dentro del módulo Bancos (8000),
 *  aplicación 8003 (Tipo de Cuenta Bancaria).
 * ================================================================
 */

namespace CapaVista_AutorizacionOrdenes.Formularios
{
    public partial class FrmTipoCuentaBancariaNavegador : Form
    {
        private ClsModeloTipoCuentaBancaria datos = new ClsModeloTipoCuentaBancaria();
        public FrmTipoCuentaBancariaNavegador()
        {
            InitializeComponent();
            Dictionary<string, object> fuentes = new Dictionary<string, object>();
            fuentes.Add("DsTipoCuentaBancaria", datos.BancosMetObtenerTodos());
            BancosNavTipoCuenta.NavegadorMetConfigurar("tblTipoCuentaBancaria", 8000, 8003, fuentes);
            this.Load += FrmTipoCuentaBancariaNavegador_Load;
        }

        private void FrmTipoCuentaBancariaNavegador_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(290, 20);
        }
    }
}
