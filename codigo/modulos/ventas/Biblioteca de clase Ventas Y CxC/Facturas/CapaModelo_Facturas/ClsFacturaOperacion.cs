using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using VentasDatos;

namespace CapaModelo_Facturas
{
    public static class ReglasFactura
    {
        public static string NormalizarIdentificacion(string valor) { return (valor ?? "").Trim().ToUpperInvariant(); }
        public static bool IdentificacionValida(string valor)
        {
            valor = NormalizarIdentificacion(valor);
            return valor == "CF" || Regex.IsMatch(valor, @"^\d{1,9}-?[0-9K]$") || Regex.IsMatch(valor, @"^\d{13}$");
        }
        public static decimal Redondear(decimal valor) { return Math.Round(valor, 4, MidpointRounding.AwayFromZero); }
        public static decimal[] Calcular(decimal cantidad, decimal precio, decimal descuento, decimal tasa)
        {
            if (cantidad <= 0 || cantidad != decimal.Truncate(cantidad)) throw new ArgumentException("La cantidad debe ser un número entero mayor a cero (unidades de inventario).");
            if (precio < 0 || descuento < 0 || descuento > cantidad * precio) throw new ArgumentException("Precio o descuento inválido.");
            if (precio != Redondear(precio) || descuento != Redondear(descuento)) throw new ArgumentException("Los importes admiten hasta cuatro decimales.");
            if (tasa < 0 || tasa > .99m || Redondear(tasa) != Math.Round(tasa, 2)) throw new ArgumentException("La tasa no es compatible con la precisión del detalle de factura.");
            decimal neto = cantidad * precio - descuento;
            decimal impuesto = Redondear(neto * tasa);
            return new[] { Redondear(neto), impuesto, Redondear(neto + impuesto) };
        }
        public static DataTable CrearDetalle()
        {
            var t = new DataTable("Detalle");
            t.Columns.Add("Codigo", typeof(int)); t.Columns.Add("Producto", typeof(string));
            foreach (string n in new[] { "Cantidad", "PrecioUnitario", "Descuento", "Tasa", "Subtotal", "Impuesto", "Total" }) t.Columns.Add(n, typeof(decimal));
            return t;
        }
        public static void Recalcular(DataTable detalle)
        {
            if (detalle == null || detalle.Rows.Count == 0) throw new ArgumentException("Agregue al menos un producto.");
            foreach (DataRow r in detalle.Rows)
            {
                var v = Calcular(Convert.ToDecimal(r["Cantidad"]), Convert.ToDecimal(r["PrecioUnitario"]), Convert.ToDecimal(r["Descuento"]), Convert.ToDecimal(r["Tasa"]));
                r["Subtotal"] = v[0]; r["Impuesto"] = v[1]; r["Total"] = v[2];
            }
        }
    }

    public sealed class SolicitudFactura
    {
        public int Cliente, Usuario, Sucursal, Empresa, OrigenId, FormaPago;
        public long Moneda;
        public bool Pedido;
        public string Condicion, ReferenciaPago;
        public DataTable Detalle;
        public DataTable Distribucion;
        public string Uuid = Guid.NewGuid().ToString().ToUpperInvariant();
    }

    public sealed class FacturaRegistrada
    {
        public int Id;
        public string Uuid, Serie, Numero, Moneda, Cliente, Identificacion, Direccion, Empresa, NitEmisor, DireccionEmisor, Condicion;
        public DateTime Fecha;
        public DataTable Detalle;
    }

