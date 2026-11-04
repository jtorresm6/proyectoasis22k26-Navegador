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
 * Propósito : Objeto de valor con una fila de la grilla del cierre:
 * Código, Empleado, Percepciones, Deducciones y Verificación.
 * ===================================================================
 */

namespace CapaControlador_CierreNominas.Objetos_de_valor
{
    public class ClsFilaCierreNominaValor
    {
        public int IdNominaEncabezado { get; set; }
        public string Codigo { get; set; }
        public string Empleado { get; set; }
        public decimal Percepciones { get; set; }
        public decimal Deducciones { get; set; }
        public string Verificacion { get; set; }
        public bool Verificado { get; set; }
    }
}