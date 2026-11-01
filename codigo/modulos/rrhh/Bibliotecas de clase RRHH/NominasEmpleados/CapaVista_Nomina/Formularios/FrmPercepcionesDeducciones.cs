using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Nomina.Formularios
{
    public partial class FrmPercepcionesDeducciones : Form
    {
        public FrmPercepcionesDeducciones()
        {
              InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RrhhMetAjustarAPantalla();
        }

        // en pantallas de 1366x768 la ventana se reduce para que quepa completa
        private void RrhhMetAjustarAPantalla()
        {
            Rectangle Area = Screen.FromControl(this).WorkingArea;
            double Factor = Math.Min(1d, Math.Min((double)Area.Width / Width, (double)Area.Height / Height));
            if (Factor >= 1d)
                return;

            List<KeyValuePair<Control, Font>> Fuentes = new List<KeyValuePair<Control, Font>>();
            RrhhMetGuardarFuentes(this, Fuentes);

            AutoScaleMode = AutoScaleMode.None;
            Scale(new SizeF((float)Factor, (float)Factor));
            foreach (KeyValuePair<Control, Font> Par in Fuentes)
                Par.Key.Font = RrhhMetEscalarFuente(Par.Value, Factor);

            foreach (DataGridView Tabla in Fuentes.Select(Par => Par.Key).OfType<DataGridView>())
            {
                Tabla.ColumnHeadersDefaultCellStyle.Font = RrhhMetEscalarFuente(Tabla.ColumnHeadersDefaultCellStyle.Font, Factor);
                Tabla.DefaultCellStyle.Font = RrhhMetEscalarFuente(Tabla.DefaultCellStyle.Font, Factor);
            }

            if (Owner != null)
                CenterToParent();
            else
                CenterToScreen();
        }

        private void RrhhMetGuardarFuentes(Control Padre, List<KeyValuePair<Control, Font>> Fuentes)
        {
            Fuentes.Add(new KeyValuePair<Control, Font>(Padre, Padre.Font));
            foreach (Control Hijo in Padre.Controls)
                RrhhMetGuardarFuentes(Hijo, Fuentes);
        }

        private Font RrhhMetEscalarFuente(Font Original, double Factor)
        {
            if (Original == null)
                return null;
            return new Font(Original.FontFamily, (float)(Original.Size * Factor), Original.Style, Original.Unit);
        }

        private void FrmNominas_Load(object sender, EventArgs e)
        {

        }

        private void NominaBtnEliminar_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
