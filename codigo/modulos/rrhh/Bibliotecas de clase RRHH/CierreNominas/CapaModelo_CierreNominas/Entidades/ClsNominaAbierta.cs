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
 * Propósito : Entidad de lectura de la capa Modelo. Representa un
 * período de nómina que todavía está abierto (aprobado y sin
 * cierre), para llenar el combo Mes y Año de la ventana.
 * ===================================================================
 */

namespace CapaModelo_CierreNominas.Entidades
{
    public class ClsNominaAbierta
    {
        public string PeriodoNomina { get; set; }
        public DateTime FechaInicioNomina { get; set; }
        public DateTime FechaFinalNomina { get; set; }
        public int CantidadEmpleadoNomina { get; set; }
    }
}