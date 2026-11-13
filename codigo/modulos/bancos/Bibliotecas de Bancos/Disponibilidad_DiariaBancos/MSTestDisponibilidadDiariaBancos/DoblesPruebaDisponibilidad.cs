using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CapaModelo_DisponibilidadDiariaBancos.Entidades;
using CapaModelo_DisponibilidadDiariaBancos.Repositorio;
using CapaModelo_DisponibilidadDiariaBancos.Servicios;

namespace MSTestDisponibilidadDiariaBancos
{
    // Dobles de prueba: todos los datos viven en memoria. Ningún método abre
    // una conexión ODBC ni llama al servicio del Banco de Guatemala.
    internal sealed class RepositorioDisponibilidadQuemado : IRepositorioDisponibilidad
    {
        public ClsCatalogosDisponibilidad Catalogos { get; set; } =
            new ClsCatalogosDisponibilidad();

        public ClsConsultaDisponibilidad Consulta { get; set; } =
            new ClsConsultaDisponibilidad
            {
                TieneTabla = true,
                Mensaje = "Datos de prueba en memoria."
            };

        public ClsCalculoAutomaticoDisponibilidad Calculo { get; set; } =
            new ClsCalculoAutomaticoDisponibilidad();

        public List<ClsDisponibilidad> RegistrosGuardados { get; } =
            new List<ClsDisponibilidad>();

        public List<bool> Reemplazos { get; } = new List<bool>();

        public ClsCatalogosDisponibilidad BancosMetObtenerCatalogos(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Catalogos;
        }

        public ClsConsultaDisponibilidad BancosMetConsultar(
            DateTime fecha, long? bancoId, long? cuentaId, string moneda,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Consulta.Fecha = fecha.Date;
            Consulta.Detalle = Consulta.Detalle ?? new List<ClsDisponibilidad>();
            Consulta.Compromisos = Consulta.Compromisos ?? new List<ClsCompromiso>();
            return Consulta;
        }

        public ClsCalculoAutomaticoDisponibilidad BancosMetCalcularAutomaticamente(
            DateTime fecha, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Calculo;
        }

        public void BancosMetGuardar(
            ClsDisponibilidad registro, bool reemplazar, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            RegistrosGuardados.Add(registro);
            Reemplazos.Add(reemplazar);
        }
    }

    internal sealed class ProveedorTipoCambioQuemado : IProveedorTipoCambioDisponibilidad
    {
        private readonly Dictionary<string, decimal> _tasas;

        public ProveedorTipoCambioQuemado(params KeyValuePair<string, decimal>[] tasas)
        {
            _tasas = tasas.ToDictionary(
                par => par.Key.Trim().ToUpperInvariant(),
                par => par.Value,
                StringComparer.OrdinalIgnoreCase);
            _tasas["GTQ"] = 1m;
        }

        public List<string> MonedasSolicitadas { get; } = new List<string>();

        public Task<ClsCotizacionDisponibilidad> BancosMetObtenerTasaAsync(
            string codigoMoneda, DateTime fecha, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            string codigo = codigoMoneda.Trim().ToUpperInvariant();
            MonedasSolicitadas.Add(codigo);

            decimal tasa;
            if (!_tasas.TryGetValue(codigo, out tasa))
                throw new InvalidOperationException("No existe una tasa quemada para " + codigo + ".");

            return Task.FromResult(new ClsCotizacionDisponibilidad
            {
                Moneda = codigo,
                Fecha = fecha.Date,
                TasaOriginal = tasa,
                TasaAplicada = tasa,
                Fuente = "Tasa quemada para prueba unitaria",
                Metodo = codigo == "GTQ" ? "IDENTIDAD" : "PRUEBA"
            });
        }
    }
}
