using CapaControlador_Seguridad.Objetos_de_valor;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta la ventana principal de Compras y administra la navegación, la apertura de subprocesos y las transiciones del menú.
 * ===================================================================
*/
namespace CapaVista_Compras
{
    public partial class FrmMDICompras : Form
    {
        private ToolTip _AyudaNavegador;
        private Form _FormularioSubproceso;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras _OpcionSubproceso;
        private bool _NavegadorContraido;
        private bool _CatalogosAbierto;
        private bool _RegistrosCompraAbierto;
        private Panel _PanelDesplazamientoNavegador;
        private int _DesplazamientoNavegador;
        private bool _PantallaCompleta;
        private FormBorderStyle _BordeAnterior;
        private FormWindowState _EstadoAnterior;
        private System.Drawing.Rectangle _LimitesAnteriores;
        private readonly Dictionary<TableLayoutPanel, ColumnStyle> _ColumnasNavegador =
            new Dictionary<TableLayoutPanel, ColumnStyle>();
        // Inicio de código de "José Pablo Cano Cóbar" - carné: "0901-23-1727" - Fecha: "29/10/26"
        // Seguridad asigna aquí cómo crear su FrmBitacora al abrir este MDI después del login.
        public Func<Form> CrearBitacora { get; set; }
        // Fin del código de "José Pablo Cano Cóbar" - Carné: "0901-23-1727" - Fecha: "29/10/26"
        public FrmMDICompras()
        {
            InitializeComponent();
            if (_Componentes == null) _Componentes = new Container();
            _AyudaNavegador = new ToolTip(_Componentes) { ShowAlways = true };
            ComprasMetConfigurarDesplazamientoNavegador();
            ComprasTlpRegistrosCompra.Paint += ComprasMetPintarFlechaRegistrosCompra;
            ComprasTlpCatalogos.Paint += ComprasMetPintarFlechaCatalogos;
            ComprasLblCatalogos.Margin = new Padding(3, 4, 26, 4);
            ComprasMetAcomodarCatalogos();
            ComprasMetAcomodarRegistrosCompra();
            ComprasBtnAyuda.Enabled = true;
            ComprasTlpBitacora.Enabled = true;
            ComprasTlpHerramientas.Enabled = true;
            ComprasLblBienvenido.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.TituloBienvenida;
            ComprasLblKpi1.Texto = "Antigüedad promedio de saldos";
            ComprasLblKpi2.Texto = "Órdenes autorizadas (%)";
            DoubleBuffered = true;
            foreach (Control ControlActual in ComprasTlpNavegador.Controls)
            {
                CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras Panel = ControlActual as CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras;
                if (Panel != null) ComprasMetConfigurarEstadosTab(Panel, Panel);
            }
            ComprasTlpPresentacion.SizeChanged += (Origen, Argumentos) => ComprasMetAjustarPresentacion();
            ComprasMetAjustarPresentacion();
            ComprasMetActualizarInfoUsuario();
            TableLayoutPanel[] PanelesIndicadores = { ComprasTlpKpi1, ComprasTlpKpi2, ComprasTlpKpi3, ComprasTlpKpi4, ComprasTlpKpi5, ComprasTlpKpi6 };
            foreach (TableLayoutPanel Panel in PanelesIndicadores)
            {
                for (int Fila = 0; Fila < ComprasTlpKpi1.RowStyles.Count; Fila++)
                {
                    Panel.RowStyles[Fila].SizeType = ComprasTlpKpi1.RowStyles[Fila].SizeType;
                    Panel.RowStyles[Fila].Height = ComprasTlpKpi1.RowStyles[Fila].Height;
                }
                Panel.RowStyles[1].SizeType = SizeType.Absolute;
                Panel.RowStyles[1].Height = 36F;
                Panel.RowStyles[2].SizeType = SizeType.Absolute;
                Panel.RowStyles[2].Height = 38F;
                foreach (Control Elemento in Panel.Controls)
                {
                    if (Panel.GetRow(Elemento) == 1)
                    {
                        Elemento.Margin = new Padding(2);
                        var Valor = Elemento as CapaVista_Componentes.Componentes.ComprasUsrLabel;
                        if (Valor != null) Valor.TamanoTipografia = 15F;
                    }
                    if (Panel.GetRow(Elemento) == 2) Elemento.Margin = new Padding(2);
                }
            }
        }
        public void ComprasMetActualizarInfoUsuario(string Usuario, string Rol)
        {
            ComprasLblUsuario.Texto = "Usuario: " + Usuario;
            ComprasLblRol.Texto = "Rol: " + Rol;
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
        protected override bool ProcessCmdKey(ref Message Mensaje, Keys Teclas)
        {
            if (Teclas != (Keys.Alt | Keys.Enter)) return base.ProcessCmdKey(ref Mensaje, Teclas);
            if ((Mensaje.LParam.ToInt64() & (1L << 30)) != 0) return true;
            ComprasMetAlternarPantallaCompleta();
            return true;
        }
        private void ComprasMetAlternarPantallaCompleta()
        {
            SuspendLayout();
            try
            {
                if (!_PantallaCompleta)
                {
                    _BordeAnterior = FormBorderStyle;
                    _EstadoAnterior = WindowState;
                    _LimitesAnteriores = WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;
                    System.Drawing.Rectangle Pantalla = Screen.FromControl(this).WorkingArea;
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.None;
                    Bounds = Pantalla;
                    _PantallaCompleta = true;
                }
                else
                {
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = _BordeAnterior;
                    Bounds = _LimitesAnteriores;
                    WindowState = _EstadoAnterior;
                    _PantallaCompleta = false;
                }
            }
            finally
            {
                ResumeLayout(true);
                ComprasMetAjustarDashboard();
            }
        }
        protected override void OnLoad(EventArgs Evento)
        {
            base.OnLoad(Evento);
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            ComprasMetAjustarDashboard();
            ComprasTlpPrincipal.PerformLayout();
            ComprasPnlMain.PerformLayout();
            ComprasMetActualizarInfoUsuario();
            ComprasMetIniciarDashboard();
        }
        private void ComprasMetConfigurarEstadosTab(Control Elemento, CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras Panel)
        {
            foreach (Control ControlActual in Panel.Controls)
            {
                var Etiqueta = ControlActual as CapaVista_Componentes.Componentes.ComprasUsrLabel;
                if (Etiqueta != null)
                {
                    _AyudaNavegador.SetToolTip(Elemento, Etiqueta.Texto);
                    break;
                }
            }
            Elemento.MouseEnter += (Origen, Argumentos) => Panel.ComprasProcActualizarHover(true);
            Elemento.MouseLeave += (Origen, Argumentos) =>
            {
                if (!IsHandleCreated || IsDisposed || Disposing) return;
                BeginInvoke(new Action(() =>
                {
                    if (Panel.IsDisposed || IsDisposed || Disposing) return;
                    Panel.ComprasProcActualizarHover(Panel.ClientRectangle.Contains(Panel.PointToClient(Cursor.Position)));
                }));
            };
            foreach (Control Hijo in Elemento.Controls) ComprasMetConfigurarEstadosTab(Hijo, Panel);
        }
        private void ComprasMetActualizarInfoUsuario()
        {
            string Usuario = ClsSesionSeguridad.HaySesionActiva
                ? ClsSesionSeguridad.NombreUsuario : "Sin sesión";
            string Rol = ClsSesionSeguridad.HaySesionActiva
                ? ClsSesionSeguridad.SeguridadMetRolesComoTexto() : string.Empty;
            ComprasLblUsuario.Texto = "Usuario: " + Usuario;
            ComprasLblRol.Texto = "Rol: " + Rol;
            ComprasLblUsuario.Width = TextRenderer.MeasureText(ComprasLblUsuario.Texto, ComprasLblUsuario.Font).Width + 16;
            ComprasLblRol.Width = TextRenderer.MeasureText(ComprasLblRol.Texto, ComprasLblRol.Font).Width + 16;
        }
        private void ComprasMetAbrirSubproceso<T>(CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras Opcion) where T : Form, new()
        {
            ComprasMetAbrirSubproceso(Opcion, () => new T());
        }
        // Inicio de código de "José Pablo Cano Cóbar" - carné: "0901-23-1727" - Fecha: "29/10/26"
        // Versión que recibe cómo crear el formulario, para abrir formularios de otros componentes
        private void ComprasMetAbrirSubproceso(CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras Opcion, Func<Form> CrearFormulario)
        {
            if (IsDisposed || Disposing) return;
            if (_FormularioSubproceso != null && !_FormularioSubproceso.IsDisposed)
            {
                if (ReferenceEquals(_OpcionSubproceso, Opcion))
                {
                    if (_FormularioSubproceso.WindowState == FormWindowState.Minimized)
                        _FormularioSubproceso.WindowState = FormWindowState.Normal;
                    _FormularioSubproceso.Activate();
                    return;
                }
                _FormularioSubproceso.Close();
                if (_FormularioSubproceso != null && !_FormularioSubproceso.IsDisposed) return;
            }
            Form Formulario = CrearFormulario();
            bool FlujoOrdenes = Formulario is CapaVista_OrdenesCompra.FrmEncabezados;
            while (Formulario != null)
            {
                Func<Form> CrearSiguiente = null;
                try
                {
                    Formulario.StartPosition = FormStartPosition.CenterParent;
                    Formulario.ShowInTaskbar = false;
                    Formulario.MaximizeBox = false;
                    Formulario.MinimizeBox = true;
                    Formulario.FormClosed += ComprasMetSubprocesoCerrado;
                    _FormularioSubproceso = Formulario;
                    _OpcionSubproceso = Opcion;
                    ComprasMetActualizarSeleccionSubproceso();
                    Formulario.ShowDialog(this);
                    if (FlujoOrdenes && !IsDisposed && !Disposing)
                    {
                        var Encabezados = Formulario as CapaVista_OrdenesCompra.FrmEncabezados;
                        if (Encabezados != null && Encabezados.AbrirOrden)
                            CrearSiguiente = Encabezados.ComprasFuncCrearOrdenSeleccionada;
                        else if (Formulario is CapaVista_OrdenesCompra.FrmOrdenesCompra
                            || Formulario is CapaVista_OrdenesCompra.FrmDetalleOrdenCompra)
                            CrearSiguiente = () => new CapaVista_OrdenesCompra.FrmEncabezados();
                    }
                }
                finally
                {
                    Formulario.FormClosed -= ComprasMetSubprocesoCerrado;
                    Formulario.Dispose();
                    _FormularioSubproceso = null;
                    _OpcionSubproceso = null;
                    ComprasMetActualizarSeleccionSubproceso();
                }
                Formulario = CrearSiguiente == null ? null : CrearSiguiente();
            }
            ComprasMetSolicitarIndicadores();
        }
        // Fin del código de "José Pablo Cano Cóbar" - Carné: "0901-23-1727" - Fecha: "29/10/26"
        private void ComprasMetActualizarSeleccionSubproceso()
        {
            if (IsDisposed || Disposing) return;
            foreach (Control ControlActual in ComprasTlpNavegador.Controls)
            {
                var Opcion = ControlActual as CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras;
                if (Opcion == null) continue;
                Opcion.Seleccionado = ReferenceEquals(Opcion, _OpcionSubproceso);
                if (!Opcion.Seleccionado) Opcion.ComprasProcActualizarHover(false);
            }
            ComprasTlpRegistrosCompra.Seleccionado = ReferenceEquals(_OpcionSubproceso, ComprasTlpOrdenesCompra)
                || ReferenceEquals(_OpcionSubproceso, ComprasTlpRegistroCompras)
                || ReferenceEquals(_OpcionSubproceso, ComprasTlpDevolucionesCompras);
            ComprasTlpCatalogos.Seleccionado = ReferenceEquals(_OpcionSubproceso, ComprasTlpProveedores)
                || ReferenceEquals(_OpcionSubproceso, ComprasTlpMantenimiento2)
                || ReferenceEquals(_OpcionSubproceso, ComprasTlpMantenimiento3);
        }
        private void ComprasMetSubprocesoCerrado(object Remitente, FormClosedEventArgs Evento)
        {
            Form Formulario = (Form)Remitente;
            Formulario.FormClosed -= ComprasMetSubprocesoCerrado;
            if (!ReferenceEquals(_FormularioSubproceso, Formulario)) return;
            _FormularioSubproceso = null;
            _OpcionSubproceso = null;
            ComprasMetActualizarSeleccionSubproceso();
        }
        private void ComprasMetAbrirMovimientosProveedores(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<CapaVista_MovimientosProveedores.FrmMovimientosProveedores>(ComprasTlpMovimientosProveedores);
        }
        private void ComprasMetAbrirPolizas(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<CapaVista_Polizas.FrmPolizas>(ComprasTlpPolizas);
        }
        private void ComprasMetAbrirOrdenesCompra(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<CapaVista_OrdenesCompra.FrmEncabezados>(ComprasTlpOrdenesCompra);
        }
        private void ComprasMetAbrirPagosProveedores(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<CapaVista_PagosProveedores.FrmPagosProveedores>(ComprasTlpPagosProveedores);
        }
        private void ComprasMetAbrirAprobacionFinanciera(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<CapaVista_AprobacionFinanciera.FrmAprobacionFinanciera>(ComprasTlpAprobacionFinanciera);
        }
        private void ComprasMetAbrirCierreMovimientos(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<CapaVista_CierreMovimientos.FrmCierreMovimientos>(ComprasTlpCierreMovimientos);
        }
        private void ComprasMetConfigurarDesplazamientoNavegador()
        {
            _PanelDesplazamientoNavegador = new ClsPanelNavegadorFluido
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = ComprasTlpNavegador.BackColor,
                Name = "ComprasPnlDesplazamientoNavegador"
            };
            ComprasTlpPrincipal.Controls.Remove(ComprasTlpNavegador);
            ComprasTlpPrincipal.Controls.Add(_PanelDesplazamientoNavegador, 0, 0);
            ComprasTlpPrincipal.SetRowSpan(_PanelDesplazamientoNavegador, 2);
            ComprasTlpNavegador.MinimumSize = System.Drawing.Size.Empty;
            ComprasTlpNavegador.MaximumSize = System.Drawing.Size.Empty;
            ComprasTlpNavegador.Dock = DockStyle.None;
            ComprasTlpNavegador.Location = System.Drawing.Point.Empty;
            _PanelDesplazamientoNavegador.Controls.Add(ComprasTlpNavegador);
            _PanelDesplazamientoNavegador.SizeChanged += (Origen, Evento) => ComprasMetAjustarDesplazamientoNavegador();
            ComprasMetConectarEnfoqueNavegador(_PanelDesplazamientoNavegador);
        }
        private void ComprasMetConectarEnfoqueNavegador(Control Elemento)
        {
            Elemento.Enter += (Origen, Evento) => ComprasMetMostrarOpcionNavegador(Elemento);
            foreach (Control Hijo in Elemento.Controls) ComprasMetConectarEnfoqueNavegador(Hijo);
        }
        private void ComprasMetAjustarDesplazamientoNavegador()
        {
            if (_PanelDesplazamientoNavegador == null) return;
            int AltoContenido = ComprasTlpNavegador.Padding.Vertical;
            foreach (RowStyle Fila in ComprasTlpNavegador.RowStyles)
                if (Fila.SizeType == SizeType.Absolute) AltoContenido += (int)Math.Ceiling(Fila.Height);
            int AltoVisible = _PanelDesplazamientoNavegador.ClientSize.Height;
            int Alto = Math.Max(AltoVisible, AltoContenido);
            _LimiteDesplazamiento = Math.Max(0, Alto - AltoVisible);
            _PosicionNavegador = Math.Max(0, Math.Min(_PosicionNavegador, _LimiteDesplazamiento));
            _DestinoNavegador = Math.Max(0, Math.Min(_DestinoNavegador, _LimiteDesplazamiento));
            _DesplazamientoNavegador = (int)Math.Round(_PosicionNavegador);
            ComprasTlpNavegador.SetBounds(0, -_DesplazamientoNavegador, ComprasTlpNavegador.Width, Alto);
        }
        private void ComprasMetAlternarCatalogos(object Remitente, EventArgs Evento)
        {
            _CatalogosAbierto = !_CatalogosAbierto;
            ComprasMetAcomodarCatalogos();
        }
        private void ComprasMetAcomodarCatalogos()
        {
            ComprasTlpNavegador.SuspendLayout();
            try
            {
                Control[] Opciones = { ComprasTlpProveedores, ComprasTlpMantenimiento2, ComprasTlpMantenimiento3 };
                foreach (Control Opcion in Opciones)
                {
                    Opcion.Visible = _CatalogosAbierto;
                    Opcion.Margin = new Padding(_NavegadorContraido ? 0 : 18, 2, 0, 2);
                    ComprasTlpNavegador.RowStyles[ComprasTlpNavegador.GetRow(Opcion)].Height = _CatalogosAbierto ? ComprasTlpNavegador.RowStyles[ComprasTlpNavegador.GetRow(ComprasTlpCatalogos)].Height : 0F;
                }
            }
            finally
            {
                ComprasTlpNavegador.ResumeLayout(true);
            }
            ComprasMetAjustarDesplazamientoNavegador();
            ComprasLblCatalogos.Texto = "Catálogos";
            ComprasBtnIconoCatalogos.AccessibleName = _CatalogosAbierto ? "Contraer catálogos" : "Expandir catálogos";
            ComprasTlpCatalogos.Invalidate();
        }
        private void ComprasMetPintarFlechaCatalogos(object Remitente, PaintEventArgs Evento)
        {
            if (_NavegadorContraido) return;
            int X = ComprasTlpCatalogos.Width - 26;
            int Y = ComprasTlpCatalogos.Height / 2;
            System.Drawing.Point[] Flecha = _CatalogosAbierto
                ? new[] { new System.Drawing.Point(X, Y - 3), new System.Drawing.Point(X + 10, Y - 3), new System.Drawing.Point(X + 5, Y + 4) }
                : new[] { new System.Drawing.Point(X + 2, Y - 6), new System.Drawing.Point(X + 2, Y + 6), new System.Drawing.Point(X + 9, Y) };
            using (var Pincel = new System.Drawing.SolidBrush(ComprasLblCatalogos.ColorTexto))
                Evento.Graphics.FillPolygon(Pincel, Flecha);
        }
        private void ComprasMetAlternarRegistrosCompra(object Remitente, EventArgs Evento)
        {
            _RegistrosCompraAbierto = !_RegistrosCompraAbierto;
            ComprasMetAcomodarRegistrosCompra();
        }
        private void ComprasMetAcomodarRegistrosCompra()
        {
            ComprasTlpNavegador.SuspendLayout();
            try
            {
                Control[] Opciones = { ComprasTlpOrdenesCompra, ComprasTlpRegistroCompras, ComprasTlpDevolucionesCompras };
                foreach (Control Opcion in Opciones)
                {
                    Opcion.Visible = _RegistrosCompraAbierto;
                    Opcion.Margin = new Padding(_NavegadorContraido ? 0 : 18, 2, 0, 2);
                    ComprasTlpNavegador.RowStyles[ComprasTlpNavegador.GetRow(Opcion)].Height = _RegistrosCompraAbierto ? ComprasTlpNavegador.RowStyles[ComprasTlpNavegador.GetRow(ComprasTlpRegistrosCompra)].Height : 0F;
                }
            }
            finally
            {
                ComprasTlpNavegador.ResumeLayout(true);
            }
            ComprasMetAjustarDesplazamientoNavegador();
            ComprasLblRegistrosCompra.Texto = "Registros de compra";
            ComprasBtnIconoRegistrosCompra.AccessibleName = _RegistrosCompraAbierto ? "Contraer registros de compra" : "Expandir registros de compra";
            ComprasTlpRegistrosCompra.Invalidate();
        }
        private void ComprasMetPintarFlechaRegistrosCompra(object Remitente, PaintEventArgs Evento)
        {
            if (_NavegadorContraido) return;
            int X = ComprasTlpRegistrosCompra.Width - 26;
            int Y = ComprasTlpRegistrosCompra.Height / 2;
            System.Drawing.Point[] Flecha = _RegistrosCompraAbierto
                ? new[] { new System.Drawing.Point(X, Y - 3), new System.Drawing.Point(X + 10, Y - 3), new System.Drawing.Point(X + 5, Y + 4) }
                : new[] { new System.Drawing.Point(X + 2, Y - 6), new System.Drawing.Point(X + 2, Y + 6), new System.Drawing.Point(X + 9, Y) };
            using (var Pincel = new System.Drawing.SolidBrush(ComprasLblRegistrosCompra.ColorTexto))
                Evento.Graphics.FillPolygon(Pincel, Flecha);
        }
        private void ComprasMetAbrirProveedores(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<FrmMantenimientoProveedor>(ComprasTlpProveedores);
        }
        // Inicio de código de "José Pablo Cano Cóbar" - carné: "0901-23-1727" - Fecha: "29/10/26"
        private void ComprasMetAbrirBitacora(object Remitente, EventArgs Evento)
        {
            if (CrearBitacora == null)
            {
                MessageBox.Show(this,
                    "La bitácora está disponible al ingresar a Compras desde el inicio de sesión de Seguridad.",
                    "Bitácora", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ComprasMetAbrirSubproceso(ComprasTlpBitacora, CrearBitacora);
        }
        // Fin del código de "José Pablo Cano Cóbar" - Carné: "0901-23-1727" - Fecha: "29/10/26"
        // Inicio de código de "José Pablo Cano Cóbar" - carné: "0901-23-1727" - Fecha: "29/10/26"
        private void ComprasMetAbrirHerramientas(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<CapaVista_Herramientas.FrmHerramientasMoneda>(ComprasTlpHerramientas);
        }
        // Fin del código de "José Pablo Cano Cóbar" - Carné: "0901-23-1727" - Fecha: "29/10/26"
        private void ComprasMetAbrirPlazos(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<FrmMantenimientoPlazo>(ComprasTlpMantenimiento2);
        }
        private void ComprasMetAbrirConceptosProveedor(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<FrmMantenimientoConceptoProveedor>(ComprasTlpMantenimiento3);
        }
        private void ComprasProcSalirClick(object Remitente, EventArgs Evento)
        {
            DialogResult Respuesta = MessageBox.Show(this,
                "¿Desea cerrar sesión?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (Respuesta == DialogResult.Yes)
            {
                Close();
                return;
            }
            ActiveControl = null;
            ComprasTlpSalir.Seleccionado = false;
            ComprasTlpSalir.ComprasProcActualizarHover(false);
            BeginInvoke(new Action(() =>
            {
                if (IsDisposed || Disposing || ComprasTlpSalir.IsDisposed) return;
                ComprasTlpSalir.ComprasProcActualizarHover(false);
            }));
        }
        private void ComprasProcAlternarMenuClick(object Remitente, EventArgs Evento)
        {
            if (IsDisposed || Disposing || !ComprasTlpPrincipal.IsHandleCreated) return;
            ComprasMetCambiarNavegador();
        }
        private void ComprasMetCambiarNavegador()
        {
            if (_CambiandoNavegador || IsDisposed || Disposing) return;
            _CambiandoNavegador = true;
            var Paneles = new List<Control>();
            ComprasMetRecopilarContenedores(this, Paneles);
            IntPtr Ventana = ComprasTlpPrincipal.IsHandleCreated && Visible
                ? ComprasTlpPrincipal.Handle : IntPtr.Zero;
            try
            {
                if (Ventana != IntPtr.Zero) ComprasFuncSuspenderRedibujado(Ventana, 0x000B, IntPtr.Zero, IntPtr.Zero);
                foreach (Control Panel in Paneles) Panel.SuspendLayout();
                if (_AnimacionNavegador != null) _AnimacionNavegador.Stop();
                _RelojNavegador.Stop();
                _PosicionNavegador = _DesplazamientoNavegador;
                _DestinoNavegador = _PosicionNavegador;
                _NavegadorContraido = !_NavegadorContraido;
                ComprasMetAlternarSegundaColumna(ComprasTlpNavegador);
                foreach (Control ControlActual in ComprasTlpNavegador.Controls)
                {
                    TableLayoutPanel Panel = ControlActual as TableLayoutPanel;
                    if (Panel != null) ComprasMetAlternarSegundaColumna(Panel);
                }
                ComprasTlpNavegador.Width = _NavegadorContraido
                    ? 64 + ComprasTlpNavegador.Padding.Horizontal : 270;
                ComprasTlpPrincipal.ColumnStyles[0].Width = _NavegadorContraido
                    ? ComprasTlpNavegador.Width + 16 : 286F;
                ComprasMetAcomodarCatalogos();
                ComprasMetAcomodarRegistrosCompra();
                ComprasBtnAlternarNavegador.AccessibleName = _NavegadorContraido
                    ? "Expandir menú" : "Contraer menú";
            }
            finally
            {
                try
                {
                    foreach (Control Panel in Paneles) Panel.ResumeLayout(true);
                    ComprasMetAjustarDesplazamientoNavegador();
                    _CambiandoNavegador = false;
                    ComprasMetAjustarDashboard();
                }
                finally
                {
                    _CambiandoNavegador = false;
                    if (Ventana != IntPtr.Zero && !IsDisposed && ComprasTlpPrincipal.IsHandleCreated
                        && ComprasTlpPrincipal.Handle == Ventana)
                    {
                        ComprasFuncSuspenderRedibujado(Ventana, 0x000B, new IntPtr(1), IntPtr.Zero);
                        ComprasFuncRedibujarVentana(Handle, IntPtr.Zero, IntPtr.Zero, 0x0181);
                    }
                }
            }
        }
        private void ComprasMetAjustarDashboard()
        {
            ComprasPnlDashboard.PerformLayout();
            ComprasTlpDashboard.PerformLayout();
            ComprasTlpHero.PerformLayout();
            ComprasMetAjustarPresentacion();
        }
        private void ComprasMetAjustarPresentacion()
        {
            if (_CambiandoNavegador) return;
            bool Compacto = ComprasTlpPresentacion.Height < 320;
            float TamanoTitulo = Compacto ? 32F : 42F;
            if (Math.Abs(ComprasLblBienvenido.TamanoTipografia - TamanoTitulo) > 0.01F)
                ComprasLblBienvenido.TamanoTipografia = TamanoTitulo;
            ComprasLblBienvenido.Margin = new Padding(0, Compacto ? 6 : 15, 12, Compacto ? 6 : 15);
            float TamanoModulo = Compacto ? 16F : 19F;
            if (Math.Abs(ComprasLblModulo.TamanoTipografia - TamanoModulo) > 0.01F)
                ComprasLblModulo.TamanoTipografia = TamanoModulo;
            RowStyle Fila = ComprasTlpPresentacion.RowStyles[1];
            SizeType Tipo = SizeType.Absolute;
            float Altura = Math.Max(ComprasLblModulo.Font.Height * 2,
                TextRenderer.MeasureText(ComprasLblModulo.Texto, ComprasLblModulo.Font,
                    new System.Drawing.Size(Math.Max(1, ComprasLblModulo.Width - 6), int.MaxValue),
                    TextFormatFlags.WordBreak).Height) + ComprasLblModulo.Margin.Vertical;
            if (Fila.SizeType == Tipo && Fila.Height == Altura) return;
            ComprasTlpPresentacion.SuspendLayout();
            try
            {
                Fila.SizeType = Tipo;
                Fila.Height = Altura;
            }
            finally
            {
                ComprasTlpPresentacion.ResumeLayout(true);
            }
        }
        private void ComprasMetAlternarSegundaColumna(TableLayoutPanel Panel)
        {
            if (Panel.ColumnCount < 2 || Panel.ColumnStyles.Count < 2) return;
            Panel.SuspendLayout();
            try
            {
                ColumnStyle Columna = Panel.ColumnStyles[1];
                if (_NavegadorContraido)
                {
                    _ColumnasNavegador[Panel] = new ColumnStyle(Columna.SizeType, Columna.Width);
                    foreach (Control ControlActual in Panel.Controls)
                        if (Panel.GetColumn(ControlActual) == 1) ControlActual.Visible = false;
                    Columna.SizeType = SizeType.Absolute;
                    Columna.Width = 0;
                }
                else
                {
                    ColumnStyle Original;
                    if (!_ColumnasNavegador.TryGetValue(Panel, out Original)) return;
                    Columna.SizeType = Original.SizeType;
                    Columna.Width = Original.Width;
                    foreach (Control ControlActual in Panel.Controls)
                        if (Panel.GetColumn(ControlActual) == 1) ControlActual.Visible = true;
                    _ColumnasNavegador.Remove(Panel);
                }
            }
            finally
            {
                Panel.ResumeLayout(true);
            }
        }
    }
}
