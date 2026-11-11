using System;
using System.Data;
using CapaModelo_Facturas;

namespace CapaControlador_Facturas
{
    public sealed class ClsFacturaOrigenControlador
    {
        private readonly ClsFacturaOrigenRepositorio _Repositorio = new ClsFacturaOrigenRepositorio();

        public DataTable VentasFuncOrigenes(bool Pedido)
        {
            return _Repositorio.VentasFuncOrigenes(Pedido);
        }

        public DataTable VentasFuncSucursales()
        {
            return _Repositorio.VentasFuncSucursales();
        }

        public DataSet VentasFuncCargar(bool Pedido, int Id)
        {
            return _Repositorio.VentasFuncCargar(Pedido, Id);
        }

        public int VentasFuncRegistrar(bool Pedido, int Id, int Sucursal, int Usuario, string Condicion, out DataSet Documento)
        {
            if (Id <= 0) throw new ArgumentException("Cargue primero una cotización o un pedido.");
            if (Condicion != "CONTADO" && Condicion != "CREDITO")
                throw new ArgumentException("Seleccione una condición de pago.");
            return _Repositorio.VentasFuncRegistrar(Pedido, Id, Sucursal, Usuario, Condicion, out Documento);
        }
    }
}
