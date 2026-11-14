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
 *  El MDI lee la sesión y los permisos mediante el Controlador; no toca el Modelo.
 *  Las cinco pantallas son ventanas modales con su propio controlador.
 *  Pólizas, presupuestos, activos fijos y cierre se despliegan en
 *  Gestionar y Reporte; cada módulo entrega su ventana de reporte.
 *  Los indicadores se leen de la base mediante el Controlador y se
 *  actualizan al cerrar cada pantalla.
 *  Los procesos contables reales y el Reporteador todavía no se implementan.
 * ===================================================================
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Contabilidad;
// Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
using CapaControlador_Navegador;
// Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026

namespace CapaVista_Contabilidad
{
    public partial class FrmMDIContabilidad : Form
    {

        private readonly int _IdModulo;
        private const int _AnchoMenuExpandido = 270;
        private const int _AnchoMenuContraido = 64;
        private const int _MargenMenu = 32;
        private const int _MargenDashboard = 44;
        private const int _AltoKpiBase = 189;
        private const int _AnchoKpiBase = 172;
        private Button _BotonActivo;
        private Button _BotonSobre;
        private ClsEstadoSeguridadContabilidad _Estado;
        private readonly ClsPermisosMenuContabilidad _PermisosMenu = new ClsPermisosMenuContabilidad();
        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
        // botones que despliegan un submenú y sus hijos
        private readonly Dictionary<Button, Button[]> _Submenus;
        // submenú desplegado
        private Button _SubmenuAbierto;
        // botones principales del menú en orden
        private readonly Button[] _MenuPrincipal;
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
        private readonly Dictionary<int, Button> _BotonesMenu;
        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
        // posición original de cada control del menú
        private readonly Dictionary<Control, int> _PosicionesMenu = new Dictionary<Control, int>();
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026

        public FrmMDIContabilidad() : this(1)
        {
        }

        public FrmMDIContabilidad(int IdModulo)
        {
            _IdModulo = IdModulo;
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetAplicarTipografia(this);
            _BotonesMenu = new Dictionary<int, Button>
            {
                { 10001, ContabilidadBtnPolizasLocales }, { 10002, ContabilidadBtnPresupuestos },
                { 10003, ContabilidadBtnActivosFijos }, { 10004, ContabilidadBtnCierreContable },
                { 10005, ContabilidadBtnEstadosFinancieros },
                { 10011, ContabilidadBtnPaises },
                { 10012, ContabilidadBtnModulosContables },
                { 10013, ContabilidadBtnVidaUtil },
                { 10014, ContabilidadBtnCuentas },
                { 10015, ContabilidadBtnCuentasModulo }
            };
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            _Submenus = new Dictionary<Button, Button[]>
            {
                { ContabilidadBtnPolizasLocales, new[] { ContabilidadBtnPolizasLocalesGestion, ContabilidadBtnPolizasLocalesReporte } },
                { ContabilidadBtnPresupuestos, new[] { ContabilidadBtnPresupuestosGestion, ContabilidadBtnPresupuestosReporte } },
                { ContabilidadBtnActivosFijos, new[] { ContabilidadBtnActivosFijosGestion, ContabilidadBtnActivosFijosReporte } },
                { ContabilidadBtnCierreContable, new[] { ContabilidadBtnCierreContableGestion, ContabilidadBtnCierreContableReporte } },
                { ContabilidadBtnCatalogos, new[] { ContabilidadBtnPaises, ContabilidadBtnModulosContables, ContabilidadBtnVidaUtil,
                    ContabilidadBtnCuentas, ContabilidadBtnCuentasModulo } }
            };
            _MenuPrincipal = new[] { ContabilidadBtnPolizasLocales, ContabilidadBtnPresupuestos, ContabilidadBtnActivosFijos,
                ContabilidadBtnCierreContable, ContabilidadBtnEstadosFinancieros, ContabilidadBtnCatalogos };
            foreach (Button Boton in _BotonesMenu.Values) ContabilidadMetAplicarPermiso(Boton, false);
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            _PermisosMenu.ContabilidadMetAplicar(ContabilidadBtnCatalogos, false);
            Disposed += (Remitente, Evento) => _PermisosMenu.Dispose();
            ContabilidadPnlPrincipal.Resize += (Remitente, Evento) => ContabilidadMetAcomodarEncabezado();

            ContabilidadMetDobleBuffer(ContabilidadPnlFondo);
            ContabilidadMetDobleBuffer(ContabilidadPnlPrincipal);
            ContabilidadMetDobleBuffer(ContabilidadPnlDashboard);
            ContabilidadMetDobleBuffer(ContabilidadPnlNavegador);
            ContabilidadMetDobleBuffer(ContabilidadPnlInfo);

            this.Load += ContabilidadMetCargarFormulario;
            ContabilidadMetConfigurarEstadosMenu();
            ContabilidadPnlDashboard.Resize += (Remitente, Evento) => ContabilidadMetAcomodarDashboard();
            ContabilidadPnlInfo.Resize += (Remitente, Evento) => ContabilidadMetAcomodarInfo();
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
            foreach (Control Elemento in ContabilidadPnlNavegador.Controls)
            {
                if (Elemento != ContabilidadBtnCerrarSesion) _PosicionesMenu[Elemento] = Elemento.Top;
                Elemento.MouseWheel += ContabilidadMetRuedaMenu;
            }
            ContabilidadPnlNavegador.MouseWheel += ContabilidadMetRuedaMenu;
            ContabilidadVsbMenu.ValueChanged += (Remitente, Evento) => ContabilidadMetAcomodarMenu();
            ContabilidadPnlNavegador.Resize += (Remitente, Evento) => ContabilidadMetAcomodarMenu();
            ContabilidadMetAcomodarMenu();
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
        }

