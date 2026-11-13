using System;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using PruebaUnitaria_OrdenesCompra.Apoyo;

class FrmImpuestoPrueba : FrmOrdenesCompra
{
    public FrmImpuestoPrueba(ClsOrdenesCompraControlador Controlador) : base(Controlador) { }
    protected override DialogResult ComprasFuncConfirmarAccion(string Mensaje, string Titulo) => DialogResult.Yes;
    protected override void ComprasProcMostrarResultado(string Mensaje, bool Error)
    {
        if (Error) throw new Exception(Mensaje);
    }
}

class ValidarImpuestoCaptura
{
    static T Campo<T>(object Formulario, string Nombre) where T : class
        => (T)typeof(FrmOrdenesCompra).GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Formulario);
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    static void Esperar(Func<bool> Condicion)
    {
        var Tiempo = Stopwatch.StartNew();
        while (!Condicion() && Tiempo.ElapsedMilliseconds < 10000) Application.DoEvents();
        Verificar(Condicion(), "La operación no terminó");
    }
    [STAThread]
    static int Main()
    {
        try { Ejecutar(); return 0; }
        catch (Exception Error) { Console.WriteLine(Error); return 1; }
    }
    static void Ejecutar()
    {
        using (var Repositorio = new ClsRepositorioOrdenesCompraPrueba())
        using (var Formulario = new FrmImpuestoPrueba(new ClsOrdenesCompraControlador(Repositorio)))
        {
            var Contenido = Campo<Panel>(Formulario, "ComprasPnlContenido"); Contenido.Parent = null; Contenido.CreateControl();
            var Porcentaje = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtImpuestoPorcentaje");
            var Importe = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtImpuestoTotal");
            Verificar(Porcentaje.Enabled && Porcentaje.ReadOnly && Importe.Enabled && Importe.ReadOnly, "Los campos no inician en solo lectura");
            var Tarea = (Task)typeof(FrmOrdenesCompra).GetMethod("ComprasProcPrepararAsync", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Formulario, new object[] { true });
            Esperar(() => Tarea.IsCompleted); Tarea.GetAwaiter().GetResult();
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProveedor").SelectedValue = 3;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSucursal").SelectedValue = 1;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboBodega").SelectedValue = 11;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto").SelectedValue = 1;
            var Cantidad = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto");
            Cantidad.Text = "2";
            Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoPorcentajeProducto").Text = "10";
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            Verificar(!Porcentaje.ReadOnly && Importe.ReadOnly, "El porcentaje debe ser la única entrada de impuesto");
            var DescuentoPorcentaje = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoPorcentajeProducto");
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto").SelectedValue = 1;
            DescuentoPorcentaje.Text = "25";
            DescuentoPorcentaje.Text = "100";
            Verificar(DescuentoPorcentaje.Text == "25 %", "Se permite ingresar tres dígitos en descuentos");
            DescuentoPorcentaje.Text = "99.99";
            Verificar(DescuentoPorcentaje.Text == "99.99 %", "No se acepta el límite de descuento");
            Porcentaje.Text = "100";
            Verificar(Porcentaje.Text == "100 %" && Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled,
                "Impuesto no acepta tres dígitos válidos");
            Porcentaje.Text = "999.99";
            Verificar(Porcentaje.Text == "999.99 %" && Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled,
                "Impuesto no acepta 999.99 cuando los importes caben");
            Porcentaje.Text = "1000";
            Verificar(Porcentaje.Text == "999.99 %", "Impuesto acepta cuatro dígitos");
            Porcentaje.Text = "12";
            Verificar(Porcentaje.Text == "12 %", "Falta la máscara porcentual");
            Verificar(Importe.ComprasFuncLeerMonto(out decimal Monto) && Monto == 16.63M, "No se calcula sobre el subtotal neto");
            var Total = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtTotalOrden");
            Verificar(Total.ComprasFuncLeerMonto(out decimal TotalMonto) && TotalMonto == 155.23M, "Total con impuesto incorrecto");
            var TablaTotales = (TableLayoutPanel)Porcentaje.Parent;
            int Filas = TablaTotales.RowCount;
            Porcentaje.Text = "12.345";
            Verificar(!Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled && !Porcentaje.ReadOnly, "No bloquea el impuesto inválido o impide corregirlo");
            Verificar(TablaTotales.GetRowSpan(Porcentaje) == 2 && TablaTotales.RowCount == Filas + 1, "No reserva la fila de error del impuesto");
            Porcentaje.Text = "12";
            Verificar(TablaTotales.GetRowSpan(Porcentaje) == 1 && TablaTotales.RowCount == Filas, "No restaura la fila del impuesto");
            Verificar(Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled, "No recupera Guardar");
            var Productos = Campo<UsrProductosOrdenesCompra>(Formulario, "ComprasUsrProductos");
            var Tabla = (ComprasUsrDataGridView)typeof(UsrProductosOrdenesCompra).GetProperty("Tabla", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Productos, null);
            Tabla.ClearSelection(); Tabla.Rows[0].Selected = true;
            Cantidad.Text = "3"; Campo<Button>(Formulario, "ComprasBtnModificarProducto").PerformClick();
            Verificar(Importe.ComprasFuncLeerMonto(out Monto) && Monto == 24.95M, "No recalcula el impuesto al modificar productos");
            var Detalles = (System.Data.DataTable)typeof(UsrProductosOrdenesCompra).GetProperty("Detalles", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Productos, null);
            decimal CostoOriginal = (decimal)Detalles.Rows[0]["costoBase"];
            var Recalcular = typeof(FrmOrdenesCompra).GetMethod("ComprasProcActualizarProductosAgregados", BindingFlags.NonPublic | BindingFlags.Instance);
            Detalles.Rows[0]["costoBase"] = 30000000M;
            Recalcular.Invoke(Formulario, null); Porcentaje.Text = "100";
            Verificar(!Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled && Total.Text == "", "Se permite guardar un total mayor que DECIMAL(12,4)");
            Detalles.Rows[0]["costoBase"] = 20000000M;
            Recalcular.Invoke(Formulario, null); Porcentaje.Text = "999.99";
            Verificar(!Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled && Importe.Text == "" && Repositorio.Creaciones == 0,
                "Se permite guardar un impuesto mayor que DECIMAL(12,4)");
            Detalles.Rows[0]["costoBase"] = CostoOriginal;
            Recalcular.Invoke(Formulario, null); Porcentaje.Text = "12";
            Verificar(Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled && !Porcentaje.ReadOnly, "No se recupera la captura al corregir el impuesto");
            var Grupo = Campo<GroupBox>(Formulario, "ComprasGbxFinalizacion");
            using (var Captura = new Bitmap(Grupo.Width, Grupo.Height))
            {
                Grupo.DrawToBitmap(Captura, new Rectangle(Point.Empty, Captura.Size)); Captura.Save("Validacion/ImpuestoSeccion6.png");
            }
            Campo<Button>(Formulario, "ComprasBtnGuardar").PerformClick();
            Esperar(() => Formulario.OrdenGuardada);
            Verificar(Repositorio.Creaciones == 1 && Repositorio.SubtotalCreado == 231M && Repositorio.DescuentoCreado == 23.1M,
                "El encabezado no recibe el descuento absoluto");
            Verificar(Repositorio.ImpuestoCreado == 24.948M, "El encabezado no recibe el impuesto absoluto con cuatro decimales");
            Contenido.Dispose();
        }
        Console.WriteLine("Impuesto en sección 6: máscara, solo lectura, cálculo neto, validación, recálculo y guardado absoluto verificados; descuento absoluto conservado.");
    }
}
