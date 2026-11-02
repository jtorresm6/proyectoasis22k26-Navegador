using CapaControlador_Seguridad.Objetos_de_valor;
//using CapaVista_Bancos.Formularios;
//using CapaVista_Rrhh.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaVista_Logistica;
//using CapaVista_Ventas;

/*
 * ============================================================
 * Área         : Seguridad
 * Autor        : Evelyn Sofía Andrade Luna
 * Carné        : 9959-23-1224
 * Fecha        : 24/09/2026
 * ============================================================
 * Propósito    :
 *   Formulario de presentación inicial del componente de Seguridad.
 *   Muestra una barra de progreso animada mientras se cargan
 *   los recursos del sistema. Al completarse la carga al 100%,
 *   cierra el splash y abre el formulario principal MDI.
 * ============================================================
 */

namespace CapaVista_Seguridad
{
    public partial class FrmSplash : Form
    {
        [System.Runtime.InteropServices.DllImport("uxtheme.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        private int _Progreso;
        private readonly string _Destino;

        public FrmSplash(string destino)
        {
            InitializeComponent();
            _Destino = destino;
            SeguridadMetAplicarColorBarra();
            SeguridadMetIniciarCarga();
        }

        private void SeguridadMetAplicarColorBarra()
        {
            SetWindowTheme(SeguridadPgbCarga.Handle, "", "");
            SeguridadPgbCarga.ForeColor = ColorTranslator.FromHtml("#006D77");
        }

        private void SeguridadMetIniciarCarga()
        {
            _Progreso = 0;
            _TimerCarga.Start();
        }

        private void _TimerCarga_Tick(object sender, EventArgs e)
        {
            _Progreso += 1;
            if (_Progreso < 100) SeguridadPgbCarga.Value = _Progreso + 1;
            SeguridadPgbCarga.Value = _Progreso;
            SeguridadLblPorcentaje.Text = _Progreso + "%";

            if (_Progreso >= 100)
            {
                _TimerCarga.Stop();
                this.Hide();

                Form principal = null;
                switch (_Destino)
                {
                    case "Seguridad":
                        principal = new FrmMDISeguridad();
                        break;
                    
                    case "Logistica":
                        var MDILogistica = new FrmMDILogistica();
                        MDILogistica.LogisticaMetActualizarInfoUsuario(
                        ClsSesionSeguridad.NombreEmpleado,
                        ClsSesionSeguridad.SeguridadMetRolesComoTexto());
                        principal = MDILogistica;
                        break;

                    //case "Compras":
                        //var MDICompras = new FrmMDICompras();
                        //MDICompras.ComprasMetActualizarInfoUsuario(
                        //ClsSesionSeguridad.NombreEmpleado,
                        //ClsSesionSeguridad.SeguridadMetRolesComoTexto());
                        //principal = MDICompras;
                        //break;

                 // case "Ventas":
                   //  var mdiVentas = new CapaVista_Ventas.FrmMdiVentas(
                     //     ClsSesionSeguridad.NombreEmpleado,
                       //     ClsSesionSeguridad.SeguridadMetRolesComoTexto(), ClsSesionSeguridad.IdUsuario);
                      //     principal = mdiVentas;
                       // break;

                    //case "Bancos":
                        //var MdiBancos = new FrmMDIBancos();
                        //MdiBancos.BancosMetActualizarInfoUsuario(
                        //ClsSesionSeguridad.NombreEmpleado,
                        //ClsSesionSeguridad.SeguridadMetRolesComoTexto());
                        //principal = MdiBancos;
                        //break;

                    //case "Contabilidad":
                        //var MDIContabilidad = new FrmMDIContabilidad();
                        //MDIContabilidad.ContabilidadMetActualizarInfoUsuario(
                        //ClsSesionSeguridad.NombreEmpleado,
                        //ClsSesionSeguridad.SeguridadMetRolesComoTexto());
                        //principal = MDIContabilidad;
                        //break;

                    //case "RRHH":
                        //var MdiRrhh = new FrmMDIRrhh();
                        //MdiRrhh.RrhhMetActualizarInfoUsuario(
                        //ClsSesionSeguridad.NombreEmpleado,
                        //ClsSesionSeguridad.SeguridadMetRolesComoTexto());
                        //principal = MdiRrhh;
                        //break;

                    default:
                        principal = new FrmMDISeguridad();
                        break;
                }

                principal.ShowDialog();
                this.Close();
            }
        }
    }
}
