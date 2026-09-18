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
    public class ClsSelectorLlave
    {
        private Form _Formulario;
        private ClsCtrlEsquema _CtrlEsquema = new ClsCtrlEsquema();
        private Dictionary<string, List<string>> _ClavesManualesPorTabla = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public ClsSelectorLlave(Form Formulario)
        {
            this._Formulario = Formulario;
        }

        public List<ClsColumnaInfo> NavegadorFuncObtenerEsquemaConLlaves(string Tabla)
        {
            List<ClsColumnaInfo> Esquema = _CtrlEsquema.NavegadorFuncObtenerEsquemaTabla(Tabla);

            if (Esquema.Exists(Columna => Columna.EsPK))
                return Esquema;

            List<string> Elegidas;

            if (!_ClavesManualesPorTabla.TryGetValue(Tabla, out Elegidas))
            {
                List<string> Nombres = Esquema.ConvertAll(Columna => Columna.Nombre);

                Elegidas = NavegadorFuncMostrarSelector(
                    "No se pudo detectar automáticamente la llave primaria de '" + Tabla + "'.\nSeleccione la o las columnas:",
                    Nombres);

                _ClavesManualesPorTabla[Tabla] = Elegidas;
            }

            foreach (ClsColumnaInfo Columna in Esquema)
            {
                if (Elegidas.Contains(Columna.Nombre, StringComparer.OrdinalIgnoreCase))
                    Columna.EsPK = true;
            }

            return Esquema;
        }

        private List<string> NavegadorFuncMostrarSelector(string Mensaje, List<string> Opciones)
        {
            List<string> Seleccion = new List<string>();

            using (Form Dialogo = new Form())
            {
                Dialogo.Text = "Definir llave primaria";
                Dialogo.StartPosition = FormStartPosition.CenterParent;
                Dialogo.Width = 380;
                Dialogo.Height = 420;
                Dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                Dialogo.MinimizeBox = false;
                Dialogo.MaximizeBox = false;

                Label NavegadorLblMensaje = new Label();
                NavegadorLblMensaje.Name = "NavegadorLblMensaje";
                NavegadorLblMensaje.Text = Mensaje;
                NavegadorLblMensaje.Location = new Point(10, 10);
                NavegadorLblMensaje.Size = new Size(340, 40);
                Dialogo.Controls.Add(NavegadorLblMensaje);

                CheckedListBox NavegadorClbOpciones = new CheckedListBox();
                NavegadorClbOpciones.Name = "NavegadorClbOpciones";
                NavegadorClbOpciones.Location = new Point(10, 55);
                NavegadorClbOpciones.Size = new Size(340, 260);

                foreach (string Opcion in Opciones)
                    NavegadorClbOpciones.Items.Add(Opcion);

                Dialogo.Controls.Add(NavegadorClbOpciones);

                Button NavegadorBtnAceptar = new Button();
                NavegadorBtnAceptar.Name = "NavegadorBtnAceptar";
                NavegadorBtnAceptar.Text = "Aceptar";
                NavegadorBtnAceptar.Location = new Point(190, 325);
                NavegadorBtnAceptar.DialogResult = DialogResult.OK;
                Dialogo.Controls.Add(NavegadorBtnAceptar);
                Dialogo.AcceptButton = NavegadorBtnAceptar;

                if (Dialogo.ShowDialog(_Formulario) == DialogResult.OK)
                {
                    foreach (object Item in NavegadorClbOpciones.CheckedItems)
                        Seleccion.Add(Item.ToString());
                }
            }

            return Seleccion;
        }
    }
}