/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using CapaControlador_MovimientosBancarios;
using CapaControlador_MovimientosBancarios.Modelo_de_Controladores;
using CapaVista_MovimientosBancarios.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_MovimientosBancarios.Formularios
{
    public partial class FrmMovimientos : Form
    {
        public FrmMovimientos()
        {
            InitializeComponent();
            BancosMetLimpiarFechas();
        }

        private long _IdConceptoSeleccionado;
        private void BancosBtnConcepto_Click(object sender, EventArgs e)
        {
            using (FrmSeleccionarConcepto Conceptos = new FrmSeleccionarConcepto())
            {
                if (Conceptos.ShowDialog(this) == DialogResult.OK)
                {
                    _IdConceptoSeleccionado = Conceptos.IdConceptoSeleccionado;
                    BancosTxtConcepto.Text = Conceptos.NombreConceptoSeleccionado;
                    BancosTxtDescConc.Text = Conceptos.DescripcionSeleccionada;
                    BancosTxtTipoConc.Text = Conceptos.NaturalezaSeleccionada;
                    BancosTxtTipoBeneficiario.Text = Conceptos.TipoBeneficiarioSeleccionado;
                    BancosMetFiltrarTiposTransaccion();
                    _IdBeneficiarioSeleccionado = 0;
                    _TipoBeneficiarioSeleccionado = "";
                    BancosTxtTitular.Text = "";
                    BancosTxtCodigo.Text = "";
                    _IdBeneficiarioSeleccionado = 0;
                    _TipoBeneficiarioSeleccionado = "";
                    BancosCbCuentaTercero.Text = "";
                    BancosCbCuentaEmpresa.Text = "";
                    BancosTxtSaldoDisponible.Text = "";
                    BancosCbCuentaTercero.SelectedIndex = -1;
                    BancosCbCuentaTercero.Enabled = false;
                    BancosCbCuentaEmpresa.SelectedIndex = -1;
                    BancosCbCuentaEmpresa.Enabled = false;
                    BancosTxtSaldoDisponible.Clear();
                    BancosMetActualizarEstadoCuentaEmpresa();
                }
            }
        }

        private long _IdBeneficiarioSeleccionado = 0;
        private string _TipoBeneficiarioSeleccionado = "";

        private void BancosBtnLimpiar_Click(object sender, EventArgs e)
        {
            BancosTxtConcepto.Text = "";
            BancosTxtDescConc.Text = "";
            BancosTxtTipoConc.Text = ""; 
            BancosTxtTitular.Text = "";
            BancosTxtCodigo.Text = "";
            BancosTxtTipoBeneficiario.Text = "";
            BancosTxtConcepto.Clear();
            BancosTxtDescConc.Clear();
            BancosTxtTipoConc.Clear();
            BancosTxtTipoBeneficiario.Clear();
            BancosCbMetodo.SelectedIndex = -1;
            BancosTxtMetodo.Clear();
            BancosMetLimpiarFechas();
            _IdBeneficiarioSeleccionado = 0;
            _TipoBeneficiarioSeleccionado = "";
            BancosCbCuentaTercero.Text = "";
            BancosCbCuentaEmpresa.Text = "";
            BancosTxtSaldoDisponible.Text = "";
            BancosTxtMontoTotal.Text = "";
            _IdConceptoSeleccionado = 0;
            BancosCbCuentaTercero.SelectedIndex = -1;
            BancosCbCuentaTercero.Enabled = false;
            BancosCbCuentaEmpresa.SelectedIndex = -1;
            BancosCbCuentaEmpresa.Enabled = false;
            BancosTxtSaldoDisponible.Clear();
            BancosCbTipoTrans.SelectedIndex = -1;
            BancosCbTipoTrans.Enabled = false;
            BancosMetActualizarEstadoCuentaEmpresa();
            if (BancosCbTipoTrans.DataSource is DataTable Tabla)
            {
                Tabla.DefaultView.RowFilter = "";
            }
        }

        private readonly ClsModeloMetodoPago _ControladorMetodo = new ClsModeloMetodoPago();
        private void BancosMetCargarMetodos()
        {
            DataTable Metodos = _ControladorMetodo.BancosMetObtenerMetodosTabla();
            BancosCbMetodo.DataSource = null;
            BancosCbMetodo.DisplayMember = "Nombre";
            BancosCbMetodo.ValueMember = "ID";
            BancosCbMetodo.DataSource = Metodos;
            BancosCbMetodo.DropDownStyle = ComboBoxStyle.DropDownList;
            BancosCbMetodo.SelectedIndex = -1;
            BancosTxtMetodo.Text = "";
            BancosTxtMetodo.ReadOnly = true;
        }


        private void BancosCbMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BancosCbMetodo.SelectedItem is DataRowView Metodo)
            {
                BancosTxtMetodo.Text = Metodo["Descripcion"].ToString();
            }
            else
            {
                BancosTxtMetodo.Clear();
            }
        }

        private void FrmMovimientos_Load(object sender, EventArgs e)
        {
            try
            {
                BancosMetCargarMetodos();
                BancosMetCargarCuentasEmpresa();
                BancosMetCargarTiposTransaccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los métodos de pago: " + ex.Message);
            }

        }

        private void BancosMetLimpiarFechas()
        {
            BancosDtpFechaReg.Format = DateTimePickerFormat.Custom;
            BancosDtpFechaReg.CustomFormat = " ";
            BancosDtpFechaApl.Format = DateTimePickerFormat.Custom;
            BancosDtpFechaApl.CustomFormat = " ";
        }

        private void BancosDtpFechaReg_ValueChanged(object sender, EventArgs e)
        {
            BancosDtpFechaReg.CustomFormat = "dd/MM/yyyy";
        }

        private void BancosDtpFechaApl_ValueChanged(object sender, EventArgs e)
        {
            BancosDtpFechaApl.CustomFormat = "dd/MM/yyyy";
        }

        private void BancosMetBuscarBeneficiario()
        {
            string Tipo = BancosTxtTipoBeneficiario.Text.Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(Tipo))
            {
                MessageBox.Show("Primero debe seleccionar un concepto bancario.");
                return;
            }

            if (Tipo == "NINGUNO")
            {
                MessageBox.Show("Este concepto bancario no requiere beneficiario.");
                return;
            }

            if (Tipo != "CLIENTE" && Tipo != "PROVEEDOR" && Tipo != "EMPLEADO")
            {
                MessageBox.Show("El tipo de beneficiario no es válido.");
                return;
            }

            using (FrmSeleccionarBeneficiario Ventana = new FrmSeleccionarBeneficiario(Tipo))
            {
                if (Ventana.ShowDialog(this) == DialogResult.OK)
                {
                    _IdBeneficiarioSeleccionado = Ventana.IdBeneficiarioSeleccionado;
                    _TipoBeneficiarioSeleccionado = Tipo;                                                                       
                    BancosTxtTitular.Text = Ventana.NombreBeneficiarioSeleccionado;
                    BancosTxtCodigo.Text = Ventana.CodigoBeneficiarioSeleccionado;
                    BancosMetCargarCuentasTercero(_TipoBeneficiarioSeleccionado,_IdBeneficiarioSeleccionado);
                }
            }
        }

        private void BancosBtnBuscarBeneficiario_Click(object sender, EventArgs e)
        {
            BancosMetBuscarBeneficiario();
        }

        private readonly ClsModeloBeneficiario _ControladorBeneficiario = new ClsModeloBeneficiario();

        private void BancosMetCargarCuentasTercero(string TipoBeneficiario,long IdBeneficiario)
        {
            BancosCbCuentaTercero.DataSource = null;
            BancosCbCuentaTercero.Items.Clear();
            BancosCbCuentaTercero.Enabled = false;

            DataTable Tabla = _ControladorBeneficiario.BancosMetObtenerCuentasTerceroTabla(TipoBeneficiario, IdBeneficiario);

            BancosCbCuentaTercero.DisplayMember = "Descripcion";
            BancosCbCuentaTercero.ValueMember = "ID";
            BancosCbCuentaTercero.DataSource = Tabla;

            BancosCbCuentaTercero.SelectedIndex = -1;
            BancosCbCuentaTercero.Enabled = Tabla.Rows.Count > 0;
        }

        private readonly ClsModeloCuentaBancariaEmpresa _ControladorCuentaEmpresa = new ClsModeloCuentaBancariaEmpresa();
        private void BancosMetCargarCuentasEmpresa()
        {
            try
            {
                BancosCbCuentaEmpresa.DataSource = null;
                BancosTxtSaldoDisponible.Clear();

                DataTable Tabla = _ControladorCuentaEmpresa.BancosMetObtenerCuentasEmpresaTabla();

                BancosCbCuentaEmpresa.DisplayMember = "Descripcion";
                BancosCbCuentaEmpresa.ValueMember = "ID";
                BancosCbCuentaEmpresa.DataSource = Tabla;

                BancosCbCuentaEmpresa.SelectedIndex = -1;
                BancosCbCuentaEmpresa.Enabled = Tabla.Rows.Count > 0;
            }
            catch (Exception Ex)
            {
                BancosCbCuentaEmpresa.DataSource = null;
                BancosCbCuentaEmpresa.Enabled = false;
                BancosTxtSaldoDisponible.Clear();

                MessageBox.Show("No se pudieron cargar las cuentas empresariales:\n" + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BancosCbCuentaEmpresa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BancosTxtSaldoDisponible.Clear();

            if (BancosCbCuentaEmpresa.SelectedIndex < 0 ||BancosCbCuentaEmpresa.SelectedValue == null)
            {
                return;
            }

            try
            {
                long IdCuentaBancaria = Convert.ToInt64(BancosCbCuentaEmpresa.SelectedValue);

                decimal SaldoDisponible = _ControladorCuentaEmpresa.BancosMetObtenerSaldoDisponible(IdCuentaBancaria);
                BancosTxtSaldoDisponible.Text = SaldoDisponible.ToString("N2");
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudo obtener el saldo de la cuenta:\n" + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BancosMetCargarTiposTransaccion()
        {
            ClsModeloTipoTransaccion Controlador = new ClsModeloTipoTransaccion();
            var Tipos = Controlador.BancosMetObtenerTodos();
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("ID", typeof(long));
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Naturaleza", typeof(string));

            foreach (var Tipo in Tipos)
            {
                if (Tipo.IsActive)
                { 
                    Tabla.Rows.Add(Convert.ToInt64(Tipo.PK_TipoTransaccion), Tipo.NombreTipoTransaccion, Tipo.NaturalezaTipoTransaccion);
                }
            }

            BancosCbTipoTrans.DataSource = null;
            BancosCbTipoTrans.DisplayMember = "Nombre";
            BancosCbTipoTrans.ValueMember = "ID";
            BancosCbTipoTrans.DataSource = Tabla;
            BancosCbTipoTrans.DropDownStyle = ComboBoxStyle.DropDownList;
            BancosCbTipoTrans.SelectedIndex = -1;
            BancosCbTipoTrans.Enabled = false;
        }

        private bool BancosMetValidarMovimiento()
        {
            List<string> Errores = new List<string>();

            if (_IdConceptoSeleccionado <= 0 || string.IsNullOrWhiteSpace(BancosTxtConcepto.Text))
            {
                Errores.Add("Debe seleccionar un concepto bancario.");
            }

            string Naturaleza = BancosTxtTipoConc.Text.Trim().ToUpperInvariant();

            if (Naturaleza != "INGRESO" && Naturaleza != "EGRESO")
            {
                Errores.Add("La naturaleza debe ser INGRESO o EGRESO.");
            }

            if (BancosCbTipoTrans.SelectedIndex < 0 || !(BancosCbTipoTrans.SelectedItem is DataRowView FilaTipo) || !long.TryParse(Convert.ToString(FilaTipo["ID"]), out long IdTipoTransaccion) || IdTipoTransaccion <= 0)
            {
                Errores.Add("Debe seleccionar un tipo de transacción bancaria válido.");
            }
            else
            {
                string NaturalezaTipo = Convert.ToString(FilaTipo["Naturaleza"]).Trim().ToUpperInvariant();
                if (NaturalezaTipo != Naturaleza && NaturalezaTipo != "AMBOS")
                {
                    Errores.Add("La naturaleza del tipo de transacción " + "no corresponde al concepto bancario.");
                }
            }
            string Tipo = BancosTxtTipoBeneficiario.Text.Trim().ToUpperInvariant();
            bool TipoValido = Tipo == "NINGUNO" || Tipo == "CLIENTE" || Tipo == "PROVEEDOR" || Tipo == "EMPLEADO";

            if (!TipoValido)
            {
                Errores.Add("El tipo de beneficiario no es válido.");
            }

            bool RequiereTercero = Tipo == "CLIENTE" || Tipo == "PROVEEDOR" || Tipo == "EMPLEADO";

            if (RequiereTercero)
            {
                if (_IdBeneficiarioSeleccionado <= 0 || _TipoBeneficiarioSeleccionado != Tipo || string.IsNullOrWhiteSpace(BancosTxtTitular.Text) || string.IsNullOrWhiteSpace(BancosTxtCodigo.Text))
                {
                    Errores.Add("Debe seleccionar un beneficiario válido.");
                }

                if (BancosCbCuentaTercero.SelectedIndex < 0 || !(BancosCbCuentaTercero.SelectedItem is DataRowView FilaTercero) ||
                    !long.TryParse(Convert.ToString(FilaTercero["ID"]), out long IdCuentaTercero) || IdCuentaTercero <= 0)
                {
                    Errores.Add("Debe seleccionar una cuenta bancaria del beneficiario.");
                }
                else
                {
                    string MonedaTercero = Convert.ToString(FilaTercero["Moneda"]).Trim().ToUpperInvariant();

                    if (MonedaTercero != "GTQ")
                    {
                        Errores.Add("Por ahora solo se admiten cuentas de terceros en GTQ.");
                    }
                }
            }

            if (BancosCbMetodo.SelectedIndex < 0 || !(BancosCbMetodo.SelectedItem is DataRowView FilaMetodo) || !long.TryParse(Convert.ToString(FilaMetodo["ID"]), out long IdMetodo) || IdMetodo <= 0)
            {
                Errores.Add("Debe seleccionar una forma de pago válida.");
            }

            bool FechaRegistroValida = BancosDtpFechaReg.CustomFormat != " ";
            bool FechaAplicacionValida = BancosDtpFechaApl.CustomFormat != " ";

            if (!FechaRegistroValida) Errores.Add("Debe seleccionar la fecha de registro.");
            if (!FechaAplicacionValida) Errores.Add("Debe seleccionar la fecha de aplicación.");
            if (FechaRegistroValida && FechaAplicacionValida)
            {
                DateTime Registro = BancosDtpFechaReg.Value.Date;
                DateTime Aplicacion = BancosDtpFechaApl.Value.Date;

                if (Aplicacion < Registro)
                {
                    Errores.Add("La fecha de aplicación no puede ser anterior " + "a la fecha de registro.");
                }

                if (Registro > DateTime.Today)
                {
                    Errores.Add("La fecha de registro no puede ser futura.");
                }

                if (Aplicacion > DateTime.Today)
                {
                    Errores.Add("La fecha de aplicación no puede ser futura.");
                }
            }

            string TextoMonto = BancosTxtMontoTotal.Text.Trim();
            bool MontoValido = decimal.TryParse(TextoMonto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal Monto);

            if (!MontoValido)
            {
                Errores.Add("Debe ingresar un monto numérico válido.");
            }
            else
            {
                if (Monto <= 0)
                {
                    Errores.Add("El monto debe ser mayor que cero.");
                }

                if (Monto >= 100000000m)
                {
                    Errores.Add("El monto supera el máximo permitido por la base de datos.");
                }

                if (decimal.Round(Monto, 4) != Monto)
                {
                    Errores.Add("El monto no puede superar cuatro decimales.");
                }
            }

            bool CuentaEmpresaValida = false;

            if (BancosCbCuentaEmpresa.SelectedIndex < 0 || !(BancosCbCuentaEmpresa.SelectedItem is DataRowView FilaEmpresa) || !long.TryParse(Convert.ToString(FilaEmpresa["ID"]), out long IdCuentaEmpresa) || IdCuentaEmpresa <= 0)
            {
                Errores.Add("Debe seleccionar una cuenta empresarial.");
            }
            else
            {
                CuentaEmpresaValida = true;
                string MonedaEmpresa = Convert.ToString(FilaEmpresa["Moneda"]).Trim().ToUpperInvariant();

                if (MonedaEmpresa != "GTQ")
                {
                    Errores.Add("Por ahora solo se admiten cuentas empresariales en GTQ.");
                }
            }

            bool SaldoValido = decimal.TryParse(BancosTxtSaldoDisponible.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal SaldoDisponible);

            if (CuentaEmpresaValida && !SaldoValido)
            {
                Errores.Add("No se pudo determinar el saldo disponible de la cuenta.");
            }

            if (Naturaleza == "EGRESO" && MontoValido && Monto > 0 && SaldoValido && CuentaEmpresaValida)
            {
                if (Monto > SaldoDisponible)
                {
                    Errores.Add( "Fondos insuficientes. El monto supera " + "el saldo disponible de la cuenta.");
                }
            }

            if (Errores.Count > 0)
            {
                string Mensaje = "Se encontraron los siguientes problemas:\n\n" + string.Join("\n\n", Errores.Select((Error, Indice) => $"{Indice + 1}. {Error}"));
                MessageBox.Show(Mensaje, "Validación del movimiento bancario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private void BancosBtnRegistra_Click(object sender, EventArgs e)
        {
            if (!BancosMetValidarMovimiento()) return;

            string Concepto = BancosTxtConcepto.Text.Trim();
            string Naturaleza = BancosTxtTipoConc.Text.Trim();
            string TipoBeneficiario = BancosTxtTipoBeneficiario.Text.Trim();

            string Beneficiario =
                TipoBeneficiario.ToUpperInvariant() == "NINGUNO"
                ? "No aplica"
                : BancosTxtTitular.Text.Trim();

            string MetodoPago = BancosCbMetodo.Text;
            string TipoTransaccion = BancosCbTipoTrans.Text;
            string CuentaEmpresa = BancosCbCuentaEmpresa.Text;

            string CuentaTercero =
                TipoBeneficiario.ToUpperInvariant() == "NINGUNO"
                ? "No aplica"
                : BancosCbCuentaTercero.Text;

            string FechaOperacion =
                BancosDtpFechaReg.Value.ToString("dd/MM/yyyy");

            string FechaAplicacion =
                BancosDtpFechaApl.Value.ToString("dd/MM/yyyy");

            decimal Monto = decimal.Parse(
                BancosTxtMontoTotal.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture);

            string Resumen =
                $"Concepto: {Concepto}\n" +
                $"Naturaleza: {Naturaleza}\n" +
                $"Forma de pago: {MetodoPago}\n" +
                $"Tipo de transacción: {TipoTransaccion}\n\n" +
                $"Beneficiario: {Beneficiario}\n" +
                $"Cuenta beneficiario: {CuentaTercero}\n\n" +
                $"Cuenta empresarial: {CuentaEmpresa}\n" +
                $"Saldo disponible: Q{BancosTxtSaldoDisponible.Text}\n\n" +
                $"Fecha de operación: {FechaOperacion}\n" +
                $"Fecha de aplicación: {FechaAplicacion}\n\n" +
                $"Monto del movimiento: Q{Monto:N2}\n\n" +
                "¿Desea continuar con el registro?";

            DialogResult Respuesta = MessageBox.Show(
                Resumen,
                "Confirmar movimiento bancario",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2
            );

            if (Respuesta != DialogResult.Yes)
                return;

            BancosBtnRegistra.Enabled = false;

            try
            {
                long IdMovimiento =
                    BancosMetRegistrarMovimientoDesdeFormulario();

                MessageBox.Show(
                    "Movimiento bancario registrado correctamente.\n\n" +
                    $"ID: {IdMovimiento}\n" +
                    $"Referencia: {_ReferenciaMovimientoPendiente}\n" +
                    $"Monto: Q{Monto:N2}",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                _ReferenciaMovimientoPendiente = null;
                BancosBtnLimpiar_Click(sender, EventArgs.Empty);
            }

            catch (Exception Ex)
            {
                string DetalleError = Ex.ToString();

                string RutaError = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"ErrorMovimientoBancario.txt"
                );

                try
                {
                    System.IO.File.WriteAllText(
                        RutaError,
                        DetalleError
                    );
                }
                catch (Exception ErrorArchivo)
                {
                    MessageBox.Show(
                        "No se pudo guardar el diagnóstico:\n" +
                        ErrorArchivo.Message
                    );
                }

                MessageBox.Show(
                    "No se pudo registrar el movimiento.\n\n" +
                    "Tipo de error: " + Ex.GetType().Name + "\n" +
                    "Mensaje: " + Ex.Message + "\n\n" +
                    "Referencia: " +
                    (_ReferenciaMovimientoPendiente ?? "No generada") +
                    "\n\nEl detalle técnico se intentó guardar " +
                    "en el escritorio.",
                    "Error de registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

        }

        private void BancosMetFiltrarTiposTransaccion()
        {
            string Naturaleza = BancosTxtTipoConc.Text.Trim().ToUpperInvariant();
            BancosCbTipoTrans.SelectedIndex = -1;
            BancosCbTipoTrans.Enabled = false;

            if (!(BancosCbTipoTrans.DataSource is DataTable Tabla))
                return;

            DataView Vista = Tabla.DefaultView;

            if (Naturaleza == "INGRESO" || Naturaleza == "EGRESO")
            {
                Vista.RowFilter =
                    $"Naturaleza = '{Naturaleza}' OR Naturaleza = 'AMBOS'";

                BancosCbTipoTrans.Enabled = Vista.Count > 0;
            }
            else
            {
                Vista.RowFilter = "1 = 0";
            }

            BancosCbTipoTrans.SelectedIndex = -1;
        }


        private void BancosMetActualizarEstadoCuentaEmpresa()
        {
            string Tipo = BancosTxtTipoBeneficiario.Text
                .Trim()
                .ToUpperInvariant();

            bool ConceptoValido = _IdConceptoSeleccionado > 0;

            bool RequiereBeneficiario = Tipo == "CLIENTE" || Tipo == "PROVEEDOR" || Tipo == "EMPLEADO";
            bool BeneficiarioSeleccionado = _IdBeneficiarioSeleccionado > 0 && _TipoBeneficiarioSeleccionado == Tipo;
            bool CuentaTerceroSeleccionada = BancosCbCuentaTercero.SelectedIndex >= 0 && BancosCbCuentaTercero.SelectedValue != null;
            bool Habilitar = ConceptoValido && (Tipo == "NINGUNO" || (RequiereBeneficiario && BeneficiarioSeleccionado && CuentaTerceroSeleccionada));

            BancosCbCuentaEmpresa.Enabled =Habilitar && BancosCbCuentaEmpresa.Items.Count > 0;

            if (!Habilitar)
            {
                BancosCbCuentaEmpresa.SelectedIndex = -1;
                BancosTxtSaldoDisponible.Clear();
            }
        }

        private void BancosCbCuentaTercero_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BancosMetActualizarEstadoCuentaEmpresa();
        }

        private readonly ClsModeloMovimientoBancario _ControladorMovimiento = new ClsModeloMovimientoBancario();
        private string _ReferenciaMovimientoPendiente;
        private long BancosMetRegistrarMovimientoDesdeFormulario()
        {
            string Tipo = BancosTxtTipoBeneficiario.Text
                .Trim().ToUpperInvariant();

            bool TieneTercero = Tipo != "NINGUNO";

            decimal Monto = decimal.Parse(
                BancosTxtMontoTotal.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture);

            if (string.IsNullOrEmpty(_ReferenciaMovimientoPendiente))
            {
                _ReferenciaMovimientoPendiente =
                    "MB-" + Guid.NewGuid().ToString("N");
            }

            return _ControladorMovimiento.BancosMetRegistrarMovimientoDesdeVista(
                _ReferenciaMovimientoPendiente,
                _IdConceptoSeleccionado,
                Convert.ToInt64(BancosCbMetodo.SelectedValue),
                Convert.ToInt64(BancosCbTipoTrans.SelectedValue),
                Convert.ToInt64(BancosCbCuentaEmpresa.SelectedValue),
                TieneTercero
                    ? (long?)Convert.ToInt64(BancosCbCuentaTercero.SelectedValue)
                    : null,
                TieneTercero
                    ? (long?)_IdBeneficiarioSeleccionado
                    : null,
                Tipo,
                BancosTxtConcepto.Text.Trim(),
                BancosTxtTipoConc.Text.Trim().ToUpperInvariant(),
                Monto,
                BancosDtpFechaReg.Value.Date,
                BancosDtpFechaApl.Value.Date
            );
        }

        private void BancosBtnSalir_Click(object sender, EventArgs e)
        {
            FrmMovimientosBancarios principal =
        this.ParentForm as FrmMovimientosBancarios;

            if (principal != null)
            {
                principal.MostrarFormulario(new FrmListaMovimientos());
            }
        }

        private void BancosBtnAyudaS_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "C:/BancosAyudas/BancosAyudas.chm", "MovimientoBancario_Bancos.html");
        }
    }
}
