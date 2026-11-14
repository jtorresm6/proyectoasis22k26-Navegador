// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con otros módulos
// Descripción: Interfaz que define las operaciones de acceso a datos del enlace, para
//              desacoplar el controlador de la implementación ODBC.
// Fecha:       12/11/2026
// =====================================================================
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
