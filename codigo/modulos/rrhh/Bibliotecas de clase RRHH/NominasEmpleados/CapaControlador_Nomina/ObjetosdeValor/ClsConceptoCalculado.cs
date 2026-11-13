/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Transporta el resultado del cálculo de una percepción o
 *  deducción, incluyendo identificador, nombre, efecto y monto.
 *  Define el nombre del concepto de comisión por ventas utilizado
 *  para integrar las comisiones al cálculo y detalle de nómina.
 * ==================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.ObjetosdeValor
{
    public class ClsConceptoCalculado
    {

        public const string NombreConceptoComision = "Comisión por Ventas";

        public int IdPercepcionDeduccion { get; set; }
        public string NombreConcepto { get; set; }
        public bool EsPercepcion { get; set; }
        public decimal Monto { get; set; }
    }
}
