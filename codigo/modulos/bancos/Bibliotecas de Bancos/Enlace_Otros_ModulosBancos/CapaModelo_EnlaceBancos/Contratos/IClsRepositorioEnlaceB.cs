using System;
using System.Data;
using CapaModelo_EnlaceB.Entidades;

namespace CapaModelo_EnlaceB.Contratos
{
    public interface IClsRepositorioEnlaceB
    {
        DataTable BancarioMetConsultarModulo(ModuloEnlace Modulo, DateTime Inicio, DateTime Fin);
        DataTable BancarioMetConsultarCatalogo(string Catalogo);
        DataTable BancarioMetConsultarPolizas(DateTime Inicio, DateTime Fin);
        DataTable BancarioMetConsultarHistorico();
        ClsPartidaEnlace BancarioMetGenerarPartida(ClsContextoPartida Contexto);
        ClsPartidaEnlace BancarioMetObtenerPartida(long Id);
        ClsResultadoEnvioContable BancarioMetEnviarPartida(long Id, ClsContextoPartida Contexto, Func<ClsEnvioContable, ClsResultadoEnvioContable> Recibir);
    }
}
