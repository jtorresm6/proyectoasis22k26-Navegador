using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Entidades;

namespace CapaModelo_MovimientosBancarios.Repositorio
{
    public class ClsRepositorioCuentaBancariaEmpresa : ClsSentencias, IRepositorioCuentaBancariaEmpresa
    {
        // Obtener cuentas empresariales
        public IEnumerable<ClsCuentaBancariaEmpresa>
            BancosMetObtenerCuentasEmpresa()
        {
            string Consulta = @"
                SELECT
                    c.idCuentaBancaria AS ID,
                    c.numeroCuentaBancaria AS Numero,
                    c.nombreCuentaBancaria AS Nombre,
                    c.saldoInicialCuentaBancaria AS SaldoInicial,
                    c.limiteSobregiroCuentaBancaria AS Sobregiro,
                    b.nombreBanco AS Banco,
                    m.codigoMoneda AS Moneda
                FROM tblCuentaBancaria c
                INNER JOIN tblBanco b
                    ON c.Fk_Banco_Codigo = b.idBanco
                INNER JOIN tblMoneda m
                    ON c.Fk_Moneda_Codigo = m.idMoneda
                WHERE c.is_active = 1
                ORDER BY b.nombreBanco, c.nombreCuentaBancaria;";

            DataTable Tabla = BancosMetEjecucionConsulta(Consulta, CommandType.Text);

            List<ClsCuentaBancariaEmpresa> Cuentas = new List<ClsCuentaBancariaEmpresa>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                Cuentas.Add(new ClsCuentaBancariaEmpresa
                {
                    IdCuentaBancaria = Convert.ToInt64(Fila["ID"]),
                    NumeroCuentaBancaria = Convert.ToString(Fila["Numero"]),
                    NombreCuentaBancaria = Convert.ToString(Fila["Nombre"]),
                    SaldoInicialCuentaBancaria = Convert.ToDecimal(Fila["SaldoInicial"]),
                    LimiteSobregiroCuentaBancaria = Convert.ToDecimal(Fila["Sobregiro"]),
                    NombreBanco = Convert.ToString(Fila["Banco"]),
                    CodigoMoneda = Convert.ToString(Fila["Moneda"])
                });
            }

            return Cuentas;
        }

        // Calcular saldo de la cuenta empresarial
        public decimal BancosMetObtenerSaldoDisponible(
            long IdCuentaBancaria)
        {
            if (IdCuentaBancaria <= 0)
                throw new ArgumentException(
                    "El identificador de la cuenta no es válido.");

            string Consulta = @"
                SELECT
                    c.saldoInicialCuentaBancaria +
                    COALESCE(
                        SUM(
                            m.importeMovimientoBancario *
                            m.sentidoMovimientoBancario
                        ), 0
                    ) AS SaldoDisponible
                FROM tblCuentaBancaria c
                LEFT JOIN tblMovimientoBancario m
                    ON m.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria
                WHERE c.idCuentaBancaria = ?
                  AND c.is_active = 1
                GROUP BY
                    c.idCuentaBancaria,
                    c.saldoInicialCuentaBancaria;";

            List<OdbcParameter> Parametros =
                new List<OdbcParameter>
                {
                    new OdbcParameter
                    {
                        OdbcType = OdbcType.BigInt,
                        Value = IdCuentaBancaria
                    }
                };

            DataTable Tabla = BancosMetEjecucionConsulta(
                Consulta,
                CommandType.Text,
                Parametros
            );

            if (Tabla.Rows.Count == 0)
                throw new InvalidOperationException(
                    "La cuenta bancaria no existe o está inactiva.");

            return Convert.ToDecimal(
                Tabla.Rows[0]["SaldoDisponible"]);
        }

        // Métodos heredados del repositorio genérico
        public IEnumerable<ClsCuentaBancariaEmpresa> BancosMetObtenerTodos()
        {
            return BancosMetObtenerCuentasEmpresa();
        }

        public int BancosMetAgregar(ClsCuentaBancariaEmpresa Entidad)
        {
            throw new NotSupportedException(
                "Las cuentas bancarias se registran desde su módulo.");
        }

        public int BancosMetEditar(ClsCuentaBancariaEmpresa Entidad)
        {
            throw new NotSupportedException(
                "Este módulo no modifica cuentas empresariales.");
        }

        public int BancosMetRemover(ClsCuentaBancariaEmpresa Entidad)
        {
            throw new NotSupportedException(
                "Este módulo no elimina cuentas empresariales.");
        }
    }
}
