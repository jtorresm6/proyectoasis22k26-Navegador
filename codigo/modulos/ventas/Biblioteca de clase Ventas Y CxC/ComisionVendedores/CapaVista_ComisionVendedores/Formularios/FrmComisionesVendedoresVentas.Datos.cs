// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Gestiona los catálogos, la asignación de facturas y la presentación de resultados.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CapaControlador_ComisionVendedores.Contratos;
using CapaControlador_ComisionVendedores.Controladores;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Enumeraciones;
using CapaControlador_ComisionVendedores.Validaciones;

namespace CapaVista_ComisionVendedores
{
    public partial class FrmComisionesVendedoresVentas
    {
        private void VentasMetRegistrarAbonoFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                int IdUsuario = VentasMetObtenerIdUsuarioSesion();
                ClsFacturaComisionVista FacturaSeleccionada = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                using (FrmPagoFacturaTemporal Formulario = new FrmPagoFacturaTemporal(_ControladorComision, IdUsuario,
                    FacturaSeleccionada == null ? (int?)null : FacturaSeleccionada.IdFactura))
                {
                    if (Formulario.ShowDialog(this) == DialogResult.OK)
                    {
                        VentasLblEstadoFactura.Text =
                            "Abono de cliente registrado. La comisión proporcional queda devengada y aparecerá al calcular comisiones.";
                        VentasMetCargarCatalogosClick(Sender, Argumentos);
                    }
                }
            }
            catch (Exception Excepcion)
            {
                VentasLblEstadoFactura.Text = Excepcion.Message;
                MessageBox.Show(this, Excepcion.Message, "Registrar abono de factura",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int VentasMetObtenerIdUsuarioSesion()
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

        private void VentasMetCargarCatalogosClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                _CargandoCatalogosFactura = true;
                VentasMetRestablecerEstado(VentasLblEstadoFactura,
                    VentasCboFactura, VentasCboVendedorFactura);
                VentasCboFactura.DataSource = null;
                VentasCboVendedorFactura.DataSource = null;
                List<ClsFacturaComisionVista> Facturas = _ControladorComision.VentasMetObtenerFacturas();
                List<ClsVendedorComisionVista> Vendedores = _ControladorComision.VentasMetObtenerVendedores();
                // Usa un campo simple del DTO para asegurar que WinForms muestre el elemento
                // aunque falle el enlace de la propiedad calculada Etiqueta.
                VentasCboFactura.DisplayMember = "IdFactura";
                VentasCboFactura.ValueMember = "IdFactura";
                VentasCboFactura.DataSource = Facturas;
                if (Facturas.Count > 0 && VentasCboFactura.SelectedIndex < 0)
                    VentasCboFactura.SelectedIndex = 0;
                VentasCboVendedorFactura.DisplayMember = "NombreVendedor";
                VentasCboVendedorFactura.ValueMember = "IdVendedor";
                VentasCboVendedorFactura.DataSource = Vendedores;
                ClsFacturaComisionVista FacturaSeleccionada = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (FacturaSeleccionada != null && FacturaSeleccionada.IdVendedor > 0 &&
                    VentasCboVendedorFactura.Items.Cast<ClsVendedorComisionVista>().Any(Vendedor => Vendedor.IdVendedor == FacturaSeleccionada.IdVendedor))
                    VentasCboVendedorFactura.SelectedValue = FacturaSeleccionada.IdVendedor;
                else
                    VentasCboVendedorFactura.SelectedIndex = -1;
                VentasLblEstadoFactura.Text = Facturas.Count == 0
                    ? "No hay facturas pendientes de asignación (puede que no existan facturas emitidas nuevas o que ya tengan comisión registrada)."
                    : Facturas.Count + " factura(s) y " + Vendedores.Count + " vendedor(es) cargados. Seleccione una factura.";
                _CargandoCatalogosFactura = false;
                if (Facturas.Count == 0)
                {
                    VentasDgvDetalleFactura.DataSource = null;
                    _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                    return;
                }
                VentasMetCalcularFacturaSeleccionada();
            }
            catch (Exception Excepcion)
            {
                _CargandoCatalogosFactura = false;
                VentasMetMostrarError(VentasLblEstadoFactura, Excepcion.Message,
                    VentasCboFactura, VentasCboVendedorFactura);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetCalcularFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstadoFactura,
                    VentasCboFactura, VentasCboVendedorFactura);
                if (VentasCboFactura.SelectedValue == null || VentasCboVendedorFactura.SelectedValue == null)
                {
                    VentasMetMostrarError(VentasLblEstadoFactura,
                        "Seleccione una factura y un vendedor para continuar.", VentasCboFactura, VentasCboVendedorFactura);
                    VentasLblEstadoFactura.ForeColor = System.Drawing.Color.Firebrick;
                    VentasLblEstadoFactura.Text = "No se puede calcular: seleccione una factura y un vendedor. Si el combo de factura está vacío, revise el error de carga del catálogo.";
                    return;
                }

                int IdFactura = Convert.ToInt32(VentasCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(VentasCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                EnumMetodoComision Metodo = EnumMetodoComision.Producto;
                List<ClsComisionFacturaDetalle> DetalleFacturaCalculado;
                if (_DetallesFactura.Count > 0 && _IdFacturaVistaPrevia == IdFactura)
                {
                    List<ClsComisionFacturaDetalle> Selecciones = VentasMetObtenerSeleccionesMetodo();
                    DetalleFacturaCalculado = _ControladorComision.VentasMetCalcularFactura(IdFactura, IdVendedor, Selecciones);
                }
                else
                    DetalleFacturaCalculado = _ControladorComision.VentasMetCalcularFactura(IdFactura, IdVendedor, Metodo);
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                string SimboloMoneda = Factura == null ? String.Empty : Factura.SimboloMoneda;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    // La columna ComboBox compara valores exactos. Las tasas históricas
                    // pueden venir como PRODUCTO/LINEA en mayúscula desde MySQL.
                    Detalle.MetodoComision = VentasMetNormalizarMetodo(Detalle.MetodoComision);
                    Detalle.SimboloMoneda = SimboloMoneda;
                }
                VentasDgvDetalleFactura.DataSource = null;
                _DetallesFactura = DetalleFacturaCalculado;
                _IdFacturaVistaPrevia = IdFactura;
                _PaginaFactura = 0;
                VentasMetMostrarPaginaFactura();

                decimal TotalComision = 0m, Disponible = 0m, Pendiente = 0m, ComisionUltimoAbono = 0m, TotalBase = 0m;
                foreach (ClsComisionFacturaDetalle Detalle in DetalleFacturaCalculado)
                {
                    TotalBase += Detalle.BaseVenta;
                    TotalComision += Detalle.ComisionTotal;
                    Disponible += Detalle.ComisionDisponible;
                    Pendiente += Detalle.ComisionPendiente;
                    ComisionUltimoAbono += Detalle.ComisionUltimoAbono;
                }
                ClsVendedorComisionVista Vendedor = VentasCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal PagadoFactura = Factura == null ? 0m : Factura.TotalPagado;
                decimal PorcentajeTotal = TotalBase <= 0m ? 0m : TotalComision / TotalBase * 100m;
                VentasLblEstadoFactura.Text = "Factura " + IdFactura + " · " + (Vendedor == null ? "" : Vendedor.NombreVendedor) +
                    " · Cobrado " + SimboloMoneda + " " + PagadoFactura.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + Environment.NewLine +
                    "Total comisión: " + PorcentajeTotal.ToString("0.00") + "% · " + SimboloMoneda + " " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Disponible " + SimboloMoneda + " " + Disponible.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Comisión del último abono " + SimboloMoneda + " " + ComisionUltimoAbono.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " · Pendiente " + SimboloMoneda + " " + Pendiente.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            }
            catch (Exception Excepcion)
            {
                string MensajeError = VentasMetObtenerDetalleError(Excepcion);
                VentasMetMostrarError(VentasLblEstadoFactura, MensajeError,
                    VentasCboFactura, VentasCboVendedorFactura);
                MessageBox.Show(MensajeError, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetAsignarVendedorFacturaClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblEstadoFactura,
                    VentasCboFactura, VentasCboVendedorFactura);
                if (VentasCboFactura.SelectedValue == null || VentasCboVendedorFactura.SelectedValue == null)
                {
                    VentasMetMostrarError(VentasLblEstadoFactura,
                        "Seleccione una factura y un vendedor.", VentasCboFactura, VentasCboVendedorFactura);
                    return;
                }
                int IdFactura = Convert.ToInt32(VentasCboFactura.SelectedValue, CultureInfo.InvariantCulture);
                int IdVendedor = Convert.ToInt32(VentasCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
                List<ClsComisionFacturaDetalle> Selecciones = VentasMetObtenerSeleccionesMetodo();
                ClsVendedorComisionVista Vendedor = VentasCboVendedorFactura.SelectedItem as ClsVendedorComisionVista;
                decimal TotalComisionGuardada = Selecciones.Sum(Detalle => Detalle.ComisionTotal);
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                string SimboloMoneda = Factura == null ? String.Empty : Factura.SimboloMoneda;
                string TextoConfirmacion = "Se asignará la factura " + IdFactura + " a " +
                    (Vendedor == null ? "el vendedor seleccionado" : Vendedor.NombreVendedor) + ". Se guardarán " +
                    Selecciones.Count + " líneas de comisión por " + SimboloMoneda + " " +
                    TotalComisionGuardada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ". ¿Desea continuar?";
                if (VentasMetConfirmar(this, "7008 – ComisionVendedores", TextoConfirmacion, "Sí, asignar") != DialogResult.Yes)
                    return;
                int IdComision = _ControladorComision.VentasMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Selecciones);
                string Confirmacion = "Factura " + IdFactura + " asignada a " +
                    (Vendedor == null ? "vendedor" : Vendedor.NombreVendedor) + ". Se guardaron " +
                    Selecciones.Count + " líneas de comisión por un total de " + SimboloMoneda + " " +
                    TotalComisionGuardada.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) +
                    " (registro " + IdComision + "). La factura se quitó de la lista para evitar asignarla otra vez.";
                VentasMetActualizarFacturasDisponiblesDespuesAsignar(IdFactura, Confirmacion);
                VentasMetMostrarExito(this, "7008 – ComisionVendedores", Confirmacion);
            }
            catch (Exception Excepcion)
            {
                string MensajeError = VentasMetObtenerDetalleError(Excepcion);
                VentasMetMostrarError(VentasLblEstadoFactura, MensajeError,
                    VentasCboFactura, VentasCboVendedorFactura);
                MessageBox.Show(MensajeError, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetActualizarFacturasDisponiblesDespuesAsignar(int IdFactura, string Confirmacion)
        {
            try
            {
                List<ClsFacturaComisionVista> FacturasDisponibles = _ControladorComision.VentasMetObtenerFacturas();
                _CargandoCatalogosFactura = true;
                VentasCboFactura.DataSource = null;
                VentasCboFactura.DisplayMember = "IdFactura";
                VentasCboFactura.ValueMember = "IdFactura";
                VentasCboFactura.DataSource = FacturasDisponibles;
                VentasCboFactura.SelectedIndex = -1;
                VentasCboVendedorFactura.SelectedIndex = -1;
                _CargandoCatalogosFactura = false;
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                _IdFacturaVistaPrevia = 0;
                _PaginaFactura = 0;
                VentasDgvDetalleFactura.DataSource = null;
                VentasMetMostrarPaginaFactura();
                VentasLblEstadoFactura.Text = Confirmacion + Environment.NewLine +
                    (FacturasDisponibles.Count == 0
                        ? "No quedan facturas pendientes de asignación."
                        : "Quedan " + FacturasDisponibles.Count + " factura(s) pendientes de asignación.");
            }
            catch (Exception ErrorActualizacion)
            {
                _CargandoCatalogosFactura = false;
                VentasLblEstadoFactura.Text = Confirmacion + Environment.NewLine +
                    "La asignación se guardó, pero no se pudo actualizar la lista de facturas: " +
                    VentasMetObtenerDetalleError(ErrorActualizacion);
            }
        }

        private void VentasMetCambioSeleccionFactura(object Sender, EventArgs Argumentos)
        {
            if (Sender == VentasCboFactura)
            {
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                _IdFacturaVistaPrevia = 0;
            }
            if (!_CargandoCatalogosFactura && Sender == VentasCboFactura)
            {
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (Factura != null && Factura.IdVendedor > 0 &&
                    VentasCboVendedorFactura.Items.Cast<ClsVendedorComisionVista>().Any(Vendedor => Vendedor.IdVendedor == Factura.IdVendedor))
                    VentasCboVendedorFactura.SelectedValue = Factura.IdVendedor;
                else
                    VentasCboVendedorFactura.SelectedIndex = -1;
            }
            VentasMetCalcularFacturaSeleccionada();
        }

        private void VentasMetCalcularFacturaSeleccionada()
        {
            if (_CargandoCatalogosFactura) return;
            if (VentasCboFactura.SelectedValue == null ||
                VentasCboVendedorFactura.SelectedValue == null)
            {
                _DetallesFactura = new List<ClsComisionFacturaDetalle>();
                VentasDgvDetalleFactura.DataSource = null;
                VentasMetMostrarPaginaFactura();
                VentasLblEstadoFactura.Text = "Seleccione una factura con vendedor asignado o seleccione un vendedor.";
                return;
            }
            ClsFacturaComisionVista FacturaSeleccionada = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
            int IdVendedorSeleccionado = Convert.ToInt32(VentasCboVendedorFactura.SelectedValue, CultureInfo.InvariantCulture);
            if (FacturaSeleccionada != null && FacturaSeleccionada.IdVendedor != IdVendedorSeleccionado)
            {
                // Cambiar el vendedor no debe borrar los renglones ni las tasas
                // seleccionadas para la factura actual.
                if (_IdFacturaVistaPrevia != FacturaSeleccionada.IdFactura || _DetallesFactura.Count == 0)
                    VentasMetCalcularFacturaClick(VentasBtnCalcularFactura, EventArgs.Empty);
                else
                    VentasLblEstadoFactura.Text = "Vendedor seleccionado: " +
                        VentasCboVendedorFactura.Text +
                        ". Revise los métodos por producto y pulse «Asignar vendedor» para guardar.";
                return;
            }
            VentasMetCalcularFacturaClick(VentasBtnCalcularFactura, EventArgs.Empty);
        }

        private List<ClsComisionFacturaDetalle> VentasMetObtenerSeleccionesMetodo()
        {
            VentasDgvDetalleFactura.EndEdit();
            foreach (DataGridViewRow Fila in VentasDgvDetalleFactura.Rows)
            {
                ClsComisionFacturaDetalle Detalle = Fila.DataBoundItem as ClsComisionFacturaDetalle;
                if (Detalle == null) continue;
                object Valor = Fila.Cells[VentasClmMetodoProducto.Index].Value;
                if (Detalle.ComisionGuardada && String.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                    Valor = Detalle.MetodoComision;
                if (Valor == null || String.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                    throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto en cada renglón de la factura.");
                Detalle.MetodoComision = VentasMetNormalizarMetodo(Convert.ToString(Valor));
                if (String.IsNullOrWhiteSpace(Detalle.MetodoComision))
                    throw new InvalidOperationException("Método no válido en la línea " + Detalle.IdFacturaDetalle + ": «" + Convert.ToString(Valor) + "».");
            }
            List<ClsComisionFacturaDetalle> Selecciones = _DetallesFactura;
            if (Selecciones.Any(Detalle => String.IsNullOrWhiteSpace(Detalle.MetodoComision)))
                throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto para cada producto. Use la paginación para revisar todos.");
            if (Selecciones.Count == 0) throw new InvalidOperationException("La factura no tiene productos cargados.");
            return Selecciones;
        }

        private void VentasMetCambiarMetodoProducto(object Sender, DataGridViewCellEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 ||
                VentasDgvDetalleFactura.Columns[Argumentos.ColumnIndex] != VentasClmMetodoProducto) return;
            ClsComisionFacturaDetalle Detalle = VentasDgvDetalleFactura.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionFacturaDetalle;
            if (Detalle == null || Detalle.ComisionGuardada) return;
            string Metodo = VentasMetNormalizarMetodo(Convert.ToString(
                VentasDgvDetalleFactura.Rows[Argumentos.RowIndex].Cells[Argumentos.ColumnIndex].Value));
            if (String.IsNullOrWhiteSpace(Metodo)) return;
            ClsCalculoComision.VentasMetActualizarVistaPrevia(Detalle, Metodo);
            decimal TotalComision = _DetallesFactura.Sum(Item => Item.ComisionTotal);
            decimal BaseTotal = _DetallesFactura.Sum(Item => Item.BaseVenta);
            decimal PorcentajePonderado = BaseTotal <= 0m ? 0m : TotalComision / BaseTotal * 100m;
            ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
            VentasLblEstadoFactura.Text = "Total comisión: " + PorcentajePonderado.ToString("0.00") + "% · " + (Factura == null ? String.Empty : Factura.SimboloMoneda) + " " + TotalComision.ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            VentasDgvDetalleFactura.Refresh();
        }

        private void VentasMetBuscarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblMantenimientoEstado,
                    VentasCboAlcance, VentasCboElementoPorcentaje, VentasTxtPorcentaje);
                if (VentasCboAlcance.SelectedItem == null)
                    throw new ArgumentException("Seleccione el tipo de porcentaje.");
                List<ClsProductoPorcentaje> Productos = VentasMetCargarPorcentajes();
                VentasLblMantenimientoEstado.Text = Productos.Count + " producto(s) cargados. Elija el elemento específico que desea modificar.";
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblMantenimientoEstado, Excepcion.Message, VentasCboAlcance);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetActualizarClick(object Sender, EventArgs Argumentos)
        {
            try
            {
                VentasMetRestablecerEstado(VentasLblMantenimientoEstado,
                    VentasCboAlcance, VentasCboElementoPorcentaje, VentasTxtPorcentaje);
                if (VentasCboAlcance.SelectedItem == null)
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "Seleccione el tipo de porcentaje.", VentasCboAlcance);
                    throw new ArgumentException("Seleccione el tipo de porcentaje.");
                }
                decimal Porcentaje;
                if (!decimal.TryParse(VentasTxtPorcentaje.Text, NumberStyles.Number,
                    CultureInfo.GetCultureInfo("es-GT"), out Porcentaje))
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "Ingrese un porcentaje válido entre 0.00 y 99.00.", VentasTxtPorcentaje);
                    throw new ArgumentException("Ingrese un porcentaje válido entre 0.00 y 99.00.");
                }
                if (Porcentaje < 0m || Porcentaje > 99m)
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "El porcentaje debe estar entre 0.00 y 99.00.", VentasTxtPorcentaje);
                    throw new ArgumentOutOfRangeException("Porcentaje", "El esquema permite porcentajes entre 0.00 y 99.00.");
                }
                var Opcion = VentasCboElementoPorcentaje.SelectedItem as KeyValuePair<int, string>?;
                if (Opcion == null)
                {
                    VentasMetMostrarError(VentasLblMantenimientoEstado,
                        "Seleccione un elemento específico.", VentasCboElementoPorcentaje);
                    throw new ArgumentException("Seleccione la línea, producto, categoría o marca que desea modificar.");
                }
                EnumMetodoComision Metodo = VentasMetObtenerMetodo(VentasCboAlcance.SelectedItem.ToString());
                string TextoConfirmacion = "Se actualizará el porcentaje de " + Opcion.Value.Value + " a " +
                    Porcentaje.ToString("0.00", CultureInfo.GetCultureInfo("es-GT")) + "%. ¿Desea continuar?";
                if (VentasMetConfirmar(this, "7008 – ComisionVendedores", TextoConfirmacion, "Sí, actualizar") != DialogResult.Yes)
                    return;
                int Actualizados = _ControladorComision.VentasMetActualizarPorcentaje(Metodo,
                    Opcion.Value.Key, Porcentaje);

                _ProductosPorcentaje = _ControladorComision.VentasMetConsultarPorcentajes(Metodo);
                _PaginaPorcentajes = Math.Min(_PaginaPorcentajes, VentasMetObtenerUltimaPagina(_ProductosPorcentaje.Count));
                VentasMetMostrarPaginaPorcentajes();
                VentasLblMantenimientoEstado.Text = Actualizados + " registro(s) de " + Opcion.Value.Value +
                    " guardado(s) con " + Porcentaje.ToString("0.00") + "%.";
                VentasMetCargarElementosPorcentaje();
                VentasMetMostrarExito(this, "7008 – ComisionVendedores", VentasLblMantenimientoEstado.Text);
            }
            catch (Exception Excepcion)
            {
                VentasMetMostrarError(VentasLblMantenimientoEstado, Excepcion.Message);
                MessageBox.Show(Excepcion.Message, "7008 – ComisionVendedores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ClsProductoPorcentaje> VentasMetCargarPorcentajes()
        {
            _ProductosPorcentaje = _ControladorComision.VentasMetConsultarPorcentajes(
                VentasMetObtenerMetodo(VentasCboAlcance.SelectedItem.ToString()));
            _PaginaPorcentajes = 0;
            VentasMetMostrarPaginaPorcentajes();
            VentasMetCargarElementosPorcentaje();
            return _ProductosPorcentaje;
        }

        private void VentasMetCambiarAlcancePorcentaje(object Sender, EventArgs Argumentos)
        {
            if (_CargandoPorcentajes || _ProductosPorcentaje == null) return;
            VentasCboElementoPorcentaje.DataSource = null;
            VentasMetCargarElementosPorcentaje();
        }

        private void VentasMetCargarElementosPorcentaje()
        {
            if (VentasCboElementoPorcentaje == null || VentasCboAlcance.SelectedItem == null) return;
            EnumMetodoComision Metodo = VentasMetObtenerMetodo(VentasCboAlcance.SelectedItem.ToString());
            IEnumerable<KeyValuePair<int, string>> Opciones;
            switch (Metodo)
            {
                case EnumMetodoComision.Producto:
                    Opciones = _ProductosPorcentaje.Select(Producto => new KeyValuePair<int, string>(Producto.IdProducto, Producto.NombreProducto));
                    break;
                case EnumMetodoComision.Marca:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdMarca > 0).GroupBy(Producto => Producto.IdMarca)
                        .Select(Grupo => new KeyValuePair<int, string>(Grupo.Key, Grupo.First().Marca + " · " + Grupo.Count() + " producto(s)"));
                    break;
                case EnumMetodoComision.Linea:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdLinea > 0).GroupBy(Producto => Producto.IdLinea)
                        .Select(Grupo => new KeyValuePair<int, string>(Grupo.Key, Grupo.First().Linea + " · " + Grupo.Count() + " producto(s)"));
                    break;
                case EnumMetodoComision.Categoria:
                    Opciones = _ProductosPorcentaje.Where(Producto => Producto.IdCategoria > 0).GroupBy(Producto => Producto.IdCategoria)
                        .Select(Grupo => new KeyValuePair<int, string>(Grupo.Key, Grupo.First().Categoria + " · " + Grupo.Count() + " producto(s)"));
                    break;
                default: Opciones = Enumerable.Empty<KeyValuePair<int, string>>(); break;
            }
            _CargandoPorcentajes = true;
            try
            {
                int IdSeleccionado = (VentasCboElementoPorcentaje.SelectedItem as KeyValuePair<int, string>?)?.Key ?? 0;
                List<KeyValuePair<int, string>> Lista = Opciones.OrderBy(Opcion => Opcion.Value).ToList();
                VentasCboElementoPorcentaje.DisplayMember = "Value";
                VentasCboElementoPorcentaje.ValueMember = "Key";
                VentasCboElementoPorcentaje.DataSource = Lista;
                VentasCboElementoPorcentaje.SelectedIndex = Lista.FindIndex(Opcion => Opcion.Key == IdSeleccionado);
            }
            finally { _CargandoPorcentajes = false; }
        }

        private void VentasMetMostrarPagina<T>(List<T> Registros, ref int Pagina, DataGridView Tabla,
            Label Resumen, Button Anterior, Button Siguiente)
        {
            int UltimaPagina = VentasMetObtenerUltimaPagina(Registros.Count);
            Pagina = Math.Max(0, Math.Min(Pagina, UltimaPagina));
            Tabla.DataSource = Registros.Skip(Pagina * _FilasPorPagina).Take(_FilasPorPagina).ToList();
            Resumen.Text = VentasMetObtenerTextoPagina(Pagina, Registros.Count);
            VentasMetActualizarBoton(Anterior, Pagina > 0);
            VentasMetActualizarBoton(Siguiente, Pagina < UltimaPagina);
            VentasMetAplicarIndicadorOrden(Tabla);
        }

        private void VentasMetMostrarPaginaResultados()
        {
            VentasMetMostrarPagina(_Resultados, ref _PaginaResultados, VentasDgvResultados,
                VentasLblPaginaResultados, VentasBtnAnteriorResultados, VentasBtnSiguienteResultados);
        }

        private void VentasMetMarcarColumnaObligatoria(object Sender, DataGridViewCellPaintingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex != -1 || Argumentos.ColumnIndex < 0 ||
                VentasDgvDetalleFactura.Columns[Argumentos.ColumnIndex] != VentasClmMetodoProducto)
                return;

            Argumentos.Paint(Argumentos.ClipBounds, DataGridViewPaintParts.All);
            string TextoEncabezado = Convert.ToString(Argumentos.FormattedValue);
            System.Drawing.Size TamanoTexto = TextRenderer.MeasureText(TextoEncabezado, Argumentos.CellStyle.Font);
            int X = Argumentos.CellBounds.Left + Argumentos.CellStyle.Padding.Left + TamanoTexto.Width;
            int Y = Argumentos.CellBounds.Top + (Argumentos.CellBounds.Height - TamanoTexto.Height) / 2;
            using (System.Drawing.Font FuenteAsterisco = new System.Drawing.Font(Argumentos.CellStyle.Font, System.Drawing.FontStyle.Bold))
            {
                TextRenderer.DrawText(Argumentos.Graphics, "*", FuenteAsterisco,
                    new System.Drawing.Point(X, Y), System.Drawing.Color.FromArgb(255, 124, 124), TextFormatFlags.NoPadding);
            }
            Argumentos.Handled = true;
        }

        private void VentasMetFormatearMoneda(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
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
            if (Tabla == VentasDgvResultados)
            {
                ClsComisionVendedorResultado Resultado = Tabla.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionVendedorResultado;
                if (Resultado != null) SimboloMoneda = Resultado.SimboloMoneda;
            }
            else
            {
                ClsFacturaComisionVista Factura = VentasCboFactura.SelectedItem as ClsFacturaComisionVista;
                if (Factura != null) SimboloMoneda = Factura.SimboloMoneda;
            }

            Argumentos.Value = (SimboloMoneda ?? String.Empty) + " " +
                Convert.ToDecimal(Argumentos.Value).ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            Argumentos.FormattingApplied = true;
        }

        private void VentasMetFormatearImporteNomina(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 || Argumentos.Value == null) return;
            string Propiedad = VentasDgvNomina.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (Propiedad != "TotalComision" && Propiedad != "MontoPagoCliente" && Propiedad != "ComisionDevengadaAbono") return;
            ClsComisionNominaVista Comision = VentasDgvNomina.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionNominaVista;
            if (Comision == null) return;
            Argumentos.Value = (Comision.SimboloMoneda ?? String.Empty) + " " +
                Convert.ToDecimal(Argumentos.Value).ToString("N2", CultureInfo.GetCultureInfo("es-GT"));
            Argumentos.FormattingApplied = true;
        }

        private void VentasMetOrdenarTabla(object Sender, DataGridViewCellMouseEventArgs Argumentos)
        {
            DataGridView Tabla = Sender as DataGridView;
            if (Tabla == null || Argumentos.ColumnIndex < 0 || Tabla.Columns[Argumentos.ColumnIndex] is DataGridViewComboBoxColumn) return;
            string Propiedad = Tabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (String.IsNullOrWhiteSpace(Propiedad)) return;

            bool Ascendente = !_ColumnasOrdenadas.ContainsKey(Tabla) || _ColumnasOrdenadas[Tabla] != Propiedad ||
                !_OrdenAscendente.ContainsKey(Tabla) || !_OrdenAscendente[Tabla];
            _ColumnasOrdenadas[Tabla] = Propiedad;
            _OrdenAscendente[Tabla] = Ascendente;
            foreach (DataGridViewColumn Columna in Tabla.Columns)
            {
                if (Columna is DataGridViewComboBoxColumn) continue;
                Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
                Columna.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            Tabla.Columns[Argumentos.ColumnIndex].HeaderCell.SortGlyphDirection = Ascendente ? SortOrder.Ascending : SortOrder.Descending;

            if (Tabla == VentasDgvResultados)
                VentasMetOrdenarLista(_Resultados, Propiedad, Ascendente);
            else if (Tabla == VentasDgvDetalleFactura)
                VentasMetOrdenarLista(_DetallesFactura, Propiedad, Ascendente);
            else if (Tabla == VentasDgvPorcentajes)
                VentasMetOrdenarLista(_ProductosPorcentaje, Propiedad, Ascendente);
            else if (Tabla == VentasDgvNomina && _ComisionesNomina != null)
                VentasMetOrdenarLista(_ComisionesNomina, Propiedad, Ascendente);

            if (Tabla == VentasDgvResultados) VentasMetMostrarPaginaResultados();
            else if (Tabla == VentasDgvDetalleFactura) VentasMetMostrarPaginaFactura();
            else if (Tabla == VentasDgvPorcentajes) VentasMetMostrarPaginaPorcentajes();
            else if (Tabla == VentasDgvNomina)
            {
                VentasMetMostrarPaginaNomina();
                VentasMetAplicarIndicadorOrden(VentasDgvNomina);
            }
        }

        private static void VentasMetOrdenarLista<T>(List<T> Registros, string NombrePropiedad, bool Ascendente)
        {
            if (Registros == null || Registros.Count < 2) return;
            System.Reflection.PropertyInfo Propiedad = typeof(T).GetProperty(NombrePropiedad);
            if (Propiedad == null) return;
            Registros.Sort((Primero, Segundo) =>
            {
                object ValorPrimero = Propiedad.GetValue(Primero, null);
                object ValorSegundo = Propiedad.GetValue(Segundo, null);
                int Comparacion;
                if (ValorPrimero == null) Comparacion = ValorSegundo == null ? 0 : -1;
                else if (ValorSegundo == null) Comparacion = 1;
                else if (ValorPrimero is IComparable) Comparacion = ((IComparable)ValorPrimero).CompareTo(ValorSegundo);
                else Comparacion = String.Compare(Convert.ToString(ValorPrimero), Convert.ToString(ValorSegundo), StringComparison.CurrentCultureIgnoreCase);
                return Ascendente ? Comparacion : -Comparacion;
            });
        }

        private void VentasMetAplicarIndicadorOrden(DataGridView Tabla)
        {
            string Propiedad;
            bool Ascendente;
            if (!_ColumnasOrdenadas.TryGetValue(Tabla, out Propiedad) || !_OrdenAscendente.TryGetValue(Tabla, out Ascendente)) return;
            foreach (DataGridViewColumn Columna in Tabla.Columns)
            {
                if (Columna is DataGridViewComboBoxColumn) continue;
                Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
                Columna.HeaderCell.SortGlyphDirection = Columna.DataPropertyName == Propiedad
                    ? (Ascendente ? SortOrder.Ascending : SortOrder.Descending) : SortOrder.None;
            }
        }

        private void VentasMetMostrarPaginaFactura()
        {
            VentasMetMostrarPagina(_DetallesFactura, ref _PaginaFactura, VentasDgvDetalleFactura,
                VentasLblPaginaFactura, VentasBtnAnteriorFactura, VentasBtnSiguienteFactura);
        }

        private void VentasMetFormatearPorcentajeProducto(object Sender, DataGridViewCellFormattingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || Argumentos.ColumnIndex < 0 ||
                VentasDgvDetalleFactura.Columns[Argumentos.ColumnIndex] != VentasClmDetallePorcentaje) return;
            ClsComisionFacturaDetalle Detalle = VentasDgvDetalleFactura.Rows[Argumentos.RowIndex].DataBoundItem as ClsComisionFacturaDetalle;
            if (Detalle != null)
            {
                Argumentos.Value = Detalle.Porcentaje.ToString("0.00") + "%";
                Argumentos.FormattingApplied = true;
            }
        }

        private void VentasMetMostrarPaginaPorcentajes()
        {
            VentasMetMostrarPagina(_ProductosPorcentaje, ref _PaginaPorcentajes, VentasDgvPorcentajes,
                VentasLblPaginaPorcentajes, VentasBtnAnteriorPorcentajes, VentasBtnSiguientePorcentajes);
        }

        private void VentasMetCambiarPaginaResultados(object Sender, EventArgs Argumentos)
        {
            _PaginaResultados += Sender == VentasBtnAnteriorResultados ? -1 : 1;
            VentasMetMostrarPaginaResultados();
        }

        private void VentasMetCambiarPaginaFactura(object Sender, EventArgs Argumentos)
        {
            _PaginaFactura += Sender == VentasBtnAnteriorFactura ? -1 : 1;
            VentasMetMostrarPaginaFactura();
        }

        private void VentasMetCambiarPaginaPorcentajes(object Sender, EventArgs Argumentos)
        {
            _PaginaPorcentajes += Sender == VentasBtnAnteriorPorcentajes ? -1 : 1;
            VentasMetMostrarPaginaPorcentajes();
        }

        private void VentasMetMostrarPaginaNomina()
        {
            if (_ComisionesNomina == null) _ComisionesNomina = new List<ClsComisionNominaVista>();
            VentasMetMostrarPagina(_ComisionesNomina, ref _PaginaNomina, VentasDgvNomina,
                VentasLblPaginaNomina, VentasBtnAnteriorNomina, VentasBtnSiguienteNomina);
        }

        private void VentasMetCambiarPaginaNomina(object Sender, EventArgs Argumentos)
        {
            _PaginaNomina += Sender == VentasBtnAnteriorNomina ? -1 : 1;
            VentasMetMostrarPaginaNomina();
        }
    }
}
