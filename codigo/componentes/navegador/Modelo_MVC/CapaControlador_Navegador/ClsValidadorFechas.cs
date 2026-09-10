using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Controlador_Navegador
{
    public class ClsValidadorFechas
    {
        public string NavegadorMetValidarFechas(
            DateTime FechaNacimiento,
            DateTime FechaContratacion)
        {
            DateTime FechaActual = DateTime.Today;

            if (FechaNacimiento > FechaActual)
            {
                return "La fecha de nacimiento no puede ser futura.";
            }

            if (FechaContratacion > FechaActual)
            {
                return "La fecha de contratación no puede ser futura.";
            }

            if (FechaNacimiento >= FechaContratacion)
            {
                return "La fecha de nacimiento debe ser anterior a la fecha de contratación.";
            }

            return null;
        }
    }
}
