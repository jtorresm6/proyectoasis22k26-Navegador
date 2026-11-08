using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CapaVista_Componentes.Componentes
{
    public sealed class ClsEditorAyudaCompras : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog Dialogo)
        {
            base.InitializeDialog(Dialogo);
            Dialogo.Title = "Seleccionar ayuda";
            Dialogo.Filter = "Ayudas CHM (*.chm)|*.chm";
            Dialogo.DefaultExt = "chm";
            Dialogo.AddExtension = true;
            Dialogo.CheckFileExists = true;
            Dialogo.Multiselect = false;
        }
    }

    public sealed class ClsEditorPaginaAyudaCompras : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog Dialogo)
        {
            base.InitializeDialog(Dialogo);
            Dialogo.Title = "Seleccionar pagina inicial de ayuda";
            Dialogo.Filter = "Paginas HTML (*.html)|*.html";
            Dialogo.DefaultExt = "html";
            Dialogo.AddExtension = true;
            Dialogo.CheckFileExists = true;
            Dialogo.Multiselect = false;
        }
    }

    public sealed class ClsEditorReporteCompras : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog Dialogo)
        {
            base.InitializeDialog(Dialogo);
            Dialogo.Title = "Seleccionar reporte";
            Dialogo.Filter = "Reportes RDLC (*.rdlc)|*.rdlc";
            Dialogo.DefaultExt = "rdlc";
            Dialogo.AddExtension = true;
            Dialogo.CheckFileExists = true;
            Dialogo.Multiselect = false;
        }
    }
}
