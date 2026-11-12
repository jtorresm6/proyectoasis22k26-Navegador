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

        internal void VentasMetIrAlSiguientePeriodo()
        {
            DateTime Objetivo = this.VentasDtpDesdeCierre.Value.Date;
            DateTime Siguiente = this._Controlador.VentasFuncSiguientePeriodo(this.Moneda, Objetivo);
            this._Controlador.VentasMetValidarCaptura(Siguiente < Objetivo, "Este es el siguiente período pendiente. Revise los saldos y confirme el cierre antes de enviar la póliza.");
            this.FechaObjetivoTraslado = Objetivo;
            VentasMetIrAlPeriodo(Siguiente);
        }

        internal void VentasMetIrAlPeriodo(DateTime Fecha)
        {
            this.Cargando = true;
            try
            {
                this.VentasTbcCierre.SelectedTab = this.VentasTbpPreparar;
                this.VentasCboTipoCierre.SelectedIndex = 0;
                this.VentasNudAnioCierre.Value = Fecha.Year;
                this.VentasCboMesCierre.SelectedIndex = Fecha.Month - 1;
                VentasMetPeriodoCambio();
            }
            finally
            {
                this.Cargando = false;
            }
            this.IdBorrador = 0;
            this.EditandoBorrador = false;
            this.VentasMetLimpiarRevision();
            VentasMetRevisar();
        }

        // Natali Sofía Montenegro Portillo | 0901-23-10017 | El período visible determina la póliza, sin selección obligatoria en el histórico.
        internal void VentasMetSeleccionarCierrePeriodo()
        {
            DataRow Cierre = this._Controlador.VentasFuncListar(this.Moneda).AsEnumerable()
                .Where(Fila => Convert.ToString(Fila["Tipo"]) == this.Tipo
                    && Convert.ToDateTime(Fila["Desde"]).Date == this.VentasDtpDesdeCierre.Value.Date
                    && Convert.ToDateTime(Fila["Hasta"]).Date == this.VentasDtpHastaCierre.Value.Date
                    && Convert.ToString(Fila["Estado"]) != "ANULADO")
                .OrderByDescending(Fila => Convert.ToString(Fila["Estado"]) == "CONFIRMADO")
                .FirstOrDefault();
            this.IdCierre = Cierre == null ? 0 : Convert.ToInt32(Cierre["Id"]);
            this.NumeroCierre = Cierre == null ? "" : Convert.ToString(Cierre["Numero"]);
        }

        internal void VentasMetElegirCierreHistorial(DataRow Fila)
        {
            DataRow Cierre = this._Controlador.VentasFuncListar(this.Moneda).AsEnumerable()
                .FirstOrDefault(Registro => Convert.ToInt32(Registro["Id"]) == Convert.ToInt32(Fila["idCierre"]));
            if (Cierre == null)
                return;
            bool CargandoAntes = this.Cargando;
            this.Cargando = true;
            try
            {
                DateTime Desde = Convert.ToDateTime(Cierre["Desde"]);
                this.VentasCboTipoCierre.SelectedIndex = Convert.ToString(Cierre["Tipo"]) == "ANUAL" ? 1 : 0;
                this.VentasNudAnioCierre.Value = Desde.Year;
                this.VentasCboMesCierre.SelectedIndex = Desde.Month - 1;
                VentasMetPeriodoCambio();
            }
            finally
            {
                this.Cargando = CargandoAntes;
            }
            this.IdCierre = Convert.ToInt32(Cierre["Id"]);
            this.NumeroCierre = Convert.ToString(Cierre["Numero"]);
        }

        internal void VentasMetHistorial()
        {
            this.Cargando = true;
            try
            {
                this.IdCierre = 0;
                this.NumeroCierre = "";
                this.VentasDgvHistorialCierre.Rows.Clear();
                foreach (DataRow Fila in this._Controlador.VentasFuncHistorial(this.Moneda, VentasFuncObtenerId(this.VentasCboClienteHistorial), this.VentasTxtPeriodoHistorial.Text).Rows)
                {
                    int IndiceFila = this.VentasDgvHistorialCierre.Rows.Add(Fila["Periodo"], Fila["Codigo"], Fila["Nombre"], Fila["Anterior"], Fila["Cargos"], Fila["Abonos"], Fila["CargosAcumulados"], Fila["AbonosAcumulados"], Fila["Actual"], Fila["Estado"]);
                    this.VentasDgvHistorialCierre.Rows[IndiceFila].Tag = Fila;
                }
            }
            finally
            {
                this.Cargando = false;
            }

            if (this.VentasDgvHistorialCierre.CurrentRow != null)
            {
                var Fila2 = this.VentasDgvHistorialCierre.CurrentRow.Tag as DataRow;
                if (Fila2 != null)
                {
                    this.IdCierre = Convert.ToInt32(Fila2["idCierre"]);
                    this.NumeroCierre = Convert.ToString(Fila2["numeroCierre"]);
                }
            }

            this.VentasBtnConfirmar.Enabled = false;
            VentasMetMostrarPartida();
        }

        internal void VentasMetMostrarPartida()
        {
            if (this.VentasTbcCierre.SelectedTab == this.VentasTbpContabilidad)
                VentasMetSeleccionarCierrePeriodo();
            this.VentasBtnIrSiguientePeriodo.Visible = false;
            DateTime Desde = this.VentasDtpDesdeCierre.Value.Date;
            DateTime Siguiente = this._Controlador.VentasFuncSiguientePeriodo(this.Moneda, Desde);
            bool FaltanMeses = this.Tipo == "MENSUAL" && Siguiente < Desde;
            string MesPendiente = Siguiente.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-GT"));
            this.VentasDgvPoliza.Rows.Clear();
            this.VentasTxtReferenciaPoliza.Clear();
            this.VentasTxtFechaTraslado.Clear();
            // Natali Sofía Montenegro Portillo | 0901-23-10017 | El botón explica el requisito faltante al pulsarlo.
            this.VentasBtnTrasladarContabilidad.Enabled = this.EnlaceContable != null;
            this.Partida = null;
            this.VentasLblPolizaTotales.Text = "Debe: 0.00 / Haber: 0.00";
            if (this.IdCierre <= 0)
            {
                this.VentasTxtReferenciaPoliza.Clear();
                this.VentasTxtFechaTraslado.Clear();
                this.VentasTxtEstadoTraslado.Text = "Sin cierre para el período elegido";
                this.VentasLblPolizaRegla.Text = "Período: " + this.VentasDtpDesdeCierre.Value.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-GT")) + ". Todavía no existe una póliza.";
                this.VentasLblTrasladoCierre.Text = FaltanMeses
                    ? "Antes de este período debe confirmar " + MesPendiente + " y los meses siguientes. Pulse el botón para empezar; cada cierre requiere su revisión y confirmación."
                    : "Paso 1: abra Revisar y confirmar, marque la constancia y confirme el cierre. Después vuelva aquí para enviar la póliza.";
                this.VentasBtnIrSiguientePeriodo.Visible = FaltanMeses;
                this.VentasBtnIrSiguientePeriodo.Text = "Ir a " + MesPendiente;
                return;
            }
            bool CierreConfirmado = this._Controlador.VentasFuncEstadoCierre(this.IdCierre) == "CONFIRMADO";
            this.Partida = this._Controlador.VentasFuncPartida(this.IdCierre);
            foreach (DataRow Fila in this.Partida.Rows)
                this.VentasDgvPoliza.Rows.Add(Fila["Cuenta"], Fila["Descripcion"], Fila["Debe"], Fila["Haber"]);
            decimal Debe = this.Partida.AsEnumerable().Sum(Registro => (decimal)Registro["Debe"]), Haber = this.Partida.AsEnumerable().Sum(Registro => (decimal)Registro["Haber"]);
            this.VentasLblPolizaTotales.Text = "Debe: " + Debe.ToString("N2", Cultura) + " / Haber: " + Haber.ToString("N2", Cultura);
            this.VentasLblPolizaRegla.Text = "Cierre " + this.NumeroCierre + " · " + this.VentasDtpDesdeCierre.Value.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-GT")) + ". Revise las cuentas y que Debe y Haber sean iguales.";
            this.VentasTxtEstadoTraslado.Text = CierreConfirmado ? "Cierre confirmado; póliza pendiente" : FaltanMeses ? "Borrador: faltan cierres previos" : "Borrador: confirme el cierre";
            this.VentasLblTrasladoCierre.Text = CierreConfirmado
                ? this.Partida.Rows.Count == 0
                    ? "Este cierre está confirmado, pero no tuvo movimientos. No hay póliza que enviar."
                    : "Paso 3: elija el país, revise que Debe y Haber coincidan y pulse Enviar póliza. El código aparecerá arriba cuando Contabilidad la acepte."
                : FaltanMeses
                    ? "Vista previa del cierre en borrador. Para enviar, confirme primero " + MesPendiente + " y los meses siguientes. Pulse el botón para empezar."
                    : "Esta partida es una vista previa: el cierre sigue en borrador. Abra Revisar y confirmar, marque la constancia y confirme.";
            this.VentasBtnIrSiguientePeriodo.Visible = !CierreConfirmado && FaltanMeses;
            this.VentasBtnIrSiguientePeriodo.Text = "Ir a " + MesPendiente;
            DataTable Estado = this._Controlador.VentasFuncEstadoTraslado(this.IdCierre);
            bool Confirmado = Estado.Rows.Count > 0 && (string)Estado.Rows[0]["estado"] == "CONFIRMADO";
            if (Confirmado)
            {
                this.VentasTxtReferenciaPoliza.Text = Convert.ToString(Estado.Rows[0]["referenciaContable"]);
                this.VentasTxtFechaTraslado.Text = Convert.ToDateTime(Estado.Rows[0]["fechaConfirmacion"]).ToString("dd/MM/yyyy HH:mm");
                this.VentasTxtEstadoTraslado.Text = "Confirmado por Contabilidad";
                this.VentasLblTrasladoCierre.Text = "Póliza enviada. El código y la fecha de confirmación aparecen arriba; este cierre no necesita otro envío.";
            }

            this.VentasBtnTrasladarContabilidad.Enabled = this.EnlaceContable != null && CierreConfirmado && !Confirmado && this.Partida.Rows.Count > 0;
            if (this.EnlaceContable == null)
                this.VentasLblTrasladoCierre.Text = "Prueba local: el cierre funciona sin Contabilidad. El envío de pólizas se habilitará al integrar su DLL.";
        }

        internal void VentasMetImprimirCierre()
        {
            DataTable Datos = this.Revision;
            string Titulo = "Cierre " + this.NumeroCierre;
            if (this.VentasTbcCierre.SelectedTab == this.VentasTbpHistorial)
            {
                Datos = this._Controlador.VentasFuncHistorial(this.Moneda, VentasFuncObtenerId(this.VentasCboClienteHistorial), this.VentasTxtPeriodoHistorial.Text);
                Titulo = "Historial de cierres";
            }
            else if (this.VentasTbcCierre.SelectedTab == this.VentasTbpContabilidad)
            {
                Datos = this.Partida;
                Titulo = "Partida del cierre " + this.NumeroCierre;
            }

            if (Datos == null || Datos.Rows.Count == 0)
            {
                MessageBox.Show(this, "No hay registros para imprimir en esta sección.", "Cierre de movimientos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var Reporte = new FrmReporteCierreMovimientos(Datos, Titulo,
                this.VentasDtpDesdeCierre.Value.Date, this.VentasDtpHastaCierre.Value.Date,
                this.VentasCboMonedaCierre.Text, this.VentasTbcCierre.SelectedTab == this.VentasTbpContabilidad))
                Reporte.ShowDialog(this);
        }

        public void VentasMetConfigurarEnlaceContable(Func<string, int, DateTime, DataTable, string> Registrar)
        {
            this.EnlaceContable = Registrar;
            if (this.IdCierre > 0)
                VentasMetMostrarPartida();
        }

            }
}
