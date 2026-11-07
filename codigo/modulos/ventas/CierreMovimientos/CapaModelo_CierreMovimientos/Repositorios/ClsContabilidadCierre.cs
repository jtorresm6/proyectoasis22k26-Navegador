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
            const string Consulta = @"SELECT Pk_CierreMovimientoCxCEncabezado idCierre,Fk_Moneda_Codigo idMoneda,fechaHastaCierreMovimientoCxCEncabezado fechaHasta FROM tblCierreMovimientoCxCEncabezado WHERE Pk_CierreMovimientoCxCEncabezado=? AND estadoCierreMovimientoCxCEncabezado='CONFIRMADO' FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncPrepararTrasladoConsultarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*) FROM tblTrasladoCxCEncabezado WHERE Fk_CierreMovimientoCxCEncabezado_Codigo=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncPrepararTrasladoInsertarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblTrasladoCxCEncabezado(Fk_CierreMovimientoCxCEncabezado_Codigo,claveIntegracionTrasladoCxCEncabezado,Fk_Usuario_Codigo) VALUES (?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncPrepararTrasladoInsertarTrasladocxcdetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblTrasladoCxCDetalle(Fk_TrasladoCxCEncabezado_Codigo,numeroLineaTrasladoCxCDetalle,cuentaTrasladoCxCDetalle,descripcionTrasladoCxCDetalle,debeTrasladoCxCDetalle,haberTrasladoCxCDetalle) VALUES (?,?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public DataTable VentasFuncPrepararTrasladoConsultarTrasladocxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT t.Fk_CierreMovimientoCxCEncabezado_Codigo idCierre,t.claveIntegracionTrasladoCxCEncabezado claveIntegracion,t.estadoTrasladoCxCEncabezado estado,t.referenciaContableTrasladoCxCEncabezado referenciaContable,c.Fk_Moneda_Codigo idMoneda,c.fechaHastaCierreMovimientoCxCEncabezado fechaHasta FROM tblTrasladoCxCEncabezado t JOIN tblCierreMovimientoCxCEncabezado c ON c.Pk_CierreMovimientoCxCEncabezado=t.Fk_CierreMovimientoCxCEncabezado_Codigo WHERE t.Fk_CierreMovimientoCxCEncabezado_Codigo=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncPrepararTrasladoConsultarTrasladocxcdetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT cuentaTrasladoCxCDetalle Cuenta,descripcionTrasladoCxCDetalle Descripcion,debeTrasladoCxCDetalle Debe,haberTrasladoCxCDetalle Haber FROM tblTrasladoCxCDetalle WHERE Fk_TrasladoCxCEncabezado_Codigo=? ORDER BY numeroLineaTrasladoCxCDetalle";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Partida: consultas parametrizadas.
        public DataTable VentasFuncPartidaConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT m.tipoMovimientoCxC tipoMovimiento,SUM(m.cargoMovimientoCxC) Cargos,SUM(m.abonoMovimientoCxC) Abonos,p.cuentaClientesCuentaContableCxC cuentaClientes,p.cuentaContrapartidaCuentaContableCxC cuentaContrapartida
    FROM tblCierreMovimientoCxCEncabezado c JOIN tblFacturaEncabezado f ON f.Fk_Moneda_Codigo=c.Fk_Moneda_Codigo JOIN tblMovimientoCxC m ON m.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado LEFT JOIN tblCuentaContableCxC p ON p.Fk_Moneda_Codigo=c.Fk_Moneda_Codigo AND p.tipoCuentaContableCxC=m.tipoMovimientoCxC
    WHERE c.Pk_CierreMovimientoCxCEncabezado=? AND c.estadoCierreMovimientoCxCEncabezado='CONFIRMADO' AND m.fechaMovimientoCxC BETWEEN c.fechaDesdeCierreMovimientoCxCEncabezado AND c.fechaHastaCierreMovimientoCxCEncabezado
    GROUP BY m.tipoMovimientoCxC,p.cuentaClientesCuentaContableCxC,p.cuentaContrapartidaCuentaContableCxC ORDER BY m.tipoMovimientoCxC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // EstadoTraslado: consultas parametrizadas.
        public DataTable VentasFuncEstadoTrasladoConsultarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT Fk_CierreMovimientoCxCEncabezado_Codigo idCierre,claveIntegracionTrasladoCxCEncabezado claveIntegracion,estadoTrasladoCxCEncabezado estado,referenciaContableTrasladoCxCEncabezado referenciaContable FROM tblTrasladoCxCEncabezado WHERE Fk_CierreMovimientoCxCEncabezado_Codigo=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // ConfirmarTraslado: consultas parametrizadas.
        public DataTable VentasFuncConfirmarTrasladoConsultarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT Fk_CierreMovimientoCxCEncabezado_Codigo idCierre,claveIntegracionTrasladoCxCEncabezado claveIntegracion,estadoTrasladoCxCEncabezado estado,referenciaContableTrasladoCxCEncabezado referenciaContable FROM tblTrasladoCxCEncabezado WHERE Fk_CierreMovimientoCxCEncabezado_Codigo=? FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncConfirmarTrasladoActualizarTrasladocxc(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblTrasladoCxCEncabezado SET estadoTrasladoCxCEncabezado='CONFIRMADO',referenciaContableTrasladoCxCEncabezado=?,fechaConfirmacionTrasladoCxCEncabezado=CURRENT_TIMESTAMP WHERE Fk_CierreMovimientoCxCEncabezado_Codigo=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
