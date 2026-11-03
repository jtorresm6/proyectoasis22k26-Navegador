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

        // Guardar: consultas parametrizadas.
        public object VentasFuncGuardarConsultarCliente(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblCliente
    WHERE Pk_Cliente=?
      AND activoCliente=1";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncGuardarConsultarVendedor(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblVendedor
    WHERE Pk_Vendedor=?
      AND activoVendedor=1";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncGuardarConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT estadoCotizacionEncabezado estadoCotizacion,numeroCotizacionEncabezado numeroCotizacion,vigenteHastaCotizacionEncabezado vigenteHasta
    FROM tblCotizacionEncabezado
    WHERE Pk_CotizacionEncabezado=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncGuardarConsultarProductoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT pr.nombreProducto,p.tipoItemProductoVenta tipoItem,p.tasaImpuestoProductoVenta*100 porcentajeImpuesto,d.precioUnitarioListaPrecioDetalle precioUnitario
    FROM tblProductoVenta p
    JOIN tblProducto pr ON pr.Pk_Producto=p.Pk_ProductoVenta
    JOIN tblListaPrecioDetalle d ON d.Fk_Producto_Codigo=p.Pk_ProductoVenta
    WHERE p.Pk_ProductoVenta=?
      AND d.Fk_ListaPrecioEncabezado_Codigo=?
      AND p.activoProductoVenta=1
      AND d.activoListaPrecioDetalle=1";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncGuardarInsertarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblCotizacionEncabezado(numeroCotizacionEncabezado,Fk_Cliente_Codigo,Fk_Vendedor_Codigo,Fk_Moneda_Codigo,fechaEmisionCotizacionEncabezado,vigenteHastaCotizacionEncabezado,estadoCotizacionEncabezado,Fk_Usuario_Registro,Fk_ListaPrecioEncabezado_Codigo)
    VALUES (?,?,?,?,?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncGuardarActualizarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCotizacionEncabezado
    SET Fk_Cliente_Codigo=?,Fk_Vendedor_Codigo=?,Fk_Moneda_Codigo=?,fechaEmisionCotizacionEncabezado=?,vigenteHastaCotizacionEncabezado=?,estadoCotizacionEncabezado=?,Fk_ListaPrecioEncabezado_Codigo=?
    WHERE Pk_CotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncGuardarEliminarCotizaciondetalle(params object[] Parametros)
        {
            const string Consulta = @"DELETE
    FROM tblCotizacionDetalle
    WHERE Fk_CotizacionEncabezado_Codigo=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncGuardarInsertarCotizaciondetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblCotizacionDetalle(Fk_CotizacionEncabezado_Codigo,numeroLineaCotizacionDetalle,Fk_Producto_Codigo,descripcionItemCotizacionDetalle,cantidadCotizacionDetalle,precioUnitarioCotizacionDetalle,descuentoMontoCotizacionDetalle,tasaImpuestoCotizacionDetalle)
    VALUES (?,?,?,?,?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        // Anular: consultas parametrizadas.
        public DataTable VentasFuncAnularConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT estadoCotizacionEncabezado
    FROM tblCotizacionEncabezado
    WHERE Pk_CotizacionEncabezado=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncAnularActualizarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCotizacionEncabezado
    SET estadoCotizacionEncabezado='ANULADA'
    WHERE Pk_CotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
