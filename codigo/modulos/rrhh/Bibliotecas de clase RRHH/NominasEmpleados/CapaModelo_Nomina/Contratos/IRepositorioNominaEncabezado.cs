/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Define las operaciones de acceso a datos del encabezado de
 *  nómina. Amplía el contrato genérico con consultas de empleado
 *  activo, salario, horas extra, conceptos aplicables, rangos
 *  salariales y períodos de nómina que se traslapan.
 * ==================================================================
 */

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
