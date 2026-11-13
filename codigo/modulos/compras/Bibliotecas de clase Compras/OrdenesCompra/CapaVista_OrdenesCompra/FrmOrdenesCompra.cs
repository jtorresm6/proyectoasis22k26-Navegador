using CapaControlador_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Gestiona la captura, validación y guardado de una nueva orden de compra.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    public partial class FrmOrdenesCompra : Form
    {
        private readonly ClsOrdenesCompraControlador _Controlador;
        private readonly ErrorProvider _Errores;
        private readonly List<DataTable> _Catalogos = new List<DataTable>();
        private bool _CargandoCatalogos = true;
        private bool _CatalogosCargados;
        private bool _Liberado;
        private bool _Guardando;
        public bool OrdenGuardada { get; private set; }
        public int? NumeroOrdenGuardada { get; private set; }
        private int _VersionCambio;
        private decimal? _SaldoProveedorBase;
        private decimal? _CostoProductoBase;
        private DataView _VistaBodegas;
        private DataView _VistaProductosSku;
        private DataView _VistaProductosNombre;
        private bool _ActualizandoProducto;
        private bool _ActualizandoSucursal;
        private bool _ActualizandoProveedor;
        private bool _ProveedorPendiente;
        private int? _ProveedorAceptado;
        private string _TextoProveedorAceptado = string.Empty;
        private bool _CapturaDisponible;
        private readonly Presentacion.ClsOrdenesCompraComunEdicion _Edicion;
        private bool _ActualizandoDescuento;
        private decimal _DescuentoPorcentaje;
        private decimal _ImpuestoPorcentaje;
        private bool _ImpuestoValido = true;
        private bool _TotalesValidos = true;
        private bool _ActualizandoImpuesto;
        private bool _DescuentoValido = true;
        private readonly Presentacion.ClsOrdenesCompraErrores _FilasErrores;
        private DateTime _FechaEmision = DateTime.Today;
        public bool CatalogosCargados => _CatalogosCargados;
        public long? IdMonedaSeleccionada { get; private set; }
        public long? IdTipoCambioSeleccionado { get; private set; }
        public decimal? TipoCambioSeleccionado { get; private set; }
        public int? IdProveedorSeleccionado { get; private set; }
        public int? IdPlazoSeleccionado { get; private set; }
        public DateTime? FechaVencimiento { get; private set; }

        public FrmOrdenesCompra() : this(new ClsOrdenesCompraControlador()) { }

        public FrmOrdenesCompra(ClsOrdenesCompraControlador Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
            _Edicion = new Presentacion.ClsOrdenesCompraComunEdicion(ComprasPnlContenido);
            ComprasTxtDescripcionProducto.Entrada.ScrollBars = RichTextBoxScrollBars.Vertical;
            ComprasProcAlinearEtiquetas(ComprasPnlContenido);
            _Componentes = new System.ComponentModel.Container();
            _Errores = new ErrorProvider(_Componentes) { ContainerControl = this, BlinkStyle = ErrorBlinkStyle.NeverBlink };
            _FilasErrores = new Presentacion.ClsOrdenesCompraErrores(ComprasPnlContenido, ComprasProcReorganizarGrupos);
            ComprasProcReorganizarGrupos();
            foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo, ComprasCboSucursal, ComprasCboBodega, ComprasCboSkuProducto, ComprasCboNombreProducto })
            {
                Combo.TipoDatos = EnmTipoDatosCompras.Ninguno;
                Combo.PermitirEscritura = true;
                _Edicion.Establecer(Combo, false);
                ComprasProcConfigurarValidacionCatalogo(Combo);
            }
            ComprasProcConfigurarProductos();
            ComprasBtnGuardar.Click += ComprasProcGuardarOrden;
            ComprasBtnDescartar.Click += ComprasProcRegresarEncabezados;
            ComprasBtnGuardar.AccessibleName = "Guardar orden de compra";
            ComprasBtnDescartar.AccessibleName = "Regresar a encabezados";
            ComprasBtnGuardar.Enabled = false;
            FormClosing += (Remitente, Evento) => { if (_Guardando) Evento.Cancel = true; };
            ComprasTxtFechaEmision.Text = _FechaEmision.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            ComprasLblEstadoProveedorValor.Texto = string.Empty;
            ComprasCboMoneda.SelectedIndexChanged += async (Remitente, Evento) => await ComprasProcActualizarCambioAsync();
            ComprasCboMoneda.Entrada.FormattingEnabled = true;
            ComprasCboPlazo.Entrada.FormattingEnabled = true;
            ComprasCboNombreProveedor.Entrada.FormattingEnabled = true;
            ComprasCboSkuProducto.Entrada.FormattingEnabled = true;
            ComprasCboNombreProducto.Entrada.FormattingEnabled = true;
            ComprasCboNombreProveedor.SelectedIndexChanged += (Remitente, Evento) => ComprasProcProgramarProveedor();
            ComprasCboNombreProveedor.TextChanged += (Remitente, Evento) => ComprasProcProgramarProveedor();
            ComprasCboPlazo.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarVencimiento();
            ComprasCboSucursal.Entrada.FormattingEnabled = true;
            ComprasCboBodega.Entrada.FormattingEnabled = true;
            ComprasCboSucursal.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarSucursal();
            ComprasCboSucursal.TextChanged += (Remitente, Evento) => ComprasProcActualizarSucursal();
            ComprasCboBodega.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarBodega();
            ComprasCboSkuProducto.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarProducto(ComprasCboSkuProducto);
            ComprasCboNombreProducto.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarProducto(ComprasCboNombreProducto);
            ComprasCboSkuProducto.TextChanged += (Remitente, Evento) => ComprasProcActualizarProducto(ComprasCboSkuProducto);
            ComprasCboNombreProducto.TextChanged += (Remitente, Evento) => ComprasProcActualizarProducto(ComprasCboNombreProducto);
            ComprasTxtCantidadProducto.TextChanged += (Remitente, Evento) => ComprasProcActualizarDescuentos();
            ComprasTxtDescuentoPorcentajeProducto.TextChanged += (Remitente, Evento) => ComprasProcCambiarDescuento();
            ComprasTxtImpuestoPorcentaje.TextChanged += (Remitente, Evento) => ComprasProcCambiarImpuesto();
            ComprasTxtFechaEmision.TextChanged += async (Remitente, Evento) =>
            {
                if (_Liberado) return;
                if (!DateTime.TryParseExact(ComprasTxtFechaEmision.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _FechaEmision))
                {
                    _VersionCambio++;
                    IdTipoCambioSeleccionado = null;
                    TipoCambioSeleccionado = null;
                    FechaVencimiento = null;
                    IdPlazoSeleccionado = null;
                    ComprasTxtTipoCambio.Text = string.Empty;
                    ComprasProcActualizarImportesConvertidos();
                    ComprasTxtFechaVencimiento.Text = string.Empty;
                    ComprasProcEstablecerError(ComprasTxtFechaEmision, "Usa una fecha valida con formato dd/MM/yyyy.");
                    return;
                }
                ComprasProcEstablecerError(ComprasTxtFechaEmision, string.Empty);
                ComprasProcActualizarVencimiento();
                await ComprasProcActualizarCambioAsync();
            };
            foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo })
            {
                Combo.TextChanged += (Remitente, Evento) => ComprasProcActualizarFlujo();
                Combo.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarFlujo();
            }
            ComprasTxtFechaEmision.TextChanged += (Remitente, Evento) => ComprasProcActualizarFlujo();
            ComprasTxtFechaVencimiento.TextChanged += (Remitente, Evento) => ComprasProcActualizarFlujo();
            ComprasProcActualizarFlujo();
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasUsrProductos.Tabla.Tabla.ScrollBars = ScrollBars.None;
        }

        protected virtual DialogResult ComprasFuncConfirmarAccion(string Mensaje, string Titulo)
        {
            return MessageBox.Show(this, Mensaje, Titulo, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }

        protected virtual void ComprasProcMostrarResultado(string Mensaje, bool Error)
        {
            MessageBox.Show(this, Mensaje, "Órdenes de compra", MessageBoxButtons.OK,
                Error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
        }

        private void ComprasProcRegresarEncabezados(object Remitente, EventArgs Evento)
        {
            if (_Guardando) return;
            if (ComprasFuncConfirmarAccion("¿Deseas regresar a encabezados sin guardar la orden de compra?",
                "Regresar a encabezados") == DialogResult.Yes)
                Close();
        }

        private async void ComprasProcGuardarOrden(object Remitente, EventArgs Evento)
        {
            if (_Guardando || OrdenGuardada || _Liberado) return;
            if (_CargandoCatalogos || !_CatalogosCargados || !IdProveedorSeleccionado.HasValue
                || !IdPlazoSeleccionado.HasValue || !IdMonedaSeleccionada.HasValue
                || !TipoCambioSeleccionado.HasValue || !FechaVencimiento.HasValue
                || ComprasUsrProductos.Detalles.Rows.Count == 0)
            {
                ComprasProcMostrarResultado("Selecciona proveedor, moneda, plazo y tipo de cambio válidos, y agrega al menos un producto.", true);
                return;
            }
            if (_VistaProductosSku != null)
                foreach (DataRow Detalle in ComprasUsrProductos.Detalles.Rows)
                {
                    bool Pertenece = false;
                    foreach (DataRowView Producto in _VistaProductosSku)
                        if (Convert.ToInt32(Producto["idProducto"]) == Convert.ToInt32(Detalle["idProducto"]))
                        { Pertenece = true; break; }
                    if (!Pertenece)
                    {
                        ComprasProcMostrarResultado("La orden contiene productos de otro proveedor. Elimínalos o selecciona su proveedor antes de guardar.", true);
                        return;
                    }
                }
            ComprasProcActualizarImpuesto();
            if (!_ImpuestoValido || !_TotalesValidos)
            {
                ComprasProcMostrarResultado("Revisa el porcentaje de impuesto y el total de la orden antes de guardar.", true);
                return;
            }
            if (ComprasFuncConfirmarAccion("¿Estás seguro de guardar la orden de compra?",
                "Guardar orden de compra") != DialogResult.Yes) return;
            using (var Detalles = ComprasUsrProductos.Detalles.Copy())
            {
                int Proveedor = IdProveedorSeleccionado.Value;
                int Plazo = IdPlazoSeleccionado.Value;
                int Moneda = checked((int)IdMonedaSeleccionada.Value);
                DateTime Emision = _FechaEmision;
                DateTime Vencimiento = FechaVencimiento.Value;
                decimal Cambio = TipoCambioSeleccionado.Value;
                long? IdCambio = IdTipoCambioSeleccionado;
                string Comentario = ComprasTxtComentario.Text;
                decimal ImpuestoPorcentaje = _ImpuestoPorcentaje;
                _Guardando = true;
                ComprasProcActualizarFlujo();
                UseWaitCursor = true;
                try
                {
                    int Numero = await Task.Run(() => _Controlador.ComprasFuncCrearConDetalles(Proveedor, Plazo, Moneda,
                        Emision, Vencimiento, Cambio, IdCambio, Comentario, Detalles, ImpuestoPorcentaje));
                    if (Numero <= 0) throw new InvalidOperationException("No se obtuvo el número de orden.");
                    NumeroOrdenGuardada = Numero;
                    OrdenGuardada = true;
                }
                catch (Exception Ex)
                {
                    ComprasProcMostrarResultado("No se pudo guardar la orden. " + Ex.Message, true);
                }
                finally
                {
                    _Guardando = false;
                    ComprasProcActualizarFlujo();
                    UseWaitCursor = false;
                }
            }
            if (OrdenGuardada)
            {
                ComprasProcMostrarResultado("Orden de compra guardada. Número de orden: "
                    + NumeroOrdenGuardada.Value.ToString(CultureInfo.InvariantCulture), false);
                Close();
            }
        }

        private void ComprasProcAlinearEtiquetas(Control Contenedor)
        {
            var Hijos = new List<Control>();
            foreach (Control Hijo in Contenedor.Controls) Hijos.Add(Hijo);
            foreach (Control Hijo in Hijos) ComprasProcAlinearEtiquetas(Hijo);
            var Tabla = Contenedor as TableLayoutPanel;
            if (Tabla == null) return;
            foreach (Control Etiqueta in Hijos)
            {
                if (!(Etiqueta is ComprasUsrLabel)) continue;
                int Columna = Tabla.GetColumn(Etiqueta);
                int Fila = Tabla.GetRow(Etiqueta);
                var Campo = Tabla.GetControlFromPosition(Columna + 1, Fila);
                Control Entrada = (Campo as ComprasUsrComboBox)?.Entrada;
                if (Entrada == null) Entrada = (Campo as ComprasUsrTextBox)?.Entrada;
                if (Entrada == null) continue;
                int Filas = Tabla.GetRowSpan(Etiqueta);
                int Columnas = Tabla.GetColumnSpan(Etiqueta);
                var Espacio = new Panel
                {
                    Name = Etiqueta.Name + "Espacio", Dock = DockStyle.Fill,
                    Margin = Padding.Empty, TabStop = false, TabIndex = Etiqueta.TabIndex
                };
                Tabla.Controls.Remove(Etiqueta);
                Tabla.Controls.Add(Espacio, Columna, Fila);
                Tabla.SetRowSpan(Espacio, Filas);
                Tabla.SetColumnSpan(Espacio, Columnas);
                Espacio.Controls.Add(Etiqueta);
                Etiqueta.Dock = DockStyle.None;
                Etiqueta.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                Action Alinear = () =>
                {
                    int Superior = Entrada.Height / 2;
                    for (Control Actual = Entrada; Actual != null && Actual != Tabla; Actual = Actual.Parent)
                        Superior += Actual.Top;
                    Etiqueta.SetBounds(Etiqueta.Margin.Left,
                        Math.Max(0, Superior - Espacio.Top - Etiqueta.Height / 2),
                        Math.Max(0, Espacio.ClientSize.Width - Etiqueta.Margin.Horizontal), Etiqueta.Height);
                };
                Espacio.Layout += (Remitente, Evento) => Alinear();
                Campo.Layout += (Remitente, Evento) => Alinear();
                Campo.LocationChanged += (Remitente, Evento) => Alinear();
                Entrada.SizeChanged += (Remitente, Evento) => Alinear();
                Entrada.LocationChanged += (Remitente, Evento) => Alinear();
                Alinear();
            }
        }

        protected override async void OnShown(EventArgs Evento)
        {
            base.OnShown(Evento);
            await ComprasProcPrepararAsync(false);
        }

        internal async Task ComprasProcPrepararAsync(bool PropagarError = true)
        {
            if (_CatalogosCargados || _Liberado) return;
            ActiveControl = null;
            ComprasPnlDesplazamiento.AutoScrollPosition = Point.Empty;
            try
            {
                var Datos = await Task.Run(() =>
                {
                    var Tablas = new List<DataTable>();
                    try
                    {
                        Tablas.Add(_Controlador.ComprasFuncListarMonedas());
                        Tablas.Add(_Controlador.ComprasFuncListarProveedores());
                        Tablas.Add(_Controlador.ComprasFuncListarPlazos());
                        Tablas.Add(_Controlador.ComprasFuncListarSucursales());
                        Tablas.Add(_Controlador.ComprasFuncListarBodegas());
                        Tablas.Add(_Controlador.ComprasFuncListarProductos());
                        return Tablas;
                    }
                    catch
                    {
                        foreach (var Tabla in Tablas) Tabla.Dispose();
                        throw;
                    }
                });
                if (_Liberado || IsDisposed || Disposing)
                {
                    foreach (var Tabla in Datos) Tabla.Dispose();
                    return;
                }
                _Catalogos.AddRange(Datos);
                Datos[0].Columns.Add("monedaVisible", typeof(string), "codigoMoneda + ' - ' + nombreMoneda");
                ComprasProcVincular(ComprasCboMoneda, Datos[0], "idMoneda", "monedaVisible");
                ComprasProcVincular(ComprasCboNombreProveedor, Datos[1], "idProveedor", "nombreProveedor");
                ComprasProcVincular(ComprasCboPlazo, Datos[2], "idPlazo", "nombrePlazo");
                ComprasProcVincular(ComprasCboSucursal, Datos[3], "idSucursal", "nombreSucursal");
                _VistaBodegas = new DataView(Datos[4]) { RowFilter = "1 = 0" };
                ComprasProcVincular(ComprasCboBodega, _VistaBodegas, "idBodega", "nombreBodega");
                ComprasCboSkuProducto.BindingContext = new BindingContext();
                ComprasCboNombreProducto.BindingContext = new BindingContext();
                _VistaProductosSku = new DataView(Datos[5]) { RowFilter = "1 = 0" };
                _VistaProductosNombre = new DataView(Datos[5]) { RowFilter = "1 = 0" };
                ComprasProcVincular(ComprasCboSkuProducto, _VistaProductosSku, "idProducto", "skuProducto");
                ComprasProcVincular(ComprasCboNombreProducto, _VistaProductosNombre, "idProducto", "nombreProducto");
                _CargandoCatalogos = false;
                foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo, ComprasCboSucursal })
                    _Edicion.Establecer(Combo, true);
                for (int Indice = 0; Indice < Datos[0].Rows.Count; Indice++)
                    if (Convert.ToString(Datos[0].Rows[Indice]["codigoMoneda"]) == "GTQ")
                    {
                        ComprasCboMoneda.SelectedIndex = Indice;
                        break;
                    }
                ComprasProcActualizarProveedor();
                ComprasProcActualizarVencimiento();
                ComprasProcActualizarSucursal();
                ComprasProcActualizarProducto(ComprasCboNombreProducto);
                _CatalogosCargados = true;
                ComprasProcActualizarFlujo();
                ComprasCboMoneda.Focus();
                ComprasPnlDesplazamiento.AutoScrollPosition = Point.Empty;
            }
            catch (Exception Ex)
            {
                if (_Liberado || IsDisposed || Disposing) return;
                _CargandoCatalogos = false;
                if (PropagarError) throw;
                foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo, ComprasCboSucursal, ComprasCboBodega, ComprasCboSkuProducto, ComprasCboNombreProducto })
                    ComprasProcEstablecerError(Combo, "No se pudieron cargar los catalogos. Cierra y vuelve a abrir la orden. " + Ex.Message);
                ComprasPnlDesplazamiento.AutoScrollPosition = Point.Empty;
            }
        }

        private static void ComprasProcVincular(ComprasUsrComboBox Combo, object Datos, string Valor, string Texto)
        {
            ClsOrdenesCompraCatalogos.ComprasProcVincular(Combo, Datos, Valor, Texto);
        }

        private void ComprasProcConfigurarValidacionCatalogo(ComprasUsrComboBox Combo)
        {
            bool Pendiente = false;
            Action Validar = () =>
            {
                if (_Liberado || _CargandoCatalogos || _ActualizandoProducto || Combo.IsDisposed) return;
                bool Invalido = !string.IsNullOrWhiteSpace(Combo.Text) && Combo.FindStringExact(Combo.Text) < 0;
                ComprasProcEstablecerError(Combo, Invalido ? "Selecciona una opción del catálogo." : string.Empty);
            };
            EventHandler Programar = (Remitente, Evento) =>
            {
                if (Pendiente || !Combo.IsHandleCreated || _Liberado) return;
                Pendiente = true;
                Combo.BeginInvoke(new Action(() => { Pendiente = false; Validar(); }));
            };
            Combo.TextChanged += Programar;
            Combo.SelectedIndexChanged += Programar;
            Combo.Leave += (Remitente, Evento) => Validar();
        }

        private static DataRow ComprasFuncFila(ComprasUsrComboBox Combo)
        {
            return ClsOrdenesCompraCatalogos.ComprasFuncFila(Combo);
        }

        private async Task ComprasProcActualizarCambioAsync()
        {
            if (_CargandoCatalogos || _Liberado) return;
            int Version = ++_VersionCambio;
            IdMonedaSeleccionada = null;
            IdTipoCambioSeleccionado = null;
            TipoCambioSeleccionado = null;
            ComprasTxtTipoCambio.Text = string.Empty;
            ComprasProcActualizarImportesConvertidos();
            ComprasProcEstablecerError(ComprasCboMoneda, string.Empty);
            var Moneda = ComprasFuncFila(ComprasCboMoneda);
            try { ComprasProcActualizarCamposMoneda(Moneda); }
            catch (ArgumentException Ex)
            {
                ComprasProcEstablecerError(ComprasCboMoneda, Ex.Message);
                return;
            }
            if (Moneda == null || _FechaEmision == DateTime.MinValue) return;
            IdMonedaSeleccionada = Convert.ToInt64(Moneda["idMoneda"]);
            if (Convert.ToString(Moneda["codigoMoneda"]) == "GTQ")
            {
                TipoCambioSeleccionado = 1M;
                ComprasTxtTipoCambio.Text = 1M.ToString("N6");
                ComprasProcActualizarImportesConvertidos();
                return;
            }
            try
            {
                long Id = IdMonedaSeleccionada.Value;
                DateTime Fecha = _FechaEmision;
                using (var Cotizacion = await Task.Run(() => _Controlador.ComprasFuncObtenerTipoCambio(Id, Fecha)))
                {
                    if (_Liberado || IsDisposed || Disposing || Version != _VersionCambio) return;
                    if (Cotizacion.Rows.Count == 0)
                    {
                        ComprasProcEstablecerError(ComprasCboMoneda, "No existe una cotizacion hacia GTQ para la fecha de emision o una fecha anterior.");
                        return;
                    }
                    var Fila = Cotizacion.Rows[0];
                    decimal Tasa = Convert.ToDecimal(Fila["valorTipoCambio"]);
                    if (Tasa <= 0) throw new InvalidOperationException("La cotizacion debe ser positiva.");
                    IdTipoCambioSeleccionado = Convert.ToInt64(Fila["idTipoCambio"]);
                    TipoCambioSeleccionado = Tasa;
                    ComprasTxtTipoCambio.Text = Tasa.ToString("N6");
                    ComprasProcActualizarImportesConvertidos();
                }
            }
            catch (Exception Ex)
            {
                if (!_Liberado && !IsDisposed && !Disposing && Version == _VersionCambio)
                    ComprasProcEstablecerError(ComprasCboMoneda, "No se pudo consultar el tipo de cambio. " + Ex.Message);
            }
        }

        private void ComprasProcActualizarCamposMoneda(DataRow Moneda)
        {
            if (Moneda == null) return;
            string Codigo = Convert.ToString(Moneda["codigoMoneda"]);
            string Simbolo = Convert.ToString(Moneda["simboloMoneda"]);
            bool SincronizacionAnterior = _ActualizandoDescuento;
            _ActualizandoDescuento = true;
            try
            {
                foreach (var Campo in new[] { ComprasTxtSaldoActualProveedor, ComprasTxtCostoUnitarioProducto, ComprasTxtDescuentoProducto, ComprasTxtTotalProducto,
                    ComprasTxtSubtotalOrden, ComprasTxtDescuentoTotal,
                    ComprasTxtSubtotalNeto, ComprasTxtImpuestoTotal, ComprasTxtTotalOrden })
                {
                    Campo.ComprasMetConfigurarMoneda(Codigo, Simbolo);
                    Campo.TipoEntrada = ComprasEnumTipoEntrada.Moneda;
                    if (Campo != ComprasTxtSaldoActualProveedor && Campo != ComprasTxtCostoUnitarioProducto && Campo.ReadOnly && string.IsNullOrWhiteSpace(Campo.Text))
                        ClsPresentacionCompras.ComprasProcMostrarMonto(Campo, 0M);
                }
            }
            finally { _ActualizandoDescuento = SincronizacionAnterior; }
            if (ComprasUsrProductos.Tabla.Columns.Count > 0)
                foreach (DataGridViewColumn Columna in ComprasUsrProductos.Tabla.Columns)
                    if (Columna.DataPropertyName == "costoUnitarioDetalleOrdenCompra" || Columna.DataPropertyName == "subtotalDetalleOrdenCompra")
                        Columna.DefaultCellStyle.Format = "'" + Simbolo.Replace("'", "''") + "' #,##0.0000";
        }

        private void ComprasProcProgramarProveedor()
        {
            if (_ActualizandoProveedor || _ProveedorPendiente || _CargandoCatalogos || _Liberado) return;
            if (ComprasUsrProductos.Detalles.Rows.Count == 0 || !ComprasCboNombreProveedor.IsHandleCreated)
            {
                ComprasProcActualizarProveedor();
                return;
            }
            _ProveedorPendiente = true;
            ComprasCboNombreProveedor.BeginInvoke(new Action(() =>
            {
                _ProveedorPendiente = false;
                ComprasProcActualizarProveedor();
                ComprasProcActualizarFlujo();
            }));
        }

        private void ComprasProcActualizarProveedor()
        {
            if (_CargandoCatalogos || _Liberado || _ActualizandoProveedor) return;
            _ActualizandoProveedor = true;
            try
            {
                var Proveedor = ComprasFuncFila(ComprasCboNombreProveedor);
                int? NuevoProveedor = Proveedor == null ? (int?)null : Convert.ToInt32(Proveedor["idProveedor"]);
                if (_ProveedorAceptado != NuevoProveedor && ComprasUsrProductos.Detalles.Rows.Count > 0)
                {
                    if (ComprasFuncConfirmarAccion("Al cambiar de proveedor se eliminarán todos los productos agregados a la orden. ¿Deseas continuar?",
                        "Cambiar proveedor") != DialogResult.Yes)
                    {
                        ComprasCboNombreProveedor.SelectedValue = _ProveedorAceptado;
                        ComprasCboNombreProveedor.Text = _TextoProveedorAceptado;
                        ComprasProcEstablecerError(ComprasCboNombreProveedor, string.Empty);
                        return;
                    }
                    bool Anterior = _ActualizandoDetalle;
                    _ActualizandoDetalle = true;
                    try
                    {
                        ComprasUsrProductos.Detalles.Clear();
                        ComprasUsrProductos.BodegaVisible = null;
                        _ProductoEditado = null;
                        ComprasUsrProductos.ComprasProcActualizarPestanasBodega();
                    }
                    finally { _ActualizandoDetalle = Anterior; }
                }
                _ProveedorAceptado = NuevoProveedor;
                _TextoProveedorAceptado = Proveedor == null ? string.Empty : Convert.ToString(Proveedor["nombreProveedor"]);
                IdProveedorSeleccionado = null;
                _SaldoProveedorBase = null;
                foreach (var Campo in new[] { ComprasTxtNitProveedor, ComprasTxtDireccionProveedor, ComprasTxtTelefonoProveedor,
                    ComprasTxtCorreoProveedor, ComprasTxtSaldoActualProveedor }) Campo.Text = string.Empty;
                ComprasLblEstadoProveedorValor.Texto = string.Empty;
                ComprasProcFiltrarProductosProveedor(Proveedor);
                ComprasProcSeleccionarPlazoProveedor(Proveedor);
                if (Proveedor != null)
                {
                    IdProveedorSeleccionado = NuevoProveedor;
                    ComprasTxtNitProveedor.Text = Convert.ToString(Proveedor["nitProveedor"]);
                    ComprasTxtDireccionProveedor.Text = Convert.ToString(Proveedor["direccionProveedor"]);
                    ComprasTxtTelefonoProveedor.Text = Convert.ToString(Proveedor["telefonoProveedor"]);
                    ComprasTxtCorreoProveedor.Text = Convert.ToString(Proveedor["correoProveedor"]);
                    _SaldoProveedorBase = Convert.ToDecimal(Proveedor["saldoActualProveedor"]);
                    bool Activo = Convert.ToBoolean(Proveedor["estadoProveedor"]);
                    ComprasLblEstadoProveedorValor.Texto = Activo ? "ACTIVO" : "INACTIVO";
                    ComprasLblEstadoProveedorValor.ColorTexto = Activo ? Color.DarkGreen : Color.Red;
                }
                ComprasProcActualizarImportesConvertidos();
            }
            finally
            {
                _ActualizandoProveedor = false;
                ComprasProcActualizarFlujo();
            }
        }

        private void ComprasProcFiltrarProductosProveedor(DataRow Proveedor)
        {
            if (_VistaProductosSku == null || _VistaProductosNombre == null) return;
            string Filtro = Proveedor == null ? "1 = 0" : "idProveedorProductos = "
                + Convert.ToInt32(Proveedor["idProveedor"]).ToString(CultureInfo.InvariantCulture);
            if (_VistaProductosSku.RowFilter == Filtro && _VistaProductosNombre.RowFilter == Filtro) return;
            bool Anterior = _ActualizandoProducto;
            _ActualizandoProducto = true;
            try
            {
                _Edicion.Establecer(ComprasCboSkuProducto, false);
                _Edicion.Establecer(ComprasCboNombreProducto, false);
                _VistaProductosSku.RowFilter = _VistaProductosNombre.RowFilter = Filtro;
                foreach (var Combo in new[] { ComprasCboSkuProducto, ComprasCboNombreProducto })
                {
                    Combo.SelectedIndex = -1;
                    Combo.Text = string.Empty;
                    ComprasProcEstablecerError(Combo, string.Empty);
                    _Edicion.Establecer(Combo, Proveedor != null && _CapturaDisponible);
                }
                ComprasProcLimpiarDatosProducto();
                _ProductoEditado = null;
                ComprasProcEstadoBotonesProducto();
            }
            finally { _ActualizandoProducto = Anterior; }
        }

        private void ComprasProcLimpiarDatosProducto()
        {
            bool Anterior = _ActualizandoDescuento;
            _ActualizandoDescuento = true;
            try
            {
                _CostoProductoBase = null;
                _DescuentoPorcentaje = 0M;
                _DescuentoValido = true;
                foreach (var Campo in new[] { ComprasTxtDescripcionProducto, ComprasTxtPesoProducto,
                    ComprasTxtCantidadProducto, ComprasTxtCostoUnitarioProducto, ComprasTxtDescuentoProducto,
                    ComprasTxtDescuentoPorcentajeProducto, ComprasTxtTotalProducto })
                {
                    Campo.Text = string.Empty;
                    ComprasProcEstablecerError(Campo, string.Empty);
                }
            }
            finally { _ActualizandoDescuento = Anterior; }
        }

        private void ComprasProcActualizarImportesConvertidos()
        {
            ComprasTxtSaldoActualProveedor.Text = string.Empty;
            ComprasTxtCostoUnitarioProducto.Text = string.Empty;
            if (TipoCambioSeleccionado.HasValue && TipoCambioSeleccionado.Value > 0)
            {
                if (_SaldoProveedorBase.HasValue)
                    ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSaldoActualProveedor, _SaldoProveedorBase.Value / TipoCambioSeleccionado.Value);
                if (_CostoProductoBase.HasValue)
                    ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtCostoUnitarioProducto, _CostoProductoBase.Value / TipoCambioSeleccionado.Value);
            }
            ComprasProcActualizarDescuentos();
            ComprasProcActualizarProductosAgregados();
        }

        private bool ComprasFuncLimitarDigitosPorcentaje(ComprasUsrTextBox Campo, decimal UltimoValido, ref bool Actualizando, int MaximoDigitos = 2)
        {
            string Texto = Campo.Text.Replace("%", string.Empty).Trim();
            string Separador = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string Enteros = Texto.Split(new[] { Separador }, StringSplitOptions.None)[0];
            if (Enteros.Length <= MaximoDigitos) return false;
            foreach (char Digito in Enteros) if (!char.IsDigit(Digito)) return false;
            Actualizando = true;
            try
            {
                string Numero = UltimoValido.ToString("0.##", CultureInfo.CurrentCulture);
                Campo.Text = Numero + " %";
                Campo.SelectionStart = Numero.Length;
            }
            finally { Actualizando = false; }
            return true;
        }

        private void ComprasProcCambiarDescuento()
        {
            if (_ActualizandoDescuento || _CargandoCatalogos || _Liberado) return;
            var Campo = ComprasTxtDescuentoPorcentajeProducto;
            if (ComprasFuncLimitarDigitosPorcentaje(Campo, _DescuentoPorcentaje, ref _ActualizandoDescuento))
            {
                ComprasProcCambiarDescuento();
                return;
            }
            string Texto = Campo.Text.Replace("%", string.Empty).Trim();
            if (_CostoProductoBase.HasValue)
            {
                int Posicion = Math.Min(Campo.SelectionStart, Texto.Length);
                _ActualizandoDescuento = true;
                try
                {
                    Campo.Text = Texto + " %";
                    Campo.SelectionStart = Posicion;
                }
                finally { _ActualizandoDescuento = false; }
            }
            decimal Valor = 0M;
            _DescuentoValido = Texto.Length == 0 || decimal.TryParse(Texto,
                NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out Valor);
            _DescuentoValido = _DescuentoValido && Valor >= 0M && Valor <= 99.99M && decimal.Round(Valor, 2) == Valor;
            if (!_DescuentoValido)
            {
                ComprasProcEstablecerError(Campo, "Ingresa un porcentaje entre 0 y 99.99 con hasta 2 decimales.");
                ComprasProcActualizarDescuentos(Campo);
                return;
            }
            _DescuentoPorcentaje = Valor;
            ComprasProcActualizarDescuentos(Campo);
        }

        private void ComprasProcActualizarDescuentos(ComprasUsrTextBox CampoEditado = null)
        {
            if (_ActualizandoDescuento || _CargandoCatalogos || _Liberado) return;
            if (!_CostoProductoBase.HasValue)
            {
                ComprasProcLimpiarDatosProducto();
                return;
            }
            _ActualizandoDescuento = true;
            try
            {
                ComprasTxtTotalProducto.Text = string.Empty;
                ComprasTxtDescuentoProducto.ComprasMetLimpiarError();
                ComprasTxtDescuentoPorcentajeProducto.ComprasMetLimpiarError();
                ComprasProcEstablecerError(ComprasTxtCantidadProducto, string.Empty);
                bool CantidadValida = int.TryParse(ComprasTxtCantidadProducto.Text, NumberStyles.None, CultureInfo.CurrentCulture, out int Cantidad)
                    && Cantidad > 0 && Cantidad <= 99999999;
                if (!CantidadValida && ComprasTxtCantidadProducto.Text.Length > 0)
                    ComprasProcEstablecerError(ComprasTxtCantidadProducto, "La cantidad debe ser un entero mayor que cero.");
                var Origen = ComprasTxtDescuentoPorcentajeProducto;
                var Destino = ComprasTxtDescuentoProducto;
                if (!_DescuentoValido || !_CostoProductoBase.HasValue || !TipoCambioSeleccionado.HasValue)
                {
                    Destino.Text = string.Empty;
                    return;
                }
                decimal Costo = decimal.Round(_CostoProductoBase.Value / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
                decimal Bruto = Costo;
                ComprasProcEstablecerError(ComprasTxtTotalProducto, string.Empty);
                if (Costo < 0M || Costo > 99999999.9999M || CantidadValida && Cantidad * Costo > 99999999.9999M)
                {
                    ComprasProcEstablecerError(ComprasTxtTotalProducto, "El importe del producto supera el máximo de 99,999,999.9999. Reduce la cantidad o el precio unitario.");
                    return;
                }
                decimal Importe = ClsOrdenesCompraControlador.ComprasFuncCalcularDescuento(Bruto, _DescuentoPorcentaje);
                if (Importe > Bruto)
                {
                    ComprasProcEstablecerError(Origen, "El descuento no puede superar el precio unitario.");
                    Origen.ComprasMetMostrarError("El descuento no puede superar el precio unitario.");
                    Destino.Text = string.Empty;
                    return;
                }
                ComprasProcEstablecerError(ComprasTxtDescuentoProducto, string.Empty);
                ComprasProcEstablecerError(ComprasTxtDescuentoPorcentajeProducto, string.Empty);
                if (CantidadValida)
                    ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalProducto,
                        ClsOrdenesCompraControlador.ComprasFuncCalcularTotalProducto(Cantidad, Costo, _DescuentoPorcentaje));
                ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtDescuentoProducto, Importe);
                if (CampoEditado != ComprasTxtDescuentoPorcentajeProducto)
                    ComprasTxtDescuentoPorcentajeProducto.Text = _DescuentoPorcentaje.ToString("0.##", CultureInfo.CurrentCulture) + " %";
            }
            finally
            {
                ComprasProcAjustarErrorDescuento();
                _ActualizandoDescuento = false;
            }
        }

        private void ComprasProcAjustarErrorDescuento()
        {
            foreach (var Campo in new[] { ComprasTxtDescuentoProducto, ComprasTxtDescuentoPorcentajeProducto })
            {
                string Mensaje = _Errores.GetError(Campo);
                if (string.IsNullOrWhiteSpace(Mensaje)) Campo.ComprasMetLimpiarError();
                else Campo.ComprasMetMostrarError(Mensaje);
                _FilasErrores.Establecer(Campo, !string.IsNullOrWhiteSpace(Mensaje));
            }
        }

        private void ComprasProcEstablecerError(Control Campo, string Mensaje)
        {
            _Errores.SetError(Campo, Mensaje);
            var Texto = Campo as ComprasUsrTextBox;
            var Combo = Campo as ComprasUsrComboBox;
            if (Texto != null)
            {
                if (string.IsNullOrWhiteSpace(Mensaje)) Texto.ComprasMetLimpiarError();
                else Texto.ComprasMetMostrarError(Mensaje);
            }
            if (Combo != null)
            {
                if (string.IsNullOrWhiteSpace(Mensaje)) Combo.ComprasMetLimpiarError();
                else Combo.ComprasMetMostrarError(Mensaje);
            }
            _FilasErrores.Establecer(Campo, !string.IsNullOrWhiteSpace(Mensaje));
        }

        private void ComprasProcReorganizarGrupos()
        {
            Point Posicion = ComprasPnlDesplazamiento.AutoScrollPosition;
            ComprasPnlContenido.SuspendLayout();
            try
            {
                ComprasGbxProveedor.Top = ComprasGbxPlazo.Top = Math.Max(315, ComprasGbxMoneda.Bottom + 22);
                ComprasGbxBodega.Top = ComprasUsrProductos.Top = Math.Max(559,
                    Math.Max(ComprasGbxProveedor.Bottom, ComprasGbxPlazo.Bottom) + 22);
                ComprasGbxProducto.Top = ComprasGbxBodega.Bottom + 16;
                ComprasUsrProductos.Height = Math.Max(772, ComprasGbxProducto.Bottom - ComprasUsrProductos.Top);
                ComprasGbxFinalizacion.Top = Math.Max(1328,
                    Math.Max(ComprasGbxProducto.Bottom, ComprasUsrProductos.Bottom) + 20);
                ComprasPnlContenido.Height = ComprasGbxFinalizacion.Bottom + 40;
            }
            finally
            {
                ComprasPnlContenido.ResumeLayout(true);
                ComprasPnlDesplazamiento.AutoScrollPosition = new Point(-Posicion.X, -Posicion.Y);
            }
        }

        private decimal _BrutoOrden;
        private decimal _DescuentoLineasOrden;

        private void ComprasProcCambiarImpuesto()
        {
            if (_ActualizandoImpuesto || _CargandoCatalogos || _Liberado) return;
            var Campo = ComprasTxtImpuestoPorcentaje;
            if (ComprasFuncLimitarDigitosPorcentaje(Campo, _ImpuestoPorcentaje, ref _ActualizandoImpuesto, 3))
            {
                ComprasProcCambiarImpuesto();
                return;
            }
            string Texto = Campo.Text.Replace("%", string.Empty).Trim();
            int Posicion = Math.Min(Campo.SelectionStart, Texto.Length);
            _ActualizandoImpuesto = true;
            try
            {
                Campo.Text = Texto + " %";
                Campo.SelectionStart = Posicion;
            }
            finally { _ActualizandoImpuesto = false; }
            decimal Valor = 0M;
            _ImpuestoValido = (Texto.Length == 0 || decimal.TryParse(Texto,
                NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out Valor))
                && Valor >= 0M && Valor <= 999.99M && decimal.Round(Valor, 2) == Valor;
            if (_ImpuestoValido) _ImpuestoPorcentaje = Valor;
            else ComprasProcEstablecerError(Campo, "Ingresa un impuesto entre 0 y 999.99 % con hasta 2 decimales.");
            ComprasProcActualizarImpuesto();
        }

        private void ComprasProcActualizarImpuesto()
        {
            if (_Liberado) return;
            _TotalesValidos = false;
            ComprasTxtImpuestoTotal.Text = string.Empty;
            ComprasTxtTotalOrden.Text = string.Empty;
            try
            {
                if (!_ImpuestoValido) return;
                decimal Neto = _BrutoOrden - _DescuentoLineasOrden;
                decimal Impuesto = ClsOrdenesCompraControlador.ComprasFuncCalcularImpuesto(Neto, _ImpuestoPorcentaje);
                ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtImpuestoTotal, Impuesto);
                ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalOrden, Neto + Impuesto);
                ComprasProcEstablecerError(ComprasTxtImpuestoPorcentaje, string.Empty);
                _TotalesValidos = true;
            }
            catch (ArgumentException Error)
            {
                ComprasProcEstablecerError(ComprasTxtImpuestoPorcentaje, Error.Message);
            }
            finally { ComprasProcActualizarFlujo(); }
        }

        private void ComprasProcActualizarDescuentoTotal()
        {
            if (!TipoCambioSeleccionado.HasValue || TipoCambioSeleccionado.Value <= 0M) return;
            decimal Porcentaje = _BrutoOrden == 0M ? 0M
                : decimal.Round(_DescuentoLineasOrden * 100M / _BrutoOrden, 2, MidpointRounding.AwayFromZero);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSubtotalOrden, _BrutoOrden);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtDescuentoTotal, _DescuentoLineasOrden);
            ComprasTxtDescuentoPorcentajeTotal.Text = Porcentaje.ToString("0.######", CultureInfo.CurrentCulture) + " %";
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSubtotalNeto, _BrutoOrden - _DescuentoLineasOrden);
            ComprasProcActualizarImpuesto();
        }

        private void ComprasProcActualizarSucursal()
        {
            if (_CargandoCatalogos || _Liberado || _VistaBodegas == null || _ActualizandoSucursal) return;
            var Sucursal = ComprasFuncFila(ComprasCboSucursal);
            string TextoSucursal = ComprasCboSucursal.Text;
            string Filtro = Sucursal == null ? "1 = 0" : "Fk_Sucursal_Codigo = "
                + Convert.ToInt32(Sucursal["idSucursal"]).ToString(CultureInfo.InvariantCulture);
            if (Sucursal != null && _Edicion.PuedeEditar(ComprasCboBodega) && _VistaBodegas.RowFilter == Filtro) return;
            _ActualizandoSucursal = true;
            try
            {
                _Edicion.Establecer(ComprasCboBodega, false);
                _VistaBodegas.RowFilter = Filtro;
                ComprasCboBodega.SelectedIndex = -1;
                ComprasCboBodega.Text = string.Empty;
                ComprasProcEstablecerError(ComprasCboBodega, string.Empty);
                ComprasProcActualizarBodega();
                _Edicion.Establecer(ComprasCboBodega, Sucursal != null && _CapturaDisponible);
                if (Sucursal == null)
                {
                    ComprasCboSucursal.SelectedIndex = -1;
                    ComprasCboSucursal.Text = TextoSucursal;
                    if (string.IsNullOrEmpty(TextoSucursal)) ComprasCboSucursal.SelectedIndex = -1;
                }
            }
            finally { _ActualizandoSucursal = false; }
        }

        private void ComprasProcActualizarBodega()
        {
            if (_CargandoCatalogos || _Liberado) return;
            var Bodega = ComprasFuncFila(ComprasCboBodega);
            ComprasTxtPaisBodega.Text = Bodega == null ? string.Empty : Convert.ToString(Bodega["paisBodega"]);
        }

        private void ComprasProcActualizarProducto(ComprasUsrComboBox Selector)
        {
            if (_CargandoCatalogos || _Liberado || _ActualizandoProducto) return;
            _ActualizandoProducto = true;
            try
            {
                var Producto = ComprasFuncFila(Selector);
                _CostoProductoBase = null;
                ComprasTxtDescripcionProducto.Text = string.Empty;
                ComprasTxtPesoProducto.Text = string.Empty;
                var Otro = Selector == ComprasCboSkuProducto ? ComprasCboNombreProducto : ComprasCboSkuProducto;
                if (Producto == null)
                {
                    Otro.SelectedIndex = -1;
                    Otro.Text = string.Empty;
                    ComprasProcLimpiarDatosProducto();
                    if (!_ActualizandoDetalle) _ProductoEditado = null;
                    ComprasProcEstadoBotonesProducto();
                }
                else
                {
                    var Anterior = ComprasFuncFila(Otro);
                    if (!_ActualizandoDetalle && (Anterior == null
                        || Convert.ToInt32(Anterior["idProducto"]) != Convert.ToInt32(Producto["idProducto"])))
                        ComprasProcLimpiarDatosProducto();
                    Otro.SelectedValue = Convert.ToInt32(Producto["idProducto"]);
                    ComprasTxtDescripcionProducto.Text = Convert.ToString(Producto["descripcionProducto"]);
                    ComprasTxtPesoProducto.Text = Convert.ToDecimal(Producto["pesoProducto"]).ToString("N2");
                    _CostoProductoBase = Convert.ToDecimal(Producto["precioCostoProducto"]);
                }
                ComprasProcActualizarImportesConvertidos();
            }
            finally { _ActualizandoProducto = false; }
        }

        private void ComprasProcSeleccionarPlazoProveedor(DataRow Proveedor)
        {
            if (Proveedor != null && !Proveedor.IsNull("idPlazo"))
            {
                ComprasCboPlazo.SelectedValue = Convert.ToInt32(Proveedor["idPlazo"]);
                var Plazo = ComprasFuncFila(ComprasCboPlazo);
                if (Plazo != null && Convert.ToInt32(Plazo["idPlazo"]) == Convert.ToInt32(Proveedor["idPlazo"]))
                {
                    ComprasProcActualizarVencimiento();
                    return;
                }
            }
            ComprasCboPlazo.SelectedIndex = -1;
            IdPlazoSeleccionado = null;
            FechaVencimiento = null;
            ComprasTxtFechaVencimiento.Text = string.Empty;
            ComprasProcEstablecerError(ComprasCboPlazo, Proveedor == null ? string.Empty : "El proveedor no tiene un plazo disponible. Selecciona un plazo.");
        }

        private void ComprasProcActualizarVencimiento()
        {
            if (_CargandoCatalogos || _Liberado) return;
            IdPlazoSeleccionado = null;
            FechaVencimiento = null;
            ComprasTxtFechaVencimiento.Text = string.Empty;
            ComprasProcEstablecerError(ComprasCboPlazo, string.Empty);
            var Plazo = ComprasFuncFila(ComprasCboPlazo);
            if (Plazo == null || _FechaEmision == DateTime.MinValue) return;
            try
            {
                IdPlazoSeleccionado = Convert.ToInt32(Plazo["idPlazo"]);
                FechaVencimiento = _Controlador.ComprasFuncCalcularVencimiento(_FechaEmision, Convert.ToInt32(Plazo["diasPlazo"]));
                ComprasTxtFechaVencimiento.Text = FechaVencimiento.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (ArgumentOutOfRangeException)
            {
                ComprasProcEstablecerError(ComprasCboPlazo, "El plazo no permite calcular una fecha de vencimiento valida.");
            }
        }

        private void ComprasProcLiberarCatalogos()
        {
            if (_Liberado) return;
            _Edicion?.Dispose();
            _Liberado = true;
            _VersionCambio++;
            foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo, ComprasCboSucursal, ComprasCboBodega, ComprasCboSkuProducto, ComprasCboNombreProducto })
                Combo.DataSource = null;
            ComprasUsrProductos.Tabla.DataSource = null;
            _VistaBodegas?.Dispose();
            _VistaProductosSku?.Dispose();
            _VistaProductosNombre?.Dispose();
            foreach (var Tabla in _Catalogos) Tabla.Dispose();
            _Catalogos.Clear();
        }
        private DataRow _ProductoEditado;
        private bool _ActualizandoDetalle;

        private void ComprasProcConfigurarProductos()
        {
            ComprasBtnIngresarProducto.Click += (Remitente, Evento) => ComprasProcGuardarProducto(false);
            ComprasBtnModificarProducto.Click += (Remitente, Evento) => ComprasProcGuardarProducto(true);
            ComprasBtnEliminarProducto.Click += (Remitente, Evento) => ComprasProcEliminarProducto();
            ComprasUsrProductos.ProductoSeleccionado += (Remitente, Evento) => ComprasProcSeleccionarProductoAgregado();
            ComprasProcEstadoBotonesProducto();
        }

        private void ComprasProcEstadoBotonesProducto()
        {
            bool Seleccionado = _ProductoEditado != null && _ProductoEditado.RowState != DataRowState.Detached;
            ComprasBtnModificarProducto.Enabled = Seleccionado && _CapturaDisponible;
            ComprasBtnEliminarProducto.Enabled = Seleccionado && _CapturaDisponible;
        }

        private void ComprasProcSeleccionarProductoAgregado()
        {
            if (_ActualizandoDetalle || _Liberado) return;
            _ProductoEditado = ComprasUsrProductos.Seleccionado;
            ComprasProcEstadoBotonesProducto();
            if (_ProductoEditado == null) return;
            _ActualizandoDetalle = true;
            try
            {
                DataRow Fila = _ProductoEditado;
                ComprasCboSucursal.SelectedValue = (int)Fila["idSucursal"];
                ComprasCboBodega.SelectedValue = (int)Fila["idBodega"];
                ComprasCboSkuProducto.SelectedValue = (int)Fila["idProducto"];
                var Producto = ComprasFuncFila(ComprasCboSkuProducto);
                if (Producto == null || Convert.ToInt32(Producto["idProducto"]) != (int)Fila["idProducto"])
                {
                    ComprasProcLimpiarDatosProducto();
                    return;
                }
                _CostoProductoBase = (decimal)Fila["costoBase"];
                ComprasTxtDescripcionProducto.Text = Convert.ToString(Fila["descripcion"]);
                ComprasTxtPesoProducto.Text = ((decimal)Fila["peso"]).ToString("N2");
                ComprasTxtCantidadProducto.Text = Convert.ToString(Fila["cantidad"], CultureInfo.CurrentCulture);
                _DescuentoPorcentaje = (decimal)Fila["porcentaje"];
                _DescuentoValido = true;
                ComprasProcActualizarImportesConvertidos();
            }
            finally { _ActualizandoDetalle = false; }
        }

        private void ComprasProcGuardarProducto(bool Modificar)
        {
            if (_CargandoCatalogos || _Liberado) return;
            var Producto = ComprasFuncFila(ComprasCboNombreProducto);
            var Bodega = ComprasFuncFila(ComprasCboBodega);
            ComprasProcEstablecerError(ComprasCboNombreProducto, Producto == null ? "Selecciona un producto." : string.Empty);
            ComprasProcEstablecerError(ComprasCboBodega, Bodega == null ? "Selecciona una bodega." : string.Empty);
            ComprasProcActualizarDescuentos();
            if (Producto == null || Bodega == null || string.IsNullOrWhiteSpace(ComprasTxtTotalProducto.Text)) return;
            if (Modificar && (_ProductoEditado == null || _ProductoEditado.RowState == DataRowState.Detached))
            {
                ComprasProcEstablecerError(ComprasUsrProductos.Tabla, "Selecciona el producto que deseas modificar.");
                return;
            }
            int Cantidad = int.Parse(ComprasTxtCantidadProducto.Text, CultureInfo.CurrentCulture);
            decimal Costo = decimal.Round(_CostoProductoBase.Value / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
            _ActualizandoDetalle = true;
            try
            {
            ComprasUsrProductos.Guardar(Modificar ? _ProductoEditado : null, Producto, Bodega, Cantidad,
                _CostoProductoBase.Value, TipoCambioSeleccionado.Value, Costo, _DescuentoPorcentaje,
                Convert.ToString(ComprasFuncFila(ComprasCboSucursal)?["nombreSucursal"]));
            _ProductoEditado = null;
            ComprasProcEstablecerError(ComprasUsrProductos.Tabla, string.Empty);
            ComprasUsrProductos.BodegaVisible = Convert.ToInt32(Bodega["idBodega"]);
            ComprasProcActualizarProductosAgregados();
            ComprasUsrProductos.ComprasProcActualizarPestanasBodega();
            }
            catch (ArgumentException Ex)
            {
                ComprasProcEstablecerError(ComprasTxtTotalProducto, Ex.Message);
                ComprasTxtTotalProducto.ComprasMetMostrarError(Ex.Message);
                ComprasProcMostrarResultado(Ex.Message, true);
                return;
            }
            finally { _ActualizandoDetalle = false; ComprasProcEstadoBotonesProducto(); }
            ComprasProcEstablecerError(ComprasTxtTotalProducto, string.Empty);
            ComprasTxtTotalProducto.ComprasMetLimpiarError();
            if (!Modificar) ComprasProcLimpiarCapturaProducto();
        }

        private void ComprasProcLimpiarCapturaProducto()
        {
            _ActualizandoProducto = true;
            _ActualizandoDescuento = true;
            try
            {
                ComprasCboSkuProducto.SelectedIndex = -1;
                ComprasCboNombreProducto.SelectedIndex = -1;
                ComprasCboSkuProducto.Text = string.Empty;
                ComprasCboNombreProducto.Text = string.Empty;
                ComprasCboSkuProducto.ComprasMetLimpiarError();
                ComprasCboNombreProducto.ComprasMetLimpiarError();
                _CostoProductoBase = null;
                _DescuentoPorcentaje = 0M;
                _DescuentoValido = true;
                foreach (var Campo in new[] { ComprasTxtDescripcionProducto, ComprasTxtPesoProducto,
                    ComprasTxtCantidadProducto, ComprasTxtCostoUnitarioProducto, ComprasTxtDescuentoProducto,
                    ComprasTxtDescuentoPorcentajeProducto, ComprasTxtTotalProducto })
                {
                    Campo.Text = string.Empty;
                    Campo.ComprasMetLimpiarError();
                    ComprasProcEstablecerError(Campo, string.Empty);
                }
                ComprasProcEstablecerError(ComprasCboSkuProducto, string.Empty);
                ComprasProcEstablecerError(ComprasCboNombreProducto, string.Empty);
            }
            finally
            {
                _ActualizandoProducto = false;
                _ActualizandoDescuento = false;
            }
            ComprasProcAjustarErrorDescuento();
            ComprasCboSkuProducto.Focus();
        }

        private void ComprasProcEliminarProducto()
        {
            if (_ProductoEditado == null) return;
            _ActualizandoDetalle = true;
            try
            {
                ComprasUsrProductos.Eliminar(_ProductoEditado);
                _ProductoEditado = null;
                ComprasProcActualizarProductosAgregados();
                ComprasUsrProductos.ComprasProcActualizarPestanasBodega();
            }
            finally { _ActualizandoDetalle = false; ComprasProcEstadoBotonesProducto(); }
        }

        private void ComprasProcActualizarFlujo()
        {
            if (_Liberado || IsDisposed || Disposing) return;
            ComprasGbxMoneda.Enabled = true;
            ComprasGbxProveedor.Enabled = true;
            ComprasGbxPlazo.Enabled = true;
            var Moneda = ComprasFuncFila(ComprasCboMoneda);
            var Proveedor = ComprasFuncFila(ComprasCboNombreProveedor);
            var Plazo = ComprasFuncFila(ComprasCboPlazo);
            bool EncabezadoValido = new ClsOrdenesCompraFlujo
            {
                CargandoCatalogos = _CargandoCatalogos, CatalogosCargados = _CatalogosCargados,
                FechaEmisionTexto = ComprasTxtFechaEmision.Text, FechaEmision = _FechaEmision,
                IdMonedaSeleccionada = IdMonedaSeleccionada, IdProveedorSeleccionado = IdProveedorSeleccionado,
                IdPlazoSeleccionado = IdPlazoSeleccionado, TipoCambioSeleccionado = TipoCambioSeleccionado,
                IdTipoCambioSeleccionado = IdTipoCambioSeleccionado, FechaVencimiento = FechaVencimiento
            }.EncabezadoValido(Moneda, Proveedor, Plazo);
            _CapturaDisponible = EncabezadoValido && !_Guardando && !OrdenGuardada;
            _Edicion.Aplicar(ComprasGbxMoneda, !_Guardando);
            _Edicion.Aplicar(ComprasGbxProveedor, !_Guardando);
            _Edicion.Aplicar(ComprasGbxPlazo, !_Guardando);
            foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo })
                _Edicion.Establecer(Combo, !_Guardando && _CatalogosCargados);
            _Edicion.Aplicar(ComprasGbxBodega, _CapturaDisponible);
            _Edicion.Establecer(ComprasCboBodega, _CapturaDisponible && ComprasFuncFila(ComprasCboSucursal) != null);
            _Edicion.Aplicar(ComprasGbxProducto, _CapturaDisponible);
            _Edicion.Establecer(ComprasCboSkuProducto, _CapturaDisponible && Proveedor != null);
            _Edicion.Establecer(ComprasCboNombreProducto, _CapturaDisponible && Proveedor != null);
            _Edicion.Establecer(ComprasUsrProductos, _CapturaDisponible);
            bool PuedeGuardar = _CapturaDisponible && ComprasUsrProductos.Detalles.Rows.Count > 0;
            _Edicion.Aplicar(ComprasGbxFinalizacion, PuedeGuardar);
            ComprasBtnGuardar.Enabled = PuedeGuardar && _ImpuestoValido && _TotalesValidos;
            ComprasBtnDescartar.Enabled = true;
            ComprasProcEstadoBotonesProducto();
        }

        private void ComprasProcActualizarProductosAgregados()
        {
            ComprasProcActualizarFlujo();
            if (_ActualizandoDetalle && _ProductoEditado != null) return;
            bool ActualizandoAntes = _ActualizandoDetalle;
            _ActualizandoDetalle = true;
            try
            {
                var Moneda = ComprasFuncFila(ComprasCboMoneda);
                ComprasUsrProductos.Recalcular(TipoCambioSeleccionado, Moneda == null ? string.Empty : Convert.ToString(Moneda["simboloMoneda"]));
                if (!TipoCambioSeleccionado.HasValue || TipoCambioSeleccionado.Value <= 0)
                {
                    foreach (var Campo in new[] { ComprasTxtSubtotalOrden, ComprasTxtDescuentoTotal, ComprasTxtDescuentoPorcentajeTotal, ComprasTxtSubtotalNeto, ComprasTxtImpuestoTotal, ComprasTxtTotalOrden }) Campo.Text = string.Empty;
                    return;
                }
                _BrutoOrden = ComprasUsrProductos.Bruto;
                _DescuentoLineasOrden = ComprasUsrProductos.Descuento;
                ComprasProcActualizarDescuentoTotal();
            }
            finally
            {
                if (!ActualizandoAntes)
                {
                    ComprasUsrProductos.Tabla.ClearSelection();
                    _ProductoEditado = null;
                    ComprasProcEstadoBotonesProducto();
                }
                _ActualizandoDetalle = ActualizandoAntes;
            }
        }
    }
}

