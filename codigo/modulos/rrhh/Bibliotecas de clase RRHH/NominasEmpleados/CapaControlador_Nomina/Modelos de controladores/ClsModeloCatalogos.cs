using CapaModelo_Nomina.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.Modelos_de_controladores
{
    public class ClsModeloCatalogos
    {
        private ClsRepositorioCatalogos _RepositorioCatalogos;

        public ClsModeloCatalogos()
        {
            _RepositorioCatalogos = new ClsRepositorioCatalogos();
        }

        public List<KeyValuePair<int, string>> RrhhMetObtenerEmpleados()
        {
            return _RepositorioCatalogos.RrhhMetObtenerEmpleados();
        }

        public List<KeyValuePair<int, string>> RrhhMetObtenerEmpleadosActivos()
        {
            return _RepositorioCatalogos.RrhhMetObtenerEmpleadosActivos();
        }

        public string RrhhMetObtenerDepartamentoEmpleado(int IdEmpleado)
        {
            return _RepositorioCatalogos.RrhhMetObtenerDepartamentoEmpleado(IdEmpleado);
        }

        public List<KeyValuePair<int, string>> RrhhMetObtenerDepartamentos()
        {
            return _RepositorioCatalogos.RrhhMetObtenerDepartamentos();
        }

    }
}
