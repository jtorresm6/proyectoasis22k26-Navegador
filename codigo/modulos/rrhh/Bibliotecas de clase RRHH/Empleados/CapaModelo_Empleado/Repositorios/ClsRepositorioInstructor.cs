using CapaModelo_Empleado.Contratos;
using CapaModelo_Empleado.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Empleado.Repositorios
{
    public class ClsRepositorioInstructor : ClsSentencias, IRepositorioInstructor
    {
        private string _SelectAll;

        public ClsRepositorioInstructor()
        {
            _SelectAll = @"
                SELECT i.idInstructor, i.nombreInstructor, i.especialidadInstructor,
                       i.correoInstructor, i.is_active
                FROM tblInstructor i
                ORDER BY i.nombreInstructor";
        }

        public IEnumerable<ClsInstructor> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsInstructor>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new ClsInstructor
                {
                    IdInstructor = Convert.ToInt32(Fila["idInstructor"]),
                    NombreInstructor = Fila["nombreInstructor"].ToString(),
                    EspecialidadInstructor = Fila["especialidadInstructor"].ToString(),
                    CorreoInstructor = Fila["correoInstructor"].ToString(),
                    EstadoInstructor = Convert.ToBoolean(Fila["is_active"])
                });
            }
            return Lista;
        }
    }
}
