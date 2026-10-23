// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
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

        internal ClsCierrePresentador(FrmCierreMovimientosVentas Vista)
        {
            this.Vista = Vista;
            BorradorCierre = new ClsBorradorCierre(this);
            VentasMetConectarEventos();
        }

        // EventosCierre.
        internal void VentasMetConectarEventos()
        {
            this.Vista.Load += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetCargar);
            this.Vista.VentasBtnIngresar.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetNuevo);
            this.Vista.VentasBtnRevisar.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.BorradorCierre.VentasMetConsultarCierre);
            this.Vista.VentasBtnGuardarBorrador.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.BorradorCierre.VentasMetGuardarBorrador);
            this.Vista.VentasBtnModificar.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.BorradorCierre.VentasMetModificarBorrador);
            this.Vista.VentasBtnEliminar.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.BorradorCierre.VentasMetAnularBorrador);
            this.Vista.VentasBtnRefrescar.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetRevisar);
            this.Vista.VentasCboClienteHistorial.SelectedIndexChanged += (Remitente, ArgumentosEvento) =>
            {
                if (!this.Cargando && this.Moneda > 0 && this.Vista.VentasTbcCierre.SelectedTab == this.Vista.VentasTbpHistorial)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetHistorial);
            };
            this.Vista.VentasTxtPeriodoHistorial.KeyDown += (Remitente, ArgumentosEvento) =>
            {
                if (ArgumentosEvento.KeyCode == Keys.Enter)
                {
                    ArgumentosEvento.SuppressKeyPress = true;
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetHistorial);
                }
            };
            this.Vista.VentasBtnConfirmar.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, this.BorradorCierre.VentasMetConfirmar);
            this.Vista.VentasBtnCancelar.Click += (Remitente, ArgumentosEvento) => VentasMetLimpiarRevision();
            this.Vista.VentasBtnCerrar.Click += (Remitente, ArgumentosEvento) => this.Vista.Close();
            this.Vista.VentasBtnTrasladarContabilidad.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, () =>
            {
                if (Ventas.Estandarizacion.ClsDialogosVentas.VentasMetConfirmar(this.Vista, "¿Desea registrar esta partida en Contabilidad?", "Traslado contable") != DialogResult.Yes)
                    return;
                string Referencia = this.Controlador.VentasMetTrasladar(this.IdCierre, this.VentasIdUsuario, this.EnlaceContable);
                VentasMetMostrarPartida();
                Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(this.Vista, "Contabilidad confirmo la partida: " + Referencia);
            });
            this.Vista.VentasBtnImprimir.Click += (Remitente, ArgumentosEvento) => ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetImprimirCierre);
            this.Vista.VentasCboMonedaCierre.SelectedIndexChanged += (Remitente, ArgumentosEvento) =>
            {
                if (!this.Cargando)
                {
                    this.IdBorrador = 0;
                    this.EditandoBorrador = true;
                    VentasMetLimpiarRevision();
                    this.BorradorCierre.VentasMetActualizarAccionesCierre();
                }
            };
            this.Vista.VentasChkRevisionDocumentos.CheckedChanged += (Remitente, ArgumentosEvento) => VentasMetActualizarConfirmacion();
            this.Vista.VentasTbcCierre.SelectedIndexChanged += (Remitente, ArgumentosEvento) =>
            {
                if (!this.Cargando && this.Moneda > 0)
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetRevisar);
            };
            this.Vista.VentasDgvHistorialCierre.SelectionChanged += (Remitente, ArgumentosEvento) =>
            {
                if (this.Cargando || this.Vista.VentasDgvHistorialCierre.CurrentRow == null)
                    return;
                var F = this.Vista.VentasDgvHistorialCierre.CurrentRow.Tag as DataRow;
                if (F != null)
                {
                    this.IdCierre = Convert.ToInt32(F["idCierre"]);
                    this.NumeroCierre = Convert.ToString(F["numeroCierre"]);
                    ClsHerramientasVentas.VentasMetEjecutar(this.Vista, VentasMetMostrarPartida);
                }
            };
            this.Vista.VentasDtpDesdeCierre.Enabled = false;
            this.Vista.VentasDtpHastaCierre.Enabled = false;
            this.Vista.VentasDtpCorteCierre.Enabled = false;
            this.Vista.VentasTxtPeriodoHistorial.MaxLength = 7;
            this.Vista.VentasBtnTrasladarContabilidad.Enabled = false;
            this.Vista.VentasLblTrasladoCierre.Text = "La partida se prepara despues del cierre. El registro contable requiere el enlace del modulo de Contabilidad.";
        }

        // RevisionCierre.
        internal void VentasMetCargar()
        {
            this.Cargando = true;
            try
            {
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboUsuarioRegistro, this.Controlador.VentasMetCatalogo("Usuarios"), this.UsuarioInicial);
                ClsHerramientasVentas.VentasMetCargarOpciones(this.Vista.VentasCboMonedaCierre, this.Controlador.VentasMetCatalogo("Monedas"));
                DataTable Clientes = this.Controlador.VentasMetCatalogo("Clientes");
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
            this.Controlador.VentasMetValidarCaptura(!(this.Moneda <= 0), "Seleccione la moneda. Cada moneda se cierra por separado.");
            if (this.Vista.VentasTbcCierre.SelectedTab == this.Vista.VentasTbpHistorial)
            {
                VentasMetHistorial();
                return;
            }

            if (this.Vista.VentasTbcCierre.SelectedTab == this.Vista.VentasTbpContabilidad)
            {
                VentasMetMostrarPartida();
                return;
            }

            DataTable Estado = this.Controlador.VentasMetEstadoPeriodo(this.Moneda, this.Tipo, this.Vista.VentasDtpDesdeCierre.Value, this.Vista.VentasDtpHastaCierre.Value);
            this.IdCierre = Estado.Rows.Count == 0 ? 0 : Convert.ToInt32(Estado.Rows[0]["idCierre"]);
            this.NumeroCierre = Estado.Rows.Count == 0 ? "" : Convert.ToString(Estado.Rows[0]["numeroCierre"]);
            if (this.IdCierre == 0)
                this.Revision = this.Controlador.VentasMetRevisar(this.Moneda, this.Tipo, this.Vista.VentasDtpDesdeCierre.Value, this.Vista.VentasDtpHastaCierre.Value);
            else
            {
                DataTable Historia = this.Controlador.VentasMetHistorial(this.Moneda, 0, this.Vista.VentasDtpDesdeCierre.Value.Year.ToString());
                this.Revision = Historia.Clone();
                foreach (DataRow F in Historia.Rows)
                    if (Convert.ToInt32(F["idCierre"]) == this.IdCierre)
                        this.Revision.ImportRow(F);
            }

            this.Vista.VentasDgvRevisionCierre.Rows.Clear();
            foreach (DataRow F in this.Revision.Rows)
                this.Vista.VentasDgvRevisionCierre.Rows.Add(F["Codigo"], F["Nombre"], F["Anterior"], F["Cargos"], F["Abonos"], F["CargosAcumulados"], F["AbonosAcumulados"], F["Actual"]);
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
            for (int I = 0; I < Columnas.Length; I++)
                Etiquetas[I].Text = this.Revision.AsEnumerable().Sum(F => Convert.ToDecimal(F[Columnas[I]])).ToString("N2", ClsHerramientasVentas.Cultura);
            if (this.IdCierre == 0 && !this.EditandoBorrador)
            {
                DataRow B = this.Controlador.VentasMetListar(this.Moneda).AsEnumerable().FirstOrDefault(F => Convert.ToString(F["Estado"]) == "BORRADOR" && Convert.ToString(F["Tipo"]) == this.Tipo && Convert.ToDateTime(F["Desde"]).Date == this.Vista.VentasDtpDesdeCierre.Value.Date && Convert.ToDateTime(F["Hasta"]).Date == this.Vista.VentasDtpHastaCierre.Value.Date);
                if (B != null)
                {
                    this.IdBorrador = Convert.ToInt32(B["Id"]);
                    this.Vista.VentasTxtObservacionesCierre.Text = Convert.ToString(B["Observaciones"]);
                }
            }

            this.Vista.VentasTxtEstadoCierre.Text = this.IdCierre == 0 ? (this.IdBorrador > 0 ? "Borrador revisado" : "Revisado; sin cerrar") : "Cerrado: " + this.NumeroCierre;
            VentasMetActualizarConfirmacion();
            VentasMetMostrarPartida();
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
            DateTime[] Periodo = this.Controlador.VentasMetPeriodo(Anio, Mes, Anual);
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

        // HistorialCierre.
        internal void VentasMetHistorial()
        {
            this.Cargando = true;
            try
            {
                this.IdCierre = 0;
                this.NumeroCierre = "";
                this.Vista.VentasDgvHistorialCierre.Rows.Clear();
                foreach (DataRow F in this.Controlador.VentasMetHistorial(this.Moneda, ClsHerramientasVentas.VentasFuncObtenerId(this.Vista.VentasCboClienteHistorial), this.Vista.VentasTxtPeriodoHistorial.Text).Rows)
                {
                    int I = this.Vista.VentasDgvHistorialCierre.Rows.Add(F["Periodo"], F["Codigo"], F["Nombre"], F["Anterior"], F["Cargos"], F["Abonos"], F["CargosAcumulados"], F["AbonosAcumulados"], F["Actual"], F["Estado"]);
                    this.Vista.VentasDgvHistorialCierre.Rows[I].Tag = F;
                }
            }
            finally
            {
                this.Cargando = false;
            }

            if (this.Vista.VentasDgvHistorialCierre.CurrentRow != null)
            {
                var F = this.Vista.VentasDgvHistorialCierre.CurrentRow.Tag as DataRow;
                if (F != null)
                {
                    this.IdCierre = Convert.ToInt32(F["idCierre"]);
                    this.NumeroCierre = Convert.ToString(F["numeroCierre"]);
                }
            }

            this.Vista.VentasBtnConfirmar.Enabled = false;
            VentasMetMostrarPartida();
        }

        internal void VentasMetMostrarPartida()
        {
            this.Vista.VentasDgvPoliza.Rows.Clear();
            this.Vista.VentasBtnTrasladarContabilidad.Enabled = false;
            if (this.IdCierre <= 0)
                return;
            this.Partida = this.Controlador.VentasMetPartida(this.IdCierre);
            foreach (DataRow F in this.Partida.Rows)
                this.Vista.VentasDgvPoliza.Rows.Add(F["Cuenta"], F["Descripcion"], F["Debe"], F["Haber"]);
            decimal Debe = this.Partida.AsEnumerable().Sum(F => (decimal)F["Debe"]), Haber = this.Partida.AsEnumerable().Sum(F => (decimal)F["Haber"]);
            this.Vista.VentasLblPolizaTotales.Text = "Debe: " + Debe.ToString("N2", ClsHerramientasVentas.Cultura) + " / Haber: " + Haber.ToString("N2", ClsHerramientasVentas.Cultura);
            this.Vista.VentasTxtReferenciaPoliza.Text = "CXC-" + this.NumeroCierre;
            this.Vista.VentasTxtEstadoTraslado.Text = "Preparada; pendiente de enlace";
            DataTable Estado = this.Controlador.VentasMetEstadoTraslado(this.IdCierre);
            bool Confirmado = Estado.Rows.Count > 0 && (string)Estado.Rows[0]["estado"] == "CONFIRMADO";
            if (Confirmado)
            {
                this.Vista.VentasTxtReferenciaPoliza.Text = Convert.ToString(Estado.Rows[0]["referenciaContable"]);
                this.Vista.VentasTxtFechaTraslado.Text = Convert.ToDateTime(Estado.Rows[0]["fechaConfirmacion"]).ToString("dd/MM/yyyy HH:mm");
                this.Vista.VentasTxtEstadoTraslado.Text = "Confirmado por Contabilidad";
            }

            this.Vista.VentasBtnTrasladarContabilidad.Enabled = !Confirmado && this.EnlaceContable != null && this.Partida.Rows.Count > 0 && !this.Partida.AsEnumerable().Any(F => ((string)F["Cuenta"]).StartsWith("PENDIENTE", StringComparison.Ordinal));
        }

        internal void VentasMetImprimirCierre()
        {
            DataTable Datos = this.Revision;
            string Titulo = "Cierre " + this.NumeroCierre;
            if (this.Vista.VentasTbcCierre.SelectedTab == this.Vista.VentasTbpHistorial)
            {
                Datos = this.Controlador.VentasMetHistorial(this.Moneda, ClsHerramientasVentas.VentasFuncObtenerId(this.Vista.VentasCboClienteHistorial), this.Vista.VentasTxtPeriodoHistorial.Text);
                Titulo = "Historial de cierres";
            }
            else if (this.Vista.VentasTbcCierre.SelectedTab == this.Vista.VentasTbpContabilidad)
            {
                Datos = this.Partida;
                Titulo = "Partida del cierre " + this.NumeroCierre;
            }

            ClsHerramientasVentas.VentasMetImprimir(this.Vista, Titulo, this.Vista.VentasCboMonedaCierre.Text, Datos);
        }

        // EnlaceCierre.
        public void VentasMetConfigurarEnlaceContable(Func<string, int, DateTime, DataTable, string> Registrar)
        {
            this.EnlaceContable = Registrar;
            if (this.IdCierre > 0)
                VentasMetMostrarPartida();
        }
    }
}
