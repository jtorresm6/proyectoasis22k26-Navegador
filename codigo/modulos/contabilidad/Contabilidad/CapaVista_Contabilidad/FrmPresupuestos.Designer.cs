/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: presentación del prototipo de Presupuestos (Mario). */
using System.Windows.Forms;
using CapaVista_Contabilidad.Componentes;
using Diseno = CapaVista_Contabilidad.Componentes.ClsDisenoPrototiposContabilidad;

namespace CapaVista_Contabilidad
{
    partial class FrmPresupuestos
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            if (Disposing && _Detalle != null) _Detalle.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Diseno.ContabilidadMetTitulo(this, "Presupuestos");
            var Encabezado = Diseno.ContabilidadFuncTarjeta(this, "ContabilidadPnlEncabezado", 76, 112);
            ContabilidadDtpInicio = Diseno.ContabilidadFuncCampo(Encabezado, Diseno.ContabilidadFuncFecha(), "ContabilidadDtpInicio", "Fecha de inicio", 24, 20, 240);
            ContabilidadDtpCierre = Diseno.ContabilidadFuncCampo(Encabezado, Diseno.ContabilidadFuncFecha(), "ContabilidadDtpCierre", "Fecha de cierre (opcional)", 290, 20, 240);
            ContabilidadDtpCierre.ShowCheckBox = true;
            ContabilidadNudGastado = Diseno.ContabilidadFuncCampo(Encabezado, Diseno.ContabilidadFuncImporte(), "ContabilidadNudGastado", "Gastado (Q)", 556, 20, 240);
            var Detalle = Diseno.ContabilidadFuncTarjeta(this, "ContabilidadPnlDetalle", 204, 342);
            ContabilidadTxtNombre = Diseno.ContabilidadFuncCampo(Detalle, new TextBox { MaxLength = 150 }, "ContabilidadTxtNombre", "Nombre de la partida", 20, 16, 365);
            ContabilidadCboModulo = Diseno.ContabilidadFuncCampo(Detalle, Diseno.ContabilidadFuncLista(), "ContabilidadCboModulo", "Módulo", 405, 16, 230);
            ContabilidadNudCantidad = Diseno.ContabilidadFuncCampo(Detalle, Diseno.ContabilidadFuncImporte(), "ContabilidadNudCantidad", "Asignación (Q)", 655, 16, 230);
            ContabilidadBtnAgregar = Diseno.ContabilidadFuncBoton(Detalle, "ContabilidadBtnAgregar", "Agregar", 907, 34, 184, EstiloBotonContabilidad.Acento);
            ContabilidadBtnAgregar.Icono = Properties.Resources.ico_mas;
            ContabilidadDgvDetalle = Diseno.ContabilidadFuncTabla(Detalle, "ContabilidadDgvDetalle", 92, 228,
                new[] { "Nombre", "Modulo", "Cantidad" }, new[] { "Partida presupuestaria", "Módulo", "Asignación (Q)" });
            ContabilidadLblResumen = Diseno.ContabilidadFuncEtiqueta(this, "ContabilidadLblResumen", "", 26, 558, 1128);
            ContabilidadBtnNueva = Diseno.ContabilidadFuncBoton(this, "ContabilidadBtnNueva", "Nuevo", 24, 626, 150, EstiloBotonContabilidad.Secundario);
            ContabilidadBtnQuitar = Diseno.ContabilidadFuncBoton(this, "ContabilidadBtnQuitar", "Quitar partida", 190, 626, 180, EstiloBotonContabilidad.Secundario);
            ContabilidadBtnEjemplo = Diseno.ContabilidadFuncBoton(this, "ContabilidadBtnEjemplo", "Restablecer", 386, 626, 190, EstiloBotonContabilidad.Acento);
            ContabilidadBtnGuardar = Diseno.ContabilidadFuncBoton(this, "ContabilidadBtnGuardar", "Guardar presupuesto", 916, 626, 240);
            ContabilidadBtnGuardar.Icono = Properties.Resources.ico_guardar;
            Name = "FrmPresupuestos";
            ResumeLayout(false);
        }

        private DateTimePicker ContabilidadDtpInicio, ContabilidadDtpCierre;
        private NumericUpDown ContabilidadNudGastado, ContabilidadNudCantidad;
        private TextBox ContabilidadTxtNombre;
        private ComboBox ContabilidadCboModulo;
        private ClsTablaContabilidad ContabilidadDgvDetalle;
        private Label ContabilidadLblResumen;
        private ClsBotonContabilidad ContabilidadBtnAgregar, ContabilidadBtnNueva, ContabilidadBtnQuitar, ContabilidadBtnEjemplo, ContabilidadBtnGuardar;
    }
}
