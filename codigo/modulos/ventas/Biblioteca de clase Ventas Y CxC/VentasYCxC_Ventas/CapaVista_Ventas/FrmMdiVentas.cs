/*
 * INICIO DEL CÓDIGO — Jose Torres
 * Carné: 0901-23-1091 | Fecha: 15/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Presentar el dashboard, sus indicadores y la navegación del módulo de Ventas.
 */
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Drawing.Imaging;
using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;

namespace CapaVista_Ventas
{
    public partial class FrmMdiVentas : Form
    {
        private const int VentasIdModuloPermisos = 7000;
        private readonly int _IdUsuario;
        private readonly ClsMenuProcesosVentas _MenuProcesos;
        private ClsModeloAsigAppPerf _ModeloPermisosVentas;
        private bool _PermisosVentasCargados;
        private Exception _ErrorCargaPermisosVentas;
        private readonly System.Collections.Generic.Dictionary<int, bool> _AccesosAplicacionesVentas =
            new System.Collections.Generic.Dictionary<int, bool>();
        private readonly System.Collections.Generic.Dictionary<Button, int> _AplicacionesMenuDirectas =
            new System.Collections.Generic.Dictionary<Button, int>();
        private readonly System.Collections.Generic.Dictionary<Image, Image> _ImagenesMenuGrises =
            new System.Collections.Generic.Dictionary<Image, Image>();
        private static Assembly _ModuloCierreMovimientos;
        private static string _DirectorioCierreMovimientos;
        private static bool _ResolverCierreRegistrado;
        private static readonly object _BloqueoCargaCierre = new object();

        // El MDI carga Cierre de movimientos al abrir la opción, desde la salida actual de su MVC.
        private static Assembly VentasMetCargarEnsambladoCierreMovimientos()
        {
            if (_ModuloCierreMovimientos != null)
                return _ModuloCierreMovimientos;

            lock (_BloqueoCargaCierre)
            {
                if (_ModuloCierreMovimientos != null)
                    return _ModuloCierreMovimientos;

#if DEBUG
                const string Configuracion = "Debug";
#else
                const string Configuracion = "Release";
#endif
                string RutaModulo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CapaVista_CierreMovimientos_Ventas.dll");
                if (!File.Exists(RutaModulo))
                    RutaModulo = Path.GetFullPath(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\..\Biblioteca de clase Ventas Y CxC\CierreMovimientos_Ventas\CapaVista_CierreMovimientos_Ventas\bin\" +
                    Configuracion + @"\CapaVista_CierreMovimientos_Ventas.dll"));

                if (!File.Exists(RutaModulo))
                    throw new FileNotFoundException(
                        "No se encontró la DLL de Cierre de movimientos. Compile el módulo de Cierre de Ventas.",
                        RutaModulo);

                _DirectorioCierreMovimientos = Path.GetDirectoryName(RutaModulo);
                if (!_ResolverCierreRegistrado)
                {
                    AppDomain.CurrentDomain.AssemblyResolve += VentasMetResolverEnsambladoCierre;
                    _ResolverCierreRegistrado = true;
                }

                // Precarga primero las dependencias directas de la vista desde la misma salida MVC.
                // Esto evita que el CLR elija una copia antigua junto al ejecutable.
                string RutaModelo = Path.Combine(_DirectorioCierreMovimientos, "CapaModelo_CierreMovimientos_Ventas.dll");
                string RutaControlador = Path.Combine(_DirectorioCierreMovimientos, "CapaControlador_CierreMovimientos_Ventas.dll");
                if (!File.Exists(RutaModelo))
                    throw new FileNotFoundException("No se encontró el modelo MVC de Cierre de movimientos.", RutaModelo);
                if (!File.Exists(RutaControlador))
                    throw new FileNotFoundException("No se encontró el controlador MVC de Cierre de movimientos.", RutaControlador);

                Assembly.LoadFrom(RutaModelo);
                Assembly.LoadFrom(RutaControlador);
                Assembly ModuloCierre = Assembly.LoadFrom(RutaModulo);
                if (ModuloCierre.GetType("CapaVista_CierreMovimientos_Ventas.FrmCierreMovimientosVentas", false) == null)
                    throw new TypeLoadException(
                        "La DLL de Cierre no contiene FrmCierreMovimientosVentas: " + RutaModulo);

                _ModuloCierreMovimientos = ModuloCierre;

                return _ModuloCierreMovimientos;
            }
        }

