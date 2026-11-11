// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System.Data;
using System;

namespace CapaControlador_Cotizaciones
{
    public sealed class ClsCotizacionesControlador
    {
        private readonly Servicios.ClsCotizacionesServicio _Servicio = new Servicios.ClsCotizacionesServicio();
        public DataTable VentasFuncListar(string Buscar)
        {
            return _Servicio.VentasFuncListar(Buscar);
        }

        public DataTable VentasFuncCatalogo(string Nombre)
        {
            return _Servicio.VentasFuncCatalogo(Nombre);
        }

        public DataTable VentasFuncListaCliente(int Cliente, int Moneda, DateTime Fecha)
        {
            return _Servicio.VentasFuncListaCliente(Cliente, Moneda, Fecha);
        }

        public DataTable VentasFuncProductos(int Cliente, int Moneda, DateTime Fecha)
        {
            return _Servicio.VentasFuncProductos(Cliente, Moneda, Fecha);
        }

        public DataSet VentasFuncConsultar(string Numero)
        {
            return _Servicio.VentasFuncConsultar(Numero);
        }

        public string VentasFuncGuardar(int IdCotizacion, string Numero, int Cliente, int Vendedor, int Moneda, int Usuario, DateTime Fecha, DateTime Vigencia, decimal TipoCambio, string Estado, DataTable Lineas)
        {
            return _Servicio.VentasFuncGuardar(IdCotizacion, Numero, Cliente, Vendedor, Moneda, Usuario, Fecha, Vigencia, TipoCambio, Estado, Lineas);
        }

        public string VentasFuncConvertir(int IdCotizacion, bool AFactura, int Establecimiento, int Usuario)
        {
            return _Servicio.VentasFuncConvertir(IdCotizacion, AFactura, Establecimiento, Usuario);
        }

        public void VentasMetAnular(int IdCotizacion, int Usuario)
        {
            _Servicio.VentasMetAnular(IdCotizacion, Usuario);
        }

        public void VentasMetValidarCaptura(bool Condicion, string Mensaje)
        {
            VentasValidaciones.ClsValidacionesVentas.VentasMetExigir(Condicion, Mensaje);
        }

        public decimal VentasFuncImporteCelda(object Valor)
        {
            return VentasValidaciones.ClsValidacionesVentas.VentasFuncImporteCelda(Valor);
        }

        public bool VentasFuncPuedeEditar(int Id, string Estado, DateTime Vigencia)
        {
            return Validaciones.ClsReglasCotizaciones.VentasFuncPuedeEditar(Id, Estado, Vigencia);
        }

        public void VentasMetValidarEdicion(int Id, string Estado, DateTime Vigencia)
        {
            Validaciones.ClsReglasCotizaciones.VentasMetValidarEdicion(Id, Estado, Vigencia);
        }

        public object[] VentasFuncCalcularLinea(DataRow Producto, decimal Cantidad, decimal Descuento)
        {
            return Validaciones.ClsReglasCotizaciones.VentasFuncCalcularLinea(Producto, Cantidad, Descuento);
        }
    }
}
