using CapaModelo_AutorizacionOrdenes.Contratos;
using CapaModelo_AutorizacionOrdenes.Entidades;
using CapaModelo_AutorizacionOrdenes.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 07/11/2026
 * ==================================================================
 * Propósito :
 *  La clase Repositorio de Tipo de Cuenta Bancaria se encarga de 
 *  implementar las operaciones CRUD sobre la tabla tblTipoCuentaBancaria 
 *  mediante ODBC, ejecutando las sentencias SQL de inserción, 
 *  actualización, eliminación y consulta heredando de la clase 
 *  ClsSentencias para la ejecución de comandos e implementa la  
 *  interfaz IRepositorioTipoCuentaBancaria como contrato de la capa 
 *  modelo.
 * ===================================================================
*/

namespace CapaModelo_Bancos.Repositorios
{
    public class ClsRepositorioTipoCuentaBancaria : ClsSentencias, IRepositorioTipoCuentaBancaria
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioTipoCuentaBancaria()
        {
            _SelectAll = "SELECT idTipoCuentaBancaria, codigoTipoCuentaBancaria, nombreTipoCuentaBancaria, " +
                                      "is_active, created_at, updated_at " +
                                      "FROM tblTipoCuentaBancaria";
            _Insert = "INSERT INTO tblTipoCuentaBancaria VALUES (DEFAULT, ?, ?, ?, DEFAULT, DEFAULT)";
            _Update = "UPDATE tblTipoCuentaBancaria SET codigoTipoCuentaBancaria=?, nombreTipoCuentaBancaria=?, is_active=? WHERE idTipoCuentaBancaria=?";
            _Delete = "DELETE FROM tblTipoCuentaBancaria WHERE idTipoCuentaBancaria=?";
        }

        public int BancosMetAgregar(ClsTipoCuentaBancaria Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_codigoTipoCuentaBancaria", Entidad.CodigoTipoCuentaBancaria));
            Parametros.Add(new OdbcParameter("p_nombreTipoCuentaBancaria", Entidad.NombreTipoCuentaBancaria));
            Parametros.Add(new OdbcParameter("p_is_active", Entidad.IsActive));
            return BancosMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int BancosMetEditar(ClsTipoCuentaBancaria Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_codigoTipoCuentaBancaria", Entidad.CodigoTipoCuentaBancaria));
            Parametros.Add(new OdbcParameter("p_nombreTipoCuentaBancaria", Entidad.NombreTipoCuentaBancaria));
            Parametros.Add(new OdbcParameter("p_is_active", Entidad.IsActive));
            Parametros.Add(new OdbcParameter("p_Pk_TipoCuentaBancaria", Entidad.IdTipoCuentaBancaria));
            return BancosMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int BancosMetRemover(ClsTipoCuentaBancaria Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_Pk_TipoCuentaBancaria", Entidad.IdTipoCuentaBancaria));
            return BancosMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsTipoCuentaBancaria> BancosMetObtenerTodos()
        {
            var ListaTiposCuenta = new List<ClsTipoCuentaBancaria>();
            var TablaDatos = BancosMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Row in TablaDatos.Rows)
            {
                var TipoCuenta = new ClsTipoCuentaBancaria();
                TipoCuenta.IdTipoCuentaBancaria = Convert.ToInt32(Row[0]);
                TipoCuenta.CodigoTipoCuentaBancaria = Convert.ToString(Row[1]);
                TipoCuenta.NombreTipoCuentaBancaria = Convert.ToString(Row[2]);
                TipoCuenta.IsActive = Convert.ToBoolean(Row[3]);
                TipoCuenta.CreatedAt = Convert.ToDateTime(Row[4]);
                TipoCuenta.UpdatedAt = Convert.ToDateTime(Row[5]);
                ListaTiposCuenta.Add(TipoCuenta);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaTiposCuenta;
        }
    }
}