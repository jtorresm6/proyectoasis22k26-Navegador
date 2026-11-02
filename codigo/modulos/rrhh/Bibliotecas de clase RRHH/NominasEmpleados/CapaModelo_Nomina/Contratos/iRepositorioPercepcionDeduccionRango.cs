using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Contratos
{
    public interface iRepositorioPercepcionDeduccionRango : IRepositorioGenerico<ClsPercepcionDeduccionRango>
    {
        int RrhhMetContarPorDesde(int IdPercepcionDeduccion, decimal SalarioDesde, int IdExcluir);
    }
}
