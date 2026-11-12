/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Define el contrato de acceso a datos para las percepciones
 *  y deducciones. Hereda las operaciones básicas del repositorio
 *  genérico y agrega el conteo por nombre, permitiendo excluir
 *  un registro durante la validación de nombres duplicados.
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
    public interface IRepositorioPercepcionDeduccion : IRepositorioGenerico<ClsPercepcionDeduccion>
    {
        int RrhhMetContarPorNombre(string NombrePercepcionDeduccion, int IdExcluir);
    }
}
