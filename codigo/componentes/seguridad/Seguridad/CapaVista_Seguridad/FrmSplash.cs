using CapaControlador_Seguridad.Objetos_de_valor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        // Los modulos dependen de Seguridad; se cargan al abrirlos para evitar
        // que Seguridad tenga que referenciarlos tambien durante la compilacion.
        private static readonly Dictionary<string, string> _RutasModulos =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static bool _ResolucionRegistrada;

        private static Form SeguridadMetAbrirModulo(string ensamblado, string formulario, string actualizar)
        {
            if (!_ResolucionRegistrada)
            {
                string registro = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modulos.xml");
                if (System.IO.File.Exists(registro))
                {
                    var xml = new System.Xml.XmlDocument { XmlResolver = null };
                    xml.Load(registro);
                    foreach (System.Xml.XmlElement elemento in xml.SelectNodes("/modulos/ensamblado"))
                        _RutasModulos[elemento.GetAttribute("nombre")] = System.IO.Path.GetFullPath(
                            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, elemento.GetAttribute("ruta")));
                }
                AppDomain.CurrentDomain.AssemblyResolve += SeguridadMetResolverModulo;
                _ResolucionRegistrada = true;
            }
            var assembly = System.Reflection.Assembly.Load(ensamblado);
            var tipo = assembly.GetType(formulario, true);
            object[] argumentos = actualizar == null
                ? new object[] { ClsSesionSeguridad.NombreEmpleado, ClsSesionSeguridad.SeguridadMetRolesComoTexto(), ClsSesionSeguridad.IdUsuario }
                : new object[0];
            var ventana = (Form)Activator.CreateInstance(tipo, argumentos);
            try
            {
                if (actualizar != null)
                {
                    var metodo = tipo.GetMethod(actualizar, new[] { typeof(string), typeof(string) });
                    if (metodo == null) throw new MissingMethodException(tipo.FullName, actualizar);
                    metodo.Invoke(ventana, new object[] { ClsSesionSeguridad.NombreEmpleado, ClsSesionSeguridad.SeguridadMetRolesComoTexto() });
                }
                return ventana;
            }
            catch { ventana.Dispose(); throw; }
        }

        private static System.Reflection.Assembly SeguridadMetResolverModulo(object sender, ResolveEventArgs args)
        {
            string nombre = new System.Reflection.AssemblyName(args.Name).Name;
            string ruta;
            if (!_RutasModulos.TryGetValue(nombre, out ruta))
                ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombre + ".dll");
            return System.IO.File.Exists(ruta) ? System.Reflection.Assembly.LoadFrom(ruta) : null;
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
                try
                {
                    switch (_Destino)
                    {
                        case "Logistica": principal = SeguridadMetAbrirModulo("CapaVista_Logistica", "CapaVista_Logistica.Formularios.FrmMDILogistica", "LogisticaMetActualizarInfoUsuario"); break;
                        case "Compras": principal = SeguridadMetAbrirModulo("CapaVista_Compras", "CapaVista_Compras.FrmMDICompras", "ComprasMetActualizarInfoUsuario"); break;
                        case "Ventas": principal = SeguridadMetAbrirModulo("CapaVista_Ventas", "CapaVista_Ventas.FrmMdiVentas", null); break;
                        case "Bancos": principal = SeguridadMetAbrirModulo("CapaVista_Bancos", "CapaVista_Bancos.Formularios.FrmMDIBancos", "BancosMetActualizarInfoUsuario"); break;
                        case "Contabilidad": principal = SeguridadMetAbrirModulo("CapaVista_Contabilidad", "CapaVista_Contabilidad.FrmMDIContabilidad", "ContabilidadMetActualizarInfoUsuario"); break;
                        case "RRHH": principal = SeguridadMetAbrirModulo("CapaVista_Rrhh", "CapaVista_Rrhh.Formularios.FrmMDIRrhh", "RrhhMetActualizarInfoUsuario"); break;
                        default: principal = new FrmMDISeguridad(); break;
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("No se pudo abrir el modulo " + _Destino + ": " + error.GetBaseException().Message,
                        "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                principal.ShowDialog();
                this.Close();
            }
        }
    }
}
