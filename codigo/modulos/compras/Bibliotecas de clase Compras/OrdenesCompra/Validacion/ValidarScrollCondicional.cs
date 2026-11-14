using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;

class ValidarScrollCondicional
{
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    [STAThread]
    static int Main()
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        try { Ejecutar(); return 0; }
        catch (Exception Error) { Console.WriteLine(Error); return 1; }
    }
    static void Ejecutar()
    {
        using (var Panel = new Panel { Size = new Size(900, 650) })
        using (var Productos = new UsrProductosOrdenesCompra { Dock = DockStyle.Fill })
        using (var Datos = new DataTable())
        {
            Panel.Controls.Add(Productos); Panel.CreateControl();
            var Tabla = (ComprasUsrDataGridView)typeof(UsrProductosOrdenesCompra).GetProperty("Tabla", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Productos, null);
            DataGridView Grid = Tabla.Tabla;
            Datos.Columns.Add("sku"); Datos.Columns.Add("producto"); Datos.Columns.Add("cantidad", typeof(int));
            for (int Indice = 0; Indice < 50; Indice++) Datos.Rows.Add("SKU-" + Indice, "Producto", 1);
            Tabla.DataSource = Datos; Application.DoEvents();
            Action Comprobar = () =>
            {
                Application.DoEvents();
                int Alto = Grid.ClientSize.Height - Grid.ColumnHeadersHeight;
                int Capacidad = Math.Max(1, Alto / Math.Max(28, Grid.RowTemplate.Height));
                Verificar(Grid.ScrollBars == ScrollBars.None, "El grid no debe tener scroll");
                Verificar(Tabla.RegistrosPorPagina == Capacidad, "El límite no coincide con las filas que caben");
                Verificar(Grid.Rows.Count <= Capacidad, "La página excede la capacidad visible");
                Verificar(Grid.Rows.GetRowsHeight(DataGridViewElementStates.Visible) <= Alto, "Hay filas parcialmente ocultas");
            };
            Comprobar();
            int PrimeraCapacidad = Tabla.RegistrosPorPagina;
            Verificar(Grid.Rows.Count == PrimeraCapacidad, "No se aprovecha todo el espacio visible");
            var Siguiente = (Button)typeof(ComprasUsrDataGridView).GetField("ComprasBtnSiguiente", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Tabla);
            Siguiente.PerformClick(); Comprobar();
            Verificar(Convert.ToString(Grid.Rows[0].Cells["ComprasColsku"].Value) == "SKU-" + PrimeraCapacidad, "La paginación pierde productos");
            Panel.Size = new Size(900, 300); Panel.PerformLayout(); Comprobar();
            Verificar(Tabla.RegistrosPorPagina < PrimeraCapacidad, "La página no reduce filas al reducir el espacio");
            Panel.Size = new Size(900, 1200); Panel.PerformLayout(); Comprobar();
            Verificar(Tabla.RegistrosPorPagina > PrimeraCapacidad, "La página no aumenta filas al ampliar el espacio");
            var ControlScroll = typeof(UsrProductosOrdenesCompra).GetField("_ScrollTabla", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Productos);
            int Inicio = Grid.FirstDisplayedScrollingRowIndex;
            foreach (int Codigo in new[] { 0x020A, 0x020E, 0x0114, 0x0115 })
            {
                var Argumentos = new object[] { Message.Create(Grid.Handle, Codigo, (IntPtr)(-120 << 16), IntPtr.Zero) };
                ControlScroll.GetType().GetMethod("WndProc", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(ControlScroll, Argumentos);
                Verificar(((Message)Argumentos[0]).Result == IntPtr.Zero, "No se bloqueó el desplazamiento");
            }
            Verificar(Grid.FirstDisplayedScrollingRowIndex == Inicio, "La rueda desplaza las filas");
            Datos.Clear(); Application.DoEvents(); Comprobar();
            Verificar(Grid.Rows.Count == 0 && !Siguiente.Enabled, "El grid vacío conserva páginas activas");
        }
        Console.WriteLine("Sin scroll: capacidad visible máxima, paginación sin perder productos, redimensionado, rueda bloqueada y grid vacío verificados.");
    }
}
