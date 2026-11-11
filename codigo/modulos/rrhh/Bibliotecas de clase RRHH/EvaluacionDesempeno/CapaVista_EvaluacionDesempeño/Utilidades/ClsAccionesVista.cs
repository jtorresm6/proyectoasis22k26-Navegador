using System;
using System.IO;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * Modificado por : Daniella Jurado 0901-23-3679 (11/11/2026)
 * ==================================================================
 * Propósito :
 *  Acciones comunes de las ventanas de Evaluación de Desempeño:
 *  abrir la página de ayuda de cada ventana en RrhhAyudas.chm.
 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Utilidades
{
    public static class ClsAccionesVista
    {
        private const string _Ayuda = @"ayuda\modulos\rrhh\RrhhAyudas\RrhhAyudas.chm";

        // Busca el .chm de RRHH subiendo carpetas desde donde corre el programa y abre la página indicada.
        public static void RrhhMetMostrarAyuda(Control Ventana, string Pagina)
        {
            for (var Carpeta = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); Carpeta != null; Carpeta = Carpeta.Parent)
            {
                string Ruta = Path.Combine(Carpeta.FullName, _Ayuda);
                if (File.Exists(Ruta))
                {
                    Help.ShowHelp(Ventana, Ruta, Pagina);
                    return;
                }
            }

            MessageBox.Show("No se encontró el archivo de ayuda de RRHH.", "Ayuda",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
