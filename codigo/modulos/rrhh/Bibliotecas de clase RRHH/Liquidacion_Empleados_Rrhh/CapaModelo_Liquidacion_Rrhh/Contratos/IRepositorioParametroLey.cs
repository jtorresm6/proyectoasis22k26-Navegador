using CapaModelo_Liquidacion_Rrhh.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Liquidacion_Rrhh.Contratos
{
    public interface IRepositorioParametroLey
    {
        IEnumerable<ClsParametroLey> RrhhMetObtenerTodos();
    }
}