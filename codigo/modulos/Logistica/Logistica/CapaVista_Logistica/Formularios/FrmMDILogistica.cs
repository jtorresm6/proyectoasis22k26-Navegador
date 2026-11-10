/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA
 * FORMULARIO: FrmMDILogistica
 * DESCRIPCIÓN:
 * Navegador con menús tipo acordeón y vista compacta, sin alterar las
 * imágenes ni los eventos de los formularios del módulo.
 * RESPONSABLE: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using CapaVista_Inventario_Logistica.Formularios;
using CapaVista_Traslados_Logistica.Formularios;
using CapaVista_MuestreoAuditoria_Logistica.Formularios;
using CapaVista_CierreInventario_Logistica.Formularios;
using CapaVista_Enlace_Logistica.Formas;
using CapaVista_TransporteInterno_Logistica.Formularios;
using CapaVista_TransporteExterno_Logistica.Formularios;

namespace CapaVista_Logistica.Formularios
{
    public partial class FrmMDILogistica : Form
    {
        // Los botones originales miden 264 x 53 y contienen texto en sus PNG.
        // No se redimensionan ni se les escribe texto por encima.
        private const int LogisticaAnchoExpandido = 300;
        private const int LogisticaAnchoContraido = 64;
        private const int LogisticaMargenMenu = 8;
        private const int LogisticaSeparacion = 7;

        private readonly Color LogisticaColorTurquesa =
            Color.FromArgb(62, 138, 132);

        private Button[] LogisticaBotonesMenu;
        private Panel[] LogisticaPanelesMenu;
        private Label[] LogisticaFlechasMenu;
        private Panel[] LogisticaIndicadores;

        // Se usan los nombres solamente para la ayuda al pasar el mouse.
        private readonly string[] LogisticaNombresMenu =
        {
            "Mantenimientos",
            "Gestión de Inventario y Catálogo",
            "Cierre y Auditoría de Inventario",
            "Gestión de Traslados",
            "Gestión de Viajes y Despacho",
            "Transporte Externo y Monitoreo",
            "Administración, Costos e Incidencias"
        };

        private Panel LogisticaPnlContenidoNavegador;
        private Panel LogisticaPnlSubMenuEmergente;
        private ToolTip LogisticaAyudasMenu;
        private readonly Dictionary<Control, Point> LogisticaUbicacionesOriginales =
            new Dictionary<Control, Point>();

        private int LogisticaIndiceAbierto = -1;
        private bool LogisticaMenuContraido = false;
        private ImageLayout LogisticaDisposicionOriginalFondo;

        // ============================================
        // CONSTRUCTOR
        // ============================================

