// Inicio cambio - Mario Alberto Taracena Pérez - 0901-23-9335
// Archivo nuevo. Sirve para "simular" un login mientras Navegador todavía no tiene su propia
// pantalla de inicio de sesión.
using System.Collections.Generic;

namespace CapaControlador_Navegador
{
    public static class ClsSesionPrueba
    {
        // Propiedades locales para simular la sesión sin depender de DLLs externas
        public static int IdUsuario { get; private set; } = 1;
        public static string Usuario { get; private set; } = "admin";
        public static string NombreCompleto { get; private set; } = "Usuario Prueba";

        public enum UsuarioDemo
        {
            Administrador,
            Supervisor,
            Operativo
        }

        private const UsuarioDemo UsuarioPrueba = UsuarioDemo.Supervisor;

        public static void NavegadorMetIniciarSesionPrueba()
        {
            switch (UsuarioPrueba)
            {
                case UsuarioDemo.Supervisor:
                    IdUsuario = 2;
                    Usuario = "cramirez";
                    NombreCompleto = "Carlos Ramírez";
                    break;

                case UsuarioDemo.Operativo:
                    IdUsuario = 3;
                    Usuario = "mlopez";
                    NombreCompleto = "María López";
                    break;

                default:
                    IdUsuario = 1;
                    Usuario = "imelendez";
                    NombreCompleto = "Isabel Meléndez";
                    break;
            }
        }
    }
}
// Fin cambio - Mario Alberto Taracena Pérez - 0901-23-9335