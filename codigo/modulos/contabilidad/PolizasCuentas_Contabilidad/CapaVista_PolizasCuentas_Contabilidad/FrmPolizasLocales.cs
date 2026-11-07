/*
 * Área: Contabilidad
 * Autor: Oskar Cermeño
 * Carné: 0901-23-15379
 * Fecha: 14/10/2026
 * Propósito: prototipo no funcional de pólizas locales de Embutidos de Calidad,
 * basado en tblCuenta, tblPolizaEncabezado y tblPolizaDetalle de dbContabilidad.
 */
using System;
// Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
using System.Collections.Generic;
// Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaControlador_Contabilidad;
using CapaVista_Contabilidad.Componentes;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
// Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
using CapaControlador_Navegador;
// Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public partial class FrmPolizasLocales : Form
    {
        private readonly int _IdModulo;
        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private ClsModeloPermisosPantallaContabilidad _Permisos;
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private readonly ClsControladorPolizasContabilidad _Controlador = new ClsControladorPolizasContabilidad();
        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private readonly BindingList<ClsModeloLineaPolizaContabilidad> _Lineas = new BindingList<ClsModeloLineaPolizaContabilidad>();
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private readonly CultureInfo _Cultura = CultureInfo.GetCultureInfo("es-GT");
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();
        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        private readonly ClsPermisosMenuContabilidad _PermisosBotones = new ClsPermisosMenuContabilidad();
        private readonly string _ModoActualizacion;
        private readonly List<int> _Polizas;
        private int _IdPoliza;
        private string _Estado = "BORRADOR";
        private string _CodigoPoliza;
        private string _PeriodoOriginal;
        private bool _Editando;
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmPolizasLocales() : this(ClsModeloPermisosPantallaContabilidad.Total, 1)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmPolizasLocales(ClsModeloPermisosPantallaContabilidad Permisos) : this(Permisos, 1)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmPolizasLocales(ClsModeloPermisosPantallaContabilidad Permisos, int IdModulo)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            : this(Permisos, IdModulo, "BATCH", null, 0)
        {
        }

        public FrmPolizasLocales(ClsModeloPermisosPantallaContabilidad Permisos, int IdModulo, string ModoActualizacion,
            IList<int> Polizas, int IdPoliza)
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        {
            _IdModulo = IdModulo;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
            _ModoActualizacion = ModoActualizacion == "LINEA" ? "LINEA" : "BATCH";
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            _Polizas = Polizas == null ? new List<int>() : new List<int>(Polizas);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            InitializeComponent();
            _Permisos = Permisos;
            ContabilidadMetAplicarPermisos(Permisos);
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Pólizas locales", new Size(1180, 760));
            ContabilidadLblModo.Text = "● " + _ModoActualizacion;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
            ContabilidadLblModo.ForeColor = _ModoActualizacion == "LINEA" ? Color.FromArgb(34, 139, 70) : Color.FromArgb(197, 143, 62);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
            foreach (Control Campo in new Control[] { ContabilidadTxtDescripcion, ContabilidadDtpFecha, ContabilidadCboEstado, ContabilidadCboOrigen, ContabilidadCboCuenta })
                _Validacion.ContabilidadMetObligatorio(Campo);
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            Disposed += (Remitente, Argumentos) => { _Validacion.Dispose(); _PermisosBotones.Dispose(); };
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadColDebe.DefaultCellStyle.FormatProvider = _Cultura;
            ContabilidadColHaber.DefaultCellStyle.FormatProvider = _Cultura;
            ContabilidadMetCargarCatalogos();
            ContabilidadDgvDetalle.DataSource = _Lineas;
            ContabilidadDtpFecha.ValueChanged += ContabilidadMetActualizarPeriodo;
            ContabilidadBtnAgregar.Click += ContabilidadMetAgregarCuenta;
            ContabilidadBtnLimpiarDetalle.Click += ContabilidadMetLimpiarDetalle;
            ContabilidadBtnAplicar.Click += ContabilidadMetAplicar;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadBtnIngresar.Click += ContabilidadMetNueva;
            ContabilidadBtnConsultar.Click += (Remitente, Argumentos) => Close();
            ContabilidadBtnModificar.Click += ContabilidadMetModificar;
            ContabilidadBtnEliminar.Click += ContabilidadMetAnular;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadBtnGuardar.Click += ContabilidadMetGuardar;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadBtnCancelar.Click += ContabilidadMetCancelar;
            ContabilidadBtnRefrescar.Click += (Remitente, Argumentos) => ContabilidadMetCargarPoliza(_IdPoliza);
            ContabilidadBtnImprimir.Click += (Remitente, Argumentos) => MessageBox.Show(this,
                "La impresión de pólizas se conectará con el componente Reporteador.", "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
            ContabilidadBtnAyuda.Click += ContabilidadMetMostrarAyuda;
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadBtnInicio.Click += (Remitente, Argumentos) => ContabilidadMetIrA(0);
            ContabilidadBtnAnterior.Click += (Remitente, Argumentos) => ContabilidadMetIrA(_Polizas.IndexOf(_IdPoliza) - 1);
            ContabilidadBtnSiguiente.Click += (Remitente, Argumentos) => ContabilidadMetIrA(_Polizas.IndexOf(_IdPoliza) + 1);
            ContabilidadBtnFin.Click += (Remitente, Argumentos) => ContabilidadMetIrA(_Polizas.Count - 1);
            ContabilidadBtnSalir.Click += (Remitente, Argumentos) => Close();
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadDgvDetalle.CellContentClick += ContabilidadMetQuitarCuenta;
            ContabilidadDgvDetalle.CellEndEdit += ContabilidadMetTerminarEdicion;
            ContabilidadDgvDetalle.CellParsing += ContabilidadMetInterpretarImporte;
            ContabilidadDgvDetalle.DataError += ContabilidadMetErrorImporte;
            ContabilidadDgvDetalle.ColumnWidthChanged += ContabilidadMetAlinearTotales;
            ContabilidadDgvDetalle.SizeChanged += ContabilidadMetAlinearTotales;
            _Lineas.ListChanged += ContabilidadMetCambiarDetalle;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            if (IdPoliza > 0) ContabilidadMetCargarPoliza(IdPoliza);
            else ContabilidadMetNueva(this, EventArgs.Empty);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAplicarPermisos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnAgregar, Permisos.PuedeInsertar || Permisos.PuedeEditar);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnAplicar, Permisos.PuedeEditar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnLimpiarDetalle, Permisos.PuedeEliminar);
        }

        private void ContabilidadMetCargarCatalogos()
        {
            ContabilidadCboCuenta.DisplayMember = "Descripcion";
            ContabilidadCboCuenta.ValueMember = "IdCuenta";
            try
            {
                ContabilidadCboCuenta.DataSource = _Controlador.ContabilidadFuncObtenerCuentas(_IdModulo);
            }
            catch (Exception Excepcion)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron cargar las cuentas: " + Excepcion.Message);
            }
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadCboEstado.DataSource = new[] { "BORRADOR", "VALIDADA", "APLICADA", "ANULADA" };
            ContabilidadCboEstado.Enabled = false;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadCboOrigen.DataSource = new[] { _Controlador.ContabilidadFuncObtenerNombreModulo(_IdModulo) };
            ContabilidadCboOrigen.Enabled = false;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ContabilidadCboPais.DisplayMember = "Value";
            ContabilidadCboPais.ValueMember = "Key";
            try
            {
                ContabilidadCboPais.DataSource = _Controlador.ContabilidadFuncObtenerPaises();
                ContabilidadCboPais.SelectedIndexChanged += (Remitente, Argumentos) => ContabilidadMetRecalcularTotales();
            }
            catch (Exception Excepcion)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron cargar los países: " + Excepcion.Message);
            }
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        }

        private void ContabilidadMetActualizarPeriodo(object Remitente, EventArgs Argumentos)
        {
            ContabilidadTxtPeriodo.Text = _Controlador.ContabilidadFuncObtenerPeriodo(ContabilidadDtpFecha.Value);
        }

        private void ContabilidadMetAgregarCuenta(object Remitente, EventArgs Argumentos)
        {
            ContabilidadDgvDetalle.EndEdit();
            string Nombre = ContabilidadCboCuenta.Text.Trim();
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ClsModeloCuentaPolizaContabilidad Cuenta = ContabilidadCboCuenta.Items.Cast<ClsModeloCuentaPolizaContabilidad>().FirstOrDefault(Elemento =>
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
                string.Equals(Elemento.Descripcion, Nombre, StringComparison.CurrentCultureIgnoreCase));
            if (Cuenta == null)
            {
                _Validacion.ContabilidadFuncValidar(ContabilidadCboCuenta, "Seleccione una cuenta válida del catálogo.");
                ContabilidadCboCuenta.Focus();
                return;
            }
            int IdCuenta = Cuenta.IdCuenta;
            if (_Lineas.Any(Linea => Linea.IdCuenta == IdCuenta))
            {
                _Validacion.ContabilidadFuncValidar(ContabilidadCboCuenta, "La cuenta ya está agregada al detalle.");
                return;
            }
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            _Lineas.Add(new ClsModeloLineaPolizaContabilidad
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            {
                IdCuenta = IdCuenta, Cuenta = Cuenta.Descripcion, Debe = 0m, Haber = 0m
            });
            int Indice = _Lineas.Count - 1;
            ContabilidadDgvDetalle.CurrentCell = ContabilidadDgvDetalle.Rows[Indice].Cells[ContabilidadColDebe.Index];
            ContabilidadDgvDetalle.Focus();
            ContabilidadDgvDetalle.BeginEdit(true);
            ContabilidadCboCuenta.SelectedIndex = -1;
            ContabilidadCboCuenta.Text = string.Empty;
        }

        private void ContabilidadMetQuitarCuenta(object Remitente, DataGridViewCellEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex != ContabilidadColQuitar.Index) return;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            if (!_Editando) return;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadDgvDetalle.EndEdit();
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ClsModeloLineaPolizaContabilidad Linea = ContabilidadDgvDetalle.Rows[Argumentos.RowIndex].DataBoundItem as ClsModeloLineaPolizaContabilidad;
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            if (Linea != null) _Lineas.Remove(Linea);
        }

        private void ContabilidadMetInterpretarImporte(object Remitente, DataGridViewCellParsingEventArgs Argumentos)
        {
            if (Argumentos.ColumnIndex != ContabilidadColDebe.Index && Argumentos.ColumnIndex != ContabilidadColHaber.Index) return;
            string Texto = Convert.ToString(Argumentos.Value, _Cultura);
            decimal Importe;
            if (string.IsNullOrWhiteSpace(Texto)) Importe = 0m;
            else if (!decimal.TryParse(Texto, NumberStyles.Number, _Cultura, out Importe)) return;
            if (Importe < -ClsLimitesContabilidad.MaximoMonetario || Importe > ClsLimitesContabilidad.MaximoMonetario)
            {
                Argumentos.Value = "Importe fuera de rango";
                return;
            }
            Argumentos.Value = decimal.Round(Importe, 2, MidpointRounding.AwayFromZero);
            Argumentos.ParsingApplied = true;
        }

        private void ContabilidadMetErrorImporte(object Remitente, DataGridViewDataErrorEventArgs Argumentos)
        {
            Argumentos.ThrowException = false;
            Argumentos.Cancel = false;
            if (Argumentos.RowIndex < 0 ||
                (Argumentos.ColumnIndex != ContabilidadColDebe.Index && Argumentos.ColumnIndex != ContabilidadColHaber.Index)) return;
            ContabilidadDgvDetalle.CancelEdit();
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ClsModeloLineaPolizaContabilidad Linea = ContabilidadDgvDetalle.Rows[Argumentos.RowIndex].DataBoundItem as ClsModeloLineaPolizaContabilidad;
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            if (Linea != null)
            {
                if (Argumentos.ColumnIndex == ContabilidadColDebe.Index) Linea.Debe = 0m;
                else Linea.Haber = 0m;
                _Lineas.ResetItem(Argumentos.RowIndex);
            }
            ContabilidadMetRecalcularTotales();
            ClsDialogosContabilidad.ContabilidadMetError(this, "Importe inválido. Ingrese un número dentro del rango permitido. Se asignó 0.00.");
        }

        private void ContabilidadMetTerminarEdicion(object Remitente, DataGridViewCellEventArgs Argumentos)
        {
            ContabilidadMetRecalcularTotales();
        }

        private void ContabilidadMetCambiarDetalle(object Remitente, ListChangedEventArgs Argumentos)
        {
            ContabilidadMetRecalcularTotales();
        }

        private void ContabilidadMetRecalcularTotales()
        {
            decimal Debe = _Controlador.ContabilidadFuncTotalDebe(_Lineas);
            decimal Haber = _Controlador.ContabilidadFuncTotalHaber(_Lineas);
            bool Cuadrada = _Controlador.ContabilidadFuncEstaCuadrada(_Lineas);
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            string Moneda = _Controlador.ContabilidadFuncMonedaPais(ContabilidadCboPais.SelectedValue == null ? 1 : (int)ContabilidadCboPais.SelectedValue);
            ContabilidadColDebe.HeaderText = "Debe (" + Moneda + ")";
            ContabilidadColHaber.HeaderText = "Haber (" + Moneda + ")";
            ContabilidadLblTotalDebe.Text = "Total debe: " + Moneda + " " + Debe.ToString("N2", _Cultura);
            ContabilidadLblTotalHaber.Text = "Total haber: " + Moneda + " " + Haber.ToString("N2", _Cultura);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            if (_Lineas.Count == 0)
            {
                ContabilidadLblCuadre.Text = "Agregue las cuentas de la póliza";
                ContabilidadLblCuadre.ForeColor = Color.FromArgb(70, 104, 125);
            }
            else
            {
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                ContabilidadLblCuadre.Text = Cuadrada ? "Cuadrada" : "Descuadrada · diferencia " + Moneda + " " + Math.Abs(Debe - Haber).ToString("N2", _Cultura);
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                ContabilidadLblCuadre.ForeColor = Cuadrada ? Color.FromArgb(70, 104, 125) : Color.FromArgb(197, 143, 62);
            }
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadBtnAplicar.Enabled = _Editando && Cuadrada && _Lineas.Count >= 2;
            ContabilidadBtnLimpiarDetalle.Enabled = _Editando && _Lineas.Count > 0;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadMetAlinearTotales(this, EventArgs.Empty);
        }

        private void ContabilidadMetAlinearTotales(object Remitente, EventArgs Argumentos)
        {
            if (Disposing || IsDisposed || ContabilidadDgvDetalle.Disposing || ContabilidadDgvDetalle.IsDisposed ||
                ContabilidadColDebe.Index < 0 || ContabilidadColHaber.Index < 0) return;
            Rectangle AreaDebe = ContabilidadDgvDetalle.GetColumnDisplayRectangle(ContabilidadColDebe.Index, false);
            Rectangle AreaHaber = ContabilidadDgvDetalle.GetColumnDisplayRectangle(ContabilidadColHaber.Index, false);
            ContabilidadLblTotalDebe.Left = ContabilidadDgvDetalle.Left + AreaDebe.Left;
            ContabilidadLblTotalDebe.Width = AreaDebe.Width;
            ContabilidadLblTotalHaber.Left = ContabilidadDgvDetalle.Left + AreaHaber.Left;
            ContabilidadLblTotalHaber.Width = AreaHaber.Width;
        }

        private void ContabilidadMetLimpiarDetalle(object Remitente, EventArgs Argumentos)
        {
            ContabilidadDgvDetalle.CancelEdit();
            _Lineas.Clear();
        }

        private void ContabilidadMetNueva(object Remitente, EventArgs Argumentos)
        {
            _Validacion.ContabilidadMetLimpiar();
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            _IdPoliza = 0;
            _Estado = "BORRADOR";
            _CodigoPoliza = null;
            _PeriodoOriginal = null;
            _Editando = _Permisos.PuedeInsertar;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadMetLimpiarDetalle(Remitente, Argumentos);
            ContabilidadDtpFecha.Value = DateTime.Today;
            ContabilidadMetActualizarPeriodo(this, EventArgs.Empty);
            ContabilidadCboEstado.SelectedItem = "BORRADOR";
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            if (ContabilidadCboPais.Items.Count > 0) ContabilidadCboPais.SelectedIndex = 0;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ContabilidadTxtDescripcion.Clear();
            ContabilidadCboCuenta.SelectedIndex = -1;
            ContabilidadCboCuenta.Text = string.Empty;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadMetActualizarVista();
            ContabilidadTxtDescripcion.Focus();
        }

        private void ContabilidadMetCargarPoliza(int IdPoliza)
        {
            ClsModeloPolizaContabilidad Poliza;
            try
            {
                Poliza = _Controlador.ContabilidadFuncObtenerPoliza(IdPoliza);
            }
            catch (Exception Excepcion)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudo consultar la póliza: " + Excepcion.Message);
                return;
            }
            if (Poliza == null)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "La póliza ya no existe o fue anulada.");
                _Polizas.Remove(IdPoliza);
                ContabilidadMetNueva(this, EventArgs.Empty);
                return;
            }
            _Validacion.ContabilidadMetLimpiar();
            _IdPoliza = Poliza.IdPoliza;
            _Estado = Poliza.Estado;
            _CodigoPoliza = Poliza.CodigoPoliza;
            _PeriodoOriginal = Poliza.Periodo;
            _Editando = false;
            ContabilidadDgvDetalle.CancelEdit();
            _Lineas.RaiseListChangedEvents = false;
            _Lineas.Clear();
            foreach (ClsModeloLineaPolizaContabilidad Linea in Poliza.Lineas) _Lineas.Add(Linea);
            _Lineas.RaiseListChangedEvents = true;
            _Lineas.ResetBindings();
            ContabilidadDtpFecha.Value = Poliza.Fecha;
            ContabilidadTxtPeriodo.Text = Poliza.Periodo;
            ContabilidadCboEstado.SelectedItem = Poliza.Estado;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ContabilidadCboPais.SelectedValue = Poliza.IdPais;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ContabilidadTxtDescripcion.Text = Poliza.Descripcion;
            ContabilidadMetActualizarVista();
        }

        private void ContabilidadMetActualizarVista()
        {
            bool Nueva = _IdPoliza == 0;
            bool Modificable = _Controlador.ContabilidadFuncPuedeModificar(_Estado);
            ContabilidadLblTitulo.Text = Nueva
                ? "Nueva póliza " + _Controlador.ContabilidadFuncObtenerNombreModulo(_IdModulo)
                : "Póliza " + _CodigoPoliza;
            ContabilidadLblEstadoPoliza.Text = Nueva ? "NUEVA · en captura"
                : Modificable ? "BORRADOR · se puede modificar"
                : _Estado == "APLICADA" ? "APLICADA · saldos actualizados" : _Estado + " · no se puede modificar";
            ContabilidadLblEstadoPoliza.BackColor = Modificable ? Color.FromArgb(34, 139, 70) : Color.Firebrick;
            ContabilidadLblEstadoPoliza.ForeColor = Color.White;

            ContabilidadTxtDescripcion.ReadOnly = !_Editando;
            ContabilidadDtpFecha.Enabled = _Editando;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ContabilidadCboPais.Enabled = _Editando;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ContabilidadCboCuenta.Enabled = _Editando;
            ContabilidadBtnAgregar.Enabled = _Editando;
            ContabilidadDgvDetalle.ReadOnly = !_Editando;
            ContabilidadColCuenta.ReadOnly = true;
            ContabilidadColQuitar.Visible = _Editando;

            int Posicion = _Polizas.IndexOf(_IdPoliza);
            bool Navegar = !_Editando && _Polizas.Count > 0;
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnIngresar, !_Editando && _Permisos.PuedeInsertar);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnConsultar, !_Editando);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnModificar, !_Editando && !Nueva && Modificable && _Permisos.PuedeEditar);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnEliminar, !_Editando && !Nueva &&
                (_Estado == "BORRADOR" || _Estado == "VALIDADA") && _Permisos.PuedeEliminar);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnGuardar, _Editando);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnCancelar, _Editando);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnRefrescar, !_Editando && !Nueva);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnImprimir, !_Editando && !Nueva && _Permisos.PuedeImprimir);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnInicio, Navegar && Posicion != 0);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnAnterior, Navegar && Posicion > 0);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnSiguiente, Navegar && Posicion < _Polizas.Count - 1);
            _PermisosBotones.ContabilidadMetAplicar(ContabilidadBtnFin, Navegar && Posicion != _Polizas.Count - 1);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadMetRecalcularTotales();
        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        }

        private void ContabilidadMetIrA(int Indice)
        {
            if (Indice < 0 || Indice >= _Polizas.Count) return;
            ContabilidadMetCargarPoliza(_Polizas[Indice]);
        }

        private void ContabilidadMetModificar(object Remitente, EventArgs Argumentos)
        {
            if (!_Controlador.ContabilidadFuncPuedeModificar(_Estado))
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "Solo se puede modificar una póliza en borrador.");
                return;
            }
            _Editando = true;
            ContabilidadMetActualizarVista();
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            ContabilidadTxtDescripcion.Focus();
        }

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        private void ContabilidadMetCancelar(object Remitente, EventArgs Argumentos)
        {
            if (_IdPoliza > 0) ContabilidadMetCargarPoliza(_IdPoliza);
            else ContabilidadMetNueva(Remitente, Argumentos);
        }

        private void ContabilidadMetAnular(object Remitente, EventArgs Argumentos)
        {
            if (_IdPoliza == 0) return;
            if (!ClsDialogosContabilidad.ContabilidadFuncConfirmar(this,
                    "¿Desea anular la póliza " + _CodigoPoliza + "? Una póliza anulada no se puede recuperar.", "Sí, anular"))
                return;
            ClsModeloPolizaContabilidad Modelo = new ClsModeloPolizaContabilidad
            {
                IdPoliza = _IdPoliza, Descripcion = ContabilidadTxtDescripcion.Text.Trim(), Fecha = ContabilidadDtpFecha.Value.Date,
                Estado = _Estado, IdModulo = _IdModulo, Periodo = _PeriodoOriginal, Lineas = _Lineas.ToList()
            };
            string Mensaje;
            if (_Controlador.ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad.Borrar, Modelo, out Mensaje))
            {
                ClsDialogosContabilidad.ContabilidadMetExito(this, Mensaje);
                int Posicion = _Polizas.IndexOf(_IdPoliza);
                _Polizas.Remove(_IdPoliza);
                if (_Polizas.Count == 0) ContabilidadMetNueva(this, EventArgs.Empty);
                else ContabilidadMetIrA(Math.Min(Math.Max(Posicion, 0), _Polizas.Count - 1));
            }
            else
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, Mensaje);
            }
        }
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026

        private void ContabilidadMetProcesarPoliza(bool Aplicar)
        {
            if (!ContabilidadDgvDetalle.EndEdit()) return;
            ContabilidadMetRecalcularTotales();

            if (!_Validacion.ContabilidadFuncValidar(ContabilidadTxtDescripcion,
                string.IsNullOrWhiteSpace(ContabilidadTxtDescripcion.Text) ? "Escriba la descripción de la póliza." : null)) return;

            string Motivo = _Controlador.ContabilidadFuncValidar(ContabilidadTxtDescripcion.Text, _Lineas);
            if (!string.IsNullOrEmpty(Motivo))
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, Motivo);
                return;
            }

            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            if (Aplicar && !ClsDialogosContabilidad.ContabilidadFuncConfirmar(this,
                    "¿Desea validar la póliza? Una póliza validada ya no se podrá modificar.", "Validar"))
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
                return;

            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            string Estado = Aplicar ? "VALIDADA" : "BORRADOR";
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026

            ClsModeloPolizaContabilidad Modelo = new ClsModeloPolizaContabilidad
            {
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                IdPoliza = _IdPoliza,
                Periodo = _PeriodoOriginal,
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                Descripcion = ContabilidadTxtDescripcion.Text.Trim(),
                Fecha = ContabilidadDtpFecha.Value.Date,
                Estado = Estado,
                IdModulo = _IdModulo,
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                IdPais = ContabilidadCboPais.SelectedValue == null ? 0 : (int)ContabilidadCboPais.SelectedValue,
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                ModoActualizacion = _ModoActualizacion,
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                Lineas = _Lineas.ToList()
            };

            string Mensaje;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            EstadoEntidadContabilidad Accion = _IdPoliza == 0 ? EstadoEntidadContabilidad.Agregar : EstadoEntidadContabilidad.Modificar;
            if (_Controlador.ContabilidadFuncGrabarCambios(Accion, Modelo, out Mensaje))
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            {
                ClsDialogosContabilidad.ContabilidadMetExito(this, Mensaje);
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                if (!_Polizas.Contains(Modelo.IdPoliza)) _Polizas.Insert(0, Modelo.IdPoliza);
                ContabilidadMetCargarPoliza(Modelo.IdPoliza);
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            }
            else
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, Mensaje);
            }
        }

        private void ContabilidadMetGuardar(object Remitente, EventArgs Argumentos)
        {
            ContabilidadMetProcesarPoliza(false);
        }

        private void ContabilidadMetAplicar(object Remitente, EventArgs Argumentos)
        {
            ContabilidadMetProcesarPoliza(true);
        }

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
        private void ContabilidadMetMostrarAyuda(object Remitente, EventArgs Argumentos)
        {
            try
            {
                ClsDestinoAyuda Destino = new ClsCtrlAyuda().NavegadorFuncObtenerDestino(10006, GetType().Name);

                if (Destino.RutaArchivo == "")
                {
                    MessageBox.Show(this, Destino.Mensaje, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Destino.Pagina == "")
                {
                    Help.ShowHelp(this, Destino.RutaArchivo);
                }
                else
                {
                    Help.ShowHelp(this, Destino.RutaArchivo, HelpNavigator.Topic, Destino.Pagina);
                }
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(this, "No se pudo abrir la ayuda de Contabilidad.\n\n" + Excepcion.Message,
                    "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
    }
}
