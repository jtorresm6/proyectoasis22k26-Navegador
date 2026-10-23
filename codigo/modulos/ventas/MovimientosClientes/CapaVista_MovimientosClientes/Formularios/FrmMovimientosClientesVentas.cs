/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Fecha de actualización: 17/10/2026
 * Propósito: Operar CxC, registrar documentos y recibos y preparar depósitos con persistencia transaccional.
 */
using System;
using System.Windows.Forms;

namespace CapaVista_MovimientosClientes
{
    public partial class FrmMovimientosClientesVentas : Form
    {
        private readonly ClsMovimientosPresentador _Presentador;
        public FrmMovimientosClientesVentas()
        {
            InitializeComponent();
            _Presentador = new ClsMovimientosPresentador(this);
        }

        public int VentasIdUsuario
        {
            get
            {
                return _Presentador.VentasIdUsuario;
            }

            set
            {
                _Presentador.VentasIdUsuario = value;
            }
        }

        public void VentasMetMostrarRecepcionPagos()
        {
            _Presentador.VentasMetMostrarRecepcionPagos();
        }

        public void VentasMetMostrarConsultaRecibos(string Recibo, string Deposito)
        {
            _Presentador.VentasMetMostrarConsultaRecibos(Recibo, Deposito);
        }

        public void VentasMetConfigurarCatalogosBancos(System.Data.DataTable Empresas, System.Data.DataTable Bancos, System.Data.DataTable Cuentas, System.Data.DataTable Conceptos, string Modo)
        {
            _Presentador.VentasMetConfigurarCatalogosBancos(Empresas, Bancos, Cuentas, Conceptos, Modo);
        }

        private void VentasTbcMovimientos_SelectedIndexChanged(object Remitente, EventArgs ArgumentosEvento)
        {
            if (_Presentador != null)
                _Presentador.VentasMetCambiarPestana();
        }

        private void VentasCboTipoDocumento_SelectedIndexChanged(object Remitente, EventArgs ArgumentosEvento)
        {
            if (_Presentador != null)
                _Presentador.VentasMetTipoDocumentoCambio();
        }
    }
}
