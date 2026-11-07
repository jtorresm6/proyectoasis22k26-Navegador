using System;
using CapaModelo_MovimientosClientes;
using VentasValidaciones;

namespace CapaControlador_MovimientosClientes.Validaciones
{
    internal static class ClsReglasMovimientos
    {
        internal static void VentasMetUsuario(this ClsMovimientosRepositorio Repositorio, int Usuario)
        {
            ClsValidacionesVentas.VentasMetExigir(Usuario > 0 && Repositorio.VentasFuncUsuarioActivo(Usuario), "Seleccione un responsable activo de Seguridad.");
        }

        internal static void VentasMetBloquearMoneda(this ClsMovimientosRepositorio Repositorio, int Moneda)
        {
            ClsValidacionesVentas.VentasMetExigir(Repositorio.EnTransaccion, "La operacion requiere una transaccion.");
            ClsValidacionesVentas.VentasMetExigir(Moneda > 0 && Repositorio.VentasFuncMonedaActiva(Moneda), "Seleccione una moneda activa.");
            Repositorio.VentasMetAsegurarBloqueoMoneda(Moneda);
        }

        internal static void VentasMetPeriodoAbierto(this ClsMovimientosRepositorio Repositorio, int Moneda, DateTime Fecha)
        {
            Repositorio.VentasMetBloquearMoneda(Moneda);
            object Ultimo = Repositorio.VentasFuncUltimoCierre(Moneda);
            ClsValidacionesVentas.VentasMetExigir(Ultimo == null || Ultimo == DBNull.Value || Fecha.Date > Convert.ToDateTime(Ultimo).Date, "La fecha pertenece a un periodo cerrado. Registre el movimiento en un periodo abierto.");
        }
    }
}
