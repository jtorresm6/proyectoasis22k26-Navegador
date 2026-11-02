using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_ComisionVendedores.Contratos;
using CapaControlador_ComisionVendedores.Controladores;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Enumeraciones;

namespace CapaVista_ComisionVendedores
{
    public partial class FrmMdiComisionVendedores : Form
    {
        private readonly IControladorComision _ControladorComision;
        private List<ClsComisionVendedorResultado> _Resultados;
        private List<ClsComisionFacturaDetalle> _DetallesFactura;
        private List<ClsProductoPorcentaje> _ProductosPorcentaje;
        private EnumMetodoComision _MetodoCalculado = EnumMetodoComision.Vendedor;
        private const int _FilasPorPagina = 9;
        private int _PaginaResultados;
        private int _PaginaFactura;
        private int _PaginaPorcentajes;

        public FrmMdiComisionVendedores()
            : this(new ClsControladorComision())
        {
        }

        public FrmMdiComisionVendedores(IControladorComision ControladorComision)
        {
            InitializeComponent();
            var AreaTrabajo = Screen.FromControl(this).WorkingArea;
            AutoScrollMinSize = ClientSize;
            AutoScroll = true;
            MinimumSize = System.Drawing.Size.Empty;
            Size = new System.Drawing.Size(Math.Min(Width, Math.Max(1, AreaTrabajo.Width - 32)),
                Math.Min(Height, Math.Max(1, AreaTrabajo.Height - 32)));
            _ControladorComision = ControladorComision ?? throw new ArgumentNullException("ControladorComision");
            _Resultados = new List<ClsComisionVendedorResultado>();
            _DetallesFactura = new List<ClsComisionFacturaDetalle>();
            _ProductosPorcentaje = new List<ClsProductoPorcentaje>();
        }

