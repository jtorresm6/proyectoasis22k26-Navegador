/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 09/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Consultar saldos e historial de cierres.
 */
// Natali Sofía Montenegro Portillo | Carné 0901-23-10017 | Cierre de movimientos | 09/11/2026
// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_CierreMovimientos
{
    public sealed class ClsConsultasCierre
    {
        private readonly ClsDatosVentas _Datos;

        internal ClsConsultasCierre(ClsDatosVentas Datos)
        {
            _Datos = Datos;
        }

        // Saldos: consultas parametrizadas.
        public DataTable VentasFuncSaldosConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.idCliente idCliente,c.codigoCliente Codigo,c.nombreCliente Nombre,SUM(CASE WHEN m.fechaMovimientoCxC<? THEN m.cargoMovimientoCxC-m.abonoMovimientoCxC ELSE 0 END) Anterior,SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.cargoMovimientoCxC ELSE 0 END) Cargos,SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.abonoMovimientoCxC ELSE 0 END) Abonos,SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.cargoMovimientoCxC ELSE 0 END) CargosAcumulados,SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.abonoMovimientoCxC ELSE 0 END) AbonosAcumulados,SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC) Actual
    FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=m.idFacturaEncabezado JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion JOIN tblCliente c ON c.idCliente=cv.idCliente
    WHERE f.idMoneda=? AND m.fechaMovimientoCxC<=?
    GROUP BY c.idCliente,c.codigoCliente,c.nombreCliente ORDER BY c.codigoCliente";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Listar: consultas parametrizadas.
        public DataTable VentasFuncListarConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.idCierreMovimientoCxCEncabezado Id,c.idMoneda idMoneda,c.numeroCierreMovimientoCxCEncabezado Numero,m.codigoMoneda Moneda,c.tipoCierreMovimientoCxCEncabezado Tipo,c.fechaDesdeCierreMovimientoCxCEncabezado Desde,c.fechaHastaCierreMovimientoCxCEncabezado Hasta,c.estadoCierreMovimientoCxCEncabezado Estado,COALESCE(c.observacionesCierreMovimientoCxCEncabezado,'') Observaciones
    FROM tblCierreMovimientoCxCEncabezado c JOIN tblMoneda m ON m.idMoneda=c.idMoneda
    WHERE (?=0 OR c.idMoneda=?) ORDER BY c.fechaDesdeCierreMovimientoCxCEncabezado DESC,c.idCierreMovimientoCxCEncabezado DESC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Historial: consultas parametrizadas.
        public DataTable VentasFuncHistorialConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT h.idCierreMovimientoCxCEncabezado idCierre,h.numeroCierreMovimientoCxCEncabezado numeroCierre,CONCAT(h.tipoCierreMovimientoCxCEncabezado,' ',DATE_FORMAT(h.fechaDesdeCierreMovimientoCxCEncabezado,'%Y-%m')) Periodo,c.codigoClienteCierreSaldoCliente Codigo,c.nombreClienteCierreSaldoCliente Nombre,c.saldoAnteriorCierreSaldoCliente Anterior,c.cargosPeriodoCierreSaldoCliente Cargos,c.abonosPeriodoCierreSaldoCliente Abonos,c.cargosAcumuladosCierreSaldoCliente CargosAcumulados,c.abonosAcumuladosCierreSaldoCliente AbonosAcumulados,c.saldoActualCierreSaldoCliente Actual,h.estadoCierreMovimientoCxCEncabezado Estado
    FROM tblCierreMovimientoCxCEncabezado h JOIN tblCierreSaldoCliente c ON c.idCierreMovimientoCxCEncabezado=h.idCierreMovimientoCxCEncabezado
    WHERE h.idMoneda=? AND (?=0 OR c.idCliente=?) AND DATE_FORMAT(h.fechaDesdeCierreMovimientoCxCEncabezado,'%Y-%m') LIKE CONCAT(?,'%')
    ORDER BY h.fechaDesdeCierreMovimientoCxCEncabezado DESC,h.idCierreMovimientoCxCEncabezado,c.codigoClienteCierreSaldoCliente";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // EstadoPeriodo: consultas parametrizadas.
        public DataTable VentasFuncEstadoPeriodoConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT idCierreMovimientoCxCEncabezado idCierre,numeroCierreMovimientoCxCEncabezado numeroCierre,idMoneda idMoneda,estadoCierreMovimientoCxCEncabezado estadoCierre
    FROM tblCierreMovimientoCxCEncabezado WHERE idMoneda=? AND tipoCierreMovimientoCxCEncabezado=? AND fechaDesdeCierreMovimientoCxCEncabezado=? AND fechaHastaCierreMovimientoCxCEncabezado=? AND estadoCierreMovimientoCxCEncabezado='CONFIRMADO'";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }
    }
}
