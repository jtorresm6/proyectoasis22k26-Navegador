/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Diego
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Diego Alejandro Cheng Peña | Carné: 0901-22-8091
 * Encargado: Diego Alejandro Cheng Peña
 * Fecha de actualización: 01/11/2026
 * Propósito: Gestión de listados de precios conectada a dbsistemaembutidos, con filtros,
 * selección por alcance, cálculo de modificaciones y guardado transaccional.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using CapaControlador_ListadoPrecios;
using CapaModelo_ListadoPrecios.Entidades;
using ComboBox = System.Windows.Forms.ComboBox;

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

            // Las columnas adicionales se agregan en ejecución para conservar el Designer original.
            VentasMetAgregarColumnasListado();
            VentasLblTipoCliente.Text = "Lista de precios";
            VentasClmTipoCliente.HeaderText = "Lista de precios";

            VentasNudPorcentajeDescuento.DecimalPlaces = 2;
            VentasNudPorcentajeDescuento.Minimum = 0;
            VentasNudPorcentajeDescuento.Maximum = 100;

            VentasNudPorcentajeDescuento.ValueChanged += VentasMetActualizarPrecioFinal;
            VentasTxtLisPre.TextChanged += VentasMetActualizarPrecioFinal;

            VentasRbtnDescuento.CheckedChanged += VentasMetTipoModificacionChanged;
            VentasRbtnAumPor.CheckedChanged += VentasMetTipoModificacionChanged;
            VentasRbtnAumBase.CheckedChanged += VentasMetTipoModificacionChanged;

            VentasRbtnPrdInd.CheckedChanged += VentasMetAlcanceModificacionChanged;
            VentasRbtnPrdSelec.CheckedChanged += VentasMetAlcanceModificacionChanged;
            VentasRbtnTdsRslt.CheckedChanged += VentasMetAlcanceModificacionChanged;

            VentasDgvListadoClientes.CellClick += VentasMetDgvListadoClientesCellClick;
            VentasDgvListadoClientes.SelectionChanged += VentasMetActualizarPrecioFinal;

            VentasBtnConsultarListadoPrecios.Click += VentasMetBtnConsultarClick;
            VentasBtnCotRefrescar.Click += VentasMetBtnRefrescarClick;
            VentasBtnGuardarListadoPrecios.Click += VentasMetBtnGuardarClick;
            VentasBtnCancelarListadoPrecios.Click += VentasMetBtnCancelarClick;

            VentasCboTipoCliente.SelectedIndexChanged += VentasMetFiltrosSelectedIndexChanged;
            VentasCboTipoProducto.SelectedIndexChanged += VentasMetFiltrosSelectedIndexChanged;
            VentasCboLinea.SelectedIndexChanged += VentasMetFiltrosSelectedIndexChanged;
            VentasCboCategoria.SelectedIndexChanged += VentasMetFiltrosSelectedIndexChanged;
            VentasCboProveedores.SelectedIndexChanged += VentasMetFiltrosSelectedIndexChanged;
            VentasCboProducto.SelectedIndexChanged += VentasMetFiltrosSelectedIndexChanged;

            VentasRbtnDescuento.Checked = true;
            VentasRbtnPrdInd.Checked = true;
            VentasTxtLisPre.Text = "0.00";
            VentasNudPorcentajeDescuento.Value = 0;

            VentasMetConfigurarPermisosModificacion();
            VentasMetConfigurarAlcanceGrilla();
            VentasMetCargarListado();
        }

        private void VentasMetAgregarColumnasListado()
        {
            if (!VentasDgvListadoClientes.Columns.Contains("VentasClmMarca"))
                VentasDgvListadoClientes.Columns.Insert(2, new DataGridViewTextBoxColumn
                {
                    Name = "VentasClmMarca", HeaderText = "Marca", ReadOnly = true,
                    MinimumWidth = 80, SortMode = DataGridViewColumnSortMode.NotSortable
                });

            if (!VentasDgvListadoClientes.Columns.Contains("VentasClmLinea"))
                VentasDgvListadoClientes.Columns.Insert(3, new DataGridViewTextBoxColumn
                {
                    Name = "VentasClmLinea", HeaderText = "Línea", ReadOnly = true,
                    MinimumWidth = 80, SortMode = DataGridViewColumnSortMode.NotSortable
                });

            if (!VentasDgvListadoClientes.Columns.Contains("VentasClmCategoria"))
                VentasDgvListadoClientes.Columns.Insert(4, new DataGridViewTextBoxColumn
                {
                    Name = "VentasClmCategoria", HeaderText = "Categoría", ReadOnly = true,
                    MinimumWidth = 90, SortMode = DataGridViewColumnSortMode.NotSortable
                });

            if (!VentasDgvListadoClientes.Columns.Contains("VentasClmProveedores"))
                VentasDgvListadoClientes.Columns.Insert(5, new DataGridViewTextBoxColumn
                {
                    Name = "VentasClmProveedores", HeaderText = "Proveedor", ReadOnly = true,
                    MinimumWidth = 90, SortMode = DataGridViewColumnSortMode.NotSortable
                });

            VentasDgvListadoClientes.MultiSelect = false;
            VentasDgvListadoClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            VentasDgvListadoClientes.ReadOnly = true;
            VentasDgvListadoClientes.AllowUserToAddRows = false;
            VentasDgvListadoClientes.AllowUserToDeleteRows = false;
        }

        private void VentasMetCargarListado()
        {
            try
            {
                _Cargando = true;
                _DatosListado = _Controlador.VentasFuncConsultarListado(null, null, null, null, null, null);
                VentasMetLlenarCombosDinamicos();
            }
            catch (Exception Error)
            {
                _DatosListado = new List<ClsPrecioCliente>();
                MessageBox.Show("No fue posible cargar el listado desde la base de datos.\n" + Error.Message,
                    "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _Cargando = false;
            }
            VentasMetAplicarFiltros();
        }

        private void VentasMetLlenarCombosDinamicos()
        {
            VentasMetLlenarCombo(VentasCboTipoCliente, x => x.TipoCliente);
            VentasMetLlenarCombo(VentasCboTipoProducto, x => x.Marca);
            VentasMetLlenarCombo(VentasCboLinea, x => x.Linea);
            VentasMetLlenarCombo(VentasCboCategoria, x => x.Categoria);
            VentasMetLlenarCombo(VentasCboProveedores, x => x.Proveedor);
            VentasMetLlenarCombo(VentasCboProducto, x => x.Producto);
        }

        private void VentasMetLlenarCombo(ComboBox combo, Func<ClsPrecioCliente, string> selector)
        {
            if (combo == null) return;
            string seleccionado = Convert.ToString(combo.SelectedItem);
            var valores = new List<string> { "Todos" };
            foreach (ClsPrecioCliente item in _DatosListado)
            {
                string valor = selector(item);
                if (!string.IsNullOrWhiteSpace(valor) && !valores.Contains(valor))
                    valores.Add(valor);
            }

            combo.BeginUpdate();
            try
            {
                combo.Items.Clear();
                combo.Items.AddRange(valores.ToArray());
                combo.SelectedItem = valores.Contains(seleccionado) ? seleccionado : "Todos";
            }
            finally
            {
                combo.EndUpdate();
            }
        }

        private void VentasMetAplicarFiltros()
        {
            if (_Cargando) return;
            bool estadoAnterior = _Cargando;
            _Cargando = true;
            try
            {
                string tipoCliente = Convert.ToString(VentasCboTipoCliente.SelectedItem);
                string marca = Convert.ToString(VentasCboTipoProducto.SelectedItem);
                string linea = Convert.ToString(VentasCboLinea.SelectedItem);
                string categoria = Convert.ToString(VentasCboCategoria.SelectedItem);
                string proveedor = Convert.ToString(VentasCboProveedores.SelectedItem);
                string producto = Convert.ToString(VentasCboProducto.SelectedItem);

                VentasDgvListadoClientes.Rows.Clear();
                foreach (ClsPrecioCliente elemento in _DatosListado)
                {
                    bool coincide =
                        (string.IsNullOrWhiteSpace(tipoCliente) || tipoCliente == "Todos" || string.Equals(elemento.TipoCliente, tipoCliente, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrWhiteSpace(marca) || marca == "Todos" || string.Equals(elemento.Marca, marca, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrWhiteSpace(linea) || linea == "Todos" || string.Equals(elemento.Linea, linea, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrWhiteSpace(categoria) || categoria == "Todos" || string.Equals(elemento.Categoria, categoria, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrWhiteSpace(proveedor) || proveedor == "Todos" || string.Equals(elemento.Proveedor, proveedor, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrWhiteSpace(producto) || producto == "Todos" || string.Equals(elemento.Producto, producto, StringComparison.OrdinalIgnoreCase));

                    if (!coincide) continue;

                    int indice = VentasDgvListadoClientes.Rows.Add();
                    DataGridViewRow fila = VentasDgvListadoClientes.Rows[indice];
                    fila.Cells["VentasClmTipoCliente"].Value = elemento.TipoCliente;
                    fila.Cells["VentasClmTipoProducto"].Value = elemento.Producto;
                    fila.Cells["VentasClmMarca"].Value = elemento.Marca;
                    fila.Cells["VentasClmLinea"].Value = elemento.Linea;
                    fila.Cells["VentasClmCategoria"].Value = elemento.Categoria;
                    fila.Cells["VentasClmProveedores"].Value = elemento.Proveedor;
                    fila.Cells["VentasClmPorcentaje"].Value = elemento.PorcentajeDescuento.ToString("N2", _Cultura);
                    fila.Cells["VentasClmPrecioBase"].Value = elemento.PrecioBase.ToString("N2", _Cultura);
                    fila.Cells["VentasClmPrecioFinal"].Value = elemento.PrecioFinal.ToString("N2", _Cultura);
                    fila.Cells["VentasClmAcciones"].Value = "Seleccionar";
                    fila.Tag = elemento;
                }
                VentasDgvListadoClientes.ClearSelection();
            }
            finally
            {
                _Cargando = estadoAnterior;
            }
        }

        private void VentasMetConfigurarPermisosModificacion()
        {
            bool esPorcentaje = VentasRbtnDescuento.Checked || VentasRbtnAumPor.Checked;
            bool esBase = VentasRbtnAumBase.Checked;
            VentasNudPorcentajeDescuento.Enabled = esPorcentaje;
            VentasTxtLisPre.Enabled = esBase;
            VentasLblSimboloPorcentaje.Enabled = esPorcentaje;
        }

        private void VentasMetConfigurarAlcanceGrilla()
        {
            VentasDgvListadoClientes.ClearSelection();
            if (VentasRbtnPrdSelec.Checked)
            {
                VentasDgvListadoClientes.MultiSelect = true;
                VentasDgvListadoClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            else
            {
                VentasDgvListadoClientes.MultiSelect = false;
                VentasDgvListadoClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            VentasMetActualizarPrecioFinal(null, EventArgs.Empty);
        }

        private void VentasMetActualizarPrecioFinal(object sender, EventArgs e)
        {
            if (_Cargando || VentasDgvListadoClientes == null || VentasDgvListadoClientes.Rows.Count == 0) return;

            string tipo = VentasRbtnDescuento.Checked ? "Descuento" :
                (VentasRbtnAumPor.Checked ? "AumentoPorcentaje" : "AumentoBase");
            decimal valor;
            if (VentasRbtnAumBase.Checked)
            {
                if (!decimal.TryParse(VentasTxtLisPre.Text, NumberStyles.Number, _Cultura, out valor) || valor < 0m) return;
            }
            else valor = VentasNudPorcentajeDescuento.Value;

            var filas = new List<DataGridViewRow>();
            if (VentasRbtnTdsRslt.Checked)
            {
                foreach (DataGridViewRow fila in VentasDgvListadoClientes.Rows)
                    if (!fila.IsNewRow) filas.Add(fila);
            }
            else
            {
                foreach (DataGridViewRow fila in VentasDgvListadoClientes.SelectedRows)
                    if (!fila.IsNewRow) filas.Add(fila);
            }

            foreach (DataGridViewRow fila in filas)
            {
                ClsPrecioCliente registro = fila.Tag as ClsPrecioCliente;
                if (registro == null) continue;
                decimal nuevaBase = registro.PrecioBase;
                decimal nuevoPrecio;
                decimal nuevoPorcentaje = registro.PorcentajeDescuento;

                if (tipo == "AumentoBase")
                {
                    nuevaBase = registro.PrecioBase + valor;
                    nuevoPrecio = nuevaBase - (nuevaBase * (registro.PorcentajeDescuento / 100m));
                }
                else
                {
                    nuevoPrecio = _Controlador.VentasFuncCalcularPrecioFinal(registro.PrecioBase, valor, tipo);
                    nuevoPorcentaje = registro.PrecioBase > 0m
                        ? Math.Round((registro.PrecioBase - nuevoPrecio) / registro.PrecioBase * 100m, 2)
                        : 0m;
                }

                fila.Cells["VentasClmPrecioBase"].Value = nuevaBase.ToString("N2", _Cultura);
                fila.Cells["VentasClmPrecioFinal"].Value = nuevoPrecio.ToString("N2", _Cultura);
                fila.Cells["VentasClmPorcentaje"].Value = nuevoPorcentaje.ToString("N2", _Cultura);
            }
        }

        private void VentasMetTipoModificacionChanged(object sender, EventArgs e)
        {
            RadioButton boton = sender as RadioButton;
            if (boton != null && boton.Checked)
            {
                VentasMetConfigurarPermisosModificacion();
                VentasMetActualizarPrecioFinal(null, EventArgs.Empty);
            }
        }

        private void VentasMetAlcanceModificacionChanged(object sender, EventArgs e)
        {
            RadioButton boton = sender as RadioButton;
            if (boton != null && boton.Checked)
                VentasMetConfigurarAlcanceGrilla();
        }

        private void VentasMetBtnConsultarClick(object Origen, EventArgs Evento)
        {
            VentasMetCargarListado();
            MessageBox.Show(_Controlador.MensajeEstado ?? "Consulta realizada exitosamente.",
                "Listado de Precios", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void VentasMetBtnRefrescarClick(object Origen, EventArgs Evento)
        {
            VentasMetLimpiarFiltros();
            VentasMetCargarListado();
        }

        private void VentasMetLimpiarFiltros()
        {
            _Cargando = true;
            try
            {
                if (VentasCboTipoCliente.Items.Count > 0) VentasCboTipoCliente.SelectedIndex = 0;
                if (VentasCboTipoProducto.Items.Count > 0) VentasCboTipoProducto.SelectedIndex = 0;
                if (VentasCboLinea.Items.Count > 0) VentasCboLinea.SelectedIndex = 0;
                if (VentasCboCategoria.Items.Count > 0) VentasCboCategoria.SelectedIndex = 0;
                if (VentasCboProveedores.Items.Count > 0) VentasCboProveedores.SelectedIndex = 0;
                if (VentasCboProducto.Items.Count > 0) VentasCboProducto.SelectedIndex = 0;
                VentasRbtnDescuento.Checked = true;
                VentasRbtnPrdInd.Checked = true;
                VentasNudPorcentajeDescuento.Value = 0;
                VentasTxtLisPre.Text = "0.00";
                VentasDgvListadoClientes.ClearSelection();
            }
            finally
            {
                _Cargando = false;
            }
            VentasMetConfigurarPermisosModificacion();
            VentasMetConfigurarAlcanceGrilla();
            VentasMetAplicarFiltros();
        }

        private void VentasMetBtnGuardarClick(object Origen, EventArgs Evento)
        {
            string tipoMod = VentasRbtnDescuento.Checked ? "Descuento" :
                (VentasRbtnAumPor.Checked ? "AumentoPorcentaje" : "AumentoBase");
            string alcanceMod = VentasRbtnPrdInd.Checked ? "Individual" :
                (VentasRbtnPrdSelec.Checked ? "Seleccionados" : "TodosFiltros");

            decimal valorModificacion;
            if (VentasRbtnAumBase.Checked)
            {
                if (!decimal.TryParse(VentasTxtLisPre.Text, NumberStyles.Number, _Cultura, out valorModificacion) || valorModificacion < 0m)
                {
                    MessageBox.Show("Ingrese un valor base válido y no negativo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    VentasTxtLisPre.Focus();
                    return;
                }
            }
            else valorModificacion = VentasNudPorcentajeDescuento.Value;

            var filas = new List<DataGridViewRow>();
            if (VentasRbtnTdsRslt.Checked)
            {
                foreach (DataGridViewRow fila in VentasDgvListadoClientes.Rows)
                    if (!fila.IsNewRow && fila.Tag is ClsPrecioCliente) filas.Add(fila);
            }
            else
            {
                foreach (DataGridViewRow fila in VentasDgvListadoClientes.SelectedRows)
                    if (!fila.IsNewRow && fila.Tag is ClsPrecioCliente) filas.Add(fila);
            }

            if (filas.Count == 0)
            {
                MessageBox.Show(VentasRbtnTdsRslt.Checked
                    ? "Los filtros no devolvieron productos."
                    : "Seleccione un producto en la tabla. Para varios productos use Ctrl + clic o Shift + clic.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (VentasRbtnPrdInd.Checked && filas.Count != 1)
            {
                MessageBox.Show("En modificación individual debe seleccionar exactamente un producto.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var registros = new List<ClsPrecioCliente>();
            foreach (DataGridViewRow fila in filas)
            {
                ClsPrecioCliente original = fila.Tag as ClsPrecioCliente;
                if (original == null) continue;

                decimal nuevoPrecioBase = original.PrecioBase;
                decimal precioFinal;
                decimal nuevoPorcentaje = original.PorcentajeDescuento;

                if (tipoMod == "AumentoBase")
                {
                    nuevoPrecioBase = original.PrecioBase + valorModificacion;
                    precioFinal = nuevoPrecioBase - (nuevoPrecioBase * (original.PorcentajeDescuento / 100m));
                }
                else
                {
                    precioFinal = _Controlador.VentasFuncCalcularPrecioFinal(original.PrecioBase, valorModificacion, tipoMod);
                    nuevoPorcentaje = original.PrecioBase > 0m
                        ? Math.Round((original.PrecioBase - precioFinal) / original.PrecioBase * 100m, 2)
                        : 0m;
                }

                if (precioFinal < 0m || nuevoPrecioBase < 0m)
                {
                    MessageBox.Show("La modificación produciría un precio negativo para " + original.Producto + ".",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                registros.Add(new ClsPrecioCliente
                {
                    IdListaPrecio = original.IdListaPrecio,
                    IdProducto = original.IdProducto,
                    TipoCliente = original.TipoCliente,
                    Marca = original.Marca,
                    Linea = original.Linea,
                    Categoria = original.Categoria,
                    Proveedor = original.Proveedor,
                    Producto = original.Producto,
                    PrecioBase = nuevoPrecioBase,
                    PrecioFinal = precioFinal,
                    PorcentajeDescuento = nuevoPorcentaje,
                    TipoModificacion = tipoMod,
                    Valor = valorModificacion,
                    AlcanceModificacion = alcanceMod
                });
            }

            DialogResult confirmar = MessageBox.Show(
                "Se modificarán " + registros.Count + " registro(s) con el alcance '" + alcanceMod + "'. ¿Desea continuar?",
                "Confirmar modificación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            string mensaje;
            bool guardado = _Controlador.VentasFuncGuardarVarios(registros, out mensaje);
            MessageBox.Show(mensaje, guardado ? "Modificación guardada" : "Error al guardar",
                MessageBoxButtons.OK, guardado ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (guardado) VentasMetCargarListado();
        }

        private void VentasMetBtnCancelarClick(object Origen, EventArgs Evento)
        {
            DialogResult cancelar = MessageBox.Show("¿Desea cancelar la operación actual?", "Cancelar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cancelar == DialogResult.Yes) VentasMetLimpiarFiltros();
        }

        private void VentasMetFiltrosSelectedIndexChanged(object Origen, EventArgs Evento)
        {
            if (!_Cargando) VentasMetAplicarFiltros();
        }

        private void VentasMetDgvListadoClientesCellClick(object Origen, DataGridViewCellEventArgs Evento)
        {
            if (Evento.RowIndex < 0) return;
            if (VentasRbtnPrdInd.Checked)
            {
                VentasDgvListadoClientes.ClearSelection();
                VentasDgvListadoClientes.Rows[Evento.RowIndex].Selected = true;
            }
            VentasMetActualizarPrecioFinal(null, EventArgs.Empty);
        }
    }
}

// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha de creacion: 18/10/2026 | Fecha de actualizacion: 01/11/2026
