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
            const string Consulta = @"SELECT c.Pk_Cliente idCliente,c.codigoCliente Codigo,c.nombreCliente Nombre,SUM(CASE WHEN m.fechaMovimientoCxC<? THEN m.cargoMovimientoCxC-m.abonoMovimientoCxC ELSE 0 END) Anterior,SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.cargoMovimientoCxC ELSE 0 END) Cargos,SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.abonoMovimientoCxC ELSE 0 END) Abonos,SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.cargoMovimientoCxC ELSE 0 END) CargosAcumulados,SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.abonoMovimientoCxC ELSE 0 END) AbonosAcumulados,SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC) Actual
    FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=m.Fk_FacturaEncabezado_Codigo JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo JOIN tblCliente c ON c.Pk_Cliente=cv.Fk_Cliente_Codigo
    WHERE f.Fk_Moneda_Codigo=? AND m.fechaMovimientoCxC<=?
    GROUP BY c.Pk_Cliente,c.codigoCliente,c.nombreCliente ORDER BY c.codigoCliente";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Listar: consultas parametrizadas.
        public DataTable VentasFuncListarConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.Pk_CierreMovimientoCxCEncabezado Id,c.Fk_Moneda_Codigo idMoneda,c.numeroCierreMovimientoCxCEncabezado Numero,m.codigoMoneda Moneda,c.tipoCierreMovimientoCxCEncabezado Tipo,c.fechaDesdeCierreMovimientoCxCEncabezado Desde,c.fechaHastaCierreMovimientoCxCEncabezado Hasta,c.estadoCierreMovimientoCxCEncabezado Estado,COALESCE(c.observacionesCierreMovimientoCxCEncabezado,'') Observaciones
    FROM tblCierreMovimientoCxCEncabezado c JOIN tblMoneda m ON m.Pk_Moneda=c.Fk_Moneda_Codigo
    WHERE (?=0 OR c.Fk_Moneda_Codigo=?) ORDER BY c.fechaDesdeCierreMovimientoCxCEncabezado DESC,c.Pk_CierreMovimientoCxCEncabezado DESC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Historial: consultas parametrizadas.
        public DataTable VentasFuncHistorialConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT h.Pk_CierreMovimientoCxCEncabezado idCierre,h.numeroCierreMovimientoCxCEncabezado numeroCierre,CONCAT(h.tipoCierreMovimientoCxCEncabezado,' ',DATE_FORMAT(h.fechaDesdeCierreMovimientoCxCEncabezado,'%Y-%m')) Periodo,c.codigoClienteCierreSaldoCliente Codigo,c.nombreClienteCierreSaldoCliente Nombre,c.saldoAnteriorCierreSaldoCliente Anterior,c.cargosPeriodoCierreSaldoCliente Cargos,c.abonosPeriodoCierreSaldoCliente Abonos,c.cargosAcumuladosCierreSaldoCliente CargosAcumulados,c.abonosAcumuladosCierreSaldoCliente AbonosAcumulados,c.saldoActualCierreSaldoCliente Actual,h.estadoCierreMovimientoCxCEncabezado Estado
    FROM tblCierreMovimientoCxCEncabezado h JOIN tblCierreSaldoCliente c ON c.Fk_CierreMovimientoCxCEncabezado_Codigo=h.Pk_CierreMovimientoCxCEncabezado
    WHERE h.Fk_Moneda_Codigo=? AND (?=0 OR c.Fk_Cliente_Codigo=?) AND DATE_FORMAT(h.fechaDesdeCierreMovimientoCxCEncabezado,'%Y-%m') LIKE CONCAT(?,'%')
    ORDER BY h.fechaDesdeCierreMovimientoCxCEncabezado DESC,h.Pk_CierreMovimientoCxCEncabezado,c.codigoClienteCierreSaldoCliente";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // EstadoPeriodo: consultas parametrizadas.
        public DataTable VentasFuncEstadoPeriodoConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_CierreMovimientoCxCEncabezado idCierre,numeroCierreMovimientoCxCEncabezado numeroCierre,Fk_Moneda_Codigo idMoneda,estadoCierreMovimientoCxCEncabezado estadoCierre
    FROM tblCierreMovimientoCxCEncabezado WHERE Fk_Moneda_Codigo=? AND tipoCierreMovimientoCxCEncabezado=? AND fechaDesdeCierreMovimientoCxCEncabezado=? AND fechaHastaCierreMovimientoCxCEncabezado=? AND estadoCierreMovimientoCxCEncabezado='CONFIRMADO'";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }
    }
}
