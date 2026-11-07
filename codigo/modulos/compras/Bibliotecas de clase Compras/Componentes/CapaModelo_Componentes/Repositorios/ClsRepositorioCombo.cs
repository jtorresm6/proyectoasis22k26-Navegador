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
            string TablaPermitida;
            if (string.Equals(Tabla, "tblProveedor", StringComparison.OrdinalIgnoreCase))
                TablaPermitida = "tblProveedor";
            else if (string.Equals(Tabla, "tblMetodoPago", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblMetodoPago";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idMetodoPago", "nombreMetodoPago" };
            }
            else if (string.Equals(Tabla, "tblPlazo", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblPlazo";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idPlazo", "nombrePlazo" };
            }
            else if (string.Equals(Tabla, "tblConceptoProveedor", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblConceptoProveedor";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idConceptoProveedor", "nombreConceptoProveedor" };
            }
            else if (string.Equals(Tabla, "tblMoneda", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblMoneda";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idMoneda", "nombreMoneda" };
            }
            else if (string.Equals(Tabla, "tblTipoCambio", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblTipoCambio";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idTipoCambio", "descripcionTipoCambio" };
            }
            else if (string.Equals(Tabla, "tblSucursal", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblSucursal";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idSucursal", "nombreSucursal" };
            }
            else if (string.Equals(Tabla, "tblBodega", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblBodega";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idBodega", "nombreBodega" };
            }
            else if (string.Equals(Tabla, "tblCategoria", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblCategoria";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idCategoria", "nombreCategoria" };
            }
            else if (string.Equals(Tabla, "tblLinea", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblLinea";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idLinea", "nombreLinea" };
            }
            else if (string.Equals(Tabla, "tblMarca", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblMarca";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idMarca", "nombreMarca" };
            }
            else if (string.Equals(Tabla, "tblProducto", StringComparison.OrdinalIgnoreCase))
            {
                TablaPermitida = "tblProducto";
                CamposPermitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "idProducto", "nombreProducto", "skuProducto" };
            }
            else
                throw new ArgumentException("La tabla solicitada no esta permitida para el selector de Compras.");

            if (
                !CamposPermitidos.Contains(Campo1 ?? string.Empty) || !CamposPermitidos.Contains(Campo2 ?? string.Empty))
                throw new ArgumentException("La tabla o los campos solicitados no están permitidos para el selector de Compras.");
            if (TablaPermitida == "tblMoneda")
                return ComprasFuncEjecucionConsulta("SELECT * FROM tblMoneda ORDER BY nombreMoneda", CommandType.Text);
            if (TablaPermitida == "tblTipoCambio")
                return ComprasFuncEjecucionConsulta(
                    "SELECT tc.idTipoCambio, CONCAT(origen.codigoMoneda, ' / ', destino.codigoMoneda, ' - ', " +
                    "tc.fechaTipoCambio, ' - ', tc.valorTipoCambio, ' - ', tc.fuenteTipoCambio, " +
                    "' - v', tc.versionTipoCambio) AS descripcionTipoCambio " +
                    "FROM tblTipoCambio tc " +
                    "INNER JOIN tblMoneda origen ON origen.idMoneda = tc.Fk_Moneda_Origen_Codigo " +
                    "INNER JOIN tblMoneda destino ON destino.idMoneda = tc.Fk_Moneda_Destino_Codigo " +
                    "ORDER BY tc.fechaTipoCambio DESC, tc.idTipoCambio DESC", CommandType.Text);
            return ComprasFuncEjecucionConsulta("SELECT `" + Campo1 + "`, `" + Campo2 + "` FROM `" + TablaPermitida + "`", CommandType.Text);
        }
    }
}
