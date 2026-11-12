/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Contratos
{
    public interface IRepositorioGenerico<Entidad> where Entidad : class
    {
        int BancosMetAgregar(Entidad Entidad);
        int BancosMetEditar(Entidad Entidad);
        int BancosMetRemover(Entidad Entidad);
        IEnumerable<Entidad> BancosMetObtenerTodos();
    }
}