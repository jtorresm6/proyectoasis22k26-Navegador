// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_CierreMovimientos
{
    public sealed class ClsCierreRepositorio : IDisposable
    {
        public ClsContabilidadCierre Contabilidad { get; }
        public ClsConsultasCierre Consultas { get; }

        public ClsCierreRepositorio()
        {
            // Los grupos comparten la conexion y transaccion del repositorio.
            Contabilidad = new ClsContabilidadCierre(_Datos);
            Consultas = new ClsConsultasCierre(_Datos);
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

        // GuardarBorrador: consultas parametrizadas.
        public DataTable VentasFuncGuardarBorradorConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT Fk_Moneda_Codigo idMoneda,estadoCierreMovimientoCxCEncabezado estadoCierre FROM tblCierreMovimientoCxCEncabezado WHERE Pk_CierreMovimientoCxCEncabezado=? FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncGuardarBorradorConsultarCierremovimientos2(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*) FROM tblCierreMovimientoCxCEncabezado WHERE Fk_Moneda_Codigo=? AND tipoCierreMovimientoCxCEncabezado=? AND fechaDesdeCierreMovimientoCxCEncabezado=? AND fechaHastaCierreMovimientoCxCEncabezado=? AND estadoCierreMovimientoCxCEncabezado IN ('BORRADOR','CONFIRMADO') AND Pk_CierreMovimientoCxCEncabezado<>?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncGuardarBorradorInsertarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblCierreMovimientoCxCEncabezado(numeroCierreMovimientoCxCEncabezado,fechaDesdeCierreMovimientoCxCEncabezado,fechaHastaCierreMovimientoCxCEncabezado,fechaCorteCierreMovimientoCxCEncabezado,Fk_Moneda_Codigo,tipoCierreMovimientoCxCEncabezado,estadoCierreMovimientoCxCEncabezado,observacionesCierreMovimientoCxCEncabezado,Fk_Usuario_Responsable) VALUES (?,?,?,?,?,?,'BORRADOR',?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public object VentasFuncGuardarBorradorConsultarCierremovimientos3(params object[] Parametros)
        {
            const string Consulta = @"SELECT numeroCierreMovimientoCxCEncabezado FROM tblCierreMovimientoCxCEncabezado WHERE Pk_CierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncGuardarBorradorActualizarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCierreMovimientoCxCEncabezado SET tipoCierreMovimientoCxCEncabezado=?,fechaDesdeCierreMovimientoCxCEncabezado=?,fechaHastaCierreMovimientoCxCEncabezado=?,fechaCorteCierreMovimientoCxCEncabezado=?,observacionesCierreMovimientoCxCEncabezado=? WHERE Pk_CierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        // Confirmar: consultas parametrizadas.
        public object VentasFuncConfirmarConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*) FROM tblCierreMovimientoCxCEncabezado WHERE Fk_Moneda_Codigo=? AND tipoCierreMovimientoCxCEncabezado=? AND fechaDesdeCierreMovimientoCxCEncabezado=? AND fechaHastaCierreMovimientoCxCEncabezado=? AND estadoCierreMovimientoCxCEncabezado='CONFIRMADO'";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncConfirmarConsultarCierremovimientos2(params object[] Parametros)
        {
            const string Consulta = @"SELECT MIN(fechaDesdeCierreMovimientoCxCEncabezado) FROM tblCierreMovimientoCxCEncabezado WHERE Fk_Moneda_Codigo=? AND tipoCierreMovimientoCxCEncabezado='MENSUAL' AND estadoCierreMovimientoCxCEncabezado='CONFIRMADO'";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncConfirmarConsultarCierremovimientos3(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(DISTINCT MONTH(fechaDesdeCierreMovimientoCxCEncabezado)) FROM tblCierreMovimientoCxCEncabezado WHERE Fk_Moneda_Codigo=? AND tipoCierreMovimientoCxCEncabezado='MENSUAL' AND estadoCierreMovimientoCxCEncabezado='CONFIRMADO' AND YEAR(fechaDesdeCierreMovimientoCxCEncabezado)=? AND MONTH(fechaDesdeCierreMovimientoCxCEncabezado)>=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncConfirmarConsultarControlperiodocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT fechaUltimoCierreControlPeriodoCxC FROM tblControlPeriodoCxC WHERE Fk_Moneda_Codigo=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncConfirmarConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT MIN(m.fechaMovimientoCxC) FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=m.Fk_FacturaEncabezado_Codigo WHERE f.Fk_Moneda_Codigo=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncConfirmarConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*) FROM tblFacturaEncabezado WHERE Fk_Moneda_Codigo=? AND fechaEmisionFacturaEncabezado BETWEEN ? AND ? AND estadoFacturaEncabezado='BORRADOR'";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncConfirmarConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*) FROM tblReciboPago WHERE Fk_Moneda_Codigo=? AND fechaReciboPago BETWEEN ? AND ? AND estadoReciboPago='BORRADOR'";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncConfirmarConsultarCierremovimientos4(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_CierreMovimientoCxCEncabezado idCierre,numeroCierreMovimientoCxCEncabezado numeroCierre FROM tblCierreMovimientoCxCEncabezado WHERE Fk_Moneda_Codigo=? AND tipoCierreMovimientoCxCEncabezado=? AND fechaDesdeCierreMovimientoCxCEncabezado=? AND fechaHastaCierreMovimientoCxCEncabezado=? AND estadoCierreMovimientoCxCEncabezado='BORRADOR' FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncConfirmarActualizarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCierreMovimientoCxCEncabezado SET documentosRevisadosCierreMovimientoCxCEncabezado=1,Fk_Usuario_Responsable=? WHERE Pk_CierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConfirmarInsertarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblCierreMovimientoCxCEncabezado(numeroCierreMovimientoCxCEncabezado,fechaDesdeCierreMovimientoCxCEncabezado,fechaHastaCierreMovimientoCxCEncabezado,fechaCorteCierreMovimientoCxCEncabezado,Fk_Moneda_Codigo,tipoCierreMovimientoCxCEncabezado,estadoCierreMovimientoCxCEncabezado,documentosRevisadosCierreMovimientoCxCEncabezado,pagosConciliadosCierreMovimientoCxCEncabezado,Fk_Usuario_Responsable) VALUES (?,?,?,?,?,?,'BORRADOR',1,0,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConfirmarInsertarCierresaldocliente(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblCierreSaldoCliente(Fk_CierreMovimientoCxCEncabezado_Codigo,Fk_Cliente_Codigo,codigoClienteCierreSaldoCliente,nombreClienteCierreSaldoCliente,saldoAnteriorCierreSaldoCliente,cargosPeriodoCierreSaldoCliente,abonosPeriodoCierreSaldoCliente,cargosAcumuladosCierreSaldoCliente,abonosAcumuladosCierreSaldoCliente,saldoActualCierreSaldoCliente) VALUES (?,?,?,?,?,?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConfirmarInsertarCierremovimientosdetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblCierreMovimientoCxCDetalle(Fk_CierreMovimientoCxCEncabezado_Codigo,Fk_FacturaEncabezado_Codigo,saldoInicialCierreMovimientoCxCDetalle,cargosPeriodoCierreMovimientoCxCDetalle,abonosPeriodoCierreMovimientoCxCDetalle,saldoFinalCierreMovimientoCxCDetalle) SELECT ?,m.Fk_FacturaEncabezado_Codigo,SUM(CASE WHEN m.fechaMovimientoCxC<? THEN m.cargoMovimientoCxC-m.abonoMovimientoCxC ELSE 0 END),SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.cargoMovimientoCxC ELSE 0 END),SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.abonoMovimientoCxC ELSE 0 END),SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC)
    FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=m.Fk_FacturaEncabezado_Codigo WHERE f.Fk_Moneda_Codigo=? AND m.fechaMovimientoCxC<=? GROUP BY m.Fk_FacturaEncabezado_Codigo";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConfirmarActualizarCierremovimientos2(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCierreMovimientoCxCEncabezado SET estadoCierreMovimientoCxCEncabezado='CONFIRMADO',fechaConfirmacionCierreMovimientoCxCEncabezado=CURRENT_TIMESTAMP WHERE Pk_CierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConfirmarActualizarControlperiodocxc(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblControlPeriodoCxC SET fechaUltimoCierreControlPeriodoCxC=? WHERE Fk_Moneda_Codigo=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        // Generales: consultas parametrizadas.
        public DataTable VentasFuncGeneralesEliminarBorradorConsultarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"SELECT Fk_Moneda_Codigo idMoneda FROM tblCierreMovimientoCxCEncabezado WHERE Pk_CierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncGeneralesEliminarBorradorConsultarCierremovimientos2(params object[] Parametros)
        {
            const string Consulta = @"SELECT estadoCierreMovimientoCxCEncabezado estadoCierre FROM tblCierreMovimientoCxCEncabezado WHERE Pk_CierreMovimientoCxCEncabezado=? FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncGeneralesEliminarBorradorActualizarCierremovimientos(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCierreMovimientoCxCEncabezado SET estadoCierreMovimientoCxCEncabezado='ANULADO',observacionesCierreMovimientoCxCEncabezado=? WHERE Pk_CierreMovimientoCxCEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
