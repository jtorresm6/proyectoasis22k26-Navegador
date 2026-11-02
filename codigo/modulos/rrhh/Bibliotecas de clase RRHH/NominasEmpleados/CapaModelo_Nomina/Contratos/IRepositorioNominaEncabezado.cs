using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Contratos
{
    public interface IRepositorioNominaEncabezado : IRepositorioGenerico<ClsNominaEncabezado>
    {
        bool RrhhMetEsEmpleadoActivo(int IdEmpleado);

        decimal? RrhhMetObtenerSalarioEmpleado(int IdEmpleado);

        void RrhhMetObtenerResumenHorasExtra(int IdEmpleado, DateTime FechaInicio, DateTime FechaFinal,
            out int Duracion, out decimal TotalPago);

        List<ClsPercepcionDeduccion> RrhhMetObtenerConceptosAplicables(int IdEmpleado);

        bool RrhhMetObtenerRangoSalario(int IdPercepcionDeduccion, decimal Salario,
            out string Modo, out decimal Valor, out decimal MontoFijoBase);

        int RrhhMetContarTraslapes(int IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, int IdExcluir);
    }
}
