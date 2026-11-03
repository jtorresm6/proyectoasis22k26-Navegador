using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using CapaControlador_ComisionVendedores.Contratos;
using CapaControlador_ComisionVendedores.Controladores;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Enumeraciones;

namespace ComisionVendedoresVista
{
    public partial class FrmComisionVendedores : Form
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
        private ComboBox ComisionVendedoresCboElementoPorcentaje;
        private bool _CargandoPorcentajes;

        private sealed class ClsOpcionPorcentaje
        {
            public int IdClasificacion { get; set; }
            public string Etiqueta { get; set; }
        }

        public FrmComisionVendedores()
            : this(new ClsControladorComision())
        {
        }

        public FrmComisionVendedores(IControladorComision ControladorComision)
        {
            InitializeComponent();
            ComisionVendedoresBtnAyuda.Click += ComisionVendedoresMetAbrirAyuda;
            ComisionVendedoresCboMetodoFactura.Visible = false;
            ComisionVendedoresLblMetodoFactura.Visible = false;
            _ControladorComision = ControladorComision ?? throw new ArgumentNullException("ControladorComision");
            _Resultados = new List<ClsComisionVendedorResultado>();
            _DetallesFactura = new List<ClsComisionFacturaDetalle>();
            _ProductosPorcentaje = new List<ClsProductoPorcentaje>();
            ComisionVendedoresTbcPestanas.SelectedIndexChanged += ComisionVendedoresMetCambiarPestana;
            ComisionVendedoresTabAsignarFactura.Enter += ComisionVendedoresMetCambiarPestana;
            Load += (Sender, Argumentos) =>
            {
                if (ComisionVendedoresTbcPestanas.SelectedTab == ComisionVendedoresTabAsignarFactura)
                    ComisionVendedoresMetCargarCatalogosClick(Sender, Argumentos);
            };
            ComisionVendedoresCboFactura.SelectedValueChanged += ComisionVendedoresMetCambioSeleccionFactura;
            ComisionVendedoresCboVendedorFactura.SelectedValueChanged += ComisionVendedoresMetCambioSeleccionFactura;
            ComisionVendedoresDgvResultados.CellFormatting += ComisionVendedoresMetFormatearMoneda;
            ComisionVendedoresDgvDetalleFactura.CellFormatting += ComisionVendedoresMetFormatearMoneda;
            ComisionVendedoresDgvDetalleFactura.CurrentCellDirtyStateChanged += (Sender, Argumentos) =>
            {
                if (ComisionVendedoresDgvDetalleFactura.IsCurrentCellDirty)
                    ComisionVendedoresDgvDetalleFactura.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            ComisionVendedoresDgvDetalleFactura.CellValueChanged += ComisionVendedoresMetCambiarMetodoProducto;
            ComisionVendedoresCboAlcance.SelectedIndexChanged += ComisionVendedoresMetCambiarAlcancePorcentaje;
            ComisionVendedoresLblAlcance.Text = "Tipo de porcentaje";
            ComisionVendedoresLblAlcance.Location = new System.Drawing.Point(24, 7);
            ComisionVendedoresCboAlcance.Location = new System.Drawing.Point(24, 25);
            ComisionVendedoresCboAlcance.Size = new System.Drawing.Size(150, 31);
            ComisionVendedoresLblPorcentajeNuevo.AutoSize = false;
            ComisionVendedoresLblPorcentajeNuevo.Location = new System.Drawing.Point(420, 7);
            ComisionVendedoresLblPorcentajeNuevo.Size = new System.Drawing.Size(135, 23);
            ComisionVendedoresTxtPorcentaje.Location = new System.Drawing.Point(420, 25);
            ComisionVendedoresTxtPorcentaje.Size = new System.Drawing.Size(110, 30);
            ComisionVendedoresLblObligatorioPorcentaje.Location = new System.Drawing.Point(557, 7);
            ComisionVendedoresBtnBuscar.Location = new System.Drawing.Point(568, 23);
            ComisionVendedoresBtnBuscar.Size = new System.Drawing.Size(100, 30);
            ComisionVendedoresBtnActualizar.Location = new System.Drawing.Point(670, 23);
            ComisionVendedoresBtnActualizar.Size = new System.Drawing.Size(190, 30);
            ComisionVendedoresLblMantenimientoEstado.AutoSize = false;
            ComisionVendedoresLblMantenimientoEstado.Location = new System.Drawing.Point(24, 68);
            ComisionVendedoresLblMantenimientoEstado.Size = new System.Drawing.Size(930, 28);
            ComisionVendedoresTabMantenimiento.Controls.Add(new Label
            {
                Name = "ComisionVendedoresLblElementoPorcentaje",
                Text = "Elemento específico", Location = new System.Drawing.Point(190, 7),
                Size = new System.Drawing.Size(220, 23), ForeColor = System.Drawing.Color.FromArgb(59, 29, 74)
            });
            ComisionVendedoresCboElementoPorcentaje = new ComboBox
            {
                Name = "ComisionVendedoresCboElementoPorcentaje",
                DropDownStyle = ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(190, 25),
                Size = new System.Drawing.Size(220, 31), Font = ComisionVendedoresCboAlcance.Font
            };
            ComisionVendedoresTabMantenimiento.Controls.Add(ComisionVendedoresCboElementoPorcentaje);
        }

        private void ComisionVendedoresMetCalcularClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblEstado,
                    ComisionVendedoresDtpFechaInicio, ComisionVendedoresDtpFechaFin);
                if (ComisionVendedoresDtpFechaInicio.Value.Date > ComisionVendedoresDtpFechaFin.Value.Date)
                    throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");
                _Resultados = _ControladorComision.ComisionVendedoresMetCalcular(EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado,
                    ComisionVendedoresDtpFechaInicio.Value.Date, ComisionVendedoresDtpFechaFin.Value.Date);
                _PaginaResultados = 0;
                ComisionVendedoresMetMostrarPaginaResultados();
                bool TieneAbonosPagables = _Resultados.Any(Resultado => Resultado.Movimientos != null &&
                    Resultado.Movimientos.Any(Movimiento => Movimiento.IdComisionAbono > 0 && Movimiento.MontoComision > 0m));
                ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnGuardar, TieneAbonosPagables,
                    System.Drawing.Color.FromArgb(201, 162, 74));
                ComisionVendedoresLblEstado.Text = _Resultados.Count == 0
                    ? "No hay comisiones asignadas ni pagos de comisión en el rango seleccionado."
                    : !TieneAbonosPagables
                        ? "Se muestran asignaciones del período; aún no hay abonos de comisión pagables. Registrar pago permanece deshabilitado."
                        : "Cálculo completado. Se muestran comisiones pagables y, cuando aplique, asignaciones sin abono pendiente.";
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnGuardar, false,
                    System.Drawing.Color.FromArgb(201, 162, 74));
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstado, Excepcion.Message,
                    ComisionVendedoresDtpFechaInicio, ComisionVendedoresDtpFechaFin);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetGuardarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                if (_Resultados == null || !_Resultados.Any(Resultado => Resultado.Movimientos != null &&
                    Resultado.Movimientos.Any(Movimiento => Movimiento.IdComisionAbono > 0 && Movimiento.MontoComision > 0m)))
                    throw new InvalidOperationException("No hay abonos de comisión pagables para registrar en el período. Las asignaciones sin cobros se muestran como información y no se pueden pagar todavía.");

                var AbonosPagables = _Resultados
                    .Where(Resultado => Resultado.Movimientos != null)
                    .SelectMany(Resultado => Resultado.Movimientos)
                    .Where(Movimiento => Movimiento.IdComisionAbono > 0 && Movimiento.MontoComision > 0m).ToList();
                string TotalPendiente = String.Join(" · ", AbonosPagables
                    .GroupBy(Movimiento => new { Movimiento.IdMoneda, Movimiento.SimboloMoneda })
                    .Select(Grupo => (Grupo.Key.SimboloMoneda ?? String.Empty) + " " +
                        Grupo.Sum(Movimiento => Movimiento.MontoComision).ToString("N2", CultureInfo.GetCultureInfo("es-GT"))));
                DialogResult Confirmacion = MessageBox.Show(this,
                    "Se registrarán como pagados " + AbonosPagables.Count +
                    " abono(s) de comisión por un total de " + TotalPendiente +
                    ". ¿Desea continuar?",
                    "Registrar pago de comisiones", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Confirmacion != DialogResult.Yes) return;

                int Registrados = _ControladorComision.ComisionVendedoresMetGuardar(_Resultados,
                    ComisionVendedoresDtpFechaInicio.Value.Date, ComisionVendedoresDtpFechaFin.Value.Date,
                    EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado);
                ComisionVendedoresLblEstado.Text = Registrados + " abono(s) de comisión marcados como pagados.";
                ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnGuardar, false,
                    System.Drawing.Color.FromArgb(201, 162, 74));
                _Resultados = new List<ClsComisionVendedorResultado>();
                ComisionVendedoresMetMostrarPaginaResultados();
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstado, Excepcion.Message,
                    ComisionVendedoresDtpFechaInicio, ComisionVendedoresDtpFechaFin);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetRegistrarAbonoFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                int IdUsuario = ComisionVendedoresMetObtenerIdUsuarioSesion();
                using (FrmPagoFacturaTemporal Formulario = new FrmPagoFacturaTemporal(_ControladorComision, IdUsuario))
                {
                    if (Formulario.ShowDialog(this) == DialogResult.OK)
                    {
                        ComisionVendedoresLblEstadoFactura.Text =
                            "Abono de cliente registrado. La comisión proporcional queda devengada y aparecerá al calcular comisiones.";
                        ComisionVendedoresMetCargarCatalogosClick(Sender, Argumentos);
                    }
                }
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresLblEstadoFactura.Text = Excepcion.Message;
                MessageBox.Show(this, Excepcion.Message, "Registrar abono de factura",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int ComisionVendedoresMetObtenerIdUsuarioSesion()
        {
            const string NombreTipo = "CapaControlador_Seguridad.Objetos_de_valor.ClsSesionSeguridad";
            Type TipoSesion = AppDomain.CurrentDomain.GetAssemblies()
                .Select(Ensamblado => Ensamblado.GetType(NombreTipo, false))
                .FirstOrDefault(Tipo => Tipo != null);
            if (TipoSesion == null)
            {
                try
                {
                    TipoSesion = Assembly.Load("CapaControlador_Seguridad").GetType(NombreTipo, false);
                }
                catch { }
            }
            PropertyInfo Propiedad = TipoSesion == null ? null : TipoSesion.GetProperty(
                "IdUsuario", BindingFlags.Public | BindingFlags.Static);
            if (Propiedad == null)
                throw new InvalidOperationException("No se encontró el usuario de la sesión. Inicie sesión nuevamente para registrar el abono.");
            int IdUsuario = Convert.ToInt32(Propiedad.GetValue(null, null));
            if (IdUsuario <= 0)
                throw new InvalidOperationException("La sesión no tiene un usuario válido. Inicie sesión nuevamente para registrar el abono.");
            return IdUsuario;
        }

        private void ComisionVendedoresMetBuscarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblMantenimientoEstado, ComisionVendedoresCboAlcance);
                List<ClsProductoPorcentaje> Productos = ComisionVendedoresMetCargarPorcentajes();
                ComisionVendedoresLblMantenimientoEstado.Text = Productos.Count + " producto(s) cargados. Elija el elemento específico que desea modificar.";
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado, Excepcion.Message, ComisionVendedoresCboAlcance);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetCargarCatalogosClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                _CargandoCatalogosFactura = true;
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblEstadoFactura,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                ComisionVendedoresCboFactura.DataSource = null;
                ComisionVendedoresCboVendedorFactura.DataSource = null;
                List<ClsFacturaComisionVista> Facturas = _ControladorComision.ComisionVendedoresMetObtenerFacturas();
                List<ClsVendedorComisionVista> Vendedores = _ControladorComision.ComisionVendedoresMetObtenerVendedores();
                // Usa un campo simple del DTO para asegurar que WinForms muestre el elemento
                // aunque falle el enlace de la propiedad calculada Etiqueta.
                ComisionVendedoresCboFactura.DisplayMember = "IdFactura";
                ComisionVendedoresCboFactura.ValueMember = "IdFactura";
                ComisionVendedoresCboFactura.DataSource = Facturas;
                if (Facturas.Count > 0 && ComisionVendedoresCboFactura.SelectedIndex < 0)
                    ComisionVendedoresCboFactura.SelectedIndex = 0;
                ComisionVendedoresCboVendedorFactura.DisplayMember = "NombreVendedor";
                ComisionVendedoresCboVendedorFactura.ValueMember = "IdVendedor";
                ComisionVendedoresCboVendedorFactura.DataSource = Vendedores;
                ClsFacturaComisionVista FacturaSeleccionada = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (FacturaSeleccionada != null && FacturaSeleccionada.IdVendedor > 0 &&
                    ComisionVendedoresCboVendedorFactura.Items.Cast<ClsVendedorComisionVista>().Any(Vendedor => Vendedor.IdVendedor == FacturaSeleccionada.IdVendedor))
                    ComisionVendedoresCboVendedorFactura.SelectedValue = FacturaSeleccionada.IdVendedor;
                else
                    ComisionVendedoresCboVendedorFactura.SelectedIndex = -1;
                ComisionVendedoresLblEstadoFactura.Text = Facturas.Count == 0
                    ? "No hay facturas pendientes de asignación (puede que no existan facturas emitidas nuevas o que ya tengan comisión registrada)."
                    : Facturas.Count + " factura(s) y " + Vendedores.Count + " vendedor(es) cargados. Seleccione una factura.";
                _CargandoCatalogosFactura = false;
                if (Facturas.Count == 0)
                {
                    ComisionVendedoresDgvDetalleFactura.DataSource = null;
                    _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                    return;
                }
                ComisionVendedoresMetCalcularFacturaSeleccionada();
            }
            catch (Exception Excepcion)
            {
                _CargandoCatalogosFactura = false;
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstadoFactura, Excepcion.Message,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetCalcularFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblEstadoFactura,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                if (ComisionVendedoresCboFactura.SelectedValue == null || ComisionVendedoresCboVendedorFactura.SelectedValue == null)
                {
                    ComisionVendedoresLblEstadoFactura.ForeColor = System.Drawing.Color.Firebrick;
                    ComisionVendedoresLblEstadoFactura.Text = "No se puede calcular: seleccione una factura y un vendedor. Si el combo de factura está vacío, revise el error de carga del catálogo.";
                    return;
                }

                int IdFactura = Convert.ToInt32(ComisionVendedoresCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(ComisionVendedoresCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                EnumMetodoComision Metodo = EnumMetodoComision.Producto;
                List<ClsComisionFacturaDetalle> DetalleFacturaCalculado;
                if (_DetallesFactura.Count > 0 && _IdFacturaVistaPrevia == IdFactura)
                {
                    List<ClsComisionFacturaDetalle> Selecciones = ComisionVendedoresMetObtenerSeleccionesMetodo();
                    DetalleFacturaCalculado = _ControladorComision.ComisionVendedoresMetCalcularFactura(IdFactura, IdVendedor, Selecciones);
                }
                else
                    DetalleFacturaCalculado = _ControladorComision.ComisionVendedoresMetCalcularFactura(IdFactura, IdVendedor, Metodo);
                ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                string SimboloMoneda = Factura == null ? String.Empty : Factura.SimboloMoneda;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    // La columna ComboBox compara valores exactos. Las tasas históricas
                    // pueden venir como PRODUCTO/LINEA en mayúscula desde MySQL.
                    Detalle.MetodoComision = ComisionVendedoresMetNormalizarMetodo(Detalle.MetodoComision);
                    Detalle.SimboloMoneda = SimboloMoneda;
                }
                ComisionVendedoresDgvDetalleFactura.DataSource = null;
                _DetallesFactura = DetalleFacturaCalculado;
                _IdFacturaVistaPrevia = IdFactura;
                _PaginaFactura = 0;
                ComisionVendedoresMetMostrarPaginaFactura();

                decimal TotalComision = 0m, Disponible = 0m, Pendiente = 0m, ComisionUltimoAbono = 0m, TotalBase = 0m;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    TotalBase += Detalle.BaseVenta;
                    TotalComision += Detalle.ComisionTotal;
                    Disponible += Detalle.ComisionDisponible;
                    Pendiente += Detalle.ComisionPendiente;
                    ComisionUltimoAbono += Detalle.ComisionUltimoAbono;
                }
                ClsVendedorComisionVista Vendedor = ComisionVendedoresCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal PagadoFactura = Factura == null ? 0m : Factura.TotalPagado;
                decimal PorcentajeTotal = TotalBase <= 0m ? 0m : TotalComision / TotalBase * 100m;
                ComisionVendedoresLblEstadoFactura.Text = "Factura " + IdFactura + " · " + (Vendedor == null ? "" : Vendedor.NombreVendedor) +
                    " · Cobrado " + SimboloMoneda + " " + PagadoFactura.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + Environment.NewLine +
                    "Total comisión: " + PorcentajeTotal.ToString("0.00") + "% · " + SimboloMoneda + " " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Disponible " + SimboloMoneda + " " + Disponible.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Comisión del último abono " + SimboloMoneda + " " + ComisionUltimoAbono.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Pendiente " + SimboloMoneda + " " + Pendiente.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            }
            catch (Exception Excepcion)
            {
                string MensajeError = ComisionVendedoresMetObtenerDetalleError(Excepcion);
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstadoFactura, MensajeError,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                MessageBox.Show(MensajeError, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetAsignarVendedorFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                if (ComisionVendedoresCboFactura.SelectedValue == null || ComisionVendedoresCboVendedorFactura.SelectedValue == null)
                    throw new ArgumentException("Seleccione una factura y un vendedor.");
                int IdFactura = Convert.ToInt32(ComisionVendedoresCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(ComisionVendedoresCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                List<ClsComisionFacturaDetalle> Selecciones = ComisionVendedoresMetObtenerSeleccionesMetodo();
                int IdComision = _ControladorComision.ComisionVendedoresMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Selecciones);
                ClsVendedorComisionVista Vendedor = ComisionVendedoresCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal TotalComisionGuardada = Selecciones.Sum(Detalle => Detalle.ComisionTotal);
                ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                string SimboloMoneda = Factura == null ? String.Empty : Factura.SimboloMoneda;
                string Confirmacion = "Factura " + IdFactura + " asignada a " +
                    (Vendedor == null ? "vendedor" : Vendedor.NombreVendedor) + ". Se guardaron " +
                    Selecciones.Count + " líneas de comisión por un total de " + SimboloMoneda + " " +
                    TotalComisionGuardada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " (registro " + IdComision + "). La factura se quitó de la lista para evitar asignarla otra vez.";
                ComisionVendedoresMetActualizarFacturasDisponiblesDespuesAsignar(IdFactura, Confirmacion);
                MessageBox.Show(this, Confirmacion, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Excepcion)
            {
                string MensajeError = ComisionVendedoresMetObtenerDetalleError(Excepcion);
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblEstadoFactura, MensajeError,
                    ComisionVendedoresCboFactura, ComisionVendedoresCboVendedorFactura);
                MessageBox.Show(MensajeError, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComisionVendedoresMetActualizarFacturasDisponiblesDespuesAsignar(int IdFactura, string Confirmacion)
        {
            try
            {
                List<ClsFacturaComisionVista> FacturasDisponibles = _ControladorComision.ComisionVendedoresMetObtenerFacturas();
                _CargandoCatalogosFactura = true;
                ComisionVendedoresCboFactura.DataSource = null;
                ComisionVendedoresCboFactura.DisplayMember = "IdFactura";
                ComisionVendedoresCboFactura.ValueMember = "IdFactura";
                ComisionVendedoresCboFactura.DataSource = FacturasDisponibles;
                ComisionVendedoresCboFactura.SelectedIndex = -1;
                ComisionVendedoresCboVendedorFactura.SelectedIndex = -1;
                _CargandoCatalogosFactura = false;
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                _IdFacturaVistaPrevia = 0;
                _PaginaFactura = 0;
                ComisionVendedoresDgvDetalleFactura.DataSource = null;
                ComisionVendedoresMetMostrarPaginaFactura();
                ComisionVendedoresLblEstadoFactura.Text = Confirmacion + Environment.NewLine +
                    (FacturasDisponibles.Count == 0
                        ? "No quedan facturas pendientes de asignación."
                        : "Quedan " + FacturasDisponibles.Count + " factura(s) pendientes de asignación.");
            }
            catch (Exception ErrorActualizacion)
            {
                _CargandoCatalogosFactura = false;
                ComisionVendedoresLblEstadoFactura.Text = Confirmacion + Environment.NewLine +
                    "La asignación se guardó, pero no se pudo actualizar la lista de facturas: " +
                    ComisionVendedoresMetObtenerDetalleError(ErrorActualizacion);
            }
        }

        private void ComisionVendedoresMetCambiarPestana(object Sender, EventArgs Argumentos)
        {
            if (ComisionVendedoresTbcPestanas.SelectedTab == ComisionVendedoresTabAsignarFactura)
                ComisionVendedoresMetCargarCatalogosClick(Sender, Argumentos);
            else if (ComisionVendedoresTbcPestanas.SelectedTab == ComisionVendedoresTabMantenimiento)
                ComisionVendedoresMetBuscarClick(Sender, Argumentos);
        }

        private void ComisionVendedoresMetCambioSeleccionFactura(object Sender, EventArgs Argumentos)
        {
            if (Sender == ComisionVendedoresCboFactura)
            {
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                _IdFacturaVistaPrevia = 0;
            }
            if (!_CargandoCatalogosFactura && Sender == ComisionVendedoresCboFactura)
            {
                ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (Factura != null && Factura.IdVendedor > 0 &&
                    ComisionVendedoresCboVendedorFactura.Items.Cast<ClsVendedorComisionVista>().Any(Vendedor => Vendedor.IdVendedor == Factura.IdVendedor))
                    ComisionVendedoresCboVendedorFactura.SelectedValue = Factura.IdVendedor;
                else
                    ComisionVendedoresCboVendedorFactura.SelectedIndex = -1;
            }
            ComisionVendedoresMetCalcularFacturaSeleccionada();
        }

        private void ComisionVendedoresMetCalcularFacturaSeleccionada()
        {
            if (_CargandoCatalogosFactura) return;
            if (ComisionVendedoresCboFactura.SelectedValue == null ||
                ComisionVendedoresCboVendedorFactura.SelectedValue == null)
            {
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                ComisionVendedoresDgvDetalleFactura.DataSource = null;
                ComisionVendedoresMetMostrarPaginaFactura();
                ComisionVendedoresLblEstadoFactura.Text = "Seleccione una factura con vendedor asignado o seleccione un vendedor.";
                return;
            }
            ClsFacturaComisionVista FacturaSeleccionada = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
            int IdVendedorSeleccionado = Convert.ToInt32(ComisionVendedoresCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
            if (FacturaSeleccionada != null && FacturaSeleccionada.IdVendedor != IdVendedorSeleccionado)
            {
                // Cambiar el vendedor no debe borrar los renglones ni las tasas
                // seleccionadas para la factura actual.
                if (_IdFacturaVistaPrevia != FacturaSeleccionada.IdFactura || _DetallesFactura.Count == 0)
                    ComisionVendedoresMetCalcularFacturaClick(ComisionVendedoresBtnCalcularFactura, EventArgs.Empty);
                else
                    ComisionVendedoresLblEstadoFactura.Text = "Vendedor seleccionado: " +
                        ComisionVendedoresCboVendedorFactura.Text +
                        ". Revise los métodos por producto y pulse «Asignar vendedor» para guardar.";
                return;
            }
            ComisionVendedoresMetCalcularFacturaClick(ComisionVendedoresBtnCalcularFactura, EventArgs.Empty);
        }

        private List<ClsComisionFacturaDetalle> ComisionVendedoresMetObtenerSeleccionesMetodo()
        {
            ComisionVendedoresDgvDetalleFactura.EndEdit();
            foreach (DataGridViewRow Fila in ComisionVendedoresDgvDetalleFactura.Rows)
            {
                ClsComisionFacturaDetalle Detalle = Fila.DataBoundItem as ClsComisionFacturaDetalle;
                if (Detalle == null) continue;
                object Valor = Fila.Cells["ComisionVendedoresClmMetodoProducto"].Value;
                if (Detalle.ComisionGuardada && String.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                    Valor = Detalle.MetodoComision;
                if (Valor == null || String.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                    throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto en cada renglón de la factura.");
                Detalle.MetodoComision = ComisionVendedoresMetNormalizarMetodo(Convert.ToString(Valor));
                if (String.IsNullOrWhiteSpace(Detalle.MetodoComision))
                    throw new InvalidOperationException("Método no válido en la línea " + Detalle.IdFacturaDetalle + ": «" + Convert.ToString(Valor) + "».");
            }
            List<ClsComisionFacturaDetalle> Selecciones = _DetallesFactura;
            if (Selecciones.Any(Detalle => String.IsNullOrWhiteSpace(Detalle.MetodoComision)))
                throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto para cada producto. Use la paginación para revisar todos.");
            if (Selecciones.Count == 0) throw new InvalidOperationException("La factura no tiene productos cargados.");
            return Selecciones;
        }

        private void ComisionVendedoresMetActualizarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                ComisionVendedoresMetRestablecerEstado(ComisionVendedoresLblMantenimientoEstado, ComisionVendedoresTxtPorcentaje);
                decimal Porcentaje;
                if (!decimal.TryParse(ComisionVendedoresTxtPorcentaje.Text, NumberStyles.Number,
                    CultureInfo.GetCultureInfo("es-GT"), out Porcentaje))
                {
                    ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado,
                        "Ingrese un porcentaje válido entre 0.00 y 99.00.", ComisionVendedoresTxtPorcentaje);
                    throw new ArgumentException("Ingrese un porcentaje válido entre 0.00 y 99.00.");
                }
                if (Porcentaje < 0m || Porcentaje > 99m)
                    throw new ArgumentOutOfRangeException("Porcentaje", "El esquema permite porcentajes entre 0.00 y 99.00.");
                ClsOpcionPorcentaje Opcion = ComisionVendedoresCboElementoPorcentaje.SelectedItem as ClsOpcionPorcentaje;
                if (Opcion == null) throw new ArgumentException("Seleccione la línea, producto, categoría o marca que desea modificar.");
                EnumMetodoComision Metodo = ComisionVendedoresMetObtenerMetodo(ComisionVendedoresCboAlcance.SelectedItem.ToString());
                int Actualizados = _ControladorComision.ComisionVendedoresMetActualizarPorcentaje(Metodo,
                    Opcion.IdClasificacion, Porcentaje);

                _ProductosPorcentaje = _ControladorComision.ComisionVendedoresMetConsultarPorcentajes(Metodo);
                _PaginaPorcentajes = Math.Min(_PaginaPorcentajes, ComisionVendedoresMetObtenerUltimaPagina(_ProductosPorcentaje.Count));
                ComisionVendedoresMetMostrarPaginaPorcentajes();
                ComisionVendedoresLblMantenimientoEstado.Text = Actualizados + " registro(s) de " + Opcion.Etiqueta +
                    " guardado(s) con " + Porcentaje.ToString("0.00") + "%.";
                ComisionVendedoresMetCargarElementosPorcentaje();
            }
            catch (Exception Excepcion)
            {
                ComisionVendedoresMetMostrarError(ComisionVendedoresLblMantenimientoEstado, Excepcion.Message);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ClsProductoPorcentaje> ComisionVendedoresMetCargarPorcentajes()
        {
            _ProductosPorcentaje = _ControladorComision.ComisionVendedoresMetConsultarPorcentajes(
                ComisionVendedoresMetObtenerMetodo(ComisionVendedoresCboAlcance.SelectedItem.ToString()));
            _PaginaPorcentajes = 0;
            ComisionVendedoresMetMostrarPaginaPorcentajes();
            ComisionVendedoresMetCargarElementosPorcentaje();
            return _ProductosPorcentaje;
        }

        private void ComisionVendedoresMetCambiarAlcancePorcentaje(object Sender, EventArgs Argumentos)
        {
            if (_CargandoPorcentajes || _ProductosPorcentaje == null) return;
            ComisionVendedoresCboElementoPorcentaje.DataSource = null;
            ComisionVendedoresMetCargarElementosPorcentaje();
        }

        private void ComisionVendedoresMetCargarElementosPorcentaje()
        {
            if (ComisionVendedoresCboElementoPorcentaje == null || ComisionVendedoresCboAlcance.SelectedItem == null) return;
            EnumMetodoComision Metodo = ComisionVendedoresMetObtenerMetodo(ComisionVendedoresCboAlcance.SelectedItem.ToString());
            IEnumerable<ClsOpcionPorcentaje> Opciones;
            switch (Metodo)
            {
                case EnumMetodoComision.Producto:
                    Opciones = _ProductosPorcentaje.Select(Producto => new ClsOpcionPorcentaje
                    { IdClasificacion = Producto.IdProducto, Etiqueta = Producto.NombreProducto });
                    break;
                case EnumMetodoComision.Marca:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdMarca > 0).GroupBy(Producto => Producto.IdMarca)
                        .Select(Grupo => new ClsOpcionPorcentaje { IdClasificacion = Grupo.Key,
                            Etiqueta = Grupo.First().Marca + " · " + Grupo.Count() + " producto(s)" });
                    break;
                case EnumMetodoComision.Linea:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdLinea > 0).GroupBy(Producto => Producto.IdLinea)
                        .Select(Grupo => new ClsOpcionPorcentaje { IdClasificacion = Grupo.Key,
                            Etiqueta = Grupo.First().Linea + " · " + Grupo.Count() + " producto(s)" });
                    break;
                case EnumMetodoComision.Categoria:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdCategoria > 0).GroupBy(Producto => Producto.IdCategoria)
                        .Select(Grupo => new ClsOpcionPorcentaje { IdClasificacion = Grupo.Key,
                            Etiqueta = Grupo.First().Categoria + " · " + Grupo.Count() + " producto(s)" });
                    break;
                default: Opciones = Enumerable.Empty<ClsOpcionPorcentaje>(); break;
            }
            _CargandoPorcentajes = true;
            try
            {
                int IdSeleccionado = (ComisionVendedoresCboElementoPorcentaje.SelectedItem as ClsOpcionPorcentaje)?.IdClasificacion ?? 0;
                List<ClsOpcionPorcentaje> Lista = Opciones.OrderBy(Opcion => Opcion.Etiqueta).ToList();
                ComisionVendedoresCboElementoPorcentaje.DisplayMember = "Etiqueta";
                ComisionVendedoresCboElementoPorcentaje.ValueMember = "IdClasificacion";
                ComisionVendedoresCboElementoPorcentaje.DataSource = Lista;
                ComisionVendedoresCboElementoPorcentaje.SelectedIndex = Lista.FindIndex(Opcion => Opcion.IdClasificacion == IdSeleccionado);
            }
            finally { _CargandoPorcentajes = false; }
        }

        private void ComisionVendedoresMetMostrarPaginaResultados()
        {
            ComisionVendedoresDgvResultados.DataSource = _Resultados.Skip(_PaginaResultados * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            ComisionVendedoresLblPaginaResultados.Text = ComisionVendedoresMetObtenerTextoPagina(_PaginaResultados, _Resultados.Count);
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnAnteriorResultados, _PaginaResultados > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnSiguienteResultados,
                _PaginaResultados < ComisionVendedoresMetObtenerUltimaPagina(_Resultados.Count), System.Drawing.Color.FromArgb(228, 212, 234));
        }

        private void ComisionVendedoresMetFormatearMoneda(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            DataGridView Tabla = Sender as DataGridView;
            if (Tabla == null || Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 || Argumentos.Value == null)
                return;

            string Propiedad = Tabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (Propiedad != "BaseVentas" && Propiedad != "MontoComision" &&
                Propiedad != "BaseVenta" && Propiedad != "ComisionTotal" &&
                Propiedad != "VentaCobrado" && Propiedad != "ComisionUltimoAbono" &&
                Propiedad != "ComisionDisponible" && Propiedad != "ComisionPendiente")
                return;

            string SimboloMoneda = String.Empty;
            if (Tabla == ComisionVendedoresDgvResultados)
            {
                ClsComisionVendedorResultado Resultado = Tabla.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionVendedorResultado;
                if (Resultado != null) SimboloMoneda = Resultado.SimboloMoneda;
            }
            else
            {
                ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (Factura != null) SimboloMoneda = Factura.SimboloMoneda;
            }

            Argumentos.Value = (SimboloMoneda ?? String.Empty) + " " +
                Convert.ToDecimal(Argumentos.Value).ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            Argumentos.FormattingApplied = true;
        }

        private void ComisionVendedoresMetMostrarPaginaFactura()
        {
            ComisionVendedoresDgvDetalleFactura.DataSource = _DetallesFactura.Skip(_PaginaFactura * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            if (ComisionVendedoresDgvDetalleFactura.Columns.Contains("ComisionVendedoresClmMetodoProducto"))
                ComisionVendedoresDgvDetalleFactura.Columns["ComisionVendedoresClmMetodoProducto"].DataPropertyName = "MetodoComision";
            ComisionVendedoresLblPaginaFactura.Text = ComisionVendedoresMetObtenerTextoPagina(_PaginaFactura, _DetallesFactura.Count);
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnAnteriorFactura, _PaginaFactura > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnSiguienteFactura,
                _PaginaFactura < ComisionVendedoresMetObtenerUltimaPagina(_DetallesFactura.Count), System.Drawing.Color.FromArgb(228, 212, 234));
        }

        private void ComisionVendedoresMetCambiarMetodoProducto(object Sender, DataGridViewCellEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 ||
                ComisionVendedoresDgvDetalleFactura.Columns[Argumentos.ColumnIndex].Name != "ComisionVendedoresClmMetodoProducto") return;
            ClsComisionFacturaDetalle Detalle = ComisionVendedoresDgvDetalleFactura.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionFacturaDetalle;
            if (Detalle == null || Detalle.ComisionGuardada) return;
            string Metodo = ComisionVendedoresMetNormalizarMetodo(Convert.ToString(
                ComisionVendedoresDgvDetalleFactura.Rows[Argumentos.RowIndex].Cells[Argumentos.ColumnIndex].Value));
            if (String.IsNullOrWhiteSpace(Metodo)) return;
            decimal Porcentaje = Metodo == "Producto" ? Detalle.PorcentajeProducto : Metodo == "Marca" ? Detalle.PorcentajeMarca : Metodo == "Linea" ? Detalle.PorcentajeLinea : Detalle.PorcentajeCategoria;
            Detalle.MetodoComision = Metodo;
            Detalle.Porcentaje = Porcentaje;
            Detalle.ComisionTotal = Math.Round(Detalle.BaseVenta * Porcentaje / 100m, 4, MidpointRounding.AwayFromZero);
            Detalle.ComisionPendiente = Math.Max(0m, Detalle.ComisionTotal - Detalle.ComisionDisponible);
            decimal TotalComision = _DetallesFactura.Sum(Item => Item.ComisionTotal);
            decimal PorcentajePonderado = _DetallesFactura.Sum(Item => Item.BaseVenta) <= 0m ? 0m : TotalComision / _DetallesFactura.Sum(Item => Item.BaseVenta) * 100m;
            ClsFacturaComisionVista Factura = ComisionVendedoresCboFactura.SelectedItem as ClsFacturaComisionVista;
            ComisionVendedoresLblEstadoFactura.Text = "Total comisión: " + PorcentajePonderado.ToString("0.00") + "% · " + (Factura == null ? String.Empty : Factura.SimboloMoneda) + " " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            ComisionVendedoresDgvDetalleFactura.Refresh();
        }

        private void ComisionVendedoresMetAbrirAyuda(object Sender, EventArgs Argumentos)
        {
            string RutaAyuda = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Ayudas", "ComisionVendedoresAyudas.chm");
            if (File.Exists(RutaAyuda))
            {
                Help.ShowHelp(this, RutaAyuda);
                return;
            }
            string RutaHtml = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Ayudas", "FrmComisionVendedores_ComisionVendedores.htm");
            if (File.Exists(RutaHtml)) Process.Start(new ProcessStartInfo(RutaHtml) { UseShellExecute = true });
            else MessageBox.Show(this, "No se encontró el archivo de ayuda del módulo.", "Ayuda ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComisionVendedoresMetFormatearMetodoProducto(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 ||
                ComisionVendedoresDgvDetalleFactura.Columns[Argumentos.ColumnIndex].Name != "ComisionVendedoresClmDetallePorcentaje") return;
            ClsComisionFacturaDetalle Detalle = ComisionVendedoresDgvDetalleFactura.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionFacturaDetalle;
            if (Detalle != null)
            {
                Argumentos.Value = Detalle.Porcentaje.ToString("0.00") + "%";
                Argumentos.FormattingApplied = true;
            }
        }

        private void ComisionVendedoresMetMostrarPaginaPorcentajes()
        {
            ComisionVendedoresDgvPorcentajes.DataSource = _ProductosPorcentaje.Skip(_PaginaPorcentajes * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            ComisionVendedoresLblPaginaPorcentajes.Text = ComisionVendedoresMetObtenerTextoPagina(_PaginaPorcentajes, _ProductosPorcentaje.Count);
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnAnteriorPorcentajes, _PaginaPorcentajes > 0,
                System.Drawing.Color.FromArgb(228, 212, 234));
            ComisionVendedoresMetActualizarBoton(ComisionVendedoresBtnSiguientePorcentajes,
                _PaginaPorcentajes < ComisionVendedoresMetObtenerUltimaPagina(_ProductosPorcentaje.Count), System.Drawing.Color.FromArgb(228, 212, 234));
        }

        private int ComisionVendedoresMetObtenerUltimaPagina(int TotalRegistros)
        {
            return Math.Max(0, (int)Math.Ceiling(TotalRegistros / (double)_FilasPorPagina) - 1);
        }

        private void ComisionVendedoresMetMostrarError(Label EtiquetaEstado, string Mensaje, params Control[] Controles)
        {
            EtiquetaEstado.ForeColor = System.Drawing.Color.Firebrick;
            EtiquetaEstado.Text = Mensaje;
            foreach (Control ControlValidado in Controles)
                ControlValidado.BackColor = System.Drawing.Color.MistyRose;
        }

        private string ComisionVendedoresMetObtenerDetalleError(Exception Excepcion)
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

        private static string ComisionVendedoresMetNormalizarMetodo(string Metodo)
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

        private void ComisionVendedoresMetActualizarBoton(Button Boton, bool Habilitado, System.Drawing.Color ColorActivo)
        {
            Boton.Enabled = Habilitado;
            Boton.BackColor = Habilitado ? ColorActivo : System.Drawing.Color.LightGray;
            Boton.ForeColor = Habilitado ? System.Drawing.Color.FromArgb(59, 29, 74) : System.Drawing.SystemColors.GrayText;
        }

        private void ComisionVendedoresMetRestablecerEstado(Label EtiquetaEstado, params Control[] Controles)
        {
            EtiquetaEstado.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            foreach (Control ControlValidado in Controles)
                ControlValidado.BackColor = System.Drawing.SystemColors.Window;
        }

        private string ComisionVendedoresMetObtenerTextoPagina(int Pagina, int TotalRegistros)
        {
            int TotalPaginas = TotalRegistros == 0 ? 0 : ComisionVendedoresMetObtenerUltimaPagina(TotalRegistros) + 1;
            return "Página " + (TotalPaginas == 0 ? 0 : Pagina + 1) + " de " + TotalPaginas + " · " + TotalRegistros + " registros";
        }

        private void ComisionVendedoresMetCambiarPaginaResultados(object Sender, EventArgs Argumentos)
        {
            _PaginaResultados += Sender == ComisionVendedoresBtnAnteriorResultados ? -1 : 1;
            ComisionVendedoresMetMostrarPaginaResultados();
        }

        private void ComisionVendedoresMetCambiarPaginaFactura(object Sender, EventArgs Argumentos)
        {
            _PaginaFactura += Sender == ComisionVendedoresBtnAnteriorFactura ? -1 : 1;
            ComisionVendedoresMetMostrarPaginaFactura();
        }

        private void ComisionVendedoresMetCambiarPaginaPorcentajes(object Sender, EventArgs Argumentos)
        {
            _PaginaPorcentajes += Sender == ComisionVendedoresBtnAnteriorPorcentajes ? -1 : 1;
            ComisionVendedoresMetMostrarPaginaPorcentajes();
        }

        private EnumMetodoComision ComisionVendedoresMetObtenerMetodo(string Texto)
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
