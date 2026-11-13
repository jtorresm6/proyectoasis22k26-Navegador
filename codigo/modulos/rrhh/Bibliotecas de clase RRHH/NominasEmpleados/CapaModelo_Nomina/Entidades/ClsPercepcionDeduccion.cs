/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Representa un concepto de percepción o deducción de la nómina.
 *  Almacena su nombre, período, efecto sobre el pago, tipo y valor
 *  de cálculo, alcance de aplicación, estado activo y fechas
 *  de creación y actualización.
 * ==================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Entidades
{
    public class ClsPercepcionDeduccion
    {
        public int IdPercepcionDeduccion { get; set; }
        public string NombrePercepcionDeduccion { get; set; }
        public string PeriodoPercepcionDeduccion { get; set; }      // ENUM('MENSUAL','ANUAL')
        public bool EfectoPercepcionDeduccion { get; set; }
        public string TipoPercepcionDeduccion { get; set; }         // ENUM('Tabla','%','Monto')
        public decimal ValorTipoPercepcionDeduccion { get; set; }
        public string AplicacionPercepcionDeduccion { get; set; }   // ENUM('Todos','Algunos')
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
