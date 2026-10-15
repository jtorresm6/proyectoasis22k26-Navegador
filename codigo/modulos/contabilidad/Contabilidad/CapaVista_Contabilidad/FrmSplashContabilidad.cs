/*
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * Propósito : Mostrar el progreso de carga, abrir el MDI y cerrar la sesión al regresar.
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CapaVista_Contabilidad
{
    public partial class FrmSplashContabilidad : Form
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr Ventana, string NombreAplicacion, string ListaIdentificadores);

        private int _Progreso;

        public FrmSplashContabilidad()
        {
            InitializeComponent();
            ContabilidadMetAplicarColorBarra();
            ContabilidadMetIniciarCarga();
        }

        private void ContabilidadMetAplicarColorBarra()
        {
            SetWindowTheme(ContabilidadPgbCarga.Handle, "", "");
            ContabilidadPgbCarga.ForeColor = Color.FromArgb(197, 143, 62);
        }

        private void ContabilidadMetIniciarCarga()
        {
            _Progreso = 0;
            _TimerCarga.Start();
        }

        private void ContabilidadMetActualizarCarga(object Remitente, EventArgs Evento)
        {
            _Progreso += 1;
            if (_Progreso < 100) ContabilidadPgbCarga.Value = _Progreso + 1;
            ContabilidadPgbCarga.Value = _Progreso;
            ContabilidadLblPorcentaje.Text = _Progreso + "%";

            if (_Progreso >= 100)
            {
                _TimerCarga.Stop();
                Hide();
                using (FrmMDIContabilidad Principal = new FrmMDIContabilidad())
                {
                    Principal.ShowDialog();
                }
                new CapaControlador_Contabilidad.ClsControladorLoginContabilidad().ContabilidadMetCerrarSesion();
                Close();
            }
        }
    }
}
