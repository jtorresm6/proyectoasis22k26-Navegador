/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Proporciona al cálculo de nómina el total de comisiones
 *  aprobadas de un empleado dentro de las fechas indicadas,
 *  delegando la consulta de los datos al repositorio
 *  de comisiones.
 * ==================================================================
 */

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
