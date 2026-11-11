using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_AprobacionFinanciera;
using CapaVista_Componentes.Componentes;

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmSelecDevoluciones : Form
    {
        private readonly ClsControladorDevoluciones _Controlador;
        private DataTable _Datos;
        private bool _Cargando;
        private bool _CargaInicial = true;
        private bool _RestableciendoFiltros;

        public FrmSelecDevoluciones()
        {
            _Controlador = new ClsControladorDevoluciones();
            InitializeComponent();

            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);

            ComprasCboEstado.Items.AddRange(new object[] { "Todos", "Registrada", "Anulada" });
            ComprasProcRestablecerFiltros();

            ComprasBtnNuevaDevolucion.Enabled = false;

            ComprasTxtBuscar.TextChanged += (Remitente, Evento) => ComprasProcAplicarFiltros();
            ComprasCboEstado.SelectedIndexChanged += (Remitente, Evento) =>
            {
                if (!_RestableciendoFiltros) ComprasProcAplicarFiltros();
            };

            ComprasUsrFechas.RangoCambiado += (Remitente, Evento) => ComprasProcAplicarFiltros();
            ComprasChkFechas.CheckedChanged += (Remitente, Evento) =>
            {
                ComprasUsrFechas.Enabled = ComprasChkFechas.Checked;
                ComprasProcAplicarFiltros();
            };

            ComprasBtnActualizar.Click += async (Remitente, Evento) => await ComprasProcRefrescarAsync();
            ComprasBtnCerrar.Click += (Remitente, Evento) => Close();

            ComprasDgvEncabezados.SelectionChanged += ComprasDgvEncabezados_SelectionChanged;
            ComprasBtnNuevaDevolucion.Click += ComprasProcProcesarDevolucion;
            ComprasDgvEncabezados.CellDoubleClick += ComprasProcVerCompra;
        }

        protected override async void OnLoad(EventArgs Evento)
        {
            base.OnLoad(Evento);
            await ComprasProcCargarDatosAsync();
        }

        private void ComprasProcActualizarDisponibilidad()
        {
            bool Disponible = !_Cargando;
            ComprasBtnActualizar.Enabled = Disponible;
            ComprasGbxFiltros.Enabled = Disponible;
            ComprasDgvEncabezados.Enabled = Disponible;
            ComprasBtnCerrar.Enabled = true;
            UseWaitCursor = !Disponible;

            ComprasBtnNuevaDevolucion.Enabled = Disponible && ComprasDgvEncabezados.SelectedRows.Count > 0;
        }

        private async Task ComprasProcCargarDatosAsync()
        {
            if (_Cargando) return;
            _Cargando = true;
            ComprasProcActualizarDisponibilidad();
            ComprasLblResultado.ColorTexto = ClsTemaCompras.PrincipalNavegador;
            ComprasLblResultado.Texto = "Cargando compras registradas...";

            try
            {
                DataTable Datos = await Task.Run(() => _Controlador.ComprasFuncListarCompras());

                if (IsDisposed || Disposing)
                {
                    Datos?.Dispose();
                    return;
                }

                DataTable Anteriores = _Datos;
                _Datos = Datos;
                Anteriores?.Dispose();

                ComprasProcAplicarFiltros();
            }
            catch (Exception Ex)
            {
                if (!IsDisposed && !Disposing)
                {
                    ComprasProcVaciarDatos();
                    ComprasProcMostrarError("No se pudieron cargar las compras.", Ex);
                }
            }
            finally
            {
                _Cargando = false;
                if (!IsDisposed && !Disposing)
                {
                    ComprasProcActualizarDisponibilidad();
                    if (_CargaInicial)
                    {
                        _CargaInicial = false;
                        ComprasPnlDesplazamiento.AutoScrollPosition = Point.Empty;
                    }
                }
            }
        }

        private void ComprasProcAplicarFiltros()
        {
            if (_Datos == null || _RestableciendoFiltros) return;

            try
            {
                string FiltroSQL = "1=1";

                if (ComprasCboEstado.Text != "Todos" && !string.IsNullOrEmpty(ComprasCboEstado.Text))
                {
                    FiltroSQL += $" AND estadoCompra = '{ComprasCboEstado.Text}'";
                }

                if (ComprasChkFechas.Checked)
                {
                    string FechaInicio = ComprasUsrFechas.FechaInicio.ToString("yyyy-MM-dd");
                    string FechaFin = ComprasUsrFechas.FechaFin.ToString("yyyy-MM-dd");
                    FiltroSQL += $" AND fechaCompra >= '{FechaInicio}' AND fechaCompra <= '{FechaFin}'";
                }

                if (!string.IsNullOrWhiteSpace(ComprasTxtBuscar.Text))
                {
                    string Busqueda = ComprasTxtBuscar.Text.Trim().Replace("'", "''");
                    FiltroSQL += $" AND (serieFacturaCompra LIKE '%{Busqueda}%' OR numeroFacturaCompra LIKE '%{Busqueda}%')";
                }

                _Datos.DefaultView.RowFilter = FiltroSQL;
                ComprasDgvEncabezados.DataSource = _Datos;

                if (!string.IsNullOrWhiteSpace(ComprasTxtBuscar.Text) && _Datos.DefaultView.Count == 0)
                    ComprasTxtBuscar.ComprasMetMostrarError("No se encontraron coincidencias.");
                else
                    ComprasTxtBuscar.ComprasMetLimpiarError();

                ComprasLblResultado.ColorTexto = ClsTemaCompras.PrincipalNavegador;
                ComprasLblResultado.Texto = $"{_Datos.DefaultView.Count} compras encontradas. Seleccione una y presione 'Nueva devolución'.";
            }
            catch (Exception Ex)
            {
                ComprasProcMostrarError("Error al filtrar los datos.", Ex);
            }
        }

        private void ComprasDgvEncabezados_SelectionChanged(object sender, EventArgs e)
        {
            ComprasBtnNuevaDevolucion.Enabled = ComprasDgvEncabezados.SelectedRows.Count > 0 && !_Cargando;
        }

        private void ComprasProcProcesarDevolucion(object sender, EventArgs e)
        {
            if (ComprasDgvEncabezados.SelectedRows.Count == 0 || _Cargando) return;

            try
            {
                DataRowView FilaVisual = (DataRowView)ComprasDgvEncabezados.SelectedRows[0].DataBoundItem;
                int IdCompraSeleccionada = Convert.ToInt32(FilaVisual["idCompra"]);

                string estadoCompra = FilaVisual["estadoCompra"].ToString();
                if (estadoCompra.Equals("Anulada", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Restricción del sistema: No se puede generar una nota de devolución sobre un documento de compra que se encuentra anulado.", "Operación Denegada", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                FrmCrearDevolucion frmDevolucion = new FrmCrearDevolucion(IdCompraSeleccionada);
                frmDevolucion.ShowDialog();

                //Opcional: Refrescar la tabla al volver
                 _ = ComprasProcRefrescarAsync();
            }
            catch (Exception Ex)
            {
                ComprasProcMostrarError("Error al iniciar la devolución.", Ex);
            }
        }

        private void ComprasProcVerCompra(object sender, DataGridViewCellEventArgs e)
        {
            if (_Cargando || e.RowIndex < 0 || ComprasDgvEncabezados.SelectedRows.Count == 0) return;

            try
            {
                DataRowView FilaVisual = (DataRowView)ComprasDgvEncabezados.SelectedRows[0].DataBoundItem;
                int IdCompraSeleccionada = Convert.ToInt32(FilaVisual["idCompra"]);

                // LLAMADA AL VISOR (Usando el constructor limpio que acabas de configurar)
                FrmVerCompra frmVisor = new FrmVerCompra(IdCompraSeleccionada);
                frmVisor.ShowDialog();
            }
            catch (Exception Ex)
            {
                ComprasProcMostrarError("Error al intentar ver los detalles de la compra.", Ex);
            }
        }

        private async Task ComprasProcRefrescarAsync()
        {
            if (_Cargando) return;
            ComprasProcRestablecerFiltros();
            await ComprasProcCargarDatosAsync();
        }

        private void ComprasProcRestablecerFiltros()
        {
            _RestableciendoFiltros = true;
            try
            {
                ComprasTxtBuscar.Text = string.Empty;
                ComprasTxtBuscar.ComprasMetLimpiarError();
                if (ComprasCboEstado.Items.Count > 0) ComprasCboEstado.SelectedIndex = 0;

                ComprasChkFechas.Checked = false;
                DateTime Hoy = DateTime.Today;
                ComprasUsrFechas.ComprasProcEstablecerRango(new DateTime(Hoy.Year, Hoy.Month, 1), Hoy);
                ComprasUsrFechas.Enabled = false;
            }
            finally { _RestableciendoFiltros = false; }
        }

        private void ComprasProcVaciarDatos()
        {
            ComprasDgvEncabezados.DataSource = null;
            _Datos?.Dispose();
            _Datos = null;
        }

        protected override void OnFormClosed(FormClosedEventArgs Evento)
        {
            ComprasProcVaciarDatos();
            base.OnFormClosed(Evento);
        }

        private void ComprasProcMostrarError(string Mensaje, Exception Ex)
        {
            System.Diagnostics.Debug.WriteLine(Ex);
            ComprasLblResultado.ColorTexto = Color.Red;
            ComprasLblResultado.Texto = $"{Mensaje} {Ex.Message}";
        }
    }
}
