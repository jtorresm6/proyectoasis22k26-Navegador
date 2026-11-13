using CapaModelo_Nomina.Contratos;
using CapaModelo_Nomina.Entidades;
using CapaModelo_Nomina.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.Modelos_de_controladores
{
    public class ClsModeloReporteNomina
    {
        private readonly IRepositorioReporteNomina
            _RepositorioReporteNomina;

        public ClsModeloReporteNomina()
        {
            _RepositorioReporteNomina =
                new ClsRepositorioReporteNomina();
        }

        public List<ClsReporteNomina> RrhhMetObtenerPorPeriodo(
            DateTime FechaInicio,
            DateTime FechaFinal)
        {
            if (FechaFinal.Date < FechaInicio.Date)
            {
                throw new ArgumentException(
                    "La fecha final no puede ser menor " +
                    "a la fecha inicial.");
            }

            return _RepositorioReporteNomina
                .RrhhMetObtenerPorPeriodo(
                    FechaInicio.Date,
                    FechaFinal.Date);
        }

        
    }
}
