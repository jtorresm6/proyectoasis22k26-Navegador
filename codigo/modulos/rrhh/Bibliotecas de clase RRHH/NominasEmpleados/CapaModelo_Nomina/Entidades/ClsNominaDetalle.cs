/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Representa una línea del detalle de nómina, relacionando el
 *  encabezado con una percepción o deducción y su monto registrado.
 *  Incluye el identificador del detalle y las fechas de creación
 *  y actualización del registro.
 * ==================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Entidades
{
    public class ClsNominaDetalle
    {
        public int IdNominaDetalle { get; set; }
        public int IdNominaEncabezado { get; set; }
        public int IdPercepcionDeduccion { get; set; }
        public decimal MontoNominaDetalle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
