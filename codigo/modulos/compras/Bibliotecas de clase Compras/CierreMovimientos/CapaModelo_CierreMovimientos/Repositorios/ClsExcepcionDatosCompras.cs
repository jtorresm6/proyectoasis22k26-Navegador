using System;
using System.Data.Odbc;

namespace CapaModelo_CierreMovimientos.Repositorios
{
    [Serializable]
    public sealed class ClsExcepcionDatosCompras : Exception
    {
        public int Codigo { get; }
        public ClsExcepcionDatosCompras() : base() { }

        public ClsExcepcionDatosCompras(string mensaje) : base(mensaje) { }

        public ClsExcepcionDatosCompras(string mensaje, Exception interna) : base(mensaje, interna) { }
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