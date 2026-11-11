using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Herramientas;
using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 29/10/2026
 * ==================================================================
 * Propósito :
 *  Gráfica de línea sencilla, con los colores de Compras, para mostrar la
 *  evolución del tipo de cambio en un rango de fechas. Se dibuja con GDI+
 *  para no depender de librerías de gráficas externas.
 * ===================================================================
*/

namespace CapaVista_Herramientas.Controles
{
    public class ClsGraficaLineaCompras : Control
    {
        private const int _MargenIzquierdo = 78;
        private const int _MargenDerecho = 24;
        private const int _MargenSuperior = 20;
        private const int _MargenInferior = 42;
        private const int _LineasGuia = 4;

        private List<ClsPuntoTipoCambio> _Puntos = new List<ClsPuntoTipoCambio>();
        private string _MensajeVacio = "Seleccione las monedas y el rango de fechas para ver el historial.";

        public ClsGraficaLineaCompras()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Color.White;
            ForeColor = ClsTemaCompras.PrincipalNavegador;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        }

        public void ComprasProcMostrar(IEnumerable<ClsPuntoTipoCambio> Puntos, string MensajeVacio)
        {
            _Puntos = Puntos == null ? new List<ClsPuntoTipoCambio>() : Puntos.ToList();
            if (!string.IsNullOrWhiteSpace(MensajeVacio)) _MensajeVacio = MensajeVacio;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs Evento)
        {
            base.OnPaint(Evento);
            Graphics Grafico = Evento.Graphics;
            Grafico.SmoothingMode = SmoothingMode.AntiAlias;
            Grafico.Clear(BackColor);

            Rectangle Area = new Rectangle(_MargenIzquierdo, _MargenSuperior,
                Math.Max(1, Width - _MargenIzquierdo - _MargenDerecho),
                Math.Max(1, Height - _MargenSuperior - _MargenInferior));

            using (var PincelTexto = new SolidBrush(ForeColor))
            using (var LapizEje = new Pen(ClsTemaCompras.PrincipalNavegador, 1F))
            using (var LapizGuia = new Pen(ClsTemaCompras.FondoBotones, 1F) { DashStyle = DashStyle.Dash })
            {
                if (_Puntos.Count == 0)
                {
                    var Formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    Grafico.DrawString(_MensajeVacio, Font, PincelTexto, ClientRectangle, Formato);
                    return;
                }

                decimal Minimo = _Puntos.Min(Punto => Punto.Valor);
                decimal Maximo = _Puntos.Max(Punto => Punto.Valor);
                decimal Margen = Maximo == Minimo ? Math.Max(Maximo * 0.01m, 0.0001m) : (Maximo - Minimo) * 0.15m;
                decimal EscalaMinima = Minimo - Margen;
                decimal EscalaMaxima = Maximo + Margen;

                for (int Indice = 0; Indice <= _LineasGuia; Indice++)
                {
                    float Y = Area.Bottom - Area.Height * Indice / (float)_LineasGuia;
                    decimal Valor = EscalaMinima + (EscalaMaxima - EscalaMinima) * Indice / _LineasGuia;
                    Grafico.DrawLine(LapizGuia, Area.Left, Y, Area.Right, Y);
                    string Texto = Valor.ToString("N4", CultureInfo.InvariantCulture);
                    SizeF Medida = Grafico.MeasureString(Texto, Font);
                    Grafico.DrawString(Texto, Font, PincelTexto, Area.Left - Medida.Width - 6, Y - Medida.Height / 2);
                }

                Grafico.DrawLine(LapizEje, Area.Left, Area.Top, Area.Left, Area.Bottom);
                Grafico.DrawLine(LapizEje, Area.Left, Area.Bottom, Area.Right, Area.Bottom);

                DateTime Inicio = _Puntos.First().Fecha;
                double Dias = Math.Max(1, (_Puntos.Last().Fecha - Inicio).TotalDays);
                PointF[] Coordenadas = _Puntos.Select(Punto => new PointF(
                    Area.Left + (float)((Punto.Fecha - Inicio).TotalDays / Dias) * Area.Width,
                    Area.Bottom - (float)((Punto.Valor - EscalaMinima) / (EscalaMaxima - EscalaMinima)) * Area.Height)).ToArray();

                if (_Puntos.Count == 1) Coordenadas[0].X = Area.Left + Area.Width / 2F;

                using (var LapizLinea = new Pen(ClsTemaCompras.AcentoNavegador, 2.5F))
                using (var PincelPunto = new SolidBrush(ClsTemaCompras.FondoEtiquetasDestacadas))
                {
                    if (Coordenadas.Length > 1) Grafico.DrawLines(LapizLinea, Coordenadas);
                    foreach (PointF Coordenada in Coordenadas)
                        Grafico.FillEllipse(PincelPunto, Coordenada.X - 4, Coordenada.Y - 4, 8, 8);
                }

                for (int Indice = 0; Indice < _Puntos.Count; Indice++)
                {
                    bool Extremo = Indice == 0 || Indice == _Puntos.Count - 1;
                    if (!Extremo && _Puntos.Count > 8) continue;
                    string Fecha = _Puntos[Indice].Fecha.ToString("dd/MM", CultureInfo.InvariantCulture);
                    SizeF Medida = Grafico.MeasureString(Fecha, Font);
                    float X = Math.Max(Area.Left, Math.Min(Area.Right - Medida.Width, Coordenadas[Indice].X - Medida.Width / 2));
                    Grafico.DrawString(Fecha, Font, PincelTexto, X, Area.Bottom + 6);
                }
            }
        }
    }
}
