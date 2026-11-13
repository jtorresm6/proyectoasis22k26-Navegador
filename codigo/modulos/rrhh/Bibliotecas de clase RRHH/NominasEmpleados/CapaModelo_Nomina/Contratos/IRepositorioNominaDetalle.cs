/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Define las operaciones para registrar líneas de detalle en
 *  lote, consultar y contar los detalles de una nómina y obtener
 *  el identificador de un concepto por su nombre, necesarias
 *  para la generación del desglose de percepciones y deducciones.
 * ==================================================================
 */

using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Contratos
{
    public interface IRepositorioNominaDetalle
    {
        // Inserta todas las lineas en una sola transaccion 
        int RrhhMetAgregarLote(List<ClsNominaDetalle> Lineas);

        IEnumerable<ClsNominaDetalle> RrhhMetObtenerPorNomina(int IdNominaEncabezado);

        int RrhhMetContarPorNomina(int IdNominaEncabezado);

        int? RrhhMetObtenerIdConceptoPorNombre(string NombreConcepto);
    }
}
