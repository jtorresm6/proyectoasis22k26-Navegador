using CapaModelo_Nomina.Contratos;
using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Repositorios
{
    public class ClsRepositorioReporteNomina : ClsSentencias, IRepositorioReporteNomina
    {
        public List<ClsReporteNomina> RrhhMetObtenerPorPeriodo(
            DateTime FechaInicio,
            DateTime FechaFinal)
        {
            string Sql = @"
                SELECT
                    ne.idNominaEncabezado,
                    ne.idEmpleado,
                    TRIM(CONCAT(
                        COALESCE(e.nombresEmpleado, ''),
                        ' ',
                        COALESCE(e.apellidosEmpleado, '')
                    )) AS NombreEmpleado,
                    ne.fechaInicioNominaEncabezado,
                        ne.fechaFinalNominaEncabezado,
                    ne.salarioBaseNominaEncabezado,
                    ne.totalPercepcionNominaEncabezado,
                    ne.totalDeduccionNominaEncabezado,
                    ne.totalNetoNominaEncabezado,
                    ne.estadoNminaEncabezado
                FROM tblNominaEncabezado ne
                INNER JOIN tblEmpleado e
                    ON e.idEmpleado = ne.idEmpleado
                WHERE ne.fechaInicioNominaEncabezado >= ?
                  AND ne.fechaFinalNominaEncabezado <= ?
                  AND ne.estadoNminaEncabezado <> 'Rechazada'
                ORDER BY
                    NombreEmpleado,
                    ne.fechaInicioNominaEncabezado,
                    ne.idNominaEncabezado";

            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("P_fechaInicio", OdbcType.Date)
                {
                    Value = FechaInicio.Date
                },
                new OdbcParameter("P_fechaFinal", OdbcType.Date)
                {
                    Value = FechaFinal.Date
                }
            };

            DataTable Tabla = RrhhMetEjecucionConsulta(
                Sql,
                CommandType.Text,
                Parametros);

            var Lista = new List<ClsReporteNomina>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Lista.Add(new ClsReporteNomina
                {
                    IdNominaEncabezado =
                        Convert.ToInt32(Fila["idNominaEncabezado"]),

                    IdEmpleado =
                        Convert.ToInt32(Fila["idEmpleado"]),

                    NombreEmpleado =
                        Convert.ToString(Fila["NombreEmpleado"]),

                    FechaInicio =
                        Convert.ToDateTime(
                            Fila["fechaInicioNominaEncabezado"]),

                    FechaFinal =
                        Convert.ToDateTime(
                            Fila["fechaFinalNominaEncabezado"]),

                    SalarioBase =
                        Convert.ToDecimal(
                            Fila["salarioBaseNominaEncabezado"]),

                    TotalPercepciones =
                        Convert.ToDecimal(
                            Fila["totalPercepcionNominaEncabezado"]),

                    TotalDeducciones =
                        Convert.ToDecimal(
                            Fila["totalDeduccionNominaEncabezado"]),

                    TotalNeto =
                        Convert.ToDecimal(
                            Fila["totalNetoNominaEncabezado"]),

                    EstadoNomina =
                        Convert.ToString(Fila["estadoNminaEncabezado"])
                });
            }

            return Lista;
        }

        

    }
}
