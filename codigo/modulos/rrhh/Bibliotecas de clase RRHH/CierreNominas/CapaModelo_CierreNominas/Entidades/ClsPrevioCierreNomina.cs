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
 * Propósito : Entidad de lectura de la capa Modelo. Una fila por
 * empleado con los totales de su nómina abierta, los totales
 * sumados desde el detalle y cuántos conceptos no tienen
 * cuenta contable. Con estos datos el controlador calcula la
 * verificación de cuadre antes de cerrar.
 * ===================================================================
 */

namespace CapaModelo_CierreNominas.Entidades
{
    public class ClsPrevioCierreNomina
    {
        public int IdNominaEncabezado { get; set; }
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public decimal SalarioBaseNominaEncabezado { get; set; }
        public decimal TotalPercepcionNominaEncabezado { get; set; }
        public decimal TotalDeduccionNominaEncabezado { get; set; }
        public decimal TotalNetoNominaEncabezado { get; set; }
        public decimal DetallePercepcion { get; set; }
        public decimal DetalleDeduccion { get; set; }
        public int ConceptosSinCuenta { get; set; }
    }
}