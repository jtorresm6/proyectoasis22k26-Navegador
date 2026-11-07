using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_Compras;
using CapaVista_Componentes.Componentes;

namespace CapaVista_Compras
{
    [ToolboxItem(false)]
    public sealed class ClsTablaNavegadorFluida : TableLayoutPanel
    {
        public ClsTablaNavegadorFluida()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }
    }

    public partial class FrmMDICompras : IMessageFilter
    {
        private Timer _AnimacionNavegador;
        private Timer _ActualizacionIndicadores;
        private readonly Stopwatch _RelojNavegador = new Stopwatch();
        private double _PosicionNavegador;
        private double _DestinoNavegador;
        private int _LimiteDesplazamiento;
        private bool _DashboardIniciado;
        private bool _ConsultandoIndicadores;
        private bool _CambiandoNavegador;
        private ToolTip _AyudaIndicadores;
        private readonly CultureInfo _CulturaIndicadores = CultureInfo.GetCultureInfo("es-GT");

        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        private static extern IntPtr ComprasFuncSuspenderRedibujado(IntPtr Ventana, int Mensaje, IntPtr Parametro, IntPtr Datos);

        [DllImport("user32.dll", EntryPoint = "RedrawWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ComprasFuncRedibujarVentana(IntPtr Ventana, IntPtr Area, IntPtr Region, uint Opciones);

        private static void ComprasMetRecopilarContenedores(Control Elemento, List<Control> Paneles)
        {
            if (!Elemento.HasChildren) return;
            Paneles.Add(Elemento);
            foreach (Control Hijo in Elemento.Controls) ComprasMetRecopilarContenedores(Hijo, Paneles);
        }

        private sealed class ClsPanelNavegadorFluido : Panel
        {
            public ClsPanelNavegadorFluido()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    var Parametros = base.CreateParams;
                    Parametros.ExStyle |= 0x02000000;
                    return Parametros;
                }
            }
        }

        private void ComprasMetIniciarDashboard()
        {
            if (_DashboardIniciado) return;
            _DashboardIniciado = true;
            if (_Componentes == null) _Componentes = new Container();
            _AnimacionNavegador = new Timer(_Componentes) { Interval = 15 };
            _AnimacionNavegador.Tick += ComprasMetAnimarNavegador;
            _ActualizacionIndicadores = new Timer(_Componentes) { Interval = 60000 };
            _ActualizacionIndicadores.Tick += (Origen, Evento) => ComprasMetSolicitarIndicadores();
            _AyudaIndicadores = new ToolTip(_Componentes) { AutoPopDelay = 15000 };
            ComprasLblKpi6.Texto = "Saldo por vencer en 7 días";
            string[] Ayudas =
            {
                "Promedio de días desde la emisión de los documentos con saldo pendiente. Cada documento tiene el mismo peso.",
                "Órdenes autorizadas o recibidas / órdenes no anuladas. El esquema no registra la fecha de autorización.",
                "Órdenes autorizadas que todavía no están recibidas.",
                "Saldos pendientes y parciales de cargos a proveedores, convertidos a GTQ con el tipo de cambio del documento.",
                "Saldo pendiente en GTQ de documentos con vencimiento anterior a hoy.",
                "Saldo pendiente en GTQ con vencimiento entre hoy y los próximos siete días, inclusive."
            };
            Control[] Tarjetas = { ComprasPnlKpi1, ComprasPnlKpi2, ComprasPnlKpi3, ComprasPnlKpi4, ComprasPnlKpi5, ComprasPnlKpi6 };
            for (int i = 0; i < Tarjetas.Length; i++) ComprasMetAsignarAyuda(Tarjetas[i], Ayudas[i]);
            Application.AddMessageFilter(this);
            Disposed += (Origen, Evento) => Application.RemoveMessageFilter(this);
            Activated += (Origen, Evento) => ComprasMetSolicitarIndicadores();
            _ActualizacionIndicadores.Start();
            ComprasMetSolicitarIndicadores();
        }

        public bool PreFilterMessage(ref Message Mensaje)
        {
            if (Mensaje.Msg != 0x020A || IsDisposed || Disposing || !Visible || !Enabled
                || _FormularioSubproceso != null || _AnimacionNavegador == null) return false;
            long Coordenadas = Mensaje.LParam.ToInt64();
            var Punto = new Point(unchecked((short)(Coordenadas & 0xffff)), unchecked((short)((Coordenadas >> 16) & 0xffff)));
            if (!_PanelDesplazamientoNavegador.RectangleToScreen(_PanelDesplazamientoNavegador.ClientRectangle).Contains(Punto)) return false;
            Form Activo = Form.ActiveForm;
            if (Activo != this) return false;
            int Delta = unchecked((short)((Mensaje.WParam.ToInt64() >> 16) & 0xffff));
            int Lineas = SystemInformation.MouseWheelScrollLines;
            double Distancia = Lineas < 0 ? _PanelDesplazamientoNavegador.ClientSize.Height : Lineas * 20.0;
            ComprasMetMoverNavegador(_DestinoNavegador - Delta * Distancia / SystemInformation.MouseWheelScrollDelta);
            return true;
        }

        private void ComprasMetMoverNavegador(double Destino)
        {
            _DestinoNavegador = Math.Max(0, Math.Min(Destino, _LimiteDesplazamiento));
            if (_AnimacionNavegador == null || Math.Abs(_DestinoNavegador - _PosicionNavegador) < 0.01) return;
            if (!_AnimacionNavegador.Enabled)
            {
                _RelojNavegador.Restart();
                _AnimacionNavegador.Start();
            }
        }

        private void ComprasMetAnimarNavegador(object Origen, EventArgs Evento)
        {
            double Segundos = _RelojNavegador.Elapsed.TotalSeconds;
            _RelojNavegador.Restart();
            double Diferencia = _DestinoNavegador - _PosicionNavegador;
            _PosicionNavegador += Diferencia * (1 - Math.Exp(-20 * Segundos));
            if (Math.Abs(_DestinoNavegador - _PosicionNavegador) < 0.5)
            {
                _PosicionNavegador = _DestinoNavegador;
                _AnimacionNavegador.Stop();
                _RelojNavegador.Stop();
            }
            int Posicion = (int)Math.Round(_PosicionNavegador);
            if (Posicion == _DesplazamientoNavegador) return;
            _DesplazamientoNavegador = Posicion;
            ComprasTlpNavegador.Top = -Posicion;
        }

        private void ComprasMetMostrarOpcionNavegador(Control Elemento)
        {
            if (!_DashboardIniciado || _CambiandoNavegador || !Elemento.Visible || Elemento == _PanelDesplazamientoNavegador || Elemento == ComprasTlpNavegador) return;
            Rectangle Limites = _PanelDesplazamientoNavegador.RectangleToClient(Elemento.RectangleToScreen(Elemento.ClientRectangle));
            if (Limites.Top < 0) ComprasMetMoverNavegador(_PosicionNavegador + Limites.Top - 8);
            else if (Limites.Bottom > _PanelDesplazamientoNavegador.ClientSize.Height)
                ComprasMetMoverNavegador(_PosicionNavegador + Limites.Bottom - _PanelDesplazamientoNavegador.ClientSize.Height + 8);
        }

        private void ComprasMetAsignarAyuda(Control Elemento, string Texto)
        {
            _AyudaIndicadores.SetToolTip(Elemento, Texto);
            foreach (Control Hijo in Elemento.Controls) ComprasMetAsignarAyuda(Hijo, Texto);
        }

        private async void ComprasMetSolicitarIndicadores()
        {
            if (!_DashboardIniciado || _ConsultandoIndicadores || IsDisposed || Disposing) return;
            _ConsultandoIndicadores = true;
            ComprasLblIntegracion.Texto = "Actualizando indicadores…";
            try
            {
                var Datos = await Task.Run(() => new ClsIndicadoresCompras().ComprasFuncConsultar());
                if (IsDisposed || Disposing) return;
                ComprasLblValorKpi1.Texto = Datos.AntiguedadPromedio.ToString("N1", _CulturaIndicadores) + " días";
                ComprasLblValorKpi2.Texto = Datos.PorcentajeAutorizadas.ToString("N1", _CulturaIndicadores) + " %";
                ComprasLblValorKpi3.Texto = Datos.PendientesRecepcion.ToString("N0", _CulturaIndicadores);
                ComprasLblValorKpi4.Texto = "Q " + Datos.SaldoPorPagar.ToString("N2", _CulturaIndicadores);
                ComprasLblValorKpi5.Texto = "Q " + Datos.SaldoVencido.ToString("N2", _CulturaIndicadores);
                ComprasLblValorKpi6.Texto = "Q " + Datos.ProximosVencimientos.ToString("N2", _CulturaIndicadores);
                ComprasLblIntegracion.Texto = "Actualizado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " · Importes en GTQ";
                _AyudaIndicadores.SetToolTip(ComprasLblIntegracion, "Actualización automática cada minuto y al regresar de un proceso.");
            }
            catch (Exception Error)
            {
                if (IsDisposed || Disposing) return;
                ComprasUsrLabel[] Valores = { ComprasLblValorKpi1, ComprasLblValorKpi2, ComprasLblValorKpi3, ComprasLblValorKpi4, ComprasLblValorKpi5, ComprasLblValorKpi6 };
                foreach (var Valor in Valores) Valor.Texto = "—";
                ComprasLblIntegracion.Texto = "No se pudieron actualizar los indicadores. Se reintentará automáticamente.";
                _AyudaIndicadores.SetToolTip(ComprasLblIntegracion, Error.Message);
                Trace.TraceError("No se pudieron consultar los indicadores de Compras: {0}", Error);
            }
            finally
            {
                _ConsultandoIndicadores = false;
            }
        }
    }
}
