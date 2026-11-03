namespace CapaVista_MovimientosClientes
{
    // Ventana independiente; reutiliza la captura y las consultas del proceso.
    public sealed class FrmConsultaRecibosVentas : FrmMovimientosClientesVentas
    {
        public FrmConsultaRecibosVentas()
        {
            VentasMetConfigurarProceso(4, "Consulta de recibos", "7016");
        }
    }
}
