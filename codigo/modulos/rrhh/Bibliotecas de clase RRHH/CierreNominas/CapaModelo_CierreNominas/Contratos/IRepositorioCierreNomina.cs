using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_CierreNominas.Entidades;

/*
 * ==================================================================
 * Área: Recursos Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 31/10/2026
 * ==================================================================
 * Propósito : Contrato de la capa Modelo para el repositorio de
 * Cierre de Nómina. Hereda del repositorio genérico y agrega
 * las consultas propias del cierre: nóminas abiertas, previo
 * por empleado, líneas para la póliza y verificación de
 * nóminas ya cerradas.
 * ===================================================================
 */

namespace CapaModelo_CierreNominas.Contratos
{
    public interface IRepositorioCierreNomina : IRepositorioGenerico<ClsCierreNomina>
    {
        IEnumerable<ClsNominaAbierta> RrhhMetObtenerNominasAbiertas();

        IEnumerable<ClsPrevioCierreNomina> RrhhMetObtenerPrevio(DateTime FechaInicio, DateTime FechaFinal);

        IEnumerable<ClsLineaPolizaNomina> RrhhMetObtenerLineasPoliza(DateTime FechaInicio, DateTime FechaFinal);

        IEnumerable<ClsCierreNominaEmpleado> RrhhMetObtenerDetalleCierre(int IdCierreNomina);

        bool RrhhMetNominaCerrada(int IdNominaEncabezado);
    }
}