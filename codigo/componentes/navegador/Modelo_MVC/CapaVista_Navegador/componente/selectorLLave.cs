using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Navegador;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    // Si el driver ODBC no detecta la llave primaria, la pregunta una vez y la recuerda por tabla
    public class selectorLlave
    {
        private Form formulario;
        private ctrlEsquema ctrlEsquema = new ctrlEsquema();
        private Dictionary<string, List<string>> clavesManualesPorTabla = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public selectorLlave(Form formulario)
        {
            this.formulario = formulario;
        }

        public List<columnaInfo> ObtenerEsquemaConLlaves(string tabla)
        {
            List<columnaInfo> esquema = ctrlEsquema.ObtenerEsquemaTabla(tabla);

            if (esquema.Exists(c => c.EsPK))
                return esquema;

            List<string> elegidas;

            if (!clavesManualesPorTabla.TryGetValue(tabla, out elegidas))
            {
                List<string> nombres = esquema.ConvertAll(c => c.Nombre);

                elegidas = MostrarSelector(
                    "No se pudo detectar automáticamente la llave primaria de '" + tabla + "'.\nSeleccione la o las columnas:",
                    nombres);

                clavesManualesPorTabla[tabla] = elegidas;
            }

            foreach (columnaInfo col in esquema)
            {
                if (elegidas.Contains(col.Nombre, StringComparer.OrdinalIgnoreCase))
                    col.EsPK = true;
            }

            return esquema;
        }

        private List<string> MostrarSelector(string mensaje, List<string> opciones)
        {
            List<string> seleccion = new List<string>();

            using (Form dialogo = new Form())
            {
                dialogo.Text = "Definir llave primaria";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.Width = 380;
                dialogo.Height = 420;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;

                Label lbl = new Label();
                lbl.Text = mensaje;
                lbl.Location = new Point(10, 10);
                lbl.Size = new Size(340, 40);
                dialogo.Controls.Add(lbl);

                CheckedListBox clb = new CheckedListBox();
                clb.Location = new Point(10, 55);
                clb.Size = new Size(340, 260);

                foreach (string opcion in opciones)
                    clb.Items.Add(opcion);

                dialogo.Controls.Add(clb);

                Button btnOk = new Button();
                btnOk.Text = "Aceptar";
                btnOk.Location = new Point(190, 325);
                btnOk.DialogResult = DialogResult.OK;
                dialogo.Controls.Add(btnOk);
                dialogo.AcceptButton = btnOk;

                if (dialogo.ShowDialog(formulario) == DialogResult.OK)
                {
                    foreach (object item in clb.CheckedItems)
                        seleccion.Add(item.ToString());
                }
            }

            return seleccion;
        }
    }
}