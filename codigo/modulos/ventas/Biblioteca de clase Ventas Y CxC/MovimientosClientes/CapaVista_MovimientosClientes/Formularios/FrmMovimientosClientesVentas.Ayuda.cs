using System;
using System.Windows.Forms;
using VentasPresentacion;

namespace CapaVista_MovimientosClientes
{
    public partial class FrmMovimientosClientesVentas
    {
        private string VentasFuncTemaAyuda()
        {
            string[] Temas = { "Estado_Cuenta", "Cargos_Ajustes", "Recepcion_Pagos", "Depositos_Bancos", "Consulta_Recibos" };
            int Proceso = VentasProceso >= 0 ? VentasProceso : VentasTbcMovimientos.SelectedIndex;
            return "html/" + (Proceso >= 0 && Proceso < Temas.Length ? Temas[Proceso] : "Movimientos_General") + ".html";
        }

        private void VentasMetAyudaClick(object Origen, EventArgs Evento)
        {
            ClsAyudaVentas.VentasMetMostrar(this, typeof(FrmMovimientosClientesVentas).Assembly,
                "CapaVista_MovimientosClientes.Ayudas.MovimientosClientes.chm", VentasFuncTemaAyuda());
        }

        protected override bool ProcessCmdKey(ref Message Mensaje, Keys Tecla)
        {
            if (Tecla == Keys.F1) { VentasMetAyudaClick(this, EventArgs.Empty); return true; }
            return base.ProcessCmdKey(ref Mensaje, Tecla);
        }
    }
}
