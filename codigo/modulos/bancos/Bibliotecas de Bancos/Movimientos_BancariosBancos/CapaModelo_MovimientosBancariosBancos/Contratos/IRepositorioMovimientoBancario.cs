using CapaModelo_MovimientosBancarios.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Contratos
{
    public interface IRepositorioMovimientoBancario : IRepositorioGenerico<ClsRegistroMovimientoBancario>
    {
        long BancosMetRegistrarMovimiento(ClsRegistroMovimientoBancario Movimiento);
        DataTable BancosMetObtenerMovimientosTabla();
    }
}
