/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta los registros de tblTipoPuesto para el reporte de la aplicación 9003.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Empleado.Contratos;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Repositorios
{
    public class ClsRepositorioTipoPuesto : ClsSentencias, IRepositorioTipoPuesto
    {
        private string _SelectAll;

        public ClsRepositorioTipoPuesto()
        {
            _SelectAll = @"
                SELECT idTipoPuesto, nombreTipoPuesto, is_active
                FROM tblTipoPuesto
                ORDER BY nombreTipoPuesto";
        }

        public IEnumerable<ClsTipoPuesto> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsTipoPuesto>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsTipoPuesto
                {
                    IdTipoPuesto = Convert.ToInt32(Fila["idTipoPuesto"]),
                    NombreTipoPuesto = Fila["nombreTipoPuesto"].ToString(),
                    EstadoTipoPuesto = Convert.ToBoolean(Fila["is_active"])
                });
            }
            return Lista;
        }
    }
}
