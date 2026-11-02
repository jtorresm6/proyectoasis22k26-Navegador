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
 *  Convierte un monto entre dos monedas con el tipo de cambio que Bancos tenía
 *  registrado en la fecha elegida, e indica cómo se obtuvo (directo, inverso o
 *  cruzado a través de la moneda local).
 * ===================================================================
*/

namespace CapaVista_Herramientas.Controles
{
    public partial class UsrConversorMoneda : UserControl
    {
        private ClsControladorMoneda _Controlador;

        public UsrConversorMoneda()
        {
            InitializeComponent();
            ComprasBtnConvertir.Click += ComprasMetConvertir;
            ComprasBtnIntercambiar.Click += ComprasMetIntercambiar;
            ComprasTxtMonto.TextoKeyDown += ComprasMetMontoKeyDown;
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
            ComprasDtpFecha.Value = DateTime.Today;
            ComprasProcLimpiarResultado();
        }

        private static void ComprasProcCargarMonedas(CapaVista_Componentes.Componentes.UsrComboBoxCompras Selector,
            List<ClsMonedaCompras> Monedas)
        {
            // Cada selector recibe su propia lista para que elegir en uno no cambie el otro.
            Selector.DisplayMember = nameof(ClsMonedaCompras.Descripcion);
            Selector.ValueMember = nameof(ClsMonedaCompras.IdMoneda);
            Selector.DataSource = Monedas.ToList();
        }

        private void ComprasMetMontoKeyDown(object Remitente, KeyEventArgs Evento)
        {
            if (Evento.KeyCode != Keys.Enter) return;
            Evento.SuppressKeyPress = true;
            ComprasProcConvertir();
        }

        private void ComprasMetConvertir(object Remitente, EventArgs Evento)
        {
            ComprasProcConvertir();
        }

        private void ComprasMetIntercambiar(object Remitente, EventArgs Evento)
        {
            object Origen = ComprasCboOrigen.SelectedValue;
            ComprasCboOrigen.SelectedValue = ComprasCboDestino.SelectedValue;
            ComprasCboDestino.SelectedValue = Origen;
            if (!string.IsNullOrWhiteSpace(ComprasTxtMonto.Text)) ComprasProcConvertir();
        }

        private void ComprasProcConvertir()
        {
            if (_Controlador == null) return;
            ComprasTxtMonto.ComprasMetLimpiarError();

            ClsResultadoConversion Resultado;
            try
            {
                Resultado = _Controlador.ComprasFuncConvertir(ComprasTxtMonto.Text,
                    ComprasCboOrigen.SelectedItem as ClsMonedaCompras,
                    ComprasCboDestino.SelectedItem as ClsMonedaCompras,
                    ComprasDtpFecha.Value);
            }
            catch (InvalidOperationException Error)
            {
                MessageBox.Show(Error.Message, "Conversor de montos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Resultado.Exitoso)
            {
                ComprasProcLimpiarResultado();
                ComprasLblEquivalencia.Texto = Resultado.Mensaje;
                if (Resultado.MontoOrigen == 0) ComprasTxtMonto.ComprasMetMostrarError(Resultado.Mensaje);
                return;
            }

            ComprasLblResultado.Texto = ClsControladorMoneda.ComprasFuncFormatearMonto(Resultado.MontoConvertido, Resultado.MonedaDestino);
            ComprasLblEquivalencia.Texto = Resultado.Mensaje;
            ComprasLblMetodo.Texto = "Método: " + Resultado.Metodo;
            ComprasLblFechaTipoCambio.Texto = "Tipo de cambio del: " +
                (Resultado.FechaTipoCambio.HasValue ? Resultado.FechaTipoCambio.Value.ToString("dd/MM/yyyy") : "—");
            ComprasLblFuente.Texto = "Fuente: " + Resultado.Fuente;
        }

        private void ComprasProcLimpiarResultado()
        {
            ComprasLblResultado.Texto = "—";
            ComprasLblEquivalencia.Texto = "Ingrese un monto y presione Convertir.";
            ComprasLblMetodo.Texto = "Método: —";
            ComprasLblFechaTipoCambio.Texto = "Tipo de cambio del: —";
            ComprasLblFuente.Texto = "Fuente: —";
        }
    }
}
