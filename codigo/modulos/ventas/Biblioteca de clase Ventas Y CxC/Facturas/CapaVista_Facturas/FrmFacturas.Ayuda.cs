using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
namespace CapaVista_Facturas
{
    public partial class FrmFacturas
    {
        private void MostrarAyudaFacturas()
        {
            Ejecutar(()=>{
                byte[] contenido;
                using(var recurso=typeof(FrmFacturas).Assembly.GetManifestResourceStream("CapaVista_Facturas.Ayudas.Facturas.chm"))
                using(var memoria=new MemoryStream())
                {
                    if(recurso==null)throw new FileNotFoundException("No se encontró la guía de Facturas integrada.");
                    recurso.CopyTo(memoria);contenido=memoria.ToArray();
                }
                string hash;using(var sha=SHA256.Create())hash=BitConverter.ToString(sha.ComputeHash(contenido)).Replace("-","");
                string carpeta=Path.Combine(Path.GetTempPath(),"EmbutidosCalidad","AyudaFacturas");Directory.CreateDirectory(carpeta);
                string ruta=Path.Combine(carpeta,"Facturas-"+hash+".chm");
                if(!File.Exists(ruta))File.WriteAllBytes(ruta,contenido);
                Help.ShowHelp(this,ruta,HelpNavigator.Topic,"General.html");
            });
        }
        protected override bool ProcessCmdKey(ref Message mensaje,Keys tecla)
        {
            if(tecla==Keys.F1){MostrarAyudaFacturas();return true;}
            return base.ProcessCmdKey(ref mensaje,tecla);
        }
    }
}
