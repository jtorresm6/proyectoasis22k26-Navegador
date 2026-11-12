using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CapaModelo_TipoCambio.Entidades;
using CapaModelo_TipoCambio.Repositorio;

namespace CapaControlador_TipoCambio
{
    public class ClsControladorMonedaBanguat
    {
        private readonly IRepositorioMonedaBanguat _repositorio;
        public static event EventHandler BancosCatalogoMonedaActualizado;

        public ClsControladorMonedaBanguat() : this(new ClsRepositorioMonedaBanguat()) { }
        public ClsControladorMonedaBanguat(IRepositorioMonedaBanguat repositorio)
        { _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio)); }

        public Task<List<ClsMonedaCatalogoLocal>> BancosMetConsultarAsync(CancellationToken token)
        { return Task.Run(() => _repositorio.BancosMetConsultar(token), token); }

        public async Task<ClsResultadoSincronizacionMonedas> BancosMetSincronizarAsync(CancellationToken token)
        {
            var resultado = await _repositorio.BancosMetSincronizarAsync(token).ConfigureAwait(false);
            NotificarCatalogo();
            return resultado;
        }

        public async Task BancosMetEstablecerActivaAsync(long id, bool activa, CancellationToken token)
        {
            await Task.Run(() => _repositorio.BancosMetEstablecerActiva(id, activa, token), token).ConfigureAwait(false);
            NotificarCatalogo();
        }

        private void NotificarCatalogo()
        {
            var evento = BancosCatalogoMonedaActualizado;
            if (evento == null) return;
            foreach (EventHandler receptor in evento.GetInvocationList())
            {
                // El guardado ya se confirmó: un formulario cerrado o un fallo
                // de refresco no deben presentarlo como una migración fallida.
                try { receptor(this, EventArgs.Empty); }
                catch (Exception) { }
            }
        }
    }
}
