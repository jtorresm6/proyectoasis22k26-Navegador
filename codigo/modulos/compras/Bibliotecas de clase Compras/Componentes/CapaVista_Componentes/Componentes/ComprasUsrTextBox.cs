using System;
using CapaControlador_Componentes;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    public enum ComprasEnumTipoEntrada
    {
        Texto,
        Enteros,
        Porcentaje,
        Moneda
    }

    public class ComprasTipoMonedaConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext Contexto) => true;
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext Contexto) => true;

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext Contexto)
        {
            var Codigos = new List<string>();
            foreach (DataRow Moneda in new ClsComboCompras().ComprasFuncEnviarDatos("tblMoneda", "Pk_Moneda", "nombreMoneda").Rows)
                Codigos.Add(Convert.ToString(Moneda["codigoMoneda"]));
            return new StandardValuesCollection(Codigos);
        }
    }
    public class ComprasRichTextBox : RichTextBox
    {
        public ComprasRichTextBox()
        {
            ComprasFiltroRueda.Instalar();
        }

        protected override void OnEnter(EventArgs Evento)
        {
            base.OnEnter(Evento);
            SelectAll();
            if (!IsHandleCreated) return;
            string Texto = Text;
            BeginInvoke(new Action(() =>
            {
                if (!IsDisposed && Focused && Text == Texto)
                    SelectAll();
            }));
        }

        protected override void WndProc(ref Message Mensaje)
        {
            if ((Mensaje.Msg == 0x020A || Mensaje.Msg == 0x020E) && ComprasFiltroRueda.DebeBloquear(this))
            {
                ComprasFiltroRueda.EnviarAlContenedor(this, ref Mensaje);
                return;
            }
            base.WndProc(ref Mensaje);
        }
    }
    
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
        private readonly ClsFondoEntradaCompras _FondoEntrada;

        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        [Category("Compras")]
        [DefaultValue(false)]
        public bool PermitirRuedaMouse { get; set; }

        private HorizontalAlignment _AlineacionMoneda = HorizontalAlignment.Right;

        [Category("Compras")]
        [DefaultValue(HorizontalAlignment.Right)]
        public HorizontalAlignment AlineacionMoneda
        {
            get => _AlineacionMoneda;
            set
            {
                if (!Enum.IsDefined(typeof(HorizontalAlignment), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(HorizontalAlignment));
                _AlineacionMoneda = value;
                ComprasProcAplicarFormatoTexto();
            }
        }
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
        private bool _ModoPorcentaje;
        private string _UltimoTextoPorcentaje = string.Empty;

        [Category("Compras")]
        [DefaultValue(ComprasEnumTipoEntrada.Texto)]
        [RefreshProperties(RefreshProperties.All)]
        public ComprasEnumTipoEntrada TipoEntrada
        {
            get => _ModoPorcentaje ? ComprasEnumTipoEntrada.Porcentaje :
                _SoloEnteros ? ComprasEnumTipoEntrada.Enteros :
                _ModoMoneda ? ComprasEnumTipoEntrada.Moneda : ComprasEnumTipoEntrada.Texto;
            set
            {
                if (!Enum.IsDefined(typeof(ComprasEnumTipoEntrada), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(ComprasEnumTipoEntrada));
                if (TipoEntrada == value) return;
                string Limpio = Text;
                if (_ModoPorcentaje) Limpio = Limpio.TrimEnd().TrimEnd('%').TrimEnd();
                if (_ModoMoneda) Limpio = Limpio.Replace(ComprasFuncObtenerSimboloMoneda(), "").Replace(",", "").Trim();
                _ModoPorcentaje = false;
                SoloEnteros = false;
                ModoMoneda = false;
                Text = Limpio;
                if (value == ComprasEnumTipoEntrada.Enteros) SoloEnteros = true;
                else if (value == ComprasEnumTipoEntrada.Moneda) ModoMoneda = true;
                else if (value == ComprasEnumTipoEntrada.Porcentaje)
                {
                    _UltimoTextoPorcentaje = ComprasFuncEsPorcentaje(Text) ? Text : string.Empty;
                    Text = _UltimoTextoPorcentaje;
                    _ModoPorcentaje = true;
                    ComprasProcFormatearPorcentaje();
                }
            }
        }

        private static bool ComprasFuncEsPorcentaje(string Texto)
        {
            string Numero = Texto.TrimEnd().TrimEnd('%').TrimEnd();
            if (Numero.Length == 0) return Texto.Length == 0;
            string Separador = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string Digitos = Numero.Replace(Separador, "");
            return Digitos.Length > 0 && ComprasFuncEsTextoEntero(Digitos) &&
                decimal.TryParse(Numero, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture,
                    out decimal Valor) && Valor >= 0 && Valor <= 100;
        }

        private void ComprasProcFormatearPorcentaje()
        {
            if (!_ModoPorcentaje || string.IsNullOrWhiteSpace(Text)) return;
            string Numero = Text.TrimEnd().TrimEnd('%').TrimEnd();
            if (decimal.TryParse(Numero, out decimal Valor)) Text = Valor.ToString("0.##") + " %";
        }

        
        private bool _ModoMoneda = false;
        private string _TipoMoneda = string.Empty;
        private string _SimboloMoneda = "Q";
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
        [DefaultValue(typeof(Padding), "6, 6, 6, 1")]
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
                ComprasTxtTexto.SelectionAlignment = _ModoMoneda ? _AlineacionMoneda : _AlineacionTexto;
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
                if (value)
                {
                    _ModoPorcentaje = false;
                    ModoMoneda = false;
                }
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
                if (value)
                {
                    _ModoPorcentaje = false;
                    SoloEnteros = false;
                }
                _ModoMoneda = value;
                if (_ModoMoneda)
                {
                    ComprasProcAplicarFormatoTexto();
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
        [DefaultValue("")]
        [TypeConverter(typeof(ComprasTipoMonedaConverter))]
        public string TipoMoneda
        {
            get => _TipoMoneda;
            set
            {
                string Codigo = (value ?? string.Empty).Trim();
                if (Codigo.Length == 0)
                {
                    ComprasProcConfigurarMoneda(string.Empty, "Q");
                    return;
                }
                foreach (DataRow Moneda in new ClsComboCompras().ComprasFuncEnviarDatos("tblMoneda", "Pk_Moneda", "nombreMoneda").Rows)
                {
                    if (!string.Equals(Convert.ToString(Moneda["codigoMoneda"]), Codigo, StringComparison.OrdinalIgnoreCase)) continue;
                    ComprasMetConfigurarMoneda(Convert.ToString(Moneda["codigoMoneda"]), Convert.ToString(Moneda["simboloMoneda"]));
                    return;
                }
                throw new ArgumentException("La moneda no existe en tblMoneda.", nameof(value));
            }
        }
        
        public void ComprasMetConfigurarMoneda(string Codigo, string Simbolo)
        {
            if (string.IsNullOrWhiteSpace(Codigo)) throw new ArgumentException("Indica el codigo de moneda.", nameof(Codigo));
            if (string.IsNullOrWhiteSpace(Simbolo)) throw new ArgumentException("Indica el simbolo de moneda.", nameof(Simbolo));
            ComprasProcConfigurarMoneda(Codigo.Trim(), Simbolo.Trim());
        }

        private void ComprasProcConfigurarMoneda(string Codigo, string Simbolo)
        {
            string Anterior = _SimboloMoneda;
            _TipoMoneda = Codigo;
            _SimboloMoneda = Simbolo;
            if (!_ModoMoneda) return;
            if (Text.StartsWith(Anterior, StringComparison.Ordinal))
                Text = Simbolo + Text.Substring(Anterior.Length);
            ComprasMetAplicarFormatoMoneda();
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
            _FondoEntrada = new ClsFondoEntradaCompras(this, ComprasProcActualizarColorEntrada);
            DoubleBuffered = true;
            ComprasLblError.ComprasProcConservarRojo();

            ComprasTxtTexto.TextChanged += (Sender, Evento) => ComprasProcTextoCambiado(Evento);
            ComprasTxtTexto.HandleCreated += (Sender, Evento) => ComprasProcAplicarFormatoTexto();
            ComprasTxtTexto.ReadOnlyChanged += (Sender, Evento) => ComprasProcActualizarColorEntrada();
            ComprasTxtTexto.EnabledChanged += (Sender, Evento) => ComprasProcActualizarColorEntrada();
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
                if (_ModoPorcentaje) Text = Text.TrimEnd().TrimEnd('%').TrimEnd();
            };
            ComprasTxtTexto.Leave += (Sender, Evento) =>
            {
                ComprasProcActualizarBorde();
                if (_ModoMoneda) ComprasMetAplicarFormatoMoneda();
                ComprasProcFormatearPorcentaje();
            };
            
            ComprasMetLimpiarError();
        }

        

        internal string ComprasFuncObtenerSimboloMoneda()
        {
            return _SimboloMoneda;
        }

        [Category("Compras")]
        [DefaultValue(false)]
        public bool ConservarPrecisionMoneda { get; set; }

        public bool ComprasFuncLeerMonto(out decimal Monto)
        {
            string Simbolo = ComprasFuncObtenerSimboloMoneda();
            string Numero = Text.Trim();
            if (!ConservarPrecisionMoneda)
                return decimal.TryParse(Numero.Replace(Simbolo, "").Replace(",", "").Trim(), out Monto);

            if (Numero.StartsWith(Simbolo, StringComparison.Ordinal))
                Numero = Numero.Substring(Simbolo.Length).TrimStart();

            Monto = 0;
            if (Numero.IndexOf(',') >= 0 && !System.Text.RegularExpressions.Regex.IsMatch(Numero, @"\A[+-]?[0-9]{1,3}(,[0-9]{3})+(\.[0-9]*)?\z"))
                return false;
            return decimal.TryParse(Numero, NumberStyles.Number, CultureInfo.InvariantCulture, out Monto);
        }

        private string ComprasFuncFormatearMonto(decimal Monto, bool AgruparMiles)
        {
            if (!ConservarPrecisionMoneda)
                return Monto.ToString(AgruparMiles ? "N2" : "0.00");

            int Decimales = Math.Max(2, (decimal.GetBits(Monto)[3] >> 16) & 0xFF);
            return Monto.ToString((AgruparMiles ? "N" : "F") + Decimales, CultureInfo.InvariantCulture);
        }

        private void ComprasMetAplicarFormatoMoneda()
        {
            if (!_ModoMoneda || string.IsNullOrWhiteSpace(this.Text)) return;

            if (ComprasFuncLeerMonto(out decimal Monto))
                Text = $"{ComprasFuncObtenerSimboloMoneda()} {ComprasFuncFormatearMonto(Monto, AgruparMiles: true)}";
        }

        private void ComprasMetRemoverFormatoMoneda()
        {
            if (!_ModoMoneda) return;

            string Simbolo = ComprasFuncObtenerSimboloMoneda();
            if (ComprasFuncLeerMonto(out decimal Monto))
            {
                Text = $"{Simbolo} {ComprasFuncFormatearMonto(Monto, AgruparMiles: false)}";
            }
            else if (!ConservarPrecisionMoneda || string.IsNullOrWhiteSpace(Text))
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

            if ((Evento.KeyChar == '.') && (Text.Remove(SelectionStart, SelectionLength).IndexOf('.') > -1))
            {
                Evento.Handled = true;
            }

            string Simbolo = ComprasFuncObtenerSimboloMoneda();
            if (ComprasTxtTexto.SelectionLength != ComprasTxtTexto.TextLength &&
                ComprasTxtTexto.SelectionStart <= Simbolo.Length && !char.IsControl(Evento.KeyChar))
            {
                ComprasTxtTexto.SelectionStart = Simbolo.Length + 1;
            }
        }

        

        private void ComprasProcTextoCambiado(EventArgs Evento)
        {
            if (_RestaurandoEntero) return;
            if (_ModoPorcentaje)
            {
                if (!ComprasFuncEsPorcentaje(ComprasTxtTexto.Text))
                {
                    int Posicion = ComprasTxtTexto.SelectionStart;
                    _RestaurandoEntero = true;
                    try
                    {
                        ComprasTxtTexto.Text = _UltimoTextoPorcentaje;
                        ComprasTxtTexto.SelectionStart = Math.Min(Posicion, _UltimoTextoPorcentaje.Length);
                    }
                    finally { _RestaurandoEntero = false; }
                    return;
                }
                _UltimoTextoPorcentaje = ComprasTxtTexto.Text;
            }
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
                    ComprasTxtTexto.SelectionStart = Math.Max(Simbolo.Length + 1, PosicionCursor + Simbolo.Length + 1);
                }
                _ProtegiendoMoneda = false;
            }
            
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
            ComprasProcActualizarColorEntrada();
            if (ComprasTlpMain != null) ComprasTlpMain.BackColor = BackColor;
        }

        private void ComprasProcActualizarColorEntrada()
        {
            if (ComprasTxtTexto == null) return;
            Color Fondo = ComprasTxtTexto.ReadOnly || !ComprasTxtTexto.Enabled
                ? ClsTemaCompras.FondoBotones : ClsFondoEntradaCompras.ObtenerColor(this);
            ComprasTxtTexto.BackColor = Fondo;
            if (ComprasPnlInterior != null) ComprasPnlInterior.BackColor = Fondo;
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
