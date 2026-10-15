/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 09/10/2026
 * ==================================================================
 * Propósito :
 *  MDI de prototipo no funcional del módulo de Contabilidad. Replica
 *  la estructura del MDI de Seguridad (menú lateral con logo y botones,
 *  encabezado con usuario y rol, bienvenida con Don Buti, botón de
 *  ayuda, resumen del sistema con 6 KPIs, información relevante y nota
 *  de integración) con la paleta de Contabilidad.
 * Reglas especificas:
 *  El MDI lee la sesión y permisos existentes de Seguridad mediante el Controlador.
 *  Las ventanas CRUD usan la DLL del Navegador sin configurar tablas.
 *  Los indicadores son valores de presentación del prototipo.
 *  Los procesos contables y el Reporteador todavía no se implementan.
 * ===================================================================
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Contabilidad;

namespace CapaVista_Contabilidad
{
    public partial class FrmMDIContabilidad : Form
    {
        // Las coordenadas y tamaños son Int32 porque la API de WinForms los exige.
        // Las proporciones se calculan en Double; no hay cálculos monetarios en la Vista.
        private const int _AnchoMenuExpandido = 270;
        private const int _AnchoMenuContraido = 64;
        private const int _MargenMenu = 32;      // 16 px a cada lado del panel del menú
        private const int _MargenDashboard = 44;
        private const int _AltoKpiBase = 189;
        private const int _AnchoKpiBase = 172;
        private Button _BotonActivo;
        private Button _BotonSobre;
        private readonly ClsPermisosMenuContabilidad _PermisosMenu = new ClsPermisosMenuContabilidad();
        private readonly Dictionary<int, Button> _BotonesMenu;

        public FrmMDIContabilidad()
        {
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetAplicarTipografia(this);
            _BotonesMenu = new Dictionary<int, Button>
            {
                { 10001, ContabilidadBtnPolizasLocales }, { 10002, ContabilidadBtnPresupuestos },
                { 10003, ContabilidadBtnActivosFijos }, { 10004, ContabilidadBtnCierreContable },
                { 10005, ContabilidadBtnEstadosFinancieros }
            };
            foreach (Button Boton in _BotonesMenu.Values) _PermisosMenu.ContabilidadMetAplicar(Boton, false);
            Disposed += (Remitente, Evento) => _PermisosMenu.Dispose();
            ContabilidadPnlPrincipal.Resize += (Remitente, Evento) => ContabilidadMetAcomodarEncabezado();
            this.WindowState = FormWindowState.Maximized;

            ContabilidadMetDobleBuffer(ContabilidadPnlFondo);
            ContabilidadMetDobleBuffer(ContabilidadPnlPrincipal);
            ContabilidadMetDobleBuffer(ContabilidadPnlDashboard);
            ContabilidadMetDobleBuffer(ContabilidadPnlNavegador);
            ContabilidadMetDobleBuffer(ContabilidadPnlInfo);

            this.Load += ContabilidadMetCargarFormulario;
            ContabilidadMetConfigurarEstadosMenu();
            ContabilidadPnlDashboard.Resize += (Remitente, Evento) => ContabilidadMetAcomodarDashboard();
            ContabilidadPnlInfo.Resize += (Remitente, Evento) => ContabilidadMetAcomodarInfo();
        }

        private void ContabilidadMetCargarFormulario(object Remitente, EventArgs Evento)
        {
            ContabilidadMetActualizarSeguridad(true);
            ContabilidadMetAcomodarDashboard();
        }

