using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Seguridad.Ayudas;


//-----------------------------------------------------
// - Hecho por: Natali Sofía Montenegro Portillo 
// - Ultima modificación: 14/09/2026
// - Carne: 0901-23-10017 
namespace CapaVista_Navegador
{
    // Inicio cambio - Gabriel André Guillén Pocón - 0901-23-1998
    // Antes esta clase tenía una validación propia que siempre decía que sí había acceso.
    // Ahora ya no valida nada por su cuenta: solo le pasa el Módulo y la Aplicación de este
    // formulario a ClsSeguridadFormHelper (que es del componente Seguridad) y deja que él
    // decida qué botones habilitar o deshabilitar.
    public class ClsCrudSeguridad
    {
        // Guardan a qué Módulo y Aplicación de Seguridad pertenece este formulario.
        private readonly int _IdModulo;
        private readonly int _IdAplicacion;

        // El constructor solo recibe y guarda esos dos números.
        public ClsCrudSeguridad(int IdModulo, int IdAplicacion)
        {
            _IdModulo = IdModulo;
            _IdAplicacion = IdAplicacion;
        }

        // Habilita/deshabilita los controles del mapa según los permisos del usuario en sesión
        // (ClsSesionSeguridad) para este Módulo/Aplicación. Si no tiene acceso, deshabilita todo el formulario.
        // MapaBotones dice qué botón corresponde a qué permiso (Insertar, Editar, Eliminar, Imprimir).
        //
        // La consulta de permisos se hace contra la base de datos de Seguridad. El estándar exige que
        // toda falla de conexión muestre el diálogo estándar de Error (ícono de X roja y botón Aceptar)
        // en vez de dejar que la excepción se propague y la aplicación reviente.
        public void NavegadorMetAplicarPermisos(Form Formulario, Dictionary<Control, TipoPermiso> MapaBotones)
        {
            try
            {
                ClsSeguridadFormHelper.SeguridadMetInicializarSeguridad(Formulario, _IdModulo, _IdAplicacion, MapaBotones);
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(
                    "No fue posible verificar los permisos de seguridad porque no hay conexión con la base de datos.\n\n" +
                    Excepcion.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
    // Fin cambio - Gabriel André Guillén Pocón - 0901-23-1998
}

//--------------------------------------------------
// - Final: Natali Sofía Montenegro Portillo 
// - Carne: 0901-23-10017 
// Clase que permite aplicar los permisos de Seguridad (Insertar/Editar/Eliminar/Imprimir)
// sobre los botones de un formulario CRUD de Navegador, delegando en ClsSeguridadFormHelper.