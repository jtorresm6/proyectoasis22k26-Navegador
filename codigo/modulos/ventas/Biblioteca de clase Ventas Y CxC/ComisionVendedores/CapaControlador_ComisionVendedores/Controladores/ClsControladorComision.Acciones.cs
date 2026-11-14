// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Coordina la asignación de vendedores, el cálculo por factura y el registro de abonos.

using System;
using System.Collections.Generic;
using System.Linq;
using CapaControlador_ComisionVendedores.Contratos;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Enumeraciones;
using CapaModelo_ComisionVendedores.Contratos;
using CapaModelo_ComisionVendedores.Entidades;
using CapaModelo_ComisionVendedores.Repositorios;

namespace CapaControlador_ComisionVendedores.Controladores
{
    public partial class ClsControladorComision
    {
        public List<ClsFacturaComisionVista> VentasMetObtenerFacturas()
        {
            List<ClsFacturaComisionVista> Resultado = new List<ClsFacturaComisionVista>();
            foreach (ClsFacturaComision Factura in _RepositorioComision.VentasMetObtenerFacturas())
                Resultado.Add(new ClsFacturaComisionVista { IdFactura = Factura.IdFactura, IdVendedor = Factura.IdVendedor, Fecha = Factura.Fecha, Cliente = Factura.Cliente, Total = Factura.Total, TotalPagado = Factura.TotalPagado, UltimoAbono = Factura.UltimoAbono, SimboloMoneda = Factura.SimboloMoneda });
            return Resultado;
        }

        public List<ClsVendedorComisionVista> VentasMetObtenerVendedores()
        {
            List<ClsVendedorComisionVista> Resultado = new List<ClsVendedorComisionVista>();
            foreach (ClsVendedorComision Vendedor in _RepositorioComision.VentasMetObtenerVendedores())
                Resultado.Add(new ClsVendedorComisionVista { IdVendedor = Vendedor.IdVendedor, NombreVendedor = Vendedor.NombreVendedor, Porcentaje = Vendedor.Porcentaje });
            return Resultado;
        }

        public List<ClsComisionFacturaDetalle> VentasMetCalcularFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo)
        {
            if (Metodo == EnumMetodoComision.Vendedor)
                throw new ArgumentException("La comisión se calcula por Línea, Producto, Categoría o Marca.");
            List<ClsDetalleProductoComision> Iniciales = _RepositorioComision.VentasMetObtenerDetallesFactura(IdFactura, Metodo.ToString());
            List<ClsComisionFacturaDetalle> Selecciones = Iniciales.Select(Detalle => new ClsComisionFacturaDetalle
            {
                IdFacturaDetalle = Detalle.IdFacturaDetalle,
                MetodoComision = Detalle.ComisionGuardada ? Detalle.MetodoGuardado : VentasMetObtenerMetodoMenor(Detalle),
                PorcentajeProducto = Detalle.PorcentajeProducto,
                PorcentajeMarca = Detalle.PorcentajeMarca,
                PorcentajeLinea = Detalle.PorcentajeLinea,
                PorcentajeCategoria = Detalle.PorcentajeCategoria
            }).ToList();
            List<ClsComisionFacturaDetalle> Resultado = VentasMetCalcularFactura(IdFactura, IdVendedor, Selecciones);
            return Resultado;
        }

        private static string VentasMetObtenerMetodoMenor(ClsDetalleProductoComision Detalle)
        {
            return new[] {
                new { Metodo = "Producto", Tasa = Detalle.PorcentajeProducto },
                new { Metodo = "Marca", Tasa = Detalle.PorcentajeMarca },
                new { Metodo = "Linea", Tasa = Detalle.PorcentajeLinea },
                new { Metodo = "Categoria", Tasa = Detalle.PorcentajeCategoria }
            }.OrderBy(Opcion => Opcion.Tasa).First().Metodo;
        }

