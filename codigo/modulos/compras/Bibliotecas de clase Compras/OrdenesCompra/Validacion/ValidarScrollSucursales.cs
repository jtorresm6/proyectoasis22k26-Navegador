using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;
using PruebaUnitaria_OrdenesCompra.Apoyo;

class ValidarScrollSucursales
{
    static T Campo<T>(object Objeto, Type Tipo, string Nombre) where T : class
        => (T)Tipo.GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Objeto);
    static T Campo<T>(FrmOrdenesCompra Formulario, string Nombre) where T : class => Campo<T>(Formulario, typeof(FrmOrdenesCompra), Nombre);
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
            var Scroll = Campo<ScrollableControl>(Formulario, "ComprasPnlDesplazamiento");
            Scroll.Parent = null; Scroll.CreateControl();
            var Tarea = (Task)typeof(FrmOrdenesCompra).GetMethod("ComprasProcPrepararAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(Formulario, new object[] { true });
            var Tiempo = Stopwatch.StartNew();
            while (!Tarea.IsCompleted && Tiempo.ElapsedMilliseconds < 10000) Application.DoEvents();
            Verificar(Tarea.IsCompleted, "La preparación no terminó"); Tarea.GetAwaiter().GetResult();
            Scroll.AutoScrollPosition = new Point(0, 700);
            Point Original = Scroll.AutoScrollPosition;
            Verificar(Original.Y < 0, "No se pudo desplazar el panel para probar");
            var Sku = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto");
            var MetodoScroll = Scroll.GetType().GetMethod("ScrollToControl", BindingFlags.Instance | BindingFlags.NonPublic);
            Verificar((Point)MetodoScroll.Invoke(Scroll, new object[] { Sku }) == Scroll.DisplayRectangle.Location,
                "El cambio de foco solicita otro desplazamiento");
            Sku.Text = "SKU inválido";
            typeof(Control).GetMethod("OnLeave", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Sku, new object[] { EventArgs.Empty });
            Verificar(Scroll.AutoScrollPosition == Original, "Salir del campo con error mueve el panel");
            typeof(FrmOrdenesCompra).GetMethod("ComprasProcEstablecerError", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(Formulario, new object[] { Sku, "" });
            Verificar(Scroll.AutoScrollPosition == Original, "Quitar la fila de error mueve el panel");
            Scroll.AutoScrollPosition = new Point(0, 800);
            Verificar(Scroll.AutoScrollPosition.Y == -800, "Se bloqueó el desplazamiento manual");
            var VistaBodegas = Campo<DataView>(Formulario, "_VistaBodegas");
            VistaBodegas.Table.Columns.Remove("nombreSucursal");
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProveedor").SelectedValue = 3;
            var Sucursal = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSucursal");
            var Almacen = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboBodega");
            var Cantidad = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto");
            var Porcentaje = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoPorcentajeProducto");
            var Productos = Campo<UsrProductosOrdenesCompra>(Formulario, "ComprasUsrProductos");
            Sucursal.SelectedValue = 1; Almacen.SelectedValue = 11; Sku.SelectedValue = 1;
            Cantidad.Text = "1"; Porcentaje.Text = "10";
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            var Pestanas = Campo<FlowLayoutPanel>(Productos, typeof(UsrProductosOrdenesCompra), "ComprasFlpAccionesProductos");
            Verificar(Pestanas.Controls.Count == 1 && Pestanas.Controls[0].Text.Contains("Central") && Pestanas.Controls[0].Text.Contains("Bodega Central"),
                "La pestaña no indica sucursal y bodega");
            Sucursal.SelectedValue = 2; Almacen.SelectedValue = 12; Sku.SelectedValue = 1;
            Cantidad.Text = "2"; Porcentaje.Text = "5";
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            Verificar(Pestanas.Controls.Count == 2 && Pestanas.Controls[1].Text.Contains("Norte") && Pestanas.Controls[1].Text.Contains("Bodega Norte"),
                "La segunda pestaña no indica su sucursal");
            var Tabla = (ComprasUsrDataGridView)typeof(UsrProductosOrdenesCompra).GetProperty("Tabla", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Productos, null);
            Campo<Button>(Productos, typeof(UsrProductosOrdenesCompra), "ComprasBtnBodegaAnterior").PerformClick();
            Verificar(((DataView)Tabla.DataSource).Count == 1 && Convert.ToInt32(((DataView)Tabla.DataSource)[0]["idSucursal"]) == 1,
                "No navega a la sucursal anterior");
            var PestañaNorte = (Button)Pestanas.Controls.Find("ComprasTabBodega12", false)[0];
            Point AntesPestana = Scroll.AutoScrollPosition;
            PestañaNorte.PerformClick();
            Verificar(Convert.ToInt32(((DataView)Tabla.DataSource)[0]["idSucursal"]) == 2, "La pestaña no filtra por bodega");
            Verificar(Scroll.AutoScrollPosition == AntesPestana, "Cambiar de pestaña mueve el panel principal");
            var Detalles = (DataTable)typeof(UsrProductosOrdenesCompra).GetProperty("Detalles", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Productos, null);
            for (int Indice = 0; Indice < 10; Indice++)
            {
                var Valores = Detalles.Rows[0].ItemArray;
                Valores[Detalles.Columns["idLinea"].Ordinal] = 1000 + Indice;
                Valores[Detalles.Columns["idBodega"].Ordinal] = 100 + Indice;
                Valores[Detalles.Columns["bodega"].Ordinal] = "Bodega adicional";
                Detalles.Rows.Add(Valores);
            }
            typeof(UsrProductosOrdenesCompra).GetMethod("ComprasProcActualizarPestanasBodega", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Productos, null);
            var Siguiente = Campo<Button>(Productos, typeof(UsrProductosOrdenesCompra), "ComprasBtnBodegaSiguiente");
            for (int Indice = 0; Indice < 10; Indice++) Siguiente.PerformClick();
            Application.DoEvents();
            Verificar(!Productos.AutoScroll && !Pestanas.AutoScroll && !Pestanas.HorizontalScroll.Visible && !Pestanas.VerticalScroll.Visible,
                "El control de productos o las pestañas muestran barras de scroll");
            Verificar(!Siguiente.Enabled && Pestanas.Controls[Pestanas.Controls.Count - 1].Visible,
                "Las flechas no permiten llegar a una pestaña oculta");
            Verificar(Tabla.Tabla.ScrollBars == ScrollBars.None && !Tabla.AutoScroll, "El dgv interno conserva barras de scroll");
            using (var Captura = new Bitmap(Productos.Width, Productos.Height))
            {
                Productos.DrawToBitmap(Captura, new Rectangle(Point.Empty, Captura.Size));
                Captura.Save("Validacion/PestanasSucursal.png");
            }
            Scroll.Dispose();
        }
        Console.WriteLine("Posición conservada al perder foco y validar, scroll manual disponible y pestañas sucursal/bodega verificadas.");
    }
}
