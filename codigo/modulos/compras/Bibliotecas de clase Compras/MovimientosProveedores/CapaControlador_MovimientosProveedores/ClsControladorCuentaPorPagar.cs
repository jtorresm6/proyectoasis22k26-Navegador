using System;
using System.Data;
using CapaModelo_MovimientosProveedores.Contratos;
using CapaModelo_MovimientosProveedores.Repositorios;

namespace CapaControlador_MovimientosProveedores
{
    public sealed class ClsControladorCuentaPorPagar
    {
        private readonly IRepositorioCuentaPorPagar _Repositorio;

        public ClsControladorCuentaPorPagar() : this(new ClsRepositorioCuentaPorPagar()) { }

        public ClsControladorCuentaPorPagar(IRepositorioCuentaPorPagar Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public DataTable ComprasMetConsultar(int? IdProveedor = null)
        {
            if (IdProveedor.HasValue && IdProveedor.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(IdProveedor), "Seleccione un proveedor válido.");
            return _Repositorio.ComprasMetConsultar(IdProveedor);
        }
    }
}
