/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: presentación de estado de resultados de ejemplo. */
using System.Windows.Forms;
using CapaVista_Contabilidad.Componentes;
using Diseno = CapaVista_Contabilidad.Componentes.ClsDisenoPrototiposContabilidad;

namespace CapaVista_Contabilidad
{
    partial class FrmEstadosFinancieros
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            if (Disposing && _Resultado != null) _Resultado.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Diseno.ContabilidadMetTitulo(this, "Estados financieros");
            var Filtros = Diseno.ContabilidadFuncTarjeta(this, "ContabilidadPnlFiltros", 76, 112);
            ContabilidadDtpDesde = Diseno.ContabilidadFuncCampo(Filtros, Diseno.ContabilidadFuncFecha(), "ContabilidadDtpDesde", "Desde", 24, 20, 250);
            ContabilidadDtpHasta = Diseno.ContabilidadFuncCampo(Filtros, Diseno.ContabilidadFuncFecha(), "ContabilidadDtpHasta", "Hasta", 298, 20, 250);
            ContabilidadBtnConsultar = Diseno.ContabilidadFuncBoton(Filtros, "ContabilidadBtnConsultar", "Ver resultados", 574, 38, 220, EstiloBotonContabilidad.Acento);
            Diseno.ContabilidadFuncEtiqueta(Filtros, "ContabilidadLblFiltro", "Solo pólizas aplicadas", 820, 26, 290, 60);
            var Informe = Diseno.ContabilidadFuncTarjeta(this, "ContabilidadPnlInforme", 204, 342);
            ContabilidadLblPeriodo = Diseno.ContabilidadFuncEtiqueta(Informe, "ContabilidadLblPeriodo", "", 20, 14, 1092);
            ContabilidadDgvResultado = Diseno.ContabilidadFuncTabla(Informe, "ContabilidadDgvResultado", 52, 180,
                new[] { "Rubro", "Codigo", "Cuenta", "Importe" }, new[] { "Rubro", "Código de cuenta", "Cuenta", "Importe (Q)" });
            ContabilidadLblEstado = Diseno.ContabilidadFuncEtiqueta(Informe, "ContabilidadLblEstado", "", 20, 248, 1092, 64);
            ContabilidadLblResumen = Diseno.ContabilidadFuncEtiqueta(this, "ContabilidadLblResumen", "", 26, 558, 1130);
            Name = "FrmEstadosFinancieros";
            ResumeLayout(false);
        }

        private DateTimePicker ContabilidadDtpDesde, ContabilidadDtpHasta;
        private ClsTablaContabilidad ContabilidadDgvResultado;
        private Label ContabilidadLblPeriodo, ContabilidadLblEstado, ContabilidadLblResumen;
        private ClsBotonContabilidad ContabilidadBtnConsultar;
    }
}
