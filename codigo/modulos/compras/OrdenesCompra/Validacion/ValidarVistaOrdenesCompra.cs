using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;

internal static class ValidarVistaOrdenesCompra
{
    private static int _Pruebas;
    private static T ComprasFuncCampo<T>(object Vista, string Nombre)
    {
        return (T)Vista.GetType().GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Vista);
    }
    private static void ComprasProcVerificar(bool Condicion, [System.Runtime.CompilerServices.CallerLineNumber] int Linea = 0)
    {
        if (!Condicion) throw new Exception("Fallo de vista en prueba " + (_Pruebas + 1) + ", linea " + Linea);
        _Pruebas++;
    }
    private static void ComprasProcVerificarDistribucion(Control Contenedor)
    {
        foreach (Control Campo in Contenedor.Controls)
        {
            ComprasProcVerificar(Contenedor.ClientRectangle.Contains(Campo.Bounds));
            foreach (Control Otro in Contenedor.Controls)
                if (!ReferenceEquals(Campo, Otro))
                    ComprasProcVerificar(!Campo.Bounds.IntersectsWith(Otro.Bounds));
            if (Campo is TableLayoutPanel)
                ComprasProcVerificarDistribucion(Campo);
        }
    }

    private static async Task ComprasProcEsperarAsync(Func<bool> Listo)
    {
        DateTime Limite = DateTime.UtcNow.AddSeconds(10);
        while (!Listo())
        {
            if (DateTime.UtcNow > Limite) throw new TimeoutException("La vista no termino la operacion.");
            await Task.Delay(20);
        }
    }

    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        using (DataTable Datos = ValidarOrdenesCompra.ComprasFuncDatos())
        using (var Vista = new FrmEncabezados(new ClsOrdenesCompra(new ValidarOrdenesCompra.RepositorioPrueba(Datos))))
        {
            var Controlador = ComprasFuncCampo<ClsOrdenesCompra>(Vista, "_Controlador");
            var Repositorio = (ValidarOrdenesCompra.RepositorioPrueba)typeof(ClsOrdenesCompra)
                .GetField("_Repositorio", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Controlador);
            Vista.Opacity = 0;
            Vista.ShowInTaskbar = false;
            Vista.Shown += async (Remitente, Evento) =>
            {
                try
                {
                    var Dgv = ComprasFuncCampo<ComprasUsrDataGridView>(Vista, "ComprasDgvEncabezados");
                    var Tabla = Dgv.Tabla;
                    var Estado = ComprasFuncCampo<ComprasUsrComboBox>(Vista, "ComprasCboEstado");
                    var Resultado = ComprasFuncCampo<ComprasUsrLabel>(Vista, "ComprasLblResultado");
                    await ComprasProcEsperarAsync(() => !ComprasFuncCampo<bool>(Vista, "_Cargando"));
                    ComprasProcVerificar(Tabla.Rows.Count == 5);
                    var BotonNueva = ComprasFuncCampo<ClsButtonCompras>(Vista, "ComprasBtnNuevaOrden");
                    var PanelScroll = ComprasFuncCampo<Panel>(Vista, "ComprasPnlDesplazamiento");
                    ComprasProcVerificar(PanelScroll.RectangleToScreen(PanelScroll.ClientRectangle).Contains(BotonNueva.RectangleToScreen(BotonNueva.ClientRectangle)));

                    ComprasProcVerificar(!Tabla.Columns.Contains("ComprasColNuevoEstado"));
                    ComprasProcVerificar(!Tabla.Columns.Contains("ComprasColCambiarEstado"));
                    ComprasProcVerificar(Tabla.ReadOnly);
                    ComprasProcVerificar(Tabla.Columns["ComprasColcodigoOrdenCompra"].DataPropertyName == "codigoOrdenCompra");
                    ComprasProcVerificar(Convert.ToString(Tabla.Rows[0].Cells["ComprasColcodigoOrdenCompra"].Value) == "OC-2026-001");
                    using (var Nueva = new FrmOrdenesCompra(Controlador))
                    {
                        Nueva.Opacity = 0;
                        Nueva.ShowInTaskbar = false;
                        foreach (string Nombre in new[] { "ComprasCboNombreProveedor", "ComprasCboMoneda", "ComprasCboPlazo", "ComprasCboSucursal", "ComprasCboBodega", "ComprasCboSkuProducto", "ComprasCboNombreProducto" })
                            ComprasFuncCampo<ComprasUsrComboBox>(Nueva, Nombre).TipoDatos = EnmTipoDatosCompras.Ninguno;
                        Nueva.Show(Vista);
                        await ComprasProcEsperarAsync(() => Nueva.CatalogosCargados);
                        var ScrollInicial = ComprasFuncCampo<ClsPanelVerticalCompras>(Nueva, "ComprasPnlDesplazamiento");
                        var EncabezadoInicial = ComprasFuncCampo<ComprasUsrEncabezado>(Nueva, "ComprasUsrEncabezado");
                        ComprasProcVerificar(ScrollInicial.AutoScrollPosition == Point.Empty && ScrollInicial.RectangleToScreen(ScrollInicial.ClientRectangle).Contains(EncabezadoInicial.RectangleToScreen(EncabezadoInicial.ClientRectangle)));

                        var MonedaNueva = ComprasFuncCampo<ComprasUsrComboBox>(Nueva, "ComprasCboMoneda");
                        var ProveedorNuevo = ComprasFuncCampo<ComprasUsrComboBox>(Nueva, "ComprasCboNombreProveedor");
                        var PlazoNuevo = ComprasFuncCampo<ComprasUsrComboBox>(Nueva, "ComprasCboPlazo");
                        ComprasProcVerificar(Nueva.IdMonedaSeleccionada == 10L && Nueva.TipoCambioSeleccionado == 1M && Nueva.IdTipoCambioSeleccionado == null);
                        MonedaNueva.SelectedValue = 20L;
                        await ComprasProcEsperarAsync(() => Nueva.TipoCambioSeleccionado.HasValue);
                        ComprasProcVerificar(Nueva.TipoCambioSeleccionado == 7.7M && Nueva.IdTipoCambioSeleccionado == 901L);
                        foreach (string Campo in new[] { "ComprasTxtSaldoActualProveedor", "ComprasTxtCostoUnitarioProducto", "ComprasTxtDescuentoProducto", "ComprasTxtSubtotalOrden", "ComprasTxtDescuentoTotal", "ComprasTxtSubtotalNeto", "ComprasTxtImpuestoTotal", "ComprasTxtTotalOrden" })
                            ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, Campo).TipoMoneda == "USD");
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtSaldoActualProveedor").TipoMoneda == "USD");

                        var Porcentaje = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtDescuentoPorcentajeProducto");
                        Porcentaje.Text = "12.50";
                        MonedaNueva.SelectedValue = 30L;
                        await ComprasProcEsperarAsync(() => Nueva.TipoCambioSeleccionado == 8.4M);
                        ComprasProcVerificar(Nueva.IdTipoCambioSeleccionado == 902L);
                        ComprasProcVerificar(Porcentaje.TipoEntrada == ComprasEnumTipoEntrada.Porcentaje && !Porcentaje.ModoMoneda && Porcentaje.Text.Contains("12.5"));
                        Porcentaje.Text = "101";
                        ComprasProcVerificar(!Porcentaje.Text.Contains("101"));
                        var Cantidad = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtCantidadProducto");
                        Cantidad.Text = "2";
                        Cantidad.Text = "2.5000";
                        ComprasProcVerificar(Cantidad.Text == "2" && Cantidad.SoloEnteros && !Cantidad.ModoMoneda);

                        MonedaNueva.SelectedValue = 40L;
                        await ComprasProcEsperarAsync(() => ComprasFuncCampo<ErrorProvider>(Nueva, "_Errores").GetError(MonedaNueva).Length > 0);
                        ComprasProcVerificar(Nueva.TipoCambioSeleccionado == null && Nueva.IdTipoCambioSeleccionado == null);
                        MonedaNueva.SelectedValue = 20L;
                        MonedaNueva.SelectedValue = 10L;
                        await Task.Delay(100);
                        ComprasProcVerificar(Nueva.TipoCambioSeleccionado == 1M && Nueva.IdTipoCambioSeleccionado == null);
                        ProveedorNuevo.SelectedValue = 3;
                        ComprasProcVerificar(Nueva.IdPlazoSeleccionado == 2 && Nueva.FechaVencimiento == DateTime.Today.AddDays(30));
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtNitProveedor").Text == "1234567890");
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtTelefonoProveedor").Text == "5555-1111");
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtCorreoProveedor").Text == "uno@example.com");
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtDireccionProveedor").Text == "Direccion Uno");
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtSaldoActualProveedor").Text.StartsWith("Q ") && ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtSaldoActualProveedor").Text.Contains("125"));
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrLabel>(Nueva, "ComprasLblEstadoProveedorValor").Texto == "ACTIVO");
                        var SucursalNueva = ComprasFuncCampo<ComprasUsrComboBox>(Nueva, "ComprasCboSucursal");
                        var BodegaNueva = ComprasFuncCampo<ComprasUsrComboBox>(Nueva, "ComprasCboBodega");
                        SucursalNueva.SelectedValue = 1;
                        BodegaNueva.SelectedValue = 11;
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtPaisBodega").Text == "Guatemala");
                        SucursalNueva.SelectedValue = 2;
                        ComprasProcVerificar(BodegaNueva.Items.Count == 1);
                        BodegaNueva.SelectedValue = 12;
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtPaisBodega").Text == "Mexico");
                        var SkuNuevo = ComprasFuncCampo<ComprasUsrComboBox>(Nueva, "ComprasCboSkuProducto");
                        var ProductoNuevo = ComprasFuncCampo<ComprasUsrComboBox>(Nueva, "ComprasCboNombreProducto");
                        SkuNuevo.SelectedValue = 1;
                        ComprasProcVerificar(Convert.ToInt32(ProductoNuevo.SelectedValue) == 1);
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtDescripcionProducto").Text == "Descripcion Uno");
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtPesoProducto").Text.Contains("1.50"));
                        var CostoNuevo = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtCostoUnitarioProducto");
                        var SaldoNuevo = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtSaldoActualProveedor");
                        ComprasProcVerificar(!CostoNuevo.Enabled && CostoNuevo.Text == "Q 77.00");
                        MonedaNueva.SelectedValue = 20L;
                        await ComprasProcEsperarAsync(() => Nueva.TipoCambioSeleccionado == 7.7M);
                        ComprasProcVerificar(SaldoNuevo.Text == "$ 16.30" && CostoNuevo.Text == "$ 10.00");
                        MonedaNueva.SelectedValue = 40L;
                        ComprasProcVerificar(SaldoNuevo.Text.Length == 0 && CostoNuevo.Text.Length == 0);
                        MonedaNueva.SelectedValue = 10L;
                        ComprasProcVerificar(SaldoNuevo.Text == "Q 125.50" && CostoNuevo.Text == "Q 77.00");
                        ProductoNuevo.SelectedValue = 2;
                        ComprasProcVerificar(Convert.ToInt32(SkuNuevo.SelectedValue) == 2 && CostoNuevo.Text == "Q 154.00");
                        SkuNuevo.SelectedValue = 1;
                        Cantidad.Text = "2";
                        var DescuentoImporte = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtDescuentoProducto");
                        var TotalProducto = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtTotalProducto");
                        ComprasProcVerificar(!TotalProducto.Enabled && TotalProducto.ReadOnly);
                        DescuentoImporte.Text = "15.40";
                        ComprasProcVerificar(TotalProducto.Text == "Q 123.20");
                        ComprasProcVerificar(Porcentaje.Text == "20 %");
                        Porcentaje.Text = "25";
                        ComprasProcVerificar(DescuentoImporte.Text == "Q 19.25");
                        Cantidad.Text = "4";
                        ComprasProcVerificar(TotalProducto.Text == "Q 231.00");
                        ComprasProcVerificar(DescuentoImporte.Text == "Q 19.25" && Porcentaje.Text == "25 %");
                        DescuentoImporte.Text = "38.50";
                        ComprasProcVerificar(Porcentaje.Text == "50 %");
                        Cantidad.Text = "2";
                        ComprasProcVerificar(Porcentaje.Text == "50 %");
                        MonedaNueva.SelectedValue = 20L;
                        await ComprasProcEsperarAsync(() => Nueva.TipoCambioSeleccionado == 7.7M);
                        ComprasProcVerificar(DescuentoImporte.Text == "$ 5.00" && Porcentaje.Text == "50 %");
                        ComprasProcVerificar(TotalProducto.Text == "$ 10.00");
                        DescuentoImporte.Text = "5";
                        ComprasProcVerificar(Porcentaje.Text == "50 %");
                        MonedaNueva.SelectedValue = 10L;
                        ComprasProcVerificar(DescuentoImporte.Text == "Q 38.50");
                        Cantidad.Text = "0";
                        ComprasProcVerificar(Porcentaje.Text == "50 %");
                        Cantidad.Text = "2";
                        var DescripcionProducto = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtDescripcionProducto");
                        int AlturaDescripcionNormal = DescripcionProducto.Height;
                        int AlturaDescuentoNormal = DescuentoImporte.Height;
                        DescuentoImporte.Text = "200";
                        ComprasProcVerificar(DescripcionProducto.Height < AlturaDescripcionNormal && DescuentoImporte.Height > AlturaDescuentoNormal);
                        ComprasProcVerificar(DescuentoImporte.MostrarEtiquetaError && ComprasFuncCampo<ComprasUsrLabel>(DescuentoImporte, "ComprasLblError").Visible);
                        ComprasProcVerificar(Porcentaje.Text.Length == 0 && ComprasFuncCampo<ErrorProvider>(Nueva, "_Errores").GetError(DescuentoImporte).Length > 0);
                        ComprasProcVerificar(ComprasFuncCampo<Panel>(DescuentoImporte, "ComprasPnlBorde").BackColor == Color.Red);
                        ComprasProcVerificar(TotalProducto.Text.Length == 0);
                        Porcentaje.Text = "100";
                        ComprasProcVerificar(TotalProducto.Text == "Q 0.00");
                        ComprasProcVerificar(DescripcionProducto.Height == AlturaDescripcionNormal && DescuentoImporte.Height == AlturaDescuentoNormal && !DescuentoImporte.MostrarEtiquetaError);
                        ComprasProcVerificar(DescuentoImporte.Text == "Q 77.00");
                        ComprasProcVerificar(ComprasFuncCampo<Panel>(DescuentoImporte, "ComprasPnlBorde").BackColor != Color.Red);
                        Porcentaje.Text = "0";
                        ComprasProcVerificar(DescuentoImporte.Text == "Q 0.00");

                        SucursalNueva.SelectedValue = 1;
                        BodegaNueva.SelectedValue = 11;
                        Cantidad.Text = "2";
                        Porcentaje.Text = "10";
                        var InsertarProducto = ComprasFuncCampo<Button>(Nueva, "ComprasBtnInsertarProducto");
                        var GridProductos = ComprasFuncCampo<ComprasUsrDataGridView>(Nueva, "ComprasDgvProductos");
                        var PestanasBodega = ComprasFuncCampo<FlowLayoutPanel>(Nueva, "ComprasFlpAccionesProductos");
                        ComprasProcVerificar(!ComprasFuncCampo<Button>(Nueva, "ComprasBtnModificarProducto").Enabled && !ComprasFuncCampo<Button>(Nueva, "ComprasBtnEliminarProducto").Enabled);
                        InsertarProducto.PerformClick();
                        ComprasProcVerificar(GridProductos.Rows.Count == 1 && GridProductos.Columns.Count == 8 && PestanasBodega.Controls.Count == 1);
                        ComprasProcVerificar(Convert.ToDecimal(((DataRowView)GridProductos.Rows[0].DataBoundItem)["total"]) == 138.60M);
                        SucursalNueva.SelectedValue = 2;
                        BodegaNueva.SelectedValue = 12;
                        InsertarProducto.PerformClick();
                        ComprasProcVerificar(PestanasBodega.Controls[0] is ClsButtonCompras);
                        ComprasProcVerificar(PestanasBodega.Controls.Count == 2 && ComprasFuncCampo<int?>(Nueva, "_BodegaVisible") == 12);
                        ComprasProcVerificar(GridProductos.Rows.Count == 1 && Convert.ToInt32(((DataRowView)GridProductos.Rows[0].DataBoundItem)["idBodega"]) == 12);
                        var AnteriorBodega = ComprasFuncCampo<ClsButtonCompras>(Nueva, "ComprasBtnBodegaAnterior");
                        var SiguienteBodega = ComprasFuncCampo<ClsButtonCompras>(Nueva, "ComprasBtnBodegaSiguiente");
                        ComprasProcVerificar(AnteriorBodega.Enabled && !SiguienteBodega.Enabled);
                        AnteriorBodega.PerformClick();
                        var PestanaInactiva = (ClsButtonCompras)PestanasBodega.Controls[1];
                        ComprasProcVerificar(PestanaInactiva.BackColor == ClsTemaCompras.FondoBotones && PestanaInactiva.BackColor.A == 255);
                        ComprasProcVerificar(PestanaInactiva.FlatAppearance.MouseOverBackColor == PestanaInactiva.BackColor && PestanaInactiva.FlatAppearance.MouseDownBackColor == PestanaInactiva.BackColor);
                        ComprasProcVerificar(!AnteriorBodega.Enabled && SiguienteBodega.Enabled);
                        SiguienteBodega.PerformClick();
                        ComprasProcVerificar(ComprasFuncCampo<int?>(Nueva, "_BodegaVisible") == 12);
                        ComprasProcVerificar(PestanasBodega.Controls[0].BackColor == ClsTemaCompras.FondoBotones);
                        AnteriorBodega.PerformClick();
                        ComprasProcVerificar(ComprasFuncCampo<int?>(Nueva, "_BodegaVisible") == 11 && GridProductos.Rows.Count == 1);
                        InsertarProducto.PerformClick();
                        ComprasProcVerificar(ComprasFuncCampo<int?>(Nueva, "_BodegaVisible") == 12 && GridProductos.Rows.Count == 2);
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtTotalOrden").Text == "Q 415.80");
                        MonedaNueva.SelectedValue = 20L;
                        await ComprasProcEsperarAsync(() => Nueva.TipoCambioSeleccionado == 7.7M);
                        ComprasProcVerificar(Convert.ToDecimal(((DataRowView)GridProductos.Rows[0].DataBoundItem)["total"]) == 18M);
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtTotalOrden").Text == "$ 54.00");
                        GridProductos.Rows[0].Selected = true;
                        ComprasProcVerificar(ComprasFuncCampo<Button>(Nueva, "ComprasBtnModificarProducto").Enabled && ComprasFuncCampo<Button>(Nueva, "ComprasBtnEliminarProducto").Enabled);
                        ComprasProcVerificar(Convert.ToInt32(BodegaNueva.SelectedValue) == 12 && Convert.ToInt32(SucursalNueva.SelectedValue) == 2 && Cantidad.Text == "2" && TotalProducto.Text == "$ 18.00");
                        Cantidad.Text = "3";
                        ComprasFuncCampo<Button>(Nueva, "ComprasBtnModificarProducto").PerformClick();
                        ComprasProcVerificar(GridProductos.Rows.Count == 2 && ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtTotalOrden").Text == "$ 63.00");
                        ComprasProcVerificar(!ComprasFuncCampo<Button>(Nueva, "ComprasBtnModificarProducto").Enabled && !ComprasFuncCampo<Button>(Nueva, "ComprasBtnEliminarProducto").Enabled);
                        GridProductos.Rows[0].Selected = true;
                        ComprasFuncCampo<Button>(Nueva, "ComprasBtnEliminarProducto").PerformClick();
                        ComprasProcVerificar(GridProductos.Rows.Count == 1 && ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtTotalOrden").Text == "$ 36.00");
                        var ImporteOrden = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtDescuentoTotal");
                        var PorcentajeOrden = ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtDescuentoPorcentajeTotal");
                        ComprasProcVerificar(!ImporteOrden.Enabled && ImporteOrden.ReadOnly && !PorcentajeOrden.Enabled && PorcentajeOrden.ReadOnly);
                        ComprasProcVerificar(ImporteOrden.Text == "$ 4.00" && PorcentajeOrden.Text == "10 %");
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtTotalOrden").Text == "$ 36.00");
                        MonedaNueva.SelectedValue = 10L;
                        ComprasProcVerificar(ImporteOrden.Text == "Q 30.80" && PorcentajeOrden.Text == "10 %");
                        PlazoNuevo.SelectedValue = 2;
                        foreach (string Grupo in new[] { "ComprasGbxMoneda", "ComprasGbxProveedor", "ComprasGbxPlazo" })
                        {
                            var Seccion = ComprasFuncCampo<GroupBox>(Nueva, Grupo);
                            using (var ImagenPaso = new Bitmap(Seccion.Width, Seccion.Height))
                            {
                                Seccion.DrawToBitmap(ImagenPaso, new Rectangle(Point.Empty, Seccion.Size));
                                ImagenPaso.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Grupo + ".png"), ImageFormat.Png);
                            }
                        }
                        ProveedorNuevo.SelectedValue = 4;
                        ComprasProcVerificar(Nueva.IdPlazoSeleccionado == 1 && Nueva.FechaVencimiento == DateTime.Today);
                        ComprasProcVerificar(ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtNitProveedor").Text == "0987654321");
                        ProveedorNuevo.SelectedValue = 5;
                        ComprasProcVerificar(Nueva.IdPlazoSeleccionado == null && Nueva.FechaVencimiento == null && PlazoNuevo.SelectedIndex == -1);
                        ProveedorNuevo.DataSource = null;
                        ComprasProcVerificar(Nueva.IdProveedorSeleccionado == null && ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtTelefonoProveedor").Text.Length == 0);
                        PlazoNuevo.SelectedValue = 2;
                        ComprasProcVerificar(Nueva.FechaVencimiento == DateTime.Today.AddDays(30));
                        PlazoNuevo.SelectedValue = 1;
                        ComprasProcVerificar(Nueva.FechaVencimiento == DateTime.Today);
                        ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtFechaEmision").Text = "28/02/2028";
                        PlazoNuevo.SelectedValue = 2;
                        ComprasProcVerificar(Nueva.FechaVencimiento == new DateTime(2028, 3, 29));
                        ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtFechaEmision").Text = "fecha invalida";
                        ComprasProcVerificar(Nueva.FechaVencimiento == null && Nueva.TipoCambioSeleccionado == null);
                        ComprasFuncCampo<ComprasUsrTextBox>(Nueva, "ComprasTxtFechaEmision").Text = DateTime.Today.ToString("dd/MM/yyyy");
                        PlazoNuevo.DataSource = null;
                        ComprasProcVerificar(Nueva.FechaVencimiento == null);
                        var Canvas = ComprasFuncCampo<Panel>(Nueva, "ComprasPnlContenido");
                        var Encabezado = ComprasFuncCampo<ComprasUsrEncabezado>(Nueva, "ComprasUsrEncabezado");
                        ComprasProcVerificar(Nueva.Controls.Count == 1 && Canvas.Controls.Contains(Encabezado));
                        var Desplazamiento = ComprasFuncCampo<ClsPanelVerticalCompras>(Nueva, "ComprasPnlDesplazamiento");
                        ComprasProcVerificar(Desplazamiento.Top == 0 && Canvas.Parent == Desplazamiento && Encabezado.Parent == Canvas);
                        ComprasProcVerificar(Desplazamiento.VerticalScroll.Visible && !Desplazamiento.HorizontalScroll.Visible);
                        Desplazamiento.AutoScrollPosition = Point.Empty;
                        int PosicionEncabezado = Encabezado.PointToScreen(Point.Empty).Y;
                        Desplazamiento.AutoScrollPosition = new Point(0, 200);
                        ComprasProcVerificar(Desplazamiento.AutoScrollPosition.Y < 0 && Encabezado.PointToScreen(Point.Empty).Y < PosicionEncabezado - Encabezado.Height);
                        using (var ImagenScroll = new Bitmap(Nueva.Width, Nueva.Height))
                        {
                            Nueva.DrawToBitmap(ImagenScroll, new Rectangle(Point.Empty, Nueva.Size));
                            ImagenScroll.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VistaOrdenesCompraDesplazada.png"), ImageFormat.Png);
                        }
                        Desplazamiento.AutoScrollPosition = Point.Empty;
                        using (var Imagen = new Bitmap(Nueva.ClientSize.Width, Nueva.ClientSize.Height))
                        {
                            Nueva.DrawToBitmap(Imagen, new Rectangle(Point.Empty, Nueva.ClientSize));
                            Imagen.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VistaOrdenesCompraCanvas.png"), ImageFormat.Png);
                        }
                    }
                    object DisenoDetalles;
                    using (var Detalle = new FrmDetalleOrdenCompra(Controlador.ComprasFuncObtener(1), Controlador))
                    {
                        var Codigo = ComprasFuncCampo<ComprasUsrTextBox>(Detalle, "ComprasTxtCodigoOrden");
                        ComprasProcVerificar(Codigo.Text == "1" && !Codigo.Enabled && Codigo.MaxLength == 30);
                        Detalle.Opacity = 0;
                        Detalle.ShowInTaskbar = false;
                        Detalle.Show(Vista);
                        await ComprasProcEsperarAsync(() => ComprasFuncCampo<ComprasUsrDataGridView>(Detalle, "ComprasDgvProductos").DataSource != null);
                        ComprasFuncCampo<ComprasUsrDataGridView>(Detalle, "ComprasDgvProductos").DataSource = null;
                        Detalle.CreateControl();
                        Detalle.PerformLayout();
                        var Finalizacion = ComprasFuncCampo<GroupBox>(Detalle, "ComprasGbxFinalizacion");
                        ComprasProcVerificarDistribucion(Finalizacion);
                        using (var Imagen = new Bitmap(Finalizacion.Width, Finalizacion.Height))
                        {
                            Finalizacion.DrawToBitmap(Imagen, new Rectangle(Point.Empty, Finalizacion.Size));
                            Imagen.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VistaFinalizacion.png"), ImageFormat.Png);
                        }
                        var ContenidoDetalle = ComprasFuncCampo<Panel>(Detalle, "ComprasPnlContenido");
                        using (var Imagen = new Bitmap(ContenidoDetalle.Width, ContenidoDetalle.Height))
                        {
                            ContenidoDetalle.DrawToBitmap(Imagen, new Rectangle(Point.Empty, ContenidoDetalle.Size));
                            Imagen.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VistaDetalleOrdenCompra.png"), ImageFormat.Png);
                        }
                        var AnchoFinalizacion = Finalizacion.Width;
                        Finalizacion.Dock = DockStyle.None;
                        Finalizacion.Width = 1000;
                        Finalizacion.PerformLayout();
                        ComprasProcVerificarDistribucion(Finalizacion);
                        using (var Imagen = new Bitmap(Finalizacion.Width, Finalizacion.Height))
                        {
                            Finalizacion.DrawToBitmap(Imagen, new Rectangle(Point.Empty, Finalizacion.Size));
                            Imagen.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VistaFinalizacion1000.png"), ImageFormat.Png);
                        }
                        Finalizacion.Width = AnchoFinalizacion;
                        Finalizacion.Dock = DockStyle.Fill;
                        DisenoDetalles = ComprasFuncCampo<object>(Detalle, "_DisenoDetalles");
                        var Productos = ComprasFuncCampo<ComprasUsrDataGridView>(Detalle, "ComprasDgvProductos").Tabla;
                        var Distribucion = ComprasFuncCampo<TableLayoutPanel>(Detalle, "ComprasTlpDetalles");
                        ComprasProcVerificar(Productos.Columns.Count == 6 && Productos.ReadOnly);
                        Productos.Rows.Add(3);
                        float AlturaConFilas = Distribucion.RowStyles[1].Height;
                        ComprasProcVerificar(Productos.Rows.Count == 3 && Productos.Rows[0].MinimumHeight == 40);
                        Productos.Rows.Clear();
                        ComprasProcVerificar(Distribucion.RowStyles[1].Height < AlturaConFilas);
                    }
                    ComprasProcVerificar(ComprasFuncCampo<bool>(DisenoDetalles, "_Liberado"));
                    Estado.Focus();
                    PanelScroll.AutoScrollPosition = new Point(0, 50);
                    Point ScrollAntes = PanelScroll.AutoScrollPosition;
                    Estado.SelectedIndex = 1;
                    await ComprasProcEsperarAsync(() => !ComprasFuncCampo<bool>(Vista, "_Cargando"));
                    ComprasProcVerificar(Repositorio.EstadoConsultado == null && Tabla.Rows.Count == 1);
                    ComprasProcVerificar(PanelScroll.AutoScrollPosition == ScrollAntes && Estado.ContainsFocus);
                    var Buscar = ComprasFuncCampo<ComprasUsrTextBox>(Vista, "ComprasTxtBuscar");
                    Buscar.Focus();
                    Point ScrollBusqueda = PanelScroll.AutoScrollPosition;
                    Buscar.Text = "BUSQUEDA-SIN-COINCIDENCIAS";
                    ComprasProcVerificar(Tabla.Rows.Count == 0 && PanelScroll.AutoScrollPosition == ScrollBusqueda);
                    var ErrorBusqueda = ComprasFuncCampo<ComprasUsrLabel>(Buscar, "ComprasLblError");
                    ComprasProcVerificar(Buscar.MostrarEtiquetaError && ErrorBusqueda.Visible && ErrorBusqueda.Texto.Length > 0);
                    ComprasProcVerificar(Buscar.ClientRectangle.Contains(ErrorBusqueda.Bounds.Location) && Buscar.Height <= Buscar.Parent.Height);
                    var FiltrosCaptura = ComprasFuncCampo<GroupBox>(Vista, "ComprasGbxFiltros");
                    using (var ImagenError = new Bitmap(FiltrosCaptura.Width, FiltrosCaptura.Height))
                    {
                        FiltrosCaptura.DrawToBitmap(ImagenError, new Rectangle(Point.Empty, FiltrosCaptura.Size));
                        ImagenError.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VistaBusquedaSinResultados.png"), ImageFormat.Png);
                    }
                    Buscar.Text = string.Empty;
                    ComprasProcVerificar(ErrorBusqueda.Texto.Length == 0 && Tabla.Rows.Count == 1);

                    Estado.SelectedIndex = 2;
                    await ComprasProcEsperarAsync(() => !ComprasFuncCampo<bool>(Vista, "_Cargando"));
                    ComprasProcVerificar(Tabla.Rows.Count == 1 && Repositorio.EstadoConsultado == null);
                    Repositorio.RechazarConsulta = true;
                    ComprasFuncCampo<Button>(Vista, "ComprasBtnActualizar").PerformClick();
                    await ComprasProcEsperarAsync(() => !ComprasFuncCampo<bool>(Vista, "_Cargando"));
                    ComprasProcVerificar(Tabla.Rows.Count == 0 && Resultado.Texto.Contains("Consulta rechazada"));
                    Repositorio.RechazarConsulta = false;
                    ComprasFuncCampo<Button>(Vista, "ComprasBtnActualizar").PerformClick();
                    await ComprasProcEsperarAsync(() => !ComprasFuncCampo<bool>(Vista, "_Cargando"));
                    ComprasProcVerificar(Tabla.Rows.Count == 5);
                    var Contenido = ComprasFuncCampo<Panel>(Vista, "ComprasPnlContenido");
                    using (var Captura = new Bitmap(Contenido.Width, Contenido.Height))
                    {
                        Contenido.DrawToBitmap(Captura, new Rectangle(Point.Empty, Contenido.Size));
                        Captura.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VistaOrdenesCompra.png"), ImageFormat.Png);
                    }
                    Console.WriteLine(_Pruebas + " pruebas de vista correctas.");
                }
                catch (Exception Error)
                {
                    Console.Error.WriteLine(Error);
                    Environment.ExitCode = 1;
                }
                finally { Vista.Close(); }
            };
            Application.Run(Vista);
        }
    }
}

