// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_MovimientosClientes_Ventas
{
    public sealed class ClsMovimientosRepositorio : IDisposable
    {
        public ClsRecibosMovimientos Recibos { get; }
        public ClsConsultasMovimientos Consultas { get; }

        public ClsMovimientosRepositorio()
        {
            // Todos los grupos usan la misma conexion y transaccion.
            Recibos = new ClsRecibosMovimientos(_Datos);
            Consultas = new ClsConsultasMovimientos(_Datos);
        }

        private readonly ClsDatosVentas _Datos = new ClsDatosVentas();

        public bool EnTransaccion
        {
            get
            {
                return _Datos.EnTransaccion;
            }
        }

        public void VentasMetIniciar()
        {
            _Datos.VentasMetIniciar();
        }

        public void VentasMetConfirmar()
        {
            _Datos.VentasMetConfirmar();
        }

        public DataTable VentasFuncCatalogo(string Nombre)
        {
            if (Nombre == "BancosDeposito") return _Datos.VentasFuncTabla("SELECT idBanco Id,nombreBanco Nombre FROM tblBanco WHERE is_active=1");
            if (Nombre == "CuentasDeposito") return _Datos.VentasFuncTabla("SELECT idCuentaBancaria Id,CONCAT(numeroCuentaBancaria,' - ',nombreCuentaBancaria) Nombre,Fk_Banco_Codigo Banco,Fk_Moneda_Codigo Moneda FROM tblCuentaBancaria WHERE is_active=1");
            if (Nombre == "ConceptosDeposito") return _Datos.VentasFuncTabla("SELECT idTipoTransaccion Id,nombreTipoTransaccion Nombre FROM tblTipoTransaccion WHERE is_active=1 AND naturalezaTipoTransaccion IN ('INGRESO','AMBOS')");
            if (Nombre == "TiposDocumentoCxC")
            {
                // ERP v1.6 guarda el tipo en tblNotaCxC y tblMovimientoCxC.
                // El orden corresponde al índice que usa el formulario al registrar.
                var Tipos = new DataTable();
                Tipos.Columns.Add("Id", typeof(int));
                Tipos.Columns.Add("Nombre", typeof(string));
                Tipos.Columns.Add("Efecto", typeof(string));
                Tipos.Rows.Add(0, "Factura de CxC (sin productos)", "CARGO");
                Tipos.Rows.Add(1, "Nota de crédito (reduce deuda)", "ABONO");
                Tipos.Rows.Add(2, "Nota de débito (aumenta deuda)", "CARGO");
                Tipos.Rows.Add(3, "Ajuste de cargo (aumenta deuda)", "CARGO");
                Tipos.Rows.Add(4, "Ajuste de abono (reduce deuda)", "ABONO");
                return Tipos;
            }
            return _Datos.VentasFuncCatalogo(Nombre);
        }

        public int VentasFuncObtenerId()
        {
            return _Datos.VentasFuncObtenerId();
        }

        public string VentasFuncNumero(string Prefijo)
        {
            return _Datos.VentasFuncNumero(Prefijo);
        }

        public bool VentasFuncUsuarioActivo(int Usuario)
        {
            return Convert.ToInt32(_Datos.VentasFuncEscalar("SELECT COUNT(*) FROM tblusuario WHERE idUsuario=? AND is_active=1", Usuario)) == 1;
        }

        public bool VentasFuncMonedaActiva(int Moneda)
        {
            return Convert.ToInt32(_Datos.VentasFuncEscalar("SELECT COUNT(*) FROM tblMoneda WHERE idMoneda=? AND is_active=1", Moneda)) == 1;
        }

        public void VentasMetAsegurarBloqueoMoneda(int Moneda)
        {
            _Datos.VentasFuncEjecutar("INSERT INTO tblControlPeriodoCxC(idMoneda) VALUES (?) ON DUPLICATE KEY UPDATE idMoneda=VALUES(idMoneda)", Moneda);
            _Datos.VentasFuncTabla("SELECT idMoneda FROM tblControlPeriodoCxC WHERE idMoneda=? FOR UPDATE", Moneda);
        }

        public object VentasFuncUltimoCierre(int Moneda)
        {
            return _Datos.VentasFuncEscalar("SELECT fechaUltimoCierreControlPeriodoCxC FROM tblControlPeriodoCxC WHERE idMoneda=?", Moneda);
        }

        public void VentasMetAuditar(string Entidad, int Id, string Accion, string Detalle, int Usuario)
        {
            _Datos.VentasMetAuditar(Entidad, Id, Accion, Detalle, Usuario);
        }

        public void Dispose()
        {
            _Datos.Dispose();
        }

        public object VentasFuncSaldoConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT COALESCE(SUM(cargoMovimientoCxC-abonoMovimientoCxC),0)
    FROM tblMovimientoCxC
    WHERE idFacturaEncabezado=?
      AND fechaMovimientoCxC<=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncSaldoConsultarMovimientocxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT COALESCE(SUM(cargoMovimientoCxC-abonoMovimientoCxC),0)
    FROM tblMovimientoCxC
    WHERE idFacturaEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncRegistrarDocumentoConsultarCliente(params object[] Parametros)
        {
            const string Consulta = @"SELECT idCliente idCliente,tipoIdentificacionCliente tipoIdentificacion,
    COALESCE(nitCliente,dpiCliente,'CF') numeroIdentificacion,
    nombreCliente,direccionCliente,diasCreditoCliente diasCredito,nitCliente,dpiCliente
    FROM tblCliente
    WHERE idCliente=? AND activoCliente=1 FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncRegistrarDocumentoConsultarEstablecimientoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*) FROM tblSucursal WHERE idSucursal=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncRegistrarDocumentoInsertarFactura(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblFacturaEncabezado(uuidDteFacturaEncabezado,serieFacturaEncabezado,numeroFacturaEncabezado,idSucursal,idMoneda,fechaEmisionFacturaEncabezado,fechaVencimientoFacturaEncabezado,tipoDteFacturaEncabezado,condicionPagoFacturaEncabezado,estadoFacturaEncabezado,observacionesFacturaEncabezado,idUsuarioRegistro,idClienteFacturaVersion)
    VALUES (UUID(),'CXC',?,?,?,?,?,'INTERNO_CXC',?,'EMITIDA',?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncCrearVersionCliente(int Cliente)
        {
            const string Consulta = @"INSERT INTO tblClienteFacturaVersion(idCliente,numeroVersionClienteFacturaVersion,tipoIdentificacionClienteFacturaVersion,nitClienteFacturaVersion,dpiClienteFacturaVersion,nombreClienteFacturaVersion,direccionClienteFacturaVersion)
    SELECT c.idCliente,COALESCE((SELECT MAX(v.numeroVersionClienteFacturaVersion)+1 FROM tblClienteFacturaVersion v WHERE v.idCliente=c.idCliente),1),c.tipoIdentificacionCliente,c.nitCliente,c.dpiCliente,c.nombreCliente,c.direccionCliente
    FROM tblCliente c WHERE c.idCliente=?";
            _Datos.VentasFuncEjecutar(Consulta, Cliente);
            return _Datos.VentasFuncObtenerId();
        }

        public DataTable VentasFuncRegistrarDocumentoConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.idFacturaEncabezado idFactura,f.fechaEmisionFacturaEncabezado fechaEmision
    FROM tblFacturaEncabezado f
    JOIN tblClienteFacturaVersion c ON c.idClienteFacturaVersion=f.idClienteFacturaVersion
    WHERE c.idCliente=?
      AND f.idMoneda=?
      AND CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado)=?
      AND f.estadoFacturaEncabezado='EMITIDA'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncRegistrarDocumentoInsertarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblNotaCxC(numeroNotaCxC,idFacturaEncabezado,tipoNotaCxC,fechaNotaCxC,montoNotaCxC,motivoNotaCxC,estadoNotaCxC,idUsuarioRegistro)
    VALUES (?,?,?,?,?,?,'APLICADA',?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncMovimientoInsertarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblMovimientoCxC(idFacturaEncabezado,fechaMovimientoCxC,tipoMovimientoCxC,descripcionMovimientoCxC,cargoMovimientoCxC,abonoMovimientoCxC,idUsuarioRegistro,idAplicacionPago,idNotaCxC,idMovimientoCxCRevertido)
    VALUES (?,?,?,?,?,?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncModificarDocumentoActualizarFactura(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblFacturaEncabezado
    SET observacionesFacturaEncabezado=?,fechaVencimientoFacturaEncabezado=?
    WHERE idFacturaEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncModificarDocumentoActualizarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblNotaCxC
    SET motivoNotaCxC=?
    WHERE idNotaCxC=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public object VentasFuncDisponibleConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT MIN(Saldo)
    FROM (SELECT fechaMovimientoCxC,SUM(cargoMovimientoCxC-abonoMovimientoCxC) OVER (ORDER BY fechaMovimientoCxC,idMovimientoCxC ROWS UNBOUNDED PRECEDING) Saldo
    FROM tblMovimientoCxC
    WHERE idFacturaEncabezado=?) s
    WHERE fechaMovimientoCxC>?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncBloquearDocumentoConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT idFacturaEncabezado idFactura,idMoneda idMoneda,fechaEmisionFacturaEncabezado Fecha
    FROM tblFacturaEncabezado
    WHERE idFacturaEncabezado=? AND serieFacturaEncabezado='CXC'";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncBloquearDocumentoConsultarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.idFacturaEncabezado idFactura,f.idMoneda idMoneda,n.fechaNotaCxC Fecha
    FROM tblNotaCxC n
    JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=n.idFacturaEncabezado
    WHERE n.idNotaCxC=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncBloquearDocumentoConsultarFactura2(params object[] Parametros)
        {
            const string Consulta = @"SELECT idFacturaEncabezado idFactura,idMoneda idMoneda,
    fechaEmisionFacturaEncabezado fechaEmision,estadoFacturaEncabezado estadoFactura
    FROM tblFacturaEncabezado
    WHERE idFacturaEncabezado=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncBloquearDocumentoConsultarNotacxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT idNotaCxC idNota,idFacturaEncabezado idFactura,estadoNotaCxC estadoNota
    FROM tblNotaCxC
    WHERE idNotaCxC=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularDocumentoConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT idMoneda idMoneda
    FROM tblFacturaEncabezado
    WHERE idFacturaEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncAnularDocumentoConsultarAplicacionpago(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblAplicacionPago
    WHERE idFacturaEncabezado=?
      AND estadoAplicacionPago='APLICADA'";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncAnularDocumentoConsultarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblNotaCxC
    WHERE idFacturaEncabezado=?
      AND estadoNotaCxC='APLICADA'";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularDocumentoConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT idMovimientoCxC idMovimiento,idFacturaEncabezado idFactura,cargoMovimientoCxC cargo,abonoMovimientoCxC abono
    FROM tblMovimientoCxC
    WHERE idFacturaEncabezado=?
      AND tipoMovimientoCxC='FACTURA'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularDocumentoConsultarMovimientocxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT idMovimientoCxC idMovimiento,idFacturaEncabezado idFactura,cargoMovimientoCxC cargo,abonoMovimientoCxC abono
    FROM tblMovimientoCxC
    WHERE idNotaCxC=?
      AND tipoMovimientoCxC<>'REVERSO'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncAnularDocumentoConsultarMovimientocxc3(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblMovimientoCxC
    WHERE idMovimientoCxCRevertido=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncAnularDocumentoActualizarFactura(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblFacturaEncabezado
    SET estadoFacturaEncabezado='ANULADA'
    WHERE idFacturaEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncAnularDocumentoActualizarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblNotaCxC
    SET estadoNotaCxC='ANULADA'
    WHERE idNotaCxC=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
