/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta los registros de tblJornada para el reporte de la aplicación 9005.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Empleado.Contratos;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Repositorios
{
    public class ClsRepositorioJornada : ClsSentencias, IRepositorioJornada
    {
        private string _SelectAll;

        public ClsRepositorioJornada()
        {
            _SelectAll = @"
                SELECT idJornada, nombreJornada, horaInicioJornada, horaFinJornada,
                       minutosToleranciaJornada, is_active
                FROM tblJornada
                ORDER BY horaInicioJornada";
        }

        public IEnumerable<ClsJornada> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsJornada>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsJornada
                {
                    IdJornada = Convert.ToInt32(Fila["idJornada"]),
                    NombreJornada = Fila["nombreJornada"].ToString(),
                    HoraInicioJornada = TimeSpan.Parse(Fila["horaInicioJornada"].ToString()),
                    HoraFinJornada = TimeSpan.Parse(Fila["horaFinJornada"].ToString()),
                    MinutosToleranciaJornada = Convert.ToInt32(Fila["minutosToleranciaJornada"]),
                    EstadoJornada = Convert.ToBoolean(Fila["is_active"])
                });
            }
            return Lista;
        }
    }
}
