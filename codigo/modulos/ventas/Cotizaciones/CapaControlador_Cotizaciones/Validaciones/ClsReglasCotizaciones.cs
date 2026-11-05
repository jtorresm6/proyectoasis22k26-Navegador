using System;
using System.Data;
using CapaModelo_Cotizaciones;
using VentasValidaciones;

namespace CapaControlador_Cotizaciones.Validaciones
{
    internal static class ClsReglasCotizaciones
    {
        internal static void VentasMetUsuario(this ClsCotizacionesRepositorio Repositorio, int Usuario)
        {
            ClsValidacionesVentas.VentasMetExigir(Usuario > 0 && Repositorio.VentasFuncUsuarioActivo(Usuario), "Seleccione un responsable activo de Seguridad.");
        }

        internal static void VentasMetBloquearMoneda(this ClsCotizacionesRepositorio Repositorio, int Moneda)
        {
            ClsValidacionesVentas.VentasMetExigir(Repositorio.EnTransaccion, "La operacion requiere una transaccion.");
            ClsValidacionesVentas.VentasMetExigir(Moneda > 0 && Repositorio.VentasFuncMonedaActiva(Moneda), "Seleccione una moneda activa.");
            Repositorio.VentasMetAsegurarBloqueoMoneda(Moneda);
        }

        internal static void VentasMetPeriodoAbierto(this ClsCotizacionesRepositorio Repositorio, int Moneda, DateTime Fecha)
        {
            Repositorio.VentasMetBloquearMoneda(Moneda);
            object Ultimo = Repositorio.VentasFuncUltimoCierre(Moneda);
            ClsValidacionesVentas.VentasMetExigir(Ultimo == null || Ultimo == DBNull.Value || Fecha.Date > Convert.ToDateTime(Ultimo).Date, "La fecha pertenece a un periodo cerrado. Registre el movimiento en un periodo abierto.");
        }

        internal static bool VentasFuncPuedeEditar(int Id, string Estado, DateTime Vigencia)
        {
            return Id == 0 || Estado == "BORRADOR" && Vigencia.Date >= DateTime.Today;
        }

        internal static void VentasMetValidarEdicion(int Id, string Estado, DateTime Vigencia)
        {
            ClsValidacionesVentas.VentasMetExigir(VentasFuncPuedeEditar(Id, Estado, Vigencia), "Solo se modifica una cotizacion en borrador y vigente.");
        }

        internal static object[] VentasFuncCalcularLinea(DataRow Producto, decimal Cantidad, decimal Descuento)
        {
            ClsValidacionesVentas.VentasMetExigir(Producto != null, "Seleccione un producto de la lista asignada al cliente.");
            decimal Precio = Convert.ToDecimal(Producto["Precio"]), Tasa = Convert.ToDecimal(Producto["Tasa"]);
            ClsValidacionesVentas.VentasMetExigir(Cantidad > 0 && Cantidad == Math.Round(Cantidad, 4), "La cantidad debe ser positiva y tener hasta cuatro decimales.");
            decimal Base = Math.Round(Cantidad * Precio, 2, MidpointRounding.AwayFromZero);
            ClsValidacionesVentas.VentasMetExigir(Descuento >= 0 && Descuento <= Base && Descuento == Math.Round(Descuento, 2), "El descuento debe tener hasta dos decimales y no superar el subtotal.");
            decimal Impuesto = Math.Round((Base - Descuento) * Tasa / 100m, 2, MidpointRounding.AwayFromZero);
            return new object[]
            {
                Producto["Id"],
                Producto["Descripcion"],
                Producto["Tipo"],
                Cantidad,
                Precio,
                Descuento,
                Tasa,
                Base,
                Impuesto,
                Base - Descuento + Impuesto
            };
        }
    }
}
