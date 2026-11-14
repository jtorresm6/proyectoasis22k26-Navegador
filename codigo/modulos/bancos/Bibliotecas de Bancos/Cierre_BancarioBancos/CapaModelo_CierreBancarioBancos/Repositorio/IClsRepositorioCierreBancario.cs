/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Autor  : Andre De Jesus Gonzalez Camey
 * Carné  : 9959-23-1224 --- 9959-23-3117
 * Fecha  : 08/11/2026
 * ================================================================
 * Propósito :
 *  Declara las operaciones de persistencia requeridas por el
 *  proceso de cierre bancario.
 * ================================================================
 */
using System;
using System.Data;

namespace CapaModelo_CierreBancario.Repositorio
{
    public interface IClsRepositorioCierreBancario
    {
        DataTable BancosMetConsultarResumenCuentas(DateTime FechaInicial, DateTime FechaFinal);
        bool BancosMetValidarPeriodoSinCierre(DateTime FechaInicial, DateTime FechaFinal);
        bool BancosMetValidarConciliacionesCompletas(DateTime FechaInicial, DateTime FechaFinal);
        bool BancosMetValidarSaldosVerificados(DateTime FechaInicial, DateTime FechaFinal);
        string BancosMetCerrarPeriodo(DateTime FechaInicial, DateTime FechaFinal);
        DataTable BancosMetConsultarHistorico();
        DataTable BancosMetConsultarSaldosCierrePeriodo(DateTime FechaInicial, DateTime FechaFinal);
        DataTable BancosMetConsultarSaldosCierresHistoricos();
    }
}