        private void ContabilidadMetCargarFormulario(object Remitente, EventArgs Evento)
        {
            ContabilidadMetActualizarSeguridad(true);
            ContabilidadMetAcomodarDashboard();
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            ContabilidadMetActualizarIndicadores();
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
        }

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
        // Lee los seis indicadores; el que no se pudo leer muestra una raya
        private void ContabilidadMetActualizarIndicadores()
        {
            ClsModeloIndicadoresContabilidad Indicadores = new ClsControladorIndicadoresContabilidad().ContabilidadFuncObtener();
            ContabilidadLblKPIResp1.Text = ContabilidadFuncTextoIndicador(Indicadores.Polizas);
            ContabilidadLblKPIResp2.Text = ContabilidadFuncTextoIndicador(Indicadores.PresupuestosVigentes);
            ContabilidadLblKPIResp3.Text = ContabilidadFuncTextoIndicador(Indicadores.ActivosFijos);
            ContabilidadLblKPIResp4.Text = ContabilidadFuncTextoIndicador(Indicadores.PeriodosCerrados);
            ContabilidadLblKPIResp5.Text = ContabilidadFuncTextoIndicador(Indicadores.PolizasAplicadas);
            ContabilidadLblKPIResp6.Text = ContabilidadFuncTextoIndicador(Indicadores.Cuentas);
        }

