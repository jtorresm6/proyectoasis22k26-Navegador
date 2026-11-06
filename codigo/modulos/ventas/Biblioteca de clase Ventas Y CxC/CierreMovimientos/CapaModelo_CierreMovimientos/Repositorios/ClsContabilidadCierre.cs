// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_CierreMovimientos
{
    public sealed class ClsContabilidadCierre
    {
        private readonly ClsDatosVentas _Datos;

        internal ClsContabilidadCierre(ClsDatosVentas Datos)
        {
            _Datos = Datos;
        }

        // PrepararTraslado: consultas parametrizadas.
        public DataTable VentasFuncPrepararTrasladoConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT idCierreMovimientoCxCEncabezado idCierre,idMoneda idMoneda,fechaHastaCierreMovimientoCxCEncabezado fechaHasta FROM tblCierreMovimientoCxCEncabezado WHERE idCierreMovimientoCxCEncabezado=? AND estadoCierreMovimientoCxCEncabezado='CONFIRMADO' FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncPrepararTrasladoConsultarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*) FROM tblTrasladoCxCEncabezado WHERE idCierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncPrepararTrasladoInsertarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblTrasladoCxCEncabezado(idCierreMovimientoCxCEncabezado,claveIntegracionTrasladoCxCEncabezado,idUsuario) VALUES (?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncPrepararTrasladoInsertarTrasladocxcdetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblTrasladoCxCDetalle(idTrasladoCxCEncabezado,numeroLineaTrasladoCxCDetalle,cuentaTrasladoCxCDetalle,descripcionTrasladoCxCDetalle,debeTrasladoCxCDetalle,haberTrasladoCxCDetalle)
    VALUES ((SELECT idTrasladoCxCEncabezado FROM tblTrasladoCxCEncabezado WHERE idCierreMovimientoCxCEncabezado=?),?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public DataTable VentasFuncPrepararTrasladoConsultarTrasladocxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT t.idCierreMovimientoCxCEncabezado idCierre,t.claveIntegracionTrasladoCxCEncabezado claveIntegracion,t.estadoTrasladoCxCEncabezado estado,t.referenciaContableTrasladoCxCEncabezado referenciaContable,c.idMoneda idMoneda,c.fechaHastaCierreMovimientoCxCEncabezado fechaHasta FROM tblTrasladoCxCEncabezado t JOIN tblCierreMovimientoCxCEncabezado c ON c.idCierreMovimientoCxCEncabezado=t.idCierreMovimientoCxCEncabezado WHERE t.idCierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncPrepararTrasladoConsultarTrasladocxcdetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT d.cuentaTrasladoCxCDetalle Cuenta,d.descripcionTrasladoCxCDetalle Descripcion,d.debeTrasladoCxCDetalle Debe,d.haberTrasladoCxCDetalle Haber
    FROM tblTrasladoCxCDetalle d JOIN tblTrasladoCxCEncabezado t ON t.idTrasladoCxCEncabezado=d.idTrasladoCxCEncabezado
    WHERE t.idCierreMovimientoCxCEncabezado=? ORDER BY d.numeroLineaTrasladoCxCDetalle";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Partida: consultas parametrizadas.
        public DataTable VentasFuncPartidaConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT m.tipoMovimientoCxC tipoMovimiento,SUM(m.cargoMovimientoCxC) Cargos,SUM(m.abonoMovimientoCxC) Abonos,p.cuentaClientesCuentaContableCxC cuentaClientes,p.cuentaContrapartidaCuentaContableCxC cuentaContrapartida
    FROM tblCierreMovimientoCxCEncabezado c JOIN tblFacturaEncabezado f ON f.idMoneda=c.idMoneda JOIN tblMovimientoCxC m ON m.idFacturaEncabezado=f.idFacturaEncabezado LEFT JOIN tblCuentaContableCxC p ON p.idMoneda=c.idMoneda AND p.tipoCuentaContableCxC=m.tipoMovimientoCxC
    WHERE c.idCierreMovimientoCxCEncabezado=? AND c.estadoCierreMovimientoCxCEncabezado='CONFIRMADO' AND m.fechaMovimientoCxC BETWEEN c.fechaDesdeCierreMovimientoCxCEncabezado AND c.fechaHastaCierreMovimientoCxCEncabezado
    GROUP BY m.tipoMovimientoCxC,p.cuentaClientesCuentaContableCxC,p.cuentaContrapartidaCuentaContableCxC ORDER BY m.tipoMovimientoCxC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // EstadoTraslado: consultas parametrizadas.
        public DataTable VentasFuncEstadoTrasladoConsultarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT idCierreMovimientoCxCEncabezado idCierre,claveIntegracionTrasladoCxCEncabezado claveIntegracion,estadoTrasladoCxCEncabezado estado,referenciaContableTrasladoCxCEncabezado referenciaContable FROM tblTrasladoCxCEncabezado WHERE idCierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // ConfirmarTraslado: consultas parametrizadas.
        public DataTable VentasFuncConfirmarTrasladoConsultarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT idCierreMovimientoCxCEncabezado idCierre,claveIntegracionTrasladoCxCEncabezado claveIntegracion,estadoTrasladoCxCEncabezado estado,referenciaContableTrasladoCxCEncabezado referenciaContable FROM tblTrasladoCxCEncabezado WHERE idCierreMovimientoCxCEncabezado=? FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncConfirmarTrasladoActualizarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblTrasladoCxCEncabezado SET estadoTrasladoCxCEncabezado='CONFIRMADO',referenciaContableTrasladoCxCEncabezado=?,fechaConfirmacionTrasladoCxCEncabezado=CURRENT_TIMESTAMP WHERE idCierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
