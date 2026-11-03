using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Acciones comunes de las ventanas de Evaluación de Desempeño:
 *  imprimir lo que se ve en la ventana y abrir la ayuda del módulo.
 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Utilidades
{
    public static class ClsAccionesVista
    {
        private const string _Ayuda = @"ayuda\modulos\rrhh\RrhhAyudas\Ayudas_EvaluacionDesempeno.html";

        // Toma una imagen de la ventana y la muestra en vista previa de impresión, ajustada a la hoja.
        public static void RrhhMetImprimir(Form Ventana)
        {
            Bitmap Captura = new Bitmap(Ventana.ClientSize.Width, Ventana.ClientSize.Height);
            Ventana.DrawToBitmap(Captura, new Rectangle(Point.Empty, Ventana.ClientSize));

            using (var Documento = new PrintDocument())
            using (var Vista = new PrintPreviewDialog())
            {
                Documento.DocumentName = Ventana.Text;
                Documento.DefaultPageSettings.Landscape = true;
                Documento.PrintPage += (Remitente, Evento) =>
                {
                    Rectangle Area = Evento.MarginBounds;
                    float Escala = Math.Min((float)Area.Width / Captura.Width, (float)Area.Height / Captura.Height);
                    Evento.Graphics.DrawImage(Captura, Area.X, Area.Y, Captura.Width * Escala, Captura.Height * Escala);
                };
                Vista.Document = Documento;
                Vista.WindowState = FormWindowState.Maximized;
                Vista.ShowDialog(Ventana);
            }
            Captura.Dispose();
        }

        // La ayuda está en la carpeta "ayuda" del repositorio: se busca subiendo desde el ejecutable.
        public static void RrhhMetMostrarAyuda(Form Ventana)
        {
            DirectoryInfo Carpeta = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (Carpeta != null)
            {
                string Ruta = Path.Combine(Carpeta.FullName, _Ayuda);
                if (File.Exists(Ruta))
                {
                    Process.Start(Ruta);
                    return;
                }
                Carpeta = Carpeta.Parent;
            }

            MessageBox.Show(Ventana, "No se encontró el archivo de ayuda de Evaluación de Desempeño.",
                "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
