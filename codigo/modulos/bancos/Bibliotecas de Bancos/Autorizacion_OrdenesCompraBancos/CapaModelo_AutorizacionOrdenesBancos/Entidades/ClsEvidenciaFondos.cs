using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  La entidad ClsEvidenciaFondos representa la evidencia del saldo y del tipo
 *  de cambio utilizados al decidir (tabla tblAutDisponibilidadFondos).
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Entidades
{
    public class ClsEvidenciaFondos
    {
        public long IdAutorizacionDisponibilidadFondos { get; set; }
        public long IdAutorizacionOrden { get; set; }
        public long IdCuenta { get; set; }
        public DateTime FechaCorteDisponibilidadFondos { get; set; }
        public DateTime FechaEvaluacionDisponibilidadFondos { get; set; }
        public decimal SaldoDisponibleDisponibilidadFondos { get; set; }
        public long IdMonedaOrigen { get; set; }
        public long IdMonedaDestino { get; set; }
        public decimal TipoCambioDisponibilidadFondos { get; set; }
        public long? IdTipoCambio { get; set; }
        public string FuenteDisponibilidadFondos { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
