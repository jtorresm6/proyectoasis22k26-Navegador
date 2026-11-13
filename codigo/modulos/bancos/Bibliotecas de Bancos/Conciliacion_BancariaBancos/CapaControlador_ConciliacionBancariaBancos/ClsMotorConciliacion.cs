/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Empareja los movimientos del libro con las líneas del estado de
 *  cuenta, respeta las parejas manuales y calcula totales, estado
 *  y método de la conciliación.
 * ================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_ConciliacionBancaria.Entidades;

namespace CapaControlador_ConciliacionBancaria
{
    public static class ClsMotorConciliacion
    {
        public const int ToleranciaDias = 3;
        public const string MotivoMovimientoSinBanco = "Movimiento registrado que no aparece en el estado de cuenta.";
        public const string MotivoLineaSinMovimiento = "Línea del estado de cuenta sin movimiento registrado.";
        public const string MotivoImporteDistinto = "El importe del movimiento y el del estado de cuenta no coinciden.";

        public static List<ClsDetalleConciliacion> BancosFuncEmparejar(
            IEnumerable<ClsMovimientoLibro> Libro,
            IEnumerable<ClsLineaExtracto> Banco,
            IEnumerable<ClsDetalleConciliacion> Manuales)
        {
            List<ClsDetalleConciliacion> ParejasManuales = (Manuales ?? new List<ClsDetalleConciliacion>())
                .Where(Detalle => Detalle.EsParejaManual).ToList();
            HashSet<long> MovimientosManuales = new HashSet<long>(ParejasManuales.Select(Detalle => Detalle.Movimiento.IdMovimiento));
            HashSet<long> LineasManuales = new HashSet<long>(ParejasManuales.Select(Detalle => Detalle.Linea.IdLinea));

            List<ClsMovimientoLibro> LibroLibre = (Libro ?? new List<ClsMovimientoLibro>())
                .Where(Movimiento => !MovimientosManuales.Contains(Movimiento.IdMovimiento))
                .OrderBy(Movimiento => Movimiento.FechaValor).ThenBy(Movimiento => Movimiento.IdMovimiento).ToList();
            List<ClsLineaExtracto> BancoLibre = (Banco ?? new List<ClsLineaExtracto>())
                .Where(Linea => !LineasManuales.Contains(Linea.IdLinea))
                .OrderBy(Linea => Linea.FechaValor).ThenBy(Linea => Linea.IdLinea).ToList();

            List<ClsDetalleConciliacion> Resultado = new List<ClsDetalleConciliacion>(ParejasManuales);

            BancosMetEmparejarPor(LibroLibre, BancoLibre, Resultado,
                (Movimiento, Linea) => BancosFuncReferenciasIguales(Movimiento.Referencia, Linea.Referencia)
                    && Movimiento.Sentido == Linea.Sentido && Movimiento.Importe == Linea.Importe);

            BancosMetEmparejarPor(LibroLibre, BancoLibre, Resultado,
                (Movimiento, Linea) => BancosFuncReferenciasIguales(Movimiento.Referencia, Linea.Referencia)
                    && Movimiento.Sentido == Linea.Sentido);

            BancosMetEmparejarPor(LibroLibre, BancoLibre, Resultado,
                (Movimiento, Linea) => BancosFuncReferenciasCompatibles(Movimiento.Referencia, Linea.Referencia)
                    && Movimiento.Sentido == Linea.Sentido && Movimiento.Importe == Linea.Importe
                    && Math.Abs((Movimiento.FechaValor - Linea.FechaValor).TotalDays) <= ToleranciaDias);

            foreach (ClsMovimientoLibro Movimiento in LibroLibre)
                Resultado.Add(new ClsDetalleConciliacion
                {
                    Resultado = ClsValoresConciliacion.ResultadoPendiente,
                    Metodo = ClsValoresConciliacion.MetodoAutomatica,
                    Diferencia = 0m,
                    Motivo = MotivoMovimientoSinBanco,
                    Movimiento = Movimiento
                });

            foreach (ClsLineaExtracto Linea in BancoLibre)
                Resultado.Add(new ClsDetalleConciliacion
                {
                    Resultado = ClsValoresConciliacion.ResultadoPendiente,
                    Metodo = ClsValoresConciliacion.MetodoAutomatica,
                    Diferencia = 0m,
                    Motivo = MotivoLineaSinMovimiento,
                    Linea = Linea
                });

            return BancosFuncOrdenar(Resultado);
        }

        public static List<ClsDetalleConciliacion> BancosFuncOrdenar(IEnumerable<ClsDetalleConciliacion> Detalles)
        {
            return Detalles
                .OrderBy(Detalle => BancosFuncPrioridad(Detalle.Resultado))
                .ThenBy(Detalle => BancosFuncFecha(Detalle))
                .ThenBy(Detalle => Detalle.Movimiento != null ? Detalle.Movimiento.IdMovimiento : long.MaxValue)
                .ThenBy(Detalle => Detalle.Linea != null ? Detalle.Linea.IdLinea : long.MaxValue)
                .ToList();
        }

        public static DateTime BancosFuncFecha(ClsDetalleConciliacion Detalle)
        {
            if (Detalle.Movimiento != null) return Detalle.Movimiento.FechaValor;
            if (Detalle.Linea != null) return Detalle.Linea.FechaValor;
            return DateTime.MinValue;
        }

