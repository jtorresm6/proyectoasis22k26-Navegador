using System;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 29/10/2026
 * ==================================================================
 * Propósito :
 *  Guarda, durante la sesión, la moneda en la que el usuario quiere ver los montos
 *  del módulo de Compras y convierte montos expresados en moneda local hacia ella.
 *  Los demás submódulos pueden usar ComprasFuncFormatearDesdeLocal para mostrar sus
 *  totales y suscribirse a MonedaCambiada para refrescar sus pantallas.
 * Reglas específicas:
 *  - Si no se ha elegido moneda, o no existe tipo de cambio, se muestra el monto
 *    en moneda local para no presentar valores incorrectos.
 * ===================================================================
*/

namespace CapaControlador_Herramientas
{
    public static class ClsPreferenciaMonedaCompras
    {
        private static ClsMonedaCompras _MonedaVisualizacion;

        public static event EventHandler MonedaCambiada;

        public static ClsMonedaCompras MonedaVisualizacion => _MonedaVisualizacion;

        public static void ComprasProcEstablecerMoneda(ClsMonedaCompras Moneda)
        {
            if (Moneda == null) throw new ArgumentNullException(nameof(Moneda));
            if (_MonedaVisualizacion != null && _MonedaVisualizacion.IdMoneda == Moneda.IdMoneda) return;
            _MonedaVisualizacion = Moneda;
            MonedaCambiada?.Invoke(null, EventArgs.Empty);
        }

        public static string ComprasFuncFormatearDesdeLocal(decimal MontoLocal, DateTime Fecha,
            ClsControladorMoneda Controlador, ClsMonedaCompras MonedaLocal)
        {
            if (Controlador == null) throw new ArgumentNullException(nameof(Controlador));

            if (_MonedaVisualizacion == null || MonedaLocal == null || _MonedaVisualizacion.IdMoneda == MonedaLocal.IdMoneda)
                return ClsControladorMoneda.ComprasFuncFormatearMonto(MontoLocal, MonedaLocal);

            ClsResultadoConversion Conversion = Controlador.ComprasFuncConvertir(MontoLocal, MonedaLocal, _MonedaVisualizacion, Fecha);
            return Conversion.Exitoso
                ? ClsControladorMoneda.ComprasFuncFormatearMonto(Conversion.MontoConvertido, _MonedaVisualizacion)
                : ClsControladorMoneda.ComprasFuncFormatearMonto(MontoLocal, MonedaLocal);
        }
    }
}
