// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using CapaControlador_CierreMovimientos;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_CierreMovimientos
{
    internal sealed class ClsCierrePresentador
    {
        internal ClsConsultasCierre Consultas { get; }

        internal ClsCierrePresentador(FrmCierreMovimientosVentas Vista)
        {
            this.Vista = Vista;
            BorradorCierre = new ClsBorradorCierre(this);
            Consultas = new ClsConsultasCierre(this);
            new ClsEventosCierre(this).VentasMetConectarEventos();
        }

        internal FrmCierreMovimientosVentas Vista { get; set; }

        internal ClsCierreControlador Controlador { get; set; } = new ClsCierreControlador();

        internal bool Cargando { get; set; } = true;

        internal int UsuarioInicial { get; set; }

        internal int IdCierre { get; set; }

        internal DataTable Revision { get; set; }

        internal DataTable Partida { get; set; }

        internal string NumeroCierre { get; set; } = "";

        internal Func<string, int, DateTime, DataTable, string> EnlaceContable { get; set; }

        internal int IdBorrador { get; set; }

        internal bool EditandoBorrador { get; set; } = true;

        public int VentasIdUsuario
        {
            get
            {
                return UsuarioInicial > 0 ? UsuarioInicial : ClsHerramientasVentas.VentasFuncObtenerId(Vista.VentasCboUsuarioRegistro);
            }

            set
            {
                UsuarioInicial = value;
                Vista.VentasCboUsuarioRegistro.Enabled = value <= 0;
                if (Vista.VentasCboUsuarioRegistro.DataSource != null)
                    Vista.VentasCboUsuarioRegistro.SelectedValue = value;
            }
        }

        internal int Moneda
        {
            get
            {
                return ClsHerramientasVentas.VentasFuncObtenerId(Vista.VentasCboMonedaCierre);
            }
        }

        internal string Tipo
        {
            get
            {
                return Vista.VentasCboTipoCierre.SelectedIndex == 1 ? "ANUAL" : "MENSUAL";
            }
        }

        internal ClsBorradorCierre BorradorCierre { get; private set; }

        internal void VentasMetCargar()
        {
            this.Cargando = true;
            try
            {
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboUsuarioRegistro, this.Controlador.VentasFuncCatalogo("Usuarios"), this.UsuarioInicial);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboMonedaCierre, this.Controlador.VentasFuncCatalogo("Monedas"));
                DataTable Clientes = this.Controlador.VentasFuncCatalogo("Clientes");
                DataRow Todos = Clientes.NewRow();
                Todos["Id"] = 0;
                Todos["Nombre"] = "Todos los clientes";
                Clientes.Rows.InsertAt(Todos, 0);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboClienteHistorial, Clientes);
                this.Vista.VentasCboClienteHistorial.SelectedValue = 0;
            }
            finally
            {
                this.Cargando = false;
            }

            VentasMetLimpiarRevision();
        }

        internal void VentasMetNuevo()
        {
            this.IdBorrador = 0;
            this.EditandoBorrador = true;
            this.Vista.VentasTxtObservacionesCierre.Clear();
            this.Cargando = true;
            try
            {
                this.Vista.VentasTbcCierre.SelectedTab = this.Vista.VentasTbpPreparar;
                this.Vista.VentasNudAnioCierre.Value = DateTime.Today.Year;
                this.Vista.VentasCboMesCierre.SelectedIndex = DateTime.Today.Month - 1;
                this.Vista.VentasCboTipoCierre.SelectedIndex = 0;
            }
            finally
            {
                this.Cargando = false;
            }

            VentasMetPeriodoCambio();
            this.Vista.VentasCboMonedaCierre.Focus();
            this.BorradorCierre.VentasMetActualizarAccionesCierre();
        }

        internal void VentasMetLimpiarRevision()
        {
            this.IdCierre = 0;
            this.Revision = null;
            this.Partida = null;
            this.NumeroCierre = "";
            this.Vista.VentasDgvRevisionCierre.Rows.Clear();
            this.Vista.VentasDgvPoliza.Rows.Clear();
            this.Vista.VentasChkRevisionDocumentos.Checked = false;
            this.Vista.VentasBtnConfirmar.Enabled = false;
            this.Vista.VentasTxtEstadoCierre.Text = "Pendiente de revision";
            this.Vista.VentasTxtReferenciaPoliza.Clear();
            this.Vista.VentasTxtFechaTraslado.Clear();
            this.Vista.VentasTxtEstadoTraslado.Text = "Sin traslado contable";
            this.Vista.VentasLblValorAnteriorCierre.Text = this.Vista.VentasLblValorCargosCierre.Text = this.Vista.VentasLblValorAbonosCierre.Text = this.Vista.VentasLblValorCargosAcumulados.Text = this.Vista.VentasLblValorAbonosAcumulados.Text = this.Vista.VentasLblValorActualCierre.Text = "0.00";
        }

        internal void VentasMetRevisar()
        {
            this.Vista.VentasBtnConfirmar.Enabled = false;
            this.Controlador.VentasMetValidarCaptura(this.Moneda > 0, "Seleccione la moneda. Cada moneda se cierra por separado.");
            if (this.Vista.VentasTbcCierre.SelectedTab == this.Vista.VentasTbpHistorial)
            {
                Consultas.VentasMetHistorial();
                return;
            }

            if (this.Vista.VentasTbcCierre.SelectedTab == this.Vista.VentasTbpContabilidad)
            {
                Consultas.VentasMetMostrarPartida();
                return;
            }

            DataTable Estado = this.Controlador.VentasFuncEstadoPeriodo(this.Moneda, this.Tipo, this.Vista.VentasDtpDesdeCierre.Value, this.Vista.VentasDtpHastaCierre.Value);
            this.IdCierre = Estado.Rows.Count == 0 ? 0 : Convert.ToInt32(Estado.Rows[0]["idCierre"]);
            this.NumeroCierre = Estado.Rows.Count == 0 ? "" : Convert.ToString(Estado.Rows[0]["numeroCierre"]);
            if (this.IdCierre == 0)
                this.Revision = this.Controlador.VentasFuncRevisar(this.Moneda, this.Tipo, this.Vista.VentasDtpDesdeCierre.Value, this.Vista.VentasDtpHastaCierre.Value);
            else
            {
                DataTable Historia = this.Controlador.VentasFuncHistorial(this.Moneda, 0, this.Vista.VentasDtpDesdeCierre.Value.Year.ToString());
                this.Revision = Historia.Clone();
                foreach (DataRow Fila in Historia.Rows)
                    if (Convert.ToInt32(Fila["idCierre"]) == this.IdCierre)
                        this.Revision.ImportRow(Fila);
            }

            this.Vista.VentasDgvRevisionCierre.Rows.Clear();
            foreach (DataRow Fila2 in this.Revision.Rows)
                this.Vista.VentasDgvRevisionCierre.Rows.Add(Fila2["Codigo"], Fila2["Nombre"], Fila2["Anterior"], Fila2["Cargos"], Fila2["Abonos"], Fila2["CargosAcumulados"], Fila2["AbonosAcumulados"], Fila2["Actual"]);
            var Etiquetas = new[]
            {
                this.Vista.VentasLblValorAnteriorCierre,
                this.Vista.VentasLblValorCargosCierre,
                this.Vista.VentasLblValorAbonosCierre,
                this.Vista.VentasLblValorCargosAcumulados,
                this.Vista.VentasLblValorAbonosAcumulados,
                this.Vista.VentasLblValorActualCierre
            };
            string[] Columnas =
            {
                "Anterior",
                "Cargos",
                "Abonos",
                "CargosAcumulados",
                "AbonosAcumulados",
                "Actual"
            };
            for (int IndiceFila = 0; IndiceFila < Columnas.Length; IndiceFila++)
                Etiquetas[IndiceFila].Text = this.Revision.AsEnumerable().Sum(Registro => Convert.ToDecimal(Registro[Columnas[IndiceFila]])).ToString("N2", ClsHerramientasVentas.Cultura);
            if (this.IdCierre == 0 && !this.EditandoBorrador)
            {
                DataRow Borrador = this.Controlador.VentasFuncListar(this.Moneda).AsEnumerable().FirstOrDefault(Registro => Convert.ToString(Registro["Estado"]) == "BORRADOR" && Convert.ToString(Registro["Tipo"]) == this.Tipo && Convert.ToDateTime(Registro["Desde"]).Date == this.Vista.VentasDtpDesdeCierre.Value.Date && Convert.ToDateTime(Registro["Hasta"]).Date == this.Vista.VentasDtpHastaCierre.Value.Date);
                if (Borrador != null)
                {
                    this.IdBorrador = Convert.ToInt32(Borrador["Id"]);
                    this.Vista.VentasTxtObservacionesCierre.Text = Convert.ToString(Borrador["Observaciones"]);
                }
            }

            this.Vista.VentasTxtEstadoCierre.Text = this.IdCierre == 0 ? (this.IdBorrador > 0 ? "Borrador revisado" : "Revisado; sin cerrar") : "Cerrado: " + this.NumeroCierre;
            VentasMetActualizarConfirmacion();
            Consultas.VentasMetMostrarPartida();
        }

        internal void VentasMetActualizarConfirmacion()
        {
            this.BorradorCierre.VentasMetActualizarAccionesCierre();
            this.Vista.VentasBtnConfirmar.Enabled = this.Revision != null && this.IdCierre == 0 && this.Vista.VentasChkRevisionDocumentos.Checked && this.Vista.VentasTbcCierre.SelectedTab == this.Vista.VentasTbpPreparar && this.Vista.VentasDtpHastaCierre.Value.Date < DateTime.Today;
        }

        internal void VentasMetPeriodoCambio()
        {
            // Mantiene el período completo; la revisión anterior queda invalidada al cambiarlo.
            bool Anual = this.Vista.VentasCboTipoCierre.Text == "Anual";
            int Anio = (int)this.Vista.VentasNudAnioCierre.Value;
            int Mes = this.Vista.VentasCboMesCierre.SelectedIndex < 0 ? DateTime.Today.Month : this.Vista.VentasCboMesCierre.SelectedIndex + 1;
            DateTime[] Periodo = this.Controlador.VentasFuncPeriodo(Anio, Mes, Anual);
            DateTime Desde = Periodo[0], Hasta = Periodo[1];
            this.Vista.VentasCboMesCierre.Enabled = !Anual;
            this.Vista.VentasDtpDesdeCierre.Value = Desde;
            this.Vista.VentasDtpHastaCierre.Value = Hasta;
            this.Vista.VentasDtpCorteCierre.Value = Hasta;
            string Unidad = Anual ? "año" : "mes";
            this.Vista.VentasClmRevisionCierreCargos.HeaderText = "Cargos del " + Unidad;
            this.Vista.VentasClmRevisionCierreAbonos.HeaderText = "Abonos del " + Unidad;
            this.Vista.VentasLblTituloCargosCierre.Text = "Cargos del " + Unidad;
            this.Vista.VentasLblTituloAbonosCierre.Text = "Abonos del " + Unidad;
            if (!this.Cargando)
            {
                if (!this.EditandoBorrador)
                {
                    this.IdBorrador = 0;
                    this.EditandoBorrador = true;
                    this.Vista.VentasTxtObservacionesCierre.Clear();
                }

                VentasMetLimpiarRevision();
                this.BorradorCierre.VentasMetActualizarAccionesCierre();
            }
        }
    }
}