        public static ClsTotalesConciliacion BancosFuncCalcularTotales(IEnumerable<ClsDetalleConciliacion> Detalles)
        {
            ClsTotalesConciliacion Totales = new ClsTotalesConciliacion();
            foreach (ClsDetalleConciliacion Detalle in Detalles)
            {
                decimal Importe = Detalle.Linea != null ? Detalle.Linea.Importe : Detalle.Movimiento.Importe;
                int Sentido = Detalle.Linea != null ? Detalle.Linea.Sentido : Detalle.Movimiento.Sentido;

                if (Detalle.Resultado == ClsValoresConciliacion.ResultadoCoincide)
                {
                    Totales.Coincidencias++;
                    if (Sentido < 0) Totales.CargosConciliados += Importe;
                    else Totales.AbonosConciliados += Importe;
                    continue;
                }

                if (Detalle.Resultado == ClsValoresConciliacion.ResultadoDiferencia) Totales.Diferencias++;
                else Totales.Pendientes++;

                if (Sentido < 0) Totales.CargosPorConciliar += Importe;
                else Totales.AbonosPorConciliar += Importe;
            }
            return Totales;
        }

        public static string BancosFuncDeterminarEstado(ClsTotalesConciliacion Totales, decimal DiferenciaSaldos)
        {
            bool SinPartidasAbiertas = Totales.Pendientes == 0 && Totales.Diferencias == 0;
            return SinPartidasAbiertas && DiferenciaSaldos == 0m
                ? ClsValoresConciliacion.EstadoConciliada
                : ClsValoresConciliacion.EstadoConDiferencias;
        }

        public static string BancosFuncDeterminarMetodo(IEnumerable<ClsDetalleConciliacion> Detalles)
        {
            List<ClsDetalleConciliacion> Marcadas = Detalles
                .Where(Detalle => Detalle.Resultado != ClsValoresConciliacion.ResultadoPendiente).ToList();
            int Manuales = Marcadas.Count(Detalle => Detalle.Metodo == ClsValoresConciliacion.MetodoManual);
            int Automaticas = Marcadas.Count - Manuales;

            if (Manuales > 0 && Automaticas > 0) return ClsValoresConciliacion.MetodoMixta;
            if (Manuales > 0) return ClsValoresConciliacion.MetodoManual;
            return ClsValoresConciliacion.MetodoAutomatica;
        }

        public static string BancosFuncTextoEstado(string Estado)
        {
            switch (Estado)
            {
                case ClsValoresConciliacion.EstadoConciliada: return "Conciliada";
                case ClsValoresConciliacion.EstadoConDiferencias: return "Con diferencias";
                case ClsValoresConciliacion.EstadoCerrada: return "Cerrada";
                default: return "Abierta";
            }
        }

        private static void BancosMetEmparejarPor(
            List<ClsMovimientoLibro> LibroLibre,
            List<ClsLineaExtracto> BancoLibre,
            List<ClsDetalleConciliacion> Resultado,
            Func<ClsMovimientoLibro, ClsLineaExtracto, bool> Criterio)
        {
            foreach (ClsMovimientoLibro Movimiento in LibroLibre.ToList())
            {
                ClsLineaExtracto Mejor = BancoLibre
                    .Where(Linea => Criterio(Movimiento, Linea))
                    .OrderBy(Linea => Math.Abs((Movimiento.FechaValor - Linea.FechaValor).TotalDays))
                    .ThenBy(Linea => Linea.IdLinea)
                    .FirstOrDefault();
                if (Mejor == null) continue;

                bool ImportesIguales = Movimiento.Importe == Mejor.Importe;
                Resultado.Add(new ClsDetalleConciliacion
                {
                    Resultado = ImportesIguales ? ClsValoresConciliacion.ResultadoCoincide : ClsValoresConciliacion.ResultadoDiferencia,
                    Metodo = ClsValoresConciliacion.MetodoAutomatica,
                    Diferencia = Mejor.Importe - Movimiento.Importe,
                    Motivo = ImportesIguales ? null : MotivoImporteDistinto,
                    Movimiento = Movimiento,
                    Linea = Mejor
                });
                LibroLibre.Remove(Movimiento);
                BancoLibre.Remove(Mejor);
            }
        }

        private static bool BancosFuncReferenciasIguales(string Primera, string Segunda)
        {
            string A = BancosFuncNormalizar(Primera);
            string B = BancosFuncNormalizar(Segunda);
            return A.Length > 0 && A == B;
        }

        private static bool BancosFuncReferenciasCompatibles(string Primera, string Segunda)
        {
            string A = BancosFuncNormalizar(Primera);
            string B = BancosFuncNormalizar(Segunda);
            return A.Length == 0 || B.Length == 0 || A == B;
        }

        private static string BancosFuncNormalizar(string Texto)
        {
            return string.IsNullOrWhiteSpace(Texto) ? string.Empty : Texto.Trim().ToUpperInvariant();
        }

        private static int BancosFuncPrioridad(string Resultado)
        {
            switch (Resultado)
            {
                case ClsValoresConciliacion.ResultadoPendiente: return 0;
                case ClsValoresConciliacion.ResultadoDiferencia: return 1;
                default: return 2;
            }
        }
    }
}
