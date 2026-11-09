/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Repositorio;

namespace CapaControlador_MovimientosBancarios.Modelo_de_Controladores
{
    public class ClsModeloBeneficiario
    {
        private readonly IRepositorioBeneficiario _Repositorio;
        public ClsModeloBeneficiario()
        {
            _Repositorio = new ClsRepositorioBeneficiario();
        }

        public DataTable BancosMetObtenerBeneficiariosTabla(string TipoBeneficiario)
        {
            var Beneficiarios = _Repositorio.BancosMetObtenerPorTipo(TipoBeneficiario);

            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("ID", typeof(long));
            Tabla.Columns.Add("Codigo", typeof(string));
            Tabla.Columns.Add("Nombre", typeof(string));

            foreach (var Beneficiario in Beneficiarios)
            {
                Tabla.Rows.Add(Beneficiario.IdBeneficiario,Beneficiario.CodigoBeneficiario,Beneficiario.NombreBeneficiario);
            }
            return Tabla;
        }


        public DataTable BancosMetObtenerCuentasTerceroTabla(string TipoBeneficiario, long IdBeneficiario)
        {
            var Cuentas = _Repositorio.BancosMetObtenerCuentasPorBeneficiario(TipoBeneficiario, IdBeneficiario);

            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("ID", typeof(long));
            Tabla.Columns.Add("Numero", typeof(string));
            Tabla.Columns.Add("Titular", typeof(string));
            Tabla.Columns.Add("Banco", typeof(string));
            Tabla.Columns.Add("Moneda", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));

            foreach (var Cuenta in Cuentas)
            {
                Tabla.Rows.Add(
                    Cuenta.IdCuentaBancariaTercero,
                    Cuenta.NumeroCuentaBancariaTercero,
                    Cuenta.NombreTitularCuentaBancariaTercero,
                    Cuenta.NombreBanco,
                    Cuenta.CodigoMoneda,
                    Cuenta.DescripcionCuenta
                );
            }
            return Tabla;
        }
    }
}
