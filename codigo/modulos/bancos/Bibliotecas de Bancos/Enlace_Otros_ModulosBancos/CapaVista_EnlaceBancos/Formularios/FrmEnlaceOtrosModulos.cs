using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_EnlaceB;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceOtrosModulos : Form
    {
        private readonly ClsControladorEnlaceB _Controlador;

        public FrmEnlaceOtrosModulos()
        {
            InitializeComponent();

            _Controlador = new ClsControladorEnlaceB();

            // DataPropertyName — BancDgvMovimientos
            Estatus.DataPropertyName     = "Estatus";
            Estatus.TrueValue            = 1;
            Estatus.FalseValue           = 0;
            Fecha.DataPropertyName       = "FechaPoliza";
            Referencia.DataPropertyName  = "ReferenciaPoliza";
            Concepto.DataPropertyName    = "ConceptoPoliza";
            Póliza.DataPropertyName      = "NumeroPoliza";
            Importe_GTQ.DataPropertyName = "ImporteTotalPoliza";

            // DataPropertyName — BancDgvPartidaContable
            Código_de_Cuenta.DataPropertyName = "CodigoCuentaDetallePartida";
            Nombre_de_cuenta.DataPropertyName = "NombreCuentaDetallePartida";
            Debe.DataPropertyName             = "DebeDetallePartida";
            Haber.DataPropertyName            = "HaberDetallePartida";

            // Grids de sólo lectura
            BancDgvMovimientos.ReadOnly     = true;
            BancDgvPartidaContable.ReadOnly = true;

            // Suscripción de eventos
            BancBtnConsultarMovimientos.Click   += BancBtnConsultarMovimientos_Click;
            BancBtnGenerarPartida.Click         += BancBtnGenerarPartida_Click;
            BancosBtnEnviar.Click               += BancosBtnEnviar_Click;
            BancosBtnLimpiar.Click              += BancosBtnLimpiar_Click;
            BancBtnCerrar.Click                 += BancBtnCerrar_Click;
            BancCbxPeriodo.SelectedIndexChanged += BancCbxPeriodo_SelectedIndexChanged;
        }

        // ----------------------------------------------------------------
        // Consultar movimientos del período
        // ----------------------------------------------------------------
        private void BancBtnConsultarMovimientos_Click(object sender, EventArgs e)
        {
            string Error = _Controlador.BancosMetConsultarMovimientos(
                dateTimePicker1.Value, dateTimePicker2.Value, out DataTable Tabla);

            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BancDgvMovimientos.DataSource = Tabla;
        }

        // ----------------------------------------------------------------
        // Generar partida consolidada
        // ----------------------------------------------------------------
        private void BancBtnGenerarPartida_Click(object sender, EventArgs e)
        {
            string Error = _Controlador.BancosMetGenerarPartida(
                dateTimePicker1.Value, dateTimePicker2.Value, out DataTable Detalle);

            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BancDgvPartidaContable.DataSource = Detalle;
            BancosActualizarTotales(Detalle);

            // Refrescar grilla de movimientos para reflejar estado PENDIENTE_ENVIO
            _Controlador.BancosMetConsultarMovimientos(
                dateTimePicker1.Value, dateTimePicker2.Value, out DataTable Movimientos);
            BancDgvMovimientos.DataSource = Movimientos;
        }

        // ----------------------------------------------------------------
        // Enviar partida a Contabilidad
        // ----------------------------------------------------------------
        private void BancosBtnEnviar_Click(object sender, EventArgs e)
        {
            if (_Controlador.BancsFuncObtenerPkPartidaActual() <= 0)
            {
                MessageBox.Show(
                    "Debe generar la partida contable antes de enviar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult Confirmacion = MessageBox.Show(
                "¿Confirma el traslado de la partida a Contabilidad?",
                "Confirmar traslado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (Confirmacion != DialogResult.Yes)
                return;

            string Error = _Controlador.BancosMetEnviarPartida(
                dateTimePicker1.Value, dateTimePicker2.Value);

            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "Error al enviar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(
                "Partida trasladada correctamente a Contabilidad.",
                "Operación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

            BancosBtnLimpiar_Click(sender, e);
        }

        // ----------------------------------------------------------------
        // Limpiar formulario
        // ----------------------------------------------------------------
        private void BancosBtnLimpiar_Click(object sender, EventArgs e)
        {
            BancDgvMovimientos.DataSource     = null;
            BancDgvPartidaContable.DataSource = null;
            BancLblTotalDebe.Text             = "Total Debe";
            BancLblTotalHaber.Text            = "Total Haber";
            BancLblDiferencia.Text            = "Diferencia";
            dateTimePicker1.Value             = DateTime.Today;
            dateTimePicker2.Value             = DateTime.Today;
            BancCbxPeriodo.SelectedIndex      = -1;
        }

        // ----------------------------------------------------------------
        // Cerrar formulario
        // ----------------------------------------------------------------
        private void BancBtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ----------------------------------------------------------------
        // Ajustar fechas automáticamente según el tipo de período
        // ----------------------------------------------------------------
        private void BancCbxPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BancCbxPeriodo.SelectedIndex < 0) return;

            DateTime Hoy = DateTime.Today;

            if (BancCbxPeriodo.SelectedItem.ToString() == "Mensual")
            {
                dateTimePicker1.Value = new DateTime(Hoy.Year, Hoy.Month, 1);
                dateTimePicker2.Value = new DateTime(Hoy.Year, Hoy.Month,
                    DateTime.DaysInMonth(Hoy.Year, Hoy.Month));
            }
            else if (BancCbxPeriodo.SelectedItem.ToString() == "Anual")
            {
                dateTimePicker1.Value = new DateTime(Hoy.Year, 1, 1);
                dateTimePicker2.Value = new DateTime(Hoy.Year, 12, 31);
            }
        }

        // ----------------------------------------------------------------
        // Actualizar etiquetas de totales
        // ----------------------------------------------------------------
        private void BancosActualizarTotales(DataTable Detalle)
        {
            decimal TotalDebe  = 0;
            decimal TotalHaber = 0;

            foreach (DataRow Fila in Detalle.Rows)
            {
                TotalDebe  += Convert.ToDecimal(Fila["DebeDetallePartida"]);
                TotalHaber += Convert.ToDecimal(Fila["HaberDetallePartida"]);
            }

            decimal Diferencia = TotalDebe - TotalHaber;

            BancLblTotalDebe.Text  = $"Total Debe: {TotalDebe:N2}";
            BancLblTotalHaber.Text = $"Total Haber: {TotalHaber:N2}";
            BancLblDiferencia.Text = $"Diferencia: {Diferencia:N2}";
        }

        // ----------------------------------------------------------------
        // Stub existente — sin funcionalidad adicional
        // ----------------------------------------------------------------
        private void BancDgvPartidaContable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
