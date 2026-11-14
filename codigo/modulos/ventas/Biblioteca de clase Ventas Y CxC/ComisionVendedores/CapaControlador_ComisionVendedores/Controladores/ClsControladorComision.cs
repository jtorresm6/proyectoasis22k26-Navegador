// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Agrupa los movimientos del período y coordina su liquidación.

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
    public partial class ClsControladorComision : IControladorComision
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

        public List<ClsComisionVendedorResultado> VentasMetCalcular(
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo,
            DateTime FechaInicio,
            DateTime FechaFin)
        {
            if (FechaInicio.Date > FechaFin.Date)
                throw new ArgumentException("La fecha inicial debe ser anterior o igual a la fecha final.");

            if (BaseCalculo != EnumBaseComision.Cobrado)
                throw new NotSupportedException("Las comisiones se devengan sobre cobros confirmados.");

            List<ClsVentaVendedorBase> Ventas = _RepositorioComision.VentasMetObtenerVentasPorMetodo(
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

        public int VentasMetGuardar(
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
            return _RepositorioComision.VentasMetGuardarComisiones(Comisiones, FechaInicio.Date, FechaFin.Date, Metodo.ToString());
        }

        public List<ClsProductoPorcentaje> VentasMetConsultarPorcentajes(EnumMetodoComision Metodo)
        {
            VentasMetValidarMetodoMantenimiento(Metodo);
            List<ClsProductoPorcentaje> Resultado = new List<ClsProductoPorcentaje>();
            foreach (ClsProductoComision Producto in _RepositorioComision.VentasMetConsultarPorcentajes(Metodo.ToString()))
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

        public int VentasMetActualizarPorcentaje(EnumMetodoComision Metodo, int IdClasificacion, decimal Porcentaje)
        {
            VentasMetValidarMetodoMantenimiento(Metodo);
            if (IdClasificacion <= 0) throw new ArgumentException("Seleccione el elemento específico que desea actualizar.");
            if (Porcentaje < 0 || Porcentaje > 99m) throw new ArgumentOutOfRangeException("Porcentaje", "El porcentaje debe estar entre 0.00 y 99.00% según el esquema de la base de datos.");
            return _RepositorioComision.VentasMetActualizarPorcentaje(Metodo.ToString(), IdClasificacion, Porcentaje);
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

        private string VentasMetObtenerElementoComision(ClsDetalleProductoComision Detalle, EnumMetodoComision Metodo, string NombreVendedor)
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
