using System;
using System.Collections.Generic;
using CapaModelo_MovimientosProveedores.Entidades;

namespace CapaModelo_MovimientosProveedores.Contratos
{
    public interface IRepositorioBalanceAntiguedadProveedor
    {
        List<ClsBalanceAntiguedadProveedor> ComprasMetConsultarBalanceAntiguedad(DateTime FechaCorte, int? IdProveedor = null);
    }
}
