namespace CapaVista_MovimientosClientes_Ventas
{
    // Ventana independiente; reutiliza la captura y las consultas del proceso.
    public sealed class FrmCargosAjustesVentas : FrmMovimientosClientesVentas
    {
        public FrmCargosAjustesVentas()
        {
            VentasMetConfigurarProceso(1, "Cargos y ajustes", "7014");
        }
    }
}
