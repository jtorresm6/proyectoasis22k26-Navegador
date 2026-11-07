/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Diego
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Diego Alejandro Cheng Peña | Carné: 0901-22-8091
 * Encargado: Diego Alejandro Cheng Peña
 * Fecha de actualización: 18/10/2026
 * Propósito: Listado de precios por tipo de cliente con arquitectura Vista-Controlador-Modelo.
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
            VentasNudPorcentajeDescuento.ValueChanged += VentasMetNudPorcentajeDescuentoValueChanged;
            VentasTxtPrecioBase.TextChanged += VentasMetTxtPrecioBaseTextChanged;
            VentasDgvListadoClientes.CellClick += VentasMetDgvListadoClientesCellClick;
            VentasBtnConsultar.Click += VentasMetBtnConsultarClick;
            VentasBtnGuardar.Click += VentasMetBtnGuardarClick;
            VentasBtnCancelar.Click += VentasMetBtnCancelarClick;
            VentasCboTipoCliente.SelectedIndexChanged += VentasMetCboTipoClienteSelectedIndexChanged;
            VentasCboTipoProducto.SelectedIndexChanged += VentasMetCboTipoProductoSelectedIndexChanged;
            VentasMetAgregarOpcion(VentasCboTipoCliente, "Todos");
            VentasMetAgregarOpcion(VentasCboTipoProducto, "Todos");
            VentasCboTipoCliente.SelectedItem = "Todos";
            VentasCboTipoProducto.SelectedItem = "Todos";
            VentasTxtPrecioFinal.ReadOnly = true;
            VentasTxtPrecioBase.ReadOnly = true;
            VentasTxtPrecioBase.Text = "0.00";
            VentasTxtPrecioFinal.Text = "0.00";
            VentasMetCargarListado();
        }

        private void VentasMetCargarListado()
        {
            try
            {
                _Cargando = true;
                _DatosListado = _Controlador.VentasFuncConsultarListado();
                foreach (ClsPrecioCliente Elemento in _DatosListado)
                {
                    VentasMetAgregarOpcion(VentasCboTipoCliente, Elemento.TipoCliente);
                    VentasMetAgregarOpcion(VentasCboTipoProducto, Elemento.TipoProducto);
                }
                VentasMetAplicarFiltros();
            }
            catch (Exception Error)
            {
                MessageBox.Show("No fue posible cargar el listado de precios.\n" + Error.Message,
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
            foreach (ClsPrecioCliente Elemento in _DatosListado)
            {
                bool CoincideCliente = string.IsNullOrWhiteSpace(TipoCliente) || TipoCliente == "Todos" ||
                    string.Equals(Elemento.TipoCliente, TipoCliente, StringComparison.OrdinalIgnoreCase);
                bool CoincideProducto = string.IsNullOrWhiteSpace(TipoProducto) || TipoProducto == "Todos" ||
                    string.Equals(Elemento.TipoProducto, TipoProducto, StringComparison.OrdinalIgnoreCase);
                if (!CoincideCliente || !CoincideProducto) continue;
                VentasDgvListadoClientes.Rows.Add(Elemento.TipoCliente, Elemento.TipoProducto,
                    Elemento.PorcentajeDescuento.ToString("N2", _Cultura), Elemento.PrecioBase.ToString("N2", _Cultura),
                    Elemento.PrecioFinal.ToString("N2", _Cultura), "Seleccionar");
            }
            VentasDgvListadoClientes.ClearSelection();
        }

        private void VentasMetActualizarPrecioFinal()
        {
            decimal BasePrecio;
            if (!decimal.TryParse(VentasTxtPrecioBase.Text, NumberStyles.Currency, _Cultura, out BasePrecio) &&
                !decimal.TryParse(VentasTxtPrecioBase.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out BasePrecio))
            { VentasTxtPrecioFinal.Text = ""; return; }
            if (BasePrecio < 0) { VentasTxtPrecioFinal.Text = ""; return; }
            try { VentasTxtPrecioFinal.Text = _Controlador.VentasFuncCalcularPrecioFinal(BasePrecio, VentasNudPorcentajeDescuento.Value).ToString("N2", _Cultura); }
            catch { VentasTxtPrecioFinal.Text = ""; }
        }

        private void VentasMetBtnConsultarClick(object Origen, EventArgs Evento)
        {
            VentasMetCargarListado();
            MessageBox.Show(_Controlador.MensajeEstado, "Consulta de precios", MessageBoxButtons.OK,
                _Controlador.UsaBaseDeDatos ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void VentasMetBtnGuardarClick(object Origen, EventArgs Evento)
        {
            decimal PrecioBase;
            if (!decimal.TryParse(VentasTxtPrecioBase.Text, NumberStyles.Currency, _Cultura, out PrecioBase) &&
                !decimal.TryParse(VentasTxtPrecioBase.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out PrecioBase))
            { MessageBox.Show("Ingrese un precio base numérico.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); VentasTxtPrecioBase.Focus(); return; }
            string Mensaje;
            bool Guardado = _Controlador.VentasFuncGuardar(Convert.ToString(VentasCboTipoCliente.SelectedItem),
                Convert.ToString(VentasCboTipoProducto.SelectedItem), VentasNudPorcentajeDescuento.Value, PrecioBase, out Mensaje);
            MessageBox.Show(Mensaje, Guardado ? "Listado de precios" : "No se pudo guardar", MessageBoxButtons.OK,
                Guardado && _Controlador.UsaBaseDeDatos ? MessageBoxIcon.Information : (Guardado ? MessageBoxIcon.Warning : MessageBoxIcon.Error));
            if (Guardado) { VentasMetCargarListado(); VentasMetLimpiarCampos(false); }
        }

        private void VentasMetBtnCancelarClick(object Origen, EventArgs Evento) { VentasMetLimpiarCampos(true); }
        private void VentasMetCboTipoClienteSelectedIndexChanged(object Origen, EventArgs Evento) { if (!_Cargando) VentasMetAplicarFiltros(); }
        private void VentasMetCboTipoProductoSelectedIndexChanged(object Origen, EventArgs Evento) { if (!_Cargando) VentasMetAplicarFiltros(); }
        private void VentasMetNudPorcentajeDescuentoValueChanged(object Origen, EventArgs Evento) { VentasMetActualizarPrecioFinal(); }
        private void VentasMetTxtPrecioBaseTextChanged(object Origen, EventArgs Evento) { VentasMetActualizarPrecioFinal(); }

        private void VentasMetDgvListadoClientesCellClick(object Origen, DataGridViewCellEventArgs Evento)
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
                decimal Descuento, BasePrecio;
                if (decimal.TryParse(Convert.ToString(Fila.Cells[2].Value), NumberStyles.Any, _Cultura, out Descuento))
                    VentasNudPorcentajeDescuento.Value = Math.Min(VentasNudPorcentajeDescuento.Maximum, Math.Max(VentasNudPorcentajeDescuento.Minimum, Descuento));
                if (decimal.TryParse(Convert.ToString(Fila.Cells[3].Value), NumberStyles.Any, _Cultura, out BasePrecio)) VentasTxtPrecioBase.Text = BasePrecio.ToString("N2", _Cultura);
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

        private void VentasMetLblSimboloPorcentajeClick(object Origen, EventArgs Evento) { }
    }
}

// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 18/10/2026
