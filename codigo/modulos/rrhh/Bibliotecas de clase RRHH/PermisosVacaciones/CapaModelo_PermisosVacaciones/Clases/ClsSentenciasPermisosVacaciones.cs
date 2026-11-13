using System;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * ==================================================================
 * Propósito :
 *  Capa Modelo de la ventana 9201 - PermisosVacaciones. Guarda el nombre
 *  de las tablas que mantiene el Navegador en Permisos (9202) y
 *  Vacaciones (9203) y de sus columnas de fecha inicio y fin.
 *  También consulta la fecha de contratación y las vacaciones de un
 *  empleado para validar las reglas del Código de Trabajo.
 * Modificado por : Byron Alexander Chiquito Paredes 0901-23-3747 (23/10/2026)
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)
 * ===================================================================
*/

namespace CapaModelo_PermisosVacaciones.Clases
{
    public class ClsSentenciasPermisosVacaciones
    {
        private readonly string _ConnectionString = "Dsn=EmbutidosS.A";

        private readonly string _TablaPermisos = "tblPermisoEmpleado";
        private readonly string _TablaVacaciones = "tblVacacionEmpleado";

        private readonly string[] _FechasPermisos = { "fechaInicioPermisoEmpleado", "fechaFinPermisoEmpleado" };
        private readonly string[] _FechasVacaciones = { "fechaInicioVacacionEmpleado", "fechaFinVacacionEmpleado" };

        public string RrhhFuncTablaPermisos()
        {
            return _TablaPermisos;
        }

        public string RrhhFuncTablaVacaciones()
        {
            return _TablaVacaciones;
        }

        // [0] = fecha inicio, [1] = fecha fin
        public string[] RrhhFuncFechasPermisos()
        {
            return _FechasPermisos;
        }

        public string[] RrhhFuncFechasVacaciones()
        {
            return _FechasVacaciones;
        }

        public DateTime RrhhFuncFechaContratacion(int IdEmpleado)
        {
            DataTable Tabla = RrhhFuncConsultar(
                "SELECT fechaContratacionEmpleado FROM tblEmpleado WHERE idEmpleado = ?", IdEmpleado);
            return Convert.ToDateTime(Tabla.Rows[0][0]);
        }

        // Vacaciones del empleado: id, fecha inicio y días tomados.
        public DataTable RrhhFuncVacacionesEmpleado(int IdEmpleado)
        {
            return RrhhFuncConsultar(
                @"SELECT idVacacionEmpleado, fechaInicioVacacionEmpleado, diasTomarVacacionEmpleado
                  FROM tblVacacionEmpleado
                  WHERE idEmpleado = ? AND is_active = 1", IdEmpleado);
        }

        private DataTable RrhhFuncConsultar(string Consulta, int IdEmpleado)
        {
            var Tabla = new DataTable();
            using (var Conexion = new OdbcConnection(_ConnectionString))
            using (var Comando = new OdbcCommand(Consulta, Conexion))
            {
                Comando.Parameters.AddWithValue("p_idEmpleado", IdEmpleado);
                Conexion.Open();
                new OdbcDataAdapter(Comando).Fill(Tabla);
            }
            return Tabla;
        }
    }
}
