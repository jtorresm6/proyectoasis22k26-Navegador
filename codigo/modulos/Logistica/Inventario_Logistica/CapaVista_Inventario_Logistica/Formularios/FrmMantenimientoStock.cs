
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * FORMULARIO: FrmMantenimientoStock.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA DE REALIZACIÓN: 07/11/2026
 *
 * DESCRIPCIÓN:
 * Formulario MVC para el mantenimiento de Stock.
 * Permite ingresar, consultar, modificar, eliminar, buscar
 * y visualizar existencias por sucursal, bodega y producto.
 *
 * Incluye los mensajes de confirmación, éxito y error
 * establecidos en la estandarización de Logística.
 * ============================================================================
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using CapaControlador_Inventario_Logistica.Modelos_de_Controladores;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoStock : Form
    {
        // ========================================================
        // VARIABLES
        // ========================================================

        private readonly ClsModeloStock _ModeloStock =
            new ClsModeloStock();

        private List<ClsDatosStock> _Todos =
            new List<ClsDatosStock>();

        private List<ClsDatosStock> _Filtrados =
            new List<ClsDatosStock>();

        private bool _ModoEdicion;

        private string _ColumnaOrden = "IdStock";

        private bool _OrdenAscendente = false;

        private bool _CargandoCombos = false;

        private enum ClsTipoDialogo
        {
            Confirmacion,
            Exito,
            Error
        }

        // ========================================================
        // CONSTRUCTOR
        // ========================================================

        public FrmMantenimientoStock()
        {
            InitializeComponent();

            LogisticaProcConfigurarFormulario();

            LogisticaProcConfigurarTabla();

            LogisticaProcVincularEventos();

            LogisticaProcLimpiarCampos();

            LogisticaProcHabilitarEdicion(false);
        }

        // ========================================================
        // CONFIGURAR FORMULARIO
        // ========================================================

        private void LogisticaProcConfigurarFormulario()
        {
            LogisticaTxtIdStock.ReadOnly = true;

            LogisticaTxtIdStock.TabStop = false;

            // Etiquetas de campos automáticos y booleanos.
            LogisticaLblIdStock.Text = "ID:";

            LogisticaLblEstadoStock.Text =
                "Con existencias:";

            LogisticaLblEstadoVenta.Text =
                "Estado de venta:";

            LogisticaLblEstadoCompra.Text =
                "Estado de compra:";

            // Los ComboBox deben seleccionar registros válidos.
            LogisticaCboIdSucursal.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboBodegaStock.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboProductoStock.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // Estado de stock es un indicador calculado.
            LogisticaChkEstadoStock.Enabled = false;

            LogisticaChkEstadoStock.TabStop = false;
        }

        // ========================================================
        // CONFIGURAR DATAGRIDVIEW
        // ========================================================

        private void LogisticaProcConfigurarTabla()
        {
            LogisticaDgvListadoStock.AutoGenerateColumns =
                false;

            LogisticaDgvListadoStock.Columns.Clear();

            LogisticaDgvListadoStock.ReadOnly = true;

            LogisticaDgvListadoStock.AllowUserToAddRows =
                false;

            LogisticaDgvListadoStock.AllowUserToDeleteRows =
                false;

            LogisticaDgvListadoStock.MultiSelect = false;

            LogisticaDgvListadoStock.RowHeadersVisible =
                false;

            LogisticaDgvListadoStock.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // Barra horizontal y vertical.
            LogisticaDgvListadoStock.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            LogisticaDgvListadoStock.ScrollBars =
                ScrollBars.Both;

            LogisticaDgvListadoStock.AllowUserToResizeColumns =
                true;

            // Colores de Logística.
            LogisticaDgvListadoStock.EnableHeadersVisualStyles =
                false;

            LogisticaDgvListadoStock
                .ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(62, 138, 132);

            LogisticaDgvListadoStock
                .ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            LogisticaDgvListadoStock
                .ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);

            LogisticaDgvListadoStock.DefaultCellStyle.Font =
                new Font("Segoe UI", 9F);

            LogisticaDgvListadoStock
                .DefaultCellStyle.BackColor =
                Color.FromArgb(251, 246, 242);

            LogisticaDgvListadoStock
                .AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(242, 216, 197);

            LogisticaDgvListadoStock
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 99, 46);

            LogisticaDgvListadoStock
                .DefaultCellStyle.SelectionForeColor =
                Color.White;

            // ====================================================
            // COLUMNAS DE STOCK
            // ====================================================

            LogisticaProcAgregarColumna(
                "IdStock", "ID", 65);

            LogisticaProcAgregarColumna(
                "NombreSucursal", "Sucursal", 160);

            LogisticaProcAgregarColumna(
                "NombreBodega", "Bodega", 160);

            LogisticaProcAgregarColumna(
                "NombreProducto", "Producto", 210);

            LogisticaProcAgregarColumna(
                "StockFisicoStock", "Físico", 95);

            LogisticaProcAgregarColumna(
                "StockVirtualStock", "Virtual", 95);

            LogisticaProcAgregarColumna(
                "StockAnteriorStock", "Anterior", 95);

            LogisticaProcAgregarColumna(
                "StockActualStock", "Actual", 95);

            LogisticaProcAgregarColumna(
                "CargosMesStock", "Cargos mes", 115);

            LogisticaProcAgregarColumna(
                "AbonosMesStock", "Abonos mes", 115);

            LogisticaProcAgregarColumna(
                "CargosAcumuladosStock",
                "Cargos acumulados", 160);

            LogisticaProcAgregarColumna(
                "AbonosAcumuladosStock",
                "Abonos acumulados", 160);

            LogisticaProcAgregarColumna(
                "ApartadosStock", "Apartados", 105);

            // Columnas booleanas.
            LogisticaProcAgregarBooleano(
                "TieneExistencias", "Con existencias");

            LogisticaProcAgregarBooleano(
                "EstadoVentaStock", "Venta habilitada");

            LogisticaProcAgregarBooleano(
                "EstadoCompraStock", "Compra habilitada");

            LogisticaDgvListadoStock.BringToFront();
        }

        // ========================================================
        // CREAR COLUMNA DE TEXTO
        // ========================================================

        private void LogisticaProcAgregarColumna(
            string Propiedad,
            string Titulo,
            int Ancho)
        {
            LogisticaDgvListadoStock.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = Propiedad,
                    DataPropertyName = Propiedad,
                    HeaderText = Titulo,
                    Width = Ancho,

                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.None,

                    SortMode =
                        DataGridViewColumnSortMode.Programmatic
                });
        }

        // ========================================================
        // CREAR COLUMNA BOOLEANO
        // ========================================================

        private void LogisticaProcAgregarBooleano(
            string Propiedad,
            string Titulo)
        {
            LogisticaDgvListadoStock.Columns.Add(
                new DataGridViewCheckBoxColumn
                {
                    Name = Propiedad,
                    DataPropertyName = Propiedad,
                    HeaderText = Titulo,
                    ValueType = typeof(bool),

                    TrueValue = true,
                    FalseValue = false,

                    ThreeState = false,
                    ReadOnly = true,
                    Width = 130,

                    SortMode =
                        DataGridViewColumnSortMode.Programmatic
                });
        }

        // ========================================================
        // VINCULAR EVENTOS
        // ========================================================

        private void LogisticaProcVincularEventos()
        {
            Load += FrmMantenimientoStock_Load;

            LogisticaBtnIngresarStock.Click +=
                LogisticaBtnIngresarStock_Click;

            LogisticaBtnConsultarStock.Click +=
                LogisticaBtnConsultarStock_Click;

            LogisticaBtnModificarStock.Click +=
                LogisticaBtnModificarStock_Click;

            LogisticaBtnGuardarStock.Click +=
                LogisticaBtnGuardarStock_Click;

            LogistiaBtnEliminarStock.Click +=
                LogistiaBtnEliminarStock_Click;

            LogisticaBtnRefrescarStock.Click +=
                LogisticaBtnRefrescarStock_Click;

            LogisticaBtnImprimirStock.Click +=
                LogisticaBtnImprimirStock_Click;

            LogisticaBtnAyudaStock.Click +=
                LogisticaBtnAyudaStock_Click;

            LogisticaCboIdSucursal.SelectedIndexChanged +=
                LogisticaCboIdSucursal_SelectedIndexChanged;

            LogisticaTxtStockActual.TextChanged +=
                LogisticaTxtStockActual_TextChanged;

            LogisticaTxtBusquedaInteligente.TextChanged +=
                LogisticaTxtBusquedaInteligente_TextChanged;

            LogisticaDgvListadoStock.CellDoubleClick +=
                LogisticaDgvListadoStock_CellDoubleClick;

            LogisticaDgvListadoStock.ColumnHeaderMouseClick +=
                LogisticaDgvListadoStock_ColumnHeaderMouseClick;
        }

        // ========================================================
        // CARGA INICIAL
        // ========================================================

        private void FrmMantenimientoStock_Load(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcCargarCatalogos();

                LogisticaProcRecargarDatos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ========================================================
        // CARGAR CATÁLOGOS
        // ========================================================

        private void LogisticaProcCargarCatalogos()
        {
            _CargandoCombos = true;

            try
            {
                LogisticaProcCargarCombo(
                    LogisticaCboIdSucursal,
                    _ModeloStock.LogisticaFuncSucursales());

                LogisticaProcCargarCombo(
                    LogisticaCboBodegaStock,
                    new List<ClsOpcionCatalogo>());

                LogisticaProcCargarCombo(
                    LogisticaCboProductoStock,
                    _ModeloStock.LogisticaFuncProductos());
            }
            finally
            {
                _CargandoCombos = false;
            }
        }

        // ========================================================
        // CONFIGURAR COMBOBOX
        // ========================================================

        private void LogisticaProcCargarCombo(
            ComboBox Combo,
            List<ClsOpcionCatalogo> Datos)
        {
            Combo.DataSource = null;

            Combo.DisplayMember = "Nombre";

            Combo.ValueMember = "Id";

            Combo.DataSource = Datos;

            Combo.SelectedIndex = -1;
        }

        // ========================================================
        // CARGAR BODEGAS SEGÚN SUCURSAL
        // ========================================================

        private void LogisticaCboIdSucursal_SelectedIndexChanged(
            object Sender,
            EventArgs E)
        {
            if (_CargandoCombos)
            {
                return;
            }

            try
            {
                int IdSucursal =
                    LogisticaFuncObtenerIdCombo(
                        LogisticaCboIdSucursal);

                if (IdSucursal > 0)
                {
                    LogisticaProcCargarCombo(
                        LogisticaCboBodegaStock,
                        _ModeloStock.LogisticaFuncBodegas(
                            IdSucursal));
                }
                else
                {
                    LogisticaProcCargarCombo(
                        LogisticaCboBodegaStock,
                        new List<ClsOpcionCatalogo>());
                }
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ========================================================
        // OBTENER ID SELECCIONADO
        // ========================================================

        private int LogisticaFuncObtenerIdCombo(
            ComboBox Combo)
        {
            int Id;

            if (Combo.SelectedValue != null &&
                int.TryParse(
                    Convert.ToString(Combo.SelectedValue),
                    out Id))
            {
                return Id;
            }

            return 0;
        }

        // ========================================================
        // INDICADOR DE EXISTENCIAS
        // ========================================================

        private void LogisticaTxtStockActual_TextChanged(
            object Sender,
            EventArgs E)
        {
            int Actual;

            LogisticaChkEstadoStock.Checked =
                int.TryParse(
                    LogisticaTxtStockActual.Text,
                    out Actual) &&
                Actual > 0;
        }

        // ========================================================
        // RECARGAR REGISTROS
        // ========================================================

        private void LogisticaProcRecargarDatos()
        {
            _Todos =
                _ModeloStock.LogisticaFuncObtenerTodos();

            LogisticaProcFiltrarDatos();
        }

        // ========================================================
        // BÚSQUEDA INTELIGENTE
        // ========================================================

        private void LogisticaProcFiltrarDatos()
        {
            string Buscar =
                LogisticaTxtBusquedaInteligente.Text.Trim();

            _Filtrados = _Todos.Where(D =>
                D.IdStock.ToString().Contains(Buscar) ||

                (D.NombreSucursal ?? "").IndexOf(
                    Buscar,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (D.NombreBodega ?? "").IndexOf(
                    Buscar,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (D.NombreProducto ?? "").IndexOf(
                    Buscar,
                    StringComparison.OrdinalIgnoreCase) >= 0

            ).ToList();

            LogisticaProcOrdenarDatos();

            LogisticaDgvListadoStock.DataSource = null;

            LogisticaDgvListadoStock.DataSource = _Filtrados;

            LogisticaDgvListadoStock.ClearSelection();
        }

        // ========================================================
        // EVENTO BÚSQUEDA
        // ========================================================

        private void LogisticaTxtBusquedaInteligente_TextChanged(
            object Sender,
            EventArgs E)
        {
            LogisticaProcFiltrarDatos();
        }

        // ========================================================
        // ORDENAR DATOS
        // ========================================================

        private void LogisticaProcOrdenarDatos()
        {
            PropertyInfo Propiedad =
                typeof(ClsDatosStock).GetProperty(
                    _ColumnaOrden);

            if (Propiedad == null)
            {
                return;
            }

            _Filtrados.Sort((A, B) =>
            {
                int Comparacion =
                    Comparer.DefaultInvariant.Compare(
                        Propiedad.GetValue(A, null),
                        Propiedad.GetValue(B, null));

                return _OrdenAscendente
                    ? Comparacion
                    : -Comparacion;
            });
        }

        // ========================================================
        // ORDENAR POR ENCABEZADO
        // ========================================================

        private void LogisticaDgvListadoStock_ColumnHeaderMouseClick(
            object Sender,
            DataGridViewCellMouseEventArgs E)
        {
            if (E.ColumnIndex < 0)
            {
                return;
            }

            string Columna =
                LogisticaDgvListadoStock.Columns[
                    E.ColumnIndex].DataPropertyName;

            if (string.IsNullOrEmpty(Columna))
            {
                return;
            }

            _OrdenAscendente =
                _ColumnaOrden == Columna
                    ? !_OrdenAscendente
                    : true;

            _ColumnaOrden = Columna;

            LogisticaProcFiltrarDatos();
        }

        // ========================================================
        // OBTENER STOCK SELECCIONADO
        // ========================================================

        private ClsDatosStock LogisticaFuncStockSeleccionado()
        {
            if (LogisticaDgvListadoStock.SelectedRows.Count == 0)
            {
                throw new ValidationException(
                    "Seleccione un registro del listado.");
            }

            ClsDatosStock Datos =
                LogisticaDgvListadoStock.SelectedRows[0]
                    .DataBoundItem as ClsDatosStock;

            if (Datos == null)
            {
                throw new ValidationException(
                    "No se pudo seleccionar el registro.");
            }

            return Datos;
        }

        // ========================================================
        // DOBLE CLIC EN LA TABLA
        // ========================================================

        private void LogisticaDgvListadoStock_CellDoubleClick(
            object Sender,
            DataGridViewCellEventArgs E)
        {
            if (E.RowIndex < 0)
            {
                return;
            }

            try
            {
                LogisticaDgvListadoStock.ClearSelection();

                LogisticaDgvListadoStock.Rows[E.RowIndex]
                    .Selected = true;

                LogisticaProcCargarSeleccionado();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ========================================================
        // CARGAR STOCK SELECCIONADO
        // ========================================================

        private void LogisticaProcCargarSeleccionado()
        {
            ClsDatosStock D =
                LogisticaFuncStockSeleccionado();

            _ModeloStock.Estado =
                ClsEstadoEntidad.Modified;

            LogisticaProcHabilitarEdicion(true);

            LogisticaTxtIdStock.Text =
                D.IdStock.ToString();

            // Seleccionar sucursal recarga las bodegas.
            LogisticaCboIdSucursal.SelectedValue =
                D.FkSucursalCodigo;

            LogisticaCboBodegaStock.SelectedValue =
                D.FkBodegaCodigo;

            LogisticaCboProductoStock.SelectedValue =
                D.FkProductoCodigo;

            LogisticaTxtStockFisico.Text =
                D.StockFisicoStock.ToString();

            LogisticaTxtStockVirtual.Text =
                D.StockVirtualStock.ToString();

            LogisticaTxtAnteriorStock.Text =
                D.StockAnteriorStock.ToString();

            LogisticaTxtStockActual.Text =
                D.StockActualStock.ToString();

            LogisticaTxtCargosMes.Text =
                D.CargosMesStock.ToString();

            LogisticaTxtAbonosMes.Text =
                D.AbonosMesStock.ToString();

            LogisticaTxtCargosAcumulados.Text =
                D.CargosAcumuladosStock.ToString();

            LogisticaTxtAbonosAcumulados.Text =
                D.AbonosAcumuladosStock.ToString();

            LogisticaTxtApartadosStock.Text =
                D.ApartadosStock.ToString();

            LogisticaChkEstadoVenta.Checked =
                D.EstadoVentaStock;

            LogisticaChkEstadoCompra.Checked =
                D.EstadoCompraStock;

            LogisticaChkEstadoStock.Checked =
                D.TieneExistencias;
        }

        // ========================================================
        // BOTÓN INGRESAR
        // ========================================================

        private void LogisticaBtnIngresarStock_Click(
            object Sender,
            EventArgs E)
        {
            LogisticaProcLimpiarCampos();

            LogisticaDgvListadoStock.ClearSelection();

            _ModeloStock.Estado =
                ClsEstadoEntidad.Added;

            LogisticaProcHabilitarEdicion(true);

            LogisticaCboIdSucursal.Focus();
        }

        // ========================================================
        // BOTÓN CONSULTAR
        // ========================================================

        private void LogisticaBtnConsultarStock_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcRecargarDatos();

                LogisticaTxtBusquedaInteligente.Focus();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ========================================================
        // BOTÓN MODIFICAR
        // ========================================================

        private void LogisticaBtnModificarStock_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcCargarSeleccionado();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ========================================================
        // VALIDAR CAMPOS COMPLETOS
        // ========================================================

        private bool LogisticaFuncCamposCompletos()
        {
            if (
                LogisticaFuncObtenerIdCombo(
                    LogisticaCboIdSucursal) <= 0 ||

                LogisticaFuncObtenerIdCombo(
                    LogisticaCboBodegaStock) <= 0 ||

                LogisticaFuncObtenerIdCombo(
                    LogisticaCboProductoStock) <= 0)
            {
                return false;
            }

            TextBox[] Campos =
            {
                LogisticaTxtStockFisico,
                LogisticaTxtStockVirtual,
                LogisticaTxtAnteriorStock,
                LogisticaTxtStockActual,
                LogisticaTxtCargosMes,
                LogisticaTxtAbonosMes,
                LogisticaTxtCargosAcumulados,
                LogisticaTxtAbonosAcumulados,
                LogisticaTxtApartadosStock
            };

            return Campos.All(C =>
                !string.IsNullOrWhiteSpace(C.Text));
        }

        // ========================================================
        // VALIDAR ENTEROS
        // ========================================================

        private int LogisticaFuncLeerEntero(
            TextBox Campo,
            string Nombre)
        {
            int Valor;

            if (!int.TryParse(
                    Campo.Text.Trim(),
                    out Valor) ||
                Valor < 0)
            {
                throw new ValidationException(
                    Nombre +
                    " debe ser un entero mayor o igual a cero.");
            }

            return Valor;
        }

        // ========================================================
        // VALIDAR ENTEROS LARGOS
        // ========================================================

        private long LogisticaFuncLeerEnteroLargo(
            TextBox Campo,
            string Nombre)
        {
            long Valor;

            if (!long.TryParse(
                    Campo.Text.Trim(),
                    out Valor) ||
                Valor < 0)
            {
                throw new ValidationException(
                    Nombre +
                    " debe ser un entero mayor o igual a cero.");
            }

            return Valor;
        }

        // ========================================================
        // LEER FORMULARIO
        // ========================================================

        private ClsDatosStock LogisticaFuncLeerFormulario()
        {
            if (!LogisticaFuncCamposCompletos())
            {
                throw new ValidationException(
                    "Completar todos los campos.");
            }

            int IdStock = 0;

            if (_ModeloStock.Estado ==
                ClsEstadoEntidad.Modified)
            {
                if (!int.TryParse(
                        LogisticaTxtIdStock.Text,
                        out IdStock) ||
                    IdStock <= 0)
                {
                    throw new ValidationException(
                        "Seleccione un stock válido para modificar.");
                }
            }

            return new ClsDatosStock
            {
                IdStock = IdStock,

                FkSucursalCodigo =
                    LogisticaFuncObtenerIdCombo(
                        LogisticaCboIdSucursal),

                FkBodegaCodigo =
                    LogisticaFuncObtenerIdCombo(
                        LogisticaCboBodegaStock),

                FkProductoCodigo =
                    LogisticaFuncObtenerIdCombo(
                        LogisticaCboProductoStock),

                StockFisicoStock =
                    LogisticaFuncLeerEntero(
                        LogisticaTxtStockFisico,
                        "Stock físico"),

                StockVirtualStock =
                    LogisticaFuncLeerEntero(
                        LogisticaTxtStockVirtual,
                        "Stock virtual"),

                StockAnteriorStock =
                    LogisticaFuncLeerEntero(
                        LogisticaTxtAnteriorStock,
                        "Stock anterior"),

                StockActualStock =
                    LogisticaFuncLeerEntero(
                        LogisticaTxtStockActual,
                        "Stock actual"),

                CargosMesStock =
                    LogisticaFuncLeerEntero(
                        LogisticaTxtCargosMes,
                        "Cargos del mes"),

                AbonosMesStock =
                    LogisticaFuncLeerEntero(
                        LogisticaTxtAbonosMes,
                        "Abonos del mes"),

                CargosAcumuladosStock =
                    LogisticaFuncLeerEnteroLargo(
                        LogisticaTxtCargosAcumulados,
                        "Cargos acumulados"),

                AbonosAcumuladosStock =
                    LogisticaFuncLeerEnteroLargo(
                        LogisticaTxtAbonosAcumulados,
                        "Abonos acumulados"),

                ApartadosStock =
                    LogisticaFuncLeerEntero(
                        LogisticaTxtApartadosStock,
                        "Apartados"),

                EstadoVentaStock =
                    LogisticaChkEstadoVenta.Checked,

                EstadoCompraStock =
                    LogisticaChkEstadoCompra.Checked
            };
        }

        // ========================================================
        // BOTÓN GUARDAR
        // ========================================================

        private void LogisticaBtnGuardarStock_Click(
            object Sender,
            EventArgs E)
        {
            if (!_ModoEdicion)
            {
                LogisticaProcMostrarError(
                    "Primero presione Ingresar o Modificar.");

                return;
            }

            if (!LogisticaFuncCamposCompletos())
            {
                LogisticaProcMostrarError(
                    "Completar todos los campos.");

                return;
            }

            try
            {
                _ModeloStock.Stock =
                    LogisticaFuncLeerFormulario();

                string Mensaje =
                    _ModeloStock.LogisticaFuncGrabarCambios();

                LogisticaProcMostrarExito(Mensaje);

                LogisticaProcLimpiarCampos();

                LogisticaProcHabilitarEdicion(false);

                LogisticaProcRecargarDatos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ========================================================
        // BOTÓN ELIMINAR
        // ========================================================

        private void LogistiaBtnEliminarStock_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                ClsDatosStock D =
                    LogisticaFuncStockSeleccionado();

                bool Confirmado =
                    LogisticaFuncConfirmarEliminacion(
                        "¿Eliminar el stock ID " +
                        D.IdStock +
                        "? Esta acción no se puede deshacer.");

                if (!Confirmado)
                {
                    return;
                }

                _ModeloStock.Estado =
                    ClsEstadoEntidad.Deleted;

                _ModeloStock.Stock =
                    new ClsDatosStock
                    {
                        IdStock = D.IdStock
                    };

                string Mensaje =
                    _ModeloStock.LogisticaFuncGrabarCambios();

                LogisticaProcMostrarExito(Mensaje);

                LogisticaProcLimpiarCampos();

                LogisticaProcHabilitarEdicion(false);

                LogisticaProcRecargarDatos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ========================================================
        // BOTÓN REFRESCAR
        // ========================================================

        private void LogisticaBtnRefrescarStock_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcLimpiarCampos();

                LogisticaProcHabilitarEdicion(false);

                LogisticaProcCargarCatalogos();

                LogisticaTxtBusquedaInteligente.Clear();

                LogisticaProcRecargarDatos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ========================================================
        // LIMPIAR CAMPOS
        // ========================================================

        private void LogisticaProcLimpiarCampos()
        {
            TextBox[] Campos =
            {
                LogisticaTxtIdStock,
                LogisticaTxtStockFisico,
                LogisticaTxtStockVirtual,
                LogisticaTxtAnteriorStock,
                LogisticaTxtStockActual,
                LogisticaTxtCargosMes,
                LogisticaTxtAbonosMes,
                LogisticaTxtCargosAcumulados,
                LogisticaTxtAbonosAcumulados,
                LogisticaTxtApartadosStock
            };

            foreach (TextBox Campo in Campos)
            {
                Campo.Clear();
            }

            LogisticaCboIdSucursal.SelectedIndex = -1;

            LogisticaCboBodegaStock.SelectedIndex = -1;

            LogisticaCboProductoStock.SelectedIndex = -1;

            LogisticaChkEstadoStock.Checked = false;

            LogisticaChkEstadoVenta.Checked = false;

            LogisticaChkEstadoCompra.Checked = false;
        }

        // ========================================================
        // HABILITAR EDICIÓN
        // ========================================================

        private void LogisticaProcHabilitarEdicion(
            bool Habilitar)
        {
            _ModoEdicion = Habilitar;

            LogistinaPnlDatosMovimiento.Enabled = Habilitar;

            LogisticaBtnGuardarStock.Enabled = Habilitar;

            // El estado se calcula automáticamente.
            LogisticaChkEstadoStock.Enabled = false;
        }

        // ========================================================
        // IMPRIMIR Y AYUDA
        // ========================================================

        private void LogisticaBtnImprimirStock_Click(
            object Sender,
            EventArgs E)
        {
            MessageBox.Show(
                "El reporte RDLC de Stock está pendiente " +
                "de integrar.",
                "Logística - Información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void LogisticaBtnAyudaStock_Click(
            object Sender,
            EventArgs E)
        {
            MessageBox.Show(
                "La ayuda CHM de Stock está pendiente " +
                "de integrar.",
                "Logística - Información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ========================================================
        // CONFIRMACIÓN, ÉXITO Y ERROR
        // ========================================================

        private bool LogisticaFuncConfirmarEliminacion(
            string Mensaje)
        {
            return LogisticaFuncMostrarDialogo(
                ClsTipoDialogo.Confirmacion,
                "Confirmar eliminación",
                Mensaje) == DialogResult.Yes;
        }

        private void LogisticaProcMostrarExito(
            string Mensaje)
        {
            LogisticaFuncMostrarDialogo(
                ClsTipoDialogo.Exito,
                "Operación exitosa",
                Mensaje);
        }

        private void LogisticaProcMostrarError(
            string Mensaje)
        {
            Debug.WriteLine(
                "LOGÍSTICA - STOCK: " + Mensaje);

            if (string.IsNullOrWhiteSpace(Mensaje))
            {
                Mensaje =
                    "No se pudo completar la operación.";
            }

            if (Mensaje.Length > 160)
            {
                Mensaje =
                    "No se pudo completar la operación. " +
                    "Revise los datos o la conexión.";
            }

            LogisticaFuncMostrarDialogo(
                ClsTipoDialogo.Error,
                "Ocurrió un error",
                Mensaje);
        }

        // ========================================================
        // DIÁLOGOS COMPACTOS DEL ESTÁNDAR LOGÍSTICA
        // ========================================================

        private DialogResult LogisticaFuncMostrarDialogo(
            ClsTipoDialogo Tipo,
            string Titulo,
            string Mensaje)
        {
            Color ColorIcono =
                Tipo == ClsTipoDialogo.Confirmacion
                    ? Color.FromArgb(236, 171, 42)
                    : Tipo == ClsTipoDialogo.Exito
                        ? Color.FromArgb(45, 158, 93)
                        : Color.FromArgb(218, 62, 70);

            string Simbolo =
                Tipo == ClsTipoDialogo.Confirmacion
                    ? "!"
                    : Tipo == ClsTipoDialogo.Exito
                        ? "✓"
                        : "×";

            using (Form Ventana = new Form())
            {
                Ventana.Text = Titulo;

                Ventana.ClientSize = new Size(430, 185);

                Ventana.FormBorderStyle =
                    FormBorderStyle.FixedDialog;

                Ventana.StartPosition =
                    FormStartPosition.CenterParent;

                Ventana.MaximizeBox = false;
                Ventana.MinimizeBox = false;
                Ventana.ShowIcon = false;
                Ventana.ShowInTaskbar = false;
                Ventana.BackColor = Color.White;

                // Ícono circular.
                PictureBox Icono = new PictureBox
                {
                    Location = new Point(24, 60),
                    Size = new Size(48, 48)
                };

                Icono.Paint += (S, E) =>
                {
                    E.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    using (SolidBrush P =
                        new SolidBrush(ColorIcono))
                    {
                        E.Graphics.FillEllipse(
                            P, 2, 2, 44, 44);
                    }

                    using (Font F =
                        new Font(
                            "Segoe UI Symbol",
                            20F,
                            FontStyle.Bold))
                    {
                        TextRenderer.DrawText(
                            E.Graphics,
                            Simbolo,
                            F,
                            new Rectangle(2, 2, 44, 44),
                            Color.White,
                            TextFormatFlags.HorizontalCenter |
                            TextFormatFlags.VerticalCenter);
                    }
                };

                // Título.
                Label LblTitulo = new Label
                {
                    Text = Titulo,
                    Location = new Point(90, 22),
                    Size = new Size(320, 30),
                    ForeColor = ColorIcono,

                    Font = new Font(
                        "Segoe UI",
                        13F,
                        FontStyle.Bold)
                };

                // Mensaje sin barras de desplazamiento.
                Label LblMensaje = new Label
                {
                    Text = Mensaje,
                    Location = new Point(90, 55),
                    Size = new Size(315, 77),
                    ForeColor = Color.FromArgb(60, 60, 60),
                    Font = new Font("Segoe UI", 9.5F),
                    AutoEllipsis = true,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

                // Botón principal.
                Button Principal = new Button
                {
                    Location = new Point(295, 143),
                    Size = new Size(115, 30),

                    BackColor =
                        Color.FromArgb(47, 49, 58),

                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,

                    Font = new Font(
                        "Segoe UI",
                        9F,
                        FontStyle.Bold)
                };

                Principal.FlatAppearance.BorderSize = 0;

                if (Tipo == ClsTipoDialogo.Confirmacion)
                {
                    Principal.Text = "Sí, eliminar";

                    Principal.DialogResult =
                        DialogResult.Yes;

                    Button No = new Button
                    {
                        Text = "No",
                        Location = new Point(195, 143),
                        Size = new Size(90, 30),
                        BackColor = Color.White,

                        ForeColor =
                            Color.FromArgb(47, 49, 58),

                        FlatStyle = FlatStyle.Flat,

                        DialogResult = DialogResult.No
                    };

                    Ventana.Controls.Add(No);

                    Ventana.AcceptButton = No;

                    Ventana.CancelButton = No;
                }
                else
                {
                    Principal.Text = "Aceptar";

                    Principal.DialogResult =
                        DialogResult.OK;

                    Ventana.AcceptButton = Principal;

                    Ventana.CancelButton = Principal;
                }

                Ventana.Controls.Add(Icono);
                Ventana.Controls.Add(LblTitulo);
                Ventana.Controls.Add(LblMensaje);
                Ventana.Controls.Add(Principal);

                return Ventana.ShowDialog(this);
            }
        }
    }
}
