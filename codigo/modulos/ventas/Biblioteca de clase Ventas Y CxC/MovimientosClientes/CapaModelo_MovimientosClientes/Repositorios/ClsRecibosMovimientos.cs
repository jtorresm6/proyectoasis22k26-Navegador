// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_MovimientosClientes
{
    public sealed class ClsRecibosMovimientos
    {
        private readonly ClsDatosVentas _Datos;

        internal ClsRecibosMovimientos(ClsDatosVentas Datos)
        {
            _Datos = Datos;
        }

        public object VentasFuncRegistrarReciboConsultarCliente(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblCliente
    WHERE idCliente=?
      AND activoCliente=1";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncRegistrarReciboConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.fechaEmisionFacturaEncabezado fechaEmision
    FROM tblFacturaEncabezado f
    JOIN tblClienteFacturaVersion c ON c.idClienteFacturaVersion=f.idClienteFacturaVersion
    WHERE f.idFacturaEncabezado=?
      AND c.idCliente=?
      AND f.idMoneda=?
      AND f.estadoFacturaEncabezado='EMITIDA'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncRegistrarReciboInsertarPago(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblReciboPago(numeroReciboPago,idCliente,idMoneda,fechaReciboPago,montoReciboPago,referenciaReciboPago,estadoReciboPago,observacionesReciboPago,idUsuarioRegistro,idFormaPagoVenta)
    SELECT ?,?,?,?,?,?,'CONFIRMADO',?,?,fp.idFormaPagoVenta FROM tblFormaPagoVenta fp WHERE fp.codigoFormaPagoVenta=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncRegistrarReciboInsertarAplicacionpago(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblAplicacionPago(idReciboPago,idFacturaEncabezado,fechaAplicacionPago,montoAplicadoAplicacionPago,estadoAplicacionPago,idUsuarioRegistro)
    VALUES (?,?,?,?,'APLICADA',?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public DataTable VentasFuncPrepararDepositoConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT montoReciboPago montoPago,fechaReciboPago fechaPago
    FROM tblReciboPago
    WHERE idReciboPago=?
      AND idMoneda=?
      AND estadoReciboPago='CONFIRMADO'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncPrepararDepositoConsultarDepositoventasdetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT COALESCE(SUM(importeDepositoVentaDetalle),0)
    FROM tblDepositoVentaDetalle
    WHERE idReciboPago=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncPrepararDepositoInsertarDepositoventas(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblDepositoVentaEncabezado(codigoDepositoVentaEncabezado,fechaDepositoVentaEncabezado,referenciaDepositoVentaEncabezado,idMoneda,totalDepositoVentaEncabezado,idUsuarioRegistro,idCuentaBancaria,idTipoTransaccion)
    SELECT ?,?,?,?,?,?,cb.idCuentaBancaria,t.idTipoTransaccion
    FROM tblCuentaBancaria cb JOIN tblBanco b ON b.idBanco=cb.Fk_Banco_Codigo
    CROSS JOIN tblTipoTransaccion t
    WHERE (cb.numeroCuentaBancaria=? OR CAST(cb.idCuentaBancaria AS CHAR)=?)
      AND (b.nombreBanco=? OR b.codigoBanco=? OR CAST(b.idBanco AS CHAR)=?)
      AND (t.codigoTipoTransaccion=? OR t.nombreTipoTransaccion=? OR CAST(t.idTipoTransaccion AS CHAR)=?)
      AND cb.is_active=1 AND b.is_active=1 AND t.is_active=1 AND cb.Fk_Moneda_Codigo=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncPrepararDepositoInsertarDepositoventasdetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblDepositoVentaDetalle(idDepositoVentaEncabezado,idReciboPago,importeDepositoVentaDetalle)
    VALUES (?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public void VentasMetEnviarDepositoBancos(int Deposito)
        {
            // Se ejecuta dentro de la misma transaccion que reserva los recibos.
            // MANUAL es el origen admitido por Bancos para ingresos externos al estado de cuenta.
            const string Insertar = @"INSERT INTO tblMovimientoBancario
                (referenciaMovimientoBancario, conceptoMovimientoBancario,
                 importeMovimientoBancario, importeGTQMovimientoBancario,
                 tipoCambioAplicadoMovimientoBancario, sentidoMovimientoBancario,
                 fechaOperacionMovimientoBancario, fechaValorMovimientoBancario,
                 origenMovimientoBancario, Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo)
                SELECT d.referenciaDepositoVentaEncabezado,
                       CONCAT('Deposito de Ventas ',d.codigoDepositoVentaEncabezado),
                       d.totalDepositoVentaEncabezado,d.totalDepositoVentaEncabezado,1,1,
                       d.fechaDepositoVentaEncabezado,d.fechaDepositoVentaEncabezado,
                       'MANUAL',d.idCuentaBancaria,d.idTipoTransaccion
                FROM tblDepositoVentaEncabezado d
                JOIN tblMoneda m ON m.idMoneda=d.idMoneda
                WHERE d.idDepositoVentaEncabezado=? AND m.codigoMoneda='GTQ'
                  AND d.estadoDepositoVentaEncabezado='PREPARADO'
                  AND d.idMovimientoBancario IS NULL";
            if (_Datos.VentasFuncEjecutar(Insertar, Deposito) != 1)
                throw new InvalidOperationException("El deposito no esta disponible para enviarlo a Bancos.");

            int Movimiento = _Datos.VentasFuncObtenerId();
            _Datos.VentasFuncEjecutar(@"UPDATE tblDepositoVentaEncabezado
                SET idMovimientoBancario=?,estadoDepositoVentaEncabezado='ENVIADO'
                WHERE idDepositoVentaEncabezado=?", Movimiento, Deposito);
        }

        public DataTable VentasFuncModificarReciboConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT idMoneda idMoneda,fechaReciboPago fechaPago
    FROM tblReciboPago
    WHERE idReciboPago=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncModificarReciboConsultarPago2(params object[] Parametros)
        {
            const string Consulta = @"SELECT estadoReciboPago estadoPago
    FROM tblReciboPago
    WHERE idReciboPago=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncModificarReciboActualizarPago(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblReciboPago
    SET observacionesReciboPago=?
    WHERE idReciboPago=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public object VentasFuncAnularReciboConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT idMoneda
    FROM tblReciboPago
    WHERE idReciboPago=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularReciboConsultarPago2(params object[] Parametros)
        {
            const string Consulta = @"SELECT idReciboPago idPago,idMoneda idMoneda,fechaReciboPago fechaPago,estadoReciboPago estadoPago
    FROM tblReciboPago
    WHERE idReciboPago=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncAnularReciboConsultarDepositoventasdetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblDepositoVentaDetalle
    WHERE idReciboPago=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularReciboConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT m.idMovimientoCxC idMovimiento,m.idFacturaEncabezado idFactura,m.cargoMovimientoCxC cargo,m.abonoMovimientoCxC abono
    FROM tblMovimientoCxC m
    JOIN tblAplicacionPago a ON a.idAplicacionPago=m.idAplicacionPago
    WHERE a.idReciboPago=?
      AND a.estadoAplicacionPago='APLICADA'
    ORDER BY m.idFacturaEncabezado
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncAnularReciboActualizarAplicacionpago(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblAplicacionPago
    SET estadoAplicacionPago='REVERTIDA'
    WHERE idReciboPago=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncAnularReciboActualizarPago(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblReciboPago
    SET estadoReciboPago='ANULADO'
    WHERE idReciboPago=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