    public sealed partial class ClsFacturaOrigenRepositorio
    {
        public DataTable Monedas() { using (var d = new ClsDatosVentas()) return d.VentasFuncTabla("SELECT idMoneda Id,CONCAT(codigoMoneda,' - ',nombreMoneda) Nombre,codigoMoneda Codigo FROM tblMoneda WHERE is_active=1 ORDER BY codigoMoneda"); }
        public DataTable Empresas() { using (var d = new ClsDatosVentas()) return d.VentasFuncTabla("SELECT idEmpresa Id,razonSocialEmpresa Nombre FROM tblEmpresa WHERE is_active=1 ORDER BY idEmpresa"); }
        public DataTable FormasPago() { using (var d = new ClsDatosVentas()) return d.VentasFuncTabla("SELECT idFormaPagoVenta Id,nombreFormaPagoVenta Nombre,requiereReferenciaFormaPagoVenta RequiereReferencia FROM tblFormaPagoVenta ORDER BY nombreFormaPagoVenta"); }
        private const string ClienteSql = @"SELECT c.idCliente Id,c.codigoCliente Codigo,c.tipoIdentificacionCliente Tipo,
            COALESCE(c.nitCliente,c.dpiCliente,'CF') Identificacion,
            CASE WHEN c.tipoIdentificacionCliente='CF' THEN 'Consumidor Final' ELSE c.nombreCliente END Nombre,
            CASE WHEN c.tipoIdentificacionCliente='CF' THEN 'Ciudad' ELSE COALESCE(c.direccionCliente,'Ciudad') END Direccion,
            c.diasCreditoCliente Dias,c.limiteCreditoCliente Limite,c.idMonedaCredito MonedaCredito,
            l.idListaPrecioEncabezado Lista,l.idMoneda Moneda
            FROM tblCliente c LEFT JOIN tblClienteListaPrecio a ON a.idCliente=c.idCliente
            LEFT JOIN tblListaPrecioEncabezado l ON l.idListaPrecioEncabezado=a.idListaPrecioEncabezado
            AND l.activoListaPrecioEncabezado=1 AND CURRENT_DATE>=l.vigenteDesdeListaPrecioEncabezado
            AND (l.vigenteHastaListaPrecioEncabezado IS NULL OR CURRENT_DATE<=l.vigenteHastaListaPrecioEncabezado)
            WHERE c.activoCliente=1 ";
        public DataTable BuscarCliente(string identificacion, bool codigo)
        {
            identificacion = ReglasFactura.NormalizarIdentificacion(identificacion);
            using (var d = new ClsDatosVentas())
                return codigo ? d.VentasFuncTabla(ClienteSql + " AND c.codigoCliente=?", identificacion)
                    : d.VentasFuncTabla(ClienteSql + " AND ((?='CF' AND c.tipoIdentificacionCliente='CF') OR REPLACE(c.nitCliente,'-','')=REPLACE(?,'-','') OR c.dpiCliente=?)", identificacion, identificacion, identificacion);
        }
        private static decimal Tasa(ClsDatosVentas d)
        {
            var t = d.VentasFuncTabla("SELECT valorImpuesto FROM tblImpuesto WHERE UPPER(nombreImpuesto)='IVA' AND operacionImpuesto IN ('VENTA','AMBAS')");
            if (t.Rows.Count != 1) throw new InvalidOperationException("Debe existir un único impuesto IVA aplicable a ventas.");
            decimal tasa = Convert.ToDecimal(t.Rows[0][0]); ReglasFactura.Calcular(1, 1, 0, tasa); return tasa;
        }
        private const string ProductosSql = @"SELECT p.idProducto Codigo,p.nombreProducto Producto,d.precioUnitarioListaPrecioDetalle Precio,l.idMoneda Moneda,
            COALESCE((SELECT SUM(GREATEST(0,LEAST(s.stockActualStock,s.stockFisicoStock)-s.apartadosStock)) FROM tblStock s WHERE s.Fk_Producto_Codigo=p.idProducto AND s.estadoVentaStock=1),0) Disponible
            FROM tblClienteListaPrecio a JOIN tblListaPrecioEncabezado l ON l.idListaPrecioEncabezado=a.idListaPrecioEncabezado
            JOIN tblListaPrecioDetalle d ON d.idListaPrecioEncabezado=l.idListaPrecioEncabezado AND d.activoListaPrecioDetalle=1
            JOIN tblProducto p ON p.idProducto=d.idProducto JOIN tblProductoVenta v ON v.idProductoVenta=p.idProducto AND v.activoProductoVenta=1
            WHERE a.idCliente=? AND l.activoListaPrecioEncabezado=1 AND CURRENT_DATE>=l.vigenteDesdeListaPrecioEncabezado
            AND (l.vigenteHastaListaPrecioEncabezado IS NULL OR CURRENT_DATE<=l.vigenteHastaListaPrecioEncabezado) ORDER BY p.idProducto";
        private static DataTable Productos(ClsDatosVentas d, int cliente)
        {
            var t = d.VentasFuncTabla(ProductosSql, cliente); t.Columns.Add("Tasa", typeof(decimal));
            decimal tasa = Tasa(d); foreach (DataRow r in t.Rows) r["Tasa"] = tasa; return t;
        }
        public DataTable Productos(int cliente) { using (var d = new ClsDatosVentas()) return Productos(d, cliente); }
        public DataTable Existencias(int producto)
        {
            using (var d = new ClsDatosVentas()) return d.VentasFuncTabla(@"SELECT s.idStock StockId,b.nombreBodega Bodega,s.Fk_Bodega_Codigo BodegaId,
                GREATEST(0,LEAST(s.stockActualStock,s.stockFisicoStock)-s.apartadosStock) Disponible
                FROM tblStock s JOIN tblBodega b ON b.idBodega=s.Fk_Bodega_Codigo
                WHERE s.Fk_Producto_Codigo=? AND s.estadoVentaStock=1 ORDER BY s.idStock", producto);
        }
        public DataTable PrepararDetalleOrigen(bool pedido, int id)
        {
            using (var d = new ClsDatosVentas())
            {
                var doc = VentasFuncDocumento(d, pedido, id); var t = ReglasFactura.CrearDetalle(); decimal tasa = Tasa(d);
                foreach (DataRow r in doc.Tables[1].Rows) t.Rows.Add(r["ProductoId"],r["Producto"],r["Cantidad"],r["Precio"],r["Descuento"],tasa,0,0,0);
                ReglasFactura.Recalcular(t); return t;
            }
        }
        public FacturaRegistrada Registrar(SolicitudFactura s, Action<FacturaRegistrada> guardarPdf)
        {
            if (s == null || guardarPdf == null) throw new ArgumentNullException();
            if (s.Condicion != "CONTADO" && s.Condicion != "CREDITO") throw new ArgumentException("Seleccione contado o crédito.");
            Guid uuid; if (!Guid.TryParse(s.Uuid, out uuid)) throw new ArgumentException("UUID inválido.");
            var detalle = s.Detalle.Copy(); ReglasFactura.Recalcular(detalle);
            using (var d = new ClsDatosVentas())
            {
                d.VentasMetIniciar();
                if (Convert.ToInt32(d.VentasFuncEscalar("SELECT COUNT(*) FROM tblUsuario WHERE idUsuario=? AND is_active=1",s.Usuario)) != 1) throw new InvalidOperationException("Se requiere una sesión activa.");
                if (Convert.ToInt32(d.VentasFuncEscalar("SELECT COUNT(*) FROM tblSucursal WHERE idSucursal=?",s.Sucursal)) != 1) throw new ArgumentException("Seleccione la sucursal emisora.");
                d.VentasFuncEjecutar("INSERT INTO tblControlPeriodoCxC(idMoneda) VALUES (?) ON DUPLICATE KEY UPDATE idMoneda=VALUES(idMoneda)",s.Moneda);
                var cierre = d.VentasFuncEscalar("SELECT fechaUltimoCierreControlPeriodoCxC FROM tblControlPeriodoCxC WHERE idMoneda=? FOR UPDATE",s.Moneda);
                if (cierre != null && cierre != DBNull.Value && DateTime.Today <= Convert.ToDateTime(cierre).Date) throw new InvalidOperationException("El período de esta moneda está cerrado.");
                d.VentasFuncEscalar("SELECT idCliente FROM tblCliente WHERE idCliente=? FOR UPDATE",s.Cliente);
                var clientes = d.VentasFuncTabla(ClienteSql + " AND c.idCliente=?",s.Cliente);
                if (clientes.Rows.Count != 1) throw new InvalidOperationException("Cliente inactivo o inexistente.");
                var c = clientes.Rows[0]; object cotizacion = DBNull.Value, pedidoId = DBNull.Value, vendedor = DBNull.Value;
                decimal tasa = Tasa(d);
                if (s.OrigenId > 0)
                {
                    d.VentasFuncEscalar(s.Pedido ? "SELECT idPedidoEncabezado FROM tblPedidoEncabezado WHERE idPedidoEncabezado=? FOR UPDATE" : "SELECT idCotizacionEncabezado FROM tblCotizacionEncabezado WHERE idCotizacionEncabezado=? FOR UPDATE",s.OrigenId);
                    var origen = VentasFuncDocumento(d,s.Pedido,s.OrigenId); var h=origen.Tables[0].Rows[0];
                    if (Convert.ToInt32(h["ClienteId"])!=s.Cliente || Convert.ToInt64(h["MonedaId"])!=s.Moneda) throw new InvalidOperationException("Cambió el documento de origen. Vuelva a cargarlo.");
                    cotizacion=h["CotizacionId"]; pedidoId=h["PedidoId"]; vendedor=h["VendedorId"];
                    // Se permite quitar productos, pero no cambiar las condiciones del documento original.
                    var pendientes = origen.Tables[1].AsEnumerable().ToList();
                    foreach(DataRow r in detalle.Rows)
                    {
                        var original=pendientes.FirstOrDefault(x=>Convert.ToInt32(x["ProductoId"])==Convert.ToInt32(r["Codigo"]) && Convert.ToDecimal(x["Precio"])==Convert.ToDecimal(r["PrecioUnitario"]) && Convert.ToDecimal(x["Cantidad"])==Convert.ToDecimal(r["Cantidad"]) && Convert.ToDecimal(x["Descuento"])==Convert.ToDecimal(r["Descuento"]));
                        if(original==null) throw new InvalidOperationException("Cambió el detalle de origen. Vuelva a cargarlo.");
                        pendientes.Remove(original);
                    }
                }
                else
                {
                    if(c["Lista"]==DBNull.Value || Convert.ToInt64(c["Moneda"])!=s.Moneda) throw new InvalidOperationException("El cliente necesita una lista vigente en la moneda seleccionada.");
                    var productos=Productos(d,s.Cliente);
                    foreach(DataRow r in detalle.Rows)
                    {
                        var p=productos.AsEnumerable().SingleOrDefault(x=>Convert.ToInt32(x["Codigo"])==Convert.ToInt32(r["Codigo"]));
                        if(p==null || Convert.ToDecimal(p["Precio"])!=Convert.ToDecimal(r["PrecioUnitario"])) throw new InvalidOperationException("Cambió la lista de precios. Actualice los productos antes de emitir.");
                    }
                }
                foreach(DataRow r in detalle.Rows) if(Convert.ToDecimal(r["Tasa"])!=tasa) throw new InvalidOperationException("Cambió el impuesto IVA. Actualice el detalle antes de emitir.");
                decimal total=detalle.AsEnumerable().Sum(r=>Convert.ToDecimal(r["Total"]));
                if(total<=0) throw new ArgumentException("El total de la factura debe ser mayor a cero.");
                if(s.Condicion=="CREDITO")
                {
                    if(Convert.ToString(c["Tipo"])=="CF" || Convert.ToInt32(c["Dias"])<=0 || Convert.ToInt64(c["MonedaCredito"])!=s.Moneda) throw new InvalidOperationException("El cliente no tiene crédito habilitado en esta moneda.");
                    decimal saldo=Convert.ToDecimal(d.VentasFuncEscalar(@"SELECT COALESCE(SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC),0) FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=m.idFacturaEncabezado JOIN tblClienteFacturaVersion v ON v.idClienteFacturaVersion=f.idClienteFacturaVersion WHERE v.idCliente=? AND f.idMoneda=?",s.Cliente,s.Moneda));
                    if(saldo+total>Convert.ToDecimal(c["Limite"])) throw new InvalidOperationException("La factura supera el límite de crédito disponible.");
                }
                var moneda=d.VentasFuncTabla("SELECT codigoMoneda FROM tblMoneda WHERE idMoneda=? AND is_active=1",s.Moneda);
                var empresa=d.VentasFuncTabla("SELECT razonSocialEmpresa,nitEmpresa,direccionEmpresa FROM tblEmpresa WHERE idEmpresa=? AND is_active=1",s.Empresa);
                if(moneda.Rows.Count!=1 || empresa.Rows.Count!=1) throw new InvalidOperationException("Seleccione moneda y empresa activas.");
                var f=new FacturaRegistrada { Uuid=s.Uuid,Numero=d.VentasFuncNumero("FVE"),Fecha=DateTime.Now,Detalle=detalle,
                    Moneda=Convert.ToString(moneda.Rows[0][0]),Cliente=Convert.ToString(c["Nombre"]),Identificacion=Convert.ToString(c["Identificacion"]),Direccion=Convert.ToString(c["Direccion"]),
                    Empresa=Convert.ToString(empresa.Rows[0][0]),NitEmisor=Convert.ToString(empresa.Rows[0][1]),DireccionEmisor=Convert.ToString(empresa.Rows[0][2]),Condicion=s.Condicion };
                f.Serie="S-"+f.Numero.Substring(4); // Correlativo protegido por bloqueo, único también para la serie.
                d.VentasFuncEjecutar(@"INSERT INTO tblClienteFacturaVersion(idCliente,numeroVersionClienteFacturaVersion,tipoIdentificacionClienteFacturaVersion,nitClienteFacturaVersion,dpiClienteFacturaVersion,nombreClienteFacturaVersion,direccionClienteFacturaVersion)
                    SELECT c.idCliente,COALESCE((SELECT MAX(v.numeroVersionClienteFacturaVersion)+1 FROM tblClienteFacturaVersion v WHERE v.idCliente=c.idCliente),1),c.tipoIdentificacionCliente,c.nitCliente,c.dpiCliente,?,? FROM tblCliente c WHERE c.idCliente=?",f.Cliente,f.Direccion,s.Cliente);
                int version=d.VentasFuncObtenerId();
                d.VentasFuncEjecutar(@"INSERT INTO tblFacturaEncabezado(uuidDteFacturaEncabezado,serieFacturaEncabezado,numeroFacturaEncabezado,idSucursal,idClienteFacturaVersion,idVendedor,idMoneda,idCotizacionEncabezado,idPedidoEncabezado,fechaEmisionFacturaEncabezado,fechaVencimientoFacturaEncabezado,condicionPagoFacturaEncabezado,estadoFacturaEncabezado,subtotalFacturaEncabezado,impuestoIvaFacturaEncabezado,totalFacturaEncabezado,idUsuarioRegistro,observacionesFacturaEncabezado)
                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,'EMITIDA',?,?,?,?,?)",f.Uuid,f.Serie,f.Numero,s.Sucursal,version,vendedor,s.Moneda,cotizacion,pedidoId,f.Fecha,f.Fecha.Date.AddDays(s.Condicion=="CREDITO"?Convert.ToInt32(c["Dias"]):0),s.Condicion,
                    detalle.AsEnumerable().Sum(r=>Convert.ToDecimal(r["Subtotal"])),detalle.AsEnumerable().Sum(r=>Convert.ToDecimal(r["Impuesto"])),total,s.Usuario,"Emisión local sin certificación FEL. Empresa: "+f.Empresa+" | NIT: "+f.NitEmisor);
                f.Id=d.VentasFuncObtenerId();int linea=0;
                foreach(DataRow r in detalle.Rows) d.VentasFuncEjecutar(@"INSERT INTO tblFacturaDetalle(idFacturaEncabezadoUuidDte,numeroLineaFacturaDetalle,idProducto,cantidadFacturaDetalle,precioUnitarioFacturaDetalle,descuentoMontoFacturaDetalle,tasaImpuestoFacturaDetalle) VALUES (?,?,?,?,?,?,?)",f.Uuid,++linea,r["Codigo"],r["Cantidad"],r["PrecioUnitario"],r["Descuento"],r["Tasa"]);
                DescontarStock(d,s, f);
                d.VentasFuncEjecutar("INSERT INTO tblMovimientoCxC(idFacturaEncabezado,fechaMovimientoCxC,tipoMovimientoCxC,descripcionMovimientoCxC,cargoMovimientoCxC,idUsuarioRegistro) VALUES (?,?,'FACTURA',?,?,?)",f.Id,f.Fecha.Date,f.Numero,total,s.Usuario);
                if(s.Condicion=="CONTADO") RegistrarPago(d,s,f,total);
                if(cotizacion!=DBNull.Value) d.VentasFuncEjecutar("UPDATE tblCotizacionEncabezado SET estadoCotizacionEncabezado='ACEPTADA' WHERE idCotizacionEncabezado=?",cotizacion);
                if(pedidoId!=DBNull.Value) d.VentasFuncEjecutar("UPDATE tblPedidoEncabezado SET estadoPedidoEncabezado='COMPLETADO' WHERE idPedidoEncabezado=?",pedidoId);
                d.VentasMetAuditar("FACTURA",f.Id,"GUARDAR",f.Numero+" | "+f.Uuid,s.Usuario);
                guardarPdf(f); // Si falla o cancela la escritura, Dispose revierte toda la operación.
                d.VentasMetConfirmar();return f;
            }
        }
        private static void RegistrarPago(ClsDatosVentas d,SolicitudFactura s,FacturaRegistrada f,decimal total)
        {
            var forma=d.VentasFuncTabla("SELECT requiereReferenciaFormaPagoVenta FROM tblFormaPagoVenta WHERE idFormaPagoVenta=?",s.FormaPago);
            if(forma.Rows.Count!=1 || (Convert.ToBoolean(forma.Rows[0][0]) && string.IsNullOrWhiteSpace(s.ReferenciaPago))) throw new ArgumentException("Seleccione una forma de pago y su referencia cuando corresponda.");
            d.VentasFuncEjecutar(@"INSERT INTO tblReciboPago(numeroReciboPago,idCliente,idMoneda,fechaReciboPago,montoReciboPago,referenciaReciboPago,estadoReciboPago,idUsuarioRegistro,idFormaPagoVenta) VALUES (?,?,?,?,?,?,'CONFIRMADO',?,?)",d.VentasFuncNumero("REC"),s.Cliente,s.Moneda,f.Fecha.Date,total,s.ReferenciaPago,s.Usuario,s.FormaPago);
            int recibo=d.VentasFuncObtenerId();
            d.VentasFuncEjecutar("INSERT INTO tblAplicacionPago(idReciboPago,idFacturaEncabezado,fechaAplicacionPago,montoAplicadoAplicacionPago,estadoAplicacionPago,idUsuarioRegistro) VALUES (?,?,?,?,'APLICADA',?)",recibo,f.Id,f.Fecha.Date,total,s.Usuario);
            int aplicacion=d.VentasFuncObtenerId();
            d.VentasFuncEjecutar("INSERT INTO tblMovimientoCxC(idFacturaEncabezado,idAplicacionPago,fechaMovimientoCxC,tipoMovimientoCxC,descripcionMovimientoCxC,abonoMovimientoCxC,idUsuarioRegistro) VALUES (?,?,?,'PAGO',?,?,?)",f.Id,aplicacion,f.Fecha.Date,"Pago contado "+f.Numero,total,s.Usuario);
        }
        private static void DescontarStock(ClsDatosVentas d,SolicitudFactura s,FacturaRegistrada f)
        {
            if(s.Distribucion==null) throw new ArgumentException("Seleccione las bodegas de salida.");
            var requerido=f.Detalle.AsEnumerable().GroupBy(r=>Convert.ToInt32(r["Codigo"])).ToDictionary(g=>g.Key,g=>g.Sum(r=>Convert.ToDecimal(r["Cantidad"])));
            foreach(var par in requerido)
                if(s.Distribucion.AsEnumerable().Where(r=>Convert.ToInt32(r["ProductoId"])==par.Key).Sum(r=>Convert.ToDecimal(r["Cantidad"]))!=par.Value) throw new ArgumentException("La distribución por bodegas no coincide con la cantidad facturada.");
            var tipo=d.VentasFuncEscalar("SELECT idTipoMovimiento FROM tblTipoMovimiento WHERE nombreTipoMovimiento='Salida por facturación' AND signoTipoMovimiento='-'");
            if(tipo==null || tipo==DBNull.Value) throw new InvalidOperationException("Configure el tipo de movimiento Salida por facturación.");
            foreach(DataRow r in s.Distribucion.Select("", "StockId ASC"))
            {
                int producto=Convert.ToInt32(r["ProductoId"]);decimal cantidad=Convert.ToDecimal(r["Cantidad"]);
                if(!requerido.ContainsKey(producto) || cantidad<=0 || cantidad!=decimal.Truncate(cantidad)) throw new ArgumentException("Distribución de inventario inválida.");
                var stock=d.VentasFuncTabla("SELECT s.*,p.precioCostoProducto FROM tblStock s JOIN tblProducto p ON p.idProducto=s.Fk_Producto_Codigo WHERE s.idStock=? AND s.Fk_Producto_Codigo=? AND s.estadoVentaStock=1 FOR UPDATE",r["StockId"],producto);
                if(stock.Rows.Count!=1) throw new InvalidOperationException("La bodega ya no está disponible.");
                var st=stock.Rows[0];
                decimal disponible=Math.Min(Convert.ToDecimal(st["stockActualStock"]),Convert.ToDecimal(st["stockFisicoStock"]))-Convert.ToDecimal(st["apartadosStock"]);
                if(cantidad>disponible) throw new InvalidOperationException("Existencias insuficientes. Vuelva a distribuir las bodegas.");
                d.VentasFuncEjecutar(@"UPDATE tblStock SET stockAnteriorStock=stockActualStock,stockActualStock=stockActualStock-?,stockFisicoStock=stockFisicoStock-?,stockVirtualStock=GREATEST(0,stockVirtualStock-?),abonosMesStock=abonosMesStock+?,abonosAcumuladosStock=abonosAcumuladosStock+? WHERE idStock=?",cantidad,cantidad,cantidad,cantidad,cantidad,r["StockId"]);
                d.VentasFuncEjecutar("INSERT INTO tblMovimientoInventario(Fk_Tipo_Movimiento_Codigo,Fk_Bodega_Origen_Codigo,observacionesMovimientoInventario,estadoMovimientoInventario) VALUES (?,?,?,'CUADRADO')",tipo,st["Fk_Bodega_Codigo"],"Factura "+f.Numero+" | UUID "+f.Uuid);
                int movimiento=d.VentasFuncObtenerId();
                d.VentasFuncEjecutar("INSERT INTO tblDetalleMovimiento(Fk_Movimiento_Inventario_Codigo,Fk_Producto_Codigo,cantidadDetalleMovimiento,precioCostoDetalleMovimiento) VALUES (?,?,?,?)",movimiento,producto,cantidad,st["precioCostoProducto"]);
            }
        }
    }
}
