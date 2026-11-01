using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Empleado.Contratos;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Repositorios
{
    public class ClsRepositorioCapacitacion : ClsSentencias, IRepositorioCapacitacion
    {
        private string _SelectAll;

        public ClsRepositorioCapacitacion()
        {
            _SelectAll = @"
                SELECT c.idCapacitacion, c.idDepartamento, d.nombreDepartamento,
                       c.nombreCapacitacion, c.descripcionCapacitacion,
                       c.fechaInicioCapacitacion, c.fechaFinCapacitacion,
                       c.costoCapacitacion, c.cupoCapacitacion, c.estadoCapacitacion
                FROM tblCapacitacion c
                LEFT JOIN tblDepartamento d ON c.idDepartamento = d.idDepartamento
                ORDER BY c.fechaInicioCapacitacion";
        }

        public IEnumerable<ClsCapacitacion> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsCapacitacion>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsCapacitacion
                {
                    IdCapacitacion = Convert.ToInt32(Fila["idCapacitacion"]),
                    IdDepartamento = Fila["idDepartamento"] == DBNull.Value
                        ? (int?)null
                        : Convert.ToInt32(Fila["idDepartamento"]),
                    NombreDepartamento = Fila["nombreDepartamento"] == DBNull.Value
                        ? "Sin departamento"
                        : Fila["nombreDepartamento"].ToString(),
                    NombreCapacitacion = Fila["nombreCapacitacion"].ToString(),
                    DescripcionCapacitacion = Fila["descripcionCapacitacion"].ToString(),
                    FechaInicioCapacitacion = Convert.ToDateTime(Fila["fechaInicioCapacitacion"]),
                    FechaFinCapacitacion = Convert.ToDateTime(Fila["fechaFinCapacitacion"]),
                    CostoCapacitacion = Convert.ToDecimal(Fila["costoCapacitacion"]),
                    CupoCapacitacion = Convert.ToInt32(Fila["cupoCapacitacion"]),
                    EstadoCapacitacion = Convert.ToBoolean(Fila["estadoCapacitacion"])
                });
            }
            return Lista;
        }
    }
}