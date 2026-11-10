/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Repositorio;

namespace CapaControlador_MovimientosBancarios.Modelo_de_Controladores
{
    public class ClsModeloCuentaBancariaEmpresa
    {
        private readonly IRepositorioCuentaBancariaEmpresa _Repositorio;

        public ClsModeloCuentaBancariaEmpresa()
        {
            _Repositorio = new ClsRepositorioCuentaBancariaEmpresa();
        }

        public DataTable BancosMetObtenerCuentasEmpresaTabla()
        {
            var Cuentas = _Repositorio.BancosMetObtenerCuentasEmpresa();

            DataTable Tabla = new DataTable();

            Tabla.Columns.Add("ID", typeof(long));
            Tabla.Columns.Add("Descripcion", typeof(string));
            Tabla.Columns.Add("Moneda", typeof(string));

            foreach (var Cuenta in Cuentas)
            {
                Tabla.Rows.Add(Cuenta.IdCuentaBancaria, Cuenta.DescripcionCuenta,Cuenta.CodigoMoneda);
            }

            return Tabla;
        }

        public decimal BancosMetObtenerSaldoDisponible(
            long IdCuentaBancaria)
        {
            if (IdCuentaBancaria <= 0)
            {
                throw new ArgumentException("Debe seleccionar una cuenta bancaria válida.");
            }
            return _Repositorio.BancosMetObtenerSaldoDisponible(IdCuentaBancaria);
        }
    }
}
