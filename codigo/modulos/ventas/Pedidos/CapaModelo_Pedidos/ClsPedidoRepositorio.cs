using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;

namespace CapaModelo_Pedidos
{
    public sealed class ClsPedidoRepositorio : ClsConexion
    {
        private OdbcConnection VentasFuncAbrir()
        {
            var Conexion = VentasFuncObtenerConexion();
            Conexion.Open();
            return Conexion;
        }

        private static OdbcCommand VentasFuncComando(OdbcConnection Conexion, OdbcTransaction Transaccion, string SentenciaSql, params object[] Valores)
        {
            var Comando = new OdbcCommand(SentenciaSql, Conexion, Transaccion);
            foreach (object Valor in Valores)
                Comando.Parameters.AddWithValue("?", Valor is decimal
                    ? ((decimal)Valor).ToString(CultureInfo.InvariantCulture)
                    : (Valor ?? DBNull.Value));
            return Comando;
        }

        private List<T> VentasFuncConsultar<T>(string SentenciaSql, Func<OdbcDataReader, T> Leer, params object[] Valores)
        {
            var Lista = new List<T>();
            using (var Conexion = VentasFuncAbrir())
            using (var Comando = VentasFuncComando(Conexion, null, SentenciaSql, Valores))
            using (var Lector = Comando.ExecuteReader())
                while (Lector.Read()) Lista.Add(Leer(Lector));
            return Lista;
        }

        public List<ClsCatalogoPedido> VentasFuncClientes()
        {
            return VentasFuncCatalogo("SELECT Pk_Cliente, CONCAT(codigoCliente, ' · ', nombreCliente) FROM tblCliente WHERE activoCliente=1 ORDER BY nombreCliente");
        }

        public List<ClsCatalogoPedido> VentasFuncVendedores()
        {
            return VentasFuncCatalogo("SELECT Pk_Vendedor, codigoVendedor FROM tblVendedor WHERE activoVendedor=1 ORDER BY codigoVendedor");
        }

        public List<ClsCotizacionPedido> VentasFuncCotizaciones(DateTime Fecha)
        {
            const string Sql = @"SELECT c.Pk_CotizacionEncabezado,c.numeroCotizacionEncabezado,c.Fk_Cliente_Codigo,c.Fk_Vendedor_Codigo,c.Fk_Moneda_Codigo,c.vigenteHastaCotizacionEncabezado,
                d.Pk_CotizacionDetalle,d.Fk_Producto_Codigo,p.skuProducto,d.descripcionItemCotizacionDetalle,pv.tipoItemProductoVenta,
                d.cantidadCotizacionDetalle,d.precioUnitarioCotizacionDetalle,d.descuentoMontoCotizacionDetalle,d.tasaImpuestoCotizacionDetalle
                FROM tblCotizacionEncabezado c JOIN tblCotizacionDetalle d ON d.Fk_CotizacionEncabezado_Codigo=c.Pk_CotizacionEncabezado
                JOIN tblProducto p ON p.Pk_Producto=d.Fk_Producto_Codigo JOIN tblProductoVenta pv ON pv.Pk_ProductoVenta=p.Pk_Producto
                WHERE c.estadoCotizacionEncabezado IN ('EMITIDA','ACEPTADA') AND c.vigenteHastaCotizacionEncabezado>=?
                  AND NOT EXISTS(SELECT 1 FROM tblPedidoEncabezado pe WHERE pe.Fk_CotizacionEncabezado_Codigo=c.Pk_CotizacionEncabezado)
                  AND NOT EXISTS(SELECT 1 FROM tblFacturaEncabezado fe WHERE fe.Fk_CotizacionEncabezado_Codigo=c.Pk_CotizacionEncabezado)
                ORDER BY c.numeroCotizacionEncabezado,d.numeroLineaCotizacionDetalle";
            var Resultado = new List<ClsCotizacionPedido>();
            using (var Conexion = VentasFuncAbrir())
            using (var Comando = VentasFuncComando(Conexion, null, Sql, Fecha.Date))
            using (var Lector = Comando.ExecuteReader())
            {
                ClsCotizacionPedido Cotizacion = null;
                while (Lector.Read())
                {
                    int Id = Convert.ToInt32(Lector[0]);
                    if (Cotizacion == null || Cotizacion.Id != Id)
                    {
                        Cotizacion = new ClsCotizacionPedido {
                            Id = Id, Numero = Convert.ToString(Lector[1]), Nombre = Convert.ToString(Lector[1]),
                            ClienteId = Convert.ToInt32(Lector[2]), VendedorId = Convert.ToInt32(Lector[3]),
                            MonedaId = Convert.ToInt32(Lector[4]), Vence = Convert.ToDateTime(Lector[5])
                        };
                        Resultado.Add(Cotizacion);
                    }
                    Cotizacion.Lineas.Add(new ClsLineaPedido {
                        CotizacionDetalleId = Convert.ToInt32(Lector[6]), ProductoId = Convert.ToInt32(Lector[7]),
                        Codigo = Convert.ToString(Lector[8]), Descripcion = Convert.ToString(Lector[9]),
                        Tipo = Convert.ToString(Lector[10]), Cantidad = Convert.ToDecimal(Lector[11]),
                        Precio = Convert.ToDecimal(Lector[12]), Descuento = Convert.ToDecimal(Lector[13]),
                        PorcentajeImpuesto = Convert.ToDecimal(Lector[14]) * 100m
                    });
                }
            }
            return Resultado;
        }

