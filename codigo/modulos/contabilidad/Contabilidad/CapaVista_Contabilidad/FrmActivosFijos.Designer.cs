/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: presentación del prototipo de Activos fijos (Dylan). */
using System.Windows.Forms;
using CapaVista_Contabilidad.Componentes;
using Diseno = CapaVista_Contabilidad.Componentes.ClsDisenoPrototiposContabilidad;

namespace CapaVista_Contabilidad
{
    partial class FrmActivosFijos
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            if (Disposing && _Catalogo != null) _Catalogo.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Diseno.ContabilidadMetTitulo(this, "Activos fijos");
            var Ficha = Diseno.ContabilidadFuncTarjeta(this, "ContabilidadPnlFicha", 76, 272);
            ContabilidadTxtDescripcion = Diseno.ContabilidadFuncCampo(Ficha, new TextBox { MaxLength = 255 }, "ContabilidadTxtDescripcion", "Descripción", 24, 20, 520);
            ContabilidadCboModulo = Diseno.ContabilidadFuncCampo(Ficha, Diseno.ContabilidadFuncLista(), "ContabilidadCboModulo", "Módulo", 568, 20, 250);
            ContabilidadTxtCentro = Diseno.ContabilidadFuncCampo(Ficha, new TextBox { MaxLength = 20 }, "ContabilidadTxtCentro", "Código centro de costo", 842, 20, 266);
            ContabilidadCboTipo = Diseno.ContabilidadFuncCampo(Ficha, Diseno.ContabilidadFuncLista(), "ContabilidadCboTipo", "Tipo de activo", 24, 98, 330);
            ContabilidadDtpAdquisicion = Diseno.ContabilidadFuncCampo(Ficha, Diseno.ContabilidadFuncFecha(), "ContabilidadDtpAdquisicion", "Fecha de adquisición", 378, 98, 230);
            ContabilidadDtpInicio = Diseno.ContabilidadFuncCampo(Ficha, Diseno.ContabilidadFuncFecha(), "ContabilidadDtpInicio", "Inicio de uso", 632, 98, 230);
            ContabilidadNudMeses = Diseno.ContabilidadFuncCampo(Ficha, new NumericUpDown { Minimum = 1, Maximum = 1200 }, "ContabilidadNudMeses", "Vida útil (meses)", 886, 98, 222);
            ContabilidadNudCompra = Diseno.ContabilidadFuncCampo(Ficha, Diseno.ContabilidadFuncImporte(), "ContabilidadNudCompra", "Valor de compra (Q)", 24, 176, 330);
            ContabilidadNudResidual = Diseno.ContabilidadFuncCampo(Ficha, Diseno.ContabilidadFuncImporte(), "ContabilidadNudResidual", "Valor residual (Q)", 378, 176, 230);
            Diseno.ContabilidadFuncEtiqueta(Ficha, "ContabilidadLblMetodo", "Método: línea recta", 655, 176, 430);
            var Catalogo = Diseno.ContabilidadFuncTarjeta(this, "ContabilidadPnlCatalogo", 364, 216);
            Diseno.ContabilidadFuncEtiqueta(Catalogo, "ContabilidadLblCatalogo", "Maestro de vida útil", 20, 10, 1092);
            ContabilidadDgvCatalogo = Diseno.ContabilidadFuncTabla(Catalogo, "ContabilidadDgvCatalogo", 40, 160,
                new[] { "Tipo", "Nombre", "Meses" }, new[] { "Tipo", "Nombre", "Vida útil (meses)" });
            ContabilidadDgvCatalogo.ColumnHeadersHeight = 32;
            ContabilidadDgvCatalogo.RowTemplate.Height = 22;
            ContabilidadLblResumen = Diseno.ContabilidadFuncEtiqueta(this, "ContabilidadLblResumen", "", 26, 592, 1130);
            ContabilidadBtnNueva = Diseno.ContabilidadFuncBoton(this, "ContabilidadBtnNueva", "Nuevo activo", 24, 626, 180, EstiloBotonContabilidad.Secundario);
            ContabilidadBtnEjemplo = Diseno.ContabilidadFuncBoton(this, "ContabilidadBtnEjemplo", "Restablecer", 220, 626, 190, EstiloBotonContabilidad.Acento);
            ContabilidadBtnGuardar = Diseno.ContabilidadFuncBoton(this, "ContabilidadBtnGuardar", "Guardar activo", 936, 626, 220);
            ContabilidadBtnGuardar.Icono = Properties.Resources.ico_guardar;
            Name = "FrmActivosFijos";
            ResumeLayout(false);
        }

        private TextBox ContabilidadTxtDescripcion, ContabilidadTxtCentro;
        private ComboBox ContabilidadCboModulo, ContabilidadCboTipo;
        private DateTimePicker ContabilidadDtpAdquisicion, ContabilidadDtpInicio;
        private NumericUpDown ContabilidadNudMeses, ContabilidadNudCompra, ContabilidadNudResidual;
        private Label ContabilidadLblResumen;
        private ClsTablaContabilidad ContabilidadDgvCatalogo;
        private ClsBotonContabilidad ContabilidadBtnNueva, ContabilidadBtnEjemplo, ContabilidadBtnGuardar;
    }
}
