/*
 * Área: Contabilidad
 * Autor: Oskar Cermeño
 * Carné: 0901-23-15379
 * Fecha: 14/10/2026
 * Propósito: prototipo no funcional de pólizas locales de Embutidos de Calidad,
 * basado en tblCuenta, tblPolizaEncabezado y tblPolizaDetalle de dbContabilidad.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaControlador_Contabilidad;
using CapaVista_Contabilidad.Componentes;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

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
        {
            _IdModulo = IdModulo;
            InitializeComponent();
            _Permisos = Permisos;
            ContabilidadMetAplicarPermisos(Permisos);
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Pólizas locales", new Size(1180, 688));
            ContabilidadLblTitulo.Text = "Póliza " + _Controlador.ContabilidadFuncObtenerNombreModulo(_IdModulo);
            foreach (Control Campo in new Control[] { ContabilidadTxtDescripcion, ContabilidadDtpFecha, ContabilidadCboEstado, ContabilidadCboOrigen, ContabilidadCboCuenta })
                _Validacion.ContabilidadMetObligatorio(Campo);
            Disposed += (Remitente, Argumentos) => _Validacion.Dispose();
            ContabilidadColDebe.DefaultCellStyle.FormatProvider = _Cultura;
            ContabilidadColHaber.DefaultCellStyle.FormatProvider = _Cultura;
            ContabilidadMetCargarCatalogos();
            ContabilidadDgvDetalle.DataSource = _Lineas;
            ContabilidadDtpFecha.ValueChanged += ContabilidadMetActualizarPeriodo;
            ContabilidadBtnAgregar.Click += ContabilidadMetAgregarCuenta;
            ContabilidadBtnNueva.Click += ContabilidadMetNueva;
            ContabilidadBtnLimpiarDetalle.Click += ContabilidadMetLimpiarDetalle;
            ContabilidadBtnAplicar.Click += ContabilidadMetAplicar;
            ContabilidadBtnGuardar.Click += ContabilidadMetGuardar;
            ContabilidadDgvDetalle.CellContentClick += ContabilidadMetQuitarCuenta;
            ContabilidadDgvDetalle.CellEndEdit += ContabilidadMetTerminarEdicion;
            ContabilidadDgvDetalle.CellParsing += ContabilidadMetInterpretarImporte;
            ContabilidadDgvDetalle.DataError += ContabilidadMetErrorImporte;
            ContabilidadDgvDetalle.ColumnWidthChanged += ContabilidadMetAlinearTotales;
            ContabilidadDgvDetalle.SizeChanged += ContabilidadMetAlinearTotales;
            _Lineas.ListChanged += ContabilidadMetCambiarDetalle;
            ContabilidadMetNueva(this, EventArgs.Empty);
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAplicarPermisos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnNueva, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnAgregar, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnGuardar, Permisos.PuedeInsertar);
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
            ContabilidadCboEstado.DataSource = _Controlador.ContabilidadFuncObtenerEstados();
            ContabilidadCboOrigen.DataSource = new[] { _Controlador.ContabilidadFuncObtenerNombreModulo(_IdModulo) };
            ContabilidadCboOrigen.Enabled = false;
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
            if (!_Permisos.PuedeEliminar) return;
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
            ContabilidadLblTotalDebe.Text = "Total debe: Q " + Debe.ToString("N2", _Cultura);
            ContabilidadLblTotalHaber.Text = "Total haber: Q " + Haber.ToString("N2", _Cultura);
            if (_Lineas.Count == 0)
            {
                ContabilidadLblCuadre.Text = "Agregue las cuentas de la póliza";
                ContabilidadLblCuadre.ForeColor = Color.FromArgb(70, 104, 125);
            }
            else
            {
                ContabilidadLblCuadre.Text = Cuadrada ? "Cuadrada" : "Descuadrada · diferencia Q " + Math.Abs(Debe - Haber).ToString("N2", _Cultura);
                ContabilidadLblCuadre.ForeColor = Cuadrada ? Color.FromArgb(70, 104, 125) : Color.FromArgb(197, 143, 62);
            }
            ContabilidadBtnAplicar.Enabled = Cuadrada && _Lineas.Count >= 2;
            ContabilidadBtnGuardar.Enabled = _Lineas.Count > 0;
            ContabilidadBtnLimpiarDetalle.Enabled = _Lineas.Count > 0;
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
            ContabilidadMetLimpiarDetalle(Remitente, Argumentos);
            ContabilidadDtpFecha.Value = DateTime.Today;
            ContabilidadMetActualizarPeriodo(this, EventArgs.Empty);
            ContabilidadCboEstado.SelectedItem = "BORRADOR";
            ContabilidadTxtDescripcion.Clear();
            ContabilidadCboCuenta.SelectedIndex = -1;
            ContabilidadCboCuenta.Text = string.Empty;
            ContabilidadMetRecalcularTotales();
            ContabilidadTxtDescripcion.Focus();
        }

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

            string Estado = Aplicar ? "VALIDADA" : Convert.ToString(ContabilidadCboEstado.SelectedItem);

            ClsModeloPolizaContabilidad Modelo = new ClsModeloPolizaContabilidad
            {
                Descripcion = ContabilidadTxtDescripcion.Text.Trim(),
                Fecha = ContabilidadDtpFecha.Value.Date,
                Estado = Estado,
                IdModulo = _IdModulo,
                Lineas = _Lineas.ToList()
            };

            string Mensaje;
            if (_Controlador.ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad.Agregar, Modelo, out Mensaje))
            {
                ClsDialogosContabilidad.ContabilidadMetExito(this, Mensaje);
                ContabilidadMetNueva(this, EventArgs.Empty);
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
    }
}
