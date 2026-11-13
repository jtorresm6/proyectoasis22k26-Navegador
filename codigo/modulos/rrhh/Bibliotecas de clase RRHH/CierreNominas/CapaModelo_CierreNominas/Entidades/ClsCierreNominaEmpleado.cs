using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área: Recursos Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 31/10/2026
 * ==================================================================
 * Propósito : Entidad de la capa Modelo para el histórico por empleado
 * de un cierre de nómina. Guarda los totales del empleado al
 * momento de cerrar y sus acumulados del año.
 * ===================================================================
 */

namespace CapaModelo_CierreNominas.Entidades
{
    public class ClsCierreNominaEmpleado
    {
        //IMPORTANTE TODOS ESTOS CAMPOS DEBEN SER IGUAL A COMO
        //LO TENGAN EN SU BASE DE DATOS PARA QUE HAGAN MATCH
        public int IdCierreNominaEmpleado { get; set; }
        public int IdCierreNomina { get; set; }
        public int IdNominaEncabezado { get; set; }
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }      // viene del JOIN con tblEmpleado
        public string NombreEmpleado { get; set; }      // viene del JOIN con tblEmpleado

        public decimal SalarioBaseCierreNominaEmpleado { get; set; }
        public decimal TotalPercepcionCierreNominaEmpleado { get; set; }
        public decimal TotalDeduccionCierreNominaEmpleado { get; set; }
        public decimal TotalNetoCierreNominaEmpleado { get; set; }
        public decimal AcumuladoPercepcionAnioCierreNominaEmpleado { get; set; }
        public decimal AcumuladoDeduccionAnioCierreNominaEmpleado { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}