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
        private Form _FormularioSubproceso;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras _OpcionSubproceso;
        private bool _NavegadorContraido;
        private bool _PantallaCompleta;
        private FormBorderStyle _BordeAnterior;
        private FormWindowState _EstadoAnterior;
        private System.Drawing.Rectangle _LimitesAnteriores;
        private readonly Dictionary<TableLayoutPanel, ColumnStyle> _ColumnasNavegador =
            new Dictionary<TableLayoutPanel, ColumnStyle>();

        public FrmMDICompras()
        {
            InitializeComponent();
            ComprasBtnAyuda.Enabled = false;
            ComprasTlpBitacora.Enabled = false;
            ComprasTlpHerramientas.Enabled = false;
            ComprasLblBienvenido.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.TituloBienvenida;
            ComprasLblKpi1.Texto = "Antigüedad promedio de saldos";
            ComprasLblKpi2.Texto = "Órdenes autorizadas a tiempo (%)";
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
                Panel.RowStyles[1].Height = 30F;
                Panel.RowStyles[2].SizeType = SizeType.Absolute;
                Panel.RowStyles[2].Height = 38F;
                foreach (Control Elemento in Panel.Controls)
                    if (Panel.GetRow(Elemento) == 2) Elemento.Margin = new Padding(2);
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
                CreateParams Parametros = base.CreateParams;
                // Buffer the entire child control tree during sidebar resizing.
                // DoubleBuffered on the form alone does not buffer child windows.
                const int EstiloComposicion = 0x02000000; // WS_EX_COMPOSITED
                Parametros.ExStyle |= EstiloComposicion;
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
        }

        private void ComprasMetConfigurarEstadosTab(Control Elemento, CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras Panel)
        {
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
            if (IsDisposed || Disposing) return;
            if (_FormularioSubproceso != null && !_FormularioSubproceso.IsDisposed)
            {
                if (_FormularioSubproceso is T)
                {
                    if (_FormularioSubproceso.WindowState == FormWindowState.Minimized)
                        _FormularioSubproceso.WindowState = FormWindowState.Normal;
                    _FormularioSubproceso.Activate();
                    return;
                }
                _FormularioSubproceso.Close();
                if (_FormularioSubproceso != null && !_FormularioSubproceso.IsDisposed) return;
            }

            Form Formulario = new T();
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
                        else if (Formulario is CapaVista_OrdenesCompra.FrmOrdenesCompra)
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
        }

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

        private void ComprasMetAbrirProveedores(object Remitente, EventArgs Evento)
        {
            ComprasMetAbrirSubproceso<FrmMantenimientoProveedor>(ComprasTlpProveedores);
        }

        private void ComprasMetOpcionPendienteClick(object Remitente, EventArgs Evento)
        {
            
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
            SuspendLayout();
            try
            {
                ComprasMetCambiarNavegador();
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        private void ComprasMetCambiarNavegador()
        {
            _NavegadorContraido = !_NavegadorContraido;
            ComprasTlpPrincipal.SuspendLayout();
            ComprasTlpNavegador.SuspendLayout();
            try
            {
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
                ComprasBtnAlternarNavegador.AccessibleName = _NavegadorContraido
                    ? "Expandir menú" : "Contraer menú";
            }
            finally
            {
                ComprasTlpNavegador.ResumeLayout(true);
                ComprasTlpPrincipal.ResumeLayout(true);
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
