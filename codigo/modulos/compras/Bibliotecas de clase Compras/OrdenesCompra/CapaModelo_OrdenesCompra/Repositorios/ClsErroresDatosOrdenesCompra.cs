using System;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Traduce errores ODBC en mensajes comprensibles para el usuario.
 * ===================================================================
*/

namespace CapaModelo_OrdenesCompra.Repositorios
{
    internal static class ClsErroresDatosOrdenesCompra
    {

        internal static Exception ComprasFuncTraducirError(OdbcException Error)
        {
            string Mensaje = "No se pudo completar la operación. Verifica la conexión configurada en Seguridad.";
            foreach (OdbcError Detalle in Error.Errors)
            {
                if (Detalle.SQLState == "IM002")
                {
                    Mensaje = "No se encuentra el DSN EmbutidosS.A para esta aplicación. Verifica el origen de datos ODBC de 64 bits (IM002).";
                    continue;
                }
                switch (Detalle.NativeError)
                {
                    case 1045:
                        Mensaje = "MySQL rechazó las credenciales del DSN EmbutidosS.A. Verifica el usuario y la contraseña en ODBC de 64 bits (error 1045).";
                        break;
                    case 1049:
                        Mensaje = "La base configurada en el DSN EmbutidosS.A no existe en el servidor seleccionado (error 1049).";
                        break;
                    case 1046:
                        Mensaje = "El DSN EmbutidosS.A no tiene una base de datos seleccionada (error 1046).";
                        break;
                    case 1054:
                        Mensaje = "Falta una columna del esquema de Compras en la base conectada (error 1054). " + Detalle.Message;
                        break;
                    case 1146:
                        Mensaje = "Falta una tabla del esquema de Compras en la base conectada (error 1146). " + Detalle.Message;
                        break;
                    case 1451:
                    case 1452:
                        Mensaje = "La orden tiene relaciones pendientes o una referencia ya no existe.";
                        break;
                    case 1062:
                        Mensaje = "Ya existe una orden de compra con ese código.";
                        break;
                    case 1406:
                        Mensaje = "Uno de los campos excede la longitud permitida.";
                        break;
                    case 3819:
                    case 4025:
                    case 1644:
                        Mensaje = "La operación no cumple las reglas de integridad de la base de datos.";
                        break;
                }
            }
            return new InvalidOperationException(Mensaje, Error);
        }
    }
}
