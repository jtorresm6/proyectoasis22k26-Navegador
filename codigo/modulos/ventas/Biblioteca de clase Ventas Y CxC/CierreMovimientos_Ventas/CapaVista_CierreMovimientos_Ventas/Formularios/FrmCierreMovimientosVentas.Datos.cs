/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 10/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Operar el cierre de movimientos desde el formulario.
 */
using CapaControlador_CierreMovimientos_Ventas;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CapaVista_CierreMovimientos_Ventas
{
    public partial class FrmCierreMovimientosVentas
    {




        private readonly ClsCierreControlador _Controlador = new ClsCierreControlador();

        internal bool Cargando { get; set; } = true;

        internal int UsuarioInicial { get; set; }

        internal int IdCierre { get; set; }

        internal DataTable Revision { get; set; }

        internal DataTable Partida { get; set; }

        internal string NumeroCierre { get; set; } = "";

        internal DateTime? FechaObjetivoTraslado { get; set; }

        internal Func<string, int, DateTime, DataTable, string> EnlaceContable { get; set; }

        internal int IdBorrador { get; set; }

        internal bool EditandoBorrador { get; set; } = true;

        public int VentasIdUsuario
        {
            get
            {
                return UsuarioInicial > 0 ? UsuarioInicial : VentasFuncObtenerId(VentasCboUsuarioRegistro);
            }

            set
            {
                UsuarioInicial = value;
                VentasCboUsuarioRegistro.Enabled = value <= 0;
                if (VentasCboUsuarioRegistro.DataSource != null)
                    VentasCboUsuarioRegistro.SelectedValue = value;
            }
        }

        internal int Moneda
        {
            get
            {
                return VentasFuncObtenerId(VentasCboMonedaCierre);
            }
        }

        internal int PaisContable => VentasFuncObtenerId(VentasCboPaisContable);

        internal string Tipo
        {
            get
            {
                return VentasCboTipoCierre.SelectedIndex == 1 ? "ANUAL" : "MENSUAL";
            }
        }

        internal void VentasMetCargar()
        {
            this.Cargando = true;
            try
            {
                VentasMetCargarOpciones(this.VentasCboUsuarioRegistro, this._Controlador.VentasFuncCatalogo("Usuarios"), this.UsuarioInicial);
                DataTable Monedas = this._Controlador.VentasFuncCatalogo("Monedas");
                DataRow Quetzal = Monedas.AsEnumerable().FirstOrDefault(Fila => Convert.ToString(Fila["Nombre"]).IndexOf("GTQ", StringComparison.OrdinalIgnoreCase) >= 0);
                VentasMetCargarOpciones(this.VentasCboMonedaCierre, Monedas);
                if (this.VentasCboMonedaCierre.Items.Count > 0)
                    this.VentasCboMonedaCierre.SelectedValue = Quetzal == null ? Monedas.Rows[0]["Id"] : Quetzal["Id"];
                this.VentasCboTipoCierre.SelectedIndex = 0;
                this.VentasCboMesCierre.SelectedIndex = DateTime.Today.Month - 1;
                // Natali Sofía Montenegro Portillo | 0901-23-10017 | El país se obtiene de tblPais aun sin la DLL contable.
                DataTable Paises = this._Controlador.VentasFuncPaisesContables();
                DataRow Guatemala = Paises.AsEnumerable().FirstOrDefault(Fila => Convert.ToString(Fila["Nombre"]).IndexOf("Guatemala", StringComparison.OrdinalIgnoreCase) >= 0);
                DataRow Seleccione = Paises.NewRow();
                Seleccione["Id"] = 0;
                Seleccione["Nombre"] = "Seleccione un país";
                Paises.Rows.InsertAt(Seleccione, 0);
                VentasMetCargarOpciones(this.VentasCboPaisContable, Paises);
                this.VentasCboPaisContable.SelectedValue = Guatemala == null ? 0 : Convert.ToInt32(Guatemala["Id"]);
                DataTable Clientes = this._Controlador.VentasFuncCatalogo("Clientes");
                DataRow Todos = Clientes.NewRow();
                Todos["Id"] = 0;
                Todos["Nombre"] = "Todos los clientes";
                Clientes.Rows.InsertAt(Todos, 0);
                VentasMetCargarOpciones(this.VentasCboClienteHistorial, Clientes);
                this.VentasCboClienteHistorial.SelectedValue = 0;
            }
            finally
            {
                this.Cargando = false;
            }

            VentasMetLimpiarRevision();
            if (this.Moneda > 0)
            {
                DateTime UltimoMesCompleto = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
                DateTime Pendiente = this._Controlador.VentasFuncSiguientePeriodo(this.Moneda, UltimoMesCompleto);
                VentasMetIrAlPeriodo(Pendiente <= UltimoMesCompleto ? Pendiente : UltimoMesCompleto);
            }
        }

        internal void VentasMetNuevo()
        {
            this.FechaObjetivoTraslado = null;
            this.IdBorrador = 0;
            this.EditandoBorrador = true;
            this.VentasTxtObservacionesCierre.Clear();
            this.Cargando = true;
            try
            {
                this.VentasTbcCierre.SelectedTab = this.VentasTbpPreparar;
                this.VentasNudAnioCierre.Value = DateTime.Today.Year;
                this.VentasCboMesCierre.SelectedIndex = DateTime.Today.Month - 1;
                this.VentasCboTipoCierre.SelectedIndex = 0;
            }
            finally
            {
                this.Cargando = false;
            }

            VentasMetPeriodoCambio();
            this.VentasCboMonedaCierre.Focus();
            VentasMetActualizarAccionesCierre();
        }

        internal void VentasMetLimpiarRevision()
        {
            this.IdCierre = 0;
            this.Revision = null;
            this.Partida = null;
            this.NumeroCierre = "";
            this.VentasDgvRevisionCierre.Rows.Clear();
            this.VentasDgvPoliza.Rows.Clear();
            this.VentasChkRevisionDocumentos.Checked = false;
            this.VentasBtnConfirmar.Enabled = false;
            this.VentasTxtEstadoCierre.Text = "Pendiente de revisión";
            this.VentasTxtReferenciaPoliza.Clear();
            this.VentasTxtFechaTraslado.Clear();
            this.VentasTxtEstadoTraslado.Text = "Sin traslado contable";
            this.VentasLblValorAnteriorCierre.Text = this.VentasLblValorCargosCierre.Text = this.VentasLblValorAbonosCierre.Text = this.VentasLblValorCargosAcumulados.Text = this.VentasLblValorAbonosAcumulados.Text = this.VentasLblValorActualCierre.Text = "0.00";
            VentasMetActualizarConfirmacion();
        }

        internal void VentasMetRevisar()
        {
            this.VentasBtnConfirmar.Enabled = false;
            this._Controlador.VentasMetValidarCaptura(this.Moneda > 0, "Seleccione la moneda. Cada moneda se cierra por separado.");
            if (this.VentasTbcCierre.SelectedTab == this.VentasTbpHistorial)
            {
                VentasMetHistorial();
                return;
            }

            if (this.VentasTbcCierre.SelectedTab == this.VentasTbpContabilidad)
            {
                VentasMetMostrarPartida();
                return;
            }

            DataTable Estado = this._Controlador.VentasFuncEstadoPeriodo(this.Moneda, this.Tipo, this.VentasDtpDesdeCierre.Value, this.VentasDtpHastaCierre.Value);
            this.IdCierre = Estado.Rows.Count == 0 ? 0 : Convert.ToInt32(Estado.Rows[0]["idCierre"]);
            this.NumeroCierre = Estado.Rows.Count == 0 ? "" : Convert.ToString(Estado.Rows[0]["numeroCierre"]);
            if (this.IdCierre == 0)
                this.Revision = this._Controlador.VentasFuncRevisar(this.Moneda, this.Tipo, this.VentasDtpDesdeCierre.Value, this.VentasDtpHastaCierre.Value);
            else
            {
                DataTable Historia = this._Controlador.VentasFuncHistorial(this.Moneda, 0, this.VentasDtpDesdeCierre.Value.ToString("yyyy-MM"));
                this.Revision = Historia.Clone();
                foreach (DataRow Fila in Historia.Rows)
                    if (Convert.ToInt32(Fila["idCierre"]) == this.IdCierre)
                        this.Revision.ImportRow(Fila);
            }

            this.VentasDgvRevisionCierre.Rows.Clear();
            foreach (DataRow Fila2 in this.Revision.Rows)
                this.VentasDgvRevisionCierre.Rows.Add(Fila2["Codigo"], Fila2["Nombre"], Fila2["Anterior"], Fila2["Cargos"], Fila2["Abonos"], Fila2["CargosAcumulados"], Fila2["AbonosAcumulados"], Fila2["Actual"]);
            // Natali Sofía Montenegro Portillo | 0901-23-10017 | Aclara por qué la revisión puede estar vacía.
            this.VentasLblRevisionContable.Text = this.Revision.Rows.Count == 0
                ? "Sin movimientos ni saldos hasta " + this.VentasDtpHastaCierre.Value.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-GT"))
                    + " para esta moneda. La tabla vacía es correcta; puede confirmar el período sin movimientos si corresponde."
                : "El cierre conserva los saldos por cliente y período; los cargos de CxC no tienen que igualar los abonos.";
            var Etiquetas = new[]
            {
                this.VentasLblValorAnteriorCierre,
                this.VentasLblValorCargosCierre,
                this.VentasLblValorAbonosCierre,
                this.VentasLblValorCargosAcumulados,
                this.VentasLblValorAbonosAcumulados,
                this.VentasLblValorActualCierre
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
                Etiquetas[IndiceFila].Text = this.Revision.AsEnumerable().Sum(Registro => Convert.ToDecimal(Registro[Columnas[IndiceFila]])).ToString("N2", Cultura);
            if (this.IdCierre == 0)
            {
                DataRow Borrador = this._Controlador.VentasFuncListar(this.Moneda).AsEnumerable().FirstOrDefault(Registro => Convert.ToString(Registro["Estado"]) == "BORRADOR" && Convert.ToString(Registro["Tipo"]) == this.Tipo && Convert.ToDateTime(Registro["Desde"]).Date == this.VentasDtpDesdeCierre.Value.Date && Convert.ToDateTime(Registro["Hasta"]).Date == this.VentasDtpHastaCierre.Value.Date);
                if (Borrador != null)
                {
                    this.IdBorrador = Convert.ToInt32(Borrador["Id"]);
                    if (!this.EditandoBorrador)
                        this.VentasTxtObservacionesCierre.Text = Convert.ToString(Borrador["Observaciones"]);
                }
            }

            this.VentasTxtEstadoCierre.Text = this.IdCierre == 0 ? (this.IdBorrador > 0 ? "Borrador revisado" : "Revisado; sin cerrar") : "Cerrado: " + this.NumeroCierre;
            VentasMetActualizarConfirmacion();
        }

        internal void VentasMetActualizarConfirmacion()
        {
            VentasMetActualizarAccionesCierre();
            this.VentasBtnConfirmar.Enabled = this.Revision != null && this.IdCierre == 0 && this.VentasChkRevisionDocumentos.Checked && this.VentasTbcCierre.SelectedTab == this.VentasTbpPreparar && this.VentasDtpHastaCierre.Value.Date < DateTime.Today;
            this.VentasLblCierreHistorico.Text = this.VentasDtpHastaCierre.Value.Date >= DateTime.Today
                ? "El período debe terminar antes de confirmarlo. Elija un mes anterior."
                : this.IdCierre > 0 ? "Paso 3: abra Ver póliza y enviar."
                : this.Revision == null ? "Elija un mes y revise los saldos que aparecen."
                : !this.VentasChkRevisionDocumentos.Checked ? "Marque la constancia de revisión para habilitar Confirmar cierre."
                : "Pulse Confirmar cierre. Guardar solo conserva un borrador.";
            if (this.FechaObjetivoTraslado.HasValue && this.VentasTbcCierre.SelectedTab == this.VentasTbpPreparar
                && this.VentasDtpDesdeCierre.Value.Date < this.FechaObjetivoTraslado.Value && this.Revision != null)
            {
                string Meta = this.FechaObjetivoTraslado.Value.ToString("MMMM", new System.Globalization.CultureInfo("es-GT"));
                this.VentasLblCierreHistorico.Text = this.VentasChkRevisionDocumentos.Checked
                    ? "Meta: " + Meta + ". Confirme este mes para continuar."
                    : "Meta: " + Meta + ". Marque la constancia de este mes.";
            }
        }

        internal void VentasMetPeriodoCambio()
        {
            // Mantiene el período completo; la revisión anterior queda invalidada al cambiarlo.
            bool Anual = this.VentasCboTipoCierre.Text == "Anual";
            int Anio = (int)this.VentasNudAnioCierre.Value;
            int Mes = this.VentasCboMesCierre.SelectedIndex < 0 ? DateTime.Today.Month : this.VentasCboMesCierre.SelectedIndex + 1;
            DateTime[] Periodo = this._Controlador.VentasFuncPeriodo(Anio, Mes, Anual);
            DateTime Desde = Periodo[0], Hasta = Periodo[1];
            this.VentasCboMesCierre.Enabled = !Anual;
            this.VentasDtpDesdeCierre.Value = Desde;
            this.VentasDtpHastaCierre.Value = Hasta;
            this.VentasDtpCorteCierre.Value = Hasta;
            this.VentasLblPeriodoCalculado.Text = "Del " + Desde.ToString("dd/MM/yyyy") + " al " + Hasta.ToString("dd/MM/yyyy");
            string Unidad = Anual ? "año" : "mes";
            this.VentasClmRevisionCierreCargos.HeaderText = "Cargos del " + Unidad;
            this.VentasClmRevisionCierreAbonos.HeaderText = "Abonos del " + Unidad;
            this.VentasLblTituloCargosCierre.Text = "Cargos del " + Unidad;
            this.VentasLblTituloAbonosCierre.Text = "Abonos del " + Unidad;
            if (!this.Cargando)
            {
                this.FechaObjetivoTraslado = null;
                if (!this.EditandoBorrador)
                {
                    this.IdBorrador = 0;
                    this.EditandoBorrador = true;
                    this.VentasTxtObservacionesCierre.Clear();
                }

                VentasMetLimpiarRevision();
                VentasMetActualizarAccionesCierre();
                if (this.VentasTbcCierre.SelectedTab == this.VentasTbpContabilidad && this.Moneda > 0)
                    VentasMetMostrarPartida();
            }
        }

            }
}