        public List<ClsComisionFacturaDetalle> VentasMetCalcularFactura(int IdFactura, int IdVendedor, List<ClsComisionFacturaDetalle> Selecciones)
        {
            ClsFacturaComision Factura = _RepositorioComision.VentasMetObtenerFactura(IdFactura);
            ClsVendedorComision Vendedor = _RepositorioComision.VentasMetObtenerVendedor(IdVendedor);
            if (Factura == null) throw new ArgumentException("La factura seleccionada no existe.");
            if (Vendedor == null) throw new ArgumentException("El vendedor seleccionado no existe.");
            _RepositorioComision.VentasMetSincronizarAbonos(IdFactura);
            if (Selecciones == null || Selecciones.Count == 0)
                throw new InvalidOperationException("La factura no tiene productos para calcular comisión.");
            decimal ProporcionPagada = Factura.Total <= 0 ? 0 : Math.Min(1m, Math.Max(0m, Factura.TotalPagado / Factura.Total));
            decimal PagoPrevioAlUltimo = Math.Max(0m, Factura.TotalPagado - Factura.UltimoAbono);
            decimal UltimoAbonoAplicable = Math.Min(Factura.UltimoAbono, Math.Max(0m, Factura.Total - PagoPrevioAlUltimo));
            decimal ProporcionUltimoAbono = Factura.Total <= 0 ? 0 : UltimoAbonoAplicable / Factura.Total;
            List<ClsComisionFacturaDetalle> Resultado = new List<ClsComisionFacturaDetalle>();
            var DetallesPorMetodo = new Dictionary<EnumMetodoComision, ILookup<int, ClsDetalleProductoComision>>();
            foreach (ClsComisionFacturaDetalle Seleccion in Selecciones)
            {
                EnumMetodoComision Metodo;
                if (!Enum.TryParse(Seleccion.MetodoComision, true, out Metodo) || Metodo == EnumMetodoComision.Vendedor)
                    throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto para cada producto.");
                if (!DetallesPorMetodo.TryGetValue(Metodo, out var Detalles))
                {
                    Detalles = _RepositorioComision.VentasMetObtenerDetallesFactura(IdFactura, Metodo.ToString())
                        .ToLookup(Item => Item.IdFacturaDetalle);
                    DetallesPorMetodo.Add(Metodo, Detalles);
                }
                ClsDetalleProductoComision Detalle = Detalles[Seleccion.IdFacturaDetalle].FirstOrDefault();
                if (Detalle == null) throw new InvalidOperationException("No se encontró un producto de la factura.");
                if (Detalle.IdProductoLogistica <= 0 || Detalle.IdClasificacionLogistica <= 0)
                    throw new InvalidOperationException("El producto " + Detalle.NombreProducto + " no tiene relación o tasa válida en Logística.");
                if (Detalle.ComisionGuardada && !string.Equals(Detalle.MetodoGuardado, Metodo.ToString(), StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Esta factura ya tiene una comisión guardada con otro método; el cálculo histórico no se puede cambiar.");
                if (Detalle.ComisionGuardada && Detalle.IdVendedorGuardado > 0 && Detalle.IdVendedorGuardado != IdVendedor)
                    throw new InvalidOperationException("Esta factura ya tiene una comisión guardada para otro vendedor.");
                decimal BaseVenta = Detalle.BaseComisionable;
                decimal Porcentaje = Detalle.Porcentaje;
                decimal ComisionTotal = Math.Round(BaseVenta * Porcentaje / 100m, 4, MidpointRounding.AwayFromZero);
                decimal Disponible = Detalle.ComisionGuardada
                    ? Math.Min(ComisionTotal, Math.Round(ComisionTotal * ProporcionPagada, 2, MidpointRounding.AwayFromZero))
                    : Math.Round(ComisionTotal * ProporcionPagada, 2, MidpointRounding.AwayFromZero);
                decimal ComisionAbono = Math.Round(ComisionTotal * ProporcionUltimoAbono, 4, MidpointRounding.AwayFromZero);
                Resultado.Add(new ClsComisionFacturaDetalle
                {
                    IdFacturaDetalle = Detalle.IdFacturaDetalle,
                    MetodoComision = Detalle.ComisionGuardada ? Detalle.MetodoGuardado : Metodo.ToString(),
                    ComisionGuardada = Detalle.ComisionGuardada,
                    NombreProducto = Detalle.NombreProducto,
                    ElementoComision = Detalle.ElementoComision,
                    Cantidad = Detalle.Cantidad,
                    BaseVenta = BaseVenta,
                    Porcentaje = Porcentaje,
                    PorcentajeProducto = Detalle.PorcentajeProducto,
                    PorcentajeMarca = Detalle.PorcentajeMarca,
                    PorcentajeLinea = Detalle.PorcentajeLinea,
                    PorcentajeCategoria = Detalle.PorcentajeCategoria,
                    ComisionTotal = ComisionTotal,
                    VentaCobrado = Math.Round(BaseVenta * ProporcionPagada, 4, MidpointRounding.AwayFromZero),
                    ComisionUltimoAbono = ComisionAbono,
                    ComisionDisponible = Disponible,
                    ComisionPendiente = Math.Max(0m, ComisionTotal - Disponible)
                });
            }
            return Resultado;
        }

        public int VentasMetGuardarComisionFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo)
        {
            if (IdFactura <= 0 || IdVendedor <= 0) throw new ArgumentOutOfRangeException("Factura y vendedor deben ser válidos.");
            if (Metodo == EnumMetodoComision.Vendedor) throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            return _RepositorioComision.VentasMetGuardarComisionFactura(IdFactura, IdVendedor, Metodo.ToString());
        }

        public int VentasMetAsignarVendedorFactura(int IdFactura, int IdVendedor)
        {
            if (IdFactura <= 0 || IdVendedor <= 0) throw new ArgumentOutOfRangeException("Factura y vendedor deben ser válidos.");
            return _RepositorioComision.VentasMetAsignarVendedorFactura(IdFactura, IdVendedor);
        }

        public int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, EnumMetodoComision Metodo)
        {
            if (IdFactura <= 0 || IdVendedor <= 0) throw new ArgumentOutOfRangeException("Factura y vendedor deben ser válidos.");
            if (Metodo == EnumMetodoComision.Vendedor) throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            return _RepositorioComision.VentasMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Metodo.ToString());
        }

        public int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsComisionFacturaDetalle> Selecciones)
        {
            if (IdFactura <= 0 || IdVendedor <= 0) throw new ArgumentOutOfRangeException("Factura y vendedor deben ser válidos.");
            if (Selecciones == null || Selecciones.Count == 0) throw new ArgumentException("Elija el método para cada producto antes de asignar.");
            List<ClsDetalleProductoComision> Datos = Selecciones.Select(Seleccion => new ClsDetalleProductoComision
            {
                IdFacturaDetalle = Seleccion.IdFacturaDetalle,
                MetodoSeleccionado = Seleccion.MetodoComision
            }).ToList();
            return _RepositorioComision.VentasMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Datos);
        }

        public List<ClsFacturaPagoVista> VentasMetObtenerFacturasConSaldo()
        {
            return _RepositorioComision.VentasMetObtenerFacturasConSaldo()
                .Select(Factura => new ClsFacturaPagoVista
                {
                    IdFactura = Factura.IdFactura,
                    IdCliente = Factura.IdCliente,
                    IdMoneda = Factura.IdMoneda,
                    Fecha = Factura.Fecha,
                    Estado = Factura.Estado,
                    Etiqueta = Factura.Etiqueta,
                    SimboloMoneda = Factura.SimboloMoneda,
                    Total = Factura.Total,
                    TotalPagado = Factura.TotalPagado,
                    Saldo = Factura.Saldo,
                    ComisionTotal = Factura.ComisionTotal,
                    ComisionDevengada = Factura.ComisionDevengada,
                    ComisionPagada = Factura.ComisionPagada,
                    ComisionPendientePago = Factura.ComisionPendientePago
                }).ToList();
        }

        public List<ClsFormaPagoVentaVista> VentasMetObtenerFormasPagoVenta()
        {
            return _RepositorioComision.VentasMetObtenerFormasPagoVenta()
                .Select(Forma => new ClsFormaPagoVentaVista
                {
                    IdFormaPago = Forma.IdFormaPago,
                    Nombre = Forma.Nombre,
                    RequiereReferencia = Forma.RequiereReferencia
                }).ToList();
        }

        public decimal VentasMetRegistrarAbonoFactura(int IdFactura, decimal Monto,
            int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario)
        {
            if (IdFactura <= 0) throw new ArgumentOutOfRangeException("IdFactura", "Seleccione una factura válida.");
            if (Monto <= 0m || Monto != Math.Round(Monto, 2, MidpointRounding.AwayFromZero))
                throw new ArgumentOutOfRangeException("Monto", "El monto del abono debe ser positivo y tener hasta dos decimales.");
            if (IdFormaPago <= 0) throw new ArgumentOutOfRangeException("IdFormaPago", "Seleccione un método de pago.");
            if (IdUsuario <= 0) throw new ArgumentOutOfRangeException("IdUsuario", "No se encontró un usuario activo en la sesión.");
            return _RepositorioComision.VentasMetRegistrarAbonoFactura(
                IdFactura, Monto, IdFormaPago, Fecha.Date, Referencia, IdUsuario);
        }

        public List<ClsComisionNominaVista> VentasMetObtenerComisionesNomina()
        {
            return _RepositorioComision.VentasMetObtenerComisionesNomina()
                .Select(Comision => new ClsComisionNominaVista
                {
                    IdAbono = Comision.IdAbono,
                    IdFactura = Comision.IdFactura,
                    FechaAbono = Comision.FechaAbono,
                    Vendedor = Comision.Vendedor,
                    SimboloMoneda = Comision.SimboloMoneda,
                    TotalComision = Comision.TotalComision,
                    MontoPagoCliente = Comision.MontoPagoCliente,
                    ComisionDevengadaAbono = Comision.ComisionDevengadaAbono,
                    EstadoPago = Comision.EstadoPago
                }).ToList();
        }
    }
}
