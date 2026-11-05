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
 *  Presenta las compras por pagar y administra su búsqueda y filtros,
 *  la actualización de datos y la apertura de órdenes nuevas o existentes.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    public partial class FrmEncabezados : Form
    {
        private readonly ClsComprasPorPagar _Controlador;
        private DataTable _Datos;
        private bool _Cargando;
        private bool _AbriendoCompra;
        private bool _RestableciendoFiltros;
        public bool AbrirOrden { get; private set; }
        private ClsCompraPorPagar _CompraSeleccionada;
        public Form ComprasFuncCrearOrdenSeleccionada()
        {
            if (!AbrirOrden) throw new InvalidOperationException("No hay una orden seleccionada.");
            return new FrmOrdenesCompra(_CompraSeleccionada, _Controlador);
        }

        public FrmEncabezados() : this(new ClsComprasPorPagar()) { }
        public FrmEncabezados(ClsComprasPorPagar Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasCboEstado.Items.AddRange(new object[] { "Todos", "Pendiente", "Parcial", "Pagada", "Anulada" });
            ComprasProcRestablecerFiltros();
            ComprasProcConfigurarTabla();
            ComprasTxtBuscar.TextChanged += (Remitente, Evento) => ComprasProcAplicarFiltros();
            ComprasCboEstado.SelectedIndexChanged += (Remitente, Evento) => ComprasProcAplicarFiltros();
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

        private void ComprasProcConfigurarTabla()
        {
            ComprasDgvEncabezados.Tabla.RowTemplate.MinimumHeight = 40;
            ComprasDgvEncabezados.Tabla.RowTemplate.Height = 40;
            ComprasDgvEncabezados.AutoGenerateColumns = false;
            foreach (var Campo in new[] {
                new[] { "numeroFacturaCuentaPorPagar", "Factura", "16" },
                new[] { "nombreProveedor", "Proveedor", "24" },
                new[] { "nitProveedor", "NIT", "11" },
                new[] { "fechaEmisionCuentaPorPagar", "Emision", "12" },
                new[] { "fechaVencimientoCuentaPorPagar", "Vencimiento", "12" },
                new[] { "montoTotalCuentaPorPagar", "Monto total", "12" },
                new[] { "saldoCuentaPorPagar", "Saldo", "12" },
                new[] { "estadoCuentaPorPagar", "Estado", "11" } })
            {
                var Columna = new DataGridViewTextBoxColumn { Name = "ComprasCol" + Campo[0],
                    DataPropertyName = Campo[0], HeaderText = Campo[1], FillWeight = int.Parse(Campo[2]), ReadOnly = true };
                if (Campo[0].StartsWith("fecha")) Columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                if (Campo[0] == "montoTotalCuentaPorPagar" || Campo[0] == "saldoCuentaPorPagar")
                {
                    Columna.DefaultCellStyle.Format = "'Q '#,##0.00";
                    Columna.DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;
                    Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                ComprasDgvEncabezados.Columns.Add(Columna);
            }
        }

        private async Task ComprasProcCargarDatosAsync()
        {
            if (_Cargando) return;
            _Cargando = true;
            ComprasBtnActualizar.Enabled = false;
            ComprasBtnNuevaOrden.Enabled = false;
            UseWaitCursor = true;
            ComprasLblResultado.ColorTexto = ClsTemaCompras.PrincipalNavegador;
            ComprasLblResultado.Texto = "Cargando compras por pagar...";
            try
            {
                DataTable Datos = await Task.Run(() => _Controlador.ComprasFuncListar());
                if (IsDisposed || Disposing) return;
                _Datos = Datos;
                ComprasProcAplicarFiltros();
            }
            catch (Exception Ex)
            {
                if (!IsDisposed && !Disposing) ComprasProcMostrarError("No se pudieron cargar las compras por pagar.", Ex);
            }
            finally
            {
                _Cargando = false;
                if (!IsDisposed && !Disposing) { ComprasBtnActualizar.Enabled = true; ComprasBtnNuevaOrden.Enabled = true; UseWaitCursor = false; }
            }
        }

        private void ComprasProcAplicarFiltros()
        {
            if (_Datos == null || _RestableciendoFiltros) return;
            DateTime? Inicio = ComprasChkFechas.Checked ? (DateTime?)ComprasUsrFechas.FechaInicio : null;
            DateTime? Fin = ComprasChkFechas.Checked ? (DateTime?)ComprasUsrFechas.FechaFin : null;
            DataTable Resultado = _Controlador.ComprasFuncFiltrar(_Datos, ComprasTxtBuscar.Text, ComprasCboEstado.Text, Inicio, Fin);
            ComprasDgvEncabezados.DataSource = Resultado;
            ComprasDgvEncabezados.ClearSelection();
            if (!string.IsNullOrWhiteSpace(ComprasTxtBuscar.Text) && Resultado.Rows.Count == 0)
                ComprasTxtBuscar.ComprasMetMostrarError("No se encontraron compras por pagar para la búsqueda.");
            else
                ComprasTxtBuscar.ComprasMetLimpiarError();
            ComprasLblResultado.ColorTexto = ClsTemaCompras.PrincipalNavegador;
            ComprasLblResultado.Texto = Resultado.Rows.Count + " compras por pagar. Doble clic para abrir.";
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

        private void ComprasProcMostrarOrden(ClsCompraPorPagar Compra)
        {
            _CompraSeleccionada = Compra;
            AbrirOrden = true;
            Close();
        }

        private async void ComprasProcAbrirCompra(object Remitente, DataGridViewCellEventArgs Evento)
        {
            if (_Cargando || _AbriendoCompra || Evento.RowIndex < 0 || Evento.RowIndex >= ComprasDgvEncabezados.Rows.Count) return;
            var Fila = ComprasDgvEncabezados.Rows[Evento.RowIndex].DataBoundItem as DataRowView;
            if (Fila == null) return;
            _AbriendoCompra = true;
            ComprasDgvEncabezados.Enabled = false;
            UseWaitCursor = true;
            try
            {
                int Id = Convert.ToInt32(Fila["idCuentaPorPagar"]);
                ClsCompraPorPagar Compra = await Task.Run(() => _Controlador.ComprasFuncObtener(Id));
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
                if (!IsDisposed && !Disposing) { ComprasDgvEncabezados.Enabled = true; UseWaitCursor = false; }
            }
        }

        private void ComprasProcMostrarError(string Mensaje, Exception Ex)
        {
            System.Diagnostics.Debug.WriteLine(Ex);
            ComprasLblResultado.ColorTexto = Color.Red;
            ComprasLblResultado.Texto = Mensaje + " " + Ex.Message;
        }
    }
}
