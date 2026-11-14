using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Liquidacion_Rrhh.Contratos
{
    public interface IRepositorioGenerico<Entidad> where Entidad : class
    {
        int LiquidacionMetAgregar(Entidad Entidad);
        int LiquidacionMetEditar(Entidad Entidad);
        int LiquidacionMetRemover(Entidad Entidad);
        IEnumerable<Entidad> LiquidacionMetObtenerTodos();
    }
}
