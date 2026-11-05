using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Inventario_Logistica.Repositorios
{
    public abstract class ClsRepositorio
    {
        private readonly string _CadenaConexion;

        protected ClsRepositorio()
        {
            _CadenaConexion = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection LogisticaMetObtenerConexion()
        {
            return new OdbcConnection(_CadenaConexion);
        }
    }
}
