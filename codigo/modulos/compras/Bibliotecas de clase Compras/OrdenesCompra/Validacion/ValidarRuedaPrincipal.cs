using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using PruebaUnitaria_OrdenesCompra.Apoyo;

class ValidarRuedaPrincipal
{
    static T Campo<T>(object Objeto, Type Tipo, string Nombre) where T : class
        => (T)Tipo.GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Objeto);
    static T Campo<T>(FrmOrdenesCompra Formulario, string Nombre) where T : class => Campo<T>(Formulario, typeof(FrmOrdenesCompra), Nombre);
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    static void Rueda(FrmOrdenesCompra Formulario, ScrollableControl Principal, UsrProductosOrdenesCompra Productos, ComprasUsrDataGridView Tabla)
    {
        Principal.AutoScrollPosition = new Point(0, 300);
        var Handle = Tabla.Tabla.Handle;
        int Antes = Principal.AutoScrollPosition.Y;
        int Fila = Tabla.Tabla.FirstDisplayedScrollingRowIndex;
        int Pagina = (int)typeof(ComprasUsrDataGridView).GetField("_PaginaActual", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Tabla);
        var Mensaje = Message.Create(Tabla.Tabla.Handle, 0x020A, (IntPtr)(-120 << 16), IntPtr.Zero);
        var FiltroEdicion = Campo<IMessageFilter>(Formulario, "_Edicion");
        Verificar(!FiltroEdicion.PreFilterMessage(ref Mensaje), "El modo de solo lectura consume la rueda");
        var ControlScroll = Campo<object>(Productos, typeof(UsrProductosOrdenesCompra), "_ScrollTabla");
        ControlScroll.GetType().GetMethod("WndProc", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(ControlScroll, new object[] { Mensaje });
        Verificar(Principal.AutoScrollPosition.Y < Antes, "La rueda sobre el dgv no mueve el scroll principal");
        Verificar(Tabla.Tabla.ScrollBars == ScrollBars.None && Tabla.Tabla.FirstDisplayedScrollingRowIndex == Fila,
            "La rueda desplaza el dgv o muestra sus barras");
        Verificar((int)typeof(ComprasUsrDataGridView).GetField("_PaginaActual", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Tabla) == Pagina,
            "La rueda cambia la página de productos");
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
            var Principal = Campo<ScrollableControl>(Formulario, "ComprasPnlDesplazamiento"); Principal.Parent = null; Principal.CreateControl();
            var Productos = Campo<UsrProductosOrdenesCompra>(Formulario, "ComprasUsrProductos");
            var Tabla = (ComprasUsrDataGridView)typeof(UsrProductosOrdenesCompra).GetProperty("Tabla", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Productos, null);
            Rueda(Formulario, Principal, Productos, Tabla);
            var Tarea = (Task)typeof(FrmOrdenesCompra).GetMethod("ComprasProcPrepararAsync", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Formulario, new object[] { true });
            var Tiempo = Stopwatch.StartNew();
            while (!Tarea.IsCompleted && Tiempo.ElapsedMilliseconds < 10000) Application.DoEvents();
            Verificar(Tarea.IsCompleted, "La preparación no terminó"); Tarea.GetAwaiter().GetResult();
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProveedor").SelectedValue = 3;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSucursal").SelectedValue = 1;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboBodega").SelectedValue = 11;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto").SelectedValue = 1;
            Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto").Text = "1";
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            var Detalles = (DataTable)typeof(UsrProductosOrdenesCompra).GetProperty("Detalles", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Productos, null);
            for (int Indice = 0; Indice < 39; Indice++)
            {
                var Valores = Detalles.Rows[0].ItemArray; Valores[0] = Indice + 2; Detalles.Rows.Add(Valores);
            }
            Application.DoEvents();
            var Siguiente = (Button)typeof(ComprasUsrDataGridView).GetField("ComprasBtnSiguiente", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Tabla);
            Verificar(Siguiente.Enabled, "Faltan productos para probar la segunda página"); Siguiente.PerformClick();
            Rueda(Formulario, Principal, Productos, Tabla);
            Principal.Dispose();
        }
        Console.WriteLine("Rueda sobre dgv vacío/solo lectura y con productos paginados desplaza el panel principal, sin mover filas ni cambiar páginas.");
    }
}

