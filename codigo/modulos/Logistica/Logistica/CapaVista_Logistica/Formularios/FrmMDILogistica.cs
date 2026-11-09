/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA
 * FORMULARIO: FrmMDILogistica
 * DESCRIPCIÓN:
 * Formulario principal con navegación mediante menús desplegables.
 *
 * RESPONSABLE: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * ============================================================================
 */

using System;
using System.Drawing;
using System.Windows.Forms;

// Formularios de los submódulos.
using CapaVista_Inventario_Logistica.Formularios;
using CapaVista_Traslados_Logistica.Formularios;
using CapaVista_MuestreoAuditoria_Logistica.Formularios;
using CapaVista_CierreInventario_Logistica.Formularios;
using CapaVista_Enlace_Logistica.Formas;
using CapaVista_TransporteInterno_Logistica.Formularios;

namespace CapaVista_Logistica.Formularios
{
    public partial class FrmMDILogistica : Form
    {
        // ========================================================
        // NAVEGADOR: estado y elementos visuales
        // ========================================================

        private const int LogisticaAnchoExpandido = 273;
        private const int LogisticaAnchoContraido = 64;

        private static readonly Color LogisticaColorTurquesa =
            Color.FromArgb(62, 138, 132);

        private static readonly Color LogisticaColorSeleccion =
            Color.FromArgb(242, 216, 197);

        private Button[] LogisticaBotonesMenu;
        private Panel[] LogisticaPanelesMenu;
        private Label[] LogisticaFlechasMenu;
        private Panel[] LogisticaBarrasActivas;
        private string[] LogisticaTitulosMenu;
        private int[] LogisticaAnchosMenu;
        private int[] LogisticaIzquierdasMenu;
        private int[] LogisticaAlturasMenu;
        private Padding[] LogisticaRellenosMenu;
        private ContentAlignment[] LogisticaAlineacionesMenu;
        private Color[] LogisticaFondosMenu;

        private bool LogisticaMenuContraido = false;
        private ToolTip LogisticaAyudasMenu;

        // ============================================
        // CONSTRUCTOR PRINCIPAL
        // ============================================

        public FrmMDILogistica()
        {
            InitializeComponent();

            // Ajusta el área visible y el scroll al tamaño real de la ventana.
            LogisticaPnlMDI.Dock = DockStyle.Fill;

            // Configurar los menús desplegables.
            MetPanelesSubMenu();
        }

        // ============================================
        // ACTUALIZAR INFORMACIÓN DEL USUARIO
        // ============================================

        public void LogisticaMetActualizarInfoUsuario(
            string Nombre, string Rol)
        {
            LogisticaLblUsuario.Text = Nombre;
            LogisticaLblRolUsuario.Text = Rol;

            LogisticaLbUsuarioRol.Left =
                LogisticaLblUsuario.Right + 30;
        }

        // ============================================
        // INICIALIZAR SUBMENÚS
        // ============================================

