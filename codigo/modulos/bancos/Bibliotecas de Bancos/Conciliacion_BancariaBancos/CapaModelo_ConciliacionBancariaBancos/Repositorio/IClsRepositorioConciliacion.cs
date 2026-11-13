/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Declara las operaciones de persistencia requeridas por la
 *  conciliación bancaria.
 * ================================================================
 */
using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_ConciliacionBancaria.Entidades;

namespace CapaModelo_ConciliacionBancaria.Repositorio
{
    public interface IClsRepositorioConciliacion
    {
        DataTable BancosMetConsultarCuentas();
        DataTable BancosMetConsultarExtractos(long IdCuenta);
        ClsDatosExtracto BancosMetObtenerExtracto(long IdExtracto);
        decimal BancosMetObtenerSaldoLibro(long IdCuenta, DateTime FechaCorte);
        decimal BancosMetObtenerNetoExtracto(long IdExtracto, DateTime FechaCorte);
        List<ClsMovimientoLibro> BancosMetConsultarMovimientosLibro(long IdCuenta, DateTime FechaCorte, long IdConciliacion);
        List<ClsLineaExtracto> BancosMetConsultarLineasExtracto(long IdExtracto, DateTime FechaCorte, long IdConciliacion);
        ClsConciliacion BancosMetBuscarConciliacion(long IdCuenta, DateTime FechaCorte);
        ClsConciliacion BancosMetObtenerConciliacion(long IdConciliacion);
        List<ClsDetalleConciliacion> BancosMetConsultarDetalles(long IdConciliacion);
        string BancosMetGuardarConciliacion(ClsConciliacion Encabezado, List<ClsDetalleConciliacion> Detalles, out long IdConciliacion);
    }
}
