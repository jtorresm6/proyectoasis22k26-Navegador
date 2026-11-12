using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using CapaControlador_Polizas;

namespace CapaVista_Polizas
{
    public partial class FrmPolizas : Form
    {
        private readonly ClsControladorPolizasCompras _Controlador = new ClsControladorPolizasCompras();
        private List<ClsModeloCuentaPolizaCompras> _Cuentas = new List<ClsModeloCuentaPolizaCompras>();
        private DataGridView _Lineas;

        public FrmPolizas()
        {
            InitializeComponent();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasBtnIngresarCuenta.Enabled = true;
            ComprasBtnNuevo.Enabled = false;
            ComprasBtnGuardar.Enabled = false;
            ComprasBtnEnviar.Enabled = true;
            ComprasBtnNuevo.Visible = false;
            ComprasBtnGuardar.Visible = false;
            ComprasLblDiferencia.Text = "Total Debe: Q 0.00    Total Haber: Q 0.00    Diferencia: Q 0.00";
            ComprasLblDiferencia.Texto = ComprasLblDiferencia.Text;
            ComprasLblCuenta.MostrarAsterisco = true;
            ComprasCboCuenta.Entrada.Validating += (Remitente, Evento) =>
            {
                string Codigo = Convert.ToString(ComprasCboCuenta.Entrada.SelectedValue);
                if (string.IsNullOrEmpty(Codigo)) return;
                bool EsValido = ClsCuentaContable.ComprasFuncFormatoValido(Codigo);
                Evento.Cancel = !EsValido;
                if (EsValido) ComprasCboCuenta.ComprasMetLimpiarError();
                else ComprasCboCuenta.ComprasMetMostrarError("Use el formato C.GG.CC.SSS; por ejemplo, 1.01.02.001.");
            };
            ComprasBtnIngresarCuenta.Click += ComprasBtnIngresarCuenta_Click;
            ComprasBtnEnviar.Click += ComprasBtnEnviar_Click;
            ComprasCboOrigen.Entrada.SelectedIndexChanged += ComprasCboOrigen_SelectedIndexChanged;
            Load += FrmPolizas_Load;
        }