        private void MetPanelesSubMenu()
        {
            // Se mantienen los controles y el diseño del Designer original.
            LogisticaBotonesMenu = new Button[]
            {
                LogisticaBtnMenuMantenimientos,
                LogisticaBtnMenuGestionInventario,
                LogisticaBtnMenuCierreInventario,
                LogisticaBtnMenuGestionTraslados,
                LogisticaBtnMenuGestionViajes,
                LogisticaBtnMenuTransporteExterno,
                LogisticaBtnMenuAdministracion
            };

            LogisticaPanelesMenu = new Panel[]
            {
                LogisticaPnlMenuMantenimientos,
                LogisticaPnlMenuGestionInventario,
                LogisticaPnlMenuCierreInventario,
                LogisticaBtnMenuTraslados,
                LogisticaPnlMenuGesionViajes,
                LogisticaPnlMenuTransporteExterno,
                LogisticaPnlMenuAdministracionCostos
            };

            int cantidad = LogisticaBotonesMenu.Length;
            LogisticaFlechasMenu = new Label[cantidad];
            LogisticaBarrasActivas = new Panel[cantidad];
            LogisticaTitulosMenu = new string[cantidad];
            LogisticaAnchosMenu = new int[cantidad];
            LogisticaIzquierdasMenu = new int[cantidad];
            LogisticaAlturasMenu = new int[cantidad];
            LogisticaRellenosMenu = new Padding[cantidad];
            LogisticaAlineacionesMenu = new ContentAlignment[cantidad];
            LogisticaFondosMenu = new Color[cantidad];

            LogisticaPnlNavegador.AutoScroll = true;
            LogisticaPnlNavegador.AutoScrollMinSize = Size.Empty;
            LogisticaPnlNavegador.AutoScrollMargin = new Size(0, 10);
            LogistincaPnlMenuFin.Visible = false;

            LogisticaAyudasMenu = new ToolTip();
            LogisticaAyudasMenu.SetToolTip(LogisticaBtnCerrarSesion, "Cerrar Sesión");
            this.Disposed += (s, e) => LogisticaAyudasMenu.Dispose();
            LogisticaPnlNavegador.Scroll += (s, e) =>
                LogisticaMetPosicionarSubMenuFlotante();
            LogisticaPnlMDI.Resize += (s, e) =>
                LogisticaMetPosicionarSubMenuFlotante();

            for (int i = 0; i < cantidad; i++)
            {
                Button boton = LogisticaBotonesMenu[i];
                LogisticaTitulosMenu[i] = boton.Text;
                LogisticaAnchosMenu[i] = boton.Width;
                LogisticaIzquierdasMenu[i] = boton.Left;
                LogisticaAlturasMenu[i] = boton.Height;
                LogisticaRellenosMenu[i] = boton.Padding;
                LogisticaAlineacionesMenu[i] = boton.TextAlign;
                LogisticaFondosMenu[i] = boton.BackColor;

                LogisticaAyudasMenu.SetToolTip(boton, boton.Text);
                LogisticaMetAgregarFlecha(boton, i);
            }

            // Si el Designer ya conectó eventos, se evita duplicarlos.
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

            // Mantener activos los formularios que ya funcionaban, aun si
            // el Designer no tiene sus eventos Click configurados.
            LogisticaMetConectarFormularios();

            MetEsconderSubMenu();
            MetOrganizarMenu();
            LogisticaMetActualizarFlechas();
        }

        // ============================================
        // OCULTAR TODOS LOS SUBMENÚS
        // ============================================

        private void MetEsconderSubMenu()
        {
            // Si un submenú estaba flotando, regresarlo al navegador
            // antes de ocultarlo. No se pierden los controles interiores.
            foreach (Panel subMenu in LogisticaPanelesMenu)
            {
                subMenu.Visible = false;

                if (subMenu.Parent != LogisticaPnlNavegador)
                    subMenu.Parent = LogisticaPnlNavegador;
            }
        }

        // ============================================
        // MOSTRAR U OCULTAR SUBMENÚ
        // ============================================

        private void metShowSubMenu(Panel subMenu)
        {
            // Abrir/cerrar sin cambiar el ancho del navegador.
            // En modo compacto, las opciones aparecen completas a la derecha.
            bool abrir = !subMenu.Visible;
            MetEsconderSubMenu();

            if (abrir)
            {
                if (LogisticaMenuContraido)
                    subMenu.Parent = LogisticaPnlMDI;

                subMenu.Visible = true;
            }

            MetOrganizarMenu();
            LogisticaMetActualizarFlechas();
        }

        // ============================================
        // ORGANIZAR LOS MENÚS VERTICALMENTE
        // ============================================

