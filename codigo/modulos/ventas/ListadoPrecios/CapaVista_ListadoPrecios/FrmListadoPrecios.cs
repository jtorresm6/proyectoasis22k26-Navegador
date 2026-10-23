/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Diego
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Diego Alejandro Cheng Peña | Carné: 0901-22-8091
 * Encargado: Diego Alejandro Cheng Peña
 * Fecha de actualización: 18/10/2026
 * Propósito: Listado de precios por tipo de Cliente con arquitectura Vista-Controlador-Modelo.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using CapaControlador_ListadoPrecios;

namespace CapaVista_ListadoPrecios
{
    public partial class FrmListadoPrecios : Form
    {
        private readonly ClsCtrlListadoPrecios _Controlador;
        private readonly CultureInfo _Cultura = CultureInfo.GetCultureInfo("es-GT");
        private bool _Cargando;
        private List<ClsPrecioCliente> _DatosListado = new List<ClsPrecioCliente>();

        public FrmListadoPrecios()
        {
            InitializeComponent();
            _Controlador = new ClsCtrlListadoPrecios();
            VentasNudPorcentajeDescuento.DecimalPlaces = 2;
            VentasNudPorcentajeDescuento.Minimum = 0;
            VentasNudPorcentajeDescuento.Maximum = 100;
            VentasNudPorcentajeDescuento.ValueChanged += VentasNudPorcentajeDescuento_ValueChanged;
            VentasTxtPrecioBase.TextChanged += VentasTxtPrecioBase_TextChanged;
            VentasDgvListadoClientes.CellClick += VentasDgvListadoClientes_CellClick;
            VentasBtnConsultar.Click += VentasBtnConsultar_Click;
            VentasBtnGuardar.Click += VentasBtnGuardar_Click;
            VentasBtnCancelar.Click += VentasBtnCancelar_Click;
            VentasCboTipoCliente.SelectedIndexChanged += VentasCboTipoCliente_SelectedIndexChanged;
            VentasCboTipoProducto.SelectedIndexChanged += VentasCboTipoProducto_SelectedIndexChanged;
            VentasMetAgregarOpcion(VentasCboTipoCliente, "Todos");
            VentasMetAgregarOpcion(VentasCboTipoProducto, "Todos");
            VentasCboTipoCliente.SelectedItem = "Todos";
            VentasCboTipoProducto.SelectedItem = "Todos";
            VentasTxtPrecioFinal.ReadOnly = true;
            VentasTxtPrecioBase.Text = "0.00";
            VentasTxtPrecioFinal.Text = "0.00";
            VentasMetCargarListado();
        }

