using CapaControlador_CierreNominas;
using CapaControlador_CierreNominas.Objetos_de_valor;
using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Nomina.Reportes;
using CapaVista_Seguridad.Ayudas;
using CapaVista_Seguridad.frmReportes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área: Recursos Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 01/11/2026
 * ==================================================================
 * Propósito : Ventana que cierra la nómina de un mes: muestra los
 * meses abiertos, el resumen del período con la verificación
 * de cuadre y el detalle por empleado, y ejecuta el cierre
 * (póliza a Contabilidad) con seguridad por botón.
 * ===================================================================
 */

namespace CapaVista_CierreNominas.Formularios
{
    public partial class FrmCierreNominas : Form
    {
        private ClsModeloCierreNomina _CierreNomina = new ClsModeloCierreNomina();

        private ClsPermisoAplicacion _MisPermisos;
        private ClsResumenCierreNominaValor _ResumenActual;
        private bool _CargandoPeriodos;
        private readonly ToolTip _Ayuda = new ToolTip();
        private const int ID_MODULO = 9000;
        private const int ID_APLICACION = 9601;

        public FrmCierreNominas()
        {
            InitializeComponent();
            Load += FrmCierreNominas_Load;
            RrhhCboPeriodo.SelectedIndexChanged += RrhhCboPeriodo_SelectedIndexChanged;
            RrhhBtnCerrarNomina.Click += RrhhBtnCerrarNomina_Click;
        }

        private void FrmCierreNominas_Load(object sender, EventArgs e)
        {
            var MapaBotones = new Dictionary<Control, TipoPermiso>
            {
                { RrhhBtnCerrarNomina, TipoPermiso.Insertar }
            };

            _MisPermisos = ClsSeguridadFormHelper.SeguridadMetInicializarSeguridad(
                this, ID_MODULO, ID_APLICACION, MapaBotones);

            if (!_MisPermisos.TieneAcceso)
                return;

            RrhhCboPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhMetConfigurarColumnas();
            RrhhMetLimpiarResumen();
            RrhhMetCargarPeriodos();
        }

