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

        /*
         * Nombre: Oskar Saul Cermeño Jimenez
         * Carnet: 0901-23-15379
         * Fecha: 14/09/2026
         * Descripción: Prepara y retorna un OdbcDataAdapter con una consulta SELECT * para enlace desacoplado de datos.
         */
        public OdbcDataAdapter NavegadorFuncLlenarTbl(string NombreTabla)
        {
            // Valida que el nombre de la tabla no contenga caracteres inválidos o inyección SQL
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            // Construye la sentencia de selección completa para la tabla
            string ConsultaSQL = "SELECT * FROM " + NombreTabla;

            // Abre y obtiene la conexión física hacia la base de datos
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            // Retorna la instancia del adaptador enlazada a la consulta y conexión establecida
            return new OdbcDataAdapter(ConsultaSQL, NavegadorFuncConexion);
        }

        /*
         * Nombre: Oskar Saul Cermeño Jimenez
         * Carnet: 0901-23-15379
         * Fecha: 14/09/2026
         * Descripción: Ejecuta la consulta de todos los registros de una tabla y los carga en un DataTable liberando la conexión.
         */
        public DataTable NavegadorFuncConsultarTodo(string NombreTabla)
        {
            // Valida el identificador del nombre de la tabla
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            // Define la consulta de lectura total
            string ConsultaSQL = "SELECT * FROM " + NombreTabla;

            // Establece la conexión con el repositorio de datos
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            // Estructura en memoria donde se alojará el conjunto de resultados
            DataTable TablaDatos = new DataTable();

            try
            {
                // Crea el adaptador dentro de un bloque using para asegurar su desecho al poblar el DataTable
                using (OdbcDataAdapter AdaptadorDatos = new OdbcDataAdapter(ConsultaSQL, NavegadorFuncConexion))
                    AdaptadorDatos.Fill(TablaDatos);
            }
            finally
            {
                // Garantiza el cierre determinista de la conexión física para evitar fugas en el pool
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }

            return TablaDatos;
        }

        /*
         * Nombre: Oskar Saul Cermeño Jimenez
         * Carnet: 0901-23-15379
         * Fecha: 14/09/2026
         * Descripción: Verifica de forma preventiva si ya existe un registro con las llaves primarias simples o compuestas dadas.
         */
        public bool NavegadorFuncExisteLlavePrimaria(string NombreTabla, string[] CamposPK, string[] ValoresPK)
        {
            // Si no se proporcionaron campos de llave primaria, no procede la búsqueda
            if (CamposPK == null || CamposPK.Length == 0) return false;

            // Comprueba que la cantidad de columnas coincida con la cantidad de valores proporcionados
            if (ValoresPK == null || ValoresPK.Length != CamposPK.Length) return false;

            // Sanitiza el nombre de la tabla
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            string Condiciones = "";

            // Ensambla la condición WHERE dinámicamente uniendo los campos clave con el operador AND y parámetros '?'
            for (int Indice = 0; Indice < CamposPK.Length; Indice++)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(CamposPK[Indice]);

                if (Indice > 0) Condiciones += " AND ";
                Condiciones += CamposPK[Indice] + " = ?";
            }

            // Define la instrucción de conteo condicional sobre la tabla
            string ConsultaSQL = "SELECT COUNT(*) FROM " + NombreTabla + " WHERE " + Condiciones;
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, NavegadorFuncConexion))
                {
                    // Asocia secuencialmente los valores de las claves primarias como parámetros tipificados
                    for (int Indice = 0; Indice < ValoresPK.Length; Indice++)
                        Comando.Parameters.AddWithValue("@p" + Indice, ValoresPK[Indice]);

                    // Obtiene la cantidad de coincidencias y retorna true si existe al menos un registro
                    int Cantidad = Convert.ToInt32(Comando.ExecuteScalar());
                    return Cantidad > 0;
                }
            }
            finally
            {
                // Cierre seguro de la conexión ODBC
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

        /*
         * Nombre: Oskar Saul Cermeño Jimenez
         * Carnet: 0901-23-15379
         * Fecha: 14/09/2026
         * Descripción: Comprueba si existe un valor específico dentro de una columna determinada de la tabla.
         */
        public bool NavegadorFuncExisteValorCampo(string NombreTabla, string NombreCampo, string Valor)
        {
            // Valida los identificadores de la tabla y de la columna
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreCampo);

            // Prepara la consulta parametrizada para el conteo de coincidencias
            string ConsultaSQL = "SELECT COUNT(*) FROM " + NombreTabla + " WHERE " + NombreCampo + " = ?";
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, NavegadorFuncConexion))
                {
                    // Asigna el valor buscado al parámetro de la consulta
                    Comando.Parameters.AddWithValue("@valor", Valor);

                    // Ejecuta la consulta escalar y comprueba si el valor ya existe
                    int Cantidad = Convert.ToInt32(Comando.ExecuteScalar());
                    return Cantidad > 0;
                }
            }
            finally
            {
                // Cierre de conexión asegurado
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

        /*
         * Nombre: Oskar Saul Cermeño Jimenez
         * Carnet: 0901-23-15379
         * Fecha: 14/09/2026
         * Descripción: Genera y ejecuta dinámicamente una sentencia INSERT parametrizada a partir de un diccionario de datos.
         */
        public bool NavegadorFuncInsertarRegistro(string NombreTabla, Dictionary<string, string> Datos)
        {
            // Valida que el conjunto de datos a insertar contenga elementos
            if (Datos == null || Datos.Count == 0) return false;

            // Sanitiza el nombre de la tabla
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            string Columnas = "";
            string Valores = "";
            int Contador = 0;

            // Recorre el diccionario construyendo la lista de columnas y los marcadores de posición '?'
            foreach (KeyValuePair<string, string> Dato in Datos)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(Dato.Key);

                if (Contador > 0) { Columnas += ", "; Valores += ", "; }

                Columnas += Dato.Key;
                Valores += "?";
                Contador++;
            }

            // Ensambla la sentencia SQL INSERT INTO final
            string ConsultaSQL = "INSERT INTO " + NombreTabla + " (" + Columnas + ") VALUES (" + Valores + ")";
            OdbcConnection NavegadorFuncConexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, NavegadorFuncConexion))
                {
                    int Pos = 0;

                    // Asigna ordenadamente cada valor del diccionario a la colección de parámetros del comando
                    foreach (KeyValuePair<string, string> Dato in Datos)
                    {
                        Comando.Parameters.AddWithValue("@p" + Pos, Dato.Value);
                        Pos++;
                    }

                    // Ejecuta la inserción y retorna true si se insertó al menos un registro
                    return Comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                // Libera y desconecta la sesión de base de datos
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

        /*
         * Nombre: Oskar Saul Cermeño Jimenez
         * Carnet: 0901-23-15379
         * Fecha: 14/09/2026
         * Descripción: Construye y ejecuta dinámicamente una sentencia UPDATE separando datos a modificar y llaves del WHERE.
         */
        public bool NavegadorFuncActualizarRegistro(string NombreTabla, Dictionary<string, string> Valores, Dictionary<string, string> ClavesPrimarias)
        {
            // Valida que existan tanto valores a actualizar como identificadores de clave primaria
            if (Valores == null || Valores.Count == 0 || ClavesPrimarias == null || ClavesPrimarias.Count == 0)
                return false;

            // Sanitiza el identificador de la tabla
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            // Diccionario auxiliar para aislar las columnas modificables excluyendo las claves primarias
            Dictionary<string, string> ValoresActualizar = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (KeyValuePair<string, string> Dato in Valores)
            {
                ClsValidaciones.NavegadorMetValidarIdentificador(Dato.Key);

                // Excluye las columnas de llave primaria de la cláusula SET
                if (!ClavesPrimarias.ContainsKey(Dato.Key))
                    ValoresActualizar[Dato.Key] = Dato.Value;
            }

            // Si no quedan columnas para actualizar, cancela la ejecución
            if (ValoresActualizar.Count == 0) return false;

            StringBuilder Sql = new StringBuilder("UPDATE " + NombreTabla + " SET ");
            int Indice = 0;

            // Construye los asignadores columna = ? para la sección SET
            foreach (KeyValuePair<string, string> Dato in ValoresActualizar)
            {
                if (Indice > 0) Sql.Append(", ");
                Sql.Append(Dato.Key + " = ?");
                Indice++;
            }

            Sql.Append(" WHERE ");
            Indice = 0;

            // Construye los filtros de búsqueda clave = ? para la sección WHERE
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
                    // Asocia primero los parámetros correspondientes a la sección SET
                    foreach (KeyValuePair<string, string> Dato in ValoresActualizar)
                        Comando.Parameters.AddWithValue("@valor_" + Dato.Key, Dato.Value);

                    // Asocia posteriormente los parámetros correspondientes a la sección WHERE respetando el orden posicional
                    foreach (KeyValuePair<string, string> Clave in ClavesPrimarias)
                        Comando.Parameters.AddWithValue("@pk_" + Clave.Key, Clave.Value);

                    // Ejecuta la actualización y comprueba si afectó registros
                    return Comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                // Cierre obligatorio de la conexión
                _ConexionBD.NavegadorMetDesconexion(NavegadorFuncConexion);
            }
        }

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
    }
}