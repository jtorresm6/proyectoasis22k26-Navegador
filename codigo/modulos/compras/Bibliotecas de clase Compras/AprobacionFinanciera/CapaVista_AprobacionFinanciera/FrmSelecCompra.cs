using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_AprobacionFinanciera;
/*
 * ==================================================================
 * Área : Compras
 * Autor : Pedro José Gómez Villalobos
 * Carné : 0901-23-4868
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta la consulta de órdenes para su revisión en el proceso de aprobación financiera.
 * ===================================================================
*/

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmAprobacionFinanciera : System.Windows.Forms.Form
    {
        // 1. Instancia del controlador
        private ClsAprobacionFinanciera _Controlador = new ClsAprobacionFinanciera();

        // 2. Variables para control de método de valuación SAT
        private string _MetodoValuacionAnterior = "Promedio";
        private bool _IgnorarCambioValuacion = false;

        private bool _AbriendoVentana = false;

        public FrmAprobacionFinanciera()
        {
            InitializeComponent();

            // Configuraciones visuales originales
            ComprasBtnEvaluar.Enabled = false;
            ComprasBtnRegresar.Click += (Remitente, Evento) => Close();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);

            // 3. Inicializar el ComboBox de Valuación de Inventario (PEPS, UEPS, Promedio)
            ComprasCboValuacion.Items.Clear();
            ComprasCboValuacion.Items.AddRange(new object[] { "Promedio", "PEPS", "UEPS" });
            ComprasCboValuacion.SelectedIndex = 0; // Promedio por defecto
            ComprasCboValuacion.SelectedIndexChanged += ComprasCboValuacion_SelectedIndexChanged;
        }

        private void FrmAprobacionFinanciera_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Pedimos los datos al controlador
                DataTable Datos = _Controlador.ComprasFuncListarPendientes();

                // Forzamos a que el grid dibuje las columnas automáticamente
                ComprasDgvOrdenesDisponibles.AutoGenerateColumns = true;

                // Llenamos el DataGridView
                ComprasDgvOrdenesDisponibles.DataSource = Datos;

                // Formateamos la columna
                if (ComprasDgvOrdenesDisponibles.Columns.Contains("Total Esperado"))
                    ComprasDgvOrdenesDisponibles.Columns["Total Esperado"].DefaultCellStyle.Format = "N2";

                // Habilitamos el botón si hay datos
                if (Datos.Rows.Count > 0)
                {
                    ComprasBtnEvaluar.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Ocurrió un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComprasCboValuacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_IgnorarCambioValuacion || ComprasCboValuacion.SelectedItem == null) return;

            string nuevoMetodo = ComprasCboValuacion.SelectedItem.ToString();

            if (nuevoMetodo != _MetodoValuacionAnterior)
            {
                DialogResult respuesta = MessageBox.Show(
                    "El cambio de método de valuación (PEPS, UEPS, Promedio) tiene un impacto directo en la estructura de costos y requiere notificación fiscal.\n\n¿Cuenta con la resolución de autorización emitida por la SAT para realizar este cambio?",
                    "Verificación Legal SAT", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.No)
                {
                    // Si no tiene permiso, la interfaz retrocede al valor anterior silenciosamente
                    _IgnorarCambioValuacion = true;
                    ComprasCboValuacion.SelectedItem = _MetodoValuacionAnterior;
                    _IgnorarCambioValuacion = false;
                }
                else
                {
                    // Si acepta, el sistema adopta el nuevo método
                    _MetodoValuacionAnterior = nuevoMetodo;
                }
            }
        }

        private void ComprasBtnEvaluar_Click(object sender, EventArgs e)
        {

            if (ComprasDgvOrdenesDisponibles.SelectedRows.Count == 0 || _AbriendoVentana)
            {
                return;
            }
            if (ComprasDgvOrdenesDisponibles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione una orden de la tabla para evaluarla.", "Confirmar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _AbriendoVentana = true;
            ComprasBtnEvaluar.Enabled = false;
            try
            {
                int IdOrden = Convert.ToInt32(ComprasDgvOrdenesDisponibles.SelectedRows[0].Cells["ID Orden"].Value);
                decimal MontoEsperado = Convert.ToDecimal(ComprasDgvOrdenesDisponibles.SelectedRows[0].Cells["Total Esperado"].Value);
                string metodoValuacion = ComprasCboValuacion.SelectedItem.ToString();

                FrmCompra frmEvaluacion = new FrmCompra(IdOrden, MontoEsperado, metodoValuacion);
                frmEvaluacion.ShowDialog();

                // Refrescar tabla
                DataTable DatosActualizados = _Controlador.ComprasFuncListarPendientes();
                ComprasDgvOrdenesDisponibles.DataSource = DatosActualizados;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ocurrió un error al intentar abrir la evaluación: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Pase lo que pase (se guarde, se cierre o dé error), abrimos el candado al final
                ComprasBtnEvaluar.Enabled = true;

                // Forzamos a Windows a procesar y desechar cualquier clic atascado antes de liberar la variable
                Application.DoEvents();
                _AbriendoVentana = false;
            }
        }
    }
}