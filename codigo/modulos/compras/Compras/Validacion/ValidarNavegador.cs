using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

public static class ValidarNavegador
{
    private static object Campo(object Objeto, string Nombre)
    {
        return Objeto.GetType().GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Objeto);
    }

    private static void Asignar(object Objeto, string Nombre, object Valor)
    {
        Objeto.GetType().GetField(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Objeto, Valor);
    }

    private static void Invocar(object Objeto, string Nombre, params object[] Argumentos)
    {
        Objeto.GetType().GetMethod(Nombre, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Objeto, Argumentos);
    }

    private static void Verificar(bool Condicion, string Mensaje)
    {
        if (!Condicion) throw new InvalidOperationException(Mensaje);
    }

    public static void Ejecutar(string Raiz)
    {
        string Bin = Path.Combine(Raiz, "CapaVista_Compras", "bin", "Debug");
        ResolveEventHandler Resolver = (Origen, Evento) =>
        {
            string Ruta = Path.Combine(Bin, new AssemblyName(Evento.Name).Name + ".dll");
            return File.Exists(Ruta) ? Assembly.LoadFrom(Ruta) : null;
        };
        AppDomain.CurrentDomain.AssemblyResolve += Resolver;
        try
        {
            var Ensamblado = Assembly.LoadFrom(Path.Combine(Bin, "CapaVista_Compras.dll"));
            using (var Formulario = (Form)Activator.CreateInstance(Ensamblado.GetType("CapaVista_Compras.FrmMDICompras")))
            using (var Temporizador = new System.Windows.Forms.Timer())
            {
                Asignar(Formulario, "_AnimacionNavegador", Temporizador);
                var Parametros = (CreateParams)Formulario.GetType().GetProperty("CreateParams", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Formulario, null);
                Verificar((Parametros.ExStyle & 0x02000000) != 0, "El formulario debe componer sus controles en un solo repintado.");
                var Panel = (Panel)Campo(Formulario, "_PanelDesplazamientoNavegador");
                var Tabla = (TableLayoutPanel)Campo(Formulario, "ComprasTlpNavegador");
                Panel.Size = new System.Drawing.Size(286, 400);
                Invocar(Formulario, "ComprasMetAcomodarCatalogos");
                Invocar(Formulario, "ComprasMetAlternarCatalogos", null, EventArgs.Empty);
                Invocar(Formulario, "ComprasMetAlternarRegistrosCompra", null, EventArgs.Empty);
                int Limite = (int)Campo(Formulario, "_LimiteDesplazamiento");
                Verificar(Limite > 0, "El contenido expandido debe superar la altura visible.");
                Invocar(Formulario, "ComprasMetMoverNavegador", 0.5);
                Verificar((double)Campo(Formulario, "_DestinoNavegador") == 0.5, "Se perdio un desplazamiento fraccional.");
                Invocar(Formulario, "ComprasMetMoverNavegador", 100000.0);
                Verificar((double)Campo(Formulario, "_DestinoNavegador") == Limite, "El destino supera el limite inferior.");
                var Reloj = (Stopwatch)Campo(Formulario, "_RelojNavegador");
                Reloj.Restart();
                Thread.Sleep(20);
                Invocar(Formulario, "ComprasMetAnimarNavegador", null, EventArgs.Empty);
                double Posicion = (double)Campo(Formulario, "_PosicionNavegador");
                Verificar(Posicion > 0 && Posicion < Limite, "El desplazamiento debe avanzar sin saltar al destino.");
                Verificar(Tabla.Top == -(int)Math.Round(Posicion), "La tabla no sigue la posicion animada.");
                Invocar(Formulario, "ComprasMetMoverNavegador", -100.0);
                Verificar((double)Campo(Formulario, "_DestinoNavegador") == 0, "El destino supera el limite superior.");
                Asignar(Formulario, "_PosicionNavegador", (double)Limite);
                Asignar(Formulario, "_DestinoNavegador", (double)Limite);
                Panel.Height = 2000;
                Invocar(Formulario, "ComprasMetAjustarDesplazamientoNavegador");
                Verificar((double)Campo(Formulario, "_PosicionNavegador") == 0 && Tabla.Top == 0, "El redimensionamiento no reajusta la posicion.");
                Invocar(Formulario, "ComprasMetCambiarNavegador");
                Verificar(Tabla.Width < 270, "El menu no se contrae.");
                Invocar(Formulario, "ComprasMetCambiarNavegador");
                Verificar(Tabla.Width == 270, "El menu no recupera su ancho.");
                Panel.Height = 400;
                Invocar(Formulario, "ComprasMetAjustarDesplazamientoNavegador");
                Invocar(Formulario, "ComprasMetMoverNavegador", 200.0);
                Verificar(Temporizador.Enabled, "La prueba requiere desplazamiento en curso.");
                for (int i = 0; i < 20; i++)
                {
                    Invocar(Formulario, "ComprasMetCambiarNavegador");
                    Verificar(!Temporizador.Enabled, "El desplazamiento debe detenerse al transformar el menu.");
                    Verificar((double)Campo(Formulario, "_DestinoNavegador") == (double)Campo(Formulario, "_PosicionNavegador"), "La transformacion conserva un destino de desplazamiento anterior.");
                    Verificar(!(bool)Campo(Formulario, "_CambiandoNavegador"), "La transformacion no libero su estado.");
                }
                var Principal = (TableLayoutPanel)Campo(Formulario, "ComprasTlpPrincipal");
                Verificar(Tabla.Width == 270 && Principal.ColumnStyles[0].Width == 286F, "Los cambios repetidos alteran el ancho original.");
                Verificar((bool)Campo(Formulario, "_CatalogosAbierto") && (bool)Campo(Formulario, "_RegistrosCompraAbierto"), "La transformacion cambio los grupos abiertos.");
                Console.WriteLine("PASS: fractional input, eased motion, bounds, resizing, twenty burger toggles, scroll interruption and preserved menu groups.");
            }
        }
        finally
        {
            AppDomain.CurrentDomain.AssemblyResolve -= Resolver;
        }
    }
}
