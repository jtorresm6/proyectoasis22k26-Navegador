// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_Cotizaciones
{
    public sealed class ClsCotizacionesRepositorio : IDisposable
    {
        public ClsConversionCotizaciones Conversion { get; }
        public ClsConsultasCotizaciones Consultas { get; }

        public ClsCotizacionesRepositorio()
        {
            // Los grupos comparten la conexion y transaccion del repositorio.
            Conversion = new ClsConversionCotizaciones(_Datos);
            Consultas = new ClsConsultasCotizaciones(_Datos);
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

        // Guardar: consultas parametrizadas.
        public object VentasFuncGuardarConsultarCliente(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblCliente
    WHERE idCliente=?
      AND activoCliente=1";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncGuardarConsultarVendedor(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblVendedor
    WHERE idVendedor=?
      AND activoVendedor=1";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncGuardarConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT estadoCotizacionEncabezado estadoCotizacion,numeroCotizacionEncabezado numeroCotizacion,vigenteHastaCotizacionEncabezado vigenteHasta
    FROM tblCotizacionEncabezado
    WHERE idCotizacionEncabezado=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncGuardarConsultarProductoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT pr.nombreProducto,p.tipoItemProductoVenta tipoItem,p.tasaImpuestoProductoVenta*100 porcentajeImpuesto,d.precioUnitarioListaPrecioDetalle precioUnitario
    FROM tblProductoVenta p
    JOIN tblProducto pr ON pr.idProducto=p.idProductoVenta
    JOIN tblListaPrecioDetalle d ON d.idProducto=p.idProductoVenta
    WHERE p.idProductoVenta=?
      AND d.idListaPrecioEncabezado=?
      AND p.activoProductoVenta=1
      AND d.activoListaPrecioDetalle=1";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncGuardarInsertarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblCotizacionEncabezado(numeroCotizacionEncabezado,idCliente,idVendedor,idMoneda,fechaEmisionCotizacionEncabezado,vigenteHastaCotizacionEncabezado,estadoCotizacionEncabezado,idUsuarioRegistro,idListaPrecioEncabezado)
    VALUES (?,?,?,?,?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncGuardarActualizarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCotizacionEncabezado
    SET idCliente=?,idVendedor=?,idMoneda=?,fechaEmisionCotizacionEncabezado=?,vigenteHastaCotizacionEncabezado=?,estadoCotizacionEncabezado=?,idListaPrecioEncabezado=?
    WHERE idCotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncGuardarEliminarCotizaciondetalle(params object[] Parametros)
        {
            const string Consulta = @"DELETE
    FROM tblCotizacionDetalle
    WHERE idCotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncGuardarInsertarCotizaciondetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblCotizacionDetalle(idCotizacionEncabezado,numeroLineaCotizacionDetalle,idProducto,descripcionItemCotizacionDetalle,cantidadCotizacionDetalle,precioUnitarioCotizacionDetalle,descuentoMontoCotizacionDetalle,tasaImpuestoCotizacionDetalle)
    VALUES (?,?,?,?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        // Anular: consultas parametrizadas.
        public DataTable VentasFuncAnularConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT estadoCotizacionEncabezado
    FROM tblCotizacionEncabezado
    WHERE idCotizacionEncabezado=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncAnularActualizarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCotizacionEncabezado
    SET estadoCotizacionEncabezado='ANULADA'
    WHERE idCotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
