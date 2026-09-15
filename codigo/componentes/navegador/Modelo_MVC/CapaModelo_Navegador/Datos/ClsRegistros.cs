using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;

namespace CapaModelo_Navegador
{
    // Todo lo que consulta o modifica los datos de una tabla (no metadatos)
    public class ClsRegistros
    {
        ClsConexionBD _ConexionBD = new ClsConexionBD();

        public OdbcDataAdapter NavegadorFuncLlenarTbl(string NombreTabla)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            string ConsultaSQL = "SELECT * FROM " + NombreTabla;
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();
            return new OdbcDataAdapter(ConsultaSQL, NavegadorFuncConexion);
        }

        public DataTable NavegadorFuncConsultarTodo(string NombreTabla)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            string ConsultaSQL = "SELECT * FROM " + NombreTabla;
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();
            DataTable TablaDatos = new DataTable();

            try
            {
                using (OdbcDataAdapter AdaptadorDatos = new OdbcDataAdapter(ConsultaSQL, NavegadorFuncConexion))
                    AdaptadorDatos.Fill(TablaDatos);
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }

            return TablaDatos;
        }

        public bool NavegadorFuncExisteLlavePrimaria(string NombreTabla, string[] CamposPK, string[] ValoresPK)
        {
            if (CamposPK == null || CamposPK.Length == 0) return false;
            if (ValoresPK == null || ValoresPK.Length != CamposPK.Length) return false;

            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            string Condiciones = "";

            for (int Indice = 0; Indice < CamposPK.Length; Indice++)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(CamposPK[Indice]);

                if (Indice > 0) Condiciones += " AND ";
                Condiciones += CamposPK[Indice] + " = ?";
            }

            string ConsultaSQL = "SELECT COUNT(*) FROM " + NombreTabla + " WHERE " + Condiciones;
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, NavegadorFuncConexion))
                {
                    for (int Indice = 0; Indice < ValoresPK.Length; Indice++)
                        Comando.Parameters.AddWithValue("@p" + Indice, ValoresPK[Indice]);

                    int Cantidad = Convert.ToInt32(Comando.ExecuteScalar());
                    return Cantidad > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

        public bool NavegadorFuncExisteValorCampo(string NombreTabla, string NombreCampo, string Valor)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreCampo);

            string ConsultaSQL = "SELECT COUNT(*) FROM " + NombreTabla + " WHERE " + NombreCampo + " = ?";
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, NavegadorFuncConexion))
                {
                    Comando.Parameters.AddWithValue("@valor", Valor);
                    int Cantidad = Convert.ToInt32(Comando.ExecuteScalar());
                    return Cantidad > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

        public bool NavegadorFuncInsertarRegistro(string NombreTabla, Dictionary<string, string> Datos)
        {
            if (Datos == null || Datos.Count == 0) return false;

            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            string Columnas = "";
            string Valores = "";
            int Contador = 0;

            foreach (KeyValuePair<string, string> Dato in Datos)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(Dato.Key);

                if (Contador > 0) { Columnas += ", "; Valores += ", "; }

                Columnas += Dato.Key;
                Valores += "?";
                Contador++;
            }

            string ConsultaSQL = "INSERT INTO " + NombreTabla + " (" + Columnas + ") VALUES (" + Valores + ")";
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, NavegadorFuncConexion))
                {
                    int Pos = 0;

                    foreach (KeyValuePair<string, string> Dato in Datos)
                    {
                        Comando.Parameters.AddWithValue("@p" + Pos, Dato.Value);
                        Pos++;
                    }

                    return Comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

        public bool NavegadorFuncActualizarRegistro(string NombreTabla, Dictionary<string, string> Valores, Dictionary<string, string> ClavesPrimarias)
        {
            if (Valores == null || Valores.Count == 0 || ClavesPrimarias == null || ClavesPrimarias.Count == 0)
                return false;

            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            Dictionary<string, string> ValoresActualizar = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (KeyValuePair<string, string> Dato in Valores)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(Dato.Key);

                if (!ClavesPrimarias.ContainsKey(Dato.Key))
                    ValoresActualizar[Dato.Key] = Dato.Value;
            }

            if (ValoresActualizar.Count == 0) return false;

            StringBuilder Sql = new StringBuilder("UPDATE " + NombreTabla + " SET ");
            int Indice = 0;

            foreach (KeyValuePair<string, string> Dato in ValoresActualizar)
            {
                if (Indice > 0) Sql.Append(", ");
                Sql.Append(Dato.Key + " = ?");
                Indice++;
            }

            Sql.Append(" WHERE ");
            Indice = 0;

            foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(Clave.Key);

                if (Indice > 0) Sql.Append(" AND ");
                Sql.Append(Clave.Key + " = ?");
                Indice++;
            }

            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(Sql.ToString(), NavegadorFuncConexion))
                {
                    foreach (KeyValuePair<string, string> Dato in ValoresActualizar)
                        Comando.Parameters.AddWithValue("@valor_" + Dato.Key, Dato.Value);

                    foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
                        Comando.Parameters.AddWithValue("@pk_" + Clave.Key, Clave.Value);

                    return Comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }
        /*
            Inicio de aporte de código
            Mario Alberto Taracena Pérez
            0901-23-9335
         */
        public bool NavegadorFuncEliminarRegistro(string NombreTabla, Dictionary<string, string> ClavesPrimarias)
        {
            if (ClavesPrimarias == null || ClavesPrimarias.Count == 0) return false;

            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            StringBuilder Sql = new StringBuilder("DELETE FROM " + NombreTabla + " WHERE ");
            int Indice = 0;

            foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(Clave.Key);

                if (Indice > 0) Sql.Append(" AND ");
                Sql.Append(Clave.Key + " = ?");
                Indice++;
            }

            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(Sql.ToString(), NavegadorFuncConexion))
                {
                    foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
                        Comando.Parameters.AddWithValue("@pk_" + Clave.Key, Clave.Value);

                    return Comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

        public OdbcDataAdapter NavegadorFuncFiltrarTbl(string NombreTabla, string Columna, string Valor)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            ClsValidaciones.NavegadorMetValidarIdentificador(Columna);

            string ConsultaSQL = "SELECT * FROM " + NombreTabla + " WHERE " + Columna + " LIKE ?";
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            OdbcCommand Comando = new OdbcCommand(ConsultaSQL, NavegadorFuncConexion);
            Comando.Parameters.AddWithValue("@valor", "%" + Valor + "%");

            return new OdbcDataAdapter(Comando);
        }

        public void NavegadorMetEjecutarSql(string Sql)
        {
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(Sql, NavegadorFuncConexion))
                    Comando.ExecuteNonQuery();
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

        public void NavegadorMetGuardarDatos(string Query)
        {
            try
            {
                using (OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion())
                using (OdbcCommand Comando = new OdbcCommand(Query, NavegadorFuncConexion))
                    Comando.ExecuteNonQuery();
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al ejecutar la sentencia en la base de datos: " + Excepcion.Message, Excepcion);
            }
        }
        /*
            Fin de aporte de código
            Mario Alberto Taracena Pérez
            0901-23-9335
         */
    }
}