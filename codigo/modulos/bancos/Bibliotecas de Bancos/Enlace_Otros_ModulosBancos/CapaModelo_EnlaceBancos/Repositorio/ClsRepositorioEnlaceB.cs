using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using CapaModelo_EnlaceB.Contratos;
using CapaModelo_EnlaceB.Entidades;

namespace CapaModelo_EnlaceB.Repositorio
{
    public sealed class ClsRepositorioEnlaceB : IClsRepositorioEnlaceB
    {
        private readonly string _Conexion;
        public ClsRepositorioEnlaceB() : this("Dsn=EmbutidosS.A")
        {
        }

        public ClsRepositorioEnlaceB(string Conexion)
        {
            this._Conexion = Conexion;
        }

        private static OdbcCommand BancarioMetComando(OdbcConnection Conexion, OdbcTransaction Transaccion, string SentenciaSql, params object[] Args)
        {
            var ComandoOdbc = new OdbcCommand(SentenciaSql, Conexion, Transaccion)
            {
                CommandTimeout = 30
            };
            foreach (object Value in Args)
            {
                OdbcType Tipo = Value is DateTime ? OdbcType.DateTime : Value is decimal ? OdbcType.Decimal : Value is long ? OdbcType.BigInt : Value is int ? OdbcType.Int : OdbcType.VarChar;
                var Parametro = ComandoOdbc.Parameters.Add("p" + ComandoOdbc.Parameters.Count, Tipo);
                if (Tipo == OdbcType.Decimal)
                {
                    Parametro.Precision = 12;
                    Parametro.Scale = 4;
                }

                if (Tipo == OdbcType.VarChar)
                    Parametro.Size = Math.Max(1, Convert.ToString(Value).Length);
                Parametro.Value = Value ?? DBNull.Value;
            }

            return ComandoOdbc;
        }

        private static DataTable BancarioMetTabla(OdbcConnection Conexion, OdbcTransaction Transaccion, string SentenciaSql, params object[] Args)
        {
            using (var ComandoOdbc = BancarioMetComando(Conexion, Transaccion, SentenciaSql, Args))
            using (var Adapter = new OdbcDataAdapter(ComandoOdbc))
            {
                var TablaDatos = new DataTable();
                Adapter.Fill(TablaDatos);
                return TablaDatos;
            }
        }

        private static object BancarioMetEscalar(OdbcConnection Conexion, OdbcTransaction Transaccion, string SentenciaSql, params object[] Args)
        {
            using (var ComandoOdbc = BancarioMetComando(Conexion, Transaccion, SentenciaSql, Args))
                return ComandoOdbc.ExecuteScalar();
        }

        private static void BancarioMetEjecutar(OdbcConnection Conexion, OdbcTransaction Transaccion, string SentenciaSql, params object[] Args)
        {
            using (var ComandoOdbc = BancarioMetComando(Conexion, Transaccion, SentenciaSql, Args))
                ComandoOdbc.ExecuteNonQuery();
        }

        private DataTable BancarioMetLeer(string SentenciaSql, params object[] Args)
        {
            using (var Conexion = new OdbcConnection(_Conexion))
            {
                Conexion.Open();
                return BancarioMetTabla(Conexion, null, SentenciaSql, Args);
            }
        }

        public DataTable BancarioMetConsultarCatalogo(string Catalogo)
        {
            switch (Catalogo)
            {
                case "empresa":
                    return BancarioMetLeer("SELECT idEmpresa AS Id, razonSocialEmpresa AS Nombre FROM tblEmpresa WHERE is_active=1 ORDER BY razonSocialEmpresa");
                case "pais":
                    return BancarioMetLeer("SELECT idPais AS Id, CONCAT(nombrePais,' (',codigoMonedaPais,')') AS Nombre FROM tblPais WHERE is_active=1 ORDER BY nombrePais");
                case "modulo":
                    return BancarioMetLeer("SELECT idModuloContable AS Id, CONCAT(codigoModuloContable,' - ',nombreModuloContable) AS Nombre FROM tblModuloContable WHERE is_active=1 AND UPPER(codigoModuloContable) IN ('MOD-BANCO','BANCOS','BAN','8000') ORDER BY nombreModuloContable");
                default:
                    throw new ArgumentException("Catálogo no permitido.");
            }
        }

