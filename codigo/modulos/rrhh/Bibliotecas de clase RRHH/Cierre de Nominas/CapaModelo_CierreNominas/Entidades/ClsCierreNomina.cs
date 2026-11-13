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
 * Propósito : Entidad de la capa Modelo para el encabezado del cierre
 * de nómina. Sus propiedades (get/set) guardan el período
 * cerrado, los totales, el número y la fecha de la póliza
 * contable devuelta por Contabilidad y el usuario que cerró.
 * ===================================================================
 */

namespace CapaModelo_CierreNominas.Entidades
{
    public class ClsCierreNomina
    {
        //IMPORTANTE TODOS ESTOS CAMPOS DEBEN SER IGUAL A COMO
        //LO TENGAN EN SU BASE DE DATOS PARA QUE HAGAN MATCH
        public int IdCierreNomina { get; set; }
        public string PeriodoCierreNomina { get; set; }
        public DateTime FechaInicioCierreNomina { get; set; }
        public DateTime FechaFinalCierreNomina { get; set; }
        public int CantidadEmpleadoCierreNomina { get; set; }

        public decimal TotalPercepcionCierreNomina { get; set; }
        public decimal TotalDeduccionCierreNomina { get; set; }
        public decimal TotalNetoCierreNomina { get; set; }
        public decimal TotalPatronalCierreNomina { get; set; }
        public decimal TotalDebeCierreNomina { get; set; }
        public decimal TotalHaberCierreNomina { get; set; }

        public string DocumentoOrigenCierreNomina { get; set; }
        public string CodigoPolizaCierreNomina { get; set; }
        public DateTime FechaPolizaCierreNomina { get; set; }
        public string EstadoPolizaCierreNomina { get; set; }

        public int IdUsuarioCierra { get; set; }
        public DateTime FechaEjecucionCierreNomina { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}