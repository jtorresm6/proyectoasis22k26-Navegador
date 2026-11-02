using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Herramientas;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 29/10/2026
 * ==================================================================
 * Propósito :
 *  Permite elegir la moneda en la que el usuario desea ver los montos de
 *  Compras durante su sesión. Los montos se guardan siempre en la moneda
 *  local; esta preferencia solo cambia cómo se muestran.
 * ===================================================================
*/

namespace CapaVista_Herramientas.Controles
{
    public partial class UsrMonedaVisualizacion : UserControl
    {
        private const decimal _MontoEjemplo = 10000m;
        private ClsControladorMoneda _Controlador;
        private ClsMonedaCompras _MonedaLocal;

        public UsrMonedaVisualizacion()
        {
            InitializeComponent();
            ComprasBtnAplicar.Click += ComprasMetAplicar;
            ComprasBtnRestablecer.Click += ComprasMetRestablecer;
            ComprasCboMoneda.SelectedIndexChanged += ComprasMetMonedaSeleccionada;
        }

        public void ComprasProcInicializar(ClsControladorMoneda Controlador, List<ClsMonedaCompras> Monedas,
            ClsMonedaCompras MonedaLocal)
        {
            _Controlador = Controlador;
            _MonedaLocal = MonedaLocal;
            ComprasCboMoneda.DisplayMember = nameof(ClsMonedaCompras.Descripcion);
            ComprasCboMoneda.ValueMember = nameof(ClsMonedaCompras.IdMoneda);
            ComprasCboMoneda.DataSource = Monedas.ToList();

            ClsMonedaCompras Actual = ClsPreferenciaMonedaCompras.MonedaVisualizacion ?? MonedaLocal;
            if (Actual != null) ComprasCboMoneda.SelectedValue = Actual.IdMoneda;
            ComprasProcMostrarActual();
            ComprasProcMostrarVistaPrevia();
        }

        private void ComprasMetMonedaSeleccionada(object Remitente, EventArgs Evento)
        {
            ComprasProcMostrarVistaPrevia();
        }

        private void ComprasMetAplicar(object Remitente, EventArgs Evento)
        {
            if (!(ComprasCboMoneda.SelectedItem is ClsMonedaCompras Moneda)) return;
            ClsPreferenciaMonedaCompras.ComprasProcEstablecerMoneda(Moneda);
            ComprasProcMostrarActual();
            MessageBox.Show("Los montos se mostrarán en " + Moneda.Nombre + " durante esta sesión.",
                "Moneda de visualización", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComprasMetRestablecer(object Remitente, EventArgs Evento)
        {
            if (_MonedaLocal == null) return;
            ComprasCboMoneda.SelectedValue = _MonedaLocal.IdMoneda;
            ClsPreferenciaMonedaCompras.ComprasProcEstablecerMoneda(_MonedaLocal);
            ComprasProcMostrarActual();
        }

        private void ComprasProcMostrarActual()
        {
            ClsMonedaCompras Actual = ClsPreferenciaMonedaCompras.MonedaVisualizacion ?? _MonedaLocal;
            ComprasLblActual.Texto = Actual == null ? "—" : Actual.Descripcion;
        }

        private void ComprasProcMostrarVistaPrevia()
        {
            if (_Controlador == null || _MonedaLocal == null) return;
            ComprasLblMontoLocal.Texto = ClsControladorMoneda.ComprasFuncFormatearMonto(_MontoEjemplo, _MonedaLocal);

            if (!(ComprasCboMoneda.SelectedItem is ClsMonedaCompras Moneda)) return;
            try
            {
                ClsResultadoConversion Resultado = _Controlador.ComprasFuncConvertir(_MontoEjemplo, _MonedaLocal, Moneda, DateTime.Today);
                ComprasLblMontoConvertido.Texto = Resultado.Exitoso
                    ? ClsControladorMoneda.ComprasFuncFormatearMonto(Resultado.MontoConvertido, Moneda)
                    : "Sin tipo de cambio";
                ComprasLblDetalle.Texto = Resultado.Exitoso
                    ? Resultado.Mensaje
                    : Resultado.Mensaje + " Los montos se seguirán mostrando en " + _MonedaLocal.Nombre + ".";
            }
            catch (InvalidOperationException Error)
            {
                ComprasLblMontoConvertido.Texto = "—";
                ComprasLblDetalle.Texto = Error.Message;
            }
        }
    }
}
