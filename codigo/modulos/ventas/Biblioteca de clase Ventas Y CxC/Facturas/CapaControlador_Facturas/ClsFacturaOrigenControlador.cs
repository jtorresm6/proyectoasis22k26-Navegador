using System;
using System.Data;
using CapaModelo_Facturas;
namespace CapaControlador_Facturas
{
    public sealed class ClsSolicitudFactura : CapaModelo_Facturas.ClsSolicitudFactura { }
    public sealed class ClsFacturaRegistrada : CapaModelo_Facturas.ClsFacturaRegistrada
    {
        internal static ClsFacturaRegistrada VentasFuncDesdeModelo(CapaModelo_Facturas.ClsFacturaRegistrada Factura)
        {
            return new ClsFacturaRegistrada { Id=Factura.Id, Uuid=Factura.Uuid, Serie=Factura.Serie, Numero=Factura.Numero,
                Moneda=Factura.Moneda, Cliente=Factura.Cliente, Identificacion=Factura.Identificacion, Direccion=Factura.Direccion,
                Empresa=Factura.Empresa, NitEmisor=Factura.NitEmisor, DireccionEmisor=Factura.DireccionEmisor,
                Condicion=Factura.Condicion, Fecha=Factura.Fecha, FechaVencimiento=Factura.FechaVencimiento, Detalle=Factura.Detalle.Copy() };
        }
    }
    public static class ClsReglasFactura
    {
        public static string VentasFuncNormalizarIdentificacion(string Valor) { return CapaModelo_Facturas.ClsReglasFactura.VentasFuncNormalizarIdentificacion(Valor); }
        public static bool VentasFuncIdentificacionValida(string Valor) { return CapaModelo_Facturas.ClsReglasFactura.VentasFuncIdentificacionValida(Valor); }
        public static DataTable VentasFuncCrearDetalle() { return CapaModelo_Facturas.ClsReglasFactura.VentasFuncCrearDetalle(); }
        public static decimal[] VentasFuncCalcular(decimal Cantidad, decimal Precio, decimal Descuento, decimal Tasa) { return CapaModelo_Facturas.ClsReglasFactura.VentasFuncCalcular(Cantidad,Precio,Descuento,Tasa); }
        public static void VentasMetRecalcular(DataTable Detalle) { CapaModelo_Facturas.ClsReglasFactura.VentasMetRecalcular(Detalle); }
    }
    public sealed class ClsFacturaOrigenControlador
    {
        private readonly ClsFacturaOrigenRepositorio _Repositorio = new ClsFacturaOrigenRepositorio();
        public DataTable VentasFuncOrigenes(bool Pedido) { return _Repositorio.VentasFuncOrigenes(Pedido); }
        public DataTable VentasFuncSucursales() { return _Repositorio.VentasFuncSucursales(); }
        public DataSet VentasFuncCargar(bool Pedido,int Id) { return _Repositorio.VentasFuncCargar(Pedido,Id); }
        public DataTable VentasFuncMonedas() { return _Repositorio.VentasFuncMonedas(); }
        public DataTable VentasFuncEmpresas() { return _Repositorio.VentasFuncEmpresas(); }
        public DataTable VentasFuncFormasPago() { return _Repositorio.VentasFuncFormasPago(); }
        public DataTable VentasFuncBuscarCliente(string Valor,bool Codigo) { return _Repositorio.VentasFuncBuscarCliente(Valor,Codigo); }
        public DataTable VentasFuncProductos(int Cliente) { return _Repositorio.VentasFuncProductos(Cliente); }
        public DataTable VentasFuncExistencias(int Producto) { return _Repositorio.VentasFuncExistencias(Producto); }
        public DataTable VentasFuncDetalleOrigen(bool Pedido,int Id) { return _Repositorio.VentasFuncPrepararDetalleOrigen(Pedido,Id); }
        public ClsFacturaRegistrada VentasFuncRegistrar(ClsSolicitudFactura Solicitud,Action<ClsFacturaRegistrada> GuardarPdf)
        {
            if (Solicitud == null) throw new ArgumentNullException(nameof(Solicitud));
            if (GuardarPdf == null) throw new ArgumentNullException(nameof(GuardarPdf));
            ClsReglasFactura.VentasMetRecalcular(Solicitud.Detalle);
            ClsFacturaRegistrada Resultado = null;
            _Repositorio.VentasFuncRegistrar(Solicitud, Factura => { Resultado=ClsFacturaRegistrada.VentasFuncDesdeModelo(Factura); GuardarPdf(Resultado); });
            return Resultado;
        }
    }
}
