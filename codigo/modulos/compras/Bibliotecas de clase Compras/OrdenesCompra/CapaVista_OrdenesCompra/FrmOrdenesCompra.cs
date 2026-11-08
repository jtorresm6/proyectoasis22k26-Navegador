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
        private readonly ClsOrdenesCompra _Controlador;
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
        private bool _ActualizandoProducto;
        private bool _ActualizandoDescuento;
        private bool _DescuentoDesdePorcentaje = true;
        private decimal _DescuentoPorcentaje;
        private decimal _DescuentoImporteBase;
        private bool _DescuentoValido = true;
        private float[] _AlturasProducto;
        private int _AlturaDescripcion;
        private int _AlturaDescuento;
        private int _AlturaPorcentaje;
        private DateTime _FechaEmision = DateTime.Today;
        public bool CatalogosCargados => _CatalogosCargados;
        public long? IdMonedaSeleccionada { get; private set; }
        public long? IdTipoCambioSeleccionado { get; private set; }
        public decimal? TipoCambioSeleccionado { get; private set; }
        public int? IdProveedorSeleccionado { get; private set; }
        public int? IdPlazoSeleccionado { get; private set; }
        public DateTime? FechaVencimiento { get; private set; }

        public FrmOrdenesCompra() : this(new ClsOrdenesCompra()) { }

        public FrmOrdenesCompra(ClsOrdenesCompra Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
            ComprasTxtDescripcionProducto.Entrada.ScrollBars = RichTextBoxScrollBars.Vertical;
            ComprasProcAlinearEtiquetas(ComprasPnlContenido);
            _AlturasProducto = new float[ComprasTlpProducto.RowStyles.Count];
            for (int Indice = 0; Indice < _AlturasProducto.Length; Indice++)
                _AlturasProducto[Indice] = ComprasTlpProducto.RowStyles[Indice].Height;
            _AlturaDescripcion = ComprasTxtDescripcionProducto.Height;
            _AlturaDescuento = ComprasTxtDescuentoProducto.Height;
            _AlturaPorcentaje = ComprasTxtDescuentoPorcentajeProducto.Height;
            _Componentes = new System.ComponentModel.Container();
            _Errores = new ErrorProvider(_Componentes) { ContainerControl = this, BlinkStyle = ErrorBlinkStyle.NeverBlink };
            foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo, ComprasCboSucursal, ComprasCboBodega, ComprasCboSkuProducto, ComprasCboNombreProducto })
            {
                Combo.TipoDatos = EnmTipoDatosCompras.Ninguno;
                Combo.PermitirEscritura = true;
                Combo.Enabled = false;
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
            ComprasCboNombreProveedor.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarProveedor();
            ComprasCboPlazo.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarVencimiento();
            ComprasCboSucursal.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarSucursal();
            ComprasCboBodega.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarBodega();
            ComprasCboSkuProducto.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarProducto(ComprasCboSkuProducto);
            ComprasCboNombreProducto.SelectedIndexChanged += (Remitente, Evento) => ComprasProcActualizarProducto(ComprasCboNombreProducto);
            ComprasTxtCantidadProducto.TextChanged += (Remitente, Evento) => ComprasProcActualizarDescuentos();
            ComprasTxtDescuentoProducto.TextChanged += (Remitente, Evento) => ComprasProcCambiarDescuento(false);
            ComprasTxtDescuentoPorcentajeProducto.TextChanged += (Remitente, Evento) => ComprasProcCambiarDescuento(true);
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
                    _Errores.SetError(ComprasTxtFechaEmision, "Usa una fecha valida con formato dd/MM/yyyy.");
                    return;
                }
                _Errores.SetError(ComprasTxtFechaEmision, string.Empty);
                ComprasProcActualizarVencimiento();
                await ComprasProcActualizarCambioAsync();
            };
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
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
                _Guardando = true;
                ComprasPnlContenido.Enabled = false;
                UseWaitCursor = true;
                try
                {
                    int Numero = await Task.Run(() => _Controlador.ComprasFuncCrearConDetalles(Proveedor, Plazo, Moneda,
                        Emision, Vencimiento, Cambio, IdCambio, Comentario, Detalles));
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
                    ComprasPnlContenido.Enabled = true;
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
                ComprasProcVincular(ComprasCboSkuProducto, Datos[5], "idProducto", "skuProducto");
                ComprasProcVincular(ComprasCboNombreProducto, Datos[5], "idProducto", "nombreProducto");
                _CargandoCatalogos = false;
                foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo, ComprasCboSucursal, ComprasCboBodega, ComprasCboSkuProducto, ComprasCboNombreProducto })
                    Combo.Enabled = true;
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
                ComprasCboMoneda.Focus();
                ComprasPnlDesplazamiento.AutoScrollPosition = Point.Empty;
            }
            catch (Exception Ex)
            {
                if (_Liberado || IsDisposed || Disposing) return;
                _CargandoCatalogos = false;
                if (PropagarError) throw;
                foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo, ComprasCboSucursal, ComprasCboBodega, ComprasCboSkuProducto, ComprasCboNombreProducto })
                    _Errores.SetError(Combo, "No se pudieron cargar los catalogos. Cierra y vuelve a abrir la orden. " + Ex.Message);
                ComprasPnlDesplazamiento.AutoScrollPosition = Point.Empty;
            }
        }

        private static void ComprasProcVincular(ComprasUsrComboBox Combo, object Datos, string Valor, string Texto)
        {
            Combo.BeginUpdate();
            try
            {
                Combo.ValueMember = Valor;
                Combo.DisplayMember = Texto;
                Combo.DataSource = Datos;
                Combo.SelectedIndex = -1;
            }
            finally { Combo.EndUpdate(); }
        }

        private void ComprasProcConfigurarValidacionCatalogo(ComprasUsrComboBox Combo)
        {
            bool ErrorVisible = false;
            bool Pendiente = false;
            float EspacioTomado = 0F;
            Action Validar = () =>
            {
                if (_Liberado || _CargandoCatalogos || _ActualizandoProducto || Combo.IsDisposed) return;
                bool Invalido = !string.IsNullOrWhiteSpace(Combo.Text) && Combo.FindStringExact(Combo.Text) < 0;
                if (Invalido != ErrorVisible)
                {
                    var Tabla = Combo.Parent as TableLayoutPanel;
                    if (Tabla != null)
                    {
                        int Fila = Tabla.GetRow(Combo);
                        int Reserva = Tabla == ComprasTlpProducto ? (Combo == ComprasCboSkuProducto ? 3 : 4) : Tabla.RowStyles.Count - 1;
                        float Cambio = Invalido ? 30F : -30F;
                        if (Invalido) EspacioTomado = Math.Min(30F, Tabla.RowStyles[Reserva].Height);
                        float ReservaCambio = Invalido ? EspacioTomado : -EspacioTomado;
                        Tabla.RowStyles[Fila].Height += Cambio;
                        Tabla.RowStyles[Reserva].Height -= ReservaCambio;
                        if (Tabla == ComprasTlpProducto)
                        {
                            _AlturasProducto[Fila] += Cambio;
                            _AlturasProducto[Reserva] -= ReservaCambio;
                            _AlturaDescripcion -= (int)ReservaCambio;
                            ComprasProcAjustarErrorDescuento();
                        }
                    }
                    Combo.MostrarEtiquetaError = Invalido;
                    ErrorVisible = Invalido;
                }
                if (Invalido) Combo.ComprasMetMostrarError("Selecciona una opción del catálogo.");
                else Combo.ComprasMetLimpiarError();
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
            var Fila = Combo.SelectedItem as DataRowView;
            return Combo.SelectedIndex < 0 || Fila == null || Fila.Row.RowState == DataRowState.Deleted
                || !string.Equals(Combo.Text, Combo.GetItemText(Combo.SelectedItem), StringComparison.CurrentCultureIgnoreCase) ? null : Fila.Row;
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
            _Errores.SetError(ComprasCboMoneda, string.Empty);
            var Moneda = ComprasFuncFila(ComprasCboMoneda);
            try { ComprasProcActualizarCamposMoneda(Moneda); }
            catch (ArgumentException Ex)
            {
                _Errores.SetError(ComprasCboMoneda, Ex.Message);
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
                        _Errores.SetError(ComprasCboMoneda, "No existe una cotizacion hacia GTQ para la fecha de emision o una fecha anterior.");
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
                    _Errores.SetError(ComprasCboMoneda, "No se pudo consultar el tipo de cambio. " + Ex.Message);
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

        private void ComprasProcActualizarProveedor()
        {
            if (_CargandoCatalogos || _Liberado) return;
            IdProveedorSeleccionado = null;
            _SaldoProveedorBase = null;
            foreach (var Campo in new[] { ComprasTxtNitProveedor, ComprasTxtDireccionProveedor, ComprasTxtTelefonoProveedor,
                ComprasTxtCorreoProveedor, ComprasTxtSaldoActualProveedor }) Campo.Text = string.Empty;
            ComprasLblEstadoProveedorValor.Texto = string.Empty;
            var Proveedor = ComprasFuncFila(ComprasCboNombreProveedor);
            ComprasProcSeleccionarPlazoProveedor(Proveedor);
            if (Proveedor == null) return;
            IdProveedorSeleccionado = Convert.ToInt32(Proveedor["idProveedor"]);
            ComprasTxtNitProveedor.Text = Convert.ToString(Proveedor["nitProveedor"]);
            ComprasTxtDireccionProveedor.Text = Convert.ToString(Proveedor["direccionProveedor"]);
            ComprasTxtTelefonoProveedor.Text = Convert.ToString(Proveedor["telefonoProveedor"]);
            ComprasTxtCorreoProveedor.Text = Convert.ToString(Proveedor["correoProveedor"]);
            _SaldoProveedorBase = Convert.ToDecimal(Proveedor["saldoActualProveedor"]);
            ComprasProcActualizarImportesConvertidos();
            bool Activo = Convert.ToBoolean(Proveedor["estadoProveedor"]);
            ComprasLblEstadoProveedorValor.Texto = Activo ? "ACTIVO" : "INACTIVO";
            ComprasLblEstadoProveedorValor.ColorTexto = Activo ? Color.DarkGreen : Color.Red;
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

        private void ComprasProcCambiarDescuento(bool DesdePorcentaje)
        {
            if (_ActualizandoDescuento || _CargandoCatalogos || _Liberado) return;
            var Campo = DesdePorcentaje ? ComprasTxtDescuentoPorcentajeProducto : ComprasTxtDescuentoProducto;
            string Texto = Campo.Text.Replace("%", string.Empty).Trim();
            var Moneda = ComprasFuncFila(ComprasCboMoneda);
            if (!DesdePorcentaje && Moneda != null)
                Texto = Texto.Replace(Convert.ToString(Moneda["simboloMoneda"]), string.Empty).Trim();
            _Errores.SetError(DesdePorcentaje ? ComprasTxtDescuentoProducto : ComprasTxtDescuentoPorcentajeProducto, string.Empty);
            _DescuentoDesdePorcentaje = DesdePorcentaje;
            _DescuentoValido = Texto.Length == 0 || decimal.TryParse(Texto, NumberStyles.Number, CultureInfo.CurrentCulture, out _);
            decimal Valor = Texto.Length == 0 || !_DescuentoValido ? 0M : decimal.Parse(Texto, CultureInfo.CurrentCulture);
            _DescuentoValido = _DescuentoValido && Valor >= 0 && (DesdePorcentaje ? Valor <= 100 : Valor <= 99999999.9999M);
            if (!_DescuentoValido)
            {
                _Errores.SetError(Campo, DesdePorcentaje ? "Ingresa un porcentaje entre 0 y 100." : "Ingresa un importe de descuento valido.");
                ComprasProcActualizarDescuentos(Campo);
                return;
            }
            if (DesdePorcentaje) _DescuentoPorcentaje = Valor;
            else _DescuentoImporteBase = Valor * (TipoCambioSeleccionado ?? 1M);
            ComprasProcActualizarDescuentos(Campo);
        }

        private void ComprasProcActualizarDescuentos(ComprasUsrTextBox CampoEditado = null)
        {
            if (_ActualizandoDescuento || _CargandoCatalogos || _Liberado) return;
            _ActualizandoDescuento = true;
            try
            {
                ComprasTxtTotalProducto.Text = string.Empty;
                ComprasTxtDescuentoProducto.ComprasMetLimpiarError();
                ComprasTxtDescuentoPorcentajeProducto.ComprasMetLimpiarError();
                _Errores.SetError(ComprasTxtCantidadProducto, string.Empty);
                bool CantidadValida = int.TryParse(ComprasTxtCantidadProducto.Text, NumberStyles.None, CultureInfo.CurrentCulture, out int Cantidad)
                    && Cantidad > 0 && Cantidad <= 99999999;
                if (!CantidadValida && ComprasTxtCantidadProducto.Text.Length > 0)
                    _Errores.SetError(ComprasTxtCantidadProducto, "La cantidad debe ser un entero mayor que cero.");
                var Origen = _DescuentoDesdePorcentaje ? ComprasTxtDescuentoPorcentajeProducto : ComprasTxtDescuentoProducto;
                var Destino = _DescuentoDesdePorcentaje ? ComprasTxtDescuentoProducto : ComprasTxtDescuentoPorcentajeProducto;
                if (!_DescuentoValido || !_CostoProductoBase.HasValue || !TipoCambioSeleccionado.HasValue)
                {
                    Destino.Text = string.Empty;
                    return;
                }
                decimal Costo = decimal.Round(_CostoProductoBase.Value / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
                decimal Bruto = Costo;
                decimal Importe = _DescuentoDesdePorcentaje
                    ? decimal.Round(Bruto * _DescuentoPorcentaje / 100M, 2, MidpointRounding.AwayFromZero)
                    : decimal.Round(_DescuentoImporteBase / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
                if (Importe > Bruto)
                {
                    _Errores.SetError(Origen, "El descuento no puede superar el precio unitario.");
                    Origen.ComprasMetMostrarError("El descuento no puede superar el precio unitario.");
                    Destino.Text = string.Empty;
                    return;
                }
                _Errores.SetError(ComprasTxtDescuentoProducto, string.Empty);
                _Errores.SetError(ComprasTxtDescuentoPorcentajeProducto, string.Empty);
                if (CantidadValida)
                    ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalProducto,
                        decimal.Round(Cantidad * (Costo - Importe), 2, MidpointRounding.AwayFromZero));
                decimal Porcentaje = _DescuentoDesdePorcentaje ? _DescuentoPorcentaje
                    : Bruto == 0M ? 0M : decimal.Round(Importe * 100M / Bruto, 2, MidpointRounding.AwayFromZero);
                if (CampoEditado != ComprasTxtDescuentoProducto)
                    ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtDescuentoProducto, Importe);
                if (CampoEditado != ComprasTxtDescuentoPorcentajeProducto)
                    ComprasTxtDescuentoPorcentajeProducto.Text = Porcentaje.ToString("0.##", CultureInfo.CurrentCulture) + " %";
            }
            finally
            {
                ComprasProcAjustarErrorDescuento();
                _ActualizandoDescuento = false;
            }
        }

        private void ComprasProcAjustarErrorDescuento()
        {
            var Campo = _DescuentoDesdePorcentaje ? ComprasTxtDescuentoPorcentajeProducto : ComprasTxtDescuentoProducto;
            string Mensaje = _Errores.GetError(Campo);
            bool Mostrar = !string.IsNullOrWhiteSpace(Mensaje);
            ComprasTlpProducto.SuspendLayout();
            try
            {
                ComprasTxtDescuentoProducto.MostrarEtiquetaError = Mostrar && Campo == ComprasTxtDescuentoProducto;
                ComprasTxtDescuentoPorcentajeProducto.MostrarEtiquetaError = Mostrar && Campo == ComprasTxtDescuentoPorcentajeProducto;
                for (int Indice = 0; Indice < _AlturasProducto.Length; Indice++)
                    ComprasTlpProducto.RowStyles[Indice].Height = _AlturasProducto[Indice];
                if (Mostrar)
                {
                    ComprasTlpProducto.RowStyles[3].Height = Math.Max(0F, _AlturasProducto[3] - 25F);
                    ComprasTlpProducto.RowStyles[4].Height = Math.Max(0F, _AlturasProducto[4] - 25F);
                    ComprasTlpProducto.RowStyles[Campo == ComprasTxtDescuentoProducto ? 7 : 8].Height += 50F;
                    Campo.AlturaEtiquetaError = 44;
                    Campo.ComprasMetMostrarError(Mensaje);
                }
                int EspacioDescuento = Mostrar ? (int)(Math.Min(25F, _AlturasProducto[3]) + Math.Min(25F, _AlturasProducto[4])) : 0;
                ComprasTxtDescripcionProducto.Height = _AlturaDescripcion - EspacioDescuento;
                ComprasTxtDescuentoProducto.Height = _AlturaDescuento + (Mostrar && Campo == ComprasTxtDescuentoProducto ? 50 : 0);
                ComprasTxtDescuentoPorcentajeProducto.Height = _AlturaPorcentaje + (Mostrar && Campo == ComprasTxtDescuentoPorcentajeProducto ? 50 : 0);
            }
            finally { ComprasTlpProducto.ResumeLayout(true); }
        }

        private decimal _BrutoOrden;
        private decimal _DescuentoLineasOrden;

        private void ComprasProcActualizarDescuentoTotal()
        {
            if (!TipoCambioSeleccionado.HasValue || TipoCambioSeleccionado.Value <= 0M) return;
            decimal Porcentaje = _BrutoOrden == 0M ? 0M
                : decimal.Round(_DescuentoLineasOrden * 100M / _BrutoOrden, 2, MidpointRounding.AwayFromZero);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSubtotalOrden, _BrutoOrden);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtDescuentoTotal, _DescuentoLineasOrden);
            ComprasTxtDescuentoPorcentajeTotal.Text = Porcentaje.ToString("0.##", CultureInfo.CurrentCulture) + " %";
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtSubtotalNeto, _BrutoOrden - _DescuentoLineasOrden);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtImpuestoTotal, 0M);
            ClsPresentacionCompras.ComprasProcMostrarMonto(ComprasTxtTotalOrden, _BrutoOrden - _DescuentoLineasOrden);
        }

        private void ComprasProcActualizarSucursal()
        {
            if (_CargandoCatalogos || _Liberado || _VistaBodegas == null) return;
            var Sucursal = ComprasFuncFila(ComprasCboSucursal);
            _VistaBodegas.RowFilter = Sucursal == null ? "1 = 0" : "Fk_Sucursal_Codigo = " + Convert.ToInt32(Sucursal["idSucursal"]).ToString(CultureInfo.InvariantCulture);
            ComprasCboBodega.SelectedIndex = -1;
            ComprasProcActualizarBodega();
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
                if (Producto == null) Otro.SelectedIndex = -1;
                else
                {
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
            _Errores.SetError(ComprasCboPlazo, Proveedor == null ? string.Empty : "El proveedor no tiene un plazo disponible. Selecciona un plazo.");
        }

        private void ComprasProcActualizarVencimiento()
        {
            if (_CargandoCatalogos || _Liberado) return;
            IdPlazoSeleccionado = null;
            FechaVencimiento = null;
            ComprasTxtFechaVencimiento.Text = string.Empty;
            _Errores.SetError(ComprasCboPlazo, string.Empty);
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
                _Errores.SetError(ComprasCboPlazo, "El plazo no permite calcular una fecha de vencimiento valida.");
            }
        }

        private void ComprasProcLiberarCatalogos()
        {
            if (_Liberado) return;
            _Liberado = true;
            _VersionCambio++;
            foreach (var Combo in new[] { ComprasCboMoneda, ComprasCboNombreProveedor, ComprasCboPlazo, ComprasCboSucursal, ComprasCboBodega, ComprasCboSkuProducto, ComprasCboNombreProducto })
                Combo.DataSource = null;
            ComprasUsrProductos.Tabla.DataSource = null;
            _VistaBodegas?.Dispose();
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
            ComprasBtnModificarProducto.Enabled = Seleccionado;
            ComprasBtnEliminarProducto.Enabled = Seleccionado;
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
                _CostoProductoBase = (decimal)Fila["costoBase"];
                ComprasTxtDescripcionProducto.Text = Convert.ToString(Fila["descripcion"]);
                ComprasTxtPesoProducto.Text = ((decimal)Fila["peso"]).ToString("N2");
                ComprasTxtCantidadProducto.Text = Convert.ToString(Fila["cantidad"], CultureInfo.CurrentCulture);
                _DescuentoDesdePorcentaje = false;
                _DescuentoImporteBase = (decimal)Fila["descuentoBase"];
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
            _Errores.SetError(ComprasCboNombreProducto, Producto == null ? "Selecciona un producto." : string.Empty);
            _Errores.SetError(ComprasCboBodega, Bodega == null ? "Selecciona una bodega." : string.Empty);
            ComprasProcActualizarDescuentos();
            if (Producto == null || Bodega == null || string.IsNullOrWhiteSpace(ComprasTxtTotalProducto.Text)) return;
            if (Modificar && (_ProductoEditado == null || _ProductoEditado.RowState == DataRowState.Detached))
            {
                _Errores.SetError(ComprasUsrProductos.Tabla, "Selecciona el producto que deseas modificar.");
                return;
            }
            int Cantidad = int.Parse(ComprasTxtCantidadProducto.Text, CultureInfo.CurrentCulture);
            decimal Costo = decimal.Round(_CostoProductoBase.Value / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
            decimal Descuento = _DescuentoDesdePorcentaje
                ? decimal.Round(Costo * _DescuentoPorcentaje / 100M, 2, MidpointRounding.AwayFromZero)
                : decimal.Round(_DescuentoImporteBase / TipoCambioSeleccionado.Value, 2, MidpointRounding.AwayFromZero);
            _ActualizandoDetalle = true;
            try
            {
            ComprasUsrProductos.Guardar(Modificar ? _ProductoEditado : null, Producto, Bodega, Cantidad,
                _CostoProductoBase.Value, Descuento, TipoCambioSeleccionado.Value, Costo);
            _ProductoEditado = null;
            _Errores.SetError(ComprasUsrProductos.Tabla, string.Empty);
            ComprasUsrProductos.BodegaVisible = Convert.ToInt32(Bodega["idBodega"]);
            ComprasProcActualizarProductosAgregados();
            ComprasUsrProductos.ComprasProcActualizarPestanasBodega();
            }
            finally { _ActualizandoDetalle = false; ComprasProcEstadoBotonesProducto(); }
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
                _DescuentoDesdePorcentaje = true;
                _DescuentoPorcentaje = 0M;
                _DescuentoImporteBase = 0M;
                _DescuentoValido = true;
                foreach (var Campo in new[] { ComprasTxtDescripcionProducto, ComprasTxtPesoProducto,
                    ComprasTxtCantidadProducto, ComprasTxtCostoUnitarioProducto, ComprasTxtDescuentoProducto,
                    ComprasTxtDescuentoPorcentajeProducto, ComprasTxtTotalProducto })
                {
                    Campo.Text = string.Empty;
                    Campo.ComprasMetLimpiarError();
                    _Errores.SetError(Campo, string.Empty);
                }
                _Errores.SetError(ComprasCboSkuProducto, string.Empty);
                _Errores.SetError(ComprasCboNombreProducto, string.Empty);
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

        private void ComprasProcActualizarProductosAgregados()
        {
            ComprasBtnGuardar.Enabled = !_Guardando && ComprasUsrProductos.Detalles.Rows.Count > 0;
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
