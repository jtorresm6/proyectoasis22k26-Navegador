/*
 * ================================================================
 * �rea   : Bancos
 * Autor  : Oscar Emilio Morales Lemus
 * Carn�  : 9959-23-3070
 * Autor  : Evelyn Sof�a Andrade Luna
 * Carn�  : 9959-23-1224
 * Fecha  : 23/10/2026
 * ================================================================
 * Prop�sito :
 *  Formulario MDI principal del m�dulo Bancos. Contiene el panel
 *  de navegaci�n colapsable (bot�n burger) y centraliza el acceso
 *  a todos los subformularios del m�dulo: Mantenimientos (Moneda,
 *  Bancos, Tipo de Cuenta Bancaria, Tipo de Transacci�n, Cuenta
 *  Bancaria) y Procesos (Movimientos Bancarios, Autorizaci�n de
 *  �rdenes, Conciliaci�n Bancaria, Disponibilidad Diaria, Tipo de
 *  Cambio, Generaci�n de Pagos, Cierre Bancario, Enlace a Otros
 *  M�dulos). Muestra el usuario y rol activo, incluye acceso a la
 *  ayuda del m�dulo y permite cerrar la sesi�n.
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
using CapaVista_EnlaceB.Formularios;
using CapaVista_MovimientosBancarios.Formularios;
using CapaVista_AutorizacionOrdenes;
using CapaVista_AutorizacionOrdenes.Formularios;
using CapaVista_DisponibilidadDiaria.Formularios;
using CapaVista_TipoCambio.Formularios;
using CapaVista_ConciliacionBancaria.Formularios;
using CapaVista_GeneracionPagos.Formularios;
using CapaVista_CierreBancario.Formularios;


namespace CapaVista_Bancos.Formularios
{
    public partial class FrmMDIBancos : Form
    {
        public FrmMDIBancos()
        {
            InitializeComponent();
        }

        private void BancosBtnBurger_Click(object sender, EventArgs e)
        {
            if (BancosPnlNavegador.Width == 293)
            {
                BancosPnlNavegador.Width = 67;
                BancosPnlDashboard.Location = new Point(200, 52);
                BancosBtnBurger.Location = new Point(220, 13);
                BancosLblUsuario.Location = new Point(285, 14);
                BancosLblUsuarioRol.Location = new Point(524, 14);
                BancosLblMan.Text = "M:";
                BancosLblPro.Text = "P:";
            }
            else
            {
                BancosPnlNavegador.Width = 293;
                BancosPnlDashboard.Location = new Point(307, 52);
                BancosBtnBurger.Location = new Point(323, 13);
                BancosLblUsuario.Location = new Point(385, 14);
                BancosLblUsuarioRol.Location = new Point(624, 14);
                BancosLblMan.Text = "Mantenimientos:";
                BancosLblPro.Text = "Procesos:";
            }
        }
        private void BancosBtnMovBan_Click(object sender, EventArgs e)
        {
            FrmMovimientosBancarios MovimientosBancarios = new FrmMovimientosBancarios();
            MovimientosBancarios.ShowDialog();
        }
        public void BancosMetActualizarInfoUsuario(string Nombre, string Rol)
        {
            BancosLblUsuario.Text = $"Usuario: {Nombre}";
            BancosLblUsuarioRol.Text = $"Rol: {Rol}";
            BancosLblUsuarioRol.Left = BancosLblUsuario.Right + 30;
        }

        private void BancosBtnAutoCompra_Click(object sender, EventArgs e)
        {
            FrmAutorizacionOrdenes AutorizacionOrdenes = new FrmAutorizacionOrdenes();
            AutorizacionOrdenes.ShowDialog();
        }

        private void BancosBtnEnlace_Click(object sender, EventArgs e)
        {
            FrmEnlaceOtrosModulos Enlace = new FrmEnlaceOtrosModulos();
            Enlace.ShowDialog();
        }

        private void BancosBtnDisp_Click(object sender, EventArgs e)
        {
            FrmDisponibilidadDiaria Enlace = new FrmDisponibilidadDiaria();
            Enlace.ShowDialog();
        }

        private void BancosBtnCambio_Click(object sender, EventArgs e)
        {
            FrmTipoCambio Enlace = new FrmTipoCambio();
            Enlace.ShowDialog();
        }

        private void BancosBtnConBan_Click(object sender, EventArgs e)
        {
            FrmConciliacion Conciliacion = new FrmConciliacion();
            Conciliacion.ShowDialog();
        }

        private void BancosBtnGenPagos_Click(object sender, EventArgs e)
        {
            FrmGeneracionPagos GeneracionPagos = new FrmGeneracionPagos();
            GeneracionPagos.ShowDialog();
        }

        private void BancosBtnCierreBan_Click(object sender, EventArgs e)
        {
            FrmCierreBancario CierreBancario = new FrmCierreBancario();
            CierreBancario.ShowDialog();
        }

        private void BancosBtnTipoTransaccion_Click(object sender, EventArgs e)
        {
            FrmTipoTransaccionNavegador TipoTransaccion = new FrmTipoTransaccionNavegador();
            TipoTransaccion.ShowDialog();
        }

        private void BancosBtnTipoCuentaBancaria_Click(object sender, EventArgs e)
        {
            FrmTipoCuentaBancariaNavegador TipoCuentaBancariaNavegador = new FrmTipoCuentaBancariaNavegador();
            TipoCuentaBancariaNavegador.ShowDialog();
        }

        private void BancosBtnBancos_Click(object sender, EventArgs e)
        {
            FrmBancoNavegador Banco = new FrmBancoNavegador();
            Banco.ShowDialog();
        }

        private void BancosBtnCuentaBancaria_Click(object sender, EventArgs e)
        {
            FrmCuentaBancariaNavegador cuentaBancariaNavegador = new FrmCuentaBancariaNavegador();
            cuentaBancariaNavegador.ShowDialog();
        }

        private void BancosBtnMoneda_Click(object sender, EventArgs e)
        {
            FrmMonedaNavegador Moneda = new FrmMonedaNavegador();
            Moneda.ShowDialog();
        }

        private void BancosBtnAyuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "C:/BancosAyudas/CapaBancos.chm", "MDI_Bancos.html");
        }

        private void BancosBtnCerrarP_Click(object sender, EventArgs e)
        {
            DialogResult Resultado = MessageBox.Show(
                 "�Seguro que deseas cerrar sesi�n?",
                 "Cerrar sesi�n",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);

            if (Resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
