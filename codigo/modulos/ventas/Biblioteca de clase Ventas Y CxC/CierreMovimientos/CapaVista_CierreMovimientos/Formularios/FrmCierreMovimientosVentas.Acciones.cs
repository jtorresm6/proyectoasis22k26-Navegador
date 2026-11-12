/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 10/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Operar el cierre de movimientos desde el formulario.
 */
using CapaControlador_CierreMovimientos;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CapaVista_CierreMovimientos
{
    public partial class FrmCierreMovimientosVentas
    {

        // Acciones del borrador.
        internal void VentasMetConsultarCierre()
        {
            if (this.VentasTbcCierre.SelectedTab != this.VentasTbpPreparar)
            {
                this.VentasMetRevisar();
                return;
            }

            DataRow Fila = VentasFuncSeleccionar(this, "Consultar cierres y borradores", this._Controlador.VentasFuncListar(this.Moneda));
            if (Fila == null)
                return;
            this.Cargando = true;
            try
            {
                this.VentasCboMonedaCierre.SelectedValue = Fila["idMoneda"];
                DateTime Desde = Convert.ToDateTime(Fila["Desde"]);
                this.VentasCboTipoCierre.SelectedIndex = Convert.ToString(Fila["Tipo"]) == "ANUAL" ? 1 : 0;
                this.VentasNudAnioCierre.Value = Desde.Year;
                this.VentasCboMesCierre.SelectedIndex = Desde.Month - 1;
                this.VentasMetPeriodoCambio();
                this.IdBorrador = Convert.ToString(Fila["Estado"]) == "BORRADOR" ? Convert.ToInt32(Fila["Id"]) : 0;
                this.VentasTxtObservacionesCierre.Text = Convert.ToString(Fila["Observaciones"]);
                this.EditandoBorrador = false;
            }
            finally
            {
                this.Cargando = false;
            }

            if (Convert.ToString(Fila["Estado"]) == "ANULADO")
            {
                this.VentasMetLimpiarRevision();
                this.VentasTxtEstadoCierre.Text = "Anulado: " + Fila["Numero"];
            }
            else
                this.VentasMetRevisar();
            VentasMetActualizarAccionesCierre();
        }

        internal void VentasMetModificarBorrador()
        {
            if (this.IdBorrador == 0)
                VentasMetConsultarCierre();
            this._Controlador.VentasMetValidarCaptura(this.IdBorrador > 0, "Seleccione un cierre en borrador. Un cierre confirmado conserva su histórico.");
            this.EditandoBorrador = true;
            VentasMetActualizarAccionesCierre();
            this.VentasTxtObservacionesCierre.Focus();
        }

        internal void VentasMetGuardarBorrador()
        {
            VentasMetPersistirBorrador();
            this.VentasMetRevisar();
            VentasMetInformar(this, "Borrador guardado. Revise los saldos antes de confirmar el cierre.");
        }

        internal void VentasMetPersistirBorrador()
        {
            string Numero = this._Controlador.VentasFuncGuardarBorrador(this.IdBorrador, this.Moneda, this.VentasIdUsuario, this.Tipo, this.VentasDtpDesdeCierre.Value, this.VentasDtpHastaCierre.Value, this.VentasTxtObservacionesCierre.Text);
            this.IdBorrador = Convert.ToInt32(this._Controlador.VentasFuncListar(this.Moneda).AsEnumerable().Single(Registro => Convert.ToString(Registro["Numero"]) == Numero)["Id"]);
            this.EditandoBorrador = false;
            VentasMetActualizarAccionesCierre();
        }

        internal void VentasMetAnularBorrador()
        {
            if (this.IdBorrador == 0)
                VentasMetConsultarCierre();
            this._Controlador.VentasMetValidarCaptura(this.IdBorrador > 0, "Seleccione un borrador para anular. Los cierres confirmados se conservan.");
            string Motivo = VentasFuncMotivo(this, "Anular borrador de cierre");
            if (Motivo == null)
                return;
            this._Controlador.VentasMetEliminarBorrador(this.IdBorrador, this.VentasIdUsuario, Motivo);
            this.IdBorrador = 0;
            this.VentasMetNuevo();
            VentasMetInformar(this, "Borrador anulado.");
        }

        internal void VentasMetActualizarAccionesCierre()
        {
            bool Preparar = this.VentasTbcCierre.SelectedTab == this.VentasTbpPreparar;
            this.VentasBtnGuardarBorrador.Enabled = Preparar && this.Moneda > 0 && this.IdCierre == 0 && this.EditandoBorrador;
            this.VentasBtnModificar.Enabled = this.VentasBtnEliminar.Enabled = Preparar && this.IdBorrador > 0 && this.IdCierre == 0;
            this.VentasTxtObservacionesCierre.ReadOnly = !this.EditandoBorrador || this.IdCierre > 0;
        }

        // ConfirmacionVistaCierre.
        // Natali Sofía Montenegro Portillo | 0901-23-10017 | El usuario confirma el período revisado antes del enlace contable.
        internal void VentasMetConfirmar()
        {
            this._Controlador.VentasMetValidarCaptura(!(this.Revision == null), "Revise el periodo antes de confirmarlo.");
            if (MessageBox.Show(this, "El cierre conservará los saldos y bloqueará registros con fechas de este período. ¿Desea confirmarlo?", "Confirmar cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            if (this.IdBorrador == 0 || this.EditandoBorrador)
                VentasMetPersistirBorrador();
            DateTime PeriodoConfirmado = this.VentasDtpDesdeCierre.Value.Date;
            string Numero = this._Controlador.VentasFuncConfirmar(this.Moneda, this.VentasIdUsuario, this.Tipo, this.VentasDtpDesdeCierre.Value, this.VentasDtpHastaCierre.Value, this.VentasChkRevisionDocumentos.Checked);
            if (this.FechaObjetivoTraslado.HasValue && PeriodoConfirmado < this.FechaObjetivoTraslado.Value)
            {
                DateTime Siguiente = PeriodoConfirmado.AddMonths(1);
                VentasMetIrAlPeriodo(Siguiente);
                VentasMetInformar(this, "Cierre confirmado: " + Numero + ". Ahora revise y confirme " + Siguiente.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-GT")) + ".");
                return;
            }
            this.FechaObjetivoTraslado = null;
            this.VentasMetRevisar();
            this.VentasTbcCierre.SelectedTab = this.VentasTbpContabilidad;
            VentasMetInformar(this, "Cierre confirmado: " + Numero + ". Revise ahora la póliza de este período. Si no hubo movimientos, no habrá póliza que enviar.");
        }

            }
}
