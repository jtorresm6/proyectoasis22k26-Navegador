// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Gestiona las pestañas y el cálculo de comisiones del período.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CapaControlador_ComisionVendedores_Ventas.Contratos;
using CapaControlador_ComisionVendedores_Ventas.Controladores;
using CapaControlador_ComisionVendedores_Ventas.DTO;
using CapaControlador_ComisionVendedores_Ventas.Enumeraciones;

namespace CapaVista_ComisionVendedores_Ventas
{
    public partial class FrmComisionesVendedoresVentas : Form
    {
        private readonly IControladorComision _ControladorComision;
        private List<ClsComisionVendedorResultado> _Resultados;
        private List<ClsComisionFacturaDetalle> _DetallesFactura;
        private List<ClsProductoPorcentaje> _ProductosPorcentaje;
        private const int _FilasPorPagina = 9;
        private int _PaginaResultados;
        private int _PaginaFactura;
        private int _PaginaPorcentajes;
        private bool _CargandoCatalogosFactura;
        private int _IdFacturaVistaPrevia;
        private bool _CargandoPorcentajes;
        private List<ClsComisionNominaVista> _ComisionesNomina;
        private int _PaginaNomina;
        private readonly ErrorProvider VentasErpValidacion = new ErrorProvider();
        private readonly Dictionary<DataGridView, string> _ColumnasOrdenadas = new Dictionary<DataGridView, string>();
        private readonly Dictionary<DataGridView, bool> _OrdenAscendente = new Dictionary<DataGridView, bool>();

        public FrmComisionesVendedoresVentas()
            : this(new ClsControladorComision())
        {
        }

        public FrmComisionesVendedoresVentas(IControladorComision ControladorComision)
        {
            _ControladorComision = ControladorComision ?? throw new ArgumentNullException("ControladorComision");
            _Resultados = new List<ClsComisionVendedorResultado>();
            _DetallesFactura = new List<ClsComisionFacturaDetalle>();
            _ProductosPorcentaje = new List<ClsProductoPorcentaje>();
            InitializeComponent();
            VentasErpValidacion.ContainerControl = this;
            VentasErpValidacion.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void VentasMetCargarFormulario(object Sender, EventArgs Argumentos)
        {
            if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            if (VentasTbcPestanas.SelectedTab == VentasTbpAsignarFactura)
                VentasMetCargarCatalogosClick(Sender, Argumentos);
        }

        private void VentasMetConfirmarEdicionCelda(object Sender, EventArgs Argumentos)
        {
            if (VentasDgvDetalleFactura.IsCurrentCellDirty)
                VentasDgvDetalleFactura.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void VentasMetCalcularClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstado,
                    VentasDtpFechaInicio, VentasDtpFechaFin);
                if (VentasDtpFechaInicio.Value.Date > VentasDtpFechaFin.Value.Date)
                    throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");
                _Resultados = _ControladorComision.VentasMetCalcular(EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado,
                    VentasDtpFechaInicio.Value.Date, VentasDtpFechaFin.Value.Date);
                _PaginaResultados = 0;
                VentasMetMostrarPaginaResultados();
                VentasLblEstado.Text = _Resultados.Count == 0
                    ? "No hay comisiones asignadas ni pagos de comisión en el rango seleccionado."
                    : "Cálculo completado. Se muestran los movimientos de comisión devengados en el período.";
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblEstado, Excepcion.Message,
                    VentasDtpFechaInicio, VentasDtpFechaFin);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static DialogResult VentasMetConfirmar(IWin32Window VentanaPropietaria, string Titulo, string Mensaje, string TextoAccion)
        {
            return VentasMetMostrarDialogoOperacion(VentanaPropietaria, Titulo, Mensaje, true, TextoAccion);
        }

        internal static void VentasMetMostrarExito(IWin32Window VentanaPropietaria, string Titulo, string Mensaje)
        {
            VentasMetMostrarDialogoOperacion(VentanaPropietaria, Titulo, Mensaje, false, "Aceptar");
        }

        private static DialogResult VentasMetMostrarDialogoOperacion(
            IWin32Window VentanaPropietaria, string Titulo, string Mensaje, bool EsConfirmacion, string TextoAccion)
        {
            using (var Dialogo = new FrmOperacionComision(Titulo, Mensaje, EsConfirmacion, TextoAccion))
                return VentanaPropietaria == null ? Dialogo.ShowDialog() : Dialogo.ShowDialog(VentanaPropietaria);
        }

        private void VentasMetCambiarPestana(object Sender, EventArgs Argumentos)
        {
            if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            if (VentasTbcPestanas.SelectedTab == VentasTbpAsignarFactura)
                VentasMetCargarCatalogosClick(Sender, Argumentos);
            else if (VentasTbcPestanas.SelectedTab == VentasTbpMantenimiento)
                VentasMetBuscarClick(Sender, Argumentos);
            else if (VentasTbcPestanas.SelectedTab == VentasTbpNomina)
                VentasMetCargarNominaClick(Sender, Argumentos);
        }

