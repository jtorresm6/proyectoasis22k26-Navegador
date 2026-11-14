using CapaVista_OrdenesCompra.Presentacion;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using Microsoft.Reporting.WinForms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Coordina la consulta, los filtros, los reportes y la apertura de órdenes de compra.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    public partial class FrmEncabezados : Form
    {
        private readonly ClsOrdenesCompraControlador _Controlador;
        private readonly ClsOrdenesCompraComunEdicion _Edicion;
        private DataTable _Datos;
        private bool _Cargando;
        private bool _CargaInicial = true;
        private bool _AbriendoCompra;
        private bool _RestableciendoFiltros;
        public bool AbrirOrden { get; private set; }
        private Form _OrdenPreparada;

        public Form ComprasFuncCrearOrdenSeleccionada()
        {
            if (!AbrirOrden) throw new InvalidOperationException("No hay una orden seleccionada.");
            var Orden = _OrdenPreparada;
            if (Orden == null) throw new InvalidOperationException("La orden no está preparada para abrirse.");
            _OrdenPreparada = null;
            return Orden;
        }

        public FrmEncabezados() : this(new ClsOrdenesCompraControlador()) { }

        public FrmEncabezados(ClsOrdenesCompraControlador Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
            _Edicion = new ClsOrdenesCompraComunEdicion(this);
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasCboEstado.Items.AddRange(new object[] { "Todos", "Pendiente", "Autorizada", "Rechazada", "Anulada", "Recibida" });
            ComprasProcRestablecerFiltros();
            ClsEncabezadosOrdenesCompraTabla.ComprasProcConfigurarEncabezados(ComprasDgvEncabezados);
            ComprasUsrEncabezado.PrepararReporte += (Remitente, Evento) =>
            {
                if (_Cargando || _AbriendoCompra)
                    throw new InvalidOperationException("Espera a que termine la carga de las órdenes de compra.");
                var DatosReporte = ComprasDgvEncabezados.DataSource as DataTable;
                if (DatosReporte == null)
                    throw new InvalidOperationException("Actualiza el listado de órdenes de compra antes de abrir el reporte.");
                Evento.Reporte.DataSources.Clear();
                Evento.Reporte.DataSources.Add(new ReportDataSource("RpReporteEncabezadosCompras", DatosReporte));
            };
            ComprasTxtBuscar.TextChanged += (Remitente, Evento) => ComprasProcAplicarFiltros();
            ComprasCboEstado.SelectedIndexChanged += (Remitente, Evento) =>
            {
                if (!_RestableciendoFiltros)
                    ComprasProcAplicarFiltros();
            };
            ComprasUsrFechas.RangoCambiado += (Remitente, Evento) => ComprasProcAplicarFiltros();
            ComprasChkFechas.CheckedChanged += (Remitente, Evento) =>
            {
                _Edicion.Establecer(ComprasUsrFechas, ComprasChkFechas.Checked && !_Cargando && !_AbriendoCompra);
                ComprasProcAplicarFiltros();
            };
            ComprasBtnActualizar.Click += async (Remitente, Evento) => await ComprasProcRefrescarAsync();
            ComprasBtnNuevaOrden.Click += ComprasProcNuevaOrden;
            ComprasBtnCerrar.Click += (Remitente, Evento) => Close();
            ComprasDgvEncabezados.CellDoubleClick += ComprasProcAbrirCompra;
        }

        protected override async void OnLoad(EventArgs Evento)
        {
            base.OnLoad(Evento);
            await ComprasProcCargarDatosAsync();
        }

        private void ComprasProcActualizarDisponibilidad()
        {
            bool Disponible = !_Cargando && !_AbriendoCompra;
            ComprasBtnActualizar.Enabled = Disponible;
            ComprasBtnNuevaOrden.Enabled = Disponible;
            _Edicion.Aplicar(ComprasGbxFiltros, Disponible);
            _Edicion.Establecer(ComprasUsrFechas, Disponible && ComprasChkFechas.Checked);
            _Edicion.Establecer(ComprasDgvEncabezados, Disponible);
            ComprasBtnCerrar.Enabled = true;
            UseWaitCursor = !Disponible;
        }

        private async Task ComprasProcCargarDatosAsync()
        {
            if (_Cargando) return;
            Point Desplazamiento = ComprasPnlDesplazamiento.AutoScrollPosition;
            Control Foco = ComprasFuncControlEnfocado(this);
            _Cargando = true;
            ComprasProcActualizarDisponibilidad();
            ComprasPnlDesplazamiento.AutoScrollPosition = new Point(-Desplazamiento.X, -Desplazamiento.Y);
            string Estado = ComprasCboEstado.Text;
            ComprasLblResultado.ColorTexto = ClsTemaCompras.PrincipalNavegador;
            ComprasLblResultado.Texto = "Cargando órdenes de compra...";
            try
            {
                DataTable Datos = await Task.Run(() => _Controlador.ComprasFuncListar(Estado));
                if (IsDisposed || Disposing)
                {
                    Datos.Dispose();
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
                    ComprasProcMostrarError("No se pudieron cargar las órdenes de compra.", Ex);
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
                        ComprasBtnNuevaOrden.Focus();
                        ComprasPnlDesplazamiento.AutoScrollPosition = Point.Empty;
                    }
                    else
                    {
                        if (Foco != null && !Foco.IsDisposed && Foco.CanFocus)
                            Foco.Focus();
                        ComprasPnlDesplazamiento.AutoScrollPosition = new Point(-Desplazamiento.X, -Desplazamiento.Y);
                    }
                }
            }
        }

        private static Control ComprasFuncControlEnfocado(Control Contenedor)
        {
            if (Contenedor.Focused) return Contenedor;
            if (!Contenedor.ContainsFocus) return null;
            foreach (Control Hijo in Contenedor.Controls)
            {
                Control Foco = ComprasFuncControlEnfocado(Hijo);
                if (Foco != null) return Foco;
            }
            return null;
        }

        private void ComprasProcAplicarFiltros()
        {
            if (_Datos == null || _RestableciendoFiltros) return;
            DateTime? Inicio = ComprasChkFechas.Checked ? (DateTime?)ComprasUsrFechas.FechaInicio : null;
            DateTime? Fin = ComprasChkFechas.Checked ? (DateTime?)ComprasUsrFechas.FechaFin : null;
            DataTable Resultado;
            try
            {
                Resultado = _Controlador.ComprasFuncFiltrar(_Datos, ComprasTxtBuscar.Text, ComprasCboEstado.Text, Inicio, Fin);
            }
            catch (ArgumentException Ex)
            {
                ComprasLblResultado.ColorTexto = Color.Red;
                ComprasLblResultado.Texto = Ex.Message;
                return;
            }
            Point Desplazamiento = ComprasPnlDesplazamiento.AutoScrollPosition;
            var Anterior = ComprasDgvEncabezados.DataSource as DataTable;
            ComprasDgvEncabezados.DataSource = Resultado;
            Anterior?.Dispose();
            ComprasDgvEncabezados.ClearSelection();
            if (!string.IsNullOrWhiteSpace(ComprasTxtBuscar.Text) && Resultado.Rows.Count == 0)
                ComprasTxtBuscar.ComprasMetMostrarError("No se encontraron órdenes de compra para la búsqueda.");
            else
                ComprasTxtBuscar.ComprasMetLimpiarError();
            ComprasLblResultado.ColorTexto = ClsTemaCompras.PrincipalNavegador;
            ComprasLblResultado.Texto = Resultado.Rows.Count + " órdenes de compra. Haz doble clic en una fila para ver el detalle.";
        }

        private async Task ComprasProcRefrescarAsync()
        {
            if (_Cargando || _AbriendoCompra) return;
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
                ComprasCboEstado.SelectedIndex = 0;
                ComprasChkFechas.Checked = false;
                DateTime Hoy = DateTime.Today;
                ComprasUsrFechas.ComprasProcEstablecerRango(new DateTime(Hoy.Year, Hoy.Month, 1), Hoy);
                _Edicion.Establecer(ComprasUsrFechas, false);
            }
            finally { _RestableciendoFiltros = false; }
        }

        private async void ComprasProcNuevaOrden(object Remitente, EventArgs Evento)
        {
            if (_Cargando || _AbriendoCompra) return;
            _AbriendoCompra = true;
            ComprasProcActualizarDisponibilidad();
            try { await ComprasProcMostrarOrdenAsync(null); }
            catch (Exception Ex)
            {
                if (!IsDisposed && !Disposing) ComprasProcMostrarError("No se pudo abrir la nueva orden.", Ex);
            }
            finally
            {
                _AbriendoCompra = false;
                if (!IsDisposed && !Disposing) ComprasProcActualizarDisponibilidad();
            }
        }

        private async Task ComprasProcMostrarOrdenAsync(ClsOrdenesCompraDatos Compra)
        {
            Form Orden = null;
            try
            {
                if (Compra == null)
                {
                    var Nueva = new FrmOrdenesCompra(_Controlador);
                    Orden = Nueva;
                    await Nueva.ComprasProcPrepararAsync();
                }
                else
                {
                    var Detalle = new FrmDetalleOrdenCompra(Compra, _Controlador);
                    Orden = Detalle;
                    await Detalle.ComprasProcPrepararAsync();
                }
                if (IsDisposed || Disposing) return;
                _OrdenPreparada = Orden;
                AbrirOrden = true;
                Close();
                if (!AbrirOrden) return;
                Orden = null;
            }
            finally
            {
                if (Orden != null)
                {
                    if (ReferenceEquals(_OrdenPreparada, Orden))
                    {
                        _OrdenPreparada = null;
                        AbrirOrden = false;
                    }
                    Orden.Dispose();
                }
            }
        }

        private async void ComprasProcAbrirCompra(object Remitente, DataGridViewCellEventArgs Evento)
        {
            if (_Cargando || _AbriendoCompra || Evento.RowIndex < 0 || Evento.ColumnIndex < 0
                || Evento.RowIndex >= ComprasDgvEncabezados.Rows.Count) return;
            var Fila = ComprasDgvEncabezados.Rows[Evento.RowIndex].DataBoundItem as DataRowView;
            if (Fila == null) return;
            _AbriendoCompra = true;
            ComprasProcActualizarDisponibilidad();
            try
            {
                int Id = Convert.ToInt32(Fila["idOrdenCompra"]);
                ClsOrdenesCompraDatos Compra = await Task.Run(() => _Controlador.ComprasFuncObtener(Id));
                if (IsDisposed || Disposing) return;
                if (Compra == null) { ComprasLblResultado.Texto = "La compra ya no esta disponible. Actualiza la lista."; return; }
                await ComprasProcMostrarOrdenAsync(Compra);
            }
            catch (Exception Ex)
            {
                if (!IsDisposed && !Disposing) ComprasProcMostrarError("No se pudo abrir el detalle de la orden seleccionada.", Ex);
            }
            finally
            {
                _AbriendoCompra = false;
                if (!IsDisposed && !Disposing) ComprasProcActualizarDisponibilidad();
            }
        }

        private void ComprasProcVaciarDatos()
        {
            var Resultado = ComprasDgvEncabezados.DataSource as DataTable;
            ComprasDgvEncabezados.DataSource = null;
            Resultado?.Dispose();
            _Datos?.Dispose();
            _Datos = null;
        }

        protected override void OnFormClosing(FormClosingEventArgs Evento)
        {
            base.OnFormClosing(Evento);
            if (Evento.Cancel && AbrirOrden)
            {
                AbrirOrden = false;
                _OrdenPreparada?.Dispose();
                _OrdenPreparada = null;
            }
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
            ComprasLblResultado.Texto = Mensaje + " " + Ex.Message;
        }
    }
}
