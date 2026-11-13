using System.Windows.Forms;

namespace CapaVista_Componentes.Componentes
{
    public static class ClsReportesCompras
    {
        public static string ObtenerRuta(Control Vista, string Nombre)
        {
            return ClsArchivosEncabezadoCompras.BuscarDesdeVista(Vista, Nombre, "Reportes", ".rdlc");
        }

        public static string ObtenerRuta(string Nombre)
        {
            return ClsArchivosEncabezadoCompras.BuscarDesdeVista(null, Nombre, "Reportes", ".rdlc");
        }
    }
}
