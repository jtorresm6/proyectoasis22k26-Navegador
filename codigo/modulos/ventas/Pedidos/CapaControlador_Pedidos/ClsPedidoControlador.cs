using System;
using System.Collections.Generic;
using System.Linq;
using Modelo = CapaModelo_Pedidos;

namespace CapaControlador_Pedidos
{
    public sealed class ClsPedidoControlador
    {
        private readonly Modelo.ClsPedidoRepositorio _Repositorio;

        public ClsPedidoControlador()
        {
            _Repositorio = new Modelo.ClsPedidoRepositorio();
        }

        public List<ClsCatalogoPedido> VentasFuncClientes()
        {
            return VentasFuncConvertirCatalogo(_Repositorio.VentasFuncClientes());
        }

        public List<ClsCatalogoPedido> VentasFuncVendedores()
        {
            return VentasFuncConvertirCatalogo(_Repositorio.VentasFuncVendedores());
        }

        private static List<ClsCatalogoPedido> VentasFuncConvertirCatalogo(List<Modelo.ClsCatalogoPedido> Datos)
        {
            return Datos.Select(Dato => new ClsCatalogoPedido { Id = Dato.Id, Nombre = Dato.Nombre }).ToList();
        }

        public List<ClsCotizacionPedido> VentasFuncCotizaciones()
        {
            return _Repositorio.VentasFuncCotizaciones(DateTime.Today).Select(Dato => new ClsCotizacionPedido {
                Id = Dato.Id, Nombre = Dato.Nombre, Numero = Dato.Numero,
                ClienteId = Dato.ClienteId, VendedorId = Dato.VendedorId, MonedaId = Dato.MonedaId,
                Vence = Dato.Vence, Lineas = Dato.Lineas.Select(VentasFuncConvertirLinea).ToList()
            }).ToList();
        }

        public List<ClsLineaPedido> VentasFuncProductos(DateTime Fecha)
        {
            return _Repositorio.VentasFuncProductosPublicos(Fecha)
                .Select(VentasFuncConvertirLinea).ToList();
        }

        private static ClsLineaPedido VentasFuncConvertirLinea(Modelo.ClsLineaPedido Dato)
        {
            return new ClsLineaPedido {
                ProductoId = Dato.ProductoId, CotizacionDetalleId = Dato.CotizacionDetalleId,
                Codigo = Dato.Codigo, Descripcion = Dato.Descripcion, Tipo = Dato.Tipo,
                Cantidad = Dato.Cantidad, Precio = Dato.Precio, Descuento = Dato.Descuento,
                PorcentajeImpuesto = Dato.PorcentajeImpuesto
            };
        }

        public int VentasFuncMonedaPublica() { return _Repositorio.VentasFuncMonedaPublica(); }

        public static void VentasMetCalcular(ClsLineaPedido Linea)
        {
            decimal Base = Math.Round(Linea.Cantidad * Linea.Precio, 2, MidpointRounding.AwayFromZero);
            Linea.Impuesto = Math.Round((Base - Linea.Descuento) * Linea.PorcentajeImpuesto / 100m, 2, MidpointRounding.AwayFromZero);
            Linea.Total = Base - Linea.Descuento + Linea.Impuesto;
        }

        public int VentasFuncGuardar(ClsPedido Pedido)
        {
            if (Pedido == null || string.IsNullOrWhiteSpace(Pedido.Numero) || Pedido.Numero.Trim().Length > 30)
                throw new ArgumentException("Indique un número de pedido de hasta 30 caracteres.");
            if (Pedido.ClienteId <= 0 || Pedido.VendedorId <= 0 || Pedido.MonedaId <= 0)
                throw new ArgumentException("Seleccione cliente, vendedor y moneda válidos.");
            if (Pedido.CotizacionId.HasValue == !string.IsNullOrWhiteSpace(Pedido.Referencia))
                throw new ArgumentException("Seleccione una cotización o escriba una referencia independiente.");
            if (Pedido.Lineas == null || Pedido.Lineas.Count == 0)
                throw new ArgumentException("Agregue al menos un producto al pedido.");
            if (Pedido.Vencimiento.Date < Pedido.Fecha.Date || Pedido.Entrega.Date < Pedido.Fecha.Date)
                throw new ArgumentException("Revise las fechas de entrega y vencimiento.");
            foreach (ClsLineaPedido Linea in Pedido.Lineas)
                if (Linea.ProductoId <= 0 || Linea.Cantidad <= 0 || Linea.Precio < 0 ||
                    Linea.Descuento < 0 || Linea.Descuento > Linea.Cantidad * Linea.Precio ||
                    Linea.PorcentajeImpuesto < 0 || Linea.PorcentajeImpuesto > 99)
                    throw new ArgumentException("Una línea del pedido tiene cantidad, precio, descuento o impuesto inválido.");
            var Registro = new Modelo.ClsPedido {
                Numero = Pedido.Numero.Trim(), CotizacionId = Pedido.CotizacionId,
                Referencia = Pedido.Referencia, ClienteId = Pedido.ClienteId,
                VendedorId = Pedido.VendedorId, MonedaId = Pedido.MonedaId,
                Fecha = Pedido.Fecha.Date,
                Entrega = Pedido.Entrega.Date, Vencimiento = Pedido.Vencimiento.Date,
                TipoCambio = Pedido.TipoCambio
            };
            foreach (ClsLineaPedido Linea in Pedido.Lineas)
            {
                VentasMetCalcular(Linea);
                Registro.Lineas.Add(new Modelo.ClsLineaPedido {
                    ProductoId = Linea.ProductoId, CotizacionDetalleId = Linea.CotizacionDetalleId,
                    Descripcion = Linea.Descripcion, Tipo = Linea.Tipo,
                    Cantidad = Linea.Cantidad, Precio = Linea.Precio, Descuento = Linea.Descuento,
                    PorcentajeImpuesto = Linea.PorcentajeImpuesto, Impuesto = Linea.Impuesto, Total = Linea.Total
                });
            }
            return _Repositorio.VentasFuncGuardar(Registro);
        }

    }
}
