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
 * Propósito : Entidad de lectura de la capa Modelo. Monto total de
 * una cuenta contable sumado entre todos los empleados del
 * período, indicando si viene de percepciones o deducciones.
 * El controlador la convierte en líneas Debe/Haber de la póliza.
 * ===================================================================
 */

namespace CapaModelo_CierreNominas.Entidades
{
    public class ClsLineaPolizaNomina
    {
        public int IdCuenta { get; set; }
        public string CodigoCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public bool EsPercepcion { get; set; }
        public decimal MontoTotal { get; set; }
    }
}