        public FrmMDILogistica()
        {
            InitializeComponent();

            // No modificar el Designer cuando Visual Studio lo abre.
            if (System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            LogisticaPnlMDI.Dock = DockStyle.Fill;
            MetPanelesSubMenu();
        }

        public void LogisticaMetActualizarInfoUsuario(string Nombre, string Rol)
        {
            LogisticaLblUsuario.Text = Nombre;
            LogisticaLblRolUsuario.Text = Rol;
            LogisticaLbUsuarioRol.Left = LogisticaLblUsuario.Right + 30;
        }

        // ============================================
        // CONFIGURAR NAVEGACIÓN
        // ============================================

        private void MetPanelesSubMenu()
        {
            LogisticaBotonesMenu = new[]
            {
                LogisticaBtnMenuMantenimientos,
                LogisticaBtnMenuGestionInventario,
                LogisticaBtnMenuCierreInventario,
                LogisticaBtnMenuGestionTraslados,
                LogisticaBtnMenuGestionViajes,
                LogisticaBtnMenuTransporteExterno,
                LogisticaBtnMenuAdministracion
            };

            LogisticaPanelesMenu = new[]
            {
                LogisticaPnlMenuMantenimientos,
                LogisticaPnlMenuGestionInventario,
                LogisticaPnlMenuCierreInventario,
                LogisticaBtnMenuTraslados,
                LogisticaPnlMenuGesionViajes,
                LogisticaPnlMenuTransporteExterno,
                LogisticaPnlMenuAdministracionCostos
            };

            // Se guardan las ubicaciones del Designer para que la hamburguesa
            // siempre vuelva exactamente al mismo lugar y no acumule desfases.
            Control[] controlesCabecera =
            {
                LogisticaPnlBanner,
                LogisticaBtnHamburguesa,
                LogisticaLblTextoUsuario,
                LogisticaLblUsuario,
                LogisticaLblTextoRol,
                LogisticaLblRolUsuario
            };

            foreach (Control control in controlesCabecera)
                LogisticaUbicacionesOriginales.Add(control, control.Location);

            LogisticaDisposicionOriginalFondo =
                LogisticaPnlNavegador.BackgroundImageLayout;

            LogisticaPnlNavegador.SuspendLayout();
            LogisticaPnlNavegador.Width = LogisticaAnchoExpandido;
            LogisticaPnlNavegador.AutoScroll = true;
            LogisticaPnlNavegador.AutoScrollMinSize = Size.Empty;
            LogisticaPnlNavegador.AutoScrollMargin = Size.Empty;

            // El contenedor interno elimina el movimiento errático de Top
            // producido al modificar controles dentro de un Panel con AutoScroll.
            LogisticaPnlContenidoNavegador = new Panel();
            LogisticaPnlContenidoNavegador.Name = "LogisticaPnlContenidoNavegador";
            LogisticaPnlContenidoNavegador.Location = Point.Empty;
            LogisticaPnlContenidoNavegador.Size = new Size(
                LogisticaAnchoExpandido - SystemInformation.VerticalScrollBarWidth - 3,
                1);
            LogisticaPnlContenidoNavegador.BackColor = Color.Transparent;
            LogisticaPnlContenidoNavegador.AutoScroll = false;
            LogisticaPnlContenidoNavegador.Anchor =
                AnchorStyles.Top | AnchorStyles.Left;

            // Transferir los mismos controles creados por el Designer.
            // No se crean botones sustitutos ni se pierden sus imágenes.
            LogisticaPnlContenidoNavegador.Controls.Add(LogisticaPbLogo);
            foreach (Button boton in LogisticaBotonesMenu)
                LogisticaPnlContenidoNavegador.Controls.Add(boton);

            foreach (Panel submenu in LogisticaPanelesMenu)
            {
                LogisticaPnlContenidoNavegador.Controls.Add(submenu);
                submenu.Visible = false;
                submenu.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }

            LogisticaPnlContenidoNavegador.Controls.Add(LogisticaBtnCerrarSesion);
            LogisticaPnlNavegador.Controls.Add(LogisticaPnlContenidoNavegador);
            LogistincaPnlMenuFin.Visible = false;
            LogisticaPnlNavegador.ResumeLayout(true);

            // Este panel solo aparece al usar el menú estrecho. Permite
            // ver los submenús completos sin aplastar los iconos ni las fotos.
            LogisticaPnlSubMenuEmergente = new Panel();
            LogisticaPnlSubMenuEmergente.Name = "LogisticaPnlSubMenuEmergente";
            LogisticaPnlSubMenuEmergente.BackColor =
                Color.FromArgb(251, 246, 242);
            LogisticaPnlSubMenuEmergente.BorderStyle = BorderStyle.FixedSingle;
            LogisticaPnlSubMenuEmergente.AutoScroll = true;
            LogisticaPnlSubMenuEmergente.Visible = false;
            LogisticaPnlSubMenuEmergente.Size = new Size(293, 200);
            LogisticaPnlMDI.Controls.Add(LogisticaPnlSubMenuEmergente);

            LogisticaFlechasMenu = new Label[LogisticaBotonesMenu.Length];
            LogisticaIndicadores = new Panel[LogisticaBotonesMenu.Length];
            LogisticaAyudasMenu = new ToolTip();

            for (int i = 0; i < LogisticaBotonesMenu.Length; i++)
            {
                LogisticaMetAgregarIndicadores(i);
                LogisticaAyudasMenu.SetToolTip(
                    LogisticaBotonesMenu[i], LogisticaNombresMenu[i]);
            }

            LogisticaAyudasMenu.SetToolTip(LogisticaBtnCerrarSesion,
                "Cerrar sesión");
            Disposed += (s, e) => LogisticaAyudasMenu.Dispose();

            // No se agrega dos veces ningún evento de menú.
            LogisticaBtnMenuMantenimientos.Click -= LogisticaBtnCatalogo_Click;
            LogisticaBtnMenuMantenimientos.Click += LogisticaBtnCatalogo_Click;

            LogisticaBtnMenuGestionInventario.Click -= LogisticaMetClickInventario;
            LogisticaBtnMenuGestionInventario.Click += LogisticaMetClickInventario;

            LogisticaBtnMenuCierreInventario.Click -= LogisticaMetClickCierres;
            LogisticaBtnMenuCierreInventario.Click += LogisticaMetClickCierres;

            LogisticaBtnMenuGestionTraslados.Click -= LogisticaMetClickTraslados;
            LogisticaBtnMenuGestionTraslados.Click += LogisticaMetClickTraslados;

            LogisticaBtnMenuGestionViajes.Click -= LogisticaMetClickViajes;
            LogisticaBtnMenuGestionViajes.Click += LogisticaMetClickViajes;

            LogisticaBtnMenuTransporteExterno.Click -= LogisticaMetClickExterno;
            LogisticaBtnMenuTransporteExterno.Click += LogisticaMetClickExterno;

            LogisticaBtnMenuAdministracion.Click -= LogisticaMetClickAdministracion;
            LogisticaBtnMenuAdministracion.Click += LogisticaMetClickAdministracion;

            LogisticaMetConectarFormularios();

            LogisticaPnlNavegador.Scroll += (s, e) =>
                LogisticaMetPosicionarSubMenuEmergente();
            LogisticaPnlMDI.Resize += (s, e) =>
                LogisticaMetPosicionarSubMenuEmergente();

            MetOrganizarMenu();
        }

        // ============================================
        // FLECHAS E INDICADORES
        // ============================================

        private void LogisticaMetAgregarIndicadores(int indice)
        {
            Button boton = LogisticaBotonesMenu[indice];

            // La flecha va al costado del PNG, NO encima de las letras
            // que ya están dibujadas en la imagen del botón.
            Label flecha = new Label();
            flecha.Name = "LogisticaLblFlecha" + indice;
            flecha.AutoSize = false;
            flecha.Size = new Size(14, boton.Height);
            flecha.BackColor = Color.Transparent;
            flecha.ForeColor = Color.FromArgb(242, 216, 197);
            flecha.Font = new Font("Segoe UI Symbol", 10f, FontStyle.Bold);
            flecha.TextAlign = ContentAlignment.MiddleCenter;
            flecha.Cursor = Cursors.Hand;
            flecha.Click += (s, e) => boton.PerformClick();
            LogisticaPnlContenidoNavegador.Controls.Add(flecha);

            Panel indicador = new Panel();
            indicador.Name = "LogisticaPnlIndicador" + indice;
            indicador.Width = 4;
            indicador.BackColor = LogisticaColorTurquesa;
            indicador.Visible = false;
            indicador.Cursor = Cursors.Hand;
            indicador.Click += (s, e) => boton.PerformClick();
            LogisticaPnlContenidoNavegador.Controls.Add(indicador);

            flecha.BringToFront();
            indicador.BringToFront();
            LogisticaFlechasMenu[indice] = flecha;
            LogisticaIndicadores[indice] = indicador;
        }

        private void LogisticaMetActualizarIndicadores()
        {
            for (int i = 0; i < LogisticaBotonesMenu.Length; i++)
            {
                Button boton = LogisticaBotonesMenu[i];
                Label flecha = LogisticaFlechasMenu[i];
                Panel indicador = LogisticaIndicadores[i];
                bool activo = i == LogisticaIndiceAbierto;

                flecha.Text = activo ? "▼" : "▶";
                flecha.Left = boton.Right - 6;
                flecha.Top = boton.Top;
                flecha.Height = boton.Height;
                flecha.Visible = !LogisticaMenuContraido;
                flecha.BringToFront();

                indicador.SetBounds(boton.Left - 2, boton.Top + 3,
                    4, boton.Height - 6);
                indicador.Visible = activo;
                indicador.BringToFront();
            }
        }

        // ============================================
        // ACOMODAR EL ACORDEÓN SIN ALTERAR LAS IMÁGENES
        // ============================================

        private void MetOrganizarMenu()
        {
            if (LogisticaPnlContenidoNavegador == null)
                return;

            LogisticaPnlContenidoNavegador.SuspendLayout();

            LogisticaPnlContenidoNavegador.Width =
                LogisticaMenuContraido
                    ? 60
                    : LogisticaAnchoExpandido -
                      SystemInformation.VerticalScrollBarWidth - 3;

            // El logotipo mantiene su posición de diseño.
            LogisticaPbLogo.Visible = !LogisticaMenuContraido;
            LogisticaPbLogo.Location = new Point(77, 11);

            int posicionY = LogisticaMenuContraido
                ? 10 : LogisticaPbLogo.Bottom + 6;

            for (int i = 0; i < LogisticaBotonesMenu.Length; i++)
            {
                Button boton = LogisticaBotonesMenu[i];
                Panel subMenu = LogisticaPanelesMenu[i];

                // Las imágenes de los encabezados ya incluyen textos e iconos.
                // Mantener su altura y su ancho ORIGINAL en modo expandido.
                boton.SetBounds(
                    LogisticaMenuContraido ? 5 : LogisticaMargenMenu,
                    posicionY,
                    LogisticaMenuContraido ? 54 : 264,
                    53);

                // El PNG mantiene su escala, se recorta en modo icono.
                // No asignar boton.Text ni cambiar BackgroundImage.
                posicionY += boton.Height + LogisticaSeparacion;

                if (subMenu.Parent == LogisticaPnlContenidoNavegador)
                    subMenu.Location = new Point(LogisticaMargenMenu,
                        posicionY);

                if (!LogisticaMenuContraido &&
                    LogisticaIndiceAbierto == i)
                    posicionY += subMenu.Height + LogisticaSeparacion;
            }

            LogisticaBtnCerrarSesion.SetBounds(
                LogisticaMenuContraido ? 5 : LogisticaMargenMenu,
                posicionY,
                LogisticaMenuContraido ? 54 : 264,
                53);

            // ¡No asignar Text = "Cerrar Sesión"! Ya está escrito en el PNG.
            posicionY += LogisticaBtnCerrarSesion.Height + 14;
            LogisticaPnlContenidoNavegador.Height = posicionY;
            LogisticaPnlContenidoNavegador.ResumeLayout(true);

            // La barra vertical se controla automáticamente por la altura
            // del panel interno. No se usa AutoScrollMinSize = 3100.
            LogisticaMetActualizarIndicadores();
            LogisticaMetPosicionarSubMenuEmergente();
        }

        // ============================================
        // ABRIR/CERRAR MENÚS
        // ============================================

        private void MetEsconderSubMenu()
        {
            LogisticaIndiceAbierto = -1;
            LogisticaMetAplicarVisibilidadSubMenus();
            MetOrganizarMenu();
        }

        private void metShowSubMenu(Panel subMenu)
        {
            int indice = Array.IndexOf(LogisticaPanelesMenu, subMenu);
            if (indice < 0)
                return;

            LogisticaIndiceAbierto = LogisticaIndiceAbierto == indice
                ? -1 : indice;

            LogisticaMetAplicarVisibilidadSubMenus();
            MetOrganizarMenu();

            // Facilitar el acceso a un encabezado situado al final del menú.
            if (!LogisticaMenuContraido && LogisticaIndiceAbierto >= 0)
                LogisticaPnlNavegador.ScrollControlIntoView(
                    LogisticaBotonesMenu[LogisticaIndiceAbierto]);
        }

        private void LogisticaMetAplicarVisibilidadSubMenus()
        {
            for (int i = 0; i < LogisticaPanelesMenu.Length; i++)
            {
                Panel subMenu = LogisticaPanelesMenu[i];
                bool activo = LogisticaIndiceAbierto == i;
                Control padre = (activo && LogisticaMenuContraido)
                    ? (Control)LogisticaPnlSubMenuEmergente
                    : (Control)LogisticaPnlContenidoNavegador;

                // Cambiar Parent solo cuando haga falta para evitar
                // parpadeos y la reordenación de los controles internos.
                if (subMenu.Parent != padre)
                    subMenu.Parent = padre;

                subMenu.Visible = activo;
            }

            LogisticaPnlSubMenuEmergente.Visible =
                LogisticaMenuContraido && LogisticaIndiceAbierto >= 0;
        }

        // ============================================
        // VISTA COMPACTA: LOS BOTONES SIGUEN SIENDO ICONOS
        // ============================================

        private void LogisticaMetCambiarNavegador(bool contraer)
        {
            if (contraer == LogisticaMenuContraido)
                return;

            LogisticaMenuContraido = contraer;
            LogisticaPnlNavegador.Width = contraer
                ? LogisticaAnchoContraido : LogisticaAnchoExpandido;

            // Con la barra compacta recortar el fondo en lugar de estirarlo.
            LogisticaPnlNavegador.BackgroundImageLayout = contraer
                ? ImageLayout.None : LogisticaDisposicionOriginalFondo;

            // Restaurar siempre la posición original, sin sumar
            // desplazamientos acumulativos después de varios clics.
            int desplazamiento = LogisticaPnlNavegador.Width -
                LogisticaAnchoExpandido;
            foreach (KeyValuePair<Control, Point> elemento in
                     LogisticaUbicacionesOriginales)
            {
                elemento.Key.Location = new Point(
                    elemento.Value.X + desplazamiento,
                    elemento.Value.Y);
            }

            LogisticaPnlNavegador.AutoScrollPosition = Point.Empty;
            LogisticaMetAplicarVisibilidadSubMenus();
            MetOrganizarMenu();
        }

        private void LogisticaMetPosicionarSubMenuEmergente()
        {
            if (LogisticaPnlSubMenuEmergente == null ||
                !LogisticaMenuContraido || LogisticaIndiceAbierto < 0)
                return;

            Panel subMenu = LogisticaPanelesMenu[LogisticaIndiceAbierto];
            Button boton = LogisticaBotonesMenu[LogisticaIndiceAbierto];

            // Vista emergente de ancho completo: el contenido original
            // permanece accesible, incluso si tiene muchos botones.
            int alturaDisponible = Math.Max(90,
                LogisticaPnlMDI.ClientSize.Height - 48);
            int alto = Math.Min(subMenu.Height + 10, alturaDisponible);
            int ancho = 293;

            LogisticaPnlSubMenuEmergente.SuspendLayout();
            LogisticaPnlSubMenuEmergente.Size = new Size(ancho, alto);
            LogisticaPnlSubMenuEmergente.AutoScrollMinSize = Size.Empty;
            subMenu.Location = new Point(4, 4);
            LogisticaPnlSubMenuEmergente.AutoScrollMinSize =
                new Size(0, subMenu.Height + 10);
            LogisticaPnlSubMenuEmergente.ResumeLayout(true);

            // Posición respecto del encabezado que activó el submenú.
            Point puntoBoton = LogisticaPnlMDI.PointToClient(
                boton.PointToScreen(Point.Empty));
            int superior = Math.Max(34, Math.Min(puntoBoton.Y,
                Math.Max(34, LogisticaPnlMDI.ClientSize.Height - alto - 5)));
            int izquierda = LogisticaPnlNavegador.Right + 3;

            LogisticaPnlSubMenuEmergente.Location =
                new Point(izquierda, superior);
            LogisticaPnlSubMenuEmergente.Visible = true;
            LogisticaPnlSubMenuEmergente.BringToFront();
        }

        // ============================================
        // EVENTOS DE LOS ENCABEZADOS
        // ============================================

        private void LogisticaMetClickInventario(object sender, EventArgs e)
        {
            metShowSubMenu(LogisticaPnlMenuGestionInventario);
        }

        private void LogisticaMetClickCierres(object sender, EventArgs e)
        {
            metShowSubMenu(LogisticaPnlMenuCierreInventario);
        }

        private void LogisticaMetClickTraslados(object sender, EventArgs e)
        {
            metShowSubMenu(LogisticaBtnMenuTraslados);
        }

        private void LogisticaMetClickViajes(object sender, EventArgs e)
        {
            metShowSubMenu(LogisticaPnlMenuGesionViajes);
        }

        private void LogisticaMetClickExterno(object sender, EventArgs e)
        {
            metShowSubMenu(LogisticaPnlMenuTransporteExterno);
        }

        private void LogisticaMetClickAdministracion(object sender, EventArgs e)
        {
            metShowSubMenu(LogisticaPnlMenuAdministracionCostos);
        }

        // ========================================================
        // CONEXIONES DE FORMULARIOS: conservan métodos originales.
        // ========================================================

        private void LogisticaMetConectarFormularios()
        {
            LogisticaMetConectarClick(LogisticaBtnBodegas, LogisticaBtnBodegas_Click);
            LogisticaMetConectarClick(LogisticaBtnSucursales, LogisticaBtnSucursales_Click);
            LogisticaMetConectarClick(LogisticaBtnCategoria, LogisticaBtnCategoria_Click);
            LogisticaMetConectarClick(LogisticaBtnLinea, LogisticaBtnLinea_Click);
            LogisticaMetConectarClick(LogisticaBtnMarca, LogisticaBtnMarca_Click);
            LogisticaMetConectarClick(LogisticaBtnDetalleMovimiento, LogisticaBtnDetalleMovimiento_Click);
            LogisticaMetConectarClick(LogisticaBtnTipoMovimiento, LogisticaTipoMovimiento_Click);
            LogisticaMetConectarClick(LogisticaBtnCamaraRefrigeración, LogisticaBtnCamaraRefrigeración_Click);
            LogisticaMetConectarClick(LogisticaBtnConductor, LogisticaBtnConductor_Click);
            LogisticaMetConectarClick(LogisticaBtnEstadoTransporte, LogisticaBtnEstadoTransporte_Click);
            LogisticaMetConectarClick(LogisticaBtnTipoTransporte, LogisticaBtnTipoTransporte_Click);
            LogisticaMetConectarClick(LogisticaBtnTransporte, LogisticaBtnTransporte_Click);

            // El original tenía dos manejadores para Producto.
            // Usamos solo el primero para que no se abra dos veces.
            LogisticaBtnProducto.Click -= LogisticaBtnProducto_Click_1;
            LogisticaMetConectarClick(LogisticaBtnProducto, LogisticaBtnProducto_Click);
            LogisticaMetConectarClick(LogisticaBtnStock, LogisticaBtnStock_Click);

            LogisticaMetConectarClick(LogisticaBtnEncabezadoViaje, LogisticaBtnEncabezadoViaje_Click);
            LogisticaMetConectarClick(LogisticaBtnDetalleViaje, LogisticaBtnDetalleViaje_Click);
            LogisticaMetConectarClick(LogisticaBtnDetalleViajeProducto, LogisticaBtnDetalleViajeProducto_Click);
            LogisticaMetConectarClick(LogisticaBtnTelemetriaTransporte, LogisticaBtnTelemetriaTransporte_Click);
            LogisticaMetConectarClick(LogisticaBtnAlertaCadenaFrio, LogisticaBtnAlertaCadenaFrio_Click);
            LogisticaMetConectarClick(LogisticaBtnEnlaceAOtrosModulos, LogisticaBtnEnlaceAOtrosModulos_Click);
            LogisticaMetConectarClick(LogisticaBtnCerrarSesion, LogisticaBtnCerrarSesion_Click);
        }

        private void LogisticaMetConectarClick(Button boton, EventHandler evento)
        {
            boton.Click -= evento;
            boton.Click += evento;
        }

        // ============================================
        // EVENTO DEL MENÚ MANTENIMIENTOS
        // ============================================

        public void LogisticaBtnCatalogo_Click(
            object sender, EventArgs e)
        {
            metShowSubMenu(
                LogisticaPnlMenuMantenimientos);
        }

        // ============================================
        // BOTÓN HAMBURGUESA
        // ============================================

        public void LogisticaBtnHamburguesa_Click(
            object sender, EventArgs e)
        {
            LogisticaMetCambiarNavegador(!LogisticaMenuContraido);
        }

        // ============================================
        // EVENTO DEL PANEL NAVEGADOR
        // ============================================

        private void LogisticaPnlNavegador_Paint(
            object sender, PaintEventArgs e)
        {
            // Sin implementación.
        }

        // Procedimiento para abrir formularios secundarios.
        public void LogisticaMetAbrirFormulario(Form Formulario)
        {
            using (Formulario)
            {
                Formulario.StartPosition = FormStartPosition.CenterParent;

                Formulario.ShowDialog(this);
            }
        }

        private void LogisticaBtnBodegas_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoBodegas());
        }