        private List<ClsCatalogoPedido> VentasFuncCatalogo(string SentenciaSql)
        {
            return VentasFuncConsultar(SentenciaSql, Lector => new ClsCatalogoPedido { Id = Lector.GetInt32(0), Nombre = Lector.GetString(1) });
        }

        public List<ClsLineaPedido> VentasFuncProductosPublicos(DateTime Fecha)
        {
            return VentasFuncConsultar(
                "SELECT p.Pk_ProductoVenta,pr.skuProducto,pr.nombreProducto,p.tipoItemProductoVenta,p.tasaImpuestoProductoVenta*100,d.precioUnitarioListaPrecioDetalle " +
                "FROM tblProductoVenta p JOIN tblProducto pr ON pr.Pk_Producto=p.Pk_ProductoVenta " +
                "JOIN tblListaPrecioDetalle d ON d.Fk_Producto_Codigo=p.Pk_ProductoVenta AND d.activoListaPrecioDetalle=1 " +
                "JOIN tblListaPrecioEncabezado l ON l.Pk_ListaPrecioEncabezado=d.Fk_ListaPrecioEncabezado_Codigo AND l.activoListaPrecioEncabezado=1 " +
                "WHERE p.activoProductoVenta=1 AND UPPER(l.codigoListaPrecioEncabezado)='PUBLICO' AND l.vigenteDesdeListaPrecioEncabezado<=? " +
                "AND (l.vigenteHastaListaPrecioEncabezado IS NULL OR l.vigenteHastaListaPrecioEncabezado>=?) ORDER BY pr.nombreProducto",
                Lector => new ClsLineaPedido {
                    ProductoId = Lector.GetInt32(0), Codigo = Lector.GetString(1), Descripcion = Lector.GetString(2),
                    Tipo = Lector.GetString(3), PorcentajeImpuesto = Lector.GetDecimal(4), Precio = Lector.GetDecimal(5)
                }, Fecha.Date, Fecha.Date);
        }

        public int VentasFuncMonedaPublica()
        {
            using (var Conexion = VentasFuncAbrir())
            using (var Comando = VentasFuncComando(Conexion, null,
                "SELECT Fk_Moneda_Codigo FROM tblListaPrecioEncabezado WHERE UPPER(codigoListaPrecioEncabezado)='PUBLICO' AND activoListaPrecioEncabezado=1 LIMIT 1"))
            {
                object Valor = Comando.ExecuteScalar();
                return Valor == null || Valor == DBNull.Value ? 0 : Convert.ToInt32(Valor);
            }
        }

        public int VentasFuncGuardar(ClsPedido Pedido)
        {
            using (var Conexion = VentasFuncAbrir())
            using (var Transaccion = Conexion.BeginTransaction())
            {
                try
                {
                    using (var Comando = VentasFuncComando(Conexion, Transaccion,
                        "INSERT INTO tblPedidoEncabezado (numeroPedidoEncabezado,Fk_CotizacionEncabezado_Codigo,referenciaIndependientePedidoEncabezado,Fk_Cliente_Codigo,Fk_Vendedor_Codigo,Fk_Moneda_Codigo,fechaPedidoEncabezado,fechaEntregaPedidoEncabezado,fechaVencimientoPedidoEncabezado,estadoPedidoEncabezado) " +
                        "VALUES (?,?,?,?,?,?,?,?,?,'PENDIENTE')",
                        Pedido.Numero, Pedido.CotizacionId, Pedido.Referencia, Pedido.ClienteId, Pedido.VendedorId,
                        Pedido.MonedaId, Pedido.Fecha.Date, Pedido.Entrega.Date, Pedido.Vencimiento.Date))
                        Comando.ExecuteNonQuery();
                    int Id;
                    using (var Comando = VentasFuncComando(Conexion, Transaccion, "SELECT LAST_INSERT_ID()")) Id = Convert.ToInt32(Comando.ExecuteScalar());
                    for (int Indice = 0; Indice < Pedido.Lineas.Count; Indice++)
                    {
                        ClsLineaPedido Linea = Pedido.Lineas[Indice];
                        using (var Comando = VentasFuncComando(Conexion, Transaccion,
                            "INSERT INTO tblPedidoDetalle (Fk_PedidoEncabezado_Codigo,Fk_CotizacionDetalle_Codigo,numeroLineaPedidoDetalle,Fk_Producto_Codigo,descripcionItemPedidoDetalle,cantidadPedidoDetalle,precioUnitarioPedidoDetalle,descuentoMontoPedidoDetalle,tasaImpuestoPedidoDetalle) " +
                            "VALUES (?,?,?,?,?,?,?,?,?)", Id, Linea.CotizacionDetalleId, Indice + 1, Linea.ProductoId,
                            Linea.Descripcion, Linea.Cantidad, Linea.Precio, Linea.Descuento, Linea.PorcentajeImpuesto / 100m))
                            Comando.ExecuteNonQuery();
                    }
                    Transaccion.Commit();
                    return Id;
                }
                catch { try { Transaccion.Rollback(); } catch { } throw; }
            }
        }

    }
}
