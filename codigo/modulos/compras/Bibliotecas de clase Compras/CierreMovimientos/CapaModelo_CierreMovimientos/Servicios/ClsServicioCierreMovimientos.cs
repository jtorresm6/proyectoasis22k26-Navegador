using System;
using System.Collections.Generic;
using CapaModelo_CierreMovimientos.Entidades;
using CapaModelo_CierreMovimientos.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Coordina el cierre de movimientos: arma la vista previa (saldos, antigüedad y
 *  validaciones) y, si no hay validaciones bloqueantes, la registra con la
 *  transacción de ClsComandosCierreMovimientos. Antes de guardar vuelve a
 *  calcular todo para no cerrar con datos desactualizados.
 * ===================================================================
*/

namespace CapaModelo_CierreMovimientos.Servicios
{
    public class ClsPreparacionCierre
    {
        public ClsPeriodoCompras Periodo { get; set; }
        public long IdMonedaLocal { get; set; }
        public List<ClsSaldoProveedorCierre> Saldos { get; set; }
        public List<ClsAntiguedadCierre> Antiguedad { get; set; }
        public List<ClsValidacionCierre> Validaciones { get; set; }
        public bool PuedeCerrar => ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(Validaciones);
    }

    public class ClsServicioCierreMovimientos
    {
        public const string CodigoMonedaLocal = "GTQ";

        private readonly ClsConsultasCierreMovimientos _Consultas;
        private readonly ClsComandosCierreMovimientos _Comandos;

        public ClsServicioCierreMovimientos() : this(new ClsConsultasCierreMovimientos(), new ClsComandosCierreMovimientos()) { }

        public ClsServicioCierreMovimientos(ClsConsultasCierreMovimientos Consultas, ClsComandosCierreMovimientos Comandos)
        {
            _Consultas = Consultas ?? throw new ArgumentNullException(nameof(Consultas));
            _Comandos = Comandos ?? throw new ArgumentNullException(nameof(Comandos));
        }

        public List<ClsPeriodoCompras> ComprasFuncObtenerPeriodosAbiertos()
        {
            return _Consultas.ComprasFuncObtenerPeriodosAbiertos();
        }

        public ClsPreparacionCierre ComprasFuncPrepararCierre(int IdPeriodo, bool HaySesion, DateTime Hoy)
        {
            ClsPeriodoCompras Periodo = _Consultas.ComprasFuncObtenerPeriodo(IdPeriodo)
                ?? throw new InvalidOperationException("El período seleccionado no existe.");
            long IdMonedaLocal = _Consultas.ComprasFuncObtenerIdMoneda(CodigoMonedaLocal);

            List<ClsSaldoProveedorCierre> Saldos = _Consultas.ComprasFuncObtenerSaldosProveedor(Periodo);
            List<ClsRangoAntiguedad> Rangos = _Consultas.ComprasFuncObtenerRangosAntiguedad();
            ClsIndicadoresCierre Indicadores = _Consultas.ComprasFuncObtenerIndicadores(Periodo, IdMonedaLocal);

            return new ClsPreparacionCierre
            {
                Periodo = Periodo,
                IdMonedaLocal = IdMonedaLocal,
                Saldos = Saldos,
                Antiguedad = Rangos.Count == 0
                    ? new List<ClsAntiguedadCierre>()
                    : ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(
                        _Consultas.ComprasFuncObtenerDocumentosPendientes(Periodo.FechaFin), Rangos, Periodo.FechaFin),
                Validaciones = ClsReglasCierreMovimientos.ComprasFuncEvaluarValidaciones(
                    Periodo, Indicadores, Saldos.Count, HaySesion, Hoy)
            };
        }

        public ClsResultadoCierre ComprasFuncEjecutarCierre(int IdPeriodo, int? IdUsuario, string Ip, DateTime Hoy)
        {
            ClsPreparacionCierre Preparacion = ComprasFuncPrepararCierre(IdPeriodo, IdUsuario.HasValue, Hoy);
            if (!Preparacion.PuedeCerrar)
                throw new InvalidOperationException("El período tiene validaciones bloqueantes. Vuelva a validar y corríjalas antes de cerrar.");

            return _Comandos.ComprasFuncRegistrarCierre(new ClsDatosCierre
            {
                Periodo = Preparacion.Periodo,
                IdMonedaLocal = Preparacion.IdMonedaLocal,
                Saldos = Preparacion.Saldos,
                Antiguedad = Preparacion.Antiguedad,
                IdUsuario = IdUsuario,
                Ip = Ip
            });
        }
    }
}
