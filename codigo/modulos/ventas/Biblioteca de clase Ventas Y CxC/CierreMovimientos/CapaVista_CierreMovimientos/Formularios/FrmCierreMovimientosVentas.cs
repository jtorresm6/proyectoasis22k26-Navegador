/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Todos
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
 * Natali Sofía Montenegro Portillo | Carné: 0901-23-10017
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Diego Alejandro Cheng Peña | Carné: 0901-22-8091
 * Encargado: Todos
 * Fecha de actualización: 17/10/2026
 * Propósito: Cierre transaccional por moneda, histórico y bloqueo de períodos.
 */
using System;
using System.Windows.Forms;

namespace CapaVista_CierreMovimientos
{
    public partial class FrmCierreMovimientosVentas : Form
    {
        private readonly ClsCierrePresentador _Presentador;
        public FrmCierreMovimientosVentas()
        {
            InitializeComponent();
            _Presentador = new ClsCierrePresentador(this);
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

        public void VentasMetConfigurarEnlaceContable(Func<string, int, DateTime, System.Data.DataTable, string> Registrar)
        {
            _Presentador.Consultas.VentasMetConfigurarEnlaceContable(Registrar);
        }

        private void VentasMetPeriodoCierreChanged(object Origen, EventArgs Evento)
        {
            if (_Presentador != null)
                _Presentador.VentasMetPeriodoCambio();
        }
    }
}
