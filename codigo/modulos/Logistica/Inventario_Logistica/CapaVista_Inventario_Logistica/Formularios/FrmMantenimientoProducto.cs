
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * FORMULARIO: FrmMantenimientoProducto.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA DE REALIZACIÓN: 06/11/2026
 *
 * DESCRIPCIÓN:
 * Formulario para el mantenimiento de productos.
 * Permite ingresar, consultar, modificar y eliminar productos,
 * cargar catálogos, seleccionar imágenes y consultar registros
 * mediante búsqueda inteligente y paginación.
 *
 * Implementa mensajes compactos de confirmación, éxito y error
 * conforme a la estandarización de Logística.
 * ============================================================================
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using CapaControlador_Inventario_Logistica.Modelos_de_Controladores;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoProducto : Form
    {
        // ============================================================
        // VARIABLES
        // ============================================================

        private readonly ClsModeloProducto _ModeloProducto =
            new ClsModeloProducto();

        private List<ClsDatosProducto> _Productos =
            new List<ClsDatosProducto>();

        private List<ClsDatosProducto> _Filtrados =
            new List<ClsDatosProducto>();

        private byte[] _ImagenProducto;

        private bool _ModoEdicion = false;

        private int _PaginaActual = 1;

        private const int _TamanoPagina = 10;

        private string _ColumnaOrden = "IdProducto";

        private bool _OrdenAscendente = false;

        // Tipos de diálogos.
        private enum ClsTipoDialogo
        {
            Confirmacion,
            Exito,
            Error
        }

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmMantenimientoProducto()
        {
            InitializeComponent();

            LogisticaProcConfigurarFormulario();

            LogisticaProcConfigurarTabla();

            LogisticaProcVincularEventos();

            LogisticaProcLimpiarCampos();

            LogisticaProcHabilitarEdicion(false);
        }

        // ============================================================
        // CONFIGURAR FORMULARIO
        // ============================================================

        private void LogisticaProcConfigurarFormulario()
        {
            LogisticaTxtIdProducto.ReadOnly = true;

            LogisticaTxtIdProducto.TabStop = false;

            LogisticaTxtSKUProducto.MaxLength = 30;

            LogisticaTxtClasificacionProducto.MaxLength = 5;

            LogisticaTxtNombreProducto.MaxLength = 100;

            LogisticaTxtDescripcionProducto.MaxLength = 255;

            LogisticaTxtPaisProducto.MaxLength = 100;

            // Configurar ComboBox.
            LogisticaCboCategoria.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboMarca.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboLinea.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboProveedor.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // Imagen.
            LogisticaPbImagenProducto.SizeMode =
                PictureBoxSizeMode.Zoom;

            // Los estilos, tamaños y posiciones restantes
            // se conservan desde el Designer.
        }

        // ============================================================
        // CONFIGURAR DATAGRIDVIEW
        // ============================================================

        private void LogisticaProcConfigurarTabla()
        {
            LogisticaDgvProducto.AutoGenerateColumns = false;

            LogisticaDgvProducto.Columns.Clear();

            LogisticaDgvProducto.ReadOnly = true;

            LogisticaDgvProducto.AllowUserToAddRows = false;

            LogisticaDgvProducto.AllowUserToDeleteRows = false;

            LogisticaDgvProducto.AllowUserToResizeColumns = true;

            LogisticaDgvProducto.MultiSelect = false;

            LogisticaDgvProducto.RowHeadersVisible = false;

            LogisticaDgvProducto.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // Desplazamiento horizontal y vertical.
            LogisticaDgvProducto.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            LogisticaDgvProducto.ScrollBars =
                ScrollBars.Both;

            // Estandarización de colores.
            LogisticaDgvProducto.BackgroundColor =
                Color.FromArgb(251, 246, 242);

            LogisticaDgvProducto.EnableHeadersVisualStyles =
                false;

            LogisticaDgvProducto.ColumnHeadersHeight = 32;

            LogisticaDgvProducto.RowTemplate.Height = 27;

            LogisticaDgvProducto
                .ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(62, 138, 132);

            LogisticaDgvProducto
                .ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            LogisticaDgvProducto
                .ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI", 9F, FontStyle.Bold);

            LogisticaDgvProducto
                .DefaultCellStyle.BackColor =
                Color.FromArgb(251, 246, 242);

            LogisticaDgvProducto
                .AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(242, 216, 197);

            LogisticaDgvProducto
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 99, 46);

            LogisticaDgvProducto
                .DefaultCellStyle.SelectionForeColor =
                Color.White;

            // ========================================================
            // TEXTOS Y COLUMNAS DEL DATAGRIDVIEW
            // ========================================================

            LogisticaProcAgregarColumna(
                "IdProducto", "ID", 70);

            LogisticaProcAgregarColumna(
                "SkuProducto", "SKU", 130);

            LogisticaProcAgregarColumna(
                "NombreProducto", "Nombre", 200);

            LogisticaProcAgregarColumna(
                "ClasificacionProducto", "Clasificación", 130);

            LogisticaProcAgregarColumna(
                "PaisProducto", "País", 150);

            LogisticaProcAgregarColumna(
                "NombreCategoria", "Categoría", 160);

            LogisticaProcAgregarColumna(
                "NombreMarca", "Marca", 160);

            LogisticaProcAgregarColumna(
                "NombreLinea", "Línea", 160);

            LogisticaProcAgregarColumna(
                "NombreProveedor", "Proveedor", 220);

            LogisticaProcAgregarColumna(
                "PesoProducto", "Peso", 100);

            LogisticaProcAgregarColumna(
                "PrecioCostoProducto", "Precio de costo (Q)", 160);

            LogisticaProcAgregarColumna(
                "PrecioVentaProducto", "Precio de venta (Q)", 160);

            LogisticaProcAgregarColumna(
                "StockMinimoProducto", "Stock mínimo", 130);

            LogisticaProcAgregarColumna(
                "StockMaximoProducto", "Stock máximo", 130);

            LogisticaProcAgregarColumna(
                "PorcentajeComisionProducto", "Comisión", 120);

            LogisticaProcAgregarColumna(
                "DescripcionProducto", "Descripción", 250);

            // ========================================================
            // IMAGEN COMO BOOLEANO
            // ========================================================

            DataGridViewCheckBoxColumn ColumnaImagen =
                new DataGridViewCheckBoxColumn();

            ColumnaImagen.Name = "TieneImagen";

            ColumnaImagen.HeaderText = "Imagen";

            ColumnaImagen.DataPropertyName = "TieneImagen";

            ColumnaImagen.ValueType = typeof(bool);

            ColumnaImagen.TrueValue = true;

            ColumnaImagen.FalseValue = false;

            ColumnaImagen.ThreeState = false;

            ColumnaImagen.ReadOnly = true;

            ColumnaImagen.Width = 100;

            ColumnaImagen.SortMode =
                DataGridViewColumnSortMode.Programmatic;

            LogisticaDgvProducto.Columns.Add(ColumnaImagen);

            // Formatos.
            LogisticaDgvProducto.Columns["PesoProducto"]
                .DefaultCellStyle.Format = "N2";

            LogisticaDgvProducto.Columns["PrecioCostoProducto"]
                .DefaultCellStyle.Format = "N2";

            LogisticaDgvProducto.Columns["PrecioVentaProducto"]
                .DefaultCellStyle.Format = "N2";

            LogisticaDgvProducto.Columns["PorcentajeComisionProducto"]
                .DefaultCellStyle.Format = "N2";
        }

        // ============================================================
        // AGREGAR COLUMNA
        // ============================================================

        private void LogisticaProcAgregarColumna(
            string Propiedad,
            string Titulo,
            int Ancho)
        {
            DataGridViewTextBoxColumn Columna =
                new DataGridViewTextBoxColumn();

            Columna.Name = Propiedad;

            Columna.DataPropertyName = Propiedad;

            Columna.HeaderText = Titulo;

            Columna.Width = Ancho;

            Columna.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.None;

            Columna.SortMode =
                DataGridViewColumnSortMode.Programmatic;

            LogisticaDgvProducto.Columns.Add(Columna);
        }

        // ============================================================
        // VINCULAR EVENTOS
        // ============================================================

        private void LogisticaProcVincularEventos()
        {
            Load += FrmMantenimientoProducto_Load;

            LogisticaBtnIngresarProducto.Click +=
                LogisticaBtnIngresarProducto_Click;

            LogisticaBtnConsultarProducto.Click +=
                LogisticaBtnConsultarProducto_Click;

            LogisticaBtnModificarProducto.Click +=
                LogisticaBtnModificarProducto_Click;

            LogisticaBtnGuardarProducto.Click +=
                LogisticaBtnGuardarProducto_Click;

            LogisticaBtnEliminarProducto.Click +=
                LogisticaBtnEliminarProducto_Click;

            LogisticaBtnRefrescarProducto.Click +=
                LogisticaBtnRefrescarProducto_Click;

            LogisticaBtnSeleccionarImagen.Click +=
                LogisticaBtnSeleccionarImagen_Click;

            LogisticaBtnImprimirProducto.Click +=
                LogisticaBtnImprimirProducto_Click;

            LogisticaBtnAyudaProducto.Click +=
                LogisticaBtnAyudaProducto_Click;

            // Buscar.
            LogisticaTxtBuscarInteligenteProducto.TextChanged +=
                LogisticaTxtBuscarInteligenteProducto_TextChanged;

            // Paginación.
            LogisticaBtrnAnteriorProducto.Click +=
                LogisticaBtnAnteriorProducto_Click;

            LogisticaBtnSiguiente.Click +=
                LogisticaBtnSiguienteProducto_Click;

            // Tabla.
            LogisticaDgvProducto.CellDoubleClick +=
                LogisticaDgvProducto_CellDoubleClick;

            LogisticaDgvProducto.ColumnHeaderMouseClick +=
                LogisticaDgvProducto_ColumnHeaderMouseClick;
        }

        // ============================================================
        // CARGA INICIAL
        // ============================================================

        private void FrmMantenimientoProducto_Load(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcCargarCatalogos();

                LogisticaProcRecargarProductos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // CARGAR CATÁLOGOS
        // ============================================================

        private void LogisticaProcCargarCatalogos()
        {
            LogisticaProcCargarCombo(
                LogisticaCboCategoria,
                _ModeloProducto.LogisticaFuncCategorias());

            LogisticaProcCargarCombo(
                LogisticaCboMarca,
                _ModeloProducto.LogisticaFuncMarcas());

            LogisticaProcCargarCombo(
                LogisticaCboLinea,
                _ModeloProducto.LogisticaFuncLineas());

            LogisticaProcCargarCombo(
                LogisticaCboProveedor,
                _ModeloProducto.LogisticaFuncProveedores());
        }

        private void LogisticaProcCargarCombo(
            ComboBox Combo,
            List<ClsOpcionCatalogo> Opciones)
        {
            Combo.DataSource = null;

            Combo.DisplayMember = "Nombre";

            Combo.ValueMember = "Id";

            Combo.DataSource = Opciones;

            Combo.SelectedIndex = -1;
        }

        // ============================================================
        // RECARGAR PRODUCTOS
        // ============================================================

        private void LogisticaProcRecargarProductos()
        {
            _Productos =
                _ModeloProducto.LogisticaFuncObtenerTodos();

            LogisticaProcFiltrarProductos();
        }

        // ============================================================
        // FILTRAR PRODUCTOS
        // ============================================================

        private void LogisticaProcFiltrarProductos()
        {
            string Busqueda =
                LogisticaTxtBuscarInteligenteProducto.Text.Trim();

            _Filtrados = _Productos.Where(P =>
                P.IdProducto.ToString().Contains(Busqueda) ||

                (P.SkuProducto ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.NombreProducto ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.ClasificacionProducto ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.PaisProducto ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.NombreCategoria ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.NombreMarca ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.NombreLinea ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.NombreProveedor ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.DescripcionProducto ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            LogisticaProcOrdenarProductos();

            _PaginaActual = 1;

            LogisticaProcMostrarPagina();
        }

        private void LogisticaTxtBuscarInteligenteProducto_TextChanged(
            object Sender,
            EventArgs E)
        {
            LogisticaProcFiltrarProductos();
        }

        // ============================================================
        // ORDENAR PRODUCTOS
        // ============================================================

        private void LogisticaProcOrdenarProductos()
        {
            PropertyInfo Propiedad =
                typeof(ClsDatosProducto).GetProperty(_ColumnaOrden);

            if (Propiedad == null)
            {
                return;
            }

            _Filtrados.Sort((A, B) =>
            {
                object ValorA = Propiedad.GetValue(A, null);

                object ValorB = Propiedad.GetValue(B, null);

                int Resultado =
                    Comparer.DefaultInvariant.Compare(
                        ValorA, ValorB);

                return _OrdenAscendente
                    ? Resultado
                    : -Resultado;
            });
        }

        private void LogisticaDgvProducto_ColumnHeaderMouseClick(
            object Sender,
            DataGridViewCellMouseEventArgs E)
        {
            if (E.ColumnIndex < 0)
            {
                return;
            }

            string Nombre =
                LogisticaDgvProducto.Columns[E.ColumnIndex]
                    .DataPropertyName;

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                return;
            }

            _OrdenAscendente =
                _ColumnaOrden == Nombre
                    ? !_OrdenAscendente
                    : true;

            _ColumnaOrden = Nombre;

            LogisticaProcOrdenarProductos();

            _PaginaActual = 1;

            LogisticaProcMostrarPagina();
        }

        // ============================================================
        // PAGINACIÓN
        // ============================================================

        private int LogisticaFuncTotalPaginas()
        {
            return Math.Max(
                1,
                (_Filtrados.Count + _TamanoPagina - 1)
                / _TamanoPagina);
        }

        private void LogisticaProcMostrarPagina()
        {
            int Desplazamiento =
                LogisticaDgvProducto.HorizontalScrollingOffset;

            LogisticaDgvProducto.DataSource = null;

            LogisticaDgvProducto.DataSource =
                _Filtrados
                    .Skip((_PaginaActual - 1) * _TamanoPagina)
                    .Take(_TamanoPagina)
                    .ToList();

            foreach (DataGridViewColumn Columna
                in LogisticaDgvProducto.Columns)
            {
                Columna.HeaderCell.SortGlyphDirection =
                    SortOrder.None;
            }

            if (LogisticaDgvProducto.Columns.Contains(
                _ColumnaOrden))
            {
                LogisticaDgvProducto.Columns[_ColumnaOrden]
                    .HeaderCell.SortGlyphDirection =
                    _OrdenAscendente
                        ? SortOrder.Ascending
                        : SortOrder.Descending;
            }

            LogisticaDgvProducto.ClearSelection();

            try
            {
                LogisticaDgvProducto.HorizontalScrollingOffset =
                    Desplazamiento;
            }
            catch (ArgumentOutOfRangeException)
            {
                LogisticaDgvProducto.HorizontalScrollingOffset = 0;
            }

            LogisticaLblDatosPaginaTabla.Text =
                string.Format(
                    "Página {0} de {1} | {2} productos",
                    _PaginaActual,
                    LogisticaFuncTotalPaginas(),
                    _Filtrados.Count);

            LogisticaBtrnAnteriorProducto.Enabled =
                _PaginaActual > 1;

            LogisticaBtnSiguiente.Enabled =
                _PaginaActual < LogisticaFuncTotalPaginas();
        }

        private void LogisticaBtnAnteriorProducto_Click(
            object Sender,
            EventArgs E)
        {
            if (_PaginaActual > 1)
            {
                _PaginaActual--;

                LogisticaProcMostrarPagina();
            }
        }

        private void LogisticaBtnSiguienteProducto_Click(
            object Sender,
            EventArgs E)
        {
            if (_PaginaActual < LogisticaFuncTotalPaginas())
            {
                _PaginaActual++;

                LogisticaProcMostrarPagina();
            }
        }

        // ============================================================
        // SELECCIONAR PRODUCTO
        // ============================================================

        private ClsDatosProducto LogisticaFuncProductoSeleccionado()
        {
            if (LogisticaDgvProducto.SelectedRows.Count == 0)
            {
                throw new ValidationException(
                    "Seleccione un producto en la tabla.");
            }

            ClsDatosProducto Producto =
                LogisticaDgvProducto.SelectedRows[0]
                    .DataBoundItem as ClsDatosProducto;

            if (Producto == null)
            {
                throw new ValidationException(
                    "No se pudo identificar el producto.");
            }

            return Producto;
        }

        private void LogisticaDgvProducto_CellDoubleClick(
            object Sender,
            DataGridViewCellEventArgs E)
        {
            if (E.RowIndex < 0)
            {
                return;
            }

            try
            {
                LogisticaDgvProducto.ClearSelection();

                LogisticaDgvProducto.Rows[E.RowIndex]
                    .Selected = true;

                LogisticaProcCargarProductoSeleccionado();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // CARGAR PRODUCTO SELECCIONADO
        // ============================================================

        private void LogisticaProcCargarProductoSeleccionado()
        {
            ClsDatosProducto Producto =
                LogisticaFuncProductoSeleccionado();

            _ModeloProducto.Estado =
                ClsEstadoEntidad.Modified;

            LogisticaProcHabilitarEdicion(true);

            LogisticaTxtIdProducto.Text =
                Producto.IdProducto.ToString();

            LogisticaTxtSKUProducto.Text =
                Producto.SkuProducto;

            LogisticaTxtNombreProducto.Text =
                Producto.NombreProducto;

            LogisticaTxtClasificacionProducto.Text =
                Producto.ClasificacionProducto;

            LogisticaTxtPaisProducto.Text =
                Producto.PaisProducto;

            LogisticaCboCategoria.SelectedValue =
                Producto.FkCategoriaCodigo;

            LogisticaCboMarca.SelectedValue =
                Producto.FkMarcaCodigo;

            LogisticaCboLinea.SelectedValue =
                Producto.FkLineaCodigo;

            LogisticaCboProveedor.SelectedValue =
                Producto.IdProveedorProductos;

            LogisticaTxtDescripcionProducto.Text =
                Producto.DescripcionProducto;

            LogisticaTxtPesoProducto.Text =
                Producto.PesoProducto.ToString(
                    CultureInfo.InvariantCulture);

            LogisticaTxtPrecioCostoProducto.Text =
                Producto.PrecioCostoProducto.ToString(
                    CultureInfo.InvariantCulture);

            LogisticaTxtPrecioVentaProducto.Text =
                Producto.PrecioVentaProducto.ToString(
                    CultureInfo.InvariantCulture);

            LogisticaTxtStockMinimoProducto.Text =
                Producto.StockMinimoProducto.ToString();

            LogisticaTxtStockMaximoProducto.Text =
                Producto.StockMaximoProducto.ToString();

            LogisticaTxtComisionProducto.Text =
                Producto.PorcentajeComisionProducto.ToString(
                    CultureInfo.InvariantCulture);

            _ImagenProducto = Producto.ImagenProducto;

            LogisticaProcMostrarImagen(_ImagenProducto);
        }

        // ============================================================
        // INGRESAR
        // ============================================================

        private void LogisticaBtnIngresarProducto_Click(
            object Sender,
            EventArgs E)
        {
            LogisticaProcLimpiarCampos();

            LogisticaDgvProducto.ClearSelection();

            _ModeloProducto.Estado =
                ClsEstadoEntidad.Added;

            LogisticaProcHabilitarEdicion(true);

            LogisticaTxtSKUProducto.Focus();
        }

        // ============================================================
        // CONSULTAR
        // ============================================================

        private void LogisticaBtnConsultarProducto_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcRecargarProductos();

                LogisticaTxtBuscarInteligenteProducto.Focus();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // MODIFICAR
        // ============================================================

        private void LogisticaBtnModificarProducto_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcCargarProductoSeleccionado();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // VALIDAR CAMPOS OBLIGATORIOS
        // ============================================================

        private bool LogisticaFuncCamposCompletos()
        {
            TextBox[] Campos = new TextBox[]
            {
                LogisticaTxtSKUProducto,
                LogisticaTxtNombreProducto,
                LogisticaTxtClasificacionProducto,
                LogisticaTxtPaisProducto,
                LogisticaTxtDescripcionProducto,
                LogisticaTxtPesoProducto,
                LogisticaTxtPrecioCostoProducto,
                LogisticaTxtPrecioVentaProducto,
                LogisticaTxtStockMinimoProducto,
                LogisticaTxtStockMaximoProducto,
                LogisticaTxtComisionProducto
            };

            foreach (TextBox Campo in Campos)
            {
                if (string.IsNullOrWhiteSpace(Campo.Text))
                {
                    return false;
                }
            }

            if (LogisticaCboCategoria.SelectedIndex < 0 ||
                LogisticaCboMarca.SelectedIndex < 0 ||
                LogisticaCboLinea.SelectedIndex < 0 ||
                LogisticaCboProveedor.SelectedIndex < 0)
            {
                return false;
            }

            if (_ImagenProducto == null ||
                _ImagenProducto.Length == 0)
            {
                return false;
            }

            return true;
        }

        // ============================================================
        // GUARDAR
        // ============================================================

        private void LogisticaBtnGuardarProducto_Click(
            object Sender,
            EventArgs E)
        {
            if (!_ModoEdicion)
            {
                LogisticaProcMostrarError(
                    "Primero presione Ingresar o Modificar.");

                return;
            }

            // Verificar todos los campos antes de guardar.
            if (!LogisticaFuncCamposCompletos())
            {
                LogisticaProcMostrarError(
                    "Completar todos los campos.");

                return;
            }

            try
            {
                bool EsNuevo =
                    _ModeloProducto.Estado ==
                    ClsEstadoEntidad.Added;

                _ModeloProducto.Producto =
                    LogisticaFuncLeerFormulario();

                // Guardar mediante MVC.
                _ModeloProducto.LogisticaFuncGrabarCambios();

                LogisticaProcMostrarExito(
                    EsNuevo
                        ? "El producto se guardó correctamente."
                        : "El producto se modificó correctamente.");

                LogisticaProcLimpiarCampos();

                LogisticaProcHabilitarEdicion(false);
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
                return;
            }

            try
            {
                LogisticaProcRecargarProductos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(
                    "El producto se guardó, pero no se pudo " +
                    "actualizar el listado.");

                Debug.WriteLine(Ex.ToString());
            }
        }

        // ============================================================
        // LEER FORMULARIO
        // ============================================================

        private ClsDatosProducto LogisticaFuncLeerFormulario()
        {
            if (!LogisticaFuncCamposCompletos())
            {
                throw new ValidationException(
                    "Completar todos los campos.");
            }

            int StockMinimo;
            int StockMaximo;

            if (!int.TryParse(
                    LogisticaTxtStockMinimoProducto.Text,
                    out StockMinimo) ||
                !int.TryParse(
                    LogisticaTxtStockMaximoProducto.Text,
                    out StockMaximo))
            {
                throw new ValidationException(
                    "El stock debe contener números enteros.");
            }

            int IdProducto = 0;

            if (_ModeloProducto.Estado ==
                ClsEstadoEntidad.Modified)
            {
                if (!int.TryParse(
                    LogisticaTxtIdProducto.Text,
                    out IdProducto) ||
                    IdProducto <= 0)
                {
                    throw new ValidationException(
                        "Seleccione un producto válido.");
                }
            }

            return new ClsDatosProducto
            {
                IdProducto = IdProducto,

                SkuProducto =
                    LogisticaTxtSKUProducto.Text.Trim(),

                NombreProducto =
                    LogisticaTxtNombreProducto.Text.Trim(),

                ClasificacionProducto =
                    LogisticaTxtClasificacionProducto.Text.Trim(),

                PaisProducto =
                    LogisticaTxtPaisProducto.Text.Trim(),

                FkCategoriaCodigo =
                    Convert.ToInt32(
                        LogisticaCboCategoria.SelectedValue),

                FkMarcaCodigo =
                    Convert.ToInt32(
                        LogisticaCboMarca.SelectedValue),

                FkLineaCodigo =
                    Convert.ToInt32(
                        LogisticaCboLinea.SelectedValue),

                IdProveedorProductos =
                    Convert.ToInt32(
                        LogisticaCboProveedor.SelectedValue),

                DescripcionProducto =
                    LogisticaTxtDescripcionProducto.Text.Trim(),

                ImagenProducto = _ImagenProducto,

                PesoProducto =
                    LogisticaFuncLeerDecimal(
                        LogisticaTxtPesoProducto,
                        "Peso"),

                PrecioCostoProducto =
                    LogisticaFuncLeerDecimal(
                        LogisticaTxtPrecioCostoProducto,
                        "Precio de costo"),

                PrecioVentaProducto =
                    LogisticaFuncLeerDecimal(
                        LogisticaTxtPrecioVentaProducto,
                        "Precio de venta"),

                StockMinimoProducto = StockMinimo,

                StockMaximoProducto = StockMaximo,

                PorcentajeComisionProducto =
                    LogisticaFuncLeerDecimal(
                        LogisticaTxtComisionProducto,
                        "Comisión")
            };
        }

        // ============================================================
        // VALIDAR DECIMALES
        // ============================================================

        private decimal LogisticaFuncLeerDecimal(
            TextBox Campo,
            string Nombre)
        {
            string Texto =
                Campo.Text.Trim().Replace(',', '.');

            decimal Valor;

            if (!decimal.TryParse(
                Texto,
                NumberStyles.AllowLeadingSign |
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out Valor))
            {
                throw new ValidationException(
                    Nombre + " debe ser un número válido.");
            }

            return Valor;
        }

        // ============================================================
        // ELIMINAR
        // ============================================================

        private void LogisticaBtnEliminarProducto_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                ClsDatosProducto Producto =
                    LogisticaFuncProductoSeleccionado();

                string NombreProducto =
                    Producto.NombreProducto ?? "";

                if (NombreProducto.Length > 55)
                {
                    NombreProducto =
                        NombreProducto.Substring(0, 55) + "...";
                }

                bool Confirmado =
                    LogisticaFuncConfirmarEliminacion(
                        "¿Desea eliminar \"" +
                        NombreProducto + "\"?\n" +
                        "Esta acción no se puede deshacer.");

                if (!Confirmado)
                {
                    return;
                }

                _ModeloProducto.Estado =
                    ClsEstadoEntidad.Deleted;

                _ModeloProducto.Producto =
                    new ClsDatosProducto
                    {
                        IdProducto = Producto.IdProducto
                    };

                _ModeloProducto.LogisticaFuncGrabarCambios();

                LogisticaProcMostrarExito(
                    "El producto se eliminó correctamente.");

                LogisticaProcLimpiarCampos();

                LogisticaProcHabilitarEdicion(false);

                LogisticaProcRecargarProductos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // REFRESCAR
        // ============================================================

        private void LogisticaBtnRefrescarProducto_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcLimpiarCampos();

                LogisticaProcHabilitarEdicion(false);

                LogisticaProcCargarCatalogos();

                LogisticaTxtBuscarInteligenteProducto.Clear();

                LogisticaProcRecargarProductos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // SELECCIONAR IMAGEN
        // ============================================================

        private void LogisticaBtnSeleccionarImagen_Click(
            object Sender,
            EventArgs E)
        {
            using (OpenFileDialog Dialogo =
                new OpenFileDialog())
            {
                Dialogo.Title =
                    "Seleccionar imagen del producto";

                Dialogo.Filter =
                    "Imágenes|*.png;*.jpg;*.jpeg;*.bmp";

                Dialogo.CheckFileExists = true;

                if (Dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    byte[] Bytes =
                        File.ReadAllBytes(Dialogo.FileName);

                    if (Bytes.Length == 0 ||
                        Bytes.Length > 10 * 1024 * 1024)
                    {
                        throw new ValidationException(
                            "Seleccione una imagen de hasta 10 MB.");
                    }

                    LogisticaProcMostrarImagen(Bytes);

                    _ImagenProducto = Bytes;
                }
                catch (Exception Ex)
                {
                    LogisticaProcMostrarError(Ex.Message);
                }
            }
        }

        // ============================================================
        // MOSTRAR IMAGEN
        // ============================================================

        private void LogisticaProcMostrarImagen(byte[] Datos)
        {
            Image NuevaImagen = null;

            if (Datos != null && Datos.Length > 0)
            {
                using (MemoryStream Flujo =
                    new MemoryStream(Datos))
                using (Image Temporal =
                    Image.FromStream(Flujo))
                {
                    NuevaImagen = new Bitmap(Temporal);
                }
            }

            Image ImagenAnterior =
                LogisticaPbImagenProducto.Image;

            LogisticaPbImagenProducto.Image = NuevaImagen;

            if (ImagenAnterior != null)
            {
                ImagenAnterior.Dispose();
            }
        }

        // ============================================================
        // LIMPIAR CAMPOS
        // ============================================================

        private void LogisticaProcLimpiarCampos()
        {
            LogisticaTxtIdProducto.Clear();
            LogisticaTxtSKUProducto.Clear();
            LogisticaTxtNombreProducto.Clear();
            LogisticaTxtClasificacionProducto.Clear();
            LogisticaTxtPaisProducto.Clear();

            LogisticaTxtDescripcionProducto.Clear();
            LogisticaTxtPesoProducto.Clear();

            LogisticaTxtPrecioCostoProducto.Clear();
            LogisticaTxtPrecioVentaProducto.Clear();

            LogisticaTxtStockMinimoProducto.Clear();
            LogisticaTxtStockMaximoProducto.Clear();
            LogisticaTxtComisionProducto.Clear();

            LogisticaCboCategoria.SelectedIndex = -1;
            LogisticaCboMarca.SelectedIndex = -1;
            LogisticaCboLinea.SelectedIndex = -1;
            LogisticaCboProveedor.SelectedIndex = -1;

            _ImagenProducto = null;

            LogisticaProcMostrarImagen(null);
        }

        // ============================================================
        // HABILITAR EDICIÓN
        // ============================================================

        private void LogisticaProcHabilitarEdicion(bool Habilitar)
        {
            _ModoEdicion = Habilitar;

            LogisticaPnlDatosProducto.Enabled = Habilitar;

            LogisticaBtnGuardarProducto.Enabled = Habilitar;
        }

        // ============================================================
        // IMPRIMIR Y AYUDA
        // ============================================================

        private void LogisticaBtnImprimirProducto_Click(
            object Sender,
            EventArgs E)
        {
            MessageBox.Show(
                "El reporte RDLC de productos está pendiente.",
                "Información - Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void LogisticaBtnAyudaProducto_Click(
            object Sender,
            EventArgs E)
        {
            MessageBox.Show(
                "El manual CHM de Producto está pendiente.",
                "Información - Logística",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ============================================================
        // MOSTRAR CONFIRMACIÓN
        // ============================================================

        private bool LogisticaFuncConfirmarEliminacion(
            string Mensaje)
        {
            return LogisticaFuncMostrarDialogo(
                ClsTipoDialogo.Confirmacion,
                "Confirmar eliminación",
                Mensaje) == DialogResult.Yes;
        }

        // ============================================================
        // MOSTRAR ÉXITO
        // ============================================================

        private void LogisticaProcMostrarExito(string Mensaje)
        {
            LogisticaFuncMostrarDialogo(
                ClsTipoDialogo.Exito,
                "Operación exitosa",
                Mensaje);
        }

        // ============================================================
        // MOSTRAR ERROR
        // ============================================================

        private void LogisticaProcMostrarError(string Mensaje)
        {
            // Guardar mensaje técnico completo
            // en la ventana Salida de Visual Studio.
            Debug.WriteLine(
                "LOGÍSTICA - PRODUCTO: " + Mensaje);

            if (string.IsNullOrWhiteSpace(Mensaje))
            {
                Mensaje = "No se pudo completar la operación.";
            }

            // Evitar mensajes extensos en ventanas pequeñas.
            if (Mensaje.Length > 150)
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

        // ============================================================
        // DIÁLOGOS ESTÁNDAR DE LOGÍSTICA
        //
        // Confirmación: ámbar
        // Éxito: verde
        // Error: rojo
        // ============================================================

        private DialogResult LogisticaFuncMostrarDialogo(
            ClsTipoDialogo Tipo,
            string Titulo,
            string Mensaje)
        {
            Color ColorIcono;
            string Simbolo;

            switch (Tipo)
            {
                case ClsTipoDialogo.Confirmacion:
                    ColorIcono = Color.FromArgb(236, 171, 42);
                    Simbolo = "!";
                    break;

                case ClsTipoDialogo.Exito:
                    ColorIcono = Color.FromArgb(45, 158, 93);
                    Simbolo = "✓";
                    break;

                default:
                    ColorIcono = Color.FromArgb(218, 62, 70);
                    Simbolo = "×";
                    break;
            }

            using (Form Ventana = new Form())
            {
                Ventana.Text = Titulo;

                // Mensaje compacto.
                Ventana.ClientSize = new Size(430, 185);

                Ventana.StartPosition =
                    FormStartPosition.CenterParent;

                Ventana.FormBorderStyle =
                    FormBorderStyle.FixedDialog;

                Ventana.MaximizeBox = false;
                Ventana.MinimizeBox = false;
                Ventana.ShowIcon = false;
                Ventana.ShowInTaskbar = false;

                Ventana.BackColor = Color.White;

                // ====================================================
                // ÍCONO
                // ====================================================

                PictureBox Icono = new PictureBox
                {
                    Location = new Point(25, 60),
                    Size = new Size(48, 48),
                    BackColor = Color.White
                };

                Icono.Paint += (Sender, E) =>
                {
                    E.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    using (SolidBrush Pincel =
                        new SolidBrush(ColorIcono))
                    {
                        E.Graphics.FillEllipse(
                            Pincel, 2, 2, 44, 44);
                    }

                    using (Font Fuente =
                        new Font(
                            "Segoe UI Symbol",
                            20F,
                            FontStyle.Bold))
                    {
                        TextRenderer.DrawText(
                            E.Graphics,
                            Simbolo,
                            Fuente,
                            new Rectangle(2, 2, 44, 44),
                            Color.White,
                            TextFormatFlags.HorizontalCenter |
                            TextFormatFlags.VerticalCenter |
                            TextFormatFlags.NoPadding);
                    }
                };

                // ====================================================
                // TÍTULO
                // ====================================================

                Label LblTitulo = new Label
                {
                    Text = Titulo,
                    Location = new Point(90, 22),
                    Size = new Size(310, 30),
                    ForeColor = ColorIcono,
                    Font = new Font(
                        "Segoe UI",
                        13F,
                        FontStyle.Bold)
                };

                // ====================================================
                // TEXTO DEL MENSAJE
                // Sin TextBox y sin barras de desplazamiento.
                // ====================================================

                Label LblMensaje = new Label
                {
                    Text = Mensaje,
                    Location = new Point(90, 58),
                    Size = new Size(315, 70),
                    ForeColor = Color.FromArgb(60, 60, 60),
                    Font = new Font("Segoe UI", 9.5F),
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoEllipsis = true
                };

                Ventana.Controls.Add(Icono);
                Ventana.Controls.Add(LblTitulo);
                Ventana.Controls.Add(LblMensaje);

                // ====================================================
                // BOTÓN PRINCIPAL
                // ====================================================

                Button BtnPrincipal = new Button
                {
                    Location = new Point(295, 143),
                    Size = new Size(115, 30),
                    BackColor = Color.FromArgb(47, 49, 58),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font(
                        "Segoe UI",
                        9F,
                        FontStyle.Bold)
                };

                BtnPrincipal.FlatAppearance.BorderSize = 0;

                // ====================================================
                // CONFIRMACIÓN
                // ====================================================

                if (Tipo == ClsTipoDialogo.Confirmacion)
                {
                    BtnPrincipal.Text = "Sí, eliminar";

                    BtnPrincipal.DialogResult =
                        DialogResult.Yes;

                    Button BtnNo = new Button
                    {
                        Text = "No",
                        Location = new Point(195, 143),
                        Size = new Size(90, 30),
                        BackColor = Color.White,
                        ForeColor = Color.FromArgb(47, 49, 58),
                        FlatStyle = FlatStyle.Flat,
                        DialogResult = DialogResult.No,
                        Font = new Font("Segoe UI", 9F)
                    };

                    BtnNo.FlatAppearance.BorderColor =
                        Color.LightGray;

                    Ventana.Controls.Add(BtnNo);

                    Ventana.CancelButton = BtnNo;

                    Ventana.AcceptButton = BtnNo;
                }
                else
                {
                    BtnPrincipal.Text = "Aceptar";

                    BtnPrincipal.DialogResult =
                        DialogResult.OK;

                    Ventana.AcceptButton = BtnPrincipal;

                    Ventana.CancelButton = BtnPrincipal;
                }

                Ventana.Controls.Add(BtnPrincipal);

                return Ventana.ShowDialog(this);
            }
        }
    }
}
