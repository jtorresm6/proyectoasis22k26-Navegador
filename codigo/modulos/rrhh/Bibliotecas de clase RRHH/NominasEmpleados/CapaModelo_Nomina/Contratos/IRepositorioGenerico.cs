using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Contratos
{
    public interface IRepositorioGenerico<Entidad> where Entidad : class
    {
        int RrhhMetAgregar(Entidad Entidad);
        int RrhhMetEditar(Entidad Entidad);
        int RrhhMetRemover(Entidad Entidad);
        IEnumerable<Entidad> RrhhMetObtenerTodos();
    }
}
