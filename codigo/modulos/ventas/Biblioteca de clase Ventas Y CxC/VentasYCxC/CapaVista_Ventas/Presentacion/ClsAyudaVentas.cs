using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace VentasPresentacion
{
    // Fuente enlazada en las vistas; el manual viaja dentro de cada DLL.
    internal static class ClsAyudaVentas
    {
        internal static string VentasFuncPrepararManual(Assembly Biblioteca, string Recurso, string Carpeta)
        {
            using (Stream Manual = Biblioteca.GetManifestResourceStream(Recurso))
            {
                if (Manual == null) throw new FileNotFoundException("No se encontró el manual de ayuda incluido en la aplicación.");
                byte[] Huella;
                using (var Resumen = SHA256.Create()) Huella = Resumen.ComputeHash(Manual);
                string Archivo = Path.Combine(Carpeta, Biblioteca.GetName().Name + "_" + BitConverter.ToString(Huella, 0, 8).Replace("-", "") + ".chm");
                Directory.CreateDirectory(Carpeta);
                if (!File.Exists(Archivo))
                {
                    string Temporal = Archivo + "." + Guid.NewGuid().ToString("N") + ".tmp";
                    try
                    {
                        Manual.Position = 0;
                        using (var Destino = new FileStream(Temporal, FileMode.CreateNew, FileAccess.Write)) Manual.CopyTo(Destino);
                        try { File.Move(Temporal, Archivo); }
                        catch (IOException) { if (!File.Exists(Archivo)) throw; }
                    }
                    finally { if (File.Exists(Temporal)) File.Delete(Temporal); }
                }
                return Archivo;
            }
        }

        internal static void VentasMetMostrar(Form Ventana, Assembly Biblioteca, string Recurso, string Tema)
        {
            try
            {
                string Carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EmbutidosDeCalidad", "Ventas", "Ayuda");
                string Archivo = VentasFuncPrepararManual(Biblioteca, Recurso, Carpeta);
                Help.ShowHelp(Ventana, Archivo, Tema);
            }
            catch (IOException Error) { MostrarError(Ventana, Error.Message); }
            catch (UnauthorizedAccessException Error) { MostrarError(Ventana, Error.Message); }
            catch (System.ComponentModel.Win32Exception Error) { MostrarError(Ventana, Error.Message); }
        }

        private static void MostrarError(Form Ventana, string Mensaje)
        { MessageBox.Show(Ventana, "No fue posible abrir la ayuda. " + Mensaje, "Ayuda de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}
