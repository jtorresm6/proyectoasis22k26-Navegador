/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Lee el catálogo de cuentas con su cuenta padre y los montos de las
 *  pólizas aplicadas de un país para armar los estados financieros.
 *  También lee los países con su moneda y los tipos de cambio que
 *  registra Bancos.
 * Reglas especificas:
 *  El saldo es el acumulado de las pólizas aplicadas hasta el período
 *  pedido. Cargo y abono suman los períodos del rango. El efectivo son
 *  las cuentas de detalle de Caja (1.01.01) y Bancos (1.01.02); el
 *  flujo se toma de la contrapartida de cada póliza. Los montos salen
 *  en la moneda del país de la póliza.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Contabilidad.Entidades;

namespace CapaModelo_Contabilidad.Repositorios
{
    public class ClsRepositorioEstadosFinancierosContabilidad : ClsRepositorioMaestroContabilidad
    {
        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 29/10/2026
        private const string _CondicionPoliza =
            "pe.estadoPolizaEncabezado = 'APLICADA' AND pe.is_active = TRUE AND pd.is_active = TRUE AND pe.idPais = ? ";

        private const string _SelectCuentas =
            "SELECT c.idCuenta, c.idCuentaPadre, c.codigoCuenta, c.nombreCuenta, c.tipoCuenta, c.clasificacionCuenta, " +
            "COALESCE(CASE WHEN c.tipoCuenta = 'DEUDORA' THEN m.debe - m.haber ELSE m.haber - m.debe END, 0) AS saldo, " +
            "COALESCE(m.cargo, 0) AS cargo, COALESCE(m.abono, 0) AS abono " +
            "FROM tblCuenta c LEFT JOIN (SELECT pd.idCuenta, SUM(pd.debePolizaDetalle) AS debe, SUM(pd.haberPolizaDetalle) AS haber, " +
            "SUM(CASE WHEN pe.periodoContablePolizaEncabezado >= ? THEN pd.debePolizaDetalle ELSE 0 END) AS cargo, " +
            "SUM(CASE WHEN pe.periodoContablePolizaEncabezado >= ? THEN pd.haberPolizaDetalle ELSE 0 END) AS abono " +
            "FROM tblPolizaDetalle pd INNER JOIN tblPolizaEncabezado pe ON pe.idPolizaEncabezado = pd.idPolizaEncabezado " +
            "WHERE " + _CondicionPoliza + "AND pe.periodoContablePolizaEncabezado <= ? GROUP BY pd.idCuenta) m ON m.idCuenta = c.idCuenta " +
            "WHERE c.is_active = TRUE ORDER BY c.codigoCuenta";
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 29/10/2026

        private const string _CondicionEfectivo = "(ce.codigoCuenta LIKE '1.01.01.%' OR ce.codigoCuenta LIKE '1.01.02.%')";

        private const string _SelectFlujo =
            "SELECT pd.idCuenta, SUM(pd.haberPolizaDetalle - pd.debePolizaDetalle) FROM tblPolizaDetalle pd " +
            "INNER JOIN tblPolizaEncabezado pe ON pe.idPolizaEncabezado = pd.idPolizaEncabezado " +
            "INNER JOIN tblCuenta c ON c.idCuenta = pd.idCuenta " +
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 29/10/2026
            "WHERE " + _CondicionPoliza +
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 29/10/2026
            "AND pe.fechaPolizaEncabezado BETWEEN ? AND ? AND NOT (c.codigoCuenta LIKE '1.01.01.%' OR c.codigoCuenta LIKE '1.01.02.%') " +
            "AND EXISTS (SELECT 1 FROM tblPolizaDetalle e INNER JOIN tblCuenta ce ON ce.idCuenta = e.idCuenta " +
            "WHERE e.idPolizaEncabezado = pe.idPolizaEncabezado AND e.is_active = TRUE AND " + _CondicionEfectivo + ") " +
            "GROUP BY pd.idCuenta";

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 29/10/2026
        private const string _SelectEfectivo =
            "SELECT SUM(pd.debePolizaDetalle - pd.haberPolizaDetalle) FROM tblPolizaDetalle pd " +
            "INNER JOIN tblPolizaEncabezado pe ON pe.idPolizaEncabezado = pd.idPolizaEncabezado " +
            "INNER JOIN tblCuenta ce ON ce.idCuenta = pd.idCuenta " +
            "WHERE " + _CondicionPoliza + "AND pe.periodoContablePolizaEncabezado <= ? AND " + _CondicionEfectivo;

