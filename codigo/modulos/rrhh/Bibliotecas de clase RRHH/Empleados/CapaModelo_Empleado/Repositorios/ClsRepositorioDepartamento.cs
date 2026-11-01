/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta los registros de tblDepartamento para el reporte de la aplicación 9002.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Empleado.Contratos;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Repositorios
{
    public class ClsRepositorioDepartamento : ClsSentencias, IRepositorioDepartamento
    {
        private string _SelectAll;

        public ClsRepositorioDepartamento()
        {
            _SelectAll = @"
                SELECT idDepartamento, nombreDepartamento, is_active
                FROM tblDepartamento
                ORDER BY nombreDepartamento";
        }

        public IEnumerable<ClsDepartamento> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsDepartamento>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsDepartamento
                {
                    IdDepartamento = Convert.ToInt32(Fila["idDepartamento"]),
                    NombreDepartamento = Fila["nombreDepartamento"].ToString(),
                    EstadoDepartamento = Convert.ToBoolean(Fila["is_active"])
                });
            }
            return Lista;
        }
    }
}