        private void FrmPolizas_Load(object Remitente, EventArgs Evento)
        {
            try
            {
                _Cuentas = _Controlador.ContabilidadFuncCuentasCompras();
                ComprasCboCuenta.Entrada.DataSource = _Cuentas;
                ComprasCboCuenta.Entrada.DisplayMember = "Texto";
                ComprasCboCuenta.Entrada.ValueMember = "Codigo";

                _Lineas = ComprasDgvLineasPoliza;
                _Lineas.AutoGenerateColumns = false;
                DataGridViewComboBoxColumn ColumnaCuenta = (DataGridViewComboBoxColumn)_Lineas.Columns["Cuenta"];
                ColumnaCuenta.DataSource = _Cuentas.ToArray();
                ColumnaCuenta.DisplayMember = "Texto";
                ColumnaCuenta.ValueMember = "Codigo";
                ColumnaCuenta.ReadOnly = true;
                _Lineas.CellEndEdit += (EmisorCuadricula, Argumentos) => ComprasMetActualizarTotales();
                _Lineas.RowsRemoved += (EmisorCuadricula, Argumentos) => ComprasMetActualizarTotales();
                _Lineas.CellValueChanged += (EmisorCuadricula, Argumentos) => ComprasMetActualizarTotales();
                _Lineas.DataError += (EmisorCuadricula, Argumentos) => Argumentos.ThrowException = false;

                DataTable CuentasPorPagar = _Controlador.ComprasFuncConsultarCuentasPorPagar();
                ComprasCboOrigen.Entrada.DropDownStyle = ComboBoxStyle.DropDownList;
                ComprasCboOrigen.Entrada.DisplayMember = "TextoOrigen";
                ComprasCboOrigen.Entrada.ValueMember = "DocumentoOrigen";
                ComprasCboOrigen.Entrada.DataSource = CuentasPorPagar;
                ComprasCboOrigen.Entrada.SelectedIndex = -1;
                if (_Cuentas.Count == 0)
                    MessageBox.Show("Contabilidad no devolvió cuentas permitidas para MOD-COMPR.", "Pólizas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudieron cargar las cuentas o documentos de Compras:" + Environment.NewLine + ComprasFuncObtenerErrorCompleto(Ex),
                    "Pólizas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string ComprasFuncObtenerErrorCompleto(Exception Ex)
        {
            System.Text.StringBuilder Mensaje = new System.Text.StringBuilder();
            for (Exception Actual = Ex; Actual != null; Actual = Actual.InnerException)
            {
                if (Mensaje.Length > 0) Mensaje.AppendLine();
                Mensaje.Append(Actual.Message);
            }
            return Mensaje.ToString();
        }

        private void ComprasCboOrigen_SelectedIndexChanged(object Remitente, EventArgs Evento)
        {
            DataRowView Fila = ComprasCboOrigen.Entrada.SelectedItem as DataRowView;
            if (Fila == null) return;
            DateTime Fecha = Convert.ToDateTime(Fila["Fecha"]);
            ComprasCboFecha.Entrada.Text = Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            ComprasTxtDescripcion.Text = "Cuenta por pagar " + Convert.ToString(Fila["DocumentoOrigen"]) +
                " - " + Convert.ToString(Fila["Proveedor"]);
            ComprasCboEstado.Entrada.Text = Convert.ToString(Fila["Estado"]);
            ComprasTxtPeriodo.Text = Fecha.ToString("yyyy-MM", CultureInfo.InvariantCulture);
        }

        private void ComprasBtnIngresarCuenta_Click(object Remitente, EventArgs Evento)
        {
            if (_Lineas == null || ComprasCboCuenta.Entrada.SelectedValue == null) return;
            int Indice = _Lineas.Rows.Add();
            _Lineas.Rows[Indice].Cells["Cuenta"].Value = Convert.ToString(ComprasCboCuenta.Entrada.SelectedValue);
            _Lineas.CurrentCell = _Lineas.Rows[Indice].Cells["Debe"];
            _Lineas.BeginEdit(true);
            ComprasMetActualizarTotales();
        }

        private void ComprasMetActualizarTotales()
        {
            decimal TotalDebe = 0m;
            decimal TotalHaber = 0m;
            if (_Lineas != null)
            {
                foreach (DataGridViewRow Fila in _Lineas.Rows)
                {
                    if (Fila.IsNewRow) continue;
                    ComprasFuncLeerImporte(Fila.Cells["Debe"].Value, out decimal Debe);
                    ComprasFuncLeerImporte(Fila.Cells["Haber"].Value, out decimal Haber);
                    TotalDebe += Debe;
                    TotalHaber += Haber;
                }
            }

            decimal Diferencia = Math.Abs(TotalDebe - TotalHaber);
            ComprasLblDiferencia.Text = string.Format(CultureInfo.CurrentCulture,
                "Total Debe: Q {0:N2}    Total Haber: Q {1:N2}    Diferencia: Q {2:N2}", TotalDebe, TotalHaber, Diferencia);
            ComprasLblDiferencia.Texto = ComprasLblDiferencia.Text;
        }

        private static bool ComprasFuncLeerImporte(object Valor, out decimal Importe)
        {
            string Texto = Convert.ToString(Valor);
            if (string.IsNullOrWhiteSpace(Texto))
            {
                Importe = 0m;
                return true;
            }
            return decimal.TryParse(Texto, NumberStyles.Number, CultureInfo.CurrentCulture, out Importe)
                || decimal.TryParse(Texto, NumberStyles.Number, CultureInfo.InvariantCulture, out Importe);
        }

        private void ComprasBtnEnviar_Click(object Remitente, EventArgs Evento)
        {
            if (_Lineas != null)
            {
                if (_Lineas.IsCurrentCellInEditMode) _Lineas.EndEdit();
                _Lineas.CommitEdit(DataGridViewDataErrorContexts.Commit);
                _Lineas.EndEdit();
            }
            DataRowView Factura = ComprasCboOrigen.Entrada.SelectedItem as DataRowView;
            if (Factura == null)
            {
                MessageBox.Show("Seleccione primero un documento en el campo Origen para enviar la póliza.", "Pólizas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!DateTime.TryParse(ComprasCboFecha.Entrada.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime Fecha))
            {
                MessageBox.Show("Indique una fecha válida para la póliza.", "Pólizas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var Poliza = new ClsModeloPolizaCompras
            {
                DocumentoOrigen = Convert.ToString(Factura["DocumentoOrigen"]),
                Fecha = Fecha.Date,
                Descripcion = ComprasTxtDescripcion.Text,
                Lineas = new List<ClsModeloLineaPolizaCompras>()
            };
            foreach (DataGridViewRow Fila in _Lineas.Rows)
            {
                if (Fila.IsNewRow) continue;
                string Cuenta = Convert.ToString(Fila.Cells["Cuenta"].Value);
                if (!ComprasFuncLeerImporte(Fila.Cells["Debe"].Value, out decimal Debe) ||
                    !ComprasFuncLeerImporte(Fila.Cells["Haber"].Value, out decimal Haber))
                {
                    MessageBox.Show("Revise el importe de Debe y Haber en la fila " + (Fila.Index + 1) + ". Use solo números.",
                        "Pólizas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cuenta) || (Debe == 0m && Haber == 0m)) continue;
                Poliza.Lineas.Add(new ClsModeloLineaPolizaCompras
                {
                    CodigoCuenta = Cuenta,
                    Debe = Debe,
                    Haber = Haber,
                    Descripcion = Convert.ToString(Fila.Cells["Descripcion"].Value)
                });
            }

            bool Aceptada = _Controlador.ContabilidadFuncEnviarPoliza(Poliza,
                out string Codigo, out string Estado, out string Mensaje);
            ComprasCboEstado.Entrada.Text = Estado;
            MessageBox.Show(Mensaje + (string.IsNullOrEmpty(Codigo) ? string.Empty : Environment.NewLine + "Código: " + Codigo),
                "Pólizas de Compras", MessageBoxButtons.OK,
                Aceptada ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (Aceptada) ComprasMetLimpiarTabla();
        }

        private void ComprasMetLimpiarTabla()
        {
            if (_Lineas != null) _Lineas.Rows.Clear();
            ComprasMetActualizarTotales();
        }
    }
}