        private void MetOrganizarMenu()
        {
            int scrollAnterior = Math.Max(0,
                -LogisticaPnlNavegador.AutoScrollPosition.Y);

            LogisticaPnlNavegador.SuspendLayout();
            LogisticaPnlNavegador.AutoScrollPosition = Point.Empty;

            // Se conserva la ubicación original del logotipo y se
            // agrupan las secciones cerradas sin dejar huecos.
            int posicionY = LogisticaMenuContraido
                ? 12 : LogisticaPbLogo.Bottom + 6;

            for (int i = 0; i < LogisticaBotonesMenu.Length; i++)
            {
                Button boton = LogisticaBotonesMenu[i];
                Panel subMenu = LogisticaPanelesMenu[i];

                // Nunca se sustituyen los títulos por iniciales.
                boton.Text = LogisticaTitulosMenu[i];
                boton.BackColor = LogisticaFondosMenu[i];

                if (!LogisticaMenuContraido)
                {
                    boton.SetBounds(LogisticaIzquierdasMenu[i], posicionY,
                        LogisticaAnchosMenu[i], LogisticaAlturasMenu[i]);
                    boton.TextAlign = LogisticaAlineacionesMenu[i];
                    boton.Padding = LogisticaRellenosMenu[i];
                }
                else
                {
                    // El ancho de 64 px solo permite ver una parte del
                    // título; el texto completo permanece en el botón.
                    // Al abrir el menú, las opciones salen como panel flotante.
                    boton.SetBounds(5, posicionY, 54, LogisticaAlturasMenu[i]);
                    boton.TextAlign = ContentAlignment.MiddleLeft;
                    boton.Padding = new Padding(5, 0, 0, 0);
                }

                posicionY += boton.Height + 6;

                if (subMenu.Parent == LogisticaPnlNavegador)
                    subMenu.Top = posicionY;

                // Un panel flotante no ocupa altura dentro del navegador.
                if (!LogisticaMenuContraido && subMenu.Visible)
                    posicionY += subMenu.Height + 6;
            }

            // Mantener el botón original de cierre de sesión.
            LogisticaBtnCerrarSesion.Left = LogisticaMenuContraido ? 5 : 7;
            LogisticaBtnCerrarSesion.Width = LogisticaMenuContraido ? 54 : 264;
            LogisticaBtnCerrarSesion.Text = "Cerrar Sesión";
            LogisticaBtnCerrarSesion.Top = posicionY;
            posicionY += LogisticaBtnCerrarSesion.Height + 12;

            LogisticaPnlNavegador.AutoScrollMinSize =
                new Size(0, posicionY);
            LogisticaPnlNavegador.ResumeLayout(true);

            LogisticaPnlNavegador.AutoScrollPosition =
                new Point(0, scrollAnterior);

            LogisticaMetPosicionarSubMenuFlotante();
            LogisticaMetActualizarFlechas();
        }


        // ========================================================
        // FLECHAS E INDICADOR LATERAL (estilo Contabilidad)
        // Sin reemplazar los botones originales.
        // ========================================================

        private void LogisticaMetAgregarFlecha(Button boton, int indice)
        {
            // Label encima del botón: evita que Windows tape la flecha
            // cuando pinta el estilo nativo de los controles.
            Label flecha = new Label();
            flecha.Name = "LogisticaLblFlecha" + indice;
            flecha.AutoSize = false;
            flecha.Size = new Size(27, boton.Height);
            flecha.Location = new Point(boton.Width - 29, 0);
            flecha.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            flecha.BackColor = boton.BackColor;
            flecha.ForeColor = LogisticaColorTurquesa;
            flecha.Font = new Font("Segoe UI Symbol", 12f, FontStyle.Bold);
            flecha.TextAlign = ContentAlignment.MiddleCenter;
            flecha.Text = "▶";
            flecha.Cursor = Cursors.Hand;
            flecha.Click += (s, e) => boton.PerformClick();

            Panel indicador = new Panel();
            indicador.Name = "LogisticaPnlIndicador" + indice;
            indicador.Dock = DockStyle.Left;
            indicador.Width = 4;
            indicador.BackColor = LogisticaColorTurquesa;
            indicador.Visible = false;
            indicador.Cursor = Cursors.Hand;
            indicador.Click += (s, e) => boton.PerformClick();

            boton.Controls.Add(flecha);
            boton.Controls.Add(indicador);
            flecha.BringToFront();
            indicador.BringToFront();

            LogisticaFlechasMenu[indice] = flecha;
            LogisticaBarrasActivas[indice] = indicador;
        }

