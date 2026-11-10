/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Autor  : Andre De Jesus Gonzalez Camey
 * Carné  : 9959-23-1224 -- 9959-23-3117
 * Fecha  : 08/11/2026
 * ================================================================
 * Propósito :
 *  Representa los datos de una conciliación bancaria utilizados
 *  durante la consulta y el cierre de un período.
 * ================================================================
 */
using System;

namespace CapaModelo_CierreBancario.Entidades
{
    public class ClsConciliacion
    {
        public long   PkConciliacion       { get; set; }
        public DateTime FechaCorte         { get; set; }
        public string EstadoConciliacion   { get; set; }
        public string MetodoConciliacion   { get; set; }
        public decimal SaldoLibro          { get; set; }
        public decimal SaldoBanco          { get; set; }
        public decimal Diferencia          { get; set; }
        public string Responsable          { get; set; }
        public DateTime? FechaCierre       { get; set; }
        public string NombreCuenta         { get; set; }
        public string NumeroCuenta         { get; set; }
    }
}
