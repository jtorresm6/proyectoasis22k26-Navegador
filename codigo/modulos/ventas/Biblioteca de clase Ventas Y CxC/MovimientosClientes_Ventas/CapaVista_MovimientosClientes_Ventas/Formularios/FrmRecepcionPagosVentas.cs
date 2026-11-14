namespace CapaVista_MovimientosClientes_Ventas
{
    // Ventana independiente; reutiliza la captura y las consultas del proceso.
    public sealed class FrmRecepcionPagosVentas : FrmMovimientosClientesVentas
    {
        public FrmRecepcionPagosVentas()
        {
            VentasMetConfigurarProceso(2, "Recepción de pagos", "7005");
        }
    }
}
