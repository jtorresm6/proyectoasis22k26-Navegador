using System;
using System.Windows.Forms;
using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;

namespace CapaVista_Navegador
{
    // Fachada de la bitácora de Seguridad para el componente Navegador.
    // Quien use el navegador (por ejemplo el CRUD) solo pide "registra esta acción" y no necesita
    // conocer las clases de Seguridad. El usuario que queda en la bitácora es el de la sesión activa.
    public class ClsNavegadorBitacora
    {
        private readonly ClsModeloBitacora _Bitacora = new ClsModeloBitacora();

        // Se usa el método de instancia con IdUsuario explícito porque el método estático
        // SeguridadMetRegistrarAccion usa otra clase de sesión (ClsSesion) fija en el usuario 1.
        // La IP se pasa en null y Seguridad la calcula sola.
        //
        // El registro de bitácora se hace contra la base de datos de Seguridad. Si no hay conexión no
        // debe reventar la acción que ya se guardó (Insertar/Modificar/Eliminar); se avisa con el
        // diálogo estándar de Error (X roja y Aceptar) y se continúa.
        public void NavegadorMetRegistrarBitacora(string Accion, string Tabla, int IdRegistro, string Detalles)
        {
            try
            {
                _Bitacora.SeguridadMetRegistrarBitacora(
                    ClsSesionSeguridad.IdUsuario, Accion, Tabla, IdRegistro, Detalles, null);
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(
                    "El registro se guardó, pero no se pudo registrar en la bitácora porque no hay conexión con la base de datos.\n\n" +
                    Excepcion.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
