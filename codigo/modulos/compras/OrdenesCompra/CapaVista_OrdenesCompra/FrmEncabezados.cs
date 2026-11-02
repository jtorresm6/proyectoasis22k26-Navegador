using CapaVista_OrdenesCompra.Presentacion;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaControlador_OrdenesCompra.Entidades;
using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Coordina la consulta y apertura de órdenes de compra desde su listado.
 * ===================================================================
*/









namespace CapaVista_OrdenesCompra
{
    public partial class FrmEncabezados : Form
    {
        private readonly ClsOrdenesCompra _Controlador;
        private DataTable _Datos;
        private bool _Cargando;
        private bool _CargaInicial = true;
        private bool _AbriendoCompra;
        private bool _RestableciendoFiltros;
        public bool AbrirOrden { get; private set; }
        private ClsOrdenCompra _CompraSeleccionada;
        
        
        
        public Form ComprasFuncCrearOrdenSeleccionada()
        {
            if (!AbrirOrden) throw new InvalidOperationException("No hay una orden seleccionada.");
            return _CompraSeleccionada == null
                ? (Form)new FrmOrdenesCompra()
                : new FrmDetalleOrdenCompra(_CompraSeleccionada, _Controlador);
        }

        
        
        
        public FrmEncabezados() : this(new ClsOrdenesCompra()) { }
        
        
        
        
        public FrmEncabezados(ClsOrdenesCompra Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasCboEstado.Items.AddRange(new object[] { "Todos", "Pendiente", "Autorizada", "Rechazada", "Anulada", "Recibida" });
            ComprasProcRestablecerFiltros();
            ClsTablasOrdenesCompra.ComprasProcConfigurarEncabezados(ComprasDgvEncabezados);
            ComprasTxtBuscar.TextChanged += (Remitente, Evento) => ComprasProcAplicarFiltros();
            ComprasCboEstado.SelectedIndexChanged += (Remitente, Evento) =>
            {
                if (!_RestableciendoFiltros)
                    ComprasProcAplicarFiltros();
            };
            ComprasUsrFechas.RangoCambiado += (Remitente, Evento) => ComprasProcAplicarFiltros();
            ComprasChkFechas.CheckedChanged += (Remitente, Evento) =>
            {
                ComprasUsrFechas.Enabled = ComprasChkFechas.Checked;
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
            ComprasGbxFiltros.Enabled = Disponible;
            ComprasDgvEncabezados.Enabled = Disponible;
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
            ComprasLblResultado.Texto = Resultado.Rows.Count + " órdenes de compra. Doble clic para abrir.";
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
                ComprasUsrFechas.Enabled = false;
            }
            finally { _RestableciendoFiltros = false; }
        }

        
        
        
        private void ComprasProcNuevaOrden(object Remitente, EventArgs Evento)
        {
            if (_Cargando || _AbriendoCompra) return;
            _AbriendoCompra = true;
            try { ComprasProcMostrarOrden(null); }
            catch (Exception Ex) { ComprasProcMostrarError("No se pudo abrir la nueva orden.", Ex); }
            finally { _AbriendoCompra = false; }
        }

        
        
        
        
        private void ComprasProcMostrarOrden(ClsOrdenCompra Compra)
        {
            _CompraSeleccionada = Compra;
            AbrirOrden = true;
            Close();
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
                ClsOrdenCompra Compra = await Task.Run(() => _Controlador.ComprasFuncObtener(Id));
                if (IsDisposed || Disposing) return;
                UseWaitCursor = false;
                if (Compra == null) { ComprasLblResultado.Texto = "La compra ya no esta disponible. Actualiza la lista."; return; }
                ComprasProcMostrarOrden(Compra);
            }
            catch (Exception Ex)
            {
                if (!IsDisposed && !Disposing) ComprasProcMostrarError("No se pudo abrir la compra seleccionada.", Ex);
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
