// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using CapaControlador_CierreMovimientos;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_CierreMovimientos
{
    internal sealed class ClsEventosCierre
    {
        private readonly ClsCierrePresentador _Estado;

        internal ClsEventosCierre(ClsCierrePresentador Estado)
        {
            _Estado = Estado;
        }

        internal void VentasMetConectarEventos()
        {
            foreach (var Grid in new[] { _Estado.Vista.VentasDgvRevisionCierre, _Estado.Vista.VentasDgvHistorialCierre, _Estado.Vista.VentasDgvPoliza })
                ClsHerramientasVentas.VentasMetFormatearFechas(Grid);
            _Estado.Vista.Load += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.VentasMetCargar);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnIngresar, _Estado.VentasMetNuevo);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnRevisar, _Estado.BorradorCierre.VentasMetConsultarCierre);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnGuardarBorrador, _Estado.BorradorCierre.VentasMetGuardarBorrador);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnModificar, _Estado.BorradorCierre.VentasMetModificarBorrador);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnEliminar, _Estado.BorradorCierre.VentasMetAnularBorrador);
            VentasMetConectarBoton(_Estado.Vista.VentasBtnRefrescar, _Estado.VentasMetRevisar);
            _Estado.Vista.VentasCboClienteHistorial.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!_Estado.Cargando && _Estado.Moneda > 0 && _Estado.Vista.VentasTbcCierre.SelectedTab == _Estado.Vista.VentasTbpHistorial)
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.Consultas.VentasMetHistorial);
            };
            _Estado.Vista.VentasTxtPeriodoHistorial.KeyDown += (Origen, Evento) =>
            {
                if (Evento.KeyCode == Keys.Enter)
                {
                    Evento.SuppressKeyPress = true;
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.Consultas.VentasMetHistorial);
                }
            };
            VentasMetConectarBoton(_Estado.Vista.VentasBtnConfirmar, _Estado.BorradorCierre.VentasMetConfirmar);
            _Estado.Vista.VentasBtnCancelar.Click += (Origen, Evento) => _Estado.VentasMetLimpiarRevision();
            _Estado.Vista.VentasBtnCerrar.Click += (Origen, Evento) => _Estado.Vista.Close();
            _Estado.Vista.VentasBtnTrasladarContabilidad.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, () =>
            {
                if (MessageBox.Show(_Estado.Vista, "¿Desea registrar esta partida en Contabilidad?", "Traslado contable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                string Referencia = _Estado.Controlador.VentasFuncTrasladar(_Estado.IdCierre, _Estado.VentasIdUsuario, _Estado.EnlaceContable);
                _Estado.Consultas.VentasMetMostrarPartida();
                ClsHerramientasVentas.VentasMetInformar(_Estado.Vista, "Contabilidad confirmo la partida: " + Referencia);
            });
            VentasMetConectarBoton(_Estado.Vista.VentasBtnImprimir, _Estado.Consultas.VentasMetImprimirCierre);
            _Estado.Vista.VentasCboMonedaCierre.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!_Estado.Cargando)
                {
                    _Estado.IdBorrador = 0;
                    _Estado.EditandoBorrador = true;
                    _Estado.VentasMetLimpiarRevision();
                    _Estado.BorradorCierre.VentasMetActualizarAccionesCierre();
                }
            };
            _Estado.Vista.VentasChkRevisionDocumentos.CheckedChanged += (Origen, Evento) => _Estado.VentasMetActualizarConfirmacion();
            _Estado.Vista.VentasTbcCierre.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!_Estado.Cargando && _Estado.Moneda > 0)
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.VentasMetRevisar);
            };
            _Estado.Vista.VentasDgvHistorialCierre.SelectionChanged += (Origen, Evento) =>
            {
                if (_Estado.Cargando || _Estado.Vista.VentasDgvHistorialCierre.CurrentRow == null)
                    return;
                var Fila = _Estado.Vista.VentasDgvHistorialCierre.CurrentRow.Tag as DataRow;
                if (Fila != null)
                {
                    _Estado.IdCierre = Convert.ToInt32(Fila["idCierre"]);
                    _Estado.NumeroCierre = Convert.ToString(Fila["numeroCierre"]);
                    ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, _Estado.Consultas.VentasMetMostrarPartida);
                }
            };
            _Estado.Vista.VentasDtpDesdeCierre.Enabled = false;
            _Estado.Vista.VentasDtpHastaCierre.Enabled = false;
            _Estado.Vista.VentasDtpCorteCierre.Enabled = false;
            _Estado.Vista.VentasTxtPeriodoHistorial.MaxLength = 7;
            _Estado.Vista.VentasBtnTrasladarContabilidad.Enabled = false;
            _Estado.Vista.VentasLblTrasladoCierre.Text = "La partida se prepara despues del cierre. El registro contable requiere el enlace del modulo de Contabilidad.";
        }
        private void VentasMetConectarBoton(Button Boton, Action Accion)
        {
            Boton.Click += (Origen, Evento) => ClsHerramientasVentas.VentasMetEjecutar(_Estado.Vista, Accion);
        }
    }
}
