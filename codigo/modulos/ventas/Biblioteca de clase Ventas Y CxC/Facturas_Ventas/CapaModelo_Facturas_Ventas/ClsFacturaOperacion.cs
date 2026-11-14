using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using VentasDatos;

namespace CapaModelo_Facturas_Ventas
{
    public static class ClsReglasFactura
    {
        public static string VentasFuncNormalizarIdentificacion(string Valor) { return (Valor ?? "").Trim().ToUpperInvariant(); }
        public static bool VentasFuncIdentificacionValida(string Valor)
        {
            Valor = VentasFuncNormalizarIdentificacion(Valor);
            return Valor == "CF" || Regex.IsMatch(Valor, @"^\d{1,9}-?[0-9K]$") || Regex.IsMatch(Valor, @"^\d{13}$");
        }
        public static decimal VentasFuncRedondear(decimal Valor) { return Math.Round(Valor, 4, MidpointRounding.AwayFromZero); }
        public static decimal[] VentasFuncCalcular(decimal Cantidad, decimal Precio, decimal Descuento, decimal Tasa)
        {
            if (Cantidad <= 0 || Cantidad != decimal.Truncate(Cantidad)) throw new ArgumentException("La cantidad debe ser un número entero mayor a cero (unidades de inventario).");
            if (Precio < 0 || Descuento < 0 || Descuento > Cantidad * Precio) throw new ArgumentException("Precio o descuento inválido.");
            if (Precio != VentasFuncRedondear(Precio) || Descuento != VentasFuncRedondear(Descuento)) throw new ArgumentException("Los importes admiten hasta cuatro decimales.");
            if (Tasa < 0 || Tasa > .99m || VentasFuncRedondear(Tasa) != Math.Round(Tasa, 2)) throw new ArgumentException("La tasa no es compatible con la precisión del detalle de factura.");
            decimal Neto = Cantidad * Precio - Descuento;
            decimal BaseImponible = VentasFuncRedondear(Neto / (1m + Tasa));
            decimal Impuesto = VentasFuncRedondear(Neto - BaseImponible);
            return new[] { BaseImponible, Impuesto, VentasFuncRedondear(Neto) };
        }
        // La BD conserva columnas calculadas con IVA agregado. Se persiste la base,
        // mientras el contrato de la factura conserva precio y descuento con IVA incluido.
        public static decimal[] VentasFuncConvertirDetallePersistido(decimal Cantidad, decimal Precio, decimal Descuento, decimal Tasa)
        {
            decimal[] Importes = VentasFuncCalcular(Cantidad, Precio, Descuento, Tasa);
            decimal PrecioBase = Math.Ceiling(Precio / (1m + Tasa) * 10000m) / 10000m;
            decimal DescuentoBase = VentasFuncRedondear(Cantidad * PrecioBase - Importes[0]);
            decimal ImpuestoBase = VentasFuncRedondear(Importes[0] * Tasa);
            if (ImpuestoBase != Importes[1] || DescuentoBase < 0)
                throw new ArgumentException("El importe no puede representarse exactamente a cuatro decimales en el detalle de la BD. Ajuste el descuento en 0.0001 y revise los totales antes de emitir.");
            return new[] { PrecioBase, DescuentoBase };
        }
        public static DataTable VentasFuncCrearDetalle()
        {
            var Tabla = new DataTable("Detalle");
            Tabla.Columns.Add("Codigo", typeof(int)); Tabla.Columns.Add("Producto", typeof(string));
            foreach (string NombreColumna in new[] { "Cantidad", "PrecioUnitario", "Descuento", "Tasa", "Subtotal", "Impuesto", "Total" }) Tabla.Columns.Add(NombreColumna, typeof(decimal));
            return Tabla;
        }
        public static void VentasMetRecalcular(DataTable Detalle)
        {
            if (Detalle == null || Detalle.Rows.Count == 0) throw new ArgumentException("Agregue al menos un producto.");
            foreach (DataRow Fila in Detalle.Rows)
            {
                var Importes = VentasFuncCalcular(Convert.ToDecimal(Fila["Cantidad"]), Convert.ToDecimal(Fila["PrecioUnitario"]), Convert.ToDecimal(Fila["Descuento"]), Convert.ToDecimal(Fila["Tasa"]));
                Fila["Subtotal"] = Importes[0]; Fila["Impuesto"] = Importes[1]; Fila["Total"] = Importes[2];
            }
        }
    }

