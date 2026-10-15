/*
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * Propósito : Validar las credenciales y abrir la presentación inicial de Contabilidad.
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CapaControlador_Contabilidad;
using CapaVista_Contabilidad.Componentes;

namespace CapaVista_Contabilidad
{
    public partial class FrmLoginContabilidad : Form
    {
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();
        public FrmLoginContabilidad()
        {
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetAplicarTipografia(this);
            ContabilidadTxtUsuario.Font = new Font("Segoe UI", 10F);
            ContabilidadTxtContrasena.Font = new Font("Segoe UI", 10F);
            Icon = (System.Drawing.Icon)new System.ComponentModel.ComponentResourceManager(typeof(FrmMDIContabilidad)).GetObject("$this.Icon");
            _Validacion.ContabilidadMetObligatorio(ContabilidadTxtUsuario);
            _Validacion.ContabilidadMetObligatorio(ContabilidadTxtContrasena);
            Disposed += (Remitente, Argumentos) => _Validacion.Dispose();
        }

        private void ContabilidadMetRedondearEsquinas(Control Control, int Radio)
        {
            Rectangle Limites = Control.ClientRectangle;
            int Diametro = Radio * 2;
            using (GraphicsPath RutaGrafica = new GraphicsPath())
            {
                RutaGrafica.AddArc(Limites.X, Limites.Y, Diametro, Diametro, 180, 90);
                RutaGrafica.AddArc(Limites.Right - Diametro, Limites.Y, Diametro, Diametro, 270, 90);
                RutaGrafica.AddArc(Limites.Right - Diametro, Limites.Bottom - Diametro, Diametro, Diametro, 0, 90);
                RutaGrafica.AddArc(Limites.X, Limites.Bottom - Diametro, Diametro, Diametro, 90, 90);
                RutaGrafica.CloseFigure();
                Region RegionAnterior = Control.Region;
                Control.Region = new Region(RutaGrafica);
                if (RegionAnterior != null) RegionAnterior.Dispose();
            }
        }

        private void ContabilidadMetPintarPanel(object Remitente, PaintEventArgs Evento)
        {
            ContabilidadMetRedondearEsquinas(ContabilidadPnlInterfazLogin, 20);
        }

        private void ContabilidadMetIniciarSesion(object Remitente, EventArgs Evento)
        {
            bool UsuarioValido = _Validacion.ContabilidadFuncValidar(ContabilidadTxtUsuario,
                string.IsNullOrWhiteSpace(ContabilidadTxtUsuario.Text) ? "Ingrese su usuario." : null);
            bool ContrasenaValida = _Validacion.ContabilidadFuncValidar(ContabilidadTxtContrasena,
                string.IsNullOrEmpty(ContabilidadTxtContrasena.Text) ? "Ingrese su contraseña." : null);
            if (!UsuarioValido || !ContrasenaValida) return;
            try
            {
                ClsControladorLoginContabilidad Controlador = new ClsControladorLoginContabilidad();
                if (Controlador.ContabilidadFuncIniciarSesion(ContabilidadTxtUsuario.Text, ContabilidadTxtContrasena.Text))
                {
                    Hide();
                    using (FrmSplashContabilidad Splash = new FrmSplashContabilidad())
                    {
                        Splash.ShowDialog();
                    }
                    ContabilidadTxtContrasena.Clear();
                    Show();
                }
                else
                {
                    ClsDialogosContabilidad.ContabilidadMetError(this, "Usuario o contraseña incorrectos.");
                    ContabilidadTxtContrasena.Clear();
                    ContabilidadTxtContrasena.Focus();
                }
            }
            catch (Exception Excepcion)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudo iniciar sesión: " + Excepcion.Message);
            }
        }

        private void ContabilidadMetMostrarContrasena(object Remitente, EventArgs Evento)
        {
            ContabilidadTxtContrasena.PasswordChar = ContabilidadChkMostrarContra.Checked ? '\0' : '*';
        }
    }
}
