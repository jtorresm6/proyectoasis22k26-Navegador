/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Representa un rango salarial utilizado para calcular una
 *  percepción o deducción. Almacena los límites del rango,
 *  el modo de cálculo, su valor, el monto fijo base y el estado
 *  activo, permitiendo un límite superior sin definir.
 * ==================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Entidades
{
    public class ClsPercepcionDeduccionRango
    {
        public int IdPercepcionDeduccionRango { get; set; }
        public int IdPercepcionDeduccion { get; set; }
        public decimal SalarioDesdePercepcionDeduccionRango { get; set; }
        public decimal? SalarioHastaPercepcionDeduccionRango { get; set; }   // NULL = sin limite superior
        public string ModoValorPercepcionDeduccionRango { get; set; }        // ENUM('%','Monto')
        public decimal ValorPercepcionDeduccionRango { get; set; }
        public decimal MontoFijoBasePercepcionDeduccionRango { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
