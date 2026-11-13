/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Valida los datos de la conciliación bancaria y coordina el corte,
 *  el marcado manual y la consulta del reporte con el repositorio.
 * ================================================================
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using CapaModelo_ConciliacionBancaria.Entidades;
using CapaModelo_ConciliacionBancaria.Repositorio;

namespace CapaControlador_ConciliacionBancaria
{
    public class ClsControladorConciliacion
    {
        private const string MensajeSinConciliacion = "Realice primero el corte de conciliación.";
        private const string MensajeCerrada = "La conciliación de este corte ya está cerrada y no se puede modificar.";
        private const string MensajeSeleccionPartidas = "Seleccione un movimiento del libro y una línea del estado de cuenta.";

        private readonly IClsRepositorioConciliacion _Repositorio;

        public ClsControladorConciliacion()
            : this(new ClsRepositorioConciliacion())
        {
        }

        public ClsControladorConciliacion(IClsRepositorioConciliacion Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public string BancosMetConsultarCuentas(out DataTable Resultado)
        {
            Resultado = null;
            try
            {
                Resultado = _Repositorio.BancosMetConsultarCuentas();
                return string.Empty;
            }
            catch (OdbcException)
            {
                return "Error de base de datos al consultar las cuentas.";
            }
            catch (Exception)
            {
                return "Error inesperado al consultar las cuentas.";
            }
        }

        public string BancosMetConsultarExtractos(long IdCuenta, out DataTable Resultado)
        {
            Resultado = null;
            if (IdCuenta <= 0)
                return "Seleccione una cuenta bancaria.";

            try
            {
                Resultado = _Repositorio.BancosMetConsultarExtractos(IdCuenta);
                return string.Empty;
            }
            catch (OdbcException)
            {
                return "Error de base de datos al consultar los extractos.";
            }
            catch (Exception)
            {
                return "Error inesperado al consultar los extractos.";
            }
        }

        public string BancosMetObtenerPeriodoExtracto(long IdExtracto, out DateTime FechaInicio, out DateTime FechaFin, out decimal SaldoInicial)
        {
            FechaInicio = DateTime.Today;
            FechaFin = DateTime.Today;
            SaldoInicial = 0m;
            if (IdExtracto <= 0)
                return "Seleccione un extracto bancario.";

            try
            {
                ClsDatosExtracto Extracto = _Repositorio.BancosMetObtenerExtracto(IdExtracto);
                if (Extracto == null)
                    return "No se encontró el extracto seleccionado.";

                FechaInicio = Extracto.FechaInicio;
                FechaFin = Extracto.FechaFin;
                SaldoInicial = Extracto.SaldoInicial;
                return string.Empty;
            }
            catch (OdbcException)
            {
                return "Error de base de datos al consultar el extracto.";
            }
            catch (Exception)
            {
                return "Error inesperado al consultar el extracto.";
            }
        }

        public string BancosMetEjecutarCorte(long IdCuenta, long IdExtracto, string FechaCorte, string Responsable,
            out ClsResultadoConciliacion Resultado)
        {
            Resultado = null;
            if (IdCuenta <= 0)
                return "Seleccione una cuenta bancaria.";
            if (IdExtracto <= 0)
                return "Seleccione un extracto bancario.";
            if (!DateTime.TryParseExact(FechaCorte, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime Corte))
                return "Fecha de corte no es válida. Use el formato dd/MM/aaaa.";

            try
            {
                ClsConciliacion Existente = _Repositorio.BancosMetBuscarConciliacion(IdCuenta, Corte);
                List<ClsDetalleConciliacion> Manuales = new List<ClsDetalleConciliacion>();
                if (Existente != null)
                {
                    if (Existente.IdExtracto != IdExtracto)
                        return "Ya existe una conciliación de esta cuenta con esa fecha de corte para otro extracto.";
                    if (Existente.Estado == ClsValoresConciliacion.EstadoCerrada)
                        return MensajeCerrada;
                    Manuales = _Repositorio.BancosMetConsultarDetalles(Existente.IdConciliacion)
                        .Where(Detalle => Detalle.EsParejaManual).ToList();
                }

                return BancosFuncProcesar(IdCuenta, IdExtracto, Corte, Responsable, Existente, Manuales, out Resultado);
            }
            catch (OdbcException)
            {
                Resultado = null;
                return "Error de base de datos al realizar el corte de conciliación.";
            }
            catch (Exception)
            {
                Resultado = null;
                return "Error inesperado al realizar el corte de conciliación.";
            }
        }

        public decimal BancosFuncDiferenciaMarca(decimal ImporteLibro, decimal ImporteBanco)
        {
            return Math.Abs(ImporteBanco) - Math.Abs(ImporteLibro);
        }

        public string BancosMetMarcar(long IdConciliacion, long IdMovimiento, long IdLinea, bool AceptarDiferencia,
            string Responsable, out ClsResultadoConciliacion Resultado)
        {
            Resultado = null;
            if (IdConciliacion <= 0)
                return MensajeSinConciliacion;
            if (IdMovimiento <= 0 || IdLinea <= 0)
                return MensajeSeleccionPartidas;

            try
            {
                ClsConciliacion Encabezado = _Repositorio.BancosMetObtenerConciliacion(IdConciliacion);
                if (Encabezado == null)
                    return MensajeSinConciliacion;
                if (Encabezado.Estado == ClsValoresConciliacion.EstadoCerrada)
                    return MensajeCerrada;

                List<ClsDetalleConciliacion> Detalles = _Repositorio.BancosMetConsultarDetalles(IdConciliacion);
                ClsDetalleConciliacion SoloLibro = Detalles.FirstOrDefault(Detalle =>
                    Detalle.Resultado == ClsValoresConciliacion.ResultadoPendiente && Detalle.Linea == null
                    && Detalle.Movimiento != null && Detalle.Movimiento.IdMovimiento == IdMovimiento);
                ClsDetalleConciliacion SoloBanco = Detalles.FirstOrDefault(Detalle =>
                    Detalle.Resultado == ClsValoresConciliacion.ResultadoPendiente && Detalle.Movimiento == null
                    && Detalle.Linea != null && Detalle.Linea.IdLinea == IdLinea);

                if (SoloLibro == null || SoloBanco == null)
                    return "Solo se pueden marcar partidas que estén pendientes.";
                if (SoloLibro.Movimiento.Sentido != SoloBanco.Linea.Sentido)
                    return "El movimiento y la línea del estado de cuenta deben ser ambos cargos o ambos abonos.";

                bool ImportesIguales = SoloLibro.Movimiento.Importe == SoloBanco.Linea.Importe;
                if (!ImportesIguales && !AceptarDiferencia)
                    return "Los importes no coinciden. Confirme la diferencia para marcar las partidas.";

                List<ClsDetalleConciliacion> Manuales = Detalles.Where(Detalle => Detalle.EsParejaManual).ToList();
                Manuales.Add(new ClsDetalleConciliacion
                {
                    Resultado = ImportesIguales ? ClsValoresConciliacion.ResultadoCoincide : ClsValoresConciliacion.ResultadoDiferencia,
                    Metodo = ClsValoresConciliacion.MetodoManual,
                    Diferencia = SoloBanco.Linea.Importe - SoloLibro.Movimiento.Importe,
                    Motivo = ImportesIguales ? "Marcado manualmente." : "Importe distinto, marcado manualmente.",
                    Movimiento = SoloLibro.Movimiento,
                    Linea = SoloBanco.Linea
                });

                return BancosFuncProcesar(Encabezado.IdCuenta, Encabezado.IdExtracto, Encabezado.FechaCorte,
                    Responsable, Encabezado, Manuales, out Resultado);
            }
            catch (OdbcException)
            {
                Resultado = null;
                return "Error de base de datos al marcar las partidas.";
            }
            catch (Exception)
            {
                Resultado = null;
                return "Error inesperado al marcar las partidas.";
            }
        }

        public string BancosMetDesmarcar(long IdConciliacion, long IdMovimiento, long IdLinea,
            string Responsable, out ClsResultadoConciliacion Resultado)
        {
            Resultado = null;
            if (IdConciliacion <= 0)
                return MensajeSinConciliacion;
            if (IdMovimiento <= 0 || IdLinea <= 0)
                return "Seleccione una pareja marcada manualmente para desmarcarla.";

            try
            {
                ClsConciliacion Encabezado = _Repositorio.BancosMetObtenerConciliacion(IdConciliacion);
                if (Encabezado == null)
                    return MensajeSinConciliacion;
                if (Encabezado.Estado == ClsValoresConciliacion.EstadoCerrada)
                    return MensajeCerrada;

                List<ClsDetalleConciliacion> Manuales = _Repositorio.BancosMetConsultarDetalles(IdConciliacion)
                    .Where(Detalle => Detalle.EsParejaManual).ToList();
                int Quitadas = Manuales.RemoveAll(Detalle =>
                    Detalle.Movimiento.IdMovimiento == IdMovimiento && Detalle.Linea.IdLinea == IdLinea);
                if (Quitadas == 0)
                    return "Solo se pueden desmarcar las partidas que se marcaron manualmente.";

                return BancosFuncProcesar(Encabezado.IdCuenta, Encabezado.IdExtracto, Encabezado.FechaCorte,
                    Responsable, Encabezado, Manuales, out Resultado);
            }
            catch (OdbcException)
            {
                Resultado = null;
                return "Error de base de datos al desmarcar las partidas.";
            }
            catch (Exception)
            {
                Resultado = null;
                return "Error inesperado al desmarcar las partidas.";
            }
        }

        public string BancosMetConsultarReporte(long IdConciliacion, out DataTable Resultado)
        {
            Resultado = null;
            if (IdConciliacion <= 0)
                return "No hay una conciliación para reportar. Realice primero el corte.";

            try
            {
                ClsConciliacion Encabezado = _Repositorio.BancosMetObtenerConciliacion(IdConciliacion);
                if (Encabezado == null)
                    return "La conciliación seleccionada ya no existe.";

                List<ClsDetalleConciliacion> Detalles = ClsMotorConciliacion.BancosFuncOrdenar(
                    _Repositorio.BancosMetConsultarDetalles(IdConciliacion));
                Resultado = ClsTablaConciliacion.BancosFuncConstruir(Detalles, Encabezado);
                return string.Empty;
            }
            catch (OdbcException)
            {
                return "Error de base de datos al consultar el reporte.";
            }
            catch (Exception)
            {
                return "Error inesperado al consultar el reporte.";
            }
        }

        private string BancosFuncProcesar(long IdCuenta, long IdExtracto, DateTime FechaCorte, string Responsable,
            ClsConciliacion Existente, List<ClsDetalleConciliacion> Manuales, out ClsResultadoConciliacion Resultado)
        {
            Resultado = null;
            ClsDatosExtracto Extracto = _Repositorio.BancosMetObtenerExtracto(IdExtracto);
            if (Extracto == null)
                return "No se encontró el extracto seleccionado.";
            if (Extracto.IdCuenta != IdCuenta)
                return "El extracto no pertenece a la cuenta seleccionada.";
            if (FechaCorte < Extracto.FechaInicio || FechaCorte > Extracto.FechaFin)
                return "La fecha de corte debe estar dentro del período del extracto ("
                    + Extracto.FechaInicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " al "
                    + Extracto.FechaFin.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + ").";

            long IdExistente = Existente == null ? 0 : Existente.IdConciliacion;
            List<ClsMovimientoLibro> Libro = _Repositorio.BancosMetConsultarMovimientosLibro(IdCuenta, FechaCorte, IdExistente);
            List<ClsLineaExtracto> Banco = _Repositorio.BancosMetConsultarLineasExtracto(IdExtracto, FechaCorte, IdExistente);
            List<ClsDetalleConciliacion> Detalles = ClsMotorConciliacion.BancosFuncEmparejar(Libro, Banco, Manuales);

            decimal SaldoBanco = Extracto.SaldoInicial + _Repositorio.BancosMetObtenerNetoExtracto(IdExtracto, FechaCorte);
            decimal SaldoLibro = _Repositorio.BancosMetObtenerSaldoLibro(IdCuenta, FechaCorte);
            decimal Diferencia = SaldoBanco - SaldoLibro;
            ClsTotalesConciliacion Totales = ClsMotorConciliacion.BancosFuncCalcularTotales(Detalles);

            ClsConciliacion Encabezado = new ClsConciliacion
            {
                IdConciliacion = IdExistente,
                IdCuenta = IdCuenta,
                IdExtracto = IdExtracto,
                FechaCorte = FechaCorte,
                Estado = ClsMotorConciliacion.BancosFuncDeterminarEstado(Totales, Diferencia),
                Metodo = ClsMotorConciliacion.BancosFuncDeterminarMetodo(Detalles),
                SaldoLibro = SaldoLibro,
                SaldoBanco = SaldoBanco,
                Diferencia = Diferencia,
                Responsable = BancosFuncResponsable(Responsable)
            };

            string Error = _Repositorio.BancosMetGuardarConciliacion(Encabezado, Detalles, out long IdConciliacion);
            if (!string.IsNullOrEmpty(Error))
                return Error;

            Resultado = new ClsResultadoConciliacion
            {
                IdConciliacion = IdConciliacion,
                Estado = Encabezado.Estado,
                EstadoTexto = ClsMotorConciliacion.BancosFuncTextoEstado(Encabezado.Estado),
                SaldoInicialBanco = Extracto.SaldoInicial,
                SaldoBanco = SaldoBanco,
                SaldoLibro = SaldoLibro,
                Diferencia = Diferencia,
                CargosConciliados = Totales.CargosConciliados,
                AbonosConciliados = Totales.AbonosConciliados,
                CargosPorConciliar = Totales.CargosPorConciliar,
                AbonosPorConciliar = Totales.AbonosPorConciliar,
                Coincidencias = Totales.Coincidencias,
                Diferencias = Totales.Diferencias,
                Pendientes = Totales.Pendientes,
                Tabla = ClsTablaConciliacion.BancosFuncConstruir(Detalles, null)
            };
            return string.Empty;
        }

        private static string BancosFuncResponsable(string Responsable)
        {
            string Texto = string.IsNullOrWhiteSpace(Responsable) ? "SISTEMA" : Responsable.Trim();
            return Texto.Length > 100 ? Texto.Substring(0, 100) : Texto;
        }
    }
}
