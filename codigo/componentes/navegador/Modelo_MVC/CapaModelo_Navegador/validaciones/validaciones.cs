using System;
using System.Text.RegularExpressions;

namespace CapaModelo_Navegador
{
    // Evita que se cuele algo raro dentro de un nombre de tabla o columna
    public static class validaciones
    {
        public static void ValidarIdentificador(string identificador)
        {
            if (string.IsNullOrWhiteSpace(identificador) || !Regex.IsMatch(identificador, @"^[A-Za-z0-9_$.]+$"))
                throw new ArgumentException("El nombre de tabla o columna no es válido.");
        }
    }
}