    public class ClsSolicitudFactura
    {
        public int Cliente, Usuario, Sucursal, Empresa, OrigenId, FormaPago;
        public long Moneda;
        public bool Pedido;
        public string Condicion, ReferenciaPago;
        public DataTable Detalle;
        public DataTable Distribucion;
        public string Uuid = Guid.NewGuid().ToString().ToUpperInvariant();
    }

    public class ClsFacturaRegistrada
    {
        public int Id;
        public string Uuid, Serie, Numero, Moneda, Cliente, Identificacion, Direccion, Empresa, NitEmisor, DireccionEmisor, Condicion;
        public DateTime Fecha;
        public DateTime FechaVencimiento;
        public DataTable Detalle;
    }

    public sealed partial class ClsFacturaOrigenRepositorio
    {
        public DataTable VentasFuncMonedas() { using (var Datos = new ClsDatosVentas()) return Datos.VentasFuncTabla("SELECT idMoneda Id,CONCAT(codigoMoneda,' - ',nombreMoneda) Nombre,codigoMoneda Codigo FROM tblMoneda WHERE is_active=1 ORDER BY codigoMoneda"); }
        public DataTable VentasFuncEmpresas() { using (var Datos = new ClsDatosVentas()) return Datos.VentasFuncTabla("SELECT idEmpresa Id,razonSocialEmpresa Nombre FROM tblEmpresa WHERE is_active=1 ORDER BY idEmpresa"); }
        public DataTable VentasFuncFormasPago() { using (var Datos = new ClsDatosVentas()) return Datos.VentasFuncTabla("SELECT idFormaPagoVenta Id,nombreFormaPagoVenta Nombre,requiereReferenciaFormaPagoVenta RequiereReferencia FROM tblFormaPagoVenta ORDER BY nombreFormaPagoVenta"); }
        private const string _ClienteSql = @"SELECT c.idCliente Id,c.codigoCliente Codigo,c.tipoIdentificacionCliente Tipo,
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
        public DataTable VentasFuncBuscarCliente(string Identificacion, bool Codigo)
        {
            Identificacion = ClsReglasFactura.VentasFuncNormalizarIdentificacion(Identificacion);
            using (var Datos = new ClsDatosVentas())
                return Codigo ? Datos.VentasFuncTabla(_ClienteSql + " AND c.codigoCliente=?", Identificacion)
                    : Datos.VentasFuncTabla(_ClienteSql + " AND ((?='CF' AND c.tipoIdentificacionCliente='CF') OR REPLACE(c.nitCliente,'-','')=REPLACE(?,'-','') OR c.dpiCliente=?)", Identificacion, Identificacion, Identificacion);
        }
        private static decimal VentasFuncTasa(ClsDatosVentas Datos)
        {
            var Tabla = Datos.VentasFuncTabla("SELECT valorImpuesto FROM tblImpuesto WHERE UPPER(nombreImpuesto)='IVA' AND operacionImpuesto IN ('VENTA','AMBAS')");
            if (Tabla.Rows.Count != 1) throw new InvalidOperationException("Debe existir un único impuesto IVA aplicable a ventas.");
            decimal Tasa = Convert.ToDecimal(Tabla.Rows[0][0]); ClsReglasFactura.VentasFuncCalcular(1, 1, 0, Tasa); return Tasa;
        }
        private const string _ProductosSql = @"SELECT p.idProducto Codigo,p.nombreProducto Producto,d.precioUnitarioListaPrecioDetalle Precio,l.idMoneda Moneda,
            COALESCE((SELECT SUM(GREATEST(0,LEAST(s.stockActualStock,s.stockFisicoStock)-s.apartadosStock)) FROM tblStock s WHERE s.Fk_Producto_Codigo=p.idProducto AND s.estadoVentaStock=1),0) Disponible
            FROM tblClienteListaPrecio a JOIN tblListaPrecioEncabezado l ON l.idListaPrecioEncabezado=a.idListaPrecioEncabezado
            JOIN tblListaPrecioDetalle d ON d.idListaPrecioEncabezado=l.idListaPrecioEncabezado AND d.activoListaPrecioDetalle=1
            JOIN tblProducto p ON p.idProducto=d.idProducto JOIN tblProductoVenta v ON v.idProductoVenta=p.idProducto AND v.activoProductoVenta=1
            WHERE a.idCliente=? AND l.activoListaPrecioEncabezado=1 AND CURRENT_DATE>=l.vigenteDesdeListaPrecioEncabezado
            AND (l.vigenteHastaListaPrecioEncabezado IS NULL OR CURRENT_DATE<=l.vigenteHastaListaPrecioEncabezado) ORDER BY p.idProducto";
        private static DataTable VentasFuncProductos(ClsDatosVentas Datos, int Cliente)
        {
            var Tabla = Datos.VentasFuncTabla(_ProductosSql, Cliente); Tabla.Columns.Add("Tasa", typeof(decimal));
            decimal Tasa = VentasFuncTasa(Datos); foreach (DataRow Fila in Tabla.Rows) Fila["Tasa"] = Tasa; return Tabla;
        }
        public DataTable VentasFuncProductos(int Cliente) { using (var Datos = new ClsDatosVentas()) return VentasFuncProductos(Datos, Cliente); }
        public DataTable VentasFuncExistencias(int Producto)
        {
            using (var Datos = new ClsDatosVentas()) return Datos.VentasFuncTabla(@"SELECT s.idStock StockId,b.nombreBodega Bodega,s.Fk_Bodega_Codigo BodegaId,
                GREATEST(0,LEAST(s.stockActualStock,s.stockFisicoStock)-s.apartadosStock) Disponible
                FROM tblStock s JOIN tblBodega b ON b.idBodega=s.Fk_Bodega_Codigo
                WHERE s.Fk_Producto_Codigo=? AND s.estadoVentaStock=1 ORDER BY s.idStock", Producto);
        }
        public DataTable VentasFuncPrepararDetalleOrigen(bool Pedido, int Id)
        {
            using (var Datos = new ClsDatosVentas())
            {
                var Doc = VentasFuncDocumento(Datos, Pedido, Id); var Tabla = ClsReglasFactura.VentasFuncCrearDetalle(); decimal Tasa = VentasFuncTasa(Datos);
                foreach (DataRow Fila in Doc.Tables[1].Rows) Tabla.Rows.Add(Fila["ProductoId"],Fila["Producto"],Fila["Cantidad"],Fila["Precio"],Fila["Descuento"],Tasa,0,0,0);
                ClsReglasFactura.VentasMetRecalcular(Tabla); return Tabla;
            }
        }
        public ClsFacturaRegistrada VentasFuncRegistrar(ClsSolicitudFactura Solicitud, Action<ClsFacturaRegistrada> GuardarPdf)
        {
            if (Solicitud == null || GuardarPdf == null) throw new ArgumentNullException();
            if (Solicitud.Condicion != "CONTADO" && Solicitud.Condicion != "CREDITO") throw new ArgumentException("Seleccione contado o crédito.");
            Guid Uuid; if (!Guid.TryParse(Solicitud.Uuid, out Uuid)) throw new ArgumentException("UUID inválido.");
            var Detalle = Solicitud.Detalle.Copy(); ClsReglasFactura.VentasMetRecalcular(Detalle);
            using (var Datos = new ClsDatosVentas())
            {
                Datos.VentasMetIniciar();
                if (Convert.ToInt32(Datos.VentasFuncEscalar("SELECT COUNT(*) FROM tblUsuario WHERE idUsuario=? AND is_active=1",Solicitud.Usuario)) != 1) throw new InvalidOperationException("Se requiere una sesión activa.");
                if (Convert.ToInt32(Datos.VentasFuncEscalar("SELECT COUNT(*) FROM tblSucursal WHERE idSucursal=?",Solicitud.Sucursal)) != 1) throw new ArgumentException("Seleccione la sucursal emisora.");
                Datos.VentasFuncEjecutar("INSERT INTO tblControlPeriodoCxC(idMoneda) VALUES (?) ON DUPLICATE KEY UPDATE idMoneda=VALUES(idMoneda)",Solicitud.Moneda);
                var Cierre = Datos.VentasFuncEscalar("SELECT fechaUltimoCierreControlPeriodoCxC FROM tblControlPeriodoCxC WHERE idMoneda=? FOR UPDATE",Solicitud.Moneda);
                if (Cierre != null && Cierre != DBNull.Value && DateTime.Today <= Convert.ToDateTime(Cierre).Date) throw new InvalidOperationException("El período de esta moneda está cerrado.");
                Datos.VentasFuncEscalar("SELECT idCliente FROM tblCliente WHERE idCliente=? FOR UPDATE",Solicitud.Cliente);
                var Clientes = Datos.VentasFuncTabla(_ClienteSql + " AND c.idCliente=?",Solicitud.Cliente);
                if (Clientes.Rows.Count != 1) throw new InvalidOperationException("Cliente inactivo o inexistente.");
                var Cliente = Clientes.Rows[0]; object Cotizacion = DBNull.Value, PedidoId = DBNull.Value, Vendedor = DBNull.Value;
                decimal Tasa = VentasFuncTasa(Datos);
                if (Solicitud.OrigenId > 0)
                {
                    Datos.VentasFuncEscalar(Solicitud.Pedido ? "SELECT idPedidoEncabezado FROM tblPedidoEncabezado WHERE idPedidoEncabezado=? FOR UPDATE" : "SELECT idCotizacionEncabezado FROM tblCotizacionEncabezado WHERE idCotizacionEncabezado=? FOR UPDATE",Solicitud.OrigenId);
                    var Origen = VentasFuncDocumento(Datos,Solicitud.Pedido,Solicitud.OrigenId); var Encabezado=Origen.Tables[0].Rows[0];
                    if (Convert.ToInt32(Encabezado["ClienteId"])!=Solicitud.Cliente || Convert.ToInt64(Encabezado["MonedaId"])!=Solicitud.Moneda) throw new InvalidOperationException("Cambió el documento de origen. Vuelva a cargarlo.");
                    Cotizacion=Encabezado["CotizacionId"]; PedidoId=Encabezado["PedidoId"]; Vendedor=Encabezado["VendedorId"];
                    // Se permite quitar productos, pero no cambiar las condiciones del documento original.
                    var Pendientes = Origen.Tables[1].AsEnumerable().ToList();
                    foreach(DataRow Fila in Detalle.Rows)
                    {
                        var Original=Pendientes.FirstOrDefault(X=>Convert.ToInt32(X["ProductoId"])==Convert.ToInt32(Fila["Codigo"]) && Convert.ToDecimal(X["Precio"])==Convert.ToDecimal(Fila["PrecioUnitario"]) && Convert.ToDecimal(X["Cantidad"])==Convert.ToDecimal(Fila["Cantidad"]) && Convert.ToDecimal(X["Descuento"])==Convert.ToDecimal(Fila["Descuento"]));
                        if(Original==null) throw new InvalidOperationException("Cambió el detalle de origen. Vuelva a cargarlo.");
                        Pendientes.Remove(Original);
                    }
                }
                else
                {
                    if(Cliente["Lista"]==DBNull.Value || Convert.ToInt64(Cliente["Moneda"])!=Solicitud.Moneda) throw new InvalidOperationException("El cliente necesita una lista vigente en la moneda seleccionada.");
                    var _Productos=VentasFuncProductos(Datos,Solicitud.Cliente);
                    foreach(DataRow Fila in Detalle.Rows)
                    {
                        var Producto=_Productos.AsEnumerable().SingleOrDefault(X=>Convert.ToInt32(X["Codigo"])==Convert.ToInt32(Fila["Codigo"]));
                        if(Producto==null || Convert.ToDecimal(Producto["Precio"])!=Convert.ToDecimal(Fila["PrecioUnitario"])) throw new InvalidOperationException("Cambió la lista de precios. Actualice los productos antes de emitir.");
                    }
                }
                foreach(DataRow Fila in Detalle.Rows) if(Convert.ToDecimal(Fila["Tasa"])!=Tasa) throw new InvalidOperationException("Cambió el impuesto IVA. Actualice el detalle antes de emitir.");
                decimal Total=Detalle.AsEnumerable().Sum(Fila=>Convert.ToDecimal(Fila["Total"]));
                if(Total<=0) throw new ArgumentException("El total de la factura debe ser mayor a cero.");
                if(Solicitud.Condicion=="CREDITO")
                {
                    if(Convert.ToString(Cliente["Tipo"])=="CF" || Convert.ToInt32(Cliente["Dias"])<=0 || Convert.ToInt64(Cliente["MonedaCredito"])!=Solicitud.Moneda) throw new InvalidOperationException("El cliente no tiene crédito habilitado en esta moneda.");
                    decimal Saldo=Convert.ToDecimal(Datos.VentasFuncEscalar(@"SELECT COALESCE(SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC),0) FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=m.idFacturaEncabezado JOIN tblClienteFacturaVersion v ON v.idClienteFacturaVersion=f.idClienteFacturaVersion WHERE v.idCliente=? AND f.idMoneda=?",Solicitud.Cliente,Solicitud.Moneda));
                    if(Saldo+Total>Convert.ToDecimal(Cliente["Limite"])) throw new InvalidOperationException("La factura supera el límite de crédito disponible.");
                }
                var Moneda=Datos.VentasFuncTabla("SELECT codigoMoneda FROM tblMoneda WHERE idMoneda=? AND is_active=1",Solicitud.Moneda);
                var Empresa=Datos.VentasFuncTabla("SELECT razonSocialEmpresa,nitEmpresa,direccionEmpresa FROM tblEmpresa WHERE idEmpresa=? AND is_active=1",Solicitud.Empresa);
                if(Moneda.Rows.Count!=1 || Empresa.Rows.Count!=1) throw new InvalidOperationException("Seleccione moneda y empresa activas.");
                var Factura=new ClsFacturaRegistrada { Uuid=Solicitud.Uuid,Numero=Datos.VentasFuncNumero("FVE"),Fecha=DateTime.Now,Detalle=Detalle,
                    Moneda=Convert.ToString(Moneda.Rows[0][0]),Cliente=Convert.ToString(Cliente["Nombre"]),Identificacion=Convert.ToString(Cliente["Identificacion"]),Direccion=Convert.ToString(Cliente["Direccion"]),
                    Empresa=Convert.ToString(Empresa.Rows[0][0]),NitEmisor=Convert.ToString(Empresa.Rows[0][1]),DireccionEmisor=Convert.ToString(Empresa.Rows[0][2]),Condicion=Solicitud.Condicion };
                Factura.FechaVencimiento=Factura.Fecha.Date.AddDays(Solicitud.Condicion=="CREDITO"?Convert.ToInt32(Cliente["Dias"]):0);
                Factura.Serie="S-"+Factura.Numero.Substring(4); // Correlativo protegido por bloqueo, único también para la serie.
                Datos.VentasFuncEjecutar(@"INSERT INTO tblClienteFacturaVersion(idCliente,numeroVersionClienteFacturaVersion,tipoIdentificacionClienteFacturaVersion,nitClienteFacturaVersion,dpiClienteFacturaVersion,nombreClienteFacturaVersion,direccionClienteFacturaVersion)
                    SELECT c.idCliente,COALESCE((SELECT MAX(v.numeroVersionClienteFacturaVersion)+1 FROM tblClienteFacturaVersion v WHERE v.idCliente=c.idCliente),1),c.tipoIdentificacionCliente,c.nitCliente,c.dpiCliente,?,? FROM tblCliente c WHERE c.idCliente=?",Factura.Cliente,Factura.Direccion,Solicitud.Cliente);
                int Version=Datos.VentasFuncObtenerId();
                Datos.VentasFuncEjecutar(@"INSERT INTO tblFacturaEncabezado(uuidDteFacturaEncabezado,serieFacturaEncabezado,numeroFacturaEncabezado,idSucursal,idClienteFacturaVersion,idVendedor,idMoneda,idCotizacionEncabezado,idPedidoEncabezado,fechaEmisionFacturaEncabezado,fechaVencimientoFacturaEncabezado,condicionPagoFacturaEncabezado,estadoFacturaEncabezado,subtotalFacturaEncabezado,impuestoIvaFacturaEncabezado,totalFacturaEncabezado,idUsuarioRegistro,observacionesFacturaEncabezado)
                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,'EMITIDA',?,?,?,?,?)",Factura.Uuid,Factura.Serie,Factura.Numero,Solicitud.Sucursal,Version,Vendedor,Solicitud.Moneda,Cotizacion,PedidoId,Factura.Fecha,Factura.FechaVencimiento,Solicitud.Condicion,
                    Detalle.AsEnumerable().Sum(Fila=>Convert.ToDecimal(Fila["Subtotal"])),Detalle.AsEnumerable().Sum(Fila=>Convert.ToDecimal(Fila["Impuesto"])),Total,Solicitud.Usuario,"Emisión local sin certificación FEL. Empresa: "+Factura.Empresa+" | NIT: "+Factura.NitEmisor);
                Factura.Id=Datos.VentasFuncObtenerId();
                Factura.FechaVencimiento=Convert.ToDateTime(Datos.VentasFuncEscalar("SELECT fechaVencimientoFacturaEncabezado FROM tblFacturaEncabezado WHERE idFacturaEncabezado=?",Factura.Id));int Linea=0;
                foreach(DataRow Fila in Detalle.Rows)
                {
                    decimal[] BasePersistida=ClsReglasFactura.VentasFuncConvertirDetallePersistido(Convert.ToDecimal(Fila["Cantidad"]),Convert.ToDecimal(Fila["PrecioUnitario"]),Convert.ToDecimal(Fila["Descuento"]),Convert.ToDecimal(Fila["Tasa"]));
                    Datos.VentasFuncEjecutar(@"INSERT INTO tblFacturaDetalle(idFacturaEncabezadoUuidDte,numeroLineaFacturaDetalle,idProducto,cantidadFacturaDetalle,precioUnitarioFacturaDetalle,descuentoMontoFacturaDetalle,tasaImpuestoFacturaDetalle) VALUES (?,?,?,?,?,?,?)",Factura.Uuid,++Linea,Fila["Codigo"],Fila["Cantidad"],BasePersistida[0],BasePersistida[1],Fila["Tasa"]);
                }
                VentasMetDescontarStock(Datos,Solicitud, Factura);
                Datos.VentasFuncEjecutar("INSERT INTO tblMovimientoCxC(idFacturaEncabezado,fechaMovimientoCxC,tipoMovimientoCxC,descripcionMovimientoCxC,cargoMovimientoCxC,idUsuarioRegistro) VALUES (?,?,'FACTURA',?,?,?)",Factura.Id,Factura.Fecha.Date,Factura.Numero,Total,Solicitud.Usuario);
                if(Solicitud.Condicion=="CONTADO") VentasMetRegistrarPago(Datos,Solicitud,Factura,Total);
                if(Cotizacion!=DBNull.Value) Datos.VentasFuncEjecutar("UPDATE tblCotizacionEncabezado SET estadoCotizacionEncabezado='ACEPTADA' WHERE idCotizacionEncabezado=?",Cotizacion);
                if(PedidoId!=DBNull.Value) Datos.VentasFuncEjecutar("UPDATE tblPedidoEncabezado SET estadoPedidoEncabezado='COMPLETADO' WHERE idPedidoEncabezado=?",PedidoId);
                Datos.VentasMetAuditar("FACTURA",Factura.Id,"GUARDAR",Factura.Numero+" | "+Factura.Uuid,Solicitud.Usuario);
                GuardarPdf(Factura); // Si falla o cancela la escritura, Dispose revierte toda la operación.
                Datos.VentasMetConfirmar();return Factura;
            }
        }
        private static void VentasMetRegistrarPago(ClsDatosVentas Datos,ClsSolicitudFactura Solicitud,ClsFacturaRegistrada Factura,decimal Total)
        {
            var Forma=Datos.VentasFuncTabla("SELECT requiereReferenciaFormaPagoVenta FROM tblFormaPagoVenta WHERE idFormaPagoVenta=?",Solicitud.FormaPago);
            if(Forma.Rows.Count!=1 || (Convert.ToBoolean(Forma.Rows[0][0]) && string.IsNullOrWhiteSpace(Solicitud.ReferenciaPago))) throw new ArgumentException("Seleccione una forma de pago y su referencia cuando corresponda.");
            Datos.VentasFuncEjecutar(@"INSERT INTO tblReciboPago(numeroReciboPago,idCliente,idMoneda,fechaReciboPago,montoReciboPago,referenciaReciboPago,estadoReciboPago,idUsuarioRegistro,idFormaPagoVenta) VALUES (?,?,?,?,?,?,'CONFIRMADO',?,?)",Datos.VentasFuncNumero("REC"),Solicitud.Cliente,Solicitud.Moneda,Factura.Fecha.Date,Total,Solicitud.ReferenciaPago,Solicitud.Usuario,Solicitud.FormaPago);
            int Recibo=Datos.VentasFuncObtenerId();
            Datos.VentasFuncEjecutar("INSERT INTO tblAplicacionPago(idReciboPago,idFacturaEncabezado,fechaAplicacionPago,montoAplicadoAplicacionPago,estadoAplicacionPago,idUsuarioRegistro) VALUES (?,?,?,?,'APLICADA',?)",Recibo,Factura.Id,Factura.Fecha.Date,Total,Solicitud.Usuario);
            int Aplicacion=Datos.VentasFuncObtenerId();
            Datos.VentasFuncEjecutar("INSERT INTO tblMovimientoCxC(idFacturaEncabezado,idAplicacionPago,fechaMovimientoCxC,tipoMovimientoCxC,descripcionMovimientoCxC,abonoMovimientoCxC,idUsuarioRegistro) VALUES (?,?,?,'PAGO',?,?,?)",Factura.Id,Aplicacion,Factura.Fecha.Date,"Pago contado "+Factura.Numero,Total,Solicitud.Usuario);
        }
        private static void VentasMetDescontarStock(ClsDatosVentas Datos,ClsSolicitudFactura Solicitud,ClsFacturaRegistrada Factura)
        {
            if(Solicitud.Distribucion==null) throw new ArgumentException("Seleccione las bodegas de salida.");
            var Requerido=Factura.Detalle.AsEnumerable().GroupBy(Fila=>Convert.ToInt32(Fila["Codigo"])).ToDictionary(G=>G.Key,G=>G.Sum(Fila=>Convert.ToDecimal(Fila["Cantidad"])));
            foreach(var Par in Requerido)
                if(Solicitud.Distribucion.AsEnumerable().Where(Fila=>Convert.ToInt32(Fila["ProductoId"])==Par.Key).Sum(Fila=>Convert.ToDecimal(Fila["Cantidad"]))!=Par.Value) throw new ArgumentException("La distribución por bodegas no coincide con la cantidad facturada.");
            var Tipo=Datos.VentasFuncEscalar("SELECT idTipoMovimiento FROM tblTipoMovimiento WHERE nombreTipoMovimiento='Salida por facturación' AND signoTipoMovimiento='-'");
            if(Tipo==null || Tipo==DBNull.Value) throw new InvalidOperationException("Configure el tipo de movimiento Salida por facturación.");
            foreach(DataRow Fila in Solicitud.Distribucion.Select("", "StockId ASC"))
            {
                int Producto=Convert.ToInt32(Fila["ProductoId"]);decimal Cantidad=Convert.ToDecimal(Fila["Cantidad"]);
                if(!Requerido.ContainsKey(Producto) || Cantidad<=0 || Cantidad!=decimal.Truncate(Cantidad)) throw new ArgumentException("Distribución de inventario inválida.");
                var Stock=Datos.VentasFuncTabla("SELECT s.*,p.precioCostoProducto FROM tblStock s JOIN tblProducto p ON p.idProducto=s.Fk_Producto_Codigo WHERE s.idStock=? AND s.Fk_Producto_Codigo=? AND s.estadoVentaStock=1 FOR UPDATE",Fila["StockId"],Producto);
                if(Stock.Rows.Count!=1) throw new InvalidOperationException("La bodega ya no está disponible.");
                var St=Stock.Rows[0];
                decimal Disponible=Math.Min(Convert.ToDecimal(St["stockActualStock"]),Convert.ToDecimal(St["stockFisicoStock"]))-Convert.ToDecimal(St["apartadosStock"]);
                if(Cantidad>Disponible) throw new InvalidOperationException("Existencias insuficientes. Vuelva a distribuir las bodegas.");
                Datos.VentasFuncEjecutar(@"UPDATE tblStock SET stockAnteriorStock=stockActualStock,stockActualStock=stockActualStock-?,stockFisicoStock=stockFisicoStock-?,stockVirtualStock=GREATEST(0,stockVirtualStock-?),abonosMesStock=abonosMesStock+?,abonosAcumuladosStock=abonosAcumuladosStock+? WHERE idStock=?",Cantidad,Cantidad,Cantidad,Cantidad,Cantidad,Fila["StockId"]);
                Datos.VentasFuncEjecutar("INSERT INTO tblMovimientoInventario(Fk_Tipo_Movimiento_Codigo,Fk_Bodega_Origen_Codigo,observacionesMovimientoInventario,estadoMovimientoInventario) VALUES (?,?,?,'CUADRADO')",Tipo,St["Fk_Bodega_Codigo"],"Factura "+Factura.Numero+" | UUID "+Factura.Uuid);
                int Movimiento=Datos.VentasFuncObtenerId();
                Datos.VentasFuncEjecutar("INSERT INTO tblDetalleMovimiento(Fk_Movimiento_Inventario_Codigo,Fk_Producto_Codigo,cantidadDetalleMovimiento,precioCostoDetalleMovimiento) VALUES (?,?,?,?)",Movimiento,Producto,Cantidad,St["precioCostoProducto"]);
            }
        }
    }
}
