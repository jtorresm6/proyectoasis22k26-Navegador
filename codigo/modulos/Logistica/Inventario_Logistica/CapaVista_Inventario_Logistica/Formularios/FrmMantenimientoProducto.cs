
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: FrmMantenimientoProducto.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Formulario encargado del mantenimiento de productos del módulo de
 * Logística. Permite ingresar, consultar, modificar y eliminar productos,
 * administrar sus imágenes, cargar catálogos relacionados y visualizar
 * los registros mediante una tabla con búsqueda y paginación.
 *
 * FECHA DE REALIZACIÓN: 05/11/2026
 * ============================================================================
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
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
        // Controlador del mantenimiento de productos.
        private readonly ClsModeloProducto _ModeloProducto =
            new ClsModeloProducto();

        // Listas de categorías y productos.
        private List<ClsOpcionCatalogo> _Categorias =
            new List<ClsOpcionCatalogo>();

        private List<ClsDatosProducto> _TodosProductos =
            new List<ClsDatosProducto>();

        private List<ClsDatosProducto> _ProductosFiltrados =
            new List<ClsDatosProducto>();

        // Imagen del producto.
        private byte[] _ImagenProducto;

        // Estado del formulario.
        private bool _ModoEdicion;

        // Configuración de paginación.
        private int _PaginaActual = 1;

        private const int _TamanoPagina = 10;

        private string _ColumnaOrden = "IdProducto";

        private bool _OrdenAscendente = false;

        // Controles de consulta creados mediante código.
        private DataGridView LogisticaDgvProductos;

        private TextBox LogisticaTxtBuscarProducto;

        private Label LogisticaLblBuscarProducto;

        private Label LogisticaLblPaginacionProducto;

        private Button LogisticaBtnAnteriorProducto;

        private Button LogisticaBtnSiguienteProducto;

        // ============================================================
        // CONSTRUCTOR DEL FORMULARIO
        // ============================================================

        public FrmMantenimientoProducto()
        {
            InitializeComponent();

            LogisticaProcCrearConsultaVisual();

            // Conserva los controles del diseñador.
            ClientSize = new Size(1144, 680);

            LogisticaPnlProducto.AutoScroll = true;

            LogisticaPnlProducto.AutoScrollMinSize =
                new Size(1125, 742);

            LogisticaProcVincularEventos();

            LogisticaProcBloquearFormulario();
        }

        // ============================================================
        // VINCULACIÓN DE EVENTOS
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
        }

        // ============================================================
        // EVENTO LOAD
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
                LogisticaProcMostrarError(
                    "No se pudieron cargar los datos: " +
                    Ex.Message);
            }
        }

        // ============================================================
        // CREACIÓN DE CONTROLES PARA CONSULTAS
        // ============================================================

        private void LogisticaProcCrearConsultaVisual()
        {
            Color Fondo =
                Color.FromArgb(251, 246, 242);

            Color Verde =
                Color.FromArgb(62, 138, 132);

            Color Naranja =
                Color.FromArgb(210, 99, 46);

            Color Botones =
                Color.FromArgb(242, 216, 197);

            // Etiqueta del buscador.
            LogisticaLblBuscarProducto = new Label
            {
                Name = "LogisticaLblBuscarProducto",
                Text = "Buscar:",
                Location = new Point(37, 486),
                Size = new Size(85, 28),
                BackColor = Color.Transparent,
                ForeColor = Verde,
                Font = new Font("Tahoma", 12.75f)
            };

            // Campo para buscar productos.
            LogisticaTxtBuscarProducto = new TextBox
            {
                Name = "LogisticaTxtBuscarProducto",
                Location = new Point(124, 484),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 12f)
            };

            LogisticaTxtBuscarProducto.TextChanged +=
                LogisticaTxtBuscarProducto_TextChanged;

            // Tabla para visualizar los productos.
            LogisticaDgvProductos = new DataGridView
            {
                Name = "LogisticaDgvProductos",
                Location = new Point(37, 523),
                Size = new Size(1078, 178),
                BackgroundColor = Fondo,
                BorderStyle = BorderStyle.FixedSingle,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,

                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                RowHeadersVisible = false,

                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                EnableHeadersVisualStyles = false
            };

            // Colores de encabezados.
            LogisticaDgvProductos
                .ColumnHeadersDefaultCellStyle.BackColor = Verde;

            LogisticaDgvProductos
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Colores de filas.
            LogisticaDgvProductos
                .DefaultCellStyle.BackColor = Fondo;

            LogisticaDgvProductos
                .AlternatingRowsDefaultCellStyle.BackColor = Botones;

            LogisticaDgvProductos
                .DefaultCellStyle.SelectionBackColor = Naranja;

            LogisticaDgvProductos
                .DefaultCellStyle.SelectionForeColor = Color.White;

            // Eventos del DataGridView.
            LogisticaDgvProductos.CellDoubleClick +=
                LogisticaDgvProductos_CellDoubleClick;

            LogisticaDgvProductos.ColumnHeaderMouseClick +=
                LogisticaDgvProductos_ColumnHeaderMouseClick;

            // Botón anterior.
            LogisticaBtnAnteriorProducto = new Button
            {
                Name = "LogisticaBtnAnteriorProducto",
                Text = "Anterior",
                Location = new Point(37, 707),
                Size = new Size(93, 28),
                BackColor = Botones
            };

            LogisticaBtnAnteriorProducto.Click += (Sender, E) =>
            {
                if (_PaginaActual > 1)
                {
                    _PaginaActual--;

                    LogisticaProcMostrarPagina();
                }
            };

            // Botón siguiente.
            LogisticaBtnSiguienteProducto = new Button
            {
                Name = "LogisticaBtnSiguienteProducto",
                Text = "Siguiente",
                Location = new Point(140, 707),
                Size = new Size(93, 28),
                BackColor = Botones
            };

            LogisticaBtnSiguienteProducto.Click += (Sender, E) =>
            {
                if (_PaginaActual < LogisticaFuncTotalPaginas())
                {
                    _PaginaActual++;

                    LogisticaProcMostrarPagina();
                }
            };

            // Etiqueta de paginación.
            LogisticaLblPaginacionProducto = new Label
            {
                Name = "LogisticaLblPaginacionProducto",
                Location = new Point(248, 710),
                Size = new Size(550, 24),
                BackColor = Color.Transparent,
                ForeColor = Verde,
                Font = new Font("Segoe UI", 10f)
            };

            // Agregar controles al panel principal.
            LogisticaPnlProducto.Controls.Add(
                LogisticaLblBuscarProducto);

            LogisticaPnlProducto.Controls.Add(
                LogisticaTxtBuscarProducto);

            LogisticaPnlProducto.Controls.Add(
                LogisticaDgvProductos);

            LogisticaPnlProducto.Controls.Add(
                LogisticaBtnAnteriorProducto);

            LogisticaPnlProducto.Controls.Add(
                LogisticaBtnSiguienteProducto);

            LogisticaPnlProducto.Controls.Add(
                LogisticaLblPaginacionProducto);

            // Configuración de imagen.
            LogisticaPbImagenProducto.SizeMode =
                PictureBoxSizeMode.Zoom;

            // El identificador es automático.
            LogisticaTxtIdProducto.ReadOnly = true;

            LogisticaTxtIdProducto.TabStop = false;

            // Autocompletado de categoría.
            LogisticaTxtCategoriaProducto.AutoCompleteMode =
                AutoCompleteMode.SuggestAppend;

            LogisticaTxtCategoriaProducto.AutoCompleteSource =
                AutoCompleteSource.CustomSource;

            // Los ComboBox solo admiten registros existentes.
            LogisticaCboMarca.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboLinea.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LogisticaCboProveedor.DropDownStyle =
                ComboBoxStyle.DropDownList;
        }

        // ============================================================
        // CARGA DE CATÁLOGOS
        // ============================================================

        private void LogisticaProcCargarCatalogos()
        {
            // Obtener categorías desde el controlador.
            _Categorias =
                _ModeloProducto.LogisticaFuncCategorias();

            var Nombres = new AutoCompleteStringCollection();

            Nombres.AddRange(
                _Categorias.Select(C => C.Nombre).ToArray());

            LogisticaTxtCategoriaProducto
                .AutoCompleteCustomSource = Nombres;

            // Cargar marcas.
            LogisticaProcConfigurarCombo(
                LogisticaCboMarca,
                _ModeloProducto.LogisticaFuncMarcas());

            // Cargar líneas.
            LogisticaProcConfigurarCombo(
                LogisticaCboLinea,
                _ModeloProducto.LogisticaFuncLineas());

            // Cargar proveedores.
            LogisticaProcConfigurarCombo(
                LogisticaCboProveedor,
                _ModeloProducto.LogisticaFuncProveedores());
        }

        // Configura los ComboBox.
        private void LogisticaProcConfigurarCombo(
            ComboBox Combo,
            List<ClsOpcionCatalogo> Datos)
        {
            Combo.DataSource = Datos;

            Combo.DisplayMember = "Nombre";

            Combo.ValueMember = "Id";

            Combo.SelectedIndex = -1;
        }

        // ============================================================
        // CONSULTAR PRODUCTOS
        // ============================================================

        private void LogisticaProcRecargarProductos()
        {
            _TodosProductos =
                _ModeloProducto.LogisticaFuncObtenerTodos();

            LogisticaProcFiltrarProductos();
        }

        // Filtrar por ID, SKU, nombre o categoría.
        private void LogisticaProcFiltrarProductos()
        {
            string Busqueda =
                LogisticaTxtBuscarProducto.Text.Trim();

            _ProductosFiltrados = _TodosProductos.Where(P =>
                P.IdProducto.ToString().Contains(Busqueda) ||

                (P.SkuProducto ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.NombreProducto ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||

                (P.NombreCategoria ?? "").IndexOf(
                    Busqueda,
                    StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            LogisticaProcOrdenarProductos();

            _PaginaActual = 1;

            LogisticaProcMostrarPagina();
        }

        // Ordenar registros.
        private void LogisticaProcOrdenarProductos()
        {
            PropertyInfo Propiedad =
                typeof(ClsDatosProducto).GetProperty(_ColumnaOrden);

            if (Propiedad == null ||
                _ColumnaOrden == "ImagenProducto")
            {
                return;
            }

            _ProductosFiltrados.Sort((A, B) =>
            {
                object ValorA =
                    Propiedad.GetValue(A, null);

                object ValorB =
                    Propiedad.GetValue(B, null);

                int Resultado =
                    Comparer.DefaultInvariant.Compare(
                        ValorA,
                        ValorB);

                return _OrdenAscendente
                    ? Resultado
                    : -Resultado;
            });
        }

        // Obtener cantidad de páginas.
        private int LogisticaFuncTotalPaginas()
        {
            return Math.Max(
                1,
                (_ProductosFiltrados.Count +
                _TamanoPagina - 1) / _TamanoPagina);
        }

        // ============================================================
        // PAGINACIÓN
        // ============================================================

        private void LogisticaProcMostrarPagina()
        {
            LogisticaDgvProductos.DataSource = null;

            LogisticaDgvProductos.DataSource =
                _ProductosFiltrados
                    .Skip((_PaginaActual - 1) * _TamanoPagina)
                    .Take(_TamanoPagina)
                    .ToList();

            // Ocultar columnas que no deben mostrarse.
            foreach (DataGridViewColumn Columna
                in LogisticaDgvProductos.Columns)
            {
                Columna.SortMode =
                    DataGridViewColumnSortMode.Programmatic;

                if (Columna.Name == "ImagenProducto" ||
                    Columna.Name.StartsWith("Fk") ||
                    Columna.Name == "IdProveedorProductos" ||
                    Columna.Name == "DescripcionProducto")
                {
                    Columna.Visible = false;
                }
            }

            // Títulos de las columnas.
            LogisticaProcTituloColumna(
                "IdProducto", "Id");

            LogisticaProcTituloColumna(
                "SkuProducto", "SKU");

            LogisticaProcTituloColumna(
                "NombreProducto", "Producto");

            LogisticaProcTituloColumna(
                "NombreCategoria", "Categoría");

            LogisticaProcTituloColumna(
                "NombreMarca", "Marca");

            LogisticaProcTituloColumna(
                "NombreLinea", "Línea");

            LogisticaProcTituloColumna(
                "NombreProveedor", "Proveedor");

            LogisticaProcTituloColumna(
                "PrecioVentaProducto", "Precio de venta");

            if (LogisticaDgvProductos.Columns.Contains(
                _ColumnaOrden))
            {
                LogisticaDgvProductos.Columns[_ColumnaOrden]
                    .HeaderCell.SortGlyphDirection =
                    _OrdenAscendente
                        ? SortOrder.Ascending
                        : SortOrder.Descending;
            }

            LogisticaDgvProductos.ClearSelection();

            LogisticaLblPaginacionProducto.Text =
                string.Format(
                    "Página {0} de {1}  |  {2} productos",
                    _PaginaActual,
                    LogisticaFuncTotalPaginas(),
                    _ProductosFiltrados.Count);

            LogisticaBtnAnteriorProducto.Enabled =
                _PaginaActual > 1;

            LogisticaBtnSiguienteProducto.Enabled =
                _PaginaActual < LogisticaFuncTotalPaginas();
        }

        // Personalizar títulos de columnas.
        private void LogisticaProcTituloColumna(
            string Nombre,
            string Titulo)
        {
            if (LogisticaDgvProductos.Columns.Contains(Nombre))
            {
                LogisticaDgvProductos.Columns[Nombre]
                    .HeaderText = Titulo;
            }
        }

        // Evento para búsqueda automática.
        private void LogisticaTxtBuscarProducto_TextChanged(
            object Sender,
            EventArgs E)
        {
            LogisticaProcFiltrarProductos();
        }

        // Ordenar haciendo clic en el encabezado.
        private void LogisticaDgvProductos_ColumnHeaderMouseClick(
            object Sender,
            DataGridViewCellMouseEventArgs E)
        {
            string Nombre =
                LogisticaDgvProductos.Columns[E.ColumnIndex]
                    .DataPropertyName;

            if (string.IsNullOrEmpty(Nombre) ||
                Nombre == "ImagenProducto")
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

        // Seleccionar producto con doble clic.
        private void LogisticaDgvProductos_CellDoubleClick(
            object Sender,
            DataGridViewCellEventArgs E)
        {
            if (E.RowIndex >= 0)
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
        }

        // ============================================================
        // OBTENER PRODUCTO SELECCIONADO
        // ============================================================

        private ClsDatosProducto LogisticaFuncSeleccionado()
        {
            if (LogisticaDgvProductos.SelectedRows.Count == 0)
            {
                throw new ValidationException(
                    "Seleccione una fila del listado de productos.");
            }

            var Producto =
                LogisticaDgvProductos.SelectedRows[0]
                    .DataBoundItem as ClsDatosProducto;

            if (Producto == null)
            {
                throw new ValidationException(
                    "No se pudo identificar el producto seleccionado.");
            }

            return Producto;
        }

        // ============================================================
        // CARGAR PRODUCTO EN EL FORMULARIO
        // ============================================================

        private void LogisticaProcCargarSeleccionado()
        {
            ClsDatosProducto Producto =
                LogisticaFuncSeleccionado();

            LogisticaPnlDatosProducto.Enabled = true;

            _ModoEdicion = true;

            _ModeloProducto.Estado =
                ClsEstadoEntidad.Modified;

            LogisticaTxtIdProducto.Text =
                Producto.IdProducto.ToString();

            LogisticaTxtSKUProducto.Text =
                Producto.SkuProducto;

            LogisticaTxtClasificacionProducto.Text =
                Producto.ClasificacionProducto;

            LogisticaTxtNombreProducto.Text =
                Producto.NombreProducto;

            LogisticaTxtCategoriaProducto.Text =
                Producto.NombreCategoria;

            LogisticaCboMarca.SelectedValue =
                Producto.FkMarcaCodigo;

            LogisticaCboLinea.SelectedValue =
                Producto.FkLineaCodigo;

            LogisticaCboProveedor.SelectedValue =
                Producto.IdProveedorProductos;

            LogisticaTxtDescripcionProducto.Text =
                Producto.DescripcionProducto;

            LogisticaTxtPaisProducto.Text =
                Producto.PaisProducto;

            LogisticaTxtPrecioCostoProducto.Text =
                Producto.PrecioCostoProducto.ToString(
                    CultureInfo.InvariantCulture);

            LogisticaTxtPrecioVentaProducto.Text =
                Producto.PrecioVentaProducto.ToString(
                    CultureInfo.InvariantCulture);

            LogisticaTxtComisionProducto.Text =
                Producto.PorcentajeComisionProducto.ToString(
                    CultureInfo.InvariantCulture);

            LogisticaTxtStockMinimoProducto.Text =
                Producto.StockMinimoProducto.ToString();

            LogisticaTxtStockMaximoProducto.Text =
                Producto.StockMaximoProducto.ToString();

            LogisticaTxtPesoProducto.Text =
                Producto.PesoProducto.ToString(
                    CultureInfo.InvariantCulture);

            _ImagenProducto =
                Producto.ImagenProducto;

            LogisticaProcMostrarImagen(_ImagenProducto);

            LogisticaBtnGuardarProducto.Enabled = true;

            LogisticaTxtSKUProducto.Focus();
        }

        // ============================================================
        // BOTÓN INGRESAR
        // ============================================================

        private void LogisticaBtnIngresarProducto_Click(
            object Sender,
            EventArgs E)
        {
            LogisticaProcBloquearFormulario();

            _ModoEdicion = true;

            _ModeloProducto.Estado =
                ClsEstadoEntidad.Added;

            LogisticaPnlDatosProducto.Enabled = true;

            LogisticaBtnGuardarProducto.Enabled = true;

            LogisticaTxtSKUProducto.Focus();
        }

        // ============================================================
        // BOTÓN CONSULTAR
        // ============================================================

        private void LogisticaBtnConsultarProducto_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcRecargarProductos();

                LogisticaTxtBuscarProducto.Focus();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // BOTÓN MODIFICAR
        // ============================================================

        private void LogisticaBtnModificarProducto_Click(
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

        // ============================================================
        // BOTÓN GUARDAR
        // ============================================================

        private void LogisticaBtnGuardarProducto_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                if (!_ModoEdicion)
                {
                    return;
                }

                // Obtener datos del formulario.
                _ModeloProducto.Producto =
                    LogisticaFuncLeerFormulario();

                // Ejecutar operación en el controlador.
                string Mensaje =
                    _ModeloProducto.LogisticaFuncGrabarCambios();

                MessageBox.Show(
                    Mensaje,
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Limpiar y actualizar el listado.
                LogisticaProcBloquearFormulario();

                LogisticaProcRecargarProductos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // LEER DATOS DEL FORMULARIO
        // ============================================================

        private ClsDatosProducto LogisticaFuncLeerFormulario()
        {
            // Buscar categoría por nombre.
            ClsOpcionCatalogo Categoria =
                _Categorias.FirstOrDefault(C =>
                    string.Equals(
                        C.Nombre,
                        LogisticaTxtCategoriaProducto.Text.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (Categoria == null)
            {
                throw new ValidationException(
                    "Escriba una categoría existente del catálogo.");
            }

            // Validar ComboBox.
            if (LogisticaCboMarca.SelectedValue == null ||
                LogisticaCboLinea.SelectedValue == null ||
                LogisticaCboProveedor.SelectedValue == null)
            {
                throw new ValidationException(
                    "Seleccione marca, línea y proveedor.");
            }

            // Validar stock.
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
                    "Stock mínimo y máximo deben ser números enteros.");
            }

            int Id = 0;

            // Verificar ID al modificar.
            if (_ModeloProducto.Estado ==
                    ClsEstadoEntidad.Modified &&
                !int.TryParse(
                    LogisticaTxtIdProducto.Text,
                    out Id))
            {
                throw new ValidationException(
                    "El identificador del producto no es válido.");
            }

            // Construir objeto del producto.
            return new ClsDatosProducto
            {
                IdProducto = Id,

                SkuProducto =
                    LogisticaTxtSKUProducto.Text.Trim(),

                ClasificacionProducto =
                    LogisticaTxtClasificacionProducto.Text.Trim(),

                NombreProducto =
                    LogisticaTxtNombreProducto.Text.Trim(),

                IdProveedorProductos =
                    Convert.ToInt32(
                        LogisticaCboProveedor.SelectedValue),

                FkMarcaCodigo =
                    Convert.ToInt32(
                        LogisticaCboMarca.SelectedValue),

                FkCategoriaCodigo =
                    Categoria.Id,

                FkLineaCodigo =
                    Convert.ToInt32(
                        LogisticaCboLinea.SelectedValue),

                DescripcionProducto =
                    LogisticaTxtDescripcionProducto.Text.Trim(),

                ImagenProducto =
                    _ImagenProducto,

                PaisProducto =
                    LogisticaTxtPaisProducto.Text.Trim(),

                PrecioCostoProducto =
                    LogisticaFuncDecimal(
                        LogisticaTxtPrecioCostoProducto,
                        "Precio de costo"),

                PrecioVentaProducto =
                    LogisticaFuncDecimal(
                        LogisticaTxtPrecioVentaProducto,
                        "Precio de venta"),

                PorcentajeComisionProducto =
                    LogisticaFuncDecimal(
                        LogisticaTxtComisionProducto,
                        "Comisión"),

                StockMinimoProducto =
                    StockMinimo,

                StockMaximoProducto =
                    StockMaximo,

                PesoProducto =
                    LogisticaFuncDecimal(
                        LogisticaTxtPesoProducto,
                        "Peso")
            };
        }

        // ============================================================
        // VALIDAR NÚMEROS DECIMALES
        // ============================================================

        private decimal LogisticaFuncDecimal(
            TextBox Campo,
            string Nombre)
        {
            decimal Valor;

            string Texto = Campo.Text.Trim();

            if (!decimal.TryParse(
                    Texto,
                    NumberStyles.AllowDecimalPoint |
                    NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out Valor) &&

                !decimal.TryParse(
                    Texto,
                    NumberStyles.AllowDecimalPoint |
                    NumberStyles.AllowLeadingSign,
                    CultureInfo.CurrentCulture,
                    out Valor))
            {
                throw new ValidationException(
                    Nombre + " debe ser un número decimal válido.");
            }

            return Valor;
        }

        // ============================================================
        // BOTÓN ELIMINAR
        // ============================================================

        private void LogisticaBtnEliminarProducto_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                ClsDatosProducto Producto =
                    LogisticaFuncSeleccionado();

                DialogResult Confirmacion =
                    MessageBox.Show(
                        "¿Desea eliminar el producto " +
                        Producto.NombreProducto + "?",
                        "Confirmar eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (Confirmacion != DialogResult.Yes)
                {
                    return;
                }

                // Establecer ID del producto.
                _ModeloProducto.Producto =
                    new ClsDatosProducto
                    {
                        IdProducto = Producto.IdProducto
                    };

                // Indicar operación de eliminación.
                _ModeloProducto.Estado =
                    ClsEstadoEntidad.Deleted;

                // Ejecutar operación.
                string Mensaje =
                    _ModeloProducto.LogisticaFuncGrabarCambios();

                MessageBox.Show(
                    Mensaje,
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LogisticaProcBloquearFormulario();

                LogisticaProcRecargarProductos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // BOTÓN REFRESCAR
        // ============================================================

        private void LogisticaBtnRefrescarProducto_Click(
            object Sender,
            EventArgs E)
        {
            try
            {
                LogisticaProcBloquearFormulario();

                LogisticaProcCargarCatalogos();

                LogisticaTxtBuscarProducto.Clear();

                LogisticaProcRecargarProductos();
            }
            catch (Exception Ex)
            {
                LogisticaProcMostrarError(Ex.Message);
            }
        }

        // ============================================================
        // BOTÓN SELECCIONAR IMAGEN
        // ============================================================

        private void LogisticaBtnSeleccionarImagen_Click(
            object Sender,
            EventArgs E)
        {
            using (var Dialogo = new OpenFileDialog())
            {
                Dialogo.Title =
                    "Seleccionar imagen del producto";

                Dialogo.Filter =
                    "Imágenes|*.jpg;*.jpeg;*.png;*.bmp|" +
                    "Todos los archivos|*.*";

                if (Dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    byte[] Bytes =
                        File.ReadAllBytes(Dialogo.FileName);

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
        // MOSTRAR IMAGEN DEL PRODUCTO
        // ============================================================

        private void LogisticaProcMostrarImagen(byte[] Datos)
        {
            Image Anterior =
                LogisticaPbImagenProducto.Image;

            LogisticaPbImagenProducto.Image = null;

            if (Anterior != null)
            {
                Anterior.Dispose();
            }

            if (Datos == null || Datos.Length == 0)
            {
                return;
            }

            using (var Flujo = new MemoryStream(Datos))
            using (var Temporal = Image.FromStream(Flujo))
            {
                LogisticaPbImagenProducto.Image =
                    new Bitmap(Temporal);
            }
        }

        // ============================================================
        // LIMPIAR Y BLOQUEAR CAMPOS
        // ============================================================

        private void LogisticaProcBloquearFormulario()
        {
            _ModoEdicion = false;

            LogisticaPnlDatosProducto.Enabled = false;

            LogisticaBtnGuardarProducto.Enabled = false;

            // Limpiar campos.
            LogisticaTxtIdProducto.Clear();

            LogisticaTxtSKUProducto.Clear();

            LogisticaTxtClasificacionProducto.Clear();

            LogisticaTxtNombreProducto.Clear();

            LogisticaTxtCategoriaProducto.Clear();

            LogisticaTxtDescripcionProducto.Clear();

            LogisticaTxtPaisProducto.Clear();

            LogisticaTxtPrecioCostoProducto.Clear();

            LogisticaTxtPrecioVentaProducto.Clear();

            LogisticaTxtComisionProducto.Clear();

            LogisticaTxtStockMinimoProducto.Clear();

            LogisticaTxtStockMaximoProducto.Clear();

            LogisticaTxtPesoProducto.Clear();

            // Limpiar selecciones.
            LogisticaCboMarca.SelectedIndex = -1;

            LogisticaCboLinea.SelectedIndex = -1;

            LogisticaCboProveedor.SelectedIndex = -1;

            // Limpiar imagen.
            _ImagenProducto = null;

            LogisticaProcMostrarImagen(null);
        }

        // ============================================================
        // BOTÓN IMPRIMIR
        // ============================================================

        private void LogisticaBtnImprimirProducto_Click(
            object Sender,
            EventArgs E)
        {
            MessageBox.Show(
                "La impresión requiere el reporte RDLC del módulo: " +
                "FrmReporteProducto.cs y RpReporteProducto.rdlc.",
                "Reporte pendiente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ============================================================
        // BOTÓN AYUDA
        // ============================================================

        private void LogisticaBtnAyudaProducto_Click(
            object Sender,
            EventArgs E)
        {
            MessageBox.Show(
                "Para activar Ayuda, integra el archivo oficial " +
                "LogisticaAyudas.chm creado en HTML Help Workshop.",
                "Ayuda pendiente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ============================================================
        // MOSTRAR ERRORES
        // ============================================================

        private void LogisticaProcMostrarError(string Mensaje)
        {
            MessageBox.Show(
                Mensaje,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
