using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Pedro José Gómez Villalobos
 * Carné : 0901-23-4868
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta un campo de texto reutilizable con validación, mensajes de
 *  error y formato monetario para los formularios de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    
    public enum ComprasEnumTipoMoneda
    {
        Quetzales,
        Dolares,
        Euros
    }

    [DefaultBindingProperty("Text")]
    [DefaultEvent("TextChanged")]
    public partial class ComprasUsrTextBox : UserControl
    {
        private bool _ErrorVisible;
        private bool _MostrarEtiquetaError = true;
        private int _AlturaEtiquetaError = 22;
        private int _AlturaErrorAmpliada;
        private int _AlturaFilaErrorAplicada = 30;
        private int _VersionTexto;
        private HorizontalAlignment _AlineacionTexto = HorizontalAlignment.Left;
        private int _SangriaTexto;
        private bool _AplicandoFormatoTexto;
        private bool _SoloEnteros;
        private bool _RestaurandoEntero;
        private string _UltimoTextoEntero = string.Empty;

        
        private bool _ModoMoneda = false;
        private ComprasEnumTipoMoneda _TipoMoneda = ComprasEnumTipoMoneda.Quetzales;
        private bool _ProtegiendoMoneda = false;

        [Category("Compras")]
        [Description("Alineaci?n del texto: izquierda, centro o derecha. El modo moneda utiliza derecha.")]
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment AlineacionTexto
        {
            get => _AlineacionTexto;
            set
            {
                if (!Enum.IsDefined(typeof(HorizontalAlignment), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(HorizontalAlignment));
                _AlineacionTexto = value;
                ComprasProcAplicarFormatoTexto();
            }
        }

        [Category("Compras")]
        [Description("Sangr?a del p?rrafo en p?xeles desde el margen izquierdo.")]
        [DefaultValue(0)]
        public int SangriaTexto
        {
            get => _SangriaTexto;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                _SangriaTexto = value;
                ComprasProcAplicarFormatoTexto();
            }
        }

        [Category("Compras")]
        [Description("Espacio interior en p?xeles entre el borde y el texto.")]
        [DefaultValue(typeof(Padding), "6, 1, 6, 1")]
        public Padding EspaciadoTexto
        {
            get => ComprasPnlInterior.Padding;
            set
            {
                if (value.Left < 0 || value.Top < 0 || value.Right < 0 || value.Bottom < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                ComprasPnlInterior.Padding = value;
            }
        }

        private void ComprasProcAplicarFormatoTexto()
        {
            if (ComprasTxtTexto == null || _AplicandoFormatoTexto) return;
            _AplicandoFormatoTexto = true;
            int Inicio = ComprasTxtTexto.SelectionStart;
            int Longitud = ComprasTxtTexto.SelectionLength;
            try
            {
                ComprasTxtTexto.SelectAll();
                ComprasTxtTexto.SelectionAlignment = _ModoMoneda ? HorizontalAlignment.Right : _AlineacionTexto;
                ComprasTxtTexto.SelectionIndent = _SangriaTexto;
                ComprasTxtTexto.Select(Inicio, Longitud);
            }
            finally
            {
                _AplicandoFormatoTexto = false;
            }
        }

        [Category("Compras")]
        [Description("Permite solo d?gitos del 0 al 9, sin signos, decimales ni s?mbolos de moneda. Rechaza texto pegado no v?lido y desactiva ModoMoneda.")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool SoloEnteros
        {
            get => _SoloEnteros;
            set
            {
                if (_SoloEnteros == value) return;
                if (value) ModoMoneda = false;
                _SoloEnteros = value;
                if (value)
                {
                    _UltimoTextoEntero = ComprasFuncEsTextoEntero(Text) ? Text : string.Empty;
                    if (Text != _UltimoTextoEntero) Text = _UltimoTextoEntero;
                }
            }
        }

        private static bool ComprasFuncEsTextoEntero(string Texto)
        {
            for (int Indice = 0; Indice < Texto.Length; Indice++)
            {
                char Caracter = Texto[Indice];
                if (Caracter < '0' || Caracter > '9') return false;
            }
            return true;
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
        [Description("Activa o desactiva el comportamiento de máscara de moneda en el TextBox.")]
        [DefaultValue(false)]
        public bool ModoMoneda
        {
            get => _ModoMoneda;
            set
            {
                if (value) SoloEnteros = false;
                _ModoMoneda = value;
                if (_ModoMoneda)
                {
                    ComprasTxtTexto.SelectionAlignment = HorizontalAlignment.Right;
                    ComprasMetAplicarFormatoMoneda();
                }
                else
                {
                    ComprasProcAplicarFormatoTexto();
                }
            }
        }

        [Category("Compras")]
        [Description("Define el tipo de moneda a mostrar cuando ModoMoneda está activado.")]
        [DefaultValue(ComprasEnumTipoMoneda.Quetzales)]
        public ComprasEnumTipoMoneda TipoMoneda
        {
            get => _TipoMoneda;
            set
            {
                _TipoMoneda = value;
                if (_ModoMoneda) ComprasMetAplicarFormatoMoneda();
            }
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

        public ComprasUsrTextBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ComprasLblError.ComprasProcConservarRojo();

            ComprasTxtTexto.TextChanged += (Sender, Evento) => ComprasProcTextoCambiado(Evento);
            ComprasTxtTexto.HandleCreated += (Sender, Evento) => ComprasProcAplicarFormatoTexto();
            Enter += (Sender, Evento) => ComprasTxtTexto.Focus();

            ComprasTxtTexto.KeyDown += (Sender, Evento) => OnKeyDown(Evento);
            ComprasTxtTexto.KeyPress += (Sender, Evento) =>
            
            {
                if (_ModoMoneda) ComprasProcValidarTeclasMoneda(Evento);
                OnKeyPress(Evento);
                if (_SoloEnteros && !char.IsControl(Evento.KeyChar))
                {
                    string Propuesto = Text.Remove(SelectionStart, SelectionLength)
                        .Insert(SelectionStart, Evento.KeyChar.ToString());
                    if (!ComprasFuncEsTextoEntero(Propuesto)) Evento.Handled = true;
                }
            };
            
            ComprasTxtTexto.KeyUp += (Sender, Evento) => OnKeyUp(Evento);
            ComprasTxtTexto.MouseClick += (Sender, Evento) => OnMouseClick(Evento);

            
            ComprasTxtTexto.Enter += (Sender, Evento) =>
            {
                ComprasProcActualizarBorde();
                if (_ModoMoneda) ComprasMetRemoverFormatoMoneda();
            };
            ComprasTxtTexto.Leave += (Sender, Evento) =>
            {
                ComprasProcActualizarBorde();
                if (_ModoMoneda) ComprasMetAplicarFormatoMoneda();
            };
            
            ComprasMetLimpiarError();
        }

        

        private string ComprasFuncObtenerSimboloMoneda()
        {
            switch (_TipoMoneda)
            {
                case ComprasEnumTipoMoneda.Dolares: return "$";
                case ComprasEnumTipoMoneda.Euros: return "€";
                case ComprasEnumTipoMoneda.Quetzales:
                default: return "Q";
            }
        }

        private void ComprasMetAplicarFormatoMoneda()
        {
            if (!_ModoMoneda || string.IsNullOrWhiteSpace(this.Text)) return;

            string TextoPuro = this.Text.Replace(ComprasFuncObtenerSimboloMoneda(), "").Replace(",", "").Trim();
            if (decimal.TryParse(TextoPuro, out decimal Monto))
            {
                this.Text = $"{ComprasFuncObtenerSimboloMoneda()} {Monto:N2}";
            }
        }

        private void ComprasMetRemoverFormatoMoneda()
        {
            if (!_ModoMoneda) return;

            string Simbolo = ComprasFuncObtenerSimboloMoneda();
            string TextoPuro = this.Text.Replace(Simbolo, "").Replace(",", "").Trim();

            if (decimal.TryParse(TextoPuro, out decimal Monto))
            {
                this.Text = $"{Simbolo} {Monto.ToString("0.00")}";
            }
            else
            {
                this.Text = $"{Simbolo} ";
            }
        }

        private void ComprasProcValidarTeclasMoneda(KeyPressEventArgs Evento)
        {
            if (Evento.KeyChar == (char)Keys.Enter)
            {
                Evento.Handled = true;
                Form ParentForm = this.FindForm();
                if (ParentForm != null) ParentForm.ActiveControl = null;
                return;
            }

            if (!char.IsControl(Evento.KeyChar) && !char.IsDigit(Evento.KeyChar) && (Evento.KeyChar != '.'))
            {
                Evento.Handled = true;
            }

            if ((Evento.KeyChar == '.') && (this.Text.IndexOf('.') > -1))
            {
                Evento.Handled = true;
            }

            string Simbolo = ComprasFuncObtenerSimboloMoneda();
            if (ComprasTxtTexto.SelectionStart <= Simbolo.Length && !char.IsControl(Evento.KeyChar))
            {
                ComprasTxtTexto.SelectionStart = Simbolo.Length + 1;
            }
        }

        

        private void ComprasProcTextoCambiado(EventArgs Evento)
        {
            if (_RestaurandoEntero) return;
            if (_SoloEnteros)
            {
                if (!ComprasFuncEsTextoEntero(ComprasTxtTexto.Text))
                {
                    int Posicion = ComprasTxtTexto.SelectionStart;
                    _RestaurandoEntero = true;
                    try
                    {
                        ComprasTxtTexto.Text = _UltimoTextoEntero;
                        ComprasTxtTexto.SelectionStart = Math.Min(Posicion, _UltimoTextoEntero.Length);
                    }
                    finally
                    {
                        _RestaurandoEntero = false;
                    }
                    return;
                }
                _UltimoTextoEntero = ComprasTxtTexto.Text;
            }
            _VersionTexto++;
            ComprasMetLimpiarError();

            
            if (_ModoMoneda && !_ProtegiendoMoneda && ComprasTxtTexto.Focused)
            {
                _ProtegiendoMoneda = true;
                string Simbolo = ComprasFuncObtenerSimboloMoneda();

                if (!string.IsNullOrEmpty(ComprasTxtTexto.Text) && !ComprasTxtTexto.Text.StartsWith(Simbolo))
                {
                    int PosicionCursor = ComprasTxtTexto.SelectionStart;
                    string TextoLimpio = ComprasTxtTexto.Text.Replace(Simbolo, "").TrimStart();

                    ComprasTxtTexto.Text = $"{Simbolo} {TextoLimpio}";
                    ComprasTxtTexto.SelectionStart = Math.Max(Simbolo.Length + 1, PosicionCursor + Simbolo.Length);
                }
                _ProtegiendoMoneda = false;
            }
            
            ComprasProcAplicarFormatoTexto();
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
                return ComprasTxtTexto == null ? base.Text : ComprasTxtTexto.Text;
            }
            set
            {
                if (ComprasTxtTexto != null)
                {
                    string Nuevo = value ?? string.Empty;
                    bool Cambiado = ComprasTxtTexto.Text != Nuevo;
                    int Version = _VersionTexto;
                    ComprasTxtTexto.Text = Nuevo;
                    if (Cambiado && Version == _VersionTexto)
                        ComprasProcTextoCambiado(EventArgs.Empty);
                }
                else
                {
                    base.Text = value;
                }
            }
        }

        internal RichTextBox ComprasTxtEntrada => ComprasTxtTexto;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RichTextBox Entrada => ComprasTxtTexto;

        [DefaultValue(false)]
        public bool ReadOnly { get => ComprasTxtTexto.ReadOnly; set => ComprasTxtTexto.ReadOnly = value; }

        [Category("Compras")]
        [Description("Permite escribir varias líneas cuando es True y una sola línea cuando es False.")]
        [DefaultValue(false)]
        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Multiline { get => ComprasTxtTexto.Multiline; set => ComprasTxtTexto.Multiline = value; }

        [Browsable(false)]
        public int SelectionStart { get => ComprasTxtTexto.SelectionStart; set => ComprasTxtTexto.SelectionStart = value; }
        [Browsable(false)]
        public int SelectionLength { get => ComprasTxtTexto.SelectionLength; set => ComprasTxtTexto.SelectionLength = value; }
        [Browsable(false)]
        public string SelectedText { get => ComprasTxtTexto.SelectedText; set => ComprasTxtTexto.SelectedText = value; }
        [Browsable(false)]
        public int TextLength => ComprasTxtTexto.TextLength;
        public void Clear() => ComprasTxtTexto.Clear();
        public void Select(int Inicio, int Longitud) => ComprasTxtTexto.Select(Inicio, Longitud);
        public void SelectAll() => ComprasTxtTexto.SelectAll();
        public void AppendText(string Texto) => ComprasTxtTexto.AppendText(Texto);
        public void Copy() => ComprasTxtTexto.Copy();
        public void Cut() => ComprasTxtTexto.Cut();
        public void Paste() => ComprasTxtTexto.Paste();
        public void Undo() => ComprasTxtTexto.Undo();

        protected override void OnFontChanged(EventArgs Evento)
        {
            base.OnFontChanged(Evento);
            if (ComprasTxtTexto != null) ComprasTxtTexto.Font = Font;
        }

        protected override void OnForeColorChanged(EventArgs Evento)
        {
            base.OnForeColorChanged(Evento);
            if (ComprasTxtTexto != null) ComprasTxtTexto.ForeColor = ForeColor;
        }

        protected override void OnBackColorChanged(EventArgs Evento)
        {
            base.OnBackColorChanged(Evento);
            if (ComprasTxtTexto != null) ComprasTxtTexto.BackColor = BackColor;
            if (ComprasPnlInterior != null) ComprasPnlInterior.BackColor = BackColor;
            if (ComprasTlpMain != null) ComprasTlpMain.BackColor = BackColor;
        }

        private void ComprasProcActualizarBorde()
        {
            ComprasPnlBorde.BackColor = _ErrorVisible ? Color.Red :
                ComprasTxtTexto.Focused ? ClsTemaCompras.AcentoNavegador : ClsTemaCompras.PrincipalNavegador;
        }

        protected override void OnCausesValidationChanged(EventArgs Evento)
        {
            base.OnCausesValidationChanged(Evento);
            if (ComprasTxtTexto != null) ComprasTxtTexto.CausesValidation = CausesValidation;
        }

        public int MaxLength
        {
            get
            {
                return ComprasTxtTexto.MaxLength;
            }
            set
            {
                ComprasTxtTexto.MaxLength = value;
            }
        }

        public event KeyEventHandler TextoKeyDown
        {
            add
            {
                ComprasTxtTexto.KeyDown += value;
            }
            remove
            {
                ComprasTxtTexto.KeyDown -= value;
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
    }
}