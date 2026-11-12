using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_CierreMovimientos;
using CapaControlador_Herramientas;
using CapaVista_Componentes.Componentes;
using Microsoft.Reporting.WinForms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 24/10/2026 (prototipo) - 04/11/2026 (datos reales y cierre) - 11/11/2026 (reporte y ayuda)
 * ==================================================================
 * Propósito :
 *  Presenta el período, las validaciones y los saldos del cierre de movimientos
 *  y coordina las acciones de la pantalla: validar el período con datos reales,
 *  imprimir la vista previa (saldos, antigüedad y validaciones), abrir la ayuda
 *  y, si no hay validaciones bloqueantes, ejecutar el cierre.
 *  Si en Herramientas se eligió otra moneda de visualización, las etiquetas de
 *  los totales muestran también su equivalente con el tipo de cambio de Bancos.
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
        private readonly ClsControladorMoneda _ControladorMoneda = new ClsControladorMoneda();
        private readonly Dictionary<ComprasUsrLabel, string> _EtiquetasTotales = new Dictionary<ComprasUsrLabel, string>();
        private ClsResumenCierre _Resumen;

        public FrmCierreMovimientos()
        {
            InitializeComponent();
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasProcConfigurarTablas();
            _EtiquetasTotales.Add(ComprasLblEtiquetaTotalCargos, ComprasLblEtiquetaTotalCargos.Texto);
            _EtiquetasTotales.Add(ComprasLblEtiquetaTotalAbonos, ComprasLblEtiquetaTotalAbonos.Texto);
            _EtiquetasTotales.Add(ComprasLblEtiquetaSaldoFinal, ComprasLblEtiquetaSaldoFinal.Texto);

            Load += ComprasMetCargar;
            ComprasBtnValidar.Click += ComprasMetValidar;
            ComprasBtnEjecutarCierre.Click += ComprasMetEjecutarCierre;
            ComprasBtnImprimir.Click += ComprasMetImprimir;
            ComprasBtnAyuda.Click += ComprasMetAyuda;
            ComprasUsrEncabezadoFormulario.PrepararReporte += ComprasMetPrepararReporte;
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

            _Resumen = Resumen;
            ComprasDgvValidaciones.DataSource = Resumen.Validaciones;
            ComprasDgvSaldos.DataSource = Resumen.SaldosProveedor;
            ComprasProcMostrarTotales(Resumen.TotalCargos, Resumen.TotalAbonos, Resumen.SaldoFinal, Resumen.FechaCorte);
            ComprasBtnEjecutarCierre.Enabled = Resumen.PuedeCerrar;
            ComprasBtnImprimir.Enabled = true;

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

        private void ComprasMetImprimir(object Remitente, EventArgs Evento)
        {
            ComprasProcAbrirDesdeEncabezado(ComprasUsrEncabezadoFormulario.AbrirReporte);
        }

        private void ComprasMetAyuda(object Remitente, EventArgs Evento)
        {
            ComprasProcAbrirDesdeEncabezado(ComprasUsrEncabezadoFormulario.AbrirAyuda);
        }

        // El reporte y la ayuda los abre el encabezado de Componentes; aquí solo se muestran sus errores.
        private void ComprasProcAbrirDesdeEncabezado(Action Abrir)
        {
            try
            {
                Abrir();
            }
            catch (Exception Error)
            {
                ComprasProcMostrarError(Error.Message);
            }
        }

        private void ComprasMetPrepararReporte(object Remitente, ComprasReporteEventArgs Evento)
        {
            if (_Resumen == null)
                throw new InvalidOperationException("Valide el período antes de generar el reporte del cierre.");

            Evento.Reporte.DataSources.Clear();
            Evento.Reporte.DataSources.Add(new ReportDataSource("DsSaldos", _Resumen.SaldosProveedor));
            Evento.Reporte.DataSources.Add(new ReportDataSource("DsAntiguedad", _Resumen.Antiguedad));
            Evento.Reporte.DataSources.Add(new ReportDataSource("DsValidaciones", _Resumen.Validaciones));
            Evento.Reporte.SetParameters(new[]
            {
                new ReportParameter("Periodo", _Resumen.Periodo),
                new ReportParameter("FechaCorte", _Resumen.FechaCorte.ToString("dd/MM/yyyy")),
                new ReportParameter("Estado", _Resumen.PuedeCerrar ? "Listo para cerrar" : "Con validaciones bloqueantes")
            });
        }

        private void ComprasProcMostrarTotales(decimal TotalCargos, decimal TotalAbonos, decimal SaldoFinal, DateTime? FechaCorte)
        {
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalCargos, TotalCargos);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalAbonos, TotalAbonos);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSaldoFinal, SaldoFinal);

            ClsMonedaCompras MonedaLocal = FechaCorte.HasValue ? ComprasFuncObtenerMonedaLocal() : null;
            bool MostrarEquivalente = ClsPreferenciaMonedaCompras.ComprasFuncUsaOtraMoneda(MonedaLocal);
            var Montos = new Dictionary<ComprasUsrLabel, decimal>
            {
                { ComprasLblEtiquetaTotalCargos, TotalCargos },
                { ComprasLblEtiquetaTotalAbonos, TotalAbonos },
                { ComprasLblEtiquetaSaldoFinal, SaldoFinal }
            };
            foreach (KeyValuePair<ComprasUsrLabel, decimal> Total in Montos)
            {
                string Texto = _EtiquetasTotales[Total.Key];
                if (MostrarEquivalente)
                    Texto += "  ≈ " + ClsPreferenciaMonedaCompras.ComprasFuncFormatearDesdeLocal(
                        Total.Value, FechaCorte.Value, _ControladorMoneda, MonedaLocal);
                Total.Key.Texto = Texto;
            }
        }

        // Si Bancos no responde, los totales se quedan solo en quetzales.
        private ClsMonedaCompras ComprasFuncObtenerMonedaLocal()
        {
            try
            {
                return _ControladorMoneda.ComprasFuncObtenerMonedaLocal(_ControladorMoneda.ComprasFuncObtenerMonedas());
            }
            catch (InvalidOperationException)
            {
                return null;
            }
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
            _Resumen = null;
            ComprasDgvValidaciones.DataSource = null;
            ComprasDgvSaldos.DataSource = null;
            ComprasProcMostrarTotales(0, 0, 0, null);
            ComprasBtnEjecutarCierre.Enabled = false;
            ComprasBtnImprimir.Enabled = false;
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