        private const string _SelectPaises =
            "SELECT idPais, nombrePais, codigoMonedaPais FROM tblPais WHERE is_active = TRUE ORDER BY idPais";

        private const string _SelectTiposCambio =
            "SELECT mo.codigoMoneda, md.codigoMoneda, t.valorTipoCambio FROM tblTipoCambio t " +
            "INNER JOIN tblMoneda mo ON mo.Pk_Moneda = t.Fk_Moneda_Origen_Codigo " +
            "INNER JOIN tblMoneda md ON md.Pk_Moneda = t.Fk_Moneda_Destino_Codigo " +
            "WHERE t.fechaTipoCambio <= ? ORDER BY t.fechaTipoCambio DESC, t.versionTipoCambio DESC, t.Pk_TipoCambio DESC";

        public List<ClsCuentaSaldoContabilidad> ContabilidadFuncObtenerCuentas(string Desde, string Hasta, int IdPais)
        {
            List<ClsCuentaSaldoContabilidad> Cuentas = new List<ClsCuentaSaldoContabilidad>();
            List<OdbcParameter> Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("DesdeCargo", Desde), new OdbcParameter("DesdeAbono", Desde),
                new OdbcParameter("Pais", IdPais), new OdbcParameter("Hasta", Hasta)
            };
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 29/10/2026
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectCuentas, CommandType.Text, Parametros))
                foreach (DataRow Fila in Tabla.Rows)
                    Cuentas.Add(new ClsCuentaSaldoContabilidad
                    {
                        IdCuenta = Convert.ToInt32(Fila["idCuenta"]),
                        IdCuentaPadre = Fila["idCuentaPadre"] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila["idCuentaPadre"]),
                        Codigo = Convert.ToString(Fila["codigoCuenta"]),
                        Nombre = Convert.ToString(Fila["nombreCuenta"]),
                        Tipo = Convert.ToString(Fila["tipoCuenta"]),
                        Clasificacion = Convert.ToString(Fila["clasificacionCuenta"]),
                        SaldoFinal = Convert.ToDecimal(Fila["saldo"]),
                        Cargo = Convert.ToDecimal(Fila["cargo"]),
                        Abono = Convert.ToDecimal(Fila["abono"])
                    });
            return Cuentas;
        }

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 29/10/2026
        public Dictionary<int, decimal> ContabilidadFuncObtenerFlujoEfectivo(DateTime Desde, DateTime Hasta, int IdPais)
        {
            Dictionary<int, decimal> Flujo = new Dictionary<int, decimal>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectFlujo, CommandType.Text, new List<OdbcParameter>
                {
                    new OdbcParameter("Pais", IdPais), new OdbcParameter("Desde", OdbcType.Date) { Value = Desde.Date },
                    new OdbcParameter("Hasta", OdbcType.Date) { Value = Hasta.Date }
                }))
                foreach (DataRow Fila in Tabla.Rows) Flujo[Convert.ToInt32(Fila[0])] = Convert.ToDecimal(Fila[1]);
            return Flujo;
        }

        public decimal ContabilidadFuncObtenerEfectivo(string Periodo, int IdPais)
        {
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectEfectivo, CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Pais", IdPais), new OdbcParameter("Periodo", Periodo) }))
                return Tabla.Rows.Count == 0 || Tabla.Rows[0][0] == DBNull.Value ? 0m : Convert.ToDecimal(Tabla.Rows[0][0]);
        }

        public DataTable ContabilidadFuncObtenerPaises()
        {
            return ContabilidadFuncEjecutarConsulta(_SelectPaises, CommandType.Text);
        }

        public Dictionary<string, decimal> ContabilidadFuncObtenerTiposCambio(DateTime Fecha)
        {
            Dictionary<string, decimal> Tipos = new Dictionary<string, decimal>();
            using (DataTable Tabla = ContabilidadFuncEjecutarConsulta(_SelectTiposCambio, CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date } }))
                foreach (DataRow Fila in Tabla.Rows)
                {
                    string Par = Convert.ToString(Fila[0]) + ">" + Convert.ToString(Fila[1]);
                    if (!Tipos.ContainsKey(Par)) Tipos[Par] = Convert.ToDecimal(Fila[2]);
                }
            return Tipos;
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 29/10/2026
    }
}
