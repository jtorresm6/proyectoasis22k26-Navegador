using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using PruebaUnitaria_OrdenesCompra.Apoyo;

class FrmCambioProveedorPrueba : FrmOrdenesCompra
{
    public DialogResult Respuesta = DialogResult.No;
    public int Preguntas;
    public FrmCambioProveedorPrueba(ClsOrdenesCompraControlador Controlador) : base(Controlador) { }
    protected override DialogResult ComprasFuncConfirmarAccion(string Mensaje, string Titulo)
    {
        if (Titulo == "Cambiar proveedor") { Preguntas++; return Respuesta; }
        return DialogResult.Yes;
    }
    protected override void ComprasProcMostrarResultado(string Mensaje, bool Error)
    {
        if (Error) throw new Exception(Mensaje);
    }
}

class ValidarMascaraCambioProveedor
{
    static T Campo<T>(object Formulario, string Nombre) where T : class
        => (T)typeof(FrmOrdenesCompra).GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Formulario);
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    static bool Bloquea(object Formulario, Control Control)
    {
        var Filtro = Campo<IMessageFilter>(Formulario, "_Edicion");
        var Mensaje = Message.Create(Control.Handle, 0x0100, (IntPtr)Keys.A, IntPtr.Zero);
        return Filtro.PreFilterMessage(ref Mensaje);
    }
    static void Esperar(Func<bool> Condicion)
    {
        var Tiempo = Stopwatch.StartNew();
        while (!Condicion() && Tiempo.ElapsedMilliseconds < 10000) Application.DoEvents();
        Verificar(Condicion(), "La acción no terminó");
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
        using (var Formulario = new FrmCambioProveedorPrueba(new ClsOrdenesCompraControlador(Repositorio)))
        {
            var Contenido = Campo<Panel>(Formulario, "ComprasPnlContenido");
            Contenido.Parent = null; Contenido.CreateControl();
            var Proveedor = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProveedor");
            var Sku = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto");
            var Cantidad = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto");
            var Porcentaje = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoPorcentajeProducto");
            var Importe = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoProducto");
            var Comentario = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtComentario");
            Verificar(Sku.Enabled && Cantidad.Enabled && Comentario.Enabled && Cantidad.ReadOnly && Comentario.ReadOnly,
                "Los campos deben permanecer enabled y solo lectura al inicio");
            Verificar(Campo<Button>(Formulario, "ComprasBtnDescartar").Enabled, "Descartar debe estar habilitado al iniciar");
            Verificar(Bloquea(Formulario, Sku.Entrada), "El combo inicial permite cambios");
            var Tarea = (Task)typeof(FrmOrdenesCompra).GetMethod("ComprasProcPrepararAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(Formulario, new object[] { true });
            Esperar(() => Tarea.IsCompleted); Tarea.GetAwaiter().GetResult();
            Proveedor.SelectedValue = 3;
            Verificar(!Cantidad.ReadOnly && !Bloquea(Formulario, Sku.Entrada) && Importe.ReadOnly, "No se recupera la edición de captura");
            var Sucursal = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSucursal");
            var Almacen = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboBodega");
            Verificar(Almacen.Enabled && Bloquea(Formulario, Almacen.Entrada), "El almacén debe ser ineditable sin sucursal");
            Sucursal.SelectedValue = 1;
            Verificar(Almacen.Enabled && !Bloquea(Formulario, Almacen.Entrada), "El almacén no recuperó la edición");
            Almacen.SelectedValue = 11;
            Sku.SelectedValue = 1; Cantidad.Text = "3";
            Porcentaje.Text = "12.34";
            Verificar(Porcentaje.Text == "12.34 %", "Falta máscara porcentual durante la captura");
            Porcentaje.Text = "33.333";
            Verificar(Porcentaje.Text.EndsWith("%") && Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtTotalProducto").Text == "", "Máscara o validación incorrectas");
            Porcentaje.Text = "10";
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            var Productos = Campo<UsrProductosOrdenesCompra>(Formulario, "ComprasUsrProductos");
            var Detalles = (DataTable)typeof(UsrProductosOrdenesCompra).GetProperty("Detalles", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Productos, null);
            Verificar(Detalles.Rows.Count == 1 && !Comentario.ReadOnly, "No se activó la sección 6");
            Sku.SelectedValue = 1; Cantidad.Text = "1"; Porcentaje.Text = "5";
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            Verificar(Detalles.Rows.Count == 2, "No se agregó el segundo producto");
            Formulario.Respuesta = DialogResult.No;
            Proveedor.SelectedValue = 4;
            Esperar(() => Formulario.Preguntas == 1);
            Verificar(Detalles.Rows.Count == 2 && Formulario.IdProveedorSeleccionado == 3 && Convert.ToInt32(Proveedor.SelectedValue) == 3,
                "Cancelar no conservó el proveedor y los productos");
            Verificar(!Comentario.ReadOnly && Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled, "Cancelar alteró la disponibilidad");
            Formulario.Respuesta = DialogResult.Yes;
            Proveedor.SelectedValue = 4;
            Esperar(() => Formulario.Preguntas == 2);
            Verificar(Detalles.Rows.Count == 0 && Formulario.IdProveedorSeleccionado == 4, "Aceptar no eliminó los productos");
            Verificar(Comentario.Enabled && Comentario.ReadOnly && !Campo<Button>(Formulario, "ComprasBtnGuardar").Enabled,
                "La sección 6 no volvió a solo lectura");
            Verificar(Sku.Items.Count == 1 && Convert.ToInt32(((DataRowView)Sku.Items[0])["idProducto"]) == 2, "Filtro de proveedor incorrecto");
            Verificar(Campo<Button>(Formulario, "ComprasBtnDescartar").Enabled, "Descartar debe estar habilitado sin productos");
            Proveedor.SelectedValue = 3;
            Verificar(Formulario.Preguntas == 2, "Se confirmó un cambio sin productos");
            Contenido.Dispose();
        }
        Console.WriteLine("Máscara %, confirmación No/Sí, borrado de productos, campos enabled en solo lectura y Descartar siempre habilitado verificados.");
    }
}
