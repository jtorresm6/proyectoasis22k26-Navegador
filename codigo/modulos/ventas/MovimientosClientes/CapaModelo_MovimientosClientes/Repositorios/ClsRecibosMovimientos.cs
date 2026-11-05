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
    WHERE Pk_Cliente=?
      AND activoCliente=1";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncRegistrarReciboConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.fechaEmisionFacturaEncabezado fechaEmision
    FROM tblFacturaEncabezado f
    JOIN tblClienteFacturaVersion c ON c.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
    WHERE f.Pk_FacturaEncabezado=?
      AND c.Fk_Cliente_Codigo=?
      AND f.Fk_Moneda_Codigo=?
      AND f.estadoFacturaEncabezado='EMITIDA'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncRegistrarReciboInsertarPago(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblReciboPago(numeroReciboPago,Fk_Cliente_Codigo,Fk_Moneda_Codigo,fechaReciboPago,montoReciboPago,referenciaReciboPago,estadoReciboPago,observacionesReciboPago,Fk_Usuario_Registro,Fk_FormaPagoVenta_Codigo)
    SELECT ?,?,?,?,?,?,'CONFIRMADO',?,?,fp.Pk_FormaPagoVenta FROM tblFormaPagoVenta fp WHERE fp.codigoFormaPagoVenta=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncRegistrarReciboInsertarAplicacionpago(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblAplicacionPago(Fk_ReciboPago_Codigo,Fk_FacturaEncabezado_Codigo,fechaAplicacionPago,montoAplicadoAplicacionPago,estadoAplicacionPago,Fk_Usuario_Registro)
    VALUES (?,?,?,?,'APLICADA',?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public DataTable VentasFuncPrepararDepositoConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT montoReciboPago montoPago,fechaReciboPago fechaPago
    FROM tblReciboPago
    WHERE Pk_ReciboPago=?
      AND Fk_Moneda_Codigo=?
      AND estadoReciboPago='CONFIRMADO'
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncPrepararDepositoConsultarDepositoventasdetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT COALESCE(SUM(importeDepositoVentaDetalle),0)
    FROM tblDepositoVentaDetalle
    WHERE Fk_ReciboPago_Codigo=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncPrepararDepositoInsertarDepositoventas(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblDepositoVentaEncabezado(codigoDepositoVentaEncabezado,fechaDepositoVentaEncabezado,referenciaDepositoVentaEncabezado,Fk_Moneda_Codigo,totalDepositoVentaEncabezado,Fk_Usuario_Registro,Fk_CuentaBancaria_Codigo,Fk_TipoTransaccion_Codigo)
    SELECT ?,?,?,?,?,?,cb.Pk_CuentaBancaria,t.Pk_TipoTransaccion
    FROM tblCuentaBancaria cb JOIN tblBanco b ON b.Pk_Banco=cb.Fk_Banco_Codigo
    CROSS JOIN tblTipoTransaccion t
    WHERE (cb.numeroCuentaBancaria=? OR CAST(cb.Pk_CuentaBancaria AS CHAR)=?)
      AND (b.nombreBanco=? OR b.codigoBanco=? OR CAST(b.Pk_Banco AS CHAR)=?)
      AND (t.codigoTipoTransaccion=? OR t.nombreTipoTransaccion=? OR CAST(t.Pk_TipoTransaccion AS CHAR)=?)
      AND cb.is_active=1 AND b.is_active=1 AND t.is_active=1 AND cb.Fk_Moneda_Codigo=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncPrepararDepositoInsertarDepositoventasdetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblDepositoVentaDetalle(Fk_DepositoVentaEncabezado_Codigo,Fk_ReciboPago_Codigo,importeDepositoVentaDetalle)
    VALUES (?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public DataTable VentasFuncModificarReciboConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT Fk_Moneda_Codigo idMoneda,fechaReciboPago fechaPago
    FROM tblReciboPago
    WHERE Pk_ReciboPago=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncModificarReciboConsultarPago2(params object[] Parametros)
        {
            const string Consulta = @"SELECT estadoReciboPago estadoPago
    FROM tblReciboPago
    WHERE Pk_ReciboPago=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncModificarReciboActualizarPago(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblReciboPago
    SET observacionesReciboPago=?
    WHERE Pk_ReciboPago=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public object VentasFuncAnularReciboConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT Fk_Moneda_Codigo
    FROM tblReciboPago
    WHERE Pk_ReciboPago=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularReciboConsultarPago2(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_ReciboPago idPago,Fk_Moneda_Codigo idMoneda,fechaReciboPago fechaPago,estadoReciboPago estadoPago
    FROM tblReciboPago
    WHERE Pk_ReciboPago=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncAnularReciboConsultarDepositoventasdetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblDepositoVentaDetalle
    WHERE Fk_ReciboPago_Codigo=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncAnularReciboConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT m.Pk_MovimientoCxC idMovimiento,m.Fk_FacturaEncabezado_Codigo idFactura,m.cargoMovimientoCxC cargo,m.abonoMovimientoCxC abono
    FROM tblMovimientoCxC m
    JOIN tblAplicacionPago a ON a.Pk_AplicacionPago=m.Fk_AplicacionPago_Codigo
    WHERE a.Fk_ReciboPago_Codigo=?
      AND a.estadoAplicacionPago='APLICADA'
    ORDER BY m.Fk_FacturaEncabezado_Codigo
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncAnularReciboActualizarAplicacionpago(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblAplicacionPago
    SET estadoAplicacionPago='REVERTIDA'
    WHERE Fk_ReciboPago_Codigo=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncAnularReciboActualizarPago(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblReciboPago
    SET estadoReciboPago='ANULADO'
    WHERE Pk_ReciboPago=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
