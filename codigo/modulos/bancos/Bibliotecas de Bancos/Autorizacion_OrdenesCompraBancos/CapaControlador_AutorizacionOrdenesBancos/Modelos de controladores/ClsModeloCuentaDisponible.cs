using CapaModelo_AutorizacionOrdenes.Entidades;
using CapaModelo_AutorizacionOrdenes.Repositorios;
using System;
using System.Collections.Generic;
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
 *  La clase Modelo de Cuenta Disponible pertenece a la capa controlador. Se
 *  encarga de obtener, desde el repositorio, las cuentas bancarias activas con
 *  su saldo disponible del día y de entregarlas a la vista para la asignación
 *  de pago de una orden de compra.
 * ===================================================================
*/

namespace CapaControlador_AutorizacionOrdenes.Modelos_de_controladores
{
    public class ClsModeloCuentaDisponible
    {
        private long _IdCuenta;
        private string _NombreBanco;
        private string _NumeroCuenta;
        private long _IdMoneda;
        private string _CodigoMoneda;
        private decimal _SaldoDisponible;
        private long? _IdTipoCambio;
        private decimal _TipoCambio;
        private ClsRepositorioCuentaDisponible _RepositorioCuentaDisponible;

        public long IdCuenta { get => _IdCuenta; private set => _IdCuenta = value; }

        public string NombreBanco { get => _NombreBanco; private set => _NombreBanco = value; }

        public string NombreCuentaCompleto { get => _NombreBanco + " - " + _NumeroCuenta; }
        public string NumeroCuenta { get => _NumeroCuenta; private set => _NumeroCuenta = value; }

        public long IdMoneda { get => _IdMoneda; private set => _IdMoneda = value; }

        public string CodigoMoneda { get => _CodigoMoneda; private set => _CodigoMoneda = value; }

        public decimal SaldoDisponible { get => _SaldoDisponible; private set => _SaldoDisponible = value; }

        public long? IdTipoCambio { get => _IdTipoCambio; private set => _IdTipoCambio = value; }

        public decimal TipoCambio { get => _TipoCambio; private set => _TipoCambio = value; }

        private List<ClsModeloCuentaDisponible> _ListaCuentaDisponible;
        public ClsModeloCuentaDisponible()
        {
            _RepositorioCuentaDisponible = new ClsRepositorioCuentaDisponible();
        }

        public List<ClsModeloCuentaDisponible> BancosMetObtenerTodos()
        {
            var DatosCuentas = _RepositorioCuentaDisponible.BancosMetObtenerTodos();
            _ListaCuentaDisponible = new List<ClsModeloCuentaDisponible>();
            foreach (ClsCuentaDisponible Item in DatosCuentas)
            {
                _ListaCuentaDisponible.Add(new ClsModeloCuentaDisponible
                {
                    _IdCuenta = Item.IdCuenta,
                    _NombreBanco = Item.NombreBanco,
                    _NumeroCuenta = Item.NumeroCuenta,
                    _IdMoneda = Item.IdMoneda,
                    _CodigoMoneda = Item.CodigoMoneda,
                    _SaldoDisponible = Item.SaldoDisponible,
                    _IdTipoCambio = Item.IdTipoCambio,
                    _TipoCambio = Item.TipoCambio,
                });
            }
            return _ListaCuentaDisponible;
        }

        public ClsModeloCuentaDisponible BancosMetBuscarPorId(long IdCuenta)
        {
            return _ListaCuentaDisponible.Find(e => e._IdCuenta == IdCuenta);
        }
    }
}
