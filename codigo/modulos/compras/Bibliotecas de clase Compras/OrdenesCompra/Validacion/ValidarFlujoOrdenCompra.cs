using System;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using PruebaUnitaria_OrdenesCompra.Apoyo;

class ValidarFlujoOrdenCompra
{
    static T Campo<T>(FrmOrdenesCompra Formulario, string Nombre) where T : class
        => (T)typeof(FrmOrdenesCompra).GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Formulario);
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    static void Estado(FrmOrdenesCompra Formulario, bool Captura, bool Final)
    {
        foreach (string Nombre in new[] { "ComprasGbxMoneda", "ComprasGbxProveedor", "ComprasGbxPlazo" })
            Verificar(Campo<GroupBox>(Formulario, Nombre).Enabled, Nombre + " está deshabilitado");
        foreach (string Nombre in new[] { "ComprasGbxBodega", "ComprasGbxProducto", "ComprasGbxFinalizacion" })
            Verificar(Campo<GroupBox>(Formulario, Nombre).Enabled, Nombre + " no debe cambiar a enabled=false");
        Verificar(Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto").ReadOnly == !Captura, "Captura no cumple el flujo de solo lectura");
        Verificar(Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtComentario").ReadOnly == !Final, "Sección 6 no cumple el flujo de solo lectura");
        Verificar(Campo<UserControl>(Formulario, "ComprasUsrProductos").Enabled, "DGV no debe quedar disabled");
        Verificar(Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled == Final, "Guardar no cumple el flujo");
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
            Estado(Formulario, false, false);
            var Contenido = Campo<Panel>(Formulario, "ComprasPnlContenido");
            Contenido.Parent = null; Contenido.CreateControl();
            var Tarea = (Task)typeof(FrmOrdenesCompra).GetMethod("ComprasProcPrepararAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(Formulario, new object[] { true });
            var Tiempo = Stopwatch.StartNew();
            while (!Tarea.IsCompleted && Tiempo.ElapsedMilliseconds < 10000) Application.DoEvents();
            Verificar(Tarea.IsCompleted, "La preparación no terminó"); Tarea.GetAwaiter().GetResult();
            Estado(Formulario, false, false);
            var Proveedor = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProveedor");
            var Moneda = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboMoneda");
            var Plazo = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboPlazo");
            var Fecha = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtFechaEmision");
            Proveedor.SelectedValue = 3;
            Estado(Formulario, true, false);
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSucursal").SelectedValue = 1;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboBodega").SelectedValue = 11;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto").SelectedValue = 1;
            Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto").Text = "2";
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            Estado(Formulario, true, true);
            Plazo.Text = "Plazo inválido"; Estado(Formulario, false, false);
            Plazo.SelectedIndex = -1; Plazo.SelectedValue = 2; Estado(Formulario, true, true);
            string FechaOriginal = Fecha.Text;
            Fecha.Text = "fecha inválida"; Estado(Formulario, false, false);
            Fecha.Text = FechaOriginal; Estado(Formulario, true, true);
            Moneda.Text = "Moneda inválida"; Estado(Formulario, false, false);
            Moneda.SelectedIndex = -1; Moneda.SelectedValue = 10L; Estado(Formulario, true, true);
            Proveedor.Text = "Proveedor inválido"; Estado(Formulario, false, false);
            Proveedor.SelectedIndex = -1; Proveedor.SelectedValue = 3; Estado(Formulario, true, true);
            var Productos = Campo<UsrProductosOrdenesCompra>(Formulario, "ComprasUsrProductos");
            var Tabla = (ComprasUsrDataGridView)typeof(UsrProductosOrdenesCompra)
                .GetProperty("Tabla", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Productos, null);
            Tabla.ClearSelection(); Tabla.Rows[0].Selected = true;
            Campo<Button>(Formulario, "ComprasBtnEliminarProducto").PerformClick();
            Estado(Formulario, true, false);
            Contenido.Dispose();
        }
        Console.WriteLine("Flujo: estados iniciales, encabezado válido, primer producto, entradas inválidas, recuperación y eliminación del último producto correctos.");
    }
}