        // Texto de un indicador
        private static string ContabilidadFuncTextoIndicador(int? Valor)
        {
            return Valor.HasValue ? Valor.Value.ToString("N0") : "—";
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026

        public void ContabilidadMetActualizarInfoUsuario(string NombreEmpleado, string Roles)
        {
            ContabilidadLblUsuario.Text = "Usuario: " + NombreEmpleado;
            ContabilidadLblUsuarioRol.Text = "Rol: " + Roles;
            ContabilidadMetAcomodarEncabezado();
        }

        public void ContabilidadMetActualizarSeguridad(bool MostrarAviso = false)
        {
            ClsEstadoSeguridadContabilidad Estado = new ClsControladorSeguridadContabilidad().ContabilidadFuncObtenerEstado();
            _Estado = Estado;
            ContabilidadLblUsuario.Text = "Usuario: " + Estado.NombreUsuario;
            ContabilidadLblUsuarioRol.Text = "Rol: " + Estado.Roles;
            foreach (KeyValuePair<int, Button> Boton in _BotonesMenu)
                // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
                ContabilidadMetAplicarPermiso(Boton.Value, Estado.Permisos[Boton.Key].TieneAcceso);
                // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            bool AlgunCatalogo = false;
            foreach (int Codigo in new[] { 10011, 10012, 10013, 10014, 10015 }) AlgunCatalogo |= Estado.Permisos[Codigo].TieneAcceso;
            _PermisosMenu.ContabilidadMetAplicar(ContabilidadBtnCatalogos, AlgunCatalogo);
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            if (_SubmenuAbierto != null && !_SubmenuAbierto.Enabled) _SubmenuAbierto = null;
            ContabilidadMetAcomodarSubmenus();
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            if (_BotonActivo != null && !_BotonActivo.Enabled) _BotonActivo = null;
            _BotonSobre = null;
            ContabilidadMetAcomodarEncabezado();
            if (MostrarAviso && !string.IsNullOrWhiteSpace(Estado.Mensaje))
                MessageBox.Show(this, Estado.Mensaje, "Seguridad de Contabilidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
        // El permiso de un módulo vale también para Gestionar y Reporte
        private void ContabilidadMetAplicarPermiso(Button Boton, bool TieneAcceso)
        {
            _PermisosMenu.ContabilidadMetAplicar(Boton, TieneAcceso);
            Button[] Hijos;
            if (Boton != ContabilidadBtnCatalogos && _Submenus.TryGetValue(Boton, out Hijos))
                foreach (Button Hijo in Hijos) _PermisosMenu.ContabilidadMetAplicar(Hijo, TieneAcceso);
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026

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

        private static void ContabilidadMetDobleBuffer(Control Objetivo)
        {
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(Objetivo, true, null);
        }

        private void ContabilidadMetAlternarMenu(object Remitente, EventArgs Evento)
        {
            bool Expandido = ContabilidadPnlNavegador.Width == _AnchoMenuExpandido;
            int NuevoAncho = Expandido ? _AnchoMenuContraido : _AnchoMenuExpandido;

            ContabilidadPnlNavegador.Width = NuevoAncho;
            ContabilidadPnlLateral.Width = NuevoAncho + _MargenMenu;
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            ContabilidadMetAcomodarSubmenus();
        }

        // Abre o cierra el submenú del botón; solo queda uno abierto
        private void ContabilidadMetAlternarSubmenu(object Remitente, EventArgs Evento)
        {
            Button Boton = (Button)Remitente;
            _SubmenuAbierto = _SubmenuAbierto == Boton ? null : Boton;
            ContabilidadMetAcomodarSubmenus();
        }

        // Muestra los hijos del submenú abierto y oculta los demás
        private void ContabilidadMetAcomodarSubmenus()
        {
            int Izquierda = ContabilidadPnlNavegador.Width == _AnchoMenuExpandido ? 21 : 3;
            foreach (KeyValuePair<Button, Button[]> Submenu in _Submenus)
            {
                foreach (Button Hijo in Submenu.Value)
                {
                    Hijo.Visible = Submenu.Key == _SubmenuAbierto;
                    Hijo.Left = Izquierda;
                }
                Submenu.Key.Invalidate();
            }
            ContabilidadMetAcomodarMenu();
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
        // Cerrar sesión va al final del menú; si no cabe, la barra de la izquierda mueve el menú
        private void ContabilidadMetAcomodarMenu()
        {
            int Visible = ContabilidadPnlNavegador.ClientSize.Height;
            int Alto = ContabilidadBtnCerrarSesion.Height;
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            // Cada botón va debajo del anterior y el submenú abierto empuja a los de abajo
            Dictionary<Control, int> Posiciones = new Dictionary<Control, int>(_PosicionesMenu);
            int Siguiente = _PosicionesMenu[ContabilidadBtnPolizasLocales];
            foreach (Button Principal in _MenuPrincipal)
            {
                Posiciones[Principal] = Siguiente;
                Siguiente += Principal.Height + 9;
                if (Principal != _SubmenuAbierto) continue;
                foreach (Button Hijo in _Submenus[Principal])
                {
                    Posiciones[Hijo] = Siguiente - 2;
                    Siguiente += Hijo.Height + 7;
                }
            }
            int TopCerrar = Math.Max(Siguiente + 3, Visible - Alto - 16);
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            int Contenido = TopCerrar + Alto + 16;
            bool Desborda = Contenido > Visible && Visible > 0;
            ContabilidadVsbMenu.Visible = Desborda;
            int Desplazamiento = 0;
            if (Desborda)
            {
                ContabilidadVsbMenu.Minimum = 0;
                ContabilidadVsbMenu.Maximum = Contenido - 1;
                ContabilidadVsbMenu.LargeChange = Visible;
                if (ContabilidadVsbMenu.Value > Contenido - Visible) ContabilidadVsbMenu.Value = Contenido - Visible;
                Desplazamiento = ContabilidadVsbMenu.Value;
            }
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            foreach (KeyValuePair<Control, int> Posicion in Posiciones) Posicion.Key.Top = Posicion.Value - Desplazamiento;
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            ContabilidadBtnCerrarSesion.Top = TopCerrar - Desplazamiento;
        }

        // La rueda del mouse sobre el menú mueve la barra
        private void ContabilidadMetRuedaMenu(object Remitente, MouseEventArgs Evento)
        {
            if (!ContabilidadVsbMenu.Visible) return;
            int Maximo = ContabilidadVsbMenu.Maximum - ContabilidadVsbMenu.LargeChange + 1;
            int Nuevo = ContabilidadVsbMenu.Value - Math.Sign(Evento.Delta) * ContabilidadVsbMenu.SmallChange * 3;
            ContabilidadVsbMenu.Value = Math.Max(0, Math.Min(Maximo, Nuevo));
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026

        private void ContabilidadMetAbrirPaises(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10011, (Button)Remitente, Permisos => new FrmMantenimientoPais(Permisos));
        }

        private void ContabilidadMetAbrirModulosContables(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10012, (Button)Remitente, Permisos => new FrmMantenimientoModuloContable(Permisos));
        }

        private void ContabilidadMetAbrirVidaUtil(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10013, (Button)Remitente, Permisos => new FrmMantenimientoVidaUtil(Permisos));
        }

        private void ContabilidadMetAbrirCuentas(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10014, (Button)Remitente, Permisos => new FrmMantenimientoCuenta(Permisos));
        }

        private void ContabilidadMetAbrirCuentasModulo(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10015, (Button)Remitente, Permisos => new FrmAsignacionCuentaModulo(Permisos));
        }

        private void ContabilidadMetAbrirPolizasLocales(object Remitente, EventArgs Evento)
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 28/10/2026
            ContabilidadMetAbrirPantalla(10001, (Button)Remitente, Permisos => new FrmPolizas(Permisos, _IdModulo));
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 28/10/2026
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        }

