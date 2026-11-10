/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 30/10/2026
 * ==================================================================
 * Propósito :
 *  Lee los impuestos activos de tblImpuesto con el código de sus
 *  cuentas contables y les agrega sus tramos de tblImpuestoTramo.
 * Reglas especificas:
 *  Valor y tasa vienen como fracción (0.12 es 12 %). Los tramos van
 *  ordenados por el monto desde el que aplican.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Contabilidad.Entidades;

namespace CapaModelo_Contabilidad.Repositorios
{
    public class ClsRepositorioImpuestosContabilidad : ClsRepositorioMaestroContabilidad
    {
        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 02/11/2026
        private const string _SelectImpuestos =
            "SELECT i.idImpuesto, i.nombreImpuesto, i.descripcionImpuesto, i.tipoImpuesto, i.operacionImpuesto, i.valorImpuesto, " +
            "i.periodicidadImpuesto, ca.codigoCuenta AS cuentaAcreditable, cp.codigoCuenta AS cuentaPorPagar FROM tblImpuesto i " +
            "LEFT JOIN tblCuenta ca ON ca.idCuenta = i.idCuentaAcreditableImpuesto " +
            "LEFT JOIN tblCuenta cp ON cp.idCuenta = i.idCuentaPorPagarImpuesto " +
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026
            "ORDER BY i.idImpuesto";

        private const string _SelectTramos =
            "SELECT idImpuesto, montoDesdeImpuestoTramo, montoHastaImpuestoTramo, tasaImpuestoTramo, importeFijoImpuestoTramo " +
            "FROM tblImpuestoTramo ORDER BY idImpuesto, montoDesdeImpuestoTramo";
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 09/11/2026

        public List<ClsImpuestoContabilidad> ContabilidadFuncObtenerImpuestos()
        {
            Dictionary<int, ClsImpuestoContabilidad> Impuestos = new Dictionary<int, ClsImpuestoContabilidad>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectImpuestos, CommandType.Text))
                foreach (DataRow Fila in Tabla.Rows)
                {
                    ClsImpuestoContabilidad Impuesto = new ClsImpuestoContabilidad
                    {
                        IdImpuesto = Convert.ToInt32(Fila["idImpuesto"]),
                        Nombre = Convert.ToString(Fila["nombreImpuesto"]),
                        Descripcion = Fila["descripcionImpuesto"] == DBNull.Value ? null : Convert.ToString(Fila["descripcionImpuesto"]),
                        Tipo = Convert.ToString(Fila["tipoImpuesto"]),
                        Operacion = Convert.ToString(Fila["operacionImpuesto"]),
                        Valor = Convert.ToDecimal(Fila["valorImpuesto"]),
                        Periodicidad = Convert.ToString(Fila["periodicidadImpuesto"]),
                        CuentaAcreditable = Fila["cuentaAcreditable"] == DBNull.Value ? null : Convert.ToString(Fila["cuentaAcreditable"]),
                        CuentaPorPagar = Fila["cuentaPorPagar"] == DBNull.Value ? null : Convert.ToString(Fila["cuentaPorPagar"])
                    };
                    Impuestos[Impuesto.IdImpuesto] = Impuesto;
                }
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectTramos, CommandType.Text))
                foreach (DataRow Fila in Tabla.Rows)
                {
                    int IdImpuesto = Convert.ToInt32(Fila["idImpuesto"]);
                    if (!Impuestos.ContainsKey(IdImpuesto)) continue;
                    Impuestos[IdImpuesto].Tramos.Add(new ClsImpuestoTramoContabilidad
                    {
                        MontoDesde = Convert.ToDecimal(Fila["montoDesdeImpuestoTramo"]),
                        MontoHasta = Fila["montoHastaImpuestoTramo"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(Fila["montoHastaImpuestoTramo"]),
                        Tasa = Convert.ToDecimal(Fila["tasaImpuestoTramo"]),
                        ImporteFijo = Convert.ToDecimal(Fila["importeFijoImpuestoTramo"])
                    });
                }
            return new List<ClsImpuestoContabilidad>(Impuestos.Values);
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 02/11/2026
    }
}
