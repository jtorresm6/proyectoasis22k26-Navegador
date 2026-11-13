using CapaModelo_Nomina.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.ObjetosdeValor
{
    public class ClsModeloComisiones
    {
        private ClsRepositorioComisiones _RepositorioComisiones;

        public ClsModeloComisiones()
        {
            _RepositorioComisiones = new ClsRepositorioComisiones();
        }

        // Total de comisiones aprobadas del empleado que caen dentro del periodo de la nomina
        public decimal RrhhMetObtenerTotalComisiones(int IdEmpleado, DateTime FechaInicio, DateTime FechaFinal)
        {
            return _RepositorioComisiones.RrhhMetObtenerTotalComisiones(IdEmpleado, FechaInicio, FechaFinal);
        }
    }
}
