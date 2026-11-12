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
 *  El cierre de movimientos usa ComprasFuncFormatearDesdeLocal para mostrar el
 *  equivalente de sus totales; cualquier otro submódulo puede usarla igual sin
 *  cambiar su código actual. Los formularios del MDI son modales, por eso cada
 *  pantalla lee la preferencia al mostrar sus montos.
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

        public static ClsMonedaCompras MonedaVisualizacion => _MonedaVisualizacion;

        public static void ComprasProcEstablecerMoneda(ClsMonedaCompras Moneda)
        {
            if (Moneda == null) throw new ArgumentNullException(nameof(Moneda));
            _MonedaVisualizacion = Moneda;
        }

        // Indica si el usuario eligió una moneda distinta a la local para ver los montos.
        public static bool ComprasFuncUsaOtraMoneda(ClsMonedaCompras MonedaLocal)
        {
            return _MonedaVisualizacion != null && MonedaLocal != null && _MonedaVisualizacion.IdMoneda != MonedaLocal.IdMoneda;
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
