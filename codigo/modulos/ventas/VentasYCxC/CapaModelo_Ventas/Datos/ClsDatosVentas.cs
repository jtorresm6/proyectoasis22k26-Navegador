// Jose Javier Torres Martinez | 0901-23-1091 | 20/10/2026
// Fuente enlazada en cada Modelo: no agrega una DLL compartida.
using System;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

namespace VentasDatos
{
    internal sealed class ClsDatosVentas : IDisposable
    {
        private readonly OdbcConnection _Conexion;
        private OdbcTransaction _Transaccion;
        internal bool EnTransaccion
        {
            get
            {
                return _Transaccion != null;
            }
        }

        internal ClsDatosVentas()
        {
            _Conexion = new OdbcConnection(CapaModelo_Ventas.ClsConexion.CadenaConexion);
            try
            {
                _Conexion.Open();
            }
            catch
            {
                _Conexion.Dispose();
                throw;
            }
        }

        internal void VentasMetIniciar()
        {
            _Transaccion = _Conexion.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        internal void VentasMetConfirmar()
        {
            _Transaccion.Commit();
            _Transaccion.Dispose();
            _Transaccion = null;
        }

        private OdbcCommand VentasFuncComando(string SentenciaSql, object[] Valores)
        {
            if (SentenciaSql.Split('?').Length - 1 != Valores.Length)
                throw new ArgumentException("Cantidad de parametros SQL incorrecta.");
            var Comando = new OdbcCommand(SentenciaSql, _Conexion, _Transaccion)
            {
                CommandTimeout = 20
            };
            foreach (object Valor in Valores)
            {
                var Parametro = new OdbcParameter();
                // ODBC 8 ANSI falla al enlazar SQL_C_NUMERIC despues de varios parametros.
                // Representacion decimal exacta e invariante; MySQL la guarda en columnas DECIMAL.
                // No convertir a double ni interpolar importes en el SQL.
                if (Valor is decimal)
                {
                    Parametro.OdbcType = OdbcType.VarChar;
                    Parametro.Size = 64;
                    Parametro.Value = ((decimal)Valor).ToString(CultureInfo.InvariantCulture);
                    Comando.Parameters.Add(Parametro);
                    continue;
                }

                if (Valor is DateTime)
                    Parametro.OdbcType = OdbcType.DateTime;
                Parametro.Value = Valor ?? DBNull.Value;
                Comando.Parameters.Add(Parametro);
            }

            return Comando;
        }

        internal DataTable VentasFuncTabla(string SentenciaSql, params object[] Valores)
        {
            using (var Comando = VentasFuncComando(SentenciaSql, Valores))
            using (var Adaptador = new OdbcDataAdapter(Comando))
            {
                var Tabla = new DataTable();
                Adaptador.Fill(Tabla);
                return Tabla;
            }
        }

        internal object VentasFuncEscalar(string SentenciaSql, params object[] Valores)
        {
            using (var Comando = VentasFuncComando(SentenciaSql, Valores))
                return Comando.ExecuteScalar();
        }

        internal int VentasFuncEjecutar(string SentenciaSql, params object[] Valores)
        {
            using (var Comando = VentasFuncComando(SentenciaSql, Valores))
                return Comando.ExecuteNonQuery();
        }

        internal int VentasFuncObtenerId()
        {
            return Convert.ToInt32(VentasFuncEscalar("SELECT LAST_INSERT_ID()"));
        }

        internal DataTable VentasFuncCatalogo(string Nombre)
        {
            switch (Nombre)
            {
                case "Clientes":
                    return this.VentasFuncTabla("SELECT Pk_Cliente Id,CONCAT(codigoCliente,' - ',nombreCliente) Nombre,COALESCE(nitCliente,dpiCliente,identificacionExtranjeraCliente,'CF') numeroIdentificacion,direccionCliente,nombreContactoCliente contactoCliente,diasCreditoCliente diasCredito,limiteCreditoCliente limiteCredito,Fk_Moneda_Credito idMonedaCredito FROM tblCliente WHERE activoCliente=1 ORDER BY nombreCliente");
                case "Monedas":
                    return this.VentasFuncTabla("SELECT Pk_Moneda Id,CONCAT(codigoMoneda,' - ',nombreMoneda) Nombre,codigoMoneda,simboloMoneda FROM tblMoneda WHERE is_active=1 ORDER BY codigoMoneda");
                case "Usuarios":
                    return this.VentasFuncTabla("SELECT idUsuario Id,nombreUsuario Nombre FROM tblUsuario WHERE is_active=1 ORDER BY nombreUsuario");
                case "Vendedores":
                    return this.VentasFuncTabla("SELECT v.Pk_Vendedor Id,CONCAT(v.codigoVendedor,' - ',e.nombresEmpleado,' ',e.apellidosEmpleado) Nombre FROM tblVendedor v JOIN tblEmpleado e ON e.idEmpleado=v.Fk_Empleado_Codigo WHERE v.activoVendedor=1 ORDER BY v.codigoVendedor");
                case "Establecimientos":
                    // Las facturas del esquema actual se asignan a una sucursal.
                    return this.VentasFuncTabla("SELECT Pk_Sucursal Id,nombreSucursal Nombre FROM tblSucursal ORDER BY nombreSucursal");
                default:
                    throw new ArgumentException("Catalogo no reconocido.");
            }
        }

        internal string VentasFuncNumero(string Prefijo)
        {
            // Se llama dentro de la transaccion; la fila serializa los correlativos por prefijo.
            this.VentasFuncEjecutar("INSERT INTO tblCorrelativoVenta(prefijoCorrelativoVenta,ultimoNumeroCorrelativoVenta) VALUES (?,0) ON DUPLICATE KEY UPDATE prefijoCorrelativoVenta=VALUES(prefijoCorrelativoVenta)", Prefijo);
            long UltimoNumero = Convert.ToInt64(this.VentasFuncEscalar("SELECT ultimoNumeroCorrelativoVenta FROM tblCorrelativoVenta WHERE prefijoCorrelativoVenta=? FOR UPDATE", Prefijo));
            string Buscar;
            switch (Prefijo)
            {
                case "COT":
                    Buscar = "SELECT COUNT(*) FROM tblCotizacionEncabezado WHERE numeroCotizacionEncabezado=?";
                    break;
                case "REC":
                    Buscar = "SELECT COUNT(*) FROM tblReciboPago WHERE numeroReciboPago=?";
                    break;
                case "CXC":
                    Buscar = "SELECT COUNT(*) FROM tblFacturaEncabezado WHERE serieFacturaEncabezado='CXC' AND numeroFacturaEncabezado=?";
                    break;
                case "NOT":
                    Buscar = "SELECT COUNT(*) FROM tblNotaCxC WHERE numeroNotaCxC=?";
                    break;
                case "DEP":
                    Buscar = "SELECT COUNT(*) FROM tblDepositoVentaEncabezado WHERE codigoDepositoVentaEncabezado=?";
                    break;
                case "CIE":
                    Buscar = "SELECT COUNT(*) FROM tblCierreMovimientoCxCEncabezado WHERE numeroCierreMovimientoCxCEncabezado=?";
                    break;
                case "PCOT":
                    Buscar = "SELECT COUNT(*) FROM tblPedidoEncabezado WHERE numeroPedidoEncabezado=?";
                    break;
                case "FCOT":
                    Buscar = "SELECT COUNT(*) FROM tblFacturaEncabezado WHERE serieFacturaEncabezado='COT' AND numeroFacturaEncabezado=?";
                    break;
                default:
                    throw new ArgumentException("Correlativo no reconocido.");
            }

            string Numero;
            do
            {
                Numero = Prefijo + "-" + (++UltimoNumero).ToString("D8", CultureInfo.InvariantCulture);
            }
            while (Convert.ToInt32(this.VentasFuncEscalar(Buscar, Numero)) > 0);
            this.VentasFuncEjecutar("UPDATE tblCorrelativoVenta SET ultimoNumeroCorrelativoVenta=? WHERE prefijoCorrelativoVenta=?", UltimoNumero, Prefijo);
            return Numero;
        }

        internal void VentasMetAuditar(string Entidad, int Id, string Accion, string Detalle, int Usuario)
        {
            if (_Transaccion == null)
                throw new InvalidOperationException("La auditoria requiere una transaccion.");
            string Direccion = "127.0.0.1";
            try
            {
                foreach (System.Net.IPAddress DireccionLocal in System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()))
                    if (DireccionLocal.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        Direccion = DireccionLocal.ToString();
                        break;
                    }
            }
            catch (System.Net.Sockets.SocketException) { }
            VentasFuncEjecutar("INSERT INTO tblbitacora(idUsuario,accionBitacora,tablaBitacora,idRegistroBitacora,detallesBitacora,ipBitacora) VALUES (?,?,?,?,?,?)",
                Usuario > 0 ? (object)Usuario : DBNull.Value, Accion, Entidad, Id, Detalle ?? "", Direccion);
        }

        public void Dispose()
        {
            if (_Transaccion != null)
            {
                try
                {
                    _Transaccion.Rollback();
                }
                catch (OdbcException)
                {
                }

                _Transaccion.Dispose();
            }

            _Conexion.Dispose();
        }
    }
}