        private void ContabilidadMetAbrirPresupuestos(object Remitente, EventArgs Evento)
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ContabilidadMetAbrirPantalla(10002, (Button)Remitente, Permisos => new FrmPresupuestos(Permisos));
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        }

        private void ContabilidadMetAbrirActivosFijos(object Remitente, EventArgs Evento)
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ContabilidadMetAbrirPantalla(10003, (Button)Remitente, Permisos => new FrmActivosFijos(Permisos));
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        }

        private void ContabilidadMetAbrirCierreContable(object Remitente, EventArgs Evento)
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ContabilidadMetAbrirPantalla(10004, (Button)Remitente, Permisos => new FrmCierreContable(Permisos));
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        }

        private void ContabilidadMetAbrirEstadosFinancieros(object Remitente, EventArgs Evento)
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ContabilidadMetAbrirPantalla(10005, (Button)Remitente, Permisos => new FrmEstadosFinancieros(Permisos));
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        }

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
        // Reporte de pólizas locales
        private void ContabilidadMetAbrirReportePolizas(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10001, (Button)Remitente, ClsReportePolizasContabilidad.ContabilidadFuncCrearReporte);
        }

        // Reporte de presupuestos
        private void ContabilidadMetAbrirReportePresupuestos(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10002, (Button)Remitente, ClsReportePresupuestosContabilidad.ContabilidadFuncCrearReporte);
        }

        // Reporte de activos fijos
        private void ContabilidadMetAbrirReporteActivosFijos(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10003, (Button)Remitente, ClsReporteActivosFijosContabilidad.ContabilidadFuncCrearReporte);
        }

        // Reporte de cierre contable
        private void ContabilidadMetAbrirReporteCierre(object Remitente, EventArgs Evento)
        {
            ContabilidadMetAbrirPantalla(10004, (Button)Remitente, ClsReporteCierreContabilidad.ContabilidadFuncCrearReporte);
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026

        private void ContabilidadMetConfigurarEstadosMenu()
        {
            Button[] Botones = { ContabilidadBtnPolizasLocales, ContabilidadBtnPresupuestos,
                ContabilidadBtnActivosFijos, ContabilidadBtnCierreContable, ContabilidadBtnEstadosFinancieros,
                ContabilidadBtnCatalogos,
                ContabilidadBtnPaises,
                ContabilidadBtnModulosContables,
                ContabilidadBtnVidaUtil,
                ContabilidadBtnCuentas,
                // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
                ContabilidadBtnCuentasModulo,
                ContabilidadBtnCerrarSesion,
                // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
                // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
                ContabilidadBtnPolizasLocalesGestion, ContabilidadBtnPolizasLocalesReporte,
                ContabilidadBtnPresupuestosGestion, ContabilidadBtnPresupuestosReporte,
                ContabilidadBtnActivosFijosGestion, ContabilidadBtnActivosFijosReporte,
                ContabilidadBtnCierreContableGestion, ContabilidadBtnCierreContableReporte };
                // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
            string[] Titulos = { "Pólizas locales", "Presupuestos", "Activos fijos",
                "Cierre contable", "Estados financieros", "Catálogos", "Países", "Módulos contables", "Vida útil", "Cuentas contables", "Cuentas por módulo",
                "Cerrar sesión",
                // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
                // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
                "Gestionar", "Reporte", "Gestionar", "Reporte", "Gestionar", "Reporte", "Gestionar", "Reporte" };
                // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
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
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            // Un botón con submenú se marca cuando la pantalla activa es de sus hijos
            Button[] Hijos;
            bool TieneSubmenu = _Submenus.TryGetValue(Boton, out Hijos);
            bool Activo = Boton == _BotonActivo || (TieneSubmenu && _BotonActivo != null && Array.IndexOf(Hijos, _BotonActivo) >= 0);
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            if (Boton.Enabled && (Boton == _BotonSobre || Activo))
            {
                using (SolidBrush Fondo = new SolidBrush(Color.FromArgb(65, 70, 104, 125)))
                    Evento.Graphics.FillRectangle(Fondo, Boton.ClientRectangle);
            }
            if (Boton.Enabled && Activo)
            {
                using (SolidBrush Acento = new SolidBrush(Color.FromArgb(197, 143, 62)))
                    Evento.Graphics.FillRectangle(Acento, 0, 0, 4, Boton.Height);
            }
            Rectangle Etiqueta = new Rectangle(60, 4, Boton.Width - 64, Boton.Height - 8);
            using (SolidBrush Fondo = new SolidBrush(Boton.Enabled ? Color.FromArgb(70, 104, 125) : Color.FromArgb(96, 96, 96)))
                Evento.Graphics.FillRectangle(Fondo, Etiqueta);
            Color Texto = !Boton.Enabled ? Color.FromArgb(210, 210, 210) :
                Activo ? Color.FromArgb(197, 143, 62) : Color.FromArgb(212, 222, 228);
            TextRenderer.DrawText(Evento.Graphics, Boton.AccessibleName, Boton.Font,
                Etiqueta, Texto, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            if (TieneSubmenu)
            {
                int X = Boton.Width - 26, Y = Boton.Height / 2;
                Point[] Flecha = Boton == _SubmenuAbierto
                // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
                    ? new[] { new Point(X, Y - 3), new Point(X + 10, Y - 3), new Point(X + 5, Y + 4) }
                    : new[] { new Point(X + 2, Y - 6), new Point(X + 2, Y + 6), new Point(X + 9, Y) };
                using (SolidBrush Pincel = new SolidBrush(Texto)) Evento.Graphics.FillPolygon(Pincel, Flecha);
            }
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAbrirPantalla(int Codigo, Button Boton, Func<ClsModeloPermisosPantallaContabilidad, Form> Crear)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            ContabilidadMetActualizarSeguridad(true);
            if (!_Estado.Permisos[Codigo].TieneAcceso) return;
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            // Un reporte que el módulo aún no entrega solo avisa
            Form Formulario = Crear(_Estado.Permisos[Codigo]);
            if (Formulario == null)
            {
                MessageBox.Show(this, "Este reporte todavía no está disponible.", Boton.AccessibleName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            Button Anterior = _BotonActivo;
            _BotonActivo = Boton;
            Anterior?.Invalidate();
            Boton.Invalidate();
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            using (Formulario)
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            {
                // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
                ContabilidadMetAgregarAyuda(Formulario, Codigo);
                // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
                Formulario.ShowDialog(this);
            }
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            ContabilidadMetActualizarIndicadores();
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
        }

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
        // Cierra el MDI y regresa al login de Seguridad
        private void ContabilidadMetCerrarSesion(object Remitente, EventArgs Evento)
        {
            DialogResult Resultado = MessageBox.Show(this, "¿Seguro que desea cerrar sesión?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Resultado == DialogResult.Yes) Close();
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026

        private void ContabilidadMetMostrarAyuda(object Remitente, EventArgs Evento)
        {
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
            ContabilidadMetAbrirAyuda(this, 10000, GetType().Name);
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
        }

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
        private static void ContabilidadMetAbrirAyuda(Control Duenio, int Codigo, string NombreFormulario)
        {
            try
            {
                ClsDestinoAyuda Destino = new ClsCtrlAyuda().NavegadorFuncObtenerDestino(Codigo, NombreFormulario);

                if (Destino.RutaArchivo == "")
                {
                    MessageBox.Show(Duenio, Destino.Mensaje, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Destino.Pagina == "")
                {
                    Help.ShowHelp(Duenio, Destino.RutaArchivo);
                }
                else
                {
                    Help.ShowHelp(Duenio, Destino.RutaArchivo, HelpNavigator.Topic, Destino.Pagina);
                }
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(Duenio, "No se pudo abrir la ayuda de Contabilidad.\n\n" + Excepcion.Message,
                    "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ContabilidadMetAgregarAyuda(Form Formulario, int Codigo)
        {
            Formulario.KeyPreview = true;
            Formulario.KeyDown += (Remitente, Tecla) =>
            {
                if (Tecla.KeyCode != Keys.F1) return;
                Tecla.Handled = true;
                ContabilidadMetAbrirAyuda(Formulario, Codigo, Formulario.GetType().Name);
            };

            if (Codigo < 10001 || Codigo > 10005) return;

            Button BtnAyuda = new Button
            {
                Name = "ContabilidadBtnAyudaVentana",
                Size = new Size(44, 44),
                FlatStyle = FlatStyle.Flat,
                BackColor = ContabilidadBtnAyuda.BackColor,
                BackgroundImage = ContabilidadBtnAyuda.BackgroundImage,
                BackgroundImageLayout = ImageLayout.Stretch,
                Cursor = Cursors.Hand,
                TabStop = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            BtnAyuda.FlatAppearance.BorderSize = 0;
            new ToolTip().SetToolTip(BtnAyuda, "Ayuda (F1)");
            BtnAyuda.Click += (Remitente, Argumentos) => ContabilidadMetAbrirAyuda(Formulario, Codigo, Formulario.GetType().Name);
            Formulario.Shown += (Remitente, Argumentos) => ContabilidadMetUbicarAyuda(Formulario, BtnAyuda);
        }

        private static void ContabilidadMetUbicarAyuda(Form Formulario, Button BtnAyuda)
        {
            Control Contenedor = Formulario;
            Point Referencia = new Point(Formulario.ClientSize.Width - 40, 40);
            foreach (Control Hijo in Formulario.Controls)
            {
                if (Hijo.Visible && Hijo.Bounds.Contains(Referencia) && Hijo.Controls.Count > 0)
                {
                    Contenedor = Hijo;
                    break;
                }
            }

            Rectangle Zona = new Rectangle(Contenedor.ClientSize.Width - 44 - 18, 12, 44, 44);
            while (Zona.X > Contenedor.ClientSize.Width / 2)
            {
                bool Ocupada = false;
                foreach (Control Hijo in Contenedor.Controls)
                {
                    if (Hijo.Visible && ContabilidadFuncAreaOcupada(Hijo).IntersectsWith(Rectangle.Inflate(Zona, 6, 6)))
                    {
                        Ocupada = true;
                        break;
                    }
                }
                if (!Ocupada)
                {
                    BtnAyuda.Location = Zona.Location;
                    Contenedor.Controls.Add(BtnAyuda);
                    BtnAyuda.BringToFront();
                    return;
                }
                Zona.Offset(-10, 0);
            }
            BtnAyuda.Dispose();
        }

        private static Rectangle ContabilidadFuncAreaOcupada(Control Hijo)
        {
            Label Etiqueta = Hijo as Label;
            if (Etiqueta == null || Etiqueta.AutoSize || Etiqueta.Image != null) return Hijo.Bounds;

            int Ancho = Math.Min(Hijo.Width, TextRenderer.MeasureText(Etiqueta.Text, Etiqueta.Font).Width + 12);
            ContentAlignment Alineacion = Etiqueta.TextAlign;
            bool Derecha = Alineacion == ContentAlignment.TopRight || Alineacion == ContentAlignment.MiddleRight || Alineacion == ContentAlignment.BottomRight;
            bool Centro = Alineacion == ContentAlignment.TopCenter || Alineacion == ContentAlignment.MiddleCenter || Alineacion == ContentAlignment.BottomCenter;
            int Izquierda = Derecha ? Hijo.Right - Ancho : Centro ? Hijo.Left + (Hijo.Width - Ancho) / 2 : Hijo.Left;
            return new Rectangle(Izquierda, Hijo.Top, Ancho, Hijo.Height);
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 07/11/2026
    }
}