using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Miguel David Contreras Jacinto
 * Carné : 0901-21-3878
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta los datos de proveedores mediante sentencias parametrizadas para los selectores de Compras.
 * ===================================================================
*/

namespace CapaModelo_Componentes.Repositorios
{
    public class ClsRepositorioCombo : ClsRepositorioMaestroCompras
    {


        public DataTable ComprasFuncObtenerProveedores()
        {
            return ComprasFuncEjecucionConsulta("SELECT * FROM tblProveedor ORDER BY nombreProveedor", CommandType.Text);
        }

        public DataTable ComprasFuncObtenerProveedor(int IdProveedor)
        {
            if (IdProveedor <= 0) throw new ArgumentOutOfRangeException(nameof(IdProveedor));
            return ComprasFuncEjecucionConsulta("SELECT * FROM tblProveedor WHERE idProveedor = ?", CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("IdProveedor", OdbcType.Int) { Value = IdProveedor } });
        }

        public DataTable ComprasFuncObtenerDatos(string Tabla, string Campo1, string Campo2)
        {

            var CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "idProveedor", "nombreProveedor", "nitProveedor", "telefonoProveedor",
                "correoProveedor", "direccionProveedor", "estadoProveedor", "created_at", "updated_at"
            };
            if (!string.Equals(Tabla, "tblProveedor", StringComparison.OrdinalIgnoreCase) ||
                !CamposPermitidos.Contains(Campo1 ?? string.Empty) || !CamposPermitidos.Contains(Campo2 ?? string.Empty))
                throw new ArgumentException("La tabla o los campos solicitados no están permitidos para el selector de Compras.");
            return ComprasFuncEjecucionConsulta("SELECT `" + Campo1 + "`, `" + Campo2 + "` FROM tblProveedor", CommandType.Text);
        }
    }
}