        private void LogisticaMetActualizarFlechas()
        {
            if (LogisticaBotonesMenu == null)
                return;

            for (int i = 0; i < LogisticaBotonesMenu.Length; i++)
            {
                Button boton = LogisticaBotonesMenu[i];
                Label flecha = LogisticaFlechasMenu[i];
                Panel indicador = LogisticaBarrasActivas[i];
                bool abierto = LogisticaPanelesMenu[i].Visible;

                boton.BackColor = abierto
                    ? LogisticaColorSeleccion : LogisticaFondosMenu[i];

                // Como en Contabilidad: derecha = cerrado, abajo = abierto.
                flecha.Text = abierto ? "▼" : "▶";
                flecha.BackColor = boton.BackColor;
                flecha.ForeColor = LogisticaColorTurquesa;
                flecha.Height = boton.Height;
                flecha.Width = LogisticaMenuContraido ? 17 : 27;
                // Reservar espacio para la barra vertical del navegador.
                flecha.Left = boton.Width - flecha.Width -
                    (LogisticaMenuContraido ? 2 : 18);
                flecha.Top = 0;
                flecha.Visible = true;
                flecha.BringToFront();

                indicador.Visible = abierto;
            }
        }

        // ========================================================
        // EVENTOS DE LOS ENCABEZADOS
        // ========================================================

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
        // HAMBURGUESA: preservar el submenú abierto y los botones.
        // ========================================================

        // Un submenú visible mantiene todo su ancho aun con la barra
        // de navegación contraída (64 px).
        private void LogisticaMetPosicionarSubMenuFlotante()
        {
            if (!LogisticaMenuContraido || LogisticaPanelesMenu == null)
                return;

            for (int i = 0; i < LogisticaPanelesMenu.Length; i++)
            {
                Panel panel = LogisticaPanelesMenu[i];
                if (!panel.Visible || panel.Parent != LogisticaPnlMDI)
                    continue;

                Button boton = LogisticaBotonesMenu[i];
                Point origen = LogisticaPnlMDI.PointToClient(
                    boton.PointToScreen(Point.Empty));

                int izquierda = LogisticaPnlNavegador.Right + 6;
                int superior = Math.Max(4, Math.Min(origen.Y,
                    Math.Max(4, LogisticaPnlMDI.ClientSize.Height - panel.Height - 4)));

                panel.Location = new Point(izquierda, superior);
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.BringToFront();
            }
        }

        private void LogisticaMetCambiarNavegador(bool contraer)
        {
            if (contraer == LogisticaMenuContraido)
                return;

            // Identificar el submenú abierto. NO lo ocultamos.
            Panel abierto = null;
            foreach (Panel panel in LogisticaPanelesMenu)
            {
                if (panel.Visible)
                {
                    abierto = panel;
                    break;
                }
            }

            LogisticaMenuContraido = contraer;

            if (abierto != null)
            {
                // Al reducir, mostrar las opciones completas al costado;
                // al expandir, regresarlas a su contenedor original.
                abierto.Parent = contraer
                    ? LogisticaPnlMDI : LogisticaPnlNavegador;
                abierto.Visible = true;
            }

            LogisticaPnlNavegador.Width = contraer
                ? LogisticaAnchoContraido : LogisticaAnchoExpandido;
            LogisticaPbLogo.Visible = !contraer;

            // Se respeta el posicionamiento que tenía tu botón
            // hamburguesa desde el comienzo.
            int desplazamiento = contraer ? 73 : 279;
            LogisticaPnlBanner.Location = new Point(desplazamiento, 34);
            LogisticaBtnHamburguesa.Location =
                new Point(desplazamiento, 8);

            LogisticaLblTextoUsuario.Location =
                new Point(contraer ? 128 : 334, 8);
            LogisticaLblUsuario.Location =
                new Point(contraer ? 228 : 434, 9);
            LogisticaLblTextoRol.Location =
                new Point(contraer ? 500 : 706, 9);
            LogisticaLblRolUsuario.Location =
                new Point(contraer ? 546 : 752, 9);

            MetOrganizarMenu();
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
            LogisticaMetConectarClick(LogisticaTipoMovimiento, LogisticaTipoMovimiento_Click);
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

        }

        private void LogisticaBtnAlertaCadenaFrio_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmAlertaCadenaFrio());

        }

        private void LogisticaBtnProducto_Click_1(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoProducto());
        }
    }
}
