using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área: Recursos Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 31/10/2026
 * ==================================================================
 * Propósito : Objeto de valor para el combo Mes y Año de la ventana.
 * Lleva el período abierto, sus fechas límite y el texto que
 * se muestra al usuario (por ejemplo, Octubre 2026).
 * ===================================================================
 */

namespace CapaControlador_CierreNominas.Objetos_de_valor
{
    public class ClsPeriodoNominaValor
    {
        public string PeriodoNomina { get; set; }
        public DateTime FechaInicioNomina { get; set; }
        public DateTime FechaFinalNomina { get; set; }
        public int CantidadEmpleadoNomina { get; set; }

        public string Texto
        {
            get
            {
                string Mes = FechaFinalNomina.ToString("MMMM yyyy", new CultureInfo("es-GT"));
                return char.ToUpper(Mes[0]) + Mes.Substring(1);
            }
        }
    }
}