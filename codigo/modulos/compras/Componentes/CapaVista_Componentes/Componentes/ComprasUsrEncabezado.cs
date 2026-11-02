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
 *  Presenta el código, título y descripción de los formularios de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    [DefaultProperty("Titulo")]
    public partial class ComprasUsrEncabezado : UserControl
    {
        private string _Titulo = "Titulo";
        private string _Codigo = "6000";
        private readonly Image _IconoPredeterminado;

        public ComprasUsrEncabezado()
        {
            InitializeComponent();
            _IconoPredeterminado = ComprasPbxIcono.BackgroundImage;
            ComprasProcActualizarTitulo();
            DoubleBuffered = true;
            TabStop = false;
        }

        [Category("Compras")]
        [Description("T?tulo principal del encabezado.")]
        [Browsable(true)]
        [Bindable(true)]
        [Localizable(true)]
        [DefaultValue("Titulo")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Titulo
        {
            get => _Titulo;
            set
            {
                _Titulo = value ?? string.Empty;
                ComprasProcActualizarTitulo();
            }
        }

        [Category("Compras")]
        [Description("Descripci?n del formulario o proceso.")]
        [Browsable(true)]
        [Bindable(true)]
        [Localizable(true)]
        [DefaultValue("Descripcion")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Descripcion
        {
            get => ComprasLblDescripcion.Texto;
            set => ComprasLblDescripcion.Texto = value ?? string.Empty;
        }

        [Category("Compras")]
        [Description("C?digo del formulario de Compras, en el rango 6000?6999.")]
        [Browsable(true)]
        [Bindable(true)]
        [Localizable(true)]
        [DefaultValue("6000")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Codigo
        {
            get => _Codigo;
            set
            {
                _Codigo = value ?? string.Empty;
                ComprasProcActualizarTitulo();
            }
        }

        [Category("Compras")]
        [Description("Imagen del encabezado. Restablecer recupera el icono original.")]
        [Browsable(true)]
        [Localizable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Icono
        {
            get => ComprasPbxIcono.BackgroundImage;
            set => ComprasPbxIcono.BackgroundImage = value;
        }

        public bool ShouldSerializeIcono() => !ReferenceEquals(Icono, _IconoPredeterminado);

        public void ResetIcono() => Icono = _IconoPredeterminado;

        private void ComprasProcActualizarTitulo()
        {
            ComprasLblTitulo.Texto = _Titulo.ToUpperInvariant();
        }
    }
}
