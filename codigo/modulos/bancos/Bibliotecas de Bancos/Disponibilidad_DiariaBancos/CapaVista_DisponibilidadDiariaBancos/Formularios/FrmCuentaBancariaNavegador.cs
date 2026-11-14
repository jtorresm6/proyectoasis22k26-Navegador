/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Victor Josue Ortiz Samayoa
 * Carné  : 0901-23-3424
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Formulario navegador del módulo Cuenta Bancaria. Inicializa
 *  el control de navegación apuntando a la tabla tblCuentaBancaria,
 *  dentro del módulo Bancos (8000), aplicación 8005.
 * ================================================================
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_TipoCambio;

namespace CapaVista_DisponibilidadDiaria.Formularios
{
    public partial class FrmCuentaBancariaNavegador : Form
    {
        private readonly ClsControladorMonedaBanguat _Monedas;
        private readonly CancellationTokenSource _CierreMonedas = new CancellationTokenSource();
        private readonly ToolTip _InformacionMonedas = new ToolTip();
        private bool _EditandoCuenta;
        private ComboBox _ComboMoneda;
        private object[] _OpcionesMoneda = new object[0];
        private long _MonedaRegistrada;
        private int _VersionMonedas;

        public FrmCuentaBancariaNavegador() : this(new ClsControladorMonedaBanguat()) { }

        public FrmCuentaBancariaNavegador(ClsControladorMonedaBanguat Monedas)
        {
            _Monedas = Monedas ?? throw new ArgumentNullException(nameof(Monedas));
            InitializeComponent();
            BancosNavCuentaBancaria.NavegadorMetConfigurar("tblCuentaBancaria", 8000, 8005);
            BancosNavCuentaBancaria.NavegadorAccionSolicitada += BancosCuenta_AccionSolicitada;
            ClsControladorMonedaBanguat.BancosCatalogoMonedaActualizado += BancosCuenta_CatalogoActualizado;
            FormClosed += (s, e) =>
            {
                _CierreMonedas.Cancel();
                ClsControladorMonedaBanguat.BancosCatalogoMonedaActualizado -= BancosCuenta_CatalogoActualizado;
                _InformacionMonedas.Dispose();
            };
        }

        private void BancosCuenta_AccionSolicitada(string Accion)
        {
            if (Accion != "INGRESAR" && Accion != "MODIFICAR") return;
            _EditandoCuenta = Accion == "MODIFICAR";
            // El Navegador crea el editor despues de notificar la accion.
            // Se conservan sus opciones originales para que el guardado siga
            // utilizando el mismo componente y sus validaciones.
            BeginInvoke(new Action(async () => await BancosMetFiltrarMonedasCuentaAsync()));
        }

        private void BancosCuenta_CatalogoActualizado(object Sender, EventArgs E)
        {
            if (IsDisposed || Disposing || !IsHandleCreated) return;
            try { BeginInvoke(new Action(async () => await BancosMetFiltrarMonedasCuentaAsync())); }
            catch (InvalidOperationException) { }
        }

        private async Task BancosMetFiltrarMonedasCuentaAsync()
        {
            if (IsDisposed || Disposing || _CierreMonedas.IsCancellationRequested) return;
            var Combo = BancosNavCuentaBancaria.Controls.Find("NavegadorCboFk_Moneda_Codigo", true)
                .OfType<ComboBox>().FirstOrDefault();
            if (Combo == null || !Combo.Visible) return;
            int Version = ++_VersionMonedas;
            object Seleccionada = Combo.SelectedItem;
            if (!ReferenceEquals(_ComboMoneda, Combo))
            {
                _ComboMoneda = Combo;
                _OpcionesMoneda = Combo.Items.Cast<object>().ToArray();
                _MonedaRegistrada = 0;
                long Registrada;
                if (_EditandoCuenta && BancosMetIdOpcionMoneda(Seleccionada, out Registrada)) _MonedaRegistrada = Registrada;
            }
            Combo.Enabled = false;
            try
            {
                var Catalogo = await _Monedas.BancosMetConsultarAsync(_CierreMonedas.Token);
                if (IsDisposed || Combo.IsDisposed || _CierreMonedas.IsCancellationRequested || Version != _VersionMonedas) return;
                var Permitidas = new HashSet<long>(Catalogo.Where(m => m.Activa && !m.RequiereMigrarId).Select(m => m.Id));
                if (_MonedaRegistrada > 0) Permitidas.Add(_MonedaRegistrada);
                Combo.BeginUpdate();
                try
                {
                    Combo.Items.Clear();
                    foreach (object Opcion in _OpcionesMoneda)
                    {
                        long Id;
                        if (BancosMetIdOpcionMoneda(Opcion, out Id) && Permitidas.Contains(Id)) Combo.Items.Add(Opcion);
                    }
                    if (Seleccionada != null && Combo.Items.Contains(Seleccionada)) Combo.SelectedItem = Seleccionada;
                }
                finally { Combo.EndUpdate(); }
                string Mensaje = Combo.Items.Count == 0
                    ? "No hay monedas habilitadas con ID Banguat. Consulta o sincroniza el catalogo de Moneda."
                    : "Monedas habilitadas del catalogo Banguat. Una cuenta existente conserva su moneda actual.";
                _InformacionMonedas.SetToolTip(Combo, Mensaje);
                Combo.AccessibleDescription = Mensaje;
            }
            catch (OperationCanceledException) when (_CierreMonedas.IsCancellationRequested) { }
            catch (Exception)
            {
                if (!Combo.IsDisposed && Version == _VersionMonedas)
                {
                    Combo.Items.Clear();
                    _InformacionMonedas.SetToolTip(Combo, "No se pudo consultar el catalogo local de monedas. Cierra y vuelve a abrir este registro para reintentar.");
                }
            }
            finally
            {
                if (!Combo.IsDisposed && Version == _VersionMonedas) Combo.Enabled = Combo.Items.Count > 0;
            }
        }

        private static bool BancosMetIdOpcionMoneda(object Opcion, out long Id)
        {
            string Texto = Convert.ToString(Opcion);
            int Separador = Texto.IndexOf(" - ", StringComparison.Ordinal);
            if (Separador >= 0) Texto = Texto.Substring(0, Separador);
            return long.TryParse(Texto, NumberStyles.None, CultureInfo.InvariantCulture, out Id) && Id > 0;
        }
    }
}
