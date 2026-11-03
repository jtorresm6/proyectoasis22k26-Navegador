namespace CapaVista_MovimientosClientes
{
    // Ventana independiente; reutiliza la captura y las consultas del proceso.
    public sealed class FrmEstadoCuentaVentas : FrmMovimientosClientesVentas
    {
        public FrmEstadoCuentaVentas()
        {
            VentasMetConfigurarProceso(0, "Estado de cuenta", "7003");
        }
    }
}
