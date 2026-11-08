using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CapaVista_Componentes.Componentes
{
    public static class ClsAyudasCompras
    {
        private static readonly string RutaRelativa = Path.Combine("ayuda", "modulos", "compras", "Ayuda_Compras.chm");

        public static string ObtenerRuta(Control Vista = null)
        {
            string UbicacionFormulario = Vista?.FindForm()?.GetType().Assembly.Location;
            string[] Directorios =
            {
                string.IsNullOrEmpty(UbicacionFormulario) ? null : Path.GetDirectoryName(UbicacionFormulario),
                Application.StartupPath,
                Path.GetDirectoryName(typeof(ClsAyudasCompras).Assembly.Location)
            };
            foreach (string Directorio in Directorios)
            {
                if (string.IsNullOrEmpty(Directorio)) continue;
                string Ruta = BuscarDesdeDirectorio(Directorio);
                if (Ruta != null) return Ruta;
            }
            throw new FileNotFoundException("No se encontro " + RutaRelativa +
                " desde la carpeta del formulario o del ejecutable ni en sus directorios padres.", RutaRelativa);
        }

        internal static string BuscarDesdeDirectorio(string DirectorioInicial)
        {
            DirectoryInfo Directorio = new DirectoryInfo(DirectorioInicial);
            while (Directorio != null)
            {
                string Ruta = Path.Combine(Directorio.FullName, RutaRelativa);
                if (File.Exists(Ruta)) return Ruta;
                Directorio = Directorio.Parent;
            }
            return null;
        }

        public static string ObtenerRuta(Control Vista, string Nombre)
        {
            return ObtenerRuta(Vista);
        }

        public static string ObtenerRuta(string Nombre)
        {
            return ObtenerRuta();
        }

        public static void Abrir(Control Vista, string PaginaInicial = null)
        {
            string Ruta = ObtenerRuta(Vista);
            Help.ShowHelp(Vista, Ruta, HelpNavigator.Topic, ResolverPaginaInicial(Ruta, PaginaInicial));
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int URLOpenBlockingStreamW(IntPtr Solicitante, string Url,
            out IntPtr Flujo, uint Reservado, IntPtr Notificacion);

        internal static string ResolverPaginaInicial(string Ruta, string PaginaInicial)
        {
            if (string.IsNullOrWhiteSpace(PaginaInicial)) return "index.html";
            string Pagina = PaginaInicial.Trim().Replace('\\', '/').TrimStart('/');
            string Archivo = Pagina.Split('#', '?')[0];
            if (Archivo.Length == 0) return "index.html";
            IntPtr Flujo = IntPtr.Zero;
            try
            {
                int Resultado = URLOpenBlockingStreamW(IntPtr.Zero,
                    "ms-its:" + Ruta + "::/" + Archivo, out Flujo, 0, IntPtr.Zero);
                if (Resultado == unchecked((int)0x80030002) ||
                    Resultado == unchecked((int)0x80070002) ||
                    Resultado == unchecked((int)0x800C0005))
                    return "index.html";
                return Pagina;
            }
            finally
            {
                if (Flujo != IntPtr.Zero) Marshal.Release(Flujo);
            }
        }

        public static void Abrir(Control Vista, string Nombre, string PaginaInicial)
        {
            Abrir(Vista, PaginaInicial);
        }
    }
}