        public DataTable BancarioMetConsultarModulo(ModuloEnlace Modulo, DateTime Inicio, DateTime Fin)
        {
            string SentenciaSql;
            switch (Modulo)
            {
                case ModuloEnlace.Logistica:
                    SentenciaSql = @"SELECT s.idSolicitudPagoTransporte AS SolicitudId, s.codigoSolicitudPagoTransporte AS Solicitud,
                        t.numeroGuiaServicioTransporteExterno AS Guia, e.nombreEmpresaTransportista AS Beneficiario,
                        t.fechaContratacionServicioTransporteExterno AS Fecha, c.monedaCostoTransporte AS Moneda,
                        c.costoTotalCostoTransporte AS Importe, c.tipoPagoCostoTransporte AS Medio,
                        es.nombreEstadoSolicitudPago AS EstadoSolicitud, t.estadoServicioTransporteExterno AS EstadoServicio,
                        o.idOrdenPago AS OrdenId, o.estadoOrdenPago AS EstadoOrden,
                        m.idMovimientoBancario AS MovimientoId, s.referenciaBancariaSolicitudPagoTransporte AS ReferenciaBanco,
                        s.fechaPagoSolicitudPagoTransporte AS FechaPago
                        FROM tblSolicitudPagoTransporte s
                        JOIN tblServicioTransporteExterno t ON t.idServicioTransporteExterno=s.Fk_Servicio_Transporte_Externo_Codigo
                        JOIN tblEmpresaTransportista e ON e.idEmpresaTransportista=t.Fk_Empresa_Transportista_Codigo
                        JOIN tblEstadoSolicitudPago es ON es.idEstadoSolicitudPago=s.Fk_Estado_Solicitud_Pago_Codigo
                        LEFT JOIN tblCostoTransporte c ON c.Fk_Servicio_Transporte_Externo_Codigo=t.idServicioTransporteExterno
                        LEFT JOIN tblOrdenPago o ON o.Fk_SolicitudPagoTransporte_Codigo=s.idSolicitudPagoTransporte
                        LEFT JOIN tblMovimientoBancario m ON m.Fk_OrdenPago_Codigo=o.idOrdenPago
                        WHERE t.fechaContratacionServicioTransporteExterno>=? AND t.fechaContratacionServicioTransporteExterno<?
                        ORDER BY s.idSolicitudPagoTransporte DESC, o.idOrdenPago";
                    break;
                case ModuloEnlace.Compras:
                    // Normalizar todas las columnas de texto del UNION sin cambiar el esquema.
                    SentenciaSql = @"SELECT CONVERT('Compras: orden original' USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Origen,
                        CAST(oc.idOrdenCompra AS CHAR CHARACTER SET utf8mb4) COLLATE utf8mb4_unicode_ci AS Documento,
                        CONVERT(p.nombreProveedor USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Proveedor,
                        oc.fechaEmisionOrdenCompra AS Fecha, CONVERT(mo.codigoMoneda USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Moneda,
                        oc.totalOrdenCompra AS Importe, CONVERT(oc.estadoOrdenCompra USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Estado,
                        oc.bloqueadoOrdenCompra AS Bloqueada,
                        CONVERT('Documento de Compras; la recepción bancaria no tiene FK directa a esta orden.' USING utf8mb4) COLLATE utf8mb4_unicode_ci AS RelacionBancaria
                        FROM tblEncabezadoOrdenCompra oc JOIN tblProveedor p ON p.idProveedor=oc.idProveedor
                        JOIN tblMoneda mo ON mo.idMoneda=oc.idMoneda
                        WHERE oc.fechaEmisionOrdenCompra>=? AND oc.fechaEmisionOrdenCompra<?
                        UNION ALL
                        SELECT CONVERT('Bancos: documento recibido' USING utf8mb4) COLLATE utf8mb4_unicode_ci,
                        CONVERT(r.referenciaExternaOrdenCompraRecibida USING utf8mb4) COLLATE utf8mb4_unicode_ci,
                        CONVERT(r.proveedorOrdenCompraRecibida USING utf8mb4) COLLATE utf8mb4_unicode_ci,
                        r.fechaOrdenCompraRecibida, CONVERT(mo.codigoMoneda USING utf8mb4) COLLATE utf8mb4_unicode_ci,
                        r.importeOrdenCompraRecibida,CONVERT(r.estadoOrdenCompraRecibida USING utf8mb4) COLLATE utf8mb4_unicode_ci,NULL,
                        CONVERT('Recepción existente: referencia externa, sin asociación automática por ID.' USING utf8mb4) COLLATE utf8mb4_unicode_ci
                        FROM tblOrdenCompraRecibida r JOIN tblMoneda mo ON mo.idMoneda=r.Fk_Moneda_Codigo
                        WHERE r.fechaOrdenCompraRecibida>=? AND r.fechaOrdenCompraRecibida<? ORDER BY Fecha DESC,Documento";
                    break;
                case ModuloEnlace.Ventas:
                    SentenciaSql = @"SELECT d.idDepositoVentaEncabezado AS DepositoId, d.codigoDepositoVentaEncabezado AS Deposito,
                        d.fechaDepositoVentaEncabezado AS Fecha, mo.codigoMoneda AS Moneda,
                        d.totalDepositoVentaEncabezado AS Importe, d.estadoDepositoVentaEncabezado AS Estado,
                        c.numeroCuentaBancaria AS Cuenta, d.idMovimientoBancario AS MovimientoId,
                        m.referenciaMovimientoBancario AS ReferenciaBanco,
                        m.importeMovimientoBancario AS ImporteMovimiento,
                        CASE WHEN d.idMovimientoBancario IS NULL THEN 'Sin movimiento vinculado'
                        WHEN m.Fk_CuentaBancaria_Codigo<>d.idCuentaBancaria OR m.importeMovimientoBancario<>d.totalDepositoVentaEncabezado
                        OR m.sentidoMovimientoBancario<>1 THEN 'Revisar cuenta, importe o sentido'
                        ELSE 'Movimiento asociado' END AS Verificacion
                        FROM tblDepositoVentaEncabezado d JOIN tblMoneda mo ON mo.idMoneda=d.idMoneda
                        JOIN tblCuentaBancaria c ON c.idCuentaBancaria=d.idCuentaBancaria
                        LEFT JOIN tblMovimientoBancario m ON m.idMovimientoBancario=d.idMovimientoBancario
                        WHERE d.fechaDepositoVentaEncabezado>=? AND d.fechaDepositoVentaEncabezado<?
                        ORDER BY d.idDepositoVentaEncabezado DESC";
                    break;
                case ModuloEnlace.RecursosHumanos:
                    SentenciaSql = @"SELECT p.Pk_PlanillaRecibida AS PlanillaId, p.referenciaExternaPlanillaRecibida AS Planilla,
                        p.inicioPeriodoPlanillaRecibida AS Inicio, p.finPeriodoPlanillaRecibida AS Fin,
                        mo.codigoMoneda AS Moneda, p.estadoPlanillaRecibida AS EstadoPlanilla,
                        d.idDetallePlanillaRecibida AS DetalleId, d.referenciaEmpleadoDetallePlanillaRecibida AS Empleado,
                        d.beneficiarioDetallePlanillaRecibida AS Beneficiario, d.importeNetoDetallePlanillaRecibida AS Neto,
                        o.idOrdenPago AS OrdenId, o.estadoOrdenPago AS EstadoOrden, o.medioOrdenPago AS Medio,
                        m.idMovimientoBancario AS MovimientoId
                        FROM tblPlanillaRecibida p JOIN tblMoneda mo ON mo.idMoneda=p.Fk_Moneda_Codigo
                        LEFT JOIN tblDetallePlanillaRecibida d ON d.Fk_PlanillaRecibida_Codigo=p.Pk_PlanillaRecibida
                        LEFT JOIN tblOrdenPago o ON o.Fk_DetallePlanillaRecibida_Codigo=d.idDetallePlanillaRecibida
                        LEFT JOIN tblMovimientoBancario m ON m.Fk_OrdenPago_Codigo=o.idOrdenPago
                        WHERE p.inicioPeriodoPlanillaRecibida>=? AND p.inicioPeriodoPlanillaRecibida<?
                        ORDER BY p.Pk_PlanillaRecibida DESC, d.idDetallePlanillaRecibida, o.idOrdenPago";
                    break;
                default:
                    throw new ArgumentException("Módulo no permitido.");
            }

            return Modulo == ModuloEnlace.Compras ? BancarioMetLeer(SentenciaSql, Inicio.Date, Fin.Date.AddDays(1), Inicio.Date, Fin.Date.AddDays(1)) : BancarioMetLeer(SentenciaSql, Inicio.Date, Fin.Date.AddDays(1));
        }

        public DataTable BancarioMetConsultarPolizas(DateTime Inicio, DateTime Fin)
        {
            return BancarioMetLeer(@"SELECT p.idPolizaBancaria AS PolizaId, p.fechaPolizaBancaria AS Fecha,
                m.referenciaMovimientoBancario AS Referencia, m.conceptoMovimientoBancario AS Concepto,
                c.numeroCuentaBancaria AS Cuenta, mo.codigoMoneda AS Moneda,
                m.importeMovimientoBancario AS Importe, p.estadoPolizaBancaria AS Estado,
                v.Fk_PartidaConsolidada_Codigo AS PartidaId,
                CASE WHEN v.Fk_PartidaConsolidada_Codigo IS NOT NULL THEN 'Ya incluida'
                WHEN mo.codigoMoneda<>'GTQ' THEN 'Requiere política de conversión'
                WHEN p.estadoPolizaBancaria<>'VALIDADA' THEN 'Pendiente de validación'
                WHEN c.is_active<>1 THEN 'Cuenta inactiva' ELSE 'Disponible' END AS Resultado
                FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario=p.Fk_MovimientoBancario_Codigo
                JOIN tblCuentaBancaria c ON c.idCuentaBancaria=m.Fk_CuentaBancaria_Codigo
                JOIN tblMoneda mo ON mo.idMoneda=c.Fk_Moneda_Codigo
                LEFT JOIN tblPartidaConsolidadaPolizaBancaria v ON v.Fk_PolizaBancaria_Codigo=p.idPolizaBancaria
                WHERE p.fechaPolizaBancaria>=? AND p.fechaPolizaBancaria<? ORDER BY p.fechaPolizaBancaria,p.idPolizaBancaria", Inicio.Date, Fin.Date.AddDays(1));
        }

        public DataTable BancarioMetConsultarHistorico()
        {
            return BancarioMetLeer(@"SELECT p.idPartidaConsolidada AS Id, p.fechaInicioPartidaConsolidada AS Inicio,
                p.fechaFinPartidaConsolidada AS Fin, p.identificadorEmpresaPartidaConsolidada AS EmpresaId, e.razonSocialEmpresa AS Empresa, mo.codigoMoneda AS Moneda,
                p.totalDebePartidaConsolidada AS Debe, p.totalHaberPartidaConsolidada AS Haber,
                p.estadoPartidaConsolidada AS Estado, p.referenciaContabilidadPartidaConsolidada AS Referencia,
                p.fechaTrasladoPartidaConsolidada AS FechaTraslado FROM vwPartidaConsolidadaPeriodo p
                LEFT JOIN tblEmpresa e ON e.idEmpresa=p.identificadorEmpresaPartidaConsolidada
                JOIN tblMoneda mo ON mo.idMoneda=p.Fk_Moneda_Codigo ORDER BY p.idPartidaConsolidada DESC");
        }

        private static DataTable BancarioMetDetalle(OdbcConnection Conexion, OdbcTransaction Transaccion, long Id)
        {
            return BancarioMetTabla(Conexion, Transaccion, @"SELECT codigoCuentaContableDetallePartidaConsolidada AS CodigoCuentaDetallePartida,
                nombreCuentaContableDetallePartidaConsolidada AS NombreCuentaDetallePartida,
                debeDetallePartidaConsolidada AS DebeDetallePartida, haberDetallePartidaConsolidada AS HaberDetallePartida
                FROM tblDetallePartidaConsolidada WHERE Fk_PartidaConsolidada_Codigo=?
                ORDER BY codigoCuentaContableDetallePartidaConsolidada", Id);
        }

        public ClsPartidaEnlace BancarioMetObtenerPartida(long Id)
        {
            using (var Conexion = new OdbcConnection(_Conexion))
            {
                Conexion.Open();
                var TablaDatos = BancarioMetTabla(Conexion, null, "SELECT estadoPartidaConsolidada, referenciaContabilidadPartidaConsolidada FROM tblPartidaConsolidada WHERE Pk_PartidaConsolidada=?", Id);
                if (TablaDatos.Rows.Count != 1)
                    throw new InvalidOperationException("La partida ya no existe.");
                return new ClsPartidaEnlace
                {
                    Id = Id,
                    Estado = Convert.ToString(TablaDatos.Rows[0][0]),
                    Referencia = Convert.ToString(TablaDatos.Rows[0][1]),
                    Detalle = BancarioMetDetalle(Conexion, null, Id)
                };
            }
        }

        private static long BancarioMetValidarOperacion(OdbcConnection Conexion, OdbcTransaction Transaccion, ClsContextoPartida Contexto, bool Envio)
        {
            ClsReglasEnlace.BancarioMetValidarContexto(Contexto);
            var Usuario = BancarioMetTabla(Conexion, Transaccion, "SELECT idUsuario FROM tblusuario WHERE idUsuario=? AND nombreUsuario=? AND is_active=1", Contexto.UsuarioId, Contexto.Responsable);
            if (Usuario.Rows.Count != 1)
                throw new InvalidOperationException("Inicie sesión con un usuario activo antes de generar o enviar.");
            string CampoUsuario = Envio ? "derEditarUsuarioModuloAplicacion" : "derInsertarUsuarioModuloAplicacion";
            string CampoRol = Envio ? "derEditarRolModuloAplicacion" : "derInsertarRolModuloAplicacion";
            var Directo = BancarioMetTabla(Conexion, Transaccion, "SELECT u." + CampoUsuario + " FROM tblusuariomoduloaplicacion u JOIN tblaplicacion a ON a.idAplicacion=u.idAplicacion AND a.idModulo=u.idModulo WHERE u.idUsuario=? AND a.idAplicacion=8008 AND a.is_active=1", Contexto.UsuarioId);
            bool Permiso = Directo.Rows.Count > 0 ? Convert.ToInt32(Directo.Rows[0][0]) == 1 : Convert.ToInt32(BancarioMetEscalar(Conexion, Transaccion, "SELECT COUNT(*) FROM tblusuariorol ur JOIN tblrol r ON r.idRol=ur.idRol AND r.is_active=1 JOIN tblrolmoduloaplicacion rm ON rm.idRol=ur.idRol JOIN tblaplicacion a ON a.idAplicacion=rm.idAplicacion AND a.idModulo=rm.idModulo WHERE ur.idUsuario=? AND a.idAplicacion=8008 AND a.is_active=1 AND rm." + CampoRol + "=1", Contexto.UsuarioId)) > 0;
            if (!Permiso)
                throw new InvalidOperationException("Seguridad no autoriza esta acción en la aplicación 8008. Revise sus permisos.");
            var Empresas = BancarioMetTabla(Conexion, Transaccion, "SELECT idEmpresa FROM tblEmpresa WHERE is_active=1 ORDER BY idEmpresa FOR UPDATE");
            if (Empresas.Rows.Count != 1 || Convert.ToInt32(Empresas.Rows[0][0]) != Contexto.Empresa)
                throw new InvalidOperationException("El traslado requiere una única empresa activa: las cuentas bancarias no identifican empresa en este esquema.");
            var Pais = BancarioMetTabla(Conexion, Transaccion, "SELECT codigoMonedaPais FROM tblPais WHERE idPais=? AND is_active=1", Contexto.Pais);
            if (Pais.Rows.Count != 1 || Convert.ToString(Pais.Rows[0][0]) != "GTQ")
                throw new InvalidOperationException("Esta operación requiere país con moneda GTQ; otras monedas necesitan una política acordada.");
            if (Convert.ToInt32(BancarioMetEscalar(Conexion, Transaccion, "SELECT COUNT(*) FROM tblModuloContable WHERE idModuloContable=? AND is_active=1 AND UPPER(codigoModuloContable) IN ('MOD-BANCO','BANCOS','BAN','8000')", Contexto.ModuloContable)) != 1)
                throw new InvalidOperationException("Seleccione el módulo contable de Bancos (código MOD-BANCO, BANCOS, BAN u 8000), según el catálogo existente.");
            object Moneda = BancarioMetEscalar(Conexion, Transaccion, "SELECT idMoneda FROM tblMoneda WHERE codigoMoneda='GTQ' AND is_active=1");
            if (Moneda == null)
                throw new InvalidOperationException("No existe la moneda GTQ activa.");
            return Convert.ToInt64(Moneda);
        }

        private static DataTable BancarioMetConsolidar(OdbcConnection Conexion, OdbcTransaction Transaccion, DataTable Polizas, int Modulo, int Pais)
        {
            var Agrupado = new SortedDictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow Parametro in Polizas.Rows)
            {
                long Id = Convert.ToInt64(Parametro["Id"]);
                var Lineas = BancarioMetTabla(Conexion, Transaccion, @"SELECT codigoCuentaContableDetallePolizaBancaria AS Codigo,
                    debeDetallePolizaBancaria AS Debe, haberDetallePolizaBancaria AS Haber
                    FROM tblDetallePolizaBancaria WHERE Fk_PolizaBancaria_Codigo=? FOR UPDATE", Id);
                decimal Debe = 0, Haber = 0;
                foreach (DataRow L in Lineas.Rows)
                {
                    string Codigo = Convert.ToString(L["Codigo"]);
                    decimal ImporteDebe = Convert.ToDecimal(L["Debe"]), ImporteHaber = Convert.ToDecimal(L["Haber"]);
                    if (ImporteDebe < 0 || ImporteHaber < 0 || ((ImporteDebe > 0) == (ImporteHaber > 0)))
                        throw new InvalidOperationException("La póliza " + Id + " tiene una línea inválida.");
                    if (!Agrupado.ContainsKey(Codigo))
                        Agrupado[Codigo] = new decimal[2];
                    Agrupado[Codigo][0] += ImporteDebe;
                    Agrupado[Codigo][1] += ImporteHaber;
                    Debe += ImporteDebe;
                    Haber += ImporteHaber;
                }

                if (Debe <= 0 || Debe != Haber || Debe != Convert.ToDecimal(Parametro["Importe"]))
                    throw new InvalidOperationException("La póliza " + Id + " no cuadra con el importe de su movimiento GTQ.");
            }

            var Resultado = new DataTable();
            Resultado.Columns.Add("CodigoCuentaDetallePartida", typeof(string));
            Resultado.Columns.Add("NombreCuentaDetallePartida", typeof(string));
            Resultado.Columns.Add("DebeDetallePartida", typeof(decimal));
            Resultado.Columns.Add("HaberDetallePartida", typeof(decimal));
            foreach (var Par in Agrupado)
            {
                var Cuenta = BancarioMetCuenta(Conexion, Transaccion, Par.Key, Modulo, Pais);
                Resultado.Rows.Add(Par.Key, Convert.ToString(Cuenta["Nombre"]), Par.Value[0], Par.Value[1]);
            }

            ClsReglasEnlace.BancarioMetPrepararLineasContables(Resultado);
            return Resultado;
        }

        private static DataRow BancarioMetCuenta(OdbcConnection Conexion, OdbcTransaction Transaccion, string Codigo, int Modulo, int Pais)
        {
            var TablaDatos = BancarioMetTabla(Conexion, Transaccion, @"SELECT c.idCuenta AS Id, c.nombreCuenta AS Nombre FROM tblCuenta c
                JOIN tblModuloCuentaPais mp ON mp.idCuenta=c.idCuenta
                WHERE c.codigoCuenta=? AND c.is_active=1 AND c.clasificacionCuenta='DETALLE'
                AND mp.idModuloContable=? AND mp.idPais=? AND mp.is_active=1 FOR UPDATE", Codigo, Modulo, Pais);
            if (TablaDatos.Rows.Count != 1)
                throw new InvalidOperationException("Cuenta no habilitada para módulo/país: " + Codigo);
            return TablaDatos.Rows[0];
        }

        private static decimal BancarioMetSumar(DataTable TablaDatos, string Campo)
        {
            return TablaDatos.Rows.Cast<DataRow>().Sum(Registro => Convert.ToDecimal(Registro[Campo]));
        }

        private static void BancarioMetAuditar(OdbcConnection Conexion, OdbcTransaction Transaccion, ClsContextoPartida Contexto, long Id, string Accion, string Detalle)
        {
            if (Id > int.MaxValue)
                throw new InvalidOperationException("El ID de partida supera la capacidad de la bitácora existente.");
            BancarioMetEjecutar(Conexion, Transaccion, "INSERT INTO tblbitacora (idUsuario,accionBitacora,tablaBitacora,idRegistroBitacora,detallesBitacora,ipBitacora) VALUES (?,?,?,?,?,?)", Contexto.UsuarioId, Accion, "tblPartidaConsolidada", (int)Id, Detalle, string.IsNullOrWhiteSpace(Contexto.Ip) ? "127.0.0.1" : Contexto.Ip);
        }

        public ClsPartidaEnlace BancarioMetGenerarPartida(ClsContextoPartida Contexto)
        {
            using (var Conexion = new OdbcConnection(_Conexion))
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        long Moneda = BancarioMetValidarOperacion(Conexion, Transaccion, Contexto, false);
                        var Previa = BancarioMetTabla(Conexion, Transaccion, "SELECT Pk_PartidaConsolidada AS Id FROM tblPartidaConsolidada WHERE identificadorEmpresaPartidaConsolidada=? AND fechaInicioPartidaConsolidada=? AND Fk_Moneda_Codigo=? FOR UPDATE", Contexto.Empresa, Contexto.Inicio.Date, Moneda);
                        if (Previa.Rows.Count > 0)
                            throw new InvalidOperationException("Ya existe la partida del mes. Selecciónela en Historial para consultarla o enviarla.");
                        var Polizas = BancarioMetTabla(Conexion, Transaccion, @"SELECT p.idPolizaBancaria AS Id, m.importeMovimientoBancario AS Importe
                            FROM tblPolizaBancaria p JOIN tblMovimientoBancario m ON m.idMovimientoBancario=p.Fk_MovimientoBancario_Codigo
                            JOIN tblCuentaBancaria c ON c.idCuentaBancaria=m.Fk_CuentaBancaria_Codigo
                            WHERE p.fechaPolizaBancaria>=? AND p.fechaPolizaBancaria<? AND p.estadoPolizaBancaria='VALIDADA'
                            AND c.Fk_Moneda_Codigo=? AND c.is_active=1
                            AND NOT EXISTS(SELECT 1 FROM tblPartidaConsolidadaPolizaBancaria v WHERE v.Fk_PolizaBancaria_Codigo=p.idPolizaBancaria)
                            ORDER BY p.idPolizaBancaria FOR UPDATE", Contexto.Inicio.Date, Contexto.Fin.Date.AddDays(1), Moneda);
                        if (Polizas.Rows.Count == 0)
                            throw new InvalidOperationException("No hay pólizas GTQ validadas y disponibles en el mes.");
                        var Detalle = BancarioMetConsolidar(Conexion, Transaccion, Polizas, Contexto.ModuloContable, Contexto.Pais);
                        string Clave = "BANC-" + Contexto.Empresa + "-" + Contexto.Inicio.ToString("yyyyMM", CultureInfo.InvariantCulture) + "-GTQ";
                        BancarioMetEjecutar(Conexion, Transaccion, @"INSERT INTO tblPartidaConsolidada (identificadorEmpresaPartidaConsolidada,fechaInicioPartidaConsolidada,
                            conceptoPartidaConsolidada,Fk_Moneda_Codigo,totalDebePartidaConsolidada,totalHaberPartidaConsolidada,
                            estadoPartidaConsolidada,identificadorUsuarioGeneraPartidaConsolidada,claveTrasladoPartidaConsolidada)
                            VALUES (?,?,?,?,?,?,'VALIDADA',?,?)", Contexto.Empresa, Contexto.Inicio.Date, "Partida de Bancos " + Contexto.Inicio.ToString("yyyy-MM"), Moneda, BancarioMetSumar(Detalle, "DebeDetallePartida"), BancarioMetSumar(Detalle, "HaberDetallePartida"), Contexto.Responsable, Clave);
                        long Id = Convert.ToInt64(BancarioMetEscalar(Conexion, Transaccion, "SELECT LAST_INSERT_ID()"));
                        foreach (DataRow ImporteDebe in Detalle.Rows)
                            BancarioMetEjecutar(Conexion, Transaccion, @"INSERT INTO tblDetallePartidaConsolidada
                            (Fk_PartidaConsolidada_Codigo,codigoCuentaContableDetallePartidaConsolidada,nombreCuentaContableDetallePartidaConsolidada,
                            debeDetallePartidaConsolidada,haberDetallePartidaConsolidada) VALUES (?,?,?,?,?)", Id, Convert.ToString(ImporteDebe[0]), Convert.ToString(ImporteDebe[1]), Convert.ToDecimal(ImporteDebe[2]), Convert.ToDecimal(ImporteDebe[3]));
                        foreach (DataRow Parametro in Polizas.Rows)
                        {
                            BancarioMetEjecutar(Conexion, Transaccion, "INSERT INTO tblPartidaConsolidadaPolizaBancaria (Fk_PartidaConsolidada_Codigo,Fk_PolizaBancaria_Codigo) VALUES (?,?)", Id, Convert.ToInt64(Parametro["Id"]));
                            BancarioMetEjecutar(Conexion, Transaccion, "UPDATE tblPolizaBancaria SET estadoPolizaBancaria='PENDIENTE_ENVIO' WHERE idPolizaBancaria=?", Convert.ToInt64(Parametro["Id"]));
                        }

                        BancarioMetAuditar(Conexion, Transaccion, Contexto, Id, "GENERAR_PARTIDA", "Partida GTQ validada; pólizas: " + Polizas.Rows.Count);
                        Transaccion.Commit();
                        return new ClsPartidaEnlace
                        {
                            Id = Id,
                            Estado = "VALIDADA",
                            Detalle = Detalle
                        };
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        private static bool BancarioMetIguales(DataTable Guardado, DataTable Actual)
        {
            if (Guardado.Rows.Count != Actual.Rows.Count)
                return false;
            var PorCodigo = Actual.Rows.Cast<DataRow>().ToDictionary(Fila => Convert.ToString(Fila["CodigoCuentaDetallePartida"]), StringComparer.OrdinalIgnoreCase);
            foreach (DataRow Fila in Guardado.Rows)
            {
                DataRow Cuenta;
                if (!PorCodigo.TryGetValue(Convert.ToString(Fila["CodigoCuentaDetallePartida"]), out Cuenta) || Convert.ToDecimal(Cuenta["DebeDetallePartida"]) != Convert.ToDecimal(Fila["DebeDetallePartida"]) || Convert.ToDecimal(Cuenta["HaberDetallePartida"]) != Convert.ToDecimal(Fila["HaberDetallePartida"]))
                    return false;
            }

            return true;
        }

        public ClsResultadoEnvioContable BancarioMetEnviarPartida(long Id, ClsContextoPartida Contexto, Func<ClsEnvioContable, ClsResultadoEnvioContable> Recibir)
        {
            ClsEnvioContable Envio;
            var Existente = BancarioMetRevisarEnvio(Id, Contexto, null, out Envio);
            if (Existente != null) return Existente;

            // Contabilidad confirma su propia transacción. Libere primero los bloqueos de Bancos.
            var Respuesta = Recibir(Envio);
            try
            {
                return BancarioMetRevisarEnvio(Id, Contexto, Respuesta, out Envio);
            }
            catch (Exception Ex)
            {
                throw new InvalidOperationException("Contabilidad respondió con la póliza " + Respuesta.Codigo
                    + ". Falta confirmar el traslado en Bancos. Reintente la misma partida. " + Ex.Message, Ex);
            }
        }

        private ClsResultadoEnvioContable BancarioMetRevisarEnvio(long Id, ClsContextoPartida Contexto,
            ClsResultadoEnvioContable Respuesta, out ClsEnvioContable Envio)
        {
            Envio = null;
            using (var Conexion = new OdbcConnection(_Conexion))
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    var Resultado = BancarioMetPrepararOConfirmar(Conexion, Transaccion, Id, Contexto, Respuesta, out Envio);
                    Transaccion.Commit();
                    return Resultado;
                }
            }
        }

        private static ClsResultadoEnvioContable BancarioMetPrepararOConfirmar(OdbcConnection Conexion, OdbcTransaction Transaccion,
            long Id, ClsContextoPartida Contexto, ClsResultadoEnvioContable Respuesta, out ClsEnvioContable Envio)
        {
            Envio = null;
            long Moneda = BancarioMetValidarOperacion(Conexion, Transaccion, Contexto, true);
            var TablaDatos = BancarioMetTabla(Conexion, Transaccion, "SELECT * FROM tblPartidaConsolidada WHERE Pk_PartidaConsolidada=? FOR UPDATE", Id);
            if (TablaDatos.Rows.Count != 1)
                throw new InvalidOperationException("No existe la partida seleccionada.");
            var Parametro = TablaDatos.Rows[0];
            string Estado = Convert.ToString(Parametro["estadoPartidaConsolidada"]);
            if (Convert.ToInt32(Parametro["identificadorEmpresaPartidaConsolidada"]) != Contexto.Empresa || Convert.ToDateTime(Parametro["fechaInicioPartidaConsolidada"]).Date != Contexto.Inicio.Date || Convert.ToInt64(Parametro["Fk_Moneda_Codigo"]) != Moneda)
                throw new InvalidOperationException("La selección no coincide con la empresa, moneda y mes de la partida.");
            string Origen = "BANC-PC-" + Id.ToString(CultureInfo.InvariantCulture);
            var Recibida = BancarioMetTabla(Conexion, Transaccion, "SELECT idPolizaEncabezado,codigoPolizaEncabezado,estadoPolizaEncabezado,periodoContablePolizaEncabezado,idPais FROM tblPolizaEncabezado WHERE idModuloContable=? AND documentoOrigenPolizaEncabezado=? FOR UPDATE", Contexto.ModuloContable, Origen);
            if (Estado != "VALIDADA" && Estado != "ERROR" && Estado != "TRASLADADA")
                throw new InvalidOperationException("La partida no está lista para enviar.");
            var Detalle = BancarioMetDetalle(Conexion, Transaccion, Id);
            var Lineas = ClsReglasEnlace.BancarioMetPrepararLineasContables(Detalle);
            if (BancarioMetSumar(Detalle, "DebeDetallePartida") != Convert.ToDecimal(Parametro["totalDebePartidaConsolidada"]) || BancarioMetSumar(Detalle, "HaberDetallePartida") != Convert.ToDecimal(Parametro["totalHaberPartidaConsolidada"]))
                throw new InvalidOperationException("Los totales no coinciden con el detalle guardado.");
            if (Estado == "TRASLADADA")
            {
                if (Recibida.Rows.Count != 1 || Convert.ToString(Recibida.Rows[0]["codigoPolizaEncabezado"]) != Convert.ToString(Parametro["referenciaContabilidadPartidaConsolidada"]))
                    throw new InvalidOperationException("No se pudo verificar el receptor de la partida trasladada.");
            }
            else
            {
                BancarioMetValidarFuentes(Conexion, Transaccion, Id, Moneda, Contexto, Detalle);
            }

            if (Recibida.Rows.Count > 1)
                throw new InvalidOperationException("Hay varias pólizas receptoras para el mismo origen.");

            var Cuentas = BancarioMetObtenerCuentas(Conexion, Transaccion, Lineas, Contexto, Recibida.Rows.Count == 0);
            if (Recibida.Rows.Count == 0)
            {
                if (Respuesta != null)
                    throw new InvalidOperationException("La póliza recibida no aparece en la base conectada a Bancos.");
                Envio = new ClsEnvioContable
                {
                    CodigoModulo = Convert.ToString(BancarioMetEscalar(Conexion, Transaccion,
                        "SELECT codigoModuloContable FROM tblModuloContable WHERE idModuloContable=?", Contexto.ModuloContable)),
                    Pais = Contexto.Pais,
                    Fecha = Contexto.Fin.Date,
                    DocumentoOrigen = Origen,
                    Concepto = Convert.ToString(Parametro["conceptoPartidaConsolidada"]),
                    Lineas = Lineas
                };
                return null;
            }

            string Codigo = BancarioMetVerificarReceptor(Conexion, Transaccion, Recibida, Detalle, Cuentas, Contexto);
            string EstadoContable = Convert.ToString(Recibida.Rows[0]["estadoPolizaEncabezado"]);
            if (Estado != "TRASLADADA" && EstadoContable != "VALIDADA" && EstadoContable != "APLICADA")
                throw new InvalidOperationException("El documento ya existe en Contabilidad con estado " + EstadoContable
                    + ". Revise esa póliza antes de confirmar el traslado.");
            if (Respuesta != null && !string.Equals(Codigo, Respuesta.Codigo, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("La referencia recibida no coincide con la póliza del documento.");

            var Resultado = new ClsResultadoEnvioContable
            {
                Codigo = Codigo,
                Estado = EstadoContable,
                Mensaje = Respuesta != null && Respuesta.Estado == EstadoContable
                    ? Respuesta.Mensaje : "Póliza " + Codigo + " registrada en Contabilidad. Estado: " + EstadoContable + "."
            };
            if (Estado != "TRASLADADA")
                BancarioMetRegistrarTraslado(Conexion, Transaccion, Id, Codigo, EstadoContable, Contexto);
            return Resultado;
        }

        private static void BancarioMetValidarFuentes(OdbcConnection Conexion, OdbcTransaction Transaccion, long Id, long Moneda, ClsContextoPartida Contexto, DataTable Detalle)
        {
            var Fuentes = BancarioMetTabla(Conexion, Transaccion, @"SELECT pb.idPolizaBancaria AS Id, m.importeMovimientoBancario AS Importe,
                pb.estadoPolizaBancaria AS Estado, c.Fk_Moneda_Codigo AS Moneda, pb.fechaPolizaBancaria AS Fecha
                FROM tblPartidaConsolidadaPolizaBancaria v JOIN tblPolizaBancaria pb ON pb.idPolizaBancaria=v.Fk_PolizaBancaria_Codigo
                JOIN tblMovimientoBancario m ON m.idMovimientoBancario=pb.Fk_MovimientoBancario_Codigo
                JOIN tblCuentaBancaria c ON c.idCuentaBancaria=m.Fk_CuentaBancaria_Codigo
                WHERE v.Fk_PartidaConsolidada_Codigo=? ORDER BY pb.idPolizaBancaria FOR UPDATE", Id);
            if (Fuentes.Rows.Count == 0 || Fuentes.Rows.Cast<DataRow>().Any(Registro => Convert.ToString(Registro["Estado"]) != "PENDIENTE_ENVIO" || Convert.ToInt64(Registro["Moneda"]) != Moneda || Convert.ToDateTime(Registro["Fecha"]).Date < Contexto.Inicio.Date || Convert.ToDateTime(Registro["Fecha"]).Date > Contexto.Fin.Date))
                throw new InvalidOperationException("Cambió el estado, moneda o fecha de las pólizas fuente.");
            if (!BancarioMetIguales(Detalle, BancarioMetConsolidar(Conexion, Transaccion, Fuentes, Contexto.ModuloContable, Contexto.Pais)))
                throw new InvalidOperationException("Las pólizas fuente cambiaron después de generar la partida.");
        }

        private static Dictionary<string, int> BancarioMetObtenerCuentas(OdbcConnection Conexion, OdbcTransaction Transaccion, DataTable Lineas, ClsContextoPartida Contexto, bool ValidarPeriodo)
        {
            var Cuentas = new Dictionary<string, int>();
            foreach (DataRow Linea in Lineas.Rows)
            {
                string Codigo = Convert.ToString(Linea["Codigo"]);
                if (!Cuentas.ContainsKey(Codigo))
                    Cuentas[Codigo] = Convert.ToInt32(BancarioMetCuenta(Conexion, Transaccion, Codigo, Contexto.ModuloContable, Contexto.Pais)["Id"]);
            }

            if (!ValidarPeriodo) return Cuentas;
            foreach (int Cuenta in Cuentas.Values)
                if (Convert.ToInt32(BancarioMetEscalar(Conexion, Transaccion, "SELECT COUNT(*) FROM tblSaldoCuentaPeriodo WHERE idCuenta=? AND periodoContableSaldoCuentaPeriodo=? AND estadoSaldoCuentaPeriodo='CERRADO'", Cuenta, Contexto.Inicio.ToString("yyyy-MM"))) > 0)
                    throw new InvalidOperationException("Hay cuentas con período contable cerrado.");
            return Cuentas;
        }

        private static string BancarioMetVerificarReceptor(OdbcConnection Conexion, OdbcTransaction Transaccion, DataTable Recibida, DataTable Detalle, Dictionary<string, int> Cuentas, ClsContextoPartida Contexto)
        {
            var Encabezado = Recibida.Rows[0];
            long Receptor = Convert.ToInt64(Encabezado["idPolizaEncabezado"]);
            string Codigo = Convert.ToString(Encabezado["codigoPolizaEncabezado"]);
            if (Convert.ToString(Encabezado["estadoPolizaEncabezado"]) == "ANULADA" || Convert.ToString(Encabezado["periodoContablePolizaEncabezado"]) != Contexto.Inicio.ToString("yyyy-MM") || Convert.ToInt32(Encabezado["idPais"]) != Contexto.Pais)
                throw new InvalidOperationException("El receptor existente no coincide o está anulado.");
            var Recibido = BancarioMetTabla(Conexion, Transaccion, "SELECT idCuenta, SUM(debePolizaDetalle) AS Debe, SUM(haberPolizaDetalle) AS Haber FROM tblPolizaDetalle WHERE idPolizaEncabezado=? AND is_active=1 GROUP BY idCuenta", Receptor);
            var PorCuenta = Recibido.Rows.Cast<DataRow>().ToDictionary(Fila => Convert.ToInt32(Fila["idCuenta"]));
            if (PorCuenta.Count != Detalle.Rows.Count)
                throw new InvalidOperationException("La cantidad de cuentas recibidas difiere de la partida.");
            foreach (DataRow Fila in Detalle.Rows)
            {
                DataRow RecibidaCuenta;
                int IdCuenta = Cuentas[Convert.ToString(Fila["CodigoCuentaDetallePartida"])];
                if (!PorCuenta.TryGetValue(IdCuenta, out RecibidaCuenta) || Convert.ToDecimal(RecibidaCuenta["Debe"]) != Convert.ToDecimal(Fila["DebeDetallePartida"]) || Convert.ToDecimal(RecibidaCuenta["Haber"]) != Convert.ToDecimal(Fila["HaberDetallePartida"]))
                    throw new InvalidOperationException("El detalle contable existente no coincide con la partida.");
            }

            return Codigo;
        }

        private static void BancarioMetRegistrarTraslado(OdbcConnection Conexion, OdbcTransaction Transaccion, long Id, string Codigo, string EstadoContable, ClsContextoPartida Contexto)
        {
            BancarioMetEjecutar(Conexion, Transaccion, @"UPDATE tblPartidaConsolidada SET estadoPartidaConsolidada='TRASLADADA',
                identificadorUsuarioTrasladaPartidaConsolidada=?,fechaTrasladoPartidaConsolidada=CURRENT_TIMESTAMP,
                referenciaContabilidadPartidaConsolidada=?,mensajeResultadoTrasladoPartidaConsolidada=? WHERE Pk_PartidaConsolidada=?", Contexto.Responsable, Codigo, "Estado en Contabilidad: " + EstadoContable, Id);
            BancarioMetEjecutar(Conexion, Transaccion, @"UPDATE tblPolizaBancaria pb JOIN tblPartidaConsolidadaPolizaBancaria v ON v.Fk_PolizaBancaria_Codigo=pb.idPolizaBancaria
                SET pb.estadoPolizaBancaria='ACEPTADA',pb.referenciaContabilidadPolizaBancaria=?,pb.fechaEnvioContabilidadPolizaBancaria=CURRENT_TIMESTAMP
                WHERE v.Fk_PartidaConsolidada_Codigo=?", Codigo, Id);
            BancarioMetAuditar(Conexion, Transaccion, Contexto, Id, "ENVIAR_PARTIDA", "Recibida como " + Codigo + ". Estado: " + EstadoContable);
        }
    }
}