        private void VentasMetCargarNominaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasLblNominaEstado.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
                _ComisionesNomina = _ControladorComision.VentasMetObtenerComisionesNomina();
                _PaginaNomina = 0;
                VentasDgvNomina.DataSource = null;
                VentasMetMostrarPaginaNomina();
                VentasLblNominaEstado.Text = _ComisionesNomina.Count == 0
                    ? "Aún no hay abonos de comisión. Asigne un vendedor y registre pagos confirmados del cliente."
                    : _ComisionesNomina.Count + " abono(s) de comisión. Cada fila corresponde a un pago confirmado del cliente.";
            }
            catch (Exception Error)
            {
                VentasLblNominaEstado.ForeColor = System.Drawing.Color.Firebrick;
                VentasLblNominaEstado.Text = "No se pudieron cargar las comisiones: " + VentasMetObtenerDetalleError(Error);
            }
        }

        private void VentasMetAbrirAyuda(object Sender, EventArgs Argumentos)
        {
            try
            {
                Help.ShowHelp(this, VentasMetRutaAyuda(), HelpNavigator.Topic,
                    "html/ComisionVendedores_General.html");
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, "No fue posible abrir la ayuda. " + Error.Message,
                    "Ayuda de Comisiones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string VentasMetRutaAyuda()
        {
            const string Recurso = "CapaVista_ComisionVendedores_Ventas.Ayudas.ComisionVendedoresAyudas.chm";
            using (Stream Manual = typeof(FrmComisionesVendedoresVentas).Assembly.GetManifestResourceStream(Recurso))
            {
                if (Manual == null) throw new FileNotFoundException("No se encontró la ayuda de Comisiones.");
                byte[] Huella;
                using (var Resumen = System.Security.Cryptography.SHA256.Create()) Huella = Resumen.ComputeHash(Manual);
                string Carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "EmbutidosDeCalidad", "Ventas", "Ayuda");
                Directory.CreateDirectory(Carpeta);
                string Archivo = Path.Combine(Carpeta, "ComisionVendedores_" +
                    BitConverter.ToString(Huella, 0, 8).Replace("-", "") + ".chm");
                if (File.Exists(Archivo)) return Archivo;

                string Temporal = Archivo + "." + Guid.NewGuid().ToString("N") + ".tmp";
                try
                {
                    Manual.Position = 0;
                    using (var Destino = File.Create(Temporal)) Manual.CopyTo(Destino);
                    try { File.Move(Temporal, Archivo); }
                    catch (IOException) when (File.Exists(Archivo)) { }
                }
                finally { File.Delete(Temporal); }
                return Archivo;
            }
        }

        private int VentasMetObtenerUltimaPagina(int TotalRegistros)
        {
            return Math.Max(0, (int)Math.Ceiling(TotalRegistros / (double)_FilasPorPagina) - 1);
        }

        private void VentasMetMostrarError(Label EtiquetaEstado, string Mensaje, params Control[] Controles)
        {
            EtiquetaEstado.ForeColor = System.Drawing.Color.Firebrick;
            EtiquetaEstado.Text = Mensaje;
            foreach (Control ControlValidado in Controles)
            {
                ControlValidado.BackColor = System.Drawing.Color.MistyRose;
                if (ControlValidado.Parent is Panel Contenedor)
                    Contenedor.BackColor = System.Drawing.Color.Firebrick;
                VentasErpValidacion.SetError(ControlValidado, Mensaje);
            }
        }

        private string VentasMetObtenerDetalleError(Exception Excepcion)
        {
            List<string> Detalles = new List<string>();
            for (Exception Actual = Excepcion; Actual != null; Actual = Actual.InnerException)
            {
                string Mensaje = Actual.Message;
                Detalles.Add(String.IsNullOrWhiteSpace(Mensaje)
                    ? Actual.GetType().FullName
                    : Actual.GetType().FullName + ": " + Mensaje);
            }
            return String.Join(Environment.NewLine, Detalles);
        }

        private static string VentasMetNormalizarMetodo(string Metodo)
        {
            if (String.IsNullOrWhiteSpace(Metodo)) return String.Empty;
            string Valor = Metodo.Trim().Replace('í', 'i').Replace('Í', 'I')
                .Replace('á', 'a').Replace('Á', 'A').Replace('é', 'e').Replace('É', 'E');
            switch (Valor.ToUpperInvariant())
            {
                case "PRODUCTO": return "Producto";
                case "MARCA": return "Marca";
                case "LINEA": return "Linea";
                case "CATEGORIA": return "Categoria";
                default: return String.Empty;
            }
        }

        private void VentasMetActualizarBoton(Button Boton, bool Habilitado)
        {
            Boton.Enabled = Habilitado;
            Boton.BackColor = Habilitado ? System.Drawing.Color.FromArgb(59, 29, 74) : System.Drawing.Color.LightGray;
            Boton.ForeColor = Habilitado ? System.Drawing.Color.FromArgb(248, 245, 249) : System.Drawing.SystemColors.GrayText;
        }

        private void VentasMetRestablecerEstado(Label EtiquetaEstado, params Control[] Controles)
        {
            EtiquetaEstado.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            foreach (Control ControlValidado in Controles)
            {
                ControlValidado.BackColor = System.Drawing.SystemColors.Window;
                if (ControlValidado.Parent is Panel Contenedor)
                    Contenedor.BackColor = BackColor;
                VentasErpValidacion.SetError(ControlValidado, String.Empty);
            }
        }

        private string VentasMetObtenerTextoPagina(int Pagina, int TotalRegistros)
        {
            int TotalPaginas = TotalRegistros == 0 ? 0 : VentasMetObtenerUltimaPagina(TotalRegistros) + 1;
            return "Página " + (TotalPaginas == 0 ? 0 : Pagina + 1) + " de " + TotalPaginas + " · " + TotalRegistros + " registros";
        }

        private EnumMetodoComision VentasMetObtenerMetodo(string Texto)
        {
            switch (Texto)
            {
                case "Vendedor": return EnumMetodoComision.Vendedor;
                case "Marca": return EnumMetodoComision.Marca;
                case "Línea": return EnumMetodoComision.Linea;
                case "Producto": return EnumMetodoComision.Producto;
                case "Categoría": return EnumMetodoComision.Categoria;
                default: throw new ArgumentException("Seleccione un método de comisión.");
            }
        }
    }
}
