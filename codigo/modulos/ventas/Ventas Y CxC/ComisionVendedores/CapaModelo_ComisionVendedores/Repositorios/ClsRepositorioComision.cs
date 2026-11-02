using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using CapaModelo_ComisionVendedores.Contratos;
using CapaModelo_ComisionVendedores.Entidades;

namespace CapaModelo_ComisionVendedores.Repositorios
{
    public class ClsRepositorioComision : ClsSentencias, IRepositorioComision
    {
        public List<ClsFacturaComision> ComisionVendedoresMetObtenerFacturas()
        {
            const string Comando = @"
                SELECT f.Pk_FacturaEncabezado AS id,
                       COALESCE(f.Fk_Vendedor_Codigo,0) AS id_vendedor,
                       f.fechaEmisionFacturaEncabezado AS fecha,
                       COALESCE(cfv.nombreClienteFacturaVersion,'') AS cliente,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle)
                           FROM tblFacturaDetalle fd
                           WHERE fd.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado),0) AS total,
                       0 AS total_pagado, 0 AS ultimo_abono
                FROM tblFacturaEncabezado f
                LEFT JOIN tblClienteFacturaVersion cfv ON cfv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
                INNER JOIN tblMoneda m ON m.Pk_Moneda=f.Fk_Moneda_Codigo
                WHERE NOT EXISTS (
                    SELECT 1 FROM tblComisionFacturaEncabezado cf
                    WHERE cf.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado)
                ORDER BY f.fechaEmisionFacturaEncabezado DESC, f.Pk_FacturaEncabezado DESC";
            DataTable Datos = ComisionVendedoresMetEjecucionConsulta(Comando, new List<OdbcParameter>());
            if (Datos.Rows.Count == 0)
            {
                DataTable BaseDatos = ComisionVendedoresMetEjecucionConsulta(
                    "SELECT DATABASE() AS base_actual, @@hostname AS servidor, @@port AS puerto, CURRENT_USER() AS usuario_mysql",
                    new List<OdbcParameter>());
                string NombreBase = BaseDatos.Rows.Count == 0 ? "desconocida" : Convert.ToString(BaseDatos.Rows[0]["base_actual"]);
                string Servidor = BaseDatos.Rows.Count == 0 ? "desconocido" : Convert.ToString(BaseDatos.Rows[0]["servidor"]);
                string Puerto = BaseDatos.Rows.Count == 0 ? "desconocido" : Convert.ToString(BaseDatos.Rows[0]["puerto"]);
                string Usuario = BaseDatos.Rows.Count == 0 ? "desconocido" : Convert.ToString(BaseDatos.Rows[0]["usuario_mysql"]);
                throw new InvalidOperationException("La conexión ODBC llegó a '" + Servidor + ":" + Puerto + "/" + NombreBase + "' como '" + Usuario + "', pero tblFacturaEncabezado no contiene facturas. Compare servidor y puerto con Workbench.");
            }
            return ComisionVendedoresMetMapearFacturas(Datos);
        }

        public List<ClsVendedorComision> ComisionVendedoresMetObtenerVendedores()
        {
            const string Comando = @"
                SELECT v.Pk_Vendedor AS id,
                       COALESCE(NULLIF(TRIM(CONCAT_WS(' ', e.nombresEmpleado, e.apellidosEmpleado)), ''), v.codigoVendedor) AS nombre,
                       0 AS porcentaje
                FROM tblVendedor v
                LEFT JOIN tblEmpleado e ON e.idEmpleado = v.Fk_Empleado_Codigo
                WHERE v.activoVendedor = 1 ORDER BY nombre";
            DataTable Datos = ComisionVendedoresMetEjecucionConsulta(Comando, new List<OdbcParameter>());
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

        public List<ClsFacturaPago> ComisionVendedoresMetObtenerFacturasConSaldo()
        {
            // Dejar al día los abonos de comisión antes de mostrar el estimado en el formulario.
            ComisionVendedoresMetSincronizarAbonos(null);
            const string Comando = @"
                SELECT f.Pk_FacturaEncabezado AS id_factura,
                       cfv.Fk_Cliente_Codigo AS id_cliente,
                       f.Fk_Moneda_Codigo AS id_moneda,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       f.fechaEmisionFacturaEncabezado AS fecha,
                       CONCAT('FAC-',f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado,' | ',
                              COALESCE(cfv.nombreClienteFacturaVersion,''),' | ',m.simboloMoneda) AS etiqueta,
                       COALESCE(ft.total,0) AS total,
                       COALESCE(pa.pagado,0) AS pagado,
                       GREATEST(COALESCE(mc.saldo,COALESCE(ft.total,0)-COALESCE(pa.pagado,0)),0) AS saldo,
                       COALESCE(cc.total_comision,0) AS total_comision,
                       COALESCE(ca.comision_devengada,0) AS comision_devengada
                FROM tblFacturaEncabezado f
                INNER JOIN tblClienteFacturaVersion cfv ON cfv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
                INNER JOIN tblMoneda m ON m.Pk_Moneda=f.Fk_Moneda_Codigo
                LEFT JOIN (SELECT Fk_FacturaEncabezado_Codigo,SUM(totalLineaFacturaDetalle) AS total
                           FROM tblFacturaDetalle GROUP BY Fk_FacturaEncabezado_Codigo) ft
                       ON ft.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                LEFT JOIN (SELECT ap.Fk_FacturaEncabezado_Codigo,SUM(ap.montoAplicadoAplicacionPago) AS pagado
                           FROM tblAplicacionPago ap INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                           WHERE ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'
                           GROUP BY ap.Fk_FacturaEncabezado_Codigo) pa
                       ON pa.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                LEFT JOIN (SELECT Fk_FacturaEncabezado_Codigo,
                                  SUM(cargoMovimientoCxC-abonoMovimientoCxC) AS saldo
                           FROM tblMovimientoCxC GROUP BY Fk_FacturaEncabezado_Codigo) mc
                       ON mc.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                LEFT JOIN (SELECT cf.Fk_FacturaEncabezado_Codigo,
                                  SUM(cd.baseComisionFacturaDetalle*cd.tasaAplicadaComisionFacturaDetalle) AS total_comision
                           FROM tblComisionFacturaEncabezado cf INNER JOIN tblComisionFacturaDetalle cd
                             ON cd.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado
                           WHERE cf.estadoComisionFacturaEncabezado='CALCULADA'
                           GROUP BY cf.Fk_FacturaEncabezado_Codigo) cc
                       ON cc.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                LEFT JOIN (SELECT cf.Fk_FacturaEncabezado_Codigo,
                                  SUM(ca.montoComisionDevengadoComisionAbono) AS comision_devengada
                           FROM tblComisionFacturaEncabezado cf INNER JOIN tblComisionAbono ca
                             ON ca.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado
                           WHERE ca.estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')
                           GROUP BY cf.Fk_FacturaEncabezado_Codigo) ca
                       ON ca.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                WHERE f.estadoFacturaEncabezado='EMITIDA'
                  AND GREATEST(COALESCE(mc.saldo,COALESCE(ft.total,0)-COALESCE(pa.pagado,0)),0)>0
                ORDER BY f.fechaEmisionFacturaEncabezado,f.Pk_FacturaEncabezado";
            DataTable Datos = ComisionVendedoresMetEjecucionConsulta(Comando, new List<OdbcParameter>());
            List<ClsFacturaPago> Resultado = new List<ClsFacturaPago>();
            foreach (DataRow Fila in Datos.Rows)
            {
                decimal Total = Convert.ToDecimal(Fila["total"]);
                decimal Pagado = Convert.ToDecimal(Fila["pagado"]);
                Resultado.Add(new ClsFacturaPago
                {
                    IdFactura = Convert.ToInt32(Fila["id_factura"]),
                    IdCliente = Convert.ToInt32(Fila["id_cliente"]),
                    IdMoneda = Convert.ToInt64(Fila["id_moneda"]),
                    Fecha = Convert.ToDateTime(Fila["fecha"]),
                    Etiqueta = Convert.ToString(Fila["etiqueta"]),
                    SimboloMoneda = Convert.ToString(Fila["simbolo_moneda"]),
                    Total = Total,
                    TotalPagado = Pagado,
                    Saldo = Convert.ToDecimal(Fila["saldo"]),
                    ComisionTotal = Convert.ToDecimal(Fila["total_comision"]),
                    ComisionDevengada = Convert.ToDecimal(Fila["comision_devengada"])
                });
            }
            return Resultado;
        }

        public List<ClsFormaPagoVenta> ComisionVendedoresMetObtenerFormasPagoVenta()
        {
            const string Comando = @"SELECT Pk_FormaPagoVenta AS id,nombreFormaPagoVenta AS nombre,
                                           requiereReferenciaFormaPagoVenta AS requiere_referencia
                                    FROM tblFormaPagoVenta ORDER BY nombreFormaPagoVenta";
            DataTable Datos = ComisionVendedoresMetEjecucionConsulta(Comando, new List<OdbcParameter>());
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

        public decimal ComisionVendedoresMetRegistrarAbonoFactura(int IdFactura, decimal Monto,
            int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario)
        {
            using (OdbcConnection Conexion = ComisionVendedoresMetObtenerConexion())
            {
                ComisionVendedoresMetAbrirConexion(Conexion);
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        int IdCliente;
                        long IdMoneda;
                        string SimboloMoneda;
                        decimal TotalFactura;
                        decimal SaldoFactura;
                        bool TieneMovimientosCxC;
                        string EstadoFactura;
                        using (OdbcCommand BloquearFactura = new OdbcCommand(@"
                            SELECT f.estadoFacturaEncabezado,f.Fk_Moneda_Codigo,COALESCE(m.simboloMoneda,''),
                                   cfv.Fk_Cliente_Codigo,
                                   EXISTS(SELECT 1 FROM tblMovimientoCxC mc
                                          WHERE mc.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado) AS tiene_movimientos_cxc,
                                   COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd
                                       WHERE fd.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado),0) AS total_factura,
                                   GREATEST(COALESCE(
                                       (SELECT SUM(mc.cargoMovimientoCxC-mc.abonoMovimientoCxC)
                                        FROM tblMovimientoCxC mc WHERE mc.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado),
                                       COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd
                                                 WHERE fd.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado),0)
                                       - COALESCE((SELECT SUM(ap.montoAplicadoAplicacionPago) FROM tblAplicacionPago ap
                                                   INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                                                   WHERE ap.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                                                     AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'),0)
                                   ),0) AS saldo_factura
                            FROM tblFacturaEncabezado f
                            INNER JOIN tblClienteFacturaVersion cfv ON cfv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
                            INNER JOIN tblMoneda m ON m.Pk_Moneda=f.Fk_Moneda_Codigo
                            WHERE f.Pk_FacturaEncabezado=? FOR UPDATE", Conexion, Transaccion))
                        {
                            BloquearFactura.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            using (OdbcDataReader Lector = BloquearFactura.ExecuteReader())
                            {
                                if (!Lector.Read()) throw new InvalidOperationException("La factura no existe o no tiene datos de cliente.");
                                EstadoFactura = Convert.ToString(Lector["estadoFacturaEncabezado"]);
                                IdMoneda = Convert.ToInt64(Lector["Fk_Moneda_Codigo"]);
                                SimboloMoneda = Convert.ToString(Lector[2]);
                                IdCliente = Convert.ToInt32(Lector["Fk_Cliente_Codigo"]);
                                TieneMovimientosCxC = Convert.ToInt32(Lector["tiene_movimientos_cxc"]) != 0;
                                TotalFactura = Convert.ToDecimal(Lector["total_factura"]);
                                SaldoFactura = Convert.ToDecimal(Lector["saldo_factura"]);
                            }
                        }
                        if (!String.Equals(EstadoFactura, "EMITIDA", StringComparison.OrdinalIgnoreCase))
                            throw new InvalidOperationException("Solo se reciben abonos de facturas emitidas.");
                        decimal Saldo = SaldoFactura;
                        if (TotalFactura <= 0m) throw new InvalidOperationException("La factura no tiene un total válido.");
                        if (Monto > Saldo) throw new InvalidOperationException("El abono excede el saldo pendiente de " + (SimboloMoneda ?? String.Empty) + " " + Saldo.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ".");

                        bool RequiereReferencia;
                        using (OdbcCommand Forma = new OdbcCommand("SELECT requiereReferenciaFormaPagoVenta FROM tblFormaPagoVenta WHERE Pk_FormaPagoVenta=?", Conexion, Transaccion))
                        {
                            Forma.Parameters.Add(new OdbcParameter("Forma", OdbcType.Int) { Value = IdFormaPago });
                            object Valor = Forma.ExecuteScalar();
                            if (Valor == null || Valor == DBNull.Value) throw new InvalidOperationException("El método de pago seleccionado ya no existe.");
                            RequiereReferencia = Convert.ToBoolean(Valor);
                        }
                        if (RequiereReferencia && String.IsNullOrWhiteSpace(Referencia))
                            throw new InvalidOperationException("El método de pago seleccionado requiere número de referencia.");

                        string NumeroRecibo = "CV" + DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) +
                            Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
                        using (OdbcCommand Recibo = new OdbcCommand(@"
                            INSERT INTO tblReciboPago
                                (numeroReciboPago,Fk_Cliente_Codigo,Fk_Moneda_Codigo,fechaReciboPago,montoReciboPago,
                                 referenciaReciboPago,estadoReciboPago,Fk_Usuario_Registro,Fk_FormaPagoVenta_Codigo)
                            VALUES (?,?,?,?,?,?,'CONFIRMADO',?,?)", Conexion, Transaccion))
                        {
                            Recibo.Parameters.Add(new OdbcParameter("Numero", OdbcType.VarChar, 30) { Value = NumeroRecibo });
                            Recibo.Parameters.Add(new OdbcParameter("Cliente", OdbcType.Int) { Value = IdCliente });
                            Recibo.Parameters.Add(new OdbcParameter("Moneda", OdbcType.BigInt) { Value = IdMoneda });
                            Recibo.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                            Recibo.Parameters.Add(new OdbcParameter("Monto", OdbcType.VarChar, 32)
                            { Value = Monto.ToString(CultureInfo.InvariantCulture) });
                            Recibo.Parameters.Add(new OdbcParameter("Referencia", OdbcType.VarChar, 255)
                            { Value = String.IsNullOrWhiteSpace(Referencia) ? (object)DBNull.Value : Referencia.Trim() });
                            Recibo.Parameters.Add(new OdbcParameter("Usuario", OdbcType.Int) { Value = IdUsuario });
                            Recibo.Parameters.Add(new OdbcParameter("Forma", OdbcType.Int) { Value = IdFormaPago });
                            Recibo.ExecuteNonQuery();
                        }
                        int IdRecibo;
                        using (OdbcCommand ObtenerRecibo = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                            IdRecibo = Convert.ToInt32(ObtenerRecibo.ExecuteScalar());

                        using (OdbcCommand Aplicacion = new OdbcCommand(@"
                            INSERT INTO tblAplicacionPago
                                (Fk_ReciboPago_Codigo,Fk_FacturaEncabezado_Codigo,fechaAplicacionPago,
                                 montoAplicadoAplicacionPago,estadoAplicacionPago,Fk_Usuario_Registro)
                            VALUES (?,?,?,?,'APLICADA',?)", Conexion, Transaccion))
                        {
                            Aplicacion.Parameters.Add(new OdbcParameter("Recibo", OdbcType.Int) { Value = IdRecibo });
                            Aplicacion.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            Aplicacion.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                            Aplicacion.Parameters.Add(new OdbcParameter("Monto", OdbcType.VarChar, 32)
                            { Value = Monto.ToString(CultureInfo.InvariantCulture) });
                            Aplicacion.Parameters.Add(new OdbcParameter("Usuario", OdbcType.Int) { Value = IdUsuario });
                            Aplicacion.ExecuteNonQuery();
                        }
                        int IdAplicacion;
                        using (OdbcCommand ObtenerAplicacion = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                            IdAplicacion = Convert.ToInt32(ObtenerAplicacion.ExecuteScalar());

                        // Algunas facturas anteriores al libro CxC tienen pagos aplicados,
                        // pero aún no tienen movimientos. Abrir el saldo vigente en el libro
                        // para que este y los próximos abonos queden conciliados.
                        if (!TieneMovimientosCxC)
                        {
                            using (OdbcCommand SaldoInicial = new OdbcCommand(@"
                                INSERT INTO tblMovimientoCxC
                                    (Fk_FacturaEncabezado_Codigo,fechaMovimientoCxC,tipoMovimientoCxC,
                                     descripcionMovimientoCxC,cargoMovimientoCxC,abonoMovimientoCxC,
                                     Fk_Usuario_Registro,Fk_AplicacionPago_Codigo,Fk_NotaCxC_Codigo,
                                     Fk_MovimientoCxC_Revertido)
                                VALUES (?,?,'AJUSTE_POSITIVO',?,?,0,?,NULL,NULL,NULL)", Conexion, Transaccion))
                            {
                                SaldoInicial.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                                SaldoInicial.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                                SaldoInicial.Parameters.Add(new OdbcParameter("Descripcion", OdbcType.VarChar, 255)
                                { Value = "Saldo inicial CxC al registrar " + NumeroRecibo });
                                SaldoInicial.Parameters.Add(new OdbcParameter("Cargo", OdbcType.VarChar, 32)
                                { Value = Saldo.ToString(CultureInfo.InvariantCulture) });
                                SaldoInicial.Parameters.Add(new OdbcParameter("Usuario", OdbcType.Int) { Value = IdUsuario });
                                SaldoInicial.ExecuteNonQuery();
                            }
                        }

                        using (OdbcCommand MovimientoCxC = new OdbcCommand(@"
                            INSERT INTO tblMovimientoCxC
                                (Fk_FacturaEncabezado_Codigo,fechaMovimientoCxC,tipoMovimientoCxC,
                                 descripcionMovimientoCxC,cargoMovimientoCxC,abonoMovimientoCxC,
                                 Fk_Usuario_Registro,Fk_AplicacionPago_Codigo,Fk_NotaCxC_Codigo,
                                 Fk_MovimientoCxC_Revertido)
                            VALUES (?,?,'PAGO',?,0,?,?,?,NULL,NULL)", Conexion, Transaccion))
                        {
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Descripcion", OdbcType.VarChar, 255)
                            { Value = "Abono de comisión " + NumeroRecibo });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Abono", OdbcType.VarChar, 32)
                            { Value = Monto.ToString(CultureInfo.InvariantCulture) });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Usuario", OdbcType.Int) { Value = IdUsuario });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Aplicacion", OdbcType.Int) { Value = IdAplicacion });
                            MovimientoCxC.ExecuteNonQuery();
                        }

                        ComisionVendedoresMetSincronizarAbonosInterno(Conexion, Transaccion, IdFactura);
                        decimal ComisionDevengada;
                        using (OdbcCommand ComisionPago = new OdbcCommand(@"
                            SELECT COALESCE(SUM(montoComisionDevengadoComisionAbono),0)
                            FROM tblComisionAbono WHERE Fk_AplicacionPago_Codigo=?
                              AND estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')", Conexion, Transaccion))
                        {
                            ComisionPago.Parameters.Add(new OdbcParameter("Aplicacion", OdbcType.Int) { Value = IdAplicacion });
                            ComisionDevengada = Convert.ToDecimal(ComisionPago.ExecuteScalar());
                        }
                        Transaccion.Commit();
                        return ComisionDevengada;
                    }
                    catch (Exception ErrorOriginal)
                    {
                        try { Transaccion.Rollback(); }
                        catch (Exception ErrorRollback)
                        {
                            throw new InvalidOperationException("Falló el registro del abono: " +
                                ComisionVendedoresMetDescribirError(ErrorOriginal) + " Error al revertir: " +
                                ComisionVendedoresMetDescribirError(ErrorRollback), ErrorOriginal);
                        }
                        throw;
                    }
                }
            }
        }

        public ClsFacturaComision ComisionVendedoresMetObtenerFactura(int IdFactura)
        {
            const string Comando = @"
                SELECT f.Pk_FacturaEncabezado AS id, f.Fk_Vendedor_Codigo AS id_vendedor,
                       f.fechaEmisionFacturaEncabezado AS fecha, cfv.nombreClienteFacturaVersion AS cliente,
                       COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd WHERE fd.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado),0) AS total,
                       COALESCE((SELECT SUM(ap.montoAplicadoAplicacionPago) FROM tblAplicacionPago ap INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                           WHERE ap.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'),0) AS total_pagado,
                       COALESCE((SELECT ap.montoAplicadoAplicacionPago FROM tblAplicacionPago ap INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                           WHERE ap.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'
                           ORDER BY ap.fechaAplicacionPago DESC,ap.Pk_AplicacionPago DESC LIMIT 1),0) AS ultimo_abono
                FROM tblFacturaEncabezado f LEFT JOIN tblClienteFacturaVersion cfv ON cfv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
                WHERE f.Pk_FacturaEncabezado = ?";
            DataTable Datos = ComisionVendedoresMetEjecucionConsulta(Comando,
                new List<OdbcParameter> { new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura } });
            return Datos.Rows.Count == 0 ? null : ComisionVendedoresMetMapearFactura(Datos.Rows[0]);
        }

        public ClsVendedorComision ComisionVendedoresMetObtenerVendedor(int IdVendedor)
        {
            return ComisionVendedoresMetObtenerVendedores().FirstOrDefault(Vendedor => Vendedor.IdVendedor == IdVendedor);
        }

        public List<ClsDetalleProductoComision> ComisionVendedoresMetObtenerDetallesFactura(int IdFactura, string Metodo)
        {
            Metodo = ComisionVendedoresMetNormalizarMetodo(Metodo);
            string NombreColumna;
            string IdClasificacion = ComisionVendedoresMetObtenerIdClasificacionSql(Metodo);
            string Porcentaje = ComisionVendedoresMetObtenerPorcentajeSql(Metodo);
            switch (Metodo)
            {
                case "Producto": NombreColumna = "p.nombreProducto"; break;
                case "Marca": NombreColumna = "m.nombreMarca"; break;
                case "Linea": NombreColumna = "l.nombreLinea"; break;
                case "Categoria": NombreColumna = "c.nombreCategoria"; break;
                default: throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            }

            string Comando = @"
                SELECT fd.Pk_FacturaDetalle AS id_detalle, fd.Fk_Producto_Codigo AS id_producto,
                       p.Pk_Producto AS id_producto_logistica,
                       CASE COALESCE(cd.metodoComisionFacturaDetalle, '')
                         WHEN 'PRODUCTO' THEN p.Pk_Producto WHEN 'MARCA' THEN p.Fk_Marca_Codigo
                         WHEN 'LINEA' THEN p.Fk_Linea_Codigo WHEN 'CATEGORIA' THEN p.Fk_Categoria_Codigo
                         ELSE " + IdClasificacion + @" END AS id_clasificacion,
                       COALESCE(p.nombreProducto, fd.descripcionItemFacturaDetalle) AS nombre_producto,
                       COALESCE(m.nombreMarca, '') AS marca, COALESCE(l.nombreLinea, '') AS linea,
                       COALESCE(c.nombreCategoria, '') AS categoria,
                       COALESCE(cd.metodoComisionFacturaDetalle, cf.metodoComisionFacturaEncabezado, '') AS metodo_guardado,
                       COALESCE(f.Fk_Vendedor_Codigo, 0) AS vendedor_guardado,
                       COALESCE(cf.Pk_ComisionFacturaEncabezado, 0) AS comision_guardada,
                       COALESCE(cd.montoComisionLineaComisionFacturaDetalle, 0) AS monto_comision_guardado,
                       COALESCE(cd.nombreClasificacionHistoricaComisionFacturaDetalle, " + NombreColumna + @") AS elemento,
                       fd.cantidadFacturaDetalle AS cantidad, fd.precioUnitarioFacturaDetalle AS precio_unitario,
                       GREATEST(fd.totalLineaFacturaDetalle - COALESCE(fd.impuestoMontoFacturaDetalle,0), 0) AS base_neta,
                       CAST(COALESCE(cd.tasaAplicadaComisionFacturaDetalle * 100, " + Porcentaje + @") AS CHAR) AS porcentaje,
                       CAST(CASE WHEN COALESCE(p.porcentajeComisionProducto,0)>0.99 THEN p.porcentajeComisionProducto ELSE COALESCE(p.porcentajeComisionProducto,0)*100 END AS CHAR) AS porcentaje_producto,
                       CAST(CASE WHEN COALESCE(m.porcentajeComisionMarca,0)>0.99 THEN m.porcentajeComisionMarca ELSE COALESCE(m.porcentajeComisionMarca,0)*100 END AS CHAR) AS porcentaje_marca,
                       CAST(CASE WHEN COALESCE(l.porcentajeComisionLinea,0)>0.99 THEN l.porcentajeComisionLinea ELSE COALESCE(l.porcentajeComisionLinea,0)*100 END AS CHAR) AS porcentaje_linea,
                       CAST(CASE WHEN COALESCE(c.porcentajeComisionCategoria,0)>0.99 THEN c.porcentajeComisionCategoria ELSE COALESCE(c.porcentajeComisionCategoria,0)*100 END AS CHAR) AS porcentaje_categoria,
                       COALESCE((SELECT SUM(ca.montoComisionDevengadoComisionAbono)
                           FROM tblComisionAbono ca INNER JOIN tblAplicacionPago ap ON ap.Pk_AplicacionPago=ca.Fk_AplicacionPago_Codigo
                           INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                           WHERE ca.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado
                             AND ca.estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')
                             AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'),0) AS comision_devengada,
                       COALESCE((SELECT ca.montoComisionDevengadoComisionAbono
                           FROM tblComisionAbono ca INNER JOIN tblAplicacionPago ap ON ap.Pk_AplicacionPago=ca.Fk_AplicacionPago_Codigo
                           INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                           WHERE ca.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado
                             AND ca.estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')
                             AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'
                           ORDER BY ap.fechaAplicacionPago DESC,ap.Pk_AplicacionPago DESC LIMIT 1),0) AS ultima_comision_devengada
                FROM tblFacturaDetalle fd INNER JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=fd.Fk_FacturaEncabezado_Codigo
                INNER JOIN tblProducto p ON p.Pk_Producto=fd.Fk_Producto_Codigo
                LEFT JOIN tblMarca m ON m.Pk_Marca=p.Fk_Marca_Codigo
                LEFT JOIN tblLinea l ON l.Pk_Linea=p.Fk_Linea_Codigo
                LEFT JOIN tblCategoria c ON c.Pk_Categoria=p.Fk_Categoria_Codigo
                LEFT JOIN tblComisionFacturaEncabezado cf ON cf.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                LEFT JOIN tblComisionFacturaDetalle cd ON cd.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado AND cd.Fk_FacturaDetalle_Codigo=fd.Pk_FacturaDetalle
                WHERE fd.Fk_FacturaEncabezado_Codigo = ? ORDER BY fd.numeroLineaFacturaDetalle";
            DataTable Datos = ComisionVendedoresMetEjecucionConsulta(Comando,
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
                    Cantidad = Convert.ToDecimal(Fila["cantidad"]),
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

        public List<ClsVentaVendedorBase> ComisionVendedoresMetObtenerVentasPorMetodo(string Metodo, DateTime FechaInicio, DateTime FechaFinExclusiva)
        {
            ComisionVendedoresMetSincronizarAbonos(null);
            string Comando = @"
                SELECT ca.Pk_ComisionAbono AS id_abono, f.Fk_Vendedor_Codigo AS id_vendedor,
                       COALESCE(NULLIF(TRIM(CONCAT_WS(' ', e.nombresEmpleado, e.apellidosEmpleado)), ''), v.codigoVendedor) AS nombre_vendedor,
                       f.Fk_Moneda_Codigo AS id_moneda,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       f.Pk_FacturaEncabezado AS id_factura, ca.Fk_AplicacionPago_Codigo AS id_aplicacion_pago,
                       ap.fechaAplicacionPago AS fecha_aplicacion,
                       CONCAT('FAC-',f.Pk_FacturaEncabezado,' · ',fd.descripcionItemFacturaDetalle) AS elemento_comision,
                       cd.metodoComisionFacturaDetalle AS metodo_comision,
                       cd.tasaAplicadaComisionFacturaDetalle * 100 AS porcentaje,
                       (cd.baseComisionFacturaDetalle / NULLIF(totalFactura.total,0)) * ap.montoAplicadoAplicacionPago AS base_cobrada,
                       (cd.montoComisionLineaComisionFacturaDetalle / NULLIF(comisionFactura.total_comision,0)) * ca.montoComisionDevengadoComisionAbono AS monto_comision,
                       fd.Pk_FacturaDetalle AS id_detalle
                FROM tblComisionAbono ca
                INNER JOIN tblComisionFacturaEncabezado cf ON cf.Pk_ComisionFacturaEncabezado=ca.Fk_ComisionFacturaEncabezado_Codigo
                INNER JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=cf.Fk_FacturaEncabezado_Codigo
                INNER JOIN tblMoneda m ON m.Pk_Moneda=f.Fk_Moneda_Codigo
                INNER JOIN tblVendedor v ON v.Pk_Vendedor=f.Fk_Vendedor_Codigo
                LEFT JOIN tblEmpleado e ON e.idEmpleado=v.Fk_Empleado_Codigo
                INNER JOIN tblAplicacionPago ap ON ap.Pk_AplicacionPago=ca.Fk_AplicacionPago_Codigo
                INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                INNER JOIN tblFacturaDetalle fd ON fd.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                INNER JOIN tblComisionFacturaDetalle cd ON cd.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado AND cd.Fk_FacturaDetalle_Codigo=fd.Pk_FacturaDetalle
                INNER JOIN (SELECT Fk_FacturaEncabezado_Codigo,SUM(totalLineaFacturaDetalle) total FROM tblFacturaDetalle GROUP BY Fk_FacturaEncabezado_Codigo) totalFactura ON totalFactura.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                INNER JOIN (SELECT Fk_ComisionFacturaEncabezado_Codigo,SUM(montoComisionLineaComisionFacturaDetalle) total_comision FROM tblComisionFacturaDetalle GROUP BY Fk_ComisionFacturaEncabezado_Codigo) comisionFactura ON comisionFactura.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado
                WHERE ca.estadoPagoVendedorComisionAbono = 'PENDIENTE'
                  AND cf.estadoComisionFacturaEncabezado = 'CALCULADA'
                  AND f.estadoFacturaEncabezado = 'EMITIDA'
                  AND ap.estadoAplicacionPago = 'APLICADA' AND rp.estadoReciboPago = 'CONFIRMADO'
                  AND ap.fechaAplicacionPago >= ? AND ap.fechaAplicacionPago < ?
                ORDER BY v.codigoVendedor, ap.fechaAplicacionPago, f.Pk_FacturaEncabezado, fd.numeroLineaFacturaDetalle";
            List<OdbcParameter> Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("FechaInicio", OdbcType.Date) { Value = FechaInicio.Date },
                new OdbcParameter("FechaFin", OdbcType.Date) { Value = FechaFinExclusiva.Date }
            };
            DataTable Datos = ComisionVendedoresMetEjecucionConsulta(Comando, Parametros);
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
                SELECT f.Fk_Vendedor_Codigo AS id_vendedor, v.codigoVendedor,
                       COALESCE(NULLIF(TRIM(CONCAT_WS(' ',e.nombresEmpleado,e.apellidosEmpleado)),''),v.codigoVendedor) AS nombre_vendedor,
                       f.Fk_Moneda_Codigo AS id_moneda, f.Pk_FacturaEncabezado AS id_factura,
                       0 AS id_detalle, f.fechaEmisionFacturaEncabezado AS fecha_factura,
                       COALESCE(m.simboloMoneda,'') AS simbolo_moneda,
                       CONCAT('FAC-',f.Pk_FacturaEncabezado,' · Comisión asignada a un vendedor; ',
                              dc.cantidad_lineas,' producto(s); total calculado ',m.simboloMoneda,' ',FORMAT(dc.total_comision,2),
                              ' (sin abono de comisión pendiente en el rango)') AS elemento_comision,
                       dc.metodo_comision,
                       dc.porcentaje,
                       dc.total_comision AS monto_comision_total
                FROM tblComisionFacturaEncabezado cf
                INNER JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=cf.Fk_FacturaEncabezado_Codigo
                INNER JOIN tblMoneda m ON m.Pk_Moneda=f.Fk_Moneda_Codigo
                INNER JOIN tblVendedor v ON v.Pk_Vendedor=f.Fk_Vendedor_Codigo
                LEFT JOIN tblEmpleado e ON e.idEmpleado=v.Fk_Empleado_Codigo
                INNER JOIN (
                    SELECT Fk_ComisionFacturaEncabezado_Codigo,COUNT(*) AS cantidad_lineas,
                           CASE WHEN COUNT(DISTINCT metodoComisionFacturaDetalle)=1 THEN MAX(metodoComisionFacturaDetalle) ELSE 'MIXTA' END AS metodo_comision,
                           CASE WHEN SUM(baseComisionFacturaDetalle)>0
                                THEN SUM(baseComisionFacturaDetalle*tasaAplicadaComisionFacturaDetalle)/SUM(baseComisionFacturaDetalle)*100
                                ELSE 0 END AS porcentaje,
                           SUM(baseComisionFacturaDetalle*tasaAplicadaComisionFacturaDetalle) AS total_comision
                    FROM tblComisionFacturaDetalle GROUP BY Fk_ComisionFacturaEncabezado_Codigo
                ) dc ON dc.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado
                WHERE cf.estadoComisionFacturaEncabezado='CALCULADA' AND f.estadoFacturaEncabezado='EMITIDA'
                  AND f.fechaEmisionFacturaEncabezado>=? AND f.fechaEmisionFacturaEncabezado<?
                  AND NOT EXISTS (
                      SELECT 1 FROM tblComisionAbono ca
                      INNER JOIN tblAplicacionPago ap ON ap.Pk_AplicacionPago=ca.Fk_AplicacionPago_Codigo
                      INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                      WHERE ca.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado
                        AND ca.estadoPagoVendedorComisionAbono='PENDIENTE'
                        AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'
                        AND ap.fechaAplicacionPago>=? AND ap.fechaAplicacionPago<?)
                ORDER BY v.codigoVendedor,f.fechaEmisionFacturaEncabezado,f.Pk_FacturaEncabezado";
            List<OdbcParameter> ParametrosAsignaciones = new List<OdbcParameter>
            {
                new OdbcParameter("FechaFacturaInicio", OdbcType.Date) { Value = FechaInicio.Date },
                new OdbcParameter("FechaFacturaFin", OdbcType.Date) { Value = FechaFinExclusiva.Date },
                new OdbcParameter("FechaPagoInicio", OdbcType.Date) { Value = FechaInicio.Date },
                new OdbcParameter("FechaPagoFin", OdbcType.Date) { Value = FechaFinExclusiva.Date }
            };
            DataTable Asignaciones = ComisionVendedoresMetEjecucionConsulta(SqlAsignacionesSinAbono, ParametrosAsignaciones);
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

        public List<ClsProductoComision> ComisionVendedoresMetConsultarPorcentajes(string Metodo)
        {
            ComisionVendedoresMetObtenerIdClasificacionSql(Metodo);
            const string Comando = @"SELECT p.Pk_Producto AS id, p.nombreProducto AS nombre,
                       COALESCE(m.Pk_Marca,0) AS id_marca, COALESCE(l.Pk_Linea,0) AS id_linea,
                       COALESCE(c.Pk_Categoria,0) AS id_categoria,
                       COALESCE(m.nombreMarca,'') AS marca, COALESCE(l.nombreLinea,'') AS linea,
                       COALESCE(c.nombreCategoria,'') AS categoria,
                       CAST(CASE WHEN COALESCE(m.porcentajeComisionMarca,0)>0.99 THEN m.porcentajeComisionMarca ELSE COALESCE(m.porcentajeComisionMarca,0)*100 END AS CHAR) AS porcentaje_marca,
                       CAST(CASE WHEN COALESCE(l.porcentajeComisionLinea,0)>0.99 THEN l.porcentajeComisionLinea ELSE COALESCE(l.porcentajeComisionLinea,0)*100 END AS CHAR) AS porcentaje_linea,
                       CAST(CASE WHEN COALESCE(c.porcentajeComisionCategoria,0)>0.99 THEN c.porcentajeComisionCategoria ELSE COALESCE(c.porcentajeComisionCategoria,0)*100 END AS CHAR) AS porcentaje_categoria,
                       CAST(CASE WHEN COALESCE(p.porcentajeComisionProducto,0)>0.99 THEN p.porcentajeComisionProducto ELSE COALESCE(p.porcentajeComisionProducto,0)*100 END AS CHAR) AS porcentaje_producto
                       FROM tblProducto p LEFT JOIN tblMarca m ON m.Pk_Marca=p.Fk_Marca_Codigo
                       LEFT JOIN tblLinea l ON l.Pk_Linea=p.Fk_Linea_Codigo
                       LEFT JOIN tblCategoria c ON c.Pk_Categoria=p.Fk_Categoria_Codigo
                       ORDER BY p.nombreProducto";
            DataTable Datos = ComisionVendedoresMetEjecucionConsulta(Comando, new List<OdbcParameter>());
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

        public int ComisionVendedoresMetActualizarPorcentaje(string Metodo, int IdClasificacion, decimal Porcentaje)
        {
            if (IdClasificacion <= 0) throw new ArgumentOutOfRangeException("IdClasificacion", "Seleccione una clasificación válida.");
            if (Porcentaje < 0m || Porcentaje > 99m)
                throw new ArgumentOutOfRangeException("Porcentaje", "Ingrese una comisión entre 0.00% y 99.00%.");
            string Tabla, Columna, Id;
            switch (Metodo)
            {
                case "Producto": Tabla = "tblProducto"; Columna = "porcentajeComisionProducto"; Id = "Pk_Producto"; break;
                case "Marca": Tabla = "tblMarca"; Columna = "porcentajeComisionMarca"; Id = "Pk_Marca"; break;
                case "Linea": Tabla = "tblLinea"; Columna = "porcentajeComisionLinea"; Id = "Pk_Linea"; break;
                case "Categoria": Tabla = "tblCategoria"; Columna = "porcentajeComisionCategoria"; Id = "Pk_Categoria"; break;
                default: throw new ArgumentException("Seleccione Marca, Línea, Producto o Categoría.");
            }
            string Comando = "UPDATE " + Tabla + " SET " + Columna + " = ? WHERE " + Id + " = ?";
            using (OdbcConnection Conexion = ComisionVendedoresMetObtenerConexion())
            {
                ComisionVendedoresMetAbrirConexion(Conexion);
                using (OdbcCommand Actualizar = new OdbcCommand(Comando, Conexion))
                {
                    // El esquema almacena la tasa como fracción (1% = 0.01).
                    // Texto numérico invariante evita la conversión incorrecta SQL_C_NUMERIC del ODBC.
                    Actualizar.Parameters.Add(new OdbcParameter("Porcentaje", OdbcType.VarChar, 32)
                    { Value = (Porcentaje / 100m).ToString(CultureInfo.InvariantCulture) });
                    Actualizar.Parameters.Add(new OdbcParameter("IdClasificacion", OdbcType.Int) { Value = IdClasificacion });
                    int FilasAfectadas = Actualizar.ExecuteNonQuery();
                    if (FilasAfectadas == 0)
                    {
                        using (OdbcCommand Verificar = new OdbcCommand("SELECT COUNT(*) FROM " + Tabla + " WHERE " + Id + "=?", Conexion))
                        {
                            Verificar.Parameters.Add(new OdbcParameter("IdClasificacion", OdbcType.Int) { Value = IdClasificacion });
                            int Existe = Convert.ToInt32(Verificar.ExecuteScalar());
                            if (Existe == 0)
                                throw new InvalidOperationException("No se encontró el elemento seleccionado en " + Tabla + " (ID " + IdClasificacion + "). No se actualizó ningún porcentaje.");
                            // Un resultado de cero también puede significar que el valor ya era igual.
                            return Existe;
                        }
                    }
                    return FilasAfectadas;
                }
            }
        }

        public int ComisionVendedoresMetAsignarVendedorFactura(int IdFactura, int IdVendedor)
        {
            using (OdbcConnection Conexion = ComisionVendedoresMetObtenerConexion())
            {
                ComisionVendedoresMetAbrirConexion(Conexion);
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        using (OdbcCommand YaCalculada = new OdbcCommand(
                            "SELECT Fk_Vendedor_Codigo FROM tblFacturaEncabezado WHERE Pk_FacturaEncabezado = ?", Conexion, Transaccion))
                        {
                            YaCalculada.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura });
                            object VendedorGuardado = YaCalculada.ExecuteScalar();
                            if (VendedorGuardado != null && VendedorGuardado != DBNull.Value)
                            {
                                if (Convert.ToInt32(VendedorGuardado) != IdVendedor)
                                    throw new InvalidOperationException("La comisión ya está guardada; no se puede cambiar el vendedor de esa factura.");
                                Transaccion.Commit();
                                return 1;
                            }
                        }
                        using (OdbcCommand Actualizar = new OdbcCommand(@"
                            UPDATE tblFacturaEncabezado SET Fk_Vendedor_Codigo = ?
                            WHERE Pk_FacturaEncabezado = ? AND estadoFacturaEncabezado = 'EMITIDA'", Conexion, Transaccion))
                        {
                            Actualizar.Parameters.Add(new OdbcParameter("IdVendedor", OdbcType.Int) { Value = IdVendedor });
                            Actualizar.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura });
                            int Afectados = Actualizar.ExecuteNonQuery();
                            if (Afectados == 0)
                            {
                                using (OdbcCommand Confirmar = new OdbcCommand(
                                    "SELECT COUNT(*) FROM tblFacturaEncabezado WHERE Pk_FacturaEncabezado=? AND Fk_Vendedor_Codigo=? AND estadoFacturaEncabezado='EMITIDA'", Conexion, Transaccion))
                                {
                                    Confirmar.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura });
                                    Confirmar.Parameters.Add(new OdbcParameter("IdVendedor", OdbcType.Int) { Value = IdVendedor });
                                    Afectados = Convert.ToInt32(Confirmar.ExecuteScalar());
                                }
                            }
                            if (Afectados == 0) throw new InvalidOperationException("No se pudo asignar: la factura no existe o no está emitida.");
                        }
                        Transaccion.Commit();
                        return 1;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, string Metodo)
        {
            ClsFacturaComision FacturaAnterior = ComisionVendedoresMetObtenerFactura(IdFactura);
            if (FacturaAnterior == null) throw new InvalidOperationException("No se encontró la factura seleccionada.");
            List<ClsDetalleProductoComision> Lineas = ComisionVendedoresMetObtenerDetallesFactura(IdFactura, Metodo);
            foreach (ClsDetalleProductoComision Linea in Lineas) Linea.MetodoSeleccionado = Metodo;
            return ComisionVendedoresMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Lineas);
        }

        public int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones)
        {
            if (Selecciones == null || Selecciones.Count == 0) throw new ArgumentException("Elija el método para cada producto antes de asignar.");
            List<ClsDetalleProductoComision> Lineas = new List<ClsDetalleProductoComision>();
            foreach (ClsDetalleProductoComision Seleccion in Selecciones)
            {
                string MetodoOriginal = Seleccion.MetodoSeleccionado;
                Seleccion.MetodoSeleccionado = ComisionVendedoresMetNormalizarMetodo(Seleccion.MetodoSeleccionado);
                if (String.IsNullOrWhiteSpace(Seleccion.MetodoSeleccionado))
                    throw new InvalidOperationException("Método no válido en el detalle " + Seleccion.IdFacturaDetalle + ": «" + (MetodoOriginal ?? "(vacío)") + "».");
                ClsDetalleProductoComision Linea = ComisionVendedoresMetObtenerDetallesFactura(IdFactura, Seleccion.MetodoSeleccionado)
                    .FirstOrDefault(Detalle => Detalle.IdFacturaDetalle == Seleccion.IdFacturaDetalle);
                if (Linea == null || Linea.IdClasificacionLogistica <= 0) throw new InvalidOperationException("No se encontró la clasificación/tasa para un producto de la factura.");
                Linea.MetodoSeleccionado = Seleccion.MetodoSeleccionado;
                Lineas.Add(Linea);
            }
            if (Lineas.Select(Linea => Linea.IdFacturaDetalle).Distinct().Count() != Lineas.Count) throw new InvalidOperationException("Hay renglones de factura duplicados.");
            using (OdbcConnection Conexion = ComisionVendedoresMetObtenerConexion())
            {
                ComisionVendedoresMetAbrirConexion(Conexion);
                string PasoActual = "iniciar transacción";
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        int VendedorActual = 0;
                        bool FacturaEncontrada = false;
                        bool ComisionYaCalculada = false;
                        using (OdbcCommand ConsultarAsignacion = new OdbcCommand(@"
                            SELECT COALESCE(f.Fk_Vendedor_Codigo,0) AS vendedor_actual,
                                   EXISTS(SELECT 1 FROM tblComisionFacturaEncabezado cf
                                          WHERE cf.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                                            AND cf.estadoComisionFacturaEncabezado='CALCULADA') AS comision_calculada
                            FROM tblFacturaEncabezado f
                            WHERE f.Pk_FacturaEncabezado=? FOR UPDATE", Conexion, Transaccion))
                        {
                            ConsultarAsignacion.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            using (OdbcDataReader Lector = ConsultarAsignacion.ExecuteReader())
                            {
                                if (Lector.Read())
                                {
                                    FacturaEncontrada = true;
                                    VendedorActual = Convert.ToInt32(Lector["vendedor_actual"]);
                                    ComisionYaCalculada = Convert.ToInt32(Lector["comision_calculada"]) != 0;
                                }
                            }
                        }
                        if (!FacturaEncontrada) throw new InvalidOperationException("La factura seleccionada ya no existe.");
                        if (ComisionYaCalculada && VendedorActual != IdVendedor)
                            throw new InvalidOperationException("Esta factura ya tiene una comisión calculada para otro vendedor. No se permite reasignarla, porque cambiaría el dueño del saldo/comisión.");

                        PasoActual = "asignar vendedor a la factura";
                        using (OdbcCommand Asignar = new OdbcCommand("UPDATE tblFacturaEncabezado SET Fk_Vendedor_Codigo=? WHERE Pk_FacturaEncabezado=? AND estadoFacturaEncabezado='EMITIDA'", Conexion, Transaccion))
                        {
                            Asignar.Parameters.Add(new OdbcParameter("Vendedor", OdbcType.Int) { Value = IdVendedor });
                            Asignar.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            Asignar.ExecuteNonQuery();
                        }
                        PasoActual = "guardar encabezado y detalle de comisión por producto";
                        int IdComision = ComisionVendedoresMetGuardarComisionFacturaLineas(Conexion, Transaccion, IdFactura, IdVendedor, Lineas);
                        PasoActual = "sincronizar comisiones con los pagos aplicados";
                        ComisionVendedoresMetSincronizarAbonosInterno(Conexion, Transaccion, IdFactura);
                        Transaccion.Commit();
                        return IdComision;
                    }
                    catch (Exception ErrorOriginal)
                    {
                        // No permitir que un error secundario del rollback oculte el
                        // error SQL que hizo fallar la asignación de la comisión.
                        try { Transaccion.Rollback(); }
                        catch (Exception ErrorRollback)
                        {
                            throw new InvalidOperationException(
                                "Falló la asignación en el paso «" + PasoActual + "». Error original: " +
                                ComisionVendedoresMetDescribirError(ErrorOriginal) +
                                " Error al revertir la transacción: " + ComisionVendedoresMetDescribirError(ErrorRollback),
                                ErrorOriginal);
                        }
                        throw new InvalidOperationException(
                            "Falló la asignación en el paso «" + PasoActual + "». " +
                            ComisionVendedoresMetDescribirError(ErrorOriginal), ErrorOriginal);
                    }
                }
            }
        }

        private static string ComisionVendedoresMetDescribirError(Exception Error)
        {
            List<string> Detalles = new List<string>();
            for (Exception Actual = Error; Actual != null; Actual = Actual.InnerException)
            {
                string Mensaje = Actual.Message;
                Detalles.Add(String.IsNullOrWhiteSpace(Mensaje)
                    ? Actual.GetType().FullName
                    : Actual.GetType().FullName + ": " + Mensaje);
                OdbcException ErrorOdbc = Actual as OdbcException;
                if (ErrorOdbc != null)
                {
                    foreach (OdbcError ErrorDriver in ErrorOdbc.Errors)
                        Detalles.Add("ODBC SQLSTATE " + ErrorDriver.SQLState + " (" + ErrorDriver.NativeError + "): " + ErrorDriver.Message);
                    if (ErrorOdbc.Errors.Count == 0 && !String.IsNullOrWhiteSpace(ErrorOdbc.StackTrace))
                        Detalles.Add("Pila ODBC: " + ErrorOdbc.StackTrace);
                }
            }
            return String.Join(" -> ", Detalles);
        }

        public int ComisionVendedoresMetGuardarComisionFacturaPorProducto(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones)
        {
            return ComisionVendedoresMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Selecciones);
        }

        private int ComisionVendedoresMetGuardarComisionFacturaLineas(OdbcConnection Conexion, OdbcTransaction Transaccion, int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Lineas)
        {
            if (Lineas.Select(Item => Item.IdFacturaDetalle).Distinct().Count() != Lineas.Count)
                throw new InvalidOperationException("Hay productos repetidos en la selección de comisión.");
                        using (OdbcCommand Existente = new OdbcCommand("SELECT Pk_ComisionFacturaEncabezado,Fk_FacturaEncabezado_Codigo FROM tblComisionFacturaEncabezado WHERE Fk_FacturaEncabezado_Codigo=? AND estadoComisionFacturaEncabezado='CALCULADA' FOR UPDATE", Conexion, Transaccion))
                        {
                            Existente.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            using (OdbcDataReader Reader = Existente.ExecuteReader())
                            {
                                if (Reader.Read())
                                {
                                    int IdComisionExistente = Convert.ToInt32(Reader["Pk_ComisionFacturaEncabezado"]);
                                    Reader.Close();
                                    foreach (ClsDetalleProductoComision Linea in Lineas)
                                    {
                        if (!Linea.ComisionGuardada || !String.Equals(Linea.MetodoGuardado, Linea.MetodoSeleccionado, StringComparison.OrdinalIgnoreCase))
                                            throw new InvalidOperationException("La factura ya fue calculada con otros métodos. No se puede alterar su histórico.");
                                    }
                                    return IdComisionExistente;
                                }
                            }
                        }
                        using (OdbcCommand Verificar = new OdbcCommand("SELECT COUNT(*) FROM tblVendedor WHERE Pk_Vendedor=? AND activoVendedor=1", Conexion, Transaccion))
                        {
                            Verificar.Parameters.Add(new OdbcParameter("Vendedor", OdbcType.Int) { Value = IdVendedor });
                            if (Convert.ToInt32(Verificar.ExecuteScalar()) == 0) throw new InvalidOperationException("El vendedor no existe o está inactivo.");
                        }
                        int Conteo;
                        using (OdbcCommand Contar = new OdbcCommand("SELECT COUNT(*) FROM tblFacturaDetalle WHERE Fk_FacturaEncabezado_Codigo=?", Conexion, Transaccion))
                        {
                            Contar.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            Conteo = Convert.ToInt32(Contar.ExecuteScalar());
                        }
                        if (Conteo != Lineas.Count) throw new InvalidOperationException("Debe elegir un método para todos los productos de la factura.");
                        using (OdbcCommand Validar = new OdbcCommand("SELECT estadoFacturaEncabezado,Fk_Vendedor_Codigo,COALESCE((SELECT SUM(totalLineaFacturaDetalle) FROM tblFacturaDetalle WHERE Fk_FacturaEncabezado_Codigo=Pk_FacturaEncabezado),0) AS total FROM tblFacturaEncabezado WHERE Pk_FacturaEncabezado=? FOR UPDATE", Conexion, Transaccion))
                        {
                            Validar.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            using (OdbcDataReader Reader = Validar.ExecuteReader())
                            {
                                if (!Reader.Read() || !String.Equals(Convert.ToString(Reader["estadoFacturaEncabezado"]), "EMITIDA", StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("Solo se puede comisionar una factura emitida.");
                                if (Reader.IsDBNull(Reader.GetOrdinal("Fk_Vendedor_Codigo")) || Convert.ToInt32(Reader["Fk_Vendedor_Codigo"]) != IdVendedor) throw new InvalidOperationException("El vendedor debe estar asignado a esta factura.");
                                if (Convert.ToDecimal(Reader["total"]) <= 0m) throw new InvalidOperationException("La factura debe tener total mayor que cero.");
                            }
                        }
                        decimal BaseTotal = Lineas.Sum(Item => Item.BaseComisionable);
                        decimal ComisionTotal = Lineas.Sum(Item => Math.Round(Item.BaseComisionable * Item.Porcentaje / 100m, 2, MidpointRounding.AwayFromZero));
                    ClsDetalleProductoComision Primera = Lineas[0];
                        string MetodoCabecera = Lineas.Select(Item => Item.MetodoSeleccionado.ToUpperInvariant()).Distinct().Count() == 1 ? Primera.MetodoSeleccionado.ToUpperInvariant() : "MIXTA";
                        using (OdbcCommand Insertar = new OdbcCommand(@"INSERT INTO tblComisionFacturaEncabezado
                            (Fk_FacturaEncabezado_Codigo,metodoComisionFacturaEncabezado,estadoComisionFacturaEncabezado)
                            VALUES (?,?,'CALCULADA')", Conexion, Transaccion))
                        {
                            Insertar.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            Insertar.Parameters.Add(new OdbcParameter("Metodo", OdbcType.VarChar) { Value = MetodoCabecera });
                            Insertar.ExecuteNonQuery();
                        }
                        int IdComision;
                        using (OdbcCommand ObtenerId = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion)) IdComision = Convert.ToInt32(ObtenerId.ExecuteScalar());
                        foreach (ClsDetalleProductoComision Linea in Lineas)
                        {
                            string ColumnaClasificacion = Linea.MetodoSeleccionado == "Producto" ? "" : Linea.MetodoSeleccionado == "Marca" ? ",Fk_Marca_Codigo" : Linea.MetodoSeleccionado == "Linea" ? ",Fk_Linea_Codigo" : ",Fk_Categoria_Codigo";
                            using (OdbcCommand InsertarLinea = new OdbcCommand(@"INSERT INTO tblComisionFacturaDetalle
                            (Fk_ComisionFacturaEncabezado_Codigo,Fk_FacturaDetalle_Codigo,metodoComisionFacturaDetalle,nombreClasificacionHistoricaComisionFacturaDetalle,tasaAplicadaComisionFacturaDetalle,baseComisionFacturaDetalle" + ColumnaClasificacion + @")
                                VALUES (?,?,?,?,?,?" + (ColumnaClasificacion.Length == 0 ? "" : ",?") + ")", Conexion, Transaccion))
                            {
                                InsertarLinea.Parameters.Add(new OdbcParameter("Comision", OdbcType.Int) { Value = IdComision });
                                InsertarLinea.Parameters.Add(new OdbcParameter("Detalle", OdbcType.Int) { Value = Linea.IdFacturaDetalle });
                                InsertarLinea.Parameters.Add(new OdbcParameter("Metodo", OdbcType.VarChar) { Value = Linea.MetodoSeleccionado.ToUpperInvariant() });
                                InsertarLinea.Parameters.Add(new OdbcParameter("Nombre", OdbcType.VarChar, 100) { Value = Linea.ElementoComision });
                                // El controlador ODBC instalado falla al enlazar ciertos DECIMAL
                                // en INSERT. Enviarlos como texto numérico invariante permite que
                                // MySQL los convierta según la escala de las columnas DECIMAL.
                                InsertarLinea.Parameters.Add(new OdbcParameter("Porcentaje", OdbcType.VarChar, 32)
                                { Value = (Linea.Porcentaje / 100m).ToString(CultureInfo.InvariantCulture) });
                                InsertarLinea.Parameters.Add(new OdbcParameter("Base", OdbcType.VarChar, 32)
                                { Value = Linea.BaseComisionable.ToString(CultureInfo.InvariantCulture) });
                                if (ColumnaClasificacion.Length > 0) InsertarLinea.Parameters.Add(new OdbcParameter("Clasificacion", OdbcType.Int) { Value = Linea.IdClasificacionLogistica });
                                InsertarLinea.ExecuteNonQuery();
                            }
                        }
                        return IdComision;
        }

        public int ComisionVendedoresMetGuardarComisionFactura(int IdFactura, int IdVendedor, string Metodo)
        {
            List<ClsDetalleProductoComision> Lineas = ComisionVendedoresMetObtenerDetallesFactura(IdFactura, Metodo);
            foreach (ClsDetalleProductoComision Linea in Lineas) Linea.MetodoSeleccionado = Metodo;
            return ComisionVendedoresMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Lineas);
        }


        public void ComisionVendedoresMetSincronizarAbonos(int? IdFactura)
        {
            using (OdbcConnection Conexion = ComisionVendedoresMetObtenerConexion())
            {
                ComisionVendedoresMetAbrirConexion(Conexion);
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        ComisionVendedoresMetSincronizarAbonosInterno(Conexion, Transaccion, IdFactura);
                        Transaccion.Commit();
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        private void ComisionVendedoresMetSincronizarAbonosInterno(OdbcConnection Conexion, OdbcTransaction Transaccion, int? IdFactura)
        {
            // Algunas filas de prueba se guardaron con tasa/base en cero por el
            // enlace numérico ODBC anterior. Reparar únicamente comisiones totalmente
            // en cero y sin abonos; no tocar históricos que ya generaron pagos.
            List<int> ComisionesSinAbonoEnCero = new List<int>();
            using (OdbcCommand BuscarCeros = new OdbcCommand(@"
                SELECT cf.Pk_ComisionFacturaEncabezado
                FROM tblComisionFacturaEncabezado cf
                INNER JOIN tblComisionFacturaDetalle cd ON cd.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado
                WHERE cf.estadoComisionFacturaEncabezado='CALCULADA'
                  AND NOT EXISTS (SELECT 1 FROM tblComisionAbono ca WHERE ca.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado)
                GROUP BY cf.Pk_ComisionFacturaEncabezado
                HAVING COALESCE(SUM(cd.baseComisionFacturaDetalle*cd.tasaAplicadaComisionFacturaDetalle),0)=0", Conexion, Transaccion))
            using (OdbcDataReader LectorCeros = BuscarCeros.ExecuteReader())
            {
                while (LectorCeros.Read()) ComisionesSinAbonoEnCero.Add(Convert.ToInt32(LectorCeros[0]));
            }
            foreach (int IdComisionReparar in ComisionesSinAbonoEnCero)
            {
                using (OdbcCommand Reparar = new OdbcCommand(@"
                    UPDATE tblComisionFacturaDetalle cd
                    INNER JOIN tblComisionFacturaEncabezado cf ON cf.Pk_ComisionFacturaEncabezado=cd.Fk_ComisionFacturaEncabezado_Codigo
                    INNER JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=cf.Fk_FacturaEncabezado_Codigo
                    INNER JOIN tblFacturaDetalle fd ON fd.Pk_FacturaDetalle=cd.Fk_FacturaDetalle_Codigo
                    INNER JOIN tblProducto p ON p.Pk_Producto=fd.Fk_Producto_Codigo
                    LEFT JOIN tblMarca m ON m.Pk_Marca=p.Fk_Marca_Codigo
                    LEFT JOIN tblLinea l ON l.Pk_Linea=p.Fk_Linea_Codigo
                    LEFT JOIN tblCategoria c ON c.Pk_Categoria=p.Fk_Categoria_Codigo
                    SET cd.baseComisionFacturaDetalle=GREATEST(fd.totalLineaFacturaDetalle-COALESCE(fd.impuestoMontoFacturaDetalle,0),0),
                        cd.tasaAplicadaComisionFacturaDetalle=CASE UPPER(cd.metodoComisionFacturaDetalle)
                            WHEN 'PRODUCTO' THEN COALESCE(p.porcentajeComisionProducto,0)
                            WHEN 'MARCA' THEN COALESCE(m.porcentajeComisionMarca,0)
                            WHEN 'LINEA' THEN COALESCE(l.porcentajeComisionLinea,0)
                            WHEN 'CATEGORIA' THEN COALESCE(c.porcentajeComisionCategoria,0)
                            ELSE 0 END
                    WHERE cd.Fk_ComisionFacturaEncabezado_Codigo=?
                      AND NOT EXISTS (SELECT 1 FROM tblComisionAbono ca WHERE ca.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado)", Conexion, Transaccion))
                {
                    Reparar.Parameters.Add(new OdbcParameter("Comision", OdbcType.Int) { Value = IdComisionReparar });
                    Reparar.ExecuteNonQuery();
                }
            }

            string Filtro = IdFactura.HasValue ? " AND f.Pk_FacturaEncabezado = ?" : "";
            string SqlComisiones = @"
                SELECT cf.Pk_ComisionFacturaEncabezado, cf.Fk_FacturaEncabezado_Codigo,
                       COALESCE((SELECT SUM(cd.baseComisionFacturaDetalle * cd.tasaAplicadaComisionFacturaDetalle) FROM tblComisionFacturaDetalle cd
                         WHERE cd.Fk_ComisionFacturaEncabezado_Codigo=cf.Pk_ComisionFacturaEncabezado),0) AS montoComisionTotal,
                       COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd
                         WHERE fd.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado),0) AS total
                FROM tblComisionFacturaEncabezado cf
                INNER JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=cf.Fk_FacturaEncabezado_Codigo
                WHERE cf.estadoComisionFacturaEncabezado='CALCULADA' AND f.estadoFacturaEncabezado='EMITIDA'" + Filtro + " ORDER BY cf.Pk_ComisionFacturaEncabezado FOR UPDATE";
            using (OdbcCommand BuscarComisiones = new OdbcCommand(SqlComisiones, Conexion, Transaccion))
            {
                if (IdFactura.HasValue) BuscarComisiones.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura.Value });
                using (OdbcDataReader Comisiones = BuscarComisiones.ExecuteReader())
                {
                    List<int> IdComisiones = new List<int>();
                    List<int> IdFacturas = new List<int>();
                    List<decimal> TotalesFactura = new List<decimal>();
                    List<decimal> TotalesComision = new List<decimal>();
                    while (Comisiones.Read())
                    {
                        IdComisiones.Add(Convert.ToInt32(Comisiones["Pk_ComisionFacturaEncabezado"]));
                        IdFacturas.Add(Convert.ToInt32(Comisiones["Fk_FacturaEncabezado_Codigo"]));
                        TotalesComision.Add(Convert.ToDecimal(Comisiones["montoComisionTotal"]));
                        TotalesFactura.Add(Convert.ToDecimal(Comisiones["total"]));
                    }
                    Comisiones.Close();

                    for (int Indice = 0; Indice < IdComisiones.Count; Indice++)
                        ComisionVendedoresMetSincronizarAbonosComision(Conexion, Transaccion,
                            IdComisiones[Indice], IdFacturas[Indice], TotalesComision[Indice], TotalesFactura[Indice]);
                }
            }
        }

        private void ComisionVendedoresMetSincronizarAbonosComision(OdbcConnection Conexion, OdbcTransaction Transaccion,
            int IdComision, int IdFactura, decimal MontoComisionTotal, decimal TotalFactura)
        {
            const string PagosSql = @"
                SELECT ap.Pk_AplicacionPago, ap.montoAplicadoAplicacionPago
                FROM tblAplicacionPago ap INNER JOIN tblReciboPago rp ON rp.Pk_ReciboPago=ap.Fk_ReciboPago_Codigo
                WHERE ap.Fk_FacturaEncabezado_Codigo=? AND ap.estadoAplicacionPago='APLICADA'
                  AND rp.estadoReciboPago='CONFIRMADO' AND ap.montoAplicadoAplicacionPago > 0
                ORDER BY ap.fechaAplicacionPago, ap.Pk_AplicacionPago";
            using (OdbcCommand Pagos = new OdbcCommand(PagosSql, Conexion, Transaccion))
            {
                Pagos.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura });
                using (OdbcDataReader Lector = Pagos.ExecuteReader())
                {
                    List<int> IdAplicaciones = new List<int>();
                    List<decimal> MontosAplicados = new List<decimal>();
                    while (Lector.Read())
                    {
                        IdAplicaciones.Add(Convert.ToInt32(Lector["Pk_AplicacionPago"]));
                        MontosAplicados.Add(Convert.ToDecimal(Lector["montoAplicadoAplicacionPago"]));
                    }
                    Lector.Close();

                    decimal AcumuladoAplicado = 0m;
                    decimal AcumuladoComision = 0m;
                    for (int Indice = 0; Indice < IdAplicaciones.Count; Indice++)
                    {
                        AcumuladoAplicado += MontosAplicados[Indice];
                        decimal Proporcion = TotalFactura <= 0m ? 0m : Math.Min(1m, AcumuladoAplicado / TotalFactura);
                        decimal ComisionAcumuladaObjetivo = Math.Round(MontoComisionTotal * Proporcion, 4, MidpointRounding.AwayFromZero);
                        using (OdbcCommand Existente = new OdbcCommand(@"
                            SELECT montoComisionDevengadoComisionAbono, estadoPagoVendedorComisionAbono
                            FROM tblComisionAbono WHERE Fk_AplicacionPago_Codigo=?", Conexion, Transaccion))
                        {
                            Existente.Parameters.Add(new OdbcParameter("IdAplicacion", OdbcType.Int) { Value = IdAplicaciones[Indice] });
                            using (OdbcDataReader Fila = Existente.ExecuteReader())
                            {
                                if (Fila.Read())
                                {
                                    if (!string.Equals(Convert.ToString(Fila["estadoPagoVendedorComisionAbono"]), "REVERSADO", StringComparison.OrdinalIgnoreCase))
                                        AcumuladoComision += Convert.ToDecimal(Fila["montoComisionDevengadoComisionAbono"]);
                                    continue;
                                }
                            }
                        }

                        decimal MontoAbonoComision = Math.Max(0m, ComisionAcumuladaObjetivo - AcumuladoComision);
                        if (MontoAbonoComision <= 0m) continue;
                        decimal PorcentajePagado = Math.Round(Proporcion, 4, MidpointRounding.AwayFromZero);
                        using (OdbcCommand Insertar = new OdbcCommand(@"
                            INSERT INTO tblComisionAbono
                                (Fk_ComisionFacturaEncabezado_Codigo,Fk_AplicacionPago_Codigo,
                                 tasaFacturaPagadaAcumuladaComisionAbono,montoComisionDevengadoComisionAbono,estadoPagoVendedorComisionAbono)
                            VALUES (?,?,?,?,'PENDIENTE')", Conexion, Transaccion))
                        {
                            Insertar.Parameters.Add(new OdbcParameter("IdComision", OdbcType.Int) { Value = IdComision });
                            Insertar.Parameters.Add(new OdbcParameter("IdAplicacion", OdbcType.Int) { Value = IdAplicaciones[Indice] });
                                Insertar.Parameters.Add(new OdbcParameter("PorcentajePagado", OdbcType.Decimal) { Value = PorcentajePagado });
                                Insertar.Parameters.Add(new OdbcParameter("MontoComision", OdbcType.Decimal) { Value = MontoAbonoComision });
                            Insertar.ExecuteNonQuery();
                        }
                        AcumuladoComision += MontoAbonoComision;
                    }
                }
            }
        }

        public int ComisionVendedoresMetGuardarComisiones(List<ClsVentaVendedorBase> Comisiones, DateTime FechaInicio, DateTime FechaFin, string Metodo)
        {
            if (Comisiones == null || Comisiones.Count == 0) return 0;
            List<int> Ids = Comisiones.Where(Movimiento => Movimiento.IdComisionAbono > 0)
                .Select(Movimiento => Movimiento.IdComisionAbono).Distinct().ToList();
            if (Ids.Count == 0) return 0;
            int Guardados = 0;
            using (OdbcConnection Conexion = ComisionVendedoresMetObtenerConexion())
            {
                ComisionVendedoresMetAbrirConexion(Conexion);
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        foreach (int IdAbono in Ids)
                        {
                            using (OdbcCommand Actualizar = new OdbcCommand(@"
                                UPDATE tblComisionAbono
                                SET estadoPagoVendedorComisionAbono='PAGADO', fechaPagoVendedorComisionAbono=CURDATE()
                                WHERE Pk_ComisionAbono=? AND estadoPagoVendedorComisionAbono='PENDIENTE'", Conexion, Transaccion))
                            {
                                Actualizar.Parameters.Add(new OdbcParameter("IdAbono", OdbcType.Int) { Value = IdAbono });
                                Guardados += Actualizar.ExecuteNonQuery();
                            }
                        }
                        Transaccion.Commit();
                        return Guardados;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        private string ComisionVendedoresMetObtenerIdClasificacionSql(string Metodo)
        {
            switch (Metodo)
            {
                case "Producto": return "p.Pk_Producto";
                case "Marca": return "m.Pk_Marca";
                case "Linea": return "l.Pk_Linea";
                case "Categoria": return "c.Pk_Categoria";
                default: throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            }
        }

        private static string ComisionVendedoresMetNormalizarMetodo(string Metodo)
        {
            if (String.IsNullOrWhiteSpace(Metodo)) return String.Empty;
            string Valor = Metodo.Trim().Replace('í', 'i').Replace('Í', 'I')
                .Replace('á', 'a').Replace('Á', 'A').Replace('é', 'e').Replace('É', 'E');
            switch (Valor.ToUpperInvariant())
            {
                case "PRODUCTO": return "Producto";
                case "MARCA": return "Marca";
                case "LINEA": return "Linea";
                case "CATEGORIA": return "Categoria";
                default: return String.Empty;
            }
        }

        private string ComisionVendedoresMetObtenerPorcentajeSql(string Metodo)
        {
            switch (Metodo)
            {
                case "Producto": return "p.porcentajeComisionProducto";
                case "Marca": return "m.porcentajeComisionMarca";
                case "Linea": return "l.porcentajeComisionLinea";
                case "Categoria": return "c.porcentajeComisionCategoria";
                default: throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            }
        }

        private ClsFacturaComision ComisionVendedoresMetMapearFactura(DataRow Fila)
        {
            return new ClsFacturaComision
            {
                IdFactura = Convert.ToInt32(Fila["id"]),
                IdVendedor = Fila.IsNull("id_vendedor") ? 0 : Convert.ToInt32(Fila["id_vendedor"]),
                Fecha = Convert.ToDateTime(Fila["fecha"]), Cliente = Convert.ToString(Fila["cliente"]),
                Total = Convert.ToDecimal(Fila["total"]), TotalPagado = Convert.ToDecimal(Fila["total_pagado"]),
                UltimoAbono = Convert.ToDecimal(Fila["ultimo_abono"])
            };
        }

        private List<ClsFacturaComision> ComisionVendedoresMetMapearFacturas(DataTable Datos)
        {
            List<ClsFacturaComision> Resultado = new List<ClsFacturaComision>();
            foreach (DataRow Fila in Datos.Rows) Resultado.Add(ComisionVendedoresMetMapearFactura(Fila));
            return Resultado;
        }
    }
}
