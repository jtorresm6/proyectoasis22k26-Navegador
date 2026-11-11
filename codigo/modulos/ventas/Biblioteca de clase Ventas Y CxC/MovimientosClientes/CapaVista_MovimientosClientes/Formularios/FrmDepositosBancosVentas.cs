namespace CapaVista_MovimientosClientes
{
    // Ventana independiente; reutiliza la captura y las consultas del proceso.
    public sealed class FrmDepositosBancosVentas : FrmMovimientosClientesVentas
    {
        public FrmDepositosBancosVentas()
        {
            VentasMetConfigurarProceso(3, "Depósitos a Bancos", "7015");
        }
    }
}
