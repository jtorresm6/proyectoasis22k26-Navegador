/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta los registros de tblPuestoEmpleado para el reporte de la aplicación 9006.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Empleado.Contratos;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Repositorios
{
    public class ClsRepositorioPuestoEmpleado : ClsSentencias, IRepositorioPuestoEmpleado
    {
        private string _SelectAll;

        public ClsRepositorioPuestoEmpleado()
        {
            _SelectAll = @"
                SELECT pe.idPuestoEmpleado, pe.idEmpleado, e.codigoEmpleado,
                       CONCAT(e.nombresEmpleado, ' ', e.apellidosEmpleado) AS nombreEmpleado,
                       pe.idPuesto, p.nombrePuesto,
                       pe.fechaAsignacionPuestoEmpleado, pe.fechaFinPuestoEmpleado, pe.is_active
                FROM tblPuestoEmpleado pe
                INNER JOIN tblEmpleado e ON pe.idEmpleado = e.idEmpleado
                INNER JOIN tblPuesto p ON pe.idPuesto = p.idPuesto
                ORDER BY nombreEmpleado, pe.fechaAsignacionPuestoEmpleado DESC";
        }

        public IEnumerable<ClsPuestoEmpleado> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsPuestoEmpleado>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsPuestoEmpleado
                {
                    IdPuestoEmpleado = Convert.ToInt32(Fila["idPuestoEmpleado"]),
                    IdEmpleado = Convert.ToInt32(Fila["idEmpleado"]),
                    CodigoEmpleado = Fila["codigoEmpleado"].ToString(),
                    NombreEmpleado = Fila["nombreEmpleado"].ToString(),
                    IdPuesto = Convert.ToInt32(Fila["idPuesto"]),
                    NombrePuesto = Fila["nombrePuesto"].ToString(),
                    FechaAsignacionPuestoEmpleado = Convert.ToDateTime(Fila["fechaAsignacionPuestoEmpleado"]),
                    FechaFinPuestoEmpleado = Fila["fechaFinPuestoEmpleado"] == DBNull.Value
                        ? (DateTime?)null
                        : Convert.ToDateTime(Fila["fechaFinPuestoEmpleado"]),
                    EstadoPuestoEmpleado = Convert.ToBoolean(Fila["is_active"])
                });
            }
            return Lista;
        }
    }
}
