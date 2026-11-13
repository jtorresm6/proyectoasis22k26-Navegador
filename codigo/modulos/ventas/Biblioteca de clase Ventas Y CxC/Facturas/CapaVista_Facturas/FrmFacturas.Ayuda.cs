using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
namespace CapaVista_Facturas
{
    public partial class FrmFacturas
    {
        private void VentasMetMostrarAyudaFacturas()
        {
            VentasMetEjecutar(()=>{
                byte[] Contenido;
                using(var Recurso=typeof(FrmFacturas).Assembly.GetManifestResourceStream("CapaVista_Facturas.Ayudas.Facturas.chm"))
                using(var Memoria=new MemoryStream())
                {
                    if(Recurso==null)throw new FileNotFoundException("No se encontró la guía de Facturas integrada.");
                    Recurso.CopyTo(Memoria);Contenido=Memoria.ToArray();
                }
                string Hash;using(var Sha=SHA256.Create())Hash=BitConverter.ToString(Sha.ComputeHash(Contenido)).Replace("-","");
                string Carpeta=Path.Combine(Path.GetTempPath(),"EmbutidosCalidad","AyudaFacturas");Directory.CreateDirectory(Carpeta);
                string Ruta=Path.Combine(Carpeta,"Facturas-"+Hash+".chm");
                if(!File.Exists(Ruta))File.WriteAllBytes(Ruta,Contenido);
                Help.ShowHelp(this,Ruta,HelpNavigator.Topic,"General.html");
            });
        }
        protected override bool ProcessCmdKey(ref Message Mensaje,Keys Tecla)
        {
            if(Tecla==Keys.F1){VentasMetMostrarAyudaFacturas();return true;}
            return base.ProcessCmdKey(ref Mensaje,Tecla);
        }
    }
}
