using System;
using System.Collections.Generic;
using CapaModelo_MovimientosProveedores.Entidades;
using CapaModelo_MovimientosProveedores.Contratos;
using CapaModelo_MovimientosProveedores.Repositorios;

namespace CapaControlador_MovimientosProveedores
{
    /// <summary>Proporciona a la vista las filas del balance para el reporte RDLC.</summary>
    public sealed class ClsControladorBalanceAntiguedadProveedor
    {
        private readonly IRepositorioBalanceAntiguedadProveedor _Repositorio;

        public ClsControladorBalanceAntiguedadProveedor()
            : this(new ClsRepositorioBalanceAntiguedadProveedor()) { }

        public ClsControladorBalanceAntiguedadProveedor(IRepositorioBalanceAntiguedadProveedor Repositorio)
        {
            if (Repositorio == null) throw new ArgumentNullException("Repositorio");
            _Repositorio = Repositorio;
        }

        /// <summary>
        /// Clasifica saldos actuales a la fecha indicada, separados por proveedor y moneda.
        /// No reconstruye saldos historicos. null en IdProveedor consulta todos los proveedores.
        /// </summary>
        public List<ClsBalanceAntiguedadProveedor> ComprasMetConsultarBalanceAntiguedad(
            DateTime FechaCorte, int? IdProveedor = null)
        {
            return _Repositorio.ComprasMetConsultarBalanceAntiguedad(FechaCorte, IdProveedor);
        }
    }
}
