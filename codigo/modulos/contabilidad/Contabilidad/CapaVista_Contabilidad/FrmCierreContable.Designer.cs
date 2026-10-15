/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: presentación del prototipo de Cierre contable (Oskar). */
using System.Windows.Forms;
using CapaVista_Contabilidad.Componentes;
using Diseno = CapaVista_Contabilidad.Componentes.ClsDisenoPrototiposContabilidad;

namespace CapaVista_Contabilidad
{
    partial class FrmCierreContable
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            if (Disposing && _Polizas != null) _Polizas.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Diseno.ContabilidadMetTitulo(this, "Cierre contable");
            var Encabezado = Diseno.ContabilidadFuncTarjeta(this, "ContabilidadPnlPeriodo", 76, 130);
            ContabilidadDtpPeriodo = Diseno.ContabilidadFuncCampo(Encabezado, Diseno.ContabilidadFuncFecha(), "ContabilidadDtpPeriodo", "Período contable", 24, 20, 240);
            ContabilidadDtpPeriodo.CustomFormat = "yyyy-MM";
            ContabilidadDtpPeriodo.ShowUpDown = true;
            ContabilidadBtnRevisar = Diseno.ContabilidadFuncBoton(Encabezado, "ContabilidadBtnRevisar", "Revisar período", 290, 40, 210, EstiloBotonContabilidad.Acento);
            Diseno.ContabilidadFuncEtiqueta(Encabezado, "ContabilidadLblAlcance", "Las pólizas anuladas no se incluyen en los totales del cierre.", 530, 26, 560, 64);
            var Detalle = Diseno.ContabilidadFuncTarjeta(this, "ContabilidadPnlPolizas", 222, 324);
            Diseno.ContabilidadFuncEtiqueta(Detalle, "ContabilidadLblPolizas", "Pólizas del período", 20, 14, 1092);
            ContabilidadDgvPolizas = Diseno.ContabilidadFuncTabla(Detalle, "ContabilidadDgvPolizas", 52, 214,
                new[] { "Poliza", "Fecha", "Estado", "Debe", "Haber", "Diferencia" }, new[] { "Póliza", "Fecha", "Estado", "Debe (Q)", "Haber (Q)", "Diferencia (Q)" });
            ContabilidadLblValidacion = Diseno.ContabilidadFuncEtiqueta(Detalle, "ContabilidadLblValidacion", "", 20, 280, 1092, 36);
            ContabilidadLblResumen = Diseno.ContabilidadFuncEtiqueta(this, "ContabilidadLblResumen", "", 26, 558, 1130);
            ContabilidadBtnSimular = Diseno.ContabilidadFuncBoton(this, "ContabilidadBtnSimular", "Cerrar período", 936, 626, 220, EstiloBotonContabilidad.Dorado);
            ContabilidadBtnSimular.Icono = Properties.Resources.ico_aplicar;
            Name = "FrmCierreContable";
            ResumeLayout(false);
        }

        private DateTimePicker ContabilidadDtpPeriodo;
        private ClsTablaContabilidad ContabilidadDgvPolizas;
        private Label ContabilidadLblValidacion, ContabilidadLblResumen;
        private ClsBotonContabilidad ContabilidadBtnRevisar, ContabilidadBtnSimular;
    }
}
