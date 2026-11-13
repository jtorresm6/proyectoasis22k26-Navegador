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
 * Propósito : Objeto de valor con el resumen del período que muestra
 * la ventana (empleados, percepciones, deducciones, salario
 * líquido y verificación de cuadre), las filas de la grilla y
 * si el cierre se puede ejecutar o por qué está bloqueado.
 * ===================================================================
 */

namespace CapaControlador_CierreNominas.Objetos_de_valor
{
    public class ClsResumenCierreNominaValor
    {
        public int EmpleadosEnNomina { get; set; }
        public decimal TotalPercepciones { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal SalarioLiquido { get; set; }
        public decimal TotalDebe { get; set; }
        public decimal TotalHaber { get; set; }

        public bool CuadreCorrecto { get; set; }
        public string TextoCuadre { get; set; }
        public bool PuedeCerrar { get; set; }
        public string MensajeBloqueo { get; set; }

        public List<ClsFilaCierreNominaValor> Filas { get; set; }

        public ClsResumenCierreNominaValor()
        {
            Filas = new List<ClsFilaCierreNominaValor>();
        }
    }
}