        private void RrhhMetConfigurarColumnas()
        {
            RrhhDgvVistaCierreNomina.AutoGenerateColumns = false;
            RrhhDgvVistaCierreNomina.AllowUserToAddRows = false;
            RrhhDgvVistaCierreNomina.AllowUserToDeleteRows = false;
            RrhhDgvVistaCierreNomina.ReadOnly = true;
            RrhhDgvVistaCierreNomina.Columns.Clear();

            RrhhDgvVistaCierreNomina.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Codigo",
                HeaderText = "Código",
                Name = "Codigo"
            });
            RrhhDgvVistaCierreNomina.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Empleado",
                HeaderText = "Empleado",
                Name = "Empleado"
            });
            RrhhDgvVistaCierreNomina.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Percepciones",
                HeaderText = "Percepciones",
                Name = "Percepciones",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            RrhhDgvVistaCierreNomina.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Deducciones",
                HeaderText = "Deducciones",
                Name = "Deducciones",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            RrhhDgvVistaCierreNomina.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Verificacion",
                HeaderText = "Verificación",
                Name = "Verificacion"
            });
        }

        // Llena el combo con los meses abiertos y lo deja sin selección
        private List<ClsPeriodoNominaValor> RrhhMetCargarPeriodos()
        {
            List<ClsPeriodoNominaValor> Periodos = new List<ClsPeriodoNominaValor>();
            try
            {
                _CargandoPeriodos = true;
                Periodos = _CierreNomina.RrhhMetObtenerPeriodosAbiertos();
                RrhhCboPeriodo.DataSource = null;
                RrhhCboPeriodo.DisplayMember = "Texto";
                RrhhCboPeriodo.ValueMember = "PeriodoNomina";
                RrhhCboPeriodo.DataSource = Periodos;
                RrhhCboPeriodo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _CargandoPeriodos = false;
            }
            return Periodos;
        }

        private void RrhhCboPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_CargandoPeriodos)
                return;

            var Periodo = RrhhCboPeriodo.SelectedItem as ClsPeriodoNominaValor;
            if (Periodo == null)
            {
                RrhhMetLimpiarResumen();
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                _ResumenActual = _CierreNomina.RrhhMetObtenerPrevio(Periodo.FechaInicioNomina, Periodo.FechaFinalNomina);
                RrhhMetMostrarResumen(_ResumenActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                RrhhMetLimpiarResumen();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void RrhhMetMostrarResumen(ClsResumenCierreNominaValor Resumen)
        {
            RrhhLblValorResumen.Text = Resumen.EmpleadosEnNomina.ToString();
            RrhhLblValorResumen2.Text = "Q " + Resumen.TotalPercepciones.ToString("N2");
            RrhhLblValorResumen3.Text = "Q " + Resumen.TotalDeducciones.ToString("N2");
            RrhhLblValorResumen4.Text = "Q " + Resumen.SalarioLiquido.ToString("N2");

            RrhhLblVerificacionCuadre.Text = "Verificación de cuadre: " + Resumen.TextoCuadre;
            RrhhLblVerificacionCuadre.ForeColor = Resumen.CuadreCorrecto
                ? Color.FromArgb(14, 76, 85)
                : Color.Firebrick;

            // El motivo del bloqueo se ve al pasar el mouse por la verificación
            string Motivo = Resumen.PuedeCerrar ? string.Empty : Resumen.MensajeBloqueo;
            _Ayuda.SetToolTip(RrhhLblVerificacionCuadre, Motivo);
            _Ayuda.SetToolTip(RrhhPnlDecorativo5, Motivo);

            RrhhDgvVistaCierreNomina.DataSource = null;
            RrhhDgvVistaCierreNomina.DataSource = Resumen.Filas;
            foreach (DataGridViewRow Fila in RrhhDgvVistaCierreNomina.Rows)
            {
                var Item = Fila.DataBoundItem as ClsFilaCierreNominaValor;
                if (Item != null && !Item.Verificado)
                    Fila.DefaultCellStyle.ForeColor = Color.Firebrick;
            }

            RrhhMetActualizarBoton();
        }

        private void RrhhMetLimpiarResumen()
        {
            _ResumenActual = null;
            RrhhLblValorResumen.Text = "0";
            RrhhLblValorResumen2.Text = "0";
            RrhhLblValorResumen3.Text = "0";
            RrhhLblValorResumen4.Text = "0";
            RrhhLblVerificacionCuadre.Text = "Verificación de cuadre";
            RrhhLblVerificacionCuadre.ForeColor = Color.FromArgb(14, 76, 85);
            _Ayuda.SetToolTip(RrhhLblVerificacionCuadre, string.Empty);
            _Ayuda.SetToolTip(RrhhPnlDecorativo5, string.Empty);
            RrhhDgvVistaCierreNomina.DataSource = null;
            RrhhMetActualizarBoton();
        }

        // El botón solo se habilita si el rol puede insertar y el período está listo para cerrar
        private void RrhhMetActualizarBoton()
        {
            RrhhBtnCerrarNomina.Enabled = _MisPermisos != null
                && _MisPermisos.PuedeInsertar
                && _ResumenActual != null
                && _ResumenActual.PuedeCerrar;
        }

        private void RrhhBtnCerrarNomina_Click(object sender, EventArgs e)
        {
            var Periodo = RrhhCboPeriodo.SelectedItem as ClsPeriodoNominaValor;
            if (Periodo == null || _ResumenActual == null || !_ResumenActual.PuedeCerrar)
            {
                MessageBox.Show("Seleccione un mes con nómina lista para cerrar.");
                return;
            }

            string Pregunta = "Va a cerrar la nómina de " + Periodo.Texto + "." + Environment.NewLine + Environment.NewLine +
                "Empleados: " + _ResumenActual.EmpleadosEnNomina + Environment.NewLine +
                "Salario líquido: Q " + _ResumenActual.SalarioLiquido.ToString("N2") + Environment.NewLine + Environment.NewLine +
                "Se enviará la póliza a Contabilidad. Este proceso es irreversible. ¿Desea continuar?";

            if (MessageBox.Show(Pregunta, "Cerrar nómina", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                RrhhBtnCerrarNomina.Enabled = false;

                _CierreNomina.PeriodoCierreNomina = Periodo.PeriodoNomina;
                _CierreNomina.FechaInicioCierreNomina = Periodo.FechaInicioNomina;
                _CierreNomina.FechaFinalCierreNomina = Periodo.FechaFinalNomina;

                bool Valido = new ClsValidacionDatos(_CierreNomina).SeguridadMetValidar();
                if (Valido)
                {
                    string Resultado = _CierreNomina.RrhhMetCerrarNomina();

                    // Si el mes ya no está entre los abiertos, el cierre se realizó
                    List<ClsPeriodoNominaValor> Abiertos = RrhhMetCargarPeriodos();
                    int Posicion = Abiertos.FindIndex(p => p.PeriodoNomina == Periodo.PeriodoNomina);
                    bool Cerrada = Posicion < 0;

                    if (Cerrada)
                        RrhhMetLimpiarResumen();
                    else
                        RrhhCboPeriodo.SelectedIndex = Posicion;

                    MessageBox.Show(Resultado, "Cierre de nómina", MessageBoxButtons.OK,
                        Cerrada ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                RrhhMetActualizarBoton();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RrhhMetAjustarAPantalla();
        }

        private void RrhhMetAjustarAPantalla()
        {
            Rectangle Area = Screen.FromControl(this).WorkingArea;
            double Factor = Math.Min(1d, Math.Min((double)Area.Width / Width, (double)Area.Height / Height));
            if (Factor >= 1d)
                return;

            List<KeyValuePair<Control, Font>> Fuentes = new List<KeyValuePair<Control, Font>>();
            RrhhMetGuardarFuentes(this, Fuentes);

            AutoScaleMode = AutoScaleMode.None;
            Scale(new SizeF((float)Factor, (float)Factor));
            foreach (KeyValuePair<Control, Font> Par in Fuentes)
                Par.Key.Font = RrhhMetEscalarFuente(Par.Value, Factor);

            foreach (DataGridView Tabla in Fuentes.Select(Par => Par.Key).OfType<DataGridView>())
            {
                Tabla.ColumnHeadersDefaultCellStyle.Font = RrhhMetEscalarFuente(Tabla.ColumnHeadersDefaultCellStyle.Font, Factor);
                Tabla.DefaultCellStyle.Font = RrhhMetEscalarFuente(Tabla.DefaultCellStyle.Font, Factor);
            }

            if (Owner != null)
                CenterToParent();
            else
                CenterToScreen();
        }

        private void RrhhMetGuardarFuentes(Control Padre, List<KeyValuePair<Control, Font>> Fuentes)
        {
            Fuentes.Add(new KeyValuePair<Control, Font>(Padre, Padre.Font));
            foreach (Control Hijo in Padre.Controls)
                RrhhMetGuardarFuentes(Hijo, Fuentes);
        }

        private Font RrhhMetEscalarFuente(Font Original, double Factor)
        {
            if (Original == null)
                return null;
            return new Font(Original.FontFamily, (float)(Original.Size * Factor), Original.Style, Original.Unit);
        }

        private void RrhhBtnReporteCierreNomina_Click(object sender, EventArgs e)
        {
            FrmReporteCierreNominas reporte = new FrmReporteCierreNominas();
            reporte.Show();
        }

        private void RrhhBtnAyudaCierreNomina_Click(object sender, EventArgs e)
        {
            RrhhMetAbrirAyuda(this, "CierreNominas__RecursosHumanos.html");

        }

        internal static void RrhhMetAbrirAyuda(Control Ventana, string Pagina)
        {
            const string Ayuda = @"ayuda\modulos\rrhh\RrhhAyudas\RecursosHumanosAyudas.chm";
            for (var Carpeta = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); Carpeta != null; Carpeta = Carpeta.Parent)
            {
                string Ruta = Path.Combine(Carpeta.FullName, Ayuda);
                if (File.Exists(Ruta))
                {
                    Help.ShowHelp(Ventana, Ruta, Pagina);
                    return;
                }
            }
            MessageBox.Show("No se encontró el archivo de ayuda de RRHH.", "Ayuda",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}