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
            _SelectAll = "SELECT c.idCuentaBancaria, b.nombreBanco, c.numeroCuentaBancaria, " +
                         "c.Fk_Moneda_Codigo, m.codigoMoneda, " +
                         "(d.saldoInicialDisponibilidadDiaria + d.ingresosDisponibilidadDiaria - " +
                         " d.egresosDisponibilidadDiaria - d.restringidoDisponibilidadDiaria - " +
                         " COALESCE((SELECT SUM(ap.montoAsignadoAutPagoOrden) FROM tblAutPagoOrden ap " +
                         " JOIN tblAutDisponibilidadFondos af ON af.idAutDisponibilidadFondos = ap.Fk_AutDisponibilidadFondos_Codigo " +
                         " WHERE af.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria " +
                         " AND UPPER(ap.estadoAutPagoOrden) IN ('RESERVADA','RESERVADO') " +
                         " AND NOT EXISTS (SELECT 1 FROM tblMovimientoBancario mb WHERE mb.Fk_OrdenPago_Codigo = ap.Fk_OrdenPago_Codigo)), 0) - " +
                         " COALESCE((SELECT SUM(op.importeOrdenPago) FROM tblOrdenPago op " +
                         " WHERE op.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria " +
                         " AND UPPER(op.estadoOrdenPago) IN ('AUTORIZADA','EMITIDA') " +
                         " AND NOT EXISTS (SELECT 1 FROM tblMovimientoBancario mb2 WHERE mb2.Fk_OrdenPago_Codigo = op.idOrdenPago) " +
                         " AND NOT EXISTS (SELECT 1 FROM tblAutPagoOrden ap2 WHERE ap2.Fk_OrdenPago_Codigo = op.idOrdenPago " +
                         " AND UPPER(ap2.estadoAutPagoOrden) IN ('RESERVADA','RESERVADO'))), 0)) AS saldoDisponible, " +
                         "d.Fk_TipoCambio_Codigo, d.tipoCambioAplicadoDisponibilidadDiaria, " +
                         "COALESCE((SELECT SUM(ap3.montoAsignadoAutPagoOrden) FROM tblAutPagoOrden ap3 " +
                         " JOIN tblAutDisponibilidadFondos af3 ON af3.idAutDisponibilidadFondos = ap3.Fk_AutDisponibilidadFondos_Codigo " +
                         " WHERE af3.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria " +
                         " AND UPPER(ap3.estadoAutPagoOrden) IN ('RESERVADA','RESERVADO') " +
                         " AND NOT EXISTS (SELECT 1 FROM tblMovimientoBancario mb3 WHERE mb3.Fk_OrdenPago_Codigo = ap3.Fk_OrdenPago_Codigo)), 0) + " +
                         "COALESCE((SELECT SUM(op3.importeOrdenPago) FROM tblOrdenPago op3 " +
                         " WHERE op3.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria " +
                         " AND UPPER(op3.estadoOrdenPago) IN ('AUTORIZADA','EMITIDA') " +
                         " AND NOT EXISTS (SELECT 1 FROM tblMovimientoBancario mb4 WHERE mb4.Fk_OrdenPago_Codigo = op3.idOrdenPago) " +
                         " AND NOT EXISTS (SELECT 1 FROM tblAutPagoOrden ap4 WHERE ap4.Fk_OrdenPago_Codigo = op3.idOrdenPago " +
                         " AND UPPER(ap4.estadoAutPagoOrden) IN ('RESERVADA','RESERVADO'))), 0) AS comprometidoActual " +
                         "FROM tblCuentaBancaria c " +
                         "INNER JOIN tblBanco b ON b.idBanco = c.Fk_Banco_Codigo " +
                         "INNER JOIN tblMoneda m ON m.idMoneda = c.Fk_Moneda_Codigo " +
                         "INNER JOIN tblDisponibilidadDiaria d ON d.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria " +
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
