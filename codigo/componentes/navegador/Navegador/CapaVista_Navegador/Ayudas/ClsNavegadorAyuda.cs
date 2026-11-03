/*
 * ============================================================
 * Área   : NuevoNavegador
 * Autor  : Natali Sofía Montenegro Portillo
 * Modificación: Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 26/10/2026
 *
 * Propósito:
 * Gestiona la visualización de la ayuda dinámica del componente
 * Navegador. Solicita al Controlador la información del destino
 * de ayuda correspondiente a la aplicación y muestra el archivo
 * CHM y la página indicados. También presenta los mensajes
 * correspondientes cuando no existe una ayuda disponible o
 * cuando ocurre un error durante su apertura.
 *
 * ============================================================
 */

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CapaControlador_Navegador;


namespace CapaVista_Navegador
{
    
    public static class ClsNavegadorAyuda
    {
        private const string _Recurso = "Navegador.chm";

        // Inicio cambio - Evelyn Sofía Andrade Luna - 9959-23-1224 - 26/10/2026
        public static void NavegadorMetMostrar(Navegador Componente)
        {
            try
            {
                
                Form Formulario = Componente.FindForm();
                string NombreFormulario = Formulario == null ? "" : Formulario.GetType().Name;

                ClsCtrlAyuda Controlador = new ClsCtrlAyuda();
                ClsDestinoAyuda Destino = Controlador.NavegadorFuncObtenerDestino(Componente.NavegadorIdAplicacion, NombreFormulario);

                if (Destino.RutaArchivo == "")
                {
                    MessageBox.Show(
                        Destino.Mensaje,
                        "Ayuda",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                if (Destino.Pagina == "")
                {
                    Help.ShowHelp(Componente, Destino.RutaArchivo);
                }
                else
                {
                    Help.ShowHelp(Componente, Destino.RutaArchivo, HelpNavigator.Topic, Destino.Pagina);
                }
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(
                    "No se pudo abrir la ayuda de esta aplicación.\n\n" + Excepcion.Message,
                    "Ayuda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        
        private static string NavegadorFuncExtraer()
        {
            string Carpeta = Path.Combine(Path.GetTempPath(), "Navegador");
            string Ruta = Path.Combine(Carpeta, _Recurso);

            using (Stream Origen = Assembly.GetExecutingAssembly().GetManifestResourceStream(_Recurso))
            {
                if (Origen == null)
                {
                    throw new FileNotFoundException("La ayuda no está incluida en el componente Navegador.");
                }

                if (!File.Exists(Ruta) || new FileInfo(Ruta).Length != Origen.Length)
                {
                    Directory.CreateDirectory(Carpeta);

                    using (FileStream Destino = File.Create(Ruta))
                    {
                        Origen.CopyTo(Destino);
                    }
                }
            }

            return Ruta;
        }
    }
}

