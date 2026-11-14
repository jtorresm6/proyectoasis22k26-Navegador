// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Consulta y convierte las filas del ERP en datos de facturas, vendedores y comisiones.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using CapaModelo_ComisionVendedores_Ventas.Contratos;
using CapaModelo_ComisionVendedores_Ventas.Entidades;

namespace CapaModelo_ComisionVendedores_Ventas.Repositorios
{
    public partial class ClsRepositorioComision
    {
        public List<ClsFacturaComision> VentasMetObtenerFacturas()
        {
            const string Comando = @"
                SELECT f.idFacturaEncabezado AS id,
                       COALESCE(f.idVendedor,0) AS id_vendedor,
                       f.fechaEmisionFacturaEncabezado AS fecha,
                       COALESCE(cfv.nombreClienteFacturaVersion,'') AS cliente,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle)
                           FROM tblFacturaDetalle fd
                           WHERE fd.idFacturaEncabezadoUuidDte=f.uuidDteFacturaEncabezado),0) AS total,
                       0 AS total_pagado, 0 AS ultimo_abono
                FROM tblFacturaEncabezado f
                LEFT JOIN tblClienteFacturaVersion cfv ON cfv.idClienteFacturaVersion=f.idClienteFacturaVersion
                INNER JOIN tblMoneda m ON m.idMoneda=f.idMoneda
                WHERE COALESCE(f.idVendedor,0)=0 OR NOT EXISTS (
                    SELECT 1 FROM tblComisionFacturaEncabezado cf
                    WHERE cf.idFacturaEncabezado=f.idFacturaEncabezado)
                ORDER BY f.fechaEmisionFacturaEncabezado DESC, f.idFacturaEncabezado DESC";
            DataTable Datos = VentasMetEjecucionConsulta(Comando, new List<OdbcParameter>());
            if (Datos.Rows.Count == 0)
            {
                DataTable BaseDatos = VentasMetEjecucionConsulta(
                    @"SELECT DATABASE() AS base_actual, @@hostname AS servidor, @@port AS puerto,
                             CURRENT_USER() AS usuario_mysql,
                             (SELECT COUNT(*) FROM tblFacturaEncabezado) AS total_facturas",
                    new List<OdbcParameter>());
                string NombreBase = BaseDatos.Rows.Count == 0 ? "desconocida" : Convert.ToString(BaseDatos.Rows[0]["base_actual"]);
                string Servidor = BaseDatos.Rows.Count == 0 ? "desconocido" : Convert.ToString(BaseDatos.Rows[0]["servidor"]);
                string Puerto = BaseDatos.Rows.Count == 0 ? "desconocido" : Convert.ToString(BaseDatos.Rows[0]["puerto"]);
                string Usuario = BaseDatos.Rows.Count == 0 ? "desconocido" : Convert.ToString(BaseDatos.Rows[0]["usuario_mysql"]);
                int TotalFacturas = BaseDatos.Rows.Count == 0 ? 0 : Convert.ToInt32(BaseDatos.Rows[0]["total_facturas"]);
                string Detalle = TotalFacturas == 0
                    ? "no contiene facturas. Compare servidor y puerto con Workbench."
                    : "sí contiene " + TotalFacturas + " factura(s), pero ninguna está sin vendedor o sin comisión calculada. Revise el vendedor y la comisión de las facturas.";
                throw new InvalidOperationException("La conexión ODBC llegó a '" + Servidor + ":" + Puerto + "/" + NombreBase + "' como '" + Usuario + "'; " + Detalle);
            }
            return VentasMetMapearFacturas(Datos);
        }

        public List<ClsVendedorComision> VentasMetObtenerVendedores()
        {
            const string Comando = @"
                SELECT v.idVendedor AS id,
                       COALESCE(NULLIF(TRIM(CONCAT_WS(' ', e.nombresEmpleado, e.apellidosEmpleado)), ''), v.codigoVendedor) AS nombre,
                       0 AS porcentaje
                FROM tblVendedor v
                LEFT JOIN tblEmpleado e ON e.idEmpleado = v.idEmpleado
                WHERE v.activoVendedor = 1 ORDER BY nombre";
            DataTable Datos = VentasMetEjecucionConsulta(Comando, new List<OdbcParameter>());
            List<ClsVendedorComision> Resultado = new List<ClsVendedorComision>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsVendedorComision
                {
                    IdVendedor = Convert.ToInt32(Fila["id"]),
                    NombreVendedor = Convert.ToString(Fila["nombre"]),
                    Porcentaje = 0m
                });
            return Resultado;
        }

        public List<ClsFacturaPago> VentasMetObtenerFacturasConSaldo()
        {
            // Dejar al día los abonos de comisión antes de mostrar el estimado en el formulario.
            VentasMetSincronizarAbonos(null);
            const string Comando = @"
                SELECT f.idFacturaEncabezado AS id_factura,
                       COALESCE(cfv.idCliente,0) AS id_cliente,
                       COALESCE(f.idMoneda,0) AS id_moneda,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       f.fechaEmisionFacturaEncabezado AS fecha,
                       f.estadoFacturaEncabezado AS estado,
                       CONCAT('FAC-',f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado,' | ',
                              COALESCE(cfv.nombreClienteFacturaVersion,''),' | ',COALESCE(m.simboloMoneda,'')) AS etiqueta,
                       COALESCE(ft.total,0) AS total,
                       COALESCE(pa.pagado,0) AS pagado,
                       -- Si CxC tiene un saldo cero sin registrar pagos, recuperar el pendiente desde recibos confirmados.
                       GREATEST(COALESCE(NULLIF(mc.saldo,0),COALESCE(ft.total,0)-COALESCE(pa.pagado,0)),0) AS saldo,
                       COALESCE(cc.total_comision,0) AS total_comision,
                       COALESCE(ca.comision_devengada,0) AS comision_devengada,
                       COALESCE(ca.comision_pagada,0) AS comision_pagada,
                       COALESCE(ca.comision_pendiente_pago,0) AS comision_pendiente_pago
                FROM tblFacturaEncabezado f
                LEFT JOIN tblClienteFacturaVersion cfv ON cfv.idClienteFacturaVersion=f.idClienteFacturaVersion
                LEFT JOIN tblMoneda m ON m.idMoneda=f.idMoneda
                LEFT JOIN (SELECT f2.idFacturaEncabezado,SUM(fd.totalLineaFacturaDetalle) AS total
                           FROM tblFacturaEncabezado f2
                           INNER JOIN tblFacturaDetalle fd
                             ON fd.idFacturaEncabezadoUuidDte=f2.uuidDteFacturaEncabezado
                           GROUP BY f2.idFacturaEncabezado) ft
                       ON ft.idFacturaEncabezado=f.idFacturaEncabezado
                LEFT JOIN (SELECT ap.idFacturaEncabezado,SUM(ap.montoAplicadoAplicacionPago) AS pagado
                           FROM tblAplicacionPago ap INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                           WHERE ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'
                           GROUP BY ap.idFacturaEncabezado) pa
                       ON pa.idFacturaEncabezado=f.idFacturaEncabezado
                LEFT JOIN (SELECT idFacturaEncabezado,
                                  SUM(cargoMovimientoCxC-abonoMovimientoCxC) AS saldo
                           FROM tblMovimientoCxC GROUP BY idFacturaEncabezado) mc
                       ON mc.idFacturaEncabezado=f.idFacturaEncabezado
                LEFT JOIN (SELECT cf.idFacturaEncabezado,
                                  SUM(cd.baseComisionFacturaDetalle*cd.tasaAplicadaComisionFacturaDetalle) AS total_comision
                           FROM tblComisionFacturaEncabezado cf INNER JOIN tblComisionFacturaDetalle cd
                             ON cd.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                           WHERE cf.estadoComisionFacturaEncabezado='CALCULADA'
                           GROUP BY cf.idFacturaEncabezado) cc
                       ON cc.idFacturaEncabezado=f.idFacturaEncabezado
                LEFT JOIN (SELECT cf.idFacturaEncabezado,
                                  SUM(CASE WHEN ca.estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')
                                           THEN ca.montoComisionDevengadoComisionAbono ELSE 0 END) AS comision_devengada,
                                  SUM(CASE WHEN ca.estadoPagoVendedorComisionAbono='PAGADO'
                                           THEN ca.montoComisionDevengadoComisionAbono ELSE 0 END) AS comision_pagada,
                                  SUM(CASE WHEN ca.estadoPagoVendedorComisionAbono='PENDIENTE'
                                           THEN ca.montoComisionDevengadoComisionAbono ELSE 0 END) AS comision_pendiente_pago
                           FROM tblComisionFacturaEncabezado cf INNER JOIN tblComisionAbono ca
                             ON ca.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                           GROUP BY cf.idFacturaEncabezado) ca
                       ON ca.idFacturaEncabezado=f.idFacturaEncabezado
                ORDER BY f.fechaEmisionFacturaEncabezado,f.idFacturaEncabezado";
            DataTable Datos = VentasMetEjecucionConsulta(Comando, new List<OdbcParameter>());
            List<ClsFacturaPago> Resultado = new List<ClsFacturaPago>();
            foreach (DataRow Fila in Datos.Rows)
            {
                int IdFactura = Convert.ToInt32(Fila["id_factura"]);
                decimal Saldo = Convert.ToDecimal(Fila["saldo"]);
                string Estado = Convert.ToString(Fila["estado"]);
                string Simbolo = Convert.ToString(Fila["simbolo_moneda"]);
                decimal Total = Convert.ToDecimal(Fila["total"]);
                decimal Pagado = Convert.ToDecimal(Fila["pagado"]);
                Resultado.Add(new ClsFacturaPago
                {
                    IdFactura = IdFactura,
                    IdCliente = Convert.ToInt32(Fila["id_cliente"]),
                    IdMoneda = Convert.ToInt64(Fila["id_moneda"]),
                    Fecha = Convert.ToDateTime(Fila["fecha"]),
                    Estado = Estado,
                    Etiqueta = "Factura #" + IdFactura + " | " + Convert.ToString(Fila["etiqueta"]) +
                        " | " + Estado + " | Saldo " + Simbolo + " " + Saldo.ToString("N2", CultureInfo.GetCultureInfo("es-GT")),
                    SimboloMoneda = Simbolo,
                    Total = Total,
                    TotalPagado = Pagado,
                    Saldo = Saldo,
                    ComisionTotal = Convert.ToDecimal(Fila["total_comision"]),
                    ComisionDevengada = Convert.ToDecimal(Fila["comision_devengada"]),
                    ComisionPagada = Convert.ToDecimal(Fila["comision_pagada"]),
                    ComisionPendientePago = Convert.ToDecimal(Fila["comision_pendiente_pago"])
                });
            }
            return Resultado;
        }

        public List<ClsFormaPagoVenta> VentasMetObtenerFormasPagoVenta()
        {
            const string Comando = @"SELECT idFormaPagoVenta AS id,nombreFormaPagoVenta AS nombre,
                                           requiereReferenciaFormaPagoVenta AS requiere_referencia
                                    FROM tblFormaPagoVenta ORDER BY nombreFormaPagoVenta";
            DataTable Datos = VentasMetEjecucionConsulta(Comando, new List<OdbcParameter>());
            List<ClsFormaPagoVenta> Resultado = new List<ClsFormaPagoVenta>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsFormaPagoVenta
                {
                    IdFormaPago = Convert.ToInt32(Fila["id"]),
                    Nombre = Convert.ToString(Fila["nombre"]),
                    RequiereReferencia = Convert.ToBoolean(Fila["requiere_referencia"])
                });
            return Resultado;
        }

        public List<ClsComisionNomina> VentasMetObtenerComisionesNomina()
        {
            // Una fila por cada pago confirmado del cliente y su abono de comisión.
            VentasMetSincronizarAbonos(null);
            const string Comando = @"
                SELECT ca.idComisionAbono AS id_abono,
                       f.idFacturaEncabezado AS id_factura,
                       ap.fechaAplicacionPago AS fecha_abono,
                       COALESCE(NULLIF(TRIM(CONCAT_WS(' ',e.nombresEmpleado,e.apellidosEmpleado)),''),v.codigoVendedor,'') AS vendedor,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       COALESCE(det.total_comision,0) AS total_comision,
                       ap.montoAplicadoAplicacionPago AS monto_pago_cliente,
                       ca.montoComisionDevengadoComisionAbono AS comision_devengada_abono,
                       ca.estadoPagoVendedorComisionAbono AS estado_pago
                FROM tblComisionFacturaEncabezado cf
                INNER JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=cf.idFacturaEncabezado
                LEFT JOIN tblVendedor v ON v.idVendedor=f.idVendedor
                LEFT JOIN tblEmpleado e ON e.idEmpleado=v.idEmpleado
                LEFT JOIN tblMoneda m ON m.idMoneda=f.idMoneda
                INNER JOIN tblComisionAbono ca ON ca.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                INNER JOIN tblAplicacionPago ap ON ap.idAplicacionPago=ca.idAplicacionPago
                INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                LEFT JOIN (SELECT idComisionFacturaEncabezado,
                                  SUM(montoComisionLineaComisionFacturaDetalle) AS total_comision
                           FROM tblComisionFacturaDetalle GROUP BY idComisionFacturaEncabezado) det
                       ON det.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                WHERE cf.estadoComisionFacturaEncabezado='CALCULADA'
                  AND ca.estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')
                  AND ap.estadoAplicacionPago='APLICADA'
                  AND rp.estadoReciboPago='CONFIRMADO'
                ORDER BY ap.fechaAplicacionPago DESC,ca.idComisionAbono DESC";
            DataTable Datos = VentasMetEjecucionConsulta(Comando, new List<OdbcParameter>());
            List<ClsComisionNomina> Resultado = new List<ClsComisionNomina>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsComisionNomina
                {
                    IdAbono = Convert.ToInt32(Fila["id_abono"]),
                    IdFactura = Convert.ToInt32(Fila["id_factura"]),
                    FechaAbono = Convert.ToDateTime(Fila["fecha_abono"]),
                    Vendedor = Convert.ToString(Fila["vendedor"]),
                    SimboloMoneda = Convert.ToString(Fila["simbolo_moneda"]),
                    TotalComision = Convert.ToDecimal(Fila["total_comision"]),
                    MontoPagoCliente = Convert.ToDecimal(Fila["monto_pago_cliente"]),
                    ComisionDevengadaAbono = Convert.ToDecimal(Fila["comision_devengada_abono"]),
                    EstadoPago = Convert.ToString(Fila["estado_pago"])
                });
            return Resultado;
        }

        public ClsFacturaComision VentasMetObtenerFactura(int IdFactura)
        {
            const string Comando = @"
                SELECT f.idFacturaEncabezado AS id, f.idVendedor AS id_vendedor,
                       f.fechaEmisionFacturaEncabezado AS fecha, cfv.nombreClienteFacturaVersion AS cliente,
                       COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd WHERE fd.idFacturaEncabezadoUuidDte=f.uuidDteFacturaEncabezado),0) AS total,
                       COALESCE((SELECT SUM(ap.montoAplicadoAplicacionPago) FROM tblAplicacionPago ap INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                           WHERE ap.idFacturaEncabezado=f.idFacturaEncabezado AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'),0) AS total_pagado,
                       COALESCE((SELECT ap.montoAplicadoAplicacionPago FROM tblAplicacionPago ap INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                           WHERE ap.idFacturaEncabezado=f.idFacturaEncabezado AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'
                           ORDER BY ap.fechaAplicacionPago DESC,ap.idAplicacionPago DESC LIMIT 1),0) AS ultimo_abono
                FROM tblFacturaEncabezado f LEFT JOIN tblClienteFacturaVersion cfv ON cfv.idClienteFacturaVersion=f.idClienteFacturaVersion
                WHERE f.idFacturaEncabezado = ?";
            DataTable Datos = VentasMetEjecucionConsulta(Comando,
                new List<OdbcParameter> { new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura } });
            return Datos.Rows.Count == 0 ? null : VentasMetMapearFactura(Datos.Rows[0]);
        }

        public ClsVendedorComision VentasMetObtenerVendedor(int IdVendedor)
        {
            return VentasMetObtenerVendedores().FirstOrDefault(Vendedor => Vendedor.IdVendedor == IdVendedor);
        }

        public List<ClsDetalleProductoComision> VentasMetObtenerDetallesFactura(int IdFactura, string Metodo)
        {
            Metodo = VentasMetNormalizarMetodo(Metodo);
            string NombreColumna;
            string IdClasificacion = VentasMetObtenerIdClasificacionSql(Metodo);
            string Porcentaje = VentasMetObtenerPorcentajeSql(Metodo);
            string PorcentajeLogistica = "CASE WHEN COALESCE(" + Porcentaje + ",0)>0.99 THEN COALESCE(" +
                Porcentaje + ",0) ELSE COALESCE(" + Porcentaje + ",0)*100 END";
            switch (Metodo)
            {
                case "Producto": NombreColumna = "p.nombreProducto"; break;
                case "Marca": NombreColumna = "m.nombreMarca"; break;
                case "Linea": NombreColumna = "l.nombreLinea"; break;
                case "Categoria": NombreColumna = "c.nombreCategoria"; break;
                default: throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            }

            string Comando = @"
                SELECT fd.idFacturaDetalle AS id_detalle, fd.idProducto AS id_producto,
                       p.idProducto AS id_producto_logistica,
                       CASE COALESCE(cd.metodoComisionFacturaDetalle, '')
                         WHEN 'PRODUCTO' THEN p.idProducto WHEN 'MARCA' THEN p.Fk_Marca_Codigo
                         WHEN 'LINEA' THEN p.Fk_Linea_Codigo WHEN 'CATEGORIA' THEN p.Fk_Categoria_Codigo
                         ELSE " + IdClasificacion + @" END AS id_clasificacion,
                        p.nombreProducto AS nombre_producto,
                       COALESCE(m.nombreMarca, '') AS marca, COALESCE(l.nombreLinea, '') AS linea,
                       COALESCE(c.nombreCategoria, '') AS categoria,
                       COALESCE(cd.metodoComisionFacturaDetalle, cf.metodoComisionFacturaEncabezado, '') AS metodo_guardado,
                       COALESCE(f.idVendedor, 0) AS vendedor_guardado,
                       COALESCE(cf.idComisionFacturaEncabezado, 0) AS comision_guardada,
                       COALESCE(cd.montoComisionLineaComisionFacturaDetalle, 0) AS monto_comision_guardado,
                       COALESCE(cd.nombreClasificacionHistoricaComisionFacturaDetalle, " + NombreColumna + @") AS elemento,
                       fd.cantidadFacturaDetalle AS cantidad, fd.precioUnitarioFacturaDetalle AS precio_unitario,
                       GREATEST(fd.totalLineaFacturaDetalle - COALESCE(fd.impuestoMontoFacturaDetalle,0), 0) AS base_neta,
                       CAST((" + PorcentajeLogistica + @") AS CHAR) AS porcentaje,
                       CAST(CASE WHEN COALESCE(p.porcentajeComisionProducto,0)>0.99 THEN p.porcentajeComisionProducto ELSE COALESCE(p.porcentajeComisionProducto,0)*100 END AS CHAR) AS porcentaje_producto,
                       CAST(CASE WHEN COALESCE(m.porcentajeComisionMarca,0)>0.99 THEN m.porcentajeComisionMarca ELSE COALESCE(m.porcentajeComisionMarca,0)*100 END AS CHAR) AS porcentaje_marca,
                       CAST(CASE WHEN COALESCE(l.porcentajeComisionLinea,0)>0.99 THEN l.porcentajeComisionLinea ELSE COALESCE(l.porcentajeComisionLinea,0)*100 END AS CHAR) AS porcentaje_linea,
                       CAST(CASE WHEN COALESCE(c.porcentajeComisionCategoria,0)>0.99 THEN c.porcentajeComisionCategoria ELSE COALESCE(c.porcentajeComisionCategoria,0)*100 END AS CHAR) AS porcentaje_categoria,
                       COALESCE((SELECT SUM(ca.montoComisionDevengadoComisionAbono)
                           FROM tblComisionAbono ca INNER JOIN tblAplicacionPago ap ON ap.idAplicacionPago=ca.idAplicacionPago
                           INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                           WHERE ca.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                             AND ca.estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')
                             AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'),0) AS comision_devengada,
                       COALESCE((SELECT ca.montoComisionDevengadoComisionAbono
                           FROM tblComisionAbono ca INNER JOIN tblAplicacionPago ap ON ap.idAplicacionPago=ca.idAplicacionPago
                           INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                           WHERE ca.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                             AND ca.estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')
                             AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'
                           ORDER BY ap.fechaAplicacionPago DESC,ap.idAplicacionPago DESC LIMIT 1),0) AS ultima_comision_devengada
                FROM tblFacturaDetalle fd INNER JOIN tblFacturaEncabezado f ON f.uuidDteFacturaEncabezado=fd.idFacturaEncabezadoUuidDte
                INNER JOIN tblProducto p ON p.idProducto=fd.idProducto
                LEFT JOIN tblMarca m ON m.idMarca=p.Fk_Marca_Codigo
                LEFT JOIN tblLinea l ON l.idLinea=p.Fk_Linea_Codigo
                LEFT JOIN tblCategoria c ON c.idCategoria=p.Fk_Categoria_Codigo
                LEFT JOIN tblComisionFacturaEncabezado cf ON cf.idFacturaEncabezado=f.idFacturaEncabezado
                LEFT JOIN tblComisionFacturaDetalle cd ON cd.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado AND cd.idFacturaDetalle=fd.idFacturaDetalle
                 WHERE f.idFacturaEncabezado = ? ORDER BY fd.numeroLineaFacturaDetalle";
            DataTable Datos = VentasMetEjecucionConsulta(Comando,
                new List<OdbcParameter> { new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura } });
            List<ClsDetalleProductoComision> Resultado = new List<ClsDetalleProductoComision>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsDetalleProductoComision
                {
                    IdFacturaDetalle = Convert.ToInt32(Fila["id_detalle"]),
                    IdProducto = Convert.ToInt32(Fila["id_producto"]),
                    IdProductoLogistica = Fila.IsNull("id_producto_logistica") ? 0 : Convert.ToInt32(Fila["id_producto_logistica"]),
                    IdClasificacionLogistica = Fila.IsNull("id_clasificacion") ? 0 : Convert.ToInt32(Fila["id_clasificacion"]),
                    NombreProducto = Convert.ToString(Fila["nombre_producto"]),
                    Marca = Convert.ToString(Fila["marca"]),
                    Linea = Convert.ToString(Fila["linea"]),
                    Categoria = Convert.ToString(Fila["categoria"]),
                    MetodoGuardado = Convert.ToString(Fila["metodo_guardado"]),
                    IdVendedorGuardado = Convert.ToInt32(Fila["vendedor_guardado"]),
                    ComisionGuardada = Convert.ToInt32(Fila["comision_guardada"]) > 0,
                    MontoComisionTotalGuardado = Convert.ToDecimal(Fila["monto_comision_guardado"]),
                    ComisionDevengada = Convert.ToDecimal(Fila["comision_devengada"]),
                    UltimaComisionDevengada = Convert.ToDecimal(Fila["ultima_comision_devengada"]),
                    ElementoComision = Convert.ToString(Fila["elemento"]),
                    Cantidad = Convert.ToDouble(Fila["cantidad"], CultureInfo.InvariantCulture),
                    PrecioUnitario = Convert.ToDecimal(Fila["precio_unitario"]),
                    BaseComisionable = Convert.ToDecimal(Fila["base_neta"]),
                    Porcentaje = Convert.ToDecimal(Fila["porcentaje"], CultureInfo.InvariantCulture),
                    PorcentajeProducto = Convert.ToDecimal(Fila["porcentaje_producto"], CultureInfo.InvariantCulture),
                    PorcentajeMarca = Convert.ToDecimal(Fila["porcentaje_marca"], CultureInfo.InvariantCulture),
                    PorcentajeLinea = Convert.ToDecimal(Fila["porcentaje_linea"], CultureInfo.InvariantCulture),
                    PorcentajeCategoria = Convert.ToDecimal(Fila["porcentaje_categoria"], CultureInfo.InvariantCulture)
                });
            return Resultado;
        }

        public List<ClsVentaVendedorBase> VentasMetObtenerVentasPorMetodo(string Metodo, DateTime FechaInicio, DateTime FechaFinExclusiva)
        {
            VentasMetSincronizarAbonos(null);
            string Comando = @"
                SELECT ca.idComisionAbono AS id_abono, f.idVendedor AS id_vendedor,
                       COALESCE(NULLIF(TRIM(CONCAT_WS(' ', e.nombresEmpleado, e.apellidosEmpleado)), ''), v.codigoVendedor) AS nombre_vendedor,
                       f.idMoneda AS id_moneda,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       f.idFacturaEncabezado AS id_factura, ca.idAplicacionPago AS id_aplicacion_pago,
                       ap.fechaAplicacionPago AS fecha_aplicacion,
                        CONCAT('FAC-',f.idFacturaEncabezado,' · ',p.nombreProducto) AS elemento_comision,
                       cd.metodoComisionFacturaDetalle AS metodo_comision,
                       cd.tasaAplicadaComisionFacturaDetalle * 100 AS porcentaje,
                       (cd.baseComisionFacturaDetalle / NULLIF(totalFactura.total,0)) * ap.montoAplicadoAplicacionPago AS base_cobrada,
                       (cd.montoComisionLineaComisionFacturaDetalle / NULLIF(comisionFactura.total_comision,0)) * ca.montoComisionDevengadoComisionAbono AS monto_comision,
                       fd.idFacturaDetalle AS id_detalle
                FROM tblComisionAbono ca
                INNER JOIN tblComisionFacturaEncabezado cf ON cf.idComisionFacturaEncabezado=ca.idComisionFacturaEncabezado
                INNER JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=cf.idFacturaEncabezado
                INNER JOIN tblMoneda m ON m.idMoneda=f.idMoneda
                INNER JOIN tblVendedor v ON v.idVendedor=f.idVendedor
                LEFT JOIN tblEmpleado e ON e.idEmpleado=v.idEmpleado
                INNER JOIN tblAplicacionPago ap ON ap.idAplicacionPago=ca.idAplicacionPago
                INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                INNER JOIN tblFacturaDetalle fd ON fd.idFacturaEncabezadoUuidDte=f.uuidDteFacturaEncabezado
                INNER JOIN tblProducto p ON p.idProducto=fd.idProducto
                INNER JOIN tblComisionFacturaDetalle cd ON cd.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado AND cd.idFacturaDetalle=fd.idFacturaDetalle
                INNER JOIN (SELECT f2.idFacturaEncabezado,SUM(fd.totalLineaFacturaDetalle) total
                            FROM tblFacturaEncabezado f2
                            INNER JOIN tblFacturaDetalle fd ON fd.idFacturaEncabezadoUuidDte=f2.uuidDteFacturaEncabezado
                            GROUP BY f2.idFacturaEncabezado) totalFactura ON totalFactura.idFacturaEncabezado=f.idFacturaEncabezado
                INNER JOIN (SELECT idComisionFacturaEncabezado,SUM(montoComisionLineaComisionFacturaDetalle) total_comision FROM tblComisionFacturaDetalle GROUP BY idComisionFacturaEncabezado) comisionFactura ON comisionFactura.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                WHERE ca.estadoPagoVendedorComisionAbono = 'PENDIENTE'
                  AND cf.estadoComisionFacturaEncabezado = 'CALCULADA'
                  AND f.estadoFacturaEncabezado = 'EMITIDA'
                  AND ap.estadoAplicacionPago = 'APLICADA' AND rp.estadoReciboPago = 'CONFIRMADO'
                  AND ap.fechaAplicacionPago >= ? AND ap.fechaAplicacionPago < ?
                ORDER BY v.codigoVendedor, ap.fechaAplicacionPago, f.idFacturaEncabezado, fd.numeroLineaFacturaDetalle";
            List<OdbcParameter> Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("FechaInicio", OdbcType.Date) { Value = FechaInicio.Date },
                new OdbcParameter("FechaFin", OdbcType.Date) { Value = FechaFinExclusiva.Date }
            };
            DataTable Datos = VentasMetEjecucionConsulta(Comando, Parametros);
            List<ClsVentaVendedorBase> Resultado = new List<ClsVentaVendedorBase>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsVentaVendedorBase
                {
                    IdComisionAbono = Convert.ToInt32(Fila["id_abono"]),
                    IdVendedor = Convert.ToInt32(Fila["id_vendedor"]),
                    IdMoneda = Convert.ToInt32(Fila["id_moneda"]),
                    SimboloMoneda = Convert.ToString(Fila["simbolo_moneda"]),
                    IdFactura = Convert.ToInt32(Fila["id_factura"]),
                    IdFacturaDetalle = Convert.ToInt32(Fila["id_detalle"]),
                    IdAplicacionPago = Convert.ToInt32(Fila["id_aplicacion_pago"]),
                    FechaAplicacion = Convert.ToDateTime(Fila["fecha_aplicacion"]),
                    NombreVendedor = Convert.ToString(Fila["nombre_vendedor"]),
                    ElementoComision = Convert.ToString(Fila["elemento_comision"]),
                    MetodoComision = Convert.ToString(Fila["metodo_comision"]),
                    PorcentajeVendedor = Convert.ToDecimal(Fila["porcentaje"]),
                    BaseVentas = Convert.ToDecimal(Fila["base_cobrada"]),
                    MontoComision = Convert.ToDecimal(Fila["monto_comision"])
                });

            // También lista asignaciones de facturas cuya comisión aún no tiene un
            // abono pendiente en el período, una fila única por factura/vendedor.
            // Estas filas informan el total calculado,
            // pero llevan IdComisionAbono=0 y no se pueden registrar como pago.
            const string SqlAsignacionesSinAbono = @"
                SELECT f.idVendedor AS id_vendedor, v.codigoVendedor,
                       COALESCE(NULLIF(TRIM(CONCAT_WS(' ',e.nombresEmpleado,e.apellidosEmpleado)),''),v.codigoVendedor) AS nombre_vendedor,
                       f.idMoneda AS id_moneda, f.idFacturaEncabezado AS id_factura,
                       0 AS id_detalle, f.fechaEmisionFacturaEncabezado AS fecha_factura,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       CONCAT('FAC-',f.idFacturaEncabezado,' · Comisión asignada a un vendedor; ',
                              dc.cantidad_lineas,' producto(s); total calculado ',m.simboloMoneda,' ',FORMAT(dc.total_comision,2),
                              ' (sin abono de comisión pendiente en el rango)') AS elemento_comision,
                       dc.metodo_comision,
                       dc.porcentaje,
                       dc.total_comision AS monto_comision_total
                FROM tblComisionFacturaEncabezado cf
                INNER JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=cf.idFacturaEncabezado
                INNER JOIN tblMoneda m ON m.idMoneda=f.idMoneda
                INNER JOIN tblVendedor v ON v.idVendedor=f.idVendedor
                LEFT JOIN tblEmpleado e ON e.idEmpleado=v.idEmpleado
                INNER JOIN (
                    SELECT idComisionFacturaEncabezado,COUNT(*) AS cantidad_lineas,
                           CASE WHEN COUNT(DISTINCT metodoComisionFacturaDetalle)=1 THEN MAX(metodoComisionFacturaDetalle) ELSE 'MIXTA' END AS metodo_comision,
                           CASE WHEN SUM(baseComisionFacturaDetalle)>0
                                THEN SUM(baseComisionFacturaDetalle*tasaAplicadaComisionFacturaDetalle)/SUM(baseComisionFacturaDetalle)*100
                                ELSE 0 END AS porcentaje,
                           SUM(baseComisionFacturaDetalle*tasaAplicadaComisionFacturaDetalle) AS total_comision
                    FROM tblComisionFacturaDetalle GROUP BY idComisionFacturaEncabezado
                ) dc ON dc.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                WHERE cf.estadoComisionFacturaEncabezado='CALCULADA' AND f.estadoFacturaEncabezado='EMITIDA'
                  AND f.fechaEmisionFacturaEncabezado>=? AND f.fechaEmisionFacturaEncabezado<?
                  AND NOT EXISTS (
                      SELECT 1 FROM tblComisionAbono ca
                      INNER JOIN tblAplicacionPago ap ON ap.idAplicacionPago=ca.idAplicacionPago
                      INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                      WHERE ca.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                        AND ca.estadoPagoVendedorComisionAbono='PENDIENTE'
                        AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'
                        AND ap.fechaAplicacionPago>=? AND ap.fechaAplicacionPago<?)
                ORDER BY v.codigoVendedor,f.fechaEmisionFacturaEncabezado,f.idFacturaEncabezado";
            List<OdbcParameter> ParametrosAsignaciones = new List<OdbcParameter>
            {
                new OdbcParameter("FechaFacturaInicio", OdbcType.Date) { Value = FechaInicio.Date },
                new OdbcParameter("FechaFacturaFin", OdbcType.Date) { Value = FechaFinExclusiva.Date },
                new OdbcParameter("FechaPagoInicio", OdbcType.Date) { Value = FechaInicio.Date },
                new OdbcParameter("FechaPagoFin", OdbcType.Date) { Value = FechaFinExclusiva.Date }
            };
            DataTable Asignaciones = VentasMetEjecucionConsulta(SqlAsignacionesSinAbono, ParametrosAsignaciones);
            foreach (DataRow Fila in Asignaciones.Rows)
                Resultado.Add(new ClsVentaVendedorBase
                {
                    IdComisionAbono = 0,
                    IdAplicacionPago = 0,
                    IdVendedor = Convert.ToInt32(Fila["id_vendedor"]),
                    IdMoneda = Convert.ToInt32(Fila["id_moneda"]),
                    SimboloMoneda = Convert.ToString(Fila["simbolo_moneda"]),
                    IdFactura = Convert.ToInt32(Fila["id_factura"]),
                    IdFacturaDetalle = Convert.ToInt32(Fila["id_detalle"]),
                    FechaAplicacion = Convert.ToDateTime(Fila["fecha_factura"]),
                    NombreVendedor = Convert.ToString(Fila["nombre_vendedor"]),
                    ElementoComision = Convert.ToString(Fila["elemento_comision"]),
                    MetodoComision = Convert.ToString(Fila["metodo_comision"]),
                    PorcentajeVendedor = Convert.ToDecimal(Fila["porcentaje"]),
                    BaseVentas = 0m,
                    MontoComision = Convert.ToDecimal(Fila["monto_comision_total"])
                });
            return Resultado;
        }

        public List<ClsProductoComision> VentasMetConsultarPorcentajes(string Metodo)
        {
            VentasMetObtenerIdClasificacionSql(Metodo);
            const string Comando = @"SELECT p.idProducto AS id, p.nombreProducto AS nombre,
                       COALESCE(m.idMarca,0) AS id_marca, COALESCE(l.idLinea,0) AS id_linea,
                       COALESCE(c.idCategoria,0) AS id_categoria,
                       COALESCE(m.nombreMarca,'') AS marca, COALESCE(l.nombreLinea,'') AS linea,
                       COALESCE(c.nombreCategoria,'') AS categoria,
                       CAST(CASE WHEN COALESCE(m.porcentajeComisionMarca,0)>0.99 THEN m.porcentajeComisionMarca ELSE COALESCE(m.porcentajeComisionMarca,0)*100 END AS CHAR) AS porcentaje_marca,
                       CAST(CASE WHEN COALESCE(l.porcentajeComisionLinea,0)>0.99 THEN l.porcentajeComisionLinea ELSE COALESCE(l.porcentajeComisionLinea,0)*100 END AS CHAR) AS porcentaje_linea,
                       CAST(CASE WHEN COALESCE(c.porcentajeComisionCategoria,0)>0.99 THEN c.porcentajeComisionCategoria ELSE COALESCE(c.porcentajeComisionCategoria,0)*100 END AS CHAR) AS porcentaje_categoria,
                       CAST(CASE WHEN COALESCE(p.porcentajeComisionProducto,0)>0.99 THEN p.porcentajeComisionProducto ELSE COALESCE(p.porcentajeComisionProducto,0)*100 END AS CHAR) AS porcentaje_producto
                       FROM tblProducto p LEFT JOIN tblMarca m ON m.idMarca=p.Fk_Marca_Codigo
                       LEFT JOIN tblLinea l ON l.idLinea=p.Fk_Linea_Codigo
                       LEFT JOIN tblCategoria c ON c.idCategoria=p.Fk_Categoria_Codigo
                       ORDER BY p.nombreProducto";
            DataTable Datos = VentasMetEjecucionConsulta(Comando, new List<OdbcParameter>());
            List<ClsProductoComision> Resultado = new List<ClsProductoComision>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsProductoComision
                {
                    IdProducto = Convert.ToInt32(Fila["id"]),
                    IdMarca = Convert.ToInt32(Fila["id_marca"]), IdLinea = Convert.ToInt32(Fila["id_linea"]),
                    IdCategoria = Convert.ToInt32(Fila["id_categoria"]), NombreProducto = Convert.ToString(Fila["nombre"]),
                    Marca = Convert.ToString(Fila["marca"]), Linea = Convert.ToString(Fila["linea"]),
                    Categoria = Convert.ToString(Fila["categoria"]),
                    Porcentaje = 0m,
                    PorcentajeMarca = Convert.ToDecimal(Fila["porcentaje_marca"], CultureInfo.InvariantCulture),
                    PorcentajeLinea = Convert.ToDecimal(Fila["porcentaje_linea"], CultureInfo.InvariantCulture),
                    PorcentajeCategoria = Convert.ToDecimal(Fila["porcentaje_categoria"], CultureInfo.InvariantCulture),
                    PorcentajeProducto = Convert.ToDecimal(Fila["porcentaje_producto"], CultureInfo.InvariantCulture)
                });
            return Resultado;
        }
    }
}
