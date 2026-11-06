using CapaModelo_Seguridad.Contratos;
using CapaModelo_Seguridad.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * ==================================================================
 * Área : Seguridad
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 22/09/2026
 * ==================================================================
 * Propósito :
 * Aqui es esta clase se encuentran los querys utilizados para
 * realizar las diferentes acciones como insertar, seleccionar,
 * modificar y eliminar junto con el metodo para cada accion.
 * ===================================================================
*/

namespace CapaModelo_Seguridad.Repositorios
{
    public class ClsRepositorioEmpleado : ClsSentencias, IRepositorioEmpleado
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioEmpleado()
        {
            _SelectAll = "SELECT idEmpleado, codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado, " +
                         "idDepartamento, idJornada, salarioEmpleado, generoEmpleado, fechaNacimientoEmpleado, " +
                         "fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado, is_active, created_at, updated_at " +
                         "FROM tblEmpleado";

            _Insert = "INSERT INTO tblEmpleado (codigoEmpleado, dpiEmpleado, nitEmpleado, nombresEmpleado, apellidosEmpleado, " +
                      "idDepartamento, idJornada, salarioEmpleado, generoEmpleado, fechaNacimientoEmpleado, " +
                      "fechaContratacionEmpleado, telefonoEmpleado, correoEmpleado) " +
                      "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            _Update = "UPDATE tblEmpleado SET codigoEmpleado=?, dpiEmpleado=?, nitEmpleado=?, nombresEmpleado=?, apellidosEmpleado=?, " +
                      "idDepartamento=?, idJornada=?, salarioEmpleado=?, generoEmpleado=?, fechaNacimientoEmpleado=?, " +
                      "fechaContratacionEmpleado=?, telefonoEmpleado=?, correoEmpleado=? WHERE idEmpleado=?";

            _Delete = "DELETE FROM tblEmpleado WHERE idEmpleado=?";
        }

        public int SeguridadMetAgregar(ClsEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_codigoEmpleado", Entidad.CodigoEmpleado));
            Parametros.Add(new OdbcParameter("p_dpiEmpleado", Entidad.DpiEmpleado));
            Parametros.Add(new OdbcParameter("p_nitEmpleado", (object)Entidad.NitEmpleado ?? DBNull.Value));
            Parametros.Add(new OdbcParameter("p_nombresEmpleado", Entidad.NombresEmpleado));
            Parametros.Add(new OdbcParameter("p_apellidosEmpleado", Entidad.ApellidosEmpleado));
            Parametros.Add(new OdbcParameter("p_idDepartamento", Entidad.IdDepartamento));
            Parametros.Add(new OdbcParameter("p_idJornada", Entidad.IdJornada));
            Parametros.Add(new OdbcParameter("p_salarioEmpleado", Entidad.SalarioEmpleado));
            Parametros.Add(new OdbcParameter("p_generoEmpleado", Entidad.GeneroEmpleado));
            Parametros.Add(new OdbcParameter("p_fechaNacimientoEmpleado", Entidad.FechaNacimientoEmpleado));
            Parametros.Add(new OdbcParameter("p_fechaContratacionEmpleado", Entidad.FechaContratacionEmpleado));
            Parametros.Add(new OdbcParameter("p_telefonoEmpleado", (object)Entidad.TelefonoEmpleado ?? DBNull.Value));
            Parametros.Add(new OdbcParameter("p_correoEmpleado", (object)Entidad.CorreoEmpleado ?? DBNull.Value));

            return SeguridadMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int SeguridadMetEditar(ClsEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_codigoEmpleado", Entidad.CodigoEmpleado));
            Parametros.Add(new OdbcParameter("p_dpiEmpleado", Entidad.DpiEmpleado));
            Parametros.Add(new OdbcParameter("p_nitEmpleado", (object)Entidad.NitEmpleado ?? DBNull.Value));
            Parametros.Add(new OdbcParameter("p_nombresEmpleado", Entidad.NombresEmpleado));
            Parametros.Add(new OdbcParameter("p_apellidosEmpleado", Entidad.ApellidosEmpleado));
            Parametros.Add(new OdbcParameter("p_idDepartamento", Entidad.IdDepartamento));
            Parametros.Add(new OdbcParameter("p_idJornada", Entidad.IdJornada));
            Parametros.Add(new OdbcParameter("p_salarioEmpleado", Entidad.SalarioEmpleado));
            Parametros.Add(new OdbcParameter("p_generoEmpleado", Entidad.GeneroEmpleado));
            Parametros.Add(new OdbcParameter("p_fechaNacimientoEmpleado", Entidad.FechaNacimientoEmpleado));
            Parametros.Add(new OdbcParameter("p_fechaContratacionEmpleado", Entidad.FechaContratacionEmpleado));
            Parametros.Add(new OdbcParameter("p_telefonoEmpleado", (object)Entidad.TelefonoEmpleado ?? DBNull.Value));
            Parametros.Add(new OdbcParameter("p_correoEmpleado", (object)Entidad.CorreoEmpleado ?? DBNull.Value));
            Parametros.Add(new OdbcParameter("p_idEmpleado", Entidad.IdEmpleado));

            return SeguridadMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int SeguridadMetRemover(ClsEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idEmpleado", Entidad.IdEmpleado));

            return SeguridadMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsEmpleado> SeguridadMetObtenerTodos()
        {
            var ListaEmpleados = new List<ClsEmpleado>();
            var TablaDatos = SeguridadMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Empleado = new ClsEmpleado();
                Empleado.IdEmpleado = Convert.ToInt32(Fila[0]);
                Empleado.CodigoEmpleado = Fila[1].ToString();
                Empleado.DpiEmpleado = Fila[2].ToString();
                Empleado.NitEmpleado = Fila[3] == DBNull.Value ? null : Fila[3].ToString();
                Empleado.NombresEmpleado = Fila[4].ToString();
                Empleado.ApellidosEmpleado = Fila[5].ToString();
                Empleado.IdDepartamento = Convert.ToInt32(Fila[6]);
                Empleado.IdJornada = Convert.ToInt32(Fila[7]);
                Empleado.SalarioEmpleado = Convert.ToDecimal(Fila[8]);
                Empleado.GeneroEmpleado = Fila[9].ToString();
                Empleado.FechaNacimientoEmpleado = Convert.ToDateTime(Fila[10]);
                Empleado.FechaContratacionEmpleado = Convert.ToDateTime(Fila[11]);
                Empleado.TelefonoEmpleado = Fila[12] == DBNull.Value ? null : Fila[12].ToString();
                Empleado.CorreoEmpleado = Fila[13] == DBNull.Value ? null : Fila[13].ToString();
                Empleado.IsActive = Convert.ToBoolean(Fila[14]);
                Empleado.CreatedAt = Convert.ToDateTime(Fila[15]);
                Empleado.UpdatedAt = Convert.ToDateTime(Fila[16]);
                ListaEmpleados.Add(Empleado);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaEmpleados;
        }
    }
}