        private static Assembly VentasMetResolverEnsambladoCierre(object Origen, ResolveEventArgs Argumentos)
        {
            Assembly ModuloCierre = _ModuloCierreMovimientos;
            string NombreEnsamblado = new AssemblyName(Argumentos.Name).Name;
            if (ModuloCierre != null && String.Equals(NombreEnsamblado,
                "CapaVista_CierreMovimientos_Ventas", StringComparison.OrdinalIgnoreCase))
                return ModuloCierre;

            if (!String.IsNullOrEmpty(_DirectorioCierreMovimientos))
            {
                string RutaDependencia = Path.Combine(_DirectorioCierreMovimientos, NombreEnsamblado + ".dll");
                if (File.Exists(RutaDependencia))
                    return Assembly.LoadFrom(RutaDependencia);
            }

            return null;
        }

        // WM_SETREDRAW agrupa los cambios de tamaño para evitar imágenes superpuestas al contraer el menú.
        private const int _WmSetRedraw = 0x000B;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr Ventana, int Mensaje, IntPtr Valor, IntPtr Datos);

        public FrmMdiVentas()
        {
            InitializeComponent();
            VentasMetAjustarIconoMantenimientos();
            VentasMetAjustarBotonMantenimientos();
            VentasMetConfigurarAplicacionesDirectas();
            _MenuProcesos = new ClsMenuProcesosVentas(VentasTlpMenu, VentasBtnMovimientosClientes,
                VentasBtnRecepcionPagos, VentasBtnPedidos, VentasMetAbrirFormulario,
                () => _IdUsuario, VentasMetTieneAccesoAplicacion, VentasMetMostrarAccesoDenegado,
                VentasMetAplicarEstadoAccesoMenu);
            Load += VentasMetAplicarPermisosMenu;
            // El doble búfer reduce el parpadeo al redimensionar la ventana.
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            VentasPnlPrincipal.AutoScroll = true;
            VentasPnlPrincipal.Resize += (Origen, Evento) => VentasMetAjustarContenido();
            Shown += (Origen, Evento) => VentasMetAjustarContenido();
        }

        private void VentasMetAjustarIconoMantenimientos()
        {
            Image IconoOriginal = CapaVista_Ventas.Properties.Resources.ResourceManager
                .GetObject("IconoMantenimientosVentas") as Image;
            if (IconoOriginal == null)
                throw new InvalidOperationException("No se encontró el icono de Mantenimientos de Ventas.");

            // El recurso de herramientas tiene margen exterior; se recorta antes de igualar su marco.
            var IconoMenu = new Bitmap(56, 42);
            using (Graphics Dibujo = Graphics.FromImage(IconoMenu))
            {
                Dibujo.Clear(Color.Transparent);
                Dibujo.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                int Margen = (int)(Math.Min(IconoOriginal.Width, IconoOriginal.Height) * 0.05);
                var AreaOriginal = new Rectangle(Margen, Margen,
                    IconoOriginal.Width - 2 * Margen, IconoOriginal.Height - 2 * Margen);
                Dibujo.DrawImage(IconoOriginal, new Rectangle(0, 0, 42, 42),
                    AreaOriginal, GraphicsUnit.Pixel);
            }
            VentasBtnMantenimientos.Image = IconoMenu;
        }

        private void VentasMetAjustarBotonMantenimientos()
        {
            VentasBtnMantenimientos.Text = VentasBtnMantenimientos.Text.TrimStart();
            int Fila = VentasTlpMenu.GetRow(VentasBtnMantenimientos);
            int FilaReferencia = VentasTlpMenu.GetRow(VentasBtnComisionesVendedores);
            VentasTlpMenu.RowStyles[Fila].SizeType = VentasTlpMenu.RowStyles[FilaReferencia].SizeType;
            VentasTlpMenu.RowStyles[Fila].Height = VentasTlpMenu.RowStyles[FilaReferencia].Height;
        }

        private void VentasMetAjustarContenido()
        {
            // Conserva el ancho original en pantallas grandes y permite desplazarlo en pantallas pequeñas.
            int AnchoDisponible = VentasPnlPrincipal.ClientSize.Width - VentasTlpContenido.Left - 14;
            VentasTlpContenido.Width = Math.Max(750, AnchoDisponible);
        }

        private void VentasMetAbrirFormulario(Form Formulario)
        {
            Rectangle Area = Screen.FromControl(this).WorkingArea;
            int Ancho = Math.Max(1, Area.Width - 32);
            int Alto = Math.Max(1, Area.Height - 32);
            Formulario.AutoScrollMinSize = Formulario.ClientSize;
            Formulario.AutoScroll = true;
            Formulario.MinimumSize = Size.Empty;
            Formulario.Size = new Size(Math.Min(Formulario.Width, Ancho), Math.Min(Formulario.Height, Alto));
            Formulario.StartPosition = FormStartPosition.CenterParent;
            Formulario.ShowDialog(this);
        }

        public FrmMdiVentas(string NombreUsuario, string RolUsuario) : this(NombreUsuario, RolUsuario, 0) { }

        public FrmMdiVentas(string NombreUsuario, string RolUsuario, int IdUsuario) : this()
        {
            _IdUsuario = IdUsuario;
            VentasMetActualizarSesion(NombreUsuario, RolUsuario);
        }

        // Presenta el usuario y el rol cuando otro módulo proporciona esa información.
        public void VentasMetActualizarSesion(string NombreUsuario, string RolUsuario)
        {
            VentasLblUsuario.Text = "Usuario: " + (string.IsNullOrWhiteSpace(NombreUsuario) ? "Sin sesión" : NombreUsuario);
            VentasLblRol.Text = "Rol: " + (string.IsNullOrWhiteSpace(RolUsuario) ? "Sin asignar" : RolUsuario);
        }

        // Carga una sola vez los permisos del usuario autenticado, siguiendo el modelo que usa Seguridad.
        private bool VentasMetCargarPermisos()
        {
            if (_PermisosVentasCargados)
                return _ModeloPermisosVentas != null;

            _PermisosVentasCargados = true;
            if (_IdUsuario <= 0 || !ClsSesionSeguridad.HaySesionActiva ||
                ClsSesionSeguridad.IdUsuario != _IdUsuario)
                return false;

            try
            {
                _ModeloPermisosVentas = new ClsModeloAsigAppPerf();
                _ModeloPermisosVentas.SeguridadMetObtenerTodos();
                return true;
            }
            catch (Exception Error)
            {
                _ModeloPermisosVentas = null;
                _ErrorCargaPermisosVentas = Error;
                return false;
            }
        }

        // Un permiso solo habilita la opción cuando coincide el módulo Ventas y su aplicación.
        private bool VentasMetTieneAccesoAplicacion(int IdAplicacion)
        {
            if (_AccesosAplicacionesVentas.TryGetValue(IdAplicacion, out bool TieneAcceso))
                return TieneAcceso;

            TieneAcceso = false;
            if (VentasMetCargarPermisos())
            {
                var Permisos = _ModeloPermisosVentas.SeguridadMetObtenerPermisosSesion(
                    VentasIdModuloPermisos, IdAplicacion);
                TieneAcceso = Permisos != null && Permisos.TieneAcceso;
            }

            _AccesosAplicacionesVentas[IdAplicacion] = TieneAcceso;
            return TieneAcceso;
        }

        private void VentasMetAplicarPermisosMenu(object Origen, EventArgs Evento)
        {
            foreach (var Aplicacion in _AplicacionesMenuDirectas)
                VentasMetAplicarEstadoAccesoMenu(
                    Aplicacion.Key, VentasMetTieneAccesoAplicacion(Aplicacion.Value));

            _MenuProcesos.VentasMetActualizarPermisos();

            if (_IdUsuario > 0 && ClsSesionSeguridad.IdUsuario == _IdUsuario &&
                _ErrorCargaPermisosVentas != null)
            {
                MessageBox.Show(this,
                    "No se pudieron cargar los permisos de Ventas. Las opciones permanecerán bloqueadas.\r\n\r\n" +
                    _ErrorCargaPermisosVentas.Message,
                    "Permisos de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool VentasMetExigirAccesoAplicacion(int IdAplicacion, Button Opcion)
        {
            if (VentasMetTieneAccesoAplicacion(IdAplicacion))
                return true;

            VentasMetMostrarAccesoDenegado(IdAplicacion, Opcion == null ? "esta aplicación" : Opcion.Text.Trim());
            return false;
        }

        private void VentasMetMostrarAccesoDenegado(int IdAplicacion, string NombreAplicacion)
        {
            if (_ErrorCargaPermisosVentas != null)
            {
                MessageBox.Show(this,
                    "No fue posible verificar el permiso para " + NombreAplicacion + ". Las opciones están bloqueadas.\r\n\r\n" +
                    _ErrorCargaPermisosVentas.Message,
                    "Permisos de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(this,
                "No tienes acceso a la aplicación " + NombreAplicacion + " del módulo de Ventas.",
                "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void VentasMetConfigurarAplicacionesDirectas()
        {
            _AplicacionesMenuDirectas.Add(VentasBtnListadoPrecios, 7002);
            // El informe de antigüedad corresponde a la consulta de movimientos de CxC.
            _AplicacionesMenuDirectas.Add(VentasBtnReportes, 7003);
            _AplicacionesMenuDirectas.Add(VentasBtnCotizaciones, 7011);
            _AplicacionesMenuDirectas.Add(VentasBtnFacturas, 7007);
            _AplicacionesMenuDirectas.Add(VentasBtnComisionesVendedores, 7013);
            _AplicacionesMenuDirectas.Add(VentasBtnMantenimientos, 7009);
            _AplicacionesMenuDirectas.Add(VentasBtnCierreMovimientos, 7010);
        }

        // Mantiene los accesos visibles y muestra en gris los que no tienen autorización, como Seguridad.
        private void VentasMetAplicarEstadoAccesoMenu(Button Boton, bool TieneAcceso)
        {
            Boton.Enabled = true;
            if (Boton.BackgroundImage != null &&
                !Boton.Name.StartsWith("VentasBtnProceso", StringComparison.Ordinal))
            {
                Image FondoNormal = CapaVista_Ventas.Properties.Resources.BotonMenuNormalVentas;
                Boton.BackgroundImage = TieneAcceso ? FondoNormal : VentasMetObtenerImagenGris(FondoNormal);
                Boton.ForeColor = TieneAcceso ? Color.FromArgb(248, 245, 249) : Color.Gainsboro;
            }
            else
            {
                Boton.BackColor = TieneAcceso ? Color.FromArgb(59, 29, 74) : Color.Gray;
                Boton.ForeColor = TieneAcceso ? Color.White : Color.Gainsboro;
                Boton.FlatAppearance.BorderColor = TieneAcceso ? Color.White : Color.DarkGray;
            }
        }

        private Image VentasMetObtenerImagenGris(Image ImagenOriginal)
        {
            if (_ImagenesMenuGrises.TryGetValue(ImagenOriginal, out Image ImagenGris))
                return ImagenGris;

            var Resultado = new Bitmap(ImagenOriginal.Width, ImagenOriginal.Height);
            using (Graphics Graficos = Graphics.FromImage(Resultado))
            using (var Atributos = new ImageAttributes())
            {
                var Matriz = new ColorMatrix(new float[][]
                {
                    new float[] { .3f, .3f, .3f, 0, 0 },
                    new float[] { .59f, .59f, .59f, 0, 0 },
                    new float[] { .11f, .11f, .11f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 0, 1 }
                });
                Atributos.SetColorMatrix(Matriz);
                Graficos.DrawImage(ImagenOriginal,
                    new Rectangle(0, 0, Resultado.Width, Resultado.Height),
                    0, 0, ImagenOriginal.Width, ImagenOriginal.Height,
                    GraphicsUnit.Pixel, Atributos);
            }

            _ImagenesMenuGrises.Add(ImagenOriginal, Resultado);
            return Resultado;
        }

        private bool VentasMetTienePermisoVisual(Button Boton)
        {
            if (_AplicacionesMenuDirectas.TryGetValue(Boton, out int IdAplicacion))
                return VentasMetTieneAccesoAplicacion(IdAplicacion);

            return _MenuProcesos == null || !_MenuProcesos.VentasMetEsGrupo(Boton) ||
                _MenuProcesos.VentasMetTieneAccesoGrupo(Boton);
        }

        // Actualiza directamente los seis labels del diseñador con datos proporcionados por el controlador.
        public void VentasMetActualizarIndicadores(decimal VentasDia, decimal SaldoCxC, int ClientesActivos,
            int FacturasPendientes, decimal PagosDia, int PedidosPendientes)
        {
            var Cultura = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            VentasLblValorKpi1.Text = "Q " + VentasDia.ToString("N2", Cultura);
            VentasLblValorKpi2.Text = "Q " + SaldoCxC.ToString("N2", Cultura);
            VentasLblValorKpi3.Text = ClientesActivos.ToString("N0", Cultura);
            VentasLblValorKpi4.Text = FacturasPendientes.ToString("N0", Cultura);
            VentasLblValorKpi5.Text = "Q " + PagosDia.ToString("N2", Cultura);
            VentasLblValorKpi6.Text = PedidosPendientes.ToString("N0", Cultura);
        }

        private void VentasMetBtnBurgerClick(object Origen, EventArgs Evento)
        {
            // Como en Seguridad: el panel pasa de 270 a 64 px y el contenido cambia de posición.
            // Los botones conservan su ancho, Image y Text; el panel recorta la parte que queda fuera.
            bool Contraer = VentasPnlNavegador.Width == 270;
            SuspendLayout();
            VentasPnlPrincipal.SuspendLayout();
            SendMessage(Handle, _WmSetRedraw, IntPtr.Zero, IntPtr.Zero);
            try
            {
                VentasPnlNavegador.Width = Contraer ? 64 : 270;
                VentasTlpContenido.Left = Contraer ? 200 : 300;
                VentasMetAjustarContenido();
                VentasBtnBurger.AccessibleName = Contraer ? "Expandir menú" : "Contraer menú";
            }
            finally
            {
                VentasPnlPrincipal.ResumeLayout(true);
                ResumeLayout(true);
                SendMessage(Handle, _WmSetRedraw, new IntPtr(1), IntPtr.Zero);
                // Repinta también los controles hijos una sola vez al terminar el cambio.
                Invalidate(true);
                Update();
            }
        }

        private void VentasMetAplicarEstadoBoton(Button Boton, bool Sobre)
        {
            bool Activo = Boton == VentasBtnInicio;
            if (!VentasMetTienePermisoVisual(Boton))
            {
                VentasMetAplicarEstadoAccesoMenu(Boton, false);
                return;
            }
            // Efecto de difuminado (hover): al pasar el cursor se sustituye el fondo por un tono
            // morado más suave. No altera el icono Image ni el texto Text del botón.
            Boton.BackgroundImage = Activo ? CapaVista_Ventas.Properties.Resources.BotonMenuActivoVentas :
                (Sobre ? CapaVista_Ventas.Properties.Resources.BotonMenuHoverVentas : CapaVista_Ventas.Properties.Resources.BotonMenuNormalVentas);
            Boton.ForeColor = Activo ? Color.FromArgb(59, 29, 74) : Color.FromArgb(248, 245, 249);
        }

        private void VentasMetBtnOpcionMouseEnter(object Origen, EventArgs Evento)
        { VentasMetAplicarEstadoBoton((Button)Origen, true); }

        private void VentasMetBtnOpcionMouseLeave(object Origen, EventArgs Evento)
        { VentasMetAplicarEstadoBoton((Button)Origen, false); }

        // Inicio conserva la pantalla principal del módulo.
        private void VentasMetBtnInicioClick(object Origen, EventArgs Evento)
        {
            // Aquí se puede agregar la acción específica del botón Inicio.
        }

        // Abre el listado de precios de escritorio sobre el MDI.
        private void VentasMetBtnListadoPreciosClick(object Origen, EventArgs Evento)
        {
            if (!VentasMetExigirAccesoAplicacion(7002, VentasBtnListadoPrecios)) return;
            using (var Formulario = new CapaVista_ListadoPrecios_Ventas.FrmListadoPrecios())
                VentasMetAbrirFormulario(Formulario);
        }

        // Abre la opción de movimientos de clientes.
        private void VentasMetBtnMovimientosClientesClick(object Origen, EventArgs Evento)
        {
            _MenuProcesos.VentasMetAlternar(VentasBtnMovimientosClientes);
        }
        private void VentasMetBtnRecepcionPagosClick(object Origen, EventArgs Evento)
        {
            _MenuProcesos.VentasMetAlternar(VentasBtnRecepcionPagos);
        }

        // Abre la opción de reportes de Ventas.
        private void VentasMetBtnReportesClick(object Origen, EventArgs Evento)
        {
            if (!VentasMetExigirAccesoAplicacion(7003, VentasBtnReportes)) return;
            CapaVista_Reportes_Ventas.ClsDocumentoAntiguedad.VentasMetAbrir(this);
        }

        // Abre el formulario independiente de cotizaciones.
        private void VentasMetBtnCotizacionesClick(object Origen, EventArgs Evento)
        {
            if (!VentasMetExigirAccesoAplicacion(7011, VentasBtnCotizaciones)) return;
            using (var Formulario = new CapaVista_Cotizaciones_Ventas.FrmCotizacionVentas() { VentasIdUsuario = _IdUsuario })
                VentasMetAbrirFormulario(Formulario);
        }

        // Abre el seguimiento y desde allí la creación o edición de pedidos.
        private void VentasMetBtnPedidosClick(object Origen, EventArgs Evento)
        {
            _MenuProcesos.VentasMetAlternar(VentasBtnPedidos);
        }

        // Abre el formulario independiente de facturas.
        private void VentasMetBtnFacturasClick(object Origen, EventArgs Evento)
        {
            if (!VentasMetExigirAccesoAplicacion(7007, VentasBtnFacturas)) return;
            using (var Formulario = new CapaVista_Facturas_Ventas.FrmFacturas() { VentasIdUsuario = _IdUsuario })
                VentasMetAbrirFormulario(Formulario);
        }

        // Abre la opción de comisiones de vendedores.
        private void VentasMetBtnComisionesVendedoresClick(object Origen, EventArgs Evento)
        {
            if (!VentasMetExigirAccesoAplicacion(7013, VentasBtnComisionesVendedores)) return;
            using (var Formulario = new CapaVista_ComisionVendedores_Ventas.FrmComisionesVendedoresVentas())
                VentasMetAbrirFormulario(Formulario);
        }

        // Abre la opción de mantenimientos del módulo.
        private void VentasMetBtnMantenimientosClick(object Origen, EventArgs Evento)
        {
            if (!VentasMetExigirAccesoAplicacion(7009, VentasBtnMantenimientos)) return;
            using (var Formulario = new CapaVista_Mantenimientos_Ventas.FrmMantenimientosVentas())
                VentasMetAbrirFormulario(Formulario);
        }

        // Abre la opción de cierre de movimientos.
        private void VentasMetBtnCierreMovimientosClick(object Origen, EventArgs Evento)
        {
            if (!VentasMetExigirAccesoAplicacion(7010, VentasBtnCierreMovimientos)) return;
            try
            {
                Assembly ModuloCierre = VentasMetCargarEnsambladoCierreMovimientos();
                Type TipoFormulario = ModuloCierre.GetType("CapaVista_CierreMovimientos_Ventas.FrmCierreMovimientosVentas", true);
                Form Formulario = (Form)Activator.CreateInstance(TipoFormulario);
                var PropiedadUsuario = TipoFormulario.GetProperty("VentasIdUsuario");
                if (PropiedadUsuario != null && PropiedadUsuario.CanWrite)
                    PropiedadUsuario.SetValue(Formulario, _IdUsuario, null);

                using (Formulario)
                    VentasMetAbrirFormulario(Formulario);
            }
            catch (Exception Error)
            {
                MessageBox.Show(this,
                    "No se pudo abrir Cierre de movimientos. Verifique que la vista MVC de Cierre esté compilada.\r\n\r\n" + Error.Message,
                    "Cierre de movimientos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Confirma la salida del módulo y cierra su ventana principal.
        private void VentasMetBtnSalirClick(object Origen, EventArgs Evento)
        {
            if (MessageBox.Show(this, "¿Desea salir del módulo de Ventas?", "Salir de Ventas",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void VentasMetBtnAyudaClick(object Origen, EventArgs Evento)
        {
            const string NombreRecurso = "CapaVista_Ventas.Ayudas.Ventas.chm";
            using (Stream Manual = typeof(FrmMdiVentas).Assembly.GetManifestResourceStream(NombreRecurso))
            {
                if (Manual == null)
                {
                    MessageBox.Show(this, "No se encontró el manual de Ventas.", "Ayuda de Ventas",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    byte[] Huella;
                    using (SHA256 Resumen = SHA256.Create())
                        Huella = Resumen.ComputeHash(Manual);

                    string CarpetaAyuda = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "EmbutidosDeCalidad", "Ventas", "Ayuda");
                    string NombreArchivo = "Ventas_" + BitConverter.ToString(Huella, 0, 8).Replace("-", "") + ".chm";
                    string ArchivoAyuda = Path.Combine(CarpetaAyuda, NombreArchivo);

                    if (!File.Exists(ArchivoAyuda))
                    {
                        Directory.CreateDirectory(CarpetaAyuda);
                        Manual.Position = 0;
                        using (var Destino = new FileStream(ArchivoAyuda, FileMode.CreateNew, FileAccess.Write))
                            Manual.CopyTo(Destino);
                    }

                    Help.ShowHelp(this, ArchivoAyuda, "html/Ventas_General.html");
                }
                catch (IOException Error)
                {
                    MessageBox.Show(this, "No fue posible abrir el manual de Ventas. " + Error.Message,
                        "Ayuda de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (UnauthorizedAccessException Error)
                {
                    MessageBox.Show(this, "No fue posible abrir el manual de Ventas. " + Error.Message,
                        "Ayuda de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

// FIN DEL CÓDIGO — Jose Torres | Carné: 0901-23-1091 | Fecha: 15/10/2026
