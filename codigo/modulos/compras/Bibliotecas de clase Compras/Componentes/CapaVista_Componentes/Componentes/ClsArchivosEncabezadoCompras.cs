using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CapaVista_Componentes.Componentes
{
    internal static class ClsArchivosEncabezadoCompras
    {
        internal static string NormalizarNombre(string Archivo, string Extension)
        {
            if (string.IsNullOrWhiteSpace(Archivo)) return string.Empty;
            string Nombre = Path.GetFileName(Archivo.Trim());
            if (string.IsNullOrEmpty(Nombre))
                throw new ArgumentException("Seleccione un archivo " + Extension + ".", nameof(Archivo));
            if (string.IsNullOrEmpty(Path.GetExtension(Nombre))) Nombre += Extension;
            if (!string.Equals(Path.GetExtension(Nombre), Extension, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("El archivo debe tener extension " + Extension + ".", nameof(Archivo));
            return Nombre;
        }

        internal static string BuscarDesdeVista(Control Vista, string Nombre, string Carpeta, string Extension)
        {
            Control Formulario = Vista?.FindForm();
            if (Formulario != null)
            {
                string Ubicacion = Formulario.GetType().Assembly.Location;
                if (!string.IsNullOrEmpty(Ubicacion))
                {
                    try
                    {
                        return Buscar(Nombre, Carpeta, Extension, Path.GetDirectoryName(Ubicacion));
                    }
                    catch (FileNotFoundException)
                    {
                    }
                }
            }
            try
            {
                return Buscar(Nombre, Carpeta, Extension, Application.StartupPath);
            }
            catch (FileNotFoundException)
            {
                return Buscar(Nombre, Carpeta, Extension,
                    Path.GetDirectoryName(typeof(ClsArchivosEncabezadoCompras).Assembly.Location));
            }
        }

        internal static string Buscar(string Nombre, string Carpeta, string Extension, string DirectorioInicial)
        {
            Nombre = NormalizarNombre(Nombre, Extension);
            if (Nombre.Length == 0)
                throw new InvalidOperationException("Seleccione un archivo " + Extension + " en las propiedades del encabezado.");
            DirectoryInfo Directorio = new DirectoryInfo(DirectorioInicial);
            string Biblioteca = null;
            while (Directorio != null)
            {
                string Ruta = Path.Combine(Directorio.FullName, Carpeta, Nombre);
                if (File.Exists(Ruta)) return Ruta;
                if (Biblioteca == null)
                {
                    if (string.Equals(Directorio.Name, "Bibliotecas de clase Compras", StringComparison.OrdinalIgnoreCase))
                        Biblioteca = Directorio.FullName;
                    else
                    {
                        string Candidata = Path.Combine(Directorio.FullName, "Bibliotecas de clase Compras");
                        if (Directory.Exists(Candidata)) Biblioteca = Candidata;
                    }
                }
                Directorio = Directorio.Parent;
            }
            if (Biblioteca != null)
            {
                string Ruta = BuscarEnBiblioteca(Biblioteca, Nombre);
                if (Ruta != null) return Ruta;
            }
            throw new FileNotFoundException("No se encontro " + Path.Combine(Carpeta, Nombre) +
                " desde " + DirectorioInicial + " ni en sus directorios padres" +
                (Biblioteca == null ? "." : " ni en las carpetas de " + Biblioteca + "."), Nombre);
        }

        private static string BuscarEnBiblioteca(string Biblioteca, string Nombre)
        {
            var Pendientes = new Queue<string>();
            Pendientes.Enqueue(Biblioteca);
            while (Pendientes.Count > 0)
            {
                string Directorio = Pendientes.Dequeue();
                string Ruta = Path.Combine(Directorio, Nombre);
                if (File.Exists(Ruta)) return Ruta;
                try
                {
                    string[] Subdirectorios = Directory.GetDirectories(Directorio);
                    Array.Sort(Subdirectorios, StringComparer.OrdinalIgnoreCase);
                    foreach (string Subdirectorio in Subdirectorios)
                    {
                        try
                        {
                            if ((File.GetAttributes(Subdirectorio) & FileAttributes.ReparsePoint) == 0)
                                Pendientes.Enqueue(Subdirectorio);
                        }
                        catch (UnauthorizedAccessException) { }
                        catch (IOException) { }
                    }
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }
            return null;
        }
    }
}
