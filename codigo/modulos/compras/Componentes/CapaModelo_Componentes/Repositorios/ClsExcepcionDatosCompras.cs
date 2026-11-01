using System;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Representa los errores de acceso a datos y proporciona mensajes comprensibles para Compras.
 * ===================================================================
*/

namespace CapaModelo_Componentes.Repositorios
{
    public sealed class ClsExcepcionDatosCompras : Exception
    {
        public int Codigo { get; }

        private ClsExcepcionDatosCompras(int Codigo, string Mensaje) : base(Mensaje)
        {
            this.Codigo = Codigo;
        }

        internal static ClsExcepcionDatosCompras ComprasFuncTraducir(OdbcException Error)
        {
            foreach (OdbcError Detalle in Error.Errors)
            {
                switch (Detalle.NativeError)
                {
                    case 1062:
                        return new ClsExcepcionDatosCompras(1062, "Ya existe un registro con los mismos datos únicos.");
                    case 1305:
                        return new ClsExcepcionDatosCompras(1305, "Falta un procedimiento requerido en la base de datos. Solicite el script correspondiente.");
                    case 1451:
                    case 1452:
                        return new ClsExcepcionDatosCompras(Detalle.NativeError, "El registro tiene relaciones pendientes o una referencia seleccionada ya no existe.");
                    case 1406:
                        return new ClsExcepcionDatosCompras(1406, "Uno de los campos excede la longitud permitida.");
                    case 3819:
                        return new ClsExcepcionDatosCompras(3819, "La operación no cumple las reglas de integridad de la base de datos.");
                    case 1644:
                        string Mensaje = Detalle.Message;
                        int Separador = Mensaje.LastIndexOf(']');
                        if (Separador >= 0) Mensaje = Mensaje.Substring(Separador + 1).Trim();
                        return new ClsExcepcionDatosCompras(1644, string.IsNullOrWhiteSpace(Mensaje)
                            ? "La operación no cumple las reglas de integridad de la base de datos." : Mensaje);
                }
            }
            return new ClsExcepcionDatosCompras(0, "No se pudo completar la operación de datos. Verifique la conexión y vuelva a intentarlo.");
        }
    }
}
