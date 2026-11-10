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
            ComprasGbxDetalle.Height = 390;
            ComprasLblDiferencia.Location = new System.Drawing.Point(10, 315);
            ComprasLblDiferencia.Size = new System.Drawing.Size(900, 32);
            ComprasBtnEnviar.Location = new System.Drawing.Point(1146, 300);
            ComprasLblDiferencia.Text = "Total Debe: Q 0.00    Total Haber: Q 0.00    Diferencia: Q 0.00";
            ComprasLblDiferencia.Texto = ComprasLblDiferencia.Text;
            ComprasLblCuenta.MostrarAsterisco = true;
            ComprasCboCuenta.Entrada.Validating += (Remitente, Evento) =>
            {
                if (string.IsNullOrEmpty(ComprasCboCuenta.Text)) return;
                bool EsValido = ClsCuentaContable.ComprasFuncFormatoValido(ComprasCboCuenta.Text);
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

                _Lineas = new DataGridView
                {
                    Name = "ComprasDgvLineasPoliza",
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = true,
                    AutoGenerateColumns = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.CellSelect,
                    Location = new System.Drawing.Point(10, 125),
                    Size = new System.Drawing.Size(1220, 165),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                _Lineas.Columns.Add(new DataGridViewComboBoxColumn
                {
                    Name = "Cuenta", HeaderText = "Cuenta", DataSource = _Cuentas.ToArray(),
                    DisplayMember = "Texto", ValueMember = "Codigo", Width = 270
                });
                _Lineas.Columns["Cuenta"].ReadOnly = true;
                _Lineas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Debe", HeaderText = "Debe", Width = 125 });
                _Lineas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Haber", HeaderText = "Haber", Width = 125 });
                _Lineas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Descripcion", HeaderText = "Descripción", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                _Lineas.CellEndEdit += (EmisorCuadricula, Argumentos) => ComprasMetActualizarTotales();
                _Lineas.RowsRemoved += (EmisorCuadricula, Argumentos) => ComprasMetActualizarTotales();
                _Lineas.CellValueChanged += (EmisorCuadricula, Argumentos) => ComprasMetActualizarTotales();
                ComprasGbxDetalle.Controls.Add(_Lineas);

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
                MessageBox.Show("No se pudieron cargar las cuentas o documentos de Compras: " + Ex.Message,
                    "Pólizas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    decimal.TryParse(Convert.ToString(Fila.Cells["Debe"].Value), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal Debe);
                    decimal.TryParse(Convert.ToString(Fila.Cells["Haber"].Value), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal Haber);
                    TotalDebe += Debe;
                    TotalHaber += Haber;
                }
            }

            decimal Diferencia = Math.Abs(TotalDebe - TotalHaber);
            ComprasLblDiferencia.Text = string.Format(CultureInfo.CurrentCulture,
                "Total Debe: Q {0:N2}    Total Haber: Q {1:N2}    Diferencia: Q {2:N2}", TotalDebe, TotalHaber, Diferencia);
            ComprasLblDiferencia.Texto = ComprasLblDiferencia.Text;
        }

        private void ComprasBtnEnviar_Click(object Remitente, EventArgs Evento)
        {
            if (_Lineas != null) _Lineas.EndEdit();
            DataRowView Factura = ComprasCboOrigen.Entrada.SelectedItem as DataRowView;
            if (Factura == null)
            {
                MessageBox.Show("Seleccione una cuenta por pagar pendiente.", "Pólizas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (string.IsNullOrWhiteSpace(Cuenta)) continue;
                decimal.TryParse(Convert.ToString(Fila.Cells["Debe"].Value), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal Debe);
                decimal.TryParse(Convert.ToString(Fila.Cells["Haber"].Value), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal Haber);
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
        }
    }
}
