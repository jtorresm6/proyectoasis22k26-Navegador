using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta el identificador del módulo de Compras registrado en Seguridad.
 * ===================================================================
*/

namespace CapaModelo_Componentes.Repositorios
{
    public sealed class ClsRepositorioSeguridadCompras : ClsRepositorioMaestroCompras
    {
        public int ComprasFuncObtenerModulo()
        {
            using (var Datos = ComprasFuncEjecucionConsulta(
                "SELECT idModulo FROM tblmodulo WHERE nombreModulo = ? AND is_active = 1", CommandType.Text,
                new List<OdbcParameter>
                {
                    new OdbcParameter("NombreModulo", OdbcType.VarChar, 100) { Value = "Compras y Cuentas por Pagar" }
                }))
            {
                if (Datos.Rows.Count == 0) throw new InvalidOperationException("El módulo de Compras no está registrado en Seguridad.");
                if (Datos.Rows.Count != 1) throw new InvalidOperationException("El registro del módulo de Compras está duplicado.");
                return Convert.ToInt32(Datos.Rows[0]["idModulo"]);
            }
        }
    }
}
