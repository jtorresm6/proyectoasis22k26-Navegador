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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Threading;
using CapaControlador_TipoCambio;

namespace CapaVista_DisponibilidadDiaria.Formularios
{
    public partial class FrmCuentaBancariaNavegador : Form
    {
        private readonly ClsControladorMonedaBanguat _monedas;
        private readonly CancellationTokenSource _cierreMonedas = new CancellationTokenSource();
        private readonly ToolTip _informacionMonedas = new ToolTip();
        private bool _editandoCuenta;
        private ComboBox _comboMoneda;
        private object[] _opcionesMoneda = new object[0];
        private long _monedaRegistrada;
        private int _versionMonedas;

        public FrmCuentaBancariaNavegador() : this(new ClsControladorMonedaBanguat()) { }

        public FrmCuentaBancariaNavegador(ClsControladorMonedaBanguat monedas)
        {
            _monedas = monedas ?? throw new ArgumentNullException(nameof(monedas));
            InitializeComponent();
            BancosNavCuentaBancaria.NavegadorMetConfigurar("tblCuentaBancaria", 8000, 8005);
            BancosNavCuentaBancaria.NavegadorAccionSolicitada += BancosCuenta_AccionSolicitada;
            ClsControladorMonedaBanguat.BancosCatalogoMonedaActualizado += BancosCuenta_CatalogoActualizado;
            FormClosed += (s, e) =>
            {
                _cierreMonedas.Cancel();
                ClsControladorMonedaBanguat.BancosCatalogoMonedaActualizado -= BancosCuenta_CatalogoActualizado;
                _informacionMonedas.Dispose();
            };
        }

        private void BancosCuenta_AccionSolicitada(string accion)
        {
            if (accion != "INGRESAR" && accion != "MODIFICAR") return;
            _editandoCuenta = accion == "MODIFICAR";
            // El Navegador crea el editor despues de notificar la accion.
            // Se conservan sus opciones originales para que el guardado siga
            // utilizando el mismo componente y sus validaciones.
            BeginInvoke(new Action(async () => await BancosMetFiltrarMonedasCuentaAsync()));
        }

        private void BancosCuenta_CatalogoActualizado(object sender, EventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated) return;
            try { BeginInvoke(new Action(async () => await BancosMetFiltrarMonedasCuentaAsync())); }
            catch (InvalidOperationException) { }
        }

        private async Task BancosMetFiltrarMonedasCuentaAsync()
        {
            if (IsDisposed || Disposing || _cierreMonedas.IsCancellationRequested) return;
            var combo = BancosNavCuentaBancaria.Controls.Find("NavegadorCboFk_Moneda_Codigo", true)
                .OfType<ComboBox>().FirstOrDefault();
            if (combo == null || !combo.Visible) return;
            int version = ++_versionMonedas;
            object seleccionada = combo.SelectedItem;
            if (!ReferenceEquals(_comboMoneda, combo))
            {
                _comboMoneda = combo;
                _opcionesMoneda = combo.Items.Cast<object>().ToArray();
                _monedaRegistrada = 0;
                long registrada;
                if (_editandoCuenta && BancosMetIdOpcionMoneda(seleccionada, out registrada)) _monedaRegistrada = registrada;
            }
            combo.Enabled = false;
            try
            {
                var catalogo = await _monedas.BancosMetConsultarAsync(_cierreMonedas.Token);
                if (IsDisposed || combo.IsDisposed || _cierreMonedas.IsCancellationRequested || version != _versionMonedas) return;
                var permitidas = new HashSet<long>(catalogo.Where(m => m.Activa && !m.RequiereMigrarId).Select(m => m.Id));
                if (_monedaRegistrada > 0) permitidas.Add(_monedaRegistrada);
                combo.BeginUpdate();
                try
                {
                    combo.Items.Clear();
                    foreach (object opcion in _opcionesMoneda)
                    {
                        long id;
                        if (BancosMetIdOpcionMoneda(opcion, out id) && permitidas.Contains(id)) combo.Items.Add(opcion);
                    }
                    if (seleccionada != null && combo.Items.Contains(seleccionada)) combo.SelectedItem = seleccionada;
                }
                finally { combo.EndUpdate(); }
                string mensaje = combo.Items.Count == 0
                    ? "No hay monedas habilitadas con ID Banguat. Consulta o sincroniza el catalogo de Moneda."
                    : "Monedas habilitadas del catalogo Banguat. Una cuenta existente conserva su moneda actual.";
                _informacionMonedas.SetToolTip(combo, mensaje);
                combo.AccessibleDescription = mensaje;
            }
            catch (OperationCanceledException) when (_cierreMonedas.IsCancellationRequested) { }
            catch (Exception)
            {
                if (!combo.IsDisposed && version == _versionMonedas)
                {
                    combo.Items.Clear();
                    _informacionMonedas.SetToolTip(combo, "No se pudo consultar el catalogo local de monedas. Cierra y vuelve a abrir este registro para reintentar.");
                }
            }
            finally
            {
                if (!combo.IsDisposed && version == _versionMonedas) combo.Enabled = combo.Items.Count > 0;
            }
        }

        private static bool BancosMetIdOpcionMoneda(object opcion, out long id)
        {
            string texto = Convert.ToString(opcion);
            int separador = texto.IndexOf(" - ", StringComparison.Ordinal);
            if (separador >= 0) texto = texto.Substring(0, separador);
            return long.TryParse(texto, NumberStyles.None, CultureInfo.InvariantCulture, out id) && id > 0;
        }
    }
}
