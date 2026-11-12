using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CapaModelo_TipoCambio.Entidades;
using CapaModelo_TipoCambio.Repositorio;

namespace CapaControlador_TipoCambio
{
    public class ClsControladorTipoCambio
    {
        private readonly ClsRepositorioTipoCambio _repositorio;

        public ClsControladorTipoCambio() : this(new ClsRepositorioTipoCambio()) { }

        public ClsControladorTipoCambio(ClsRepositorioTipoCambio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<List<ClsMoneda>> BancosMetObtenerMonedasAsync(
            CancellationToken cancellationToken)
        {
            return _repositorio.BancosMetObtenerMonedasAsync(cancellationToken);
        }

        public Task BancosMetInvalidarCacheAsync(CancellationToken cancellationToken)
        {
            return _repositorio.BancosMetInvalidarCacheAsync(cancellationToken);
        }

        public async Task<DataTable> BancosMetObtenerTasasActualesAsync(
            DateTime fecha, bool incluirSinTasa, bool forzar,
            CancellationToken cancellationToken)
        {
            var tasas = await _repositorio.BancosMetObtenerTasasActualesAsync(
                fecha, incluirSinTasa, forzar, cancellationToken).ConfigureAwait(false);
            var tabla = new DataTable("Tasas de cambio");
            tabla.Columns.Add("Código", typeof(string));
            tabla.Columns.Add("Moneda", typeof(string));
            tabla.Columns.Add("Tasa en GTQ", typeof(decimal));
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Fuente", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));
            tabla.Columns.Add("Metodo", typeof(string));
            foreach (var tasa in tasas)
            {
                bool disponible = tasa.Tasa > 0 && tasa.Estado != "Sin tasa";
                tabla.Rows.Add(tasa.CodigoMoneda, tasa.NombreMoneda,
                    disponible ? (object)tasa.Tasa : DBNull.Value,
                    tasa.FechaTasa.Date, tasa.Fuente,
                    disponible ? "Disponible" : "Sin tasa", tasa.Metodo);
            }
            return tabla;
        }

        public async Task<DataTable> BancosMetObtenerHistoricoAsync(
            DateTime desde, DateTime hasta, string origen, string destino,
            CancellationToken cancellationToken)
        {
            var tasas = await _repositorio.BancosMetObtenerHistoricoAsync(
                desde, hasta, origen, destino, cancellationToken).ConfigureAwait(false);
            var tabla = new DataTable("Histórico de tipos de cambio");
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Moneda origen", typeof(string));
            tabla.Columns.Add("Moneda destino", typeof(string));
            tabla.Columns.Add("Tasa", typeof(decimal));
            tabla.Columns.Add("Fuente", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));
            tabla.Columns.Add("Metodo", typeof(string));
            foreach (var tasa in tasas)
                tabla.Rows.Add(tasa.FechaTasa.Date, tasa.MonedaOrigen,
                    tasa.MonedaDestino, tasa.Tasa, tasa.Fuente, tasa.Estado, tasa.Metodo);
            return tabla;
        }

        public Task<ClsResultadoConversion> BancosMetConvertirAsync(
            decimal importe, DateTime fecha, string origen, string destino,
            CancellationToken cancellationToken)
        {
            return _repositorio.BancosMetConvertirAsync(
                importe, fecha, origen, destino, cancellationToken);
        }
    }
}
