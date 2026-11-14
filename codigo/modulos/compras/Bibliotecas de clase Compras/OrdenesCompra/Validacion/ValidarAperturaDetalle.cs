using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using CapaControlador_OrdenesCompra;
using CapaModelo_OrdenesCompra.Repositorios;
using CapaVista_OrdenesCompra;
using CapaVista_Componentes.Componentes;

class ValidarAperturaDetalle
{
    static T Campo<T>(object Instancia, string Nombre) where T : class
    {
        for (Type Tipo = Instancia.GetType(); Tipo != null; Tipo = Tipo.BaseType)
        {
            var Campo = Tipo.GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic);
            if (Campo != null) return (T)Campo.GetValue(Instancia);
        }
        throw new MissingFieldException(Instancia.GetType().FullName, Nombre);
    }
    static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new Exception(Mensaje);
    }
    static void SoloLectura(Control Contenedor)
    {
        foreach (Control Control in Contenedor.Controls)
        {
            var Texto = Control as ComprasUsrTextBox;
            if (Texto != null) Verificar(Texto.Enabled && Texto.ReadOnly, "Campo de detalle deshabilitado o editable: " + Texto.Name);
            else if (Control is ComprasUsrComboBox || Control is DateTimePicker)
                Verificar(Control.Enabled, "Selector de detalle deshabilitado: " + Control.Name);
            else SoloLectura(Control);
        }
    }
    [STAThread]
    static int Main()
    {
        try
        {
            var Controlador = new ClsOrdenesCompraControlador();
            using (var Formulario = new FrmEncabezados(Controlador))
            using (var Datos = Controlador.ComprasFuncListar())
            {
                Verificar(Datos.Rows.Count > 0, "No hay órdenes para verificar");
                var Contenido = Campo<Panel>(Formulario, "ComprasPnlContenido");
                Contenido.Parent = null; Contenido.CreateControl();
                var Grupo = Campo<GroupBox>(Formulario, "ComprasGbxOrdenes");
                Verificar(Grupo.Text.Contains("Doble clic") && Grupo.Text.Contains("detalle"), "Falta la instrucción permanente");
                var Tabla = Campo<ComprasUsrDataGridView>(Formulario, "ComprasDgvEncabezados");
                Tabla.DataSource = Datos.Copy();
                var Abrir = typeof(FrmEncabezados).GetMethod("ComprasProcAbrirCompra", BindingFlags.Instance | BindingFlags.NonPublic);
                Abrir.Invoke(Formulario, new object[] { Tabla, new DataGridViewCellEventArgs(0, 0) });
                var Tiempo = Stopwatch.StartNew();
                var Abriendo = typeof(FrmEncabezados).GetField("_AbriendoCompra", BindingFlags.NonPublic | BindingFlags.Instance);
                while ((bool)Abriendo.GetValue(Formulario) && Tiempo.ElapsedMilliseconds < 15000) Application.DoEvents();
                Verificar(Formulario.AbrirOrden, "No abrió el detalle: " + Campo<ComprasUsrLabel>(Formulario, "ComprasLblResultado").Texto);
                using (var Detalle = Formulario.ComprasFuncCrearOrdenSeleccionada())
                {
                    Verificar(Detalle is FrmDetalleOrdenCompra,
                        "La navegación del menú no abrió el formulario de detalle");
                    SoloLectura(Detalle);
                    var Productos = Campo<ComprasUsrDataGridView>(Detalle, "ComprasDgvProductos").DataSource as DataTable;
                    Verificar(Productos != null && Productos.Rows.Count > 0, "El detalle no cargó productos");
                    Console.WriteLine("Apertura desde encabezados correcta. Productos cargados: " + Productos.Rows.Count);
                }
                Contenido.Dispose();
            }
            return 0;
        }
        catch (Exception Error) { Console.WriteLine(Error); return 1; }
    }
}
