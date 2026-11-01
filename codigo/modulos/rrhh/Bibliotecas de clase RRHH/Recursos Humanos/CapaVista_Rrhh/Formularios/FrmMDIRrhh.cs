/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander
 * Carné : 0901-23-3747
 * Fecha : 11/10/2026
 * ==================================================================
 * Propósito :
 *  Formulario MDI (menú de inicio) del módulo de Recursos Humanos.
 *  Tiene el menú lateral con las opciones del módulo, la barra de
 *  arriba con el usuario y el rol, y el panel de bienvenida con el
 *  resumen de indicadores (KPIs), la información del módulo y la
 *  nota de acceso por perfil. Es la única ventana del módulo que se puede
 *  maximizar, por eso el contenido se acomoda al tamaño que tenga.
 * Reglas especificas:
 *  Por ahora es un prototipo no funcional, los KPIs quedan en 0 y
 *  solo algunos botones del menú abren su ventana. El usuario, el
 *  rol y los permisos se reciben desde afuera con los métodos
 *  RrhhMetActualizarInfoUsuario y RrhhMetAplicarPermisos, para no
 *  depender de las DLL de Seguridad.
 * ===================================================================
*/

using CapaVista_CierreNominas.Formularios;
using CapaVista_Empleado.Formularios;
using CapaVista_EvaluacionDesempeño.Formularios;
using CapaVista_Nomina.Formularios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CapaVista_Rrhh.Formularios
{
    public partial class FrmMDIRrhh : Form
    {
        private bool _NavegadorColapsado;
        private int _AnchoNavegador;

        private Image _FondoDashboard;
        private Image _FondoNavegador;
        private Bitmap _FondoNavegadorEscalado;

        private bool _MedidasListas;
        // float porque Scale(), SizeF y Font de WinForms solo trabajan con float
        private float _Zoom = 1f;
        private int _Margen, _Separacion, _AnchoKpiBase, _AltoKpiBase, _TopResumen, _OffsetKpi, _OffsetInfo;
        private int _AltoHeroMin, _MascotaAbajo, _AltoInfoBase, _InfoPad, _DxIcono, _DxNota;
        private Size _MascotaBase;
        private readonly Dictionary<Control, KeyValuePair<Rectangle, Font>> _Originales =
            new Dictionary<Control, KeyValuePair<Rectangle, Font>>();

        public FrmMDIRrhh()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Load += FrmMDIRrhh_Load;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams Parametros = base.CreateParams;
                Parametros.ExStyle |= 0x02000000;
                return Parametros;
            }
        }

        private void FrmMDIRrhh_Load(object sender, EventArgs e)
        {
            RrhhMetGuardarMedidas();
            RrhhMetPrepararImagenes();
            RrhhMetCargarIconos();
            RrhhMetDarFormaTarjetas();
            RrhhMetAcomodarDashboard();

            float Escala;
            using (Graphics G = CreateGraphics()) Escala = G.DpiX / 96f;
            Rectangle Area = Screen.FromControl(this).WorkingArea;
            this.MinimumSize = new Size(Math.Min((int)(1024 * Escala), Area.Width),
                                        Math.Min((int)(720 * Escala), Area.Height));
        }

        private void RrhhMetGuardarMedidas()
        {
            _Margen = RrhhPnlKPI1.Left;
            _Separacion = RrhhPnlKPI2.Left - RrhhPnlKPI1.Right;
            _AnchoKpiBase = RrhhPnlKPI1.Width;
            _AltoKpiBase = RrhhPnlKPI1.Height;
            _TopResumen = RrhhLblResumen.Top;
            _OffsetKpi = RrhhPnlKPI1.Top - RrhhLblResumen.Top;
            _OffsetInfo = RrhhPnlInfo.Top - RrhhPnlKPI1.Bottom;
            _AltoHeroMin = RrhhBtnAyuda.Bottom + _Separacion;
            _MascotaBase = RrhhPbMascota.Size;
            _MascotaAbajo = RrhhPbMascota.Bottom - RrhhLblResumen.Top;
            _AltoInfoBase = RrhhPnlInfo.Height;
            _InfoPad = RrhhLblInfo.Left;
            _DxIcono = RrhhLblIconoNota.Left - RrhhPnlSeparador.Left;
            _DxNota = RrhhLblNota.Left - RrhhPnlSeparador.Left;

            RrhhMetGuardarControles(RrhhPnlDashboard);
            _MedidasListas = true;
        }

        private void RrhhMetGuardarControles(Control Padre)
        {
            foreach (Control Hijo in Padre.Controls)
            {
                _Originales[Hijo] = new KeyValuePair<Rectangle, Font>(Hijo.Bounds, Hijo.Font);
                RrhhMetGuardarControles(Hijo);
            }
        }

        private void RrhhMetAcomodarDashboard()
        {
            if (!_MedidasListas) return;

            int Ancho = RrhhPnlDashboard.Width;
            int Alto = RrhhPnlDashboard.Height;
            int AltoBase = _AltoHeroMin + _OffsetKpi + _AltoKpiBase + _OffsetInfo + _AltoInfoBase + _Margen / 2;
            int AnchoBase = 2 * _Margen + 5 * _Separacion + 6 * (_AnchoKpiBase * 85 / 100);

            float Zoom = Math.Min(1f, Math.Min((float)Alto / AltoBase, (float)Ancho / AnchoBase));
            Zoom = Math.Max(0.6f, (float)Math.Floor(Zoom * 20) / 20);

            RrhhPnlDashboard.SuspendLayout();
            RrhhMetAplicarZoom(Zoom);
            if (RrhhMetAcomodarContenido(Ancho) > Alto)
                RrhhMetAcomodarContenido(Ancho - SystemInformation.VerticalScrollBarWidth);
            RrhhPnlDashboard.ResumeLayout();
        }

        private void RrhhMetAplicarZoom(float Zoom)
        {
            if (Zoom == _Zoom) return;
            _Zoom = Zoom;
            int Dy = RrhhPnlDashboard.AutoScrollPosition.Y;

            foreach (var Par in _Originales)
            {
                Control Control = Par.Key;
                Rectangle Original = Par.Value.Key;
                Font FuenteOriginal = Par.Value.Value;
                int Desplazamiento = Control.Parent == RrhhPnlDashboard ? Dy : 0;

                Control.SetBounds(RrhhMetZ(Original.X), RrhhMetZ(Original.Y) + Desplazamiento,
                                  RrhhMetZ(Original.Width), RrhhMetZ(Original.Height));

                Font Anterior = Control.Font;
                Control.Font = Zoom == 1f ? FuenteOriginal
                    : new Font(FuenteOriginal.FontFamily, FuenteOriginal.Size * Zoom, FuenteOriginal.Style);
                if (Anterior != FuenteOriginal && Anterior != Control.Font) Anterior.Dispose();
            }

            foreach (Control Icono in new Control[] { RrhhPnlKPIIcono1, RrhhPnlKPIIcono2, RrhhPnlKPIIcono3,
                                                      RrhhPnlKPIIcono4, RrhhPnlKPIIcono5, RrhhPnlKPIIcono6, RrhhLblIconoNota })
            {
                Region Anterior = Icono.Region;
                using (GraphicsPath Circulo = new GraphicsPath())
                {
                    Circulo.AddEllipse(Icono.ClientRectangle);
                    Icono.Region = new Region(Circulo);
                }
                Anterior?.Dispose();
            }
        }

        private int RrhhMetZ(int Valor)
        {
            return (int)Math.Round(Valor * _Zoom);
        }

        private int RrhhMetAcomodarContenido(int Ancho)
        {
            Panel[] Kpis = { RrhhPnlKPI1, RrhhPnlKPI2, RrhhPnlKPI3, RrhhPnlKPI4, RrhhPnlKPI5, RrhhPnlKPI6 };
            Label[] Textos = { RrhhLblKPI1, RrhhLblKPI2, RrhhLblKPI3, RrhhLblKPI4, RrhhLblKPI5, RrhhLblKPI6 };
            int Dy = RrhhPnlDashboard.AutoScrollPosition.Y;
            int Margen = RrhhMetZ(_Margen), Separacion = RrhhMetZ(_Separacion);

            int Columnas = 6;
            int AnchoKpi = (Ancho - 2 * Margen - 5 * Separacion) / 6;
            if (AnchoKpi < RrhhMetZ(_AnchoKpiBase) * 4 / 5)
            {
                Columnas = 3;
                AnchoKpi = (Ancho - 2 * Margen - 2 * Separacion) / 3;
            }
            int Filas = Kpis.Length / Columnas;
            int AltoKpi = RrhhMetZ(_AltoKpiBase);
            int AltoFilas = Filas * AltoKpi + (Filas - 1) * Separacion;

            int AltoInfo = RrhhMetAcomodarInfo(Ancho - 2 * Margen);
            int AltoAbajo = RrhhMetZ(_OffsetKpi) + AltoFilas + RrhhMetZ(_OffsetInfo) + AltoInfo + Margen / 2;

            int YResumen = Math.Max(RrhhMetZ(_AltoHeroMin),
                Math.Min(RrhhMetZ(_TopResumen), RrhhPnlDashboard.Height - AltoAbajo));
            int YKpi = YResumen + RrhhMetZ(_OffsetKpi);
            RrhhLblResumen.Top = YResumen + Dy;

            for (int i = 0; i < Kpis.Length; i++)
            {
                int Col = i % Columnas, Fila = i / Columnas;
                Kpis[i].SetBounds(Margen + Col * (AnchoKpi + Separacion),
                                  YKpi + Fila * (AltoKpi + Separacion) + Dy, AnchoKpi, AltoKpi);
                Textos[i].Width = AnchoKpi - Textos[i].Left - 4;
                RrhhMetRedondear(Kpis[i], 18);
            }

            int YInfo = YKpi + AltoFilas + RrhhMetZ(_OffsetInfo);
            RrhhPnlInfo.SetBounds(Margen, YInfo + Dy, Ancho - 2 * Margen, AltoInfo);
            RrhhMetRedondear(RrhhPnlInfo, 18);

            int Izq = Math.Max(RrhhLblBienvenido.Right, RrhhLblParrafo.Right) + Margen / 2;
            int EspacioAncho = Ancho - Margen - Izq;
            int Piso = YResumen + RrhhMetZ(_MascotaAbajo);
            float Escala = Math.Min(_Zoom, Math.Min((float)EspacioAncho / _MascotaBase.Width,
                                                    (float)(Piso - Margen / 4) / _MascotaBase.Height));
            RrhhPbMascota.Visible = Escala > 0.35f;
            int AnchoMascota = (int)(_MascotaBase.Width * Escala);
            int AltoMascota = (int)(_MascotaBase.Height * Escala);
            RrhhPbMascota.SetBounds(Izq + (EspacioAncho - AnchoMascota) / 2, Piso - AltoMascota + Dy,
                                    AnchoMascota, AltoMascota);

            return YInfo + AltoInfo + Margen / 2;
        }

        private int RrhhMetAcomodarInfo(int AnchoPanel)
        {
            int Pad = RrhhMetZ(_InfoPad);
            int Separador = AnchoPanel * 62 / 100;
            RrhhPnlSeparador.Left = Separador;
            RrhhLblIconoNota.Left = Separador + RrhhMetZ(_DxIcono);
            RrhhLblNotaTitulo.Left = Separador + RrhhMetZ(_DxNota);
            RrhhLblNota.Left = Separador + RrhhMetZ(_DxNota);

            RrhhLblInfo.Width = Separador - RrhhLblInfo.Left - Pad;
            RrhhLblNota.Width = AnchoPanel - RrhhLblNota.Left - Pad;
            RrhhLblInfo.Height = RrhhMetAltoTexto(RrhhLblInfo);
            RrhhLblNota.Height = RrhhMetAltoTexto(RrhhLblNota);

            int Alto = Math.Max(RrhhMetZ(_AltoInfoBase), Math.Max(RrhhLblInfo.Bottom, RrhhLblNota.Bottom) + Pad / 2);
            RrhhPnlSeparador.Height = Alto - 2 * RrhhPnlSeparador.Top;
            return Alto;
        }

        private static int RrhhMetAltoTexto(Label Etiqueta)
        {
            return TextRenderer.MeasureText(Etiqueta.Text, Etiqueta.Font,
                new Size(Etiqueta.Width, 0), TextFormatFlags.WordBreak).Height + 4;
        }

        private void RrhhMetRedondear(Control Control, int Radio)
        {
            Region Anterior = Control.Region;
            using (GraphicsPath Ruta = RrhhMetRedondearEsquinas(Control.ClientRectangle, Radio))
                Control.Region = new Region(Ruta);
            Anterior?.Dispose();
        }

        private void RrhhMetPrepararImagenes()
        {
            _FondoDashboard = RrhhPnlDashboard.BackgroundImage;
            RrhhMetEscalarFondoDashboard();

            _FondoNavegador = RrhhPnlNavegador.BackgroundImage;
            RrhhPnlNavegador.BackgroundImage = null;

            RrhhMetAjustarImagen(RrhhPbLogo);
            RrhhMetAjustarImagen(RrhhPbMascota);
            RrhhPbMascota.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void RrhhMetEscalarFondoDashboard()
        {
            Size Tamano = RrhhPnlDashboard.ClientSize;
            if (_FondoDashboard == null || Tamano.Width <= 0 || Tamano.Height <= 0) return;

            Image Anterior = RrhhPnlDashboard.BackgroundImage;
            RrhhPnlDashboard.BackgroundImageLayout = ImageLayout.None;
            RrhhPnlDashboard.BackgroundImage = RrhhMetEscalarImagen(_FondoDashboard, Tamano.Width, Tamano.Height, false);
            if (Anterior != null && Anterior != _FondoDashboard) Anterior.Dispose();
        }

        private void RrhhMetAjustarImagen(PictureBox Caja)
        {
            Image Original = Caja.Image;
            if (Original == null) return;

            float Escala = Math.Min((float)Caja.Width / Original.Width, (float)Caja.Height / Original.Height);
            int Ancho = Math.Max(1, (int)(Original.Width * Escala));
            int Alto = Math.Max(1, (int)(Original.Height * Escala));

            Caja.Image = RrhhMetEscalarImagen(Original, Ancho, Alto, true);
            Caja.SizeMode = PictureBoxSizeMode.CenterImage;
            Original.Dispose();
        }

        private void RrhhMetCargarIconos()
        {
            var Iconos = new Dictionary<Button, Image>
            {
                { RrhhBtnEmpleados, Properties.Resources.btn_empleados },
                { RrhhBtnAsistencia, Properties.Resources.btn_asistencia },
                { RrhhBtnPermisosVacaciones, Properties.Resources.btn_permisos },
                { RrhhBtnCapacitaciones, Properties.Resources.btn_capacitaciones },
                { RrhhBtnEvaluacionDesempeno, Properties.Resources.btn_evaluacion },
                { RrhhBtnNomina, Properties.Resources.btn_nomina },
                { RrhhBtnCierresNomina, Properties.Resources.btn_cierres },
                { RrhhBtnLiquidacion, Properties.Resources.btn_liquidacion },
                { RrhhBtnCerrarSesion, Properties.Resources.btn_cerrarsesion }
            };

            int Lado;
            using (Graphics G = CreateGraphics())
            {
                Lado = (int)Math.Round(36 * G.DpiX / 96f);
            }

            foreach (var Par in Iconos)
            {
                Par.Key.Image = RrhhMetEscalarImagen(Par.Value, Lado, Lado, true);
            }
        }

        private static Bitmap RrhhMetEscalarImagen(Image Original, int Ancho, int Alto, bool AltaCalidad)
        {
            var Resultado = new Bitmap(Ancho, Alto, PixelFormat.Format32bppPArgb);
            using (Graphics G = Graphics.FromImage(Resultado))
            {
                G.InterpolationMode = AltaCalidad ? InterpolationMode.HighQualityBicubic : InterpolationMode.Bilinear;
                G.PixelOffsetMode = AltaCalidad ? PixelOffsetMode.HighQuality : PixelOffsetMode.HighSpeed;
                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.DrawImage(Original, 0, 0, Ancho, Alto);
            }
            return Resultado;
        }

        public void RrhhMetActualizarInfoUsuario(string Nombre, string Rol)
        {
            RrhhLblUsuario.Text = $"Usuario: {Nombre}";
            RrhhLblUsuarioRol.Text = $"Rol: {Rol}";
            RrhhLblUsuarioRol.Left = RrhhLblUsuario.Right + 30;
        }

        public void RrhhMetAplicarPermisos(Func<int, bool> TieneAcceso)
        {
            var MapaBotonesMDI = new Dictionary<Button, int>
            {
                { RrhhBtnEmpleados, 0 },
                { RrhhBtnAsistencia, 0 },
                { RrhhBtnPermisosVacaciones, 0 },
                { RrhhBtnCapacitaciones, 0 },
                { RrhhBtnEvaluacionDesempeno, 0 },
                { RrhhBtnNomina, 0 },
                { RrhhBtnCierresNomina, 0 },
                { RrhhBtnLiquidacion, 0 }
            };

            foreach (var Par in MapaBotonesMDI)
            {
                if (!TieneAcceso(Par.Value))
                    RrhhMetDeshabilitarBoton(Par.Key);
            }
        }

        public void RrhhMetDeshabilitarBoton(Button Boton)
        {
            Boton.Enabled = false;
            Boton.BackColor = Color.FromArgb(200, 200, 200);
        }

        private void RrhhBtnMenu_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = Color.FromArgb(14, 76, 85);
        }

        private void RrhhBtnMenu_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = Color.White;
        }

        private void RrhhMetDarFormaTarjetas()
        {
            foreach (Control Tarjeta in RrhhPnlDashboard.Controls)
            {
                if (!(Tarjeta is Panel) || !Tarjeta.Name.StartsWith("RrhhPnlKPI")) continue;
                RrhhMetRedondear(Tarjeta, 18);

                foreach (Control Icono in Tarjeta.Controls)
                {
                    if (!(Icono is Panel)) continue;

                    if (Icono.BackgroundImage != null)
                    {
                        Image Original = Icono.BackgroundImage;
                        Icono.BackgroundImage = RrhhMetEscalarImagen(Original, Icono.Width, Icono.Height, true);
                        Icono.BackgroundImageLayout = ImageLayout.Zoom;
                        Original.Dispose();
                    }

                    GraphicsPath Circulo = new GraphicsPath();
                    Circulo.AddEllipse(Icono.ClientRectangle);
                    Icono.Region = new Region(Circulo);
                }
            }

            RrhhMetRedondear(RrhhPnlInfo, 18);
            GraphicsPath CirculoNota = new GraphicsPath();
            CirculoNota.AddEllipse(RrhhLblIconoNota.ClientRectangle);
            RrhhLblIconoNota.Region = new Region(CirculoNota);

            foreach (Control Boton in RrhhPnlNavegador.Controls)
            {
                if (Boton is Button) Boton.Region = new Region(RrhhMetRedondearEsquinas(Boton.ClientRectangle, 10));
            }
            RrhhBtnBurger.Region = new Region(RrhhMetRedondearEsquinas(RrhhBtnBurger.ClientRectangle, 8));
        }

        private void RrhhPnlDashboard_Resize(object sender, EventArgs e)
        {
            RrhhPnlDashboard.Region = new Region(RrhhMetRedondearEsquinas(RrhhPnlDashboard.ClientRectangle, 20));
            RrhhMetEscalarFondoDashboard();
            RrhhMetAcomodarDashboard();
        }

        private void RrhhPnlNavegador_Resize(object sender, EventArgs e)
        {
            RrhhPnlNavegador.Region = new Region(RrhhMetRedondearEsquinas(RrhhPnlNavegador.ClientRectangle, 22));
        }

        private void RrhhPnlNavegador_Paint(object sender, PaintEventArgs e)
        {
            if (_FondoNavegador == null) return;
            int AnchoFondo = _NavegadorColapsado ? _AnchoNavegador : RrhhPnlNavegador.Width;

            if (_FondoNavegadorEscalado == null || _FondoNavegadorEscalado.Width != AnchoFondo
                || _FondoNavegadorEscalado.Height != RrhhPnlNavegador.Height)
            {
                _FondoNavegadorEscalado?.Dispose();
                _FondoNavegadorEscalado = RrhhMetEscalarImagen(_FondoNavegador, AnchoFondo, RrhhPnlNavegador.Height, false);
            }
            e.Graphics.DrawImageUnscaled(_FondoNavegadorEscalado, 0, 0);
        }

        private void RrhhPnlKPI_Paint(object sender, PaintEventArgs e)
        {
            Control Tarjeta = (Control)sender;
            Rectangle Borde = new Rectangle(0, 0, Tarjeta.Width - 1, Tarjeta.Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath Ruta = RrhhMetRedondearEsquinas(Borde, 18))
            using (Pen Lapiz = new Pen(Color.FromArgb(160, 31, 138, 143), 1.5f))
            {
                e.Graphics.DrawPath(Lapiz, Ruta);
            }
        }

        private GraphicsPath RrhhMetRedondearEsquinas(Rectangle Rectangulo, int Radio)
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

        private void RrhhBtnBurger_Click(object sender, EventArgs e)
        {
            bool Colapsar = !_NavegadorColapsado;
            _NavegadorColapsado = Colapsar;

            this.SuspendLayout();
            RrhhPnlNavegador.SuspendLayout();

            if (Colapsar)
            {
                _AnchoNavegador = RrhhPnlNavegador.Width;
                RrhhPnlNavegador.Width = RrhhBtnEmpleados.Left + RrhhBtnEmpleados.Padding.Left
                    + RrhhBtnEmpleados.Image.Width + 18;
            }
            else
            {
                RrhhPnlNavegador.Width = _AnchoNavegador;
            }
            RrhhPbLogo.Visible = !Colapsar;

            foreach (Control Boton in RrhhPnlNavegador.Controls)
            {
                if (Boton is Button) Boton.Text = Colapsar ? "" : (string)Boton.Tag;
            }

            RrhhPnlNavegador.ResumeLayout();
            this.ResumeLayout();
        }

        private void RrhhBtnAsistencia_Click(object sender, EventArgs e)
        {
            using (var Asistencia = new CapaVista_Asistencia.Formularios.FrmAsistencia())
            {
                Asistencia.ShowDialog(this);
            }
        }

        private void RrhhBtnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult Resultado = MessageBox.Show(
                "¿Seguro que deseas cerrar sesión?",
                "Cerrar sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (Resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void RrhhBtnEmpleados_Click(object sender, EventArgs e)
        {
            FrmEmpleados Empleados = new FrmEmpleados();
            Empleados.ShowDialog(this);
        }

        private void RrhhBtnCierresNomina_Click(object sender, EventArgs e)
        {
            FrmCierreNominas CierreNominas = new FrmCierreNominas();
            CierreNominas.ShowDialog(this);
        }

        private void RrhhBtnNomina_Click(object sender, EventArgs e)
        {
            FrmPercepcionesDeducciones Percepciones = new FrmPercepcionesDeducciones();
            Percepciones.ShowDialog(this);
        }

        private void RrhhBtnCapacitaciones_Click(object sender, EventArgs e)
        {
            FrmCapacitacion Capacitacion = new FrmCapacitacion();
            Capacitacion.ShowDialog(this);
        }

        private void RrhhBtnPermisosVacaciones_Click(object sender, EventArgs e)
        {
            using (var Permisos = new CapaVista_PermisosVacaciones.Formularios.FrmPermisosVacaciones())
            {
                Permisos.ShowDialog(this);
            }
        }

        private void RrhhBtnEvaluacionDesempeno_Click(object sender, EventArgs e)
        {
            FrmEvaluacionDesempeño Evaluacion = new FrmEvaluacionDesempeño();
            Evaluacion.ShowDialog(this);
            
        }

        private void RrhhBtnLiquidacion_Click(object sender, EventArgs e)
        {
            using (var Liquidacion = new CapaVista_Liquidacion_Rrhh.Formularios.FrmLiquidacionEmpleados())
            {
                Liquidacion.ShowDialog(this);
            }
        }
    }
}
