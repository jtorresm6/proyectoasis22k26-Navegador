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
    public class ClsControladorComision : IControladorComision
    {
        private readonly IRepositorioComision _RepositorioComision;

        public ClsControladorComision()
            : this(new ClsRepositorioComision())
        {
        }

        public ClsControladorComision(IRepositorioComision RepositorioComision)
        {
            _RepositorioComision = RepositorioComision ?? throw new ArgumentNullException("RepositorioComision");
        }

        public List<ClsComisionVendedorResultado> ComisionVendedoresMetCalcular(
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo,
            DateTime FechaInicio,
            DateTime FechaFin)
        {
            if (FechaInicio.Date > FechaFin.Date)
                throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");

            if (BaseCalculo != EnumBaseComision.Cobrado)
                throw new NotSupportedException("Las comisiones se devengan sobre cobros confirmados.");

            List<ClsVentaVendedorBase> Ventas = _RepositorioComision.ComisionVendedoresMetObtenerVentasPorMetodo(
                "Vendedor",
                FechaInicio.Date,
                FechaFin.Date.AddDays(1));
            List<ClsComisionVendedorResultado> Resultados = new List<ClsComisionVendedorResultado>();
            foreach (System.Linq.IGrouping<string, ClsVentaVendedorBase> Grupo in System.Linq.Enumerable.GroupBy(
                Ventas, Venta => Venta.IdVendedor + "|" + Venta.IdMoneda + "|" + Venta.IdFactura + "|" + Venta.IdFacturaDetalle))
            {
                ClsVentaVendedorBase Primero = System.Linq.Enumerable.First(Grupo);
                List<ClsVentaVendedorBase> Movimientos = System.Linq.Enumerable.ToList(Grupo);
                Resultados.Add(new ClsComisionVendedorResultado
                {
                    IdVendedor = Primero.IdVendedor,
                    IdMoneda = Primero.IdMoneda,
                    SimboloMoneda = Primero.SimboloMoneda,
                    NombreVendedor = Primero.NombreVendedor,
                    ElementoComision = Primero.ElementoComision,
                    MetodoComision = Primero.MetodoComision,
                    Porcentaje = Primero.PorcentajeVendedor,
                    BaseVentas = System.Linq.Enumerable.Sum(Movimientos, Movimiento => Movimiento.BaseVentas),
                    MontoComision = System.Linq.Enumerable.Sum(Movimientos, Movimiento => Movimiento.MontoComision),
                    Movimientos = Movimientos.ConvertAll(Movimiento => new ClsMovimientoComisionVista
                    {
                        IdVendedor = Movimiento.IdVendedor,
                        IdMoneda = Movimiento.IdMoneda,
                        SimboloMoneda = Movimiento.SimboloMoneda,
                        IdFactura = Movimiento.IdFactura,
                        IdFacturaDetalle = Movimiento.IdFacturaDetalle,
                        MetodoComision = Movimiento.MetodoComision,
                        IdAplicacionPago = Movimiento.IdAplicacionPago,
                        IdComisionAbono = Movimiento.IdComisionAbono,
                        IdReglaComision = Movimiento.IdReglaComision,
                        FechaAplicacion = Movimiento.FechaAplicacion,
                        NombreVendedor = Movimiento.NombreVendedor,
                        ElementoComision = Movimiento.ElementoComision,
                        PorcentajeVendedor = Movimiento.PorcentajeVendedor,
                        BaseVentas = Movimiento.BaseVentas,
                        MontoComision = Movimiento.MontoComision
                    })
                });
            }
            return Resultados;
        }

        public int ComisionVendedoresMetGuardar(
            List<ClsComisionVendedorResultado> Resultados,
            DateTime FechaInicio,
            DateTime FechaFin,
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo)
        {
            if (Resultados == null || Resultados.Count == 0)
                return 0;

            if (FechaInicio.Date > FechaFin.Date)
                throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");

            if (BaseCalculo != EnumBaseComision.Cobrado)
                throw new NotSupportedException("Las comisiones se pagan proporcionalmente a cobros confirmados.");
            List<ClsVentaVendedorBase> Comisiones = new List<ClsVentaVendedorBase>();
            foreach (ClsComisionVendedorResultado Resultado in Resultados)
                if (Resultado.Movimientos != null)
                    foreach (ClsMovimientoComisionVista Movimiento in Resultado.Movimientos)
                        Comisiones.Add(new ClsVentaVendedorBase
                        {
                            IdVendedor = Movimiento.IdVendedor,
                            IdMoneda = Movimiento.IdMoneda,
                            SimboloMoneda = Movimiento.SimboloMoneda,
                            IdFactura = Movimiento.IdFactura,
                            IdFacturaDetalle = Movimiento.IdFacturaDetalle,
                            MetodoComision = Movimiento.MetodoComision,
                            IdAplicacionPago = Movimiento.IdAplicacionPago,
                            IdComisionAbono = Movimiento.IdComisionAbono,
                            IdReglaComision = Movimiento.IdReglaComision,
                            FechaAplicacion = Movimiento.FechaAplicacion,
                            NombreVendedor = Movimiento.NombreVendedor,
                            ElementoComision = Movimiento.ElementoComision,
                            PorcentajeVendedor = Movimiento.PorcentajeVendedor,
                            BaseVentas = Movimiento.BaseVentas,
                            MontoComision = Movimiento.MontoComision
                        });
            if (Comisiones.Count == 0) throw new InvalidOperationException("Vuelva a calcular para cargar los abonos que se enviarán.");
            return _RepositorioComision.ComisionVendedoresMetGuardarComisiones(Comisiones, FechaInicio.Date, FechaFin.Date, Metodo.ToString());
        }

        public List<ClsProductoPorcentaje> ComisionVendedoresMetConsultarPorcentajes(EnumMetodoComision Metodo)
        {
            ComisionVendedoresMetValidarMetodoMantenimiento(Metodo);
            List<ClsProductoPorcentaje> Resultado = new List<ClsProductoPorcentaje>();
            foreach (ClsProductoComision Producto in _RepositorioComision.ComisionVendedoresMetConsultarPorcentajes(Metodo.ToString()))
                Resultado.Add(new ClsProductoPorcentaje
                {
                    IdProducto = Producto.IdProducto, IdMarca = Producto.IdMarca, IdLinea = Producto.IdLinea,
                    IdCategoria = Producto.IdCategoria, NombreProducto = Producto.NombreProducto,
                    Marca = Producto.Marca, Linea = Producto.Linea, Categoria = Producto.Categoria,
                    Porcentaje = Producto.Porcentaje, PorcentajeMarca = Producto.PorcentajeMarca,
                    PorcentajeLinea = Producto.PorcentajeLinea, PorcentajeCategoria = Producto.PorcentajeCategoria,
                    PorcentajeProducto = Producto.PorcentajeProducto
                });
            return Resultado;
        }

        public int ComisionVendedoresMetActualizarPorcentaje(EnumMetodoComision Metodo, int IdClasificacion, decimal Porcentaje)
        {
            ComisionVendedoresMetValidarMetodoMantenimiento(Metodo);
            if (IdClasificacion <= 0) throw new ArgumentException("Seleccione el elemento específico que desea actualizar.");
            if (Porcentaje < 0 || Porcentaje > 99m) throw new ArgumentOutOfRangeException("Porcentaje", "El porcentaje debe estar entre 0.00 y 99.00% según el esquema de la base de datos.");
            return _RepositorioComision.ComisionVendedoresMetActualizarPorcentaje(Metodo.ToString(), IdClasificacion, Porcentaje);
        }

        private void ComisionVendedoresMetValidarMetodoMantenimiento(EnumMetodoComision Metodo)
        {
            switch (Metodo)
            {
                case EnumMetodoComision.Marca:
                case EnumMetodoComision.Linea:
                case EnumMetodoComision.Producto:
                case EnumMetodoComision.Categoria:
                    return;
                default:
                    throw new ArgumentException("Seleccione Marca, Línea, Producto o Categoría.");
            }
        }

        public List<ClsFacturaComisionVista> ComisionVendedoresMetObtenerFacturas()
        {
            List<ClsFacturaComisionVista> Resultado = new List<ClsFacturaComisionVista>();
            foreach (ClsFacturaComision Factura in _RepositorioComision.ComisionVendedoresMetObtenerFacturas())
                Resultado.Add(new ClsFacturaComisionVista { IdFactura = Factura.IdFactura, IdVendedor = Factura.IdVendedor, Fecha = Factura.Fecha, Cliente = Factura.Cliente, Total = Factura.Total, TotalPagado = Factura.TotalPagado, UltimoAbono = Factura.UltimoAbono, SimboloMoneda = Factura.SimboloMoneda });
            return Resultado;
        }

        public List<ClsVendedorComisionVista> ComisionVendedoresMetObtenerVendedores()
        {
            List<ClsVendedorComisionVista> Resultado = new List<ClsVendedorComisionVista>();
            foreach (ClsVendedorComision Vendedor in _RepositorioComision.ComisionVendedoresMetObtenerVendedores())
                Resultado.Add(new ClsVendedorComisionVista { IdVendedor = Vendedor.IdVendedor, NombreVendedor = Vendedor.NombreVendedor, Porcentaje = Vendedor.Porcentaje });
            return Resultado;
        }

        public List<ClsComisionFacturaDetalle> ComisionVendedoresMetCalcularFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo)
        {
            if (Metodo == EnumMetodoComision.Vendedor)
                throw new ArgumentException("La comisión se calcula por Línea, Producto, Categoría o Marca.");
            List<ClsDetalleProductoComision> Iniciales = _RepositorioComision.ComisionVendedoresMetObtenerDetallesFactura(IdFactura, Metodo.ToString());
            List<ClsComisionFacturaDetalle> Selecciones = Iniciales.Select(Detalle => new ClsComisionFacturaDetalle
            {
                IdFacturaDetalle = Detalle.IdFacturaDetalle,
                MetodoComision = Detalle.ComisionGuardada ? Detalle.MetodoGuardado : ComisionVendedoresMetObtenerMetodoMenor(Detalle),
                PorcentajeProducto = Detalle.PorcentajeProducto,
                PorcentajeMarca = Detalle.PorcentajeMarca,
                PorcentajeLinea = Detalle.PorcentajeLinea,
                PorcentajeCategoria = Detalle.PorcentajeCategoria
            }).ToList();
            List<ClsComisionFacturaDetalle> Resultado = ComisionVendedoresMetCalcularFactura(IdFactura, IdVendedor, Selecciones);
            return Resultado;
        }

        private static string ComisionVendedoresMetObtenerMetodoMenor(ClsDetalleProductoComision Detalle)
        {
            return new[] {
                new { Metodo = "Producto", Tasa = Detalle.PorcentajeProducto },
                new { Metodo = "Marca", Tasa = Detalle.PorcentajeMarca },
                new { Metodo = "Linea", Tasa = Detalle.PorcentajeLinea },
                new { Metodo = "Categoria", Tasa = Detalle.PorcentajeCategoria }
            }.OrderBy(Opcion => Opcion.Tasa).First().Metodo;
        }

        public List<ClsComisionFacturaDetalle> ComisionVendedoresMetCalcularFactura(int IdFactura, int IdVendedor, List<ClsComisionFacturaDetalle> Selecciones)
        {
            ClsFacturaComision Factura = _RepositorioComision.ComisionVendedoresMetObtenerFactura(IdFactura);
            ClsVendedorComision Vendedor = _RepositorioComision.ComisionVendedoresMetObtenerVendedor(IdVendedor);
            if (Factura == null) throw new ArgumentException("La factura seleccionada no existe.");
            if (Vendedor == null) throw new ArgumentException("El vendedor seleccionado no existe.");
            _RepositorioComision.ComisionVendedoresMetSincronizarAbonos(IdFactura);
            if (Selecciones == null || Selecciones.Count == 0)
                throw new InvalidOperationException("La factura no tiene productos para calcular comisión.");
            decimal ProporcionPagada = Factura.Total <= 0 ? 0 : Math.Min(1m, Math.Max(0m, Factura.TotalPagado / Factura.Total));
            decimal PagoPrevioAlUltimo = Math.Max(0m, Factura.TotalPagado - Factura.UltimoAbono);
            decimal UltimoAbonoAplicable = Math.Min(Factura.UltimoAbono, Math.Max(0m, Factura.Total - PagoPrevioAlUltimo));
            decimal ProporcionUltimoAbono = Factura.Total <= 0 ? 0 : UltimoAbonoAplicable / Factura.Total;
            List<ClsComisionFacturaDetalle> Resultado = new List<ClsComisionFacturaDetalle>();
            foreach (ClsComisionFacturaDetalle Seleccion in Selecciones)
            {
                EnumMetodoComision Metodo;
                if (!Enum.TryParse(Seleccion.MetodoComision, true, out Metodo) || Metodo == EnumMetodoComision.Vendedor)
                    throw new InvalidOperationException("Elija Línea, Marca, Categoría o Producto para cada producto.");
                ClsDetalleProductoComision Detalle = _RepositorioComision.ComisionVendedoresMetObtenerDetallesFactura(IdFactura, Metodo.ToString())
                    .FirstOrDefault(Item => Item.IdFacturaDetalle == Seleccion.IdFacturaDetalle);
                if (Detalle == null) throw new InvalidOperationException("No se encontró un producto de la factura.");
                if (Detalle.IdProductoLogistica <= 0 || Detalle.IdClasificacionLogistica <= 0)
                    throw new InvalidOperationException("El producto " + Detalle.NombreProducto + " no tiene relación o tasa válida en Logística.");
                if (Detalle.ComisionGuardada && !string.Equals(Detalle.MetodoGuardado, Metodo.ToString(), StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Esta factura ya tiene una comisión guardada con otro método; el cálculo histórico no se puede cambiar.");
                if (Detalle.ComisionGuardada && Detalle.IdVendedorGuardado != IdVendedor)
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

        public int ComisionVendedoresMetGuardarComisionFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo)
        {
            if (IdFactura <= 0 || IdVendedor <= 0) throw new ArgumentOutOfRangeException("Factura y vendedor deben ser válidos.");
            if (Metodo == EnumMetodoComision.Vendedor) throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            return _RepositorioComision.ComisionVendedoresMetGuardarComisionFactura(IdFactura, IdVendedor, Metodo.ToString());
        }

        public int ComisionVendedoresMetAsignarVendedorFactura(int IdFactura, int IdVendedor)
        {
            if (IdFactura <= 0 || IdVendedor <= 0) throw new ArgumentOutOfRangeException("Factura y vendedor deben ser válidos.");
            return _RepositorioComision.ComisionVendedoresMetAsignarVendedorFactura(IdFactura, IdVendedor);
        }

        public int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, EnumMetodoComision Metodo)
        {
            if (IdFactura <= 0 || IdVendedor <= 0) throw new ArgumentOutOfRangeException("Factura y vendedor deben ser válidos.");
            if (Metodo == EnumMetodoComision.Vendedor) throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            return _RepositorioComision.ComisionVendedoresMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Metodo.ToString());
        }

        public int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsComisionFacturaDetalle> Selecciones)
        {
            if (IdFactura <= 0 || IdVendedor <= 0) throw new ArgumentOutOfRangeException("Factura y vendedor deben ser válidos.");
            if (Selecciones == null || Selecciones.Count == 0) throw new ArgumentException("Elija el método para cada producto antes de asignar.");
            List<ClsDetalleProductoComision> Datos = Selecciones.Select(Seleccion => new ClsDetalleProductoComision
            {
                IdFacturaDetalle = Seleccion.IdFacturaDetalle,
                MetodoSeleccionado = Seleccion.MetodoComision
            }).ToList();
            return _RepositorioComision.ComisionVendedoresMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Datos);
        }

        public List<ClsFacturaPagoVista> ComisionVendedoresMetObtenerFacturasConSaldo()
        {
            return _RepositorioComision.ComisionVendedoresMetObtenerFacturasConSaldo()
                .Select(Factura => new ClsFacturaPagoVista
                {
                    IdFactura = Factura.IdFactura,
                    Fecha = Factura.Fecha,
                    Etiqueta = Factura.Etiqueta,
                    SimboloMoneda = Factura.SimboloMoneda,
                    Total = Factura.Total,
                    TotalPagado = Factura.TotalPagado,
                    Saldo = Factura.Saldo,
                    ComisionTotal = Factura.ComisionTotal,
                    ComisionDevengada = Factura.ComisionDevengada
                }).ToList();
        }

        public List<ClsFormaPagoVentaVista> ComisionVendedoresMetObtenerFormasPagoVenta()
        {
            return _RepositorioComision.ComisionVendedoresMetObtenerFormasPagoVenta()
                .Select(Forma => new ClsFormaPagoVentaVista
                {
                    IdFormaPago = Forma.IdFormaPago,
                    Nombre = Forma.Nombre,
                    RequiereReferencia = Forma.RequiereReferencia
                }).ToList();
        }

        public decimal ComisionVendedoresMetRegistrarAbonoFactura(int IdFactura, decimal Monto,
            int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario)
        {
            if (IdFactura <= 0) throw new ArgumentOutOfRangeException("IdFactura", "Seleccione una factura válida.");
            if (Monto <= 0m || Monto != Math.Round(Monto, 2, MidpointRounding.AwayFromZero))
                throw new ArgumentOutOfRangeException("Monto", "El monto del abono debe ser positivo y tener hasta dos decimales.");
            if (IdFormaPago <= 0) throw new ArgumentOutOfRangeException("IdFormaPago", "Seleccione un método de pago.");
            if (IdUsuario <= 0) throw new ArgumentOutOfRangeException("IdUsuario", "No se encontró un usuario activo en la sesión.");
            return _RepositorioComision.ComisionVendedoresMetRegistrarAbonoFactura(
                IdFactura, Monto, IdFormaPago, Fecha.Date, Referencia, IdUsuario);
        }

        private string ComisionVendedoresMetObtenerElementoComision(ClsDetalleProductoComision Detalle, EnumMetodoComision Metodo, string NombreVendedor)
        {
            switch (Metodo)
            {
                case EnumMetodoComision.Marca: return "Marca: " + Detalle.Marca;
                case EnumMetodoComision.Linea: return "Línea: " + Detalle.Linea;
                case EnumMetodoComision.Producto: return "Producto: " + Detalle.NombreProducto;
                case EnumMetodoComision.Categoria: return "Categoría: " + Detalle.Categoria;
                default: throw new ArgumentException("Método de comisión no válido.");
            }
        }
    }
}
