/*
 * Autor: Julio Roberto Rosales Mejía.
 * Carné: 0901-23-1426
 * Creación de clase "ClsConexionBD.cs"
 * Documentación Interna del código.
 */

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Navegador
{
    //Clase encargada de administrar la conexión entre el componente
    //Navegaor y la base de datos.
    public class ClsConexionBD
    {
        //Crea y abre una conexión con la base de datos mediante ODBC (Abrir ODBC con la bases de datos).
        //El método devuelve el objeot de conexión para que otras clases puedan utilizarlo para realizar
        //Operaciones sobre la misma base de datos. 

        public OdbcConnection NavegadorFuncConexion()
        {
            //Se crea la conexión utilizando el DSN configurado para la base de datos.
            //El valor entre corchetes debe sustituirse por el nombre correspondiente de la base de datos.
            
            // Inicio cambio - Gabriel André Guillén Pocón - 0901-23-1998
            // Se apunta al DSN "EmbutidosS.A", que es el mismo que usa el componente Seguridad
            // (base de datos dbSistemaEmbutidos), para poder leer los permisos reales de los usuarios.
            OdbcConnection Conexion = new OdbcConnection("Dsn=EmbutidosS.A");
            // Fin cambio - Gabriel André Guillén Pocón - 0901-23-1998

            try
            {
                //Intenta abrir la conexión con la base de datos.
                Conexion.Open();
            }
            catch (OdbcException)
            {
                // Si ocurre un error relacionado con la conexión ODBC,
                // Se informa del problema medainte un mensaje de consola (cambiar por un mensaje en pantalla).
                
                Console.WriteLine("Error al conectar a la base de datos");
            }

           //Se devuleve la conexión creada para que pueda ser utilizada
           //Por las clases que necesiten acceder a la base de datos.
            
            return Conexion;
        }

        //Cierra una conexión existente con la base de datos.
        //Recibe como parámetro la conexión que se desea cerrar.

        public void NavegadorMetDesconexion(OdbcConnection Conexion)
        {
            try
            {
                // Primero se verifica qque la conexión exista y que no
                // Se encuentre cerrada antes de intentar cerrarla.
                
                if (Conexion != null && Conexion.State != System.Data.ConnectionState.Closed)
                {
                    // Se cierra la conexión para liberar el recurso
                    // Utilizado por la comunicación con la base de datos.
                    Conexion.Close();
                }
            }
            catch (OdbcException)
            {
                // Si ocurre un error durante el cierre de la conexión, 
                // Se muestar un mensaje indicando el problema (Cambiar por un meensaje de error en pantalla).
                
                Console.WriteLine("Error al desconectar de la base de datos");
            }
        }
    }

}


/*
 *
 *FIN DEL PROGRAMA
 *AUTOR: Julio Roberto Rosales Mejía
 *Carné: 0901-23-1426
 *
 **/