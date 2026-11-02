using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Repositorios
{
    public class ClsRepositorioCatalogos : ClsSentencias
    {
        public List<KeyValuePair<int, string>> RrhhMetObtenerEmpleados()
        {
            var Lista = new List<KeyValuePair<int, string>>();
            var TablaDatos = RrhhMetEjecucionConsulta(
                "SELECT idEmpleado, nombresEmpleado, apellidosEmpleado FROM tblEmpleado " +
                "ORDER BY nombresEmpleado, apellidosEmpleado", CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                string Nombre = (Fila["nombresEmpleado"].ToString() + " " + Fila["apellidosEmpleado"].ToString()).Trim();
                Lista.Add(new KeyValuePair<int, string>(Convert.ToInt32(Fila["idEmpleado"]), Nombre));
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }

        // Solo empleados activos 
        public List<KeyValuePair<int, string>> RrhhMetObtenerEmpleadosActivos()
        {
            var Lista = new List<KeyValuePair<int, string>>();
            var TablaDatos = RrhhMetEjecucionConsulta(
                "SELECT idEmpleado, nombresEmpleado, apellidosEmpleado FROM tblEmpleado " +
                "WHERE is_active = 1 ORDER BY nombresEmpleado, apellidosEmpleado", CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                string Nombre = (Fila["nombresEmpleado"].ToString() + " " + Fila["apellidosEmpleado"].ToString()).Trim();
                Lista.Add(new KeyValuePair<int, string>(Convert.ToInt32(Fila["idEmpleado"]), Nombre));
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }

        // Nombre del departamento al que pertenece el empleado
        public string RrhhMetObtenerDepartamentoEmpleado(int IdEmpleado)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idEmpleado", OdbcType.Int) { Value = IdEmpleado });
            var Tabla = RrhhMetEjecucionConsulta(
                "SELECT d.nombreDepartamento FROM tblEmpleado e " +
                "LEFT JOIN tblDepartamento d ON d.idDepartamento = e.idDepartamento " +
                "WHERE e.idEmpleado = ?", CommandType.Text, Parametros);
            if (Tabla.Rows.Count == 0 || Tabla.Rows[0][0] == DBNull.Value)
                return "Sin departamento";
            return Tabla.Rows[0][0].ToString();
        }

        public List<KeyValuePair<int, string>> RrhhMetObtenerDepartamentos()
        {
            var Lista = new List<KeyValuePair<int, string>>();
            var TablaDatos = RrhhMetEjecucionConsulta(
                "SELECT idDepartamento, nombreDepartamento FROM tblDepartamento " +
                "ORDER BY nombreDepartamento", CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Lista.Add(new KeyValuePair<int, string>(
                    Convert.ToInt32(Fila["idDepartamento"]), Fila["nombreDepartamento"].ToString()));
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }
    }
}
