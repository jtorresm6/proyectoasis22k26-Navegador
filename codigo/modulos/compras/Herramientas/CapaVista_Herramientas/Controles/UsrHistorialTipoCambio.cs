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
 *  Muestra cómo se movió el tipo de cambio entre dos monedas en un rango de
 *  fechas, con su mínimo, máximo, promedio y variación. Sirve para decidir
 *  cuándo programar pagos a proveedores en moneda extranjera.
 * ===================================================================
*/

namespace CapaVista_Herramientas.Controles
{
    public partial class UsrHistorialTipoCambio : UserControl
    {
        private const int _DiasPorDefecto = 30;
        private ClsControladorMoneda _Controlador;

        public UsrHistorialTipoCambio()
        {
            InitializeComponent();
            ComprasBtnConsultar.Click += ComprasMetConsultar;
        }

        public void ComprasProcInicializar(ClsControladorMoneda Controlador, List<ClsMonedaCompras> Monedas,
            ClsMonedaCompras MonedaLocal)
        {
            _Controlador = Controlador;
            ComprasProcCargarMonedas(ComprasCboOrigen, Monedas);
            ComprasProcCargarMonedas(ComprasCboDestino, Monedas);

            ClsMonedaCompras Extranjera = Controlador.ComprasFuncObtenerMonedaExtranjeraSugerida(Monedas);
            if (Extranjera != null) ComprasCboOrigen.SelectedValue = Extranjera.IdMoneda;
            if (MonedaLocal != null) ComprasCboDestino.SelectedValue = MonedaLocal.IdMoneda;

            ComprasRngFechas.ComprasProcEstablecerRango(DateTime.Today.AddDays(-_DiasPorDefecto), DateTime.Today);
            ComprasProcConsultar();
        }

        private static void ComprasProcCargarMonedas(CapaVista_Componentes.Componentes.UsrComboBoxCompras Selector,
            List<ClsMonedaCompras> Monedas)
        {
            Selector.DisplayMember = nameof(ClsMonedaCompras.Descripcion);
            Selector.ValueMember = nameof(ClsMonedaCompras.IdMoneda);
            Selector.DataSource = Monedas.ToList();
        }

        private void ComprasMetConsultar(object Remitente, EventArgs Evento)
        {
            ComprasProcConsultar();
        }

        private void ComprasProcConsultar()
        {
            if (_Controlador == null) return;
            var Origen = ComprasCboOrigen.SelectedItem as ClsMonedaCompras;
            var Destino = ComprasCboDestino.SelectedItem as ClsMonedaCompras;

            ClsHistorialTipoCambio Historial;
            try
            {
                Historial = _Controlador.ComprasFuncObtenerHistorial(Origen, Destino,
                    ComprasRngFechas.FechaInicio, ComprasRngFechas.FechaFin);
            }
            catch (InvalidOperationException Error)
            {
                ComprasProcLimpiarResumen();
                ComprasGrfHistorial.ComprasProcMostrar(null, Error.Message);
                return;
            }

            if (!Historial.TieneDatos)
            {
                ComprasProcLimpiarResumen();
                ComprasGrfHistorial.ComprasProcMostrar(null,
                    "Bancos no registró tipos de cambio entre " + Origen.Codigo + " y " + Destino.Codigo + " en ese rango.");
                return;
            }

            ComprasGrfHistorial.ComprasProcMostrar(Historial.Puntos, null);
            ComprasLblPar.Texto = "Valor de 1 " + Origen.Codigo + " en " + Destino.Codigo +
                (Historial.EsInverso ? "   (calculado con el inverso del registro de Bancos)" : string.Empty);
            ComprasLblMinimo.Texto = "Mínimo: " + ClsControladorMoneda.ComprasFuncFormatearTasa(Historial.Minimo);
            ComprasLblMaximo.Texto = "Máximo: " + ClsControladorMoneda.ComprasFuncFormatearTasa(Historial.Maximo);
            ComprasLblPromedio.Texto = "Promedio: " + ClsControladorMoneda.ComprasFuncFormatearTasa(Historial.Promedio);
            ComprasLblVariacion.Texto = "Variación: " + (Historial.VariacionPorcentual > 0 ? "+" : string.Empty) +
                Historial.VariacionPorcentual.ToString("N2") + " %";
        }

        private void ComprasProcLimpiarResumen()
        {
            ComprasLblPar.Texto = "—";
            ComprasLblMinimo.Texto = "Mínimo: —";
            ComprasLblMaximo.Texto = "Máximo: —";
            ComprasLblPromedio.Texto = "Promedio: —";
            ComprasLblVariacion.Texto = "Variación: —";
        }
    }
}
