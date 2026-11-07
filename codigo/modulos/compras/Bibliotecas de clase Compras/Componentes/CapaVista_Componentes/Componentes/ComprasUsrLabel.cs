using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta etiquetas configurables con tipografía, alineación e indicador de campo requerido.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public enum TipografiaCompras
    {
        Etiqueta,
        Campo,
        TituloFormulario,
        TituloBienvenida
    }

    [DefaultBindingProperty("Text")]
    [DefaultEvent("Click")]
    public partial class ComprasUsrLabel : UserControl
    {

        private bool _MostrarAsterisco = true;
        private TipografiaCompras _Tipografia = TipografiaCompras.Etiqueta;

        internal void ComprasProcConservarRojo()
        {
            ((ClsEtiquetaCompras)ComprasLblTexto).ComprasConservarRojo = true;
            ColorTexto = Color.Red;
            ComprasLblTexto.Invalidate();
        }

        public ComprasUsrLabel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ComprasProcAplicarTipografia(13F);
            ComprasLblTexto.Font = Font;
            TabStop = false;
            ComprasLblTexto.TextChanged += (Sender, Evento) => OnTextChanged(Evento);
            ComprasLblTexto.Click += (Sender, Evento) => OnClick(Evento);
            ComprasLblTexto.DoubleClick += (Sender, Evento) => OnDoubleClick(Evento);
            ComprasLblAsterisco.Click += (Sender, Evento) => OnClick(Evento);
        }

        [Category("Compras")]
        [Description("Uso tipografico: Tahoma para etiquetas, Segoe UI para campos y Lucida Sans en negrita y cursiva para titulos. Al elegir un uso se aplica su tamano de referencia.")]
        [DefaultValue(TipografiaCompras.Etiqueta)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TipografiaCompras Tipografia
        {
            get { return _Tipografia; }
            set
            {
                if (!Enum.IsDefined(typeof(TipografiaCompras), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(TipografiaCompras));
                if (_Tipografia == value) return;
                _Tipografia = value;
                ComprasProcAplicarTipografia(value == TipografiaCompras.TituloBienvenida ? 42F :
                    value == TipografiaCompras.TituloFormulario ? 18F :
                    value == TipografiaCompras.Campo ? 14F : 13F);
            }
        }

        [Category("Compras")]
        [Description("Tamano de la tipografia en puntos, ajustable segun el diseno y la legibilidad.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float TamanoTipografia
        {
            get { return Font.SizeInPoints; }
            set
            {
                if (float.IsNaN(value) || float.IsInfinity(value) || value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "El tamano debe ser un numero positivo y finito.");
                ComprasProcAplicarTipografia(value);
            }
        }

        public bool ShouldSerializeTamanoTipografia()
        {
            float Referencia = _Tipografia == TipografiaCompras.TituloBienvenida ? 42F :
                _Tipografia == TipografiaCompras.TituloFormulario ? 18F :
                _Tipografia == TipografiaCompras.Campo ? 14F : 13F;
            return Math.Abs(TamanoTipografia - Referencia) > 0.01F;
        }

        private void ComprasProcAplicarTipografia(float Tamano)
        {
            bool EsTitulo = _Tipografia == TipografiaCompras.TituloFormulario ||
                _Tipografia == TipografiaCompras.TituloBienvenida;
            Font = new Font(EsTitulo ? "Lucida Sans" :
                _Tipografia == TipografiaCompras.Campo ? "Segoe UI" : "Tahoma",
                Tamano, EsTitulo ? FontStyle.Bold | FontStyle.Italic : FontStyle.Regular,
                GraphicsUnit.Point);
        }

        [Browsable(true)]
        [Bindable(true)]
        [DefaultValue("Label de Compras")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => ComprasLblTexto == null ? base.Text : ComprasLblTexto.Text;
            set
            {
                if (ComprasLblTexto == null) base.Text = value;
                else ComprasLblTexto.Text = value ?? string.Empty;
            }
        }

        [DefaultValue(ContentAlignment.MiddleRight)]
        public ContentAlignment TextAlign
        {
            get => AlineacionTexto;
            set => AlineacionTexto = value;
        }

        protected override void OnFontChanged(EventArgs Evento)
        {
            base.OnFontChanged(Evento);
            if (ComprasLblTexto != null) ComprasLblTexto.Font = Font;
            if (ComprasLblAsterisco != null) ComprasLblAsterisco.Font = Font;
            if (ComprasTlpMain != null) ComprasTlpMain.Font = Font;
            if (ComprasLblTexto != null) Height = Math.Max(Height, Font.Height + Padding.Vertical);
        }

        protected override void OnForeColorChanged(EventArgs Evento)
        {
            base.OnForeColorChanged(Evento);
            if (ComprasLblTexto != null) ComprasLblTexto.ForeColor = ForeColor;
        }

        protected override void OnBackColorChanged(EventArgs Evento)
        {
            base.OnBackColorChanged(Evento);
            if (ComprasTlpMain != null) ComprasTlpMain.BackColor = BackColor;
        }

        [Category("Compras")]
        [Description("Texto que se mostrará en el label.")]
        [DefaultValue("Label de Compras")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Texto
        {
            get { return Text; }
            set { Text = value; }
        }

        [Category("Compras")]
        [Description("Color del texto del label.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorTexto
        {
            get { return ForeColor; }
            set { ForeColor = value; }
        }

        public bool ShouldSerializeColorTexto() => ColorTexto != ClsTemaCompras.PrincipalNavegador;
        public void ResetColorTexto() => ColorTexto = ClsTemaCompras.PrincipalNavegador;

        [Category("Compras")]
        [Description("Muestra u oculta el asterisco y su columna.")]
        [DefaultValue(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool MostrarAsterisco
        {
            get { return _MostrarAsterisco; }
            set { ComprasMetMostrarAsterisco(value); }
        }

        [Category("Compras")]
        [Description("Define la alineación del texto de la etiqueta.")]
        [DefaultValue(ContentAlignment.MiddleRight)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ContentAlignment AlineacionTexto
        {
            get { return ComprasLblTexto.TextAlign; }
            set { ComprasLblTexto.TextAlign = value; }
        }


        public void ComprasMetMostrarAsterisco(bool Mostrar)
        {
            if (_MostrarAsterisco == Mostrar && ComprasTlpMain.ColumnCount == (Mostrar ? 2 : 1)) return;
            ComprasTlpMain.SuspendLayout();
            try
            {
                if (Mostrar)
                {
                    if (ComprasTlpMain.ColumnCount == 1)
                    {
                        ComprasTlpMain.ColumnCount = 2;
                        ComprasTlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
                    }

                    if (!ComprasTlpMain.Controls.Contains(ComprasLblAsterisco))
                    {
                        ComprasTlpMain.Controls.Add(ComprasLblAsterisco, 1, 0);
                    }

                    if (!ComprasLblAsterisco.Visible) ComprasLblAsterisco.Visible = true;
                }
                else
                {
                    if (ComprasLblAsterisco.Visible) ComprasLblAsterisco.Visible = false;
                    ComprasTlpMain.Controls.Remove(ComprasLblAsterisco);

                    while (ComprasTlpMain.ColumnStyles.Count > 1)
                    {
                        ComprasTlpMain.ColumnStyles.RemoveAt(1);
                    }

                    ComprasTlpMain.ColumnCount = 1;
                }

                _MostrarAsterisco = Mostrar;
            }
            finally
            {
                ComprasTlpMain.ResumeLayout(true);
            }
        }

    }
}
