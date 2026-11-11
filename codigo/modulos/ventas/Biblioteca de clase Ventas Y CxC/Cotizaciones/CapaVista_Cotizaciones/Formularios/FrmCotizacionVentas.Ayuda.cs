using System;
using System.Windows.Forms;
using VentasPresentacion;

namespace CapaVista_Cotizaciones
{
    public partial class FrmCotizacionVentas
    {
        private void VentasMetAyudaClick(object Origen, EventArgs Evento)
        {
            ClsAyudaVentas.VentasMetMostrar(this, typeof(FrmCotizacionVentas).Assembly,
                "CapaVista_Cotizaciones.Ayudas.Cotizaciones.chm", "html/Cotizaciones_General.html");
        }

        protected override bool ProcessCmdKey(ref Message Mensaje, Keys Tecla)
        {
            if (Tecla == Keys.F1) { VentasMetAyudaClick(this, EventArgs.Empty); return true; }
            return base.ProcessCmdKey(ref Mensaje, Tecla);
        }
    }
}