        // Puede llamarlo el anfitrión después de iniciar/cambiar la sesión de Seguridad.
        public void ContabilidadMetActualizarSeguridad(bool MostrarAviso = false)
        {
            ClsEstadoSeguridadContabilidad Estado = new ClsControladorSeguridadContabilidad().ContabilidadFuncObtenerEstado();
            ContabilidadLblUsuario.Text = "Usuario: " + Estado.NombreUsuario;
            ContabilidadLblUsuarioRol.Text = "Rol: " + Estado.Roles;
            foreach (KeyValuePair<int, Button> Boton in _BotonesMenu)
                _PermisosMenu.ContabilidadMetAplicar(Boton.Value, Estado.Accesos[Boton.Key]);
            if (_BotonActivo != null && !_BotonActivo.Enabled) _BotonActivo = null;
            _BotonSobre = null;
            ContabilidadMetAcomodarEncabezado();
            if (MostrarAviso && !string.IsNullOrWhiteSpace(Estado.Mensaje))
                MessageBox.Show(this, Estado.Mensaje, "Seguridad de Contabilidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ContabilidadMetAcomodarEncabezado()
        {
            int AnchoDisponible = Math.Max(0, ContabilidadPnlPrincipal.ClientSize.Width - ContabilidadLblUsuario.Left - 24);
            int AnchoUsuario = Math.Max(0, (AnchoDisponible - 24) / 2);
            ContabilidadLblUsuario.AutoSize = false;
            ContabilidadLblUsuario.AutoEllipsis = true;
            ContabilidadLblUsuario.Width = AnchoUsuario;
            ContabilidadLblUsuarioRol.AutoSize = false;
            ContabilidadLblUsuarioRol.AutoEllipsis = true;
            ContabilidadLblUsuarioRol.Left = ContabilidadLblUsuario.Right + 24;
            ContabilidadLblUsuarioRol.Width = Math.Max(0, AnchoDisponible - AnchoUsuario - 24);
        }

        // Reacomoda el contenido del dashboard cuando cambia el tamaño de la ventana: Don Buti, el
        // resumen y la fila de KPIs se reparten según el espacio disponible.
        private void ContabilidadMetAcomodarDashboard()
        {
            int Ancho = ContabilidadPnlDashboard.ClientSize.Width;
            int Alto = ContabilidadPnlDashboard.ClientSize.Height;
            if (Ancho <= 0 || Alto <= 0)
            {
                return;
            }

            int AltoInfo = ContabilidadPnlInfo.Height;
            int PosicionSuperiorInfo = Alto - 24 - AltoInfo;
            ContabilidadPnlInfo.SetBounds(_MargenDashboard, PosicionSuperiorInfo, Ancho - 2 * _MargenDashboard, AltoInfo);

            bool Compacto = Alto < 720;
            int AnchoKpi = Math.Max(100, Math.Min(190, (Ancho - 2 * _MargenDashboard - 5 * 12) / 6));
            // Al colapsar el menú en una laptop, el ancho adicional no debe hacer
            // crecer las tarjetas hacia la bienvenida y el botón de Ayuda.
            if (Compacto)
            {
                AnchoKpi = Math.Min(140, AnchoKpi);
            }
            int AltoKpi = (int)Math.Round(AnchoKpi * (double)_AltoKpiBase / _AnchoKpiBase);
            int Separacion = (Ancho - 2 * _MargenDashboard - 6 * AnchoKpi) / 5;
            int PosicionSuperiorKpi = PosicionSuperiorInfo - 24 - AltoKpi;

            Panel[] Kpis = { ContabilidadPnlKPI1, ContabilidadPnlKPI2, ContabilidadPnlKPI3,
                             ContabilidadPnlKPI4, ContabilidadPnlKPI5, ContabilidadPnlKPI6 };
            Label[] Valores = { ContabilidadLblKPIResp1, ContabilidadLblKPIResp2, ContabilidadLblKPIResp3,
                                ContabilidadLblKPIResp4, ContabilidadLblKPIResp5, ContabilidadLblKPIResp6 };
            Label[] Textos = { ContabilidadLblKPI1, ContabilidadLblKPI2, ContabilidadLblKPI3,
                               ContabilidadLblKPI4, ContabilidadLblKPI5, ContabilidadLblKPI6 };

            for (int Indice = 0; Indice < Kpis.Length; Indice++)
            {
                Kpis[Indice].SetBounds(_MargenDashboard + Indice * (AnchoKpi + Separacion), PosicionSuperiorKpi, AnchoKpi, AltoKpi);
                Valores[Indice].Location = new Point(14 * AnchoKpi / _AnchoKpiBase, 95 * AltoKpi / _AltoKpiBase);
                Textos[Indice].Location = new Point(18 * AnchoKpi / _AnchoKpiBase, 138 * AltoKpi / _AltoKpiBase);
            }

            ContabilidadLblResumen.Location = new Point(38, PosicionSuperiorKpi - ContabilidadLblResumen.Height - 12);

            int AltoButti = Math.Max(180, PosicionSuperiorKpi - 50);
            int AnchoButti = (int)Math.Round(AltoButti * 313.0 / 415.0);
            int PosicionIzquierdaButti = Math.Max(500, (int)(Ancho * 0.43));
            ContabilidadPbButti.SetBounds(PosicionIzquierdaButti, 10, AnchoButti, AltoButti);
            ContabilidadLblBienvenido.Top = Compacto ? 16 : 36;
            ContabilidadLblModulo.Top = Compacto ? 92 : 112;
            ContabilidadLblParrafo.Top = Compacto ? 136 : 160;
            int TamanoAyuda = Compacto ? 64 : 84;
            int PosicionAyuda = Math.Min(Compacto ? 220 : 268, ContabilidadLblResumen.Top - TamanoAyuda - 12);
            ContabilidadBtnAyuda.SetBounds(52, PosicionAyuda, TamanoAyuda, TamanoAyuda);
            ContabilidadMetAcomodarInfo();
        }

        // Divide el panel inferior: información relevante a la izquierda, nota de integración a la derecha.
        private void ContabilidadMetAcomodarInfo()
        {
            int Divisor = (int)(ContabilidadPnlInfo.Width * 0.60);
            ContabilidadPnlInfoDivisor.Location = new Point(Divisor, 14);
            ContabilidadPbInfo.Location = new Point(Divisor + 28, (ContabilidadPnlInfo.Height - ContabilidadPbInfo.Height) / 2);
            ContabilidadLblNotaTitulo.Location = new Point(Divisor + 90, 14);
            ContabilidadLblNotaTexto.Location = new Point(Divisor + 92, 40);
            ContabilidadLblInfoTexto.AutoSize = false;
            ContabilidadLblInfoTexto.Size = new Size(Math.Max(1, Divisor - 52), 40);
            ContabilidadLblNotaTexto.AutoSize = false;
            ContabilidadLblNotaTexto.Size = new Size(Math.Max(1, ContabilidadPnlInfo.Width - Divisor - 112), 48);
            ContabilidadPnlInfo.Invalidate();
        }

        // Fondo blanco con esquinas redondeadas y borde suave del panel de información.
        private void ContabilidadMetPintarInformacion(object Remitente, PaintEventArgs Evento)
        {
            Graphics Lienzo = Evento.Graphics;
            Lienzo.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle Area = new Rectangle(1, 1, ContabilidadPnlInfo.Width - 3, ContabilidadPnlInfo.Height - 3);
            using (GraphicsPath Ruta = ContabilidadFuncRedondearEsquinas(Area, 22))
            using (SolidBrush Relleno = new SolidBrush(Color.White))
            using (Pen Borde = new Pen(Color.FromArgb(212, 222, 228), 2))
            {
                Lienzo.FillPath(Relleno, Ruta);
                Lienzo.DrawPath(Borde, Ruta);
            }
        }

        private GraphicsPath ContabilidadFuncRedondearEsquinas(Rectangle Rectangulo, int Radio)
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

        // Evita el parpadeo al redimensionar (DoubleBuffered es protegida en Control).
        private static void ContabilidadMetDobleBuffer(Control Objetivo)
        {
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(Objetivo, true, null);
        }

        // Contrae/expande el menú lateral; el resto del MDI se reacomoda solo por el Dock/Anchor.
        private void ContabilidadMetAlternarMenu(object Remitente, EventArgs Evento)
        {
            bool Expandido = ContabilidadPnlNavegador.Width == _AnchoMenuExpandido;
            int NuevoAncho = Expandido ? _AnchoMenuContraido : _AnchoMenuExpandido;

            ContabilidadPnlNavegador.Width = NuevoAncho;
            ContabilidadPnlLateral.Width = NuevoAncho + _MargenMenu;
        }

        // Los mantenimientos usan la DLL de Navegador y la sesión de Seguridad.
        private void ContabilidadMetAbrirPolizasLocales(object Remitente, EventArgs Evento)
        {
            using (FrmPolizasLocales Formulario = new FrmPolizasLocales())
            {
                ContabilidadMetAbrirFormulario(Formulario, (Button)Remitente);
            }
        }

        private void ContabilidadMetAbrirPresupuestos(object Remitente, EventArgs Evento)
        {
            using (FrmPresupuestos Formulario = new FrmPresupuestos())
            {
                ContabilidadMetAbrirFormulario(Formulario, (Button)Remitente);
            }
        }

        private void ContabilidadMetAbrirActivosFijos(object Remitente, EventArgs Evento)
        {
            using (FrmActivosFijos Formulario = new FrmActivosFijos())
            {
                ContabilidadMetAbrirFormulario(Formulario, (Button)Remitente);
            }
        }

        private void ContabilidadMetAbrirCierreContable(object Remitente, EventArgs Evento)
        {
            using (FrmCierreContable Formulario = new FrmCierreContable())
            {
                ContabilidadMetAbrirFormulario(Formulario, (Button)Remitente);
            }
        }

        private void ContabilidadMetAbrirEstadosFinancieros(object Remitente, EventArgs Evento)
        {
            using (FrmEstadosFinancieros Formulario = new FrmEstadosFinancieros())
            {
                ContabilidadMetAbrirFormulario(Formulario, (Button)Remitente);
            }
        }


        // Estados visuales de presentación (EST-10, Parte I §11.3).
        private void ContabilidadMetConfigurarEstadosMenu()
        {
            Button[] Botones = { ContabilidadBtnPolizasLocales, ContabilidadBtnPresupuestos,
                ContabilidadBtnActivosFijos, ContabilidadBtnCierreContable, ContabilidadBtnEstadosFinancieros };
            string[] Titulos = { "Pólizas locales", "Presupuestos", "Activos fijos",
                "Cierre contable", "Estados financieros" };
            for (int Indice = 0; Indice < Botones.Length; Indice++)
            {
                Button Boton = Botones[Indice];
                Boton.AccessibleName = Titulos[Indice];
                Boton.MouseEnter += (Remitente, Evento) =>
                {
                    _BotonSobre = (Button)Remitente;
                    _BotonSobre.Invalidate();
                };
                Boton.MouseLeave += (Remitente, Evento) =>
                {
                    _BotonSobre = null;
                    ((Button)Remitente).Invalidate();
                };
                Boton.Paint += ContabilidadMetPintarEstadoMenu;
            }
        }

        private void ContabilidadMetPintarEstadoMenu(object Remitente, PaintEventArgs Evento)
        {
            Button Boton = (Button)Remitente;
            if (Boton.Enabled && (Boton == _BotonSobre || Boton == _BotonActivo))
            {
                using (SolidBrush Fondo = new SolidBrush(Color.FromArgb(65, 70, 104, 125)))
                    Evento.Graphics.FillRectangle(Fondo, Boton.ClientRectangle);
            }
            if (Boton.Enabled && Boton == _BotonActivo)
            {
                using (SolidBrush Acento = new SolidBrush(Color.FromArgb(197, 143, 62)))
                    Evento.Graphics.FillRectangle(Acento, 0, 0, 4, Boton.Height);
            }
            // La etiqueta se presenta en Tahoma y con solo la primera letra mayúscula
            // (EST-10 §2.3 y §9), conservando el dibujo y el archivo PNG originales.
            Rectangle Etiqueta = new Rectangle(60, 4, Boton.Width - 64, Boton.Height - 8);
            using (SolidBrush Fondo = new SolidBrush(Boton.Enabled ? Color.FromArgb(70, 104, 125) : Color.FromArgb(96, 96, 96)))
                Evento.Graphics.FillRectangle(Fondo, Etiqueta);
            Color Texto = !Boton.Enabled ? Color.FromArgb(210, 210, 210) :
                Boton == _BotonActivo ? Color.FromArgb(197, 143, 62) : Color.FromArgb(212, 222, 228);
            TextRenderer.DrawText(Evento.Graphics, Boton.AccessibleName, Boton.Font,
                Etiqueta, Texto, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        private void ContabilidadMetAbrirFormulario(Form Formulario, Button Boton)
        {
            // Volver a consultar evita usar un acceso anterior si cambió la sesión.
            ContabilidadMetActualizarSeguridad(true);
            if (!Boton.Enabled) return;
            Button Anterior = _BotonActivo;
            _BotonActivo = Boton;
            Anterior?.Invalidate();
            Boton.Invalidate();
            Formulario.ShowDialog(this);
        }

        private void ContabilidadMetMostrarAyuda(object Remitente, EventArgs Evento)
        {
            MessageBox.Show("La ayuda del módulo de Contabilidad aún no está disponible.",
                "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
