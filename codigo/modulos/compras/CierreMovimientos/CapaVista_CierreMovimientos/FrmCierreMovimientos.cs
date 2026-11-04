using System;
using System.Windows.Forms;
using CapaControlador_CierreMovimientos;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta el período, las validaciones y los saldos del cierre de movimientos y coordina las acciones de la pantalla.
 * ===================================================================
*/

namespace CapaVista_CierreMovimientos
{

    public partial class FrmCierreMovimientos : System.Windows.Forms.Form
    {
        private readonly CapaControlador_CierreMovimientos.ClsCierreMovimientos _Controlador = new CapaControlador_CierreMovimientos.ClsCierreMovimientos();

        public FrmCierreMovimientos()
        {
            InitializeComponent();
            ComprasBtnEjecutarCierre.Enabled = false;
            ComprasBtnImprimir.Enabled = false;
            ComprasBtnAyuda.Enabled = false;
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasProcConfigurarTablas();
            Load += ComprasMetCargar;
            ComprasBtnValidar.Click += ComprasMetValidar;

            ComprasBtnSalir.Click += ComprasMetSalir;
        }

        private void ComprasProcConfigurarTablas()
        {
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvValidaciones, "cmp_estado", "Estado", 25, false);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvValidaciones, "cmp_validacion", "Validación", 45, false);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvValidaciones, "cmp_origen", "Origen", 30, false);

            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_proveedor", "Proveedor", 32, false);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_saldo_inicial", "Saldo inicial", 17, true);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_total_cargos", "Cargos", 17, true);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_total_abonos", "Abonos", 17, true);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_saldo_final", "Saldo final", 17, true);
        }

        private void ComprasMetCargar(object Remitente, EventArgs Evento)
        {
            ComprasCboPeriodo.DisplayMember = "cmp_descripcion";
            ComprasCboPeriodo.ValueMember = "cmp_id_periodo";
            ComprasCboPeriodo.DataSource = _Controlador.ComprasFuncObtenerPeriodosCerrables();

            ComprasBtnEjecutarCierre.Enabled = false;
            ComprasBtnImprimir.Enabled = false;
        }

        private void ComprasMetValidar(object Remitente, EventArgs Evento)
        {
            if (ComprasCboPeriodo.SelectedValue == null)
            {
                MessageBox.Show("Seleccione el período a cerrar.", "Cierre de Movimientos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int IdPeriodo = Convert.ToInt32(ComprasCboPeriodo.SelectedValue);
            ClsResumenCierre Resumen = _Controlador.ComprasFuncPrepararCierre(IdPeriodo);

            ComprasDgvValidaciones.DataSource = Resumen.Validaciones;
            ComprasDgvSaldos.DataSource = Resumen.SaldosProveedor;
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalCargos, Resumen.TotalCargos);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalAbonos, Resumen.TotalAbonos);
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSaldoFinal, Resumen.SaldoFinal);

            ComprasBtnEjecutarCierre.Enabled = false;
            ComprasBtnImprimir.Enabled = false;
        }




        private void ComprasMetSalir(object Remitente, EventArgs Evento)
        {
            Close();
        }
    }
}
