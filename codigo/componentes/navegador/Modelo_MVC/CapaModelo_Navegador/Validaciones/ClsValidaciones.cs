/*
 * Nombre: Gabriel André Guillén Pocón
 * Carnet: 0901-23-420 (placeholder)
 * Fecha: 16/09/2026
 * Descripción: Clase para validar nombres de identificadores como tablas o columnas.
 */

using System;
using System.Text.RegularExpressions;

namespace CapaModelo_Navegador
{
    // Esta clase sirve para evitar que se envíen caracteres raros en los nombres de las tablas o columnas
    public static class ClsValidaciones
    {
        // Método que se encarga de revisar que el identificador esté bien escrito
        public static void NavegadorMetValidarIdentificador(string Identificador)
        {
            // Primero, miramos que no esté vacío. Luego, con la expresión regular, nos aseguramos 
            // de que solo tenga letras, números y algunos caracteres permitidos.
            if (string.IsNullOrWhiteSpace(Identificador) || !Regex.IsMatch(Identificador, @"^[A-Za-z0-9_$.]+$"))
                // Si tiene algo raro o viene vacío, lanzamos un error para detener el proceso
                throw new ArgumentException("El nombre de tabla o Columna no es válido.");
        }
    }
}

/*
 * Fin de la clase ClsValidaciones
 * Nombre: Gabriel André Guillén Pocón
 * Fecha: 16/09/2026
 */