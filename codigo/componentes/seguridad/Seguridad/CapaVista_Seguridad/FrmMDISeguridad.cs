using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Seguridad.Ayudas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CapaVista_Seguridad
{
    public partial class FrmMDISeguridad : Form
    {
        public FrmMDISeguridad()
        {
            InitializeComponent();
            this.Load += FrmMDISeguridad_Load;
        }

        //---------------------------------------------------------------------**INICIO ANDRE Y EVELYN
        /*
 * ==================================================================
 * Área : Seguridad
 * Autores : André De Jesús Gonzalez Camey
 *           Evelyn Sofia Andrade Luna
 * Fecha : 23/09/2026
 * ==================================================================
 * Propósito :
 * Se agregó la lógica para que al cargar el formulario MDI,
 * los botones del menú lateral que corresponden a módulos
 * a los que el usuario no tiene acceso se muestren en escala
 * de grises, utilizando el método
 * SeguridadMetAplicarPermisosEnBotonesMDI del helper.
 * ===================================================================
*/
        private void FrmMDISeguridad_Load(object sender, EventArgs e)
        {
            SeguridadMetActualizarInfoUsuario();
            SeguridadMetCargarKPIs();

            var MapaBotonesMDI = new Dictionary<Control, (int, int)>
            {
                { SeguridadBtnEmpleados,(4, 4)},
                { SeguridadBtnUsuarios,(4, 5)},
                { SeguridadBtnModulos,(4, 6)},
                { SeguridadBtnAplicaciones,(4, 7)},
                { SeguridadBtnPerfiles,(4, 8)},
                { SeguridadBtnAsignaPerfiles,(4, 9)},
                { SeguridadBtnAplicaPerfiles,(4, 10)},
                { SeguridadBtnAplicaUsuario,(4, 11)},
                { SeguridadBtnBitacora,(4, 12)}
            };
            ClsSeguridadFormHelper.SeguridadMetAplicarPermisosEnBotonesMDI(MapaBotonesMDI);
        }
        //----------------------------------------------------------**FIN CODIGO ANDRE Y EVELYN

        private void SeguridadMetActualizarInfoUsuario()
        {
            SeguridadLblUsuario.Text = $"Usuario: {ClsSesionSeguridad.NombreEmpleado}";
            SeguridadLblUsuarioRol.Text = $"Rol: {ClsSesionSeguridad.SeguridadMetRolesComoTexto()}";
        }

        private void SeguridadMetCargarKPIs()
        {
            try
            {
                var Dashboard = new ClsModeloDashboard();
                SeguridadLblKPIResp1.Text = Dashboard.SeguridadMetUsuarios().ToString();
                SeguridadLblKPIResp2.Text = Dashboard.SeguridadMetAplicaciones().ToString();
                SeguridadLblKPIResp3.Text = Dashboard.SeguridadMetPerfiles().ToString();
                SeguridadLblKPIResp4.Text = Dashboard.SeguridadMetModulos().ToString();
                SeguridadLblKPIResp5.Text = Dashboard.SeguridadMetBitacora().ToString();
                SeguridadLblKPIResp6.Text = Dashboard.SeguridadMetAsignaciones().ToString();
            }
            catch { }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            int Radio = 20;
            SeguridadPnlDashboard.Region = new Region(SeguridadMetRedondearEsquinas(SeguridadPnlDashboard.ClientRectangle, Radio));
        }

        private GraphicsPath SeguridadMetRedondearEsquinas(Rectangle Rectangulo, int Radio)
        {
            GraphicsPath RutaGrafica = new GraphicsPath();
            int Diametro = Radio * 2;

            RutaGrafica.AddArc(Rectangulo.X, Rectangulo.Y, Diametro, Diametro, 180, 90);
            RutaGrafica.AddArc(Rectangulo.Right - Diametro, Rectangulo.Y, Diametro, Diametro, 270, 90);
            RutaGrafica.AddArc(Rectangulo.Right - Diametro, Rectangulo.Bottom - Diametro, Diametro, Diametro, 0, 90);
            RutaGrafica.AddArc(Rectangulo.X, Rectangulo.Bottom - Diametro, Diametro, Diametro, 90, 90);
            RutaGrafica.CloseFigure();

            return RutaGrafica;
        }

        /*
 * ==================================================================
 * Área : Seguridad
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 22/09/2026
 * ==================================================================
 * Propósito :
 * Aqui realice la accion de que si el usuario tiene acceso se 
 * realizara el llamado a la ventana de lo contrario se mostrara
 * el mensaje.
 * ===================================================================
*/
        private void button10_Click(object sender, EventArgs e)
        {
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 10))
            {
                MessageBox.Show("No tienes acceso a este módulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmAsignacionAppPerf Perfil = new FrmAsignacionAppPerf();
            Perfil.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 12))
            {
                MessageBox.Show("No tienes acceso a este módulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmBitacora Bitacora = new FrmBitacora();
            Bitacora.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 11))
            {
                MessageBox.Show("No tienes acceso a este módulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmAsignacionAplicacionUsuario AsigAplicacionUsuario = new FrmAsignacionAplicacionUsuario();
            AsigAplicacionUsuario.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 9))
            {
                MessageBox.Show("No tienes acceso a este módulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FrmAsignacionPerfiles AsignacionPerfiles = new FrmAsignacionPerfiles();
            AsignacionPerfiles.ShowDialog();
        }

        private void SeguridadBtnPerfiles_Click(object sender, EventArgs e)
        {
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 8))
            {
                MessageBox.Show("No tienes acceso a este módulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmMantenimientoPerfiles MantenimientoPerfiles = new FrmMantenimientoPerfiles();
            MantenimientoPerfiles.ShowDialog();
        }

        private void SeguridadBtnUsuarios_Click(object sender, EventArgs e)
        {
            FrmMantenimientoUsuarios Usuarios = new FrmMantenimientoUsuarios();
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 5))
            {
                MessageBox.Show("No tienes acceso a este módulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Usuarios.ShowDialog();
        }

        private void SeguridadBtnModulos_Click(object sender, EventArgs e)
        {
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 6))
            {
                MessageBox.Show("No tienes acceso a este módulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SeguridadFrmModulo Modulo = new SeguridadFrmModulo();
            Modulo.ShowDialog();
        }

        private void SeguridadBtnEmpleados_Click(object sender, EventArgs e)
        {
            //if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 4))
            //{
            //    MessageBox.Show("No tienes acceso a este módulo.",
            //        "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

               FrmEmpleadosNavegador Empleados = new FrmEmpleadosNavegador();
            Empleados.ShowDialog();
        }

        private void SeguridadBtnAplicaciones_Click(object sender, EventArgs e)
        {
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(IdModulo: 4, IdAplicacion: 7))
            {
                MessageBox.Show("No tienes acceso a este módulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmMantenimientoAplicacion Aplicaciones = new FrmMantenimientoAplicacion();
            Aplicaciones.ShowDialog();
        }
        /*
         * Fin de codigo de Carlos David Calderón Ramirez
        */

        private void SeguridadBtnBurger_Click(object sender, EventArgs e)
        {
            if (SeguridadPnlNavegador.Width == 270)
            {
                SeguridadPnlNavegador.Width = 64;
                SeguridadPnlDashboard.Location = new Point(200, 52);
                SeguridadBtnBurger.Location = new Point(220, 13);
                SeguridadLblUsuario.Location = new Point(285, 19);
                SeguridadLblUsuarioRol.Location = new Point(285, 39);
            }
            else
            {
                SeguridadPnlNavegador.Width = 270;
                SeguridadPnlDashboard.Location = new Point(307, 52);
                SeguridadBtnBurger.Location = new Point(323, 13);
                SeguridadLblUsuario.Location = new Point(390, 19);
                SeguridadLblUsuarioRol.Location = new Point(390, 39);
            }
        }

        private void SeguridadBtnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                 "¿Seguro que deseas cerrar sesión?",
                 "Cerrar sesión",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void SeguridadBtnAyudas_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "C:/SeguridadAyudas/SeguridadAyudas.chm", "MDI_Seguridad.html");
        }
    }
}