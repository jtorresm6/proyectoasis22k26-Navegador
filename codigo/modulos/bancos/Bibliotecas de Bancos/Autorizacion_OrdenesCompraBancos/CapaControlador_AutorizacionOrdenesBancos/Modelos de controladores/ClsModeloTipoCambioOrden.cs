using CapaControlador_TipoCambio;
using CapaModelo_AutorizacionOrdenes.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

/*
 * ==========================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 13/11/2026
 * ==========================================================================
 * Propósito :
 *  La clase Modelo de Tipo de Cambio de la Orden pertenece a la capa controlador.
 *  Consulta la tasa del día entre la moneda de la cuenta y la de la orden con el
 *  método BancosMetConvertirAsync del módulo de Tipo de Cambio, y la guarda en
 *  tblTipoCambio mediante el repositorio para obtener su identificador.
 * ==========================================================================
*/

namespace CapaControlador_AutorizacionOrdenes.Modelos_de_controladores
{
    public class ClsModeloTipoCambioOrden
    {
        private readonly ClsControladorTipoCambio _ControladorTipoCambio = new ClsControladorTipoCambio();
        private readonly ClsRepositorioTipoCambioAutorizacion _RepositorioTipoCambio = new ClsRepositorioTipoCambioAutorizacion();

        // Devuelve cuántas unidades de la moneda de la orden equivalen a una unidad
        // de la moneda de la cuenta, en la fecha de hoy.
        public async Task<ClsModeloTasaCambio> BancosMetObtenerTasaAsync(string CodigoMonedaCuenta, string CodigoMonedaOrden)
        {
            if (string.IsNullOrWhiteSpace(CodigoMonedaCuenta) || string.IsNullOrWhiteSpace(CodigoMonedaOrden))
            {
                throw new InvalidOperationException("No se conoce el código de moneda de la cuenta o de la orden.");
            }

            var Resultado = await _ControladorTipoCambio.BancosMetConvertirAsync(
                1m, DateTime.Today, CodigoMonedaCuenta, CodigoMonedaOrden, CancellationToken.None);

            return new ClsModeloTasaCambio
            {
                Tasa = Resultado.Tasa,
                Fecha = Resultado.Fecha,
                Fuente = Resultado.Fuente,
                Metodo = ClsModeloTasaCambio.BancosMetNormalizarMetodo(Resultado.Metodo)
            };
        }

        public long BancosMetGuardarTasa(ClsModeloTasaCambio Tasa, long IdMonedaCuenta, long IdMonedaOrden)
        {
            Tasa.IdTipoCambio = _RepositorioTipoCambio.BancosMetGuardarTipoCambio(
                Tasa.Fecha, Tasa.Tasa, Tasa.Fuente, Tasa.Metodo, IdMonedaCuenta, IdMonedaOrden);
            return Tasa.IdTipoCambio.Value;
        }
    }
}
