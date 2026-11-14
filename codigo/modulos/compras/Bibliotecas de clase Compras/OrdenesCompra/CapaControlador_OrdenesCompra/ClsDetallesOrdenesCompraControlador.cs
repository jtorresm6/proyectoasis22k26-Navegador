using System;
using System.Data;
using CapaModelo_OrdenesCompra.Contratos;
using CapaControlador_OrdenesCompra;
using CapaControlador_OrdenesCompra.Servicios;
namespace CapaControlador_OrdenesCompra
{
    public sealed class ClsDetallesOrdenesCompraControlador
    {
        private readonly IOrdenesCompraRepositorio _Repositorio;

        public ClsDetallesOrdenesCompraControlador(IOrdenesCompraRepositorio Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public DataTable ComprasFuncListarDetalles(int Id)
        {
            ClsOrdenesCompraValidar.ComprasProcValidarId(Id);
            return _Repositorio.ComprasFuncListarDetalles(Id);
        }

        public ClsOrdenesCompraDatos ComprasFuncAnular(int Id)
        {
            ClsOrdenesCompraValidar.ComprasProcValidarId(Id);
            _Repositorio.ComprasProcAnular(Id);
            var Orden = ComprasFuncObtener(Id);
            if (Orden == null || Orden.Estado != "Anulada")
                throw new InvalidOperationException("No se pudo verificar la anulación. Actualiza la lista.");
            return Orden;
        }

        public void ComprasProcActualizarVencimiento(int Id, DateTime Fecha)
        {
            var Orden = ComprasFuncObtener(Id);
            ClsOrdenesCompraValidar.ComprasProcValidarVencimiento(Orden, Fecha);
            _Repositorio.ComprasProcActualizarVencimiento(Id, Fecha.Date);
        }

        public ClsOrdenesCompraDatos ComprasFuncObtener(int Id)
        {
            ClsOrdenesCompraValidar.ComprasProcValidarId(Id);
            var Orden = _Repositorio.ComprasFuncObtener(Id);
            return Orden == null ? null : new ClsOrdenesCompraDatos(Orden);
        }
    }
}