        private void VentasMetCargarListado()
        {
            try
            {
                _Cargando = true;
                _DatosListado = _Controlador.ListadoPreciosMetConsultarListado();
                foreach (ClsPrecioCliente Registro in _DatosListado)
                {
                    VentasMetAgregarOpcion(VentasCboTipoCliente, Registro.TipoCliente);
                    VentasMetAgregarOpcion(VentasCboTipoProducto, Registro.TipoProducto);
                }
                VentasMetAplicarFiltros();
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show("No fue posible cargar el listado de precios.\n" + Excepcion.Message,
                    "Listado de precios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _Cargando = false; }
        }

        private void VentasMetAgregarOpcion(ComboBox Combo, string Valor)
        {
            if (!string.IsNullOrWhiteSpace(Valor) && !Combo.Items.Contains(Valor)) Combo.Items.Add(Valor);
        }

        // Aplica ambos filtros al listado sin modificar los registros originales.
        private void VentasMetAplicarFiltros()
        {
            string TipoCliente = Convert.ToString(VentasCboTipoCliente.SelectedItem);
            string TipoProducto = Convert.ToString(VentasCboTipoProducto.SelectedItem);
            VentasDgvListadoClientes.Rows.Clear();
            foreach (ClsPrecioCliente Registro in _DatosListado)
            {
                bool CoincideCliente = string.IsNullOrWhiteSpace(TipoCliente) || TipoCliente == "Todos" ||
                    string.Equals(Registro.TipoCliente, TipoCliente, StringComparison.OrdinalIgnoreCase);
                bool CoincideProducto = string.IsNullOrWhiteSpace(TipoProducto) || TipoProducto == "Todos" ||
                    string.Equals(Registro.TipoProducto, TipoProducto, StringComparison.OrdinalIgnoreCase);
                if (!CoincideCliente || !CoincideProducto) continue;
                VentasDgvListadoClientes.Rows.Add(Registro.TipoCliente, Registro.TipoProducto,
                    Registro.PorcentajeDescuento.ToString("N2", _Cultura), Registro.PrecioBase.ToString("N2", _Cultura),
                    Registro.PrecioFinal.ToString("N2", _Cultura), "Seleccionar");
            }
            VentasDgvListadoClientes.ClearSelection();
        }

        private void VentasMetActualizarPrecioFinal()
        {
            decimal PrecioBase;
            if (!decimal.TryParse(VentasTxtPrecioBase.Text, NumberStyles.Currency, _Cultura, out PrecioBase) &&
                !decimal.TryParse(VentasTxtPrecioBase.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out PrecioBase))
            { VentasTxtPrecioFinal.Text = ""; return; }
            if (PrecioBase < 0) { VentasTxtPrecioFinal.Text = ""; return; }
            try { VentasTxtPrecioFinal.Text = _Controlador.ListadoPreciosMetCalcularPrecioFinal(PrecioBase, VentasNudPorcentajeDescuento.Value).ToString("N2", _Cultura); }
            catch { VentasTxtPrecioFinal.Text = ""; }
        }

        private void VentasBtnConsultar_Click(object Remitente, EventArgs Evento)
        {
            VentasMetCargarListado();
            if (_Controlador.UsaBaseDeDatos) Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(this, _Controlador.MensajeEstado, "Consulta de precios");
            else Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarAdvertencia(this, _Controlador.MensajeEstado, "Consulta de precios");
        }

        private void VentasBtnGuardar_Click(object Remitente, EventArgs Evento)
        {
            decimal PrecioBase;
            if (!decimal.TryParse(VentasTxtPrecioBase.Text, NumberStyles.Currency, _Cultura, out PrecioBase) &&
                !decimal.TryParse(VentasTxtPrecioBase.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out PrecioBase))
            { MessageBox.Show("Ingrese un precio base numérico.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); VentasTxtPrecioBase.Focus(); return; }
            string Mensaje;
            bool Guardado = _Controlador.ListadoPreciosMetGuardar(Convert.ToString(VentasCboTipoCliente.SelectedItem),
                Convert.ToString(VentasCboTipoProducto.SelectedItem), VentasNudPorcentajeDescuento.Value, PrecioBase, out Mensaje);
            if (Guardado && _Controlador.UsaBaseDeDatos) Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarExito(this, Mensaje, "Listado de precios");
            else if (Guardado) Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarAdvertencia(this, Mensaje, "Listado de precios");
            else Ventas.Estandarizacion.ClsDialogosVentas.VentasMetMostrarError(this, Mensaje, "No se pudo guardar");
            if (Guardado) { VentasMetCargarListado(); VentasMetLimpiarCampos(false); }
        }

        private void VentasBtnCancelar_Click(object Remitente, EventArgs Evento) { VentasMetLimpiarCampos(true); }
        private void VentasCboTipoCliente_SelectedIndexChanged(object Remitente, EventArgs Evento) { if (!_Cargando) VentasMetAplicarFiltros(); }
        private void VentasCboTipoProducto_SelectedIndexChanged(object Remitente, EventArgs Evento) { if (!_Cargando) VentasMetAplicarFiltros(); }
        private void VentasNudPorcentajeDescuento_ValueChanged(object Remitente, EventArgs Evento) { VentasMetActualizarPrecioFinal(); }
        private void VentasTxtPrecioBase_TextChanged(object Remitente, EventArgs Evento) { VentasMetActualizarPrecioFinal(); }

        private void VentasDgvListadoClientes_CellClick(object Remitente, DataGridViewCellEventArgs Evento)
        {
            if (Evento.RowIndex < 0) return;
            VentasMetCargarFila(VentasDgvListadoClientes.Rows[Evento.RowIndex]);
        }

        private void VentasMetCargarFila(DataGridViewRow Fila)
        {
            _Cargando = true;
            try
            {
                string Cliente = Convert.ToString(Fila.Cells[0].Value);
                string Producto = Convert.ToString(Fila.Cells[1].Value);
                if (VentasCboTipoCliente.Items.Contains(Cliente)) VentasCboTipoCliente.SelectedItem = Cliente;
                if (VentasCboTipoProducto.Items.Contains(Producto)) VentasCboTipoProducto.SelectedItem = Producto;
                decimal Descuento, PrecioBase;
                if (decimal.TryParse(Convert.ToString(Fila.Cells[2].Value), NumberStyles.Any, _Cultura, out Descuento))
                    VentasNudPorcentajeDescuento.Value = Math.Min(VentasNudPorcentajeDescuento.Maximum, Math.Max(VentasNudPorcentajeDescuento.Minimum, Descuento));
                if (decimal.TryParse(Convert.ToString(Fila.Cells[3].Value), NumberStyles.Any, _Cultura, out PrecioBase)) VentasTxtPrecioBase.Text = PrecioBase.ToString("N2", _Cultura);
            }
            finally { _Cargando = false; VentasMetActualizarPrecioFinal(); }
        }

        private void VentasMetLimpiarCampos(bool LimpiarSeleccion)
        {
            _Cargando = true;
            try
            {
                if (VentasCboTipoCliente.Items.Contains("Todos")) VentasCboTipoCliente.SelectedItem = "Todos";
                if (VentasCboTipoProducto.Items.Contains("Todos")) VentasCboTipoProducto.SelectedItem = "Todos";
                VentasNudPorcentajeDescuento.Value = 0;
                VentasTxtPrecioBase.Text = "0.00";
                VentasTxtPrecioFinal.Text = "0.00";
                if (LimpiarSeleccion) VentasDgvListadoClientes.ClearSelection();
            }
            finally { _Cargando = false; VentasMetAplicarFiltros(); VentasMetActualizarPrecioFinal(); }
        }

        private void VentasLblSimboloPorcentaje_Click(object Remitente, EventArgs Evento) { }
    }
}

// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 18/10/2026
