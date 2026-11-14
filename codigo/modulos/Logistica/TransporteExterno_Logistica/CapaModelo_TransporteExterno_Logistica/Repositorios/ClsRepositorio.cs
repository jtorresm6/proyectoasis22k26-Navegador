using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_TransporteExterno_Logistica.Repositorio
{
    public abstract class ClsRepositorio
    {
        private readonly string _CadenaConexion;

        protected ClsRepositorio()
        {
            _CadenaConexion = "Dsn=dbsistemaembutidos;";
        }

        protected OdbcConnection LogisticaMetObtenerConexion()
        {
            return new OdbcConnection(_CadenaConexion);
        }
    }
}
