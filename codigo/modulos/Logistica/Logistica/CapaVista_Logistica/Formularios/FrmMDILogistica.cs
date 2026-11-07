
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
using CapaVista_Enlace_Logistica.Formas;

namespace CapaVista_Logistica.Formularios
{
    public partial class FrmMDILogistica : Form
    {
        // ============================================
        // CONSTRUCTOR PRINCIPAL
        // ============================================

        public FrmMDILogistica()
        {
            InitializeComponent();

            // Ajustar el contenedor a la ventana.
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
            // Ocultar todos los submenús al iniciar.
            MetEsconderSubMenu();

            // Habilitar el desplazamiento vertical.
            LogisticaPnlNavegador.AutoScroll = true;

            // Eliminar la altura fija de desplazamiento.
            LogisticaPnlNavegador.AutoScrollMinSize =
                Size.Empty;

            LogisticaPnlNavegador.AutoScrollMargin =
                new Size(0, 10);

            // Ocultar el panel final porque está vacío.
            LogistincaPnlMenuFin.Visible = false;

            // Configurar eventos de los menús principales.
            // Mantenimientos ya tiene su evento en Designer.

            LogisticaBtnMenuGestionInventario.Click +=
                (s, e) => metShowSubMenu(
                    LogisticaPnlMenuGestionInventario);

            LogisticaBtnMenuCierreInventario.Click +=
                (s, e) => metShowSubMenu(
                    LogisticaPnlMenuCierreInventario);

            LogisticaBtnMenuGestionTraslados.Click +=
                (s, e) => metShowSubMenu(
                    LogisticaBtnMenuTraslados);

            LogisticaBtnMenuGestionViajes.Click +=
                (s, e) => metShowSubMenu(
                    LogisticaPnlMenuGesionViajes);

            LogisticaBtnMenuTransporteExterno.Click +=
                (s, e) => metShowSubMenu(
                    LogisticaPnlMenuTransporteExterno);

            LogisticaBtnMenuAdministracion.Click +=
                (s, e) => metShowSubMenu(
                    LogisticaPnlMenuAdministracionCostos);

            // Organizar los botones inicialmente.
            MetOrganizarMenu();
        }

        // ============================================
        // OCULTAR TODOS LOS SUBMENÚS
        // ============================================

        private void MetEsconderSubMenu()
        {
            LogisticaPnlMenuMantenimientos.Visible = false;

            LogisticaPnlMenuGestionInventario.Visible = false;

            LogisticaPnlMenuCierreInventario.Visible = false;

            LogisticaBtnMenuTraslados.Visible = false;

            LogisticaPnlMenuGesionViajes.Visible = false;

            LogisticaPnlMenuTransporteExterno.Visible = false;

            LogisticaPnlMenuAdministracionCostos.Visible = false;
        }

        // ============================================
        // MOSTRAR U OCULTAR SUBMENÚ
        // ============================================

        private void metShowSubMenu(Panel subMenu)
        {
            // Verificar si el submenú estaba cerrado.
            bool abrirMenu = !subMenu.Visible;

            // Cerrar los demás submenús.
            MetEsconderSubMenu();

            // Abrir únicamente el menú seleccionado.
            subMenu.Visible = abrirMenu;

            // Reacomodar todos los botones.
            MetOrganizarMenu();
        }

        // ============================================
        // ORGANIZAR LOS MENÚS VERTICALMENTE
        // ============================================

        private void MetOrganizarMenu()
        {
            // Guardar el desplazamiento actual.
            int scrollActual = Math.Max(
                0,
                -LogisticaPnlNavegador.AutoScrollPosition.Y);

            LogisticaPnlNavegador.SuspendLayout();

            // Volver temporalmente al inicio.
            LogisticaPnlNavegador.AutoScrollPosition =
                Point.Empty;

            // Iniciar debajo del logotipo.
            int posicionY = LogisticaPbLogo.Bottom + 6;

            // 1. Mantenimientos.
            MetPosicionarSeccion(
                LogisticaBtnMenuMantenimientos,
                LogisticaPnlMenuMantenimientos,
                ref posicionY);

            // 2. Gestión de Inventario.
            MetPosicionarSeccion(
                LogisticaBtnMenuGestionInventario,
                LogisticaPnlMenuGestionInventario,
                ref posicionY);

            // 3. Cierre y Auditoría.
            MetPosicionarSeccion(
                LogisticaBtnMenuCierreInventario,
                LogisticaPnlMenuCierreInventario,
                ref posicionY);

            // 4. Gestión de Traslados.
            MetPosicionarSeccion(
                LogisticaBtnMenuGestionTraslados,
                LogisticaBtnMenuTraslados,
                ref posicionY);

            // 5. Gestión de Viajes.
            MetPosicionarSeccion(
                LogisticaBtnMenuGestionViajes,
                LogisticaPnlMenuGesionViajes,
                ref posicionY);

            // 6. Transporte Externo.
            MetPosicionarSeccion(
                LogisticaBtnMenuTransporteExterno,
                LogisticaPnlMenuTransporteExterno,
                ref posicionY);

            // 7. Administración.
            MetPosicionarSeccion(
                LogisticaBtnMenuAdministracion,
                LogisticaPnlMenuAdministracionCostos,
                ref posicionY);

            // 8. Cerrar sesión.
            LogisticaBtnCerrarSesion.Top = posicionY;

            posicionY +=
                LogisticaBtnCerrarSesion.Height + 12;

            // Actualizar el área desplazable.
            LogisticaPnlNavegador.AutoScrollMinSize =
                new Size(0, posicionY);

            LogisticaPnlNavegador.ResumeLayout(true);

            // Restaurar el desplazamiento si es posible.
            LogisticaPnlNavegador.AutoScrollPosition =
                new Point(0, scrollActual);
        }

        // ============================================
        // POSICIONAR UNA SECCIÓN DEL MENÚ
        // ============================================

        private void MetPosicionarSeccion(
            Button boton,
            Panel subMenu,
            ref int posicionY)
        {
            // Posicionar el botón principal.
            boton.Top = posicionY;

            // Avanzar según la altura del botón.
            posicionY += boton.Height + 6;

            // Posicionar el submenú.
            subMenu.Top = posicionY;

            // Solamente reservar espacio si está visible.
            if (subMenu.Visible)
            {
                posicionY += subMenu.Height + 6;
            }
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
            if (LogisticaPnlNavegador.Width > 64)
            {
                // Contraer el menú lateral.
                LogisticaPnlNavegador.Width = 64;

                LogisticaPnlBanner.Location =
                    new Point(73, 34);

                LogisticaBtnHamburguesa.Location =
                    new Point(73, 8);

                LogisticaLblTextoUsuario.Location =
                    new Point(128, 8);

                LogisticaLblUsuario.Location =
                    new Point(228, 9);

                LogisticaLblTextoRol.Location =
                    new Point(500, 9);

                LogisticaLblRolUsuario.Location =
                    new Point(546, 9);
            }
            else
            {
                // Expandir el menú lateral.
                LogisticaPnlNavegador.Width = 273;

                LogisticaPnlBanner.Location =
                    new Point(279, 34);

                LogisticaBtnHamburguesa.Location =
                    new Point(279, 8);

                LogisticaLblTextoUsuario.Location =
                    new Point(334, 8);

                LogisticaLblUsuario.Location =
                    new Point(434, 9);

                LogisticaLblTextoRol.Location =
                    new Point(706, 9);

                LogisticaLblRolUsuario.Location =
                    new Point(752, 9);
            }
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
            LogisticaMetAbrirFormulario( new FrmMantenimientoDetalleMovimiento());
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
