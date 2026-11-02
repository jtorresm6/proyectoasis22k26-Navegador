using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Componentes.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 29/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta las monedas y los tipos de cambio que administra el módulo de Bancos
 *  (tblMoneda y tblTipoCambio) para las herramientas de moneda de Compras.
 *  Compras solo lee estas tablas: nunca crea ni modifica monedas o tipos de cambio.
 *  Las consultas viven en la capa Modelo, sin procedimientos almacenados.
 * ===================================================================
*/

namespace CapaModelo_Herramientas.Repositorios
{
    public class ClsRepositorioMoneda : ClsRepositorioMaestroCompras
    {
        private const string _SelectMonedas =
            "SELECT Pk_Moneda AS idMoneda, codigoMoneda, nombreMoneda, simboloMoneda " +
            "FROM tblMoneda WHERE is_active = 1 ORDER BY codigoMoneda";

        // Último tipo de cambio registrado por cada par de monedas hasta la fecha indicada.
        // Si en un mismo día hay varias versiones, se toma la versión más reciente.
        private const string _SelectVigentes =
            "SELECT tc.Pk_TipoCambio AS idTipoCambio, o.codigoMoneda AS codigoOrigen, o.simboloMoneda AS simboloOrigen, " +
            "       d.codigoMoneda AS codigoDestino, d.simboloMoneda AS simboloDestino, tc.valorTipoCambio, " +
            "       tc.fechaTipoCambio, tc.fuenteTipoCambio, tc.metodoTipoCambio, tc.versionTipoCambio " +
            "FROM tblTipoCambio tc " +
            "INNER JOIN tblMoneda o ON o.Pk_Moneda = tc.Fk_Moneda_Origen_Codigo " +
            "INNER JOIN tblMoneda d ON d.Pk_Moneda = tc.Fk_Moneda_Destino_Codigo " +
            "WHERE o.is_active = 1 AND d.is_active = 1 " +
            "  AND tc.Pk_TipoCambio = (SELECT t2.Pk_TipoCambio FROM tblTipoCambio t2 " +
            "                          WHERE t2.Fk_Moneda_Origen_Codigo = tc.Fk_Moneda_Origen_Codigo " +
            "                            AND t2.Fk_Moneda_Destino_Codigo = tc.Fk_Moneda_Destino_Codigo " +
            "                            AND t2.fechaTipoCambio <= ? " +
            "                          ORDER BY t2.fechaTipoCambio DESC, t2.versionTipoCambio DESC, t2.Pk_TipoCambio DESC " +
            "                          LIMIT 1) " +
            "ORDER BY o.codigoMoneda, d.codigoMoneda";

        private const string _SelectTipoCambioPar =
            "SELECT valorTipoCambio, fechaTipoCambio, fuenteTipoCambio " +
            "FROM tblTipoCambio " +
            "WHERE Fk_Moneda_Origen_Codigo = ? AND Fk_Moneda_Destino_Codigo = ? AND fechaTipoCambio <= ? " +
            "ORDER BY fechaTipoCambio DESC, versionTipoCambio DESC, Pk_TipoCambio DESC LIMIT 1";

        private const string _SelectHistorial =
            "SELECT fechaTipoCambio, valorTipoCambio, versionTipoCambio, fuenteTipoCambio " +
            "FROM tblTipoCambio " +
            "WHERE Fk_Moneda_Origen_Codigo = ? AND Fk_Moneda_Destino_Codigo = ? " +
            "  AND fechaTipoCambio BETWEEN ? AND ? " +
            "ORDER BY fechaTipoCambio, versionTipoCambio, Pk_TipoCambio";

        public DataTable ComprasFuncObtenerMonedasActivas()
        {
            return ComprasFuncEjecucionConsulta(_SelectMonedas, CommandType.Text);
        }

        public DataTable ComprasFuncObtenerTiposCambioVigentes(DateTime Fecha)
        {
            return ComprasFuncEjecucionConsulta(_SelectVigentes, CommandType.Text,
                new List<OdbcParameter> { ComprasFuncParametroFecha("pFecha", Fecha) });
        }

        // Devuelve una tabla con una fila (el tipo de cambio más reciente del par a la fecha) o vacía si no existe.
        public DataTable ComprasFuncObtenerTipoCambio(long IdMonedaOrigen, long IdMonedaDestino, DateTime Fecha)
        {
            return ComprasFuncEjecucionConsulta(_SelectTipoCambioPar, CommandType.Text, new List<OdbcParameter>
            {
                ComprasFuncParametroMoneda("pOrigen", IdMonedaOrigen),
                ComprasFuncParametroMoneda("pDestino", IdMonedaDestino),
                ComprasFuncParametroFecha("pFecha", Fecha)
            });
        }

        public DataTable ComprasFuncObtenerHistorial(long IdMonedaOrigen, long IdMonedaDestino, DateTime Desde, DateTime Hasta)
        {
            return ComprasFuncEjecucionConsulta(_SelectHistorial, CommandType.Text, new List<OdbcParameter>
            {
                ComprasFuncParametroMoneda("pOrigen", IdMonedaOrigen),
                ComprasFuncParametroMoneda("pDestino", IdMonedaDestino),
                ComprasFuncParametroFecha("pDesde", Desde),
                ComprasFuncParametroFecha("pHasta", Hasta)
            });
        }

        private static OdbcParameter ComprasFuncParametroMoneda(string Nombre, long Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.BigInt) { Value = Valor };
        }

        private static OdbcParameter ComprasFuncParametroFecha(string Nombre, DateTime Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.Date) { Value = Valor.Date };
        }
    }
}
