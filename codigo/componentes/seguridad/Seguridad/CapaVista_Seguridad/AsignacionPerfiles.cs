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

namespace AplicacionPerfiles
{
    public partial class AsignacionPerfiles : Form
    {
        public AsignacionPerfiles()
        {
            InitializeComponent();
        }

        // Helper para dar esquinas redondeadas a los paneles (mismo patrón usado en el módulo de Seguridad)
        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            panelHeader.Region = new Region(GetRoundedRect(panelHeader.ClientRectangle, 18));
        }

        private void panelConsulta_Paint(object sender, PaintEventArgs e)
        {
            panelConsulta.Region = new Region(GetRoundedRect(panelConsulta.ClientRectangle, 18));
        }

        private void panelAsignacion_Paint(object sender, PaintEventArgs e)
        {
            panelAsignacion.Region = new Region(GetRoundedRect(panelAsignacion.ClientRectangle, 18));
        }

        private void panelIconPrincipal_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panelIconConsulta_Paint(object sender, PaintEventArgs e)
        {
            panelIconConsulta.Region = new Region(GetRoundedRect(panelIconConsulta.ClientRectangle, 10));
        }

        private void panelIconAsignacion_Paint(object sender, PaintEventArgs e)
        {
            panelIconAsignacion.Region = new Region(GetRoundedRect(panelIconAsignacion.ClientRectangle, 10));
        }

        // ---- Prototipo NO funcional: los siguientes eventos están vacíos a propósito ----

        private void buttonCancelarConsulta_Click(object sender, EventArgs e)
        {

        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {

        }

        private void buttonCancelarAsignacion_Click(object sender, EventArgs e)
        {

        }

        private void buttonAsignar_Click(object sender, EventArgs e)
        {

        }
    }
}
