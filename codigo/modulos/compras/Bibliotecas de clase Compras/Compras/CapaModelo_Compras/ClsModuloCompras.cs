using System;
using CapaModelo_Componentes.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Obtiene la configuración del módulo de Compras mediante el repositorio compartido y comunica errores de consulta.
 * ===================================================================
*/

namespace CapaModelo_Compras
{
    public static class ClsModuloCompras
    {
        public static int ComprasFuncObtenerIdModulo()
        {
            try { return new ClsRepositorioSeguridadCompras().ComprasFuncObtenerModulo(); }
            catch (ClsExcepcionDatosCompras Error)
            {
                throw new InvalidOperationException("No se pudo consultar la configuración de Seguridad.", Error);
            }
        }

    }
}