        private void LogisticaBtnSucursales_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoSucursales());
        }

        private void LogisticaBtnCategoria_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoCategoria());
        }

        private void LogisticaBtnCerrarSesion_Click(object sender, EventArgs e)
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

        private void LogisticaBtnLinea_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoLinea());
        }

        private void LogisticaBtnMarca_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoMarca());

        }

        private void LogisticaBtnDetalleMovimiento_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoDetalleMovimiento());
        }

        private void LogisticaTipoMovimiento_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoTipoMovimiento());
        }

        private void LogisticaBtnCamaraRefrigeración_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoCamaraRefrigeracion());
        }

        private void LogisticaBtnConductor_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoConductor());
        }

        private void LogisticaBtnEstadoTransporte_Click(object sender, EventArgs e)
        {

        }

        private void LogisticaBtnTipoTransporte_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoTipoTransporte());
        }

        private void LogisticaBtnTransporte_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoTransporte());
        }

        private void LogisticaBtnProducto_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoProducto());
        }


        private void LogisticaBtnEnlaceAOtrosModulos_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new Frm_Solicitud_Pago());
        }

        private void LogisticaBtnStock_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoStock());
        }

        private void LogisticaBtnEncabezadoViaje_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmEncabezadoViaje());
        }

        private void LogisticaBtnDetalleViaje_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmDetalleViaje());
        }

        private void LogisticaBtnDetalleViajeProducto_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmDetalleViajeProducto());
        }

        private void LogisticaBtnTelemetriaTransporte_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmTelemetriaTransporte());
        }

        private void LogisticaBtnAlertaCadenaFrio_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmAlertaCadenaFrio());

        }

        private void LogisticaBtnProducto_Click_1(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoProducto());
        }

        private void LogisticaBtnMovimientoInventario_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoMovimientoInventario());
        }

        private void LogisticaBtnSaldos_Click(object sender, EventArgs e)
        {

        }

        private void LogisticaBtnHistoricoInventario_Click(object sender, EventArgs e)
        {

        }

        private void LogisticaBtnCierreInventario_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmCierreInventario());
        }

        private void LogisticaBtnConsultaCierre_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmConsultaCierres());
        }

        private void LogisticaBtnMuestreoyAuditoria_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmAuditoriaInventario());
        }

        private void LogisticaBtnDetalleMuestreo_Click(object sender, EventArgs e)
        {

        }

        private void LogisticaBtnTraslado_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmTraslados());
        }

        private void LogisticaBtnDetalleTraslado_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmDetalleTraslados());
        }

        private void LogisticaBtnTipodeTraslado_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmTipoTraslado());
        }

        private void LogisticaBtnProductoTraslado_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmProductosATrasladar());
        }

        private void LogisticaBtnRegistroTransporteExterno_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmRegistroTransporteExterno());
        }

        private void LogisticaBtnRutaSeguimiento_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmRutaYSeguimiento());
        }

        private void LogisticaBtnCostoyPago_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmCostosYPagosDelServicio());
        }

        private void LogisticaBtnIndecencia_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmIncidenciasEHistorial());
        }

        private void LogisticaBtnListadoTransporteExterno_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmListadoTransporteExterno());
        }
    }
}
