// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_MovimientosClientes
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
            return Convert.ToInt32(_Datos.VentasFuncEscalar("SELECT COUNT(*) FROM tblMoneda WHERE Pk_Moneda=? AND is_active=1", Moneda)) == 1;
        }

        public void VentasMetAsegurarBloqueoMoneda(int Moneda)
        {
            _Datos.VentasFuncEjecutar("INSERT INTO tblControlPeriodoCxC(Fk_Moneda_Codigo) VALUES (?) ON DUPLICATE KEY UPDATE Fk_Moneda_Codigo=VALUES(Fk_Moneda_Codigo)", Moneda);
            _Datos.VentasFuncTabla("SELECT Fk_Moneda_Codigo FROM tblControlPeriodoCxC WHERE Fk_Moneda_Codigo=? FOR UPDATE", Moneda);
        }

        public object VentasFuncUltimoCierre(int Moneda)
        {
            return _Datos.VentasFuncEscalar("SELECT fechaUltimoCierreControlPeriodoCxC FROM tblControlPeriodoCxC WHERE Fk_Moneda_Codigo=?", Moneda);
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
    WHERE Fk_FacturaEncabezado_Codigo=?
      AND fechaMovimientoCxC<=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncSaldoConsultarMovimientocxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT COALESCE(SUM(cargoMovimientoCxC-abonoMovimientoCxC),0)
    FROM tblMovimientoCxC
    WHERE Fk_FacturaEncabezado_Codigo=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncRegistrarDocumentoConsultarCliente(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_Cliente idCliente,tipoIdentificacionCliente tipoIdentificacion,
    COALESCE(nitCliente,dpiCliente,identificacionExtranjeraCliente,'CF') numeroIdentificacion,
    nombreCliente,direccionCliente,diasCreditoCliente diasCredito,nitCliente,dpiCliente,identificacionExtranjeraCliente
    FROM tblCliente
    WHERE Pk_Cliente=? AND activoCliente=1 FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncRegistrarDocumentoConsultarEstablecimientoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*) FROM tblSucursal WHERE Pk_Sucursal=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncRegistrarDocumentoInsertarFactura(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblFacturaEncabezado(serieFacturaEncabezado,numeroFacturaEncabezado,Fk_Sucursal_Codigo,Fk_Moneda_Codigo,fechaEmisionFacturaEncabezado,fechaVencimientoFacturaEncabezado,tipoDteFacturaEncabezado,condicionPagoFacturaEncabezado,estadoFacturaEncabezado,observacionesFacturaEncabezado,Fk_Usuario_Registro,Fk_ClienteFacturaVersion_Codigo)
    VALUES ('CXC',?,?,?,?,?,'INTERNO_CXC',?,'EMITIDA',?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncCrearVersionCliente(int Cliente)
        {
            const string Consulta = @"INSERT INTO tblClienteFacturaVersion(Fk_Cliente_Codigo,numeroVersionClienteFacturaVersion,tipoIdentificacionClienteFacturaVersion,nitClienteFacturaVersion,dpiClienteFacturaVersion,identificacionExtranjeraClienteFacturaVersion,nombreClienteFacturaVersion,direccionClienteFacturaVersion)
    SELECT c.Pk_Cliente,COALESCE((SELECT MAX(v.numeroVersionClienteFacturaVersion)+1 FROM tblClienteFacturaVersion v WHERE v.Fk_Cliente_Codigo=c.Pk_Cliente),1),c.tipoIdentificacionCliente,c.nitCliente,c.dpiCliente,c.identificacionExtranjeraCliente,c.nombreCliente,c.direccionCliente
    FROM tblCliente c WHERE c.Pk_Cliente=?";
            _Datos.VentasFuncEjecutar(Consulta, Cliente);
            return _Datos.VentasFuncObtenerId();
        }

        public DataTable VentasFuncRegistrarDocumentoConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.Pk_FacturaEncabezado idFactura,f.fechaEmisionFacturaEncabezado fechaEmision
    FROM tblFacturaEncabezado f
    JOIN tblClienteFacturaVersion c ON c.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
    WHERE c.Fk_Cliente_Codigo=?
      AND f.Fk_Moneda_Codigo=?
      AND CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado)=?
      AND f.estadoFacturaEncabezado='EMITIDA'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncRegistrarDocumentoInsertarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblNotaCxC(numeroNotaCxC,Fk_FacturaEncabezado_Codigo,tipoNotaCxC,fechaNotaCxC,montoNotaCxC,motivoNotaCxC,estadoNotaCxC,Fk_Usuario_Registro)
    VALUES (?,?,?,?,?,?,'APLICADA',?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncMovimientoInsertarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblMovimientoCxC(Fk_FacturaEncabezado_Codigo,fechaMovimientoCxC,tipoMovimientoCxC,descripcionMovimientoCxC,cargoMovimientoCxC,abonoMovimientoCxC,Fk_Usuario_Registro,Fk_AplicacionPago_Codigo,Fk_NotaCxC_Codigo,Fk_MovimientoCxC_Revertido)
    VALUES (?,?,?,?,?,?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncModificarDocumentoActualizarFactura(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblFacturaEncabezado
    SET observacionesFacturaEncabezado=?,fechaVencimientoFacturaEncabezado=?
    WHERE Pk_FacturaEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncModificarDocumentoActualizarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblNotaCxC
    SET motivoNotaCxC=?
    WHERE Pk_NotaCxC=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public object VentasFuncDisponibleConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT MIN(Saldo)
    FROM (SELECT fechaMovimientoCxC,SUM(cargoMovimientoCxC-abonoMovimientoCxC) OVER (ORDER BY fechaMovimientoCxC,Pk_MovimientoCxC ROWS UNBOUNDED PRECEDING) Saldo
    FROM tblMovimientoCxC
    WHERE Fk_FacturaEncabezado_Codigo=?) s
    WHERE fechaMovimientoCxC>?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncBloquearDocumentoConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_FacturaEncabezado idFactura,Fk_Moneda_Codigo idMoneda,fechaEmisionFacturaEncabezado Fecha
    FROM tblFacturaEncabezado
    WHERE Pk_FacturaEncabezado=? AND serieFacturaEncabezado='CXC'";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncBloquearDocumentoConsultarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.Pk_FacturaEncabezado idFactura,f.Fk_Moneda_Codigo idMoneda,n.fechaNotaCxC Fecha
    FROM tblNotaCxC n
    JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=n.Fk_FacturaEncabezado_Codigo
    WHERE n.Pk_NotaCxC=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncBloquearDocumentoConsultarFactura2(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_FacturaEncabezado idFactura,Fk_Moneda_Codigo idMoneda,
    fechaEmisionFacturaEncabezado fechaEmision,estadoFacturaEncabezado estadoFactura
    FROM tblFacturaEncabezado
    WHERE Pk_FacturaEncabezado=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncBloquearDocumentoConsultarNotacxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_NotaCxC idNota,Fk_FacturaEncabezado_Codigo idFactura,estadoNotaCxC estadoNota
    FROM tblNotaCxC
    WHERE Pk_NotaCxC=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularDocumentoConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT Fk_Moneda_Codigo idMoneda
    FROM tblFacturaEncabezado
    WHERE Pk_FacturaEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncAnularDocumentoConsultarAplicacionpago(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblAplicacionPago
    WHERE Fk_FacturaEncabezado_Codigo=?
      AND estadoAplicacionPago='APLICADA'";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncAnularDocumentoConsultarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblNotaCxC
    WHERE Fk_FacturaEncabezado_Codigo=?
      AND estadoNotaCxC='APLICADA'";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularDocumentoConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_MovimientoCxC idMovimiento,Fk_FacturaEncabezado_Codigo idFactura,cargoMovimientoCxC cargo,abonoMovimientoCxC abono
    FROM tblMovimientoCxC
    WHERE Fk_FacturaEncabezado_Codigo=?
      AND tipoMovimientoCxC='FACTURA'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularDocumentoConsultarMovimientocxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_MovimientoCxC idMovimiento,Fk_FacturaEncabezado_Codigo idFactura,cargoMovimientoCxC cargo,abonoMovimientoCxC abono
    FROM tblMovimientoCxC
    WHERE Fk_NotaCxC_Codigo=?
      AND tipoMovimientoCxC<>'REVERSO'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncAnularDocumentoConsultarMovimientocxc3(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblMovimientoCxC
    WHERE Fk_MovimientoCxC_Revertido=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncAnularDocumentoActualizarFactura(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblFacturaEncabezado
    SET estadoFacturaEncabezado='ANULADA'
    WHERE Pk_FacturaEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncAnularDocumentoActualizarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblNotaCxC
    SET estadoNotaCxC='ANULADA'
    WHERE Pk_NotaCxC=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
