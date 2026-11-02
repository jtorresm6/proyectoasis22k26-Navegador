using System.Drawing;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Miguel David Contreras Jacinto
 * Carné : 0901-21-3878
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Aplica la presentación y el comportamiento compartido a los selectores de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    internal sealed class ComprasFiltroRueda : IMessageFilter
    {
        [System.ThreadStatic]
        private static ComprasFiltroRueda _Instancia;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr WindowFromPoint(Point Punto);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr SendMessage(System.IntPtr Ventana, int Mensaje,
            System.IntPtr WParam, System.IntPtr LParam);

        internal static void EnviarAlContenedor(Control Entrada, ref Message Mensaje)
        {
            Control Contenedor = Entrada.Parent;
            while (Contenedor != null && ObtenerEntrada(Contenedor) != null)
                Contenedor = Contenedor.Parent;
            Control Destino = Contenedor;
            while (Contenedor != null)
            {
                var Desplazable = Contenedor as ScrollableControl;
                if (Desplazable != null && (Mensaje.Msg == 0x020A
                    ? Desplazable.VerticalScroll.Visible : Desplazable.HorizontalScroll.Visible))
                {
                    Destino = Contenedor;
                    break;
                }
                Contenedor = Contenedor.Parent;
            }
            Mensaje.Result = Destino != null && !Destino.IsDisposed
                ? SendMessage(Destino.Handle, Mensaje.Msg, Mensaje.WParam, Mensaje.LParam)
                : System.IntPtr.Zero;
        }

        internal static void Instalar()
        {
            if (_Instancia != null || System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime) return;
            _Instancia = new ComprasFiltroRueda();
            Application.AddMessageFilter(_Instancia);
        }

        private static bool BloquearRueda(Control Actual)
        {
            while (Actual != null)
            {
                var Combo = Actual as ComprasUsrComboBox;
                if (Combo != null) return !Combo.PermitirRuedaMouse;
                var Texto = Actual as ComprasUsrTextBox;
                if (Texto != null) return !Texto.PermitirRuedaMouse;
                Actual = Actual.Parent;
            }
            return true;
        }

        internal static bool DebeBloquear(Control Entrada) => BloquearRueda(Entrada);

        private static Control ObtenerEntrada(Control Actual)
        {
            while (Actual != null)
            {
                if (Actual is ClsSelectorCompras || Actual is ComprasRichTextBox ||
                    Actual is ComprasUsrComboBox || Actual is ComprasUsrTextBox) return Actual;
                Actual = Actual.Parent;
            }
            return null;
        }

        public bool PreFilterMessage(ref Message Mensaje)
        {
            if (Mensaje.Msg != 0x020A && Mensaje.Msg != 0x020E) return false;
            Control BajoPuntero = ObtenerEntrada(Control.FromChildHandle(WindowFromPoint(Cursor.Position)));
            Control Destino = ObtenerEntrada(Control.FromChildHandle(Mensaje.HWnd));
            if (Destino == null) return false;
            Control Entrada = BajoPuntero ?? Destino;
            if (!BloquearRueda(Entrada) && !BloquearRueda(Destino)) return false;
            EnviarAlContenedor(Entrada, ref Mensaje);
            return true;
        }
    }

    public partial class ClsSelectorCompras : ComboBox
    {

        public ClsSelectorCompras()
        {
            ComprasFiltroRueda.Instalar();
            Font = new Font("Segoe UI",10F,FontStyle.Regular,GraphicsUnit.Point);
            ForeColor = ClsTemaCompras.PrincipalNavegador;
            BackColor = ClsTemaCompras.FondoGeneral;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;
            IntegralHeight = true;
            MaxDropDownItems = 8;
            Margin = new Padding(3);
        }

        protected override Size DefaultSize =>
            new Size(200, 27);

        protected override void WndProc(ref Message Mensaje)
        {
            if ((Mensaje.Msg == 0x020A || Mensaje.Msg == 0x020E) && ComprasFiltroRueda.DebeBloquear(this))
            {
                ComprasFiltroRueda.EnviarAlContenedor(this, ref Mensaje);
                return;
            }
            base.WndProc(ref Mensaje);
        }
    }


}
