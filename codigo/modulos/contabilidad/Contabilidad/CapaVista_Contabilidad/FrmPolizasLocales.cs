/*
 * Área: Contabilidad
 * Autor: Gabriel André Guillén Pocón
 * Carné: 0901-23-1998
 * Fecha: 14/10/2026
 * Propósito: prototipo no funcional de pólizas locales de Embutidos de Calidad,
 * basado en cuenta, poliza_encabezado y poliza_detalle de presupuesto_db.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Contabilidad;
using CapaVista_Contabilidad.Componentes;

namespace CapaVista_Contabilidad
{
    public partial class FrmPolizasLocales : Form
    {
        private readonly ClsControladorPolizasContabilidad _Controlador = new ClsControladorPolizasContabilidad();
        private readonly BindingList<ClsLineaPolizaContabilidad> _Lineas = new BindingList<ClsLineaPolizaContabilidad>();
        private readonly CultureInfo _Cultura = CultureInfo.GetCultureInfo("es-GT");
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();

        public FrmPolizasLocales()
        {
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Pólizas locales", new Size(1180, 688));
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
            ContabilidadBtnGuardar.Click += ContabilidadMetGuardar;
            ContabilidadBtnAplicar.Click += ContabilidadMetAplicar;
            ContabilidadDgvDetalle.CellContentClick += ContabilidadMetQuitarCuenta;
            ContabilidadDgvDetalle.CellEndEdit += ContabilidadMetTerminarEdicion;
            ContabilidadDgvDetalle.CellParsing += ContabilidadMetInterpretarImporte;
            ContabilidadDgvDetalle.DataError += ContabilidadMetErrorImporte;
            ContabilidadDgvDetalle.ColumnWidthChanged += ContabilidadMetAlinearTotales;
            ContabilidadDgvDetalle.SizeChanged += ContabilidadMetAlinearTotales;
            _Lineas.ListChanged += ContabilidadMetCambiarDetalle;
            ContabilidadMetNueva(this, EventArgs.Empty);
        }

        private void ContabilidadMetCargarCatalogos()
        {
            ContabilidadCboCuenta.DisplayMember = "Descripcion";
            ContabilidadCboCuenta.ValueMember = "IdCuenta";
            ContabilidadCboCuenta.DataSource = _Controlador.ContabilidadFuncObtenerCuentas();
            ContabilidadCboEstado.DataSource = _Controlador.ContabilidadFuncObtenerEstados();
            ContabilidadCboOrigen.DataSource = _Controlador.ContabilidadFuncObtenerOrigenes();
        }

        private void ContabilidadMetActualizarPeriodo(object Remitente, EventArgs Argumentos)
        {
            ContabilidadTxtPeriodo.Text = _Controlador.ContabilidadFuncObtenerPeriodo(ContabilidadDtpFecha.Value);
        }

        private void ContabilidadMetAgregarCuenta(object Remitente, EventArgs Argumentos)
        {
            ContabilidadDgvDetalle.EndEdit();
            string Nombre = ContabilidadCboCuenta.Text.Trim();
            ClsCuentaPolizaContabilidad Cuenta = ContabilidadCboCuenta.Items.Cast<ClsCuentaPolizaContabilidad>().FirstOrDefault(Elemento =>
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
            _Lineas.Add(new ClsLineaPolizaContabilidad
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
            ContabilidadDgvDetalle.EndEdit();
            ClsLineaPolizaContabilidad Linea = ContabilidadDgvDetalle.Rows[Argumentos.RowIndex].DataBoundItem as ClsLineaPolizaContabilidad;
            if (Linea != null) _Lineas.Remove(Linea);
        }

        private void ContabilidadMetInterpretarImporte(object Remitente, DataGridViewCellParsingEventArgs Argumentos)
        {
            if (Argumentos.ColumnIndex != ContabilidadColDebe.Index && Argumentos.ColumnIndex != ContabilidadColHaber.Index) return;
            string Texto = Convert.ToString(Argumentos.Value, _Cultura);
            decimal Importe;
            if (string.IsNullOrWhiteSpace(Texto)) Importe = 0m;
            else if (!decimal.TryParse(Texto, NumberStyles.Number, _Cultura, out Importe)) return;
            // DECIMAL(15,2): conservar dos decimales también en el valor enlazado.
            if (Importe < -9999999999999.99m || Importe > 9999999999999.99m)
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
            ClsLineaPolizaContabilidad Linea = ContabilidadDgvDetalle.Rows[Argumentos.RowIndex].DataBoundItem as ClsLineaPolizaContabilidad;
            if (Linea != null)
            {
                if (Argumentos.ColumnIndex == ContabilidadColDebe.Index) Linea.Debe = 0m;
                else Linea.Haber = 0m;
                _Lineas.ResetItem(Argumentos.RowIndex);
            }
            ContabilidadMetRecalcularTotales();
            ClsDialogosContabilidad.ContabilidadMetError(this, "Importe inválido. Ingrese un número dentro del rango DECIMAL(15,2). Se asignó 0.00.");
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
            ContabilidadCboOrigen.SelectedItem = "MANUAL";
            ContabilidadTxtDescripcion.Clear();
            ContabilidadCboCuenta.SelectedIndex = -1;
            ContabilidadCboCuenta.Text = string.Empty;
            ContabilidadMetRecalcularTotales();
            ContabilidadTxtDescripcion.Focus();
        }

        private void ContabilidadMetGuardar(object Remitente, EventArgs Argumentos)
        {
            ContabilidadMetValidarPrototipo(false);
        }

        private void ContabilidadMetAplicar(object Remitente, EventArgs Argumentos)
        {
            ContabilidadMetValidarPrototipo(true);
        }

        private void ContabilidadMetValidarPrototipo(bool Aplicar)
        {
            if (!ContabilidadDgvDetalle.EndEdit()) return;
            ContabilidadMetRecalcularTotales();
            string Motivo = _Controlador.ContabilidadFuncValidar(ContabilidadTxtDescripcion.Text, _Lineas);
            if (!_Validacion.ContabilidadFuncValidar(ContabilidadTxtDescripcion,
                string.IsNullOrWhiteSpace(ContabilidadTxtDescripcion.Text) ? "Escriba la descripción de la póliza." : null)) return;
            if (!string.IsNullOrEmpty(Motivo))
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, Motivo);
                return;
            }
            string Mensaje = Aplicar ? "La póliza está cuadrada y lista para aplicar." : "Póliza validada correctamente.";
            ClsDialogosContabilidad.ContabilidadMetExito(this, Mensaje);
        }
    }
}
