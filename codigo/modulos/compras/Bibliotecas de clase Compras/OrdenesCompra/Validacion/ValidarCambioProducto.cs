using System;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using PruebaUnitaria_OrdenesCompra.Apoyo;

class ValidarCambioProducto
{
    static T Campo<T>(FrmOrdenesCompra Formulario, string Nombre) where T : class
        => (T)typeof(FrmOrdenesCompra).GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Formulario);
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
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
        using (var Formulario = new FrmOrdenesCompra(new ClsOrdenesCompraControlador(Repositorio)))
        {
            var Contenido = Campo<Panel>(Formulario, "ComprasPnlContenido"); Contenido.Parent = null; Contenido.CreateControl();
            var Tarea = (Task)typeof(FrmOrdenesCompra).GetMethod("ComprasProcPrepararAsync", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Formulario, new object[] { true });
            var Tiempo = Stopwatch.StartNew();
            while (!Tarea.IsCompleted && Tiempo.ElapsedMilliseconds < 10000) Application.DoEvents();
            Verificar(Tarea.IsCompleted, "La preparación no terminó"); Tarea.GetAwaiter().GetResult();
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProveedor").SelectedValue = 3;
            var Sku = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto");
            var Nombre = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProducto");
            var Catalogo = ((DataView)Sku.DataSource).Table;
            var Nuevo = Catalogo.NewRow(); Nuevo.ItemArray = Catalogo.Rows[0].ItemArray;
            Nuevo["idProducto"] = 3; Nuevo["skuProducto"] = "SKU-003"; Nuevo["nombreProducto"] = "Producto Tres";
            Nuevo["descripcionProducto"] = "Descripción nueva"; Nuevo["precioCostoProducto"] = 99M; Catalogo.Rows.Add(Nuevo);
            var Cantidad = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto");
            var Porcentaje = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoPorcentajeProducto");
            var Total = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtTotalProducto");
            Sku.SelectedIndex = -1; Sku.SelectedValue = 1; Cantidad.Text = "7"; Porcentaje.Text = "15";
            Verificar(Total.Text != "", "No se calculó el producto inicial");
            Sku.SelectedValue = 3;
            Verificar(Cantidad.Text == "" && Total.Text == "" && Porcentaje.Text == "0 %", "Cambiar SKU conserva captura anterior");
            Verificar(Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescripcionProducto").Text == "Descripción nueva", "No se cargó el producto nuevo");
            Cantidad.Text = "5"; Porcentaje.Text = "20";
            Nombre.SelectedValue = 1;
            Verificar(Cantidad.Text == "" && Total.Text == "" && Porcentaje.Text == "0 %", "Cambiar nombre conserva captura anterior");
            Cantidad.Text = "2"; Porcentaje.Text = "10";
            typeof(FrmOrdenesCompra).GetMethod("ComprasProcActualizarProducto", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Formulario, new object[] { Sku });
            Verificar(Cantidad.Text == "2" && Porcentaje.Text == "10 %", "Se limpia sin cambiar el producto");
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSucursal").SelectedValue = 1;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboBodega").SelectedValue = 11;
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            var Productos = Campo<UsrProductosOrdenesCompra>(Formulario, "ComprasUsrProductos");
            var Tabla = (ComprasUsrDataGridView)typeof(UsrProductosOrdenesCompra).GetProperty("Tabla", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Productos, null);
            Tabla.ClearSelection(); Tabla.Rows[0].Selected = true;
            Verificar(Cantidad.Text == "2" && Porcentaje.Text == "10 %", "Editar una línea no recupera su cantidad y descuento");
            Contenido.Dispose();
        }
        Console.WriteLine("Cambio de producto por SKU/nombre limpia captura; seleccionar el mismo producto y editar una línea conservan sus datos.");
    }
}

