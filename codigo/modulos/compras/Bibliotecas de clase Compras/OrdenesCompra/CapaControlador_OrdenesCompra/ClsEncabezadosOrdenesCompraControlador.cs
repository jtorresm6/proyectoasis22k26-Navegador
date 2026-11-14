using System;
using System.Data;
using CapaModelo_OrdenesCompra.Contratos;
using CapaControlador_OrdenesCompra;
using CapaControlador_OrdenesCompra.Servicios;
namespace CapaControlador_OrdenesCompra
{
    public sealed class ClsEncabezadosOrdenesCompraControlador
    {
        private readonly IOrdenesCompraRepositorio _Repositorio;

        public ClsEncabezadosOrdenesCompraControlador(IOrdenesCompraRepositorio Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public DataTable ComprasFuncListar() => _Repositorio.ComprasFuncListar();

        public DataTable ComprasFuncListar(string Estado)
        {
            if (ClsOrdenesCompraEstados.ComprasFuncEsTodos(Estado))
                return ComprasFuncListar();
            return _Repositorio.ComprasFuncListar(ClsOrdenesCompraEstados.ComprasFuncNormalizarEstado(Estado));
        }

        public void ComprasProcCambiarEstado(int Id, string Estado)
        {
            ClsOrdenesCompraValidar.ComprasProcValidarId(Id);
            _Repositorio.ComprasProcCambiarEstado(Id, ClsOrdenesCompraEstados.ComprasFuncNormalizarEstado(Estado));
        }

        public DataTable ComprasFuncFiltrar(DataTable Datos, string Palabra, string Estado, DateTime? Inicio, DateTime? Fin)
        {
            return ClsEncabezadosOrdenesCompraFiltrar.ComprasFuncFiltrar(Datos, Palabra, Estado, Inicio, Fin);
        }
    }
}
