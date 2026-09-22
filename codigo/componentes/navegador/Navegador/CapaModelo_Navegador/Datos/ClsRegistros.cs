using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;

namespace CapaModelo_Navegador
{
    public class ClsRegistros
    {
        private ClsConexionBD _ConexionBD = new ClsConexionBD();

        public OdbcDataAdapter NavegadorFuncLlenarTbl(string NombreTabla)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            string ConsultaSQL = "SELECT * FROM " + NombreTabla;
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();
            return new OdbcDataAdapter(ConsultaSQL, Conexion);
        }

        public DataTable NavegadorFuncConsultarTodo(string NombreTabla)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            string ConsultaSQL = "SELECT * FROM " + NombreTabla;
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();
            DataTable TablaDatos = new DataTable();

            try
            {
                using (OdbcDataAdapter AdaptadorDatos = new OdbcDataAdapter(ConsultaSQL, Conexion))
                    AdaptadorDatos.Fill(TablaDatos);
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
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
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    for (int Indice = 0; Indice < ValoresPK.Length; Indice++)
                        Comando.Parameters.AddWithValue("@p" + Indice, ValoresPK[Indice]);

                    int Cantidad = Convert.ToInt32(Comando.ExecuteScalar());
                    return Cantidad > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }
        }

        public bool NavegadorFuncExisteValorCampo(string NombreTabla, string NombreCampo, string Valor)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreCampo);

            string ConsultaSQL = "SELECT COUNT(*) FROM " + NombreTabla + " WHERE " + NombreCampo + " = ?";
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@valor", Valor);
                    int Cantidad = Convert.ToInt32(Comando.ExecuteScalar());
                    return Cantidad > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
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
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    int Posicion = 0;

                    foreach (KeyValuePair<string, string> Dato in Datos)
                    {
                        Comando.Parameters.AddWithValue("@p" + Posicion, Dato.Value);
                        Posicion++;
                    }

                    return Comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
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

            StringBuilder ConsultaSQL = new StringBuilder("UPDATE " + NombreTabla + " SET ");
            int Indice = 0;

            foreach (KeyValuePair<string, string> Dato in ValoresActualizar)
            {
                if (Indice > 0) ConsultaSQL.Append(", ");
                ConsultaSQL.Append(Dato.Key + " = ?");
                Indice++;
            }

            ConsultaSQL.Append(" WHERE ");
            Indice = 0;

            foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(Clave.Key);

                if (Indice > 0) ConsultaSQL.Append(" AND ");
                ConsultaSQL.Append(Clave.Key + " = ?");
                Indice++;
            }

            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL.ToString(), Conexion))
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
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }
        }

        // ====================================================================
        // Nombre:        Oskar Saul Cermeño Jimenez
        // Carnet:        0901-23-15379
        // Fecha:         16/09/2026
        // Función:       NavegadorFuncEliminarRegistro
        // Descripción:   Elimina físicamente un registro en la base de datos 
        //                ejecutando una sentencia DELETE parametrizada, filtrando
        //                únicamente por las llaves primarias pasadas en el diccionario.
        // Parámetros:    - NombreTabla: Nombre de la tabla sobre la cual eliminar.
        //                - ClavesPrimarias: Diccionario con los campos clave y sus valores.
        // Retorna:       True si se eliminó una o más filas en la base de datos, False de lo contrario.
        // ====================================================================
        public bool NavegadorFuncEliminarRegistro(string NombreTabla, Dictionary<string, string> ClavesPrimarias)
        {
            if (ClavesPrimarias == null || ClavesPrimarias.Count == 0) return false;

            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            StringBuilder ConsultaSQL = new StringBuilder("DELETE FROM " + NombreTabla + " WHERE ");
            int Indice = 0;

            foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(Clave.Key);

                if (Indice > 0) ConsultaSQL.Append(" AND ");
                ConsultaSQL.Append(Clave.Key + " = ?");
                Indice++;
            }

            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL.ToString(), Conexion))
                {
                    foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
                        Comando.Parameters.AddWithValue("@pk_" + Clave.Key, Clave.Value);

                    return Comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }
        }

        public DataTable NavegadorFuncFiltrarDatos(string NombreTabla, string Columna, string Valor)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            ClsValidaciones.NavegadorMetValidarIdentificador(Columna);

            string ConsultaSQL = "SELECT * FROM " + NombreTabla + " WHERE " + Columna + " LIKE ?";
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();
            DataTable TablaDatos = new DataTable();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@valor", "%" + Valor + "%");
                    using (OdbcDataAdapter AdaptadorDatos = new OdbcDataAdapter(Comando))
                        AdaptadorDatos.Fill(TablaDatos);
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }

            return TablaDatos;
        }

        // ====================================================================
        // Nombre:        Oskar Saul Cermeño Jimenez
        // Carnet:        0901-23-15379
        // Fecha:         16/09/2026
        // Función:       NavegadorFuncFiltrarTbl
        // Descripción:   Genera un OdbcDataAdapter configurado para realizar una 
        //                búsqueda por coincidencia de texto mediante LIKE (%valor%)
        //                sobre una columna específica, manteniendo la conexión abierta.
        // Parámetros:    - NombreTabla: Tabla sobre la cual se aplicará el filtro.
        //                - Columna: Columna utilizada como criterio de búsqueda.
        //                - Valor: Cadena de búsqueda a comparar.
        // Retorna:       Instancia de OdbcDataAdapter con la consulta parametrizada.
        // ====================================================================
        public OdbcDataAdapter NavegadorFuncFiltrarTbl(string NombreTabla, string Columna, string Valor)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            ClsValidaciones.NavegadorMetValidarIdentificador(Columna);

            string ConsultaSQL = "SELECT * FROM " + NombreTabla + " WHERE " + Columna + " LIKE ?";
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion);
            Comando.Parameters.AddWithValue("@valor", "%" + Valor + "%");

            return new OdbcDataAdapter(Comando);
        }

        // ====================================================================
        // Nombre:        Oskar Saul Cermeño Jimenez
        // Carnet:        0901-23-15379
        // Fecha:         16/09/2026
        // Procedimiento: NavegadorMetEjecutarSql
        // Descripción:   Ejecuta una instrucción SQL de forma directa sin esperar
        //                un conjunto de resultados (ExecuteNonQuery), garantizando
        //                el cierre y liberación de la conexión mediante un bloque finally.
        // Parámetros:    - ConsultaSQL: Sentencia SQL a ejecutar.
        // ====================================================================
        public void NavegadorMetEjecutarSql(string ConsultaSQL)
        {
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                    Comando.ExecuteNonQuery();
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }
        }

        // ====================================================================
        // Nombre:        Oskar Saul Cermeño Jimenez
        // Carnet:        0901-23-15379
        // Fecha:         16/09/2026
        // Procedimiento: NavegadorMetGuardarDatos
        // Descripción:   Ejecuta una sentencia SQL para persistencia de datos 
        //                manejando la apertura y cierre de la conexión mediante bloques 
        //                using, capturando fallos para relanzarlos como una excepción amigable.
        // Parámetros:    - ConsultaSQL: Sentencia SQL de persistencia a ejecutar.
        // ====================================================================
        public void NavegadorMetGuardarDatos(string ConsultaSQL)
        {
            try
            {
                using (OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion())
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                    Comando.ExecuteNonQuery();
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al ejecutar la sentencia en la base de datos: " + Excepcion.Message, Excepcion);
            }
        }
    }
}