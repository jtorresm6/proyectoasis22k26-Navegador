using System;
using System.Data;
using CapaModelo_Facturas;
namespace CapaControlador_Facturas
{
    public sealed class ClsFacturaOrigenControlador
    {
        private readonly ClsFacturaOrigenRepositorio repositorio = new ClsFacturaOrigenRepositorio();
        public DataTable VentasFuncOrigenes(bool pedido) { return repositorio.VentasFuncOrigenes(pedido); }
        public DataTable VentasFuncSucursales() { return repositorio.VentasFuncSucursales(); }
        public DataSet VentasFuncCargar(bool pedido,int id) { return repositorio.VentasFuncCargar(pedido,id); }
        public DataTable Monedas() { return repositorio.Monedas(); }
        public DataTable Empresas() { return repositorio.Empresas(); }
        public DataTable FormasPago() { return repositorio.FormasPago(); }
        public DataTable BuscarCliente(string valor,bool codigo) { return repositorio.BuscarCliente(valor,codigo); }
        public DataTable Productos(int cliente) { return repositorio.Productos(cliente); }
        public DataTable Existencias(int producto) { return repositorio.Existencias(producto); }
        public DataTable DetalleOrigen(bool pedido,int id) { return repositorio.PrepararDetalleOrigen(pedido,id); }
        public FacturaRegistrada Registrar(SolicitudFactura solicitud,Action<FacturaRegistrada> guardarPdf) { return repositorio.Registrar(solicitud,guardarPdf); }
    }
}
