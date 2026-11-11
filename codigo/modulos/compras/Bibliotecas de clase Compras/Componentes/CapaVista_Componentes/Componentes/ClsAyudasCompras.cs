using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace CapaVista_Componentes.Componentes
{
    public static class ClsAyudasCompras
    {
        private static readonly string RutaRelativa = Path.Combine("ayuda", "modulos", "compras", "ComprasAyudas.chm");

        public static string ObtenerRuta(Control Vista = null)
        {
            string Incrustada = PrepararAyudaIncrustada();
            if (Incrustada != null) return Incrustada;
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

        internal static string PrepararAyudaIncrustada()
        {
            using (Stream Recurso = typeof(ClsAyudasCompras).Assembly.GetManifestResourceStream("Ayudas/ComprasAyudas.chm"))
            {
                if (Recurso == null) return null;
                string Carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Embutidos de Calidad", "Ayudas");
                Directory.CreateDirectory(Carpeta);
                string Ruta = Path.Combine(Carpeta, "ComprasAyudas.chm");
                byte[] Contenido;
                using (var Memoria = new MemoryStream())
                {
                    Recurso.CopyTo(Memoria);
                    Contenido = Memoria.ToArray();
                }
                using (var Hash = System.Security.Cryptography.SHA256.Create())
                {
                    if (File.Exists(Ruta))
                        using (var Existente = File.OpenRead(Ruta))
                            if (Convert.ToBase64String(Hash.ComputeHash(Existente)) == Convert.ToBase64String(Hash.ComputeHash(Contenido))) return Ruta;
                }
                string Temporal = Ruta + "." + Guid.NewGuid().ToString("N") + ".tmp";
                try
                {
                    File.WriteAllBytes(Temporal, Contenido);
                    if (File.Exists(Ruta)) File.Replace(Temporal, Ruta, null);
                    else File.Move(Temporal, Ruta);
                }
                catch (IOException Error)
                {
                    throw new IOException("No se pudo actualizar la ayuda de Compras. Cierra las ventanas de ayuda y vuelve a intentarlo.", Error);
                }
                finally { if (File.Exists(Temporal)) File.Delete(Temporal); }
                return Ruta;
            }
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
            string Pagina = ResolverPaginaInicial(Ruta, PaginaInicial);
            using (Process Visor = Process.Start(CrearInicioAyuda(Ruta, Pagina)))
            {
                if (Visor == null) throw new InvalidOperationException("No se pudo abrir el visor de ayuda de Compras.");
            }
        }

        internal static ProcessStartInfo CrearInicioAyuda(string Ruta, string Pagina)
        {
            return new ProcessStartInfo
            {
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "hh.exe"),
                Arguments = "\"" + Ruta + "::/" + Pagina + "\"",
                WorkingDirectory = Path.GetDirectoryName(Ruta),
                UseShellExecute = false
            };
        }

        internal static string ResolverPaginaInicial(string Ruta, string PaginaInicial)
        {
            if (string.IsNullOrWhiteSpace(PaginaInicial)) return "index.html";
            string Pagina = PaginaInicial.Trim().Replace('\\', '/').TrimStart('/');
            string Archivo = Pagina.Split('#', '?')[0];
            if (Archivo.Length == 0) return "index.html";
            return ClsContenidoAyudaCompras.ContienePagina(Ruta, Archivo) ? Pagina : "index.html";
        }

        public static void Abrir(Control Vista, string Nombre, string PaginaInicial)
        {
            Abrir(Vista, PaginaInicial);
        }
    }
}
