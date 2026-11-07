using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
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
 *  Define los estilos y estados de los botones de Compras y su serialización para el diseñador.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public enum ComprasTipoBoton
    {
        Normal,
        MDI,
        Libre,
        Tabla
    }

    [DesignerSerializer(typeof(ClsSerializadorButtonCompras), typeof(CodeDomSerializer))]
    public partial class ClsButtonCompras : Button
    {


        private Image _ImagenDeshabilitado;
        private Image _ImagenNormal;
        private bool _ActualizandoImagen;
        private ComprasTipoBoton _TipoBoton;
        private bool _EsActivo;
        private bool _MouseDentro;
        private Font _FuenteModo;
        private bool _AplicarColoresEstado = true;
        private Color _ColorHoverAnterior, _ColorPresionadoAnterior;

        [Category("Compras")]
        [Description("Al desactivarlo, los fondos hover y presionado son transparentes. No cambia el color seleccionado.")]
        [DefaultValue(true)]
        public bool AplicarColoresEstado
        {
            get { return _AplicarColoresEstado; }
            set
            {
                if (_AplicarColoresEstado == value) return;
                if (!value)
                {
                    _ColorHoverAnterior = FlatAppearance.MouseOverBackColor;
                    _ColorPresionadoAnterior = FlatAppearance.MouseDownBackColor;
                }
                _AplicarColoresEstado = value;
                if (value)
                {
                    FlatAppearance.MouseOverBackColor = _ColorHoverAnterior;
                    FlatAppearance.MouseDownBackColor = _ColorPresionadoAnterior;
                }
                else
                {
                    FlatAppearance.MouseOverBackColor = Color.Transparent;
                    FlatAppearance.MouseDownBackColor = Color.Transparent;
                }
                ComprasProcActualizarEstadoTabla();
                Invalidate();
            }
        }

        [Category("Compras")]
        [Description("Selecciona el estilo normal, MDI, de tamaño libre o de tabla.")]
        [DefaultValue(ComprasTipoBoton.Normal)]
        [RefreshProperties(RefreshProperties.All)]
        public ComprasTipoBoton TipoBoton
        {
            get => _TipoBoton;
            set
            {
                if (!Enum.IsDefined(typeof(ComprasTipoBoton), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(ComprasTipoBoton));
                if (_TipoBoton == value) return;
                _TipoBoton = value;
                ComprasProcAplicarTipo();
            }
        }

        [Category("Compras")]
        [Description("Indica si el botón de tabla está seleccionado.")]
        [DefaultValue(false)]
        public bool EsActivo
        {
            get => _EsActivo;
            set
            {
                if (_EsActivo == value) return;
                _EsActivo = value;
                ComprasProcActualizarEstadoTabla();
            }
        }

        [Category("Compras")]
        [Description("Imagen de fondo que se muestra cuando el botón está deshabilitado.")]
        [DefaultValue(null)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image ImagenDeshabilitado
        {
            get { return _ImagenDeshabilitado; }
            set
            {
                _ImagenDeshabilitado = value;
                ComprasProcActualizarImagen();
            }
        }

        public ClsButtonCompras()
        {
            ComprasProcAplicarTipo();
        }

        private void ComprasProcAplicarTipo()
        {
            Size Tamano = Size;
            SuspendLayout();
            try
            {
                AutoSize = false;
                MinimumSize = Size.Empty;
                MaximumSize = Size.Empty;
                BackColor = ClsTemaCompras.FondoBotones;
                ForeColor = ClsTemaCompras.PrincipalNavegador;
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                FlatAppearance.BorderColor = ClsTemaCompras.PrincipalNavegador;
                FlatAppearance.MouseOverBackColor = ClsTemaCompras.BotonHover;
                FlatAppearance.MouseDownBackColor = ClsTemaCompras.BotonPresionado;
                UseVisualStyleBackColor = false;
                BackgroundImageLayout = ImageLayout.Stretch;
                Cursor = Enabled ? Cursors.Hand : Cursors.Default;
                Margin = new Padding(0);
                Anchor = AnchorStyles.None;
                TextAlign = ContentAlignment.MiddleCenter;
                Font FuenteAnterior = _FuenteModo;
                _FuenteModo = _TipoBoton == ComprasTipoBoton.MDI ? new Font("Tahoma", 9F, FontStyle.Regular) :
                    _TipoBoton == ComprasTipoBoton.Tabla ? new Font("Segoe UI", 9F, FontStyle.Regular) : null;
                Font = _FuenteModo ?? Control.DefaultFont;
                FuenteAnterior?.Dispose();

                switch (_TipoBoton)
                {
                    case ComprasTipoBoton.Normal:
                        Size = new Size(80, 80);
                        MinimumSize = Size;
                        MaximumSize = Size;
                        break;
                    case ComprasTipoBoton.MDI:
                        Size = new Size(352, 52);
                        Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        FlatStyle = FlatStyle.Popup;
                        FlatAppearance.BorderSize = 1;
                        TextAlign = ContentAlignment.MiddleLeft;
                        break;
                    case ComprasTipoBoton.Libre:
                        Size = Tamano;
                        break;
                    case ComprasTipoBoton.Tabla:
                        Size = new Size(36, 30);
                        Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        Margin = new Padding(2);
                        FlatAppearance.MouseOverBackColor = ClsTemaCompras.AcentoNavegador;
                        FlatAppearance.MouseDownBackColor = ClsTemaCompras.PrincipalNavegador;
                        ComprasProcActualizarEstadoTabla();
                        break;
                }
            }
            finally
            {
                ResumeLayout(true);
            }
            Invalidate();
        }

        protected override Size DefaultSize => new Size(80, 80);

        protected override void OnPaint(PaintEventArgs Evento)
        {
            if (!_AplicarColoresEstado)
            {
                FlatAppearance.MouseOverBackColor = Color.Transparent;
                FlatAppearance.MouseDownBackColor = Color.Transparent;
            }
            base.OnPaint(Evento);
            if (!Enabled && BackgroundImage != null && _ImagenDeshabilitado == null)
            {
                Evento.Graphics.Clear(ClsTemaCompras.FondoBotones);
                float Escala = System.Math.Min((float)ClientSize.Width / BackgroundImage.Width,
                    (float)ClientSize.Height / BackgroundImage.Height);
                int Ancho = System.Math.Max(1, (int)(BackgroundImage.Width * Escala));
                int Alto = System.Math.Max(1, (int)(BackgroundImage.Height * Escala));
                var AreaImagen = new Rectangle((ClientSize.Width - Ancho) / 2, (ClientSize.Height - Alto) / 2, Ancho, Alto);
                using (var Atributos = new System.Drawing.Imaging.ImageAttributes())
                {
                    Atributos.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(new[]
                    {
                        new[] { 0.12F, 0.12F, 0.12F, 0F, 0F },
                        new[] { 0.12F, 0.12F, 0.12F, 0F, 0F },
                        new[] { 0.12F, 0.12F, 0.12F, 0F, 0F },
                        new[] { 0F, 0F, 0F, 1F, 0F },
                        new[] { 0.6F, 0.6F, 0.6F, 0F, 1F }
                    }));
                    Evento.Graphics.DrawImage(BackgroundImage, AreaImagen, 0, 0, BackgroundImage.Width,
                        BackgroundImage.Height, GraphicsUnit.Pixel, Atributos);
                }
            }
            if (Focused && ShowFocusCues)
            {
                var Area = ClientRectangle;
                Area.Inflate(-2, -2);
                ControlPaint.DrawBorder(Evento.Graphics, Area, ClsTemaCompras.AcentoNavegador, ButtonBorderStyle.Solid);
            }
        }

        protected override void OnBackgroundImageChanged(EventArgs Evento)
        {
            if (!_ActualizandoImagen)
            {
                _ImagenNormal = BackgroundImage;
                ComprasProcActualizarImagen();
            }

            base.OnBackgroundImageChanged(Evento);
        }

        protected override void OnEnabledChanged(EventArgs Evento)
        {
            ComprasProcActualizarImagen();
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            ComprasProcActualizarEstadoTabla();

            base.OnEnabledChanged(Evento);
        }

        protected override void OnMouseEnter(EventArgs Evento)
        {
            base.OnMouseEnter(Evento);
            _MouseDentro = true;
            ComprasProcActualizarEstadoTabla();
        }

        protected override void OnMouseLeave(EventArgs Evento)
        {
            base.OnMouseLeave(Evento);
            _MouseDentro = false;
            ComprasProcActualizarEstadoTabla();
        }

        private void ComprasProcActualizarEstadoTabla()
        {
            if (_TipoBoton != ComprasTipoBoton.Tabla) return;
            BackColor = !Enabled ? ClsTemaCompras.FondoBotones :
                _EsActivo || (_AplicarColoresEstado && _MouseDentro) ? ClsTemaCompras.AcentoNavegador : ClsTemaCompras.PrincipalNavegador;
            ForeColor = Enabled ? Color.White : ClsTemaCompras.PrincipalNavegador;
        }

        private void ComprasProcActualizarImagen()
        {
            Image ImagenActual = !Enabled && _ImagenDeshabilitado != null ? _ImagenDeshabilitado : _ImagenNormal;

            if (ReferenceEquals(BackgroundImage, ImagenActual))
            {
                return;
            }

            _ActualizandoImagen = true;

            try
            {
                BackgroundImage = ImagenActual;
            }
            finally
            {
                _ActualizandoImagen = false;
            }
        }

    }

    public sealed class ClsSerializadorButtonCompras : CodeDomSerializer
    {
        public override object Serialize(IDesignerSerializationManager Gestor, object Valor)
        {
            CodeDomSerializer Serializador = (CodeDomSerializer)Gestor.GetSerializer(typeof(Button), typeof(CodeDomSerializer));
            object Resultado = Serializador.Serialize(Gestor, Valor);
            CodeStatementCollection Sentencias = Resultado as CodeStatementCollection;
            if (Sentencias == null) return Resultado;

            CodeAssignStatement Tipo = null;
            foreach (CodeStatement Sentencia in Sentencias)
            {
                CodeAssignStatement Asignacion = Sentencia as CodeAssignStatement;
                CodePropertyReferenceExpression Propiedad = Asignacion?.Left as CodePropertyReferenceExpression;
                if (Propiedad?.PropertyName == nameof(ClsButtonCompras.TipoBoton))
                {
                    Tipo = Asignacion;
                    break;
                }
            }

            if (Tipo == null) return Resultado;
            Sentencias.Remove(Tipo);
            int Posicion = 0;
            for (int Indice = 0; Indice < Sentencias.Count; Indice++)
            {
                CodeAssignStatement Asignacion = Sentencias[Indice] as CodeAssignStatement;
                if (Sentencias[Indice] is CodeVariableDeclarationStatement || Asignacion?.Right is CodeObjectCreateExpression)
                    Posicion = Indice + 1;
                else if (Asignacion?.Left is CodePropertyReferenceExpression)
                    break;
            }
            Sentencias.Insert(Posicion, Tipo);
            return Resultado;
        }
    }
}
