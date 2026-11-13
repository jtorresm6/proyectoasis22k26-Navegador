using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==========================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 13/11/2026
 * ==========================================================================
 * Propósito :
 *  La clase Repositorio de Tipo de Cambio de Autorización guarda en tblTipoCambio
 *  la tasa consultada al módulo de Tipo de Cambio, para que la evidencia de fondos
 *  de una cuenta en otra moneda tenga su identificador. Si la tasa ya existe la
 *  reutiliza; si la fecha ya tiene otro valor crea una versión nueva. Hereda de
 *  ClsSentencias para ejecutar los comandos por ODBC.
 * ==========================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Repositorios
{
    public class ClsRepositorioTipoCambioAutorizacion : ClsSentencias
    {
        private const string SelectExistente =
            "SELECT idTipoCambio FROM tblTipoCambio " +
            "WHERE Fk_Moneda_Origen_Codigo = ? AND Fk_Moneda_Destino_Codigo = ? " +
            "AND fechaTipoCambio = '{0}' AND fuenteTipoCambio = ? AND valorTipoCambio = {1} " +
            "ORDER BY versionTipoCambio DESC LIMIT 1";

        private const string SelectSiguienteVersion =
            "SELECT COALESCE(MAX(versionTipoCambio), 0) + 1 FROM tblTipoCambio " +
            "WHERE Fk_Moneda_Origen_Codigo = ? AND Fk_Moneda_Destino_Codigo = ? " +
            "AND fechaTipoCambio = '{0}' AND fuenteTipoCambio = ?";

        private const string InsertTipoCambio =
            "INSERT INTO tblTipoCambio (fechaTipoCambio, valorTipoCambio, fuenteTipoCambio, " +
            "versionTipoCambio, metodoTipoCambio, Fk_Moneda_Origen_Codigo, Fk_Moneda_Destino_Codigo) " +
            "VALUES ('{0}', {1}, ?, {2}, ?, ?, ?)";

        private const string SelectPorVersion =
            "SELECT idTipoCambio FROM tblTipoCambio " +
            "WHERE Fk_Moneda_Origen_Codigo = ? AND Fk_Moneda_Destino_Codigo = ? " +
            "AND fechaTipoCambio = '{0}' AND fuenteTipoCambio = ? AND versionTipoCambio = {1}";

        public long BancosMetGuardarTipoCambio(DateTime Fecha, decimal Valor, string Fuente, string Metodo,
            long IdMonedaOrigen, long IdMonedaDestino)
        {
            Fecha = Fecha.Date;
            Valor = Math.Round(Valor, 4, MidpointRounding.AwayFromZero);

            if (Valor <= 0m)
            {
                throw new InvalidOperationException("El tipo de cambio debe ser mayor que cero.");
            }
            if (IdMonedaOrigen == IdMonedaDestino)
            {
                throw new InvalidOperationException("Las monedas del tipo de cambio deben ser distintas.");
            }
            if (string.IsNullOrWhiteSpace(Fuente))
            {
                Fuente = "Banco de Guatemala";
            }

            // Fecha y valor se escriben como literales ya validados (no son texto del usuario);
            // el ODBC de MySQL falla al enlazar parámetros decimal/fecha sin precisión.
            string FechaTexto = Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string ValorTexto = Valor.ToString("0.0000", CultureInfo.InvariantCulture);

            object Existente = BancosMetPrimerValor(string.Format(SelectExistente, FechaTexto, ValorTexto),
                new List<OdbcParameter>
                {
                    BancosMetNumero(IdMonedaOrigen), BancosMetNumero(IdMonedaDestino), BancosMetTexto(Fuente, 100)
                });
            if (Existente != null)
            {
                return Convert.ToInt64(Existente);
            }

            int Version = Convert.ToInt32(BancosMetPrimerValor(string.Format(SelectSiguienteVersion, FechaTexto),
                new List<OdbcParameter>
                {
                    BancosMetNumero(IdMonedaOrigen), BancosMetNumero(IdMonedaDestino), BancosMetTexto(Fuente, 100)
                }));
            string VersionTexto = Version.ToString(CultureInfo.InvariantCulture);

            BancosMetEjecucionNonQuery(string.Format(InsertTipoCambio, FechaTexto, ValorTexto, VersionTexto),
                new List<OdbcParameter>
                {
                    BancosMetTexto(Fuente, 100), BancosMetTexto(Metodo, 30),
                    BancosMetNumero(IdMonedaOrigen), BancosMetNumero(IdMonedaDestino)
                }, CommandType.Text);

            object IdNuevo = BancosMetPrimerValor(string.Format(SelectPorVersion, FechaTexto, VersionTexto),
                new List<OdbcParameter>
                {
                    BancosMetNumero(IdMonedaOrigen), BancosMetNumero(IdMonedaDestino), BancosMetTexto(Fuente, 100)
                });
            if (IdNuevo == null)
            {
                throw new InvalidOperationException("No se pudo registrar el tipo de cambio.");
            }
            return Convert.ToInt64(IdNuevo);
        }

        private object BancosMetPrimerValor(string Sql, List<OdbcParameter> Parametros)
        {
            DataTable Tabla = BancosMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            if (Tabla.Rows.Count == 0 || Tabla.Rows[0][0] == DBNull.Value)
            {
                return null;
            }
            return Tabla.Rows[0][0];
        }

        // Los parámetros se envían sin tipo explícito, igual que en el resto del módulo.
        private static OdbcParameter BancosMetNumero(long Valor)
        {
            return new OdbcParameter { Value = Valor };
        }

        private static OdbcParameter BancosMetFecha(DateTime Valor)
        {
            return new OdbcParameter { Value = Valor };
        }

        private static OdbcParameter BancosMetTexto(string Valor, int Longitud)
        {
            return new OdbcParameter { Value = Valor };
        }

        private static OdbcParameter BancosMetDecimal(decimal Valor)
        {
            return new OdbcParameter { Value = Valor };
        }
    }
}
