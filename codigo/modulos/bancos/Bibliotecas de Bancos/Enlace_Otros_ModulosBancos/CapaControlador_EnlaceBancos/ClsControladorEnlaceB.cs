using System;
using System.Data;
using CapaModelo_EnlaceB.Contratos;
using CapaModelo_EnlaceB.Entidades;
using CapaModelo_EnlaceB.Repositorio;

namespace CapaControlador_EnlaceB
{
    public sealed class ClsControladorEnlaceB
    {
        private readonly IClsRepositorioEnlaceB _Repositorio;
        public ClsControladorEnlaceB() : this(new ClsRepositorioEnlaceB()) { }
        public ClsControladorEnlaceB(IClsRepositorioEnlaceB Repositorio)
        {
            this._Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }
        public DataTable BancarioMetConsultarModulo(ModuloEnlace Modulo, DateTime Inicio, DateTime Fin)
        {
            if (!Enum.IsDefined(typeof(ModuloEnlace), Modulo)) throw new ArgumentException("Módulo no válido.");
            ClsReglasEnlace.BancarioMetValidarFechas(Inicio, Fin, false);
            return _Repositorio.BancarioMetConsultarModulo(Modulo, Inicio.Date, Fin.Date);
        }
        public DataTable BancarioMetConsultarPolizas(DateTime Inicio, DateTime Fin)
        {
            ClsReglasEnlace.BancarioMetValidarFechas(Inicio, Fin, false);
            return _Repositorio.BancarioMetConsultarPolizas(Inicio.Date, Fin.Date);
        }
        public DataTable BancarioMetCatalogo(string Nombre) { return _Repositorio.BancarioMetConsultarCatalogo(Nombre); }
        public DataTable BancarioMetHistorico() { return _Repositorio.BancarioMetConsultarHistorico(); }
        public ClsPartidaEnlace BancarioMetGenerar(ClsContextoPartida Contexto)
        {
            ClsReglasEnlace.BancarioMetValidarContexto(Contexto);
            return _Repositorio.BancarioMetGenerarPartida(Contexto);
        }
        public ClsPartidaEnlace BancarioMetObtener(long Id)
        {
            if (Id <= 0) throw new ArgumentException("Seleccione una partida del historial.");
            return _Repositorio.BancarioMetObtenerPartida(Id);
        }
        public string BancarioMetEnviar(long Id, ClsContextoPartida Contexto)
        {
            if (Id <= 0) throw new ArgumentException("Genere o seleccione una partida antes de enviar.");
            ClsReglasEnlace.BancarioMetValidarContexto(Contexto);
            return _Repositorio.BancarioMetEnviarPartida(Id, Contexto);
        }
    }
}
