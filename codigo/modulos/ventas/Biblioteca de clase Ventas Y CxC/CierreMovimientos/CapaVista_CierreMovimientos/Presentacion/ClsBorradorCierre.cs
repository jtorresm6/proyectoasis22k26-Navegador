// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using CapaControlador_CierreMovimientos;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_CierreMovimientos
{
    internal sealed class ClsBorradorCierre
    {
        private readonly ClsCierrePresentador _Estado;
        internal ClsBorradorCierre(ClsCierrePresentador Estado)
        {
            _Estado = Estado;
        }

        // BorradorCierre.
        internal void VentasMetConsultarCierre()
        {
            if (_Estado.Vista.VentasTbcCierre.SelectedTab != _Estado.Vista.VentasTbpPreparar)
            {
                _Estado.VentasMetRevisar();
                return;
            }

            DataRow Fila = ClsHerramientasVentas.VentasFuncSeleccionar(_Estado.Vista, "Consultar cierres y borradores", _Estado.Controlador.VentasFuncListar(_Estado.Moneda));
            if (Fila == null)
                return;
            _Estado.Cargando = true;
            try
            {
                _Estado.Vista.VentasCboMonedaCierre.SelectedValue = Fila["idMoneda"];
                DateTime Desde = Convert.ToDateTime(Fila["Desde"]);
                _Estado.Vista.VentasCboTipoCierre.SelectedIndex = Convert.ToString(Fila["Tipo"]) == "ANUAL" ? 1 : 0;
                _Estado.Vista.VentasNudAnioCierre.Value = Desde.Year;
                _Estado.Vista.VentasCboMesCierre.SelectedIndex = Desde.Month - 1;
                _Estado.VentasMetPeriodoCambio();
                _Estado.IdBorrador = Convert.ToString(Fila["Estado"]) == "BORRADOR" ? Convert.ToInt32(Fila["Id"]) : 0;
                _Estado.Vista.VentasTxtObservacionesCierre.Text = Convert.ToString(Fila["Observaciones"]);
                _Estado.EditandoBorrador = false;
            }
            finally
            {
                _Estado.Cargando = false;
            }

            if (Convert.ToString(Fila["Estado"]) == "ANULADO")
            {
                _Estado.VentasMetLimpiarRevision();
                _Estado.Vista.VentasTxtEstadoCierre.Text = "Anulado: " + Fila["Numero"];
            }
            else
                _Estado.VentasMetRevisar();
            VentasMetActualizarAccionesCierre();
        }

        internal void VentasMetModificarBorrador()
        {
            if (_Estado.IdBorrador == 0)
                VentasMetConsultarCierre();
            _Estado.Controlador.VentasMetValidarCaptura(_Estado.IdBorrador > 0, "Seleccione un cierre en borrador. Un cierre confirmado conserva su historico.");
            _Estado.EditandoBorrador = true;
            VentasMetActualizarAccionesCierre();
            _Estado.Vista.VentasTxtObservacionesCierre.Focus();
        }

        internal void VentasMetGuardarBorrador()
        {
            VentasMetPersistirBorrador();
            _Estado.VentasMetRevisar();
            ClsHerramientasVentas.VentasMetInformar(_Estado.Vista, "Borrador guardado. Revise los saldos antes de confirmar el cierre.");
        }

        internal void VentasMetPersistirBorrador()
        {
            string Numero = _Estado.Controlador.VentasFuncGuardarBorrador(_Estado.IdBorrador, _Estado.Moneda, _Estado.VentasIdUsuario, _Estado.Tipo, _Estado.Vista.VentasDtpDesdeCierre.Value, _Estado.Vista.VentasDtpHastaCierre.Value, _Estado.Vista.VentasTxtObservacionesCierre.Text);
            _Estado.IdBorrador = Convert.ToInt32(_Estado.Controlador.VentasFuncListar(_Estado.Moneda).AsEnumerable().Single(Registro => Convert.ToString(Registro["Numero"]) == Numero)["Id"]);
            _Estado.EditandoBorrador = false;
            VentasMetActualizarAccionesCierre();
        }

        internal void VentasMetAnularBorrador()
        {
            if (_Estado.IdBorrador == 0)
                VentasMetConsultarCierre();
            _Estado.Controlador.VentasMetValidarCaptura(_Estado.IdBorrador > 0, "Seleccione un borrador para anular. Los cierres confirmados se conservan.");
            string Motivo = ClsHerramientasVentas.VentasFuncMotivo(_Estado.Vista, "Anular borrador de cierre");
            if (Motivo == null)
                return;
            _Estado.Controlador.VentasMetEliminarBorrador(_Estado.IdBorrador, _Estado.VentasIdUsuario, Motivo);
            _Estado.IdBorrador = 0;
            _Estado.VentasMetNuevo();
            ClsHerramientasVentas.VentasMetInformar(_Estado.Vista, "Borrador anulado.");
        }

        internal void VentasMetActualizarAccionesCierre()
        {
            bool Preparar = _Estado.Vista.VentasTbcCierre.SelectedTab == _Estado.Vista.VentasTbpPreparar;
            _Estado.Vista.VentasBtnGuardarBorrador.Enabled = Preparar && _Estado.Moneda > 0 && _Estado.IdCierre == 0 && _Estado.EditandoBorrador;
            _Estado.Vista.VentasBtnModificar.Enabled = _Estado.Vista.VentasBtnEliminar.Enabled = Preparar && _Estado.IdBorrador > 0 && _Estado.IdCierre == 0;
            _Estado.Vista.VentasTxtObservacionesCierre.ReadOnly = !_Estado.EditandoBorrador || _Estado.IdCierre > 0;
        }

        // ConfirmacionVistaCierre.
        internal void VentasMetConfirmar()
        {
            _Estado.Controlador.VentasMetValidarCaptura(!(_Estado.Revision == null), "Revise el periodo antes de confirmarlo.");
            if (MessageBox.Show(_Estado.Vista, "El cierre conservara los saldos y bloqueara registros con fechas de este periodo. ¿Desea confirmarlo?", "Confirmar cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            if (_Estado.IdBorrador == 0 || _Estado.EditandoBorrador)
                VentasMetPersistirBorrador();
            string Numero = _Estado.Controlador.VentasFuncConfirmar(_Estado.Moneda, _Estado.VentasIdUsuario, _Estado.Tipo, _Estado.Vista.VentasDtpDesdeCierre.Value, _Estado.Vista.VentasDtpHastaCierre.Value, _Estado.Vista.VentasChkRevisionDocumentos.Checked);
            _Estado.VentasMetRevisar();
            ClsHerramientasVentas.VentasMetInformar(_Estado.Vista, "Cierre confirmado: " + Numero + ". Se conservaron los saldos historicos por cliente y factura.");
        }
    }
}
