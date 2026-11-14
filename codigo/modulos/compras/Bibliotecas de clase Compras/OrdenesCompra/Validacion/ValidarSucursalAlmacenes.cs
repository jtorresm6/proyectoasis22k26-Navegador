using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;

class ValidarSucursalAlmacenes
{
    static FieldInfo Campo(string Nombre) => typeof(FrmOrdenesCompra).GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic);
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    static bool Bloquea(FrmOrdenesCompra Formulario, ComprasUsrComboBox Almacen)
    {
        var Filtro = (IMessageFilter)Campo("_Edicion").GetValue(Formulario);
        var Mensaje = Message.Create(Almacen.Entrada.Handle, 0x0100, (IntPtr)Keys.A, IntPtr.Zero);
        return Filtro.PreFilterMessage(ref Mensaje);
    }
    [STAThread]
    static void Main()
    {
        using (var Formulario = new FrmOrdenesCompra())
        using (var Sucursales = new DataTable())
        using (var Almacenes = new DataTable())
        {
            var Contenido = (Panel)Campo("ComprasPnlContenido").GetValue(Formulario);
            Contenido.Parent = null; Contenido.CreateControl();
            var Sucursal = (ComprasUsrComboBox)Campo("ComprasCboSucursal").GetValue(Formulario);
            var Almacen = (ComprasUsrComboBox)Campo("ComprasCboBodega").GetValue(Formulario);
            var Pais = (ComprasUsrTextBox)Campo("ComprasTxtPaisBodega").GetValue(Formulario);
            Sucursales.Columns.Add("idSucursal", typeof(int)); Sucursales.Columns.Add("nombreSucursal");
            Sucursales.Rows.Add(1, "Central"); Sucursales.Rows.Add(2, "Norte");
            Almacenes.Columns.Add("idBodega", typeof(int)); Almacenes.Columns.Add("nombreBodega");
            Almacenes.Columns.Add("Fk_Sucursal_Codigo", typeof(int)); Almacenes.Columns.Add("paisBodega");
            Almacenes.Rows.Add(10, "Principal", 1, "Guatemala"); Almacenes.Rows.Add(20, "Secundario", 2, "Guatemala");
            using (var Vista = new DataView(Almacenes) { RowFilter = "1 = 0" })
            {
                Sucursal.Entrada.BindingContext = new BindingContext(); Almacen.Entrada.BindingContext = new BindingContext();
                Sucursal.ValueMember = "idSucursal"; Sucursal.DisplayMember = "nombreSucursal"; Sucursal.DataSource = Sucursales;
                Almacen.ValueMember = "idBodega"; Almacen.DisplayMember = "nombreBodega"; Almacen.DataSource = Vista;
                Campo("_VistaBodegas").SetValue(Formulario, Vista);
                Sucursal.SelectedIndex = -1;
                Campo("_CargandoCatalogos").SetValue(Formulario, false);
                Sucursal.Enabled = true;
                Campo("_CapturaDisponible").SetValue(Formulario, true);
                var HandleSucursal = Sucursal.Entrada.Handle;
                var HandleAlmacen = Almacen.Entrada.Handle;
                typeof(FrmOrdenesCompra).GetMethod("ComprasProcActualizarSucursal", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Formulario, null);
                Verificar(Almacen.Enabled && Bloquea(Formulario, Almacen) && Almacen.SelectedIndex == -1 && Almacen.Text == "", "Estado inicial inválido");
                Sucursal.SelectedIndex = 0;
                Verificar(Almacen.Enabled && !Bloquea(Formulario, Almacen) && Vista.Count == 1, "Sucursal válida no habilita almacenes");
                Almacen.SelectedIndex = 0;
                Verificar(Pais.Text == "Guatemala", "No se seleccionó el almacén");
                Sucursal.Text = "Sucursal inexistente";
                Verificar(Almacen.Enabled && Bloquea(Formulario, Almacen) && Almacen.SelectedIndex == -1 && Almacen.Text == "" && Pais.Text == "", "Texto inválido conserva el almacén");
                Sucursal.SelectedIndex = 1;
                Verificar(Almacen.Enabled && !Bloquea(Formulario, Almacen) && Vista.Count == 1 && (int)Vista[0]["idBodega"] == 20, "No filtra por nueva sucursal");
                Almacen.SelectedIndex = 0;
                Application.DoEvents();
                Sucursal.Text = "";
                Verificar(Almacen.Enabled && Bloquea(Formulario, Almacen) && Almacen.SelectedIndex == -1 && Almacen.Text == "" && Pais.Text == "", "Vaciar la sucursal conserva el almacén");
                Sucursal.SelectedIndex = 0; Almacen.SelectedIndex = 0;
                Sucursal.SelectedIndex = 1;
                Verificar(Almacen.Enabled && !Bloquea(Formulario, Almacen) && Almacen.SelectedIndex == -1 && Almacen.Text == "", "Cambiar de sucursal conserva almacén anterior");
            }
        }
        Console.WriteLine("Sucursal y almacenes: inicio vacío, texto inválido, borrado y cambio de sucursal correctos.");
    }
}







