using CapaControlador_Componentes;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Miguel David Contreras Jacinto
 * Carné : 0901-21-3878
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta un selector reutilizable que carga datos, administra la selección y muestra errores de validación.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public enum EnmTipoDatosCompras
    {
        Ninguno = 0,
        Proveedores = 1,
        Metodos = 2
    }


    [DefaultBindingProperty("SelectedValue")]
    [DefaultEvent("SelectedIndexChanged")]
    public partial class ComprasUsrComboBox : UserControl
    {

        private bool _ErrorVisible;
        private bool _MostrarEtiquetaError = true;
        private int _AlturaEtiquetaError = 22;
        private int _AlturaErrorAmpliada;
        private int _AlturaFilaErrorAplicada = 30;
        private int _VersionTexto;
        private EnmTipoDatosCompras _TipoDatos = EnmTipoDatosCompras.Ninguno;
        private bool _Iniciado;
        private bool _PermitirEscritura;
        private ComboBoxStyle _EstiloSinEscritura;
        private AutoCompleteMode _AutoCompleteSinEscritura;
        private AutoCompleteSource _AutoSourceSinEscritura;

        [Category("Compras")]
        [Description("Permite escribir y muestra sugerencias nativas de las opciones del combo conforme se escribe.")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool PermitirEscritura
        {
            get => _PermitirEscritura;
            set
            {
                if (_PermitirEscritura == value) return;
                if (value)
                {
                    _EstiloSinEscritura = ComprasCboEntrada.DropDownStyle;
                    _AutoCompleteSinEscritura = ComprasCboEntrada.AutoCompleteMode;
                    _AutoSourceSinEscritura = ComprasCboEntrada.AutoCompleteSource;
                    ComprasCboEntrada.AutoCompleteMode = AutoCompleteMode.None;
                    ComprasCboEntrada.DropDownStyle = ComboBoxStyle.DropDown;
                    ComprasCboEntrada.AutoCompleteSource = AutoCompleteSource.ListItems;
                    ComprasCboEntrada.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
                else
                {
                    ComprasCboEntrada.AutoCompleteMode = AutoCompleteMode.None;
                    ComprasCboEntrada.DropDownStyle = _EstiloSinEscritura;
                    ComprasCboEntrada.AutoCompleteSource = _AutoSourceSinEscritura;
                    ComprasCboEntrada.AutoCompleteMode = _AutoCompleteSinEscritura;
                }
                _PermitirEscritura = value;
                ComprasProcAjustarAlturaEntrada();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FiltrarAlEscribir
        {
            get => PermitirEscritura;
            set => PermitirEscritura = value;
        }

        [Category("Compras")]
        [Description("Muestra la etiqueta de error y reserva su espacio. Al desactivarla, solo se conserva el borde de error.")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool MostrarEtiquetaError
        {
            get => _MostrarEtiquetaError;
            set
            {
                if (_MostrarEtiquetaError == value) return;
                _MostrarEtiquetaError = value;
                ComprasProcActualizarEspacioError();
            }
        }

        [Category("Compras")]
        [Description("Altura en píxeles de la etiqueta de error, sin incluir sus márgenes.")]
        [DefaultValue(22)]
        [RefreshProperties(RefreshProperties.All)]
        public int AlturaEtiquetaError
        {
            get => _AlturaEtiquetaError;
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException(nameof(value));
                if (_AlturaEtiquetaError == value) return;
                _AlturaEtiquetaError = value;
                ComprasProcActualizarEspacioError();
            }
        }


      

        [Category("Compras")]
        [Description("Conjunto de datos del combo. En ejecución el combo se llena solo al elegirlo.")]
        [DefaultValue(EnmTipoDatosCompras.Ninguno)]
        public EnmTipoDatosCompras TipoDatos
        {
            get => _TipoDatos;
            set
            {
                if (_TipoDatos == value) return;
                _TipoDatos = value;
                if (_Iniciado) ComprasProcCargarSegunTipo();
            }
        }

        private bool ComprasEnDiseno =>
            DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        protected override void OnLoad(EventArgs Evento)
        {
            base.OnLoad(Evento);
            _Iniciado = true;
            ComprasProcCargarSegunTipo();
        }



        private void ComprasProcActualizarEspacioError()
        {
            if (ComprasLblError == null || ComprasTlpMain == null) return;
            ComprasTlpMain.SuspendLayout();
            try
            {
                int Altura = Math.Max(_AlturaEtiquetaError, _AlturaErrorAmpliada);
                int AlturaFila = _MostrarEtiquetaError ? Altura + ComprasLblError.Margin.Vertical : 0;
                int Diferencia = AlturaFila - _AlturaFilaErrorAplicada;
                _AlturaFilaErrorAplicada = AlturaFila;
                ComprasLblError.MinimumSize = Size.Empty;
                ComprasLblError.MaximumSize = new Size(0, Altura);
                ComprasLblError.Visible = _MostrarEtiquetaError;
                RowStyle FilaError = ComprasTlpMain.RowStyles[1];
                if (FilaError.SizeType != SizeType.Absolute) FilaError.SizeType = SizeType.Absolute;
                if (FilaError.Height != AlturaFila) FilaError.Height = AlturaFila;
                if (Diferencia != 0) Height = Math.Max(1, Height + Diferencia);
            }
            finally
            {
                ComprasTlpMain.ResumeLayout(true);
            }
        }

        public ComprasUsrComboBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ComprasLblError.ComprasProcConservarRojo();
            ComprasCboEntrada.TextChanged += (Sender, Evento) => ComprasProcTextoCambiado(Evento);
            Enter += (Sender, Evento) => ComprasCboEntrada.Focus();
            ComprasCboEntrada.KeyDown += (Sender, Evento) => OnKeyDown(Evento);
            ComprasCboEntrada.KeyPress += (Sender, Evento) => OnKeyPress(Evento);
            ComprasCboEntrada.KeyUp += (Sender, Evento) => OnKeyUp(Evento);
            ComprasCboEntrada.MouseClick += (Sender, Evento) => OnMouseClick(Evento);
            ComprasCboEntrada.Enter += (Sender, Evento) => ComprasProcActualizarBorde();
            ComprasCboEntrada.Leave += (Sender, Evento) => ComprasProcActualizarBorde();
            ComprasCboEntrada.SelectedIndexChanged += (Sender, Evento) =>
            {
                ComprasMetLimpiarError();
                SelectedIndexChanged?.Invoke(this, Evento);
            };
            ComprasCboEntrada.SelectedValueChanged += (Sender, Evento) => SelectedValueChanged?.Invoke(this, Evento);
            ComprasCboEntrada.SelectionChangeCommitted += (Sender, Evento) => SelectionChangeCommitted?.Invoke(this, Evento);
            ComprasCboEntrada.DropDown += (Sender, Evento) => DropDown?.Invoke(this, Evento);
            ComprasCboEntrada.DropDownClosed += (Sender, Evento) => DropDownClosed?.Invoke(this, Evento);
            ComprasCboEntrada.DataSourceChanged += (Sender, Evento) => DataSourceChanged?.Invoke(this, Evento);
            ComprasProcAjustarAlturaEntrada();
            ComprasMetLimpiarError();
        }

        private void ComprasProcTextoCambiado(EventArgs Evento)
        {
            _VersionTexto++;
            ComprasMetLimpiarError();
            OnTextChanged(Evento);
        }

        [Browsable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return ComprasCboEntrada == null ? base.Text : ComprasCboEntrada.Text;
            }
            set
            {
                if (ComprasCboEntrada != null)
                {
                    string Nuevo = value ?? string.Empty;
                    string Anterior = ComprasCboEntrada.Text;
                    int Version = _VersionTexto;
                    ComprasCboEntrada.Text = Nuevo;
                    if (Anterior != ComprasCboEntrada.Text && Version == _VersionTexto)
                        ComprasProcTextoCambiado(EventArgs.Empty);
                }
                else
                {
                    base.Text = value;
                }
            }
        }

        internal ClsSelectorCompras ComprasCboInterno => ComprasCboEntrada;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ClsSelectorCompras Entrada => ComprasCboEntrada;

        [Category("Datos")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ComboBox.ObjectCollection Items => ComprasCboEntrada.Items;

        [Category("Datos")]
        [DefaultValue(null)]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource { get => ComprasCboEntrada.DataSource; set => ComprasCboEntrada.DataSource = value; }

        [Category("Datos")]
        [DefaultValue("")]
        public string DisplayMember { get => ComprasCboEntrada.DisplayMember; set => ComprasCboEntrada.DisplayMember = value; }

        [Category("Datos")]
        [DefaultValue("")]
        public string ValueMember { get => ComprasCboEntrada.ValueMember; set => ComprasCboEntrada.ValueMember = value; }

        [Browsable(false)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue { get => ComprasCboEntrada.SelectedValue; set => ComprasCboEntrada.SelectedValue = value; }

        [Browsable(false)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedItem { get => ComprasCboEntrada.SelectedItem; set => ComprasCboEntrada.SelectedItem = value; }

        [Category("Datos")]
        [DefaultValue(-1)]
        [Bindable(true)]
        public int SelectedIndex { get => ComprasCboEntrada.SelectedIndex; set => ComprasCboEntrada.SelectedIndex = value; }

        [Category("Comportamiento")]
        [DefaultValue(ComboBoxStyle.DropDownList)]
        [RefreshProperties(RefreshProperties.All)]
        public ComboBoxStyle DropDownStyle
        {
            get => ComprasCboEntrada.DropDownStyle;
            set { if (_PermitirEscritura) _EstiloSinEscritura = value; ComprasCboEntrada.DropDownStyle = _PermitirEscritura ? ComboBoxStyle.DropDown : value; ComprasProcAjustarAlturaEntrada(); }
        }

        [Category("Comportamiento")]
        [DefaultValue(false)]
        public bool Sorted { get => ComprasCboEntrada.Sorted; set => ComprasCboEntrada.Sorted = value; }

        [Category("Comportamiento")]
        [DefaultValue(8)]
        public int MaxDropDownItems { get => ComprasCboEntrada.MaxDropDownItems; set => ComprasCboEntrada.MaxDropDownItems = value; }

        [Category("Comportamiento")]
        [DefaultValue(true)]
        public bool IntegralHeight { get => ComprasCboEntrada.IntegralHeight; set => ComprasCboEntrada.IntegralHeight = value; }

        [Category("Comportamiento")]
        public AutoCompleteMode AutoCompleteMode { get => ComprasCboEntrada.AutoCompleteMode; set => ComprasCboEntrada.AutoCompleteMode = value; }

        [Category("Comportamiento")]
        public AutoCompleteSource AutoCompleteSource { get => ComprasCboEntrada.AutoCompleteSource; set => ComprasCboEntrada.AutoCompleteSource = value; }

        public bool ShouldSerializeAutoCompleteMode() => !PermitirEscritura && AutoCompleteMode != AutoCompleteMode.None;
        public bool ShouldSerializeAutoCompleteSource() => !PermitirEscritura && AutoCompleteSource != AutoCompleteSource.None;
        public void ResetAutoCompleteMode() => AutoCompleteMode = AutoCompleteMode.None;
        public void ResetAutoCompleteSource() => AutoCompleteSource = AutoCompleteSource.None;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AutoCompleteStringCollection AutoCompleteCustomSource => ComprasCboEntrada.AutoCompleteCustomSource;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DroppedDown { get => ComprasCboEntrada.DroppedDown; set => ComprasCboEntrada.DroppedDown = value; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart { get => ComprasCboEntrada.SelectionStart; set => ComprasCboEntrada.SelectionStart = value; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength { get => ComprasCboEntrada.SelectionLength; set => ComprasCboEntrada.SelectionLength = value; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText { get => ComprasCboEntrada.SelectedText; set => ComprasCboEntrada.SelectedText = value; }

        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;
        public event EventHandler SelectionChangeCommitted;
        public event EventHandler DropDown;
        public event EventHandler DropDownClosed;
        public event EventHandler DataSourceChanged;

        public void BeginUpdate() => ComprasCboEntrada.BeginUpdate();
        public void EndUpdate() => ComprasCboEntrada.EndUpdate();
        public int FindString(string Texto) => ComprasCboEntrada.FindString(Texto);
        public int FindStringExact(string Texto) => ComprasCboEntrada.FindStringExact(Texto);
        public string GetItemText(object Elemento) => ComprasCboEntrada.GetItemText(Elemento);
        public void Select(int Inicio, int Longitud) => ComprasCboEntrada.Select(Inicio, Longitud);
        public void SelectAll() => ComprasCboEntrada.SelectAll();

        private void ComprasProcAjustarAlturaEntrada()
        {
            if (ComprasCboEntrada == null || ComprasPnlBorde == null) return;
            ComprasPnlBorde.MaximumSize = DropDownStyle == ComboBoxStyle.Simple ? Size.Empty :
                new Size(0, ComprasCboEntrada.PreferredHeight + ComprasPnlBorde.Padding.Vertical);
        }

        protected override void OnFontChanged(EventArgs Evento)
        {
            base.OnFontChanged(Evento);
            if (ComprasCboEntrada != null) ComprasCboEntrada.Font = Font;
            ComprasProcAjustarAlturaEntrada();
        }

        protected override void OnForeColorChanged(EventArgs Evento)
        {
            base.OnForeColorChanged(Evento);
            if (ComprasCboEntrada != null) ComprasCboEntrada.ForeColor = ForeColor;
        }

        protected override void OnBackColorChanged(EventArgs Evento)
        {
            base.OnBackColorChanged(Evento);
            if (ComprasCboEntrada != null) ComprasCboEntrada.BackColor = BackColor;
            if (ComprasTlpMain != null) ComprasTlpMain.BackColor = BackColor;
        }

        private void ComprasProcActualizarBorde()
        {
            ComprasPnlBorde.BackColor = _ErrorVisible ? Color.Red :
                ComprasCboEntrada.Focused ? ClsTemaCompras.AcentoNavegador : ClsTemaCompras.PrincipalNavegador;
        }

        protected override void OnCausesValidationChanged(EventArgs Evento)
        {
            base.OnCausesValidationChanged(Evento);
            if (ComprasCboEntrada != null) ComprasCboEntrada.CausesValidation = CausesValidation;
        }

        public int MaxLength
        {
            get
            {
                return ComprasCboEntrada.MaxLength;
            }
            set
            {
                ComprasCboEntrada.MaxLength = value;
            }
        }

        public event KeyEventHandler TextoKeyDown
        {
            add
            {
                ComprasCboEntrada.KeyDown += value;
            }
            remove
            {
                ComprasCboEntrada.KeyDown -= value;
            }
        }

        public void ComprasMetMostrarError(string Mensaje)
        {
            if (string.IsNullOrWhiteSpace(Mensaje))
            {
                ComprasMetLimpiarError();
                return;
            }

            ComprasTlpMain.SuspendLayout();
            try
            {
                if (ComprasLblError.Texto != Mensaje) ComprasLblError.Texto = Mensaje;
                if (ComprasLblError.ColorTexto != Color.Red) ComprasLblError.ColorTexto = Color.Red;
                _ErrorVisible = true;
                ComprasProcActualizarEspacioError();
                if (ComprasPnlBorde.BackColor != Color.Red) ComprasPnlBorde.BackColor = Color.Red;
            }
            finally
            {
                ComprasTlpMain.ResumeLayout(true);
            }
        }

        internal int ComprasFuncAlturaError(string Mensaje)
        {
            int Ancho = System.Math.Max(1, Width - 12);
            int Altura = System.Math.Max(22, TextRenderer.MeasureText(Mensaje, ComprasLblError.Font, new Size(Ancho, int.MaxValue), TextFormatFlags.WordBreak).Height);
            int AlturaEntrada = Math.Max(25, ComprasPnlBorde.Height) + ComprasTlpMain.Padding.Vertical;
            return AlturaEntrada + (_MostrarEtiquetaError ? Math.Max(_AlturaEtiquetaError, Altura) + ComprasLblError.Margin.Vertical : 0);
        }

        internal void ComprasProcAmpliarError(int AlturaFila)
        {
            _AlturaErrorAmpliada = Math.Max(0, AlturaFila - Math.Max(25, ComprasPnlBorde.Height)
                - ComprasTlpMain.Padding.Vertical - ComprasLblError.Margin.Vertical);
            ComprasProcActualizarEspacioError();
        }

        public void ComprasMetLimpiarError()
        {
            ComprasTlpMain.SuspendLayout();
            try
            {
                if (ComprasLblError.Texto != string.Empty) ComprasLblError.Texto = string.Empty;
                _ErrorVisible = false;
                _AlturaErrorAmpliada = 0;
                ComprasProcActualizarEspacioError();
                ComprasProcActualizarBorde();
            }
            finally
            {
                ComprasTlpMain.ResumeLayout(true);
            }
        }


        
        private static void ComprasProcObtenerConfiguracion(EnmTipoDatosCompras Tipo,
            out string Tabla, out string Valor, out string Mostrar)
        {
            switch (Tipo)
            {
                case EnmTipoDatosCompras.Proveedores:
                    Tabla = "tblProveedor";
                    Valor = "idProveedor";
                    Mostrar = "nombreProveedor";
                    break;

                case EnmTipoDatosCompras.Metodos:
             
                    Tabla = string.Empty;
                    Valor = "IDmetodo";
                    Mostrar = "nombreMetodo";
                    break;

                default:
                    throw new InvalidOperationException(
                        "Debe definir TipoDatos antes de cargar datos en el combo.");
            }
        }

        private void ComprasProcCargarSegunTipo()
        {
            if (_TipoDatos == EnmTipoDatosCompras.Ninguno || ComprasEnDiseno) return;

            try
            {
                if (_TipoDatos == EnmTipoDatosCompras.Metodos)
                {
                    DataTable Metodos = new DataTable();
                    Metodos.Columns.Add("IDmetodo", typeof(string));
                    Metodos.Columns.Add("nombreMetodo", typeof(string));
                    foreach (string Metodo in new[] { "Transferencia", "Cheque", "Efectivo", "Deposito" })
                        Metodos.Rows.Add(Metodo, Metodo);
                    ComprasMetCargarDatos(Metodos);
                    return;
                }

                ComprasProcObtenerConfiguracion(_TipoDatos,
                    out string Tabla, out string Valor, out string Mostrar);

               
                DataTable Datos = _TipoDatos == EnmTipoDatosCompras.Proveedores
                    ? new ClsComboCompras().ComprasFuncObtenerProveedores()
                    : new ClsComboCompras().ComprasFuncEnviarDatos(Tabla, Valor, Mostrar);
                ComprasMetCargarDatos(Datos);
            }
            catch (Exception Ex)
            {
               
                Debug.WriteLine(Ex);
                ComprasMetMostrarError("No se pudieron cargar los datos: " + Ex.Message);
            }
        }

        
        public string ComprasFuncObtenerCampoProveedor(string Campo)
        {
            if (string.IsNullOrWhiteSpace(Campo))
                throw new ArgumentException("Indique el nombre del campo del proveedor.", nameof(Campo));
            var Proveedor = ComprasFuncObtenerProveedorSeleccionado();
            if (Proveedor == null) return string.Empty;
            if (!Proveedor.Campos.TryGetValue(Campo.Trim(), out object Valor))
                throw new ArgumentException("El proveedor no contiene el campo '" + Campo + "'.", nameof(Campo));
            return Convert.ToString(Valor);
        }

        public ClsProveedorCompras ComprasFuncObtenerProveedorSeleccionado()
        {
            if (_TipoDatos != EnmTipoDatosCompras.Proveedores)
                throw new InvalidOperationException("El combo debe tener TipoDatos = Proveedores.");
            if (ComprasCboEntrada.SelectedIndex < 0) return null;
            DataRowView Seleccion = ComprasCboEntrada.SelectedItem as DataRowView;
            if (Seleccion == null || Seleccion.Row.RowState == DataRowState.Deleted ||
                Seleccion.Row.IsNull("idProveedor")) return null;
            string[] Columnas = { "idProveedor", "nombreProveedor", "nitProveedor", "telefonoProveedor",
                "correoProveedor", "direccionProveedor", "estadoProveedor", "created_at", "updated_at" };
            foreach (string Columna in Columnas)
            {
                if (!Seleccion.Row.Table.Columns.Contains(Columna))
                    return new ClsComboCompras().ComprasFuncObtenerProveedor(Convert.ToInt32(Seleccion.Row["idProveedor"]));
            }
            return new ClsProveedorCompras(Seleccion.Row);
        }

        public void ComprasMetCargarDatos(EnmTipoDatosCompras Tipo, DataTable Tabla,
            string CampoAuto1 = null, string CampoAuto2 = null)
        {
            _TipoDatos = Tipo;  
            ComprasMetCargarDatos(Tabla, CampoAuto1, CampoAuto2);
        }

       
       
        public void ComprasMetCargarDatos(DataTable Tabla, string CampoAuto1 = null, string CampoAuto2 = null)
        {
            if (Tabla == null) throw new ArgumentNullException(nameof(Tabla));

            ComprasProcObtenerConfiguracion(_TipoDatos,
                out string _, out string Valor, out string Mostrar);

            string Auto1 = CampoAuto1 ?? Valor;
            string Auto2 = CampoAuto2 ?? Mostrar;

            foreach (string Columna in new[] { Valor, Mostrar, Auto1, Auto2 })
            {
                if (!Tabla.Columns.Contains(Columna))
                    throw new ArgumentException(
                        "La tabla no contiene la columna '" + Columna + "' requerida por " + _TipoDatos + ".");
            }

            string[] Sugerencias = new string[Tabla.Rows.Count * 2];
            int Indice = 0;
            foreach (DataRow Fila in Tabla.Rows)
            {
                string A = Convert.ToString(Fila[Auto1]);
                string B = Convert.ToString(Fila[Auto2]);
                Sugerencias[Indice++] = A + "-" + B;
                Sugerencias[Indice++] = B + "-" + A;
            }

            AutoCompleteStringCollection Coleccion = new AutoCompleteStringCollection();
            Coleccion.AddRange(Sugerencias);

            ComprasCboEntrada.BeginUpdate();
            try
            {
       
                ComprasCboEntrada.AutoCompleteMode = AutoCompleteMode.None;
                ComprasCboEntrada.DataSource = null;

                ComprasCboEntrada.ValueMember = Valor;
                ComprasCboEntrada.DisplayMember = Mostrar;
                ComprasCboEntrada.DataSource = Tabla;

               
                ComprasCboEntrada.SelectedIndex = -1;

             
                if (_PermitirEscritura)
                {
                    ComprasCboEntrada.AutoCompleteSource = AutoCompleteSource.ListItems;
                    ComprasCboEntrada.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
                else if (ComprasCboEntrada.DropDownStyle == ComboBoxStyle.DropDown)
                {
                    ComprasCboEntrada.AutoCompleteCustomSource = Coleccion;
                    ComprasCboEntrada.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    ComprasCboEntrada.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
            }
            finally
            {
                ComprasCboEntrada.EndUpdate();
            }
        }


    }
}