        private void VentasMetCalcularClick(object Origen, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstado,
                    VentasDtpFechaInicio, VentasDtpFechaFin, VentasCboMetodo);
                _MetodoCalculado = VentasFuncObtenerMetodo(VentasCboMetodo.SelectedItem.ToString());
                if (VentasDtpFechaInicio.Value.Date > VentasDtpFechaFin.Value.Date)
                    throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");
                _Resultados = _ControladorComision.VentasFuncCalcular(_MetodoCalculado, EnumBaseComision.Ventas,
                    VentasDtpFechaInicio.Value.Date, VentasDtpFechaFin.Value.Date);
                _PaginaResultados = 0;
                VentasMetMostrarPaginaResultados();
                VentasMetActualizarBoton(VentasBtnGuardar, _Resultados.Count > 0,
                    System.Drawing.Color.FromArgb(201, 162, 74));
                VentasLblEstado.Text = _Resultados.Count == 0 ? "No se encontraron facturas para el periodo." : "Cálculo completado. Base: VENTAS.";
            }
            catch (Exception Excepcion)
            {
                VentasMetActualizarBoton(VentasBtnGuardar, false,
                    System.Drawing.Color.FromArgb(201, 162, 74));
                VentasMetMostrarError(VentasLblEstado, Excepcion.Message,
                    VentasDtpFechaInicio, VentasDtpFechaFin, VentasCboMetodo);
                MessageBox.Show(Excepcion.Message, "7013 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetGuardarClick(object Origen, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstado);
                int Guardados = _ControladorComision.VentasFuncGuardar(_Resultados,
                    VentasDtpFechaInicio.Value.Date, VentasDtpFechaFin.Value.Date,
                    _MetodoCalculado, EnumBaseComision.Ventas);
                VentasLblEstado.Text = "Resultados guardados en ComisionCalculada.";
                VentasMetActualizarBoton(VentasBtnGuardar, false,
                    System.Drawing.Color.FromArgb(201, 162, 74));
                MessageBox.Show("Se guardaron " + Guardados + " resultados.", "7013 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblEstado, Excepcion.Message);
                MessageBox.Show(Excepcion.Message, "7013 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetBuscarClick(object Origen, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblMantenimientoEstado, VentasCboAlcance);
                List<ClsProductoPorcentaje> Productos = VentasFuncCargarPorcentajes();
                VentasLblMantenimientoEstado.Text = Productos.Count + " producto(s) encontrados.";
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblMantenimientoEstado, Excepcion.Message, VentasCboAlcance);
                MessageBox.Show(Excepcion.Message, "7013 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetCargarCatalogosClick(object Origen, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstadoFactura,
                    VentasCboFactura, VentasCboVendedorFactura, VentasCboMetodoFactura);
                VentasCboFactura.DataSource = null;
                VentasCboVendedorFactura.DataSource = null;
                VentasCboFactura.DataSource = _ControladorComision.VentasFuncObtenerFacturas();
                VentasCboVendedorFactura.DataSource = _ControladorComision.VentasFuncObtenerVendedores();
                VentasLblEstadoFactura.Text = "Seleccione una factura y el vendedor responsable.";
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblEstadoFactura, Excepcion.Message,
                    VentasCboFactura, VentasCboVendedorFactura);
                MessageBox.Show(Excepcion.Message, "7013 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetCalcularFacturaClick(object Origen, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstadoFactura,
                    VentasCboFactura, VentasCboVendedorFactura, VentasCboMetodoFactura);
                if (VentasCboFactura.SelectedValue == null || VentasCboVendedorFactura.SelectedValue == null)
                    throw new ArgumentException("Cargue las facturas y vendedores, y seleccione ambos.");

                int IdFactura = Convert.ToInt32(VentasCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(VentasCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                EnumMetodoComision Metodo = VentasFuncObtenerMetodo(VentasCboMetodoFactura.SelectedItem.ToString());
                List<ClsComisionFacturaDetalle> DetalleFacturaCalculado = _ControladorComision.VentasFuncCalcularFactura(IdFactura, IdVendedor, Metodo);
                VentasDgvDetalleFactura.DataSource = null;
                _DetallesFactura = DetalleFacturaCalculado;
                _PaginaFactura = 0;
                VentasMetMostrarPaginaFactura();

                decimal TotalComision = 0m, Disponible = 0m, Pendiente = 0m, ComisionUltimoAbono = 0m;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    TotalComision += Detalle.ComisionTotal;
                    Disponible += Detalle.ComisionDisponible;
                    Pendiente += Detalle.ComisionPendiente;
                    ComisionUltimoAbono += Detalle.ComisionUltimoAbono;
                }
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                ClsVendedorComisionVista Vendedor = VentasCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal PagadoFactura = Factura == null ? 0m : Factura.TotalPagado;
                VentasLblEstadoFactura.Text = "Factura " + IdFactura + " · " + (Vendedor == null ? "" : Vendedor.NombreVendedor) +
                    " · Cobrado Q " + PagadoFactura.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Comisión total Q " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Disponible Q " + Disponible.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Comisión último abono Q " + ComisionUltimoAbono.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Pendiente Q " + Pendiente.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · El registro persistente se conectará a la tabla que proporcionarás.";
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblEstadoFactura, Excepcion.Message,
                    VentasCboFactura, VentasCboVendedorFactura, VentasCboMetodoFactura);
                MessageBox.Show(Excepcion.Message, "7013 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetActualizarClick(object Origen, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblMantenimientoEstado, VentasTxtPorcentaje);
                decimal Porcentaje;
                if (!decimal.TryParse(VentasTxtPorcentaje.Text, NumberStyles.Number,
                    CultureInfo.GetCultureInfo("es-GT"), out Porcentaje))
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "Ingrese un porcentaje válido entre 0 y 100.", VentasTxtPorcentaje);
                    throw new ArgumentException("Ingrese un porcentaje válido entre 0 y 100.");
                }
                if (VentasDgvPorcentajes.SelectedRows.Count == 0)
                    throw new ArgumentException("Busque y seleccione al menos un elemento para actualizar.");

                EnumMetodoComision Metodo = VentasFuncObtenerMetodo(VentasCboAlcance.SelectedItem.ToString());
                HashSet<int> IdsProducto = new HashSet<int>();
                foreach (DataGridViewRow Fila in VentasDgvPorcentajes.SelectedRows)
                {
                    ClsProductoPorcentaje Producto = Fila.DataBoundItem as ClsProductoPorcentaje;
                    if (Producto == null) continue;
                    IdsProducto.Add(Producto.IdProducto);
                }

                if (IdsProducto.Count == 0) throw new ArgumentException("Seleccione una fila válida para actualizar.");
                int Actualizados = _ControladorComision.VentasFuncActualizarPorcentaje(Metodo, IdsProducto.ToList(), Porcentaje);

                _ProductosPorcentaje = _ControladorComision.VentasFuncConsultarPorcentajes(Metodo);
                _PaginaPorcentajes = Math.Min(_PaginaPorcentajes, VentasFuncObtenerUltimaPagina(_ProductosPorcentaje.Count));
                VentasMetMostrarPaginaPorcentajes();
                VentasLblMantenimientoEstado.Text = Actualizados + " producto(s) actualizado(s).";
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblMantenimientoEstado, Excepcion.Message);
                MessageBox.Show(Excepcion.Message, "7013 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ClsProductoPorcentaje> VentasFuncCargarPorcentajes()
        {
            _ProductosPorcentaje = _ControladorComision.VentasFuncConsultarPorcentajes(
                VentasFuncObtenerMetodo(VentasCboAlcance.SelectedItem.ToString()));
            _PaginaPorcentajes = 0;
            VentasMetMostrarPaginaPorcentajes();
            return _ProductosPorcentaje;
        }

        private void VentasMetMostrarPaginaResultados()
        {
            VentasDgvResultados.DataSource = _Resultados.Skip(_PaginaResultados * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            VentasLblPaginaResultados.Text = VentasFuncObtenerTextoPagina(_PaginaResultados, _Resultados.Count);
            VentasMetActualizarBoton(VentasBtnAnteriorResultados, _PaginaResultados > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetActualizarBoton(VentasBtnSiguienteResultados,
                _PaginaResultados < VentasFuncObtenerUltimaPagina(_Resultados.Count), System.Drawing.Color.FromArgb(228, 212, 234));
        }

        private void VentasMetMostrarPaginaFactura()
        {
            VentasDgvDetalleFactura.DataSource = _DetallesFactura.Skip(_PaginaFactura * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            VentasLblPaginaFactura.Text = VentasFuncObtenerTextoPagina(_PaginaFactura, _DetallesFactura.Count);
            VentasMetActualizarBoton(VentasBtnAnteriorFactura, _PaginaFactura > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetActualizarBoton(VentasBtnSiguienteFactura,
                _PaginaFactura < VentasFuncObtenerUltimaPagina(_DetallesFactura.Count), System.Drawing.Color.FromArgb(228, 212, 234));
        }

        private void VentasMetMostrarPaginaPorcentajes()
        {
            VentasDgvPorcentajes.DataSource = _ProductosPorcentaje.Skip(_PaginaPorcentajes * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            VentasLblPaginaPorcentajes.Text = VentasFuncObtenerTextoPagina(_PaginaPorcentajes, _ProductosPorcentaje.Count);
            VentasMetActualizarBoton(VentasBtnAnteriorPorcentajes, _PaginaPorcentajes > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            VentasMetActualizarBoton(VentasBtnSiguientePorcentajes,
                _PaginaPorcentajes < VentasFuncObtenerUltimaPagina(_ProductosPorcentaje.Count), System.Drawing.Color.FromArgb(228, 212, 234));
        }

        private int VentasFuncObtenerUltimaPagina(int TotalRegistros)
        {
            return Math.Max(0, (int)Math.Ceiling(TotalRegistros / (double)_FilasPorPagina) - 1);
        }

        private void VentasMetMostrarError(Label EtiquetaEstado, string Mensaje, params Control[] Controles)
        {
            EtiquetaEstado.ForeColor = System.Drawing.Color.Firebrick;
            EtiquetaEstado.Text = Mensaje;
            foreach (Control ControlValidado in Controles)
                ControlValidado.BackColor = System.Drawing.Color.MistyRose;
        }

        private void VentasMetActualizarBoton(Button Boton, bool Habilitado, System.Drawing.Color ColorActivo)
        {
            Boton.Enabled = Habilitado;
            Boton.BackColor = Habilitado ? ColorActivo : System.Drawing.Color.LightGray;
            Boton.ForeColor = Habilitado ? System.Drawing.Color.FromArgb(59, 29, 74) : System.Drawing.SystemColors.GrayText;
        }

        private void VentasMetRestablecerEstado(Label EtiquetaEstado, params Control[] Controles)
        {
            EtiquetaEstado.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            foreach (Control ControlValidado in Controles)
                ControlValidado.BackColor = System.Drawing.SystemColors.Window;
        }

        private string VentasFuncObtenerTextoPagina(int Pagina, int TotalRegistros)
        {
            int TotalPaginas = TotalRegistros == 0 ? 0 : VentasFuncObtenerUltimaPagina(TotalRegistros) + 1;
            return "Página " + (TotalPaginas == 0 ? 0 : Pagina + 1) + " de " + TotalPaginas + " · " + TotalRegistros + " registros";
        }

        private void VentasMetCambiarPaginaResultados(object Origen, EventArgs Argumentos)
        {
            _PaginaResultados += Origen == VentasBtnAnteriorResultados ? -1 : 1;
            VentasMetMostrarPaginaResultados();
        }

        private void VentasMetCambiarPaginaFactura(object Origen, EventArgs Argumentos)
        {
            _PaginaFactura += Origen == VentasBtnAnteriorFactura ? -1 : 1;
            VentasMetMostrarPaginaFactura();
        }

        private void VentasMetCambiarPaginaPorcentajes(object Origen, EventArgs Argumentos)
        {
            _PaginaPorcentajes += Origen == VentasBtnAnteriorPorcentajes ? -1 : 1;
            VentasMetMostrarPaginaPorcentajes();
        }

        private EnumMetodoComision VentasFuncObtenerMetodo(string Texto)
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
