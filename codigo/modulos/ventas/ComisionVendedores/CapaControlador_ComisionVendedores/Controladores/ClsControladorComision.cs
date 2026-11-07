using System;
using System.Collections.Generic;
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

        public List<ClsComisionVendedorResultado> VentasFuncCalcular(
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo,
            DateTime FechaInicio,
            DateTime FechaFin)
        {
            if (FechaInicio.Date > FechaFin.Date)
                throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");

            if (BaseCalculo != EnumBaseComision.Ventas)
                throw new NotSupportedException("La base COBRADO está reservada para la siguiente etapa.");

            List<ClsVentaVendedorBase> Ventas = _RepositorioComision.VentasFuncObtenerVentasPorMetodo(
                Metodo.ToString(),
                FechaInicio.Date,
                FechaFin.Date.AddDays(1));
            List<ClsComisionVendedorResultado> Resultados = new List<ClsComisionVendedorResultado>();

            foreach (ClsVentaVendedorBase Venta in Ventas)
            {
                Resultados.Add(new ClsComisionVendedorResultado
                {
                    IdVendedor = Venta.IdVendedor,
                    NombreVendedor = Venta.NombreVendedor,
                    ElementoComision = Venta.ElementoComision,
                    Porcentaje = Venta.PorcentajeVendedor,
                    BaseVentas = Venta.BaseVentas,
                    MontoComision = Math.Round(Venta.BaseVentas * Venta.PorcentajeVendedor / 100m, 2, MidpointRounding.AwayFromZero)
                });
            }

            return Resultados;
        }

        public int VentasFuncGuardar(
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

            if (BaseCalculo != EnumBaseComision.Ventas)
                throw new NotSupportedException("La base COBRADO está reservada para la siguiente etapa.");

            List<ClsComisionCalculada> Comisiones = new List<ClsComisionCalculada>();
            foreach (System.Linq.IGrouping<int, ClsComisionVendedorResultado> Grupo in System.Linq.Enumerable.GroupBy(Resultados, Resultado => Resultado.IdVendedor))
            {
                Comisiones.Add(new ClsComisionCalculada
                {
                    IdVendedor = Grupo.Key,
                    FechaInicio = FechaInicio.Date,
                    FechaFin = FechaFin.Date,
                    Metodo = Metodo.ToString(),
                    BaseCalculo = BaseCalculo.ToString().ToUpperInvariant(),
                    Monto = System.Linq.Enumerable.Sum(Grupo, Resultado => Resultado.MontoComision)
                });
            }

            return _RepositorioComision.VentasFuncGuardarComisiones(Comisiones);
        }

        public List<ClsProductoPorcentaje> VentasFuncConsultarPorcentajes(EnumMetodoComision Metodo)
        {
            VentasMetValidarMetodoMantenimiento(Metodo);
            List<ClsProductoPorcentaje> Resultado = new List<ClsProductoPorcentaje>();
            foreach (ClsProductoComision Producto in _RepositorioComision.VentasFuncConsultarPorcentajes(Metodo.ToString()))
                Resultado.Add(new ClsProductoPorcentaje { IdProducto = Producto.IdProducto, NombreProducto = Producto.NombreProducto, Marca = Producto.Marca, Linea = Producto.Linea, Categoria = Producto.Categoria, Porcentaje = Producto.Porcentaje });
            return Resultado;
        }

        public int VentasFuncActualizarPorcentaje(EnumMetodoComision Metodo, List<int> IdsProducto, decimal Porcentaje)
        {
            VentasMetValidarMetodoMantenimiento(Metodo);
            if (IdsProducto == null || IdsProducto.Count == 0) throw new ArgumentException("Seleccione al menos un producto para actualizar.");
            if (Porcentaje < 0 || Porcentaje > 100) throw new ArgumentOutOfRangeException("Porcentaje", "El porcentaje debe estar entre 0 y 100.");
            return _RepositorioComision.VentasFuncActualizarPorcentaje(Metodo.ToString(), IdsProducto, Porcentaje);
        }

        private void VentasMetValidarMetodoMantenimiento(EnumMetodoComision Metodo)
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

        public List<ClsFacturaComisionVista> VentasFuncObtenerFacturas()
        {
            List<ClsFacturaComisionVista> Resultado = new List<ClsFacturaComisionVista>();
            foreach (ClsFacturaComision Factura in _RepositorioComision.VentasFuncObtenerFacturas())
                Resultado.Add(new ClsFacturaComisionVista { IdFactura = Factura.IdFactura, Fecha = Factura.Fecha, Cliente = Factura.Cliente, Total = Factura.Total, TotalPagado = Factura.TotalPagado, UltimoAbono = Factura.UltimoAbono });
            return Resultado;
        }

        public List<ClsVendedorComisionVista> VentasFuncObtenerVendedores()
        {
            List<ClsVendedorComisionVista> Resultado = new List<ClsVendedorComisionVista>();
            foreach (ClsVendedorComision Vendedor in _RepositorioComision.VentasFuncObtenerVendedores())
                Resultado.Add(new ClsVendedorComisionVista { IdVendedor = Vendedor.IdVendedor, NombreVendedor = Vendedor.NombreVendedor, Porcentaje = Vendedor.Porcentaje });
            return Resultado;
        }

        public List<ClsComisionFacturaDetalle> VentasFuncCalcularFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo)
        {
            ClsFacturaComision Factura = _RepositorioComision.VentasFuncObtenerFactura(IdFactura);
            if (Factura == null) throw new ArgumentException("La factura seleccionada no existe.");
            ClsVendedorComision Vendedor = _RepositorioComision.VentasFuncObtenerVendedor(IdVendedor, Factura.Fecha);
            if (Vendedor == null) throw new ArgumentException("El vendedor seleccionado no existe.");

            decimal ProporcionPagada = Factura.Total <= 0 ? 0 : Math.Min(1m, Math.Max(0m, Factura.TotalPagado / Factura.Total));
            decimal PagoPrevioAlUltimo = Math.Max(0m, Factura.TotalPagado - Factura.UltimoAbono);
            decimal UltimoAbonoAplicable = Math.Min(Factura.UltimoAbono, Math.Max(0m, Factura.Total - PagoPrevioAlUltimo));
            decimal ProporcionUltimoAbono = Factura.Total <= 0 ? 0 : UltimoAbonoAplicable / Factura.Total;
            List<ClsComisionFacturaDetalle> Resultado = new List<ClsComisionFacturaDetalle>();
            foreach (ClsDetalleProductoComision Detalle in _RepositorioComision.VentasFuncObtenerDetallesFactura(IdFactura, Metodo.ToString()))
            {
                decimal BaseVenta = Detalle.Cantidad * Detalle.PrecioUnitario - Detalle.Descuento;
                decimal Porcentaje = Metodo == EnumMetodoComision.Vendedor ? Vendedor.Porcentaje : Detalle.Porcentaje;
                decimal ComisionTotal = Math.Round(BaseVenta * Porcentaje / 100m, 2, MidpointRounding.AwayFromZero);
                Resultado.Add(new ClsComisionFacturaDetalle
                {
                    NombreProducto = Detalle.NombreProducto,
                    ElementoComision = VentasFuncObtenerElementoComision(Detalle, Metodo),
                    Cantidad = Detalle.Cantidad,
                    BaseVenta = BaseVenta,
                    Porcentaje = Porcentaje,
                    ComisionTotal = ComisionTotal,
                    VentaCobrado = Math.Round(BaseVenta * ProporcionPagada, 2, MidpointRounding.AwayFromZero),
                    ComisionUltimoAbono = Math.Round(ComisionTotal * ProporcionUltimoAbono, 2, MidpointRounding.AwayFromZero),
                    ComisionDisponible = Math.Round(ComisionTotal * ProporcionPagada, 2, MidpointRounding.AwayFromZero),
                    ComisionPendiente = Math.Round(ComisionTotal * (1m - ProporcionPagada), 2, MidpointRounding.AwayFromZero)
                });
            }
            return Resultado;
        }

        private string VentasFuncObtenerElementoComision(ClsDetalleProductoComision Detalle, EnumMetodoComision Metodo)
        {
            switch (Metodo)
            {
                case EnumMetodoComision.Vendedor: return "Vendedor";
                case EnumMetodoComision.Marca: return Detalle.Marca;
                case EnumMetodoComision.Linea: return Detalle.Linea;
                case EnumMetodoComision.Producto: return Detalle.NombreProducto;
                case EnumMetodoComision.Categoria: return Detalle.Categoria;
                default: throw new ArgumentException("Método de comisión no válido.");
            }
        }
    }
}
