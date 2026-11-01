/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta los registros de tblPuesto para el reporte de la aplicación 9004.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Empleado.Contratos;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Repositorios
{
    public class ClsRepositorioPuesto : ClsSentencias, IRepositorioPuesto
    {
        private string _SelectAll;

        public ClsRepositorioPuesto()
        {
            _SelectAll = @"
                SELECT p.idPuesto, p.idTipoPuesto, t.nombreTipoPuesto,
                       p.nombrePuesto, p.descripcionPuesto, p.is_active
                FROM tblPuesto p
                INNER JOIN tblTipoPuesto t ON p.idTipoPuesto = t.idTipoPuesto
                ORDER BY t.nombreTipoPuesto, p.nombrePuesto";
        }

        public IEnumerable<ClsPuesto> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsPuesto>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsPuesto
                {
                    IdPuesto = Convert.ToInt32(Fila["idPuesto"]),
                    IdTipoPuesto = Convert.ToInt32(Fila["idTipoPuesto"]),
                    NombreTipoPuesto = Fila["nombreTipoPuesto"].ToString(),
                    NombrePuesto = Fila["nombrePuesto"].ToString(),
                    DescripcionPuesto = Fila["descripcionPuesto"].ToString(),
                    EstadoPuesto = Convert.ToBoolean(Fila["is_active"])
                });
            }
            return Lista;
        }
    }
}
