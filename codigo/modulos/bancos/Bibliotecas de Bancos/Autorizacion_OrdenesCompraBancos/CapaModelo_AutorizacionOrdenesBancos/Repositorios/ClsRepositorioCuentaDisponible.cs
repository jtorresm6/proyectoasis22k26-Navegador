using CapaModelo_AutorizacionOrdenes.Contratos;
using CapaModelo_AutorizacionOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  La clase Repositorio de Cuenta Disponible se encarga de consultar mediante
 *  ODBC las cuentas bancarias activas con disponibilidad del día, heredando de
 *  ClsSentencias e implementando la interfaz IRepositorioCuentaDisponible.
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Repositorios
{
    public class ClsRepositorioCuentaDisponible : ClsSentencias, IRepositorioCuentaDisponible
    {
        private string _SelectAll;


        public ClsRepositorioCuentaDisponible()
        {
            _SelectAll = "SELECT c.Pk_CuentaBancaria, b.nombreBanco, c.numeroCuentaBancaria, " +
                         "c.Fk_Moneda_Codigo, m.codigoMoneda, d.saldoDisponibleDisponibilidadDiaria, " +
                         "d.Fk_TipoCambio_Codigo, d.tipoCambioAplicadoDisponibilidadDiaria, " +
                         "d.comprometidoDisponibilidadDiaria " +
                         "FROM tblCuentaBancaria c " +
                         "INNER JOIN tblBanco b ON b.Pk_Banco = c.Fk_Banco_Codigo " +
                         "INNER JOIN tblMoneda m ON m.Pk_Moneda = c.Fk_Moneda_Codigo " +
                         "INNER JOIN tblDisponibilidadDiaria d ON d.Fk_CuentaBancaria_Codigo = c.Pk_CuentaBancaria " +
                         "WHERE c.is_active = 1 AND d.fechaDisponibilidadDiaria = CURDATE() " +
                         "ORDER BY b.nombreBanco, c.numeroCuentaBancaria";
        }

        public int BancosMetAgregar(ClsCuentaDisponible Entidad)
        {
            throw new NotSupportedException("Las cuentas las administra el módulo de maestros.");
        }

        public int BancosMetEditar(ClsCuentaDisponible Entidad)
        {
            throw new NotSupportedException("Las cuentas las administra el módulo de maestros.");
        }

        public int BancosMetRemover(ClsCuentaDisponible Entidad)
        {
            throw new NotSupportedException("Las cuentas las administra el módulo de maestros.");
        }
        public IEnumerable<ClsCuentaDisponible> BancosMetObtenerTodos()
        {
            var Lista = new List<ClsCuentaDisponible>();
            var TablaDatos = BancosMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Row in TablaDatos.Rows)
            {
                var Cuenta = new ClsCuentaDisponible();
                Cuenta.IdCuenta = Convert.ToInt64(Row[0]);
                Cuenta.NombreBanco = Convert.ToString(Row[1]);
                Cuenta.NumeroCuenta = Convert.ToString(Row[2]);
                Cuenta.IdMoneda = Convert.ToInt64(Row[3]);
                Cuenta.CodigoMoneda = Convert.ToString(Row[4]);
                Cuenta.SaldoDisponible = Convert.ToDecimal(Row[5]);
                Cuenta.IdTipoCambio = Row[6] == DBNull.Value ? (long?)null : Convert.ToInt64(Row[6]);
                Cuenta.TipoCambio = Convert.ToDecimal(Row[7]);
                Cuenta.MontoComprometido = Convert.ToDecimal(Row[8]);
                Lista.Add(Cuenta);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }
    }
}
