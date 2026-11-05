/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar

 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Fecha de actualización: 17/10/2026
 * Propósito: Cotizaciones con precios históricos, validaciones y conversión documental.
 */
using System;
using System.Windows.Forms;

namespace CapaVista_Cotizaciones
{
    public partial class FrmCotizacionVentas : Form
    {
        private readonly ClsCotizacionesPresentador _Presentador;
        public FrmCotizacionVentas()
        {
            InitializeComponent();
            _Presentador = new ClsCotizacionesPresentador(this);
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

        private void VentasMetCotVigenciaChanged(object Origen, EventArgs Evento)
        {
            if (_Presentador != null)
                _Presentador.VentasMetMostrarVigencia();
        }
    }
}
