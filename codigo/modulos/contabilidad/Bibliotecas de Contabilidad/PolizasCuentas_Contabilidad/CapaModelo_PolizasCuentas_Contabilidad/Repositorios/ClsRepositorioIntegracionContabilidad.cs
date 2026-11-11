/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Consultas que necesita la recepción de pólizas de otros módulos:
 *  el módulo por su código, las cuentas que tiene asignadas y la
 *  póliza ya registrada para un documento de origen.
 * Reglas especificas:
 *  Solo se devuelven cuentas de detalle activas asignadas al módulo en
 *  tblModuloCuentaPais. La búsqueda por documento incluye pólizas
 *  anuladas para no aceptar dos veces el mismo documento.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Contabilidad.Entidades;

namespace CapaModelo_Contabilidad.Repositorios
{
    public class ClsRepositorioIntegracionContabilidad : ClsRepositorioMaestroContabilidad
    {
        private const string _SelectModulo =
            "SELECT idModuloContable, metodoActualizacionModuloContable FROM tblModuloContable WHERE codigoModuloContable = ? AND is_active = TRUE";

        private const string _SelectCuentasPermitidas =
            "SELECT DISTINCT c.idCuenta, c.codigoCuenta, c.nombreCuenta, c.tipoCuenta FROM tblModuloCuentaPais mc " +
            "INNER JOIN tblCuenta c ON c.idCuenta = mc.idCuenta WHERE mc.idModuloContable = ? AND mc.is_active = TRUE " +
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            "AND (? = 0 OR mc.idPais = ?) " +
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            "AND c.is_active = TRUE AND c.clasificacionCuenta = 'DETALLE' ORDER BY c.codigoCuenta";

        private const string _SelectPorDocumento =
            "SELECT idPolizaEncabezado, codigoPolizaEncabezado, estadoPolizaEncabezado, periodoContablePolizaEncabezado FROM tblPolizaEncabezado " +
            "WHERE idModuloContable = ? AND documentoOrigenPolizaEncabezado = ?";

        public bool ContabilidadFuncObtenerModulo(string Codigo, out int IdModulo, out string Metodo)
        {
            IdModulo = 0;
            Metodo = string.Empty;
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectModulo, CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Codigo", Codigo) }))
            {
                if (Tabla.Rows.Count == 0) return false;
                IdModulo = Convert.ToInt32(Tabla.Rows[0][0]);
                Metodo = Convert.ToString(Tabla.Rows[0][1]);
                return true;
            }
        }

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        public List<ClsCuentaContabilidad> ContabilidadFuncObtenerCuentasPermitidas(int IdModulo, int IdPais)
        {
            List<ClsCuentaContabilidad> Cuentas = new List<ClsCuentaContabilidad>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectCuentasPermitidas, CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Modulo", IdModulo), new OdbcParameter("Pais", IdPais), new OdbcParameter("PaisFiltro", IdPais) }))
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                foreach (DataRow Fila in Tabla.Rows)
                    Cuentas.Add(new ClsCuentaContabilidad
                    {
                        IdCuenta = Convert.ToInt32(Fila["idCuenta"]),
                        Codigo = Convert.ToString(Fila["codigoCuenta"]),
                        Nombre = Convert.ToString(Fila["nombreCuenta"]),
                        Tipo = Convert.ToString(Fila["tipoCuenta"]),
                        Activo = true
                    });
            return Cuentas;
        }

        public ClsPolizaContabilidad ContabilidadFuncObtenerPorDocumento(int IdModulo, string Documento)
        {
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectPorDocumento, CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Modulo", IdModulo), new OdbcParameter("Documento", Documento) }))
            {
                if (Tabla.Rows.Count == 0) return null;
                return new ClsPolizaContabilidad
                {
                    IdPoliza = Convert.ToInt32(Tabla.Rows[0]["idPolizaEncabezado"]),
                    CodigoPoliza = Convert.ToString(Tabla.Rows[0]["codigoPolizaEncabezado"]),
                    Estado = Convert.ToString(Tabla.Rows[0]["estadoPolizaEncabezado"]),
                    PeriodoContable = Convert.ToString(Tabla.Rows[0]["periodoContablePolizaEncabezado"]),
                    IdModulo = IdModulo,
                    DocumentoOrigen = Documento
                };
            }
        }
    }
}
