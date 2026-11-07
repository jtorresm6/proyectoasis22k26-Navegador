using System;
using CapaModelo_CierreMovimientos;
using VentasValidaciones;

namespace CapaControlador_CierreMovimientos.Validaciones
{
    internal static class ClsReglasCierre
    {
        internal static void VentasMetUsuario(this ClsCierreRepositorio Repositorio, int Usuario)
        {
            ClsValidacionesVentas.VentasMetExigir(Usuario > 0 && Repositorio.VentasFuncUsuarioActivo(Usuario), "Seleccione un responsable activo de Seguridad.");
        }

        internal static void VentasMetBloquearMoneda(this ClsCierreRepositorio Repositorio, int Moneda)
        {
            ClsValidacionesVentas.VentasMetExigir(Repositorio.EnTransaccion, "La operacion requiere una transaccion.");
            ClsValidacionesVentas.VentasMetExigir(Moneda > 0 && Repositorio.VentasFuncMonedaActiva(Moneda), "Seleccione una moneda activa.");
            Repositorio.VentasMetAsegurarBloqueoMoneda(Moneda);
        }

        internal static void VentasMetPeriodoAbierto(this ClsCierreRepositorio Repositorio, int Moneda, DateTime Fecha)
        {
            Repositorio.VentasMetBloquearMoneda(Moneda);
            object Ultimo = Repositorio.VentasFuncUltimoCierre(Moneda);
            ClsValidacionesVentas.VentasMetExigir(Ultimo == null || Ultimo == DBNull.Value || Fecha.Date > Convert.ToDateTime(Ultimo).Date, "La fecha pertenece a un periodo cerrado. Registre el movimiento en un periodo abierto.");
        }

        internal static DateTime[] VentasFuncPeriodo(int Anio, int Mes, bool Anual)
        {
            ClsValidacionesVentas.VentasMetExigir(Anio >= 2000 && Anio <= 9998 && Mes >= 1 && Mes <= 12, "Seleccione un anio y mes validos.");
            DateTime Desde = new DateTime(Anio, Anual ? 1 : Mes, 1);
            return new[]
            {
                Desde,
                Anual ? new DateTime(Anio, 12, 31) : Desde.AddMonths(1).AddDays(-1)
            };
        }

        internal static void VentasMetValidarPeriodo(string Tipo, DateTime Desde, DateTime Hasta)
        {
            bool Mensual = Tipo == "MENSUAL" && Desde.Day == 1 && Hasta.Date == Desde.Date.AddMonths(1).AddDays(-1);
            bool Anual = Tipo == "ANUAL" && Desde.Month == 1 && Desde.Day == 1 && Hasta.Date == Desde.Date.AddYears(1).AddDays(-1);
            ClsValidacionesVentas.VentasMetExigir(Mensual || Anual, "Seleccione un periodo mensual o anual completo.");
        }
    }
}
