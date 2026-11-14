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

class ValidarProductosProveedor
{
    static T Campo<T>(FrmOrdenesCompra Formulario, string Nombre) where T : class
        => (T)typeof(FrmOrdenesCompra).GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Formulario);
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    static void Vacio(FrmOrdenesCompra Formulario)
    {
        foreach (string Nombre in new[] { "ComprasTxtDescripcionProducto", "ComprasTxtPesoProducto", "ComprasTxtCostoUnitarioProducto",
            "ComprasTxtCantidadProducto", "ComprasTxtDescuentoProducto", "ComprasTxtDescuentoPorcentajeProducto", "ComprasTxtTotalProducto" })
            Verificar(Campo<ComprasUsrTextBox>(Formulario, Nombre).Text == "", "No se limpió " + Nombre);
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
            var Contenido = Campo<Panel>(Formulario, "ComprasPnlContenido");
            Contenido.Parent = null; Contenido.CreateControl();
            var Tarea = (Task)typeof(FrmOrdenesCompra).GetMethod("ComprasProcPrepararAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(Formulario, new object[] { true });
            var Tiempo = Stopwatch.StartNew();
            while (!Tarea.IsCompleted && Tiempo.ElapsedMilliseconds < 10000) Application.DoEvents();
            Verificar(Tarea.IsCompleted, "La preparación no terminó");
            Tarea.GetAwaiter().GetResult();
            var Proveedor = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProveedor");
            var Sku = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSkuProducto");
            var Producto = Campo<ComprasUsrComboBox>(Formulario, "ComprasCboNombreProducto");
            var Porcentaje = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoPorcentajeProducto");
            var Importe = Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoProducto");
            var TablaProducto = (TableLayoutPanel)Porcentaje.Parent;
            Verificar(Porcentaje.ReadOnly && Importe.ReadOnly && Importe.Entrada.ReadOnly && !Importe.TabStop,
                "Los descuentos deben iniciar en solo lectura");
            Verificar(TablaProducto.GetRow(Porcentaje) == 7 && TablaProducto.GetRow(Importe) == 8,
                "No se intercambiaron las posiciones de los descuentos");
            Verificar(Sku.Enabled && Producto.Enabled && Sku.Items.Count == 0 && Producto.Items.Count == 0, "Sin proveedor se muestran productos");
            Vacio(Formulario);
            Proveedor.SelectedValue = 3;
            Verificar(!Porcentaje.ReadOnly && Importe.ReadOnly, "Solo debe habilitarse la edición porcentual");
            Verificar(Sku.Enabled && Producto.Enabled && Sku.Items.Count == 1 && Producto.Items.Count == 1, "Filtro del proveedor 3 incorrecto");
            Verificar(Sku.SelectedIndex == -1 && Producto.SelectedIndex == -1, "Se seleccionó producto automáticamente");
            Sku.SelectedValue = 1;
            Verificar(Convert.ToInt32(Producto.SelectedValue) == 1, "No se sincronizó Producto con SKU");
            Verificar(Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescripcionProducto").Text == "Descripcion Uno", "No se cargó el producto");
            Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto").Text = "3";
            Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtDescuentoPorcentajeProducto").Text = "10";
            Porcentaje.Text = "12.34";
            Verificar(Importe.ComprasFuncLeerMonto(out decimal Descuento) && Descuento == 9.50M, "Importe calculado incorrecto");
            Porcentaje.Text = "33.333";
            Verificar(Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtTotalProducto").Text == "", "Se aceptaron más de dos decimales");
            Porcentaje.Text = "33.33";
            Sku.Text = "SKU inválido";
            Vacio(Formulario);
            Verificar(Producto.SelectedIndex == -1 && Producto.Text == "", "Quedó el producto anterior");
            Sku.SelectedValue = 1;
            Producto.Text = "Producto inexistente";
            Vacio(Formulario);
            Verificar(Sku.SelectedIndex == -1 && Sku.Text == "", "Quedó el SKU anterior");
            Producto.SelectedValue = 1;
            Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto").Text = "2";
            Proveedor.SelectedValue = 4;
            Verificar(Sku.Items.Count == 1 && Producto.Items.Count == 1 && Convert.ToInt32(((DataRowView)Sku.Items[0])["idProducto"]) == 2, "No se cambió el filtro");
            Verificar(Sku.SelectedIndex == -1 && Producto.SelectedIndex == -1, "Quedó una selección del proveedor anterior");
            Vacio(Formulario);
            Producto.SelectedValue = 2;
            Verificar(Convert.ToInt32(Sku.SelectedValue) == 2, "No se sincronizó SKU con Producto");
            Proveedor.Text = "Proveedor inválido";
            Verificar(Sku.Enabled && Producto.Enabled && Sku.Items.Count == 0 && Producto.Items.Count == 0, "Proveedor inválido conserva catálogo");
            Vacio(Formulario);
            Proveedor.SelectedValue = 5;
            Verificar(Sku.Items.Count == 0 && Producto.Items.Count == 0, "Proveedor sin productos muestra catálogo ajeno");
            Vacio(Formulario);
            Proveedor.SelectedIndex = -1; Proveedor.SelectedValue = 3;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboSucursal").SelectedValue = 1;
            Campo<ComprasUsrComboBox>(Formulario, "ComprasCboBodega").SelectedValue = 11;
            Sku.SelectedValue = 1;
            Campo<ComprasUsrTextBox>(Formulario, "ComprasTxtCantidadProducto").Text = "3";
            Porcentaje.Text = "33.33";
            Campo<Button>(Formulario, "ComprasBtnIngresarProducto").PerformClick();
            var Productos = Campo<UsrProductosOrdenesCompra>(Formulario, "ComprasUsrProductos");
            var Tabla = (ComprasUsrDataGridView)typeof(UsrProductosOrdenesCompra).GetProperty("Tabla", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Productos, null);
            Verificar(Tabla.Rows.Count == 1, "No se agregó el producto");
            Verificar(!Tabla.Columns.Contains("ComprasColpeso"), "La columna Peso sigue visible");
            Verificar(Tabla.Columns["ComprasColdescuentoTotal"].DisplayIndex == Tabla.Columns["ComprasColtotal"].DisplayIndex - 1,
                "El descuento total debe estar antes de Total");
            Tabla.ClearSelection(); Tabla.Rows[0].Selected = true;
            Verificar(Porcentaje.Text.Contains("33.33") && Importe.ReadOnly, "La edición no conserva el porcentaje");
            Porcentaje.Text = "10";
            Campo<Button>(Formulario, "ComprasBtnModificarProducto").PerformClick();
            var Detalles = (DataTable)typeof(UsrProductosOrdenesCompra).GetProperty("Detalles", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Productos, null);
            Verificar((decimal)Detalles.Rows[0]["porcentaje"] == 10M && (decimal)Detalles.Rows[0]["total"] == 207.9M,
                "No se modificó el descuento porcentual");
            Verificar((decimal)Detalles.Rows[0]["descuentoTotal"] == 23.1M, "Descuento absoluto de línea incorrecto");
            using (var Captura = new System.Drawing.Bitmap(Contenido.Width, Contenido.Height))
            {
                Contenido.DrawToBitmap(Captura, new System.Drawing.Rectangle(System.Drawing.Point.Empty, Captura.Size));
                Captura.Save("Validacion/DescuentoPorcentaje.png");
            }
            Contenido.Dispose();
        }
        Console.WriteLine("Productos por proveedor: filtros, sincronización, limpieza de campos, cambio de proveedor y catálogo vacío correctos.");
    }
}
