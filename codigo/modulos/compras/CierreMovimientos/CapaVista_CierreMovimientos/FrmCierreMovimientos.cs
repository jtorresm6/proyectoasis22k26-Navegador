using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_CierreMovimientos;
using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 24/10/2026 (prototipo) - 04/11/2026 (datos reales y cierre)
 * ==================================================================
 * Propósito :
 *  Presenta el período, las validaciones y los saldos del cierre de movimientos
 *  y coordina las acciones de la pantalla: validar el período con datos reales
 *  y, si no hay validaciones bloqueantes, ejecutar el cierre.
 * ===================================================================
*/

namespace CapaVista_CierreMovimientos
{
    public partial class FrmCierreMovimientos : Form
    {
        private const string _Titulo = "Cierre de Movimientos";
        private static readonly Color _ColorBloqueante = Color.FromArgb(176, 42, 42);
        private static readonly Color _ColorCorrecta = Color.FromArgb(34, 120, 70);

        private readonly ClsCierreMovimientos _Controlador = new ClsCierreMovimientos();

        public FrmCierreMovimientos()
        {
            InitializeComponent();
            ComprasBtnImprimir.Enabled = false;
            ComprasBtnAyuda.Enabled = false;
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasProcConfigurarTablas();

            Load += ComprasMetCargar;
            ComprasBtnValidar.Click += ComprasMetValidar;
            ComprasBtnEjecutarCierre.Click += ComprasMetEjecutarCierre;
            ComprasBtnSalir.Click += ComprasMetSalir;
            ComprasCboPeriodo.SelectedIndexChanged += ComprasMetPeriodoCambiado;
            ComprasDgvValidaciones.Tabla.CellFormatting += ComprasMetFormatearValidacion;
        }

        private void ComprasProcConfigurarTablas()
        {
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvValidaciones, "cmp_estado", "Estado", 18, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvValidaciones, "cmp_validacion", "Validación", 57, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvValidaciones, "cmp_origen", "Origen", 25, false);

            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_proveedor", "Proveedor", 32, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_saldo_inicial", "Saldo inicial", 17, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_total_cargos", "Cargos", 17, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_total_abonos", "Abonos", 17, true);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvSaldos, "cmp_saldo_final", "Saldo final", 17, true);
        }

        private void ComprasMetCargar(object Remitente, EventArgs Evento)
        {
            ComprasProcCargarPeriodos();
        }

        private void ComprasProcCargarPeriodos()
        {
            ComprasProcLimpiarResumen();
            DataTable Periodos;
            try
            {
                Periodos = _Controlador.ComprasFuncObtenerPeriodosCerrables();
            }
            catch (InvalidOperationException Error)
            {
                ComprasProcMostrarError(Error.Message);
                return;
            }

            ComprasCboPeriodo.DisplayMember = "cmp_descripcion";
            ComprasCboPeriodo.ValueMember = "cmp_id_periodo";
            ComprasCboPeriodo.DataSource = Periodos;
            ComprasBtnValidar.Enabled = Periodos.Rows.Count > 0;

            if (Periodos.Rows.Count == 0)
                MessageBox.Show("No hay períodos abiertos. Registre el primer período de Compras en tblPeriodoCompras.",
                    _Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComprasMetPeriodoCambiado(object Remitente, EventArgs Evento)
        {
            // Si cambia el período, la vista previa anterior ya no aplica.
            ComprasProcLimpiarResumen();
        }

        private void ComprasMetValidar(object Remitente, EventArgs Evento)
        {
            if (!ComprasFuncObtenerPeriodo(out int IdPeriodo)) return;

            ClsResumenCierre Resumen;
            try
            {
                Cursor = Cursors.WaitCursor;
                Resumen = _Controlador.ComprasFuncPrepararCierre(IdPeriodo);
            }
            catch (InvalidOperationException Error)
            {
                ComprasProcMostrarError(Error.Message);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            ComprasDgvValidaciones.DataSource = Resumen.Validaciones;
            ComprasDgvSaldos.DataSource = Resumen.SaldosProveedor;
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalCargos, Resumen.TotalCargos);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalAbonos, Resumen.TotalAbonos);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSaldoFinal, Resumen.SaldoFinal);
            ComprasBtnEjecutarCierre.Enabled = Resumen.PuedeCerrar;

            if (!Resumen.PuedeCerrar)
                MessageBox.Show(Resumen.Periodo + " tiene validaciones bloqueantes. Corríjalas y vuelva a validar.",
                    _Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ComprasMetEjecutarCierre(object Remitente, EventArgs Evento)
        {
            if (!ComprasFuncObtenerPeriodo(out int IdPeriodo)) return;

            DialogResult Respuesta = MessageBox.Show(
                "¿Desea cerrar " + ComprasCboPeriodo.Text + "?" + Environment.NewLine + Environment.NewLine +
                "Se guardarán los saldos y la antigüedad de saldos, y los documentos del período quedarán bloqueados. " +
                "Esta acción no se puede deshacer.",
                _Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (Respuesta != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                string Mensaje = _Controlador.ComprasFuncEjecutarCierre(IdPeriodo);
                Cursor = Cursors.Default;
                MessageBox.Show(Mensaje, _Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException Error)
            {
                Cursor = Cursors.Default;
                ComprasProcMostrarError(Error.Message);
                return;
            }

            ComprasProcCargarPeriodos();
        }

        private void ComprasMetFormatearValidacion(object Remitente, DataGridViewCellFormattingEventArgs Evento)
        {
            DataGridView Tabla = ComprasDgvValidaciones.Tabla;
            if (Evento.RowIndex < 0 || Tabla.Columns[Evento.ColumnIndex].DataPropertyName != "cmp_estado") return;

            string Estado = Convert.ToString(Evento.Value);
            Evento.CellStyle.Font = new Font(Tabla.Font, FontStyle.Bold);
            Evento.CellStyle.ForeColor = Estado == "Bloqueante" ? _ColorBloqueante
                : Estado == "Advertencia" ? ClsTemaCompras.FondoEtiquetasDestacadas
                : _ColorCorrecta;
        }

        private bool ComprasFuncObtenerPeriodo(out int IdPeriodo)
        {
            IdPeriodo = 0;
            if (ComprasCboPeriodo.SelectedValue == null || !int.TryParse(Convert.ToString(ComprasCboPeriodo.SelectedValue), out IdPeriodo))
            {
                MessageBox.Show("Seleccione el período a cerrar.", _Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ComprasProcLimpiarResumen()
        {
            ComprasDgvValidaciones.DataSource = null;
            ComprasDgvSaldos.DataSource = null;
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalCargos, 0);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalAbonos, 0);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSaldoFinal, 0);
            ComprasBtnEjecutarCierre.Enabled = false;
        }

        private static void ComprasProcMostrarError(string Mensaje)
        {
            MessageBox.Show(Mensaje, _Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ComprasMetSalir(object Remitente, EventArgs Evento)
        {
            Close();
        }
    }
}
